using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DTO;

namespace BUS
{
    public class BaiHocBUS
    {
        private static List<BaiHocDTO>? _cacheBaiHoc;
        private static readonly DAL.BaiHocDAL _baiHocDAL = new DAL.BaiHocDAL();

        public static List<BaiHocDTO> LayDanhSachTuDatabase(int? maChuDe = null, string? search = null)
        {
            try
            {
                var list = _baiHocDAL.GetAll(maChuDe, search) ?? new List<BaiHocDTO>();
                KhoHocLieuSyncHelper.EnrichBaiHocFileInfo(list);
                return list;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocBUS] Lỗi đọc bài học từ DB: {ex.Message}");
                return new List<BaiHocDTO>();
            }
        }

        public static List<BaiHocDTO> LayDanhSachBaiHoc()
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            _cacheBaiHoc = BaiHocDAL.Instance.LayDanhSachBaiHoc(userId);
            return _cacheBaiHoc;
        }

        public static BaiHocDTO? LayChiTietBaiHoc(int maBaiHoc)
        {
            var list = LayDanhSachBaiHoc();
            var item = list.FirstOrDefault(b => b.MaBaiHoc == maBaiHoc);
            if (item != null)
            {
                NapCauHoiChoBaiHoc(item);
                return item;
            }

            var allFallbacks = new[] { "A1", "A2", "B1", "B2", "C1" }
                .SelectMany(BaiHocDAL.TaoDanhSachBaiHocDuPhongTheoTrinhDo);
            var fb = allFallbacks.FirstOrDefault(b => b.MaBaiHoc == maBaiHoc);
            if (fb != null) NapCauHoiChoBaiHoc(fb);
            return fb;
        }

        private static void NapCauHoiChoBaiHoc(BaiHocDTO baiHoc)
        {
            if (baiHoc.DanhSachCauHoi != null && baiHoc.DanhSachCauHoi.Count > 0)
                return;

            baiHoc.DanhSachCauHoi = new List<CauHoiDTO>();
            try
            {
                string query = @"
                    SELECT TOP 10 ch.MaCauHoi, ch.NoiDungCauHoi, ch.GiaiThichDapAn
                    FROM CauHoi ch
                    WHERE ch.MaBaiHoc = @MaBai
                    ORDER BY ch.MaCauHoi ASC";

                var dt = SqlDataProvider.Instance.ExecuteQuery(query, new[]
                {
                    new Microsoft.Data.SqlClient.SqlParameter("@MaBai", baiHoc.MaBaiHoc)
                });

                foreach (System.Data.DataRow r in dt.Rows)
                {
                    int maCauHoi = Convert.ToInt32(r["MaCauHoi"]);
                    var chDto = new CauHoiDTO
                    {
                        MaCauHoi = maCauHoi,
                        NoiDungCauHoi = r["NoiDungCauHoi"]?.ToString() ?? "",
                        GiaiThichChiTiet = r["GiaiThichDapAn"] == DBNull.Value ? "Đáp án đã được kiểm chứng chuẩn xác." : r["GiaiThichDapAn"]?.ToString() ?? ""
                    };

                    string qOpt = "SELECT KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung FROM LuaChonDapAn WHERE MaCauHoi = @MaCH ORDER BY KyHieuLuaChon ASC";
                    var dtOpt = SqlDataProvider.Instance.ExecuteQuery(qOpt, new[] { new Microsoft.Data.SqlClient.SqlParameter("@MaCH", maCauHoi) });

                    int optIdx = 0;
                    foreach (System.Data.DataRow optRow in dtOpt.Rows)
                    {
                        string nd = optRow["NoiDungLuaChon"]?.ToString() ?? "";
                        bool isCorrect = Convert.ToBoolean(optRow["LaDapAnDung"]);
                        chDto.DanhSachLuaChon.Add(nd);
                        if (isCorrect)
                        {
                            chDto.DapAnDungIndex = optIdx;
                        }
                        optIdx++;
                    }

                    if (chDto.DanhSachLuaChon.Count > 0)
                    {
                        baiHoc.DanhSachCauHoi.Add(chDto);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocBUS.NapCauHoiChoBaiHoc] Lỗi: {ex.Message}");
            }
        }

        public static void CapNhatTienDo(int maBaiHoc, double phanTram, string trangThai, string diemSo)
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            string dbTrangThai = phanTram >= 100 ? "HoanThanh" : (phanTram > 0 ? "DangHoc" : "ChuaHoc");

            if (userId > 0)
            {
                BaiHocDAL.Instance.CapNhatTienDo(userId, maBaiHoc, phanTram, dbTrangThai);
            }

            var baiHoc = LayChiTietBaiHoc(maBaiHoc);
            if (baiHoc != null)
            {
                baiHoc.PhanTramTienDo = Math.Min(100, Math.Max(0, phanTram));
                baiHoc.TrangThai = trangThai;
                baiHoc.DiemSo = diemSo;
                baiHoc.NgayHocGanNhat = "Hôm nay";

                if (baiHoc.PhanTramTienDo >= 100)
                {
                    baiHoc.TrangThai = "Hoàn thành";
                    baiHoc.StatusColor = "#0F766E";
                    baiHoc.StatusBackground = "#E6F7F3";
                    baiHoc.ButtonActionText = "Ôn tập lại";
                }
                else if (baiHoc.PhanTramTienDo > 0)
                {
                    baiHoc.TrangThai = "Đang học";
                    baiHoc.StatusColor = "#2563EB";
                    baiHoc.StatusBackground = "#EFF6FF";
                    baiHoc.ButtonActionText = "Học tiếp";
                }
            }
        }

        public static void CongDonThoiGianHoc(int soPhut)
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            if (userId > 0 && soPhut > 0)
            {
                BaiHocDAL.Instance.CongDonThoiGianHoc(userId, soPhut);
                if (SessionHelper.CurrentHoSo != null)
                {
                    SessionHelper.CurrentHoSo.SoPhutHocTichLuy += soPhut;
                }
            }
        }

        public static ThongKeHocTapDTO LayThongKeSinhVien()
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            return BaiHocDAL.Instance.LayThongKeSinhVien(userId);
        }

        public static List<BaiHocDTO> LayDanhSachBaiHocGanDay(int top = 4)
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            return BaiHocDAL.Instance.LayDanhSachBaiHocGanDay(userId, top);
        }

        public static (bool Success, string Message, int NewId) ThemBaiHoc(BaiHocDTO bh)
        {
            if (string.IsNullOrWhiteSpace(bh.TieuDe))
                return (false, "Tiêu đề học liệu không được để trống.", 0);

            if (bh.MaChuDe <= 0)
                return (false, "Vui lòng chọn danh mục chủ đề hợp lệ cho tài liệu.", 0);

            // Kiểm tra trùng lặp tiêu đề trong cùng chủ đề
            if (_baiHocDAL.KiemTraTrungTieuDe(bh.MaChuDe, bh.TieuDe))
            {
                return (false, $"Chủ đề này đã có bài học / tài liệu mang tiêu đề '{bh.TieuDe}'. Vui lòng đổi tiêu đề khác hoặc chọn tài liệu hiện có để tránh trùng lặp.", 0);
            }

            bool ok = _baiHocDAL.InsertBaiHoc(bh);
            if (ok)
            {
                _cacheBaiHoc = null; // Invalidate cache
                return (true, "Đã lưu học liệu thành công vào cơ sở dữ liệu!", bh.MaBaiHoc);
            }
            return (false, "Lỗi khi lưu học liệu vào cơ sở dữ liệu.", 0);
        }

        public static (bool Success, string Message) TaoDeThiThuTuBaiHoc(int maBaiHoc, string tenDeThi, int? nguoiTao = null)
        {
            if (maBaiHoc <= 0)
                return (false, "Mã bài học không hợp lệ.");

            bool ok = _baiHocDAL.TaoDeThiThuTuBaiHoc(maBaiHoc, tenDeThi, nguoiTao);
            if (ok)
                return (true, $"Đã tạo bộ đề thi thử '{tenDeThi}' và lưu vào hệ thống thành công!");
            return (false, "Lỗi khi tạo bộ đề thi thử trong cơ sở dữ liệu.");
        }

        public static (bool Success, string Message) XoaBaiHoc(int maBaiHoc)
        {
            if (maBaiHoc <= 0)
                return (false, "Mã bài học không hợp lệ.");

            try
            {
                bool ok = _baiHocDAL.DeleteBaiHoc(maBaiHoc);
                if (ok)
                {
                    _cacheBaiHoc = null;
                    return (true, "Đã xóa bài học / đề thi thành công.");
                }
                return (false, "Không tìm thấy bài học để xóa hoặc thao tác thất bại.");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi xóa bài học: {ex.Message}");
            }
        }

        public static (bool Success, string Message) DiChuyenBaiHoc(int maBaiHoc, int maChuDeMoi)
        {
            if (maBaiHoc <= 0)
                return (false, "Mã bài học không hợp lệ.");
            if (maChuDeMoi <= 0)
                return (false, "Vui lòng chọn thư mục đích hợp lệ.");

            try
            {
                bool ok = _baiHocDAL.DiChuyenBaiHoc(maBaiHoc, maChuDeMoi);
                if (ok)
                {
                    _cacheBaiHoc = null;
                    return (true, "Đã di chuyển bài học sang thư mục mới thành công!");
                }
                return (false, "Không thể di chuyển bài học. Vui lòng thử lại.");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi di chuyển bài học: {ex.Message}");
            }
        }

        public static bool LuuBaiHoc(BaiHocDTO baiHoc, out string errorMessage)
        {
            _cacheBaiHoc = null;
            return _baiHocDAL.LuuBaiHoc(baiHoc, out errorMessage);
        }

        public static bool CapNhatTrangThaiXuatBan(int maBaiHoc, bool daXuatBan)
        {
            _cacheBaiHoc = null;
            return _baiHocDAL.CapNhatTrangThaiXuatBan(maBaiHoc, daXuatBan);
        }
    }
}
