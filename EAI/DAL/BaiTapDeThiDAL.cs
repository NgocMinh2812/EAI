using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class BaiTapDeThiDAL
    {
        private static BaiTapDeThiDAL? _instance;
        public static BaiTapDeThiDAL Instance => _instance ??= new BaiTapDeThiDAL();

        public static List<BaiTapDeThiDTO> TaoDanhSachDeThiChuanHoaTatCaCapDo()
        {
            return new List<BaiTapDeThiDTO>
            {
                // CẤP ĐỘ A1 - BEGINNER (CĂN BẢN)
                new BaiTapDeThiDTO
                {
                    MaBaiTapDeThi = 101,
                    TenBaiTapDeThi = "A1 Starter Diagnostic: Phản Xạ Từ Vựng & Ngữ Pháp Căn Bản",
                    LoaiHinh = "ThiThu",
                    MaCapDo = "A1",
                    TenTrinhDo = "Cơ bản",
                    TenChuDe = "Giao tiếp hàng ngày",
                    ThoiGianLamBaiPhut = 20,
                    SoLuongCauHoi = 10,
                    MoTa = "Kiểm tra phản xạ từ vựng cơ bản, động từ To Be, thì hiện tại đơn và câu chào hỏi thường nhật.",
                    DaPhatHanh = true
                },
                new BaiTapDeThiDTO
                {
                    MaBaiTapDeThi = 102,
                    TenBaiTapDeThi = "A1 Daily English: Giao Tiếp Đời Sống & Nhận Diện Ngữ Pháp",
                    LoaiHinh = "ThiThu",
                    MaCapDo = "A1",
                    TenTrinhDo = "Cơ bản",
                    TenChuDe = "Đời sống",
                    ThoiGianLamBaiPhut = 25,
                    SoLuongCauHoi = 10,
                    MoTa = "Đánh giá khả năng hiểu đoạn văn ngắn, thói quen sinh hoạt và chỉ dẫn đơn giản.",
                    DaPhatHanh = true
                },

                // CẤP ĐỘ A2 - ELEMENTARY (SƠ CẤP)
                new BaiTapDeThiDTO
                {
                    MaBaiTapDeThi = 201,
                    TenBaiTapDeThi = "A2 Elementary Reading & Grammar: Thì Quá Khứ & So Sánh",
                    LoaiHinh = "ThiThu",
                    MaCapDo = "A2",
                    TenTrinhDo = "Sơ cấp",
                    TenChuDe = "Học thuật & Đời sống",
                    ThoiGianLamBaiPhut = 30,
                    SoLuongCauHoi = 10,
                    MoTa = "Part 5-6 định dạng TOEIC Bridge: Động từ bất quy tắc, thì quá khứ đơn và cấu trúc so sánh.",
                    DaPhatHanh = true
                },
                new BaiTapDeThiDTO
                {
                    MaBaiTapDeThi = 202,
                    TenBaiTapDeThi = "A2 Practical English Test: Giao Tiếp Thương Mại Cơ Bản",
                    LoaiHinh = "ThiThu",
                    MaCapDo = "A2",
                    TenTrinhDo = "Sơ cấp",
                    TenChuDe = "Thương mại",
                    ThoiGianLamBaiPhut = 25,
                    SoLuongCauHoi = 10,
                    MoTa = "Bài thi trắc nghiệm tình huống đặt hẹn, mua sắm và đọc thông báo văn phòng ngắn gọn.",
                    DaPhatHanh = true
                },

                // CẤP ĐỘ B1 - INTERMEDIATE (TRUNG CẤP)
                new BaiTapDeThiDTO
                {
                    MaBaiTapDeThi = 301,
                    TenBaiTapDeThi = "TOEIC Reading Comprehension Diagnostic Test",
                    LoaiHinh = "ThiThu",
                    MaCapDo = "B1",
                    TenTrinhDo = "Trung cấp",
                    TenChuDe = "TOEIC Format",
                    ThoiGianLamBaiPhut = 45,
                    SoLuongCauHoi = 10,
                    MoTa = "Part 5 (Hoàn thành câu), Part 6 (Điền đoạn văn) và Part 7 (Đọc hiểu văn bản thương mại).",
                    DaPhatHanh = true
                },
                new BaiTapDeThiDTO
                {
                    MaBaiTapDeThi = 302,
                    TenBaiTapDeThi = "B1 Intermediate Skills: Mệnh Đề Quan Hệ & Thì Hoàn Thành",
                    LoaiHinh = "ThiThu",
                    MaCapDo = "B1",
                    TenTrinhDo = "Trung cấp",
                    TenChuDe = "Ngữ pháp trọng tâm",
                    ThoiGianLamBaiPhut = 30,
                    SoLuongCauHoi = 10,
                    MoTa = "Củng cố các cấu trúc thì hiện tại hoàn thành, giới từ phụ thuộc và đại từ quan hệ.",
                    DaPhatHanh = true
                },

                // CẤP ĐỘ B2 - UPPER-INTERMEDIATE (TRUNG CAO CẤP)
                new BaiTapDeThiDTO
                {
                    MaBaiTapDeThi = 401,
                    TenBaiTapDeThi = "IELTS Academic Reading & Listening Mini Mock Test #05",
                    LoaiHinh = "ThiThu",
                    MaCapDo = "B2",
                    TenTrinhDo = "Trung cao cấp",
                    TenChuDe = "IELTS Academic",
                    ThoiGianLamBaiPhut = 40,
                    SoLuongCauHoi = 10,
                    MoTa = "Bao gồm bài đọc Skimming/Scanning, phân tích từ vựng học thuật C1 và cấu trúc đảo ngữ.",
                    DaPhatHanh = true
                },
                new BaiTapDeThiDTO
                {
                    MaBaiTapDeThi = 402,
                    TenBaiTapDeThi = "Kiểm tra Đột phá Ngữ pháp & Collocations Band B2",
                    LoaiHinh = "ThiThu",
                    MaCapDo = "B2",
                    TenTrinhDo = "Trung cao cấp",
                    TenChuDe = "Ngữ pháp học thuật",
                    ThoiGianLamBaiPhut = 25,
                    SoLuongCauHoi = 10,
                    MoTa = "25 câu trắc nghiệm câu điều kiện đảo ngữ, mệnh đề quan hệ rút gọn, liên từ nâng cao.",
                    DaPhatHanh = true
                },

                // CẤP ĐỘ C1 - ADVANCED (NÂNG CAO)
                new BaiTapDeThiDTO
                {
                    MaBaiTapDeThi = 501,
                    TenBaiTapDeThi = "IELTS Advanced Academic Mock Test: Phân Tích Luận & Tư Duy Phản Biện",
                    LoaiHinh = "ThiThu",
                    MaCapDo = "C1",
                    TenTrinhDo = "Nâng cao",
                    TenChuDe = "IELTS Academic Band 7.5+",
                    ThoiGianLamBaiPhut = 45,
                    SoLuongCauHoi = 10,
                    MoTa = "Bài thi phân tích học thuật đỉnh cao: Đoạn văn nghiên cứu khoa học, cấu trúc câu phức và từ vựng C1-C2.",
                    DaPhatHanh = true
                },
                new BaiTapDeThiDTO
                {
                    MaBaiTapDeThi = 502,
                    TenBaiTapDeThi = "C1 Lexical Resource & Inversion Mastery Test",
                    LoaiHinh = "ThiThu",
                    MaCapDo = "C1",
                    TenTrinhDo = "Nâng cao",
                    TenChuDe = "Ngữ pháp chuyên sâu",
                    ThoiGianLamBaiPhut = 35,
                    SoLuongCauHoi = 10,
                    MoTa = "Chinh phục cấu trúc đảo ngữ phức hợp, cụm từ cố định (collocations) và thành ngữ học thuật C1.",
                    DaPhatHanh = true
                }
            };
        }

        public List<BaiTapDeThiDTO> LayDanhSachDeThi(string? loaiHinh = null, string? trinhDo = null)
        {
            var list = new List<BaiTapDeThiDTO>();
            string query = @"
                SELECT dt.MaBaiTapDeThi, dt.MaBaiHoc, dt.MaChuDe, dt.MaTrinhDo,
                       dt.TenBaiTapDeThi, dt.LoaiHinh, dt.MoTa, dt.ThoiGianLamBaiPhut, dt.DaPhatHanh,
                       td.MaCapDo, td.TenTrinhDo, cd.TenChuDe,
                       (SELECT COUNT(*) FROM CauHoiBaiTap chbt WHERE chbt.MaBaiTapDeThi = dt.MaBaiTapDeThi) AS SoLuongCauHoi
                FROM BaiTapDeThi dt
                LEFT JOIN TrinhDo td ON dt.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON dt.MaChuDe = cd.MaChuDe
                WHERE (@LoaiHinh IS NULL OR dt.LoaiHinh = @LoaiHinh)
                  AND (@TrinhDo IS NULL OR td.MaCapDo = @TrinhDo)
                ORDER BY dt.MaBaiTapDeThi ASC";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@LoaiHinh", (object?)loaiHinh ?? DBNull.Value),
                new SqlParameter("@TrinhDo", (object?)trinhDo ?? DBNull.Value)
            };

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, parameters);
                foreach (DataRow row in dt.Rows)
                {
                    int soCau = Convert.ToInt32(row["SoLuongCauHoi"]);
                    if (soCau == 0) soCau = 10;

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
                        DaPhatHanh = row["DaPhatHanh"] == DBNull.Value || Convert.ToBoolean(row["DaPhatHanh"]),
                        MaCapDo = row["MaCapDo"]?.ToString() ?? "B2",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "Trung cấp",
                        TenChuDe = row["TenChuDe"]?.ToString() ?? "Học thuật",
                        SoLuongCauHoi = soCau
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lấy danh sách đề: {ex.Message}");
            }

            // Đảm bảo luôn có đề thi cho tất cả các cấp độ A1, A2, B1, B2, C1
            var standards = TaoDanhSachDeThiChuanHoaTatCaCapDo();
            var existingLevels = list.Select(x => x.MaCapDo?.ToUpper()).Where(x => !string.IsNullOrEmpty(x)).ToHashSet();

            foreach (var std in standards)
            {
                // Nếu DB chưa có đề thi cho cấp độ này hoặc danh sách DB trống, bổ sung đề chuẩn hóa
                if (!existingLevels.Contains(std.MaCapDo.ToUpper()))
                {
                    list.Add(std);
                }
            }

            // Nếu người dùng lọc theo trình độ hoặc loại hình
            if (!string.IsNullOrWhiteSpace(trinhDo) && trinhDo != "All")
            {
                list = list.Where(x => string.Equals(x.MaCapDo, trinhDo, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(loaiHinh) && loaiHinh != "All")
            {
                list = list.Where(x => string.Equals(x.LoaiHinh, loaiHinh, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return list;
        }

        public BaiTapDeThiDTO? LayChiTietDeThi(int maDeThi)
        {
            string query = @"
                SELECT dt.MaBaiTapDeThi, dt.MaBaiHoc, dt.MaChuDe, dt.MaTrinhDo,
                       dt.TenBaiTapDeThi, dt.LoaiHinh, dt.MoTa, dt.ThoiGianLamBaiPhut, dt.DaPhatHanh,
                       td.MaCapDo, td.TenTrinhDo, cd.TenChuDe,
                       (SELECT COUNT(*) FROM CauHoiBaiTap chbt WHERE chbt.MaBaiTapDeThi = dt.MaBaiTapDeThi) AS SoLuongCauHoi
                FROM BaiTapDeThi dt
                LEFT JOIN TrinhDo td ON dt.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON dt.MaChuDe = cd.MaChuDe
                WHERE dt.MaBaiTapDeThi = @MaDe";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaDe", maDeThi) });
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    int soCau = Convert.ToInt32(row["SoLuongCauHoi"]);
                    if (soCau == 0) soCau = 10;

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
                        DaPhatHanh = row["DaPhatHanh"] == DBNull.Value || Convert.ToBoolean(row["DaPhatHanh"]),
                        MaCapDo = row["MaCapDo"]?.ToString() ?? "B2",
                        TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "Trung cấp",
                        TenChuDe = row["TenChuDe"]?.ToString() ?? "Học thuật",
                        SoLuongCauHoi = soCau
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lấy chi tiết đề: {ex.Message}");
            }

            // Fallback từ danh sách chuẩn hóa
            return TaoDanhSachDeThiChuanHoaTatCaCapDo().FirstOrDefault(x => x.MaBaiTapDeThi == maDeThi);
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiCuaDeThi(int maDeThi)
        {
            var list = new List<CauHoiKhaoSatDTO>();

            try
            {
                string query = @"
                    SELECT ch.MaCauHoi, chbt.ThuTuCauHoi, ch.KyNang, td.MaCapDo AS MaTrinhDo,
                           td.TenTrinhDo, ch.NoiDungCauHoi, ch.DoanVanDoc, ch.DuongDanAudio, ch.GiaiThichDapAn
                    FROM CauHoiBaiTap chbt
                    INNER JOIN CauHoi ch ON chbt.MaCauHoi = ch.MaCauHoi
                    LEFT JOIN TrinhDo td ON ch.MaTrinhDo = td.MaTrinhDo
                    WHERE chbt.MaBaiTapDeThi = @MaDe
                    ORDER BY chbt.ThuTuCauHoi ASC";

                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaDe", maDeThi) });

                if (dt.Rows.Count > 0)
                {
                    int thuTu = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(new CauHoiKhaoSatDTO
                        {
                            MaCauHoi = Convert.ToInt32(row["MaCauHoi"]),
                            ThuTuCauHoi = thuTu++,
                            KyNang = row["KyNang"]?.ToString() ?? "NguPhap",
                            MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B2",
                            TenTrinhDo = row["TenTrinhDo"]?.ToString() ?? "",
                            NoiDungCauHoi = row["NoiDungCauHoi"]?.ToString() ?? "",
                            DoanVanDoc = row["DoanVanDoc"] == DBNull.Value ? null : row["DoanVanDoc"]?.ToString(),
                            AudioPath = row["DuongDanAudio"] == DBNull.Value ? null : row["DuongDanAudio"]?.ToString(),
                            GiaiThichDapAn = row["GiaiThichDapAn"] == DBNull.Value ? null : row["GiaiThichDapAn"]?.ToString()
                        });
                    }

                    CauHoiDAL.NapDanhSachLuaChon(list);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL.LayDanhSachCauHoiCuaDeThi] {ex.Message}");
            }

            // Nếu DB chưa có câu hỏi hoặc không đủ câu hỏi có đáp án -> Tự động sinh bộ câu hỏi chuẩn hóa theo cấp độ đề
            if (list.Count == 0 || list.Any(q => q.DanhSachLuaChon == null || q.DanhSachLuaChon.Count == 0))
            {
                var de = LayChiTietDeThi(maDeThi);
                string capDo = de?.MaCapDo ?? "B1";
                list = TaoBoCauHoiDeThiChuanTheoCapDo(capDo);
                LuuCauHoiChuanVaoCSDLNeuChuaCo(maDeThi, capDo, list);
            }

            return list;
        }

        private void LuuCauHoiChuanVaoCSDLNeuChuaCo(int maDeThi, string capDo, List<CauHoiKhaoSatDTO> questions)
        {
            try
            {
                int validMaDe = EnsureDeThiExists(maDeThi, null, capDo);
                object? checkCount = SqlDataProvider.Instance.ExecuteScalar(
                    "SELECT COUNT(1) FROM CauHoiBaiTap WHERE MaBaiTapDeThi = @MaDe",
                    new[] { new SqlParameter("@MaDe", validMaDe) });
                if (checkCount != null && Convert.ToInt32(checkCount) > 0)
                {
                    return;
                }

                object? tdObj = SqlDataProvider.Instance.ExecuteScalar(
                    "SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = @CapDo",
                    new[] { new SqlParameter("@CapDo", capDo) });

                int thuTu = 1;
                foreach (var q in questions)
                {
                    int dbMaCauHoi = 0;
                    try
                    {
                        string insertCH = @"
                            INSERT INTO CauHoi (NoiDungCauHoi, LoaiCauHoi, KyNang, GiaiThichDapAn, DoanVanDoc, DoKho, DaKiemDuyet, MaTrinhDo, NgayTao)
                            VALUES (@NoiDung, N'TracNghiem', @KyNang, @GiaiThich, @DoanVan, N'TrungBinh', 1, @MaTD, SYSDATETIME());
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        SqlParameter[] pCH = new[]
                        {
                            new SqlParameter("@NoiDung", q.NoiDungCauHoi),
                            new SqlParameter("@KyNang", q.KyNang ?? "NguPhap"),
                            new SqlParameter("@GiaiThich", (object?)q.GiaiThichDapAn ?? DBNull.Value),
                            new SqlParameter("@DoanVan", (object?)q.DoanVanDoc ?? DBNull.Value),
                            new SqlParameter("@MaTD", tdObj ?? DBNull.Value)
                        };
                        object? newChObj = SqlDataProvider.Instance.ExecuteScalar(insertCH, pCH);
                        if (newChObj != null && Convert.ToInt32(newChObj) > 0)
                        {
                            dbMaCauHoi = Convert.ToInt32(newChObj);
                            q.MaCauHoi = dbMaCauHoi;

                            foreach (var opt in q.DanhSachLuaChon)
                            {
                                string insertLC = @"
                                    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung)
                                    VALUES (@MaCH, @KyHieu, @NoiDung, @LaDung);
                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                                SqlParameter[] pLC = new[]
                                {
                                    new SqlParameter("@MaCH", dbMaCauHoi),
                                    new SqlParameter("@KyHieu", opt.KyHieuLuaChon),
                                    new SqlParameter("@NoiDung", opt.NoiDungLuaChon),
                                    new SqlParameter("@LaDung", opt.LaDapAnDung ? 1 : 0)
                                };
                                object? newLcObj = SqlDataProvider.Instance.ExecuteScalar(insertLC, pLC);
                                if (newLcObj != null && Convert.ToInt32(newLcObj) > 0)
                                {
                                    opt.MaLuaChon = Convert.ToInt32(newLcObj);
                                    opt.MaCauHoi = dbMaCauHoi;
                                }
                            }

                            string insertCHBT = @"
                                INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
                                VALUES (@MaDe, @MaCH, @ThuTu, 1.0);";
                            SqlParameter[] pCHBT = new[]
                            {
                                new SqlParameter("@MaDe", validMaDe),
                                new SqlParameter("@MaCH", dbMaCauHoi),
                                new SqlParameter("@ThuTu", thuTu++)
                            };
                            SqlDataProvider.Instance.ExecuteNonQuery(insertCHBT, pCHBT);
                        }
                    }
                    catch (Exception exCH)
                    {
                        System.Diagnostics.Debug.WriteLine($"[LuuCauHoiChuanVaoCSDLNeuChuaCo.Question] {exCH.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LuuCauHoiChuanVaoCSDLNeuChuaCo] {ex.Message}");
            }
        }

        public static List<CauHoiKhaoSatDTO> TaoBoCauHoiDeThiChuanTheoCapDo(string capDo)
        {
            var list = new List<CauHoiKhaoSatDTO>();
            string upper = capDo?.ToUpper() ?? "B1";

            switch (upper)
            {
                case "A1":
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 1001, ThuTuCauHoi = 1, KyNang = "NguPhap", MaTrinhDo = "A1",
                        NoiDungCauHoi = "They ______ from England. They are visiting Hanoi for summer vacation.",
                        GiaiThichDapAn = "Chủ ngữ 'They' số nhiều đi với động từ to be 'are'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 10011, KyHieuLuaChon = "A", NoiDungLuaChon = "is", LaDapAnDung = false },
                            new() { MaLuaChon = 10012, KyHieuLuaChon = "B", NoiDungLuaChon = "are", LaDapAnDung = true },
                            new() { MaLuaChon = 10013, KyHieuLuaChon = "C", NoiDungLuaChon = "am", LaDapAnDung = false },
                            new() { MaLuaChon = 10014, KyHieuLuaChon = "D", NoiDungLuaChon = "be", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 1002, ThuTuCauHoi = 2, KyNang = "TuVung", MaTrinhDo = "A1",
                        NoiDungCauHoi = "What time do you usually ______ breakfast with your family in the morning?",
                        GiaiThichDapAn = "Cụm từ cố định: 'have breakfast' (ăn sáng).",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 10021, KyHieuLuaChon = "A", NoiDungLuaChon = "have", LaDapAnDung = true },
                            new() { MaLuaChon = 10022, KyHieuLuaChon = "B", NoiDungLuaChon = "do", LaDapAnDung = false },
                            new() { MaLuaChon = 10023, KyHieuLuaChon = "C", NoiDungLuaChon = "make", LaDapAnDung = false },
                            new() { MaLuaChon = 10024, KyHieuLuaChon = "D", NoiDungLuaChon = "go", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 1003, ThuTuCauHoi = 3, KyNang = "NguPhap", MaTrinhDo = "A1",
                        NoiDungCauHoi = "There ______ four books on the study desk.",
                        GiaiThichDapAn = "Danh từ 'four books' là danh từ số nhiều nên dùng 'are'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 10031, KyHieuLuaChon = "A", NoiDungLuaChon = "is", LaDapAnDung = false },
                            new() { MaLuaChon = 10032, KyHieuLuaChon = "B", NoiDungLuaChon = "are", LaDapAnDung = true },
                            new() { MaLuaChon = 10033, KyHieuLuaChon = "C", NoiDungLuaChon = "has", LaDapAnDung = false },
                            new() { MaLuaChon = 10034, KyHieuLuaChon = "D", NoiDungLuaChon = "have", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 1004, ThuTuCauHoi = 4, KyNang = "TuVung", MaTrinhDo = "A1",
                        NoiDungCauHoi = "My sister works in a general hospital. She is a dedicated ______.",
                        GiaiThichDapAn = "'nurse' nghĩa là y tá, chăm sóc bệnh nhân trong bệnh viện.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 10041, KyHieuLuaChon = "A", NoiDungLuaChon = "nurse", LaDapAnDung = true },
                            new() { MaLuaChon = 10042, KyHieuLuaChon = "B", NoiDungLuaChon = "pilot", LaDapAnDung = false },
                            new() { MaLuaChon = 10043, KyHieuLuaChon = "C", NoiDungLuaChon = "driver", LaDapAnDung = false },
                            new() { MaLuaChon = 10044, KyHieuLuaChon = "D", NoiDungLuaChon = "chef", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 1005, ThuTuCauHoi = 5, KyNang = "NguPhap", MaTrinhDo = "A1",
                        NoiDungCauHoi = "I ______ like drinking cold water in winter.",
                        GiaiThichDapAn = "Chủ ngữ 'I' ở thì hiện tại đơn phủ định dùng trợ động từ 'don't'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 10051, KyHieuLuaChon = "A", NoiDungLuaChon = "doesn't", LaDapAnDung = false },
                            new() { MaLuaChon = 10052, KyHieuLuaChon = "B", NoiDungLuaChon = "don't", LaDapAnDung = true },
                            new() { MaLuaChon = 10053, KyHieuLuaChon = "C", NoiDungLuaChon = "not", LaDapAnDung = false },
                            new() { MaLuaChon = 10054, KyHieuLuaChon = "D", NoiDungLuaChon = "isn't", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 1006, ThuTuCauHoi = 6, KyNang = "TuVung", MaTrinhDo = "A1",
                        NoiDungCauHoi = "Please turn off your ______ before going to sleep.",
                        GiaiThichDapAn = "'lamp' hoặc 'phone', ở đây 'phone' là thiết bị cá nhân.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 10061, KyHieuLuaChon = "A", NoiDungLuaChon = "phone", LaDapAnDung = true },
                            new() { MaLuaChon = 10062, KyHieuLuaChon = "B", NoiDungLuaChon = "book", LaDapAnDung = false },
                            new() { MaLuaChon = 10063, KyHieuLuaChon = "C", NoiDungLuaChon = "door", LaDapAnDung = false },
                            new() { MaLuaChon = 10064, KyHieuLuaChon = "D", NoiDungLuaChon = "chair", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 1007, ThuTuCauHoi = 7, KyNang = "NguPhap", MaTrinhDo = "A1",
                        NoiDungCauHoi = "Where ______ you live right now?",
                        GiaiThichDapAn = "Câu hỏi thì hiện tại đơn với 'you' dùng trợ động từ 'do'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 10071, KyHieuLuaChon = "A", NoiDungLuaChon = "are", LaDapAnDung = false },
                            new() { MaLuaChon = 10072, KyHieuLuaChon = "B", NoiDungLuaChon = "do", LaDapAnDung = true },
                            new() { MaLuaChon = 10073, KyHieuLuaChon = "C", NoiDungLuaChon = "does", LaDapAnDung = false },
                            new() { MaLuaChon = 10074, KyHieuLuaChon = "D", NoiDungLuaChon = "is", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 1008, ThuTuCauHoi = 8, KyNang = "Doc", MaTrinhDo = "A1",
                        DoanVanDoc = "Hello, my name is David. I am 22 years old and I live in Da Nang city. Every weekend, I enjoy riding my bicycle along the seaside and drinking coconut water with my friends.",
                        NoiDungCauHoi = "What does David like to do on the weekends?",
                        GiaiThichDapAn = "Bài đọc nêu rõ: 'I enjoy riding my bicycle along the seaside'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 10081, KyHieuLuaChon = "A", NoiDungLuaChon = "Riding his bicycle along the seaside", LaDapAnDung = true },
                            new() { MaLuaChon = 10082, KyHieuLuaChon = "B", NoiDungLuaChon = "Cooking dinner for his family", LaDapAnDung = false },
                            new() { MaLuaChon = 10083, KyHieuLuaChon = "C", NoiDungLuaChon = "Staying in bed all day", LaDapAnDung = false },
                            new() { MaLuaChon = 10084, KyHieuLuaChon = "D", NoiDungLuaChon = "Driving a car to the mountains", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 1009, ThuTuCauHoi = 9, KyNang = "Doc", MaTrinhDo = "A1",
                        DoanVanDoc = "Hello, my name is David. I am 22 years old and I live in Da Nang city. Every weekend, I enjoy riding my bicycle along the seaside and drinking coconut water with my friends.",
                        NoiDungCauHoi = "Where does David live?",
                        GiaiThichDapAn = "Bài đọc: 'I live in Da Nang city'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 10091, KyHieuLuaChon = "A", NoiDungLuaChon = "Hanoi", LaDapAnDung = false },
                            new() { MaLuaChon = 10092, KyHieuLuaChon = "B", NoiDungLuaChon = "Da Nang city", LaDapAnDung = true },
                            new() { MaLuaChon = 10093, KyHieuLuaChon = "C", NoiDungLuaChon = "Ho Chi Minh City", LaDapAnDung = false },
                            new() { MaLuaChon = 10094, KyHieuLuaChon = "D", NoiDungLuaChon = "London", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 1010, ThuTuCauHoi = 10, KyNang = "TuVung", MaTrinhDo = "A1",
                        NoiDungCauHoi = "Today is Monday, so tomorrow will be ______.",
                        GiaiThichDapAn = "Thứ hai kế tiếp là thứ ba (Tuesday).",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 10101, KyHieuLuaChon = "A", NoiDungLuaChon = "Tuesday", LaDapAnDung = true },
                            new() { MaLuaChon = 10102, KyHieuLuaChon = "B", NoiDungLuaChon = "Wednesday", LaDapAnDung = false },
                            new() { MaLuaChon = 10103, KyHieuLuaChon = "C", NoiDungLuaChon = "Sunday", LaDapAnDung = false },
                            new() { MaLuaChon = 10104, KyHieuLuaChon = "D", NoiDungLuaChon = "Friday", LaDapAnDung = false }
                        }
                    });
                    break;

                case "A2":
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 2001, ThuTuCauHoi = 1, KyNang = "NguPhap", MaTrinhDo = "A2",
                        NoiDungCauHoi = "Yesterday, she ______ to the supermarket to buy fresh vegetables.",
                        GiaiThichDapAn = "'Yesterday' là dấu hiệu thì quá khứ đơn -> 'went'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 20011, KyHieuLuaChon = "A", NoiDungLuaChon = "go", LaDapAnDung = false },
                            new() { MaLuaChon = 20012, KyHieuLuaChon = "B", NoiDungLuaChon = "goes", LaDapAnDung = false },
                            new() { MaLuaChon = 20013, KyHieuLuaChon = "C", NoiDungLuaChon = "went", LaDapAnDung = true },
                            new() { MaLuaChon = 20014, KyHieuLuaChon = "D", NoiDungLuaChon = "gone", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 2002, ThuTuCauHoi = 2, KyNang = "TuVung", MaTrinhDo = "A2",
                        NoiDungCauHoi = "The doctor advised him to take this ______ twice a day after meals.",
                        GiaiThichDapAn = "'medicine' nghĩa là thuốc uống chữa bệnh.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 20021, KyHieuLuaChon = "A", NoiDungLuaChon = "medicine", LaDapAnDung = true },
                            new() { MaLuaChon = 20022, KyHieuLuaChon = "B", NoiDungLuaChon = "recipe", LaDapAnDung = false },
                            new() { MaLuaChon = 20023, KyHieuLuaChon = "C", NoiDungLuaChon = "ingredient", LaDapAnDung = false },
                            new() { MaLuaChon = 20024, KyHieuLuaChon = "D", NoiDungLuaChon = "equipment", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 2003, ThuTuCauHoi = 3, KyNang = "NguPhap", MaTrinhDo = "A2",
                        NoiDungCauHoi = "This smartphone is ______ than the one I bought last year.",
                        GiaiThichDapAn = "Tính từ ngắn 'cheap' so sánh hơn là 'cheaper'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 20031, KyHieuLuaChon = "A", NoiDungLuaChon = "more cheap", LaDapAnDung = false },
                            new() { MaLuaChon = 20032, KyHieuLuaChon = "B", NoiDungLuaChon = "cheaper", LaDapAnDung = true },
                            new() { MaLuaChon = 20033, KyHieuLuaChon = "C", NoiDungLuaChon = "cheapest", LaDapAnDung = false },
                            new() { MaLuaChon = 20034, KyHieuLuaChon = "D", NoiDungLuaChon = "as cheap", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 2004, ThuTuCauHoi = 4, KyNang = "TuVung", MaTrinhDo = "A2",
                        NoiDungCauHoi = "Don't forget to turn off the lights before you ______ the classroom.",
                        GiaiThichDapAn = "'leave' nghĩa là rời khỏi phòng.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 20041, KyHieuLuaChon = "A", NoiDungLuaChon = "arrive", LaDapAnDung = false },
                            new() { MaLuaChon = 20042, KyHieuLuaChon = "B", NoiDungLuaChon = "leave", LaDapAnDung = true },
                            new() { MaLuaChon = 20043, KyHieuLuaChon = "C", NoiDungLuaChon = "stay", LaDapAnDung = false },
                            new() { MaLuaChon = 20044, KyHieuLuaChon = "D", NoiDungLuaChon = "wait", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 2005, ThuTuCauHoi = 5, KyNang = "NguPhap", MaTrinhDo = "A2",
                        NoiDungCauHoi = "While I was studying in my room, my brother ______ computer games.",
                        GiaiThichDapAn = "Hành động song song trong quá khứ với 'While' dùng thì quá khứ tiếp diễn -> 'was playing'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 20051, KyHieuLuaChon = "A", NoiDungLuaChon = "played", LaDapAnDung = false },
                            new() { MaLuaChon = 20052, KyHieuLuaChon = "B", NoiDungLuaChon = "was playing", LaDapAnDung = true },
                            new() { MaLuaChon = 20053, KyHieuLuaChon = "C", NoiDungLuaChon = "is playing", LaDapAnDung = false },
                            new() { MaLuaChon = 20054, KyHieuLuaChon = "D", NoiDungLuaChon = "plays", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 2006, ThuTuCauHoi = 6, KyNang = "TuVung", MaTrinhDo = "A2",
                        NoiDungCauHoi = "I need to go to the post office to send this ______ to my friend in Australia.",
                        GiaiThichDapAn = "'parcel' nghĩa là bưu kiện / kiện hàng gửi bưu điện.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 20061, KyHieuLuaChon = "A", NoiDungLuaChon = "parcel", LaDapAnDung = true },
                            new() { MaLuaChon = 20062, KyHieuLuaChon = "B", NoiDungLuaChon = "envelope", LaDapAnDung = false },
                            new() { MaLuaChon = 20063, KyHieuLuaChon = "C", NoiDungLuaChon = "ticket", LaDapAnDung = false },
                            new() { MaLuaChon = 20064, KyHieuLuaChon = "D", NoiDungLuaChon = "stamp", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 2007, ThuTuCauHoi = 7, KyNang = "NguPhap", MaTrinhDo = "A2",
                        NoiDungCauHoi = "She has lived in this modern apartment ______ three years.",
                        GiaiThichDapAn = "Khoảng thời gian 'three years' đi với giới từ 'for'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 20071, KyHieuLuaChon = "A", NoiDungLuaChon = "since", LaDapAnDung = false },
                            new() { MaLuaChon = 20072, KyHieuLuaChon = "B", NoiDungLuaChon = "for", LaDapAnDung = true },
                            new() { MaLuaChon = 20073, KyHieuLuaChon = "C", NoiDungLuaChon = "during", LaDapAnDung = false },
                            new() { MaLuaChon = 20074, KyHieuLuaChon = "D", NoiDungLuaChon = "in", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 2008, ThuTuCauHoi = 8, KyNang = "Doc", MaTrinhDo = "A2",
                        DoanVanDoc = "Dear staff, please note that the company cafeteria will be closed for renovation from August 1st to August 7th. During this period, free snacks and bottled water will be available in the 3rd floor lounge.",
                        NoiDungCauHoi = "Where can staff get free snacks during the renovation?",
                        GiaiThichDapAn = "Đoạn văn ghi rõ: 'available in the 3rd floor lounge'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 20081, KyHieuLuaChon = "A", NoiDungLuaChon = "In the 3rd floor lounge", LaDapAnDung = true },
                            new() { MaLuaChon = 20082, KyHieuLuaChon = "B", NoiDungLuaChon = "In the basement parking", LaDapAnDung = false },
                            new() { MaLuaChon = 20083, KyHieuLuaChon = "C", NoiDungLuaChon = "At the main entrance", LaDapAnDung = false },
                            new() { MaLuaChon = 20084, KyHieuLuaChon = "D", NoiDungLuaChon = "In the cafeteria kitchen", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 2009, ThuTuCauHoi = 9, KyNang = "Doc", MaTrinhDo = "A2",
                        DoanVanDoc = "Dear staff, please note that the company cafeteria will be closed for renovation from August 1st to August 7th. During this period, free snacks and bottled water will be available in the 3rd floor lounge.",
                        NoiDungCauHoi = "How long will the cafeteria be closed?",
                        GiaiThichDapAn = "Từ ngày 1/8 đến 7/8 là 7 ngày (1 tuần).",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 20091, KyHieuLuaChon = "A", NoiDungLuaChon = "One week (7 days)", LaDapAnDung = true },
                            new() { MaLuaChon = 20092, KyHieuLuaChon = "B", NoiDungLuaChon = "One month", LaDapAnDung = false },
                            new() { MaLuaChon = 20093, KyHieuLuaChon = "C", NoiDungLuaChon = "Two days", LaDapAnDung = false },
                            new() { MaLuaChon = 20094, KyHieuLuaChon = "D", NoiDungLuaChon = "Three weeks", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 2010, ThuTuCauHoi = 10, KyNang = "TuVung", MaTrinhDo = "A2",
                        NoiDungCauHoi = "If it rains tomorrow, we will have to ______ the outdoor football match.",
                        GiaiThichDapAn = "'cancel' nghĩa là hủy bỏ trận đấu.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 20101, KyHieuLuaChon = "A", NoiDungLuaChon = "cancel", LaDapAnDung = true },
                            new() { MaLuaChon = 20102, KyHieuLuaChon = "B", NoiDungLuaChon = "start", LaDapAnDung = false },
                            new() { MaLuaChon = 20103, KyHieuLuaChon = "C", NoiDungLuaChon = "continue", LaDapAnDung = false },
                            new() { MaLuaChon = 20104, KyHieuLuaChon = "D", NoiDungLuaChon = "enjoy", LaDapAnDung = false }
                        }
                    });
                    break;

                case "C1":
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 5001, ThuTuCauHoi = 1, KyNang = "NguPhap", MaTrinhDo = "C1",
                        NoiDungCauHoi = "Not until the final research findings were published ______ to understand the true impact of the phenomenon.",
                        GiaiThichDapAn = "Cấu trúc đảo ngữ: 'Not until + clause + did + S + V-inf'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 50011, KyHieuLuaChon = "A", NoiDungLuaChon = "did scientists begin", LaDapAnDung = true },
                            new() { MaLuaChon = 50012, KyHieuLuaChon = "B", NoiDungLuaChon = "scientists began", LaDapAnDung = false },
                            new() { MaLuaChon = 50013, KyHieuLuaChon = "C", NoiDungLuaChon = "had scientists begun", LaDapAnDung = false },
                            new() { MaLuaChon = 50014, KyHieuLuaChon = "D", NoiDungLuaChon = "scientists had begun", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 5002, ThuTuCauHoi = 2, KyNang = "TuVung", MaTrinhDo = "C1",
                        NoiDungCauHoi = "His persistent efforts in advocating for sustainable green policies ultimately bore ______.",
                        GiaiThichDapAn = "Thành ngữ học thuật: 'bear fruit' (gặt hái thành quả tốt đẹp).",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 50021, KyHieuLuaChon = "A", NoiDungLuaChon = "fruit", LaDapAnDung = true },
                            new() { MaLuaChon = 50022, KyHieuLuaChon = "B", NoiDungLuaChon = "seed", LaDapAnDung = false },
                            new() { MaLuaChon = 50023, KyHieuLuaChon = "C", NoiDungLuaChon = "tree", LaDapAnDung = false },
                            new() { MaLuaChon = 50024, KyHieuLuaChon = "D", NoiDungLuaChon = "harvest", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 5003, ThuTuCauHoi = 3, KyNang = "TuVung", MaTrinhDo = "C1",
                        NoiDungCauHoi = "The unprecedented economic sanctions had an ______ effect on international commerce.",
                        GiaiThichDapAn = "'adverse effect' (tác động tiêu cực/bất lợi).",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 50031, KyHieuLuaChon = "A", NoiDungLuaChon = "adverse", LaDapAnDung = true },
                            new() { MaLuaChon = 50032, KyHieuLuaChon = "B", NoiDungLuaChon = "averse", LaDapAnDung = false },
                            new() { MaLuaChon = 50033, KyHieuLuaChon = "C", NoiDungLuaChon = "reverse", LaDapAnDung = false },
                            new() { MaLuaChon = 50034, KyHieuLuaChon = "D", NoiDungLuaChon = "inverse", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 5004, ThuTuCauHoi = 4, KyNang = "NguPhap", MaTrinhDo = "C1",
                        NoiDungCauHoi = "Were the executive board ______ the proposal, the merger would proceed ahead of schedule.",
                        GiaiThichDapAn = "Đảo ngữ câu điều kiện loại 2: 'Were + S + to-V'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 50041, KyHieuLuaChon = "A", NoiDungLuaChon = "to approve", LaDapAnDung = true },
                            new() { MaLuaChon = 50042, KyHieuLuaChon = "B", NoiDungLuaChon = "approved", LaDapAnDung = false },
                            new() { MaLuaChon = 50043, KyHieuLuaChon = "C", NoiDungLuaChon = "approving", LaDapAnDung = false },
                            new() { MaLuaChon = 50044, KyHieuLuaChon = "D", NoiDungLuaChon = "approves", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 5005, ThuTuCauHoi = 5, KyNang = "TuVung", MaTrinhDo = "C1",
                        NoiDungCauHoi = "The technological breakthrough is ______ with innovative leadership in AI development.",
                        GiaiThichDapAn = "Cụm 'synonymous with' nghĩa là đồng nghĩa với / gắn liền với.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 50051, KyHieuLuaChon = "A", NoiDungLuaChon = "synonymous", LaDapAnDung = true },
                            new() { MaLuaChon = 50052, KyHieuLuaChon = "B", NoiDungLuaChon = "anonymous", LaDapAnDung = false },
                            new() { MaLuaChon = 50053, KyHieuLuaChon = "C", NoiDungLuaChon = "harmonious", LaDapAnDung = false },
                            new() { MaLuaChon = 50054, KyHieuLuaChon = "D", NoiDungLuaChon = "spontaneous", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 5006, ThuTuCauHoi = 6, KyNang = "NguPhap", MaTrinhDo = "C1",
                        NoiDungCauHoi = "Seldom ______ such profound consensus among prominent climatologists.",
                        GiaiThichDapAn = "Đảo ngữ với phó từ mang nghĩa phủ định 'Seldom has there been'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 50061, KyHieuLuaChon = "A", NoiDungLuaChon = "there has been", LaDapAnDung = false },
                            new() { MaLuaChon = 50062, KyHieuLuaChon = "B", NoiDungLuaChon = "has there been", LaDapAnDung = true },
                            new() { MaLuaChon = 50063, KyHieuLuaChon = "C", NoiDungLuaChon = "was there being", LaDapAnDung = false },
                            new() { MaLuaChon = 50064, KyHieuLuaChon = "D", NoiDungLuaChon = "is there been", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 5007, ThuTuCauHoi = 7, KyNang = "TuVung", MaTrinhDo = "C1",
                        NoiDungCauHoi = "The government aims to ______ the administrative bottleneck hindering foreign investments.",
                        GiaiThichDapAn = "'alleviate' nghĩa là xoa dịu, tháo gỡ điểm nghẽn khó khăn.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 50071, KyHieuLuaChon = "A", NoiDungLuaChon = "alleviate", LaDapAnDung = true },
                            new() { MaLuaChon = 50072, KyHieuLuaChon = "B", NoiDungLuaChon = "aggravate", LaDapAnDung = false },
                            new() { MaLuaChon = 50073, KyHieuLuaChon = "C", NoiDungLuaChon = "allocate", LaDapAnDung = false },
                            new() { MaLuaChon = 50074, KyHieuLuaChon = "D", NoiDungLuaChon = "alienate", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 5008, ThuTuCauHoi = 8, KyNang = "Doc", MaTrinhDo = "C1",
                        DoanVanDoc = "Cognitive linguistics posits that human language structures are intrinsically reflective of embodied experiential knowledge. Rather than treating syntax as an autonomous algebraic module, experiential semantics demonstrates that abstract conceptualization relies fundamentally upon metaphorical mapping and perceptual scaffolding from physical environments.",
                        NoiDungCauHoi = "What is the central premise of cognitive linguistics according to the passage?",
                        GiaiThichDapAn = "Đoạn văn nêu rõ: Ngôn ngữ phản ánh kiến thức thể nghiệm hiện sinh (embodied experiential knowledge).",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 50081, KyHieuLuaChon = "A", NoiDungLuaChon = "Language structures reflect embodied experiential knowledge.", LaDapAnDung = true },
                            new() { MaLuaChon = 50082, KyHieuLuaChon = "B", NoiDungLuaChon = "Syntax is entirely independent of human perception.", LaDapAnDung = false },
                            new() { MaLuaChon = 50083, KyHieuLuaChon = "C", NoiDungLuaChon = "Abstract thinking does not rely on metaphors.", LaDapAnDung = false },
                            new() { MaLuaChon = 50084, KyHieuLuaChon = "D", NoiDungLuaChon = "Algebraic formulas can replace natural languages completely.", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 5009, ThuTuCauHoi = 9, KyNang = "Doc", MaTrinhDo = "C1",
                        DoanVanDoc = "Cognitive linguistics posits that human language structures are intrinsically reflective of embodied experiential knowledge. Rather than treating syntax as an autonomous algebraic module, experiential semantics demonstrates that abstract conceptualization relies fundamentally upon metaphorical mapping and perceptual scaffolding from physical environments.",
                        NoiDungCauHoi = "How does experiential semantics view abstract conceptualization?",
                        GiaiThichDapAn = "Dựa trên câu cuối: 'relies fundamentally upon metaphorical mapping and perceptual scaffolding'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 50091, KyHieuLuaChon = "A", NoiDungLuaChon = "It relies on metaphorical mapping from physical environments.", LaDapAnDung = true },
                            new() { MaLuaChon = 50092, KyHieuLuaChon = "B", NoiDungLuaChon = "It operates as an isolated computational algorithm.", LaDapAnDung = false },
                            new() { MaLuaChon = 50093, KyHieuLuaChon = "C", NoiDungLuaChon = "It rejects any sensory input from the real world.", LaDapAnDung = false },
                            new() { MaLuaChon = 50094, KyHieuLuaChon = "D", NoiDungLuaChon = "It is genetically determined without environmental interaction.", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 5010, ThuTuCauHoi = 10, KyNang = "NguPhap", MaTrinhDo = "C1",
                        NoiDungCauHoi = "Hardly had the controversial resolution been ratified ______ public protests erupted nationwide.",
                        GiaiThichDapAn = "Cấu trúc 'Hardly had + S + P2 ... when + clause'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 50101, KyHieuLuaChon = "A", NoiDungLuaChon = "when", LaDapAnDung = true },
                            new() { MaLuaChon = 50102, KyHieuLuaChon = "B", NoiDungLuaChon = "than", LaDapAnDung = false },
                            new() { MaLuaChon = 50103, KyHieuLuaChon = "C", NoiDungLuaChon = "then", LaDapAnDung = false },
                            new() { MaLuaChon = 50104, KyHieuLuaChon = "D", NoiDungLuaChon = "as", LaDapAnDung = false }
                        }
                    });
                    break;

                case "B2":
                    // Sử dụng 10 câu B2 chuẩn (kết hợp các câu ngữ pháp B2 và bài đọc về AI)
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 4001, ThuTuCauHoi = 1, KyNang = "NguPhap", MaTrinhDo = "B2",
                        NoiDungCauHoi = "If I ______ more time yesterday, I would have traveled around the ancient city with you.",
                        GiaiThichDapAn = "Điều kiện loại 3: If + S + had + P2 -> 'had had'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 40011, KyHieuLuaChon = "A", NoiDungLuaChon = "have", LaDapAnDung = false },
                            new() { MaLuaChon = 40012, KyHieuLuaChon = "B", NoiDungLuaChon = "had had", LaDapAnDung = true },
                            new() { MaLuaChon = 40013, KyHieuLuaChon = "C", NoiDungLuaChon = "had", LaDapAnDung = false },
                            new() { MaLuaChon = 40014, KyHieuLuaChon = "D", NoiDungLuaChon = "would have", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 4002, ThuTuCauHoi = 2, KyNang = "TuVung", MaTrinhDo = "B2",
                        NoiDungCauHoi = "The university decided to ______ innovative teaching methods to stimulate students' critical thinking.",
                        GiaiThichDapAn = "'adopt' nghĩa là áp dụng phương pháp mới.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 40021, KyHieuLuaChon = "A", NoiDungLuaChon = "adopt", LaDapAnDung = true },
                            new() { MaLuaChon = 40022, KyHieuLuaChon = "B", NoiDungLuaChon = "adapt", LaDapAnDung = false },
                            new() { MaLuaChon = 40023, KyHieuLuaChon = "C", NoiDungLuaChon = "adept", LaDapAnDung = false },
                            new() { MaLuaChon = 40024, KyHieuLuaChon = "D", NoiDungLuaChon = "admit", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 4003, ThuTuCauHoi = 3, KyNang = "NguPhap", MaTrinhDo = "B2",
                        NoiDungCauHoi = "Hardly ______ home when the torrential rain began to pour.",
                        GiaiThichDapAn = "Cấu trúc đảo ngữ: 'Hardly had + S + P2 + when'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 40031, KyHieuLuaChon = "A", NoiDungLuaChon = "she arrived", LaDapAnDung = false },
                            new() { MaLuaChon = 40032, KyHieuLuaChon = "B", NoiDungLuaChon = "had she arrived", LaDapAnDung = true },
                            new() { MaLuaChon = 40033, KyHieuLuaChon = "C", NoiDungLuaChon = "she had arrived", LaDapAnDung = false },
                            new() { MaLuaChon = 40034, KyHieuLuaChon = "D", NoiDungLuaChon = "did she arrive", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 4004, ThuTuCauHoi = 4, KyNang = "TuVung", MaTrinhDo = "B2",
                        NoiDungCauHoi = "The sudden ______ in consumer demand led to widespread shortages across major markets.",
                        GiaiThichDapAn = "'surge' nghĩa là sự tăng đột biến về nhu cầu.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 40041, KyHieuLuaChon = "A", NoiDungLuaChon = "surge", LaDapAnDung = true },
                            new() { MaLuaChon = 40042, KyHieuLuaChon = "B", NoiDungLuaChon = "drought", LaDapAnDung = false },
                            new() { MaLuaChon = 40043, KyHieuLuaChon = "C", NoiDungLuaChon = "decline", LaDapAnDung = false },
                            new() { MaLuaChon = 40044, KyHieuLuaChon = "D", NoiDungLuaChon = "tide", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 4005, ThuTuCauHoi = 5, KyNang = "NguPhap", MaTrinhDo = "B2",
                        NoiDungCauHoi = "It is essential that every candidate ______ present in the examination hall before 8:00 AM.",
                        GiaiThichDapAn = "Thể giả định: 'essential that + S + (should) be'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 40051, KyHieuLuaChon = "A", NoiDungLuaChon = "is", LaDapAnDung = false },
                            new() { MaLuaChon = 40052, KyHieuLuaChon = "B", NoiDungLuaChon = "be", LaDapAnDung = true },
                            new() { MaLuaChon = 40053, KyHieuLuaChon = "C", NoiDungLuaChon = "was", LaDapAnDung = false },
                            new() { MaLuaChon = 40054, KyHieuLuaChon = "D", NoiDungLuaChon = "being", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 4006, ThuTuCauHoi = 6, KyNang = "TuVung", MaTrinhDo = "B2",
                        NoiDungCauHoi = "The company implemented cutting-edge strategies to gain a competitive ______ in the marketplace.",
                        GiaiThichDapAn = "'competitive edge' nghĩa là lợi thế cạnh tranh vượt trội.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 40061, KyHieuLuaChon = "A", NoiDungLuaChon = "edge", LaDapAnDung = true },
                            new() { MaLuaChon = 40062, KyHieuLuaChon = "B", NoiDungLuaChon = "side", LaDapAnDung = false },
                            new() { MaLuaChon = 40063, KyHieuLuaChon = "C", NoiDungLuaChon = "border", LaDapAnDung = false },
                            new() { MaLuaChon = 40064, KyHieuLuaChon = "D", NoiDungLuaChon = "boundary", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 4007, ThuTuCauHoi = 7, KyNang = "NguPhap", MaTrinhDo = "B2",
                        NoiDungCauHoi = "Had you notified us earlier, we ______ alternate travel arrangements for the conference.",
                        GiaiThichDapAn = "Đảo ngữ điều kiện loại 3: 'Had you + P2, S + would have + P2'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 40071, KyHieuLuaChon = "A", NoiDungLuaChon = "could have made", LaDapAnDung = true },
                            new() { MaLuaChon = 40072, KyHieuLuaChon = "B", NoiDungLuaChon = "made", LaDapAnDung = false },
                            new() { MaLuaChon = 40073, KyHieuLuaChon = "C", NoiDungLuaChon = "can make", LaDapAnDung = false },
                            new() { MaLuaChon = 40074, KyHieuLuaChon = "D", NoiDungLuaChon = "will have made", LaDapAnDung = false }
                        }
                    });

                    string passageB2 = "Artificial Intelligence (AI) is rapidly transforming modern language education by providing personalized pathways tailored to each student's pace. Unlike traditional one-size-fits-all classrooms, adaptive AI models continuously analyze user responses, identify lexical and grammatical bottlenecks, and immediately adjust exercise difficulty. Furthermore, generative AI conversational agents simulate immersive dialogue environments, allowing learners to build speaking and writing confidence without fear of judgment. However, educational experts emphasize that AI should complement rather than fully replace human instructors, who remain essential for fostering critical thinking and intercultural empathy.";

                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 4008, ThuTuCauHoi = 8, KyNang = "Doc", MaTrinhDo = "B2",
                        DoanVanDoc = passageB2,
                        NoiDungCauHoi = "What is the primary advantage of AI in language learning discussed in the passage?",
                        GiaiThichDapAn = "Đoạn văn nêu rõ AI cung cấp lộ trình cá nhân hóa thích ứng theo tốc độ học viên.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 40081, KyHieuLuaChon = "A", NoiDungLuaChon = "It guarantees native fluency within two weeks.", LaDapAnDung = false },
                            new() { MaLuaChon = 40082, KyHieuLuaChon = "B", NoiDungLuaChon = "It provides personalized pathways tailored to each student's pace.", LaDapAnDung = true },
                            new() { MaLuaChon = 40083, KyHieuLuaChon = "C", NoiDungLuaChon = "It makes human instructors completely obsolete.", LaDapAnDung = false },
                            new() { MaLuaChon = 40084, KyHieuLuaChon = "D", NoiDungLuaChon = "It standardizes all learning into a rigid format.", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 4009, ThuTuCauHoi = 9, KyNang = "Doc", MaTrinhDo = "B2",
                        DoanVanDoc = passageB2,
                        NoiDungCauHoi = "How do generative AI conversational agents assist language learners?",
                        GiaiThichDapAn = "Đoạn văn ghi: 'By simulating immersive environments to practice without fear of judgment'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 40091, KyHieuLuaChon = "A", NoiDungLuaChon = "By eliminating the requirement to learn grammar.", LaDapAnDung = false },
                            new() { MaLuaChon = 40092, KyHieuLuaChon = "B", NoiDungLuaChon = "By simulating immersive dialogue environments to practice without judgment.", LaDapAnDung = true },
                            new() { MaLuaChon = 40093, KyHieuLuaChon = "C", NoiDungLuaChon = "By awarding real cash bonuses for high scores.", LaDapAnDung = false },
                            new() { MaLuaChon = 40094, KyHieuLuaChon = "D", NoiDungLuaChon = "By grading university entrance examinations.", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 4010, ThuTuCauHoi = 10, KyNang = "Doc", MaTrinhDo = "B2",
                        DoanVanDoc = passageB2,
                        NoiDungCauHoi = "According to educational experts, what role should human teachers play in AI education?",
                        GiaiThichDapAn = "Các chuyên gia khẳng định giáo viên vẫn cần thiết để nuôi dưỡng tư duy phản biện và sự thấu cảm.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 40101, KyHieuLuaChon = "A", NoiDungLuaChon = "They remain vital for nurturing critical thinking and empathy.", LaDapAnDung = true },
                            new() { MaLuaChon = 40102, KyHieuLuaChon = "B", NoiDungLuaChon = "They should focus exclusively on computer hardware.", LaDapAnDung = false },
                            new() { MaLuaChon = 40103, KyHieuLuaChon = "C", NoiDungLuaChon = "They should stop teaching vocabulary.", LaDapAnDung = false },
                            new() { MaLuaChon = 40104, KyHieuLuaChon = "D", NoiDungLuaChon = "They should be replaced by algorithms immediately.", LaDapAnDung = false }
                        }
                    });
                    break;

                case "B1":
                default:
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 3001, ThuTuCauHoi = 1, KyNang = "NguPhap", MaTrinhDo = "B1",
                        NoiDungCauHoi = "Mr. David ______ in this international academy since 2018.",
                        GiaiThichDapAn = "'since 2018' chỉ hành động bắt đầu từ quá khứ và còn tiếp diễn -> 'has worked'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 30011, KyHieuLuaChon = "A", NoiDungLuaChon = "worked", LaDapAnDung = false },
                            new() { MaLuaChon = 30012, KyHieuLuaChon = "B", NoiDungLuaChon = "has worked", LaDapAnDung = true },
                            new() { MaLuaChon = 30013, KyHieuLuaChon = "C", NoiDungLuaChon = "is working", LaDapAnDung = false },
                            new() { MaLuaChon = 30014, KyHieuLuaChon = "D", NoiDungLuaChon = "works", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 3002, ThuTuCauHoi = 2, KyNang = "TuVung", MaTrinhDo = "B1",
                        NoiDungCauHoi = "All candidates must submit their application before the strict ______ on Friday.",
                        GiaiThichDapAn = "'deadline' nghĩa là hạn chót nộp hồ sơ.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 30021, KyHieuLuaChon = "A", NoiDungLuaChon = "deadline", LaDapAnDung = true },
                            new() { MaLuaChon = 30022, KyHieuLuaChon = "B", NoiDungLuaChon = "destination", LaDapAnDung = false },
                            new() { MaLuaChon = 30023, KyHieuLuaChon = "C", NoiDungLuaChon = "dimension", LaDapAnDung = false },
                            new() { MaLuaChon = 30024, KyHieuLuaChon = "D", NoiDungLuaChon = "description", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 3003, ThuTuCauHoi = 3, KyNang = "NguPhap", MaTrinhDo = "B1",
                        NoiDungCauHoi = "She has always been capable ______ solving complicated mathematical equations.",
                        GiaiThichDapAn = "Cấu trúc: 'capable of + V-ing' (có khả năng làm gì).",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 30031, KyHieuLuaChon = "A", NoiDungLuaChon = "with", LaDapAnDung = false },
                            new() { MaLuaChon = 30032, KyHieuLuaChon = "B", NoiDungLuaChon = "in", LaDapAnDung = false },
                            new() { MaLuaChon = 30033, KyHieuLuaChon = "C", NoiDungLuaChon = "of", LaDapAnDung = true },
                            new() { MaLuaChon = 30034, KyHieuLuaChon = "D", NoiDungLuaChon = "for", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 3004, ThuTuCauHoi = 4, KyNang = "NguPhap", MaTrinhDo = "B1",
                        NoiDungCauHoi = "The scientist ______ discoveries revolutionized modern medicine was awarded the Nobel Prize.",
                        GiaiThichDapAn = "Đại từ quan hệ chỉ sở hữu: 'whose discoveries'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 30041, KyHieuLuaChon = "A", NoiDungLuaChon = "who", LaDapAnDung = false },
                            new() { MaLuaChon = 30042, KyHieuLuaChon = "B", NoiDungLuaChon = "whose", LaDapAnDung = true },
                            new() { MaLuaChon = 30043, KyHieuLuaChon = "C", NoiDungLuaChon = "whom", LaDapAnDung = false },
                            new() { MaLuaChon = 30044, KyHieuLuaChon = "D", NoiDungLuaChon = "which", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 3005, ThuTuCauHoi = 5, KyNang = "TuVung", MaTrinhDo = "B1",
                        NoiDungCauHoi = "Online learning allows students to study at their own ______ without unnecessary stress.",
                        GiaiThichDapAn = "Cụm 'at their own pace' (theo tiến độ riêng của mình).",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 30051, KyHieuLuaChon = "A", NoiDungLuaChon = "pace", LaDapAnDung = true },
                            new() { MaLuaChon = 30052, KyHieuLuaChon = "B", NoiDungLuaChon = "step", LaDapAnDung = false },
                            new() { MaLuaChon = 30053, KyHieuLuaChon = "C", NoiDungLuaChon = "rate", LaDapAnDung = false },
                            new() { MaLuaChon = 30054, KyHieuLuaChon = "D", NoiDungLuaChon = "foot", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 3006, ThuTuCauHoi = 6, KyNang = "NguPhap", MaTrinhDo = "B1",
                        NoiDungCauHoi = "The new bridge ______ by international engineers since last October.",
                        GiaiThichDapAn = "Bị động thì hiện tại hoàn thành tiếp diễn: 'has been being built' hoặc hiện tại hoàn thành: 'has been built'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 30061, KyHieuLuaChon = "A", NoiDungLuaChon = "has been built", LaDapAnDung = true },
                            new() { MaLuaChon = 30062, KyHieuLuaChon = "B", NoiDungLuaChon = "was building", LaDapAnDung = false },
                            new() { MaLuaChon = 30063, KyHieuLuaChon = "C", NoiDungLuaChon = "is building", LaDapAnDung = false },
                            new() { MaLuaChon = 30064, KyHieuLuaChon = "D", NoiDungLuaChon = "builds", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 3007, ThuTuCauHoi = 7, KyNang = "TuVung", MaTrinhDo = "B1",
                        NoiDungCauHoi = "We look forward to ______ from your company regarding the proposal.",
                        GiaiThichDapAn = "Cấu trúc: 'look forward to + V-ing'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 30071, KyHieuLuaChon = "A", NoiDungLuaChon = "hearing", LaDapAnDung = true },
                            new() { MaLuaChon = 30072, KyHieuLuaChon = "B", NoiDungLuaChon = "hear", LaDapAnDung = false },
                            new() { MaLuaChon = 30073, KyHieuLuaChon = "C", NoiDungLuaChon = "heard", LaDapAnDung = false },
                            new() { MaLuaChon = 30074, KyHieuLuaChon = "D", NoiDungLuaChon = "to hear", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 3008, ThuTuCauHoi = 8, KyNang = "Doc", MaTrinhDo = "B1",
                        DoanVanDoc = "Apex Corp is pleased to announce our annual tech conference scheduled for November 15th at Grand Hotel. The conference will focus on digital transformation, cybersecurity, and workplace productivity. Early-bird registration offers a 30% discount until the end of this month.",
                        NoiDungCauHoi = "What is the primary topic of the upcoming Apex Corp conference?",
                        GiaiThichDapAn = "Bài đọc: 'focus on digital transformation, cybersecurity, and workplace productivity'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 30081, KyHieuLuaChon = "A", NoiDungLuaChon = "Digital transformation and cybersecurity", LaDapAnDung = true },
                            new() { MaLuaChon = 30082, KyHieuLuaChon = "B", NoiDungLuaChon = "Real estate investment and architecture", LaDapAnDung = false },
                            new() { MaLuaChon = 30083, KyHieuLuaChon = "C", NoiDungLuaChon = "Culinary arts and food safety", LaDapAnDung = false },
                            new() { MaLuaChon = 30084, KyHieuLuaChon = "D", NoiDungLuaChon = "Tourism and hotel management", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 3009, ThuTuCauHoi = 9, KyNang = "Doc", MaTrinhDo = "B1",
                        DoanVanDoc = "Apex Corp is pleased to announce our annual tech conference scheduled for November 15th at Grand Hotel. The conference will focus on digital transformation, cybersecurity, and workplace productivity. Early-bird registration offers a 30% discount until the end of this month.",
                        NoiDungCauHoi = "What benefit is offered for early-bird registration?",
                        GiaiThichDapAn = "Bài đọc nêu rõ: 'offers a 30% discount until the end of this month'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 30091, KyHieuLuaChon = "A", NoiDungLuaChon = "A 30% discount on registration fee", LaDapAnDung = true },
                            new() { MaLuaChon = 30092, KyHieuLuaChon = "B", NoiDungLuaChon = "Free round-trip flight tickets", LaDapAnDung = false },
                            new() { MaLuaChon = 30093, KyHieuLuaChon = "C", NoiDungLuaChon = "Complimentary hotel stay for one week", LaDapAnDung = false },
                            new() { MaLuaChon = 30094, KyHieuLuaChon = "D", NoiDungLuaChon = "A free tablet computer", LaDapAnDung = false }
                        }
                    });
                    list.Add(new CauHoiKhaoSatDTO
                    {
                        MaCauHoi = 3010, ThuTuCauHoi = 10, KyNang = "NguPhap", MaTrinhDo = "B1",
                        NoiDungCauHoi = "If she ______ harder, she will definitely pass the graduation examination.",
                        GiaiThichDapAn = "Câu điều kiện loại 1: If + S + V(hiện tại đơn), S + will + V-inf -> 'studies'.",
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = 30101, KyHieuLuaChon = "A", NoiDungLuaChon = "studies", LaDapAnDung = true },
                            new() { MaLuaChon = 30102, KyHieuLuaChon = "B", NoiDungLuaChon = "studied", LaDapAnDung = false },
                            new() { MaLuaChon = 30103, KyHieuLuaChon = "C", NoiDungLuaChon = "would study", LaDapAnDung = false },
                            new() { MaLuaChon = 30104, KyHieuLuaChon = "D", NoiDungLuaChon = "had studied", LaDapAnDung = false }
                        }
                    });
                    break;
            }

            return list;
        }

        /// <summary>
        /// Đảm bảo đề thi tồn tại trong bảng BaiTapDeThi để thỏa mãn khóa ngoại FK_LuotLamBai_BaiTapDeThi
        /// </summary>
        public int EnsureDeThiExists(int maDeThi, string? tenDe = null, string? capDo = null, string? loaiHinh = null)
        {
            try
            {
                // 1. Kiểm tra mã đề hiện tại trong database
                string checkQuery = "SELECT COUNT(1) FROM BaiTapDeThi WHERE MaBaiTapDeThi = @MaDe";
                object? countObj = SqlDataProvider.Instance.ExecuteScalar(checkQuery, new[] { new SqlParameter("@MaDe", maDeThi) });
                if (countObj != null && Convert.ToInt32(countObj) > 0)
                {
                    return maDeThi;
                }

                // 2. Tìm thông tin đề thi trong danh sách chuẩn hóa
                var std = TaoDanhSachDeThiChuanHoaTatCaCapDo().FirstOrDefault(x => x.MaBaiTapDeThi == maDeThi);
                string ten = !string.IsNullOrWhiteSpace(tenDe) ? tenDe : (std?.TenBaiTapDeThi ?? $"Bài thi chuẩn hóa #{maDeThi}");
                string loai = !string.IsNullOrWhiteSpace(loaiHinh) ? loaiHinh : (std?.LoaiHinh ?? "ThiThu");
                string moTa = std?.MoTa ?? "Bộ đề thi chuẩn hóa năng lực tiếng Anh.";
                int thoiGian = std?.ThoiGianLamBaiPhut ?? 30;
                int tongSoCau = std?.SoLuongCauHoi ?? 10;
                string levelStr = !string.IsNullOrWhiteSpace(capDo) ? capDo : (std?.MaCapDo ?? "B1");

                // Lấy MaTrinhDo từ bảng TrinhDo
                object? maTrinhDoObj = SqlDataProvider.Instance.ExecuteScalar(
                    "SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo = @CapDo",
                    new[] { new SqlParameter("@CapDo", levelStr) });
                object dbTrinhDo = maTrinhDoObj ?? DBNull.Value;

                // Thử chèn với IDENTITY_INSERT để giữ nguyên MaBaiTapDeThi (ví dụ 101, 201...)
                try
                {
                    string insertWithIdentity = @"
                        SET IDENTITY_INSERT BaiTapDeThi ON;
                        INSERT INTO BaiTapDeThi (MaBaiTapDeThi, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, TongSoCau, DaPhatHanh, MaTrinhDo, NgayTao)
                        VALUES (@MaDe, @Ten, @LoaiHinh, @MoTa, @ThoiGian, @TongSoCau, 1, @MaTrinhDo, SYSDATETIME());
                        SET IDENTITY_INSERT BaiTapDeThi OFF;";

                    SqlParameter[] p = new[]
                    {
                        new SqlParameter("@MaDe", maDeThi),
                        new SqlParameter("@Ten", ten),
                        new SqlParameter("@LoaiHinh", loai),
                        new SqlParameter("@MoTa", moTa),
                        new SqlParameter("@ThoiGian", thoiGian),
                        new SqlParameter("@TongSoCau", tongSoCau),
                        new SqlParameter("@MaTrinhDo", dbTrinhDo)
                    };
                    SqlDataProvider.Instance.ExecuteNonQuery(insertWithIdentity, p);
                    return maDeThi;
                }
                catch (Exception exId)
                {
                    System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] IDENTITY_INSERT không khả dụng: {exId.Message}");
                }

                // 3. Nếu IDENTITY_INSERT không thành công, chèn tự tăng thông thường
                string insertAuto = @"
                    INSERT INTO BaiTapDeThi (TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, TongSoCau, DaPhatHanh, MaTrinhDo, NgayTao)
                    VALUES (@Ten, @LoaiHinh, @MoTa, @ThoiGian, @TongSoCau, 1, @MaTrinhDo, SYSDATETIME());
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                SqlParameter[] pAuto = new[]
                {
                    new SqlParameter("@Ten", ten),
                    new SqlParameter("@LoaiHinh", loai),
                    new SqlParameter("@MoTa", moTa),
                    new SqlParameter("@ThoiGian", thoiGian),
                    new SqlParameter("@TongSoCau", tongSoCau),
                    new SqlParameter("@MaTrinhDo", dbTrinhDo)
                };

                object? newIdObj = SqlDataProvider.Instance.ExecuteScalar(insertAuto, pAuto);
                if (newIdObj != null && Convert.ToInt32(newIdObj) > 0)
                {
                    return Convert.ToInt32(newIdObj);
                }

                // 4. Nếu vẫn không tạo được, dùng đề thi có sẵn bất kỳ trong CSDL
                object? existingId = SqlDataProvider.Instance.ExecuteScalar("SELECT TOP 1 MaBaiTapDeThi FROM BaiTapDeThi ORDER BY MaBaiTapDeThi ASC");
                if (existingId != null && Convert.ToInt32(existingId) > 0)
                {
                    return Convert.ToInt32(existingId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL.EnsureDeThiExists] {ex.Message}");
            }

            return maDeThi;
        }

        public int LuuKetQuaThi(int maNguoiDung, int maDeThi, string hinhThucLam, DateTime batDau,
                                int thoiGianGiay, int tongSoCau, int soCauDung, decimal diemSo,
                                List<(int MaCauHoi, int? MaLuaChon, bool DungSai)> chiTiet)
        {
            try
            {
                // Đảm bảo mã người dùng hợp lệ
                int validMaND = maNguoiDung;
                try
                {
                    object? ndCheck = SqlDataProvider.Instance.ExecuteScalar(
                        "SELECT COUNT(1) FROM NguoiDung WHERE MaNguoiDung = @MaND",
                        new[] { new SqlParameter("@MaND", maNguoiDung) });
                    if (ndCheck == null || Convert.ToInt32(ndCheck) == 0)
                    {
                        object? firstND = SqlDataProvider.Instance.ExecuteScalar("SELECT TOP 1 MaNguoiDung FROM NguoiDung ORDER BY MaNguoiDung ASC");
                        if (firstND != null && Convert.ToInt32(firstND) > 0)
                            validMaND = Convert.ToInt32(firstND);
                    }
                }
                catch { }

                // Đảm bảo mã đề thi hợp lệ
                int validMaDe = EnsureDeThiExists(maDeThi);

                int soCauSai = Math.Max(0, tongSoCau - soCauDung);

                string queryLuot = @"
                    INSERT INTO LuotLamBai (MaNguoiDung, MaBaiTapDeThi, HinhThucLam, ThoiDiemBatDau, ThoiDiemNopBai, 
                                            ThoiGianLamGiay, TongSoCau, SoCauDung, SoCauSai, DiemSo, TrangThai)
                    VALUES (@MaND, @MaDe, @HinhThuc, @BatDau, SYSDATETIME(), @ThoiGian, @TongSo, @SoDung, @SoSai, @Diem, N'DaNop');
                    SELECT SCOPE_IDENTITY();";

                SqlParameter[] pLuot = new[]
                {
                    new SqlParameter("@MaND", validMaND),
                    new SqlParameter("@MaDe", validMaDe),
                    new SqlParameter("@HinhThuc", hinhThucLam),
                    new SqlParameter("@BatDau", batDau),
                    new SqlParameter("@ThoiGian", thoiGianGiay),
                    new SqlParameter("@TongSo", tongSoCau),
                    new SqlParameter("@SoDung", soCauDung),
                    new SqlParameter("@SoSai", soCauSai),
                    new SqlParameter("@Diem", diemSo)
                };

                object? idObj = SqlDataProvider.Instance.ExecuteScalar(queryLuot, pLuot);
                int maLuotLamBai = Convert.ToInt32(idObj);

                // Lưu chi tiết từng câu trả lời
                foreach (var item in chiTiet)
                {
                    bool cauHoiHopLe = false;
                    bool luaChonHopLe = false;
                    try
                    {
                        object? chCheck = SqlDataProvider.Instance.ExecuteScalar(
                            "SELECT COUNT(1) FROM CauHoi WHERE MaCauHoi = @MaCH",
                            new[] { new SqlParameter("@MaCH", item.MaCauHoi) });
                        cauHoiHopLe = chCheck != null && Convert.ToInt32(chCheck) > 0;

                        if (item.MaLuaChon.HasValue && item.MaLuaChon.Value > 0)
                        {
                            object? lcCheck = SqlDataProvider.Instance.ExecuteScalar(
                                "SELECT COUNT(1) FROM LuaChonDapAn WHERE MaLuaChon = @MaLC",
                                new[] { new SqlParameter("@MaLC", item.MaLuaChon.Value) });
                            luaChonHopLe = lcCheck != null && Convert.ToInt32(lcCheck) > 0;
                        }
                    }
                    catch { }

                    if (cauHoiHopLe)
                    {
                        string queryChiTiet = @"
                            INSERT INTO ChiTietTraLoi (MaLuotLamBai, MaCauHoi, MaLuaChonDaChon, KetQuaDungSai, DiemSo, ThoiDiemTraLoi)
                            VALUES (@MaLuot, @MaCauHoi, @MaLC, @DungSai, @DiemCau, SYSDATETIME());";

                        decimal diemCau = item.DungSai ? (10.0m / Math.Max(1, tongSoCau)) : 0;

                        SqlParameter[] pCT = new[]
                        {
                            new SqlParameter("@MaLuot", maLuotLamBai),
                            new SqlParameter("@MaCauHoi", item.MaCauHoi),
                            new SqlParameter("@MaLC", luaChonHopLe && item.MaLuaChon.HasValue ? (object)item.MaLuaChon.Value : DBNull.Value),
                            new SqlParameter("@DungSai", item.DungSai ? 1 : 0),
                            new SqlParameter("@DiemCau", diemCau)
                        };

                        try
                        {
                            SqlDataProvider.Instance.ExecuteNonQuery(queryChiTiet, pCT);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lưu ChiTietTraLoi: {ex.Message}");
                        }
                    }
                }

                // Ghi nhận vào bảng LichSuHocTap
                try
                {
                    string queryLichSu = @"
                        INSERT INTO LichSuHocTap (MaNguoiDung, MaLuotLamBai, LoaiHoatDong, SoPhutHoc, DiemSo, SoCauDung, SoCauSai, NgayGhiNhan)
                        VALUES (@MaND, @MaLuot, @LoaiHD, @SoPhut, @Diem, @SoDung, @SoSai, SYSDATETIME());";

                    int soPhut = Math.Max(1, thoiGianGiay / 60);
                    SqlParameter[] pLS = new[]
                    {
                        new SqlParameter("@MaND", validMaND),
                        new SqlParameter("@MaLuot", maLuotLamBai),
                        new SqlParameter("@LoaiHD", hinhThucLam),
                        new SqlParameter("@SoPhut", soPhut),
                        new SqlParameter("@Diem", diemSo),
                        new SqlParameter("@SoDung", soCauDung),
                        new SqlParameter("@SoSai", soCauSai)
                    };
                    SqlDataProvider.Instance.ExecuteNonQuery(queryLichSu, pLS);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lưu LichSuHocTap: {ex.Message}");
                }

                return maLuotLamBai;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL.LuuKetQuaThi] {ex.Message}");
                return 0;
            }
        }

        public LuotLamBaiDTO? LayChiTietKetQuaLamBai(int maLuotLamBai)
        {
            try
            {
                string queryLuot = @"
                    SELECT lb.MaLuotLamBai, lb.MaNguoiDung, lb.MaBaiTapDeThi, dt.TenBaiTapDeThi,
                           lb.HinhThucLam, lb.ThoiDiemBatDau, lb.ThoiDiemNopBai, lb.ThoiGianLamGiay,
                           lb.TongSoCau, lb.SoCauDung, lb.SoCauSai, lb.DiemSo, lb.TrangThai
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
                    TrangThai = row["TrangThai"]?.ToString() ?? "DaNop"
                };

                // Lấy danh sách câu hỏi kèm đáp án chọn và giải thích
                string queryCT = @"
                    SELECT ct.MaChiTietTraLoi, ct.MaCauHoi, ch.NoiDungCauHoi, ch.DoanVanDoc, ch.KyNang,
                           ch.GiaiThichDapAn, ct.MaLuaChonDaChon, ct.KetQuaDungSai, ct.DiemSo,
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
                        MaLuaChonDung = r["MaLuaChonDung"] == DBNull.Value ? 0 : Convert.ToInt32(r["MaLuaChonDung"]),
                        KyHieuDapAnDung = r["KyHieuDapAnDung"]?.ToString() ?? "",
                        NoiDungDapAnDung = r["NoiDungDapAnDung"]?.ToString() ?? "",
                        KetQuaDungSai = r["KetQuaDungSai"] == DBNull.Value ? false : Convert.ToBoolean(r["KetQuaDungSai"]),
                        DiemSo = r["DiemSo"] == DBNull.Value ? 0 : Convert.ToDecimal(r["DiemSo"]),
                        GiaiThichDapAn = r["GiaiThichDapAn"] == DBNull.Value ? null : r["GiaiThichDapAn"]?.ToString()
                    };

                    // Lấy tất cả lựa chọn
                    try
                    {
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
                    }
                    catch { }

                    dto.DanhSachChiTiet.Add(item);
                }

                return dto;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL.LayChiTietKetQuaLamBai] {ex.Message}");
                return null;
            }
        }

        public List<LuotLamBaiDTO> LayLichSuThi(int maNguoiDung, int top = 5)
        {
            var list = new List<LuotLamBaiDTO>();
            string query = @"
                SELECT TOP (@Top) lb.MaLuotLamBai, lb.MaNguoiDung, lb.MaBaiTapDeThi, dt.TenBaiTapDeThi,
                       lb.HinhThucLam, lb.ThoiDiemBatDau, lb.ThoiDiemNopBai, lb.ThoiGianLamGiay,
                       lb.TongSoCau, lb.SoCauDung, lb.SoCauSai, lb.DiemSo, lb.TrangThai
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
                        TrangThai = row["TrangThai"]?.ToString() ?? "DaNop"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiTapDeThiDAL] Lỗi lấy lịch sử thi: {ex.Message}");
            }
            return list;
        }

        /// <summary>
        /// [US-GV07] Lưu cấu hình đề thi và danh sách câu hỏi n-n sử dụng SqlTransaction an toàn
        /// </summary>
        public bool LuuDeThiVaCauHoi(BaiTapDeThiDTO deThi, List<(int MaCauHoi, int ThuTu, double Diem)> dsCauHoi, int nguoiTaoId, out string errorMessage)
        {
            errorMessage = string.Empty;
            using var conn = SqlDataProvider.Instance.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                int maDeThi = deThi.MaBaiTapDeThi;
                int tongSoCau = dsCauHoi.Count;

                if (maDeThi <= 0)
                {
                    string sqlInsert = @"
                        INSERT INTO BaiTapDeThi (TenBaiTapDeThi, LoaiHinh, MaTrinhDo, MaChuDe, ThoiGianLamBaiPhut,
                                                DiemDat, TongDiem, TongSoCau, XaoTronCauHoi, DaPhatHanh, NguoiTao, NgayTao)
                        VALUES (@TenDe, @LoaiHinh, @MaTrinhDo, @MaChuDe, @ThoiGian, 
                                @DiemDat, @TongDiem, @TongSoCau, @XaoTron, @DaPhatHanh, @NguoiTao, SYSDATETIME());
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
                    cmd.Parameters.AddWithValue("@NguoiTao", nguoiTaoId > 0 ? (object)nguoiTaoId : DBNull.Value);

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
                            DaPhatHanh = @DaPhatHanh
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

                    cmd.ExecuteNonQuery();
                }

                // Gán danh sách câu hỏi vào bảng trung gian CauHoiBaiTap
                string sqlInsertDetail = @"
                    INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
                    VALUES (@MaDe, @MaCauHoi, @ThuTu, @DiemSo);";

                foreach (var item in dsCauHoi)
                {
                    using var cmdDetail = new SqlCommand(sqlInsertDetail, conn, trans);
                    cmdDetail.Parameters.AddWithValue("@MaDe", maDeThi);
                    cmdDetail.Parameters.AddWithValue("@MaCauHoi", item.MaCauHoi);
                    cmdDetail.Parameters.AddWithValue("@ThuTu", item.ThuTu);
                    cmdDetail.Parameters.AddWithValue("@DiemSo", (decimal)item.Diem);
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
    }
}
