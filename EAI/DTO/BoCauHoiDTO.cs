using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DTO
{
    public class BoCauHoiDTO : INotifyPropertyChanged
    {
        private int _maBoCauHoi;
        private string _tenBoCauHoi = string.Empty;
        private string? _moTa;
        private string _nguonTao = "AI"; // "AI_DanhMuc", "AI_BaiHoc", "ThuCong", "HeThong"
        private string? _tenNguon; // Tên tài liệu / bài học nguồn
        private int? _maBaiHoc;
        private int? _maChuDe;
        private int? _maTrinhDo;
        private int? _nguoiTao;
        private DateTime _ngayTao = DateTime.Now;
        private DateTime? _ngayCapNhat;
        private int _soLuongCauHoi;

        public int MaBoCauHoi
        {
            get => _maBoCauHoi;
            set { if (_maBoCauHoi != value) { _maBoCauHoi = value; OnPropertyChanged(); } }
        }

        public string TenBoCauHoi
        {
            get => _tenBoCauHoi;
            set { if (_tenBoCauHoi != value) { _tenBoCauHoi = value; OnPropertyChanged(); } }
        }

        public string? MoTa
        {
            get => _moTa;
            set { if (_moTa != value) { _moTa = value; OnPropertyChanged(); } }
        }

        public string NguonTao
        {
            get => _nguonTao;
            set 
            { 
                if (_nguonTao != value) 
                { 
                    _nguonTao = value; 
                    OnPropertyChanged(); 
                    OnPropertyChanged(nameof(NguonTaoHienThi)); 
                } 
            }
        }

        public string? TenNguon
        {
            get => _tenNguon;
            set { if (_tenNguon != value) { _tenNguon = value; OnPropertyChanged(); } }
        }

        public int? MaBaiHoc
        {
            get => _maBaiHoc;
            set { if (_maBaiHoc != value) { _maBaiHoc = value; OnPropertyChanged(); } }
        }

        public int? MaChuDe
        {
            get => _maChuDe;
            set { if (_maChuDe != value) { _maChuDe = value; OnPropertyChanged(); } }
        }

        public int? MaTrinhDo
        {
            get => _maTrinhDo;
            set { if (_maTrinhDo != value) { _maTrinhDo = value; OnPropertyChanged(); } }
        }

        public int? NguoiTao
        {
            get => _nguoiTao;
            set { if (_nguoiTao != value) { _nguoiTao = value; OnPropertyChanged(); } }
        }

        public DateTime NgayTao
        {
            get => _ngayTao;
            set 
            { 
                if (_ngayTao != value) 
                { 
                    _ngayTao = value; 
                    OnPropertyChanged(); 
                    OnPropertyChanged(nameof(NgayTaoHienThi)); 
                } 
            }
        }

        public DateTime? NgayCapNhat
        {
            get => _ngayCapNhat;
            set { if (_ngayCapNhat != value) { _ngayCapNhat = value; OnPropertyChanged(); } }
        }

        public int SoLuongCauHoi
        {
            get => _soLuongCauHoi;
            set { if (_soLuongCauHoi != value) { _soLuongCauHoi = value; OnPropertyChanged(); } }
        }

        public string NguonTaoHienThi => NguonTao switch
        {
            "AI_DanhMuc" => "AI • Danh mục",
            "AI_BaiHoc" => "AI • Bài học",
            "ThuCong" => "Thủ công",
            "HeThong" => "Hệ thống",
            _ => NguonTao
        };

        public string NgayTaoHienThi => NgayTao.ToString("dd/MM/yyyy HH:mm");

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
