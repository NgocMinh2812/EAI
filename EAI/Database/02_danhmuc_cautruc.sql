/*
    MODULE: 02_danhmuc_cautruc.sql
    Mục đích: Danh mục hệ thống & Cấu trúc đề thi chuẩn hóa (Exam Blueprints)
    Bao gồm: VaiTro, TrinhDo, CauHinhQuyDoiKhaoSat, NguoiDung, HoSoHocTap, ChuDe, CauTrucDeThi, ChiTietCauTrucDeThi
*/
USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

-- 0. XÓA DỮ LIỆU CŨ THEO ĐÚNG THỨ TỰ RÀNG BUỘC KHÓA NGOẠI (FOREIGN KEYS)
-- ===================================================================
IF OBJECT_ID('KetQuaLamBaiBoCauHoi', 'U') IS NOT NULL DELETE FROM KetQuaLamBaiBoCauHoi;
IF OBJECT_ID('BoCauHoi_CauHoi', 'U') IS NOT NULL DELETE FROM BoCauHoi_CauHoi;
IF OBJECT_ID('BoCauHoi', 'U') IS NOT NULL DELETE FROM BoCauHoi;
IF OBJECT_ID('NoiDungHocTapAIBaiHoc', 'U') IS NOT NULL DELETE FROM NoiDungHocTapAIBaiHoc;
DELETE FROM CauHoiDoAISinh;
DELETE FROM YeuCauSinhDeAI;
DELETE FROM KetQuaPhanTichAI;
DELETE FROM NhatKyGoiAPI;
DELETE FROM TinNhanAI;
DELETE FROM PhienHoiThoaiAI;
DELETE FROM DeXuatHocTap;
DELETE FROM DiemYeuNguoiDung;
DELETE FROM LichSuHocTap;
DELETE FROM DanhGiaBaiViet;
DELETE FROM ChiTietTraLoi;
DELETE FROM LuotLamBai;
DELETE FROM CauTraLoiKhaoSat;
DELETE FROM LuotKhaoSat;
DELETE FROM CauHoiKhaoSat;
DELETE FROM BaiKhaoSatDauVao;
DELETE FROM CauHoiBaiTap;
DELETE FROM BaiTapDeThi;
IF OBJECT_ID('ChiTietCauTrucDeThi', 'U') IS NOT NULL DELETE FROM ChiTietCauTrucDeThi;
IF OBJECT_ID('CauTrucDeThi', 'U') IS NOT NULL DELETE FROM CauTrucDeThi;
DELETE FROM LuaChonDapAn;
DELETE FROM CauHoi;
DELETE FROM ChuDeNguPhap;
DELETE FROM Flashcard;
DELETE FROM TuVung;
DELETE FROM TienDoBaiHoc;
IF OBJECT_ID('BaiHocAudioTranscript', 'U') IS NOT NULL DELETE FROM BaiHocAudioTranscript;
DELETE FROM BaiHoc;

-- Gỡ bỏ tham chiếu cha-con trước khi xóa bảng danh mục tự tham chiếu ChuDe
UPDATE ChuDe SET MaChuDeCha = NULL;
DELETE FROM ChuDe;

DELETE FROM HoSoHocTap;
DELETE FROM TepTinNhapLieu;
DELETE FROM NguoiDung;
DELETE FROM VaiTro;
DELETE FROM TrinhDo;
IF OBJECT_ID('CauHinhQuyDoiKhaoSat', 'U') IS NOT NULL DELETE FROM CauHinhQuyDoiKhaoSat;
GO

-- Reset giá trị tự tăng IDENTITY về 0 để các bản ghi mẫu nhận đúng ID từ 1 trở đi
IF OBJECT_ID('KetQuaLamBaiBoCauHoi', 'U') IS NOT NULL DBCC CHECKIDENT ('KetQuaLamBaiBoCauHoi', RESEED, 0) WITH NO_INFOMSGS;
IF OBJECT_ID('BoCauHoi', 'U') IS NOT NULL DBCC CHECKIDENT ('BoCauHoi', RESEED, 0) WITH NO_INFOMSGS;
IF OBJECT_ID('NoiDungHocTapAIBaiHoc', 'U') IS NOT NULL DBCC CHECKIDENT ('NoiDungHocTapAIBaiHoc', RESEED, 0) WITH NO_INFOMSGS;
IF OBJECT_ID('BaiHocAudioTranscript', 'U') IS NOT NULL DBCC CHECKIDENT ('BaiHocAudioTranscript', RESEED, 0) WITH NO_INFOMSGS;
IF OBJECT_ID('ChiTietCauTrucDeThi', 'U') IS NOT NULL DBCC CHECKIDENT ('ChiTietCauTrucDeThi', RESEED, 0) WITH NO_INFOMSGS;
IF OBJECT_ID('CauHinhQuyDoiKhaoSat', 'U') IS NOT NULL DBCC CHECKIDENT ('CauHinhQuyDoiKhaoSat', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('VaiTro', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('TrinhDo', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('ChuDe', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('NguoiDung', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('HoSoHocTap', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('BaiHoc', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('TuVung', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('CauHoi', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('BaiKhaoSatDauVao', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('BaiTapDeThi', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('YeuCauSinhDeAI', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('CauHoiDoAISinh', RESEED, 0) WITH NO_INFOMSGS;
GO

-- ===================================================================
-- 1. NẠP BẢNG VAI TRÒ (VaiTro)
-- ===================================================================
INSERT INTO VaiTro (TenVaiTro, MoTa)
VALUES
(N'GiangVien', N'Giảng viên quản lý học liệu, đề thi, câu hỏi và phân tích học viên'),
(N'HocVien', N'Người học tiếng Anh ôn luyện, làm bài tập và thi thử'),
(N'QuanTriVien', N'Quản trị viên toàn quyền cấu hình hệ thống');
GO

-- ===================================================================
-- 2. NẠP BẢNG TRÌNH ĐỘ (TrinhDo)
-- ===================================================================
INSERT INTO TrinhDo (MaCapDo, TenTrinhDo, MoTa)
VALUES
(N'A1', N'Beginner', N'Trình độ mở đầu, nắm bắt từ ngữ giao tiếp đơn giản'),
(N'A2', N'Elementary', N'Trình độ sơ cấp, giao tiếp các tình huống thông thường'),
(N'B1', N'Intermediate', N'Trình độ trung cấp, hiểu ý chính văn bản và giao tiếp tự tin'),
(N'B2', N'Upper-Intermediate', N'Trình độ trên trung cấp, sử dụng tiếng Anh linh hoạt và học thuật'),
(N'C1', N'Advanced', N'Trình độ cao cấp, diễn đạt trôi chảy và hiệu quả trong mọi ngữ cảnh'),
(N'C2', N'Proficiency', N'Trình độ thành thạo, tương đương người bản ngữ');
GO

-- ===================================================================
-- ===================================================================
-- 2.1 NẠP MA TRẬN QUY ĐỔI KHẢO SÁT TRÌNH ĐỘ (CauHinhQuyDoiKhaoSat)
-- ===================================================================
INSERT INTO CauHinhQuyDoiKhaoSat 
    (MaCapDo, TenHienThi, DiemPhanTramMin, DiemPhanTramMax, QuyDoiIelts, QuyDoiToeic, LoTrinhDeXuat, MauSac, ThuTuSapXep)
VALUES
    (N'A1', N'A1 (Mất gốc)', 0, 30, N'IELTS < 3.0', N'TOEIC 100 - 250', N'Ngữ pháp nền tảng + 500 từ vựng cốt lõi', N'#DC2626', 1),
    (N'A2', N'A2 (Sơ cấp)', 31, 50, N'IELTS 3.5 - 4.5', N'TOEIC 255 - 450', N'Luyện phản xạ câu & Nghe hiểu đơn giản', N'#EA580C', 2),
    (N'B1', N'B1 (Trung cấp)', 51, 70, N'IELTS 5.0 - 6.0', N'TOEIC 455 - 650', N'Từ vựng học thuật B1 & Cấu trúc câu ghép', N'#2563EB', 3),
    (N'B2', N'B2 (Trên trung cấp)', 71, 85, N'IELTS 6.5 - 7.0', N'TOEIC 655 - 800', N'Viết luận Task 2 & Skimming/Scanning đọc hiểu', N'#15803D', 4),
    (N'C1', N'C1 (Nâng cao)', 86, 100, N'IELTS 7.5 - 8.5+', N'TOEIC 805 - 990', N'Thành ngữ Collocations cao cấp & Debate', N'#7E22CE', 5);
GO

-- 3. NẠP BẢNG NGƯỜI DÙNG (NguoiDung)
-- ===================================================================
DECLARE @idGV INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro=N'GiangVien');
DECLARE @idHV INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro=N'HocVien');
DECLARE @idQTV INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro=N'QuanTriVien');

INSERT INTO NguoiDung (MaVaiTro, TenDangNhap, MatKhauMaHoa, Email, HoTen, SoDienThoai, TrangThaiHoatDong, LaLanDauDangNhap, NgayTao, LanDangNhapCuoi)
VALUES
-- Quản trị viên
(@idQTV, N'admin', N'123456', N'admin@gmail.com', N'Quản trị viên Hệ thống', N'0999888777', 1, 0, DATEADD(day, -100, SYSDATETIME()), SYSDATETIME()),

-- Giảng viên
(@idGV, N'giangvien', N'123456', N'giangvien@gmail.com', N'Giảng viên Nguyễn Văn An', N'0988777666', 1, 0, DATEADD(day, -90, SYSDATETIME()), DATEADD(minute, -12, SYSDATETIME())),
(@idGV, N'tung.hoang', N'123456', N'tung.hoang@edu.vn', N'Hoàng Văn Tùng', N'0912345678', 1, 0, DATEADD(day, -60, SYSDATETIME()), DATEADD(hour, -2, SYSDATETIME())),
(@idGV, N'maianh.tran', N'123456', N'maianh.tran@tienganhai.vn', N'Trần Mai Anh', N'0987654321', 1, 0, DATEADD(day, -45, SYSDATETIME()), DATEADD(day, -1, SYSDATETIME())),

-- Học viên
(@idHV, N'hocvien', N'123456', N'hocvien@gmail.com', N'Học viên Trần Thị Mai', N'0977666555', 1, 0, DATEADD(day, -80, SYSDATETIME()), NULL),
(@idHV, N'lan.nguyen', N'123456', N'lan.nguyen@gmail.com', N'Nguyễn Thị Lan', N'0901234561', 1, 0, DATEADD(day, -30, SYSDATETIME()), DATEADD(hour, -5, SYSDATETIME())),
(@idHV, N'khoa.tran', N'123456', N'khoa.tran@outlook.com', N'Trần Minh Khoa', N'0901234562', 1, 0, DATEADD(day, -25, SYSDATETIME()), DATEADD(day, -2, SYSDATETIME())),
(@idHV, N'ducanh.pham', N'123456', N'ducanh.pham@gmail.com', N'Phạm Đức Anh', N'0901234563', 0, 0, DATEADD(day, -50, SYSDATETIME()), NULL),
(@idHV, N'huong.le', N'123456', N'huong.le@yahoo.com', N'Lê Thu Hương', N'0901234564', 1, 0, DATEADD(day, -20, SYSDATETIME()), DATEADD(day, -4, SYSDATETIME())),
(@idHV, N'nam.vu', N'123456', N'nam.vu@company.vn', N'Vũ Hoàng Nam', N'0901234565', 1, 0, DATEADD(day, -15, SYSDATETIME()), DATEADD(day, -9, SYSDATETIME())),
(@idHV, N'mai.dang', N'123456', N'mai.dang@gmail.com', N'Đặng Thị Mai', N'0901234566', 0, 0, DATEADD(day, -40, SYSDATETIME()), NULL),
(@idHV, N'chau.ngo', N'123456', N'chau.ngo@student.edu.vn', N'Ngô Bảo Châu', N'0901234567', 1, 0, DATEADD(day, -10, SYSDATETIME()), DATEADD(day, -15, SYSDATETIME())),
(@idHV, N'bao.do', N'123456', N'bao.do@gmail.com', N'Đỗ Quốc Bảo', N'0901234568', 1, 0, DATEADD(day, -5, SYSDATETIME()), DATEADD(day, -22, SYSDATETIME()));
GO

-- ===================================================================
-- 4. NẠP HỒ SƠ HỌC TẬP (HoSoHocTap)
-- ===================================================================
-- Đảm bảo cột TrinhDoDauVao và DiemDanhGiaDauVao tồn tại nếu CSDL được tạo trước đó
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TrinhDoDauVao' AND Object_ID = Object_ID('HoSoHocTap'))
BEGIN
    ALTER TABLE HoSoHocTap ADD TrinhDoDauVao NVARCHAR(10) NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'DiemDanhGiaDauVao' AND Object_ID = Object_ID('HoSoHocTap'))
BEGIN
    ALTER TABLE HoSoHocTap ADD DiemDanhGiaDauVao DECIMAL(5,2) NULL;
END
GO

INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan)
SELECT nd.MaNguoiDung, 7.0, N'IELTS', N'B2', N'B1', 6.5, 420, N'Tập trung luyện Reading và Writing task 2'
FROM NguoiDung nd WHERE nd.TenDangNhap = N'lan.nguyen';

INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan)
SELECT nd.MaNguoiDung, 650.0, N'TOEIC', N'B1', N'A2', 5.0, 280, N'Cải thiện kỹ năng nghe part 3-4'
FROM NguoiDung nd WHERE nd.TenDangNhap = N'khoa.tran';

INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan)
SELECT nd.MaNguoiDung, 5.0, N'IELTS', N'A2', N'A1', 4.0, 90, N'Củng cố lại ngữ pháp cơ bản'
FROM NguoiDung nd WHERE nd.TenDangNhap = N'ducanh.pham';

INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan)
SELECT nd.MaNguoiDung, 8.0, N'IELTS', N'C1', N'B2', 8.5, 650, N'Mục tiêu du học thạc sĩ'
FROM NguoiDung nd WHERE nd.TenDangNhap = N'huong.le';

INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan)
SELECT nd.MaNguoiDung, 750.0, N'TOEIC', N'B1', N'A2', 5.5, 310, N'Phục vụ công việc công ty'
FROM NguoiDung nd WHERE nd.TenDangNhap = N'nam.vu';

INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan)
SELECT nd.MaNguoiDung, 6.5, N'IELTS', N'B2', N'B1', 6.0, 180, N'Đang bảo lưu tài khoản'
FROM NguoiDung nd WHERE nd.TenDangNhap = N'mai.dang';

INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan)
SELECT nd.MaNguoiDung, 7.5, N'IELTS', N'B2', N'B1', 6.5, 510, N'Chuẩn bị thi tháng 11'
FROM NguoiDung nd WHERE nd.TenDangNhap = N'chau.ngo';

INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan)
SELECT nd.MaNguoiDung, 800.0, N'TOEIC', N'B2', N'B1', 7.0, 390, N'Học thêm từ vựng chuyên ngành'
FROM NguoiDung nd WHERE nd.TenDangNhap = N'bao.do';
GO

-- ===================================================================
-- ===================================================================
-- 5. NẠP CÂY DANH MỤC CHỦ ĐỀ PHÂN CẤP CHUẨN HÓA (IDs 1 -> 26)
-- ===================================================================
SET IDENTITY_INSERT ChuDe ON;
INSERT INTO ChuDe (MaChuDe, MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong) VALUES
    (1, NULL, N'Tiếng Anh Giao Tiếp', N'Các chủ đề giao tiếp thực tế hàng ngày', 1),
    (2, 1, N'Chào hỏi & Giới thiệu bản thân', N'Từ vựng và mẫu câu khi mới gặp mặt', 1),
    (3, 1, N'Mua sắm & Nhà hàng ẩm thực', N'Mẫu câu gọi món, trả giá, thanh toán', 1),
    (4, 1, N'Du lịch & Phương tiện giao thông', N'Hỏi đường, thủ tục sân bay, khách sạn', 1),
    (5, NULL, N'Tiếng Anh Học Thuật (IELTS)', N'Luyện thi học thuật và các kỹ năng chuyên sâu', 1),
    (6, 5, N'IELTS Reading — Skimming & Scanning', N'Kỹ năng đọc nhanh, tìm từ khóa', 1),
    (7, 5, N'IELTS Writing Task 2 — Luận xã hội', N'Cấu trúc bài luận 4 đoạn chuẩn học thuật', 1),
    (8, 5, N'IELTS Listening — Section 3 & 4', N'Hội thoại học thuật và bài giảng đại học', 1),
    (9, NULL, N'Ngữ Pháp Cốt Lõi', N'Hệ thống ngữ pháp tiếng Anh từ căn bản đến nâng cao', 1),
    (10, 9, N'Các thì trong tiếng Anh (Tenses)', N'12 thì và ứng dụng thực tiễn', 1),
    (11, 9, N'Mệnh đề quan hệ & Câu bị động', N'Ngữ pháp ghép câu và diễn đạt tự nhiên', 1),
    (12, 9, N'Câu điều kiện & Đảo ngữ', N'Ngữ pháp nâng cao cho band điểm cao', 1),
    (13, NULL, N'Tiếng Anh Thương Mại & Công Sở', N'Tiếng Anh doanh nghiệp, đàm phán, email', 1),
    (14, 13, N'Viết Email & Thư tín Thương mại', N'Quy chuẩn viết email chuyên nghiệp', 1),
    (15, 13, N'Đàm phán & Thuyết trình Dự án', N'Ngôn ngữ đàm phán và thuyết phục đối tác', 1),
    (16, NULL, N'Tiếng Anh Công Nghệ Thông Tin (IT English)', N'Tiếng Anh chuyên ngành phần mềm, Agile và AI', 1),
    (17, 16, N'Thuật ngữ Lập trình & Kỹ thuật Phần mềm', N'Data structures, API, DevOps và bảo mật hệ thống', 1),
    (18, 16, N'Giao tiếp Agile, Daily Scrum & Phỏng vấn IT', N'Standup meetings, sprint retro và phỏng vấn kỹ thuật', 1),
    (19, 9, N'Đảo ngữ & Cấu trúc ngữ pháp nâng cao (C1)', N'Inversion, subjunctive và cấu trúc câu phức hợp', 1),
    (20, 1, N'Gia đình & Đời sống thường nhật', N'Mô tả gia đình, người thân và thói quen sinh hoạt', 1),
    (21, 1, N'Thể thao & Giải trí', N'Từ vựng, sở thích thể thao và hoạt động giải trí', 1),
    (22, 1, N'Thời tiết & Môi trường sống', N'Mô tả thời tiết, khí hậu và môi trường sống', 1),
    (23, 9, N'Câu gián tiếp & Tường thuật (Reported Speech)', N'Quy tắc lùi thì, trạng từ và động từ tường thuật', 1),
    (24, 5, N'IELTS Speaking - Phỏng vấn & Thuyết trình', N'Phản xạ giao tiếp và thuyết trình học thuật theo chủ đề', 1),
    (25, 13, N'Thuyết trình & Đàm phán Doanh nghiệp', N'Kỹ năng pitch ý tưởng và đàm phán hợp đồng thương mại', 1),
    (26, 16, N'Kiến trúc Hệ thống & An toàn thông tin', N'Cloud computing, DevSecOps và hệ thống phân tán', 1);
SET IDENTITY_INSERT ChuDe OFF;
GO

-- 16. NẠP 15 CẤU TRÚC ĐỀ THI CHUẨN HÓA (EXAM BLUEPRINTS) VÀ 58 PHÂN ĐOẠN CHI TIẾT
-- ===================================================================
PRINT N'Bắt đầu nạp 15 Exam Blueprints chuẩn hóa và 58 phân đoạn chi tiết...';

SET IDENTITY_INSERT CauTrucDeThi ON;

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 1)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (1, N'IELTS Academic Full Examination Blueprint', 'IELTS', 'B2', 'ThiThu', 160, 40, 10.0,
            N'Cấu trúc bài thi IELTS Học thuật chuẩn quốc tế gồm 4 kỹ năng: Listening (4 Sections), Reading (3 Passages), Writing (2 Tasks) và Speaking (3 Parts).',
            N'Cambridge English Language Assessment & British Council IELTS Specifications', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 2)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (2, N'IELTS Academic Reading Examination Blueprint', 'IELTS', 'B2', 'ThiThu', 60, 40, 10.0,
            N'Cấu trúc bài thi IELTS Academic Reading chuẩn quốc tế gồm 40 câu hỏi phân bổ qua 3 bài đọc học thuật (Passages 1, 2, 3), đánh giá toàn diện kỹ năng Skimming, Scanning, suy luận logic và vốn từ vựng học thuật B2-C1.',
            N'IELTS Academic Official Test Guidelines (British Council & IDP)', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 3)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (3, N'TOEIC Listening & Reading 200 Questions Blueprint', 'TOEIC', 'B1', 'ThiThu', 120, 200, 990.0,
            N'Cấu trúc đề thi TOEIC Quốc tế 2 kỹ năng Nghe & Đọc chuẩn ETS gồm 7 Part đánh giá năng lực tiếng Anh trong môi trường doanh nghiệp quốc tế.',
            N'ETS (Educational Testing Service) TOEIC Test Specifications', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 4)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (4, N'TOEIC Reading Test Part 5 & 6 Blueprint', 'TOEIC', 'B1', 'ThiThu', 45, 40, 10.0,
            N'Cấu trúc bài thi TOEIC Reading chuẩn hóa Phần 5 & 6 gồm 40 câu hỏi: Kiểm tra ngữ pháp thực hành công sở, thì động từ, liên từ, từ loại và đọc điền đoạn văn bản thương mại.',
            N'ETS TOEIC Reading Test Handbook', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 5)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (5, N'VSTEP B1-B2-C1 Examination Blueprint', 'VSTEP', 'B2', 'ThiThu', 180, 80, 10.0,
            N'Định dạng đề thi Đánh giá năng lực tiếng Anh bậc 3-5 theo Khung năng lực ngoại ngữ 6 bậc dùng cho Việt Nam gồm đầy đủ 4 kỹ năng.',
            N'Quyết định số 729/QĐ-BGDĐT của Bộ Giáo dục và Đào tạo', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 6)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (6, N'VSTEP B2 Standard Reading & Language Blueprint', 'VSTEP', 'B2', 'ThiThu', 60, 40, 10.0,
            N'Cấu trúc đề thi Đánh giá năng lực tiếng Anh Bậc 4 (VSTEP B2) chuẩn Bộ Giáo dục & Đào tạo gồm 40 câu hỏi phân bổ qua các phần đọc hiểu và kiến thức ngôn ngữ học thuật chuyên sâu.',
            N'Khung ĐGNL Tiếng Anh Bậc 4 VSTEP - ĐHQGHN & Bộ GD&ĐT', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 7)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (7, N'Cambridge A2 Key (KET) Examination Blueprint', 'CAMBRIDGE_KET', 'A2', 'ThiThu', 45, 30, 10.0,
            N'Cấu trúc bài thi quốc tế Cambridge A2 Key đánh giá khả năng giao tiếp đơn giản và đọc hiểu trong các tình huống thường nhật.',
            N'Cambridge Assessment English - A2 Key Handbook for Teachers', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 8)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (8, N'Cambridge B1 Preliminary (PET) Examination Blueprint', 'CAMBRIDGE_PET', 'B1', 'ThiThu', 130, 67, 10.0,
            N'Cấu trúc bài thi quốc tế Cambridge B1 Preliminary chứng nhận khả năng nắm vững các kiến thức tiếng Anh căn bản trong công việc và du lịch.',
            N'Cambridge Assessment English - B1 Preliminary Specifications', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 9)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (9, N'Cambridge B2 First (FCE) Examination Blueprint', 'CAMBRIDGE_FCE', 'B2', 'ThiThu', 60, 35, 10.0,
            N'Cấu trúc bài thi quốc tế Cambridge B2 First chứng nhận trình độ trung cao cấp, đủ năng lực làm việc trong môi trường nói tiếng Anh độc lập.',
            N'Cambridge Assessment English - B2 First Handbook for Teachers', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 10)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (10, N'Cambridge C1 Advanced (CAE) Examination Blueprint', 'CAMBRIDGE_CAE', 'C1', 'ThiThu', 90, 40, 10.0,
            N'Cấu trúc bài thi chuyên sâu Cambridge C1 Advanced chứng nhận năng lực ngôn ngữ xuất sắc ở bậc đại học và chuyên nghiệp toàn cầu.',
            N'Cambridge Assessment English - C1 Advanced Specifications', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 11)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (11, N'Cambridge Pre A1 Starters (YLE) Blueprint', 'STARTERS', 'A1', 'ThiThu', 40, 30, 10.0,
            N'Khung đánh giá tiếng Anh thiếu nhi khởi đầu Pre A1 Starters: Nhận biết đồ vật, màu sắc, từ vựng gia đình và chỉ dẫn đơn giản.',
            N'Cambridge English Young Learners (YLE) Pre A1 Handbook', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 12)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (12, N'Cambridge A1 Movers (YLE) Blueprint', 'MOVERS', 'A1', 'ThiThu', 45, 35, 10.0,
            N'Khung đánh giá tiếng Anh thiếu nhi A1 Movers: Nghe và đọc các mẩu hội thoại ngắn, mô tả người và đồ vật quen thuộc.',
            N'Cambridge English Young Learners (YLE) A1 Handbook', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 13)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (13, N'Cambridge A2 Flyers (YLE) Blueprint', 'FLYERS', 'A2', 'ThiThu', 50, 40, 10.0,
            N'Khung đánh giá tiếng Anh thiếu nhi A2 Flyers: Đọc hiểu các câu chuyện ngắn, trả lời câu hỏi và viết câu miêu tả hoàn chỉnh.',
            N'Cambridge English Young Learners (YLE) A2 Handbook', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 14)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (14, N'CEFR Adaptive Skills Practice Blueprint', 'CEFR_PRACTICE', 'MULTI', 'LuyenTap', 20, 10, 10.0,
            N'Cấu trúc gói luyện tập thích ứng linh hoạt theo 6 bậc CEFR (A1-C1): Học viên ôn tập nhanh 10-15 câu theo từng chuyên đề ngữ pháp, từ vựng hoặc đọc hiểu.',
            N'Council of Europe - Common European Framework of Reference for Languages (CEFR)', SYSDATETIME(), 'HoatDong');

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 15)
    INSERT INTO CauTrucDeThi (MaCauTruc, TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (15, N'Đề thi Tốt nghiệp THPT Quốc Gia môn Tiếng Anh Blueprint', 'THPT_QG', 'B1', 'ThiThu', 60, 50, 10.0,
            N'Cấu trúc đề thi Tốt nghiệp THPT Quốc Gia môn Tiếng Anh chuẩn quy định của Bộ Giáo dục và Đào tạo gồm 50 câu trắc nghiệm khách quan trong thời gian 60 phút.',
            N'Bộ Giáo dục và Đào tạo - Ma trận đề thi Tốt nghiệp THPT môn Tiếng Anh', SYSDATETIME(), 'HoatDong');

SET IDENTITY_INSERT CauTrucDeThi OFF;
GO

DELETE FROM ChiTietCauTrucDeThi;
INSERT INTO ChiTietCauTrucDeThi (MaCauTruc, TenPhan, ThuTuPhan, KyNang, LoaiCauHoiChoPhep, SoLuongCau, DiemMoiCau, ThoiGianPhut, YeuCauDoanVan, YeuCauAudio, MoTa)
VALUES
    -- MaCauTruc = 1: IELTS Academic Full (9 Sections)
    (1, N'Section 1: Listening Social Dialogue', 1, 'Nghe', 'DienKhuyet,TracNghiem', 10, 0.25, 10, 0, 1, N'Hội thoại đời sống thường nhật giữa 2 người (đặt phòng, hỏi thông tin dịch vụ).'),
    (1, N'Section 2: Listening Public Monologue', 2, 'Nghe', 'TracNghiem,NoiCap', 10, 0.25, 10, 0, 1, N'Bài độc thoại hướng dẫn về tiện ích công cộng hoặc sự kiện văn hóa địa phương.'),
    (1, N'Section 3: Listening Educational Discussion', 3, 'Nghe', 'TracNghiem,NoiCap', 10, 0.25, 10, 0, 1, N'Thảo luận học thuật từ 2-4 người (sinh viên và giáo sư hướng dẫn đề tài nghiên cứu).'),
    (1, N'Section 4: Listening Academic Lecture', 4, 'Nghe', 'DienKhuyet', 10, 0.25, 10, 0, 1, N'Bài giảng học thuật độc thoại chuyên ngành (khoa học, lịch sử, môi trường).'),
    (1, N'Passage 1: Academic Reading Foundation', 5, 'Doc', 'TrueFalseNotGiven,TracNghiem,DienKhuyet', 13, 0.25, 20, 1, 0, N'Văn bản đọc học thuật mô tả thực tế, sự kiện lịch sử hoặc tiểu sử khoa học.'),
    (1, N'Passage 2: Academic Reading Analysis', 6, 'Doc', 'TracNghiem,NoiCap,DienKhuyet', 13, 0.25, 20, 1, 0, N'Bài đọc phân tích quy trình công nghệ hoặc thảo luận chuyên ngành.'),
    (1, N'Passage 3: Complex Discussion & Argument', 7, 'Doc', 'TrueFalseNotGiven,TracNghiem', 14, 0.25, 20, 1, 0, N'Văn bản lập luận phức hợp với các quan điểm đối lập nhau.'),
    (1, N'Task 1: Academic Writing Data Report', 8, 'Viet', 'TuLuan', 1, 3.33, 20, 0, 0, N'Miêu tả và phân tích biểu đồ, bảng biểu hoặc quy trình (tối thiểu 150 từ).'),
    (1, N'Task 2: Discursive Essay', 9, 'Viet', 'TuLuan', 1, 6.67, 40, 0, 0, N'Viết bài luận nghị luận xã hội về các vấn đề toàn cầu (tối thiểu 250 từ).'),

    -- MaCauTruc = 2: IELTS Academic Reading (3 Passages)
    (2, N'Passage 1: Academic Reading Foundation & Core Facts', 1, 'Doc', 'TracNghiem,DungSai,DienKhuyet', 13, 0.25, 20, 1, 0, N'Bài đọc học thuật giới thiệu chủ đề thực tế, tiểu sử hoặc lịch sử khoa học.'),
    (2, N'Passage 2: Technology, Science & Research Analysis', 2, 'Doc', 'TracNghiem,NoiCap,DienKhuyet', 13, 0.25, 20, 1, 0, N'Bài đọc chuyên ngành công nghệ, y sinh hoặc trí tuệ nhân tạo.'),
    (2, N'Passage 3: Discursive Argument & Complex Critical Thinking', 3, 'Doc', 'TracNghiem,DungSai,DienKhuyet', 14, 0.25, 20, 1, 0, N'Bài đọc phân tích các luồng quan điểm đối lập, suy luận học thuật chuyên sâu.'),

    -- MaCauTruc = 3: TOEIC Listening & Reading (7 Parts)
    (3, N'Part 1: Photographs (Mô tả tranh)', 1, 'Nghe', 'TracNghiem', 6, 5.0, 5, 0, 1, N'Nghe 4 phương án miêu tả khách quan bức tranh chụp hình động và đồ vật.'),
    (3, N'Part 2: Question - Response (Hỏi - Đáp)', 2, 'Nghe', 'TracNghiem', 25, 5.0, 10, 0, 1, N'Nghe một câu hỏi và 3 câu phản hồi để chọn phương án giao tiếp phù hợp.'),
    (3, N'Part 3: Short Conversations (Hội thoại ngắn)', 3, 'Nghe', 'TracNghiem', 39, 5.0, 15, 0, 1, N'Nghe 13 đoạn hội thoại công sở giữa 2-3 người, mỗi đoạn trả lời 3 câu hỏi.'),
    (3, N'Part 4: Short Talks (Bài nói ngắn)', 4, 'Nghe', 'TracNghiem', 30, 5.0, 15, 0, 1, N'Nghe 10 bài thông báo, tin nhắn thoại, bản tin hoặc bài phát biểu, mỗi bài 3 câu hỏi.'),
    (3, N'Part 5: Incomplete Sentences (Điền câu)', 5, 'NguPhap', 'TracNghiem', 30, 5.0, 20, 0, 0, N'Hoàn thành câu thương mại với các phương án ngữ pháp, từ loại và từ vựng doanh nghiệp.'),
    (3, N'Part 6: Text Completion (Điền đoạn văn)', 6, 'Doc', 'TracNghiem', 16, 5.0, 15, 1, 0, N'Điền từ và câu phù hợp vào 4 văn bản công sở (email, thông báo, thư ngỏ).'),
    (3, N'Part 7: Reading Comprehension (Đọc hiểu văn bản)', 7, 'Doc', 'TracNghiem', 54, 5.0, 40, 1, 0, N'Đọc hiểu đoạn đơn (Single passages) và đoạn đa (Double & Triple passages) thương mại.'),

    -- MaCauTruc = 4: TOEIC Reading Part 5 & 6 (2 Parts)
    (4, N'Part 5: Incomplete Sentences (Hoàn thành câu thương mại)', 1, 'NguPhap', 'TracNghiem', 28, 0.25, 25, 0, 0, N'Ngữ pháp công sở, từ loại, thì động từ và từ vựng hợp đồng doanh nghiệp.'),
    (4, N'Part 6: Text Completion (Điền đoạn văn bản công sở)', 2, 'Doc', 'TracNghiem', 12, 0.25, 20, 1, 0, N'Đọc hiểu điền thông báo, email và bản tin công ty.'),

    -- MaCauTruc = 5: VSTEP B1-B2-C1 Full (4 Kỹ năng)
    (5, N'Kỹ năng Nghe (Listening) - 3 Phần', 1, 'Nghe', 'TracNghiem', 35, 0.28, 40, 0, 1, N'Part 1: 8 hướng dẫn thông báo; Part 2: 3 bài hội thoại; Part 3: 3 bài giảng/thuyết trình.'),
    (5, N'Kỹ năng Đọc (Reading) - 4 Bài đọc', 2, 'Doc', 'TracNghiem', 40, 0.25, 60, 1, 0, N'4 bài đọc độ dài khoảng 450-500 từ mỗi bài thuộc các chủ đề xã hội, văn hóa, khoa học.'),
    (5, N'Kỹ năng Viết (Writing) - 2 Bài viết', 3, 'Viet', 'TuLuan', 2, 5.0, 60, 0, 0, N'Task 1: Viết thư/email (120 từ); Task 2: Viết bài luận tranh luận xã hội (250 từ).'),
    (5, N'Kỹ năng Nói (Speaking) - 3 Phần', 4, 'Noi', 'TuLuan', 3, 3.33, 12, 0, 0, N'Part 1: Giao tiếp xã hội; Part 2: Thảo luận giải pháp; Part 3: Phát triển chủ đề tự chọn.'),

    -- MaCauTruc = 6: VSTEP B2 Standard Reading & Language (4 Phần)
    (6, N'Phần 1: Ngữ pháp phức hợp, Đảo ngữ & Cấu trúc B2', 1, 'NguPhap', 'TracNghiem,SuaLoiSai,VietLaiCau', 10, 0.25, 12, 0, 0, N'Mệnh đề quan hệ rút gọn, đảo ngữ điều kiện, bị động nâng cao.'),
    (6, N'Phần 2: Đọc hiểu văn bản Khoa học & Công nghệ', 2, 'Doc', 'TracNghiem,DienKhuyet', 10, 0.25, 16, 1, 0, N'Đọc hiểu tài liệu khoa học và công nghệ hiện đại.'),
    (6, N'Phần 3: Đọc hiểu Môi trường & Phát triển bền vững', 3, 'Doc', 'TracNghiem,DienKhuyet', 10, 0.25, 16, 1, 0, N'Đọc hiểu biến đổi khí hậu, sinh thái và bảo tồn.'),
    (6, N'Phần 4: Đọc hiểu Văn hóa, Xã hội & Nghị luận', 4, 'Doc', 'TracNghiem,DungSai', 10, 0.25, 16, 1, 0, N'Văn bản phân tích tác động xã hội và tư duy biện luận.'),

    -- MaCauTruc = 7: Cambridge A2 Key (KET) (3 Papers)
    (7, N'Paper 1: Reading and Writing', 1, 'Doc', 'TracNghiem,DienKhuyet', 20, 0.33, 30, 1, 0, N'Gồm 7 phần: Biển báo thông tin ngắn, đọc hiểu văn bản, điền từ vào chỗ trống và viết tin nhắn ngắn.'),
    (7, N'Paper 2: Listening', 2, 'Nghe', 'TracNghiem,DienKhuyet', 10, 0.33, 15, 0, 1, N'Gồm 5 phần nghe: Thông tin đời sống thường ngày, điền ghi chú số điện thoại/giờ giấc.'),
    (7, N'Paper 3: Speaking', 3, 'Noi', 'TuLuan', 2, 5.0, 10, 0, 0, N'Gồm 2 phần: Phỏng vấn thông tin cá nhân và thảo luận chủ đề quen thuộc với giám khảo.'),

    -- MaCauTruc = 8: Cambridge B1 Preliminary (PET) (4 Papers)
    (8, N'Paper 1: Reading', 1, 'Doc', 'TracNghiem,DienKhuyet', 32, 0.31, 45, 1, 0, N'6 phần đọc: Đọc biển báo, ghép người với thông tin (Matching), đọc hiểu bài báo và điền từ cloze.'),
    (8, N'Paper 2: Writing', 2, 'Viet', 'TuLuan', 2, 5.0, 45, 0, 0, N'2 phần viết: Viết email hồi đáp (100 từ) và viết một bài báo hoặc câu chuyện ngắn (100 từ).'),
    (8, N'Paper 3: Listening', 3, 'Nghe', 'TracNghiem,DienKhuyet', 25, 0.4, 30, 0, 1, N'4 phần nghe: Thông báo ngắn, cuộc hội thoại 2 người và bài phỏng vấn dài.'),
    (8, N'Paper 4: Speaking', 4, 'Noi', 'TuLuan', 4, 2.5, 12, 0, 0, N'4 phần nói: Chào hỏi, mô tả tranh ảnh màu đơn, thảo luận tình huống và bày tỏ ý kiến.'),

    -- MaCauTruc = 9: Cambridge B2 First (FCE) (4 Papers)
    (9, N'Paper 1: Reading and Use of English', 1, 'NguPhap', 'TracNghiem,DienKhuyet', 35, 0.28, 60, 1, 0, N'7 phần: Multiple-choice cloze, Open cloze, Word formation, Key word transformation, Reading comprehension.'),
    (9, N'Paper 2: Writing', 2, 'Viet', 'TuLuan', 2, 5.0, 80, 0, 0, N'Part 1: Viết bài luận (140-190 từ); Part 2: Chọn 1 trong các dạng (article, email, review, report).'),
    (9, N'Paper 3: Listening', 3, 'Nghe', 'TracNghiem,DienKhuyet', 30, 0.33, 40, 0, 1, N'4 phần nghe: Độc thoại ngắn, bài nói có khoảng trống, ghép nhiều người nói và phỏng vấn.'),
    (9, N'Paper 4: Speaking', 4, 'Noi', 'TuLuan', 4, 2.5, 14, 0, 0, N'4 phần nói: Phỏng vấn, so sánh 2 bức ảnh (1 phút độc thoại), nhiệm vụ tương tác và thảo luận sâu.'),

    -- MaCauTruc = 10: Cambridge C1 Advanced (CAE) (4 Papers)
    (10, N'Paper 1: Reading and Use of English', 1, 'Doc', 'TracNghiem,DienKhuyet', 40, 0.25, 90, 1, 0, N'8 phần: Multiple-choice cloze, Open cloze, Word formation, Key word transformation, Gapped text, Multiple matching.'),
    (10, N'Paper 2: Writing', 2, 'Viet', 'TuLuan', 2, 5.0, 90, 0, 0, N'Part 1: Viết bài luận học thuật dựa trên văn bản đọc (220-260 từ); Part 2: Viết thư, proposal hoặc review.'),
    (10, N'Paper 3: Listening', 3, 'Nghe', 'TracNghiem,DienKhuyet', 30, 0.33, 40, 0, 1, N'4 phần nghe: Chiết đoạn ngắn, bài nói điền thông tin chi tiết, phỏng vấn và ghép đa người nói.'),
    (10, N'Paper 4: Speaking', 4, 'Noi', 'TuLuan', 4, 2.5, 15, 0, 0, N'4 phần nói: Trao đổi mở rộng, so sánh phân tích ảnh, thảo luận hợp tác giải pháp và tranh luận quan điểm.'),

    -- MaCauTruc = 11: Cambridge Pre A1 Starters (YLE) (3 Papers)
    (11, N'Paper 1: Listening Comprehension', 1, 'Nghe', 'TracNghiem,NoiCap', 15, 0.33, 20, 0, 1, N'Nhận diện tranh vẽ, nối tên nhân vật và lắng nghe các chỉ dẫn đơn giản trong lớp học.'),
    (11, N'Paper 2: Reading & Writing Foundation', 2, 'Doc', 'TracNghiem,DienKhuyet', 15, 0.33, 20, 1, 0, N'Nhìn tranh đọc câu đúng/sai, sắp xếp chữ cái thành từ vựng và điền từ vào chỗ trống.'),
    (11, N'Paper 3: Speaking Basic Exchange', 3, 'Noi', 'TuLuan', 5, 2.0, 5, 0, 0, N'Chỉ đồ vật trong tranh, trả lời câu hỏi cá nhân ngắn gọn về tên, tuổi, sở thích.'),

    -- MaCauTruc = 12: Cambridge A1 Movers (YLE) (3 Papers)
    (12, N'Paper 1: Listening Comprehension', 1, 'Nghe', 'TracNghiem,DienKhuyet', 15, 0.28, 25, 0, 1, N'Lắng nghe chi tiết các đoạn hội thoại ngắn, ghi chú tên gọi, số lượng và tô màu đồ vật.'),
    (12, N'Paper 2: Reading & Writing Skills', 2, 'Doc', 'TracNghiem,DienKhuyet', 20, 0.28, 30, 1, 0, N'Đọc hiểu mẩu truyện thiếu nhi, chọn đáp án đúng và hoàn thành các câu mô tả ngắn.'),
    (12, N'Paper 3: Speaking Interaction', 3, 'Noi', 'TuLuan', 5, 2.0, 7, 0, 0, N'Tìm điểm khác biệt giữa 2 bức tranh, kể lại câu chuyện tranh và trả lời câu hỏi đời sống.'),

    -- MaCauTruc = 13: Cambridge A2 Flyers (YLE) (3 Papers)
    (13, N'Paper 1: Listening Comprehension', 1, 'Nghe', 'TracNghiem,DienKhuyet', 20, 0.25, 25, 0, 1, N'Lắng nghe thông tin chi tiết, nối sự kiện với nhân vật và ghi chép nhật ký ngắn.'),
    (13, N'Paper 2: Reading & Writing Advanced', 2, 'Doc', 'TracNghiem,DienKhuyet', 20, 0.25, 40, 1, 0, N'Ghép định nghĩa từ vựng, điền khuyết đoạn văn và viết một câu chuyện ngắn dựa trên 3 tranh gợi ý.'),
    (13, N'Paper 3: Speaking Comprehensive', 3, 'Noi', 'TuLuan', 5, 2.0, 9, 0, 0, N'Giải thích sự khác biệt giữa tranh ảnh, hỏi và trả lời thông tin dựa trên bảng dữ kiện gợi ý.'),

    -- MaCauTruc = 14: CEFR Adaptive Skills Practice (3 Chuyên đề)
    (14, N'Phần 1: Ngữ pháp cốt lõi & Biến đổi câu', 1, 'NguPhap', 'TracNghiem,VietLaiCau', 4, 1.0, 8, 0, 0, N'Rèn luyện nhanh các thì, câu bị động, điều kiện, đảo ngữ và liên từ thích ứng theo cấp độ.'),
    (14, N'Phần 2: Từ vựng chuyên đề & Collocations', 2, 'TuVung', 'TracNghiem', 3, 1.0, 6, 0, 0, N'Củng cố vốn từ học thuật, cụm từ cố định và thành ngữ thông dụng theo từng band điểm.'),
    (14, N'Phần 3: Đọc hiểu tăng tốc & Điền khuyết', 3, 'Doc', 'TracNghiem,DienKhuyet', 3, 1.0, 6, 1, 0, N'Luyện tập đọc lướt tìm ý chính (skimming) và quét dữ kiện chi tiết (scanning) đoạn văn ngắn.'),

    -- MaCauTruc = 15: Đề thi THPT Quốc Gia (5 Phần)
    (15, N'Phần 1: Ngữ âm & Trọng âm', 1, 'TuVung', 'TracNghiem', 4, 0.2, 5, 0, 0, N'Phát âm nguyên âm/phụ âm và trọng âm từ 2-3 âm tiết.'),
    (15, N'Phần 2: Ngữ pháp & Từ vựng căn bản đến nâng cao', 2, 'NguPhap', 'TracNghiem', 16, 0.2, 18, 0, 0, N'Thì động từ, câu bị động, điều kiện, mệnh đề quan hệ, từ loại và thành ngữ.'),
    (15, N'Phần 3: Chức năng giao tiếp & Từ đồng nghĩa - Trái nghĩa', 3, 'TuVung', 'TracNghiem', 6, 0.2, 7, 0, 0, N'Giao tiếp xã hội và tìm từ đồng nghĩa, trái nghĩa theo ngữ cảnh.'),
    (15, N'Phần 4: Đọc điền từ & Đọc hiểu 2 đoạn văn', 4, 'Doc', 'TracNghiem', 17, 0.2, 22, 1, 0, N'Đọc điền từ (Cloze test 5 câu) và 2 bài đọc hiểu phân tích chi tiết (12 câu).'),
    (15, N'Phần 5: Tìm lỗi sai & Biến đổi câu, Kết hợp câu', 5, 'Viet', 'TracNghiem,SuaLoiSai,VietLaiCau', 7, 0.2, 8, 0, 0, N'Xác định lỗi ngữ pháp và chọn câu đồng nghĩa, câu ghép chuẩn xác.');

GO

-- CHUẨN HÓA CÁC ĐỀ THI MẪU THEO ĐÚNG CẤU TRÚC EXAM BLUEPRINTS & NGÂN HÀNG CÂU HỎI
-- ====================================================================================


EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO

PRINT N'===================================================================';
PRINT N'NẠP THÀNH CÔNG DANH MỤC HỆ THỐNG & CẤU TRÚC ĐỀ THI (02_danhmuc_cautruc)!';
PRINT N' - 3 VaiTro, 5 TrinhDo, 5 CauHinhQuyDoiKhaoSat, 13 NguoiDung, 26 ChuDe';
PRINT N' - 15 CauTrucDeThi (IELTS, TOEIC, VSTEP, Cambridge, THPTQG) & 58 ChiTietCauTruc';
PRINT N'===================================================================';
GO
