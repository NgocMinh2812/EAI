using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI;
using DAL;
using DTO;

namespace BUS
{
    public class AISinhDeBUS
    {
        private static AISinhDeBUS? _instance;
        public static AISinhDeBUS Instance => _instance ??= new AISinhDeBUS();

        #region 1. TẠO CÂU HỎI BẰNG AI & LƯU HÀNG ĐỢI
        public async Task<(List<CauHoiDoAISinhItemDTO> Questions, string? Error)> SinhCauHoiVaLuuHangDoiAsync(
            string prompt,
            string level,
            int count,
            string skill,
            string? questionType,
            string modelSelection,
            int userId,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                return (new List<CauHoiDoAISinhItemDTO>(), "Vui lòng nhập chủ đề hoặc yêu cầu tạo câu hỏi.");
            }

            if (count <= 0) count = 5;

            // 1. Ánh xạ Provider & Model (nếu để trống hoặc mặc định, tự động đồng bộ từ Giám sát hệ thống)
            string? preferredProvider = null;
            string? preferredModel = null;

            if (!string.IsNullOrWhiteSpace(modelSelection))
            {
                if (modelSelection.Contains("Gemini", StringComparison.OrdinalIgnoreCase))
                {
                    preferredProvider = "Gemini";
                    preferredModel = "gemini-1.5-flash";
                }
                else if (modelSelection.Contains("Groq", StringComparison.OrdinalIgnoreCase))
                {
                    preferredProvider = "Groq";
                    preferredModel = "llama-3.3-70b-versatile";
                }
                else if (modelSelection.Contains("xKiro", StringComparison.OrdinalIgnoreCase) || modelSelection.Contains("Xkio", StringComparison.OrdinalIgnoreCase))
                {
                    preferredProvider = "Xkio";
                    preferredModel = "qwen/qwen-2.5-72b-instruct";
                }
            }

            // 2. Gọi AI sinh câu hỏi thông qua AIServiceManager hiện có
            var questions = await AIServiceManager.Instance.BocTachCauHoiAsync(
                tieuDe: prompt,
                noiDung: prompt,
                trinhDo: level,
                soLuong: count,
                loaiBaiHoc: skill,
                preferredProvider: preferredProvider,
                preferredModel: preferredModel,
                yeuCauLoaiCauHoi: questionType,
                ct: ct
            );

            if (questions == null || questions.Count == 0)
            {
                return (new List<CauHoiDoAISinhItemDTO>(), "Mô hình AI không trả về câu hỏi hợp lệ. Vui lòng thử lại.");
            }

            // 3. Ánh xạ Trình độ ID (0: A1, 1: A2, 2: B1, 3: B2, 4: C1, 5: C2 chuẩn theo bảng TrinhDo trong DB)
            int? maTrinhDo = level.ToUpper() switch
            {
                "A1" => 0,
                "A2" => 1,
                "B1" => 2,
                "B2" => 3,
                "C1" => 4,
                "C2" => 5,
                var s when s.Contains("IELTS") || s.Contains("TOEIC") => 3,
                _ => 2
            };

            // Nếu modelSelection trống, tự động lấy thông tin mô hình đang hoạt động
            if (string.IsNullOrWhiteSpace(modelSelection))
            {
                string provider = AIServiceManager.Instance.ActiveProviderType.ToString();
                string model = AIServiceManager.Instance.ActiveModel ?? "Mặc định";
                modelSelection = $"{provider} ({model})";
            }

            // 4. Lưu vào CSDL thật: YeuCauSinhDeAI & CauHoiDoAISinh
            long maYeuCau = AISinhDeDAL.Instance.LuuYeuCauSinhDe(userId, "ChuDe", prompt, maTrinhDo, questions.Count, modelSelection);
            var results = new List<CauHoiDoAISinhItemDTO>();

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            foreach (var q in questions)
            {
                if (string.IsNullOrWhiteSpace(q.KyNang)) q.KyNang = skill;
                if (string.IsNullOrWhiteSpace(q.TrinhDo)) q.TrinhDo = level;
                if (string.IsNullOrWhiteSpace(q.DoKho)) q.DoKho = "TrungBinh";
                if (string.IsNullOrWhiteSpace(q.LoaiCauHoi)) q.LoaiCauHoi = questionType ?? "TracNghiem";

                string qJson = JsonSerializer.Serialize(q, jsonOptions);
                long maCauHoiAISinh = AISinhDeDAL.Instance.LuuCauHoiDoAISinh(maYeuCau, qJson, "DangCho");

                results.Add(new CauHoiDoAISinhItemDTO
                {
                    MaCauHoiAISinh = maCauHoiAISinh,
                    MaYeuCauSinhDe = maYeuCau,
                    NoiDungCauHoiSinhRa = qJson,
                    TrangThaiDuyet = "DangCho",
                    NoiDungDauVao = prompt,
                    MoHinhAI = modelSelection,
                    LoaiDauVao = "ChuDe",
                    NgayTao = DateTime.Now,
                    QuestionDetails = q
                });
            }

            return (results, null);
        }

        public List<CauHoiDoAISinhItemDTO> LayDanhSachChoDuyet(string? tuKhoa = null, string? trangThai = "DangCho")
        {
            return AISinhDeDAL.Instance.LayDanhSachCauHoiChoDuyet(tuKhoa, trangThai);
        }

        public int DemSoCauChoDuyet()
        {
            return AISinhDeDAL.Instance.DemSoCauHoiChoDuyet();
        }

        public int DemTongCauHoiDoAISinh()
        {
            return AISinhDeDAL.Instance.DemTongCauHoiDoAISinh();
        }

        public bool CapNhatCauHoiChoDuyet(long maCauHoiAISinh, CauHoiDTO cauHoiDto)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string qJson = JsonSerializer.Serialize(cauHoiDto, jsonOptions);
            return AISinhDeDAL.Instance.CapNhatNoiDungCauHoiAISinh(maCauHoiAISinh, qJson, "DaChinhSua");
        }

        public bool PheDuyetCauHoi(long maCauHoiAISinh, CauHoiDTO q, int nguoiDuyetId, string nguonTao, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(q.NoiDungCauHoi))
            {
                errorMessage = "Nội dung câu hỏi không được để trống.";
                return false;
            }

            var cauHoiDto = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 0,
                NoiDungCauHoi = q.NoiDungCauHoi.Trim(),
                DoanVanDoc = q.DoanVanDoc,
                KyNang = string.IsNullOrWhiteSpace(q.KyNang) ? "Doc" : q.KyNang,
                DoKho = string.IsNullOrWhiteSpace(q.DoKho) ? "TrungBinh" : q.DoKho,
                MaTrinhDo = string.IsNullOrWhiteSpace(q.TrinhDo) ? "B1" : q.TrinhDo,
                LoaiCauHoi = string.IsNullOrWhiteSpace(q.LoaiCauHoi) ? "TracNghiem" : q.LoaiCauHoi,
                GiaiThichDapAn = q.GiaiThichChiTiet,
                DaKiemDuyet = true
            };

            var dsDapAn = new List<LuaChonDapAnDTO>();
            if (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 0)
            {
                for (int i = 0; i < q.DanhSachLuaChon.Count; i++)
                {
                    string kyHieu = ((char)('A' + i)).ToString();
                    string noiDung = CauHoiDTO.StripOptionPrefix(q.DanhSachLuaChon[i]);
                    bool laDung = (q.DapAnDungIndex == i);

                    dsDapAn.Add(new LuaChonDapAnDTO
                    {
                        KyHieuLuaChon = kyHieu,
                        NoiDungLuaChon = noiDung,
                        LaDapAnDung = laDung
                    });
                }
            }
            else if (!string.IsNullOrWhiteSpace(q.DapAnVanBan))
            {
                // Câu hỏi điền từ / trả lời ngắn: thêm đáp án chuẩn
                dsDapAn.Add(new LuaChonDapAnDTO
                {
                    KyHieuLuaChon = "ANS",
                    NoiDungLuaChon = q.DapAnVanBan.Trim(),
                    LaDapAnDung = true
                });
            }

            if (cauHoiDto.LoaiCauHoi == "TracNghiem" && !dsDapAn.Any(x => x.LaDapAnDung))
            {
                if (dsDapAn.Count > 0) dsDapAn[0].LaDapAnDung = true;
            }

            return AISinhDeDAL.Instance.PheDuyetCauHoiAI(maCauHoiAISinh, cauHoiDto, dsDapAn, nguoiDuyetId, nguonTao, out errorMessage);
        }

        public bool PheDuyetCauHoi(long maCauHoiAISinh, CauHoiDTO q, int nguoiDuyetId, string nguonTao, out int maCauHoiMoi, out string errorMessage)
        {
            errorMessage = string.Empty;
            maCauHoiMoi = 0;

            if (string.IsNullOrWhiteSpace(q.NoiDungCauHoi))
            {
                errorMessage = "Nội dung câu hỏi không được để trống.";
                return false;
            }

            var cauHoiDto = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 0,
                NoiDungCauHoi = q.NoiDungCauHoi.Trim(),
                DoanVanDoc = q.DoanVanDoc,
                KyNang = string.IsNullOrWhiteSpace(q.KyNang) ? "Doc" : q.KyNang,
                DoKho = string.IsNullOrWhiteSpace(q.DoKho) ? "TrungBinh" : q.DoKho,
                MaTrinhDo = string.IsNullOrWhiteSpace(q.TrinhDo) ? "B1" : q.TrinhDo,
                LoaiCauHoi = string.IsNullOrWhiteSpace(q.LoaiCauHoi) ? "TracNghiem" : q.LoaiCauHoi,
                GiaiThichDapAn = q.GiaiThichChiTiet,
                DaKiemDuyet = true
            };

            var dsDapAn = new List<LuaChonDapAnDTO>();
            if (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 0)
            {
                for (int i = 0; i < q.DanhSachLuaChon.Count; i++)
                {
                    string kyHieu = ((char)('A' + i)).ToString();
                    string noiDung = CauHoiDTO.StripOptionPrefix(q.DanhSachLuaChon[i]);
                    bool laDung = (q.DapAnDungIndex == i);

                    dsDapAn.Add(new LuaChonDapAnDTO
                    {
                        KyHieuLuaChon = kyHieu,
                        NoiDungLuaChon = noiDung,
                        LaDapAnDung = laDung
                    });
                }
            }
            else if (!string.IsNullOrWhiteSpace(q.DapAnVanBan))
            {
                dsDapAn.Add(new LuaChonDapAnDTO
                {
                    KyHieuLuaChon = "ANS",
                    NoiDungLuaChon = q.DapAnVanBan.Trim(),
                    LaDapAnDung = true
                });
            }

            if (cauHoiDto.LoaiCauHoi == "TracNghiem" && !dsDapAn.Any(x => x.LaDapAnDung))
            {
                if (dsDapAn.Count > 0) dsDapAn[0].LaDapAnDung = true;
            }

            return AISinhDeDAL.Instance.PheDuyetCauHoiAI(maCauHoiAISinh, cauHoiDto, dsDapAn, nguoiDuyetId, nguonTao, out maCauHoiMoi, out errorMessage);
        }

        public bool PheDuyetCauHoiAI(long maCauHoiAISinh, out int maCauHoiMoi, out string errorMessage)
        {
            maCauHoiMoi = 0;
            errorMessage = string.Empty;

            var list = LayDanhSachChoDuyet();
            var item = list.FirstOrDefault(x => x.MaCauHoiAISinh == maCauHoiAISinh);
            if (item == null)
            {
                errorMessage = "Không tìm thấy câu hỏi trong hàng đợi kiểm duyệt.";
                return false;
            }

            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            string nguonTao = item.LoaiDauVao == "TepTin" ? "NhapFile" : "AI";

            return PheDuyetCauHoi(maCauHoiAISinh, item.QuestionDetails, userId, nguonTao, out maCauHoiMoi, out errorMessage);
        }

        public bool TuChoiCauHoi(long maCauHoiAISinh, int nguoiDuyetId, out string errorMessage)
        {
            return AISinhDeDAL.Instance.TuChoiCauHoiAI(maCauHoiAISinh, nguoiDuyetId, out errorMessage);
        }

        public bool TuChoiCauHoiAI(long maCauHoiAISinh, out string errorMessage)
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            return TuChoiCauHoi(maCauHoiAISinh, userId, out errorMessage);
        }

        public (int SuccessCount, int FailCount, List<int> DanhSachMaCauHoiMoi, string ErrorMessage) PheDuyetTatCa(List<CauHoiDoAISinhItemDTO>? danhSach = null)
        {
            var list = danhSach ?? LayDanhSachChoDuyet();
            var dsMaMoi = new List<int>();
            if (list == null || list.Count == 0) return (0, 0, dsMaMoi, "Không có câu hỏi nào đang chờ duyệt.");

            int success = 0;
            int fail = 0;
            string lastErr = "";
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;

            foreach (var item in list)
            {
                string nguonTao = item.LoaiDauVao == "TepTin" ? "NhapFile" : "AI";
                if (PheDuyetCauHoi(item.MaCauHoiAISinh, item.QuestionDetails, userId, nguonTao, out int maMoi, out string err))
                {
                    success++;
                    if (maMoi > 0) dsMaMoi.Add(maMoi);
                }
                else
                {
                    fail++;
                    lastErr = err;
                }
            }
            return (success, fail, dsMaMoi, lastErr);
        }

        public (int SuccessCount, string ErrorMessage) TuChoiTatCa()
        {
            var list = LayDanhSachChoDuyet();
            if (list == null || list.Count == 0) return (0, "Không có câu hỏi nào đang chờ duyệt.");

            int success = 0;
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            string lastErr = "";

            foreach (var item in list)
            {
                if (TuChoiCauHoi(item.MaCauHoiAISinh, userId, out string err))
                {
                    success++;
                }
                else
                {
                    lastErr = err;
                }
            }
            return (success, lastErr);
        }

        public bool CapNhatNoiDungCauHoiAISinh(
            long maCauHoiAISinh,
            string noiDung,
            string? doanVan,
            string? giaiThich,
            string jsonLuaChon,
            out string errorMessage,
            string? kyNang = null,
            string? trinhDo = null,
            string? loaiCauHoi = null,
            string? doKho = null)
        {
            errorMessage = string.Empty;
            try
            {
                var options = JsonSerializer.Deserialize<List<LuaChonDapAnDTO>>(jsonLuaChon);
                var q = new CauHoiDTO
                {
                    NoiDungCauHoi = noiDung,
                    DoanVanDoc = doanVan,
                    GiaiThichChiTiet = giaiThich,
                    DanhSachLuaChon = options?.Select(o => o.NoiDungLuaChon).ToList() ?? new List<string>(),
                    DapAnDungIndex = options != null ? options.FindIndex(o => o.LaDapAnDung) : 0,
                    KyNang = string.IsNullOrWhiteSpace(kyNang) ? "Doc" : kyNang,
                    TrinhDo = string.IsNullOrWhiteSpace(trinhDo) ? "B1" : trinhDo,
                    DoKho = string.IsNullOrWhiteSpace(doKho) ? "TrungBinh" : doKho,
                    LoaiCauHoi = string.IsNullOrWhiteSpace(loaiCauHoi) ? "TracNghiem" : loaiCauHoi
                };

                if (q.DapAnDungIndex < 0) q.DapAnDungIndex = 0;

                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = false,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                string qJson = JsonSerializer.Serialize(q, jsonOptions);
                return AISinhDeDAL.Instance.CapNhatNoiDungCauHoiAISinh(maCauHoiAISinh, qJson, "DaChinhSua");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
        #endregion

        #region 2. BÓC TÁCH TỆP TIN (.xlsx, .docx, .json)
        public class KetQuaBocTachTep : KetQuaBocTachTepTinDTO
        {
        }

        public const long MAX_FILE_SIZE_BYTES = 10 * 1024 * 1024; // 10MB tối đa

        public static string DoanKyNangTuPrompt(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt)) return "Doc";
            string p = prompt.ToLowerInvariant();

            if (p.Contains("ngữ pháp") || p.Contains("ngu phap") || p.Contains("grammar") ||
                p.Contains("thì ") || p.Contains("tense") || p.Contains("câu điều kiện") ||
                p.Contains("lời khuyên") || p.Contains("advice") || p.Contains("modal") ||
                p.Contains("mệnh đề") || p.Contains("bị động") || p.Contains("passive") ||
                p.Contains("so sánh") || p.Contains("comparison") || p.Contains("preposition") || p.Contains("giới từ") ||
                p.Contains("hoàn thành") || p.Contains("quá khứ") || p.Contains("tương lai") || p.Contains("hiện tại"))
            {
                return "NguPhap";
            }

            if (p.Contains("từ vựng") || p.Contains("tu vung") || p.Contains("vocabulary") || p.Contains("vocab") ||
                p.Contains("từ đồng nghĩa") || p.Contains("antonym") || p.Contains("synonym") ||
                p.Contains("chuyên ngành") || p.Contains("idiom") || p.Contains("phrasal verb") || p.Contains("collocation"))
            {
                return "TuVung";
            }

            if (p.Contains("nghe") || p.Contains("listening") || p.Contains("audio") ||
                p.Contains("hội thoại") || p.Contains("dialogue") || p.Contains("conversation"))
            {
                return "Nghe";
            }

            if (p.Contains("đọc") || p.Contains("reading") || p.Contains("đoạn văn") ||
                p.Contains("passage") || p.Contains("bài đọc") || p.Contains("toeic") || p.Contains("part 7") || p.Contains("ielts reading"))
            {
                return "Doc";
            }

            if (p.Contains("viết lại") || p.Contains("sửa lỗi") || p.Contains("writing") ||
                p.Contains("essay") || p.Contains("viết đoạn") || p.Contains("viết thư") || p.Contains("viết email"))
            {
                return "Viet";
            }

            if (p.Contains("nói") || p.Contains("speaking") || p.Contains("phát âm") ||
                p.Contains("pronunciation") || p.Contains("trọng âm") || p.Contains("stress") || p.Contains("ipa"))
            {
                return "Noi";
            }

            return "Doc";
        }

        public static string DoanTrinhDoTuNoiDung(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "B1";
            string t = text.ToUpperInvariant();
            if (t.Contains("C2")) return "C2";
            if (t.Contains("C1")) return "C1";
            if (t.Contains("B2") || t.Contains("TOEIC") || t.Contains("IELTS") || t.Contains("VSTEP")) return "B2";
            if (t.Contains("B1")) return "B1";
            if (t.Contains("A2")) return "A2";
            if (t.Contains("A1")) return "A1";
            return "B1";
        }

        private static string DocVanBanThuanTuTep(string filePath, string ext)
        {
            try
            {
                if (ext == ".docx")
                {
                    var paragraphs = OfficeFileHelper.DocCacDoanVanDocx(filePath);
                    return string.Join("\n\n", paragraphs);
                }
                else if (ext == ".txt")
                {
                    try { return File.ReadAllText(filePath, System.Text.Encoding.UTF8); }
                    catch { return File.ReadAllText(filePath); }
                }
                else if (ext == ".pdf")
                {
                    return KhoHocLieuSyncHelper.DocNoiDungTep(filePath);
                }
                else if (ext == ".json")
                {
                    return File.ReadAllText(filePath, System.Text.Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DocVanBanThuanTuTep] {ex.Message}");
            }
            return string.Empty;
        }

        public async Task<KetQuaBocTachTepTinDTO> BocTachTepTinAsync(string filePath, CancellationToken ct = default)
        {
            var res = new KetQuaBocTachTepTinDTO();

            if (!File.Exists(filePath))
            {
                res.DanhSachLoi.Add("Tệp tin không tồn tại trên máy tính.");
                return res;
            }

            var fi = new FileInfo(filePath);
            res.TenTep = fi.Name;
            res.KichThuocByte = fi.Length;

            // Ràng buộc dung lượng tối đa 10MB theo yêu cầu của hệ thống để AI có thể đọc tốt
            if (fi.Length > MAX_FILE_SIZE_BYTES)
            {
                res.DanhSachLoi.Add("Dung lượng tệp vượt quá giới hạn cho phép (tối đa 10MB) để đảm bảo mô hình AI có thể đọc và phân tích toàn diện.");
                return res;
            }

            string ext = fi.Extension.ToLowerInvariant();
            res.DinhDang = ext switch
            {
                ".docx" => "Word",
                ".json" => "JSON",
                ".txt" => "Text",
                ".pdf" => "PDF",
                _ => "Unknown"
            };

            if (res.DinhDang == "Unknown")
            {
                res.DanhSachLoi.Add("Định dạng tệp không được hỗ trợ. Vui lòng chọn tệp .docx, .txt, .pdf hoặc .json (tối đa 10MB).");
                return res;
            }

            bool daBocTachThanhCong = false;

            // 1. Thử bóc tách trực tiếp nếu là tệp đề thi chuẩn hóa đã có sẵn câu hỏi và các lựa chọn đáp án (.docx / .txt / .json)
            if (ext == ".json")
            {
                try
                {
                    BocTachFileJson(filePath, res);
                    if (res.DanhSachCauHoi.Count > 0 && res.DanhSachCauHoi.Any(q => q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count >= 2))
                    {
                        daBocTachThanhCong = true;
                    }
                    else
                    {
                        res.DanhSachCauHoi.Clear();
                        res.DanhSachLoi.Clear();
                    }
                }
                catch { res.DanhSachCauHoi.Clear(); res.DanhSachLoi.Clear(); }
            }
            else if (ext == ".docx")
            {
                try
                {
                    BocTachFileWord(filePath, res);
                    if (res.DanhSachCauHoi.Count > 0 && res.DanhSachCauHoi.Any(q => q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count >= 2))
                    {
                        daBocTachThanhCong = true;
                    }
                    else
                    {
                        res.DanhSachCauHoi.Clear();
                        res.DanhSachLoi.Clear();
                    }
                }
                catch { res.DanhSachCauHoi.Clear(); res.DanhSachLoi.Clear(); }
            }
            else if (ext == ".txt")
            {
                try
                {
                    BocTachFileTxt(filePath, res);
                    if (res.DanhSachCauHoi.Count > 0 && res.DanhSachCauHoi.All(q => q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count >= 2))
                    {
                        daBocTachThanhCong = true;
                    }
                    else
                    {
                        res.DanhSachCauHoi.Clear();
                        res.DanhSachLoi.Clear();
                    }
                }
                catch { res.DanhSachCauHoi.Clear(); res.DanhSachLoi.Clear(); }
            }

            // 2. NẾU LÀ FILE WORD / PDF, HOẶC TỆP NỘI DUNG CHƯA CÓ ĐÁP ÁN: SỬ DỤNG AI ĐỌC VÀ BIÊN SOẠN CÂU HỎI
            if (!daBocTachThanhCong)
            {
                string rawText = DocVanBanThuanTuTep(filePath, ext);
                if (string.IsNullOrWhiteSpace(rawText) || rawText.Trim().Length < 15)
                {
                    res.DanhSachLoi.Add("Tệp tin không có nội dung văn bản hợp lệ để AI đọc và bóc tách.");
                    return res;
                }

                string safeContent = rawText.Trim();
                if (safeContent.Length > 12000)
                {
                    safeContent = safeContent.Substring(0, 12000);
                    res.DanhSachLoi.Add("Lưu ý: Tệp tin dài, AI đã ưu tiên đọc 12,000 ký tự trọng tâm đầu tiên để đảm bảo chất lượng ra đề cao nhất.");
                }

                string skill = DoanKyNangTuPrompt(fi.Name + " " + safeContent.Substring(0, Math.Min(safeContent.Length, 800)));
                string level = DoanTrinhDoTuNoiDung(fi.Name + " " + safeContent.Substring(0, Math.Min(safeContent.Length, 800)));
                int questionCount = safeContent.Length > 4000 ? 10 : 5;

                // Đồng bộ mô hình AI hoạt động
                AIServiceManager.Instance.SyncActiveConfigFromStorage();
                string title = Path.GetFileNameWithoutExtension(fi.Name);

                try
                {
                    var aiQuestions = await AIServiceManager.Instance.BocTachCauHoiAsync(
                        tieuDe: title,
                        noiDung: safeContent,
                        trinhDo: level,
                        soLuong: questionCount,
                        loaiBaiHoc: skill,
                        preferredProvider: null,
                        preferredModel: null,
                        yeuCauLoaiCauHoi: "TracNghiem",
                        ct: ct
                    );

                    if (aiQuestions != null && aiQuestions.Count > 0)
                    {
                        res.DanhSachCauHoi.Clear();
                        foreach (var q in aiQuestions)
                        {
                            q.KyNang = BoCauHoiBUS.ChuankHoaKyNang(string.IsNullOrWhiteSpace(q.KyNang) ? skill : q.KyNang);
                            q.TrinhDo = string.IsNullOrWhiteSpace(q.TrinhDo) ? level : q.TrinhDo;
                            q.LoaiCauHoi = string.IsNullOrWhiteSpace(q.LoaiCauHoi) ? "TracNghiem" : q.LoaiCauHoi;
                            q.DoKho = string.IsNullOrWhiteSpace(q.DoKho) ? "TrungBinh" : q.DoKho;
                            res.DanhSachCauHoi.Add(q);
                        }

                        res.TongSoCauDocDuoc = res.DanhSachCauHoi.Count;
                        res.SoCauHopLe = res.DanhSachCauHoi.Count;
                    }
                    else
                    {
                        res.DanhSachLoi.Add("Mô hình AI không thể tạo câu hỏi từ tệp tin này. Vui lòng kiểm tra lại cấu hình AI hoặc thử lại.");
                    }
                }
                catch (Exception ex)
                {
                    res.DanhSachLoi.Add($"Lỗi khi AI đọc tệp: {ex.Message}");
                }
            }

            return res;
        }

        private void BocTachFileJson(string filePath, KetQuaBocTachTepTinDTO res)
        {
            string content = File.ReadAllText(filePath);
            var list = JsonSerializer.Deserialize<List<CauHoiDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            });

            if (list == null || list.Count == 0)
            {
                res.DanhSachLoi.Add("Tệp JSON không chứa danh sách câu hỏi hợp lệ.");
                return;
            }

            res.TongSoCauDocDuoc = list.Count;
            int idx = 1;
            foreach (var q in list)
            {
                if (string.IsNullOrWhiteSpace(q.NoiDungCauHoi))
                {
                    res.DanhSachLoi.Add($"Dòng #{idx}: Câu hỏi để trống nội dung.");
                }
                else
                {
                    res.DanhSachCauHoi.Add(q);
                    res.SoCauHopLe++;
                }
                idx++;
            }
        }

        private void BocTachFileExcel(string filePath, KetQuaBocTachTepTinDTO res)
        {
            var rows = OfficeFileHelper.DocBangTinhExcel(filePath);
            if (rows.Count == 0)
            {
                res.DanhSachLoi.Add("Bảng tính Excel không có dữ liệu.");
                return;
            }

            // Bỏ qua dòng tiêu đề nếu dòng 1 chứa các từ khóa header
            int startRow = 0;
            if (rows.Count > 0 && (rows[0].Any(c => c.Contains("Câu hỏi", StringComparison.OrdinalIgnoreCase) || c.Contains("Question", StringComparison.OrdinalIgnoreCase) || c.Contains("STT", StringComparison.OrdinalIgnoreCase))))
            {
                startRow = 1;
            }

            for (int r = startRow; r < rows.Count; r++)
            {
                var row = rows[r];
                if (row.Count < 4) continue;

                res.TongSoCauDocDuoc++;

                // Cấu trúc cột mong đợi:
                // Col 0: STT
                // Col 1: Kỹ năng (Doc / Nghe / NguPhap / TuVung / Viet)
                // Col 2: Trình độ (A1, A2, B1, B2, C1)
                // Col 3: Nội dung câu hỏi
                // Col 4: Đáp án A
                // Col 5: Đáp án B
                // Col 6: Đáp án C
                // Col 7: Đáp án D
                // Col 8: Đáp án đúng (A, B, C, D)
                // Col 9: Giải thích chi tiết

                string kyNang = row.Count > 1 ? row[1].Trim() : "Doc";
                string trinhDo = row.Count > 2 ? row[2].Trim() : "B1";
                string noiDung = row.Count > 3 ? row[3].Trim() : "";

                if (string.IsNullOrWhiteSpace(noiDung))
                {
                    res.DanhSachLoi.Add($"Dòng {r + 1}: Nội dung câu hỏi đang để trống.");
                    continue;
                }

                var options = new List<string>();
                if (row.Count > 4 && !string.IsNullOrWhiteSpace(row[4])) options.Add(row[4].Trim());
                if (row.Count > 5 && !string.IsNullOrWhiteSpace(row[5])) options.Add(row[5].Trim());
                if (row.Count > 6 && !string.IsNullOrWhiteSpace(row[6])) options.Add(row[6].Trim());
                if (row.Count > 7 && !string.IsNullOrWhiteSpace(row[7])) options.Add(row[7].Trim());

                string dapAnDungChar = row.Count > 8 ? row[8].Trim().ToUpperInvariant() : "A";
                int dapAnIdx = 0;
                if (dapAnDungChar.StartsWith("B") || dapAnDungChar == "1") dapAnIdx = 1;
                else if (dapAnDungChar.StartsWith("C") || dapAnDungChar == "2") dapAnIdx = 2;
                else if (dapAnDungChar.StartsWith("D") || dapAnDungChar == "3") dapAnIdx = 3;

                string giaiThich = row.Count > 9 ? row[9].Trim() : "";

                var q = new CauHoiDTO
                {
                    NoiDungCauHoi = noiDung,
                    DanhSachLuaChon = options,
                    DapAnDungIndex = dapAnIdx,
                    KyNang = BoCauHoiBUS.ChuankHoaKyNang(kyNang),
                    TrinhDo = string.IsNullOrWhiteSpace(trinhDo) ? "B1" : trinhDo,
                    DoKho = "TrungBinh",
                    LoaiCauHoi = "TracNghiem",
                    GiaiThichChiTiet = giaiThich
                };

                res.DanhSachCauHoi.Add(q);
                res.SoCauHopLe++;
            }
        }

        private void BocTachFileWord(string filePath, KetQuaBocTachTepTinDTO res)
        {
            var paragraphs = OfficeFileHelper.DocCacDoanVanDocx(filePath);
            if (paragraphs.Count == 0)
            {
                res.DanhSachLoi.Add("Tệp Word không có nội dung văn bản.");
                return;
            }

            CauHoiDTO? currentQ = null;

            foreach (var p in paragraphs)
            {
                string line = p.Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Nhận diện câu hỏi: "Question 1:", "Câu 1:", "1.", "1/"
                var matchQ = Regex.Match(line, @"^(?:Question\s*\d+|Câu\s*\d+|\d+[\.\/\:])\s*(.*)", RegexOptions.IgnoreCase);
                if (matchQ.Success)
                {
                    if (currentQ != null && !string.IsNullOrWhiteSpace(currentQ.NoiDungCauHoi))
                    {
                        res.DanhSachCauHoi.Add(currentQ);
                        res.SoCauHopLe++;
                    }

                    string rawQ = matchQ.Groups[1].Value.Trim();
                    res.TongSoCauDocDuoc++;
                    currentQ = new CauHoiDTO
                    {
                        NoiDungCauHoi = rawQ,
                        DanhSachLuaChon = new List<string>(),
                        DapAnDungIndex = 0,
                        KyNang = DoanKyNangTuPrompt(rawQ),
                        TrinhDo = DoanTrinhDoTuNoiDung(rawQ),
                        LoaiCauHoi = "TracNghiem",
                        DoKho = "TrungBinh"
                    };
                    continue;
                }

                if (currentQ == null) continue;

                // Nhận diện các phương án: "A. ...", "B. ...", "C. ...", "D. ..."
                var matchOpt = Regex.Match(line, @"^([A-Da-d])[\.\)\:\-]\s*(.*)");
                if (matchOpt.Success)
                {
                    currentQ.DanhSachLuaChon ??= new List<string>();
                    string opt = CauHoiDTO.StripOptionPrefix(matchOpt.Groups[2].Value.Trim());
                    currentQ.DanhSachLuaChon.Add(opt);
                    continue;
                }

                // Nhận diện đáp án: "Answer: A", "Đáp án: A"
                var matchAns = Regex.Match(line, @"^(?:Answer|Đáp án|Key)[\:\s]+([A-Da-d])", RegexOptions.IgnoreCase);
                if (matchAns.Success)
                {
                    char c = char.ToUpperInvariant(matchAns.Groups[1].Value[0]);
                    currentQ.DapAnDungIndex = c - 'A';
                    continue;
                }

                // Nhận diện giải thích: "Explanation: ...", "Giải thích: ..."
                var matchExp = Regex.Match(line, @"^(?:Explanation|Giải thích)[\:\s]+(.*)", RegexOptions.IgnoreCase);
                if (matchExp.Success)
                {
                    currentQ.GiaiThichChiTiet = matchExp.Groups[1].Value.Trim();
                    continue;
                }

                // Nếu không khớp, nối tiếp vào nội dung câu hỏi
                if (currentQ.DanhSachLuaChon == null || currentQ.DanhSachLuaChon.Count == 0)
                {
                    currentQ.NoiDungCauHoi += "\n" + line;
                }
            }

            if (currentQ != null && !string.IsNullOrWhiteSpace(currentQ.NoiDungCauHoi))
            {
                res.DanhSachCauHoi.Add(currentQ);
                res.SoCauHopLe++;
            }
        }

        private void BocTachFileTxt(string filePath, KetQuaBocTachTepTinDTO res)
        {
            string[] rawLines;
            try
            {
                rawLines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);
            }
            catch
            {
                rawLines = File.ReadAllLines(filePath);
            }

            if (rawLines == null || rawLines.Length == 0)
            {
                res.DanhSachLoi.Add("Tệp văn bản .txt trống, không có nội dung.");
                return;
            }

            CauHoiDTO? currentQ = null;

            foreach (var rawLine in rawLines)
            {
                string line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("#") || line.StartsWith("//")) continue; // Bỏ qua ghi chú comment

                // 1. Nhận diện tiêu đề câu hỏi: "Question 1:", "Câu 1:", "1.", "1/", "1:", "Q1:", "#Câu 1"
                var matchQ = Regex.Match(line, @"^(?:Question\s*\d+|Câu\s*\d+|Q\s*\d+|\#?Câu\s*\d+|\d+[\.\/\:\-])\s*(.*)", RegexOptions.IgnoreCase);
                if (matchQ.Success)
                {
                    if (currentQ != null && !string.IsNullOrWhiteSpace(currentQ.NoiDungCauHoi))
                    {
                        res.DanhSachCauHoi.Add(currentQ);
                        res.SoCauHopLe++;
                    }

                    string rawQ = matchQ.Groups[1].Value.Trim();
                    res.TongSoCauDocDuoc++;
                    currentQ = new CauHoiDTO
                    {
                        NoiDungCauHoi = rawQ,
                        DanhSachLuaChon = new List<string>(),
                        DapAnDungIndex = 0,
                        KyNang = DoanKyNangTuPrompt(rawQ),
                        TrinhDo = DoanTrinhDoTuNoiDung(rawQ),
                        LoaiCauHoi = "TracNghiem",
                        DoKho = "TrungBinh"
                    };
                    continue;
                }

                if (currentQ == null)
                {
                    // Nếu dòng đầu tiên không có tiền tố STT, khởi tạo làm câu hỏi mở đầu
                    string rawLineClean = line.Trim();
                    currentQ = new CauHoiDTO
                    {
                        NoiDungCauHoi = rawLineClean,
                        DanhSachLuaChon = new List<string>(),
                        DapAnDungIndex = 0,
                        KyNang = DoanKyNangTuPrompt(rawLineClean),
                        TrinhDo = DoanTrinhDoTuNoiDung(rawLineClean),
                        LoaiCauHoi = "TracNghiem",
                        DoKho = "TrungBinh"
                    };
                    res.TongSoCauDocDuoc++;
                    continue;
                }

                // 2. Nhận diện các lựa chọn: "A. ...", "*A. ...", "A) ...", "[A] ...", "(A) ..."
                var matchOpt = Regex.Match(line, @"^[\*\-]?\s*(?:\[|\()?([A-Da-d])(?:\]|\)|\.|\:|\-)\s*(.*)");
                if (matchOpt.Success)
                {
                    currentQ.DanhSachLuaChon ??= new List<string>();
                    string optText = matchOpt.Groups[2].Value.Trim();
                    bool isStarMarked = line.StartsWith("*") || optText.EndsWith("*");
                    if (optText.EndsWith("*")) optText = optText.TrimEnd('*').Trim();
                    optText = CauHoiDTO.StripOptionPrefix(optText);

                    int currentOptIndex = currentQ.DanhSachLuaChon.Count;
                    currentQ.DanhSachLuaChon.Add(optText);

                    if (isStarMarked)
                    {
                        currentQ.DapAnDungIndex = currentOptIndex;
                    }
                    continue;
                }

                // 3. Nhận diện đáp án đúng: "Answer: A", "Đáp án: A", "Key: A", "ĐA: A", hoặc đáp án text
                var matchAns = Regex.Match(line, @"^(?:Answer|Đáp án|Dap an|Key|ĐA|Ans)[\:\s]+(.*)", RegexOptions.IgnoreCase);
                if (matchAns.Success)
                {
                    string ansValue = matchAns.Groups[1].Value.Trim();
                    if (ansValue.Length == 1 && char.IsLetter(ansValue[0]))
                    {
                        char c = char.ToUpperInvariant(ansValue[0]);
                        if (c >= 'A' && c <= 'Z')
                        {
                            currentQ.DapAnDungIndex = c - 'A';
                        }
                    }
                    else
                    {
                        currentQ.DapAnVanBan = ansValue;
                        if (currentQ.DanhSachLuaChon == null || currentQ.DanhSachLuaChon.Count == 0)
                        {
                            currentQ.LoaiCauHoi = "DienKhuyet";
                        }
                    }
                    continue;
                }

                // 4. Nhận diện giải thích: "Explanation: ...", "Giải thích: ..."
                var matchExp = Regex.Match(line, @"^(?:Explanation|Giải thích|Giai thich|Ghi chú)[\:\s]+(.*)", RegexOptions.IgnoreCase);
                if (matchExp.Success)
                {
                    currentQ.GiaiThichChiTiet = matchExp.Groups[1].Value.Trim();
                    continue;
                }

                // 5. Nếu chưa có lựa chọn nào, ghép dòng này vào nội dung câu hỏi
                if (currentQ.DanhSachLuaChon == null || currentQ.DanhSachLuaChon.Count == 0)
                {
                    currentQ.NoiDungCauHoi += "\n" + line;
                }
            }

            if (currentQ != null && !string.IsNullOrWhiteSpace(currentQ.NoiDungCauHoi))
            {
                if (currentQ.DanhSachLuaChon != null && currentQ.DanhSachLuaChon.Count == 2 &&
                    currentQ.DanhSachLuaChon.Any(x => x.Equals("True", StringComparison.OrdinalIgnoreCase) || x.Equals("Đúng", StringComparison.OrdinalIgnoreCase)))
                {
                    currentQ.LoaiCauHoi = "DungSai";
                }
                res.DanhSachCauHoi.Add(currentQ);
                res.SoCauHopLe++;
            }
        }

        public bool LuuCauHoiNhapTepVaoHangDoi(string filePath, List<CauHoiDTO> questions, int userId)
        {
            if (questions == null || questions.Count == 0) return false;

            string fileName = Path.GetFileName(filePath);
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            string dinhDang = ext == ".xlsx" ? "Excel" : ext == ".docx" ? "Word" : ext == ".txt" ? "Text" : "JSON";

            // 1. Ghi nhận vào TepTinNhapLieu
            AISinhDeDAL.Instance.LuuTepTinNhapLieu(userId, fileName, dinhDang, filePath, questions.Count, "HoanThanh");

            // 2. Tạo yêu cầu sinh đề AI (loại TepTin)
            long maYeuCau = AISinhDeDAL.Instance.LuuYeuCauSinhDe(userId, "TepTin", fileName, 3, questions.Count, $"Nhập từ tệp {dinhDang}");

            // 3. Đưa từng câu hỏi vào hàng đợi kiểm duyệt
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            foreach (var q in questions)
            {
                string json = JsonSerializer.Serialize(q, jsonOptions);
                AISinhDeDAL.Instance.LuuCauHoiDoAISinh(maYeuCau, json, "DangCho");
            }

            return true;
        }

        public bool LuuCauHoiNhapTepVaoNganHang(string filePath, List<CauHoiDTO> questions, int userId, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (questions == null || questions.Count == 0)
            {
                errorMessage = "Không có câu hỏi hợp lệ để lưu.";
                return false;
            }

            string fileName = Path.GetFileName(filePath);
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            string dinhDang = ext == ".xlsx" ? "Excel" : ext == ".docx" ? "Word" : ext == ".txt" ? "Text" : "JSON";

            // 1. Ghi nhận vào TepTinNhapLieu
            AISinhDeDAL.Instance.LuuTepTinNhapLieu(userId, fileName, dinhDang, filePath, questions.Count, "HoanThanh");

            // 2. Chuyển đổi và lưu vào ngân hàng câu hỏi
            int countSuccess = 0;
            foreach (var q in questions)
            {
                var ch = new CauHoiKhaoSatDTO
                {
                    MaCauHoi = 0,
                    NoiDungCauHoi = q.NoiDungCauHoi,
                    DoanVanDoc = q.DoanVanDoc,
                    KyNang = string.IsNullOrWhiteSpace(q.KyNang) ? "Doc" : q.KyNang,
                    MaTrinhDo = string.IsNullOrWhiteSpace(q.TrinhDo) ? "B1" : q.TrinhDo,
                    DoKho = string.IsNullOrWhiteSpace(q.DoKho) ? "TrungBinh" : q.DoKho,
                    LoaiCauHoi = string.IsNullOrWhiteSpace(q.LoaiCauHoi) ? "TracNghiem" : q.LoaiCauHoi,
                    GiaiThichDapAn = q.GiaiThichChiTiet,
                    DaKiemDuyet = true
                };

                var dsDapAn = new List<LuaChonDapAnDTO>();
                if (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 0)
                {
                    for (int i = 0; i < q.DanhSachLuaChon.Count; i++)
                    {
                        dsDapAn.Add(new LuaChonDapAnDTO
                        {
                            KyHieuLuaChon = ((char)('A' + i)).ToString(),
                            NoiDungLuaChon = CauHoiDTO.StripOptionPrefix(q.DanhSachLuaChon[i]),
                            LaDapAnDung = (q.DapAnDungIndex == i)
                        });
                    }
                }
                else if (!string.IsNullOrWhiteSpace(q.DapAnVanBan))
                {
                    dsDapAn.Add(new LuaChonDapAnDTO
                    {
                        KyHieuLuaChon = "ANS",
                        NoiDungLuaChon = q.DapAnVanBan,
                        LaDapAnDung = true
                    });
                }

                if (!dsDapAn.Any(x => x.LaDapAnDung) && dsDapAn.Count > 0)
                {
                    dsDapAn[0].LaDapAnDung = true;
                }

                if (CauHoiDAL.Instance.LuuCauHoiVaDapAn(ch, dsDapAn, userId, out string err, nguonTao: "NhapFile"))
                {
                    countSuccess++;
                }
            }

            return countSuccess > 0;
        }

        public bool LuuCauHoiVaoHangDoiTuTep(
            List<CauHoiDTO> questions,
            string filePath,
            int userId,
            out int soLuongLuu,
            out string errorMessage)
        {
            errorMessage = string.Empty;
            soLuongLuu = 0;
            if (questions == null || questions.Count == 0)
            {
                errorMessage = "Danh sách câu hỏi trống.";
                return false;
            }

            bool ok = LuuCauHoiNhapTepVaoHangDoi(filePath, questions, userId);
            if (ok)
            {
                soLuongLuu = questions.Count;
                return true;
            }
            errorMessage = "Không thể ghi nhận câu hỏi vào hàng đợi cơ sở dữ liệu.";
            return false;
        }

        public bool LuuCauHoiVaoNganHangTuTep(
            List<CauHoiDTO> questions,
            int userId,
            out int soLuongLuu,
            out string errorMessage)
        {
            errorMessage = string.Empty;
            soLuongLuu = 0;
            if (questions == null || questions.Count == 0)
            {
                errorMessage = "Danh sách câu hỏi trống.";
                return false;
            }

            bool ok = LuuCauHoiNhapTepVaoNganHang("NhapTep", questions, userId, out errorMessage);
            if (ok)
            {
                soLuongLuu = questions.Count;
                return true;
            }
            return false;
        }
        #endregion

        #region 3. TẠO TỆP TIN MẪU CHUẨN HOÁ
        public static List<CauHoiDTO> TaoBoCauHoiMau()
        {
            return new List<CauHoiDTO>
            {
                new CauHoiDTO
                {
                    NoiDungCauHoi = "She ______ to school by bus every morning.",
                    KyNang = "NguPhap",
                    TrinhDo = "A2",
                    DoKho = "De",
                    LoaiCauHoi = "TracNghiem",
                    DanhSachLuaChon = new List<string> { "go", "goes", "going", "went" },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "Thì hiện tại đơn diễn tả thói quen lặp lại, chủ ngữ ngôi thứ 3 số ít 'She' chia động từ 'goes'."
                },
                new CauHoiDTO
                {
                    NoiDungCauHoi = "Which word is closest in meaning to 'enormous'?",
                    KyNang = "TuVung",
                    TrinhDo = "B1",
                    DoKho = "TrungBinh",
                    LoaiCauHoi = "TracNghiem",
                    DanhSachLuaChon = new List<string> { "Tiny", "Huge", "Weak", "Fast" },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "'Enormous' và 'Huge' đều mang nghĩa là to lớn, khổng lồ."
                },
                new CauHoiDTO
                {
                    NoiDungCauHoi = "If I had known about the meeting, I ______ attended.",
                    KyNang = "NguPhap",
                    TrinhDo = "B2",
                    DoKho = "Kho",
                    LoaiCauHoi = "TracNghiem",
                    DanhSachLuaChon = new List<string> { "would have", "will have", "had", "would" },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "Cấu trúc câu điều kiện loại 3: If + S + had + V3/ed, S + would have + V3/ed."
                },
                new CauHoiDTO
                {
                    NoiDungCauHoi = "The company reported a substantial increase in quarterly revenue despite the economic downturn.",
                    KyNang = "Doc",
                    TrinhDo = "B2",
                    DoKho = "TrungBinh",
                    LoaiCauHoi = "TracNghiem",
                    DanhSachLuaChon = new List<string> { "slight", "significant", "negligible", "temporary" },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "Trong ngữ cảnh kinh tế và thương mại, 'substantial' đồng nghĩa với 'significant' (đáng kể, vượt bậc)."
                },
                new CauHoiDTO
                {
                    NoiDungCauHoi = "Where will the international technology conference be hosted next month?",
                    KyNang = "Nghe",
                    TrinhDo = "B1",
                    DoKho = "TrungBinh",
                    LoaiCauHoi = "TracNghiem",
                    DanhSachLuaChon = new List<string> { "At the central convention center", "Online via video conference", "In the main university hall", "At the city exhibition center" },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "Thông tin xác nhận sự kiện hội nghị công nghệ diễn ra tại Trung tâm hội nghị trung tâm."
                }
            };
        }

        public void XuatTepMauExcel(string filePath)
        {
            var sheet = new OfficeFileHelper.ExcelSheetData
            {
                SheetName = "MauCauHoi_EAI",
                Headers = new List<string>
                {
                    "STT", "Kỹ năng (Doc/Nghe/NguPhap/TuVung)", "Trình độ (A1-C1)", 
                    "Nội dung câu hỏi", "Đáp án A", "Đáp án B", "Đáp án C", "Đáp án D", 
                    "Đáp án đúng (A/B/C/D)", "Giải thích chi tiết"
                }
            };

            var questions = TaoBoCauHoiMau();
            int idx = 1;
            foreach (var q in questions)
            {
                string optA = q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 0 ? q.DanhSachLuaChon[0] : "";
                string optB = q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 1 ? q.DanhSachLuaChon[1] : "";
                string optC = q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 2 ? q.DanhSachLuaChon[2] : "";
                string optD = q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 3 ? q.DanhSachLuaChon[3] : "";
                char correctChar = (q.DapAnDungIndex.HasValue && q.DapAnDungIndex.Value >= 0) ? (char)('A' + q.DapAnDungIndex.Value) : 'A';

                sheet.Rows.Add(new List<string>
                {
                    idx.ToString(),
                    q.KyNang ?? "Doc",
                    "B1",
                    q.NoiDungCauHoi,
                    optA,
                    optB,
                    optC,
                    optD,
                    correctChar.ToString(),
                    q.GiaiThichChiTiet ?? ""
                });
                idx++;
            }

            OfficeFileHelper.TaoTepExcel(filePath, new List<OfficeFileHelper.ExcelSheetData> { sheet });
        }

        public void XuatTepMauJson(string filePath)
        {
            var questions = TaoBoCauHoiMau();
            var json = JsonSerializer.Serialize(questions, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
        }

        public void XuatTepMauWord(string filePath)
        {
            var questions = TaoBoCauHoiMau();
            OfficeFileHelper.TaoTepMauWord(filePath, "BỘ ĐỀ THI MẪU TIẾNG ANH - ENGLISHEAI", questions);
        }

        public void XuatTepMauTxt(string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# ================================================================");
            sb.AppendLine("# TỆP ĐỀ THI MẪU TIẾNG ANH CHUẨN ĐỊNH DẠNG TEXT (.TXT) - ENGLISHEAI");
            sb.AppendLine("# Quy tắc: Question 1 / Câu 1, các đáp án A/B/C/D, Answer / Đáp án, Explanation / Giải thích");
            sb.AppendLine("# ================================================================");
            sb.AppendLine();

            var questions = TaoBoCauHoiMau();
            int idx = 1;
            foreach (var q in questions)
            {
                sb.AppendLine($"Question {idx}: {q.NoiDungCauHoi}");
                if (q.DanhSachLuaChon != null)
                {
                    for (int i = 0; i < q.DanhSachLuaChon.Count; i++)
                    {
                        char optChar = (char)('A' + i);
                        sb.AppendLine($"{optChar}. {q.DanhSachLuaChon[i]}");
                    }
                }
                char ansChar = (q.DapAnDungIndex.HasValue && q.DapAnDungIndex.Value >= 0) ? (char)('A' + q.DapAnDungIndex.Value) : 'A';
                sb.AppendLine($"Answer: {ansChar}");
                if (!string.IsNullOrWhiteSpace(q.GiaiThichChiTiet))
                {
                    sb.AppendLine($"Explanation: {q.GiaiThichChiTiet}");
                }
                sb.AppendLine();
                idx++;
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        public void XuatFileMauExcel(string filePath) => XuatTepMauExcel(filePath);
        public void TaoFileMauJson(string filePath) => XuatTepMauJson(filePath);
        public void TaoFileMauDocx(string filePath) => XuatTepMauWord(filePath);
        public void TaoFileMauTxt(string filePath) => XuatTepMauTxt(filePath);
        #endregion
    }
}
