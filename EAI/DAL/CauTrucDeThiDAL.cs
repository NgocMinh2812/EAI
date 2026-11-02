using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class CauTrucDeThiDAL
    {
        private static CauTrucDeThiDAL? _instance;
        public static CauTrucDeThiDAL Instance => _instance ??= new CauTrucDeThiDAL();

        /// <summary>
        /// Lấy toàn bộ danh sách cấu trúc đề thi chuẩn hóa (Exam Blueprints)
        /// </summary>
        public List<CauTrucDeThiDTO> LayDanhSachCauTruc(string? maKyThi = null, string? loaiDeThi = null)
        {
            var list = new List<CauTrucDeThiDTO>();
            try
            {
                string query = @"
                    SELECT ct.MaCauTruc, ct.TenCauTruc, ct.MaKyThi, ct.MaCapDo, ct.LoaiDeThi, 
                           ct.ThoiGianPhut, ct.TongSoCau, ct.TongDiem, ct.MoTa, ct.NguonThamChieu, 
                           ct.NgayXacMinh, ct.TrangThai
                    FROM CauTrucDeThi ct
                    WHERE (@MaKyThi IS NULL OR ct.MaKyThi = @MaKyThi)
                      AND (@LoaiDeThi IS NULL OR ct.LoaiDeThi = @LoaiDeThi)
                    ORDER BY ct.MaKyThi ASC, ct.MaCapDo ASC";

                var parameters = new[]
                {
                    new SqlParameter("@MaKyThi", (object?)maKyThi ?? DBNull.Value),
                    new SqlParameter("@LoaiDeThi", (object?)loaiDeThi ?? DBNull.Value)
                };

                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
                foreach (DataRow r in dt.Rows)
                {
                    var dto = MapRowToDTO(r);
                    list.Add(dto);
                }

                // Nạp chi tiết các phần thi cho từng blueprint
                foreach (var bp in list)
                {
                    bp.DanhSachPhanThi = LayChiTietPhanThi(bp.MaCauTruc);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CauTrucDeThiDAL.LayDanhSachCauTruc] {ex.Message}");
            }
            return list;
        }

        public CauTrucDeThiDTO? LayTheoMa(int maCauTruc)
        {
            try
            {
                string query = @"
                    SELECT ct.MaCauTruc, ct.TenCauTruc, ct.MaKyThi, ct.MaCapDo, ct.LoaiDeThi, 
                           ct.ThoiGianPhut, ct.TongSoCau, ct.TongDiem, ct.MoTa, ct.NguonThamChieu, 
                           ct.NgayXacMinh, ct.TrangThai
                    FROM CauTrucDeThi ct
                    WHERE ct.MaCauTruc = @MaCauTruc";

                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaCauTruc", maCauTruc) });
                if (dt.Rows.Count > 0)
                {
                    var bp = MapRowToDTO(dt.Rows[0]);
                    bp.DanhSachPhanThi = LayChiTietPhanThi(bp.MaCauTruc);
                    return bp;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CauTrucDeThiDAL.LayTheoMa] {ex.Message}");
            }
            return null;
        }

        public List<ChiTietCauTrucDeThiDTO> LayChiTietPhanThi(int maCauTruc)
        {
            var parts = new List<ChiTietCauTrucDeThiDTO>();
            try
            {
                string query = @"
                    SELECT MaChiTiet, MaCauTruc, TenPhan, ThuTuPhan, KyNang, LoaiCauHoiChoPhep,
                           SoLuongCau, DiemMoiCau, ThoiGianPhut, YeuCauDoanVan, YeuCauAudio, MoTa
                    FROM ChiTietCauTrucDeThi
                    WHERE MaCauTruc = @MaCauTruc
                    ORDER BY ThuTuPhan ASC";

                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaCauTruc", maCauTruc) });
                foreach (DataRow r in dt.Rows)
                {
                    parts.Add(new ChiTietCauTrucDeThiDTO
                    {
                        MaChiTiet = Convert.ToInt32(r["MaChiTiet"]),
                        MaCauTruc = Convert.ToInt32(r["MaCauTruc"]),
                        TenPhan = r["TenPhan"]?.ToString() ?? "",
                        ThuTuPhan = Convert.ToInt32(r["ThuTuPhan"]),
                        KyNang = r["KyNang"]?.ToString() ?? "Doc",
                        LoaiCauHoiChoPhep = r["LoaiCauHoiChoPhep"]?.ToString() ?? "TracNghiem",
                        SoLuongCau = Convert.ToInt32(r["SoLuongCau"]),
                        DiemMoiCau = Convert.ToDouble(r["DiemMoiCau"]),
                        ThoiGianPhut = r["ThoiGianPhut"] == DBNull.Value ? null : Convert.ToInt32(r["ThoiGianPhut"]),
                        YeuCauDoanVan = Convert.ToBoolean(r["YeuCauDoanVan"]),
                        YeuCauAudio = Convert.ToBoolean(r["YeuCauAudio"]),
                        MoTa = r["MoTa"]?.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CauTrucDeThiDAL.LayChiTietPhanThi] {ex.Message}");
            }
            return parts;
        }

        public bool GanCauTrucChoBaiTapDeThi(int maBaiTapDeThi, int? maCauTruc)
        {
            try
            {
                string query = "UPDATE BaiTapDeThi SET MaCauTrucDeThi = @MaCauTruc WHERE MaBaiTapDeThi = @MaBai";
                int rows = SqlDataProvider.Instance.ExecuteNonQuery(query, new[]
                {
                    new SqlParameter("@MaCauTruc", (object?)maCauTruc ?? DBNull.Value),
                    new SqlParameter("@MaBai", maBaiTapDeThi)
                });
                return rows > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CauTrucDeThiDAL.GanCauTrucChoBaiTapDeThi] {ex.Message}");
                return false;
            }
        }

        private static CauTrucDeThiDTO MapRowToDTO(DataRow r)
        {
            return new CauTrucDeThiDTO
            {
                MaCauTruc = Convert.ToInt32(r["MaCauTruc"]),
                TenCauTruc = r["TenCauTruc"]?.ToString() ?? "",
                MaKyThi = r["MaKyThi"]?.ToString() ?? "IELTS",
                MaCapDo = r["MaCapDo"]?.ToString() ?? "B2",
                LoaiDeThi = r["LoaiDeThi"]?.ToString() ?? "ThiThu",
                ThoiGianPhut = Convert.ToInt32(r["ThoiGianPhut"]),
                TongSoCau = Convert.ToInt32(r["TongSoCau"]),
                TongDiem = Convert.ToDouble(r["TongDiem"]),
                MoTa = r["MoTa"]?.ToString(),
                NguonThamChieu = r["NguonThamChieu"]?.ToString(),
                NgayXacMinh = Convert.ToDateTime(r["NgayXacMinh"]),
                TrangThai = r["TrangThai"]?.ToString() ?? "HoatDong"
            };
        }
    }
}

