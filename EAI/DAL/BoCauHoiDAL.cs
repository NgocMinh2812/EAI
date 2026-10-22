using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class BoCauHoiDAL
    {
        private static BoCauHoiDAL? _instance;
        public static BoCauHoiDAL Instance => _instance ??= new BoCauHoiDAL();

        /// <summary>
        /// Lấy danh sách tất cả các bộ câu hỏi kèm số lượng câu hỏi trong mỗi bộ
        /// </summary>
        public List<BoCauHoiDTO> LayDanhSachBoCauHoi(string? tuKhoa = null, string? nguonTao = null)
        {
            var list = new List<BoCauHoiDTO>();
            string query = @"
                SELECT b.MaBoCauHoi, b.TenBoCauHoi, b.MoTa, b.NguonTao, b.TenNguon,
                       b.MaBaiHoc, b.MaChuDe, b.MaTrinhDo, b.NguoiTao, b.NgayTao, b.NgayCapNhat,
                       (SELECT COUNT(*) FROM BoCauHoi_CauHoi bc WHERE bc.MaBoCauHoi = b.MaBoCauHoi) AS SoLuongCauHoi
                FROM BoCauHoi b
                WHERE (@TuKhoa IS NULL OR @TuKhoa = '' OR b.TenBoCauHoi LIKE N'%' + @TuKhoa + N'%' OR b.TenNguon LIKE N'%' + @TuKhoa + N'%')
                  AND (@NguonTao IS NULL OR @NguonTao = 'All' OR b.NguonTao = @NguonTao)
                ORDER BY b.NgayTao DESC, b.MaBoCauHoi DESC";

            var parameters = new[]
            {
                new SqlParameter("@TuKhoa", string.IsNullOrWhiteSpace(tuKhoa) ? DBNull.Value : (object)tuKhoa.Trim()),
                new SqlParameter("@NguonTao", string.IsNullOrWhiteSpace(nguonTao) ? DBNull.Value : (object)nguonTao.Trim())
            };

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new BoCauHoiDTO
                    {
                        MaBoCauHoi = Convert.ToInt32(row["MaBoCauHoi"]),
                        TenBoCauHoi = row["TenBoCauHoi"]?.ToString() ?? "",
                        MoTa = row["MoTa"] == DBNull.Value ? null : row["MoTa"]?.ToString(),
                        NguonTao = row["NguonTao"]?.ToString() ?? "AI",
                        TenNguon = row["TenNguon"] == DBNull.Value ? null : row["TenNguon"]?.ToString(),
                        MaBaiHoc = row["MaBaiHoc"] == DBNull.Value ? null : Convert.ToInt32(row["MaBaiHoc"]),
                        MaChuDe = row["MaChuDe"] == DBNull.Value ? null : Convert.ToInt32(row["MaChuDe"]),
                        MaTrinhDo = row["MaTrinhDo"] == DBNull.Value ? null : Convert.ToInt32(row["MaTrinhDo"]),
                        NguoiTao = row["NguoiTao"] == DBNull.Value ? null : Convert.ToInt32(row["NguoiTao"]),
                        NgayTao = row["NgayTao"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["NgayTao"]),
                        NgayCapNhat = row["NgayCapNhat"] == DBNull.Value ? null : Convert.ToDateTime(row["NgayCapNhat"]),
                        SoLuongCauHoi = Convert.ToInt32(row["SoLuongCauHoi"])
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BoCauHoiDAL.LayDanhSachBoCauHoi] {ex.Message}");
            }

            return list;
        }

        /// <summary>
        /// Đếm tổng số lượng câu hỏi hiện có trong toàn hệ thống
        /// </summary>
        public int DemTongSoCauHoi()
        {
            string query = "SELECT COUNT(*) FROM CauHoi";
            try
            {
                object? res = SqlDataProvider.Instance.ExecuteScalar(query);
                return res != null && res != DBNull.Value ? Convert.ToInt32(res) : 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BoCauHoiDAL.DemTongSoCauHoi] {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Lấy danh sách câu hỏi thuộc một bộ câu hỏi cụ thể
        /// </summary>
        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiTheoBo(int maBoCauHoi, string? tuKhoa = null, string? kyNang = null, string? doKho = null)
        {
            var list = new List<CauHoiKhaoSatDTO>();
            string query = @"
                SELECT ch.MaCauHoi, bc.ThuTu AS ThuTuCauHoi, ch.KyNang, td.MaCapDo AS MaTrinhDo, 
                       td.TenTrinhDo, ch.NoiDungCauHoi, ch.DoanVanDoc, ch.GiaiThichDapAn,
                       ch.MaChuDe, ch.MaTrinhDo AS MaTrinhDoId, ch.LoaiCauHoi, ch.DoKho,
                       ch.DuongDanAudio AS AudioPath, ch.DaKiemDuyet, ch.NguoiTao AS NguoiTaoId, ch.NgayTao
                FROM BoCauHoi_CauHoi bc
                INNER JOIN CauHoi ch ON bc.MaCauHoi = ch.MaCauHoi
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                WHERE bc.MaBoCauHoi = @MaBo
                  AND (@TuKhoa IS NULL OR @TuKhoa = '' OR ch.NoiDungCauHoi LIKE N'%' + @TuKhoa + N'%')
                  AND (@KyNang IS NULL OR @KyNang = 'All' OR ch.KyNang = @KyNang)
                  AND (@DoKho IS NULL OR @DoKho = 'All' OR ch.DoKho = @DoKho)
                ORDER BY bc.ThuTu ASC, ch.MaCauHoi ASC";

            var parameters = new[]
            {
                new SqlParameter("@MaBo", maBoCauHoi),
                new SqlParameter("@TuKhoa", string.IsNullOrWhiteSpace(tuKhoa) ? DBNull.Value : (object)tuKhoa.Trim()),
                new SqlParameter("@KyNang", string.IsNullOrWhiteSpace(kyNang) ? DBNull.Value : (object)kyNang.Trim()),
                new SqlParameter("@DoKho", string.IsNullOrWhiteSpace(doKho) ? DBNull.Value : (object)doKho.Trim())
            };

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                        ThuTuCauHoi = Convert.ToInt32(row["ThuTuCauHoi"]),
                        ThuTuTrongBo = Convert.ToInt32(row["ThuTuCauHoi"]),
                        KyNang = row["KyNang"]?.ToString() ?? "Doc",
                        MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B1",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                        NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                        DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                        GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString(),
                        MaChuDe = row["MaChuDe"] == DBNull.Value ? null : Convert.ToInt32(row["MaChuDe"]),
                        MaTrinhDoId = row["MaTrinhDoId"] == DBNull.Value ? null : Convert.ToInt32(row["MaTrinhDoId"]),
                        LoaiCauHoi = row["LoaiCauHoi"]?.ToString() ?? "TracNghiem",
                        DoKho = row["DoKho"]?.ToString() ?? "TrungBinh",
                        AudioPath = row["AudioPath"] == DBNull.Value ? null : row["AudioPath"]?.ToString(),
                        DaKiemDuyet = row["DaKiemDuyet"] != DBNull.Value && Convert.ToBoolean(row["DaKiemDuyet"]),
                        NguoiTaoId = row["NguoiTaoId"] == DBNull.Value ? null : Convert.ToInt32(row["NguoiTaoId"]),
                        NgayTao = row["NgayTao"] == DBNull.Value ? null : Convert.ToDateTime(row["NgayTao"])
                    });
                }

                CauHoiDAL.NapDanhSachLuaChon(list);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BoCauHoiDAL.LayDanhSachCauHoiTheoBo] {ex.Message}");
            }

            return list;
        }

        /// <summary>
        /// Lấy toàn bộ câu hỏi trong hệ thống (Hỗ trợ tab "Tất cả câu hỏi")
        /// </summary>
        public List<CauHoiKhaoSatDTO> LayTatCaCauHoi(string? tuKhoa = null, string? kyNang = null, string? doKho = null, int top = 100)
        {
            var list = new List<CauHoiKhaoSatDTO>();
            string query = $@"
                SELECT TOP ({top}) ch.MaCauHoi, ROW_NUMBER() OVER(ORDER BY ch.MaCauHoi DESC) AS ThuTuCauHoi, 
                       ch.KyNang, td.MaCapDo AS MaTrinhDo, td.TenTrinhDo, ch.NoiDungCauHoi, ch.DoanVanDoc, 
                       ch.GiaiThichDapAn, ch.MaChuDe, ch.MaTrinhDo AS MaTrinhDoId, ch.LoaiCauHoi, ch.DoKho,
                       ch.DuongDanAudio AS AudioPath, ch.DaKiemDuyet, ch.NguoiTao AS NguoiTaoId, ch.NgayTao
                FROM CauHoi ch
                LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                WHERE (@TuKhoa IS NULL OR @TuKhoa = '' OR ch.NoiDungCauHoi LIKE N'%' + @TuKhoa + N'%')
                  AND (@KyNang IS NULL OR @KyNang = 'All' OR ch.KyNang = @KyNang)
                  AND (@DoKho IS NULL OR @DoKho = 'All' OR ch.DoKho = @DoKho)
                ORDER BY ch.MaCauHoi DESC";

            var parameters = new[]
            {
                new SqlParameter("@TuKhoa", string.IsNullOrWhiteSpace(tuKhoa) ? DBNull.Value : (object)tuKhoa.Trim()),
                new SqlParameter("@KyNang", string.IsNullOrWhiteSpace(kyNang) ? DBNull.Value : (object)kyNang.Trim()),
                new SqlParameter("@DoKho", string.IsNullOrWhiteSpace(doKho) ? DBNull.Value : (object)doKho.Trim())
            };

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                        ThuTuCauHoi = Convert.ToInt32(row["ThuTuCauHoi"]),
                        KyNang = row["KyNang"]?.ToString() ?? "Doc",
                        MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B1",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                        NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                        DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                        GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString(),
                        MaChuDe = row["MaChuDe"] == DBNull.Value ? null : Convert.ToInt32(row["MaChuDe"]),
                        MaTrinhDoId = row["MaTrinhDoId"] == DBNull.Value ? null : Convert.ToInt32(row["MaTrinhDoId"]),
                        LoaiCauHoi = row["LoaiCauHoi"]?.ToString() ?? "TracNghiem",
                        DoKho = row["DoKho"]?.ToString() ?? "TrungBinh",
                        AudioPath = row["AudioPath"] == DBNull.Value ? null : row["AudioPath"]?.ToString(),
                        DaKiemDuyet = row["DaKiemDuyet"] != DBNull.Value && Convert.ToBoolean(row["DaKiemDuyet"]),
                        NguoiTaoId = row["NguoiTaoId"] == DBNull.Value ? null : Convert.ToInt32(row["NguoiTaoId"]),
                        NgayTao = row["NgayTao"] == DBNull.Value ? null : Convert.ToDateTime(row["NgayTao"])
                    });
                }

                CauHoiDAL.NapDanhSachLuaChon(list);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BoCauHoiDAL.LayTatCaCauHoi] {ex.Message}");
            }

            return list;
        }

        /// <summary>
        /// Lưu một Bộ câu hỏi và danh sách câu hỏi thành viên bằng 1 SqlTransaction nguyên tử an toàn
        /// </summary>
        public bool LuuBoCauHoi(BoCauHoiDTO bo, List<CauHoiKhaoSatDTO> dsCauHoi, int nguoiTaoId, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (bo == null)
            {
                errorMessage = "Dữ liệu bộ câu hỏi không hợp lệ.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(bo.TenBoCauHoi))
            {
                errorMessage = "Vui lòng nhập tên bộ câu hỏi.";
                return false;
            }

            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                int maBo = bo.MaBoCauHoi;

                // 1. TẠO HOẶC CẬP NHẬT BỘ CÂU HỎI
                if (maBo <= 0)
                {
                    string insertBoSql = @"
                        INSERT INTO BoCauHoi (TenBoCauHoi, MoTa, NguonTao, TenNguon, MaBaiHoc, MaChuDe, MaTrinhDo, NguoiTao, NgayTao)
                        VALUES (@Ten, @MoTa, @NguonTao, @TenNguon, @MaBaiHoc, @MaChuDe, @MaTrinhDo, @NguoiTao, SYSDATETIME());
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using var cmdBo = new SqlCommand(insertBoSql, conn, trans);
                    cmdBo.Parameters.AddWithValue("@Ten", bo.TenBoCauHoi.Trim());
                    cmdBo.Parameters.AddWithValue("@MoTa", (object?)bo.MoTa ?? DBNull.Value);
                    cmdBo.Parameters.AddWithValue("@NguonTao", string.IsNullOrWhiteSpace(bo.NguonTao) ? "AI" : bo.NguonTao);
                    cmdBo.Parameters.AddWithValue("@TenNguon", (object?)bo.TenNguon ?? DBNull.Value);
                    cmdBo.Parameters.AddWithValue("@MaBaiHoc", (object?)bo.MaBaiHoc ?? DBNull.Value);
                    cmdBo.Parameters.AddWithValue("@MaChuDe", (object?)bo.MaChuDe ?? DBNull.Value);
                    cmdBo.Parameters.AddWithValue("@MaTrinhDo", (object?)bo.MaTrinhDo ?? DBNull.Value);
                    cmdBo.Parameters.AddWithValue("@NguoiTao", nguoiTaoId > 0 ? (object)nguoiTaoId : DBNull.Value);

                    maBo = (int)cmdBo.ExecuteScalar();
                    bo.MaBoCauHoi = maBo;
                }
                else
                {
                    string updateBoSql = @"
                        UPDATE BoCauHoi
                        SET TenBoCauHoi = @Ten,
                            MoTa = @MoTa,
                            TenNguon = @TenNguon,
                            NgayCapNhat = SYSDATETIME()
                        WHERE MaBoCauHoi = @MaBo;";

                    using var cmdBo = new SqlCommand(updateBoSql, conn, trans);
                    cmdBo.Parameters.AddWithValue("@MaBo", maBo);
                    cmdBo.Parameters.AddWithValue("@Ten", bo.TenBoCauHoi.Trim());
                    cmdBo.Parameters.AddWithValue("@MoTa", (object?)bo.MoTa ?? DBNull.Value);
                    cmdBo.Parameters.AddWithValue("@TenNguon", (object?)bo.TenNguon ?? DBNull.Value);
                    cmdBo.ExecuteNonQuery();
                }

                // 2. LƯU TỪNG CÂU HỎI VÀ LIÊN KẾT VÀO BỘ
                var newAssignedIds = new Dictionary<CauHoiKhaoSatDTO, int>();
                int thuTu = 1;
                foreach (var q in dsCauHoi)
                {
                    int maCauHoi = q.MaCauHoi;
                    bool tonTai = false;

                    if (maCauHoi > 0)
                    {
                        using var cmdCheck = new SqlCommand("SELECT COUNT(1) FROM CauHoi WHERE MaCauHoi = @id", conn, trans);
                        cmdCheck.Parameters.AddWithValue("@id", maCauHoi);
                        tonTai = Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0;
                    }

                    if (!tonTai)
                    {
                        // Thêm mới câu hỏi
                        string insertChSql = @"
                            INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, DoKho, 
                                                NoiDungCauHoi, DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, 
                                                DaKiemDuyet, NguoiTao, NguonTao, NgayTao)
                            VALUES (@MaChuDe, @MaTrinhDo, @MaBaiHoc, @LoaiCauHoi, @KyNang, @DoKho, 
                                    @NoiDung, @DoanVanDoc, @AudioPath, @GiaiThich, @LoiGiai, 
                                    @DaKiemDuyet, @NguoiTao, @NguonTao, SYSDATETIME());
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        using var cmdCh = new SqlCommand(insertChSql, conn, trans);
                        cmdCh.Parameters.AddWithValue("@MaChuDe", (object?)q.MaChuDe ?? (object?)bo.MaChuDe ?? DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@MaTrinhDo", (object?)q.MaTrinhDoId ?? (object?)bo.MaTrinhDo ?? DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@MaBaiHoc", (object?)bo.MaBaiHoc ?? DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@LoaiCauHoi", string.IsNullOrWhiteSpace(q.LoaiCauHoi) ? "TracNghiem" : q.LoaiCauHoi);
                        cmdCh.Parameters.AddWithValue("@KyNang", string.IsNullOrWhiteSpace(q.KyNang) ? "Doc" : q.KyNang);
                        cmdCh.Parameters.AddWithValue("@DoKho", string.IsNullOrWhiteSpace(q.DoKho) ? "TrungBinh" : q.DoKho);
                        cmdCh.Parameters.AddWithValue("@NoiDung", q.NoiDungCauHoi ?? "");
                        cmdCh.Parameters.AddWithValue("@DoanVanDoc", (object?)q.DoanVanDoc ?? DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@AudioPath", (object?)q.AudioPath ?? DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@GiaiThich", (object?)q.GiaiThichDapAn ?? DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@LoiGiai", (object?)q.GiaiThichDapAn ?? DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@DaKiemDuyet", q.DaKiemDuyet);
                        cmdCh.Parameters.AddWithValue("@NguoiTao", nguoiTaoId > 0 ? (object)nguoiTaoId : DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@NguonTao", bo.NguonTao ?? "AI");

                        maCauHoi = Convert.ToInt32(cmdCh.ExecuteScalar());
                        newAssignedIds[q] = maCauHoi;
                    }
                    else
                    {
                        // Cập nhật câu hỏi đã tồn tại
                        string updateChSql = @"
                            UPDATE CauHoi
                            SET LoaiCauHoi = @LoaiCauHoi,
                                KyNang = @KyNang,
                                DoKho = @DoKho,
                                NoiDungCauHoi = @NoiDung,
                                DoanVanDoc = @DoanVanDoc,
                                DuongDanAudio = @AudioPath,
                                GiaiThichDapAn = @GiaiThich,
                                LoiGiaiChiTiet = @LoiGiai,
                                DaKiemDuyet = @DaKiemDuyet
                            WHERE MaCauHoi = @MaCauHoi;
                            DELETE FROM LuaChonDapAn WHERE MaCauHoi = @MaCauHoi;";

                        using var cmdCh = new SqlCommand(updateChSql, conn, trans);
                        cmdCh.Parameters.AddWithValue("@MaCauHoi", maCauHoi);
                        cmdCh.Parameters.AddWithValue("@LoaiCauHoi", string.IsNullOrWhiteSpace(q.LoaiCauHoi) ? "TracNghiem" : q.LoaiCauHoi);
                        cmdCh.Parameters.AddWithValue("@KyNang", string.IsNullOrWhiteSpace(q.KyNang) ? "Doc" : q.KyNang);
                        cmdCh.Parameters.AddWithValue("@DoKho", string.IsNullOrWhiteSpace(q.DoKho) ? "TrungBinh" : q.DoKho);
                        cmdCh.Parameters.AddWithValue("@NoiDung", q.NoiDungCauHoi ?? "");
                        cmdCh.Parameters.AddWithValue("@DoanVanDoc", (object?)q.DoanVanDoc ?? DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@AudioPath", (object?)q.AudioPath ?? DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@GiaiThich", (object?)q.GiaiThichDapAn ?? DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@LoiGiai", (object?)q.GiaiThichDapAn ?? DBNull.Value);
                        cmdCh.Parameters.AddWithValue("@DaKiemDuyet", q.DaKiemDuyet);
                        cmdCh.ExecuteNonQuery();
                    }

                    // Lưu các lựa chọn đáp án
                    if (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 0)
                    {
                        string insertDaSql = @"
                            INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung)
                            VALUES (@MaCauHoi, @KyHieu, @NoiDung, @LaDapAnDung);";

                        foreach (var da in q.DanhSachLuaChon)
                        {
                            string kyHieu = string.IsNullOrWhiteSpace(da.KyHieuLuaChon) ? "A" : da.KyHieuLuaChon.Trim();
                            if (kyHieu.Length > 5) kyHieu = kyHieu.Substring(0, 5);

                            using var cmdDa = new SqlCommand(insertDaSql, conn, trans);
                            cmdDa.Parameters.AddWithValue("@MaCauHoi", maCauHoi);
                            cmdDa.Parameters.AddWithValue("@KyHieu", kyHieu);
                            cmdDa.Parameters.AddWithValue("@NoiDung", da.NoiDungLuaChon ?? "");
                            cmdDa.Parameters.AddWithValue("@LaDapAnDung", da.LaDapAnDung);
                            cmdDa.ExecuteNonQuery();
                        }
                    }

                    // Liên kết vào bảng BoCauHoi_CauHoi
                    string linkSql = @"
                        IF NOT EXISTS (SELECT 1 FROM BoCauHoi_CauHoi WHERE MaBoCauHoi = @MaBo AND MaCauHoi = @MaCH)
                        BEGIN
                            INSERT INTO BoCauHoi_CauHoi (MaBoCauHoi, MaCauHoi, ThuTu)
                            VALUES (@MaBo, @MaCH, @ThuTu);
                        END
                        ELSE
                        BEGIN
                            UPDATE BoCauHoi_CauHoi SET ThuTu = @ThuTu WHERE MaBoCauHoi = @MaBo AND MaCauHoi = @MaCH;
                        END";

                    using var cmdLink = new SqlCommand(linkSql, conn, trans);
                    cmdLink.Parameters.AddWithValue("@MaBo", maBo);
                    cmdLink.Parameters.AddWithValue("@MaCH", maCauHoi);
                    cmdLink.Parameters.AddWithValue("@ThuTu", thuTu++);
                    cmdLink.ExecuteNonQuery();
                }

                trans.Commit();

                // Gán lại MaCauHoi vào các đối tượng DTO khi transaction đã commit thành công
                foreach (var kvp in newAssignedIds)
                {
                    kvp.Key.MaCauHoi = kvp.Value;
                }

                bo.SoLuongCauHoi = dsCauHoi.Count;
                return true;
            }
            catch (Exception ex)
            {
                try { trans.Rollback(); } catch { }
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Đổi tên bộ câu hỏi
        /// </summary>
        public bool DoiTenBoCauHoi(int maBo, string tenMoi, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (maBo <= 0 || string.IsNullOrWhiteSpace(tenMoi))
            {
                errorMessage = "Thông tin không hợp lệ.";
                return false;
            }

            try
            {
                string sql = "UPDATE BoCauHoi SET TenBoCauHoi = @Ten, NgayCapNhat = SYSDATETIME() WHERE MaBoCauHoi = @MaBo";
                var parameters = new[]
                {
                    new SqlParameter("@MaBo", maBo),
                    new SqlParameter("@Ten", tenMoi.Trim())
                };

                int rows = SqlDataProvider.Instance.ExecuteNonQuery(sql, parameters);
                return rows > 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Xóa một bộ câu hỏi.
        /// Chú ý: Chỉ xóa bộ câu hỏi và các liên kết trong BoCauHoi_CauHoi (nhờ ON DELETE CASCADE).
        /// Bản ghi câu hỏi thực tế trong CauHoi KHÔNG bị xóa để bảo toàn dữ liệu cho các đề thi / bài khảo sát khác.
        /// </summary>
        public bool XoaBoCauHoi(int maBo, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (maBo <= 0)
            {
                errorMessage = "Mã bộ câu hỏi không hợp lệ.";
                return false;
            }

            try
            {
                string sql = "DELETE FROM BoCauHoi WHERE MaBoCauHoi = @MaBo";
                int rows = SqlDataProvider.Instance.ExecuteNonQuery(sql, new[] { new SqlParameter("@MaBo", maBo) });
                return rows > 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Gỡ câu hỏi ra khỏi bộ câu hỏi (chỉ xóa liên kết trong BoCauHoi_CauHoi, giữ nguyên bản ghi CauHoi)
        /// </summary>
        public bool XoaCauHoiKhoiBo(int maBo, int maCauHoi, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (maBo <= 0 || maCauHoi < 0)
            {
                errorMessage = "Dữ liệu không hợp lệ.";
                return false;
            }

            try
            {
                string sql = "DELETE FROM BoCauHoi_CauHoi WHERE MaBoCauHoi = @MaBo AND MaCauHoi = @MaCauHoi";
                int rows = SqlDataProvider.Instance.ExecuteNonQuery(sql, new[]
                {
                    new SqlParameter("@MaBo", maBo),
                    new SqlParameter("@MaCauHoi", maCauHoi)
                });
                return rows > 0;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Thêm câu hỏi đã có sẵn trong ngân hàng vào một bộ câu hỏi
        /// </summary>
        public bool ThemCauHoiVaoBo(int maBo, int maCauHoi, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                string sql = @"
                    IF NOT EXISTS (SELECT 1 FROM BoCauHoi_CauHoi WHERE MaBoCauHoi = @MaBo AND MaCauHoi = @MaCH)
                    BEGIN
                        DECLARE @MaxThuTu INT = (SELECT ISNULL(MAX(ThuTu), 0) FROM BoCauHoi_CauHoi WHERE MaBoCauHoi = @MaBo);
                        INSERT INTO BoCauHoi_CauHoi (MaBoCauHoi, MaCauHoi, ThuTu)
                        VALUES (@MaBo, @MaCH, @MaxThuTu + 1);
                    END";

                SqlDataProvider.Instance.ExecuteNonQuery(sql, new[]
                {
                    new SqlParameter("@MaBo", maBo),
                    new SqlParameter("@MaCH", maCauHoi)
                });
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Xóa câu hỏi vĩnh viễn khỏi toàn bộ hệ thống (có kiểm tra ràng buộc toàn vẹn)
        /// </summary>
        public bool XoaCauHoiVinhVien(int maCauHoi, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (maCauHoi < 0)
            {
                errorMessage = "Mã câu hỏi không hợp lệ.";
                return false;
            }

            // Kiểm tra xem câu hỏi có đang được sử dụng trong Đề thi hoặc Khảo sát không
            string checkSql = @"
                SELECT 
                    (SELECT COUNT(*) FROM CauHoiBaiTap WHERE MaCauHoi = @MaCH) AS SoDeThi,
                    (SELECT COUNT(*) FROM CauHoiKhaoSat WHERE MaCauHoi = @MaCH) AS SoKhaoSat";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(checkSql, new[] { new SqlParameter("@MaCH", maCauHoi) });
                if (dt.Rows.Count > 0)
                {
                    int soDe = Convert.ToInt32(dt.Rows[0]["SoDeThi"]);
                    int soKs = Convert.ToInt32(dt.Rows[0]["SoKhaoSat"]);

                    if (soDe > 0 || soKs > 0)
                    {
                        errorMessage = $"Không thể xóa câu hỏi này vì đang được dùng trong {soDe} đề thi và {soKs} bài khảo sát!";
                        return false;
                    }
                }

                // Thực hiện xóa an toàn
                using var conn = SqlDataProvider.Instance.GetConnection();
                conn.Open();
                using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    // 0. Xóa câu trả lời sinh viên & liên kết AI nếu có để bảo đảm toàn vẹn dữ liệu
                    using var cmd0 = new SqlCommand(@"
                        DELETE FROM CauTraLoiKhaoSat WHERE MaCauHoi = @MaCH OR MaLuaChonDaChon IN (SELECT MaLuaChon FROM LuaChonDapAn WHERE MaCauHoi = @MaCH);
                        DELETE FROM ChiTietTraLoi WHERE MaCauHoi = @MaCH OR MaLuaChonDaChon IN (SELECT MaLuaChon FROM LuaChonDapAn WHERE MaCauHoi = @MaCH);
                        DELETE FROM CauHoiDoAISinh WHERE MaCauHoi = @MaCH;
                        UPDATE PhienHoiThoaiAI SET MaCauHoiLienQuan = NULL WHERE MaCauHoiLienQuan = @MaCH;
                    ", conn, trans);
                    cmd0.Parameters.AddWithValue("@MaCH", maCauHoi);
                    cmd0.ExecuteNonQuery();

                    // 1. Xóa lựa chọn đáp án
                    using var cmd1 = new SqlCommand("DELETE FROM LuaChonDapAn WHERE MaCauHoi = @MaCH", conn, trans);
                    cmd1.Parameters.AddWithValue("@MaCH", maCauHoi);
                    cmd1.ExecuteNonQuery();

                    // 2. Xóa liên kết bộ câu hỏi
                    using var cmd2 = new SqlCommand("DELETE FROM BoCauHoi_CauHoi WHERE MaCauHoi = @MaCH", conn, trans);
                    cmd2.Parameters.AddWithValue("@MaCH", maCauHoi);
                    cmd2.ExecuteNonQuery();

                    // 3. Xóa bản ghi câu hỏi
                    using var cmd3 = new SqlCommand("DELETE FROM CauHoi WHERE MaCauHoi = @MaCH", conn, trans);
                    cmd3.Parameters.AddWithValue("@MaCH", maCauHoi);
                    int deleted = cmd3.ExecuteNonQuery();

                    trans.Commit();
                    return deleted > 0;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    errorMessage = ex.Message;
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
