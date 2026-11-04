-- ====================================================================================
-- SEED QUESTIONS BATCH: Batch 01 (SEED_EAI_001 -> SEED_EAI_050)
-- Số lượng câu hỏi: 50
-- Đảm bảo: Idempotent (Chạy lại nhiều lần không trùng lặp)
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Đang nạp Batch 01 (SEED_EAI_001 -> SEED_EAI_050) (50 câu hỏi)...';

-- Câu hỏi SEED_EAI_001 (Nghe - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_001')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 1, N'TracNghiem', N'Nghe', N'De', N'When meeting someone for the first time in an informal context, which greeting is the most natural?',
            NULL, NULL, N'Chào hỏi thân mật thường dùng ''Hi, how is it going?'' hoặc ''Nice to meet you''. Các cách như ''How do you do?'' là quá trang trọng.', N'Chào hỏi thân mật thường dùng ''Hi, how is it going?'' hoặc ''Nice to meet you''. Các cách như ''How do you do?'' là quá trang trọng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 0 - Mục 1: Mẫu câu chào hỏi thường ngày (Informal Greetings): ''Hi / Hello, how is it going?''', N'Extracted', N'BaiHoc_00_LaoQuen_GioiThieu.txt', 0, 'SEED_EAI_001');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'How do you do, sir?', 0),
        (@MaCH, N'B', N'Hi, nice to meet you!', 1),
        (@MaCH, N'C', N'I hereby greet you.', 0),
        (@MaCH, N'D', N'Good day to your presence.', 0);
END
GO

-- Câu hỏi SEED_EAI_002 (TuVung - Trình độ 1 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_002')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 1, N'DienKhuyet', N'TuVung', N'De', N'Complete the sentence: ''Let me introduce _______. My name is Alex, and I am from Da Nang.''',
            NULL, NULL, N'Cụm từ chuẩn ''introduce myself'' mang nghĩa ''Cho phép tôi tự giới thiệu bản thân''.', N'Cụm từ chuẩn ''introduce myself'' mang nghĩa ''Cho phép tôi tự giới thiệu bản thân''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 0 - Mục 2: Tự giới thiệu bản thân: ''Let me introduce myself. My name is...''', N'Extracted', N'BaiHoc_00_LaoQuen_GioiThieu.txt', 0, 'SEED_EAI_002');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'myself', 1),
        (@MaCH, N'B', N'me', 0),
        (@MaCH, N'C', N'mine', 0),
        (@MaCH, N'D', N'I', 0);
END
GO

-- Câu hỏi SEED_EAI_003 (NguPhap - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_003')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 1, N'TracNghiem', N'NguPhap', N'De', N'Choose the correct question to ask someone about their job:',
            NULL, NULL, N'Câu hỏi nghề nghiệp thông dụng trong tiếng Anh là ''What do you do?'' hoặc ''What is your occupation?''.', N'Câu hỏi nghề nghiệp thông dụng trong tiếng Anh là ''What do you do?'' hoặc ''What is your occupation?''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 0 - Mục 3: Hỏi về nghề nghiệp: ''What do you do? / What do you do for a living?''', N'Extracted', N'BaiHoc_00_LaoQuen_GioiThieu.txt', 0, 'SEED_EAI_003');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'What are you doing now?', 0),
        (@MaCH, N'B', N'What do you do?', 1),
        (@MaCH, N'C', N'Where do you work at this second?', 0),
        (@MaCH, N'D', N'Who is your job?', 0);
END
GO

-- Câu hỏi SEED_EAI_004 (TuVung - Trình độ 1 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_004')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 1, N'DungSai', N'TuVung', N'De', N'True or False: In English, ''Where do you come from?'' and ''Where are you from?'' have the exact same meaning.',
            NULL, NULL, N'Đúng. Cả hai câu đều dùng để hỏi về quê hương hoặc quốc tịch xuất thân của đối phương.', N'Đúng. Cả hai câu đều dùng để hỏi về quê hương hoặc quốc tịch xuất thân của đối phương.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 0 - Mục 2: Hỏi xuất xứ quê hương: ''Where are you from? = Where do you come from?''', N'Extracted', N'BaiHoc_00_LaoQuen_GioiThieu.txt', 0, 'SEED_EAI_004');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_005 (TuVung - Trình độ 1 - Dạng NoiCap - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_005')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 1, N'NoiCap', N'TuVung', N'De', N'Match each self-introduction phrase with its correct communicative purpose:
1. ''I was born and raised in Hanoi.''
2. ''I am keen on reading tech blogs.''
3. ''I work as a frontend developer.''',
            NULL, NULL, N'1 nói về nguồn gốc/quê quán; 2 nói về sở thích/đam mê; 3 nói về nghề nghiệp chuyên môn.', N'1 nói về nguồn gốc/quê quán; 2 nói về sở thích/đam mê; 3 nói về nghề nghiệp chuyên môn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 0 - Mục 4: Mở rộng thông tin cá nhân: Quê hương, Nghề nghiệp và Sở thích cá nhân.', N'Applied', N'BaiHoc_00_LaoQuen_GioiThieu.txt', 0, 'SEED_EAI_005');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'1-Hometown, 2-Hobby, 3-Occupation', 1),
        (@MaCH, N'B', N'1-Occupation, 2-Hometown, 3-Hobby', 0),
        (@MaCH, N'C', N'1-Hobby, 2-Occupation, 3-Hometown', 0),
        (@MaCH, N'D', N'1-Hometown, 2-Occupation, 3-Hobby', 0);
END
GO

-- Câu hỏi SEED_EAI_006 (Doc - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_006')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 2, N'TracNghiem', N'Doc', N'De', N'Which greeting is appropriate when meeting a business partner or client for the first time?',
            NULL, NULL, N'''Good morning, pleased to meet you'' là lời chào trang trọng (Formal), phù hợp trong môi trường kinh doanh.', N'''Good morning, pleased to meet you'' là lời chào trang trọng (Formal), phù hợp trong môi trường kinh doanh.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 1 - Bảng so sánh Formal vs Informal: Trang trọng dùng ''Pleased to meet you / Good morning Mr./Ms...''', N'Extracted', N'BaiHoc_01_Formal_Informal.txt', 1, 'SEED_EAI_006');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'What''s up, buddy?', 0),
        (@MaCH, N'B', N'Good morning, it is a pleasure to meet you.', 1),
        (@MaCH, N'C', N'Hey there, how ya doing?', 0),
        (@MaCH, N'D', N'Catch you later!', 0);
END
GO

-- Câu hỏi SEED_EAI_007 (NguPhap - Trình độ 2 - Dạng DienKhuyet - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_007')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 2, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'In a formal business meeting, complete the sentence: ''It is an honor to _______ with your prestigious company.''',
            NULL, NULL, N'Cụm ''collaborate with'' hoặc ''partner with'' mang tính trang trọng lịch sự cao.', N'Cụm ''collaborate with'' hoặc ''partner with'' mang tính trang trọng lịch sự cao.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 1 - Mục 2: Ngữ cảnh giao tiếp đối tác thương mại.', N'Applied', N'BaiHoc_01_Formal_Informal.txt', 1, 'SEED_EAI_007');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'collaborate', 1),
        (@MaCH, N'B', N'hang out', 0),
        (@MaCH, N'C', N'mess around', 0),
        (@MaCH, N'D', N'chill', 0);
END
GO

-- Câu hỏi SEED_EAI_008 (NguPhap - Trình độ 2 - Dạng SuaLoiSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_008')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 2, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the mistake in this formal email greeting: ''Dear Sir, I am writing to you because I wanna ask about the contract.''',
            NULL, NULL, N'''wanna'' là dạng viết tắt thông tục (slang/informal), trong văn bản trang trọng phải viết ''would like to'' hoặc ''wish to''.', N'''wanna'' là dạng viết tắt thông tục (slang/informal), trong văn bản trang trọng phải viết ''would like to'' hoặc ''wish to''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 1 - Quy tắc viết email công việc: Tuyệt đối không dùng tiếng lóng (gonna, wanna, gotta) trong thư trang trọng.', N'Extracted', N'BaiHoc_01_Formal_Informal.txt', 1, 'SEED_EAI_008');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''Dear Sir'' should be ''Hey Sir''', 0),
        (@MaCH, N'B', N'''wanna'' should be ''would like to''', 1),
        (@MaCH, N'C', N'''about'' should be ''with''', 0),
        (@MaCH, N'D', N'''am writing'' should be ''write''', 0);
END
GO

-- Câu hỏi SEED_EAI_009 (Doc - Trình độ 2 - Dạng SapXep - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_009')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 2, N'SapXep', N'Doc', N'TrungBinh', N'Rearrange the dialogue lines in a formal introduction:
(1) ''Pleased to meet you too, Dr. Davis. Thank you for inviting me.''
(2) ''Good afternoon, Mr. Green. Allow me to introduce Dr. Davis, our department head.''
(3) ''Good afternoon. It is a pleasure to make your acquaintance, Mr. Green.''',
            NULL, NULL, N'Thứ tự hợp lý: (2) Người dẫn giới thiệu -> (3) Dr. Davis chào và bày tỏ sự hân hạnh -> (1) Mr. Green đáp lại.', N'Thứ tự hợp lý: (2) Người dẫn giới thiệu -> (3) Dr. Davis chào và bày tỏ sự hân hạnh -> (1) Mr. Green đáp lại.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 1 - Mục 3: Thứ tự đối thoại giới thiệu bên thứ 3 trong công sở.', N'Extracted', N'BaiHoc_01_Formal_Informal.txt', 1, 'SEED_EAI_009');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'(2) -> (3) -> (1)', 1),
        (@MaCH, N'B', N'(1) -> (2) -> (3)', 0),
        (@MaCH, N'C', N'(3) -> (1) -> (2)', 0),
        (@MaCH, N'D', N'(2) -> (1) -> (3)', 0);
END
GO

-- Câu hỏi SEED_EAI_010 (TuVung - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_010')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 2, N'TracNghiem', N'TuVung', N'De', N'Which response corresponds to ''How do you do?'' in formal British English?',
            NULL, NULL, N'''How do you do?'' là một lời chào nghi thức trang trọng chứ không phải câu hỏi thăm sức khỏe; câu trả lời chuẩn tắc là ''How do you do?''.', N'''How do you do?'' là một lời chào nghi thức trang trọng chứ không phải câu hỏi thăm sức khỏe; câu trả lời chuẩn tắc là ''How do you do?''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 1 - Lưu ý văn hóa: ''How do you do?'' được đáp lại bằng chính ''How do you do?''.', N'Extracted', N'BaiHoc_01_Formal_Informal.txt', 1, 'SEED_EAI_010');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I am fine, thank you.', 0),
        (@MaCH, N'B', N'How do you do?', 1),
        (@MaCH, N'C', N'Yes, I do.', 0),
        (@MaCH, N'D', N'Not much, you?', 0);
END
GO

-- Câu hỏi SEED_EAI_011 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_011')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'Choose the word that best fits: ''As a project manager, I am _______ for coordinating cross-functional engineering teams.''',
            NULL, NULL, N'Cấu trúc ''responsible for + V-ing'' mang nghĩa ''chịu trách nhiệm về việc gì''.', N'Cấu trúc ''responsible for + V-ing'' mang nghĩa ''chịu trách nhiệm về việc gì''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 2 - Phần 2: Cấu trúc miêu tả trách nhiệm công việc: ''be responsible for / in charge of''.', N'Extracted', N'BaiHoc_02_ElevatorPitch.txt', 2, 'SEED_EAI_011');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'responsible', 1),
        (@MaCH, N'B', N'charge', 0),
        (@MaCH, N'C', N'duty', 0),
        (@MaCH, N'D', N'capable', 0);
END
GO

-- Câu hỏi SEED_EAI_012 (Viet - Trình độ 3 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_012')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 3, N'VietLaiCau', N'Viet', N'TrungBinh', N'Rewrite the sentence using ''specialize in'': ''My primary field of work is mobile app development.''',
            NULL, NULL, N'''I specialize in mobile app development.'' là cấu trúc chuẩn để giới thiệu chuyên môn sâu trong Elevator Pitch.', N'''I specialize in mobile app development.'' là cấu trúc chuẩn để giới thiệu chuyên môn sâu trong Elevator Pitch.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 2 - Elevator Pitch: Dùng động từ ''specialize in'' để nêu bật thế mạnh chuyên môn.', N'Applied', N'BaiHoc_02_ElevatorPitch.txt', 2, 'SEED_EAI_012');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I specialize in mobile app development.', 1),
        (@MaCH, N'B', N'I am specialize of mobile app development.', 0),
        (@MaCH, N'C', N'I specialized on development of mobile app.', 0),
        (@MaCH, N'D', N'My special is mobile app develop.', 0);
END
GO

-- Câu hỏi SEED_EAI_013 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_013')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 3, N'TracNghiem', N'Doc', N'TrungBinh', N'In an Elevator Pitch, what is the ideal duration recommended for introducing oneself effectively?',
            NULL, NULL, N'Theo bài học, một bài Elevator Pitch hiệu quả nên kéo dài từ 30 đến 60 giây (khoảng 75-150 từ).', N'Theo bài học, một bài Elevator Pitch hiệu quả nên kéo dài từ 30 đến 60 giây (khoảng 75-150 từ).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 2 - Mục 1: Định nghĩa Elevator Pitch: Thời lượng tối ưu 30 - 60 giây.', N'Extracted', N'BaiHoc_02_ElevatorPitch.txt', 2, 'SEED_EAI_013');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'5 to 10 minutes', 0),
        (@MaCH, N'B', N'30 to 60 seconds', 1),
        (@MaCH, N'C', N'3 to 5 minutes', 0),
        (@MaCH, N'D', N'10 to 15 seconds', 0);
END
GO

-- Câu hỏi SEED_EAI_014 (TuVung - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_014')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 3, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Complete the sentence describing a passion: ''I have a strong _______ for artificial intelligence and its applications in healthcare.''',
            NULL, NULL, N'Cụm danh từ ''have a strong passion for sth'' nghĩa là ''có niềm đam mê mãnh liệt với cái gì''.', N'Cụm danh từ ''have a strong passion for sth'' nghĩa là ''có niềm đam mê mãnh liệt với cái gì''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 2 - Mục 3: Thể hiện đam mê cá nhân: ''have a passion for / be passionate about''.', N'Extracted', N'BaiHoc_02_ElevatorPitch.txt', 2, 'SEED_EAI_014');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'passion', 1),
        (@MaCH, N'B', N'passionate', 0),
        (@MaCH, N'C', N'passionately', 0),
        (@MaCH, N'D', N'passions', 0);
END
GO

-- Câu hỏi SEED_EAI_015 (Doc - Trình độ 3 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_015')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 3, N'DungSai', N'Doc', N'De', N'True or False: When introducing your hometown to an international audience, you should include a key feature such as culture, tourism, or economic significance.',
            NULL, NULL, N'Đúng. Cung cấp một điểm nhấn nổi bật (landscape, heritage, economy) giúp đối phương dễ ghi nhớ quê hương bạn hơn.', N'Đúng. Cung cấp một điểm nhấn nổi bật (landscape, heritage, economy) giúp đối phương dễ ghi nhớ quê hương bạn hơn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 2 - Mục 4: Giới thiệu quê hương gắn liền với bản sắc độc đáo.', N'Extracted', N'BaiHoc_02_ElevatorPitch.txt', 2, 'SEED_EAI_015');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_016 (Nghe - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_016')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 2, N'TracNghiem', N'Nghe', N'De', N'What is the most polite way to ask for a table for two at a restaurant?',
            NULL, NULL, N'''We would like a table for two, please'' hoặc ''Could we have a table for two, please?'' là cách diễn đạt lịch sự chuẩn mực.', N'''We would like a table for two, please'' hoặc ''Could we have a table for two, please?'' là cách diễn đạt lịch sự chuẩn mực.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 3 - Phần 1: Mẫu câu đặt bàn: ''Could I book a table for two? / A table for two, please.''', N'Extracted', N'BaiHoc_03_DiningOut.txt', 3, 'SEED_EAI_016');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Give us a table for two right now.', 0),
        (@MaCH, N'B', N'Could we have a table for two, please?', 1),
        (@MaCH, N'C', N'Two seats are ours.', 0),
        (@MaCH, N'D', N'I take two chairs here.', 0);
END
GO

-- Câu hỏi SEED_EAI_017 (TuVung - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_017')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 2, N'TracNghiem', N'TuVung', N'De', N'What do you call the small amount of food served before the main course?',
            NULL, NULL, N'''Appetizer'' (tiếng Anh Mỹ) hoặc ''Starter'' (tiếng Anh Anh) là món khai vị ăn trước món chính.', N'''Appetizer'' (tiếng Anh Mỹ) hoặc ''Starter'' (tiếng Anh Anh) là món khai vị ăn trước món chính.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 3 - Từ vựng thực đơn: Starter / Appetizer: Món khai vị; Main course: Món chính; Dessert: Tráng miệng.', N'Extracted', N'BaiHoc_03_DiningOut.txt', 3, 'SEED_EAI_017');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Dessert', 0),
        (@MaCH, N'B', N'Main course', 0),
        (@MaCH, N'C', N'Appetizer / Starter', 1),
        (@MaCH, N'D', N'Beverage', 0);
END
GO

-- Câu hỏi SEED_EAI_018 (TuVung - Trình độ 2 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_018')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 2, N'DienKhuyet', N'TuVung', N'De', N'When asking the server for the total payment, you say: ''Could we have the _______, please?''',
            NULL, NULL, N'Trong nhà hàng, ''bill'' (Anh-Anh) hoặc ''check'' (Anh-Mỹ) là hóa đơn thanh toán tiền ăn.', N'Trong nhà hàng, ''bill'' (Anh-Anh) hoặc ''check'' (Anh-Mỹ) là hóa đơn thanh toán tiền ăn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 3 - Phần 3: Yêu cầu thanh toán: ''Could we get the bill/check, please?''', N'Extracted', N'BaiHoc_03_DiningOut.txt', 3, 'SEED_EAI_018');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'bill', 1),
        (@MaCH, N'B', N'menu', 0),
        (@MaCH, N'C', N'recipe', 0),
        (@MaCH, N'D', N'napkin', 0);
END
GO

-- Câu hỏi SEED_EAI_019 (TuVung - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_019')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 2, N'TracNghiem', N'TuVung', N'TrungBinh', N'If a customer cannot eat peanuts due to health risks, how should they inform the waiter?',
            NULL, NULL, N'Mẫu câu chuẩn: ''I am allergic to peanuts'' (Tôi bị dị ứng với đậu phộng).', N'Mẫu câu chuẩn: ''I am allergic to peanuts'' (Tôi bị dị ứng với đậu phộng).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 3 - Mục Lưu ý về dị ứng thực phẩm: ''I am allergic to [food item]''.', N'Extracted', N'BaiHoc_03_DiningOut.txt', 3, 'SEED_EAI_019');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I hate peanuts completely.', 0),
        (@MaCH, N'B', N'I am allergic to peanuts.', 1),
        (@MaCH, N'C', N'Peanuts are bad for my cooking.', 0),
        (@MaCH, N'D', N'I refuse peanuts because of habit.', 0);
END
GO

-- Câu hỏi SEED_EAI_020 (Doc - Trình độ 2 - Dạng SapXep - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_020')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 2, N'SapXep', N'Doc', N'TrungBinh', N'Order the sequence of dining out steps:
(1) Ordering the main course and beverages
(2) Asking for the bill and paying
(3) Reserving or requesting a table
(4) Enjoying the dessert',
            NULL, NULL, N'Trình tự ăn uống tại nhà hàng: Đặt bàn (3) -> Gọi món & đồ uống (1) -> Thưởng thức tráng miệng (4) -> Thanh toán (2).', N'Trình tự ăn uống tại nhà hàng: Đặt bàn (3) -> Gọi món & đồ uống (1) -> Thưởng thức tráng miệng (4) -> Thanh toán (2).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 3 - Trình tự trải nghiệm dịch vụ nhà hàng quốc tế.', N'Applied', N'BaiHoc_03_DiningOut.txt', 3, 'SEED_EAI_020');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'(3) -> (1) -> (4) -> (2)', 1),
        (@MaCH, N'B', N'(1) -> (3) -> (2) -> (4)', 0),
        (@MaCH, N'C', N'(3) -> (4) -> (1) -> (2)', 0),
        (@MaCH, N'D', N'(2) -> (3) -> (1) -> (4)', 0);
END
GO

-- Câu hỏi SEED_EAI_021 (Nghe - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_021')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 1, N'TracNghiem', N'Nghe', N'De', N'Where in a clothing store do you go to test if garments fit you?',
            NULL, NULL, N'''Fitting room'' (hoặc ''dressing room'') là phòng thử đồ trong cửa hàng thời trang.', N'''Fitting room'' (hoặc ''dressing room'') là phòng thử đồ trong cửa hàng thời trang.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 4 - Từ vựng mua sắm: Fitting room: Phòng thử quần áo.', N'Extracted', N'BaiHoc_04_ShoppingReturns.txt', 4, 'SEED_EAI_021');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Cash register', 0),
        (@MaCH, N'B', N'Fitting room', 1),
        (@MaCH, N'C', N'Warehouse', 0),
        (@MaCH, N'D', N'Showcase', 0);
END
GO

-- Câu hỏi SEED_EAI_022 (TuVung - Trình độ 1 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_022')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 1, N'DienKhuyet', N'TuVung', N'De', N'Complete the sentence: ''Excuse me, do you have this shirt in a larger _______?''',
            NULL, NULL, N'Trong mua sắm quần áo, kích cỡ dùng từ ''size'' (size S, M, L, XL).', N'Trong mua sắm quần áo, kích cỡ dùng từ ''size'' (size S, M, L, XL).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 4 - Mẫu câu hỏi kích cỡ: ''Do you have this in size M / a larger size?''', N'Extracted', N'BaiHoc_04_ShoppingReturns.txt', 4, 'SEED_EAI_022');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'size', 1),
        (@MaCH, N'B', N'length', 0),
        (@MaCH, N'C', N'volume', 0),
        (@MaCH, N'D', N'mass', 0);
END
GO

-- Câu hỏi SEED_EAI_023 (Doc - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_023')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 1, N'TracNghiem', N'Doc', N'De', N'What official document must you show to the store clerk to request a refund or return?',
            NULL, NULL, N'''Receipt'' là hóa đơn mua lẻ cần xuất trình khi đổi trả sản phẩm.', N'''Receipt'' là hóa đơn mua lẻ cần xuất trình khi đổi trả sản phẩm.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 4 - Chính sách đổi trả: Phải giữ lại ''receipt'' (hóa đơn) trong thời hạn bảo hành.', N'Extracted', N'BaiHoc_04_ShoppingReturns.txt', 4, 'SEED_EAI_023');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Menu', 0),
        (@MaCH, N'B', N'Receipt', 1),
        (@MaCH, N'C', N'Ticket', 0),
        (@MaCH, N'D', N'Leaflet', 0);
END
GO

-- Câu hỏi SEED_EAI_024 (TuVung - Trình độ 1 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_024')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 1, N'DungSai', N'TuVung', N'De', N'True or False: ''A discount of 20%'' means the customer will pay 20% more than the original price.',
            NULL, NULL, N'Sai. ''Discount'' có nghĩa là giảm giá, khách hàng được giảm 20% so với giá gốc.', N'Sai. ''Discount'' có nghĩa là giảm giá, khách hàng được giảm 20% so với giá gốc.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 4 - Thuật ngữ khuyến mãi: Discount / Sale off: Giảm giá.', N'Extracted', N'BaiHoc_04_ShoppingReturns.txt', 4, 'SEED_EAI_024');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_025 (NguPhap - Trình độ 1 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_025')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 1, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the mistake: ''I would like to return this jacket because it don''t fit me.''',
            NULL, NULL, N'Chủ ngữ ''it'' là ngôi thứ ba số ít, trợ động từ phủ định ở hiện tại đơn phải là ''doesn''t'', không dùng ''don''t''.', N'Chủ ngữ ''it'' là ngôi thứ ba số ít, trợ động từ phủ định ở hiện tại đơn phải là ''doesn''t'', không dùng ''don''t''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 4 & Ngữ pháp ngôi thứ 3: ''it doesn''t fit''.', N'Applied', N'BaiHoc_04_ShoppingReturns.txt', 4, 'SEED_EAI_025');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''would like'' should be ''wanting''', 0),
        (@MaCH, N'B', N'''don''t'' should be ''doesn''t''', 1),
        (@MaCH, N'C', N'''jacket'' should be ''jackets''', 0),
        (@MaCH, N'D', N'''fit'' should be ''fitting''', 0);
END
GO

-- Câu hỏi SEED_EAI_026 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_026')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'Which adjective best describes food with a hot burning sensation caused by chili peppers?',
            NULL, NULL, N'''Spicy'' nghĩa là cay nồng do ớt hoặc tiêu. ''Sweet'' là ngọt, ''Sour'' là chua, ''Bitter'' là đắng.', N'''Spicy'' nghĩa là cay nồng do ớt hoặc tiêu. ''Sweet'' là ngọt, ''Sour'' là chua, ''Bitter'' là đắng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 5 - Từ vựng miêu tả vị giác: Spicy: Cay nồng; Sour: Chua; Savory: Đậm đà thơm ngon.', N'Extracted', N'BaiHoc_05_StreetFoodCulture.txt', 5, 'SEED_EAI_026');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Savory', 0),
        (@MaCH, N'B', N'Spicy', 1),
        (@MaCH, N'C', N'Crispy', 0),
        (@MaCH, N'D', N'Bitter', 0);
END
GO

-- Câu hỏi SEED_EAI_027 (TuVung - Trình độ 3 - Dạng NoiCap - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_027')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 3, N'NoiCap', N'TuVung', N'TrungBinh', N'Match each sensory adjective with its texture / taste description:
1. ''Crispy''
2. ''Savory''
3. ''Tender''',
            NULL, NULL, N'1-Crispy: giòn tan khi nhai; 2-Savory: đậm đà thơm ngon (thịt/gia vị); 3-Tender: mềm mọng dễ cắt (thịt chín vừa).', N'1-Crispy: giòn tan khi nhai; 2-Savory: đậm đà thơm ngon (thịt/gia vị); 3-Tender: mềm mọng dễ cắt (thịt chín vừa).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 5 - Bảng tính từ cảm giác (Texture & Taste Descriptors).', N'Extracted', N'BaiHoc_05_StreetFoodCulture.txt', 5, 'SEED_EAI_027');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'1-Crunchy and breaking easily, 2-Pleasant salty/meaty flavor, 3-Soft and easy to cut/chew', 1),
        (@MaCH, N'B', N'1-Sweet, 2-Soft, 3-Spicy', 0),
        (@MaCH, N'C', N'1-Liquid, 2-Sour, 3-Hard', 0),
        (@MaCH, N'D', N'1-Bitter, 2-Sweet, 3-Rough', 0);
END
GO

-- Câu hỏi SEED_EAI_028 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_028')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 3, N'TracNghiem', N'Doc', N'TrungBinh', N'Why is Vietnamese street food (such as Banh Mi and Pho) celebrated globally according to culinary reviews?',
            NULL, NULL, N'Ẩm thực đường phố Việt Nam nổi tiếng nhờ sự cân bằng hoàn hảo giữa rau thơm tươi, nước dùng đậm đà và độ giòn.', N'Ẩm thực đường phố Việt Nam nổi tiếng nhờ sự cân bằng hoàn hảo giữa rau thơm tươi, nước dùng đậm đà và độ giòn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 5 - Đọc hiểu văn hóa ẩm thực đường phố Việt Nam: ''Harmonious balance of herbs, savory broth, and fresh ingredients.''', N'Extracted', N'BaiHoc_05_StreetFoodCulture.txt', 5, 'SEED_EAI_028');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Because it uses excessive dairy products.', 0),
        (@MaCH, N'B', N'Due to its harmonious balance of fresh herbs, savory flavors, and aromatic spices.', 1),
        (@MaCH, N'C', N'Because it is completely unspiced.', 0),
        (@MaCH, N'D', N'Because it can only be cooked in industrial ovens.', 0);
END
GO

-- Câu hỏi SEED_EAI_029 (TuVung - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_029')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 3, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Complete the sentence: ''The grilled pork skewers have an irresistible smoky _______ that attracts tourists.''',
            NULL, NULL, N'''Aroma'' nghĩa là mùi thơm đặc trưng hấp dẫn của thức ăn nướng/nấu chín.', N'''Aroma'' nghĩa là mùi thơm đặc trưng hấp dẫn của thức ăn nướng/nấu chín.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 5 - Từ vựng khứu giác: Smoky aroma: Mùi thơm khói nướng hấp dẫn.', N'Extracted', N'BaiHoc_05_StreetFoodCulture.txt', 5, 'SEED_EAI_029');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'aroma', 1),
        (@MaCH, N'B', N'noise', 0),
        (@MaCH, N'C', N'texture', 0),
        (@MaCH, N'D', N'weight', 0);
END
GO

-- Câu hỏi SEED_EAI_030 (TuVung - Trình độ 3 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_030')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 3, N'DungSai', N'TuVung', N'De', N'True or False: ''Mouth-watering'' is an idiom used to describe food that looks or smells so delicious it makes your saliva flow.',
            NULL, NULL, N'Đúng. ''Mouth-watering'' có nghĩa là ngon chảy nước miếng, kích thích vị giác.', N'Đúng. ''Mouth-watering'' có nghĩa là ngon chảy nước miếng, kích thích vị giác.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 5 - Thành ngữ ẩm thực: ''Mouth-watering food''.', N'Extracted', N'BaiHoc_05_StreetFoodCulture.txt', 5, 'SEED_EAI_030');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_031 (Nghe - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_031')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'TracNghiem', N'Nghe', N'De', N'In the audio listening text ''Nerry Describe Family'', where is Neri originally from?',
            N'Hello, my name is Neri and I am from Belize. My question today is describe a family member. The family member that I am going to describe is my little sister.', NULL, N'Đoạn transcript nói rõ: ''Hello, my name is Neri and I am from Belize.''', N'Đoạn transcript nói rõ: ''Hello, my name is Neri and I am from Belize.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 27 - Đoạn mở đầu transcript: ''Hello, my name is Neri and I am from Belize.''', N'Extracted', N'BaiHoc_27_NerryFamily.txt', 27, 'SEED_EAI_031');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Brazil', 0),
        (@MaCH, N'B', N'Belize', 1),
        (@MaCH, N'C', N'Belgium', 0),
        (@MaCH, N'D', N'Bolivia', 0);
END
GO

-- Câu hỏi SEED_EAI_032 (Nghe - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_032')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'TracNghiem', N'Nghe', N'De', N'Which family member does Neri choose to describe?',
            N'The family member that I am going to describe is my little sister. She is sixteen years old and she attends high school.', NULL, N'Neri mô tả em gái của mình: ''The family member that I am going to describe is my little sister.''', N'Neri mô tả em gái của mình: ''The family member that I am going to describe is my little sister.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 27 - Câu 2: ''The family member that I am going to describe is my little sister.''', N'Extracted', N'BaiHoc_27_NerryFamily.txt', 27, 'SEED_EAI_032');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Her grandmother', 0),
        (@MaCH, N'B', N'Her elder brother', 0),
        (@MaCH, N'C', N'Her little sister', 1),
        (@MaCH, N'D', N'Her mother', 0);
END
GO

-- Câu hỏi SEED_EAI_033 (NguPhap - Trình độ 1 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_033')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'DienKhuyet', N'NguPhap', N'De', N'Complete the sentence based on family descriptions: ''My sister _______ sixteen years old.''',
            NULL, NULL, N'Động từ ''to be'' đi với chủ ngữ ngôi thứ 3 số ít ''My sister'' ở hiện tại đơn là ''is''.', N'Động từ ''to be'' đi với chủ ngữ ngôi thứ 3 số ít ''My sister'' ở hiện tại đơn là ''is''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 27 - Cấu trúc miêu tả tuổi tác: ''She is sixteen years old.''', N'Extracted', N'BaiHoc_27_NerryFamily.txt', 27, 'SEED_EAI_033');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'is', 1),
        (@MaCH, N'B', N'are', 0),
        (@MaCH, N'C', N'am', 0),
        (@MaCH, N'D', N'be', 0);
END
GO

-- Câu hỏi SEED_EAI_034 (TuVung - Trình độ 1 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_034')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'TracNghiem', N'TuVung', N'De', N'What is the correct English term for your mother''s or father''s sister?',
            NULL, NULL, N'Chị gái hoặc em gái của bố mẹ được gọi là ''aunt'' (cô, dì, bác gái).', N'Chị gái hoặc em gái của bố mẹ được gọi là ''aunt'' (cô, dì, bác gái).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 27 - Mở rộng từ vựng cây gia đình (Family Tree Vocabulary).', N'Applied', N'BaiHoc_27_NerryFamily.txt', 27, 'SEED_EAI_034');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Cousin', 0),
        (@MaCH, N'B', N'Aunt', 1),
        (@MaCH, N'C', N'Niece', 0),
        (@MaCH, N'D', N'Sibling', 0);
END
GO

-- Câu hỏi SEED_EAI_035 (TuVung - Trình độ 1 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_035')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'DungSai', N'TuVung', N'De', N'True or False: The word ''siblings'' refers to brothers and sisters.',
            NULL, NULL, N'Đúng. ''Siblings'' là danh từ chỉ anh chị em ruột trong gia đình.', N'Đúng. ''Siblings'' là danh từ chỉ anh chị em ruột trong gia đình.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 27 - Từ vựng thành viên gia đình: ''Siblings = brothers and sisters.''', N'Extracted', N'BaiHoc_27_NerryFamily.txt', 27, 'SEED_EAI_035');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_036 (NguPhap - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_036')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'TracNghiem', N'NguPhap', N'De', N'Read the excerpt: ''Woman: What does your mom do? - Man: She _______ women''s clothing.'' Choose the correct verb form:',
            N'Woman: What does your mom do?
Man: She sells women''s clothing.
Woman: Oh, yeah. Where does she work?
Man: She has a small shop in town.', NULL, N'Chủ ngữ ''She'' là ngôi thứ 3 số ít, động từ ''sell'' chia ở hiện tại đơn thêm ''s'' thành ''sells''.', N'Chủ ngữ ''She'' là ngôi thứ 3 số ít, động từ ''sell'' chia ở hiện tại đơn thêm ''s'' thành ''sells''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 28 - Hội thoại 1: ''She sells women''s clothing.''', N'Extracted', N'BaiHoc_28_ThirdPersonSingular.txt', 28, 'SEED_EAI_036');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'sell', 0),
        (@MaCH, N'B', N'sells', 1),
        (@MaCH, N'C', N'selling', 0),
        (@MaCH, N'D', N'is sell', 0);
END
GO

-- Câu hỏi SEED_EAI_037 (NguPhap - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_037')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'TracNghiem', N'NguPhap', N'De', N'In the dialogue: ''Where does she work? - She _______ a small shop in town.''',
            NULL, NULL, N'Động từ bất quy tắc ''have'' chia ở ngôi thứ 3 số ít thành ''has''.', N'Động từ bất quy tắc ''have'' chia ở ngôi thứ 3 số ít thành ''has''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 28 - Hội thoại 1: ''She has a small shop in town.''', N'Extracted', N'BaiHoc_28_ThirdPersonSingular.txt', 28, 'SEED_EAI_037');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'has', 1),
        (@MaCH, N'B', N'haves', 0),
        (@MaCH, N'C', N'have', 0),
        (@MaCH, N'D', N'having', 0);
END
GO

-- Câu hỏi SEED_EAI_038 (NguPhap - Trình độ 1 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_038')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'DienKhuyet', N'NguPhap', N'De', N'Complete the negative sentence: ''My brother _______ not like spicy food.''',
            NULL, NULL, N'Trợ động từ phủ định ngôi thứ ba số ít là ''does''. ''does not like'' = ''doesn''t like''.', N'Trợ động từ phủ định ngôi thứ ba số ít là ''does''. ''does not like'' = ''doesn''t like''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 28 - Quy tắc trợ động từ phủ định ngôi thứ 3 số ít: does not + V-infinitive.', N'Extracted', N'BaiHoc_28_ThirdPersonSingular.txt', 28, 'SEED_EAI_038');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'does', 1),
        (@MaCH, N'B', N'do', 0),
        (@MaCH, N'C', N'is', 0),
        (@MaCH, N'D', N'are', 0);
END
GO

-- Câu hỏi SEED_EAI_039 (NguPhap - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_039')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'TracNghiem', N'NguPhap', N'De', N'Which verb takes ''-es'' in the third person singular form?',
            NULL, NULL, N'Các động từ tận cùng bằng -ch, -sh, -s, -x, -o thêm -es: watch -> watches, go -> goes.', N'Các động từ tận cùng bằng -ch, -sh, -s, -x, -o thêm -es: watch -> watches, go -> goes.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 28 - Quy tắc chính tả đuôi -s / -es ở ngôi thứ 3 số ít.', N'Extracted', N'BaiHoc_28_ThirdPersonSingular.txt', 28, 'SEED_EAI_039');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'play', 0),
        (@MaCH, N'B', N'watch', 1),
        (@MaCH, N'C', N'read', 0),
        (@MaCH, N'D', N'eat', 0);
END
GO

-- Câu hỏi SEED_EAI_040 (NguPhap - Trình độ 1 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_040')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the grammatical error: ''Does he lives in an apartment or in a house?''',
            NULL, NULL, N'Khi đã dùng trợ động từ ''Does'' trong câu hỏi, động từ chính phải ở dạng nguyên mẫu không ''s'': ''Does he live...''.', N'Khi đã dùng trợ động từ ''Does'' trong câu hỏi, động từ chính phải ở dạng nguyên mẫu không ''s'': ''Does he live...''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 28 - Lưu ý câu hỏi: Does + S + V(nguyên mẫu).', N'Applied', N'BaiHoc_28_ThirdPersonSingular.txt', 28, 'SEED_EAI_040');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''Does'' should be ''Do''', 0),
        (@MaCH, N'B', N'''lives'' should be ''live''', 1),
        (@MaCH, N'C', N'''in'' should be ''on''', 0),
        (@MaCH, N'D', N'''an'' should be ''a''', 0);
END
GO

-- Câu hỏi SEED_EAI_041 (Noi - Trình độ 1 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_041')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 1, N'TracNghiem', N'Noi', N'De', N'You accidentally bump into someone on the subway. What is the quickest and most polite response?',
            NULL, NULL, N'Lời xin lỗi phản xạ nhanh và lịch sự nhất trong giao tiếp đời thường là ''Oh, I''m so sorry!'' hoặc ''Excuse me!''.', N'Lời xin lỗi phản xạ nhanh và lịch sự nhất trong giao tiếp đời thường là ''Oh, I''m so sorry!'' hoặc ''Excuse me!''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 0 - Phản xạ giao tiếp lịch sự hàng ngày.', N'Applied', N'BaiHoc_00_LaoQuen_GioiThieu.txt', 0, 'SEED_EAI_041');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Watch out yourself!', 0),
        (@MaCH, N'B', N'I''m so sorry, excuse me!', 1),
        (@MaCH, N'C', N'No problem at all.', 0),
        (@MaCH, N'D', N'You are welcome.', 0);
END
GO

-- Câu hỏi SEED_EAI_042 (Doc - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_042')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 2, N'TracNghiem', N'Doc', N'TrungBinh', N'Read the restaurant sign: ''Reservations recommended for dinner on weekends.'' What does it mean?',
            NULL, NULL, N'Bảng hiệu thông báo thực khách nên gọi điện đặt bàn trước nếu muốn ăn tối vào các ngày cuối tuần.', N'Bảng hiệu thông báo thực khách nên gọi điện đặt bàn trước nếu muốn ăn tối vào các ngày cuối tuần.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 3 - Đọc hiểu thông báo nhà hàng: ''Reservations recommended''.', N'Extracted', N'BaiHoc_03_DiningOut.txt', 3, 'SEED_EAI_042');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Dinner is free on weekends.', 0),
        (@MaCH, N'B', N'You should book a table in advance for weekend dinner.', 1),
        (@MaCH, N'C', N'The restaurant is closed on weekends.', 0),
        (@MaCH, N'D', N'Only two people can dine together.', 0);
END
GO

-- Câu hỏi SEED_EAI_043 (Viet - Trình độ 1 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_043')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 1, N'VietLaiCau', N'Viet', N'De', N'Rewrite the sentence politely: ''I want water.''',
            NULL, NULL, N'''Could I have some water, please?'' hoặc ''I would like some water, please'' thể hiện sự lịch sự hơn hẳn ''I want''.', N'''Could I have some water, please?'' hoặc ''I would like some water, please'' thể hiện sự lịch sự hơn hẳn ''I want''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 3 - Văn hóa giao tiếp lịch sự: ''Could I have... please?'' thay cho ''I want''.', N'Applied', N'BaiHoc_03_DiningOut.txt', 3, 'SEED_EAI_043');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Could I have some water, please?', 1),
        (@MaCH, N'B', N'Water is what I want.', 0),
        (@MaCH, N'C', N'Give me water now.', 0),
        (@MaCH, N'D', N'I need water fast.', 0);
END
GO

-- Câu hỏi SEED_EAI_044 (TuVung - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_044')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 2, N'TracNghiem', N'TuVung', N'De', N'When paying for meals with friends, what idiom means everyone pays for their own share?',
            NULL, NULL, N'''Go Dutch'' là thành ngữ phổ biến nghĩa là ''tiền ai người nấy trả'' (chia đôi hoặc chia theo phần ăn).', N'''Go Dutch'' là thành ngữ phổ biến nghĩa là ''tiền ai người nấy trả'' (chia đôi hoặc chia theo phần ăn).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 3 - Thành ngữ thanh toán: ''Go Dutch / Split the bill''.', N'Extracted', N'BaiHoc_03_DiningOut.txt', 3, 'SEED_EAI_044');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Go French', 0),
        (@MaCH, N'B', N'Go Dutch', 1),
        (@MaCH, N'C', N'Pay double', 0),
        (@MaCH, N'D', N'Leave the money', 0);
END
GO

-- Câu hỏi SEED_EAI_045 (Doc - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_045')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 2, N'TracNghiem', N'Doc', N'De', N'Which closing sign-off is best for a formal letter to someone whose name you know (e.g., Dear Mr. Miller)?',
            NULL, NULL, N'Quy tắc chuẩn văn thư Anh Quốc: Nếu thư bắt đầu bằng ''Dear [Tên]'', kết thư dùng ''Yours sincerely''. Nếu bắt đầu ''Dear Sir/Madam'', kết thư dùng ''Yours faithfully''.', N'Quy tắc chuẩn văn thư Anh Quốc: Nếu thư bắt đầu bằng ''Dear [Tên]'', kết thư dùng ''Yours sincerely''. Nếu bắt đầu ''Dear Sir/Madam'', kết thư dùng ''Yours faithfully''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 1 - Quy tắc viết thư trang trọng: Dear Mr. X -> Yours sincerely.', N'Extracted', N'BaiHoc_01_Formal_Informal.txt', 1, 'SEED_EAI_045');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Cheers,', 0),
        (@MaCH, N'B', N'Yours sincerely,', 1),
        (@MaCH, N'C', N'See ya,', 0),
        (@MaCH, N'D', N'Later,', 0);
END
GO

-- Câu hỏi SEED_EAI_046 (NguPhap - Trình độ 1 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_046')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'TracNghiem', N'NguPhap', N'De', N'Choose the grammatically correct sentence about daily routines:',
            NULL, NULL, N'''My father drinks coffee every morning.'' tuân thủ đúng quy tắc chia động từ ngôi thứ 3 số ít ''drinks''.', N'''My father drinks coffee every morning.'' tuân thủ đúng quy tắc chia động từ ngôi thứ 3 số ít ''drinks''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 28 - Thói quen hàng ngày ở thì hiện tại đơn.', N'Applied', N'BaiHoc_28_ThirdPersonSingular.txt', 28, 'SEED_EAI_046');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'My father drink coffee every morning.', 0),
        (@MaCH, N'B', N'My father drinks coffee every morning.', 1),
        (@MaCH, N'C', N'My father drinking coffee every morning.', 0),
        (@MaCH, N'D', N'My father is drink coffee every morning.', 0);
END
GO

-- Câu hỏi SEED_EAI_047 (TuVung - Trình độ 1 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_047')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 1, N'DienKhuyet', N'TuVung', N'De', N'Fill in the blank: ''Can I pay _______ credit card or cash?''',
            NULL, NULL, N'Giới từ đi với phương thức thanh toán thẻ tín dụng là ''by'': ''pay by credit card''.', N'Giới từ đi với phương thức thanh toán thẻ tín dụng là ''by'': ''pay by credit card''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 4 - Phương thức thanh toán: Pay by card / in cash.', N'Extracted', N'BaiHoc_04_ShoppingReturns.txt', 4, 'SEED_EAI_047');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'by', 1),
        (@MaCH, N'B', N'on', 0),
        (@MaCH, N'C', N'with', 0),
        (@MaCH, N'D', N'at', 0);
END
GO

-- Câu hỏi SEED_EAI_048 (TuVung - Trình độ 1 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_048')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (20, 1, N'TracNghiem', N'TuVung', N'De', N'How do you refer to your husband''s or wife''s mother in English?',
            NULL, NULL, N'Mẹ của vợ hoặc chồng là ''mother-in-law'' (mẹ vợ / mẹ chồng).', N'Mẹ của vợ hoặc chồng là ''mother-in-law'' (mẹ vợ / mẹ chồng).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 27 - Mối quan hệ họ hàng qua hôn nhân (-in-law).', N'Applied', N'BaiHoc_27_NerryFamily.txt', 27, 'SEED_EAI_048');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Step-mother', 0),
        (@MaCH, N'B', N'Mother-in-law', 1),
        (@MaCH, N'C', N'Godmother', 0),
        (@MaCH, N'D', N'Foster mother', 0);
END
GO

-- Câu hỏi SEED_EAI_049 (Noi - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_049')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (3, 2, N'TracNghiem', N'Noi', N'De', N'When a sales assistant asks ''May I help you?'', what is the standard polite reply if you just want to browse?',
            NULL, NULL, N'Mẫu câu văn hóa khi chỉ muốn xem đồ: ''No thank you, I''m just browsing / looking around.''', N'Mẫu câu văn hóa khi chỉ muốn xem đồ: ''No thank you, I''m just browsing / looking around.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 4 - Phản hồi nhân viên bán hàng: ''I''m just browsing, thank you.''', N'Extracted', N'BaiHoc_04_ShoppingReturns.txt', 4, 'SEED_EAI_049');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Don''t bother me.', 0),
        (@MaCH, N'B', N'I''m just browsing, thank you.', 1),
        (@MaCH, N'C', N'Go away please.', 0),
        (@MaCH, N'D', N'I don''t need your voice.', 0);
END
GO

-- Câu hỏi SEED_EAI_050 (Noi - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_050')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (2, 1, N'TracNghiem', N'Noi', N'De', N'Which response is appropriate when someone says ''Thank you very much for your help!''?',
            NULL, NULL, N'Đáp lại lời cảm ơn chuẩn mực là ''You''re welcome!'' hoặc ''My pleasure!''.', N'Đáp lại lời cảm ơn chuẩn mực là ''You''re welcome!'' hoặc ''My pleasure!''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 0 - Phản hồi lời cảm ơn: ''You''re welcome / Not at all / My pleasure.''', N'Extracted', N'BaiHoc_00_LaoQuen_GioiThieu.txt', 0, 'SEED_EAI_050');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'You''re welcome!', 1),
        (@MaCH, N'B', N'Yes, of course I did.', 0),
        (@MaCH, N'C', N'Nothing special for you.', 0),
        (@MaCH, N'D', N'Don''t say thank you.', 0);
END
GO

PRINT N'Đã hoàn thành nạp Batch 01 (SEED_EAI_001 -> SEED_EAI_050) thành công!';
GO
