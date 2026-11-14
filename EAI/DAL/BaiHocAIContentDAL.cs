using System;
using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public enum AILeaseStatus
    {
        AlreadyCompleted,
        CurrentlyProcessing,
        LeaseAcquired
    }

    public class BaiHocAIContentDAL
    {
        private static BaiHocAIContentDAL? _instance;
        public static BaiHocAIContentDAL Instance => _instance ??= new BaiHocAIContentDAL();

        /// <summary>
        /// Lấy nội dung phân tích AI đã lưu trong DB nếu ContentHash khớp và trạng thái HoanThanh
        /// </summary>
        public BaiHocAIContentDTO? LayNoiDungAI(int maBaiHoc, string contentHash)
        {
            if (maBaiHoc < 0 || string.IsNullOrWhiteSpace(contentHash)) return null;

            try
            {
                const string query = @"
                    SELECT MaNoiDungAI, MaBaiHoc, ContentHash, TrangThai, KyNang, MucTieu, 
                           DuLieuKetQuaJson, MoHinhAI, ThongBaoLoi, ThoiDiemBatDauXuLy, NgayTao, NgayCapNhat
                    FROM NoiDungHocTapAIBaiHoc
                    WHERE MaBaiHoc = @MaBaiHoc AND ContentHash = @ContentHash AND TrangThai = N'HoanThanh';";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, new[]
                {
                    new SqlParameter("@MaBaiHoc", maBaiHoc),
                    new SqlParameter("@ContentHash", contentHash)
                });

                if (dt.Rows.Count > 0)
                {
                    return ChuyenDoiDataRowSangDTO(dt.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocAIContentDAL.LayNoiDungAI] Lỗi: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Cơ chế khóa chống gọi AI trùng lặp (Concurrency Safe Lease):
        /// - Nếu đã hoàn thành với ContentHash: trả về AlreadyCompleted kèm dữ liệu.
        /// - Nếu đang có tiến trình xử lý (chưa quá 60s timeout): trả về CurrentlyProcessing để client chờ.
        /// - Ngược lại: cấp quyền xử lý (LeaseAcquired) và đặt cờ DangXuLy trong DB.
        /// </summary>
        public (AILeaseStatus Status, BaiHocAIContentDTO? Data) KiemTraVaGiuKhoaXuLy(int maBaiHoc, string contentHash)
        {
            if (maBaiHoc < 0 || string.IsNullOrWhiteSpace(contentHash)) 
                return (AILeaseStatus.LeaseAcquired, null);

            try
            {
                const string atomicLeaseSql = @"
                    SET NOCOUNT ON;
                    BEGIN TRANSACTION;

                    DECLARE @Status NVARCHAR(30);
                    DECLARE @Hash NVARCHAR(64);
                    DECLARE @SecsDiff INT;

                    SELECT @Status = TrangThai, 
                           @Hash = ContentHash,
                           @SecsDiff = DATEDIFF(SECOND, ISNULL(ThoiDiemBatDauXuLy, SYSDATETIME()), SYSDATETIME())
                    FROM NoiDungHocTapAIBaiHoc WITH (UPDLOCK, HOLDLOCK)
                    WHERE MaBaiHoc = @MaBaiHoc;

                    IF @Status IS NULL
                    BEGIN
                        INSERT INTO NoiDungHocTapAIBaiHoc (MaBaiHoc, ContentHash, TrangThai, ThoiDiemBatDauXuLy, NgayTao, NgayCapNhat)
                        VALUES (@MaBaiHoc, @ContentHash, N'DangXuLy', SYSDATETIME(), SYSDATETIME(), SYSDATETIME());
                        COMMIT TRANSACTION;
                        SELECT 'LeaseAcquired' AS LeaseResult;
                    END
                    ELSE IF @Status = N'HoanThanh' AND @Hash = @ContentHash
                    BEGIN
                        COMMIT TRANSACTION;
                        SELECT 'AlreadyCompleted' AS LeaseResult;
                    END
                    ELSE IF @Status = N'DangXuLy' AND @SecsDiff < 60
                    BEGIN
                        COMMIT TRANSACTION;
                        SELECT 'CurrentlyProcessing' AS LeaseResult;
                    END
                    ELSE
                    BEGIN
                        UPDATE NoiDungHocTapAIBaiHoc
                        SET TrangThai = N'DangXuLy',
                            ContentHash = @ContentHash,
                            ThoiDiemBatDauXuLy = SYSDATETIME(),
                            ThongBaoLoi = NULL,
                            NgayCapNhat = SYSDATETIME()
                        WHERE MaBaiHoc = @MaBaiHoc;
                        COMMIT TRANSACTION;
                        SELECT 'LeaseAcquired' AS LeaseResult;
                    END";

                DataTable dt = DatabaseHelper.ExecuteQuery(atomicLeaseSql, new[]
                {
                    new SqlParameter("@MaBaiHoc", maBaiHoc),
                    new SqlParameter("@ContentHash", contentHash)
                });

                if (dt.Rows.Count > 0)
                {
                    string result = dt.Rows[0]["LeaseResult"]?.ToString() ?? "";
                    if (result == "AlreadyCompleted")
                    {
                        return (AILeaseStatus.AlreadyCompleted, LayNoiDungAI(maBaiHoc, contentHash));
                    }
                    if (result == "CurrentlyProcessing")
                    {
                        return (AILeaseStatus.CurrentlyProcessing, null);
                    }
                }

                return (AILeaseStatus.LeaseAcquired, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocAIContentDAL.KiemTraVaGiuKhoaXuLy] Lỗi: {ex.Message}");
                return (AILeaseStatus.LeaseAcquired, null);
            }
        }

        /// <summary>
        /// Lưu kết quả phân tích AI thành công vào DB
        /// </summary>
        public bool LuuKetQuaHoanThanh(BaiHocAIContentDTO dto)
        {
            if (dto == null || dto.MaBaiHoc < 0) return false;

            try
            {
                string json = JsonSerializer.Serialize(dto.DanhSachTheHoc, new JsonSerializerOptions
                {
                    WriteIndented = false,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                const string sql = @"
                    UPDATE NoiDungHocTapAIBaiHoc
                    SET TrangThai = N'HoanThanh',
                        ContentHash = @ContentHash,
                        KyNang = @KyNang,
                        MucTieu = @MucTieu,
                        DuLieuKetQuaJson = @Json,
                        MoHinhAI = @MoHinhAI,
                        ThongBaoLoi = NULL,
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaBaiHoc = @MaBaiHoc;";

                int rows = DatabaseHelper.ExecuteNonQuery(sql, new[]
                {
                    new SqlParameter("@MaBaiHoc", dto.MaBaiHoc),
                    new SqlParameter("@ContentHash", dto.ContentHash ?? ""),
                    new SqlParameter("@KyNang", (object?)dto.KyNang ?? DBNull.Value),
                    new SqlParameter("@MucTieu", (object?)dto.MucTieuBaiHoc ?? DBNull.Value),
                    new SqlParameter("@Json", json),
                    new SqlParameter("@MoHinhAI", (object?)dto.MoHinhAI ?? "Gemini")
                });

                return rows > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocAIContentDAL.LuuKetQuaHoanThanh] Lỗi: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Ghi nhận trạng thái thất bại và thông điệp lỗi
        /// </summary>
        public void LuuTrangThaiThatBai(int maBaiHoc, string errorMessage)
        {
            try
            {
                const string sql = @"
                    UPDATE NoiDungHocTapAIBaiHoc
                    SET TrangThai = N'ThatBai',
                        ThongBaoLoi = @ThongBaoLoi,
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaBaiHoc = @MaBaiHoc;";

                DatabaseHelper.ExecuteNonQuery(sql, new[]
                {
                    new SqlParameter("@MaBaiHoc", maBaiHoc),
                    new SqlParameter("@ThongBaoLoi", string.IsNullOrWhiteSpace(errorMessage) ? "Lỗi không xác định khi gọi AI" : errorMessage)
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocAIContentDAL.LuuTrangThaiThatBai] Lỗi: {ex.Message}");
            }
        }

        private static BaiHocAIContentDTO ChuyenDoiDataRowSangDTO(DataRow row)
        {
            var dto = new BaiHocAIContentDTO
            {
                MaNoiDungAI = Convert.ToInt32(row["MaNoiDungAI"]),
                MaBaiHoc = Convert.ToInt32(row["MaBaiHoc"]),
                ContentHash = row["ContentHash"]?.ToString() ?? "",
                TrangThai = row["TrangThai"]?.ToString() ?? "ChuaTao",
                KyNang = row["KyNang"]?.ToString(),
                MucTieuBaiHoc = row["MucTieu"]?.ToString(),
                MoHinhAI = row["MoHinhAI"]?.ToString(),
                ThongBaoLoi = row["ThongBaoLoi"]?.ToString(),
                ThoiDiemBatDauXuLy = row["ThoiDiemBatDauXuLy"] == DBNull.Value ? null : Convert.ToDateTime(row["ThoiDiemBatDauXuLy"]),
                NgayTao = row["NgayTao"] == DBNull.Value ? null : Convert.ToDateTime(row["NgayTao"]),
                NgayCapNhat = row["NgayCapNhat"] == DBNull.Value ? null : Convert.ToDateTime(row["NgayCapNhat"])
            };

            string json = row["DuLieuKetQuaJson"]?.ToString() ?? "";
            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    dto.DanhSachTheHoc = JsonSerializer.Deserialize<List<LearningCardDTO>>(json) ?? new List<LearningCardDTO>();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[BaiHocAIContentDAL] Lỗi deserialize thẻ học: {ex.Message}");
                    dto.DanhSachTheHoc = new List<LearningCardDTO>();
                }
            }

            return dto;
        }
    }
}
