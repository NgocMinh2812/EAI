using System;
using DAL;
using DTO;

namespace BUS
{
    public class DashboardBUS
    {
        private readonly DashboardDAL _dal = new DashboardDAL();

        public DashboardKpiDTO LaySoLieuTongQuan()
        {
            return _dal.GetKpiStats();
        }
    }
}
