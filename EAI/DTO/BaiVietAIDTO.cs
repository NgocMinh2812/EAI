using System;
using System.Collections.Generic;

namespace DTO
{
    public class BaiVietDaChamDTO
    {
        public long MaKetQuaPhanTich { get; set; }
        public int MaNguoiDung { get; set; }
        public string DeBai { get; set; } = string.Empty;
        public string BaiVietGoc { get; set; } = string.Empty;
        public string DuLieuKetQuaRaw { get; set; } = string.Empty;
        public double DiemTongQuan { get; set; }
        public string TrinhDo { get; set; } = "B1";
        public string SoTuYeuCau { get; set; } = "80–120 từ";
        public DateTime NgayTao { get; set; } = DateTime.Now;

        public KetQuaChamBaiVietJsonDTO? KetQuaParsed { get; set; }
    }

    public class ChiTietLoiSaiDTO
    {
        public string LoaiLoi { get; set; } = "Ngữ pháp"; // Chính tả, Ngữ pháp, Diễn đạt thiếu tự nhiên, Từ vựng, Cấu trúc câu
        public string ViTriHoacDoanSai { get; set; } = string.Empty;
        public string GoiYSua { get; set; } = string.Empty;
        public string GiaiThichTiengViet { get; set; } = string.Empty;
    }

    public class KetQuaChamBaiVietJsonDTO
    {
        public double DiemTongQuan { get; set; } = 7.0;
        public double TaskAchievement { get; set; } = 7.0;
        public double CoherenceCohesion { get; set; } = 7.0;
        public double LexicalResource { get; set; } = 7.0;
        public double GrammarRangeAccuracy { get; set; } = 7.0;
        public string NhanXetTongQuan { get; set; } = string.Empty;
        public List<ChiTietLoiSaiDTO> DanhSachLoiSai { get; set; } = new();
        public string BaiVietSauChinhSua { get; set; } = string.Empty;
        public string BanGoiYVietLaiBand8 { get; set; } = string.Empty;
    }

    public class DeBaiYeuCauDTO
    {
        public string TrinhDo { get; set; } = "B1"; // A1, A2, B1, B2
        public string ChuDe { get; set; } = "Học tập"; // Bản thân, Gia đình, Học tập, Công nghệ, Sức khỏe, Du lịch, Môi trường, Công việc, Đời sống, Xã hội
        public string DangBai { get; set; } = "Nêu ý kiến"; // Miêu tả, Kể chuyện, Nêu ý kiến, Giải thích, So sánh
        public string DoDai { get; set; } = "80–120 từ"; // 50–80 từ, 80–120 từ, 120–180 từ, 180–250 từ
        public int MinWords { get; set; } = 80;
        public int MaxWords { get; set; } = 120;
    }

    public class HuongDanMoDoanDTO
    {
        public string HuongDan { get; set; } = string.Empty;
        public string CauHoiDinhHuong { get; set; } = string.Empty;
        public string CauMoDauGoiY { get; set; } = string.Empty;
        public string ViDuCauMau { get; set; } = string.Empty;
        public string GiaiThichCauTruc { get; set; } = string.Empty;
    }

    public class YChinhThanDoanDTO
    {
        public string YChinh { get; set; } = string.Empty;
        public string CauHoiDinhHuong { get; set; } = string.Empty;
        public string ViDuMinhHoa { get; set; } = string.Empty;
        public string TuNoiGoiY { get; set; } = string.Empty;
        public string TuVungNguPhap { get; set; } = string.Empty;
    }

    public class HuongDanThanDoanDTO
    {
        public string CauHoiDinhHuong { get; set; } = string.Empty;
        public List<YChinhThanDoanDTO> DanhSachYChinh { get; set; } = new();
        public string ViDuMinhHoaSatDe { get; set; } = string.Empty;
        public List<string> TuNoiKhuyenDung { get; set; } = new();
    }

    public class HuongDanKetDoanDTO
    {
        public string HuongDan { get; set; } = string.Empty;
        public string CauHoiDinhHuong { get; set; } = string.Empty;
        public string CauKetLuanGoiY { get; set; } = string.Empty;
        public string ViDuCauMau { get; set; } = string.Empty;
    }

    public class HuongDanBoCucDTO
    {
        public HuongDanMoDoanDTO MoDoan { get; set; } = new();
        public HuongDanThanDoanDTO ThanDoan { get; set; } = new();
        public HuongDanKetDoanDTO KetDoan { get; set; } = new();
    }

    public class DeBaiVietAIDTO
    {
        public long MaDeBai { get; set; }
        public string TieuDe { get; set; } = string.Empty;
        public string MoTaYeuCau { get; set; } = string.Empty;
        public string TrinhDo { get; set; } = "B1";
        public string ChuDe { get; set; } = "Học tập";
        public string DangBai { get; set; } = "Nêu ý kiến";
        public string SoTuYeuCau { get; set; } = "80–120 từ";
        public int MinWords { get; set; } = 80;
        public int MaxWords { get; set; } = 120;
        public List<string> CauHoiDinhHuong { get; set; } = new();
        public List<string> TuVungGoiY { get; set; } = new();
        public List<string> NguPhapGoiY { get; set; } = new();
        public HuongDanBoCucDTO HuongDanBoCuc { get; set; } = new();
        public bool IsFavorite { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
