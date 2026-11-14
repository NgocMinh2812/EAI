using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class HoSoHocTapDAL
    {
        private static HoSoHocTapDAL? _instance;
        public static HoSoHocTapDAL Instance => _instance ??= new HoSoHocTapDAL();

        public HoSoHocTapDTO? LayTheoMaNguoiDung(int maNguoiDung)
        {
            string query = @"
                SELECT MaHoSo, MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, 
                       TrinhDoMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan, NgayCapNhat
                FROM HoSoHocTap
                WHERE MaNguoiDung = @MaND";

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaND", maNguoiDung) });
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                return new HoSoHocTapDTO
                {
                    MaHoSo = Convert.ToInt32(row["MaHoSo"]),
                    MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                    DiemMucTieu = row["DiemMucTieu"] == DBNull.Value ? null : Convert.ToDecimal(row["DiemMucTieu"]),
                    KhungNangLucMucTieu = row["KhungNangLucMucTieu"]?.ToString(),
                    TrinhDoMucTieu = row["TrinhDoMucTieu"]?.ToString(),
                    TrinhDoDauVao = row.Table.Columns.Contains("TrinhDoDauVao") && row["TrinhDoDauVao"] != DBNull.Value ? row["TrinhDoDauVao"]?.ToString() : null,
                    DiemDanhGiaDauVao = row["DiemDanhGiaDauVao"] == DBNull.Value ? null : Convert.ToDecimal(row["DiemDanhGiaDauVao"]),
                    SoPhutHocTichLuy = Convert.ToInt32(row["SoPhutHocTichLuy"]),
                    GhiChuCaNhan = row["GhiChuCaNhan"]?.ToString(),
                    NgayCapNhat = Convert.ToDateTime(row["NgayCapNhat"])
                };
            }
            return null;
        }

        public bool LuuKetQuaDanhGiaBanDau(int maNguoiDung, string trinhDo, string khungNangLuc, decimal? diemMucTieu, decimal? diemDanhGiaDauVao = null, string? ghiChu = null, string? trinhDoDauVao = null)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM HoSoHocTap WHERE MaNguoiDung = @MaND)
                BEGIN
                    UPDATE HoSoHocTap
                    SET TrinhDoMucTieu = @TrinhDo,
                        KhungNangLucMucTieu = @Khung,
                        DiemMucTieu = @Diem,
                        TrinhDoDauVao = ISNULL(@TrinhDoDauVao, TrinhDoDauVao),
                        DiemDanhGiaDauVao = ISNULL(@DiemDanhGia, DiemDanhGiaDauVao),
                        GhiChuCaNhan = ISNULL(@GhiChu, GhiChuCaNhan),
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaNguoiDung = @MaND;
                END
                ELSE
                BEGIN
                    INSERT INTO HoSoHocTap (MaNguoiDung, TrinhDoMucTieu, KhungNangLucMucTieu, DiemMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, GhiChuCaNhan, SoPhutHocTichLuy, NgayCapNhat)
                    VALUES (@MaND, @TrinhDo, @Khung, @Diem, @TrinhDoDauVao, @DiemDanhGia, @GhiChu, 0, SYSDATETIME());
                END";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@TrinhDo", (object?)trinhDo ?? DBNull.Value),
                new SqlParameter("@Khung", (object?)khungNangLuc ?? DBNull.Value),
                new SqlParameter("@Diem", (object?)diemMucTieu ?? DBNull.Value),
                new SqlParameter("@TrinhDoDauVao", (object?)trinhDoDauVao ?? DBNull.Value),
                new SqlParameter("@DiemDanhGia", (object?)diemDanhGiaDauVao ?? DBNull.Value),
                new SqlParameter("@GhiChu", (object?)ghiChu ?? DBNull.Value)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CapNhatThongTinMucTieu(int maNguoiDung, string khungNangLuc, decimal? diemMucTieu)
        {
            string query = @"
                UPDATE HoSoHocTap
                SET KhungNangLucMucTieu = @Khung,
                    DiemMucTieu = @Diem,
                    NgayCapNhat = SYSDATETIME()
                WHERE MaNguoiDung = @MaND;";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@Khung", (object?)khungNangLuc ?? DBNull.Value),
                new SqlParameter("@Diem", (object?)diemMucTieu ?? DBNull.Value)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CapNhatTrinhDoDauVao(int maNguoiDung, string trinhDoDauVao)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM HoSoHocTap WHERE MaNguoiDung = @MaND)
                BEGIN
                    UPDATE HoSoHocTap
                    SET TrinhDoDauVao = @TrinhDoDauVao,
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaNguoiDung = @MaND;
                END
                ELSE
                BEGIN
                    INSERT INTO HoSoHocTap (MaNguoiDung, TrinhDoDauVao, SoPhutHocTichLuy, NgayCapNhat)
                    VALUES (@MaND, @TrinhDoDauVao, 0, SYSDATETIME());
                END";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@TrinhDoDauVao", (object?)trinhDoDauVao ?? DBNull.Value)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CapNhatDiemDanhGiaDauVao(int maNguoiDung, decimal diemDanhGiaDauVao, string? ghiChu = null)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM HoSoHocTap WHERE MaNguoiDung = @MaND)
                BEGIN
                    UPDATE HoSoHocTap
                    SET DiemDanhGiaDauVao = @DiemDanhGia,
                        GhiChuCaNhan = ISNULL(@GhiChu, GhiChuCaNhan),
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaNguoiDung = @MaND;
                END
                ELSE
                BEGIN
                    INSERT INTO HoSoHocTap (MaNguoiDung, DiemDanhGiaDauVao, GhiChuCaNhan, SoPhutHocTichLuy, NgayCapNhat)
                    VALUES (@MaND, @DiemDanhGia, @GhiChu, 0, SYSDATETIME());
                END";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@DiemDanhGia", diemDanhGiaDauVao),
                new SqlParameter("@GhiChu", (object?)ghiChu ?? DBNull.Value)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CapNhatHoSoHocTap(int maNguoiDung, string? trinhDoDauVao, string khungNangLuc, decimal? diemMucTieu, string? ghiChu = null)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM HoSoHocTap WHERE MaNguoiDung = @MaND)
                BEGIN
                    UPDATE HoSoHocTap
                    SET TrinhDoDauVao = ISNULL(@TrinhDoDauVao, TrinhDoDauVao),
                        KhungNangLucMucTieu = @Khung,
                        DiemMucTieu = @Diem,
                        GhiChuCaNhan = ISNULL(@GhiChu, GhiChuCaNhan),
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaNguoiDung = @MaND;
                END
                ELSE
                BEGIN
                    INSERT INTO HoSoHocTap (MaNguoiDung, TrinhDoDauVao, KhungNangLucMucTieu, DiemMucTieu, GhiChuCaNhan, SoPhutHocTichLuy, NgayCapNhat)
                    VALUES (@MaND, @TrinhDoDauVao, @Khung, @Diem, @GhiChu, 0, SYSDATETIME());
                END";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@TrinhDoDauVao", (object?)trinhDoDauVao ?? DBNull.Value),
                new SqlParameter("@Khung", (object?)khungNangLuc ?? DBNull.Value),
                new SqlParameter("@Diem", (object?)diemMucTieu ?? DBNull.Value),
                new SqlParameter("@GhiChu", (object?)ghiChu ?? DBNull.Value)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        // Legacy overload kept for backward compatibility
        public bool CapNhatHoSoHocTap(int maNguoiDung, string? trinhDoDauVao, string khungNangLuc, decimal? diemMucTieu) => CapNhatHoSoHocTap(maNguoiDung, trinhDoDauVao, khungNangLuc, diemMucTieu, null);
        private bool CapNhatHoSoHocTapOld_Unused(int maNguoiDung, string? trinhDoDauVao, string khungNangLuc, decimal? diemMucTieu)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM HoSoHocTap WHERE MaNguoiDung = @MaND)
                BEGIN
                    UPDATE HoSoHocTap
                    SET TrinhDoDauVao = ISNULL(@TrinhDoDauVao, TrinhDoDauVao),
                        KhungNangLucMucTieu = @Khung,
                        DiemMucTieu = @Diem,
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaNguoiDung = @MaND;
                END
                ELSE
                BEGIN
                    INSERT INTO HoSoHocTap (MaNguoiDung, TrinhDoDauVao, KhungNangLucMucTieu, DiemMucTieu, SoPhutHocTichLuy, NgayCapNhat)
                    VALUES (@MaND, @TrinhDoDauVao, @Khung, @Diem, 0, SYSDATETIME());
                END";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@TrinhDoDauVao", (object?)trinhDoDauVao ?? DBNull.Value),
                new SqlParameter("@Khung", (object?)khungNangLuc ?? DBNull.Value),
                new SqlParameter("@Diem", (object?)diemMucTieu ?? DBNull.Value)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}
