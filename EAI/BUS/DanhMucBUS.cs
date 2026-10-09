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

            // Nút gốc ảo: "Tất cả học liệu & chủ đề"
            int totalLessons = allChuDe.Sum(x => x.SoLuongBaiHoc);
            var rootNode = new TreeNodeDTO
            {
                Id = 0,
                Title = "Tất cả tài liệu",
                Subtitle = "Thư viện học liệu trung tâm",
                Icon = "📚",
                ItemCount = totalLessons,
                IsExpanded = true
            };

            var rootChuDes = allChuDe.Where(x => !x.MaChuDeCha.HasValue).ToList();
            foreach (var r in rootChuDes)
            {
                var node = ConvertChuDeToTreeNode(r, allChuDe);
                rootNode.Children.Add(node);
            }

            tree.Add(rootNode);
            return tree;
        }

        private TreeNodeDTO ConvertChuDeToTreeNode(ChuDeDTO item, List<ChuDeDTO> all)
        {
            var node = new TreeNodeDTO
            {
                Id = item.MaChuDe,
                ParentId = item.MaChuDeCha,
                Title = item.TenChuDe,
                Subtitle = item.MoTa ?? "",
                Icon = item.MaChuDeCha.HasValue ? "📄" : "📁",
                ItemCount = item.SoLuongBaiHoc,
                Tag = item,
                IsExpanded = true
            };

            var children = all.Where(x => x.MaChuDeCha == item.MaChuDe).ToList();
            foreach (var c in children)
            {
                node.Children.Add(ConvertChuDeToTreeNode(c, all));
            }

            return node;
        }

        public (bool Success, string Message) ThemChuDe(ChuDeDTO cd)
        {
            if (string.IsNullOrWhiteSpace(cd.TenChuDe))
                return (false, "Tên chủ đề không được để trống.");

            bool ok = _dal.InsertChuDe(cd);
            if (ok)
                return (true, "Thêm chủ đề thành công.");
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
                return (true, "Cập nhật chủ đề thành công.");
            return (false, "Lỗi cập nhật chủ đề.");
        }

        /// <summary>
        /// Xóa chủ đề kèm kiểm tra logic ràng buộc bài học và chủ đề con
        /// </summary>
        public (bool Success, string Message) XoaChuDe(int maChuDe)
        {
            if (maChuDe <= 0)
                return (false, "Mã chủ đề không hợp lệ.");

            // 1. Kiểm tra xem có chủ đề con không
            int soCon = _dal.DemSoChuDeCon(maChuDe);
            if (soCon > 0)
            {
                return (false, $"Không thể xóa chủ đề này vì đang có {soCon} chủ đề con trực thuộc. Vui lòng chuyển hoặc xóa các chủ đề con trước!");
            }

            // 2. Kiểm tra ràng buộc bài học
            int soBaiHoc = _dal.DemSoBaiHocTheoChuDe(maChuDe);
            if (soBaiHoc > 0)
            {
                return (false, $"Không thể xóa chủ đề này vì đang có {soBaiHoc} bài học liên kết trực tiếp. Vui lòng di dời hoặc xóa các bài học trước khi xóa danh mục!");
            }

            // 3. Tiến hành xóa an toàn
            bool ok = _dal.DeleteChuDe(maChuDe);
            if (ok)
            {
                return (true, "Xóa chủ đề thành công.");
            }
            return (false, "Xảy ra lỗi khi xóa chủ đề trong cơ sở dữ liệu.");
        }
    }
}
