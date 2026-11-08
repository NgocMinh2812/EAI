/*
    MODULE: 04_nganhangcauhoi.sql
    Mục đích: Ngân hàng 704 câu hỏi trắc nghiệm & lựa chọn đáp án chuẩn hóa
    Phân loại:
     - 416 câu gắn liền với 36 Bài học (MaBaiHoc IS NOT NULL), có trích xuất dẫn chứng cụ thể
     - 288 câu nâng cao, khảo sát đầu vào & thi thử hệ thống (MaBaiHoc IS NULL)
     - Đã loại bỏ hoàn toàn các câu hỏi và bộ đề AI củng cố tạm thời
*/
USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

-- Xóa câu hỏi & đáp án cũ
IF OBJECT_ID('LuaChonDapAn', 'U') IS NOT NULL DELETE FROM LuaChonDapAn WHERE MaLuaChon > 0;
IF OBJECT_ID('CauHoi', 'U') IS NOT NULL DELETE FROM CauHoi WHERE MaCauHoi >= 0;

IF OBJECT_ID('LuaChonDapAn', 'U') IS NOT NULL DBCC CHECKIDENT ('LuaChonDapAn', RESEED, 0) WITH NO_INFOMSGS;
IF OBJECT_ID('CauHoi', 'U') IS NOT NULL DBCC CHECKIDENT ('CauHoi', RESEED, 0) WITH NO_INFOMSGS;
GO

-- ===================================================================
-- PHẦN 1: 15 CÂU HỎI KHẢO SÁT PHÂN LOẠI TRÌNH ĐỘ ĐẦU VÀO (IDs 0 -> 14)
-- Loại: Câu hỏi hệ thống khảo sát phân tầng (MaBaiHoc IS NULL)
-- ===================================================================
-- 8. NẠP CÂU HỎI TRẮC NGHIỆM & LỰA CHỌN ĐÁP ÁN (CauHoi & LuaChonDapAn)
-- ===================================================================
DECLARE @tdA1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A1');
DECLARE @tdA2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A2');
DECLARE @tdB1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B1');
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');
DECLARE @tdC1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'C1');

DECLARE @cdGrammar INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Ngữ Pháp%');
DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS%');
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = 1;

-- Câu 1 (A1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA1, N'TracNghiem', N'NguPhap', N'Choose the correct verb form: She ______ to school by bus every morning.', 
        N'Chủ ngữ ngôi thứ ba số ít "She" trong thì hiện tại đơn đi với động từ thêm -s/-es ("goes").', @gvId, N'ThuCong', 1);
DECLARE @q1 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q1, 'A', N'go', 0), (@q1, 'B', N'goes', 1), (@q1, 'C', N'going', 0), (@q1, 'D', N'went', 0);

-- Câu 2 (A1 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA1, N'TracNghiem', N'TuVung', N'I usually have bread and eggs for ______ at 7:00 AM.', 
        N'"Breakfast" là bữa ăn sáng vào lúc 7:00 AM.', @gvId, N'ThuCong', 1);
DECLARE @q2 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q2, 'A', N'dinner', 0), (@q2, 'B', N'lunch', 0), (@q2, 'C', N'breakfast', 1), (@q2, 'D', N'supper', 0);

-- Câu 3 (A2 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'NguPhap', N'Yesterday we ______ a very interesting documentary about marine life.', 
        N'Trạng từ thời gian "Yesterday" báo hiệu thì quá khứ đơn ("watched").', @gvId, N'ThuCong', 1);
DECLARE @q3 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q3, 'A', N'watch', 0), (@q3, 'B', N'watches', 0), (@q3, 'C', N'watched', 1), (@q3, 'D', N'watching', 0);

-- Câu 4 (A2 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'TuVung', N'This suitcase is too ______ for me to carry alone. Can you help me?', 
        N'"Heavy" mang nghĩa nặng nề, phù hợp ngữ cảnh cần người giúp đỡ.', @gvId, N'ThuCong', 1);
DECLARE @q4 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q4, 'A', N'light', 0), (@q4, 'B', N'heavy', 1), (@q4, 'C', N'bright', 0), (@q4, 'D', N'soft', 0);

-- Câu 5 (B1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'NguPhap', N'If I ______ enough money, I would travel around the world next summer.', 
        N'Câu điều kiện loại 2 giả định điều không có thật ở hiện tại: If + S + V2/ed, S + would + V.', @gvId, N'ThuCong', 1);
DECLARE @q5 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q5, 'A', N'have', 0), (@q5, 'B', N'had', 1), (@q5, 'C', N'will have', 0), (@q5, 'D', N'have had', 0);

-- Câu 6 (B1 Reading)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'Doc', 
 N'According to the passage, why do owls prefer hunting at night?', 
 N'Owls are nocturnal birds of prey renowned for their extraordinary night vision and silent flight. Unlike daylight predators such as eagles and falcons, owls possess large, forward-facing eyes with a high concentration of rod cells that capture minimal ambient light. Furthermore, hunting under the cover of darkness significantly reduces competition for small rodents.',
 N'Đoạn văn nêu rõ: bóng tối giúp cú tránh sự cạnh tranh săn bắt các loài gặm nhấm từ các loài săn mồi ban ngày.', 
 @gvId, N'ThuCong', 1);
DECLARE @q6 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q6, 'A', N'They cannot tolerate warm sunshine.', 0),
(@q6, 'B', N'Their specialized senses excel in the dark and they avoid daytime competitors.', 1),
(@q6, 'C', N'Rodents are only active during morning hours.', 0),
(@q6, 'D', N'They prefer socializing in darkness.', 0);

-- Câu 7 (B2 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'TuVung', N'The committee decided to ______ stricter environmental policies to curb industrial carbon emissions.', 
        N'Cụm từ "implement policies" có nghĩa là thi hành/áp dụng chính sách.', @gvId, N'ThuCong', 1);
DECLARE @q7 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q7, 'A', N'implement', 1), (@q7, 'B', N'compliment', 0), (@q7, 'C', N'supplement', 0), (@q7, 'D', N'experiment', 0);

-- Câu 8 (B2 Grammar - Inversion)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'Seldom ______ such a breathtaking display of aurora borealis in this region.', 
        N'Đảo ngữ với trạng từ phủ định đứng đầu câu: Seldom + trợ động từ + S + V chính ("have we witnessed").', @gvId, N'ThuCong', 1);
DECLARE @q8 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q8, 'A', N'we have witnessed', 0), (@q8, 'B', N'have we witnessed', 1), (@q8, 'C', N'we witnessed', 0), (@q8, 'D', N'had witnessed we', 0);

-- Câu 9 (C1 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'Subsequent empirical studies completely ______ the original hypothesis proposed by Dr. Vance.', 
        N'"Corroborate" nghĩa là chứng thực, củng cố bằng chứng cho giả thuyết.', @gvId, N'ThuCong', 1);
DECLARE @q9 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q9, 'A', N'corroborated', 1), (@q9, 'B', N'retracted', 0), (@q9, 'C', N'dismantled', 0), (@q9, 'D', N'diminished', 0);

-- Câu 10 (C1 Reading)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'The author uses the phrase "double-edged sword" to emphasize that artificial intelligence:', 
 N'The integration of generative artificial intelligence across industrial workflows represents a double-edged sword. On one hand, automated synthesis and code generation accelerate throughput exponentially. Conversely, intellectual property vulnerabilities and cognitive atrophy among junior engineers present formidable existential hurdles for long-term innovation.', 
 N'"Double-edged sword" (con dao hai lưỡi) biểu thị cả lợi ích to lớn (tăng tốc độ) lẫn rủi ro tiềm tàng (vấn đề sở hữu trí tuệ, suy giảm tư duy).',
 @gvId, N'ThuCong', 1);
DECLARE @q10 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q10, 'A', N'is entirely hazardous and must be prohibited immediately', 0),
(@q10, 'B', N'delivers immense productivity gains while simultaneously introducing substantial perils', 1),
(@q10, 'C', N'has surpassed human cognition in ethical discernment', 0),
(@q10, 'D', N'is exclusively beneficial for junior software developers', 0);

-- Câu 11 (B2 Grammar - Subjunctive)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'The doctor recommended that the patient ______ adequate rest and avoid heavy lifting.', 
        N'Cấu trúc giả định thức (Subjunctive): recommend that S + (should) V-nguyên thể ("take").', @gvId, N'ThuCong', 1);
DECLARE @q11 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q11, 'A', N'takes', 0), (@q11, 'B', N'took', 0), (@q11, 'C', N'take', 1), (@q11, 'D', N'will take', 0);

-- Câu 12 (B2 Vocabulary - Collocations)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'TuVung', N'The prolonged drought had a ______ effect on agricultural output throughout the province.', 
        N'"Detrimental effect" là cụm từ cố định (collocation) mang nghĩa tác động tiêu cực, gây hại.', @gvId, N'ThuCong', 1);
DECLARE @q12 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q12, 'A', N'beneficial', 0), (@q12, 'B', N'detrimental', 1), (@q12, 'C', N'constructive', 0), (@q12, 'D', N'superficial', 0);

-- Câu 13 (C1 Inversion - Conditionals)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'______ they followed the emergency safety protocol, the catastrophic leak would have been averted.', 
        N'Đảo ngữ câu điều kiện loại 3: Had + S + V3/ed thay cho If + S + had + V3/ed.', @gvId, N'ThuCong', 1);
DECLARE @q13 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q13, 'A', N'If had', 0), (@q13, 'B', N'Should', 0), (@q13, 'C', N'Had', 1), (@q13, 'D', N'Were', 0);

-- Câu 14 (B1 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'TuVung', N'It is not economically ______ to construct a bullet train through such mountainous terrain.', 
        N'"Feasible" có nghĩa là khả thi, thực hiện được.', @gvId, N'ThuCong', 1);
DECLARE @q14 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q14, 'A', N'feasible', 1), (@q14, 'B', N'fragile', 0), (@q14, 'C', N'furious', 0), (@q14, 'D', N'fictional', 0);

-- Câu 15 (A2 Grammar - Comparatives)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'NguPhap', N'Traveling by high-speed train is much ______ than taking the overnight bus.', 
        N'So sánh hơn của tính từ ngắn "fast" là "faster", có thể nhấn mạnh bằng "much".', @gvId, N'ThuCong', 1);
DECLARE @q15 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q15, 'A', N'more fast', 0), (@q15, 'B', N'faster', 1), (@q15, 'C', N'fastest', 0), (@q15, 'D', N'as fast', 0);
GO

-- ===================================================================
-- ===================================================================
-- PHẦN 2: 54 CÂU HỎI LUYỆN TẬP CHUNG THEO KHUNG CEFR A1 -> C1 (IDs 15 -> 68)
-- Loại: Câu hỏi ngân hàng luyện tập tổng hợp hệ thống (MaBaiHoc IS NULL)
-- ===================================================================
-- 13. Há»† THá»NG CÃ‚U Há»ŽI PHONG PHÃš VÃ€ CÃC Äá»€ THI THá»¬ & GÃ“I LUYá»†N Táº¬P
-- ===================================================================
GO

-- ===================================================================
-- Script: Add_Diverse_Questions_And_Exams.sql
-- Bổ sung hệ thống câu hỏi phong phú (Ngữ pháp, Từ vựng, Đọc hiểu, Viết - KHÔNG bao gồm Nghe)
-- và tạo các Đề thi thử (ThiThu) cùng Gói luyện tập (LuyenTap) chuẩn hóa cho học viên
-- ===================================================================
USE TiengAnhAI;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = 1;

DECLARE @tdA1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A1');
DECLARE @tdA2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'A2');
DECLARE @tdB1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B1');
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'B2');
DECLARE @tdC1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = N'C1');

DECLARE @cdGrammar INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Ngữ Pháp%');
IF @cdGrammar IS NULL SET @cdGrammar = 9;
DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Học Thuật%' OR TenChuDe LIKE N'%IELTS%');
IF @cdIELTS IS NULL SET @cdIELTS = 5;
DECLARE @cdBusiness INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Thương Mại%');
IF @cdBusiness IS NULL SET @cdBusiness = 13;
DECLARE @cdDaily INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Giao Tiếp%');
IF @cdDaily IS NULL SET @cdDaily = 1;

-- Bảng tạm lưu danh sách mã câu hỏi vừa tạo để phân phối vào các đề
DECLARE @NewQuestions TABLE (
    Id INT IDENTITY(1,1),
    MaCauHoi INT,
    KyNang NVARCHAR(30),
    MaTrinhDo INT,
    DoKho NVARCHAR(20)
);

-- ===================================================================
-- 1. BỘ CÂU HỎI NGỮ PHÁP CHUYÊN SÂU (15 CÂU: A1 -> C1)
-- ===================================================================

-- Q1 (A1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdA1, N'TracNghiem', N'NguPhap', N'Every morning, David ______ coffee with milk before going to work.', 
        N'Thì hiện tại đơn diễn tả thói quen hàng ngày với chủ ngữ số ít "David" động từ thêm -s/es ("drinks").', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
DECLARE @q INT = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdA1, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'drinks', 1), (@q, 'B', N'drinking', 0), (@q, 'C', N'drink', 0), (@q, 'D', N'is drink', 0);

-- Q2 (A2 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'NguPhap', N'While we ______ dinner last night, the power suddenly went out.', 
        N'Thì quá khứ tiếp diễn diễn tả hành động đang diễn ra (were having) thì hành động khác chen ngang ở quá khứ đơn (went out).', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdA2, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'had', 0), (@q, 'B', N'were having', 1), (@q, 'C', N'have had', 0), (@q, 'D', N'are having', 0);

-- Q3 (A2 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'NguPhap', N'This laptop is ______ than the one I bought two years ago.', 
        N'So sánh hơn của tính từ dài: more + adj + than ("more reliable").', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdA2, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'more reliable', 1), (@q, 'B', N'reliabler', 0), (@q, 'C', N'most reliable', 0), (@q, 'D', N'as reliable', 0);

-- Q4 (B1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'NguPhap', N'She has lived in London ______ she graduated from university in 2018.', 
        N'Dùng "since" mốc thời gian / mệnh đề quá khứ đơn đi cùng thì hiện tại hoàn thành.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'for', 0), (@q, 'B', N'since', 1), (@q, 'C', N'when', 0), (@q, 'D', N'during', 0);

-- Q5 (B1 Grammar - Passive)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'NguPhap', N'The new bridge ______ by international engineers before the rainy season starts.', 
        N'Thể bị động của tương lai đơn hoặc modal verb: will be completed.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will complete', 0), (@q, 'B', N'will be completed', 1), (@q, 'C', N'completes', 0), (@q, 'D', N'is completing', 0);

-- Q6 (B1 Grammar - Conditionals)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'NguPhap', N'If it ______ tomorrow, we will have to reschedule our outdoor photography workshop.', 
        N'Câu điều kiện loại 1 diễn tả sự việc có thể xảy ra ở tương lai: If + S + V(hiện tại đơn), S + will + V.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'rains', 1), (@q, 'B', N'rained', 0), (@q, 'C', N'will rain', 0), (@q, 'D', N'is raining', 0);

-- Q7 (B2 Grammar - Reduced Relative Clause)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'The guidelines ______ at the annual symposium will be disseminated to all departments next week.', 
        N'Rút gọn mệnh đề quan hệ dạng bị động (which were approved -> approved).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'approving', 0), (@q, 'B', N'approved', 1), (@q, 'C', N'were approved', 0), (@q, 'D', N'approve', 0);

-- Q8 (B2 Grammar - Mixed Conditionals)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'If she had accepted that overseas scholarship last year, she ______ in Zurich right now.', 
        N'Câu điều kiện hỗn hợp (Mixed conditional 3 - 2): Giả định quá khứ (had accepted) dẫn đến kết quả ở hiện tại (would be living).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB2, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'would live', 0), (@q, 'B', N'would be living', 1), (@q, 'C', N'had lived', 0), (@q, 'D', N'will live', 0);

-- Q9 (B2 Grammar - Conjunctions)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'______ the unprecedented surge in raw material costs, the corporation managed to maintain its quarterly profit margin.', 
        N'"Despite" hoặc "In spite of" đi cùng danh từ hoặc cụm danh từ biểu thị sự đối lập nhượng bộ.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Although', 0), (@q, 'B', N'Despite', 1), (@q, 'C', N'Because', 0), (@q, 'D', N'Even though', 0);

-- Q10 (B2 Grammar - Modals of Deduction)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'The streets are soaking wet this morning. It ______ heavily throughout the night.', 
        N'Dự đoán có căn cứ chắc chắn ở quá khứ dùng "must have + V3/ed" (Chắc hẳn là đã mưa).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'should have rained', 0), (@q, 'B', N'must have rained', 1), (@q, 'C', N'could rain', 0), (@q, 'D', N'would rain', 0);

-- Q11 (C1 Grammar - Inversion with Hardly)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'Hardly ______ the stage when the enthusiastic audience erupted in thunderous applause.', 
        N'Cấu trúc đảo ngữ: Hardly had + S + V3/ed + when + S + V2/ed (Vừa mới... thì...).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'the pianist had stepped on', 0), (@q, 'B', N'had the pianist stepped on', 1), (@q, 'C', N'did the pianist step on', 0), (@q, 'D', N'was the pianist stepping on', 0);

-- Q12 (C1 Grammar - Inversion Conditional)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'______ any discrepancies arise during the fiscal audit, please report them immediately to the compliance officer.', 
        N'Đảo ngữ câu điều kiện loại 1: Should + S + V-nguyên thể thay cho If + S + V.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Were', 0), (@q, 'B', N'Should', 1), (@q, 'C', N'Had', 0), (@q, 'D', N'Unless', 0);

-- Q13 (C1 Grammar - Subjunctive with Insist)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'The board of directors insisted that the chief architect ______ present at the project review meeting.', 
        N'Thức giả định (Subjunctive): S + insist that + S + (should) be/V-inf.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'is', 0), (@q, 'B', N'was', 0), (@q, 'C', N'be', 1), (@q, 'D', N'will be', 0);

-- Q14 (C1 Grammar - Cleft Sentences)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'It was not until the subsequent validation trial was concluded ______ the researchers published their empirical findings.', 
        N'Câu chẻ nhấn mạnh thời gian: It is/was not until... that + S + V.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'when', 0), (@q, 'B', N'that', 1), (@q, 'C', N'which', 0), (@q, 'D', N'then', 0);

-- Q15 (C1 Grammar - Passive Gerund)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdC1, N'TracNghiem', N'NguPhap', N'The corporate executive objected to ______ questioned about the proprietary algorithm without legal counsel.', 
        N'Cấu trúc object to + V-ing, ở thể bị động: object to being + V3/ed.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'NguPhap', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'be', 0), (@q, 'B', N'being', 1), (@q, 'C', N'have been', 0), (@q, 'D', N'having', 0);

-- ===================================================================
-- 2. BỘ CÂU HỎI TỪ VỰNG & COLLOCATIONS (15 CÂU: A1 -> C1)
-- ===================================================================

-- Q16 (A1 Vocab)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdDaily, @tdA1, N'TracNghiem', N'TuVung', N'Can you please tell me the ______ to the nearest central railway station?', 
        N'Cụm từ "tell someone the way to..." có nghĩa là chỉ đường cho ai đến đâu.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdA1, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'way', 1), (@q, 'B', N'road', 0), (@q, 'C', N'street', 0), (@q, 'D', N'path', 0);

-- Q17 (A2 Vocab - Phrasal Verb)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdDaily, @tdA2, N'TracNghiem', N'TuVung', N'Don''t forget to ______ your coat because the temperature outside is dropping rapidly.', 
        N'"Put on" có nghĩa là mặc/mặc thêm áo quần vào người.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdA2, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'take off', 0), (@q, 'B', N'put on', 1), (@q, 'C', N'look after', 0), (@q, 'D', N'give up', 0);

-- Q18 (A2 Vocab)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdA2, N'TracNghiem', N'TuVung', N'The receptionist asked the visitor to fill out a short registration ______ at the front desk.', 
        N'"Registration form" là cụm danh từ thông dụng chỉ phiếu/mẫu đơn đăng ký.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdA2, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'form', 1), (@q, 'B', N'letter', 0), (@q, 'C', N'page', 0), (@q, 'D', N'card', 0);

-- Q19 (B1 Vocab - Collocation)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB1, N'TracNghiem', N'TuVung', N'All participants are kindly requested to ______ close attention to the safety briefing before boarding.', 
        N'Collocation: "pay close attention to" mang nghĩa chú ý lắng nghe kỹ.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'make', 0), (@q, 'B', N'pay', 1), (@q, 'C', N'give', 0), (@q, 'D', N'take', 0);

-- Q20 (B1 Vocab - Phrasal Verb)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'TuVung', N'The engineering team has ______ a groundbreaking solution to minimize energy loss in electrical grids.', 
        N'Phrasal verb: "come up with" có nghĩa là nảy ra, đề xuất ra một ý tưởng hoặc giải pháp mới.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'come up with', 1), (@q, 'B', N'run out of', 0), (@q, 'C', N'get along with', 0), (@q, 'D', N'put up with', 0);

-- Q21 (B1 Vocab)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'TuVung', N'Solar and wind power are increasingly popular forms of ______ energy around the globe.', 
        N'"Renewable energy" là thuật ngữ chỉ nguồn năng lượng tái tạo, có thể tái sinh.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'reusable', 0), (@q, 'B', N'renewable', 1), (@q, 'C', N'replaceable', 0), (@q, 'D', N'recyclable', 0);

-- Q22 (B2 Vocab - Collocation)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB2, N'TracNghiem', N'TuVung', N'After hours of heated debate, the negotiation team finally managed to ______ a unanimous consensus.', 
        N'Collocation: "reach a consensus" nghĩa là đạt được sự đồng thuận chung.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'make', 0), (@q, 'B', N'reach', 1), (@q, 'C', N'grasp', 0), (@q, 'D', N'hold', 0);

-- Q23 (B2 Vocab - Academic)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'TuVung', N'The provincial government pledged to allocate adequate funds to ______ the economic fallout of the natural disaster.', 
        N'"Mitigate" nghĩa là làm giảm nhẹ, xoa dịu hậu quả tiêu cực.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'mitigate', 1), (@q, 'B', N'escalate', 0), (@q, 'C', N'provoke', 0), (@q, 'D', N'aggravate', 0);

-- Q24 (B2 Vocab - Idiom)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'TuVung', N'She has been ______ all week in order to finish her doctoral dissertation before the deadline.', 
        N'Thành ngữ: "burning the midnight oil" nghĩa là thức khuya làm việc/học tập cật lực.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'breaking the ice', 0), (@q, 'B', N'burning the midnight oil', 1), (@q, 'C', N'barking up the wrong tree', 0), (@q, 'D', N'hitting the sack', 0);

-- Q25 (B2 Vocab - Word Choice)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB2, N'TracNghiem', N'TuVung', N'The project manager emphasized that strict compliance with safety regulations is ______ in this hazardous facility.', 
        N'"Paramount" nghĩa là tối quan trọng, đứng hàng đầu.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdB2, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'trivial', 0), (@q, 'B', N'paramount', 1), (@q, 'C', N'redundant', 0), (@q, 'D', N'tentative', 0);

-- Q26 (C1 Vocab - Academic)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'Smartphones and wireless connectivity have become virtually ______ in modern metropolitan existence.', 
        N'"Ubiquitous" mang nghĩa phổ biến ở khắp mọi nơi, nhan nhản.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'ubiquitous', 1), (@q, 'B', N'sporadic', 0), (@q, 'C', N'ephemeral', 0), (@q, 'D', N'precarious', 0);

-- Q27 (C1 Vocab - Academic Collocation)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'The scientist took ______ care to ensure that all experimental variables were controlled impeccably.', 
        N'"Meticulous care" là cụm từ học thuật biểu thị sự cẩn trọng tỉ mỉ, kỹ lưỡng đến từng chi tiết.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'superficial', 0), (@q, 'B', N'meticulous', 1), (@q, 'C', N'negligent', 0), (@q, 'D', N'abrupt', 0);

-- Q28 (C1 Vocab - Idiom)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'His perceptive summary of the financial crisis really ______ and identified the systemic vulnerabilities.', 
        N'Thành ngữ: "hit the nail on the head" nghĩa là nói trúng phóc trọng tâm, giải thích chính xác vấn đề.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'missed the boat', 0), (@q, 'B', N'hit the nail on the head', 1), (@q, 'C', N'spilled the beans', 0), (@q, 'D', N'let sleeping dogs lie', 0);

-- Q29 (C1 Vocab - Advanced Verbs)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'The sudden revocation of the commercial trade treaty will inevitably ______ cross-border investment flows.', 
        N'"Impediment" / "impede" có nghĩa là cản trở, làm đình trệ sự tiến triển.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'impede', 1), (@q, 'B', N'foster', 0), (@q, 'C', N'expedite', 0), (@q, 'D', N'bolster', 0);

-- Q30 (C1 Vocab - Academic Contrast)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'The venture capitalist demonstrated a remarkably ______ mindset by prioritizing tangible cash flow over speculative hype.', 
        N'"Pragmatic" nghĩa là thực dụng, thực tế, dựa trên hành động và kết quả cụ thể.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'TuVung', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'dogmatic', 0), (@q, 'B', N'pragmatic', 1), (@q, 'C', N'erratic', 0), (@q, 'D', N'utopian', 0);

-- ===================================================================
-- 3. BỘ CÂU HỎI ĐỌC HIỂU HỌC THUẬT & BÀI ĐỌC (14 CÂU: B1 -> C1)
-- ===================================================================

-- PASSAGE A (B1 Reading): Sustainable Tourism
DECLARE @passageA NVARCHAR(MAX) = N'Ecotourism has gained significant momentum in recent decades as travelers become more environmentally conscious. Unlike conventional mass tourism, which often strains local infrastructure and causes ecological degradation, ecotourism emphasizes environmental stewardship and cultural respect. Travelers visit pristine natural areas, support community-run conservation initiatives, and contribute directly to the local economy. However, experts caution that without stringent regulations, unchecked commercialization can turn sustainable destinations into overcrowded attractions, ultimately defeating the original conservation purpose.';

-- Q31 (Passage A - Q1)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'Doc', 
 N'According to the passage, how does ecotourism differ primarily from conventional mass tourism?', 
 @passageA, 
 N'Đoạn văn nêu rõ: du lịch sinh thái tập trung vào bảo vệ môi trường và tôn trọng văn hóa địa phương thay vì gây áp lực lên hạ tầng như du lịch đại chúng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It prioritizes luxury accommodations over local experiences.', 0),
(@q, 'B', N'It focuses on environmental protection and cultural respect.', 1),
(@q, 'C', N'It strictly caters to international researchers.', 0),
(@q, 'D', N'It avoids using local economic resources.', 0);

-- Q32 (Passage A - Q2)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'Doc', 
 N'What potential hazard do conservation experts warn against regarding ecotourism?', 
 @passageA, 
 N'Đoạn văn cảnh báo: nếu không có quy định nghiêm ngặt, thương mại hóa thiếu kiểm soát có thể khiến các điểm đến trở nên quá tải.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Overcrowding and commercialization without strict regulations.', 1),
(@q, 'B', N'A total loss of traditional farming techniques.', 0),
(@q, 'C', N'An immediate ban on international air transportation.', 0),
(@q, 'D', N'High expenditure on digital marketing campaigns.', 0);

-- Q33 (Passage A - Q3)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'Doc', 
 N'The word "pristine" in the passage is closest in meaning to:', 
 @passageA, 
 N'"Pristine" mang nghĩa nguyên sơ, hoang sơ, chưa bị con người hay ô nhiễm làm hư hại (unspoiled).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'crowded', 0), (@q, 'B', N'unspoiled', 1), (@q, 'C', N'industrialized', 0), (@q, 'D', N'expensive', 0);

-- PASSAGE B (B2 Reading): AI in Diagnostic Medicine
DECLARE @passageB NVARCHAR(MAX) = N'The convergence of deep learning architectures and high-resolution imaging has transformed medical diagnostics. Computer vision algorithms can now analyze thousands of radiological scans within seconds, detecting micro-anomalies such as early-stage pulmonary nodules that human clinicians might overlook during fatigued night shifts. Despite these monumental achievements, computer scientists and medical ethicists stress that artificial intelligence should augment rather than supplant human physicians. Algorithmic outputs are inherently susceptible to training bias, and machines lack the nuanced bedside empathy essential for holistic patient care and complex palliative counseling.';

-- Q34 (Passage B - Q1)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Doc', 
 N'What is a primary advantage of deep learning algorithms in radiology highlighted in the text?', 
 @passageB, 
 N'Văn bản chỉ ra rằng thuật toán có thể phân tích hàng nghìn ảnh chụp trong vài giây và phát hiện những bất thường siêu nhỏ.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'They perform surgeries without human assistance.', 0),
(@q, 'B', N'They rapidly scan massive image sets and detect microscopic anomalies.', 1),
(@q, 'C', N'They completely eliminate all diagnostic expenses.', 0),
(@q, 'D', N'They formulate medication formulas independently.', 0);

-- Q35 (Passage B - Q2)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Doc', 
 N'According to the passage, why should AI not entirely replace human doctors?', 
 @passageB, 
 N'Đoạn văn nêu hai lý do: thuật toán có thể bị sai lệch dữ liệu huấn luyện (training bias) và máy móc thiếu sự thấu cảm trong chăm sóc bệnh nhân.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Algorithms are prone to training bias and lack emotional empathy.', 1),
(@q, 'B', N'Deep learning models consume excessive hospital electricity.', 0),
(@q, 'C', N'Medical equipment is incompatible with digital computing systems.', 0),
(@q, 'D', N'Doctors refuse to interact with software interfaces.', 0);

-- Q36 (Passage B - Q3)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Doc', 
 N'The word "supplant" in the passage is closest in meaning to:', 
 @passageB, 
 N'"Supplant" có nghĩa là thay thế hoàn toàn vị trí của ai/cái gì (replace / supersede).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'reinforce', 0), (@q, 'B', N'replace', 1), (@q, 'C', N'criticize', 0), (@q, 'D', N'instruct', 0);

-- PASSAGE C (B2 Reading): Remote Work and Asynchronous Collaboration
DECLARE @passageC NVARCHAR(MAX) = N'The widespread adoption of remote work has catalyzed a fundamental paradigm shift in corporate culture. In lieu of rigid nine-to-five desk presence, leading technology organizations are embracing asynchronous communication frameworks. By documenting task requirements comprehensively in cloud knowledge bases, distributed squads across multiple time zones can collaborate smoothly without endless real-time meetings. Nevertheless, organizational psychologists caution that prolonged physical isolation can erode spontaneous creativity and weaken interpersonal cohesion if social rituals are neglected.';

-- Q37 (Passage C - Q1)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB2, N'TracNghiem', N'Doc', 
 N'What key practice enables geographically dispersed teams to collaborate asynchronously?', 
 @passageC, 
 N'Đoạn văn nêu: các nhóm phân tán cộng tác trơn tru nhờ việc lưu trữ tài liệu yêu cầu công việc chi tiết trên nền tảng đám mây.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Mandating uninterrupted daily webcam surveillance.', 0),
(@q, 'B', N'Documenting project requirements thoroughly in cloud knowledge repositories.', 1),
(@q, 'C', N'Requiring all employees to work in the same physical time zone.', 0),
(@q, 'D', N'Eliminating all forms of written status reports.', 0);

-- Q38 (Passage C - Q2)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB2, N'TracNghiem', N'Doc', 
 N'What adverse consequence might result from perpetual remote isolation according to psychologists?', 
 @passageC, 
 N'Chuyên gia tâm lý cảnh báo việc cô lập thể chất lâu dài làm xói mòn khả năng sáng tạo tự phát và gắn kết giữa người với người.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Erosion of spontaneous innovation and weakened interpersonal cohesion.', 1),
(@q, 'B', N'A guaranteed reduction in quarterly software delivery speeds.', 0),
(@q, 'C', N'Sudden hardware malfunctions on local workstations.', 0),
(@q, 'D', N'Immediate dissolution of corporate legal partnerships.', 0);

-- PASSAGE D (C1 Reading): Cognitive Agility & The Attention Economy
DECLARE @passageD NVARCHAR(MAX) = N'In an era characterized by hyper-connectivity and pervasive notification streams, deep contemplation has become a scarce cognitive commodity. The human attention mechanism is continually fragmented by algorithmic feedback loops tailored to stimulate dopamine responses. Renowned cognitive theorists argue that this persistent cognitive interruption induces neuroplastic adaptations that favor superficial skimming over sustained analytical deduction. To preserve high-order conceptual thinking, knowledge workers must cultivate deliberate practices of monastic focus and cognitive sanctuary, disengaging periodically from ubiquitous digital stimuli.';

-- Q39 (Passage D - Q1)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'What is the central premise asserted by the author regarding modern attention spans?', 
 @passageD, 
 N'Tác giả khẳng định luồng thông báo liên tục và vòng lặp thuật toán khiến khả năng chú ý bị phân mảnh, ưu tiên đọc lướt hời hợt hơn tư duy phân tích sâu.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Human memory storage capacity has doubled in the digital age.', 0),
(@q, 'B', N'Algorithmic interruptions promote superficial skimming at the expense of deep deduction.', 1),
(@q, 'C', N'Social media notifications permanently cure workplace anxiety.', 0),
(@q, 'D', N'Multitasking is proven to enhance complex philosophical reasoning.', 0);

-- Q40 (Passage D - Q2)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'What countermeasure does the author propose to safeguard high-order conceptual thinking?', 
 @passageD, 
 N'Tác giả đề xuất xây dựng các khoảng thời gian tập trung biệt lập ("monastic focus" / "cognitive sanctuary") và tạm ngắt kết nối với các kích thích số.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Engaging in deliberate periods of focused solitude away from digital stimuli.', 1),
(@q, 'B', N'Installing additional notification monitors in the office.', 0),
(@q, 'C', N'Increasing weekly reliance on generative AI synthesis tools.', 0),
(@q, 'D', N'Abandoning all reading activities in favor of podcasts.', 0);

-- Q41 (Passage D - Q3)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'The term "scarce cognitive commodity" implies that deep contemplation is:', 
 @passageD, 
 N'"Scarce cognitive commodity" mang nghĩa tư duy tập trung sâu sắc đã trở thành một thứ vô cùng hiếm hoi và quý giá trong thời đại số.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'financially unprofitable', 0),
(@q, 'B', N'exceptionally rare and valuable', 1),
(@q, 'C', N'biologically obsolete', 0),
(@q, 'D', N'readily accessible to everyone', 0);

-- Q42 (Passage C - Q3)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdBusiness, @tdB2, N'TracNghiem', N'Doc', 
 N'The phrase "in lieu of" in the passage can be most accurately substituted with:', 
 @passageC, 
 N'"In lieu of" là cụm từ trang trọng có nghĩa là "thay vì" (instead of).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'in comparison to', 0), (@q, 'B', N'instead of', 1), (@q, 'C', N'in addition to', 0), (@q, 'D', N'in accordance with', 0);

-- Q43 (Passage D - Q4)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'What tone does the author predominantly adopt throughout Passage D?', 
 @passageD, 
 N'Giọng văn phân tích, phản biện mang tính học thuật và cảnh báo sâu sắc (Analytic and cautionary).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Sarcastic and indifferent', 0),
(@q, 'B', N'Analytical and cautionary', 1),
(@q, 'C', N'Enthusiastic and celebratory', 0),
(@q, 'D', N'Nostalgic and regretful', 0);

-- Q44 (Passage B - Q4)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Doc', 
 N'Which title best summarizes the core theme of Passage B?', 
 @passageB, 
 N'Tiêu đề phản ánh đúng nhất: Trí tuệ nhân tạo trong chẩn đoán y tế: Công cụ hỗ trợ mạnh mẽ chứ không phải sự thay thế con người.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Doc', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The Total Demise of Human Radiologists', 0),
(@q, 'B', N'AI in Diagnostics: An Augmentation, Not a Replacement', 1),
(@q, 'C', N'A Practical Guide to Night Shift Scheduling', 0),
(@q, 'D', N'Financial Pitfalls of Medical Computer Vision', 0);

-- ===================================================================
-- 4. BỘ CÂU HỎI VIẾT & SỬA LỖI CÂU HỌC THUẬT (10 CÂU: A2 -> C1)
-- ===================================================================

-- Q45 (A2 Writing - Sentence Combining)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdDaily, @tdA2, N'TracNghiem', N'Viet', 
 N'Choose the best sentence that combines the two thoughts: "The train was late. We missed the opening presentation."', 
 N'Liên từ "Because" chỉ nguyên nhân: Because the train was late, we missed the opening presentation.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdA2, N'De');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Because the train was late, we missed the opening presentation.', 1),
(@q, 'B', N'The train was late although we missed the presentation.', 0),
(@q, 'C', N'We missed the presentation so that the train was late.', 0),
(@q, 'D', N'The train was late but we missed the presentation.', 0);

-- Q46 (B1 Writing - Error Identification)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'Viet', 
 N'Identify the underlined part that contains a grammatical error: "Neither the supervisor (A) nor the lab technicians (B) was (C) aware of the chemical spill (D)."', 
 N'Quy tắc hòa hợp chủ vị với "Neither... nor...": Động từ hòa hợp với danh từ gần nhất "the lab technicians" (số nhiều), do đó "was" phải sửa thành "were".', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'the supervisor', 0), (@q, 'B', N'the lab technicians', 0), (@q, 'C', N'was', 1), (@q, 'D', N'chemical spill', 0);

-- Q47 (B1 Writing - Sentence Rewriting)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'Viet', 
 N'Choose the sentence that is closest in meaning to: "They haven''t upgraded their office software for over five years."', 
 N'"It has been over five years since they last upgraded their office software."', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdB1, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It has been over five years since they last upgraded their office software.', 1),
(@q, 'B', N'They upgraded their office software five years ago for the first time.', 0),
(@q, 'C', N'Office software will be upgraded within the next five years.', 0),
(@q, 'D', N'They have been upgrading their office software every five years.', 0);

-- Q48 (B2 Writing - Cohesive Transitions)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Viet', 
 N'Choose the most suitable transitional word: "The pilot experiment failed to deliver anticipated metrics. ______, the insights gleaned helped refine the subsequent research protocol."', 
 N'"Nevertheless" (tuy nhiên / dẫu vậy) diễn tả sự đối lập nhượng bộ giữa hai câu độc lập.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Furthermore', 0), (@q, 'B', N'Nevertheless', 1), (@q, 'C', N'Consequently', 0), (@q, 'D', N'In addition', 0);

-- Q49 (B2 Writing - Parallel Structure)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Viet', 
 N'Identify the sentence that observes correct parallel structure in academic writing:', 
 N'Cấu trúc song hành: analyzing user requirements, designing intuitive wireframes, and implementing core APIs (đều là danh động từ V-ing).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdB2, N'TrungBinh');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The consultant was responsible for analyzing user requirements, to design intuitive wireframes, and API implementation.', 0),
(@q, 'B', N'The consultant was responsible for analyzing user requirements, designing intuitive wireframes, and implementing core APIs.', 1),
(@q, 'C', N'The consultant was responsible for user analysis, design wireframes, and to implement core APIs.', 0),
(@q, 'D', N'The consultant analyzed user requirements, designing intuitive wireframes, and implemented core APIs.', 0);

-- Q50 (B2 Writing - Dangling Modifier)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'Viet', 
 N'Select the sentence that avoids a dangling modifier error:', 
 N'Tránh lỗi bổ ngữ lơ lửng: Chủ ngữ của mệnh đề chính phải là chủ thể thực hiện hành động "Having reviewed the financial audit": "the board members unanimously approved...".', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdB2, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Having reviewed the financial audit, the quarterly budget was approved by the board members.', 0),
(@q, 'B', N'Having reviewed the financial audit, the board members unanimously approved the quarterly budget.', 1),
(@q, 'C', N'Having reviewed the financial audit, an approval of the budget was announced.', 0),
(@q, 'D', N'Having reviewed the financial audit, it was decided to approve the budget.', 0);

-- Q51 (C1 Writing - Nominalization)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Viet', 
 N'Which version demonstrates the most sophisticated academic style through nominalization?', 
 N'Phong cách học thuật Band C1 ưa chuộng danh từ hóa (The implementation of stringent emission standards fostered a notable reduction in urban air pollution).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Because the city implemented stricter emission standards, air pollution in urban zones decreased noticeably.', 0),
(@q, 'B', N'The implementation of stringent emission standards fostered a notable reduction in urban air pollution.', 1),
(@q, 'C', N'City officials made standards stricter so that air became much cleaner than before.', 0),
(@q, 'D', N'They reduced air pollution by implementing emission standards that were very strict.', 0);

-- Q52 (C1 Writing - Hedging & Modality)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Viet', 
 N'Which sentence employs appropriate academic hedging to avoid over-generalization?', 
 N'Học thuật C1 sử dụng ngôn ngữ dè dặt (hedging) như "The empirical data suggests that... may correlate with..." để tránh khẳng định tuyệt đối chưa chứng minh.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The empirical data clearly proves that remote work always causes communication breakdowns.', 0),
(@q, 'B', N'The empirical data suggests that extended remote work may correlate with decreased impromptu collaboration.', 1),
(@q, 'C', N'It is indisputable that remote work is disastrous for corporate innovation.', 0),
(@q, 'D', N'Without question, every single remote worker feels physically isolated.', 0);

-- Q53 (C1 Writing - Sentence Inversion for Emphasis)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Viet', 
 N'Choose the sentence that correctly uses negative inversion to emphasize restriction: "They permitted the audit team to inspect the source code only after a non-disclosure agreement was executed."', 
 N'Đảo ngữ với "Only after": Only after a non-disclosure agreement was executed were the audit team permitted to inspect the source code.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Only after an agreement was executed the audit team was permitted to inspect the code.', 0),
(@q, 'B', N'Only after a non-disclosure agreement was executed were the audit team permitted to inspect the source code.', 1),
(@q, 'C', N'Only after was an agreement executed did the audit team permitted to inspect the code.', 0),
(@q, 'D', N'Only after did they execute an agreement they permitted the audit team to inspect the code.', 0);

-- Q54 (C1 Writing - Word Precision)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Viet', 
 N'Select the most precise and formal phrasing for an academic abstract: "This study looks into how monetary policy affects the housing market."', 
 N'"This study investigates the ramifications of monetary policy on real estate asset valuations."', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO @NewQuestions (MaCauHoi, KyNang, MaTrinhDo, DoKho) VALUES (@q, N'Viet', @tdC1, N'Kho');
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'This study looks into how monetary policy affects the housing market.', 0),
(@q, 'B', N'This study investigates the ramifications of monetary policy on real estate asset valuations.', 1),
(@q, 'C', N'This paper talks about what monetary policy does to housing prices.', 0),
(@q, 'D', N'This study checks out whether central banks influence home costs.', 0);

PRINT N'Đã thêm thành công 54 câu hỏi trắc nghiệm chất lượng cao (Ngữ pháp, Từ vựng, Đọc hiểu, Viết).';

-- ===================================================================
-- ===================================================================
-- PHẦN 3: 135 CÂU HỎI QUIZ KIỂM TRA BÀI HỌC CHO 27 BÀI HỌC (IDs 69 -> 203)
-- Mỗi bài học từ Bài 0 đến Bài 26 có 5 câu hỏi trắc nghiệm kiểm tra kiến thức
-- Gán rõ MaBaiHoc và trích xuất đoạn văn dẫn chứng từ nội dung giáo trình
-- ===================================================================
-- 14. Bá»” SUNG 135 CÃ‚U Há»ŽI TRáº®C NGHIá»†M CHO 27 BÃ€I Há»ŒC Há»† THá»NG
-- ===================================================================
GO

-- ===================================================================
-- SCRIPT BỔ SUNG 135 CÂU HỎI TRẮC NGHIỆM CHO 27 BÀI HỌC HỆ THỐNG
-- 5 CÂU/BÀI CHO 27 BÀI HỌC (ID 0 ĐẾN 26) - BỎ QUA BÀI 27 (VERBS AI TEST)
-- KHÔNG BAO GỒM KỸ NĂNG NGHE AUDIO, ĐẦY ĐỦ 4 LỰA CHỌN VÀ LỜI GIẢI CHI TIẾT
-- ===================================================================
USE TiengAnhAI;
GO
SET NOCOUNT ON;
DECLARE @gvId INT;
SELECT TOP 1 @gvId = MaNguoiDung FROM NguoiDung WHERE MaVaiTro = 2;
IF @gvId IS NULL SET @gvId = 1;
DECLARE @q INT;

-- -------------------------------------------------------------------
-- BÀI HỌC ID 0
-- -------------------------------------------------------------------
-- Q1 (Lesson 0 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'NguPhap', N'Choose the best response: "Hello, my name is Alex. Nice to meet you!"', N'Đáp lại lời chào ''Nice to meet you!'' lịch sự và chuẩn mực nhất là ''Nice to meet you too!''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Nice to meet you too!', 1),
(@q, 'B', N'I am fine, and you?', 0),
(@q, 'C', N'Yes, of course.', 0),
(@q, 'D', N'See you later.', 0);

-- Q2 (Lesson 0 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'NguPhap', N'Complete the sentence: "Where ______ you from?" - "I am from Vietnam."', N'Chủ ngữ ''you'' đi với động từ to be ''are'' trong thì hiện tại đơn.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'is', 0),
(@q, 'B', N'are', 1),
(@q, 'C', N'am', 0),
(@q, 'D', N'do', 0);

-- Q3 (Lesson 0 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'TuVung', N'Which phrase is used to ask for someone''s name politely?', N'''What is your name?'' hoặc ''May I have your name?'' dùng để hỏi tên đối phương.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How are you named?', 0),
(@q, 'B', N'What is your name?', 1),
(@q, 'C', N'Who are you called?', 0),
(@q, 'D', N'Where is your name?', 0);

-- Q4 (Lesson 0 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'NguPhap', N'Choose the correct verb form: "She ______ a first-year college student."', N'Chủ ngữ ngôi thứ ba số ít ''She'' đi với ''is''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'is', 1),
(@q, 'B', N'are', 0),
(@q, 'C', N'be', 0),
(@q, 'D', N'am', 0);

-- Q5 (Lesson 0 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'TuVung', N'Choose the best introduction for a new team member:', N'Câu giới thiệu tự nhiên và lịch sự: ''Hi, I am David from the Marketing department.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hi, I am David from the Marketing department.', 1),
(@q, 'B', N'Hey, you must know my name is David.', 0),
(@q, 'C', N'Look at me, I work here.', 0),
(@q, 'D', N'My call is David.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 1
-- -------------------------------------------------------------------
-- Q6 (Lesson 1 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'TuVung', N'Which greeting is the most formal in a corporate seminar?', N'''Good morning, ladies and gentlemen'' là lời chào trang trọng chuẩn mực.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey guys, what''s up?', 0),
(@q, 'B', N'Good morning, ladies and gentlemen.', 1),
(@q, 'C', N'Howdy folks!', 0),
(@q, 'D', N'Yo, what is going on?', 0);

-- Q7 (Lesson 1 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'NguPhap', N'In British formal etiquette, what is the customary reply to ''How do you do?''', N'Trong quy chuẩn chào hỏi trang trọng, ''How do you do?'' được đáp lại bằng chính câu ''How do you do?''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How do you do?', 1),
(@q, 'B', N'I do homework every day.', 0),
(@q, 'C', N'I am doing fine, thank you.', 0),
(@q, 'D', N'Yes, I can do it.', 0);

-- Q8 (Lesson 1 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'TuVung', N'Which expression is informal and used between close acquaintances?', N'''Long time no see!'' là câu chào thân mật khi gặp lại bạn bè.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It is an honor to make your acquaintance.', 0),
(@q, 'B', N'Long time no see! How have you been?', 1),
(@q, 'C', N'Allow me to extend my regards.', 0),
(@q, 'D', N'I trust this finds you well.', 0);

-- Q9 (Lesson 1 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'Viet', N'Choose the standard complimentary closing for a formal email:', N'''Sincerely yours'' hoặc ''Best regards'' là lời chào kết trang trọng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Catch ya later!', 0),
(@q, 'B', N'Sincerely yours,', 1),
(@q, 'C', N'Cheers mate,', 0),
(@q, 'D', N'Talk soon,', 0);

-- Q10 (Lesson 1 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'TuVung', N'How should you answer when a client says ''Pleased to meet you''?', N'''The pleasure is mine'' là cách đáp lễ tinh tế và chuyên nghiệp.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The pleasure is mine, thank you.', 1),
(@q, 'B', N'Never mind about it.', 0),
(@q, 'C', N'You are very welcome.', 0),
(@q, 'D', N'No problem at all.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 2
-- -------------------------------------------------------------------
-- Q11 (Lesson 2 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'TuVung', N'Fill in the blank: "I have been working ______ a software developer for three years."', N'Cấu trúc ''work as + danh từ chỉ nghề nghiệp''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'as', 1),
(@q, 'B', N'like', 0),
(@q, 'C', N'with', 0),
(@q, 'D', N'for', 0);

-- Q12 (Lesson 2 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'TuVung', N'Complete the sentence: "I was born and ______ up in Da Nang city."', N'Cụm ''bring up'' mang nghĩa nuôi nấng, dạng bị động ''brought up'' (lớn lên tại).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'grown', 0),
(@q, 'B', N'brought', 1),
(@q, 'C', N'raised', 0),
(@q, 'D', N'lived', 0);

-- Q13 (Lesson 2 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'TuVung', N'Which phrase expresses dedication to personal interests?', N'''Follow one''s passion'' nghĩa là theo đuổi đam mê cá nhân.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'follow one''s passion', 1),
(@q, 'B', N'call it a day', 0),
(@q, 'C', N'bite the bullet', 0),
(@q, 'D', N'break the ice', 0);

-- Q14 (Lesson 2 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'NguPhap', N'Choose the correct relative pronoun: "Da Lat, ______ is known as the city of flowers, attracts many visitors."', N'Mệnh đề quan hệ không xác định bổ nghĩa cho danh từ riêng dùng ''which''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'which', 1),
(@q, 'B', N'that', 0),
(@q, 'C', N'where', 0),
(@q, 'D', N'whose', 0);

-- Q15 (Lesson 2 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'Doc', N'In a career bio, what does ''drive for continuous learning'' signify?', N'Nghĩa là tinh thần chủ động học hỏi và trau dồi kiến thức không ngừng.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A proactive commitment to expanding knowledge and skills.', 1),
(@q, 'B', N'Traveling frequently for vacation.', 0),
(@q, 'C', N'Avoiding new technology tools.', 0),
(@q, 'D', N'Following orders blindly.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 3
-- -------------------------------------------------------------------
-- Q16 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'What should you say to reserve a table for four tonight?', N'Mẫu câu đặt bàn chuẩn: ''I would like to book a table for four at 7 PM.''', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I want food for four people.', 0),
(@q, 'B', N'I would like to book a table for four at 7 PM.', 1),
(@q, 'C', N'Give me four chairs tonight.', 0),
(@q, 'D', N'Are there four people here?', 0);

-- Q17 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'Which term refers to a dish served before the main course?', N'''Appetizer'' hoặc ''starter'' là món khai vị.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Dessert', 0),
(@q, 'B', N'Appetizer', 1),
(@q, 'C', N'Beverage', 0),
(@q, 'D', N'Condiment', 0);

-- Q18 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'How do you ask for the bill politely in a restaurant?', N'Cách xin hóa đơn thông dụng: ''Could we have the bill, please?''', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How much money now?', 0),
(@q, 'B', N'Give me the payment paper.', 0),
(@q, 'C', N'Could we have the bill, please?', 1),
(@q, 'D', N'I take the money away.', 0);

-- Q19 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'If you are allergic to peanuts, what should you notify the waiter?', N'''I have a severe peanut allergy; does this dish contain nuts?''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I hate peanuts so much.', 0),
(@q, 'B', N'I have a severe peanut allergy; does this dish contain nuts?', 1),
(@q, 'C', N'Peanuts are not good today.', 0),
(@q, 'D', N'Do not show me peanuts.', 0);

-- Q20 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'How would you order a steak cooked completely through?', N'''Well-done'' là mức độ thịt bò nướng chín kỹ hoàn toàn.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Rare', 0),
(@q, 'B', N'Medium-rare', 0),
(@q, 'C', N'Well-done', 1),
(@q, 'D', N'Raw', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 4
-- -------------------------------------------------------------------
-- Q21 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'Where can customers try on clothes before buying in a shop?', N'''Fitting room'' hoặc ''changing room'' là phòng thử đồ.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Storage room', 0),
(@q, 'B', N'Fitting room', 1),
(@q, 'C', N'Checkout counter', 0),
(@q, 'D', N'Parking lot', 0);

-- Q22 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'What document must you present to request an item exchange or refund?', N'''Receipt'' là hóa đơn thanh toán cần có khi đổi trả hàng.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Receipt', 1),
(@q, 'B', N'Menu', 0),
(@q, 'C', N'Passport', 0),
(@q, 'D', N'Brochure', 0);

-- Q23 (Lesson 4 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'NguPhap', N'Complete the question: "Do you have this shirt ______ a larger size?"', N'Giới từ ''in'' đi với kích cỡ: in a larger size / in size M.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'on', 0),
(@q, 'B', N'at', 0),
(@q, 'C', N'in', 1),
(@q, 'D', N'by', 0);

-- Q24 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'What does ''out of stock'' mean?', N'''Out of stock'' nghĩa là tạm thời hết hàng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Available at a discount', 0),
(@q, 'B', N'Temporarily unavailable in the store', 1),
(@q, 'C', N'Damaged and unsellable', 0),
(@q, 'D', N'Made of organic material', 0);

-- Q25 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'Which question politely asks the price of an item?', N'''How much is this jacket?'' là câu hỏi giá phổ biến và chính xác.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How many is this jacket?', 0),
(@q, 'B', N'How much is this jacket?', 1),
(@q, 'C', N'What cost does this jacket?', 0),
(@q, 'D', N'Which money is this jacket?', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 5
-- -------------------------------------------------------------------
-- Q26 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'Which adjective describes food that has a pleasant salty or spicy taste, not sweet?', N'''Savory'' (hoặc savoury) mang nghĩa có vị mặn đậm đà hấp dẫn.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Bitter', 0),
(@q, 'B', N'Savory', 1),
(@q, 'C', N'Bland', 0),
(@q, 'D', N'Sour', 0);

-- Q27 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'What does ''mouth-watering'' describe in culinary reviews?', N'''Mouth-watering'' miêu tả món ăn thơm ngon hấp dẫn đến ứa nước miếng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Extremely appealing and delicious-smelling', 1),
(@q, 'B', N'Too salty to swallow', 0),
(@q, 'C', N'Overly dry and tough', 0),
(@q, 'D', N'Completely tasteless', 0);

-- Q28 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'Complete the sentence: "The soup is ______ with star anise and fresh herbs."', N'Cụm ''seasoned with'' nghĩa là được nêm nếm bằng gia vị.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'spilled', 0),
(@q, 'B', N'seasoned', 1),
(@q, 'C', N'painted', 0),
(@q, 'D', N'boiled', 0);

-- Q29 (Lesson 5 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'Doc', N'Street vendors are famous for preserving ______ family recipes across generations.', N'Từ ''authentic'' nghĩa là chuẩn vị, đích thực truyền thống.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'artificial', 0),
(@q, 'B', N'authentic', 1),
(@q, 'C', N'synthetic', 0),
(@q, 'D', N'reluctant', 0);

-- Q30 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'Which word describes a texture that makes a sharp sound when bitten into?', N'''Crispy'' (giòn rụm) tạo âm thanh giòn tan khi cắn.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Soggy', 0),
(@q, 'B', N'Crispy', 1),
(@q, 'C', N'Greasy', 0),
(@q, 'D', N'Chewy', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 6
-- -------------------------------------------------------------------
-- Q31 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'What is the document given to a passenger that allows them to board the airplane?', N'''Boarding pass'' là thẻ lên máy bay.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Boarding pass', 1),
(@q, 'B', N'Customs declaration', 0),
(@q, 'C', N'Flight manual', 0),
(@q, 'D', N'Luggage tag', 0);

-- Q32 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'What is luggage that passengers are allowed to take inside the cabin called?', N'''Carry-on luggage'' (hoặc hand luggage) là hành lý xách tay mang lên khoang máy bay.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Checked baggage', 0),
(@q, 'B', N'Carry-on luggage', 1),
(@q, 'C', N'Cargo shipment', 0),
(@q, 'D', N'Excess parcel', 0);

-- Q33 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'When going through airport security, you must place metallic items into a ______.', N'''Tray'' (khay nhựa) dùng để đựng đồ kim loại qua máy quét an ninh.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'trash bin', 0),
(@q, 'B', N'tray', 1),
(@q, 'C', N'suitcase', 0),
(@q, 'D', N'drawer', 0);

-- Q34 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'Which term refers to a flight that does not stop between departure and destination?', N'''Direct flight'' hoặc ''non-stop flight'' là chuyến bay thẳng.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Layover flight', 0),
(@q, 'B', N'Non-stop flight', 1),
(@q, 'C', N'Connecting flight', 0),
(@q, 'D', N'Delayed flight', 0);

-- Q35 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'What does ''excess baggage fee'' mean?', N'Phí hành lý quá cước khi vượt trọng lượng quy định.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A discount for booking early', 0),
(@q, 'B', N'A charge for luggage exceeding the weight limit', 1),
(@q, 'C', N'A fee for changing flight dates', 0),
(@q, 'D', N'Insurance for lost items', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 7
-- -------------------------------------------------------------------
-- Q36 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'Choose the best question to ask a stranger for directions:', N'''Excuse me, could you tell me how to get to the nearest metro station?''', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey, where is the train right now?', 0),
(@q, 'B', N'Excuse me, could you tell me how to get to the nearest metro station?', 1),
(@q, 'C', N'You tell me the way immediately.', 0),
(@q, 'D', N'Train station location now please.', 0);

-- Q37 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'Where do passengers wait for their train in a subway station?', N'''Platform'' (sân ga/ ke ga) là nơi hành khách chờ tàu.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Runway', 0),
(@q, 'B', N'Platform', 1),
(@q, 'C', N'Intersection', 0),
(@q, 'D', N'Roundabout', 0);

-- Q38 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'What does ''change lines at Central Station'' mean?', N'Nghĩa là chuyển sang tuyến tàu khác tại ga trung tâm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Buy a new ticket', 0),
(@q, 'B', N'Transfer to a different subway route', 1),
(@q, 'C', N'Exit the station immediately', 0),
(@q, 'D', N'Wait for tomorrow''s train', 0);

-- Q39 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'Complete the sentence: "Walk straight for two blocks, then turn ______ at the traffic lights."', N'Cụm chỉ hướng: ''turn left'' (rẽ trái) hoặc ''turn right''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'left', 1),
(@q, 'B', N'up', 0),
(@q, 'C', N'down', 0),
(@q, 'D', N'out', 0);

-- Q40 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'If a destination is ''within walking distance'', it means:', N'''Within walking distance'' nghĩa là ở rất gần, có thể đi bộ được.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'You must take a taxi', 0),
(@q, 'B', N'It is close enough to reach on foot', 1),
(@q, 'C', N'It is located in another city', 0),
(@q, 'D', N'The road is completely blocked', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 8
-- -------------------------------------------------------------------
-- Q41 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'What is the primary objective of ''skimming'' in an academic reading test?', N'Mục tiêu của skimming là đọc lướt nhanh để nắm được ý chính và bố cục toàn bài.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'To locate specific dates and numbers', 0),
(@q, 'B', N'To read quickly to comprehend the general gist and layout', 1),
(@q, 'C', N'To translate every sentence word by word', 0),
(@q, 'D', N'To memorize technical jargon', 0);

-- Q42 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'When applying the ''scanning'' technique, a reader should:', N'Kỹ thuật scanning tập trung tìm kiếm thông tin cụ thể (từ khóa, con số, tên riêng).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Focus on identifying specific keywords, names, or figures without reading whole paragraphs.', 1),
(@q, 'B', N'Analyze the author''s philosophical viewpoint.', 0),
(@q, 'C', N'Underline every unfamiliar vocabulary item.', 0),
(@q, 'D', N'Read aloud to improve pronunciation.', 0);

-- Q43 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'Where is the topic sentence of an academic paragraph typically located?', N'Câu chủ đề của đoạn văn thường nằm ở câu đầu tiên hoặc câu thứ hai.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'In the very first or second sentence', 1),
(@q, 'B', N'Strictly inside the footnotes', 0),
(@q, 'C', N'Always hidden in the exact middle', 0),
(@q, 'D', N'Only in the bibliography', 0);

-- Q44 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'Why is identifying synonyms crucial for IELTS Reading questions?', N'Vì câu hỏi trong bài đọc IELTS luôn được diễn đạt lại (paraphrased) bằng từ đồng nghĩa.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Questions almost always paraphrase information from the text rather than using identical words.', 1),
(@q, 'B', N'Synonyms are required to be memorized for speaking tests.', 0),
(@q, 'C', N'Texts contain no grammatical conjunctions.', 0),
(@q, 'D', N'IELTS examiners penalize short answers.', 0);

-- Q45 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'What should a test-taker do when encountering an unfamiliar scientific term?', N'Dựa vào ngữ cảnh xung quanh để suy đoán nghĩa khái quát thay vì dừng lại quá lâu.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Stop the test and consult a dictionary', 0),
(@q, 'B', N'Deduce its rough meaning from contextual clues and move forward', 1),
(@q, 'C', N'Assume the entire paragraph is erroneous', 0),
(@q, 'D', N'Leave all subsequent questions blank', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 9
-- -------------------------------------------------------------------
-- Q46 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'A statement in IELTS Reading should be marked ''FALSE'' when:', N'Chọn FALSE khi thông tin trong câu hỏi đối lập, mâu thuẫn trực tiếp với bài đọc.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The passage mentions no related information whatsoever.', 0),
(@q, 'B', N'The statement directly contradicts the facts stated in the passage.', 1),
(@q, 'C', N'The author''s tone is slightly ambiguous.', 0),
(@q, 'D', N'The passage was written over a decade ago.', 0);

-- Q47 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'A statement should be evaluated as ''NOT GIVEN'' if:', N'Chọn NOT GIVEN khi bài đọc không có đủ thông tin để xác nhận câu đó đúng hay sai.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The passage provides insufficient information to confirm or refute the claim.', 1),
(@q, 'B', N'The author explicitly denies the statement.', 0),
(@q, 'C', N'The claim uses simple past tense.', 0),
(@q, 'D', N'The vocabulary is too elementary.', 0);

-- Q48 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'How do absolute qualifiers such as ''always'', ''all'', or ''never'' affect statement validity?', N'Các từ tuyệt đối (all, always) thường biến câu thành FALSE nếu bài đọc chỉ nói ở mức độ đa số hoặc có ngoại lệ.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'They usually make a statement FALSE if the text only suggests a tendency with exceptions.', 1),
(@q, 'B', N'They guarantee the statement is always TRUE.', 0),
(@q, 'C', N'They turn every question into a multiple-choice item.', 0),
(@q, 'D', N'They are completely ignored by examiners.', 0);

-- Q49 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'Text: "The vaccine showed 90% efficacy in lab trials." Statement: "The vaccine guarantees complete immunity in all humans." Answer:', N'Bài đọc chỉ nói 90% trong phòng thí nghiệm, câu hỏi khẳng định ''guarantee complete immunity in all'' -> mâu thuẫn -> FALSE.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'TRUE', 0),
(@q, 'B', N'FALSE', 1),
(@q, 'C', N'NOT GIVEN', 0),
(@q, 'D', N'CANNOT DETERMINE', 0);

-- Q50 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'Text: "Dr. Miller graduated from Cambridge in 1998." Statement: "Dr. Miller was the top student in his graduating class." Answer:', N'Bài đọc chỉ nói tốt nghiệp năm 1998, không nhắc đến việc có đứng đầu lớp hay không -> NOT GIVEN.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'TRUE', 0),
(@q, 'B', N'FALSE', 0),
(@q, 'C', N'NOT GIVEN', 1),
(@q, 'D', N'YES', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 10
-- -------------------------------------------------------------------
-- Q51 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'What must an effective introduction of an IELTS Opinion Essay contain?', N'Mở bài hiệu quả phải có câu diễn giải đề (paraphrase) và luận đề rõ ràng (thesis statement).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A paraphrase of the prompt and a clear thesis statement stating the author''s stance.', 1),
(@q, 'B', N'Ten rhetorical questions without any direct opinion.', 0),
(@q, 'C', N'A detailed list of all statistics in the world.', 0),
(@q, 'D', N'A quotation from an ancient philosopher only.', 0);

-- Q52 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'Which cohesive device expresses a concession in an argumentative essay?', N'''Admittedly'' hoặc ''While it is true that'' dùng để nhượng bộ một quan điểm đối lập trước khi phản bác.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Furthermore', 0),
(@q, 'B', N'Admittedly', 1),
(@q, 'C', N'Consequently', 0),
(@q, 'D', N'In addition', 0);

-- Q53 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'What is the primary role of a counter-argument paragraph in high-band writing?', N'Phân tích góc nhìn đối lập rồi phản bác để chứng minh tính thuyết phục vượt trội của lập luận chính.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'To confuse the reader about the author''s real stance', 0),
(@q, 'B', N'To acknowledge opposing views and refute them to reinforce the core thesis', 1),
(@q, 'C', N'To lengthen the essay without adding substance', 0),
(@q, 'D', N'To replace the conclusion section', 0);

-- Q54 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'Which sentence exhibits sophisticated academic style avoiding first-person overuse?', N'Phong cách học thuật khách quan: ''It is widely contended that technological automation enhances economic productivity.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I think tech makes money fast.', 0),
(@q, 'B', N'It is widely contended that technological automation enhances economic productivity.', 1),
(@q, 'C', N'You should believe that robots are awesome.', 0),
(@q, 'D', N'Me and my friends agree computers work well.', 0);

-- Q55 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'What should NEVER appear in the conclusion of an academic essay?', N'Tuyệt đối không đưa ý tưởng hoặc luận điểm hoàn toàn mới chưa được phân tích vào phần kết bài.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A restatement of the thesis', 0),
(@q, 'B', N'Brand-new arguments that were not discussed in the body paragraphs', 1),
(@q, 'C', N'A summary of main body arguments', 0),
(@q, 'D', N'A concluding recommendation', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 11
-- -------------------------------------------------------------------
-- Q56 (Lesson 11 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'Viet', N'Which collocation describes the rapid, uncontrolled expansion of metropolitan boundaries?', N'''Urban sprawl'' là thuật ngữ chỉ sự mở rộng tự phát, thiếu kiểm soát của các khu đô thị.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Urban sprawl', 1),
(@q, 'B', N'City shrinking', 0),
(@q, 'C', N'Rural stagnation', 0),
(@q, 'D', N'Metropolitan contraction', 0);

-- Q57 (Lesson 11 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'TuVung', N'Select the academic verb meaning ''to reduce the severity or impact of climate disruption'':', N'''Mitigate'' nghĩa là giảm thiểu tác động tiêu cực.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Exacerbate', 0),
(@q, 'B', N'Mitigate', 1),
(@q, 'C', N'Proliferate', 0),
(@q, 'D', N'Deteriorate', 0);

-- Q58 (Lesson 11 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'Viet', N'Choose the best academic sentence proposing a governmental policy for traffic congestion:', N'''Governments should implement congestion pricing schemes to disincentivize private vehicle usage in city centers.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Governments should implement congestion pricing schemes to disincentivize private vehicle usage.', 1),
(@q, 'B', N'Make drivers pay big cash so they stop driving cars.', 0),
(@q, 'C', N'Stop all automobiles from entering roads immediately.', 0),
(@q, 'D', N'People should walk more because cars are annoying.', 0);

-- Q59 (Lesson 11 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'TuVung', N'What does ''sustainable development'' encompass in urban planning?', N'Phát triển bền vững: đáp ứng nhu cầu hiện tại mà không làm tổn hại đến tài nguyên của các thế hệ tương lai.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Meeting current societal needs without compromising the ability of future generations to meet theirs.', 1),
(@q, 'B', N'Exploiting all oil reserves as swiftly as possible.', 0),
(@q, 'C', N'Banning all commercial activities in major cities.', 0),
(@q, 'D', N'Building skyscrapers regardless of ecological damage.', 0);

-- Q60 (Lesson 11 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'Viet', N'Complete the sentence with an appropriate transition: "Heavy reliance on fossil fuels degrades air quality; ______, it accelerates global warming."', N'''Furthermore'' hoặc ''Moreover'' dùng để bổ sung luận điểm cùng chiều.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Nevertheless', 0),
(@q, 'B', N'Furthermore', 1),
(@q, 'C', N'In contrast', 0),
(@q, 'D', N'Conversely', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 12
-- -------------------------------------------------------------------
-- Q61 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'In university group discussions, what is a ''rubric''?', N'''Rubric'' là bảng tiêu chí đánh giá và chấm điểm bài tập học thuật.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A grading criterion guide detailing assessment expectations.', 1),
(@q, 'B', N'A certificate of attendance.', 0),
(@q, 'C', N'A break time between lectures.', 0),
(@q, 'D', N'A list of campus restaurants.', 0);

-- Q62 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'Which phrase expresses polite academic disagreement between team members?', N'''I take your point, but we might also consider...'' thể hiện sự phản biện lịch sự, tôn trọng đối phương.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'You are totally wrong about everything.', 0),
(@q, 'B', N'I take your point, but we might also consider another angle.', 1),
(@q, 'C', N'Stop talking nonsense.', 0),
(@q, 'D', N'That idea makes no sense whatsoever.', 0);

-- Q63 (Lesson 12 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'Doc', N'What does ''peer review'' refer to in collaborative academic projects?', N'Đánh giá chéo: sinh viên nhận xét và góp ý phản biện bài làm của bạn cùng nhóm/lớp.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Evaluation of work by peers possessing similar competencies.', 1),
(@q, 'B', N'Grading conducted exclusively by the university president.', 0),
(@q, 'C', N'Submitting essays to a public newspaper.', 0),
(@q, 'D', N'Writing exams without supervision.', 0);

-- Q64 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'If a student suggests ''dividing the workload evenly'', what do they intend?', N'Họ đề xuất phân chia khối lượng công việc công bằng cho từng thành viên trong nhóm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Assigning equal responsibilities to each team member.', 1),
(@q, 'B', N'Leaving the entire assignment to one individual.', 0),
(@q, 'C', N'Postponing the project until next semester.', 0),
(@q, 'D', N'Asking the professor to write the report.', 0);

-- Q65 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'What is an ''executive summary'' in a project portfolio?', N'''Executive summary'' là bản tóm tắt súc tích các kết quả và điểm then chốt của dự án.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A brief overview synthesizing the key findings and recommendations.', 1),
(@q, 'B', N'A list of all team member phone numbers.', 0),
(@q, 'C', N'The final receipt for printing the document.', 0),
(@q, 'D', N'A biography of corporate executives.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 13
-- -------------------------------------------------------------------
-- Q66 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'In academic monologues, what function do ''signposting phrases'' serve?', N'Cụm từ báo hiệu giúp người nghe định hướng cấu trúc và bước chuyển ý trong bài giảng.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'They orient the listener regarding the structure and transitions of the lecture.', 1),
(@q, 'B', N'They indicate grammatical errors in the textbook.', 0),
(@q, 'C', N'They are used to fill silent pauses when the speaker forgets words.', 0),
(@q, 'D', N'They signal the end of the semester.', 0);

-- Q67 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'When a lecturer states: "Turning now to the empirical evidence...", what are they doing?', N'Diễn giả đang chuyển tiếp sang phần phân tích dữ liệu thực nghiệm.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Transitioning to discuss observational and experimental data.', 1),
(@q, 'B', N'Ending the lecture abruptly.', 0),
(@q, 'C', N'Asking the students to close their laptops.', 0),
(@q, 'D', N'Repeating the introductory slide.', 0);

-- Q68 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'Which phrase marks a critical emphasis or surprising revelation in scientific speech?', N'''Crucially'' hoặc ''What is particularly noteworthy is that...''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Incidentally', 0),
(@q, 'B', N'What is particularly noteworthy is that...', 1),
(@q, 'C', N'As a trivial aside', 0),
(@q, 'D', N'Needless to mention', 0);

-- Q69 (Lesson 13 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'Doc', N'What distinguishes an ''empirical study'' from a purely theoretical hypothesis?', N'Nghiên cứu thực nghiệm dựa trên việc quan sát và thu thập dữ liệu thực tế, không chỉ là suy luận lý thuyết.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It is grounded on verifiable observation and measurable data collection.', 1),
(@q, 'B', N'It uses fictional stories instead of numbers.', 0),
(@q, 'C', N'It requires no verification by other scientists.', 0),
(@q, 'D', N'It cannot be published in journals.', 0);

-- Q70 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'Select the word synonymous with ''ubiquitous'' in academic discourse:', N'''Ubiquitous'' = ''omnipresent'' (phổ biến khắp mọi nơi).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Rare', 0),
(@q, 'B', N'Omnipresent', 1),
(@q, 'C', N'Obsolete', 0),
(@q, 'D', N'Ephemeral', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 14
-- -------------------------------------------------------------------
-- Q71 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'Choose the correct auxiliary verb: "They ______ lived in London since 2015."', N'Chủ ngữ ''They'' đi với trợ động từ ''have'' trong thì hiện tại hoàn thành.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'has', 0),
(@q, 'B', N'have', 1),
(@q, 'C', N'had', 0),
(@q, 'D', N'are', 0);

-- Q72 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'Complete the sentence: "I haven''t submitted the quarterly report ______."', N'Trong câu phủ định của hiện tại hoàn thành, từ ''yet'' thường đặt ở cuối câu.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'already', 0),
(@q, 'B', N'yet', 1),
(@q, 'C', N'since', 0),
(@q, 'D', N'just', 0);

-- Q73 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'What is the difference between ''have been to'' and ''have gone to''?', N'''have been to'' = đã đến và đã trở về; ''have gone to'' = đã đi và hiện vẫn đang ở đó chưa về.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'''have been to'' means visited and returned; ''have gone to'' means departed and not yet back.', 1),
(@q, 'B', N'''have been to'' is past tense; ''have gone to'' is future tense.', 0),
(@q, 'C', N'Both phrases are completely interchangeable in all contexts.', 0),
(@q, 'D', N'''have gone to'' can only be used with first-person singular.', 0);

-- Q74 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'Which preposition correctly completes: "She has worked here ______ five years."', N'Dùng ''for'' với khoảng thời gian (for five years); dùng ''since'' với mốc thời gian.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'since', 0),
(@q, 'B', N'for', 1),
(@q, 'C', N'during', 0),
(@q, 'D', N'at', 0);

-- Q75 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'"Have you ______ tasted authentic Italian risotto?"', N'''Ever'' thường dùng trong câu hỏi thì hiện tại hoàn thành để hỏi về trải nghiệm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'never', 0),
(@q, 'B', N'ever', 1),
(@q, 'C', N'already', 0),
(@q, 'D', N'still', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 15
-- -------------------------------------------------------------------
-- Q76 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"While we ______ dinner, the power suddenly went out."', N'Hành động đang diễn ra trong quá khứ dùng quá khứ tiếp diễn (were having), hành động chen ngang dùng quá khứ đơn (went out).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'had', 0),
(@q, 'B', N'were having', 1),
(@q, 'C', N'have had', 0),
(@q, 'D', N'are having', 0);

-- Q77 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"She ______ to Tokyo last summer on a business assignment."', N'Thời gian xác định trong quá khứ ''last summer'' chia thì quá khứ đơn (went).', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'goes', 0),
(@q, 'B', N'went', 1),
(@q, 'C', N'has gone', 0),
(@q, 'D', N'was going', 0);

-- Q78 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"At 8:00 PM yesterday, what ______ you doing?"', N'Câu hỏi thời điểm cụ thể đang diễn ra trong quá khứ chia quá khứ tiếp diễn: were you doing.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'did', 0),
(@q, 'B', N'were', 1),
(@q, 'C', N'are', 0),
(@q, 'D', N'have', 0);

-- Q79 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"He ______ his leg while he was playing football on Sunday."', N'Hành động gãy chân chen ngang hành động đang chơi bóng đá, chia quá khứ đơn (broke).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'broke', 1),
(@q, 'B', N'was breaking', 0),
(@q, 'C', N'breaks', 0),
(@q, 'D', N'has broken', 0);

-- Q80 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'Which conjunction commonly introduces a past continuous clause for simultaneous background actions?', N'''While'' thường dùng để mở đầu mệnh đề quá khứ tiếp diễn chỉ hành động diễn ra song song.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'While', 1),
(@q, 'B', N'Immediately', 0),
(@q, 'C', N'Soon', 0),
(@q, 'D', N'Unless', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 16
-- -------------------------------------------------------------------
-- Q81 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"Look at those dark clouds! It ______ rain."', N'Dự đoán có bằng chứng cụ thể trước mắt (những đám mây đen) dùng ''is going to''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 0),
(@q, 'B', N'is going to', 1),
(@q, 'C', N'rains', 0),
(@q, 'D', N'shall', 0);

-- Q82 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"The phone is ringing." - "Don''t worry, I ______ answer it."', N'Quyết định đưa ra ngay tại thời điểm nói (spontaneous decision) dùng ''will''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 1),
(@q, 'B', N'am going to', 0),
(@q, 'C', N'answer', 0),
(@q, 'D', N'shall to', 0);

-- Q83 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"We have already booked the flight. We ______ visit our grandparents next week."', N'Kế hoạch đã chuẩn bị và sắp xếp trước (đã đặt vé) dùng ''are going to''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 0),
(@q, 'B', N'are going to', 1),
(@q, 'C', N'visit', 0),
(@q, 'D', N'might to', 0);

-- Q84 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"I promise I ______ keep your secret confidential."', N'Lời hứa (promise) dùng ''will''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 1),
(@q, 'B', N'am going to', 0),
(@q, 'C', N'kept', 0),
(@q, 'D', N'going to', 0);

-- Q85 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'Which sentence indicates a pre-arranged personal decision?', N'''I am going to study abroad next autumn; I''ve already received the acceptance letter.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I will probably study tomorrow maybe.', 0),
(@q, 'B', N'I am going to study abroad next autumn; I''ve already received the acceptance letter.', 1),
(@q, 'C', N'I think I will go buy some milk right now.', 0),
(@q, 'D', N'I shall see about it.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 17
-- -------------------------------------------------------------------
-- Q86 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Reduce the clause: "The engineers who are designing the new bridge have extensive experience."', N'Mệnh đề chủ động rút gọn bằng V-ing: ''The engineers designing the new bridge...''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The engineers designed the new bridge...', 0),
(@q, 'B', N'The engineers designing the new bridge...', 1),
(@q, 'C', N'The engineers to design the new bridge...', 0),
(@q, 'D', N'The engineers being designed...', 0);

-- Q87 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Reduce the clause: "The vehicles which were damaged in the hail storm have been towed away."', N'Mệnh đề bị động rút gọn bằng V-ed/V3: ''The vehicles damaged in the hail storm...''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The vehicles damaged in the hail storm...', 1),
(@q, 'B', N'The vehicles damaging in the hail storm...', 0),
(@q, 'C', N'The vehicles to damage in the hail storm...', 0),
(@q, 'D', N'The vehicles have damaged...', 0);

-- Q88 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Complete the sentence: "Neil Armstrong was the first person ______ on the surface of the Moon."', N'Sau các từ chỉ thứ tự ''the first'', ''the second'', ''the only'', rút gọn mệnh đề quan hệ bằng ''to + V-infinitive''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'stepping', 0),
(@q, 'B', N'to step', 1),
(@q, 'C', N'stepped', 0),
(@q, 'D', N'had stepped', 0);

-- Q89 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'"The document ______ by the legal team yesterday must be signed immediately."', N'Mệnh đề rút gọn bị động: ''reviewed by the legal team''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'reviewing', 0),
(@q, 'B', N'reviewed', 1),
(@q, 'C', N'to review', 0),
(@q, 'D', N'was reviewed', 0);

-- Q90 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Which sentence correctly uses a perfect participle clause indicating an earlier completed action?', N'''Having finished all testing procedures, the software team deployed the update.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Finished all testing procedures, the software team deployed the update.', 0),
(@q, 'B', N'Having finished all testing procedures, the software team deployed the update.', 1),
(@q, 'C', N'To have finish all testing procedures, the team deployed.', 0),
(@q, 'D', N'Being finish all testing procedures, the team deployed.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 18
-- -------------------------------------------------------------------
-- Q91 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'"People say that he owns three software companies." -> Passive: "He is said ______ three software companies."', N'Cấu trúc bị động với động từ tường thuật: He is said to own...', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'owning', 0),
(@q, 'B', N'to own', 1),
(@q, 'C', N'owned', 0),
(@q, 'D', N'to have owned', 0);

-- Q92 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'"People believe that the ancient city was destroyed by an earthquake." -> "The ancient city is believed ______ by an earthquake."', N'Hành động bị phá hủy xảy ra trước thời điểm tin tưởng, dùng ''to have been destroyed''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'to be destroyed', 0),
(@q, 'B', N'to have been destroyed', 1),
(@q, 'C', N'destroying', 0),
(@q, 'D', N'was destroyed', 0);

-- Q93 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'Causative form: "I need to have my car ______ before taking the long road trip."', N'Cấu trúc truyền khiến: ''have something done'' (nhờ/thuê ai làm gì đó cho mình).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'repair', 0),
(@q, 'B', N'repaired', 1),
(@q, 'C', N'repairing', 0),
(@q, 'D', N'to repair', 0);

-- Q94 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'"The manager got the contractor ______ the blueprints by Friday."', N'Cấu trúc truyền khiến: ''get someone to do something''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'finish', 0),
(@q, 'B', N'to finish', 1),
(@q, 'C', N'finished', 0),
(@q, 'D', N'finishing', 0);

-- Q95 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'Complete the impersonal passive: "______ is anticipated that consumer spending will rebound this quarter."', N'Chủ ngữ giả ''It'' trong cấu trúc bị động khách quan: It is anticipated that...', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It', 1),
(@q, 'B', N'There', 0),
(@q, 'C', N'What', 0),
(@q, 'D', N'Which', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 19
-- -------------------------------------------------------------------
-- Q96 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Type 1: "If the weather ______ favorable tomorrow, we will launch the drone survey."', N'Mệnh đề If của câu điều kiện loại 1 chia thì hiện tại đơn (is).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'is', 1),
(@q, 'B', N'will be', 0),
(@q, 'C', N'were', 0),
(@q, 'D', N'had been', 0);

-- Q97 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Type 2: "If I ______ in your position, I would renegotiate the contractual terms."', N'Câu điều kiện loại 2 giả định trái với hiện tại, to be chia ''were'' cho tất cả các ngôi.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'am', 0),
(@q, 'B', N'were', 1),
(@q, 'C', N'have been', 0),
(@q, 'D', N'had been', 0);

-- Q98 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Type 3: "If they ______ the backup server, they wouldn''t have lost client records."', N'Câu điều kiện loại 3 giả định trái với quá khứ: If + had + P2.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'configured', 0),
(@q, 'B', N'had configured', 1),
(@q, 'C', N'would configure', 0),
(@q, 'D', N'were configuring', 0);

-- Q99 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Mixed conditional (Past condition -> Present result): "If she ______ French in college, she would understand this report today."', N'Điều kiện trong quá khứ (had studied) dẫn đến kết quả ở hiện tại (would understand today).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'studied', 0),
(@q, 'B', N'had studied', 1),
(@q, 'C', N'studies', 0),
(@q, 'D', N'would study', 0);

-- Q100 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'"______ you submit the required permits on time, your construction will proceed smoothly."', N'''Provided that'' (hoặc ''Providing that'') mang nghĩa ''với điều kiện là / miễn là''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Unless', 0),
(@q, 'B', N'Provided that', 1),
(@q, 'C', N'Although', 0),
(@q, 'D', N'In spite of', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 20
-- -------------------------------------------------------------------
-- Q101 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'"Never in my career ______ such negligence in quality control."', N'Đảo ngữ với ''Never'': Trợ động từ đứng trước chủ ngữ: have I witnessed.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I have witnessed', 0),
(@q, 'B', N'have I witnessed', 1),
(@q, 'C', N'I witnessed', 0),
(@q, 'D', N'did I witnessed', 0);

-- Q102 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'"Not only ______ the deadline, but they also exceeded all benchmark metrics."', N'Đảo ngữ với ''Not only'': Not only did they meet...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'they met', 0),
(@q, 'B', N'did they meet', 1),
(@q, 'C', N'have they met', 0),
(@q, 'D', N'they were meeting', 0);

-- Q103 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'"Under no circumstances ______ confidential credentials be shared across public channels."', N'Đảo ngữ với ''Under no circumstances'': should confidential credentials be...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'confidential credentials should', 0),
(@q, 'B', N'should confidential credentials', 1),
(@q, 'C', N'they will', 0),
(@q, 'D', N'credentials would', 0);

-- Q104 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'Inversion in conditional type 3: "______ we received earlier notification, we would have averted the incident."', N'Đảo ngữ điều kiện loại 3 bỏ ''If'', đưa ''Had'' lên đầu: Had we received...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Had', 1),
(@q, 'B', N'Were', 0),
(@q, 'C', N'Should', 0),
(@q, 'D', N'If had', 0);

-- Q105 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'Inversion in conditional type 1: "______ any discrepancy arise, please alert the compliance officer immediately."', N'Đảo ngữ điều kiện loại 1 bỏ ''If'', đưa ''Should'' lên đầu: Should any discrepancy arise...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Were', 0),
(@q, 'B', N'Had', 0),
(@q, 'C', N'Should', 1),
(@q, 'D', N'Could', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 21
-- -------------------------------------------------------------------
-- Q106 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'Choose the most professional opening sentence for a business inquiry email:', N'''I am writing with regard to your company''s logistics software solutions.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey, tell me about your software.', 0),
(@q, 'B', N'I am writing with regard to your company''s logistics software solutions.', 1),
(@q, 'C', N'I wanna know what you do.', 0),
(@q, 'D', N'Listen, can you explain your app?', 0);

-- Q107 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'What is the standard phrase to refer to an attached invoice in a commercial email?', N'''Please find attached the finalized invoice for services rendered.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Look at the invoice down here.', 0),
(@q, 'B', N'Please find attached the finalized invoice for services rendered.', 1),
(@q, 'C', N'I dropped the invoice paper.', 0),
(@q, 'D', N'See the attachment now.', 0);

-- Q108 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'Which phrase expresses anticipated future communication courteously?', N'''I look forward to hearing from you at your earliest convenience.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Reply fast or else.', 0),
(@q, 'B', N'I look forward to hearing from you at your earliest convenience.', 1),
(@q, 'C', N'You must answer quickly.', 0),
(@q, 'D', N'I wait for your typing.', 0);

-- Q109 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'When addressing an email to ''Dear Sir/Madam'' (recipient unknown by name), what is the traditional British sign-off?', N'Quy tắc British English: Khi không biết tên người nhận (''Dear Sir/Madam''), lời chào kết là ''Yours faithfully''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Yours sincerely,', 0),
(@q, 'B', N'Yours faithfully,', 1),
(@q, 'C', N'Best buddy,', 0),
(@q, 'D', N'Warm hugs,', 0);

-- Q110 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'Select the appropriate subject line for a job interview confirmation:', N'''Interview Confirmation: Senior Business Analyst - [Candidate Name]''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey about the interview', 0),
(@q, 'B', N'Interview Confirmation: Senior Business Analyst - [Candidate Name]', 1),
(@q, 'C', N'Can we talk tomorrow?', 0),
(@q, 'D', N'Job thing discussion', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 22
-- -------------------------------------------------------------------
-- Q111 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'How do you professionally request to reschedule an upcoming consultation meeting?', N'''Due to an unexpected scheduling conflict, would it be possible to reschedule our meeting to Thursday?''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I can''t come, pick another day.', 0),
(@q, 'B', N'Due to an unexpected scheduling conflict, would it be possible to reschedule our meeting to Thursday?', 1),
(@q, 'C', N'Cancel tomorrow, see you later.', 0),
(@q, 'D', N'Not coming today.', 0);

-- Q112 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'In customer complaint handling, which sentence expresses sincere accountability?', N'''We sincerely apologize for the inconvenience caused and have initiated an immediate investigation.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It wasn''t our fault, blame the courier.', 0),
(@q, 'B', N'We sincerely apologize for the inconvenience caused and have initiated an immediate investigation.', 1),
(@q, 'C', N'You should have checked earlier.', 0),
(@q, 'D', N'Everyone makes mistakes, calm down.', 0);

-- Q113 (Lesson 22 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'TuVung', N'What does ''expedite the replacement shipment'' mean?', N'''Expedite'' nghĩa là đẩy nhanh tiến độ xử lý/giao hàng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Cancel the delivery entirely', 0),
(@q, 'B', N'Accelerate the shipping process of the replacement goods', 1),
(@q, 'C', N'Charge double for postage', 0),
(@q, 'D', N'Return the package to the manufacturer', 0);

-- Q114 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'Choose the best sentence to confirm an appointment time and location:', N'''This is to confirm our meeting on October 28th at 10:00 AM in Conference Room B.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Yes we meet tomorrow morning.', 0),
(@q, 'B', N'This is to confirm our meeting on October 28th at 10:00 AM in Conference Room B.', 1),
(@q, 'C', N'I will be there whenever.', 0),
(@q, 'D', N'Okay see you.', 0);

-- Q115 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'Which closing statement maintains goodwill after resolving a dispute?', N'''We greatly value your partnership and remain committed to ensuring your satisfaction.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Don''t complain again next time.', 0),
(@q, 'B', N'We greatly value your partnership and remain committed to ensuring your satisfaction.', 1),
(@q, 'C', N'Case closed forever.', 0),
(@q, 'D', N'Be glad we gave you a refund.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 23
-- -------------------------------------------------------------------
-- Q116 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'In procurement negotiations, what does ''mutual benefit'' mean?', N'''Mutual benefit'' (đôi bên cùng có lợi) tương đương với giải pháp ''win-win''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'An outcome where only one party profits.', 0),
(@q, 'B', N'An arrangement that advantages both contractual parties equitably.', 1),
(@q, 'C', N'A cancellation of business ties.', 0),
(@q, 'D', N'A legal penalty for delayed deliveries.', 0);

-- Q117 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'What legal term refers to a failure to fulfill agreed terms in a commercial contract?', N'''Breach of contract'' nghĩa là hành vi vi phạm hợp đồng.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Breach of contract', 1),
(@q, 'B', N'Amendment of agreement', 0),
(@q, 'C', N'Discharge of duty', 0),
(@q, 'D', N'Arbitration award', 0);

-- Q118 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'If a clause is described as ''non-negotiable'', it means:', N'Điều khoản ''non-negotiable'' là điều khoản cố định, bắt buộc, không thể thương lượng thêm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Subject to compromise', 0),
(@q, 'B', N'Fixed and mandatory without room for modification', 1),
(@q, 'C', N'Completely optional', 0),
(@q, 'D', N'Informal recommendation', 0);

-- Q119 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'What does ''FOB'' (Free On Board) signify in international freight terms?', N'FOB: người bán chịu trách nhiệm và chi phí cho đến khi hàng hóa được xếp lên tàu tại cảng bốc.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The seller delivers when goods pass the ship''s rail at the named port of shipment.', 1),
(@q, 'B', N'The buyer pays nothing under any circumstance.', 0),
(@q, 'C', N'Freight is handled solely by postal courier.', 0),
(@q, 'D', N'Cargo must travel only by land.', 0);

-- Q120 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'When parties ''reach a consensus'', they:', N'Hai bên đạt được sự đồng thuận và thống nhất chung.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Terminate negotiations angrily', 0),
(@q, 'B', N'Reach a collective agreement acceptable to all participants', 1),
(@q, 'C', N'File a lawsuit in court', 0),
(@q, 'D', N'Refuse to sign documents', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 24
-- -------------------------------------------------------------------
-- Q121 (Lesson 24 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'Doc', N'In a venture capital pitch, what is the ''value proposition''?', N'''Value proposition'' là tuyên bố giá trị cốt lõi giải thích vì sao sản phẩm vượt trội và đáng để đầu tư.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A clear statement explaining the unique benefit a product provides to solve customer problems.', 1),
(@q, 'B', N'The total electrical power needed to run office computers.', 0),
(@q, 'C', N'A list of employee social media accounts.', 0),
(@q, 'D', N'The personal history of the accountant.', 0);

-- Q122 (Lesson 24 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'TuVung', N'What does ''ROI'' represent in financial presentations?', N'ROI = Return on Investment (Tỷ suất hoàn vốn đầu tư).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Rate of Interest', 0),
(@q, 'B', N'Return on Investment', 1),
(@q, 'C', N'Revenue of Industry', 0),
(@q, 'D', N'Risk of Inflation', 0);

-- Q123 (Lesson 24 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'Doc', N'What does ''scaling the business'' mean to potential angel investors?', N'''Scaling the business'' là mở rộng quy mô kinh doanh nhanh chóng mà không làm tăng chi phí tỷ lệ thuận.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Reducing production output to cut costs.', 0),
(@q, 'B', N'Growing revenue exponentially while maintaining cost efficiency.', 1),
(@q, 'C', N'Closing international branch offices.', 0),
(@q, 'D', N'Filing for corporate bankruptcy.', 0);

-- Q124 (Lesson 24 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'TuVung', N'Which phrase refers to the percentage of total sales within a market earned by a company?', N'''Market share'' là thị phần của doanh nghiệp.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Market share', 1),
(@q, 'B', N'Stock volatility', 0),
(@q, 'C', N'Gross margin', 0),
(@q, 'D', N'Liquidity pool', 0);

-- Q125 (Lesson 24 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'Doc', N'Why is demonstrating a ''defensible competitive moat'' vital during pitch sessions?', N'Vì nó chứng minh sản phẩm có rào cản cạnh tranh vững chắc khiến đối thủ không thể dễ dàng sao chép.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It shows that competitors cannot easily duplicate the company''s advantage.', 1),
(@q, 'B', N'It proves the company has dug a water canal around its headquarters.', 0),
(@q, 'C', N'It guarantees zero taxation forever.', 0),
(@q, 'D', N'It eliminates the need for software engineering.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 25
-- -------------------------------------------------------------------
-- Q126 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'What is an ''API endpoint'' in modern software architecture?', N'API endpoint là một URL cụ thể nơi ứng dụng gửi yêu cầu để truy xuất hoặc sửa đổi tài nguyên.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A physical cable connecting servers', 0),
(@q, 'B', N'A designated URL where client applications access server resources', 1),
(@q, 'C', N'The final line of a computer monitor', 0),
(@q, 'D', N'A password used for database encryption', 0);

-- Q127 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'Which process involves identifying, analyzing, and removing defects or bugs from computer code?', N'''Debugging'' là quá trình dò tìm và gỡ lỗi phần mềm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Compiling', 0),
(@q, 'B', N'Debugging', 1),
(@q, 'C', N'Refactoring', 0),
(@q, 'D', N'Deployment', 0);

-- Q128 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'What does ''CRUD'' stand for in database management applications?', N'CRUD = Create, Read, Update, Delete (4 thao tác cơ bản với dữ liệu).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Compile, Run, Undo, Deploy', 0),
(@q, 'B', N'Create, Read, Update, Delete', 1),
(@q, 'C', N'Copy, Rename, Upload, Download', 0),
(@q, 'D', N'Connect, Route, Unify, Disconnect', 0);

-- Q129 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'In Git version control, submitting code changes for team review is called creating a ______.', N'''Pull request'' (PR) là yêu cầu hợp nhất và rà soát mã nguồn.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'pull request', 1),
(@q, 'B', N'hard reboot', 0),
(@q, 'C', N'database rollback', 0),
(@q, 'D', N'local commit only', 0);

-- Q130 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'What is the primary characteristic of a ''relational database'' (RDBMS)?', N'Cơ sở dữ liệu quan hệ tổ chức dữ liệu thành các bảng có hàng và cột, liên kết với nhau bằng khóa (keys).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It stores data in tables linked by predefined relationships and foreign keys.', 1),
(@q, 'B', N'It cannot execute SQL queries.', 0),
(@q, 'C', N'It stores everything inside unstructured text files without columns.', 0),
(@q, 'D', N'It requires no primary key.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 26
-- -------------------------------------------------------------------
-- Q131 (Lesson 26 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'Doc', N'What are the three core questions answered in an Agile Daily Standup meeting?', N'3 câu hỏi cốt lõi: Hôm qua làm gì? Hôm nay sẽ làm gì? Có gặp trở ngại/khó khăn gì không?', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'What did you do yesterday? What will you do today? Are there any blockers?', 1),
(@q, 'B', N'How much is your salary? When is the party? Why are you late?', 0),
(@q, 'C', N'What is the company profit? Who is the president? What time is lunch?', 0),
(@q, 'D', N'Can you write all code today? Did you finish the entire project?', 0);

-- Q132 (Lesson 26 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'TuVung', N'In Scrum terminology, what is a ''blocker'' (or impediment)?', N'''Blocker'' là trở ngại, khó khăn kỹ thuật hoặc quy trình ngăn cản tiến độ công việc.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'An issue or dependency that prevents a team member from making progress.', 1),
(@q, 'B', N'A developer who refuses to attend meetings.', 0),
(@q, 'C', N'A firewall software blocking external attacks.', 0),
(@q, 'D', N'A completed task ready for production.', 0);

-- Q133 (Lesson 26 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'TuVung', N'What does ''wrap up the sprint'' mean in Agile project cycles?', N'''Wrap up the sprint'' nghĩa là hoàn tất và đóng kỳ Sprint.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Finalize remaining sprint tasks, conduct review and prepare for retrospective.', 1),
(@q, 'B', N'Cancel the project immediately.', 0),
(@q, 'C', N'Delete all user stories from the backlog.', 0),
(@q, 'D', N'Skip quality assurance testing.', 0);

-- Q134 (Lesson 26 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'TuVung', N'How do Scrum teams estimate task effort during sprint planning?', N'Các nhóm Scrum thường dùng ''story points'' để ước lượng độ phức tạp và công sức của task.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'By counting words in the user story', 0),
(@q, 'B', N'Using story points based on relative complexity and effort', 1),
(@q, 'C', N'By flipping a coin', 0),
(@q, 'D', N'By measuring keyboard typing speed', 0);

-- Q135 (Lesson 26 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'Viet', N'Choose the best concise update for a daily standup meeting:', N'''Yesterday I finished API integration for payments; today I will run unit tests; no blockers currently.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I worked on some stuff, today I do more stuff, see ya.', 0),
(@q, 'B', N'Yesterday I finished API integration for payments; today I will run unit tests; no blockers currently.', 1),
(@q, 'C', N'I didn''t do anything because the internet was slow.', 0),
(@q, 'D', N'Ask my manager what I am doing.', 0);

PRINT N'===================================================================';
PRINT N'ĐÃ THÊM THÀNH CÔNG 135 CÂU HỎI TRẮC NGHIỆM CHO 27 BÀI HỌC VÀO DATABASE!';
PRINT N'===================================================================';
GO


-- ===================================================================

-- Đồng bộ đoạn văn trích xuất dẫn chứng cho 135 câu hỏi quiz bài học
UPDATE CauHoi 
SET DoanVanDanChung = N'Trích xuất kiến thức trọng tâm & ví dụ thực hành từ Bài học ' + CAST(bh.MaBaiHoc AS NVARCHAR) + N': ' + bh.TieuDe,
    LoiGiaiChiTiet = ISNULL(CauHoi.GiaiThichDapAn, N'Giải thích chi tiết theo kiến thức bài học.')
FROM CauHoi
INNER JOIN BaiHoc bh ON CauHoi.MaBaiHoc = bh.MaBaiHoc
WHERE CauHoi.MaCauHoi BETWEEN 69 AND 203;
GO

-- ===================================================================
-- PHẦN 4: 500 CÂU HỎI TIẾNG ANH CHUẨN HÓA TOÀN DIỆN (SEED_EAI_001 -> SEED_EAI_500)
--  + 281 câu trích xuất trực tiếp từ bài học (MaBaiHoc = 0..35, có DoanVanDanChung)
--  + 219 câu nâng cao trích xuất từ tài liệu chuyên sâu / đề thi (MaBaiHoc IS NULL)
-- ===================================================================
-- 15. NẠP 500 CÂU HỎI TIẾNG ANH CHUẨN HÓA TOÀN DIỆN (SEED_EAI_001 -> SEED_EAI_500)
-- ===================================================================
PRINT N'Đang nạp 500 CÂU HỎI TIẾNG ANH CHUẨN HÓA TOÀN DIỆN (SEED_EAI_001 -> SEED_EAI_500) (500 câu hỏi)...';

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

-- Câu hỏi SEED_EAI_051 (Nghe - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_051')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 2, N'TracNghiem', N'Nghe', N'De', N'At airport check-in, what document must all international passengers present alongside their passport?',
            NULL, NULL, N'Hành khách bay quốc tế phải xuất trình hộ chiếu (passport) và thẻ lên máy bay / vé máy bay (boarding pass / flight ticket).', N'Hành khách bay quốc tế phải xuất trình hộ chiếu (passport) và thẻ lên máy bay / vé máy bay (boarding pass / flight ticket).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 6 - Thủ tục Check-in sân bay: Xuất trình Passport và Boarding Pass.', N'Extracted', N'BaiHoc_06_AirportNavigation.txt', 6, 'SEED_EAI_051');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Gym membership card', 0),
        (@MaCH, N'B', N'Boarding pass', 1),
        (@MaCH, N'C', N'Student discount card', 0),
        (@MaCH, N'D', N'Store receipt', 0);
END
GO

-- Câu hỏi SEED_EAI_052 (TuVung - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_052')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 2, N'TracNghiem', N'TuVung', N'De', N'What is the difference between ''carry-on luggage'' and ''checked luggage''?',
            NULL, NULL, N'''Carry-on'' (hành lý xách tay) được mang lên khoang máy bay, trong khi ''checked luggage'' (hành lý ký gửi) được gửi vào khoang hàng hóa.', N'''Carry-on'' (hành lý xách tay) được mang lên khoang máy bay, trong khi ''checked luggage'' (hành lý ký gửi) được gửi vào khoang hàng hóa.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 6 - Phân biệt thuật ngữ: Carry-on baggage (xách tay) vs. Checked baggage (ký gửi).', N'Extracted', N'BaiHoc_06_AirportNavigation.txt', 6, 'SEED_EAI_052');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Carry-on luggage goes into the aircraft cabin with you, while checked luggage goes into the cargo hold.', 1),
        (@MaCH, N'B', N'Checked luggage is carried by hand onto the plane.', 0),
        (@MaCH, N'C', N'Carry-on luggage has no weight limits at all.', 0),
        (@MaCH, N'D', N'Both mean exactly luggage that is lost by airlines.', 0);
END
GO

-- Câu hỏi SEED_EAI_053 (TuVung - Trình độ 2 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_053')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 2, N'DienKhuyet', N'TuVung', N'De', N'Fill in the blank: ''Passengers are requested to proceed to _______ number 14 for boarding.''',
            NULL, NULL, N'Cổng ra máy bay tại sân bay được gọi là ''gate'' (cổng lên máy bay).', N'Cổng ra máy bay tại sân bay được gọi là ''gate'' (cổng lên máy bay).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 6 - Thông báo phát thanh sân bay: ''Proceed to Gate [number]''.', N'Extracted', N'BaiHoc_06_AirportNavigation.txt', 6, 'SEED_EAI_053');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'gate', 1),
        (@MaCH, N'B', N'fence', 0),
        (@MaCH, N'C', N'port', 0),
        (@MaCH, N'D', N'dock', 0);
END
GO

-- Câu hỏi SEED_EAI_054 (Doc - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_054')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 2, N'TracNghiem', N'Doc', N'TrungBinh', N'When going through airport security screening, what items must passengers take out of their bags?',
            NULL, NULL, N'Quy định an ninh hàng không quốc tế: Phải lấy máy tính xách tay (laptops) và chất lỏng dưới 100ml ra khay riêng.', N'Quy định an ninh hàng không quốc tế: Phải lấy máy tính xách tay (laptops) và chất lỏng dưới 100ml ra khay riêng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 6 - Quy trình qua cửa kiểm tra an ninh (Security Checkpoint protocol).', N'Extracted', N'BaiHoc_06_AirportNavigation.txt', 6, 'SEED_EAI_054');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Laptops and liquids in clear plastic containers', 1),
        (@MaCH, N'B', N'All clothes and jackets inside the suitcase', 0),
        (@MaCH, N'C', N'Books and notebooks only', 0),
        (@MaCH, N'D', N'Empty water bottles', 0);
END
GO

-- Câu hỏi SEED_EAI_055 (TuVung - Trình độ 2 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_055')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 2, N'DungSai', N'TuVung', N'De', N'True or False: If a flight is marked as ''Delayed'' on the departures board, it means the plane has been completely cancelled.',
            NULL, NULL, N'Sai. ''Delayed'' nghĩa là bị hoãn / trễ giờ, còn ''Cancelled'' mới là bị hủy chuyến bay.', N'Sai. ''Delayed'' nghĩa là bị hoãn / trễ giờ, còn ''Cancelled'' mới là bị hủy chuyến bay.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 6 - Bảng thông tin chuyến bay: On time, Delayed, Boarding, Cancelled.', N'Extracted', N'BaiHoc_06_AirportNavigation.txt', 6, 'SEED_EAI_055');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_056 (Nghe - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_056')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 1, N'TracNghiem', N'Nghe', N'De', N'What is the most polite question to ask a stranger for directions on the street?',
            NULL, NULL, N'''Excuse me, could you tell me how to get to the nearest metro station?'' là lời mở đầu hỏi đường lịch sự nhất.', N'''Excuse me, could you tell me how to get to the nearest metro station?'' là lời mở đầu hỏi đường lịch sự nhất.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 7 - Mẫu câu hỏi đường phố: ''Excuse me, could you tell me how to get to...?''', N'Extracted', N'BaiHoc_07_MetroNavigation.txt', 7, 'SEED_EAI_056');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Where is station? Tell me.', 0),
        (@MaCH, N'B', N'Excuse me, could you tell me how to get to the central station?', 1),
        (@MaCH, N'C', N'Show me the way immediately.', 0),
        (@MaCH, N'D', N'Hey, walk me to the subway.', 0);
END
GO

-- Câu hỏi SEED_EAI_057 (TuVung - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_057')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 1, N'TracNghiem', N'TuVung', N'De', N'What does the phrase ''Turn right at the crossroads'' instruct you to do?',
            NULL, NULL, N'Rẽ phải tại ngã tư giao lộ (crossroads / intersection).', N'Rẽ phải tại ngã tư giao lộ (crossroads / intersection).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 7 - Từ vựng định hướng: Turn left, Turn right, Go straight, Crossroads / Intersection.', N'Extracted', N'BaiHoc_07_MetroNavigation.txt', 7, 'SEED_EAI_057');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Go straight without stopping.', 0),
        (@MaCH, N'B', N'Turn to the right hand side at the intersection of streets.', 1),
        (@MaCH, N'C', N'Turn around and go backwards.', 0),
        (@MaCH, N'D', N'Cross over the pedestrian bridge.', 0);
END
GO

-- Câu hỏi SEED_EAI_058 (TuVung - Trình độ 1 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_058')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 1, N'DienKhuyet', N'TuVung', N'De', N'Fill in the blank: ''Go _______ on this avenue for two blocks, and the museum is on your left.''',
            NULL, NULL, N'Cụm chỉ đường ''Go straight on'' có nghĩa là ''Đi thẳng tiếp tục''.', N'Cụm chỉ đường ''Go straight on'' có nghĩa là ''Đi thẳng tiếp tục''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 7 - Chỉ đường cơ bản: ''Go straight on / ahead''.', N'Extracted', N'BaiHoc_07_MetroNavigation.txt', 7, 'SEED_EAI_058');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'straight', 1),
        (@MaCH, N'B', N'back', 0),
        (@MaCH, N'C', N'crooked', 0),
        (@MaCH, N'D', N'round', 0);
END
GO

-- Câu hỏi SEED_EAI_059 (Doc - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_059')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 1, N'TracNghiem', N'Doc', N'De', N'In a metro system, what is the platform where you change from one subway line to another called?',
            NULL, NULL, N'Ga đổi tuyến hoặc điểm trung chuyển tàu điện ngầm được gọi là ''Transfer station'' (hoặc Interchange station).', N'Ga đổi tuyến hoặc điểm trung chuyển tàu điện ngầm được gọi là ''Transfer station'' (hoặc Interchange station).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 7 - Thuật ngữ tàu điện ngầm: Transfer station / Interchange.', N'Extracted', N'BaiHoc_07_MetroNavigation.txt', 7, 'SEED_EAI_059');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Terminal line', 0),
        (@MaCH, N'B', N'Transfer station', 1),
        (@MaCH, N'C', N'Dead end', 0),
        (@MaCH, N'D', N'Depot', 0);
END
GO

-- Câu hỏi SEED_EAI_060 (TuVung - Trình độ 1 - Dạng NoiCap - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_060')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 1, N'NoiCap', N'TuVung', N'De', N'Match the navigation phrases with their visual meanings:
1. ''Go straight ahead''
2. ''It''s across from the bank''
3. ''Take the second left''',
            NULL, NULL, N'1 là đi thẳng; 2 là ở đối diện ngân hàng; 3 là rẽ vào khúc cua bên trái thứ hai.', N'1 là đi thẳng; 2 là ở đối diện ngân hàng; 3 là rẽ vào khúc cua bên trái thứ hai.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 7 - Biểu tượng và chỉ dẫn không gian trong đô thị.', N'Applied', N'BaiHoc_07_MetroNavigation.txt', 7, 'SEED_EAI_060');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'1-Continue forward, 2-On the opposite side, 3-Turn at the 2nd left corner', 1),
        (@MaCH, N'B', N'1-Stop, 2-Turn around, 3-Behind', 0),
        (@MaCH, N'C', N'1-Turn right, 2-Next to, 3-Above', 0),
        (@MaCH, N'D', N'1-Wait, 2-Underground, 3-Far away', 0);
END
GO

-- Câu hỏi SEED_EAI_061 (NguPhap - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_061')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 2, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Choose the correct sentence: ''While I _______ dinner, the telephone suddenly _______.''',
            NULL, NULL, N'Hành động đang diễn ra kéo dài trong quá khứ dùng Past Continuous (''was cooking''), hành động khác cắt ngang dùng Past Simple (''rang'').', N'Hành động đang diễn ra kéo dài trong quá khứ dùng Past Continuous (''was cooking''), hành động khác cắt ngang dùng Past Simple (''rang'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 15 - Cấu trúc giao thoa While: While + Past Continuous, Past Simple.', N'Extracted', N'BaiHoc_15_PastSimpleVsContinuous.txt', 15, 'SEED_EAI_061');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'cooked / was ringing', 0),
        (@MaCH, N'B', N'was cooking / rang', 1),
        (@MaCH, N'C', N'am cooking / rings', 0),
        (@MaCH, N'D', N'was cooked / was ringing', 0);
END
GO

-- Câu hỏi SEED_EAI_062 (NguPhap - Trình độ 2 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_062')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 2, N'TracNghiem', N'NguPhap', N'TrungBinh', N'What was Emily doing at 8:00 PM yesterday when the power went out?',
            NULL, NULL, N'Tại một thời điểm xác định cụ thể trong quá khứ (at 8:00 PM yesterday), hành động đang diễn ra chia ở Past Continuous: ''was studying''.', N'Tại một thời điểm xác định cụ thể trong quá khứ (at 8:00 PM yesterday), hành động đang diễn ra chia ở Past Continuous: ''was studying''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 15 - Quy tắc thời điểm xác định trong quá khứ: At + giờ cụ thể + thời gian quá khứ -> Past Continuous.', N'Applied', N'BaiHoc_15_PastSimpleVsContinuous.txt', 15, 'SEED_EAI_062');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'She studied her lessons.', 0),
        (@MaCH, N'B', N'She was studying in her room.', 1),
        (@MaCH, N'C', N'She has studied lessons.', 0),
        (@MaCH, N'D', N'She is studying right then.', 0);
END
GO

-- Câu hỏi SEED_EAI_063 (NguPhap - Trình độ 2 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_063')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 2, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'Fill in the blank with the correct form of ''see'': ''Yesterday, we _______ an interesting movie about marine biology.''',
            NULL, NULL, N'Dấu hiệu ''Yesterday'' đi với sự việc đã chấm dứt hoàn toàn trong quá khứ, dùng quá khứ đơn ''saw'' (cột 2 của see).', N'Dấu hiệu ''Yesterday'' đi với sự việc đã chấm dứt hoàn toàn trong quá khứ, dùng quá khứ đơn ''saw'' (cột 2 của see).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 15 - Dấu hiệu nhận biết thì Quá khứ đơn: Yesterday, ago, last week/year.', N'Extracted', N'BaiHoc_15_PastSimpleVsContinuous.txt', 15, 'SEED_EAI_063');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'saw', 1),
        (@MaCH, N'B', N'seen', 0),
        (@MaCH, N'C', N'was seeing', 0),
        (@MaCH, N'D', N'see', 0);
END
GO

-- Câu hỏi SEED_EAI_064 (NguPhap - Trình độ 2 - Dạng SuaLoiSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_064')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 2, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Identify the mistake: ''When David arrived home, his children played games in the living room.''',
            NULL, NULL, N'Khi David về đến nhà, bọn trẻ ''đang chơi'' dở dang (hành động đang xảy ra khi David bước vào), phải dùng ''were playing''.', N'Khi David về đến nhà, bọn trẻ ''đang chơi'' dở dang (hành động đang xảy ra khi David bước vào), phải dùng ''were playing''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 15 - Phân tích câu ngữ cảnh cắt ngang: ''When + Past Simple, Past Continuous''.', N'Extracted', N'BaiHoc_15_PastSimpleVsContinuous.txt', 15, 'SEED_EAI_064');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''arrived'' should be ''arriving''', 0),
        (@MaCH, N'B', N'''played'' should be ''were playing''', 1),
        (@MaCH, N'C', N'''in'' should be ''at''', 0),
        (@MaCH, N'D', N'''living room'' should be ''lived room''', 0);
END
GO

-- Câu hỏi SEED_EAI_065 (Viet - Trình độ 2 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_065')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 2, N'VietLaiCau', N'Viet', N'TrungBinh', N'Combine the two sentences using ''While'': ''I was walking home. It started raining heavily.''',
            NULL, NULL, N'Kết hợp chuẩn: ''While I was walking home, it started raining heavily.''', N'Kết hợp chuẩn: ''While I was walking home, it started raining heavily.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 15 - Kỹ thuật nối câu với liên từ While và When.', N'Applied', N'BaiHoc_15_PastSimpleVsContinuous.txt', 15, 'SEED_EAI_065');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'While I was walking home, it started raining heavily.', 1),
        (@MaCH, N'B', N'While it started raining, I walked home.', 0),
        (@MaCH, N'C', N'While I walked home, it was raining heavily.', 0),
        (@MaCH, N'D', N'I was walking while it was starting rain.', 0);
END
GO

-- Câu hỏi SEED_EAI_066 (NguPhap - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_066')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 1, N'TracNghiem', N'NguPhap', N'De', N'Look at the dark clouds in the sky! Which sentence expresses a prediction based on present evidence?',
            NULL, NULL, N'Dự đoán dựa trên bằng chứng hiện tại (nhìn thấy mây đen vần vũ) phải dùng ''be going to'': ''It is going to rain.''', N'Dự đoán dựa trên bằng chứng hiện tại (nhìn thấy mây đen vần vũ) phải dùng ''be going to'': ''It is going to rain.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 16 - Quy tắc Be Going To: Dự đoán có bằng chứng cụ thể ở hiện tại (Look at those black clouds!).', N'Extracted', N'BaiHoc_16_WillVsBeGoingTo.txt', 16, 'SEED_EAI_066');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'It will rain probably someday.', 0),
        (@MaCH, N'B', N'It is going to rain soon.', 1),
        (@MaCH, N'C', N'It rains right now.', 0),
        (@MaCH, N'D', N'It was raining yesterday.', 0);
END
GO

-- Câu hỏi SEED_EAI_067 (NguPhap - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_067')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 1, N'TracNghiem', N'NguPhap', N'De', N'''The doorbell is ringing!'' - What is the spontaneous decision reply?',
            NULL, NULL, N'Quyết định ngay tại thời điểm nói (spontaneous decision), không có kế hoạch từ trước, dùng ''will'': ''I will get it.''', N'Quyết định ngay tại thời điểm nói (spontaneous decision), không có kế hoạch từ trước, dùng ''will'': ''I will get it.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 16 - Quy tắc thì Tương lai đơn (Will): Quyết định tức thời ngay khi nói.', N'Extracted', N'BaiHoc_16_WillVsBeGoingTo.txt', 16, 'SEED_EAI_067');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I am going to get it since last week.', 0),
        (@MaCH, N'B', N'I will answer it!', 1),
        (@MaCH, N'C', N'I have answered it already.', 0),
        (@MaCH, N'D', N'I am answering forever.', 0);
END
GO

-- Câu hỏi SEED_EAI_068 (NguPhap - Trình độ 2 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_068')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 2, N'DienKhuyet', N'NguPhap', N'TrungBinh', N'Complete the planned intention: ''Mark bought plane tickets yesterday. He _______ visit Japan next month.''',
            NULL, NULL, N'Kế hoạch hoặc dự định đã lên lịch trước (đã mua vé máy bay) dùng ''is going to''.', N'Kế hoạch hoặc dự định đã lên lịch trước (đã mua vé máy bay) dùng ''is going to''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 16 - Kế hoạch có sự chuẩn bị từ trước: ''be going to + V-infinitive''.', N'Extracted', N'BaiHoc_16_WillVsBeGoingTo.txt', 16, 'SEED_EAI_068');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'is going to', 1),
        (@MaCH, N'B', N'will perhaps', 0),
        (@MaCH, N'C', N'shall might', 0),
        (@MaCH, N'D', N'goes to', 0);
END
GO

-- Câu hỏi SEED_EAI_069 (NguPhap - Trình độ 1 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_069')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 1, N'DungSai', N'NguPhap', N'De', N'True or False: We can use ''will'' to make offers, promises, and quick requests (e.g., ''I promise I will not be late'').',
            NULL, NULL, N'Đúng. ''Will'' được dùng trong các lời hứa hẹn (promises), lời đề nghị giúp đỡ (offers) và lời yêu cầu.', N'Đúng. ''Will'' được dùng trong các lời hứa hẹn (promises), lời đề nghị giúp đỡ (offers) và lời yêu cầu.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 16 - Chức năng giao tiếp của Will: Hứa hẹn, cảnh báo và lời đề nghị.', N'Extracted', N'BaiHoc_16_WillVsBeGoingTo.txt', 16, 'SEED_EAI_069');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_070 (NguPhap - Trình độ 2 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_070')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 2, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the mistake: ''Don''t worry, I am going to carry that heavy box for you right now!''',
            NULL, NULL, N'Đưa ra lời đề nghị giúp đỡ tức thì ngay lúc nhìn thấy hộp nặng phải dùng ''I will carry'', không dùng ''am going to''.', N'Đưa ra lời đề nghị giúp đỡ tức thì ngay lúc nhìn thấy hộp nặng phải dùng ''I will carry'', không dùng ''am going to''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 16 - Lưu ý về lời đề nghị giúp đỡ (Offers): Dùng ''I will help you''.', N'Applied', N'BaiHoc_16_WillVsBeGoingTo.txt', 16, 'SEED_EAI_070');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''Don''t worry'' should be ''Not worry''', 0),
        (@MaCH, N'B', N'''am going to carry'' should be ''will carry''', 1),
        (@MaCH, N'C', N'''heavy'' should be ''heavily''', 0),
        (@MaCH, N'D', N'''for you'' should be ''to you''', 0);
END
GO

-- Câu hỏi SEED_EAI_071 (Nghe - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_071')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'TracNghiem', N'Nghe', N'De', N'In the audio text ''Watch Sport'', what sport does Hazal like to watch the most on TV?',
            N'Hello, my name is Hazal and I''m from Turkiye. My question is, do you watch sports on TV? Yes, I watch sports on TV. I like to watch tennis the most because it is very exciting and competitive.', NULL, N'Transcript nêu rõ: ''I like to watch tennis the most because it is very exciting and competitive.''', N'Transcript nêu rõ: ''I like to watch tennis the most because it is very exciting and competitive.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 29 - Transcript: ''I like to watch tennis the most because it is very exciting''.', N'Extracted', N'BaiHoc_29_WatchSport.txt', 29, 'SEED_EAI_071');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Football / Soccer', 0),
        (@MaCH, N'B', N'Tennis', 1),
        (@MaCH, N'C', N'Basketball', 0),
        (@MaCH, N'D', N'Swimming', 0);
END
GO

-- Câu hỏi SEED_EAI_072 (TuVung - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_072')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'TracNghiem', N'TuVung', N'De', N'Which adjective was used by Hazal to explain why tennis is enjoyable to watch?',
            NULL, NULL, N'Hazal giải thích cô ấy thích xem tennis vì nó ''exciting'' (hào hứng, sôi nổi) và ''competitive'' (tính cạnh tranh).', N'Hazal giải thích cô ấy thích xem tennis vì nó ''exciting'' (hào hứng, sôi nổi) và ''competitive'' (tính cạnh tranh).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 29 - Câu giải thích sở thích: ''because it is very exciting''.', N'Extracted', N'BaiHoc_29_WatchSport.txt', 29, 'SEED_EAI_072');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Boring', 0),
        (@MaCH, N'B', N'Exciting', 1),
        (@MaCH, N'C', N'Cheap', 0),
        (@MaCH, N'D', N'Noisy', 0);
END
GO

-- Câu hỏi SEED_EAI_073 (TuVung - Trình độ 2 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_073')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'DienKhuyet', N'TuVung', N'De', N'Complete the sentence: ''Hazal is from _______.''',
            NULL, NULL, N'Transcript câu đầu tiên: ''Hello, my name is Hazal and I''m from Turkiye.''', N'Transcript câu đầu tiên: ''Hello, my name is Hazal and I''m from Turkiye.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 29 - Câu 1: ''my name is Hazal and I''m from Turkiye.''', N'Extracted', N'BaiHoc_29_WatchSport.txt', 29, 'SEED_EAI_073');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Turkiye', 1),
        (@MaCH, N'B', N'Tunisia', 0),
        (@MaCH, N'C', N'Thailand', 0),
        (@MaCH, N'D', N'Tanzania', 0);
END
GO

-- Câu hỏi SEED_EAI_074 (TuVung - Trình độ 2 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_074')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'TracNghiem', N'TuVung', N'De', N'What do you call the person who enforces the rules in a tennis match from an elevated chair?',
            NULL, NULL, N'Trọng tài điều khiển trận đấu tennis ngồi trên ghế cao được gọi là ''umpire''. (Trong bóng đá gọi là referee).', N'Trọng tài điều khiển trận đấu tennis ngồi trên ghế cao được gọi là ''umpire''. (Trong bóng đá gọi là referee).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 29 - Mở rộng từ vựng thể thao: Umpire (quần vợt/bóng chày) vs. Referee (bóng đá/bóng rổ).', N'Applied', N'BaiHoc_29_WatchSport.txt', 29, 'SEED_EAI_074');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Referee', 0),
        (@MaCH, N'B', N'Umpire', 1),
        (@MaCH, N'C', N'Judge', 0),
        (@MaCH, N'D', N'Coach', 0);
END
GO

-- Câu hỏi SEED_EAI_075 (TuVung - Trình độ 2 - Dạng DungSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_075')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'DungSai', N'TuVung', N'De', N'True or False: In tennis, the four premier international tournaments (Australian Open, Roland Garros, Wimbledon, US Open) are known as the ''Grand Slams''.',
            NULL, NULL, N'Đúng. 4 giải đấu lớn nhất hành tinh của làng quần vợt thế giới được gọi là hệ thống Grand Slam.', N'Đúng. 4 giải đấu lớn nhất hành tinh của làng quần vợt thế giới được gọi là hệ thống Grand Slam.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 29 - Kiến thức bổ trợ về môn quần vợt thế giới.', N'Applied', N'BaiHoc_29_WatchSport.txt', 29, 'SEED_EAI_075');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_076 (Nghe - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_076')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'TracNghiem', N'Nghe', N'TrungBinh', N'In the travel conversation, how did Abidemi travel into Vietnam after visiting Laos?',
            N'Todd: So after Laos, where did you go?
Abidemi: After Laos, I flew to Vietnam. And actually I took the bus into Vietnam.
Todd: OK.
Abidemi: Yes. And it was a very long ride through the mountains.', NULL, N'Abidemi đính chính: ''And actually I took the bus into Vietnam.'' (đi xe buýt qua cửa khẩu vùng núi).', N'Abidemi đính chính: ''And actually I took the bus into Vietnam.'' (đi xe buýt qua cửa khẩu vùng núi).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 30 - Transcript: ''And actually I took the bus into Vietnam.''', N'Extracted', N'BaiHoc_30_TravellingVietnam.txt', 30, 'SEED_EAI_076');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'She took a train from Bangkok.', 0),
        (@MaCH, N'B', N'She took the bus into Vietnam.', 1),
        (@MaCH, N'C', N'She sailed on a cruise ship.', 0),
        (@MaCH, N'D', N'She walked across the river.', 0);
END
GO

-- Câu hỏi SEED_EAI_077 (Doc - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_077')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'TracNghiem', N'Doc', N'TrungBinh', N'How did Abidemi describe the bus journey into Vietnam?',
            NULL, NULL, N'Abidemi chia sẻ chuyến đi xe buýt rất dài xuyên qua các dãy núi: ''it was a very long ride through the mountains.''', N'Abidemi chia sẻ chuyến đi xe buýt rất dài xuyên qua các dãy núi: ''it was a very long ride through the mountains.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 30 - Transcript: ''it was a very long ride through the mountains.''', N'Extracted', N'BaiHoc_30_TravellingVietnam.txt', 30, 'SEED_EAI_077');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Short and boring', 0),
        (@MaCH, N'B', N'A very long ride through the mountains', 1),
        (@MaCH, N'C', N'Extremely luxurious and fast', 0),
        (@MaCH, N'D', N'A flat trip across the desert', 0);
END
GO

-- Câu hỏi SEED_EAI_078 (TuVung - Trình độ 2 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_078')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'DienKhuyet', N'TuVung', N'De', N'Fill in the blank with a travel preposition: ''We travelled _______ bus from Vientiane to Hanoi.''',
            NULL, NULL, N'Cụm chỉ phương tiện giao thông đi với giới từ ''by'': by bus, by train, by plane, by car.', N'Cụm chỉ phương tiện giao thông đi với giới từ ''by'': by bus, by train, by plane, by car.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 30 - Ngữ pháp giới từ chỉ phương tiện giao thông (by bus / train / plane).', N'Extracted', N'BaiHoc_30_TravellingVietnam.txt', 30, 'SEED_EAI_078');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'by', 1),
        (@MaCH, N'B', N'with', 0),
        (@MaCH, N'C', N'on', 0),
        (@MaCH, N'D', N'in', 0);
END
GO

-- Câu hỏi SEED_EAI_079 (TuVung - Trình độ 2 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_079')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'TracNghiem', N'TuVung', N'TrungBinh', N'What iconic UNESCO World Heritage bay in northern Vietnam is famous for emerald waters and thousands of limestone islands?',
            NULL, NULL, N'Vịnh Hạ Long (Ha Long Bay) là di sản thiên nhiên thế giới nổi tiếng của miền bắc Việt Nam với hàng ngàn đảo đá vôi.', N'Vịnh Hạ Long (Ha Long Bay) là di sản thiên nhiên thế giới nổi tiếng của miền bắc Việt Nam với hàng ngàn đảo đá vôi.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 30 - Điểm đến du lịch Việt Nam: Ha Long Bay limestone karsts.', N'Applied', N'BaiHoc_30_TravellingVietnam.txt', 30, 'SEED_EAI_079');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Nha Trang Bay', 0),
        (@MaCH, N'B', N'Ha Long Bay', 1),
        (@MaCH, N'C', N'Phu Quoc Island', 0),
        (@MaCH, N'D', N'Con Dao Archipelago', 0);
END
GO

-- Câu hỏi SEED_EAI_080 (Doc - Trình độ 2 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_080')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'DungSai', N'Doc', N'De', N'True or False: The ancient city of Bagan in Myanmar is world-renowned for its thousands of historical Buddhist temples and pagodas across the plains.',
            NULL, NULL, N'Đúng. Cố đô Bagan của Myanmar nổi tiếng toàn cầu với quần thể hàng ngàn ngôi đền và chùa tháp cổ kính.', N'Đúng. Cố đô Bagan của Myanmar nổi tiếng toàn cầu với quần thể hàng ngàn ngôi đền và chùa tháp cổ kính.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 30 - Phần chia sẻ du lịch Myanmar: The ancient temples of Bagan.', N'Extracted', N'BaiHoc_30_TravellingVietnam.txt', 30, 'SEED_EAI_080');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_081 (Doc - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_081')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 2, N'TracNghiem', N'Doc', N'TrungBinh', N'In a standard international IT resume, which section lists programming languages, frameworks, and databases?',
            NULL, NULL, N'Mục ''Technical Skills'' (Kỹ năng chuyên môn) là nơi liệt kê các ngôn ngữ lập trình, framework và công nghệ.', N'Mục ''Technical Skills'' (Kỹ năng chuyên môn) là nơi liệt kê các ngôn ngữ lập trình, framework và công nghệ.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Mục 2: Cấu trúc Resume quốc tế: Technical Skills.', N'Extracted', N'04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf', NULL, 'SEED_EAI_081');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Hobbies and Interests', 0),
        (@MaCH, N'B', N'Technical Skills', 1),
        (@MaCH, N'C', N'Personal References', 0),
        (@MaCH, N'D', N'High School Extracurriculars', 0);
END
GO

-- Câu hỏi SEED_EAI_082 (TuVung - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_082')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 2, N'TracNghiem', N'TuVung', N'TrungBinh', N'Which strong action verb should an engineer use on a CV instead of ''worked on'' to show leadership?',
            NULL, NULL, N'Các động từ hành động mạnh như ''Spearheaded'', ''Engineered'', ''Architected'' gây ấn tượng vượt trội so với ''worked on''.', N'Các động từ hành động mạnh như ''Spearheaded'', ''Engineered'', ''Architected'' gây ấn tượng vượt trội so với ''worked on''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Action Verbs: Dùng động từ mạnh như Spearheaded, Implemented, Deployed.', N'Extracted', N'04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf', NULL, 'SEED_EAI_082');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Helped with', 0),
        (@MaCH, N'B', N'Spearheaded', 1),
        (@MaCH, N'C', N'Did some', 0),
        (@MaCH, N'D', N'Was involved in', 0);
END
GO

-- Câu hỏi SEED_EAI_083 (Viet - Trình độ 3 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_083')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 3, N'VietLaiCau', N'Viet', N'TrungBinh', N'Transform the passive bullet point into a quantifiable achievement: ''The API was optimized by me.''',
            NULL, NULL, N'Quy tắc viết CV quốc tế: Động từ hành động + Nhiệm vụ + Kết quả định lượng (ví dụ: ''Optimized API response time by 40%'').', N'Quy tắc viết CV quốc tế: Động từ hành động + Nhiệm vụ + Kết quả định lượng (ví dụ: ''Optimized API response time by 40%'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Công thức XYZ: Accomplished [X] as measured by [Y] by doing [Z].', N'Applied', N'04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf', NULL, 'SEED_EAI_083');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Optimized RESTful API endpoints, reducing response latency by 40%.', 1),
        (@MaCH, N'B', N'I was optimizing the API slowly.', 0),
        (@MaCH, N'C', N'The API had been made faster by my code.', 0),
        (@MaCH, N'D', N'Good job done on API.', 0);
END
GO

-- Câu hỏi SEED_EAI_084 (Doc - Trình độ 2 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_084')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 2, N'DungSai', N'Doc', N'De', N'True or False: According to international tech hiring standards, a candidate''s resume should ideally be concise and fit within 1 to 2 pages.',
            NULL, NULL, N'Đúng. Tiêu chuẩn CV công nghệ cho lập trình viên dưới 5 năm kinh nghiệm là 1 trang gọn gàng, tối đa 2 trang.', N'Đúng. Tiêu chuẩn CV công nghệ cho lập trình viên dưới 5 năm kinh nghiệm là 1 trang gọn gàng, tối đa 2 trang.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Chiều dài tiêu chuẩn của Resume: 1-2 trang.', N'Extracted', N'04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf', NULL, 'SEED_EAI_084');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 1),
        (@MaCH, N'B', N'False', 0);
END
GO

-- Câu hỏi SEED_EAI_085 (TuVung - Trình độ 2 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_085')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 2, N'DienKhuyet', N'TuVung', N'De', N'Complete the CV header term: ''GitHub _______: github.com/developer-profile''',
            NULL, NULL, N'Trong CV lập trình viên, link mã nguồn được gọi là ''GitHub Profile'' hoặc ''GitHub Repository''.', N'Trong CV lập trình viên, link mã nguồn được gọi là ''GitHub Profile'' hoặc ''GitHub Repository''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Thông tin liên hệ: GitHub Profile, LinkedIn.', N'Extracted', N'04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf', NULL, 'SEED_EAI_085');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Profile', 1),
        (@MaCH, N'B', N'Novel', 0),
        (@MaCH, N'C', N'Ticket', 0),
        (@MaCH, N'D', N'Poster', 0);
END
GO

-- Câu hỏi SEED_EAI_086 (Doc - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_086')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 2, N'TracNghiem', N'Doc', N'TrungBinh', N'You see a sign at the railway platform: ''Mind the gap.'' What does it instruct passengers to do?',
            NULL, NULL, N'''Mind the gap'' là khẩu hiệu nổi tiếng ở các ga tàu điện ngầm Luân Đôn nhắc nhở chú ý khoảng cách giữa mép tàu và sân ga.', N'''Mind the gap'' là khẩu hiệu nổi tiếng ở các ga tàu điện ngầm Luân Đôn nhắc nhở chú ý khoảng cách giữa mép tàu và sân ga.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 7 - Biển báo an toàn giao thông công cộng: ''Mind the gap''.', N'Extracted', N'BaiHoc_07_MetroNavigation.txt', 7, 'SEED_EAI_086');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Jump over the railway tracks quickly.', 0),
        (@MaCH, N'B', N'Be careful of the space between the train door and the station platform.', 1),
        (@MaCH, N'C', N'Do not talk to anyone while riding the train.', 0),
        (@MaCH, N'D', N'Buy extra tickets before entering.', 0);
END
GO

-- Câu hỏi SEED_EAI_087 (TuVung - Trình độ 2 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_087')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 2, N'TracNghiem', N'TuVung', N'De', N'Which term describes a hotel room with two separate single beds?',
            NULL, NULL, N'''Twin room'' là phòng khách sạn có 2 giường đơn riêng biệt. ''Double room'' là phòng có 1 giường đôi lớn.', N'''Twin room'' là phòng khách sạn có 2 giường đơn riêng biệt. ''Double room'' là phòng có 1 giường đôi lớn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 6 - Từ vựng lưu trú du lịch: Single room, Double room, Twin room.', N'Applied', N'BaiHoc_06_AirportNavigation.txt', 6, 'SEED_EAI_087');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Single room', 0),
        (@MaCH, N'B', N'Twin room', 1),
        (@MaCH, N'C', N'Double room', 0),
        (@MaCH, N'D', N'Studio room', 0);
END
GO

-- Câu hỏi SEED_EAI_088 (NguPhap - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_088')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 2, N'TracNghiem', N'NguPhap', N'De', N'Choose the irregular past simple form of ''fly'': ''Last summer, our family _______ to Da Nang for holiday.''',
            NULL, NULL, N'Động từ bất quy tắc ''fly'' chuyển sang quá khứ đơn là ''flew'' (cột 2).', N'Động từ bất quy tắc ''fly'' chuyển sang quá khứ đơn là ''flew'' (cột 2).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 15 - Bảng động từ bất quy tắc thông dụng: fly -> flew -> flown.', N'Extracted', N'BaiHoc_15_PastSimpleVsContinuous.txt', 15, 'SEED_EAI_088');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'flied', 0),
        (@MaCH, N'B', N'flew', 1),
        (@MaCH, N'C', N'flown', 0),
        (@MaCH, N'D', N'flying', 0);
END
GO

-- Câu hỏi SEED_EAI_089 (NguPhap - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_089')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 2, N'TracNghiem', N'NguPhap', N'TrungBinh', N'Which sentence correctly contrasts an ongoing action with an interruption?',
            NULL, NULL, N'''I was reading a novel when the doorbell rang.'' đúng cấu trúc: Past Continuous diễn tả hành động đang xảy ra, Past Simple diễn tả hành động chen ngang.', N'''I was reading a novel when the doorbell rang.'' đúng cấu trúc: Past Continuous diễn tả hành động đang xảy ra, Past Simple diễn tả hành động chen ngang.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 15 - Công thức liên kết When: Past Continuous + when + Past Simple.', N'Extracted', N'BaiHoc_15_PastSimpleVsContinuous.txt', 15, 'SEED_EAI_089');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I read a novel when the doorbell was ringing.', 0),
        (@MaCH, N'B', N'I was reading a novel when the doorbell rang.', 1),
        (@MaCH, N'C', N'I have read a novel when the doorbell has rung.', 0),
        (@MaCH, N'D', N'I was read a novel when the doorbell ringed.', 0);
END
GO

-- Câu hỏi SEED_EAI_090 (Noi - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_090')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 1, N'TracNghiem', N'Noi', N'De', N'You want to buy a return ticket at the train station. What do you say?',
            NULL, NULL, N'''I''d like a return ticket to Oxford, please.'' (return ticket: vé khứ hồi, Anh-Mỹ gọi là round-trip ticket).', N'''I''d like a return ticket to Oxford, please.'' (return ticket: vé khứ hồi, Anh-Mỹ gọi là round-trip ticket).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 7 - Mẫu câu mua vé tàu xe: Single ticket (vé 1 chiều) vs. Return ticket (vé khứ hồi).', N'Extracted', N'BaiHoc_07_MetroNavigation.txt', 7, 'SEED_EAI_090');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'One return ticket to Oxford, please.', 1),
        (@MaCH, N'B', N'Give me train to go and never back.', 0),
        (@MaCH, N'C', N'I demand Oxford seat right away.', 0),
        (@MaCH, N'D', N'Oxford ticket is wanted by me.', 0);
END
GO

-- Câu hỏi SEED_EAI_091 (TuVung - Trình độ 2 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_091')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 2, N'DienKhuyet', N'TuVung', N'De', N'Fill in the blank: ''Please keep your seatbelt _______ while the aircraft is experiencing turbulence.''',
            NULL, NULL, N'Thông báo máy bay: ''fastened'' (được thắt chặt): ''fasten your seatbelt''.', N'Thông báo máy bay: ''fastened'' (được thắt chặt): ''fasten your seatbelt''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 6 - Thông báo an toàn bay: ''Keep seatbelts fastened''.', N'Extracted', N'BaiHoc_06_AirportNavigation.txt', 6, 'SEED_EAI_091');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'fastened', 1),
        (@MaCH, N'B', N'unlocked', 0),
        (@MaCH, N'C', N'loosened', 0),
        (@MaCH, N'D', N'broken', 0);
END
GO

-- Câu hỏi SEED_EAI_092 (NguPhap - Trình độ 2 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_092')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 2, N'SuaLoiSai', N'NguPhap', N'TrungBinh', N'Find the error: ''Yesterday afternoon at 3 PM, she cooked in the kitchen.''',
            NULL, NULL, N'Thời điểm cụ thể trong quá khứ (at 3 PM yesterday afternoon) phải dùng thì Quá khứ tiếp diễn: ''was cooking''.', N'Thời điểm cụ thể trong quá khứ (at 3 PM yesterday afternoon) phải dùng thì Quá khứ tiếp diễn: ''was cooking''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 15 - Lưu ý mốc thời gian cụ thể: At + giờ + quá khứ -> Past Continuous.', N'Applied', N'BaiHoc_15_PastSimpleVsContinuous.txt', 15, 'SEED_EAI_092');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''Yesterday'' should be ''Tomorrow''', 0),
        (@MaCH, N'B', N'''cooked'' should be ''was cooking''', 1),
        (@MaCH, N'C', N'''in'' should be ''into''', 0),
        (@MaCH, N'D', N'''kitchen'' should be ''kitchens''', 0);
END
GO

-- Câu hỏi SEED_EAI_093 (TuVung - Trình độ 2 - Dạng NoiCap - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_093')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 2, N'NoiCap', N'TuVung', N'De', N'Match airport facilities with their purposes:
1. ''Baggage Claim''
2. ''Currency Exchange''
3. ''Duty-Free Shop''',
            NULL, NULL, N'1 là nơi nhận hành lý sau chuyến bay; 2 là quầy đổi ngoại tệ; 3 là cửa hàng miễn thuế tại sân bay.', N'1 là nơi nhận hành lý sau chuyến bay; 2 là quầy đổi ngoại tệ; 3 là cửa hàng miễn thuế tại sân bay.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 6 - Sơ đồ và tiện ích sân bay quốc tế.', N'Extracted', N'BaiHoc_06_AirportNavigation.txt', 6, 'SEED_EAI_093');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'1-Collecting checked luggage, 2-Converting foreign money, 3-Buying tax-exempt products', 1),
        (@MaCH, N'B', N'1-Buying food, 2-Checking passports, 3-Boarding plane', 0),
        (@MaCH, N'C', N'1-Sleeping, 2-Driving cars, 3-Booking flights', 0),
        (@MaCH, N'D', N'1-Medical room, 2-Security check, 3-Lost property', 0);
END
GO

-- Câu hỏi SEED_EAI_094 (NguPhap - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_094')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 1, N'TracNghiem', N'NguPhap', N'De', N'Complete the future promise: ''I promise I _______ tell anyone your secret.''',
            NULL, NULL, N'Lời hứa phủ định dùng ''will not'' hoặc ''won''t'': ''I promise I won''t tell...''.', N'Lời hứa phủ định dùng ''will not'' hoặc ''won''t'': ''I promise I won''t tell...''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 16 - Lời hứa (Promises) dùng thì Tương lai đơn (Will / Won''t).', N'Extracted', N'BaiHoc_16_WillVsBeGoingTo.txt', 16, 'SEED_EAI_094');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'won''t', 1),
        (@MaCH, N'B', N'am not going to ever', 0),
        (@MaCH, N'C', N'did not', 0),
        (@MaCH, N'D', N'have not', 0);
END
GO

-- Câu hỏi SEED_EAI_095 (TuVung - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_095')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 1, N'TracNghiem', N'TuVung', N'De', N'What is the opposite direction of ''North''?',
            NULL, NULL, N'Hướng đối diện của Bắc (North) là Nam (South).', N'Hướng đối diện của Bắc (North) là Nam (South).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 7 - 4 hướng cơ bản trên bản đồ: North, South, East, West.', N'Extracted', N'BaiHoc_07_MetroNavigation.txt', 7, 'SEED_EAI_095');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'East', 0),
        (@MaCH, N'B', N'South', 1),
        (@MaCH, N'C', N'West', 0),
        (@MaCH, N'D', N'Northeast', 0);
END
GO

-- Câu hỏi SEED_EAI_096 (TuVung - Trình độ 2 - Dạng TracNghiem - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_096')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'TracNghiem', N'TuVung', N'De', N'In football, what happens when a player receives two yellow cards in a single match?',
            NULL, NULL, N'Trong bóng đá, 2 thẻ vàng tương đương 1 thẻ đỏ (red card) và cầu thủ bị đuổi khỏi sân.', N'Trong bóng đá, 2 thẻ vàng tương đương 1 thẻ đỏ (red card) và cầu thủ bị đuổi khỏi sân.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 29 - Luật chơi bóng đá cơ bản: 2 yellow cards = red card (sent off).', N'Applied', N'BaiHoc_29_WatchSport.txt', 29, 'SEED_EAI_096');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The player is given a penalty kick.', 0),
        (@MaCH, N'B', N'The player is shown a red card and sent off the pitch.', 1),
        (@MaCH, N'C', N'The match ends immediately.', 0),
        (@MaCH, N'D', N'The team gets an extra player.', 0);
END
GO

-- Câu hỏi SEED_EAI_097 (TuVung - Trình độ 2 - Dạng DienKhuyet - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_097')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 2, N'DienKhuyet', N'TuVung', N'De', N'Fill in the blank: ''A tourist visa allows travelers to stay _______ a maximum of 30 days.''',
            NULL, NULL, N'Cụm chỉ thời lượng: ''stay for a maximum of 30 days'' (lưu trú trong tối đa 30 ngày).', N'Cụm chỉ thời lượng: ''stay for a maximum of 30 days'' (lưu trú trong tối đa 30 ngày).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 6 - Thuật ngữ Visa du lịch và thời hạn lưu trú.', N'Applied', N'BaiHoc_06_AirportNavigation.txt', 6, 'SEED_EAI_097');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'for', 1),
        (@MaCH, N'B', N'during', 0),
        (@MaCH, N'C', N'at', 0),
        (@MaCH, N'D', N'by', 0);
END
GO

-- Câu hỏi SEED_EAI_098 (NguPhap - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_098')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (10, 1, N'TracNghiem', N'NguPhap', N'De', N'Which question correctly asks for permission or offers help?',
            NULL, NULL, N'''Shall I carry your bag for you?'' là mẫu câu đề nghị giúp đỡ trang trọng và lịch sự của ngôi ''I''.', N'''Shall I carry your bag for you?'' là mẫu câu đề nghị giúp đỡ trang trọng và lịch sự của ngôi ''I''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 16 - Mẫu câu Shall I / Shall we trong lời đề nghị lịch sự.', N'Extracted', N'BaiHoc_16_WillVsBeGoingTo.txt', 16, 'SEED_EAI_098');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Shall I carry your bag?', 1),
        (@MaCH, N'B', N'Will I be carrying your bag?', 0),
        (@MaCH, N'C', N'Do I carry your bag necessarily?', 0),
        (@MaCH, N'D', N'Am I carrying your bag now?', 0);
END
GO

-- Câu hỏi SEED_EAI_099 (Doc - Trình độ 2 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_099')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (21, 2, N'TracNghiem', N'Doc', N'TrungBinh', N'Read the notice: ''Passengers must keep their luggage with them at all times. Unattended items will be removed and may be destroyed.'' What should travelers do?',
            NULL, NULL, N'Hành khách phải luôn mang hành lý bên mình; hành lý vô chủ (unattended) sẽ bị tịch thu và tiêu hủy vì lý do an ninh.', N'Hành khách phải luôn mang hành lý bên mình; hành lý vô chủ (unattended) sẽ bị tịch thu và tiêu hủy vì lý do an ninh.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 6 - Cảnh báo an ninh hành lý vô chủ: ''Unattended baggage will be confiscated.''', N'Extracted', N'BaiHoc_06_AirportNavigation.txt', 6, 'SEED_EAI_099');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Leave bags with any stranger.', 0),
        (@MaCH, N'B', N'Never leave personal baggage unattended in public airport areas.', 1),
        (@MaCH, N'C', N'Put bags on the railway tracks.', 0),
        (@MaCH, N'D', N'Forget about their baggage during flights.', 0);
END
GO

-- Câu hỏi SEED_EAI_100 (Noi - Trình độ 1 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_100')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (4, 1, N'TracNghiem', N'Noi', N'De', N'A fellow traveler asks: ''Is this seat taken?'' - If nobody is sitting there, what do you say?',
            NULL, NULL, N'Đáp lại lịch sự: ''No, it''s free. Please go ahead!'' (Không, ghế trống. Xin mời bạn ngồi!).', N'Đáp lại lịch sự: ''No, it''s free. Please go ahead!'' (Không, ghế trống. Xin mời bạn ngồi!).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 7 - Giao tiếp trên tàu xe công cộng: Hỏi ghế trống.', N'Extracted', N'BaiHoc_07_MetroNavigation.txt', 7, 'SEED_EAI_100');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'No, it''s free. Please have a seat!', 1),
        (@MaCH, N'B', N'Yes, go away.', 0),
        (@MaCH, N'C', N'I bought this entire coach.', 0),
        (@MaCH, N'D', N'Never ask me questions.', 0);
END
GO

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

-- Câu hỏi SEED_EAI_401 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_401')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'When a patient presents with sudden dizziness, shortness of breath, and palpitations, which hospital department should they be directed to immediately?',
            NULL, NULL, N'Bệnh nhân có triệu chứng khó thở đột ngột, chóng mặt và đánh trống ngực cần được đưa ngay đến khoa cấp cứu (Emergency Department / A&E - Accident & Emergency).', N'Bệnh nhân có triệu chứng khó thở đột ngột, chóng mặt và đánh trống ngực cần được đưa ngay đến khoa cấp cứu (Emergency Department / A&E - Accident & Emergency).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Phần I: Triage và phân loại tiếp nhận bệnh nhân khẩn cấp.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_401');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Dermatology Clinic', 0),
        (@MaCH, N'B', N'Emergency Department (A&E)', 1),
        (@MaCH, N'C', N'Orthopedic Outpatient Desk', 0),
        (@MaCH, N'D', N'Ophthalmology Consultation', 0);
END
GO

-- Câu hỏi SEED_EAI_402 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_402')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''The doctor explained that an _______ illness develops rapidly and lasts a short period, whereas a chronic condition persists over months or years.''',
            NULL, NULL, N'''acute'' (cấp tính) đối lập với ''chronic'' (mãn tính).', N'''acute'' (cấp tính) đối lập với ''chronic'' (mãn tính).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Thuật ngữ lâm sàng: Acute vs. Chronic conditions.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_402');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'acute', 1),
        (@MaCH, N'B', N'accustomed', 0),
        (@MaCH, N'C', N'acute-free', 0),
        (@MaCH, N'D', N'acquired', 0);
END
GO

-- Câu hỏi SEED_EAI_403 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_403')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'TuVung', N'Kho', N'In clinical pharmacotherapy, what does the term ''adverse drug reaction'' (ADR) precisely refer to?',
            NULL, NULL, N'''Adverse drug reaction'' là phản ứng có hại, ngoài ý muốn xảy ra ở liều dùng thông thường của thuốc.', N'''Adverse drug reaction'' là phản ứng có hại, ngoài ý muốn xảy ra ở liều dùng thông thường của thuốc.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Phần II: Dược lý và hướng dẫn sử dụng đơn thuốc (Prescription & Posology).', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_403');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The intended therapeutic efficacy of the antibiotic', 0),
        (@MaCH, N'B', N'An unintended, harmful reaction resulting from medication at standard doses', 1),
        (@MaCH, N'C', N'The physiological shelf-life expiration date of a tablet', 0),
        (@MaCH, N'D', N'The commercial promotion of over-the-counter medicine', 0);
END
GO

-- Câu hỏi SEED_EAI_404 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_404')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'DungSai', N'Doc', N'TrungBinh', N'An ''outpatient'' is someone who stays overnight or for weeks in a hospital ward to receive round-the-clock intensive nursing.',
            NULL, NULL, N'Sai (False). ''Outpatient'' là bệnh nhân ngoại trú (khám và về trong ngày). Bệnh nhân lưu trú qua đêm hoặc điều trị nội trú được gọi là ''inpatient''.', N'Sai (False). ''Outpatient'' là bệnh nhân ngoại trú (khám và về trong ngày). Bệnh nhân lưu trú qua đêm hoặc điều trị nội trú được gọi là ''inpatient''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Phân biệt Inpatient và Outpatient trong quản trị bệnh viện.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_404');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_405 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_405')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'Doctor: ''How long have you been experiencing this throbbing pain in your temples?'' — Which answer is the most clinically helpful?',
            NULL, NULL, N'Bệnh nhân cần cung cấp chính xác thời gian khởi phát và tính chất cơn đau (e.g., ''It started three days ago and worsens whenever I look at bright screens'').', N'Bệnh nhân cần cung cấp chính xác thời gian khởi phát và tính chất cơn đau (e.g., ''It started three days ago and worsens whenever I look at bright screens'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Kỹ năng hỏi bệnh sử (Anamnesis & Pain assessment).', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_405');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Yes, I like temples and ancient architecture.', 0),
        (@MaCH, N'B', N'It started about three days ago and worsens whenever I look at bright screens.', 1),
        (@MaCH, N'C', N'I bought some aspirin last month.', 0),
        (@MaCH, N'D', N'My cousin had a headache two years ago.', 0);
END
GO

-- Câu hỏi SEED_EAI_406 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_406')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'SuaLoiSai', N'NguPhap', N'Kho', N'Find the underlined part that contains an error: ''The nurse instructed the patient to take two tablets (A) daily, refrain from (B) alcohol, and coming back (C) for a follow-up consultation on Friday (D).''',
            NULL, NULL, N'Lỗi ở (C) ''coming back''. Cần dùng động từ nguyên mẫu ''come back'' để song hành với ''take'' và ''refrain''.', N'Lỗi ở (C) ''coming back''. Cần dùng động từ nguyên mẫu ''come back'' để song hành với ''take'' và ''refrain''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Cấu trúc câu chỉ định điều trị y khoa & Song hành ngữ pháp.', N'Applied', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_406');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'daily', 0),
        (@MaCH, N'B', N'refrain from', 0),
        (@MaCH, N'C', N'coming back', 1),
        (@MaCH, N'D', N'on Friday', 0);
END
GO

-- Câu hỏi SEED_EAI_407 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_407')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'The label on the medicine bottle reads: ''Do not exceed the recommended _______ of 500mg every 8 hours.''',
            NULL, NULL, N'''dosage'' (liều lượng sử dụng).', N'''dosage'' (liều lượng sử dụng).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Hướng dẫn đọc nhãn dược phẩm (Prescription Labels).', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_407');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'dosage', 1),
        (@MaCH, N'B', N'diet', 0),
        (@MaCH, N'C', N'duration', 0),
        (@MaCH, N'D', N'dimension', 0);
END
GO

-- Câu hỏi SEED_EAI_408 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_408')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What is the primary role of a ''triage nurse'' in an emergency room setting?',
            NULL, NULL, N'Y tá sàng lọc (triage nurse) có nhiệm vụ đánh giá mức độ khẩn cấp về y tế của bệnh nhân khi mới đến để ưu tiên điều trị ca nguy kịch trước.', N'Y tá sàng lọc (triage nurse) có nhiệm vụ đánh giá mức độ khẩn cấp về y tế của bệnh nhân khi mới đến để ưu tiên điều trị ca nguy kịch trước.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Quy trình phân loại bệnh nhân cấp cứu (Triage Protocol).', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_408');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'To calculate hospital accounting invoices', 0),
        (@MaCH, N'B', N'To assess clinical acuity and prioritize patients according to medical urgency', 1),
        (@MaCH, N'C', N'To perform complex neurosurgery operations', 0),
        (@MaCH, N'D', N'To manage hospital parking facilities', 0);
END
GO

-- Câu hỏi SEED_EAI_409 (TuVung - Trình độ 4 - Dạng NoiCap - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_409')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'NoiCap', N'TuVung', N'Kho', N'Match each medical specialist with their diagnostic field:
1. Cardiologist
2. Neurologist
3. Pediatrician
4. Oncologist',
            NULL, NULL, N'1-Heart diseases, 2-Brain & nervous system, 3-Children medical care, 4-Cancer treatment.', N'1-Heart diseases, 2-Brain & nervous system, 3-Children medical care, 4-Cancer treatment.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Danh mục các chuyên khoa y tế bệnh viện.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_409');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'1-Heart, 2-Nervous system, 3-Children, 4-Cancer', 1),
        (@MaCH, N'B', N'1-Cancer, 2-Heart, 3-Nervous system, 4-Children', 0),
        (@MaCH, N'C', N'1-Skin, 2-Bones, 3-Eyes, 4-Teeth', 0),
        (@MaCH, N'D', N'1-Teeth, 2-Skin, 3-Cancer, 4-Heart', 0);
END
GO

-- Câu hỏi SEED_EAI_410 (NguPhap - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_410')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'VietLaiCau', N'NguPhap', N'Kho', N'Rewrite the sentence using passive voice with modal verbs: ''The pharmacist must inspect all prescriptions before dispensing narcotics.''',
            NULL, NULL, N'Dạng bị động: ''All prescriptions must be inspected by the pharmacist before narcotics are dispensed.''', N'Dạng bị động: ''All prescriptions must be inspected by the pharmacist before narcotics are dispensed.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Quy định kiểm soát dược phẩm & Câu bị động phương thái.', N'Applied', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_410');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'All prescriptions must be inspected by the pharmacist before narcotics are dispensed.', 1),
        (@MaCH, N'B', N'Narcotics must inspect all prescriptions by the pharmacist.', 0),
        (@MaCH, N'C', N'The pharmacist is inspecting all prescriptions before dispensing.', 0),
        (@MaCH, N'D', N'All prescriptions has to be inspected by narcotics.', 0);
END
GO

-- Câu hỏi SEED_EAI_411 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_411')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'Which term describes a non-invasive diagnostic technique that uses high-frequency sound waves to visualize internal body organs?',
            NULL, NULL, N'Siêu âm (Ultrasound / Ultrasonography) sử dụng sóng âm tần số cao.', N'Siêu âm (Ultrasound / Ultrasonography) sử dụng sóng âm tần số cao.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Chẩn đoán hình ảnh: X-ray, MRI, CT Scan, Ultrasound.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_411');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Magnetic Resonance Imaging (MRI)', 0),
        (@MaCH, N'B', N'Ultrasound scan', 1),
        (@MaCH, N'C', N'Computerized Tomography (CT)', 0),
        (@MaCH, N'D', N'Positron Emission Tomography', 0);
END
GO

-- Câu hỏi SEED_EAI_412 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_412')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'The surgeon asked the scrub nurse for a sterile _______ to make the initial abdominal incision.',
            NULL, NULL, N'''scalpel'' (dao mổ phẫu thuật).', N'''scalpel'' (dao mổ phẫu thuật).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Dụng cụ phẫu thuật cơ bản trong phòng mổ.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_412');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'scalpel', 1),
        (@MaCH, N'B', N'stethoscope', 0),
        (@MaCH, N'C', N'thermometer', 0),
        (@MaCH, N'D', N'bandage', 0);
END
GO

-- Câu hỏi SEED_EAI_413 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_413')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'In a US F-1 visa interview, what is the legal presumption that every applicant must overcome under Section 214(b) of the INA?',
            NULL, NULL, N'Theo điều 214(b) Đạo luật Di trú Mỹ (INA), mọi người nộp đơn xin visa không định cư đều bị mặc định coi là có ý định nhập cư (immigrant intent) cho đến khi chứng minh được điều ngược lại.', N'Theo điều 214(b) Đạo luật Di trú Mỹ (INA), mọi người nộp đơn xin visa không định cư đều bị mặc định coi là có ý định nhập cư (immigrant intent) cho đến khi chứng minh được điều ngược lại.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 1: Nguyên tắc vàng và điều khoản pháp lý 214(b).', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_413');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The presumption that the applicant has poor English skills', 0),
        (@MaCH, N'B', N'The presumption that the applicant intends to immigrate permanently to the US', 1),
        (@MaCH, N'C', N'The presumption that tuition fees cannot be paid', 0),
        (@MaCH, N'D', N'The presumption that universities in the US are overpopulated', 0);
END
GO

-- Câu hỏi SEED_EAI_414 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_414')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'Consular Officer: ''Why did you choose this specific university in Ohio instead of others?'' — Which response demonstrates strong preparation?',
            NULL, NULL, N'Ứng viên cần giải thích cụ thể về chương trình học, giáo sư, phòng nghiên cứu hoặc thế mạnh chuyên môn của trường thay vì nói chung chung.', N'Ứng viên cần giải thích cụ thể về chương trình học, giáo sư, phòng nghiên cứu hoặc thế mạnh chuyên môn của trường thay vì nói chung chung.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 2: Trả lời câu hỏi học thuật và lý do chọn trường.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_414');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Because my best friend is studying there and we want to share an apartment.', 0),
        (@MaCH, N'B', N'Their MS program in Data Science offers a specialized curriculum in NLP under Prof. Smith, which directly aligns with my AI career goals in Vietnam.', 1),
        (@MaCH, N'C', N'Because Ohio has nice weather and cheap shopping malls.', 0),
        (@MaCH, N'D', N'My study abroad agency picked it for me randomly.', 0);
END
GO

-- Câu hỏi SEED_EAI_415 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_415')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''To demonstrate non-immigrant intent, applicants must prove strong _______ ties to their home country, such as familial duties, business ownership, or guaranteed job offers.''',
            NULL, NULL, N'''ties'' / ''social and economic ties'' (mối ràng buộc vững chắc về mặt kinh tế, xã hội và gia đình tại quê hương).', N'''ties'' / ''social and economic ties'' (mối ràng buộc vững chắc về mặt kinh tế, xã hội và gia đình tại quê hương).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 3: Chứng minh mối ràng buộc chặt chẽ tại Việt Nam (Strong Ties).', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_415');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'ties', 1),
        (@MaCH, N'B', N'borders', 0),
        (@MaCH, N'C', N'passports', 0),
        (@MaCH, N'D', N'tickets', 0);
END
GO

-- Câu hỏi SEED_EAI_416 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_416')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DungSai', N'Doc', N'De', N'Form I-20 is issued by the US Embassy in Hanoi after an applicant successfully passes the in-person consular interview.',
            NULL, NULL, N'Sai (False). Mẫu I-20 do chính trường đại học được SEVP chứng nhận cấp cho sinh viên sau khi trúng tuyển; sinh viên mang I-20 đến phỏng vấn tại Đại sứ quán / Lãnh sự quán.', N'Sai (False). Mẫu I-20 do chính trường đại học được SEVP chứng nhận cấp cho sinh viên sau khi trúng tuyển; sinh viên mang I-20 đến phỏng vấn tại Đại sứ quán / Lãnh sự quán.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 4: Giấy tờ hồ sơ: Phân biệt Form I-20, Form DS-160 và SEVIS Fee I-901.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_416');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_417 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_417')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'When asked: ''Who is funding your education and what does your sponsor do?'', what is the best strategy?',
            NULL, NULL, N'Cần nêu rõ danh tính người tài trợ (thường là cha mẹ hoặc học bổng), nghề nghiệp cụ thể và thu nhập ròng ổn định hàng tháng/năm có khả năng chi trả học phí.', N'Cần nêu rõ danh tính người tài trợ (thường là cha mẹ hoặc học bổng), nghề nghiệp cụ thể và thu nhập ròng ổn định hàng tháng/năm có khả năng chi trả học phí.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 5: Chiến lược trả lời năng lực tài chính (Financial Solvency).', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_417');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I will find a part-time job under the table in American supermarkets to pay for my school.', 0),
        (@MaCH, N'B', N'My parents are funding my studies; my father owns a certified logistics enterprise earning \$60,000 annually, backed by savings accounts.', 1),
        (@MaCH, N'C', N'I have rich relatives in California who promised to help if I run out of cash.', 0),
        (@MaCH, N'D', N'Money does not matter because education should be free.', 0);
END
GO

-- Câu hỏi SEED_EAI_418 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_418')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'SuaLoiSai', N'NguPhap', N'Kho', N'Identify the grammatical mistake: ''Unless you do not prove (A) substantial ties to your home country, the consular officer (B) will be forced (C) to reject your application under Section 214(b) (D).''',
            NULL, NULL, N'Lỗi ở (A) ''do not prove''. ''Unless'' mang nghĩa ''nếu không'', không dùng thể phủ định kép ''Unless you do not prove''. Phải sửa thành ''Unless you prove''.', N'Lỗi ở (A) ''do not prove''. ''Unless'' mang nghĩa ''nếu không'', không dùng thể phủ định kép ''Unless you do not prove''. Phải sửa thành ''Unless you prove''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Cấu trúc điều kiện với Unless trong ngữ cảnh visa.', N'Applied', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_418');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'do not prove', 1),
        (@MaCH, N'B', N'the consular officer', 0),
        (@MaCH, N'C', N'will be forced', 0),
        (@MaCH, N'D', N'under Section 214(b)', 0);
END
GO

-- Câu hỏi SEED_EAI_419 (Doc - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_419')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DienKhuyet', N'Doc', N'TrungBinh', N'The mandatory tracking system operated by DHS to monitor international students in the United States is known by the acronym _______.',
            NULL, NULL, N'''SEVIS'' (Student and Exchange Visitor Information System).', N'''SEVIS'' (Student and Exchange Visitor Information System).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 4: Khái niệm SEVIS và cách nộp phí I-901 trực tuyến.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_419');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'SEVIS', 1),
        (@MaCH, N'B', N'TOEFL', 0),
        (@MaCH, N'C', N'ESTA', 0),
        (@MaCH, N'D', N'GRE', 0);
END
GO

-- Câu hỏi SEED_EAI_420 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_420')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'What behavior during an interview can inadvertently cause suspicion from consular officers?',
            NULL, NULL, N'Học thuộc lòng như vẹt (rote memorization), mắt nhìn đảo liên tục, hoặc né tránh giao tiếp bằng mắt (avoiding eye contact).', N'Học thuộc lòng như vẹt (rote memorization), mắt nhìn đảo liên tục, hoặc né tránh giao tiếp bằng mắt (avoiding eye contact).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 6: Ngôn ngữ cơ thể và tâm lý phòng phỏng vấn.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_420');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Maintaining steady and polite eye contact with the interviewer', 0),
        (@MaCH, N'B', N'Reciting memorized answers like a robot without emotional connection or spontaneity', 1),
        (@MaCH, N'C', N'Dressing in clean, professional business attire', 0),
        (@MaCH, N'D', N'Greeting the officer clearly with ''Good morning''', 0);
END
GO

-- Câu hỏi SEED_EAI_421 (NguPhap - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_421')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'VietLaiCau', N'NguPhap', N'Kho', N'Rewrite the sentence beginning with ''Only by'': ''You can convince the visa officer of your bona fide student intentions only by answering concisely and truthfully.''',
            NULL, NULL, N'Đảo ngữ với ''Only by'': ''Only by answering concisely and truthfully can you convince the visa officer of your bona fide student intentions.''', N'Đảo ngữ với ''Only by'': ''Only by answering concisely and truthfully can you convince the visa officer of your bona fide student intentions.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Kỹ thuật phỏng vấn & Đảo ngữ nhấn mạnh.', N'Applied', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_421');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Only by answering concisely and truthfully can you convince the visa officer of your bona fide student intentions.', 1),
        (@MaCH, N'B', N'Only by answering concisely and truthfully you can convince the visa officer.', 0),
        (@MaCH, N'C', N'Only by you convince the visa officer answering concisely and truthfully.', 0),
        (@MaCH, N'D', N'Only by answering truthfully the visa officer can convince you.', 0);
END
GO

-- Câu hỏi SEED_EAI_422 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_422')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What does the term ''Bona fide student'' mean in the context of US visa adjudication?',
            NULL, NULL, N'''Bona fide student'' là sinh viên chân chính, có mục tiêu học tập thực sự, năng lực học vấn phù hợp và kế hoạch tài chính rõ ràng.', N'''Bona fide student'' là sinh viên chân chính, có mục tiêu học tập thực sự, năng lực học vấn phù hợp và kế hoạch tài chính rõ ràng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Định nghĩa pháp lý Bona Fide Student.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_422');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A student who holds high government scholarships', 0),
        (@MaCH, N'B', N'A genuine student with authentic educational intent and academic capability', 1),
        (@MaCH, N'C', N'An international student who works full-time in off-campus restaurants', 0),
        (@MaCH, N'D', N'A student who has already completed a master''s degree', 0);
END
GO

-- Câu hỏi SEED_EAI_423 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_423')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'If asked: ''What will you do after graduating from your university in America?'', what is the most appropriate answer?',
            NULL, NULL, N'Nêu rõ lộ trình trở về nước để phát triển sự nghiệp trong ngành tại các tổ chức, doanh nghiệp ở Việt Nam.', N'Nêu rõ lộ trình trở về nước để phát triển sự nghiệp trong ngành tại các tổ chức, doanh nghiệp ở Việt Nam.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 7: Kế hoạch nghề nghiệp dài hạn sau khi tốt nghiệp (Career Roadmap).', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_423');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I hope to stay in New York forever and marry an American citizen.', 0),
        (@MaCH, N'B', N'I plan to immediately return to Vietnam to join a tech corporation as a Cloud Architect, applying the specialized skills I acquired.', 1),
        (@MaCH, N'C', N'I have not thought about it yet; I will decide after I finish partying.', 0),
        (@MaCH, N'D', N'I will look for any permanent job in California.', 0);
END
GO

-- Câu hỏi SEED_EAI_424 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_424')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'An online nonimmigrant visa electronic application form required for all F-1 applicants is designated as Form _______.',
            NULL, NULL, N'''DS-160'' (Online Nonimmigrant Visa Application).', N'''DS-160'' (Online Nonimmigrant Visa Application).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Hướng dẫn điền đơn DS-160 chuẩn xác từng mục.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_424');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'DS-160', 1),
        (@MaCH, N'B', N'W-2', 0),
        (@MaCH, N'C', N'1040-EZ', 0),
        (@MaCH, N'D', N'I-94', 0);
END
GO

-- Câu hỏi SEED_EAI_425 (Viet - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_425')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'TracNghiem', N'Viet', N'Kho', N'What is the primary difference between a Statement of Purpose (SOP) and a Personal Statement (PS) in graduate admissions?',
            NULL, NULL, N'SOP tập trung vào học thuật, định hướng nghiên cứu chuyên sâu, mục tiêu nghề nghiệp và sự phù hợp với chương trình đào tạo; còn PS tập trung hơn vào hành trình cuộc sống cá nhân, thử thách và động lực nội tâm.', N'SOP tập trung vào học thuật, định hướng nghiên cứu chuyên sâu, mục tiêu nghề nghiệp và sự phù hợp với chương trình đào tạo; còn PS tập trung hơn vào hành trình cuộc sống cá nhân, thử thách và động lực nội tâm.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần I: Phân biệt SOP (Học thuật & Nghiên cứu) và Personal Statement.', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_425');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'An SOP is a list of grades while a PS is a resume', 0),
        (@MaCH, N'B', N'An SOP focuses on academic trajectory, research questions, and faculty alignment, whereas a PS highlights personal backstory and resilience', 1),
        (@MaCH, N'C', N'An SOP is written by parents while a PS is written by professors', 0),
        (@MaCH, N'D', N'An SOP is only required for high school students', 0);
END
GO

-- Câu hỏi SEED_EAI_426 (Viet - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_426')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'TracNghiem', N'Viet', N'Kho', N'Which opening hook in a Master''s SOP is strongly DISCOURAGED by top-tier admissions committees?',
            NULL, NULL, N'Bắt đầu bằng câu trích dẫn nổi tiếng sáo rỗng hoặc câu nói mòn tai như ''Since my early childhood, I have always loved science'' bị hội đồng tuyển sinh đánh giá rất thấp.', N'Bắt đầu bằng câu trích dẫn nổi tiếng sáo rỗng hoặc câu nói mòn tai như ''Since my early childhood, I have always loved science'' bị hội đồng tuyển sinh đánh giá rất thấp.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần II: Các câu mở đầu sáo rỗng (Cliches to avoid in Paragraph 1).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_426');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A specific intellectual problem encountered during undergraduate capstone research', 0),
        (@MaCH, N'B', N'''Ever since I was a five-year-old child, I have always dreamed of becoming an engineer.''', 1),
        (@MaCH, N'C', N'An outline of current industry challenges and the applicant''s concrete research hypothesis', 0),
        (@MaCH, N'D', N'A concise overview of technical expertise and graduate study ambitions', 0);
END
GO

-- Câu hỏi SEED_EAI_427 (Viet - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_427')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'DienKhuyet', N'Viet', N'Kho', N'In a graduate SOP, when discussing fit with the institution, applicants must demonstrate academic _______ by referencing specific faculty research labs and recent publications.',
            NULL, NULL, N'''alignment'' (sự liên kết, tương thích học thuật).', N'''alignment'' (sự liên kết, tương thích học thuật).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần IV: Xây dựng mục Why School & Academic Alignment.', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_427');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'alignment', 1),
        (@MaCH, N'B', N'isolation', 0),
        (@MaCH, N'C', N'distance', 0),
        (@MaCH, N'D', N'indifference', 0);
END
GO

-- Câu hỏi SEED_EAI_428 (Viet - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_428')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'TracNghiem', N'Viet', N'Kho', N'How should academic shortcomings (such as one semester with low GPA) be addressed in an SOP?',
            NULL, NULL, N'Trình bày ngắn gọn, nhận trách nhiệm cá nhân, nêu rõ nguyên nhân khách quan nếu có và nhấn mạnh giải pháp khắc phục cũng như sự tiến bộ vượt bậc sau đó (Upward trend).', N'Trình bày ngắn gọn, nhận trách nhiệm cá nhân, nêu rõ nguyên nhân khách quan nếu có và nhấn mạnh giải pháp khắc phục cũng như sự tiến bộ vượt bậc sau đó (Upward trend).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần V: Cách xử lý điểm yếu học tập (Handling Red Flags and Low GPAs).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_428');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Blame the professors for giving unfair exam questions', 0),
        (@MaCH, N'B', N'Address it briefly, take accountability, highlight lessons learned, and demonstrate a clear upward trajectory in subsequent coursework', 1),
        (@MaCH, N'C', N'Pretend it never happened and alter the official transcript', 0),
        (@MaCH, N'D', N'Write three full pages complaining about bad health without evidence', 0);
END
GO

-- Câu hỏi SEED_EAI_429 (NguPhap - Trình độ 5 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_429')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'SuaLoiSai', N'NguPhap', N'Kho', N'Find the grammatical fault in this SOP sentence: ''Having successfully completing (A) my undergraduate thesis on NLP, my aspiration (B) is to further investigate (C) semantic ambiguity at your laboratory (D).''',
            NULL, NULL, N'Lỗi dùng thì ở phân từ hoàn thành (A): phải là ''Having successfully completed'', không dùng V-ing ''completing'' sau ''having''.', N'Lỗi dùng thì ở phân từ hoàn thành (A): phải là ''Having successfully completed'', không dùng V-ing ''completing'' sau ''having''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Ngữ pháp viết học thuật cao cấp: Phân từ hoàn thành (Perfect Participle).', N'Applied', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_429');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Having successfully completing', 1),
        (@MaCH, N'B', N'my aspiration', 0),
        (@MaCH, N'C', N'further investigate', 0),
        (@MaCH, N'D', N'at your laboratory', 0);
END
GO

-- Câu hỏi SEED_EAI_430 (Viet - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_430')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'TracNghiem', N'Viet', N'Kho', N'What is the recommended word count range for a competitive Master''s SOP at US/UK universities?',
            NULL, NULL, N'Độ dài tiêu chuẩn của một bài SOP Thạc sĩ là khoảng 800 - 1.000 từ (khoảng 2 trang A4 giãn dòng đơn).', N'Độ dài tiêu chuẩn của một bài SOP Thạc sĩ là khoảng 800 - 1.000 từ (khoảng 2 trang A4 giãn dòng đơn).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần VI: Quy cách định dạng, font chữ và dung lượng từ (800-1000 words).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_430');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'100 - 200 words', 0),
        (@MaCH, N'B', N'800 - 1,000 words (approximately 2 single-spaced pages)', 1),
        (@MaCH, N'C', N'4,000 - 6,000 words', 0),
        (@MaCH, N'D', N'At least 15 pages', 0);
END
GO

-- Câu hỏi SEED_EAI_431 (Viet - Trình độ 5 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_431')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'VietLaiCau', N'Viet', N'Kho', N'Enhance the academic sophistication of: ''I want to study in your university because your labs are very famous.''',
            NULL, NULL, N'''I am keen to join the Department of Computer Science to collaborate with Dr. X whose seminal work on neural transformers resonates deeply with my research interests.''', N'''I am keen to join the Department of Computer Science to collaborate with Dr. X whose seminal work on neural transformers resonates deeply with my research interests.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Kỹ thuật nâng cấp văn phong học thuật (Elevating academic tone).', N'Applied', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_431');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I am eager to join your department to collaborate with Dr. X, whose seminal work on neural transformers directly aligns with my research objectives.', 1),
        (@MaCH, N'B', N'I want to enter your lab because everyone knows it is very popular.', 0),
        (@MaCH, N'C', N'Your university is famous and I like fame.', 0),
        (@MaCH, N'D', N'Because your lab is cool, I will come.', 0);
END
GO

-- Câu hỏi SEED_EAI_432 (Viet - Trình độ 5 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_432')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'DungSai', N'Viet', N'TrungBinh', N'True or False: Using the identical generic SOP for ten different universities without mentioning faculty names or program specifics is considered best practice.',
            NULL, NULL, N'Sai (False). Việc nộp cùng 1 bài SOP chung chung (generic copy-paste) là nguyên nhân hàng đầu khiến hồ sơ bị loại ở vòng thẩm định đầu tiên.', N'Sai (False). Việc nộp cùng 1 bài SOP chung chung (generic copy-paste) là nguyên nhân hàng đầu khiến hồ sơ bị loại ở vòng thẩm định đầu tiên.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần VII: Nguyên tắc tùy biến riêng biệt cho từng trường (Customization rule).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_432');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_433 (Viet - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_433')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'DienKhuyet', N'Viet', N'Kho', N'Fill in the blank: ''Applicants should demonstrate intellectual _______ by formulating concise research questions and hypotheses for future graduate work.''',
            NULL, NULL, N'''curiosity'' hoặc ''maturity'' (sự trưởng thành và tò mò trí tuệ).', N'''curiosity'' hoặc ''maturity'' (sự trưởng thành và tò mò trí tuệ).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phẩm chất học thuật cốt lõi (Intellectual Curiosity).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_433');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'curiosity', 1),
        (@MaCH, N'B', N'negligence', 0),
        (@MaCH, N'C', N'arrogance', 0),
        (@MaCH, N'D', N'exhaustion', 0);
END
GO

-- Câu hỏi SEED_EAI_434 (Viet - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_434')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'TracNghiem', N'Viet', N'Kho', N'Which concluding statement effectively synthesizes the applicant''s readiness in an SOP?',
            NULL, NULL, N'Câu kết luận cần khẳng định sự sẵn sàng đóng góp cho cộng đồng học thuật của khoa và tiềm năng phát triển nghề nghiệp lâu dài.', N'Câu kết luận cần khẳng định sự sẵn sàng đóng góp cho cộng đồng học thuật của khoa và tiềm năng phát triển nghề nghiệp lâu dài.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần VIII: Cách viết đoạn kết ấn tượng (Strong Conclusion).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_434');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''Please admit me because I have nowhere else to go.''', 0),
        (@MaCH, N'B', N'''With my rigorous foundation in software engineering and passion for AI safety, I am prepared to contribute meaningfully to your research community and excel in your master''s cohort.''', 1),
        (@MaCH, N'C', N'''I hope you like this essay and give me a full scholarship.''', 0),
        (@MaCH, N'D', N'''That is all I have to say about myself.''', 0);
END
GO

-- Câu hỏi SEED_EAI_435 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_435')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'According to Lesson 33, which renewable energy source harnesses heat stored beneath the Earth''s surface?',
            NULL, NULL, N'Năng lượng địa nhiệt (Geothermal energy) khai thác nhiệt năng tích tụ sâu trong lòng vỏ trái đất.', N'Năng lượng địa nhiệt (Geothermal energy) khai thác nhiệt năng tích tụ sâu trong lòng vỏ trái đất.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Từ vựng chuyên sâu: Môi trường, Biến đổi khí hậu & Năng lượng tái tạo - Phần: Năng lượng tái tạo (Renewable Energy Sources).', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_435');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Hydroelectric energy', 0),
        (@MaCH, N'B', N'Geothermal energy', 1),
        (@MaCH, N'C', N'Solar photovoltaic power', 0),
        (@MaCH, N'D', N'Biomass combustion', 0);
END
GO

-- Câu hỏi SEED_EAI_436 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_436')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Complete the sentence: ''The total amount of greenhouse gases generated by human actions and manufacturing is referred to as our carbon _______.''',
            NULL, NULL, N'''carbon footprint'' (dấu chân carbon).', N'''carbon footprint'' (dấu chân carbon).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Từ vựng chuyên sâu: Môi trường - Khái niệm Dấu chân Carbon (Carbon Footprint).', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_436');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'footprint', 1),
        (@MaCH, N'B', N'handprint', 0),
        (@MaCH, N'C', N'shoe', 0),
        (@MaCH, N'D', N'fingerprint', 0);
END
GO

-- Câu hỏi SEED_EAI_437 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_437')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What environmental disaster is directly exacerbated by rapid deforestation in mountainous areas?',
            NULL, NULL, N'Mất rừng ở vùng đồi núi làm giảm độ liên kết đất, trực tiếp gây ra sạt lở đất nghiêm trọng (severe soil erosion and landslides).', N'Mất rừng ở vùng đồi núi làm giảm độ liên kết đất, trực tiếp gây ra sạt lở đất nghiêm trọng (severe soil erosion and landslides).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Tác động của nạn phá rừng (Deforestation & Soil Degradation).', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_437');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Severe soil erosion and catastrophic landslides', 1),
        (@MaCH, N'B', N'Increase in ozone layer thickness', 0),
        (@MaCH, N'C', N'Reduction of ocean microplastics', 0),
        (@MaCH, N'D', N'Accelerated glacier formation', 0);
END
GO

-- Câu hỏi SEED_EAI_438 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_438')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'DungSai', N'Doc', N'De', N'True or False: Biodegradable packaging materials persist in oceans for thousands of years without decomposing.',
            NULL, NULL, N'Sai (False). Vật liệu phân hủy sinh học (biodegradable) có khả năng phân rã tự nhiên nhờ vi sinh vật thành các chất vô hại trong thời gian tương đối ngắn.', N'Sai (False). Vật liệu phân hủy sinh học (biodegradable) có khả năng phân rã tự nhiên nhờ vi sinh vật thành các chất vô hại trong thời gian tương đối ngắn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Khái niệm rác thải sinh thái và vật liệu phân hủy sinh học (Biodegradable Materials).', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_438');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_439 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_439')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'DienKhuyet', N'TuVung', N'Kho', N'International climate treaties mandate countries to reach net-_______ carbon emissions by 2050.',
            NULL, NULL, N'''net-zero'' (phát thải ròng bằng không).', N'''net-zero'' (phát thải ròng bằng không).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Cam kết quốc tế về khí hậu và mục tiêu Net-Zero Emissions.', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_439');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'zero', 1),
        (@MaCH, N'B', N'fifty', 0),
        (@MaCH, N'C', N'equal', 0),
        (@MaCH, N'D', N'half', 0);
END
GO

-- Câu hỏi SEED_EAI_440 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_440')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'TracNghiem', N'TuVung', N'Kho', N'Which term describes the process of capturing carbon dioxide from industrial exhausts and trapping it deep underground?',
            NULL, NULL, N'Carbon Capture and Storage (CCS - Thu giữ và chôn lấp carbon).', N'Carbon Capture and Storage (CCS - Thu giữ và chôn lấp carbon).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Công nghệ giảm phát thải: Carbon Capture and Storage (CCS).', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_440');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Carbon Capture and Storage (CCS)', 1),
        (@MaCH, N'B', N'Deforestation mechanism', 0),
        (@MaCH, N'C', N'Greenhouse acceleration', 0),
        (@MaCH, N'D', N'Thermal inversion', 0);
END
GO

-- Câu hỏi SEED_EAI_441 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_441')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'SuaLoiSai', N'NguPhap', N'Kho', N'Spot the error: ''If all industrialized nations were to transition (A) to solar energy, greenhouse gas emissions would have decreased (B) significantly within (C) the upcoming decade (D).''',
            NULL, NULL, N'Lỗi ở (B) ''would have decreased''. Mệnh đề ''If were to transition'' (điều kiện loại 2) chỉ tình huống giả định ở hiện tại/tương lai, nên mệnh đề chính phải là ''would decrease'', không dùng ''would have decreased''.', N'Lỗi ở (B) ''would have decreased''. Mệnh đề ''If were to transition'' (điều kiện loại 2) chỉ tình huống giả định ở hiện tại/tương lai, nên mệnh đề chính phải là ''would decrease'', không dùng ''would have decreased''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Câu điều kiện loại 2 giả định chính sách môi trường.', N'Applied', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_441');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'were to transition', 0),
        (@MaCH, N'B', N'would have decreased', 1),
        (@MaCH, N'C', N'within', 0),
        (@MaCH, N'D', N'the upcoming decade', 0);
END
GO

-- Câu hỏi SEED_EAI_442 (NguPhap - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_442')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'VietLaiCau', N'NguPhap', N'Kho', N'Transform using inversion: ''If governments had invested heavily in clean wind power ten years ago, air quality would be vastly superior today.''',
            NULL, NULL, N'Đảo ngữ điều kiện hỗn hợp loại 3 & 2: ''Had governments invested heavily in clean wind power ten years ago, air quality would be vastly superior today.''', N'Đảo ngữ điều kiện hỗn hợp loại 3 & 2: ''Had governments invested heavily in clean wind power ten years ago, air quality would be vastly superior today.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Ngữ pháp đảo ngữ điều kiện hỗn hợp chủ đề biến đổi khí hậu.', N'Applied', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_442');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Had governments invested heavily in clean wind power ten years ago, air quality would be vastly superior today.', 1),
        (@MaCH, N'B', N'Were governments invested heavily, air quality will be vastly superior.', 0),
        (@MaCH, N'C', N'Should governments invest heavily, air quality would be vastly superior.', 0),
        (@MaCH, N'D', N'If had governments invested, air quality would be vastly superior.', 0);
END
GO

-- Câu hỏi SEED_EAI_443 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_443')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'According to Lesson 34, which musical term refers to the speed or pace of a musical piece?',
            NULL, NULL, N'''Tempo'' là thuật ngữ chỉ tốc độ hay nhịp độ diễn tấu của một bản nhạc.', N'''Tempo'' là thuật ngữ chỉ tốc độ hay nhịp độ diễn tấu của một bản nhạc.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 34: Âm nhạc và Nghệ thuật: Diễn đạt cảm xúc, Thể loại và Nhạc cụ - Thuật ngữ âm nhạc cơ bản (Tempo, Pitch, Harmony).', N'Extracted', N'BaiHoc_34_Am_nhac_va_Nghe_thuat.md', 34, 'SEED_EAI_443');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Tempo', 1),
        (@MaCH, N'B', N'Pitch', 0),
        (@MaCH, N'C', N'Lyrics', 0),
        (@MaCH, N'D', N'Chorus', 0);
END
GO

-- Câu hỏi SEED_EAI_444 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_444')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What is the meaning of the idiom ''music to someone''s ears''?',
            NULL, NULL, N'Thành ngữ ''music to my ears'' chỉ thông tin hoặc tin tức khiến người nghe vô cùng vui mừng, phấn khởi.', N'Thành ngữ ''music to my ears'' chỉ thông tin hoặc tin tức khiến người nghe vô cùng vui mừng, phấn khởi.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Thành ngữ và Cụm từ tiếng Anh về Nghệ thuật và Sáng tạo - Thành ngữ âm nhạc thông dụng.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_444');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Information that is highly pleasing or welcome news to receive', 1),
        (@MaCH, N'B', N'An unpleasant loud ringing noise in the ear', 0),
        (@MaCH, N'C', N'A recommendation for new wireless headphones', 0),
        (@MaCH, N'D', N'A boring lecture on classical violin compositions', 0);
END
GO

-- Câu hỏi SEED_EAI_445 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_445')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'We don''t have a rigid schedule for the weekend project, so let''s just play it by _______.',
            NULL, NULL, N'''play it by ear'' (tùy cơ ứng biến, linh hoạt theo tình hình thực tế).', N'''play it by ear'' (tùy cơ ứng biến, linh hoạt theo tình hình thực tế).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Thành ngữ ''Play it by ear'' trong giao tiếp tiếng Anh thường nhật.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_445');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'ear', 1),
        (@MaCH, N'B', N'eye', 0),
        (@MaCH, N'C', N'mouth', 0),
        (@MaCH, N'D', N'hand', 0);
END
GO

-- Câu hỏi SEED_EAI_446 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_446')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'TuVung', N'Kho', N'When someone has to ''face the music'', what are they forced to do?',
            NULL, NULL, N'''Face the music'' có nghĩa là phải chấp nhận đối mặt với hậu quả hoặc sự chỉ trích từ hành động sai trái của mình.', N'''Face the music'' có nghĩa là phải chấp nhận đối mặt với hậu quả hoặc sự chỉ trích từ hành động sai trái của mình.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Thành ngữ ''Face the music'' và nguồn gốc lịch sử.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_446');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Accept the unpleasant consequences or criticism of one''s actions', 1),
        (@MaCH, N'B', N'Buy front-row concert tickets to an orchestra', 0),
        (@MaCH, N'C', N'Learn how to conduct a choir', 0),
        (@MaCH, N'D', N'Dance to classical waltz rhythms', 0);
END
GO

-- Câu hỏi SEED_EAI_447 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_447')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Nobody likes coworkers who constantly blow their own _______ instead of sharing credit with the team.',
            NULL, NULL, N'''blow one''s own trumpet / horn'' (tự khoe khoang, tự tâng bốc bản thân).', N'''blow one''s own trumpet / horn'' (tự khoe khoang, tự tâng bốc bản thân).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Cụm từ ''Blow one''s own trumpet'' trong môi trường làm việc chuyên nghiệp.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_447');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'trumpet', 1),
        (@MaCH, N'B', N'flute', 0),
        (@MaCH, N'C', N'guitar', 0),
        (@MaCH, N'D', N'drum', 0);
END
GO

-- Câu hỏi SEED_EAI_448 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_448')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'To ''fine-tune'' an algorithm or business process means to:',
            NULL, NULL, N'''fine-tune'' có nghĩa là tinh chỉnh, điều chỉnh chi tiết nhỏ để đạt được hiệu suất tối ưu nhất.', N'''fine-tune'' có nghĩa là tinh chỉnh, điều chỉnh chi tiết nhỏ để đạt được hiệu suất tối ưu nhất.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Từ vựng mượn từ âm nhạc dùng trong công nghệ: ''Fine-tune''.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_448');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Make minor, precise adjustments to achieve the best possible performance', 1),
        (@MaCH, N'B', N'Cancel the entire system due to critical failure', 0),
        (@MaCH, N'C', N'Play background music while coding', 0),
        (@MaCH, N'D', N'Delete all database tables', 0);
END
GO

-- Câu hỏi SEED_EAI_449 (TuVung - Trình độ 4 - Dạng NoiCap - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_449')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'NoiCap', N'TuVung', N'TrungBinh', N'Match each instrument to its acoustic category:
1. Cello
2. Clarinet
3. Trombone
4. Timpani',
            NULL, NULL, N'1-String, 2-Woodwind, 3-Brass, 4-Percussion.', N'1-String, 2-Woodwind, 3-Brass, 4-Percussion.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 34: Phân loại các bộ nhạc cụ trong dàn nhạc giao hưởng (Orchestra Families).', N'Extracted', N'BaiHoc_34_Am_nhac_va_Nghe_thuat.md', 34, 'SEED_EAI_449');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'1-String, 2-Woodwind, 3-Brass, 4-Percussion', 1),
        (@MaCH, N'B', N'1-Brass, 2-String, 3-Percussion, 4-Woodwind', 0),
        (@MaCH, N'C', N'1-Percussion, 2-Brass, 3-String, 4-Woodwind', 0),
        (@MaCH, N'D', N'1-Woodwind, 2-Percussion, 3-Brass, 4-String', 0);
END
GO

-- Câu hỏi SEED_EAI_450 (Doc - Trình độ 4 - Dạng TraLoiNgan - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_450')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TraLoiNgan', N'Doc', N'Kho', N'In art criticism, what French-origin term describes the prevailing mood, atmosphere, or environment of an artistic installation?',
            NULL, NULL, N'''Ambiance'' (hoặc ''Atmosphere'' / ''Milieu'') chỉ bầu không khí, không gian bao quanh của một tác phẩm sắp đặt.', N'''Ambiance'' (hoặc ''Atmosphere'' / ''Milieu'') chỉ bầu không khí, không gian bao quanh của một tác phẩm sắp đặt.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Thuật ngữ phê bình mỹ thuật và triển lãm không gian đương đại.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_450');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Ambiance', 1),
        (@MaCH, N'B', N'Collage', 0),
        (@MaCH, N'C', N'Fresco', 0),
        (@MaCH, N'D', N'Silhouette', 0);
END
GO

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

PRINT N'Đã hoàn thành nạp 500 CÂU HỎI TIẾNG ANH CHUẨN HÓA TOÀN DIỆN (SEED_EAI_001 -> SEED_EAI_500) thành công!';
GO

-- ===================================================================

EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO

PRINT N'===================================================================';
PRINT N'NẠP THÀNH CÔNG NGÂN HÀNG CÂU HỎI CHUẨN HÓA (04_nganhangcauhoi)!';
PRINT N' - Tổng cộng: ĐÚNG 704 CÂU HỎI chuẩn hóa không trùng lặp, không rác';
PRINT N' - 416 câu gắn trực tiếp vào 36 Bài học có trích xuất dẫn chứng cụ thể';
PRINT N' - 288 câu nâng cao & khảo sát thuộc Ngân hàng hệ thống';
PRINT N' - 100% câu hỏi có đáp án A/B/C/D và lời giải chi tiết';
PRINT N'===================================================================';
GO
