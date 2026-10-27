using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class BaoCaoThongKeDAL
    {
        private static BaoCaoThongKeDAL? _instance;
        public static BaoCaoThongKeDAL Instance => _instance ??= new BaoCaoThongKeDAL();

        /// <summary>
        /// Lấy toàn bộ số liệu thống kê học tập, KPI, biểu đồ và danh sách lượt làm bài theo bộ lọc
        /// </summary>
        public BaoCaoThongKeHocTapDTO LayBaoCaoThongKe(ThongKeHocTapFilterDTO filter)
        {
            var report = new BaoCaoThongKeHocTapDTO();

            // 1. Xây dựng điều kiện WHERE động
            var whereClauses = new List<string>();
            var parameters = new List<SqlParameter>();

            if (filter.TuNgay.HasValue)
            {
                whereClauses.Add("lb.ThoiDiemBatDau >= @TuNgay");
                parameters.Add(new SqlParameter("@TuNgay", filter.TuNgay.Value.Date));
            }

            if (filter.DenNgay.HasValue)
            {
                whereClauses.Add("lb.ThoiDiemBatDau < @DenNgay");
                parameters.Add(new SqlParameter("@DenNgay", filter.DenNgay.Value.Date.AddDays(1)));
            }

            if (filter.MaBaiTapDeThi.HasValue && filter.MaBaiTapDeThi.Value > 0)
            {
                whereClauses.Add("lb.MaBaiTapDeThi = @MaDe");
                parameters.Add(new SqlParameter("@MaDe", filter.MaBaiTapDeThi.Value));
            }

            if (!string.IsNullOrWhiteSpace(filter.HinhThucLam) && !filter.HinhThucLam.Equals("TatCa", StringComparison.OrdinalIgnoreCase))
            {
                whereClauses.Add("lb.HinhThucLam = @HinhThuc");
                parameters.Add(new SqlParameter("@HinhThuc", filter.HinhThucLam.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(filter.TuKhoa))
            {
                whereClauses.Add("(nd.HoTen LIKE N'%' + @TuKhoa + N'%' OR nd.TenDangNhap LIKE N'%' + @TuKhoa + N'%' OR dt.TenBaiTapDeThi LIKE N'%' + @TuKhoa + N'%')");
                parameters.Add(new SqlParameter("@TuKhoa", filter.TuKhoa.Trim()));
            }

            string whereSql = whereClauses.Count > 0 ? "WHERE " + string.Join(" AND ", whereClauses) : "";

            // 2. Truy vấn KPI Tổng quan
            string kpiSql = $@"
                SELECT 
                    COUNT(*) AS TongSoLuot,
                    ISNULL(SUM(CASE WHEN lb.TrangThai = 'DaNop' THEN 1 ELSE 0 END), 0) AS SoLuotDaNop,
                    ISNULL(SUM(CASE WHEN lb.TrangThai = 'DangLam' THEN 1 ELSE 0 END), 0) AS SoLuotDangLam,
                    ISNULL(SUM(CASE WHEN lb.TrangThai = 'DaHuy' THEN 1 ELSE 0 END), 0) AS SoLuotDaHuy,
                    ISNULL(AVG(CASE WHEN lb.TrangThai = 'DaNop' THEN CAST(lb.DiemSo AS FLOAT) ELSE NULL END), 0.0) AS DiemTrungBinh,
                    ISNULL(AVG(CASE WHEN lb.TrangThai = 'DaNop' AND lb.ThoiGianLamGiay > 0 THEN CAST(lb.ThoiGianLamGiay AS FLOAT) ELSE NULL END), 0.0) AS ThoiGianTrungBinhGiay,
                    COUNT(DISTINCT lb.MaNguoiDung) AS SoHocVienThamGia
                FROM LuotLamBai lb
                INNER JOIN NguoiDung nd ON lb.MaNguoiDung = nd.MaNguoiDung
                LEFT JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                {whereSql}";

            try
            {
                DataTable dtKpi = SqlDataProvider.Instance.ExecuteQuery(kpiSql, CloneParameters(parameters));
                if (dtKpi.Rows.Count > 0)
                {
                    DataRow r = dtKpi.Rows[0];
                    report.TongSoLuotLam = Convert.ToInt32(r["TongSoLuot"]);
                    report.SoLuotDaNop = Convert.ToInt32(r["SoLuotDaNop"]);
                    report.SoLuotDangLam = Convert.ToInt32(r["SoLuotDangLam"]);
                    report.SoLuotDaHuy = Convert.ToInt32(r["SoLuotDaHuy"]);
                    report.DiemTrungBinh = Math.Round(Convert.ToDouble(r["DiemTrungBinh"]), 1);
                    report.ThoiGianTrungBinhGiay = Convert.ToInt32(r["ThoiGianTrungBinhGiay"]);
                    report.SoHocVienThamGia = Convert.ToInt32(r["SoHocVienThamGia"]);
                    report.TyLeHoanThanh = report.TongSoLuotLam > 0 ? Math.Round((report.SoLuotDaNop * 100.0) / report.TongSoLuotLam, 1) : 0.0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaoCaoThongKeDAL.LayBaoCaoThongKe] Lỗi KPI: {ex.Message}");
            }

            // 3. Phân bố điểm số (Chỉ tính lượt thi đã nộp)
            string phanBoSql = $@"
                SELECT 
                    ISNULL(SUM(CASE WHEN lb.DiemSo < 5.0 THEN 1 ELSE 0 END), 0) AS Duoi5,
                    ISNULL(SUM(CASE WHEN lb.DiemSo >= 5.0 AND lb.DiemSo < 6.5 THEN 1 ELSE 0 END), 0) AS Tu5Den65,
                    ISNULL(SUM(CASE WHEN lb.DiemSo >= 6.5 AND lb.DiemSo < 8.0 THEN 1 ELSE 0 END), 0) AS Tu65Den8,
                    ISNULL(SUM(CASE WHEN lb.DiemSo >= 8.0 THEN 1 ELSE 0 END), 0) AS Tren8
                FROM LuotLamBai lb
                INNER JOIN NguoiDung nd ON lb.MaNguoiDung = nd.MaNguoiDung
                LEFT JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                {(whereClauses.Count > 0 ? whereSql + " AND lb.TrangThai = 'DaNop'" : "WHERE lb.TrangThai = 'DaNop'")}";

            try
            {
                DataTable dtPhanBo = SqlDataProvider.Instance.ExecuteQuery(phanBoSql, CloneParameters(parameters));
                if (dtPhanBo.Rows.Count > 0)
                {
                    DataRow r = dtPhanBo.Rows[0];
                    int cDuoi5 = Convert.ToInt32(r["Duoi5"]);
                    int c5Den65 = Convert.ToInt32(r["Tu5Den65"]);
                    int c65Den8 = Convert.ToInt32(r["Tu65Den8"]);
                    int cTren8 = Convert.ToInt32(r["Tren8"]);

                    int totalSubmitted = report.SoLuotDaNop > 0 ? report.SoLuotDaNop : 1;
                    int maxCount = Math.Max(1, Math.Max(Math.Max(cDuoi5, c5Den65), Math.Max(c65Den8, cTren8)));

                    report.DanhSachPhanBoDiem.Add(new PhanBoDiemItem
                    {
                        KhoangDiem = "< 5.0 điểm",
                        NhanXet = "Chưa đạt chuẩn",
                        SoLuot = cDuoi5,
                        TyLePhanTram = Math.Round((cDuoi5 * 100.0) / totalSubmitted, 1),
                        MauSac = "#EF4444",
                        CotHeight = Math.Max(15.0, (cDuoi5 * 120.0) / maxCount)
                    });

                    report.DanhSachPhanBoDiem.Add(new PhanBoDiemItem
                    {
                        KhoangDiem = "5.0 - 6.4 điểm",
                        NhanXet = "Trung bình / Đạt",
                        SoLuot = c5Den65,
                        TyLePhanTram = Math.Round((c5Den65 * 100.0) / totalSubmitted, 1),
                        MauSac = "#F59E0B",
                        CotHeight = Math.Max(15.0, (c5Den65 * 120.0) / maxCount)
                    });

                    report.DanhSachPhanBoDiem.Add(new PhanBoDiemItem
                    {
                        KhoangDiem = "6.5 - 7.9 điểm",
                        NhanXet = "Khá / Tốt",
                        SoLuot = c65Den8,
                        TyLePhanTram = Math.Round((c65Den8 * 100.0) / totalSubmitted, 1),
                        MauSac = "#3B82F6",
                        CotHeight = Math.Max(15.0, (c65Den8 * 120.0) / maxCount)
                    });

                    report.DanhSachPhanBoDiem.Add(new PhanBoDiemItem
                    {
                        KhoangDiem = "8.0 - 10.0 điểm",
                        NhanXet = "Giỏi / Xuất sắc",
                        SoLuot = cTren8,
                        TyLePhanTram = Math.Round((cTren8 * 100.0) / totalSubmitted, 1),
                        MauSac = "#10B981",
                        CotHeight = Math.Max(15.0, (cTren8 * 120.0) / maxCount)
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaoCaoThongKeDAL.LayBaoCaoThongKe] Lỗi Phân bố điểm: {ex.Message}");
            }

            // 4. Xu hướng nộp bài theo ngày (Group by ngày)
            string xuHuongSql = $@"
                SELECT 
                    CAST(lb.ThoiDiemBatDau AS DATE) AS NgayLam,
                    COUNT(*) AS SoLuot,
                    ISNULL(AVG(CASE WHEN lb.TrangThai = 'DaNop' THEN CAST(lb.DiemSo AS FLOAT) ELSE NULL END), 0.0) AS DiemTB
                FROM LuotLamBai lb
                INNER JOIN NguoiDung nd ON lb.MaNguoiDung = nd.MaNguoiDung
                LEFT JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                {whereSql}
                GROUP BY CAST(lb.ThoiDiemBatDau AS DATE)
                ORDER BY NgayLam ASC";

            try
            {
                DataTable dtTrend = SqlDataProvider.Instance.ExecuteQuery(xuHuongSql, CloneParameters(parameters));
                int maxDailyCount = 1;
                foreach (DataRow r in dtTrend.Rows)
                {
                    int sl = Convert.ToInt32(r["SoLuot"]);
                    if (sl > maxDailyCount) maxDailyCount = sl;
                }

                foreach (DataRow r in dtTrend.Rows)
                {
                    DateTime d = Convert.ToDateTime(r["NgayLam"]);
                    int sl = Convert.ToInt32(r["SoLuot"]);
                    double dtb = Math.Round(Convert.ToDouble(r["DiemTB"]), 1);

                    report.DanhSachXuHuongNgay.Add(new XuHuongNgayItem
                    {
                        FullDate = d,
                        Ngay = d.ToString("dd/MM"),
                        SoLuot = sl,
                        DiemTrungBinh = dtb,
                        CotHeight = Math.Max(18.0, (sl * 110.0) / maxDailyCount)
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaoCaoThongKeDAL.LayBaoCaoThongKe] Lỗi Xu hướng ngày: {ex.Message}");
            }

            // 5. Thống kê theo kỹ năng từ câu trả lời thực tế (Tránh lỗi tích Descartes Cartesian khi join)
            string kyNangSql = $@"
                SELECT 
                    ISNULL(ch.KyNang, N'Khac') AS KyNang,
                    COUNT(*) AS TongSoCau,
                    ISNULL(SUM(CASE WHEN ct.KetQuaDungSai = 1 THEN 1 ELSE 0 END), 0) AS SoCauDung
                FROM ChiTietTraLoi ct
                INNER JOIN LuotLamBai lb ON ct.MaLuotLamBai = lb.MaLuotLamBai
                INNER JOIN CauHoi ch ON ct.MaCauHoi = ch.MaCauHoi
                INNER JOIN NguoiDung nd ON lb.MaNguoiDung = nd.MaNguoiDung
                INNER JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                {whereSql}
                GROUP BY ch.KyNang
                ORDER BY TongSoCau DESC";

            try
            {
                DataTable dtKn = SqlDataProvider.Instance.ExecuteQuery(kyNangSql, CloneParameters(parameters));
                foreach (DataRow r in dtKn.Rows)
                {
                    string kn = r["KyNang"]?.ToString() ?? "Doc";
                    int tong = Convert.ToInt32(r["TongSoCau"]);
                    int dung = Convert.ToInt32(r["SoCauDung"]);
                    double tile = tong > 0 ? Math.Round((dung * 100.0) / tong, 1) : 0.0;

                    string tenKn = kn switch
                    {
                        "Doc" => "Đọc hiểu (Reading)",
                        "Nghe" => "Nghe hiểu (Listening)",
                        "TuVung" => "Từ vựng (Vocabulary)",
                        "NguPhap" => "Ngữ pháp (Grammar)",
                        "Viet" => "Viết (Writing)",
                        _ => kn
                    };

                    string color = kn switch
                    {
                        "Doc" => "#2563EB",
                        "Nghe" => "#7C3AED",
                        "TuVung" => "#059669",
                        "NguPhap" => "#EA580C",
                        "Viet" => "#D97706",
                        _ => "#0F4C3A"
                    };

                    report.DanhSachKyNang.Add(new KyNangThongKeItem
                    {
                        KyNang = kn,
                        TenKyNang = tenKn,
                        TongSoCau = tong,
                        SoCauDung = dung,
                        TyLeDung = tile,
                        MauSac = color
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaoCaoThongKeDAL.LayBaoCaoThongKe] Lỗi Kỹ năng: {ex.Message}");
            }

            // 6. Danh sách chi tiết các lượt làm bài gần đây
            string listSql = $@"
                SELECT TOP 500
                    lb.MaLuotLamBai, lb.MaNguoiDung, nd.HoTen, nd.TenDangNhap,
                    lb.MaBaiTapDeThi, ISNULL(dt.TenBaiTapDeThi, N'Bài thi / Luyện tập') AS TenBaiTapDeThi, lb.HinhThucLam,
                    lb.ThoiDiemBatDau, lb.ThoiDiemNopBai, lb.ThoiGianLamGiay,
                    lb.TongSoCau, lb.SoCauDung, lb.SoCauSai, lb.DiemSo, lb.TrangThai,
                    ISNULL(dt.DiemDat, 5.0) AS DiemDat, ISNULL(dt.TongDiem, 10.0) AS TongDiem
                FROM LuotLamBai lb
                INNER JOIN NguoiDung nd ON lb.MaNguoiDung = nd.MaNguoiDung
                LEFT JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                {whereSql}
                ORDER BY lb.ThoiDiemBatDau DESC, lb.MaLuotLamBai DESC";

            try
            {
                DataTable dtList = SqlDataProvider.Instance.ExecuteQuery(listSql, CloneParameters(parameters));
                foreach (DataRow r in dtList.Rows)
                {
                    report.DanhSachLuotLamBai.Add(new LuotLamBaiDTO
                    {
                        MaLuotLamBai = Convert.ToInt32(r["MaLuotLamBai"]),
                        MaNguoiDung = Convert.ToInt32(r["MaNguoiDung"]),
                        HoTenNguoiDung = r["HoTen"]?.ToString() ?? "",
                        TenDangNhap = r["TenDangNhap"]?.ToString() ?? "",
                        MaBaiTapDeThi = Convert.ToInt32(r["MaBaiTapDeThi"]),
                        TenBaiTapDeThi = r["TenBaiTapDeThi"]?.ToString() ?? "",
                        HinhThucLam = r["HinhThucLam"]?.ToString() ?? "ThiThu",
                        ThoiDiemBatDau = Convert.ToDateTime(r["ThoiDiemBatDau"]),
                        ThoiDiemNopBai = r["ThoiDiemNopBai"] == DBNull.Value ? null : Convert.ToDateTime(r["ThoiDiemNopBai"]),
                        ThoiGianLamGiay = r["ThoiGianLamGiay"] == DBNull.Value ? 0 : Convert.ToInt32(r["ThoiGianLamGiay"]),
                        TongSoCau = Convert.ToInt32(r["TongSoCau"]),
                        SoCauDung = Convert.ToInt32(r["SoCauDung"]),
                        SoCauSai = Convert.ToInt32(r["SoCauSai"]),
                        DiemSo = r["DiemSo"] == DBNull.Value ? 0m : Convert.ToDecimal(r["DiemSo"]),
                        TrangThai = r["TrangThai"]?.ToString() ?? "DangLam",
                        DiemDat = r["DiemDat"] == DBNull.Value ? 5.0m : Convert.ToDecimal(r["DiemDat"]),
                        TongDiem = r["TongDiem"] == DBNull.Value ? 10.0m : Convert.ToDecimal(r["TongDiem"])
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaoCaoThongKeDAL.LayBaoCaoThongKe] Lỗi Danh sách: {ex.Message}");
            }

            return report;
        }

        public List<(int MaBaiTapDeThi, string TenBaiTapDeThi)> LayDanhSachDeThiFilter()
        {
            var list = new List<(int, string)>();
            string sql = "SELECT MaBaiTapDeThi, TenBaiTapDeThi FROM BaiTapDeThi ORDER BY TenBaiTapDeThi ASC";
            try
            {
                var dt = SqlDataProvider.Instance.ExecuteQuery(sql);
                foreach (DataRow r in dt.Rows)
                {
                    list.Add((Convert.ToInt32(r["MaBaiTapDeThi"]), r["TenBaiTapDeThi"]?.ToString() ?? ""));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaoCaoThongKeDAL.LayDanhSachDeThiFilter] {ex.Message}");
            }
            return list;
        }

        private SqlParameter[] CloneParameters(List<SqlParameter> parameters)
        {
            var arr = new SqlParameter[parameters.Count];
            for (int i = 0; i < parameters.Count; i++)
            {
                arr[i] = new SqlParameter(parameters[i].ParameterName, parameters[i].Value);
            }
            return arr;
        }
    }
}
