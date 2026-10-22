using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using BUS;
using DTO;
using GUI.ViewModels.GiangVien;
using GUI.Views.GiangVien;

namespace GUI.Views.Common
{
    /// <summary>
    /// Lớp tiện ích dùng chung xử lý toàn bộ quy trình Tạo & Duyệt bộ câu hỏi AI cho Bài học
    /// Dùng thống nhất giữa màn hình 'Danh mục & Cấu trúc' và 'Nội dung học tập'.
    /// </summary>
    public static class AICauHoiUIHelper
    {
        /// <summary>
        /// Bộ nhớ đệm lưu câu hỏi AI đã tạo theo từng bài học trong phiên làm việc
        /// </summary>
        private static readonly Dictionary<int, List<CauHoiKhaoSatDTO>> _sessionAIDrafts = new();

        public static bool CoBanNhapTrongPhien(int maBaiHoc)
        {
            return _sessionAIDrafts.TryGetValue(maBaiHoc, out var list) && list != null && list.Count > 0;
        }

        public static List<CauHoiKhaoSatDTO>? LayBanNhapTrongPhien(int maBaiHoc)
        {
            if (_sessionAIDrafts.TryGetValue(maBaiHoc, out var list))
                return list;
            return null;
        }

        public static void LuuBanNhapTrongPhien(int maBaiHoc, List<CauHoiKhaoSatDTO> list)
        {
            _sessionAIDrafts[maBaiHoc] = list;
        }

        public static void XoaBanNhapTrongPhien(int maBaiHoc)
        {
            _sessionAIDrafts.Remove(maBaiHoc);
        }

        public static void DonDepCacBaiHocKhongTonTai(IEnumerable<int> existingIds)
        {
            var existingSet = new HashSet<int>(existingIds);
            var staleKeys = new List<int>();
            foreach (var k in _sessionAIDrafts.Keys)
            {
                if (!existingSet.Contains(k))
                    staleKeys.Add(k);
            }
            foreach (var k in staleKeys)
            {
                _sessionAIDrafts.Remove(k);
            }
        }

        /// <summary>
        /// Mở Modal duyệt bộ câu hỏi AI đã có sẵn danh sách câu hỏi
        /// </summary>
        public static bool MoModalDuyetBoCauHoi(
            Window ownerWindow, 
            BaiHocDTO lesson, 
            List<CauHoiKhaoSatDTO> questions, 
            string nguonTao = "AI_BaiHoc")
        {
            if (lesson == null || questions == null || questions.Count == 0) return false;

            var vm = new DuyetBoCauHoiAIViewModel();
            vm.NapDuLieu(
                tenNguon: lesson.TieuDe ?? "Bài học tiếng Anh",
                nguonTao: nguonTao,
                cauHois: questions,
                maBaiHoc: lesson.MaBaiHoc,
                maChuDe: lesson.MaChuDe,
                maTrinhDo: lesson.MaTrinhDoId,
                capDo: lesson.MaTrinhDo ?? "B1",
                kyNang: lesson.LoaiBaiHoc ?? "Doc",
                audioPath: lesson.AudioPath ?? lesson.DuongDanAudio
            );

            var dlg = new DuyetBoCauHoiAIDialog(vm);
            ModalHelper.ShowDialogWithDim(dlg, ownerWindow);

            if (vm.DaLuuThanhCong)
            {
                ThongBaoDialog.ThanhCong(ownerWindow, $"Bộ câu hỏi cho bài học \"{lesson.TieuDe}\" đã được lưu thành công vào Ngân hàng câu hỏi!", "Thành công");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Quy trình trọn gói: Gọi AI sinh câu hỏi theo bài học và mở Modal duyệt bộ câu hỏi
        /// </summary>
        public static async Task<bool> TaoVaDuyetBoCauHoiAsync(
            Window ownerWindow, 
            BaiHocDTO lesson, 
            string nguonTao = "AI_BaiHoc", 
            int soLuong = 10,
            bool forceRecreate = true,
            Action<string>? onStatusUpdate = null)
        {
            if (lesson == null) return false;

            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(lesson.TieuDe) && string.IsNullOrWhiteSpace(lesson.NoiDung) && string.IsNullOrWhiteSpace(lesson.DuongDanTepVatLy))
            {
                ThongBaoDialog.CanhBao(ownerWindow, "Vui lòng nhập tiêu đề hoặc tải tệp tài liệu trước khi tạo câu hỏi bằng AI.", "Thiếu thông tin");
                return false;
            }

            // Kiểm tra ràng buộc âm thanh đối với bài học nghe hiểu
            if (BaiHocDTO.KiemTraKyNangNghe(lesson.LoaiBaiHoc) && 
                string.IsNullOrWhiteSpace(lesson.AudioPath) && 
                string.IsNullOrWhiteSpace(lesson.DuongDanAudio))
            {
                ThongBaoDialog.CanhBao(ownerWindow, 
                    "Bài học này thuộc kỹ năng Luyện nghe (Listening) nhưng chưa có tệp âm thanh (Audio).\n\n" +
                    "Vui lòng đính kèm tệp Audio cho bài học trước khi tạo câu hỏi bằng AI để học viên có thể luyện nghe và hệ thống phát audio cho câu hỏi.", 
                    "Yêu cầu tệp âm thanh");
                return false;
            }

            // Nếu không ép tạo lại và đã có bản nháp trong phiên
            if (!forceRecreate && CoBanNhapTrongPhien(lesson.MaBaiHoc))
            {
                var draft = LayBanNhapTrongPhien(lesson.MaBaiHoc)!;
                return MoModalDuyetBoCauHoi(ownerWindow, lesson, draft, nguonTao);
            }

            onStatusUpdate?.Invoke("AI đang phân tích tài liệu bài học và khởi tạo bộ câu hỏi...");

            try
            {
                var (questions, kyNang, trinhDo, noiDung) = await BoCauHoiBUS.Instance.SinhCauHoiChoBaiHocAsync(
                    lesson, 
                    soLuong: soLuong, 
                    module: nguonTao);

                if (questions == null || questions.Count == 0)
                {
                    ThongBaoDialog.CanhBao(ownerWindow, "AI không thể trích xuất câu hỏi từ nội dung tài liệu này. Vui lòng kiểm tra lại nội dung bài học.", "Không tạo được câu hỏi");
                    return false;
                }

                // Cập nhật lại kỹ năng nếu bài học chưa có
                if (string.IsNullOrWhiteSpace(lesson.LoaiBaiHoc))
                {
                    lesson.LoaiBaiHoc = kyNang;
                }

                // Lưu vào bộ nhớ đệm nháp của phiên
                LuuBanNhapTrongPhien(lesson.MaBaiHoc, questions);

                // Mở giao diện duyệt bộ câu hỏi
                return MoModalDuyetBoCauHoi(ownerWindow, lesson, questions, nguonTao);
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(ownerWindow, $"Lỗi khi tạo câu hỏi bằng AI:\n{ex.Message}", "Lỗi tạo câu hỏi AI");
                return false;
            }
        }
    }
}
