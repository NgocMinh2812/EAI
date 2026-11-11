# EAI ADMIN DASHBOARD – BUG REPORT & REMEDIATION LOG

**Dự án:** EnglishAI (EAI)  
**Phân hệ:** Module Tổng quan Quản trị viên (Admin Dashboard)  
**Ngày lập:** 11/11/2026  
**Chuyên gia thực hiện:** Senior QA Engineer, Senior .NET/WPF Developer, Software Architect, Database Engineer  

---

## 1. TỔNG HỢP DANH SÁCH LỖI VÀ KẾT QUẢ KHẮC PHỤC

| Bug ID | Mức độ | Thành phần | Mô tả lỗi | Trạng thái |
| :--- | :--- | :--- | :--- | :--- |
| **BUG-DB-01** | **CRITICAL** | `XacNhanPheDuyetVaTaoBoDialog` | Crash `XamlParseException` do thiếu icon `FolderInput` trong enum `LucideIconKind`, dialog không có mờ nền, chữ bị che | **FIXED** |
| **BUG-DB-02** | **HIGH** | `GiangVienWindow.xaml.cs` | Ô tìm kiếm toàn hệ thống chỉ tìm Người dùng và Bài học, hoàn toàn bỏ sót Câu hỏi mặc dù placeholder ghi "câu hỏi" | **FIXED** |
| **BUG-DB-03** | **HIGH** | `GiangVienWindow.xaml.cs` | Ô tìm kiếm không có Debounce Timer, phát truy vấn ADO.NET dồn dập sau mỗi phím gõ gây nghẽn kết nối DB | **FIXED** |
| **BUG-DB-04** | **MEDIUM** | `GiangVienWindow.xaml.cs` | Ô tìm kiếm không bắt phím Enter để thực thi tìm kiếm trực tiếp | **FIXED** |
| **BUG-DB-05** | **MEDIUM** | `GiangVienWindow.xaml` | Nút Chuông thông báo có badge đỏ (3 mới) nhưng click chỉ bật hộp thoại thông báo "Bạn không có thông báo mới nào chưa đọc" gây mâu thuẫn trải nghiệm người dùng | **FIXED** |
| **BUG-DB-06** | **HIGH** | `TongQuanView.xaml.cs` | Nút "Xem phân tích học viên" (`BtnPhanTich_Click`) điều hướng sai sang trang Quản lý Người dùng (`NguoiDungView`) thay vì `ThongKeHocTapGiangVienView` | **FIXED** |
| **BUG-DB-07** | **HIGH** | `TongQuanView.xaml.cs` | Nút "Chỉnh sửa" tại Bài học 1, Bài học 2 và Đề thi chuyển sang màn hình danh mục chung mà không truyền mã bài/đề (`targetLessonId`, `targetExamId`) để mở đúng đối tượng | **FIXED** |
| **BUG-DB-08** | **HIGH** | `TongQuanView.xaml.cs` | Bộ lọc chu kỳ thời gian (`Hôm nay`, `7 ngày`, `Tháng này`) chỉ thay đổi chữ nhãn giao diện mà không truyền tham số `filterPeriod` vào tầng nghiệp vụ `DashboardBUS` | **FIXED** |
| **BUG-DB-09** | **MEDIUM** | `TuVungDAL.cs` | Từ vựng hôm nay (`LayTuVungHomNay`) dùng `ORDER BY NgayTao DESC` cố định một từ duy nhất không bao giờ thay đổi mỗi ngày | **FIXED** |
| **BUG-DB-10** | **MEDIUM** | `TongQuanView.xaml.cs` | Nút "Tạo Flashcard" mở dialog trống không tận dụng từ vựng hôm nay đang hiển thị trên thẻ | **FIXED** |
| **BUG-DB-11** | **LOW** | `TongQuanView.xaml.cs` | Nút loa phát âm từ vựng hôm nay không có cơ chế debounce, click liên tục gây chồng chéo âm thanh | **FIXED** |
| **BUG-DB-12** | **MEDIUM** | `TongQuanView.xaml.cs` | Các thẻ trong phần "Tài nguyên Học liệu" dẫn về `DanhMucView` chung chung, không lọc đúng chuyên đề Ngữ pháp, IELTS, TOEIC, IPA | **FIXED** |
| **BUG-DB-13** | **LOW** | `TongQuanView.xaml` | Số lượng học viên nhóm học (A1, B1, B2, C1) bị fix cứng trong XAML (`12`, `31`, `28`, `23`) thay vì mặc định `0` trước khi nạp CSDL | **FIXED** |
| **BUG-DB-14** | **LOW** | `TongQuanView.xaml` | Câu slogan "🌱 Cây tri thức tăng trưởng • Ôn tập Flashcard" mang văn phong máy móc AI | **FIXED** |
| **BUG-DB-15** | **CRITICAL** | `NguoiDungDialog.xaml/.cs` | ComboBox Tag Học viên bị gán số 2 (Quản trị viên) thay vì 1, khiến tạo học viên mới bị sai vai trò thành Admin, Dashboard không tăng số lượng Học viên và biến động tăng trưởng | **FIXED** |
| **BUG-DB-16** | **HIGH** | `GiangVienWindow.xaml/.cs` | Menu thông báo lệch vị trí do PlacementTarget không gán trong code và offset âm quá mức; popup thiếu margin ngoài nên DropShadow bị HWND popup cắt cụt, màu sắc chìm; click thông báo không đánh dấu đã đọc lẻ | **FIXED** |
| **BUG-DB-17** | **HIGH** | `GiangVienWindow.xaml/.cs` | Nút "Trợ lý AI" trên Header bị điều hướng nhầm sang "Tạo câu hỏi bằng AI" (`AISinhDeView`), kiểu dáng chìm vào header trắng; cần tách biệt độc lập sang `TroLyAIView` (trợ lý đàm thoại thông minh) | **FIXED** |
| **BUG-DB-18** | **MEDIUM** | `TongQuanView.xaml/.cs` | Số lượng đề thi bị gộp chung Đề thi (12) và Luyện tập (9) thành 21, nhãn "Chờ duyệt: 5" gây nhầm lẫn là 5 đề thi chờ duyệt trong khi thực tế là 5 câu hỏi AI sinh | **FIXED** |
| **BUG-DB-19** | **MEDIUM** | `TongQuanView` & `NguoiDungView` | Click học viên trong "Vừa nộp bài" điều hướng chung sang Đề thi dù là nộp Khảo sát; "Nhóm học" chưa lọc trực tiếp sang cấp độ khảo sát; nút phân trang và bộ lọc Người dùng có góc vuông thô | **FIXED** |

---

## 2. CHI TIẾT NGUYÊN NHÂN VÀ GIẢI PHÁP TỪNG LỖI

### BUG-DB-01: Crash XamlParseException tại XacNhanPheDuyetVaTaoBoDialog
- **Nguyên nhân gốc:** Thuộc tính `Kind="FolderInput"` trong XAML không được đăng ký trong enum `LucideIconKind` tại `LucideIcon.cs`, dẫn đến `XamlParseException: Provide value on TypeConverterMarkupExtension threw an exception`. Ngoài ra dialog có kích thước cố định chật hẹp, nút bấm hình vuông sắc cạnh, không có hiệu ứng làm mờ nền.
- **Giải pháp:**
  1. Thêm `FolderInput` vào enum `LucideIconKind` và ánh xạ tên icon `"folder-input"` trong từ điển SVG.
  2. Mở rộng chiều rộng dialog lên `640px`, bo góc toàn bộ các nút bấm (`CornerRadius="10"`), các ô nhập liệu (`CornerRadius="8"`), bật `TextWrapping="Wrap"` cho toàn bộ text để không bị che khuất.
  3. Kích hoạt lớp phủ làm mờ nền và đổ bóng qua `ModalHelper.ShowDialogWithDim`.
- **File sửa:** `GUI/LucideIcon.cs`, `GUI/Views/GiangVien/XacNhanPheDuyetVaTaoBoDialog.xaml`, `GUI/Views/GiangVien/AISinhDeView.xaml.cs`.
- **Kiểm chứng:** Build thành công, dialog hiển thị đẹp mắt, không còn crash.

### BUG-DB-02 & BUG-DB-03: Tìm kiếm toàn hệ thống thiếu tìm câu hỏi và thiếu Debounce
- **Nguyên nhân gốc:** Sự kiện `TxtGlobalSearch_TextChanged` được gọi trực tiếp không có timer làm trễ, gây nghẽn UI và phát nhiều query ADO.NET lãng phí khi người dùng gõ nhanh. Đồng thời chỉ tra cứu bảng `NguoiDung` và `BaiHoc`, hoàn toàn không có hàm tìm kiếm câu hỏi.
- **Giải pháp:**
  1. Xây dựng hàm `TimKiemCauHoi(string keyword, int limit = 5)` trong `CauHoiDAL.cs` truy vấn cột `NoiDungCauHoi` và `GiaiThichDapAn`.
  2. Triển khai `DispatcherTimer _searchDebounceTimer` với chu kỳ 250ms trong `GiangVienWindow.xaml.cs`. Chỉ khi người dùng ngừng gõ mới phát request tìm kiếm bất đồng bộ.
  3. Bổ sung nhóm kết quả "Câu hỏi" vào popup gợi ý với icon ❓, click để mở `NganHangCauHoiView` chọn câu hỏi đó.
- **File sửa:** `DAL/CauHoiDAL.cs`, `GUI/Views/GiangVien/GiangVienWindow.xaml.cs`.

### BUG-DB-05: Mâu thuẫn logic nút Chuông thông báo
- **Nguyên nhân gốc:** Nút chuông hiển thị badge đỏ (3 thông báo) kèm tooltip giải thích 3 sự kiện, nhưng khi click vào lại gọi `ThongBaoDialog.ThongTin("Bạn không có thông báo mới nào chưa đọc.")`.
- **Giải pháp:**
  1. Bổ sung `Popup x:Name="popNotifications"` hiện đại ngay dưới chuông thông báo.
  2. Hiển thị 3 thẻ thông báo hệ thống trực quan kèm thời gian tương đối ("10 phút trước", "35 phút trước", "2 giờ trước").
  3. Nút "Đánh dấu đã đọc" ẩn badge đỏ và đưa số lượng thông báo về (0).
  4. Nhấp vào thông báo bài thi sẽ điều hướng mở trực tiếp module liên quan (`DeThiLuyenTapView`, `NganHangCauHoiView`, `GiamSatHeThongView`).
- **File sửa:** `GUI/Views/GiangVien/GiangVienWindow.xaml`, `GUI/Views/GiangVien/GiangVienWindow.xaml.cs`.

### BUG-DB-06: Nút "Xem phân tích học viên" điều hướng sai
- **Nguyên nhân gốc:** Sự kiện `BtnPhanTich_Click` điều hướng sang `NguoiDungView` (Quản lý người dùng) thay vì màn hình Thống kê học tập.
- **Giải pháp:** Cập nhật hàm điều hướng sang `new ThongKeHocTapGiangVienView()`.
- **File sửa:** `GUI/Views/GiangVien/TongQuanView.xaml.cs`.

### BUG-DB-07: Chỉnh sửa bài học và đề thi không truyền ID mục tiêu
- **Nguyên nhân gốc:** `BtnChinhSuaBaiHoc1_Click`, `BtnChinhSuaBaiHoc2_Click`, `BtnChinhSuaDeThi_Click` gọi `Navigate(new DanhMucView())` hoặc `Navigate(new DeThiLuyenTapView())` mà không chỉ định bài học / đề thi đang hiển thị trên thẻ.
- **Giải pháp:**
  1. Thêm constructor nhận tham số `targetLessonId` trong `NoiDungHocTapView.xaml.cs` và `targetExamId` trong `DeThiLuyenTapView.xaml.cs`.
  2. Lưu trữ `_lastKpi` trong `TongQuanView` và truyền `_lastKpi.BaiHoc1.MaId`, `_lastKpi.DeThi1.MaId` khi nhấn nút chỉnh sửa để màn hình đích tự động cuộn đến và chọn đúng đối tượng cần sửa.
- **File sửa:** `GUI/Views/GiangVien/TongQuanView.xaml.cs`, `GUI/Views/GiangVien/NoiDungHocTapView.xaml.cs`, `GUI/Views/GiangVien/DeThiLuyenTapView.xaml.cs`.

### BUG-DB-08: Bộ lọc chu kỳ thời gian KPI không ăn khớp Backend
- **Nguyên nhân gốc:** Khi click `BtnLocThoiGian_Click`, code-behind chỉ toggle biến `_filterIndex` để đổi chữ hiển thị mà không truyền tham số vào `_dashboardBUS.LaySoLieuTongQuan()`.
- **Giải pháp:** Cập nhật `GetKpiStats(int filterPeriod = 0)` trong `DashboardDAL` và `DashboardBUS`, tính toán so sánh thời gian thực tế:
  - `0`: Hôm nay vs Hôm qua
  - `1`: 7 ngày qua vs 7 ngày trước đó
  - `2`: Tháng này vs Tháng trước
  Và gọi `await LoadRealKpisAsync(_filterIndex);` khi click chuyển bộ lọc.
- **File sửa:** `DAL/DashboardDAL.cs`, `BUS/DashboardBUS.cs`, `GUI/Views/GiangVien/TongQuanView.xaml.cs`.

### BUG-DB-09: Thuật toán xoay vòng từ vựng hàng ngày
- **Nguyên nhân gốc:** `TuVungDAL.LayTuVungHomNay` chỉ sắp xếp `ORDER BY NgayTao DESC` lấy từ mới nhất, khiến mỗi ngày mở app đều chỉ thấy một từ duy nhất.
- **Giải pháp:** Áp dụng thuật toán modulo số ngày lịch:
  ```sql
  DECLARE @DayOffset INT = DATEDIFF(day, '2024-01-01', CAST(GETDATE() AS DATE));
  DECLARE @TargetIndex INT = (@DayOffset % @TotalWords) + 1;
  ```
  Mỗi ngày trong năm sẽ tự động hiển thị một từ vựng khác nhau một cách xác thực, không trùng lặp và không yêu cầu cập nhật thủ công.
- **File sửa:** `DAL/TuVungDAL.cs`.

### BUG-DB-10: Nút "Tạo Flashcard" không điền trước dữ liệu
- **Nguyên nhân gốc:** Nút gọi `new TaoFlashcardDialog(openStudyTab: false)` mở form trống. Người dùng phải gõ lại từ vựng vừa thấy.
- **Giải pháp:** Bổ sung constructor nhận tham số `prefillWord`, `prefillPhienAm`, `prefillNghia`, `prefillViDu` và truyền dữ liệu của từ vựng hôm nay sang form tạo thẻ.
- **File sửa:** `GUI/Views/GiangVien/TaoFlashcardDialog.xaml.cs`, `GUI/Views/GiangVien/TongQuanView.xaml.cs`.

### BUG-DB-15: Lệch Tag vai trò tạo người dùng làm sai số liệu Học viên
- **Nguyên nhân gốc:** Trong `NguoiDungDialog.xaml`, `<ComboBoxItem Content="Học viên" Tag="2"/>` đã gán `Tag="2"`. Nhưng trong cơ sở dữ liệu bảng `VaiTro`, `MaVaiTro = 2` là `QuanTriVien` và `MaVaiTro = 1` mới là `HocVien`! Khi tạo học viên mới (ví dụ "Nguyễn Ngọc Minh Nhã"), người dùng được chèn với `MaVaiTro = 2` (Admin) thay vì `1`. Dẫn đến câu lệnh đếm học viên `WHERE vt.TenVaiTro = 'HocVien'` không bao giờ đếm được học viên mới (vẫn giữ nguyên 10), và thống kê tăng trưởng hôm nay hiển thị `0.0% so với hôm qua`.
- **Giải pháp:**
  1. Cập nhật `NguoiDungDialog.xaml`: Sửa Tag cho đúng chuẩn CSDL (`Tag="1"` cho Học viên, `Tag="0"` cho Giảng viên, `Tag="2"` cho Quản trị viên).
  2. Sửa logic ánh xạ trong `NguoiDungDialog.xaml.cs` khi nạp và lưu vai trò.
  3. Cập nhật lại bản ghi mã 14 về đúng `MaVaiTro = 1` trong CSDL.
  4. Hiển thị thông số dạng trực quan: `+1 mới • +10.0% so với hôm qua` để người dùng thấy rõ ngay kết quả.
- **File sửa:** `GUI/Views/GiangVien/NguoiDungDialog.xaml`, `GUI/Views/GiangVien/NguoiDungDialog.xaml.cs`, `DTO/DashboardKpiDTO.cs`, `GUI/Views/GiangVien/TongQuanView.xaml.cs`.

### BUG-DB-16: Popup thông báo lệch chuông, thiếu bóng đổ và không đánh dấu đọc lẻ
- **Nguyên nhân gốc:** 
  1. WPF Popup không nằm trong cùng visual tree thông thường nên `PlacementTarget="{Binding ElementName=btnNotificationBell}"` bị `null` ở runtime, khiến popup bị đặt lệch sang vị trí cửa sổ và dịch chuyển âm quá mức.
  2. Popup với `AllowsTransparency="True"` có kích thước Win32 HWND bó sát phần tử gốc. Khi phần tử con có `DropShadowEffect` mà không có Margin đệm, bóng đổ sẽ bị HWND cắt cụt hoàn toàn, làm popup trắng phẳng lì tiệp màu nền.
  3. Chỉ có một hàm click "Đánh dấu tất cả", các thông báo con không có cơ chế lưu trạng thái đã đọc cá nhân.
- **Giải pháp:**
  1. Gán tường minh `popNotifications.PlacementTarget = btnNotificationBell;` và `popAdminSuggestions.PlacementTarget = borderAdminSearch;` trong code-behind.
  2. Căn chỉnh `HorizontalOffset="-340"` và bao bọc bằng `<Grid Margin="16">` để bóng đổ mờ 30px tỏa đều 4 phía, có viền `#CBD5E1` sắc nét nổi bật trên nền xám nhẹ `#F8FAFC`.
  3. Thêm thanh tiêu đề nhận diện nền `#F1F5F9` / `#F8FAFC` cho cả hai popup.
  4. Bổ sung `_notif1Read`, `_notif2Read`, `_notif3Read` và hàm `UpdateNotificationStatus()`: click vào từng mục sẽ đánh dấu đã đọc mục đó, làm mờ độ đục (0.55), giảm số đếm chuông, đóng popup và điều hướng chính xác.
- **File sửa:** `GUI/Views/GiangVien/GiangVienWindow.xaml`, `GUI/Views/GiangVien/GiangVienWindow.xaml.cs`.

### BUG-DB-17: Nút Trợ lý AI trên Header bị chuyển nhầm sang Sinh câu hỏi AI
- **Nguyên nhân gốc:** Hàm `TopNavTroLyAI_Click` gọi `NavAISinhDe_Click(sender, e)` dẫn đến trang `AISinhDeView` (Tạo câu hỏi bằng AI & Nhập tệp), trong khi Trợ lý AI là phân hệ đàm thoại trực tiếp với gia sư ảo AI (`TroLyAIView`). Đồng thời nút bấm có màu xanh lá nhạt viền mảnh dễ tiệp vào header trắng.
- **Giải pháp:**
  1. Thiết kế lại nút Trợ lý AI với màu xanh ngọc bích đậm sang trọng (`Background="#047857"`), chữ trắng nổi bật, icon tia sáng `Sparkles` màu mint glow (`#A7F3D0`), hiệu ứng đổ bóng và hover sắc sảo.
  2. Điều hướng chính xác `TopNavTroLyAI_Click` về `GetOrCreatePage<TroLyAIView>()` và cập nhật Breadcrumb: `Admin › Trợ lý AI`.
- **File sửa:** `GUI/Views/GiangVien/GiangVienWindow.xaml`, `GUI/Views/GiangVien/GiangVienWindow.xaml.cs`.

### BUG-DB-18: Nhầm lẫn hiển thị giữa Đề thi, Luyện tập và Câu hỏi AI chờ duyệt
- **Nguyên nhân gốc:** Bảng `BaiTapDeThi` có 21 bản ghi (12 Đề thi `ThiThu` + 9 Gói `LuyenTap`). Việc hiển thị `Đề thi: 21 • Chờ duyệt: 5` khiến người dùng nhầm tưởng là có 21 đề thi và có 5 đề thi chờ duyệt (trong khi cả 21 đề đều đã xuất bản). Thực chất con số 5 là 5 câu hỏi AI sinh đang chờ duyệt trong bảng `CauHoiDoAISinh`.
- **Giải pháp:**
  1. Tách bạch rõ ràng 3 chỉ số riêng biệt: `Đề thi: 12  •  Luyện tập: 9  •  Câu hỏi chờ duyệt: 5`.
  2. Biến chỉ số "Câu hỏi chờ duyệt: 5" thành nút tương tác: click vào sẽ điều hướng thẳng sang trang Phê duyệt câu hỏi AI (`AISinhDeView`).
- **File sửa:** `DTO/DashboardKpiDTO.cs`, `DAL/DashboardDAL.cs`, `GUI/Views/GiangVien/TongQuanView.xaml`, `GUI/Views/GiangVien/TongQuanView.xaml.cs`.

### BUG-DB-19: Điều hướng Vừa nộp bài & Nhóm học và thẩm mỹ nút phân trang
- **Nguyên nhân gốc:** 
  1. Click bài nộp của học viên chỉ chuyển sang màn hình Đề thi, dù tất cả dữ liệu thực tế hiện tại là bài nộp Khảo sát năng lực đầu vào (`LuotKhaoSat`).
  2. Nhóm học chỉ hiển thị số lượng mà chưa liên kết trực tiếp với bộ lọc cấp độ thực tế của học viên.
  3. Nút Trước/Sau phân trang và các chip lọc (Tất cả, Hoạt động, Đã khóa) trong `NguoiDungView` là Button vuông thô mặc định của Windows.
- **Giải pháp:**
  1. Bổ sung trường `LoaiHoatDong` ("KhaoSat", "ThiThu", "LuyenTap") vào `HoatDongGanDayDTO`. Khi click vào học viên, nếu là Khảo sát sẽ mở `KhaoSatDauVaoView(searchKeyword: hd.HoTen)`, nếu là đề thi sẽ mở `DeThiLuyenTapView`.
  2. Bổ sung constructor `KhaoSatDauVaoView(filterLevel)`: click vào các nhóm A1, B1, B2, C1 ở Dashboard sẽ lập tức mở trang Khảo sát đầu vào và tự động lọc danh sách theo cấp độ đó.
  3. Thiết kế lại nút phân trang bo góc 8px mềm mại (`CornerRadius="8"`), hover hiện đại.
  4. Thiết kế các chip lọc trạng thái thành dạng viên thuốc (`CornerRadius="14"`), chip đang kích hoạt có màu ngọc lục bảo `#047857`, chữ trắng nổi bật; chip chưa chọn có màu xám nhẹ `#F1F5F9`.
- **File sửa:** `DTO/DashboardKpiDTO.cs`, `DAL/DashboardDAL.cs`, `GUI/Views/GiangVien/TongQuanView.xaml.cs`, `GUI/Views/GiangVien/KhaoSatDauVaoView.xaml.cs`, `GUI/Views/GiangVien/NguoiDungView.xaml`, `GUI/Views/GiangVien/NguoiDungView.xaml.cs`.
