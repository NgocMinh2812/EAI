using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class SqlDataProvider
    {
        private static SqlDataProvider? _instance;
        public static SqlDataProvider Instance => _instance ??= new SqlDataProvider();

        // Chuỗi kết nối mặc định - có thể thay đổi linh hoạt hoặc nạp từ biến môi trường
        private string _connectionString = "Server=.\\SQLEXPRESS;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;";

        public string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }

        public SqlDataProvider()
        {
            var envConn = Environment.GetEnvironmentVariable("EAI_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(envConn))
            {
                _connectionString = envConn;
            }
            TestConnection();
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                EnsureSchemaCompatibility();
                return true;
            }
            catch
            {
                // Thử fallback sang các instance SQL thông dụng
                string[] fallbackStrings = new[]
                {
                    "Server=.\\SQLEXPRESS;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;",
                    "Server=.;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;",
                    "Server=localhost\\SQLEXPRESS;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;",
                    "Server=localhost;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;",
                    "Server=(localdb)\\mssqllocaldb;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;",
                    "Server=127.0.0.1;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;"
                };

                foreach (var fb in fallbackStrings)
                {
                    try
                    {
                        using var fbConn = new SqlConnection(fb);
                        fbConn.Open();
                        _connectionString = fb;
                        return true;
                    }
                    catch
                    {
                        // thử tiếp
                    }
                }
                return false;
            }
        }

        public void EnsureSchemaCompatibility()
        {
            try
            {
                string script = @"
                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'NguoiDung')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'LaLanDauDangNhap' AND Object_ID = Object_ID('NguoiDung'))
                        BEGIN
                            ALTER TABLE NguoiDung ADD LaLanDauDangNhap BIT NOT NULL DEFAULT 1;
                        END
                    END;

                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'HoSoHocTap')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'DiemDanhGiaDauVao' AND Object_ID = Object_ID('HoSoHocTap'))
                        BEGIN
                            ALTER TABLE HoSoHocTap ADD DiemDanhGiaDauVao DECIMAL(5,2) NULL;
                        END

                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TrinhDoDauVao' AND Object_ID = Object_ID('HoSoHocTap'))
                        BEGIN
                            ALTER TABLE HoSoHocTap ADD TrinhDoDauVao NVARCHAR(10) NULL;
                        END
                    END;

                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'BaiKhaoSatDauVao')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM BaiKhaoSatDauVao WHERE MaBaiKhaoSat = 1)
                        BEGIN
                            SET IDENTITY_INSERT BaiKhaoSatDauVao ON;
                            INSERT INTO BaiKhaoSatDauVao (MaBaiKhaoSat, TenBaiKhaoSat, MoTa, TongSoCauHoi, DangHoatDong, NgayTao)
                            VALUES (1, N'Khảo sát đánh giá năng lực đầu vào CEFR', N'Bài khảo sát phân loại trình độ từ A1 đến C1', 15, 1, SYSDATETIME());
                            SET IDENTITY_INSERT BaiKhaoSatDauVao OFF;
                        END
                    END;

                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'LuotLamBai')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.views WHERE Name = 'KetQuaThi')
                        BEGIN
                            EXEC('CREATE VIEW KetQuaThi AS SELECT MaLuotLamBai AS MaKetQuaThi, MaNguoiDung, MaBaiTapDeThi, HinhThucLam, ThoiDiemBatDau, ThoiDiemNopBai, ThoiGianLamGiay, TongSoCau, SoCauDung, SoCauSai, DiemSo, TrangThai FROM LuotLamBai');
                        END
                    END;

                    -- SPRINT 2 SCHEMA COMPATIBILITY
                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'BaiHoc')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'AudioPath' AND Object_ID = OBJECT_ID('BaiHoc'))
                            ALTER TABLE BaiHoc ADD AudioPath NVARCHAR(500) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'AnhMinhHoaPath' AND Object_ID = OBJECT_ID('BaiHoc'))
                            ALTER TABLE BaiHoc ADD AnhMinhHoaPath NVARCHAR(500) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ThoiLuongUocTinh' AND Object_ID = OBJECT_ID('BaiHoc'))
                            ALTER TABLE BaiHoc ADD ThoiLuongUocTinh INT NOT NULL DEFAULT 15;
                    END;

                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'CauHoi')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'DoKho' AND Object_ID = OBJECT_ID('CauHoi'))
                            ALTER TABLE CauHoi ADD DoKho NVARCHAR(20) NOT NULL DEFAULT N'TrungBinh';
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'LoiGiaiChiTiet' AND Object_ID = OBJECT_ID('CauHoi'))
                            ALTER TABLE CauHoi ADD LoiGiaiChiTiet NVARCHAR(MAX) NULL;
                    END;

                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'BaiTapDeThi')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'DiemDat' AND Object_ID = OBJECT_ID('BaiTapDeThi'))
                            ALTER TABLE BaiTapDeThi ADD DiemDat FLOAT NOT NULL DEFAULT 5.0;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TongDiem' AND Object_ID = OBJECT_ID('BaiTapDeThi'))
                            ALTER TABLE BaiTapDeThi ADD TongDiem FLOAT NOT NULL DEFAULT 10.0;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'XaoTronCauHoi' AND Object_ID = OBJECT_ID('BaiTapDeThi'))
                            ALTER TABLE BaiTapDeThi ADD XaoTronCauHoi BIT NOT NULL DEFAULT 1;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TongSoCau' AND Object_ID = OBJECT_ID('BaiTapDeThi'))
                            ALTER TABLE BaiTapDeThi ADD TongSoCau INT NOT NULL DEFAULT 0;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ChoXemDapAn' AND Object_ID = OBJECT_ID('BaiTapDeThi'))
                            ALTER TABLE BaiTapDeThi ADD ChoXemDapAn BIT NOT NULL DEFAULT 1;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ChoXemLoiGiai' AND Object_ID = OBJECT_ID('BaiTapDeThi'))
                            ALTER TABLE BaiTapDeThi ADD ChoXemLoiGiai BIT NOT NULL DEFAULT 1;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'SoLuotLamToiDa' AND Object_ID = OBJECT_ID('BaiTapDeThi'))
                            ALTER TABLE BaiTapDeThi ADD SoLuotLamToiDa INT NOT NULL DEFAULT 0;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'DuongDanAudio' AND Object_ID = OBJECT_ID('BaiTapDeThi'))
                            ALTER TABLE BaiTapDeThi ADD DuongDanAudio NVARCHAR(500) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'SoLanPhatAudio' AND Object_ID = OBJECT_ID('BaiTapDeThi'))
                            ALTER TABLE BaiTapDeThi ADD SoLanPhatAudio INT NOT NULL DEFAULT 0;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TrangThai' AND Object_ID = OBJECT_ID('BaiTapDeThi'))
                            ALTER TABLE BaiTapDeThi ADD TrangThai NVARCHAR(30) NOT NULL DEFAULT N'BanNhap';

                        -- Đồng bộ trạng thái đề thi xuất bản giữa Giảng viên và Sinh viên
                        EXEC('UPDATE BaiTapDeThi SET TrangThai = N''DaXuatBan'' WHERE DaPhatHanh = 1 AND (TrangThai IS NULL OR TrangThai = N''BanNhap'')');
                    END;

                    -- BẢO TOÀN NỘI DUNG ĐỀ ĐÃ XUẤT BẢN: SNAPSHOT TRONG BẢNG CauHoiBaiTap
                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'CauHoiBaiTap')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'NoiDungCauHoiSnapshot' AND Object_ID = OBJECT_ID('CauHoiBaiTap'))
                            ALTER TABLE CauHoiBaiTap ADD NoiDungCauHoiSnapshot NVARCHAR(MAX) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'DoanVanDocSnapshot' AND Object_ID = OBJECT_ID('CauHoiBaiTap'))
                            ALTER TABLE CauHoiBaiTap ADD DoanVanDocSnapshot NVARCHAR(MAX) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'GiaiThichDapAnSnapshot' AND Object_ID = OBJECT_ID('CauHoiBaiTap'))
                            ALTER TABLE CauHoiBaiTap ADD GiaiThichDapAnSnapshot NVARCHAR(MAX) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'AudioPathSnapshot' AND Object_ID = OBJECT_ID('CauHoiBaiTap'))
                            ALTER TABLE CauHoiBaiTap ADD AudioPathSnapshot NVARCHAR(500) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'LoaiCauHoiSnapshot' AND Object_ID = OBJECT_ID('CauHoiBaiTap'))
                            ALTER TABLE CauHoiBaiTap ADD LoaiCauHoiSnapshot NVARCHAR(30) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'KyNangSnapshot' AND Object_ID = OBJECT_ID('CauHoiBaiTap'))
                            ALTER TABLE CauHoiBaiTap ADD KyNangSnapshot NVARCHAR(30) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'DoKhoSnapshot' AND Object_ID = OBJECT_ID('CauHoiBaiTap'))
                            ALTER TABLE CauHoiBaiTap ADD DoKhoSnapshot NVARCHAR(20) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'DanhSachLuaChonJson' AND Object_ID = OBJECT_ID('CauHoiBaiTap'))
                            ALTER TABLE CauHoiBaiTap ADD DanhSachLuaChonJson NVARCHAR(MAX) NULL;

                        -- Backfill snapshot cho các câu hỏi hiện có trong CauHoiBaiTap nếu chưa có snapshot
                        EXEC('
                            UPDATE chbt
                            SET chbt.NoiDungCauHoiSnapshot = ch.NoiDungCauHoi,
                                chbt.DoanVanDocSnapshot = ch.DoanVanDoc,
                                chbt.GiaiThichDapAnSnapshot = ch.GiaiThichDapAn,
                                chbt.AudioPathSnapshot = ch.DuongDanAudio,
                                chbt.LoaiCauHoiSnapshot = ch.LoaiCauHoi,
                                chbt.KyNangSnapshot = ch.KyNang,
                                chbt.DoKhoSnapshot = ch.DoKho
                            FROM CauHoiBaiTap chbt
                            INNER JOIN CauHoi ch ON chbt.MaCauHoi = ch.MaCauHoi
                            WHERE chbt.NoiDungCauHoiSnapshot IS NULL;
                        ');
                    END;

                    -- QUẢN LÝ BỘ CÂU HỎI (QUESTION SETS)
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'BoCauHoi')
                    BEGIN
                        CREATE TABLE BoCauHoi (
                            MaBoCauHoi INT IDENTITY(1,1) PRIMARY KEY,
                            TenBoCauHoi NVARCHAR(250) NOT NULL,
                            MoTa NVARCHAR(MAX) NULL,
                            NguonTao NVARCHAR(50) NOT NULL DEFAULT N'AI',
                            TenNguon NVARCHAR(250) NULL,
                            MaBaiHoc INT NULL,
                            MaChuDe INT NULL,
                            MaTrinhDo INT NULL,
                            NguoiTao INT NULL,
                            ContentHash NVARCHAR(64) NULL,
                            LoaiBoCauHoi NVARCHAR(30) NOT NULL DEFAULT N'CungCo',
                            TrangThai NVARCHAR(30) NOT NULL DEFAULT N'HoanThanh',
                            ThoiDiemBatDauXuLy DATETIME2 NULL,
                            ThongBaoLoi NVARCHAR(MAX) NULL,
                            NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
                            NgayCapNhat DATETIME2 NULL
                        );
                    END
                    ELSE
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TenNguon' AND Object_ID = OBJECT_ID('BoCauHoi'))
                            ALTER TABLE BoCauHoi ADD TenNguon NVARCHAR(250) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'NguonTao' AND Object_ID = OBJECT_ID('BoCauHoi'))
                            ALTER TABLE BoCauHoi ADD NguonTao NVARCHAR(50) NOT NULL DEFAULT N'AI';
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ContentHash' AND Object_ID = OBJECT_ID('BoCauHoi'))
                            ALTER TABLE BoCauHoi ADD ContentHash NVARCHAR(64) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'LoaiBoCauHoi' AND Object_ID = OBJECT_ID('BoCauHoi'))
                            ALTER TABLE BoCauHoi ADD LoaiBoCauHoi NVARCHAR(30) NOT NULL DEFAULT N'CungCo';
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TrangThai' AND Object_ID = OBJECT_ID('BoCauHoi'))
                            ALTER TABLE BoCauHoi ADD TrangThai NVARCHAR(30) NOT NULL DEFAULT N'HoanThanh';
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ThoiDiemBatDauXuLy' AND Object_ID = OBJECT_ID('BoCauHoi'))
                            ALTER TABLE BoCauHoi ADD ThoiDiemBatDauXuLy DATETIME2 NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ThongBaoLoi' AND Object_ID = OBJECT_ID('BoCauHoi'))
                            ALTER TABLE BoCauHoi ADD ThongBaoLoi NVARCHAR(MAX) NULL;
                    END;

                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'BoCauHoi_CauHoi')
                    BEGIN
                        CREATE TABLE BoCauHoi_CauHoi (
                            MaBoCauHoi INT NOT NULL,
                            MaCauHoi INT NOT NULL,
                            ThuTu INT NOT NULL DEFAULT 1,
                            PRIMARY KEY (MaBoCauHoi, MaCauHoi),
                            CONSTRAINT FK_BoCauHoi_CauHoi_Bo FOREIGN KEY (MaBoCauHoi) REFERENCES BoCauHoi(MaBoCauHoi) ON DELETE CASCADE,
                            CONSTRAINT FK_BoCauHoi_CauHoi_CH FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi) ON DELETE CASCADE
                        );
                    END;

                    -- BẢNG LƯU KẾT QUẢ & LỊCH SỬ LÀM BÀI TẬP RIÊNG BIỆT CHO TỪNG SINH VIÊN (BỘ A & BỘ B)
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'KetQuaLamBaiBoCauHoi')
                    BEGIN
                        CREATE TABLE KetQuaLamBaiBoCauHoi (
                            MaKetQua INT IDENTITY(1,1) PRIMARY KEY,
                            MaNguoiDung INT NOT NULL,
                            MaBoCauHoi INT NOT NULL,
                            MaBaiHoc INT NOT NULL,
                            LoaiBoCauHoi NVARCHAR(30) NOT NULL DEFAULT N'CungCo',
                            ContentHash NVARCHAR(64) NOT NULL,
                            TongSoCau INT NOT NULL DEFAULT 10,
                            SoCauDung INT NOT NULL DEFAULT 0,
                            DiemSo DECIMAL(5,2) NOT NULL DEFAULT 0,
                            ThoiGianLamGiay INT NOT NULL DEFAULT 0,
                            TrangThai NVARCHAR(30) NOT NULL DEFAULT N'DaNop',
                            ChiTietTraLoiJson NVARCHAR(MAX) NULL,
                            NgayNop DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
                            CONSTRAINT FK_KetQuaLamBaiBoCauHoi_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung) ON DELETE CASCADE,
                            CONSTRAINT FK_KetQuaLamBaiBoCauHoi_BoCauHoi FOREIGN KEY (MaBoCauHoi) REFERENCES BoCauHoi(MaBoCauHoi) ON DELETE CASCADE,
                            CONSTRAINT FK_KetQuaLamBaiBoCauHoi_BaiHoc FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc) ON DELETE NO ACTION
                        );

                        CREATE NONCLUSTERED INDEX IX_KetQuaLamBaiBoCauHoi_User_Lesson
                        ON KetQuaLamBaiBoCauHoi(MaNguoiDung, MaBaiHoc, LoaiBoCauHoi, ContentHash);
                    END;

                    -- LÀM SẠCH DỮ LIỆU HTML CŨ TRONG BẢNG BAIHOC
                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'BaiHoc')
                    BEGIN
                        UPDATE BaiHoc
                        SET NoiDung = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
                            NoiDung,
                            '<h3>', N'■ '),
                            '</h3>', CHAR(10)),
                            '<ol>', ''),
                            '</ol>', ''),
                            '<ul>', ''),
                            '</ul>', ''),
                            '<li>', N'• '),
                            '</li>', CHAR(10)),
                            '<p>', ''),
                            '</p>', CHAR(10) + CHAR(10))
                        WHERE NoiDung LIKE '%<%>%';

                        UPDATE BaiHoc
                        SET NoiDung = REPLACE(REPLACE(REPLACE(REPLACE(NoiDung, '<strong>', ''), '</strong>', ''), '<em>', ''), '</em>', '')
                        WHERE NoiDung LIKE '%<%>%';
                    END;

                    -- BẢNG PHÂN TÍCH NỘI DUNG HỌC TẬP THÔNG MINH AI
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'NoiDungHocTapAIBaiHoc')
                    BEGIN
                        CREATE TABLE NoiDungHocTapAIBaiHoc (
                            MaNoiDungAI INT IDENTITY(1,1) PRIMARY KEY,
                            MaBaiHoc INT NOT NULL,
                            ContentHash NVARCHAR(64) NOT NULL,
                            TrangThai NVARCHAR(30) NOT NULL DEFAULT N'ChuaTao',
                            KyNang NVARCHAR(50) NULL,
                            MucTieu NVARCHAR(1000) NULL,
                            DuLieuKetQuaJson NVARCHAR(MAX) NULL,
                            MoHinhAI NVARCHAR(100) NULL,
                            ThongBaoLoi NVARCHAR(MAX) NULL,
                            ThoiDiemBatDauXuLy DATETIME2 NULL,
                            NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
                            NgayCapNhat DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
                            CONSTRAINT UQ_NoiDungHocTapAIBaiHoc_MaBaiHoc UNIQUE (MaBaiHoc),
                            CONSTRAINT FK_NoiDungHocTapAIBaiHoc_BaiHoc FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc) ON DELETE CASCADE
                        );

                        CREATE NONCLUSTERED INDEX IX_NoiDungHocTapAIBaiHoc_Hash 
                        ON NoiDungHocTapAIBaiHoc(MaBaiHoc, ContentHash, TrangThai);
                    END;

                    -- SPRINT 3: CẤU HÌNH QUY ĐỔI KHẢO SÁT & FILE IMPORT
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'CauHinhQuyDoiKhaoSat')
                    BEGIN
                        CREATE TABLE CauHinhQuyDoiKhaoSat (
                            MaCauHinh INT IDENTITY(1,1) PRIMARY KEY,
                            MaCapDo NVARCHAR(10) NOT NULL,
                            TenHienThi NVARCHAR(100) NOT NULL,
                            DiemPhanTramMin INT NOT NULL,
                            DiemPhanTramMax INT NOT NULL,
                            QuyDoiIelts NVARCHAR(50) NOT NULL,
                            QuyDoiToeic NVARCHAR(50) NOT NULL,
                            LoTrinhDeXuat NVARCHAR(500) NOT NULL,
                            MauSac NVARCHAR(20) NULL,
                            ThuTuSapXep INT NOT NULL DEFAULT 1
                        );
                    END;

                    IF NOT EXISTS (SELECT 1 FROM CauHinhQuyDoiKhaoSat)
                    BEGIN
                        INSERT INTO CauHinhQuyDoiKhaoSat 
                            (MaCapDo, TenHienThi, DiemPhanTramMin, DiemPhanTramMax, QuyDoiIelts, QuyDoiToeic, LoTrinhDeXuat, MauSac, ThuTuSapXep)
                        VALUES
                            (N'A1', N'A1 (Mất gốc)', 0, 30, N'IELTS < 3.0', N'TOEIC 100 - 250', N'Ngữ pháp nền tảng + 500 từ vựng cốt lõi', N'#DC2626', 1),
                            (N'A2', N'A2 (Sơ cấp)', 31, 50, N'IELTS 3.5 - 4.5', N'TOEIC 255 - 450', N'Luyện phản xạ câu & Nghe hiểu đơn giản', N'#EA580C', 2),
                            (N'B1', N'B1 (Trung cấp)', 51, 70, N'IELTS 5.0 - 6.0', N'TOEIC 455 - 650', N'Từ vựng học thuật B1 & Cấu trúc câu ghép', N'#2563EB', 3),
                            (N'B2', N'B2 (Trên trung cấp)', 71, 85, N'IELTS 6.5 - 7.0', N'TOEIC 655 - 800', N'Viết luận Task 2 & Skimming/Scanning đọc hiểu', N'#15803D', 4),
                            (N'C1', N'C1 (Nâng cao)', 86, 100, N'IELTS 7.5 - 8.5+', N'TOEIC 805 - 990', N'Thành ngữ Collocations cao cấp & Debate', N'#7E22CE', 5);
                    END;

                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'TepTinNhapLieu')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'SoCauHoiNhanDien' AND Object_ID = OBJECT_ID('TepTinNhapLieu'))
                            ALTER TABLE TepTinNhapLieu ADD SoCauHoiNhanDien INT NULL DEFAULT 0;
                    END;

                    -- HỒ SƠ HỌC TẬP - STREAK & LAST STUDY DATE
                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'HoSoHocTap')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ChuoiNgayHoc' AND Object_ID = OBJECT_ID('HoSoHocTap'))
                            ALTER TABLE HoSoHocTap ADD ChuoiNgayHoc INT NOT NULL DEFAULT 0;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'NgayHocGanNhat' AND Object_ID = OBJECT_ID('HoSoHocTap'))
                            ALTER TABLE HoSoHocTap ADD NgayHocGanNhat DATE NULL;
                    END;

                    -- AI ASSISTANT CONVERSATION & MESSAGE PERSISTENCE SCHEMA
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'AIAssistantConversation')
                    BEGIN
                        CREATE TABLE AIAssistantConversation (
                            MaHoiThoai INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AIAssistantConversation PRIMARY KEY,
                            MaNguoiDung INT NOT NULL,
                            TieuDe NVARCHAR(255) NOT NULL DEFAULT N'Cuộc hội thoại mới',
                            NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
                            NgayCapNhat DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
                            TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Active',
                            TongSoTinNhan INT NOT NULL DEFAULT 0,
                            MoHinhUuTien NVARCHAR(100) NULL,
                            PromptHeThongTuyChon NVARCHAR(MAX) NULL,
                            CONSTRAINT FK_AIAssistantConversation_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung) ON DELETE CASCADE
                        );

                        CREATE NONCLUSTERED INDEX IX_AIAssistantConversation_MaNguoiDung_NgayCapNhat 
                        ON AIAssistantConversation(MaNguoiDung, NgayCapNhat DESC);

                        CREATE NONCLUSTERED INDEX IX_AIAssistantConversation_TieuDe 
                        ON AIAssistantConversation(TieuDe);
                    END
                    ELSE
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TrangThai' AND Object_ID = OBJECT_ID('AIAssistantConversation'))
                            ALTER TABLE AIAssistantConversation ADD TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Active';
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TongSoTinNhan' AND Object_ID = OBJECT_ID('AIAssistantConversation'))
                            ALTER TABLE AIAssistantConversation ADD TongSoTinNhan INT NOT NULL DEFAULT 0;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'MoHinhUuTien' AND Object_ID = OBJECT_ID('AIAssistantConversation'))
                            ALTER TABLE AIAssistantConversation ADD MoHinhUuTien NVARCHAR(100) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'PromptHeThongTuyChon' AND Object_ID = OBJECT_ID('AIAssistantConversation'))
                            ALTER TABLE AIAssistantConversation ADD PromptHeThongTuyChon NVARCHAR(MAX) NULL;
                    END;

                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'AIAssistantMessage')
                    BEGIN
                        CREATE TABLE AIAssistantMessage (
                            MaTinNhan BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AIAssistantMessage PRIMARY KEY,
                            MaHoiThoai INT NOT NULL,
                            VaiTro NVARCHAR(20) NOT NULL DEFAULT N'user',
                            NoiDung NVARCHAR(MAX) NOT NULL DEFAULT N'',
                            ThoiGianGui DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
                            ToolCallsJson NVARCHAR(MAX) NULL,
                            ToolResultJson NVARCHAR(MAX) NULL,
                            CardPayloadJson NVARCHAR(MAX) NULL,
                            TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Sent',
                            CONSTRAINT FK_AIAssistantMessage_Conversation FOREIGN KEY (MaHoiThoai) REFERENCES AIAssistantConversation(MaHoiThoai) ON DELETE CASCADE
                        );

                        CREATE NONCLUSTERED INDEX IX_AIAssistantMessage_MaHoiThoai_ThoiGianGui 
                        ON AIAssistantMessage(MaHoiThoai, ThoiGianGui ASC);
                    END
                    ELSE
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ToolCallsJson' AND Object_ID = OBJECT_ID('AIAssistantMessage'))
                            ALTER TABLE AIAssistantMessage ADD ToolCallsJson NVARCHAR(MAX) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'ToolResultJson' AND Object_ID = OBJECT_ID('AIAssistantMessage'))
                            ALTER TABLE AIAssistantMessage ADD ToolResultJson NVARCHAR(MAX) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'CardPayloadJson' AND Object_ID = OBJECT_ID('AIAssistantMessage'))
                            ALTER TABLE AIAssistantMessage ADD CardPayloadJson NVARCHAR(MAX) NULL;
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TrangThai' AND Object_ID = OBJECT_ID('AIAssistantMessage'))
                            ALTER TABLE AIAssistantMessage ADD TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Sent';
                    END;";
                ExecuteNonQuery(script);
            }
            catch
            {
                // Bỏ qua nếu chưa có bảng
            }
        }

        public DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            DataTable data = new DataTable();
            using var conn = GetConnection();
            using var cmd = new SqlCommand(query, conn);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(data);
            return data;
        }

        public int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(query, conn);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd.ExecuteNonQuery();
        }

        public object? ExecuteScalar(string query, SqlParameter[]? parameters = null)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(query, conn);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd.ExecuteScalar();
        }
    }
}
