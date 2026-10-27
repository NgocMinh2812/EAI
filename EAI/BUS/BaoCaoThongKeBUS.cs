using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class BaoCaoThongKeBUS
    {
        private static BaoCaoThongKeBUS? _instance;
        public static BaoCaoThongKeBUS Instance => _instance ??= new BaoCaoThongKeBUS();

        public BaoCaoThongKeHocTapDTO LayBaoCaoThongKe(ThongKeHocTapFilterDTO filter)
        {
            return BaoCaoThongKeDAL.Instance.LayBaoCaoThongKe(filter);
        }

        public List<(int MaBaiTapDeThi, string TenBaiTapDeThi)> LayDanhSachDeThiFilter()
        {
            return BaoCaoThongKeDAL.Instance.LayDanhSachDeThiFilter();
        }

        /// <summary>
        /// Xuất báo cáo giám sát học tập ra tệp Excel (.xlsx) chuẩn OpenXML 2 Sheet thực thụ
        /// Sheet 1: Tổng quan chỉ số KPI và phân bổ năng lực
        /// Sheet 2: Chi tiết các lượt làm bài của học viên
        /// </summary>
        public bool XuatBaoCaoExcel(BaoCaoThongKeHocTapDTO report, string filePath, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var sheets = new List<OfficeFileHelper.ExcelSheetData>();

                // 1. SHEET 1: TỔNG QUAN KPI & NĂNG LỰC
                var sheetKpi = new OfficeFileHelper.ExcelSheetData
                {
                    SheetName = "TongQuan_KPI",
                    Headers = new List<string> { "Danh mục / Chỉ số", "Giá trị thống kê", "Đơn vị / Tỷ lệ", "Ghi chú phân tích" }
                };

                sheetKpi.Rows.Add(new List<string> { "Tổng số lượt tham gia thi", report.TongSoLuotLam.ToString(), "Lượt", "Bao gồm cả các bài thi chưa nộp" });
                sheetKpi.Rows.Add(new List<string> { "Số lượt thi đã hoàn thành và nộp bài", report.SoLuotDaNop.ToString(), "Lượt", "Đã ghi nhận điểm số chính thức" });
                sheetKpi.Rows.Add(new List<string> { "Tỷ lệ hoàn thành bài thi", report.TyLeHoanThanh.ToString("0.0") + "%", "%", "Tính trên tổng số lượt bắt đầu thi" });
                sheetKpi.Rows.Add(new List<string> { "Điểm số trung bình toàn hệ thống", report.DiemTrungBinh.ToString("0.0"), "Thang điểm 10", "Điểm trung bình các lượt đã nộp" });
                sheetKpi.Rows.Add(new List<string> { "Thời gian làm bài trung bình", (report.ThoiGianTrungBinhGiay / 60.0).ToString("0.0"), "Phút", "Thời gian từ bắt đầu tới khi nộp bài" });
                sheetKpi.Rows.Add(new List<string> { "Tổng số học viên tham gia học", report.SoHocVienThamGia.ToString(), "Học viên", "Số tài khoản duy nhất có phát sinh lượt thi" });

                // Dòng trống phân cách
                sheetKpi.Rows.Add(new List<string> { "", "", "", "" });
                sheetKpi.Rows.Add(new List<string> { "--- PHÂN BỔ ĐIỂM SỐ ---", "", "", "" });
                foreach (var pb in report.DanhSachPhanBoDiem)
                {
                    sheetKpi.Rows.Add(new List<string>
                    {
                        $"Thang điểm: {pb.KhoangDiem}",
                        pb.SoLuot.ToString(),
                        pb.TyLePhanTram.ToString("0.0") + "%",
                        pb.NhanXet
                    });
                }

                sheetKpi.Rows.Add(new List<string> { "", "", "", "" });
                sheetKpi.Rows.Add(new List<string> { "--- HIỆU SUẤT THEO KỸ NĂNG ---", "", "", "" });
                foreach (var kn in report.DanhSachKyNang)
                {
                    sheetKpi.Rows.Add(new List<string>
                    {
                        $"Kỹ năng: {kn.TenKyNang}",
                        kn.ChiTietText,
                        kn.TyLeDung.ToString("0.0") + "% đúng",
                        $"Tổng {kn.TongSoCau} câu hỏi được ghi nhận"
                    });
                }

                sheets.Add(sheetKpi);

                // 2. SHEET 2: CHI TIẾT CÁC LƯỢT LÀM BÀI
                var sheetDetails = new OfficeFileHelper.ExcelSheetData
                {
                    SheetName = "DanhSach_BaiNop",
                    Headers = new List<string>
                    {
                        "Mã lượt", "Họ và tên học viên", "Tài khoản", "Tên bài tập / Đề thi",
                        "Hình thức", "Bắt đầu lúc", "Nộp bài lúc", "Thời gian (phút)",
                        "Số câu đúng", "Tổng câu", "Điểm số", "Trạng thái"
                    }
                };

                foreach (var lb in report.DanhSachLuotLamBai)
                {
                    double phutLam = lb.ThoiGianLamGiay > 0
                        ? Math.Round(lb.ThoiGianLamGiay / 60.0, 1)
                        : (lb.ThoiDiemNopBai.HasValue ? Math.Round((lb.ThoiDiemNopBai.Value - lb.ThoiDiemBatDau).TotalMinutes, 1) : 0);

                    sheetDetails.Rows.Add(new List<string>
                    {
                        lb.MaLuotLamBai.ToString(),
                        lb.HoTenNguoiDung,
                        lb.TenDangNhap,
                        lb.TenBaiTapDeThi,
                        lb.HinhThucLam == "ThiThu" ? "Thi thử" : "Luyện tập",
                        lb.ThoiDiemBatDau.ToString("dd/MM/yyyy HH:mm:ss"),
                        lb.ThoiDiemNopBai?.ToString("dd/MM/yyyy HH:mm:ss") ?? "Chưa nộp",
                        phutLam.ToString("0.0"),
                        lb.SoCauDung.ToString(),
                        lb.TongSoCau.ToString(),
                        lb.DiemSo.ToString("0.0"),
                        lb.TrangThai == "DaNop" ? "Đã nộp bài" : (lb.TrangThai == "HetGio" ? "Hết giờ" : "Đang làm")
                    });
                }

                sheets.Add(sheetDetails);

                OfficeFileHelper.TaoTepExcel(filePath, sheets);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
