/*
    DATABASE: TiengAnhAI
    Mục đích: CSDL cho đề tài "Ứng dụng khảo thí & học tiếng Anh tích hợp AI"
    Hệ quản trị: Microsoft SQL Server.
*/

USE master;
GO

IF DB_ID(N'TiengAnhAI') IS NOT NULL
BEGIN
    ALTER DATABASE TiengAnhAI SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TiengAnhAI;
END
GO

CREATE DATABASE TiengAnhAI;
GO

USE TiengAnhAI;
GO

/* =========================================================
   1. PHÂN QUYỀN - TÀI KHOẢN - HỒ SƠ HỌC TẬP
   ========================================================= */

CREATE TABLE VaiTro (
    MaVaiTro INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL
);
GO

CREATE TABLE NguoiDung (
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    MaVaiTro INT NOT NULL,
    TenDangNhap NVARCHAR(50) NOT NULL UNIQUE,
    MatKhauMaHoa NVARCHAR(255) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE NULL,
    SoDienThoai NVARCHAR(20) NULL,
    DuongDanAnhDaiDien NVARCHAR(500) NULL,
    TrangThaiHoatDong BIT NOT NULL DEFAULT 1,
    LaLanDauDangNhap BIT NOT NULL DEFAULT 1, -- 1: Chưa làm/chưa qua test đầu vào, 0: Đã xong
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    LanDangNhapCuoi DATETIME2 NULL,
    CONSTRAINT FK_NguoiDung_VaiTro
        FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);
GO

CREATE TABLE HoSoHocTap (
    MaHoSo INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL UNIQUE,
    DiemMucTieu DECIMAL(3,1) NULL,
    KhungNangLucMucTieu NVARCHAR(30) NULL, -- IELTS / TOEIC / CEFR...
    TrinhDoMucTieu NVARCHAR(10) NULL,     -- A1-C1...
    SoPhutHocTichLuy INT NOT NULL DEFAULT 0,
    GhiChuCaNhan NVARCHAR(MAX) NULL,
    NgayCapNhat DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_HoSoHocTap_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);
GO

/* =========================================================
   2. TRÌNH ĐỘ - CHỦ ĐỀ - BÀI HỌC
   ========================================================= */

CREATE TABLE TrinhDo (
    MaTrinhDo INT IDENTITY(1,1) PRIMARY KEY,
    MaCapDo NVARCHAR(10) NOT NULL UNIQUE, -- A1, A2, B1, B2, C1...
    TenTrinhDo NVARCHAR(50) NOT NULL,
    MoTa NVARCHAR(500) NULL
);
GO

CREATE TABLE ChuDe (
    MaChuDe INT IDENTITY(1,1) PRIMARY KEY,
    MaChuDeCha INT NULL,
    TenChuDe NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(500) NULL,
    TrangThaiHoatDong BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_ChuDe_ChuDeCha
        FOREIGN KEY (MaChuDeCha) REFERENCES ChuDe(MaChuDe)
);
GO

CREATE TABLE BaiHoc (
    MaBaiHoc INT IDENTITY(1,1) PRIMARY KEY,
    MaChuDe INT NOT NULL,
    MaTrinhDo INT NOT NULL,
    NguoiTao INT NULL,
    TieuDe NVARCHAR(200) NOT NULL,
    LoaiBaiHoc NVARCHAR(30) NOT NULL, -- TuVung / NguPhap / Doc / Nghe / Viet
    TomTat NVARCHAR(1000) NULL,
    NoiDung NVARCHAR(MAX) NULL,
    DuongDanAudio NVARCHAR(500) NULL,
    DuongDanHinhAnh NVARCHAR(500) NULL,
    ThoiGianUocTinhPhut INT NULL,
    DaXuatBan BIT NOT NULL DEFAULT 0,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    NgayCapNhat DATETIME2 NULL,
    CONSTRAINT FK_BaiHoc_ChuDe
        FOREIGN KEY (MaChuDe) REFERENCES ChuDe(MaChuDe),
    CONSTRAINT FK_BaiHoc_TrinhDo
        FOREIGN KEY (MaTrinhDo) REFERENCES TrinhDo(MaTrinhDo),
    CONSTRAINT FK_BaiHoc_NguoiTao
        FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaNguoiDung)
);
GO

CREATE TABLE TienDoBaiHoc (
    MaTienDoBaiHoc INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaBaiHoc INT NOT NULL,
    TrangThai NVARCHAR(20) NOT NULL DEFAULT N'ChuaHoc', -- ChuaHoc / DangHoc / HoanThanh
    PhanTramTienDo DECIMAL(5,2) NOT NULL DEFAULT 0,
    NgayBatDau DATETIME2 NULL,
    NgayHoanThanh DATETIME2 NULL,
    LanTruyCapCuoi DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT UQ_TienDoBaiHoc UNIQUE(MaNguoiDung, MaBaiHoc),
    CONSTRAINT FK_TienDoBaiHoc_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_TienDoBaiHoc_BaiHoc
        FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc)
);
GO

/* =========================================================
   3. TỪ VỰNG - FLASHCARD - NGỮ PHÁP
   ========================================================= */

CREATE TABLE TuVung (
    MaTuVung INT IDENTITY(1,1) PRIMARY KEY,
    MaChuDe INT NULL,
    MaTrinhDo INT NULL,
    Tu NVARCHAR(100) NOT NULL,
    LoaiTu NVARCHAR(50) NULL,
    PhienAmIPA NVARCHAR(150) NULL,
    Nghia NVARCHAR(500) NOT NULL,
    CauViDu NVARCHAR(1000) NULL,
    DuongDanAudio NVARCHAR(500) NULL,
    CumTuDiKem NVARCHAR(500) NULL,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_TuVung_ChuDe
        FOREIGN KEY (MaChuDe) REFERENCES ChuDe(MaChuDe),
    CONSTRAINT FK_TuVung_TrinhDo
        FOREIGN KEY (MaTrinhDo) REFERENCES TrinhDo(MaTrinhDo)
);
GO

CREATE TABLE Flashcard (
    MaFlashcard INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaTuVung INT NOT NULL,
    SoLanOnTap INT NOT NULL DEFAULT 0,
    SoLanDung INT NOT NULL DEFAULT 0,
    NgayOnTapTiepTheo DATETIME2 NULL,
    LanOnTapCuoi DATETIME2 NULL,
    CONSTRAINT UQ_Flashcard UNIQUE(MaNguoiDung, MaTuVung),
    CONSTRAINT FK_Flashcard_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_Flashcard_TuVung
        FOREIGN KEY (MaTuVung) REFERENCES TuVung(MaTuVung)
);
GO

CREATE TABLE ChuDeNguPhap (
    MaChuDeNguPhap INT IDENTITY(1,1) PRIMARY KEY,
    MaTrinhDo INT NULL,
    TenChuDeNguPhap NVARCHAR(150) NOT NULL,
    GiaiThichLyThuyet NVARCHAR(MAX) NULL,
    ViDuMinhHoa NVARCHAR(MAX) NULL,
    CONSTRAINT FK_ChuDeNguPhap_TrinhDo
        FOREIGN KEY (MaTrinhDo) REFERENCES TrinhDo(MaTrinhDo)
);
GO

/* =========================================================
   4. CÂU HỎI - LỰA CHỌN - BÀI TẬP / ĐỀ THI
   ========================================================= */

CREATE TABLE CauHoi (
    MaCauHoi INT IDENTITY(1,1) PRIMARY KEY,
    MaChuDe INT NULL,
    MaTrinhDo INT NULL,
    MaBaiHoc INT NULL,
    MaChuDeNguPhap INT NULL,
    LoaiCauHoi NVARCHAR(30) NOT NULL, -- TracNghiem / DienKhuyet / SapXep / TuLuan
    KyNang NVARCHAR(30) NULL,        -- Nghe / Doc / Viet / NguPhap / TuVung
    NoiDungCauHoi NVARCHAR(MAX) NOT NULL,
    GiaiThichDapAn NVARCHAR(MAX) NULL,
    DoanVanDanChung NVARCHAR(MAX) NULL,
    DuongDanAudio NVARCHAR(500) NULL,
    DoanVanDoc NVARCHAR(MAX) NULL,
    NguoiTao INT NULL,
    NguonTao NVARCHAR(30) NOT NULL DEFAULT N'ThuCong', -- ThuCong / NhapFile / AI
    DaKiemDuyet BIT NOT NULL DEFAULT 0,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_CauHoi_ChuDe
        FOREIGN KEY (MaChuDe) REFERENCES ChuDe(MaChuDe),
    CONSTRAINT FK_CauHoi_TrinhDo
        FOREIGN KEY (MaTrinhDo) REFERENCES TrinhDo(MaTrinhDo),
    CONSTRAINT FK_CauHoi_BaiHoc
        FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc),
    CONSTRAINT FK_CauHoi_NguPhap
        FOREIGN KEY (MaChuDeNguPhap) REFERENCES ChuDeNguPhap(MaChuDeNguPhap),
    CONSTRAINT FK_CauHoi_NguoiTao
        FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaNguoiDung)
);
GO

CREATE TABLE LuaChonDapAn (
    MaLuaChon INT IDENTITY(1,1) PRIMARY KEY,
    MaCauHoi INT NOT NULL,
    KyHieuLuaChon NVARCHAR(5) NOT NULL, -- A / B / C / D
    NoiDungLuaChon NVARCHAR(MAX) NOT NULL,
    LaDapAnDung BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_LuaChonDapAn_CauHoi
        FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi)
);
GO

CREATE TABLE BaiTapDeThi (
    MaBaiTapDeThi INT IDENTITY(1,1) PRIMARY KEY,
    MaBaiHoc INT NULL,
    MaChuDe INT NULL,
    MaTrinhDo INT NULL,
    TenBaiTapDeThi NVARCHAR(200) NOT NULL,
    LoaiHinh NVARCHAR(30) NOT NULL, -- LuyenTap / ThiThu
    MoTa NVARCHAR(1000) NULL,
    ThoiGianLamBaiPhut INT NULL,
    DaPhatHanh BIT NOT NULL DEFAULT 0,
    NguoiTao INT NULL,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_BaiTapDeThi_BaiHoc
        FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc),
    CONSTRAINT FK_BaiTapDeThi_ChuDe
        FOREIGN KEY (MaChuDe) REFERENCES ChuDe(MaChuDe),
    CONSTRAINT FK_BaiTapDeThi_TrinhDo
        FOREIGN KEY (MaTrinhDo) REFERENCES TrinhDo(MaTrinhDo),
    CONSTRAINT FK_BaiTapDeThi_NguoiTao
        FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaNguoiDung)
);
GO

CREATE TABLE CauHoiBaiTap (
    MaCauHoiBaiTap INT IDENTITY(1,1) PRIMARY KEY,
    MaBaiTapDeThi INT NOT NULL,
    MaCauHoi INT NOT NULL,
    ThuTuCauHoi INT NOT NULL,
    DiemSo DECIMAL(5,2) NOT NULL DEFAULT 1,
    CONSTRAINT UQ_CauHoiBaiTap UNIQUE(MaBaiTapDeThi, MaCauHoi),
    CONSTRAINT FK_CauHoiBaiTap_BaiTapDeThi
        FOREIGN KEY (MaBaiTapDeThi) REFERENCES BaiTapDeThi(MaBaiTapDeThi),
    CONSTRAINT FK_CauHoiBaiTap_CauHoi
        FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi)
);
GO

/* =========================================================
   5. KHẢO SÁT TRÌNH ĐỘ ĐẦU VÀO
   ========================================================= */

CREATE TABLE BaiKhaoSatDauVao (
    MaBaiKhaoSat INT IDENTITY(1,1) PRIMARY KEY,
    TenBaiKhaoSat NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(1000) NULL,
    TongSoCauHoi INT NOT NULL DEFAULT 0,
    DangHoatDong BIT NOT NULL DEFAULT 1,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
GO

CREATE TABLE CauHoiKhaoSat (
    MaCauHoiKhaoSat INT IDENTITY(1,1) PRIMARY KEY,
    MaBaiKhaoSat INT NOT NULL,
    MaCauHoi INT NOT NULL,
    ThuTuCauHoi INT NOT NULL,
    CONSTRAINT UQ_CauHoiKhaoSat UNIQUE(MaBaiKhaoSat, MaCauHoi),
    CONSTRAINT FK_CauHoiKhaoSat_BaiKhaoSat
        FOREIGN KEY (MaBaiKhaoSat) REFERENCES BaiKhaoSatDauVao(MaBaiKhaoSat),
    CONSTRAINT FK_CauHoiKhaoSat_CauHoi
        FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi)
);
GO

CREATE TABLE LuotKhaoSat (
    MaLuotKhaoSat INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaBaiKhaoSat INT NOT NULL,
    ThoiDiemBatDau DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    ThoiDiemNopBai DATETIME2 NULL,
    TongSoCau INT NOT NULL DEFAULT 0,
    SoCauDung INT NOT NULL DEFAULT 0,
    DiemDatDuoc DECIMAL(6,2) NULL,
    MaTrinhDoKetQua INT NULL,
    KhungDanhGiaKetQua NVARCHAR(30) NULL,
    CONSTRAINT FK_LuotKhaoSat_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_LuotKhaoSat_BaiKhaoSat
        FOREIGN KEY (MaBaiKhaoSat) REFERENCES BaiKhaoSatDauVao(MaBaiKhaoSat),
    CONSTRAINT FK_LuotKhaoSat_TrinhDo
        FOREIGN KEY (MaTrinhDoKetQua) REFERENCES TrinhDo(MaTrinhDo)
);
GO

CREATE TABLE CauTraLoiKhaoSat (
    MaCauTraLoiKhaoSat INT IDENTITY(1,1) PRIMARY KEY,
    MaLuotKhaoSat INT NOT NULL,
    MaCauHoi INT NOT NULL,
    MaLuaChonDaChon INT NULL,
    KetQuaDungSai BIT NULL,
    ThoiDiemTraLoi DATETIME2 NULL,
    CONSTRAINT UQ_CauTraLoiKhaoSat UNIQUE(MaLuotKhaoSat, MaCauHoi),
    CONSTRAINT FK_CauTraLoiKhaoSat_LuotKhaoSat
        FOREIGN KEY (MaLuotKhaoSat) REFERENCES LuotKhaoSat(MaLuotKhaoSat),
    CONSTRAINT FK_CauTraLoiKhaoSat_CauHoi
        FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi),
    CONSTRAINT FK_CauTraLoiKhaoSat_LuaChon
        FOREIGN KEY (MaLuaChonDaChon) REFERENCES LuaChonDapAn(MaLuaChon)
);
GO

/* =========================================================
   6. LƯỢT LÀM BÀI - CHI TIẾT TRẢ LỜI - CHẤM ĐIỂM
   ========================================================= */

CREATE TABLE LuotLamBai (
    MaLuotLamBai INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaBaiTapDeThi INT NOT NULL,
    HinhThucLam NVARCHAR(20) NOT NULL DEFAULT N'LuyenTap', -- LuyenTap / ThiThu
    ThoiDiemBatDau DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    ThoiDiemNopBai DATETIME2 NULL,
    ThoiGianLamGiay INT NULL,
    TongSoCau INT NOT NULL DEFAULT 0,
    SoCauDung INT NOT NULL DEFAULT 0,
    SoCauSai INT NOT NULL DEFAULT 0,
    DiemSo DECIMAL(6,2) NULL,
    TrangThai NVARCHAR(20) NOT NULL DEFAULT N'DangLam', -- DangLam / DaNop / DaHuy
    CONSTRAINT FK_LuotLamBai_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_LuotLamBai_BaiTapDeThi
        FOREIGN KEY (MaBaiTapDeThi) REFERENCES BaiTapDeThi(MaBaiTapDeThi)
);
GO

CREATE TABLE ChiTietTraLoi (
    MaChiTietTraLoi INT IDENTITY(1,1) PRIMARY KEY,
    MaLuotLamBai INT NOT NULL,
    MaCauHoi INT NOT NULL,
    MaLuaChonDaChon INT NULL,
    CauTraLoiVanBan NVARCHAR(MAX) NULL,
    KetQuaDungSai BIT NULL,
    DiemSo DECIMAL(6,2) NULL,
    AIGiaiThichChiTiet NVARCHAR(MAX) NULL,
    ThoiDiemTraLoi DATETIME2 NULL,
    CONSTRAINT UQ_ChiTietTraLoi UNIQUE(MaLuotLamBai, MaCauHoi),
    CONSTRAINT FK_ChiTietTraLoi_LuotLamBai
        FOREIGN KEY (MaLuotLamBai) REFERENCES LuotLamBai(MaLuotLamBai),
    CONSTRAINT FK_ChiTietTraLoi_CauHoi
        FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi),
    CONSTRAINT FK_ChiTietTraLoi_LuaChon
        FOREIGN KEY (MaLuaChonDaChon) REFERENCES LuaChonDapAn(MaLuaChon)
);
GO

CREATE TABLE DanhGiaBaiViet (
    MaDanhGiaBaiViet INT IDENTITY(1,1) PRIMARY KEY,
    MaChiTietTraLoi INT NOT NULL UNIQUE,
    DiemNguPhap DECIMAL(5,2) NULL,
    DiemLienKetMachLac DECIMAL(5,2) NULL,
    DiemTuVung DECIMAL(5,2) NULL,
    DiemHoanThanhDe DECIMAL(5,2) NULL,
    DiemTongQuan DECIMAL(5,2) NULL,
    NhanXetChiTiet NVARCHAR(MAX) NULL,
    GoiYVietLai NVARCHAR(MAX) NULL,
    ThoiDiemDanhGia DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_DanhGiaBaiViet_ChiTietTraLoi
        FOREIGN KEY (MaChiTietTraLoi) REFERENCES ChiTietTraLoi(MaChiTietTraLoi)
);
GO

/* =========================================================
   7. THEO DÕI TIẾN ĐỘ - PHÂN TÍCH ĐIỂM YẾU - ĐỀ XUẤT
   ========================================================= */

CREATE TABLE LichSuHocTap (
    MaLichSu BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaBaiHoc INT NULL,
    MaLuotLamBai INT NULL,
    LoaiHoatDong NVARCHAR(30) NOT NULL, -- BaiHoc / LuyenTap / ThiThu / Flashcard
    SoPhutHoc INT NOT NULL DEFAULT 0,
    DiemSo DECIMAL(6,2) NULL,
    SoCauDung INT NULL,
    SoCauSai INT NULL,
    NgayGhiNhan DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_LichSuHocTap_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_LichSuHocTap_BaiHoc
        FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc),
    CONSTRAINT FK_LichSuHocTap_LuotLamBai
        FOREIGN KEY (MaLuotLamBai) REFERENCES LuotLamBai(MaLuotLamBai)
);
GO

CREATE TABLE DiemYeuNguoiDung (
    MaDiemYeu INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaChuDe INT NULL,
    MaChuDeNguPhap INT NULL,
    KyNang NVARCHAR(30) NULL,
    TyLeChinhXac DECIMAL(6,2) NULL,
    MucDoYeu NVARCHAR(20) NULL, -- Thap / TrungBinh / NghiemTrong
    LanPhanTichCuoi DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_DiemYeuNguoiDung_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_DiemYeuNguoiDung_ChuDe
        FOREIGN KEY (MaChuDe) REFERENCES ChuDe(MaChuDe),
    CONSTRAINT FK_DiemYeuNguoiDung_NguPhap
        FOREIGN KEY (MaChuDeNguPhap) REFERENCES ChuDeNguPhap(MaChuDeNguPhap)
);
GO

CREATE TABLE DeXuatHocTap (
    MaDeXuat INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaChuDe INT NULL,
    MaBaiHoc INT NULL,
    MaBaiTapDeThi INT NULL,
    LoaiDeXuat NVARCHAR(30) NOT NULL, -- BaiHoc / BaiTap / OnTap
    LyDoDeXuat NVARCHAR(1000) NULL,
    MucDoUuTien INT NOT NULL DEFAULT 1,
    DaHoanThanh BIT NOT NULL DEFAULT 0,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_DeXuatHocTap_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_DeXuatHocTap_ChuDe
        FOREIGN KEY (MaChuDe) REFERENCES ChuDe(MaChuDe),
    CONSTRAINT FK_DeXuatHocTap_BaiHoc
        FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc),
    CONSTRAINT FK_DeXuatHocTap_BaiTapDeThi
        FOREIGN KEY (MaBaiTapDeThi) REFERENCES BaiTapDeThi(MaBaiTapDeThi)
);
GO

/* =========================================================
   8. AI: HỘI THOẠI - PHÂN TÍCH - TỰ ĐỘNG SINH ĐỀ - LOGS
   ========================================================= */

CREATE TABLE PhienHoiThoaiAI (
    MaPhienHoiThoai BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    TieuDe NVARCHAR(200) NULL,
    NguCanhHoiThoai NVARCHAR(30) NULL, -- BaiHoc / CauHoi / Chung / BaiViet
    MaCauHoiLienQuan INT NULL,
    MaBaiHocLienQuan INT NULL,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    TinNhanCuoiVao DATETIME2 NULL,
    CONSTRAINT FK_PhienHoiThoaiAI_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_PhienHoiThoaiAI_CauHoi
        FOREIGN KEY (MaCauHoiLienQuan) REFERENCES CauHoi(MaCauHoi),
    CONSTRAINT FK_PhienHoiThoaiAI_BaiHoc
        FOREIGN KEY (MaBaiHocLienQuan) REFERENCES BaiHoc(MaBaiHoc)
);
GO

CREATE TABLE TinNhanAI (
    MaTinNhan BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaPhienHoiThoai BIGINT NOT NULL,
    LoaiNguoiGui NVARCHAR(20) NOT NULL, -- NguoiDung / AI
    NoiDungTinNhan NVARCHAR(MAX) NOT NULL,
    SoTokenDauVao INT NULL,
    SoTokenDauRa INT NULL,
    TongSoToken AS ISNULL(SoTokenDauVao,0) + ISNULL(SoTokenDauRa,0) PERSISTED,
    NgayGui DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_TinNhanAI_PhienHoiThoai
        FOREIGN KEY (MaPhienHoiThoai) REFERENCES PhienHoiThoaiAI(MaPhienHoiThoai)
);
GO

CREATE TABLE KetQuaPhanTichAI (
    MaKetQuaPhanTich BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaLuotLamBai INT NULL,
    LoaiPhanTich NVARCHAR(50) NOT NULL, -- GiaiThichDapAn / PhanTichDiemYeu / DeXuatHocTap
    DuLieuDauVao NVARCHAR(MAX) NULL,
    DuLieuKetQua NVARCHAR(MAX) NOT NULL,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_KetQuaPhanTichAI_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_KetQuaPhanTichAI_LuotLamBai
        FOREIGN KEY (MaLuotLamBai) REFERENCES LuotLamBai(MaLuotLamBai)
);
GO

CREATE TABLE YeuCauSinhDeAI (
    MaYeuCauSinhDe BIGINT IDENTITY(1,1) PRIMARY KEY,
    NguoiYeuCau INT NOT NULL,
    LoaiDauVao NVARCHAR(30) NOT NULL, -- ChuDe / VanBan / TepTin
    NoiDungDauVao NVARCHAR(MAX) NULL,
    MaTrinhDoMongMuon INT NULL,
    SoLuongCauHoi INT NULL,
    TrangThai NVARCHAR(30) NOT NULL DEFAULT N'DangCho', -- DangCho / DangXuLy / HoanThanh / ThatBai
    MoHinhAI NVARCHAR(100) NULL,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    NgayHoanThanh DATETIME2 NULL,
    CONSTRAINT FK_YeuCauSinhDeAI_NguoiDung
        FOREIGN KEY (NguoiYeuCau) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_YeuCauSinhDeAI_TrinhDo
        FOREIGN KEY (MaTrinhDoMongMuon) REFERENCES TrinhDo(MaTrinhDo)
);
GO

CREATE TABLE CauHoiDoAISinh (
    MaCauHoiAISinh BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaYeuCauSinhDe BIGINT NOT NULL,
    MaCauHoi INT NULL,
    NoiDungCauHoiSinhRa NVARCHAR(MAX) NULL,
    TrangThaiDuyet NVARCHAR(30) NOT NULL DEFAULT N'DangCho', -- DangCho / DaDuyet / TuChoi / DaChinhSua
    NguoiDuyet INT NULL,
    NgayDuyet DATETIME2 NULL,
    CONSTRAINT FK_CauHoiDoAISinh_YeuCau
        FOREIGN KEY (MaYeuCauSinhDe) REFERENCES YeuCauSinhDeAI(MaYeuCauSinhDe),
    CONSTRAINT FK_CauHoiDoAISinh_CauHoi
        FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi),
    CONSTRAINT FK_CauHoiDoAISinh_NguoiDuyet
        FOREIGN KEY (NguoiDuyet) REFERENCES NguoiDung(MaNguoiDung)
);
GO

CREATE TABLE NhatKyGoiAPI (
    MaNhatKyAPI BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NULL,
    MaPhienHoiThoai BIGINT NULL,
    NhaCungCap NVARCHAR(50) NULL, -- OpenAI / Gemini / Claude / Local...
    TenMoHinh NVARCHAR(100) NULL,
    DiemCuoiEndpoint NVARCHAR(300) NULL,
    SoTokenDauVao INT NULL,
    SoTokenDauRa INT NULL,
    TongSoToken AS ISNULL(SoTokenDauVao,0) + ISNULL(SoTokenDauRa,0) PERSISTED,
    ThoiGianPhanHoiMs INT NULL,
    MaTrangThaiHttp INT NULL,
    NgayGhiNhan DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_NhatKyGoiAPI_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_NhatKyGoiAPI_PhienHoiThoai
        FOREIGN KEY (MaPhienHoiThoai) REFERENCES PhienHoiThoaiAI(MaPhienHoiThoai)
);
GO

/* =========================================================
   9. QUẢN LÝ NHẬP TỆP TIN ĐỀ THI
   ========================================================= */

CREATE TABLE TepTinNhapLieu (
    MaTepTinNhap INT IDENTITY(1,1) PRIMARY KEY,
    NguoiTaiLen INT NOT NULL,
    TenTepTin NVARCHAR(255) NOT NULL,
    DinhDangTep NVARCHAR(20) NULL, -- Excel / Word / JSON
    DuongDanTep NVARCHAR(500) NULL,
    TrangThaiNhap NVARCHAR(30) NOT NULL DEFAULT N'DangCho', -- DangCho / DangXuLy / HoanThanh / Loi
    SoDongDaNhap INT NULL,
    ThongBaoLoi NVARCHAR(MAX) NULL,
    NgayTaiLen DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_TepTinNhapLieu_NguoiDung
        FOREIGN KEY (NguoiTaiLen) REFERENCES NguoiDung(MaNguoiDung)
);
GO

/* =========================================================
   10. CHỈ MỤC (INDEX)
   ========================================================= */

CREATE INDEX IX_NguoiDung_MaVaiTro ON NguoiDung(MaVaiTro);
CREATE INDEX IX_BaiHoc_ChuDe_TrinhDo ON BaiHoc(MaChuDe, MaTrinhDo);
CREATE INDEX IX_TuVung_ChuDe_TrinhDo ON TuVung(MaChuDe, MaTrinhDo);
CREATE INDEX IX_CauHoi_ChuDe_TrinhDo ON CauHoi(MaChuDe, MaTrinhDo);
CREATE INDEX IX_CauHoi_Loai_KyNang ON CauHoi(LoaiCauHoi, KyNang);
CREATE INDEX IX_LuotLamBai_NguoiDung_Ngay ON LuotLamBai(MaNguoiDung, ThoiDiemBatDau);
CREATE INDEX IX_LichSuHocTap_NguoiDung_Ngay ON LichSuHocTap(MaNguoiDung, NgayGhiNhan);
CREATE INDEX IX_PhienHoiThoaiAI_NguoiDung_Ngay ON PhienHoiThoaiAI(MaNguoiDung, NgayTao);
CREATE INDEX IX_NhatKyGoiAPI_NgayGhiNhan ON NhatKyGoiAPI(NgayGhiNhan);
GO

/* =========================================================
   11. VIEW TỔNG HỢP CHO DASHBOARD
   ========================================================= */

CREATE VIEW vw_TongHopTienTrinhHocTap
AS
SELECT
    nd.MaNguoiDung,
    nd.HoTen,
    hs.DiemMucTieu,
    hs.KhungNangLucMucTieu,
    hs.TrinhDoMucTieu,
    hs.SoPhutHocTichLuy,
    COUNT(DISTINCT lb.MaLuotLamBai) AS TongSoLuotLamBai,
    CAST(AVG(lb.DiemSo) AS DECIMAL(6,2)) AS DiemTrungBinh,
    SUM(ISNULL(lb.SoCauDung,0)) AS TongSoCauDung,
    SUM(ISNULL(lb.SoCauSai,0)) AS TongSoCauSai
FROM NguoiDung nd
LEFT JOIN HoSoHocTap hs
    ON nd.MaNguoiDung = hs.MaNguoiDung
LEFT JOIN LuotLamBai lb
    ON nd.MaNguoiDung = lb.MaNguoiDung
GROUP BY
    nd.MaNguoiDung,
    nd.HoTen,
    hs.DiemMucTieu,
    hs.KhungNangLucMucTieu,
    hs.TrinhDoMucTieu,
    hs.SoPhutHocTichLuy;
GO

CREATE TABLE ThongBao (
    MaThongBao BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,               -- Gửi cho ai (Admin hoặc Học viên)
    TieuDe NVARCHAR(150) NOT NULL,
    NoiDung NVARCHAR(500) NOT NULL,
    LoaiThongBao NVARCHAR(30) NULL,         -- NhacNhoHoc / DuyetDe / HeThong / KetQuaThi
    DuongDanDieuHuong NVARCHAR(255) NULL,   -- Link click vào chuyển đến trang tương ứng
    DaDoc BIT NOT NULL DEFAULT 0,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_ThongBao_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);

GO

PRINT 'Tạo Database thành công';
GO