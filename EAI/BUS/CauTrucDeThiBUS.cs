using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class CauTrucDeThiBUS
    {
        private static CauTrucDeThiBUS? _instance;
        public static CauTrucDeThiBUS Instance => _instance ??= new CauTrucDeThiBUS();

        private readonly CauTrucDeThiDAL _dal = CauTrucDeThiDAL.Instance;

        public List<CauTrucDeThiDTO> LayDanhSachCauTruc(string? maKyThi = null, string? loaiDeThi = null)
        {
            return _dal.LayDanhSachCauTruc(maKyThi, loaiDeThi);
        }

        public CauTrucDeThiDTO? LayTheoMa(int maCauTruc)
        {
            return _dal.LayTheoMa(maCauTruc);
        }

        public List<ChiTietCauTrucDeThiDTO> LayChiTietPhanThi(int maCauTruc)
        {
            return _dal.LayChiTietPhanThi(maCauTruc);
        }
    }
}

