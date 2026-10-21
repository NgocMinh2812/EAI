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

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiTheoBo(int maBoCauHoi, string? tuKhoa = null, string? kyNang = null, string? doKho = null)
        {
            if (maBoCauHoi <= 0) return new List<CauHoiKhaoSatDTO>();
            return BoCauHoiDAL.Instance.LayDanhSachCauHoiTheoBo(maBoCauHoi, tuKhoa, kyNang, doKho);
        }

        public List<CauHoiKhaoSatDTO> LayTatCaCauHoi(string? tuKhoa = null, string? kyNang = null, string? doKho = null, int top = 100)
        {
            return BoCauHoiDAL.Instance.LayTatCaCauHoi(tuKhoa, kyNang, doKho, top);
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

                string chLoai = !string.IsNullOrWhiteSpace(aiQ.LoaiCauHoi)
                    ? aiQ.LoaiCauHoi
                    : "TracNghiem";

                string? chDoanVan = !string.IsNullOrWhiteSpace(aiQ.DoanVanDoc)
                    ? aiQ.DoanVanDoc
                    : (chKyNang == "Doc" ? extractedPassage : null);

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
                    AudioPath = chKyNang == "Nghe" ? audioPath : null,
                    DaKiemDuyet = true,
                    IsSelected = aiQ.IsSelected
                };

                // Phân tích và tạo danh sách đáp án
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
    }
}
