using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DTO
{
    public class CauHinhQuyDoiKhaoSatDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _maCauHinh;
        private string _maCapDo = string.Empty;
        private string _tenHienThi = string.Empty;
        private int _diemPhanTramMin;
        private int _diemPhanTramMax;
        private string _quyDoiIelts = string.Empty;
        private string _quyDoiToeic = string.Empty;
        private string _loTrinhDeXuat = string.Empty;
        private string _mauSac = "#0F4C3A";
        private int _thuTuSapXep = 1;

        public int MaCauHinh
        {
            get => _maCauHinh;
            set { if (_maCauHinh != value) { _maCauHinh = value; OnPropertyChanged(); } }
        }

        public string MaCapDo
        {
            get => _maCapDo;
            set { if (_maCapDo != value) { _maCapDo = value; OnPropertyChanged(); } }
        }

        public string TenHienThi
        {
            get => _tenHienThi;
            set { if (_tenHienThi != value) { _tenHienThi = value; OnPropertyChanged(); } }
        }

        public int DiemPhanTramMin
        {
            get => _diemPhanTramMin;
            set { if (_diemPhanTramMin != value) { _diemPhanTramMin = value; OnPropertyChanged(); OnPropertyChanged(nameof(KhoangDiemHienThi)); } }
        }

        public int DiemPhanTramMax
        {
            get => _diemPhanTramMax;
            set { if (_diemPhanTramMax != value) { _diemPhanTramMax = value; OnPropertyChanged(); OnPropertyChanged(nameof(KhoangDiemHienThi)); } }
        }

        public string KhoangDiemHienThi => $"{DiemPhanTramMin}% - {DiemPhanTramMax}%";

        public string QuyDoiIelts
        {
            get => _quyDoiIelts;
            set { if (_quyDoiIelts != value) { _quyDoiIelts = value; OnPropertyChanged(); } }
        }

        public string QuyDoiToeic
        {
            get => _quyDoiToeic;
            set { if (_quyDoiToeic != value) { _quyDoiToeic = value; OnPropertyChanged(); } }
        }

        public string LoTrinhDeXuat
        {
            get => _loTrinhDeXuat;
            set { if (_loTrinhDeXuat != value) { _loTrinhDeXuat = value; OnPropertyChanged(); } }
        }

        public string MauSac
        {
            get => _mauSac;
            set { if (_mauSac != value) { _mauSac = value; OnPropertyChanged(); } }
        }

        public int ThuTuSapXep
        {
            get => _thuTuSapXep;
            set { if (_thuTuSapXep != value) { _thuTuSapXep = value; OnPropertyChanged(); } }
        }

        public string BadgeBg => MaCapDo switch
        {
            "A1" => "#FEF2F2",
            "A2" => "#FFF7ED",
            "B1" => "#EFF6FF",
            "B2" => "#F0FDF4",
            "C1" => "#FAF5FF",
            "C2" => "#FEFCE8",
            _ => "#F8FAFC"
        };

        public string BadgeBorder => MaCapDo switch
        {
            "A1" => "#FECACA",
            "A2" => "#FED7AA",
            "B1" => "#BFDBFE",
            "B2" => "#BBF7D0",
            "C1" => "#E9D5FF",
            "C2" => "#FEF08A",
            _ => "#E2E8F0"
        };

        public string BadgeFg => !string.IsNullOrEmpty(MauSac) ? MauSac : MaCapDo switch
        {
            "A1" => "#DC2626",
            "A2" => "#EA580C",
            "B1" => "#2563EB",
            "B2" => "#15803D",
            "C1" => "#7E22CE",
            "C2" => "#A16207",
            _ => "#334155"
        };
    }
}
