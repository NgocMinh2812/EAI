/*
    SCRIPT CHẠY TOÀN BỘ CƠ SỞ DỮ LIỆU TiengAnhAI THEO ĐÚNG THỨ TỰ PHÂN HỆ
    ===================================================================
    Thứ tự thực thi:
      1. 01_cautruc_schema.sql  -> Khởi tạo Database, 39 bảng, Foreign Keys, Indexes, Views
      2. 02_danhmuc_cautruc.sql -> Nạp VaiTro, TrinhDo, NguoiDung, ChuDe, CauTrucDeThi & ChiTiet
      3. 03_noidunghoctap.sql   -> Nạp 36 Bài học, Từ vựng, Flashcards, Audio Transcripts bài nghe
      4. 04_nganhangcauhoi.sql  -> Nạp 704 Câu hỏi trắc nghiệm & Đáp án (416 bài học + 288 hệ thống)
      5. 05_dethi_luyentap.sql  -> Nạp 21 Đề thi, CauHoiBaiTap, 36 Bộ câu hỏi theo Bài học
    ===================================================================
    Cách chạy trong SQL Server Management Studio (SSMS):
      - Bật chế độ: Query -> SQLCMD Mode
      - Nhấn F5 (Execute)
    Cách chạy bằng dòng lệnh (Command Line / Terminal):
      sqlcmd -S ".\SQLEXPRESS" -E -i RunAll.sql
*/

:r .\01_cautruc_schema.sql
:r .\02_danhmuc_cautruc.sql
:r .\03_noidunghoctap.sql
:r .\04_nganhangcauhoi.sql
:r .\05_dethi_luyentap.sql

PRINT N'===================================================================';
PRINT N'CHÚC MỪNG: TOÀN BỘ HỆ THỐNG CƠ SỞ DỮ LIỆU TiengAnhAI ĐÃ ĐƯỢC NẠP 100% THÀNH CÔNG!';
PRINT N'===================================================================';
GO
