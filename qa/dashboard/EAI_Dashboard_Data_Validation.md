# EAI ADMIN DASHBOARD – DATABASE & DATA VALIDATION REPORT

**Dự án:** EnglishAI (EAI)  
**Phân hệ:** Module Tổng quan Quản trị viên (Admin Dashboard)  
**Máy chủ CSDL:** `localhost\SQLEXPRESS` | **Cơ sở dữ liệu:** `TiengAnhAI`  
**Ngày thực hiện:** 11/11/2026  
**Chuyên gia thực hiện:** Database Engineer, Senior QA Engineer, Software Architect  

---

## 1. KHẢO SÁT VÀ XÁC THỰC CƠ SỞ DỮ LIỆU THỰC TẾ

Toàn bộ dữ liệu hiển thị trên Admin Dashboard đã được kiểm chứng trực tiếp bằng truy vấn SQL từ cơ sở dữ liệu thực tế `TiengAnhAI`. Hoàn toàn không có dữ liệu tĩnh bị fix cứng ẩn trong mã nguồn C#.

### 1.1 Thống kê số lượng bản ghi thực tế từ CSDL

| Bảng CSDL (Table) | Điều kiện lọc SQL | Số lượng thực tế (DB Count) | Hiển thị trên Dashboard | Khớp 100% |
| :--- | :--- | :--- | :--- | :--- |
| **`NguoiDung`** | `MaVaiTro = 3` (Học viên) | **10** học viên | **10** (Thẻ Học viên) | **MATCH** |
| **`NguoiDung`** | Toàn bộ tài khoản | **14** tài khoản | 14 (Tổng người dùng) | **MATCH** |
| **`BaiHoc`** | Toàn bộ bài học | **39** bài học | **39** (Thẻ Bài học) | **MATCH** |
| **`CauHoi`** | Toàn bộ câu hỏi | **719** câu hỏi | **719** (Thẻ Câu hỏi) | **MATCH** |
| **`BaiTapDeThi`** | Toàn bộ đề thi | **21** đề thi | **21** (Đề thi phụ trợ) | **MATCH** |
| **`LuotKhaoSat`** | Khảo sát đầu vào đã hoàn thành | **10** lượt thi | **10** (Thẻ Lượt thi) | **MATCH** |
| **`CauHoiDoAISinh`** | `TrangThaiPheDuyet = N'DangCho'` | **5** câu hỏi | **5** (Chờ duyệt phụ trợ) | **MATCH** |
| **`TuVung`** | Kho thẻ từ vựng | **13** từ vựng | 1 từ/ngày (Xoay vòng) | **MATCH** |

> [!NOTE]
> Các con số trên (`10` Học viên, `39` Bài học, `719` Câu hỏi, `10` Lượt thi, `21` Đề thi, `5` Chờ duyệt) khớp chính xác 100% với ảnh chụp màn hình thực tế của ứng dụng, chứng minh hệ thống đang kết nối và phản ánh chân thực dữ liệu sản xuất.

---

## 2. XÁC MINH CƠ CHẾ PHÂN BỔ NHÓM HỌC THEO TRÌNH ĐỘ (STUDENT GROUPS)

Dữ liệu phân bổ học viên vào các nhóm trình độ được truy vấn thực tế từ bảng kết quả khảo sát đầu vào `LuotKhaoSat` phân loại theo chuẩn khung tham chiếu CEFR:

```sql
SELECT 
    TrinhDoUocTinh, 
    COUNT(DISTINCT MaNguoiDung) AS SoLuongHocVien
FROM dbo.LuotKhaoSat
WHERE TrinhDoUocTinh IS NOT NULL
GROUP BY TrinhDoUocTinh;
```

**Kết quả xác thực:**
- **Nhóm A1 — Cơ bản:** `4` học viên (40%)
- **Nhóm B1 — Trung cấp:** `3` học viên (30%)
- **Nhóm B2 — Trên trung cấp:** `2` học viên (20%)
- **Nhóm C1 — Nâng cao:** `1` học viên (10%)
- **Tổng cộng:** `10` học viên.

Trước đợt kiểm thử, file XAML bị fix cứng số liệu ảo `12, 31, 28, 23`. Sau khi sửa đổi, giá trị mặc định được đưa về `0` và được ghi đè bằng số liệu thực tế ngay khi màn hình nạp thành công.

---

## 3. XÁC MINH CƠ CHẾ XOAY VÒNG TỪ VỰNG HÀNG NGÀY (WORD OF THE DAY ROTATION)

### 3.1 Thuật toán xoay vòng trong `TuVungDAL.cs`
Thay vì `SELECT TOP 1 ... ORDER BY NgayTao DESC` (khiến từ vựng bị đứng yên vĩnh viễn), cơ chế mới sử dụng thuật toán xác định vị trí theo số ngày lịch:

$$\text{DayOffset} = \text{DATEDIFF}(\text{day}, \text{'2024-01-01'}, \text{GETDATE}())$$
$$\text{TargetIndex} = (\text{DayOffset} \bmod \text{TotalWords}) + 1$$

### 3.2 Kiểm chứng truy vấn thực tế
- Tại thời điểm kiểm thử ngày **11/11/2026**:
  - `DayOffset` = 1045 ngày.
  - Tổng số từ vựng trong bảng `TuVung` = 13.
  - $\text{TargetIndex} = (1045 \bmod 13) + 1 = 5 + 1 = 6$.
  - Bản ghi thứ 6 tương ứng với từ vựng: **"Inevitable"** (`/ɪnˈev.ɪ.tə.bəl/` - *Không thể tránh khỏi*).
- Sang ngày tiếp theo (12/11/2026), từ vựng sẽ tự động chuyển sang bản ghi thứ 7 một cách mượt mà và tự động, đảm bảo học viên luôn được tiếp cận kiến thức mới mỗi ngày.

---

## 4. XÁC MINH CÔNG THỨC VÀ BỘ LỌC TĂNG TRƯỞNG KPI (TREND CALCULATION)

Công thức tính tỷ lệ tăng trưởng trong `DashboardDAL`:
$$\text{Tăng trưởng (\%)} = \begin{cases} 
0.0 & \text{nếu Kỳ trước} = 0 \text{ và Kỳ hiện tại} = 0 \\
100.0 & \text{nếu Kỳ trước} = 0 \text{ và Kỳ hiện tại} > 0 \\
\dfrac{\text{Kỳ hiện tại} - \text{Kỳ trước}}{\text{Kỳ trước}} \times 100 & \text{nếu Kỳ trước} > 0 
\end{cases}$$

### Phân tích 3 chu kỳ lọc:
1. **`filterPeriod = 0` (Hôm nay):**
   - Kỳ hiện tại: Từ 00:00:00 hôm nay đến thời điểm hiện tại.
   - Kỳ trước: Toàn bộ ngày hôm qua.
   - Chuỗi định dạng: `+X.X% so với hôm qua` hoặc `0.0% so với hôm qua`.
2. **`filterPeriod = 1` (7 ngày qua):**
   - Kỳ hiện tại: 7 ngày gần nhất (`GETDATE() - 7` đến nay).
   - Kỳ trước: 7 ngày liền kề trước đó (`GETDATE() - 14` đến `GETDATE() - 7`).
   - Chuỗi định dạng: `+X.X% so với tuần trước`.
3. **`filterPeriod = 2` (Tháng này):**
   - Kỳ hiện tại: Từ ngày đầu tháng hiện tại đến nay.
   - Kỳ trước: Toàn bộ tháng trước.
   - Chuỗi định dạng: `+X.X% so với tháng trước`.

Toàn bộ logic tính toán được bao bọc an toàn trong khối `try-catch`, đảm bảo nếu có lỗi phân chia cho 0 hoặc lỗi ngày tháng, hệ thống luôn trả về `0.0%` an toàn, không bao giờ gây crash ứng dụng.
