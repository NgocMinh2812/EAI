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
            if (maNguoiDung <= 0 || maTuVung <= 0) return;

            string query = @"
                IF EXISTS (SELECT 1 FROM Flashcard WHERE MaNguoiDung = @MaND AND MaTuVung = @MaTV)
                BEGIN
                    UPDATE Flashcard
                    SET SoLanOnTap = SoLanOnTap + 1,
                        SoLanDung = SoLanDung + CASE WHEN @DaNho = 1 THEN 1 ELSE 0 END,
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

        public List<CauHoiKhaoSatDTO> LayCauHoiLuyenTap(string? kyNang = null, string? trinhDo = null, int top = 10)
        {
            var list = new List<CauHoiKhaoSatDTO>();

            string query = @"
                SELECT TOP (@Top) ch.MaCauHoi, ROW_NUMBER() OVER(ORDER BY ch.MaCauHoi ASC) AS ThuTuCauHoi,
                       ch.KyNang, td.MaCapDo AS MaTrinhDo, td.TenTrinhDo, ch.NoiDungCauHoi,
                       ch.DoanVanDoc, ch.GiaiThichDapAn
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
                    int maCH = Convert.ToInt32(row["MaCauHoi"]);
                    var q = new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = maCH,
                        ThuTuCauHoi = thuTu++,
                        KyNang = row["KyNang"]?.ToString() ?? "NguPhap",
                        MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B1",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                        NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                        DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                        GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString()
                    };

                    string queryLC = "SELECT MaLuaChon, MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung FROM LuaChonDapAn WHERE MaCauHoi = @MaCauHoi ORDER BY KyHieuLuaChon ASC";
                    DataTable dtLC = SqlDataProvider.Instance.ExecuteQuery(queryLC, new[] { new SqlParameter("@MaCauHoi", maCH) });
                    foreach (DataRow rLC in dtLC.Rows)
                    {
                        q.DanhSachLuaChon.Add(new LuaChonDapAnDTO
                        {
                            MaLuaChon = Convert.ToInt32(rLC["MaLuaChon"]),
                            MaCauHoi = Convert.ToInt32(rLC["MaCauHoi"]),
                            KyHieuLuaChon = rLC["KyHieuLuaChon"]?.ToString() ?? "",
                            NoiDungLuaChon = rLC["NoiDungLuaChon"]?.ToString() ?? "",
                            LaDapAnDung = Convert.ToBoolean(rLC["LaDapAnDung"])
                        });
                    }

                    list.Add(q);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TuVungDAL] Lỗi lấy câu hỏi luyện tập: {ex.Message}");
            }

            return list;
        }
    }
}
