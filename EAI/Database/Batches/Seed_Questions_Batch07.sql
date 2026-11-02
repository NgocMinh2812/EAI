-- ====================================================================================
-- SEED QUESTIONS BATCH: Batch 07 (SEED_EAI_301 -> SEED_EAI_350)
-- Số lượng câu hỏi: 50
-- Đảm bảo: Idempotent (Chạy lại nhiều lần không trùng lặp)
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Đang nạp Batch 07 (SEED_EAI_301 -> SEED_EAI_350) (50 câu hỏi)...';

-- Câu hỏi SEED_EAI_301 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_301')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'In software engineering, what is the process of restructuring existing code without altering its external behavior called?',
            NULL, NULL, N'''Refactoring'' là quá trình tái cấu trúc và làm sạch mã nguồn nội bộ mà không làm thay đổi hành vi đầu ra bên ngoài của phần mềm.', N'''Refactoring'' là quá trình tái cấu trúc và làm sạch mã nguồn nội bộ mà không làm thay đổi hành vi đầu ra bên ngoài của phần mềm.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Thuật ngữ công nghệ phần mềm: Refactoring: Tái cấu trúc code.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_301');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Rebooting', 0),
        (@MaCH, N'B', N'Refactoring', 1),
        (@MaCH, N'C', N'Overclocking', 0),
        (@MaCH, N'D', N'Defragmenting', 0);
END
GO

-- Câu hỏi SEED_EAI_302 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_302')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'What does the database constraint ''Foreign Key'' ensure between relational tables?',
            NULL, NULL, N'''Foreign Key'' (Khóa ngoại) đảm bảo tính toàn vẹn tham chiếu (referential integrity) giữa hai bảng có liên kết với nhau.', N'''Foreign Key'' (Khóa ngoại) đảm bảo tính toàn vẹn tham chiếu (referential integrity) giữa hai bảng có liên kết với nhau.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Thuật ngữ Cơ sở dữ liệu: Foreign Key and Referential Integrity.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_302');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'It translates table column names into foreign languages.', 0),
        (@MaCH, N'B', N'It enforces referential integrity by linking a record in one table to the primary key of another table.', 1),
        (@MaCH, N'C', N'It encrypts stored passwords automatically.', 0),
        (@MaCH, N'D', N'It prevents computers from shutting down.', 0);
END
GO

-- Câu hỏi SEED_EAI_303 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_303')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'Doc', N'Kho', N'What is the key performance benefit of creating an ''Index'' on a frequently queried SQL database column?',
            NULL, NULL, N'Tạo chỉ mục (Index) giúp tăng tốc độ tìm kiếm và truy vấn dữ liệu từ O(N) xuống O(log N) bằng cấu trúc B-Tree.', N'Tạo chỉ mục (Index) giúp tăng tốc độ tìm kiếm và truy vấn dữ liệu từ O(N) xuống O(log N) bằng cấu trúc B-Tree.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Tối ưu hóa truy vấn Database: Database Indexing.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_303');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'It decreases the physical disk storage used by tables.', 0),
        (@MaCH, N'B', N'It significantly speeds up data retrieval operations by allowing the engine to locate records without scanning the entire table.', 1),
        (@MaCH, N'C', N'It automatically generates graphical charts.', 0),
        (@MaCH, N'D', N'It prevents duplicate user sign-ups completely.', 0);
END
GO

-- Câu hỏi SEED_EAI_304 (TuVung - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_304')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 3, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Complete the software term: ''Continuous Integration and Continuous _______ (CI/CD) automates software delivery pipelines.''',
            NULL, NULL, N'CI/CD là viết tắt của Continuous Integration và Continuous Deployment (hoặc Delivery).', N'CI/CD là viết tắt của Continuous Integration và Continuous Deployment (hoặc Delivery).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Khái niệm DevOps & Tự động hóa: CI/CD (Continuous Integration / Continuous Deployment).', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_304');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Deployment', 1),
        (@MaCH, N'B', N'Destruction', 0),
        (@MaCH, N'C', N'Decline', 0),
        (@MaCH, N'D', N'Dispute', 0);
END
GO

-- Câu hỏi SEED_EAI_305 (TuVung - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_305')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'DungSai', N'TuVung', N'De', N'True or False: In concurrent computing, a ''Deadlock'' refers to a state where two or more threads are permanently blocked, each waiting for a lock held by the other.',
            NULL, NULL, N'Đúng. ''Deadlock'' trong lập trình đồng thời là tình trạng bế tắc khi hai hay nhiều tiến trình khóa tài nguyên của nhau.', N'Đúng. ''Deadlock'' trong lập trình đồng thời là tình trạng bế tắc khi hai hay nhiều tiến trình khóa tài nguyên của nhau.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Khái niệm đa luồng và đồng thời: Concurrency and Deadlocks.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_305');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_306 (Noi - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_306')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 3, N'TracNghiem', N'Noi', N'De', N'What are the three core questions answered by every team member in a Daily Scrum Standup meeting?',
            NULL, NULL, N'3 câu hỏi kinh điển trong cuộc họp Daily Scrum: ''Hôm qua tôi đã làm gì?'', ''Hôm nay tôi sẽ làm gì?'', và ''Tôi có gặp vướng mắc/trở ngại gì không?''.', N'3 câu hỏi kinh điển trong cuộc họp Daily Scrum: ''Hôm qua tôi đã làm gì?'', ''Hôm nay tôi sẽ làm gì?'', và ''Tôi có gặp vướng mắc/trở ngại gì không?''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 26 - Cấu trúc 3 câu hỏi Daily Standup: What I did yesterday, what I will do today, any blockers.', N'Extracted', N'BaiHoc_26_DailyStandupScrum.txt', 26, 'SEED_EAI_306');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'What did I do yesterday? What will I do today? Are there any blockers or impediments?', 1),
        (@MaCH, N'B', N'Who is the boss? What time is lunch? When is salary day?', 0),
        (@MaCH, N'C', N'Why is the code broken? Who made the bug? Where is the coffee?', 0),
        (@MaCH, N'D', N'What is my job? Who are you? Where do we go?', 0);
END
GO

-- Câu hỏi SEED_EAI_307 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_307')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'In Agile development, what does the term ''Blocker'' (or Impediment) signify?',
            NULL, NULL, N'''Blocker'' là bất kỳ trở ngại hoặc vướng mắc kỹ thuật/quy trình nào cản trở lập trình viên hoàn thành nhiệm vụ.', N'''Blocker'' là bất kỳ trở ngại hoặc vướng mắc kỹ thuật/quy trình nào cản trở lập trình viên hoàn thành nhiệm vụ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 26 - Thuật ngữ Scrum: Blockers / Impediments.', N'Extracted', N'BaiHoc_26_DailyStandupScrum.txt', 26, 'SEED_EAI_307');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A physical lock on the office front gate', 0),
        (@MaCH, N'B', N'An obstacle or issue that prevents a team member from making progress on a task', 1),
        (@MaCH, N'C', N'A special software firewall tool', 0),
        (@MaCH, N'D', N'A keyboard key that is stuck', 0);
END
GO

-- Câu hỏi SEED_EAI_308 (Noi - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_308')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 3, N'DienKhuyet', N'Noi', N'De', N'Fill in the blank in a standup update: ''Yesterday I finished implementing the login API. Today I am going to write unit _______.''',
            NULL, NULL, N'''unit tests'' (các bài kiểm thử đơn vị).', N'''unit tests'' (các bài kiểm thử đơn vị).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 26 - Mẫu câu báo cáo tiến độ Sprint: ''Today I will write unit tests.''', N'Extracted', N'BaiHoc_26_DailyStandupScrum.txt', 26, 'SEED_EAI_308');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'tests', 1),
        (@MaCH, N'B', N'novels', 0),
        (@MaCH, N'C', N'games', 0),
        (@MaCH, N'D', N'movies', 0);
END
GO

-- Câu hỏi SEED_EAI_309 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_309')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 3, N'TracNghiem', N'Doc', N'De', N'What is the recommended maximum duration for an effective Daily Standup meeting?',
            NULL, NULL, N'Theo chuẩn Scrum quốc tế, buổi họp Daily Standup chỉ nên kéo dài tối đa 15 phút và tất cả mọi người đứng họp để giữ sự tập trung, nhanh gọn.', N'Theo chuẩn Scrum quốc tế, buổi họp Daily Standup chỉ nên kéo dài tối đa 15 phút và tất cả mọi người đứng họp để giữ sự tập trung, nhanh gọn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 26 - Thời lượng tiêu chuẩn của Daily Standup: 15 phút.', N'Extracted', N'BaiHoc_26_DailyStandupScrum.txt', 26, 'SEED_EAI_309');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'2 hours', 0),
        (@MaCH, N'B', N'15 minutes', 1),
        (@MaCH, N'C', N'45 minutes', 0),
        (@MaCH, N'D', N'1 full working day', 0);
END
GO

-- Câu hỏi SEED_EAI_310 (Viet - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_310')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'VietLaiCau', N'Viet', N'TrungBinh', N'Rewrite the informal blocker report professionally: ''The third party API is dead so I can''t do anything.''',
            NULL, NULL, N'''I am currently blocked by downtime on the third-party payment API; our team has escalated the issue to vendor support.''', N'''I am currently blocked by downtime on the third-party payment API; our team has escalated the issue to vendor support.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 26 - Kỹ năng báo cáo Blocker chuyên nghiệp trong họp kỹ thuật.', N'Applied', N'BaiHoc_26_DailyStandupScrum.txt', 26, 'SEED_EAI_310');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I am currently blocked by an outage on the third-party API, and we have escalated the issue to vendor support.', 1),
        (@MaCH, N'B', N'API died, not my problem anymore.', 0),
        (@MaCH, N'C', N'I am sleeping until API wakes up.', 0),
        (@MaCH, N'D', N'Third party ruined everything today.', 0);
END
GO

-- Câu hỏi SEED_EAI_311 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_311')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 4, N'TracNghiem', N'TuVung', N'Kho', N'In Machine Learning, what is ''Overfitting''?',
            NULL, NULL, N'''Overfitting'' (Quá khớp) xảy ra khi mô hình học thuộc lòng quá kỹ dữ liệu huấn luyện (training data), dẫn đến khả năng tổng quát hóa kém trên dữ liệu mới (test data).', N'''Overfitting'' (Quá khớp) xảy ra khi mô hình học thuộc lòng quá kỹ dữ liệu huấn luyện (training data), dẫn đến khả năng tổng quát hóa kém trên dữ liệu mới (test data).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Mục 2: Khái niệm Overfitting vs. Underfitting.', N'Extracted', N'01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx', NULL, 'SEED_EAI_311');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'When a computer program runs out of memory', 0),
        (@MaCH, N'B', N'When a model learns training data noise too closely, resulting in poor generalization on unseen test data', 1),
        (@MaCH, N'C', N'When a neural network has zero parameters', 0),
        (@MaCH, N'D', N'When the training dataset is deleted accidentally', 0);
END
GO

-- Câu hỏi SEED_EAI_312 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_312')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 4, N'TracNghiem', N'TuVung', N'Kho', N'What is the mathematical function that quantifies the difference between model predictions and ground-truth labels during training?',
            NULL, NULL, N'''Loss Function'' (hoặc Cost Function) đo lường sai số giữa giá trị dự đoán của mô hình và nhãn thực tế để tối ưu hóa trọng số.', N'''Loss Function'' (hoặc Cost Function) đo lường sai số giữa giá trị dự đoán của mô hình và nhãn thực tế để tối ưu hóa trọng số.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Mục 3: Loss Function (Hàm mất mát) và Gradient Descent.', N'Extracted', N'01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx', NULL, 'SEED_EAI_312');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Loss Function', 1),
        (@MaCH, N'B', N'Linear Regression', 0),
        (@MaCH, N'C', N'Hash Function', 0),
        (@MaCH, N'D', N'Boolean Operator', 0);
END
GO

-- Câu hỏi SEED_EAI_313 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_313')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 4, N'DienKhuyet', N'TuVung', N'Kho', N'Fill in the blank: ''In deep learning training, one complete pass of the entire training dataset through the neural network is called an _______.''',
            NULL, NULL, N'Một chu kỳ duyệt qua toàn bộ tập dữ liệu huấn luyện được gọi là một ''Epoch''.', N'Một chu kỳ duyệt qua toàn bộ tập dữ liệu huấn luyện được gọi là một ''Epoch''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Mục 4: Epoch, Batch Size và Iterations.', N'Extracted', N'01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx', NULL, 'SEED_EAI_313');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'epoch', 1),
        (@MaCH, N'B', N'era', 0),
        (@MaCH, N'C', N'century', 0),
        (@MaCH, N'D', N'interval', 0);
END
GO

-- Câu hỏi SEED_EAI_314 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_314')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What distinguishes ''Supervised Learning'' from ''Unsupervised Learning''?',
            NULL, NULL, N'Học có giám sát (Supervised Learning) yêu cầu dữ liệu huấn luyện đã được gán nhãn sẵn (labeled data), trong khi Học không giám sát tự tìm cụm quy luật trên dữ liệu chưa gán nhãn (unlabeled data).', N'Học có giám sát (Supervised Learning) yêu cầu dữ liệu huấn luyện đã được gán nhãn sẵn (labeled data), trong khi Học không giám sát tự tìm cụm quy luật trên dữ liệu chưa gán nhãn (unlabeled data).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Mục 1: Phân loại Supervised, Unsupervised, Reinforcement Learning.', N'Extracted', N'01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx', NULL, 'SEED_EAI_314');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Supervised learning uses labeled training datasets, whereas unsupervised learning identifies patterns in unlabeled data.', 1),
        (@MaCH, N'B', N'Unsupervised learning requires humans to watch the computer constantly.', 0),
        (@MaCH, N'C', N'Supervised learning can only be programmed in Python 2.', 0),
        (@MaCH, N'D', N'There is no technical difference.', 0);
END
GO

-- Câu hỏi SEED_EAI_315 (TuVung - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_315')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 4, N'DungSai', N'TuVung', N'De', N'True or False: Regularization techniques (such as L1 Lasso, L2 Ridge, and Dropout) are specifically employed to combat and prevent overfitting in machine learning models.',
            NULL, NULL, N'Đúng. Các kỹ thuật điều chuẩn (Regularization) như L1, L2 và Dropout được áp dụng để hạn chế hiện tượng overfitting.', N'Đúng. Các kỹ thuật điều chuẩn (Regularization) như L1, L2 và Dropout được áp dụng để hạn chế hiện tượng overfitting.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Mục 5: Kỹ thuật chống Overfitting: Regularization and Dropout.', N'Extracted', N'01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx', NULL, 'SEED_EAI_315');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_316 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_316')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'TracNghiem', N'Doc', N'Kho', N'What is ''Few-Shot Prompting'' in modern Large Language Model (LLM) interaction?',
            NULL, NULL, N'''Few-shot prompting'' là kỹ thuật cung cấp một vài cặp ví dụ mẫu (input - output examples) ngay trong câu nhắc để mô hình hiểu rõ định dạng và phong cách mong muốn trước khi xử lý yêu cầu chính.', N'''Few-shot prompting'' là kỹ thuật cung cấp một vài cặp ví dụ mẫu (input - output examples) ngay trong câu nhắc để mô hình hiểu rõ định dạng và phong cách mong muốn trước khi xử lý yêu cầu chính.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Phần 2: Kỹ thuật Few-Shot Prompting vs Zero-Shot Prompting.', N'Extracted', N'02_Prompt_Engineering_Principles_for_Developers.pdf', NULL, 'SEED_EAI_316');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Sending only short prompts under five words', 0),
        (@MaCH, N'B', N'Providing a few explicit input-output demonstration examples within the prompt context before the target task', 1),
        (@MaCH, N'C', N'Reloading the browser three times before prompting', 0),
        (@MaCH, N'D', N'Training an entirely new neural network from scratch', 0);
END
GO

-- Câu hỏi SEED_EAI_317 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_317')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'TracNghiem', N'Doc', N'Kho', N'Why does ''Chain-of-Thought'' (CoT) prompting significantly improve reasoning performance on multi-step math and logic problems?',
            NULL, NULL, N'Chain-of-Thought thúc đẩy mô hình tạo ra các bước suy luận trung gian (intermediate reasoning steps) giống con người trước khi đưa ra kết luận cuối cùng.', N'Chain-of-Thought thúc đẩy mô hình tạo ra các bước suy luận trung gian (intermediate reasoning steps) giống con người trước khi đưa ra kết luận cuối cùng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Phần 3: Chain-of-Thought (CoT) Reasoning.', N'Extracted', N'02_Prompt_Engineering_Principles_for_Developers.pdf', NULL, 'SEED_EAI_317');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Because it forces the AI to break down complex reasoning into explicit intermediate steps, reducing hasty logical leaps.', 1),
        (@MaCH, N'B', N'Because it doubles the graphics card GPU clock speed.', 0),
        (@MaCH, N'C', N'Because it translates text into binary code.', 0),
        (@MaCH, N'D', N'Because it suppresses all safety filters.', 0);
END
GO

-- Câu hỏi SEED_EAI_318 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_318')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'DienKhuyet', N'TuVung', N'Kho', N'Fill in the prompt engineering term: ''Assigning a clear persona or role (e.g., "Act as a Senior Cloud Architect") is known as a _______ prompt.''',
            NULL, NULL, N'''Role'' hoặc ''System'' prompt xác định vai trò, phong cách và góc nhìn chuyên môn của AI.', N'''Role'' hoặc ''System'' prompt xác định vai trò, phong cách và góc nhìn chuyên môn của AI.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Phần 1: Role-based System Prompting.', N'Extracted', N'02_Prompt_Engineering_Principles_for_Developers.pdf', NULL, 'SEED_EAI_318');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Role', 1),
        (@MaCH, N'B', N'Joke', 0),
        (@MaCH, N'C', N'Random', 0),
        (@MaCH, N'D', N'Secret', 0);
END
GO

-- Câu hỏi SEED_EAI_319 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_319')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'TracNghiem', N'Doc', N'Kho', N'What does ''Hallucination'' refer to in generative AI and LLM outputs?',
            NULL, NULL, N'''Hallucination'' (Hiện tượng ảo giác) là khi mô hình ngôn ngữ tự bịa ra thông tin sai lệch nhưng diễn đạt với giọng điệu rất tự tin và trôi chảy như thật.', N'''Hallucination'' (Hiện tượng ảo giác) là khi mô hình ngôn ngữ tự bịa ra thông tin sai lệch nhưng diễn đạt với giọng điệu rất tự tin và trôi chảy như thật.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Phần 4: Nhận diện và giảm thiểu hiện tượng Hallucination.', N'Extracted', N'02_Prompt_Engineering_Principles_for_Developers.pdf', NULL, 'SEED_EAI_319');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'When the monitor display flickers', 0),
        (@MaCH, N'B', N'When an LLM produces plausible-sounding but factually fabricated or incorrect information presented confidently', 1),
        (@MaCH, N'C', N'When the computer runs out of power', 0),
        (@MaCH, N'D', N'When the network cable is unplugged', 0);
END
GO

-- Câu hỏi SEED_EAI_320 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_320')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 4, N'DungSai', N'Doc', N'De', N'True or False: Using precise delimiters (such as triple backticks ``` or XML tags <context>) helps an LLM distinguish between developer instructions and external user input.',
            NULL, NULL, N'Đúng. Sử dụng dấu phân cách rõ ràng (Delimiters như triple backticks hoặc XML tags) là nguyên tắc cốt lõi giúp phòng tránh lỗi Prompt Injection và tăng độ chính xác.', N'Đúng. Sử dụng dấu phân cách rõ ràng (Delimiters như triple backticks hoặc XML tags) là nguyên tắc cốt lõi giúp phòng tránh lỗi Prompt Injection và tăng độ chính xác.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Nguyên tắc phân tách ngữ cảnh bằng Delimiters.', N'Extracted', N'02_Prompt_Engineering_Principles_for_Developers.pdf', NULL, 'SEED_EAI_320');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_321 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_321')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'TracNghiem', N'Doc', N'Kho', N'What architectural breakthrough was introduced by Vaswani et al. in the landmark 2017 paper ''Attention Is All You Need''?',
            NULL, NULL, N'Bài báo ''Attention Is All You Need'' giới thiệu kiến trúc Transformer dựa hoàn toàn trên cơ chế Attention, loại bỏ hoàn toàn tính tuần tự chậm chạp của mạng RNN và LSTM.', N'Bài báo ''Attention Is All You Need'' giới thiệu kiến trúc Transformer dựa hoàn toàn trên cơ chế Attention, loại bỏ hoàn toàn tính tuần tự chậm chạp của mạng RNN và LSTM.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Tóm tắt bài báo Attention Is All You Need: The Transformer Architecture.', N'Extracted', N'03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf', NULL, 'SEED_EAI_321');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Convolutional neural networks for video games', 0),
        (@MaCH, N'B', N'The Transformer architecture, replacing recurrent and convolutional layers entirely with self-attention mechanisms', 1),
        (@MaCH, N'C', N'A new hardware processor made of gold', 0),
        (@MaCH, N'D', N'A relational database storage engine', 0);
END
GO

-- Câu hỏi SEED_EAI_322 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_322')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'TracNghiem', N'Doc', N'Kho', N'What are the three fundamental vectors calculated in the Scaled Dot-Product Attention formula?',
            NULL, NULL, N'Cơ chế Self-Attention dựa trên 3 vector: Queries (Q), Keys (K) và Values (V): Attention(Q, K, V) = softmax(QK^T / sqrt(d_k)) * V.', N'Cơ chế Self-Attention dựa trên 3 vector: Queries (Q), Keys (K) và Values (V): Attention(Q, K, V) = softmax(QK^T / sqrt(d_k)) * V.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Công thức Scaled Dot-Product Attention: Query, Key, Value vectors.', N'Extracted', N'03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf', NULL, 'SEED_EAI_322');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Queries (Q), Keys (K), and Values (V)', 1),
        (@MaCH, N'B', N'Quality, Knowledge, and Verification', 0),
        (@MaCH, N'C', N'Quantity, Kernel, and Variables', 0),
        (@MaCH, N'D', N'Question, Keyword, and Vocabulary', 0);
END
GO

-- Câu hỏi SEED_EAI_323 (TuVung - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_323')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'DienKhuyet', N'TuVung', N'Kho', N'Fill in the blank: ''Because Transformers process all tokens simultaneously without recurrence, _______ Encoding is injected to inform the model of word order.''',
            NULL, NULL, N'''Positional'' Encoding (Mã hóa vị trí) được cộng vào các vector nhúng để mô hình nhận biết thứ tự trước sau của các từ trong câu.', N'''Positional'' Encoding (Mã hóa vị trí) được cộng vào các vector nhúng để mô hình nhận biết thứ tự trước sau của các từ trong câu.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Cơ chế Positional Encoding trong Transformer.', N'Extracted', N'03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf', NULL, 'SEED_EAI_323');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Positional', 1),
        (@MaCH, N'B', N'Alphabetical', 0),
        (@MaCH, N'C', N'Random', 0),
        (@MaCH, N'D', N'Color', 0);
END
GO

-- Câu hỏi SEED_EAI_324 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_324')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'TracNghiem', N'Doc', N'Kho', N'What major computational advantage did Transformers offer over traditional Recurrent Neural Networks (RNNs)?',
            NULL, NULL, N'Ưu thế vượt trội của Transformer là khả năng song song hóa tính toán tối đa trên GPU (Massive Parallelization), giúp huấn luyện nhanh hơn gấp nhiều lần so với tính tuần tự của RNN.', N'Ưu thế vượt trội của Transformer là khả năng song song hóa tính toán tối đa trên GPU (Massive Parallelization), giúp huấn luyện nhanh hơn gấp nhiều lần so với tính tuần tự của RNN.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - So sánh hiệu năng: Khả năng tính toán song song (Parallelization).', N'Extracted', N'03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf', NULL, 'SEED_EAI_324');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'They consume zero electricity.', 0),
        (@MaCH, N'B', N'Massive parallelization across GPUs because input sequences are processed all at once rather than sequentially step-by-step.', 1),
        (@MaCH, N'C', N'They run on devices without memory.', 0),
        (@MaCH, N'D', N'They require no training data.', 0);
END
GO

-- Câu hỏi SEED_EAI_325 (Doc - Trình độ 5 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_325')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'DungSai', N'Doc', N'De', N'True or False: Modern foundational LLMs like GPT-4, Gemini, and Claude are fundamentally based on the Transformer architecture.',
            NULL, NULL, N'Đúng. Toàn bộ các mô hình ngôn ngữ lớn hiện đại (GPT, Gemini, Llama, Claude) đều phát triển từ nền tảng kiến trúc Transformer.', N'Đúng. Toàn bộ các mô hình ngôn ngữ lớn hiện đại (GPT, Gemini, Llama, Claude) đều phát triển từ nền tảng kiến trúc Transformer.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Ứng dụng của Transformer trong AI tạo sinh hiện đại.', N'Extracted', N'03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf', NULL, 'SEED_EAI_325');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_326 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_326')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'In Git version control, what does ''git commit'' do?',
            NULL, NULL, N'''git commit'' lưu lại ảnh chụp trạng thái (snapshot) các thay đổi đã được staged vào lịch sử kho mã nguồn cục bộ.', N'''git commit'' lưu lại ảnh chụp trạng thái (snapshot) các thay đổi đã được staged vào lịch sử kho mã nguồn cục bộ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Thuật ngữ quản lý phiên bản Git: Commit, Branch, Merge, Pull Request.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_326');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'It permanently deletes all files in the repository.', 0),
        (@MaCH, N'B', N'It records a snapshot of the staged changes to the local repository history with a descriptive message.', 1),
        (@MaCH, N'C', N'It uploads code directly to the live production server.', 0),
        (@MaCH, N'D', N'It reboots the computer operating system.', 0);
END
GO

-- Câu hỏi SEED_EAI_327 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_327')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What does the architectural term ''Microservices'' contrast with?',
            NULL, NULL, N'''Microservices'' (Kiến trúc vi dịch vụ phân tán) đối lập trực tiếp với ''Monolithic architecture'' (Kiến trúc khối đơn hợp nhất).', N'''Microservices'' (Kiến trúc vi dịch vụ phân tán) đối lập trực tiếp với ''Monolithic architecture'' (Kiến trúc khối đơn hợp nhất).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Kiến trúc hệ thống phần mềm: Monolith vs. Microservices.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_327');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Relational database', 0),
        (@MaCH, N'B', N'Monolithic architecture', 1),
        (@MaCH, N'C', N'Operating system', 0),
        (@MaCH, N'D', N'Agile methodology', 0);
END
GO

-- Câu hỏi SEED_EAI_328 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_328')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''To prevent SQL injection attacks, developers must always use parameterized _______.''',
            NULL, NULL, N'''parameterized queries'' (các câu truy vấn tham số hóa) là biện pháp bảo mật cốt lõi chống SQL Injection.', N'''parameterized queries'' (các câu truy vấn tham số hóa) là biện pháp bảo mật cốt lõi chống SQL Injection.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - An toàn bảo mật cơ sở dữ liệu: Parameterized queries against SQL Injection.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_328');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'queries', 1),
        (@MaCH, N'B', N'passwords', 0),
        (@MaCH, N'C', N'hard drives', 0),
        (@MaCH, N'D', N'screens', 0);
END
GO

-- Câu hỏi SEED_EAI_329 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_329')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 3, N'TracNghiem', N'Doc', N'De', N'What is a ''Pull Request'' (PR) used for in collaborative GitHub workflows?',
            NULL, NULL, N'''Pull Request'' là đề xuất trộn (merge) các commit từ một nhánh tính năng (feature branch) vào nhánh chính sau khi được đồng nghiệp review.', N'''Pull Request'' là đề xuất trộn (merge) các commit từ một nhánh tính năng (feature branch) vào nhánh chính sau khi được đồng nghiệp review.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 26 - Quy trình làm việc nhóm phần mềm: Code Review via Pull Requests.', N'Extracted', N'BaiHoc_26_DailyStandupScrum.txt', 26, 'SEED_EAI_329');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A request to pull all ethernet cables out', 0),
        (@MaCH, N'B', N'A proposed set of code changes submitted for peer review before merging into the main branch', 1),
        (@MaCH, N'C', N'An order for new office hardware', 0),
        (@MaCH, N'D', N'A formal resignation notice', 0);
END
GO

-- Câu hỏi SEED_EAI_330 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_330')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What is ''Latent Space'' in deep learning and autoencoder models?',
            NULL, NULL, N'''Không gian tiềm ẩn'' (Latent space) là không gian biểu diễn đa chiều nén chứa các đặc trưng cốt lõi rút trích được từ dữ liệu gốc.', N'''Không gian tiềm ẩn'' (Latent space) là không gian biểu diễn đa chiều nén chứa các đặc trưng cốt lõi rút trích được từ dữ liệu gốc.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Khái niệm Latent Space và Feature Representation.', N'Extracted', N'01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx', NULL, 'SEED_EAI_330');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'An empty hard drive partition', 0),
        (@MaCH, N'B', N'A compressed, lower-dimensional mathematical space where meaningful features of data are represented', 1),
        (@MaCH, N'C', N'The physical area behind a server rack', 0),
        (@MaCH, N'D', N'The RAM cache of a web browser', 0);
END
GO

-- Câu hỏi SEED_EAI_331 (TuVung - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_331')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 4, N'DungSai', N'TuVung', N'De', N'True or False: In API design, ''REST'' stands for Representational State Transfer.',
            NULL, NULL, N'Đúng. ''REST'' là viết tắt của ''Representational State Transfer'' - tiêu chuẩn thiết kế web service thông dụng nhất.', N'Đúng. ''REST'' là viết tắt của ''Representational State Transfer'' - tiêu chuẩn thiết kế web service thông dụng nhất.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Thuật ngữ API: RESTful Web Services.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_331');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_332 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_332')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'TracNghiem', N'Doc', N'Kho', N'In AI Prompt Engineering, what is ''Temperature'' as an LLM hyperparameter?',
            NULL, NULL, N'''Temperature'' điều khiển tính ngẫu nhiên và sáng tạo của câu trả lời. Temperature = 0 cho kết quả tất định, chính xác; Temperature cao (>0.8) cho kết quả phong phú, sáng tạo.', N'''Temperature'' điều khiển tính ngẫu nhiên và sáng tạo của câu trả lời. Temperature = 0 cho kết quả tất định, chính xác; Temperature cao (>0.8) cho kết quả phong phú, sáng tạo.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Tham số suy luận LLM: Temperature, Top-p, Top-k.', N'Extracted', N'02_Prompt_Engineering_Principles_for_Developers.pdf', NULL, 'SEED_EAI_332');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The physical heat of the server room measured in Celsius', 0),
        (@MaCH, N'B', N'A parameter controlling the randomness and creativity of generated token probabilities (0 = deterministic, 1 = creative)', 1),
        (@MaCH, N'C', N'The speed at which characters appear on screen', 0),
        (@MaCH, N'D', N'The length of the prompt in kilobytes', 0);
END
GO

-- Câu hỏi SEED_EAI_333 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_333')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What does ''ACID'' represent in database transaction theory?',
            NULL, NULL, N'''ACID'' là 4 thuộc tính đảm bảo giao dịch cơ sở dữ liệu tin cậy: Atomicity (Nguyên tử), Consistency (Nhất quán), Isolation (Cô lập), Durability (Bền vững).', N'''ACID'' là 4 thuộc tính đảm bảo giao dịch cơ sở dữ liệu tin cậy: Atomicity (Nguyên tử), Consistency (Nhất quán), Isolation (Cô lập), Durability (Bền vững).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Thuộc tính giao dịch Database: ACID properties.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_333');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Atomicity, Consistency, Isolation, Durability', 1),
        (@MaCH, N'B', N'Automatic, Constant, Infinite, Dynamic', 0),
        (@MaCH, N'C', N'Access, Control, Identify, Delete', 0),
        (@MaCH, N'D', N'Application, Code, Integration, Deployment', 0);
END
GO

-- Câu hỏi SEED_EAI_334 (Noi - Trình độ 3 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_334')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 3, N'DienKhuyet', N'Noi', N'De', N'Fill in the Agile term: ''At the end of each two-week cycle, the team holds a Sprint _______ to discuss what went well and what can be improved.''',
            NULL, NULL, N'''Sprint Retrospective'' là buổi họp hồi tưởng cuối Sprint để đúc rút kinh nghiệm cải tiến quy trình.', N'''Sprint Retrospective'' là buổi họp hồi tưởng cuối Sprint để đúc rút kinh nghiệm cải tiến quy trình.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 26 - Các sự kiện trong Scrum: Sprint Planning, Daily Standup, Sprint Review, Sprint Retrospective.', N'Extracted', N'BaiHoc_26_DailyStandupScrum.txt', 26, 'SEED_EAI_334');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Retrospective', 1),
        (@MaCH, N'B', N'Cancellation', 0),
        (@MaCH, N'C', N'Punishment', 0),
        (@MaCH, N'D', N'Celebration', 0);
END
GO

-- Câu hỏi SEED_EAI_335 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_335')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'In neural networks, what is the role of an ''Activation Function'' (e.g., ReLU, Sigmoid)?',
            NULL, NULL, N'Hàm kích hoạt (Activation Function) đưa tính phi tuyến (non-linearity) vào mạng nơ-ron, giúp mạng học được các quan hệ phức tạp.', N'Hàm kích hoạt (Activation Function) đưa tính phi tuyến (non-linearity) vào mạng nơ-ron, giúp mạng học được các quan hệ phức tạp.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Hàm kích hoạt trong Deep Learning: Activation Functions (ReLU, Softmax).', N'Extracted', N'01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx', NULL, 'SEED_EAI_335');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'To turn on the computer monitor', 0),
        (@MaCH, N'B', N'To introduce non-linearity into the network, enabling it to learn complex, non-linear relationships', 1),
        (@MaCH, N'C', N'To format the hard disk cleanly', 0),
        (@MaCH, N'D', N'To send emails to the administrator', 0);
END
GO

-- Câu hỏi SEED_EAI_336 (TuVung - Trình độ 3 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_336')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 3, N'TracNghiem', N'TuVung', N'De', N'Which port number is standard for encrypted web traffic using HTTPS?',
            NULL, NULL, N'Cổng tiêu chuẩn của giao thức bảo mật HTTPS là Port 443. (Cổng HTTP không mã hóa là Port 80).', N'Cổng tiêu chuẩn của giao thức bảo mật HTTPS là Port 443. (Cổng HTTP không mã hóa là Port 80).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Mạng máy tính và giao thức: HTTP vs. HTTPS Port 443.', N'Applied', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_336');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'80', 0),
        (@MaCH, N'B', N'443', 1),
        (@MaCH, N'C', N'21', 0),
        (@MaCH, N'D', N'22', 0);
END
GO

-- Câu hỏi SEED_EAI_337 (Doc - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_337')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'DienKhuyet', N'Doc', N'Kho', N'Fill in the blank: ''Fine-tuning with RLHF (Reinforcement Learning from Human _______) aligns models with human ethical values.''',
            NULL, NULL, N'''RLHF'' là viết tắt của ''Reinforcement Learning from Human Feedback'' (Học tăng cường từ phản hồi của con người).', N'''RLHF'' là viết tắt của ''Reinforcement Learning from Human Feedback'' (Học tăng cường từ phản hồi của con người).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Kỹ thuật căn chỉnh mô hình AI: RLHF (Reinforcement Learning from Human Feedback).', N'Extracted', N'02_Prompt_Engineering_Principles_for_Developers.pdf', NULL, 'SEED_EAI_337');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Feedback', 1),
        (@MaCH, N'B', N'Finance', 0),
        (@MaCH, N'C', N'Food', 0),
        (@MaCH, N'D', N'Force', 0);
END
GO

-- Câu hỏi SEED_EAI_338 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_338')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'Doc', N'Kho', N'What does ''Latency'' measure in distributed client-server systems?',
            NULL, NULL, N'''Latency'' (Độ trễ) đo lường khoảng thời gian cần thiết để một gói tin dữ liệu truyền từ điểm gửi đến điểm nhận và phản hồi lại.', N'''Latency'' (Độ trễ) đo lường khoảng thời gian cần thiết để một gói tin dữ liệu truyền từ điểm gửi đến điểm nhận và phản hồi lại.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Hiệu năng hệ thống: Latency and Throughput.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_338');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The weight of the copper networking cables', 0),
        (@MaCH, N'B', N'The time delay experienced by a data packet traveling from its source to its destination and back', 1),
        (@MaCH, N'C', N'The brightness of monitor screens', 0),
        (@MaCH, N'D', N'The number of lines of source code', 0);
END
GO

-- Câu hỏi SEED_EAI_339 (Noi - Trình độ 3 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_339')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 3, N'DungSai', N'Noi', N'De', N'True or False: Detailed technical arguments or debugging sessions should be taken ''offline'' (discussed after the standup) to respect everyone''s time.',
            NULL, NULL, N'Đúng. Trong Daily Standup, các cuộc tranh luận sâu về kỹ thuật phải được chuyển ra sau buổi họp (''take it offline'') để không làm mất thời gian cả nhóm.', N'Đúng. Trong Daily Standup, các cuộc tranh luận sâu về kỹ thuật phải được chuyển ra sau buổi họp (''take it offline'') để không làm mất thời gian cả nhóm.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 26 - Quy tắc điều phối Standup: ''Take technical debates offline''.', N'Extracted', N'BaiHoc_26_DailyStandupScrum.txt', 26, 'SEED_EAI_339');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_340 (TuVung - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_340')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'TracNghiem', N'TuVung', N'Kho', N'In NLP tokenization, what is a ''Token''?',
            NULL, NULL, N'''Token'' là đơn vị cơ bản nhỏ nhất (từ, phần của từ hoặc ký tự) mà mô hình ngôn ngữ lớn xử lý.', N'''Token'' là đơn vị cơ bản nhỏ nhất (từ, phần của từ hoặc ký tự) mà mô hình ngôn ngữ lớn xử lý.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Quá trình Tokenization và Embedding.', N'Extracted', N'03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf', NULL, 'SEED_EAI_340');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A physical metal coin used in arcades', 0),
        (@MaCH, N'B', N'The basic building block or sub-word segment that a language model uses to parse and generate text', 1),
        (@MaCH, N'C', N'A computer antivirus license key', 0),
        (@MaCH, N'D', N'A database table index', 0);
END
GO

-- Câu hỏi SEED_EAI_341 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_341')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What is the primary role of a ''Load Balancer'' in high-traffic web architectures?',
            NULL, NULL, N'''Load Balancer'' (Bộ cân bằng tải) phân phối lưu lượng truy cập mạng đồng đều giữa nhiều máy chủ hậu cần để tránh quá tải.', N'''Load Balancer'' (Bộ cân bằng tải) phân phối lưu lượng truy cập mạng đồng đều giữa nhiều máy chủ hậu cần để tránh quá tải.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Kiến trúc chịu tải cao: Load Balancing and Horizontal Scaling.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_341');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'To weigh physical server computers on a scale', 0),
        (@MaCH, N'B', N'To distribute incoming network traffic across multiple backend servers to ensure high availability and responsiveness', 1),
        (@MaCH, N'C', N'To turn off unused monitors at night', 0),
        (@MaCH, N'D', N'To calculate employee payroll', 0);
END
GO

-- Câu hỏi SEED_EAI_342 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_342')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''Transfer learning allows a model pre-trained on a massive dataset to be _______ on a smaller, domain-specific dataset.''',
            NULL, NULL, N'''fine-tuned'' (tinh chỉnh) mô hình cho bài toán chuyên biệt cụ thể.', N'''fine-tuned'' (tinh chỉnh) mô hình cho bài toán chuyên biệt cụ thể.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Khái niệm Transfer Learning và Fine-tuning.', N'Extracted', N'01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx', NULL, 'SEED_EAI_342');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'fine-tuned', 1),
        (@MaCH, N'B', N'deleted', 0),
        (@MaCH, N'C', N'formatted', 0),
        (@MaCH, N'D', N'rejected', 0);
END
GO

-- Câu hỏi SEED_EAI_343 (Noi - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_343')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 3, N'TracNghiem', N'Noi', N'TrungBinh', N'Which sentence is best to offer assistance to a teammate facing a blocker during standup?',
            NULL, NULL, N'''I can sync with you after this meeting to help troubleshoot that issue.'' thể hiện tinh thần hợp tác chuyên nghiệp.', N'''I can sync with you after this meeting to help troubleshoot that issue.'' thể hiện tinh thần hợp tác chuyên nghiệp.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 26 - Mẫu câu tương tác hợp tác trong Daily Standup.', N'Extracted', N'BaiHoc_26_DailyStandupScrum.txt', 26, 'SEED_EAI_343');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'You should have known how to code that already.', 0),
        (@MaCH, N'B', N'I have experience with that service; let''s sync right after this standup to troubleshoot together.', 1),
        (@MaCH, N'C', N'Don''t bother me with your bugs.', 0),
        (@MaCH, N'D', N'Leave the team if you are blocked.', 0);
END
GO

-- Câu hỏi SEED_EAI_344 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_344')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'TracNghiem', N'Doc', N'Kho', N'In the Transformer architecture, what is ''Multi-Head Attention''?',
            NULL, NULL, N'''Multi-Head Attention'' cho phép mô hình đồng thời chú ý đến thông tin từ các không gian biểu diễn khác nhau ở các vị trí khác nhau.', N'''Multi-Head Attention'' cho phép mô hình đồng thời chú ý đến thông tin từ các không gian biểu diễn khác nhau ở các vị trí khác nhau.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Cơ chế Multi-Head Attention.', N'Extracted', N'03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf', NULL, 'SEED_EAI_344');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Having multiple human engineers monitor the code', 0),
        (@MaCH, N'B', N'Splitting attention into multiple projection heads so the model can simultaneously attend to information from different representation subspaces', 1),
        (@MaCH, N'C', N'Connecting multiple monitors to one PC', 0),
        (@MaCH, N'D', N'Printing multiple copies of a paper', 0);
END
GO

-- Câu hỏi SEED_EAI_345 (TuVung - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_345')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'DungSai', N'TuVung', N'De', N'True or False: In relational databases, a ''Primary Key'' column allows NULL values.',
            NULL, NULL, N'Sai. Khóa chính (Primary Key) bắt buộc phải là giá trị duy nhất (Unique) và tuyệt đối KHÔNG được chứa giá trị NULL (NOT NULL).', N'Sai. Khóa chính (Primary Key) bắt buộc phải là giá trị duy nhất (Unique) và tuyệt đối KHÔNG được chứa giá trị NULL (NOT NULL).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Quy tắc khóa chính trong Database: Unique and NOT NULL.', N'Extracted', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_345');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_346 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_346')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What is ''Data Augmentation'' in computer vision and machine learning?',
            NULL, NULL, N'''Data Augmentation'' (Tăng cường dữ liệu) là việc tạo ra các mẫu huấn luyện mới bằng cách xoay, lật, cắt hoặc đổi màu các ảnh hiện có để mô hình học tốt hơn.', N'''Data Augmentation'' (Tăng cường dữ liệu) là việc tạo ra các mẫu huấn luyện mới bằng cách xoay, lật, cắt hoặc đổi màu các ảnh hiện có để mô hình học tốt hơn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Kỹ thuật tiền xử lý dữ liệu: Data Augmentation.', N'Extracted', N'01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx', NULL, 'SEED_EAI_346');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Buying more server computers', 0),
        (@MaCH, N'B', N'Artificially increasing the diversity of training data by applying transformations like rotations, flips, and cropping', 1),
        (@MaCH, N'C', N'Encrypting database backups with passwords', 0),
        (@MaCH, N'D', N'Deleting duplicate records from Excel', 0);
END
GO

-- Câu hỏi SEED_EAI_347 (TuVung - Trình độ 3 - Dạng DienKhuyet - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_347')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 3, N'DienKhuyet', N'TuVung', N'De', N'Fill in the blank: ''A software _______ fixes unexpected bugs or security vulnerabilities in deployed systems.''',
            NULL, NULL, N'''patch'' (bản vá lỗi phần mềm).', N'''patch'' (bản vá lỗi phần mềm).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 25 - Vận hành và bảo trì: Security Patches.', N'Applied', N'BaiHoc_25_SoftwareEngineeringVocab.txt', 25, 'SEED_EAI_347');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'patch', 1),
        (@MaCH, N'B', N'blanket', 0),
        (@MaCH, N'C', N'curtain', 0),
        (@MaCH, N'D', N'carpet', 0);
END
GO

-- Câu hỏi SEED_EAI_348 (Doc - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_348')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 3, N'TracNghiem', N'Doc', N'De', N'What does the Scrum metric ''Velocity'' track across development sprints?',
            NULL, NULL, N'''Velocity'' đo lường số lượng đơn vị công việc (Story Points) mà nhóm hoàn thành trung bình trong mỗi Sprint.', N'''Velocity'' đo lường số lượng đơn vị công việc (Story Points) mà nhóm hoàn thành trung bình trong mỗi Sprint.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 26 - Các chỉ số đo lường Sprint: Velocity and Burndown Chart.', N'Extracted', N'BaiHoc_26_DailyStandupScrum.txt', 26, 'SEED_EAI_348');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The typing speed of developers in words per minute', 0),
        (@MaCH, N'B', N'The amount of work (in Story Points) a development team completes successfully per sprint cycle', 1),
        (@MaCH, N'C', N'The internet download speed of the office', 0),
        (@MaCH, N'D', N'The CPU clock speed of laptops', 0);
END
GO

-- Câu hỏi SEED_EAI_349 (Doc - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_349')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'TracNghiem', N'Doc', N'Kho', N'What vulnerability occurs when an attacker crafts malicious input to override an LLM''s system instructions?',
            NULL, NULL, N'''Prompt Injection'' là lỗ hổng bảo mật khi kẻ tấn công chèn lệnh độc hại vào văn bản đầu vào để ép mô hình bỏ qua hướng dẫn hệ thống.', N'''Prompt Injection'' là lỗ hổng bảo mật khi kẻ tấn công chèn lệnh độc hại vào văn bản đầu vào để ép mô hình bỏ qua hướng dẫn hệ thống.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - An ninh bảo mật Prompt: Prompt Injection attacks and defenses.', N'Extracted', N'02_Prompt_Engineering_Principles_for_Developers.pdf', NULL, 'SEED_EAI_349');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Hardware overheating', 0),
        (@MaCH, N'B', N'Prompt Injection attack', 1),
        (@MaCH, N'C', N'Memory leakage', 0),
        (@MaCH, N'D', N'Disk fragmentation', 0);
END
GO

-- Câu hỏi SEED_EAI_350 (TuVung - Trình độ 5 - Dạng TraLoiNgan - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_350')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (16, 5, N'TraLoiNgan', N'TuVung', N'Kho', N'What does ''NLP'' stand for in modern Artificial Intelligence?',
            NULL, NULL, N'''NLP'' là viết tắt của ''Natural Language Processing'' (Xử lý ngôn ngữ tự nhiên).', N'''NLP'' là viết tắt của ''Natural Language Processing'' (Xử lý ngôn ngữ tự nhiên).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Định nghĩa các phân ngành AI: NLP (Natural Language Processing).', N'Extracted', N'01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx', NULL, 'SEED_EAI_350');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Natural Language Processing', 1),
        (@MaCH, N'B', N'Network Line Protocol', 0),
        (@MaCH, N'C', N'New Logic Programming', 0),
        (@MaCH, N'D', N'Node Level Path', 0);
END
GO

PRINT N'Đã hoàn thành nạp Batch 07 (SEED_EAI_301 -> SEED_EAI_350) thành công!';
GO
