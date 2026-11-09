using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class TienDoHocDAL
    {
        private static TienDoHocDAL? _instance;
        public static TienDoHocDAL Instance => _instance ??= new TienDoHocDAL();

        /// <summary>
        /// Lấy toàn bộ thông tin KPI và tiến độ mục tiêu của học viên
        /// </summary>
        public TongKetTienDoDTO LayTongKetTienDo(int maNguoiDung, string filter)
        {
            var result = new TongKetTienDoDTO();
            DateTime? batDau = TinhNgayBatDau(filter);

            // 1. Lấy thông tin Hồ sơ học tập (Mục tiêu, Trình độ đầu vào, Giờ học tích lũy gốc)
            try
            {
                string queryHoSo = @"
                    SELECT TOP 1 DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, TrinhDoDauVao, SoPhutHocTichLuy
                    FROM HoSoHocTap
                    WHERE MaNguoiDung = @MaND";

                DataTable dtHoSo = SqlDataProvider.Instance.ExecuteQuery(queryHoSo, new[] { new SqlParameter("@MaND", maNguoiDung) });
                if (dtHoSo.Rows.Count > 0)
                {
                    DataRow r = dtHoSo.Rows[0];
                    result.DiemMucTieu = r["DiemMucTieu"] == DBNull.Value ? null : Convert.ToDecimal(r["DiemMucTieu"]);
                    result.KhungNangLucMucTieu = r["KhungNangLucMucTieu"]?.ToString();
                    result.TrinhDoMucTieu = r["TrinhDoMucTieu"]?.ToString();
                    result.TrinhDoDauVao = dtHoSo.Columns.Contains("TrinhDoDauVao") && r["TrinhDoDauVao"] != DBNull.Value ? r["TrinhDoDauVao"]?.ToString() : null;
                    if (r["SoPhutHocTichLuy"] != DBNull.Value)
                    {
                        result.TongSoPhutHoc = Convert.ToInt32(r["SoPhutHocTichLuy"]);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TienDoHocDAL.LayTongKetTienDo] Lỗi đọc HoSoHocTap: {ex.Message}");
            }

            // 2. Lấy số phút học từ bảng LichSuHocTap
            try
            {
                var paramsLS = new List<SqlParameter> { new SqlParameter("@MaND", maNguoiDung) };
                string dateWhereLS = "";
                if (batDau.HasValue)
                {
                    dateWhereLS = "AND NgayGhiNhan >= @BatDau";
                    paramsLS.Add(new SqlParameter("@BatDau", batDau.Value));
                }

                string queryLS = $@"
                    SELECT ISNULL(SUM(SoPhutHoc), 0) AS TongPhut
                    FROM LichSuHocTap
                    WHERE MaNguoiDung = @MaND {dateWhereLS}";

                object objPhut = SqlDataProvider.Instance.ExecuteScalar(queryLS, paramsLS.ToArray());
                int phutLichSu = objPhut != null && objPhut != DBNull.Value ? Convert.ToInt32(objPhut) : 0;

                if (filter == "TatCa")
                {
                    result.TongSoPhutHoc = Math.Max(result.TongSoPhutHoc, phutLichSu);
                }
                else
                {
                    result.TongSoPhutHoc = phutLichSu;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TienDoHocDAL.LayTongKetTienDo] Lỗi đọc LichSuHocTap: {ex.Message}");
            }

            // 3. Lấy số liệu bài thi từ LuotLamBai
            try
            {
                var paramsThi = new List<SqlParameter> { new SqlParameter("@MaND", maNguoiDung) };
                string dateWhereThi = "";
                if (batDau.HasValue)
                {
                    dateWhereThi = "AND (lb.ThoiDiemNopBai >= @BatDau OR (lb.ThoiDiemNopBai IS NULL AND lb.ThoiDiemBatDau >= @BatDau))";
                    paramsThi.Add(new SqlParameter("@BatDau", batDau.Value));
                }

                string queryThi = $@"
                    SELECT 
                        COUNT(*) AS TongSoBai,
                        ISNULL(AVG(CAST(lb.DiemSo AS FLOAT)), 0.0) AS DiemTB,
                        ISNULL(SUM(CASE WHEN lb.DiemSo >= ISNULL(dt.DiemDat, 5.0) THEN 1 ELSE 0 END), 0) AS SoBaiDat,
                        ISNULL(SUM(CASE WHEN lb.DiemSo < ISNULL(dt.DiemDat, 5.0) THEN 1 ELSE 0 END), 0) AS SoBaiChuaDat
                    FROM LuotLamBai lb
                    LEFT JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                    WHERE lb.MaNguoiDung = @MaND 
                      AND lb.TrangThai = 'DaNop' {dateWhereThi}";

                DataTable dtThi = SqlDataProvider.Instance.ExecuteQuery(queryThi, paramsThi.ToArray());
                if (dtThi.Rows.Count > 0)
                {
                    DataRow r = dtThi.Rows[0];
                    result.TongSoBaiThi = Convert.ToInt32(r["TongSoBai"]);
                    result.DiemTrungBinh = Math.Round(Convert.ToDouble(r["DiemTB"]), 1);
                    result.SoBaiDat = Convert.ToInt32(r["SoBaiDat"]);
                    result.SoBaiChuaDat = Convert.ToInt32(r["SoBaiChuaDat"]);

                    if (result.TongSoBaiThi > 0)
                    {
                        result.TyLeDat = Math.Round((result.SoBaiDat * 100.0) / result.TongSoBaiThi, 1);
                        result.TyLeChuaDat = Math.Round(100.0 - result.TyLeDat, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TienDoHocDAL.LayTongKetTienDo] Lỗi đọc LuotLamBai: {ex.Message}");
            }

            // 4. Tính toán tiến độ mục tiêu
            TinhTienDoMucTieu(result);

            return result;
        }

        /// <summary>
        /// Lấy danh sách điểm số theo từng lần thi (sắp xếp tăng dần theo thời gian để vẽ Line Chart)
        /// </summary>
        public List<DiemThiTheoThoiGianDTO> LayLichSuDiemThi(int maNguoiDung, string filter)
        {
            var list = new List<DiemThiTheoThoiGianDTO>();
            DateTime? batDau = TinhNgayBatDau(filter);

            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@MaND", maNguoiDung) };
                string dateWhere = "";
                if (batDau.HasValue)
                {
                    dateWhere = "AND (lb.ThoiDiemNopBai >= @BatDau OR (lb.ThoiDiemNopBai IS NULL AND lb.ThoiDiemBatDau >= @BatDau))";
                    parameters.Add(new SqlParameter("@BatDau", batDau.Value));
                }

                string query = $@"
                    SELECT 
                        lb.MaLuotLamBai,
                        lb.MaBaiTapDeThi,
                        ISNULL(dt.TenBaiTapDeThi, N'Bài thi chuẩn hóa') AS TenDeThi,
                        lb.DiemSo,
                        ISNULL(dt.DiemDat, 5.0) AS DiemDat,
                        ISNULL(dt.TongDiem, 10.0) AS TongDiem,
                        ISNULL(lb.ThoiDiemNopBai, lb.ThoiDiemBatDau) AS ThoiDiemNopBai,
                        lb.HinhThucLam,
                        lb.ThoiGianLamGiay,
                        lb.TongSoCau,
                        lb.SoCauDung
                    FROM LuotLamBai lb
                    LEFT JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                    WHERE lb.MaNguoiDung = @MaND 
                      AND lb.TrangThai = 'DaNop' {dateWhere}
                    ORDER BY ISNULL(lb.ThoiDiemNopBai, lb.ThoiDiemBatDau) ASC, lb.MaLuotLamBai ASC";

                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters.ToArray());
                int stt = 1;
                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new DiemThiTheoThoiGianDTO
                    {
                        MaLuotLamBai = Convert.ToInt32(r["MaLuotLamBai"]),
                        MaBaiTapDeThi = Convert.ToInt32(r["MaBaiTapDeThi"]),
                        TenDeThi = r["TenDeThi"]?.ToString() ?? "Bài thi",
                        DiemSo = Convert.ToDecimal(r["DiemSo"]),
                        DiemDat = Convert.ToDecimal(r["DiemDat"]),
                        TongDiem = Convert.ToDecimal(r["TongDiem"]),
                        ThoiDiemNopBai = Convert.ToDateTime(r["ThoiDiemNopBai"]),
                        HinhThucLam = r["HinhThucLam"]?.ToString() ?? "ThiThu",
                        ThoiGianLamGiay = Convert.ToInt32(r["ThoiGianLamGiay"]),
                        TongSoCau = Convert.ToInt32(r["TongSoCau"]),
                        SoCauDung = Convert.ToInt32(r["SoCauDung"]),
                        ThuTuLanThi = stt++
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TienDoHocDAL.LayLichSuDiemThi] Lỗi: {ex.Message}");
            }

            return list;
        }

        /// <summary>
        /// Lấy thống kê số giờ/phút học phân bổ theo ngày cho biểu đồ cột
        /// </summary>
        public List<GioHocTheoNgayDTO> LayThongKeGioHocTheoNgay(int maNguoiDung, string filter)
        {
            var list = new List<GioHocTheoNgayDTO>();
            DateTime today = DateTime.Today;

            // Xác định số ngày cần hiển thị trên biểu đồ
            int soNgay = (filter == "ThangNay") ? 14 : 7;
            DateTime batDau = today.AddDays(-(soNgay - 1));

            // Khởi tạo khung ngày
            var dictNgay = new Dictionary<string, GioHocTheoNgayDTO>();
            for (int i = 0; i < soNgay; i++)
            {
                DateTime d = batDau.AddDays(i);
                string key = d.ToString("yyyy-MM-dd");
                string nhan = DinhDangThuTrongTuan(d);
                dictNgay[key] = new GioHocTheoNgayDTO
                {
                    Ngay = d,
                    NhanNgay = nhan,
                    SoPhutHoc = 0
                };
            }

            try
            {
                string query = @"
                    SELECT 
                        CONVERT(VARCHAR(10), NgayGhiNhan, 120) AS NgayStr,
                        SUM(SoPhutHoc) AS TongPhut
                    FROM LichSuHocTap
                    WHERE MaNguoiDung = @MaND 
                      AND NgayGhiNhan >= @BatDau
                    GROUP BY CONVERT(VARCHAR(10), NgayGhiNhan, 120)";

                SqlParameter[] pars = {
                    new SqlParameter("@MaND", maNguoiDung),
                    new SqlParameter("@BatDau", batDau)
                };

                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, pars);
                foreach (DataRow r in dt.Rows)
                {
                    string k = r["NgayStr"]?.ToString() ?? "";
                    if (dictNgay.ContainsKey(k))
                    {
                        dictNgay[k].SoPhutHoc = Convert.ToInt32(r["TongPhut"]);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TienDoHocDAL.LayThongKeGioHocTheoNgay] Lỗi: {ex.Message}");
            }

            list.AddRange(dictNgay.Values);
            return list;
        }

        // --- CÁC HÀM TIỆN ÍCH HỖ TRỢ ---

        private static DateTime? TinhNgayBatDau(string filter)
        {
            DateTime now = DateTime.Now;
            return filter switch
            {
                "TuanNay" => DateTime.Today.AddDays(-7),
                "ThangNay" => DateTime.Today.AddDays(-30),
                _ => null
            };
        }

        private static string DinhDangThuTrongTuan(DateTime date)
        {
            string dayOfWeek = date.DayOfWeek switch
            {
                DayOfWeek.Monday => "T2",
                DayOfWeek.Tuesday => "T3",
                DayOfWeek.Wednesday => "T4",
                DayOfWeek.Thursday => "T5",
                DayOfWeek.Friday => "T6",
                DayOfWeek.Saturday => "T7",
                DayOfWeek.Sunday => "CN",
                _ => ""
            };
            return $"{dayOfWeek} ({date:dd/MM})";
        }

        private static void TinhTienDoMucTieu(TongKetTienDoDTO result)
        {
            if (result.DiemMucTieu.HasValue && result.DiemMucTieu.Value > 0)
            {
                double target = (double)result.DiemMucTieu.Value;
                if (result.KhungNangLucMucTieu == "IELTS")
                {
                    // Scale IELTS: 0 - 9
                    if (result.TongSoBaiThi > 0)
                    {
                        double scoreInIelts = (result.DiemTrungBinh / 10.0) * 9.0;
                        result.PhanTramMucTieu = Math.Min(100.0, Math.Max(0.0, Math.Round((scoreInIelts / target) * 100.0, 0)));
                    }
                    else
                    {
                        result.PhanTramMucTieu = 0;
                    }
                }
                else if (result.KhungNangLucMucTieu == "TOEIC")
                {
                    // Scale TOEIC: target ví dụ 650
                    if (result.TongSoBaiThi > 0)
                    {
                        double scoreInToeic = (result.DiemTrungBinh / 10.0) * 990.0;
                        result.PhanTramMucTieu = Math.Min(100.0, Math.Max(0.0, Math.Round((scoreInToeic / target) * 100.0, 0)));
                    }
                    else
                    {
                        result.PhanTramMucTieu = 0;
                    }
                }
                else
                {
                    if (result.TongSoBaiThi > 0)
                    {
                        result.PhanTramMucTieu = Math.Min(100.0, Math.Max(0.0, Math.Round((result.DiemTrungBinh / target) * 100.0, 0)));
                    }
                    else
                    {
                        result.PhanTramMucTieu = 0;
                    }
                }
                result.MoTaTienDoMucTieu = $"Đạt {result.PhanTramMucTieu}% so với mục tiêu {result.KhungNangLucMucTieu} {result.DiemMucTieu:F1}";
            }
            else
            {
                // Chưa đặt điểm mục tiêu cụ thể
                if (result.TongSoBaiThi > 0)
                {
                    result.PhanTramMucTieu = Math.Min(100.0, Math.Max(0.0, Math.Round(result.DiemTrungBinh * 10.0, 0)));
                    result.MoTaTienDoMucTieu = $"Điểm trung bình {result.DiemTrungBinh:0.0}/10 (Tương đương {result.PhanTramMucTieu}%)";
                }
                else
                {
                    result.PhanTramMucTieu = 0;
                    result.MoTaTienDoMucTieu = "Chưa có bài thi để đo lường tiến độ mục tiêu";
                }
            }
        }
    }
}

