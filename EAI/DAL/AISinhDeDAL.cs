using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class AISinhDeDAL
    {
        private static AISinhDeDAL? _instance;
        public static AISinhDeDAL Instance => _instance ??= new AISinhDeDAL();

        /// <summary>
        /// Lưu một yêu cầu sinh câu hỏi mới vào bảng YeuCauSinhDeAI
        /// </summary>
        public long LuuYeuCauSinhDe(int nguoiYeuCau, string loaiDauVao, string noiDungDauVao, int? maTrinhDo, int soLuong, string moHinhAI)
        {
            string sql = @"
                INSERT INTO YeuCauSinhDeAI (NguoiYeuCau, LoaiDauVao, NoiDungDauVao, MaTrinhDoMongMuon, SoLuongCauHoi, TrangThai, MoHinhAI, NgayTao, NgayHoanThanh)
                VALUES (@NguoiYeuCau, @LoaiDauVao, @NoiDung, @MaTrinhDo, @SoLuong, N'HoanThanh', @MoHinh, SYSDATETIME(), SYSDATETIME());
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

            var parameters = new[]
            {
                new SqlParameter("@NguoiYeuCau", nguoiYeuCau > 0 ? (object)nguoiYeuCau : 1),
                new SqlParameter("@LoaiDauVao", string.IsNullOrWhiteSpace(loaiDauVao) ? "ChuDe" : loaiDauVao),
                new SqlParameter("@NoiDung", (object?)noiDungDauVao ?? DBNull.Value),
                new SqlParameter("@MaTrinhDo", (object?)maTrinhDo ?? DBNull.Value),
                new SqlParameter("@SoLuong", soLuong),
                new SqlParameter("@MoHinh", (object?)moHinhAI ?? "Google Gemini")
            };

            object? res = SqlDataProvider.Instance.ExecuteScalar(sql, parameters);
            return res != null && res != DBNull.Value ? Convert.ToInt64(res) : 0;
        }

        /// <summary>
        /// Lưu từng câu hỏi sinh ra vào hàng đợi kiểm duyệt CauHoiDoAISinh
        /// </summary>
        public long LuuCauHoiDoAISinh(long maYeuCauSinhDe, string jsonCauHoi, string trangThaiDuyet = "DangCho")
        {
            string sql = @"
                INSERT INTO CauHoiDoAISinh (MaYeuCauSinhDe, NoiDungCauHoiSinhRa, TrangThaiDuyet)
                VALUES (@MaYeuCau, @NoiDung, @TrangThai);
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

            var parameters = new[]
            {
                new SqlParameter("@MaYeuCau", maYeuCauSinhDe),
                new SqlParameter("@NoiDung", jsonCauHoi ?? string.Empty),
                new SqlParameter("@TrangThai", trangThaiDuyet)
            };

            object? res = SqlDataProvider.Instance.ExecuteScalar(sql, parameters);
            return res != null && res != DBNull.Value ? Convert.ToInt64(res) : 0;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách câu hỏi đang chờ duyệt trong hệ thống từ SQL Server
        /// </summary>
        public List<CauHoiDoAISinhItemDTO> LayDanhSachCauHoiChoDuyet(string? tuKhoa = null, string? trangThai = "DangCho")
        {
            var list = new List<CauHoiDoAISinhItemDTO>();
            string sql = @"
                SELECT c.MaCauHoiAISinh, c.MaYeuCauSinhDe, c.MaCauHoi, c.NoiDungCauHoiSinhRa, c.TrangThaiDuyet, 
                       c.NguoiDuyet, c.NgayDuyet,
                       y.NoiDungDauVao, y.MoHinhAI, y.LoaiDauVao, y.NgayTao
                FROM CauHoiDoAISinh c
                INNER JOIN YeuCauSinhDeAI y ON c.MaYeuCauSinhDe = y.MaYeuCauSinhDe
                WHERE (@TrangThai IS NULL OR @TrangThai = 'TatCa' OR c.TrangThaiDuyet = @TrangThai)
                  AND (@TuKhoa IS NULL OR @TuKhoa = '' OR c.NoiDungCauHoiSinhRa LIKE N'%' + @TuKhoa + N'%' OR y.NoiDungDauVao LIKE N'%' + @TuKhoa + N'%')
                ORDER BY c.MaCauHoiAISinh DESC";

            var parameters = new[]
            {
                new SqlParameter("@TrangThai", string.IsNullOrWhiteSpace(trangThai) ? (object)DBNull.Value : trangThai.Trim()),
                new SqlParameter("@TuKhoa", string.IsNullOrWhiteSpace(tuKhoa) ? (object)DBNull.Value : tuKhoa.Trim())
            };

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CauHoiDoAISinhItemDTO
                {
                    MaCauHoiAISinh = Convert.ToInt64(row["MaCauHoiAISinh"]),
                    MaYeuCauSinhDe = Convert.ToInt64(row["MaYeuCauSinhDe"]),
                    MaCauHoi = row["MaCauHoi"] == DBNull.Value ? null : Convert.ToInt32(row["MaCauHoi"]),
                    NoiDungCauHoiSinhRa = row["NoiDungCauHoiSinhRa"]?.ToString() ?? "",
                    TrangThaiDuyet = row["TrangThaiDuyet"]?.ToString() ?? "DangCho",
                    NguoiDuyet = row["NguoiDuyet"] == DBNull.Value ? null : Convert.ToInt32(row["NguoiDuyet"]),
                    NgayDuyet = row["NgayDuyet"] == DBNull.Value ? null : Convert.ToDateTime(row["NgayDuyet"]),
                    NoiDungDauVao = row["NoiDungDauVao"]?.ToString() ?? "",
                    MoHinhAI = row["MoHinhAI"]?.ToString() ?? "Google Gemini",
                    LoaiDauVao = row["LoaiDauVao"]?.ToString() ?? "ChuDe",
                    NgayTao = row["NgayTao"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["NgayTao"])
                });
            }

            return list;
        }

        /// <summary>
        /// Đếm chính xác số lượng câu hỏi đang chờ duyệt từ SQL Server
        /// </summary>
        public int DemSoCauHoiChoDuyet()
        {
            string sql = "SELECT COUNT(*) FROM CauHoiDoAISinh WHERE TrangThaiDuyet = N'DangCho'";
            object? res = SqlDataProvider.Instance.ExecuteScalar(sql);
            return res != null && res != DBNull.Value ? Convert.ToInt32(res) : 0;
        }

        /// <summary>
        /// Cập nhật nội dung câu hỏi do AI tạo sau khi chỉnh sửa
        /// </summary>
        public bool CapNhatNoiDungCauHoiAISinh(long maCauHoiAISinh, string jsonMoi, string trangThai = "DaChinhSua")
        {
            string sql = @"
                UPDATE CauHoiDoAISinh
                SET NoiDungCauHoiSinhRa = @NoiDung,
                    TrangThaiDuyet = @TrangThai
                WHERE MaCauHoiAISinh = @Id";

            var parameters = new[]
            {
                new SqlParameter("@NoiDung", jsonMoi ?? string.Empty),
                new SqlParameter("@TrangThai", trangThai),
                new SqlParameter("@Id", maCauHoiAISinh)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(sql, parameters) > 0;
        }

        /// <summary>
        /// Phê duyệt một câu hỏi AI trong 1 transaction: ghi vào CauHoi, ghi LuaChonDapAn và cập nhật CauHoiDoAISinh
        /// </summary>
        public bool PheDuyetCauHoiAI(
            long maCauHoiAISinh, 
            CauHoiKhaoSatDTO cauHoi, 
            List<LuaChonDapAnDTO> dsDapAn, 
            int nguoiDuyetId, 
            string nguonTao, 
            out string errorMessage)
        {
            return PheDuyetCauHoiAI(maCauHoiAISinh, cauHoi, dsDapAn, nguoiDuyetId, nguonTao, out _, out errorMessage);
        }

        public bool PheDuyetCauHoiAI(
            long maCauHoiAISinh, 
            CauHoiKhaoSatDTO cauHoi, 
            List<LuaChonDapAnDTO> dsDapAn, 
            int nguoiDuyetId, 
            string nguonTao, 
            out int maCauHoiTao,
            out string errorMessage)
        {
            maCauHoiTao = 0;
            errorMessage = string.Empty;
            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                int? maTrinhDoId = cauHoi.MaTrinhDo switch
                {
                    "A1" => 1,
                    "A2" => 2,
                    "B1" => 3,
                    "B2" => 4,
                    "C1" => 5,
                    "C2" => 6,
                    _ => cauHoi.MaTrinhDoId ?? 3
                };

                // 1. Thêm vào bảng CauHoi
                string insertChSql = @"
                    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi, 
                                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet, NguoiTao, NguonTao, NgayTao)
                    VALUES (@MaChuDe, @MaTrinhDo, @LoaiCauHoi, @KyNang, @DoKho, @NoiDung, 
                            @DoanVanDoc, @AudioPath, @GiaiThich, @LoiGiai, 1, @NguoiTao, @NguonTao, SYSDATETIME());
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                int maCauHoi;
                using (var cmd = new SqlCommand(insertChSql, conn, trans))
                {
                    cmd.Parameters.AddWithValue("@MaChuDe", (object?)cauHoi.MaChuDe ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaTrinhDo", (object?)maTrinhDoId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LoaiCauHoi", string.IsNullOrWhiteSpace(cauHoi.LoaiCauHoi) ? "TracNghiem" : cauHoi.LoaiCauHoi);
                    cmd.Parameters.AddWithValue("@KyNang", string.IsNullOrWhiteSpace(cauHoi.KyNang) ? "Doc" : cauHoi.KyNang);
                    cmd.Parameters.AddWithValue("@DoKho", string.IsNullOrWhiteSpace(cauHoi.DoKho) ? "TrungBinh" : cauHoi.DoKho);
                    cmd.Parameters.AddWithValue("@NoiDung", cauHoi.NoiDungCauHoi ?? "");
                    cmd.Parameters.AddWithValue("@DoanVanDoc", (object?)cauHoi.DoanVanDoc ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AudioPath", (object?)cauHoi.AudioPath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GiaiThich", (object?)cauHoi.GiaiThichDapAn ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LoiGiai", (object?)cauHoi.GiaiThichDapAn ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NguoiTao", nguoiDuyetId > 0 ? (object)nguoiDuyetId : DBNull.Value);
                    cmd.Parameters.AddWithValue("@NguonTao", string.IsNullOrWhiteSpace(nguonTao) ? "AI" : nguonTao);

                    maCauHoi = (int)cmd.ExecuteScalar();
                }

                // 2. Thêm danh sách đáp án vào LuaChonDapAn
                string insertDaSql = @"
                    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung)
                    VALUES (@MaCauHoi, @KyHieu, @NoiDung, @LaDapAnDung);";

                foreach (var da in dsDapAn)
                {
                    string kyHieu = string.IsNullOrWhiteSpace(da.KyHieuLuaChon) ? "A" : da.KyHieuLuaChon.Trim();
                    if (kyHieu.Length > 5) kyHieu = kyHieu.Substring(0, 5);

                    using var cmdDa = new SqlCommand(insertDaSql, conn, trans);
                    cmdDa.Parameters.AddWithValue("@MaCauHoi", maCauHoi);
                    cmdDa.Parameters.AddWithValue("@KyHieu", kyHieu);
                    cmdDa.Parameters.AddWithValue("@NoiDung", da.NoiDungLuaChon ?? "");
                    cmdDa.Parameters.AddWithValue("@LaDapAnDung", da.LaDapAnDung);
                    cmdDa.ExecuteNonQuery();
                }

                // 3. Cập nhật trạng thái duyệt trong bảng CauHoiDoAISinh
                string updateQueueSql = @"
                    UPDATE CauHoiDoAISinh
                    SET MaCauHoi = @MaCauHoi,
                        TrangThaiDuyet = N'DaDuyet',
                        NguoiDuyet = @NguoiDuyet,
                        NgayDuyet = SYSDATETIME()
                    WHERE MaCauHoiAISinh = @Id";

                using (var cmdQueue = new SqlCommand(updateQueueSql, conn, trans))
                {
                    cmdQueue.Parameters.AddWithValue("@MaCauHoi", maCauHoi);
                    cmdQueue.Parameters.AddWithValue("@NguoiDuyet", nguoiDuyetId > 0 ? (object)nguoiDuyetId : DBNull.Value);
                    cmdQueue.Parameters.AddWithValue("@Id", maCauHoiAISinh);
                    cmdQueue.ExecuteNonQuery();
                }

                maCauHoiTao = maCauHoi;
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Từ chối một câu hỏi AI trong hàng đợi kiểm duyệt
        /// </summary>
        public bool TuChoiCauHoiAI(long maCauHoiAISinh, int nguoiDuyetId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                string sql = @"
                    UPDATE CauHoiDoAISinh
                    SET TrangThaiDuyet = N'TuChoi',
                        NguoiDuyet = @NguoiDuyet,
                        NgayDuyet = SYSDATETIME()
                    WHERE MaCauHoiAISinh = @Id";

                var parameters = new[]
                {
                    new SqlParameter("@NguoiDuyet", nguoiDuyetId > 0 ? (object)nguoiDuyetId : DBNull.Value),
                    new SqlParameter("@Id", maCauHoiAISinh)
                };

                return SqlDataProvider.Instance.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Ghi nhận tệp tin tải lên vào bảng TepTinNhapLieu
        /// </summary>
        public int LuuTepTinNhapLieu(int nguoiTaiLen, string tenTepTin, string dinhDang, string duongDan, int soDong, string trangThai = "HoanThanh", string? thongBaoLoi = null)
        {
            string sql = @"
                INSERT INTO TepTinNhapLieu (NguoiTaiLen, TenTepTin, DinhDangTep, DuongDanTep, TrangThaiNhap, SoDongDaNhap, SoCauHoiNhanDien, ThongBaoLoi, NgayTaiLen)
                VALUES (@NguoiTaiLen, @TenTepTin, @DinhDang, @DuongDan, @TrangThai, @SoDong, @SoDong, @ThongBaoLoi, SYSDATETIME());
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var parameters = new[]
            {
                new SqlParameter("@NguoiTaiLen", nguoiTaiLen > 0 ? (object)nguoiTaiLen : 1),
                new SqlParameter("@TenTepTin", tenTepTin),
                new SqlParameter("@DinhDang", dinhDang),
                new SqlParameter("@DuongDan", (object?)duongDan ?? DBNull.Value),
                new SqlParameter("@TrangThai", trangThai),
                new SqlParameter("@SoDong", soDong),
                new SqlParameter("@ThongBaoLoi", (object?)thongBaoLoi ?? DBNull.Value)
            };

            object? res = SqlDataProvider.Instance.ExecuteScalar(sql, parameters);
            return res != null && res != DBNull.Value ? Convert.ToInt32(res) : 0;
        }
    }
}
