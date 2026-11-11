# EAI ADMIN DASHBOARD – FEATURE INVENTORY & ARCHITECTURAL MAPPING

**Dự án:** EnglishAI (EAI)  
**Phân hệ:** Tổng quan Quản trị viên (Admin Dashboard)  
**Ngày lập:** 11/11/2026  
**Nhóm phụ trách:** Senior QA Engineer, Senior .NET/WPF Developer, Software Architect, Database Engineer, UI/UX Designer, UX Writer  

---

## 1. TỔNG QUAN KIẾN TRÚC VÀ LUỒNG DỮ LIỆU

### 1.1 Sơ đồ luồng xử lý
```
[Giao diện WPF (XAML)]
  TongQuanView.xaml / GiangVienWindow.xaml
        │
        ▼ (Event Handler & Command)
[Code-Behind / ViewModel]
  TongQuanView.xaml.cs / GiangVienWindow.xaml.cs
        │
        ▼ (Nghiệp vụ - Business Logic Layer)
  DashboardBUS / TuVungBUS / BaiHocBUS / NguoiDungBUS
        │
        ▼ (Truy xuất dữ liệu - Data Access Layer)
  DashboardDAL / TuVungDAL / CauHoiDAL / BaiHocDAL / DatabaseHelper
        │
        ▼ (ADO.NET Direct Query / Parameterized SQL)
[Cơ sở dữ liệu SQL Server: TiengAnhAI]
  NguoiDung, BaiHoc, CauHoi, BaiTapDeThi, LuotKhaoSat, TuVung, CauHoiDoAISinh
```

### 1.2 Thành phần triển khai

| Tầng | File / Class | Trách nhiệm chính |
| :--- | :--- | :--- |
| **Shell Container** | `GUI.Views.GiangVien.GiangVienWindow` | Khung điều hướng toàn cục: Topbar, Sidebar, Modal Overlay Dimmer, Global Search, Live Notification popup |
| **View (Dashboard)** | `GUI.Views.GiangVien.TongQuanView` | Màn hình Tổng quan chứa 3 cột nội dung: Từ vựng/Tài nguyên, Nội dung/Đề thi/Xu hướng, Thống kê KPI/Live Feed/Nhóm học/Thao tác nhanh |
| **Business (BUS)** | `BUS.DashboardBUS` | Điều phối tổng hợp KPI hệ thống, tính toán tỷ lệ tăng trưởng theo chu kỳ (`0`: Hôm nay, `1`: 7 ngày, `2`: Tháng này), xu hướng tuần, danh sách nộp bài |
| **Business (BUS)** | `BUS.TuVungBUS` | Xử lý nghiệp vụ từ vựng hôm nay (Daily Word rotation) và quản lý Flashcard |
| **Data Access (DAL)**| `DAL.DashboardDAL` | Thực thi truy vấn ADO.NET đếm số liệu thực từ các bảng `NguoiDung`, `BaiHoc`, `CauHoi`, `BaiTapDeThi`, `LuotKhaoSat`, tính toán trend |
| **Data Access (DAL)**| `DAL.TuVungDAL` | Xoay vòng từ vựng theo thuật toán ngày `DATEDIFF(day, '2024-01-01', GETDATE()) % COUNT(*)` đảm bảo mỗi ngày 1 từ ngẫu nhiên cố định |
| **Data Access (DAL)**| `DAL.CauHoiDAL` | Cung cấp hàm tìm kiếm câu hỏi `TimKiemCauHoi(keyword, limit)` phục vụ thanh tìm kiếm toàn hệ thống |
| **Data Transfer (DTO)**| `DTO.DashboardKpiDTO` | Chứa dữ liệu đóng gói: 4 KPI chính, 2 phụ trợ, danh sách xu hướng `TrendPointDTO`, hoạt động gần đây `HoatDongGanDayDTO`, nhóm học |

---

## 2. FEATURE INVENTORY (DANH MỤC THÀNH PHẦN & TÍNH NĂNG CHI TIẾT)

### Khu vực A: Thanh điều hướng trên cùng (Topbar - `GiangVienWindow.xaml`)
1. **Logo & Nhãn hiệu:**
   - Icon logo sắc nét (`/Assets/logo_icon.png`).
   - Tên ứng dụng `TiếngAnhAI`, phụ đề phân quyền linh hoạt: `Admin Dashboard` (với tài khoản Quản trị viên) hoặc `Cổng Giảng Viên` (với tài khoản Giảng viên).
2. **Breadcrumb:**
   - Tự động cập nhật `Admin › Tổng quan` khi ở trang Dashboard và chuyển đổi tương ứng khi chuyển sang module khác (`Admin › Người dùng`, `Admin › Ngân hàng câu hỏi`...).
3. **Thanh tìm kiếm toàn hệ thống (`txtGlobalSearch`):**
   - Hỗ trợ Debounce Timer (250ms) chống spam truy vấn CSDL.
   - Hỗ trợ phím tắt `Ctrl + F` / `⌘ F` để kích hoạt focus.
   - Tìm kiếm Module điều hướng nội bộ.
   - Tìm kiếm Người dùng thực tế (`NguoiDungBUS.LayDanhSach`).
   - Tìm kiếm Bài học thực tế (`BaiHocBUS.LayDanhSachTuDatabase`).
   - Tìm kiếm Ngân hàng câu hỏi (`CauHoiDAL.TimKiemCauHoi`).
   - Hỗ trợ phím `Enter` để điều hướng sâu trực tiếp vào bài học / từ khóa.
   - Hỗ trợ phím `Esc` hoặc bấm ra ngoài để đóng Popup gợi ý.
4. **Nút Trợ lý AI (`TopNavTroLyAI`):**
   - Mở nhanh phân hệ `AISinhDeView` (Tạo câu hỏi bằng AI & Nhập tệp).
5. **Nút Chuông thông báo (`btnNotificationBell`):**
   - Red dot badge thông báo tin mới.
   - Popup thông báo trực quan (`popNotifications`) gồm danh sách thông báo hệ thống, thời gian, trạng thái.
   - Tương tác mở bài nộp thi IELTS, câu hỏi TOEIC, giám sát hệ thống.
   - Nút "Đánh dấu đã đọc" xóa badge và cập nhật số lượng thông báo về 0.
6. **Avatar & Hồ sơ tài khoản:**
   - Vòng tròn avatar hiển thị Initials họ tên (ví dụ: `HT`).
   - Click mở dialog cập nhật thông tin cá nhân `ProfileGiangVienDialog` kèm hiệu ứng mờ nền (`ModalHelper.ShowDialogWithDim`).
7. **Điều khiển cửa sổ:**
   - Nút Thu nhỏ (`BtnMinimize_Click` -> `WindowState.Minimized`).
   - Nút Đóng ứng dụng (`BtnCloseWindow_Click` -> `Application.Current.Shutdown` kèm bảo vệ chống mất dữ liệu chưa lưu nếu đang tạo bài).

---

### Khu vực B: Thanh bên (Sidebar - `GiangVienWindow.xaml`)
1. **Các mục điều hướng chính:**
   - **Tổng quan (`TongQuanView`):** Đánh dấu active sáng màu xanh Admin (`#064E3B`).
   - **Người dùng (`NguoiDungView`):** Quản lý học viên, giảng viên.
   - **Danh mục & Cấu trúc (`DanhMucView`):** Phân loại chủ đề, kỹ năng.
   - **Nội dung học tập (`NoiDungHocTapView`):** Kho bài giảng, tài liệu.
   - **Ngân hàng câu hỏi (`NganHangCauHoiView`):** Lưu trữ câu hỏi, bộ đề.
   - **Đề thi & Luyện tập (`DeThiLuyenTapView`):** Khảo thí, đề thi thử.
   - **Khảo sát đầu vào (`KhaoSatDauVaoView`):** Phân loại trình độ đầu vào.
   - **Thống kê học tập (`ThongKeHocTapGiangVienView`):** Báo cáo điểm số và chuyên cần.
   - **Tạo câu hỏi bằng AI (`AISinhDeView`):** AI Assistant sinh đề trắc nghiệm.
   - **Giám sát hệ thống (`GiamSatHeThongView`):** Phân quyền chỉ hiển thị cho Quản trị viên/Giảng viên.
   - **Trang chủ Học viên (`MainWindow`):** Chuyển đổi trải nghiệm sang giao diện học tập.
2. **Cơ chế Sidebar thu gọn (Collapsible Sidebar):**
   - Nút `«` thu gọn chiều rộng từ `240px` về `68px`.
   - Căn giữa các icon, ẩn nhãn chữ, chuyển avatar sang chế độ mini tooltip.
3. **Phần chân Sidebar:**
   - Họ tên, chức vụ, nút Đăng xuất an toàn kèm xác nhận dialog.

---

### Khu vực C: Thẻ Từ vựng hôm nay (`TongQuanView.xaml`)
- **Dữ liệu:** Lấy từ CSDL `TuVung` thông qua thuật toán xoay vòng hàng ngày trong `TuVungDAL.LayTuVungHomNay()`.
- **Thành phần:**
  - Từ vựng (Ví dụ: `Inevitable`, `perseverance`).
  - Phiên âm IPA chuẩn quốc tế.
  - Từ loại (`danh từ`, `tính từ`...).
  - Nghĩa tiếng Việt giải thích chuẩn xác.
  - Câu trích dẫn ví dụ đặt trong dấu ngoặc kép.
  - Nút phát âm (Icon `Volume2`) tích hợp `AIPronunciationService.PlayPronunciationAsync` kèm debounce 1.2s chống bấm liên tục gây nghẽn audio.
  - Banner thẻ ghi nhớ thông minh (`📚 Thẻ ghi nhớ thông minh • Bấm để ôn tập ngay`) click để mở trực tiếp tab ôn tập flashcard.
  - Nút **"Tạo Flashcard"**: Mở `TaoFlashcardDialog` pre-filled đầy đủ từ, phiên âm, nghĩa và câu ví dụ của từ vựng hôm nay.
  - Nút **"Xem tất cả"**: Mở dialog quản lý toàn bộ bộ thẻ từ vựng.

---

### Khu vực D: Thẻ Tài nguyên Học liệu (`TongQuanView.xaml`)
- Danh sách liên kết học liệu kèm điều hướng sâu theo chuyên đề thực tế:
  1. *Ngữ pháp tiếng Anh cơ bản — A1 đến B1:* Điều hướng trực tiếp sang `NoiDungHocTapView` lọc chuyên đề `Ngữ pháp`.
  2. *Bộ đề IELTS 2024 — Academic & General:* Điều hướng trực tiếp sang `DeThiLuyenTapView` với từ khóa `IELTS`.
  3. *1000 từ vựng TOEIC thông dụng nhất:* Điều hướng trực tiếp sang `DeThiLuyenTapView` với từ khóa `TOEIC`.
  4. *Phát âm IPA — Bảng ký hiệu và ví dụ âm:* Điều hướng sang `NoiDungHocTapView` lọc kỹ năng `Phát âm`.

---

### Khu vực E: Khu vực Nội dung & Đề thi (`TongQuanView.xaml`)
1. **Thanh tìm kiếm nội bộ (`txtSearchNoiDung`):**
   - Placeholder tự động ẩn khi focus, khôi phục khi để trống.
   - Nhấn `Enter` chuyển thẳng sang `NoiDungHocTapView(searchKeyword)` để tìm kiếm bài giảng tương ứng.
2. **Nút "Xem tất cả":**
   - Điều hướng sang `NoiDungHocTapView`.
3. **Biểu đồ Xu hướng học tập tuần qua (`icBieuDoXuHuong`):**
   - 7 cột đại diện cho 7 ngày gần nhất (T2 -> CN).
   - Chiều cao cột `CotHeight` co giãn theo số lượt làm bài thực tế trong CSDL `LuotKhaoSat` và `KetQuaLamBai`.
   - Tag tổng số lượt thi trong tuần (`txtTongLuotTuanQua`).
   - Tooltip chi tiết khi di chuột vào từng cột.
4. **Các thẻ Bài học & Đề thi nổi bật (Live DB Items):**
   - Bài học 1 (`txtTieuDeBaiHoc1`, `txtMaCodeBaiHoc1`, `txtLoaiBaiHoc1`, `txtTrinhDoBaiHoc1`, trạng thái): Nút "Chỉnh sửa" mở đúng bài học đó trong `NoiDungHocTapView(targetLessonId)`.
   - Đề thi 1 (`txtTieuDeDeThi`, `txtMaCodeDeThi`, `txtLoaiDeThi`, `txtTrinhDoDeThi`, banner ▶️): Nút "Chỉnh sửa" và Banner ▶️ mở đúng đề thi trong `DeThiLuyenTapView(targetExamId)`.
   - Bài học 2 (`txtTieuDeBaiHoc2`, `txtMaCodeBaiHoc2`, `txtLoaiBaiHoc2`): Nút "Chỉnh sửa" mở bài học đó trong `NoiDungHocTapView(targetLessonId)`.

---

### Khu vực F: Khu vực Thống kê KPI (`TongQuanView.xaml`)
- **4 chỉ số KPI lớn:**
  1. **Học viên:** Tổng số học viên thực tế (`10`), tăng trưởng và chu kỳ.
  2. **Bài học:** Tổng số bài học (`39`), tỷ lệ tăng trưởng.
  3. **Câu hỏi:** Tổng số câu hỏi trong ngân hàng (`719`), tỷ lệ tăng trưởng.
  4. **Lượt thi:** Tổng số lượt khảo sát/làm bài (`10`), tỷ lệ tăng trưởng.
- **Chỉ số phụ trợ:** Đề thi (`21`), Chờ duyệt (`5`).
- **Bộ lọc chu kỳ (`BtnLocThoiGian_Click`):**
  - Chuyển đổi linh hoạt: `Hôm nay ▼` ↔ `7 ngày qua ▼` ↔ `Tháng này ▼`.
  - Truyền `filterPeriod` vào `DashboardBUS.LaySoLieuTongQuan` để tính toán lại trend và cập nhật label hiển thị tức thì.
- **Nút "Xem phân tích học viên":**
  - Đã khắc phục liên kết: Điều hướng chính xác sang `ThongKeHocTapGiangVienView`.

---

### Khu vực G: Khu vực Vừa nộp bài (Live Feed - `TongQuanView.xaml`)
- Badge trạng thái `● Live` màu xanh nhấp nháy.
- Tự động nạp danh sách học viên nộp bài gần nhất từ CSDL.
- Tự động làm mới mỗi 30 giây qua `DispatcherTimer` chạy ngầm.
- Hiển thị Avatar Initials, Họ tên học viên, Tiêu đề bài thi, Điểm số/Trạng thái.
- Nhấp chuột vào bất kỳ mục nào sẽ điều hướng trực tiếp sang `DeThiLuyenTapView` lọc theo tên học viên để chấm bài hoặc kiểm tra.

---

### Khu vực H: Khu vực Nhóm học & Thao tác nhanh (`TongQuanView.xaml`)
1. **Phân bổ học viên theo nhóm trình độ:**
   - Nhóm A1 — Cơ bản (`4` học viên).
   - Nhóm B1 — Trung cấp (`3` học viên).
   - Nhóm B2 — Trên trung cấp (`2` học viên).
   - Nhóm C1 — Nâng cao (`1` học viên).
   - Nhấp vào từng nhóm sẽ mở `NguoiDungView` lọc đúng danh sách học viên thuộc nhóm trình độ đó.
2. **Thao tác nhanh (Quick Action Buttons):**
   - Tạo câu hỏi mới -> Mở `NganHangCauHoiView`.
   - Tạo đề thi / luyện tập -> Mở `DeThiLuyenTapView`.
   - AI sinh câu hỏi -> Mở `AISinhDeView`.
   - Xem giám sát API -> Mở `GiamSatHeThongView`.
