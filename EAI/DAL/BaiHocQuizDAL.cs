using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class BaiHocQuizDAL
    {
        private static BaiHocQuizDAL? _instance;
        public static BaiHocQuizDAL Instance => _instance ??= new BaiHocQuizDAL();

        /// <summary>
        /// Lấy bộ câu hỏi đã lưu hoàn chỉnh trong CSDL theo MaBaiHoc, ContentHash và LoaiBoCauHoi ("CungCo" / "VanDung")
        /// </summary>
        public BaiHocQuizDTO? LayBoCauHoi(int maBaiHoc, string contentHash, string loaiBo)
        {
            if (maBaiHoc <= 0 || string.IsNullOrWhiteSpace(contentHash) || string.IsNullOrWhiteSpace(loaiBo))
                return null;

            try
            {
                const string queryBo = @"
                    SELECT TOP 1 MaBoCauHoi, TenBoCauHoi, MoTa, NguonTao, TenNguon, MaBaiHoc,
                           ContentHash, LoaiBoCauHoi, TrangThai, ThongBaoLoi, NgayTao, NgayCapNhat
                    FROM BoCauHoi
                    WHERE MaBaiHoc = @MaBaiHoc 
                      AND ContentHash = @ContentHash 
                      AND LoaiBoCauHoi = @LoaiBo 
                      AND TrangThai = N'HoanThanh'
                    ORDER BY MaBoCauHoi DESC;";

                DataTable dtBo = SqlDataProvider.Instance.ExecuteQuery(queryBo, new[]
                {
                    new SqlParameter("@MaBaiHoc", maBaiHoc),
                    new SqlParameter("@ContentHash", contentHash),
                    new SqlParameter("@LoaiBo", loaiBo)
                });

                if (dtBo.Rows.Count == 0) return null;

                var rowBo = dtBo.Rows[0];
                int maBo = Convert.ToInt32(rowBo["MaBoCauHoi"]);

                var quizDto = new BaiHocQuizDTO
                {
                    MaBoCauHoi = maBo,
                    MaBaiHoc = maBaiHoc,
                    ContentHash = contentHash,
                    LoaiBoCauHoi = loaiBo,
                    TenBoCauHoi = rowBo["TenBoCauHoi"]?.ToString() ?? "",
                    MoTa = rowBo["MoTa"] == DBNull.Value ? null : rowBo["MoTa"]?.ToString(),
                    TrangThai = rowBo["TrangThai"]?.ToString() ?? "HoanThanh",
                    NgayTao = rowBo["NgayTao"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(rowBo["NgayTao"]),
                    NgayCapNhat = rowBo["NgayCapNhat"] == DBNull.Value ? null : Convert.ToDateTime(rowBo["NgayCapNhat"])
                };

                // Lấy danh sách câu hỏi và các lựa chọn đáp án tương ứng
                const string queryQuestions = @"
                    SELECT bc.ThuTu, ch.MaCauHoi, ch.NoiDungCauHoi, ch.GiaiThichDapAn, ch.DoKho,
                           ch.LoaiCauHoi, ch.KyNang, ch.DoanVanDoc, ch.DuongDanAudio
                    FROM BoCauHoi_CauHoi bc
                    INNER JOIN CauHoi ch ON bc.MaCauHoi = ch.MaCauHoi
                    WHERE bc.MaBoCauHoi = @MaBo
                    ORDER BY bc.ThuTu ASC;";

                DataTable dtQuestions = SqlDataProvider.Instance.ExecuteQuery(queryQuestions, new[]
                {
                    new SqlParameter("@MaBo", maBo)
                });

                if (dtQuestions.Rows.Count < 10)
                {
                    // Bộ câu hỏi chưa đủ 10 câu hợp lệ
                    return null;
                }

                const string queryChoices = @"
                    SELECT lc.MaLuaChon, lc.MaCauHoi, lc.KyHieuLuaChon, lc.NoiDungLuaChon, lc.LaDapAnDung
                    FROM LuaChonDapAn lc
                    INNER JOIN BoCauHoi_CauHoi bc ON lc.MaCauHoi = bc.MaCauHoi
                    WHERE bc.MaBoCauHoi = @MaBo
                    ORDER BY lc.MaCauHoi, lc.KyHieuLuaChon ASC;";

                DataTable dtChoices = SqlDataProvider.Instance.ExecuteQuery(queryChoices, new[]
                {
                    new SqlParameter("@MaBo", maBo)
                });

                var choicesByQuestion = new Dictionary<int, List<DataRow>>();
                foreach (DataRow r in dtChoices.Rows)
                {
                    int qId = Convert.ToInt32(r["MaCauHoi"]);
                    if (!choicesByQuestion.ContainsKey(qId))
                        choicesByQuestion[qId] = new List<DataRow>();
                    choicesByQuestion[qId].Add(r);
                }

                foreach (DataRow qRow in dtQuestions.Rows)
                {
                    int qId = Convert.ToInt32(qRow["MaCauHoi"]);
                    var chDto = new CauHoiDTO
                    {
                        MaCauHoi = qId,
                        NoiDungCauHoi = qRow["NoiDungCauHoi"]?.ToString() ?? "",
                        GiaiThichChiTiet = qRow["GiaiThichDapAn"]?.ToString() ?? "",
                        DoKho = qRow["DoKho"]?.ToString() ?? "TrungBinh",
                        LoaiCauHoi = qRow["LoaiCauHoi"]?.ToString() ?? "TracNghiem",
                        KyNang = qRow["KyNang"]?.ToString() ?? "Doc",
                        DoanVanDoc = qRow["DoanVanDoc"] == DBNull.Value ? null : qRow["DoanVanDoc"]?.ToString(),
                        DanhSachLuaChon = new List<string>(),
                        DapAnDungIndex = null
                    };

                    if (choicesByQuestion.TryGetValue(qId, out var choiceRows))
                    {
                        for (int i = 0; i < choiceRows.Count; i++)
                        {
                            var cr = choiceRows[i];
                            string text = CauHoiDTO.StripOptionPrefix(cr["NoiDungLuaChon"]?.ToString() ?? "");
                            chDto.DanhSachLuaChon.Add(text);

                            bool isCorrect = cr["LaDapAnDung"] != DBNull.Value && Convert.ToBoolean(cr["LaDapAnDung"]);
                            if (isCorrect)
                            {
                                chDto.DapAnDungIndex = i;
                            }
                        }
                    }

                    quizDto.DanhSachCauHoi.Add(chDto);
                }

                return quizDto;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocQuizDAL.LayBoCauHoi] Lỗi: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra và giành quyền xử lý (Lease Lock) chống tạo trùng đồng thời giữa nhiều sinh viên
        /// </summary>
        public (AILeaseStatus Status, BaiHocQuizDTO? Data) KiemTraVaGiuKhoaXuLy(
            int maBaiHoc, 
            string contentHash, 
            string loaiBo, 
            string tenBo)
        {
            if (maBaiHoc < 0 || string.IsNullOrWhiteSpace(contentHash) || string.IsNullOrWhiteSpace(loaiBo))
                return (AILeaseStatus.LeaseAcquired, null);

            try
            {
                // Bước 1: Kiểm tra xem đã có bộ câu hỏi hoàn thành chưa
                var daCo = LayBoCauHoi(maBaiHoc, contentHash, loaiBo);
                if (daCo != null && daCo.DanhSachCauHoi.Count == 10)
                {
                    return (AILeaseStatus.AlreadyCompleted, daCo);
                }

                // Bước 2: Kiểm tra bản ghi trong DB (đang xử lý hay đã có)
                const string checkQuery = @"
                    SELECT TOP 1 MaBoCauHoi, TrangThai, 
                           DATEDIFF(SECOND, ISNULL(ThoiDiemBatDauXuLy, SYSDATETIME()), SYSDATETIME()) AS SecondsSinceStart
                    FROM BoCauHoi
                    WHERE MaBaiHoc = @MaBaiHoc AND ContentHash = @ContentHash AND LoaiBoCauHoi = @LoaiBo
                    ORDER BY MaBoCauHoi DESC;";

                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(checkQuery, new[]
                {
                    new SqlParameter("@MaBaiHoc", maBaiHoc),
                    new SqlParameter("@ContentHash", contentHash),
                    new SqlParameter("@LoaiBo", loaiBo)
                });

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    int maBo = Convert.ToInt32(row["MaBoCauHoi"]);
                    string trangThai = row["TrangThai"]?.ToString() ?? "";
                    int seconds = row["SecondsSinceStart"] == DBNull.Value ? 999 : Convert.ToInt32(row["SecondsSinceStart"]);

                    // Nếu tiến trình khác đang tạo câu hỏi trong vòng 45 giây
                    if (string.Equals(trangThai, "DangXuLy", StringComparison.OrdinalIgnoreCase) && seconds < 45)
                    {
                        return (AILeaseStatus.CurrentlyProcessing, null);
                    }

                    // Quá hạn hoặc chưa xong -> cập nhật lại DangXuLy
                    const string updateSql = @"
                        UPDATE BoCauHoi
                        SET TrangThai = N'DangXuLy',
                            ThoiDiemBatDauXuLy = SYSDATETIME(),
                            ThongBaoLoi = NULL
                        WHERE MaBoCauHoi = @MaBo;";

                    SqlDataProvider.Instance.ExecuteNonQuery(updateSql, new[] { new SqlParameter("@MaBo", maBo) });
                    return (AILeaseStatus.LeaseAcquired, null);
                }

                // Chưa có bản ghi nào -> INSERT bản ghi mới với trạng thái DangXuLy
                const string insertSql = @"
                    INSERT INTO BoCauHoi (
                        TenBoCauHoi, NguonTao, MaBaiHoc, ContentHash, LoaiBoCauHoi, TrangThai, ThoiDiemBatDauXuLy, NgayTao
                    ) VALUES (
                        @TenBo, N'AI', @MaBaiHoc, @ContentHash, @LoaiBo, N'DangXuLy', SYSDATETIME(), SYSDATETIME()
                    );";

                SqlDataProvider.Instance.ExecuteNonQuery(insertSql, new[]
                {
                    new SqlParameter("@TenBo", tenBo),
                    new SqlParameter("@MaBaiHoc", maBaiHoc),
                    new SqlParameter("@ContentHash", contentHash),
                    new SqlParameter("@LoaiBo", loaiBo)
                });

                return (AILeaseStatus.LeaseAcquired, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocQuizDAL.KiemTraVaGiuKhoaXuLy] Lỗi: {ex.Message}");
                return (AILeaseStatus.LeaseAcquired, null);
            }
        }

        /// <summary>
        /// Lưu bộ câu hỏi và danh sách 10 câu hỏi vào Database trong một SqlTransaction an toàn tuyệt đối
        /// </summary>
        public bool LuuBoCauHoiVaCauHoi(
            int maBaiHoc,
            string contentHash,
            string loaiBo,
            string tenBo,
            string? moTa,
            List<CauHoiDTO> questions,
            string? kyNang = "Doc",
            string? trinhDo = "B1",
            int? nguoiTaoId = null)
        {
            if (maBaiHoc < 0 || questions == null || questions.Count < 10)
                return false;

            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction();

            try
            {
                // 1. Tìm hoặc tạo bản ghi BoCauHoi
                int maBoCauHoi;
                const string queryFindBo = @"
                    SELECT TOP 1 MaBoCauHoi FROM BoCauHoi WITH (UPDLOCK)
                    WHERE MaBaiHoc = @MaBaiHoc AND ContentHash = @ContentHash AND LoaiBoCauHoi = @LoaiBo;";

                using (var cmdFind = new SqlCommand(queryFindBo, conn, trans))
                {
                    cmdFind.Parameters.AddWithValue("@MaBaiHoc", maBaiHoc);
                    cmdFind.Parameters.AddWithValue("@ContentHash", contentHash);
                    cmdFind.Parameters.AddWithValue("@LoaiBo", loaiBo);
                    object? res = cmdFind.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        maBoCauHoi = Convert.ToInt32(res);
                        const string updateBo = @"
                            UPDATE BoCauHoi
                            SET TenBoCauHoi = @TenBo,
                                MoTa = @MoTa,
                                TrangThai = N'HoanThanh',
                                ThongBaoLoi = NULL,
                                NgayCapNhat = SYSDATETIME()
                            WHERE MaBoCauHoi = @MaBo;";

                        using var cmdUp = new SqlCommand(updateBo, conn, trans);
                        cmdUp.Parameters.AddWithValue("@TenBo", tenBo);
                        cmdUp.Parameters.AddWithValue("@MoTa", (object?)moTa ?? DBNull.Value);
                        cmdUp.Parameters.AddWithValue("@MaBo", maBoCauHoi);
                        cmdUp.ExecuteNonQuery();

                        // Xóa các liên kết câu hỏi cũ nếu có để tránh trùng lặp
                        const string deleteOldLinks = "DELETE FROM BoCauHoi_CauHoi WHERE MaBoCauHoi = @MaBo;";
                        using var cmdDel = new SqlCommand(deleteOldLinks, conn, trans);
                        cmdDel.Parameters.AddWithValue("@MaBo", maBoCauHoi);
                        cmdDel.ExecuteNonQuery();
                    }
                    else
                    {
                        const string insertBo = @"
                            INSERT INTO BoCauHoi (
                                TenBoCauHoi, MoTa, NguonTao, TenNguon, MaBaiHoc,
                                ContentHash, LoaiBoCauHoi, TrangThai, NgayTao, NgayCapNhat
                            ) VALUES (
                                @TenBo, @MoTa, N'AI', @TenBo, @MaBaiHoc,
                                @ContentHash, @LoaiBo, N'HoanThanh', SYSDATETIME(), SYSDATETIME()
                            );
                            SELECT SCOPE_IDENTITY();";

                        using var cmdIns = new SqlCommand(insertBo, conn, trans);
                        cmdIns.Parameters.AddWithValue("@TenBo", tenBo);
                        cmdIns.Parameters.AddWithValue("@MoTa", (object?)moTa ?? DBNull.Value);
                        cmdIns.Parameters.AddWithValue("@MaBaiHoc", maBaiHoc);
                        cmdIns.Parameters.AddWithValue("@ContentHash", contentHash);
                        cmdIns.Parameters.AddWithValue("@LoaiBo", loaiBo);
                        maBoCauHoi = Convert.ToInt32(cmdIns.ExecuteScalar());
                    }
                }

                // 2. Chèn 10 câu hỏi và các phương án tương ứng
                for (int i = 0; i < questions.Count; i++)
                {
                    var q = questions[i];
                    int thuTu = i + 1;

                    const string insertQ = @"
                        INSERT INTO CauHoi (
                            MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn,
                            DoKho, LoiGiaiChiTiet, NguoiTao, NguonTao, DaKiemDuyet,
                            DoanVanDoc, DuongDanAudio, NgayTao
                        ) VALUES (
                            @MaBaiHoc, @LoaiCauHoi, @KyNang, @NoiDung, @GiaiThich,
                            @DoKho, @LoiGiai, @NguoiTao, N'AI', 1,
                            @DoanVan, @Audio, SYSDATETIME()
                        );
                        SELECT SCOPE_IDENTITY();";

                    int maCauHoi;
                    using (var cmdQ = new SqlCommand(insertQ, conn, trans))
                    {
                        cmdQ.Parameters.AddWithValue("@MaBaiHoc", maBaiHoc);
                        cmdQ.Parameters.AddWithValue("@LoaiCauHoi", string.IsNullOrWhiteSpace(q.LoaiCauHoi) ? "TracNghiem" : q.LoaiCauHoi);
                        cmdQ.Parameters.AddWithValue("@KyNang", string.IsNullOrWhiteSpace(q.KyNang) ? (kyNang ?? "Doc") : q.KyNang);
                        cmdQ.Parameters.AddWithValue("@NoiDung", q.NoiDungCauHoi ?? "");
                        cmdQ.Parameters.AddWithValue("@GiaiThich", (object?)q.GiaiThichChiTiet ?? DBNull.Value);
                        cmdQ.Parameters.AddWithValue("@DoKho", string.IsNullOrWhiteSpace(q.DoKho) ? (thuTu <= 3 ? "De" : (thuTu >= 8 ? "Kho" : "TrungBinh")) : q.DoKho);
                        cmdQ.Parameters.AddWithValue("@LoiGiai", (object?)q.GiaiThichChiTiet ?? DBNull.Value);
                        cmdQ.Parameters.AddWithValue("@NguoiTao", (object?)nguoiTaoId ?? DBNull.Value);
                        cmdQ.Parameters.AddWithValue("@DoanVan", (object?)q.DoanVanDoc ?? DBNull.Value);
                        cmdQ.Parameters.AddWithValue("@Audio", (object?)q.AudioScript ?? DBNull.Value);

                        maCauHoi = Convert.ToInt32(cmdQ.ExecuteScalar());
                        q.MaCauHoi = maCauHoi;
                    }

                    // Chèn 4 phương án lựa chọn A, B, C, D
                    if (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 0)
                    {
                        for (int optIdx = 0; optIdx < q.DanhSachLuaChon.Count; optIdx++)
                        {
                            string optText = CauHoiDTO.StripOptionPrefix(q.DanhSachLuaChon[optIdx]?.Trim() ?? "");
                            string kyHieu = ((char)('A' + optIdx)).ToString();
                            bool isCorrect = (q.DapAnDungIndex.HasValue && q.DapAnDungIndex.Value == optIdx);

                            const string insertOpt = @"
                                INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung)
                                VALUES (@MaCauHoi, @KyHieu, @NoiDung, @LaDapAnDung);";

                            using var cmdOpt = new SqlCommand(insertOpt, conn, trans);
                            cmdOpt.Parameters.AddWithValue("@MaCauHoi", maCauHoi);
                            cmdOpt.Parameters.AddWithValue("@KyHieu", kyHieu);
                            cmdOpt.Parameters.AddWithValue("@NoiDung", optText);
                            cmdOpt.Parameters.AddWithValue("@LaDapAnDung", isCorrect);
                            cmdOpt.ExecuteNonQuery();
                        }
                    }

                    // Chèn liên kết BoCauHoi_CauHoi
                    const string insertLink = @"
                        INSERT INTO BoCauHoi_CauHoi (MaBoCauHoi, MaCauHoi, ThuTu)
                        VALUES (@MaBo, @MaCauHoi, @ThuTu);";

                    using (var cmdLink = new SqlCommand(insertLink, conn, trans))
                    {
                        cmdLink.Parameters.AddWithValue("@MaBo", maBoCauHoi);
                        cmdLink.Parameters.AddWithValue("@MaCauHoi", maCauHoi);
                        cmdLink.Parameters.AddWithValue("@ThuTu", thuTu);
                        cmdLink.ExecuteNonQuery();
                    }
                }

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                System.Diagnostics.Debug.WriteLine($"[BaiHocQuizDAL.LuuBoCauHoiVaCauHoi] Lỗi Transaction: {ex.Message}");
                DanhDauThatBai(maBaiHoc, contentHash, loaiBo, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Đánh dấu trạng thái lỗi khi sinh câu hỏi AI thất bại
        /// </summary>
        public void DanhDauThatBai(int maBaiHoc, string contentHash, string loaiBo, string errorMessage)
        {
            try
            {
                const string updateSql = @"
                    UPDATE BoCauHoi
                    SET TrangThai = N'ThatBai',
                        ThongBaoLoi = @ErrorMessage,
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaBaiHoc = @MaBaiHoc AND ContentHash = @ContentHash AND LoaiBoCauHoi = @LoaiBo;";

                SqlDataProvider.Instance.ExecuteNonQuery(updateSql, new[]
                {
                    new SqlParameter("@ErrorMessage", errorMessage),
                    new SqlParameter("@MaBaiHoc", maBaiHoc),
                    new SqlParameter("@ContentHash", contentHash),
                    new SqlParameter("@LoaiBo", loaiBo)
                });
            }
            catch { }
        }

        /// <summary>
        /// Lấy kết quả làm bài gần nhất của một sinh viên đối với bộ câu hỏi cụ thể
        /// </summary>
        public KetQuaLamBaiBoCauHoiDTO? LayKetQuaLamBaiCuaSinhVien(
            int maNguoiDung, 
            int maBaiHoc, 
            string contentHash, 
            string loaiBo)
        {
            if (maNguoiDung <= 0 || maBaiHoc < 0 || string.IsNullOrWhiteSpace(contentHash) || string.IsNullOrWhiteSpace(loaiBo))
                return null;

            try
            {
                const string query = @"
                    SELECT TOP 1 MaKetQua, MaNguoiDung, MaBoCauHoi, MaBaiHoc, LoaiBoCauHoi,
                           ContentHash, TongSoCau, SoCauDung, DiemSo, ThoiGianLamGiay,
                           TrangThai, ChiTietTraLoiJson, NgayNop
                    FROM KetQuaLamBaiBoCauHoi
                    WHERE MaNguoiDung = @MaNguoiDung 
                      AND MaBaiHoc = @MaBaiHoc 
                      AND LoaiBoCauHoi = @LoaiBo 
                      AND ContentHash = @ContentHash
                    ORDER BY MaKetQua DESC;";

                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[]
                {
                    new SqlParameter("@MaNguoiDung", maNguoiDung),
                    new SqlParameter("@MaBaiHoc", maBaiHoc),
                    new SqlParameter("@LoaiBo", loaiBo),
                    new SqlParameter("@ContentHash", contentHash)
                });

                if (dt.Rows.Count > 0)
                {
                    var r = dt.Rows[0];
                    return new KetQuaLamBaiBoCauHoiDTO
                    {
                        MaKetQua = Convert.ToInt32(r["MaKetQua"]),
                        MaNguoiDung = Convert.ToInt32(r["MaNguoiDung"]),
                        MaBoCauHoi = Convert.ToInt32(r["MaBoCauHoi"]),
                        MaBaiHoc = Convert.ToInt32(r["MaBaiHoc"]),
                        LoaiBoCauHoi = r["LoaiBoCauHoi"]?.ToString() ?? loaiBo,
                        ContentHash = r["ContentHash"]?.ToString() ?? contentHash,
                        TongSoCau = Convert.ToInt32(r["TongSoCau"]),
                        SoCauDung = Convert.ToInt32(r["SoCauDung"]),
                        DiemSo = Convert.ToDecimal(r["DiemSo"]),
                        ThoiGianLamGiay = Convert.ToInt32(r["ThoiGianLamGiay"]),
                        TrangThai = r["TrangThai"]?.ToString() ?? "DaNop",
                        ChiTietTraLoiJson = r["ChiTietTraLoiJson"] == DBNull.Value ? null : r["ChiTietTraLoiJson"]?.ToString(),
                        NgayNop = Convert.ToDateTime(r["NgayNop"])
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocQuizDAL.LayKetQuaLamBaiCuaSinhVien] Lỗi: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Lưu kết quả làm bài độc lập của sinh viên vào CSDL
        /// </summary>
        public bool LuuKetQuaLamBai(KetQuaLamBaiBoCauHoiDTO ketQua)
        {
            if (ketQua == null || ketQua.MaNguoiDung <= 0 || ketQua.MaBaiHoc < 0 || ketQua.MaBoCauHoi <= 0)
                return false;

            try
            {
                const string insertSql = @"
                    INSERT INTO KetQuaLamBaiBoCauHoi (
                        MaNguoiDung, MaBoCauHoi, MaBaiHoc, LoaiBoCauHoi, ContentHash,
                        TongSoCau, SoCauDung, DiemSo, ThoiGianLamGiay, TrangThai,
                        ChiTietTraLoiJson, NgayNop
                    ) VALUES (
                        @MaNguoiDung, @MaBoCauHoi, @MaBaiHoc, @LoaiBo, @ContentHash,
                        @TongSoCau, @SoCauDung, @DiemSo, @ThoiGianLamGiay, @TrangThai,
                        @ChiTietJson, SYSDATETIME()
                    );";

                int rows = SqlDataProvider.Instance.ExecuteNonQuery(insertSql, new[]
                {
                    new SqlParameter("@MaNguoiDung", ketQua.MaNguoiDung),
                    new SqlParameter("@MaBoCauHoi", ketQua.MaBoCauHoi),
                    new SqlParameter("@MaBaiHoc", ketQua.MaBaiHoc),
                    new SqlParameter("@LoaiBo", ketQua.LoaiBoCauHoi),
                    new SqlParameter("@ContentHash", ketQua.ContentHash),
                    new SqlParameter("@TongSoCau", ketQua.TongSoCau),
                    new SqlParameter("@SoCauDung", ketQua.SoCauDung),
                    new SqlParameter("@DiemSo", ketQua.DiemSo),
                    new SqlParameter("@ThoiGianLamGiay", ketQua.ThoiGianLamGiay),
                    new SqlParameter("@TrangThai", ketQua.TrangThai),
                    new SqlParameter("@ChiTietJson", (object?)ketQua.ChiTietTraLoiJson ?? DBNull.Value)
                });

                return rows > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocQuizDAL.LuuKetQuaLamBai] Lỗi: {ex.Message}");
                return false;
            }
        }
    }
}
