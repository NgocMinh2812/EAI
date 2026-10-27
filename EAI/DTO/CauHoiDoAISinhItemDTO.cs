using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace DTO
{
    public class CauHoiDoAISinhItemDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private long _maCauHoiAISinh;
        private long _maYeuCauSinhDe;
        private int? _maCauHoi;
        private string _noiDungCauHoiSinhRa = string.Empty;
        private string _trangThaiDuyet = "DangCho";
        private int? _nguoiDuyet;
        private DateTime? _ngayDuyet;

        // Joined from YeuCauSinhDeAI
        private string _noiDungDauVao = string.Empty;
        private string _moHinhAI = "Google Gemini";
        private string _loaiDauVao = "ChuDe";
        private DateTime _ngayTao = DateTime.Now;

        // Parsed Model
        private CauHoiDTO? _parsedQuestion;

        public long MaCauHoiAISinh
        {
            get => _maCauHoiAISinh;
            set { if (_maCauHoiAISinh != value) { _maCauHoiAISinh = value; OnPropertyChanged(); OnPropertyChanged(nameof(MaCauHoiAI)); } }
        }

        public long MaCauHoiAI
        {
            get => MaCauHoiAISinh;
            set => MaCauHoiAISinh = value;
        }

        public long MaYeuCauSinhDe
        {
            get => _maYeuCauSinhDe;
            set { if (_maYeuCauSinhDe != value) { _maYeuCauSinhDe = value; OnPropertyChanged(); OnPropertyChanged(nameof(MaYeuCau)); } }
        }

        public long MaYeuCau
        {
            get => MaYeuCauSinhDe;
            set => MaYeuCauSinhDe = value;
        }

        public string ChuDeYeuCau
        {
            get => NoiDungDauVao;
            set => NoiDungDauVao = value;
        }

        public DateTime ThoiGianTao
        {
            get => NgayTao;
            set => NgayTao = value;
        }

        public string MaCapDo
        {
            get => QuestionDetails.TrinhDo ?? "B1";
            set { QuestionDetails.TrinhDo = value; OnPropertyChanged(); }
        }

        public int? MaCauHoi
        {
            get => _maCauHoi;
            set { if (_maCauHoi != value) { _maCauHoi = value; OnPropertyChanged(); } }
        }

        public string NoiDungCauHoiSinhRa
        {
            get => _noiDungCauHoiSinhRa;
            set
            {
                if (_noiDungCauHoiSinhRa != value)
                {
                    _noiDungCauHoiSinhRa = value;
                    OnPropertyChanged();
                    ParseQuestion();
                }
            }
        }

        public string TrangThaiDuyet
        {
            get => _trangThaiDuyet;
            set
            {
                if (_trangThaiDuyet != value)
                {
                    _trangThaiDuyet = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TrangThaiText));
                    OnPropertyChanged(nameof(TrangThaiBg));
                    OnPropertyChanged(nameof(TrangThaiFg));
                }
            }
        }

        public int? NguoiDuyet
        {
            get => _nguoiDuyet;
            set { if (_nguoiDuyet != value) { _nguoiDuyet = value; OnPropertyChanged(); } }
        }

        public DateTime? NgayDuyet
        {
            get => _ngayDuyet;
            set { if (_ngayDuyet != value) { _ngayDuyet = value; OnPropertyChanged(); } }
        }

        public string NoiDungDauVao
        {
            get => _noiDungDauVao;
            set { if (_noiDungDauVao != value) { _noiDungDauVao = value; OnPropertyChanged(); } }
        }

        public string MoHinhAI
        {
            get => _moHinhAI;
            set { if (_moHinhAI != value) { _moHinhAI = value; OnPropertyChanged(); } }
        }

        public string LoaiDauVao
        {
            get => _loaiDauVao;
            set { if (_loaiDauVao != value) { _loaiDauVao = value; OnPropertyChanged(); OnPropertyChanged(nameof(NguonGocText)); } }
        }

        public DateTime NgayTao
        {
            get => _ngayTao;
            set { if (_ngayTao != value) { _ngayTao = value; OnPropertyChanged(); OnPropertyChanged(nameof(NgayTaoFormatted)); } }
        }

        public string NgayTaoFormatted => NgayTao.ToString("dd/MM HH:mm");

        public string NguonGocText
        {
            get
            {
                if (LoaiDauVao == "TepTin") return "Nhập tệp";
                return string.IsNullOrWhiteSpace(MoHinhAI) ? "AI" : MoHinhAI;
            }
        }

        public string TrangThaiText => TrangThaiDuyet switch
        {
            "DaDuyet" => "Đã duyệt",
            "TuChoi" => "Từ chối",
            "DaChinhSua" => "Đã sửa",
            _ => "Chờ duyệt"
        };

        public string TrangThaiBg => TrangThaiDuyet switch
        {
            "DaDuyet" => "#DCFCE7",
            "TuChoi" => "#FEE2E2",
            "DaChinhSua" => "#E0E7FF",
            _ => "#FEF3C7"
        };

        public string TrangThaiFg => TrangThaiDuyet switch
        {
            "DaDuyet" => "#15803D",
            "TuChoi" => "#B91C1C",
            "DaChinhSua" => "#4338CA",
            _ => "#B45309"
        };

        public CauHoiDTO QuestionDetails
        {
            get
            {
                if (_parsedQuestion == null) ParseQuestion();
                return _parsedQuestion ?? new CauHoiDTO { NoiDungCauHoi = NoiDungCauHoiSinhRa };
            }
            set
            {
                _parsedQuestion = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NoiDungCauHoi));
                OnPropertyChanged(nameof(DoanVanDoc));
                OnPropertyChanged(nameof(KyNang));
                OnPropertyChanged(nameof(DoKho));
                OnPropertyChanged(nameof(LoaiCauHoi));
                OnPropertyChanged(nameof(DapAnDungText));
                OnPropertyChanged(nameof(GiaiThichChiTiet));
                OnPropertyChanged(nameof(DisplayContent));
            }
        }

        public string NoiDungCauHoi
        {
            get => QuestionDetails.NoiDungCauHoi;
            set { QuestionDetails.NoiDungCauHoi = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayContent)); }
        }

        public string? DoanVanDoc
        {
            get => QuestionDetails.DoanVanDoc;
            set { QuestionDetails.DoanVanDoc = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayContent)); }
        }

        public string KyNang
        {
            get => QuestionDetails.KyNang ?? "Doc";
            set { QuestionDetails.KyNang = value; OnPropertyChanged(); }
        }

        public string DoKho
        {
            get => QuestionDetails.DoKho ?? "TrungBinh";
            set { QuestionDetails.DoKho = value; OnPropertyChanged(); }
        }

        public string LoaiCauHoi
        {
            get => QuestionDetails.LoaiCauHoi ?? "TracNghiem";
            set { QuestionDetails.LoaiCauHoi = value; OnPropertyChanged(); }
        }

        public string? GiaiThichChiTiet
        {
            get => QuestionDetails.GiaiThichChiTiet;
            set { QuestionDetails.GiaiThichChiTiet = value; OnPropertyChanged(); OnPropertyChanged(nameof(GiaiThichDapAn)); }
        }

        public string? GiaiThichDapAn
        {
            get => QuestionDetails.GiaiThichChiTiet;
            set { QuestionDetails.GiaiThichChiTiet = value; OnPropertyChanged(); OnPropertyChanged(nameof(GiaiThichChiTiet)); }
        }

        private List<LuaChonDapAnDTO>? _danhSachLuaChon;
        public List<LuaChonDapAnDTO> DanhSachLuaChon
        {
            get
            {
                if (_danhSachLuaChon == null)
                {
                    _danhSachLuaChon = new List<LuaChonDapAnDTO>();
                    var q = QuestionDetails;
                    if (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 0)
                    {
                        for (int i = 0; i < q.DanhSachLuaChon.Count; i++)
                        {
                            char c = (char)('A' + i);
                            _danhSachLuaChon.Add(new LuaChonDapAnDTO
                            {
                                KyHieuLuaChon = c.ToString(),
                                NoiDungLuaChon = CauHoiDTO.StripOptionPrefix(q.DanhSachLuaChon[i]),
                                LaDapAnDung = (q.DapAnDungIndex == i)
                            });
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(q.DapAnVanBan))
                    {
                        _danhSachLuaChon.Add(new LuaChonDapAnDTO
                        {
                            KyHieuLuaChon = "ANS",
                            NoiDungLuaChon = q.DapAnVanBan,
                            LaDapAnDung = true
                        });
                    }
                }
                return _danhSachLuaChon;
            }
            set
            {
                _danhSachLuaChon = value;
                if (value != null)
                {
                    var listStr = new List<string>();
                    int correctIdx = 0;
                    for (int i = 0; i < value.Count; i++)
                    {
                        listStr.Add(value[i].NoiDungLuaChon);
                        if (value[i].LaDapAnDung) correctIdx = i;
                    }
                    QuestionDetails.DanhSachLuaChon = listStr;
                    QuestionDetails.DapAnDungIndex = correctIdx;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayContent));
                OnPropertyChanged(nameof(DapAnDungText));
            }
        }

        public string LoaiHienThi => CauHoiKhaoSatDTO.ChuanHoaLoaiCauHoi(LoaiCauHoi);

        public string DapAnDungText
        {
            get
            {
                if (QuestionDetails.DapAnDungIndex.HasValue && QuestionDetails.DanhSachLuaChon != null && QuestionDetails.DapAnDungIndex.Value >= 0 && QuestionDetails.DapAnDungIndex.Value < QuestionDetails.DanhSachLuaChon.Count)
                {
                    char prefix = (char)('A' + QuestionDetails.DapAnDungIndex.Value);
                    return $"{prefix}. {QuestionDetails.DanhSachLuaChon[QuestionDetails.DapAnDungIndex.Value]}";
                }
                if (!string.IsNullOrWhiteSpace(QuestionDetails.DapAnVanBan)) return QuestionDetails.DapAnVanBan;
                return "Chưa xác định";
            }
        }

        public string DisplayContent
        {
            get
            {
                var q = QuestionDetails;
                string text = "";
                if (!string.IsNullOrWhiteSpace(q.DoanVanDoc))
                {
                    text += $"📖 Đoạn văn:\n{q.DoanVanDoc}\n\n";
                }
                text += q.NoiDungCauHoi;

                if (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 0)
                {
                    text += "\n\n";
                    for (int i = 0; i < q.DanhSachLuaChon.Count; i++)
                    {
                        char prefix = (char)('A' + i);
                        string opt = CauHoiDTO.StripOptionPrefix(q.DanhSachLuaChon[i]);
                        bool isCorrect = (q.DapAnDungIndex == i);
                        text += $"{(isCorrect ? "✓ " : "  ")}{prefix}) {opt}\n";
                    }
                }
                if (!string.IsNullOrWhiteSpace(q.DapAnVanBan))
                {
                    text += $"\n🔑 Đáp án mẫu: {q.DapAnVanBan}";
                }
                if (!string.IsNullOrWhiteSpace(q.GiaiThichChiTiet))
                {
                    text += $"\n💡 Giải thích: {q.GiaiThichChiTiet}";
                }
                return text;
            }
        }

        private void ParseQuestion()
        {
            _danhSachLuaChon = null;
            if (string.IsNullOrWhiteSpace(_noiDungCauHoiSinhRa))
            {
                _parsedQuestion = new CauHoiDTO();
                return;
            }

            try
            {
                if (_noiDungCauHoiSinhRa.TrimStart().StartsWith("{"))
                {
                    _parsedQuestion = JsonSerializer.Deserialize<CauHoiDTO>(_noiDungCauHoiSinhRa, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        AllowTrailingCommas = true
                    });
                }
            }
            catch
            {
                _parsedQuestion = null;
            }

            if (_parsedQuestion == null)
            {
                _parsedQuestion = new CauHoiDTO
                {
                    NoiDungCauHoi = _noiDungCauHoiSinhRa,
                    LoaiCauHoi = "TracNghiem",
                    KyNang = "Doc",
                    DoKho = "TrungBinh"
                };
            }
        }
    }
}
