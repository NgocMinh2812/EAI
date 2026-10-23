using System;
using System.Collections.Generic;
using System.IO;

namespace DocxGenerator
{
    public static class ScreenshotAttachmentHelper
    {
        private static readonly string BaseDir = @"d:\TDC\HK5\CDPTUD\EAI\artifacts\ui-screenshots";

        public static void AttachScreenshotsToFile1(List<TestCase> list)
        {
            foreach (var tc in list)
            {
                switch (tc.Id)
                {
                    case "TC-HV-SP1-AUTH-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint1\Student\TC-HV-SP1-AUTH-002.png");
                        tc.ScreenshotCaption = "Màn hình Đăng ký tài khoản học viên (DangNhapWindow.xaml - Tab Đăng ký)";
                        tc.Actual = "Giao diện chuyển tab Đăng ký mượt mà, hiển thị đầy đủ 4 trường thông tin bắt buộc (Họ tên, Email, Mật khẩu, Xác nhận mật khẩu) kèm thông báo validation.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint1/Student/TC-HV-SP1-AUTH-002.png";
                        break;

                    case "TC-HV-SP1-AUTH-006":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint1\Student\TC-HV-SP1-AUTH-001.png");
                        tc.ScreenshotCaption = "Màn hình Đăng nhập học viên hệ thống (DangNhapWindow.xaml - Tab Đăng nhập)";
                        tc.Actual = "Cửa sổ đăng nhập mở thành công với hiệu ứng Aurora Teal bo góc 24px, form nhập Email và Mật khẩu sắc nét, nút bấm kích hoạt chính xác.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint1/Student/TC-HV-SP1-AUTH-001.png";
                        break;

                    case "TC-HV-SP1-SURV-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint1\Student\TC-HV-SP1-SURV-001.png");
                        tc.ScreenshotCaption = "Màn hình Bài kiểm tra khảo sát năng lực đầu vào (DanhGiaNangLucView.xaml)";
                        tc.Actual = "Nạp bộ câu hỏi chẩn đoán năng lực đầu vào từ CSDL TiengAnhAI, hiển thị danh sách đáp án lựa chọn và ma trận câu hỏi hỗ trợ điều hướng nhanh.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint1/Student/TC-HV-SP1-SURV-001.png";
                        break;

                    case "TC-HV-SP2-HOME-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint2\Student\TC-HV-SP2-HOME-001.png");
                        tc.ScreenshotCaption = "Trang chủ học tập cá nhân hóa sinh viên (TrangChuView.xaml)";
                        tc.Actual = "Hiển thị thông tin học viên 'Trần Thị Mai', mục tiêu 'IELTS 6.5 (CEFR B2)', thẻ thống kê điểm trung bình 75.0%, biểu đồ học tập tuần và danh sách bài học gợi ý.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint2/Student/TC-HV-SP2-HOME-001.png";
                        break;

                    case "TC-HV-SP2-LRN-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint2\Student\TC-HV-SP2-LEARN-001.png");
                        tc.ScreenshotCaption = "Giao diện Danh sách bài học và Chi tiết nội dung học tập (HocTapView.xaml)";
                        tc.Actual = "Nạp danh mục bài học từ CSDL, hiển thị cây chủ đề và nội dung đa phương tiện, tích hợp bài tập củng cố và vận dụng theo từng bài.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint2/Student/TC-HV-SP2-LEARN-001.png";
                        break;

                    case "TC-HV-SP2-AUD-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint2\Student\TC-HV-SP2-AUDIO-001.png");
                        tc.ScreenshotCaption = "Trình phát âm thanh Audio Player trong bài học luyện nghe (HocTapView.xaml)";
                        tc.Actual = "Tích hợp audio player với nút Play/Pause, tua thời gian, thanh tiến trình và hiển thị transcript bài nghe hỗ trợ học viên luyện kỹ năng Listening.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint2/Student/TC-HV-SP2-AUDIO-001.png";
                        break;

                    case "TC-HV-SP2-PRAC-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint2\Student\TC-HV-SP2-PRACT-001.png");
                        tc.ScreenshotCaption = "Trung tâm Luyện tập kỹ năng Tiếng Anh theo chủ đề (LuyenTapView.xaml)";
                        tc.Actual = "Hiển thị các gói luyện tập đa dạng theo kỹ năng (Ngữ pháp, Từ vựng, Đọc hiểu, Nghe), hỗ trợ làm bài củng cố kiến thức và phản hồi kết quả.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint2/Student/TC-HV-SP2-PRACT-001.png";
                        break;

                    case "TC-HV-SP2-EXAM-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint2\Student\TC-HV-SP2-EXAM-001.png");
                        tc.ScreenshotCaption = "Phòng thi thử trắc nghiệm trực tuyến bấm giờ (ThiThuView.xaml)";
                        tc.Actual = "Giao diện làm bài thi mô phỏng đề thi thực tế chuẩn khảo thí, đồng hồ đếm ngược thời gian thực, bảng câu hỏi và chức năng nộp bài an toàn.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint2/Student/TC-HV-SP2-EXAM-001.png";
                        break;

                    case "TC-HV-SP2-PROG-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint2\Student\TC-HV-SP2-SCORE-001.png");
                        tc.ScreenshotCaption = "Báo cáo Tiến độ học tập & Thống kê kết quả thi (TienDoHocView.xaml)";
                        tc.Actual = "Tổng hợp biểu đồ tiến độ, phân tích tỷ lệ chính xác theo từng kỹ năng học thuật và lịch sử các lượt làm bài của học viên.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint2/Student/TC-HV-SP2-SCORE-001.png";
                        break;

                    case "TC-HV-SP2-REV-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint2\Student\TC-HV-SP2-REV-001.png");
                        tc.ScreenshotCaption = "Hồ sơ cá nhân & Lịch sử học tập học viên (HoSoCaNhanView.xaml)";
                        tc.Actual = "Hiển thị đầy đủ thông tin cá nhân học viên, cấp độ hiện tại, cấu hình mục tiêu học tập và danh sách bài thi đã thực hiện.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint2/Student/TC-HV-SP2-REV-001.png";
                        break;
                }
            }
        }

        public static void AttachScreenshotsToFile2(List<TestCase> list)
        {
            foreach (var tc in list)
            {
                switch (tc.Id)
                {
                    case "TC-GV-SP1-NAV-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint1\Admin\TC-GV-SP1-NAV-001.png");
                        tc.ScreenshotCaption = "Khung điều hướng Giảng viên & Dashboard tổng quan (GiangVienWindow.xaml & TongQuanView.xaml)";
                        tc.Actual = "Sidebar điều hướng mở rộng với đầy đủ 9 menu chức năng, Profile avatar 'HT', breadcrumb 'Tổng quan', thống kê số liệu và xu hướng học tập.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint1/Admin/TC-GV-SP1-NAV-001.png";
                        break;

                    case "TC-GV-SP1-DASH-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint1\Admin\TC-GV-SP1-NAV-001.png");
                        tc.ScreenshotCaption = "Dashboard Tổng quan quản trị viên (TongQuanView.xaml)";
                        tc.Actual = "Hiển thị các chỉ số tổng hợp: 9 học viên, 33 bài học, 105 câu hỏi, 85 lượt thi, biểu đồ xu hướng học tập và danh sách bài nộp trực tiếp.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint1/Admin/TC-GV-SP1-NAV-001.png";
                        break;

                    case "TC-GV-SP1-USR-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint1\Admin\TC-GV-SP1-USER-001.png");
                        tc.ScreenshotCaption = "Màn hình Quản lý người dùng hệ thống (NguoiDungView.xaml)";
                        tc.Actual = "Nạp danh sách 14 tài khoản từ CSDL TiengAnhAI, phân loại vai trò (Quản trị viên, Giảng viên, Học viên), lọc trạng thái hoạt động và các nút thao tác chỉnh sửa/khóa.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint1/Admin/TC-GV-SP1-USER-001.png";
                        break;

                    case "TC-GV-SP1-CAT-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint1\Admin\TC-GV-SP1-CAT-001.png");
                        tc.ScreenshotCaption = "Màn hình Quản lý danh mục & cấu trúc học liệu (DanhMucView.xaml)";
                        tc.Actual = "Hiển thị cây thư mục phân cấp danh mục học liệu, kỹ năng và cấp độ CEFR, hỗ trợ thêm/sửa/xóa và đồng bộ với kho học liệu.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint1/Admin/TC-GV-SP1-CAT-001.png";
                        break;

                    case "TC-GV-SP2-LES-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint1\Admin\TC-GV-SP1-LESS-001.png");
                        tc.ScreenshotCaption = "Màn hình Quản lý bài học & nội dung học tập (NoiDungHocTapView.xaml)";
                        tc.Actual = "Danh sách bài học hiển thị rõ ràng mã bài học, tên bài học, kỹ năng, thời lượng, trạng thái xuất bản và nút chỉnh sửa nội dung rich text.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint1/Admin/TC-GV-SP1-LESS-001.png";
                        break;

                    case "TC-GV-SP2-QUE-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint1\Admin\TC-GV-SP1-QBANK-001.png");
                        tc.ScreenshotCaption = "Màn hình Ngân hàng câu hỏi trắc nghiệm (NganHangCauHoiView.xaml)";
                        tc.Actual = "Quản lý ngân hàng hơn 100 câu hỏi với bộ lọc kỹ năng (Nghe, Đọc, Viết, Nói), độ khó (Dễ, Trung bình, Khó) và hỗ trợ xem trước đáp án.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint1/Admin/TC-GV-SP1-QBANK-001.png";
                        break;

                    case "TC-GV-SP2-EXAM-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint2\Admin\TC-GV-SP2-EXAM-001.png");
                        tc.ScreenshotCaption = "Màn hình Quản trị Đề thi & Bộ đề khảo thí (DeThiLuyenTapView.xaml)";
                        tc.Actual = "Danh sách đề thi hiển thị mã đề, tiêu đề, thời gian làm bài, số lượng câu hỏi và trạng thái hoạt động phục vụ tổ chức thi trực tuyến.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint2/Admin/TC-GV-SP2-EXAM-001.png";
                        break;

                    case "TC-GV-SP2-EXAM-005":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint2\Admin\TC-GV-SP2-PRAC-001.png");
                        tc.ScreenshotCaption = "Màn hình Quản trị Gói luyện tập kỹ năng (DeThiLuyenTapView.xaml - Tab Gói luyện tập)";
                        tc.Actual = "Cấu hình gói luyện tập phân theo chủ đề và trình độ, liên kết ngân hàng câu hỏi để học viên ôn tập hàng ngày.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint2/Admin/TC-GV-SP2-PRAC-001.png";
                        break;

                    case "TC-GV-SP2-INT-001":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint2\Admin\TC-GV-SP2-AI-001.png");
                        tc.ScreenshotCaption = "Màn hình Trợ lý AI & Tạo câu hỏi tự động từ tài liệu (AISinhDeView.xaml)";
                        tc.Actual = "Giao diện tích hợp AI hỗ trợ tải tệp học liệu (DOCX/PDF/TXT), tự động trích xuất nội dung và sinh câu hỏi trắc nghiệm kèm giải thích chi tiết.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint2/Admin/TC-GV-SP2-AI-001.png";
                        break;

                    case "TC-GV-SP2-INT-002":
                        tc.ScreenshotPath = Path.Combine(BaseDir, @"Sprint2\Admin\TC-GV-SP2-MON-001.png");
                        tc.ScreenshotCaption = "Màn hình Giám sát hệ thống & Thống kê khảo thí (GiamSatHeThongView.xaml)";
                        tc.Actual = "Biểu đồ trực quan hóa dữ liệu người dùng, phân bố trình độ học viên, tỷ lệ hoàn thành bài học và tình trạng tài nguyên hệ thống.";
                        tc.Status = "Pass";
                        tc.Evidence = "artifacts/ui-screenshots/Sprint2/Admin/TC-GV-SP2-MON-001.png";
                        break;
                }
            }
        }
    }
}
