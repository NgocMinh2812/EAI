USE TiengAnhAI;
GO

-- 1. Vai trò người dùng
IF NOT EXISTS (SELECT 1 FROM VaiTro WHERE TenVaiTro = N'QuanTriVien')
    INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES (N'QuanTriVien', N'Quản trị viên quản lý nội dung, câu hỏi và hệ thống');
IF NOT EXISTS (SELECT 1 FROM VaiTro WHERE TenVaiTro = N'GiangVien')
    INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES (N'GiangVien', N'Giảng viên quản lý nội dung, câu hỏi và lớp học');
IF NOT EXISTS (SELECT 1 FROM VaiTro WHERE TenVaiTro = N'HocVien')
    INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES (N'HocVien', N'Người học tiếng Anh ôn luyện và thi thử');
GO

-- 2. Trình độ chuẩn CEFR
IF NOT EXISTS (SELECT 1 FROM TrinhDo WHERE MaCapDo = N'A1')
    INSERT INTO TrinhDo (MaCapDo, TenTrinhDo, MoTa) VALUES (N'A1', N'Beginner', N'Trình độ mất gốc / bắt đầu');
IF NOT EXISTS (SELECT 1 FROM TrinhDo WHERE MaCapDo = N'A2')
    INSERT INTO TrinhDo (MaCapDo, TenTrinhDo, MoTa) VALUES (N'A2', N'Elementary', N'Trình độ sơ cấp');
IF NOT EXISTS (SELECT 1 FROM TrinhDo WHERE MaCapDo = N'B1')
    INSERT INTO TrinhDo (MaCapDo, TenTrinhDo, MoTa) VALUES (N'B1', N'Intermediate', N'Trình độ trung cấp');
IF NOT EXISTS (SELECT 1 FROM TrinhDo WHERE MaCapDo = N'B2')
    INSERT INTO TrinhDo (MaCapDo, TenTrinhDo, MoTa) VALUES (N'B2', N'Upper-Intermediate', N'Trình độ trên trung cấp');
IF NOT EXISTS (SELECT 1 FROM TrinhDo WHERE MaCapDo = N'C1')
    INSERT INTO TrinhDo (MaCapDo, TenTrinhDo, MoTa) VALUES (N'C1', N'Advanced', N'Trình độ nâng cao');
GO

-- 3. Chủ đề
IF NOT EXISTS (SELECT 1 FROM ChuDe WHERE TenChuDe = N'Đời sống')
    INSERT INTO ChuDe (MaChuDeCha, TenChuDe, MoTa) VALUES (NULL, N'Đời sống', N'Chủ đề tiếng Anh giao tiếp hàng ngày');
IF NOT EXISTS (SELECT 1 FROM ChuDe WHERE TenChuDe = N'Học thuật')
    INSERT INTO ChuDe (MaChuDeCha, TenChuDe, MoTa) VALUES (NULL, N'Học thuật', N'Tiếng Anh học thuật IELTS & TOEFL');
IF NOT EXISTS (SELECT 1 FROM ChuDe WHERE TenChuDe = N'Kinh tế & Công sở')
    INSERT INTO ChuDe (MaChuDeCha, TenChuDe, MoTa) VALUES (NULL, N'Kinh tế & Công sở', N'Tiếng Anh doanh nghiệp TOEIC');
IF NOT EXISTS (SELECT 1 FROM ChuDe WHERE TenChuDe = N'Khoa học công nghệ')
    INSERT INTO ChuDe (MaChuDeCha, TenChuDe, MoTa) VALUES (NULL, N'Khoa học công nghệ', N'Tiếng Anh lĩnh vực IT & Khoa học');
GO

-- 4. Người dùng mẫu (Mật khẩu mặc định: 123456 -> SHA-256: 8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92)
DECLARE @MaVT_GV INT = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro IN (N'QuanTriVien', N'GiangVien'));
DECLARE @MaVT_HV INT = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'HocVien');

UPDATE NguoiDung 
SET MatKhauMaHoa = N'8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92'
WHERE MatKhauMaHoa = N'CHANGE_ME_HASH' OR MatKhauMaHoa = N'123456';

IF NOT EXISTS (SELECT 1 FROM NguoiDung WHERE TenDangNhap = N'quantri')
BEGIN
    INSERT INTO NguoiDung (MaVaiTro, TenDangNhap, MatKhauMaHoa, Email, HoTen, TrangThaiHoatDong)
    VALUES (@MaVT_GV, N'quantri', N'8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92', N'admin@tienganhai.local', N'TS. Hoàng Văn Tùng', 1);
END

IF NOT EXISTS (SELECT 1 FROM NguoiDung WHERE TenDangNhap = N'giangvien')
BEGIN
    INSERT INTO NguoiDung (MaVaiTro, TenDangNhap, MatKhauMaHoa, Email, HoTen, TrangThaiHoatDong)
    VALUES (@MaVT_GV, N'giangvien', N'8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92', N'giangvien@tienganhai.local', N'ThS. Lê Thu Hà', 1);
END

IF NOT EXISTS (SELECT 1 FROM NguoiDung WHERE TenDangNhap = N'hocvien01')
BEGIN
    INSERT INTO NguoiDung (MaVaiTro, TenDangNhap, MatKhauMaHoa, Email, HoTen, TrangThaiHoatDong, LaLanDauDangNhap)
    VALUES (@MaVT_HV, N'hocvien01', N'8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92', N'hocvien01@tienganhai.local', N'Nguyễn Văn An', 1, 0);

    DECLARE @MaND_HV1 INT = (SELECT MaNguoiDung FROM NguoiDung WHERE TenDangNhap = N'hocvien01');
    INSERT INTO HoSoHocTap (MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, SoPhutHocTichLuy)
    VALUES (@MaND_HV1, 6.5, N'IELTS', N'B2', 450);

    -- Đánh dấu học viên 01 đã từng làm bài khảo sát
    IF NOT EXISTS (SELECT 1 FROM LuotKhaoSat WHERE MaNguoiDung = @MaND_HV1)
    BEGIN
        INSERT INTO LuotKhaoSat (MaNguoiDung, MaBaiKhaoSat, ThoiDiemBatDau, ThoiDiemNopBai, TongSoCau, SoCauDung, DiemDatDuoc, MaTrinhDoKetQua, KhungDanhGiaKetQua)
        VALUES (@MaND_HV1, 1, DATEADD(DAY, -1, SYSDATETIME()), SYSDATETIME(), 15, 12, 8.0, 
                (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2'), N'CEFR');
    END
END
ELSE
BEGIN
    UPDATE NguoiDung SET LaLanDauDangNhap = 0 WHERE TenDangNhap = N'hocvien01';
END

IF NOT EXISTS (SELECT 1 FROM NguoiDung WHERE TenDangNhap = N'hocvien_moi')
BEGIN
    INSERT INTO NguoiDung (MaVaiTro, TenDangNhap, MatKhauMaHoa, Email, HoTen, TrangThaiHoatDong, LaLanDauDangNhap)
    VALUES (@MaVT_HV, N'hocvien_moi', N'8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92', N'hocvienmoi@tienganhai.local', N'Trần Minh Đức', 1, 1);
END
ELSE
BEGIN
    UPDATE NguoiDung SET LaLanDauDangNhap = 1 WHERE TenDangNhap = N'hocvien_moi';
END
GO

-- 5. Bài khảo sát đầu vào (Placement Test)
IF NOT EXISTS (SELECT 1 FROM BaiKhaoSatDauVao WHERE MaBaiKhaoSat = 1)
BEGIN
    SET IDENTITY_INSERT BaiKhaoSatDauVao ON;
    INSERT INTO BaiKhaoSatDauVao (MaBaiKhaoSat, TenBaiKhaoSat, MoTa, TongSoCauHoi, DangHoatDong)
    VALUES (1, N'Bài khảo sát năng lực tiếng Anh đầu vào (Placement Test)', N'Kiểm tra đa kỹ năng: Ngữ pháp, Từ vựng, Đọc hiểu giúp phân loại trình độ CEFR (A1-B2) và xây dựng lộ trình học cá nhân hóa.', 15, 1);
    SET IDENTITY_INSERT BaiKhaoSatDauVao OFF;
END
GO

-- 6. Bộ câu hỏi khảo sát năng lực (15 câu chuẩn hóa)
-- Xóa câu hỏi khảo sát cũ nếu có để nạp mới đồng bộ
DELETE FROM CauTraLoiKhaoSat WHERE MaLuotKhaoSat IN (SELECT MaLuotKhaoSat FROM LuotKhaoSat WHERE MaBaiKhaoSat = 1);
DELETE FROM LuotKhaoSat WHERE MaBaiKhaoSat = 1;
DELETE FROM CauHoiKhaoSat WHERE MaBaiKhaoSat = 1;

DECLARE @MaTD_A1 INT = (SELECT MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A1');
DECLARE @MaTD_A2 INT = (SELECT MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A2');
DECLARE @MaTD_B1 INT = (SELECT MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B1');
DECLARE @MaTD_B2 INT = (SELECT MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');

DECLARE @MaCD_DoiSong INT = (SELECT MaChuDe FROM ChuDe WHERE TenChuDe = N'Đời sống');
DECLARE @MaCD_HocThuat INT = (SELECT MaChuDe FROM ChuDe WHERE TenChuDe = N'Học thuật');
DECLARE @MaCD_KinhTe INT = (SELECT MaChuDe FROM ChuDe WHERE TenChuDe = N'Kinh tế & Công sở');
DECLARE @MaCD_CongNghe INT = (SELECT MaChuDe FROM ChuDe WHERE TenChuDe = N'Khoa học công nghệ');

-- Helper Procedure or Direct Inserts
-- Câu 1: Ngữ pháp A1 - To be
DECLARE @QId INT;
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_DoiSong, @MaTD_A1, N'TracNghiem', N'NguPhap', N'They ______ from England. They are visiting Vietnam for their summer holiday.', N'Chủ ngữ "They" là đại từ số nhiều đi với động từ to be "are".', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'is', 0), (@QId, N'B', N'are', 1), (@QId, N'C', N'am', 0), (@QId, N'D', N'be', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 1);

-- Câu 2: Ngữ pháp A2 - Quá khứ đơn
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_DoiSong, @MaTD_A2, N'TracNghiem', N'NguPhap', N'Yesterday, she ______ to the supermarket to buy some fresh vegetables.', N'Dấu hiệu thời gian "Yesterday" chỉ hành động đã xảy ra trong quá khứ -> Dùng "went".', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'go', 0), (@QId, N'B', N'goes', 0), (@QId, N'C', N'went', 1), (@QId, N'D', N'gone', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 2);

-- Câu 3: Từ vựng A2 - Nghề nghiệp / Đời sống
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_DoiSong, @MaTD_A2, N'TracNghiem', N'TuVung', N'The doctor advised him to take this ______ twice a day after meals.', N'"medicine" nghĩa là thuốc uống, phù hợp ngữ cảnh bác sĩ dặn dò.', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'medicine', 1), (@QId, N'B', N'ingredient', 0), (@QId, N'C', N'recipe', 0), (@QId, N'D', N'equipment', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 3);

-- Câu 4: Ngữ pháp B1 - Thì hiện tại hoàn thành
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_HocThuat, @MaTD_B1, N'TracNghiem', N'NguPhap', N'Mr. David ______ in this international school since 2018.', N'Dấu hiệu "since 2018" diễn tả hành động bắt đầu trong quá khứ kéo dài đến hiện tại -> Present Perfect "has worked".', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'worked', 0), (@QId, N'B', N'has worked', 1), (@QId, N'C', N'is working', 0), (@QId, N'D', N'works', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 4);

-- Câu 5: Ngữ pháp B1 - Cụm giới từ / Collocation
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_HocThuat, @MaTD_B1, N'TracNghiem', N'NguPhap', N'She has always been capable ______ solving complicated mathematical equations under pressure.', N'Cấu trúc cố định: "capable of + V-ing" (có khả năng làm gì).', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'with', 0), (@QId, N'B', N'in', 0), (@QId, N'C', N'of', 1), (@QId, N'D', N'for', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 5);

-- Câu 6: Từ vựng B1 - Công sở
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_KinhTe, @MaTD_B1, N'TracNghiem', N'TuVung', N'All candidates must submit their job application before the strict ______ on Friday.', N'"deadline" nghĩa là hạn chót nộp hồ sơ.', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'deadline', 1), (@QId, N'B', N'destination', 0), (@QId, N'C', N'dimension', 0), (@QId, N'D', N'description', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 6);

-- Câu 7: Ngữ pháp B2 - Câu điều kiện loại 3
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_HocThuat, @MaTD_B2, N'TracNghiem', N'NguPhap', N'If I ______ more time yesterday, I would have traveled around the ancient city with you.', N'Câu điều kiện loại 3 diễn tả giả định trái với quá khứ: If + S + had + P2 -> "had had".', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'have', 0), (@QId, N'B', N'had had', 1), (@QId, N'C', N'had', 0), (@QId, N'D', N'would have', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 7);

-- Câu 8: Từ vựng B2 - Học thuật
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_HocThuat, @MaTD_B2, N'TracNghiem', N'TuVung', N'The university board decided to ______ innovative teaching methods to stimulate students'' critical thinking.', N'"adopt" nghĩa là áp dụng, chọn dùng phương pháp mới. Phân biệt với "adapt" (thích nghi).', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'adopt', 1), (@QId, N'B', N'adapt', 0), (@QId, N'C', N'adept', 0), (@QId, N'D', N'admit', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 8);

-- Câu 9: Ngữ pháp B2 - Đảo ngữ
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_HocThuat, @MaTD_B2, N'TracNghiem', N'NguPhap', N'Hardly ______ home when the torrential rain began to pour.', N'Cấu trúc đảo ngữ: "Hardly + had + S + P2 + when + clause".', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'she arrived', 0), (@QId, N'B', N'had she arrived', 1), (@QId, N'C', N'she had arrived', 0), (@QId, N'D', N'did she arrive', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 9);

-- Câu 10: Từ vựng B2 - Thành ngữ / Cụm từ
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_KinhTe, @MaTD_B2, N'TracNghiem', N'TuVung', N'When managing a large-scale project, it is crucial not to get bogged down in the ______.', N'Thành ngữ: "get bogged down in the details" (bị sa đà vào các chi tiết vụn vặt).', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'details', 1), (@QId, N'B', N'skies', 0), (@QId, N'C', N'moments', 0), (@QId, N'D', N'pictures', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 10);

-- Câu 11: Ngữ pháp B1 - Mệnh đề quan hệ
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_HocThuat, @MaTD_B1, N'TracNghiem', N'NguPhap', N'The scientist ______ discoveries revolutionized modern medicine was awarded the Nobel Prize.', N'Đại từ quan hệ sở hữu "whose discoveries" (người có những phát kiến).', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'who', 0), (@QId, N'B', N'whose', 1), (@QId, N'C', N'whom', 0), (@QId, N'D', N'which', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 11);

-- Câu 12: Từ vựng B2 - Kinh tế
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_KinhTe, @MaTD_B2, N'TracNghiem', N'TuVung', N'The sudden ______ in consumer demand led to widespread shortages across major retail markets.', N'"surge" nghĩa là sự gia tăng đột biến, mạnh mẽ.', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'surge', 1), (@QId, N'B', N'pause', 0), (@QId, N'C', N'decay', 0), (@QId, N'D', N'recession', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 12);

-- Câu 13, 14, 15: Bài Đọc hiểu (Reading Comprehension) B1-B2
DECLARE @PassageText NVARCHAR(MAX) = N'Artificial Intelligence (AI) is rapidly transforming modern language education by providing personalized pathways tailored to each student''s pace. Unlike traditional one-size-fits-all classrooms, adaptive AI models continuously analyze user responses, identify lexical and grammatical bottlenecks, and immediately adjust exercise difficulty. Furthermore, generative AI conversational agents simulate immersive dialogue environments, allowing learners to build speaking and writing confidence without fear of judgment. However, educational experts emphasize that AI should complement rather than fully replace human instructors, who remain essential for fostering critical thinking and intercultural empathy.';

-- Câu 13: Đọc hiểu - Ý chính
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoanVanDoc, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_CongNghe, @MaTD_B2, N'TracNghiem', N'Doc', @PassageText, N'What is the primary advantage of AI in language learning discussed in the first paragraph?', N'Đoạn văn nêu rõ AI cung cấp lộ trình học cá nhân hóa thích ứng theo tốc độ của từng học viên.', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'It guarantees immediate native fluency within a few weeks.', 0),
(@QId, N'B', N'It provides personalized pathways tailored to each student''s pace.', 1),
(@QId, N'C', N'It makes human teachers completely unnecessary.', 0),
(@QId, N'D', N'It standardizes all test materials into a single fixed format.', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 13);

-- Câu 14: Đọc hiểu - Chi tiết
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoanVanDoc, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_CongNghe, @MaTD_B2, N'TracNghiem', N'Doc', @PassageText, N'How do generative AI conversational agents assist language learners according to the passage?', N'Bài đọc ghi: "allowing learners to build speaking and writing confidence without fear of judgment".', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'By eliminating the need to learn grammar structures.', 0),
(@QId, N'B', N'By giving students real monetary rewards for correct answers.', 0),
(@QId, N'C', N'By simulating immersive environments to practice without fear of judgment.', 1),
(@QId, N'D', N'By grading national graduation exams automatically.', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 14);

-- Câu 15: Đọc hiểu - Suy luận & Vai trò giáo viên
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoanVanDoc, NoiDungCauHoi, GiaiThichDapAn, DaKiemDuyet)
VALUES (@MaCD_CongNghe, @MaTD_B2, N'TracNghiem', N'Doc', @PassageText, N'According to educational experts, what role should human instructors play in AI-driven education?', N'Các chuyên gia nhấn mạnh giáo viên đóng vai trò rèn luyện tư duy phản biện và sự thấu cảm văn hóa.', 1);
SET @QId = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@QId, N'A', N'They remain vital for nurturing critical thinking and intercultural empathy.', 1),
(@QId, N'B', N'They should only focus on administrative computer maintenance.', 0),
(@QId, N'C', N'They should stop teaching grammar completely.', 0),
(@QId, N'D', N'They should be replaced as soon as possible.', 0);
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES (1, @QId, 15);
-- 7. Nạp danh mục Bài học học tập thực tế (BaiHoc)
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE TieuDe LIKE N'%Conditional Sentences%')
BEGIN
    INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
    VALUES 
    (@MaCD_HocThuat, @MaTD_B2, N'Grammar B2 - Conditional Sentences (Câu điều kiện)', N'NguPhap', 
     N'Nắm vững cấu trúc, cách dùng câu điều kiện loại 0, 1, 2, 3 và câu điều kiện hỗn hợp (Mixed Conditionals).',
     N'### 1. Tổng quan về Câu điều kiện
Câu điều kiện bao gồm 2 mệnh đề: Mệnh đề If (If-clause) và Mệnh đề chính (Main clause).

### 2. Các loại câu điều kiện cơ bản
- **Loại 0**: If + S + V(s/es), S + V(s/es) -> Sự thật hiển nhiên.
- **Loại 1**: If + S + V(s/es), S + will + V -> Có thể xảy ra ở hiện tại hoặc tương lai.
- **Loại 2**: If + S + V2/ed, S + would + V -> Trái thực tế ở hiện tại.
- **Loại 3**: If + S + had + P2, S + would have + P2 -> Trái thực tế trong quá khứ.', 20, 1),

    (@MaCD_HocThuat, (SELECT MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'C1'), N'Vocabulary C1 - Academic & Essay Collocations', N'TuVung',
     N'Bộ sưu tập 30 cụm từ học thuật C1 ghi điểm tuyệt đối trong bài thi IELTS Writing Task 2 & VSTEP C1.',
     N'### 1. Academic Collocations C1
- **Substantial evidence**: Bằng chứng đáng kể.
- **Inexorably linked**: Gắn liền không thể tách rời.
- **Catalyst for change**: Chất xúc tác cho sự thay đổi.', 25, 1),

    (@MaCD_CongNghe, @MaTD_B2, N'Reading B2 - The Renaissance of Clean Energy', N'Doc',
     N'Rèn luyện kỹ năng Skimming, Scanning và suy luận từ ngữ cảnh qua bài đọc chủ đề Năng lượng tái tạo.',
     N'### Clean Energy Technologies
Solar photovoltaic and wind turbine deployments reached historic records this past decade...', 15, 1),

    (@MaCD_DoiSong, @MaTD_B1, N'Grammar B1 - Present Perfect vs Past Simple', N'NguPhap',
     N'Phân biệt thì Hiện tại hoàn thành và Quá khứ đơn, các trạng từ chỉ thời gian thường gặp.',
     N'### Present Perfect vs Past Simple
- Past Simple: Dùng cho hành động đã kết thúc tại một thời điểm xác định trong quá khứ.
- Present Perfect: Dùng cho hành động xảy ra tại thời điểm không xác định hoặc kéo dài đến hiện tại.', 15, 1),

    (@MaCD_KinhTe, @MaTD_B1, N'Vocabulary B1 - Workplace Communication & Emails', N'TuVung',
     N'Tổng hợp từ vựng và mẫu câu giao tiếp viết email công việc chuyên nghiệp chuẩn văn phong công sở.',
     N'### Professional Email Phrasing
- "I am writing to inquire about..."
- "Please find attached the requested invoice..."', 18, 1),

    (@MaCD_DoiSong, @MaTD_A1, N'Grammar A1 - Personal Pronouns & To Be Verb', N'NguPhap',
     N'Làm quen với hệ thống đại từ nhân xưng I, You, We, They, He, She, It và cách chia động từ To Be.',
     N'### 1. Đại từ nhân xưng (Subject Pronouns)
- I (tôi), You (bạn), We (chúng tôi), They (họ)
- He (anh ấy), She (cô ấy), It (nó)

### 2. Động từ To Be ở hiện tại
- I + am
- He / She / It + is
- You / We / They + are', 12, 1),

    (@MaCD_DoiSong, @MaTD_A1, N'Vocabulary A1 - Daily Greetings & Introductions', N'TuVung',
     N'20 câu chào hỏi thông dụng và cách tự giới thiệu tên, tuổi, nghề nghiệp, quê quán bằng tiếng Anh.',
     N'### Useful Phrases
- "Nice to meet you!"
- "Where are you from?"
- "I am a student at university."', 10, 1),

    (@MaCD_DoiSong, @MaTD_A2, N'Grammar A2 - Past Simple Tense & Irregular Verbs', N'NguPhap',
     N'Quy tắc thêm -ed và bảng 50 động từ bất quy tắc thông dụng nhất trong giao tiếp hàng ngày.',
     N'### Past Simple Structure
- Khẳng định: S + V2/ed + O
- Phủ định: S + did not (didn''t) + V-bare
- Nghi vấn: Did + S + V-bare?', 15, 1),

    (@MaCD_DoiSong, @MaTD_A2, N'Vocabulary A2 - Shopping & Supermarket Essentials', N'TuVung',
     N'Từ vựng về mua sắm hàng hóa, hỏi giá tiền, mặc cả và phương thức thanh toán.',
     N'### Shopping Vocabulary
- Cash, credit card, discount, receipt, refund
- "How much does this item cost?"', 15, 1);
END
GO

PRINT N'Hoàn tất nạp dữ liệu Sprint 1 vào CSDL TiengAnhAI!';
