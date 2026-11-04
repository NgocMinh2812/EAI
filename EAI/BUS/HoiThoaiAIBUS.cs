using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI;
using BUS.AI.Configuration;
using BUS.AI.Models;
using DAL;
using DTO;

namespace BUS
{
    public class HoiThoaiAIBUS
    {
        private static HoiThoaiAIBUS? _instance;
        public static HoiThoaiAIBUS Instance => _instance ??= new HoiThoaiAIBUS();

        private readonly HoiThoaiAIDAL _dal = HoiThoaiAIDAL.Instance;

        public List<PhienHoiThoaiDTO> LayDanhSachPhienHoiThoai(int maNguoiDung)
        {
            try
            {
                return _dal.LayDanhSachPhienHoiThoai(maNguoiDung);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HoiThoaiAIBUS] Lỗi lấy danh sách phiên: {ex.Message}");
                return new List<PhienHoiThoaiDTO>();
            }
        }

        public long TaoPhienHoiThoai(int maNguoiDung, string tieuDe, string nguCanh = "General")
        {
            try
            {
                return _dal.TaoPhienHoiThoai(maNguoiDung, tieuDe, nguCanh);
            }
            catch
            {
                return 0;
            }
        }

        public bool CapNhatTieuDePhien(long maPhien, string tieuDe)
        {
            try
            {
                return _dal.CapNhatTieuDePhien(maPhien, tieuDe);
            }
            catch
            {
                return false;
            }
        }

        public bool XoaPhienHoiThoai(long maPhien, int maNguoiDung)
        {
            try
            {
                return _dal.XoaPhienHoiThoai(maPhien, maNguoiDung);
            }
            catch
            {
                return false;
            }
        }

        public List<TinNhanAIDTO> LayDanhSachTinNhan(long maPhien)
        {
            try
            {
                return _dal.LayDanhSachTinNhan(maPhien);
            }
            catch
            {
                return new List<TinNhanAIDTO>();
            }
        }

        public long LuuTinNhan(long maPhien, string loaiNguoiGui, string noiDung)
        {
            try
            {
                return _dal.ThemTinNhan(maPhien, loaiNguoiGui, noiDung);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Gửi câu hỏi đến Trợ lý AI sử dụng Provider và Model được Admin cấu hình qua AIConfigurationService
        /// </summary>
        public async Task<AIResponse> GuiCauHoiChoAIAsync(string userMessage, string mode = "General", CancellationToken ct = default)
        {
            string activeModel = AIConfigurationService.Instance.ActiveModel;
            var activeProvider = AIConfigurationService.Instance.ActiveProvider;

            string systemPrompt = mode switch
            {
                "Grammar" =>
                    "Bạn là Gia sư AI Tiếng Anh chuyên sâu về NGỮ PHÁP và CẤU TRÚC CÂU. " +
                    "Nhiệm vụ: Giải thích chi tiết bản chất ngữ pháp, so sánh cấu trúc, nêu rõ công thức, " +
                    "chỉ ra các lỗi sai thường gặp và đưa ra bài tập ví dụ sinh động cho học viên. Trình bày song ngữ rõ ràng.",
                "Vocab" =>
                    "Bạn là Gia sư AI Tiếng Anh chuyên về TỪ VỰNG, THÀNH NGỮ và COLLOCATIONS (IELTS/TOEIC). " +
                    "Nhiệm vụ: Cung cấp nghĩa, từ loại, phiên âm IPA, các collocations học thuật hay (Band 7.0+), " +
                    "từ đồng nghĩa/trái nghĩa và đặt câu ví dụ thực tế.",
                "Explain" =>
                    "Bạn là Chuyên gia luyện thi Tiếng Anh hướng dẫn GIẢI THÍCH ĐÁP ÁN BÀI TẬP. " +
                    "Nhiệm vụ: Phân tích vì sao một đáp án là ĐÚNG, vì sao các phương án khác là SAI (bẫy đề thi), " +
                    "và đúc kết mẹo làm bài thi nhanh và chính xác nhất cho học viên.",
                "Speaking" =>
                    "You are a friendly, natural English Conversation Partner and Tutor. " +
                    "Roleplay or chat interactively with the student in English. Keep the dialogue engaging, " +
                    "provide gentle corrections if they make mistakes, and suggest more natural, native-like expressions.",
                _ =>
                    "Bạn là Gia sư AI Tiếng Anh thông minh, thân thiện của hệ thống EnglishAI. " +
                    "Nhiệm vụ: Giải đáp ngữ pháp, từ vựng, phân tích câu, hướng dẫn học tập và luyện giao tiếp song ngữ Anh - Việt. " +
                    "Trình bày mạch lạc, có cấu trúc gạch đầu dòng và ví dụ minh họa."
            };

            string fullPrompt = $"{systemPrompt}\n\n[Câu hỏi của học viên]:\n{userMessage}";

            return await AIServiceManager.Instance.ExecutePromptAsync(
                prompt: fullPrompt,
                preferredProvider: activeProvider.ToString(),
                preferredModel: activeModel,
                actionName: "TroLyAI Chat",
                ct: ct);
        }

        public int DemTongLuotDung()
        {
            return _dal.DemTongSoLuotDungTroLyAI();
        }
    }
}

