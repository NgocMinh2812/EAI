using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO.Assistant;

namespace DAL.Assistant
{
    /// <summary>
    /// Data Access Layer cho phân hệ Trợ lý AI EAI.
    /// Quản lý persistence cho AIAssistantConversation và AIAssistantMessage với 100% Parameterized SQL.
    /// </summary>
    public class AIAssistantDAL
    {
        private static AIAssistantDAL? _instance;
        public static AIAssistantDAL Instance => _instance ??= new AIAssistantDAL();

        /// <summary>
        /// Tạo mới một cuộc hội thoại trong CSDL.
        /// </summary>
        /// <param name="conv">DTO thông tin hội thoại</param>
        /// <returns>Mã cuộc hội thoại mới sinh (MaHoiThoai)</returns>
        public int TaoHoiThoai(AIAssistantConversationDTO conv)
        {
            if (conv == null) throw new ArgumentNullException(nameof(conv));
            if (conv.MaNguoiDung <= 0) return 0;

            string safeTitle = string.IsNullOrWhiteSpace(conv.TieuDe) ? "Cuộc trò chuyện mới" : conv.TieuDe.Trim();
            if (safeTitle.Length > 250)
            {
                safeTitle = safeTitle.Substring(0, 247).Trim() + "...";
            }

            string query = @"
                INSERT INTO AIAssistantConversation 
                    (MaNguoiDung, TieuDe, NgayTao, NgayCapNhat, TrangThai, TongSoTinNhan, MoHinhUuTien, PromptHeThongTuyChon)
                VALUES 
                    (@MaNguoiDung, @TieuDe, SYSDATETIME(), SYSDATETIME(), @TrangThai, @TongSoTinNhan, @MoHinhUuTien, @PromptHeThongTuyChon);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parameters = {
                new SqlParameter("@MaNguoiDung", conv.MaNguoiDung),
                new SqlParameter("@TieuDe", SqlDbType.NVarChar, 255) { Value = safeTitle },
                new SqlParameter("@TrangThai", string.IsNullOrWhiteSpace(conv.TrangThai) ? "Active" : conv.TrangThai.Trim()),
                new SqlParameter("@TongSoTinNhan", conv.TongSoTinNhan),
                new SqlParameter("@MoHinhUuTien", (object?)conv.MoHinhUuTien ?? DBNull.Value),
                new SqlParameter("@PromptHeThongTuyChon", (object?)conv.PromptHeThongTuyChon ?? DBNull.Value)
            };

            object? result = SqlDataProvider.Instance.ExecuteScalar(query, parameters);
            int newId = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
            conv.MaHoiThoai = newId;
            return newId;
        }

        /// <summary>
        /// Lấy danh sách các cuộc hội thoại của một người dùng, có lọc từ khóa và giới hạn số lượng.
        /// </summary>
        public List<AIAssistantConversationDTO> LayDanhSachHoiThoai(int maNguoiDung, string? tuKhoa = null, int top = 50)
        {
            var list = new List<AIAssistantConversationDTO>();
            if (maNguoiDung <= 0) return list;
            if (top <= 0) top = 50;

            string query = $@"
                SELECT TOP ({top}) 
                    MaHoiThoai, MaNguoiDung, TieuDe, NgayTao, NgayCapNhat, TrangThai, 
                    TongSoTinNhan, MoHinhUuTien, PromptHeThongTuyChon
                FROM AIAssistantConversation
                WHERE MaNguoiDung = @MaNguoiDung
                  AND TrangThai <> 'Deleted'";

            var paramList = new List<SqlParameter>
            {
                new SqlParameter("@MaNguoiDung", maNguoiDung)
            };

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string normalizedKw = tuKhoa.Trim();
                if (normalizedKw.Length > 200)
                {
                    normalizedKw = normalizedKw.Substring(0, 200);
                }
                query += " AND (TieuDe LIKE @TuKhoa OR PromptHeThongTuyChon LIKE @TuKhoa)";
                paramList.Add(new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 400) { Value = $"%{normalizedKw}%" });
            }

            query += " ORDER BY NgayCapNhat DESC, MaHoiThoai DESC";

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, paramList.ToArray());
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapConversation(row));
            }

            return list;
        }

        /// <summary>
        /// Lấy thông tin chi tiết một cuộc hội thoại theo ID và kiểm tra quyền sở hữu người dùng.
        /// </summary>
        public AIAssistantConversationDTO? LayHoiThoaiTheoId(int maHoiThoai, int maNguoiDung)
        {
            if (maHoiThoai <= 0 || maNguoiDung <= 0) return null;

            string query = @"
                SELECT MaHoiThoai, MaNguoiDung, TieuDe, NgayTao, NgayCapNhat, TrangThai, 
                       TongSoTinNhan, MoHinhUuTien, PromptHeThongTuyChon
                FROM AIAssistantConversation
                WHERE MaHoiThoai = @MaHoiThoai
                  AND MaNguoiDung = @MaNguoiDung
                  AND TrangThai <> 'Deleted'";

            SqlParameter[] parameters = {
                new SqlParameter("@MaHoiThoai", maHoiThoai),
                new SqlParameter("@MaNguoiDung", maNguoiDung)
            };

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapConversation(dt.Rows[0]);
            }

            return null;
        }

        /// <summary>
        /// Cập nhật tiêu đề cuộc hội thoại.
        /// </summary>
        public bool CapNhatTieuDeHoiThoai(int maHoiThoai, int maNguoiDung, string tieuDeMoi)
        {
            if (maHoiThoai <= 0 || maNguoiDung <= 0 || string.IsNullOrWhiteSpace(tieuDeMoi)) return false;

            string safeTitle = tieuDeMoi.Trim();
            if (safeTitle.Length > 250)
            {
                safeTitle = safeTitle.Substring(0, 247).Trim() + "...";
            }

            string query = @"
                UPDATE AIAssistantConversation
                SET TieuDe = @TieuDeMoi,
                    NgayCapNhat = SYSDATETIME()
                WHERE MaHoiThoai = @MaHoiThoai
                  AND MaNguoiDung = @MaNguoiDung
                  AND TrangThai <> 'Deleted'";

            SqlParameter[] parameters = {
                new SqlParameter("@MaHoiThoai", maHoiThoai),
                new SqlParameter("@MaNguoiDung", maNguoiDung),
                new SqlParameter("@TieuDeMoi", SqlDbType.NVarChar, 255) { Value = safeTitle }
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>
        /// Tự động cập nhật tiêu đề theo tin nhắn đầu tiên của user nếu tiêu đề hiện tại đang là mặc định hoặc rỗng.
        /// Đảm bảo nguyên tử: chỉ cập nhật khi TongSoTinNhan <= 1 và tiêu đề là tiêu đề mặc định.
        /// </summary>
        public bool CapNhatTieuDeTuDongLanDau(int maHoiThoai, string tieuDeMoi)
        {
            if (maHoiThoai <= 0 || string.IsNullOrWhiteSpace(tieuDeMoi)) return false;

            string safeTitle = tieuDeMoi.Trim();
            if (safeTitle.Length > 250)
            {
                safeTitle = safeTitle.Substring(0, 247).Trim() + "...";
            }

            string query = @"
                UPDATE AIAssistantConversation
                SET TieuDe = @TieuDeMoi,
                    NgayCapNhat = SYSDATETIME()
                WHERE MaHoiThoai = @MaHoiThoai
                  AND TrangThai <> 'Deleted'
                  AND TongSoTinNhan <= 1
                  AND (
                      TieuDe IS NULL 
                      OR LTRIM(RTRIM(TieuDe)) = ''
                      OR TieuDe = N'Cuộc trò chuyện mới'
                      OR TieuDe LIKE N'Cuộc trò chuyện [0-9][0-9]/[0-9][0-9] %'
                  );";

            SqlParameter[] parameters = {
                new SqlParameter("@MaHoiThoai", maHoiThoai),
                new SqlParameter("@TieuDeMoi", SqlDbType.NVarChar, 255) { Value = safeTitle }
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>
        /// Xóa cuộc hội thoại (mặc định xóa mềm bằng cách cập nhật TrangThai = 'Deleted').
        /// Nếu hardDelete = true, xóa hẳn khỏi CSDL (cascade delete sẽ xóa tin nhắn tương ứng).
        /// </summary>
        public bool XoaHoiThoai(int maHoiThoai, int maNguoiDung, bool hardDelete = false)
        {
            if (maHoiThoai <= 0 || maNguoiDung <= 0) return false;

            string query;
            if (hardDelete)
            {
                query = @"
                    DELETE FROM AIAssistantConversation
                    WHERE MaHoiThoai = @MaHoiThoai
                      AND MaNguoiDung = @MaNguoiDung";
            }
            else
            {
                query = @"
                    UPDATE AIAssistantConversation
                    SET TrangThai = 'Deleted',
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaHoiThoai = @MaHoiThoai
                      AND MaNguoiDung = @MaNguoiDung
                      AND TrangThai <> 'Deleted'";
            }

            SqlParameter[] parameters = {
                new SqlParameter("@MaHoiThoai", maHoiThoai),
                new SqlParameter("@MaNguoiDung", maNguoiDung)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>
        /// Thêm một tin nhắn mới vào cuộc hội thoại và cập nhật số lượng tin nhắn, thời điểm cập nhật của hội thoại.
        /// Sử dụng phép tăng lũy kế nguyên tử TongSoTinNhan = ISNULL(TongSoTinNhan, 0) + 1 để triệt tiêu Deadlock (Error 1205).
        /// </summary>
        public long ThemTinNhan(AIAssistantMessageDTO msg)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));
            if (msg.MaHoiThoai <= 0) return 0;

            string query = @"
                INSERT INTO AIAssistantMessage 
                    (MaHoiThoai, VaiTro, NoiDung, ThoiGianGui, ToolCallsJson, ToolResultJson, CardPayloadJson, TrangThai)
                VALUES 
                    (@MaHoiThoai, @VaiTro, @NoiDung, SYSDATETIME(), @ToolCallsJson, @ToolResultJson, @CardPayloadJson, @TrangThai);
                
                DECLARE @NewMsgId BIGINT = SCOPE_IDENTITY();

                UPDATE AIAssistantConversation
                SET TongSoTinNhan = ISNULL(TongSoTinNhan, 0) + 1,
                    NgayCapNhat = SYSDATETIME()
                WHERE MaHoiThoai = @MaHoiThoai;

                SELECT @NewMsgId;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaHoiThoai", msg.MaHoiThoai),
                new SqlParameter("@VaiTro", string.IsNullOrWhiteSpace(msg.VaiTro) ? "user" : msg.VaiTro.Trim()),
                new SqlParameter("@NoiDung", msg.NoiDung ?? string.Empty),
                new SqlParameter("@ToolCallsJson", (object?)msg.ToolCallsJson ?? DBNull.Value),
                new SqlParameter("@ToolResultJson", (object?)msg.ToolResultJson ?? DBNull.Value),
                new SqlParameter("@CardPayloadJson", (object?)msg.CardPayloadJson ?? DBNull.Value),
                new SqlParameter("@TrangThai", string.IsNullOrWhiteSpace(msg.TrangThai) ? "Sent" : msg.TrangThai.Trim())
            };

            object? result = SqlDataProvider.Instance.ExecuteScalar(query, parameters);
            long newMsgId = result != null && result != DBNull.Value ? Convert.ToInt64(result) : 0;
            msg.MaTinNhan = newMsgId;
            return newMsgId;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách tin nhắn của cuộc hội thoại, kiểm tra quyền sở hữu người dùng.
        /// </summary>
        public List<AIAssistantMessageDTO> LayDanhSachTinNhan(int maHoiThoai, int maNguoiDung)
        {
            var list = new List<AIAssistantMessageDTO>();
            if (maHoiThoai <= 0 || maNguoiDung <= 0) return list;

            string query = @"
                SELECT m.MaTinNhan, m.MaHoiThoai, m.VaiTro, m.NoiDung, m.ThoiGianGui, 
                       m.ToolCallsJson, m.ToolResultJson, m.CardPayloadJson, m.TrangThai
                FROM AIAssistantMessage m
                INNER JOIN AIAssistantConversation c ON m.MaHoiThoai = c.MaHoiThoai
                WHERE m.MaHoiThoai = @MaHoiThoai
                  AND c.MaNguoiDung = @MaNguoiDung
                  AND c.TrangThai <> 'Deleted'
                ORDER BY m.ThoiGianGui ASC, m.MaTinNhan ASC";

            SqlParameter[] parameters = {
                new SqlParameter("@MaHoiThoai", maHoiThoai),
                new SqlParameter("@MaNguoiDung", maNguoiDung)
            };

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapMessage(row));
            }

            return list;
        }

        /// <summary>
        /// Xóa tất cả tin nhắn trong cuộc hội thoại (Clear chat history).
        /// </summary>
        public bool XoaTatCaTinNhan(int maHoiThoai, int maNguoiDung)
        {
            if (maHoiThoai <= 0 || maNguoiDung <= 0) return false;

            string query = @"
                IF EXISTS (
                    SELECT 1 FROM AIAssistantConversation 
                    WHERE MaHoiThoai = @MaHoiThoai 
                      AND MaNguoiDung = @MaNguoiDung
                      AND TrangThai <> 'Deleted'
                )
                BEGIN
                    DELETE FROM AIAssistantMessage WHERE MaHoiThoai = @MaHoiThoai;

                    UPDATE AIAssistantConversation
                    SET TongSoTinNhan = 0,
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaHoiThoai = @MaHoiThoai;

                    SELECT 1;
                END
                ELSE
                BEGIN
                    SELECT 0;
                END;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaHoiThoai", maHoiThoai),
                new SqlParameter("@MaNguoiDung", maNguoiDung)
            };

            object? result = SqlDataProvider.Instance.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) == 1;
        }

        /// <summary>
        /// Cập nhật mô hình ưu tiên cho cuộc hội thoại.
        /// </summary>
        public bool CapNhatMoHinhUuTien(int maHoiThoai, int maNguoiDung, string moHinh)
        {
            if (maHoiThoai <= 0 || maNguoiDung <= 0) return false;

            string query = @"
                UPDATE AIAssistantConversation
                SET MoHinhUuTien = @MoHinh,
                    NgayCapNhat = SYSDATETIME()
                WHERE MaHoiThoai = @MaHoiThoai
                  AND MaNguoiDung = @MaNguoiDung
                  AND TrangThai <> 'Deleted'";

            SqlParameter[] parameters = {
                new SqlParameter("@MaHoiThoai", maHoiThoai),
                new SqlParameter("@MaNguoiDung", maNguoiDung),
                new SqlParameter("@MoHinh", string.IsNullOrWhiteSpace(moHinh) ? DBNull.Value : (object)moHinh.Trim())
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        // Helper mapper
        private static AIAssistantConversationDTO MapConversation(DataRow row)
        {
            return new AIAssistantConversationDTO
            {
                MaHoiThoai = Convert.ToInt32(row["MaHoiThoai"]),
                MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                TieuDe = row["TieuDe"] != DBNull.Value ? row["TieuDe"].ToString()! : "Cuộc hội thoại",
                NgayTao = Convert.ToDateTime(row["NgayTao"]),
                NgayCapNhat = Convert.ToDateTime(row["NgayCapNhat"]),
                TrangThai = row["TrangThai"] != DBNull.Value ? row["TrangThai"].ToString()! : "Active",
                TongSoTinNhan = row["TongSoTinNhan"] != DBNull.Value ? Convert.ToInt32(row["TongSoTinNhan"]) : 0,
                MoHinhUuTien = row["MoHinhUuTien"] != DBNull.Value ? row["MoHinhUuTien"].ToString() : null,
                PromptHeThongTuyChon = row["PromptHeThongTuyChon"] != DBNull.Value ? row["PromptHeThongTuyChon"].ToString() : null
            };
        }

        private static AIAssistantMessageDTO MapMessage(DataRow row)
        {
            return new AIAssistantMessageDTO
            {
                MaTinNhan = Convert.ToInt64(row["MaTinNhan"]),
                MaHoiThoai = Convert.ToInt32(row["MaHoiThoai"]),
                VaiTro = row["VaiTro"] != DBNull.Value ? row["VaiTro"].ToString()! : "user",
                NoiDung = row["NoiDung"] != DBNull.Value ? row["NoiDung"].ToString()! : string.Empty,
                ThoiGianGui = Convert.ToDateTime(row["ThoiGianGui"]),
                ToolCallsJson = row["ToolCallsJson"] != DBNull.Value ? row["ToolCallsJson"].ToString() : null,
                ToolResultJson = row["ToolResultJson"] != DBNull.Value ? row["ToolResultJson"].ToString() : null,
                CardPayloadJson = row["CardPayloadJson"] != DBNull.Value ? row["CardPayloadJson"].ToString() : null,
                TrangThai = row["TrangThai"] != DBNull.Value ? row["TrangThai"].ToString()! : "Sent"
            };
        }
    }
}
