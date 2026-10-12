using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class CauHoiDAL
    {
        private static CauHoiDAL? _instance;
        public static CauHoiDAL Instance => _instance ??= new CauHoiDAL();

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiKhaoSat(int maBaiKhaoSat = 1)
        {
            List<CauHoiKhaoSatDTO> danhSach = new List<CauHoiKhaoSatDTO>();

            string queryCauHoi = @"
                SELECT ch.MaCauHoi, chk.ThuTuCauHoi, ch.KyNang, td.MaCapDo AS MaTrinhDo, 
                       td.TenTrinhDo, ch.NoiDungCauHoi, ch.DoanVanDoc, ch.GiaiThichDapAn
                FROM CauHoiKhaoSat chk
                INNER JOIN CauHoi ch ON chk.MaCauHoi = ch.MaCauHoi
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                WHERE chk.MaBaiKhaoSat = @MaBai
                ORDER BY chk.ThuTuCauHoi ASC";

            DataTable dtCauHoi = SqlDataProvider.Instance.ExecuteQuery(queryCauHoi, new[] { new SqlParameter("@MaBai", maBaiKhaoSat) });

            if (dtCauHoi.Rows.Count == 0)
            {
                // Fallback nếu chưa gán vào bảng CauHoiKhaoSat: lấy trực tiếp 15 câu đã kiểm duyệt từ CauHoi
                string fallbackQuery = @"
                    SELECT TOP 15 ch.MaCauHoi, ROW_NUMBER() OVER(ORDER BY ch.MaCauHoi) AS ThuTuCauHoi, 
                           ch.KyNang, td.MaCapDo AS MaTrinhDo, td.TenTrinhDo, ch.NoiDungCauHoi, 
                           ch.DoanVanDoc, ch.GiaiThichDapAn
                    FROM CauHoi ch
                    LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                    WHERE ch.DaKiemDuyet = 1
                    ORDER BY ch.MaCauHoi ASC";
                dtCauHoi = SqlDataProvider.Instance.ExecuteQuery(fallbackQuery);
            }

            foreach (DataRow row in dtCauHoi.Rows)
            {
                var q = new CauHoiKhaoSatDTO
                {
                    MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                    ThuTuCauHoi = Convert.ToInt32(row["ThuTuCauHoi"]),
                    KyNang = row["KyNang"]?.ToString() ?? "NguPhap",
                    MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "A1",
                    TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                    NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                    DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                    GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString()
                };

                // Lấy các lựa chọn cho câu hỏi này
                string queryLuaChon = @"
                    SELECT MaLuaChon, MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung
                    FROM LuaChonDapAn
                    WHERE MaCauHoi = @MaCauHoi
                    ORDER BY KyHieuLuaChon ASC";

                DataTable dtLuaChon = SqlDataProvider.Instance.ExecuteQuery(queryLuaChon, new[] { new SqlParameter("@MaCauHoi", q.MaCauHoi) });
                foreach (DataRow rLC in dtLuaChon.Rows)
                {
                    q.DanhSachLuaChon.Add(new LuaChonDapAnDTO
                    {
                        MaLuaChon = Convert.ToInt32(rLC["MaLuaChon"]),
                        MaCauHoi = Convert.ToInt32(rLC["MaCauHoi"]),
                        KyHieuLuaChon = rLC["KyHieuLuaChon"]?.ToString() ?? "",
                        NoiDungLuaChon = rLC["NoiDungLuaChon"]?.ToString() ?? "",
                        LaDapAnDung = Convert.ToBoolean(rLC["LaDapAnDung"])
                    });
                }

                danhSach.Add(q);
            }

            return danhSach;
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiTheoTrinhDo(string maCapDo, int soLuong = 10)
        {
            List<CauHoiKhaoSatDTO> danhSach = new List<CauHoiKhaoSatDTO>();

            string query = @"
                SELECT TOP (@Top) ch.MaCauHoi, ROW_NUMBER() OVER(ORDER BY ch.MaCauHoi) AS ThuTuCauHoi, 
                       ch.KyNang, td.MaCapDo AS MaTrinhDo, td.TenTrinhDo, ch.NoiDungCauHoi, 
                       ch.DoanVanDoc, ch.GiaiThichDapAn
                FROM CauHoi ch
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                WHERE ch.DaKiemDuyet = 1 AND td.MaCapDo = @MaCapDo
                ORDER BY ch.MaCauHoi ASC";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaCapDo", maCapDo),
                new SqlParameter("@Top", soLuong)
            };

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);

            int thuTu = 1;
            foreach (DataRow row in dt.Rows)
            {
                var q = new CauHoiKhaoSatDTO
                {
                    MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                    ThuTuCauHoi = thuTu++,
                    KyNang = row["KyNang"]?.ToString() ?? "NguPhap",
                    MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? maCapDo,
                    TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                    NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                    DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                    GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString()
                };

                string queryLuaChon = @"
                    SELECT MaLuaChon, MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung
                    FROM LuaChonDapAn
                    WHERE MaCauHoi = @MaCauHoi
                    ORDER BY KyHieuLuaChon ASC";

                DataTable dtLuaChon = SqlDataProvider.Instance.ExecuteQuery(queryLuaChon, new[] { new SqlParameter("@MaCauHoi", q.MaCauHoi) });
                foreach (DataRow rLC in dtLuaChon.Rows)
                {
                    q.DanhSachLuaChon.Add(new LuaChonDapAnDTO
                    {
                        MaLuaChon = Convert.ToInt32(rLC["MaLuaChon"]),
                        MaCauHoi = Convert.ToInt32(rLC["MaCauHoi"]),
                        KyHieuLuaChon = rLC["KyHieuLuaChon"]?.ToString() ?? "",
                        NoiDungLuaChon = rLC["NoiDungLuaChon"]?.ToString() ?? "",
                        LaDapAnDung = Convert.ToBoolean(rLC["LaDapAnDung"])
                    });
                }

                danhSach.Add(q);
            }

            return danhSach;
        }

        public int LuuLuotKhaoSat(int maNguoiDung, int maBaiKhaoSat, int tongSoCau, int soCauDung, decimal diem, string trinhDo)
        {
            string query = @"
                DECLARE @MaBaiActual INT = (SELECT TOP 1 MaBaiKhaoSat FROM BaiKhaoSatDauVao WHERE MaBaiKhaoSat = @MaBai);
                IF @MaBaiActual IS NULL
                BEGIN
                    SET @MaBaiActual = (SELECT TOP 1 MaBaiKhaoSat FROM BaiKhaoSatDauVao);
                END
                IF @MaBaiActual IS NULL
                BEGIN
                    INSERT INTO BaiKhaoSatDauVao (TenBaiKhaoSat, MoTa, TongSoCauHoi, DangHoatDong, NgayTao)
                    VALUES (N'Khảo sát đánh giá năng lực đầu vào CEFR', N'Bài khảo sát phân loại trình độ từ A1 đến C1', 15, 1, SYSDATETIME());
                    SET @MaBaiActual = SCOPE_IDENTITY();
                END

                DECLARE @MaTD INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = @TrinhDo);
                
                INSERT INTO LuotKhaoSat (MaNguoiDung, MaBaiKhaoSat, ThoiDiemBatDau, ThoiDiemNopBai, TongSoCau, SoCauDung, DiemDatDuoc, MaTrinhDoKetQua, KhungDanhGiaKetQua)
                VALUES (@MaND, @MaBaiActual, DATEADD(MINUTE, -15, SYSDATETIME()), SYSDATETIME(), @TongSo, @SoDung, @Diem, @MaTD, N'CEFR');
                
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@MaBai", maBaiKhaoSat),
                new SqlParameter("@TongSo", tongSoCau),
                new SqlParameter("@SoDung", soCauDung),
                new SqlParameter("@Diem", diem),
                new SqlParameter("@TrinhDo", trinhDo)
            };

            object? id = SqlDataProvider.Instance.ExecuteScalar(query, parameters);
            return Convert.ToInt32(id);
        }

        public void LuuChiTietCauTraLoi(int maLuotKhaoSat, int maCauHoi, int? maLuaChon, bool laDung)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM CauHoi WHERE MaCauHoi = @MaCauHoi)
                BEGIN
                    DECLARE @MaLC INT = @MaLuaChon;
                    IF @MaLC IS NOT NULL AND NOT EXISTS (SELECT 1 FROM LuaChonDapAn WHERE MaLuaChon = @MaLC)
                        SET @MaLC = NULL;

                    INSERT INTO CauTraLoiKhaoSat (MaLuotKhaoSat, MaCauHoi, MaLuaChonDaChon, KetQuaDungSai, ThoiDiemTraLoi)
                    VALUES (@MaLuot, @MaCauHoi, @MaLC, @KetQua, SYSDATETIME());
                END";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaLuot", maLuotKhaoSat),
                new SqlParameter("@MaCauHoi", maCauHoi),
                new SqlParameter("@MaLuaChon", (object?)maLuaChon ?? DBNull.Value),
                new SqlParameter("@KetQua", laDung ? 1 : 0)
            };

            SqlDataProvider.Instance.ExecuteNonQuery(query, parameters);
        }

        public KetQuaKhaoSatDTO? LayKetQuaKhaoSatGanNhat(int maNguoiDung)
        {
            if (maNguoiDung <= 0) return null;

            string query = @"
                SELECT TOP 1 lks.MaLuotKhaoSat, lks.TongSoCau, lks.SoCauDung, lks.DiemDatDuoc, 
                             td.MaCapDo AS MaTrinhDo, td.TenTrinhDo
                FROM LuotKhaoSat lks
                LEFT JOIN TrinhDo td ON lks.MaTrinhDoKetQua = td.MaTrinhDo
                WHERE lks.MaNguoiDung = @MaND
                ORDER BY lks.ThoiDiemNopBai DESC, lks.MaLuotKhaoSat DESC";

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaND", maNguoiDung) });
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                string maCapDo = row["MaTrinhDo"]?.ToString() ?? "B1";
                string tenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "Intermediate (Trung cấp)";
                int tongSoCau = Convert.ToInt32(row["TongSoCau"]);
                int soCauDung = Convert.ToInt32(row["SoCauDung"]);
                decimal diem = Convert.ToDecimal(row["DiemDatDuoc"]);

                return new KetQuaKhaoSatDTO
                {
                    TongSoCau = tongSoCau,
                    SoCauDung = soCauDung,
                    DiemSo = diem,
                    TrinhDoDaChon = maCapDo,
                    KetQuaDat = true,
                    TrinhDoPhuHop = maCapDo,
                    TenTrinhDoPhuHop = tenTrinhDo
                };
            }

            return null;
        }
    }
}
