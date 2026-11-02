-- ====================================================================================
-- SEED QUESTIONS BATCH: Batch 05 (SEED_EAI_201 -> SEED_EAI_250)
-- Số lượng câu hỏi: 50
-- Đảm bảo: Idempotent (Chạy lại nhiều lần không trùng lặp)
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Đang nạp Batch 05 (SEED_EAI_201 -> SEED_EAI_250) (50 câu hỏi)...';

-- Câu hỏi SEED_EAI_201 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_201')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct sentence: ''I _______ in this tech startup since it was founded in 2021.''',
            NULL, NULL, N'Hành động bắt đầu trong quá khứ kéo dài liên tục đến hiện tại với dấu hiệu ''since + mốc thời gian'' phải dùng thì Hiện tại hoàn thành: ''have worked'' hoặc ''have been working''.', N'Hành động bắt đầu trong quá khứ kéo dài liên tục đến hiện tại với dấu hiệu ''since + mốc thời gian'' phải dùng thì Hiện tại hoàn thành: ''have worked'' hoặc ''have been working''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 14 - Cấu trúc thì Hiện tại hoàn thành với Since và For: S + have/has + V3/ed + since + mốc thời gian.', N'Extracted', N'BaiHoc_14_PresentPerfect.txt', 14, 'SEED_EAI_201');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'worked', 0),
        (@MaCH, N'B', N'have worked', 1),
        (@MaCH, N'C', N'had worked', 0),
        (@MaCH, N'D', N'am working', 0);
END
GO

-- Câu hỏi SEED_EAI_202 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_202')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'TracNghiem', N'NguPhap', N'De', N'Fill in the blank with the appropriate adverb: ''Have you _______ visited the software research lab in Silicon Valley?''',
            NULL, NULL, N'Trong câu hỏi trải nghiệm của thì Hiện tại hoàn thành, dùng ''ever'' (đã từng bao giờ chưa).', N'Trong câu hỏi trải nghiệm của thì Hiện tại hoàn thành, dùng ''ever'' (đã từng bao giờ chưa).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 14 - Phó từ trong câu nghi vấn Hiện tại hoàn thành: ''Have you ever + V3...?''.', N'Extracted', N'BaiHoc_14_PresentPerfect.txt', 14, 'SEED_EAI_202');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'never', 0),
        (@MaCH, N'B', N'ever', 1),
        (@MaCH, N'C', N'already', 0),
        (@MaCH, N'D', N'yet', 0);
END
GO

-- Câu hỏi SEED_EAI_203 (NguPhap - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_203')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'Complete the sentence with ''already'' or ''yet'': ''We have _______ deployed the latest server update; our systems are fully operational now.''',
            NULL, NULL, N'''Already'' dùng trong câu khẳng định chỉ hành động đã hoàn tất sớm hơn dự kiến.', N'''Already'' dùng trong câu khẳng định chỉ hành động đã hoàn tất sớm hơn dự kiến.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 14 - Phân biệt Already (câu khẳng định) vs. Yet (cuối câu phủ định và nghi vấn).', N'Extracted', N'BaiHoc_14_PresentPerfect.txt', 14, 'SEED_EAI_203');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'already', 1),
        (@MaCH, N'B', N'yet', 0),
        (@MaCH, N'C', N'still', 0),
        (@MaCH, N'D', N'ever', 0);
END
GO

-- Câu hỏi SEED_EAI_204 (NguPhap - Trình độ 3 - Dạng SuaLoiSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_204')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Identify the error: ''The development team has deployed the v2.0 release yesterday morning.''',
            NULL, NULL, N'Khi có trạng từ chỉ thời gian quá khứ xác định cụ thể (''yesterday morning''), không được dùng Hiện tại hoàn thành mà phải dùng Quá khứ đơn: ''deployed''.', N'Khi có trạng từ chỉ thời gian quá khứ xác định cụ thể (''yesterday morning''), không được dùng Hiện tại hoàn thành mà phải dùng Quá khứ đơn: ''deployed''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 14 - Lỗi sai phổ biến: Nhầm lẫn giữa Past Simple (thời điểm xác định) và Present Perfect (không nêu thời điểm).', N'Extracted', N'BaiHoc_14_PresentPerfect.txt', 14, 'SEED_EAI_204');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''has deployed'' should be ''deployed''', 1),
        (@MaCH, N'B', N'''yesterday'' should be ''tomorrow''', 0),
        (@MaCH, N'C', N'''release'' should be ''releasing''', 0),
        (@MaCH, N'D', N'''The'' should be ''A''', 0);
END
GO

-- Câu hỏi SEED_EAI_205 (Viet - Trình độ 3 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_205')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'VietLaiCau', N'Viet', N'TrungBinh', N'Rewrite using Present Perfect: ''The last time I talked to Professor Miller was two months ago.''',
            NULL, NULL, N'''I have not talked to Professor Miller for two months.'' là dạng biến đổi câu chuẩn giữa Quá khứ đơn và Hiện tại hoàn thành.', N'''I have not talked to Professor Miller for two months.'' là dạng biến đổi câu chuẩn giữa Quá khứ đơn và Hiện tại hoàn thành.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 14 - Chuyển đổi câu tương đương: The last time S + V2 was [time] ago -> S + have not + V3 for [time].', N'Applied', N'BaiHoc_14_PresentPerfect.txt', 14, 'SEED_EAI_205');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I have not talked to Professor Miller for two months.', 1),
        (@MaCH, N'B', N'I talked to Professor Miller since two months.', 0),
        (@MaCH, N'C', N'I have talked to Professor Miller two months ago.', 0),
        (@MaCH, N'D', N'I had not been talking to Professor Miller.', 0);
END
GO

-- Câu hỏi SEED_EAI_206 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_206')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct sentence with non-defining relative clause punctuation:',
            NULL, NULL, N'Mệnh đề quan hệ không xác định (Non-defining) bắt buộc phải có dấu phẩy ngăn cách và tuyệt đối không dùng đại từ ''that'': ''Dr. Nguyen, who pioneered AI research in Vietnam, was invited to speak.''', N'Mệnh đề quan hệ không xác định (Non-defining) bắt buộc phải có dấu phẩy ngăn cách và tuyệt đối không dùng đại từ ''that'': ''Dr. Nguyen, who pioneered AI research in Vietnam, was invited to speak.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 17 & 36 - Mệnh đề quan hệ không xác định: Có dấu phẩy, cung cấp thông tin phụ, không dùng ''that''.', N'Extracted', N'BaiHoc_17_RelativeClauses.txt', 17, 'SEED_EAI_206');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Dr. Nguyen who pioneered AI research in Vietnam, was invited.', 0),
        (@MaCH, N'B', N'Dr. Nguyen, that pioneered AI research in Vietnam, was invited.', 0),
        (@MaCH, N'C', N'Dr. Nguyen, who pioneered AI research in Vietnam, was invited to speak.', 1),
        (@MaCH, N'D', N'Dr. Nguyen which pioneered AI research, was invited.', 0);
END
GO

-- Câu hỏi SEED_EAI_207 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_207')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Select the correct relative pronoun showing possession: ''The engineer _______ laptop was stolen filed a police report.''',
            NULL, NULL, N'Đại từ chỉ sở hữu cho người hoặc vật trong mệnh đề quan hệ là ''whose'': ''The engineer whose laptop...''.', N'Đại từ chỉ sở hữu cho người hoặc vật trong mệnh đề quan hệ là ''whose'': ''The engineer whose laptop...''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 17 - Đại từ quan hệ chỉ sở hữu: Whose + Noun.', N'Extracted', N'BaiHoc_17_RelativeClauses.txt', 17, 'SEED_EAI_207');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'who', 0),
        (@MaCH, N'B', N'whose', 1),
        (@MaCH, N'C', N'whom', 0),
        (@MaCH, N'D', N'which', 0);
END
GO

-- Câu hỏi SEED_EAI_208 (Viet - Trình độ 4 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_208')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 4, N'VietLaiCau', N'Viet', N'Kho', N'Reduce the relative clause into a present participle phrase: ''Software engineers who specialize in cybersecurity earn high salaries.''',
            NULL, NULL, N'Rút gọn mệnh đề quan hệ chủ động bằng V-ing: ''Software engineers specializing in cybersecurity earn high salaries.''', N'Rút gọn mệnh đề quan hệ chủ động bằng V-ing: ''Software engineers specializing in cybersecurity earn high salaries.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 17 & 36 - Rút gọn mệnh đề quan hệ chủ động: V-ing.', N'Extracted', N'BaiHoc_17_RelativeClauses.txt', 17, 'SEED_EAI_208');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Software engineers specializing in cybersecurity earn high salaries.', 1),
        (@MaCH, N'B', N'Software engineers specialized in cybersecurity earn high salaries.', 0),
        (@MaCH, N'C', N'Software engineers to specialize in cybersecurity earn.', 0),
        (@MaCH, N'D', N'Specializing software engineers earn high salaries.', 0);
END
GO

-- Câu hỏi SEED_EAI_209 (Viet - Trình độ 4 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_209')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 4, N'VietLaiCau', N'Viet', N'Kho', N'Reduce the relative clause into a past participle phrase: ''The research paper which was published by MIT researchers attracted global attention.''',
            NULL, NULL, N'Rút gọn mệnh đề quan hệ bị động bằng V3/ed: ''The research paper published by MIT researchers attracted global attention.''', N'Rút gọn mệnh đề quan hệ bị động bằng V3/ed: ''The research paper published by MIT researchers attracted global attention.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 17 & 36 - Rút gọn mệnh đề quan hệ bị động: V3/ed.', N'Extracted', N'BaiHoc_17_RelativeClauses.txt', 17, 'SEED_EAI_209');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The research paper publishing by MIT researchers attracted attention.', 0),
        (@MaCH, N'B', N'The research paper published by MIT researchers attracted global attention.', 1),
        (@MaCH, N'C', N'The research paper having publish by MIT attracted.', 0),
        (@MaCH, N'D', N'Published research paper attracted global attention.', 0);
END
GO

-- Câu hỏi SEED_EAI_210 (NguPhap - Trình độ 3 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_210')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 3, N'DungSai', N'NguPhap', N'De', N'True or False: In formal English, prepositions can precede relative pronouns (e.g., ''the company for which I work'').',
            NULL, NULL, N'Đúng. Trong văn phong trang trọng, giới từ có thể đứng trước ''whom'' hoặc ''which'' (for which, in which, to whom).', N'Đúng. Trong văn phong trang trọng, giới từ có thể đứng trước ''whom'' hoặc ''which'' (for which, in which, to whom).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 17 - Giới từ trong mệnh đề quan hệ trang trọng: Preposition + whom / which.', N'Extracted', N'BaiHoc_17_RelativeClauses.txt', 17, 'SEED_EAI_210');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_211 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_211')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct causative passive structure: ''I need to have my air conditioner _______ before summer begins.''',
            NULL, NULL, N'Cấu trúc truyền khiến bị động: ''have / get something + V3/ed'' (nhờ/thuê ai làm việc gì cho mình). Ở đây dùng ''have my air conditioner serviced''.', N'Cấu trúc truyền khiến bị động: ''have / get something + V3/ed'' (nhờ/thuê ai làm việc gì cho mình). Ở đây dùng ''have my air conditioner serviced''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 18 - Thể truyền khiến bị động: S + have / get + O(vật) + V3/ed.', N'Extracted', N'BaiHoc_18_ReportedSpeechAndCausatives.txt', 18, 'SEED_EAI_211');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'service', 0),
        (@MaCH, N'B', N'servicing', 0),
        (@MaCH, N'C', N'serviced', 1),
        (@MaCH, N'D', N'to service', 0);
END
GO

-- Câu hỏi SEED_EAI_212 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_212')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct causative active structure: ''The project manager got the lead developer _______ the legacy database.''',
            NULL, NULL, N'Cấu trúc truyền khiến chủ động với ''get'': ''get someone TO DO something''. (Khác với ''have someone DO something'').', N'Cấu trúc truyền khiến chủ động với ''get'': ''get someone TO DO something''. (Khác với ''have someone DO something'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 18 - Phân biệt thể truyền khiến chủ động: Have someone DO sth vs. Get someone TO DO sth.', N'Extracted', N'BaiHoc_18_ReportedSpeechAndCausatives.txt', 18, 'SEED_EAI_212');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'migrate', 0),
        (@MaCH, N'B', N'to migrate', 1),
        (@MaCH, N'C', N'migrated', 0),
        (@MaCH, N'D', N'migrating', 0);
END
GO

-- Câu hỏi SEED_EAI_213 (NguPhap - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_213')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 4, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'Complete the causative sentence: ''She had her assistant _______ the flight tickets to Tokyo.''',
            NULL, NULL, N'''Have someone do something'' dùng động từ nguyên thể không ''to'': ''book''.', N'''Have someone do something'' dùng động từ nguyên thể không ''to'': ''book''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 18 - Cấu trúc: S + have + O(người) + V-bare.', N'Extracted', N'BaiHoc_18_ReportedSpeechAndCausatives.txt', 18, 'SEED_EAI_213');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'book', 1),
        (@MaCH, N'B', N'to book', 0),
        (@MaCH, N'C', N'booked', 0),
        (@MaCH, N'D', N'booking', 0);
END
GO

-- Câu hỏi SEED_EAI_214 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_214')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Which reporting verb requires the pattern ''verb + object + to-infinitive''?',
            NULL, NULL, N'Động từ ''encourage'' đi với tân ngữ + to-V: ''The mentor encouraged him to apply for the scholarship.''', N'Động từ ''encourage'' đi với tân ngữ + to-V: ''The mentor encouraged him to apply for the scholarship.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 18 & 31 - Động từ tường thuật chỉ dẫn/khuyên bảo: S + advise/encourage/remind + O + to-V.', N'Extracted', N'BaiHoc_18_ReportedSpeechAndCausatives.txt', 18, 'SEED_EAI_214');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'suggest', 0),
        (@MaCH, N'B', N'encourage', 1),
        (@MaCH, N'C', N'admit', 0),
        (@MaCH, N'D', N'deny', 0);
END
GO

-- Câu hỏi SEED_EAI_215 (Viet - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_215')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 4, N'VietLaiCau', N'Viet', N'TrungBinh', N'Transform into reported speech: ''"I will submit the assignment tomorrow," Linda told her professor.''',
            NULL, NULL, N'Lùi thì ''will'' thành ''would'', trạng từ ''tomorrow'' đổi thành ''the next day'' hoặc ''the following day'': ''Linda told her professor that she would submit the assignment the following day.''', N'Lùi thì ''will'' thành ''would'', trạng từ ''tomorrow'' đổi thành ''the next day'' hoặc ''the following day'': ''Linda told her professor that she would submit the assignment the following day.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 18 & 31 - Quy tắc lùi thì và chuyển đổi trạng từ thời gian trong câu gián tiếp.', N'Applied', N'BaiHoc_18_ReportedSpeechAndCausatives.txt', 18, 'SEED_EAI_215');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Linda told her professor that she would submit the assignment the following day.', 1),
        (@MaCH, N'B', N'Linda told her professor she will submit the assignment tomorrow.', 0),
        (@MaCH, N'C', N'Linda told her professor she had submitted the assignment yesterday.', 0),
        (@MaCH, N'D', N'Linda asked her professor if she would submit assignment.', 0);
END
GO

-- Câu hỏi SEED_EAI_216 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_216')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 3, N'TracNghiem', N'NguPhap', N'De', N'Which sentence represents a First Conditional (real possibility in the future)?',
            NULL, NULL, N'Câu điều kiện loại 1: ''If + S + V(hiện tại đơn), S + will + V(nguyên mẫu)''.', N'Câu điều kiện loại 1: ''If + S + V(hiện tại đơn), S + will + V(nguyên mẫu)''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 19 - Cấu trúc câu điều kiện loại 1 (First Conditional).', N'Extracted', N'BaiHoc_19_ConditionalsMixed.txt', 19, 'SEED_EAI_216');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'If it rains tomorrow, we will postpone the outdoor sports match.', 1),
        (@MaCH, N'B', N'If I had wings, I would fly across the ocean.', 0),
        (@MaCH, N'C', N'If you had studied harder, you would have passed.', 0),
        (@MaCH, N'D', N'If ice melts, it becomes water.', 0);
END
GO

-- Câu hỏi SEED_EAI_217 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_217')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct Second Conditional sentence (hypothetical present situation):',
            NULL, NULL, N'Câu điều kiện loại 2 giả định trái ngược hiện tại: ''If + S + V2/were, S + would + V''.', N'Câu điều kiện loại 2 giả định trái ngược hiện tại: ''If + S + V2/were, S + would + V''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 19 - Cấu trúc câu điều kiện loại 2 (Second Conditional): If I were you, I would...', N'Extracted', N'BaiHoc_19_ConditionalsMixed.txt', 19, 'SEED_EAI_217');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'If I won the lottery tomorrow, I buy a house.', 0),
        (@MaCH, N'B', N'If I were the CEO of that multinational company, I would implement a four-day workweek.', 1),
        (@MaCH, N'C', N'If I will be CEO, I would implement four-day workweek.', 0),
        (@MaCH, N'D', N'If I am CEO, I would have implemented.', 0);
END
GO

-- Câu hỏi SEED_EAI_218 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_218')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 4, N'TracNghiem', N'NguPhap', N'Kho', N'Select the correct Third Conditional sentence (regret about past unfulfilled event):',
            NULL, NULL, N'Câu điều kiện loại 3 giả định trái ngược quá khứ: ''If + S + had + V3, S + would have + V3''.', N'Câu điều kiện loại 3 giả định trái ngược quá khứ: ''If + S + had + V3, S + would have + V3''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 19 - Cấu trúc câu điều kiện loại 3 (Third Conditional).', N'Extracted', N'BaiHoc_19_ConditionalsMixed.txt', 19, 'SEED_EAI_218');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'If we had left the hotel earlier, we would not have missed our flight.', 1),
        (@MaCH, N'B', N'If we left earlier, we would not miss flight.', 0),
        (@MaCH, N'C', N'If we have left earlier, we will not miss flight.', 0),
        (@MaCH, N'D', N'If we had left earlier, we will not have missed.', 0);
END
GO

-- Câu hỏi SEED_EAI_219 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_219')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 4, N'TracNghiem', N'NguPhap', N'Kho', N'Analyze the mixed conditional: ''If Mark had taken the job offer in London last year, he _______ living there now.''',
            NULL, NULL, N'Điều kiện trộn (Past cause -> Present result): Mệnh đề If loại 3 (had taken) kết hợp với mệnh đề chính loại 2 chỉ kết quả ở hiện tại (would be living now).', N'Điều kiện trộn (Past cause -> Present result): Mệnh đề If loại 3 (had taken) kết hợp với mệnh đề chính loại 2 chỉ kết quả ở hiện tại (would be living now).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 19 - Câu điều kiện trộn (Mixed Conditionals): Giả định quá khứ dẫn đến kết quả ở hiện tại.', N'Extracted', N'BaiHoc_19_ConditionalsMixed.txt', 19, 'SEED_EAI_219');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'would have been', 0),
        (@MaCH, N'B', N'would be', 1),
        (@MaCH, N'C', N'will be', 0),
        (@MaCH, N'D', N'is being', 0);
END
GO

-- Câu hỏi SEED_EAI_220 (Viet - Trình độ 4 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_220')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 4, N'VietLaiCau', N'Viet', N'Kho', N'Rewrite using inverted conditional: ''If you should require any further information, please contact our helpdesk.''',
            NULL, NULL, N'Đảo ngữ điều kiện loại 1: Bỏ If, đảo ''Should'' lên đầu câu: ''Should you require any further information, please contact our helpdesk.''', N'Đảo ngữ điều kiện loại 1: Bỏ If, đảo ''Should'' lên đầu câu: ''Should you require any further information, please contact our helpdesk.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 19 & 20 - Đảo ngữ câu điều kiện loại 1: Should + S + V-inf.', N'Extracted', N'BaiHoc_19_ConditionalsMixed.txt', 19, 'SEED_EAI_220');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Should you require any further information, please contact our helpdesk.', 1),
        (@MaCH, N'B', N'Did you require further information, contact helpdesk.', 0),
        (@MaCH, N'C', N'Were you requiring information, contact helpdesk.', 0),
        (@MaCH, N'D', N'Had you required information, please contact helpdesk.', 0);
END
GO

-- Câu hỏi SEED_EAI_221 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_221')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Which reporting verb is followed by a gerund (V-ing)?',
            NULL, NULL, N'Động từ ''admit'' và ''deny'' đi trực tiếp với danh động từ V-ing: ''He admitted breaking the test tube.''', N'Động từ ''admit'' và ''deny'' đi trực tiếp với danh động từ V-ing: ''He admitted breaking the test tube.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 31 - Cấu trúc động từ tường thuật đặc biệt: admit / deny + V-ing.', N'Extracted', N'BaiHoc_31_ReportedSpeechB2.txt', 31, 'SEED_EAI_221');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'refuse', 0),
        (@MaCH, N'B', N'admit', 1),
        (@MaCH, N'C', N'threaten', 0),
        (@MaCH, N'D', N'agree', 0);
END
GO

-- Câu hỏi SEED_EAI_222 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_222')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'How do you report an apology in indirect speech: ''"I am truly sorry I forgot our appointment," John said.''?',
            NULL, NULL, N'''apologize for + V-ing'': ''John apologized for forgetting their appointment.''', N'''apologize for + V-ing'': ''John apologized for forgetting their appointment.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 31 - Mẫu câu xin lỗi gián tiếp: S + apologize to O for + V-ing.', N'Extracted', N'BaiHoc_31_ReportedSpeechB2.txt', 31, 'SEED_EAI_222');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'John said sorry that he forgot.', 0),
        (@MaCH, N'B', N'John apologized for forgetting their appointment.', 1),
        (@MaCH, N'C', N'John apologized to forget their appointment.', 0),
        (@MaCH, N'D', N'John denied forgetting the appointment.', 0);
END
GO

-- Câu hỏi SEED_EAI_223 (NguPhap - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_223')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'Complete the indirect question: ''She asked me where I _______ during my summer holiday.''',
            NULL, NULL, N'Trong câu hỏi gián tiếp, cấu trúc từ hỏi là S+V (không đảo trợ động từ), lùi thì Quá khứ đơn thành Quá khứ hoàn thành: ''had gone''.', N'Trong câu hỏi gián tiếp, cấu trúc từ hỏi là S+V (không đảo trợ động từ), lùi thì Quá khứ đơn thành Quá khứ hoàn thành: ''had gone''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 31 - Cấu trúc câu hỏi gián tiếp: Wh-word + S + V(lùi thì).', N'Extracted', N'BaiHoc_31_ReportedSpeechB2.txt', 31, 'SEED_EAI_223');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'had gone', 1),
        (@MaCH, N'B', N'did go', 0),
        (@MaCH, N'C', N'went I', 0),
        (@MaCH, N'D', N'have gone', 0);
END
GO

-- Câu hỏi SEED_EAI_224 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_224')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the mistake: ''My supervisor warned me not touch the exposed electrical wire.''',
            NULL, NULL, N'Cấu trúc cảnh báo phủ định: ''warn someone NOT TO DO something''. Thiếu giới từ ''to'': ''not to touch''.', N'Cấu trúc cảnh báo phủ định: ''warn someone NOT TO DO something''. Thiếu giới từ ''to'': ''not to touch''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 31 - Động từ cảnh báo: S + warn + O + (not) to-V.', N'Extracted', N'BaiHoc_31_ReportedSpeechB2.txt', 31, 'SEED_EAI_224');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''warned'' should be ''warning''', 0),
        (@MaCH, N'B', N'''not touch'' should be ''not to touch''', 1),
        (@MaCH, N'C', N'''exposed'' should be ''exposing''', 0),
        (@MaCH, N'D', N'''electrical'' should be ''electrician''', 0);
END
GO

-- Câu hỏi SEED_EAI_225 (NguPhap - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_225')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'DungSai', N'NguPhap', N'TrungBinh', N'True or False: When reporting a universal truth or scientific law (e.g., ''The teacher said the Earth revolves around the Sun''), backshifting of tenses is not required.',
            NULL, NULL, N'Đúng. Với chân lý vĩnh cửu hoặc quy luật khoa học hiển nhiên, không bắt buộc lùi thì động từ.', N'Đúng. Với chân lý vĩnh cửu hoặc quy luật khoa học hiển nhiên, không bắt buộc lùi thì động từ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 31 - Trường hợp ngoại lệ không lùi thì: Sự thật hiển nhiên và chân lý khoa học.', N'Extracted', N'BaiHoc_31_ReportedSpeechB2.txt', 31, 'SEED_EAI_225');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_226 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_226')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Select the impersonal passive structure used in academic writing:',
            NULL, NULL, N'''It is widely believed that renewable energy will replace fossil fuels.'' là cấu trúc bị động khách quan (Impersonal Passive) rất được ưa chuộng trong văn phong học thuật.', N'''It is widely believed that renewable energy will replace fossil fuels.'' là cấu trúc bị động khách quan (Impersonal Passive) rất được ưa chuộng trong văn phong học thuật.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 32 - Bị động khách quan: It is said / believed / reported that + clause.', N'Extracted', N'BaiHoc_32_PassiveVoiceB2.txt', 32, 'SEED_EAI_226');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'People widely believe that renewable energy replaces.', 0),
        (@MaCH, N'B', N'It is widely believed that renewable energy will replace fossil fuels.', 1),
        (@MaCH, N'C', N'Renewable energy widely believes replacing fossil fuels.', 0),
        (@MaCH, N'D', N'Believing widely is that energy replaces.', 0);
END
GO

-- Câu hỏi SEED_EAI_227 (Viet - Trình độ 4 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_227')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'VietLaiCau', N'Viet', N'Kho', N'Transform into personal passive with infinitive: ''People say that the CEO donated two million dollars to cancer research.''',
            NULL, NULL, N'Chuyển thành: ''The CEO is said to have donated two million dollars to cancer research.'' (Hành động ''donated'' xảy ra trước hành động ''say'', dùng ''to have + V3'').', N'Chuyển thành: ''The CEO is said to have donated two million dollars to cancer research.'' (Hành động ''donated'' xảy ra trước hành động ''say'', dùng ''to have + V3'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 32 - Chuyển đổi bị động kép: S + be + V3 + to have + V3 (hành động trước thì hiện tại).', N'Extracted', N'BaiHoc_32_PassiveVoiceB2.txt', 32, 'SEED_EAI_227');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The CEO is said to have donated two million dollars to cancer research.', 1),
        (@MaCH, N'B', N'The CEO is said to donate two million dollars.', 0),
        (@MaCH, N'C', N'The CEO was said donating two million dollars.', 0),
        (@MaCH, N'D', N'The CEO said to be donated two million dollars.', 0);
END
GO

-- Câu hỏi SEED_EAI_228 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_228')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the passive form with a modal verb: ''Critical security patches _______ without delay.''',
            NULL, NULL, N'Bị động với động từ khiếm khuyết (Modal verb): ''Modal + must be + V3/ed''. Ở đây dùng ''must be installed''.', N'Bị động với động từ khiếm khuyết (Modal verb): ''Modal + must be + V3/ed''. Ở đây dùng ''must be installed''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 32 - Câu bị động với động từ khiếm khuyết: Modal + be + V3/ed.', N'Extracted', N'BaiHoc_32_PassiveVoiceB2.txt', 32, 'SEED_EAI_228');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'must install', 0),
        (@MaCH, N'B', N'must be installed', 1),
        (@MaCH, N'C', N'must have install', 0),
        (@MaCH, N'D', N'must being installed', 0);
END
GO

-- Câu hỏi SEED_EAI_229 (NguPhap - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_229')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'Complete the passive continuous sentence: ''A state-of-the-art laboratory is currently _______ constructed on campus.''',
            NULL, NULL, N'Thì Hiện tại tiếp diễn ở thể bị động: ''is being + V3/ed''.', N'Thì Hiện tại tiếp diễn ở thể bị động: ''is being + V3/ed''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 32 - Bị động ở các thì tiếp diễn: be + being + V3/ed.', N'Extracted', N'BaiHoc_32_PassiveVoiceB2.txt', 32, 'SEED_EAI_229');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'being', 1),
        (@MaCH, N'B', N'been', 0),
        (@MaCH, N'C', N'be', 0),
        (@MaCH, N'D', N'having', 0);
END
GO

-- Câu hỏi SEED_EAI_230 (NguPhap - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_230')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'DungSai', N'NguPhap', N'De', N'True or False: Intransitive verbs such as ''arrive'', ''happen'', and ''die'' cannot be used in the passive voice.',
            NULL, NULL, N'Đúng. Nội động từ (Intransitive verbs) không có tân ngữ trực tiếp nên không thể chuyển sang thể bị động.', N'Đúng. Nội động từ (Intransitive verbs) không có tân ngữ trực tiếp nên không thể chuyển sang thể bị động.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 32 - Điều kiện chuyển câu bị động: Động từ phải là Ngoại động từ (Transitive Verbs).', N'Extracted', N'BaiHoc_32_PassiveVoiceB2.txt', 32, 'SEED_EAI_230');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_231 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_231')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 5, N'TracNghiem', N'NguPhap', N'Kho', N'Choose the grammatically correct inversion sentence:',
            NULL, NULL, N'Khi cụm từ phủ định ''Under no circumstances'' đứng đầu câu, phải đảo trợ động từ lên trước chủ ngữ: ''Under no circumstances should confidential files be shared.''', N'Khi cụm từ phủ định ''Under no circumstances'' đứng đầu câu, phải đảo trợ động từ lên trước chủ ngữ: ''Under no circumstances should confidential files be shared.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 20 - Đảo ngữ với cụm từ phủ định: Under no circumstances + Auxiliary + S + V.', N'Extracted', N'BaiHoc_20_AdvancedInversion.txt', 20, 'SEED_EAI_231');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Under no circumstances confidential files should be shared.', 0),
        (@MaCH, N'B', N'Under no circumstances should confidential files be shared.', 1),
        (@MaCH, N'C', N'Under no circumstances files are confidential to share.', 0),
        (@MaCH, N'D', N'Under no circumstances sharing files should happen.', 0);
END
GO

-- Câu hỏi SEED_EAI_232 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_232')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 5, N'TracNghiem', N'NguPhap', N'Kho', N'Complete the inverted sentence: ''Seldom _______ such an inspiring keynote address at a tech summit.''',
            NULL, NULL, N'Sau ''Seldom'', đảo trợ động từ của thì Hiện tại hoàn thành: ''have I heard''.', N'Sau ''Seldom'', đảo trợ động từ của thì Hiện tại hoàn thành: ''have I heard''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 20 - Đảo ngữ với Seldom, Rarely, Never.', N'Extracted', N'BaiHoc_20_AdvancedInversion.txt', 20, 'SEED_EAI_232');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I have heard', 0),
        (@MaCH, N'B', N'have I heard', 1),
        (@MaCH, N'C', N'heard I', 0),
        (@MaCH, N'D', N'did I heard', 0);
END
GO

-- Câu hỏi SEED_EAI_233 (Viet - Trình độ 5 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_233')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 5, N'VietLaiCau', N'Viet', N'Kho', N'Invert the conditional sentence: ''If they had known about the severe traffic, they would have taken the train.''',
            NULL, NULL, N'Đảo ngữ câu điều kiện loại 3: Bỏ If, đảo ''Had'' lên trước chủ ngữ: ''Had they known about the severe traffic, they would have taken the train.''', N'Đảo ngữ câu điều kiện loại 3: Bỏ If, đảo ''Had'' lên trước chủ ngữ: ''Had they known about the severe traffic, they would have taken the train.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 20 - Đảo ngữ câu điều kiện loại 3: Had + S + (not) + V3/ed.', N'Extracted', N'BaiHoc_20_AdvancedInversion.txt', 20, 'SEED_EAI_233');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Had they known about the severe traffic, they would have taken the train.', 1),
        (@MaCH, N'B', N'Were they known about traffic, they would take train.', 0),
        (@MaCH, N'C', N'Did they know about traffic, they had taken train.', 0),
        (@MaCH, N'D', N'Should they have known traffic, they take train.', 0);
END
GO

-- Câu hỏi SEED_EAI_234 (Viet - Trình độ 5 - Dạng VietLaiCau - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_234')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 5, N'VietLaiCau', N'Viet', N'Kho', N'Invert the conditional sentence: ''If I were in your position, I would accept the overseas fellowship.''',
            NULL, NULL, N'Đảo ngữ câu điều kiện loại 2: Bỏ If, đảo ''Were'' lên trước chủ ngữ: ''Were I in your position, I would accept the overseas fellowship.''', N'Đảo ngữ câu điều kiện loại 2: Bỏ If, đảo ''Were'' lên trước chủ ngữ: ''Were I in your position, I would accept the overseas fellowship.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 20 - Đảo ngữ câu điều kiện loại 2: Were + S + to-V / complement.', N'Extracted', N'BaiHoc_20_AdvancedInversion.txt', 20, 'SEED_EAI_234');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Were I in your position, I would accept the overseas fellowship.', 1),
        (@MaCH, N'B', N'Was I in your position, I accept.', 0),
        (@MaCH, N'C', N'Had I been in position, I will accept.', 0),
        (@MaCH, N'D', N'Should I be in position, I would accepted.', 0);
END
GO

-- Câu hỏi SEED_EAI_235 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_235')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 5, N'TracNghiem', N'NguPhap', N'Kho', N'Fill in the correlative pair: ''No sooner had the keynote begun _______ a power outage plunged the hall into darkness.''',
            NULL, NULL, N'Cặp liên từ đi với No sooner là ''than'' (''No sooner had + S + V3 than + S + V2''). (Khác với ''Hardly... when'').', N'Cặp liên từ đi với No sooner là ''than'' (''No sooner had + S + V3 than + S + V2''). (Khác với ''Hardly... when'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 20 - Cặp đảo ngữ thời gian: No sooner... than vs. Hardly / Scarcely... when.', N'Extracted', N'BaiHoc_20_AdvancedInversion.txt', 20, 'SEED_EAI_235');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'when', 0),
        (@MaCH, N'B', N'than', 1),
        (@MaCH, N'C', N'then', 0),
        (@MaCH, N'D', N'as', 0);
END
GO

-- Câu hỏi SEED_EAI_236 (NguPhap - Trình độ 3 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_236')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Which sentence uses the correct order of adjectives before a noun (OSASCOMP)?',
            NULL, NULL, N'Thứ tự tính từ OSASCOMP: Opinion -> Size -> Age -> Shape -> Color -> Origin -> Material -> Purpose. ''a beautiful small antique wooden desk'' đúng thứ tự: Opinion (beautiful) -> Size (small) -> Age (antique) -> Material (wooden).', N'Thứ tự tính từ OSASCOMP: Opinion -> Size -> Age -> Shape -> Color -> Origin -> Material -> Purpose. ''a beautiful small antique wooden desk'' đúng thứ tự: Opinion (beautiful) -> Size (small) -> Age (antique) -> Material (wooden).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 14 - Trật tự tính từ trước danh từ (OSASCOMP rule).', N'Applied', N'BaiHoc_14_PresentPerfect.txt', 14, 'SEED_EAI_236');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'a wooden antique beautiful small desk', 0),
        (@MaCH, N'B', N'a beautiful small antique wooden desk', 1),
        (@MaCH, N'C', N'a small wooden antique beautiful desk', 0),
        (@MaCH, N'D', N'an antique beautiful wooden small desk', 0);
END
GO

-- Câu hỏi SEED_EAI_237 (NguPhap - Trình độ 3 - Dạng DienKhuyet - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_237')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'DienKhuyet', N'NguPhap', N'De', N'Fill in the blank with a comparative form: ''Electric cars are becoming significantly _______ expensive than they were a decade ago.''',
            NULL, NULL, N'''less expensive'' (ít đắt đỏ hơn) hoặc ''more expensive''. Ở đây ngữ cảnh xe điện ngày càng dễ tiếp cận là ''less expensive''.', N'''less expensive'' (ít đắt đỏ hơn) hoặc ''more expensive''. Ở đây ngữ cảnh xe điện ngày càng dễ tiếp cận là ''less expensive''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 15 - Cấu trúc so sánh hơn kém của tính từ dài.', N'Applied', N'BaiHoc_15_PastSimpleVsContinuous.txt', 15, 'SEED_EAI_237');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'less', 1),
        (@MaCH, N'B', N'fewer', 0),
        (@MaCH, N'C', N'least', 0),
        (@MaCH, N'D', N'little', 0);
END
GO

-- Câu hỏi SEED_EAI_238 (NguPhap - Trình độ 4 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_238')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct prepositional relative clause: ''The framework _______ the web application is built requires Node.js v18.''',
            NULL, NULL, N'''on which'' bổ nghĩa cho nền tảng framework mà trên đó ứng dụng được xây dựng.', N'''on which'' bổ nghĩa cho nền tảng framework mà trên đó ứng dụng được xây dựng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 17 & 36 - Giới từ đi kèm đại từ quan hệ: on which / in which.', N'Applied', N'BaiHoc_17_RelativeClauses.txt', 17, 'SEED_EAI_238');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'on which', 1),
        (@MaCH, N'B', N'at that', 0),
        (@MaCH, N'C', N'with whom', 0),
        (@MaCH, N'D', N'for whose', 0);
END
GO

-- Câu hỏi SEED_EAI_239 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_239')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 3, N'TracNghiem', N'NguPhap', N'De', N'Which clause is synonymous with ''Unless you study daily, you will not master the language''?',
            NULL, NULL, N'''Unless'' = ''If not''. Câu tương đương là: ''If you do not study daily, you will not master the language.''', N'''Unless'' = ''If not''. Câu tương đương là: ''If you do not study daily, you will not master the language.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 19 - Cách dùng Unless = If not.', N'Extracted', N'BaiHoc_19_ConditionalsMixed.txt', 19, 'SEED_EAI_239');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'If you study daily, you fail.', 0),
        (@MaCH, N'B', N'If you do not study daily, you will not master the language.', 1),
        (@MaCH, N'C', N'Provided you study daily, you will fail.', 0),
        (@MaCH, N'D', N'As long as you study, you do not master.', 0);
END
GO

-- Câu hỏi SEED_EAI_240 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_240')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the mistake: ''The ancient manuscript was discovered by accidental during excavations.''',
            NULL, NULL, N'Cụm trạng từ đúng là ''by accident'' (tình cờ) hoặc dùng trạng từ ''accidentally'', không dùng ''by accidental''.', N'Cụm trạng từ đúng là ''by accident'' (tình cờ) hoặc dùng trạng từ ''accidentally'', không dùng ''by accidental''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 32 - Trạng từ chỉ cách thức trong câu bị động.', N'Applied', N'BaiHoc_32_PassiveVoiceB2.txt', 32, 'SEED_EAI_240');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''was discovered'' should be ''is discover''', 0),
        (@MaCH, N'B', N'''by accidental'' should be ''by accident''', 1),
        (@MaCH, N'C', N'''during'' should be ''while''', 0),
        (@MaCH, N'D', N'''excavations'' should be ''excavate''', 0);
END
GO

-- Câu hỏi SEED_EAI_241 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_241')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Select the sentence with the correct past perfect continuous tense:',
            NULL, NULL, N'''They had been debugging the codebase for five hours before they found the memory leak.'' diễn tả hành động xảy ra và kéo dài liên tục trước một mốc quá khứ khác.', N'''They had been debugging the codebase for five hours before they found the memory leak.'' diễn tả hành động xảy ra và kéo dài liên tục trước một mốc quá khứ khác.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 15 - Thì Quá khứ hoàn thành tiếp diễn (Past Perfect Continuous): had been + V-ing.', N'Extracted', N'BaiHoc_15_PastSimpleVsContinuous.txt', 15, 'SEED_EAI_241');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'They had been debugging the codebase for five hours before they found the memory leak.', 1),
        (@MaCH, N'B', N'They have been debugging for five hours before found leak.', 0),
        (@MaCH, N'C', N'They were debugging since five hours before found.', 0),
        (@MaCH, N'D', N'They had debugged continually before find.', 0);
END
GO

-- Câu hỏi SEED_EAI_242 (NguPhap - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_242')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 4, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'Complete the conditional phrase: ''_______ for your timely intervention, the company would have gone bankrupt.''',
            NULL, NULL, N'Thành ngữ điều kiện: ''But for + Noun'' = ''Were it not for / Had it not been for'' (Nếu không nhờ có...).', N'Thành ngữ điều kiện: ''But for + Noun'' = ''Were it not for / Had it not been for'' (Nếu không nhờ có...).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 19 - Cụm từ thay thế câu điều kiện: But for / Without + Noun.', N'Extracted', N'BaiHoc_19_ConditionalsMixed.txt', 19, 'SEED_EAI_242');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'But', 1),
        (@MaCH, N'B', N'Except', 0),
        (@MaCH, N'C', N'Unless', 0),
        (@MaCH, N'D', N'Besides', 0);
END
GO

-- Câu hỏi SEED_EAI_243 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_243')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct preposition: ''The candidate was praised _______ her exceptional analytical skills during the debate.''',
            NULL, NULL, N'Cụm động từ bị động: ''be praised for sth'' (được khen ngợi vì điều gì).', N'Cụm động từ bị động: ''be praised for sth'' (được khen ngợi vì điều gì).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 32 - Động từ bị động đi kèm giới từ cố định: be praised for, be accused of.', N'Extracted', N'BaiHoc_32_PassiveVoiceB2.txt', 32, 'SEED_EAI_243');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'with', 0),
        (@MaCH, N'B', N'for', 1),
        (@MaCH, N'C', N'at', 0),
        (@MaCH, N'D', N'of', 0);
END
GO

-- Câu hỏi SEED_EAI_244 (NguPhap - Trình độ 3 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_244')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'DungSai', N'NguPhap', N'De', N'True or False: Stative verbs (e.g., ''belong'', ''know'', ''believe'') are rarely used in continuous tenses.',
            NULL, NULL, N'Đúng. Động từ chỉ trạng thái (Stative verbs) chỉ nhận thức, sở hữu và cảm xúc thường không chia ở thì tiếp diễn.', N'Đúng. Động từ chỉ trạng thái (Stative verbs) chỉ nhận thức, sở hữu và cảm xúc thường không chia ở thì tiếp diễn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 14 & 15 - Phân biệt Action Verbs vs. Stative Verbs.', N'Extracted', N'BaiHoc_14_PresentPerfect.txt', 14, 'SEED_EAI_244');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_245 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_245')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 5, N'TracNghiem', N'NguPhap', N'Kho', N'Invert the sentence starting with ''Not only'': ''The software is both fast and energy-efficient.''',
            NULL, NULL, N'''Not only is the software fast, but it is also energy-efficient.'' đảo trợ động từ ''is'' lên trước chủ ngữ.', N'''Not only is the software fast, but it is also energy-efficient.'' đảo trợ động từ ''is'' lên trước chủ ngữ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 20 - Đảo ngữ với Not only: Not only + Aux + S + V, but S + also + V.', N'Extracted', N'BaiHoc_20_AdvancedInversion.txt', 20, 'SEED_EAI_245');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Not only the software is fast, but it is also energy-efficient.', 0),
        (@MaCH, N'B', N'Not only is the software fast, but it is also energy-efficient.', 1),
        (@MaCH, N'C', N'Not only fast software is, but energy-efficient.', 0),
        (@MaCH, N'D', N'Not only does the software fast, but also energy.', 0);
END
GO

-- Câu hỏi SEED_EAI_246 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_246')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (11, 3, N'TracNghiem', N'NguPhap', N'De', N'Which relative pronoun is used to refer to a place: ''Da Nang is the coastal city _______ my parents were born.''',
            NULL, NULL, N'Trạng từ quan hệ chỉ nơi chốn là ''where''.', N'Trạng từ quan hệ chỉ nơi chốn là ''where''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 17 & 36 - Trạng từ quan hệ chỉ nơi chốn: Where = In/at which.', N'Extracted', N'BaiHoc_17_RelativeClauses.txt', 17, 'SEED_EAI_246');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'when', 0),
        (@MaCH, N'B', N'where', 1),
        (@MaCH, N'C', N'which', 0),
        (@MaCH, N'D', N'why', 0);
END
GO

-- Câu hỏi SEED_EAI_247 (Viet - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_247')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (23, 4, N'VietLaiCau', N'Viet', N'TrungBinh', N'Change into active voice: ''The bridge was designed by Gustave Eiffel in 1889.''',
            NULL, NULL, N'Chuyển sang chủ động: ''Gustave Eiffel designed the bridge in 1889.''', N'Chuyển sang chủ động: ''Gustave Eiffel designed the bridge in 1889.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 32 - Chuyển đổi giữa câu Chủ động và Bị động.', N'Applied', N'BaiHoc_32_PassiveVoiceB2.txt', 32, 'SEED_EAI_247');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Gustave Eiffel designed the bridge in 1889.', 1),
        (@MaCH, N'B', N'Gustave Eiffel was designing the bridge in 1889.', 0),
        (@MaCH, N'C', N'Gustave Eiffel has designed the bridge in 1889.', 0),
        (@MaCH, N'D', N'The bridge had Gustave Eiffel designed in 1889.', 0);
END
GO

-- Câu hỏi SEED_EAI_248 (NguPhap - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_248')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 4, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'Fill in the blank with the correct conditional connector: ''You can borrow my car _______ you return it with a full tank of fuel.''',
            NULL, NULL, N'''as long as'' hoặc ''provided that'' mang nghĩa ''miễn là'' (điều kiện ràng buộc).', N'''as long as'' hoặc ''provided that'' mang nghĩa ''miễn là'' (điều kiện ràng buộc).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 19 - Các từ nối điều kiện: As long as / Provided that / In case.', N'Extracted', N'BaiHoc_19_ConditionalsMixed.txt', 19, 'SEED_EAI_248');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'as long as', 1),
        (@MaCH, N'B', N'unless', 0),
        (@MaCH, N'C', N'in spite of', 0),
        (@MaCH, N'D', N'although', 0);
END
GO

-- Câu hỏi SEED_EAI_249 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_249')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 4, N'SuaLoiSai', N'NguPhap', N'Kho', N'Find the mistake: ''I would rather you don''t reveal the project timeline until the launch.''',
            NULL, NULL, N'Cấu trúc ''would rather someone did/didn''t do sth'' (muốn ai làm/không làm gì ở hiện tại/tương lai) lùi 1 thì về quá khứ giả định: ''didn''t reveal'', không dùng ''don''t''.', N'Cấu trúc ''would rather someone did/didn''t do sth'' (muốn ai làm/không làm gì ở hiện tại/tương lai) lùi 1 thì về quá khứ giả định: ''didn''t reveal'', không dùng ''don''t''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 14 - Cấu trúc giả định với Would rather: S1 + would rather + S2 + Past Simple.', N'Extracted', N'BaiHoc_14_PresentPerfect.txt', 14, 'SEED_EAI_249');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''would rather'' should be ''had rather''', 0),
        (@MaCH, N'B', N'''don''t'' should be ''didn''t''', 1),
        (@MaCH, N'C', N'''reveal'' should be ''revealed''', 0),
        (@MaCH, N'D', N'''until'' should be ''since''', 0);
END
GO

-- Câu hỏi SEED_EAI_250 (NguPhap - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_250')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (12, 5, N'TracNghiem', N'NguPhap', N'Kho', N'Choose the inverted sentence using ''Only after'': ''The researchers understood the anomaly only after they repeated the experiment five times.''',
            NULL, NULL, N'Khi ''Only after + clause'' đứng đầu câu, mệnh đề chính phía sau phải đảo ngữ: ''Only after they repeated the experiment five times did the researchers understand the anomaly.''', N'Khi ''Only after + clause'' đứng đầu câu, mệnh đề chính phía sau phải đảo ngữ: ''Only after they repeated the experiment five times did the researchers understand the anomaly.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 20 - Đảo ngữ với Only after: Only after + Clause + Aux + S + V.', N'Extracted', N'BaiHoc_20_AdvancedInversion.txt', 20, 'SEED_EAI_250');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Only after they repeated the experiment five times, the researchers understood the anomaly.', 0),
        (@MaCH, N'B', N'Only after they repeated the experiment five times did the researchers understand the anomaly.', 1),
        (@MaCH, N'C', N'Only after did they repeat the experiment five times, researchers understood.', 0),
        (@MaCH, N'D', N'Only after repeating five times, anomaly was understood by researchers.', 0);
END
GO

PRINT N'Đã hoàn thành nạp Batch 05 (SEED_EAI_201 -> SEED_EAI_250) thành công!';
GO
