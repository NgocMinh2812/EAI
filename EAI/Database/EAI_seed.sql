USE TiengAnhAI;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- Xóa dữ liệu cũ nếu có theo đúng thứ tự ràng buộc khóa ngoại (Foreign Keys)
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
DBCC CHECKIDENT ('BaiTapDeThi', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('YeuCauSinhDeAI', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('CauHoiDoAISinh', RESEED, 0) WITH NO_INFOMSGS;
GO

-- 1. Vai trò
INSERT INTO VaiTro(TenVaiTro, MoTa)
VALUES
(N'GiangVien', N'Giảng viên quản lý học liệu, đề thi, câu hỏi và phân tích học viên'),
(N'HocVien', N'Người học tiếng Anh ôn luyện, làm bài tập và thi thử'),
(N'QuanTriVien', N'Quản trị viên toàn quyền cấu hình hệ thống');
GO

-- 2. Trình độ
INSERT INTO TrinhDo(MaCapDo, TenTrinhDo, MoTa)
VALUES
(N'A1', N'Beginner', N'Trình độ mở đầu, nắm bắt từ ngữ giao tiếp đơn giản'),
(N'A2', N'Elementary', N'Trình độ sơ cấp, giao tiếp các tình huống thông thường'),
(N'B1', N'Intermediate', N'Trình độ trung cấp, hiểu ý chính văn bản và giao tiếp tự tin'),
(N'B2', N'Upper-Intermediate', N'Trình độ trên trung cấp, sử dụng tiếng Anh linh hoạt và học thuật'),
(N'C1', N'Advanced', N'Trình độ cao cấp, diễn đạt trôi chảy và hiệu quả trong mọi ngữ cảnh'),
(N'C2', N'Proficiency', N'Trình độ thành thạo, tương đương người bản ngữ');
GO

-- 3. Người dùng
DECLARE @idGV INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro=N'GiangVien');
DECLARE @idHV INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro=N'HocVien');
DECLARE @idQTV INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro=N'QuanTriVien');

INSERT INTO NguoiDung(MaVaiTro, TenDangNhap, MatKhauMaHoa, Email, HoTen, SoDienThoai, TrangThaiHoatDong, LaLanDauDangNhap, NgayTao, LanDangNhapCuoi)
VALUES
(@idGV, N'tung.hoang', N'123456', N'tung.hoang@edu.vn', N'Hoàng Văn Tùng', N'0912345678', 1, 0, DATEADD(day, -60, SYSDATETIME()), DATEADD(hour, -2, SYSDATETIME())),
(@idGV, N'maianh.tran', N'123456', N'maianh.tran@tienganhai.vn', N'Trần Mai Anh', N'0987654321', 1, 0, DATEADD(day, -45, SYSDATETIME()), DATEADD(day, -1, SYSDATETIME())),
(@idHV, N'lan.nguyen', N'123456', N'lan.nguyen@gmail.com', N'Nguyễn Thị Lan', N'0901234561', 1, 0, DATEADD(day, -30, SYSDATETIME()), DATEADD(hour, -5, SYSDATETIME())),
(@idHV, N'khoa.tran', N'123456', N'khoa.tran@outlook.com', N'Trần Minh Khoa', N'0901234562', 1, 0, DATEADD(day, -25, SYSDATETIME()), DATEADD(day, -2, SYSDATETIME())),
(@idHV, N'ducanh.pham', N'123456', N'ducanh.pham@gmail.com', N'Phạm Đức Anh', N'0901234563', 0, 0, DATEADD(day, -50, SYSDATETIME()), NULL),
(@idHV, N'huong.le', N'123456', N'huong.le@yahoo.com', N'Lê Thu Hương', N'0901234564', 1, 0, DATEADD(day, -20, SYSDATETIME()), DATEADD(day, -4, SYSDATETIME())),
(@idHV, N'nam.vu', N'123456', N'nam.vu@company.vn', N'Vũ Hoàng Nam', N'0901234565', 1, 0, DATEADD(day, -15, SYSDATETIME()), DATEADD(day, -9, SYSDATETIME())),
(@idHV, N'mai.dang', N'123456', N'mai.dang@gmail.com', N'Đặng Thị Mai', N'0901234566', 0, 0, DATEADD(day, -40, SYSDATETIME()), NULL),
(@idHV, N'chau.ngo', N'123456', N'chau.ngo@student.edu.vn', N'Ngô Bảo Châu', N'0901234567', 1, 0, DATEADD(day, -10, SYSDATETIME()), DATEADD(day, -15, SYSDATETIME())),
(@idHV, N'bao.do', N'123456', N'bao.do@gmail.com', N'Đỗ Quốc Bảo', N'0901234568', 1, 0, DATEADD(day, -5, SYSDATETIME()), DATEADD(day, -22, SYSDATETIME())),
(@idQTV, N'admin', N'123456', N'admin@gmail.com', N'Quản trị viên Hệ thống', N'0999888777', 1, 0, DATEADD(day, -100, SYSDATETIME()), SYSDATETIME()),
(@idGV, N'giangvien', N'123456', N'giangvien@gmail.com', N'Giảng viên Nguyễn Văn An', N'0988777666', 1, 0, DATEADD(day, -90, SYSDATETIME()), DATEADD(minute, -12, SYSDATETIME())),
(@idHV, N'hocvien', N'123456', N'hocvien@gmail.com', N'Học viên Trần Thị Mai', N'0977666555', 1, 0, DATEADD(day, -80, SYSDATETIME()), NULL);
GO

-- 4. Hồ sơ học tập
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

-- 5. Chủ đề phân cấp (Cây đệ quy)
-- Root 1: Tiếng Anh Giao Tiếp
INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Tiếng Anh Giao Tiếp', N'Các chủ đề giao tiếp thực tế hàng ngày', 1);
DECLARE @rootGiaoTiep INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@rootGiaoTiep, N'Chào hỏi & Giới thiệu bản thân', N'Từ vựng và mẫu câu khi mới gặp mặt', 1),
(@rootGiaoTiep, N'Mua sắm & Nhà hàng ẩm thực', N'Mẫu câu gọi món, trả giá, thanh toán', 1),
(@rootGiaoTiep, N'Du lịch & Phương tiện giao thông', N'Hỏi đường, thủ tục sân bay, khách sạn', 1);

-- Root 2: Tiếng Anh Học Thuật (IELTS & Academic)
INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Tiếng Anh Học Thuật (IELTS)', N'Luyện thi học thuật và các kỹ năng chuyên sâu', 1);
DECLARE @rootAcademic INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@rootAcademic, N'IELTS Reading — Skimming & Scanning', N'Kỹ năng đọc nhanh, tìm từ khóa', 1),
(@rootAcademic, N'IELTS Writing Task 2 — Luận xã hội', N'Cấu trúc bài luận 4 đoạn chuẩn học thuật', 1),
(@rootAcademic, N'IELTS Listening — Section 3 & 4', N'Hội thoại học thuật và bài giảng đại học', 1);

-- Root 3: Ngữ Pháp Cốt Lõi
INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Ngữ Pháp Cốt Lõi', N'Hệ thống ngữ pháp tiếng Anh từ căn bản đến nâng cao', 1);
DECLARE @rootGrammar INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@rootGrammar, N'Các thì trong tiếng Anh (Tenses)', N'12 thì và ứng dụng thực tiễn', 1),
(@rootGrammar, N'Mệnh đề quan hệ & Câu bị động', N'Ngữ pháp ghép câu và diễn đạt tự nhiên', 1),
(@rootGrammar, N'Câu điều kiện & Đảo ngữ', N'Ngữ pháp nâng cao cho band điểm cao', 1);

-- Root 4: Tiếng Anh Thương Mại (Business English)
INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Tiếng Anh Thương Mại & Công Sở', N'Tiếng Anh doanh nghiệp, đàm phán, email', 1);
DECLARE @rootBusiness INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@rootBusiness, N'Viết Email & Thư tín Thương mại', N'Quy chuẩn viết email chuyên nghiệp', 1),
(@rootBusiness, N'Đàm phán & Thuyết trình Dự án', N'Ngôn ngữ đàm phán và thuyết phục đối tác', 1);
GO

-- 6. Bài học mẫu liên kết chủ đề và trình độ
DECLARE @cdTenses INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Các thì%');
DECLARE @cdWriting INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS Writing Task 2%');
DECLARE @cdBusiness INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Viết Email%');
DECLARE @cdChaoHoi INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Chào hỏi%');

DECLARE @tdA1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A1');
DECLARE @tdB1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B1');
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');

DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDangNhap = N'tung.hoang');
IF @gvId IS NULL SET @gvId = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = (SELECT TOP 1 MaNguoiDung FROM NguoiDung);

INSERT INTO BaiHoc(MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES
(@cdTenses, @tdB1, @gvId, N'Thì Hiện Tại Hoàn Thành — Cấu trúc & Cách dùng', N'NguPhap', 
 N'Bài học trình bày chi tiết cấu trúc thì hiện tại hoàn thành, các trạng từ đi kèm (already, yet, ever, never, just) và cách phân biệt với thì quá khứ đơn.', 
 N'<h3>1. Định nghĩa</h3><p>Thì hiện tại hoàn thành diễn tả hành động bắt đầu trong quá khứ và còn tiếp diễn ở hiện tại...</p>', 20, 1),

(@cdWriting, @tdB2, @gvId, N'IELTS Mock Test #1 — Academic Reading & Listening', N'Doc', 
 N'Đề thi thử IELTS Academic dạng đầy đủ, gồm 4 phần Reading (40 câu) và Listening (40 câu). Tự động chấm điểm và phân tích điểm yếu sau khi nộp bài.', 
 N'<h3>IELTS Academic Reading Test</h3><p>Passage 1: Ancient Roman Concrete...</p>', 60, 1),

(@cdBusiness, @tdB2, @gvId, N'Từ vựng Kinh tế — Business English cho người đi làm', N'TuVung', 
 N'Tổng hợp 120 từ vựng kinh tế thường gặp trong môi trường công sở, hội thảo và email công việc. Kèm flashcard ôn tập và câu ví dụ thực tế.', 
 N'<h3>Từ vựng công sở thông dụng</h3><ul><li>Agenda: Chương trình nghị sự</li><li>Deadline: Hạn chót</li></ul>', 25, 0),

(@cdChaoHoi, @tdA1, @gvId, N'Giao tiếp cơ bản: Làm quen và giới thiệu bản thân', N'Nghe', 
 N'Học cách chào hỏi tự nhiên, giới thiệu tên tuổi, nghề nghiệp và sở thích bằng tiếng Anh chuẩn giọng bản xứ.', 
 N'<h3>Basic Greetings</h3><p>Hi, nice to meet you! My name is John...</p>', 15, 1);
GO

-- 7. Đề thi / Luyện tập
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');
DECLARE @cdWriting INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS Writing Task 2%');
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDangNhap = N'tung.hoang');
IF @gvId IS NULL SET @gvId = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = (SELECT TOP 1 MaNguoiDung FROM NguoiDung);

INSERT INTO BaiTapDeThi(MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES
(@cdWriting, @tdB2, N'IELTS Academic Practice Exam - Test 01', N'ThiThu', N'Đề thi thử học thuật chuẩn định dạng Cambridge IELTS', 60, 1, @gvId),
(@cdWriting, @tdB2, N'Luyện tập tổng hợp Ngữ pháp B1-B2', N'LuyenTap', N'50 câu trắc nghiệm ngữ pháp và cấu trúc câu', 45, 1, @gvId);
GO

-- 8. Yêu cầu sinh đề AI và Câu hỏi sinh bởi AI (để test KPI "Chờ duyệt")
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDangNhap = N'tung.hoang');
IF @gvId IS NULL SET @gvId = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = (SELECT TOP 1 MaNguoiDung FROM NguoiDung);

DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');

IF @gvId IS NOT NULL AND @tdB2 IS NOT NULL
BEGIN
    INSERT INTO YeuCauSinhDeAI(NguoiYeuCau, LoaiDauVao, NoiDungDauVao, MaTrinhDoMongMuon, SoLuongCauHoi, TrangThai, MoHinhAI)
    VALUES
    (@gvId, N'VanBan', N'Bài đọc về biến đổi khí hậu và năng lượng tái tạo', @tdB2, 4, N'DangXuLy', N'Gemini-Pro-1.5');
    DECLARE @ycId BIGINT = SCOPE_IDENTITY();

    IF @ycId IS NOT NULL
    BEGIN
        INSERT INTO CauHoiDoAISinh(MaYeuCauSinhDe, NoiDungCauHoiSinhRa, TrangThaiDuyet)
        VALUES
        (@ycId, N'What is the main topic of the passage regarding renewable energy?', N'DangCho'),
        (@ycId, N'According to paragraph 2, why are solar cells becoming more efficient?', N'DangCho'),
        (@ycId, N'The word "resilience" in line 14 is closest in meaning to...', N'DangCho'),
        (@ycId, N'Which of the following statements is NOT mentioned in the text?', N'DangCho');
    END
END
GO

PRINT N'Nạp dữ liệu mẫu Sprint 1 thành công!';
GO
