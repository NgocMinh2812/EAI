using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DTO
{
    public class ChuDeDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _maChuDe;
        public int MaChuDe
        {
            get => _maChuDe;
            set { if (_maChuDe != value) { _maChuDe = value; OnPropertyChanged(); } }
        }

        private int? _maChuDeCha;
        public int? MaChuDeCha
        {
            get => _maChuDeCha;
            set { if (_maChuDeCha != value) { _maChuDeCha = value; OnPropertyChanged(); OnPropertyChanged(nameof(TenHienThi)); } }
        }

        private string _tenChuDe = string.Empty;
        public string TenChuDe
        {
            get => _tenChuDe;
            set { if (_tenChuDe != value) { _tenChuDe = value; OnPropertyChanged(); OnPropertyChanged(nameof(TenHienThi)); } }
        }

        private string? _moTa;
        public string? MoTa
        {
            get => _moTa;
            set { if (_moTa != value) { _moTa = value; OnPropertyChanged(); } }
        }

        private bool _trangThaiHoatDong = true;
        public bool TrangThaiHoatDong
        {
            get => _trangThaiHoatDong;
            set { if (_trangThaiHoatDong != value) { _trangThaiHoatDong = value; OnPropertyChanged(); } }
        }

        // Bổ sung thống kê
        private string _tenChuDeCha = string.Empty;
        public string TenChuDeCha
        {
            get => _tenChuDeCha;
            set { if (_tenChuDeCha != value) { _tenChuDeCha = value; OnPropertyChanged(); OnPropertyChanged(nameof(TenHienThi)); } }
        }

        private int _soLuongBaiHoc = 0;
        public int SoLuongBaiHoc
        {
            get => _soLuongBaiHoc;
            set { if (_soLuongBaiHoc != value) { _soLuongBaiHoc = value; OnPropertyChanged(); } }
        }

        public List<ChuDeDTO> DanhSachCon { get; set; } = new List<ChuDeDTO>();

        public string TenHienThi => !string.IsNullOrWhiteSpace(TenChuDeCha)
            ? $"{TenChuDeCha} ❯ {TenChuDe}"
            : TenChuDe;
    }
}

