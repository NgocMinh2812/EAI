using System;
using System.Collections.Generic;
using System.Data;
using DTO;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class BaiHocDAL
    {
        public List<BaiHocDTO> GetAll(int? maChuDe = null, string? search = null)
        {
            var list = new List<BaiHocDTO>();
            var query = @"
                SELECT 
                    bh.MaBaiHoc,
                    bh.MaChuDe,
                    cd.TenChuDe,
                    bh.MaTrinhDo,
                    td.MaCapDo AS TenTrinhDo,
                    bh.NguoiTao,
                    bh.TieuDe,
                    bh.LoaiBaiHoc,
                    bh.TomTat,
                    bh.NoiDung,
                    bh.ThoiGianUocTinhPhut,
                    bh.DaXuatBan,
                    bh.NgayTao
                FROM BaiHoc bh
                INNER JOIN ChuDe cd ON bh.MaChuDe = cd.MaChuDe
                INNER JOIN TrinhDo td ON bh.MaTrinhDo = td.MaTrinhDo
                WHERE 1 = 1";

            var parameters = new List<SqlParameter>();

            if (maChuDe.HasValue && maChuDe.Value > 0)
            {
                // Bao gồm cả chủ đề con
                query += @" AND (bh.MaChuDe = @maChuDe OR bh.MaChuDe IN (SELECT MaChuDe FROM ChuDe WHERE MaChuDeCha = @maChuDe))";
                parameters.Add(new SqlParameter("@maChuDe", maChuDe.Value));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query += " AND (bh.TieuDe LIKE @search OR bh.TomTat LIKE @search)";
                parameters.Add(new SqlParameter("@search", $"%{search.Trim()}%"));
            }

            query += " ORDER BY bh.MaBaiHoc DESC";

            var dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
            foreach (DataRow row in dt.Rows)
            {
                var loai = row["LoaiBaiHoc"]?.ToString() ?? "TuVung";
                var tenLoai = loai switch
                {
                    "NguPhap" => "Ngữ pháp",
                    "Doc" or "DocHieu" => "Đọc hiểu",
                    "Nghe" or "NgheHieu" => "Luyện nghe",
                    "Viet" or "VietLuan" => "Viết luận",
                    _ => "Từ vựng"
                };

                var icon = loai switch
                {
                    "NguPhap" => "📘",
                    "Doc" or "DocHieu" => "📕",
                    "Nghe" or "NgheHieu" => "🎧",
                    "Viet" or "VietLuan" => "✍️",
                    _ => "📙"
                };

                bool daXuatBan = Convert.ToBoolean(row["DaXuatBan"]);

                list.Add(new BaiHocDTO
                {
                    MaBaiHoc = Convert.ToInt32(row["MaBaiHoc"]),
                    TieuDe = row["TieuDe"]?.ToString() ?? "",
                    TenChuDe = row["TenChuDe"]?.ToString() ?? "",
                    LoaiBaiHoc = loai,
                    TenLoaiHienThi = tenLoai,
                    MaTrinhDo = row["TenTrinhDo"]?.ToString() ?? "B1",
                    TomTat = row["TomTat"] == DBNull.Value ? "" : (row["TomTat"]?.ToString() ?? ""),
                    NoiDung = row["NoiDung"] == DBNull.Value ? "" : (row["NoiDung"]?.ToString() ?? ""),
                    ThoiGianUocTinhPhut = Convert.ToInt32(row["ThoiGianUocTinhPhut"]),
                    TrangThai = daXuatBan ? "Đã xuất bản" : "Bản nháp",
                    IconSkill = icon,
                    StatusBackground = daXuatBan ? "#DCFCE7" : "#F1F5F9",
                    StatusColor = daXuatBan ? "#15803D" : "#64748B"
                });
            }

            return list;
        }
    }
}
