USE TiengAnhAI
GO

INSERT INTO VaiTro(TenVaiTro, MoTa)
VALUES
(N'GiangVien', N'Quản trị viên quản lý nội dung, câu hỏi và hệ thống'),
(N'HocVien', N'Người học tiếng Anh ôn luyện và thi thử');
GO

INSERT INTO TrinhDo(MaCapDo, TenTrinhDo, MoTa)
VALUES
(N'A1', N'Beginner', N'Trình độ cơ bản'),
(N'A2', N'Elementary', N'Trình độ sơ cấp'),
(N'B1', N'Intermediate', N'Trình độ trung cấp'),
(N'B2', N'Upper-Intermediate', N'Trình độ trên trung cấp'),
(N'C1', N'Advanced', N'Trình độ nâng cao');
GO

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa)
VALUES
(NULL, N'Đời sống', N'Các chủ đề tiếng Anh thông dụng trong đời sống'),
(NULL, N'Kinh tế', N'Từ vựng và kiến thức tiếng Anh về kinh tế'),
(NULL, N'Môi trường', N'Chủ đề môi trường'),
(NULL, N'Giáo dục', N'Chủ đề giáo dục');
GO

INSERT INTO NguoiDung(MaVaiTro, TenDangNhap, MatKhauMaHoa, Email, HoTen)
VALUES
((SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro=N'QuanTriVien'),
 N'quantri', N'CHANGE_ME_HASH', N'admin@tienganhai.local', N'Quản trị viên'),
((SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro=N'HocVien'),
 N'hocvien01', N'CHANGE_ME_HASH', N'hocvien01@tienganhai.local', N'Học viên mẫu');
GO

INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu)
SELECT MaNguoiDung, 6.5, N'IELTS', N'B2'
FROM NguoiDung WHERE TenDangNhap=N'hocvien01';
GO

INSERT INTO TuVung(MaChuDe, MaTrinhDo, Tu, LoaiTu, PhienAmIPA, Nghia, CauViDu, CumTuDiKem)
SELECT
    (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe=N'Đời sống'),
    (SELECT MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'A2'),
    N'practice', N'verb', N'/ˈpræk.tɪs/',
    N'luyện tập', N'I practice English every day.',
    N'practice English'
UNION ALL
SELECT
    (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe=N'Giáo dục'),
    (SELECT MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'B1'),
    N'improve', N'verb', N'/ɪmˈpruːv/',
    N'cải thiện', N'I want to improve my English.',
    N'improve your skills';
GO
