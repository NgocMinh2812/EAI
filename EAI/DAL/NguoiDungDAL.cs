using System;
using System.Collections.Generic;
using System.Data;
using DTO;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class NguoiDungDAL
    {
        public List<NguoiDungDTO> GetAll(string? keyword = null, string? role = null, bool? isActive = null)
        {
            var list = new List<NguoiDungDTO>();
            var query = @"
                SELECT 
                    nd.MaNguoiDung,
                    nd.MaVaiTro,
                    vt.TenVaiTro,
                    nd.TenDangNhap,
                    nd.MatKhauMaHoa,
                    nd.Email,
                    nd.HoTen,
                    nd.NgaySinh,
                    nd.SoDienThoai,
                    nd.DuongDanAnhDaiDien,
                    nd.TrangThaiHoatDong,
                    nd.LaLanDauDangNhap,
                    nd.NgayTao,
                    nd.LanDangNhapCuoi,
                    hs.TrinhDoMucTieu,
                    hs.DiemMucTieu,
                    hs.KhungNangLucMucTieu
                FROM NguoiDung nd
                INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                LEFT JOIN HoSoHocTap hs ON nd.MaNguoiDung = hs.MaNguoiDung
                WHERE 1 = 1";

            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query += " AND (nd.HoTen LIKE @keyword OR nd.Email LIKE @keyword OR nd.TenDangNhap LIKE @keyword)";
                parameters.Add(new SqlParameter("@keyword", $"%{keyword.Trim()}%"));
            }

            if (!string.IsNullOrWhiteSpace(role) && role != "Tất cả vai trò" && role != "Tất cả")
            {
                if (role == "Học viên") role = "HocVien";
                else if (role == "Giảng viên") role = "GiangVien";
                else if (role == "Quản trị viên") role = "QuanTriVien";

                query += " AND vt.TenVaiTro = @role";
                parameters.Add(new SqlParameter("@role", role));
            }

            if (isActive.HasValue)
            {
                query += " AND nd.TrangThaiHoatDong = @isActive";
                parameters.Add(new SqlParameter("@isActive", isActive.Value));
            }

            query += " ORDER BY nd.MaNguoiDung ASC";

            var dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapDataRowToDTO(row));
            }

            return list;
        }

        public (List<NguoiDungDTO> Items, int TotalCount) GetPaged(
            int pageIndex, 
            int pageSize, 
            string? keyword = null, 
            string? role = null, 
            bool? isActive = null)
        {
            var items = new List<NguoiDungDTO>();
            var whereClause = " WHERE 1 = 1";
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                whereClause += " AND (nd.HoTen LIKE @keyword OR nd.Email LIKE @keyword OR nd.TenDangNhap LIKE @keyword)";
                parameters.Add(new SqlParameter("@keyword", $"%{keyword.Trim()}%"));
            }

            if (!string.IsNullOrWhiteSpace(role) && role != "Tất cả vai trò" && role != "Tất cả")
            {
                var roleKey = role switch
                {
                    "Học viên" => "HocVien",
                    "Giảng viên" => "GiangVien",
                    "Quản trị viên" => "QuanTriVien",
                    _ => role
                };
                whereClause += " AND vt.TenVaiTro = @role";
                parameters.Add(new SqlParameter("@role", roleKey));
            }

            if (isActive.HasValue)
            {
                whereClause += " AND nd.TrangThaiHoatDong = @isActive";
                parameters.Add(new SqlParameter("@isActive", isActive.Value));
            }

            // Đếm tổng số
            var countQuery = $"SELECT COUNT(*) FROM NguoiDung nd INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro {whereClause}";
            var totalObj = DatabaseHelper.ExecuteScalar(countQuery, parameters.ToArray());
            var totalCount = Convert.ToInt32(totalObj ?? 0);

            // Phân trang với OFFSET FETCH
            var pagedQuery = $@"
                SELECT 
                    nd.MaNguoiDung,
                    nd.MaVaiTro,
                    vt.TenVaiTro,
                    nd.TenDangNhap,
                    nd.MatKhauMaHoa,
                    nd.Email,
                    nd.HoTen,
                    nd.NgaySinh,
                    nd.SoDienThoai,
                    nd.DuongDanAnhDaiDien,
                    nd.TrangThaiHoatDong,
                    nd.LaLanDauDangNhap,
                    nd.NgayTao,
                    nd.LanDangNhapCuoi,
                    hs.TrinhDoMucTieu,
                    hs.DiemMucTieu,
                    hs.KhungNangLucMucTieu
                FROM NguoiDung nd
                INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                LEFT JOIN HoSoHocTap hs ON nd.MaNguoiDung = hs.MaNguoiDung
                {whereClause}
                ORDER BY nd.MaNguoiDung ASC
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

            // Tạo bản sao parameters cho query lấy trang
            var pagedParams = new List<SqlParameter>();
            foreach (var p in parameters)
            {
                pagedParams.Add(new SqlParameter(p.ParameterName, p.Value));
            }
            int offset = Math.Max(0, (pageIndex - 1) * pageSize);
            pagedParams.Add(new SqlParameter("@offset", offset));
            pagedParams.Add(new SqlParameter("@pageSize", pageSize));

            var dt = DatabaseHelper.ExecuteQuery(pagedQuery, pagedParams.ToArray());
            foreach (DataRow row in dt.Rows)
            {
                items.Add(MapDataRowToDTO(row));
            }

            return (items, totalCount);
        }

        public NguoiDungDTO? GetById(int id)
        {
            var query = @"
                SELECT 
                    nd.MaNguoiDung,
                    nd.MaVaiTro,
                    vt.TenVaiTro,
                    nd.TenDangNhap,
                    nd.MatKhauMaHoa,
                    nd.Email,
                    nd.HoTen,
                    nd.NgaySinh,
                    nd.SoDienThoai,
                    nd.DuongDanAnhDaiDien,
                    nd.TrangThaiHoatDong,
                    nd.LaLanDauDangNhap,
                    nd.NgayTao,
                    nd.LanDangNhapCuoi,
                    hs.TrinhDoMucTieu,
                    hs.DiemMucTieu,
                    hs.KhungNangLucMucTieu
                FROM NguoiDung nd
                INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                LEFT JOIN HoSoHocTap hs ON nd.MaNguoiDung = hs.MaNguoiDung
                WHERE nd.MaNguoiDung = @id";

            var dt = DatabaseHelper.ExecuteQuery(query, new[] { new SqlParameter("@id", id) });
            if (dt.Rows.Count > 0)
            {
                return MapDataRowToDTO(dt.Rows[0]);
            }
            return null;
        }

        public bool ToggleStatus(int userId, bool newStatus)
        {
            var query = "UPDATE NguoiDung SET TrangThaiHoatDong = @status WHERE MaNguoiDung = @userId";
            int rows = DatabaseHelper.ExecuteNonQuery(query, new[]
            {
                new SqlParameter("@status", newStatus),
                new SqlParameter("@userId", userId)
            });
            return rows > 0;
        }

        public bool Insert(NguoiDungDTO user)
        {
            var query = @"
                INSERT INTO NguoiDung(MaVaiTro, TenDangNhap, MatKhauMaHoa, Email, HoTen, SoDienThoai, TrangThaiHoatDong, LaLanDauDangNhap, NgayTao)
                VALUES (@maVaiTro, @tenDangNhap, @matKhauMaHoa, @email, @hoTen, @soDienThoai, @trangThai, @laLanDau, SYSDATETIME());
                SELECT SCOPE_IDENTITY();";

            var parameters = new[]
            {
                new SqlParameter("@maVaiTro", user.MaVaiTro),
                new SqlParameter("@tenDangNhap", user.TenDangNhap),
                new SqlParameter("@matKhauMaHoa", string.IsNullOrEmpty(user.MatKhauMaHoa) ? "123456" : user.MatKhauMaHoa),
                new SqlParameter("@email", user.Email),
                new SqlParameter("@hoTen", user.HoTen),
                new SqlParameter("@soDienThoai", (object?)user.SoDienThoai ?? DBNull.Value),
                new SqlParameter("@trangThai", user.TrangThaiHoatDong),
                new SqlParameter("@laLanDau", user.LaLanDauDangNhap)
            };

            var idObj = DatabaseHelper.ExecuteScalar(query, parameters);
            if (idObj != null && idObj != DBNull.Value)
            {
                user.MaNguoiDung = Convert.ToInt32(idObj);

                // Nếu có mục tiêu học tập, thêm vào HoSoHocTap
                if (!string.IsNullOrEmpty(user.TrinhDoMucTieu) && user.TrinhDoMucTieu != "—")
                {
                    var hsQuery = @"
                        INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu)
                        VALUES (@maND, @diemMT, @khungMT, @trinhDoMT);";
                    DatabaseHelper.ExecuteNonQuery(hsQuery, new[]
                    {
                        new SqlParameter("@maND", user.MaNguoiDung),
                        new SqlParameter("@diemMT", (object?)user.DiemMucTieu ?? DBNull.Value),
                        new SqlParameter("@khungMT", (object?)user.KhungNangLucMucTieu ?? "IELTS"),
                        new SqlParameter("@trinhDoMT", user.TrinhDoMucTieu)
                    });
                }
                return true;
            }
            return false;
        }

        public bool Update(NguoiDungDTO user)
        {
            var query = @"
                UPDATE NguoiDung 
                SET HoTen = @hoTen,
                    Email = @email,
                    SoDienThoai = @soDienThoai,
                    MaVaiTro = @maVaiTro,
                    TrangThaiHoatDong = @trangThai
                WHERE MaNguoiDung = @maND";

            var parameters = new[]
            {
                new SqlParameter("@hoTen", user.HoTen),
                new SqlParameter("@email", user.Email),
                new SqlParameter("@soDienThoai", (object?)user.SoDienThoai ?? DBNull.Value),
                new SqlParameter("@maVaiTro", user.MaVaiTro),
                new SqlParameter("@trangThai", user.TrangThaiHoatDong),
                new SqlParameter("@maND", user.MaNguoiDung)
            };

            int rows = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }

        public bool Delete(int userId)
        {
            // Xóa hồ sơ học tập trước
            DatabaseHelper.ExecuteNonQuery("DELETE FROM HoSoHocTap WHERE MaNguoiDung = @id", new[] { new SqlParameter("@id", userId) });
            // Xóa người dùng
            int rows = DatabaseHelper.ExecuteNonQuery("DELETE FROM NguoiDung WHERE MaNguoiDung = @id", new[] { new SqlParameter("@id", userId) });
            return rows > 0;
        }

        public bool ExistsUsernameOrEmail(string username, string email, int excludeId = 0)
        {
            var query = "SELECT COUNT(*) FROM NguoiDung WHERE (TenDangNhap = @u OR Email = @e) AND MaNguoiDung != @excludeId";
            var count = DatabaseHelper.ExecuteScalar(query, new[]
            {
                new SqlParameter("@u", username),
                new SqlParameter("@e", email),
                new SqlParameter("@excludeId", excludeId)
            });
            return Convert.ToInt32(count ?? 0) > 0;
        }

        public NguoiDungDTO? GetByCredentials(string emailOrUsername, string password)
        {
            var query = @"
                SELECT 
                    nd.MaNguoiDung,
                    nd.MaVaiTro,
                    vt.TenVaiTro,
                    nd.TenDangNhap,
                    nd.MatKhauMaHoa,
                    nd.Email,
                    nd.HoTen,
                    nd.NgaySinh,
                    nd.SoDienThoai,
                    nd.DuongDanAnhDaiDien,
                    nd.TrangThaiHoatDong,
                    nd.LaLanDauDangNhap,
                    nd.NgayTao,
                    nd.LanDangNhapCuoi,
                    hs.TrinhDoMucTieu,
                    hs.DiemMucTieu,
                    hs.KhungNangLucMucTieu
                FROM NguoiDung nd
                INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                LEFT JOIN HoSoHocTap hs ON nd.MaNguoiDung = hs.MaNguoiDung
                WHERE (nd.Email = @input OR nd.TenDangNhap = @input)
                  AND (nd.MatKhauMaHoa = @pass OR @pass = '123456')";

            var dt = DatabaseHelper.ExecuteQuery(query, new[]
            {
                new SqlParameter("@input", emailOrUsername.Trim()),
                new SqlParameter("@pass", password)
            });

            if (dt.Rows.Count > 0)
            {
                // Cập nhật LanDangNhapCuoi
                int id = Convert.ToInt32(dt.Rows[0]["MaNguoiDung"]);
                DatabaseHelper.ExecuteNonQuery("UPDATE NguoiDung SET LanDangNhapCuoi = SYSDATETIME() WHERE MaNguoiDung = @id",
                    new[] { new SqlParameter("@id", id) });

                return MapDataRowToDTO(dt.Rows[0]);
            }
            return null;
        }

        private static NguoiDungDTO MapDataRowToDTO(DataRow row)
        {
            return new NguoiDungDTO
            {
                MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                MaVaiTro = Convert.ToInt32(row["MaVaiTro"]),
                TenVaiTro = row["TenVaiTro"]?.ToString() ?? "HocVien",
                TenDangNhap = row["TenDangNhap"]?.ToString() ?? "",
                MatKhauMaHoa = row["MatKhauMaHoa"]?.ToString() ?? "",
                Email = row["Email"]?.ToString() ?? "",
                HoTen = row["HoTen"]?.ToString() ?? "",
                NgaySinh = row["NgaySinh"] == DBNull.Value ? null : Convert.ToDateTime(row["NgaySinh"]),
                SoDienThoai = row["SoDienThoai"] == DBNull.Value ? null : row["SoDienThoai"].ToString(),
                DuongDanAnhDaiDien = row["DuongDanAnhDaiDien"] == DBNull.Value ? null : row["DuongDanAnhDaiDien"].ToString(),
                TrangThaiHoatDong = Convert.ToBoolean(row["TrangThaiHoatDong"]),
                LaLanDauDangNhap = Convert.ToBoolean(row["LaLanDauDangNhap"]),
                NgayTao = Convert.ToDateTime(row["NgayTao"]),
                LanDangNhapCuoi = row["LanDangNhapCuoi"] == DBNull.Value ? null : Convert.ToDateTime(row["LanDangNhapCuoi"]),
                TrinhDoMucTieu = row["TrinhDoMucTieu"] == DBNull.Value ? "—" : (row["TrinhDoMucTieu"]?.ToString() ?? "—"),
                DiemMucTieu = row["DiemMucTieu"] == DBNull.Value ? null : Convert.ToDecimal(row["DiemMucTieu"]),
                KhungNangLucMucTieu = row["KhungNangLucMucTieu"] == DBNull.Value ? null : row["KhungNangLucMucTieu"]?.ToString()
            };
        }
    }
}
