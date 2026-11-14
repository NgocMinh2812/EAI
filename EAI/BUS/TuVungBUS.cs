using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class TuVungBUS
    {
        private static TuVungBUS? _instance;
        public static TuVungBUS Instance => _instance ??= new TuVungBUS();

        public List<FlashcardDTO> LayDanhSachFlashcard(string? trinhDo = null)
        {
            int userId = SessionHelper.CurrentMaNguoiDung;
            if (userId <= 0) return new List<FlashcardDTO>();
            return TuVungDAL.Instance.LayDanhSachTuVungFlashcard(userId, trinhDo);
        }

        public void CapNhatKetQuaFlashcard(int maTuVung, bool daNho)
        {
            int userId = SessionHelper.CurrentMaNguoiDung;
            if (userId > 0)
            {
                TuVungDAL.Instance.CapNhatKetQuaFlashcard(userId, maTuVung, daNho);
            }
        }

        public (bool Success, string Message, int MaTuVung) ThemTuVungVaFlashcard(TuVungDTO tuVung)
        {
            if (tuVung == null || string.IsNullOrWhiteSpace(tuVung.Tu))
                return (false, "Từ vựng tiếng Anh không được để trống.", 0);
            if (string.IsNullOrWhiteSpace(tuVung.Nghia))
                return (false, "Nghĩa tiếng Việt không được để trống.", 0);

            int userId = SessionHelper.CurrentMaNguoiDung;
            if (userId <= 0)
                return (false, "Phiên đăng nhập không hợp lệ.", 0);
            bool ok = TuVungDAL.Instance.ThemTuVungVaFlashcard(tuVung, userId, out int maTuVung, out int _);
            if (ok)
                return (true, "Đã lưu từ vựng và tạo Flashcard thành công!", maTuVung);
            return (false, "Lỗi khi lưu từ vựng vào cơ sở dữ liệu.", 0);
        }

        public FlashcardDTO? LayTuVungHomNay()
        {
            return TuVungDAL.Instance.LayTuVungHomNay(SessionHelper.CurrentMaNguoiDung);
        }

        public List<CauHoiKhaoSatDTO> LayCauHoiLuyenTap(string? kyNang = null, string? trinhDo = null, int top = 10)
        {
            return TuVungDAL.Instance.LayCauHoiLuyenTap(kyNang, trinhDo, top);
        }
    }
}
