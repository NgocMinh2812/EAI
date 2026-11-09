using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class TuVungDAL
    {
        private static TuVungDAL? _instance;
        public static TuVungDAL Instance => _instance ??= new TuVungDAL();

        public List<FlashcardDTO> LayDanhSachTuVungFlashcard(int maNguoiDung = 0, string? trinhDo = null)
        {
            var list = new List<FlashcardDTO>();

            string query = @"
                SELECT tv.MaTuVung, tv.Tu, tv.LoaiTu, tv.PhienAmIPA, tv.Nghia, tv.CauViDu,
                       tv.DuongDanAudio, tv.CumTuDiKem, td.MaCapDo, cd.TenChuDe,
                       ISNULL(fc.MaFlashcard, 0) AS MaFlashcard,
                       ISNULL(fc.SoLanOnTap, 0) AS SoLanOnTap,
                       ISNULL(fc.SoLanDung, 0) AS SoLanDung,
                       fc.LanOnTapCuoi
                FROM TuVung tv
                LEFT JOIN TrinhDo td ON tv.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON tv.MaChuDe = cd.MaChuDe
                LEFT JOIN Flashcard fc ON tv.MaTuVung = fc.MaTuVung AND fc.MaNguoiDung = @MaND
                WHERE (@TrinhDo IS NULL OR td.MaCapDo = @TrinhDo)
                ORDER BY tv.MaTuVung ASC";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@TrinhDo", (object?)trinhDo ?? DBNull.Value)
            };

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new FlashcardDTO
                    {
                        MaFlashcard = Convert.ToInt32(row["MaFlashcard"]),
                        MaNguoiDung = maNguoiDung,
                        MaTuVung = Convert.ToInt32(row["MaTuVung"]),
                        Tu = row["Tu"]?.ToString() ?? "",
                        LoaiTu = row["LoaiTu"]?.ToString() ?? "",
                        PhienAmIPA = row["PhienAmIPA"]?.ToString() ?? "",
                        Nghia = row["Nghia"]?.ToString() ?? "",
                        CauViDu = row["CauViDu"]?.ToString() ?? "",
                        DuongDanAudio = row["DuongDanAudio"] == DBNull.Value ? null : row["DuongDanAudio"]?.ToString(),
                        CumTuDiKem = row["CumTuDiKem"]?.ToString() ?? "",
                        MaCapDo = row["MaCapDo"]?.ToString() ?? "B2",
                        TenChuDe = row["TenChuDe"]?.ToString() ?? "Từ vựng chung",
                        SoLanOnTap = Convert.ToInt32(row["SoLanOnTap"]),
                        SoLanDung = Convert.ToInt32(row["SoLanDung"]),
                        LanOnTapCuoi = row["LanOnTapCuoi"] == DBNull.Value ? null : Convert.ToDateTime(row["LanOnTapCuoi"])
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TuVungDAL] Lỗi lấy danh sách flashcard: {ex.Message}");
            }

            return list;
        }

        public void CapNhatKetQuaFlashcard(int maNguoiDung, int maTuVung, bool daNho)
        {
            if (maNguoiDung < 0 || maTuVung <= 0) return;

            string query = @"
                IF EXISTS (SELECT 1 FROM Flashcard WHERE MaNguoiDung = @MaND AND MaTuVung = @MaTV)
                BEGIN
                    UPDATE Flashcard
                    SET SoLanOnTap = SoLanOnTap + 1,
                        SoLanDung = CASE WHEN @DaNho = 1 THEN SoLanDung + 1 ELSE 0 END,
                        LanOnTapCuoi = SYSDATETIME()
                    WHERE MaNguoiDung = @MaND AND MaTuVung = @MaTV;
                END
                ELSE
                BEGIN
                    INSERT INTO Flashcard (MaNguoiDung, MaTuVung, SoLanOnTap, SoLanDung, LanOnTapCuoi)
                    VALUES (@MaND, @MaTV, 1, CASE WHEN @DaNho = 1 THEN 1 ELSE 0 END, SYSDATETIME());
                END";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@MaTV", maTuVung),
                new SqlParameter("@DaNho", daNho ? 1 : 0)
            };

            try
            {
                SqlDataProvider.Instance.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TuVungDAL] Lỗi lưu Flashcard: {ex.Message}");
            }
        }

        public bool ThemTuVungVaFlashcard(TuVungDTO tuVung, int maNguoiDung, out int maTuVungMoi, out int maFlashcardMoi)
        {
            maTuVungMoi = 0;
            maFlashcardMoi = 0;
            if (string.IsNullOrWhiteSpace(tuVung.Tu) || string.IsNullOrWhiteSpace(tuVung.Nghia))
                return false;

            string sql = @"
                DECLARE @MaTV INT = 0;
                SELECT @MaTV = MaTuVung FROM TuVung WHERE LOWER(LTRIM(RTRIM(Tu))) = LOWER(LTRIM(RTRIM(@Tu)));

                IF @MaTV = 0 OR @MaTV IS NULL
                BEGIN
                    INSERT INTO TuVung (MaChuDe, MaTrinhDo, Tu, LoaiTu, PhienAmIPA, Nghia, CauViDu, DuongDanAudio, CumTuDiKem, NgayTao)
                    VALUES (@MaChuDe, @MaTrinhDo, @Tu, @LoaiTu, @PhienAmIPA, @Nghia, @CauViDu, @DuongDanAudio, @CumTuDiKem, SYSDATETIME());
                    SET @MaTV = CAST(SCOPE_IDENTITY() AS INT);
                END
                ELSE
                BEGIN
                    UPDATE TuVung
                    SET LoaiTu = CASE WHEN @LoaiTu IS NOT NULL AND @LoaiTu <> '' THEN @LoaiTu ELSE LoaiTu END,
                        PhienAmIPA = CASE WHEN @PhienAmIPA IS NOT NULL AND @PhienAmIPA <> '' THEN @PhienAmIPA ELSE PhienAmIPA END,
                        Nghia = @Nghia,
                        CauViDu = CASE WHEN @CauViDu IS NOT NULL AND @CauViDu <> '' THEN @CauViDu ELSE CauViDu END,
                        MaChuDe = ISNULL(@MaChuDe, MaChuDe),
                        MaTrinhDo = ISNULL(@MaTrinhDo, MaTrinhDo)
                    WHERE MaTuVung = @MaTV;
                END

                DECLARE @MaFC INT = 0;
                IF @MaND >= 0
                BEGIN
                    SELECT @MaFC = MaFlashcard FROM Flashcard WHERE MaNguoiDung = @MaND AND MaTuVung = @MaTV;

                    IF @MaFC = 0 OR @MaFC IS NULL
                    BEGIN
                        INSERT INTO Flashcard (MaNguoiDung, MaTuVung, SoLanOnTap, SoLanDung, NgayOnTapTiepTheo, LanOnTapCuoi)
                        VALUES (@MaND, @MaTV, 0, 0, DATEADD(day, 1, SYSDATETIME()), SYSDATETIME());
                        SET @MaFC = CAST(SCOPE_IDENTITY() AS INT);
                    END
                END

                SELECT ISNULL(@MaTV, 0) AS MaTuVung, ISNULL(@MaFC, 0) AS MaFlashcard;";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@Tu", tuVung.Tu.Trim()),
                new SqlParameter("@LoaiTu", (object?)tuVung.LoaiTu ?? DBNull.Value),
                new SqlParameter("@PhienAmIPA", (object?)tuVung.PhienAmIPA ?? DBNull.Value),
                new SqlParameter("@Nghia", tuVung.Nghia.Trim()),
                new SqlParameter("@CauViDu", (object?)tuVung.CauViDu ?? DBNull.Value),
                new SqlParameter("@DuongDanAudio", (object?)tuVung.DuongDanAudio ?? DBNull.Value),
                new SqlParameter("@CumTuDiKem", (object?)tuVung.CumTuDiKem ?? DBNull.Value),
                new SqlParameter("@MaChuDe", (object?)tuVung.MaChuDe ?? DBNull.Value),
                new SqlParameter("@MaTrinhDo", (object?)tuVung.MaTrinhDo ?? DBNull.Value),
                new SqlParameter("@MaND", maNguoiDung)
            };

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(sql, parameters);
                if (dt.Rows.Count > 0)
                {
                    maTuVungMoi = Convert.ToInt32(dt.Rows[0]["MaTuVung"]);
                    maFlashcardMoi = Convert.ToInt32(dt.Rows[0]["MaFlashcard"]);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TuVungDAL] Lỗi thêm từ vựng/flashcard: {ex.Message}");
            }
            return false;
        }

        public FlashcardDTO? LayTuVungHomNay()
        {
            string query = @"
                SELECT TOP 1 tv.MaTuVung, tv.Tu, tv.LoaiTu, tv.PhienAmIPA, tv.Nghia, tv.CauViDu,
                       tv.DuongDanAudio, tv.CumTuDiKem, td.MaCapDo, cd.TenChuDe,
                       ISNULL(fc.MaFlashcard, 0) AS MaFlashcard,
                       ISNULL(fc.SoLanOnTap, 0) AS SoLanOnTap,
                       ISNULL(fc.SoLanDung, 0) AS SoLanDung,
                       fc.LanOnTapCuoi
                FROM TuVung tv
                LEFT JOIN TrinhDo td ON tv.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON tv.MaChuDe = cd.MaChuDe
                LEFT JOIN Flashcard fc ON tv.MaTuVung = fc.MaTuVung
                ORDER BY tv.NgayTao DESC, tv.MaTuVung DESC";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return new FlashcardDTO
                    {
                        MaFlashcard = Convert.ToInt32(row["MaFlashcard"]),
                        MaTuVung = Convert.ToInt32(row["MaTuVung"]),
                        Tu = row["Tu"]?.ToString() ?? "",
                        LoaiTu = row["LoaiTu"]?.ToString() ?? "",
                        PhienAmIPA = row["PhienAmIPA"]?.ToString() ?? "",
                        Nghia = row["Nghia"]?.ToString() ?? "",
                        CauViDu = row["CauViDu"]?.ToString() ?? "",
                        DuongDanAudio = row["DuongDanAudio"] == DBNull.Value ? null : row["DuongDanAudio"]?.ToString(),
                        CumTuDiKem = row["CumTuDiKem"]?.ToString() ?? "",
                        MaCapDo = row["MaCapDo"]?.ToString() ?? "B2",
                        TenChuDe = row["TenChuDe"]?.ToString() ?? "Từ vựng chung",
                        SoLanOnTap = Convert.ToInt32(row["SoLanOnTap"]),
                        SoLanDung = Convert.ToInt32(row["SoLanDung"]),
                        LanOnTapCuoi = row["LanOnTapCuoi"] == DBNull.Value ? null : Convert.ToDateTime(row["LanOnTapCuoi"])
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TuVungDAL] Lỗi lấy từ vựng hôm nay: {ex.Message}");
            }
            return null;
        }

        public List<CauHoiKhaoSatDTO> LayCauHoiLuyenTap(string? kyNang = null, string? trinhDo = null, int top = 10)
        {
            var list = new List<CauHoiKhaoSatDTO>();

            string query = @"
                SELECT TOP (@Top) ch.MaCauHoi, ROW_NUMBER() OVER(ORDER BY ch.MaCauHoi ASC) AS ThuTuCauHoi,
                       ch.KyNang, td.MaCapDo AS MaTrinhDo, td.TenTrinhDo, ch.NoiDungCauHoi,
                       ch.DoanVanDoc, ch.DuongDanAudio, ch.GiaiThichDapAn
                FROM CauHoi ch
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                WHERE (@KyNang IS NULL OR ch.KyNang = @KyNang)
                  AND (@TrinhDo IS NULL OR td.MaCapDo = @TrinhDo)
                  AND ch.DaKiemDuyet = 1
                ORDER BY ch.MaCauHoi ASC";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@Top", top),
                new SqlParameter("@KyNang", (object?)kyNang ?? DBNull.Value),
                new SqlParameter("@TrinhDo", (object?)trinhDo ?? DBNull.Value)
            };

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
                int thuTu = 1;
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                        ThuTuCauHoi = thuTu++,
                        KyNang = row["KyNang"]?.ToString() ?? "NguPhap",
                        MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B1",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                        NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                        DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                        AudioPath = row["DuongDanAudio"] == DBNull.Value ? null : row["DuongDanAudio"]?.ToString(),
                        GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString()
                    });
                }

                // Nạp toàn bộ lựa chọn trong 1 truy vấn duy nhất (loại bỏ N+1 query)
                CauHoiDAL.NapDanhSachLuaChon(list);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TuVungDAL] Lỗi lấy câu hỏi luyện tập: {ex.Message}");
            }

            // Nếu lọc theo kỹ năng Nghe mà DB chưa có dữ liệu câu hỏi nghe -> Sinh bộ câu hỏi nghe chuẩn hóa
            if (list.Count == 0 && (string.Equals(kyNang, "Nghe", StringComparison.OrdinalIgnoreCase) || string.Equals(kyNang, "Listening", StringComparison.OrdinalIgnoreCase)))
            {
                list = TaoBoCauHoiLuyenNgheChuan();
            }

            return list;
        }

        public static List<CauHoiKhaoSatDTO> TaoBoCauHoiLuyenNgheChuan()
        {
            return new List<CauHoiKhaoSatDTO>
            {
                new()
                {
                    MaCauHoi = 9001,
                    ThuTuCauHoi = 1,
                    KyNang = "Nghe",
                    MaTrinhDo = "B1",
                    NoiDungCauHoi = "Where is the conversation most likely taking place?",
                    DoanVanDoc = "Audio Transcript:\n\"Good afternoon, sir. Are you checking in today? Yes, I have a reservation under the name Anderson. May I see your passport and credit card, please? Here you go. Your room 402 on the fourth floor is ready. Breakfast is served from 7 to 10 AM in the restaurant.\"",
                    GiaiThichDapAn = "Người nói dùng các từ 'checking in', 'reservation', 'room 402', 'breakfast' -> Địa điểm diễn ra là Khách sạn (Hotel).",
                    DanhSachLuaChon = new List<LuaChonDapAnDTO>
                    {
                        new() { MaLuaChon = 90011, KyHieuLuaChon = "A", NoiDungLuaChon = "At a hotel front desk", LaDapAnDung = true },
                        new() { MaLuaChon = 90012, KyHieuLuaChon = "B", NoiDungLuaChon = "At an airport gate", LaDapAnDung = false },
                        new() { MaLuaChon = 90013, KyHieuLuaChon = "C", NoiDungLuaChon = "At a train station", LaDapAnDung = false },
                        new() { MaLuaChon = 90014, KyHieuLuaChon = "D", NoiDungLuaChon = "At a library", LaDapAnDung = false }
                    }
                },
                new()
                {
                    MaCauHoi = 9002,
                    ThuTuCauHoi = 2,
                    KyNang = "Nghe",
                    MaTrinhDo = "B1",
                    NoiDungCauHoi = "What time will the product launch meeting start?",
                    DoanVanDoc = "Audio Transcript:\n\"Attention all marketing team members. Due to the client's flight delay, our quarterly product launch meeting originally scheduled for 2:00 PM has been pushed back to 3:30 PM in Conference Room B. Please be punctual.\"",
                    GiaiThichDapAn = "Thông báo nêu rõ cuộc họp bị hoãn từ 2:00 PM sang 3:30 PM: 'has been pushed back to 3:30 PM'.",
                    DanhSachLuaChon = new List<LuaChonDapAnDTO>
                    {
                        new() { MaLuaChon = 90021, KyHieuLuaChon = "A", NoiDungLuaChon = "2:00 PM", LaDapAnDung = false },
                        new() { MaLuaChon = 90022, KyHieuLuaChon = "B", NoiDungLuaChon = "3:30 PM", LaDapAnDung = true },
                        new() { MaLuaChon = 90023, KyHieuLuaChon = "C", NoiDungLuaChon = "4:00 PM", LaDapAnDung = false },
                        new() { MaLuaChon = 90024, KyHieuLuaChon = "D", NoiDungLuaChon = "1:30 PM", LaDapAnDung = false }
                    }
                },
                new()
                {
                    MaCauHoi = 9003,
                    ThuTuCauHoi = 3,
                    KyNang = "Nghe",
                    MaTrinhDo = "B2",
                    NoiDungCauHoi = "What problem is the woman addressing in the broadcast?",
                    DoanVanDoc = "Audio Transcript:\n\"Hello listeners. Welcome back to Tech Today. Recent cybersecurity investigations revealed that thousands of smart home devices are vulnerable to unauthorized access because consumers neglect to change default factory passwords. Today we have an expert to discuss solutions.\"",
                    GiaiThichDapAn = "Đoạn phát thanh nhấn mạnh lỗ hổng bảo mật của thiết bị nhà thông minh do không đổi mật khẩu mặc định (Security vulnerabilities of smart home devices).",
                    DanhSachLuaChon = new List<LuaChonDapAnDTO>
                    {
                        new() { MaLuaChon = 90031, KyHieuLuaChon = "A", NoiDungLuaChon = "High prices of computer hardware", LaDapAnDung = false },
                        new() { MaLuaChon = 90032, KyHieuLuaChon = "B", NoiDungLuaChon = "Security vulnerabilities in smart home devices", LaDapAnDung = true },
                        new() { MaLuaChon = 90033, KyHieuLuaChon = "C", NoiDungLuaChon = "Power outages in the city", LaDapAnDung = false },
                        new() { MaLuaChon = 90034, KyHieuLuaChon = "D", NoiDungLuaChon = "Slow internet download speeds", LaDapAnDung = false }
                    }
                },
                new()
                {
                    MaCauHoi = 9004,
                    ThuTuCauHoi = 4,
                    KyNang = "Nghe",
                    MaTrinhDo = "B2",
                    NoiDungCauHoi = "What is the speaker advising passengers to do?",
                    DoanVanDoc = "Audio Transcript:\n\"May I have your attention, please. Flight VN214 bound for London is now ready for boarding at Gate 18. All passengers traveling with infants and those requiring special assistance, please proceed to the gate now.\"",
                    GiaiThichDapAn = "Người phát thanh yêu cầu hành khách đi cùng trẻ nhỏ hoặc cần hỗ trợ đặc biệt tiến ra cửa khởi hành (Board the plane at gate 18).",
                    DanhSachLuaChon = new List<LuaChonDapAnDTO>
                    {
                        new() { MaLuaChon = 90041, KyHieuLuaChon = "A", NoiDungLuaChon = "Proceed to Gate 18 for boarding", LaDapAnDung = true },
                        new() { MaLuaChon = 90042, KyHieuLuaChon = "B", NoiDungLuaChon = "Collect their luggage from Carousel 3", LaDapAnDung = false },
                        new() { MaLuaChon = 90043, KyHieuLuaChon = "C", NoiDungLuaChon = "Change their ticket reservations", LaDapAnDung = false },
                        new() { MaLuaChon = 90044, KyHieuLuaChon = "D", NoiDungLuaChon = "Exit the airport terminal", LaDapAnDung = false }
                    }
                },
                new()
                {
                    MaCauHoi = 9005,
                    ThuTuCauHoi = 5,
                    KyNang = "Nghe",
                    MaTrinhDo = "C1",
                    NoiDungCauHoi = "According to the professor, what led to the revival of renewable architecture?",
                    DoanVanDoc = "Audio Transcript:\n\"In the late twentieth century, urban planners largely disregarded ecological footprints. However, the unprecedented escalation of fossil fuel costs and acute climate awareness in the early 2000s precipitated a paradigm shift, prompting architects to reintegrate passive solar design and natural ventilation.\"",
                    GiaiThichDapAn = "Giáo sư giải thích sự tăng vọt chi phí nhiên liệu hóa thạch và nhận thức về khí hậu ('escalation of fossil fuel costs and climate awareness') đã thúc đẩy sự hồi sinh của kiến trúc tái tạo.",
                    DanhSachLuaChon = new List<LuaChonDapAnDTO>
                    {
                        new() { MaLuaChon = 90051, KyHieuLuaChon = "A", NoiDungLuaChon = "Escalating energy costs and climate awareness", LaDapAnDung = true },
                        new() { MaLuaChon = 90052, KyHieuLuaChon = "B", NoiDungLuaChon = "A decline in urban population", LaDapAnDung = false },
                        new() { MaLuaChon = 90053, KyHieuLuaChon = "C", NoiDungLuaChon = "International sports competitions", LaDapAnDung = false },
                        new() { MaLuaChon = 90054, KyHieuLuaChon = "D", NoiDungLuaChon = "The invention of artificial lighting", LaDapAnDung = false }
                    }
                }
            };
        }
    }
}
