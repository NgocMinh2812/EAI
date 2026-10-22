using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DAL;
using DTO;

namespace BUS
{
    public class DanhMucBUS
    {
        private readonly DanhMucDAL _dal = new DanhMucDAL();

        public List<TrinhDoDTO> LayDanhSachTrinhDo()
        {
            return _dal.GetAllTrinhDo();
        }

        public (bool Success, string Message) ThemTrinhDo(TrinhDoDTO td)
        {
            if (string.IsNullOrWhiteSpace(td.MaCapDo))
                return (false, "Mã cấp độ (A1, A2, B1...) không được để trống.");

            if (string.IsNullOrWhiteSpace(td.TenTrinhDo))
                return (false, "Tên trình độ không được để trống.");

            var all = _dal.GetAllTrinhDo();
            if (all.Any(x => x.MaCapDo.Equals(td.MaCapDo.Trim(), StringComparison.OrdinalIgnoreCase)))
                return (false, $"Mã cấp độ '{td.MaCapDo}' đã tồn tại trong danh mục.");

            bool ok = _dal.InsertTrinhDo(td);
            if (ok)
                return (true, "Thêm trình độ mới thành công!");
            return (false, "Lỗi khi thêm trình độ vào cơ sở dữ liệu.");
        }

        public (bool Success, string Message) CapNhatTrinhDo(TrinhDoDTO td)
        {
            if (td.MaTrinhDo <= 0)
                return (false, "Mã trình độ không hợp lệ.");

            if (string.IsNullOrWhiteSpace(td.MaCapDo))
                return (false, "Mã cấp độ không được để trống.");

            if (string.IsNullOrWhiteSpace(td.TenTrinhDo))
                return (false, "Tên trình độ không được để trống.");

            var all = _dal.GetAllTrinhDo();
            if (all.Any(x => x.MaTrinhDo != td.MaTrinhDo && x.MaCapDo.Equals(td.MaCapDo.Trim(), StringComparison.OrdinalIgnoreCase)))
                return (false, $"Mã cấp độ '{td.MaCapDo}' đã được dùng bởi một trình độ khác.");

            bool ok = _dal.UpdateTrinhDo(td);
            if (ok)
                return (true, "Cập nhật trình độ thành công!");
            return (false, "Lỗi khi cập nhật trình độ vào cơ sở dữ liệu.");
        }

        public (bool Success, string Message) XoaTrinhDo(int maTrinhDo)
        {
            if (maTrinhDo <= 0)
                return (false, "Mã trình độ không hợp lệ.");

            var (baiHoc, cauHoi, deThi) = _dal.DemRangBuocKhoaNgoaiTrinhDo(maTrinhDo);
            if (baiHoc > 0 || cauHoi > 0 || deThi > 0)
            {
                return (false, $"Không thể xóa trình độ này vì đang có dữ liệu ràng buộc: {baiHoc} bài học, {cauHoi} câu hỏi, {deThi} đề thi!");
            }

            bool ok = _dal.DeleteTrinhDo(maTrinhDo);
            if (ok)
                return (true, "Đã xóa trình độ thành công.");
            return (false, "Lỗi khi xóa trình độ trong cơ sở dữ liệu.");
        }

        public List<ChuDeDTO> LayDanhSachChuDe()
        {
            return _dal.GetAllChuDe();
        }

        public List<ChuDeDTO> LayCayChuDe()
        {
            return _dal.GetChuDeTree();
        }

        /// <summary>
        /// Chuyển đổi dữ liệu danh mục thành mô hình TreeNodeDTO phục vụ Binding TreeView WPF
        /// </summary>
        public ObservableCollection<TreeNodeDTO> LayCayTreeNode()
        {
            var tree = new ObservableCollection<TreeNodeDTO>();
            var allChuDe = _dal.GetAllChuDe();

            var visited = new HashSet<int>();
            var rootChuDes = allChuDe.Where(x => !x.MaChuDeCha.HasValue || x.MaChuDeCha <= 0).ToList();
            var rootChildren = new ObservableCollection<TreeNodeDTO>();

            foreach (var r in rootChuDes)
            {
                var node = ConvertChuDeToTreeNode(r, allChuDe, visited);
                rootChildren.Add(node);
            }

            // Nút gốc ảo: "Tất cả tài liệu"
            int totalLessons = 0;
            try
            {
                var cnt = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM BaiHoc");
                if (cnt != null && cnt != DBNull.Value) 
                    totalLessons = Convert.ToInt32(cnt);
                else 
                    totalLessons = rootChildren.Sum(x => x.ItemCount);
            }
            catch
            {
                totalLessons = rootChildren.Sum(x => x.ItemCount);
            }

            var rootNode = new TreeNodeDTO
            {
                Id = 0,
                Title = "Tất cả tài liệu",
                Subtitle = "Thư viện học liệu trung tâm",
                Icon = "📚",
                ItemCount = totalLessons,
                IsExpanded = true,
                Children = rootChildren
            };

            tree.Add(rootNode);
            return tree;
        }

        private TreeNodeDTO ConvertChuDeToTreeNode(ChuDeDTO item, List<ChuDeDTO> all, HashSet<int> visited)
        {
            var node = new TreeNodeDTO
            {
                Id = item.MaChuDe,
                ParentId = item.MaChuDeCha,
                Title = item.TenChuDe,
                Subtitle = item.MoTa ?? "",
                Icon = "📁",
                ItemCount = item.SoLuongBaiHoc,
                Tag = item,
                IsExpanded = true
            };

            if (visited.Contains(item.MaChuDe))
            {
                return node;
            }
            visited.Add(item.MaChuDe);

            var children = all.Where(x => x.MaChuDeCha == item.MaChuDe && x.MaChuDe != item.MaChuDe && !visited.Contains(x.MaChuDe)).ToList();
            foreach (var c in children)
            {
                node.Children.Add(ConvertChuDeToTreeNode(c, all, visited));
            }

            // Tính tổng số lượng bài học: bài học trực tiếp + tổng bài học của tất cả các nhánh con cháu
            node.ItemCount = item.SoLuongBaiHoc + node.Children.Sum(c => c.ItemCount);

            return node;
        }

        public (bool Success, string Message) ThemChuDe(ChuDeDTO cd)
        {
            if (string.IsNullOrWhiteSpace(cd.TenChuDe))
                return (false, "Tên chủ đề không được để trống.");

            bool ok = _dal.InsertChuDe(cd);
            if (ok)
            {
                return (true, "Thêm chủ đề thành công.");
            }
            return (false, "Lỗi thêm chủ đề vào cơ sở dữ liệu.");
        }

        public (bool Success, string Message) CapNhatChuDe(ChuDeDTO cd)
        {
            if (cd.MaChuDe <= 0)
                return (false, "Mã chủ đề không hợp lệ.");

            if (string.IsNullOrWhiteSpace(cd.TenChuDe))
                return (false, "Tên chủ đề không được để trống.");

            if (cd.MaChuDeCha.HasValue && cd.MaChuDeCha.Value == cd.MaChuDe)
                return (false, "Chủ đề cha không thể là chính chủ đề này.");

            bool ok = _dal.UpdateChuDe(cd);
            if (ok)
            {
                return (true, "Cập nhật chủ đề thành công.");
            }
            return (false, "Lỗi cập nhật chủ đề.");
        }

        /// <summary>
        /// Xóa chủ đề: nếu các chủ đề con đều có 0 bài học thì xóa luôn cả cha lẫn toàn bộ con trong 1 transaction an toàn
        /// </summary>
        public (bool Success, string Message) XoaChuDe(int maChuDe)
        {
            if (maChuDe <= 0)
                return (false, "Mã chủ đề không hợp lệ.");

            // 1. Kiểm tra tổng số bài học trong cây chủ đề (bao gồm cả chủ đề này và tất cả chủ đề con)
            int soBaiHoc = _dal.DemTongSoBaiHocTrongCayChuDe(maChuDe);
            if (soBaiHoc > 0)
            {
                return (false, $"Không thể xóa danh mục này vì đang có {soBaiHoc} bài học liên kết (tại danh mục này hoặc các danh mục con trực thuộc). Vui lòng di dời hoặc xóa các bài học trước khi xóa danh mục!");
            }

            // 2. Lấy số lượng chủ đề con trực thuộc để thông báo chính xác cho người dùng
            int soCon = _dal.DemSoChuDeConTrongCay(maChuDe);

            // 3. Tiến hành xóa an toàn toàn bộ (cha + các con nếu có) trong 1 Transaction
            var (success, msg, deletedCount) = _dal.DeleteChuDeVaCacCon(maChuDe);
            if (success)
            {
                string successMsg = soCon > 0
                    ? $"Đã xóa thành công danh mục cha cùng {soCon} danh mục con trực thuộc (toàn bộ đều có 0 bài học)!"
                    : "Đã xóa danh mục thành công!";
                return (true, successMsg);
            }
            return (false, msg);
        }

        public int DemTongSoBaiHocTrongCayChuDe(int rootMaChuDe)
        {
            return _dal.DemTongSoBaiHocTrongCayChuDe(rootMaChuDe);
        }

        public int DemSoChuDeConTrongCay(int rootMaChuDe)
        {
            return _dal.DemSoChuDeConTrongCay(rootMaChuDe);
        }
    }
}
