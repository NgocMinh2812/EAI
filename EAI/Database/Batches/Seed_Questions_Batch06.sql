-- ====================================================================================
-- SEED QUESTIONS BATCH: Batch 06 (SEED_EAI_251 -> SEED_EAI_300)
-- Số lượng câu hỏi: 50
-- Đảm bảo: Idempotent (Chạy lại nhiều lần không trùng lặp)
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Đang nạp Batch 06 (SEED_EAI_251 -> SEED_EAI_300) (50 câu hỏi)...';

-- Câu hỏi SEED_EAI_251 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_251')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What is the primary operational distinction between ''Skimming'' and ''Scanning'' in IELTS Academic Reading?',
            NULL, NULL, N'''Skimming'' là kỹ thuật đọc lướt để nắm ý chính (gist) và bố cục toàn bài, còn ''Scanning'' là đọc quét mắt tìm từ khóa cụ thể (con số, tên riêng, thuật ngữ).', N'''Skimming'' là kỹ thuật đọc lướt để nắm ý chính (gist) và bố cục toàn bài, còn ''Scanning'' là đọc quét mắt tìm từ khóa cụ thể (con số, tên riêng, thuật ngữ).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 8 - Mục 1: Phân biệt Skimming (đọc lấy ý chính) vs. Scanning (đọc tìm dữ liệu cụ thể).', N'Extracted', N'BaiHoc_08_IeltsSkimmingScanning.txt', 8, 'SEED_EAI_251');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Skimming is for spelling checks, while scanning is for translation.', 0),
        (@MaCH, N'B', N'Skimming quickly captures the central gist and paragraph structure, while scanning targets specific keywords, dates, and names.', 1),
        (@MaCH, N'C', N'Scanning reads the entire passage slowly word-by-word.', 0),
        (@MaCH, N'D', N'Both techniques require vocal reading out loud.', 0);
END
GO

-- Câu hỏi SEED_EAI_252 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_252')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'Where in an academic paragraph can readers typically locate the ''Topic Sentence''?',
            NULL, NULL, N'Trong văn bản học thuật IELTS, câu chủ đề (Topic Sentence) thường nằm ở câu đầu tiên hoặc câu thứ hai của đoạn văn.', N'Trong văn bản học thuật IELTS, câu chủ đề (Topic Sentence) thường nằm ở câu đầu tiên hoặc câu thứ hai của đoạn văn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 8 - Mục 2: Nhận diện Topic Sentence: Nằm ở vị trí câu đầu hoặc câu thứ hai của đoạn.', N'Extracted', N'BaiHoc_08_IeltsSkimmingScanning.txt', 8, 'SEED_EAI_252');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Exclusively in the footnotes', 0),
        (@MaCH, N'B', N'Usually in the first or second sentence of the paragraph', 1),
        (@MaCH, N'C', N'Only in the title of the book', 0),
        (@MaCH, N'D', N'In the appendix at the back', 0);
END
GO

-- Câu hỏi SEED_EAI_253 (Doc - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_253')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'DienKhuyet', N'Doc', N'TrungBinh', N'Complete the reading technique: ''When scanning, candidates should search for _______ rather than looking for identical words, because IELTS tests paraphrase skills.''',
            NULL, NULL, N'Đề thi IELTS hầu như luôn ''paraphrase'' (diễn đạt lại bằng từ đồng nghĩa). Do đó khi scan phải tìm từ đồng nghĩa (synonyms).', N'Đề thi IELTS hầu như luôn ''paraphrase'' (diễn đạt lại bằng từ đồng nghĩa). Do đó khi scan phải tìm từ đồng nghĩa (synonyms).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 8 - Mục 3: Cảnh báo bẫy Paraphrase trong IELTS Reading: Tìm kiếm Synonyms.', N'Extracted', N'BaiHoc_08_IeltsSkimmingScanning.txt', 8, 'SEED_EAI_253');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'synonyms', 1),
        (@MaCH, N'B', N'rhymes', 0),
        (@MaCH, N'C', N'emojis', 0),
        (@MaCH, N'D', N'spaces', 0);
END
GO

-- Câu hỏi SEED_EAI_254 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_254')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'DungSai', N'Doc', N'De', N'True or False: In IELTS Reading, candidates should spend approximately 20 minutes on each of the three passages to manage the 60-minute time limit effectively.',
            NULL, NULL, N'Đúng. Bài thi IELTS Reading gồm 3 bài đọc với 40 câu trong 60 phút, trung bình mỗi bài phân bổ khoảng 20 phút.', N'Đúng. Bài thi IELTS Reading gồm 3 bài đọc với 40 câu trong 60 phút, trung bình mỗi bài phân bổ khoảng 20 phút.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 8 - Chiến lược quản lý thời gian: 20 phút / Passage.', N'Extracted', N'BaiHoc_08_IeltsSkimmingScanning.txt', 8, 'SEED_EAI_254');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_255 (Doc - Trình độ 4 - Dạng NoiCap - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_255')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'NoiCap', N'Doc', N'TrungBinh', N'Match each IELTS Reading question type with its primary strategy:
1. ''Matching Headings''
2. ''Sentence Completion''
3. ''True / False / Not Given''',
            NULL, NULL, N'1-Đọc câu đầu/cuối tìm ý chính đoạn; 2-Xác định loại từ cần điền và scan từ khóa; 3-Đối chiếu tính chính xác của dữ kiện văn bản.', N'1-Đọc câu đầu/cuối tìm ý chính đoạn; 2-Xác định loại từ cần điền và scan từ khóa; 3-Đối chiếu tính chính xác của dữ kiện văn bản.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 8 - Bảng chiến thuật tương ứng từng dạng bài IELTS Reading.', N'Extracted', N'BaiHoc_08_IeltsSkimmingScanning.txt', 8, 'SEED_EAI_255');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'1-Skim topic sentences for paragraph gist, 2-Identify grammatical word form and scan keyword, 3-Verify factual claims against text', 1),
        (@MaCH, N'B', N'1-Count words, 2-Translate into Vietnamese, 3-Guess randomly', 0),
        (@MaCH, N'C', N'1-Look at images, 2-Memorize spelling, 3-Read backwards', 0),
        (@MaCH, N'D', N'1-Underline nouns, 2-Skip text, 3-Check punctuation', 0);
END
GO

-- Câu hỏi SEED_EAI_256 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_256')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'TracNghiem', N'Doc', N'Kho', N'In IELTS Reading, what is the crucial definition of ''NOT GIVEN''?',
            NULL, NULL, N'''NOT GIVEN'' có nghĩa là thông tin đó không thể khẳng định cũng không thể bác bỏ dựa trên văn bản đọc (văn bản không đề cập đủ để kết luận).', N'''NOT GIVEN'' có nghĩa là thông tin đó không thể khẳng định cũng không thể bác bỏ dựa trên văn bản đọc (văn bản không đề cập đủ để kết luận).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 9 - Định nghĩa chuẩn mực: TRUE = matches information; FALSE = contradicts information; NOT GIVEN = impossible to verify from text.', N'Extracted', N'BaiHoc_09_TrueFalseNotGiven.txt', 9, 'SEED_EAI_256');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The statement is 100% false in real life.', 0),
        (@MaCH, N'B', N'There is not enough information in the passage to confirm or contradict the statement.', 1),
        (@MaCH, N'C', N'The author personally dislikes the statement.', 0),
        (@MaCH, N'D', N'The words in the statement are completely misspelled.', 0);
END
GO

-- Câu hỏi SEED_EAI_257 (Doc - Trình độ 4 - Dạng TrueFalseNotGiven - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_257')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'TrueFalseNotGiven', N'Doc', N'Kho', N'Read the excerpt: ''While electric cars produce zero tailpipe emissions, the carbon footprint of battery manufacturing remains a challenge.'' Determine: ''Electric vehicles have no negative environmental impacts whatsoever.''',
            N'While electric cars produce zero tailpipe emissions, the carbon footprint of battery manufacturing remains a challenge.', NULL, N'Văn bản nêu rõ việc chế tạo pin vẫn tạo ra ''carbon footprint'' (vấn đề môi trường), trái ngược hoàn toàn với mệnh đề ''không có bất kỳ tác động tiêu cực nào''. Vì vậy đáp án là FALSE.', N'Văn bản nêu rõ việc chế tạo pin vẫn tạo ra ''carbon footprint'' (vấn đề môi trường), trái ngược hoàn toàn với mệnh đề ''không có bất kỳ tác động tiêu cực nào''. Vì vậy đáp án là FALSE.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 9 - Phân tích câu mâu thuẫn trực tiếp (Contradiction -> FALSE).', N'Extracted', N'BaiHoc_09_TrueFalseNotGiven.txt', 9, 'SEED_EAI_257');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1),
        (@MaCH, N'C', N'Not Given', 0);
END
GO

-- Câu hỏi SEED_EAI_258 (Doc - Trình độ 4 - Dạng TrueFalseNotGiven - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_258')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'TrueFalseNotGiven', N'Doc', N'Kho', N'Read the excerpt: ''Dr. Carter developed the synthetic vaccine in his laboratory in Zurich.'' Determine: ''Dr. Carter spent ten years researching the synthetic vaccine.''',
            N'Dr. Carter developed the synthetic vaccine in his laboratory in Zurich.', NULL, N'Văn bản chỉ nói Dr. Carter phát triển vắc-xin tại Zurich, hoàn toàn không nói ông ấy mất 10 năm hay bao lâu. Do đó đáp án là NOT GIVEN.', N'Văn bản chỉ nói Dr. Carter phát triển vắc-xin tại Zurich, hoàn toàn không nói ông ấy mất 10 năm hay bao lâu. Do đó đáp án là NOT GIVEN.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 9 - Bẫy thông tin bổ sung không được đề cập -> NOT GIVEN.', N'Extracted', N'BaiHoc_09_TrueFalseNotGiven.txt', 9, 'SEED_EAI_258');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 0),
        (@MaCH, N'C', N'Not Given', 1);
END
GO

-- Câu hỏi SEED_EAI_259 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_259')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'TracNghiem', N'Doc', N'Kho', N'What danger do ''Qualifying Words'' (such as all, always, solely vs. some, sometimes, often) present in True/False/Not Given questions?',
            NULL, NULL, N'Các từ hạn định tuyệt đối (all, solely, always) thường bóp méo ý của bài đọc (bài đọc chỉ nói ''some'' hoặc ''tend to''). Nếu câu hỏi dùng từ tuyệt đối hóa thì câu đó thường là FALSE.', N'Các từ hạn định tuyệt đối (all, solely, always) thường bóp méo ý của bài đọc (bài đọc chỉ nói ''some'' hoặc ''tend to''). Nếu câu hỏi dùng từ tuyệt đối hóa thì câu đó thường là FALSE.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 9 - Bẫy từ hạn định (Qualifying words): all/only/always vs some/often/might.', N'Extracted', N'BaiHoc_09_TrueFalseNotGiven.txt', 9, 'SEED_EAI_259');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'They make sentences sound poetic.', 0),
        (@MaCH, N'B', N'They alter the scope of claims; absolute words (all, always) often contradict moderate statements (some, likely) in the text.', 1),
        (@MaCH, N'C', N'They make text impossible to translate.', 0),
        (@MaCH, N'D', N'They are always typos by the examiner.', 0);
END
GO

-- Câu hỏi SEED_EAI_260 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_260')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'DungSai', N'Doc', N'TrungBinh', N'True or False: In IELTS Reading, questions in True/False/Not Given tasks always follow the chronological order of the reading passage.',
            NULL, NULL, N'Đúng. Dạng bài TFNG/YNNG tuân theo trình tự tuần tự của văn bản đọc (thông tin câu 2 sẽ xuất hiện sau câu 1).', N'Đúng. Dạng bài TFNG/YNNG tuân theo trình tự tuần tự của văn bản đọc (thông tin câu 2 sẽ xuất hiện sau câu 1).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 9 - Quy tắc tuần tự của dạng bài TFNG (Questions follow chronological text order).', N'Extracted', N'BaiHoc_09_TrueFalseNotGiven.txt', 9, 'SEED_EAI_260');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_261 (Viet - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_261')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 4, N'TracNghiem', N'Viet', N'Kho', N'In an IELTS Writing Task 2 Opinion Essay, what two elements MUST be included in the Introduction paragraph?',
            NULL, NULL, N'Mở bài Task 2 chuẩn gồm 2 phần: Paraphrase lại đề bài (Background statement) và Nêu rõ lập trường cá nhân (Thesis statement).', N'Mở bài Task 2 chuẩn gồm 2 phần: Paraphrase lại đề bài (Background statement) và Nêu rõ lập trường cá nhân (Thesis statement).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 10 - Cấu trúc mở bài Task 2: Paraphrase topic + Clear Thesis Statement.', N'Extracted', N'BaiHoc_10_OpinionEssayBand7.txt', 10, 'SEED_EAI_261');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A funny joke and personal phone number', 0),
        (@MaCH, N'B', N'A paraphrase of the prompt prompt and a clear thesis statement stating your position', 1),
        (@MaCH, N'C', N'A list of bullet points and dictionary definitions', 0),
        (@MaCH, N'D', N'A summary of counter-arguments with no personal stance', 0);
END
GO

-- Câu hỏi SEED_EAI_262 (Viet - Trình độ 5 - Dạng TuLuan - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_262')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 5, N'TuLuan', N'Viet', N'Kho', N'IELTS Writing Task 2 Prompt:
''Some people believe that artificial intelligence will replace human teachers in the future. To what extent do you agree or disagree?''
Draft a 250-word discursive essay with a clear thesis, two body paragraphs, and a conclusion.',
            NULL, NULL, N'Tiêu chí chấm điểm Band 7.5+: Task Response (trả lời đầy đủ, lập trường rõ ràng), Coherence & Cohesion (liên kết đoạn mạch lạc), Lexical Resource (từ vựng học thuật C1), Grammatical Range & Accuracy (câu phức, đảo ngữ, bị động).', N'Tiêu chí chấm điểm Band 7.5+: Task Response (trả lời đầy đủ, lập trường rõ ràng), Coherence & Cohesion (liên kết đoạn mạch lạc), Lexical Resource (từ vựng học thuật C1), Grammatical Range & Accuracy (câu phức, đảo ngữ, bị động).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 10 - Tiêu chuẩn chấm bài IELTS Writing Task 2 theo 4 tiêu chí của British Council.', N'Extracted', N'BaiHoc_10_OpinionEssayBand7.txt', 10, 'SEED_EAI_262');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'RUBRIC', N'Task Achievement (25%), Coherence & Cohesion (25%), Lexical Resource (25%), Grammatical Range (25%)', 1);
END
GO

-- Câu hỏi SEED_EAI_263 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_263')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'Which academic linking phrase expresses a strong counter-argument?',
            NULL, NULL, N'''On the other hand'' hoặc ''Conversely'' biểu thị quan điểm tương phản học thuật.', N'''On the other hand'' hoặc ''Conversely'' biểu thị quan điểm tương phản học thuật.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 10 - Từ nối học thuật Task 2: Furthermore, In contrast, Conversely.', N'Extracted', N'BaiHoc_10_OpinionEssayBand7.txt', 10, 'SEED_EAI_263');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'By the way', 0),
        (@MaCH, N'B', N'On the other hand / Conversely', 1),
        (@MaCH, N'C', N'Also guess what', 0),
        (@MaCH, N'D', N'Anyway', 0);
END
GO

-- Câu hỏi SEED_EAI_264 (Viet - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_264')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 4, N'DienKhuyet', N'Viet', N'TrungBinh', N'Complete the concluding phrase: ''In _______, while technology offers benefits, human educators remain indispensable.''',
            NULL, NULL, N'''In conclusion'' (Tóm lại) là cụm từ kết luận chuẩn mực trong văn bản nghị luận Task 2.', N'''In conclusion'' (Tóm lại) là cụm từ kết luận chuẩn mực trong văn bản nghị luận Task 2.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 10 - Phần Kết bài: ''In conclusion / To conclude...''', N'Extracted', N'BaiHoc_10_OpinionEssayBand7.txt', 10, 'SEED_EAI_264');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'conclusion', 1),
        (@MaCH, N'B', N'end', 0),
        (@MaCH, N'C', N'final', 0),
        (@MaCH, N'D', N'finish', 0);
END
GO

-- Câu hỏi SEED_EAI_265 (Viet - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_265')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 4, N'DungSai', N'Viet', N'De', N'True or False: Writing fewer than 250 words in IELTS Writing Task 2 incurs an automatic penalty on the Task Response score.',
            NULL, NULL, N'Đúng. Viết dưới 250 từ ở Task 2 bị trừ điểm tiêu chí Task Response do bài chưa phát triển đầy đủ ý.', N'Đúng. Viết dưới 250 từ ở Task 2 bị trừ điểm tiêu chí Task Response do bài chưa phát triển đầy đủ ý.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 10 - Quy định độ dài Task 2: Tối thiểu 250 từ.', N'Extracted', N'BaiHoc_10_OpinionEssayBand7.txt', 10, 'SEED_EAI_265');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_266 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_266')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 5, N'TracNghiem', N'TuVung', N'Kho', N'Which advanced collocations best describe severe urban environmental challenges?',
            NULL, NULL, N'''Unprecedented smog levels'' (mức độ khói bụi chưa từng có) và ''unchecked carbon emissions'' (khí thải carbon không kiểm soát) là từ vựng C1 chuẩn xác.', N'''Unprecedented smog levels'' (mức độ khói bụi chưa từng có) và ''unchecked carbon emissions'' (khí thải carbon không kiểm soát) là từ vựng C1 chuẩn xác.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 11 - Bộ từ vựng chủ đề Đô thị hóa & Môi trường Band C1: Unprecedented smog, unchecked emissions.', N'Extracted', N'BaiHoc_11_ProblemSolutionEssay.txt', 11, 'SEED_EAI_266');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Bad dirty air and dirty rivers', 0),
        (@MaCH, N'B', N'Unprecedented smog levels and unchecked carbon emissions', 1),
        (@MaCH, N'C', N'Smoke from cooking food', 0),
        (@MaCH, N'D', N'Loud street noise from bikes', 0);
END
GO

-- Câu hỏi SEED_EAI_267 (Viet - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_267')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 5, N'TracNghiem', N'Viet', N'Kho', N'In a Problem-Solution essay, what must each proposed solution directly address?',
            NULL, NULL, N'Mỗi giải pháp đề xuất trong Body 2 bắt buộc phải tương ứng và giải quyết trực tiếp một nguyên nhân hoặc vấn đề đã nêu ở Body 1.', N'Mỗi giải pháp đề xuất trong Body 2 bắt buộc phải tương ứng và giải quyết trực tiếp một nguyên nhân hoặc vấn đề đã nêu ở Body 1.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 11 - Nguyên tắc gắn kết Cause - Effect - Solution: Giải pháp phải ánh xạ tương ứng vấn đề.', N'Extracted', N'BaiHoc_11_ProblemSolutionEssay.txt', 11, 'SEED_EAI_267');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Random worldwide dilemmas', 0),
        (@MaCH, N'B', N'The specific causes and problems identified in the preceding paragraph', 1),
        (@MaCH, N'C', N'The author''s childhood memories', 0),
        (@MaCH, N'D', N'Historical battles from centuries ago', 0);
END
GO

-- Câu hỏi SEED_EAI_268 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_268')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 5, N'DienKhuyet', N'TuVung', N'Kho', N'Fill in the academic term: ''Governments should enact stricter legislation to _______ corporate polluters.''',
            NULL, NULL, N'''penalize'' (xử phạt) các doanh nghiệp gây ô nhiễm môi trường.', N'''penalize'' (xử phạt) các doanh nghiệp gây ô nhiễm môi trường.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 11 - Từ vựng giải pháp chính sách: Enact legislation, Penalize corporate polluters.', N'Extracted', N'BaiHoc_11_ProblemSolutionEssay.txt', 11, 'SEED_EAI_268');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'penalize', 1),
        (@MaCH, N'B', N'reward', 0),
        (@MaCH, N'C', N'praise', 0),
        (@MaCH, N'D', N'ignore', 0);
END
GO

-- Câu hỏi SEED_EAI_269 (Viet - Trình độ 5 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_269')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 5, N'VietLaiCau', N'Viet', N'Kho', N'Rewrite using ''result in'': ''Rapid urbanization causes traffic congestion and habitat loss.''',
            NULL, NULL, N'''Rapid urbanization results in severe traffic congestion and habitat loss.''', N'''Rapid urbanization results in severe traffic congestion and habitat loss.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 11 - Cấu trúc diễn đạt quan hệ nhân quả: lead to / result in / give rise to.', N'Applied', N'BaiHoc_11_ProblemSolutionEssay.txt', 11, 'SEED_EAI_269');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Rapid urbanization results in severe traffic congestion and habitat loss.', 1),
        (@MaCH, N'B', N'Traffic congestion results in urbanization rapidly.', 0),
        (@MaCH, N'C', N'Rapid urbanization is resulted of traffic.', 0),
        (@MaCH, N'D', N'Habitat loss resulting rapid urbanization.', 0);
END
GO

-- Câu hỏi SEED_EAI_270 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_270')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 4, N'DungSai', N'Doc', N'De', N'True or False: The term ''renewable energy'' refers to natural energy resources that do not deplete, such as solar, wind, and hydroelectric power.',
            NULL, NULL, N'Đúng. Năng lượng tái tạo (renewable energy) bao gồm năng lượng mặt trời, gió và thủy điện.', N'Đúng. Năng lượng tái tạo (renewable energy) bao gồm năng lượng mặt trời, gió và thủy điện.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 11 - Thuật ngữ môi trường cốt lõi: Renewable energy resources.', N'Extracted', N'BaiHoc_11_ProblemSolutionEssay.txt', 11, 'SEED_EAI_270');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_271 (Nghe - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_271')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'TracNghiem', N'Nghe', N'TrungBinh', N'In IELTS Listening Section 3, what context is always depicted?',
            NULL, NULL, N'IELTS Listening Section 3 luôn là cuộc thảo luận học thuật từ 2 đến 4 người (thường là giữa sinh viên với nhau hoặc sinh viên với giáo sư hướng dẫn).', N'IELTS Listening Section 3 luôn là cuộc thảo luận học thuật từ 2 đến 4 người (thường là giữa sinh viên với nhau hoặc sinh viên với giáo sư hướng dẫn).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 12 - Đặc điểm Section 3: Cuộc thảo luận học thuật nhóm (2-4 người).', N'Extracted', N'BaiHoc_12_IeltsListeningSec3.txt', 12, 'SEED_EAI_271');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A monologue advertising grocery discounts', 0),
        (@MaCH, N'B', N'An academic discussion between 2-4 speakers (students and tutors) about an educational assignment or research project', 1),
        (@MaCH, N'C', N'A single person giving a bedtime story', 0),
        (@MaCH, N'D', N'A song recital', 0);
END
GO

-- Câu hỏi SEED_EAI_272 (Nghe - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_272')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'TracNghiem', N'Nghe', N'Kho', N'What listening trap is most prevalent in Section 3 when speakers change their minds?',
            NULL, NULL, N'Bẫy thay đổi ý định (Distractor / Self-correction): Người nói đưa ra một ý kiến nhưng sau đó nói ''Actually, on second thought...'' và chọn phương án khác.', N'Bẫy thay đổi ý định (Distractor / Self-correction): Người nói đưa ra một ý kiến nhưng sau đó nói ''Actually, on second thought...'' và chọn phương án khác.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 12 - Bẫy sửa đổi quyết định (Self-correction & Distractor trap).', N'Extracted', N'BaiHoc_12_IeltsListeningSec3.txt', 12, 'SEED_EAI_272');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Distractor trap where speakers initially agree on a plan but revise it with words like ''Actually'' or ''However''', 1),
        (@MaCH, N'B', N'The audio speed dropping to zero', 0),
        (@MaCH, N'C', N'Speakers speaking in foreign languages', 0),
        (@MaCH, N'D', N'Background music drowning out all speech', 0);
END
GO

-- Câu hỏi SEED_EAI_273 (Nghe - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_273')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'DienKhuyet', N'Nghe', N'TrungBinh', N'Fill in the blank: ''Before listening begins, candidates have 30 seconds to _______ the question stems and underline keywords.''',
            NULL, NULL, N'''analyze'' hoặc ''read'' các câu hỏi và gạch chân từ khóa cốt lõi.', N'''analyze'' hoặc ''read'' các câu hỏi và gạch chân từ khóa cốt lõi.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 12 - Tận dụng 30 giây chuẩn bị trước mỗi Section.', N'Extracted', N'BaiHoc_12_IeltsListeningSec3.txt', 12, 'SEED_EAI_273');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'analyze', 1),
        (@MaCH, N'B', N'ignore', 0),
        (@MaCH, N'C', N'erase', 0),
        (@MaCH, N'D', N'memorize', 0);
END
GO

-- Câu hỏi SEED_EAI_274 (Nghe - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_274')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'DungSai', N'Nghe', N'De', N'True or False: In Section 3 multiple matching questions, options are often paraphrased using synonyms rather than repeated word-for-word.',
            NULL, NULL, N'Đúng. Đề thi luôn diễn đạt lại ý bằng từ đồng nghĩa (paraphrasing).', N'Đúng. Đề thi luôn diễn đạt lại ý bằng từ đồng nghĩa (paraphrasing).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 12 - Kỹ thuật nhận diện Paraphrasing trong Section 3.', N'Extracted', N'BaiHoc_12_IeltsListeningSec3.txt', 12, 'SEED_EAI_274');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_275 (Nghe - Trình độ 4 - Dạng TraLoiNgan - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_275')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'TraLoiNgan', N'Nghe', N'TrungBinh', N'In IELTS Listening, what is the maximum word limit when instructions say ''NO MORE THAN TWO WORDS AND/OR A NUMBER''?',
            NULL, NULL, N'Thí sinh chỉ được viết tối đa 2 từ và/hoặc 1 con số. Viết từ thứ 3 sẽ bị 0 điểm.', N'Thí sinh chỉ được viết tối đa 2 từ và/hoặc 1 con số. Viết từ thứ 3 sẽ bị 0 điểm.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 12 - Quy tắc đếm từ trong bài thi Nghe IELTS.', N'Extracted', N'BaiHoc_12_IeltsListeningSec3.txt', 12, 'SEED_EAI_275');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Two words and/or a number', 1),
        (@MaCH, N'B', N'Three words', 0),
        (@MaCH, N'C', N'Four words', 0),
        (@MaCH, N'D', N'Unlimited', 0);
END
GO

-- Câu hỏi SEED_EAI_276 (Nghe - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_276')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 5, N'TracNghiem', N'Nghe', N'Kho', N'In IELTS Listening Section 4, what is unique about the audio delivery compared to earlier sections?',
            NULL, NULL, N'Section 4 là một bài giảng học thuật độc thoại (monologue lecture) kéo dài liên tục không có thời gian tạm nghỉ giữa chừng.', N'Section 4 là một bài giảng học thuật độc thoại (monologue lecture) kéo dài liên tục không có thời gian tạm nghỉ giữa chừng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 13 - Đặc thù Section 4: Độc thoại học thuật liên tục không ngắt quãng giữa chừng.', N'Extracted', N'BaiHoc_13_IeltsListeningSec4.txt', 13, 'SEED_EAI_276');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'There is a 2-minute music break in the middle.', 0),
        (@MaCH, N'B', N'It is an uninterrupted university monologue lecture without any mid-section pauses.', 1),
        (@MaCH, N'C', N'The speaker asks the test takers questions directly.', 0),
        (@MaCH, N'D', N'Audio is played three times.', 0);
END
GO

-- Câu hỏi SEED_EAI_277 (Nghe - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_277')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 5, N'TracNghiem', N'Nghe', N'Kho', N'How do lecture signpost words (e.g., ''Turning now to...'', ''In addition to this...'') assist listeners in Section 4?',
            NULL, NULL, N'Từ chỉ dẫn (signpost words) báo hiệu giáo sư chuyển sang mục tiếp theo trong dàn ý ghi chú, giúp thí sinh không bị lạc khỏi vị trí câu hỏi.', N'Từ chỉ dẫn (signpost words) báo hiệu giáo sư chuyển sang mục tiếp theo trong dàn ý ghi chú, giúp thí sinh không bị lạc khỏi vị trí câu hỏi.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 13 - Kỹ thuật bám sát cấu trúc bài giảng bằng Signpost words.', N'Extracted', N'BaiHoc_13_IeltsListeningSec4.txt', 13, 'SEED_EAI_277');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'They warn students that the exam has concluded.', 0),
        (@MaCH, N'B', N'They signal transitions to new bullet points and prevent test takers from losing their place in the notes.', 1),
        (@MaCH, N'C', N'They provide humorous distractions.', 0),
        (@MaCH, N'D', N'They define simple grammar rules.', 0);
END
GO

-- Câu hỏi SEED_EAI_278 (Nghe - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_278')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 5, N'DienKhuyet', N'Nghe', N'Kho', N'Fill in the blank: ''In Section 4 note completion, answers must be spelled _______; misspelled words are marked incorrect.''',
            NULL, NULL, N'Trong IELTS Listening, từ điền vào phải viết đúng chính tả (correctly/accurately); sai chính tả bị tính là sai.', N'Trong IELTS Listening, từ điền vào phải viết đúng chính tả (correctly/accurately); sai chính tả bị tính là sai.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 13 - Quy định tính điểm chính tả trong IELTS Listening.', N'Extracted', N'BaiHoc_13_IeltsListeningSec4.txt', 13, 'SEED_EAI_278');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'accurately', 1),
        (@MaCH, N'B', N'loosely', 0),
        (@MaCH, N'C', N'randomly', 0),
        (@MaCH, N'D', N'vaguely', 0);
END
GO

-- Câu hỏi SEED_EAI_279 (Nghe - Trình độ 5 - Dạng TraLoiNgan - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_279')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 5, N'TraLoiNgan', N'Nghe', N'Kho', N'If the listening audio says ''twenty percent of marine species'', and the note template says ''_______ of ocean organisms'', what goes in the blank?',
            NULL, NULL, N'Cụm cần điền là ''20%'' hoặc ''twenty percent'' (marine species đã được paraphrase thành ocean organisms).', N'Cụm cần điền là ''20%'' hoặc ''twenty percent'' (marine species đã được paraphrase thành ocean organisms).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 13 - Ví dụ mẫu Paraphrase trong bài giảng sinh học đại dương.', N'Extracted', N'BaiHoc_13_IeltsListeningSec4.txt', 13, 'SEED_EAI_279');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'20%', 1),
        (@MaCH, N'B', N'50%', 0),
        (@MaCH, N'C', N'Zero', 0),
        (@MaCH, N'D', N'All', 0);
END
GO

-- Câu hỏi SEED_EAI_280 (Nghe - Trình độ 5 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_280')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 5, N'DungSai', N'Nghe', N'De', N'True or False: In computer-delivered IELTS, candidates have 2 minutes at the end of the Listening test to review their answers, compared to 10 minutes in paper-based tests.',
            NULL, NULL, N'Đúng. Thi máy có 2 phút kiểm tra lại, thi giấy có 10 phút chép đáp án sang Answer Sheet.', N'Đúng. Thi máy có 2 phút kiểm tra lại, thi giấy có 10 phút chép đáp án sang Answer Sheet.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 13 - Khác biệt giữa IELTS trên máy tính và trên giấy.', N'Extracted', N'BaiHoc_13_IeltsListeningSec4.txt', 13, 'SEED_EAI_280');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_281 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_281')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What is the primary function of a literature review in an academic research paper?',
            NULL, NULL, N'Tổng quan y văn (Literature review) nhằm khảo sát, phân tích và tổng hợp các nghiên cứu đã công bố liên quan đến đề tài.', N'Tổng quan y văn (Literature review) nhằm khảo sát, phân tích và tổng hợp các nghiên cứu đã công bố liên quan đến đề tài.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 8 - Cấu trúc bài báo nghiên cứu khoa học: Literature review.', N'Extracted', N'BaiHoc_08_IeltsSkimmingScanning.txt', 8, 'SEED_EAI_281');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'To critique poetry and novels for entertainment', 0),
        (@MaCH, N'B', N'To survey, analyze, and synthesize previously published scholarly research on the subject', 1),
        (@MaCH, N'C', N'To advertise research equipment for sale', 0),
        (@MaCH, N'D', N'To thank personal family friends', 0);
END
GO

-- Câu hỏi SEED_EAI_282 (Doc - Trình độ 5 - Dạng TrueFalseNotGiven - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_282')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 5, N'TrueFalseNotGiven', N'Doc', N'Kho', N'Excerpt: ''Empirical data demonstrated a moderate correlation between sleep deprivation and cognitive decline, though causation was not conclusively proven.'' Determine: ''Researchers proved that lack of sleep directly causes cognitive decline.''',
            N'Empirical data demonstrated a moderate correlation between sleep deprivation and cognitive decline, though causation was not conclusively proven.', NULL, N'Văn bản khẳng định: ''though causation was not conclusively proven'' (mối quan hệ nhân quả chưa được chứng minh dứt khoát). Câu hỏi bảo ''đã chứng minh gây ra'' là sai trái với bài -> FALSE.', N'Văn bản khẳng định: ''though causation was not conclusively proven'' (mối quan hệ nhân quả chưa được chứng minh dứt khoát). Câu hỏi bảo ''đã chứng minh gây ra'' là sai trái với bài -> FALSE.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 9 - Bẫy Correlation (tương quan) vs. Causation (nhân quả) trong IELTS Reading.', N'Applied', N'BaiHoc_09_TrueFalseNotGiven.txt', 9, 'SEED_EAI_282');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1),
        (@MaCH, N'C', N'Not Given', 0);
END
GO

-- Câu hỏi SEED_EAI_283 (Viet - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_283')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 4, N'TracNghiem', N'Viet', N'TrungBinh', N'Which academic introductory phrase introduces an opposing viewpoint politely?',
            NULL, NULL, N'''While it is often argued that... I would contend that...'' là cấu trúc nhượng bộ phản biện cao cấp.', N'''While it is often argued that... I would contend that...'' là cấu trúc nhượng bộ phản biện cao cấp.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 10 - Mẫu câu nhượng bộ nâng cao: While it is argued that X, I would contend that Y.', N'Extracted', N'BaiHoc_10_OpinionEssayBand7.txt', 10, 'SEED_EAI_283');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'People are silly to think that', 0),
        (@MaCH, N'B', N'While it is commonly argued that X, I would contend that Y', 1),
        (@MaCH, N'C', N'Nobody knows anything except me that', 0),
        (@MaCH, N'D', N'Whatever people say is invalid because', 0);
END
GO

-- Câu hỏi SEED_EAI_284 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_284')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 5, N'DienKhuyet', N'TuVung', N'Kho', N'Complete the academic collocations: ''Over-reliance on fossil fuels exacerbates climate change and leads to severe _______ degradation.''',
            NULL, NULL, N'''environmental degradation'' (suy thoái môi trường) là cụm từ học thuật C1 chuẩn xác.', N'''environmental degradation'' (suy thoái môi trường) là cụm từ học thuật C1 chuẩn xác.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 11 - Collocation môi trường: Environmental degradation, habitat fragmentation.', N'Extracted', N'BaiHoc_11_ProblemSolutionEssay.txt', 11, 'SEED_EAI_284');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'environmental', 1),
        (@MaCH, N'B', N'environmentally', 0),
        (@MaCH, N'C', N'environmentalist', 0),
        (@MaCH, N'D', N'environment', 0);
END
GO

-- Câu hỏi SEED_EAI_285 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_285')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'When skimming an academic text, which elements should the reader pay special attention to?',
            NULL, NULL, N'Khi skim bài đọc, cần chú ý tiêu đề chính (headings), tiêu đề phụ (sub-headings), câu đầu/cuối đoạn và các hình ảnh/biểu đồ.', N'Khi skim bài đọc, cần chú ý tiêu đề chính (headings), tiêu đề phụ (sub-headings), câu đầu/cuối đoạn và các hình ảnh/biểu đồ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 8 - Kỹ năng Skim bài đọc: Headings, Subheadings, Visuals and Paragraph Openers.', N'Extracted', N'BaiHoc_08_IeltsSkimmingScanning.txt', 8, 'SEED_EAI_285');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The page numbers and font size', 0),
        (@MaCH, N'B', N'Headings, subheadings, topic sentences, and visual data graphs', 1),
        (@MaCH, N'C', N'Every single preposition and comma', 0),
        (@MaCH, N'D', N'Only the bibliography at the very end', 0);
END
GO

-- Câu hỏi SEED_EAI_286 (Doc - Trình độ 5 - Dạng TrueFalseNotGiven - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_286')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 5, N'TrueFalseNotGiven', N'Doc', N'Kho', N'Excerpt: ''The archeological team uncovered fragments of pottery dating back to the Bronze Age in the northern sector of the excavation site.'' Determine: ''The archeologists found gold jewelry alongside the pottery.''',
            N'The archeological team uncovered fragments of pottery dating back to the Bronze Age in the northern sector of the excavation site.', NULL, N'Văn bản chỉ đề cập đến các mảnh gốm (pottery fragments), hoàn toàn không đề cập đến trang sức vàng. Đáp án là NOT GIVEN.', N'Văn bản chỉ đề cập đến các mảnh gốm (pottery fragments), hoàn toàn không đề cập đến trang sức vàng. Đáp án là NOT GIVEN.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 9 - Phân biệt thông tin chưa được kiểm chứng trong bài -> NOT GIVEN.', N'Extracted', N'BaiHoc_09_TrueFalseNotGiven.txt', 9, 'SEED_EAI_286');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 0),
        (@MaCH, N'C', N'Not Given', 1);
END
GO

-- Câu hỏi SEED_EAI_287 (Viet - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_287')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 4, N'TracNghiem', N'Viet', N'TrungBinh', N'What is the consequence of writing an essay with memorized template sentences that do not fit the specific topic?',
            NULL, NULL, N'Việc học vẹt câu mẫu không ăn khớp với đề bài sẽ bị giám khảo trừ điểm nặng ở tiêu chí Task Response và Coherence.', N'Việc học vẹt câu mẫu không ăn khớp với đề bài sẽ bị giám khảo trừ điểm nặng ở tiêu chí Task Response và Coherence.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 10 - Cảnh báo lỗi học vẹt Memorized templates trong IELTS Writing.', N'Extracted', N'BaiHoc_10_OpinionEssayBand7.txt', 10, 'SEED_EAI_287');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'It automatically awards a Band 9.0 score.', 0),
        (@MaCH, N'B', N'It results in penalties under Task Response and Coherence because arguments appear generic and disconnected from the prompt.', 1),
        (@MaCH, N'C', N'Examiners will invite the student for tea.', 0),
        (@MaCH, N'D', N'No effect on scores whatsoever.', 0);
END
GO

-- Câu hỏi SEED_EAI_288 (Nghe - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_288')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'TracNghiem', N'Nghe', N'TrungBinh', N'In IELTS Listening, if you miss an answer in Section 2 or 3, what should you do immediately?',
            NULL, NULL, N'Chiến lược tâm lý phòng thi: Bỏ qua ngay câu đã lỡ để tập trung nghe câu kế tiếp, tuyệt đối không dừng lại hoảng loạn làm mất chuỗi các câu sau.', N'Chiến lược tâm lý phòng thi: Bỏ qua ngay câu đã lỡ để tập trung nghe câu kế tiếp, tuyệt đối không dừng lại hoảng loạn làm mất chuỗi các câu sau.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 12 - Chiến lược xử lý sự cố phòng thi: Move on immediately if you miss a question.', N'Extracted', N'BaiHoc_12_IeltsListeningSec3.txt', 12, 'SEED_EAI_288');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Stop listening and cry loudly.', 0),
        (@MaCH, N'B', N'Let it go immediately and refocus on the next question to avoid losing the entire stream of answers.', 1),
        (@MaCH, N'C', N'Ask the exam invigilator to pause the audio recording.', 0),
        (@MaCH, N'D', N'Leave the examination hall right away.', 0);
END
GO

-- Câu hỏi SEED_EAI_289 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_289')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 5, N'DienKhuyet', N'TuVung', N'Kho', N'Complete the academic conclusion: ''In summary, tackling climate change requires concerted global efforts and innovative technological _______.''',
            NULL, NULL, N'''solutions'' (các giải pháp công nghệ đổi mới sáng tạo).', N'''solutions'' (các giải pháp công nghệ đổi mới sáng tạo).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 11 - Từ vựng kết luận bài nghị luận môi trường: Concerted global efforts.', N'Extracted', N'BaiHoc_11_ProblemSolutionEssay.txt', 11, 'SEED_EAI_289');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'solutions', 1),
        (@MaCH, N'B', N'problems', 0),
        (@MaCH, N'C', N'failures', 0),
        (@MaCH, N'D', N'debts', 0);
END
GO

-- Câu hỏi SEED_EAI_290 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_290')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What does the Latin academic abbreviation ''e.g.'' mean in scholarly texts?',
            NULL, NULL, N'''e.g.'' là viết tắt của ''exempli gratia'' (nghĩa là ''for example'' - ví dụ như). Khác với ''i.e.'' nghĩa là ''that is'' (tức là).', N'''e.g.'' là viết tắt của ''exempli gratia'' (nghĩa là ''for example'' - ví dụ như). Khác với ''i.e.'' nghĩa là ''that is'' (tức là).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 8 - Các từ viết tắt học thuật phổ biến: e.g. (for example), i.e. (that is), et al. (and others).', N'Extracted', N'BaiHoc_08_IeltsSkimmingScanning.txt', 8, 'SEED_EAI_290');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'For example (exempli gratia)', 1),
        (@MaCH, N'B', N'In conclusion', 0),
        (@MaCH, N'C', N'That is to say', 0),
        (@MaCH, N'D', N'Without question', 0);
END
GO

-- Câu hỏi SEED_EAI_291 (Doc - Trình độ 5 - Dạng TrueFalseNotGiven - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_291')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 5, N'TrueFalseNotGiven', N'Doc', N'Kho', N'Excerpt: ''The preliminary findings were restricted to laboratory mice; trials on human subjects have yet to be approved by the ethics committee.'' Determine: ''The pharmaceutical treatment has been extensively tested on human patients.''',
            N'The preliminary findings were restricted to laboratory mice; trials on human subjects have yet to be approved by the ethics committee.', NULL, N'Văn bản nêu rõ thử nghiệm trên người ''chưa được phê duyệt'' (have yet to be approved), mâu thuẫn hoàn toàn với mệnh đề ''đã được thử nghiệm rộng rãi trên người''. Đáp án là FALSE.', N'Văn bản nêu rõ thử nghiệm trên người ''chưa được phê duyệt'' (have yet to be approved), mâu thuẫn hoàn toàn với mệnh đề ''đã được thử nghiệm rộng rãi trên người''. Đáp án là FALSE.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 9 - Phân tích câu mâu thuẫn trực tiếp (Contradiction -> FALSE).', N'Applied', N'BaiHoc_09_TrueFalseNotGiven.txt', 9, 'SEED_EAI_291');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1),
        (@MaCH, N'C', N'Not Given', 0);
END
GO

-- Câu hỏi SEED_EAI_292 (Viet - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_292')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 4, N'TracNghiem', N'Viet', N'TrungBinh', N'Which sentence best avoids overgeneralization by using academic hedging?',
            NULL, NULL, N'''Evidence suggests that regular physical exercise tends to improve cognitive memory in older adults.'' sử dụng kỹ thuật ''hedging'' (suggests, tends to) để đưa ra khẳng định khoa học cẩn trọng, không tuyệt đối hóa.', N'''Evidence suggests that regular physical exercise tends to improve cognitive memory in older adults.'' sử dụng kỹ thuật ''hedging'' (suggests, tends to) để đưa ra khẳng định khoa học cẩn trọng, không tuyệt đối hóa.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 10 - Kỹ thuật Hedging trong văn viết học thuật Band 7.5+.', N'Extracted', N'BaiHoc_10_OpinionEssayBand7.txt', 10, 'SEED_EAI_292');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Exercise always cures all mental illnesses without exception.', 0),
        (@MaCH, N'B', N'Evidence suggests that regular physical exercise tends to improve cognitive memory in older adults.', 1),
        (@MaCH, N'C', N'Without sports everyone becomes completely foolish.', 0),
        (@MaCH, N'D', N'Exercise is 100% the only thing brains need.', 0);
END
GO

-- Câu hỏi SEED_EAI_293 (Nghe - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_293')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 5, N'TracNghiem', N'Nghe', N'Kho', N'In Section 4 lectures, which word indicates that the professor is presenting an exception to a general theory?',
            NULL, NULL, N'''However'', ''Nonetheless'', hoặc ''An exception to this rule is...'' báo hiệu ngoại lệ của lý thuyết.', N'''However'', ''Nonetheless'', hoặc ''An exception to this rule is...'' báo hiệu ngoại lệ của lý thuyết.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 13 - Nhận biết từ tín hiệu tương phản trong bài giảng học thuật: Exception, However, Nonetheless.', N'Extracted', N'BaiHoc_13_IeltsListeningSec4.txt', 13, 'SEED_EAI_293');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Furthermore', 0),
        (@MaCH, N'B', N'Nonetheless / However', 1),
        (@MaCH, N'C', N'Similarly', 0),
        (@MaCH, N'D', N'Consequently', 0);
END
GO

-- Câu hỏi SEED_EAI_294 (Doc - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_294')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'DienKhuyet', N'Doc', N'TrungBinh', N'Fill in the blank: ''IELTS Academic Reading texts are sourced from books, journals, magazines, and _______ written for a non-specialist audience.''',
            NULL, NULL, N'Văn bản đọc IELTS được trích xuất từ sách, tạp chí, tập san và báo chí (newspapers).', N'Văn bản đọc IELTS được trích xuất từ sách, tạp chí, tập san và báo chí (newspapers).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 8 - Nguồn ngữ liệu đề thi IELTS Reading: Books, journals, magazines and newspapers.', N'Extracted', N'BaiHoc_08_IeltsSkimmingScanning.txt', 8, 'SEED_EAI_294');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'newspapers', 1),
        (@MaCH, N'B', N'novels', 0),
        (@MaCH, N'C', N'comics', 0),
        (@MaCH, N'D', N'scripts', 0);
END
GO

-- Câu hỏi SEED_EAI_295 (Viet - Trình độ 5 - Dạng TuLuan - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_295')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 5, N'TuLuan', N'Viet', N'Kho', N'IELTS Writing Task 2 Problem-Solution Prompt:
''In many modern megacities, traffic congestion and air pollution have reached critical levels. Discuss the primary causes of this phenomenon and propose viable governmental solutions.''
Provide a structured outline including 2 main causes and 2 corresponding solutions.',
            NULL, NULL, N'Đề bài yêu cầu lập dàn ý chi tiết phân tích 2 nguyên nhân (quá tải xe cá nhân, cơ sở hạ tầng thiếu đồng bộ) và 2 giải pháp (phát triển giao thông công cộng metro, đánh thuế phát thải carbon).', N'Đề bài yêu cầu lập dàn ý chi tiết phân tích 2 nguyên nhân (quá tải xe cá nhân, cơ sở hạ tầng thiếu đồng bộ) và 2 giải pháp (phát triển giao thông công cộng metro, đánh thuế phát thải carbon).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 11 - Hướng dẫn dàn bài Problem & Solution chuẩn Band 7.5+.', N'Extracted', N'BaiHoc_11_ProblemSolutionEssay.txt', 11, 'SEED_EAI_295');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'RUBRIC', N'Causes: Private vehicle overload & inadequate public transit. Solutions: Metro expansion & Congestion pricing.', 1);
END
GO

-- Câu hỏi SEED_EAI_296 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_296')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'In IELTS Reading, how should you transfer your answers to the Answer Sheet?',
            NULL, NULL, N'Không giống như Listening (có thêm thời gian chép bài ở cuối bài thi giấy), IELTS Reading KHÔNG CÓ thời gian chuyển đáp án riêng; thí sinh phải ghi trực tiếp đáp án vào phiếu trong vòng 60 phút.', N'Không giống như Listening (có thêm thời gian chép bài ở cuối bài thi giấy), IELTS Reading KHÔNG CÓ thời gian chuyển đáp án riêng; thí sinh phải ghi trực tiếp đáp án vào phiếu trong vòng 60 phút.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 8 - Lưu ý chí mạng về thời gian làm bài IELTS Reading: Ghi trực tiếp đáp án trong 60 phút.', N'Extracted', N'BaiHoc_08_IeltsSkimmingScanning.txt', 8, 'SEED_EAI_296');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Wait for the 10-minute transfer period at the end of the test.', 0),
        (@MaCH, N'B', N'Write answers directly onto the answer sheet as you complete each passage, because no extra transfer time is provided.', 1),
        (@MaCH, N'C', N'Tell the invigilator your answers verbally.', 0),
        (@MaCH, N'D', N'Only write answers on the question booklet.', 0);
END
GO

-- Câu hỏi SEED_EAI_297 (Nghe - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_297')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 4, N'DungSai', N'Nghe', N'De', N'True or False: In IELTS Listening, writing ''10 Downing St'' when the audio says ''10 Downing Street'' is acceptable if word count permits.',
            NULL, NULL, N'Đúng. Viết tắt thông dụng như ''St'' cho ''Street'' hoặc ''Ave'' cho ''Avenue'' được chấp nhận nếu đúng nghĩa và trong giới hạn số từ.', N'Đúng. Viết tắt thông dụng như ''St'' cho ''Street'' hoặc ''Ave'' cho ''Avenue'' được chấp nhận nếu đúng nghĩa và trong giới hạn số từ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 12 - Quy tắc chấp nhận chữ viết tắt địa chỉ thông dụng trong IELTS Listening.', N'Extracted', N'BaiHoc_12_IeltsListeningSec3.txt', 12, 'SEED_EAI_297');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_298 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_298')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (6, 5, N'TracNghiem', N'Doc', N'Kho', N'What does the Latin abbreviation ''et al.'' mean in academic in-text citations (e.g., Smith et al., 2024)?',
            NULL, NULL, N'''et al.'' là viết tắt của ''et alia'' (nghĩa là ''và các cộng sự'' - and others), dùng khi bài báo có từ 3 tác giả trở lên.', N'''et al.'' là viết tắt của ''et alia'' (nghĩa là ''và các cộng sự'' - and others), dùng khi bài báo có từ 3 tác giả trở lên.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 8 - Trích dẫn học thuật quốc tế: et al. (and others).', N'Extracted', N'BaiHoc_08_IeltsSkimmingScanning.txt', 8, 'SEED_EAI_298');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'And others (et alia)', 1),
        (@MaCH, N'B', N'At all times', 0),
        (@MaCH, N'C', N'End of article', 0),
        (@MaCH, N'D', N'Edited along', 0);
END
GO

-- Câu hỏi SEED_EAI_299 (Viet - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_299')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (7, 4, N'DienKhuyet', N'Viet', N'TrungBinh', N'Fill in the blank with an academic adverb: ''Public transit investments have _______ reduced carbon emissions in several European metropolitan areas.''',
            NULL, NULL, N'''substantially'' (đáng kể) là phó từ học thuật C1 chuẩn xác để mô tả mức độ tác động.', N'''substantially'' (đáng kể) là phó từ học thuật C1 chuẩn xác để mô tả mức độ tác động.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 11 - Phó từ chỉ mức độ học thuật: Substantially, significantly, considerably.', N'Extracted', N'BaiHoc_11_ProblemSolutionEssay.txt', 11, 'SEED_EAI_299');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'substantially', 1),
        (@MaCH, N'B', N'a lot', 0),
        (@MaCH, N'C', N'very much', 0),
        (@MaCH, N'D', N'super', 0);
END
GO

-- Câu hỏi SEED_EAI_300 (Nghe - Trình độ 5 - Dạng TraLoiNgan - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_300')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (8, 5, N'TraLoiNgan', N'Nghe', N'Kho', N'In Section 4, what is the term for a scholarly article that undergoes rigorous evaluation by other experts in the same field before publication?',
            NULL, NULL, N'''Peer-reviewed journal'' hoặc ''Peer review'' (bài báo được bình duyệt học thuật).', N'''Peer-reviewed journal'' hoặc ''Peer review'' (bài báo được bình duyệt học thuật).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 13 - Thuật ngữ quy trình nghiên cứu học thuật: Peer-reviewed journal.', N'Extracted', N'BaiHoc_13_IeltsListeningSec4.txt', 13, 'SEED_EAI_300');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Peer-reviewed article / journal', 1),
        (@MaCH, N'B', N'Popular magazine', 0),
        (@MaCH, N'C', N'Blog post', 0),
        (@MaCH, N'D', N'Daily tabloid', 0);
END
GO

PRINT N'Đã hoàn thành nạp Batch 06 (SEED_EAI_251 -> SEED_EAI_300) thành công!';
GO
