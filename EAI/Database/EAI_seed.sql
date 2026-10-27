-- ===================================================================
-- Script: EAI_seed.sql (BẢN TỔNG HỢP TOÀN DIỆN CHO TOÀN BỘ HỆ THỐNG EAI)
-- Bao gồm đầy đủ dữ liệu: Vai trò, Trình độ, Người dùng, Hồ sơ học tập,
-- Cây danh mục chủ đề, Học liệu phong phú, Từ vựng, Flashcard,
-- Câu hỏi trắc nghiệm A1-C1, Khảo sát đầu vào, Đề thi thử và AI sinh đề.
-- Chạy trên SQL Server Management Studio (SSMS) với bảng mã UTF-8.
-- ===================================================================
USE TiengAnhAI;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- ===================================================================
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
DELETE FROM LuaChonDapAn;
DELETE FROM CauHoi;
DELETE FROM ChuDeNguPhap;
DELETE FROM Flashcard;
DELETE FROM TuVung;
DELETE FROM TienDoBaiHoc;
DELETE FROM BaiHoc;

-- Gỡ bỏ tham chiếu cha-con trước khi xóa bảng danh mục tự tham chiếu ChuDe
UPDATE ChuDe SET MaChuDeCha = NULL;
DELETE FROM ChuDe;

DELETE FROM HoSoHocTap;
DELETE FROM TepTinNhapLieu;
DELETE FROM NguoiDung;
DELETE FROM VaiTro;
DELETE FROM TrinhDo;
GO

-- Reset giá trị tự tăng IDENTITY về 0 để các bản ghi mẫu nhận đúng ID từ 1 trở đi
IF OBJECT_ID('KetQuaLamBaiBoCauHoi', 'U') IS NOT NULL DBCC CHECKIDENT ('KetQuaLamBaiBoCauHoi', RESEED, 0) WITH NO_INFOMSGS;
IF OBJECT_ID('BoCauHoi', 'U') IS NOT NULL DBCC CHECKIDENT ('BoCauHoi', RESEED, 0) WITH NO_INFOMSGS;
IF OBJECT_ID('NoiDungHocTapAIBaiHoc', 'U') IS NOT NULL DBCC CHECKIDENT ('NoiDungHocTapAIBaiHoc', RESEED, 0) WITH NO_INFOMSGS;
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
-- 5. NẠP CÂY DANH MỤC CHỦ ĐỀ PHÂN CẤP (ChuDe)
-- ===================================================================
-- Root 1: Tiếng Anh Giao Tiếp (MaChuDe = 1)
INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Tiếng Anh Giao Tiếp', N'Các chủ đề giao tiếp thực tế hàng ngày', 1);
DECLARE @rootGiaoTiep INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@rootGiaoTiep, N'Chào hỏi & Giới thiệu bản thân', N'Từ vựng và mẫu câu khi mới gặp mặt', 1),
(@rootGiaoTiep, N'Mua sắm & Nhà hàng ẩm thực', N'Mẫu câu gọi món, trả giá, thanh toán', 1),
(@rootGiaoTiep, N'Du lịch & Phương tiện giao thông', N'Hỏi đường, thủ tục sân bay, khách sạn', 1);

-- Root 2: Tiếng Anh Học Thuật (IELTS) (MaChuDe = 5)
INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Tiếng Anh Học Thuật (IELTS)', N'Luyện thi học thuật và các kỹ năng chuyên sâu', 1);
DECLARE @rootAcademic INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@rootAcademic, N'IELTS Reading — Skimming & Scanning', N'Kỹ năng đọc nhanh, tìm từ khóa', 1),
(@rootAcademic, N'IELTS Writing Task 2 — Luận xã hội', N'Cấu trúc bài luận 4 đoạn chuẩn học thuật', 1),
(@rootAcademic, N'IELTS Listening — Section 3 & 4', N'Hội thoại học thuật và bài giảng đại học', 1);

-- Root 3: Ngữ Pháp Cốt Lõi (MaChuDe = 9)
INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Ngữ Pháp Cốt Lõi', N'Hệ thống ngữ pháp tiếng Anh từ căn bản đến nâng cao', 1);
DECLARE @rootGrammar INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@rootGrammar, N'Các thì trong tiếng Anh (Tenses)', N'12 thì và ứng dụng thực tiễn', 1),
(@rootGrammar, N'Mệnh đề quan hệ & Câu bị động', N'Ngữ pháp ghép câu và diễn đạt tự nhiên', 1),
(@rootGrammar, N'Câu điều kiện & Đảo ngữ', N'Ngữ pháp nâng cao cho band điểm cao', 1);

-- Root 4: Tiếng Anh Thương Mại & Công Sở (MaChuDe = 13)
INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Tiếng Anh Thương Mại & Công Sở', N'Tiếng Anh doanh nghiệp, đàm phán, email', 1);
DECLARE @rootBusiness INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@rootBusiness, N'Viết Email & Thư tín Thương mại', N'Quy chuẩn viết email chuyên nghiệp', 1),
(@rootBusiness, N'Đàm phán & Thuyết trình Dự án', N'Ngôn ngữ đàm phán và thuyết phục đối tác', 1);

-- Root 5: Tiếng Anh Công Nghệ Thông Tin (IT English)
INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Tiếng Anh Công Nghệ Thông Tin (IT English)', N'Tiếng Anh chuyên ngành phần mềm, Agile và AI', 1);
DECLARE @rootIT INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@rootIT, N'Thuật ngữ Lập trình & Kỹ thuật Phần mềm', N'Microservices, ORM, Refactoring và CI/CD', 1),
(@rootIT, N'Giao tiếp Agile, Daily Scrum & Phỏng vấn IT', N'Kỹ năng họp Daily Scrum và phỏng vấn kỹ thuật', 1);
GO

-- ===================================================================
-- 6. NẠP BẢNG BÀI HỌC PHONG PHÚ (BaiHoc)
-- ===================================================================
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDangNhap = N'tung.hoang');
IF @gvId IS NULL SET @gvId = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = 1;

DECLARE @tdA1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A1');
DECLARE @tdA2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A2');
DECLARE @tdB1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B1');
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');
DECLARE @tdC1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'C1');

-- Lấy mã các chủ đề con
DECLARE @cdChaoHoi INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Chào hỏi%');
DECLARE @cdMuaSam INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Mua sắm%');
DECLARE @cdDuLich INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Du lịch%');
DECLARE @cdReading INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS Reading%');
DECLARE @cdWriting INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS Writing%');
DECLARE @cdListening INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS Listening%');
DECLARE @cdTenses INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Các thì%');
DECLARE @cdMenhDe INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Mệnh đề quan hệ%');
DECLARE @cdDieuKien INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Câu điều kiện%');
DECLARE @cdEmail INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Viết Email%');
DECLARE @cdDamPhan INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Đàm phán%');
DECLARE @cdThuatNguIT INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Thuật ngữ Lập trình%');
DECLARE @cdAgile INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Giao tiếp Agile%');

INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES
-- Chào hỏi
(@cdChaoHoi, @tdA1, @gvId, N'Giao tiếp cơ bản: Làm quen và giới thiệu bản thân', N'Nghe', 
 N'Học cách chào hỏi tự nhiên, giới thiệu tên tuổi, nghề nghiệp và sở thích bằng tiếng Anh chuẩn giọng bản xứ.', 
 N'<div class=''lesson-content''><h2>Nghệ Thuật Giao Tiếp Cơ Bản: Làm Quen & Tự Giới Thiệu Bản Thân (Mastering Introductions & Small Talk)</h2><p class=''lead''>Kỹ năng giới thiệu bản thân tự tin, tự nhiên là chìa khóa vàng giúp tạo ấn tượng ban đầu (first impression) xuất sắc trong môi trường học thuật, công sở và giao lưu quốc tế.</p><h3>1. Các mẫu câu chào hỏi theo ngữ cảnh (Contextual Greetings & Responses)</h3><ul><li><strong>Chào hỏi theo khung giờ chuẩn mực:</strong> "Good morning" (trước 12h trưa), "Good afternoon" (12h - 18h), "Good evening" (sau 18h).</li><li><strong>Chào hỏi thân thiện, tạo sự kết nối:</strong> "Hello! It is a pleasure to meet you", "Hi there, how are you doing today?", "How has your day been so far?".</li><li><strong>Phản hồi tự nhiên & lịch sự:</strong> "I am doing great, thank you for asking!", "Could not be better, how about yourself?", "It is wonderful to meet you too."</li></ul><h3>2. Khung cấu trúc 4 bước giới thiệu bản thân hoàn chỉnh (4-Step Self-Introduction Framework)</h3><ul><li><strong>Bước 1 - Họ tên & Danh xưng thân mật:</strong> "My full name is Nguyen Van An, but please feel free to call me John."</li><li><strong>Bước 2 - Quê hương & Nơi cư trú hiện tại:</strong> "I was born and raised in Da Nang, and I am currently living and studying in Ho Chi Minh City."</li><li><strong>Bước 3 - Nghề nghiệp & Chuyên môn:</strong> "I am currently working as a junior frontend software engineer, with a keen focus on modern web interfaces."</li><li><strong>Bước 4 - Sở thích & Hoạt động ngoại khóa:</strong> "Outside of work, I have a strong passion for landscape photography, playing the guitar, and reading technology magazines."</li></ul><h3>3. Đoạn hội thoại mẫu hoàn chỉnh trong ngữ cảnh thực tế (Model Conversational Dialogue)</h3><div class=''dialogue-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''><strong>David (Senior Project Manager):</strong> "Good morning! Welcome to our international technology exchange workshop. My name is David Miller from the Sydney office. Are you settling in well?"<br/><br/><strong>An (Software Engineer):</strong> "Good morning Mr. Miller! It is an absolute pleasure to meet you. My name is An Nguyen, but you can call me John. I am a software engineer based in the Ho Chi Minh City tech hub. Everything has been well-organized this morning, thank you!"<br/><br/><strong>David:</strong> "Great to have you here, John! Is this your first time collaborating directly with the overseas engineering division?"<br/><br/><strong>An:</strong> "Yes, it is my very first in-person event with the global team. I am particularly excited about our upcoming cross-platform integration session and eager to exchange technical practices with your team."<br/><br/><strong>David:</strong> "That sounds wonderful! We have a dedicated technical discussion scheduled right after lunch. Let us grab a coffee during the break and discuss your team''s workflow."<br/><br/><strong>An:</strong> "I would love that, Mr. Miller. Thank you very much, and I look forward to speaking with you then!"</div></div>', 15, 1),

(@cdChaoHoi, @tdA2, @gvId, N'Hội thoại Chào hỏi Trang trọng vs. Thân mật (Formal & Informal)', N'TuVung',
 N'Phân biệt cách chào hỏi trong môi trường học thuật/công sở với bạn bè hàng ngày.',
 N'<div class=''lesson-content''><h2>Phân Biệt Nghệ Thuật Chào Hỏi Trang Trọng vs. Thân Mật (Formal vs. Informal Greetings Mastery)</h2><p class=''lead''>Sử dụng đúng văn phong (linguistic register) giúp bạn thể hiện sự tinh tế, chuyên nghiệp và tránh các hiểu lầm văn hóa đáng tiếc trong cả môi trường công sở lẫn giao tiếp đời thường.</p><h3>1. Quy chuẩn chào hỏi trang trọng (Formal Register - Business & Academic)</h3><ul><li><strong>Đối tượng áp dụng:</strong> Khách hàng, đối tác cấp cao, giáo sư, người lớn tuổi hoặc người lần đầu gặp mặt trong bối cảnh công vụ.</li><li><strong>Lời chào trang trọng:</strong> "Good morning / afternoon / evening, Mr./Ms. [Last Name]", "How do you do?" (câu chào nghi thức, đáp lại bằng "How do you do?").</li><li><strong>Bày tỏ sự vinh hạnh:</strong> "It is an honor to make your acquaintance", "Allow me to introduce myself; my name is...".</li><li><strong>Hỏi thăm sức khỏe lịch thiệp:</strong> "I hope you are having a productive week", "How has your journey been?".</li></ul><h3>2. Chào hỏi thân mật & tự nhiên (Informal Register - Friends & Close Peers)</h3><ul><li><strong>Đối tượng áp dụng:</strong> Bạn bè cùng trang lứa, đồng nghiệp thân thiết cùng nhóm dự án, người quen lâu năm.</li><li><strong>Mẫu câu chào nhanh:</strong> "Hey, what is up?", "How is it going?", "Long time no see!", "Good to see you again!".</li><li><strong>Đáp lại súc tích:</strong> "Not much, just the usual!", "Pretty good, how about you?", "Can not complain, surviving the week!".</li></ul><h3>3. Bảng đối chiếu tình huống thực tế (Comparative Dialogue Samples)</h3><div class=''dialogue-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''><strong>Tình huống A (Formal - Cuộc họp ban giám đốc):</strong><br/>• <em>Director:</em> "Good morning, Ms. Jenkins. I trust you had a pleasant flight from Tokyo."<br/>• <em>Ms. Jenkins:</em> "Good morning, Director Thomas. Yes, indeed. Thank you very much for your warm hospitality and for arranging the conference schedule so smoothly."<br/><br/><strong>Tình huống B (Informal - Quầy cà phê văn phòng):</strong><br/>• <em>Mark:</em> "Morning Sarah! Coffee rush again? How was your weekend?"<br/>• <em>Sarah:</em> "Hey Mark! Pretty relaxing, thanks. Went hiking with my dog. How about yours? Still working on that sprint release?"</div></div>', 15, 1),

(@cdChaoHoi, @tdB1, @gvId, N'Tự tin giới thiệu Nghề nghiệp, Quê hương và Đam mê cá nhân', N'Doc',
 N'Mẫu câu và cấu trúc đoạn văn ngắn giúp bạn tự tin tỏa sáng khi được hỏi "Tell me about yourself".',
 N'<div class=''lesson-content''><h2>Chiến Lược Tự Tin Giới Thiệu Bản Thân Chuyên Sâu (Elevator Pitch & Personal Storytelling)</h2><p class=''lead''>Làm chủ phương pháp Elevator Pitch (thuyết trình ngắn 60 giây) giúp bạn biến câu hỏi kinh điển ''Tell me about yourself'' thành cơ hội tỏa sáng rực rỡ trước nhà tuyển dụng và đối tác.</p><h3>1. Công thức 3 thì quá khứ - hiện tại - tương lai (Past - Present - Future Model)</h3><ul><li><strong>Present (Hiện tại - Chuyên môn & Trọng trách):</strong> Mở đầu bằng vị trí công tác hiện tại, những thế mạnh cốt lõi và giá trị chuyên biệt bạn đóng góp cho tổ chức.</li><li><strong>Past (Quá khứ - Nền tảng & Thành tựu):</strong> Nhắc ngắn gọn về bằng cấp chuyên ngành, 1-2 dự án tiêu biểu hoặc thử thách thực tế đã vượt qua thành công.</li><li><strong>Future (Tương lai - Tầm nhìn & Mục tiêu):</strong> Nêu rõ định hướng nghề nghiệp, lý do bạn hào hứng với cơ hội hiện tại và mong muốn tạo ra đột phá mới.</li></ul><h3>2. Bộ cấu trúc câu nâng tầm bài nói (Lexical Toolkit)</h3><ul><li><strong>Miêu tả chuyên môn:</strong> "I specialize in developing scalable cloud-native architectures that streamline user workflows."</li><li><strong>Miêu tả quê hương:</strong> "I hail from Da Nang, a dynamic coastal city renowned for its innovation, vibrant culinary culture, and booming tech ecosystem."</li><li><strong>Bày tỏ đam mê:</strong> "I am deeply passionate about mentoring young engineers and contributing to open-source software repositories."</li></ul><h3>3. Bài thuyết trình mẫu hoàn chỉnh (Model 60-Second Elevator Pitch)</h3><div class=''dialogue-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''>"Good morning, members of the evaluation panel. My name is Tran Minh, and I am a software engineer with over four years of specialized experience in building high-throughput financial microservices.<br/><br/>In my current role at VinaTech Solutions, I spearheaded the complete refactoring of our core payment gateway, which successfully reduced transaction failure rates by 35% and scaled our daily capacity to over two million active requests. Prior to this, I completed my degree in Computer Science at Hanoi University of Science and Technology, where I conducted research on automated security verification.<br/><br/>Beyond my technical work, I grew up in Hai Phong, a historic port city that instilled in me a spirit of relentless resilience and adaptability. In my personal time, I actively write technical tutorials on system design and contribute to developer communities across Southeast Asia.<br/><br/>I am eager to bring my architectural expertise and problem-solving mindset to your pioneering engineering team today. Thank you very much for having me!"</div></div>', 20, 1),

-- Mua sắm & Nhà hàng
(@cdMuaSam, @tdA2, @gvId, N'Đặt bàn và Gọi món tại Nhà hàng Quốc tế (Dining Out & Ordering Food)', N'Nghe',
 N'Học các mẫu câu gọi món, hỏi thực đơn món chay, yêu cầu hóa đơn và thanh toán tiền tip.',
 N'<div class=''lesson-content''><h2>Cẩm Nang Tiếng Anh Nhà Hàng: Đặt Bàn, Gọi Món & Thanh Toán (International Dining Out Mastery)</h2><p class=''lead''>Trang bị trọn bộ mẫu câu giao tiếp tự tin tại nhà hàng quốc tế, từ việc đặt chỗ trước qua điện thoại, yêu cầu món ăn theo chế độ ăn uống riêng biệt cho đến thanh toán hóa đơn văn minh.</p><h3>1. Các giai đoạn phục vụ cốt lõi tại nhà hàng (Standard Dining Workflow)</h3><ul><li><strong>Giai đoạn 1 - Đặt bàn trước (Making a Reservation):</strong> "I would like to book a table for four under the name of Nguyen for tonight at 7:30 PM, preferably by the window."</li><li><strong>Giai đoạn 2 - Gọi món & Yêu cầu thực đơn (Ordering Food & Inquiring Menu):</strong> "Could you walk us through today''s chef specials?", "Could we start with the mushroom bruschetta?", "Does this creamy soup contain dairy products?".</li><li><strong>Giai đoạn 3 - Điều chỉnh món theo khẩu vị:</strong> "Could I have the steak medium-rare, with the peppercorn sauce served on the side, please?"</li><li><strong>Giai đoạn 4 - Yêu cầu hóa đơn & Thanh toán (Paying the Bill):</strong> "Could we get the check, please?", "Do you accept credit card payments?", "Could we split the bill evenly?".</li></ul><h3>2. Hội thoại mẫu hoàn chỉnh tại nhà hàng Âu sang trọng (Fine Dining Model Dialogue)</h3><div class=''dialogue-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''><strong>Server:</strong> "Good evening, ladies and gentlemen! Welcome to The Olive Tree Bistro. Here are your menus. May I start you off with something refreshing to drink while you review the appetizers?"<br/><br/><strong>Customer (Linda):</strong> "Good evening! Yes, please. Could we have a bottle of sparkling mineral water with lime slices for the table?"<br/><br/><strong>Server:</strong> "Certainly, ma''am. I will bring that right away... Here you are. Are you ready to order your entrees, or would you prefer a few more minutes?"<br/><br/><strong>Customer (Linda):</strong> "We are ready, thank you. To start, we would love the grilled calamari and the Caesar salad. For our main courses, my colleague would like the pan-seared salmon with asparagus, and I will have the ribeye steak."<br/><br/><strong>Server:</strong> "An excellent selection! How would you prefer your ribeye steak prepared?"<br/><br/><strong>Customer (Linda):</strong> "Medium-rare, please. And could we have the garlic butter sauce served in a separate cup on the side?"<br/><br/><strong>Server:</strong> "Of course, noted with thanks. Your starters will be served in approximately ten minutes. Please enjoy your evening!"</div></div>', 20, 1),

(@cdMuaSam, @tdA1, @gvId, N'Kỹ năng Mua sắm, Hỏi kích cỡ và Đổi trả hàng hóa (Shopping & Returns)', N'TuVung',
 N'Bộ từ vựng phong phú về quần áo, phụ kiện, chính sách hoàn tiền và thẻ khách hàng thân thiết.',
 N'<div class=''lesson-content''><h2>Tiếng Anh Mua Sắm: Chọn Size, Thử Đồ & Chính Sách Đổi Trả (Smart Shopping & Retail Expressions)</h2><p class=''lead''>Nắm bắt hệ thống từ vựng và mẫu câu xử lý linh hoạt mọi tình huống khi mua sắm tại các trung tâm thương mại quốc tế, cửa hàng thời trang và quầy chăm sóc khách hàng.</p><h3>1. Từ vựng & Mẫu câu hỏi kích cỡ, màu sắc, phòng thử đồ</h3><ul><li><strong>Kích cỡ & Độ vừa vặn (Sizes & Fit):</strong> Extra Small (XS), Small (S), Medium (M), Large (L), Extra Large (XL). "Do you happen to have this navy blazer in a size Medium?", "This jacket feels slightly tight around the shoulders."</li><li><strong>Phòng thử đồ (Fitting Room):</strong> "Excuse me, where are the fitting rooms located?", "May I take these three items into the fitting room?".</li><li><strong>Khuyến mãi & Giảm giá (Discounts & Offers):</strong> "Is this collection currently on sale?", "Is there any buy-one-get-one promotion applicable today?".</li></ul><h3>2. Quy trình đổi trả & yêu cầu hoàn tiền (Returns & Refund Protocols)</h3><ul><li><strong>Lý do đổi hàng:</strong> "The zipper is defective", "The fabric shrank after the initial wash", "It is the wrong size for my brother."</li><li><strong>Chính sách hoàn tiền:</strong> "I would like to request a full refund to my original payment method", "Do I need to present the original sales receipt?".</li><li><strong>Quy định bảo hành / Thẻ quà tặng:</strong> "We offer a 30-day exchange window with the tags intact", "Store credit will be credited to your membership account."</li></ul><h3>3. Hội thoại mẫu hoàn chỉnh tại quầy chăm sóc khách hàng (Customer Service Return Dialogue)</h3><div class=''dialogue-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''><strong>Store Associate:</strong> "Hello, welcome to Nordstrom Customer Care. How may I assist you today?"<br/><br/><strong>Customer (Emily):</strong> "Hi there. I purchased this beige wool coat online three days ago, but unfortunately, it is one size too large for me. I would like to exchange it for a size Small if you have it in stock."<br/><br/><strong>Store Associate:</strong> "I would be happy to check that for you right away. Do you have your order confirmation number or the physical paper receipt?"<br/><br/><strong>Customer (Emily):</strong> "Yes, certainly. Here is the digital receipt on my phone, and all the original garment tags are still securely attached."<br/><br/><strong>Store Associate:</strong> "Thank you very much. Let me verify our inventory... Good news! We have the exact coat in size Small in our stockroom. Would you like to try it on in the fitting room to verify the fit?"<br/><br/><strong>Customer (Emily):</strong> "That would be wonderful! Thank you so much for your efficient and courteous assistance."</div></div>', 15, 1),

(@cdMuaSam, @tdB1, @gvId, N'Văn hóa Ẩm thực Đường phố và Miêu tả Hương vị Món ăn', N'Doc',
 N'Bài đọc hiểu về văn hóa ẩm thực Á - Âu cùng cách dùng tính từ miêu tả vị giác tinh tế.',
 N'<div class=''lesson-content''><h2>Văn Hóa Ẩm Thực Đường Phố Toàn Cầu & Nghệ Thuật Miêu Tả Vị Giác (Street Food Culture & Sensory Descriptors)</h2><p class=''lead''>Khám phá chiều sâu văn hóa ẩm thực thông qua bài đọc chuyên sâu và làm chủ hệ thống tính từ miêu tả vị giác tinh tế, phục vụ các bài thi IELTS Reading và Speaking Band 7.5+.</p><h3>1. Hệ thống tính từ vị giác & kết cấu thực phẩm nâng cao (Sensory Lexicon)</h3><ul><li><strong>Hương vị (Taste & Flavor):</strong> Savory (đậm đà, vừa miệng), Tangy (chua thanh sảng khoái), Succulent (mọng nước, thơm mềm), Zesty (cay nồng vị vỏ cam chanh), Umami (vị ngọt thịt tự nhiên sâu lắng), Fragrant (thơm nức mũi).</li><li><strong>Kết cấu (Texture):</strong> Crispy (giòn rụm bên ngoài), Crunchy (giòn sần sật), Tender (mềm mịn tan trong miệng), Velvety (mịn như nhung), Chewy (dẻo dai vừa phải).</li><li><strong>Đặc tính ẩm thực:</strong> Decadent (béo ngậy xa hoa), Authentic (nguyên bản truyền thống), Street-side delicacy (món ngon hè phố).</li></ul><h3>2. Bài đọc hiểu chuyên sâu: Giao thoa ẩm thực đường phố Bangkok & Paris (Comparative Reading Passage)</h3><div class=''reading-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''>Street food is far more than a convenient dining option; it serves as a dynamic living mirror reflecting the sociological heritage and communal spirit of urban centers worldwide.<br/><br/>In the bustling labyrinth of Bangkok''s Yaowarat Road, open-air hawker stalls come alive as the sun dips below the horizon. The ambient air is saturated with the heady aroma of crushed lemongrass, charred galangal, and searing wok smoke. Here, artisans who have perfected their signature Pad Thai over three generations execute culinary choreography: tossing hand-made rice noodles with succulent river prawns, crushed peanuts, and a tangy tamarind reduction. The resulting harmony of sweet, sour, salty, and spicy sensations exemplifies Thailand''s profound appreciation for multi-dimensional balance.<br/><br/>In striking contrast, the historic covered markets of Paris, such as Marché des Enfants Rouges, offer an understated yet equally vibrant gastronomic immersion. Strollers are greeted by artisanal crêperies crafting paper-thin buckwheat galettes filled with melted Gruyère cheese, savory smoked ham, and a velvety sunny-side-up organic egg. The Parisian street food ethos places supreme reverence upon pristine ingredient provenance and time-honored artisanal mastery. Despite their geographical and stylistic divergences, both metropolises celebrate street food as an egalitarian bridge that unites disparate social strata through the universal language of shared flavor.</div></div>', 25, 1),

-- Du lịch & Giao thông
(@cdDuLich, @tdA2, @gvId, N'Tiếng Anh Sân bay: Check-in, Cân hành lý và Qua cửa An ninh', N'Doc',
 N'Các đoạn hội thoại chuẩn tại quầy vé, khu vực kiểm tra hành lý và cửa ra máy bay (Boarding gate).',
 N'<div class=''lesson-content''><h2>Cẩm Nang Tiếng Anh Hàng Không: Check-in, Cân Hành Lý & Cửa An Ninh (Airport Navigation & Flight Protocol)</h2><p class=''lead''>Trang bị đầy đủ thuật ngữ hàng không và mẫu câu phản xạ nhanh giúp bạn tự tin làm thủ tục xuất nhập cảnh, cân hành lý ký gửi và qua cửa an ninh suôn sẻ tại các sân bay quốc tế.</p><h3>1. Từ vựng chuyên ngành hàng không cốt lõi (Aviation & Terminal Vocabulary)</h3><ul><li><strong>Thủ tục quầy vé:</strong> Boarding pass (Thẻ lên tàu bay), Checked baggage (Hành lý ký gửi), Carry-on baggage (Hành lý xách tay), Excess baggage fee (Phí hành lý quá cân), Baggage allowance (Định mức hành lý miễn cước).</li><li><strong>Khu vực sân bay:</strong> Security checkpoint (Cổng soi chiếu an ninh), Customs declaration (Tờ khai hải quan), Immigration desk (Bàn kiểm tra xuất nhập cảnh), Departure gate (Cửa ra máy bay), Terminal (Nhà ga hành khách).</li><li><strong>Thuật ngữ chuyến bay:</strong> Layover / Transit (Điểm dừng quá cảnh), Delayed (Bị hoãn), Boarding announcement (Loa thông báo lên máy bay).</li></ul><h3>2. Mẫu câu ứng thoại tại cổng kiểm tra an ninh (Security Protocol Expressions)</h3><ul><li>"Please place your laptop, tablet, and any liquids in a separate plastic bin." (Vui lòng đặt máy tính và chất lỏng ra khay riêng).</li><li>"Do I need to take off my shoes and belt?" (Tôi có cần tháo giày và thắt lưng không?).</li><li>"Ensure all liquid containers are under 100 milliliters and sealed in a clear transparent bag." (Đảm bảo mỗi bình chất lỏng dưới 100ml).</li></ul><h3>3. Hội thoại mẫu hoàn chỉnh tại quầy Check-in quốc tế (Airport Check-in Model Dialogue)</h3><div class=''dialogue-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''><strong>Agent:</strong> "Good afternoon! Welcome to Singapore Airlines. May I have your passport and electronic ticket reference, please?"<br/><br/><strong>Passenger (David):</strong> "Good afternoon. Here is my passport and my booking confirmation for flight SQ178 to London Heathrow."<br/><br/><strong>Agent:</strong> "Thank you, Mr. David. Are you checking any luggage today, or do you have carry-on bags only?"<br/><br/><strong>Passenger (David):</strong> "I have one large suitcase to check in, and this backpack as my carry-on item."<br/><br/><strong>Agent:</strong> "Please place your suitcase onto the luggage scale... It weighs exactly 21.5 kilograms, well within your 30-kilogram allowance. Would you prefer a window seat or an aisle seat for this long-haul flight?"<br/><br/><strong>Passenger (David):</strong> "I would greatly appreciate an aisle seat near the front of the cabin if one is available."<br/><br/><strong>Agent:</strong> "You are all set in seat 18C. Here is your boarding pass and luggage claim tag. Boarding commences at 14:15 at Gate B7. Please proceed directly through security screening. Have a delightful flight!"</div></div>', 25, 1),

(@cdDuLich, @tdA1, @gvId, N'Hỏi đường và Sử dụng Tàu điện ngầm (Metro & City Navigation)', N'TuVung',
 N'Chỉ dẫn phương hướng: rẽ trái, ngã tư, đi thẳng và mua vé tàu điện ngầm tự động.',
 N'<div class=''lesson-content''><h2>Chỉ Dẫn Đường Phố & Sử Dụng Hệ Thống Tàu Điện Ngầm (Urban Navigation & Metro Transit Mastery)</h2><p class=''lead''>Nắm vững kỹ năng hỏi đường chuẩn xác và tự tin làm chủ mạng lưới giao thông công cộng, metro, xe buýt tại các đại đô thị phát triển như Tokyo, London hay New York.</p><h3>1. Các cụm từ chỉ hướng & Mốc định vị không gian (Spatial Directions & Landmarks)</h3><ul><li><strong>Chỉ hướng vận động:</strong> "Turn left at the intersection", "Take the second right turn", "Head straight for two blocks", "Cross over the pedestrian crossing".</li><li><strong>Vị trí tương quan:</strong> "It is directly opposite the central library", "Adjacent to the pharmacy", "Right around the corner from the museum", "At the junction between 5th Avenue and Broadway".</li><li><strong>Hỏi khoảng cách & thời gian:</strong> "How far is it on foot?", "Is it within walking distance or should I take a taxi?".</li></ul><h3>2. Thuật ngữ & Quy chuẩn sử dụng hệ thống Metro / Subway</h3><ul><li><strong>Các khái niệm cơ bản:</strong> Transit card (Thẻ đi tàu tích hợp), Ticket vending machine (Máy bán vé tự động), Platform (Sân ga chờ tàu), Transfer station (Ga chuyển tuyến).</li><li><strong>Hướng di chuyển:</strong> Inbound / Outbound (Hướng vào trung tâm / Hướng ra ngoại ô), Northbound / Southbound (Hướng về phía Bắc / Nam).</li><li><strong>Thông báo thông dụng:</strong> "Stand clear of the closing doors", "Mind the gap between the train and the platform", "Change here for the Jubilee Line".</li></ul><h3>3. Hội thoại mẫu hoàn chỉnh tại ga tàu điện ngầm (Metro Station Model Dialogue)</h3><div class=''dialogue-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''><strong>Tourist (Ken):</strong> "Excuse me, officer. I am trying to get to the British Museum. Could you please show me which train line I need to take?"<br/><br/><strong>Station Officer:</strong> "Certainly! You are currently at King''s Cross St. Pancras. You will want to take the Piccadilly Line southbound toward Piccadilly Circus."<br/><br/><strong>Tourist (Ken):</strong> "Do I need to transfer to another line, or is it a direct connection?"<br/><br/><strong>Station Officer:</strong> "It is very straightforward: ride three stops on the Piccadilly Line and get off at Russell Square Station. Once you exit through the ticket barrier, turn left on Southampton Row and walk straight for about five minutes. The museum entrance will be right on your left."<br/><br/><strong>Tourist (Ken):</strong> "Can I simply tap my contactless credit card at the automated turnstile, or do I need to purchase a physical paper ticket?"<br/><br/><strong>Station Officer:</strong> "Contactless cards and Apple Pay are accepted at all turnstiles and automatically calculate the cheapest daily fare cap."<br/><br/><strong>Tourist (Ken):</strong> "Brilliant! Thank you ever so much for your clear directions!"</div></div>', 15, 1),

-- IELTS Reading
(@cdReading, @tdB2, @gvId, N'Chiến lược Skimming & Scanning bài đọc IELTS Academic chuyên sâu', N'Doc',
 N'Phương pháp đọc lướt nắm ý chính và quét từ khóa giúp tiết kiệm 40% thời gian làm bài Reading.',
 N'<div class=''lesson-content''><h2>Chiến Lược Tối Ưu Hóa Kỹ Năng Skimming & Scanning Trong IELTS Academic Reading (Band 7.5+ Strategies)</h2><p class=''lead''>Làm chủ phương pháp đọc nhanh khoa học giúp thí sinh kiểm soát bài đọc dài 2.700 từ trong vòng 60 phút mà vẫn đảm bảo độ chính xác tuyệt đối ở các câu hỏi học thuật khó.</p><h3>1. Kỹ thuật Skimming (Đọc lướt định hình kiến trúc văn bản - 90 giây/bài)</h3><ul><li><strong>Mục tiêu Skimming:</strong> Nắm bắt luận điểm tổng quát (gist), thái độ tác giả (stance) và cấu trúc triển khai của từng đoạn văn mà không sa đà vào từ vựng chi tiết.</li><li><strong>Quy tắc 3 điểm vàng:</strong> Đọc kỹ Title & Sub-title -> Đọc câu chủ đề (Topic Sentence) đầu đoạn -> Đọc câu kết luận (Concluding Sentence) cuối đoạn.</li><li><strong>Nhận diện từ nối logic (Discourse Markers):</strong> Đánh dấu các mốc chuyển ý: <em>However, Consequently, Furthermore, In spite of, By contrast</em> để nắm nhanh bước chuyển mạch văn.</li></ul><h3>2. Kỹ thuật Scanning (Quét radar định vị thông tin vi mô - 30 giây/câu hỏi)</h3><ul><li><strong>Dấu vết dữ liệu cứng (Unchangeable Anchors):</strong> Quét nhanh tên riêng in hoa (Proper nouns), số liệu thống kê (Percentages, Years), thuật ngữ khoa học in nghiêng hoặc trong ngoặc kép.</li><li><strong>Kỹ thuật di chuyển mắt zíc-zắc (Z-pattern / Snake Scan):</strong> Di chuyển ánh mắt từ góc phải sang góc trái theo hình chữ Z, không đọc thầm trong đầu (sub-vocalization) để gia tăng tốc độ quét lên 350+ từ/phút.</li><li><strong>Bản đồ Paraphrasing:</strong> Luôn chuẩn bị tinh thần tìm từ đồng nghĩa. Ví dụ: đề bài hỏi "financial constraints", bài đọc sẽ dùng "budgetary limitations" hoặc "fiscal deficit".</li></ul><h3>3. Bài đọc thực hành mẫu kèm phân tích Skim & Scan (Practical Case Analysis)</h3><div class=''reading-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''><strong>Paragraph A:</strong> Over the past two decades, urban climatologists have documented an accelerating phenomenon known as the Urban Heat Island (UHI) effect. Densely built metropolises replace natural vegetation with impermeable concrete and dark asphalt surfaces, which absorb short-wave solar radiation during daylight hours and re-radiate thermal energy throughout the evening. Consequently, nocturnal ambient temperatures in city centers frequently exceed surrounding rural baselines by 4.5 to 7.0 degrees Celsius.<br/><br/><strong>Paragraph B:</strong> In response to mounting public health concerns and soaring air conditioning electricity expenditures, municipal authorities in Stuttgart and Tokyo have pioneered vertical greening initiatives. By mandating intensive rooftop gardens and vegetative facades on all newly constructed commercial edifices exceeding thirty meters in height, these urban centers have observed a measurable 15% reduction in localized surface heat absorption.<br/><br/><strong>Chiến thuật áp dụng:</strong><br/>• Khi gặp câu hỏi: <em>"What materials contribute significantly to nocturnal thermal radiation in cities?"</em><br/>• <strong>Scan:</strong> Dò tìm từ khóa "nocturnal", "radiation", "materials" ở Paragraph A.<br/>• <strong>Kết quả:</strong> Định vị chuẩn xác "impermeable concrete and dark asphalt surfaces" chỉ trong vòng 15 giây.</div></div>', 35, 1),

(@cdReading, @tdB2, @gvId, N'Tuyệt chiêu xử lý dạng bài True / False / Not Given & Yes / No / Not Given', N'Doc',
 N'Phân biệt rõ ràng giữa False (thông tin mâu thuẫn) và Not Given (không đủ căn cứ kết luận).',
 N'<div class=''lesson-content''><h2>Phương Pháp Làm Chủ Tuyệt Đối Dạng Bài True / False / Not Given & Yes / No / Not Given (IELTS Reading Band 8.0)</h2><p class=''lead''>Phân tích triệt để bản chất logic của từng đáp án và giải mã các bẫy tinh vi thường gặp, giúp biến dạng bài dễ mất điểm nhất thành thế mạnh tối đa của bạn.</p><h3>1. Bản chất logic tam giác chân lý (The Three-Way Truth Matrix)</h3><ul><li><strong>TRUE / YES:</strong> Mọi thông tin trong nhận định của đề bài đều trùng khớp 100% với hàm ý của bài đọc (kể cả khi được diễn đạt bằng từ đồng nghĩa khác hoàn toàn).</li><li><strong>FALSE / NO:</strong> Nhận định của đề bài có ít nhất một chi tiết mâu thuẫn trực tiếp hoặc phủ định hoàn toàn thông tin được khẳng định trong bài đọc (Fact A vs. Fact B đối lập).</li><li><strong>NOT GIVEN:</strong> Bài đọc hoàn toàn không nhắc đến thông tin, HOẶC bài đọc có nhắc đến đối tượng nhưng không cung cấp đủ căn cứ logic để xác nhận nhận định là đúng hay sai.</li></ul><h3>2. Các bẫy kinh điển cần cảnh giác cao độ (High-Risk Traps)</h3><ul><li><strong>Bẫy từ chỉ tần suất & phạm vi (Qualifiers Trap):</strong> Chú ý các từ hạn định tuyệt đối: <em>always, all, every, impossible</em> so với từ tương đối trong bài đọc: <em>often, many, feasible, occasionally</em>.</li><li><strong>Bẫy so sánh hơn giả tạo (Unsubstantiated Comparison):</strong> Đề bài viết "A is more efficient than B", trong khi bài đọc chỉ khen ngợi cả A và B hoạt động tốt mà không hề đưa ra phép so sánh trực tiếp nào -> <strong>NOT GIVEN</strong>.</li><li><strong>Bẫy suy đoán cảm tính (External Knowledge Bias):</strong> Tuyệt đối không dùng hiểu biết cá nhân bên ngoài xã hội để kết luận; chỉ dựa vào dữ kiện bằng chữ có trong đoạn văn.</li></ul><h3>3. Bài tập minh họa đối chiếu logic 3 trường hợp (Worked Case Study)</h3><div class=''reading-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''><strong>Văn bản gốc:</strong> "The newly designed electric turbine operates with an average mechanical efficiency of 88%, which exceeds earlier diesel prototypes. While European utility providers have welcomed the technological breakthrough, manufacturing expenses remain considerably high due to scarce neodymium magnets."<br/><br/><strong>Câu 1:</strong> <em>"The electric turbine demonstrates superior efficiency compared to older diesel models."</em><br/>➔ <strong>Đáp án: TRUE</strong> (Bởi vì "efficiency of 88% exceeds earlier diesel prototypes").<br/><br/><strong>Câu 2:</strong> <em>"The production costs of the new electric turbine are affordable for small-scale enterprises."</em><br/>➔ <strong>Đáp án: FALSE</strong> (Bài viết khẳng định: "manufacturing expenses remain considerably high").<br/><br/><strong>Câu 3:</strong> <em>"Asian utility corporations have invested heavily in importing these electric turbines."</em><br/>➔ <strong>Đáp án: NOT GIVEN</strong> (Bài viết chỉ nhắc tới "European utility providers", không hề đề cập đến chính sách nhập khẩu hay mức đầu tư của các công ty châu Á).</div></div>', 30, 1),

-- IELTS Writing
(@cdWriting, @tdB2, @gvId, N'Cấu trúc viết bài Opinion Essay (Agree or Disagree) chuẩn Band 7.5+', N'Viet',
 N'Bố cục 4 đoạn chuẩn mực: Introduction, 2 Body Paragraphs với luận cứ chặt chẽ và Conclusion sắc bén.',
 N'<div class=''lesson-content''><h2>Khung Cấu Trúc Viết Bài Opinion Essay (Agree / Disagree) Chuẩn Band 7.5+ (IELTS Writing Task 2)</h2><p class=''lead''>Xây dựng tư duy phản biện sắc bén và kỹ thuật tổ chức bài viết mạch lạc 4 đoạn, tối ưu hóa điểm số Task Response và Coherence & Cohesion theo thang điểm giám khảo quốc tế.</p><h3>1. Chiến lược chọn lập trường: Hoàn toàn đồng ý (Strong Stance) vs. Dung hòa (Balanced)</h3><ul><li><strong>Phương án 1 - Strong Opinion (Khuyến nghị cho Band 7.0+):</strong> Nghiêng 100% về một phía (Strongly Agree hoặc Strongly Disagree). Giúp luận điểm tập trung, không bị mâu thuẫn nội tại và tiết kiệm thời gian phân bổ ý.</li><li><strong>Phương án 2 - Balanced Opinion:</strong> Thừa nhận một phần tính hợp lý của quan điểm đối lập nhưng khẳng định lập trường chủ đạo vẫn vượt trội hơn.</li></ul><h3>2. Cấu trúc 4 đoạn chuẩn mực (Standard 4-Paragraph Blueprint)</h3><ul><li><strong>Introduction (40-50 từ):</strong> Sentence 1: Paraphrase lại đề bài bằng cấu trúc từ vựng mới -> Sentence 2: Thesis statement nêu rõ lập trường kiên định của tác giả.</li><li><strong>Body Paragraph 1 (80-95 từ):</strong> Topic sentence (Luận điểm 1) -> Explanation (Giải thích sâu nguyên nhân - hệ quả) -> Concrete Example (Dẫn chứng cụ thể, số liệu nghiên cứu hoặc case study thực tế).</li><li><strong>Body Paragraph 2 (80-95 từ):</strong> Topic sentence (Luận điểm 2) -> Elaborate analysis (Phân tích lợi ích sâu rộng hơn) -> Real-world impact & deduction.</li><li><strong>Conclusion (35-45 từ):</strong> Reiterate Thesis statement -> Summarize key arguments -> Final forward-looking takeaway.</li></ul><h3>3. Bài viết mẫu hoàn chỉnh Band 8.0 (Model Band 8.0 Opinion Essay)</h3><div class=''essay-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''><strong>Prompt:</strong> <em>Some people believe that universities should focus exclusively on providing graduates with practical skills for future employment, while others argue that the fundamental purpose of higher education should be the pursuit of pure knowledge. To what extent do you agree or disagree?</em><br/><br/>It is frequently argued that tertiary educational institutions should gear their curricula entirely toward pragmatic career readiness rather than academic inquiry for its own sake. While vocational preparedness is undeniably advantageous for immediate employability, I fundamentally disagree with the notion that higher education should restrict itself solely to job training.<br/><br/>To begin with, restricting university programs to narrow practical trades overlooks the indispensable value of critical thinking and philosophical foundation. Historically, ground-breaking societal innovations have originated from theoretical pursuits that possessed no immediate commercial utility at their inception. For instance, the theoretical mathematical principles underlying quantum mechanics developed a century ago laid the indispensable groundwork for modern supercomputing and semiconductor fabrication. Should universities abandon pure scientific exploration in pursuit of short-term vocational outcomes, the engine of foundational human innovation would inevitably stall.<br/><br/>Furthermore, the contemporary global employment landscape is defined by unprecedented technological volatility and automated dislocation. Specialized technical capabilities acquired today frequently become obsolete within a decade due to algorithmic advancement. Conversely, comprehensive academic disciplines—such as literature, theoretical sciences, and sociology—equip scholars with cognitive flexibility, qualitative analysis, and ethical reasoning. These transferable cognitive attributes empower graduates to adapt fluidly across multiple disruptive industry transformations throughout their professional lifespans.<br/><br/>In conclusion, while practical vocational training holds valid merit in vocational colleges, I firmly contend that universities must continue to champion the pursuit of pure knowledge and intellectual rigor as their core mission.</div></div>', 40, 1),

(@cdWriting, @tdC1, @gvId, N'Phát triển ý tưởng và Từ vựng dạng Problem & Solution Essay (Môi trường & Đô thị hóa)', N'Viet',
 N'Cách liên kết nguyên nhân - giải pháp logic, sử dụng từ nối học thuật C1 như Consequently, Mitigate, Implement.',
 N'<div class=''lesson-content''><h2>Phát Triển Luận Điểm & Kho Từ Vựng Học Thuật C1 Dạng Problem & Solution Essay (Environmental & Urban Issues)</h2><p class=''lead''>Làm chủ phương pháp liên kết nhân quả logic nhiều tầng và áp dụng kho từ vựng học thuật cao cấp C1/C2 để phân tích các vấn nạn môi trường, giao thông và đô thị hóa toàn cầu.</p><h3>1. Mô hình tư duy logic Root-Cause -> Impact -> Multi-Tier Solution</h3><ul><li><strong>Không viết giải pháp chung chung:</strong> Thay vì viết "the government should raise awareness", hãy đề xuất các chính sách vĩ mô cụ thể như "implementing progressive carbon taxation and subsidizing renewable infrastructure".</li><li><strong>Nguyên tắc tương ứng 1-1 (Direct Correspondence):</strong> Giải pháp ở Body 2 phải giải quyết trực tiếp và triệt để từng nguyên nhân gốc rễ đã được chỉ ra ở Body 1.</li></ul><h3>2. Danh mục từ vựng C1 chủ đề Đô thị hóa & Môi trường (Advanced Lexical Bank)</h3><ul><li><strong>Thuật ngữ nguyên nhân:</strong> Rapid urbanization (đô thị hóa chóng mặt), Unchecked industrial effluent (nước thải công nghiệp chưa xử lý), Proliferation of private automobiles (sự bùng nổ phương tiện cá nhân), High-density residential sprawl (sự bành trướng dân cư mật độ cao).</li><li><strong>Thuật ngữ tác động:</strong> Ecological degradation (suy thoái sinh thái), Deterioration of air quality indices (suy giảm chỉ số chất lượng không khí), Strain on civic infrastructure (áp lực đè nặng lên hạ tầng dân sinh).</li><li><strong>Thuật ngữ giải pháp:</strong> Subsidize clean public transit networks (trợ giá mạng lưới giao thông công cộng sạch), Stringent environmental mandates (các quy định môi trường nghiêm ngặt), Incentivize circular economy practices (khuyến khích mô hình kinh tế tuần hoàn).</li></ul><h3>3. Bài luận mẫu học thuật C1 hoàn chỉnh (Model Academic C1 Essay)</h3><div class=''essay-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''>The exponential migration of rural populations into metropolitan areas has exacerbated urban congestion and environmental degradation to alarming proportions. This essay will examine the principal determinants behind this predicament and formulate actionable systemic remedies.<br/><br/>The primary catalyst behind modern urban pollution is the disproportionate reliance on fossil-fuel powered private transport, coupled with obsolete civic urban planning. As burgeoning middle-class demographics expand in developing metropolises, millions of personal motorbikes and combustion-engine automobiles flood under-dimensioned road arteries daily. Furthermore, municipal zoning policies historically permitted polluting manufacturing clusters to operate in close proximity to residential quarters, resulting in chronic particulate matter (PM2.5) emissions that severely compromise public respiratory health.<br/><br/>To mitigate these pressing predicaments, a synchronized multi-tier intervention is imperative. First and foremost, municipal administrations must modernize mass public transit by investing substantially in electric bus fleets and underground metropolitan rail lines, rendering daily public commuting faster and more cost-effective than private automobile ownership. Concurrently, governing bodies should institute congestion pricing schemes within central business districts to disincentivize vehicular overcrowding. On the industrial front, enforcing stringent emission limits backed by deterrent fiscal penalties will compel manufacturing facilities to transition toward green renewable energy sources.<br/><br/>In summary, while urban ecological challenges stem from overpopulation and vehicular overuse, comprehensive investments in sustainable public transportation and rigorous regulatory oversight represent viable long-term antidotes.</div></div>', 45, 1),

-- IELTS Listening
(@cdListening, @tdB2, @gvId, N'Chiến lược chinh phục IELTS Listening Section 3: Thảo luận Đề tài Nhóm', N'Nghe',
 N'Nhận diện các bẫy đổi ý kiến, bẫy đồng thuận và kỹ thuật ghi chú nhanh câu trả lời.',
 N'<div class=''lesson-content''><h2>Chiến Thuật Chinh Phục IELTS Listening Section 3: Thảo Luận Học Thuật Nhóm (Tackle Academic Discussions)</h2><p class=''lead''>Vượt qua các bẫy đổi ý kiến, bẫy đồng thuận nửa vời và làm chủ kỹ thuật bắt mạch tranh luận giữa 2-3 người nói trong các bài thảo luận đồ án học thuật Section 3.</p><h3>1. Đặc trưng cấu trúc bài thi Listening Section 3</h3><ul><li><strong>Bối cảnh học thuật (Academic Setting):</strong> Cuộc trao đổi giữa 2 đến 3 người (ví dụ: hai sinh viên đại học thảo luận với giáo sư hướng dẫn về đề tài nghiên cứu khoa học, bài tập nhóm hoặc bài thuyết trình).</li><li><strong>Đặc điểm nhịp độ:</strong> Tốc độ nói nhanh, tự nhiên, có xen lẫn các đoạn ngắt lời, ngập ngừng, tranh luận và thay đổi quyết định vào phút chót.</li></ul><h3>2. Ba bẫy âm thanh phổ biến nhất và cách vô hiệu hóa</h3><ul><li><strong>Bẫy đổi ý kiến (The Mind-Change Trap):</strong> Người nói A đề xuất phương án X, người nói B phản đối, người nói A đồng ý chuyển sang phương án Y. Chú ý các từ nối chuyển hướng: <em>"Actually, on second thought...", "I was leaning toward that initially, but...", "That would have worked, except..."</em>.</li><li><strong>Bẫy đồng thuận một phần (Partial Agreement Trap):</strong> Người nghe nói <em>"I completely agree with your premise, but we lack the sample size"</em> ➔ Đây là lời từ chối khéo, không phải đồng thuận!</li><li><strong>Bẫy từ khóa lặp lại (Distractor Repetition):</strong> Lựa chọn sai thường chứa chính xác từ vựng nghe được trong băng nhưng mang ý nghĩa bị phủ định.</li></ul><h3>3. Kịch bản băng ghi âm mẫu & Phân tích đáp án (Model Script & Question Walkthrough)</h3><div class=''dialogue-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''><strong>Professor Clark:</strong> "Good afternoon, Rachel and Liam. I have reviewed the preliminary outline for your renewable energy case study. How are you dividing the presentation tasks?"<br/><br/><strong>Liam:</strong> "Well, Professor, I originally volunteered to handle the historical background of geothermal installations, while Rachel planned to analyze the financial cost-benefit models."<br/><br/><strong>Rachel:</strong> "That was the initial plan, Liam, but when I began compiling the data yesterday, the econometric statistical regression was much more complex than anticipated. Since your minor is in financial economics, wouldn''t it be far more efficient if you tackled the financial analytics while I compiled the environmental impact survey?"<br/><br/><strong>Liam:</strong> "You are absolutely right, Rachel. I am comfortable with regression modeling. Let us swap those two sections then."<br/><br/><strong>Phân tích câu hỏi trắc nghiệm:</strong><br/>• <em>Question: Which section of the presentation will Liam ultimately present?</em><br/>• <strong>Phân tích bẫy:</strong> Nghe thấy "historical background" đầu tiên (bẫy ban đầu), nhưng sau đó Rachel đề xuất đổi và Liam chốt: "Let us swap those two sections".<br/>• ➔ <strong>Đáp án chính xác:</strong> The financial analytics model.</div></div>', 30, 1),

(@cdListening, @tdC1, @gvId, N'Kỹ thuật bắt từ khóa trong bài giảng học thuật Section 4 (Monologue Lecture)', N'Nghe',
 N'Luyện nghe độc thoại tốc độ nhanh về các đề tài khảo cổ, sinh học và công nghệ vũ trụ.',
 N'<div class=''lesson-content''><h2>Kỹ Thuật Bắt Từ Khóa & Ghi Chú Tốc Ký Trong Bài Giảng Học Thuật Section 4 (Advanced Monologue Lecture)</h2><p class=''lead''>Duy trì sự tập trung cao độ trong suốt bài độc thoại học thuật dài 6 phút không có quãng nghỉ, kết hợp kỹ thuật dự đoán từ loại và bắt tín hiệu ngôn ngữ Signposting Language.</p><h3>1. Bản đồ định hướng bài giảng bằng Signposting Language (Cụm từ dẫn đường)</h3><ul><li><strong>Mở đầu & Định hình bài giảng:</strong> "Today, I would like to direct our attention to...", "In this lecture, we will explore three distinct facets of...".</li><li><strong>Chuyển sang luận điểm mới:</strong> "Turning now to the experimental methodology...", "Having examined the historical context, let us move on to...".</li><li><strong>Nhấn mạnh phát hiện bất ngờ:</strong> "What came as a profound revelation to researchers was...", "Contrary to prevailing scientific consensus...".</li><li><strong>Tóm tắt & Kết luận:</strong> "To recap the principal takeaways...", "In conclusion, the implications are twofold...".</li></ul><h3>2. Chiến lược 4 bước dự đoán chỗ trống trong phần điền từ (Gap-Fill Strategies)</h3><ul><li><strong>Bước 1 - Xác định loại từ cần điền:</strong> Dựa vào cấu trúc ngữ pháp trước và sau khoảng trống (Danh từ số ít/số nhiều, Tính từ, Động từ nguyên mẫu hay Con số).</li><li><strong>Bước 2 - Khoanh vùng trường nghĩa (Semantic Category):</strong> Chỗ trống liên quan đến địa danh, kích thước, tên loài vật hay công cụ nghiên cứu?</li><li><strong>Bước 3 - Bắt từ đồng nghĩa của từ khóa dẫn (Leading Keywords):</strong> Giảng viên trên băng gần như luôn paraphrase cụm từ xung quanh chỗ trống.</li><li><strong>Bước 4 - Kiểm tra chính tả tuyệt đối:</strong> Viết đúng số nhiều (-s/-es), viết hoa tên riêng và kiểm tra giới hạn từ (NO MORE THAN TWO WORDS AND/OR A NUMBER).</li></ul><h3>3. Đoạn trích bài giảng khoa học mẫu (Model Scientific Lecture Script)</h3><div class=''lecture-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''>"Good morning, class. Today, we continue our investigation into marine ecosystem resilience, focusing specifically on deep-sea hydrothermal vent communities.<br/><br/>Discovered only in 1977 along the Galapagos Rift, these extreme biological environments thrive in utter darkness, subjected to crushing hydrostatic pressures exceeding two hundred atmospheres. What astonished marine biologists was not merely the survival of organisms in near-boiling mineral water, but the foundational trophic mechanism driving the ecosystem.<br/><br/>In conventional aquatic biomes, photosynthesis serves as the primary energy engine. However, in hydrothermal zones devoid of sunlight, specialized bacteria carry out a biochemical process termed <em>chemosynthesis</em>. By oxidizing hydrogen sulfide vented from the Earth''s crust, these microorganisms synthesize organic compounds, forming the primary nutritional bedrock for giant tube worms and ghost crabs."</div></div>', 35, 1),

-- Ngữ pháp: Các thì
(@cdTenses, @tdB1, @gvId, N'Thì Hiện Tại Hoàn Thành — Cấu trúc & Cách dùng', N'NguPhap', 
 N'Bài học trình bày chi tiết cấu trúc thì hiện tại hoàn thành, các trạng từ đi kèm (already, yet, ever, never, just) và cách phân biệt với thì quá khứ đơn.', 
 N'<div class=''lesson-content''><h2>Thì Hiện Tại Hoàn Thành (Present Perfect Tense)</h2><p class=''lead''>Thì Hiện tại hoàn thành là một trong những điểm ngữ pháp trọng yếu và xuất hiện với tần suất dày đặc nhất trong cả văn nói lẫn văn viết tiếng Anh học thuật và chuyên ngành.</p><h3>1. Cấu trúc ngữ pháp chuẩn xác (Form & Syntax)</h3><div class=''formula-box''><p><strong>Khẳng định (+):</strong> Subject + have / has + Past Participle (V3/ed) + Object</p><p><strong>Phủ định (-):</strong> Subject + have / has + not (haven''t / hasn''t) + Past Participle (V3/ed)</p><p><strong>Nghi vấn (?):</strong> (Wh-word) + have / has + Subject + Past Participle (V3/ed)?</p></div><h3>2. 4 Cách dùng cốt lõi trong công việc (Core Practical Usages)</h3><ol><li><strong>Hành động đã hoàn thành nhưng kết quả còn quan trọng ở hiện tại:</strong><br/><em>"We have successfully deployed the latest security patch to all production clusters."</em></li><li><strong>Trải nghiệm hoặc kinh nghiệm tích lũy (thường đi với ever, never, so far):</strong><br/><em>"She has led multiple cross-functional engineering teams over the last five years."</em></li><li><strong>Hành động bắt đầu trong quá khứ và vẫn đang tiếp diễn (đi với Since và For):</strong><br/><em>"Our company has collaborated with international cloud partners since 2018."</em></li><li><strong>Hành động vừa mới xảy ra hoặc hoàn tất sớm hơn dự kiến (đi với Just, Already, Yet):</strong><br/><em>"Have you finalized the pull request for the authentication module yet?"</em></li></ol><h3>3. Bảng phân biệt Hiện tại hoàn thành vs. Quá khứ đơn</h3><table border=''1'' cellpadding=''6'' style=''border-collapse:collapse; width:100%;''><thead><tr style=''background:#f1f5f9;''><th>Thì Hiện Tại Hoàn Thành</th><th>Thì Quá Khứ Đơn</th></tr></thead><tbody><tr><td>Thời gian không xác định hoặc kéo dài đến hiện tại: <em>"I have reviewed three proposals this morning."</em></td><td>Thời gian đã chấm dứt hoàn toàn trong quá khứ: <em>"I reviewed the proposal yesterday."</em></td></tr><tr><td>Từ nhận biết: <em>already, yet, just, since, for, recently, so far.</em></td><td>Từ nhận biết: <em>yesterday, ago, last week, in 2020.</em></td></tr></tbody></table></div>', 20, 1),

(@cdTenses, @tdA2, @gvId, N'Phân biệt Quá khứ Đơn (Past Simple) và Quá khứ Tiếp diễn (Past Continuous)', N'NguPhap',
 N'Cách dùng cấu trúc When và While để diễn tả hành động đang xảy ra thì có hành động khác xen vào.',
 N'<div class=''lesson-content''><h2>Làm Chủ Phân Biệt Quá Khứ Đơn (Past Simple) & Quá Khứ Tiếp Diễn (Past Continuous)</h2><p class=''lead''>Nắm trọn bản chất ngữ pháp diễn tả các sự kiện trong quá khứ, làm chủ cấu trúc phối hợp thời gian với When và While để kể chuyện và viết bài học thuật sinh động.</p><h3>1. Cấu trúc ngữ pháp & Bản chất thời gian (Form & Temporal Logic)</h3><ul><li><strong>Quá khứ đơn (Past Simple):</strong> Subject + V2/ed (Phủ định: did not + V-bare). Diễn tả hành động đã diễn ra và kết thúc hoàn toàn tại một thời điểm xác định trong quá khứ.</li><li><strong>Quá khứ tiếp diễn (Past Continuous):</strong> Subject + was / were + V-ing (Phủ định: was / were not + V-ing). Diễn tả hành động đang xảy ra tại một thời điểm cụ thể trong quá khứ hoặc đóng vai trò bối cảnh nền (background action).</li></ul><h3>2. Ba nguyên lý phối hợp thì kinh điển (Three Classic Combinations)</h3><ul><li><strong>Quy tắc 1 - Hành động đang xảy ra thì có hành động khác xen vào:</strong><br/>- Hành động kéo dài (nền) chia <em>Past Continuous</em>, hành động xen vào (ngắn) chia <em>Past Simple</em>.<br/>- <em>Công thức:</em> <strong>While / As</strong> + Past Continuous, Past Simple HOẶC Past Continuous + <strong>When</strong> + Past Simple.<br/>- <em>Ví dụ:</em> "While the lead developer was updating the server configurations, an unexpected power outage occurred."</li><li><strong>Quy tắc 2 - Hai hành động diễn ra song song cùng lúc trong quá khứ:</strong><br/>- Cả hai vế đều chia <em>Past Continuous</em>.<br/>- <em>Ví dụ:</em> "While Alice was analyzing the quarterly financial statements, Bob was drafting the executive summary."</li><li><strong>Quy tắc 3 - Chuỗi hành động liên tiếp xảy ra trong quá khứ:</strong><br/>- Toàn bộ chuỗi hành động đều chia <em>Past Simple</em>.<br/>- <em>Ví dụ:</em> "She entered the boardroom, unpacked her presentation notes, and greeted the directors."</li></ul><h3>3. Đoạn văn mẫu ứng dụng thực tế (Model Narrative Passage)</h3><div class=''narrative-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''>Last Friday was an extraordinarily eventful afternoon at our tech incubator. At approximately 3:00 PM, while our engineering leads <em>were presenting</em> the latest AI prototype to a delegation of foreign investors, the building''s fire alarm suddenly <em>went off</em>.<br/><br/>The sprinkler systems instantly activated. While panic-stricken participants <em>were scrambling</em> toward the emergency stairwells, our chief security engineer calmly <em>secured</em> the server backups, <em>disconnected</em> the power distribution units, and <em>escorted</em> the visiting guests safely to the assembly point.<br/><br/>Fortunately, the incident was merely caused by a faulty smoke detector in the cafeteria kitchen. By 4:30 PM, everyone <em>had returned</em> to their workstations, and the team successfully <em>concluded</em> the investment negotiations.</div></div>', 20, 1),

(@cdTenses, @tdA1, @gvId, N'Làm chủ Tương lai Đơn (Will) và Tương lai Gần (Be Going To)', N'NguPhap',
 N'Phân biệt quyết định tức thời tại thời điểm nói với kế hoạch hoặc dự đoán có căn cứ xác thực.',
 N'<div class=''lesson-content''><h2>Phân Biệt Chuẩn Xác Tương Lai Đơn (Will) và Tương Lai Gần (Be Going To)</h2><p class=''lead''>Hiểu sâu sắc sự khác biệt giữa quyết định bột phát tức thời và kế hoạch đã chuẩn bị từ trước, giúp bạn diễn đạt dự định và phán đoán tương lai chuẩn xác như người bản xứ.</p><h3>1. Tương lai đơn với WILL (Spontaneous Decisions, Promises & Predictions)</h3><ul><li><strong>Quyết định đưa ra ngay tại thời điểm nói (On-the-spot decision):</strong> "The phone is ringing. I will answer it!" (Không có kế hoạch trước).</li><li><strong>Lời hứa, cam kết hoặc đề nghị giúp đỡ:</strong> "Do not worry about the deadline; I will assist you with compiling the report.", "I promise I will keep your password confidential."</li><li><strong>Dự đoán dựa trên quan điểm, cảm tính cá nhân (thường đi với Think, Believe, Hope, Probably):</strong> "I believe our startup will achieve profitability within the next two fiscal quarters."</li></ul><h3>2. Tương lai gần với BE GOING TO (Prior Intentions & Evidence-based Predictions)</h3><ul><li><strong>Kế hoạch, dự định đã được cân nhắc và quyết định từ trước:</strong> "We are going to launch our mobile application next month; the beta testing phase is already complete."</li><li><strong>Dự đoán dựa trên bằng chứng cụ thể rõ ràng ở hiện tại (Physical evidence):</strong> "Look at those dark overcast clouds; it is going to rain heavily in a few minutes.", "The car is speeding and skidding; it is going to hit the barricade!"</li></ul><h3>3. Bảng đối chiếu tình huống thực hành (Comparative Dialogue Matrix)</h3><div class=''dialogue-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''><strong>Tình huống 1 (Kế hoạch có sẵn):</strong><br/>• <em>Manager:</em> "Why are you packing up your design monitors so early, Thomas?"<br/>• <em>Thomas:</em> "I <strong>am going to attend</strong> the annual UX Design Summit in Chicago tomorrow morning. My flight was booked two weeks ago."<br/><br/><strong>Tình huống 2 (Quyết định phát sinh đột xuất):</strong><br/>• <em>Manager:</em> "Oh, I completely forgot to notify the client about our server maintenance tonight!"<br/>• <em>Thomas:</em> "Do not stress, manager. I <strong>will draft and send</strong> the notification email right away before I leave."</div></div>', 15, 1),

-- Ngữ pháp: Mệnh đề quan hệ & Bị động
(@cdMenhDe, @tdB2, @gvId, N'Bí quyết Rút gọn Mệnh đề Quan hệ dạng V-ing, V-ed và To-Infinitive', N'NguPhap',
 N'Chuyên đề ngữ pháp nâng cao giúp câu văn gọn gàng, tăng điểm Grammatical Range & Accuracy.',
 N'<div class=''lesson-content''><h2>Tuyệt Kỹ Rút Gọn Mệnh Đề Quan Hệ (Reduced Relative Clauses Mastery - Band 7.0+)</h2><p class=''lead''>Tối ưu hóa câu văn học thuật, loại bỏ sự rườm rà không cần thiết và gia tăng điểm số Grammatical Range & Accuracy trong các bài luận chuyên ngành và bài thi quốc tế.</p><h3>1. Rút gọn thành Cụm Hiện tại phân từ (Present Participle Phrase - V-ing)</h3><ul><li><strong>Điều kiện:</strong> Mệnh đề quan hệ ở thể chủ động (Active voice).</li><li><strong>Quy tắc:</strong> Lược bỏ đại từ quan hệ (who, which, that) và trợ động từ to be (nếu có), chuyển động từ chính về dạng V-ing.</li><li><em>Câu gốc:</em> The passengers <em>who are waiting</em> at Gate 4 should proceed to the security counter.<br/>➔ <em>Rút gọn:</em> The passengers <strong>waiting</strong> at Gate 4 should proceed to the security counter.</li><li><em>Câu gốc:</em> Any employee <em>who wishes</em> to enroll in the data science seminar must register by Friday.<br/>➔ <em>Rút gọn:</em> Any employee <strong>wishing</strong> to enroll in the data science seminar must register by Friday.</li></ul><h3>2. Rút gọn thành Cụm Quá khứ phân từ (Past Participle Phrase - V-ed/V3)</h3><ul><li><strong>Điều kiện:</strong> Mệnh đề quan hệ ở thể bị động (Passive voice).</li><li><strong>Quy tắc:</strong> Lược bỏ đại từ quan hệ và to be, giữ lại quá khứ phân từ V-ed/V3.</li><li><em>Câu gốc:</em> The confidential documents <em>which were leaked</em> to the press caused substantial market volatility.<br/>➔ <em>Rút gọn:</em> The confidential documents <strong>leaked</strong> to the press caused substantial market volatility.</li><li><em>Câu gốc:</em> Architecture projects <em>that are funded</em> by international green bonds must satisfy rigorous sustainability standards.<br/>➔ <em>Rút gọn:</em> Architecture projects <strong>funded</strong> by international green bonds must satisfy rigorous sustainability standards.</li></ul><h3>3. Rút gọn thành Cụm Động từ nguyên mẫu (To-Infinitive Phrase)</h3><ul><li><strong>Điều kiện:</strong> Danh từ đi trước có chứa các từ chỉ thứ tự: <em>the first, the second, the last, the only</em> hoặc cấp so sánh nhất (<em>the best, the oldest</em>).</li><li><em>Câu gốc:</em> Neil Armstrong was the first person <em>who set foot</em> on the lunar surface.<br/>➔ <em>Rút gọn:</em> Neil Armstrong was the first person <strong>to set foot</strong> on the lunar surface.</li><li><em>Câu gốc:</em> Dr. Watson was the only researcher <em>who identified</em> the molecular anomaly.<br/>➔ <em>Rút gọn:</em> Dr. Watson was the only researcher <strong>to identify</strong> the molecular anomaly.</li></ul><h3>4. Đoạn trích học thuật ứng dụng thực tế (Academic Excerpt with Reductions)</h3><div class=''reading-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''>In a groundbreaking study <strong>published</strong> by the Oxford Economic Forum, analysts <strong>investigating</strong> decentralized finance discovered significant patterns. Cryptocurrencies <strong>pegged</strong> to statutory commodities demonstrated 40% less volatility than speculative tokens. Furthermore, Norway became the premier nation <strong>to implement</strong> fully digitized central bank clearing protocols, <strong>paving</strong> the way for modernized monetary sovereignty.</div></div>', 25, 1),

(@cdMenhDe, @tdB1, @gvId, N'Câu Bị Động Đặc Biệt: Động từ tường thuật và Thể truyền khiến (Have/Get)', N'NguPhap',
 N'Cấu trúc It is said that..., He is believed to be... và cấu trúc Have something done.',
 N'<div class=''lesson-content''><h2>Chuyên Đề Câu Bị Động Đặc Biệt: Động Từ Tường Thuật & Thể Truyền Khiến (Causative Form & Impersonal Passive)</h2><p class=''lead''>Làm chủ thể bị động khách quan học thuật (It is said that...) và thể truyền khiến (Have/Get something done), biến những câu văn khô khan thành phong cách diễn đạt báo chí chuyên nghiệp.</p><h3>1. Thể truyền khiến nhờ vả / thuê mướn (The Causative Form)</h3><ul><li><strong>Cấu trúc chủ động:</strong> Have someone DO something (nhờ ai làm gì) / Get someone TO DO something (thuyết phục ai làm gì).<br/>- <em>Ví dụ:</em> "We had the cloud consultant audit our system." / "We got the contractor to expedite the delivery."</li><li><strong>Cấu trúc bị động:</strong> <strong>Subject + have / get + OBJECT (vật) + Past Participle (V3/ed)</strong> (cho biết một việc đã được thực hiện bởi người khác).<br/>- <em>Ví dụ:</em> "We <strong>had our firewall upgraded</strong> yesterday." / "She <strong>got her laptop repaired</strong> at the authorized service center."</li></ul><h3>2. Bị động khách quan với Động từ tường thuật (Impersonal Passive)</h3><ul><li><strong>Các động từ thường gặp:</strong> Say, believe, think, report, rumor, claim, assume, estimate, acknowledge.</li><li><strong>Cấu trúc 1 (Chủ ngữ giả IT):</strong> <strong>It + be + V3/ed (said/believed/reported) + THAT + S + V</strong>.<br/>- <em>Ví dụ:</em> "It is widely acknowledged that artificial intelligence will transform medical diagnostics."</li><li><strong>Cấu trúc 2 (Đưa chủ ngữ thật lên đầu câu):</strong><br/>- <em>Khi cùng thì với mệnh đề chính:</em> <strong>S + be + V3/ed + TO + V-bare</strong>.<br/>  <em>Ví dụ:</em> "The tech giant is rumored <strong>to acquire</strong> the autonomous driving startup."<br/>- <em>Khi xảy ra trước mệnh đề chính:</em> <strong>S + be + V3/ed + TO HAVE + V3/ed</strong>.<br/>  <em>Ví dụ:</em> "The ancient temple is believed <strong>to have collapsed</strong> during the 14th-century earthquake."</li></ul><h3>3. Bản tin thời sự tài chính mẫu (Model Financial News Release)</h3><div class=''news-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''>It has been officially confirmed this morning that Apex Global Telecom has had its international satellite bandwidth doubled following the orbital deployment of satellite APX-9. The corporation is reported to have invested upwards of four hundred million dollars in the venture. Furthermore, Chief Executive Marcus Vance is understood to be finalizing a monumental partnership agreement with European aerospace agencies, an enterprise that is widely expected to revolutionize global cellular connectivity in isolated polar territories.</div></div>', 20, 1),

-- Ngữ pháp: Điều kiện & Đảo ngữ
(@cdDieuKien, @tdB1, @gvId, N'Tổng hợp Câu Điều Kiện loại 1, 2, 3 và Điều Kiện Trộn (Mixed Conditionals)', N'NguPhap',
 N'Nắm trọn bản chất giả định điều kiện hiện tại, quá khứ và sự kết hợp kết quả hiện tại.',
 N'<div class=''lesson-content''><h2>Toàn Thư Câu Điều Kiện: Loại 1, 2, 3 & Điều Kiện Trộn Hỗn Hợp (Mastering All Conditional Types)</h2><p class=''lead''>Nắm trọn bản chất giả định thời gian, từ các khả năng có thật ở tương lai, giả định trái ngược hiện tại, nuối tiếc quá khứ cho đến tư duy kết hợp đa chiều trong Điều kiện trộn.</p><h3>1. Hệ thống 3 loại câu điều kiện căn bản (The Fundamental Triad)</h3><ul><li><strong>Loại 1 (Real in Future):</strong> <em>If + Present Simple, will + V-bare</em>.<br/>- Diễn tả điều kiện có thật hoặc hoàn toàn khả thi trong hiện tại hoặc tương lai.<br/>- <em>Ví dụ:</em> "If we secure the venture funding, we will expand our engineering division."</li><li><strong>Loại 2 (Unreal in Present):</strong> <em>If + Past Simple (to be: were), would + V-bare</em>.<br/>- Giả định điều trái ngược với thực tế ở hiện tại hoặc khó có thể xảy ra.<br/>- <em>Ví dụ:</em> "If I were the Minister of Education, I would make coding a compulsory subject in all primary schools."</li><li><strong>Loại 3 (Unreal in Past):</strong> <em>If + Past Perfect (had + V3), would have + V3</em>.<br/>- Giả định điều trái ngược với quá khứ, thể hiện sự nuối tiếc hoặc phân tích bài học lịch sử.<br/>- <em>Ví dụ:</em> "If the security team had patched the server vulnerability in March, the data breach would not have occurred."</li></ul><h3>2. Câu điều kiện trộn hỗn hợp (Mixed Conditionals Mastery)</h3><ul><li><strong>Dạng 1 (Quá khứ giả định ➔ Kết quả Hiện tại):</strong><br/>- <em>Công thức:</em> <strong>If + had + V3/ed, S + would + V-bare (now/today)</strong>.<br/>- <em>Bản chất:</em> Hành động trong quá khứ không xảy ra, dẫn đến kết quả ở hiện tại bị thay đổi.<br/>- <em>Ví dụ:</em> "If I <strong>had accepted</strong> the scholarship to Stanford five years ago, I <strong>would be living</strong> in California right now."</li><li><strong>Dạng 2 (Bản chất hiện tại ➔ Hành động quá khứ):</strong><br/>- <em>Công thức:</em> <strong>If + Past Simple (were/V2), S + would have + V3/ed</strong>.<br/>- <em>Bản chất:</em> Một phẩm chất hay sự thật hiển nhiên ở hiện tại đã chi phối hành động trong quá khứ.<br/>- <em>Ví dụ:</em> "If she <strong>were not so terrified</strong> of public speaking, she <strong>would have accepted</strong> the keynote invitation last month."</li></ul><h3>3. Đoạn văn suy ngẫm nghề nghiệp mẫu (Model Career Reflection)</h3><div class=''essay-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''>Looking back on our entrepreneurial voyage, decision points are remarkably evident. If our founders had hesitated during the seed round in 2021, our software platform would not dominate the Southeast Asian logistics sector today. Had we possessed greater architectural insight at the outset, we would have avoided months of excruciating database migration. Nonetheless, as long as we maintain customer-centric dedication, our enterprise will continue to redefine the boundaries of digital logistics innovation.</div></div>', 25, 1),

(@cdDieuKien, @tdC1, @gvId, N'Cấu trúc Đảo ngữ nâng cao với Never, Seldom, Not only và Đảo ngữ câu điều kiện', N'NguPhap',
 N'Tuyệt chiêu biến hóa câu văn đẳng cấp C1 với Had I known..., Were she to arrive..., Should you need help.',
 N'<div class=''lesson-content''><h2>Nghệ Thuật Đảo Ngữ Nâng Cao Trong Tiếng Anh Học Thuật (Advanced Inversion Mastery - C1/C2)</h2><p class=''lead''>Tạo điểm nhấn tu từ mạnh mẽ, thể hiện năng lực ngữ pháp bậc thầy và nâng cấp văn phong học thuật lên tiêu chuẩn của các ấn phẩm quốc tế.</p><h3>1. Đảo ngữ với trạng từ mang nghĩa phủ định và bán phủ định</h3><ul><li><strong>Nhóm trạng từ:</strong> Never, Rarely, Seldom, Hardly, Scarcely, Barely, Little.</li><li><strong>Cấu trúc:</strong> <strong>Trạng từ phủ định + Trợ động từ (Auxiliary) + S + V-chính</strong>.</li><li><em>Ví dụ 1:</em> "Seldom have economists observed such rapid synchronization across global supply chains."</li><li><em>Ví dụ 2:</em> "Hardly had the quantum prototype been energized when it achieved stable superposition." (Hardly... when / No sooner... than).</li></ul><h3>2. Đảo ngữ với cụm Not Only, Only và Cụm giới từ hạn định</h3><ul><li><strong>Not only... but also:</strong> "Not only did the engineering team meet the aggressive deadline, but they also reduced cloud operational costs by 30%."</li><li><strong>Only after / Only when / Only by:</strong> Lưu ý đảo ngữ ở <em>mệnh đề chính</em>.<br/>- "Only by automating compliance testing can organizations deploy software multiple times a day safely."</li><li><strong>Under no circumstances / On no account (Tuyệt đối không trong bất kỳ hoàn cảnh nào):</strong><br/>- "Under no circumstances should root database credentials be committed into public version control repositories."</li></ul><h3>3. Đảo ngữ câu điều kiện không dùng IF (Conditional Inversion)</h3><ul><li><strong>Loại 1: Should + S + V-bare:</strong> "Should you require further clarification regarding the SLA terms, please do not hesitate to contact our legal counsel." (Thay cho If you require...).</li><li><strong>Loại 2: Were + S + to-V (hoặc Were + S + adj/noun):</strong> "Were the federal reserve to hike interest rates unexpectedly, commercial borrowing costs would spike immediately." (Thay cho If the federal reserve were to hike...).</li><li><strong>Loại 3: Had + S + V3/ed:</strong> "Had the flight navigation system received the telemetry update in time, the rerouting delay would have been entirely averted." (Thay cho If the flight navigation system had received...).</li></ul><h3>4. Diễn văn khai mạc hội nghị quốc tế mẫu (Model Keynote Speech Excerpt)</h3><div class=''speech-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''>"Distinguished delegates and fellow scientists. Never in human history has technological evolution accelerated with such breathtaking momentum as we bear witness to today. Seldom before have ethical imperatives and computer sciences intersected so intimately. Only when we establish transparent, auditable guardrails can the promises of artificial general intelligence truly benefit all humanity. Should we fail to uphold this collective stewardship, future generations will rightly question our wisdom. Let us rise together to meet this profound technological epoch."</div></div>', 30, 1),

-- Thương mại: Email & Đàm phán
(@cdEmail, @tdB1, @gvId, N'Quy chuẩn Viết Email Thương mại: Mở đầu, Thân bài và Lời chào kết', N'Viet',
 N'Cách mở đầu chuyên nghiệp (Dear Mr./Ms.), nêu mục đích viết thư rõ ràng và kết thúc lịch sự.',
 N'<div class=''lesson-content''><h2>Quy Chuẩn Viết Email Thương Mại Quốc Tế (Professional Business Email Mastery)</h2><p class=''lead''>Kỹ năng soạn thảo email thương mại chuyên nghiệp là tiêu chuẩn bắt buộc trong môi trường làm việc toàn cầu, giúp thiết lập lòng tin và tối ưu hóa hiệu quả trao đổi công việc.</p><h3>1. Cấu trúc 4 phần của một Email chuẩn mực (Standard Email Architecture)</h3><ul><li><strong>1. Subject Line (Dòng tiêu đề súc tích):</strong> Phải trực diện, chứa từ khóa chính (Action Required, Meeting Request, Project Update).</li><li><strong>2. Professional Salutation (Lời chào trang trọng):</strong> Dear Mr./Ms. [Last Name], Dear Project Team, hoặc Dear Hiring Committee.</li><li><strong>3. Body Paragraphs (Nội dung súc tích & mạch lạc):</strong><br/>- Đoạn 1: Nêu lý do gửi thư (Reason for writing).<br/>- Đoạn 2: Trình bày chi tiết công việc hoặc báo cáo số liệu.<br/>- Đoạn 3: Nêu kỳ vọng hành động tiếp theo (Call to action / Next steps).</li><li><strong>4. Complimentary Close & Sign-off (Lời chào kết & Chữ ký):</strong> Sincerely, Best regards, hoặc Warm regards kèm chữ ký số đầy đủ.</li></ul><h3>2. Bài viết mẫu hoàn chỉnh (Model Business Email)</h3><div class=''email-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''><strong>Subject:</strong> [Project Alpha] Sprint 14 Progress Report & Next Steps<br/><br/>Dear Mr. Henderson,<br/><br/>I am writing to provide you with a comprehensive status report on our current development cycle for Project Alpha. Over the past two weeks, the engineering team has achieved all core technical objectives ahead of the agreed milestones.<br/><br/>Specifically, we have successfully resolved the database latency bottleneck, resulting in a 45% reduction in API response times across our authentication endpoints. Furthermore, our automated integration test coverage has now reached 92%, satisfying all compliance requirements for the upcoming enterprise rollout.<br/><br/>Moving forward, we are scheduled to conduct user acceptance testing (UAT) on Thursday morning. Could you please confirm if your team is available for a thirty-minute walkthrough at 10:00 AM EST?<br/><br/>Thank you very much for your continuous collaboration and support. Please do not hesitate to contact me if you have any questions.<br/><br/>Sincerely yours,<br/><strong>Minh Nguyen</strong><br/>Lead Software Engineer | Enterprise Solutions Division</div></div>', 25, 1),

(@cdEmail, @tdB2, @gvId, N'Soạn thảo Email Đặt lịch hẹn, Xác nhận và Xử lý Khiếu nại Khách hàng', N'Viet',
 N'Mẫu thư xin lỗi đối tác chuyên nghiệp, cam kết phương án bồi thường và giữ gìn uy tín doanh nghiệp.',
 N'<div class=''lesson-content''><h2>Kỹ Năng Soạn Thảo Email Thương Mại Chuyên Biệt: Đặt Lịch Hẹn, Xác Nhận & Xử Lý Khiếu Nại Khách Hàng (Customer Care & Escalation Emails)</h2><p class=''lead''>Làm chủ nghệ thuật viết thư giải quyết khiếu nại (escalation management), thể hiện sự đồng cảm doanh nghiệp chuẩn mực và biến sự cố bất khả kháng thành cơ hội thắt chặt lòng trung thành của khách hàng.</p><h3>1. Cấu trúc 5 bước xử lý khiếu nại khách hàng theo mô hình quốc tế (The LAST Formula)</h3><ul><li><strong>1. Listen & Acknowledge (Lắng nghe & Thấu hiểu):</strong> Xác nhận ngay lập tức rằng bạn đã tiếp nhận thông tin và thấu hiểu sâu sắc những bất tiện mà khách hàng đang trải qua.</li><li><strong>2. Apologize Sincerely (Xin lỗi chân thành & Chuyên nghiệp):</strong> Đưa ra lời xin lỗi đại diện cho công ty mà không đổ lỗi cho các yếu tố ngoại cảnh hay cá nhân nhân viên.</li><li><strong>3. Solve & Outline Action Steps (Giải pháp cụ thể):</strong> Nêu rõ các bước khắc phục kỹ thuật đang được tiến hành khẩn trương kèm mốc thời gian (ETA) rõ ràng.</li><li><strong>4. Compensate & Value (Bồi thường thỏa đáng):</strong> Cung cấp phương án đền bù thiện chí (hoàn phí, gia hạn dịch vụ miễn phí, nâng cấp hạng tài khoản).</li><li><strong>5. Thank & Prevent Recurrence (Cảm ơn & Cam kết phòng ngừa):</strong> Cảm ơn sự kiên nhẫn của khách hàng và nêu biện pháp ngăn chặn sự cố tái diễn.</li></ul><h3>2. Thư thương mại mẫu hoàn chỉnh: Xử lý sự cố gián đoạn dịch vụ Cloud (Model Escalation Email)</h3><div class=''email-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''><strong>Subject:</strong> [Official Resolution] Service Interruption Incident #INC-89241 - Account Enterprise Tier<br/><br/>Dear Mr. Abernathy,<br/><br/>I am writing to you directly regarding the unexpected latency disruption your team experienced on our cloud database cluster between 08:30 and 10:15 EST this morning. On behalf of the entire executive leadership at CloudCore Systems, I offer our unreserved apologies for the substantial inconvenience this caused to your live e-commerce operations.<br/><br/>Our root cause analysis has confirmed that the disruption was triggered by a rare hardware memory fault during an automated routing update at our Virginia data center. Within twenty minutes of detection, our Site Reliability Engineering team isolated the affected cluster, redirected all client traffic to redundant standby instances, and completely restored nominal API performance.<br/><br/>To ensure your operations are made whole and to reaffirm our unwavering commitment to your success, we are implementing the following immediate remedies:<br/>• A 100% service fee credit applied to your entire monthly hosting invoice, exceeding our standard Service Level Agreement (SLA) commitment.<br/>• Complimentary upgrade of your account to our 24/7 Dedicated White-Glove Architectural Support for the next six months.<br/>• Implementation of secondary failover nodes that will automatically eliminate single-point hardware dependencies moving forward.<br/><br/>We deeply value the partnership we have built with your enterprise over the past four years. Please do not hesitate to contact me directly at my mobile number below if you would like to arrange a technical debriefing session today.<br/><br/>Warm regards,<br/><strong>Elena Rostova</strong><br/>Vice President of Global Customer Operations | CloudCore Infrastructure Inc.</div></div>', 30, 1),

(@cdDamPhan, @tdB2, @gvId, N'Chiến lược Đàm phán Hợp đồng, Thỏa thuận Giá cả và Điều khoản Giao hàng', N'TuVung',
 N'Bộ từ vựng và câu nói ngoại giao giúp bạn đạt được thỏa thuận có lợi nhất (Win-Win Solution).',
 N'<div class=''lesson-content''><h2>Chiến Thuật Đàm Phán Thương Mại Quốc Tế: Giá Cả, Hợp Đồng & Điều Khoản Giao Nhận (International Commercial Negotiation)</h2><p class=''lead''>Làm chủ nghệ thuật đàm phán ngoại giao (diplomatic phrasing), bảo vệ biên lợi nhuận và thiết lập các thỏa thuận cùng có lợi (Win-Win Outcomes) trong môi trường kinh doanh xuyên biên giới.</p><h3>1. Danh mục thuật ngữ hợp đồng thương mại quốc tế (Commercial Lexicon)</h3><ul><li><strong>Giá cả & Khối lượng:</strong> Unit cost (Đơn giá sản phẩm), Volume discount (Chiết khấu số lượng lớn), Price ceiling / Price floor (Mức giá trần / Giá sàn).</li><li><strong>Điều kiện giao nhận Incoterms:</strong> FOB (Free on Board - Giao hàng lên tàu), CIF (Cost, Insurance and Freight - Tiền hàng, bảo hiểm và cước phí), Lead time (Thời gian từ khi đặt hàng đến khi nhận hàng).</li><li><strong>Điều khoản thanh toán & Chế tài:</strong> Net 30 / Net 60 (Thanh toán trong vòng 30/60 ngày), Irrevocable Letter of Credit (Thư tín dụng không hủy ngang - L/C), Liquidated damages (Khoản bồi thường thiệt hại được ước lượng trước theo hợp đồng).</li></ul><h3>2. Nghệ thuật sử dụng ngôn từ giảm nhẹ và ngoại giao (Diplomatic Hedging & Counter-Offers)</h3><ul><li><strong>Từ chối khéo léo đề xuất ban đầu:</strong> "While we appreciate your perspective, that price point would place significant strain on our production margins."</li><li><strong>Đưa ra giải pháp có điều kiện (Conditional Concessions):</strong> "We would be amenable to lowering the unit price by 6%, provided that your firm commits to a minimum annual volume of 50,000 units."</li><li><strong>Chốt điểm đồng thuận (Securing Alignment):</strong> "It appears we have established solid consensus on delivery timelines and payment terms. Shall we instruct our respective legal teams to formalize this draft?"</li></ul><h3>3. Biên bản đàm phán mẫu hoàn chỉnh (Model Executive Negotiation Transcript)</h3><div class=''dialogue-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:15px; border-radius:6px; font-family:sans-serif;''><strong>Buyer (Arthur Vance - Global Procurement Director):</strong> "Good morning, Ms. Zhang. We have reviewed your initial proposal for the industrial sensor batch. While we admire the technical specifications, your proposed quote of $145 per unit is approximately 15% above our allocated capital budget."<br/><br/><strong>Supplier (Mei Zhang - VP of Global Sales):</strong> "Good morning, Mr. Vance. Thank you for your candid feedback. Our pricing reflects our proprietary titanium casing and the extended three-year warranty we include as standard. However, we genuinely value a long-term strategic partnership with your enterprise. If your company could increase the initial purchase order from 20,000 to 35,000 units, we could comfortably adjust the unit price down to $130 on FOB terms."<br/><br/><strong>Buyer (Arthur Vance):</strong> "A price point of $130 sounds much more competitive. However, maintaining reliable inventory is critical for our assembly plants. Could your production facility guarantee a strict lead time of four weeks rather than six?"<br/><br/><strong>Supplier (Mei Zhang):</strong> "We can certainly commit to a four-week lead time, provided that down payments are disbursed via confirmed Letter of Credit within ten business days of signing."<br/><br/><strong>Buyer (Arthur Vance):</strong> "That sounds like a thoroughly equitable arrangement for both parties. Let us draw up the formal memorandum of understanding."</div></div>', 30, 1),

(@cdDamPhan, @tdC1, @gvId, N'Kỹ năng Thuyết trình Dự án (Pitching) trước Hội đồng Đầu tư Quốc tế', N'Doc',
 N'Cấu trúc Slide bài thuyết trình, ngôn ngữ cơ thể, cách dẫn dắt số liệu và trả lời phần Q&A sắc bén.',
 N'<div class=''lesson-content''><h2>Nghệ Thuật Pitching Dự Án Khởi Nghiệp Trước Quỹ Đầu Tư Mạo Hiểm (Venture Capital Pitching Mastery)</h2><p class=''lead''>Làm chủ cấu trúc thuyết trình gọi vốn chuẩn Thung lũng Silicon, tạo ấn tượng mạnh mẽ trước các nhà đầu tư mạo hiểm (Venture Capitalists) và bảo vệ định giá doanh nghiệp sắc bén.</p><h3>1. Cấu trúc 8 slide vàng của bài Pitch Deck kinh điển (The Proven 8-Slide Architecture)</h3><ul><li><strong>1. The Hook & The Problem (Vấn đề nhức nhối):</strong> Nêu rõ nỗi đau thị trường (pain point) bằng câu chuyện thực tế và số liệu tổn thất hàng năm.</li><li><strong>2. The Solution & Value Proposition (Giải pháp đột phá):</strong> Trình bày sản phẩm/dịch vụ của bạn giải quyết triệt để nỗi đau đó nhanh hơn, rẻ hơn hoặc tốt hơn gấp 10 lần ra sao.</li><li><strong>3. Market Size & Opportunity (Dung lượng thị trường):</strong> Chứng minh TAM (Total Addressable Market), SAM và SOM bằng các báo cáo nghiên cứu uy tín.</li><li><strong>4. Technology & Secret Sauce (Lợi thế cạnh tranh độc quyền):</strong> Bằng sáng chế, hiệu ứng mạng lưới (network effects) hoặc công nghệ AI cốt lõi đối thủ khó sao chép.</li><li><strong>5. Traction & Milestones (Bằng chứng tăng trưởng thực tế):</strong> Doanh thu định kỳ hàng tháng (MRR), tốc độ tăng trưởng người dùng và tỷ lệ giữ chân khách hàng (retention rate).</li><li><strong>6. Business Model & Unit Economics (Mô hình kinh doanh):</strong> Lợi nhuận gộp, CAC (Chi phí thu hút khách hàng) và LTV (Giá trị vòng đời khách hàng).</li><li><strong>7. The Team (Đội ngũ sáng lập):</strong> Kinh nghiệm dày dặn của các nhà sáng lập trong ngành.</li><li><strong>8. The Ask & Use of Funds (Kêu gọi số vốn cụ thể):</strong> Nêu rõ số tiền cần huy động và kế hoạch phân bổ vốn trong 18 tháng tiếp theo.</li></ul><h3>2. Kịch bản thuyết trình mẫu 5 phút hoàn chỉnh của CEO EdTech (Model 5-Minute VC Pitch Script)</h3><div class=''pitch-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''>"Good morning, partners and investors. Did you know that over seventy percent of engineering graduates across emerging markets struggle to pass international technical interviews solely due to language communication barriers? That represents a staggering twenty-billion-dollar talent mismatch.<br/><br/>I am Dr. Minh Tran, Co-Founder and CEO of LinguaAI. We have built an adaptive, voice-driven language intelligence platform specifically engineered for software professionals. Unlike generic language learning apps that teach casual tourist phrases, LinguaAI embeds users into simulated daily Scrum meetings, code reviews, and executive product pitching sessions.<br/><br/>Over the past ten months, our platform has demonstrated tremendous organic traction: we have scaled to 120,000 active monthly learners across six countries, achieving $180,000 in monthly recurring revenue with an industry-leading 82% annual retention rate. Our customer acquisition cost stands at just $18, while our customer lifetime value exceeds $210, yielding an exceptional 11.6x LTV to CAC ratio.<br/><br/>Today, we are raising a $3.5 million Series A round to accelerate our enterprise sales expansion across Southeast Asia and enhance our proprietary real-time voice feedback engine. We invite you to join us in unlocking the global potential of millions of skilled professionals. Thank you very much!"</div></div>', 35, 1),

-- CNTT (IT English)
(@cdThuatNguIT, @tdB1, @gvId, N'Từ vựng tiếng Anh Chuyên ngành Kỹ thuật Phần mềm & Cơ sở dữ liệu', N'TuVung',
 N'Thuật ngữ chuyên sâu: Microservices, Concurrency, Indexing, ORM, Refactoring và CI/CD Pipeline.',
 N'<div class=''lesson-content''><h2>Từ Vựng Tiếng Anh Chuyên Ngành Kỹ Thuật Phần Mềm & Cơ Sở Dữ Liệu</h2><p class=''lead''>Nắm vững hệ thống thuật ngữ công nghệ thông tin chuẩn quốc tế giúp kỹ sư phần mềm tự tin đọc tài liệu kỹ thuật, trao đổi trong pull request và làm việc hiệu quả với các chuyên gia toàn cầu.</p><h3>1. Danh mục thuật ngữ then chốt (Core IT Lexicon)</h3><div class=''vocab-grid''><p><strong>1. Throughput (/ˈθruː.pʊt/) - [Noun]:</strong> Băng thông xử lý dữ liệu của hệ thống trong một đơn vị thời gian.<br/><em>Example:</em> "By optimizing our connection pooling, database throughput increased by 60% under peak load."</p><p><strong>2. Latency (/ˈleɪ.tən.si/) - [Noun]:</strong> Độ trễ thời gian kể từ khi gửi yêu cầu đến khi nhận phản hồi.<br/><em>Example:</em> "Deploying edge caching servers significantly reduced network latency for overseas users."</p><p><strong>3. Scalability (/ˌskeɪ.ləˈbɪl.ə.ti/) - [Noun]:</strong> Khả năng mở rộng linh hoạt của hệ thống khi khối lượng tải tăng vọt.<br/><em>Example:</em> "Cloud-native microservices architecture provides horizontal scalability to handle unexpected traffic spikes."</p><p><strong>4. Fault Tolerance (/fɒlt ˈtɒl.ər.əns/) - [Noun]:</strong> Khả năng chịu lỗi và duy trì hoạt động ngay cả khi có thành phần bị sập.<br/><em>Example:</em> "Our disaster recovery strategy includes redundant database replicas to ensure strict fault tolerance."</p><p><strong>5. Refactoring (/ˌriːˈfæk.tər.ɪŋ/) - [Noun]:</strong> Tái cấu trúc mã nguồn để tối ưu cấu trúc nội bộ mà không làm thay đổi hành vi bên ngoài.<br/><em>Example:</em> "We dedicated the first two days of the sprint to refactoring legacy payment calculation modules."</p></div></div>', 25, 1),

(@cdAgile, @tdB1, @gvId, N'Giao tiếp trong Buổi họp Daily Standup và Báo cáo Tiến độ Sprint', N'Doc',
 N'Mẫu câu 3 câu hỏi kinh điển trong Daily Standup: Đã làm gì hôm qua? Làm gì hôm nay? Có gặp khó khăn (blocker) gì không?',
 N'<div class=''lesson-content''><h2>Giao Tiếp Hiệu Quả Trong Buổi Họp Daily Standup & Báo Cáo Tiến Độ Sprint</h2><p class=''lead''>Cuộc họp Daily Standup (15 phút mỗi ngày) là nghi thức xương sống của phương pháp Agile Scrum. Giao tiếp ngắn gọn, chính xác và có trách nhiệm giúp toàn đội ngũ duy trì sự ăn khớp hoàn hảo.</p><h3>1. Cấu trúc 3 câu hỏi vàng (The Three Golden Questions)</h3><ol><li><strong>Question 1: What did you accomplish yesterday?</strong><br/>- Tập trung vào kết quả cụ thể đã hoàn thành, không kể lể chi tiết vụn vặt.<br/><em>Mẫu câu chuẩn:</em> "Yesterday, I finalized the API rate limiting logic and deployed it to the staging server for QA testing."</li><li><strong>Question 2: What will you work on today?</strong><br/>- Nêu rõ mục tiêu trọng tâm trong ngày để mọi người nắm bắt được luồng công việc.<br/><em>Mẫu câu chuẩn:</em> "Today, my main priority is to write comprehensive integration tests for the Stripe checkout webhook."</li><li><strong>Question 3: Are there any impediments or blockers in your way?</strong><br/>- Nêu rõ các rào cản phụ thuộc kỹ thuật hoặc chờ đợi phản hồi từ phòng ban khác.<br/><em>Mẫu câu chuẩn:</em> "I currently have one minor blocker: I am awaiting API sandbox credentials from the payment partner. John, could you help escalate this?"</li></ol></div>', 20, 1);
GO

-- ===================================================================
-- 7. NẠP BẢNG TỪ VỰNG & FLASHCARD (TuVung & Flashcard)
-- ===================================================================
DECLARE @tdA1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A1');
DECLARE @tdA2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A2');
DECLARE @tdB1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B1');
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');
DECLARE @tdC1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'C1');

DECLARE @cdGiaoTiep INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Giao Tiếp%');
DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS%');
DECLARE @cdGrammar INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Ngữ Pháp%');

INSERT INTO TuVung (MaChuDe, MaTrinhDo, Tu, LoaiTu, PhienAmIPA, Nghia, CauViDu, CumTuDiKem)
VALUES
(@cdIELTS, @tdC1, N'Ubiquitous', N'adj', N'/juːˈbɪk.wɪ.təs/', 
 N'Phổ biến ở khắp mọi nơi, hiện diện đồng thời ở mọi chỗ (seeming to be everywhere).', 
 N'"Smartphones have become ubiquitous in daily modern life, radically transforming how students study."', 
 N'• Ubiquitous influence • Ubiquitous presence of technology'),

(@cdIELTS, @tdC1, N'Meticulous', N'adj', N'/məˈtɪk.jə.ləs/', 
 N'Tỉ mỉ, cẩn thận từng chi tiết nhỏ nhất (very careful and with great attention to every detail).', 
 N'"The research paper required meticulous attention to detail to avoid statistical discrepancies."', 
 N'• Meticulous research • Meticulous planning'),

(@cdIELTS, @tdB2, N'Pragmatic', N'adj', N'/præɡˈmæt.ɪk/', 
 N'Thực tế, thực dụng, dựa trên tính khả thi thay vì lý thuyết suông.', 
 N'"In dealing with environmental issues, governments must take a pragmatic and balanced approach."', 
 N'• Pragmatic solution • Pragmatic approach'),

(@cdIELTS, @tdC1, N'Corroborate', N'verb', N'/kəˈrɒb.ə.reɪt/', 
 N'Chứng minh, củng cố thêm bằng chứng xác thực (to confirm or support with new evidence).', 
 N'"Subsequent geological studies corroborated the theory proposed by scientists a decade earlier."', 
 N'• Corroborate evidence • Corroborate findings'),

(@cdIELTS, @tdB2, N'Detrimental', N'adj', N'/ˌdet.rɪˈmen.təl/', 
 N'Gây hại, có hại (causing harm or damage).', 
 N'"Excessive screen time before sleeping is proven to have a detrimental effect on sleep quality."', 
 N'• Detrimental effect • Detrimental impact'),

(@cdIELTS, @tdB2, N'Inevitable', N'adj', N'/ɪnˈev.ɪ.tə.bəl/', 
 N'Không thể tránh khỏi, chắc chắn sẽ xảy ra.', 
 N'"With the rapid growth of AI, changes in the job market are inevitable."', 
 N'• Inevitable consequence • Inevitable outcome'),

(@cdIELTS, @tdB1, N'Resilient', N'adj', N'/rɪˈzɪl.jənt/', 
 N'Kiên cường, có khả năng phục hồi nhanh sau khó khăn hoặc nghịch cảnh.', 
 N'"Local communities showed themselves to be remarkably resilient after the severe storm."', 
 N'• Highly resilient • Resilient economy'),

(@cdIELTS, @tdB2, N'Feasible', N'adj', N'/ˈfiː.zə.bəl/', 
 N'Khả thi, có thể thực hiện được một cách thực tế.', 
 N'"It is not economically feasible to replace all fossil fuel power plants overnight."', 
 N'• Technically feasible • Feasible alternative'),

(@cdIELTS, @tdB2, N'Ambiguous', N'adj', N'/æmˈbɪɡ.ju.əs/', 
 N'Mơ hồ, có thể hiểu theo nhiều nghĩa khác nhau.', 
 N'"The instructions given by the supervisor were ambiguous and caused significant confusion."', 
 N'• Ambiguous statement • Ambiguous wording'),

(@cdIELTS, @tdB1, N'Procrastinate', N'verb', N'/prəˈkræs.tɪ.neɪt/', 
 N'Trì hoãn, chần chừ không làm việc cần làm ngay.', 
 N'"Most students tend to procrastinate until the night before the final assignment is due."', 
 N'• Procrastinate on homework • Chronic procrastination'),

(@cdGiaoTiep, @tdA1, N'Enthusiastic', N'adj', N'/ɪnˌθjuː.ziˈæs.tɪk/', 
 N'Nhiệt tình, hăng hái, hào hứng.', 
 N'"The new teacher received an enthusiastic welcome from all the students."', 
 N'• Enthusiastic support • Enthusiastic about learning'),

(@cdGiaoTiep, @tdA2, N'Diligent', N'adj', N'/ˈdɪl.ɪ.dʒənt/', 
 N'Chăm chỉ, siêng năng, cần cù.', 
 N'"She was a diligent student who consistently reviewed her lecture notes every evening."', 
 N'• Diligent worker • Diligent effort');
GO

-- ===================================================================
-- 8. NẠP CÂU HỎI TRẮC NGHIỆM & LỰA CHỌN ĐÁP ÁN (CauHoi & LuaChonDapAn)
-- ===================================================================
DECLARE @tdA1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A1');
DECLARE @tdA2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A2');
DECLARE @tdB1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B1');
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');
DECLARE @tdC1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'C1');

DECLARE @cdGrammar INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Ngữ Pháp%');
DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS%');
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = 1;

-- Câu 1 (A1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA1, N'TracNghiem', N'NguPhap', N'Choose the correct verb form: She ______ to school by bus every morning.', 
        N'Chủ ngữ ngôi thứ ba số ít "She" trong thì hiện tại đơn đi với động từ thêm -s/-es ("goes").', @gvId, N'ThuCong', 1);
DECLARE @q1 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q1, 'A', N'go', 0), (@q1, 'B', N'goes', 1), (@q1, 'C', N'going', 0), (@q1, 'D', N'went', 0);

-- Câu 2 (A1 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA1, N'TracNghiem', N'TuVung', N'I usually have bread and eggs for ______ at 7:00 AM.', 
        N'"Breakfast" là bữa ăn sáng vào lúc 7:00 AM.', @gvId, N'ThuCong', 1);
DECLARE @q2 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q2, 'A', N'dinner', 0), (@q2, 'B', N'lunch', 0), (@q2, 'C', N'breakfast', 1), (@q2, 'D', N'supper', 0);

-- Câu 3 (A2 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'NguPhap', N'Yesterday we ______ a very interesting documentary about marine life.', 
        N'Trạng từ thời gian "Yesterday" báo hiệu thì quá khứ đơn ("watched").', @gvId, N'ThuCong', 1);
DECLARE @q3 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q3, 'A', N'watch', 0), (@q3, 'B', N'watches', 0), (@q3, 'C', N'watched', 1), (@q3, 'D', N'watching', 0);

-- Câu 4 (A2 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'TuVung', N'This suitcase is too ______ for me to carry alone. Can you help me?', 
        N'"Heavy" mang nghĩa nặng nề, phù hợp ngữ cảnh cần người giúp đỡ.', @gvId, N'ThuCong', 1);
DECLARE @q4 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q4, 'A', N'light', 0), (@q4, 'B', N'heavy', 1), (@q4, 'C', N'bright', 0), (@q4, 'D', N'soft', 0);

-- Câu 5 (B1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'NguPhap', N'If I ______ enough money, I would travel around the world next summer.', 
        N'Câu điều kiện loại 2 giả định điều không có thật ở hiện tại: If + S + V2/ed, S + would + V.', @gvId, N'ThuCong', 1);
DECLARE @q5 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q5, 'A', N'have', 0), (@q5, 'B', N'had', 1), (@q5, 'C', N'will have', 0), (@q5, 'D', N'have had', 0);

-- Câu 6 (B1 Reading)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'Doc', 
 N'According to the passage, why do owls prefer hunting at night?', 
 N'Owls are nocturnal birds of prey renowned for their extraordinary night vision and silent flight. Unlike daylight predators such as eagles and falcons, owls possess large, forward-facing eyes with a high concentration of rod cells that capture minimal ambient light. Furthermore, hunting under the cover of darkness significantly reduces competition for small rodents.',
 N'Đoạn văn nêu rõ: bóng tối giúp cú tránh sự cạnh tranh săn bắt các loài gặm nhấm từ các loài săn mồi ban ngày.', 
 @gvId, N'ThuCong', 1);
DECLARE @q6 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q6, 'A', N'They cannot tolerate warm sunshine.', 0),
(@q6, 'B', N'Their specialized senses excel in the dark and they avoid daytime competitors.', 1),
(@q6, 'C', N'Rodents are only active during morning hours.', 0),
(@q6, 'D', N'They prefer socializing in darkness.', 0);

-- Câu 7 (B2 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'TuVung', N'The committee decided to ______ stricter environmental policies to curb industrial carbon emissions.', 
        N'Cụm từ "implement policies" có nghĩa là thi hành/áp dụng chính sách.', @gvId, N'ThuCong', 1);
DECLARE @q7 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q7, 'A', N'implement', 1), (@q7, 'B', N'compliment', 0), (@q7, 'C', N'supplement', 0), (@q7, 'D', N'experiment', 0);

-- Câu 8 (B2 Grammar - Inversion)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'Seldom ______ such a breathtaking display of aurora borealis in this region.', 
        N'Đảo ngữ với trạng từ phủ định đứng đầu câu: Seldom + trợ động từ + S + V chính ("have we witnessed").', @gvId, N'ThuCong', 1);
DECLARE @q8 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q8, 'A', N'we have witnessed', 0), (@q8, 'B', N'have we witnessed', 1), (@q8, 'C', N'we witnessed', 0), (@q8, 'D', N'had witnessed we', 0);

-- Câu 9 (C1 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'Subsequent empirical studies completely ______ the original hypothesis proposed by Dr. Vance.', 
        N'"Corroborate" nghĩa là chứng thực, củng cố bằng chứng cho giả thuyết.', @gvId, N'ThuCong', 1);
DECLARE @q9 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q9, 'A', N'corroborated', 1), (@q9, 'B', N'retracted', 0), (@q9, 'C', N'dismantled', 0), (@q9, 'D', N'diminished', 0);

-- Câu 10 (C1 Reading)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'The author uses the phrase "double-edged sword" to emphasize that artificial intelligence:', 
 N'The integration of generative artificial intelligence across industrial workflows represents a double-edged sword. On one hand, automated synthesis and code generation accelerate throughput exponentially. Conversely, intellectual property vulnerabilities and cognitive atrophy among junior engineers present formidable existential hurdles for long-term innovation.', 
 N'"Double-edged sword" (con dao hai lưỡi) biểu thị cả lợi ích to lớn (tăng tốc độ) lẫn rủi ro tiềm tàng (vấn đề sở hữu trí tuệ, suy giảm tư duy).',
 @gvId, N'ThuCong', 1);
DECLARE @q10 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q10, 'A', N'is entirely hazardous and must be prohibited immediately', 0),
(@q10, 'B', N'delivers immense productivity gains while simultaneously introducing substantial perils', 1),
(@q10, 'C', N'has surpassed human cognition in ethical discernment', 0),
(@q10, 'D', N'is exclusively beneficial for junior software developers', 0);

-- Câu 11 (B2 Grammar - Subjunctive)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'The doctor recommended that the patient ______ adequate rest and avoid heavy lifting.', 
        N'Cấu trúc giả định thức (Subjunctive): recommend that S + (should) V-nguyên thể ("take").', @gvId, N'ThuCong', 1);
DECLARE @q11 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q11, 'A', N'takes', 0), (@q11, 'B', N'took', 0), (@q11, 'C', N'take', 1), (@q11, 'D', N'will take', 0);

-- Câu 12 (B2 Vocabulary - Collocations)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'TuVung', N'The prolonged drought had a ______ effect on agricultural output throughout the province.', 
        N'"Detrimental effect" là cụm từ cố định (collocation) mang nghĩa tác động tiêu cực, gây hại.', @gvId, N'ThuCong', 1);
DECLARE @q12 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q12, 'A', N'beneficial', 0), (@q12, 'B', N'detrimental', 1), (@q12, 'C', N'constructive', 0), (@q12, 'D', N'superficial', 0);

-- Câu 13 (C1 Inversion - Conditionals)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'______ they followed the emergency safety protocol, the catastrophic leak would have been averted.', 
        N'Đảo ngữ câu điều kiện loại 3: Had + S + V3/ed thay cho If + S + had + V3/ed.', @gvId, N'ThuCong', 1);
DECLARE @q13 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q13, 'A', N'If had', 0), (@q13, 'B', N'Should', 0), (@q13, 'C', N'Had', 1), (@q13, 'D', N'Were', 0);

-- Câu 14 (B1 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'TuVung', N'It is not economically ______ to construct a bullet train through such mountainous terrain.', 
        N'"Feasible" có nghĩa là khả thi, thực hiện được.', @gvId, N'ThuCong', 1);
DECLARE @q14 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q14, 'A', N'feasible', 1), (@q14, 'B', N'fragile', 0), (@q14, 'C', N'furious', 0), (@q14, 'D', N'fictional', 0);

-- Câu 15 (A2 Grammar - Comparatives)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'NguPhap', N'Traveling by high-speed train is much ______ than taking the overnight bus.', 
        N'So sánh hơn của tính từ ngắn "fast" là "faster", có thể nhấn mạnh bằng "much".', @gvId, N'ThuCong', 1);
DECLARE @q15 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q15, 'A', N'more fast', 0), (@q15, 'B', N'faster', 1), (@q15, 'C', N'fastest', 0), (@q15, 'D', N'as fast', 0);
GO

-- ===================================================================
-- 9. NẠP BÀI KHẢO SÁT ĐẦU VÀO & CÂU HỎI KHẢO SÁT (BaiKhaoSatDauVao & CauHoiKhaoSat)
-- ===================================================================
INSERT INTO BaiKhaoSatDauVao (TenBaiKhaoSat, MoTa, TongSoCauHoi, DangHoatDong, NgayTao)
VALUES (N'Khảo sát Đánh giá Năng lực Đầu vào Chuẩn Quốc Tế CEFR', 
        N'Bài thi chẩn đoán phân loại trình độ từ A1 đến C1 giúp hệ thống cá nhân hóa lộ trình học.', 15, 1, SYSDATETIME());
DECLARE @maBKS INT = SCOPE_IDENTITY();

-- Liên kết toàn bộ câu hỏi vào bài khảo sát
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi)
SELECT @maBKS, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC)
FROM CauHoi;
GO

-- ===================================================================
-- 10. NẠP ĐỀ THI THỬ & LUYỆN TẬP (BaiTapDeThi & CauHoiBaiTap)
-- ===================================================================
DECLARE @tdA1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A1');
DECLARE @tdA2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A2');
DECLARE @tdB1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B1');
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');
DECLARE @tdC1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'C1');
DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS%');
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = 1;

-- Đề A1: A1 Starter Diagnostic
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, TrangThai, NguoiTao)
VALUES (@cdIELTS, @tdA1, N'A1 Starter Diagnostic: Phản Xạ Từ Vựng & Ngữ Pháp Căn Bản', N'ThiThu', 
        N'Kiểm tra phản xạ từ vựng cơ bản, động từ To Be, thì hiện tại đơn và câu chào hỏi thường nhật.', 20, 1, N'DaXuatBan', @gvId);
DECLARE @deA1 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deA1, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaTrinhDo = @tdA1 OR MaTrinhDo IS NULL;

-- Đề A2: A2 Elementary Reading & Grammar
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, TrangThai, NguoiTao)
VALUES (@cdIELTS, @tdA2, N'A2 Elementary Reading & Grammar: Thì Quá Khứ & So Sánh', N'ThiThu', 
        N'Part 5-6 định dạng TOEIC Bridge: Động từ bất quy tắc, thì quá khứ đơn và cấu trúc so sánh.', 30, 1, N'DaXuatBan', @gvId);
DECLARE @deA2 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deA2, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaTrinhDo = @tdA2 OR MaTrinhDo IS NULL;

-- Đề B1: TOEIC Reading Comprehension
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, TrangThai, NguoiTao)
VALUES (@cdIELTS, @tdB1, N'TOEIC Reading Comprehension Diagnostic Test', N'ThiThu', 
        N'Part 5 (Hoàn thành câu), Part 6 (Điền đoạn văn) và Part 7 (Đọc hiểu văn bản thương mại).', 45, 1, N'DaXuatBan', @gvId);
DECLARE @deB1 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deB1, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 10 MaCauHoi FROM CauHoi ORDER BY MaCauHoi ASC);

-- Đề B2: IELTS Academic Reading & Listening Mini Mock Test #05
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, TrangThai, NguoiTao)
VALUES (@cdIELTS, @tdB2, N'IELTS Academic Reading & Listening Mini Mock Test #05', N'ThiThu', 
        N'Bao gồm bài đọc Skimming/Scanning, phân tích từ vựng học thuật C1 và cấu trúc ngữ pháp đảo ngữ.', 40, 1, N'DaXuatBan', @gvId);
DECLARE @deB2 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deB2, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 10 MaCauHoi FROM CauHoi ORDER BY MaCauHoi DESC);

-- Đề B2: Ngữ pháp chuyên sâu B2
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, TrangThai, NguoiTao)
VALUES (@cdIELTS, @tdB2, N'Kiểm tra Đột phá Ngữ pháp & Collocations Band B2', N'ThiThu', 
        N'25 câu trắc nghiệm câu điều kiện đảo ngữ, mệnh đề quan hệ rút gọn, liên từ nâng cao.', 25, 1, N'DaXuatBan', @gvId);
DECLARE @deB2_2 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deB2_2, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 10 MaCauHoi FROM CauHoi ORDER BY NEWID());

-- Đề C1: IELTS Advanced Academic Mock Test
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, TrangThai, NguoiTao)
VALUES (@cdIELTS, @tdC1, N'IELTS Advanced Academic Mock Test: Phân Tích Luận & Tư Duy Phản Biện', N'ThiThu', 
        N'Bài thi phân tích học thuật đỉnh cao: Đoạn văn nghiên cứu khoa học, cấu trúc câu phức và từ vựng C1-C2.', 45, 1, N'DaXuatBan', @gvId);
DECLARE @deC1 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deC1, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaTrinhDo = @tdC1 OR MaTrinhDo IS NULL;

-- Đề 4: Luyện tập phản xạ nhanh 60s
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, TrangThai, NguoiTao)
VALUES (@cdIELTS, @tdB1, N'Thử Thách Phản Xạ Trắc Nghiệm 60s', N'LuyenTap', 
        N'Luyện tập phản xạ nhanh từ vựng và ngữ pháp tiếng Anh trong 60 giây.', 10, 1, N'DaXuatBan', @gvId);
DECLARE @de4 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @de4, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 5 MaCauHoi FROM CauHoi ORDER BY MaCauHoi ASC);
GO

-- ===================================================================
-- 11. NẠP YÊU CẦU SINH ĐỀ AI VÀ CÂU HỎI AI SINH (YeuCauSinhDeAI & CauHoiDoAISinh)
-- ===================================================================
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDangNhap = N'tung.hoang');
IF @gvId IS NULL SET @gvId = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = 1;

DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');

IF @gvId IS NOT NULL AND @tdB2 IS NOT NULL
BEGIN
    INSERT INTO YeuCauSinhDeAI (NguoiYeuCau, LoaiDauVao, NoiDungDauVao, MaTrinhDoMongMuon, SoLuongCauHoi, TrangThai, MoHinhAI)
    VALUES
    (@gvId, N'VanBan', N'Bài đọc về biến đổi khí hậu và năng lượng tái tạo', @tdB2, 4, N'DangXuLy', N'Gemini-Pro-1.5');
    DECLARE @ycId BIGINT = SCOPE_IDENTITY();

    IF @ycId IS NOT NULL
    BEGIN
        INSERT INTO CauHoiDoAISinh (MaYeuCauSinhDe, NoiDungCauHoiSinhRa, TrangThaiDuyet)
        VALUES
        (@ycId, N'What is the main topic of the passage regarding renewable energy?', N'DangCho'),
        (@ycId, N'According to paragraph 2, why are solar cells becoming more efficient?', N'DangCho'),
        (@ycId, N'The word "resilience" in line 14 is closest in meaning to...', N'DangCho'),
        (@ycId, N'Which of the following statements is NOT mentioned in the text?', N'DangCho');
    END
END
GO

-- ===================================================================
-- 12. NẠP DỮ LIỆU BỘ CÂU HỎI BÀI HỌC VÀ KẾT QUẢ HỌC TẬP AI
--     (BoCauHoi, BoCauHoi_CauHoi, KetQuaLamBaiBoCauHoi, NoiDungHocTapAIBaiHoc)
-- ===================================================================
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDangNhap = N'tung.hoang');
IF @gvId IS NULL SET @gvId = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = 1;

DECLARE @hvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDangNhap = N'lan.nguyen');
IF @hvId IS NULL SET @hvId = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'HocVien'));

DECLARE @bhChaoHoi INT = (SELECT TOP 1 MaBaiHoc FROM BaiHoc WHERE TieuDe LIKE N'%Chào hỏi%');
DECLARE @cdChaoHoi INT = (SELECT TOP 1 MaChuDe FROM BaiHoc WHERE MaBaiHoc = @bhChaoHoi);
DECLARE @tdA1 INT = (SELECT TOP 1 MaTrinhDo FROM BaiHoc WHERE MaBaiHoc = @bhChaoHoi);

-- 12.1 Nạp phân tích bài học AI (NoiDungHocTapAIBaiHoc)
IF @bhChaoHoi IS NOT NULL AND NOT EXISTS (SELECT 1 FROM NoiDungHocTapAIBaiHoc WHERE MaBaiHoc = @bhChaoHoi)
BEGIN
    INSERT INTO NoiDungHocTapAIBaiHoc (MaBaiHoc, ContentHash, TrangThai, KyNang, MucTieu, DuLieuKetQuaJson, MoHinhAI, NgayTao, NgayCapNhat)
    VALUES (
        @bhChaoHoi,
        N'EAI_HASH_CHAO_HOI_A1_INIT',
        N'HoanThanh',
        N'Giao tiếp, Nghe, Nói',
        N'Làm chủ các mẫu câu chào hỏi trang trọng và thân mật, tự tin giới thiệu bản thân trong môi trường học thuật và doanh nghiệp.',
        N'{"overview":"Bài học nền tảng giới thiệu quy chuẩn chào hỏi và giới thiệu bản thân 4 bước.","keyPhrases":["Good morning/afternoon","Pleasure to meet you","My name is...","I specialize in..."],"practicePoints":"Luyện tập ngữ điệu tự nhiên và phát âm rõ ràng trọng âm."}',
        N'Gemini-1.5-Pro',
        SYSDATETIME(),
        SYSDATETIME()
    );
END

-- 12.2 Nạp bộ câu hỏi củng cố bài học (BoCauHoi & BoCauHoi_CauHoi)
IF @bhChaoHoi IS NOT NULL
BEGIN
    INSERT INTO BoCauHoi (TenBoCauHoi, MoTa, NguonTao, TenNguon, MaBaiHoc, MaChuDe, MaTrinhDo, NguoiTao, ContentHash, LoaiBoCauHoi, TrangThai)
    VALUES (
        N'Bộ câu hỏi củng cố: Chào hỏi & Tự giới thiệu bản thân',
        N'10 câu hỏi trắc nghiệm rèn luyện phản xạ chào hỏi theo ngữ cảnh và tự giới thiệu bản thân chuyên nghiệp.',
        N'AI',
        N'Gemini Flash 1.5',
        @bhChaoHoi,
        @cdChaoHoi,
        @tdA1,
        @gvId,
        N'EAI_HASH_CHAO_HOI_A1_INIT',
        N'CungCo',
        N'HoanThanh'
    );
    DECLARE @boChaoHoiId INT = SCOPE_IDENTITY();

    -- Gán các câu hỏi trắc nghiệm vào bộ câu hỏi
    INSERT INTO BoCauHoi_CauHoi (MaBoCauHoi, MaCauHoi, ThuTu)
    SELECT @boChaoHoiId, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC)
    FROM (SELECT TOP 4 MaCauHoi FROM CauHoi ORDER BY MaCauHoi ASC) ch;

    -- 12.3 Nạp kết quả làm bài tập củng cố của sinh viên mẫu (KetQuaLamBaiBoCauHoi)
    IF @hvId IS NOT NULL AND @boChaoHoiId IS NOT NULL
    BEGIN
        INSERT INTO KetQuaLamBaiBoCauHoi (MaNguoiDung, MaBoCauHoi, MaBaiHoc, LoaiBoCauHoi, ContentHash, TongSoCau, SoCauDung, DiemSo, ThoiGianLamGiay, TrangThai, ChiTietTraLoiJson, NgayNop)
        VALUES (
            @hvId,
            @boChaoHoiId,
            @bhChaoHoi,
            N'CungCo',
            N'EAI_HASH_CHAO_HOI_A1_INIT',
            4,
            4,
            10.0,
            115,
            N'DaNop',
            N'[{"cauHoi":1,"daChon":"A","dung":true},{"cauHoi":2,"daChon":"B","dung":true},{"cauHoi":3,"daChon":"C","dung":true},{"cauHoi":4,"daChon":"A","dung":true}]',
            DATEADD(hour, -3, SYSDATETIME())
        );
    END
END
GO


-- ===================================================================
-- 13. Há»† THá»NG CÃ‚U Há»ŽI PHONG PHÃš VÃ€ CÃC Äá»€ THI THá»¬ & GÃ“I LUYá»†N Táº¬P
-- ===================================================================
GO

-- ===================================================================
-- Script: Add_Diverse_Questions_And_Exams.sql
-- Bổ sung hệ thống câu hỏi phong phú (Ngữ pháp, Từ vựng, Đọc hiểu, Viết - KHÔNG bao gồm Nghe)
-- và tạo các Đề thi thử (ThiThu) cùng Gói luyện tập (LuyenTap) chuẩn hóa cho học viên
-- ===================================================================
USE TiengAnhAI;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = 1;

DECLARE @tdA1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A1');
DECLARE @tdA2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A2');
DECLARE @tdB1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B1');
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');
DECLARE @tdC1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'C1');

DECLARE @cdGrammar INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Ngữ Pháp%');
IF @cdGrammar IS NULL SET @cdGrammar = 9;
DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Học Thuật%' OR TenChuDe LIKE N'%IELTS%');
IF @cdIELTS IS NULL SET @cdIELTS = 5;
DECLARE @cdBusiness INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Thương Mại%');
IF @cdBusiness IS NULL SET @cdBusiness = 13;
DECLARE @cdDaily INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Giao Tiếp%');
IF @cdDaily IS NULL SET @cdDaily = 1;

-- Bảng tạm lưu danh sách mã câu hỏi vừa tạo để phân phối vào các đề
DECLARE @NewQuestions TABLE (
    Id INT IDENTITY(1,1),
    MaCauHoi INT,
    KyNang NVARCHAR(30),
    MaTrinhDo INT,
    DoKho NVARCHAR(20)
);

-- ===================================================================
-- 1. BỘ CÂU HỎI NGỮ PHÁP CHUYÊN SÂU (15 CÂU: A1 -> C1)
-- ===================================================================

-- Q1 (A1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdA1, N'TracNghiem', N'NguPhap', N'Every morning, David ______ coffee with milk before going to work.', 
        N'Thì hiện tại đơn diễn tả thói quen hàng ngày với chủ ngữ số ít "David" động từ thêm -s/es ("drinks").', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
DECLARE @q INT = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdA1, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'drinks', 1), (@q, 'B', N'drinking', 0), (@q, 'C', N'drink', 0), (@q, 'D', N'is drink', 0);

-- Q2 (A2 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'NguPhap', N'While we ______ dinner last night, the power suddenly went out.', 
        N'Thì quá khứ tiếp diễn diễn tả hành động đang diễn ra (were having) thì hành động khác chen ngang ở quá khứ đơn (went out).', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdA2, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'had', 0), (@q, 'B', N'were having', 1), (@q, 'C', N'have had', 0), (@q, 'D', N'are having', 0);

-- Q3 (A2 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'NguPhap', N'This laptop is ______ than the one I bought two years ago.', 
        N'So sánh hơn của tính từ dài: more + adj + than ("more reliable").', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdA2, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'more reliable', 1), (@q, 'B', N'reliabler', 0), (@q, 'C', N'most reliable', 0), (@q, 'D', N'as reliable', 0);

-- Q4 (B1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'NguPhap', N'She has lived in London ______ she graduated from university in 2018.', 
        N'Dùng "since" mốc thời gian / mệnh đề quá khứ đơn đi cùng thì hiện tại hoàn thành.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'for', 0), (@q, 'B', N'since', 1), (@q, 'C', N'when', 0), (@q, 'D', N'during', 0);

-- Q5 (B1 Grammar - Passive)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'NguPhap', N'The new bridge ______ by international engineers before the rainy season starts.', 
        N'Thể bị động của tương lai đơn hoặc modal verb: will be completed.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will complete', 0), (@q, 'B', N'will be completed', 1), (@q, 'C', N'completes', 0), (@q, 'D', N'is completing', 0);

-- Q6 (B1 Grammar - Conditionals)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'NguPhap', N'If it ______ tomorrow, we will have to reschedule our outdoor photography workshop.', 
        N'Câu điều kiện loại 1 diễn tả sự việc có thể xảy ra ở tương lai: If + S + V(hiện tại đơn), S + will + V.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'rains', 1), (@q, 'B', N'rained', 0), (@q, 'C', N'will rain', 0), (@q, 'D', N'is raining', 0);

-- Q7 (B2 Grammar - Reduced Relative Clause)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'The guidelines ______ at the annual symposium will be disseminated to all departments next week.', 
        N'Rút gọn mệnh đề quan hệ dạng bị động (which were approved -> approved).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'approving', 0), (@q, 'B', N'approved', 1), (@q, 'C', N'were approved', 0), (@q, 'D', N'approve', 0);

-- Q8 (B2 Grammar - Mixed Conditionals)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'If she had accepted that overseas scholarship last year, she ______ in Zurich right now.', 
        N'Câu điều kiện hỗn hợp (Mixed conditional 3 - 2): Giả định quá khứ (had accepted) dẫn đến kết quả ở hiện tại (would be living).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB2, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'would live', 0), (@q, 'B', N'would be living', 1), (@q, 'C', N'had lived', 0), (@q, 'D', N'will live', 0);

-- Q9 (B2 Grammar - Conjunctions)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'______ the unprecedented surge in raw material costs, the corporation managed to maintain its quarterly profit margin.', 
        N'"Despite" hoặc "In spite of" đi cùng danh từ hoặc cụm danh từ biểu thị sự đối lập nhượng bộ.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Although', 0), (@q, 'B', N'Despite', 1), (@q, 'C', N'Because', 0), (@q, 'D', N'Even though', 0);

-- Q10 (B2 Grammar - Modals of Deduction)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'The streets are soaking wet this morning. It ______ heavily throughout the night.', 
        N'Dự đoán có căn cứ chắc chắn ở quá khứ dùng "must have + V3/ed" (Chắc hẳn là đã mưa).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'should have rained', 0), (@q, 'B', N'must have rained', 1), (@q, 'C', N'could rain', 0), (@q, 'D', N'would rain', 0);

-- Q11 (C1 Grammar - Inversion with Hardly)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'Hardly ______ the stage when the enthusiastic audience erupted in thunderous applause.', 
        N'Cấu trúc đảo ngữ: Hardly had + S + V3/ed + when + S + V2/ed (Vừa mới... thì...).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'the pianist had stepped on', 0), (@q, 'B', N'had the pianist stepped on', 1), (@q, 'C', N'did the pianist step on', 0), (@q, 'D', N'was the pianist stepping on', 0);

-- Q12 (C1 Grammar - Inversion Conditional)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'______ any discrepancies arise during the fiscal audit, please report them immediately to the compliance officer.', 
        N'Đảo ngữ câu điều kiện loại 1: Should + S + V-nguyên thể thay cho If + S + V.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Were', 0), (@q, 'B', N'Should', 1), (@q, 'C', N'Had', 0), (@q, 'D', N'Unless', 0);

-- Q13 (C1 Grammar - Subjunctive with Insist)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'The board of directors insisted that the chief architect ______ present at the project review meeting.', 
        N'Thức giả định (Subjunctive): S + insist that + S + (should) be/V-inf.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'is', 0), (@q, 'B', N'was', 0), (@q, 'C', N'be', 1), (@q, 'D', N'will be', 0);

-- Q14 (C1 Grammar - Cleft Sentences)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'It was not until the subsequent validation trial was concluded ______ the researchers published their empirical findings.', 
        N'Câu chẻ nhấn mạnh thời gian: It is/was not until... that + S + V.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'when', 0), (@q, 'B', N'that', 1), (@q, 'C', N'which', 0), (@q, 'D', N'then', 0);

-- Q15 (C1 Grammar - Passive Gerund)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'The corporate executive objected to ______ questioned about the proprietary algorithm without legal counsel.', 
        N'Cấu trúc object to + V-ing, ở thể bị động: object to being + V3/ed.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'be', 0), (@q, 'B', N'being', 1), (@q, 'C', N'have been', 0), (@q, 'D', N'having', 0);

-- ===================================================================
-- 2. BỘ CÂU HỎI TỪ VỰNG & COLLOCATIONS (15 CÂU: A1 -> C1)
-- ===================================================================

-- Q16 (A1 Vocab)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdDaily, @tdA1, N'TracNghiem', N'TuVung', N'Can you please tell me the ______ to the nearest central railway station?', 
        N'Cụm từ "tell someone the way to..." có nghĩa là chỉ đường cho ai đến đâu.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdA1, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'way', 1), (@q, 'B', N'road', 0), (@q, 'C', N'street', 0), (@q, 'D', N'path', 0);

-- Q17 (A2 Vocab - Phrasal Verb)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdDaily, @tdA2, N'TracNghiem', N'TuVung', N'Don''t forget to ______ your coat because the temperature outside is dropping rapidly.', 
        N'"Put on" có nghĩa là mặc/mặc thêm áo quần vào người.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdA2, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'take off', 0), (@q, 'B', N'put on', 1), (@q, 'C', N'look after', 0), (@q, 'D', N'give up', 0);

-- Q18 (A2 Vocab)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdA2, N'TracNghiem', N'TuVung', N'The receptionist asked the visitor to fill out a short registration ______ at the front desk.', 
        N'"Registration form" là cụm danh từ thông dụng chỉ phiếu/mẫu đơn đăng ký.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdA2, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'form', 1), (@q, 'B', N'letter', 0), (@q, 'C', N'page', 0), (@q, 'D', N'card', 0);

-- Q19 (B1 Vocab - Collocation)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB1, N'TracNghiem', N'TuVung', N'All participants are kindly requested to ______ close attention to the safety briefing before boarding.', 
        N'Collocation: "pay close attention to" mang nghĩa chú ý lắng nghe kỹ.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'make', 0), (@q, 'B', N'pay', 1), (@q, 'C', N'give', 0), (@q, 'D', N'take', 0);

-- Q20 (B1 Vocab - Phrasal Verb)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'TuVung', N'The engineering team has ______ a groundbreaking solution to minimize energy loss in electrical grids.', 
        N'Phrasal verb: "come up with" có nghĩa là nảy ra, đề xuất ra một ý tưởng hoặc giải pháp mới.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'come up with', 1), (@q, 'B', N'run out of', 0), (@q, 'C', N'get along with', 0), (@q, 'D', N'put up with', 0);

-- Q21 (B1 Vocab)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'TuVung', N'Solar and wind power are increasingly popular forms of ______ energy around the globe.', 
        N'"Renewable energy" là thuật ngữ chỉ nguồn năng lượng tái tạo, có thể tái sinh.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'reusable', 0), (@q, 'B', N'renewable', 1), (@q, 'C', N'replaceable', 0), (@q, 'D', N'recyclable', 0);

-- Q22 (B2 Vocab - Collocation)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB2, N'TracNghiem', N'TuVung', N'After hours of heated debate, the negotiation team finally managed to ______ a unanimous consensus.', 
        N'Collocation: "reach a consensus" nghĩa là đạt được sự đồng thuận chung.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'make', 0), (@q, 'B', N'reach', 1), (@q, 'C', N'grasp', 0), (@q, 'D', N'hold', 0);

-- Q23 (B2 Vocab - Academic)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'TuVung', N'The provincial government pledged to allocate adequate funds to ______ the economic fallout of the natural disaster.', 
        N'"Mitigate" nghĩa là làm giảm nhẹ, xoa dịu hậu quả tiêu cực.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'mitigate', 1), (@q, 'B', N'escalate', 0), (@q, 'C', N'provoke', 0), (@q, 'D', N'aggravate', 0);

-- Q24 (B2 Vocab - Idiom)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'TuVung', N'She has been ______ all week in order to finish her doctoral dissertation before the deadline.', 
        N'Thành ngữ: "burning the midnight oil" nghĩa là thức khuya làm việc/học tập cật lực.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'breaking the ice', 0), (@q, 'B', N'burning the midnight oil', 1), (@q, 'C', N'barking up the wrong tree', 0), (@q, 'D', N'hitting the sack', 0);

-- Q25 (B2 Vocab - Word Choice)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB2, N'TracNghiem', N'TuVung', N'The project manager emphasized that strict compliance with safety regulations is ______ in this hazardous facility.', 
        N'"Paramount" nghĩa là tối quan trọng, đứng hàng đầu.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB2, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'trivial', 0), (@q, 'B', N'paramount', 1), (@q, 'C', N'redundant', 0), (@q, 'D', N'tentative', 0);

-- Q26 (C1 Vocab - Academic)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'Smartphones and wireless connectivity have become virtually ______ in modern metropolitan existence.', 
        N'"Ubiquitous" mang nghĩa phổ biến ở khắp mọi nơi, nhan nhản.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'ubiquitous', 1), (@q, 'B', N'sporadic', 0), (@q, 'C', N'ephemeral', 0), (@q, 'D', N'precarious', 0);

-- Q27 (C1 Vocab - Academic Collocation)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'The scientist took ______ care to ensure that all experimental variables were controlled impeccably.', 
        N'"Meticulous care" là cụm từ học thuật biểu thị sự cẩn trọng tỉ mỉ, kỹ lưỡng đến từng chi tiết.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'superficial', 0), (@q, 'B', N'meticulous', 1), (@q, 'C', N'negligent', 0), (@q, 'D', N'abrupt', 0);

-- Q28 (C1 Vocab - Idiom)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'His perceptive summary of the financial crisis really ______ and identified the systemic vulnerabilities.', 
        N'Thành ngữ: "hit the nail on the head" nghĩa là nói trúng phóc trọng tâm, giải thích chính xác vấn đề.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'missed the boat', 0), (@q, 'B', N'hit the nail on the head', 1), (@q, 'C', N'spilled the beans', 0), (@q, 'D', N'let sleeping dogs lie', 0);

-- Q29 (C1 Vocab - Advanced Verbs)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'The sudden revocation of the commercial trade treaty will inevitably ______ cross-border investment flows.', 
        N'"Impediment" / "impede" có nghĩa là cản trở, làm đình trệ sự tiến triển.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'impede', 1), (@q, 'B', N'foster', 0), (@q, 'C', N'expedite', 0), (@q, 'D', N'bolster', 0);

-- Q30 (C1 Vocab - Academic Contrast)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'The venture capitalist demonstrated a remarkably ______ mindset by prioritizing tangible cash flow over speculative hype.', 
        N'"Pragmatic" nghĩa là thực dụng, thực tế, dựa trên hành động và kết quả cụ thể.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'dogmatic', 0), (@q, 'B', N'pragmatic', 1), (@q, 'C', N'erratic', 0), (@q, 'D', N'utopian', 0);

-- ===================================================================
-- 3. BỘ CÂU HỎI ĐỌC HIỂU HỌC THUẬT & BÀI ĐỌC (14 CÂU: B1 -> C1)
-- ===================================================================

-- PASSAGE A (B1 Reading): Sustainable Tourism
DECLARE @passageA NVARCHAR(MAX) = N'Ecotourism has gained significant momentum in recent decades as travelers become more environmentally conscious. Unlike conventional mass tourism, which often strains local infrastructure and causes ecological degradation, ecotourism emphasizes environmental stewardship and cultural respect. Travelers visit pristine natural areas, support community-run conservation initiatives, and contribute directly to the local economy. However, experts caution that without stringent regulations, unchecked commercialization can turn sustainable destinations into overcrowded attractions, ultimately defeating the original conservation purpose.';

-- Q31 (Passage A - Q1)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'Doc', 
 N'According to the passage, how does ecotourism differ primarily from conventional mass tourism?', 
 @passageA, 
 N'Đoạn văn nêu rõ: du lịch sinh thái tập trung vào bảo vệ môi trường và tôn trọng văn hóa địa phương thay vì gây áp lực lên hạ tầng như du lịch đại chúng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It prioritizes luxury accommodations over local experiences.', 0),
(@q, 'B', N'It focuses on environmental protection and cultural respect.', 1),
(@q, 'C', N'It strictly caters to international researchers.', 0),
(@q, 'D', N'It avoids using local economic resources.', 0);

-- Q32 (Passage A - Q2)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'Doc', 
 N'What potential hazard do conservation experts warn against regarding ecotourism?', 
 @passageA, 
 N'Đoạn văn cảnh báo: nếu không có quy định nghiêm ngặt, thương mại hóa thiếu kiểm soát có thể khiến các điểm đến trở nên quá tải.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Overcrowding and commercialization without strict regulations.', 1),
(@q, 'B', N'A total loss of traditional farming techniques.', 0),
(@q, 'C', N'An immediate ban on international air transportation.', 0),
(@q, 'D', N'High expenditure on digital marketing campaigns.', 0);

-- Q33 (Passage A - Q3)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'Doc', 
 N'The word "pristine" in the passage is closest in meaning to:', 
 @passageA, 
 N'"Pristine" mang nghĩa nguyên sơ, hoang sơ, chưa bị con người hay ô nhiễm làm hư hại (unspoiled).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'crowded', 0), (@q, 'B', N'unspoiled', 1), (@q, 'C', N'industrialized', 0), (@q, 'D', N'expensive', 0);

-- PASSAGE B (B2 Reading): AI in Diagnostic Medicine
DECLARE @passageB NVARCHAR(MAX) = N'The convergence of deep learning architectures and high-resolution imaging has transformed medical diagnostics. Computer vision algorithms can now analyze thousands of radiological scans within seconds, detecting micro-anomalies such as early-stage pulmonary nodules that human clinicians might overlook during fatigued night shifts. Despite these monumental achievements, computer scientists and medical ethicists stress that artificial intelligence should augment rather than supplant human physicians. Algorithmic outputs are inherently susceptible to training bias, and machines lack the nuanced bedside empathy essential for holistic patient care and complex palliative counseling.';

-- Q34 (Passage B - Q1)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Doc', 
 N'What is a primary advantage of deep learning algorithms in radiology highlighted in the text?', 
 @passageB, 
 N'Văn bản chỉ ra rằng thuật toán có thể phân tích hàng nghìn ảnh chụp trong vài giây và phát hiện những bất thường siêu nhỏ.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'They perform surgeries without human assistance.', 0),
(@q, 'B', N'They rapidly scan massive image sets and detect microscopic anomalies.', 1),
(@q, 'C', N'They completely eliminate all diagnostic expenses.', 0),
(@q, 'D', N'They formulate medication formulas independently.', 0);

-- Q35 (Passage B - Q2)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Doc', 
 N'According to the passage, why should AI not entirely replace human doctors?', 
 @passageB, 
 N'Đoạn văn nêu hai lý do: thuật toán có thể bị sai lệch dữ liệu huấn luyện (training bias) và máy móc thiếu sự thấu cảm trong chăm sóc bệnh nhân.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Algorithms are prone to training bias and lack emotional empathy.', 1),
(@q, 'B', N'Deep learning models consume excessive hospital electricity.', 0),
(@q, 'C', N'Medical equipment is incompatible with digital computing systems.', 0),
(@q, 'D', N'Doctors refuse to interact with software interfaces.', 0);

-- Q36 (Passage B - Q3)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Doc', 
 N'The word "supplant" in the passage is closest in meaning to:', 
 @passageB, 
 N'"Supplant" có nghĩa là thay thế hoàn toàn vị trí của ai/cái gì (replace / supersede).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'reinforce', 0), (@q, 'B', N'replace', 1), (@q, 'C', N'criticize', 0), (@q, 'D', N'instruct', 0);

-- PASSAGE C (B2 Reading): Remote Work and Asynchronous Collaboration
DECLARE @passageC NVARCHAR(MAX) = N'The widespread adoption of remote work has catalyzed a fundamental paradigm shift in corporate culture. In lieu of rigid nine-to-five desk presence, leading technology organizations are embracing asynchronous communication frameworks. By documenting task requirements comprehensively in cloud knowledge bases, distributed squads across multiple time zones can collaborate smoothly without endless real-time meetings. Nevertheless, organizational psychologists caution that prolonged physical isolation can erode spontaneous creativity and weaken interpersonal cohesion if social rituals are neglected.';

-- Q37 (Passage C - Q1)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB2, N'TracNghiem', N'Doc', 
 N'What key practice enables geographically dispersed teams to collaborate asynchronously?', 
 @passageC, 
 N'Đoạn văn nêu: các nhóm phân tán cộng tác trơn tru nhờ việc lưu trữ tài liệu yêu cầu công việc chi tiết trên nền tảng đám mây.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Mandating uninterrupted daily webcam surveillance.', 0),
(@q, 'B', N'Documenting project requirements thoroughly in cloud knowledge repositories.', 1),
(@q, 'C', N'Requiring all employees to work in the same physical time zone.', 0),
(@q, 'D', N'Eliminating all forms of written status reports.', 0);

-- Q38 (Passage C - Q2)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB2, N'TracNghiem', N'Doc', 
 N'What adverse consequence might result from perpetual remote isolation according to psychologists?', 
 @passageC, 
 N'Chuyên gia tâm lý cảnh báo việc cô lập thể chất lâu dài làm xói mòn khả năng sáng tạo tự phát và gắn kết giữa người với người.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Erosion of spontaneous innovation and weakened interpersonal cohesion.', 1),
(@q, 'B', N'A guaranteed reduction in quarterly software delivery speeds.', 0),
(@q, 'C', N'Sudden hardware malfunctions on local workstations.', 0),
(@q, 'D', N'Immediate dissolution of corporate legal partnerships.', 0);

-- PASSAGE D (C1 Reading): Cognitive Agility & The Attention Economy
DECLARE @passageD NVARCHAR(MAX) = N'In an era characterized by hyper-connectivity and pervasive notification streams, deep contemplation has become a scarce cognitive commodity. The human attention mechanism is continually fragmented by algorithmic feedback loops tailored to stimulate dopamine responses. Renowned cognitive theorists argue that this persistent cognitive interruption induces neuroplastic adaptations that favor superficial skimming over sustained analytical deduction. To preserve high-order conceptual thinking, knowledge workers must cultivate deliberate practices of monastic focus and cognitive sanctuary, disengaging periodically from ubiquitous digital stimuli.';

-- Q39 (Passage D - Q1)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'What is the central premise asserted by the author regarding modern attention spans?', 
 @passageD, 
 N'Tác giả khẳng định luồng thông báo liên tục và vòng lặp thuật toán khiến khả năng chú ý bị phân mảnh, ưu tiên đọc lướt hời hợt hơn tư duy phân tích sâu.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Human memory storage capacity has doubled in the digital age.', 0),
(@q, 'B', N'Algorithmic interruptions promote superficial skimming at the expense of deep deduction.', 1),
(@q, 'C', N'Social media notifications permanently cure workplace anxiety.', 0),
(@q, 'D', N'Multitasking is proven to enhance complex philosophical reasoning.', 0);

-- Q40 (Passage D - Q2)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'What countermeasure does the author propose to safeguard high-order conceptual thinking?', 
 @passageD, 
 N'Tác giả đề xuất xây dựng các khoảng thời gian tập trung biệt lập ("monastic focus" / "cognitive sanctuary") và tạm ngắt kết nối với các kích thích số.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Engaging in deliberate periods of focused solitude away from digital stimuli.', 1),
(@q, 'B', N'Installing additional notification monitors in the office.', 0),
(@q, 'C', N'Increasing weekly reliance on generative AI synthesis tools.', 0),
(@q, 'D', N'Abandoning all reading activities in favor of podcasts.', 0);

-- Q41 (Passage D - Q3)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'The term "scarce cognitive commodity" implies that deep contemplation is:', 
 @passageD, 
 N'"Scarce cognitive commodity" mang nghĩa tư duy tập trung sâu sắc đã trở thành một thứ vô cùng hiếm hoi và quý giá trong thời đại số.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'financially unprofitable', 0),
(@q, 'B', N'exceptionally rare and valuable', 1),
(@q, 'C', N'biologically obsolete', 0),
(@q, 'D', N'readily accessible to everyone', 0);

-- Q42 (Passage C - Q3)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB2, N'TracNghiem', N'Doc', 
 N'The phrase "in lieu of" in the passage can be most accurately substituted with:', 
 @passageC, 
 N'"In lieu of" là cụm từ trang trọng có nghĩa là "thay vì" (instead of).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'in comparison to', 0), (@q, 'B', N'instead of', 1), (@q, 'C', N'in addition to', 0), (@q, 'D', N'in accordance with', 0);

-- Q43 (Passage D - Q4)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'What tone does the author predominantly adopt throughout Passage D?', 
 @passageD, 
 N'Giọng văn phân tích, phản biện mang tính học thuật và cảnh báo sâu sắc (Analytic and cautionary).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Sarcastic and indifferent', 0),
(@q, 'B', N'Analytical and cautionary', 1),
(@q, 'C', N'Enthusiastic and celebratory', 0),
(@q, 'D', N'Nostalgic and regretful', 0);

-- Q44 (Passage B - Q4)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Doc', 
 N'Which title best summarizes the core theme of Passage B?', 
 @passageB, 
 N'Tiêu đề phản ánh đúng nhất: Trí tuệ nhân tạo trong chẩn đoán y tế: Công cụ hỗ trợ mạnh mẽ chứ không phải sự thay thế con người.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The Total Demise of Human Radiologists', 0),
(@q, 'B', N'AI in Diagnostics: An Augmentation, Not a Replacement', 1),
(@q, 'C', N'A Practical Guide to Night Shift Scheduling', 0),
(@q, 'D', N'Financial Pitfalls of Medical Computer Vision', 0);

-- ===================================================================
-- 4. BỘ CÂU HỎI VIẾT & SỬA LỖI CÂU HỌC THUẬT (10 CÂU: A2 -> C1)
-- ===================================================================

-- Q45 (A2 Writing - Sentence Combining)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdDaily, @tdA2, N'TracNghiem', N'Viet', 
 N'Choose the best sentence that combines the two thoughts: "The train was late. We missed the opening presentation."', 
 N'Liên từ "Because" chỉ nguyên nhân: Because the train was late, we missed the opening presentation.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdA2, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Because the train was late, we missed the opening presentation.', 1),
(@q, 'B', N'The train was late although we missed the presentation.', 0),
(@q, 'C', N'We missed the presentation so that the train was late.', 0),
(@q, 'D', N'The train was late but we missed the presentation.', 0);

-- Q46 (B1 Writing - Error Identification)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'Viet', 
 N'Identify the underlined part that contains a grammatical error: "Neither the supervisor (A) nor the lab technicians (B) was (C) aware of the chemical spill (D)."', 
 N'Quy tắc hòa hợp chủ vị với "Neither... nor...": Động từ hòa hợp với danh từ gần nhất "the lab technicians" (số nhiều), do đó "was" phải sửa thành "were".', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'the supervisor', 0), (@q, 'B', N'the lab technicians', 0), (@q, 'C', N'was', 1), (@q, 'D', N'chemical spill', 0);

-- Q47 (B1 Writing - Sentence Rewriting)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'Viet', 
 N'Choose the sentence that is closest in meaning to: "They haven''t upgraded their office software for over five years."', 
 N'"It has been over five years since they last upgraded their office software."', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It has been over five years since they last upgraded their office software.', 1),
(@q, 'B', N'They upgraded their office software five years ago for the first time.', 0),
(@q, 'C', N'Office software will be upgraded within the next five years.', 0),
(@q, 'D', N'They have been upgrading their office software every five years.', 0);

-- Q48 (B2 Writing - Cohesive Transitions)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Viet', 
 N'Choose the most suitable transitional word: "The pilot experiment failed to deliver anticipated metrics. ______, the insights gleaned helped refine the subsequent research protocol."', 
 N'"Nevertheless" (tuy nhiên / dẫu vậy) diễn tả sự đối lập nhượng bộ giữa hai câu độc lập.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Furthermore', 0), (@q, 'B', N'Nevertheless', 1), (@q, 'C', N'Consequently', 0), (@q, 'D', N'In addition', 0);

-- Q49 (B2 Writing - Parallel Structure)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Viet', 
 N'Identify the sentence that observes correct parallel structure in academic writing:', 
 N'Cấu trúc song hành: analyzing user requirements, designing intuitive wireframes, and implementing core APIs (đều là danh động từ V-ing).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The consultant was responsible for analyzing user requirements, to design intuitive wireframes, and API implementation.', 0),
(@q, 'B', N'The consultant was responsible for analyzing user requirements, designing intuitive wireframes, and implementing core APIs.', 1),
(@q, 'C', N'The consultant was responsible for user analysis, design wireframes, and to implement core APIs.', 0),
(@q, 'D', N'The consultant analyzed user requirements, designing intuitive wireframes, and implemented core APIs.', 0);

-- Q50 (B2 Writing - Dangling Modifier)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Viet', 
 N'Select the sentence that avoids a dangling modifier error:', 
 N'Tránh lỗi bổ ngữ lơ lửng: Chủ ngữ của mệnh đề chính phải là chủ thể thực hiện hành động "Having reviewed the financial audit": "the board members unanimously approved...".', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdB2, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Having reviewed the financial audit, the quarterly budget was approved by the board members.', 0),
(@q, 'B', N'Having reviewed the financial audit, the board members unanimously approved the quarterly budget.', 1),
(@q, 'C', N'Having reviewed the financial audit, an approval of the budget was announced.', 0),
(@q, 'D', N'Having reviewed the financial audit, it was decided to approve the budget.', 0);

-- Q51 (C1 Writing - Nominalization)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Viet', 
 N'Which version demonstrates the most sophisticated academic style through nominalization?', 
 N'Phong cách học thuật Band C1 ưa chuộng danh từ hóa (The implementation of stringent emission standards fostered a notable reduction in urban air pollution).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Because the city implemented stricter emission standards, air pollution in urban zones decreased noticeably.', 0),
(@q, 'B', N'The implementation of stringent emission standards fostered a notable reduction in urban air pollution.', 1),
(@q, 'C', N'City officials made standards stricter so that air became much cleaner than before.', 0),
(@q, 'D', N'They reduced air pollution by implementing emission standards that were very strict.', 0);

-- Q52 (C1 Writing - Hedging & Modality)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Viet', 
 N'Which sentence employs appropriate academic hedging to avoid over-generalization?', 
 N'Học thuật C1 sử dụng ngôn ngữ dè dặt (hedging) như "The empirical data suggests that... may correlate with..." để tránh khẳng định tuyệt đối chưa chứng minh.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The empirical data clearly proves that remote work always causes communication breakdowns.', 0),
(@q, 'B', N'The empirical data suggests that extended remote work may correlate with decreased impromptu collaboration.', 1),
(@q, 'C', N'It is indisputable that remote work is disastrous for corporate innovation.', 0),
(@q, 'D', N'Without question, every single remote worker feels physically isolated.', 0);

-- Q53 (C1 Writing - Sentence Inversion for Emphasis)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Viet', 
 N'Choose the sentence that correctly uses negative inversion to emphasize restriction: "They permitted the audit team to inspect the source code only after a non-disclosure agreement was executed."', 
 N'Đảo ngữ với "Only after": Only after a non-disclosure agreement was executed were the audit team permitted to inspect the source code.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Only after an agreement was executed the audit team was permitted to inspect the code.', 0),
(@q, 'B', N'Only after a non-disclosure agreement was executed were the audit team permitted to inspect the source code.', 1),
(@q, 'C', N'Only after was an agreement executed did the audit team permitted to inspect the code.', 0),
(@q, 'D', N'Only after did they execute an agreement they permitted the audit team to inspect the code.', 0);

-- Q54 (C1 Writing - Word Precision)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Viet', 
 N'Select the most precise and formal phrasing for an academic abstract: "This study looks into how monetary policy affects the housing market."', 
 N'"This study investigates the ramifications of monetary policy on real estate asset valuations."', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'This study looks into how monetary policy affects the housing market.', 0),
(@q, 'B', N'This study investigates the ramifications of monetary policy on real estate asset valuations.', 1),
(@q, 'C', N'This paper talks about what monetary policy does to housing prices.', 0),
(@q, 'D', N'This study checks out whether central banks influence home costs.', 0);

PRINT N'Đã thêm thành công 54 câu hỏi trắc nghiệm chất lượng cao (Ngữ pháp, Từ vựng, Đọc hiểu, Viết).';

-- ===================================================================
-- 5. BỔ SUNG CÂU HỎI VÀO CÁC ĐỀ THI THỬ HIỆN CÓ ĐỂ ĐỦ 10 CÂU
-- ===================================================================
-- Đề 0 (A1): Thêm câu hỏi A1-A2
;WITH CTE0 AS (
    SELECT MaCauHoi, KyNang, DoKho,
           (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 0) + ROW_NUMBER() OVER(ORDER BY MaCauHoi) AS NextOrder
    FROM @NewQuestions
    WHERE MaTrinhDo IN (@tdA1, @tdA2) AND MaCauHoi NOT IN (SELECT MaCauHoi FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 0)
)
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
SELECT 0, MaCauHoi, NextOrder, 1.0, N'TracNghiem', KyNang, DoKho
FROM CTE0 WHERE NextOrder <= 10;
UPDATE BaiTapDeThi SET TongSoCau = (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 0) WHERE MaBaiTapDeThi = 0;

-- Đề 1 (A2): Thêm câu hỏi A2-B1
;WITH CTE1 AS (
    SELECT MaCauHoi, KyNang, DoKho,
           (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 1) + ROW_NUMBER() OVER(ORDER BY MaCauHoi) AS NextOrder
    FROM @NewQuestions
    WHERE MaTrinhDo IN (@tdA2, @tdB1) AND MaCauHoi NOT IN (SELECT MaCauHoi FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 1)
)
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
SELECT 1, MaCauHoi, NextOrder, 1.0, N'TracNghiem', KyNang, DoKho
FROM CTE1 WHERE NextOrder <= 10;
UPDATE BaiTapDeThi SET TongSoCau = (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 1) WHERE MaBaiTapDeThi = 1;

-- Đề 5 (C1): Thêm câu hỏi C1
;WITH CTE5 AS (
    SELECT MaCauHoi, KyNang, DoKho,
           (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 5) + ROW_NUMBER() OVER(ORDER BY MaCauHoi) AS NextOrder
    FROM @NewQuestions
    WHERE MaTrinhDo = @tdC1 AND MaCauHoi NOT IN (SELECT MaCauHoi FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 5)
)
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
SELECT 5, MaCauHoi, NextOrder, 1.0, N'TracNghiem', KyNang, DoKho
FROM CTE5 WHERE NextOrder <= 10;
UPDATE BaiTapDeThi SET TongSoCau = (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 5) WHERE MaBaiTapDeThi = 5;

-- ===================================================================
-- 6. TẠO CÁC ĐỀ THI THỬ MỚI (LoaiHinh = 'ThiThu')
-- ===================================================================

-- Thi thử 1: TOEIC Part 5 & 6 Doanh Nghiệp
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%TOEIC Part 5 & 6: Khảo Thí Ngữ Pháp%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdBusiness, @tdB1, N'TOEIC Part 5 & 6: Khảo Thí Ngữ Pháp & Từ Vựng Doanh Nghiệp', N'ThiThu', 
            N'Đề thi thử định dạng TOEIC Reading: Hoàn thành câu công sở, từ vựng hợp đồng, liên từ và cấu trúc văn bản thương mại.', 25, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deThiThu1 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deThiThu1, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.MaTrinhDo IN (@tdB1, @tdB2) AND q.KyNang IN (N'NguPhap', N'TuVung', N'Viet')
    ORDER BY q.Id;
END

-- Thi thử 2: IELTS Academic Reading Mock Test #06
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%IELTS Academic Reading & Language In Use Mock Test #06%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdIELTS, @tdB2, N'IELTS Academic Reading & Language In Use Mock Test #06', N'ThiThu', 
            N'Đề thi thử IELTS Học thuật: Bao gồm bài đọc chuyên sâu AI trong y học, Skimming & Scanning, từ vựng C1 và phân tích suy luận logic.', 35, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deThiThu2 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deThiThu2, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id DESC), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.MaTrinhDo IN (@tdB2, @tdC1)
    ORDER BY q.Id DESC;
END

-- Thi thử 3: VSTEP B2 Chuẩn Hóa
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%VSTEP B2 Chuẩn Hóa%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdGrammar, @tdB2, N'VSTEP B2 Chuẩn Hóa: Ngữ Pháp Chuyên Sâu & Đọc Hiểu Tổng Hợp', N'ThiThu', 
            N'Đề thi mô phỏng định dạng VSTEP Bậc 4 (B2): Kiểm tra toàn diện ngữ pháp phức hợp, đảo ngữ, đọc hiểu công sở và kỹ năng viết chuẩn mực.', 30, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deThiThu3 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deThiThu3, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.MaTrinhDo = @tdB2
    ORDER BY q.Id;
END

-- ===================================================================
-- 7. TẠO CÁC GÓI BÀI TẬP & ĐỀ LUYỆN TẬP (LoaiHinh = 'LuyenTap')
-- ===================================================================

-- Gói Luyện Tập 1: Chuyên đề Câu điều kiện & Đảo ngữ
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%Gói Luyện Tập: Chuyên Đề Câu Điều Kiện%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdGrammar, @tdB2, N'Gói Luyện Tập: Chuyên Đề Câu Điều Kiện & Đảo Ngữ Nâng Cao', N'LuyenTap', 
            N'Luyện tập chuyên sâu các dạng câu điều kiện loại 1, 2, 3, hỗn hợp và các cấu trúc đảo ngữ thường gặp trong đề thi.', 20, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen1 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen1, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.KyNang = N'NguPhap' AND q.MaTrinhDo IN (@tdB1, @tdB2, @tdC1)
    ORDER BY q.Id;
END

-- Gói Luyện Tập 2: 100 Từ Vựng & Collocations Oxford Cốt Lõi
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%100 Từ Vựng & Collocations Oxford%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdIELTS, @tdB1, N'Gói Luyện Tập: 100 Từ Vựng & Collocations Oxford Cốt Lõi', N'LuyenTap', 
            N'Rèn luyện phản xạ từ vựng cốt lõi, cụm từ đi kèm (collocations) và cụm động từ (phrasal verbs) mức độ B1 - B2.', 15, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen2 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen2, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.KyNang = N'TuVung'
    ORDER BY q.Id;
END

-- Gói Luyện Tập 3: Đọc Hiểu Chuyên Sâu: Trí Tuệ Nhân Tạo & Y Học
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%Đọc Hiểu Chuyên Sâu: Trí Tuệ Nhân Tạo%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdIELTS, @tdB2, N'Gói Luyện Tập Đọc Hiểu Chuyên Sâu: Trí Tuệ Nhân Tạo & Y Học Hiện Đại', N'LuyenTap', 
            N'Luyện tập đọc hiểu theo đoạn văn dài: Đoán nghĩa từ vựng trong ngữ cảnh, tìm ý chính và phân tích suy luận logic.', 25, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen3 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen3, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.KyNang = N'Doc'
    ORDER BY q.Id;
END

-- Gói Luyện Tập 4: Cấu Trúc Viết & Sửa Lỗi Ngữ Pháp Học Thuật
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%Cấu Trúc Viết & Phát Hiện Lỗi Sai%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdIELTS, @tdB2, N'Gói Luyện Tập: Cấu Trúc Viết & Phát Hiện Lỗi Sai Học Thuật', N'LuyenTap', 
            N'Chuyên đề phát hiện lỗi sai ngữ pháp, viết lại câu đồng nghĩa, cấu trúc song hành và chuyển tiếp mạch lạc cho bài luận.', 20, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen4 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen4, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.KyNang = N'Viet'
    ORDER BY q.Id;
END

-- Gói Luyện Tập 5: Tổng Hợp Căn Bản A2
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%Tổng Hợp: Ngữ Pháp Căn Bản & Giao Tiếp%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdDaily, @tdA2, N'Gói Luyện Tập Tổng Hợp: Ngữ Pháp Căn Bản & Giao Tiếp Đời Sống', N'LuyenTap', 
            N'Gói ôn tập nền tảng dành cho học viên A1 - A2: Thì quá khứ, hiện tại hoàn thành căn bản, so sánh và từ vựng thông dụng.', 15, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen5 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen5, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.MaTrinhDo IN (@tdA1, @tdA2, @tdB1) AND q.KyNang IN (N'NguPhap', N'TuVung', N'Viet')
    ORDER BY q.Id;
END

-- Gói Luyện Tập 6: Thành Ngữ & Cụm Từ Học Thuật C1
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%Thành Ngữ & Cụm Từ Học Thuật Band C1%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdIELTS, @tdC1, N'Gói Luyện Tập Thành Ngữ & Cụm Từ Học Thuật Band C1 (Advanced Idioms)', N'LuyenTap', 
            N'Thử thách nâng cao vốn từ vựng học thuật C1, thành ngữ báo chí, tư duy phản biện và phong cách diễn đạt sắc sảo.', 20, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen6 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen6, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id DESC), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.MaTrinhDo = @tdC1
    ORDER BY q.Id DESC;
END

PRINT N'===================================================================';
PRINT N'ĐÃ BỔ SUNG TOÀN BỘ CÂU HỎI VÀ TẠO CÁC ĐỀ THI THỬ & GÓI LUYỆN TẬP THÀNH CÔNG!';
PRINT N'===================================================================';
GO


-- ===================================================================
-- 14. Bá»” SUNG 135 CÃ‚U Há»ŽI TRáº®C NGHIá»†M CHO 27 BÃ€I Há»ŒC Há»† THá»NG
-- ===================================================================
GO

-- ===================================================================
-- SCRIPT BỔ SUNG 135 CÂU HỎI TRẮC NGHIỆM CHO 27 BÀI HỌC HỆ THỐNG
-- 5 CÂU/BÀI CHO 27 BÀI HỌC (ID 0 ĐẾN 26) - BỎ QUA BÀI 27 (VERBS AI TEST)
-- KHÔNG BAO GỒM KỸ NĂNG NGHE AUDIO, ĐẦY ĐỦ 4 LỰA CHỌN VÀ LỜI GIẢI CHI TIẾT
-- ===================================================================
USE TiengAnhAI;
GO
SET NOCOUNT ON;
DECLARE @gvId INT;
SELECT TOP 1 @gvId = MaNguoiDung FROM NguoiDung WHERE MaVaiTro = 2;
IF @gvId IS NULL SET @gvId = 1;
DECLARE @q INT;

-- -------------------------------------------------------------------
-- BÀI HỌC ID 0
-- -------------------------------------------------------------------
-- Q1 (Lesson 0 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'NguPhap', N'Choose the best response: "Hello, my name is Alex. Nice to meet you!"', N'Đáp lại lời chào ''Nice to meet you!'' lịch sự và chuẩn mực nhất là ''Nice to meet you too!''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Nice to meet you too!', 1),
(@q, 'B', N'I am fine, and you?', 0),
(@q, 'C', N'Yes, of course.', 0),
(@q, 'D', N'See you later.', 0);

-- Q2 (Lesson 0 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'NguPhap', N'Complete the sentence: "Where ______ you from?" - "I am from Vietnam."', N'Chủ ngữ ''you'' đi với động từ to be ''are'' trong thì hiện tại đơn.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'is', 0),
(@q, 'B', N'are', 1),
(@q, 'C', N'am', 0),
(@q, 'D', N'do', 0);

-- Q3 (Lesson 0 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'TuVung', N'Which phrase is used to ask for someone''s name politely?', N'''What is your name?'' hoặc ''May I have your name?'' dùng để hỏi tên đối phương.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How are you named?', 0),
(@q, 'B', N'What is your name?', 1),
(@q, 'C', N'Who are you called?', 0),
(@q, 'D', N'Where is your name?', 0);

-- Q4 (Lesson 0 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'NguPhap', N'Choose the correct verb form: "She ______ a first-year college student."', N'Chủ ngữ ngôi thứ ba số ít ''She'' đi với ''is''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'is', 1),
(@q, 'B', N'are', 0),
(@q, 'C', N'be', 0),
(@q, 'D', N'am', 0);

-- Q5 (Lesson 0 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'TuVung', N'Choose the best introduction for a new team member:', N'Câu giới thiệu tự nhiên và lịch sự: ''Hi, I am David from the Marketing department.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hi, I am David from the Marketing department.', 1),
(@q, 'B', N'Hey, you must know my name is David.', 0),
(@q, 'C', N'Look at me, I work here.', 0),
(@q, 'D', N'My call is David.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 1
-- -------------------------------------------------------------------
-- Q6 (Lesson 1 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'TuVung', N'Which greeting is the most formal in a corporate seminar?', N'''Good morning, ladies and gentlemen'' là lời chào trang trọng chuẩn mực.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey guys, what''s up?', 0),
(@q, 'B', N'Good morning, ladies and gentlemen.', 1),
(@q, 'C', N'Howdy folks!', 0),
(@q, 'D', N'Yo, what is going on?', 0);

-- Q7 (Lesson 1 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'NguPhap', N'In British formal etiquette, what is the customary reply to ''How do you do?''', N'Trong quy chuẩn chào hỏi trang trọng, ''How do you do?'' được đáp lại bằng chính câu ''How do you do?''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How do you do?', 1),
(@q, 'B', N'I do homework every day.', 0),
(@q, 'C', N'I am doing fine, thank you.', 0),
(@q, 'D', N'Yes, I can do it.', 0);

-- Q8 (Lesson 1 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'TuVung', N'Which expression is informal and used between close acquaintances?', N'''Long time no see!'' là câu chào thân mật khi gặp lại bạn bè.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It is an honor to make your acquaintance.', 0),
(@q, 'B', N'Long time no see! How have you been?', 1),
(@q, 'C', N'Allow me to extend my regards.', 0),
(@q, 'D', N'I trust this finds you well.', 0);

-- Q9 (Lesson 1 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'Viet', N'Choose the standard complimentary closing for a formal email:', N'''Sincerely yours'' hoặc ''Best regards'' là lời chào kết trang trọng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Catch ya later!', 0),
(@q, 'B', N'Sincerely yours,', 1),
(@q, 'C', N'Cheers mate,', 0),
(@q, 'D', N'Talk soon,', 0);

-- Q10 (Lesson 1 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'TuVung', N'How should you answer when a client says ''Pleased to meet you''?', N'''The pleasure is mine'' là cách đáp lễ tinh tế và chuyên nghiệp.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The pleasure is mine, thank you.', 1),
(@q, 'B', N'Never mind about it.', 0),
(@q, 'C', N'You are very welcome.', 0),
(@q, 'D', N'No problem at all.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 2
-- -------------------------------------------------------------------
-- Q11 (Lesson 2 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'TuVung', N'Fill in the blank: "I have been working ______ a software developer for three years."', N'Cấu trúc ''work as + danh từ chỉ nghề nghiệp''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'as', 1),
(@q, 'B', N'like', 0),
(@q, 'C', N'with', 0),
(@q, 'D', N'for', 0);

-- Q12 (Lesson 2 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'TuVung', N'Complete the sentence: "I was born and ______ up in Da Nang city."', N'Cụm ''bring up'' mang nghĩa nuôi nấng, dạng bị động ''brought up'' (lớn lên tại).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'grown', 0),
(@q, 'B', N'brought', 1),
(@q, 'C', N'raised', 0),
(@q, 'D', N'lived', 0);

-- Q13 (Lesson 2 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'TuVung', N'Which phrase expresses dedication to personal interests?', N'''Follow one''s passion'' nghĩa là theo đuổi đam mê cá nhân.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'follow one''s passion', 1),
(@q, 'B', N'call it a day', 0),
(@q, 'C', N'bite the bullet', 0),
(@q, 'D', N'break the ice', 0);

-- Q14 (Lesson 2 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'NguPhap', N'Choose the correct relative pronoun: "Da Lat, ______ is known as the city of flowers, attracts many visitors."', N'Mệnh đề quan hệ không xác định bổ nghĩa cho danh từ riêng dùng ''which''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'which', 1),
(@q, 'B', N'that', 0),
(@q, 'C', N'where', 0),
(@q, 'D', N'whose', 0);

-- Q15 (Lesson 2 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'Doc', N'In a career bio, what does ''drive for continuous learning'' signify?', N'Nghĩa là tinh thần chủ động học hỏi và trau dồi kiến thức không ngừng.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A proactive commitment to expanding knowledge and skills.', 1),
(@q, 'B', N'Traveling frequently for vacation.', 0),
(@q, 'C', N'Avoiding new technology tools.', 0),
(@q, 'D', N'Following orders blindly.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 3
-- -------------------------------------------------------------------
-- Q16 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'What should you say to reserve a table for four tonight?', N'Mẫu câu đặt bàn chuẩn: ''I would like to book a table for four at 7 PM.''', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I want food for four people.', 0),
(@q, 'B', N'I would like to book a table for four at 7 PM.', 1),
(@q, 'C', N'Give me four chairs tonight.', 0),
(@q, 'D', N'Are there four people here?', 0);

-- Q17 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'Which term refers to a dish served before the main course?', N'''Appetizer'' hoặc ''starter'' là món khai vị.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Dessert', 0),
(@q, 'B', N'Appetizer', 1),
(@q, 'C', N'Beverage', 0),
(@q, 'D', N'Condiment', 0);

-- Q18 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'How do you ask for the bill politely in a restaurant?', N'Cách xin hóa đơn thông dụng: ''Could we have the bill, please?''', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How much money now?', 0),
(@q, 'B', N'Give me the payment paper.', 0),
(@q, 'C', N'Could we have the bill, please?', 1),
(@q, 'D', N'I take the money away.', 0);

-- Q19 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'If you are allergic to peanuts, what should you notify the waiter?', N'''I have a severe peanut allergy; does this dish contain nuts?''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I hate peanuts so much.', 0),
(@q, 'B', N'I have a severe peanut allergy; does this dish contain nuts?', 1),
(@q, 'C', N'Peanuts are not good today.', 0),
(@q, 'D', N'Do not show me peanuts.', 0);

-- Q20 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'How would you order a steak cooked completely through?', N'''Well-done'' là mức độ thịt bò nướng chín kỹ hoàn toàn.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Rare', 0),
(@q, 'B', N'Medium-rare', 0),
(@q, 'C', N'Well-done', 1),
(@q, 'D', N'Raw', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 4
-- -------------------------------------------------------------------
-- Q21 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'Where can customers try on clothes before buying in a shop?', N'''Fitting room'' hoặc ''changing room'' là phòng thử đồ.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Storage room', 0),
(@q, 'B', N'Fitting room', 1),
(@q, 'C', N'Checkout counter', 0),
(@q, 'D', N'Parking lot', 0);

-- Q22 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'What document must you present to request an item exchange or refund?', N'''Receipt'' là hóa đơn thanh toán cần có khi đổi trả hàng.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Receipt', 1),
(@q, 'B', N'Menu', 0),
(@q, 'C', N'Passport', 0),
(@q, 'D', N'Brochure', 0);

-- Q23 (Lesson 4 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'NguPhap', N'Complete the question: "Do you have this shirt ______ a larger size?"', N'Giới từ ''in'' đi với kích cỡ: in a larger size / in size M.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'on', 0),
(@q, 'B', N'at', 0),
(@q, 'C', N'in', 1),
(@q, 'D', N'by', 0);

-- Q24 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'What does ''out of stock'' mean?', N'''Out of stock'' nghĩa là tạm thời hết hàng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Available at a discount', 0),
(@q, 'B', N'Temporarily unavailable in the store', 1),
(@q, 'C', N'Damaged and unsellable', 0),
(@q, 'D', N'Made of organic material', 0);

-- Q25 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'Which question politely asks the price of an item?', N'''How much is this jacket?'' là câu hỏi giá phổ biến và chính xác.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How many is this jacket?', 0),
(@q, 'B', N'How much is this jacket?', 1),
(@q, 'C', N'What cost does this jacket?', 0),
(@q, 'D', N'Which money is this jacket?', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 5
-- -------------------------------------------------------------------
-- Q26 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'Which adjective describes food that has a pleasant salty or spicy taste, not sweet?', N'''Savory'' (hoặc savoury) mang nghĩa có vị mặn đậm đà hấp dẫn.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Bitter', 0),
(@q, 'B', N'Savory', 1),
(@q, 'C', N'Bland', 0),
(@q, 'D', N'Sour', 0);

-- Q27 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'What does ''mouth-watering'' describe in culinary reviews?', N'''Mouth-watering'' miêu tả món ăn thơm ngon hấp dẫn đến ứa nước miếng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Extremely appealing and delicious-smelling', 1),
(@q, 'B', N'Too salty to swallow', 0),
(@q, 'C', N'Overly dry and tough', 0),
(@q, 'D', N'Completely tasteless', 0);

-- Q28 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'Complete the sentence: "The soup is ______ with star anise and fresh herbs."', N'Cụm ''seasoned with'' nghĩa là được nêm nếm bằng gia vị.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'spilled', 0),
(@q, 'B', N'seasoned', 1),
(@q, 'C', N'painted', 0),
(@q, 'D', N'boiled', 0);

-- Q29 (Lesson 5 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'Doc', N'Street vendors are famous for preserving ______ family recipes across generations.', N'Từ ''authentic'' nghĩa là chuẩn vị, đích thực truyền thống.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'artificial', 0),
(@q, 'B', N'authentic', 1),
(@q, 'C', N'synthetic', 0),
(@q, 'D', N'reluctant', 0);

-- Q30 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'Which word describes a texture that makes a sharp sound when bitten into?', N'''Crispy'' (giòn rụm) tạo âm thanh giòn tan khi cắn.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Soggy', 0),
(@q, 'B', N'Crispy', 1),
(@q, 'C', N'Greasy', 0),
(@q, 'D', N'Chewy', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 6
-- -------------------------------------------------------------------
-- Q31 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'What is the document given to a passenger that allows them to board the airplane?', N'''Boarding pass'' là thẻ lên máy bay.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Boarding pass', 1),
(@q, 'B', N'Customs declaration', 0),
(@q, 'C', N'Flight manual', 0),
(@q, 'D', N'Luggage tag', 0);

-- Q32 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'What is luggage that passengers are allowed to take inside the cabin called?', N'''Carry-on luggage'' (hoặc hand luggage) là hành lý xách tay mang lên khoang máy bay.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Checked baggage', 0),
(@q, 'B', N'Carry-on luggage', 1),
(@q, 'C', N'Cargo shipment', 0),
(@q, 'D', N'Excess parcel', 0);

-- Q33 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'When going through airport security, you must place metallic items into a ______.', N'''Tray'' (khay nhựa) dùng để đựng đồ kim loại qua máy quét an ninh.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'trash bin', 0),
(@q, 'B', N'tray', 1),
(@q, 'C', N'suitcase', 0),
(@q, 'D', N'drawer', 0);

-- Q34 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'Which term refers to a flight that does not stop between departure and destination?', N'''Direct flight'' hoặc ''non-stop flight'' là chuyến bay thẳng.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Layover flight', 0),
(@q, 'B', N'Non-stop flight', 1),
(@q, 'C', N'Connecting flight', 0),
(@q, 'D', N'Delayed flight', 0);

-- Q35 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'What does ''excess baggage fee'' mean?', N'Phí hành lý quá cước khi vượt trọng lượng quy định.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A discount for booking early', 0),
(@q, 'B', N'A charge for luggage exceeding the weight limit', 1),
(@q, 'C', N'A fee for changing flight dates', 0),
(@q, 'D', N'Insurance for lost items', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 7
-- -------------------------------------------------------------------
-- Q36 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'Choose the best question to ask a stranger for directions:', N'''Excuse me, could you tell me how to get to the nearest metro station?''', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey, where is the train right now?', 0),
(@q, 'B', N'Excuse me, could you tell me how to get to the nearest metro station?', 1),
(@q, 'C', N'You tell me the way immediately.', 0),
(@q, 'D', N'Train station location now please.', 0);

-- Q37 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'Where do passengers wait for their train in a subway station?', N'''Platform'' (sân ga/ ke ga) là nơi hành khách chờ tàu.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Runway', 0),
(@q, 'B', N'Platform', 1),
(@q, 'C', N'Intersection', 0),
(@q, 'D', N'Roundabout', 0);

-- Q38 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'What does ''change lines at Central Station'' mean?', N'Nghĩa là chuyển sang tuyến tàu khác tại ga trung tâm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Buy a new ticket', 0),
(@q, 'B', N'Transfer to a different subway route', 1),
(@q, 'C', N'Exit the station immediately', 0),
(@q, 'D', N'Wait for tomorrow''s train', 0);

-- Q39 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'Complete the sentence: "Walk straight for two blocks, then turn ______ at the traffic lights."', N'Cụm chỉ hướng: ''turn left'' (rẽ trái) hoặc ''turn right''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'left', 1),
(@q, 'B', N'up', 0),
(@q, 'C', N'down', 0),
(@q, 'D', N'out', 0);

-- Q40 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'If a destination is ''within walking distance'', it means:', N'''Within walking distance'' nghĩa là ở rất gần, có thể đi bộ được.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'You must take a taxi', 0),
(@q, 'B', N'It is close enough to reach on foot', 1),
(@q, 'C', N'It is located in another city', 0),
(@q, 'D', N'The road is completely blocked', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 8
-- -------------------------------------------------------------------
-- Q41 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'What is the primary objective of ''skimming'' in an academic reading test?', N'Mục tiêu của skimming là đọc lướt nhanh để nắm được ý chính và bố cục toàn bài.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'To locate specific dates and numbers', 0),
(@q, 'B', N'To read quickly to comprehend the general gist and layout', 1),
(@q, 'C', N'To translate every sentence word by word', 0),
(@q, 'D', N'To memorize technical jargon', 0);

-- Q42 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'When applying the ''scanning'' technique, a reader should:', N'Kỹ thuật scanning tập trung tìm kiếm thông tin cụ thể (từ khóa, con số, tên riêng).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Focus on identifying specific keywords, names, or figures without reading whole paragraphs.', 1),
(@q, 'B', N'Analyze the author''s philosophical viewpoint.', 0),
(@q, 'C', N'Underline every unfamiliar vocabulary item.', 0),
(@q, 'D', N'Read aloud to improve pronunciation.', 0);

-- Q43 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'Where is the topic sentence of an academic paragraph typically located?', N'Câu chủ đề của đoạn văn thường nằm ở câu đầu tiên hoặc câu thứ hai.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'In the very first or second sentence', 1),
(@q, 'B', N'Strictly inside the footnotes', 0),
(@q, 'C', N'Always hidden in the exact middle', 0),
(@q, 'D', N'Only in the bibliography', 0);

-- Q44 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'Why is identifying synonyms crucial for IELTS Reading questions?', N'Vì câu hỏi trong bài đọc IELTS luôn được diễn đạt lại (paraphrased) bằng từ đồng nghĩa.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Questions almost always paraphrase information from the text rather than using identical words.', 1),
(@q, 'B', N'Synonyms are required to be memorized for speaking tests.', 0),
(@q, 'C', N'Texts contain no grammatical conjunctions.', 0),
(@q, 'D', N'IELTS examiners penalize short answers.', 0);

-- Q45 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'What should a test-taker do when encountering an unfamiliar scientific term?', N'Dựa vào ngữ cảnh xung quanh để suy đoán nghĩa khái quát thay vì dừng lại quá lâu.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Stop the test and consult a dictionary', 0),
(@q, 'B', N'Deduce its rough meaning from contextual clues and move forward', 1),
(@q, 'C', N'Assume the entire paragraph is erroneous', 0),
(@q, 'D', N'Leave all subsequent questions blank', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 9
-- -------------------------------------------------------------------
-- Q46 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'A statement in IELTS Reading should be marked ''FALSE'' when:', N'Chọn FALSE khi thông tin trong câu hỏi đối lập, mâu thuẫn trực tiếp với bài đọc.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The passage mentions no related information whatsoever.', 0),
(@q, 'B', N'The statement directly contradicts the facts stated in the passage.', 1),
(@q, 'C', N'The author''s tone is slightly ambiguous.', 0),
(@q, 'D', N'The passage was written over a decade ago.', 0);

-- Q47 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'A statement should be evaluated as ''NOT GIVEN'' if:', N'Chọn NOT GIVEN khi bài đọc không có đủ thông tin để xác nhận câu đó đúng hay sai.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The passage provides insufficient information to confirm or refute the claim.', 1),
(@q, 'B', N'The author explicitly denies the statement.', 0),
(@q, 'C', N'The claim uses simple past tense.', 0),
(@q, 'D', N'The vocabulary is too elementary.', 0);

-- Q48 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'How do absolute qualifiers such as ''always'', ''all'', or ''never'' affect statement validity?', N'Các từ tuyệt đối (all, always) thường biến câu thành FALSE nếu bài đọc chỉ nói ở mức độ đa số hoặc có ngoại lệ.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'They usually make a statement FALSE if the text only suggests a tendency with exceptions.', 1),
(@q, 'B', N'They guarantee the statement is always TRUE.', 0),
(@q, 'C', N'They turn every question into a multiple-choice item.', 0),
(@q, 'D', N'They are completely ignored by examiners.', 0);

-- Q49 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'Text: "The vaccine showed 90% efficacy in lab trials." Statement: "The vaccine guarantees complete immunity in all humans." Answer:', N'Bài đọc chỉ nói 90% trong phòng thí nghiệm, câu hỏi khẳng định ''guarantee complete immunity in all'' -> mâu thuẫn -> FALSE.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'TRUE', 0),
(@q, 'B', N'FALSE', 1),
(@q, 'C', N'NOT GIVEN', 0),
(@q, 'D', N'CANNOT DETERMINE', 0);

-- Q50 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'Text: "Dr. Miller graduated from Cambridge in 1998." Statement: "Dr. Miller was the top student in his graduating class." Answer:', N'Bài đọc chỉ nói tốt nghiệp năm 1998, không nhắc đến việc có đứng đầu lớp hay không -> NOT GIVEN.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'TRUE', 0),
(@q, 'B', N'FALSE', 0),
(@q, 'C', N'NOT GIVEN', 1),
(@q, 'D', N'YES', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 10
-- -------------------------------------------------------------------
-- Q51 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'What must an effective introduction of an IELTS Opinion Essay contain?', N'Mở bài hiệu quả phải có câu diễn giải đề (paraphrase) và luận đề rõ ràng (thesis statement).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A paraphrase of the prompt and a clear thesis statement stating the author''s stance.', 1),
(@q, 'B', N'Ten rhetorical questions without any direct opinion.', 0),
(@q, 'C', N'A detailed list of all statistics in the world.', 0),
(@q, 'D', N'A quotation from an ancient philosopher only.', 0);

-- Q52 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'Which cohesive device expresses a concession in an argumentative essay?', N'''Admittedly'' hoặc ''While it is true that'' dùng để nhượng bộ một quan điểm đối lập trước khi phản bác.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Furthermore', 0),
(@q, 'B', N'Admittedly', 1),
(@q, 'C', N'Consequently', 0),
(@q, 'D', N'In addition', 0);

-- Q53 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'What is the primary role of a counter-argument paragraph in high-band writing?', N'Phân tích góc nhìn đối lập rồi phản bác để chứng minh tính thuyết phục vượt trội của lập luận chính.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'To confuse the reader about the author''s real stance', 0),
(@q, 'B', N'To acknowledge opposing views and refute them to reinforce the core thesis', 1),
(@q, 'C', N'To lengthen the essay without adding substance', 0),
(@q, 'D', N'To replace the conclusion section', 0);

-- Q54 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'Which sentence exhibits sophisticated academic style avoiding first-person overuse?', N'Phong cách học thuật khách quan: ''It is widely contended that technological automation enhances economic productivity.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I think tech makes money fast.', 0),
(@q, 'B', N'It is widely contended that technological automation enhances economic productivity.', 1),
(@q, 'C', N'You should believe that robots are awesome.', 0),
(@q, 'D', N'Me and my friends agree computers work well.', 0);

-- Q55 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'What should NEVER appear in the conclusion of an academic essay?', N'Tuyệt đối không đưa ý tưởng hoặc luận điểm hoàn toàn mới chưa được phân tích vào phần kết bài.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A restatement of the thesis', 0),
(@q, 'B', N'Brand-new arguments that were not discussed in the body paragraphs', 1),
(@q, 'C', N'A summary of main body arguments', 0),
(@q, 'D', N'A concluding recommendation', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 11
-- -------------------------------------------------------------------
-- Q56 (Lesson 11 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'Viet', N'Which collocation describes the rapid, uncontrolled expansion of metropolitan boundaries?', N'''Urban sprawl'' là thuật ngữ chỉ sự mở rộng tự phát, thiếu kiểm soát của các khu đô thị.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Urban sprawl', 1),
(@q, 'B', N'City shrinking', 0),
(@q, 'C', N'Rural stagnation', 0),
(@q, 'D', N'Metropolitan contraction', 0);

-- Q57 (Lesson 11 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'TuVung', N'Select the academic verb meaning ''to reduce the severity or impact of climate disruption'':', N'''Mitigate'' nghĩa là giảm thiểu tác động tiêu cực.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Exacerbate', 0),
(@q, 'B', N'Mitigate', 1),
(@q, 'C', N'Proliferate', 0),
(@q, 'D', N'Deteriorate', 0);

-- Q58 (Lesson 11 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'Viet', N'Choose the best academic sentence proposing a governmental policy for traffic congestion:', N'''Governments should implement congestion pricing schemes to disincentivize private vehicle usage in city centers.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Governments should implement congestion pricing schemes to disincentivize private vehicle usage.', 1),
(@q, 'B', N'Make drivers pay big cash so they stop driving cars.', 0),
(@q, 'C', N'Stop all automobiles from entering roads immediately.', 0),
(@q, 'D', N'People should walk more because cars are annoying.', 0);

-- Q59 (Lesson 11 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'TuVung', N'What does ''sustainable development'' encompass in urban planning?', N'Phát triển bền vững: đáp ứng nhu cầu hiện tại mà không làm tổn hại đến tài nguyên của các thế hệ tương lai.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Meeting current societal needs without compromising the ability of future generations to meet theirs.', 1),
(@q, 'B', N'Exploiting all oil reserves as swiftly as possible.', 0),
(@q, 'C', N'Banning all commercial activities in major cities.', 0),
(@q, 'D', N'Building skyscrapers regardless of ecological damage.', 0);

-- Q60 (Lesson 11 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'Viet', N'Complete the sentence with an appropriate transition: "Heavy reliance on fossil fuels degrades air quality; ______, it accelerates global warming."', N'''Furthermore'' hoặc ''Moreover'' dùng để bổ sung luận điểm cùng chiều.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Nevertheless', 0),
(@q, 'B', N'Furthermore', 1),
(@q, 'C', N'In contrast', 0),
(@q, 'D', N'Conversely', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 12
-- -------------------------------------------------------------------
-- Q61 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'In university group discussions, what is a ''rubric''?', N'''Rubric'' là bảng tiêu chí đánh giá và chấm điểm bài tập học thuật.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A grading criterion guide detailing assessment expectations.', 1),
(@q, 'B', N'A certificate of attendance.', 0),
(@q, 'C', N'A break time between lectures.', 0),
(@q, 'D', N'A list of campus restaurants.', 0);

-- Q62 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'Which phrase expresses polite academic disagreement between team members?', N'''I take your point, but we might also consider...'' thể hiện sự phản biện lịch sự, tôn trọng đối phương.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'You are totally wrong about everything.', 0),
(@q, 'B', N'I take your point, but we might also consider another angle.', 1),
(@q, 'C', N'Stop talking nonsense.', 0),
(@q, 'D', N'That idea makes no sense whatsoever.', 0);

-- Q63 (Lesson 12 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'Doc', N'What does ''peer review'' refer to in collaborative academic projects?', N'Đánh giá chéo: sinh viên nhận xét và góp ý phản biện bài làm của bạn cùng nhóm/lớp.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Evaluation of work by peers possessing similar competencies.', 1),
(@q, 'B', N'Grading conducted exclusively by the university president.', 0),
(@q, 'C', N'Submitting essays to a public newspaper.', 0),
(@q, 'D', N'Writing exams without supervision.', 0);

-- Q64 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'If a student suggests ''dividing the workload evenly'', what do they intend?', N'Họ đề xuất phân chia khối lượng công việc công bằng cho từng thành viên trong nhóm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Assigning equal responsibilities to each team member.', 1),
(@q, 'B', N'Leaving the entire assignment to one individual.', 0),
(@q, 'C', N'Postponing the project until next semester.', 0),
(@q, 'D', N'Asking the professor to write the report.', 0);

-- Q65 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'What is an ''executive summary'' in a project portfolio?', N'''Executive summary'' là bản tóm tắt súc tích các kết quả và điểm then chốt của dự án.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A brief overview synthesizing the key findings and recommendations.', 1),
(@q, 'B', N'A list of all team member phone numbers.', 0),
(@q, 'C', N'The final receipt for printing the document.', 0),
(@q, 'D', N'A biography of corporate executives.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 13
-- -------------------------------------------------------------------
-- Q66 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'In academic monologues, what function do ''signposting phrases'' serve?', N'Cụm từ báo hiệu giúp người nghe định hướng cấu trúc và bước chuyển ý trong bài giảng.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'They orient the listener regarding the structure and transitions of the lecture.', 1),
(@q, 'B', N'They indicate grammatical errors in the textbook.', 0),
(@q, 'C', N'They are used to fill silent pauses when the speaker forgets words.', 0),
(@q, 'D', N'They signal the end of the semester.', 0);

-- Q67 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'When a lecturer states: "Turning now to the empirical evidence...", what are they doing?', N'Diễn giả đang chuyển tiếp sang phần phân tích dữ liệu thực nghiệm.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Transitioning to discuss observational and experimental data.', 1),
(@q, 'B', N'Ending the lecture abruptly.', 0),
(@q, 'C', N'Asking the students to close their laptops.', 0),
(@q, 'D', N'Repeating the introductory slide.', 0);

-- Q68 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'Which phrase marks a critical emphasis or surprising revelation in scientific speech?', N'''Crucially'' hoặc ''What is particularly noteworthy is that...''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Incidentally', 0),
(@q, 'B', N'What is particularly noteworthy is that...', 1),
(@q, 'C', N'As a trivial aside', 0),
(@q, 'D', N'Needless to mention', 0);

-- Q69 (Lesson 13 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'Doc', N'What distinguishes an ''empirical study'' from a purely theoretical hypothesis?', N'Nghiên cứu thực nghiệm dựa trên việc quan sát và thu thập dữ liệu thực tế, không chỉ là suy luận lý thuyết.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It is grounded on verifiable observation and measurable data collection.', 1),
(@q, 'B', N'It uses fictional stories instead of numbers.', 0),
(@q, 'C', N'It requires no verification by other scientists.', 0),
(@q, 'D', N'It cannot be published in journals.', 0);

-- Q70 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'Select the word synonymous with ''ubiquitous'' in academic discourse:', N'''Ubiquitous'' = ''omnipresent'' (phổ biến khắp mọi nơi).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Rare', 0),
(@q, 'B', N'Omnipresent', 1),
(@q, 'C', N'Obsolete', 0),
(@q, 'D', N'Ephemeral', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 14
-- -------------------------------------------------------------------
-- Q71 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'Choose the correct auxiliary verb: "They ______ lived in London since 2015."', N'Chủ ngữ ''They'' đi với trợ động từ ''have'' trong thì hiện tại hoàn thành.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'has', 0),
(@q, 'B', N'have', 1),
(@q, 'C', N'had', 0),
(@q, 'D', N'are', 0);

-- Q72 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'Complete the sentence: "I haven''t submitted the quarterly report ______."', N'Trong câu phủ định của hiện tại hoàn thành, từ ''yet'' thường đặt ở cuối câu.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'already', 0),
(@q, 'B', N'yet', 1),
(@q, 'C', N'since', 0),
(@q, 'D', N'just', 0);

-- Q73 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'What is the difference between ''have been to'' and ''have gone to''?', N'''have been to'' = đã đến và đã trở về; ''have gone to'' = đã đi và hiện vẫn đang ở đó chưa về.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'''have been to'' means visited and returned; ''have gone to'' means departed and not yet back.', 1),
(@q, 'B', N'''have been to'' is past tense; ''have gone to'' is future tense.', 0),
(@q, 'C', N'Both phrases are completely interchangeable in all contexts.', 0),
(@q, 'D', N'''have gone to'' can only be used with first-person singular.', 0);

-- Q74 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'Which preposition correctly completes: "She has worked here ______ five years."', N'Dùng ''for'' với khoảng thời gian (for five years); dùng ''since'' với mốc thời gian.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'since', 0),
(@q, 'B', N'for', 1),
(@q, 'C', N'during', 0),
(@q, 'D', N'at', 0);

-- Q75 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'"Have you ______ tasted authentic Italian risotto?"', N'''Ever'' thường dùng trong câu hỏi thì hiện tại hoàn thành để hỏi về trải nghiệm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'never', 0),
(@q, 'B', N'ever', 1),
(@q, 'C', N'already', 0),
(@q, 'D', N'still', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 15
-- -------------------------------------------------------------------
-- Q76 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"While we ______ dinner, the power suddenly went out."', N'Hành động đang diễn ra trong quá khứ dùng quá khứ tiếp diễn (were having), hành động chen ngang dùng quá khứ đơn (went out).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'had', 0),
(@q, 'B', N'were having', 1),
(@q, 'C', N'have had', 0),
(@q, 'D', N'are having', 0);

-- Q77 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"She ______ to Tokyo last summer on a business assignment."', N'Thời gian xác định trong quá khứ ''last summer'' chia thì quá khứ đơn (went).', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'goes', 0),
(@q, 'B', N'went', 1),
(@q, 'C', N'has gone', 0),
(@q, 'D', N'was going', 0);

-- Q78 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"At 8:00 PM yesterday, what ______ you doing?"', N'Câu hỏi thời điểm cụ thể đang diễn ra trong quá khứ chia quá khứ tiếp diễn: were you doing.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'did', 0),
(@q, 'B', N'were', 1),
(@q, 'C', N'are', 0),
(@q, 'D', N'have', 0);

-- Q79 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"He ______ his leg while he was playing football on Sunday."', N'Hành động gãy chân chen ngang hành động đang chơi bóng đá, chia quá khứ đơn (broke).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'broke', 1),
(@q, 'B', N'was breaking', 0),
(@q, 'C', N'breaks', 0),
(@q, 'D', N'has broken', 0);

-- Q80 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'Which conjunction commonly introduces a past continuous clause for simultaneous background actions?', N'''While'' thường dùng để mở đầu mệnh đề quá khứ tiếp diễn chỉ hành động diễn ra song song.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'While', 1),
(@q, 'B', N'Immediately', 0),
(@q, 'C', N'Soon', 0),
(@q, 'D', N'Unless', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 16
-- -------------------------------------------------------------------
-- Q81 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"Look at those dark clouds! It ______ rain."', N'Dự đoán có bằng chứng cụ thể trước mắt (những đám mây đen) dùng ''is going to''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 0),
(@q, 'B', N'is going to', 1),
(@q, 'C', N'rains', 0),
(@q, 'D', N'shall', 0);

-- Q82 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"The phone is ringing." - "Don''t worry, I ______ answer it."', N'Quyết định đưa ra ngay tại thời điểm nói (spontaneous decision) dùng ''will''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 1),
(@q, 'B', N'am going to', 0),
(@q, 'C', N'answer', 0),
(@q, 'D', N'shall to', 0);

-- Q83 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"We have already booked the flight. We ______ visit our grandparents next week."', N'Kế hoạch đã chuẩn bị và sắp xếp trước (đã đặt vé) dùng ''are going to''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 0),
(@q, 'B', N'are going to', 1),
(@q, 'C', N'visit', 0),
(@q, 'D', N'might to', 0);

-- Q84 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"I promise I ______ keep your secret confidential."', N'Lời hứa (promise) dùng ''will''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 1),
(@q, 'B', N'am going to', 0),
(@q, 'C', N'kept', 0),
(@q, 'D', N'going to', 0);

-- Q85 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'Which sentence indicates a pre-arranged personal decision?', N'''I am going to study abroad next autumn; I''ve already received the acceptance letter.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I will probably study tomorrow maybe.', 0),
(@q, 'B', N'I am going to study abroad next autumn; I''ve already received the acceptance letter.', 1),
(@q, 'C', N'I think I will go buy some milk right now.', 0),
(@q, 'D', N'I shall see about it.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 17
-- -------------------------------------------------------------------
-- Q86 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Reduce the clause: "The engineers who are designing the new bridge have extensive experience."', N'Mệnh đề chủ động rút gọn bằng V-ing: ''The engineers designing the new bridge...''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The engineers designed the new bridge...', 0),
(@q, 'B', N'The engineers designing the new bridge...', 1),
(@q, 'C', N'The engineers to design the new bridge...', 0),
(@q, 'D', N'The engineers being designed...', 0);

-- Q87 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Reduce the clause: "The vehicles which were damaged in the hail storm have been towed away."', N'Mệnh đề bị động rút gọn bằng V-ed/V3: ''The vehicles damaged in the hail storm...''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The vehicles damaged in the hail storm...', 1),
(@q, 'B', N'The vehicles damaging in the hail storm...', 0),
(@q, 'C', N'The vehicles to damage in the hail storm...', 0),
(@q, 'D', N'The vehicles have damaged...', 0);

-- Q88 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Complete the sentence: "Neil Armstrong was the first person ______ on the surface of the Moon."', N'Sau các từ chỉ thứ tự ''the first'', ''the second'', ''the only'', rút gọn mệnh đề quan hệ bằng ''to + V-infinitive''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'stepping', 0),
(@q, 'B', N'to step', 1),
(@q, 'C', N'stepped', 0),
(@q, 'D', N'had stepped', 0);

-- Q89 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'"The document ______ by the legal team yesterday must be signed immediately."', N'Mệnh đề rút gọn bị động: ''reviewed by the legal team''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'reviewing', 0),
(@q, 'B', N'reviewed', 1),
(@q, 'C', N'to review', 0),
(@q, 'D', N'was reviewed', 0);

-- Q90 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Which sentence correctly uses a perfect participle clause indicating an earlier completed action?', N'''Having finished all testing procedures, the software team deployed the update.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Finished all testing procedures, the software team deployed the update.', 0),
(@q, 'B', N'Having finished all testing procedures, the software team deployed the update.', 1),
(@q, 'C', N'To have finish all testing procedures, the team deployed.', 0),
(@q, 'D', N'Being finish all testing procedures, the team deployed.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 18
-- -------------------------------------------------------------------
-- Q91 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'"People say that he owns three software companies." -> Passive: "He is said ______ three software companies."', N'Cấu trúc bị động với động từ tường thuật: He is said to own...', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'owning', 0),
(@q, 'B', N'to own', 1),
(@q, 'C', N'owned', 0),
(@q, 'D', N'to have owned', 0);

-- Q92 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'"People believe that the ancient city was destroyed by an earthquake." -> "The ancient city is believed ______ by an earthquake."', N'Hành động bị phá hủy xảy ra trước thời điểm tin tưởng, dùng ''to have been destroyed''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'to be destroyed', 0),
(@q, 'B', N'to have been destroyed', 1),
(@q, 'C', N'destroying', 0),
(@q, 'D', N'was destroyed', 0);

-- Q93 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'Causative form: "I need to have my car ______ before taking the long road trip."', N'Cấu trúc truyền khiến: ''have something done'' (nhờ/thuê ai làm gì đó cho mình).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'repair', 0),
(@q, 'B', N'repaired', 1),
(@q, 'C', N'repairing', 0),
(@q, 'D', N'to repair', 0);

-- Q94 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'"The manager got the contractor ______ the blueprints by Friday."', N'Cấu trúc truyền khiến: ''get someone to do something''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'finish', 0),
(@q, 'B', N'to finish', 1),
(@q, 'C', N'finished', 0),
(@q, 'D', N'finishing', 0);

-- Q95 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'Complete the impersonal passive: "______ is anticipated that consumer spending will rebound this quarter."', N'Chủ ngữ giả ''It'' trong cấu trúc bị động khách quan: It is anticipated that...', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It', 1),
(@q, 'B', N'There', 0),
(@q, 'C', N'What', 0),
(@q, 'D', N'Which', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 19
-- -------------------------------------------------------------------
-- Q96 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Type 1: "If the weather ______ favorable tomorrow, we will launch the drone survey."', N'Mệnh đề If của câu điều kiện loại 1 chia thì hiện tại đơn (is).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'is', 1),
(@q, 'B', N'will be', 0),
(@q, 'C', N'were', 0),
(@q, 'D', N'had been', 0);

-- Q97 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Type 2: "If I ______ in your position, I would renegotiate the contractual terms."', N'Câu điều kiện loại 2 giả định trái với hiện tại, to be chia ''were'' cho tất cả các ngôi.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'am', 0),
(@q, 'B', N'were', 1),
(@q, 'C', N'have been', 0),
(@q, 'D', N'had been', 0);

-- Q98 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Type 3: "If they ______ the backup server, they wouldn''t have lost client records."', N'Câu điều kiện loại 3 giả định trái với quá khứ: If + had + P2.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'configured', 0),
(@q, 'B', N'had configured', 1),
(@q, 'C', N'would configure', 0),
(@q, 'D', N'were configuring', 0);

-- Q99 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Mixed conditional (Past condition -> Present result): "If she ______ French in college, she would understand this report today."', N'Điều kiện trong quá khứ (had studied) dẫn đến kết quả ở hiện tại (would understand today).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'studied', 0),
(@q, 'B', N'had studied', 1),
(@q, 'C', N'studies', 0),
(@q, 'D', N'would study', 0);

-- Q100 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'"______ you submit the required permits on time, your construction will proceed smoothly."', N'''Provided that'' (hoặc ''Providing that'') mang nghĩa ''với điều kiện là / miễn là''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Unless', 0),
(@q, 'B', N'Provided that', 1),
(@q, 'C', N'Although', 0),
(@q, 'D', N'In spite of', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 20
-- -------------------------------------------------------------------
-- Q101 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'"Never in my career ______ such negligence in quality control."', N'Đảo ngữ với ''Never'': Trợ động từ đứng trước chủ ngữ: have I witnessed.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I have witnessed', 0),
(@q, 'B', N'have I witnessed', 1),
(@q, 'C', N'I witnessed', 0),
(@q, 'D', N'did I witnessed', 0);

-- Q102 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'"Not only ______ the deadline, but they also exceeded all benchmark metrics."', N'Đảo ngữ với ''Not only'': Not only did they meet...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'they met', 0),
(@q, 'B', N'did they meet', 1),
(@q, 'C', N'have they met', 0),
(@q, 'D', N'they were meeting', 0);

-- Q103 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'"Under no circumstances ______ confidential credentials be shared across public channels."', N'Đảo ngữ với ''Under no circumstances'': should confidential credentials be...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'confidential credentials should', 0),
(@q, 'B', N'should confidential credentials', 1),
(@q, 'C', N'they will', 0),
(@q, 'D', N'credentials would', 0);

-- Q104 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'Inversion in conditional type 3: "______ we received earlier notification, we would have averted the incident."', N'Đảo ngữ điều kiện loại 3 bỏ ''If'', đưa ''Had'' lên đầu: Had we received...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Had', 1),
(@q, 'B', N'Were', 0),
(@q, 'C', N'Should', 0),
(@q, 'D', N'If had', 0);

-- Q105 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'Inversion in conditional type 1: "______ any discrepancy arise, please alert the compliance officer immediately."', N'Đảo ngữ điều kiện loại 1 bỏ ''If'', đưa ''Should'' lên đầu: Should any discrepancy arise...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Were', 0),
(@q, 'B', N'Had', 0),
(@q, 'C', N'Should', 1),
(@q, 'D', N'Could', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 21
-- -------------------------------------------------------------------
-- Q106 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'Choose the most professional opening sentence for a business inquiry email:', N'''I am writing with regard to your company''s logistics software solutions.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey, tell me about your software.', 0),
(@q, 'B', N'I am writing with regard to your company''s logistics software solutions.', 1),
(@q, 'C', N'I wanna know what you do.', 0),
(@q, 'D', N'Listen, can you explain your app?', 0);

-- Q107 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'What is the standard phrase to refer to an attached invoice in a commercial email?', N'''Please find attached the finalized invoice for services rendered.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Look at the invoice down here.', 0),
(@q, 'B', N'Please find attached the finalized invoice for services rendered.', 1),
(@q, 'C', N'I dropped the invoice paper.', 0),
(@q, 'D', N'See the attachment now.', 0);

-- Q108 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'Which phrase expresses anticipated future communication courteously?', N'''I look forward to hearing from you at your earliest convenience.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Reply fast or else.', 0),
(@q, 'B', N'I look forward to hearing from you at your earliest convenience.', 1),
(@q, 'C', N'You must answer quickly.', 0),
(@q, 'D', N'I wait for your typing.', 0);

-- Q109 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'When addressing an email to ''Dear Sir/Madam'' (recipient unknown by name), what is the traditional British sign-off?', N'Quy tắc British English: Khi không biết tên người nhận (''Dear Sir/Madam''), lời chào kết là ''Yours faithfully''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Yours sincerely,', 0),
(@q, 'B', N'Yours faithfully,', 1),
(@q, 'C', N'Best buddy,', 0),
(@q, 'D', N'Warm hugs,', 0);

-- Q110 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'Select the appropriate subject line for a job interview confirmation:', N'''Interview Confirmation: Senior Business Analyst - [Candidate Name]''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey about the interview', 0),
(@q, 'B', N'Interview Confirmation: Senior Business Analyst - [Candidate Name]', 1),
(@q, 'C', N'Can we talk tomorrow?', 0),
(@q, 'D', N'Job thing discussion', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 22
-- -------------------------------------------------------------------
-- Q111 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'How do you professionally request to reschedule an upcoming consultation meeting?', N'''Due to an unexpected scheduling conflict, would it be possible to reschedule our meeting to Thursday?''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I can''t come, pick another day.', 0),
(@q, 'B', N'Due to an unexpected scheduling conflict, would it be possible to reschedule our meeting to Thursday?', 1),
(@q, 'C', N'Cancel tomorrow, see you later.', 0),
(@q, 'D', N'Not coming today.', 0);

-- Q112 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'In customer complaint handling, which sentence expresses sincere accountability?', N'''We sincerely apologize for the inconvenience caused and have initiated an immediate investigation.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It wasn''t our fault, blame the courier.', 0),
(@q, 'B', N'We sincerely apologize for the inconvenience caused and have initiated an immediate investigation.', 1),
(@q, 'C', N'You should have checked earlier.', 0),
(@q, 'D', N'Everyone makes mistakes, calm down.', 0);

-- Q113 (Lesson 22 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'TuVung', N'What does ''expedite the replacement shipment'' mean?', N'''Expedite'' nghĩa là đẩy nhanh tiến độ xử lý/giao hàng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Cancel the delivery entirely', 0),
(@q, 'B', N'Accelerate the shipping process of the replacement goods', 1),
(@q, 'C', N'Charge double for postage', 0),
(@q, 'D', N'Return the package to the manufacturer', 0);

-- Q114 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'Choose the best sentence to confirm an appointment time and location:', N'''This is to confirm our meeting on October 28th at 10:00 AM in Conference Room B.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Yes we meet tomorrow morning.', 0),
(@q, 'B', N'This is to confirm our meeting on October 28th at 10:00 AM in Conference Room B.', 1),
(@q, 'C', N'I will be there whenever.', 0),
(@q, 'D', N'Okay see you.', 0);

-- Q115 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'Which closing statement maintains goodwill after resolving a dispute?', N'''We greatly value your partnership and remain committed to ensuring your satisfaction.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Don''t complain again next time.', 0),
(@q, 'B', N'We greatly value your partnership and remain committed to ensuring your satisfaction.', 1),
(@q, 'C', N'Case closed forever.', 0),
(@q, 'D', N'Be glad we gave you a refund.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 23
-- -------------------------------------------------------------------
-- Q116 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'In procurement negotiations, what does ''mutual benefit'' mean?', N'''Mutual benefit'' (đôi bên cùng có lợi) tương đương với giải pháp ''win-win''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'An outcome where only one party profits.', 0),
(@q, 'B', N'An arrangement that advantages both contractual parties equitably.', 1),
(@q, 'C', N'A cancellation of business ties.', 0),
(@q, 'D', N'A legal penalty for delayed deliveries.', 0);

-- Q117 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'What legal term refers to a failure to fulfill agreed terms in a commercial contract?', N'''Breach of contract'' nghĩa là hành vi vi phạm hợp đồng.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Breach of contract', 1),
(@q, 'B', N'Amendment of agreement', 0),
(@q, 'C', N'Discharge of duty', 0),
(@q, 'D', N'Arbitration award', 0);

-- Q118 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'If a clause is described as ''non-negotiable'', it means:', N'Điều khoản ''non-negotiable'' là điều khoản cố định, bắt buộc, không thể thương lượng thêm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Subject to compromise', 0),
(@q, 'B', N'Fixed and mandatory without room for modification', 1),
(@q, 'C', N'Completely optional', 0),
(@q, 'D', N'Informal recommendation', 0);

-- Q119 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'What does ''FOB'' (Free On Board) signify in international freight terms?', N'FOB: người bán chịu trách nhiệm và chi phí cho đến khi hàng hóa được xếp lên tàu tại cảng bốc.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The seller delivers when goods pass the ship''s rail at the named port of shipment.', 1),
(@q, 'B', N'The buyer pays nothing under any circumstance.', 0),
(@q, 'C', N'Freight is handled solely by postal courier.', 0),
(@q, 'D', N'Cargo must travel only by land.', 0);

-- Q120 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'When parties ''reach a consensus'', they:', N'Hai bên đạt được sự đồng thuận và thống nhất chung.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Terminate negotiations angrily', 0),
(@q, 'B', N'Reach a collective agreement acceptable to all participants', 1),
(@q, 'C', N'File a lawsuit in court', 0),
(@q, 'D', N'Refuse to sign documents', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 24
-- -------------------------------------------------------------------
-- Q121 (Lesson 24 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'Doc', N'In a venture capital pitch, what is the ''value proposition''?', N'''Value proposition'' là tuyên bố giá trị cốt lõi giải thích vì sao sản phẩm vượt trội và đáng để đầu tư.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A clear statement explaining the unique benefit a product provides to solve customer problems.', 1),
(@q, 'B', N'The total electrical power needed to run office computers.', 0),
(@q, 'C', N'A list of employee social media accounts.', 0),
(@q, 'D', N'The personal history of the accountant.', 0);

-- Q122 (Lesson 24 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'TuVung', N'What does ''ROI'' represent in financial presentations?', N'ROI = Return on Investment (Tỷ suất hoàn vốn đầu tư).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Rate of Interest', 0),
(@q, 'B', N'Return on Investment', 1),
(@q, 'C', N'Revenue of Industry', 0),
(@q, 'D', N'Risk of Inflation', 0);

-- Q123 (Lesson 24 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'Doc', N'What does ''scaling the business'' mean to potential angel investors?', N'''Scaling the business'' là mở rộng quy mô kinh doanh nhanh chóng mà không làm tăng chi phí tỷ lệ thuận.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Reducing production output to cut costs.', 0),
(@q, 'B', N'Growing revenue exponentially while maintaining cost efficiency.', 1),
(@q, 'C', N'Closing international branch offices.', 0),
(@q, 'D', N'Filing for corporate bankruptcy.', 0);

-- Q124 (Lesson 24 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'TuVung', N'Which phrase refers to the percentage of total sales within a market earned by a company?', N'''Market share'' là thị phần của doanh nghiệp.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Market share', 1),
(@q, 'B', N'Stock volatility', 0),
(@q, 'C', N'Gross margin', 0),
(@q, 'D', N'Liquidity pool', 0);

-- Q125 (Lesson 24 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'Doc', N'Why is demonstrating a ''defensible competitive moat'' vital during pitch sessions?', N'Vì nó chứng minh sản phẩm có rào cản cạnh tranh vững chắc khiến đối thủ không thể dễ dàng sao chép.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It shows that competitors cannot easily duplicate the company''s advantage.', 1),
(@q, 'B', N'It proves the company has dug a water canal around its headquarters.', 0),
(@q, 'C', N'It guarantees zero taxation forever.', 0),
(@q, 'D', N'It eliminates the need for software engineering.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 25
-- -------------------------------------------------------------------
-- Q126 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'What is an ''API endpoint'' in modern software architecture?', N'API endpoint là một URL cụ thể nơi ứng dụng gửi yêu cầu để truy xuất hoặc sửa đổi tài nguyên.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A physical cable connecting servers', 0),
(@q, 'B', N'A designated URL where client applications access server resources', 1),
(@q, 'C', N'The final line of a computer monitor', 0),
(@q, 'D', N'A password used for database encryption', 0);

-- Q127 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'Which process involves identifying, analyzing, and removing defects or bugs from computer code?', N'''Debugging'' là quá trình dò tìm và gỡ lỗi phần mềm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Compiling', 0),
(@q, 'B', N'Debugging', 1),
(@q, 'C', N'Refactoring', 0),
(@q, 'D', N'Deployment', 0);

-- Q128 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'What does ''CRUD'' stand for in database management applications?', N'CRUD = Create, Read, Update, Delete (4 thao tác cơ bản với dữ liệu).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Compile, Run, Undo, Deploy', 0),
(@q, 'B', N'Create, Read, Update, Delete', 1),
(@q, 'C', N'Copy, Rename, Upload, Download', 0),
(@q, 'D', N'Connect, Route, Unify, Disconnect', 0);

-- Q129 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'In Git version control, submitting code changes for team review is called creating a ______.', N'''Pull request'' (PR) là yêu cầu hợp nhất và rà soát mã nguồn.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'pull request', 1),
(@q, 'B', N'hard reboot', 0),
(@q, 'C', N'database rollback', 0),
(@q, 'D', N'local commit only', 0);

-- Q130 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'What is the primary characteristic of a ''relational database'' (RDBMS)?', N'Cơ sở dữ liệu quan hệ tổ chức dữ liệu thành các bảng có hàng và cột, liên kết với nhau bằng khóa (keys).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It stores data in tables linked by predefined relationships and foreign keys.', 1),
(@q, 'B', N'It cannot execute SQL queries.', 0),
(@q, 'C', N'It stores everything inside unstructured text files without columns.', 0),
(@q, 'D', N'It requires no primary key.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 26
-- -------------------------------------------------------------------
-- Q131 (Lesson 26 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'Doc', N'What are the three core questions answered in an Agile Daily Standup meeting?', N'3 câu hỏi cốt lõi: Hôm qua làm gì? Hôm nay sẽ làm gì? Có gặp trở ngại/khó khăn gì không?', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'What did you do yesterday? What will you do today? Are there any blockers?', 1),
(@q, 'B', N'How much is your salary? When is the party? Why are you late?', 0),
(@q, 'C', N'What is the company profit? Who is the president? What time is lunch?', 0),
(@q, 'D', N'Can you write all code today? Did you finish the entire project?', 0);

-- Q132 (Lesson 26 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'TuVung', N'In Scrum terminology, what is a ''blocker'' (or impediment)?', N'''Blocker'' là trở ngại, khó khăn kỹ thuật hoặc quy trình ngăn cản tiến độ công việc.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'An issue or dependency that prevents a team member from making progress.', 1),
(@q, 'B', N'A developer who refuses to attend meetings.', 0),
(@q, 'C', N'A firewall software blocking external attacks.', 0),
(@q, 'D', N'A completed task ready for production.', 0);

-- Q133 (Lesson 26 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'TuVung', N'What does ''wrap up the sprint'' mean in Agile project cycles?', N'''Wrap up the sprint'' nghĩa là hoàn tất và đóng kỳ Sprint.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Finalize remaining sprint tasks, conduct review and prepare for retrospective.', 1),
(@q, 'B', N'Cancel the project immediately.', 0),
(@q, 'C', N'Delete all user stories from the backlog.', 0),
(@q, 'D', N'Skip quality assurance testing.', 0);

-- Q134 (Lesson 26 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'TuVung', N'How do Scrum teams estimate task effort during sprint planning?', N'Các nhóm Scrum thường dùng ''story points'' để ước lượng độ phức tạp và công sức của task.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'By counting words in the user story', 0),
(@q, 'B', N'Using story points based on relative complexity and effort', 1),
(@q, 'C', N'By flipping a coin', 0),
(@q, 'D', N'By measuring keyboard typing speed', 0);

-- Q135 (Lesson 26 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'Viet', N'Choose the best concise update for a daily standup meeting:', N'''Yesterday I finished API integration for payments; today I will run unit tests; no blockers currently.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I worked on some stuff, today I do more stuff, see ya.', 0),
(@q, 'B', N'Yesterday I finished API integration for payments; today I will run unit tests; no blockers currently.', 1),
(@q, 'C', N'I didn''t do anything because the internet was slow.', 0),
(@q, 'D', N'Ask my manager what I am doing.', 0);

PRINT N'===================================================================';
PRINT N'ĐÃ THÊM THÀNH CÔNG 135 CÂU HỎI TRẮC NGHIỆM CHO 27 BÀI HỌC VÀO DATABASE!';
PRINT N'===================================================================';
GO

-- ===================================================================
-- 15. CẬP NHẬT TRẠNG THÁI VÀ HOÀN TẤT
-- ===================================================================
UPDATE NguoiDung SET LanDangNhapCuoi = SYSDATETIME() WHERE TenDangNhap = N'admin';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(minute, -12, SYSDATETIME()) WHERE TenDangNhap = N'giangvien';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(hour, -2, SYSDATETIME()) WHERE TenDangNhap = N'tung.hoang';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -1, SYSDATETIME()) WHERE TenDangNhap = N'maianh.tran';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(hour, -5, SYSDATETIME()) WHERE TenDangNhap = N'lan.nguyen';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -2, SYSDATETIME()) WHERE TenDangNhap = N'khoa.tran';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -4, SYSDATETIME()) WHERE TenDangNhap = N'huong.le';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -9, SYSDATETIME()) WHERE TenDangNhap = N'nam.vu';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -15, SYSDATETIME()) WHERE TenDangNhap = N'chau.ngo';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -22, SYSDATETIME()) WHERE TenDangNhap = N'bao.do';
UPDATE NguoiDung SET LanDangNhapCuoi = NULL WHERE TenDangNhap IN (N'ducanh.pham', N'mai.dang', N'hocvien');
GO

PRINT N'===================================================================';
PRINT N'ĐÃ GỘP VÀ NẠP TOÀN BỘ CƠ SỞ DỮ LIỆU SEED CHO EAI THÀNH CÔNG 100%!';
PRINT N'Tài khoản đăng nhập mặc định (Mật khẩu: 123456):';
PRINT N' - Admin: admin / 123456';
PRINT N' - Giảng viên: giangvien / 123456 (hoặc tung.hoang)';
PRINT N' - Học viên: hocvien / 123456 (hoặc lan.nguyen)';
PRINT N'===================================================================';
GO
