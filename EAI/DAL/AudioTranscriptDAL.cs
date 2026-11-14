using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class AudioTranscriptDAL
    {
        private static AudioTranscriptDAL? _instance;
        public static AudioTranscriptDAL Instance => _instance ??= new AudioTranscriptDAL();

        /// <summary>
        /// Delegate kiểm tra quyền giảng viên được gán từ tầng BUS. Mặc định false (Fail-safe Least Privilege).
        /// </summary>
        public static Func<bool>? RoleCheckGiangVien { get; set; }

        private static bool KiemTraQuyenGiangVien(bool? laGiangVien)
        {
            if (laGiangVien.HasValue) return laGiangVien.Value;
            return RoleCheckGiangVien?.Invoke() ?? false;
        }

        public List<AudioTranscriptDTO> LayTheoMaBaiHoc(int maBaiHoc, bool? laGiangVien = null)
        {
            var list = new List<AudioTranscriptDTO>();
            bool isTeacher = KiemTraQuyenGiangVien(laGiangVien);
            try
            {
                const string query = @"
                    SELECT MaAudioTranscript, MaBaiHoc, AudioPath, LoaiAudio, TieuDeAudio,
                           CASE WHEN ChoPhepSinhVienXem = 1 OR @LaGiangVien = 1 THEN ScriptGoc ELSE '' END AS ScriptGoc,
                           CASE WHEN ChoPhepSinhVienXem = 1 OR @LaGiangVien = 1 THEN TranscriptText ELSE '' END AS TranscriptText,
                           CASE WHEN ChoPhepSinhVienXem = 1 OR @LaGiangVien = 1 THEN TranscriptSegmentsJson ELSE NULL END AS TranscriptSegmentsJson,
                           Provider, MoHinhSTT, MoHinhTTS,
                           VoiceName, AudioHash, TrangThaiDuyet, ChoPhepSinhVienXem, ThoiLuongGiay, 
                           DungLuongBytes, NguoiTao, NgayTao, NgayCapNhat
                    FROM BaiHocAudioTranscript
                    WHERE MaBaiHoc = @MaBaiHoc
                    ORDER BY CASE WHEN TrangThaiDuyet = N'DaDuyet' THEN 0 ELSE 1 END, NgayCapNhat DESC;";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, new[] { 
                    new SqlParameter("@MaBaiHoc", maBaiHoc),
                    new SqlParameter("@LaGiangVien", isTeacher ? 1 : 0)
                });
                foreach (DataRow r in dt.Rows)
                {
                    list.Add(MapRowToDTO(r, isTeacher));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AudioTranscriptDAL.LayTheoMaBaiHoc] Lỗi: {ex.Message}");
            }
            return list;
        }

        public AudioTranscriptDTO? LayAudioChinhCuaBaiHoc(int maBaiHoc, bool? laGiangVien = null)
        {
            var all = LayTheoMaBaiHoc(maBaiHoc, laGiangVien);
            return all.Count > 0 ? all[0] : null;
        }

        public AudioTranscriptDTO? LayTheoAudioHash(string audioHash, bool? laGiangVien = null)
        {
            if (string.IsNullOrWhiteSpace(audioHash)) return null;
            bool isTeacher = KiemTraQuyenGiangVien(laGiangVien);
            try
            {
                const string query = @"
                    SELECT TOP 1 MaAudioTranscript, MaBaiHoc, AudioPath, LoaiAudio, TieuDeAudio,
                           CASE WHEN ChoPhepSinhVienXem = 1 OR @LaGiangVien = 1 THEN ScriptGoc ELSE '' END AS ScriptGoc,
                           CASE WHEN ChoPhepSinhVienXem = 1 OR @LaGiangVien = 1 THEN TranscriptText ELSE '' END AS TranscriptText,
                           CASE WHEN ChoPhepSinhVienXem = 1 OR @LaGiangVien = 1 THEN TranscriptSegmentsJson ELSE NULL END AS TranscriptSegmentsJson,
                           Provider, MoHinhSTT, MoHinhTTS,
                           VoiceName, AudioHash, TrangThaiDuyet, ChoPhepSinhVienXem, ThoiLuongGiay, 
                           DungLuongBytes, NguoiTao, NgayTao, NgayCapNhat
                    FROM BaiHocAudioTranscript
                    WHERE AudioHash = @AudioHash
                    ORDER BY CASE WHEN TrangThaiDuyet = N'DaDuyet' THEN 0 ELSE 1 END, NgayCapNhat DESC;";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, new[] { 
                    new SqlParameter("@AudioHash", audioHash.Trim()),
                    new SqlParameter("@LaGiangVien", isTeacher ? 1 : 0)
                });
                if (dt.Rows.Count > 0)
                {
                    return MapRowToDTO(dt.Rows[0], isTeacher);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AudioTranscriptDAL.LayTheoAudioHash] Lỗi: {ex.Message}");
            }
            return null;
        }

        public AudioTranscriptDTO? LayTheoDuongDanAudio(string audioPath, bool? laGiangVien = null)
        {
            if (string.IsNullOrWhiteSpace(audioPath)) return null;
            bool isTeacher = KiemTraQuyenGiangVien(laGiangVien);
            try
            {
                const string query = @"
                    SELECT TOP 1 MaAudioTranscript, MaBaiHoc, AudioPath, LoaiAudio, TieuDeAudio,
                           CASE WHEN ChoPhepSinhVienXem = 1 OR @LaGiangVien = 1 THEN ScriptGoc ELSE '' END AS ScriptGoc,
                           CASE WHEN ChoPhepSinhVienXem = 1 OR @LaGiangVien = 1 THEN TranscriptText ELSE '' END AS TranscriptText,
                           CASE WHEN ChoPhepSinhVienXem = 1 OR @LaGiangVien = 1 THEN TranscriptSegmentsJson ELSE NULL END AS TranscriptSegmentsJson,
                           Provider, MoHinhSTT, MoHinhTTS,
                           VoiceName, AudioHash, TrangThaiDuyet, ChoPhepSinhVienXem, ThoiLuongGiay, 
                           DungLuongBytes, NguoiTao, NgayTao, NgayCapNhat
                    FROM BaiHocAudioTranscript
                    WHERE AudioPath = @AudioPath
                    ORDER BY CASE WHEN TrangThaiDuyet = N'DaDuyet' THEN 0 ELSE 1 END, NgayCapNhat DESC;";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, new[] { 
                    new SqlParameter("@AudioPath", audioPath.Trim()),
                    new SqlParameter("@LaGiangVien", isTeacher ? 1 : 0)
                });
                if (dt.Rows.Count > 0)
                {
                    return MapRowToDTO(dt.Rows[0], isTeacher);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AudioTranscriptDAL.LayTheoDuongDanAudio] Lỗi: {ex.Message}");
            }
            return null;
        }

        public int LuuAudioTranscript(AudioTranscriptDTO dto)
        {
            if (dto == null) return 0;
            try
            {
                if (dto.MaAudioTranscript > 0)
                {
                    const string updateSql = @"
                        UPDATE BaiHocAudioTranscript
                        SET MaBaiHoc = @MaBaiHoc,
                            AudioPath = @AudioPath,
                            LoaiAudio = @LoaiAudio,
                            TieuDeAudio = @TieuDeAudio,
                            ScriptGoc = @ScriptGoc,
                            TranscriptText = @TranscriptText,
                            TranscriptSegmentsJson = @TranscriptSegmentsJson,
                            Provider = @Provider,
                            MoHinhSTT = @MoHinhSTT,
                            MoHinhTTS = @MoHinhTTS,
                            VoiceName = @VoiceName,
                            AudioHash = @AudioHash,
                            TrangThaiDuyet = @TrangThaiDuyet,
                            ChoPhepSinhVienXem = @ChoPhepSinhVienXem,
                            ThoiLuongGiay = @ThoiLuongGiay,
                            DungLuongBytes = @DungLuongBytes,
                            NgayCapNhat = SYSDATETIME()
                        WHERE MaAudioTranscript = @MaAudioTranscript;";

                    DatabaseHelper.ExecuteNonQuery(updateSql, GetSqlParameters(dto, true));
                    return dto.MaAudioTranscript;
                }
                else
                {
                    const string insertSql = @"
                        INSERT INTO BaiHocAudioTranscript (
                            MaBaiHoc, AudioPath, LoaiAudio, TieuDeAudio, ScriptGoc, TranscriptText, 
                            TranscriptSegmentsJson, Provider, MoHinhSTT, MoHinhTTS, VoiceName, 
                            AudioHash, TrangThaiDuyet, ChoPhepSinhVienXem, ThoiLuongGiay, 
                            DungLuongBytes, NguoiTao, NgayTao, NgayCapNhat
                        )
                        VALUES (
                            @MaBaiHoc, @AudioPath, @LoaiAudio, @TieuDeAudio, @ScriptGoc, @TranscriptText, 
                            @TranscriptSegmentsJson, @Provider, @MoHinhSTT, @MoHinhTTS, @VoiceName, 
                            @AudioHash, @TrangThaiDuyet, @ChoPhepSinhVienXem, @ThoiLuongGiay, 
                            @DungLuongBytes, @NguoiTao, SYSDATETIME(), SYSDATETIME()
                        );
                        SELECT SCOPE_IDENTITY();";

                    object res = DatabaseHelper.ExecuteScalar(insertSql, GetSqlParameters(dto, false));
                    if (res != null && int.TryParse(res.ToString(), out int newId))
                    {
                        dto.MaAudioTranscript = newId;
                        return newId;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AudioTranscriptDAL.LuuAudioTranscript] Lỗi: {ex.Message}");
            }
            return 0;
        }

        public bool CapNhatTrangThaiDuyet(int maId, string trangThai, bool choPhepXem)
        {
            try
            {
                const string query = @"
                    UPDATE BaiHocAudioTranscript
                    SET TrangThaiDuyet = @TrangThai,
                        ChoPhepSinhVienXem = @ChoPhepXem,
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaAudioTranscript = @MaId;";

                int rows = DatabaseHelper.ExecuteNonQuery(query, new[]
                {
                    new SqlParameter("@TrangThai", trangThai),
                    new SqlParameter("@ChoPhepXem", choPhepXem),
                    new SqlParameter("@MaId", maId)
                });
                return rows > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AudioTranscriptDAL.CapNhatTrangThaiDuyet] Lỗi: {ex.Message}");
                return false;
            }
        }

        public bool CapNhatTranscriptText(int maId, string text, string segmentsJson)
        {
            try
            {
                const string query = @"
                    UPDATE BaiHocAudioTranscript
                    SET TranscriptText = @Text,
                        TranscriptSegmentsJson = @SegmentsJson,
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaAudioTranscript = @MaId;";

                int rows = DatabaseHelper.ExecuteNonQuery(query, new[]
                {
                    new SqlParameter("@Text", text ?? string.Empty),
                    new SqlParameter("@SegmentsJson", (object?)segmentsJson ?? DBNull.Value),
                    new SqlParameter("@MaId", maId)
                });
                return rows > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AudioTranscriptDAL.CapNhatTranscriptText] Lỗi: {ex.Message}");
                return false;
            }
        }

        private static SqlParameter[] GetSqlParameters(AudioTranscriptDTO dto, bool includeId)
        {
            var pList = new List<SqlParameter>
            {
                new SqlParameter("@MaBaiHoc", dto.MaBaiHoc.HasValue ? (object)dto.MaBaiHoc.Value : DBNull.Value),
                new SqlParameter("@AudioPath", dto.AudioPath ?? string.Empty),
                new SqlParameter("@LoaiAudio", dto.LoaiAudio ?? "Uploaded"),
                new SqlParameter("@TieuDeAudio", (object?)dto.TieuDeAudio ?? DBNull.Value),
                new SqlParameter("@ScriptGoc", (object?)dto.ScriptGoc ?? DBNull.Value),
                new SqlParameter("@TranscriptText", (object?)dto.TranscriptText ?? DBNull.Value),
                new SqlParameter("@TranscriptSegmentsJson", (object?)dto.TranscriptSegmentsJson ?? DBNull.Value),
                new SqlParameter("@Provider", dto.Provider ?? "Groq"),
                new SqlParameter("@MoHinhSTT", (object?)dto.MoHinhSTT ?? DBNull.Value),
                new SqlParameter("@MoHinhTTS", (object?)dto.MoHinhTTS ?? DBNull.Value),
                new SqlParameter("@VoiceName", (object?)dto.VoiceName ?? DBNull.Value),
                new SqlParameter("@AudioHash", (object?)dto.AudioHash ?? DBNull.Value),
                new SqlParameter("@TrangThaiDuyet", dto.TrangThaiDuyet ?? "ChoDuyet"),
                new SqlParameter("@ChoPhepSinhVienXem", dto.ChoPhepSinhVienXem),
                new SqlParameter("@ThoiLuongGiay", dto.ThoiLuongGiay.HasValue ? (object)dto.ThoiLuongGiay.Value : DBNull.Value),
                new SqlParameter("@DungLuongBytes", dto.DungLuongBytes.HasValue ? (object)dto.DungLuongBytes.Value : DBNull.Value),
                new SqlParameter("@NguoiTao", dto.NguoiTao.HasValue ? (object)dto.NguoiTao.Value : DBNull.Value)
            };

            if (includeId)
            {
                pList.Add(new SqlParameter("@MaAudioTranscript", dto.MaAudioTranscript));
            }

            return pList.ToArray();
        }

        private static AudioTranscriptDTO MapRowToDTO(DataRow r) => MapRowToDTO(r, KiemTraQuyenGiangVien(null));

        private static AudioTranscriptDTO MapRowToDTO(DataRow r, bool isTeacher)
        {
            bool choPhepSinhVienXem = r["ChoPhepSinhVienXem"] != DBNull.Value && Convert.ToBoolean(r["ChoPhepSinhVienXem"]);
            bool canView = isTeacher || choPhepSinhVienXem;

            string? rawScript = r["ScriptGoc"] == DBNull.Value ? null : r["ScriptGoc"].ToString();
            string? rawText = r["TranscriptText"] == DBNull.Value ? null : r["TranscriptText"].ToString();
            string? rawSegments = r["TranscriptSegmentsJson"] == DBNull.Value ? null : r["TranscriptSegmentsJson"].ToString();

            var dto = new AudioTranscriptDTO
            {
                MaAudioTranscript = Convert.ToInt32(r["MaAudioTranscript"]),
                MaBaiHoc = r["MaBaiHoc"] == DBNull.Value ? null : Convert.ToInt32(r["MaBaiHoc"]),
                AudioPath = r["AudioPath"]?.ToString() ?? string.Empty,
                LoaiAudio = r["LoaiAudio"]?.ToString() ?? "Uploaded",
                TieuDeAudio = r["TieuDeAudio"] == DBNull.Value ? null : r["TieuDeAudio"].ToString(),
                ScriptGoc = canView ? (rawScript ?? string.Empty) : string.Empty,
                TranscriptText = canView ? (rawText ?? string.Empty) : string.Empty,
                TranscriptSegmentsJson = canView ? rawSegments : null,
                Provider = r["Provider"]?.ToString() ?? "Groq",
                MoHinhSTT = r["MoHinhSTT"] == DBNull.Value ? null : r["MoHinhSTT"].ToString(),
                MoHinhTTS = r["MoHinhTTS"] == DBNull.Value ? null : r["MoHinhTTS"].ToString(),
                VoiceName = r["VoiceName"] == DBNull.Value ? null : r["VoiceName"].ToString(),
                AudioHash = r["AudioHash"] == DBNull.Value ? null : r["AudioHash"].ToString(),
                TrangThaiDuyet = r["TrangThaiDuyet"]?.ToString() ?? "ChoDuyet",
                ChoPhepSinhVienXem = choPhepSinhVienXem,
                ThoiLuongGiay = r["ThoiLuongGiay"] == DBNull.Value ? null : Convert.ToDouble(r["ThoiLuongGiay"]),
                DungLuongBytes = r["DungLuongBytes"] == DBNull.Value ? null : Convert.ToInt64(r["DungLuongBytes"]),
                NguoiTao = r["NguoiTao"] == DBNull.Value ? null : Convert.ToInt32(r["NguoiTao"]),
                NgayTao = r["NgayTao"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(r["NgayTao"]),
                NgayCapNhat = r["NgayCapNhat"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(r["NgayCapNhat"])
            };

            if (!canView)
            {
                dto.Segments = new List<TranscriptSegmentItemDTO>();
            }

            return dto;
        }
    }
}

