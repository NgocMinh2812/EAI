# EAI ADMIN DASHBOARD – TEST CASES SPECIFICATION & MATRIX

**Dự án:** EnglishAI (EAI)  
**Phân hệ:** Module Tổng quan Quản trị viên (Admin Dashboard)  
**Mã kiểm thử:** TC-DASH-2026-01 -> TC-DASH-2026-40  
**Tác giả:** Senior QA Engineer, Senior .NET/WPF Developer  

---

## 1. MA TRẬN TEST CASE TỔNG QUAN

| ID | Nhóm kiểm thử | Mục tiêu kiểm thử | Đầu vào / Thao tác | Kết quả mong đợi | Trạng thái |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **TC-01** | Khởi tạo Dashboard | Mở Dashboard sau khi đăng nhập | Đăng nhập tài khoản Quản trị viên | Mở `TongQuanView`, Sidebar đánh dấu mục "Tổng quan", breadcrumb `Admin › Tổng quan` | **PASS** |
| **TC-02** | Điều hướng Sidebar | Chuyển trang qua lại giữa các menu | Bấm lần lượt: Người dùng, Danh mục, Nội dung, Ngân hàng, Đề thi... rồi quay lại Tổng quan | Chuyển view đúng, không bị mở trùng trang nhờ `_pageCache`, breadcrumb cập nhật chính xác, không trắng trang | **PASS** |
| **TC-03** | Sidebar thu gọn | Thu nhỏ và mở rộng thanh bên | Bấm nút `«` ở góc trên Sidebar | Chiều rộng thu hẹp về `68px`, icon căn giữa, tooltip hiển thị đầy đủ, bấm `»` mở rộng về `240px` | **PASS** |
| **TC-04** | Phân quyền Giám sát | Kiểm tra quyền truy cập Giám sát hệ thống | Đăng nhập tài khoản Giảng viên vs Quản trị viên | Quản trị viên hiển thị menu Giám sát; Giảng viên ẩn hoặc hiển thị cảnh báo nếu truy cập trái phép | **PASS** |
| **TC-05** | Đăng xuất an toàn | Thoát phiên làm việc quản trị | Bấm nút Đăng xuất ở chân Sidebar | Hiển thị Dialog xác nhận, chọn "Đăng xuất" xóa `SessionHelper` và quay về `DangNhapWindow` | **PASS** |
| **TC-06** | Tìm kiếm toàn hệ thống | Tìm kiếm chức năng nội bộ | Gõ "Đề thi" hoặc "Người dùng" vào `txtGlobalSearch` | Hiển thị gợi ý module tương ứng, click chuyển trang chính xác | **PASS** |
| **TC-07** | Tìm kiếm Người dùng | Tìm học viên/giảng viên theo tên | Gõ "Hoàng" hoặc "admin" | Hiển thị gợi ý người dùng thực từ CSDL, click chuyển `NguoiDungView` lọc đúng từ khóa | **PASS** |
| **TC-08** | Tìm kiếm Bài học | Tìm bài học theo tiêu đề | Gõ "Thì hiện tại" hoặc "Kinh tế" | Hiển thị gợi ý bài học thực từ CSDL, click chuyển thẳng vào bài học trong `NoiDungHocTapView` | **PASS** |
| **TC-09** | Tìm kiếm Câu hỏi | Tìm câu hỏi trắc nghiệm | Gõ "weather" hoặc "grammar" | Hiển thị gợi ý câu hỏi từ CSDL (`CauHoiDAL`), click mở `NganHangCauHoiView` | **PASS** |
| **TC-10** | Phím tắt Ctrl + F | Kích hoạt nhanh ô tìm kiếm | Bấm tổ hợp `Ctrl + F` | Con trỏ focus vào `txtGlobalSearch` và bôi đen toàn bộ nội dung | **PASS** |
| **TC-11** | Phím Enter ô tìm kiếm | Thực hiện tìm kiếm nhanh bằng Enter | Gõ từ khóa rồi nhấn `Enter` | Popup đóng, chuyển thẳng sang `NoiDungHocTapView` tìm kiếm với từ khóa đó | **PASS** |
| **TC-12** | Phím Esc ô tìm kiếm | Hủy bỏ gợi ý tìm kiếm | Gõ từ khóa, popup mở, bấm `Esc` | Popup đóng, không làm gián đoạn trạng thái màn hình hiện tại | **PASS** |
| **TC-13** | Debounce tìm kiếm | Kiểm tra chống spam truy vấn DB | Gõ liên tục chuỗi 15 ký tự trong 0.5s | Chỉ phát đúng 1 truy vấn sau khi ngừng gõ 250ms, UI mượt mà không giật lag | **PASS** |
| **TC-14** | Trợ lý AI (Topbar) | Mở trợ lý AI sinh đề | Bấm nút "Trợ lý AI" trên Topbar | Chuyển ngay đến `AISinhDeView` | **PASS** |
| **TC-15** | Chuông thông báo | Mở danh sách thông báo hệ thống | Bấm biểu tượng Chuông | Mở Popup thông báo với danh sách các sự kiện mới, thời gian và trạng thái | **PASS** |
| **TC-16** | Đánh dấu đã đọc | Xóa badge đỏ thông báo | Bấm "Đánh dấu đã đọc" trong Popup | Badge đỏ biến mất, số lượng cập nhật về (0), các item chuyển mờ | **PASS** |
| **TC-17** | Điều hướng từ thông báo | Mở bài nộp cần chấm từ thông báo | Bấm thông báo "2 bài nộp Đề thi IELTS..." | Popup đóng, chuyển thẳng sang `DeThiLuyenTapView` | **PASS** |
| **TC-18** | Từ vựng hôm nay (Rotation)| Kiểm tra xoay vòng từ vựng theo ngày | Truy vấn CSDL `TuVung` theo thuật toán xoay vòng ngày | Hiển thị từ vựng hợp lệ, IPA, nghĩa, ví dụ; thay đổi sang từ khác khi sang ngày mới | **PASS** |
| **TC-19** | Phát âm từ vựng | Bấm nút loa phát âm | Bấm nút loa cạnh từ vựng hôm nay | Gọi `AIPronunciationService`, phát âm thanh chuẩn; debounce 1.2s chặn spam click | **PASS** |
| **TC-20** | Tạo Flashcard từ vựng | Điền trước thông tin từ vựng hôm nay | Bấm nút "Tạo Flashcard" | Mở `TaoFlashcardDialog` với từ, phiên âm, nghĩa và ví dụ đã điền sẵn | **PASS** |
| **TC-21** | Ôn tập Flashcard | Mở tab ôn tập từ vựng | Bấm vào Banner sách hoặc "Xem tất cả" | Mở `TaoFlashcardDialog` ở tab ôn tập (`openStudyTab = true`) | **PASS** |
| **TC-22** | Tài nguyên Ngữ pháp | Điều hướng tài nguyên Ngữ pháp A1-B1 | Bấm vào thẻ "Ngữ pháp tiếng Anh cơ bản" | Mở `NoiDungHocTapView` lọc chuyên đề Ngữ pháp | **PASS** |
| **TC-23** | Tài nguyên IELTS | Điều hướng tài nguyên Bộ đề IELTS | Bấm vào thẻ "Bộ đề IELTS 2024" | Mở `DeThiLuyenTapView` với từ khóa "IELTS" | **PASS** |
| **TC-24** | Tài nguyên TOEIC | Điều hướng tài nguyên TOEIC | Bấm vào thẻ "1000 từ vựng TOEIC" | Mở `DeThiLuyenTapView` với từ khóa "TOEIC" | **PASS** |
| **TC-25** | Tài nguyên IPA | Điều hướng tài nguyên Phát âm IPA | Bấm vào thẻ "Phát âm IPA" | Mở `NoiDungHocTapView` lọc kỹ năng Phát âm | **PASS** |
| **TC-26** | Tìm kiếm bài học/đề thi | Tìm bài học tại khu vực trung tâm | Gõ từ khóa vào `txtSearchNoiDung` và bấm `Enter` | Chuyển sang `NoiDungHocTapView` lọc theo từ khóa | **PASS** |
| **TC-27** | Xem tất cả nội dung | Xem toàn bộ kho học liệu | Bấm "Xem tất cả" ở góc khu vực 2 | Mở `NoiDungHocTapView` | **PASS** |
| **TC-28** | Biểu đồ xu hướng tuần | Hiển thị biểu đồ 7 ngày | Nạp dữ liệu `TongQuanView` | Hiển thị 7 cột ngày tương ứng số lượt thi thực tế từ CSDL, có tooltip chi tiết | **PASS** |
| **TC-29** | Chỉnh sửa Bài học 1 | Mở sửa bài học nổi bật 1 | Bấm nút "Chỉnh sửa" tại Bài học 1 | Mở `NoiDungHocTapView` chọn đúng `MaId` của bài học đó | **PASS** |
| **TC-30** | Chỉnh sửa Đề thi | Mở sửa đề thi nổi bật | Bấm nút "Chỉnh sửa" tại Đề thi | Mở `DeThiLuyenTapView` chọn đúng `MaId` của đề thi đó | **PASS** |
| **TC-31** | Làm thử Đề thi (Play) | Vào làm bài thi nhanh | Bấm Banner ▶️ màu xanh tại Đề thi | Mở `DeThiLuyenTapView` với đề thi đã chọn sẵn sàng làm bài | **PASS** |
| **TC-32** | Chỉnh sửa Bài học 2 | Mở sửa bài học nổi bật 2 | Bấm nút "Chỉnh sửa" tại Bài học 2 | Mở `NoiDungHocTapView` chọn đúng `MaId` của bài học đó | **PASS** |
| **TC-33** | Chỉ số 4 KPI chính | Khớp số liệu với cơ sở dữ liệu thực | Kiểm tra 4 ô: Học viên, Bài học, Câu hỏi, Lượt thi | Khớp chính xác với database: 10 học viên, 39 bài học, 719 câu hỏi, 10 lượt thi | **PASS** |
| **TC-34** | Bộ lọc thời gian KPI | Chuyển đổi Hôm nay / 7 ngày / Tháng này | Bấm nhãn chu kỳ thời gian | Chu kỳ chuyển `Hôm nay ▼` -> `7 ngày qua ▼` -> `Tháng này ▼`, tải lại KPI và trend | **PASS** |
| **TC-35** | Xem phân tích học viên | Điều hướng sang phân tích học tập | Bấm nút "Xem phân tích học viên" | Mở đúng màn hình `ThongKeHocTapGiangVienView` | **PASS** |
| **TC-36** | Live Feed Vừa nộp bài | Cập nhật tự động bài nộp mới | Chờ 30 giây chạy ngầm | `DispatcherTimer` kích hoạt làm mới danh sách nộp bài mà không làm lag giao diện | **PASS** |
| **TC-37** | Điều hướng từ bài nộp | Bấm vào một bài nộp trong Live feed | Nhấp vào hàng học viên vừa nộp bài | Mở `DeThiLuyenTapView` lọc theo tên học viên để tra cứu bài thi | **PASS** |
| **TC-38** | Phân nhóm học viên | Hiển thị phân bổ A1, B1, B2, C1 | Kiểm tra số lượng học viên 4 nhóm | Số liệu thực từ bảng khảo sát: A1 (4), B1 (3), B2 (2), C1 (1) | **PASS** |
| **TC-39** | Lọc theo Nhóm học | Bấm vào nhóm trình độ | Bấm vào nhóm A1, B1, B2 hoặc C1 | Mở `NguoiDungView` lọc đúng trình độ tương ứng | **PASS** |
| **TC-40** | Các nút Thao tác nhanh | Bấm 4 nút hành động nhanh | Bấm Tạo câu hỏi, Tạo đề thi, AI sinh câu hỏi, Giám sát | Mở chính xác 4 màn hình tương ứng | **PASS** |
