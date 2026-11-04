-- ====================================================================================
-- SEED QUESTIONS BATCH: Batch 08 (SEED_EAI_351 -> SEED_EAI_400)
-- Số lượng câu hỏi: 50
-- Đảm bảo: Idempotent (Chạy lại nhiều lần không trùng lặp)
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Đang nạp Batch 08 (SEED_EAI_351 -> SEED_EAI_400) (50 câu hỏi)...';

-- Câu hỏi SEED_EAI_351 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_351')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'According to the interview guide ''Tell Me About Yourself'', what is the recommended 3-part framework for crafting a compelling response?',
            NULL, NULL, N'Công thức chuẩn mực quốc tế: ''Present - Past - Future'' (Hiện tại: vai trò hiện tại & thế mạnh -> Quá khứ: kinh nghiệm & thành tựu -> Tương lai: lý do ứng tuyển vào vị trí này).', N'Công thức chuẩn mực quốc tế: ''Present - Past - Future'' (Hiện tại: vai trò hiện tại & thế mạnh -> Quá khứ: kinh nghiệm & thành tựu -> Tương lai: lý do ứng tuyển vào vị trí này).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Công thức Present - Past - Future trong phỏng vấn tuyển dụng.', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_351');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Childhood - High School - University', 0),
        (@MaCH, N'B', N'Present (current role & strengths) - Past (relevant experience & accomplishments) - Future (why this role & company)', 1),
        (@MaCH, N'C', N'Salary history - Personal debt - Family tree', 0),
        (@MaCH, N'D', N'Hobbies - Favorite food - Music tastes', 0);
END
GO

-- Câu hỏi SEED_EAI_352 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_352')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'What is the biggest mistake candidates make when answering ''Tell Me About Yourself''?',
            NULL, NULL, N'Sai lầm lớn nhất là đọc lại nguyên văn từng dòng trong bản CV hoặc kể lể chi tiết tiểu sử cá nhân đời tư từ thời thơ ấu.', N'Sai lầm lớn nhất là đọc lại nguyên văn từng dòng trong bản CV hoặc kể lể chi tiết tiểu sử cá nhân đời tư từ thời thơ ấu.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Các sai lầm chí mạng cần tránh: Đọc lại CV, kể chuyện đời tư.', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_352');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Smiling politely at the interviewer', 0),
        (@MaCH, N'B', N'Reciting their entire resume chronologically or recounting personal life stories irrelevant to the job', 1),
        (@MaCH, N'C', N'Mentioning their core technical skills', 0),
        (@MaCH, N'D', N'Speaking clearly in professional English', 0);
END
GO

-- Câu hỏi SEED_EAI_353 (Noi - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_353')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DienKhuyet', N'Noi', N'TrungBinh', N'Fill in the blank in a strong interview opening: ''Currently, I am a software engineer _______ in cloud-native microservices architecture.''',
            NULL, NULL, N'''specializing'' (chuyên về mảng).', N'''specializing'' (chuyên về mảng).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Mẫu câu mở đầu phần Present: Currently, I am a [role] specializing in...', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_353');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'specializing', 1),
        (@MaCH, N'B', N'special', 0),
        (@MaCH, N'C', N'specially', 0),
        (@MaCH, N'D', N'specialty', 0);
END
GO

-- Câu hỏi SEED_EAI_354 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_354')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'De', N'What is the ideal time duration for answering ''Tell Me About Yourself'' in an interview?',
            NULL, NULL, N'Thời lượng tối ưu cho câu trả lời là khoảng 90 đến 120 giây (1.5 - 2 phút).', N'Thời lượng tối ưu cho câu trả lời là khoảng 90 đến 120 giây (1.5 - 2 phút).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Độ dài tối ưu: 90 đến 120 giây.', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_354');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'10 seconds', 0),
        (@MaCH, N'B', N'90 to 120 seconds', 1),
        (@MaCH, N'C', N'15 minutes', 0),
        (@MaCH, N'D', N'30 minutes', 0);
END
GO

-- Câu hỏi SEED_EAI_355 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_355')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DungSai', N'Doc', N'De', N'True or False: Every achievement shared in ''Tell Me About Yourself'' should be tailored directly to the specific job requirements listed in the company''s job description.',
            NULL, NULL, N'Đúng. Mọi thành tựu chia sẻ cần được may đo trực tiếp để chứng minh bạn là mảnh ghép hoàn hảo cho vị trí đang tuyển.', N'Đúng. Mọi thành tựu chia sẻ cần được may đo trực tiếp để chứng minh bạn là mảnh ghép hoàn hảo cho vị trí đang tuyển.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Nguyên tắc May đo câu trả lời (Tailoring to the Job Description).', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_355');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_356 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_356')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'In behavioral interviews, what does the ''STAR'' acronym stand for?',
            NULL, NULL, N'''STAR'' là viết tắt của: Situation (Tình huống), Task (Nhiệm vụ), Action (Hành động cụ thể của bạn), Result (Kết quả định lượng đạt được).', N'''STAR'' là viết tắt của: Situation (Tình huống), Task (Nhiệm vụ), Action (Hành động cụ thể của bạn), Result (Kết quả định lượng đạt được).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Định nghĩa mô hình STAR: Situation, Task, Action, Result.', N'Extracted', N'02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx', NULL, 'SEED_EAI_356');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Strategy, Timing, Analysis, Review', 0),
        (@MaCH, N'B', N'Situation, Task, Action, Result', 1),
        (@MaCH, N'C', N'Software, Testing, Automation, Release', 0),
        (@MaCH, N'D', N'Salary, Title, Allowance, Reward', 0);
END
GO

-- Câu hỏi SEED_EAI_357 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_357')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Doc', N'Kho', N'Which part of the STAR method should receive the largest proportion (approximately 60%) of your answer time?',
            NULL, NULL, N'Phần ''Action'' (Hành động bạn đã thực hiện) là trọng tâm quan trọng nhất mà nhà tuyển dụng muốn nghe để đánh giá năng lực thực chiến và kỹ năng giải quyết vấn đề.', N'Phần ''Action'' (Hành động bạn đã thực hiện) là trọng tâm quan trọng nhất mà nhà tuyển dụng muốn nghe để đánh giá năng lực thực chiến và kỹ năng giải quyết vấn đề.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Phân bổ thời lượng mô hình STAR: Action chiếm 50-60% dung lượng.', N'Extracted', N'02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx', NULL, 'SEED_EAI_357');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Situation (complaining about old colleagues)', 0),
        (@MaCH, N'B', N'Action (the specific steps, technologies, and decisions you took to resolve the challenge)', 1),
        (@MaCH, N'C', N'Task (describing the project assignment)', 0),
        (@MaCH, N'D', N'Waiting silently', 0);
END
GO

-- Câu hỏi SEED_EAI_358 (Viet - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_358')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'VietLaiCau', N'Viet', N'TrungBinh', N'Convert the generic result into an impactful STAR outcome: ''Our team improved system performance a lot.''',
            NULL, NULL, N'''As a result, we reduced server latency by 45% and scaled our daily active users from 10k to 50k without downtime.'' cung cấp số liệu định lượng cụ thể, minh chứng rõ ràng.', N'''As a result, we reduced server latency by 45% and scaled our daily active users from 10k to 50k without downtime.'' cung cấp số liệu định lượng cụ thể, minh chứng rõ ràng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Quy tắc trình bày phần Result: Định lượng hóa bằng số liệu cụ thể.', N'Applied', N'02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx', NULL, 'SEED_EAI_358');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'As a result of our optimization, API latency dropped by 45%, and the platform handled 50,000 concurrent users without downtime.', 1),
        (@MaCH, N'B', N'The code was faster and boss was super happy.', 0),
        (@MaCH, N'C', N'Performance was good because I coded well.', 0),
        (@MaCH, N'D', N'Everything improved by unknown amounts.', 0);
END
GO

-- Câu hỏi SEED_EAI_359 (Noi - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_359')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DienKhuyet', N'Noi', N'TrungBinh', N'Fill in the blank: ''In the STAR Action section, candidates should use "I" rather than "_______" to highlight their individual contribution to the team.''',
            NULL, NULL, N'Khi trả lời phỏng vấn hành vi, phải dùng ''I'' để nêu bật đóng góp cá nhân, tránh dùng ''we'' chung chung làm mờ vai trò của bản thân.', N'Khi trả lời phỏng vấn hành vi, phải dùng ''I'' để nêu bật đóng góp cá nhân, tránh dùng ''we'' chung chung làm mờ vai trò của bản thân.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Lưu ý đại từ nhân xưng: Dùng ''I'' thay vì ''we'' trong phần Action.', N'Extracted', N'02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx', NULL, 'SEED_EAI_359');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'we', 1),
        (@MaCH, N'B', N'they', 0),
        (@MaCH, N'C', N'you', 0),
        (@MaCH, N'D', N'he', 0);
END
GO

-- Câu hỏi SEED_EAI_360 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_360')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DungSai', N'Doc', N'De', N'True or False: An effective STAR Result can also include reflective lessons learned (what you would do differently in hindsight).',
            NULL, NULL, N'Đúng. Nêu bài học kinh nghiệm (Lesson learned) thể hiện tư duy cầu thị (growth mindset) được đánh giá rất cao.', N'Đúng. Nêu bài học kinh nghiệm (Lesson learned) thể hiện tư duy cầu thị (growth mindset) được đánh giá rất cao.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Mở rộng phần Result: Bài học kinh nghiệm và tư duy hoàn thiện bản thân.', N'Extracted', N'02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx', NULL, 'SEED_EAI_360');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_361 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_361')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'Kho', N'What are the three most powerful ''Hook'' techniques for opening an international presentation?',
            NULL, NULL, N'3 kỹ thuật Hook kinh điển: 1. Đưa ra một số liệu thống kê giật mình; 2. Đặt một câu hỏi khiêu khích tư duy; 3. Kể một câu chuyện ngắn có tính liên hệ sâu sắc.', N'3 kỹ thuật Hook kinh điển: 1. Đưa ra một số liệu thống kê giật mình; 2. Đặt một câu hỏi khiêu khích tư duy; 3. Kể một câu chuyện ngắn có tính liên hệ sâu sắc.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - 3 Chiến thuật Hook kinh điển: Surprising statistic, Provocative question, Personal narrative.', N'Extracted', N'01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx', NULL, 'SEED_EAI_361');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Clearing throat, adjusting glasses, reading agenda', 0),
        (@MaCH, N'B', N'A shocking statistical data point, a thought-provoking question, or a compelling brief story', 1),
        (@MaCH, N'C', N'Apologizing for being nervous, checking microphone repeatedly, coughing', 0),
        (@MaCH, N'D', N'Complaining about hotel traffic and bad weather', 0);
END
GO

-- Câu hỏi SEED_EAI_362 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_362')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'TrungBinh', N'Analyze this opening hook: ''Did you know that 85% of corporate cyberattacks stem from human error rather than technical flaws?'' Which technique is this?',
            NULL, NULL, N'Đây là kỹ thuật ''Startling Statistic Hook'' (Mở đầu bằng số liệu thống kê giật mình gây tò mò).', N'Đây là kỹ thuật ''Startling Statistic Hook'' (Mở đầu bằng số liệu thống kê giật mình gây tò mò).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Ví dụ phân tích Startling Statistic Hook.', N'Extracted', N'01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx', NULL, 'SEED_EAI_362');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Startling Statistic Hook', 1),
        (@MaCH, N'B', N'Fictional fairy tale', 0),
        (@MaCH, N'C', N'Self-deprecating humor', 0),
        (@MaCH, N'D', N'Product complaint', 0);
END
GO

-- Câu hỏi SEED_EAI_363 (Noi - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_363')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'DienKhuyet', N'Noi', N'Kho', N'Fill in the blank: ''After delivering the hook, the speaker must establish the WIIFM factor: "What''s In It For _______?"''',
            NULL, NULL, N'''WIIFM'' là nguyên tắc tâm lý khán giả: ''What''s In It For Me?'' (Bài nói này mang lại giá trị gì cho tôi/người nghe?).', N'''WIIFM'' là nguyên tắc tâm lý khán giả: ''What''s In It For Me?'' (Bài nói này mang lại giá trị gì cho tôi/người nghe?).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Nguyên tắc tâm lý WIIFM (What''s In It For Me).', N'Extracted', N'01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx', NULL, 'SEED_EAI_363');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Me', 1),
        (@MaCH, N'B', N'You', 0),
        (@MaCH, N'C', N'Us', 0),
        (@MaCH, N'D', N'Them', 0);
END
GO

-- Câu hỏi SEED_EAI_364 (Noi - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_364')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'DungSai', N'Noi', N'De', N'True or False: Opening a presentation by saying ''I didn''t have much time to prepare this slide deck'' builds credibility with executive audiences.',
            NULL, NULL, N'Sai. Tuyệt đối không bao giờ mở đầu bằng lời xin lỗi về việc thiếu chuẩn bị vì nó phá hủy hoàn toàn uy tín chuyên môn của diễn giả.', N'Sai. Tuyệt đối không bao giờ mở đầu bằng lời xin lỗi về việc thiếu chuẩn bị vì nó phá hủy hoàn toàn uy tín chuyên môn của diễn giả.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Những câu mở đầu phá hủy uy tín cần tránh tuyệt đối.', N'Extracted', N'01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx', NULL, 'SEED_EAI_364');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_365 (Viet - Trình độ 5 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_365')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'VietLaiCau', N'Viet', N'Kho', N'Turn this boring opening into an engaging presentation hook: ''Today I will talk about our company''s new AI software.''',
            NULL, NULL, N'''Imagine reducing your customer support turnaround time from four hours to four seconds. Today, I am excited to reveal how our new AI platform makes that a reality.''', N'''Imagine reducing your customer support turnaround time from four hours to four seconds. Today, I am excited to reveal how our new AI platform makes that a reality.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Bài tập thực hành chuyển đổi mở bài nhàm chán thành Hook truyền cảm hứng.', N'Applied', N'01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx', NULL, 'SEED_EAI_365');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Imagine reducing customer support turnaround from four hours to four seconds. Today, I will reveal how our AI engine achieves exactly that.', 1),
        (@MaCH, N'B', N'I have some AI slides to show today if you care.', 0),
        (@MaCH, N'C', N'Software is being discussed by me today.', 0),
        (@MaCH, N'D', N'Listen to me because I have software.', 0);
END
GO

-- Câu hỏi SEED_EAI_366 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_366')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'Kho', N'In high-stakes executive Q&A, what should a speaker do before immediately answering an aggressive or hostile question?',
            NULL, NULL, N'Kỹ thuật chuyên nghiệp: Giữ bình tĩnh, dừng lại 2 giây, cảm ơn và diễn đạt lại (rephrase) câu hỏi bằng ngôn từ trung tính nhằm giảm tính công kích.', N'Kỹ thuật chuyên nghiệp: Giữ bình tĩnh, dừng lại 2 giây, cảm ơn và diễn đạt lại (rephrase) câu hỏi bằng ngôn từ trung tính nhằm giảm tính công kích.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Kỹ thuật Rephrasing và De-escalating câu hỏi công kích.', N'Extracted', N'02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx', NULL, 'SEED_EAI_366');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Shout back angrily at the attendee', 0),
        (@MaCH, N'B', N'Pause, thank the questioner, and rephrase the question using neutral, objective terminology', 1),
        (@MaCH, N'C', N'Pretend not to hear and look out the window', 0),
        (@MaCH, N'D', N'Call security immediately', 0);
END
GO

-- Câu hỏi SEED_EAI_367 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_367')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'Kho', N'Which phrase politely defuses a multi-part, rambling question from an audience member?',
            NULL, NULL, N'''Thank you. To ensure I address your core concern, are you primarily asking about our data privacy compliance?'' giúp cô đọng trọng tâm câu hỏi.', N'''Thank you. To ensure I address your core concern, are you primarily asking about our data privacy compliance?'' giúp cô đọng trọng tâm câu hỏi.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Xử lý câu hỏi lan man (Rambling questions).', N'Extracted', N'02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx', NULL, 'SEED_EAI_367');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'You talk too much, stop talking.', 0),
        (@MaCH, N'B', N'Thank you. To ensure I address your core concern, are you primarily asking about our data privacy compliance?', 1),
        (@MaCH, N'C', N'I have no idea what you just said.', 0),
        (@MaCH, N'D', N'Sit down please.', 0);
END
GO

-- Câu hỏi SEED_EAI_368 (Noi - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_368')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'DienKhuyet', N'Noi', N'Kho', N'Fill in the blank with a diplomatic phrase: ''That is an insightful observation; however, our empirical data indicates a somewhat _______ trend.''',
            NULL, NULL, N'''different'' (một xu hướng hơi khác biệt) là cách bác bỏ ý kiến đối lập một cách lịch thiệp và khách quan.', N'''different'' (một xu hướng hơi khác biệt) là cách bác bỏ ý kiến đối lập một cách lịch thiệp và khách quan.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Mẫu câu phản biện lịch sự (Diplomatic disagreement).', N'Extracted', N'02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx', NULL, 'SEED_EAI_368');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'different', 1),
        (@MaCH, N'B', N'stupid', 0),
        (@MaCH, N'C', N'terrible', 0),
        (@MaCH, N'D', N'worthless', 0);
END
GO

-- Câu hỏi SEED_EAI_369 (Doc - Trình độ 5 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_369')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'DungSai', N'Doc', N'De', N'True or False: Repeating or paraphrasing an audience question benefits everyone in the room because other attendees may not have heard the question clearly.',
            NULL, NULL, N'Đúng. Lặp lại câu hỏi vừa giúp toàn thể hội trường nghe rõ, vừa giúp diễn giả có thêm 3-5 giây suy nghĩ câu trả lời thấu đáo.', N'Đúng. Lặp lại câu hỏi vừa giúp toàn thể hội trường nghe rõ, vừa giúp diễn giả có thêm 3-5 giây suy nghĩ câu trả lời thấu đáo.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Lợi ích của việc lặp lại câu hỏi trước toàn thể hội trường.', N'Extracted', N'02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx', NULL, 'SEED_EAI_369');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_370 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_370')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'Kho', N'How do you smoothly bridge back to your core presentation message when answering a tangential question?',
            NULL, NULL, N'Kỹ thuật ''Bridging'': Trả lời ngắn gọn câu hỏi rồi dùng cụm nối ''And that ties directly into our broader mission to...'' để đưa câu chuyện trở lại thông điệp trọng tâm.', N'Kỹ thuật ''Bridging'': Trả lời ngắn gọn câu hỏi rồi dùng cụm nối ''And that ties directly into our broader mission to...'' để đưa câu chuyện trở lại thông điệp trọng tâm.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Kỹ thuật Bridging (Cầu nối thông điệp).', N'Extracted', N'02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx', NULL, 'SEED_EAI_370');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Ignore the question and tell everyone to look at slide 1 again.', 0),
        (@MaCH, N'B', N'Answer briefly, then use a transitional bridge like ''And that connects directly with our primary objective of...''', 1),
        (@MaCH, N'C', N'Tell the attendee that their question is off-topic and ban them.', 0),
        (@MaCH, N'D', N'End the Q&A abruptly.', 0);
END
GO

-- Câu hỏi SEED_EAI_371 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_371')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Doc', N'Kho', N'In the EdTech startup pitch deck summary, what core customer pain point is addressed?',
            NULL, NULL, N'Vấn đề cốt lõi: Người học tiếng Anh thiếu môi trường luyện tập phản xạ thực tế cá nhân hóa và chi phí gia sư 1-1 bản xứ quá đắt đỏ.', N'Vấn đề cốt lõi: Người học tiếng Anh thiếu môi trường luyện tập phản xạ thực tế cá nhân hóa và chi phí gia sư 1-1 bản xứ quá đắt đỏ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 2: The Problem: High 1-on-1 tutoring costs and lack of personalized adaptive practice.', N'Extracted', N'03_Slide_Pitching_Goi_von_Startup_EdTech.pdf', NULL, 'SEED_EAI_371');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A shortage of physical pencils in school classrooms', 0),
        (@MaCH, N'B', N'Prohibitive costs of private 1-on-1 tutoring and lack of scalable, personalized speaking practice', 1),
        (@MaCH, N'C', N'The internet being too fast for students', 0),
        (@MaCH, N'D', N'Students disliking mobile games', 0);
END
GO

-- Câu hỏi SEED_EAI_372 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_372')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Doc', N'Kho', N'What is the EdTech startup''s proposed technology solution according to the pitch deck?',
            NULL, NULL, N'Giải pháp: Ứng dụng AI Voice Agent tương tác thời gian thực, cung cấp phản hồi ngữ pháp tức thì và lộ trình học cá nhân hóa theo CEFR.', N'Giải pháp: Ứng dụng AI Voice Agent tương tác thời gian thực, cung cấp phản hồi ngữ pháp tức thì và lộ trình học cá nhân hóa theo CEFR.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 3: The Solution: Real-time AI Voice Tutor with personalized CEFR learning paths.', N'Extracted', N'03_Slide_Pitching_Goi_von_Startup_EdTech.pdf', NULL, 'SEED_EAI_372');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A printed dictionary textbook', 0),
        (@MaCH, N'B', N'An AI-powered interactive voice tutor delivering personalized feedback and adaptive curriculum at 1/10th the cost', 1),
        (@MaCH, N'C', N'A traditional chalkboard classroom chain', 0),
        (@MaCH, N'D', N'A social media messaging app for celebrities', 0);
END
GO

-- Câu hỏi SEED_EAI_373 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_373')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'DienKhuyet', N'TuVung', N'Kho', N'Fill in the business model term: ''The EdTech startup operates on a _______ model offering free baseline access with tiered monthly subscriptions.''',
            NULL, NULL, N'''Freemium'' là mô hình kinh doanh phổ biến cho phép dùng thử miễn phí các tính năng cơ bản và trả phí để mở khóa tính năng nâng cao.', N'''Freemium'' là mô hình kinh doanh phổ biến cho phép dùng thử miễn phí các tính năng cơ bản và trả phí để mở khóa tính năng nâng cao.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 5: Business Model: Freemium with B2C subscriptions and B2B school licensing.', N'Extracted', N'03_Slide_Pitching_Goi_von_Startup_EdTech.pdf', NULL, 'SEED_EAI_373');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Freemium', 1),
        (@MaCH, N'B', N'Charity', 0),
        (@MaCH, N'C', N'Donation', 0),
        (@MaCH, N'D', N'Barter', 0);
END
GO

-- Câu hỏi SEED_EAI_374 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_374')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'TuVung', N'Kho', N'What metric does ''CAC'' represent in startup growth slides?',
            NULL, NULL, N'''CAC'' là viết tắt của ''Customer Acquisition Cost'' (Chi phí để thu hút được một khách hàng mới).', N'''CAC'' là viết tắt của ''Customer Acquisition Cost'' (Chi phí để thu hút được một khách hàng mới).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 6: Unit Economics: LTV (Lifetime Value) to CAC ratio.', N'Extracted', N'03_Slide_Pitching_Goi_von_Startup_EdTech.pdf', NULL, 'SEED_EAI_374');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Customer Acquisition Cost', 1),
        (@MaCH, N'B', N'Corporate Account Credit', 0),
        (@MaCH, N'C', N'Cash Asset Calculation', 0),
        (@MaCH, N'D', N'Client Allowance Charge', 0);
END
GO

-- Câu hỏi SEED_EAI_375 (Doc - Trình độ 5 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_375')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'DungSai', N'Doc', N'De', N'True or False: In a venture pitch deck, ''The Ask'' slide specifies the exact amount of capital being raised and how the funds will be allocated across engineering, marketing, and expansion.',
            NULL, NULL, N'Đúng. Slide ''The Ask'' (Kêu gọi vốn) nêu rõ số vốn cần huy động và kế hoạch phân bổ sử dụng vốn cụ thể.', N'Đúng. Slide ''The Ask'' (Kêu gọi vốn) nêu rõ số vốn cần huy động và kế hoạch phân bổ sử dụng vốn cụ thể.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 10: The Ask: Capital requirements and use of funds allocation.', N'Extracted', N'03_Slide_Pitching_Goi_von_Startup_EdTech.pdf', NULL, 'SEED_EAI_375');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_376 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_376')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'How should a candidate answer the interview question: ''What is your greatest weakness?''',
            NULL, NULL, N'Chia sẻ một điểm yếu có thật nhưng không mang tính chí mạng, kèm theo các hành động cụ thể bạn đang chủ động rèn luyện để khắc phục.', N'Chia sẻ một điểm yếu có thật nhưng không mang tính chí mạng, kèm theo các hành động cụ thể bạn đang chủ động rèn luyện để khắc phục.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Chiến lược trả lời câu hỏi điểm yếu (Genuine weakness + Actionable improvement).', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_376');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Claim that you have zero weaknesses and are completely perfect.', 0),
        (@MaCH, N'B', N'Name a genuine, non-critical weakness and detail the proactive steps you are currently taking to improve it.', 1),
        (@MaCH, N'C', N'Admit that you hate waking up early and dislike working in teams.', 0),
        (@MaCH, N'D', N'Say you work too hard and care too much as a disguised humblebrag.', 0);
END
GO

-- Câu hỏi SEED_EAI_377 (Noi - Trình độ 4 - Dạng SapXep - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_377')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'SapXep', N'Noi', N'TrungBinh', N'Order the four stages of the STAR interview answer:
(1) Actions taken by you
(2) Situation context
(3) Quantifiable results achieved
(4) Task and responsibilities',
            NULL, NULL, N'Thứ tự: Situation (2) -> Task (4) -> Action (1) -> Result (3).', N'Thứ tự: Situation (2) -> Task (4) -> Action (1) -> Result (3).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Trình tự 4 bước của mô hình STAR.', N'Extracted', N'02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx', NULL, 'SEED_EAI_377');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'(2) -> (4) -> (1) -> (3)', 1),
        (@MaCH, N'B', N'(1) -> (2) -> (3) -> (4)', 0),
        (@MaCH, N'C', N'(4) -> (2) -> (3) -> (1)', 0),
        (@MaCH, N'D', N'(3) -> (1) -> (4) -> (2)', 0);
END
GO

-- Câu hỏi SEED_EAI_378 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_378')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'De', N'At the end of an interview, the interviewer asks: ''Do you have any questions for us?'' What is the best strategy?',
            NULL, NULL, N'Luôn chuẩn bị sẵn 2-3 câu hỏi sâu sắc về văn hóa công ty, thách thức của vị trí hoặc định hướng công nghệ để thể hiện sự quan tâm thực sự.', N'Luôn chuẩn bị sẵn 2-3 câu hỏi sâu sắc về văn hóa công ty, thách thức của vị trí hoặc định hướng công nghệ để thể hiện sự quan tâm thực sự.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Phần kết thúc phỏng vấn: Đặt câu hỏi cho nhà tuyển dụng.', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_378');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Say ''No, I just want to know when my salary arrives.''', 0),
        (@MaCH, N'B', N'Ask thoughtful questions about the team''s engineering culture, upcoming challenges, and growth opportunities.', 1),
        (@MaCH, N'C', N'Ask nothing and leave immediately.', 0),
        (@MaCH, N'D', N'Ask personal questions about the interviewer''s family.', 0);
END
GO

-- Câu hỏi SEED_EAI_379 (Noi - Trình độ 5 - Dạng TuLuan - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_379')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TuLuan', N'Noi', N'Kho', N'Speaking Task Prompt (STAR Interview Simulation):
''Describe a time when you had to resolve a high-pressure conflict within your engineering team. What actions did you take, and what was the outcome?''
Structure your 2-minute response explicitly following the STAR methodology.',
            NULL, NULL, N'Tiêu chí chấm: Cấu trúc rõ ràng 4 phần (S-T-A-R), sử dụng từ vựng giải quyết xung đột (active listening, mediation, consensus), kết quả hòa giải thành công và giữ vững tiến độ dự án.', N'Tiêu chí chấm: Cấu trúc rõ ràng 4 phần (S-T-A-R), sử dụng từ vựng giải quyết xung đột (active listening, mediation, consensus), kết quả hòa giải thành công và giữ vững tiến độ dự án.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Tình huống mẫu: Xung đột nhóm (Team Conflict Resolution).', N'Extracted', N'02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx', NULL, 'SEED_EAI_379');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'RUBRIC', N'Situation & Task (25%), Action with diplomatic initiative (50%), Result & Lesson learned (25%)', 1);
END
GO

-- Câu hỏi SEED_EAI_380 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_380')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'Kho', N'What does the presentation acronym ''KISS'' stand for?',
            NULL, NULL, N'''Keep It Simple, Stupid'' (hoặc Keep It Short and Simple) nhắc nhở việc trình bày súc tích, tránh đưa thông tin rườm rà phức tạp.', N'''Keep It Simple, Stupid'' (hoặc Keep It Short and Simple) nhắc nhở việc trình bày súc tích, tránh đưa thông tin rườm rà phức tạp.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Nguyên tắc KISS trong thiết kế thông điệp thuyết trình.', N'Extracted', N'01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx', NULL, 'SEED_EAI_380');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Keep It Short and Simple (or Keep It Simple, Stupid)', 1),
        (@MaCH, N'B', N'Knowledge Is Super Special', 0),
        (@MaCH, N'C', N'Key Information System Software', 0),
        (@MaCH, N'D', N'Kindness Is So Sweet', 0);
END
GO

-- Câu hỏi SEED_EAI_381 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_381')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''Her strong interpersonal skills allowed her to build immediate _______ with interviewers.''',
            NULL, NULL, N'''rapport'' nghĩa là mối liên kết hòa hợp, thiện cảm tự nhiên giữa người với người.', N'''rapport'' nghĩa là mối liên kết hòa hợp, thiện cảm tự nhiên giữa người với người.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Nghệ thuật tạo thiện cảm trong phỏng vấn: Building rapport.', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_381');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'rapport', 1),
        (@MaCH, N'B', N'wall', 0),
        (@MaCH, N'C', N'barrier', 0),
        (@MaCH, N'D', N'conflict', 0);
END
GO

-- Câu hỏi SEED_EAI_382 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_382')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'Kho', N'When facing an audience question that criticizes your startup''s competitor analysis, which stance is most professional?',
            NULL, NULL, N'Thừa nhận thế mạnh của đối thủ một cách khách quan, sau đó làm nổi bật điểm khác biệt cốt lõi (unique differentiator) của sản phẩm mình.', N'Thừa nhận thế mạnh của đối thủ một cách khách quan, sau đó làm nổi bật điểm khác biệt cốt lõi (unique differentiator) của sản phẩm mình.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Ứng xử trước câu hỏi so sánh đối thủ cạnh tranh.', N'Extracted', N'02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx', NULL, 'SEED_EAI_382');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Insult the competitor and call their product garbage.', 0),
        (@MaCH, N'B', N'Acknowledge the competitor''s established presence respectfully, then articulate your unique value differentiator and proprietary technology.', 1),
        (@MaCH, N'C', N'Pretend the competitor does not exist.', 0),
        (@MaCH, N'D', N'Refuse to comment on anything related to the market.', 0);
END
GO

-- Câu hỏi SEED_EAI_383 (Noi - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_383')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DungSai', N'Noi', N'De', N'True or False: Using mock interviews with a peer or recording your video answers helps identify unconscious speech fillers like ''um'', ''ah'', and ''like''.',
            NULL, NULL, N'Đúng. Luyện tập phỏng vấn thử (mock interviews) và quay video lại giúp nhận biết và loại bỏ các từ đệm vô nghĩa (filler words).', N'Đúng. Luyện tập phỏng vấn thử (mock interviews) và quay video lại giúp nhận biết và loại bỏ các từ đệm vô nghĩa (filler words).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Luyện tập thực tế: Loại bỏ Filler words.', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_383');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_384 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_384')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'TuVung', N'Kho', N'In investor terminology, what is ''Runway''?',
            NULL, NULL, N'''Runway'' (Đường băng tài chính) là số tháng mà một công ty khởi nghiệp có thể tiếp tục hoạt động với số tiền mặt hiện có trước khi hết vốn.', N'''Runway'' (Đường băng tài chính) là số tháng mà một công ty khởi nghiệp có thể tiếp tục hoạt động với số tiền mặt hiện có trước khi hết vốn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Chỉ số tài chính: Runway (thời gian hoạt động còn lại).', N'Extracted', N'03_Slide_Pitching_Goi_von_Startup_EdTech.pdf', NULL, 'SEED_EAI_384');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'An airport concrete strip for airplanes', 0),
        (@MaCH, N'B', N'The number of months a startup can operate before running out of money, calculated as cash reserves divided by monthly burn rate', 1),
        (@MaCH, N'C', N'A fashion catwalk model stage', 0),
        (@MaCH, N'D', N'The length of optical fiber cables', 0);
END
GO

-- Câu hỏi SEED_EAI_385 (Viet - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_385')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'VietLaiCau', N'Viet', N'TrungBinh', N'Rewrite the task description concisely: ''My boss told me that I had to fix the broken login screen.''',
            NULL, NULL, N'''I was tasked with resolving critical authentication failures on the user login portal.'' diễn đạt chuyên nghiệp và sắc sảo.', N'''I was tasked with resolving critical authentication failures on the user login portal.'' diễn đạt chuyên nghiệp và sắc sảo.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Cách diễn đạt phần Task trong STAR.', N'Applied', N'02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx', NULL, 'SEED_EAI_385');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I was tasked with resolving critical authentication failures on the user login portal.', 1),
        (@MaCH, N'B', N'Boss ordered me to do screens.', 0),
        (@MaCH, N'C', N'Screen was bad and my boss hated it.', 0),
        (@MaCH, N'D', N'I had task of clicking buttons.', 0);
END
GO

-- Câu hỏi SEED_EAI_386 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_386')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'What is the ''Rule of Three'' in rhetoric and presentation design?',
            NULL, NULL, N'''Quy tắc số ba'' (Rule of Three) dựa trên nguyên lý tâm lý học: Bộ não con người ghi nhớ thông tin tốt nhất khi được phân thành 3 cụm ý tưởng, 3 lợi ích hoặc 3 ví dụ.', N'''Quy tắc số ba'' (Rule of Three) dựa trên nguyên lý tâm lý học: Bộ não con người ghi nhớ thông tin tốt nhất khi được phân thành 3 cụm ý tưởng, 3 lợi ích hoặc 3 ví dụ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Quy tắc số ba (Rule of Three) trong thuật hùng biện.', N'Extracted', N'01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx', NULL, 'SEED_EAI_386');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Speaking for exactly three seconds', 0),
        (@MaCH, N'B', N'Grouping concepts, benefits, or examples into sets of three for maximum cognitive retention and rhetorical impact', 1),
        (@MaCH, N'C', N'Wearing three layers of jackets', 0),
        (@MaCH, N'D', N'Inviting three people on stage', 0);
END
GO

-- Câu hỏi SEED_EAI_387 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_387')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Complete the STAR Result statement: ''Our redesign led to a 35% increase in user _______.''',
            NULL, NULL, N'''retention'' (tỷ lệ giữ chân người dùng) là chỉ số sản phẩm then chốt.', N'''retention'' (tỷ lệ giữ chân người dùng) là chỉ số sản phẩm then chốt.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Các chỉ số đo lường kết quả công nghệ: User retention, engagement.', N'Extracted', N'02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx', NULL, 'SEED_EAI_387');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'retention', 1),
        (@MaCH, N'B', N'rejection', 0),
        (@MaCH, N'C', N'refusal', 0),
        (@MaCH, N'D', N'reluctance', 0);
END
GO

-- Câu hỏi SEED_EAI_388 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_388')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Doc', N'Kho', N'What is an ''Elevator Pitch'' in startup capital fundraising?',
            NULL, NULL, N'Bài giới thiệu siêu ngắn (30-60 giây) tóm tắt trọn vẹn vấn đề, giải pháp và giá trị cốt lõi của startup, đủ nhanh để trình bày trong một chuyến đi thang máy với nhà đầu tư.', N'Bài giới thiệu siêu ngắn (30-60 giây) tóm tắt trọn vẹn vấn đề, giải pháp và giá trị cốt lõi của startup, đủ nhanh để trình bày trong một chuyến đi thang máy với nhà đầu tư.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Khái niệm Elevator Pitch.', N'Extracted', N'03_Slide_Pitching_Goi_von_Startup_EdTech.pdf', NULL, 'SEED_EAI_388');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A mechanical inspection of elevators', 0),
        (@MaCH, N'B', N'A concise 30-to-60-second summary that articulates a venture''s core value proposition so compellingly that investors want to learn more', 1),
        (@MaCH, N'C', N'A speech given only in high-rise buildings', 0),
        (@MaCH, N'D', N'A conversation with elevator technicians', 0);
END
GO

-- Câu hỏi SEED_EAI_389 (Noi - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_389')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DungSai', N'Noi', N'De', N'True or False: Maintaining steady, natural eye contact with all interviewers on a panel (rather than looking solely at the most senior person) conveys inclusivity and confidence.',
            NULL, NULL, N'Đúng. Duy trì giao tiếp mắt đều khắp với tất cả các thành viên trong hội đồng phỏng vấn thể hiện sự tự tin và tôn trọng toàn diện.', N'Đúng. Duy trì giao tiếp mắt đều khắp với tất cả các thành viên trong hội đồng phỏng vấn thể hiện sự tự tin và tôn trọng toàn diện.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Giao tiếp phi ngôn ngữ trong phỏng vấn hội đồng (Panel interview).', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_389');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_390 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_390')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'Kho', N'Which concluding statement effectively closes an executive investment pitch?',
            NULL, NULL, N'''We invite you to join us in revolutionizing personalized language education. Thank you, and we welcome your questions.'' là lời kết thúc tự tin và chuyên nghiệp.', N'''We invite you to join us in revolutionizing personalized language education. Thank you, and we welcome your questions.'' là lời kết thúc tự tin và chuyên nghiệp.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 10: Closing statement and call to partnership.', N'Extracted', N'03_Slide_Pitching_Goi_von_Startup_EdTech.pdf', NULL, 'SEED_EAI_390');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'That''s all, give us money if you want.', 0),
        (@MaCH, N'B', N'We invite you to partner with us in revolutionizing global language education; thank you, and we look forward to your questions.', 1),
        (@MaCH, N'C', N'I''m exhausted, no more talking.', 0),
        (@MaCH, N'D', N'Bye everyone.', 0);
END
GO

-- Câu hỏi SEED_EAI_391 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_391')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What does the interview idiom ''hit the ground running'' mean?',
            NULL, NULL, N'''Hit the ground running'' nghĩa là bắt nhịp làm việc hiệu quả ngay từ ngày đầu tiên mà không mất thời gian hòa nhập chậm chạp.', N'''Hit the ground running'' nghĩa là bắt nhịp làm việc hiệu quả ngay từ ngày đầu tiên mà không mất thời gian hòa nhập chậm chạp.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Thành ngữ phỏng vấn tuyển dụng: Hit the ground running.', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_391');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'To trip and fall on the pavement', 0),
        (@MaCH, N'B', N'To start a new job or project enthusiastically, competently, and with immediate productivity', 1),
        (@MaCH, N'C', N'To run in a marathon race', 0),
        (@MaCH, N'D', N'To dig a hole in the ground', 0);
END
GO

-- Câu hỏi SEED_EAI_392 (Noi - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_392')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'DienKhuyet', N'Noi', N'TrungBinh', N'Fill in the blank: ''To keep audience engagement high, presenters should vary their vocal _______ (pitch, tone, and pacing).''',
            NULL, NULL, N'''variety'' (sự đa dạng ngữ điệu giọng nói): vocal variety.', N'''variety'' (sự đa dạng ngữ điệu giọng nói): vocal variety.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Kỹ thuật điều khiển giọng nói (Vocal variety).', N'Extracted', N'01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx', NULL, 'SEED_EAI_392');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'variety', 1),
        (@MaCH, N'B', N'silence', 0),
        (@MaCH, N'C', N'damage', 0),
        (@MaCH, N'D', N'shouting', 0);
END
GO

-- Câu hỏi SEED_EAI_393 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_393')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'In technical interviews, what is a ''Whiteboard Coding Challenge'' designed to assess?',
            NULL, NULL, N'''Thử thách code bảng'' kiểm tra tư duy thuật toán, kỹ năng giao tiếp truyền đạt giải pháp và cách ứng phó dưới áp lực, chứ không chỉ là cú pháp.', N'''Thử thách code bảng'' kiểm tra tư duy thuật toán, kỹ năng giao tiếp truyền đạt giải pháp và cách ứng phó dưới áp lực, chứ không chỉ là cú pháp.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Chuẩn bị phỏng vấn kỹ thuật: Whiteboard coding challenges.', N'Extracted', N'04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf', NULL, 'SEED_EAI_393');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Penmanship and whiteboard cleanliness', 0),
        (@MaCH, N'B', N'Algorithmic problem-solving, real-time communication of thought processes, and composure under pressure', 1),
        (@MaCH, N'C', N'Drawing artistic sketches', 0),
        (@MaCH, N'D', N'Speed reading abilities', 0);
END
GO

-- Câu hỏi SEED_EAI_394 (Noi - Trình độ 5 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_394')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'DungSai', N'Noi', N'De', N'True or False: Using the speaker''s name when answering a question from the audience (e.g., ''Thank you for that question, John'') builds personal rapport and commands executive respect.',
            NULL, NULL, N'Đúng. Xưng hô tên người hỏi thể hiện sự chú ý lắng nghe và tác phong ngoại giao lịch thiệp.', N'Đúng. Xưng hô tên người hỏi thể hiện sự chú ý lắng nghe và tác phong ngoại giao lịch thiệp.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Tương tác cá nhân hóa trong phần Q&A.', N'Extracted', N'02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx', NULL, 'SEED_EAI_394');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_395 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_395')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'Which response appropriately addresses why a candidate wants to leave their current employer?',
            NULL, NULL, N'Tập trung vào cơ hội phát triển nghề nghiệp mới thay vì chỉ trích tiêu cực về sếp hoặc công ty cũ.', N'Tập trung vào cơ hội phát triển nghề nghiệp mới thay vì chỉ trích tiêu cực về sếp hoặc công ty cũ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Trả lời lý do chuyển việc: Tập trung vào Career growth thay vì than phiền.', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_395');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'My current manager is toxic and I cannot stand my colleagues.', 0),
        (@MaCH, N'B', N'I have had valuable growth at my current company, and I am now ready for a new challenge where I can apply my cloud architecture expertise to larger scale systems.', 1),
        (@MaCH, N'C', N'The commute is five minutes longer than I like.', 0),
        (@MaCH, N'D', N'I just want higher pay with less work.', 0);
END
GO

-- Câu hỏi SEED_EAI_396 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_396')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'DienKhuyet', N'TuVung', N'Kho', N'Complete the startup valuation term: ''The valuation agreed upon before receiving new investment capital is the _______-money valuation.''',
            NULL, NULL, N'''Pre-money valuation'' là định giá doanh nghiệp trước khi nhận dòng vốn mới (khác với Post-money valuation = Pre-money + Vốn đầu tư).', N'''Pre-money valuation'' là định giá doanh nghiệp trước khi nhận dòng vốn mới (khác với Post-money valuation = Pre-money + Vốn đầu tư).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Thuật ngữ gọi vốn: Pre-money vs. Post-money valuation.', N'Extracted', N'03_Slide_Pitching_Goi_von_Startup_EdTech.pdf', NULL, 'SEED_EAI_396');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Pre', 1),
        (@MaCH, N'B', N'Post', 0),
        (@MaCH, N'C', N'Mid', 0),
        (@MaCH, N'D', N'After', 0);
END
GO

-- Câu hỏi SEED_EAI_397 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_397')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What is a ''Cover Letter'' submitted alongside a job resume?',
            NULL, NULL, N'''Cover Letter'' (Thư ứng tuyển) là bức thư ngắn 1 trang giới thiệu bản thân, giải thích lý do bạn quan tâm đến vị trí và nêu bật các phẩm chất phù hợp nhất.', N'''Cover Letter'' (Thư ứng tuyển) là bức thư ngắn 1 trang giới thiệu bản thân, giải thích lý do bạn quan tâm đến vị trí và nêu bật các phẩm chất phù hợp nhất.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Hồ sơ ứng tuyển hoàn chỉnh: Resume + Cover Letter.', N'Extracted', N'04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf', NULL, 'SEED_EAI_397');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A picture of the applicant''s house', 0),
        (@MaCH, N'B', N'A personalized formal letter accompanying a resume explaining why you are the ideal fit for the role', 1),
        (@MaCH, N'C', N'A receipt from university tuition payments', 0),
        (@MaCH, N'D', N'A blank document with a signature only', 0);
END
GO

-- Câu hỏi SEED_EAI_398 (Noi - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_398')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TracNghiem', N'Noi', N'Kho', N'What is the primary objective of a ''Call to Action'' (CTA) at the end of a product pitch?',
            NULL, NULL, N'Kêu gọi hành động (CTA) hướng dẫn khách hàng hoặc nhà đầu tư thực hiện bước đi cụ thể tiếp theo (ví dụ: đăng ký bản dùng thử, hẹn gặp trực tiếp thẩm định tài chính).', N'Kêu gọi hành động (CTA) hướng dẫn khách hàng hoặc nhà đầu tư thực hiện bước đi cụ thể tiếp theo (ví dụ: đăng ký bản dùng thử, hẹn gặp trực tiếp thẩm định tài chính).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Thiết kế Call to Action (CTA) trong Pitch Deck.', N'Extracted', N'03_Slide_Pitching_Goi_von_Startup_EdTech.pdf', NULL, 'SEED_EAI_398');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'To confuse the audience so they stay longer', 0),
        (@MaCH, N'B', N'To clearly instruct investors or clients on the exact next step to take (e.g., schedule a due-diligence meeting or sign up for a pilot)', 1),
        (@MaCH, N'C', N'To ask for free lunch vouchers', 0),
        (@MaCH, N'D', N'To show funny internet memes', 0);
END
GO

-- Câu hỏi SEED_EAI_399 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_399')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DungSai', N'Doc', N'De', N'True or False: Sending a polite thank-you email within 24 hours of completing a job interview reinforces your interest and leaves a lasting professional impression.',
            NULL, NULL, N'Đúng. Thư cảm ơn (Thank-you note) gửi trong vòng 24 giờ sau buổi phỏng vấn là nghi thức tuyển dụng quốc tế rất được đánh giá cao.', N'Đúng. Thư cảm ơn (Thank-you note) gửi trong vòng 24 giờ sau buổi phỏng vấn là nghi thức tuyển dụng quốc tế rất được đánh giá cao.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Nghi thức sau phỏng vấn: Post-interview Thank You email trong 24h.', N'Extracted', N'01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx', NULL, 'SEED_EAI_399');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_400 (Noi - Trình độ 5 - Dạng TuLuan - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_400')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 5, N'TuLuan', N'Noi', N'Kho', N'Speaking Task Prompt (Elevator Pitch Simulation):
''Deliver a 60-second elevator pitch introducing an innovative AI educational tool to an angel investor. Highlight the core problem, your technological solution, and the market potential.''
Provide a scripted 120-word pitch demonstration.',
            NULL, NULL, N'Tiêu chí chấm: Cấu trúc Hook ấn tượng (15s), nêu giải pháp AI cốt lõi (30s) và lời kêu gọi đầu tư mạnh mẽ (15s).', N'Tiêu chí chấm: Cấu trúc Hook ấn tượng (15s), nêu giải pháp AI cốt lõi (30s) và lời kêu gọi đầu tư mạnh mẽ (15s).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Bài tập thực hành thuyết trình gọi vốn 60 giây.', N'Extracted', N'03_Slide_Pitching_Goi_von_Startup_EdTech.pdf', NULL, 'SEED_EAI_400');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'RUBRIC', N'Hook & Problem (25%), Solution & Uniqueness (50%), Business Model & CTA (25%)', 1);
END
GO

PRINT N'Đã hoàn thành nạp Batch 08 (SEED_EAI_351 -> SEED_EAI_400) thành công!';
GO
