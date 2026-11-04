-- ====================================================================================
-- SEED QUESTIONS BATCH: Batch 04 (SEED_EAI_151 -> SEED_EAI_200)
-- Số lượng câu hỏi: 50
-- Đảm bảo: Idempotent (Chạy lại nhiều lần không trùng lặp)
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Đang nạp Batch 04 (SEED_EAI_151 -> SEED_EAI_200) (50 câu hỏi)...';

-- Câu hỏi SEED_EAI_151 (Nghe - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_151')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'TracNghiem', N'Nghe', N'TrungBinh', N'In TOEIC Part 1, why is the statement ''The bicycle is being repaired'' incorrect if the photo shows bicycles parked on an empty street with no mechanic present?',
            N'Statement A: A man is fixing a bicycle wheel.
Statement B: Some boxes are stacked on the shelf.
Statement C: The woman is paying for merchandise.
Statement D: A pedestrian is crossing the street.', NULL, N'Trong TOEIC Part 1, cấu trúc bị động tiếp diễn ''is being + V3'' bắt buộc phải có người đang trực tiếp thực hiện hành động. Nếu không có người thao tác, câu này chắc chắn là bẫy.', N'Trong TOEIC Part 1, cấu trúc bị động tiếp diễn ''is being + V3'' bắt buộc phải có người đang trực tiếp thực hiện hành động. Nếu không có người thao tác, câu này chắc chắn là bẫy.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Mục 2: Bẫy Be + V-ing vs Be + being + V3: ''Tranh không có người thao tác nhưng đáp án dùng is being repaired là SAI.''', N'Extracted', N'01_TOEIC_Listening_Part1_Photographs.docx', NULL, 'SEED_EAI_151');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Because bicycles cannot be repaired anywhere.', 0),
        (@MaCH, N'B', N'Because ''is being + V3'' requires a human subject actively performing the action in the photograph.', 1),
        (@MaCH, N'C', N'Because bicycles are always plural.', 0),
        (@MaCH, N'D', N'Because ''repair'' is an informal verb.', 0);
END
GO

-- Câu hỏi SEED_EAI_152 (Nghe - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_152')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'TracNghiem', N'Nghe', N'De', N'Which common pronunciation trap does ETS use in TOEIC Part 1 to confuse listeners with similar sounding words?',
            NULL, NULL, N'ETS thường dùng các cặp từ phát âm gần giống nhau (similar sounding words) như walking/working, coffee/copy để đánh bẫy thí sinh.', N'ETS thường dùng các cặp từ phát âm gần giống nhau (similar sounding words) như walking/working, coffee/copy để đánh bẫy thí sinh.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Mục 2: Bẫy từ đồng âm / gần âm: walking vs working, coffee vs copy.', N'Extracted', N'01_TOEIC_Listening_Part1_Photographs.docx', NULL, 'SEED_EAI_152');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Walking vs. Working', 1),
        (@MaCH, N'B', N'Table vs. Chair', 0),
        (@MaCH, N'C', N'Elephant vs. Cat', 0),
        (@MaCH, N'D', N'Computer vs. Mouse', 0);
END
GO

-- Câu hỏi SEED_EAI_153 (Nghe - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_153')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'TracNghiem', N'Nghe', N'TrungBinh', N'Look at the sample statement from Part 1: ''Some boxes are stacked on the shelf.'' What does ''stacked'' describe?',
            NULL, NULL, N'''Stacked'' miêu tả trạng thái các hộp được xếp chồng ngay ngắn lên nhau trên kệ.', N'''Stacked'' miêu tả trạng thái các hộp được xếp chồng ngay ngắn lên nhau trên kệ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Mục 3: Ví dụ mẫu: ''Statement B: Some boxes are stacked on the shelf.''', N'Extracted', N'01_TOEIC_Listening_Part1_Photographs.docx', NULL, 'SEED_EAI_153');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Boxes being torn apart', 0),
        (@MaCH, N'B', N'Boxes neatly arranged in a pile on top of one another', 1),
        (@MaCH, N'C', N'Boxes falling down the stairs', 0),
        (@MaCH, N'D', N'Boxes being burnt', 0);
END
GO

-- Câu hỏi SEED_EAI_154 (Nghe - Trình độ 3 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_154')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'DungSai', N'Nghe', N'De', N'True or False: In TOEIC Part 1, an answer that makes an exaggerated assumption not visible in the picture (e.g., ''He is thinking about his wife'') is typically incorrect.',
            NULL, NULL, N'Đúng. TOEIC Part 1 chỉ chấp nhận các mô tả khách quan, nhìn thấy rõ ràng trên tranh. Mọi suy đoán cảm xúc hoặc ý nghĩ đều là đáp án sai.', N'Đúng. TOEIC Part 1 chỉ chấp nhận các mô tả khách quan, nhìn thấy rõ ràng trên tranh. Mọi suy đoán cảm xúc hoặc ý nghĩ đều là đáp án sai.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Nguyên tắc chọn đáp án miêu tả khách quan trực quan.', N'Extracted', N'01_TOEIC_Listening_Part1_Photographs.docx', NULL, 'SEED_EAI_154');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_155 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_155')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'Which verb is used in TOEIC Part 1 to describe a person who is wearing a watch or glasses (state of wearing)?',
            NULL, NULL, N'Trong TOEIC, ''is wearing'' miêu tả trạng thái đang mặc/đeo đồ, còn ''is putting on'' miêu tả hành động đang xỏ tay vào áo hay đeo kính.', N'Trong TOEIC, ''is wearing'' miêu tả trạng thái đang mặc/đeo đồ, còn ''is putting on'' miêu tả hành động đang xỏ tay vào áo hay đeo kính.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Phân biệt Wearing (trạng thái) vs. Putting on (hành động).', N'Extracted', N'01_TOEIC_Listening_Part1_Photographs.docx', NULL, 'SEED_EAI_155');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'is putting on', 0),
        (@MaCH, N'B', N'is wearing', 1),
        (@MaCH, N'C', N'is taking off', 0),
        (@MaCH, N'D', N'is holding up', 0);
END
GO

-- Câu hỏi SEED_EAI_156 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_156')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct word to complete the TOEIC sentence: ''The marketing department completed the project _______ despite tight deadlines.''',
            NULL, NULL, N'Cần một trạng từ (Adverb) để bổ nghĩa cho động từ ''completed''. ''successfully'' là trạng từ mang nghĩa ''một cách thành công''.', N'Cần một trạng từ (Adverb) để bổ nghĩa cho động từ ''completed''. ''successfully'' là trạng từ mang nghĩa ''một cách thành công''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Quy tắc vị trí trạng từ: Động từ + Tân ngữ + Trạng từ bổ nghĩa.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_156');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'success', 0),
        (@MaCH, N'B', N'successful', 0),
        (@MaCH, N'C', N'successfully', 1),
        (@MaCH, N'D', N'succeed', 0);
END
GO

-- Câu hỏi SEED_EAI_157 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_157')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Fill in the blank: ''Ms. Jenkins reviewed the financial audit _______ before submitting it to the board.''',
            NULL, NULL, N'Đại từ phản thân ''herself'' được dùng ở vị trí nhấn mạnh chủ ngữ: chính cô ấy đã kiểm tra bản kiểm toán.', N'Đại từ phản thân ''herself'' được dùng ở vị trí nhấn mạnh chủ ngữ: chính cô ấy đã kiểm tra bản kiểm toán.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Chuyên đề đại từ: Đại từ phản thân (Reflexive Pronouns) nhấn mạnh.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_157');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'she', 0),
        (@MaCH, N'B', N'her', 0),
        (@MaCH, N'C', N'herself', 1),
        (@MaCH, N'D', N'hers', 0);
END
GO

-- Câu hỏi SEED_EAI_158 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_158')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Select the appropriate conjunction: ''_______ the severe weather conditions, the cargo ship arrived at the port safely.''',
            NULL, NULL, N'''Despite'' hoặc ''In spite of'' đi với cụm danh từ (severe weather conditions) để chỉ sự nhượng bộ (mặc dù). ''Although'' phải đi với mệnh đề S+V.', N'''Despite'' hoặc ''In spite of'' đi với cụm danh từ (severe weather conditions) để chỉ sự nhượng bộ (mặc dù). ''Although'' phải đi với mệnh đề S+V.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Phân biệt Although / Even though (+ Clause) vs. Despite / In spite of (+ Noun phrase).', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_158');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Although', 0),
        (@MaCH, N'B', N'Despite', 1),
        (@MaCH, N'C', N'Because', 0),
        (@MaCH, N'D', N'Unless', 0);
END
GO

-- Câu hỏi SEED_EAI_159 (TuVung - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_159')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Complete the corporate sentence: ''All employees are required to comply _______ the updated safety regulations.''',
            NULL, NULL, N'Cụm động từ cố định trong TOEIC: ''comply with'' nghĩa là ''tuân thủ quy định/luật lệ''.', N'Cụm động từ cố định trong TOEIC: ''comply with'' nghĩa là ''tuân thủ quy định/luật lệ''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Cụm động từ kèm giới từ thông dụng: Comply with, Adhere to, Conform to.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_159');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'with', 1),
        (@MaCH, N'B', N'for', 0),
        (@MaCH, N'C', N'on', 0),
        (@MaCH, N'D', N'at', 0);
END
GO

-- Câu hỏi SEED_EAI_160 (NguPhap - Trình độ 3 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_160')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the error: ''The manager was pleased with how quick the staff responded to the inquiry.''',
            NULL, NULL, N'Bổ nghĩa cho động từ ''responded'' phải là một phó từ: ''quickly'', không thể dùng tính từ ''quick''.', N'Bổ nghĩa cho động từ ''responded'' phải là một phó từ: ''quickly'', không thể dùng tính từ ''quick''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Lỗi sai từ loại Tính từ vs Trạng từ trong Part 5.', N'Applied', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_160');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''pleased'' should be ''pleasure''', 0),
        (@MaCH, N'B', N'''quick'' should be ''quickly''', 1),
        (@MaCH, N'C', N'''with'' should be ''by''', 0),
        (@MaCH, N'D', N'''inquiry'' should be ''inquire''', 0);
END
GO

-- Câu hỏi SEED_EAI_161 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_161')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'TracNghiem', N'Doc', N'TrungBinh', N'According to the mini test reading passage, why was the annual leadership banquet rescheduled?',
            N'MEMORANDUM
To: All Department Heads
From: Facilities Management
Date: October 12
Subject: Annual Banquet Rescheduling
Please note that due to ongoing electrical renovations in the Grand Ballroom, the annual leadership banquet originally scheduled for November 5 has been postponed to November 19.', NULL, N'Văn bản nêu rõ nguyên nhân dời lịch: ''due to ongoing electrical renovations in the Grand Ballroom'' (do đang cải tạo hệ thống điện).', N'Văn bản nêu rõ nguyên nhân dời lịch: ''due to ongoing electrical renovations in the Grand Ballroom'' (do đang cải tạo hệ thống điện).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Bài đọc thông báo nội bộ (Internal Memo).', N'Extracted', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_161');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Because of bad weather in another city', 0),
        (@MaCH, N'B', N'Due to electrical renovations taking place in the Grand Ballroom', 1),
        (@MaCH, N'C', N'Because the CEO cancelled the celebration', 0),
        (@MaCH, N'D', N'Because of food delivery problems', 0);
END
GO

-- Câu hỏi SEED_EAI_162 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_162')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'TracNghiem', N'Doc', N'TrungBinh', N'On what date will the leadership banquet actually take place?',
            NULL, NULL, N'Ngày mới được ấn định là: ''has been postponed to November 19''.', N'Ngày mới được ấn định là: ''has been postponed to November 19''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Thông tin ngày tổ chức mới.', N'Extracted', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_162');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'October 12', 0),
        (@MaCH, N'B', N'November 5', 0),
        (@MaCH, N'C', N'November 19', 1),
        (@MaCH, N'D', N'December 1', 0);
END
GO

-- Câu hỏi SEED_EAI_163 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_163')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the best verb tense: ''By the time the new regional director arrives next week, the team _______ the audit report.''',
            NULL, NULL, N'Cấu trúc tương lai hoàn thành: ''By the time + S + V(hiện tại đơn), S + will have + V3''. Ở đây dùng ''will have finalized''.', N'Cấu trúc tương lai hoàn thành: ''By the time + S + V(hiện tại đơn), S + will have + V3''. Ở đây dùng ''will have finalized''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Cấu trúc thì nâng cao với By the time.', N'Extracted', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_163');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'finalized', 0),
        (@MaCH, N'B', N'will have finalized', 1),
        (@MaCH, N'C', N'finalizes', 0),
        (@MaCH, N'D', N'has finalized', 0);
END
GO

-- Câu hỏi SEED_EAI_164 (TuVung - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_164')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 3, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Complete the corporate sentence: ''The HR department aims to attract highly _______ candidates for the software architect position.''',
            NULL, NULL, N'''highly qualified'' (đủ trình độ chuyên môn cao) là tính từ ghép thông dụng trong tuyển dụng.', N'''highly qualified'' (đủ trình độ chuyên môn cao) là tính từ ghép thông dụng trong tuyển dụng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Collocations tuyển dụng nhân sự: Highly qualified candidates.', N'Extracted', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_164');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'qualified', 1),
        (@MaCH, N'B', N'qualify', 0),
        (@MaCH, N'C', N'qualification', 0),
        (@MaCH, N'D', N'qualifyingly', 0);
END
GO

-- Câu hỏi SEED_EAI_165 (Doc - Trình độ 3 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_165')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'DungSai', N'Doc', N'De', N'True or False: In a TOEIC business memo, the ''Date'' line indicates the deadline by which the task must be completed.',
            NULL, NULL, N'Sai. Dòng ''Date'' ở đầu thông báo là ngày phát hành văn bản, còn hạn chót được nêu bằng từ ''Deadline'' hoặc ''Due date'' trong nội dung.', N'Sai. Dòng ''Date'' ở đầu thông báo là ngày phát hành văn bản, còn hạn chót được nêu bằng từ ''Deadline'' hoặc ''Due date'' trong nội dung.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Cấu trúc tiêu đề Memo thương mại.', N'Extracted', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_165');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_166 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_166')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'TracNghiem', N'Doc', N'Kho', N'In TOEIC Part 7 Double Passages, what is a cross-reference question?',
            NULL, NULL, N'Câu hỏi chéo (cross-reference question) yêu cầu thí sinh phải kết hợp thông tin từ cả 2 văn bản (ví dụ: email đặt hàng và bảng báo giá/hóa đơn) mới tìm ra đáp án.', N'Câu hỏi chéo (cross-reference question) yêu cầu thí sinh phải kết hợp thông tin từ cả 2 văn bản (ví dụ: email đặt hàng và bảng báo giá/hóa đơn) mới tìm ra đáp án.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Chiến lược câu hỏi liên kết thông tin 2 đoạn (Cross-reference).', N'Extracted', N'04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx', NULL, 'SEED_EAI_166');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A question about grammar rules only', 0),
        (@MaCH, N'B', N'A question requiring the reader to synthesize and link clues from both Passage 1 and Passage 2 to deduce the correct answer', 1),
        (@MaCH, N'C', N'A question asking for the font size of the text', 0),
        (@MaCH, N'D', N'A question that has two identical answers', 0);
END
GO

-- Câu hỏi SEED_EAI_167 (Doc - Trình độ 4 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_167')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'Read the double passage summary: Document 1 is a conference schedule; Document 2 is an email from Dr. Adams stating he can only attend on the afternoon of Day 2. Which workshop will Dr. Adams be able to attend?',
            NULL, NULL, N'Thí sinh phải đối chiếu lịch trình Ngày 2 buổi chiều trong Tài liệu 1 với nguyện vọng của Dr. Adams trong Tài liệu 2.', N'Thí sinh phải đối chiếu lịch trình Ngày 2 buổi chiều trong Tài liệu 1 với nguyện vọng của Dr. Adams trong Tài liệu 2.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Bài tập mẫu đối chiếu lịch trình hội nghị và thư xác nhận.', N'Applied', N'04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx', NULL, 'SEED_EAI_167');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Morning keynote on Day 1', 0),
        (@MaCH, N'B', N'The workshop scheduled for Day 2 after 1:00 PM', 1),
        (@MaCH, N'C', N'Breakfast meeting on Day 1', 0),
        (@MaCH, N'D', N'Opening ceremony on Day 1', 0);
END
GO

-- Câu hỏi SEED_EAI_168 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_168')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''The updated price list will supersede and _______ all previous promotional catalogs.''',
            NULL, NULL, N'''replace'' nghĩa là thay thế toàn bộ danh mục cũ.', N'''replace'' nghĩa là thay thế toàn bộ danh mục cũ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Từ vựng hợp đồng và biểu phí thương mại: Supersede / Replace.', N'Extracted', N'04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx', NULL, 'SEED_EAI_168');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'replace', 1),
        (@MaCH, N'B', N'break', 0),
        (@MaCH, N'C', N'hide', 0),
        (@MaCH, N'D', N'copy', 0);
END
GO

-- Câu hỏi SEED_EAI_169 (Doc - Trình độ 4 - Dạng TrueFalseNotGiven - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_169')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'TrueFalseNotGiven', N'Doc', N'TrungBinh', N'Based on double passages, determine: ''Dr. Adams requested reimbursement for his flight ticket.''',
            NULL, NULL, N'Nếu trong cả hai văn bản không hề nhắc đến việc hoàn trả tiền vé máy bay, câu trả lời chính xác là ''Not Given''.', N'Nếu trong cả hai văn bản không hề nhắc đến việc hoàn trả tiền vé máy bay, câu trả lời chính xác là ''Not Given''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Kỹ thuật nhận diện thông tin không được đề cập (Not Given).', N'Applied', N'04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx', NULL, 'SEED_EAI_169');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 0),
        (@MaCH, N'C', N'Not Given', 1);
END
GO

-- Câu hỏi SEED_EAI_170 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_170')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What is the recommended reading strategy for Double Passages in TOEIC Part 7 to save time?',
            NULL, NULL, N'Chiến lược tối ưu: Đọc lướt tiêu đề & người gửi của cả hai đoạn, đọc câu hỏi trước để xác định cần tìm manh mối ở văn bản 1 hay văn bản 2.', N'Chiến lược tối ưu: Đọc lướt tiêu đề & người gửi của cả hai đoạn, đọc câu hỏi trước để xác định cần tìm manh mối ở văn bản 1 hay văn bản 2.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Chiến lược quản lý thời gian 55 giây/câu trong Part 7.', N'Extracted', N'04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx', NULL, 'SEED_EAI_170');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Read every single word slowly three times before looking at questions.', 0),
        (@MaCH, N'B', N'Skim headings and context, read questions first, then scan for linked clues across both documents.', 1),
        (@MaCH, N'C', N'Guess all answers as B without reading.', 0),
        (@MaCH, N'D', N'Only read the first line of passage 1.', 0);
END
GO

-- Câu hỏi SEED_EAI_171 (NguPhap - Trình độ 3 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_171')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct word: ''The company provides an _______ training program for newly hired software engineers.''',
            NULL, NULL, N'Trước danh từ ''training program'' cần một tính từ. ''extensive'' (chuyên sâu, toàn diện) là tính từ phù hợp nghĩa.', N'Trước danh từ ''training program'' cần một tính từ. ''extensive'' (chuyên sâu, toàn diện) là tính từ phù hợp nghĩa.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Vị trí tính từ đứng trước danh từ.', N'Applied', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_171');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'extension', 0),
        (@MaCH, N'B', N'extensive', 1),
        (@MaCH, N'C', N'extensively', 0),
        (@MaCH, N'D', N'extend', 0);
END
GO

-- Câu hỏi SEED_EAI_172 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_172')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'Fill in the blank: ''Our customer support team is available _______ the clock to assist overseas clients.''',
            NULL, NULL, N'Thành ngữ thương mại ''around the clock'' nghĩa là 24/7, liên tục suốt ngày đêm.', N'Thành ngữ thương mại ''around the clock'' nghĩa là 24/7, liên tục suốt ngày đêm.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Thành ngữ thời gian trong dịch vụ khách hàng: Around the clock.', N'Extracted', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_172');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'around', 1),
        (@MaCH, N'B', N'beside', 0),
        (@MaCH, N'C', N'under', 0),
        (@MaCH, N'D', N'between', 0);
END
GO

-- Câu hỏi SEED_EAI_173 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_173')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Select the correct relative pronoun: ''The architect _______ designed the eco-friendly headquarters received an international award.''',
            NULL, NULL, N'Đại từ quan hệ thay thế cho danh từ chỉ người ''The architect'' làm chủ ngữ là ''who''.', N'Đại từ quan hệ thay thế cho danh từ chỉ người ''The architect'' làm chủ ngữ là ''who''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Mệnh đề quan hệ trong đề thi TOEIC: who, which, that, whose.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_173');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'which', 0),
        (@MaCH, N'B', N'who', 1),
        (@MaCH, N'C', N'whom', 0),
        (@MaCH, N'D', N'whose', 0);
END
GO

-- Câu hỏi SEED_EAI_174 (TuVung - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_174')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 3, N'DienKhuyet', N'TuVung', N'De', N'Fill in the blank: ''Payment must be received _______ 30 days of the invoice date.''',
            NULL, NULL, N'Giới từ ''within'' đi với khoảng thời gian: ''within 30 days'' (trong vòng 30 ngày).', N'Giới từ ''within'' đi với khoảng thời gian: ''within 30 days'' (trong vòng 30 ngày).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Cụm từ thanh toán: Within 30 days.', N'Extracted', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_174');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'within', 1),
        (@MaCH, N'B', N'among', 0),
        (@MaCH, N'C', N'between', 0),
        (@MaCH, N'D', N'along', 0);
END
GO

-- Câu hỏi SEED_EAI_175 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_175')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 4, N'TracNghiem', N'NguPhap', N'Kho', N'Choose the correct subjunctive form: ''The committee recommended that the contract _______ renegotiated immediately.''',
            NULL, NULL, N'Cấu trúc giả định (Subjunctive mood) sau động từ ''recommend that'' yêu cầu động từ ở dạng nguyên thể không chia: ''be renegotiated''.', N'Cấu trúc giả định (Subjunctive mood) sau động từ ''recommend that'' yêu cầu động từ ở dạng nguyên thể không chia: ''be renegotiated''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Thể giả định (Subjunctive Mood): recommend/require that S + (should) be + V3.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_175');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'is', 0),
        (@MaCH, N'B', N'be', 1),
        (@MaCH, N'C', N'was', 0),
        (@MaCH, N'D', N'will be', 0);
END
GO

-- Câu hỏi SEED_EAI_176 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_176')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'What is a ''vending machine''?',
            NULL, NULL, N'''Vending machine'' là máy bán hàng tự động (nước ngọt, đồ ăn nhẹ) hoạt động bằng tiền xu hoặc thanh toán thẻ.', N'''Vending machine'' là máy bán hàng tự động (nước ngọt, đồ ăn nhẹ) hoạt động bằng tiền xu hoặc thanh toán thẻ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Đồ vật phổ biến trong tranh văn phòng: Vending machine.', N'Extracted', N'01_TOEIC_Listening_Part1_Photographs.docx', NULL, 'SEED_EAI_176');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A machine that washes cars', 0),
        (@MaCH, N'B', N'An automated machine that dispenses snacks and drinks upon payment', 1),
        (@MaCH, N'C', N'A printer for printing badges', 0),
        (@MaCH, N'D', N'A vacuum cleaner', 0);
END
GO

-- Câu hỏi SEED_EAI_177 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_177')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'TracNghiem', N'Doc', N'TrungBinh', N'In TOEIC Part 6 (Text Completion), what type of item was added in the revised format?',
            NULL, NULL, N'Định dạng TOEIC mới bổ sung câu hỏi ''Sentence Insertion'' (chọn cả một câu hoàn chỉnh phù hợp nhất để điền vào đoạn văn).', N'Định dạng TOEIC mới bổ sung câu hỏi ''Sentence Insertion'' (chọn cả một câu hoàn chỉnh phù hợp nhất để điền vào đoạn văn).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Cấu trúc câu hỏi TOEIC hiện đại: Sentence Insertion.', N'Extracted', N'04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx', NULL, 'SEED_EAI_177');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Drawing a diagram', 0),
        (@MaCH, N'B', N'Inserting an entire sentence that fits the context logically', 1),
        (@MaCH, N'C', N'Translating into another language', 0),
        (@MaCH, N'D', N'Recording voice notes', 0);
END
GO

-- Câu hỏi SEED_EAI_178 (NguPhap - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_178')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'Fill in the blank with the correct preposition: ''The flight was cancelled due _______ technical difficulties.''',
            NULL, NULL, N'Cụm chỉ nguyên nhân: ''due to + Noun phrase'' (do / bởi vì).', N'Cụm chỉ nguyên nhân: ''due to + Noun phrase'' (do / bởi vì).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Cụm liên từ chỉ nguyên nhân: Due to / Because of / Owing to.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_178');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'to', 1),
        (@MaCH, N'B', N'of', 0),
        (@MaCH, N'C', N'for', 0),
        (@MaCH, N'D', N'with', 0);
END
GO

-- Câu hỏi SEED_EAI_179 (TuVung - Trình độ 3 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_179')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 3, N'TracNghiem', N'TuVung', N'De', N'Which word refers to an official reduction in the price of goods or services?',
            NULL, NULL, N'''Discount'' (chiết khấu, giảm giá).', N'''Discount'' (chiết khấu, giảm giá).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Từ vựng bán hàng và chiết khấu thương mại.', N'Applied', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_179');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Tax', 0),
        (@MaCH, N'B', N'Discount', 1),
        (@MaCH, N'C', N'Penalty', 0),
        (@MaCH, N'D', N'Interest', 0);
END
GO

-- Câu hỏi SEED_EAI_180 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_180')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 4, N'SuaLoiSai', N'NguPhap', N'Kho', N'Find the mistake: ''Hardly the manager had stepped into the room when the alarm went off.''',
            NULL, NULL, N'Khi trạng từ phủ định ''Hardly'' đứng đầu câu, phải thực hiện đảo ngữ: ''Hardly had the manager stepped...''.', N'Khi trạng từ phủ định ''Hardly'' đứng đầu câu, phải thực hiện đảo ngữ: ''Hardly had the manager stepped...''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Đảo ngữ với phó từ phủ định (Hardly... when).', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_180');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''Hardly the manager had'' should be ''Hardly had the manager''', 1),
        (@MaCH, N'B', N'''stepped'' should be ''stepping''', 0),
        (@MaCH, N'C', N'''when'' should be ''than''', 0),
        (@MaCH, N'D', N'''went off'' should be ''gone off''', 0);
END
GO

-- Câu hỏi SEED_EAI_181 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_181')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'TracNghiem', N'Doc', N'TrungBinh', N'What document lists items shipped to a customer along with prices and payment terms?',
            NULL, NULL, N'''Invoice'' (Hóa đơn thương mại) ghi rõ danh mục hàng hóa, đơn giá và điều khoản thanh toán.', N'''Invoice'' (Hóa đơn thương mại) ghi rõ danh mục hàng hóa, đơn giá và điều khoản thanh toán.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Các dạng văn bản Part 7: Invoice, Purchase Order.', N'Extracted', N'04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx', NULL, 'SEED_EAI_181');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Brochure', 0),
        (@MaCH, N'B', N'Invoice', 1),
        (@MaCH, N'C', N'Pamphlet', 0),
        (@MaCH, N'D', N'Itinerary', 0);
END
GO

-- Câu hỏi SEED_EAI_182 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_182')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Select the correct form: ''The CEO insisted on _______ the site inspection personally.''',
            NULL, NULL, N'Sau giới từ ''on'', động từ phải ở dạng V-ing: ''insisted on conducting''.', N'Sau giới từ ''on'', động từ phải ở dạng V-ing: ''insisted on conducting''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Động từ đi kèm giới từ + V-ing: insist on, succeed in.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_182');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'conduct', 0),
        (@MaCH, N'B', N'conducting', 1),
        (@MaCH, N'C', N'conducted', 0),
        (@MaCH, N'D', N'conductor', 0);
END
GO

-- Câu hỏi SEED_EAI_183 (TuVung - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_183')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 3, N'DienKhuyet', N'TuVung', N'De', N'Fill in the blank: ''Please retain your ticket as proof of _______.''',
            NULL, NULL, N'''proof of purchase'' là cụm cố định nghĩa là ''bằng chứng mua hàng''.', N'''proof of purchase'' là cụm cố định nghĩa là ''bằng chứng mua hàng''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Collocations bán lẻ: Proof of purchase.', N'Extracted', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_183');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'purchase', 1),
        (@MaCH, N'B', N'cooking', 0),
        (@MaCH, N'C', N'traveling', 0),
        (@MaCH, N'D', N'sleeping', 0);
END
GO

-- Câu hỏi SEED_EAI_184 (Nghe - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_184')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'TracNghiem', N'Nghe', N'De', N'In TOEIC Part 1, if a woman is walking down the stairs, what is the British / American synonym often heard?',
            NULL, NULL, N'''Descending the stairs'' là từ vựng học thuật thường dùng trong TOEIC để miêu tả đi xuống cầu thang.', N'''Descending the stairs'' là từ vựng học thuật thường dùng trong TOEIC để miêu tả đi xuống cầu thang.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Từ vựng chuyển động: Ascending (đi lên) vs. Descending (đi xuống).', N'Extracted', N'01_TOEIC_Listening_Part1_Photographs.docx', NULL, 'SEED_EAI_184');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Descending the stairs', 1),
        (@MaCH, N'B', N'Climbing up the roof', 0),
        (@MaCH, N'C', N'Sitting on a bench', 0),
        (@MaCH, N'D', N'Waiting at the curb', 0);
END
GO

-- Câu hỏi SEED_EAI_185 (NguPhap - Trình độ 3 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_185')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'DungSai', N'NguPhap', N'De', N'True or False: In business English, ''Information'' is an uncountable noun and can never take a plural ''-s''.',
            NULL, NULL, N'Đúng. ''Information'' là danh từ không đếm được, không bao giờ có dạng ''informations'' hay dùng với mạo từ ''an''.', N'Đúng. ''Information'' là danh từ không đếm được, không bao giờ có dạng ''informations'' hay dùng với mạo từ ''an''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Danh từ không đếm được thông dụng: Information, Equipment, Furniture.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_185');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_186 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_186')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What does ''unanimously'' mean in board meeting minutes: ''The board approved the merger unanimously.''?',
            NULL, NULL, N'''Unanimously'' nghĩa là nhất trí 100%, không có phiếu chống nào.', N'''Unanimously'' nghĩa là nhất trí 100%, không có phiếu chống nào.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Thuật ngữ biên bản họp hội đồng: Approved unanimously.', N'Extracted', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_186');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'With strong disagreement', 0),
        (@MaCH, N'B', N'With total agreement and no votes against', 1),
        (@MaCH, N'C', N'After cancelling the company', 0),
        (@MaCH, N'D', N'By secret lottery', 0);
END
GO

-- Câu hỏi SEED_EAI_187 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_187')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct correlative conjunction: ''The new software is _______ user-friendly but also highly secure.''',
            NULL, NULL, N'Cặp liên từ tương quan chuẩn: ''not only... but also...'' (không những... mà còn...).', N'Cặp liên từ tương quan chuẩn: ''not only... but also...'' (không những... mà còn...).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Liên từ tương quan: Either... or, Neither... nor, Not only... but also.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_187');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'neither', 0),
        (@MaCH, N'B', N'not only', 1),
        (@MaCH, N'C', N'either', 0),
        (@MaCH, N'D', N'both', 0);
END
GO

-- Câu hỏi SEED_EAI_188 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_188')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'In a business schedule, what does ''TBD'' stand for?',
            NULL, NULL, N'''TBD'' là viết tắt của ''To Be Determined'' (Sẽ được quyết định / thông báo sau).', N'''TBD'' là viết tắt của ''To Be Determined'' (Sẽ được quyết định / thông báo sau).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Viết tắt trong biểu mẫu: TBD (To Be Determined), TBA (To Be Announced).', N'Extracted', N'04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx', NULL, 'SEED_EAI_188');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'To Be Deleted', 0),
        (@MaCH, N'B', N'To Be Determined', 1),
        (@MaCH, N'C', N'Total Bank Deposit', 0),
        (@MaCH, N'D', N'Time Before Departure', 0);
END
GO

-- Câu hỏi SEED_EAI_189 (NguPhap - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_189')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'Complete the sentence: ''Mr. Tanaka has worked here _______ more than fifteen years.''',
            NULL, NULL, N'''For'' đi với khoảng thời gian kéo dài (for more than fifteen years). ''Since'' chỉ đi với mốc thời gian bắt đầu.', N'''For'' đi với khoảng thời gian kéo dài (for more than fifteen years). ''Since'' chỉ đi với mốc thời gian bắt đầu.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Phân biệt For (khoảng thời gian) và Since (mốc thời gian).', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_189');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'for', 1),
        (@MaCH, N'B', N'since', 0),
        (@MaCH, N'C', N'during', 0),
        (@MaCH, N'D', N'in', 0);
END
GO

-- Câu hỏi SEED_EAI_190 (TuVung - Trình độ 3 - Dạng NoiCap - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_190')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'NoiCap', N'TuVung', N'TrungBinh', N'Match TOEIC business terms with their equivalents:
1. ''Quarterly''
2. ''Annual''
3. ''Biannual''',
            NULL, NULL, N'1 là 3 tháng 1 lần (hàng quý); 2 là 1 năm 1 lần (hàng năm); 3 là 2 lần 1 năm (nửa năm 1 lần).', N'1 là 3 tháng 1 lần (hàng quý); 2 là 1 năm 1 lần (hàng năm); 3 là 2 lần 1 năm (nửa năm 1 lần).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Thuật ngữ chu kỳ tài chính và báo cáo doanh nghiệp.', N'Applied', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_190');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'1-Every three months, 2-Once a year, 3-Twice a year', 1),
        (@MaCH, N'B', N'1-Every week, 2-Every month, 3-Every day', 0),
        (@MaCH, N'C', N'1-Once in ten years, 2-Twice a month, 3-Every hour', 0),
        (@MaCH, N'D', N'1-Daily, 2-Weekly, 3-Hourly', 0);
END
GO

-- Câu hỏi SEED_EAI_191 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_191')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What is a ''Press Release'' issued by a corporation?',
            NULL, NULL, N'''Thông cáo báo chí'' (Press Release) là bản tuyên bố chính thức gửi cho giới truyền thông để công bố tin tức mới về sản phẩm hoặc sự kiện công ty.', N'''Thông cáo báo chí'' (Press Release) là bản tuyên bố chính thức gửi cho giới truyền thông để công bố tin tức mới về sản phẩm hoặc sự kiện công ty.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Thể loại văn bản Press Release trong Part 7.', N'Extracted', N'04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx', NULL, 'SEED_EAI_191');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'An internal memo reprimanding a worker', 0),
        (@MaCH, N'B', N'An official public statement sent to media outlets providing information or making an announcement', 1),
        (@MaCH, N'C', N'A personal handwritten diary', 0),
        (@MaCH, N'D', N'A factory operating handbook', 0);
END
GO

-- Câu hỏi SEED_EAI_192 (NguPhap - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_192')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct preposition: ''The workshop will be held _______ Tuesday morning, October 24.''',
            NULL, NULL, N'Đi với các buổi trong một ngày cụ thể (Tuesday morning) phải dùng giới từ ''on''.', N'Đi với các buổi trong một ngày cụ thể (Tuesday morning) phải dùng giới từ ''on''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Giới từ chỉ thời gian: In / On / At.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_192');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'in', 0),
        (@MaCH, N'B', N'on', 1),
        (@MaCH, N'C', N'at', 0),
        (@MaCH, N'D', N'by', 0);
END
GO

-- Câu hỏi SEED_EAI_193 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_193')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''The new smartphone model features a battery life that is _______ to its predecessor.''',
            NULL, NULL, N'Cấu trúc so sánh: ''superior to'' (vượt trội hơn hẳn so với cái gì).', N'Cấu trúc so sánh: ''superior to'' (vượt trội hơn hẳn so với cái gì).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Tính từ so sánh đi với giới từ ''to'': Superior to, Inferior to, Prior to.', N'Extracted', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_193');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'superior', 1),
        (@MaCH, N'B', N'greater', 0),
        (@MaCH, N'C', N'higher', 0),
        (@MaCH, N'D', N'richer', 0);
END
GO

-- Câu hỏi SEED_EAI_194 (NguPhap - Trình độ 3 - Dạng SuaLoiSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_194')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the mistake: ''Neither the CEO nor the board members was satisfied with the outcome.''',
            NULL, NULL, N'Quy tắc hòa hợp chủ vị với ''Neither... nor'': Động từ chia theo chủ ngữ gần nhất. ''board members'' là số nhiều, do đó phải chia ''were satisfied'', không dùng ''was''.', N'Quy tắc hòa hợp chủ vị với ''Neither... nor'': Động từ chia theo chủ ngữ gần nhất. ''board members'' là số nhiều, do đó phải chia ''were satisfied'', không dùng ''was''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Quy tắc hòa hợp chủ ngữ số ít/số nhiều với Neither... nor.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_194');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''Neither'' should be ''Either''', 0),
        (@MaCH, N'B', N'''was'' should be ''were''', 1),
        (@MaCH, N'C', N'''with'' should be ''about''', 0),
        (@MaCH, N'D', N'''outcome'' should be ''outcomes''', 0);
END
GO

-- Câu hỏi SEED_EAI_195 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_195')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'TracNghiem', N'Doc', N'De', N'What is the purpose of an ''Itinerary'' sent to a corporate traveler?',
            NULL, NULL, N'''Itinerary'' là lịch trình chi tiết chuyến đi bao gồm số hiệu chuyến bay, giờ hạ cánh, khách sạn và các cuộc hẹn công tác.', N'''Itinerary'' là lịch trình chi tiết chuyến đi bao gồm số hiệu chuyến bay, giờ hạ cánh, khách sạn và các cuộc hẹn công tác.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Lịch trình công tác (Travel Itinerary).', N'Extracted', N'04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx', NULL, 'SEED_EAI_195');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A medical prescription', 0),
        (@MaCH, N'B', N'A detailed travel plan outlining flight times, hotels, and scheduled activities', 1),
        (@MaCH, N'C', N'A bank loan approval letter', 0),
        (@MaCH, N'D', N'A tax return form', 0);
END
GO

-- Câu hỏi SEED_EAI_196 (Viet - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_196')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 4, N'VietLaiCau', N'Viet', N'TrungBinh', N'Rewrite using ''adhere to'': ''All factory staff must follow safety protocols strictly.''',
            NULL, NULL, N'''All factory staff must adhere strictly to safety protocols.'' là dạng câu tương đương chuyên nghiệp trong văn bản quy định.', N'''All factory staff must adhere strictly to safety protocols.'' là dạng câu tương đương chuyên nghiệp trong văn bản quy định.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Cụm từ đồng nghĩa: Follow strictly = Adhere strictly to.', N'Applied', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_196');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'All factory staff must adhere strictly to safety protocols.', 1),
        (@MaCH, N'B', N'All staff adhere of safety strictly.', 0),
        (@MaCH, N'C', N'Staff must adhere safety with factories.', 0),
        (@MaCH, N'D', N'Safety must adhere the staff.', 0);
END
GO

-- Câu hỏi SEED_EAI_197 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_197')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'Which noun denotes a person who uses goods or services produced in an economy?',
            NULL, NULL, N'''Consumer'' (Người tiêu dùng).', N'''Consumer'' (Người tiêu dùng).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Từ vựng thị trường: Consumer, Producer, Wholesaler, Retailer.', N'Extracted', N'03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf', NULL, 'SEED_EAI_197');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Manufacturer', 0),
        (@MaCH, N'B', N'Consumer', 1),
        (@MaCH, N'C', N'Distributor', 0),
        (@MaCH, N'D', N'Competitor', 0);
END
GO

-- Câu hỏi SEED_EAI_198 (NguPhap - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_198')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 3, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'Complete the sentence: ''The seminar was so informative _______ every attendee praised the speaker.''',
            NULL, NULL, N'Cấu trúc kết quả ''so + adj + that + clause'' (quá... đến nỗi mà...).', N'Cấu trúc kết quả ''so + adj + that + clause'' (quá... đến nỗi mà...).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Mệnh đề chỉ kết quả với So... that và Such... that.', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_198');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'that', 1),
        (@MaCH, N'B', N'as', 0),
        (@MaCH, N'C', N'than', 0),
        (@MaCH, N'D', N'since', 0);
END
GO

-- Câu hỏi SEED_EAI_199 (Doc - Trình độ 3 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_199')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 3, N'DungSai', N'Doc', N'De', N'True or False: In a TOEIC business letter, the abbreviation ''Encl.'' at the very bottom indicates that additional documents are enclosed in the envelope.',
            NULL, NULL, N'Đúng. ''Encl.'' là viết tắt của ''Enclosure'' (Tài liệu đính kèm bên trong phong bì thư).', N'Đúng. ''Encl.'' là viết tắt của ''Enclosure'' (Tài liệu đính kèm bên trong phong bì thư).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Ký hiệu thư tín: Encl. (Enclosure).', N'Extracted', N'04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx', NULL, 'SEED_EAI_199');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_200 (NguPhap - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_200')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 4, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Select the sentence with correct participle clause reduction:',
            NULL, NULL, N'''Having reviewed the quarterly data, the CFO drafted the budget recommendations.'' rút gọn mệnh đề cùng chủ ngữ ở dạng phân từ hoàn thành (Having + V3) diễn tả hành động xảy ra trước.', N'''Having reviewed the quarterly data, the CFO drafted the budget recommendations.'' rút gọn mệnh đề cùng chủ ngữ ở dạng phân từ hoàn thành (Having + V3) diễn tả hành động xảy ra trước.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Rút gọn mệnh đề trạng ngữ bằng phân từ (Participle Clauses).', N'Extracted', N'02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf', NULL, 'SEED_EAI_200');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Reviewing the data, the budget was drafted by the computer.', 0),
        (@MaCH, N'B', N'Having reviewed the quarterly data, the CFO drafted the budget recommendations.', 1),
        (@MaCH, N'C', N'Reviewed the data, drafting the budget happened.', 0),
        (@MaCH, N'D', N'Having been reviewing data, CFO drafting.', 0);
END
GO

PRINT N'Đã hoàn thành nạp Batch 04 (SEED_EAI_151 -> SEED_EAI_200) thành công!';
GO
