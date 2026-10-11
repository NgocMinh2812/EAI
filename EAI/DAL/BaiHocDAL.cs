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
                        PhanTramTienDo = phanTram,
                        TrangThai = trangThaiHienThi
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
            var query = @"
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
                    bh.DaXuatBan,
                    bh.NgayTao
                FROM BaiHoc bh
                INNER JOIN ChuDe cd ON bh.MaChuDe = cd.MaChuDe
                INNER JOIN TrinhDo td ON bh.MaTrinhDo = td.MaTrinhDo
                WHERE 1 = 1";

            var parameters = new List<SqlParameter>();

            if (maChuDe.HasValue && maChuDe.Value > 0)
            {
                // Bao gồm cả chủ đề con
                query += @" AND (bh.MaChuDe = @maChuDe OR bh.MaChuDe IN (SELECT MaChuDe FROM ChuDe WHERE MaChuDeCha = @maChuDe))";
                parameters.Add(new SqlParameter("@maChuDe", maChuDe.Value));
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

                bool daXuatBan = Convert.ToBoolean(row["DaXuatBan"]);

                list.Add(new BaiHocDTO
                {
                    MaBaiHoc = Convert.ToInt32(row["MaBaiHoc"]),
                    TieuDe = row["TieuDe"]?.ToString() ?? "",
                    TenChuDe = row["TenChuDe"]?.ToString() ?? "",
                    LoaiBaiHoc = loai,
                    TenLoaiHienThi = tenLoai,
                    MaTrinhDo = row["TenTrinhDo"]?.ToString() ?? "B1",
                    TomTat = row["TomTat"] == DBNull.Value ? "" : (row["TomTat"]?.ToString() ?? ""),
                    NoiDung = row["NoiDung"] == DBNull.Value ? "" : (row["NoiDung"]?.ToString() ?? ""),
                    ThoiGianUocTinhPhut = Convert.ToInt32(row["ThoiGianUocTinhPhut"]),
                    TrangThai = daXuatBan ? "Đã xuất bản" : "Bản nháp",
                    IconSkill = icon,
                    StatusBackground = daXuatBan ? "#DCFCE7" : "#F1F5F9",
                    StatusColor = daXuatBan ? "#15803D" : "#64748B"
                });
            }

            return list;
        }
    }
}
