/*
    BẢNG VÀ CỘT MỚI: QUẢN LÝ BỘ CÂU HỎI BÀI HỌC VÀ KẾT QUẢ LÀM BÀI ĐỘC LẬP TỪNG SINH VIÊN
    - Bổ sung ContentHash, LoaiBoCauHoi, TrangThai, ThoiDiemBatDauXuLy, ThongBaoLoi vào bảng BoCauHoi.
    - Tạo bảng BoCauHoi_CauHoi (nếu chưa có).
    - Tạo bảng KetQuaLamBaiBoCauHoi lưu lượt làm bài, câu trả lời, điểm số độc lập cho từng học viên.
*/

USE TiengAnhAI;
GO

-- 1. Bổ sung các cột mở rộng cho bảng BoCauHoi
IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'BoCauHoi')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ContentHash' AND Object_ID = OBJECT_ID('BoCauHoi'))
        ALTER TABLE BoCauHoi ADD ContentHash NVARCHAR(64) NULL;
        
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'LoaiBoCauHoi' AND Object_ID = OBJECT_ID('BoCauHoi'))
        ALTER TABLE BoCauHoi ADD LoaiBoCauHoi NVARCHAR(30) NOT NULL DEFAULT N'CungCo';
        
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TrangThai' AND Object_ID = OBJECT_ID('BoCauHoi'))
        ALTER TABLE BoCauHoi ADD TrangThai NVARCHAR(30) NOT NULL DEFAULT N'HoanThanh';
        
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ThoiDiemBatDauXuLy' AND Object_ID = OBJECT_ID('BoCauHoi'))
        ALTER TABLE BoCauHoi ADD ThoiDiemBatDauXuLy DATETIME2 NULL;
        
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ThongBaoLoi' AND Object_ID = OBJECT_ID('BoCauHoi'))
        ALTER TABLE BoCauHoi ADD ThongBaoLoi NVARCHAR(MAX) NULL;

    PRINT N'Đã cập nhật các cột mở rộng cho bảng BoCauHoi.';
END
ELSE
BEGIN
    CREATE TABLE BoCauHoi (
        MaBoCauHoi INT IDENTITY(1,1) PRIMARY KEY,
        TenBoCauHoi NVARCHAR(250) NOT NULL,
        MoTa NVARCHAR(MAX) NULL,
        NguonTao NVARCHAR(50) NOT NULL DEFAULT N'AI',
        TenNguon NVARCHAR(250) NULL,
        MaBaiHoc INT NULL,
        MaChuDe INT NULL,
        MaTrinhDo INT NULL,
        NguoiTao INT NULL,
        ContentHash NVARCHAR(64) NULL,
        LoaiBoCauHoi NVARCHAR(30) NOT NULL DEFAULT N'CungCo',
        TrangThai NVARCHAR(30) NOT NULL DEFAULT N'HoanThanh',
        ThoiDiemBatDauXuLy DATETIME2 NULL,
        ThongBaoLoi NVARCHAR(MAX) NULL,
        NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        NgayCapNhat DATETIME2 NULL
    );
    PRINT N'Đã tạo mới bảng BoCauHoi.';
END
GO

-- 2. Tạo bảng liên kết BoCauHoi_CauHoi
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'BoCauHoi_CauHoi')
BEGIN
    CREATE TABLE BoCauHoi_CauHoi (
        MaBoCauHoi INT NOT NULL,
        MaCauHoi INT NOT NULL,
        ThuTu INT NOT NULL DEFAULT 1,
        PRIMARY KEY (MaBoCauHoi, MaCauHoi),
        CONSTRAINT FK_BoCauHoi_CauHoi_Bo FOREIGN KEY (MaBoCauHoi) REFERENCES BoCauHoi(MaBoCauHoi) ON DELETE CASCADE,
        CONSTRAINT FK_BoCauHoi_CauHoi_CH FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi) ON DELETE CASCADE
    );
    PRINT N'Đã tạo bảng BoCauHoi_CauHoi.';
END
GO

-- 3. Tạo bảng lưu kết quả làm bài tập độc lập của từng sinh viên
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'KetQuaLamBaiBoCauHoi')
BEGIN
    CREATE TABLE KetQuaLamBaiBoCauHoi (
        MaKetQua INT IDENTITY(1,1) PRIMARY KEY,
        MaNguoiDung INT NOT NULL,
        MaBoCauHoi INT NOT NULL,
        MaBaiHoc INT NOT NULL,
        LoaiBoCauHoi NVARCHAR(30) NOT NULL DEFAULT N'CungCo',
        ContentHash NVARCHAR(64) NOT NULL,
        TongSoCau INT NOT NULL DEFAULT 10,
        SoCauDung INT NOT NULL DEFAULT 0,
        DiemSo DECIMAL(5,2) NOT NULL DEFAULT 0,
        ThoiGianLamGiay INT NOT NULL DEFAULT 0,
        TrangThai NVARCHAR(30) NOT NULL DEFAULT N'DaNop',
        ChiTietTraLoiJson NVARCHAR(MAX) NULL,
        NgayNop DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        CONSTRAINT FK_KetQuaLamBaiBoCauHoi_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung) ON DELETE CASCADE,
        CONSTRAINT FK_KetQuaLamBaiBoCauHoi_BoCauHoi FOREIGN KEY (MaBoCauHoi) REFERENCES BoCauHoi(MaBoCauHoi) ON DELETE CASCADE,
        CONSTRAINT FK_KetQuaLamBaiBoCauHoi_BaiHoc FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc) ON DELETE NO ACTION
    );

    CREATE NONCLUSTERED INDEX IX_KetQuaLamBaiBoCauHoi_User_Lesson
    ON KetQuaLamBaiBoCauHoi(MaNguoiDung, MaBaiHoc, LoaiBoCauHoi, ContentHash);

    PRINT N'Đã tạo bảng KetQuaLamBaiBoCauHoi thành công.';
END
GO
