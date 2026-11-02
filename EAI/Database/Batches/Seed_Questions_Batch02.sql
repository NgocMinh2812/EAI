-- ====================================================================================
-- SEED QUESTIONS BATCH: Batch 02 (SEED_EAI_051 -> SEED_EAI_100)
-- Số lượng câu hỏi: 50
-- Đảm bảo: Idempotent (Chạy lại nhiều lần không trùng lặp)
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Đang nạp Batch 02 (SEED_EAI_051 -> SEED_EAI_100) (50 câu hỏi)...';

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

PRINT N'Đã hoàn thành nạp Batch 02 (SEED_EAI_051 -> SEED_EAI_100) thành công!';
GO
