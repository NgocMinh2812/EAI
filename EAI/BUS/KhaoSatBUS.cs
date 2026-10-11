using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DTO;

namespace BUS
{
    public class KhaoSatBUS
    {
        private static KhaoSatBUS? _instance;
        public static KhaoSatBUS Instance => _instance ??= new KhaoSatBUS();

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiKhaoSat(int maBaiKhaoSat = 1)
        {
            return CauHoiDAL.Instance.LayDanhSachCauHoiKhaoSat(maBaiKhaoSat);
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiTheoTrinhDo(string maCapDo)
        {
            // 1. Thử lấy bài khảo sát chuẩn 15 câu từ CSDL
            var list = CauHoiDAL.Instance.LayDanhSachCauHoiKhaoSat(1);
            if (list == null || list.Count < 10)
            {
                list = CauHoiDAL.Instance.LayDanhSachCauHoiTheoTrinhDo(maCapDo, 15);
            }
            if (list == null || list.Count < 10)
            {
                list = TaoBoCauHoiKhaoSatChuan15Cau(maCapDo);
            }

            int index = 1;
            foreach (var q in list)
            {
                q.ThuTuCauHoi = index++;
            }
            return list;
        }

        public KetQuaKhaoSatDTO DanhGiaThichUng(string trinhDoDaChon, List<CauHoiKhaoSatDTO> danhSachCauHoi, int maNguoiDung)
        {
            int tongSoCau = danhSachCauHoi.Count;
            int soCauDung = 0;

            foreach (var q in danhSachCauHoi)
            {
                var dapAnDung = q.DanhSachLuaChon.FirstOrDefault(l => l.LaDapAnDung);
                if (dapAnDung != null && q.MaLuaChonDaChon.HasValue && q.MaLuaChonDaChon.Value == dapAnDung.MaLuaChon)
                {
                    soCauDung++;
                }
            }

            double tyLeDung = tongSoCau > 0 ? (double)soCauDung / tongSoCau : 0;
            decimal diem10 = Math.Round((decimal)(tyLeDung * 10), 1);

            // Ngưỡng đạt chuẩn là >= 60% (tương đương >= 6.0/10)
            bool ketQuaDat = tyLeDung >= 0.60;
            string trinhDoPhuHop;
            string lyDoDieuChinh;

            if (ketQuaDat)
            {
                if (tyLeDung >= 0.85 && trinhDoDaChon != "C1")
                {
                    trinhDoPhuHop = NangCapTrinhDo(trinhDoDaChon);
                    lyDoDieuChinh = $"Xuất sắc! Bạn trả lời đúng {soCauDung}/{tongSoCau} câu ({diem10}/10 điểm - {tyLeDung * 100:0}%), vượt trội chuẩn năng lực cấp độ {trinhDoDaChon}. Hệ thống AI đề xuất bạn nâng mục tiêu lên trình độ {trinhDoPhuHop} để tối ưu hóa tiềm năng!";
                }
                else
                {
                    trinhDoPhuHop = trinhDoDaChon;
                    lyDoDieuChinh = $"Chúc mừng bạn! Bạn trả lời đúng {soCauDung}/{tongSoCau} câu ({diem10}/10 điểm - {tyLeDung * 100:0}%), đạt yêu cầu năng lực chuẩn của trình độ {trinhDoDaChon}.";
                }
            }
            else
            {
                trinhDoPhuHop = HaCapTrinhDo(trinhDoDaChon);
                lyDoDieuChinh = $"Bạn đạt {soCauDung}/{tongSoCau} câu ({diem10}/10 điểm - {tyLeDung * 100:0}%), chưa đạt ngưỡng yêu cầu 60% của cấp độ {trinhDoDaChon}. Hệ thống AI đề xuất bạn học tập ở trình độ {trinhDoPhuHop} để củng cố nền tảng trước khi nâng cao.";
            }

            var thongTin = LayThongTinTrinhDo(trinhDoPhuHop);

            var ketQua = new KetQuaKhaoSatDTO
            {
                TongSoCau = tongSoCau,
                SoCauDung = soCauDung,
                DiemSo = diem10,
                TrinhDoDaChon = trinhDoDaChon,
                KetQuaDat = ketQuaDat,
                TrinhDoPhuHop = trinhDoPhuHop,
                TenTrinhDoPhuHop = thongTin.Ten,
                UocTinhIelts = thongTin.Ielts,
                UocTinhToeic = thongTin.Toeic,
                NhanXetLộTrinh = thongTin.NhanXet,
                LyDoDieuChinh = lyDoDieuChinh
            };

            // Đề xuất bài học theo trình độ phù hợp
            try
            {
                ketQua.DanhSachBaiHocDeXuat = BaiHocDAL.Instance.LayDanhSachBaiHocTheoTrinhDo(trinhDoPhuHop, maNguoiDung, 3);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi lấy bài học đề xuất: {ex.Message}");
                ketQua.DanhSachBaiHocDeXuat = new List<BaiHocDTO>();
            }

            // Lưu vào CSDL
            try
            {
                if (maNguoiDung > 0)
                {
                    int maLuot = CauHoiDAL.Instance.LuuLuotKhaoSat(maNguoiDung, 1, tongSoCau, soCauDung, diem10, trinhDoPhuHop);

                    foreach (var q in danhSachCauHoi)
                    {
                        var dapAnDung = q.DanhSachLuaChon.FirstOrDefault(l => l.LaDapAnDung);
                        bool laDung = dapAnDung != null && q.MaLuaChonDaChon.HasValue && q.MaLuaChonDaChon.Value == dapAnDung.MaLuaChon;
                        CauHoiDAL.Instance.LuuChiTietCauTraLoi(maLuot, q.MaCauHoi, q.MaLuaChonDaChon, laDung);
                    }

                    // Cập nhật Hồ sơ học tập (Lưu điểm đánh giá năng lực, trình độ và ghi chú vào hồ sơ cá nhân)
                    string ghiChu = $"Đánh giá năng lực: {diem10}/10 ({soCauDung}/{tongSoCau} câu đúng) • Xếp loại {trinhDoPhuHop} ({thongTin.Ten}) • {thongTin.Ielts} • {thongTin.Toeic}";
                    HoSoHocTapDAL.Instance.LuuKetQuaDanhGiaBanDau(maNguoiDung, trinhDoPhuHop, "CEFR", thongTin.TargetIelts, diem10, ghiChu);

                    // Cập nhật cờ lần đầu đăng nhập
                    NguoiDungDAL.Instance.CapNhatTrangThaiLanDauDangNhap(maNguoiDung, false);

                    // Cập nhật Session
                    if (SessionHelper.CurrentUser != null)
                    {
                        SessionHelper.CurrentUser.LaLanDauDangNhap = false;
                    }
                    SessionHelper.CurrentHoSo = HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(maNguoiDung);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu kết quả khảo sát: {ex.Message}");
            }

            return ketQua;
        }

        public KetQuaKhaoSatDTO ChamDiemVaXepLoai(List<CauHoiKhaoSatDTO> danhSachCauHoi, int maNguoiDung)
        {
            return DanhGiaThichUng("B1", danhSachCauHoi, maNguoiDung);
        }

        public KetQuaKhaoSatDTO? LayKetQuaKhaoSatGanNhat(int maNguoiDung)
        {
            var ketQua = CauHoiDAL.Instance.LayKetQuaKhaoSatGanNhat(maNguoiDung);
            if (ketQua != null)
            {
                var thongTin = LayThongTinTrinhDo(ketQua.TrinhDoPhuHop);
                ketQua.TenTrinhDoPhuHop = thongTin.Ten;
                ketQua.UocTinhIelts = thongTin.Ielts;
                ketQua.UocTinhToeic = thongTin.Toeic;
                ketQua.NhanXetLộTrinh = thongTin.NhanXet;
                ketQua.LyDoDieuChinh = $"Bạn đã hoàn thành khảo sát đánh giá với kết quả {ketQua.SoCauDung}/{ketQua.TongSoCau} câu đúng ({ketQua.DiemSo}/10 điểm), xác lập trình độ chuẩn {ketQua.TrinhDoPhuHop}.";
                ketQua.DanhSachBaiHocDeXuat = BaiHocDAL.Instance.LayDanhSachBaiHocTheoTrinhDo(ketQua.TrinhDoPhuHop, maNguoiDung, 4);
            }
            return ketQua;
        }

        public static string HaCapTrinhDo(string capDo)
        {
            return capDo.ToUpper() switch
            {
                "C1" => "B2",
                "B2" => "B1",
                "B1" => "A2",
                "A2" => "A1",
                "A1" => "A1",
                _ => "A1"
            };
        }

        public static string NangCapTrinhDo(string capDo)
        {
            return capDo.ToUpper() switch
            {
                "A1" => "A2",
                "A2" => "B1",
                "B1" => "B2",
                "B2" => "C1",
                "C1" => "C1",
                _ => "B1"
            };
        }

        private static (string Ten, string Ielts, string Toeic, string NhanXet, decimal TargetIelts) LayThongTinTrinhDo(string capDo)
        {
            return capDo.ToUpper() switch
            {
                "C1" => ("Advanced (Nâng cao)", "IELTS 7.5+ • CEFR C1", "TOEIC 900+", "Bạn có vốn ngôn ngữ và tư duy học thuật vượt trội. Lộ trình tập trung vào diễn đạt chuyên sâu, viết luận học thuật và phản biện.", 8.0m),
                "B2" => ("Upper-Intermediate (Trên trung cấp)", "IELTS 6.0 - 7.0 • CEFR B2", "TOEIC 700 - 850", "Nền tảng của bạn rất vững! Lộ trình hướng đến bứt phá các bài đọc hiểu chuyên sâu, câu điều kiện, cấu trúc đảo ngữ.", 6.5m),
                "B1" => ("Intermediate (Trung cấp)", "IELTS 5.0 - 5.5 • CEFR B1", "TOEIC 550 - 650", "Bạn đã nắm chắc ngữ pháp cơ bản. Lộ trình sẽ giúp bạn củng cố các thì nâng cao, mở rộng vốn từ vựng học thuật và tự tin giao tiếp.", 5.5m),
                "A2" => ("Elementary (Sơ cấp)", "IELTS 4.0 - 4.5 • CEFR A2", "TOEIC 350 - 450", "Bạn có thể giao tiếp những câu đơn giản. Lộ trình tập trung mở rộng vốn từ vựng đời sống, thì hiện tại hoàn thành và quá khứ đơn.", 4.5m),
                _ => ("Beginner (Cơ bản / Mất gốc)", "IELTS 3.0 • CEFR A1", "TOEIC 200 - 300", "Đừng lo lắng! Lộ trình được thiết kế từ phát âm chuẩn, từ vựng nền tảng và các cấu trúc cơ bản nhất để xây gốc tiếng Anh vững chắc.", 3.5m)
            };
        }

        public static List<CauHoiKhaoSatDTO> TaoBoCauHoiDuPhongTheoCapDo(string capDo)
        {
            var list = new List<CauHoiKhaoSatDTO>();

            switch (capDo.ToUpper())
            {
                case "A1":
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 101, KyNang = "NguPhap", MaTrinhDo = "A1",
                        NoiDungCauHoi = "They ______ from England. They are visiting Vietnam for holiday.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 1, KyHieuLuaChon = "A", NoiDungLuaChon = "is", LaDapAnDung = false },
                            new() { MaLuaChon = 2, KyHieuLuaChon = "B", NoiDungLuaChon = "are", LaDapAnDung = true },
                            new() { MaLuaChon = 3, KyHieuLuaChon = "C", NoiDungLuaChon = "am", LaDapAnDung = false },
                            new() { MaLuaChon = 4, KyHieuLuaChon = "D", NoiDungLuaChon = "be", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 102, KyNang = "TuVung", MaTrinhDo = "A1",
                        NoiDungCauHoi = "What time do you usually ______ breakfast in the morning?",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 5, KyHieuLuaChon = "A", NoiDungLuaChon = "have", LaDapAnDung = true },
                            new() { MaLuaChon = 6, KyHieuLuaChon = "B", NoiDungLuaChon = "do", LaDapAnDung = false },
                            new() { MaLuaChon = 7, KyHieuLuaChon = "C", NoiDungLuaChon = "make", LaDapAnDung = false },
                            new() { MaLuaChon = 8, KyHieuLuaChon = "D", NoiDungLuaChon = "go", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 103, KyNang = "NguPhap", MaTrinhDo = "A1",
                        NoiDungCauHoi = "There ______ three apples on the kitchen table.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 9, KyHieuLuaChon = "A", NoiDungLuaChon = "is", LaDapAnDung = false },
                            new() { MaLuaChon = 10, KyHieuLuaChon = "B", NoiDungLuaChon = "are", LaDapAnDung = true },
                            new() { MaLuaChon = 11, KyHieuLuaChon = "C", NoiDungLuaChon = "has", LaDapAnDung = false },
                            new() { MaLuaChon = 12, KyHieuLuaChon = "D", NoiDungLuaChon = "have", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 104, KyNang = "TuVung", MaTrinhDo = "A1",
                        NoiDungCauHoi = "My sister is a ______. She takes care of sick people in the hospital.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 13, KyHieuLuaChon = "A", NoiDungLuaChon = "nurse", LaDapAnDung = true },
                            new() { MaLuaChon = 14, KyHieuLuaChon = "B", NoiDungLuaChon = "pilot", LaDapAnDung = false },
                            new() { MaLuaChon = 15, KyHieuLuaChon = "C", NoiDungLuaChon = "chef", LaDapAnDung = false },
                            new() { MaLuaChon = 16, KyHieuLuaChon = "D", NoiDungLuaChon = "driver", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 105, KyNang = "NguPhap", MaTrinhDo = "A1",
                        NoiDungCauHoi = "I ______ like drinking cold coffee in winter.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 17, KyHieuLuaChon = "A", NoiDungLuaChon = "doesn't", LaDapAnDung = false },
                            new() { MaLuaChon = 18, KyHieuLuaChon = "B", NoiDungLuaChon = "don't", LaDapAnDung = true },
                            new() { MaLuaChon = 19, KyHieuLuaChon = "C", NoiDungLuaChon = "not", LaDapAnDung = false },
                            new() { MaLuaChon = 20, KyHieuLuaChon = "D", NoiDungLuaChon = "isn't", LaDapAnDung = false }
                        }
                    });
                    break;

                case "A2":
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 201, KyNang = "NguPhap", MaTrinhDo = "A2",
                        NoiDungCauHoi = "Yesterday, she ______ to the supermarket to buy some fresh vegetables.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 21, KyHieuLuaChon = "A", NoiDungLuaChon = "go", LaDapAnDung = false },
                            new() { MaLuaChon = 22, KyHieuLuaChon = "B", NoiDungLuaChon = "goes", LaDapAnDung = false },
                            new() { MaLuaChon = 23, KyHieuLuaChon = "C", NoiDungLuaChon = "went", LaDapAnDung = true },
                            new() { MaLuaChon = 24, KyHieuLuaChon = "D", NoiDungLuaChon = "gone", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 202, KyNang = "TuVung", MaTrinhDo = "A2",
                        NoiDungCauHoi = "The doctor advised him to take this ______ twice a day after meals.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 25, KyHieuLuaChon = "A", NoiDungLuaChon = "medicine", LaDapAnDung = true },
                            new() { MaLuaChon = 26, KyHieuLuaChon = "B", NoiDungLuaChon = "recipe", LaDapAnDung = false },
                            new() { MaLuaChon = 27, KyHieuLuaChon = "C", NoiDungLuaChon = "ingredient", LaDapAnDung = false },
                            new() { MaLuaChon = 28, KyHieuLuaChon = "D", NoiDungLuaChon = "equipment", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 203, KyNang = "NguPhap", MaTrinhDo = "A2",
                        NoiDungCauHoi = "This smartphone is ______ than the one I bought last year.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 29, KyHieuLuaChon = "A", NoiDungLuaChon = "more cheap", LaDapAnDung = false },
                            new() { MaLuaChon = 30, KyHieuLuaChon = "B", NoiDungLuaChon = "cheaper", LaDapAnDung = true },
                            new() { MaLuaChon = 31, KyHieuLuaChon = "C", NoiDungLuaChon = "cheapest", LaDapAnDung = false },
                            new() { MaLuaChon = 32, KyHieuLuaChon = "D", NoiDungLuaChon = "as cheap", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 204, KyNang = "TuVung", MaTrinhDo = "A2",
                        NoiDungCauHoi = "Don't forget to turn off the lights before you ______ the room.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 33, KyHieuLuaChon = "A", NoiDungLuaChon = "arrive", LaDapAnDung = false },
                            new() { MaLuaChon = 34, KyHieuLuaChon = "B", NoiDungLuaChon = "leave", LaDapAnDung = true },
                            new() { MaLuaChon = 35, KyHieuLuaChon = "C", NoiDungLuaChon = "stay", LaDapAnDung = false },
                            new() { MaLuaChon = 36, KyHieuLuaChon = "D", NoiDungLuaChon = "enter", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 205, KyNang = "NguPhap", MaTrinhDo = "A2",
                        NoiDungCauHoi = "While I was studying in my bedroom, my brother ______ computer games.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 37, KyHieuLuaChon = "A", NoiDungLuaChon = "played", LaDapAnDung = false },
                            new() { MaLuaChon = 38, KyHieuLuaChon = "B", NoiDungLuaChon = "was playing", LaDapAnDung = true },
                            new() { MaLuaChon = 39, KyHieuLuaChon = "C", NoiDungLuaChon = "is playing", LaDapAnDung = false },
                            new() { MaLuaChon = 40, KyHieuLuaChon = "D", NoiDungLuaChon = "plays", LaDapAnDung = false }
                        }
                    });
                    break;

                case "B1":
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 301, KyNang = "NguPhap", MaTrinhDo = "B1",
                        NoiDungCauHoi = "Mr. David ______ in this international school since 2018.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 41, KyHieuLuaChon = "A", NoiDungLuaChon = "worked", LaDapAnDung = false },
                            new() { MaLuaChon = 42, KyHieuLuaChon = "B", NoiDungLuaChon = "has worked", LaDapAnDung = true },
                            new() { MaLuaChon = 43, KyHieuLuaChon = "C", NoiDungLuaChon = "is working", LaDapAnDung = false },
                            new() { MaLuaChon = 44, KyHieuLuaChon = "D", NoiDungLuaChon = "works", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 302, KyNang = "NguPhap", MaTrinhDo = "B1",
                        NoiDungCauHoi = "She has always been capable ______ solving complicated mathematical equations.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 45, KyHieuLuaChon = "A", NoiDungLuaChon = "with", LaDapAnDung = false },
                            new() { MaLuaChon = 46, KyHieuLuaChon = "B", NoiDungLuaChon = "in", LaDapAnDung = false },
                            new() { MaLuaChon = 47, KyHieuLuaChon = "C", NoiDungLuaChon = "of", LaDapAnDung = true },
                            new() { MaLuaChon = 48, KyHieuLuaChon = "D", NoiDungLuaChon = "for", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 303, KyNang = "TuVung", MaTrinhDo = "B1",
                        NoiDungCauHoi = "All candidates must submit their application before the strict ______ on Friday.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 49, KyHieuLuaChon = "A", NoiDungLuaChon = "deadline", LaDapAnDung = true },
                            new() { MaLuaChon = 50, KyHieuLuaChon = "B", NoiDungLuaChon = "destination", LaDapAnDung = false },
                            new() { MaLuaChon = 51, KyHieuLuaChon = "C", NoiDungLuaChon = "dimension", LaDapAnDung = false },
                            new() { MaLuaChon = 52, KyHieuLuaChon = "D", NoiDungLuaChon = "description", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 304, KyNang = "NguPhap", MaTrinhDo = "B1",
                        NoiDungCauHoi = "The scientist ______ discoveries revolutionized modern medicine was awarded the prize.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 53, KyHieuLuaChon = "A", NoiDungLuaChon = "who", LaDapAnDung = false },
                            new() { MaLuaChon = 54, KyHieuLuaChon = "B", NoiDungLuaChon = "whose", LaDapAnDung = true },
                            new() { MaLuaChon = 55, KyHieuLuaChon = "C", NoiDungLuaChon = "whom", LaDapAnDung = false },
                            new() { MaLuaChon = 56, KyHieuLuaChon = "D", NoiDungLuaChon = "which", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 305, KyNang = "TuVung", MaTrinhDo = "B1",
                        NoiDungCauHoi = "Online learning allows students to study at their own ______ without excessive pressure.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 57, KyHieuLuaChon = "A", NoiDungLuaChon = "pace", LaDapAnDung = true },
                            new() { MaLuaChon = 58, KyHieuLuaChon = "B", NoiDungLuaChon = "step", LaDapAnDung = false },
                            new() { MaLuaChon = 59, KyHieuLuaChon = "C", NoiDungLuaChon = "rate", LaDapAnDung = false },
                            new() { MaLuaChon = 60, KyHieuLuaChon = "D", NoiDungLuaChon = "foot", LaDapAnDung = false }
                        }
                    });
                    break;

                case "B2":
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 401, KyNang = "NguPhap", MaTrinhDo = "B2",
                        NoiDungCauHoi = "If I ______ more time yesterday, I would have traveled around the ancient city with you.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 61, KyHieuLuaChon = "A", NoiDungLuaChon = "have", LaDapAnDung = false },
                            new() { MaLuaChon = 62, KyHieuLuaChon = "B", NoiDungLuaChon = "had had", LaDapAnDung = true },
                            new() { MaLuaChon = 63, KyHieuLuaChon = "C", NoiDungLuaChon = "had", LaDapAnDung = false },
                            new() { MaLuaChon = 64, KyHieuLuaChon = "D", NoiDungLuaChon = "would have", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 402, KyNang = "TuVung", MaTrinhDo = "B2",
                        NoiDungCauHoi = "The university decided to ______ innovative teaching methods to stimulate students' critical thinking.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 65, KyHieuLuaChon = "A", NoiDungLuaChon = "adopt", LaDapAnDung = true },
                            new() { MaLuaChon = 66, KyHieuLuaChon = "B", NoiDungLuaChon = "adapt", LaDapAnDung = false },
                            new() { MaLuaChon = 67, KyHieuLuaChon = "C", NoiDungLuaChon = "adept", LaDapAnDung = false },
                            new() { MaLuaChon = 68, KyHieuLuaChon = "D", NoiDungLuaChon = "admit", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 403, KyNang = "NguPhap", MaTrinhDo = "B2",
                        NoiDungCauHoi = "Hardly ______ home when the torrential rain began to pour.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 69, KyHieuLuaChon = "A", NoiDungLuaChon = "she arrived", LaDapAnDung = false },
                            new() { MaLuaChon = 70, KyHieuLuaChon = "B", NoiDungLuaChon = "had she arrived", LaDapAnDung = true },
                            new() { MaLuaChon = 71, KyHieuLuaChon = "C", NoiDungLuaChon = "she had arrived", LaDapAnDung = false },
                            new() { MaLuaChon = 72, KyHieuLuaChon = "D", NoiDungLuaChon = "did she arrive", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 404, KyNang = "TuVung", MaTrinhDo = "B2",
                        NoiDungCauHoi = "The sudden ______ in consumer demand led to widespread shortages across major markets.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 73, KyHieuLuaChon = "A", NoiDungLuaChon = "surge", LaDapAnDung = true },
                            new() { MaLuaChon = 74, KyHieuLuaChon = "B", NoiDungLuaChon = "drought", LaDapAnDung = false },
                            new() { MaLuaChon = 75, KyHieuLuaChon = "C", NoiDungLuaChon = "decline", LaDapAnDung = false },
                            new() { MaLuaChon = 76, KyHieuLuaChon = "D", NoiDungLuaChon = "tide", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 405, KyNang = "NguPhap", MaTrinhDo = "B2",
                        NoiDungCauHoi = "It is essential that every student ______ present at the auditorium before 8:00 AM.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 77, KyHieuLuaChon = "A", NoiDungLuaChon = "is", LaDapAnDung = false },
                            new() { MaLuaChon = 78, KyHieuLuaChon = "B", NoiDungLuaChon = "be", LaDapAnDung = true },
                            new() { MaLuaChon = 79, KyHieuLuaChon = "C", NoiDungLuaChon = "was", LaDapAnDung = false },
                            new() { MaLuaChon = 80, KyHieuLuaChon = "D", NoiDungLuaChon = "being", LaDapAnDung = false }
                        }
                    });
                    break;

                case "C1":
                default:
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 501, KyNang = "NguPhap", MaTrinhDo = "C1",
                        NoiDungCauHoi = "Not until the final research findings were published ______ to understand the true impact of the phenomenon.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 81, KyHieuLuaChon = "A", NoiDungLuaChon = "did scientists begin", LaDapAnDung = true },
                            new() { MaLuaChon = 82, KyHieuLuaChon = "B", NoiDungLuaChon = "scientists began", LaDapAnDung = false },
                            new() { MaLuaChon = 83, KyHieuLuaChon = "C", NoiDungLuaChon = "had scientists begun", LaDapAnDung = false },
                            new() { MaLuaChon = 84, KyHieuLuaChon = "D", NoiDungLuaChon = "scientists had begun", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 502, KyNang = "TuVung", MaTrinhDo = "C1",
                        NoiDungCauHoi = "His persistent efforts in advocating for sustainable policies ultimately bore ______.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 85, KyHieuLuaChon = "A", NoiDungLuaChon = "fruit", LaDapAnDung = true },
                            new() { MaLuaChon = 86, KyHieuLuaChon = "B", NoiDungLuaChon = "seed", LaDapAnDung = false },
                            new() { MaLuaChon = 87, KyHieuLuaChon = "C", NoiDungLuaChon = "tree", LaDapAnDung = false },
                            new() { MaLuaChon = 88, KyHieuLuaChon = "D", NoiDungLuaChon = "flower", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 503, KyNang = "TuVung", MaTrinhDo = "C1",
                        NoiDungCauHoi = "The economic sanctions had an ______ effect on international trade relationships.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 89, KyHieuLuaChon = "A", NoiDungLuaChon = "adverse", LaDapAnDung = true },
                            new() { MaLuaChon = 90, KyHieuLuaChon = "B", NoiDungLuaChon = "averse", LaDapAnDung = false },
                            new() { MaLuaChon = 91, KyHieuLuaChon = "C", NoiDungLuaChon = "reverse", LaDapAnDung = false },
                            new() { MaLuaChon = 92, KyHieuLuaChon = "D", NoiDungLuaChon = "inverse", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 504, KyNang = "NguPhap", MaTrinhDo = "C1",
                        NoiDungCauHoi = "Were the executive board ______ to approve the budget, the merger would proceed immediately.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 93, KyHieuLuaChon = "A", NoiDungLuaChon = "to decide", LaDapAnDung = true },
                            new() { MaLuaChon = 94, KyHieuLuaChon = "B", NoiDungLuaChon = "decided", LaDapAnDung = false },
                            new() { MaLuaChon = 95, KyHieuLuaChon = "C", NoiDungLuaChon = "deciding", LaDapAnDung = false },
                            new() { MaLuaChon = 96, KyHieuLuaChon = "D", NoiDungLuaChon = "has decided", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 505, KyNang = "TuVung", MaTrinhDo = "C1",
                        NoiDungCauHoi = "The company's groundbreaking technological innovation is ______ with excellence in the modern industry.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 97, KyHieuLuaChon = "A", NoiDungLuaChon = "synonymous", LaDapAnDung = true },
                            new() { MaLuaChon = 98, KyHieuLuaChon = "B", NoiDungLuaChon = "anonymous", LaDapAnDung = false },
                            new() { MaLuaChon = 99, KyHieuLuaChon = "C", NoiDungLuaChon = "harmonious", LaDapAnDung = false },
                            new() { MaLuaChon = 100, KyHieuLuaChon = "D", NoiDungLuaChon = "spontaneous", LaDapAnDung = false }
                        }
                    });
                    break;
            }

            return list;
        }

        public static List<CauHoiKhaoSatDTO> TaoBoCauHoiKhaoSatChuan15Cau(string capDoGoiY = "B1")
        {
            var list = new List<CauHoiKhaoSatDTO>();

            // Câu 1: Ngữ pháp A1 - To be
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 1, KyNang = "NguPhap", MaTrinhDo = "A1",
                NoiDungCauHoi = "They ______ from England. They are visiting Vietnam for their summer holiday.",
                GiaiThichDapAn = "Chủ ngữ 'They' là số nhiều đi với động từ to be 'are'.",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 1, KyHieuLuaChon = "A", NoiDungLuaChon = "is", LaDapAnDung = false },
                    new() { MaLuaChon = 2, KyHieuLuaChon = "B", NoiDungLuaChon = "are", LaDapAnDung = true },
                    new() { MaLuaChon = 3, KyHieuLuaChon = "C", NoiDungLuaChon = "am", LaDapAnDung = false },
                    new() { MaLuaChon = 4, KyHieuLuaChon = "D", NoiDungLuaChon = "be", LaDapAnDung = false }
                }
            });

            // Câu 2: Từ vựng A1 - Thói quen hàng ngày
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 2, KyNang = "TuVung", MaTrinhDo = "A1",
                NoiDungCauHoi = "What time do you usually ______ breakfast in the morning?",
                GiaiThichDapAn = "Cụm cố định: 'have breakfast' (ăn sáng).",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 5, KyHieuLuaChon = "A", NoiDungLuaChon = "have", LaDapAnDung = true },
                    new() { MaLuaChon = 6, KyHieuLuaChon = "B", NoiDungLuaChon = "do", LaDapAnDung = false },
                    new() { MaLuaChon = 7, KyHieuLuaChon = "C", NoiDungLuaChon = "make", LaDapAnDung = false },
                    new() { MaLuaChon = 8, KyHieuLuaChon = "D", NoiDungLuaChon = "go", LaDapAnDung = false }
                }
            });

            // Câu 3: Ngữ pháp A2 - Quá khứ đơn
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 3, KyNang = "NguPhap", MaTrinhDo = "A2",
                NoiDungCauHoi = "Yesterday, she ______ to the supermarket to buy some fresh vegetables.",
                GiaiThichDapAn = "'Yesterday' là dấu hiệu quá khứ đơn -> 'went'.",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 9, KyHieuLuaChon = "A", NoiDungLuaChon = "go", LaDapAnDung = false },
                    new() { MaLuaChon = 10, KyHieuLuaChon = "B", NoiDungLuaChon = "goes", LaDapAnDung = false },
                    new() { MaLuaChon = 11, KyHieuLuaChon = "C", NoiDungLuaChon = "went", LaDapAnDung = true },
                    new() { MaLuaChon = 12, KyHieuLuaChon = "D", NoiDungLuaChon = "gone", LaDapAnDung = false }
                }
            });

            // Câu 4: Từ vựng A2 - Thuốc men / Đời sống
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 4, KyNang = "TuVung", MaTrinhDo = "A2",
                NoiDungCauHoi = "The doctor advised him to take this ______ twice a day after meals.",
                GiaiThichDapAn = "'medicine' nghĩa là thuốc uống theo toa bác sĩ.",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 13, KyHieuLuaChon = "A", NoiDungLuaChon = "medicine", LaDapAnDung = true },
                    new() { MaLuaChon = 14, KyHieuLuaChon = "B", NoiDungLuaChon = "recipe", LaDapAnDung = false },
                    new() { MaLuaChon = 15, KyHieuLuaChon = "C", NoiDungLuaChon = "ingredient", LaDapAnDung = false },
                    new() { MaLuaChon = 16, KyHieuLuaChon = "D", NoiDungLuaChon = "equipment", LaDapAnDung = false }
                }
            });

            // Câu 5: Ngữ pháp A2 - So sánh hơn
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 5, KyNang = "NguPhap", MaTrinhDo = "A2",
                NoiDungCauHoi = "This smartphone is ______ than the one I bought last year.",
                GiaiThichDapAn = "Tính từ ngắn 'cheap' so sánh hơn thêm '-er' -> 'cheaper'.",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 17, KyHieuLuaChon = "A", NoiDungLuaChon = "more cheap", LaDapAnDung = false },
                    new() { MaLuaChon = 18, KyHieuLuaChon = "B", NoiDungLuaChon = "cheaper", LaDapAnDung = true },
                    new() { MaLuaChon = 19, KyHieuLuaChon = "C", NoiDungLuaChon = "cheapest", LaDapAnDung = false },
                    new() { MaLuaChon = 20, KyHieuLuaChon = "D", NoiDungLuaChon = "as cheap", LaDapAnDung = false }
                }
            });

            // Câu 6: Ngữ pháp B1 - Hiện tại hoàn thành
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 6, KyNang = "NguPhap", MaTrinhDo = "B1",
                NoiDungCauHoi = "Mr. David ______ in this international school since 2018.",
                GiaiThichDapAn = "'since 2018' diễn tả hành động kéo dài đến hiện tại -> 'has worked'.",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 21, KyHieuLuaChon = "A", NoiDungLuaChon = "worked", LaDapAnDung = false },
                    new() { MaLuaChon = 22, KyHieuLuaChon = "B", NoiDungLuaChon = "has worked", LaDapAnDung = true },
                    new() { MaLuaChon = 23, KyHieuLuaChon = "C", NoiDungLuaChon = "is working", LaDapAnDung = false },
                    new() { MaLuaChon = 24, KyHieuLuaChon = "D", NoiDungLuaChon = "works", LaDapAnDung = false }
                }
            });

            // Câu 7: Từ vựng B1 - Hạn chót công sở
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 7, KyNang = "TuVung", MaTrinhDo = "B1",
                NoiDungCauHoi = "All candidates must submit their application before the strict ______ on Friday.",
                GiaiThichDapAn = "'deadline' nghĩa là hạn chót nộp hồ sơ.",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 25, KyHieuLuaChon = "A", NoiDungLuaChon = "deadline", LaDapAnDung = true },
                    new() { MaLuaChon = 26, KyHieuLuaChon = "B", NoiDungLuaChon = "destination", LaDapAnDung = false },
                    new() { MaLuaChon = 27, KyHieuLuaChon = "C", NoiDungLuaChon = "dimension", LaDapAnDung = false },
                    new() { MaLuaChon = 28, KyHieuLuaChon = "D", NoiDungLuaChon = "description", LaDapAnDung = false }
                }
            });

            // Câu 8: Ngữ pháp B1 - Giới từ đi kèm
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 8, KyNang = "NguPhap", MaTrinhDo = "B1",
                NoiDungCauHoi = "She has always been capable ______ solving complicated mathematical equations.",
                GiaiThichDapAn = "Cấu trúc cố định: 'capable of + V-ing' (có khả năng làm gì).",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 29, KyHieuLuaChon = "A", NoiDungLuaChon = "with", LaDapAnDung = false },
                    new() { MaLuaChon = 30, KyHieuLuaChon = "B", NoiDungLuaChon = "in", LaDapAnDung = false },
                    new() { MaLuaChon = 31, KyHieuLuaChon = "C", NoiDungLuaChon = "of", LaDapAnDung = true },
                    new() { MaLuaChon = 32, KyHieuLuaChon = "D", NoiDungLuaChon = "for", LaDapAnDung = false }
                }
            });

            // Câu 9: Ngữ pháp B1 - Mệnh đề quan hệ
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 9, KyNang = "NguPhap", MaTrinhDo = "B1",
                NoiDungCauHoi = "The scientist ______ discoveries revolutionized modern medicine was awarded the prize.",
                GiaiThichDapAn = "Đại từ sở hữu 'whose discoveries' (người có những phát minh).",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 33, KyHieuLuaChon = "A", NoiDungLuaChon = "who", LaDapAnDung = false },
                    new() { MaLuaChon = 34, KyHieuLuaChon = "B", NoiDungLuaChon = "whose", LaDapAnDung = true },
                    new() { MaLuaChon = 35, KyHieuLuaChon = "C", NoiDungLuaChon = "whom", LaDapAnDung = false },
                    new() { MaLuaChon = 36, KyHieuLuaChon = "D", NoiDungLuaChon = "which", LaDapAnDung = false }
                }
            });

            // Câu 10: Từ vựng B2 - Phương pháp giảng dạy
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 10, KyNang = "TuVung", MaTrinhDo = "B2",
                NoiDungCauHoi = "The university decided to ______ innovative teaching methods to stimulate students' critical thinking.",
                GiaiThichDapAn = "'adopt' nghĩa là áp dụng phương pháp mới, khác với 'adapt' (thích nghi).",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 37, KyHieuLuaChon = "A", NoiDungLuaChon = "adopt", LaDapAnDung = true },
                    new() { MaLuaChon = 38, KyHieuLuaChon = "B", NoiDungLuaChon = "adapt", LaDapAnDung = false },
                    new() { MaLuaChon = 39, KyHieuLuaChon = "C", NoiDungLuaChon = "adept", LaDapAnDung = false },
                    new() { MaLuaChon = 40, KyHieuLuaChon = "D", NoiDungLuaChon = "admit", LaDapAnDung = false }
                }
            });

            // Câu 11: Ngữ pháp B2 - Câu điều kiện loại 3
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 11, KyNang = "NguPhap", MaTrinhDo = "B2",
                NoiDungCauHoi = "If I ______ more time yesterday, I would have traveled around the ancient city with you.",
                GiaiThichDapAn = "Điều kiện loại 3: If + S + had + P2 -> 'had had'.",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 41, KyHieuLuaChon = "A", NoiDungLuaChon = "have", LaDapAnDung = false },
                    new() { MaLuaChon = 42, KyHieuLuaChon = "B", NoiDungLuaChon = "had had", LaDapAnDung = true },
                    new() { MaLuaChon = 43, KyHieuLuaChon = "C", NoiDungLuaChon = "had", LaDapAnDung = false },
                    new() { MaLuaChon = 44, KyHieuLuaChon = "D", NoiDungLuaChon = "would have", LaDapAnDung = false }
                }
            });

            // Câu 12: Ngữ pháp B2 - Cấu trúc đảo ngữ
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 12, KyNang = "NguPhap", MaTrinhDo = "B2",
                NoiDungCauHoi = "Hardly ______ home when the torrential rain began to pour.",
                GiaiThichDapAn = "Cấu trúc đảo ngữ: 'Hardly had + S + P2 + when + clause'.",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 45, KyHieuLuaChon = "A", NoiDungLuaChon = "she arrived", LaDapAnDung = false },
                    new() { MaLuaChon = 46, KyHieuLuaChon = "B", NoiDungLuaChon = "had she arrived", LaDapAnDung = true },
                    new() { MaLuaChon = 47, KyHieuLuaChon = "C", NoiDungLuaChon = "she had arrived", LaDapAnDung = false },
                    new() { MaLuaChon = 48, KyHieuLuaChon = "D", NoiDungLuaChon = "did she arrive", LaDapAnDung = false }
                }
            });

            // Đoạn văn đọc hiểu dùng chung cho câu 13, 14, 15
            string passage = "Artificial Intelligence (AI) is rapidly transforming modern language education by providing personalized pathways tailored to each student's pace. Unlike traditional one-size-fits-all classrooms, adaptive AI models continuously analyze user responses, identify lexical and grammatical bottlenecks, and immediately adjust exercise difficulty. Furthermore, generative AI conversational agents simulate immersive dialogue environments, allowing learners to build speaking and writing confidence without fear of judgment. However, educational experts emphasize that AI should complement rather than fully replace human instructors, who remain essential for fostering critical thinking and intercultural empathy.";

            // Câu 13: Đọc hiểu - Ý chính
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 13, KyNang = "Doc", MaTrinhDo = "B2",
                NoiDungCauHoi = "What is the primary advantage of AI in language learning discussed in the passage?",
                DoanVanDoc = passage,
                GiaiThichDapAn = "Đoạn văn nêu rõ AI cung cấp lộ trình học cá nhân hóa thích ứng theo tốc độ của học viên.",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 49, KyHieuLuaChon = "A", NoiDungLuaChon = "It guarantees immediate native fluency within two weeks.", LaDapAnDung = false },
                    new() { MaLuaChon = 50, KyHieuLuaChon = "B", NoiDungLuaChon = "It provides personalized pathways tailored to each student's pace.", LaDapAnDung = true },
                    new() { MaLuaChon = 51, KyHieuLuaChon = "C", NoiDungLuaChon = "It makes human instructors completely obsolete.", LaDapAnDung = false },
                    new() { MaLuaChon = 52, KyHieuLuaChon = "D", NoiDungLuaChon = "It standardizes all test materials into a single rigid format.", LaDapAnDung = false }
                }
            });

            // Câu 14: Đọc hiểu - Chi tiết
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 14, KyNang = "Doc", MaTrinhDo = "B2",
                NoiDungCauHoi = "How do generative AI conversational agents assist language learners according to the passage?",
                DoanVanDoc = passage,
                GiaiThichDapAn = "Bài đọc ghi: 'allowing learners to build speaking and writing confidence without fear of judgment'.",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 53, KyHieuLuaChon = "A", NoiDungLuaChon = "By eliminating the requirement to learn vocabulary.", LaDapAnDung = false },
                    new() { MaLuaChon = 54, KyHieuLuaChon = "B", NoiDungLuaChon = "By offering real cash rewards for correct answers.", LaDapAnDung = false },
                    new() { MaLuaChon = 55, KyHieuLuaChon = "C", NoiDungLuaChon = "By simulating immersive environments to practice without fear of judgment.", LaDapAnDung = true },
                    new() { MaLuaChon = 56, KyHieuLuaChon = "D", NoiDungLuaChon = "By automatically scoring national university entrance examinations.", LaDapAnDung = false }
                }
            });

            // Câu 15: Đọc hiểu - Suy luận & Vai trò giáo viên
            list.Add(new CauHoiKhaoSatDTO
            {
                MaCauHoi = 15, KyNang = "Doc", MaTrinhDo = "B2",
                NoiDungCauHoi = "According to educational experts, what role should human instructors play in AI-driven education?",
                DoanVanDoc = passage,
                GiaiThichDapAn = "Các chuyên gia khẳng định giáo viên vẫn thiết yếu trong việc rèn luyện tư duy phản biện và sự thấu cảm văn hóa.",
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 57, KyHieuLuaChon = "A", NoiDungLuaChon = "They remain vital for nurturing critical thinking and intercultural empathy.", LaDapAnDung = true },
                    new() { MaLuaChon = 58, KyHieuLuaChon = "B", NoiDungLuaChon = "They should solely focus on technical computer hardware maintenance.", LaDapAnDung = false },
                    new() { MaLuaChon = 59, KyHieuLuaChon = "C", NoiDungLuaChon = "They should abandon teaching grammar rules altogether.", LaDapAnDung = false },
                    new() { MaLuaChon = 60, KyHieuLuaChon = "D", NoiDungLuaChon = "They should be replaced by algorithms as soon as possible.", LaDapAnDung = false }
                }
            });

            return list;
        }
    }
}
