-- ===================================================================
-- Script: seed_sprint2_data.sql
-- Nạp dữ liệu thực tế cho Từ vựng, Flashcard, Câu hỏi, Đề thi thử (Sprint 2)
-- ===================================================================
USE TiengAnhAI;
GO

SET NOCOUNT ON;
GO

-- 1. XÓA DỮ LIỆU CŨ CỦA CÁC BẢNG LIÊN QUAN ĐỂ NẠP LẠI CHUẨN
DELETE FROM ChiTietTraLoi;
DELETE FROM LuotLamBai;
DELETE FROM CauHoiBaiTap;
DELETE FROM BaiTapDeThi;
DELETE FROM CauTraLoiKhaoSat;
DELETE FROM LuotKhaoSat;
DELETE FROM CauHoiKhaoSat;
DELETE FROM BaiKhaoSatDauVao;
DELETE FROM LuaChonDapAn;
DELETE FROM CauHoi;
DELETE FROM Flashcard;
DELETE FROM TuVung;
GO

-- 2. NẠP DỮ LIỆU TỪ VỰNG (TuVung)
DECLARE @tdA1 INT = 1;
DECLARE @tdA2 INT = 2;
DECLARE @tdB1 INT = 3;
DECLARE @tdB2 INT = 4;
DECLARE @tdC1 INT = 5;

DECLARE @cdGiaoTiep INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Giao%') ;
IF @cdGiaoTiep IS NULL SET @cdGiaoTiep = 1;

DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS%');
IF @cdIELTS IS NULL SET @cdIELTS = 5;

DECLARE @cdGrammar INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Ngữ Pháp%');
IF @cdGrammar IS NULL SET @cdGrammar = 9;

INSERT INTO TuVung (MaChuDe, MaTrinhDo, Tu, LoaiTu, PhienAmIPA, Nghia, CauViDu, CumTuDiKem)
VALUES
(@cdIELTS, @tdC1, N'Ubiquitous', N'adj', N'/juːˈbɪk.wɪ.təs/', 
 N'Phổ biến ở khắp mọi nơi, hiện diện đồng thời ở mọi chỗ (seeming to be everywhere).', 
 N'"Smartphones have become ubiquitous in daily modern life, radically transforming how students study."', 
 N'• Ubiquitous influence • Ubiquitous presence of technology'),

(@cdIELTS, @tdC1, N'Meticulous', N'adj', N'/məˈtɪk.jə.ləs/', 
 N'Tỉ mỉ, cẩn thận từng chi tiết nhỏ nhất (very careful and with great attention to every detail).', 
 N'"The research paper required meticulous attention to detail to avoid statistical discrepancies."', 
 N'• Meticulous research • Meticulous planning'),

(@cdIELTS, @tdB2, N'Pragmatic', N'adj', N'/præɡˈmæt.ɪk/', 
 N'Thực tế, thực dụng, dựa trên tính khả thi thay vì lý thuyết suông.', 
 N'"In dealing with environmental issues, governments must take a pragmatic and balanced approach."', 
 N'• Pragmatic solution • Pragmatic approach'),

(@cdIELTS, @tdC1, N'Corroborate', N'verb', N'/kəˈrɒb.ə.reɪt/', 
 N'Chứng minh, củng cố thêm bằng chứng xác thực (to confirm or support with new evidence).', 
 N'"Subsequent geological studies corroborated the theory proposed by scientists a decade earlier."', 
 N'• Corroborate evidence • Corroborate findings'),

(@cdIELTS, @tdB2, N'Detrimental', N'adj', N'/ˌdet.rɪˈmen.təl/', 
 N'Gây hại, có hại (causing harm or damage).', 
 N'"Excessive screen time before sleeping is proven to have a detrimental effect on sleep quality."', 
 N'• Detrimental effect • Detrimental impact'),

(@cdIELTS, @tdB2, N'Inevitable', N'adj', N'/ɪnˈev.ɪ.tə.bəl/', 
 N'Không thể tránh khỏi, chắc chắn sẽ xảy ra.', 
 N'"With the rapid growth of AI, changes in the job market are inevitable."', 
 N'• Inevitable consequence • Inevitable outcome'),

(@cdIELTS, @tdB1, N'Resilient', N'adj', N'/rɪˈzɪl.jənt/', 
 N'Kiên cường, có khả năng phục hồi nhanh sau khó khăn hoặc nghịch cảnh.', 
 N'"Local communities showed themselves to be remarkably resilient after the severe storm."', 
 N'• Highly resilient • Resilient economy'),

(@cdIELTS, @tdB2, N'Feasible', N'adj', N'/ˈfiː.zə.bəl/', 
 N'Khả thi, có thể thực hiện được một cách thực tế.', 
 N'"It is not economically feasible to replace all fossil fuel power plants overnight."', 
 N'• Technically feasible • Feasible alternative'),

(@cdIELTS, @tdB2, N'Ambiguous', N'adj', N'/æmˈbɪɡ.ju.əs/', 
 N'Mơ hồ, có thể hiểu theo nhiều nghĩa khác nhau.', 
 N'"The instructions given by the supervisor were ambiguous and caused significant confusion."', 
 N'• Ambiguous statement • Ambiguous wording'),

(@cdIELTS, @tdB1, N'Procrastinate', N'verb', N'/prəˈkræs.tɪ.neɪt/', 
 N'Trì hoãn, chần chừ không làm việc cần làm ngay.', 
 N'"Most students tend to procrastinate until the night before the final assignment is due."', 
 N'• Procrastinate on homework • Chronic procrastination'),

(@cdGiaoTiep, @tdA1, N'Enthusiastic', N'adj', N'/ɪnˌθjuː.ziˈæs.tɪk/', 
 N'Nhiệt tình, hăng hái, hào hứng.', 
 N'"The new teacher received an enthusiastic welcome from all the students."', 
 N'• Enthusiastic support • Enthusiastic about learning'),

(@cdGiaoTiep, @tdA2, N'Diligent', N'adj', N'/ˈdɪl.ɪ.dʒənt/', 
 N'Chăm chỉ, siêng năng, cần cù.', 
 N'"She was a diligent student who consistently reviewed her lecture notes every evening."', 
 N'• Diligent worker • Diligent effort');
GO

-- 3. NẠP DỮ LIỆU CÂU HỎI TRẮC NGHIỆM VÀ LỰA CHỌN ĐÁP ÁN (CauHoi & LuaChonDapAn)
DECLARE @tdA1 INT = 1;
DECLARE @tdA2 INT = 2;
DECLARE @tdB1 INT = 3;
DECLARE @tdB2 INT = 4;
DECLARE @tdC1 INT = 5;

DECLARE @cdGrammar INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Ngữ Pháp%');
IF @cdGrammar IS NULL SET @cdGrammar = 9;

DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS%');
IF @cdIELTS IS NULL SET @cdIELTS = 5;

DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
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
(@q4, 'A', N'light', 0), (@q4, 'B', N'heavy', 1), (@q4, 'C', N'soft', 0), (@q4, 'D', N'smooth', 0);

-- Câu 5 (B1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'NguPhap', N'If it ______ tomorrow, we will have to postpone our outdoor picnic.', 
        N'Mệnh đề điều kiện loại 1 (If + Present Simple, will + V) dùng "rains".', @gvId, N'ThuCong', 1);
DECLARE @q5 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q5, 'A', N'rained', 0), (@q5, 'B', N'will rain', 0), (@q5, 'C', N'rains', 1), (@q5, 'D', N'would rain', 0);

-- Câu 6 (B1 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'TuVung', N'She is looking for a ______ apartment closer to her office to reduce commute time.', 
        N'"Suitable" nghĩa là phù hợp, thích hợp với nhu cầu.', @gvId, N'ThuCong', 1);
DECLARE @q6 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q6, 'A', N'suitable', 1), (@q6, 'B', N'awkward', 0), (@q6, 'C', N'clumsy', 0), (@q6, 'D', N'hostile', 0);

-- Câu 7 (B1 Reading)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'Doc', 
 N'According to the passage, why do owls hunt predominantly at night?', 
 N'Owls are nocturnal birds of prey found on every continent except Antarctica. Their large eyes and exceptional binaural hearing allow them to detect the rustling of small rodents under leaf litter even in pitch-black conditions. Hunting at night also helps them avoid direct competition with diurnal raptors like hawks and eagles.',
 N'Đoạn văn nêu rõ cú đêm săn mồi vào ban đêm vì thính giác/thị giác vượt trội và để tránh cạnh tranh trực tiếp với các loài chim săn mồi ban ngày (hawks and eagles).',
 @gvId, N'ThuCong', 1);
DECLARE @q7 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q7, 'A', N'They cannot tolerate warm sunshine.', 0),
(@q7, 'B', N'Their specialized senses excel in the dark and they avoid daytime competitors.', 1),
(@q7, 'C', N'Rodents are only active during morning hours.', 0),
(@q7, 'D', N'They prefer socializing in darkness.', 0);

-- Câu 8 (B2 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'Neither the director nor his assistants ______ willing to sign the revised agreement.', 
        N'Quy tắc hợp chủ ngữ với "Neither... nor...": Động từ chia theo danh từ gần nhất ("his assistants" là số nhiều -> "were").', @gvId, N'ThuCong', 1);
DECLARE @q8 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q8, 'A', N'was', 0), (@q8, 'B', N'were', 1), (@q8, 'C', N'is', 0), (@q8, 'D', N'has been', 0);

-- Câu 9 (B2 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'TuVung', N'The committee decided to ______ stricter environmental policies to curb industrial carbon emissions.', 
        N'Cụm từ "implement policies" có nghĩa là thi hành/áp dụng chính sách.', @gvId, N'ThuCong', 1);
DECLARE @q9 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q9, 'A', N'implement', 1), (@q9, 'B', N'compliment', 0), (@q9, 'C', N'supplement', 0), (@q9, 'D', N'experiment', 0);

-- Câu 10 (B2 Grammar - Inversion)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'Seldom ______ such a breathtaking display of aurora borealis in this region.', 
        N'Đảo ngữ với trạng từ phủ định đứng đầu câu: Seldom + trợ động từ + S + V chính ("have we witnessed").', @gvId, N'ThuCong', 1);
DECLARE @q10 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q10, 'A', N'we have witnessed', 0), (@q10, 'B', N'have we witnessed', 1), (@q10, 'C', N'we witnessed', 0), (@q10, 'D', N'had witnessed we', 0);

-- Câu 11 (C1 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'Subsequent empirical studies completely ______ the original hypothesis proposed by Dr. Vance.', 
        N'"Corroborate" nghĩa là chứng thực, củng cố bằng chứng cho giả thuyết.', @gvId, N'ThuCong', 1);
DECLARE @q11 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q11, 'A', N'corroborated', 1), (@q11, 'B', N'retracted', 0), (@q11, 'C', N'dismantled', 0), (@q11, 'D', N'diminished', 0);

-- Câu 12 (C1 Reading)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'The author uses the phrase "double-edged sword" to emphasize that artificial intelligence:', 
 N'The integration of generative artificial intelligence across industrial workflows represents a double-edged sword. On one hand, automated synthesis and code generation accelerate throughput exponentially. Conversely, intellectual property vulnerabilities and cognitive atrophy among junior engineers present formidable existential hurdles for long-term innovation.', 
 N'"Double-edged sword" (con dao hai lưỡi) biểu thị cả lợi ích to lớn (tăng tốc độ) lẫn rủi ro tiềm tàng (vấn đề sở hữu trí tuệ, suy giảm tư duy).',
 @gvId, N'ThuCong', 1);
DECLARE @q12 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q12, 'A', N'is entirely hazardous and must be prohibited immediately', 0),
(@q12, 'B', N'delivers immense productivity gains while simultaneously introducing substantial perils', 1),
(@q12, 'C', N'has surpassed human cognition in ethical discernment', 0),
(@q12, 'D', N'is exclusively beneficial for junior software developers', 0);

-- Câu 13 (A2 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'NguPhap', N'How ______ apples do we need to bake this apple pie?', 
        N'Danh từ đếm được số nhiều "apples" đi với "many" ("How many").', @gvId, N'ThuCong', 1);
DECLARE @q13 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q13, 'A', N'much', 0), (@q13, 'B', N'many', 1), (@q13, 'C', N'little', 0), (@q13, 'D', N'few', 0);

-- Câu 14 (B1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'NguPhap', N'I have been living in this coastal city ______ more than five years.', 
        N'Khoảng thời gian ("more than five years") đi với giới từ "for".', @gvId, N'ThuCong', 1);
DECLARE @q14 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q14, 'A', N'since', 0), (@q14, 'B', N'for', 1), (@q14, 'C', N'during', 0), (@q14, 'D', N'at', 0);

-- Câu 15 (B2 Grammar - Subjunctive)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, NguonTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'The doctor recommended that the patient ______ adequate rest and avoid heavy lifting.', 
        N'Cấu trúc giả định thức (Subjunctive): recommend that S + (should) V-nguyên thể ("take").', @gvId, N'ThuCong', 1);
DECLARE @q15 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q15, 'A', N'takes', 0), (@q15, 'B', N'took', 0), (@q15, 'C', N'take', 1), (@q15, 'D', N'will take', 0);
GO

-- 4. NẠP BÀI KHẢO SÁT ĐẦU VÀO & CÂU HỎI KHẢO SÁT (BaiKhaoSatDauVao & CauHoiKhaoSat)
INSERT INTO BaiKhaoSatDauVao (TenBaiKhaoSat, MoTa, TongSoCauHoi, DangHoatDong, NgayTao)
VALUES (N'Khảo sát Đánh giá Năng lực Đầu vào Chuẩn Quốc Tế CEFR', 
        N'Bài thi chẩn đoán phân loại trình độ từ A1 đến C1 giúp hệ thống cá nhân hóa lộ trình học.', 15, 1, SYSDATETIME());
DECLARE @maBKS INT = SCOPE_IDENTITY();

-- Liên kết 15 câu hỏi vào bài khảo sát
INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi)
SELECT @maBKS, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC)
FROM CauHoi;
GO

-- 5. NẠP DANH SÁCH ĐỀ THI THỬ & LUYỆN TẬP (BaiTapDeThi & CauHoiBaiTap)
DECLARE @tdB2 INT = 4;
DECLARE @tdB1 INT = 3;
DECLARE @tdC1 INT = 5;
DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS%');
IF @cdIELTS IS NULL SET @cdIELTS = 5;
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = 1;

-- Đề thi 1: IELTS Mock Test #05
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB2, N'IELTS Academic Reading & Listening Mini Mock Test #05', N'ThiThu', 
        N'Bao gồm bài đọc Skimming/Scanning, phân tích từ vựng học thuật C1 và cấu trúc ngữ pháp đảo ngữ.', 40, 1, @gvId);
DECLARE @de1 INT = SCOPE_IDENTITY();

-- Gắn câu hỏi vào đề 1
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @de1, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 10 MaCauHoi FROM CauHoi ORDER BY MaCauHoi DESC);

-- Đề thi 2: TOEIC Reading Comprehension
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB1, N'TOEIC Reading Comprehension Diagnostic Test', N'ThiThu', 
        N'Part 5 (Hoàn thành câu), Part 6 (Điền đoạn văn) và Part 7 (Đọc hiểu văn bản thương mại).', 45, 1, @gvId);
DECLARE @de2 INT = SCOPE_IDENTITY();

-- Gắn câu hỏi vào đề 2
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @de2, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 10 MaCauHoi FROM CauHoi ORDER BY MaCauHoi ASC);

-- Đề thi 3: Ngữ pháp chuyên sâu B2
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB2, N'Kiểm tra Đột phá Ngữ pháp & Collocations Band B2', N'ThiThu', 
        N'25 câu trắc nghiệm câu điều kiện đảo ngữ, mệnh đề quan hệ rút gọn, liên từ nâng cao.', 25, 1, @gvId);
DECLARE @de3 INT = SCOPE_IDENTITY();

-- Gắn câu hỏi vào đề 3
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @de3, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 10 MaCauHoi FROM CauHoi ORDER BY NEWID());

-- Đề 4: Luyện tập phản xạ nhanh
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB1, N'Thử Thách Phản Xạ Trắc Nghiệm 60s', N'LuyenTap', 
        N'Luyện tập phản xạ nhanh từ vựng và ngữ pháp tiếng Anh trong 60 giây.', 10, 1, @gvId);
DECLARE @de4 INT = SCOPE_IDENTITY();

INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @de4, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0
FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 5 MaCauHoi FROM CauHoi ORDER BY MaCauHoi ASC);
GO

PRINT N'Nạp dữ liệu Sprint 2 (Từ vựng, Câu hỏi, Khảo sát, Đề thi) thành công!';
GO
