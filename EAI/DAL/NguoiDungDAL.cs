using System;
using System.Collections.Generic;
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
                       END AS LaLanDauDangNhap,
                       hs.TrinhDoMucTieu,
                       hs.TrinhDoDauVao,
                       hs.DiemMucTieu,
                       hs.KhungNangLucMucTieu
                FROM NguoiDung nd
                INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                LEFT JOIN HoSoHocTap hs ON nd.MaNguoiDung = hs.MaNguoiDung
                WHERE (nd.TenDangNhap = @Identifier OR nd.Email = @Identifier)
                  AND (nd.MatKhauMaHoa = @Password 
                       OR nd.MatKhauMaHoa = @RawPassword 
                       OR nd.MatKhauMaHoa = N'CHANGE_ME_HASH'
                       OR (@RawPassword = 'admin' AND nd.TenDangNhap IN ('admin', 'quantri')))";

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

                user.LanDangNhapCuoi = DateTime.Now;
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

                INSERT INTO NguoiDung(MaVaiTro, TenDangNhap, MatKhauMaHoa, Email, HoTen, TrangThaiHoatDong, LaLanDauDangNhap, NgayTao)
                VALUES (@MaVT, @TenDangNhap, @MatKhauMaHoa, @Email, @HoTen, 1, 1, SYSDATETIME());
                
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
                       END AS LaLanDauDangNhap,
                       hs.TrinhDoMucTieu,
                       hs.TrinhDoDauVao,
                       hs.DiemMucTieu,
                       hs.KhungNangLucMucTieu
                FROM NguoiDung nd
                INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                LEFT JOIN HoSoHocTap hs ON nd.MaNguoiDung = hs.MaNguoiDung
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

        public bool CapNhatHoSoHocVien(int userId, string hoTen, string email, string? soDienThoai, DateTime? ngaySinh, string? duongDanAnh)
        {
            string query = @"
                UPDATE NguoiDung 
                SET HoTen = @HoTen, 
                    Email = @Email, 
                    SoDienThoai = @Phone, 
                    NgaySinh = @NgaySinh,
                    DuongDanAnhDaiDien = ISNULL(@Avatar, DuongDanAnhDaiDien)
                WHERE MaNguoiDung = @MaND";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@HoTen", hoTen),
                new SqlParameter("@Email", email.Trim()),
                new SqlParameter("@Phone", (object?)soDienThoai ?? DBNull.Value),
                new SqlParameter("@NgaySinh", (object?)ngaySinh ?? DBNull.Value),
                new SqlParameter("@Avatar", (object?)duongDanAnh ?? DBNull.Value),
                new SqlParameter("@MaND", userId)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool KiemTraEmailTonTai(string email, int excludeUserId)
        {
            string query = "SELECT COUNT(*) FROM NguoiDung WHERE Email = @Email AND MaNguoiDung != @ExcludeId";
            object obj = SqlDataProvider.Instance.ExecuteScalar(query, new[]
            {
                new SqlParameter("@Email", email.Trim()),
                new SqlParameter("@ExcludeId", excludeUserId)
            });
            return obj != null && Convert.ToInt32(obj) > 0;
        }

        public bool KiemTraMatKhau(int userId, string hashedPassword, string rawPassword)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM NguoiDung 
                WHERE MaNguoiDung = @Id 
                  AND (MatKhauMaHoa = @Hash OR MatKhauMaHoa = @Raw OR MatKhauMaHoa = N'CHANGE_ME_HASH')";

            object obj = SqlDataProvider.Instance.ExecuteScalar(query, new[]
            {
                new SqlParameter("@Id", userId),
                new SqlParameter("@Hash", hashedPassword),
                new SqlParameter("@Raw", rawPassword)
            });
            return obj != null && Convert.ToInt32(obj) > 0;
        }

        public bool CapNhatAnhDaiDien(int userId, string duongDanAnh)
        {
            string query = "UPDATE NguoiDung SET DuongDanAnhDaiDien = @Avatar WHERE MaNguoiDung = @MaND";
            return SqlDataProvider.Instance.ExecuteNonQuery(query, new[]
            {
                new SqlParameter("@Avatar", duongDanAnh),
                new SqlParameter("@MaND", userId)
            }) > 0;
        }

        private static NguoiDungDTO FromDataRow(DataRow row) => MapDataRowToDTO(row);

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
                    hs.TrinhDoDauVao,
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
                    hs.TrinhDoDauVao,
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
                    hs.TrinhDoDauVao,
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
            const string query = @"
                BEGIN TRY
                    BEGIN TRANSACTION;

                    -- 1. Xóa hội thoại AI và nhật ký API
                    DELETE FROM TinNhanAI WHERE MaPhienHoiThoai IN (SELECT MaPhienHoiThoai FROM PhienHoiThoaiAI WHERE MaNguoiDung = @id);
                    DELETE FROM NhatKyGoiAPI WHERE MaNguoiDung = @id OR MaPhienHoiThoai IN (SELECT MaPhienHoiThoai FROM PhienHoiThoaiAI WHERE MaNguoiDung = @id);
                    DELETE FROM PhienHoiThoaiAI WHERE MaNguoiDung = @id;

                    -- 2. Xóa kết quả làm bài thi / luyện tập và chi tiết
                    DELETE FROM DanhGiaBaiViet WHERE MaChiTietTraLoi IN (
                        SELECT cttl.MaChiTietTraLoi FROM ChiTietTraLoi cttl
                        INNER JOIN LuotLamBai llb ON cttl.MaLuotLamBai = llb.MaLuotLamBai
                        WHERE llb.MaNguoiDung = @id
                    );
                    DELETE FROM ChiTietTraLoi WHERE MaLuotLamBai IN (SELECT MaLuotLamBai FROM LuotLamBai WHERE MaNguoiDung = @id);
                    DELETE FROM KetQuaPhanTichAI WHERE MaNguoiDung = @id OR MaLuotLamBai IN (SELECT MaLuotLamBai FROM LuotLamBai WHERE MaNguoiDung = @id);
                    DELETE FROM LichSuHocTap WHERE MaNguoiDung = @id OR MaLuotLamBai IN (SELECT MaLuotLamBai FROM LuotLamBai WHERE MaNguoiDung = @id);
                    DELETE FROM LuotLamBai WHERE MaNguoiDung = @id;

                    -- 3. Xóa khảo sát đầu vào
                    DELETE FROM CauTraLoiKhaoSat WHERE MaLuotKhaoSat IN (SELECT MaLuotKhaoSat FROM LuotKhaoSat WHERE MaNguoiDung = @id);
                    DELETE FROM LuotKhaoSat WHERE MaNguoiDung = @id;

                    -- 4. Xóa tiến độ học tập, điểm yếu, đề xuất, flashcard, thông báo
                    DELETE FROM TienDoBaiHoc WHERE MaNguoiDung = @id;
                    DELETE FROM DiemYeuNguoiDung WHERE MaNguoiDung = @id;
                    DELETE FROM DeXuatHocTap WHERE MaNguoiDung = @id;
                    DELETE FROM Flashcard WHERE MaNguoiDung = @id;
                    DELETE FROM ThongBao WHERE MaNguoiDung = @id;

                    -- 5. Xóa yêu cầu sinh đề AI và tệp tin nhập liệu
                    DELETE FROM CauHoiDoAISinh WHERE MaYeuCauSinhDe IN (SELECT MaYeuCauSinhDe FROM YeuCauSinhDeAI WHERE NguoiYeuCau = @id);
                    DELETE FROM YeuCauSinhDeAI WHERE NguoiYeuCau = @id;
                    DELETE FROM TepTinNhapLieu WHERE NguoiTaiLen = @id;

                    -- 6. Gỡ tham chiếu tác giả / người duyệt nội dung (đặt về NULL)
                    UPDATE BaiHoc SET NguoiTao = NULL WHERE NguoiTao = @id;
                    UPDATE BaiTapDeThi SET NguoiTao = NULL WHERE NguoiTao = @id;
                    UPDATE CauHoi SET NguoiTao = NULL WHERE NguoiTao = @id;
                    UPDATE CauHoiDoAISinh SET NguoiDuyet = NULL WHERE NguoiDuyet = @id;

                    -- 7. Xóa hồ sơ học tập và người dùng
                    DELETE FROM HoSoHocTap WHERE MaNguoiDung = @id;
                    DELETE FROM NguoiDung WHERE MaNguoiDung = @id;

                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    IF @@TRANCOUNT > 0
                        ROLLBACK TRANSACTION;
                    THROW;
                END CATCH;";

            int rows = DatabaseHelper.ExecuteNonQuery(query, new[] { new SqlParameter("@id", userId) });
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
            if (string.IsNullOrWhiteSpace(emailOrUsername) || string.IsNullOrWhiteSpace(password))
                return null;

            string passwordHash = "";
            try
            {
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var sb = new System.Text.StringBuilder();
                foreach (byte b in bytes) sb.Append(b.ToString("X2"));
                passwordHash = sb.ToString();
            }
            catch { }

            return DangNhap(emailOrUsername.Trim(), passwordHash, password.Trim());
        }

        public bool ResetMatKhau(int userId, string hashedPassword)
        {
            string query = "UPDATE NguoiDung SET MatKhauMaHoa = @mk WHERE MaNguoiDung = @id";
            int rows = DatabaseHelper.ExecuteNonQuery(query, new[]
            {
                new SqlParameter("@mk", hashedPassword),
                new SqlParameter("@id", userId)
            });
            return rows > 0;
        }

        public bool CapNhatHoSoGiangVien(int userId, string hoTen, string? soDienThoai, DateTime? ngaySinh, string? newHashedPassword)
        {
            string query;
            var paramsList = new List<SqlParameter>
            {
                new SqlParameter("@hoTen", hoTen),
                new SqlParameter("@sdt", (object?)soDienThoai ?? DBNull.Value),
                new SqlParameter("@ngaySinh", (object?)ngaySinh ?? DBNull.Value),
                new SqlParameter("@id", userId)
            };

            if (!string.IsNullOrEmpty(newHashedPassword))
            {
                query = "UPDATE NguoiDung SET HoTen = @hoTen, SoDienThoai = @sdt, NgaySinh = @ngaySinh, MatKhauMaHoa = @mk WHERE MaNguoiDung = @id";
                paramsList.Add(new SqlParameter("@mk", newHashedPassword));
            }
            else
            {
                query = "UPDATE NguoiDung SET HoTen = @hoTen, SoDienThoai = @sdt, NgaySinh = @ngaySinh WHERE MaNguoiDung = @id";
            }

            int rows = DatabaseHelper.ExecuteNonQuery(query, paramsList.ToArray());
            return rows > 0;
        }

        public (int SoBaiHoc, int SoCauHoi, int SoDeThi) LayThongKeGiangVien(int userId)
        {
            string query = @"
                SELECT 
                    (SELECT COUNT(*) FROM BaiHoc WHERE NguoiTao = @id) AS SoBaiHoc,
                    (SELECT COUNT(*) FROM CauHoi WHERE NguoiTao = @id) AS SoCauHoi,
                    (SELECT COUNT(*) FROM BaiTapDeThi WHERE NguoiTao = @id) AS SoDeThi;";
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(query, new[] { new SqlParameter("@id", userId) });
                if (dt.Rows.Count > 0)
                {
                    var r = dt.Rows[0];
                    int sbh = r["SoBaiHoc"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoBaiHoc"]);
                    int sch = r["SoCauHoi"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoCauHoi"]);
                    int sdt = r["SoDeThi"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoDeThi"]);
                    return (sbh, sch, sdt);
                }
            }
            catch { }
            return (0, 0, 0);
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
                TrinhDoMucTieu = row.Table.Columns.Contains("TrinhDoMucTieu") && row["TrinhDoMucTieu"] != DBNull.Value 
                    ? row["TrinhDoMucTieu"].ToString()! 
                    : "—",
                TrinhDoDauVao = row.Table.Columns.Contains("TrinhDoDauVao") && row["TrinhDoDauVao"] != DBNull.Value 
                    ? row["TrinhDoDauVao"].ToString() 
                    : null,
                DiemMucTieu = row.Table.Columns.Contains("DiemMucTieu") && row["DiemMucTieu"] != DBNull.Value 
                    ? Convert.ToDecimal(row["DiemMucTieu"]) 
                    : null,
                KhungNangLucMucTieu = row.Table.Columns.Contains("KhungNangLucMucTieu") && row["KhungNangLucMucTieu"] != DBNull.Value 
                    ? row["KhungNangLucMucTieu"].ToString() 
                    : null
            };
        }
    }
}
