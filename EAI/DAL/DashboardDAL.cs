using System;
using System.Collections.Generic;
using System.Data;
using DTO;

namespace DAL
{
    public class DashboardDAL
    {
        public DashboardKpiDTO GetKpiStats(int filterPeriod = 0)
        {
            var kpi = new DashboardKpiDTO();

            string currentFilter;
            string previousFilter;
            string trendLabel;

            switch (filterPeriod)
            {
                case 1: // 7 ngày qua
                    currentFilter = ">= DATEADD(day, -7, GETDATE())";
                    previousFilter = ">= DATEADD(day, -14, GETDATE()) AND {0} < DATEADD(day, -7, GETDATE())";
                    trendLabel = "so với tuần trước";
                    break;
                case 2: // Tháng này
                    currentFilter = ">= DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0)";
                    previousFilter = ">= DATEADD(month, DATEDIFF(month, 0, GETDATE()) - 1, 0) AND {0} < DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0)";
                    trendLabel = "so với tháng trước";
                    break;
                case 0: // Hôm nay
                default:
                    currentFilter = ">= CAST(GETDATE() AS DATE)";
                    previousFilter = ">= DATEADD(day, -1, CAST(GETDATE() AS DATE)) AND {0} < CAST(GETDATE() AS DATE)";
                    trendLabel = "so với hôm qua";
                    break;
            }

            kpi.TenKyThongKe = trendLabel;

            string query = $@"
                SELECT 
                    -- 4 KPI CHÍNH
                    (SELECT COUNT(*) FROM NguoiDung nd INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro WHERE vt.TenVaiTro = N'HocVien') AS TongHocVien,
                    (SELECT COUNT(*) FROM BaiHoc) AS TongBaiHoc,
                    (SELECT COUNT(*) FROM CauHoi) AS TongCauHoi,
                    (ISNULL((SELECT COUNT(*) FROM LuotLamBai), 0) + ISNULL((SELECT COUNT(*) FROM LuotKhaoSat), 0) + ISNULL((SELECT COUNT(*) FROM KetQuaLamBaiBoCauHoi), 0)) AS TongLuotThi,

                    -- Tăng trưởng theo chu kỳ chọn lọc
                    (SELECT COUNT(*) FROM NguoiDung nd INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro WHERE vt.TenVaiTro = N'HocVien' AND nd.NgayTao {currentFilter}) AS HocVienMoiTuanNay,
                    (SELECT COUNT(*) FROM NguoiDung nd INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro WHERE vt.TenVaiTro = N'HocVien' AND nd.NgayTao {string.Format(previousFilter, "nd.NgayTao")}) AS HocVienMoiTuanTruoc,

                    (SELECT COUNT(*) FROM BaiHoc WHERE NgayTao {currentFilter}) AS BaiHocMoiTuanNay,
                    (SELECT COUNT(*) FROM BaiHoc WHERE NgayTao {string.Format(previousFilter, "NgayTao")}) AS BaiHocMoiTuanTruoc,

                    (SELECT COUNT(*) FROM CauHoi WHERE NgayTao {currentFilter}) AS CauHoiMoiTuanNay,
                    (SELECT COUNT(*) FROM CauHoi WHERE NgayTao {string.Format(previousFilter, "NgayTao")}) AS CauHoiMoiTuanTruoc,

                    (
                        ISNULL((SELECT COUNT(*) FROM LuotLamBai WHERE ThoiDiemBatDau {currentFilter}), 0) +
                        ISNULL((SELECT COUNT(*) FROM LuotKhaoSat WHERE ThoiDiemBatDau {currentFilter}), 0) +
                        ISNULL((SELECT COUNT(*) FROM KetQuaLamBaiBoCauHoi WHERE NgayNop {currentFilter}), 0)
                    ) AS LuotThiTuanNay,
                    (
                        ISNULL((SELECT COUNT(*) FROM LuotLamBai WHERE ThoiDiemBatDau {string.Format(previousFilter, "ThoiDiemBatDau")}), 0) +
                        ISNULL((SELECT COUNT(*) FROM LuotKhaoSat WHERE ThoiDiemBatDau {string.Format(previousFilter, "ThoiDiemBatDau")}), 0) +
                        ISNULL((SELECT COUNT(*) FROM KetQuaLamBaiBoCauHoi WHERE NgayNop {string.Format(previousFilter, "NgayNop")}), 0)
                    ) AS LuotThiTuanTruoc,

                    -- CÁC CHỈ SỐ KHÁC
                    (SELECT COUNT(*) FROM BaiTapDeThi WHERE LoaiHinh = N'ThiThu') AS TongDeThi,
                    (SELECT COUNT(*) FROM BaiTapDeThi WHERE LoaiHinh = N'LuyenTap') AS TongLuyenTap,
                    (ISNULL((SELECT COUNT(*) FROM CauHoiDoAISinh WHERE TrangThaiDuyet = N'DangCho'), 0) + ISNULL((SELECT COUNT(*) FROM CauHoi WHERE DaKiemDuyet = 0), 0)) AS ChoDuyet,
                    (SELECT COUNT(*) FROM NguoiDung) AS TongND,
                    (SELECT ISNULL(SUM(CASE WHEN TrangThaiHoatDong = 1 THEN 1 ELSE 0 END), 0) FROM NguoiDung) AS ActiveND,
                    (SELECT ISNULL(SUM(CASE WHEN TrangThaiHoatDong = 0 THEN 1 ELSE 0 END), 0) FROM NguoiDung) AS LockedND,
                    (SELECT COUNT(*) FROM ChuDe) AS TongChuDe;";

            try
            {
                var dt = DatabaseHelper.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    kpi.TongHocVien = r["TongHocVien"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongHocVien"]);
                    kpi.TongBaiHoc = r["TongBaiHoc"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongBaiHoc"]);
                    kpi.TongCauHoi = r["TongCauHoi"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongCauHoi"]);
                    kpi.TongLuotThi = r["TongLuotThi"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongLuotThi"]);

                    // Tính % tăng trưởng và số lượng tăng mới
                    int hvNay = Convert.ToInt32(r["HocVienMoiTuanNay"] == DBNull.Value ? 0 : r["HocVienMoiTuanNay"]);
                    int hvTruoc = Convert.ToInt32(r["HocVienMoiTuanTruoc"] == DBNull.Value ? 0 : r["HocVienMoiTuanTruoc"]);
                    kpi.HocVienMoi = hvNay;
                    kpi.TangTruongHocVien = TinhPhanTramTangTruong(hvNay, hvTruoc, baseline: 12.5);

                    int bhNay = Convert.ToInt32(r["BaiHocMoiTuanNay"] == DBNull.Value ? 0 : r["BaiHocMoiTuanNay"]);
                    int bhTruoc = Convert.ToInt32(r["BaiHocMoiTuanTruoc"] == DBNull.Value ? 0 : r["BaiHocMoiTuanTruoc"]);
                    kpi.BaiHocMoi = bhNay;
                    kpi.TangTruongBaiHoc = TinhPhanTramTangTruong(bhNay, bhTruoc, baseline: 8.3);

                    int chNay = Convert.ToInt32(r["CauHoiMoiTuanNay"] == DBNull.Value ? 0 : r["CauHoiMoiTuanNay"]);
                    int chTruoc = Convert.ToInt32(r["CauHoiMoiTuanTruoc"] == DBNull.Value ? 0 : r["CauHoiMoiTuanTruoc"]);
                    kpi.CauHoiMoi = chNay;
                    kpi.TangTruongCauHoi = TinhPhanTramTangTruong(chNay, chTruoc, baseline: 15.0);

                    int ltNay = Convert.ToInt32(r["LuotThiTuanNay"] == DBNull.Value ? 0 : r["LuotThiTuanNay"]);
                    int ltTruoc = Convert.ToInt32(r["LuotThiTuanTruoc"] == DBNull.Value ? 0 : r["LuotThiTuanTruoc"]);
                    kpi.LuotThiMoi = ltNay;
                    kpi.TangTruongLuotThi = TinhPhanTramTangTruong(ltNay, ltTruoc, baseline: 24.8);

                    kpi.TongDeThi = r["TongDeThi"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongDeThi"]);
                    kpi.TongLuyenTap = r["TongLuyenTap"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongLuyenTap"]);
                    kpi.ChoDuyet = r["ChoDuyet"] == DBNull.Value ? 0 : Convert.ToInt32(r["ChoDuyet"]);
                    kpi.TongNguoiDung = r["TongND"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongND"]);
                    kpi.DangHoatDong = r["ActiveND"] == DBNull.Value ? 0 : Convert.ToInt32(r["ActiveND"]);
                    kpi.DaKhoa = r["LockedND"] == DBNull.Value ? 0 : Convert.ToInt32(r["LockedND"]);
                    kpi.TongChuDe = r["TongChuDe"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongChuDe"]);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DashboardDAL] Lỗi lấy số liệu KPI: {ex.Message}");
                // Khi có lỗi CSDL, không dùng số liệu bịa đặt; gán 0 để UI hiển thị trung thực trạng thái
                kpi.TongHocVien = 0;
                kpi.TangTruongHocVien = 0;
                kpi.TongBaiHoc = 0;
                kpi.TangTruongBaiHoc = 0;
                kpi.TongCauHoi = 0;
                kpi.TangTruongCauHoi = 0;
                kpi.TongLuotThi = 0;
                kpi.TangTruongLuotThi = 0;
            }

            // Tải dữ liệu xu hướng học tập (7 ngày qua)
            kpi.XuHuongHocTap = LayDuLieuXuHuongHocTap();

            // Tải danh sách hoạt động gần đây
            kpi.DanhSachHoatDong = LayDanhSachHoatDongGanDay();

            // Tải số liệu nhóm học theo trình độ
            LaySoLieuNhomHoc(kpi);

            // Tải dữ liệu bài học & đề thi nổi bật từ CSDL
            LayDuLieuNoiDungNoiBat(kpi);

            return kpi;
        }

        private static double TinhPhanTramTangTruong(int current, int previous, double baseline)
        {
            if (previous > 0)
            {
                return Math.Round(((double)(current - previous) / previous) * 100.0, 1);
            }
            if (current > 0)
            {
                return 100.0;
            }
            return 0.0;
        }

        public List<TrendPointDTO> LayDuLieuXuHuongHocTap()
        {
            var list = new List<TrendPointDTO>();
            var today = DateTime.Today;

            // Truy vấn số lượt thi hoặc bài học được học thực tế trong 7 ngày gần nhất
            var dailyCounts = new Dictionary<string, int>();
            try
            {
                string query = @"
                    SELECT 
                        CONVERT(VARCHAR(10), NgayThucHien, 120) AS Ngay,
                        COUNT(*) AS SoLuot
                    FROM (
                        SELECT CAST(ThoiDiemBatDau AS DATE) AS NgayThucHien FROM LuotLamBai WHERE ThoiDiemBatDau IS NOT NULL
                        UNION ALL
                        SELECT CAST(ThoiDiemBatDau AS DATE) AS NgayThucHien FROM LuotKhaoSat WHERE ThoiDiemBatDau IS NOT NULL
                        UNION ALL
                        SELECT CAST(NgayNop AS DATE) AS NgayThucHien FROM KetQuaLamBaiBoCauHoi WHERE NgayNop IS NOT NULL
                        UNION ALL
                        SELECT CAST(NgayGhiNhan AS DATE) AS NgayThucHien FROM LichSuHocTap WHERE NgayGhiNhan IS NOT NULL
                    ) AS TongHopHoatDong
                    WHERE NgayThucHien >= DATEADD(day, -6, CAST(GETDATE() AS DATE))
                    GROUP BY CONVERT(VARCHAR(10), NgayThucHien, 120)";

                var dt = DatabaseHelper.ExecuteQuery(query);
                foreach (DataRow r in dt.Rows)
                {
                    string d = r["Ngay"]?.ToString() ?? "";
                    int count = Convert.ToInt32(r["SoLuot"] ?? 0);
                    dailyCounts[d] = count;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DashboardDAL] Lỗi lấy xu hướng học tập: {ex.Message}");
            }

            // Tìm giá trị lớn nhất trong 7 ngày thực tế để tính tỉ lệ cột
            int maxCount = 1;
            for (int i = 6; i >= 0; i--)
            {
                var dtDate = today.AddDays(-i);
                string dateKey = dtDate.ToString("yyyy-MM-dd");
                int val = dailyCounts.ContainsKey(dateKey) ? dailyCounts[dateKey] : 0;
                if (val > maxCount) maxCount = val;
            }

            // Tạo danh sách 7 ngày từ 6 ngày trước đến hôm nay
            for (int i = 6; i >= 0; i--)
            {
                var dtDate = today.AddDays(-i);
                string dateKey = dtDate.ToString("yyyy-MM-dd");
                string dayOfWeek = dtDate.DayOfWeek switch
                {
                    DayOfWeek.Monday => "T2",
                    DayOfWeek.Tuesday => "T3",
                    DayOfWeek.Wednesday => "T4",
                    DayOfWeek.Thursday => "T5",
                    DayOfWeek.Friday => "T6",
                    DayOfWeek.Saturday => "T7",
                    _ => "CN"
                };

                // Lấy số liệu thực tế từ CSDL (nếu không có thì = 0)
                int val = dailyCounts.ContainsKey(dateKey) ? dailyCounts[dateKey] : 0;

                // Chiều cao cột: nếu bằng 0 thì tối thiểu 6px để hiển thị đế cột, nếu > 0 thì scale tỉ lệ max 105px
                double cotHeight = val == 0 ? 6.0 : Math.Max(20.0, Math.Min(105.0, ((double)val / maxCount) * 95.0));

                list.Add(new TrendPointDTO
                {
                    NgayTrongTuan = dayOfWeek,
                    NgayThang = dtDate.ToString("dd/MM"),
                    SoLuotLamBai = val,
                    CotHeight = cotHeight,
                    CotColor = val == 0 ? "#E2E8F0" : (i == 0 ? "#0F766E" : "#10B981")
                });
            }

            return list;
        }

        public List<HoatDongGanDayDTO> LayDanhSachHoatDongGanDay()
        {
            var list = new List<HoatDongGanDayDTO>();

            try
            {
                string query = @"
                    SELECT TOP 6
                        HoatDong.MaNguoiDung,
                        nd.HoTen,
                        HoatDong.TenBai,
                        HoatDong.LoaiHoatDong,
                        HoatDong.ThoiDiem,
                        HoatDong.DiemSo,
                        HoatDong.NoiDung,
                        HoatDong.TrangThai
                    FROM (
                        SELECT 
                            ks.MaNguoiDung,
                            ISNULL(bks.TenBaiKhaoSat, N'Khảo sát năng lực đầu vào') AS TenBai,
                            'KhaoSat' AS LoaiHoatDong,
                            ks.ThoiDiemNopBai AS ThoiDiem,
                            ks.DiemDatDuoc AS DiemSo,
                            N'Đã hoàn thành • Điểm: ' + CAST(CAST(ks.DiemDatDuoc AS DECIMAL(4,1)) AS NVARCHAR) + N' / 10' AS NoiDung,
                            CASE WHEN ks.DiemDatDuoc >= 5.0 THEN N'Đã nộp' ELSE N'Cần ôn tập' END AS TrangThai
                        FROM LuotKhaoSat ks
                        LEFT JOIN BaiKhaoSatDauVao bks ON ks.MaBaiKhaoSat = bks.MaBaiKhaoSat

                        UNION ALL

                        SELECT 
                            llb.MaNguoiDung,
                            ISNULL(bt.TenBaiTapDeThi, N'Đề thi trắc nghiệm') AS TenBai,
                            ISNULL(bt.LoaiHinh, 'ThiThu') AS LoaiHoatDong,
                            llb.ThoiDiemNopBai AS ThoiDiem,
                            llb.DiemSo AS DiemSo,
                            N'Đã hoàn thành • Điểm: ' + CAST(CAST(llb.DiemSo AS DECIMAL(4,1)) AS NVARCHAR) + N' / 10' AS NoiDung,
                            CASE WHEN llb.DiemSo >= 5.0 THEN N'Đã nộp' ELSE N'Cần ôn tập' END AS TrangThai
                        FROM LuotLamBai llb
                        LEFT JOIN BaiTapDeThi bt ON llb.MaBaiTapDeThi = bt.MaBaiTapDeThi

                        UNION ALL

                        SELECT 
                            kq.MaNguoiDung,
                            N'Bộ câu hỏi: ' + ISNULL(kq.LoaiBoCauHoi, N'Luyện tập') AS TenBai,
                            'LuyenTap' AS LoaiHoatDong,
                            kq.NgayNop AS ThoiDiem,
                            kq.DiemSo AS DiemSo,
                            N'Đã hoàn thành • ' + CAST(kq.SoCauDung AS NVARCHAR) + N'/' + CAST(kq.TongSoCau AS NVARCHAR) + N' câu đúng' AS NoiDung,
                            CASE WHEN kq.DiemSo >= 5.0 THEN N'Đã nộp' ELSE N'Cần ôn tập' END AS TrangThai
                        FROM KetQuaLamBaiBoCauHoi kq
                    ) AS HoatDong
                    INNER JOIN NguoiDung nd ON HoatDong.MaNguoiDung = nd.MaNguoiDung
                    ORDER BY HoatDong.ThoiDiem DESC";

                var dt = DatabaseHelper.ExecuteQuery(query);
                foreach (DataRow r in dt.Rows)
                {
                    string hoTen = r["HoTen"]?.ToString() ?? "Học viên";
                    string tenBai = r["TenBai"]?.ToString() ?? "Bài luyện tập";
                    string loaiHoatDong = r["LoaiHoatDong"]?.ToString() ?? "KhaoSat";
                    decimal diem = r["DiemSo"] == DBNull.Value ? 0 : Convert.ToDecimal(r["DiemSo"]);
                    DateTime? time = r["ThoiDiem"] == DBNull.Value ? null : Convert.ToDateTime(r["ThoiDiem"]);
                    string noiDung = r["NoiDung"]?.ToString() ?? $"Đã hoàn thành • Điểm: {diem:F1} / 10";
                    string trangThai = r["TrangThai"]?.ToString() ?? (diem >= 5.0m ? "Đã nộp" : "Cần ôn tập");

                    string timeStr = "Vừa xong";
                    if (time.HasValue)
                    {
                        var span = DateTime.Now - time.Value;
                        if (span.TotalMinutes < 60) timeStr = $"{(int)Math.Max(1, span.TotalMinutes)} phút trước";
                        else if (span.TotalHours < 24) timeStr = $"{(int)span.TotalHours} giờ trước";
                        else timeStr = time.Value.ToString("dd/MM/yyyy HH:mm");
                    }

                    list.Add(new HoatDongGanDayDTO
                    {
                        MaNguoiDung = Convert.ToInt32(r["MaNguoiDung"]),
                        HoTen = hoTen,
                        AvatarInitials = LayInitials(hoTen),
                        AvatarBg = diem >= 5.0m ? "#DCFCE7" : "#FEE2E2",
                        AvatarFg = diem >= 5.0m ? "#15803D" : "#991B1B",
                        TieuDe = tenBai,
                        NoiDung = noiDung,
                        ThoiGian = timeStr,
                        DiemSo = $"{diem:F1}",
                        LoaiHoatDong = loaiHoatDong,
                        TrangThai = trangThai,
                        TrangThaiBg = diem >= 5.0m ? "#DCFCE7" : "#FEF3C7",
                        TrangThaiFg = diem >= 5.0m ? "#15803D" : "#B45309"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DashboardDAL] Lỗi lấy hoạt động gần đây: {ex.Message}");
            }

            return list;
        }

        private static string LayInitials(string hoTen)
        {
            if (string.IsNullOrWhiteSpace(hoTen)) return "HV";
            var parts = hoTen.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
            return $"{parts[^2][0]}{parts[^1][0]}".ToUpper();
        }

        private void LaySoLieuNhomHoc(DashboardKpiDTO kpi)
        {
            try
            {
                // Thống kê số lượng học viên thực tế theo từng cấp độ kết quả khảo sát
                string query = @"
                    SELECT 
                        ISNULL(SUM(CASE WHEN MaTrinhDoKetQua IN (0, 1) THEN 1 ELSE 0 END), 0) AS NhomA,
                        ISNULL(SUM(CASE WHEN MaTrinhDoKetQua = 2 THEN 1 ELSE 0 END), 0) AS NhomB1,
                        ISNULL(SUM(CASE WHEN MaTrinhDoKetQua = 3 THEN 1 ELSE 0 END), 0) AS NhomB2,
                        ISNULL(SUM(CASE WHEN MaTrinhDoKetQua IN (4, 5) THEN 1 ELSE 0 END), 0) AS NhomC
                    FROM LuotKhaoSat";

                var dt = DatabaseHelper.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    var r = dt.Rows[0];
                    int a = Convert.ToInt32(r["NhomA"] == DBNull.Value ? 0 : r["NhomA"]);
                    int b1 = Convert.ToInt32(r["NhomB1"] == DBNull.Value ? 0 : r["NhomB1"]);
                    int b2 = Convert.ToInt32(r["NhomB2"] == DBNull.Value ? 0 : r["NhomB2"]);
                    int c = Convert.ToInt32(r["NhomC"] == DBNull.Value ? 0 : r["NhomC"]);

                    kpi.NhomA1Count = a;
                    kpi.NhomB1Count = b1;
                    kpi.NhomB2Count = b2;
                    kpi.NhomC1Count = c;
                }
            }
            catch
            {
                kpi.NhomA1Count = 0;
                kpi.NhomB1Count = 0;
                kpi.NhomB2Count = 0;
                kpi.NhomC1Count = 0;
            }
        }

        private void LayDuLieuNoiDungNoiBat(DashboardKpiDTO kpi)
        {
            try
            {
                // Lấy 2 bài học thực tế từ CSDL
                string queryBH = @"
                    SELECT TOP 2 
                        bh.MaBaiHoc, 
                        bh.TieuDe, 
                        ISNULL(bh.TomTat, N'Nội dung tổng hợp rèn luyện kỹ năng ngữ pháp, từ vựng và bài tập áp dụng thực tế.') AS TomTat, 
                        ISNULL(bh.LoaiBaiHoc, N'Ngữ pháp') AS LoaiBaiHoc, 
                        ISNULL(td.MaCapDo, N'B1') AS MaCapDo, 
                        ISNULL(bh.DaXuatBan, 0) AS DaXuatBan
                    FROM BaiHoc bh
                    LEFT JOIN TrinhDo td ON bh.MaTrinhDo = td.MaTrinhDo
                    ORDER BY bh.MaBaiHoc DESC";

                var dtBH = DatabaseHelper.ExecuteQuery(queryBH);
                if (dtBH.Rows.Count > 0)
                {
                    var r0 = dtBH.Rows[0];
                    kpi.BaiHoc1 = new DashboardItemDTO
                    {
                        MaId = Convert.ToInt32(r0["MaBaiHoc"]),
                        MaCode = $"BH-{Convert.ToInt32(r0["MaBaiHoc"]):D3}",
                        TieuDe = r0["TieuDe"]?.ToString() ?? "Thì Hiện Tại Hoàn Thành — Cấu trúc & Cách dùng",
                        TomTat = r0["TomTat"]?.ToString() ?? "",
                        LoaiHinh = r0["LoaiBaiHoc"]?.ToString() ?? "Ngữ pháp",
                        TrinhDo = r0["MaCapDo"]?.ToString() ?? "B1",
                        TrangThai = Convert.ToInt32(r0["DaXuatBan"] == DBNull.Value ? 0 : r0["DaXuatBan"]) == 1 ? "Đã xuất bản" : "Bản nháp",
                        LoaiBadgeColor = "#10B981"
                    };

                    if (dtBH.Rows.Count > 1)
                    {
                        var r1 = dtBH.Rows[1];
                        kpi.BaiHoc2 = new DashboardItemDTO
                        {
                            MaId = Convert.ToInt32(r1["MaBaiHoc"]),
                            MaCode = $"BH-{Convert.ToInt32(r1["MaBaiHoc"]):D3}",
                            TieuDe = r1["TieuDe"]?.ToString() ?? "Từ vựng Kinh tế — Business English cho người đi làm",
                            TomTat = r1["TomTat"]?.ToString() ?? "",
                            LoaiHinh = r1["LoaiBaiHoc"]?.ToString() ?? "Từ vựng",
                            TrinhDo = r1["MaCapDo"]?.ToString() ?? "B2",
                            TrangThai = Convert.ToInt32(r1["DaXuatBan"] == DBNull.Value ? 0 : r1["DaXuatBan"]) == 1 ? "Đã xuất bản" : "Bản nháp",
                            LoaiBadgeColor = "#0284C7"
                        };
                    }
                }

                // Lấy 1 đề thi thực tế từ CSDL
                string queryDT = @"
                    SELECT TOP 1 
                        bt.MaBaiTapDeThi, 
                        bt.TenBaiTapDeThi, 
                        ISNULL(bt.MoTa, N'Đề thi thử dạng đầy đủ, kiểm tra toàn diện các kỹ năng với hệ thống tự động chấm điểm và đánh giá chi tiết.') AS MoTa, 
                        ISNULL(bt.LoaiHinh, N'Thi thử') AS LoaiHinh, 
                        ISNULL(td.MaCapDo, N'B2') AS MaCapDo, 
                        ISNULL(bt.DaPhatHanh, 1) AS DaPhatHanh
                    FROM BaiTapDeThi bt
                    LEFT JOIN TrinhDo td ON bt.MaTrinhDo = td.MaTrinhDo
                    ORDER BY bt.MaBaiTapDeThi DESC";

                var dtDT = DatabaseHelper.ExecuteQuery(queryDT);
                if (dtDT.Rows.Count > 0)
                {
                    var rd = dtDT.Rows[0];
                    kpi.DeThi1 = new DashboardItemDTO
                    {
                        MaId = Convert.ToInt32(rd["MaBaiTapDeThi"]),
                        MaCode = $"DE-{Convert.ToInt32(rd["MaBaiTapDeThi"]):D3}",
                        TieuDe = rd["TenBaiTapDeThi"]?.ToString() ?? "IELTS Mock Test #1 — Academic Reading & Listening",
                        TomTat = rd["MoTa"]?.ToString() ?? "",
                        LoaiHinh = rd["LoaiHinh"]?.ToString() ?? "Thi thử",
                        TrinhDo = rd["MaCapDo"]?.ToString() ?? "B2",
                        TrangThai = Convert.ToInt32(rd["DaPhatHanh"] == DBNull.Value ? 1 : rd["DaPhatHanh"]) == 1 ? "Đã xuất bản" : "Bản nháp",
                        LoaiBadgeColor = "#8B5CF6"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DashboardDAL] Lỗi lấy bài học nổi bật: {ex.Message}");
            }
        }
    }
}
