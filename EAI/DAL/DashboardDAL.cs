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

            // 1. Tổng số học viên
            var queryHocVien = @"
                SELECT COUNT(*) 
                FROM NguoiDung nd 
                INNER JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro 
                WHERE vt.TenVaiTro = N'HocVien'";
            kpi.TongHocVien = Convert.ToInt32(DatabaseHelper.ExecuteScalar(queryHocVien) ?? 0);

            // 2. Tổng số bài học
            var queryBaiHoc = "SELECT COUNT(*) FROM BaiHoc";
            kpi.TongBaiHoc = Convert.ToInt32(DatabaseHelper.ExecuteScalar(queryBaiHoc) ?? 0);

            // 3. Tổng số đề thi / luyện tập
            var queryDeThi = "SELECT COUNT(*) FROM BaiTapDeThi";
            kpi.TongDeThi = Convert.ToInt32(DatabaseHelper.ExecuteScalar(queryDeThi) ?? 0);

            // 4. Số mục chờ duyệt (Câu hỏi AI hoặc Đề thi)
            var queryChoDuyet = @"
                SELECT 
                    (SELECT COUNT(*) FROM CauHoiDoAISinh WHERE TrangThaiDuyet = N'DangCho') +
                    (SELECT COUNT(*) FROM CauHoi WHERE DaKiemDuyet = 0)";
            kpi.ChoDuyet = Convert.ToInt32(DatabaseHelper.ExecuteScalar(queryChoDuyet) ?? 0);

            // 5. Thống kê phụ người dùng
            var queryUserStats = @"
                SELECT 
                    COUNT(*) AS TongND,
                    SUM(CASE WHEN TrangThaiHoatDong = 1 THEN 1 ELSE 0 END) AS ActiveND,
                    SUM(CASE WHEN TrangThaiHoatDong = 0 THEN 1 ELSE 0 END) AS LockedND
                FROM NguoiDung";
            var dt = DatabaseHelper.ExecuteQuery(queryUserStats);
            if (dt.Rows.Count > 0)
            {
                kpi.TongNguoiDung = Convert.ToInt32(dt.Rows[0]["TongND"] == DBNull.Value ? 0 : dt.Rows[0]["TongND"]);
                kpi.DangHoatDong = Convert.ToInt32(dt.Rows[0]["ActiveND"] == DBNull.Value ? 0 : dt.Rows[0]["ActiveND"]);
                kpi.DaKhoa = Convert.ToInt32(dt.Rows[0]["LockedND"] == DBNull.Value ? 0 : dt.Rows[0]["LockedND"]);
            }

            // 6. Tổng số chủ đề
            kpi.TongChuDe = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM ChuDe") ?? 0);

            return kpi;
        }
    }
}
