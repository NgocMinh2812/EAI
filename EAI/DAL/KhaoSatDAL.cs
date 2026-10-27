using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class KhaoSatDAL
    {
        private static KhaoSatDAL? _instance;
        public static KhaoSatDAL Instance => _instance ??= new KhaoSatDAL();

        /// <summary>
        /// Lấy thông tin bài khảo sát đầu vào đang hoạt động (DangHoatDong = 1)
        /// </summary>
        public (int MaBaiKhaoSat, string TenBai, string MoTa, int TongSoCau, bool DangHoatDong) LayBaiKhaoSatDangHoatDong()
        {
            string sql = @"
                SELECT TOP 1 MaBaiKhaoSat, TenBaiKhaoSat, MoTa, TongSoCauHoi, DangHoatDong 
                FROM BaiKhaoSatDauVao 
                ORDER BY DangHoatDong DESC, MaBaiKhaoSat ASC";

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(sql);
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                return (
                    Convert.ToInt32(row["MaBaiKhaoSat"]),
                    row["TenBaiKhaoSat"]?.ToString() ?? "Khảo sát đánh giá năng lực đầu vào CEFR",
                    row["MoTa"]?.ToString() ?? "",
                    Convert.ToInt32(row["TongSoCauHoi"]),
                    row["DangHoatDong"] != DBNull.Value && Convert.ToBoolean(row["DangHoatDong"])
                );
            }

            return (1, "Khảo sát đánh giá năng lực đầu vào CEFR", "Bài kiểm tra chẩn đoán năng lực", 15, true);
        }

        /// <summary>
        /// Cập nhật thông tin tiêu đề, mô tả và trạng thái bài khảo sát
        /// </summary>
        public bool CapNhatThongTinBaiKhaoSat(int maBaiKhaoSat, string tenBai, string moTa, bool dangHoatDong)
        {
            string sql = @"
                UPDATE BaiKhaoSatDauVao
                SET TenBaiKhaoSat = @Ten,
                    MoTa = @MoTa,
                    DangHoatDong = @DangHoatDong
                WHERE MaBaiKhaoSat = @MaBai";

            var parameters = new[]
            {
                new SqlParameter("@Ten", tenBai),
                new SqlParameter("@MoTa", (object?)moTa ?? DBNull.Value),
                new SqlParameter("@DangHoatDong", dangHoatDong),
                new SqlParameter("@MaBai", maBaiKhaoSat)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(sql, parameters) > 0;
        }

        /// <summary>
        /// Lấy danh sách câu hỏi thuộc bộ đề khảo sát đang chọn kèm các lựa chọn đáp án
        /// </summary>
        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiTheoBaiKhaoSat(int maBaiKhaoSat)
        {
            var list = new List<CauHoiKhaoSatDTO>();
            string sql = @"
                SELECT ch.MaCauHoi, chk.ThuTuCauHoi, ch.KyNang, td.MaCapDo AS MaTrinhDo, 
                       td.TenTrinhDo, ch.NoiDungCauHoi, ch.DoanVanDoc, ch.GiaiThichDapAn,
                       ch.LoaiCauHoi, ch.DoKho, ch.DuongDanAudio AS AudioPath, ch.DaKiemDuyet
                FROM CauHoiKhaoSat chk
                INNER JOIN CauHoi ch ON chk.MaCauHoi = ch.MaCauHoi
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                WHERE chk.MaBaiKhaoSat = @MaBai
                ORDER BY chk.ThuTuCauHoi ASC";

            var dt = SqlDataProvider.Instance.ExecuteQuery(sql, new[] { new SqlParameter("@MaBai", maBaiKhaoSat) });
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CauHoiKhaoSatDTO
                {
                    MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                    ThuTuCauHoi = Convert.ToInt32(row["ThuTuCauHoi"]),
                    KyNang = row["KyNang"]?.ToString() ?? "Doc",
                    MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B1",
                    TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                    NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                    DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                    GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString(),
                    LoaiCauHoi = row["LoaiCauHoi"]?.ToString() ?? "TracNghiem",
                    DoKho = row["DoKho"]?.ToString() ?? "TrungBinh",
                    AudioPath = row["AudioPath"] == DBNull.Value ? null : row["AudioPath"]?.ToString(),
                    DaKiemDuyet = row["DaKiemDuyet"] != DBNull.Value && Convert.ToBoolean(row["DaKiemDuyet"])
                });
            }

            CauHoiDAL.NapDanhSachLuaChon(list);
            return list;
        }

        /// <summary>
        /// Cập nhật toàn bộ danh sách câu hỏi và thứ tự câu hỏi trong bài khảo sát (Nguyên tử bằng Transaction)
        /// </summary>
        public bool CapNhatCauHoiBaiKhaoSat(int maBaiKhaoSat, List<int> dsMaCauHoi, out string errorMessage)
        {
            errorMessage = string.Empty;
            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                // 1. Xóa các liên kết câu hỏi cũ của bài khảo sát
                string deleteSql = "DELETE FROM CauHoiKhaoSat WHERE MaBaiKhaoSat = @MaBai";
                using (var cmdDel = new SqlCommand(deleteSql, conn, trans))
                {
                    cmdDel.Parameters.AddWithValue("@MaBai", maBaiKhaoSat);
                    cmdDel.ExecuteNonQuery();
                }

                // 2. Thêm lại từng câu hỏi theo đúng thứ tự
                string insertSql = @"
                    INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi)
                    VALUES (@MaBai, @MaCH, @ThuTu);";

                int order = 1;
                foreach (int maCauHoi in dsMaCauHoi)
                {
                    using var cmdIns = new SqlCommand(insertSql, conn, trans);
                    cmdIns.Parameters.AddWithValue("@MaBai", maBaiKhaoSat);
                    cmdIns.Parameters.AddWithValue("@MaCH", maCauHoi);
                    cmdIns.Parameters.AddWithValue("@ThuTu", order++);
                    cmdIns.ExecuteNonQuery();
                }

                // 3. Cập nhật tổng số câu hỏi trong BaiKhaoSatDauVao
                string updateCountSql = @"
                    UPDATE BaiKhaoSatDauVao 
                    SET TongSoCauHoi = @TongSo 
                    WHERE MaBaiKhaoSat = @MaBai";

                using (var cmdCount = new SqlCommand(updateCountSql, conn, trans))
                {
                    cmdCount.Parameters.AddWithValue("@TongSo", dsMaCauHoi.Count);
                    cmdCount.Parameters.AddWithValue("@MaBai", maBaiKhaoSat);
                    cmdCount.ExecuteNonQuery();
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

        /// <summary>
        /// Lấy danh sách câu hỏi trong ngân hàng có thể thêm vào bài khảo sát (chưa có trong bài)
        /// </summary>
        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiCoTheThem(int maBaiKhaoSat, string? tuKhoa = null, string? trinhDo = null)
        {
            var list = new List<CauHoiKhaoSatDTO>();
            string sql = @"
                SELECT ch.MaCauHoi, 0 AS ThuTuCauHoi, ch.KyNang, td.MaCapDo AS MaTrinhDo, 
                       td.TenTrinhDo, ch.NoiDungCauHoi, ch.DoanVanDoc, ch.GiaiThichDapAn,
                       ch.LoaiCauHoi, ch.DoKho, ch.DuongDanAudio AS AudioPath, ch.DaKiemDuyet
                FROM CauHoi ch
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                WHERE ch.DaKiemDuyet = 1
                  AND ch.MaCauHoi NOT IN (SELECT MaCauHoi FROM CauHoiKhaoSat WHERE MaBaiKhaoSat = @MaBai)
                  AND (@TuKhoa IS NULL OR @TuKhoa = '' OR ch.NoiDungCauHoi LIKE N'%' + @TuKhoa + N'%')
                  AND (@TrinhDo IS NULL OR @TrinhDo = 'TatCa' OR td.MaCapDo = @TrinhDo)
                ORDER BY ch.MaCauHoi DESC";

            var parameters = new[]
            {
                new SqlParameter("@MaBai", maBaiKhaoSat),
                new SqlParameter("@TuKhoa", string.IsNullOrWhiteSpace(tuKhoa) ? (object)DBNull.Value : tuKhoa.Trim()),
                new SqlParameter("@TrinhDo", string.IsNullOrWhiteSpace(trinhDo) ? (object)DBNull.Value : trinhDo.Trim())
            };

            var dt = SqlDataProvider.Instance.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CauHoiKhaoSatDTO
                {
                    MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                    ThuTuCauHoi = 0,
                    KyNang = row["KyNang"]?.ToString() ?? "Doc",
                    MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B1",
                    TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                    NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                    DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                    GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString(),
                    LoaiCauHoi = row["LoaiCauHoi"]?.ToString() ?? "TracNghiem",
                    DoKho = row["DoKho"]?.ToString() ?? "TrungBinh"
                });
            }

            CauHoiDAL.NapDanhSachLuaChon(list);
            return list;
        }

        /// <summary>
        /// Lấy toàn bộ cấu hình ma trận quy đổi điểm sang khung CEFR / IELTS / TOEIC từ SQL Server
        /// </summary>
        public List<CauHinhQuyDoiKhaoSatDTO> LayMaTranQuyDoi()
        {
            var list = new List<CauHinhQuyDoiKhaoSatDTO>();
            string sql = @"
                SELECT MaCauHinh, MaCapDo, TenHienThi, DiemPhanTramMin, DiemPhanTramMax, 
                       QuyDoiIelts, QuyDoiToeic, LoTrinhDeXuat, MauSac, ThuTuSapXep
                FROM CauHinhQuyDoiKhaoSat
                ORDER BY ThuTuSapXep ASC, DiemPhanTramMin ASC";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(sql);
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new CauHinhQuyDoiKhaoSatDTO
                    {
                        MaCauHinh = Convert.ToInt32(row["MaCauHinh"]),
                        MaCapDo = row["MaCapDo"]?.ToString() ?? "",
                        TenHienThi = row["TenHienThi"]?.ToString() ?? "",
                        DiemPhanTramMin = Convert.ToInt32(row["DiemPhanTramMin"]),
                        DiemPhanTramMax = Convert.ToInt32(row["DiemPhanTramMax"]),
                        QuyDoiIelts = row["QuyDoiIelts"]?.ToString() ?? "",
                        QuyDoiToeic = row["QuyDoiToeic"]?.ToString() ?? "",
                        LoTrinhDeXuat = row["LoTrinhDeXuat"]?.ToString() ?? "",
                        MauSac = row["MauSac"]?.ToString() ?? "#0F4C3A",
                        ThuTuSapXep = Convert.ToInt32(row["ThuTuSapXep"])
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[KhaoSatDAL.LayMaTranQuyDoi] {ex.Message}");
            }

            return list;
        }

        /// <summary>
        /// Lưu cập nhật ma trận quy đổi trình độ vào SQL Server
        /// </summary>
        public bool LuuMaTranQuyDoi(List<CauHinhQuyDoiKhaoSatDTO> danhSach, out string errorMessage)
        {
            errorMessage = string.Empty;
            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                string updateSql = @"
                    UPDATE CauHinhQuyDoiKhaoSat
                    SET TenHienThi = @TenHienThi,
                        DiemPhanTramMin = @Min,
                        DiemPhanTramMax = @Max,
                        QuyDoiIelts = @Ielts,
                        QuyDoiToeic = @Toeic,
                        LoTrinhDeXuat = @LoTrinh,
                        MauSac = @MauSac
                    WHERE MaCapDo = @MaCapDo;";

                foreach (var item in danhSach)
                {
                    using var cmd = new SqlCommand(updateSql, conn, trans);
                    cmd.Parameters.AddWithValue("@TenHienThi", item.TenHienThi);
                    cmd.Parameters.AddWithValue("@Min", item.DiemPhanTramMin);
                    cmd.Parameters.AddWithValue("@Max", item.DiemPhanTramMax);
                    cmd.Parameters.AddWithValue("@Ielts", item.QuyDoiIelts);
                    cmd.Parameters.AddWithValue("@Toeic", item.QuyDoiToeic);
                    cmd.Parameters.AddWithValue("@LoTrinh", item.LoTrinhDeXuat);
                    cmd.Parameters.AddWithValue("@MauSac", (object?)item.MauSac ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaCapDo", item.MaCapDo);
                    cmd.ExecuteNonQuery();
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

        /// <summary>
        /// Lấy danh sách toàn bộ sinh viên đã nộp bài khảo sát đầu vào từ CSDL
        /// </summary>
        public List<SinhVienKhaoSatResultDTO> LayDanhSachSinhVienKhaoSat(string? tuKhoa = null, string? trinhDo = null)
        {
            var list = new List<SinhVienKhaoSatResultDTO>();
            string sql = @"
                SELECT lks.MaLuotKhaoSat, lks.MaNguoiDung, nd.HoTen, nd.TenDangNhap, nd.Email,
                       lks.ThoiDiemBatDau, lks.ThoiDiemNopBai, lks.TongSoCau, lks.SoCauDung, lks.DiemDatDuoc,
                       ISNULL(td.MaCapDo, lks.KhungDanhGiaKetQua) AS MaCapDo,
                       ISNULL(td.TenTrinhDo, N'Trình độ ' + ISNULL(td.MaCapDo, 'B1')) AS TenTrinhDo,
                       lks.KhungDanhGiaKetQua
                FROM LuotKhaoSat lks
                INNER JOIN NguoiDung nd ON lks.MaNguoiDung = nd.MaNguoiDung
                LEFT JOIN TrinhDo td ON lks.MaTrinhDoKetQua = td.MaTrinhDo
                WHERE (@TuKhoa IS NULL OR @TuKhoa = '' OR nd.HoTen LIKE N'%' + @TuKhoa + N'%' OR nd.TenDangNhap LIKE N'%' + @TuKhoa + N'%')
                  AND (@TrinhDo IS NULL OR @TrinhDo = 'TatCa' OR td.MaCapDo = @TrinhDo)
                ORDER BY lks.ThoiDiemNopBai DESC, lks.MaLuotKhaoSat DESC";

            var parameters = new[]
            {
                new SqlParameter("@TuKhoa", string.IsNullOrWhiteSpace(tuKhoa) ? (object)DBNull.Value : tuKhoa.Trim()),
                new SqlParameter("@TrinhDo", string.IsNullOrWhiteSpace(trinhDo) ? (object)DBNull.Value : trinhDo.Trim())
            };

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(sql, parameters);
                var matrix = LayMaTranQuyDoi();

                foreach (DataRow row in dt.Rows)
                {
                    string capDo = row["MaCapDo"]?.ToString() ?? "B1";
                    var matrixItem = matrix.Find(m => string.Equals(m.MaCapDo, capDo, StringComparison.OrdinalIgnoreCase));

                    list.Add(new SinhVienKhaoSatResultDTO
                    {
                        MaLuotKhaoSat = Convert.ToInt32(row["MaLuotKhaoSat"]),
                        MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                        HoTen = row["HoTen"]?.ToString() ?? "",
                        TenDangNhap = row["TenDangNhap"]?.ToString() ?? "",
                        Email = row["Email"]?.ToString() ?? "",
                        ThoiDiemBatDau = row["ThoiDiemBatDau"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["ThoiDiemBatDau"]),
                        ThoiDiemNopBai = row["ThoiDiemNopBai"] == DBNull.Value ? null : Convert.ToDateTime(row["ThoiDiemNopBai"]),
                        TongSoCau = Convert.ToInt32(row["TongSoCau"]),
                        SoCauDung = Convert.ToInt32(row["SoCauDung"]),
                        DiemDatDuoc = Convert.ToDecimal(row["DiemDatDuoc"]),
                        MaCapDo = capDo,
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                        KhungDanhGiaKetQua = row["KhungDanhGiaKetQua"]?.ToString() ?? "CEFR",
                        QuyDoiIelts = matrixItem?.QuyDoiIelts ?? "IELTS 5.0 - 5.5",
                        QuyDoiToeic = matrixItem?.QuyDoiToeic ?? "TOEIC 550 - 650"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[KhaoSatDAL.LayDanhSachSinhVienKhaoSat] {ex.Message}");
            }

            return list;
        }

        /// <summary>
        /// Lấy chi tiết các câu trả lời của học viên trong một lượt làm bài khảo sát
        /// </summary>
        public List<ChiTietTraLoiKhaoSatItemDTO> LayChiTietKhaoSatHocVien(int maLuotKhaoSat)
        {
            var list = new List<ChiTietTraLoiKhaoSatItemDTO>();
            string sql = @"
                SELECT ct.MaCauTraLoiKhaoSat, ct.MaCauHoi, ch.NoiDungCauHoi, ch.DoanVanDoc, 
                       ch.KyNang, td.MaCapDo, ct.MaLuaChonDaChon, ct.KetQuaDungSai, ch.GiaiThichDapAn,
                       lc.KyHieuLuaChon, lc.NoiDungLuaChon
                FROM CauTraLoiKhaoSat ct
                INNER JOIN CauHoi ch ON ct.MaCauHoi = ch.MaCauHoi
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN LuaChonDapAn lc ON ct.MaLuaChonDaChon = lc.MaLuaChon
                WHERE ct.MaLuotKhaoSat = @MaLuot
                ORDER BY ct.MaCauTraLoiKhaoSat ASC";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(sql, new[] { new SqlParameter("@MaLuot", maLuotKhaoSat) });
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new ChiTietTraLoiKhaoSatItemDTO
                    {
                        MaCauTraLoiKhaoSat = Convert.ToInt32(row["MaCauTraLoiKhaoSat"]),
                        MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                        NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                        DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                        KyNang = row["KyNang"]?.ToString() ?? "Doc",
                        MaTrinhDo = row["MaCapDo"]?.ToString() ?? "B1",
                        MaLuaChonDaChon = row["MaLuaChonDaChon"] == DBNull.Value ? null : Convert.ToInt32(row["MaLuaChonDaChon"]),
                        KyHieuDaChon = row["KyHieuLuaChon"]?.ToString() ?? "",
                        NoiDungDaChon = row["NoiDungLuaChon"]?.ToString() ?? "",
                        KetQuaDungSai = row["KetQuaDungSai"] != DBNull.Value && Convert.ToBoolean(row["KetQuaDungSai"]),
                        GiaiThichDapAn = row["GiaiThichDapAn"]?.ToString() ?? ""
                    });
                }

                // Nạp tất cả lựa chọn cho từng câu hỏi
                var tempQuestionList = new List<CauHoiKhaoSatDTO>();
                foreach (var item in list)
                {
                    tempQuestionList.Add(new CauHoiKhaoSatDTO { MaCauHoi = item.MaCauHoi });
                }
                CauHoiDAL.NapDanhSachLuaChon(tempQuestionList);

                for (int i = 0; i < list.Count; i++)
                {
                    list[i].DanhSachLuaChon = tempQuestionList[i].DanhSachLuaChon;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[KhaoSatDAL.LayChiTietKhaoSatHocVien] {ex.Message}");
            }

            return list;
        }
    }
}
