using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class NguoiDungDAL
    {
        private static NguoiDungDAL? _instance;
        public static NguoiDungDAL Instance => _instance ??= new NguoiDungDAL();

        public NguoiDungDTO? DangNhap(string usernameOrEmail, string matKhauMaHoa, string rawPassword = "")
        {
            string query = @"
                SELECT nd.MaNguoiDung, nd.MaVaiTro, vt.TenVaiTro, nd.TenDangNhap, nd.MatKhauMaHoa, 
                       nd.Email, nd.HoTen, nd.NgaySinh, nd.SoDienThoai, nd.DuongDanAnhDaiDien, 
                       nd.TrangThaiHoatDong, nd.NgayTao, nd.LanDangNhapCuoi,
                       CASE 
                           WHEN nd.LaLanDauDangNhap = 0 THEN CAST(0 AS BIT)
                           WHEN NOT EXISTS (SELECT 1 FROM LuotKhaoSat WHERE MaNguoiDung = nd.MaNguoiDung) THEN CAST(1 AS BIT)
                           ELSE CAST(0 AS BIT)
                       END AS LaLanDauDangNhap
                FROM NguoiDung nd
                INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                WHERE (nd.TenDangNhap = @Identifier OR nd.Email = @Identifier)
                  AND (nd.MatKhauMaHoa = @Password 
                       OR nd.MatKhauMaHoa = @RawPassword 
                       OR nd.MatKhauMaHoa = N'CHANGE_ME_HASH')";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@Identifier", usernameOrEmail),
                new SqlParameter("@Password", matKhauMaHoa),
                new SqlParameter("@RawPassword", string.IsNullOrEmpty(rawPassword) ? matKhauMaHoa : rawPassword)
            };

            DataTable table = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
            if (table.Rows.Count > 0)
            {
                var user = FromDataRow(table.Rows[0]);

                // Cập nhật lần đăng nhập cuối và đồng bộ mật khẩu đã mã hóa nếu trước đó là CHANGE_ME_HASH hoặc plaintext
                string updateLastLogin = @"
                    UPDATE NguoiDung 
                    SET LanDangNhapCuoi = SYSDATETIME(),
                        MatKhauMaHoa = @NewHash
                    WHERE MaNguoiDung = @MaND";
                SqlDataProvider.Instance.ExecuteNonQuery(updateLastLogin, new[] 
                { 
                    new SqlParameter("@MaND", user.MaNguoiDung),
                    new SqlParameter("@NewHash", matKhauMaHoa)
                });

                return user;
            }
            return null;
        }

        public bool KiemTraEmailTonTai(string email)
        {
            string query = "SELECT COUNT(1) FROM NguoiDung WHERE Email = @Email";
            object? result = SqlDataProvider.Instance.ExecuteScalar(query, new[] { new SqlParameter("@Email", email) });
            return Convert.ToInt32(result) > 0;
        }

        public bool KiemTraTenDangNhapTonTai(string tenDangNhap)
        {
            string query = "SELECT COUNT(1) FROM NguoiDung WHERE TenDangNhap = @TenDangNhap";
            object? result = SqlDataProvider.Instance.ExecuteScalar(query, new[] { new SqlParameter("@TenDangNhap", tenDangNhap) });
            return Convert.ToInt32(result) > 0;
        }

        public int DangKy(NguoiDungDTO user)
        {
            // Mặc định đăng ký là Học Viên (MaVaiTro tương ứng TenVaiTro = 'HocVien')
            string query = @"
                DECLARE @MaVT INT = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = N'HocVien');
                IF @MaVT IS NULL SET @MaVT = 2;

                INSERT INTO NguoiDung(MaVaiTro, TenDangNhap, MatKhauMaHoa, Email, HoTen, TrangThaiHoatDong, NgayTao)
                VALUES (@MaVT, @TenDangNhap, @MatKhauMaHoa, @Email, @HoTen, 1, SYSDATETIME());
                
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@TenDangNhap", user.TenDangNhap),
                new SqlParameter("@MatKhauMaHoa", user.MatKhauMaHoa),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@HoTen", user.HoTen)
            };

            object? newId = SqlDataProvider.Instance.ExecuteScalar(query, parameters);
            return Convert.ToInt32(newId);
        }

        public bool CapNhatTrangThaiLanDauDangNhap(int maNguoiDung, bool laLanDau)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'LaLanDauDangNhap' AND Object_ID = Object_ID('NguoiDung'))
                BEGIN
                    UPDATE NguoiDung SET LaLanDauDangNhap = @LaLanDau WHERE MaNguoiDung = @MaND;
                END";
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@LaLanDau", laLanDau ? 1 : 0),
                new SqlParameter("@MaND", maNguoiDung)
            };
            SqlDataProvider.Instance.ExecuteNonQuery(query, parameters);
            return true;
        }

        public NguoiDungDTO? LayTheoId(int maNguoiDung)
        {
            string query = @"
                SELECT nd.MaNguoiDung, nd.MaVaiTro, vt.TenVaiTro, nd.TenDangNhap, nd.MatKhauMaHoa, 
                       nd.Email, nd.HoTen, nd.NgaySinh, nd.SoDienThoai, nd.DuongDanAnhDaiDien, 
                       nd.TrangThaiHoatDong, nd.NgayTao, nd.LanDangNhapCuoi,
                       CASE 
                           WHEN nd.LaLanDauDangNhap = 0 THEN CAST(0 AS BIT)
                           WHEN NOT EXISTS (SELECT 1 FROM LuotKhaoSat WHERE MaNguoiDung = nd.MaNguoiDung) THEN CAST(1 AS BIT)
                           ELSE CAST(0 AS BIT)
                       END AS LaLanDauDangNhap
                FROM NguoiDung nd
                INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                WHERE nd.MaNguoiDung = @MaND";

            DataTable table = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaND", maNguoiDung) });
            if (table.Rows.Count > 0)
            {
                return FromDataRow(table.Rows[0]);
            }
            return null;
        }

        public DataTable LayDanhSachNguoiDung()
        {
            string query = @"
                SELECT nd.MaNguoiDung, nd.HoTen, nd.Email, nd.TenDangNhap, nd.TrangThaiHoatDong, 
                       nd.LanDangNhapCuoi, vt.TenVaiTro, hs.TrinhDoMucTieu
                FROM NguoiDung nd
                INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                LEFT JOIN HoSoHocTap hs ON nd.MaNguoiDung = hs.MaNguoiDung
                ORDER BY nd.MaNguoiDung ASC";
            return SqlDataProvider.Instance.ExecuteQuery(query);
        }

        public bool DoiTrangThaiKhoa(int maNguoiDung, bool trangThaiMoi)
        {
            string query = "UPDATE NguoiDung SET TrangThaiHoatDong = @TrangThai WHERE MaNguoiDung = @MaND";
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@TrangThai", trangThaiMoi ? 1 : 0),
                new SqlParameter("@MaND", maNguoiDung)
            };
            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CapNhatThongTinCaNhan(int maNguoiDung, string hoTen, string soDienThoai)
        {
            string query = "UPDATE NguoiDung SET HoTen = @HoTen, SoDienThoai = @Phone WHERE MaNguoiDung = @MaND";
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@HoTen", hoTen),
                new SqlParameter("@Phone", (object?)soDienThoai ?? DBNull.Value),
                new SqlParameter("@MaND", maNguoiDung)
            };
            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        private static NguoiDungDTO FromDataRow(DataRow row)
        {
            return new NguoiDungDTO
            {
                MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                MaVaiTro = Convert.ToInt32(row["MaVaiTro"]),
                TenVaiTro = row["TenVaiTro"]?.ToString() ?? string.Empty,
                TenDangNhap = row["TenDangNhap"]?.ToString() ?? string.Empty,
                MatKhauMaHoa = row["MatKhauMaHoa"]?.ToString() ?? string.Empty,
                Email = row["Email"]?.ToString() ?? string.Empty,
                HoTen = row["HoTen"]?.ToString() ?? string.Empty,
                NgaySinh = row["NgaySinh"] == DBNull.Value ? null : Convert.ToDateTime(row["NgaySinh"]),
                SoDienThoai = row["SoDienThoai"]?.ToString(),
                DuongDanAnhDaiDien = row["DuongDanAnhDaiDien"]?.ToString(),
                TrangThaiHoatDong = Convert.ToBoolean(row["TrangThaiHoatDong"]),
                LaLanDauDangNhap = Convert.ToBoolean(row["LaLanDauDangNhap"]),
                NgayTao = Convert.ToDateTime(row["NgayTao"]),
                LanDangNhapCuoi = row["LanDangNhapCuoi"] == DBNull.Value ? null : Convert.ToDateTime(row["LanDangNhapCuoi"])
            };
        }
    }
}
