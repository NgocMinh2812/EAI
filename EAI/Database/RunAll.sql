/*
    SCRIPT CHẠY TOÀN BỘ CƠ SỞ DỮ LIỆU TiengAnhAI THEO ĐÚNG THỨ TỰ PHÂN HỆ
    ===================================================================
    Thứ tự thực thi:
      1. 01_cautruc_schema.sql  -> Khởi tạo Database, 41 bảng, Foreign Keys, Indexes, Views
      2. 02_danhmuc_cautruc.sql -> Nạp VaiTro, TrinhDo, NguoiDung, ChuDe, CauTrucDeThi & ChiTiet
      3. 03_dulieu_hethong.sql  -> Nạp toàn bộ dữ liệu: Bài học, Từ vựng, Ngân hàng câu hỏi, Đề thi, Bộ câu hỏi & Lượt làm bài
    ===================================================================
    Cách chạy trong SQL Server Management Studio (SSMS):
      - Bật chế độ: Query -> SQLCMD Mode
      - Nhấn F5 (Execute)
    Cách chạy bằng dòng lệnh (Command Line / Terminal):
      sqlcmd -S ".\SQLEXPRESS" -E -i RunAll.sql
*/

:r .\01_cautruc_schema.sql
:r .\02_danhmuc_cautruc.sql
:r .\03_dulieu_hethong.sql

PRINT N'===================================================================';
PRINT N'CHÚC MỪNG: TOÀN BỘ HỆ THỐNG CƠ SỞ DỮ LIỆU TiengAnhAI ĐÃ ĐƯỢC NẠP 100% THÀNH CÔNG!';
PRINT N'===================================================================';
GO
