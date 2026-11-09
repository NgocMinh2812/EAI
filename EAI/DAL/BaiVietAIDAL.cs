using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class BaiVietAIDAL
    {
        private static BaiVietAIDAL? _instance;
        public static BaiVietAIDAL Instance => _instance ??= new BaiVietAIDAL();

        public long LuuKetQuaChamBai(int maNguoiDung, string deBai, string baiVietGoc, string duLieuKetQuaJson, string trinhDo = "B1", string soTuYeuCau = "80–120 từ")
        {
            var inputObj = new
            {
                Topic = deBai,
                Essay = baiVietGoc,
                Level = trinhDo,
                TargetWords = soTuYeuCau
            };
            string inputJson = JsonSerializer.Serialize(inputObj);

            string query = @"
                INSERT INTO KetQuaPhanTichAI (MaNguoiDung, LoaiPhanTich, DuLieuDauVao, DuLieuKetQua, NgayTao)
                VALUES (@MaNguoiDung, N'ChamBaiVietAI', @DuLieuDauVao, @DuLieuKetQua, SYSDATETIME());
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@MaNguoiDung", maNguoiDung),
                new SqlParameter("@DuLieuDauVao", inputJson),
                new SqlParameter("@DuLieuKetQua", duLieuKetQuaJson)
            };

            object result = SqlDataProvider.Instance.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt64(result) : 0;
        }

        public List<BaiVietDaChamDTO> LayLichSuBaiViet(int maNguoiDung)
        {
            var list = new List<BaiVietDaChamDTO>();
            string query = @"
                SELECT MaKetQuaPhanTich, MaNguoiDung, DuLieuDauVao, DuLieuKetQua, NgayTao
                FROM KetQuaPhanTichAI
                WHERE MaNguoiDung = @MaNguoiDung AND LoaiPhanTich = N'ChamBaiVietAI'
                ORDER BY NgayTao DESC";

            SqlParameter[] parameters = { new SqlParameter("@MaNguoiDung", maNguoiDung) };
            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                string inputRaw = row["DuLieuDauVao"] != DBNull.Value ? row["DuLieuDauVao"].ToString()! : "";
                string resultRaw = row["DuLieuKetQua"] != DBNull.Value ? row["DuLieuKetQua"].ToString()! : "";

                string topic = "Đoạn văn luyện viết";
                string essay = "";
                string level = "B1";
                string words = "80–120 từ";

                if (!string.IsNullOrWhiteSpace(inputRaw))
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(inputRaw);
                        if (doc.RootElement.TryGetProperty("Topic", out var t)) topic = t.GetString() ?? topic;
                        if (doc.RootElement.TryGetProperty("Essay", out var e)) essay = e.GetString() ?? essay;
                        if (doc.RootElement.TryGetProperty("Level", out var l)) level = l.GetString() ?? level;
                        if (doc.RootElement.TryGetProperty("TargetWords", out var w)) words = w.GetString() ?? words;
                    }
                    catch
                    {
                        essay = inputRaw;
                    }
                }

                double score = 7.0;
                KetQuaChamBaiVietJsonDTO? parsed = null;
                if (!string.IsNullOrWhiteSpace(resultRaw))
                {
                    try
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        parsed = JsonSerializer.Deserialize<KetQuaChamBaiVietJsonDTO>(resultRaw, options);
                        if (parsed != null) score = parsed.DiemTongQuan;
                    }
                    catch { }
                }

                list.Add(new BaiVietDaChamDTO
                {
                    MaKetQuaPhanTich = Convert.ToInt64(row["MaKetQuaPhanTich"]),
                    MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                    DeBai = topic,
                    BaiVietGoc = essay,
                    TrinhDo = level,
                    SoTuYeuCau = words,
                    DuLieuKetQuaRaw = resultRaw,
                    DiemTongQuan = score,
                    NgayTao = Convert.ToDateTime(row["NgayTao"]),
                    KetQuaParsed = parsed
                });
            }

            return list;
        }

        public bool XoaKetQuaBaiViet(long maKetQua, int maNguoiDung)
        {
            string query = "DELETE FROM KetQuaPhanTichAI WHERE MaKetQuaPhanTich = @MaKQ AND MaNguoiDung = @MaND";
            SqlParameter[] parameters = {
                new SqlParameter("@MaKQ", maKetQua),
                new SqlParameter("@MaND", maNguoiDung)
            };
            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public long LuuDeBaiYeuThich(int maNguoiDung, DeBaiVietAIDTO deBai)
        {
            string inputJson = JsonSerializer.Serialize(new
            {
                Title = deBai.TieuDe,
                Level = deBai.TrinhDo,
                Topic = deBai.ChuDe,
                Type = deBai.DangBai,
                TargetWords = deBai.SoTuYeuCau
            });

            string resultJson = JsonSerializer.Serialize(deBai);

            string query = @"
                INSERT INTO KetQuaPhanTichAI (MaNguoiDung, LoaiPhanTich, DuLieuDauVao, DuLieuKetQua, NgayTao)
                VALUES (@MaNguoiDung, N'DeBaiLuyenVietAI', @DuLieuDauVao, @DuLieuKetQua, SYSDATETIME());
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@MaNguoiDung", maNguoiDung),
                new SqlParameter("@DuLieuDauVao", inputJson),
                new SqlParameter("@DuLieuKetQua", resultJson)
            };

            object result = SqlDataProvider.Instance.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt64(result) : 0;
        }

        public List<DeBaiVietAIDTO> LayDanhSachDeYeuThich(int maNguoiDung)
        {
            var list = new List<DeBaiVietAIDTO>();
            string query = @"
                SELECT MaKetQuaPhanTich, MaNguoiDung, DuLieuDauVao, DuLieuKetQua, NgayTao
                FROM KetQuaPhanTichAI
                WHERE MaNguoiDung = @MaNguoiDung AND LoaiPhanTich = N'DeBaiLuyenVietAI'
                ORDER BY NgayTao DESC";

            SqlParameter[] parameters = { new SqlParameter("@MaNguoiDung", maNguoiDung) };
            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                string resultRaw = row["DuLieuKetQua"] != DBNull.Value ? row["DuLieuKetQua"].ToString()! : "";
                if (!string.IsNullOrWhiteSpace(resultRaw))
                {
                    try
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var de = JsonSerializer.Deserialize<DeBaiVietAIDTO>(resultRaw, options);
                        if (de != null)
                        {
                            de.MaDeBai = Convert.ToInt64(row["MaKetQuaPhanTich"]);
                            de.IsFavorite = true;
                            de.NgayTao = Convert.ToDateTime(row["NgayTao"]);
                            list.Add(de);
                        }
                    }
                    catch { }
                }
            }

            return list;
        }

        public bool XoaDeYeuThich(long maDe, int maNguoiDung)
        {
            string query = "DELETE FROM KetQuaPhanTichAI WHERE MaKetQuaPhanTich = @MaDe AND MaNguoiDung = @MaND AND LoaiPhanTich = N'DeBaiLuyenVietAI'";
            SqlParameter[] parameters = {
                new SqlParameter("@MaDe", maDe),
                new SqlParameter("@MaND", maNguoiDung)
            };
            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public int DemTongSoLuotChamBaiViet()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM KetQuaPhanTichAI WHERE LoaiPhanTich = N'ChamBaiVietAI'";
                object res = SqlDataProvider.Instance.ExecuteScalar(query);
                return res != null && res != DBNull.Value ? Convert.ToInt32(res) : 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}
