using System;
using System.Collections.Generic;

namespace DTO
{
    public class LuaChonDapAnDTO
    {
        public int MaLuaChon { get; set; }
        public int MaCauHoi { get; set; }
        public string KyHieuLuaChon { get; set; } = string.Empty; // A, B, C, D
        public string NoiDungLuaChon { get; set; } = string.Empty;
        public bool LaDapAnDung { get; set; }
    }

    public class CauHoiKhaoSatDTO
    {
        public int MaCauHoi { get; set; }
        public int ThuTuCauHoi { get; set; }
        public string KyNang { get; set; } = string.Empty; // NguPhap, TuVung, Doc
        public string MaTrinhDo { get; set; } = string.Empty; // A1, A2, B1, B2
        public string TenTrinhDo { get; set; } = string.Empty;
        public string NoiDungCauHoi { get; set; } = string.Empty;
        public string? DoanVanDoc { get; set; }
        public string? GiaiThichDapAn { get; set; }

        public int? MaChuDe { get; set; }
        public int? MaTrinhDoId { get; set; }
        public string LoaiCauHoi { get; set; } = "TracNghiem"; // TracNghiem, DienKhuyet
        public string DoKho { get; set; } = "TrungBinh"; // De, TrungBinh, Kho
        public string? AudioPath { get; set; }
        public bool DaKiemDuyet { get; set; } = true;
        public int? NguoiTaoId { get; set; }
        public DateTime? NgayTao { get; set; }

        public List<LuaChonDapAnDTO> DanhSachLuaChon { get; set; } = new List<LuaChonDapAnDTO>();

        // Lựa chọn hiện tại của học viên
        public int? MaLuaChonDaChon { get; set; }
        public int ChiMucDaChon { get; set; } = -1; // 0: A, 1: B, 2: C, 3: D

        public string ChuyenMucHienThi
        {
            get
            {
                string kyNangStr = KyNang switch
                {
                    "NguPhap" => "NGỮ PHÁP",
                    "TuVung" => "TỪ VỰNG",
                    "Doc" => "ĐỌC HIỂU",
                    _ => KyNang.ToUpper()
                };
                return $"{kyNangStr} • TRÌNH ĐỘ {MaTrinhDo}";
            }
        }
    }

    public class KetQuaKhaoSatDTO
    {
        public int TongSoCau { get; set; }
        public int SoCauDung { get; set; }
        public decimal DiemSo { get; set; } // Thang 10

        // Thông tin tự chọn & đánh giá thích ứng
        public string TrinhDoDaChon { get; set; } = "B1"; // Cấp độ học viên tự chọn ban đầu
        public bool KetQuaDat { get; set; } = true; // true: Đạt chuẩn cấp độ đã chọn, false: Chưa đạt (hạ cấp)
        public string TrinhDoPhuHop { get; set; } = "B1"; // Cấp độ AI xác định phù hợp
        public string TenTrinhDoPhuHop { get; set; } = "Intermediate (Trung cấp)";
        public string LyDoDieuChinh { get; set; } = string.Empty;

        // Tương thích
        public string TrinhDoCEFR => TrinhDoPhuHop;
        public string TenTrinhDo => TenTrinhDoPhuHop;
        public string UocTinhIelts { get; set; } = "5.0 - 5.5";
        public string UocTinhToeic { get; set; } = "550 - 650";
        public string NhanXetLộTrinh { get; set; } = string.Empty;

        // Danh sách bài học AI đề xuất phù hợp năng lực
        public List<BaiHocDTO> DanhSachBaiHocDeXuat { get; set; } = new List<BaiHocDTO>();
    }
}
