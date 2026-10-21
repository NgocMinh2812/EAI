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
 N'<h3>Basic Greetings & Introductions</h3><p>Hi, nice to meet you! My name is John. I am a software engineer from Vietnam.</p><ul><li>Good morning / Good afternoon</li><li>Pleased to meet you</li><li>Where are you from?</li></ul>', 15, 1),

(@cdChaoHoi, @tdA2, @gvId, N'Hội thoại Chào hỏi Trang trọng vs. Thân mật (Formal & Informal)', N'TuVung',
 N'Phân biệt cách chào hỏi trong môi trường học thuật/công sở với bạn bè hàng ngày.',
 N'<h3>Phân biệt Chào hỏi Trang trọng và Thân mật</h3><ol><li><strong>Formal Greetings:</strong> "Good morning", "How do you do?", "Pleased to meet you".</li><li><strong>Informal Greetings:</strong> "Hey, what’s up?", "How’s it going?", "Long time no see!".</li><li><strong>Thực hành:</strong> Hội thoại làm quen nơi công sở.</li></ol>', 15, 1),

(@cdChaoHoi, @tdB1, @gvId, N'Tự tin giới thiệu Nghề nghiệp, Quê hương và Đam mê cá nhân', N'Doc',
 N'Mẫu câu và cấu trúc đoạn văn ngắn giúp bạn tự tin tỏa sáng khi được hỏi "Tell me about yourself".',
 N'<h3>Kỹ thuật Self-Introduction 3 Bước</h3><ul><li><strong>Bước 1 (Hook & Background):</strong> Tên, quê hương và chuyên môn hiện tại.</li><li><strong>Bước 2 (Highlight Achievement):</strong> Dự án hoặc kỹ năng nổi bật.</li><li><strong>Bước 3 (Future Goal):</strong> Mục tiêu phát triển bản thân trong tương lai.</li></ul>', 20, 1),

-- Mua sắm & Nhà hàng
(@cdMuaSam, @tdA2, @gvId, N'Đặt bàn và Gọi món tại Nhà hàng Quốc tế (Dining Out & Ordering Food)', N'Nghe',
 N'Học các mẫu câu gọi món, hỏi thực đơn món chay, yêu cầu hóa đơn và thanh toán tiền tip.',
 N'<h3>Dining Out English Mastery</h3><ul><li>"Could we have a table for four, please?"</li><li>"What do you recommend for the main course?"</li><li>"Could we split the bill, please?"</li></ul>', 20, 1),

(@cdMuaSam, @tdA1, @gvId, N'Kỹ năng Mua sắm, Hỏi kích cỡ và Đổi trả hàng hóa (Shopping & Returns)', N'TuVung',
 N'Bộ từ vựng phong phú về quần áo, phụ kiện, chính sách hoàn tiền và thẻ khách hàng thân thiết.',
 N'<h3>Shopping Vocabulary & Expressions</h3><ul><li>Fitting room (Phòng thử đồ)</li><li>"Do you have this shirt in a medium size?"</li><li>"I would like to exchange this item or get a refund."</li></ul>', 15, 1),

(@cdMuaSam, @tdB1, @gvId, N'Văn hóa Ẩm thực Đường phố và Miêu tả Hương vị Món ăn', N'Doc',
 N'Bài đọc hiểu về văn hóa ẩm thực Á - Âu cùng cách dùng tính từ miêu tả vị giác tinh tế.',
 N'<h3>Food Culture & Taste Descriptors</h3><p>Crispy (giòn tan), Savory (đậm đà vừa miệng), Fragrant (thơm phức).</p><p>Đoạn văn so sánh văn hóa ẩm thực đường phố Bangkok và Paris.</p>', 25, 1),

-- Du lịch & Giao thông
(@cdDuLich, @tdA2, @gvId, N'Tiếng Anh Sân bay: Check-in, Cân hành lý và Qua cửa An ninh', N'Doc',
 N'Các đoạn hội thoại chuẩn tại quầy vé, khu vực kiểm tra hành lý và cửa ra máy bay (Boarding gate).',
 N'<h3>Airport Survival Guide</h3><ul><li>Boarding pass, Carry-on luggage, Baggage allowance</li><li>"Would you prefer a window or an aisle seat?"</li><li>"Please place all liquids and electronic devices in the tray."</li></ul>', 25, 1),

(@cdDuLich, @tdA1, @gvId, N'Hỏi đường và Sử dụng Tàu điện ngầm (Metro & City Navigation)', N'TuVung',
 N'Chỉ dẫn phương hướng: rẽ trái, ngã tư, đi thẳng và mua vé tàu điện ngầm tự động.',
 N'<h3>Directions & Metro Navigation</h3><ul><li>"Excuse me, how do I get to Central Station?"</li><li>"Take the red line and transfer at City Hall."</li><li>"Go straight for two blocks, then turn left at the traffic light."</li></ul>', 15, 1),

-- IELTS Reading
(@cdReading, @tdB2, @gvId, N'Chiến lược Skimming & Scanning bài đọc IELTS Academic chuyên sâu', N'Doc',
 N'Phương pháp đọc lướt nắm ý chính và quét từ khóa giúp tiết kiệm 40% thời gian làm bài Reading.',
 N'<h3>IELTS Reading Mastery: Skimming & Scanning</h3><ol><li><strong>Skimming:</strong> Đọc tiêu đề, câu đầu (topic sentence) và câu cuối mỗi đoạn văn.</li><li><strong>Scanning:</strong> Quét nhanh tên riêng, con số, thuật ngữ in hoa.</li><li><strong>Paraphrasing:</strong> Nhận diện từ đồng nghĩa giữa câu hỏi và bài đọc.</li></ol>', 35, 1),

(@cdReading, @tdB2, @gvId, N'Tuyệt chiêu xử lý dạng bài True / False / Not Given & Yes / No / Not Given', N'Doc',
 N'Phân biệt rõ ràng giữa False (thông tin mâu thuẫn) và Not Given (không đủ căn cứ kết luận).',
 N'<h3>Dạng bài T/F/NG trong IELTS</h3><ul><li><strong>True:</strong> Thông tin trùng khớp hoàn toàn về ngữ nghĩa.</li><li><strong>False:</strong> Thông tin trái ngược trực tiếp với bài viết.</li><li><strong>Not Given:</strong> Bài viết không đề cập hoặc không thể suy diễn chắc chắn.</li></ul>', 30, 1),

-- IELTS Writing
(@cdWriting, @tdB2, @gvId, N'Cấu trúc viết bài Opinion Essay (Agree or Disagree) chuẩn Band 7.5+', N'Viet',
 N'Bố cục 4 đoạn chuẩn mực: Introduction, 2 Body Paragraphs với luận cứ chặt chẽ và Conclusion sắc bén.',
 N'<h3>Template 4 Đoạn Essay Band 7.5+</h3><ul><li><strong>Introduction:</strong> Paraphrase đề bài + Thesis statement.</li><li><strong>Body 1:</strong> Luận điểm 1 + Phân tích giải thích + Ví dụ thực tế.</li><li><strong>Body 2:</strong> Luận điểm 2 + Phản biện đối lập.</li><li><strong>Conclusion:</strong> Tóm lược luận điểm và khẳng định lại quan điểm.</li></ul>', 40, 1),

(@cdWriting, @tdC1, @gvId, N'Phát triển ý tưởng và Từ vựng dạng Problem & Solution Essay (Môi trường & Đô thị hóa)', N'Viet',
 N'Cách liên kết nguyên nhân - giải pháp logic, sử dụng từ nối học thuật C1 như Consequently, Mitigate, Implement.',
 N'<h3>Problem & Solution Essay Strategy</h3><p>Cấu trúc Cause-Effect-Solution với từ vựng chủ đề môi trường: carbon footprint, renewable energy, urban sprawling.</p>', 45, 1),

-- IELTS Listening
(@cdListening, @tdB2, @gvId, N'Chiến lược chinh phục IELTS Listening Section 3: Thảo luận Đề tài Nhóm', N'Nghe',
 N'Nhận diện các bẫy đổi ý kiến, bẫy đồng thuận và kỹ thuật ghi chú nhanh câu trả lời.',
 N'<h3>Listening Section 3 Strategies</h3><p>Theo dõi mạch tranh luận giữa 2-3 người nói. Chú ý các từ nối chuyển ý: "However, actually, to be honest, I thought so at first but...".</p>', 30, 1),

(@cdListening, @tdC1, @gvId, N'Kỹ thuật bắt từ khóa trong bài giảng học thuật Section 4 (Monologue Lecture)', N'Nghe',
 N'Luyện nghe độc thoại tốc độ nhanh về các đề tài khảo cổ, sinh học và công nghệ vũ trụ.',
 N'<h3>Section 4 Note-taking</h3><p>Signposting words: "Turning now to...", "What is surprising is that...", "In conclusion...".</p>', 35, 1),

-- Ngữ pháp: Các thì
(@cdTenses, @tdB1, @gvId, N'Thì Hiện Tại Hoàn Thành — Cấu trúc & Cách dùng', N'NguPhap', 
 N'Bài học trình bày chi tiết cấu trúc thì hiện tại hoàn thành, các trạng từ đi kèm (already, yet, ever, never, just) và cách phân biệt với thì quá khứ đơn.', 
 N'<h3>Thì Hiện Tại Hoàn Thành (Present Perfect)</h3><p><strong>Công thức cốt lõi:</strong><br/>(+) S + have/has + V3/ed<br/>(-) S + have/has not + V3/ed<br/>(?) Have/Has + S + V3/ed?</p><h4>Ví dụ ngữ cảnh thực tế:</h4><ul><li>"Dr. Miller has published over twenty research papers on renewable energy." (Tiến sĩ Miller đã công bố hơn 20 bài nghiên cứu về năng lượng tái tạo.)</li><li>"We have lived in this apartment since we moved to Ho Chi Minh City." (Chúng tôi đã sống ở căn hộ này từ khi chuyển vào TP.HCM.)</li></ul><h4>Lưu ý bẫy bài thi:</h4><p>Nếu có thời gian xác định trong quá khứ (yesterday, in 2020, 2 days ago) -> BẮT BUỘC dùng Quá khứ đơn, KHÔNG dùng Hiện tại hoàn thành.</p>', 20, 1),

(@cdTenses, @tdA2, @gvId, N'Phân biệt Quá khứ Đơn (Past Simple) và Quá khứ Tiếp diễn (Past Continuous)', N'NguPhap',
 N'Cách dùng cấu trúc When và While để diễn tả hành động đang xảy ra thì có hành động khác xen vào.',
 N'<h3>Past Simple vs. Past Continuous</h3><p><strong>Công thức phối hợp thì:</strong><br/>• S + was/were + V-ing + WHEN + S + V2/ed.<br/>• WHILE + S + was/were + V-ing, S + was/were + V-ing.</p><h4>Ví dụ minh họa:</h4><ul><li>"I was debugging the backend API when the power suddenly tripped." (Tôi đang sửa lỗi API thì đột nhiên bị sập nguồn điện.)</li><li>"While the marketing director was presenting, the CEO was taking notes." (Trong khi giám đốc tiếp thị thuyết trình, CEO đang ghi chép.)</li></ul>', 20, 1),

(@cdTenses, @tdA1, @gvId, N'Làm chủ Tương lai Đơn (Will) và Tương lai Gần (Be Going To)', N'NguPhap',
 N'Phân biệt quyết định tức thời tại thời điểm nói với kế hoạch hoặc dự đoán có căn cứ xác thực.',
 N'<h3>Will vs. Be Going To</h3><p><strong>Bản chất khác biệt:</strong><br/>• <strong>Will:</strong> Quyết định bột phát tức thì, lời hứa, lời đề nghị.<br/>• <strong>Be going to:</strong> Kế hoạch có từ trước, hoặc dự đoán dựa trên bằng chứng hiện tại.</p><h4>Ví dụ cụ thể:</h4><ul><li>"The phone is ringing. - Don''t worry, I will answer it!" (Điện thoại đang reo. - Đừng lo, tôi sẽ nghe máy ngay!)</li><li>"Look at those dark storm clouds! It is going to rain heavily." (Hãy nhìn những đám mây đen kia kìa! Trời sắp mưa to rồi - Có bằng chứng thực tế.)</li></ul>', 15, 1),

-- Ngữ pháp: Mệnh đề quan hệ & Bị động
(@cdMenhDe, @tdB2, @gvId, N'Bí quyết Rút gọn Mệnh đề Quan hệ dạng V-ing, V-ed và To-Infinitive', N'NguPhap',
 N'Chuyên đề ngữ pháp nâng cao giúp câu văn gọn gàng, tăng điểm Grammatical Range & Accuracy.',
 N'<h3>Rút gọn Mệnh đề Quan hệ</h3><ol><li><strong>Chủ động -> V-ing:</strong> The man <em>standing over there</em> is my teacher.</li><li><strong>Bị động -> V-ed/V3:</strong> The documents <em>sent yesterday</em> were confidential.</li><li><strong>Số thứ tự -> To-V:</strong> She was the only student <em>to pass</em> the test.</li></ol>', 25, 1),

(@cdMenhDe, @tdB1, @gvId, N'Câu Bị Động Đặc Biệt: Động từ tường thuật và Thể truyền khiến (Have/Get)', N'NguPhap',
 N'Cấu trúc It is said that..., He is believed to be... và cấu trúc Have something done.',
 N'<h3>Thể Bị Động Đặc Biệt</h3><ul><li>"It is rumored that the merger will take place soon."</li><li>"I had my car repaired last weekend."</li></ul>', 20, 1),

-- Ngữ pháp: Điều kiện & Đảo ngữ
(@cdDieuKien, @tdB1, @gvId, N'Tổng hợp Câu Điều Kiện loại 1, 2, 3 và Điều Kiện Trộn (Mixed Conditionals)', N'NguPhap',
 N'Nắm trọn bản chất giả định điều kiện hiện tại, quá khứ và sự kết hợp kết quả hiện tại.',
 N'<h3>Conditionals Masterclass</h3><ul><li>Type 1: If + V(s/es), will + V</li><li>Type 2: If + V2/ed, would + V</li><li>Type 3: If + had + V3/ed, would have + V3/ed</li><li>Mixed: If I had studied harder, I would have a better job now.</li></ul>', 25, 1),

(@cdDieuKien, @tdC1, @gvId, N'Cấu trúc Đảo ngữ nâng cao với Never, Seldom, Not only và Đảo ngữ câu điều kiện', N'NguPhap',
 N'Tuyệt chiêu biến hóa câu văn đẳng cấp C1 với Had I known..., Were she to arrive..., Should you need help.',
 N'<h3>Advanced Inversion</h3><ul><li>"Never have I witnessed such a remarkable performance."</li><li>"Had they followed the instructions, the system would not have crashed."</li></ul>', 30, 1),

-- Thương mại: Email & Đàm phán
(@cdEmail, @tdB1, @gvId, N'Quy chuẩn Viết Email Thương mại: Mở đầu, Thân bài và Lời chào kết', N'Viet',
 N'Cách mở đầu chuyên nghiệp (Dear Mr./Ms.), nêu mục đích viết thư rõ ràng và kết thúc lịch sự.',
 N'<h3>Professional Business Email Framework</h3><ul><li>"I am writing to inquire about your corporate training programs."</li><li>"Please find attached the latest budget estimation."</li><li>"I look forward to hearing from you soon. Sincerely yours,"</li></ul>', 25, 1),

(@cdEmail, @tdB2, @gvId, N'Soạn thảo Email Đặt lịch hẹn, Xác nhận và Xử lý Khiếu nại Khách hàng', N'Viet',
 N'Mẫu thư xin lỗi đối tác chuyên nghiệp, cam kết phương án bồi thường và giữ gìn uy tín doanh nghiệp.',
 N'<h3>Client Complaint Resolution Email</h3><p>"We sincerely apologize for the delay. To compensate for the inconvenience, we have upgraded your subscription free of charge."</p>', 30, 1),

(@cdDamPhan, @tdB2, @gvId, N'Chiến lược Đàm phán Hợp đồng, Thỏa thuận Giá cả và Điều khoản Giao hàng', N'TuVung',
 N'Bộ từ vựng và câu nói ngoại giao giúp bạn đạt được thỏa thuận có lợi nhất (Win-Win Solution).',
 N'<h3>Business Negotiation Phrases</h3><ul><li>"We are willing to consider your proposal if you can offer a 5% discount."</li><li>"That sounds like a fair compromise. Let us put that in writing."</li></ul>', 30, 1),

(@cdDamPhan, @tdC1, @gvId, N'Kỹ năng Thuyết trình Dự án (Pitching) trước Hội đồng Đầu tư Quốc tế', N'Doc',
 N'Cấu trúc Slide bài thuyết trình, ngôn ngữ cơ thể, cách dẫn dắt số liệu và trả lời phần Q&A sắc bén.',
 N'<h3>Project Pitching Mastery</h3><p>Problem Statement -> Value Proposition -> Market Size -> Financial Projections & Call to Action.</p>', 35, 1),

-- CNTT (IT English)
(@cdThuatNguIT, @tdB1, @gvId, N'Từ vựng tiếng Anh Chuyên ngành Kỹ thuật Phần mềm & Cơ sở dữ liệu', N'TuVung',
 N'Thuật ngữ chuyên sâu: Microservices, Concurrency, Indexing, ORM, Refactoring và CI/CD Pipeline.',
 N'<h3>Software Engineering Terminology</h3><ul><li>Throughput, Latency, Scalability, Fault Tolerance</li><li>"The pull request has passed all automated unit tests and code linting."</li><li>"We need to optimize the database query to reduce API response time."</li></ul>', 25, 1),

(@cdAgile, @tdB1, @gvId, N'Giao tiếp trong Buổi họp Daily Standup và Báo cáo Tiến độ Sprint', N'Doc',
 N'Mẫu câu 3 câu hỏi kinh điển trong Daily Standup: Đã làm gì hôm qua? Làm gì hôm nay? Có gặp khó khăn (blocker) gì không?',
 N'<h3>Daily Scrum Communication Guide</h3><ol><li>"Yesterday, I completed the user authentication module and wrote unit tests."</li><li>"Today, I am going to integrate the payment gateway API."</li><li>"I have no blockers, but I need feedback from the design team."</li></ol>', 20, 1);
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
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdA1, N'A1 Starter Diagnostic: Phản Xạ Từ Vựng & Ngữ Pháp Căn Bản', N'ThiThu', 
        N'Kiểm tra phản xạ từ vựng cơ bản, động từ To Be, thì hiện tại đơn và câu chào hỏi thường nhật.', 20, 1, @gvId);
DECLARE @deA1 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deA1, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaTrinhDo = @tdA1 OR MaTrinhDo IS NULL;

-- Đề A2: A2 Elementary Reading & Grammar
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdA2, N'A2 Elementary Reading & Grammar: Thì Quá Khứ & So Sánh', N'ThiThu', 
        N'Part 5-6 định dạng TOEIC Bridge: Động từ bất quy tắc, thì quá khứ đơn và cấu trúc so sánh.', 30, 1, @gvId);
DECLARE @deA2 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deA2, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaTrinhDo = @tdA2 OR MaTrinhDo IS NULL;

-- Đề B1: TOEIC Reading Comprehension
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB1, N'TOEIC Reading Comprehension Diagnostic Test', N'ThiThu', 
        N'Part 5 (Hoàn thành câu), Part 6 (Điền đoạn văn) và Part 7 (Đọc hiểu văn bản thương mại).', 45, 1, @gvId);
DECLARE @deB1 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deB1, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 10 MaCauHoi FROM CauHoi ORDER BY MaCauHoi ASC);

-- Đề B2: IELTS Academic Reading & Listening Mini Mock Test #05
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB2, N'IELTS Academic Reading & Listening Mini Mock Test #05', N'ThiThu', 
        N'Bao gồm bài đọc Skimming/Scanning, phân tích từ vựng học thuật C1 và cấu trúc ngữ pháp đảo ngữ.', 40, 1, @gvId);
DECLARE @deB2 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deB2, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 10 MaCauHoi FROM CauHoi ORDER BY MaCauHoi DESC);

-- Đề B2: Ngữ pháp chuyên sâu B2
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB2, N'Kiểm tra Đột phá Ngữ pháp & Collocations Band B2', N'ThiThu', 
        N'25 câu trắc nghiệm câu điều kiện đảo ngữ, mệnh đề quan hệ rút gọn, liên từ nâng cao.', 25, 1, @gvId);
DECLARE @deB2_2 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deB2_2, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 10 MaCauHoi FROM CauHoi ORDER BY NEWID());

-- Đề C1: IELTS Advanced Academic Mock Test
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdC1, N'IELTS Advanced Academic Mock Test: Phân Tích Luận & Tư Duy Phản Biện', N'ThiThu', 
        N'Bài thi phân tích học thuật đỉnh cao: Đoạn văn nghiên cứu khoa học, cấu trúc câu phức và từ vựng C1-C2.', 45, 1, @gvId);
DECLARE @deC1 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @deC1, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaTrinhDo = @tdC1 OR MaTrinhDo IS NULL;

-- Đề 4: Luyện tập phản xạ nhanh 60s
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB1, N'Thử Thách Phản Xạ Trắc Nghiệm 60s', N'LuyenTap', 
        N'Luyện tập phản xạ nhanh từ vựng và ngữ pháp tiếng Anh trong 60 giây.', 10, 1, @gvId);
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
-- 12. CẬP NHẬT TRẠNG THÁI VÀ HOÀN TẤT
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
