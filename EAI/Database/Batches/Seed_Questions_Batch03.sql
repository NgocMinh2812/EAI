-- ====================================================================================
-- SEED QUESTIONS BATCH: Batch 03 (SEED_EAI_101 -> SEED_EAI_150)
-- Số lượng câu hỏi: 50
-- Đảm bảo: Idempotent (Chạy lại nhiều lần không trùng lặp)
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Đang nạp Batch 03 (SEED_EAI_101 -> SEED_EAI_150) (50 câu hỏi)...';

-- Câu hỏi SEED_EAI_101 (Viet - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_101')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 3, N'TracNghiem', N'Viet', N'TrungBinh', N'When writing an email to announce a product delay to business clients, what is the best opening sentence?',
            NULL, NULL, N'''We are writing to inform you that...'' là mẫu câu mở đầu chuẩn mực, chuyên nghiệp và lịch thiệp trong thư tín thương mại.', N'''We are writing to inform you that...'' là mẫu câu mở đầu chuẩn mực, chuyên nghiệp và lịch thiệp trong thư tín thương mại.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Phần 1: Mẫu câu mở đầu email thương mại: ''I am writing to inform / confirm / request...''', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_101');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I am writing to inform you of an unexpected delay regarding your recent order.', 1),
        (@MaCH, N'B', N'Hey clients, the products are late, deal with it.', 0),
        (@MaCH, N'C', N'Products are slow because of reasons.', 0),
        (@MaCH, N'D', N'I tell you today that nothing is delivered.', 0);
END
GO

-- Câu hỏi SEED_EAI_102 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_102')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'What does the abbreviation ''FYI'' mean in corporate email communication?',
            NULL, NULL, N'''FYI'' là viết tắt của ''For Your Information'' (Để bạn nắm thông tin, không yêu cầu hành động bắt buộc).', N'''FYI'' là viết tắt của ''For Your Information'' (Để bạn nắm thông tin, không yêu cầu hành động bắt buộc).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Viết tắt thông dụng trong văn phòng: FYI (For Your Information), ASAP (As Soon As Possible), EOD (End of Day).', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_102');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'For Your Inspection', 0),
        (@MaCH, N'B', N'For Your Information', 1),
        (@MaCH, N'C', N'Follow Your Instincts', 0),
        (@MaCH, N'D', N'Find Your Invoice', 0);
END
GO

-- Câu hỏi SEED_EAI_103 (Viet - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_103')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 3, N'DienKhuyet', N'Viet', N'TrungBinh', N'Complete the professional email closing: ''Please do not _______ to contact me if you require any further assistance.''',
            NULL, NULL, N'Thành ngữ thư tín kinh điển: ''do not hesitate to contact me'' (đừng ngần ngại liên hệ với tôi).', N'Thành ngữ thư tín kinh điển: ''do not hesitate to contact me'' (đừng ngần ngại liên hệ với tôi).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Lời kết thúc email chuyên nghiệp: ''Please do not hesitate to contact me...''', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_103');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'hesitate', 1),
        (@MaCH, N'B', N'delay', 0),
        (@MaCH, N'C', N'refuse', 0),
        (@MaCH, N'D', N'doubt', 0);
END
GO

-- Câu hỏi SEED_EAI_104 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_104')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 3, N'TracNghiem', N'Doc', N'TrungBinh', N'Which phrase is used to introduce an attached file in a formal email?',
            NULL, NULL, N'''Please find attached the quarterly financial report'' là cấu trúc quy chuẩn báo tệp đính kèm.', N'''Please find attached the quarterly financial report'' là cấu trúc quy chuẩn báo tệp đính kèm.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Đính kèm tài liệu: ''Please find attached / I have attached...''', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_104');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Look down for files.', 0),
        (@MaCH, N'B', N'Please find attached the quarterly financial report.', 1),
        (@MaCH, N'C', N'Files are glued below.', 0),
        (@MaCH, N'D', N'Here are random documents.', 0);
END
GO

-- Câu hỏi SEED_EAI_105 (NguPhap - Trình độ 3 - Dạng SuaLoiSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_105')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 3, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the mistake: ''I look forward to hear from you soon regarding this proposal.''',
            NULL, NULL, N'Cấu trúc ''look forward to'' đi kèm với V-ing hoặc danh từ: ''look forward to hearing from you''.', N'Cấu trúc ''look forward to'' đi kèm với V-ing hoặc danh từ: ''look forward to hearing from you''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Cấu trúc câu kết thư: Look forward to + V-ing.', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_105');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''look'' should be ''looking''', 0),
        (@MaCH, N'B', N'''hear'' should be ''hearing''', 1),
        (@MaCH, N'C', N'''from'' should be ''to''', 0),
        (@MaCH, N'D', N'''proposal'' should be ''proposed''', 0);
END
GO

-- Câu hỏi SEED_EAI_106 (Viet - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_106')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 4, N'TracNghiem', N'Viet', N'TrungBinh', N'When proposing a rescheduled meeting time to a client, which expression is most professional?',
            NULL, NULL, N'''Would 10:00 AM this Thursday be convenient for you?'' là cách hỏi lịch sự và chuyên nghiệp nhất.', N'''Would 10:00 AM this Thursday be convenient for you?'' là cách hỏi lịch sự và chuyên nghiệp nhất.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 22 - Đặt lịch hẹn và dời lịch: ''Would [time] suit you? / Would [time] be convenient?''', N'Extracted', N'BaiHoc_22_AppointmentAndComplaints.txt', 22, 'SEED_EAI_106');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Come to my office at 10 AM Thursday or else.', 0),
        (@MaCH, N'B', N'Would 10:00 AM this Thursday be convenient for you?', 1),
        (@MaCH, N'C', N'Thursday 10 AM is the only time I have.', 0),
        (@MaCH, N'D', N'I force our meeting to be on Thursday.', 0);
END
GO

-- Câu hỏi SEED_EAI_107 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_107')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 4, N'TracNghiem', N'Doc', N'Kho', N'What is the primary objective of the opening paragraph in a customer complaint response email?',
            NULL, NULL, N'Khi trả lời khiếu nại của khách hàng, việc đầu tiên là ghi nhận vấn đề, đồng cảm và gửi lời xin lỗi chân thành vì sự bất tiện.', N'Khi trả lời khiếu nại của khách hàng, việc đầu tiên là ghi nhận vấn đề, đồng cảm và gửi lời xin lỗi chân thành vì sự bất tiện.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 22 - Khung giải quyết khiếu nại (Acknowledge & Apologize): Thừa nhận sự cố và xin lỗi vì trải nghiệm bất tiện.', N'Extracted', N'BaiHoc_22_AppointmentAndComplaints.txt', 22, 'SEED_EAI_107');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'To blame the third-party logistics company.', 0),
        (@MaCH, N'B', N'To acknowledge the issue, apologize sincerely, and validate the customer''s frustration.', 1),
        (@MaCH, N'C', N'To demand that the customer pay extra fees.', 0),
        (@MaCH, N'D', N'To ignore the complaint entirely.', 0);
END
GO

-- Câu hỏi SEED_EAI_108 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_108')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Complete the complaint resolution phrase: ''To _______ for the inconvenience caused, we would like to offer you a 25% discount voucher.''',
            NULL, NULL, N'''Compensate for'' có nghĩa là bồi thường hoặc bù đắp cho sự bất tiện.', N'''Compensate for'' có nghĩa là bồi thường hoặc bù đắp cho sự bất tiện.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 22 - Đưa ra giải pháp bồi thường: ''To compensate for the inconvenience...''', N'Extracted', N'BaiHoc_22_AppointmentAndComplaints.txt', 22, 'SEED_EAI_108');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'compensate', 1),
        (@MaCH, N'B', N'punish', 0),
        (@MaCH, N'C', N'charge', 0),
        (@MaCH, N'D', N'increase', 0);
END
GO

-- Câu hỏi SEED_EAI_109 (Viet - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_109')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 4, N'VietLaiCau', N'Viet', N'TrungBinh', N'Rewrite more diplomatically: ''You sent the wrong invoice.''',
            NULL, NULL, N'Cách diễn đạt khéo léo trong kinh doanh: ''There seems to be a discrepancy in the invoice received.'' (Có vẻ như có sự sai lệch nhỏ trong hóa đơn).', N'Cách diễn đạt khéo léo trong kinh doanh: ''There seems to be a discrepancy in the invoice received.'' (Có vẻ như có sự sai lệch nhỏ trong hóa đơn).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 22 - Ngoại giao thương mại (Diplomatic language in emails).', N'Applied', N'BaiHoc_22_AppointmentAndComplaints.txt', 22, 'SEED_EAI_109');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'There appears to be a slight discrepancy in the invoice we received.', 1),
        (@MaCH, N'B', N'Your accounting team made a terrible mistake.', 0),
        (@MaCH, N'C', N'I demand you learn how to make invoices.', 0),
        (@MaCH, N'D', N'This invoice is totally fake.', 0);
END
GO

-- Câu hỏi SEED_EAI_110 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_110')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 4, N'DungSai', N'Doc', N'De', N'True or False: In business emails, using ALL CAPITAL LETTERS in a sentence is interpreted as aggressive and unprofessional (shouting).',
            NULL, NULL, N'Đúng. Viết hoa toàn bộ chữ cái (ALL CAPS) trong thư điện tử được coi như đang la hét, thô lỗ.', N'Đúng. Viết hoa toàn bộ chữ cái (ALL CAPS) trong thư điện tử được coi như đang la hét, thô lỗ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 22 - Văn hóa email doanh nghiệp (Netiquette): Tránh viết hoa toàn bộ từ ngữ.', N'Extracted', N'BaiHoc_22_AppointmentAndComplaints.txt', 22, 'SEED_EAI_110');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_111 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_111')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'TuVung', N'Kho', N'In contract negotiations, what does the term ''concession'' mean?',
            NULL, NULL, N'''Concession'' là sự nhượng bộ (chấp nhận từ bỏ hoặc nới lỏng một yêu cầu để đạt được thỏa thuận chung).', N'''Concession'' là sự nhượng bộ (chấp nhận từ bỏ hoặc nới lỏng một yêu cầu để đạt được thỏa thuận chung).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Thuật ngữ đàm phán hợp đồng: Concession (sự nhượng bộ lẫn nhau để đạt Win-Win).', N'Extracted', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_111');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A complete cancellation of business ties.', 0),
        (@MaCH, N'B', N'Something granted or yielded, especially in response to demands during negotiations.', 1),
        (@MaCH, N'C', N'A penalty fine for violating delivery terms.', 0),
        (@MaCH, N'D', N'An upfront payment before delivery.', 0);
END
GO

-- Câu hỏi SEED_EAI_112 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_112')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Which conditional structure expresses a flexible negotiation stance: ''If you _______ the order volume to 1,000 units, we _______ offer a 10% discount.''',
            NULL, NULL, N'Đàm phán thương mại thường dùng câu điều kiện loại 1: ''If you increase... we will / can offer...''.', N'Đàm phán thương mại thường dùng câu điều kiện loại 1: ''If you increase... we will / can offer...''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Mẫu câu điều kiện trao đổi quyền lợi: If you [concession], we will [benefit].', N'Extracted', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_112');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'increased / would have offered', 0),
        (@MaCH, N'B', N'increase / can offer', 1),
        (@MaCH, N'C', N'had increased / will offer', 0),
        (@MaCH, N'D', N'increasing / offered', 0);
END
GO

-- Câu hỏi SEED_EAI_113 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_113')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank with an international trade term: ''The parties agreed on FOB (Free _______ Board) shipping terms.''',
            NULL, NULL, N'FOB là Incoterm viết tắt của ''Free On Board'' (Giao hàng lên tàu).', N'FOB là Incoterm viết tắt của ''Free On Board'' (Giao hàng lên tàu).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Điều khoản thương mại quốc tế Incoterms: FOB (Free On Board), CIF (Cost, Insurance and Freight).', N'Extracted', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_113');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'On', 1),
        (@MaCH, N'B', N'In', 0),
        (@MaCH, N'C', N'At', 0),
        (@MaCH, N'D', N'By', 0);
END
GO

-- Câu hỏi SEED_EAI_114 (TuVung - Trình độ 4 - Dạng NoiCap - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_114')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'NoiCap', N'TuVung', N'TrungBinh', N'Match each negotiation term with its definition:
1. ''Deadlock / Impasse''
2. ''Win-Win outcome''
3. ''Warranty period''',
            NULL, NULL, N'1 là bế tắc đàm phán; 2 là kết quả hai bên cùng có lợi; 3 là thời hạn bảo hành sản phẩm.', N'1 là bế tắc đàm phán; 2 là kết quả hai bên cùng có lợi; 3 là thời hạn bảo hành sản phẩm.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Bảng thuật ngữ chuyên sâu về hợp đồng thương mại.', N'Extracted', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_114');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'1-A state where no progress is possible, 2-Both parties benefit mutually, 3-Duration of product guarantee', 1),
        (@MaCH, N'B', N'1-Payment, 2-Loss, 3-Delivery', 0),
        (@MaCH, N'C', N'1-Meeting, 2-Contract, 3-Signature', 0),
        (@MaCH, N'D', N'1-Profit, 2-Debt, 3-Tax', 0);
END
GO

-- Câu hỏi SEED_EAI_115 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_115')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What does a ''force majeure'' clause in a commercial contract protect companies against?',
            NULL, NULL, N'''Force majeure'' (Bất khả kháng) bảo vệ các bên khỏi trách nhiệm pháp lý khi xảy ra thiên tai, chiến tranh hoặc thảm họa ngoài tầm kiểm soát.', N'''Force majeure'' (Bất khả kháng) bảo vệ các bên khỏi trách nhiệm pháp lý khi xảy ra thiên tai, chiến tranh hoặc thảm họa ngoài tầm kiểm soát.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Điều khoản bất khả kháng (Force Majeure clause).', N'Extracted', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_115');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Minor employee tardiness', 0),
        (@MaCH, N'B', N'Unforeseeable extraordinary events like natural disasters or war that prevent fulfillment of obligations', 1),
        (@MaCH, N'C', N'Routine fluctuations in stock prices', 0),
        (@MaCH, N'D', N'Changes in executive management', 0);
END
GO

-- Câu hỏi SEED_EAI_116 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_116')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'Kho', N'In a startup pitch to venture capitalists, what does the ''Value Proposition'' articulate?',
            NULL, NULL, N'''Value Proposition'' (Đề xuất giá trị) nêu rõ lợi ích cốt lõi độc đáo mà giải pháp của startup mang lại để giải quyết nỗi đau của khách hàng.', N'''Value Proposition'' (Đề xuất giá trị) nêu rõ lợi ích cốt lõi độc đáo mà giải pháp của startup mang lại để giải quyết nỗi đau của khách hàng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 24 - Cấu trúc bài thuyết trình Pitching: Problem, Solution, Value Proposition, Market Size, Traction.', N'Extracted', N'BaiHoc_24_ProjectPitching.txt', 24, 'SEED_EAI_116');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The list of founder office amenities', 0),
        (@MaCH, N'B', N'The unique, measurable value and benefits a solution delivers to solve customer pain points', 1),
        (@MaCH, N'C', N'The physical weight of the product packaging', 0),
        (@MaCH, N'D', N'The personal hobbies of the coding team', 0);
END
GO

-- Câu hỏi SEED_EAI_117 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_117')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'TuVung', N'Kho', N'What startup metric does ''TAM'' stand for during an investment pitch?',
            NULL, NULL, N'''TAM'' là viết tắt của ''Total Addressable Market'' (Tổng dung lượng thị trường tiềm năng).', N'''TAM'' là viết tắt của ''Total Addressable Market'' (Tổng dung lượng thị trường tiềm năng).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 24 - Thuật ngữ quy mô thị trường: TAM (Total Addressable Market), SAM, SOM.', N'Extracted', N'BaiHoc_24_ProjectPitching.txt', 24, 'SEED_EAI_117');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Total Addressable Market', 1),
        (@MaCH, N'B', N'Targeted Annual Margin', 0),
        (@MaCH, N'C', N'Technology Assessment Model', 0),
        (@MaCH, N'D', N'Technical Asset Management', 0);
END
GO

-- Câu hỏi SEED_EAI_118 (Noi - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_118')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'DienKhuyet', N'Noi', N'Kho', N'Complete the transition phrase in an investment pitch: ''Now that we have covered the market size, let''s _______ our attention to our traction and revenue growth.''',
            NULL, NULL, N'''Turn our attention to'' là cụm chuyển ý chuyên nghiệp kinh điển khi thuyết trình.', N'''Turn our attention to'' là cụm chuyển ý chuyên nghiệp kinh điển khi thuyết trình.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 24 - Kỹ năng kết nối chuyển ý (Signposting language): ''Let us turn our attention to...''', N'Extracted', N'BaiHoc_24_ProjectPitching.txt', 24, 'SEED_EAI_118');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'turn', 1),
        (@MaCH, N'B', N'throw', 0),
        (@MaCH, N'C', N'kick', 0),
        (@MaCH, N'D', N'jump', 0);
END
GO

-- Câu hỏi SEED_EAI_119 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_119')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Doc', N'Kho', N'What is the function of the ''Hook'' in the opening 30 seconds of an executive pitch?',
            NULL, NULL, N'Cú ''Hook'' mở đầu có vai trò thu hút sự chú ý tức thì của các nhà đầu tư thông qua số liệu ấn tượng hoặc câu hỏi gợi mở.', N'Cú ''Hook'' mở đầu có vai trò thu hút sự chú ý tức thì của các nhà đầu tư thông qua số liệu ấn tượng hoặc câu hỏi gợi mở.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 24 - Nghệ thuật Hook khán giả trong 30 giây đầu.', N'Extracted', N'BaiHoc_24_ProjectPitching.txt', 24, 'SEED_EAI_119');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'To bore the audience with legal definitions', 0),
        (@MaCH, N'B', N'To seize audience attention immediately via compelling statistics, stories, or provocative questions', 1),
        (@MaCH, N'C', N'To distribute handouts silently', 0),
        (@MaCH, N'D', N'To apologize for technical flaws', 0);
END
GO

-- Câu hỏi SEED_EAI_120 (Noi - Trình độ 5 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_120')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'DungSai', N'Noi', N'TrungBinh', N'True or False: During a pitch, founders should read word-for-word from text-heavy presentation slides.',
            NULL, NULL, N'Sai. Người thuyết trình tuyệt đối không đọc nguyên văn từng chữ trên slide; slide chỉ nên chứa từ khóa, hình ảnh và số liệu trực quan.', N'Sai. Người thuyết trình tuyệt đối không đọc nguyên văn từng chữ trên slide; slide chỉ nên chứa từ khóa, hình ảnh và số liệu trực quan.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 24 - Quy tắc vàng thiết kế Slide thuyết trình: ''Slides are visual aids, not teleprompters.''', N'Extracted', N'BaiHoc_24_ProjectPitching.txt', 24, 'SEED_EAI_120');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_121 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_121')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'In the salary negotiation guide, when is the recommended timing to discuss exact compensation numbers with a recruiter?',
            NULL, NULL, N'Thời điểm tối ưu để đàm phán lương là sau khi nhận được thư mời nhận việc chính thức (Official Job Offer), khi nhà tuyển dụng đã chọn bạn.', N'Thời điểm tối ưu để đàm phán lương là sau khi nhận được thư mời nhận việc chính thức (Official Job Offer), khi nhà tuyển dụng đã chọn bạn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Thời điểm vàng đàm phán lương: Sau khi nhận Official Job Offer.', N'Extracted', N'03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx', NULL, 'SEED_EAI_121');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'In the first 2 minutes of the initial phone screening', 0),
        (@MaCH, N'B', N'After receiving an official job offer, when your leverage is highest', 1),
        (@MaCH, N'C', N'On the job application form before applying', 0),
        (@MaCH, N'D', N'After working for six months without asking', 0);
END
GO

-- Câu hỏi SEED_EAI_122 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_122')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What does the term ''Total Compensation Package'' include besides base salary?',
            NULL, NULL, N'''Total Compensation'' bao gồm lương cứng cùng các phúc lợi bổ sung như thưởng hiệu suất, bảo hiểm y tế cao cấp, ngày nghỉ phép và cổ phiếu (equity).', N'''Total Compensation'' bao gồm lương cứng cùng các phúc lợi bổ sung như thưởng hiệu suất, bảo hiểm y tế cao cấp, ngày nghỉ phép và cổ phiếu (equity).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Cấu phần gói đãi ngộ: Base salary, performance bonus, stock options, health insurance.', N'Extracted', N'03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx', NULL, 'SEED_EAI_122');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Only cash in hand', 0),
        (@MaCH, N'B', N'Base salary plus bonuses, health insurance, paid leave, equity, and retirement benefits', 1),
        (@MaCH, N'C', N'Office furniture allowance only', 0),
        (@MaCH, N'D', N'Free parking tickets exclusively', 0);
END
GO

-- Câu hỏi SEED_EAI_123 (Viet - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_123')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'VietLaiCau', N'Viet', N'TrungBinh', N'Rewrite the blunt salary request politely: ''Give me $3,000 or I quit.''',
            NULL, NULL, N'Mẫu câu đàm phán dựa trên giá trị thị trường: ''Based on my experience and industry benchmarks, I am targeting a compensation range around $3,000.''', N'Mẫu câu đàm phán dựa trên giá trị thị trường: ''Based on my experience and industry benchmarks, I am targeting a compensation range around $3,000.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Mẫu câu đàm phán khéo léo dựa trên dữ liệu thị trường.', N'Applied', N'03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx', NULL, 'SEED_EAI_123');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Based on industry benchmarks and my proven track record, I am targeting a range around $3,000.', 1),
        (@MaCH, N'B', N'Pay me $3,000 because I like money.', 0),
        (@MaCH, N'C', N'I need $3,000 to buy luxury clothes.', 0),
        (@MaCH, N'D', N'Why is your salary so cheap? Make it $3,000.', 0);
END
GO

-- Câu hỏi SEED_EAI_124 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_124')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''Candidates should research market _______ on Glassdoor or LinkedIn Salary before interview rounds.''',
            NULL, NULL, N'''Market benchmarks'' hoặc ''rates'' có nghĩa là mức chuẩn thị trường.', N'''Market benchmarks'' hoặc ''rates'' có nghĩa là mức chuẩn thị trường.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Bước 1: Khảo sát Market Benchmarks trên Glassdoor/LinkedIn.', N'Extracted', N'03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx', NULL, 'SEED_EAI_124');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'benchmarks', 1),
        (@MaCH, N'B', N'secrets', 0),
        (@MaCH, N'C', N'rumors', 0),
        (@MaCH, N'D', N'jokes', 0);
END
GO

-- Câu hỏi SEED_EAI_125 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_125')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'DungSai', N'Doc', N'De', N'True or False: If an employer cannot meet your desired base salary, you can negotiate other aspects such as flexible working hours, remote days, or annual review cycles.',
            NULL, NULL, N'Đúng. Đàm phán đãi ngộ linh hoạt bao gồm cả chế độ làm việc từ xa (remote work), thời gian linh hoạt và hỗ trợ đào tạo.', N'Đúng. Đàm phán đãi ngộ linh hoạt bao gồm cả chế độ làm việc từ xa (remote work), thời gian linh hoạt và hỗ trợ đào tạo.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Phương án thay thế khi ngân sách lương cố định: Hybrid work, review định kỳ.', N'Extracted', N'03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx', NULL, 'SEED_EAI_125');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_126 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_126')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'What does the abbreviation ''CC'' stand for in email software?',
            NULL, NULL, N'''CC'' là viết tắt của ''Carbon Copy'' (Gửi bản sao để các bên liên quan cùng theo dõi).', N'''CC'' là viết tắt của ''Carbon Copy'' (Gửi bản sao để các bên liên quan cùng theo dõi).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Các trường gửi email: To, CC (Carbon Copy), BCC (Blind Carbon Copy).', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_126');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Computer Code', 0),
        (@MaCH, N'B', N'Carbon Copy', 1),
        (@MaCH, N'C', N'Client Contact', 0),
        (@MaCH, N'D', N'Critical Case', 0);
END
GO

-- Câu hỏi SEED_EAI_127 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_127')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'What is the key privacy advantage of using ''BCC'' (Blind Carbon Copy)?',
            NULL, NULL, N'''BCC'' ẩn danh tính và địa chỉ email của những người nhận trong danh sách, bảo vệ quyền riêng tư.', N'''BCC'' ẩn danh tính và địa chỉ email của những người nhận trong danh sách, bảo vệ quyền riêng tư.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Chức năng trường BCC: Ẩn danh sách email người nhận.', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_127');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'It sends the email twice as fast.', 0),
        (@MaCH, N'B', N'Recipients in the BCC field cannot see each other''s email addresses.', 1),
        (@MaCH, N'C', N'It encrypts attachments with passwords automatically.', 0),
        (@MaCH, N'D', N'It deletes the email after reading.', 0);
END
GO

-- Câu hỏi SEED_EAI_128 (Viet - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_128')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 4, N'VietLaiCau', N'Viet', N'TrungBinh', N'Rewrite the opening sentence to be more formal: ''Thanks a lot for the email you sent yesterday.''',
            NULL, NULL, N'''Thank you for your email of [date].'' là cách diễn đạt trang trọng chuẩn văn phòng.', N'''Thank you for your email of [date].'' là cách diễn đạt trang trọng chuẩn văn phòng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Mẫu câu cảm ơn trang trọng trong email thương mại.', N'Applied', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_128');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Thank you for your email correspondence dated yesterday regarding our partnership.', 1),
        (@MaCH, N'B', N'I loved the mail you dropped yesterday.', 0),
        (@MaCH, N'C', N'Nice mail yesterday, bro.', 0),
        (@MaCH, N'D', N'Your mail was cool yesterday.', 0);
END
GO

-- Câu hỏi SEED_EAI_129 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_129')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What is an ''Out of Office'' (OOO) auto-responder intended to inform senders about?',
            NULL, NULL, N'Thư tự động báo vắng mặt (Out of Office) thông báo thời gian nhân viên đi công tác/nghỉ phép và người phụ trách thay thế.', N'Thư tự động báo vắng mặt (Out of Office) thông báo thời gian nhân viên đi công tác/nghỉ phép và người phụ trách thay thế.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 22 - Soạn thảo Out of Office message chuẩn quốc tế.', N'Extracted', N'BaiHoc_22_AppointmentAndComplaints.txt', 22, 'SEED_EAI_129');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'That the company has permanently closed down.', 0),
        (@MaCH, N'B', N'That the recipient is away from work, their expected return date, and emergency contact details.', 1),
        (@MaCH, N'C', N'That the sender should never email again.', 0),
        (@MaCH, N'D', N'That server disk space is full.', 0);
END
GO

-- Câu hỏi SEED_EAI_130 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_130')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''The contract is legally _______ once signed by authorized representatives of both corporations.''',
            NULL, NULL, N'Cụm từ pháp lý: ''legally binding'' (có hiệu lực ràng buộc về mặt pháp lý).', N'Cụm từ pháp lý: ''legally binding'' (có hiệu lực ràng buộc về mặt pháp lý).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Thuật ngữ pháp lý hợp đồng: Legally binding contract.', N'Extracted', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_130');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'binding', 1),
        (@MaCH, N'B', N'locking', 0),
        (@MaCH, N'C', N'gluing', 0),
        (@MaCH, N'D', N'tying', 0);
END
GO

-- Câu hỏi SEED_EAI_131 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_131')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'TuVung', N'Kho', N'What is a ''Non-Disclosure Agreement'' (NDA) designed to protect?',
            NULL, NULL, N'''NDA'' (Thỏa thuận bảo mật thông tin) được ký kết để bảo vệ bí mật kinh doanh và thông tin độc quyền giữa các đối tác.', N'''NDA'' (Thỏa thuận bảo mật thông tin) được ký kết để bảo vệ bí mật kinh doanh và thông tin độc quyền giữa các đối tác.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Các loại hợp đồng thương mại: NDA (Non-Disclosure Agreement).', N'Extracted', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_131');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Public marketing slogans', 0),
        (@MaCH, N'B', N'Confidential business information, intellectual property, and proprietary trade secrets', 1),
        (@MaCH, N'C', N'Office recycling guidelines', 0),
        (@MaCH, N'D', N'Cafeteria lunch menus', 0);
END
GO

-- Câu hỏi SEED_EAI_132 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_132')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the mistake: ''Unless both parties will agree in writing, no amendments can be made.''',
            NULL, NULL, N'Trong mệnh đề chỉ điều kiện bắt đầu bằng ''Unless'' (trừ khi), động từ chia ở thì hiện tại đơn, không dùng ''will'': ''Unless both parties agree...''.', N'Trong mệnh đề chỉ điều kiện bắt đầu bằng ''Unless'' (trừ khi), động từ chia ở thì hiện tại đơn, không dùng ''will'': ''Unless both parties agree...''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Quy tắc ngữ pháp mệnh đề điều kiện trong hợp đồng (Unless + Present Simple).', N'Applied', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_132');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''parties'' should be ''partys''', 0),
        (@MaCH, N'B', N'''will agree'' should be ''agree''', 1),
        (@MaCH, N'C', N'''in writing'' should be ''on written''', 0),
        (@MaCH, N'D', N'''can'' should be ''could''', 0);
END
GO

-- Câu hỏi SEED_EAI_133 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_133')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'Kho', N'During Q&A after a project pitch, how should a presenter handle a question they do not immediately have the exact data for?',
            NULL, NULL, N'Cách xử lý chuyên nghiệp: Thừa nhận câu hỏi sâu sắc, ghi nhận và cam kết gửi dữ liệu chi tiết sau buổi thuyết trình.', N'Cách xử lý chuyên nghiệp: Thừa nhận câu hỏi sâu sắc, ghi nhận và cam kết gửi dữ liệu chi tiết sau buổi thuyết trình.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 24 - Kỹ năng ứng biến trong phần Q&A: ''That is a pertinent question; let me follow up with the exact figures after this session.''', N'Extracted', N'BaiHoc_24_ProjectPitching.txt', 24, 'SEED_EAI_133');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Make up random numbers to sound smart.', 0),
        (@MaCH, N'B', N'Acknowledge the question, express willingness to follow up with exact figures, and move on smoothly.', 1),
        (@MaCH, N'C', N'Tell the investor they have no right to ask that.', 0),
        (@MaCH, N'D', N'End the pitch and walk off stage.', 0);
END
GO

-- Câu hỏi SEED_EAI_134 (Viet - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_134')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 3, N'DienKhuyet', N'Viet', N'TrungBinh', N'Fill in the blank: ''We look forward to establishing a mutually _______ partnership with your organization.''',
            NULL, NULL, N'''mutually beneficial'' (đôi bên cùng có lợi) là tính từ ghép chuẩn trong thư đối ngoại.', N'''mutually beneficial'' (đôi bên cùng có lợi) là tính từ ghép chuẩn trong thư đối ngoại.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Mẫu câu thiết lập quan hệ đối tác: Mutually beneficial partnership.', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_134');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'beneficial', 1),
        (@MaCH, N'B', N'benefit', 0),
        (@MaCH, N'C', N'benefiting', 0),
        (@MaCH, N'D', N'beneficiary', 0);
END
GO

-- Câu hỏi SEED_EAI_135 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_135')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What does the commercial term ''Lead Time'' refer to?',
            NULL, NULL, N'''Lead time'' là khoảng thời gian từ khi bắt đầu đặt đơn hàng cho đến khi hàng được sản xuất và giao tới nơi.', N'''Lead time'' là khoảng thời gian từ khi bắt đầu đặt đơn hàng cho đến khi hàng được sản xuất và giao tới nơi.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Điều khoản giao hàng: Lead time (thời gian từ đặt hàng đến giao nhận).', N'Extracted', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_135');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The time taken to drink coffee at work', 0),
        (@MaCH, N'B', N'The total amount of time between order placement and the delivery of finished goods', 1),
        (@MaCH, N'C', N'The duration of leadership training courses', 0),
        (@MaCH, N'D', N'The time taken to sign an email signature', 0);
END
GO

-- Câu hỏi SEED_EAI_136 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_136')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What does ''Subject Line'' in a professional email need to achieve?',
            NULL, NULL, N'Dòng tiêu đề (Subject line) phải ngắn gọn, súc tích và nêu bật ngay nội dung trọng tâm của email.', N'Dòng tiêu đề (Subject line) phải ngắn gọn, súc tích và nêu bật ngay nội dung trọng tâm của email.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Quy tắc vàng viết tiêu đề email (Clear & Concise Subject Lines).', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_136');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Be vague and mysterious to force people to open it.', 0),
        (@MaCH, N'B', N'Be concise, clear, and accurately summarize the email''s core purpose.', 1),
        (@MaCH, N'C', N'Contain humorous emojis exclusively.', 0),
        (@MaCH, N'D', N'Be left completely empty.', 0);
END
GO

-- Câu hỏi SEED_EAI_137 (Doc - Trình độ 3 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_137')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 3, N'DungSai', N'Doc', N'De', N'True or False: ''Reply All'' should be used cautiously to avoid cluttering inboxes of colleagues who do not need the message.',
            NULL, NULL, N'Đúng. Cần cân nhắc kỹ trước khi bấm ''Reply All'' để tránh làm phiền những người không liên quan.', N'Đúng. Cần cân nhắc kỹ trước khi bấm ''Reply All'' để tránh làm phiền những người không liên quan.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Quy tắc giao tiếp văn phòng: Sử dụng Reply vs. Reply All đúng người đúng việc.', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_137');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_138 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_138')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'TuVung', N'Kho', N'What does ''Burn Rate'' represent in startup finance and executive pitch decks?',
            NULL, NULL, N'''Burn Rate'' (Tốc độ đốt tiền) là tốc độ mà một công ty khởi nghiệp tiêu hao vốn khả dụng trước khi đạt điểm hòa vốn hoặc có dòng tiền dương.', N'''Burn Rate'' (Tốc độ đốt tiền) là tốc độ mà một công ty khởi nghiệp tiêu hao vốn khả dụng trước khi đạt điểm hòa vốn hoặc có dòng tiền dương.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 24 - Chỉ số tài chính Startup: Burn rate and Runway.', N'Extracted', N'BaiHoc_24_ProjectPitching.txt', 24, 'SEED_EAI_138');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The speed at which physical paper catches fire in the office', 0),
        (@MaCH, N'B', N'The rate at which a company spends its available capital before achieving positive cash flow', 1),
        (@MaCH, N'C', N'The electricity consumption of server racks', 0),
        (@MaCH, N'D', N'The turnover rate of kitchen staff', 0);
END
GO

-- Câu hỏi SEED_EAI_139 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_139')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'In employment contracts, what is a ''Probationary Period''?',
            NULL, NULL, N'''Probationary period'' (Thời gian thử việc) là giai đoạn đầu làm việc nhằm đánh giá sự phù hợp của nhân viên trước khi ký hợp đồng chính thức.', N'''Probationary period'' (Thời gian thử việc) là giai đoạn đầu làm việc nhằm đánh giá sự phù hợp của nhân viên trước khi ký hợp đồng chính thức.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Thuật ngữ hợp đồng lao động: Probationary period.', N'Extracted', N'03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx', NULL, 'SEED_EAI_139');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A retirement vacation', 0),
        (@MaCH, N'B', N'A trial period where employee performance and suitability are formally evaluated', 1),
        (@MaCH, N'C', N'An annual bonus award celebration', 0),
        (@MaCH, N'D', N'A permanent contract with zero reviews', 0);
END
GO

-- Câu hỏi SEED_EAI_140 (Viet - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_140')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 4, N'VietLaiCau', N'Viet', N'TrungBinh', N'Rewrite the sentence into a polite business confirmation: ''I got the file and everything is good.''',
            NULL, NULL, N'''I hereby acknowledge safe receipt of the documents, which are in order.'' là cách xác nhận chuyên nghiệp.', N'''I hereby acknowledge safe receipt of the documents, which are in order.'' là cách xác nhận chuyên nghiệp.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 22 - Xác nhận nhận tài liệu trong văn phòng.', N'Applied', N'BaiHoc_22_AppointmentAndComplaints.txt', 22, 'SEED_EAI_140');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'We hereby acknowledge receipt of the documents, and confirm that all items are in order.', 1),
        (@MaCH, N'B', N'File is in my hands and cool.', 0),
        (@MaCH, N'C', N'I have downloaded the thing you sent.', 0),
        (@MaCH, N'D', N'File confirmed by me.', 0);
END
GO

-- Câu hỏi SEED_EAI_141 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_141')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Complete the contract clause: ''This agreement shall remain in _______ for a period of three calendar years.''',
            NULL, NULL, N'Thành ngữ pháp lý: ''remain in effect / force'' (duy trì hiệu lực thi hành).', N'Thành ngữ pháp lý: ''remain in effect / force'' (duy trì hiệu lực thi hành).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Điều khoản thời hạn hợp đồng: Remain in effect for a period of...', N'Extracted', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_141');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'effect', 1),
        (@MaCH, N'B', N'sleep', 0),
        (@MaCH, N'C', N'pause', 0),
        (@MaCH, N'D', N'doubt', 0);
END
GO

-- Câu hỏi SEED_EAI_142 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_142')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What does ''ROI'' stand for in business investment evaluations?',
            NULL, NULL, N'''ROI'' là viết tắt của ''Return on Investment'' (Tỷ suất hoàn vốn đầu tư).', N'''ROI'' là viết tắt của ''Return on Investment'' (Tỷ suất hoàn vốn đầu tư).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 24 - Chỉ số đo lường hiệu quả kinh doanh: ROI (Return on Investment).', N'Extracted', N'BaiHoc_24_ProjectPitching.txt', 24, 'SEED_EAI_142');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Return on Investment', 1),
        (@MaCH, N'B', N'Rate of Interest', 0),
        (@MaCH, N'C', N'Revenue on Invoice', 0),
        (@MaCH, N'D', N'Real Operating Income', 0);
END
GO

-- Câu hỏi SEED_EAI_143 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_143')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 3, N'TracNghiem', N'Doc', N'De', N'Which salutation is appropriate in an email to a woman when you do not know her marital status?',
            NULL, NULL, N'''Ms.'' là danh xưng chuẩn mực và an toàn nhất trong công sở khi gửi thư cho phụ nữ (không phân biệt đã kết hôn hay chưa).', N'''Ms.'' là danh xưng chuẩn mực và an toàn nhất trong công sở khi gửi thư cho phụ nữ (không phân biệt đã kết hôn hay chưa).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Quy chuẩn xưng hô danh xưng: Ms. [Họ] là chuẩn mực trung lập nhất.', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_143');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Mrs.', 0),
        (@MaCH, N'B', N'Ms.', 1),
        (@MaCH, N'C', N'Miss', 0),
        (@MaCH, N'D', N'Madamoiselle', 0);
END
GO

-- Câu hỏi SEED_EAI_144 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_144')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'TuVung', N'Kho', N'What does the business idiom ''think outside the box'' encourage employees to do?',
            NULL, NULL, N'''Think outside the box'' khuyến khích tư duy đột phá, sáng tạo và tìm kiếm giải pháp phi truyền thống.', N'''Think outside the box'' khuyến khích tư duy đột phá, sáng tạo và tìm kiếm giải pháp phi truyền thống.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 24 - Thành ngữ kinh doanh quốc tế: Think outside the box.', N'Extracted', N'BaiHoc_24_ProjectPitching.txt', 24, 'SEED_EAI_144');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Work outdoors in the courtyard', 0),
        (@MaCH, N'B', N'Approach problems in an innovative, unconventional, and creative manner', 1),
        (@MaCH, N'C', N'Throw away shipping containers', 0),
        (@MaCH, N'D', N'Focus exclusively on old methodologies', 0);
END
GO

-- Câu hỏi SEED_EAI_145 (Viet - Trình độ 3 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_145')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 3, N'DungSai', N'Viet', N'De', N'True or False: Using a clear call-to-action (CTA) at the end of an email makes it easier for the recipient to know what next steps are expected.',
            NULL, NULL, N'Đúng. Lời kêu gọi hành động rõ ràng (Call to Action) ở cuối thư giúp người nhận biết chính xác việc cần làm tiếp theo.', N'Đúng. Lời kêu gọi hành động rõ ràng (Call to Action) ở cuối thư giúp người nhận biết chính xác việc cần làm tiếp theo.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Cấu trúc Call to Action trong email bán hàng và giao dịch.', N'Extracted', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_145');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_146 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_146')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What is a ''Milestone'' in a project deliverable schedule?',
            NULL, NULL, N'''Milestone'' (Cột mốc dự án) là sự kiện đánh dấu sự hoàn thành của một giai đoạn quan trọng trong tiến độ dự án.', N'''Milestone'' (Cột mốc dự án) là sự kiện đánh dấu sự hoàn thành của một giai đoạn quan trọng trong tiến độ dự án.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Điều khoản tiến độ giao nhận: Project Milestones.', N'Extracted', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_146');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A physical boundary stone on a highway', 0),
        (@MaCH, N'B', N'A significant checkpoint or stage completion in a project lifecycle', 1),
        (@MaCH, N'C', N'A daily coffee break tracker', 0),
        (@MaCH, N'D', N'The maximum file size of an email', 0);
END
GO

-- Câu hỏi SEED_EAI_147 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_147')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''Employees who perform exceptionally well may receive an annual performance _______.''',
            NULL, NULL, N'''Performance bonus'' (tiền thưởng theo hiệu suất công việc).', N'''Performance bonus'' (tiền thưởng theo hiệu suất công việc).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Các chế độ đãi ngộ: Performance bonus.', N'Extracted', N'03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx', NULL, 'SEED_EAI_147');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'bonus', 1),
        (@MaCH, N'B', N'fine', 0),
        (@MaCH, N'C', N'debt', 0),
        (@MaCH, N'D', N'penalty', 0);
END
GO

-- Câu hỏi SEED_EAI_148 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_148')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'What is the recommended body language posture during a business negotiation meeting?',
            NULL, NULL, N'Ngồi thẳng người, duy trì giao tiếp bằng mắt (eye contact) và cử chỉ mở (open gestures) thể hiện sự tự tin và cởi mở.', N'Ngồi thẳng người, duy trì giao tiếp bằng mắt (eye contact) và cử chỉ mở (open gestures) thể hiện sự tự tin và cởi mở.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 23 - Ngôn ngữ cơ thể trong đàm phán thương mại (Body language and eye contact).', N'Extracted', N'BaiHoc_23_ContractNegotiation.txt', 23, 'SEED_EAI_148');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Slouching and looking at your phone continuously', 0),
        (@MaCH, N'B', N'Upright posture, respectful eye contact, and attentive listening', 1),
        (@MaCH, N'C', N'Arms tightly crossed and turning your back', 0),
        (@MaCH, N'D', N'Staring blankly at the floor', 0);
END
GO

-- Câu hỏi SEED_EAI_149 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_149')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (14, 4, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the mistake: ''We appreciate if you could sends us the updated quotation by Monday.''',
            NULL, NULL, N'Sau trợ động từ khiếm khuyết ''could'', động từ chính phải ở dạng nguyên mẫu không chia: ''could send us''.', N'Sau trợ động từ khiếm khuyết ''could'', động từ chính phải ở dạng nguyên mẫu không chia: ''could send us''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 21 - Trợ động từ khiếm khuyết trong câu yêu cầu lịch sự: Could + V-inf.', N'Applied', N'BaiHoc_21_CommercialEmail.txt', 21, 'SEED_EAI_149');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''appreciate'' should be ''deprecate''', 0),
        (@MaCH, N'B', N'''sends'' should be ''send''', 1),
        (@MaCH, N'C', N'''updated'' should be ''updating''', 0),
        (@MaCH, N'D', N'''by'' should be ''on''', 0);
END
GO

-- Câu hỏi SEED_EAI_150 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_150')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'Kho', N'What does the presentation principle ''Signposting'' mean?',
            NULL, NULL, N'''Signposting'' là việc dùng các từ ngữ chỉ dẫn (như ''First of all'', ''Turning to'', ''In conclusion'') để người nghe dễ dàng theo dõi dòng ý tưởng.', N'''Signposting'' là việc dùng các từ ngữ chỉ dẫn (như ''First of all'', ''Turning to'', ''In conclusion'') để người nghe dễ dàng theo dõi dòng ý tưởng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 24 - Kỹ thuật Signposting trong thuyết trình chuyên nghiệp.', N'Extracted', N'BaiHoc_24_ProjectPitching.txt', 24, 'SEED_EAI_150');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Hanging physical road signs on the classroom walls', 0),
        (@MaCH, N'B', N'Using verbal verbal cues and roadmap phrases that guide listeners through the presentation structure', 1),
        (@MaCH, N'C', N'Drawing arrows on a whiteboard silently', 0),
        (@MaCH, N'D', N'Asking the audience to sign an attendance sheet', 0);
END
GO

PRINT N'Đã hoàn thành nạp Batch 03 (SEED_EAI_101 -> SEED_EAI_150) thành công!';
GO
