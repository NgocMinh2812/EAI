using System;
using DAL;
using DTO;

namespace BUS
{
    public class DashboardBUS
    {
        private readonly DashboardDAL _dal = new DashboardDAL();

        public DashboardKpiDTO LaySoLieuTongQuan(int filterPeriod = 0)
        {
            return _dal.GetKpiStats(filterPeriod);
        }

        public (bool IsConnected, long LatencyMs, string Message) KiemTraKetNoiDatabase()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                using var conn = SqlDataProvider.Instance.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT 1";
                cmd.CommandTimeout = 3;
                cmd.ExecuteScalar();
                sw.Stop();
                return (true, sw.ElapsedMilliseconds, $"Kết nối tốt ({sw.ElapsedMilliseconds}ms)");
            }
            catch (Exception ex)
            {
                sw.Stop();
                return (false, sw.ElapsedMilliseconds, $"Lỗi kết nối: {ex.Message}");
            }
        }
    }
}
