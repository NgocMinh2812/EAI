-- ====================================================================================
-- SCRIPT MIGRATION: CẤU TRÚC ĐỀ THI CHUẨN HÓA (BLUEPRINTS) & MỞ RỘNG NGÂN HÀNG CÂU HỎI
-- Dự án: EnglishAI (EAI) Desktop Application
-- Bảo đảm: Không xóa bảng, không mất dữ liệu, hoàn toàn Idempotent
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- 1. BỔ SUNG CÁC CỘT NGUỒN VÀ MÃ SEED VÀO BẢNG CauHoi
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CauHoi' AND COLUMN_NAME = 'LoaiNguon')
BEGIN
    ALTER TABLE CauHoi ADD LoaiNguon NVARCHAR(50) NULL DEFAULT 'Extracted';
    PRINT N'Đã thêm cột LoaiNguon vào bảng CauHoi.';
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CauHoi' AND COLUMN_NAME = 'TepTinNguon')
BEGIN
    ALTER TABLE CauHoi ADD TepTinNguon NVARCHAR(255) NULL;
    PRINT N'Đã thêm cột TepTinNguon vào bảng CauHoi.';
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CauHoi' AND COLUMN_NAME = 'MaDinhDanhSeed')
BEGIN
    ALTER TABLE CauHoi ADD MaDinhDanhSeed VARCHAR(100) NULL;
    PRINT N'Đã thêm cột MaDinhDanhSeed vào bảng CauHoi.';
END
GO

-- Tạo Unique Index cho MaDinhDanhSeed (chỉ áp dụng khi khác NULL để hỗ trợ Idempotency)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_CauHoi_MaDinhDanhSeed' AND object_id = OBJECT_ID('CauHoi'))
BEGIN
    CREATE UNIQUE INDEX UX_CauHoi_MaDinhDanhSeed ON CauHoi(MaDinhDanhSeed) WHERE MaDinhDanhSeed IS NOT NULL;
    PRINT N'Đã tạo Unique Index UX_CauHoi_MaDinhDanhSeed.';
END
GO

-- 2. TẠO BẢNG CauTrucDeThi (EXAM BLUEPRINTS)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'CauTrucDeThi')
BEGIN
    CREATE TABLE CauTrucDeThi (
        MaCauTruc INT IDENTITY(1,1) PRIMARY KEY,
        TenCauTruc NVARCHAR(200) NOT NULL,
        MaKyThi NVARCHAR(50) NOT NULL,               -- IELTS, TOEIC, VSTEP, CAMBRIDGE_KET, CAMBRIDGE_PET, CAMBRIDGE_FCE, CAMBRIDGE_CAE, STARTERS, MOVERS, FLYERS
        MaCapDo NVARCHAR(10) NOT NULL,               -- A1, A2, B1, B2, C1, MULTI
        LoaiDeThi NVARCHAR(50) NOT NULL DEFAULT 'ThiThu', -- ThiThu, LuyenTap, TongHop
        ThoiGianPhut INT NOT NULL,
        TongSoCau INT NOT NULL,
        TongDiem FLOAT NOT NULL DEFAULT 10.0,
        MoTa NVARCHAR(1000) NULL,
        NguonThamChieu NVARCHAR(255) NULL,           -- Link / Tài liệu quy chuẩn khảo thí quốc tế
        NgayXacMinh DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        TrangThai NVARCHAR(30) NOT NULL DEFAULT 'HoatDong'
    );
    PRINT N'Đã tạo bảng CauTrucDeThi thành công.';
END
GO

-- 3. TẠO BẢNG ChiTietCauTrucDeThi (SECTIONS / PARTS CỦA BLUEPRINT)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ChiTietCauTrucDeThi')
BEGIN
    CREATE TABLE ChiTietCauTrucDeThi (
        MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
        MaCauTruc INT NOT NULL FOREIGN KEY REFERENCES CauTrucDeThi(MaCauTruc) ON DELETE CASCADE,
        TenPhan NVARCHAR(200) NOT NULL,              -- Ví dụ: 'Part 1: Photographs', 'Reading Passage 1', 'Section 1: Listening'
        ThuTuPhan INT NOT NULL DEFAULT 1,
        KyNang NVARCHAR(50) NOT NULL,                -- Nghe, Doc, Viet, Noi, NguPhap, TuVung
        LoaiCauHoiChoPhep NVARCHAR(100) NOT NULL,    -- TracNghiem, DienKhuyet, TrueFalseNotGiven, DungSai, NoiCap, TuLuan, v.v.
        SoLuongCau INT NOT NULL,
        DiemMoiCau FLOAT NOT NULL DEFAULT 1.0,
        ThoiGianPhut INT NULL,
        YeuCauDoanVan BIT NOT NULL DEFAULT 0,
        YeuCauAudio BIT NOT NULL DEFAULT 0,
        MoTa NVARCHAR(500) NULL
    );
    PRINT N'Đã tạo bảng ChiTietCauTrucDeThi thành công.';
END
GO

-- 4. BỔ SUNG CỘT MaCauTrucDeThi VÀO BẢNG BaiTapDeThi
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BaiTapDeThi' AND COLUMN_NAME = 'MaCauTrucDeThi')
BEGIN
    ALTER TABLE BaiTapDeThi ADD MaCauTrucDeThi INT NULL FOREIGN KEY REFERENCES CauTrucDeThi(MaCauTruc);
    PRINT N'Đã thêm cột MaCauTrucDeThi vào bảng BaiTapDeThi.';
END
GO

PRINT N'Hoàn tất thiết kế Schema Migration cho Cấu trúc đề thi & Ngân hàng câu hỏi.';
GO
