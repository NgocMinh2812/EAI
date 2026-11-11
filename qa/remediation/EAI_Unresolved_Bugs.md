# EAI — DANH SÁCH LỖI CHƯA XỬ LÝ HOẶC GHI NHẬN TƯƠNG LAI (UNRESOLVED BUGS REPORT)

> **Dự án:** EnglishAI Desktop Application (.NET 10 WPF)  
> **Thời điểm cập nhật:** 11/11/2026  
> **Tình trạng hiện tại:** **0 lỗi tồn đọng thuộc phạm vi kiểm toán** (17/17 lỗi phát hiện đã được khắc phục hoàn toàn).

---

## 1. BẢNG TỔNG HỢP TRẠNG THÁI TỒN ĐỌNG

| Bug ID | Mô tả | Mức độ | Lý do chưa xử lý | Hướng xử lý tiếp theo |
|---|---|:---:|---|---|
| *Không có* | *Toàn bộ 17 lỗi P0–P3 trong Báo cáo Defect đã được khắc phục và kiểm chứng thành công.* | - | - | - |

---

## 2. GHI NHẬN CẢI TIẾN DÀI HẠN (TECHNICAL DEBT & FUTURE ENHANCEMENTS)

Dưới đây là các hạng mục nợ kỹ thuật không gây lỗi trực tiếp cho người dùng hiện tại nhưng được khuyến nghị xử lý trong các đợt phát hành tiếp theo:

### 2.1. Nâng cấp Thư viện Đồ thị `LiveCharts.Wpf` lên `LiveChartsCore.SkiaSharpView.WPF`
- **Hiện trạng:** Thư viện đồ thị `LiveCharts.Wpf 0.9.7` hiện tại được đóng gói từ thời .NET Framework 4.6.1–4.8.1. Mặc dù vẫn chạy tốt trên .NET 10 WPF thông qua cơ chế compat shim (NU1701 warning), nó không có các tối ưu phần cứng của SkiaSharp.
- **Kế hoạch tương lai:** Chuyển đổi sang `LiveChartsCore.SkiaSharpView.WPF` (bản Native .NET 8/9/10) khi có kế hoạch tái thiết kế giao diện biểu đồ.

### 2.2. Xây dựng Phân hệ Thông báo Đầy đủ (In-app Notification System)
- **Hiện trạng:** Nút chuông thông báo hiện tại đã được gắn sự kiện phản hồi thông tin thân thiện cho người dùng (khắc phục trạng thái nút chết `BUG-UI-02`). Bảng `ThongBao` hiện chưa có trong schema CSDL.
- **Kế hoạch tương lai:** Nếu nghiệp vụ đào tạo có yêu cầu gửi thông báo từ Giảng viên đến Học viên (như thông báo bài tập mới, lịch bảo trì), sẽ bổ sung bảng `ThongBao` vào database và tạo trang `ThongBaoView`.

### 2.3. Mở rộng Tỷ lệ Bao phủ Kiểm thử Tự động (Code Coverage Expansion)
- **Hiện trạng:** Đã thiết lập dự án `EAI.Tests` với 15 bài kiểm thử trọng yếu bảo vệ toàn bộ các module: Xác thực, Chấm thi, CSDL, Phân quyền, Metrics và Audio.
- **Kế hoạch tương lai:** Tiếp tục viết thêm các Unit Tests cho module `TuVungBUS`, `KhaoSatBUS`, và tích hợp quy trình CI/CD tự động chạy test khi tạo Pull Request trên GitHub Actions.
