-- ====================================================================================
-- SEED DATA: CẤU TRÚC ĐỀ THI CHUẨN HÓA (EXAM BLUEPRINTS)
-- Dự án: EnglishAI (EAI) Desktop Application
-- Quy chuẩn: IELTS, TOEIC, VSTEP, Cambridge KET, PET, FCE, CAE, THPT Quốc Gia
-- Bảo đảm: Idempotent (Chạy nhiều lần không trùng lặp, cập nhật chuẩn xác)
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Bắt đầu nạp Exam Blueprints chuẩn hóa...';

-- 1. IELTS ACADEMIC FULL EXAM BLUEPRINT (160 PHÚT - 4 KỸ NĂNG)
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'IELTS' AND TenCauTruc = N'IELTS Academic Full Examination Blueprint')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'IELTS Academic Full Examination Blueprint',
        'IELTS', 'B2', 'ThiThu', 160, 40, 10.0,
        N'Cấu trúc bài thi IELTS Học thuật chuẩn quốc tế gồm 4 kỹ năng: Listening (4 Sections), Reading (3 Passages), Writing (2 Tasks) và Speaking (3 Parts).',
        N'Cambridge English Language Assessment & British Council IELTS Specifications',
        SYSDATETIME(), 'HoatDong'
    );
    DECLARE @IdIeltsFull INT = SCOPE_IDENTITY();

    INSERT INTO ChiTietCauTrucDeThi (MaCauTruc, TenPhan, ThuTuPhan, KyNang, LoaiCauHoiChoPhep, SoLuongCau, DiemMoiCau, ThoiGianPhut, YeuCauDoanVan, YeuCauAudio, MoTa)
    VALUES 
        (@IdIeltsFull, N'Section 1: Listening Social Dialogue', 1, 'Nghe', 'DienKhuyet,TracNghiem', 10, 0.25, 10, 0, 1, N'Hội thoại đời sống thường nhật giữa 2 người (đặt phòng, hỏi thông tin dịch vụ).'),
        (@IdIeltsFull, N'Section 2: Listening Public Monologue', 2, 'Nghe', 'TracNghiem,NoiCap', 10, 0.25, 10, 0, 1, N'Bài độc thoại hướng dẫn về tiện ích công cộng hoặc sự kiện văn hóa địa phương.'),
        (@IdIeltsFull, N'Section 3: Listening Educational Discussion', 3, 'Nghe', 'TracNghiem,NoiCap', 10, 0.25, 10, 0, 1, N'Thảo luận học thuật từ 2-4 người (sinh viên và giáo sư hướng dẫn đề tài nghiên cứu).'),
        (@IdIeltsFull, N'Section 4: Listening Academic Lecture', 4, 'Nghe', 'DienKhuyet', 10, 0.25, 10, 0, 1, N'Bài giảng học thuật độc thoại chuyên ngành (khoa học, lịch sử, môi trường).'),
        (@IdIeltsFull, N'Passage 1: Academic Reading Foundation', 5, 'Doc', 'TrueFalseNotGiven,TracNghiem,DienKhuyet', 13, 0.25, 20, 1, 0, N'Văn bản đọc học thuật mô tả thực tế, sự kiện lịch sử hoặc tiểu sử khoa học.'),
        (@IdIeltsFull, N'Passage 2: Academic Reading Analysis', 6, 'Doc', 'TracNghiem,NoiCap,DienKhuyet', 13, 0.25, 20, 1, 0, N'Bài đọc phân tích quy trình công nghệ hoặc thảo luận chuyên ngành.'),
        (@IdIeltsFull, N'Passage 3: Complex Discussion & Argument', 7, 'Doc', 'TrueFalseNotGiven,TracNghiem', 14, 0.25, 20, 1, 0, N'Văn bản lập luận phức hợp với các quan điểm đối lập nhau.'),
        (@IdIeltsFull, N'Task 1: Academic Writing Data Report', 8, 'Viet', 'TuLuan', 1, 3.33, 20, 0, 0, N'Miêu tả và phân tích biểu đồ, bảng biểu hoặc quy trình (tối thiểu 150 từ).'),
        (@IdIeltsFull, N'Task 2: Discursive Essay', 9, 'Viet', 'TuLuan', 1, 6.67, 40, 0, 0, N'Viết bài luận nghị luận xã hội về các vấn đề toàn cầu (tối thiểu 250 từ).');
END
ELSE
BEGIN
    UPDATE CauTrucDeThi
    SET ThoiGianPhut = 160, TongSoCau = 40, TongDiem = 10.0
    WHERE MaKyThi = 'IELTS' AND TenCauTruc = N'IELTS Academic Full Examination Blueprint';
END
GO

-- 2. IELTS ACADEMIC READING BLUEPRINT (40 CÂU - 60 PHÚT CHUẨN QUỐC TẾ)
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 2)
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'IELTS Academic Reading Examination Blueprint',
        'IELTS', 'B2', 'ThiThu', 60, 40, 10.0,
        N'Cấu trúc bài thi IELTS Academic Reading chuẩn quốc tế gồm 40 câu hỏi phân bổ qua 3 bài đọc học thuật (Passages 1, 2, 3), đánh giá toàn diện kỹ năng Skimming, Scanning, suy luận logic và vốn từ vựng học thuật B2-C1.',
        N'IELTS Academic Official Test Guidelines (British Council & IDP)',
        SYSDATETIME(), 'HoatDong'
    );
END
ELSE
BEGIN
    UPDATE CauTrucDeThi
    SET TenCauTruc = N'IELTS Academic Reading Examination Blueprint',
        ThoiGianPhut = 60,
        TongSoCau = 40,
        TongDiem = 10.0,
        MoTa = N'Cấu trúc bài thi IELTS Academic Reading chuẩn quốc tế gồm 40 câu hỏi phân bổ qua 3 bài đọc học thuật (Passages 1, 2, 3), đánh giá toàn diện kỹ năng Skimming, Scanning, suy luận logic và vốn từ vựng học thuật B2-C1.'
    WHERE MaCauTruc = 2;
END

DELETE FROM ChiTietCauTrucDeThi WHERE MaCauTruc = 2;
INSERT INTO ChiTietCauTrucDeThi (MaCauTruc, TenPhan, ThuTuPhan, KyNang, LoaiCauHoiChoPhep, SoLuongCau, DiemMoiCau, ThoiGianPhut, YeuCauDoanVan, YeuCauAudio, MoTa)
VALUES 
    (2, N'Passage 1: Academic Reading Foundation & Core Facts', 1, 'Doc', 'TracNghiem,DungSai,DienKhuyet', 13, 0.25, 20, 1, 0, N'Bài đọc học thuật giới thiệu chủ đề thực tế, tiểu sử hoặc lịch sử khoa học.'),
    (2, N'Passage 2: Technology, Science & Research Analysis', 2, 'Doc', 'TracNghiem,NoiCap,DienKhuyet', 13, 0.25, 20, 1, 0, N'Bài đọc chuyên ngành công nghệ, y sinh hoặc trí tuệ nhân tạo.'),
    (2, N'Passage 3: Discursive Argument & Complex Critical Thinking', 3, 'Doc', 'TracNghiem,DungSai,DienKhuyet', 14, 0.25, 20, 1, 0, N'Bài đọc phân tích các luồng quan điểm đối lập, suy luận học thuật chuyên sâu.');
GO

-- 3. TOEIC LISTENING & READING FULL BLUEPRINT (ETS STANDARD 200 CÂU)
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'TOEIC' AND TenCauTruc = N'TOEIC Listening & Reading 200 Questions Blueprint')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'TOEIC Listening & Reading 200 Questions Blueprint',
        'TOEIC', 'B1', 'ThiThu', 120, 200, 990.0,
        N'Cấu trúc đề thi TOEIC Quốc tế 2 kỹ năng Nghe & Đọc chuẩn ETS gồm 7 Part đánh giá năng lực tiếng Anh trong môi trường doanh nghiệp quốc tế.',
        N'ETS (Educational Testing Service) TOEIC Test Specifications',
        SYSDATETIME(), 'HoatDong'
    );
END
GO

-- 4. TOEIC READING TEST PART 5 & 6 BLUEPRINT (40 CÂU - 45 PHÚT)
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 4)
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'TOEIC Reading Test Part 5 & 6 Blueprint',
        'TOEIC', 'B1', 'ThiThu', 45, 40, 10.0,
        N'Cấu trúc bài thi TOEIC Reading chuẩn hóa Phần 5 & 6 gồm 40 câu hỏi: Kiểm tra ngữ pháp thực hành công sở, thì động từ, liên từ, từ loại và đọc điền đoạn văn bản thương mại.',
        N'ETS TOEIC Reading Test Handbook',
        SYSDATETIME(), 'HoatDong'
    );
END
ELSE
BEGIN
    UPDATE CauTrucDeThi
    SET TenCauTruc = N'TOEIC Reading Test Part 5 & 6 Blueprint',
        ThoiGianPhut = 45,
        TongSoCau = 40,
        TongDiem = 10.0,
        MoTa = N'Cấu trúc bài thi TOEIC Reading chuẩn hóa Phần 5 & 6 gồm 40 câu hỏi: Kiểm tra ngữ pháp thực hành công sở, thì động từ, liên từ, từ loại và đọc điền đoạn văn bản thương mại.'
    WHERE MaCauTruc = 4;
END

DELETE FROM ChiTietCauTrucDeThi WHERE MaCauTruc = 4;
INSERT INTO ChiTietCauTrucDeThi (MaCauTruc, TenPhan, ThuTuPhan, KyNang, LoaiCauHoiChoPhep, SoLuongCau, DiemMoiCau, ThoiGianPhut, YeuCauDoanVan, YeuCauAudio, MoTa)
VALUES 
    (4, N'Part 5: Incomplete Sentences (Hoàn thành câu thương mại)', 1, 'NguPhap', 'TracNghiem', 28, 0.25, 25, 0, 0, N'Ngữ pháp công sở, từ loại, thì động từ và từ vựng hợp đồng doanh nghiệp.'),
    (4, N'Part 6: Text Completion (Điền đoạn văn bản công sở)', 2, 'Doc', 'TracNghiem,DienKhuyet', 12, 0.25, 20, 1, 0, N'Đọc hiểu điền thông báo, email và bản tin công ty.');
GO

-- 5. VSTEP B1-B2-C1 FULL BLUEPRINT (BỘ GD&ĐT VIỆT NAM - 82 CÂU)
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'VSTEP' AND TenCauTruc = N'VSTEP B1-B2-C1 Examination Blueprint')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'VSTEP B1-B2-C1 Examination Blueprint',
        'VSTEP', 'B2', 'ThiThu', 180, 82, 10.0,
        N'Định dạng đề thi Đánh giá năng lực tiếng Anh bậc 3-5 theo Khung năng lực ngoại ngữ 6 bậc dùng cho Việt Nam gồm đầy đủ 4 kỹ năng.',
        N'Quyết định số 729/QĐ-BGDĐT của Bộ Giáo dục và Đào tạo',
        SYSDATETIME(), 'HoatDong'
    );
END
GO

-- 6. VSTEP B2 STANDARD READING & LANGUAGE BLUEPRINT (40 CÂU - 60 PHÚT)
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaCauTruc = 6)
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'VSTEP B2 Standard Reading & Language Blueprint',
        'VSTEP', 'B2', 'ThiThu', 60, 40, 10.0,
        N'Cấu trúc đề thi Đánh giá năng lực tiếng Anh Bậc 4 (VSTEP B2) chuẩn Bộ Giáo dục & Đào tạo gồm 40 câu hỏi phân bổ qua các phần đọc hiểu và kiến thức ngôn ngữ học thuật chuyên sâu.',
        N'Khung ĐGNL Tiếng Anh Bậc 4 VSTEP - ĐHQGHN & Bộ GD&ĐT',
        SYSDATETIME(), 'HoatDong'
    );
END
ELSE
BEGIN
    UPDATE CauTrucDeThi
    SET TenCauTruc = N'VSTEP B2 Standard Reading & Language Blueprint',
        ThoiGianPhut = 60,
        TongSoCau = 40,
        TongDiem = 10.0,
        MoTa = N'Cấu trúc đề thi Đánh giá năng lực tiếng Anh Bậc 4 (VSTEP B2) chuẩn Bộ Giáo dục & Đào tạo gồm 40 câu hỏi phân bổ qua các phần đọc hiểu và kiến thức ngôn ngữ học thuật chuyên sâu.'
    WHERE MaCauTruc = 6;
END

DELETE FROM ChiTietCauTrucDeThi WHERE MaCauTruc = 6;
INSERT INTO ChiTietCauTrucDeThi (MaCauTruc, TenPhan, ThuTuPhan, KyNang, LoaiCauHoiChoPhep, SoLuongCau, DiemMoiCau, ThoiGianPhut, YeuCauDoanVan, YeuCauAudio, MoTa)
VALUES 
    (6, N'Phần 1: Ngữ pháp phức hợp, Đảo ngữ & Cấu trúc B2', 1, 'NguPhap', 'TracNghiem,SuaLoiSai,VietLaiCau', 10, 0.25, 12, 0, 0, N'Mệnh đề quan hệ rút gọn, đảo ngữ điều kiện, bị động nâng cao.'),
    (6, N'Phần 2: Đọc hiểu văn bản Khoa học & Công nghệ', 2, 'Doc', 'TracNghiem,DienKhuyet', 10, 0.25, 16, 1, 0, N'Đọc hiểu tài liệu khoa học và công nghệ hiện đại.'),
    (6, N'Phần 3: Đọc hiểu Môi trường & Phát triển bền vững', 3, 'Doc', 'TracNghiem,DienKhuyet', 10, 0.25, 16, 1, 0, N'Đọc hiểu biến đổi khí hậu, sinh thái và bảo tồn.'),
    (6, N'Phần 4: Đọc hiểu Văn hóa, Xã hội & Nghị luận', 4, 'Doc', 'TracNghiem,DungSai', 10, 0.25, 16, 1, 0, N'Văn bản phân tích tác động xã hội và tư duy biện luận.');
GO

-- 7. CAMBRIDGE A2 KEY (KET) BLUEPRINT (30 CÂU - 45 PHÚT)
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'CAMBRIDGE_KET')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'Cambridge A2 Key (KET) Examination Blueprint',
        'CAMBRIDGE_KET', 'A2', 'ThiThu', 45, 30, 10.0,
        N'Cấu trúc bài thi quốc tế Cambridge A2 Key đánh giá khả năng giao tiếp đơn giản và đọc hiểu trong các tình huống thường nhật.',
        N'Cambridge Assessment English - A2 Key Handbook for Teachers',
        SYSDATETIME(), 'HoatDong'
    );
END
ELSE
BEGIN
    UPDATE CauTrucDeThi
    SET ThoiGianPhut = 45,
        TongSoCau = 30
    WHERE MaKyThi = 'CAMBRIDGE_KET';
END
GO

-- 8. CAMBRIDGE B1 PRELIMINARY (PET) BLUEPRINT (67 CÂU - 130 PHÚT)
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'CAMBRIDGE_PET')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'Cambridge B1 Preliminary (PET) Examination Blueprint',
        'CAMBRIDGE_PET', 'B1', 'ThiThu', 130, 67, 10.0,
        N'Cấu trúc bài thi quốc tế Cambridge B1 Preliminary chứng nhận khả năng nắm vững các kiến thức tiếng Anh căn bản trong công việc và du lịch.',
        N'Cambridge Assessment English - B1 Preliminary Specifications',
        SYSDATETIME(), 'HoatDong'
    );
END
GO

-- 9. CAMBRIDGE B2 FIRST (FCE) BLUEPRINT (82 CÂU / 35 CÂU PAPER 1)
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'CAMBRIDGE_FCE')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'Cambridge B2 First (FCE) Examination Blueprint',
        'CAMBRIDGE_FCE', 'B2', 'ThiThu', 209, 82, 10.0,
        N'Cấu trúc bài thi quốc tế Cambridge B2 First chứng nhận trình độ trung cao cấp, đủ năng lực làm việc trong môi trường nói tiếng Anh độc lập.',
        N'Cambridge Assessment English - B2 First Handbook for Teachers',
        SYSDATETIME(), 'HoatDong'
    );
END
GO

-- 10. CAMBRIDGE C1 ADVANCED (CAE) BLUEPRINT
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'CAMBRIDGE_CAE')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'Cambridge C1 Advanced (CAE) Examination Blueprint',
        'CAMBRIDGE_CAE', 'C1', 'ThiThu', 235, 86, 10.0,
        N'Cấu trúc bài thi chuyên sâu Cambridge C1 Advanced chứng nhận năng lực ngôn ngữ xuất sắc ở bậc đại học và chuyên nghiệp toàn cầu.',
        N'Cambridge Assessment English - C1 Advanced Specifications',
        SYSDATETIME(), 'HoatDong'
    );
END
GO

-- 11. CAMBRIDGE YOUNG LEARNERS (STARTERS, MOVERS, FLYERS) BLUEPRINT
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'STARTERS')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'Cambridge Pre A1 Starters (YLE) Blueprint',
        'STARTERS', 'A1', 'LuyenTap', 45, 45, 10.0,
        N'Khung đánh giá tiếng Anh thiếu nhi khởi đầu Pre A1 Starters: Nhận biết đồ vật, màu sắc, từ vựng gia đình và chỉ dẫn đơn giản.',
        N'Cambridge English Young Learners (YLE) Pre A1 Handbook',
        SYSDATETIME(), 'HoatDong'
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'MOVERS')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'Cambridge A1 Movers (YLE) Blueprint',
        'MOVERS', 'A1', 'LuyenTap', 60, 60, 10.0,
        N'Khung đánh giá tiếng Anh thiếu nhi A1 Movers: Nghe và đọc các mẩu hội thoại ngắn, mô tả người và đồ vật quen thuộc.',
        N'Cambridge English Young Learners (YLE) A1 Handbook',
        SYSDATETIME(), 'HoatDong'
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'FLYERS')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'Cambridge A2 Flyers (YLE) Blueprint',
        'FLYERS', 'A2', 'LuyenTap', 75, 70, 10.0,
        N'Khung đánh giá tiếng Anh thiếu nhi A2 Flyers: Đọc hiểu các câu chuyện ngắn, trả lời câu hỏi và viết câu miêu tả hoàn chỉnh.',
        N'Cambridge English Young Learners (YLE) A2 Handbook',
        SYSDATETIME(), 'HoatDong'
    );
END
GO

-- 12. GÓI LUYỆN TẬP THÍCH ỨNG CEFR (A1 - C1) BLUEPRINT TRÊN EAI (QUICK PRACTICE: 10 CÂU)
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'CEFR_PRACTICE')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'CEFR Adaptive Skills Practice Blueprint',
        'CEFR_PRACTICE', 'MULTI', 'LuyenTap', 20, 10, 10.0,
        N'Cấu trúc gói luyện tập thích ứng linh hoạt theo 6 bậc CEFR (A1-C1): Học viên ôn tập nhanh 10-15 câu theo từng chuyên đề ngữ pháp, từ vựng hoặc đọc hiểu.',
        N'Council of Europe - Common European Framework of Reference for Languages (CEFR)',
        SYSDATETIME(), 'HoatDong'
    );
END
GO

-- 13. ĐỀ THI TỐT NGHIỆP THPT QUỐC GIA MÔN TIẾNG ANH (50 CÂU - 60 PHÚT)
IF NOT EXISTS (SELECT 1 FROM CauTrucDeThi WHERE MaKyThi = 'THPT_QG')
BEGIN
    INSERT INTO CauTrucDeThi (TenCauTruc, MaKyThi, MaCapDo, LoaiDeThi, ThoiGianPhut, TongSoCau, TongDiem, MoTa, NguonThamChieu, NgayXacMinh, TrangThai)
    VALUES (
        N'Đề thi Tốt nghiệp THPT Quốc Gia môn Tiếng Anh Blueprint',
        'THPT_QG', 'B1', 'ThiThu', 60, 50, 10.0,
        N'Cấu trúc đề thi Tốt nghiệp THPT Quốc Gia môn Tiếng Anh chuẩn quy định của Bộ Giáo dục và Đào tạo gồm 50 câu trắc nghiệm khách quan trong thời gian 60 phút.',
        N'Bộ Giáo dục và Đào tạo - Ma trận đề thi Tốt nghiệp THPT môn Tiếng Anh',
        SYSDATETIME(), 'HoatDong'
    );
    DECLARE @IdThpt INT = SCOPE_IDENTITY();
    INSERT INTO ChiTietCauTrucDeThi (MaCauTruc, TenPhan, ThuTuPhan, KyNang, LoaiCauHoiChoPhep, SoLuongCau, DiemMoiCau, ThoiGianPhut, YeuCauDoanVan, YeuCauAudio, MoTa)
    VALUES 
        (@IdThpt, N'Phần 1: Ngữ âm & Trọng âm', 1, 'TuVung', 'TracNghiem', 4, 0.2, 5, 0, 0, N'Phát âm nguyên âm/phụ âm và trọng âm từ 2-3 âm tiết.'),
        (@IdThpt, N'Phần 2: Ngữ pháp & Từ vựng căn bản đến nâng cao', 2, 'NguPhap', 'TracNghiem', 16, 0.2, 18, 0, 0, N'Thì động từ, câu bị động, điều kiện, mệnh đề quan hệ, từ loại và thành ngữ.'),
        (@IdThpt, N'Phần 3: Chức năng giao tiếp & Từ đồng nghĩa - Trái nghĩa', 3, 'TuVung', 'TracNghiem', 6, 0.2, 7, 0, 0, N'Giao tiếp xã hội và tìm từ đồng nghĩa, trái nghĩa theo ngữ cảnh.'),
        (@IdThpt, N'Phần 4: Đọc điền từ & Đọc hiểu 2 đoạn văn', 4, 'Doc', 'TracNghiem', 17, 0.2, 22, 1, 0, N'Đọc điền từ (Cloze test 5 câu) và 2 bài đọc hiểu phân tích chi tiết (12 câu).'),
        (@IdThpt, N'Phần 5: Tìm lỗi sai & Biến đổi câu, Kết hợp câu', 5, 'Viet', 'TracNghiem,SuaLoiSai,VietLaiCau', 7, 0.2, 8, 0, 0, N'Xác định lỗi ngữ pháp và chọn câu đồng nghĩa, câu ghép chuẩn xác.');
END
GO

PRINT N'Hoàn tất nạp thành công các Exam Blueprints chuẩn hóa vào cơ sở dữ liệu!';
GO

-- ====================================================================================
-- CHUẨN HÓA CÁC ĐỀ THI MẪU THEO ĐÚNG CẤU TRÚC EXAM BLUEPRINTS & NGÂN HÀNG CÂU HỎI
-- ====================================================================================

-- 1. CẬP NHẬT THÔNG TIN ĐỀ THI TRONG BẢNG BaiTapDeThi
-- TOÀN BỘ CÁC ĐỀ THI THỬ CHUẨN HÓA (30 - 50 CÂU)
UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Cambridge Pre A1 Starters: Khảo Thí Từ Vựng & Phản Xạ Căn Bản (30 Câu)',
    LoaiHinh = 'ThiThu',
    MaCauTrucDeThi = 11,
    ThoiGianLamBaiPhut = 40,
    TongSoCau = 30,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Bài thi khảo thí năng lực Pre A1 Starters: 30 câu hỏi nhận diện từ vựng hình ảnh, màu sắc, chỉ dẫn lớp học và giao tiếp căn bản.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 0;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Cambridge A2 Key (KET): Đọc Hiểu & Ngữ Pháp Quá Khứ, So Sánh (30 Câu)',
    LoaiHinh = 'ThiThu',
    MaCauTrucDeThi = 7,
    ThoiGianLamBaiPhut = 45,
    TongSoCau = 30,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Bài thi mô phỏng định dạng Cambridge A2 Key: 30 câu trắc nghiệm đọc hiểu đoạn văn ngắn, biển báo và ngữ pháp thì quá khứ, cấu trúc so sánh.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 1;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'TOEIC Reading Diagnostic Test: Khảo Thí Đọc Hiểu Thương Mại (40 Câu)',
    LoaiHinh = 'ThiThu',
    MaCauTrucDeThi = 4,
    ThoiGianLamBaiPhut = 45,
    TongSoCau = 40,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Đề thi khảo thí định dạng TOEIC Reading: 40 câu hỏi hoàn thành câu và đọc hiểu văn bản thương mại, tuyển dụng, báo cáo tài chính.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 2;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'IELTS Academic Reading Examination: Standard Mock Test #05 (40 Câu)',
    LoaiHinh = 'ThiThu',
    MaCauTrucDeThi = 2,
    ThoiGianLamBaiPhut = 60,
    TongSoCau = 40,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Đề thi chuẩn hóa IELTS Academic Reading gồm 40 câu hỏi trải đều trên 3 bài đọc chuyên sâu: Giáo dục, Công nghệ và Biến đổi khí hậu.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 3;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Cambridge B2 First (FCE): Đột Phá Ngữ Pháp & Đọc Hiểu Chuyên Sâu (35 Câu)',
    LoaiHinh = 'ThiThu',
    MaCauTrucDeThi = 9,
    ThoiGianLamBaiPhut = 60,
    TongSoCau = 35,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Đề thi chuẩn Cambridge B2 First (FCE) Paper 1: 35 câu kiểm tra biến đổi từ loại, điền khuyết đoạn văn và đọc hiểu phân tích luận điểm.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 4;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'IELTS Advanced Academic Mock Test: Phân Tích Luận Điểm & Tư Duy Phản Biện (40 Câu)',
    LoaiHinh = 'ThiThu',
    MaCauTrucDeThi = 2,
    ThoiGianLamBaiPhut = 60,
    TongSoCau = 40,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Bài thi mô phỏng IELTS Academic nâng cao: 40 câu hỏi phân tích các bài nghiên cứu y sinh, công nghệ thông tin và xã hội học.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 5;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'IELTS Academic Reading Examination: Standard 40 Questions Mock Test',
    LoaiHinh = 'ThiThu',
    MaCauTrucDeThi = 2,
    ThoiGianLamBaiPhut = 60,
    TongSoCau = 40,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Đề thi mô phỏng định dạng IELTS Academic Reading chuẩn quốc tế: 40 câu hỏi phân bổ qua 3 bài đọc học thuật về AI, Y học và Khoa học môi trường.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 8;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'TOEIC Reading Test Part 5 & 6: 40 Câu Chuẩn Hóa Doanh Nghiệp',
    LoaiHinh = 'ThiThu',
    MaCauTrucDeThi = 4,
    ThoiGianLamBaiPhut = 45,
    TongSoCau = 40,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Đề thi TOEIC Reading chuẩn hóa: 40 câu hoàn thành câu công sở (Part 5) và đọc điền đoạn văn thương mại quốc tế (Part 6).',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 7;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'VSTEP B2 Chuẩn Hóa: Khảo Thí Đọc Hiểu & Ngôn Ngữ Học Thuật (40 Câu)',
    LoaiHinh = 'ThiThu',
    MaCauTrucDeThi = 6,
    ThoiGianLamBaiPhut = 60,
    TongSoCau = 40,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Đề thi VSTEP Bậc 4 (B2) chuẩn hóa: 40 câu trắc nghiệm chuyên sâu ngữ pháp phức hợp, cấu trúc đảo ngữ và 4 bài đọc hiểu học thuật tổng hợp.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 9;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Cambridge B2 First (FCE): Use of English & Reading Comprehension (35 Câu)',
    LoaiHinh = 'ThiThu',
    MaCauTrucDeThi = 9,
    ThoiGianLamBaiPhut = 60,
    TongSoCau = 35,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Đề thi chuẩn Cambridge B2 First (FCE): 35 câu Use of English và Đọc hiểu phân tích luận điểm học thuật.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 21;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Cambridge A2 Key (KET): Giao Tiếp & Đọc Hiểu Nền Tảng (30 Câu)',
    LoaiHinh = 'ThiThu',
    MaCauTrucDeThi = 7,
    ThoiGianLamBaiPhut = 45,
    TongSoCau = 30,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Đề khảo thí chuẩn Cambridge A2 Key: 30 câu kiểm tra kỹ năng đọc biển báo, mẩu tin ngắn, giao tiếp thường nhật và ngữ pháp căn bản.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 18;

-- CÁC GÓI LUYỆN TẬP CHUYÊN ĐỀ (10 CÂU / GÓI - QUICK DRILLS)
UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Gói Luyện Tập: Chuyên Đề Ngữ Pháp Phức Hợp & Mệnh Đề Quan Hệ (B2)',
    LoaiHinh = 'LuyenTap',
    MaCauTrucDeThi = 14,
    ThoiGianLamBaiPhut = 20,
    TongSoCau = 10,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Luyện tập chuyên sâu ngữ pháp bậc B2: Mệnh đề quan hệ rút gọn, liên từ đẳng lập và thì hoàn thành.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 20;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Gói Luyện Tập: Từ Vựng & Thuật Ngữ Học Thuật Nâng Cao (B2)',
    LoaiHinh = 'LuyenTap',
    MaCauTrucDeThi = 14,
    ThoiGianLamBaiPhut = 20,
    TongSoCau = 10,
    DiemDat = 5.0,
    TongDiem = 10.0,
    MoTa = N'Rèn luyện vốn từ vựng học thuật B2 thường gặp trong các kỳ thi quốc tế và giao tiếp chuyên nghiệp.',
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 19;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Gói Luyện Tập: Chuyên Đề Câu Điều Kiện & Đảo Ngữ Nâng Cao',
    LoaiHinh = 'LuyenTap',
    MaCauTrucDeThi = 14,
    ThoiGianLamBaiPhut = 20,
    TongSoCau = 10,
    DiemDat = 5.0,
    TongDiem = 10.0,
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 10;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Gói Luyện Tập: 100 Từ Vựng & Collocations Oxford Cốt Lõi',
    LoaiHinh = 'LuyenTap',
    MaCauTrucDeThi = 14,
    ThoiGianLamBaiPhut = 15,
    TongSoCau = 10,
    DiemDat = 5.0,
    TongDiem = 10.0,
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 11;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Gói Luyện Tập: Đọc Hiểu Chuyên Sâu Trí Tuệ Nhân Tạo & Y Học',
    LoaiHinh = 'LuyenTap',
    MaCauTrucDeThi = 14,
    ThoiGianLamBaiPhut = 25,
    TongSoCau = 10,
    DiemDat = 5.0,
    TongDiem = 10.0,
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 12;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Gói Luyện Tập: Cấu Trúc Viết & Phát Hiện Lỗi Sai Học Thuật',
    LoaiHinh = 'LuyenTap',
    MaCauTrucDeThi = 14,
    ThoiGianLamBaiPhut = 20,
    TongSoCau = 10,
    DiemDat = 5.0,
    TongDiem = 10.0,
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 13;

UPDATE BaiTapDeThi
SET TenBaiTapDeThi = N'Gói Luyện Tập: Thành Ngữ & Cụm Từ Học Thuật Band C1 (Advanced Idioms)',
    LoaiHinh = 'LuyenTap',
    MaCauTrucDeThi = 14,
    ThoiGianLamBaiPhut = 20,
    TongSoCau = 10,
    DiemDat = 5.0,
    TongDiem = 10.0,
    DaPhatHanh = 1,
    TrangThai = N'DaXuatBan'
WHERE MaBaiTapDeThi = 15;
GO

-- 2. NẠP SNAPSHOT CÂU HỎI TỪ BỘ 500 CÂU VÀO CauHoiBaiTap (CHUẨN HÓA SỐ CÂU THỰC TẾ)
IF EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_001')
BEGIN
    -- IELTS Academic Reading Test (40 CÂU: SEED 251-290)
    DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 8;
    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot, AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
    SELECT 8, ch.MaCauHoi, ROW_NUMBER() OVER (ORDER BY ch.MaDinhDanhSeed ASC), 0.25, ch.NoiDungCauHoi, ch.DoanVanDoc, ISNULL(ch.GiaiThichDapAn, ch.LoiGiaiChiTiet), ch.DuongDanAudio, ch.LoaiCauHoi, ch.KyNang, ch.DoKho,
           (SELECT opt.MaLuaChon, opt.KyHieuLuaChon, opt.NoiDungLuaChon, opt.LaDapAnDung FROM LuaChonDapAn opt WHERE opt.MaCauHoi = ch.MaCauHoi FOR JSON PATH)
    FROM CauHoi ch WHERE ch.MaDinhDanhSeed BETWEEN 'SEED_EAI_251' AND 'SEED_EAI_290';

    -- TOEIC Part 5 & 6 Test (40 CÂU: SEED 101-140)
    DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 7;
    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot, AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
    SELECT 7, ch.MaCauHoi, ROW_NUMBER() OVER (ORDER BY ch.MaDinhDanhSeed ASC), 0.25, ch.NoiDungCauHoi, ch.DoanVanDoc, ISNULL(ch.GiaiThichDapAn, ch.LoiGiaiChiTiet), ch.DuongDanAudio, ch.LoaiCauHoi, ch.KyNang, ch.DoKho,
           (SELECT opt.MaLuaChon, opt.KyHieuLuaChon, opt.NoiDungLuaChon, opt.LaDapAnDung FROM LuaChonDapAn opt WHERE opt.MaCauHoi = ch.MaCauHoi FOR JSON PATH)
    FROM CauHoi ch WHERE ch.MaDinhDanhSeed BETWEEN 'SEED_EAI_101' AND 'SEED_EAI_140';

    -- VSTEP B2 Test (40 CÂU: SEED 151-190)
    DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 9;
    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot, AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
    SELECT 9, ch.MaCauHoi, ROW_NUMBER() OVER (ORDER BY ch.MaDinhDanhSeed ASC), 0.25, ch.NoiDungCauHoi, ch.DoanVanDoc, ISNULL(ch.GiaiThichDapAn, ch.LoiGiaiChiTiet), ch.DuongDanAudio, ch.LoaiCauHoi, ch.KyNang, ch.DoKho,
           (SELECT opt.MaLuaChon, opt.KyHieuLuaChon, opt.NoiDungLuaChon, opt.LaDapAnDung FROM LuaChonDapAn opt WHERE opt.MaCauHoi = ch.MaCauHoi FOR JSON PATH)
    FROM CauHoi ch WHERE ch.MaDinhDanhSeed BETWEEN 'SEED_EAI_151' AND 'SEED_EAI_190';

    -- Cambridge B2 First (FCE) (35 CÂU: SEED 201-235)
    DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 21;
    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot, AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
    SELECT 21, ch.MaCauHoi, ROW_NUMBER() OVER (ORDER BY ch.MaDinhDanhSeed ASC), ROUND(10.0 / 35.0, 3), ch.NoiDungCauHoi, ch.DoanVanDoc, ISNULL(ch.GiaiThichDapAn, ch.LoiGiaiChiTiet), ch.DuongDanAudio, ch.LoaiCauHoi, ch.KyNang, ch.DoKho,
           (SELECT opt.MaLuaChon, opt.KyHieuLuaChon, opt.NoiDungLuaChon, opt.LaDapAnDung FROM LuaChonDapAn opt WHERE opt.MaCauHoi = ch.MaCauHoi FOR JSON PATH)
    FROM CauHoi ch WHERE ch.MaDinhDanhSeed BETWEEN 'SEED_EAI_201' AND 'SEED_EAI_235';

    -- Cambridge A2 Key (KET) (30 CÂU: SEED 001-030)
    DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 18;
    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot, AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
    SELECT 18, ch.MaCauHoi, ROW_NUMBER() OVER (ORDER BY ch.MaDinhDanhSeed ASC), ROUND(10.0 / 30.0, 3), ch.NoiDungCauHoi, ch.DoanVanDoc, ISNULL(ch.GiaiThichDapAn, ch.LoiGiaiChiTiet), ch.DuongDanAudio, ch.LoaiCauHoi, ch.KyNang, ch.DoKho,
           (SELECT opt.MaLuaChon, opt.KyHieuLuaChon, opt.NoiDungLuaChon, opt.LaDapAnDung FROM LuaChonDapAn opt WHERE opt.MaCauHoi = ch.MaCauHoi FOR JSON PATH)
    FROM CauHoi ch WHERE ch.MaDinhDanhSeed BETWEEN 'SEED_EAI_001' AND 'SEED_EAI_030';

    -- Cambridge Pre A1 Starters (30 CÂU: SEED 031-060)
    DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 0;
    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot, AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
    SELECT 0, ch.MaCauHoi, ROW_NUMBER() OVER (ORDER BY ch.MaDinhDanhSeed ASC), ROUND(10.0 / 30.0, 3), ch.NoiDungCauHoi, ch.DoanVanDoc, ISNULL(ch.GiaiThichDapAn, ch.LoiGiaiChiTiet), ch.DuongDanAudio, ch.LoaiCauHoi, ch.KyNang, ch.DoKho,
           (SELECT opt.MaLuaChon, opt.KyHieuLuaChon, opt.NoiDungLuaChon, opt.LaDapAnDung FROM LuaChonDapAn opt WHERE opt.MaCauHoi = ch.MaCauHoi FOR JSON PATH)
    FROM CauHoi ch WHERE ch.MaDinhDanhSeed BETWEEN 'SEED_EAI_031' AND 'SEED_EAI_060';

    -- Cambridge A2 Key KET #2 (30 CÂU: SEED 061-090)
    DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 1;
    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot, AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
    SELECT 1, ch.MaCauHoi, ROW_NUMBER() OVER (ORDER BY ch.MaDinhDanhSeed ASC), ROUND(10.0 / 30.0, 3), ch.NoiDungCauHoi, ch.DoanVanDoc, ISNULL(ch.GiaiThichDapAn, ch.LoiGiaiChiTiet), ch.DuongDanAudio, ch.LoaiCauHoi, ch.KyNang, ch.DoKho,
           (SELECT opt.MaLuaChon, opt.KyHieuLuaChon, opt.NoiDungLuaChon, opt.LaDapAnDung FROM LuaChonDapAn opt WHERE opt.MaCauHoi = ch.MaCauHoi FOR JSON PATH)
    FROM CauHoi ch WHERE ch.MaDinhDanhSeed BETWEEN 'SEED_EAI_061' AND 'SEED_EAI_090';

    -- TOEIC Reading Diagnostic Test (40 CÂU: SEED 091-130)
    DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 2;
    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot, AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
    SELECT 2, ch.MaCauHoi, ROW_NUMBER() OVER (ORDER BY ch.MaDinhDanhSeed ASC), 0.25, ch.NoiDungCauHoi, ch.DoanVanDoc, ISNULL(ch.GiaiThichDapAn, ch.LoiGiaiChiTiet), ch.DuongDanAudio, ch.LoaiCauHoi, ch.KyNang, ch.DoKho,
           (SELECT opt.MaLuaChon, opt.KyHieuLuaChon, opt.NoiDungLuaChon, opt.LaDapAnDung FROM LuaChonDapAn opt WHERE opt.MaCauHoi = ch.MaCauHoi FOR JSON PATH)
    FROM CauHoi ch WHERE ch.MaDinhDanhSeed BETWEEN 'SEED_EAI_091' AND 'SEED_EAI_130';

    -- IELTS Academic Reading Mock Test #05 (40 CÂU: SEED 291-330)
    DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 3;
    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot, AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
    SELECT 3, ch.MaCauHoi, ROW_NUMBER() OVER (ORDER BY ch.MaDinhDanhSeed ASC), 0.25, ch.NoiDungCauHoi, ch.DoanVanDoc, ISNULL(ch.GiaiThichDapAn, ch.LoiGiaiChiTiet), ch.DuongDanAudio, ch.LoaiCauHoi, ch.KyNang, ch.DoKho,
           (SELECT opt.MaLuaChon, opt.KyHieuLuaChon, opt.NoiDungLuaChon, opt.LaDapAnDung FROM LuaChonDapAn opt WHERE opt.MaCauHoi = ch.MaCauHoi FOR JSON PATH)
    FROM CauHoi ch WHERE ch.MaDinhDanhSeed BETWEEN 'SEED_EAI_291' AND 'SEED_EAI_330';

    -- Cambridge B2 First FCE #2 (35 CÂU: SEED 216-250)
    DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 4;
    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot, AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
    SELECT 4, ch.MaCauHoi, ROW_NUMBER() OVER (ORDER BY ch.MaDinhDanhSeed ASC), ROUND(10.0 / 35.0, 3), ch.NoiDungCauHoi, ch.DoanVanDoc, ISNULL(ch.GiaiThichDapAn, ch.LoiGiaiChiTiet), ch.DuongDanAudio, ch.LoaiCauHoi, ch.KyNang, ch.DoKho,
           (SELECT opt.MaLuaChon, opt.KyHieuLuaChon, opt.NoiDungLuaChon, opt.LaDapAnDung FROM LuaChonDapAn opt WHERE opt.MaCauHoi = ch.MaCauHoi FOR JSON PATH)
    FROM CauHoi ch WHERE ch.MaDinhDanhSeed BETWEEN 'SEED_EAI_216' AND 'SEED_EAI_250';

    -- IELTS Advanced Academic Mock Test (40 CÂU: SEED 401-440)
    DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 5;
    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot, AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
    SELECT 5, ch.MaCauHoi, ROW_NUMBER() OVER (ORDER BY ch.MaDinhDanhSeed ASC), 0.25, ch.NoiDungCauHoi, ch.DoanVanDoc, ISNULL(ch.GiaiThichDapAn, ch.LoiGiaiChiTiet), ch.DuongDanAudio, ch.LoaiCauHoi, ch.KyNang, ch.DoKho,
           (SELECT opt.MaLuaChon, opt.KyHieuLuaChon, opt.NoiDungLuaChon, opt.LaDapAnDung FROM LuaChonDapAn opt WHERE opt.MaCauHoi = ch.MaCauHoi FOR JSON PATH)
    FROM CauHoi ch WHERE ch.MaDinhDanhSeed BETWEEN 'SEED_EAI_401' AND 'SEED_EAI_440';
END
GO

PRINT N'Hoàn tất đồng bộ toàn diện đề thi và ngân hàng câu hỏi chuẩn hóa!';
GO
