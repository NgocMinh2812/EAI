-- ===================================================================
-- SCRIPT MIGRATION SPRINT 3: CẤU HÌNH KHẢO SÁT & HOÀN THIỆN SCHEMA
-- Dự án: EnglishAI (EAI) Desktop Application
-- ===================================================================

USE TiengAnhAI;
GO

-- 1. TẠO BẢNG CẤU HÌNH QUY ĐỔI KHẢO SÁT (NẾU CHƯA TỒN TẠI)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'CauHinhQuyDoiKhaoSat')
BEGIN
    CREATE TABLE CauHinhQuyDoiKhaoSat (
        MaCauHinh INT IDENTITY(1,1) PRIMARY KEY,
        MaCapDo NVARCHAR(10) NOT NULL,            -- A1, A2, B1, B2, C1
        TenHienThi NVARCHAR(100) NOT NULL,        -- A1 (Mất gốc), A2 (Sơ cấp), ...
        DiemPhanTramMin INT NOT NULL,             -- 0, 31, 51, 71, 86
        DiemPhanTramMax INT NOT NULL,             -- 30, 50, 70, 85, 100
        QuyDoiIelts NVARCHAR(50) NOT NULL,        -- IELTS < 3.0, ...
        QuyDoiToeic NVARCHAR(50) NOT NULL,        -- TOEIC 100 - 250, ...
        LoTrinhDeXuat NVARCHAR(500) NOT NULL,
        MauSac NVARCHAR(20) NULL,                 -- Mã màu HEX giao diện (#DC2626, ...)
        ThuTuSapXep INT NOT NULL DEFAULT 1
    );
    PRINT N'Đã tạo bảng CauHinhQuyDoiKhaoSat thành công.';
END
GO

-- 2. NẠP DỮ LIỆU MA TRẬN MẶC ĐỊNH CHO BẢNG CẤU HÌNH (NẾU BẢNG ĐANG TRỐNG)
IF NOT EXISTS (SELECT 1 FROM CauHinhQuyDoiKhaoSat)
BEGIN
    INSERT INTO CauHinhQuyDoiKhaoSat 
        (MaCapDo, TenHienThi, DiemPhanTramMin, DiemPhanTramMax, QuyDoiIelts, QuyDoiToeic, LoTrinhDeXuat, MauSac, ThuTuSapXep)
    VALUES
        (N'A1', N'A1 (Mất gốc)', 0, 30, N'IELTS < 3.0', N'TOEIC 100 - 250', N'Ngữ pháp nền tảng + 500 từ vựng cốt lõi', N'#DC2626', 1),
        (N'A2', N'A2 (Sơ cấp)', 31, 50, N'IELTS 3.5 - 4.5', N'TOEIC 255 - 450', N'Luyện phản xạ câu & Nghe hiểu đơn giản', N'#EA580C', 2),
        (N'B1', N'B1 (Trung cấp)', 51, 70, N'IELTS 5.0 - 6.0', N'TOEIC 455 - 650', N'Từ vựng học thuật B1 & Cấu trúc câu ghép', N'#2563EB', 3),
        (N'B2', N'B2 (Trên trung cấp)', 71, 85, N'IELTS 6.5 - 7.0', N'TOEIC 655 - 800', N'Viết luận Task 2 & Skimming/Scanning đọc hiểu', N'#15803D', 4),
        (N'C1', N'C1 (Nâng cao)', 86, 100, N'IELTS 7.5 - 8.5+', N'TOEIC 805 - 990', N'Thành ngữ Collocations cao cấp & Debate', N'#7E22CE', 5);

    PRINT N'Đã nạp 5 dòng ma trận quy đổi trình độ mặc định.';
END
GO

-- 3. BẢO ĐẢM TỒN TẠI BỘ ĐỀ KHẢO SÁT CHUẨN ĐANG HOẠT ĐỘNG
IF NOT EXISTS (SELECT 1 FROM BaiKhaoSatDauVao WHERE DangHoatDong = 1)
BEGIN
    UPDATE BaiKhaoSatDauVao SET DangHoatDong = 1 WHERE MaBaiKhaoSat = (SELECT TOP 1 MaBaiKhaoSat FROM BaiKhaoSatDauVao);
END
GO

-- 4. BỔ SUNG CỘT CHO TepTinNhapLieu NẾU THIẾU
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TepTinNhapLieu' AND COLUMN_NAME = 'SoCauHoiNhanDien')
BEGIN
    ALTER TABLE TepTinNhapLieu ADD SoCauHoiNhanDien INT NULL DEFAULT 0;
END
GO

PRINT N'Hoàn tất migration Sprint 3 cho database TiengAnhAI.';
GO
