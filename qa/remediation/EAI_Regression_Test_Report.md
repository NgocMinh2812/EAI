# EAI — BÁO CÁO KIỂM THỬ HỒI QUY (REGRESSION TEST REPORT)

> **Dự án:** EnglishAI Desktop Application (.NET 10 WPF)  
> **Bộ kiểm thử:** `EAI.Tests` & Automated Harness Runner  
> **Thời điểm chạy test:** 11/11/2026  
> **Môi trường kiểm thử:** .NET 10.0 Windows, SQL Server `SQLEXPRESS` (CSDL: `TiengAnhAI`)  
> **Kết quả tổng thể:** **15 / 15 Tests PASSED (100%)** — Không phát sinh lỗi hồi quy.

---

## BẢNG KẾT QUẢ KIỂM THỬ HỒI QUY CHI TIẾT

| Test ID | Chức năng liên quan | Thao tác kiểm thử (Test Procedure) | Kết quả mong đợi (Expected Result) | Kết quả thực tế (Actual Result) | Trạng thái |
|:---:|:---|:---|:---|:---|:---:|
| **REG-01** | Xác thực & Mã băm | Gọi `NguoiDungBUS.HashPassword("123456")` | Trả về chuỗi mã băm chuẩn 64 ký tự hex SHA-256 | Trả về `8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92` | 🟢 **PASS** |
| **REG-02** | Xác thực & An toàn đầu vào | Gọi `NguoiDungBUS.HashPassword("")` và `HashPassword(null)` | Trả về chuỗi rỗng `string.Empty`, không quăng ngoại lệ `NullReferenceException` | Trả về `string.Empty` an toàn | 🟢 **PASS** |
| **REG-03** | Bảo mật Đăng nhập | Đăng nhập tài khoản `admin` với mật khẩu giả `'admin'` (khi mật khẩu thật trong DB là mã băm khác) | Trả về `null` (Bác bỏ đăng nhập, đóng hoàn toàn cửa hậu bypass) | Trả về `null`, tài khoản không được đăng nhập | 🟢 **PASS** |
| **REG-04** | Bảo mật Đăng nhập | Đăng nhập với mật khẩu giữ chỗ `CHANGE_ME_HASH` | Trả về `null` (Không cho phép đăng nhập bằng hash giả định) | Trả về `null` | 🟢 **PASS** |
| **REG-05** | Phân quyền & RBAC | Tài khoản học viên (`HocVien`) gọi hàm `NguoiDungBUS.ResetMatKhau` đối với tài khoản khác | Trả về `Success = false` kèm thông báo không có quyền quản trị | `Success = false`, Message: "Bạn không có quyền quản trị để đặt lại mật khẩu cho tài khoản khác." | 🟢 **PASS** |
| **REG-06** | Phân quyền & RBAC | Tài khoản giảng viên (`GiangVien`) gọi hàm `NguoiDungBUS.XoaTaiKhoan` đối với tài khoản khác | Trả về `Success = false` kèm thông báo chỉ Quản trị viên mới có quyền | `Success = false`, Message: "Chỉ Quản trị viên hệ thống mới có quyền xóa tài khoản." | 🟢 **PASS** |
| **REG-07** | Động cơ Chấm điểm | Nộp bài thi có câu hỏi Tự luận (`TuLuan`) với nội dung học thuật 15 từ | Không bị nuốt vào nhánh điền khuyết; chấm đúng điểm tối đa 10.0 và đánh dấu Đạt | Điểm số = 10.0, Số câu đúng = 1, `LaDatYeuCau = true` | 🟢 **PASS** |
| **REG-08** | Động cơ Chấm điểm | Nộp bài thi câu hỏi Điền khuyết (`DienKhuyet`) có đáp án phân tách bởi ký tự `\|` (sustainable \| eco-friendly) | So khớp chính xác từ người học nhập (kể cả có dấu câu như dấu chấm ở cuối) | Số câu đúng = 1, Điểm số = 10.0m | 🟢 **PASS** |
| **REG-09** | Động cơ Chấm điểm | Nộp bài thi Trắc nghiệm (`TracNghiem`) chọn phương án đúng | Ghi nhận câu đúng và cộng điểm đầy đủ theo trọng số | Số câu đúng = 1, Điểm số = 10.0m | 🟢 **PASS** |
| **REG-10** | Kết nối CSDL | Gọi `SqlDataProvider.Instance.TestConnection()` đến CSDL `TiengAnhAI` | Kết nối thành công tới SQL Server | Kết nối mở và trả về `true` | 🟢 **PASS** |
| **REG-11** | Toàn vẹn CSDL | Gọi lệnh xóa người dùng không tồn tại `NguoiDungDAL.Instance.Delete(-9999)` | Không bị gián đoạn hay crash bởi lỗi bảng `ThongBao` thiếu | Thực thi hoàn tất không có `SqlException`, trả về `false` do không có dòng bị xóa | 🟢 **PASS** |
| **REG-12** | Khóa ngoại CSDL | Kiểm tra 5 câu hỏi nghe chuẩn hóa (IDs `9001–9005`) và 20 lựa chọn trong CSDL | 5 câu hỏi tồn tại trong bảng `CauHoi` và 20 lựa chọn tồn tại trong `LuaChonDapAn` | Số lượng câu hỏi = 5, số lượng đáp án = 20 | 🟢 **PASS** |
| **REG-13** | Thống kê Dashboard | Gọi `BaiHocDAL.Instance.LayThongKeSinhVien(-9999)` cho người dùng mới | Trả về điểm trung bình = 0, chuỗi học = 0, số bài hoàn thành = 0 (loại bỏ hoàn toàn số ảo 75%) | `DiemTrungBinh = 0.0`, `ChuoiHocLienTiep = 0`, `SoBaiDaHoanThanh = 0` | 🟢 **PASS** |
| **REG-14** | Thống kê Giảng viên | Gọi `DashboardDAL.GetKpiStats()` | Trả về số liệu thống kê quản trị thực tế không âm từ CSDL | DTO trả về hợp lệ với số liệu nguyên vẹn | 🟢 **PASS** |
| **REG-15** | Thiết bị Ghi âm | Khởi tạo đối tượng `MicrophoneRecorder` và dọn dẹp tài nguyên thông qua `Dispose()` | Khởi tạo thành công qua NAudio 64-bit, tự động dọn dẹp tệp tạm không rò rỉ bộ nhớ | Quá trình khởi tạo và Dispose hoàn thành an toàn | 🟢 **PASS** |

---

## KẾT LUẬN KIỂM THỬ HỒI QUY

Không có bất kỳ trường hợp hồi quy nào xảy ra đối với các luồng nghiệp vụ hiện có. Các chức năng xung quanh như Đăng ký người dùng mới, Làm khảo sát đầu vào, Làm bài trắc nghiệm và Luyện tập đều hoạt động trơn tru trên nền tảng .NET 10.
