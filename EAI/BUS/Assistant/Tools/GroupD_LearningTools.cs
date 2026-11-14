using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS;
using BUS.Assistant;
using DAL;
using DTO;

namespace BUS.Assistant.Tools
{
    /// <summary>
    /// Công cụ tra cứu tiến độ học tập bài học cụ thể hoặc tổng quan quá trình học tập của học viên.
    /// Có cơ chế phòng chống IDOR: Học viên chỉ tra cứu được dữ liệu của chính mình.
    /// </summary>
    public class GetLessonProgressTool : IAssistantTool
    {
        public string Name => "get_lesson_progress";
        public string Description => "Tra cứu tiến độ học tập bài học cụ thể hoặc tổng quan quá trình học tập của học viên.";
        public string Category => "Learning";
        public string RequiredRole => AssistantPermissionService.RoleStudent; // Cho phép SinhVien, GiangVien, QuanTriVien
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("maBaiHoc", "int", "Mã bài học cụ thể cần kiểm tra tiến độ (bỏ trống để xem tổng quan)", isRequired: false),
            new("maNguoiDung", "int", "Mã học viên cần tra cứu (chỉ Giảng viên/Quản trị viên được xem học viên khác)", isRequired: false),
            new("top", "int", "Số lượng bài học gần đây khi xem tổng quát (mặc định: 5)", isRequired: false, defaultValue: 5)
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            try
            {
                // 1. Chống lỗ hổng IDOR: Sinh viên chỉ được tra cứu của chính mình
                int targetUserId = context.UserId;
                if ((context.IsAdmin || context.IsLecturer) && arguments.TryGetValue("maNguoiDung", out var uVal) && uVal != null)
                {
                    if (int.TryParse(uVal.ToString(), out int parsedUid) && parsedUid > 0)
                    {
                        targetUserId = parsedUid;
                    }
                }

                if (targetUserId <= 0)
                {
                    return Task.FromResult(AssistantToolResult.Fail("Không xác định được danh tính học viên cần tra cứu.", "USER_NOT_IDENTIFIED"));
                }

                // 2. Kiểm tra xem người dùng yêu cầu bài học cụ thể hay xem tổng quan
                int maBaiHoc = ToolArgumentHelper.GetInt(arguments, "maBaiHoc", 0);

                if (maBaiHoc > 0)
                {
                    // Tra cứu tiến độ của bài học cụ thể
                    var allLessons = BaiHocDAL.Instance.LayDanhSachBaiHoc(targetUserId);
                    var lesson = allLessons?.FirstOrDefault(b => b.MaBaiHoc == maBaiHoc);

                    if (lesson == null)
                    {
                        // Nếu chưa phát sinh bản ghi tiến độ, lấy thông tin bài học từ CSDL
                        var dbLessons = BaiHocBUS.LayDanhSachTuDatabase();
                        var fallbackLesson = dbLessons.FirstOrDefault(b => b.MaBaiHoc == maBaiHoc);

                        if (fallbackLesson == null)
                        {
                            return Task.FromResult(AssistantToolResult.Fail($"Không tìm thấy bài học có mã #{maBaiHoc} trong hệ thống.", "LESSON_NOT_FOUND"));
                        }

                        var notStartedPayload = new
                        {
                            user_id = targetUserId,
                            ma_bai_hoc = fallbackLesson.MaBaiHoc,
                            tieu_de = fallbackLesson.TieuDe,
                            loai_bai_hoc = fallbackLesson.LoaiBaiHoc,
                            chu_de = fallbackLesson.TenChuDe,
                            trinh_do = fallbackLesson.MaTrinhDo,
                            phan_tram_tien_do = 0,
                            trang_thai = "Chưa học",
                            diem_so = 0,
                            ngay_hoc_gan_nhat = "Chưa học",
                            da_hoan_thanh = false
                        };

                        string notStartedMsg = $"Tiến độ bài học '{fallbackLesson.TieuDe}' (ID: #{fallbackLesson.MaBaiHoc}): 0% - Trạng thái: Chưa học.";
                        return Task.FromResult(AssistantToolResult.OkWithJsonPayload(notStartedMsg, notStartedPayload, "lesson_progress_card"));
                    }

                    var lessonProgressPayload = new
                    {
                        user_id = targetUserId,
                        ma_bai_hoc = lesson.MaBaiHoc,
                        tieu_de = lesson.TieuDe,
                        loai_bai_hoc = lesson.LoaiBaiHoc,
                        chu_de = lesson.TenChuDe,
                        trinh_do = lesson.MaTrinhDo,
                        phan_tram_tien_do = lesson.PhanTramTienDo,
                        trang_thai = lesson.TrangThai,
                        diem_so = lesson.DiemSo,
                        ngay_hoc_gan_nhat = lesson.NgayHocGanNhat,
                        da_hoan_thanh = lesson.TrangThai == "Hoàn thành" || lesson.PhanTramTienDo >= 100
                    };

                    string msg = $"Tiến độ bài học '{lesson.TieuDe}' (ID: #{lesson.MaBaiHoc}): {lesson.PhanTramTienDo}% - Trạng thái: {lesson.TrangThai}. Điểm số ghi nhận: {lesson.DiemSo}.";
                    return Task.FromResult(AssistantToolResult.OkWithJsonPayload(msg, lessonProgressPayload, "lesson_progress_card"));
                }
                else
                {
                    // Tra cứu tổng quan tiến độ học tập
                    int top = Math.Clamp(ToolArgumentHelper.GetInt(arguments, "top", 5), 1, 50);

                    var thongKe = BaiHocDAL.Instance.LayThongKeSinhVien(targetUserId);
                    var dsGanDay = BaiHocDAL.Instance.LayDanhSachBaiHocGanDay(targetUserId, top);

                    double tyLeHoanThanh = thongKe.TongSoBaiHoc > 0 
                        ? Math.Round((double)thongKe.SoBaiDaHoanThanh * 100.0 / thongKe.TongSoBaiHoc, 1) 
                        : 0.0;

                    var summaryPayload = new
                    {
                        user_id = targetUserId,
                        so_bai_da_hoan_thanh = thongKe.SoBaiDaHoanThanh,
                        tong_so_bai_hoc = thongKe.TongSoBaiHoc,
                        ty_le_hoan_thanh = tyLeHoanThanh,
                        so_phut_hoc_tich_luy = thongKe.SoPhutHocTichLuy,
                        chuoi_hoc_lien_tiep = thongKe.ChuoiHocLienTiep,
                        diem_trung_binh = thongKe.DiemTrungBinh,
                        tong_so_luot_thi = thongKe.TongSoLuotThi,
                        ky_nang = new
                        {
                            tu_vung = thongKe.TyLeTuVung,
                            ngu_phap = thongKe.TyLeNguPhap,
                            doc_hieu = thongKe.TyLeDocHieu,
                            nghe_hieu = thongKe.TyLeNgheHieu
                        },
                        bai_hoc_gan_day = dsGanDay.Select(b => new
                        {
                            ma_bai_hoc = b.MaBaiHoc,
                            tieu_de = b.TieuDe,
                            phan_tram = b.PhanTramTienDo,
                            trang_thai = b.TrangThai,
                            ngay_hoc = b.NgayHocGanNhat
                        }).ToList()
                    };

                    string msg = $"Học viên đã hoàn thành {thongKe.SoBaiDaHoanThanh}/{thongKe.TongSoBaiHoc} bài học ({tyLeHoanThanh}%), tích lũy {thongKe.SoPhutHocTichLuy} phút học, điểm trung bình: {thongKe.DiemTrungBinhHienThi}.";
                    return Task.FromResult(AssistantToolResult.OkWithJsonPayload(msg, summaryPayload, "learning_progress_summary_card"));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(AssistantToolResult.Fail($"Lỗi khi tra cứu tiến độ học tập: {ex.Message}", "EXECUTION_ERROR"));
            }
        }
    }

    /// <summary>
    /// Công cụ tra cứu lịch sử làm bài thi, bài khảo sát và điểm số các lần thi của học viên.
    /// Có cơ chế phòng chống IDOR: Học viên chỉ tra cứu được lịch sử thi của chính mình.
    /// </summary>
    public class GetExamHistoryTool : IAssistantTool
    {
        public string Name => "get_exam_history";
        public string Description => "Tra cứu lịch sử làm bài thi, khảo sát, điểm số và kết quả thi của học viên.";
        public string Category => "Learning";
        public string RequiredRole => AssistantPermissionService.RoleStudent; // SinhVien, GiangVien, QuanTriVien
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("maDeThi", "int", "Mã đề thi cụ thể cần lọc lịch sử", isRequired: false),
            new("maNguoiDung", "int", "Mã học viên cần tra cứu (chỉ Giảng viên/Quản trị viên được xem học viên khác)", isRequired: false),
            new("top", "int", "Số lượt thi gần nhất cần lấy (mặc định: 5)", isRequired: false, defaultValue: 5)
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            try
            {
                // 1. Chống IDOR
                int targetUserId = context.UserId;
                if ((context.IsAdmin || context.IsLecturer) && arguments.TryGetValue("maNguoiDung", out var uVal) && uVal != null)
                {
                    if (int.TryParse(uVal.ToString(), out int parsedUid) && parsedUid > 0)
                    {
                        targetUserId = parsedUid;
                    }
                }

                if (targetUserId <= 0)
                {
                    return Task.FromResult(AssistantToolResult.Fail("Không xác định được danh tính học viên cần tra cứu lịch sử thi.", "USER_NOT_IDENTIFIED"));
                }

                int top = Math.Clamp(ToolArgumentHelper.GetInt(arguments, "top", 5), 1, 50);
                int maDeThi = ToolArgumentHelper.GetInt(arguments, "maDeThi", 0);

                // 2. Tra cứu dữ liệu từ DAL
                List<LuotLamBaiDTO> examHistory;
                if (maDeThi > 0 && (context.IsAdmin || context.IsLecturer) && !arguments.ContainsKey("maNguoiDung"))
                {
                    // Giảng viên xem toàn bộ lượt thi của một đề thi
                    examHistory = BaiTapDeThiDAL.Instance.LayLichSuLamBaiCuaDeThi(maDeThi).Take(top).ToList();
                }
                else
                {
                    // Lấy lịch sử thi của học viên cụ thể
                    var allHistory = BaiTapDeThiDAL.Instance.LayLichSuThi(targetUserId, Math.Max(top, 20));
                    if (maDeThi > 0)
                    {
                        allHistory = allHistory.Where(x => x.MaBaiTapDeThi == maDeThi).ToList();
                    }
                    examHistory = allHistory.Take(top).ToList();
                }

                if (examHistory.Count == 0)
                {
                    string emptyMsg = maDeThi > 0 
                        ? $"Chưa ghi nhận lượt làm bài nào cho đề thi #{maDeThi}." 
                        : "Học viên chưa thực hiện lượt làm bài thi nào.";

                    var emptyPayload = new
                    {
                        user_id = targetUserId,
                        tong_luot_tim_thay = 0,
                        diem_cao_nhat = 0.0m,
                        diem_trung_binh = 0.0m,
                        ty_le_dat = 0.0,
                        lich_su = new List<object>()
                    };

                    return Task.FromResult(AssistantToolResult.OkWithJsonPayload(emptyMsg, emptyPayload, "exam_history_card"));
                }

                // 3. Tính toán số liệu thống kê tổng hợp
                decimal diemCaoNhat = examHistory.Max(x => x.DiemSo);
                decimal diemTrungBinh = Math.Round(examHistory.Average(x => x.DiemSo), 2);
                int soLuotDat = examHistory.Count(x => x.DiemSo >= 5.0m);

                var items = examHistory.Select(x => new
                {
                    ma_luot = x.MaLuotLamBai,
                    ma_de_thi = x.MaBaiTapDeThi,
                    ten_de_thi = x.TenBaiTapDeThi,
                    hinh_thuc = x.HinhThucLam,
                    thoi_diem_nop = x.ThoiDiemNopBai?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa nộp",
                    thoi_gian_lam_giay = x.ThoiGianLamGiay,
                    diem_so = x.DiemSo,
                    so_cau_dung = x.SoCauDung,
                    tong_so_cau = x.TongSoCau,
                    trang_thai = x.TrangThai,
                    ket_qua = x.DiemSo >= 5.0m ? "Đạt" : "Chưa đạt"
                }).ToList();

                var payload = new
                {
                    user_id = targetUserId,
                    tong_luot_tim_thay = examHistory.Count,
                    diem_cao_nhat = diemCaoNhat,
                    diem_trung_binh = diemTrungBinh,
                    ty_le_dat = Math.Round((double)soLuotDat * 100.0 / examHistory.Count, 1),
                    lich_su = items
                };

                string summary = $"Đã tìm thấy {examHistory.Count} lượt thi gần nhất. Điểm cao nhất: {diemCaoNhat}, Điểm trung bình: {diemTrungBinh}. Tỷ lệ đạt: {payload.ty_le_dat}%.";
                return Task.FromResult(AssistantToolResult.OkWithJsonPayload(summary, payload, "exam_history_card"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AssistantToolResult.Fail($"Lỗi khi tra cứu lịch sử thi: {ex.Message}", "EXECUTION_ERROR"));
            }
        }
    }
}
