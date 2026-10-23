# Generate File 1: TestCases_Sprint1_Sprint2_Minh_HocVien.docx
# Tester: Minh (Phân hệ Học viên / Student)

$outDir = "d:\TDC\HK5\CDPTUD\EAI"
$htmlPath = "$outDir\TestCases_Minh_HocVien.html"
$docxPath = "$outDir\TestCases_Sprint1_Sprint2_Minh_HocVien.docx"

Write-Output "Building HTML content for File 1 (Minh - Hoc Vien)..."

# We will write the generator script in PowerShell that outputs clean UTF8 HTML
$scriptContent = @'
$outHtml = "d:\TDC\HK5\CDPTUD\EAI\TestCases_Minh_HocVien.html"
$sb = New-Object System.Text.StringBuilder

function Append-Line($str) {
    [void]$sb.AppendLine($str)
}

function Add-Header {
    Append-Line '<!DOCTYPE html>'
    Append-Line '<html lang="vi">'
    Append-Line '<head>'
    Append-Line '<meta charset="utf-8">'
    Append-Line '<title>Test Cases Sprint 1 & 2 - Minh (Học Viên) - EnglishAI</title>'
    Append-Line '<style>'
    Append-Line '    @page { size: A4 landscape; margin: 15mm 15mm 15mm 15mm; }'
    Append-Line '    body { font-family: "Segoe UI", Arial, sans-serif; font-size: 10pt; color: #1E293B; line-height: 1.45; margin: 0; padding: 15px; }'
    Append-Line '    .cover-box { border: 2px solid #1E3A8A; background: #F8FAFC; padding: 25px 30px; margin-bottom: 25px; border-radius: 6px; }'
    Append-Line '    .cover-title { font-size: 22pt; font-weight: bold; color: #1E3A8A; margin-bottom: 6px; text-transform: uppercase; }'
    Append-Line '    .cover-sub { font-size: 13pt; font-weight: 600; color: #2563EB; margin-bottom: 15px; }'
    Append-Line '    .meta-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 10pt; }'
    Append-Line '    .meta-table td { padding: 4px 8px; border: none; }'
    Append-Line '    .meta-table td.lbl { font-weight: bold; color: #334155; width: 180px; }'
    Append-Line '    h1 { font-size: 16pt; color: #1E3A8A; border-bottom: 2px solid #2563EB; padding-bottom: 5px; margin-top: 25px; margin-bottom: 12px; }'
    Append-Line '    h2 { font-size: 13pt; color: #1E40AF; border-left: 4px solid #3B82F6; padding-left: 8px; margin-top: 18px; margin-bottom: 10px; }'
    Append-Line '    h3 { font-size: 11pt; color: #2563EB; margin-top: 14px; margin-bottom: 8px; }'
    Append-Line '    p, li { font-size: 10pt; text-align: justify; }'
    Append-Line '    table.tbl-summary, table.tbl-matrix, table.tbl-report { width: 100%; border-collapse: collapse; margin: 10px 0 16px 0; font-size: 9pt; }'
    Append-Line '    table.tbl-summary th, table.tbl-matrix th, table.tbl-report th { background: #1E40AF; color: white; padding: 6px 8px; border: 1px solid #94A3B8; text-align: left; font-weight: 600; }'
    Append-Line '    table.tbl-summary td, table.tbl-matrix td, table.tbl-report td { padding: 5px 8px; border: 1px solid #CBD5E1; vertical-align: top; }'
    Append-Line '    table.tbl-summary tr:nth-child(even), table.tbl-matrix tr:nth-child(even), table.tbl-report tr:nth-child(even) { background: #F8FAFC; }'
    Append-Line '    table.tc-card { width: 100%; border-collapse: collapse; margin: 8px 0 16px 0; font-size: 9pt; border: 1.5px solid #1E40AF; page-break-inside: avoid; }'
    Append-Line '    table.tc-card td { padding: 5px 8px; border: 1px solid #CBD5E1; vertical-align: top; }'
    Append-Line '    table.tc-card td.tc-lbl { background: #F1F5F9; font-weight: bold; color: #1E293B; width: 15%; }'
    Append-Line '    table.tc-card td.tc-val { width: 35%; }'
    Append-Line '    table.tc-card tr.tc-exec { background: #FCFDFE; }'
    Append-Line '    .badge { display: inline-block; padding: 2px 7px; border-radius: 3px; font-weight: bold; font-size: 8.5pt; text-transform: uppercase; }'
    Append-Line '    .badge-crit { background: #FEE2E2; color: #991B1B; border: 1px solid #F87171; }'
    Append-Line '    .badge-high { background: #FFEDD5; color: #9A3412; border: 1px solid #FB923C; }'
    Append-Line '    .badge-med { background: #FEF3C7; color: #92400E; border: 1px solid #FCD34D; }'
    Append-Line '    .badge-low { background: #F1F5F9; color: #475569; border: 1px solid #CBD5E1; }'
    Append-Line '    .badge-pos { background: #DCFCE7; color: #166534; border: 1px solid #86EFAC; }'
    Append-Line '    .badge-neg { background: #FEE2E2; color: #991B1B; border: 1px solid #FCA5A5; }'
    Append-Line '    .badge-bnd { background: #F3E8FF; color: #6B21A8; border: 1px solid #D8B4FE; }'
    Append-Line '    .badge-val { background: #E0E7FF; color: #3730A3; border: 1px solid #A5B4FC; }'
    Append-Line '    .badge-int { background: #CFFAFE; color: #155E75; border: 1px solid #67E8F9; }'
    Append-Line '    .badge-sec { background: #FFE4E6; color: #9F1239; border: 1px solid #FDA4AF; }'
    Append-Line '    .badge-notrun { background: #F1F5F9; color: #64748B; border: 1px solid #CBD5E1; font-style: italic; }'
    Append-Line '    .page-break { page-break-before: always; }'
    Append-Line '</style>'
    Append-Line '</head>'
    Append-Line '<body>'
}

function Add-CoverPage {
    Append-Line '<div class="cover-box">'
    Append-Line '    <div class="cover-title">DỰ ÁN ENGLISHAI (EAI) - HỆ THỐNG KHẢO THÍ & HỌC TIẾNG ANH TÍCH HỢP AI</div>'
    Append-Line '    <div class="cover-sub">TÀI LIỆU THIẾT KẾ BỘ KỊCH BẢN KIỂM THỬ PHẦN MỀM (TEST CASE SPECIFICATION)</div>'
    Append-Line '    <div style="height: 1px; background: #CBD5E1; margin: 15px 0;"></div>'
    Append-Line '    <table class="meta-table">'
    Append-Line '        <tr><td class="lbl">Phân hệ kiểm thử:</td><td><b>PHÂN HỆ HỌC VIÊN / SINH VIÊN (STUDENT SUBSYSTEM)</b></td></tr>'
    Append-Line '        <tr><td class="lbl">Phạm vi thực hiện:</td><td><b>Sprint 1 (Tuần 1 & 2) & Sprint 2 (Tuần 3 & 4)</b></td></tr>'
    Append-Line '        <tr><td class="lbl">Người phụ trách (Owner):</td><td><b>Minh</b> (QA / Test Engineer)</td></tr>'
    Append-Line '        <tr><td class="lbl">Kiến trúc phần mềm:</td><td>C# WPF Desktop Application, SQL Server, 3-Tier Architecture (GUI - BUS - DAL - DTO)</td></tr>'
    Append-Line '        <tr><td class="lbl">Phiên bản tài liệu:</td><td><b>v1.0 (Official Release for Execution)</b></td></tr>'
    Append-Line '        <tr><td class="lbl">Ngày ban hành:</td><td>23/10/2026</td></tr>'
    Append-Line '        <tr><td class="lbl">Trạng thái kiểm thử ban đầu:</td><td><b>Not Run (Bộ tài liệu thiết kế sẵn sàng cho thực thi thủ công)</b></td></tr>'
    Append-Line '    </table>'
    Append-Line '</div>'
}

# Sections 1, 2, 3
function Add-IntroSections {
    Append-Line '<h1>MỤC 1. MỤC ĐÍCH VÀ PHẠM VI KIỂM THỬ</h1>'
    Append-Line '<p><b>1.1 Mục đích:</b> Tài liệu này cung cấp bộ kịch bản kiểm thử (Test Cases) chi tiết, chuẩn hóa và độc lập dành cho phân hệ <b>Sinh viên / Học viên</b> của ứng dụng EnglishAI, do <b>Minh</b> phụ trách. Tài liệu được thiết kế làm công cụ thực hiện kiểm thử thủ công (Manual Testing), ghi nhận kết quả thực tế (Actual Result), trạng thái Pass/Fail/Blocked, đính kèm bằng chứng (Evidence) và truy vết lỗi (Defect Tracking).</p>'
    Append-Line '<p><b>1.2 Phạm vi Sprint 1 (Đăng ký, Đăng nhập & Khảo sát Năng lực Onboarding):</b>'
    Append-Line '<ul>'
    Append-Line '    <li><b>TASK-101:</b> Khởi tạo CSDL TiengAnhAI, tính toàn vẹn khóa ngoại và bộ dữ liệu mẫu ban đầu.</li>'
    Append-Line '    <li><b>TASK-102:</b> Cấu hình Solution và kiểm tra kiến trúc 3 lớp (GUI - BUS - DAL), xử lý kết nối DB qua SqlDataProvider.</li>'
    Append-Line '    <li><b>TASK-103:</b> Tầng DAL & DTO phía học viên (NguoiDungDAL, HoSoHocTapDAL, CauHoiDAL và các model DTO).</li>'
    Append-Line '    <li><b>TASK-104:</b> Giao diện Đăng nhập / Đăng ký (DangNhapWindow), validation form, chuyển tab, kiểm tra tài khoản trùng.</li>'
    Append-Line '    <li><b>TASK-105:</b> Nghiệp vụ Xác thực, mã hóa mật khẩu, kiểm tra tài khoản khóa, khởi tạo và giải phóng phiên SessionHelper.</li>'
    Append-Line '    <li><b>TASK-106:</b> Khung giao diện chính sinh viên (MainWindow), thanh Sidebar menu, avatar viết tắt, điều hướng mượt mà.</li>'
    Append-Line '    <li><b>TASK-107:</b> Cơ chế kiểm soát lần đầu đăng nhập (cờ LaLanDauDangNhap), tự động ẩn Sidebar và dẫn vào khảo sát đầu vào.</li>'
    Append-Line '    <li><b>TASK-108:</b> Giao diện Khảo sát Năng lực (DanhGiaNangLucView), bộ câu hỏi 15-20 câu, chọn/sửa đáp án, nộp bài.</li>'
    Append-Line '    <li><b>TASK-109:</b> Thuật toán chấm điểm bài khảo sát, xếp trình độ CEFR (A1-C1) và khởi tạo hồ sơ học tập HoSoHocTap.</li>'
    Append-Line '    <li><b>TASK-110:</b> Kiểm thử tích hợp chu trình Onboarding Sprint 1 xuyên suốt.</li>'
    Append-Line '</ul></p>'
    Append-Line '<p><b>1.3 Phạm vi Sprint 2 (Học tập theo Lộ trình, Luyện tập & Động cơ Thi thử Chuẩn hóa):</b>'
    Append-Line '<ul>'
    Append-Line '    <li><b>TASK-201:</b> Giao diện Trang chủ Sinh viên (TrangChuView), lời chào cá nhân hóa, 3 card KPI, lối tắt và bài học gần đây.</li>'
    Append-Line '    <li><b>TASK-202:</b> Phân hệ Học tập theo Lộ trình (HocTapView), cây danh mục theo kỹ năng/chủ đề, trình đọc lý thuyết ngữ pháp/từ vựng.</li>'
    Append-Line '    <li><b>TASK-203:</b> Trình phát audio bài nghe (MediaPlayer), phát/tạm dừng, tua âm thanh, 4 nấc tốc độ (0.5x - 1.25x) và cơ chế Dual Engine Fallback SpeechSynthesizer.</li>'
    Append-Line '    <li><b>TASK-204:</b> Ghi nhận thời gian và tiến độ học tập, cộng dồn SoPhutHocTichLuy, cập nhật bảng TienDoBaiHoc, chống gian lận thoát nhanh.</li>'
    Append-Line '    <li><b>TASK-205:</b> Phân hệ Luyện tập theo Kỹ năng (LuyenTapView), làm câu hỏi trắc nghiệm kèm giải thích ngay, học từ vựng Flashcard IPA, Dictation chính tả.</li>'
    Append-Line '    <li><b>TASK-206:</b> Động cơ Thi thử Chuẩn hóa (ThiThuView), đồng hồ đếm ngược Countdown Timer, ma trận điều hướng câu hỏi, cắm cờ câu hỏi khó, tự động nộp khi hết giờ.</li>'
    Append-Line '    <li><b>TASK-207:</b> Chấm điểm đề thi tự động thang điểm 10.0, lưu bảng LuotLamBai, ChiTietTraLoi và lưu vết lịch sử khảo thí.</li>'
    Append-Line '    <li><b>TASK-208:</b> Màn hình Xem lại Chi tiết Bài thi (Review Mode), đối chiếu đáp án chọn với đáp án đúng, hiển thị giải thích chi tiết.</li>'
    Append-Line '    <li><b>TASK-209:</b> Kiểm thử tích hợp động cơ khảo thí Sprint 2 xuyên suốt.</li>'
    Append-Line '</ul></p>'
    Append-Line '<p><b>1.4 Ngoài phạm vi (Out of Scope):</b> Các tính năng thuộc Sprint 3 (Gia sư trợ lý AI Gemini, Chấm bài viết AI, Biểu đồ LiveCharts TienDoHocView) và toàn bộ các tính năng thuộc phân hệ Giảng viên/Admin (do Êm phụ trách).</p>'

    Append-Line '<h1>MỤC 2. CĂN CỨ KIỂM THỬ</h1>'
    Append-Line '<p>Bộ kịch bản kiểm thử được xây dựng căn cứ trên các tài liệu và hiện vật kỹ thuật sau:'
    Append-Line '<ul>'
    Append-Line '    <li>File kế hoạch triển khai dự án <code>Kehoachtrienkhai.xlsx</code>, sheet <b>Kế hoạch 3 Sprint - Minh</b> (Mã công việc từ TASK-101 đến TASK-110 và TASK-201 đến TASK-209).</li>'
    Append-Line '    <li>Tài liệu kiểm thử tham chiếu <code>EnglishEAI_BoKiemThu_TestCase.xlsx</code> (Sheet <i>Sprint 1 - Học Liệu & Auth</i> và <i>Sprint 2 - Khảo Thí & Học Tập</i>).</li>'
    Append-Line '    <li>Kịch bản CSDL chuẩn hóa <code>EAI.sql</code> và tập dữ liệu khởi tạo <code>EAI_seed.sql</code> tại thư mục <code>Database/</code>.</li>'
    Append-Line '    <li>Mã nguồn thực tế tầng GUI (DangNhapWindow, MainWindow, DanhGiaNangLucView, TrangChuView, HocTapView, LuyenTapView, ThiThuView), tầng BUS (NguoiDungBUS, SessionHelper, KhaoSatBUS, BaiHocBUS, TuVungBUS, BaiTapDeThiBUS) và tầng DAL.</li>'
    Append-Line '    <li>Các ảnh chụp giao diện thực tế của ứng dụng (MainWindow, HocTapView trình đọc và trình phát nghe, ThiThuView phòng thi và ma trận điều hướng).</li>'
    Append-Line '</ul></p>'

    Append-Line '<h1>MỤC 3. ĐIỀU KIỆN MÔI TRƯỜNG KIỂM THỬ</h1>'
    Append-Line '<table class="tbl-summary">'
    Append-Line '    <tr><th style="width:25%">Thành phần</th><th style="width:40%">Cấu hình chuẩn</th><th style="width:35%">Ghi chú / Trạng thái</th></tr>'
    Append-Line '    <tr><td>Hệ điều hành</td><td>Microsoft Windows 10 / Windows 11 (64-bit)</td><td>Môi trường chuẩn chạy WPF Desktop</td></tr>'
    Append-Line '    <tr><td>Nền tảng / Runtime</td><td>.NET 10.0 / .NET Desktop Runtime (x64)</td><td>Đã cài đặt SDK 10.0.402</td></tr>'
    Append-Line '    <tr><td>Hệ quản trị CSDL</td><td>Microsoft SQL Server 2019 / 2022 Express hoặc Enterprise</td><td>Tên CSDL: <code>TiengAnhAI</code></td></tr>'
    Append-Line '    <tr><td>Chuỗi kết nối (Connection)</td><td><code>Server=.;Database=TiengAnhAI;Integrated Security=True;TrustServerCertificate=True</code></td><td>Cấu hình trong App.config / SqlDataProvider</td></tr>'
    Append-Line '    <tr><td>Tài khoản Học viên mới</td><td>Email: <code>hocvien_moi@englisheai.edu.vn</code> | Pass: <code>123456</code></td><td>Cờ LaLanDauDangNhap = 1 (Phục vụ Onboarding)</td></tr>'
    Append-Line '    <tr><td>Tài khoản Học viên chuẩn</td><td>Email: <code>sinhvien@englisheai.edu.vn</code> | Pass: <code>123456</code></td><td>Trình độ A2, LaLanDauDangNhap = 0</td></tr>'
    Append-Line '    <tr><td>Tài khoản bị vô hiệu</td><td>Email: <code>locked_user@englisheai.edu.vn</code> | Pass: <code>123456</code></td><td>TrangThaiHoatDong = 0 (Kiểm tra chặn đăng nhập)</td></tr>'
    Append-Line '    <tr><td>Tệp đa phương tiện</td><td>File âm thanh .mp3 mẫu tại <code>Resources/Audio/</code></td><td>Kiểm tra Audio Player</td></tr>'
    Append-Line '</table>'
}

Add-Header
Add-CoverPage
Add-IntroSections

$utf8NoBom = New-Object System.Text.UTF8Encoding $false
[System.IO.File]::WriteAllText($outHtml, $sb.ToString(), $utf8NoBom)
Write-Output "Header & Intro written successfully!"
'@

$scriptContent | Out-File -FilePath "$outDir\build_file1_part1.ps1" -Encoding UTF8
powershell -ExecutionPolicy Bypass -File "$outDir\build_file1_part1.ps1"
