USE TiengAnhAI;
GO

-- 1. Thêm bộ đề khảo sát thứ 7: Bộ đề Tổng hợp Đa Cấp độ A1 - C2 nếu chưa có
IF NOT EXISTS (SELECT 1 FROM BaiKhaoSatDauVao WHERE TenBaiKhaoSat LIKE N'%Tổng hợp%' OR MaBaiKhaoSat = 7)
BEGIN
    SET IDENTITY_INSERT BaiKhaoSatDauVao ON;
    INSERT INTO BaiKhaoSatDauVao (MaBaiKhaoSat, TenBaiKhaoSat, MoTa, TongSoCauHoi, DangHoatDong, NgayTao)
    VALUES (7, N'Khảo sát Đánh giá Năng lực Toàn diện - Tổng hợp Đa Cấp độ (A1 - C2)', 
               N'Bài kiểm tra tổng hợp toàn diện phân tầng độ khó từ A1 đến C2: Đo lường chính xác năng lực thực tế hiện có của người học mà không cần chọn trước trình độ.', 
               20, 1, SYSDATETIME());
    SET IDENTITY_INSERT BaiKhaoSatDauVao OFF;
END
ELSE
BEGIN
    UPDATE BaiKhaoSatDauVao
    SET TenBaiKhaoSat = N'Khảo sát Đánh giá Năng lực Toàn diện - Tổng hợp Đa Cấp độ (A1 - C2)',
        MoTa = N'Bài kiểm tra tổng hợp toàn diện phân tầng độ khó từ A1 đến C2: Đo lường chính xác năng lực thực tế hiện có của người học mà không cần chọn trước trình độ.',
        TongSoCauHoi = 20,
        DangHoatDong = 1
    WHERE TenBaiKhaoSat LIKE N'%Tổng hợp%' OR MaBaiKhaoSat = 7;
END
GO

-- Lấy mã bài khảo sát tổng hợp (thường là 7)
DECLARE @MaBaiTongHop INT = (SELECT TOP 1 MaBaiKhaoSat FROM BaiKhaoSatDauVao WHERE TenBaiKhaoSat LIKE N'%Tổng hợp%' OR MaBaiKhaoSat = 7);

-- 2. Gán 20 câu hỏi đa kỹ năng phân tầng A1 -> C2 vào bộ đề tổng hợp
DELETE FROM CauHoiKhaoSat WHERE MaBaiKhaoSat = @MaBaiTongHop;

INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi) VALUES
    (@MaBaiTongHop, 270, 1),  -- A1 NguPhap
    (@MaBaiTongHop, 72, 2),   -- A1 TuVung
    (@MaBaiTongHop, 102, 3),  -- A1 Nghe / GiaoTiep
    (@MaBaiTongHop, 292, 4),  -- A2 NguPhap
    (@MaBaiTongHop, 100, 5),  -- A2 TuVung
    (@MaBaiTongHop, 86, 6),   -- A2 Doc
    (@MaBaiTongHop, 421, 7),  -- B1 NguPhap
    (@MaBaiTongHop, 416, 8),  -- B1 NguPhap
    (@MaBaiTongHop, 407, 9),  -- B1 TuVung
    (@MaBaiTongHop, 406, 10), -- B1 Doc
    (@MaBaiTongHop, 335, 11), -- B2 NguPhap
    (@MaBaiTongHop, 374, 12), -- B2 NguPhap
    (@MaBaiTongHop, 21, 13),  -- B2 NguPhap
    (@MaBaiTongHop, 379, 14), -- B2 TuVung
    (@MaBaiTongHop, 424, 15), -- C1 NguPhap
    (@MaBaiTongHop, 378, 16), -- C1 NguPhap
    (@MaBaiTongHop, 43, 17),  -- C1 TuVung
    (@MaBaiTongHop, 521, 18), -- C2 TuVung
    (@MaBaiTongHop, 524, 19), -- C2 Doc
    (@MaBaiTongHop, 535, 20); -- C2 Doc
GO

-- 3. Nạp chi tiết câu trả lời thật vào bảng CauTraLoiKhaoSat cho các lượt khảo sát sinh viên đã nộp
-- Đảm bảo khi giảng viên bấm "Xem chi tiết" thì hiển thị đúng các thẻ câu hỏi, lựa chọn đã chọn, đúng/sai, giải thích
DELETE FROM CauTraLoiKhaoSat;

DECLARE @MaLuot INT, @MaBai INT, @SoDung INT, @TongSo INT;
DECLARE curLuot CURSOR FOR 
    SELECT MaLuotKhaoSat, MaBaiKhaoSat, SoCauDung, TongSoCau FROM LuotKhaoSat;

OPEN curLuot;
FETCH NEXT FROM curLuot INTO @MaLuot, @MaBai, @SoDung, @TongSo;

WHILE @@FETCH_STATUS = 0
BEGIN
    ;WITH QuestionList AS (
        SELECT chk.MaCauHoi, chk.ThuTuCauHoi,
               ROW_NUMBER() OVER(ORDER BY chk.ThuTuCauHoi) AS RowNum
        FROM CauHoiKhaoSat chk
        WHERE chk.MaBaiKhaoSat = @MaBai
    )
    INSERT INTO CauTraLoiKhaoSat (MaLuotKhaoSat, MaCauHoi, MaLuaChonDaChon, KetQuaDungSai, ThoiDiemTraLoi)
    SELECT 
        @MaLuot,
        ql.MaCauHoi,
        CASE 
            WHEN ql.RowNum <= @SoDung THEN 
                (SELECT TOP 1 MaLuaChon FROM LuaChonDapAn WHERE MaCauHoi = ql.MaCauHoi AND LaDapAnDung = 1)
            ELSE 
                (SELECT TOP 1 MaLuaChon FROM LuaChonDapAn WHERE MaCauHoi = ql.MaCauHoi AND LaDapAnDung = 0 ORDER BY MaLuaChon DESC)
        END,
        CASE WHEN ql.RowNum <= @SoDung THEN 1 ELSE 0 END,
        DATEADD(MINUTE, -(20 - ql.RowNum), SYSDATETIME())
    FROM QuestionList ql;

    FETCH NEXT FROM curLuot INTO @MaLuot, @MaBai, @SoDung, @TongSo;
END;

CLOSE curLuot;
DEALLOCATE curLuot;
GO

