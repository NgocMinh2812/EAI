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
-- 5. BỔ SUNG CÂU HỎI VÀO CÁC ĐỀ THI THỬ HIỆN CÓ ĐỂ ĐỦ 10 CÂU
-- ===================================================================
-- Đề 0 (A1): Thêm câu hỏi A1-A2
;WITH CTE0 AS (
    SELECT MaCauHoi, KyNang, DoKho,
           (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 0) + ROW_NUMBER() OVER(ORDER BY MaCauHoi) AS NextOrder
    FROM @NewQuestions
    WHERE MaTrinhDo IN (@tdA1, @tdA2) AND MaCauHoi NOT IN (SELECT MaCauHoi FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 0)
)
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
SELECT 0, MaCauHoi, NextOrder, 1.0, N'TracNghiem', KyNang, DoKho
FROM CTE0 WHERE NextOrder <= 10;
UPDATE BaiTapDeThi SET TongSoCau = (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 0) WHERE MaBaiTapDeThi = 0;

-- Đề 1 (A2): Thêm câu hỏi A2-B1
;WITH CTE1 AS (
    SELECT MaCauHoi, KyNang, DoKho,
           (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 1) + ROW_NUMBER() OVER(ORDER BY MaCauHoi) AS NextOrder
    FROM @NewQuestions
    WHERE MaTrinhDo IN (@tdA2, @tdB1) AND MaCauHoi NOT IN (SELECT MaCauHoi FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 1)
)
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
SELECT 1, MaCauHoi, NextOrder, 1.0, N'TracNghiem', KyNang, DoKho
FROM CTE1 WHERE NextOrder <= 10;
UPDATE BaiTapDeThi SET TongSoCau = (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 1) WHERE MaBaiTapDeThi = 1;

-- Đề 5 (C1): Thêm câu hỏi C1
;WITH CTE5 AS (
    SELECT MaCauHoi, KyNang, DoKho,
           (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 5) + ROW_NUMBER() OVER(ORDER BY MaCauHoi) AS NextOrder
    FROM @NewQuestions
    WHERE MaTrinhDo = @tdC1 AND MaCauHoi NOT IN (SELECT MaCauHoi FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 5)
)
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
SELECT 5, MaCauHoi, NextOrder, 1.0, N'TracNghiem', KyNang, DoKho
FROM CTE5 WHERE NextOrder <= 10;
UPDATE BaiTapDeThi SET TongSoCau = (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = 5) WHERE MaBaiTapDeThi = 5;

-- ===================================================================
-- 6. TẠO CÁC ĐỀ THI THỬ MỚI (LoaiHinh = 'ThiThu')
-- ===================================================================

-- Thi thử 1: TOEIC Part 5 & 6 Doanh Nghiệp
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%TOEIC Part 5 & 6: Khảo Thí Ngữ Pháp%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdBusiness, @tdB1, N'TOEIC Part 5 & 6: Khảo Thí Ngữ Pháp & Từ Vựng Doanh Nghiệp', N'ThiThu', 
            N'Đề thi thử định dạng TOEIC Reading: Hoàn thành câu công sở, từ vựng hợp đồng, liên từ và cấu trúc văn bản thương mại.', 25, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deThiThu1 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deThiThu1, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.MaTrinhDo IN (@tdB1, @tdB2) AND q.KyNang IN (N'NguPhap', N'TuVung', N'Viet')
    ORDER BY q.Id;
END

-- Thi thử 2: IELTS Academic Reading Mock Test #06
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%IELTS Academic Reading & Language In Use Mock Test #06%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdIELTS, @tdB2, N'IELTS Academic Reading & Language In Use Mock Test #06', N'ThiThu', 
            N'Đề thi thử IELTS Học thuật: Bao gồm bài đọc chuyên sâu AI trong y học, Skimming & Scanning, từ vựng C1 và phân tích suy luận logic.', 35, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deThiThu2 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deThiThu2, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id DESC), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.MaTrinhDo IN (@tdB2, @tdC1)
    ORDER BY q.Id DESC;
END

-- Thi thử 3: VSTEP B2 Chuẩn Hóa
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%VSTEP B2 Chuẩn Hóa%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdGrammar, @tdB2, N'VSTEP B2 Chuẩn Hóa: Ngữ Pháp Chuyên Sâu & Đọc Hiểu Tổng Hợp', N'ThiThu', 
            N'Đề thi mô phỏng định dạng VSTEP Bậc 4 (B2): Kiểm tra toàn diện ngữ pháp phức hợp, đảo ngữ, đọc hiểu công sở và kỹ năng viết chuẩn mực.', 30, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deThiThu3 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deThiThu3, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.MaTrinhDo = @tdB2
    ORDER BY q.Id;
END

-- ===================================================================
-- 7. TẠO CÁC GÓI BÀI TẬP & ĐỀ LUYỆN TẬP (LoaiHinh = 'LuyenTap')
-- ===================================================================

-- Gói Luyện Tập 1: Chuyên đề Câu điều kiện & Đảo ngữ
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%Gói Luyện Tập: Chuyên Đề Câu Điều Kiện%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdGrammar, @tdB2, N'Gói Luyện Tập: Chuyên Đề Câu Điều Kiện & Đảo Ngữ Nâng Cao', N'LuyenTap', 
            N'Luyện tập chuyên sâu các dạng câu điều kiện loại 1, 2, 3, hỗn hợp và các cấu trúc đảo ngữ thường gặp trong đề thi.', 20, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen1 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen1, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.KyNang = N'NguPhap' AND q.MaTrinhDo IN (@tdB1, @tdB2, @tdC1)
    ORDER BY q.Id;
END

-- Gói Luyện Tập 2: 100 Từ Vựng & Collocations Oxford Cốt Lõi
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%100 Từ Vựng & Collocations Oxford%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdIELTS, @tdB1, N'Gói Luyện Tập: 100 Từ Vựng & Collocations Oxford Cốt Lõi', N'LuyenTap', 
            N'Rèn luyện phản xạ từ vựng cốt lõi, cụm từ đi kèm (collocations) và cụm động từ (phrasal verbs) mức độ B1 - B2.', 15, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen2 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen2, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.KyNang = N'TuVung'
    ORDER BY q.Id;
END

-- Gói Luyện Tập 3: Đọc Hiểu Chuyên Sâu: Trí Tuệ Nhân Tạo & Y Học
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%Đọc Hiểu Chuyên Sâu: Trí Tuệ Nhân Tạo%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdIELTS, @tdB2, N'Gói Luyện Tập Đọc Hiểu Chuyên Sâu: Trí Tuệ Nhân Tạo & Y Học Hiện Đại', N'LuyenTap', 
            N'Luyện tập đọc hiểu theo đoạn văn dài: Đoán nghĩa từ vựng trong ngữ cảnh, tìm ý chính và phân tích suy luận logic.', 25, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen3 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen3, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.KyNang = N'Doc'
    ORDER BY q.Id;
END

-- Gói Luyện Tập 4: Cấu Trúc Viết & Sửa Lỗi Ngữ Pháp Học Thuật
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%Cấu Trúc Viết & Phát Hiện Lỗi Sai%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdIELTS, @tdB2, N'Gói Luyện Tập: Cấu Trúc Viết & Phát Hiện Lỗi Sai Học Thuật', N'LuyenTap', 
            N'Chuyên đề phát hiện lỗi sai ngữ pháp, viết lại câu đồng nghĩa, cấu trúc song hành và chuyển tiếp mạch lạc cho bài luận.', 20, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen4 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen4, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.KyNang = N'Viet'
    ORDER BY q.Id;
END

-- Gói Luyện Tập 5: Tổng Hợp Căn Bản A2
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%Tổng Hợp: Ngữ Pháp Căn Bản & Giao Tiếp%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdDaily, @tdA2, N'Gói Luyện Tập Tổng Hợp: Ngữ Pháp Căn Bản & Giao Tiếp Đời Sống', N'LuyenTap', 
            N'Gói ôn tập nền tảng dành cho học viên A1 - A2: Thì quá khứ, hiện tại hoàn thành căn bản, so sánh và từ vựng thông dụng.', 15, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen5 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen5, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.MaTrinhDo IN (@tdA1, @tdA2, @tdB1) AND q.KyNang IN (N'NguPhap', N'TuVung', N'Viet')
    ORDER BY q.Id;
END

-- Gói Luyện Tập 6: Thành Ngữ & Cụm Từ Học Thuật C1
IF NOT EXISTS (SELECT 1 FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE N'%Thành Ngữ & Cụm Từ Học Thuật Band C1%')
BEGIN
    INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, TrangThai, NguoiTao, NgayTao)
    VALUES (@cdIELTS, @tdC1, N'Gói Luyện Tập Thành Ngữ & Cụm Từ Học Thuật Band C1 (Advanced Idioms)', N'LuyenTap', 
            N'Thử thách nâng cao vốn từ vựng học thuật C1, thành ngữ báo chí, tư duy phản biện và phong cách diễn đạt sắc sảo.', 20, 5.0, 10.0, 10, 1, 1, N'DaXuatBan', @gvId, SYSDATETIME());
    DECLARE @deLuyen6 INT = SCOPE_IDENTITY();

    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot)
    SELECT TOP 10 @deLuyen6, q.MaCauHoi, ROW_NUMBER() OVER(ORDER BY q.Id DESC), 1.0, N'TracNghiem', q.KyNang, q.DoKho
    FROM @NewQuestions q WHERE q.MaTrinhDo = @tdC1
    ORDER BY q.Id DESC;
END

PRINT N'===================================================================';
PRINT N'ĐÃ BỔ SUNG TOÀN BỘ CÂU HỎI VÀ TẠO CÁC ĐỀ THI THỬ & GÓI LUYỆN TẬP THÀNH CÔNG!';
PRINT N'===================================================================';
GO
