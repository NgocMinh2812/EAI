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
                Icon = (item.MaChuDeCha.HasValue && item.MaChuDeCha > 0) ? "📄" : "📁",
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
                // Đồng bộ thư mục vật lý trong KhoHocLieu
                var all = _dal.GetAllChuDe();
                var inserted = all.FirstOrDefault(x => x.TenChuDe == cd.TenChuDe) ?? cd;
                KhoHocLieuSyncHelper.TaoThuMucChuDe(inserted, all);

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

            var allOld = _dal.GetAllChuDe();
            var oldItem = allOld.FirstOrDefault(x => x.MaChuDe == cd.MaChuDe);
            string oldName = oldItem?.TenChuDe ?? cd.TenChuDe;

            bool ok = _dal.UpdateChuDe(cd);
            if (ok)
            {
                // Đồng bộ đổi tên thư mục vật lý trong KhoHocLieu
                var allNew = _dal.GetAllChuDe();
                KhoHocLieuSyncHelper.DoiTenThuMucChuDe(oldName, cd, allNew);

                return (true, "Cập nhật chủ đề thành công.");
            }
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

            var all = _dal.GetAllChuDe();
            var target = all.FirstOrDefault(x => x.MaChuDe == maChuDe);

            // 3. Tiến hành xóa an toàn
            bool ok = _dal.DeleteChuDe(maChuDe);
            if (ok)
            {
                // Đồng bộ xóa thư mục vật lý trong KhoHocLieu
                if (target != null)
                {
                    KhoHocLieuSyncHelper.XoaThuMucChuDe(target, all);
                }

                return (true, "Xóa chủ đề thành công.");
            }
            return (false, "Xảy ra lỗi khi xóa chủ đề trong cơ sở dữ liệu.");
        }

        /// <summary>
        /// Đồng bộ toàn bộ cây danh mục CSDL ra cây thư mục đĩa KhoHocLieu
        /// </summary>
        public (int FoldersCreated, int TotalTopics) DongBoKhoHocLieu()
        {
            var all = _dal.GetAllChuDe();
            return KhoHocLieuSyncHelper.DongBoToanBoKhoHocLieu(all);
        }
    }
}
