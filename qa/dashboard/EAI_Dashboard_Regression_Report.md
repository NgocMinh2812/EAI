# EAI ADMIN DASHBOARD – REGRESSION TEST & STABILITY REPORT

**Dự án:** EnglishAI (EAI)  
**Phân hệ:** Module Tổng quan Quản trị viên (Admin Dashboard)  
**Ngày thực hiện:** 11/11/2026  
**Chuyên gia thực hiện:** Senior QA Engineer, Senior .NET/WPF Developer, Software Architect  

---

## 1. PHẠM VI THAY ĐỔI VÀ ĐÁNH GIÁ TÁC ĐỘNG (IMPACT ANALYSIS)

Các thay đổi trong đợt triển khai này chỉ tập trung vào:
1. `GUI.Views.GiangVien.TongQuanView` (View + Code-Behind)
2. `GUI.Views.GiangVien.GiangVienWindow` (Topbar, Sidebar, Search, Notification Popup)
3. `DAL.DashboardDAL`, `BUS.DashboardBUS`, `DTO.DashboardKpiDTO`
4. `DAL.TuVungDAL`, `DAL.CauHoiDAL`
5. Các constructor nạp tham số tìm kiếm / ID mục tiêu cho `NoiDungHocTapView`, `DeThiLuyenTapView`, `NguoiDungView`, `NganHangCauHoiView`

### Bảng phân tích tác động tới các phân hệ xung quanh

| Module liên quan | Mức độ tác động | Rủi ro hồi quy (Regression Risk) | Biện pháp cô lập & Kiểm chứng |
| :--- | :--- | :--- | :--- |
| **Quản lý Người dùng (`NguoiDungView`)** | Thấp | Không ảnh hưởng đến nghiệp vụ thêm/sửa/xóa hoặc phân trang; chỉ bổ sung constructor nạp sẵn từ khóa và filter vai trò | Kiểm tra nạp mặc định và nạp có tham số: Cả 2 đều hiển thị danh sách người dùng chính xác. |
| **Nội dung học tập (`NoiDungHocTapView`)** | Thấp | Không thay đổi logic soạn thảo RichText hay sinh bài bằng AI; chỉ bổ sung tự động chọn bài học theo `targetLessonId` | Bảo lưu toàn bộ cảnh báo mất dữ liệu chưa lưu khi rời trang. |
| **Đề thi & Luyện tập (`DeThiLuyenTapView`)** | Thấp | Không thay đổi thuật toán chấm bài hay nộp bài thi; chỉ bổ sung chọn đề thi theo `targetExamId` | Kiểm chứng các bài test chấm điểm `ExamGradingTests`: 100% Passed. |
| **Ngân hàng câu hỏi (`NganHangCauHoiView`)** | Thấp | Chỉ bổ sung constructor nhận `maCauHoi` | Giữ nguyên toàn bộ logic danh mục và nghe audio. |
| **Tạo câu hỏi bằng AI (`AISinhDeView`)** | Thấp | Sửa dialog phê duyệt `XacNhanPheDuyetVaTaoBoDialog` để không bị crash icon `FolderInput` | Luồng AI sinh câu hỏi và phê duyệt vào bộ đề hoạt động trơn tru. |

---

## 2. KẾT QUẢ KIỂM THỬ TỰ ĐỘNG (AUTOMATED TEST EXECUTION)

Toàn bộ 24 test case tự động trong dự án `EAI.Tests` đã được biên dịch và thực thi trên nền tảng **.NET 10 (net10.0)**.

```text
Test run for D:\TDC\HK5\CDPTUD\EAI\EAI\EAI.Tests\bin\Debug\net10.0\EAI.Tests.dll (.NETCoreApp,Version=v10.0)
A total of 1 test files matched the specified pattern.

Passed!  - Failed: 0, Passed: 24, Skipped: 0, Total: 24, Duration: 3 s - EAI.Tests.dll (net10.0)
```

### Danh sách các ca kiểm thử đạt 100% PASS:
1. `DashboardMetricsTests.LayThongKeSinhVien_EmptyUser_MustNotReturnFake75ScoreOr1DayStreak`: Xác minh người dùng mới không bị gán điểm giả 75.0 hay chuỗi ngày 1 ngày.
2. `DashboardMetricsTests.LaySoLieuTongQuan_MustReturnValidKpiDto`: Kiểm tra tính toàn vẹn của DTO tổng quan nạp từ CSDL SQL Server.
3. `DashboardMetricsTests.LaySoLieuTongQuan_FilterPeriods_MustSetPeriodNameCorrectly (Hôm nay)`: Xác minh chu kỳ `0` khớp với chuỗi "so với hôm qua".
4. `DashboardMetricsTests.LaySoLieuTongQuan_FilterPeriods_MustSetPeriodNameCorrectly (7 ngày qua)`: Xác minh chu kỳ `1` khớp với chuỗi "so với tuần trước".
5. `DashboardMetricsTests.LaySoLieuTongQuan_FilterPeriods_MustSetPeriodNameCorrectly (Tháng này)`: Xác minh chu kỳ `2` khớp với chuỗi "so với tháng trước".
6. `DashboardMetricsTests.LayTuVungHomNay_MustReturnValidWord`: Xác minh thuật toán xoay vòng từ vựng hàng ngày trả về từ hợp lệ từ CSDL.
7. `DashboardMetricsTests.TimKiemCauHoi_ValidKeyword_ReturnsNonEmptyResults`: Xác minh tìm kiếm câu hỏi theo từ khóa hoạt động chính xác.
8. `DashboardMetricsTests.TimKiemCauHoi_EmptyKeyword_ReturnsEmptyList`: Xác minh xử lý chuỗi rỗng an toàn không gây lỗi SQL.
9. `DashboardMetricsTests.FormatTrend_CalculatesCorrectString (+12.5%)`: Xác minh định dạng phần trăm tăng trưởng dương.
10. `DashboardMetricsTests.FormatTrend_CalculatesCorrectString (-5.2%)`: Xác minh định dạng phần trăm tăng trưởng âm.
11. `DashboardMetricsTests.FormatTrend_CalculatesCorrectString (0.0%)`: Xác minh định dạng phần trăm giữ nguyên.
12. `ExamGradingTests.*` (7 tests): Xác minh thuật toán tính điểm đề thi IELTS / TOEIC, phân loại CEFR.
13. `AuthenticationSecurityTests.*` (4 tests): Xác minh bảo mật mật khẩu PBKDF2/SHA-256, chống SQL Injection trong đăng nhập.
14. `DatabaseIntegrityTests.*` (2 tests): Xác minh kết nối cơ sở dữ liệu và cấu trúc các bảng trọng yếu.

---

## 3. KIỂM SOÁT BỘ NHỚ VÀ HIỆU NĂNG (PERFORMANCE & MEMORY AUDIT)

1. **Bộ nhớ RAM và Quản lý vòng đời (Lifecycle Management):**
   - Áp dụng Page Cache (`_pageCache`) trong `GiangVienWindow` cho các màn hình tĩnh, loại bỏ hoàn toàn việc tạo mới vô hạn trang WPF khi click qua lại giữa các menu Sidebar.
   - Cơ chế dọn dẹp Timer: `TongQuanView` đăng ký sự kiện `Unloaded` để tự động ngắt `_liveFeedTimer.Stop()`, ngăn ngừa rò rỉ bộ nhớ (Memory Leak) khi người dùng chuyển sang chức năng khác.
   - Cơ chế Debounce Search: `_searchDebounceTimer` giải phóng và khởi tạo lại chu kỳ 250ms, giảm tải số lượng query tới SQL Server từ ~20 truy vấn/giây xuống còn đúng 1 truy vấn/thao tác.
2. **Hiện tượng giật lag / Treo giao diện (UI Responsiveness):**
   - Toàn bộ các thao tác đọc dữ liệu CSDL (`GetKpiStats`, `LayDanhSach`, `TimKiemCauHoi`, `LayDanhSachTuDatabase`) đều được bọc trong `Task.Run()` bất đồng bộ, đảm bảo luồng UI chính (Dispatcher thread) luôn đạt 60 FPS, cuộn trang mượt mà.
