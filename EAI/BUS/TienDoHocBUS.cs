using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class TienDoHocBUS
    {
        private static TienDoHocBUS? _instance;
        public static TienDoHocBUS Instance => _instance ??= new TienDoHocBUS();

        /// <summary>
        /// Lấy toàn bộ thông tin KPI và tiến độ mục tiêu của học viên đang đăng nhập
        /// </summary>
        public TongKetTienDoDTO LayTongKetTienDo(int? maNguoiDung = null, string filter = "TatCa")
        {
            int userId = maNguoiDung ?? (SessionHelper.CurrentUser?.MaNguoiDung ?? 1);
            return TienDoHocDAL.Instance.LayTongKetTienDo(userId, filter);
        }

        /// <summary>
        /// Lấy danh sách điểm số theo từng lần thi để hiển thị bảng và vẽ biểu đồ đường
        /// </summary>
        public List<DiemThiTheoThoiGianDTO> LayLichSuDiemThi(int? maNguoiDung = null, string filter = "TatCa")
        {
            int userId = maNguoiDung ?? (SessionHelper.CurrentUser?.MaNguoiDung ?? 1);
            return TienDoHocDAL.Instance.LayLichSuDiemThi(userId, filter);
        }

        /// <summary>
        /// Lấy thống kê số giờ học phân bổ theo ngày cho biểu đồ cột
        /// </summary>
        public List<GioHocTheoNgayDTO> LayThongKeGioHoc(int? maNguoiDung = null, string filter = "TatCa")
        {
            int userId = maNguoiDung ?? (SessionHelper.CurrentUser?.MaNguoiDung ?? 1);
            return TienDoHocDAL.Instance.LayThongKeGioHocTheoNgay(userId, filter);
        }

        /// <summary>
        /// Tạo nội dung báo cáo tiến độ học tập dạng văn bản
        /// </summary>
        public string TaoBaoCaoVanBan(TongKetTienDoDTO tongKet, List<DiemThiTheoThoiGianDTO> lichSu)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("==================================================");
            sb.AppendLine("       ENGLISHAI - BÁO CÁO TIẾN ĐỘ HỌC TẬP");
            sb.AppendLine("==================================================");
            sb.AppendLine($"Học viên: {SessionHelper.CurrentUser?.HoTen ?? "Học viên EnglishAI"}");
            sb.AppendLine($"Tài khoản: {SessionHelper.CurrentUser?.TenDangNhap ?? "hocvien"}");
            sb.AppendLine($"Thời gian xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("1. TỔNG QUAN TIẾN ĐỘ & KPI:");
            sb.AppendLine($"- Tổng thời gian học tích lũy: {tongKet.TongGioHocText}");
            sb.AppendLine($"- Tổng số bài thi đã làm: {tongKet.TongSoBaiThi} bài");
            sb.AppendLine($"- Điểm trung bình: {tongKet.DiemTrungBinhText}");
            sb.AppendLine($"- Số bài ĐẠT yêu cầu: {tongKet.SoBaiDat} ({tongKet.TyLeDatText})");
            sb.AppendLine($"- Số bài CHƯA ĐẠT: {tongKet.SoBaiChuaDat} ({tongKet.TyLeChuaDat:0.0}%)");
            sb.AppendLine($"- Mục tiêu học tập: {tongKet.MucTieuText}");
            sb.AppendLine($"- Tiến độ hoàn thành mục tiêu: {tongKet.PhanTramMucTieuText} ({tongKet.MoTaTienDoMucTieu})");
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("2. LỊCH SỬ CÁC LẦN THI:");
            if (lichSu.Count == 0)
            {
                sb.AppendLine("  (Chưa có dữ liệu bài thi nào được ghi nhận)");
            }
            else
            {
                foreach (var item in lichSu)
                {
                    sb.AppendLine($"  [{item.LanThiLabel}] {item.NgayNopText} | {item.TenDeThi}");
                    sb.AppendLine($"      Điểm số: {item.DiemHienThi} - Kết quả: {item.TrangThaiText} ({item.SoCauDungText} câu đúng)");
                }
            }
            sb.AppendLine("==================================================");
            return sb.ToString();
        }
    }
}

