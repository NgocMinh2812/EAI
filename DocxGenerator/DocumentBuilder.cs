using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocxGenerator
{
    public static class DocumentBuilder
    {
        public static string BuildFile1Html()
        {
            var sb = new StringBuilder();

            var sp1List = DataFile1Minh.GetSprint1TestCases();
            var sp2List = DataFile1Minh.GetSprint2TestCases();
            var e2eList = DataFile1Minh.GetE2ETestCases();
            var traceList = DataFile1Minh.GetTraceabilityItems();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"vi\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"utf-8\">");
            sb.AppendLine("<title>Bộ Kịch Bản Kiểm Thử Phần Mềm - Minh (Học Viên)</title>");
            sb.AppendLine("<style>");
            sb.AppendLine(HtmlHelper.GetCss());
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            // 1. Khung thông tin tài liệu
            sb.AppendLine("<div class=\"cover-box\">");
            sb.AppendLine("    <div class=\"cover-title\">DỰ ÁN ENGLISHAI (EAI) &mdash; HỆ THỐNG HỌC & THI TIẾNG ANH THÔNG MINH</div>");
            sb.AppendLine("    <div class=\"cover-sub\">TÀI LIỆU KỊCH BẢN KIỂM THỬ PHẦN MỀM (TEST CASE SPECIFICATION)</div>");
            sb.AppendLine("    <div style=\"height: 1.5px; background: #000000; margin: 12px 0;\"></div>");
            sb.AppendLine("    <table class=\"meta-table\">");
            sb.AppendLine("        <tr><td class=\"lbl\">Tên tài liệu:</td><td><b>BỘ KỊCH BẢN KIỂM THỬ SPRINT 1 & SPRINT 2 &mdash; PHÂN HỆ HỌC VIÊN</b></td></tr>");
            sb.AppendLine("        <tr><td class=\"lbl\">Người phụ trách:</td><td><b>Minh</b> (Kiểm thử viên / Tester)</td></tr>");
            sb.AppendLine("        <tr><td class=\"lbl\">Phân hệ phụ trách:</td><td>Giao diện và chức năng dành cho Học viên / Sinh viên</td></tr>");
            sb.AppendLine("        <tr><td class=\"lbl\">Phạm vi kiểm thử:</td><td><b>Sprint 1 và Sprint 2</b> (Bao phủ TASK-101 đến TASK-110 và TASK-201 đến TASK-209)</td></tr>");
            sb.AppendLine("        <tr><td class=\"lbl\">Quy cách trình bày:</td><td>Bảng ngang 5 cột chuẩn kiểm thử: <b>Mã TC | Tên TC | Thực nghiệm | Kết quả mong đợi | Pass/Fail</b></td></tr>");
            sb.AppendLine("        <tr><td class=\"lbl\">Ngày cập nhật:</td><td>23/10/2026</td></tr>");
            sb.AppendLine("    </table>");
            sb.AppendLine("</div>");

            // Mục 1: Mục đích & Môi trường
            sb.AppendLine("<h1>MỤC 1. MỤC ĐÍCH VÀ PHẠM VI KIỂM THỬ</h1>");
            sb.AppendLine("<p><b>1.1 Mục đích:</b> Tài liệu này tập hợp toàn bộ các kịch bản kiểm thử chức năng dành riêng cho <b>phân hệ Học viên</b> của hệ thống EnglishAI do <b>Minh</b> phụ trách. Các kịch bản được viết theo cách hiểu nghiệp vụ thông thường, rõ ràng, giúp người kiểm thử dễ dàng thực hiện thao tác trên màn hình máy tính và ghi nhận trực tiếp kết quả kiểm thử (Đạt/Không đạt).</p>");
            sb.AppendLine("<p><b>1.2 Phạm vi chức năng kiểm thử:</b>");
            sb.AppendLine("<ul>");
            sb.AppendLine("    <li><b>Giai đoạn 1 (Sprint 1):</b> Cài đặt dữ liệu mẫu ban đầu; Mở ứng dụng; Đăng ký và đăng nhập tài khoản học viên; Đăng xuất và bảo mật tài khoản; Giao diện chính và thanh điều hướng; Quy trình bắt buộc làm bài khảo sát năng lực khi đăng nhập lần đầu; Làm bài kiểm tra đầu vào, chấm điểm và xếp lớp tự động theo chuẩn A1 đến C1.</li>");
            sb.AppendLine("    <li><b>Giai đoạn 2 (Sprint 2):</b> Trang chủ học tập hiển thị các chỉ số thống kê; Danh mục bài học theo lộ trình; Mở xem bài giảng lý thuyết ngữ pháp và từ vựng; Nghe âm thanh bài học với các tính năng tua và chỉnh tốc độ; Tính năng tự động đọc khi chưa có file ghi âm; Tích lũy thời gian học và ghi nhận hoàn thành bài học; Luyện tập trắc nghiệm, thẻ từ vựng thông minh và nghe chép chính tả; Phòng thi thử chuẩn hóa có tính giờ, nộp bài, chấm điểm và xem lại chi tiết bài làm.</li>");
            sb.AppendLine("    <li><b>Kiểm thử toàn trình (End-to-End):</b> Toàn bộ hành trình của người học mới từ lúc tạo tài khoản đến khi học tập và hoàn thành các bài thi thử.</li>");
            sb.AppendLine("</ul></p>");

            sb.AppendLine("<h2>1.3 Môi trường kiểm thử</h2>");
            sb.AppendLine("<table class=\"tbl-summary\">");
            sb.AppendLine("    <tr><th style=\"width:25%\">Thành phần</th><th style=\"width:45%\">Cấu hình sử dụng</th><th style=\"width:30%\">Ghi chú</th></tr>");
            sb.AppendLine("    <tr><td>Hệ điều hành</td><td>Microsoft Windows 10 / Windows 11 (64-bit)</td><td>Máy tính người dùng</td></tr>");
            sb.AppendLine("    <tr><td>Môi trường ứng dụng</td><td>.NET 10.0 Desktop Runtime</td><td>Chạy phần mềm EnglishAI</td></tr>");
            sb.AppendLine("    <tr><td>Máy chủ dữ liệu</td><td>Microsoft SQL Server (Cơ sở dữ liệu TiengAnhAI)</td><td>Lưu trữ dữ liệu toàn hệ thống</td></tr>");
            sb.AppendLine("    <tr><td>Tài khoản học viên mới</td><td>Email: <code>minh_test@gmail.com</code> | Mật khẩu: <code>123456</code></td><td>Thử nghiệm lần đầu đăng nhập</td></tr>");
            sb.AppendLine("    <tr><td>Tài khoản học viên cũ</td><td>Email: <code>sinhvien@englisheai.edu.vn</code> | Mật khẩu: <code>123456</code></td><td>Đã khảo sát, trình độ B1</td></tr>");
            sb.AppendLine("    <tr><td>Tài khoản bị khóa</td><td>Email: <code>locked_user@englisheai.edu.vn</code> | Mật khẩu: <code>123456</code></td><td>Thử nghiệm chặn đăng nhập</td></tr>");
            sb.AppendLine("</table>");

            // Mục 2: Sprint 1 Test Cases (Grouped by module)
            sb.AppendLine("<div class=\"sprint-header\">MỤC 2. BỘ KỊCH BẢN KIỂM THỬ SPRINT 1 &mdash; PHÂN HỆ HỌC VIÊN</div>");
            sb.AppendLine("<p>Bao gồm các kịch bản kiểm thử từ <b>TASK-101 đến TASK-110</b>:</p>");

            var sp1Groups = sp1List.GroupBy(x => x.Module).ToList();
            foreach (var grp in sp1Groups)
            {
                var first = grp.First();
                sb.AppendLine("<div class=\"module-header\">");
                sb.AppendLine($"    <div class=\"module-title\">PHÂN HỆ: {grp.Key.ToUpper()}</div>");
                sb.AppendLine($"    <div class=\"module-desc\">Nhiệm vụ: {first.TaskId} &mdash; Số lượng kịch bản: {grp.Count()} test cases</div>");
                sb.AppendLine("</div>");

                sb.AppendLine("<table class=\"tc-table\">");
                sb.AppendLine(HtmlHelper.RenderTableHeader());
                sb.AppendLine("<tbody>");
                foreach (var tc in grp)
                {
                    sb.AppendLine(HtmlHelper.RenderTableRow(tc));
                }
                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");
            }

            // Mục 3: Sprint 2 Test Cases (Grouped by module)
            sb.AppendLine("<div class=\"page-break\"></div>");
            sb.AppendLine("<div class=\"sprint-header\">MỤC 3. BỘ KỊCH BẢN KIỂM THỬ SPRINT 2 &mdash; PHÂN HỆ HỌC VIÊN</div>");
            sb.AppendLine("<p>Bao gồm các kịch bản kiểm thử từ <b>TASK-201 đến TASK-209</b>:</p>");

            var sp2Groups = sp2List.GroupBy(x => x.Module).ToList();
            foreach (var grp in sp2Groups)
            {
                var first = grp.First();
                sb.AppendLine("<div class=\"module-header\">");
                sb.AppendLine($"    <div class=\"module-title\">PHÂN HỆ: {grp.Key.ToUpper()}</div>");
                sb.AppendLine($"    <div class=\"module-desc\">Nhiệm vụ: {first.TaskId} &mdash; Số lượng kịch bản: {grp.Count()} test cases</div>");
                sb.AppendLine("</div>");

                sb.AppendLine("<table class=\"tc-table\">");
                sb.AppendLine(HtmlHelper.RenderTableHeader());
                sb.AppendLine("<tbody>");
                foreach (var tc in grp)
                {
                    sb.AppendLine(HtmlHelper.RenderTableRow(tc));
                }
                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");
            }

            // Mục 4: E2E Integration
            sb.AppendLine("<div class=\"page-break\"></div>");
            sb.AppendLine("<div class=\"sprint-header\">MỤC 4. KỊCH BẢN KIỂM THỬ TOÀN TRÌNH (END-TO-END)</div>");
            sb.AppendLine("<p>Kiểm tra trải nghiệm liên hoàn của học viên từ lúc bắt đầu tạo tài khoản đến khi tốt nghiệp lộ trình học:</p>");

            sb.AppendLine("<table class=\"tc-table\">");
            sb.AppendLine(HtmlHelper.RenderTableHeader());
            sb.AppendLine("<tbody>");
            foreach (var tc in e2eList)
            {
                sb.AppendLine(HtmlHelper.RenderTableRow(tc));
            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");

            // Mục 5: Traceability & Mapping Matrix
            sb.AppendLine("<div class=\"page-break\"></div>");
            sb.AppendLine("<h1>MỤC 5. MA TRẬN ĐỐI CHIẾU VÀ ÁNH XẠ KỊCH BẢN KIỂM THỬ</h1>");
            sb.AppendLine("<p>Bảng đối chiếu giữa các đầu mối công việc trong file Kế hoạch triển khai (<code>Kehoachtrienkhai.xlsx</code>) với các mã kịch bản kiểm thử tương ứng, đảm bảo kiểm tra đầy đủ 100% các chức năng được yêu cầu:</p>");

            sb.AppendLine("<table class=\"tbl-matrix\">");
            sb.AppendLine("    <thead><tr><th style=\"width:12%\">Mã công việc</th><th style=\"width:24%\">Tên hạng mục chức năng</th><th style=\"width:34%\">Tiêu chí nghiệm thu</th><th style=\"width:15%\">Mã Test Case</th><th style=\"width:15%\">Ghi chú</th></tr></thead>");
            sb.AppendLine("    <tbody>");
            foreach (var tr in traceList)
            {
                sb.AppendLine($"    <tr><td><b>{tr.Code}</b></td><td>{tr.Name}</td><td>{tr.AcceptanceCriteria}</td><td><b>{tr.TestCaseIds}</b></td><td><i>{tr.Note}</i></td></tr>");
            }
            sb.AppendLine("    </tbody>");
            sb.AppendLine("</table>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public static string BuildFile2Html()
        {
            var sb = new StringBuilder();

            var sp1List = DataFile2Em.GetSprint1TestCases();
            var sp2List = DataFile2Em.GetSprint2TestCases();
            var e2eList = DataFile2Em.GetE2ETestCases();
            var traceList = DataFile2Em.GetTraceabilityItems();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"vi\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"utf-8\">");
            sb.AppendLine("<title>Bộ Kịch Bản Kiểm Thử Phần Mềm - Êm (Giảng Viên / Admin)</title>");
            sb.AppendLine("<style>");
            sb.AppendLine(HtmlHelper.GetCss());
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            // 1. Khung thông tin tài liệu
            sb.AppendLine("<div class=\"cover-box\">");
            sb.AppendLine("    <div class=\"cover-title\">DỰ ÁN ENGLISHAI (EAI) &mdash; HỆ THỐNG HỌC & THI TIẾNG ANH THÔNG MINH</div>");
            sb.AppendLine("    <div class=\"cover-sub\">TÀI LIỆU KỊCH BẢN KIỂM THỬ PHẦN MỀM (TEST CASE SPECIFICATION)</div>");
            sb.AppendLine("    <div style=\"height: 1.5px; background: #000000; margin: 12px 0;\"></div>");
            sb.AppendLine("    <table class=\"meta-table\">");
            sb.AppendLine("        <tr><td class=\"lbl\">Tên tài liệu:</td><td><b>BỘ KỊCH BẢN KIỂM THỬ SPRINT 1 & SPRINT 2 &mdash; PHÂN HỆ GIẢNG VIÊN / ADMIN</b></td></tr>");
            sb.AppendLine("        <tr><td class=\"lbl\">Người phụ trách:</td><td><b>Êm</b> (Kiểm thử viên / Tester)</td></tr>");
            sb.AppendLine("        <tr><td class=\"lbl\">Phân hệ phụ trách:</td><td>Giao diện và chức năng dành cho Giảng viên & Quản trị viên</td></tr>");
            sb.AppendLine("        <tr><td class=\"lbl\">Phạm vi kiểm thử:</td><td><b>Sprint 1 và Sprint 2</b> (Bao phủ US-GV01 đến US-GV07 và WBS T-01 đến T-23)</td></tr>");
            sb.AppendLine("        <tr><td class=\"lbl\">Quy cách trình bày:</td><td>Bảng ngang 5 cột chuẩn kiểm thử: <b>Mã TC | Tên TC | Thực nghiệm | Kết quả mong đợi | Pass/Fail</b></td></tr>");
            sb.AppendLine("        <tr><td class=\"lbl\">Ngày cập nhật:</td><td>23/10/2026</td></tr>");
            sb.AppendLine("    </table>");
            sb.AppendLine("</div>");

            // Mục 1: Mục đích & Phạm vi
            sb.AppendLine("<h1>MỤC 1. MỤC ĐÍCH VÀ PHẠM VI KIỂM THỬ</h1>");
            sb.AppendLine("<p><b>1.1 Mục đích:</b> Tài liệu này tập hợp toàn bộ các kịch bản kiểm thử chức năng dành riêng cho <b>phân hệ Giảng viên & Quản trị viên (Admin)</b> của hệ thống EnglishAI do <b>Êm</b> phụ trách. Tài liệu được thiết kế tối giản, dễ hiểu, sử dụng ngôn ngữ kiểm thử thông thường để phục vụ trực tiếp cho việc vận hành thử nghiệm trên ứng dụng thực tế.</p>");
            sb.AppendLine("<p><b>1.2 Phạm vi chức năng kiểm thử:</b>");
            sb.AppendLine("<ul>");
            sb.AppendLine("    <li><b>Giai đoạn 1 (Sprint 1):</b> Khung màn hình quản trị và thanh điều hướng; Trang Tổng quan với 4 thẻ thống kê và biểu đồ hoạt động; Quản lý tài khoản người dùng, phân trang danh sách, tìm kiếm, lọc vai trò, khóa và mở khóa tài khoản, cấp lại mật khẩu; Quản lý cây danh mục chủ đề phân cấp cha con, thêm/sửa/di chuyển/ẩn chủ đề; Kiểm thử liên thông phân hệ quản trị.</li>");
            sb.AppendLine("    <li><b>Giai đoạn 2 (Sprint 2):</b> Soạn thảo nội dung bài học theo chủ đề và trình độ, phân loại 5 kỹ năng, đính kèm file âm thanh và ảnh minh họa, xuất bản bài học; Quản lý ngân hàng câu hỏi trắc nghiệm, điền từ, đọc hiểu, nghe hiểu, giải thích chi tiết, gắn mức độ khó; Quản lý và tạo lập đề thi thử tính giờ hoặc luyện tập tự do, chọn câu hỏi, sắp xếp thứ tự và điểm số, xem trước phòng thi, phát hành đề thi cho học viên; Kiểm thử liên hoàn xuất bản học liệu và thi cử.</li>");
            sb.AppendLine("    <li><b>Kiểm thử toàn trình (End-to-End):</b> Chu trình quản lý học liệu và khảo thí trọn vẹn từ lúc dựng cây danh mục, soạn bài học, biên tập câu hỏi đến phát hành đề thi và xử lý vi phạm tài khoản.</li>");
            sb.AppendLine("</ul></p>");

            sb.AppendLine("<h2>1.3 Môi trường kiểm thử</h2>");
            sb.AppendLine("<table class=\"tbl-summary\">");
            sb.AppendLine("    <tr><th style=\"width:25%\">Thành phần</th><th style=\"width:45%\">Cấu hình sử dụng</th><th style=\"width:30%\">Ghi chú</th></tr>");
            sb.AppendLine("    <tr><td>Hệ điều hành</td><td>Microsoft Windows 10 / Windows 11 (64-bit)</td><td>Máy tính người quản trị</td></tr>");
            sb.AppendLine("    <tr><td>Môi trường ứng dụng</td><td>.NET 10.0 Desktop Runtime</td><td>Chạy phần mềm EnglishAI</td></tr>");
            sb.AppendLine("    <tr><td>Máy chủ dữ liệu</td><td>Microsoft SQL Server (Cơ sở dữ liệu TiengAnhAI)</td><td>Lưu trữ dữ liệu toàn hệ thống</td></tr>");
            sb.AppendLine("    <tr><td>Tài khoản Giảng viên</td><td>Email: <code>giangvien@englisheai.edu.vn</code> | Mật khẩu: <code>123456</code></td><td>Toàn quyền quản trị học liệu</td></tr>");
            sb.AppendLine("    <tr><td>Tài khoản Học viên thử nghiệm</td><td>Email: <code>sinhvien@englisheai.edu.vn</code> | Mật khẩu: <code>123456</code></td><td>Kiểm tra hiển thị phía học sinh</td></tr>");
            sb.AppendLine("    <tr><td>Tệp đính kèm thử nghiệm</td><td>Các file mẫu âm thanh (.mp3) và hình ảnh (.png)</td><td>Thử nghiệm đính kèm bài học</td></tr>");
            sb.AppendLine("</table>");

            // Mục 2: Sprint 1 Test Cases (Grouped by module)
            sb.AppendLine("<div class=\"sprint-header\">MỤC 2. BỘ KỊCH BẢN KIỂM THỬ SPRINT 1 &mdash; PHÂN HỆ GIẢNG VIÊN / ADMIN</div>");
            sb.AppendLine("<p>Bao gồm các kịch bản kiểm thử từ <b>US-GV01 đến US-GV04</b> và các nhiệm vụ kỹ thuật <b>T-01 đến T-13</b>:</p>");

            var sp1Groups = sp1List.GroupBy(x => x.Module).ToList();
            foreach (var grp in sp1Groups)
            {
                var first = grp.First();
                sb.AppendLine("<div class=\"module-header\">");
                sb.AppendLine($"    <div class=\"module-title\">PHÂN HỆ: {grp.Key.ToUpper()}</div>");
                sb.AppendLine($"    <div class=\"module-desc\">Nhiệm vụ: {first.TaskId} &mdash; Số lượng kịch bản: {grp.Count()} test cases</div>");
                sb.AppendLine("</div>");

                sb.AppendLine("<table class=\"tc-table\">");
                sb.AppendLine(HtmlHelper.RenderTableHeader());
                sb.AppendLine("<tbody>");
                foreach (var tc in grp)
                {
                    sb.AppendLine(HtmlHelper.RenderTableRow(tc));
                }
                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");
            }

            // Mục 3: Sprint 2 Test Cases (Grouped by module)
            sb.AppendLine("<div class=\"page-break\"></div>");
            sb.AppendLine("<div class=\"sprint-header\">MỤC 3. BỘ KỊCH BẢN KIỂM THỬ SPRINT 2 &mdash; PHÂN HỆ GIẢNG VIÊN / ADMIN</div>");
            sb.AppendLine("<p>Bao gồm các kịch bản kiểm thử từ <b>US-GV05 đến US-GV07</b> và các nhiệm vụ kỹ thuật <b>T-14 đến T-23</b>:</p>");

            var sp2Groups = sp2List.GroupBy(x => x.Module).ToList();
            foreach (var grp in sp2Groups)
            {
                var first = grp.First();
                sb.AppendLine("<div class=\"module-header\">");
                sb.AppendLine($"    <div class=\"module-title\">PHÂN HỆ: {grp.Key.ToUpper()}</div>");
                sb.AppendLine($"    <div class=\"module-desc\">Nhiệm vụ: {first.TaskId} &mdash; Số lượng kịch bản: {grp.Count()} test cases</div>");
                sb.AppendLine("</div>");

                sb.AppendLine("<table class=\"tc-table\">");
                sb.AppendLine(HtmlHelper.RenderTableHeader());
                sb.AppendLine("<tbody>");
                foreach (var tc in grp)
                {
                    sb.AppendLine(HtmlHelper.RenderTableRow(tc));
                }
                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");
            }

            // Mục 4: E2E Integration
            sb.AppendLine("<div class=\"page-break\"></div>");
            sb.AppendLine("<div class=\"sprint-header\">MỤC 4. KỊCH BẢN KIỂM THỬ TOÀN TRÌNH (END-TO-END)</div>");
            sb.AppendLine("<p>Kiểm tra chu trình quản lý học liệu và khảo thí trọn vẹn trên hệ thống quản trị:</p>");

            sb.AppendLine("<table class=\"tc-table\">");
            sb.AppendLine(HtmlHelper.RenderTableHeader());
            sb.AppendLine("<tbody>");
            foreach (var tc in e2eList)
            {
                sb.AppendLine(HtmlHelper.RenderTableRow(tc));
            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");

            // Mục 5: Traceability & Mapping Matrix
            sb.AppendLine("<div class=\"page-break\"></div>");
            sb.AppendLine("<h1>MỤC 5. MA TRẬN ĐỐI CHIẾU VÀ ÁNH XẠ KỊCH BẢN KIỂM THỬ</h1>");
            sb.AppendLine("<p>Bảng đối chiếu giữa các User Story, nhiệm vụ WBS trong Kế hoạch triển khai (<code>Kehoachtrienkhai.xlsx</code>) với các mã kịch bản kiểm thử tương ứng của phân hệ Giảng viên/Admin:</p>");

            sb.AppendLine("<table class=\"tbl-matrix\">");
            sb.AppendLine("    <thead><tr><th style=\"width:14%\">Mã công việc</th><th style=\"width:24%\">Tên hạng mục chức năng</th><th style=\"width:32%\">Tiêu chí nghiệm thu</th><th style=\"width:15%\">Mã Test Case</th><th style=\"width:15%\">Ghi chú</th></tr></thead>");
            sb.AppendLine("    <tbody>");
            foreach (var tr in traceList)
            {
                sb.AppendLine($"    <tr><td><b>{tr.Code}</b></td><td>{tr.Name}</td><td>{tr.AcceptanceCriteria}</td><td><b>{tr.TestCaseIds}</b></td><td><i>{tr.Note}</i></td></tr>");
            }
            sb.AppendLine("    </tbody>");
            sb.AppendLine("</table>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
