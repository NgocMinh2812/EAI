using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class CauHoiDAL
    {
        private static CauHoiDAL? _instance;
        public static CauHoiDAL Instance => _instance ??= new CauHoiDAL();

        /// <summary>
        /// Nạp toàn bộ các lựa chọn (A, B, C, D) cho danh sách câu hỏi trong 1 truy vấn duy nhất (Tránh lỗi N+1 Query)
        /// </summary>
        public static void NapDanhSachLuaChon(List<CauHoiKhaoSatDTO> danhSach)
        {
            if (danhSach == null || danhSach.Count == 0) return;

            var ids = danhSach.Select(q => q.MaCauHoi).Where(id => id >= 0).Distinct().ToList();
            if (ids.Count == 0) return;

            try
            {
                string idList = string.Join(",", ids);
                string query = $@"
                    SELECT MaLuaChon, MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung
                    FROM LuaChonDapAn
                    WHERE MaCauHoi IN ({idList})
                    ORDER BY MaCauHoi ASC, KyHieuLuaChon ASC";

                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query);
                var lookup = new Dictionary<int, List<LuaChonDapAnDTO>>();

                foreach (DataRow row in dt.Rows)
                {
                    int maCauHoi = Convert.ToInt32(row["MaCauHoi"]);
                    if (!lookup.TryGetValue(maCauHoi, out var list))
                    {
                        list = new List<LuaChonDapAnDTO>();
                        lookup[maCauHoi] = list;
                    }

                    list.Add(new LuaChonDapAnDTO
                    {
                        MaLuaChon = Convert.ToInt32(row["MaLuaChon"]),
                        MaCauHoi = maCauHoi,
                        KyHieuLuaChon = row["KyHieuLuaChon"]?.ToString() ?? "",
                        NoiDungLuaChon = CauHoiDTO.StripOptionPrefix(row["NoiDungLuaChon"]?.ToString() ?? ""),
                        LaDapAnDung = Convert.ToBoolean(row["LaDapAnDung"])
                    });
                }

                foreach (var q in danhSach)
                {
                    if (lookup.TryGetValue(q.MaCauHoi, out var options))
                    {
                        q.DanhSachLuaChon.Clear();
                        q.DanhSachLuaChon.AddRange(options);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CauHoiDAL.NapDanhSachLuaChon] {ex.Message}");
            }
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiKhaoSat(int maBaiKhaoSat = 1)
        {
            List<CauHoiKhaoSatDTO> danhSach = new List<CauHoiKhaoSatDTO>();

            string queryCauHoi = @"
                SELECT ch.MaCauHoi, chk.ThuTuCauHoi, ch.KyNang, td.MaCapDo AS MaTrinhDo, 
                       td.TenTrinhDo, ch.NoiDungCauHoi, ch.DoanVanDoc, ch.GiaiThichDapAn
                FROM CauHoiKhaoSat chk
                INNER JOIN CauHoi ch ON chk.MaCauHoi = ch.MaCauHoi
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                WHERE chk.MaBaiKhaoSat = @MaBai
                ORDER BY chk.ThuTuCauHoi ASC";

            DataTable dtCauHoi = SqlDataProvider.Instance.ExecuteQuery(queryCauHoi, new[] { new SqlParameter("@MaBai", maBaiKhaoSat) });

            if (dtCauHoi.Rows.Count == 0)
            {
                // Fallback nếu chưa gán vào bảng CauHoiKhaoSat: lấy trực tiếp 15 câu đã kiểm duyệt từ CauHoi
                string fallbackQuery = @"
                    SELECT TOP 15 ch.MaCauHoi, ROW_NUMBER() OVER(ORDER BY ch.MaCauHoi) AS ThuTuCauHoi, 
                           ch.KyNang, td.MaCapDo AS MaTrinhDo, td.TenTrinhDo, ch.NoiDungCauHoi, 
                           ch.DoanVanDoc, ch.GiaiThichDapAn
                    FROM CauHoi ch
                    LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                    WHERE ch.DaKiemDuyet = 1
                    ORDER BY ch.MaCauHoi ASC";
                dtCauHoi = SqlDataProvider.Instance.ExecuteQuery(fallbackQuery);
            }

            foreach (DataRow row in dtCauHoi.Rows)
            {
                danhSach.Add(new CauHoiKhaoSatDTO
                {
                    MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                    ThuTuCauHoi = Convert.ToInt32(row["ThuTuCauHoi"]),
                    KyNang = row["KyNang"]?.ToString() ?? "NguPhap",
                    MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "A1",
                    TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                    NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                    DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                    GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString()
                });
            }

            // Nạp toàn bộ lựa chọn trong 1 truy vấn duy nhất
            NapDanhSachLuaChon(danhSach);

            return danhSach;
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiTheoTrinhDo(string maCapDo, int soLuong = 10)
        {
            List<CauHoiKhaoSatDTO> danhSach = new List<CauHoiKhaoSatDTO>();

            string query = @"
                SELECT TOP (@Top) ch.MaCauHoi, ROW_NUMBER() OVER(ORDER BY ch.MaCauHoi) AS ThuTuCauHoi, 
                       ch.KyNang, td.MaCapDo AS MaTrinhDo, td.TenTrinhDo, ch.NoiDungCauHoi, 
                       ch.DoanVanDoc, ch.GiaiThichDapAn
                FROM CauHoi ch
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                WHERE ch.DaKiemDuyet = 1 
                  AND (@MaCapDo = 'TongHop' OR @MaCapDo = 'ALL' OR @MaCapDo = 'TatCa' OR td.MaCapDo = @MaCapDo)
                ORDER BY ch.MaCauHoi ASC";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaCapDo", maCapDo),
                new SqlParameter("@Top", soLuong)
            };

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);

            int thuTu = 1;
            foreach (DataRow row in dt.Rows)
            {
                danhSach.Add(new CauHoiKhaoSatDTO
                {
                    MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                    ThuTuCauHoi = thuTu++,
                    KyNang = row["KyNang"]?.ToString() ?? "NguPhap",
                    MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? maCapDo,
                    TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                    NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                    DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                    GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString()
                });
            }

            // Nạp toàn bộ lựa chọn trong 1 truy vấn duy nhất
            NapDanhSachLuaChon(danhSach);

            return danhSach;
        }

        public int LuuLuotKhaoSat(int maNguoiDung, int maBaiKhaoSat, int tongSoCau, int soCauDung, decimal diem, string trinhDo)
        {
            string query = @"
                DECLARE @MaBaiActual INT = (SELECT TOP 1 MaBaiKhaoSat FROM BaiKhaoSatDauVao WHERE MaBaiKhaoSat = @MaBai);
                IF @MaBaiActual IS NULL
                BEGIN
                    SET @MaBaiActual = (SELECT TOP 1 MaBaiKhaoSat FROM BaiKhaoSatDauVao);
                END
                IF @MaBaiActual IS NULL
                BEGIN
                    INSERT INTO BaiKhaoSatDauVao (TenBaiKhaoSat, MoTa, TongSoCauHoi, DangHoatDong, NgayTao)
                    VALUES (N'Khảo sát đánh giá năng lực đầu vào CEFR', N'Bài khảo sát phân loại trình độ từ A1 đến C1', 15, 1, SYSDATETIME());
                    SET @MaBaiActual = SCOPE_IDENTITY();
                END

                DECLARE @MaTD INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = @TrinhDo);
                
                INSERT INTO LuotKhaoSat (MaNguoiDung, MaBaiKhaoSat, ThoiDiemBatDau, ThoiDiemNopBai, TongSoCau, SoCauDung, DiemDatDuoc, MaTrinhDoKetQua, KhungDanhGiaKetQua)
                VALUES (@MaND, @MaBaiActual, DATEADD(MINUTE, -15, SYSDATETIME()), SYSDATETIME(), @TongSo, @SoDung, @Diem, @MaTD, N'CEFR');
                
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@MaBai", maBaiKhaoSat),
                new SqlParameter("@TongSo", tongSoCau),
                new SqlParameter("@SoDung", soCauDung),
                new SqlParameter("@Diem", diem),
                new SqlParameter("@TrinhDo", trinhDo)
            };

            object? id = SqlDataProvider.Instance.ExecuteScalar(query, parameters);
            return Convert.ToInt32(id);
        }

        public void LuuChiTietCauTraLoi(int maLuotKhaoSat, int maCauHoi, int? maLuaChon, bool laDung)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM CauHoi WHERE MaCauHoi = @MaCauHoi)
                BEGIN
                    DECLARE @MaLC INT = @MaLuaChon;
                    IF @MaLC IS NOT NULL AND NOT EXISTS (SELECT 1 FROM LuaChonDapAn WHERE MaLuaChon = @MaLC)
                        SET @MaLC = NULL;

                    INSERT INTO CauTraLoiKhaoSat (MaLuotKhaoSat, MaCauHoi, MaLuaChonDaChon, KetQuaDungSai, ThoiDiemTraLoi)
                    VALUES (@MaLuot, @MaCauHoi, @MaLC, @KetQua, SYSDATETIME());
                END";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaLuot", maLuotKhaoSat),
                new SqlParameter("@MaCauHoi", maCauHoi),
                new SqlParameter("@MaLuaChon", (object?)maLuaChon ?? DBNull.Value),
                new SqlParameter("@KetQua", laDung ? 1 : 0)
            };

            SqlDataProvider.Instance.ExecuteNonQuery(query, parameters);
        }

        public KetQuaKhaoSatDTO? LayKetQuaKhaoSatGanNhat(int maNguoiDung)
        {
            if (maNguoiDung <= 0) return null;

            string query = @"
                SELECT TOP 1 lks.MaLuotKhaoSat, lks.TongSoCau, lks.SoCauDung, lks.DiemDatDuoc, 
                             td.MaCapDo AS MaTrinhDo, td.TenTrinhDo
                FROM LuotKhaoSat lks
                LEFT JOIN TrinhDo td ON lks.MaTrinhDoKetQua = td.MaTrinhDo
                WHERE lks.MaNguoiDung = @MaND
                ORDER BY lks.ThoiDiemNopBai DESC, lks.MaLuotKhaoSat DESC";

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaND", maNguoiDung) });
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                string maCapDo = row["MaTrinhDo"]?.ToString() ?? "B1";
                string tenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "Intermediate (Trung cấp)";
                int tongSoCau = Convert.ToInt32(row["TongSoCau"]);
                int soCauDung = Convert.ToInt32(row["SoCauDung"]);
                decimal diem = Convert.ToDecimal(row["DiemDatDuoc"]);

                return new KetQuaKhaoSatDTO
                {
                    TongSoCau = tongSoCau,
                    SoCauDung = soCauDung,
                    DiemSo = diem,
                    TrinhDoDaChon = maCapDo,
                    KetQuaDat = true,
                    TrinhDoPhuHop = maCapDo,
                    TenTrinhDoPhuHop = tenTrinhDo
                };
            }

            return null;
        }

        /// <summary>
        /// [US-GV06] Thêm mới hoặc Cập nhật Câu hỏi cùng toàn bộ Danh sách đáp án trong 1 SqlTransaction an toàn
        /// </summary>
        public bool LuuCauHoiVaDapAn(CauHoiKhaoSatDTO cauHoi, List<LuaChonDapAnDTO> dsDapAn, int nguoiTaoId, out string errorMessage, string nguonTao = "ThuCong")
        {
            errorMessage = string.Empty;
            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                int maCauHoi = cauHoi.MaCauHoi;

                // Ánh xạ Trình độ (A1 -> 1, A2 -> 2, B1 -> 3, B2 -> 4, C1 -> 5, C2 -> 6)
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
                cauHoi.MaTrinhDoId = maTrinhDoId;

                // Kiểm tra sự tồn tại thực tế của câu hỏi trong DB (hỗ trợ cả trường hợp MaCauHoi = 0)
                bool isExisting = false;
                if (maCauHoi >= 0)
                {
                    using var checkCmd = new SqlCommand("SELECT COUNT(1) FROM CauHoi WHERE MaCauHoi = @Id", conn, trans);
                    checkCmd.Parameters.AddWithValue("@Id", maCauHoi);
                    isExisting = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                }

                if (!isExisting)
                {
                    // 1. INSERT CÂU HỎI MỚI
                    string insertSql = @"
                        INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi, 
                                            DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet, NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaDinhDanhSeed, MaBaiHoc)
                        VALUES (@MaChuDe, @MaTrinhDo, @LoaiCauHoi, @KyNang, @DoKho, @NoiDung, 
                                @DoanVanDoc, @AudioPath, @GiaiThich, @LoiGiai, @DaKiemDuyet, @NguoiTao, @NguonTao, SYSDATETIME(), @DoanVanDanChung, @LoaiNguon, @TepTinNguon, @MaDinhDanhSeed, @MaBaiHoc);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using var cmd = new SqlCommand(insertSql, conn, trans);
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
                    cmd.Parameters.AddWithValue("@DaKiemDuyet", cauHoi.DaKiemDuyet);
                    cmd.Parameters.AddWithValue("@NguoiTao", nguoiTaoId > 0 ? (object)nguoiTaoId : DBNull.Value);
                    cmd.Parameters.AddWithValue("@NguonTao", string.IsNullOrWhiteSpace(nguonTao) ? "ThuCong" : nguonTao);
                    cmd.Parameters.AddWithValue("@DoanVanDanChung", (object?)cauHoi.DoanVanDanChung ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LoaiNguon", string.IsNullOrWhiteSpace(cauHoi.LoaiNguon) ? "Extracted" : cauHoi.LoaiNguon);
                    cmd.Parameters.AddWithValue("@TepTinNguon", (object?)cauHoi.TepTinNguon ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaDinhDanhSeed", (object?)cauHoi.MaDinhDanhSeed ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaBaiHoc", (object?)cauHoi.MaBaiHoc ?? DBNull.Value);

                    maCauHoi = (int)cmd.ExecuteScalar();
                    cauHoi.MaCauHoi = maCauHoi;
                }
                else
                {
                    // 2. UPDATE CÂU HỎI HIỆN TẠI
                    string updateSql = @"
                        UPDATE CauHoi
                        SET MaChuDe = @MaChuDe,
                            MaTrinhDo = @MaTrinhDo,
                            LoaiCauHoi = @LoaiCauHoi,
                            KyNang = @KyNang,
                            DoKho = @DoKho,
                            NoiDungCauHoi = @NoiDung,
                            DoanVanDoc = @DoanVanDoc,
                            DuongDanAudio = @AudioPath,
                            GiaiThichDapAn = @GiaiThich,
                            LoiGiaiChiTiet = @LoiGiai,
                            DaKiemDuyet = @DaKiemDuyet,
                            DoanVanDanChung = @DoanVanDanChung,
                            LoaiNguon = @LoaiNguon,
                            TepTinNguon = @TepTinNguon,
                            MaBaiHoc = COALESCE(@MaBaiHoc, MaBaiHoc)
                        WHERE MaCauHoi = @MaCauHoi;

                        -- Gỡ liên kết khóa ngoại tạm thời nếu có lượt làm bài trước đó tham chiếu
                        UPDATE CauTraLoiKhaoSat SET MaLuaChonDaChon = NULL WHERE MaCauHoi = @MaCauHoi;
                        UPDATE ChiTietTraLoi SET MaLuaChonDaChon = NULL WHERE MaCauHoi = @MaCauHoi;

                        DELETE FROM LuaChonDapAn WHERE MaCauHoi = @MaCauHoi;";

                    using var cmd = new SqlCommand(updateSql, conn, trans);
                    cmd.Parameters.AddWithValue("@MaCauHoi", maCauHoi);
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
                    cmd.Parameters.AddWithValue("@DaKiemDuyet", cauHoi.DaKiemDuyet);
                    cmd.Parameters.AddWithValue("@DoanVanDanChung", (object?)cauHoi.DoanVanDanChung ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LoaiNguon", string.IsNullOrWhiteSpace(cauHoi.LoaiNguon) ? "Extracted" : cauHoi.LoaiNguon);
                    cmd.Parameters.AddWithValue("@TepTinNguon", (object?)cauHoi.TepTinNguon ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaBaiHoc", (object?)cauHoi.MaBaiHoc ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                // 3. INSERT TỪNG PHƯƠNG ÁN LỰA CHỌN
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
    }
}
