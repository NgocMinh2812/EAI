using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class LuyenTapDAL
    {
        private static LuyenTapDAL? _instance;
        public static LuyenTapDAL Instance => _instance ??= new LuyenTapDAL();

        /// <summary>
        /// [Yêu cầu 4.2] Đếm số lượng câu hỏi khả dụng thực tế trong ngân hàng theo các tiêu chí đã chọn
        /// </summary>
        public int DemSoCauKhaDung(int? maChuDe, int? maBoCauHoi, string? kyNang, string? trinhDo, string? doKho)
        {
            string query = @"
                SELECT COUNT(DISTINCT ch.MaCauHoi)
                FROM CauHoi ch
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON ch.MaChuDe = cd.MaChuDe
                LEFT JOIN BoCauHoi_CauHoi bc ON ch.MaCauHoi = bc.MaCauHoi
                WHERE (@MaChuDe IS NULL OR ch.MaChuDe = @MaChuDe)
                  AND (@MaBoCauHoi IS NULL OR bc.MaBoCauHoi = @MaBoCauHoi)
                  AND (@KyNang IS NULL OR @KyNang = 'All' OR ch.KyNang = @KyNang)
                  AND (@TrinhDo IS NULL OR @TrinhDo = 'All' OR td.MaCapDo = @TrinhDo)
                  AND (@DoKho IS NULL OR @DoKho = 'All' OR ch.DoKho = @DoKho)
                  AND ch.DaKiemDuyet = 1";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaChuDe", (object?)maChuDe ?? DBNull.Value),
                new SqlParameter("@MaBoCauHoi", (object?)maBoCauHoi ?? DBNull.Value),
                new SqlParameter("@KyNang", string.IsNullOrWhiteSpace(kyNang) || kyNang == "All" ? DBNull.Value : (object)kyNang.Trim()),
                new SqlParameter("@TrinhDo", string.IsNullOrWhiteSpace(trinhDo) || trinhDo == "All" ? DBNull.Value : (object)trinhDo.Trim()),
                new SqlParameter("@DoKho", string.IsNullOrWhiteSpace(doKho) || doKho == "All" ? DBNull.Value : (object)doKho.Trim())
            };

            try
            {
                object? res = SqlDataProvider.Instance.ExecuteScalar(query, parameters);
                return res != null && res != DBNull.Value ? Convert.ToInt32(res) : 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LuyenTapDAL.DemSoCauKhaDung] {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// [Yêu cầu 4.2] Lấy ngẫu nhiên câu hỏi theo bộ lọc không trùng lặp, bảo toàn Audio và đoạn đọc hiểu
        /// </summary>
        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiLuyenTap(
            int? maChuDe, 
            int? maBoCauHoi, 
            string? kyNang, 
            string? trinhDo, 
            string? doKho, 
            int soLuong)
        {
            var list = new List<CauHoiKhaoSatDTO>();

            string query = $@"
                SELECT TOP ({soLuong}) ch.MaCauHoi, ch.KyNang, td.MaCapDo AS MaTrinhDo, td.TenTrinhDo,
                       ch.NoiDungCauHoi, ch.DoanVanDoc, ch.GiaiThichDapAn, ch.LoaiCauHoi, ch.DoKho,
                       COALESCE(ch.DuongDanAudio, bh.DuongDanAudio, bh.AudioPath, bhAudio.AudioPath) AS AudioPath,
                       COALESCE(ch.MaBaiHoc, bh.MaBaiHoc, bhAudio.MaBaiHoc) AS MaBaiHoc,
                       COALESCE(bh.TieuDe, bhAudio.TieuDe, N'Bài học luyện nghe') AS TenBaiHoc,
                       ch.MaChuDe, cd.TenChuDe
                FROM CauHoi ch
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON ch.MaChuDe = cd.MaChuDe
                LEFT JOIN BoCauHoi_CauHoi bc ON ch.MaCauHoi = bc.MaCauHoi
                LEFT JOIN BaiHoc bh ON ch.MaBaiHoc = bh.MaBaiHoc
                OUTER APPLY (
                    SELECT TOP 1 bhSub.MaBaiHoc, bhSub.TieuDe, 
                                 COALESCE(bhSub.DuongDanAudio, bhSub.AudioPath) AS AudioPath
                    FROM BaiHoc bhSub
                    WHERE (ch.MaBaiHoc IS NULL OR (bh.DuongDanAudio IS NULL AND bh.AudioPath IS NULL))
                      AND (bhSub.DuongDanAudio IS NOT NULL OR bhSub.AudioPath IS NOT NULL)
                      AND (bhSub.LoaiBaiHoc = 'Nghe' OR bhSub.LoaiBaiHoc LIKE '%Nghe%' OR bhSub.LoaiBaiHoc LIKE '%Listen%')
                    ORDER BY CASE WHEN bhSub.MaChuDe = ch.MaChuDe AND bhSub.MaTrinhDo = ch.MaTrinhDo THEN 1
                                  WHEN bhSub.MaChuDe = ch.MaChuDe THEN 2
                                  WHEN bhSub.MaTrinhDo = ch.MaTrinhDo THEN 3 ELSE 4 END
                ) bhAudio
                WHERE (@MaChuDe IS NULL OR ch.MaChuDe = @MaChuDe)
                  AND (@MaBoCauHoi IS NULL OR bc.MaBoCauHoi = @MaBoCauHoi)
                  AND (@KyNang IS NULL OR @KyNang = 'All' OR ch.KyNang = @KyNang)
                  AND (@TrinhDo IS NULL OR @TrinhDo = 'All' OR td.MaCapDo = @TrinhDo)
                  AND (@DoKho IS NULL OR @DoKho = 'All' OR ch.DoKho = @DoKho)
                  AND ch.DaKiemDuyet = 1
                GROUP BY ch.MaCauHoi, ch.KyNang, td.MaCapDo, td.TenTrinhDo,
                         ch.NoiDungCauHoi, ch.DoanVanDoc, ch.GiaiThichDapAn, ch.LoaiCauHoi, ch.DoKho,
                         ch.DuongDanAudio, bh.DuongDanAudio, bh.AudioPath, bhAudio.AudioPath,
                         ch.MaBaiHoc, bh.MaBaiHoc, bhAudio.MaBaiHoc, bh.TieuDe, bhAudio.TieuDe,
                         ch.MaChuDe, cd.TenChuDe
                ORDER BY NEWID()";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaChuDe", (object?)maChuDe ?? DBNull.Value),
                new SqlParameter("@MaBoCauHoi", (object?)maBoCauHoi ?? DBNull.Value),
                new SqlParameter("@KyNang", string.IsNullOrWhiteSpace(kyNang) || kyNang == "All" ? DBNull.Value : (object)kyNang.Trim()),
                new SqlParameter("@TrinhDo", string.IsNullOrWhiteSpace(trinhDo) || trinhDo == "All" ? DBNull.Value : (object)trinhDo.Trim()),
                new SqlParameter("@DoKho", string.IsNullOrWhiteSpace(doKho) || doKho == "All" ? DBNull.Value : (object)doKho.Trim())
            };

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
                int thuTu = 1;
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                        ThuTuCauHoi = thuTu++,
                        KyNang = row["KyNang"]?.ToString() ?? "Doc",
                        MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B1",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                        NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                        DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                        GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString(),
                        LoaiCauHoi = row["LoaiCauHoi"]?.ToString() ?? "TracNghiem",
                        DoKho = row["DoKho"]?.ToString() ?? "TrungBinh",
                        AudioPath = row["AudioPath"] == DBNull.Value ? null : row["AudioPath"]?.ToString(),
                        MaBaiHoc = row["MaBaiHoc"] == DBNull.Value ? null : Convert.ToInt32(row["MaBaiHoc"]),
                        TenBaiHoc = row["TenBaiHoc"] == DBNull.Value ? null : row["TenBaiHoc"]?.ToString(),
                        MaChuDe = row["MaChuDe"] == DBNull.Value ? null : Convert.ToInt32(row["MaChuDe"]),
                        Diem = 1.0
                    });
                }

                // Nạp toàn bộ danh sách lựa chọn trong 1 query duy nhất
                CauHoiDAL.NapDanhSachLuaChon(list);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LuyenTapDAL.LayDanhSachCauHoiLuyenTap] {ex.Message}");
            }

            return list;
        }

        /// <summary>
        /// [Yêu cầu 4.5] Lấy danh sách các câu hỏi mà học viên đã trả lời sai trong lịch sử để ôn luyện lại
        /// Chỉ hiển thị những câu mà lần trả lời gần nhất là SAI (nếu đã luyện lại đúng thì loại bỏ)
        /// </summary>
        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiSaiCuaNguoiDung(int maNguoiDung, int top = 20)
        {
            var list = new List<CauHoiKhaoSatDTO>();
            if (maNguoiDung <= 0) return list;

            string query = $@"
                WITH LatestAnswerPerQuestion AS (
                    SELECT ct.MaCauHoi, ct.KetQuaDungSai, ct.ThoiDiemTraLoi,
                           ROW_NUMBER() OVER (
                               PARTITION BY ct.MaCauHoi 
                               ORDER BY ct.ThoiDiemTraLoi DESC, ct.MaChiTietTraLoi DESC
                           ) AS rn
                    FROM ChiTietTraLoi ct
                    INNER JOIN LuotLamBai lb ON ct.MaLuotLamBai = lb.MaLuotLamBai
                    WHERE lb.MaNguoiDung = @MaND
                )
                SELECT TOP ({top}) ch.MaCauHoi, ch.KyNang, td.MaCapDo AS MaTrinhDo, td.TenTrinhDo,
                       ch.NoiDungCauHoi, ch.DoanVanDoc, ch.GiaiThichDapAn, ch.LoaiCauHoi, ch.DoKho,
                       COALESCE(ch.DuongDanAudio, bh.DuongDanAudio, bh.AudioPath) AS AudioPath,
                       ch.MaBaiHoc, bh.TieuDe AS TenBaiHoc, ch.MaChuDe,
                       la.ThoiDiemTraLoi AS LanSaiCuoi
                FROM LatestAnswerPerQuestion la
                INNER JOIN CauHoi ch ON la.MaCauHoi = ch.MaCauHoi
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN BaiHoc bh ON ch.MaBaiHoc = bh.MaBaiHoc
                WHERE la.rn = 1 AND la.KetQuaDungSai = 0
                ORDER BY la.ThoiDiemTraLoi DESC, ch.MaCauHoi DESC";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaND", maNguoiDung) });
                int thuTu = 1;
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                        ThuTuCauHoi = thuTu++,
                        KyNang = row["KyNang"]?.ToString() ?? "Doc",
                        MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B1",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                        NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                        DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                        GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString(),
                        LoaiCauHoi = row["LoaiCauHoi"]?.ToString() ?? "TracNghiem",
                        DoKho = row["DoKho"]?.ToString() ?? "TrungBinh",
                        AudioPath = row["AudioPath"] == DBNull.Value ? null : row["AudioPath"]?.ToString(),
                        MaBaiHoc = row["MaBaiHoc"] == DBNull.Value ? null : Convert.ToInt32(row["MaBaiHoc"]),
                        TenBaiHoc = row["TenBaiHoc"] == DBNull.Value ? null : row["TenBaiHoc"]?.ToString(),
                        MaChuDe = row["MaChuDe"] == DBNull.Value ? null : Convert.ToInt32(row["MaChuDe"]),
                        Diem = 1.0
                    });
                }

                CauHoiDAL.NapDanhSachLuaChon(list);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LuyenTapDAL.LayDanhSachCauHoiSaiCuaNguoiDung] {ex.Message}");
            }

            return list;
        }

        /// <summary>
        /// [Yêu cầu 4.4] Lưu kết quả phiên luyện tập vào CSDL kèm tiêu đề chủ đề rõ ràng
        /// </summary>
        public int LuuKetQuaPhienLuyenTap(
            int maNguoiDung,
            int thoiGianGiay,
            int tongSoCau,
            int soCauDung,
            decimal tyLeChinhXac,
            List<(int MaCauHoi, int? MaLuaChon, string? CauTraLoiVanBan, bool DungSai)> chiTiet)
        {
            return LuuKetQuaPhienLuyenTap(maNguoiDung, "Phiên luyện tập tự do", thoiGianGiay, tongSoCau, soCauDung, tyLeChinhXac, chiTiet);
        }

        public int LuuKetQuaPhienLuyenTap(
            int maNguoiDung,
            string tieuDePhien,
            int thoiGianGiay,
            int tongSoCau,
            int soCauDung,
            decimal tyLeChinhXac,
            List<(int MaCauHoi, int? MaLuaChon, string? CauTraLoiVanBan, bool DungSai)> chiTiet)
        {
            if (maNguoiDung <= 0) return 0;
            int soCauSai = tongSoCau - soCauDung;
            string safeTitle = string.IsNullOrWhiteSpace(tieuDePhien) ? "Phiên luyện tập tự do" : tieuDePhien.Trim();

            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                int maBaiTapLuyenTap = 4;
                string checkSql = "SELECT TOP 1 MaBaiTapDeThi FROM BaiTapDeThi WHERE LoaiHinh = N'LuyenTap' AND TenBaiTapDeThi = @TenDe";
                using var cmdCheck = new SqlCommand(checkSql, conn, trans);
                cmdCheck.Parameters.AddWithValue("@TenDe", safeTitle);
                object? res = cmdCheck.ExecuteScalar();
                if (res != null && res != DBNull.Value)
                {
                    maBaiTapLuyenTap = Convert.ToInt32(res);
                }
                else
                {
                    string insertDeSql = @"
                        INSERT INTO BaiTapDeThi (TenBaiTapDeThi, LoaiHinh, DaPhatHanh, TrangThai, TongSoCau, NgayTao)
                        VALUES (@TenDe, N'LuyenTap', 1, N'DaXuatBan', @TongSo, SYSDATETIME());
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    using var cmdInsertDe = new SqlCommand(insertDeSql, conn, trans);
                    cmdInsertDe.Parameters.AddWithValue("@TenDe", safeTitle);
                    cmdInsertDe.Parameters.AddWithValue("@TongSo", tongSoCau);
                    maBaiTapLuyenTap = (int)cmdInsertDe.ExecuteScalar();
                }

                string queryLuot = @"
                    INSERT INTO LuotLamBai (
                        MaNguoiDung, MaBaiTapDeThi, HinhThucLam, ThoiDiemBatDau, ThoiDiemNopBai, 
                        ThoiGianLamGiay, TongSoCau, SoCauDung, SoCauSai, DiemSo, TrangThai)
                    VALUES (
                        @MaND, @MaDe, N'LuyenTap', DATEADD(SECOND, -@ThoiGian, SYSDATETIME()), SYSDATETIME(), 
                        @ThoiGian, @TongSo, @SoDung, @SoSai, @Diem, N'DaNop');
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using var cmdLuot = new SqlCommand(queryLuot, conn, trans);
                cmdLuot.Parameters.AddWithValue("@MaND", maNguoiDung);
                cmdLuot.Parameters.AddWithValue("@MaDe", maBaiTapLuyenTap);
                cmdLuot.Parameters.AddWithValue("@ThoiGian", thoiGianGiay);
                cmdLuot.Parameters.AddWithValue("@TongSo", tongSoCau);
                cmdLuot.Parameters.AddWithValue("@SoDung", soCauDung);
                cmdLuot.Parameters.AddWithValue("@SoSai", soCauSai);
                cmdLuot.Parameters.AddWithValue("@Diem", Math.Round(tyLeChinhXac / 10m, 2));

                int maLuotLamBai = (int)cmdLuot.ExecuteScalar();

                string queryChiTiet = @"
                    IF EXISTS (SELECT 1 FROM CauHoi WHERE MaCauHoi = @MaCauHoi)
                    BEGIN
                        DECLARE @ActualLC INT = @MaLC;
                        IF @ActualLC IS NOT NULL AND NOT EXISTS (SELECT 1 FROM LuaChonDapAn WHERE MaLuaChon = @ActualLC)
                            SET @ActualLC = NULL;

                        INSERT INTO ChiTietTraLoi (MaLuotLamBai, MaCauHoi, MaLuaChonDaChon, CauTraLoiVanBan, KetQuaDungSai, DiemSo, ThoiDiemTraLoi)
                        VALUES (@MaLuot, @MaCauHoi, @ActualLC, @VanBan, @DungSai, @DiemCau, SYSDATETIME());
                    END";

                decimal diemMoiCau = tongSoCau > 0 ? (10.0m / tongSoCau) : 0m;

                foreach (var item in chiTiet)
                {
                    using var cmdCT = new SqlCommand(queryChiTiet, conn, trans);
                    cmdCT.Parameters.AddWithValue("@MaLuot", maLuotLamBai);
                    cmdCT.Parameters.AddWithValue("@MaCauHoi", item.MaCauHoi);
                    cmdCT.Parameters.AddWithValue("@MaLC", (item.MaLuaChon.HasValue && item.MaLuaChon.Value > 0) ? (object)item.MaLuaChon.Value : DBNull.Value);
                    cmdCT.Parameters.AddWithValue("@VanBan", (object?)item.CauTraLoiVanBan ?? DBNull.Value);
                    cmdCT.Parameters.AddWithValue("@DungSai", item.DungSai ? 1 : 0);
                    cmdCT.Parameters.AddWithValue("@DiemCau", item.DungSai ? diemMoiCau : 0m);
                    cmdCT.ExecuteNonQuery();
                }

                // Ghi nhận vào LichSuHocTap
                string queryLichSu = @"
                    INSERT INTO LichSuHocTap (MaNguoiDung, MaLuotLamBai, LoaiHoatDong, SoPhutHoc, DiemSo, SoCauDung, SoCauSai, NgayGhiNhan)
                    VALUES (@MaND, @MaLuot, N'LuyenTap', @SoPhut, @Diem, @SoDung, @SoSai, SYSDATETIME());";

                int soPhut = Math.Max(1, thoiGianGiay / 60);
                using var cmdLS = new SqlCommand(queryLichSu, conn, trans);
                cmdLS.Parameters.AddWithValue("@MaND", maNguoiDung);
                cmdLS.Parameters.AddWithValue("@MaLuot", maLuotLamBai);
                cmdLS.Parameters.AddWithValue("@SoPhut", soPhut);
                cmdLS.Parameters.AddWithValue("@Diem", Math.Round(tyLeChinhXac / 10m, 2));
                cmdLS.Parameters.AddWithValue("@SoDung", soCauDung);
                cmdLS.Parameters.AddWithValue("@SoSai", soCauSai);
                cmdLS.ExecuteNonQuery();

                trans.Commit();
                return maLuotLamBai;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                System.Diagnostics.Debug.WriteLine($"[LuyenTapDAL.LuuKetQuaPhienLuyenTap] {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// [Yêu cầu 4.4] Lấy lịch sử các phiên luyện tập của người dùng
        /// </summary>
        public List<LuotLamBaiDTO> LayLichSuLuyenTap(int maNguoiDung, int top = 20)
        {
            var list = new List<LuotLamBaiDTO>();
            if (maNguoiDung <= 0) return list;

            string query = @"
                SELECT TOP (@Top) lb.MaLuotLamBai, lb.MaNguoiDung, lb.MaBaiTapDeThi,
                       ISNULL(dt.TenBaiTapDeThi, N'Phiên luyện tập tự do') AS TenBaiTapDeThi,
                       lb.HinhThucLam, lb.ThoiDiemBatDau, lb.ThoiDiemNopBai, lb.ThoiGianLamGiay,
                       lb.TongSoCau, lb.SoCauDung, lb.SoCauSai, lb.DiemSo, lb.TrangThai
                FROM LuotLamBai lb
                LEFT JOIN BaiTapDeThi dt ON lb.MaBaiTapDeThi = dt.MaBaiTapDeThi
                WHERE lb.MaNguoiDung = @MaND AND lb.HinhThucLam = N'LuyenTap'
                ORDER BY lb.ThoiDiemNopBai DESC, lb.MaLuotLamBai DESC";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] 
                { 
                    new SqlParameter("@MaND", maNguoiDung), 
                    new SqlParameter("@Top", top) 
                });

                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new LuotLamBaiDTO
                    {
                        MaLuotLamBai = Convert.ToInt32(row["MaLuotLamBai"]),
                        MaNguoiDung = Convert.ToInt32(row["MaNguoiDung"]),
                        MaBaiTapDeThi = Convert.ToInt32(row["MaBaiTapDeThi"]),
                        TenBaiTapDeThi = row["TenBaiTapDeThi"]?.ToString() ?? "Phiên luyện tập",
                        HinhThucLam = "LuyenTap",
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
                System.Diagnostics.Debug.WriteLine($"[LuyenTapDAL.LayLichSuLuyenTap] {ex.Message}");
            }

            return list;
        }
    }
}
