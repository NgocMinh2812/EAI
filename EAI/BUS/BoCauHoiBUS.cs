using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DTO;

namespace BUS
{
    public class BoCauHoiBUS
    {
        private static BoCauHoiBUS? _instance;
        public static BoCauHoiBUS Instance => _instance ??= new BoCauHoiBUS();

        public List<BoCauHoiDTO> LayDanhSachBoCauHoi(string? tuKhoa = null, string? nguonTao = null)
        {
            return BoCauHoiDAL.Instance.LayDanhSachBoCauHoi(tuKhoa, nguonTao);
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiTheoBo(int maBoCauHoi, string? tuKhoa = null, string? kyNang = null, string? doKho = null, bool? daKiemDuyet = null)
        {
            if (maBoCauHoi < 0) return new List<CauHoiKhaoSatDTO>();
            return BoCauHoiDAL.Instance.LayDanhSachCauHoiTheoBo(maBoCauHoi, tuKhoa, kyNang, doKho, daKiemDuyet);
        }

        public List<CauHoiKhaoSatDTO> LayTatCaCauHoi(string? tuKhoa = null, string? kyNang = null, string? doKho = null, int top = 500, bool? daKiemDuyet = null)
        {
            return BoCauHoiDAL.Instance.LayTatCaCauHoi(tuKhoa, kyNang, doKho, top, daKiemDuyet);
        }

        public int DemTongSoCauHoi()
        {
            return BoCauHoiDAL.Instance.DemTongSoCauHoi();
        }

        public bool LuuBoCauHoi(BoCauHoiDTO bo, List<CauHoiKhaoSatDTO> dsCauHoi, int nguoiTaoId, out string errorMessage)
        {
            if (bo == null)
            {
                errorMessage = "Bộ câu hỏi không hợp lệ.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(bo.TenBoCauHoi))
            {
                errorMessage = "Vui lòng nhập tên cho bộ câu hỏi.";
                return false;
            }

            if (dsCauHoi == null || dsCauHoi.Count == 0)
            {
                errorMessage = "Bộ câu hỏi cần có ít nhất một câu hỏi được chọn.";
                return false;
            }

            return BoCauHoiDAL.Instance.LuuBoCauHoi(bo, dsCauHoi, nguoiTaoId, out errorMessage);
        }

        public bool DoiTenBoCauHoi(int maBo, string tenMoi, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(tenMoi))
            {
                errorMessage = "Tên bộ câu hỏi không được để trống.";
                return false;
            }
            return BoCauHoiDAL.Instance.DoiTenBoCauHoi(maBo, tenMoi.Trim(), out errorMessage);
        }

        public bool XoaBoCauHoi(int maBo, out string errorMessage)
        {
            return BoCauHoiDAL.Instance.XoaBoCauHoi(maBo, out errorMessage);
        }

        public bool XoaCauHoiKhoiBo(int maBo, int maCauHoi, out string errorMessage)
        {
            return BoCauHoiDAL.Instance.XoaCauHoiKhoiBo(maBo, maCauHoi, out errorMessage);
        }

        public bool ThemCauHoiVaoBo(int maBo, int maCauHoi, out string errorMessage)
        {
            return BoCauHoiDAL.Instance.ThemCauHoiVaoBo(maBo, maCauHoi, out errorMessage);
        }

        public bool XoaCauHoiVinhVien(int maCauHoi, out string errorMessage)
        {
            return BoCauHoiDAL.Instance.XoaCauHoiVinhVien(maCauHoi, out errorMessage);
        }

        public static string ChuankHoaKyNang(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "Doc";
            string lower = raw.Trim().ToLowerInvariant();
            if (lower.Contains("nghe") || lower.Contains("listen")) return "Nghe";
            if (lower.Contains("nguphap") || lower.Contains("grammar")) return "NguPhap";
            if (lower.Contains("tuvung") || lower.Contains("vocab")) return "TuVung";
            if (lower.Contains("viet") || lower.Contains("writ")) return "Viet";
            if (lower.Contains("doc") || lower.Contains("read")) return "Doc";
            return "Doc";
        }

        public static string ChuanHoaDoKho(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "TrungBinh";
            string lower = raw.Trim().ToLowerInvariant();
            if (lower.Contains("de") || lower.Contains("easy")) return "De";
            if (lower.Contains("kho") || lower.Contains("hard") || lower.Contains("diff")) return "Kho";
            return "TrungBinh";
        }

        public static string TrichXuatDoanVanDocHieu(string? rawContent, int maxLength = 1500)
        {
            if (string.IsNullOrWhiteSpace(rawContent)) return string.Empty;

            string text = rawContent.Trim();

            // Tìm kiếm các phần đoạn văn đọc hiểu trong bài nếu có tiêu đề rõ ràng
            string[] markers = new[]
            {
                "■ READING COMPREHENSION",
                "READING COMPREHENSION",
                "■ BÀI ĐỌC HIỂU",
                "BÀI ĐỌC HIỂU",
                "Đoạn văn đọc hiểu",
                "ĐOẠN VĂN ĐỌC HIỂU",
                "Reading Passage",
                "READING PASSAGE",
                "Contextual Dialogue",
                "Kịch bản đối thoại",
                "2. ĐOẠN VĂN",
                "2. BÀI ĐỌC",
                "2. NỘI DUNG BÀI ĐỌC"
            };

            foreach (var marker in markers)
            {
                int idx = text.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    int start = idx + marker.Length;
                    while (start < text.Length && (text[start] == ':' || text[start] == '-' || text[start] == '\r' || text[start] == '\n' || text[start] == ' '))
                    {
                        start++;
                    }

                    string sub = text.Substring(start);
                    string[] endMarkers = new[] { "\n3.", "\n4.", "\n■ CÂU HỎI", "\nCÂU HỎI", "\n💡 Master Tip", "\n---" };
                    int endIdx = -1;
                    foreach (var em in endMarkers)
                    {
                        int pos = sub.IndexOf(em, StringComparison.OrdinalIgnoreCase);
                        if (pos > 0 && (endIdx == -1 || pos < endIdx))
                        {
                            endIdx = pos;
                        }
                    }

                    if (endIdx > 100)
                    {
                        string passage = sub.Substring(0, endIdx).Trim();
                        if (passage.Length > maxLength) passage = passage.Substring(0, maxLength).Trim() + "...";
                        return passage;
                    }
                    else if (sub.Length > 100)
                    {
                        string passage = sub.Length > maxLength ? sub.Substring(0, maxLength).Trim() + "..." : sub.Trim();
                        return passage;
                    }
                }
            }

            // Nếu không có marker đặc thù, lấy đoạn nội dung chính bỏ qua dòng tiêu đề
            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new System.Text.StringBuilder();
            foreach (var line in lines)
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("■") && sb.Length == 0) continue;
                if (trimmed.StartsWith("Tiêu đề:") && sb.Length == 0) continue;
                sb.AppendLine(trimmed);
                if (sb.Length >= maxLength) break;
            }

            string res = sb.ToString().Trim();
            if (res.Length > maxLength)
            {
                res = res.Substring(0, maxLength).Trim() + "...";
            }
            return string.IsNullOrWhiteSpace(res) ? text : res;
        }

        /// <summary>
        /// Chuẩn hóa và chuyển đổi danh sách câu hỏi AI (CauHoiDTO) thành danh sách CauHoiKhaoSatDTO đầy đủ lựa chọn
        /// </summary>
        public static List<CauHoiKhaoSatDTO> ChuyenDoiTuCauHoiAI(
            List<CauHoiDTO> aiQuestions, 
            string kyNang = "Doc", 
            string maTrinhDo = "B1", 
            int? maChuDe = null, 
            int? maTrinhDoId = null,
            string? audioPath = null,
            string? noiDungBaiHoc = null)
        {
            var result = new List<CauHoiKhaoSatDTO>();
            if (aiQuestions == null || aiQuestions.Count == 0) return result;

            string normalizedDefaultKyNang = ChuankHoaKyNang(kyNang);
            string extractedPassage = TrichXuatDoanVanDocHieu(noiDungBaiHoc);

            int thuTu = 1;
            foreach (var aiQ in aiQuestions)
            {
                string chKyNang = !string.IsNullOrWhiteSpace(aiQ.KyNang) 
                    ? ChuankHoaKyNang(aiQ.KyNang) 
                    : normalizedDefaultKyNang;

                string chDoKho = !string.IsNullOrWhiteSpace(aiQ.DoKho)
                    ? ChuanHoaDoKho(aiQ.DoKho)
                    : (thuTu <= 2 ? "De" : (thuTu >= 8 ? "Kho" : "TrungBinh"));

                string chLoai = CauHoiKhaoSatDTO.ChuanHoaLoaiCauHoi(!string.IsNullOrWhiteSpace(aiQ.LoaiCauHoi) ? aiQ.LoaiCauHoi : "TracNghiem");

                string? chDoanVan = !string.IsNullOrWhiteSpace(aiQ.DoanVanDoc)
                    ? aiQ.DoanVanDoc
                    : (!string.IsNullOrWhiteSpace(aiQ.AudioScript) ? aiQ.AudioScript : (chKyNang == "Doc" ? extractedPassage : null));

                var ch = new CauHoiKhaoSatDTO
                {
                    MaCauHoi = 0,
                    ThuTuCauHoi = thuTu,
                    ThuTuTrongBo = thuTu,
                    KyNang = chKyNang,
                    MaTrinhDo = string.IsNullOrWhiteSpace(maTrinhDo) ? "B1" : maTrinhDo,
                    NoiDungCauHoi = aiQ.NoiDungCauHoi?.Trim() ?? "",
                    GiaiThichDapAn = aiQ.GiaiThichChiTiet?.Trim(),
                    LoaiCauHoi = chLoai,
                    DoKho = chDoKho,
                    DoanVanDoc = chDoanVan,
                    MaChuDe = maChuDe,
                    MaTrinhDoId = maTrinhDoId,
                    AudioPath = !string.IsNullOrWhiteSpace(audioPath) ? audioPath : null,
                    DaKiemDuyet = true,
                    IsSelected = aiQ.IsSelected
                };

                // Phân tích và tạo danh sách đáp án theo từng loại câu hỏi
                if (aiQ.DanhSachLuaChon != null && aiQ.DanhSachLuaChon.Count > 0)
                {
                    for (int i = 0; i < aiQ.DanhSachLuaChon.Count; i++)
                    {
                        string rawOpt = aiQ.DanhSachLuaChon[i]?.Trim() ?? "";
                        string kyHieu = ((char)('A' + i)).ToString();
                        string noiDung = rawOpt;

                        // Tách ký hiệu A. B. C. D. nếu có
                        if (noiDung.Length >= 3 && char.IsLetter(noiDung[0]) && (noiDung[1] == '.' || noiDung[1] == ':'))
                        {
                            kyHieu = noiDung[0].ToString().ToUpper();
                            noiDung = noiDung.Substring(2).Trim();
                        }

                        // Loại bỏ ký hiệu "(Đúng ✓)" hoặc "(Correct)" ở cuối nếu có
                        bool laDung = (i == aiQ.DapAnDungIndex);
                        if (noiDung.Contains("(Đúng") || noiDung.Contains("(Correct") || noiDung.Contains("✓"))
                        {
                            laDung = true;
                            noiDung = noiDung.Replace("(Đúng ✓)", "")
                                             .Replace("(Đúng)", "")
                                             .Replace("(Correct ✓)", "")
                                             .Replace("(Correct)", "")
                                             .Replace("✓", "").Trim();
                        }

                        ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO
                        {
                            KyHieuLuaChon = kyHieu,
                            NoiDungLuaChon = noiDung,
                            LaDapAnDung = laDung
                        });
                    }
                }

                // Xử lý riêng cho câu hỏi Đúng / Sai nếu chưa có danh sách lựa chọn
                if (ch.IsDangDungSai && ch.DanhSachLuaChon.Count == 0)
                {
                    ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "True (Đúng)", LaDapAnDung = aiQ.DapAnDungIndex == 0 });
                    ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "False (Sai)", LaDapAnDung = aiQ.DapAnDungIndex == 1 });
                }
                // Xử lý riêng cho câu hỏi True / False / Not Given nếu chưa có danh sách lựa chọn
                else if (ch.IsDangTrueFalseNotGiven && ch.DanhSachLuaChon.Count == 0)
                {
                    ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "True", LaDapAnDung = aiQ.DapAnDungIndex == 0 });
                    ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "False", LaDapAnDung = aiQ.DapAnDungIndex == 1 });
                    ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "C", NoiDungLuaChon = "Not Given", LaDapAnDung = aiQ.DapAnDungIndex == 2 });
                }
                // Xử lý cho câu hỏi nhập văn bản (Điền khuyết, Trả lời ngắn, Viết lại câu, Sửa lỗi)
                else if (ch.IsDangNhapVanBan)
                {
                    if (!string.IsNullOrWhiteSpace(aiQ.DapAnVanBan))
                    {
                        ch.DanhSachLuaChon.Clear();
                        ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO
                        {
                            KyHieuLuaChon = "KEY",
                            NoiDungLuaChon = aiQ.DapAnVanBan.Trim(),
                            LaDapAnDung = true
                        });
                    }
                    else if (ch.DanhSachLuaChon.Count == 0 && !string.IsNullOrWhiteSpace(ch.GiaiThichDapAn))
                    {
                        ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO
                        {
                            KyHieuLuaChon = "KEY",
                            NoiDungLuaChon = ch.GiaiThichDapAn,
                            LaDapAnDung = true
                        });
                    }
                }
                // Xử lý cho câu hỏi Tự luận / Viết bài (TuLuan)
                else if (ch.IsDangTuLuan)
                {
                    string rubric = !string.IsNullOrWhiteSpace(aiQ.TieuChiCham) 
                        ? aiQ.TieuChiCham 
                        : "Tiêu chí chấm: Bám sát đề bài, Bố cục mạch lạc, Vốn từ học thuật, Độ chuẩn xác ngữ pháp.";
                    ch.DanhSachLuaChon.Clear();
                    ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO
                    {
                        KyHieuLuaChon = "RUB",
                        NoiDungLuaChon = rubric,
                        LaDapAnDung = true
                    });
                }

                // Bảo đảm ít nhất 1 đáp án đúng
                if (ch.DanhSachLuaChon.Count > 0 && !ch.DanhSachLuaChon.Any(x => x.LaDapAnDung))
                {
                    ch.DanhSachLuaChon[0].LaDapAnDung = true;
                }

                result.Add(ch);
                thuTu++;
            }

            return result;
        }

        /// <summary>
        /// Tạo bộ câu hỏi hoàn chỉnh từ AI cho một bài học (tự động đọc nội dung từ tệp vật lý/văn bản, xác định kỹ năng & trình độ)
        /// Dùng chung chuẩn hóa cho cả màn hình Danh mục & Cấu trúc và Nội dung học tập.
        /// </summary>
        public async Task<(List<CauHoiKhaoSatDTO> Questions, string KyNang, string TrinhDo, string NoiDungDaDoc)> SinhCauHoiChoBaiHocAsync(
            BaiHocDTO baiHoc, 
            int soLuong = 10, 
            string? module = null)
        {
            if (baiHoc == null) return (new List<CauHoiKhaoSatDTO>(), "Doc", "B1", "");

            // 1. Trích xuất nội dung thực tế (ưu tiên đọc từ tệp đính kèm vật lý qua KhoHocLieuSyncHelper nếu có)
            string contentToAnalyze = KhoHocLieuSyncHelper.DocNoiDungTep(baiHoc.DuongDanTepVatLy, baiHoc.NoiDung);
            if (string.IsNullOrWhiteSpace(contentToAnalyze))
            {
                contentToAnalyze = baiHoc.TieuDe ?? "Bài học tiếng Anh";
            }

            // 2. Tự động chuẩn hóa Kỹ năng và Trình độ
            string exactKyNang = baiHoc.LoaiBaiHoc;
            if (string.IsNullOrWhiteSpace(exactKyNang))
            {
                exactKyNang = BaiHocDTO.XacDinhKyNangTuTenHoacChuDe(baiHoc.TieuDe, baiHoc.TenChuDe, baiHoc.DuongDanTepVatLy);
            }
            string exactTrinhDo = string.IsNullOrWhiteSpace(baiHoc.MaTrinhDo) ? "B1" : baiHoc.MaTrinhDo;
            string tieuDe = string.IsNullOrWhiteSpace(baiHoc.TieuDe) ? "Bài học tiếng Anh" : baiHoc.TieuDe.Trim();

            // 3. Gọi AI bóc tách câu hỏi
            var aiQuestions = await GeminiAIService.BocTachCauHoiAIAsync(
                tieuDe,
                contentToAnalyze,
                exactTrinhDo,
                soLuong,
                module: module ?? "Chung",
                loaiBaiHoc: exactKyNang);

            // 4. Chuyển đổi sang danh sách câu hỏi chuẩn DTO
            var convertedQuestions = ChuyenDoiTuCauHoiAI(
                aiQuestions,
                kyNang: exactKyNang,
                maTrinhDo: exactTrinhDo,
                maChuDe: baiHoc.MaChuDe,
                maTrinhDoId: baiHoc.MaTrinhDoId,
                audioPath: baiHoc.AudioPath ?? baiHoc.DuongDanAudio,
                noiDungBaiHoc: contentToAnalyze);

            return (convertedQuestions, exactKyNang, exactTrinhDo, contentToAnalyze);
        }
    }
}
