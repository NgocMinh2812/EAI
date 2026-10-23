/*
    BẢNG: NoiDungHocTapAIBaiHoc
    Mục đích: Lưu kết quả phân tích nội dung bài học bằng AI một lần cho sinh viên,
              chống gọi AI lặp lại, kiểm soát trạng thái xử lý và tự động nhận diện bài học đã cập nhật (ContentHash).
*/

USE TiengAnhAI;
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'NoiDungHocTapAIBaiHoc')
BEGIN
    CREATE TABLE NoiDungHocTapAIBaiHoc (
        MaNoiDungAI INT IDENTITY(1,1) PRIMARY KEY,
        MaBaiHoc INT NOT NULL,
        ContentHash NVARCHAR(64) NOT NULL,
        TrangThai NVARCHAR(30) NOT NULL DEFAULT N'ChuaTao', -- ChuaTao, DangXuLy, HoanThanh, ThatBai
        KyNang NVARCHAR(50) NULL,
        MucTieu NVARCHAR(1000) NULL,
        DuLieuKetQuaJson NVARCHAR(MAX) NULL,
        MoHinhAI NVARCHAR(100) NULL,
        ThongBaoLoi NVARCHAR(MAX) NULL,
        ThoiDiemBatDauXuLy DATETIME2 NULL,
        NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        NgayCapNhat DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        CONSTRAINT UQ_NoiDungHocTapAIBaiHoc_MaBaiHoc UNIQUE (MaBaiHoc),
        CONSTRAINT FK_NoiDungHocTapAIBaiHoc_BaiHoc FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc) ON DELETE CASCADE
    );

    CREATE NONCLUSTERED INDEX IX_NoiDungHocTapAIBaiHoc_Hash 
    ON NoiDungHocTapAIBaiHoc(MaBaiHoc, ContentHash, TrangThai);

    PRINT N'Đã tạo bảng NoiDungHocTapAIBaiHoc thành công.';
END
ELSE
BEGIN
    PRINT N'Bảng NoiDungHocTapAIBaiHoc đã tồn tại.';
END
GO
