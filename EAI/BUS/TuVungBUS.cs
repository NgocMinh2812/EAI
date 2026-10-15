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
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            return TuVungDAL.Instance.LayDanhSachTuVungFlashcard(userId, trinhDo);
        }

        public void CapNhatKetQuaFlashcard(int maTuVung, bool daNho)
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            if (userId > 0)
            {
                TuVungDAL.Instance.CapNhatKetQuaFlashcard(userId, maTuVung, daNho);
            }
        }

        public List<CauHoiKhaoSatDTO> LayCauHoiLuyenTap(string? kyNang = null, string? trinhDo = null, int top = 10)
        {
            return TuVungDAL.Instance.LayCauHoiLuyenTap(kyNang, trinhDo, top);
        }
    }
}
