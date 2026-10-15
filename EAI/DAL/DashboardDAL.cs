using System;
using System.Data;
using DTO;

namespace DAL
{
    public class DashboardDAL
    {
        public DashboardKpiDTO GetKpiStats()
        {
            var kpi = new DashboardKpiDTO();

            string query = @"
                SELECT 
                    (SELECT COUNT(*) FROM NguoiDung nd INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro WHERE vt.TenVaiTro = N'HocVien') AS TongHocVien,
                    (SELECT COUNT(*) FROM BaiHoc) AS TongBaiHoc,
                    (SELECT COUNT(*) FROM BaiTapDeThi) AS TongDeThi,
                    (ISNULL((SELECT COUNT(*) FROM CauHoiDoAISinh WHERE TrangThaiDuyet = N'DangCho'), 0) + ISNULL((SELECT COUNT(*) FROM CauHoi WHERE DaKiemDuyet = 0), 0)) AS ChoDuyet,
                    (SELECT COUNT(*) FROM NguoiDung) AS TongND,
                    (SELECT ISNULL(SUM(CASE WHEN TrangThaiHoatDong = 1 THEN 1 ELSE 0 END), 0) FROM NguoiDung) AS ActiveND,
                    (SELECT ISNULL(SUM(CASE WHEN TrangThaiHoatDong = 0 THEN 1 ELSE 0 END), 0) FROM NguoiDung) AS LockedND,
                    (SELECT COUNT(*) FROM ChuDe) AS TongChuDe;";

            try
            {
                var dt = DatabaseHelper.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    kpi.TongHocVien = r["TongHocVien"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongHocVien"]);
                    kpi.TongBaiHoc = r["TongBaiHoc"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongBaiHoc"]);
                    kpi.TongDeThi = r["TongDeThi"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongDeThi"]);
                    kpi.ChoDuyet = r["ChoDuyet"] == DBNull.Value ? 0 : Convert.ToInt32(r["ChoDuyet"]);
                    kpi.TongNguoiDung = r["TongND"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongND"]);
                    kpi.DangHoatDong = r["ActiveND"] == DBNull.Value ? 0 : Convert.ToInt32(r["ActiveND"]);
                    kpi.DaKhoa = r["LockedND"] == DBNull.Value ? 0 : Convert.ToInt32(r["LockedND"]);
                    kpi.TongChuDe = r["TongChuDe"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongChuDe"]);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DashboardDAL] Lỗi lấy số liệu KPI: {ex.Message}");
            }

            return kpi;
        }
    }
}
