using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO;

namespace DAL
{
    public class BaiHocDAL
    {
        private static BaiHocDAL? _instance;
        public static BaiHocDAL Instance => _instance ??= new BaiHocDAL();

        public List<BaiHocDTO> LayDanhSachBaiHoc(int maNguoiDung = 0)
        {
            var list = new List<BaiHocDTO>();

            string query = @"
                SELECT bh.MaBaiHoc, bh.TieuDe, bh.LoaiBaiHoc, td.MaCapDo AS MaTrinhDo, 
                       cd.TenChuDe, bh.TomTat, bh.NoiDung, bh.ThoiGianUocTinhPhut,
                       ISNULL(bh.DaXuatBan, 0) AS DaXuatBan,
                       ISNULL(bh.AudioPath, bh.DuongDanAudio) AS AudioPath,
                       ISNULL(tdbh.PhanTramTienDo, 0) AS PhanTramTienDo,
                       ISNULL(tdbh.TrangThai, N'ChuaHoc') AS TrangThai
                FROM BaiHoc bh
                LEFT JOIN TrinhDo td ON bh.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON bh.MaChuDe = cd.MaChuDe
                LEFT JOIN TienDoBaiHoc tdbh ON bh.MaBaiHoc = tdbh.MaBaiHoc AND tdbh.MaNguoiDung = @MaND
                WHERE bh.DaXuatBan = 1 OR bh.DaXuatBan = 0
                ORDER BY bh.MaBaiHoc ASC";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] { new SqlParameter("@MaND", maNguoiDung) });

                foreach (DataRow row in dt.Rows)
                {
                    string loai = row["LoaiBaiHoc"]?.ToString() ?? "NguPhap";
                    double phanTram = Convert.ToDouble(row["PhanTramTienDo"]);
                    string trangThaiRaw = row["TrangThai"]?.ToString() ?? "ChuaHoc";

                    string trangThaiHienThi = trangThaiRaw switch
                    {
                        "HoanThanh" => "Hoàn thành",
                        "DangHoc" => "Đang học",
                        _ => "Chưa học"
                    };

                    string tenLoai = loai switch
                    {
                        "TuVung" => "Từ vựng",
                        "NguPhap" => "Ngữ pháp",
                        "Doc" => "Đọc hiểu",
                        "Nghe" => "Nghe hiểu",
                        "Viet" => "Viết luận",
                        _ => loai
                    };

                    bool daXuatBan = row["DaXuatBan"] != DBNull.Value && Convert.ToBoolean(row["DaXuatBan"]);
                    string? audio = row["AudioPath"] == DBNull.Value ? null : row["AudioPath"]?.ToString();

                    string iconSkill = loai switch
                    {
                        "TuVung" => "📖",
                        "NguPhap" => "📝",
                        "Doc" or "DocHieu" => "📚",
                        "Nghe" or "NgheHieu" => "🎧",
                        "Viet" or "VietLuan" => "✍️",
                        _ => "📖"
                    };

                    string badgeBg = loai switch
                    {
                        "TuVung" => "#FEF3C7",
                        "NguPhap" => "#E0E7FF",
                        "Doc" or "DocHieu" => "#ECFDF5",
                        "Nghe" or "NgheHieu" => "#EFF6FF",
                        "Viet" or "VietLuan" => "#FDF2F8",
                        _ => "#E6F7F3"
                    };

                    string badgeFg = loai switch
                    {
                        "TuVung" => "#B45309",
                        "NguPhap" => "#4338CA",
                        "Doc" or "DocHieu" => "#047857",
                        "Nghe" or "NgheHieu" => "#1D4ED8",
                        "Viet" or "VietLuan" => "#BE185D",
                        _ => "#0F766E"
                    };

                    var dto = new BaiHocDTO
                    {
                        MaBaiHoc = Convert.ToInt32(row["MaBaiHoc"]),
                        TieuDe = row["TieuDe"]?.ToString() ?? "",
                        LoaiBaiHoc = loai,
                        TenLoaiHienThi = tenLoai,
                        MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B1",
                        TenChuDe = row["TenChuDe"]?.ToString() ?? "Tổng quát",
                        TomTat = row["TomTat"]?.ToString() ?? "",
                        NoiDung = row["NoiDung"]?.ToString() ?? "",
                        ThoiGianUocTinhPhut = row["ThoiGianUocTinhPhut"] == DBNull.Value ? 15 : Convert.ToInt32(row["ThoiGianUocTinhPhut"]),
                        DaXuatBan = daXuatBan,
                        AudioPath = audio,
                        DuongDanAudio = audio,
                        PhanTramTienDo = phanTram,
                        TrangThai = trangThaiHienThi,
                        IconSkill = iconSkill,
                        BadgeBackground = badgeBg,
                        BadgeForeground = badgeFg
                    };

                    if (phanTram >= 100)
                    {
                        dto.StatusColor = "#0F766E";
                        dto.StatusBackground = "#E6F7F3";
                        dto.ButtonActionText = "Ôn tập lại";
                    }
                    else if (phanTram > 0)
                    {
                        dto.StatusColor = "#2563EB";
                        dto.StatusBackground = "#EFF6FF";
                        dto.ButtonActionText = "Học tiếp";
                    }

                    list.Add(dto);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi truy vấn BaiHocDAL: {ex.Message}");
            }

            return list;
        }

        public void CapNhatTienDo(int maNguoiDung, int maBaiHoc, double phanTram, string trangThai)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM TienDoBaiHoc WHERE MaNguoiDung = @MaND AND MaBaiHoc = @MaBai)
                BEGIN
                    UPDATE TienDoBaiHoc
                    SET PhanTramTienDo = @PhanTram,
                        TrangThai = @TrangThai,
                        LanTruyCapCuoi = SYSDATETIME(),
                        NgayHoanThanh = CASE WHEN @PhanTram >= 100 THEN SYSDATETIME() ELSE NgayHoanThanh END
                    WHERE MaNguoiDung = @MaND AND MaBaiHoc = @MaBai;
                END
                ELSE
                BEGIN
                    INSERT INTO TienDoBaiHoc (MaNguoiDung, MaBaiHoc, TrangThai, PhanTramTienDo, NgayBatDau, LanTruyCapCuoi)
                    VALUES (@MaND, @MaBai, @TrangThai, @PhanTram, SYSDATETIME(), SYSDATETIME());
                END";

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@MaBai", maBaiHoc),
                new SqlParameter("@PhanTram", phanTram),
                new SqlParameter("@TrangThai", trangThai)
            };

            SqlDataProvider.Instance.ExecuteNonQuery(query, parameters);
        }

        public void CongDonThoiGianHoc(int maNguoiDung, int soPhut)
        {
            if (maNguoiDung <= 0 || soPhut <= 0) return;
            string query = @"
                IF EXISTS (SELECT 1 FROM HoSoHocTap WHERE MaNguoiDung = @MaND)
                BEGIN
                    UPDATE HoSoHocTap 
                    SET SoPhutHocTichLuy = SoPhutHocTichLuy + @SoPhut,
                        NgayCapNhat = SYSDATETIME()
                    WHERE MaNguoiDung = @MaND;
                END
                ELSE
                BEGIN
                    INSERT INTO HoSoHocTap (MaNguoiDung, SoPhutHocTichLuy, NgayCapNhat)
                    VALUES (@MaND, @SoPhut, SYSDATETIME());
                END";

            SqlDataProvider.Instance.ExecuteNonQuery(query, new[] { 
                new SqlParameter("@MaND", maNguoiDung),
                new SqlParameter("@SoPhut", soPhut)
            });
        }

        public List<BaiHocDTO> LayDanhSachBaiHocGanDay(int maNguoiDung, int top = 5)
        {
            var list = new List<BaiHocDTO>();
            string query = @"
                SELECT TOP (@Top) bh.MaBaiHoc, bh.TieuDe, bh.LoaiBaiHoc, td.MaCapDo AS MaTrinhDo, 
                       cd.TenChuDe, bh.TomTat, bh.ThoiGianUocTinhPhut,
                       ISNULL(tdbh.PhanTramTienDo, 0) AS PhanTramTienDo,
                       ISNULL(tdbh.TrangThai, N'ChuaHoc') AS TrangThai,
                       tdbh.LanTruyCapCuoi
                FROM TienDoBaiHoc tdbh
                INNER JOIN BaiHoc bh ON tdbh.MaBaiHoc = bh.MaBaiHoc
                LEFT JOIN TrinhDo td ON bh.MaTrinhDo = td.MaTrinhDo
                LEFT JOIN ChuDe cd ON bh.MaChuDe = cd.MaChuDe
                WHERE tdbh.MaNguoiDung = @MaND
                ORDER BY tdbh.LanTruyCapCuoi DESC";

            try
            {
                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(query, new[] {
                    new SqlParameter("@MaND", maNguoiDung),
                    new SqlParameter("@Top", top)
                });

                foreach (DataRow row in dt.Rows)
                {
                    double phanTram = Convert.ToDouble(row["PhanTramTienDo"]);
                    string trangThaiRaw = row["TrangThai"]?.ToString() ?? "ChuaHoc";
                    string trangThaiHienThi = trangThaiRaw switch
                    {
                        "HoanThanh" => "Hoàn thành",
                        "DangHoc" => "Đang học",
                        _ => "Chưa học"
                    };

                    DateTime lanCuoi = row["LanTruyCapCuoi"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["LanTruyCapCuoi"]);
                    string ngayHocText = (DateTime.Now.Date - lanCuoi.Date).TotalDays switch
                    {
                        0 => "Hôm nay",
                        1 => "Hôm qua",
                        _ => lanCuoi.ToString("dd/MM/yyyy")
                    };

                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = Convert.ToInt32(row["MaBaiHoc"]),
                        TieuDe = row["TieuDe"]?.ToString() ?? "",
                        LoaiBaiHoc = row["LoaiBaiHoc"]?.ToString() ?? "NguPhap",
                        TenLoaiHienThi = row["LoaiBaiHoc"]?.ToString() switch
                        {
                            "TuVung" => "Từ vựng",
                            "NguPhap" => "Ngữ pháp",
                            "Doc" => "Đọc hiểu",
                            "Nghe" => "Nghe hiểu",
                            "Viet" => "Viết luận",
                            _ => "Bài học"
                        },
                        MaTrinhDo = row["MaTrinhDo"]?.ToString() ?? "B1",
                        TenChuDe = row["TenChuDe"]?.ToString() ?? "Tổng quát",
                        TomTat = row["TomTat"]?.ToString() ?? "",
                        ThoiGianUocTinhPhut = row["ThoiGianUocTinhPhut"] == DBNull.Value ? 15 : Convert.ToInt32(row["ThoiGianUocTinhPhut"]),
                        PhanTramTienDo = phanTram,
                        TrangThai = trangThaiHienThi,
                        NgayHocGanNhat = ngayHocText,
                        DiemSo = phanTram >= 100 ? "10/10" : (phanTram > 50 ? "8/10" : "5/10")
                    });
                }

                // Nếu chưa có bài học nào đang học, lấy 2 bài học tiêu biểu từ DB để gợi ý
                if (list.Count == 0)
                {
                    var all = LayDanhSachBaiHoc(maNguoiDung);
                    if (all.Count > 0)
                    {
                        for (int i = 0; i < Math.Min(2, all.Count); i++)
                        {
                            var b = all[i];
                            b.NgayHocGanNhat = "Chưa học";
                            b.DiemSo = "-";
                            list.Add(b);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocDAL] Lỗi lấy bài học gần đây: {ex.Message}");
            }

            return list;
        }

        public ThongKeHocTapDTO LayThongKeSinhVien(int maNguoiDung)
        {
            var dto = new ThongKeHocTapDTO();
            if (maNguoiDung <= 0) return dto;

            try
            {
                // 1. Số bài đã hoàn thành & tổng số bài
                string queryBai = @"
                    SELECT 
                        (SELECT COUNT(*) FROM TienDoBaiHoc WHERE MaNguoiDung = @MaND AND TrangThai = N'HoanThanh') AS SoHoanThanh,
                        (SELECT COUNT(*) FROM BaiHoc WHERE DaXuatBan = 1 OR DaXuatBan = 0) AS TongBai,
                        (SELECT ISNULL(SoPhutHocTichLuy, 0) FROM HoSoHocTap WHERE MaNguoiDung = @MaND) AS SoPhut,
                        (SELECT COUNT(*) FROM LuotLamBai WHERE MaNguoiDung = @MaND AND TrangThai = N'DaNop') AS TongLuotThi,
                        (SELECT AVG(DiemSo * 10) FROM LuotLamBai WHERE MaNguoiDung = @MaND AND TrangThai = N'DaNop') AS DiemTBThi,
                        (SELECT TOP 1 DiemDatDuoc * 10 FROM LuotKhaoSat WHERE MaNguoiDung = @MaND ORDER BY MaLuotKhaoSat DESC) AS DiemKhaoSat";

                DataTable dt = SqlDataProvider.Instance.ExecuteQuery(queryBai, new[] { new SqlParameter("@MaND", maNguoiDung) });
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    dto.SoBaiDaHoanThanh = r["SoHoanThanh"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoHoanThanh"]);
                    dto.TongSoBaiHoc = r["TongBai"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongBai"]);
                    dto.SoPhutHocTichLuy = r["SoPhut"] == DBNull.Value ? 0 : Convert.ToInt32(r["SoPhut"]);
                    dto.TongSoLuotThi = r["TongLuotThi"] == DBNull.Value ? 0 : Convert.ToInt32(r["TongLuotThi"]);

                    double diemThi = r["DiemTBThi"] == DBNull.Value ? 0 : Convert.ToDouble(r["DiemTBThi"]);
                    double diemKS = r["DiemKhaoSat"] == DBNull.Value ? 0 : Convert.ToDouble(r["DiemKhaoSat"]);

                    dto.DiemTrungBinh = diemThi > 0 ? diemThi : (diemKS > 0 ? diemKS : 75.0);
                    dto.ChuoiHocLienTiep = dto.SoPhutHocTichLuy > 0 ? Math.Max(1, dto.SoPhutHocTichLuy / 30) : 1;
                }

                // 2. Tỷ lệ theo kỹ năng
                string querySkills = @"
                    SELECT 
                        ch.KyNang, 
                        COUNT(*) AS TongCau,
                        SUM(CASE WHEN ct.KetQuaDungSai = 1 THEN 1 ELSE 0 END) AS SoDung
                    FROM ChiTietTraLoi ct
                    INNER JOIN LuotLamBai lb ON ct.MaLuotLamBai = lb.MaLuotLamBai
                    INNER JOIN CauHoi ch ON ct.MaCauHoi = ch.MaCauHoi
                    WHERE lb.MaNguoiDung = @MaND
                    GROUP BY ch.KyNang";

                DataTable dtSkills = SqlDataProvider.Instance.ExecuteQuery(querySkills, new[] { new SqlParameter("@MaND", maNguoiDung) });
                foreach (DataRow r in dtSkills.Rows)
                {
                    string kn = r["KyNang"]?.ToString() ?? "";
                    int tong = Convert.ToInt32(r["TongCau"]);
                    int dung = Convert.ToInt32(r["SoDung"]);
                    int pct = tong > 0 ? (dung * 100 / tong) : 70;

                    if (kn == "TuVung") dto.TyLeTuVung = pct;
                    else if (kn == "NguPhap") dto.TyLeNguPhap = pct;
                    else if (kn == "Doc") dto.TyLeDocHieu = pct;
                    else if (kn == "Nghe") dto.TyLeNgheHieu = pct;
                }

                // 3. Danh sách bài học gần đây
                dto.DanhSachBaiHocGanDay = LayDanhSachBaiHocGanDay(maNguoiDung, 4);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocDAL] Lỗi tính thống kê: {ex.Message}");
            }

            return dto;
        }

        public List<BaiHocDTO> LayDanhSachBaiHocTheoTrinhDo(string maCapDo, int maNguoiDung = 0, int top = 3)
        {
            var all = LayDanhSachBaiHoc(maNguoiDung);
            var filtered = new List<BaiHocDTO>();

            foreach (var b in all)
            {
                if (string.Equals(b.MaTrinhDo, maCapDo, StringComparison.OrdinalIgnoreCase))
                {
                    filtered.Add(b);
                    if (filtered.Count >= top) break;
                }
            }

            if (filtered.Count == 0 && all.Count > 0)
            {
                for (int i = 0; i < Math.Min(top, all.Count); i++)
                {
                    filtered.Add(all[i]);
                }
            }

            if (filtered.Count == 0)
            {
                filtered = TaoDanhSachBaiHocDuPhongTheoTrinhDo(maCapDo);
            }

            return filtered;
        }

        public static List<BaiHocDTO> TaoDanhSachBaiHocDuPhongTheoTrinhDo(string maCapDo)
        {
            var list = new List<BaiHocDTO>();
            switch (maCapDo.ToUpper())
            {
                case "A1":
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 1001,
                        TieuDe = "Grammar A1 - Personal Pronouns & To Be Verb",
                        LoaiBaiHoc = "NguPhap",
                        TenLoaiHienThi = "Ngữ pháp",
                        MaTrinhDo = "A1",
                        TenChuDe = "Đời sống",
                        TomTat = "Làm quen với đại từ nhân xưng I, You, We, They, He, She, It và cách chia động từ To Be.",
                        ThoiGianUocTinhPhut = 15,
                        BadgeBackground = "#E6F7F3",
                        BadgeForeground = "#0F766E",
                        IconSkill = "📖"
                    });
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 1002,
                        TieuDe = "Vocabulary A1 - Daily Routine & Common Greetings",
                        LoaiBaiHoc = "TuVung",
                        TenLoaiHienThi = "Từ vựng",
                        MaTrinhDo = "A1",
                        TenChuDe = "Đời sống",
                        TomTat = "Học các cụm từ vựng thông dụng về thói quen buổi sáng, lời chào hỏi và giới thiệu bản thân.",
                        ThoiGianUocTinhPhut = 15,
                        BadgeBackground = "#FEF3C7",
                        BadgeForeground = "#D97706",
                        IconSkill = "💬"
                    });
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 1003,
                        TieuDe = "Reading A1 - Short Family Stories & Introductions",
                        LoaiBaiHoc = "Doc",
                        TenLoaiHienThi = "Đọc hiểu",
                        MaTrinhDo = "A1",
                        TenChuDe = "Đời sống",
                        TomTat = "Luyện đọc đoạn văn ngắn giới thiệu về các thành viên gia đình và sở thích cá nhân.",
                        ThoiGianUocTinhPhut = 15,
                        BadgeBackground = "#DBEAFE",
                        BadgeForeground = "#2563EB",
                        IconSkill = "📑"
                    });
                    break;

                case "A2":
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 2001,
                        TieuDe = "Grammar A2 - Past Simple vs Present Continuous",
                        LoaiBaiHoc = "NguPhap",
                        TenLoaiHienThi = "Ngữ pháp",
                        MaTrinhDo = "A2",
                        TenChuDe = "Đời sống",
                        TomTat = "Phân biệt cách dùng thì Quá khứ đơn (hành động đã kết thúc) và Hiện tại tiếp diễn.",
                        ThoiGianUocTinhPhut = 15,
                        BadgeBackground = "#E6F7F3",
                        BadgeForeground = "#0F766E",
                        IconSkill = "📖"
                    });
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 2002,
                        TieuDe = "Vocabulary A2 - Travel & Shopping Communication",
                        LoaiBaiHoc = "TuVung",
                        TenLoaiHienThi = "Từ vựng",
                        MaTrinhDo = "A2",
                        TenChuDe = "Đời sống",
                        TomTat = "Bộ từ vựng và mẫu câu hội thoại thường dùng khi đi du lịch, hỏi đường và mua sắm.",
                        ThoiGianUocTinhPhut = 18,
                        BadgeBackground = "#FEF3C7",
                        BadgeForeground = "#D97706",
                        IconSkill = "💬"
                    });
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 2003,
                        TieuDe = "Reading A2 - City Life & Holiday Plans",
                        LoaiBaiHoc = "Doc",
                        TenLoaiHienThi = "Đọc hiểu",
                        MaTrinhDo = "A2",
                        TenChuDe = "Đời sống",
                        TomTat = "Thực hành đọc hiểu thông tin biển báo, email đặt phòng khách sạn và kế hoạch kỳ nghỉ.",
                        ThoiGianUocTinhPhut = 15,
                        BadgeBackground = "#DBEAFE",
                        BadgeForeground = "#2563EB",
                        IconSkill = "📑"
                    });
                    break;

                case "B1":
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 3001,
                        TieuDe = "Grammar B1 - Present Perfect vs Past Simple",
                        LoaiBaiHoc = "NguPhap",
                        TenLoaiHienThi = "Ngữ pháp",
                        MaTrinhDo = "B1",
                        TenChuDe = "Đời sống",
                        TomTat = "Phân biệt thì Hiện tại hoàn thành và Quá khứ đơn, các trạng từ chỉ thời gian thường gặp.",
                        ThoiGianUocTinhPhut = 15,
                        BadgeBackground = "#E6F7F3",
                        BadgeForeground = "#0F766E",
                        IconSkill = "📖"
                    });
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 3002,
                        TieuDe = "Vocabulary B1 - Workplace Communication & Emails",
                        LoaiBaiHoc = "TuVung",
                        TenLoaiHienThi = "Từ vựng",
                        MaTrinhDo = "B1",
                        TenChuDe = "Kinh tế & Công sở",
                        TomTat = "Tổng hợp từ vựng và mẫu câu giao tiếp viết email công việc chuyên nghiệp chuẩn công sở.",
                        ThoiGianUocTinhPhut = 18,
                        BadgeBackground = "#FEF3C7",
                        BadgeForeground = "#D97706",
                        IconSkill = "💬"
                    });
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 3003,
                        TieuDe = "Reading B1 - Technology in Modern Education",
                        LoaiBaiHoc = "Doc",
                        TenLoaiHienThi = "Đọc hiểu",
                        MaTrinhDo = "B1",
                        TenChuDe = "Khoa học công nghệ",
                        TomTat = "Rèn luyện kỹ năng đọc hiểu bài báo khoa học về tác động của công nghệ trong giảng dạy.",
                        ThoiGianUocTinhPhut = 20,
                        BadgeBackground = "#DBEAFE",
                        BadgeForeground = "#2563EB",
                        IconSkill = "📑"
                    });
                    break;

                case "B2":
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 4001,
                        TieuDe = "Grammar B2 - Conditional Sentences (Câu điều kiện)",
                        LoaiBaiHoc = "NguPhap",
                        TenLoaiHienThi = "Ngữ pháp",
                        MaTrinhDo = "B2",
                        TenChuDe = "Học thuật",
                        TomTat = "Nắm vững cấu trúc, cách dùng câu điều kiện loại 0, 1, 2, 3 và câu điều kiện hỗn hợp.",
                        ThoiGianUocTinhPhut = 20,
                        BadgeBackground = "#E6F7F3",
                        BadgeForeground = "#0F766E",
                        IconSkill = "📖"
                    });
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 4002,
                        TieuDe = "Reading B2 - The Renaissance of Clean Energy",
                        LoaiBaiHoc = "Doc",
                        TenLoaiHienThi = "Đọc hiểu",
                        MaTrinhDo = "B2",
                        TenChuDe = "Khoa học công nghệ",
                        TomTat = "Rèn luyện kỹ năng Skimming, Scanning và suy luận từ ngữ cảnh qua bài đọc Năng lượng xanh.",
                        ThoiGianUocTinhPhut = 18,
                        BadgeBackground = "#DBEAFE",
                        BadgeForeground = "#2563EB",
                        IconSkill = "📑"
                    });
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 4003,
                        TieuDe = "Vocabulary B2 - Idioms & Phrasal Verbs in Professional Context",
                        LoaiBaiHoc = "TuVung",
                        TenLoaiHienThi = "Từ vựng",
                        MaTrinhDo = "B2",
                        TenChuDe = "Kinh tế & Công sở",
                        TomTat = "Bộ thành ngữ và cụm động từ nâng cao giúp giao tiếp tự nhiên và chuẩn bản xứ.",
                        ThoiGianUocTinhPhut = 20,
                        BadgeBackground = "#FEF3C7",
                        BadgeForeground = "#D97706",
                        IconSkill = "💬"
                    });
                    break;

                case "C1":
                default:
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 5001,
                        TieuDe = "Vocabulary C1 - Academic & Essay Collocations",
                        LoaiBaiHoc = "TuVung",
                        TenLoaiHienThi = "Từ vựng",
                        MaTrinhDo = "C1",
                        TenChuDe = "Học thuật",
                        TomTat = "Bộ sưu tập 30 cụm từ học thuật C1 ghi điểm tuyệt đối trong IELTS Writing Task 2.",
                        ThoiGianUocTinhPhut = 25,
                        BadgeBackground = "#FEF3C7",
                        BadgeForeground = "#D97706",
                        IconSkill = "💬"
                    });
                    list.Add(new BaiHocDTO
                    {
                        MaBaiHoc = 5002,
                        TieuDe = "Grammar C1 - Inversion & Advanced Subjunctive Structures",
                        LoaiBaiHoc = "NguPhap",
                        TenLoaiHienThi = "Ngữ pháp",
                        MaTrinhDo = "C1",
                        TenChuDe = "Học thuật",
                        TomTat = "Cấu trúc đảo ngữ nhấn mạnh và thể giả định nâng cao trong văn phong học thuật chuyên sâu.",
                        ThoiGianUocTinhPhut = 25,
                        BadgeBackground = "#E6F7F3",
                        BadgeForeground = "#0F766E",
                        IconSkill = "📖"
                    });
                    break;
            }
            return list;
        }

        public List<BaiHocDTO> GetAll(int? maChuDe = null, string? search = null)
        {
            var list = new List<BaiHocDTO>();
            string query;
            var parameters = new List<SqlParameter>();

            if (maChuDe.HasValue && maChuDe.Value > 0)
            {
                // Sử dụng CTE đệ quy ở đầu câu lệnh để lấy cả chủ đề hiện tại và tất cả chủ đề con/cháu
                query = @"
                    ;WITH ChuDeHierarchy AS (
                        SELECT MaChuDe FROM ChuDe WHERE MaChuDe = @maChuDe
                        UNION ALL
                        SELECT c.MaChuDe FROM ChuDe c INNER JOIN ChuDeHierarchy h ON c.MaChuDeCha = h.MaChuDe
                    )
                    SELECT 
                        bh.MaBaiHoc,
                        bh.MaChuDe,
                        cd.TenChuDe,
                        bh.MaTrinhDo,
                        td.MaCapDo AS TenTrinhDo,
                        bh.NguoiTao,
                        bh.TieuDe,
                        bh.LoaiBaiHoc,
                        bh.TomTat,
                        bh.NoiDung,
                        bh.ThoiGianUocTinhPhut,
                        ISNULL(bh.DaXuatBan, 0) AS DaXuatBan,
                        ISNULL(bh.AudioPath, bh.DuongDanAudio) AS AudioPath,
                        bh.NgayTao
                    FROM BaiHoc bh
                    INNER JOIN ChuDe cd ON bh.MaChuDe = cd.MaChuDe
                    INNER JOIN TrinhDo td ON bh.MaTrinhDo = td.MaTrinhDo
                    WHERE bh.MaChuDe IN (SELECT MaChuDe FROM ChuDeHierarchy)";
                parameters.Add(new SqlParameter("@maChuDe", maChuDe.Value));
            }
            else
            {
                query = @"
                    SELECT 
                        bh.MaBaiHoc,
                        bh.MaChuDe,
                        cd.TenChuDe,
                        bh.MaTrinhDo,
                        td.MaCapDo AS TenTrinhDo,
                        bh.NguoiTao,
                        bh.TieuDe,
                        bh.LoaiBaiHoc,
                        bh.TomTat,
                        bh.NoiDung,
                        bh.ThoiGianUocTinhPhut,
                        ISNULL(bh.DaXuatBan, 0) AS DaXuatBan,
                        ISNULL(bh.AudioPath, bh.DuongDanAudio) AS AudioPath,
                        bh.NgayTao
                    FROM BaiHoc bh
                    INNER JOIN ChuDe cd ON bh.MaChuDe = cd.MaChuDe
                    INNER JOIN TrinhDo td ON bh.MaTrinhDo = td.MaTrinhDo
                    WHERE 1 = 1";
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query += " AND (bh.TieuDe LIKE @search OR bh.TomTat LIKE @search)";
                parameters.Add(new SqlParameter("@search", $"%{search.Trim()}%"));
            }

            query += " ORDER BY bh.MaBaiHoc DESC";

            var dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
            foreach (DataRow row in dt.Rows)
            {
                var loai = row["LoaiBaiHoc"]?.ToString() ?? "TuVung";
                var tenLoai = loai switch
                {
                    "NguPhap" => "Ngữ pháp",
                    "Doc" or "DocHieu" => "Đọc hiểu",
                    "Nghe" or "NgheHieu" => "Luyện nghe",
                    "Viet" or "VietLuan" => "Viết luận",
                    _ => "Từ vựng"
                };

                var icon = loai switch
                {
                    "NguPhap" => "📘",
                    "Doc" or "DocHieu" => "📕",
                    "Nghe" or "NgheHieu" => "🎧",
                    "Viet" or "VietLuan" => "✍️",
                    _ => "📙"
                };

                var badgeBg = loai switch
                {
                    "NguPhap" => "#E0E7FF",
                    "Doc" or "DocHieu" => "#FEE2E2",
                    "Nghe" or "NgheHieu" => "#FEF3C7",
                    "Viet" or "VietLuan" => "#F3E8FF",
                    _ => "#DCFCE7"
                };
                var badgeFg = loai switch
                {
                    "NguPhap" => "#4338CA",
                    "Doc" or "DocHieu" => "#B91C1C",
                    "Nghe" or "NgheHieu" => "#B45309",
                    "Viet" or "VietLuan" => "#7E22CE",
                    _ => "#15803D"
                };

                bool daXuatBan = row["DaXuatBan"] != DBNull.Value && Convert.ToBoolean(row["DaXuatBan"]);
                string? audio = row.Table.Columns.Contains("AudioPath") && row["AudioPath"] != DBNull.Value ? row["AudioPath"]?.ToString() : null;

                list.Add(new BaiHocDTO
                {
                    MaBaiHoc = Convert.ToInt32(row["MaBaiHoc"]),
                    MaChuDe = Convert.ToInt32(row["MaChuDe"]),
                    MaTrinhDoId = row["MaTrinhDo"] == DBNull.Value ? 3 : Convert.ToInt32(row["MaTrinhDo"]),
                    TieuDe = row["TieuDe"]?.ToString() ?? "",
                    TenChuDe = row["TenChuDe"]?.ToString() ?? "",
                    LoaiBaiHoc = loai,
                    TenLoaiHienThi = tenLoai,
                    MaTrinhDo = row["TenTrinhDo"]?.ToString() ?? "B1",
                    TomTat = row["TomTat"] == DBNull.Value ? "" : (row["TomTat"]?.ToString() ?? ""),
                    NoiDung = row["NoiDung"] == DBNull.Value ? "" : (row["NoiDung"]?.ToString() ?? ""),
                    ThoiGianUocTinhPhut = Convert.ToInt32(row["ThoiGianUocTinhPhut"]),
                    DaXuatBan = daXuatBan,
                    AudioPath = audio,
                    DuongDanAudio = audio,
                    TrangThai = daXuatBan ? "Đã xuất bản" : "Bản nháp",
                    IconSkill = icon,
                    BadgeBackground = badgeBg,
                    BadgeForeground = badgeFg,
                    StatusBackground = daXuatBan ? "#DCFCE7" : "#F1F5F9",
                    StatusColor = daXuatBan ? "#15803D" : "#64748B"
                });
            }

            return list;
        }

        public bool InsertBaiHoc(BaiHocDTO bh)
        {
            string audio = bh.AudioPath ?? bh.DuongDanAudio ?? "";
            string image = bh.AnhMinhHoaPath ?? bh.DuongDanHinhAnh ?? "";

            var query = @"
                INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, 
                                    DuongDanAudio, AudioPath, DuongDanHinhAnh, AnhMinhHoaPath, 
                                    ThoiGianUocTinhPhut, ThoiLuongUocTinh, DaXuatBan, NgayTao)
                VALUES (@MaChuDe, @MaTrinhDo, @NguoiTao, @TieuDe, @LoaiBaiHoc, @TomTat, @NoiDung, 
                        @Audio, @Audio, @Image, @Image, 
                        @ThoiGianUocTinhPhut, @ThoiGianUocTinhPhut, @DaXuatBan, SYSDATETIME());
                SELECT SCOPE_IDENTITY();";

            int maTrinhDo = 0;
            if (!string.IsNullOrWhiteSpace(bh.MaTrinhDo))
            {
                var trinhDoRow = DatabaseHelper.ExecuteScalar(
                    "SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE UPPER(LTRIM(RTRIM(MaCapDo))) = UPPER(LTRIM(RTRIM(@capDo)))", 
                    new[] { new SqlParameter("@capDo", bh.MaTrinhDo.Trim()) });
                if (trinhDoRow != null && trinhDoRow != DBNull.Value)
                {
                    maTrinhDo = Convert.ToInt32(trinhDoRow);
                    bh.MaTrinhDoId = maTrinhDo;
                }
            }

            if (maTrinhDo <= 0)
            {
                maTrinhDo = bh.MaTrinhDoId > 0 ? bh.MaTrinhDoId : 3;
            }

            var parameters = new[]
            {
                new SqlParameter("@MaChuDe", bh.MaChuDe),
                new SqlParameter("@MaTrinhDo", maTrinhDo),
                new SqlParameter("@NguoiTao", (object?)bh.NguoiTao ?? DBNull.Value),
                new SqlParameter("@TieuDe", bh.TieuDe),
                new SqlParameter("@LoaiBaiHoc", string.IsNullOrEmpty(bh.LoaiBaiHoc) ? "Doc" : bh.LoaiBaiHoc),
                new SqlParameter("@TomTat", (object?)bh.TomTat ?? DBNull.Value),
                new SqlParameter("@NoiDung", (object?)bh.NoiDung ?? DBNull.Value),
                new SqlParameter("@Audio", string.IsNullOrEmpty(audio) ? DBNull.Value : (object)audio),
                new SqlParameter("@Image", string.IsNullOrEmpty(image) ? DBNull.Value : (object)image),
                new SqlParameter("@ThoiGianUocTinhPhut", bh.ThoiGianUocTinhPhut > 0 ? bh.ThoiGianUocTinhPhut : 20),
                new SqlParameter("@DaXuatBan", bh.DaXuatBan)
            };

            var idObj = DatabaseHelper.ExecuteScalar(query, parameters);
            if (idObj != null && idObj != DBNull.Value)
            {
                bh.MaBaiHoc = Convert.ToInt32(idObj);
                return true;
            }
            return false;
        }

        public bool KiemTraTrungTieuDe(int maChuDe, string tieuDe, int? maBaiHocBoQua = null)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM BaiHoc WHERE MaChuDe = @MaChuDe AND LOWER(LTRIM(RTRIM(TieuDe))) = LOWER(LTRIM(RTRIM(@TieuDe)))";
                if (maBaiHocBoQua.HasValue && maBaiHocBoQua.Value > 0)
                {
                    query += " AND MaBaiHoc != @MaBaiHocBoQua";
                }

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@MaChuDe", maChuDe),
                    new SqlParameter("@TieuDe", tieuDe.Trim())
                };
                if (maBaiHocBoQua.HasValue && maBaiHocBoQua.Value > 0)
                {
                    parameters.Add(new SqlParameter("@MaBaiHocBoQua", maBaiHocBoQua.Value));
                }

                var res = DatabaseHelper.ExecuteScalar(query, parameters.ToArray());
                if (res != null && res != DBNull.Value)
                {
                    return Convert.ToInt32(res) > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocDAL.KiemTraTrungTieuDe] {ex.Message}");
            }
            return false;
        }

        public bool TaoDeThiThuTuBaiHoc(int maBaiHoc, string tenDeThi, int? nguoiTao = null)
        {
            var query = @"
                INSERT INTO BaiTapDeThi(MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
                SELECT 
                    bh.MaChuDe, 
                    bh.MaTrinhDo, 
                    @TenDeThi, 
                    N'ThiThu', 
                    N'Đề thi thử học thuật trích xuất từ tài liệu: ' + bh.TieuDe, 
                    ISNULL(bh.ThoiGianUocTinhPhut, 60), 
                    1, 
                    @NguoiTao
                FROM BaiHoc bh
                WHERE bh.MaBaiHoc = @MaBaiHoc;
                SELECT SCOPE_IDENTITY();";

            var parameters = new[]
            {
                new SqlParameter("@MaBaiHoc", maBaiHoc),
                new SqlParameter("@TenDeThi", tenDeThi),
                new SqlParameter("@NguoiTao", (object?)nguoiTao ?? DBNull.Value)
            };

            var id = DatabaseHelper.ExecuteScalar(query, parameters);
            return id != null && id != DBNull.Value;
        }

        public bool DeleteBaiHoc(int maBaiHoc)
        {
            try
            {
                const string query = @"
                    BEGIN TRY
                        BEGIN TRANSACTION;

                        -- Gỡ liên kết bài học ở các bảng liên quan
                        UPDATE CauHoi SET MaBaiHoc = NULL WHERE MaBaiHoc = @id;
                        UPDATE BaiTapDeThi SET MaBaiHoc = NULL WHERE MaBaiHoc = @id;
                        UPDATE LichSuHocTap SET MaBaiHoc = NULL WHERE MaBaiHoc = @id;
                        UPDATE DeXuatHocTap SET MaBaiHoc = NULL WHERE MaBaiHoc = @id;
                        UPDATE PhienHoiThoaiAI SET MaBaiHocLienQuan = NULL WHERE MaBaiHocLienQuan = @id;

                        -- Xóa tiến độ học tập gắn với bài học
                        DELETE FROM TienDoBaiHoc WHERE MaBaiHoc = @id;

                        -- Xóa bài học
                        DELETE FROM BaiHoc WHERE MaBaiHoc = @id;

                        COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        IF @@TRANCOUNT > 0
                            ROLLBACK TRANSACTION;
                        THROW;
                    END CATCH;";

                int rows = DatabaseHelper.ExecuteNonQuery(query, new[] { new SqlParameter("@id", maBaiHoc) });
                return rows > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocDAL.DeleteBaiHoc] Lỗi: {ex.Message}");
                throw;
            }
        }

        public bool DiChuyenBaiHoc(int maBaiHoc, int maChuDeMoi)
        {
            try
            {
                const string query = "UPDATE BaiHoc SET MaChuDe = @MaChuDeMoi, NgayCapNhat = SYSDATETIME() WHERE MaBaiHoc = @MaBaiHoc;";
                int rows = DatabaseHelper.ExecuteNonQuery(query, new[]
                {
                    new SqlParameter("@MaChuDeMoi", maChuDeMoi),
                    new SqlParameter("@MaBaiHoc", maBaiHoc)
                });
                return rows > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocDAL.DiChuyenBaiHoc] Lỗi: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// [US-GV05] Thêm mới hoặc Cập nhật Nội dung Bài học
        /// </summary>
        public bool LuuBaiHoc(BaiHocDTO baiHoc, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                // Đảm bảo MaTrinhDo chuẩn xác theo MaCapDo (A1, A2, B1, B2, C1) được chọn trên form
                int maTrinhDo = 0;
                if (!string.IsNullOrWhiteSpace(baiHoc.MaTrinhDo))
                {
                    object? tdObj = DatabaseHelper.ExecuteScalar(
                        "SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE UPPER(LTRIM(RTRIM(MaCapDo))) = UPPER(LTRIM(RTRIM(@CapDo)))", 
                        new[] { new SqlParameter("@CapDo", baiHoc.MaTrinhDo.Trim()) });
                    if (tdObj != null && tdObj != DBNull.Value)
                    {
                        maTrinhDo = Convert.ToInt32(tdObj);
                        baiHoc.MaTrinhDoId = maTrinhDo;
                    }
                }

                if (maTrinhDo <= 0)
                {
                    maTrinhDo = baiHoc.MaTrinhDoId > 0 ? baiHoc.MaTrinhDoId : 3;
                }

                // Đảm bảo MaChuDe hợp lệ nếu chưa có
                int maChuDe = baiHoc.MaChuDe;
                if (maChuDe <= 0)
                {
                    object? cdObj = DatabaseHelper.ExecuteScalar("SELECT TOP 1 MaChuDe FROM ChuDe ORDER BY MaChuDe ASC");
                    maChuDe = (cdObj != null && cdObj != DBNull.Value) ? Convert.ToInt32(cdObj) : 1;
                }

                string audio = baiHoc.AudioPath ?? baiHoc.DuongDanAudio ?? "";
                string image = baiHoc.AnhMinhHoaPath ?? baiHoc.DuongDanHinhAnh ?? "";
                int thoiLuong = baiHoc.ThoiGianUocTinhPhut > 0 ? baiHoc.ThoiGianUocTinhPhut : 15;
                string loai = string.IsNullOrWhiteSpace(baiHoc.LoaiBaiHoc) ? "NguPhap" : baiHoc.LoaiBaiHoc;

                if (baiHoc.MaBaiHoc <= 0)
                {
                    string insertSql = @"
                        INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, 
                                            DuongDanAudio, DuongDanHinhAnh, ThoiGianUocTinhPhut, DaXuatBan, 
                                            AudioPath, AnhMinhHoaPath, ThoiLuongUocTinh, NgayTao)
                        VALUES (@MaChuDe, @MaTrinhDo, @NguoiTao, @TieuDe, @LoaiBaiHoc, @TomTat, @NoiDung,
                                @Audio, @Image, @ThoiLuong, @DaXuatBan,
                                @Audio, @Image, @ThoiLuong, SYSDATETIME());
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    object? newId = DatabaseHelper.ExecuteScalar(insertSql, new[]
                    {
                        new SqlParameter("@MaChuDe", maChuDe),
                        new SqlParameter("@MaTrinhDo", maTrinhDo),
                        new SqlParameter("@NguoiTao", (object?)baiHoc.NguoiTao ?? DBNull.Value),
                        new SqlParameter("@TieuDe", baiHoc.TieuDe ?? ""),
                        new SqlParameter("@LoaiBaiHoc", loai),
                        new SqlParameter("@TomTat", (object?)baiHoc.TomTat ?? DBNull.Value),
                        new SqlParameter("@NoiDung", (object?)baiHoc.NoiDung ?? DBNull.Value),
                        new SqlParameter("@Audio", string.IsNullOrEmpty(audio) ? DBNull.Value : (object)audio),
                        new SqlParameter("@Image", string.IsNullOrEmpty(image) ? DBNull.Value : (object)image),
                        new SqlParameter("@ThoiLuong", thoiLuong),
                        new SqlParameter("@DaXuatBan", baiHoc.DaXuatBan)
                    });

                    if (newId != null && newId != DBNull.Value)
                    {
                        baiHoc.MaBaiHoc = Convert.ToInt32(newId);
                        return true;
                    }
                    return false;
                }
                else
                {
                    string updateSql = @"
                        UPDATE BaiHoc
                        SET MaChuDe = @MaChuDe,
                            MaTrinhDo = @MaTrinhDo,
                            TieuDe = @TieuDe,
                            LoaiBaiHoc = @LoaiBaiHoc,
                            TomTat = @TomTat,
                            NoiDung = @NoiDung,
                            DuongDanAudio = @Audio,
                            DuongDanHinhAnh = @Image,
                            ThoiGianUocTinhPhut = @ThoiLuong,
                            AudioPath = @Audio,
                            AnhMinhHoaPath = @Image,
                            ThoiLuongUocTinh = @ThoiLuong,
                            DaXuatBan = @DaXuatBan,
                            NgayCapNhat = SYSDATETIME()
                        WHERE MaBaiHoc = @MaBaiHoc;";

                    int rows = DatabaseHelper.ExecuteNonQuery(updateSql, new[]
                    {
                        new SqlParameter("@MaBaiHoc", baiHoc.MaBaiHoc),
                        new SqlParameter("@MaChuDe", maChuDe),
                        new SqlParameter("@MaTrinhDo", maTrinhDo),
                        new SqlParameter("@TieuDe", baiHoc.TieuDe ?? ""),
                        new SqlParameter("@LoaiBaiHoc", loai),
                        new SqlParameter("@TomTat", (object?)baiHoc.TomTat ?? DBNull.Value),
                        new SqlParameter("@NoiDung", (object?)baiHoc.NoiDung ?? DBNull.Value),
                        new SqlParameter("@Audio", string.IsNullOrEmpty(audio) ? DBNull.Value : (object)audio),
                        new SqlParameter("@Image", string.IsNullOrEmpty(image) ? DBNull.Value : (object)image),
                        new SqlParameter("@ThoiLuong", thoiLuong),
                        new SqlParameter("@DaXuatBan", baiHoc.DaXuatBan)
                    });

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// [US-GV05] Cập nhật cờ DaXuatBan
        /// </summary>
        public bool CapNhatTrangThaiXuatBan(int maBaiHoc, bool daXuatBan)
        {
            try
            {
                const string query = "UPDATE BaiHoc SET DaXuatBan = @DaXuatBan, NgayCapNhat = SYSDATETIME() WHERE MaBaiHoc = @MaBaiHoc;";
                int rows = DatabaseHelper.ExecuteNonQuery(query, new[]
                {
                    new SqlParameter("@DaXuatBan", daXuatBan),
                    new SqlParameter("@MaBaiHoc", maBaiHoc)
                });
                return rows > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocDAL.CapNhatTrangThaiXuatBan] Lỗi: {ex.Message}");
                return false;
            }
        }
    }
}
