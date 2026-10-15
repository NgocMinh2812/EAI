using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class BaiTapDeThiDAL
    {
        private static BaiTapDeThiDAL? _instance;
        public static BaiTapDeThiDAL Instance => _instance ??= new BaiTapDeThiDAL();

        public List<BaiTapDeThiDTO> LayDanhSachDeThi(string? loaiHinh = null, string? trinhDo = null)
        {
            var list = new List<BaiTapDeThiDTO>();
            string query = @"
                SELECT dt.MaBaiTapDeThi, dt.MaBaiHoc, dt.MaChuDe, dt.MaTrinhDo,
                       dt.TenBaiTapDeThi, dt.LoaiHinh, dt.MoTa, dt.ThoiGianLamBaiPhut, dt.DaPhatHanh,
                       td.MaCapDo, td.TenTrinhDo, cd.TenChuDe,
                       (SELECT COUNT(*) FROM CauHoiBaiTap chbt WHERE chbt.MaBaiTapDeThi = dt.MaBaiTapDeThi) AS SoLuongCauHoi
                FROM BaiTapDeThi dt
                LEFT JOIN TrinhDo td ON dt.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON dt.MaChuDe = cd.MaChuDe
                WHERE (@LoaiHinh IS NULL OR dt.LoaiHinh = @LoaiHinh)
                  AND (@TrinhDo IS NULL OR td.MaCapDo = @TrinhDo)
                ORDER BY dt.MaBaiTapDeThi ASC";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@LoaiHinh", (object?)loaiHinh ?? DBNull.Value),
                new SqlParameter("@TrinhDo", (object?)trinhDo ?? DBNull.Value)
            };

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
                foreach (DataRow row in dt.Rows)
                {
                    int soCau = Convert.ToInt32(row["SoLuongCauHoi"]);
                    if (soCau == 0) soCau = 10; // Default display if not linked yet

                    list.Add(new BaiTapDeThiDTO
                    {
                        MaBaiTapDeThi = Convert.ToInt32(row["MaBaiTapDeThi"]),
                        MaBaiHoc = row["MaBaiHoc"] == DBNull.Value ? null : Convert.ToInt32(row["MaBaiHoc"]),
                        MaChuDe = row["MaChuDe"] == DBNull.Value ? null : Convert.ToInt32(row["MaChuDe"]),
                        MaTrinhDo = row["MaTrinhDo"] == DBNull.Value ? null : Convert.ToInt32(row["MaTrinhDo"]),
                        TenBaiTapDeThi = row["TenBaiTapDeThi"]?.ToString() ?? "",
                        LoaiHinh = row["LoaiHinh"]?.ToString() ?? "ThiThu",
                        MoTa = row["MoTa"]?.ToString() ?? "",
                        ThoiGianLamBaiPhut = row["ThoiGianLamBaiPhut"] == DBNull.Value ? 30 : Convert.ToInt32(row["ThoiGianLamBaiPhut"]),
                        DaPhatHanh = row["DaPhatHanh"] == DBNull.Value || Convert.ToBoolean(row["DaPhatHanh"]),
                        MaCapDo = row["MaCapDo"]?.ToString() ?? "B2",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "Trung cấp",
                        TenChuDe = row["TenChuDe"]?.ToString() ?? "Học thuật",
                        SoLuongCauHoi = soCau
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lấy danh sách đề: {ex.Message}");
            }

            return list;
        }

        public BaiTapDeThiDTO? LayChiTietDeThi(int maDeThi)
        {
            string query = @"
                SELECT dt.MaBaiTapDeThi, dt.MaBaiHoc, dt.MaChuDe, dt.MaTrinhDo,
                       dt.TenBaiTapDeThi, dt.LoaiHinh, dt.MoTa, dt.ThoiGianLamBaiPhut, dt.DaPhatHanh,
                       td.MaCapDo, td.TenTrinhDo, cd.TenChuDe,
                       (SELECT COUNT(*) FROM CauHoiBaiTap chbt WHERE chbt.MaBaiTapDeThi = dt.MaBaiTapDeThi) AS SoLuongCauHoi
                FROM BaiTapDeThi dt
                LEFT JOIN TrinhDo td ON dt.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON dt.MaChuDe = cd.MaChuDe
                WHERE dt.MaBaiTapDeThi = @MaDe";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaDe", maDeThi) });
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    int soCau = Convert.ToInt32(row["SoLuongCauHoi"]);
                    if (soCau == 0) soCau = 10;

                    return new BaiTapDeThiDTO
                    {
                        MaBaiTapDeThi = Convert.ToInt32(row["MaBaiTapDeThi"]),
                        MaBaiHoc = row["MaBaiHoc"] == DBNull.Value ? null : Convert.ToInt32(row["MaBaiHoc"]),
                        MaChuDe = row["MaChuDe"] == DBNull.Value ? null : Convert.ToInt32(row["MaChuDe"]),
                        MaTrinhDo = row["MaTrinhDo"] == DBNull.Value ? null : Convert.ToInt32(row["MaTrinhDo"]),
                        TenBaiTapDeThi = row["TenBaiTapDeThi"]?.ToString() ?? "",
                        LoaiHinh = row["LoaiHinh"]?.ToString() ?? "ThiThu",
                        MoTa = row["MoTa"]?.ToString() ?? "",
                        ThoiGianLamBaiPhut = row["ThoiGianLamBaiPhut"] == DBNull.Value ? 30 : Convert.ToInt32(row["ThoiGianLamBaiPhut"]),
                        DaPhatHanh = row["DaPhatHanh"] == DBNull.Value || Convert.ToBoolean(row["DaPhatHanh"]),
                        MaCapDo = row["MaCapDo"]?.ToString() ?? "B2",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "Trung cấp",
                        TenChuDe = row["TenChuDe"]?.ToString() ?? "Học thuật",
                        SoLuongCauHoi = soCau
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lấy chi tiết đề: {ex.Message}");
            }
            return null;
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiCuaDeThi(int maDeThi)
        {
            var list = new List<CauHoiKhaoSatDTO>();

            string query = @"
                SELECT ch.MaCauHoi, chbt.ThuTuCauHoi, ch.KyNang, td.MaCapDo AS MaTrinhDo,
                       td.TenTrinhDo, ch.NoiDungCauHoi, ch.DoanVanDoc, ch.GiaiThichDapAn
                FROM CauHoiBaiTap chbt
                INNER JOIN CauHoi ch ON chbt.MaCauHoi = ch.MaCauHoi
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                WHERE chbt.MaBaiTapDeThi = @MaDe
                ORDER BY chbt.ThuTuCauHoi ASC";

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaDe", maDeThi) });

            if (dt.Rows.Count == 0)
            {
                // Fallback nếu đề thi chưa gắn câu trong CauHoiBaiTap: lấy 10 câu trắc nghiệm từ CauHoi
                string fallbackQuery = @"
                    SELECT TOP 10 ch.MaCauHoi, ROW_NUMBER() OVER(ORDER BY ch.MaCauHoi ASC) AS ThuTuCauHoi,
                           ch.KyNang, td.MaCapDo AS MaTrinhDo, td.TenTrinhDo, ch.NoiDungCauHoi,
                           ch.DoanVanDoc, ch.GiaiThichDapAn
                    FROM CauHoi ch
                    LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                    WHERE ch.DaKiemDuyet = 1
                    ORDER BY ch.MaCauHoi ASC";
                dt = SqlDataProvider.Instance.ExecuteQuery(fallbackQuery);
            }

            int thuTu = 1;
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CauHoiKhaoSatDTO
                {
                    MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                    ThuTuCauHoi = thuTu++,
                    KyNang = row["KyNang"]?.ToString() ?? "NguPhap",
                    MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B2",
                    TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                    NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                    DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                    GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString()
                });
            }

            // Nạp toàn bộ lựa chọn trong 1 truy vấn duy nhất (loại bỏ N+1 query)
            CauHoiDAL.NapDanhSachLuaChon(list);

            return list;
        }

        public int LuuKetQuaThi(int maNguoiDung, int maDeThi, string hinhThucLam, DateTime batDau,
                                int thoiGianGiay, int tongSoCau, int soCauDung, decimal diemSo,
                                List<(int MaCauHoi, int? MaLuaChon, bool DungSai)> chiTiet)
        {
            int soCauSai = tongSoCau - soCauDung;

            string queryLuot = @"
                INSERT INTO LuotLamBai (MaNguoiDung, MaBaiTapDeThi, HinhThucLam, ThoiDiemBatDau, ThoiDiemNopBai, 
                                        ThoiGianLamGiay, TongSoCau, SoCauDung, SoCauSai, DiemSo, TrangThai)
                VALUES (@MaND, @MaDe, @HinhThuc, @BatDau, SYSDATETIME(), @ThoiGian, @TongSo, @SoDung, @SoSai, @Diem, N'DaNop');
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] pLuot = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@MaDe", maDeThi),
                new SqlParameter("@HinhThuc", hinhThucLam),
                new SqlParameter("@BatDau", batDau),
                new SqlParameter("@ThoiGian", thoiGianGiay),
                new SqlParameter("@TongSo", tongSoCau),
                new SqlParameter("@SoDung", soCauDung),
                new SqlParameter("@SoSai", soCauSai),
                new SqlParameter("@Diem", diemSo)
            };

            object? idObj = SqlDataProvider.Instance.ExecuteScalar(queryLuot, pLuot);
            int maLuotLamBai = Convert.ToInt32(idObj);

            // Lưu chi tiết từng câu trả lời
            foreach (var item in chiTiet)
            {
                string queryChiTiet = @"
                    INSERT INTO ChiTietTraLoi (MaLuotLamBai, MaCauHoi, MaLuaChonDaChon, KetQuaDungSai, DiemSo, ThoiDiemTraLoi)
                    VALUES (@MaLuot, @MaCauHoi, @MaLC, @DungSai, @DiemCau, SYSDATETIME());";

                decimal diemCau = item.DungSai ? (10.0m / Math.Max(1, tongSoCau)) : 0;

                SqlParameter[] pCT = new[]
                {
                    new SqlParameter("@MaLuot", maLuotLamBai),
                    new SqlParameter("@MaCauHoi", item.MaCauHoi),
                    new SqlParameter("@MaLC", (object?)item.MaLuaChon ?? DBNull.Value),
                    new SqlParameter("@DungSai", item.DungSai ? 1 : 0),
                    new SqlParameter("@DiemCau", diemCau)
                };

                try
                {
                    SqlDataProvider.Instance.ExecuteNonQuery(queryChiTiet, pCT);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lưu ChiTietTraLoi: {ex.Message}");
                }
            }

            // Ghi nhận vào bảng LichSuHocTap
            try
            {
                string queryLichSu = @"
                    INSERT INTO LichSuHocTap (MaNguoiDung, MaLuotLamBai, LoaiHoatDong, SoPhutHoc, DiemSo, SoCauDung, SoCauSai, NgayGhiNhan)
                    VALUES (@MaND, @MaLuot, @LoaiHD, @SoPhut, @Diem, @SoDung, @SoSai, SYSDATETIME());";

                int soPhut = Math.Max(1, thoiGianGiay / 60);
                SqlParameter[] pLS = new[]
                {
                    new SqlParameter("@MaND", maNguoiDung),
                    new SqlParameter("@MaLuot", maLuotLamBai),
                    new SqlParameter("@LoaiHD", hinhThucLam),
                    new SqlParameter("@SoPhut", soPhut),
                    new SqlParameter("@Diem", diemSo),
                    new SqlParameter("@SoDung", soCauDung),
                    new SqlParameter("@SoSai", soCauSai)
                };
                SqlDataProvider.Instance.ExecuteNonQuery(queryLichSu, pLS);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lưu LichSuHocTap: {ex.Message}");
            }

            return maLuotLamBai;
        }

        public LuotLamBaiDTO? LayChiTietKetQuaLamBai(int maLuotLamBai)
        {
            string queryLuot = @"
                SELECT lb.MaLuotLamBai, lb.MaNguoiDung, lb.MaBaiTapDeThi, dt.TenBaiTapDeThi,
                       lb.HinhThucLam, lb.ThoiDiemBatDau, lb.ThoiDiemNopBai, lb.ThoiGianLamGiay,
                       lb.TongSoCau, lb.SoCauDung, lb.SoCauSai, lb.DiemSo, lb.TrangThai
                FROM LuotLamBai lb
                LEFT JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                WHERE lb.MaLuotLamBai = @MaLuot";

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(queryLuot, new[] { new SqlParameter("@MaLuot", maLuotLamBai) });
            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            var dto = new LuotLamBaiDTO
            {
                MaLuotLamBai = Convert.ToInt32(row["MaLuotLamBai"]),
                MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                MaBaiTapDeThi = Convert.ToInt32(row["MaBaiTapDeThi"]),
                TenBaiTapDeThi = row["TenBaiTapDeThi"]?.ToString() ?? "Bài thi",
                HinhThucLam = row["HinhThucLam"]?.ToString() ?? "ThiThu",
                ThoiDiemBatDau = Convert.ToDateTime(row["ThoiDiemBatDau"]),
                ThoiDiemNopBai = row["ThoiDiemNopBai"] == DBNull.Value ? null : Convert.ToDateTime(row["ThoiDiemNopBai"]),
                ThoiGianLamGiay = Convert.ToInt32(row["ThoiGianLamGiay"]),
                TongSoCau = Convert.ToInt32(row["TongSoCau"]),
                SoCauDung = Convert.ToInt32(row["SoCauDung"]),
                SoCauSai = Convert.ToInt32(row["SoCauSai"]),
                DiemSo = Convert.ToDecimal(row["DiemSo"]),
                TrangThai = row["TrangThai"]?.ToString() ?? "DaNop"
            };

            // Lấy danh sách câu hỏi kèm đáp án chọn và giải thích
            string queryCT = @"
                SELECT ct.MaChiTietTraLoi, ct.MaCauHoi, ch.NoiDungCauHoi, ch.DoanVanDoc, ch.KyNang,
                       ch.GiaiThichDapAn, ct.MaLuaChonDaChon, ct.KetQuaDungSai, ct.DiemSo,
                       lcChon.KyHieuLuaChon AS KyHieuDaChon, lcChon.NoiDungLuaChon AS NoiDungDaChon,
                       lcDung.MaLuaChon AS MaLuaChonDung, lcDung.KyHieuLuaChon AS KyHieuDapAnDung, lcDung.NoiDungLuaChon AS NoiDungDapAnDung
                FROM ChiTietTraLoi ct
                INNER JOIN CauHoi ch ON ct.MaCauHoi = ch.MaCauHoi
                LEFT JOIN LuaChonDapAn lcChon ON ct.MaLuaChonDaChon = lcChon.MaLuaChon
                LEFT JOIN LuaChonDapAn lcDung ON ch.MaCauHoi = lcDung.MaCauHoi AND lcDung.LaDapAnDung = 1
                WHERE ct.MaLuotLamBai = @MaLuot
                ORDER BY ct.MaChiTietTraLoi ASC";

            DataTable dtCT = SqlDataProvider.Instance.ExecuteQuery(queryCT, new[] { new SqlParameter("@MaLuot", maLuotLamBai) });
            int thuTu = 1;
            foreach (DataRow r in dtCT.Rows)
            {
                int maCH = Convert.ToInt32(r["MaCauHoi"]);
                var item = new ChiTietTraLoiDTO
                {
                    MaChiTietTraLoi = Convert.ToInt32(r["MaChiTietTraLoi"]),
                    MaLuotLamBai = maLuotLamBai,
                    MaCauHoi = maCH,
                    ThuTuCauHoi = thuTu++,
                    NoiDungCauHoi = r["NoiDungCauHoi"]?.ToString() ?? "",
                    DoanVanDoc = r["DoanVanDoc"] == DBNull.Value ? null : r["DoanVanDoc"]?.ToString(),
                    KyNang = r["KyNang"]?.ToString() ?? "Doc",
                    MaLuaChonDaChon = r["MaLuaChonDaChon"] == DBNull.Value ? null : Convert.ToInt32(r["MaLuaChonDaChon"]),
                    KyHieuDaChon = r["KyHieuDaChon"]?.ToString() ?? "",
                    NoiDungDaChon = r["NoiDungDaChon"]?.ToString() ?? "",
                    MaLuaChonDung = r["MaLuaChonDung"] == DBNull.Value ? 0 : Convert.ToInt32(r["MaLuaChonDung"]),
                    KyHieuDapAnDung = r["KyHieuDapAnDung"]?.ToString() ?? "",
                    NoiDungDapAnDung = r["NoiDungDapAnDung"]?.ToString() ?? "",
                    KetQuaDungSai = r["KetQuaDungSai"] == DBNull.Value ? false : Convert.ToBoolean(r["KetQuaDungSai"]),
                    DiemSo = r["DiemSo"] == DBNull.Value ? 0 : Convert.ToDecimal(r["DiemSo"]),
                    GiaiThichDapAn = r["GiaiThichDapAn"] == DBNull.Value ? null : r["GiaiThichDapAn"]?.ToString()
                };

                // Lấy tất cả lựa chọn
                string queryLC = "SELECT MaLuaChon, MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung FROM LuaChonDapAn WHERE MaCauHoi = @MaCauHoi ORDER BY KyHieuLuaChon ASC";
                DataTable dtLC = SqlDataProvider.Instance.ExecuteQuery(queryLC, new[] { new SqlParameter("@MaCauHoi", maCH) });
                foreach (DataRow rLC in dtLC.Rows)
                {
                    item.DanhSachLuaChon.Add(new LuaChonDapAnDTO
                    {
                        MaLuaChon = Convert.ToInt32(rLC["MaLuaChon"]),
                        MaCauHoi = Convert.ToInt32(rLC["MaCauHoi"]),
                        KyHieuLuaChon = rLC["KyHieuLuaChon"]?.ToString() ?? "",
                        NoiDungLuaChon = rLC["NoiDungLuaChon"]?.ToString() ?? "",
                        LaDapAnDung = Convert.ToBoolean(rLC["LaDapAnDung"])
                    });
                }

                dto.DanhSachChiTiet.Add(item);
            }

            return dto;
        }

        public List<LuotLamBaiDTO> LayLichSuThi(int maNguoiDung, int top = 5)
        {
            var list = new List<LuotLamBaiDTO>();
            string query = @"
                SELECT TOP (@Top) lb.MaLuotLamBai, lb.MaNguoiDung, lb.MaBaiTapDeThi, dt.TenBaiTapDeThi,
                       lb.HinhThucLam, lb.ThoiDiemBatDau, lb.ThoiDiemNopBai, lb.ThoiGianLamGiay,
                       lb.TongSoCau, lb.SoCauDung, lb.SoCauSai, lb.DiemSo, lb.TrangThai
                FROM LuotLamBai lb
                LEFT JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                WHERE lb.MaNguoiDung = @MaND
                ORDER BY lb.ThoiDiemNopBai DESC, lb.MaLuotLamBai DESC";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaND", maNguoiDung), new SqlParameter("@Top", top) });
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new LuotLamBaiDTO
                    {
                        MaLuotLamBai = Convert.ToInt32(row["MaLuotLamBai"]),
                        MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                        MaBaiTapDeThi = Convert.ToInt32(row["MaBaiTapDeThi"]),
                        TenBaiTapDeThi = row["TenBaiTapDeThi"]?.ToString() ?? "Bài thi",
                        HinhThucLam = row["HinhThucLam"]?.ToString() ?? "ThiThu",
                        ThoiDiemBatDau = Convert.ToDateTime(row["ThoiDiemBatDau"]),
                        ThoiDiemNopBai = row["ThoiDiemNopBai"] == DBNull.Value ? null : Convert.ToDateTime(row["ThoiDiemNopBai"]),
                        ThoiGianLamGiay = Convert.ToInt32(row["ThoiGianLamGiay"]),
                        TongSoCau = Convert.ToInt32(row["TongSoCau"]),
                        SoCauDung = Convert.ToInt32(row["SoCauDung"]),
                        SoCauSai = Convert.ToInt32(row["SoCauSai"]),
                        DiemSo = Convert.ToDecimal(row["DiemSo"]),
                        TrangThai = row["TrangThai"]?.ToString() ?? "DaNop"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lấy lịch sử thi: {ex.Message}");
            }
            return list;
        }
    }
}
