-- =====================================================================
-- Migration / Seed: Câu hỏi luyện nghe chuẩn hóa (Fallback listening questions)
-- Fix: BUG-DB-02 (Foreign Key violation when submitting practice sessions)
-- =====================================================================

IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaCauHoi = 9001)
BEGIN
    SET IDENTITY_INSERT CauHoi ON;
    INSERT INTO CauHoi (MaCauHoi, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, DoKho, NguonTao, DaKiemDuyet, NgayTao)
    VALUES 
    (9001, 2, N'TracNghiem', N'Nghe', N'Where is the conversation most likely taking place?', 
     N'Audio Transcript:\n"Good afternoon, sir. Are you checking in today? Yes, I have a reservation under the name Anderson. May I see your passport and credit card, please? Here you go. Your room 402 on the fourth floor is ready. Breakfast is served from 7 to 10 AM in the restaurant."',
     N'Người nói dùng các từ ''checking in'', ''reservation'', ''room 402'', ''breakfast'' -> Khách sạn (Hotel).', N'TrungBinh', N'HocLieuChuan', 1, SYSDATETIME()),
    (9002, 2, N'TracNghiem', N'Nghe', N'What time will the product launch meeting start?', 
     N'Audio Transcript:\n"Attention all marketing team members. Due to the client''s flight delay, our quarterly product launch meeting originally scheduled for 2:00 PM has been pushed back to 3:30 PM in Conference Room B. Please be punctual."',
     N'Thông báo hoãn sang 3:30 PM (has been pushed back to 3:30 PM).', N'TrungBinh', N'HocLieuChuan', 1, SYSDATETIME()),
    (9003, 3, N'TracNghiem', N'Nghe', N'What problem is the woman addressing in the broadcast?', 
     N'Audio Transcript:\n"Hello listeners. Welcome back to Tech Today. Recent cybersecurity investigations revealed that thousands of smart home devices are vulnerable to unauthorized access because consumers neglect to change default factory passwords. Today we have an expert to discuss solutions."',
     N'Lỗ hổng bảo mật của thiết bị nhà thông minh.', N'Kho', N'HocLieuChuan', 1, SYSDATETIME()),
    (9004, 3, N'TracNghiem', N'Nghe', N'What is the speaker advising passengers to do?', 
     N'Audio Transcript:\n"May I have your attention, please. Flight VN214 bound for London is now ready for boarding at Gate 18. All passengers traveling with infants and those requiring special assistance, please proceed to the gate now."',
     N'Tiến ra cửa khởi hành số 18 để lên máy bay.', N'TrungBinh', N'HocLieuChuan', 1, SYSDATETIME()),
    (9005, 4, N'TracNghiem', N'Nghe', N'According to the professor, what led to the revival of renewable architecture?', 
     N'Audio Transcript:\n"In the late twentieth century, urban planners largely disregarded ecological footprints. However, the unprecedented escalation of fossil fuel costs and acute climate awareness in the early 2000s precipitated a paradigm shift, prompting architects to reintegrate passive solar design and natural ventilation."',
     N'Chi phí năng lượng hóa thạch leo thang và nhận thức khí hậu.', N'Kho', N'HocLieuChuan', 1, SYSDATETIME());
    SET IDENTITY_INSERT CauHoi OFF;
END

IF NOT EXISTS (SELECT 1 FROM LuaChonDapAn WHERE MaLuaChon = 90011)
BEGIN
    SET IDENTITY_INSERT LuaChonDapAn ON;
    INSERT INTO LuaChonDapAn (MaLuaChon, MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung)
    VALUES
    (90011, 9001, N'A', N'At a hotel front desk', 1),
    (90012, 9001, N'B', N'At an airport gate', 0),
    (90013, 9001, N'C', N'At a train station', 0),
    (90014, 9001, N'D', N'At a library', 0),
    (90021, 9002, N'A', N'2:00 PM', 0),
    (90022, 9002, N'B', N'3:30 PM', 1),
    (90023, 9002, N'C', N'4:00 PM', 0),
    (90024, 9002, N'D', N'1:30 PM', 0),
    (90031, 9003, N'A', N'High prices of computer hardware', 0),
    (90032, 9003, N'B', N'Security vulnerabilities in smart home devices', 1),
    (90033, 9003, N'C', N'Power outages in the city', 0),
    (90034, 9003, N'D', N'Slow internet download speeds', 0),
    (90041, 9004, N'A', N'Proceed to Gate 18 for boarding', 1),
    (90042, 9004, N'B', N'Collect their luggage from Carousel 3', 0),
    (90043, 9004, N'C', N'Change their ticket reservations', 0),
    (90044, 9004, N'D', N'Exit the airport terminal', 0),
    (90051, 9005, N'A', N'Escalating energy costs and climate awareness', 1),
    (90052, 9005, N'B', N'A decline in urban population', 0),
    (90053, 9005, N'C', N'International sports competitions', 0),
    (90054, 9005, N'D', N'The invention of artificial lighting', 0);
    SET IDENTITY_INSERT LuaChonDapAn OFF;
END
GO

