using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DTO;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class DanhMucDAL
    {
        public List<TrinhDoDTO> GetAllTrinhDo()
        {
            var list = new List<TrinhDoDTO>();
            var query = "SELECT MaTrinhDo, MaCapDo, TenTrinhDo, MoTa FROM TrinhDo ORDER BY MaTrinhDo ASC";
            var dt = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new TrinhDoDTO
                {
                    MaTrinhDo = Convert.ToInt32(row["MaTrinhDo"]),
                    MaCapDo = row["MaCapDo"]?.ToString() ?? "",
                    TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                    MoTa = row["MoTa"] == DBNull.Value ? null : row["MoTa"]?.ToString()
                });
            }
            return list;
        }

        public List<ChuDeDTO> GetAllChuDe()
        {
            var list = new List<ChuDeDTO>();
            var query = @"
                SELECT 
                    cd.MaChuDe,
                    cd.MaChuDeCha,
                    cd.TenChuDe,
                    cd.MoTa,
                    cd.TrangThaiHoatDong,
                    cdCha.TenChuDe AS TenChuDeCha,
                    (SELECT COUNT(*) FROM BaiHoc bh WHERE bh.MaChuDe = cd.MaChuDe) AS SoLuongBaiHoc
                FROM ChuDe cd
                LEFT JOIN ChuDe cdCha ON cd.MaChuDeCha = cdCha.MaChuDe
                ORDER BY cd.MaChuDeCha ASC, cd.MaChuDe ASC";

            var dt = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ChuDeDTO
                {
                    MaChuDe = Convert.ToInt32(row["MaChuDe"]),
                    MaChuDeCha = row["MaChuDeCha"] == DBNull.Value ? null : Convert.ToInt32(row["MaChuDeCha"]),
                    TenChuDe = row["TenChuDe"]?.ToString() ?? "",
                    MoTa = row["MoTa"] == DBNull.Value ? null : row["MoTa"]?.ToString(),
                    TrangThaiHoatDong = Convert.ToBoolean(row["TrangThaiHoatDong"]),
                    TenChuDeCha = row["TenChuDeCha"] == DBNull.Value ? "" : (row["TenChuDeCha"]?.ToString() ?? ""),
                    SoLuongBaiHoc = Convert.ToInt32(row["SoLuongBaiHoc"])
                });
            }
            return list;
        }

        /// <summary>
        /// Xây dựng cây chủ đề đệ quy
        /// </summary>
        public List<ChuDeDTO> GetChuDeTree()
        {
            var all = GetAllChuDe();
            var roots = all.Where(x => !x.MaChuDeCha.HasValue || x.MaChuDeCha <= 0).ToList();
            var visited = new HashSet<int>();

            foreach (var root in roots)
            {
                BuildChildrenRecursive(root, all, visited);
            }

            return roots;
        }

        private void BuildChildrenRecursive(ChuDeDTO parent, List<ChuDeDTO> all, HashSet<int> visited)
        {
            if (visited.Contains(parent.MaChuDe)) return;
            visited.Add(parent.MaChuDe);

            parent.DanhSachCon = all.Where(x => x.MaChuDeCha == parent.MaChuDe && x.MaChuDe != parent.MaChuDe && !visited.Contains(x.MaChuDe)).ToList();
            foreach (var child in parent.DanhSachCon)
            {
                BuildChildrenRecursive(child, all, visited);
            }
        }

        public int DemSoBaiHocTheoChuDe(int maChuDe)
        {
            var query = "SELECT COUNT(*) FROM BaiHoc WHERE MaChuDe = @id";
            var count = DatabaseHelper.ExecuteScalar(query, new[] { new SqlParameter("@id", idQueryParam(maChuDe)) });
            return Convert.ToInt32(count ?? 0);
        }

        public int DemSoChuDeCon(int maChuDe)
        {
            var query = "SELECT COUNT(*) FROM ChuDe WHERE MaChuDeCha = @id";
            var count = DatabaseHelper.ExecuteScalar(query, new[] { new SqlParameter("@id", idQueryParam(maChuDe)) });
            return Convert.ToInt32(count ?? 0);
        }

        public bool InsertChuDe(ChuDeDTO cd)
        {
            var query = @"
                INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
                VALUES (@parentId, @ten, @mota, @trangThai);
                SELECT SCOPE_IDENTITY();";

            var parameters = new[]
            {
                new SqlParameter("@parentId", (object?)cd.MaChuDeCha ?? DBNull.Value),
                new SqlParameter("@ten", cd.TenChuDe),
                new SqlParameter("@mota", (object?)cd.MoTa ?? DBNull.Value),
                new SqlParameter("@trangThai", cd.TrangThaiHoatDong)
            };

            var idObj = DatabaseHelper.ExecuteScalar(query, parameters);
            if (idObj != null && idObj != DBNull.Value)
            {
                cd.MaChuDe = Convert.ToInt32(idObj);
                return true;
            }
            return false;
        }

        public bool UpdateChuDe(ChuDeDTO cd)
        {
            var query = @"
                UPDATE ChuDe 
                SET TenChuDe = @ten,
                    MoTa = @mota,
                    MaChuDeCha = @parentId,
                    TrangThaiHoatDong = @trangThai
                WHERE MaChuDe = @id";

            var parameters = new[]
            {
                new SqlParameter("@ten", cd.TenChuDe),
                new SqlParameter("@mota", (object?)cd.MoTa ?? DBNull.Value),
                new SqlParameter("@parentId", (object?)cd.MaChuDeCha ?? DBNull.Value),
                new SqlParameter("@trangThai", cd.TrangThaiHoatDong),
                new SqlParameter("@id", cd.MaChuDe)
            };

            int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }

        public bool DeleteChuDe(int maChuDe)
        {
            var query = "DELETE FROM ChuDe WHERE MaChuDe = @id";
            int rows = DatabaseHelper.ExecuteNonQuery(query, new[] { new SqlParameter("@id", maChuDe) });
            return rows > 0;
        }

        private static object idQueryParam(int id) => id;
    }
}
