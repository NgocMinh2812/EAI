using System;
using System.Collections.Generic;

namespace DTO
{
    public class ThongKeHocTapDTO
    {
        public double DiemTrungBinh { get; set; } = 0;
        public int SoBaiDaHoanThanh { get; set; } = 0;
        public int TongSoBaiHoc { get; set; } = 0;
        public int SoPhutHocTichLuy { get; set; } = 0;
        public int ChuoiHocLienTiep { get; set; } = 1;
        public int TongSoLuotThi { get; set; } = 0;

        // Skill mastery percentages (0-100)
        public int TyLeTuVung { get; set; } = 75;
        public int TyLeNguPhap { get; set; } = 70;
        public int TyLeDocHieu { get; set; } = 65;
        public int TyLeNgheHieu { get; set; } = 60;

        // Weekly activity (minutes per day Mon-Sun)
        public int[] ThoiGianTuanPhut { get; set; } = new int[] { 25, 40, 15, 60, 45, 30, 50 };

        public List<BaiHocDTO> DanhSachBaiHocGanDay { get; set; } = new List<BaiHocDTO>();

        public string DiemTrungBinhHienThi => DiemTrungBinh > 0 ? $"{DiemTrungBinh:0.0}%" : "Chưa có";
    }
}
