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
                       TrinhDoMucTieu, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan, NgayCapNhat
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
                    DiemDanhGiaDauVao = row["DiemDanhGiaDauVao"] == DBNull.Value ? null : Convert.ToDecimal(row["DiemDanhGiaDauVao"]),
                    SoPhutHocTichLuy = Convert.ToInt32(row["SoPhutHocTichLuy"]),
                    GhiChuCaNhan = row["GhiChuCaNhan"]?.ToString(),
                    NgayCapNhat = Convert.ToDateTime(row["NgayCapNhat"])
                };
            }
            return null;
        }

        public bool LuuKetQuaDanhGiaBanDau(int maNguoiDung, string trinhDo, string khungNangLuc, decimal? diemMucTieu, decimal? diemDanhGiaDauVao = null, string? ghiChu = null)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM HoSoHocTap WHERE MaNguoiDung = @MaND)
                BEGIN
                    UPDATE HoSoHocTap
                    SET TrinhDoMucTieu = @TrinhDo,
                        KhungNangLucMucTieu = @Khung,
                        DiemMucTieu = @Diem,
                        DiemDanhGiaDauVao = ISNULL(@DiemDanhGia, DiemDanhGiaDauVao),
                        GhiChuCaNhan = ISNULL(@GhiChu, GhiChuCaNhan),
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaNguoiDung = @MaND;
                END
                ELSE
                BEGIN
                    INSERT INTO HoSoHocTap (MaNguoiDung, TrinhDoMucTieu, KhungNangLucMucTieu, DiemMucTieu, DiemDanhGiaDauVao, GhiChuCaNhan, SoPhutHocTichLuy, NgayCapNhat)
                    VALUES (@MaND, @TrinhDo, @Khung, @Diem, @DiemDanhGia, @GhiChu, 0, SYSDATETIME());
                END";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@TrinhDo", (object?)trinhDo ?? DBNull.Value),
                new SqlParameter("@Khung", (object?)khungNangLuc ?? DBNull.Value),
                new SqlParameter("@Diem", (object?)diemMucTieu ?? DBNull.Value),
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
    }
}
