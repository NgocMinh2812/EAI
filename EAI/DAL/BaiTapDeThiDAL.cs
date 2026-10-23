using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class BaiTapDeThiDAL
    {
        private static BaiTapDeThiDAL? _instance;
        public static BaiTapDeThiDAL Instance => _instance ??= new BaiTapDeThiDAL();

        /// <summary>
        /// Lấy danh sách đề thi hỗ trợ bộ lọc và quyền hạn (Sinh viên chỉ lấy đề Đã xuất bản)
        /// </summary>
        public List<BaiTapDeThiDTO> LayDanhSachDeThi(
            string? loaiHinh = null, 
            string? trinhDo = null, 
            string? trangThai = null, 
            string? tuKhoa = null, 
            bool chiLayDaXuatBan = false)
        {
            var list = new List<BaiTapDeThiDTO>();
            string query = @"
                SELECT dt.MaBaiTapDeThi, dt.MaBaiHoc, dt.MaChuDe, dt.MaTrinhDo,
                       dt.TenBaiTapDeThi, dt.LoaiHinh, dt.MoTa, dt.ThoiGianLamBaiPhut, dt.DaPhatHanh,
                       dt.DiemDat, dt.TongDiem, dt.TongSoCau, dt.XaoTronCauHoi,
                       ISNULL(dt.ChoXemDapAn, 1) AS ChoXemDapAn,
                       ISNULL(dt.ChoXemLoiGiai, 1) AS ChoXemLoiGiai,
                       ISNULL(dt.SoLuotLamToiDa, 0) AS SoLuotLamToiDa,
                       ISNULL(dt.TrangThai, CASE WHEN dt.DaPhatHanh = 1 THEN N'DaXuatBan' ELSE N'BanNhap' END) AS TrangThai,
                       dt.NguoiTao, nd.HoTen AS TenNguoiTao, dt.NgayTao,
                       dt.DuongDanAudio, ISNULL(dt.SoLanPhatAudio, 0) AS SoLanPhatAudio,
                       td.MaCapDo, td.TenTrinhDo, cd.TenChuDe,
                       (SELECT COUNT(*) FROM CauHoiBaiTap chbt WHERE chbt.MaBaiTapDeThi = dt.MaBaiTapDeThi) AS SoLuongCauHoi,
                       (SELECT COUNT(*) FROM LuotLamBai lb WHERE lb.MaBaiTapDeThi = dt.MaBaiTapDeThi) AS SoLuotDaThi
                FROM BaiTapDeThi dt
                LEFT JOIN TrinhDo td ON dt.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON dt.MaChuDe = cd.MaChuDe
                LEFT JOIN NguoiDung nd ON dt.NguoiTao = nd.MaNguoiDung
                WHERE (@LoaiHinh IS NULL OR @LoaiHinh = 'All' OR dt.LoaiHinh = @LoaiHinh)
                  AND (@TrinhDo IS NULL OR @TrinhDo = 'All' OR td.MaCapDo = @TrinhDo)
                  AND (@TrangThai IS NULL OR @TrangThai = 'All' OR ISNULL(dt.TrangThai, CASE WHEN dt.DaPhatHanh = 1 THEN N'DaXuatBan' ELSE N'BanNhap' END) = @TrangThai)
                  AND (@TuKhoa IS NULL OR @TuKhoa = '' OR dt.TenBaiTapDeThi LIKE N'%' + @TuKhoa + N'%' OR dt.MoTa LIKE N'%' + @TuKhoa + N'%')
                  AND (@ChiLayDaXuatBan = 0 OR dt.DaPhatHanh = 1 OR dt.TrangThai = N'DaXuatBan')
                ORDER BY dt.NgayTao DESC, dt.MaBaiTapDeThi DESC";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@LoaiHinh", string.IsNullOrWhiteSpace(loaiHinh) ? DBNull.Value : (object)loaiHinh.Trim()),
                new SqlParameter("@TrinhDo", string.IsNullOrWhiteSpace(trinhDo) ? DBNull.Value : (object)trinhDo.Trim()),
                new SqlParameter("@TrangThai", string.IsNullOrWhiteSpace(trangThai) ? DBNull.Value : (object)trangThai.Trim()),
                new SqlParameter("@TuKhoa", string.IsNullOrWhiteSpace(tuKhoa) ? DBNull.Value : (object)tuKhoa.Trim()),
                new SqlParameter("@ChiLayDaXuatBan", chiLayDaXuatBan ? 1 : 0)
            };

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
                foreach (DataRow row in dt.Rows)
                {
                    int soCau = Convert.ToInt32(row["SoLuongCauHoi"]);
                    int tongSoCauCfg = row["TongSoCau"] == DBNull.Value ? 0 : Convert.ToInt32(row["TongSoCau"]);
                    if (soCau == 0 && tongSoCauCfg > 0) soCau = tongSoCauCfg;

                    list.Add(new BaiTapDeThiDTO
                    {
                        MaBaiTapDeThi = Convert.ToInt32(row["MaBaiTapDeThi"]),
                        MaBaiHoc = row["MaBaiHoc"] == DBNull.Value ? null : Convert.ToInt32(row["MaBaiHoc"]),
                        MaChuDe = row["MaChuDe"] == DBNull.Value ? null : Convert.ToInt32(row["MaChuDe"]),
                        MaTrinhDo = row["MaTrinhDo"] == DBNull.Value ? null : Convert.ToInt32(row["MaTrinhDo"]),
                        TenBaiTapDeThi = row["TenBaiTapDeThi"]?.ToString() ?? "",
                        LoaiHinh = row["LoaiHinh"]?.ToString() ?? "ThiThu",
                        MoTa = row["MoTa"]?.ToString() ?? "",
                        ThoiGianLamBaiPhut = row["ThoiGianLamBaiPhut"] == DBNull.Value ? 30 : Convert.ToInt32(row["ThoiGianLamBaiPhut"]),
                        DaPhatHanh = row["DaPhatHanh"] != DBNull.Value && Convert.ToBoolean(row["DaPhatHanh"]),
                        DiemDat = row["DiemDat"] == DBNull.Value ? 5.0 : Convert.ToDouble(row["DiemDat"]),
                        TongDiem = row["TongDiem"] == DBNull.Value ? 10.0 : Convert.ToDouble(row["TongDiem"]),
                        XaoTronCauHoi = row["XaoTronCauHoi"] == DBNull.Value || Convert.ToBoolean(row["XaoTronCauHoi"]),
                        ChoXemDapAn = row["ChoXemDapAn"] == DBNull.Value || Convert.ToBoolean(row["ChoXemDapAn"]),
                        ChoXemLoiGiai = row["ChoXemLoiGiai"] == DBNull.Value || Convert.ToBoolean(row["ChoXemLoiGiai"]),
                        SoLuotLamToiDa = row["SoLuotLamToiDa"] == DBNull.Value ? 0 : Convert.ToInt32(row["SoLuotLamToiDa"]),
                        TrangThai = row["TrangThai"]?.ToString() ?? "BanNhap",
                        DuongDanAudio = row.Table.Columns.Contains("DuongDanAudio") && row["DuongDanAudio"] != DBNull.Value ? row["DuongDanAudio"]?.ToString() : null,
                        SoLanPhatAudio = row.Table.Columns.Contains("SoLanPhatAudio") && row["SoLanPhatAudio"] != DBNull.Value ? Convert.ToInt32(row["SoLanPhatAudio"]) : 0,
                        NguoiTaoId = row["NguoiTao"] == DBNull.Value ? null : Convert.ToInt32(row["NguoiTao"]),
                        TenNguoiTao = row["TenNguoiTao"] == DBNull.Value ? "Hệ thống" : row["TenNguoiTao"]?.ToString(),
                        NgayTao = row["NgayTao"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["NgayTao"]),
                        MaCapDo = row["MaCapDo"]?.ToString() ?? "B2",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "Trung cấp",
                        TenChuDe = row["TenChuDe"]?.ToString() ?? "Học thuật",
                        SoLuongCauHoi = soCau,
                        SoLuotDaThi = Convert.ToInt32(row["SoLuotDaThi"])
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lấy danh sách đề: {ex.Message}");
            }

            return list;
        }

        public BaiTapDeThiDTO? LayChiTietDeThi(int maDeThi)
        {
            string query = @"
                SELECT dt.MaBaiTapDeThi, dt.MaBaiHoc, dt.MaChuDe, dt.MaTrinhDo,
                       dt.TenBaiTapDeThi, dt.LoaiHinh, dt.MoTa, dt.ThoiGianLamBaiPhut, dt.DaPhatHanh,
                       dt.DiemDat, dt.TongDiem, dt.TongSoCau, dt.XaoTronCauHoi,
                       ISNULL(dt.ChoXemDapAn, 1) AS ChoXemDapAn,
                       ISNULL(dt.ChoXemLoiGiai, 1) AS ChoXemLoiGiai,
                       ISNULL(dt.SoLuotLamToiDa, 0) AS SoLuotLamToiDa,
                       ISNULL(dt.TrangThai, CASE WHEN dt.DaPhatHanh = 1 THEN N'DaXuatBan' ELSE N'BanNhap' END) AS TrangThai,
                       dt.NguoiTao, nd.HoTen AS TenNguoiTao, dt.NgayTao,
                       dt.DuongDanAudio, ISNULL(dt.SoLanPhatAudio, 0) AS SoLanPhatAudio,
                       td.MaCapDo, td.TenTrinhDo, cd.TenChuDe,
                       (SELECT COUNT(*) FROM CauHoiBaiTap chbt WHERE chbt.MaBaiTapDeThi = dt.MaBaiTapDeThi) AS SoLuongCauHoi,
                       (SELECT COUNT(*) FROM LuotLamBai lb WHERE lb.MaBaiTapDeThi = dt.MaBaiTapDeThi) AS SoLuotDaThi
                FROM BaiTapDeThi dt
                LEFT JOIN TrinhDo td ON dt.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON dt.MaChuDe = cd.MaChuDe
                LEFT JOIN NguoiDung nd ON dt.NguoiTao = nd.MaNguoiDung
                WHERE dt.MaBaiTapDeThi = @MaDe";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaDe", maDeThi) });
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    int soCau = Convert.ToInt32(row["SoLuongCauHoi"]);
                    int tongSoCauCfg = row["TongSoCau"] == DBNull.Value ? 0 : Convert.ToInt32(row["TongSoCau"]);
                    if (soCau == 0 && tongSoCauCfg > 0) soCau = tongSoCauCfg;

                    return new BaiTapDeThiDTO
                    {
                        MaBaiTapDeThi = Convert.ToInt32(row["MaBaiTapDeThi"]),
                        MaBaiHoc = row["MaBaiHoc"] == DBNull.Value ? null : Convert.ToInt32(row["MaBaiHoc"]),
                        MaChuDe = row["MaChuDe"] == DBNull.Value ? null : Convert.ToInt32(row["MaChuDe"]),
                        MaTrinhDo = row["MaTrinhDo"] == DBNull.Value ? null : Convert.ToInt32(row["MaTrinhDo"]),
                        TenBaiTapDeThi = row["TenBaiTapDeThi"]?.ToString() ?? "",
                        LoaiHinh = row["LoaiHinh"]?.ToString() ?? "ThiThu",
                        MoTa = row["MoTa"]?.ToString() ?? "",
                        ThoiGianLamBaiPhut = row["ThoiGianLamBaiPhut"] == DBNull.Value ? 30 : Convert.ToInt32(row["ThoiGianLamBaiPhut"]),
                        DaPhatHanh = row["DaPhatHanh"] != DBNull.Value && Convert.ToBoolean(row["DaPhatHanh"]),
                        DiemDat = row["DiemDat"] == DBNull.Value ? 5.0 : Convert.ToDouble(row["DiemDat"]),
                        TongDiem = row["TongDiem"] == DBNull.Value ? 10.0 : Convert.ToDouble(row["TongDiem"]),
                        XaoTronCauHoi = row["XaoTronCauHoi"] == DBNull.Value || Convert.ToBoolean(row["XaoTronCauHoi"]),
                        ChoXemDapAn = row["ChoXemDapAn"] == DBNull.Value || Convert.ToBoolean(row["ChoXemDapAn"]),
                        ChoXemLoiGiai = row["ChoXemLoiGiai"] == DBNull.Value || Convert.ToBoolean(row["ChoXemLoiGiai"]),
                        SoLuotLamToiDa = row["SoLuotLamToiDa"] == DBNull.Value ? 0 : Convert.ToInt32(row["SoLuotLamToiDa"]),
                        TrangThai = row["TrangThai"]?.ToString() ?? "BanNhap",
                        DuongDanAudio = row.Table.Columns.Contains("DuongDanAudio") && row["DuongDanAudio"] != DBNull.Value ? row["DuongDanAudio"]?.ToString() : null,
                        SoLanPhatAudio = row.Table.Columns.Contains("SoLanPhatAudio") && row["SoLanPhatAudio"] != DBNull.Value ? Convert.ToInt32(row["SoLanPhatAudio"]) : 0,
                        NguoiTaoId = row["NguoiTao"] == DBNull.Value ? null : Convert.ToInt32(row["NguoiTao"]),
                        TenNguoiTao = row["TenNguoiTao"] == DBNull.Value ? "Hệ thống" : row["TenNguoiTao"]?.ToString(),
                        NgayTao = row["NgayTao"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["NgayTao"]),
                        MaCapDo = row["MaCapDo"]?.ToString() ?? "B2",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "Trung cấp",
                        TenChuDe = row["TenChuDe"]?.ToString() ?? "Học thuật",
                        SoLuongCauHoi = soCau,
                        SoLuotDaThi = Convert.ToInt32(row["SoLuotDaThi"])
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lấy chi tiết đề: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Lấy danh sách câu hỏi của đề thi, ưu tiên đọc từ bản chụp (SNAPSHOT) đã bảo toàn để không bị ảnh hưởng khi ngân hàng câu hỏi bị sửa
        /// </summary>
        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiCuaDeThi(int maDeThi)
        {
            var list = new List<CauHoiKhaoSatDTO>();

            string query = @"
                SELECT chbt.MaCauHoiBaiTap, chbt.MaBaiTapDeThi, chbt.MaCauHoi, chbt.ThuTuCauHoi, chbt.DiemSo,
                       chbt.NoiDungCauHoiSnapshot, chbt.DoanVanDocSnapshot, chbt.GiaiThichDapAnSnapshot,
                       chbt.AudioPathSnapshot, chbt.LoaiCauHoiSnapshot, chbt.KyNangSnapshot, chbt.DoKhoSnapshot,
                       chbt.DanhSachLuaChonJson,
                       ch.NoiDungCauHoi AS NoiDungLive, ch.DoanVanDoc AS DoanVanLive, ch.GiaiThichDapAn AS GiaiThichLive,
                       ch.DuongDanAudio AS AudioLive, ch.LoaiCauHoi AS LoaiCauHoiLive, ch.KyNang AS KyNangLive, ch.DoKho AS DoKhoLive,
                       td.MaCapDo AS MaTrinhDo, td.TenTrinhDo
                FROM CauHoiBaiTap chbt
                LEFT JOIN CauHoi ch ON chbt.MaCauHoi = ch.MaCauHoi
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                WHERE chbt.MaBaiTapDeThi = @MaDe
                ORDER BY chbt.ThuTuCauHoi ASC";

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaDe", maDeThi) });

            if (dt.Rows.Count == 0)
            {
                // Fallback nếu đề thi chưa gắn câu trong CauHoiBaiTap: lấy 10 câu trắc nghiệm từ CauHoi
                string fallbackQuery = @"
                    SELECT TOP 10 ch.MaCauHoi, ROW_NUMBER() OVER(ORDER BY ch.MaCauHoi ASC) AS ThuTuCauHoi,
                           ch.KyNang, td.MaCapDo AS MaTrinhDo, td.TenTrinhDo, ch.NoiDungCauHoi,
                           ch.DoanVanDoc, ch.GiaiThichDapAn,
                           ch.LoaiCauHoi, ch.DoKho, ch.DuongDanAudio AS AudioPath
                    FROM CauHoi ch
                    LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                    WHERE ch.DaKiemDuyet = 1
                    ORDER BY ch.MaCauHoi ASC";
                dt = SqlDataProvider.Instance.ExecuteQuery(fallbackQuery);
            }

            int thuTu = 1;
            var needLiveOptions = new List<CauHoiKhaoSatDTO>();

            foreach (DataRow row in dt.Rows)
            {
                int maCH = Convert.ToInt32(row["MaCauHoi"]);
                double diem = row.Table.Columns.Contains("DiemSo") && row["DiemSo"] != DBNull.Value 
                    ? Convert.ToDouble(row["DiemSo"]) 
                    : 1.0;

                // Kiểm tra xem đã có bản chụp Snapshot hay chưa (Yêu cầu 3.4)
                string? noiDungSnap = row.Table.Columns.Contains("NoiDungCauHoiSnapshot") && row["NoiDungCauHoiSnapshot"] != DBNull.Value
                    ? row["NoiDungCauHoiSnapshot"]?.ToString()
                    : null;

                string noiDungFinal = !string.IsNullOrWhiteSpace(noiDungSnap)
                    ? noiDungSnap
                    : (row.Table.Columns.Contains("NoiDungLive") && row["NoiDungLive"] != DBNull.Value ? row["NoiDungLive"]?.ToString() ?? "" : row["NoiDungCauHoi"]?.ToString() ?? "");

                string? doanVanFinal = row.Table.Columns.Contains("DoanVanDocSnapshot") && row["DoanVanDocSnapshot"] != DBNull.Value
                    ? row["DoanVanDocSnapshot"]?.ToString()
                    : (row.Table.Columns.Contains("DoanVanLive") && row["DoanVanLive"] != DBNull.Value ? row["DoanVanLive"]?.ToString() : (row.Table.Columns.Contains("DoanVanDoc") && row["DoanVanDoc"] != DBNull.Value ? row["DoanVanDoc"]?.ToString() : null));

                string? giaiThichFinal = row.Table.Columns.Contains("GiaiThichDapAnSnapshot") && row["GiaiThichDapAnSnapshot"] != DBNull.Value
                    ? row["GiaiThichDapAnSnapshot"]?.ToString()
                    : (row.Table.Columns.Contains("GiaiThichLive") && row["GiaiThichLive"] != DBNull.Value ? row["GiaiThichLive"]?.ToString() : (row.Table.Columns.Contains("GiaiThichDapAn") && row["GiaiThichDapAn"] != DBNull.Value ? row["GiaiThichDapAn"]?.ToString() : null));

                string? audioFinal = row.Table.Columns.Contains("AudioPathSnapshot") && row["AudioPathSnapshot"] != DBNull.Value
                    ? row["AudioPathSnapshot"]?.ToString()
                    : (row.Table.Columns.Contains("AudioLive") && row["AudioLive"] != DBNull.Value ? row["AudioLive"]?.ToString() : (row.Table.Columns.Contains("AudioPath") && row["AudioPath"] != DBNull.Value ? row["AudioPath"]?.ToString() : null));

                string loaiCHFinal = row.Table.Columns.Contains("LoaiCauHoiSnapshot") && row["LoaiCauHoiSnapshot"] != DBNull.Value
                    ? row["LoaiCauHoiSnapshot"]?.ToString() ?? "TracNghiem"
                    : (row.Table.Columns.Contains("LoaiCauHoiLive") && row["LoaiCauHoiLive"] != DBNull.Value ? row["LoaiCauHoiLive"]?.ToString() ?? "TracNghiem" : (row.Table.Columns.Contains("LoaiCauHoi") && row["LoaiCauHoi"] != DBNull.Value ? row["LoaiCauHoi"]?.ToString() ?? "TracNghiem" : "TracNghiem"));

                string kyNangFinal = row.Table.Columns.Contains("KyNangSnapshot") && row["KyNangSnapshot"] != DBNull.Value
                    ? row["KyNangSnapshot"]?.ToString() ?? "Doc"
                    : (row.Table.Columns.Contains("KyNangLive") && row["KyNangLive"] != DBNull.Value ? row["KyNangLive"]?.ToString() ?? "Doc" : (row.Table.Columns.Contains("KyNang") && row["KyNang"] != DBNull.Value ? row["KyNang"]?.ToString() ?? "Doc" : "Doc"));

                string doKhoFinal = row.Table.Columns.Contains("DoKhoSnapshot") && row["DoKhoSnapshot"] != DBNull.Value
                    ? row["DoKhoSnapshot"]?.ToString() ?? "TrungBinh"
                    : (row.Table.Columns.Contains("DoKhoLive") && row["DoKhoLive"] != DBNull.Value ? row["DoKhoLive"]?.ToString() ?? "TrungBinh" : (row.Table.Columns.Contains("DoKho") && row["DoKho"] != DBNull.Value ? row["DoKho"]?.ToString() ?? "TrungBinh" : "TrungBinh"));

                var q = new CauHoiKhaoSatDTO
                {
                    MaCauHoi = maCH,
                    ThuTuCauHoi = thuTu++,
                    KyNang = kyNangFinal,
                    MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B2",
                    TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                    NoiDungCauHoi = noiDungFinal,
                    DoanVanDoc = doanVanFinal,
                    GiaiThichDapAn = giaiThichFinal,
                    LoaiCauHoi = loaiCHFinal,
                    DoKho = doKhoFinal,
                    AudioPath = audioFinal,
                    Diem = diem
                };

                // Phục hồi danh sách lựa chọn từ Snapshot JSON nếu có
                string? jsonOptions = row.Table.Columns.Contains("DanhSachLuaChonJson") && row["DanhSachLuaChonJson"] != DBNull.Value
                    ? row["DanhSachLuaChonJson"]?.ToString()
                    : null;

                if (!string.IsNullOrWhiteSpace(jsonOptions))
                {
                    try
                    {
                        var deserializedOpts = JsonSerializer.Deserialize<List<LuaChonDapAnDTO>>(jsonOptions);
                        if (deserializedOpts != null && deserializedOpts.Count > 0)
                        {
                            q.DanhSachLuaChon.AddRange(deserializedOpts);
                        }
                    }
                    catch
                    {
                        // Fallback nạp live nếu json lỗi
                        needLiveOptions.Add(q);
                    }
                }
                else
                {
                    needLiveOptions.Add(q);
                }

                list.Add(q);
            }

            // Nạp live cho những câu chưa có snapshot lựa chọn
            if (needLiveOptions.Count > 0)
            {
                CauHoiDAL.NapDanhSachLuaChon(needLiveOptions);
            }

            return list;
        }

        /// <summary>
        /// [US-GV07 & Yêu cầu 3.4] Lưu cấu hình đề thi và danh sách câu hỏi kèm BẢN CHỤP SNAPSHOT an toàn trong SqlTransaction
        /// </summary>
        public bool LuuDeThiVaCauHoi(
            BaiTapDeThiDTO deThi, 
            List<(int MaCauHoi, int ThuTu, double Diem)> dsCauHoi, 
            int nguoiTaoId, 
            out string errorMessage)
        {
            errorMessage = string.Empty;
            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                int maDeThi = deThi.MaBaiTapDeThi;
                int tongSoCau = dsCauHoi.Count;
                string trangThai = string.IsNullOrWhiteSpace(deThi.TrangThai)
                    ? (deThi.DaPhatHanh ? "DaXuatBan" : "BanNhap")
                    : deThi.TrangThai;

                // Tự động giải quyết MaTrinhDo từ MaCapDo (A1, A2, B1, B2, C1, C2) nếu chưa có MaTrinhDo
                if ((deThi.MaTrinhDo == null || deThi.MaTrinhDo <= 0) && !string.IsNullOrWhiteSpace(deThi.MaCapDo))
                {
                    try
                    {
                        using var cmdTd = new SqlCommand("SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = @CapDo", conn, trans);
                        cmdTd.Parameters.AddWithValue("@CapDo", deThi.MaCapDo.Trim());
                        var tdObj = cmdTd.ExecuteScalar();
                        if (tdObj != null && tdObj != DBNull.Value)
                        {
                            deThi.MaTrinhDo = Convert.ToInt32(tdObj);
                        }
                    }
                    catch { }
                }

                if (maDeThi <= 0)
                {
                    string sqlInsert = @"
                        INSERT INTO BaiTapDeThi (
                            TenBaiTapDeThi, LoaiHinh, MaTrinhDo, MaChuDe, ThoiGianLamBaiPhut,
                            DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh,
                            ChoXemDapAn, ChoXemLoiGiai, SoLuotLamToiDa, TrangThai, NguoiTao, NgayTao,
                            DuongDanAudio, SoLanPhatAudio)
                        VALUES (
                            @TenDe, @LoaiHinh, @MaTrinhDo, @MaChuDe, @ThoiGian, 
                            @DiemDat, @TongDiem, @TongSoCau, @XaoTron, @DaPhatHanh,
                            @ChoXemDapAn, @ChoXemLoiGiai, @SoLuotLamToiDa, @TrangThai, @NguoiTao, SYSDATETIME(),
                            @DuongDanAudio, @SoLanPhatAudio);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using var cmd = new SqlCommand(sqlInsert, conn, trans);
                    cmd.Parameters.AddWithValue("@TenDe", deThi.TenBaiTapDeThi ?? "");
                    cmd.Parameters.AddWithValue("@LoaiHinh", deThi.LoaiHinh ?? "ThiThu");
                    cmd.Parameters.AddWithValue("@MaTrinhDo", (object?)deThi.MaTrinhDo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaChuDe", (object?)deThi.MaChuDe ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ThoiGian", deThi.ThoiGianLamBaiPhut);
                    cmd.Parameters.AddWithValue("@DiemDat", deThi.DiemDat);
                    cmd.Parameters.AddWithValue("@TongDiem", deThi.TongDiem);
                    cmd.Parameters.AddWithValue("@TongSoCau", tongSoCau);
                    cmd.Parameters.AddWithValue("@XaoTron", deThi.XaoTronCauHoi);
                    cmd.Parameters.AddWithValue("@DaPhatHanh", deThi.DaPhatHanh);
                    cmd.Parameters.AddWithValue("@ChoXemDapAn", deThi.ChoXemDapAn);
                    cmd.Parameters.AddWithValue("@ChoXemLoiGiai", deThi.ChoXemLoiGiai);
                    cmd.Parameters.AddWithValue("@SoLuotLamToiDa", deThi.SoLuotLamToiDa);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                    cmd.Parameters.AddWithValue("@NguoiTao", nguoiTaoId > 0 ? (object)nguoiTaoId : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DuongDanAudio", string.IsNullOrWhiteSpace(deThi.DuongDanAudio) ? DBNull.Value : (object)deThi.DuongDanAudio);
                    cmd.Parameters.AddWithValue("@SoLanPhatAudio", deThi.SoLanPhatAudio);

                    maDeThi = (int)cmd.ExecuteScalar();
                    deThi.MaBaiTapDeThi = maDeThi;
                }
                else
                {
                    string sqlUpdate = @"
                        UPDATE BaiTapDeThi
                        SET TenBaiTapDeThi = @TenDe,
                            LoaiHinh = @LoaiHinh,
                            MaTrinhDo = @MaTrinhDo,
                            MaChuDe = @MaChuDe,
                            ThoiGianLamBaiPhut = @ThoiGian,
                            DiemDat = @DiemDat,
                            TongDiem = @TongDiem,
                            TongSoCau = @TongSoCau,
                            XaoTronCauHoi = @XaoTron,
                            DaPhatHanh = @DaPhatHanh,
                            ChoXemDapAn = @ChoXemDapAn,
                            ChoXemLoiGiai = @ChoXemLoiGiai,
                            SoLuotLamToiDa = @SoLuotLamToiDa,
                            TrangThai = @TrangThai,
                            DuongDanAudio = @DuongDanAudio,
                            SoLanPhatAudio = @SoLanPhatAudio
                        WHERE MaBaiTapDeThi = @MaDe;
                        DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = @MaDe;";

                    using var cmd = new SqlCommand(sqlUpdate, conn, trans);
                    cmd.Parameters.AddWithValue("@MaDe", maDeThi);
                    cmd.Parameters.AddWithValue("@TenDe", deThi.TenBaiTapDeThi ?? "");
                    cmd.Parameters.AddWithValue("@LoaiHinh", deThi.LoaiHinh ?? "ThiThu");
                    cmd.Parameters.AddWithValue("@MaTrinhDo", (object?)deThi.MaTrinhDo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaChuDe", (object?)deThi.MaChuDe ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ThoiGian", deThi.ThoiGianLamBaiPhut);
                    cmd.Parameters.AddWithValue("@DiemDat", deThi.DiemDat);
                    cmd.Parameters.AddWithValue("@TongDiem", deThi.TongDiem);
                    cmd.Parameters.AddWithValue("@TongSoCau", tongSoCau);
                    cmd.Parameters.AddWithValue("@XaoTron", deThi.XaoTronCauHoi);
                    cmd.Parameters.AddWithValue("@DaPhatHanh", deThi.DaPhatHanh);
                    cmd.Parameters.AddWithValue("@ChoXemDapAn", deThi.ChoXemDapAn);
                    cmd.Parameters.AddWithValue("@ChoXemLoiGiai", deThi.ChoXemLoiGiai);
                    cmd.Parameters.AddWithValue("@SoLuotLamToiDa", deThi.SoLuotLamToiDa);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                    cmd.Parameters.AddWithValue("@DuongDanAudio", string.IsNullOrWhiteSpace(deThi.DuongDanAudio) ? DBNull.Value : (object)deThi.DuongDanAudio);
                    cmd.Parameters.AddWithValue("@SoLanPhatAudio", deThi.SoLanPhatAudio);

                    cmd.ExecuteNonQuery();
                }

                // Gán danh sách câu hỏi vào bảng trung gian CauHoiBaiTap kèm BẢN CHỤP NỘI DUNG (SNAPSHOT)
                string sqlInsertDetail = @"
                    INSERT INTO CauHoiBaiTap (
                        MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo,
                        NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot,
                        AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
                    VALUES (
                        @MaDe, @MaCauHoi, @ThuTu, @DiemSo,
                        @NoiDungSnap, @DoanVanSnap, @GiaiThichSnap,
                        @AudioSnap, @LoaiCHSnap, @KyNangSnap, @DoKhoSnap, @OptionsJson);";

                foreach (var item in dsCauHoi)
                {
                    // Lấy dữ liệu nguồn của câu hỏi để chụp Snapshot
                    string qInfoSql = @"
                        SELECT ch.NoiDungCauHoi, ch.DoanVanDoc, ch.GiaiThichDapAn, ch.DuongDanAudio,
                               ch.LoaiCauHoi, ch.KyNang, ch.DoKho
                        FROM CauHoi ch WHERE ch.MaCauHoi = @MaCH";
                    using var cmdQ = new SqlCommand(qInfoSql, conn, trans);
                    cmdQ.Parameters.AddWithValue("@MaCH", item.MaCauHoi);
                    using var rdrQ = cmdQ.ExecuteReader();
                    string noiDung = "";
                    string? doanVan = null, giaiThich = null, audio = null, loaiCH = "TracNghiem", kyNang = "Doc", doKho = "TrungBinh";
                    if (rdrQ.Read())
                    {
                        noiDung = rdrQ["NoiDungCauHoi"]?.ToString() ?? "";
                        doanVan = rdrQ["DoanVanDoc"] == DBNull.Value ? null : rdrQ["DoanVanDoc"]?.ToString();
                        giaiThich = rdrQ["GiaiThichDapAn"] == DBNull.Value ? null : rdrQ["GiaiThichDapAn"]?.ToString();
                        audio = rdrQ["DuongDanAudio"] == DBNull.Value ? null : rdrQ["DuongDanAudio"]?.ToString();
                        loaiCH = rdrQ["LoaiCauHoi"]?.ToString() ?? "TracNghiem";
                        kyNang = rdrQ["KyNang"]?.ToString() ?? "Doc";
                        doKho = rdrQ["DoKho"]?.ToString() ?? "TrungBinh";
                    }
                    rdrQ.Close();

                    // Lấy toàn bộ lựa chọn để chụp Snapshot JSON
                    string optSql = "SELECT MaLuaChon, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung FROM LuaChonDapAn WHERE MaCauHoi = @MaCH ORDER BY KyHieuLuaChon ASC";
                    using var cmdOpt = new SqlCommand(optSql, conn, trans);
                    cmdOpt.Parameters.AddWithValue("@MaCH", item.MaCauHoi);
                    using var rdrOpt = cmdOpt.ExecuteReader();
                    var optList = new List<LuaChonDapAnDTO>();
                    while (rdrOpt.Read())
                    {
                        optList.Add(new LuaChonDapAnDTO
                        {
                            MaLuaChon = Convert.ToInt32(rdrOpt["MaLuaChon"]),
                            MaCauHoi = item.MaCauHoi,
                            KyHieuLuaChon = rdrOpt["KyHieuLuaChon"]?.ToString() ?? "",
                            NoiDungLuaChon = rdrOpt["NoiDungLuaChon"]?.ToString() ?? "",
                            LaDapAnDung = Convert.ToBoolean(rdrOpt["LaDapAnDung"])
                        });
                    }
                    rdrOpt.Close();

                    string jsonOpts = JsonSerializer.Serialize(optList);

                    using var cmdDetail = new SqlCommand(sqlInsertDetail, conn, trans);
                    cmdDetail.Parameters.AddWithValue("@MaDe", maDeThi);
                    cmdDetail.Parameters.AddWithValue("@MaCauHoi", item.MaCauHoi);
                    cmdDetail.Parameters.AddWithValue("@ThuTu", item.ThuTu);
                    cmdDetail.Parameters.AddWithValue("@DiemSo", (decimal)item.Diem);
                    cmdDetail.Parameters.AddWithValue("@NoiDungSnap", noiDung);
                    cmdDetail.Parameters.AddWithValue("@DoanVanSnap", (object?)doanVan ?? DBNull.Value);
                    cmdDetail.Parameters.AddWithValue("@GiaiThichSnap", (object?)giaiThich ?? DBNull.Value);
                    cmdDetail.Parameters.AddWithValue("@AudioSnap", (object?)audio ?? DBNull.Value);
                    cmdDetail.Parameters.AddWithValue("@LoaiCHSnap", loaiCH);
                    cmdDetail.Parameters.AddWithValue("@KyNangSnap", kyNang);
                    cmdDetail.Parameters.AddWithValue("@DoKhoSnap", doKho);
                    cmdDetail.Parameters.AddWithValue("@OptionsJson", (object?)jsonOpts ?? DBNull.Value);

                    cmdDetail.ExecuteNonQuery();
                }

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// [Yêu cầu 3.1] Xuất bản đề thi (Bản nháp -> Đã xuất bản)
        /// </summary>
        public bool XuatBanDeThi(int maDeThi, out string errorMessage)
        {
            errorMessage = string.Empty;
            string sql = @"
                UPDATE BaiTapDeThi
                SET DaPhatHanh = 1,
                    TrangThai = N'DaXuatBan'
                WHERE MaBaiTapDeThi = @MaDe";

            try
            {
                int affected = SqlDataProvider.Instance.ExecuteNonQuery(sql, new[] { new SqlParameter("@MaDe", maDeThi) });
                return affected > 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// [Yêu cầu 3.1] Đóng đề thi / Ngừng cho làm bài
        /// </summary>
        public bool DongDeThi(int maDeThi, out string errorMessage)
        {
            errorMessage = string.Empty;
            string sql = @"
                UPDATE BaiTapDeThi
                SET DaPhatHanh = 0,
                    TrangThai = N'DaDong'
                WHERE MaBaiTapDeThi = @MaDe";

            try
            {
                int affected = SqlDataProvider.Instance.ExecuteNonQuery(sql, new[] { new SqlParameter("@MaDe", maDeThi) });
                return affected > 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// [Yêu cầu 3.1] Nhân bản đề thi (Clone) thành một bản nháp mới
        /// </summary>
        public int NhanBanDeThi(int maDeThi, int nguoiTaoId, out string errorMessage)
        {
            errorMessage = string.Empty;
            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                string copyExamSql = @"
                    INSERT INTO BaiTapDeThi (
                        TenBaiTapDeThi, LoaiHinh, MaTrinhDo, MaChuDe, ThoiGianLamBaiPhut,
                        DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh,
                        ChoXemDapAn, ChoXemLoiGiai, SoLuotLamToiDa, TrangThai, NguoiTao, NgayTao)
                    SELECT N'[Bản sao] ' + TenBaiTapDeThi, LoaiHinh, MaTrinhDo, MaChuDe, ThoiGianLamBaiPhut,
                           DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, 0,
                           ChoXemDapAn, ChoXemLoiGiai, SoLuotLamToiDa, N'BanNhap', @NguoiTao, SYSDATETIME()
                    FROM BaiTapDeThi
                    WHERE MaBaiTapDeThi = @MaDeGoc;
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using var cmdCopy = new SqlCommand(copyExamSql, conn, trans);
                cmdCopy.Parameters.AddWithValue("@MaDeGoc", maDeThi);
                cmdCopy.Parameters.AddWithValue("@NguoiTao", nguoiTaoId > 0 ? (object)nguoiTaoId : DBNull.Value);

                int newExamId = (int)cmdCopy.ExecuteScalar();

                // Sao chép các câu hỏi và snapshot sang đề mới
                string copyQuestionsSql = @"
                    INSERT INTO CauHoiBaiTap (
                        MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo,
                        NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot,
                        AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson)
                    SELECT @NewExamId, MaCauHoi, ThuTuCauHoi, DiemSo,
                           NoiDungCauHoiSnapshot, DoanVanDocSnapshot, GiaiThichDapAnSnapshot,
                           AudioPathSnapshot, LoaiCauHoiSnapshot, KyNangSnapshot, DoKhoSnapshot, DanhSachLuaChonJson
                    FROM CauHoiBaiTap
                    WHERE MaBaiTapDeThi = @MaDeGoc;";

                using var cmdQuestions = new SqlCommand(copyQuestionsSql, conn, trans);
                cmdQuestions.Parameters.AddWithValue("@NewExamId", newExamId);
                cmdQuestions.Parameters.AddWithValue("@MaDeGoc", maDeThi);
                cmdQuestions.ExecuteNonQuery();

                trans.Commit();
                return newExamId;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                errorMessage = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// [Yêu cầu 3.1 & 5] Xóa đề an toàn: nếu đã có lượt thi thì từ chối xóa hoặc chuyển trạng thái Đã đóng
        /// </summary>
        public bool XoaDeThi(int maDeThi, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Kiểm tra xem đã có thí sinh làm bài hay chưa
            string checkSql = "SELECT COUNT(*) FROM LuotLamBai WHERE MaBaiTapDeThi = @MaDe";
            object? res = SqlDataProvider.Instance.ExecuteScalar(checkSql, new[] { new SqlParameter("@MaDe", maDeThi) });
            int attemptCount = (res != null && res != DBNull.Value) ? Convert.ToInt32(res) : 0;

            if (attemptCount > 0)
            {
                errorMessage = $"Đề thi này đã có {attemptCount} lượt làm bài của học viên. Để bảo toàn lịch sử kết quả, không thể xóa vĩnh viễn. Hệ thống đã chuyển đề thi sang trạng thái 'Đã đóng'.";
                DongDeThi(maDeThi, out _);
                return false;
            }

            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                // Xóa chi tiết câu hỏi của đề
                string deleteChSql = "DELETE FROM CauHoiBaiTap WHERE MaBaiTapDeThi = @MaDe";
                using var cmd1 = new SqlCommand(deleteChSql, conn, trans);
                cmd1.Parameters.AddWithValue("@MaDe", maDeThi);
                cmd1.ExecuteNonQuery();

                // Xóa đề thi
                string deleteDeSql = "DELETE FROM BaiTapDeThi WHERE MaBaiTapDeThi = @MaDe";
                using var cmd2 = new SqlCommand(deleteDeSql, conn, trans);
                cmd2.Parameters.AddWithValue("@MaDe", maDeThi);
                cmd2.ExecuteNonQuery();

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// [Yêu cầu 3.7] Quản trị viên / Giảng viên xem danh sách các lượt làm bài của một đề thi
        /// </summary>
        public List<LuotLamBaiDTO> LayLichSuLamBaiCuaDeThi(int maDeThi)
        {
            var list = new List<LuotLamBaiDTO>();
            string query = @"
                SELECT lb.MaLuotLamBai, lb.MaNguoiDung, nd.HoTen, nd.TenDangNhap,
                       lb.MaBaiTapDeThi, dt.TenBaiTapDeThi, lb.HinhThucLam,
                       lb.ThoiDiemBatDau, lb.ThoiDiemNopBai, lb.ThoiGianLamGiay,
                       lb.TongSoCau, lb.SoCauDung, lb.SoCauSai, lb.DiemSo, lb.TrangThai
                FROM LuotLamBai lb
                INNER JOIN NguoiDung nd ON lb.MaNguoiDung = nd.MaNguoiDung
                INNER JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                WHERE lb.MaBaiTapDeThi = @MaDe
                ORDER BY lb.ThoiDiemNopBai DESC, lb.MaLuotLamBai DESC";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaDe", maDeThi) });
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new LuotLamBaiDTO
                    {
                        MaLuotLamBai = Convert.ToInt32(row["MaLuotLamBai"]),
                        MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                        HoTenNguoiDung = row["HoTen"]?.ToString() ?? "",
                        TenDangNhap = row["TenDangNhap"]?.ToString() ?? "",
                        MaBaiTapDeThi = Convert.ToInt32(row["MaBaiTapDeThi"]),
                        TenBaiTapDeThi = row["TenBaiTapDeThi"]?.ToString() ?? "",
                        HinhThucLam = row["HinhThucLam"]?.ToString() ?? "ThiThu",
                        ThoiDiemBatDau = Convert.ToDateTime(row["ThoiDiemBatDau"]),
                        ThoiDiemNopBai = row["ThoiDiemNopBai"] == DBNull.Value ? null : Convert.ToDateTime(row["ThoiDiemNopBai"]),
                        ThoiGianLamGiay = Convert.ToInt32(row["ThoiGianLamGiay"]),
                        TongSoCau = Convert.ToInt32(row["TongSoCau"]),
                        SoCauDung = Convert.ToInt32(row["SoCauDung"]),
                        SoCauSai = Convert.ToInt32(row["SoCauSai"]),
                        DiemSo = Convert.ToDecimal(row["DiemSo"]),
                        TrangThai = row["TrangThai"]?.ToString() ?? "DaNop"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lấy lượt thi của đề: {ex.Message}");
            }
            return list;
        }

        /// <summary>
        /// Đếm số lượt học viên đã thi một bài thi cụ thể (để kiểm tra giới hạn lượt thi)
        /// </summary>
        public int DemSoLuotHocVienDaLam(int maDeThi, int maNguoiDung)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM LuotLamBai 
                WHERE MaBaiTapDeThi = @MaDe AND MaNguoiDung = @MaND AND TrangThai = N'DaNop'";

            try
            {
                object? res = SqlDataProvider.Instance.ExecuteScalar(query, new[]
                {
                    new SqlParameter("@MaDe", maDeThi),
                    new SqlParameter("@MaND", maNguoiDung)
                });
                return res != null && res != DBNull.Value ? Convert.ToInt32(res) : 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// [Yêu cầu 3.5 & 3.6] Lưu kết quả thi của học viên bằng SqlTransaction an toàn
        /// </summary>
        public int LuuKetQuaThi(
            int maNguoiDung, 
            int maDeThi, 
            string hinhThucLam, 
            DateTime batDau,
            int thoiGianGiay, 
            int tongSoCau, 
            int soCauDung, 
            decimal diemSo,
            List<(int MaCauHoi, int? MaLuaChon, string? CauTraLoiVanBan, bool DungSai, decimal DiemCau)> chiTiet)
        {
            int soCauSai = tongSoCau - soCauDung;

            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                string queryLuot = @"
                    INSERT INTO LuotLamBai (
                        MaNguoiDung, MaBaiTapDeThi, HinhThucLam, ThoiDiemBatDau, ThoiDiemNopBai, 
                        ThoiGianLamGiay, TongSoCau, SoCauDung, SoCauSai, DiemSo, TrangThai)
                    VALUES (
                        @MaND, @MaDe, @HinhThuc, @BatDau, SYSDATETIME(), 
                        @ThoiGian, @TongSo, @SoDung, @SoSai, @Diem, N'DaNop');
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using var cmdLuot = new SqlCommand(queryLuot, conn, trans);
                cmdLuot.Parameters.AddWithValue("@MaND", maNguoiDung);
                cmdLuot.Parameters.AddWithValue("@MaDe", maDeThi);
                cmdLuot.Parameters.AddWithValue("@HinhThuc", hinhThucLam);
                cmdLuot.Parameters.AddWithValue("@BatDau", batDau);
                cmdLuot.Parameters.AddWithValue("@ThoiGian", thoiGianGiay);
                cmdLuot.Parameters.AddWithValue("@TongSo", tongSoCau);
                cmdLuot.Parameters.AddWithValue("@SoDung", soCauDung);
                cmdLuot.Parameters.AddWithValue("@SoSai", soCauSai);
                cmdLuot.Parameters.AddWithValue("@Diem", diemSo);

                int maLuotLamBai = (int)cmdLuot.ExecuteScalar();

                // Lưu chi tiết từng câu trả lời
                string queryChiTiet = @"
                    INSERT INTO ChiTietTraLoi (MaLuotLamBai, MaCauHoi, MaLuaChonDaChon, CauTraLoiVanBan, KetQuaDungSai, DiemSo, ThoiDiemTraLoi)
                    VALUES (@MaLuot, @MaCauHoi, @MaLC, @VanBan, @DungSai, @DiemCau, SYSDATETIME());";

                foreach (var item in chiTiet)
                {
                    using var cmdCT = new SqlCommand(queryChiTiet, conn, trans);
                    cmdCT.Parameters.AddWithValue("@MaLuot", maLuotLamBai);
                    cmdCT.Parameters.AddWithValue("@MaCauHoi", item.MaCauHoi);
                    cmdCT.Parameters.AddWithValue("@MaLC", (object?)item.MaLuaChon ?? DBNull.Value);
                    cmdCT.Parameters.AddWithValue("@VanBan", (object?)item.CauTraLoiVanBan ?? DBNull.Value);
                    cmdCT.Parameters.AddWithValue("@DungSai", item.DungSai ? 1 : 0);
                    cmdCT.Parameters.AddWithValue("@DiemCau", item.DiemCau);
                    cmdCT.ExecuteNonQuery();
                }

                // Ghi nhận vào bảng LichSuHocTap
                string queryLichSu = @"
                    INSERT INTO LichSuHocTap (MaNguoiDung, MaLuotLamBai, LoaiHoatDong, SoPhutHoc, DiemSo, SoCauDung, SoCauSai, NgayGhiNhan)
                    VALUES (@MaND, @MaLuot, @LoaiHD, @SoPhut, @Diem, @SoDung, @SoSai, SYSDATETIME());";

                int soPhut = Math.Max(1, thoiGianGiay / 60);
                using var cmdLS = new SqlCommand(queryLichSu, conn, trans);
                cmdLS.Parameters.AddWithValue("@MaND", maNguoiDung);
                cmdLS.Parameters.AddWithValue("@MaLuot", maLuotLamBai);
                cmdLS.Parameters.AddWithValue("@LoaiHD", hinhThucLam);
                cmdLS.Parameters.AddWithValue("@SoPhut", soPhut);
                cmdLS.Parameters.AddWithValue("@Diem", diemSo);
                cmdLS.Parameters.AddWithValue("@SoDung", soCauDung);
                cmdLS.Parameters.AddWithValue("@SoSai", soCauSai);
                cmdLS.ExecuteNonQuery();

                trans.Commit();
                return maLuotLamBai;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lưu kết quả thi: {ex.Message}");
                return 0;
            }
        }

        public LuotLamBaiDTO? LayChiTietKetQuaLamBai(int maLuotLamBai)
        {
            string queryLuot = @"
                SELECT lb.MaLuotLamBai, lb.MaNguoiDung, lb.MaBaiTapDeThi, dt.TenBaiTapDeThi,
                       lb.HinhThucLam, lb.ThoiDiemBatDau, lb.ThoiDiemNopBai, lb.ThoiGianLamGiay,
                       lb.TongSoCau, lb.SoCauDung, lb.SoCauSai, lb.DiemSo, lb.TrangThai,
                       ISNULL(dt.ChoXemDapAn, 1) AS ChoXemDapAn,
                       ISNULL(dt.ChoXemLoiGiai, 1) AS ChoXemLoiGiai,
                       dt.DiemDat, dt.TongDiem
                FROM LuotLamBai lb
                LEFT JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                WHERE lb.MaLuotLamBai = @MaLuot";

            DataTable dt = SqlDataProvider.Instance.ExecuteQuery(queryLuot, new[] { new SqlParameter("@MaLuot", maLuotLamBai) });
            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            var dto = new LuotLamBaiDTO
            {
                MaLuotLamBai = Convert.ToInt32(row["MaLuotLamBai"]),
                MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                MaBaiTapDeThi = Convert.ToInt32(row["MaBaiTapDeThi"]),
                TenBaiTapDeThi = row["TenBaiTapDeThi"]?.ToString() ?? "Bài thi",
                HinhThucLam = row["HinhThucLam"]?.ToString() ?? "ThiThu",
                ThoiDiemBatDau = Convert.ToDateTime(row["ThoiDiemBatDau"]),
                ThoiDiemNopBai = row["ThoiDiemNopBai"] == DBNull.Value ? null : Convert.ToDateTime(row["ThoiDiemNopBai"]),
                ThoiGianLamGiay = Convert.ToInt32(row["ThoiGianLamGiay"]),
                TongSoCau = Convert.ToInt32(row["TongSoCau"]),
                SoCauDung = Convert.ToInt32(row["SoCauDung"]),
                SoCauSai = Convert.ToInt32(row["SoCauSai"]),
                DiemSo = Convert.ToDecimal(row["DiemSo"]),
                TrangThai = row["TrangThai"]?.ToString() ?? "DaNop",
                ChoXemDapAn = row["ChoXemDapAn"] == DBNull.Value || Convert.ToBoolean(row["ChoXemDapAn"]),
                ChoXemLoiGiai = row["ChoXemLoiGiai"] == DBNull.Value || Convert.ToBoolean(row["ChoXemLoiGiai"]),
                DiemDat = row["DiemDat"] == DBNull.Value ? 5.0m : Convert.ToDecimal(row["DiemDat"]),
                TongDiem = row["TongDiem"] == DBNull.Value ? 10.0m : Convert.ToDecimal(row["TongDiem"])
            };

            // Lấy danh sách câu hỏi kèm đáp án chọn và giải thích
            string queryCT = @"
                SELECT ct.MaChiTietTraLoi, ct.MaCauHoi, ch.NoiDungCauHoi, ch.DoanVanDoc, ch.KyNang,
                       ch.GiaiThichDapAn, ct.MaLuaChonDaChon, ct.CauTraLoiVanBan, ct.KetQuaDungSai, ct.DiemSo,
                       lcChon.KyHieuLuaChon AS KyHieuDaChon, lcChon.NoiDungLuaChon AS NoiDungDaChon,
                       lcDung.MaLuaChon AS MaLuaChonDung, lcDung.KyHieuLuaChon AS KyHieuDapAnDung, lcDung.NoiDungLuaChon AS NoiDungDapAnDung
                FROM ChiTietTraLoi ct
                INNER JOIN CauHoi ch ON ct.MaCauHoi = ch.MaCauHoi
                LEFT JOIN LuaChonDapAn lcChon ON ct.MaLuaChonDaChon = lcChon.MaLuaChon
                LEFT JOIN LuaChonDapAn lcDung ON ch.MaCauHoi = lcDung.MaCauHoi AND lcDung.LaDapAnDung = 1
                WHERE ct.MaLuotLamBai = @MaLuot
                ORDER BY ct.MaChiTietTraLoi ASC";

            DataTable dtCT = SqlDataProvider.Instance.ExecuteQuery(queryCT, new[] { new SqlParameter("@MaLuot", maLuotLamBai) });
            int thuTu = 1;
            foreach (DataRow r in dtCT.Rows)
            {
                int maCH = Convert.ToInt32(r["MaCauHoi"]);
                var item = new ChiTietTraLoiDTO
                {
                    MaChiTietTraLoi = Convert.ToInt32(r["MaChiTietTraLoi"]),
                    MaLuotLamBai = maLuotLamBai,
                    MaCauHoi = maCH,
                    ThuTuCauHoi = thuTu++,
                    NoiDungCauHoi = r["NoiDungCauHoi"]?.ToString() ?? "",
                    DoanVanDoc = r["DoanVanDoc"] == DBNull.Value ? null : r["DoanVanDoc"]?.ToString(),
                    KyNang = r["KyNang"]?.ToString() ?? "Doc",
                    MaLuaChonDaChon = r["MaLuaChonDaChon"] == DBNull.Value ? null : Convert.ToInt32(r["MaLuaChonDaChon"]),
                    KyHieuDaChon = r["KyHieuDaChon"]?.ToString() ?? "",
                    NoiDungDaChon = r["NoiDungDaChon"]?.ToString() ?? "",
                    CauTraLoiVanBan = r["CauTraLoiVanBan"] == DBNull.Value ? null : r["CauTraLoiVanBan"]?.ToString(),
                    MaLuaChonDung = r["MaLuaChonDung"] == DBNull.Value ? 0 : Convert.ToInt32(r["MaLuaChonDung"]),
                    KyHieuDapAnDung = r["KyHieuDapAnDung"]?.ToString() ?? "",
                    NoiDungDapAnDung = r["NoiDungDapAnDung"]?.ToString() ?? "",
                    KetQuaDungSai = r["KetQuaDungSai"] != DBNull.Value && Convert.ToBoolean(r["KetQuaDungSai"]),
                    DiemSo = r["DiemSo"] == DBNull.Value ? 0 : Convert.ToDecimal(r["DiemSo"]),
                    GiaiThichDapAn = r["GiaiThichDapAn"] == DBNull.Value ? null : r["GiaiThichDapAn"]?.ToString()
                };

                // Lấy tất cả lựa chọn
                string queryLC = "SELECT MaLuaChon, MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung FROM LuaChonDapAn WHERE MaCauHoi = @MaCauHoi ORDER BY KyHieuLuaChon ASC";
                DataTable dtLC = SqlDataProvider.Instance.ExecuteQuery(queryLC, new[] { new SqlParameter("@MaCauHoi", maCH) });
                foreach (DataRow rLC in dtLC.Rows)
                {
                    item.DanhSachLuaChon.Add(new LuaChonDapAnDTO
                    {
                        MaLuaChon = Convert.ToInt32(rLC["MaLuaChon"]),
                        MaCauHoi = Convert.ToInt32(rLC["MaCauHoi"]),
                        KyHieuLuaChon = rLC["KyHieuLuaChon"]?.ToString() ?? "",
                        NoiDungLuaChon = rLC["NoiDungLuaChon"]?.ToString() ?? "",
                        LaDapAnDung = Convert.ToBoolean(rLC["LaDapAnDung"])
                    });
                }

                dto.DanhSachChiTiet.Add(item);
            }

            return dto;
        }

        public List<LuotLamBaiDTO> LayLichSuThi(int maNguoiDung, int top = 10)
        {
            var list = new List<LuotLamBaiDTO>();
            string query = @"
                SELECT TOP (@Top) lb.MaLuotLamBai, lb.MaNguoiDung, lb.MaBaiTapDeThi, dt.TenBaiTapDeThi,
                       lb.HinhThucLam, lb.ThoiDiemBatDau, lb.ThoiDiemNopBai, lb.ThoiGianLamGiay,
                       lb.TongSoCau, lb.SoCauDung, lb.SoCauSai, lb.DiemSo, lb.TrangThai,
                       ISNULL(dt.ChoXemDapAn, 1) AS ChoXemDapAn,
                       ISNULL(dt.ChoXemLoiGiai, 1) AS ChoXemLoiGiai,
                       dt.DiemDat, dt.TongDiem
                FROM LuotLamBai lb
                LEFT JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                WHERE lb.MaNguoiDung = @MaND
                ORDER BY lb.ThoiDiemNopBai DESC, lb.MaLuotLamBai DESC";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaND", maNguoiDung), new SqlParameter("@Top", top) });
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new LuotLamBaiDTO
                    {
                        MaLuotLamBai = Convert.ToInt32(row["MaLuotLamBai"]),
                        MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                        MaBaiTapDeThi = Convert.ToInt32(row["MaBaiTapDeThi"]),
                        TenBaiTapDeThi = row["TenBaiTapDeThi"]?.ToString() ?? "Bài thi",
                        HinhThucLam = row["HinhThucLam"]?.ToString() ?? "ThiThu",
                        ThoiDiemBatDau = Convert.ToDateTime(row["ThoiDiemBatDau"]),
                        ThoiDiemNopBai = row["ThoiDiemNopBai"] == DBNull.Value ? null : Convert.ToDateTime(row["ThoiDiemNopBai"]),
                        ThoiGianLamGiay = Convert.ToInt32(row["ThoiGianLamGiay"]),
                        TongSoCau = Convert.ToInt32(row["TongSoCau"]),
                        SoCauDung = Convert.ToInt32(row["SoCauDung"]),
                        SoCauSai = Convert.ToInt32(row["SoCauSai"]),
                        DiemSo = Convert.ToDecimal(row["DiemSo"]),
                        TrangThai = row["TrangThai"]?.ToString() ?? "DaNop",
                        ChoXemDapAn = row["ChoXemDapAn"] == DBNull.Value || Convert.ToBoolean(row["ChoXemDapAn"]),
                        ChoXemLoiGiai = row["ChoXemLoiGiai"] == DBNull.Value || Convert.ToBoolean(row["ChoXemLoiGiai"]),
                        DiemDat = row["DiemDat"] == DBNull.Value ? 5.0m : Convert.ToDecimal(row["DiemDat"]),
                        TongDiem = row["TongDiem"] == DBNull.Value ? 10.0m : Convert.ToDecimal(row["TongDiem"])
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lấy lịch sử thi: {ex.Message}");
            }
            return list;
        }
    }
}
