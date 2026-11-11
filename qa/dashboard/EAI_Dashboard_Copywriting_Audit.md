# EAI ADMIN DASHBOARD – UX WRITING & COPYWRITING AUDIT

**Dự án:** EnglishAI (EAI)  
**Phân hệ:** Module Tổng quan Quản trị viên (Admin Dashboard)  
**Ngày lập:** 11/11/2026  
**Chuyên gia thực hiện:** UX Writer, UI/UX Designer, Senior QA Engineer  

---

## 1. NGUYÊN TẮC BIÊN TẬP NỘI DUNG (PRODUCT VOICE & TONE GUIDELINES)

1. **Chuẩn mực Giáo dục & Học thuật:** Sử dụng ngôn từ trong sáng, chuẩn xác theo thuật ngữ giảng dạy tiếng Anh (CEFR, IELTS, TOEIC, Flashcard, IPA).
2. **Loại bỏ hoàn toàn văn phong máy móc / AI dịch thô:** Tránh các từ ngữ hoa mỹ, sáo rỗng như "Cây tri thức tăng trưởng", "Trải nghiệm kỳ diệu", "Thuật toán tối thượng". Thay vào đó dùng các câu rõ ràng, trực diện, mang tính hành động cao.
3. **Thống nhất cấu trúc và thuật ngữ:**
   - Dùng "Quản trị viên" thay cho "Admin" trong các thông báo hệ thống chính thức.
   - Dùng "Học viên" thay cho các từ lộn xộn như "User", "Sinh viên", "Khách".
   - Dùng "Đề thi & Luyện tập" thống nhất thay vì "Bài tập đề thi".
4. **Thông báo và chỉ dẫn rõ ràng (Actionable & Transparent):** Khi có lỗi hoặc cảnh báo thoát, giải thích rõ nguyên nhân và hành động khắc phục, không để người dùng hoang mang.

---

## 2. BẢNG ĐỐI CHIẾU VÀ HIỆU CHỈNH NỘI DUNG GIAO DIỆN (AUDIT LOG)

| Vị trí / Thành phần | Nội dung cũ (Trước audit) | Nội dung mới (Sau hiệu chỉnh) | Lý do hiệu chỉnh |
| :--- | :--- | :--- | :--- |
| **Thẻ Từ vựng hôm nay (Banner)** | `🌱 Cây tri thức tăng trưởng • Ôn tập Flashcard` | `📚 Thẻ ghi nhớ thông minh • Bấm để ôn tập ngay` | Loại bỏ văn phong AI hoa mỹ vô nghĩa, thay bằng lợi ích sản phẩm rõ ràng và lời kêu gọi hành động cụ thể. |
| **Thẻ Từ vựng hôm nay (Ví dụ)** | *(Bỏ trống hoặc chỉ có câu đơn không ngoặc)* | `"Learning English requires perseverance and daily practice."` | Bổ sung câu trích dẫn mẫu có ý nghĩa rèn luyện tính kiên trì, hiển thị chuẩn trong dấu ngoặc kép. |
| **Thẻ Từ vựng hôm nay (Loại từ)** | *(Viết tắt hoặc tiếng Anh thô: `noun`)* | `danh từ` / `tính từ` / `động từ` | Việt hóa chuẩn xác và trực quan cho giảng viên và học viên. |
| **Topbar (Search Placeholder)** | `Tìm kiếm...` | `Tìm kiếm người dùng, bài học, câu hỏi...` | Gợi ý rõ ràng phạm vi tìm kiếm có thể truy vấn. |
| **Chuông thông báo (Tooltip)** | Không có hoặc nội dung sơ sài | `Thông báo hệ thống (3 mới)` kèm chi tiết 3 đầu việc | Cung cấp thông tin tóm tắt nhanh trước khi người dùng click vào. |
| **Chuông thông báo (Popup Header)** | Hộp thoại thông báo "Bạn không có thông báo mới" | `Thông báo hệ thống (3)` kèm nút `Đánh dấu đã đọc` | Thống nhất giữa số badge đỏ và nội dung popup, tạo tương tác đánh dấu đã đọc. |
| **Thông báo 1** | *(Dữ liệu mockup tiếng Anh lộn xộn)* | `2 bài nộp Đề thi IELTS từ học viên • 10 phút trước • Cần kiểm tra` | Rõ việc cần làm, đúng ngữ cảnh giảng viên cần chấm bài thi. |
| **Thông báo 2** | *(Không có)* | `AI vừa hoàn tất bóc tách câu hỏi • 35 phút trước • Tự động` | Thông báo trạng thái xử lý ngầm của hệ thống AI sinh đề. |
| **Thông báo 3** | *(Không có)* | `Cập nhật hệ thống EAI 2026.1 • 2 giờ trước • Hệ thống` | Thông báo ổn định hạ tầng cho quản trị viên. |
| **Chỉ số KPI (Chu kỳ)** | Nhãn tĩnh hoặc không rõ so sánh với gì | `Hôm nay ▼` (so với hôm qua), `7 ngày qua ▼` (so với tuần trước), `Tháng này ▼` (so với tháng trước) | Thể hiện rõ bản chất tỷ lệ tăng trưởng so với kỳ trước. |
| **Nút Phân tích học viên** | `Xem phân tích học viên` | `Xem phân tích học viên` (Giữ nguyên nhưng sửa đích đến sang `ThongKeHocTapGiangVienView`) | Khớp chức năng mong muốn, văn phong trang trọng. |
| **Live Feed nộp bài** | `Live` | `● Live` kèm badge xanh nhạt bo góc | Biểu tượng chuẩn nhận diện luồng hoạt động trực tiếp. |
| **Thẻ Tài nguyên Học liệu** | Danh sách chữ không có icon hoặc nhãn | `🔖 Ngữ pháp tiếng Anh cơ bản — A1 đến B1`, `Bộ đề IELTS 2024 — Academic & General`, `1000 từ vựng TOEIC thông dụng nhất`, `Phát âm IPA — Bảng ký hiệu và ví dụ âm` | Thống nhất dấu gạch nối dài (`—`), viết hoa chuẩn tên chứng chỉ quốc tế. |
| **Hộp thoại xác nhận rời trang** | "Dữ liệu chưa lưu" | `Bạn đang soạn bài học mới và có nội dung chưa lưu. Nếu chuyển sang chức năng khác, nội dung bài học đang tạo sẽ bị mất. Bạn có chắc chắn muốn rời khỏi không?` | Lời cảnh báo chân thành, 2 nút lựa chọn rõ ràng: `Rời khỏi` và `Ở lại chỉnh sửa`. |
| **Hộp thoại xác nhận đăng xuất** | "Bạn có muốn đăng xuất?" | `Bạn có chắc chắn muốn đăng xuất khỏi phiên làm việc hiện tại?` | 2 nút hành động rõ nghĩa: `Đăng xuất` và `Ở lại`. |

---

## 3. TIÊU CHUẨN THÔNG BÁO LỖI VÀ TRẠNG THÁI RỖNG (EMPTY STATES)

- **Khi không tìm thấy kết quả tìm kiếm:**
  - Tiêu đề: `Không tìm thấy kết quả nào cho "<từ khóa>"`
  - Gợi ý: `Thử tìm kiếm với tên người dùng, email, tiêu đề bài học, câu hỏi hoặc chức năng...`
- **Khi danh sách nộp bài rỗng:**
  - `Hiện chưa có lượt nộp bài nào mới. Hệ thống sẽ tự động cập nhật ngay khi có học viên hoàn thành bài thi.`
- **Khi tải dữ liệu thất bại:**
  - Không hiện thông báo lỗi kỹ thuật mã nguồn (exception stack trace) lên giao diện người dùng. Hiển thị thông báo thân thiện:
  - `Không thể kết nối đến máy chủ dữ liệu. Vui lòng kiểm tra lại dịch vụ cơ sở dữ liệu SQL Server.`
