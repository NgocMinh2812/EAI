-- ====================================================================================
-- SEED QUESTIONS BATCH: Batch 10 (SEED_EAI_451 -> SEED_EAI_500)
-- Số lượng câu hỏi: 50
-- Đảm bảo: Idempotent (Chạy lại nhiều lần không trùng lặp)
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Đang nạp Batch 10 (SEED_EAI_451 -> SEED_EAI_500) (50 câu hỏi)...';

-- Câu hỏi SEED_EAI_451 (NguPhap - Trình độ 5 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_451')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'VietLaiCau', N'NguPhap', N'Kho', N'Invert the sentence starting with ''No sooner'': ''As soon as the CEO stepped into the conference hall, the journalists bombarded him with inquiries.''',
            NULL, NULL, N'Cấu trúc đảo ngữ: ''No sooner had + S + V3/ed + than + S + V2/ed''.', N'Cấu trúc đảo ngữ: ''No sooner had + S + V3/ed + than + S + V2/ed''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 1: Đảo ngữ với No sooner ... than và Hardly ... when.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_451');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'No sooner had the CEO stepped into the conference hall than the journalists bombarded him with inquiries.', 1),
        (@MaCH, N'B', N'No sooner the CEO had stepped into the conference hall when the journalists bombarded him.', 0),
        (@MaCH, N'C', N'No sooner did the CEO stepped into the conference hall than the journalists bombarded him.', 0),
        (@MaCH, N'D', N'No sooner had the CEO stepped into the hall when journalists did bombard him.', 0);
END
GO

-- Câu hỏi SEED_EAI_452 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_452')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'NguPhap', N'Kho', N'Choose the correct inversion form: ''Under no circumstances _______ the confidential cryptographic keys to any third party.''',
            NULL, NULL, N'Sau cụm từ phủ định ''Under no circumstances'' (dù trong bất cứ trường hợp nào cũng không), ta đảo trợ động từ lên trước chủ ngữ: ''should you disclose''.', N'Sau cụm từ phủ định ''Under no circumstances'' (dù trong bất cứ trường hợp nào cũng không), ta đảo trợ động từ lên trước chủ ngữ: ''should you disclose''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 2: Đảo ngữ với cụm từ phủ định tuyệt đối (Under no circumstances, On no account).', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_452');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'you should disclose', 0),
        (@MaCH, N'B', N'should you disclose', 1),
        (@MaCH, N'C', N'you will disclose', 0),
        (@MaCH, N'D', N'do you must disclose', 0);
END
GO

-- Câu hỏi SEED_EAI_453 (NguPhap - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_453')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'NguPhap', N'Kho', N'Hardly had the server system rebooted _______ a fresh wave of DDoS attacks overwhelmed the firewall.',
            NULL, NULL, N'Cấu trúc tương liên: ''Hardly had + S + V3/ed + WHEN + S + V2/ed''.', N'Cấu trúc tương liên: ''Hardly had + S + V3/ed + WHEN + S + V2/ed''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Phân biệt cặp liên từ: Hardly/Scarcely... WHEN đối chiếu với No sooner... THAN.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_453');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'when', 1),
        (@MaCH, N'B', N'than', 0),
        (@MaCH, N'C', N'then', 0),
        (@MaCH, N'D', N'that', 0);
END
GO

-- Câu hỏi SEED_EAI_454 (NguPhap - Trình độ 5 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_454')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'VietLaiCau', N'NguPhap', N'Kho', N'Invert the conditional sentence: ''If the board of directors were to reject our proposal, we would pursue venture debt.''',
            NULL, NULL, N'Đảo ngữ câu điều kiện loại 2 với ''Were'': ''Were the board of directors to reject our proposal, we would pursue venture debt.''', N'Đảo ngữ câu điều kiện loại 2 với ''Were'': ''Were the board of directors to reject our proposal, we would pursue venture debt.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 3: Đảo ngữ câu điều kiện (Should / Were / Had).', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_454');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Were the board of directors to reject our proposal, we would pursue venture debt.', 1),
        (@MaCH, N'B', N'Were the board reject our proposal, we would pursue venture debt.', 0),
        (@MaCH, N'C', N'If were the board of directors rejecting our proposal, we pursue venture debt.', 0),
        (@MaCH, N'D', N'Did the board of directors reject our proposal, we would pursue venture debt.', 0);
END
GO

-- Câu hỏi SEED_EAI_455 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_455')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'NguPhap', N'Kho', N'Which sentence correctly illustrates inversion with ''Seldom''?',
            NULL, NULL, N'''Seldom have I witnessed such profound resilience in the face of insurmountable economic hardship.'' (Đảo trợ động từ ''have'' lên trước ''I'').', N'''Seldom have I witnessed such profound resilience in the face of insurmountable economic hardship.'' (Đảo trợ động từ ''have'' lên trước ''I'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 4: Đảo ngữ với phó từ tần suất bán phủ định (Seldom, Rarely, Little).', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_455');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Seldom I have witnessed such profound resilience in the face of hardship.', 0),
        (@MaCH, N'B', N'Seldom have I witnessed such profound resilience in the face of insurmountable economic hardship.', 1),
        (@MaCH, N'C', N'Seldom did I witnessed such profound resilience in the face of hardship.', 0),
        (@MaCH, N'D', N'Seldom I witnessed such profound resilience.', 0);
END
GO

-- Câu hỏi SEED_EAI_456 (NguPhap - Trình độ 5 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_456')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'SuaLoiSai', N'NguPhap', N'Kho', N'Find the error: ''Not until the audit report was published (A) last week the shareholders realized (B) the sheer extent (C) of corporate mismanagement (D).''',
            NULL, NULL, N'Lỗi ở (B) ''the shareholders realized''. Sau mệnh đề thời gian ''Not until...'', mệnh đề chính phải đảo ngữ: ''did the shareholders realize''.', N'Lỗi ở (B) ''the shareholders realized''. Sau mệnh đề thời gian ''Not until...'', mệnh đề chính phải đảo ngữ: ''did the shareholders realize''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 5: Đảo ngữ với ''Not until'' và vị trí đảo ngữ ở mệnh đề chính.', N'Applied', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_456');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'was published', 0),
        (@MaCH, N'B', N'the shareholders realized', 1),
        (@MaCH, N'C', N'the sheer extent', 0),
        (@MaCH, N'D', N'of corporate mismanagement', 0);
END
GO

-- Câu hỏi SEED_EAI_457 (NguPhap - Trình độ 5 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_457')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'VietLaiCau', N'NguPhap', N'Kho', N'Invert using ''Should'': ''If you require any further documentation regarding the offshore investment, please contact compliance.''',
            NULL, NULL, N'Đảo ngữ điều kiện loại 1: ''Should you require any further documentation regarding the offshore investment, please contact compliance.''', N'Đảo ngữ điều kiện loại 1: ''Should you require any further documentation regarding the offshore investment, please contact compliance.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ điều kiện loại 1 với ''Should''.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_457');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Should you require any further documentation regarding the offshore investment, please contact compliance.', 1),
        (@MaCH, N'B', N'Should you requiring any further documentation, contact compliance.', 0),
        (@MaCH, N'C', N'If should you require documentation, contact compliance.', 0),
        (@MaCH, N'D', N'Should you to require further documentation, contact compliance.', 0);
END
GO

-- Câu hỏi SEED_EAI_458 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_458')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'NguPhap', N'Kho', N'Complete the inverted sentence: ''So intense _______ that all scheduled flights were grounded indefinitely.''',
            NULL, NULL, N'Cấu trúc: ''So + Adj + be + S + that...'': ''So intense was the blizzard that all scheduled flights were grounded indefinitely.''', N'Cấu trúc: ''So + Adj + be + S + that...'': ''So intense was the blizzard that all scheduled flights were grounded indefinitely.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 6: Đảo ngữ nhấn mạnh tính từ với So ... that và Such ... that.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_458');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'the blizzard was', 0),
        (@MaCH, N'B', N'was the blizzard', 1),
        (@MaCH, N'C', N'did the blizzard be', 0),
        (@MaCH, N'D', N'the blizzard had been', 0);
END
GO

-- Câu hỏi SEED_EAI_459 (NguPhap - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_459')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'NguPhap', N'Kho', N'Little _______ the lead developer suspect that a tiny memory leak would crash the core production database during peak hours.',
            NULL, NULL, N'Sau ''Little'' mang nghĩa phủ định, dùng đảo ngữ thời quá khứ đơn: ''Little did the lead developer suspect...''.', N'Sau ''Little'' mang nghĩa phủ định, dùng đảo ngữ thời quá khứ đơn: ''Little did the lead developer suspect...''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ với ''Little did + S + V''.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_459');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'did', 1),
        (@MaCH, N'B', N'does', 0),
        (@MaCH, N'C', N'was', 0),
        (@MaCH, N'D', N'had', 0);
END
GO

-- Câu hỏi SEED_EAI_460 (NguPhap - Trình độ 5 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_460')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'VietLaiCau', N'NguPhap', N'Kho', N'Invert using ''Such'': ''The chaos caused by the power outage was so severe that emergency protocols were enacted.''',
            NULL, NULL, N'Cấu trúc: ''Such + be + S + that...'': ''Such was the chaos caused by the power outage that emergency protocols were enacted.''', N'Cấu trúc: ''Such + be + S + that...'': ''Such was the chaos caused by the power outage that emergency protocols were enacted.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Cấu trúc đảo ngữ Such was... that...', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_460');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Such was the chaos caused by the power outage that emergency protocols were enacted.', 1),
        (@MaCH, N'B', N'Such the chaos was that emergency protocols were enacted.', 0),
        (@MaCH, N'C', N'Such did the chaos caused that emergency protocols were enacted.', 0),
        (@MaCH, N'D', N'Such was chaos that it caused power outage.', 0);
END
GO

-- Câu hỏi SEED_EAI_461 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_461')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'NguPhap', N'Kho', N'''Only after conducting an exhaustive forensic analysis _______ the root cause of the network breach.''',
            NULL, NULL, N'Sau ''Only after + clause/phrase'', mệnh đề sau phải đảo trợ động từ: ''did the cybersecurity team pinpoint''.', N'Sau ''Only after + clause/phrase'', mệnh đề sau phải đảo trợ động từ: ''did the cybersecurity team pinpoint''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 7: Đảo ngữ với Only after / Only when.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_461');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'the cybersecurity team pinpointed', 0),
        (@MaCH, N'B', N'did the cybersecurity team pinpoint', 1),
        (@MaCH, N'C', N'pinpointed the cybersecurity team', 0),
        (@MaCH, N'D', N'the team had pinpointed', 0);
END
GO

-- Câu hỏi SEED_EAI_462 (NguPhap - Trình độ 5 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_462')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DungSai', N'NguPhap', N'Kho', N'True or False: In conditional inversion type 3 (''Had + S + V3''), the word ''if'' must be preserved immediately before ''Had''.',
            NULL, NULL, N'Sai (False). Khi thực hiện đảo ngữ câu điều kiện, từ ''if'' bắt buộc phải bị lược bỏ hoàn toàn (e.g. ''Had I known'', KHÔNG dùng ''If had I known'').', N'Sai (False). Khi thực hiện đảo ngữ câu điều kiện, từ ''if'' bắt buộc phải bị lược bỏ hoàn toàn (e.g. ''Had I known'', KHÔNG dùng ''If had I known'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Quy tắc lược bỏ ''If'' khi đảo ngữ câu điều kiện.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_462');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_463 (NguPhap - Trình độ 5 - Dạng SuaLoiSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_463')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'SuaLoiSai', N'NguPhap', N'Kho', N'Find the mistake: ''On no account (A) passengers should attempt (B) to disembark before (C) the aircraft has come to a complete standstill (D).''',
            NULL, NULL, N'Lỗi ở (B) ''passengers should attempt''. Sau ''On no account'' (tuyệt đối không), phải đảo ngữ: ''should passengers attempt''.', N'Lỗi ở (B) ''passengers should attempt''. Sau ''On no account'' (tuyệt đối không), phải đảo ngữ: ''should passengers attempt''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ bắt buộc với On no account.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_463');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'On no account', 0),
        (@MaCH, N'B', N'passengers should attempt', 1),
        (@MaCH, N'C', N'before', 0),
        (@MaCH, N'D', N'complete standstill', 0);
END
GO

-- Câu hỏi SEED_EAI_464 (NguPhap - Trình độ 5 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_464')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'VietLaiCau', N'NguPhap', N'Kho', N'Invert using ''Not only'': ''The new quantum processor performs calculations at unprecedented speeds, and it also consumes 40% less electrical power.''',
            NULL, NULL, N'''Not only does the new quantum processor perform calculations at unprecedented speeds, but it also consumes 40% less electrical power.''', N'''Not only does the new quantum processor perform calculations at unprecedented speeds, but it also consumes 40% less electrical power.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 8: Đảo ngữ với Not only... but also.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_464');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Not only does the new quantum processor perform calculations at unprecedented speeds, but it also consumes 40% less electrical power.', 1),
        (@MaCH, N'B', N'Not only the quantum processor performs calculations, but also consumes power.', 0),
        (@MaCH, N'C', N'Not only did the processor performed calculations, but it consumes power.', 0),
        (@MaCH, N'D', N'Not only performs the processor calculations, but it also consumes.', 0);
END
GO

-- Câu hỏi SEED_EAI_465 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_465')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'NguPhap', N'Kho', N'''At no time _______ aware of the fraudulent transactions taking place inside the branch.''',
            NULL, NULL, N'Sau cụm phủ định ''At no time'', ta đảo trợ động từ to be: ''was the manager aware''.', N'Sau cụm phủ định ''At no time'', ta đảo trợ động từ to be: ''was the manager aware''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ với At no time.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_465');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'the manager was', 0),
        (@MaCH, N'B', N'was the manager', 1),
        (@MaCH, N'C', N'did the manager', 0),
        (@MaCH, N'D', N'the manager had been', 0);
END
GO

-- Câu hỏi SEED_EAI_466 (NguPhap - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_466')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'NguPhap', N'Kho', N'Only by mastering prompt architecture and algorithmic foundations _______ software engineers thrive in the era of generative AI.',
            NULL, NULL, N'''can'' (sau ''Only by...'', ta đảo động từ khuyết thiếu ''can software engineers thrive'').', N'''can'' (sau ''Only by...'', ta đảo động từ khuyết thiếu ''can software engineers thrive'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ với Only by.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_466');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'can', 1),
        (@MaCH, N'B', N'did', 0),
        (@MaCH, N'C', N'are', 0),
        (@MaCH, N'D', N'have', 0);
END
GO

-- Câu hỏi SEED_EAI_467 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_467')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'NguPhap', N'Kho', N'Which sentence showcases correct locative inversion (đảo ngữ chỉ nơi chốn/hướng di chuyển)?',
            NULL, NULL, N'''Along the cobblestone avenue stood rows of centuries-old neoclassical buildings.'' (Đưa cụm trạng từ chỉ nơi chốn lên đầu câu, đảo trực tiếp nội động từ ''stood'' lên trước chủ ngữ).', N'''Along the cobblestone avenue stood rows of centuries-old neoclassical buildings.'' (Đưa cụm trạng từ chỉ nơi chốn lên đầu câu, đảo trực tiếp nội động từ ''stood'' lên trước chủ ngữ).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 9: Đảo ngữ toàn bộ với trạng từ chỉ địa điểm/phương hướng (Locative Inversion).', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_467');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Along the cobblestone avenue rows of buildings stood.', 0),
        (@MaCH, N'B', N'Along the cobblestone avenue stood rows of centuries-old neoclassical buildings.', 1),
        (@MaCH, N'C', N'Along the avenue did stand rows of buildings.', 0),
        (@MaCH, N'D', N'Along the avenue was standing rows of buildings.', 0);
END
GO

-- Câu hỏi SEED_EAI_468 (NguPhap - Trình độ 5 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_468')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'VietLaiCau', N'NguPhap', N'Kho', N'Invert using ''Had'': ''If we had recognized the algorithmic bias earlier, the model deployment would not have triggered public controversy.''',
            NULL, NULL, N'''Had we recognized the algorithmic bias earlier, the model deployment would not have triggered public controversy.''', N'''Had we recognized the algorithmic bias earlier, the model deployment would not have triggered public controversy.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ điều kiện loại 3.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_468');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Had we recognized the algorithmic bias earlier, the model deployment would not have triggered public controversy.', 1),
        (@MaCH, N'B', N'Had we recognize the algorithmic bias earlier, model deployment would not trigger controversy.', 0),
        (@MaCH, N'C', N'If had we recognized the bias earlier, deployment would not trigger controversy.', 0),
        (@MaCH, N'D', N'Did we have recognized the bias earlier, controversy would not happen.', 0);
END
GO

-- Câu hỏi SEED_EAI_469 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_469')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'NguPhap', N'Kho', N'''Scarcely _______ the opening speech when the electrical grid suffered a sudden blackout.''',
            NULL, NULL, N'Cấu trúc: ''Scarcely had + S + V3/ed + when + S + V2/ed'': ''Scarcely had the keynote speaker concluded''.', N'Cấu trúc: ''Scarcely had + S + V3/ed + when + S + V2/ed'': ''Scarcely had the keynote speaker concluded''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Cấu trúc Scarcely had ... when.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_469');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'the keynote speaker had concluded', 0),
        (@MaCH, N'B', N'had the keynote speaker concluded', 1),
        (@MaCH, N'C', N'concluded the keynote speaker', 0),
        (@MaCH, N'D', N'the keynote speaker concluded', 0);
END
GO

-- Câu hỏi SEED_EAI_470 (NguPhap - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_470')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'NguPhap', N'Kho', N'Rarely _______ an academic paper provoke such passionate discourse among international scholars.',
            NULL, NULL, N'''does'' (đảo trợ động từ hiện tại đơn: ''Rarely does an academic paper provoke...'').', N'''does'' (đảo trợ động từ hiện tại đơn: ''Rarely does an academic paper provoke...'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ với Rarely ở thì hiện tại đơn.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_470');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'does', 1),
        (@MaCH, N'B', N'is', 0),
        (@MaCH, N'C', N'has', 0),
        (@MaCH, N'D', N'was', 0);
END
GO

-- Câu hỏi SEED_EAI_471 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_471')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'TuVung', N'Kho', N'After months of delaying the painful restructuring, the CEO finally decided to ''bite the bullet''. What does this idiom mean?',
            NULL, NULL, N'''Bite the bullet'' có nghĩa là can đảm đưa ra quyết định hoặc đối mặt với một tình huống đau đớn, khó khăn mà mình từng né tránh.', N'''Bite the bullet'' có nghĩa là can đảm đưa ra quyết định hoặc đối mặt với một tình huống đau đớn, khó khăn mà mình từng né tránh.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Thành ngữ nâng cao C1 & Quản trị kinh doanh - Cụm từ ''Bite the bullet''.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_471');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'To face a painful or difficult situation with fortitude after prolonging it', 1),
        (@MaCH, N'B', N'To purchase weapons for security defense', 0),
        (@MaCH, N'C', N'To cancel all future board meetings', 0),
        (@MaCH, N'D', N'To invest capital in pharmaceutical startups', 0);
END
GO

-- Câu hỏi SEED_EAI_472 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_472')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'TuVung', N'Kho', N'The final research deadline was tight, forcing the entire laboratory staff to burn the midnight _______.',
            NULL, NULL, N'''burn the midnight oil'' (thức khuya làm việc hoặc học tập cật lực).', N'''burn the midnight oil'' (thức khuya làm việc hoặc học tập cật lực).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Thành ngữ tiếng Anh C1 - ''Burn the midnight oil''.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_472');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'oil', 1),
        (@MaCH, N'B', N'lamp', 0),
        (@MaCH, N'C', N'candle', 0),
        (@MaCH, N'D', N'fuel', 0);
END
GO

-- Câu hỏi SEED_EAI_473 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_473')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'TuVung', N'Kho', N'When evaluating anonymous political commentary, wise analysts always ''take it with a grain of salt''. This implies:',
            NULL, NULL, N'''Take with a grain of salt'' có nghĩa là tiếp nhận thông tin với thái độ hoài nghi, dè dặt, không tin tưởng hoàn toàn 100%.', N'''Take with a grain of salt'' có nghĩa là tiếp nhận thông tin với thái độ hoài nghi, dè dặt, không tin tưởng hoàn toàn 100%.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Phụ lục Thành ngữ tư duy phản biện C1.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_473');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Accepting the claim with healthy skepticism rather than literal unquestioning belief', 1),
        (@MaCH, N'B', N'Adding sodium to one''s diet for cognitive health', 0),
        (@MaCH, N'C', N'Rejecting every single proposition without reading', 0),
        (@MaCH, N'D', N'Memorizing every single word exactly', 0);
END
GO

-- Câu hỏi SEED_EAI_474 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_474')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'TuVung', N'Kho', N'Her meticulous analysis really ''hit the nail on the head''. What did she do?',
            NULL, NULL, N'''Hit the nail on the head'' nghĩa là nói trúng phóc, miêu tả chính xác tuyệt đối nguyên nhân hoặc bản chất của vấn đề.', N'''Hit the nail on the head'' nghĩa là nói trúng phóc, miêu tả chính xác tuyệt đối nguyên nhân hoặc bản chất của vấn đề.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Thành ngữ diễn đạt sự chính xác tuyệt đối.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_474');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Described the exact cause or truth of a situation with precision', 1),
        (@MaCH, N'B', N'Damaged the carpentry equipment during construction', 0),
        (@MaCH, N'C', N'Made an irrelevant comment that confused everyone', 0),
        (@MaCH, N'D', N'Failed to identify the root cause of the crisis', 0);
END
GO

-- Câu hỏi SEED_EAI_475 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_475')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'TuVung', N'Kho', N'A diplomatic statement often requires readers to read between the _______ to perceive subtle underlying meanings.',
            NULL, NULL, N'''read between the lines'' (hiểu được ẩn ý ngầm đằng sau câu từ).', N'''read between the lines'' (hiểu được ẩn ý ngầm đằng sau câu từ).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Kỹ năng đọc hiểu C1: Reading between the lines.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_475');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'lines', 1),
        (@MaCH, N'B', N'words', 0),
        (@MaCH, N'C', N'pages', 0),
        (@MaCH, N'D', N'letters', 0);
END
GO

-- Câu hỏi SEED_EAI_476 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_476')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'TuVung', N'Kho', N'Artificial intelligence is often described as a ''double-edged sword'' because:',
            NULL, NULL, N'''A double-edged sword'' (con dao hai lưỡi) có nghĩa là một giải pháp hay công nghệ vừa mang lại lợi ích to lớn nhưng đồng thời cũng tiềm ẩn những rủi ro hoặc tác hại khôn lường.', N'''A double-edged sword'' (con dao hai lưỡi) có nghĩa là một giải pháp hay công nghệ vừa mang lại lợi ích to lớn nhưng đồng thời cũng tiềm ẩn những rủi ro hoặc tác hại khôn lường.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Đọc hiểu Nâng cao: Đột phá Trí tuệ Nhân tạo - Khái niệm Double-edged Sword trong công nghệ.', N'Extracted', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_476');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'It brings extraordinary economic benefits while simultaneously posing profound ethical and existential hazards', 1),
        (@MaCH, N'B', N'It is manufactured using sharp metallic semiconductors', 0),
        (@MaCH, N'C', N'It cannot be utilized on modern mobile smartphones', 0),
        (@MaCH, N'D', N'It is twice as cheap as conventional software systems', 0);
END
GO

-- Câu hỏi SEED_EAI_477 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_477')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'TuVung', N'Kho', N'The initial detection of accounting discrepancies was merely the tip of the _______; billions in unrecorded debt remained concealed.',
            NULL, NULL, N'''the tip of the iceberg'' (chỉ là bề nổi của tảng băng chìm).', N'''the tip of the iceberg'' (chỉ là bề nổi của tảng băng chìm).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Thành ngữ kinh doanh C1: The tip of the iceberg.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_477');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'iceberg', 1),
        (@MaCH, N'B', N'mountain', 0),
        (@MaCH, N'C', N'volcano', 0),
        (@MaCH, N'D', N'pyramid', 0);
END
GO

-- Câu hỏi SEED_EAI_478 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_478')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'TuVung', N'Kho', N'The detective vowed to ''leave no stone unturned'' in the search for physical evidence. This signifies:',
            NULL, NULL, N'''Leave no stone unturned'' có nghĩa là làm tất cả mọi cách, rà soát mọi khả năng có thể để đạt được mục tiêu.', N'''Leave no stone unturned'' có nghĩa là làm tất cả mọi cách, rà soát mọi khả năng có thể để đạt được mục tiêu.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Thành ngữ C1 diễn tả sự nỗ lực toàn diện triệt để.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_478');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Trying every possible course of action or looking everywhere meticulously', 1),
        (@MaCH, N'B', N'Collecting rare geological rocks from caves', 0),
        (@MaCH, N'C', N'Renovating residential gardens using stones', 0),
        (@MaCH, N'D', N'Ignoring the crime scene completely', 0);
END
GO

-- Câu hỏi SEED_EAI_479 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_479')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'TuVung', N'Kho', N'Which advanced C1 collocation correctly completes: ''The government''s new fiscal austerity policy _______ far short of the public''s expectations.''?',
            NULL, NULL, N'''fell short of expectations'' (không đạt được như kỳ vọng mong đợi).', N'''fell short of expectations'' (không đạt được như kỳ vọng mong đợi).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - C1 Collocations: ''Fall short of expectations''.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_479');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'dropped', 0),
        (@MaCH, N'B', N'fell', 1),
        (@MaCH, N'C', N'cut', 0),
        (@MaCH, N'D', N'sunk', 0);
END
GO

-- Câu hỏi SEED_EAI_480 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_480')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'TuVung', N'Kho', N'When evaluating research integrity, one must bear in _______ that correlation does not imply causation.',
            NULL, NULL, N'''bear in mind'' (ghi nhớ, khắc sâu trong tâm trí).', N'''bear in mind'' (ghi nhớ, khắc sâu trong tâm trí).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Cụm từ học thuật C1: ''Bear in mind''.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_480');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'mind', 1),
        (@MaCH, N'B', N'head', 0),
        (@MaCH, N'C', N'heart', 0),
        (@MaCH, N'D', N'brain', 0);
END
GO

-- Câu hỏi SEED_EAI_481 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_481')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'TuVung', N'Kho', N'In clinical trials, patient safety must take _______ over commercial profit deadlines.',
            NULL, NULL, N'''take precedence over'' (được ưu tiên hàng đầu, xếp trên những thứ khác).', N'''take precedence over'' (được ưu tiên hàng đầu, xếp trên những thứ khác).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - C1 Formal Collocations: ''Take precedence over''.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_481');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'precedence', 1),
        (@MaCH, N'B', N'preference', 0),
        (@MaCH, N'C', N'presence', 0),
        (@MaCH, N'D', N'presumption', 0);
END
GO

-- Câu hỏi SEED_EAI_482 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_482')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'TuVung', N'Kho', N'After years of theoretical research, their groundbreaking quantum computing prototypes finally came to _______.',
            NULL, NULL, N'''came to fruition'' (đạt được thành quả thực tế, kết trái ngọt sau bao nỗ lực).', N'''came to fruition'' (đạt được thành quả thực tế, kết trái ngọt sau bao nỗ lực).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Cụm từ C1: ''Come to fruition''.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_482');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'fruition', 1),
        (@MaCH, N'B', N'fruit', 0),
        (@MaCH, N'C', N'fulfillment', 0),
        (@MaCH, N'D', N'fortune', 0);
END
GO

-- Câu hỏi SEED_EAI_483 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_483')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'TuVung', N'Kho', N'Which idiomatic phrase means to reveal a secret prematurely, often inadvertently ruining a surprise?',
            NULL, NULL, N'''Spill the beans'' nghĩa là vô tình hay thiếu thận trọng tiết lộ bí mật.', N'''Spill the beans'' nghĩa là vô tình hay thiếu thận trọng tiết lộ bí mật.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - C1 Idioms: ''Spill the beans''.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_483');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Spill the beans', 1),
        (@MaCH, N'B', N'Bite the bullet', 0),
        (@MaCH, N'C', N'Burn the candle', 0),
        (@MaCH, N'D', N'Break the ice', 0);
END
GO

-- Câu hỏi SEED_EAI_484 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_484')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'TuVung', N'Kho', N'Despite intense pressure from investors, the visionary founder refused to throw in the _______ and continued developing the autonomous rover.',
            NULL, NULL, N'''throw in the towel'' (bỏ cuộc, đầu hàng).', N'''throw in the towel'' (bỏ cuộc, đầu hàng).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Thành ngữ ''Throw in the towel''.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_484');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'towel', 1),
        (@MaCH, N'B', N'sponge', 0),
        (@MaCH, N'C', N'sheet', 0),
        (@MaCH, N'D', N'blanket', 0);
END
GO

-- Câu hỏi SEED_EAI_485 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_485')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'TuVung', N'Kho', N'When two negotiating delegations do not ''see eye to eye'' on carbon tariffs, they:',
            NULL, NULL, N'''See eye to eye'' có nghĩa là đồng thuận quan điểm; ''not see eye to eye'' là bất đồng ý kiến.', N'''See eye to eye'' có nghĩa là đồng thuận quan điểm; ''not see eye to eye'' là bất đồng ý kiến.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Thành ngữ đàm phán: ''See eye to eye''.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_485');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Disagree with each other regarding the proposed terms', 1),
        (@MaCH, N'B', N'Wear protective eyewear during diplomatic talks', 0),
        (@MaCH, N'C', N'Refuse to look at the contract papers', 0),
        (@MaCH, N'D', N'Immediately sign the treaty without reading', 0);
END
GO

-- Câu hỏi SEED_EAI_486 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_486')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'Doc', N'Kho', N'According to the passage, what unintended consequence of generative AI usage has emerged among university students?',
            N'The ascendancy of artificial general intelligence (AGI) represents a profound paradigm shift in human history. While proponents accentuate unprecedented productivity leaps in scientific discovery and biomedical automation, skeptics articulate legitimate apprehensions regarding cognitive atrophy and algorithmic bias. Empirical studies indicate that over-reliance on generative models diminishes critical inquiry and independent problem-solving among university cohorts. Furthermore, socio-economic stratification risks exacerbation as capital accrues disproportionately to technology conglomerates orchestrating proprietary foundational models. Consequently, international regulatory bodies advocate for stringent algorithmic transparency and human-in-the-loop oversight before automated decision systems govern criminal justice, healthcare triage, or financial credit scoring.', NULL, N'Nghiên cứu thực nghiệm chỉ ra sự phụ thuộc quá mức vào AI tạo sinh làm suy giảm khả năng tư duy phản biện và giải quyết vấn đề độc lập của sinh viên (''diminishes critical inquiry and independent problem-solving'').', N'Nghiên cứu thực nghiệm chỉ ra sự phụ thuộc quá mức vào AI tạo sinh làm suy giảm khả năng tư duy phản biện và giải quyết vấn đề độc lập của sinh viên (''diminishes critical inquiry and independent problem-solving'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Đọc hiểu Nâng cao: Đột phá Trí tuệ Nhân tạo & Tác động Xã hội - Đoạn 1: Tác động của AI đối với nhận thức người học.', N'Extracted', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_486');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Complete abandonment of digital electronic hardware', 0),
        (@MaCH, N'B', N'Reduction in critical inquiry and autonomous problem-solving capabilities', 1),
        (@MaCH, N'C', N'Substantial increase in handwriting speed', 0),
        (@MaCH, N'D', N'Universal mastery of multi-lingual programming languages', 0);
END
GO

-- Câu hỏi SEED_EAI_487 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_487')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'Doc', N'Kho', N'The term ''ascendancy'' in the first sentence is closest in meaning to:',
            N'The ascendancy of artificial general intelligence (AGI) represents a profound paradigm shift in human history. While proponents accentuate unprecedented productivity leaps in scientific discovery and biomedical automation, skeptics articulate legitimate apprehensions regarding cognitive atrophy and algorithmic bias. Empirical studies indicate that over-reliance on generative models diminishes critical inquiry and independent problem-solving among university cohorts. Furthermore, socio-economic stratification risks exacerbation as capital accrues disproportionately to technology conglomerates orchestrating proprietary foundational models. Consequently, international regulatory bodies advocate for stringent algorithmic transparency and human-in-the-loop oversight before automated decision systems govern criminal justice, healthcare triage, or financial credit scoring.', NULL, N'''Ascendancy'' mang nghĩa sự trỗi dậy, vị thế thống trị hoặc vươn lên dẫn đầu (''dominance'' / ''rise to power'').', N'''Ascendancy'' mang nghĩa sự trỗi dậy, vị thế thống trị hoặc vươn lên dẫn đầu (''dominance'' / ''rise to power'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Phân tích từ vựng học thuật C1 trong văn cảnh công nghệ.', N'Extracted', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_487');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Decline', 0),
        (@MaCH, N'B', N'Dominance', 1),
        (@MaCH, N'C', N'Confusion', 0),
        (@MaCH, N'D', N'Stagnation', 0);
END
GO

-- Câu hỏi SEED_EAI_488 (Doc - Trình độ 5 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_488')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DungSai', N'Doc', N'Kho', N'True or False: The author argues that automated decision systems should govern criminal justice without any human intervention.',
            N'The ascendancy of artificial general intelligence (AGI) represents a profound paradigm shift in human history. While proponents accentuate unprecedented productivity leaps in scientific discovery and biomedical automation, skeptics articulate legitimate apprehensions regarding cognitive atrophy and algorithmic bias. Empirical studies indicate that over-reliance on generative models diminishes critical inquiry and independent problem-solving among university cohorts. Furthermore, socio-economic stratification risks exacerbation as capital accrues disproportionately to technology conglomerates orchestrating proprietary foundational models. Consequently, international regulatory bodies advocate for stringent algorithmic transparency and human-in-the-loop oversight before automated decision systems govern criminal justice, healthcare triage, or financial credit scoring.', NULL, N'Sai (False). Tác giả nêu rõ các cơ quan quản lý quốc tế yêu cầu phải có sự giám sát của con người (''human-in-the-loop oversight'') trước khi AI được ứng dụng trong tư pháp hình sự.', N'Sai (False). Tác giả nêu rõ các cơ quan quản lý quốc tế yêu cầu phải có sự giám sát của con người (''human-in-the-loop oversight'') trước khi AI được ứng dụng trong tư pháp hình sự.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Vai trò của con người trong chu trình AI (Human-in-the-loop).', N'Extracted', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_488');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_489 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_489')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'Doc', N'Kho', N'What potential societal risk is highlighted regarding tech conglomerates in the text?',
            N'The ascendancy of artificial general intelligence (AGI) represents a profound paradigm shift in human history. While proponents accentuate unprecedented productivity leaps in scientific discovery and biomedical automation, skeptics articulate legitimate apprehensions regarding cognitive atrophy and algorithmic bias. Empirical studies indicate that over-reliance on generative models diminishes critical inquiry and independent problem-solving among university cohorts. Furthermore, socio-economic stratification risks exacerbation as capital accrues disproportionately to technology conglomerates orchestrating proprietary foundational models. Consequently, international regulatory bodies advocate for stringent algorithmic transparency and human-in-the-loop oversight before automated decision systems govern criminal justice, healthcare triage, or financial credit scoring.', NULL, N'Sự bất bình đẳng kinh tế - xã hội có nguy cơ trầm trọng hơn do lợi nhuận và tư bản dồn phần lớn vào các tập đoàn công nghệ nắm giữ mô hình nền tảng.', N'Sự bất bình đẳng kinh tế - xã hội có nguy cơ trầm trọng hơn do lợi nhuận và tư bản dồn phần lớn vào các tập đoàn công nghệ nắm giữ mô hình nền tảng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Phân tầng kinh tế xã hội và độc quyền công nghệ.', N'Extracted', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_489');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Deepening socio-economic stratification due to monopolized capital concentration', 1),
        (@MaCH, N'B', N'Imminent bankruptcy of all major software corporations', 0),
        (@MaCH, N'C', N'An oversupply of hardware microchips worldwide', 0),
        (@MaCH, N'D', N'Complete disappearance of venture capital investments', 0);
END
GO

-- Câu hỏi SEED_EAI_490 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_490')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'TuVung', N'Kho', N'Fill in the blank: ''Regulatory bodies mandate that algorithmic _______ must be demonstrated so that stakeholders understand why automated models make specific decisions.''',
            N'The ascendancy of artificial general intelligence (AGI) represents a profound paradigm shift in human history. While proponents accentuate unprecedented productivity leaps in scientific discovery and biomedical automation, skeptics articulate legitimate apprehensions regarding cognitive atrophy and algorithmic bias. Empirical studies indicate that over-reliance on generative models diminishes critical inquiry and independent problem-solving among university cohorts. Furthermore, socio-economic stratification risks exacerbation as capital accrues disproportionately to technology conglomerates orchestrating proprietary foundational models. Consequently, international regulatory bodies advocate for stringent algorithmic transparency and human-in-the-loop oversight before automated decision systems govern criminal justice, healthcare triage, or financial credit scoring.', NULL, N'''transparency'' (tính minh bạch của thuật toán).', N'''transparency'' (tính minh bạch của thuật toán).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Nguyên tắc minh bạch thuật toán (Algorithmic Transparency).', N'Extracted', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_490');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'transparency', 1),
        (@MaCH, N'B', N'opacity', 0),
        (@MaCH, N'C', N'secrecy', 0),
        (@MaCH, N'D', N'ambiguity', 0);
END
GO

-- Câu hỏi SEED_EAI_491 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_491')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'Doc', N'Kho', N'What is the primary tone of the passage regarding artificial intelligence?',
            N'The ascendancy of artificial general intelligence (AGI) represents a profound paradigm shift in human history. While proponents accentuate unprecedented productivity leaps in scientific discovery and biomedical automation, skeptics articulate legitimate apprehensions regarding cognitive atrophy and algorithmic bias. Empirical studies indicate that over-reliance on generative models diminishes critical inquiry and independent problem-solving among university cohorts. Furthermore, socio-economic stratification risks exacerbation as capital accrues disproportionately to technology conglomerates orchestrating proprietary foundational models. Consequently, international regulatory bodies advocate for stringent algorithmic transparency and human-in-the-loop oversight before automated decision systems govern criminal justice, healthcare triage, or financial credit scoring.', NULL, N'Giọng văn mang tính phân tích, cân bằng và cảnh giác thận trọng (Analytic, measured, and cautiously critical).', N'Giọng văn mang tính phân tích, cân bằng và cảnh giác thận trọng (Analytic, measured, and cautiously critical).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Nhận diện thái độ và giọng điệu tác giả (Author''s Tone and Stance).', N'Extracted', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_491');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Analytic, objective, and cautiously critical', 1),
        (@MaCH, N'B', N'Blindly optimistic and dismissive of hazards', 0),
        (@MaCH, N'C', N'Purely satirical and mocking', 0),
        (@MaCH, N'D', N'Hopeless, apocalyptic, and fatalistic', 0);
END
GO

-- Câu hỏi SEED_EAI_492 (NguPhap - Trình độ 5 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_492')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'VietLaiCau', N'NguPhap', N'Kho', N'Transform into the subjunctive mood: ''It is essential that governments regulate artificial intelligence deployment.''',
            NULL, NULL, N'Thức giả định hiện tại (Present Subjunctive) dùng động từ nguyên mẫu không ''to'': ''It is essential that governments regulate artificial intelligence deployment.''', N'Thức giả định hiện tại (Present Subjunctive) dùng động từ nguyên mẫu không ''to'': ''It is essential that governments regulate artificial intelligence deployment.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Cấu trúc thức giả định Subjunctive Mood trong văn bản quy phạm.', N'Applied', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_492');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'It is essential that governments regulate artificial intelligence deployment.', 1),
        (@MaCH, N'B', N'It is essential that governments regulates AI deployment.', 0),
        (@MaCH, N'C', N'It is essential that governments will regulate AI deployment.', 0),
        (@MaCH, N'D', N'It is essential for governments to having regulated AI.', 0);
END
GO

-- Câu hỏi SEED_EAI_493 (NguPhap - Trình độ 5 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_493')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'SuaLoiSai', N'NguPhap', N'Kho', N'Identify the grammatical flaw: ''The ethics committee insisted that every AI engineer adheres (A) strictly to confidentiality protocols and submits (B) algorithmic models for peer review (C) prior to deployment (D).''',
            NULL, NULL, N'Lỗi ở (A) ''adheres'' và (B) ''submits''. Sau cấu trúc ''insisted that...'', mệnh đề phải dùng động từ nguyên mẫu theo thức giả định: ''adhere'' và ''submit''.', N'Lỗi ở (A) ''adheres'' và (B) ''submits''. Sau cấu trúc ''insisted that...'', mệnh đề phải dùng động từ nguyên mẫu theo thức giả định: ''adhere'' và ''submit''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Thức giả định Subjunctive Mood sau insist/recommend.', N'Applied', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_493');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'adheres', 1),
        (@MaCH, N'B', N'submits', 0),
        (@MaCH, N'C', N'for peer review', 0),
        (@MaCH, N'D', N'prior to deployment', 0);
END
GO

-- Câu hỏi SEED_EAI_494 (NguPhap - Trình độ 5 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_494')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'VietLaiCau', N'NguPhap', N'Kho', N'Combine using a non-defining relative clause with preposition: ''The European Union enacted the AI Act. Under this legislation, high-risk biometric surveillance systems are strictly prohibited.''',
            NULL, NULL, N'''The European Union enacted the AI Act, under which high-risk biometric surveillance systems are strictly prohibited.''', N'''The European Union enacted the AI Act, under which high-risk biometric surveillance systems are strictly prohibited.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mệnh đề quan hệ chứa giới từ (Preposition + Which).', N'Applied', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_494');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The European Union enacted the AI Act, under which high-risk biometric surveillance systems are strictly prohibited.', 1),
        (@MaCH, N'B', N'The European Union enacted the AI Act under that high-risk systems are prohibited.', 0),
        (@MaCH, N'C', N'The European Union enacted the AI Act which under high-risk surveillance is prohibited.', 0),
        (@MaCH, N'D', N'The European Union enacted the AI Act, whereunder systems was prohibited.', 0);
END
GO

-- Câu hỏi SEED_EAI_495 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_495')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'TuVung', N'Kho', N'When evaluating model robustness, adversarial testing seeks to identify systemic _______ in training datasets.',
            NULL, NULL, N'''biases'' (thiên kiến, định kiến hệ thống).', N'''biases'' (thiên kiến, định kiến hệ thống).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Thiên kiến thuật toán và kiểm thử đối kháng (Algorithmic Bias).', N'Extracted', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_495');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'biases', 1),
        (@MaCH, N'B', N'wires', 0),
        (@MaCH, N'C', N'batteries', 0),
        (@MaCH, N'D', N'keyboards', 0);
END
GO

-- Câu hỏi SEED_EAI_496 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_496')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'Doc', N'Kho', N'In cognitive psychology, ''cognitive atrophy'' refers to:',
            NULL, NULL, N'''Cognitive atrophy'' là hiện tượng thoái hóa chức năng tư duy, ghi nhớ hoặc kỹ năng nhận thức do lâu ngày không được vận dụng não bộ độc lập.', N'''Cognitive atrophy'' là hiện tượng thoái hóa chức năng tư duy, ghi nhớ hoặc kỹ năng nhận thức do lâu ngày không được vận dụng não bộ độc lập.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Thuật ngữ tâm lý nhận thức và tác động của tự động hóa lên não bộ.', N'Extracted', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_496');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The gradual deterioration of intellectual faculties and problem-solving skills from lack of mental exercise', 1),
        (@MaCH, N'B', N'A physical headache caused by staring at phone displays', 0),
        (@MaCH, N'C', N'An immediate increase in mathematical computation speed', 0),
        (@MaCH, N'D', N'The complete recovery of lost memory cells', 0);
END
GO

-- Câu hỏi SEED_EAI_497 (Viet - Trình độ 5 - Dạng TuLuan - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_497')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TuLuan', N'Viet', N'Kho', N'Write a concise argumentative paragraph (80-120 words) analyzing whether generative AI should be permitted in academic university examinations.',
            NULL, NULL, N'Yêu cầu: Nêu rõ lập trường (đồng ý, phản đối hoặc kết hợp có kiểm soát), đưa ra ít nhất 2 luận cứ xác đáng (tính liêm chính học thuật vs chuẩn bị kỹ năng thực tế cho thị trường lao động), sử dụng liên từ mạch lạc và vốn từ vựng C1 chuẩn xác.', N'Yêu cầu: Nêu rõ lập trường (đồng ý, phản đối hoặc kết hợp có kiểm soát), đưa ra ít nhất 2 luận cứ xác đáng (tính liêm chính học thuật vs chuẩn bị kỹ năng thực tế cho thị trường lao động), sử dụng liên từ mạch lạc và vốn từ vựng C1 chuẩn xác.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 37: Chủ đề thảo luận nghị luận xã hội: Liêm chính học thuật trong thời đại AI.', N'Applied', N'BaiHoc_37_Doc_hieu_Nang_cao_AI.md', NULL, 'SEED_EAI_497');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Model Rubric: Clear thesis statement, academic vocabulary (integrity, cognitive engagement, pedagogical efficacy), coherent discourse markers, balanced synthesis.', 1);
END
GO

-- Câu hỏi SEED_EAI_498 (NguPhap - Trình độ 5 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_498')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'VietLaiCau', N'NguPhap', N'Kho', N'Rewrite using ''No matter how'': ''Although the regulatory guidelines are intricate, developers must adhere to compliance standards.''',
            NULL, NULL, N'''No matter how intricate the regulatory guidelines are, developers must adhere to compliance standards.''', N'''No matter how intricate the regulatory guidelines are, developers must adhere to compliance standards.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Cấu trúc nhượng bộ: No matter how + Adj + S + V.', N'Applied', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_498');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'No matter how intricate the regulatory guidelines are, developers must adhere to compliance standards.', 1),
        (@MaCH, N'B', N'No matter what intricate guidelines are, developers must adhere.', 0),
        (@MaCH, N'C', N'No matter how the regulatory guidelines are intricate, developers adhere.', 0),
        (@MaCH, N'D', N'No matter intricate guidelines, developers must adhere.', 0);
END
GO

-- Câu hỏi SEED_EAI_499 (NguPhap - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_499')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'DienKhuyet', N'NguPhap', N'Kho', N'Not for one moment _______ the researcher anticipate that her open-source library would power global autonomous vehicle fleets.',
            NULL, NULL, N'''did'' (đảo trợ động từ quá khứ đơn sau cụm phủ định: ''Not for one moment did the researcher anticipate...'').', N'''did'' (đảo trợ động từ quá khứ đơn sau cụm phủ định: ''Not for one moment did the researcher anticipate...'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ với ''Not for one moment''.', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_499');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'did', 1),
        (@MaCH, N'B', N'was', 0),
        (@MaCH, N'C', N'had', 0),
        (@MaCH, N'D', N'does', 0);
END
GO

-- Câu hỏi SEED_EAI_500 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_500')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (19, 5, N'TracNghiem', N'NguPhap', N'Kho', N'Which sentence achieves the most formal, syntactically sophisticated conclusion for a C1 academic essay?',
            NULL, NULL, N'''Only through rigorous empirical inquiry and transparent international governance can humanity harness artificial intelligence ethically and sustainably.''', N'''Only through rigorous empirical inquiry and transparent international governance can humanity harness artificial intelligence ethically and sustainably.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Tổng kết cấu trúc câu đảo ngữ đỉnh cao C1 (Climax Inversion).', N'Extracted', N'17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx', NULL, 'SEED_EAI_500');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Only through rigorous empirical inquiry and transparent international governance can humanity harness artificial intelligence ethically and sustainably.', 1),
        (@MaCH, N'B', N'In conclusion, AI is very nice and we hope humans will use it nicely without bad things.', 0),
        (@MaCH, N'C', N'Humanity can harness AI if they do good governance and do not be greedy.', 0),
        (@MaCH, N'D', N'That is why AI will change everything tomorrow.', 0);
END
GO

PRINT N'Đã hoàn thành nạp Batch 10 (SEED_EAI_451 -> SEED_EAI_500) thành công!';
GO
