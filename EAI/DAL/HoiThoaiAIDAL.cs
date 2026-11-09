using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class HoiThoaiAIDAL
    {
        private static HoiThoaiAIDAL? _instance;
        public static HoiThoaiAIDAL Instance => _instance ??= new HoiThoaiAIDAL();

        public List<PhienHoiThoaiDTO> LayDanhSachPhienHoiThoai(int maNguoiDung)
        {
            var list = new List<PhienHoiThoaiDTO>();
            string query = @"
                SELECT p.MaPhienHoiThoai, p.MaNguoiDung, p.TieuDe, p.NguCanhHoiThoai, 
                       p.MaCauHoiLienQuan, p.MaBaiHocLienQuan, p.NgayTao, p.TinNhanCuoiVao,
                       (SELECT COUNT(*) FROM TinNhanAI t WHERE t.MaPhienHoiThoai = p.MaPhienHoiThoai) AS SoTinNhan
                FROM PhienHoiThoaiAI p
                WHERE p.MaNguoiDung = @MaNguoiDung
                ORDER BY ISNULL(p.TinNhanCuoiVao, p.NgayTao) DESC";

            SqlParameter[] parameters = { new SqlParameter("@MaNguoiDung", maNguoiDung) };
            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new PhienHoiThoaiDTO
                {
                    MaPhienHoiThoai = Convert.ToInt64(row["MaPhienHoiThoai"]),
                    MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                    TieuDe = row["TieuDe"] != DBNull.Value ? row["TieuDe"].ToString()! : "Cuộc hội thoại",
                    NguCanhHoiThoai = row["NguCanhHoiThoai"] != DBNull.Value ? row["NguCanhHoiThoai"].ToString()! : "General",
                    MaCauHoiLienQuan = row["MaCauHoiLienQuan"] != DBNull.Value ? Convert.ToInt32(row["MaCauHoiLienQuan"]) : null,
                    MaBaiHocLienQuan = row["MaBaiHocLienQuan"] != DBNull.Value ? Convert.ToInt32(row["MaBaiHocLienQuan"]) : null,
                    NgayTao = Convert.ToDateTime(row["NgayTao"]),
                    TinNhanCuoiVao = row["TinNhanCuoiVao"] != DBNull.Value ? Convert.ToDateTime(row["TinNhanCuoiVao"]) : null,
                    SoTinNhan = Convert.ToInt32(row["SoTinNhan"])
                });
            }

            return list;
        }

        public long TaoPhienHoiThoai(int maNguoiDung, string tieuDe, string nguCanh = "General")
        {
            string query = @"
                INSERT INTO PhienHoiThoaiAI (MaNguoiDung, TieuDe, NguCanhHoiThoai, NgayTao, TinNhanCuoiVao)
                VALUES (@MaNguoiDung, @TieuDe, @NguCanhHoiThoai, SYSDATETIME(), SYSDATETIME());
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@MaNguoiDung", maNguoiDung),
                new SqlParameter("@TieuDe", tieuDe),
                new SqlParameter("@NguCanhHoiThoai", string.IsNullOrEmpty(nguCanh) ? "General" : nguCanh)
            };

            object result = SqlDataProvider.Instance.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt64(result) : 0;
        }

        public bool CapNhatTieuDePhien(long maPhien, string tieuDe)
        {
            string query = @"
                UPDATE PhienHoiThoaiAI 
                SET TieuDe = @TieuDe, TinNhanCuoiVao = SYSDATETIME()
                WHERE MaPhienHoiThoai = @MaPhienHoiThoai";

            SqlParameter[] parameters = {
                new SqlParameter("@MaPhienHoiThoai", maPhien),
                new SqlParameter("@TieuDe", tieuDe)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool XoaPhienHoiThoai(long maPhien, int maNguoiDung)
        {
            string query = @"
                DELETE FROM TinNhanAI WHERE MaPhienHoiThoai = @MaPhienHoiThoai;
                DELETE FROM PhienHoiThoaiAI WHERE MaPhienHoiThoai = @MaPhienHoiThoai AND MaNguoiDung = @MaNguoiDung;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaPhienHoiThoai", maPhien),
                new SqlParameter("@MaNguoiDung", maNguoiDung)
            };

            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public List<TinNhanAIDTO> LayDanhSachTinNhan(long maPhien)
        {
            var list = new List<TinNhanAIDTO>();
            string query = @"
                SELECT MaTinNhan, MaPhienHoiThoai, LoaiNguoiGui, NoiDungTinNhan, 
                       SoTokenDauVao, SoTokenDauRa, NgayGui
                FROM TinNhanAI
                WHERE MaPhienHoiThoai = @MaPhienHoiThoai
                ORDER BY NgayGui ASC";

            SqlParameter[] parameters = { new SqlParameter("@MaPhienHoiThoai", maPhien) };
            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new TinNhanAIDTO
                {
                    MaTinNhan = Convert.ToInt64(row["MaTinNhan"]),
                    MaPhienHoiThoai = Convert.ToInt64(row["MaPhienHoiThoai"]),
                    LoaiNguoiGui = row["LoaiNguoiGui"].ToString()!,
                    NoiDungTinNhan = row["NoiDungTinNhan"].ToString()!,
                    SoTokenDauVao = row["SoTokenDauVao"] != DBNull.Value ? Convert.ToInt32(row["SoTokenDauVao"]) : null,
                    SoTokenDauRa = row["SoTokenDauRa"] != DBNull.Value ? Convert.ToInt32(row["SoTokenDauRa"]) : null,
                    NgayGui = Convert.ToDateTime(row["NgayGui"])
                });
            }

            return list;
        }

        public long ThemTinNhan(long maPhien, string loaiNguoiGui, string noiDung, int? tokenDauVao = 0, int? tokenDauRa = 0)
        {
            string query = @"
                INSERT INTO TinNhanAI (MaPhienHoiThoai, LoaiNguoiGui, NoiDungTinNhan, SoTokenDauVao, SoTokenDauRa, NgayGui)
                VALUES (@MaPhienHoiThoai, @LoaiNguoiGui, @NoiDungTinNhan, @SoTokenDauVao, @SoTokenDauRa, SYSDATETIME());
                
                UPDATE PhienHoiThoaiAI 
                SET TinNhanCuoiVao = SYSDATETIME() 
                WHERE MaPhienHoiThoai = @MaPhienHoiThoai;

                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@MaPhienHoiThoai", maPhien),
                new SqlParameter("@LoaiNguoiGui", loaiNguoiGui),
                new SqlParameter("@NoiDungTinNhan", noiDung),
                new SqlParameter("@SoTokenDauVao", (object?)tokenDauVao ?? DBNull.Value),
                new SqlParameter("@SoTokenDauRa", (object?)tokenDauRa ?? DBNull.Value)
            };

            object result = SqlDataProvider.Instance.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt64(result) : 0;
        }

        public int DemTongSoLuotDungTroLyAI()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM TinNhanAI WHERE LoaiNguoiGui = N'NguoiDung'";
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

