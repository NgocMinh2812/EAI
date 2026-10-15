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

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
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
    LaLanDauDangNhap BIT NOT NULL DEFAULT 1,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    LanDangNhapCuoi DATETIME2 NULL,
    CONSTRAINT FK_NguoiDung_VaiTro
        FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);
GO

CREATE TABLE HoSoHocTap (
    MaHoSo INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL UNIQUE,
    DiemMucTieu DECIMAL(5,1) NULL,
    KhungNangLucMucTieu NVARCHAR(30) NULL, -- IELTS / TOEIC / CEFR...
    TrinhDoMucTieu NVARCHAR(10) NULL,     -- A1-C1... (Mục tiêu hướng tới)
    TrinhDoDauVao NVARCHAR(10) NULL,      -- A1, A2, B1, B2, C1... (Trình độ đánh giá / khảo sát đầu vào)
    DiemDanhGiaDauVao DECIMAL(5,2) NULL,  -- Điểm bài khảo sát / đánh giá năng lực đầu vào
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

/* =========================================================
   12. DỮ LIỆU KHỞI TẠO HOÀN CHỈNH CHO SPRINT 2 (DATABASE CHÍNH THỨC)
   ========================================================= */

-- 12.1. Vai Trò
INSERT INTO VaiTro(TenVaiTro, MoTa)
VALUES
(N'GiangVien', N'Giảng viên quản lý học liệu, đề thi, câu hỏi và phân tích học viên'),
(N'HocVien', N'Người học tiếng Anh ôn luyện, làm bài tập và thi thử'),
(N'QuanTriVien', N'Quản trị viên toàn quyền cấu hình hệ thống');
GO

-- 12.2. Trình Độ Khung Tham Chiếu Châu Âu (CEFR)
INSERT INTO TrinhDo(MaCapDo, TenTrinhDo, MoTa)
VALUES
(N'A1', N'Beginner', N'Trình độ mở đầu, nắm bắt từ ngữ giao tiếp đơn giản'),
(N'A2', N'Elementary', N'Trình độ sơ cấp, giao tiếp các tình huống thông thường'),
(N'B1', N'Intermediate', N'Trình độ trung cấp, hiểu ý chính văn bản và giao tiếp tự tin'),
(N'B2', N'Upper-Intermediate', N'Trình độ trên trung cấp, sử dụng tiếng Anh linh hoạt và học thuật'),
(N'C1', N'Advanced', N'Trình độ cao cấp, diễn đạt trôi chảy và hiệu quả trong mọi ngữ cảnh'),
(N'C2', N'Proficiency', N'Trình độ thành thạo, tương đương người bản ngữ');
GO

-- 12.3. Tài khoản Người dùng (Mật khẩu chuẩn SHA256: 123456 -> 8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92)
DECLARE @idGV INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro=N'GiangVien');
DECLARE @idHV INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro=N'HocVien');
DECLARE @idQTV INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro=N'QuanTriVien');

INSERT INTO NguoiDung(MaVaiTro, TenDangNhap, MatKhauMaHoa, Email, HoTen, SoDienThoai, TrangThaiHoatDong, LaLanDauDangNhap, NgayTao)
VALUES
(@idQTV, N'admin', N'8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92', N'admin@tienganhai.vn', N'Quản trị viên Hệ thống', N'0999888777', 1, 0, SYSDATETIME()),
(@idGV, N'giangvien', N'8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92', N'giangvien@tienganhai.vn', N'Giảng viên Nguyễn Văn An', N'0988777666', 1, 0, SYSDATETIME()),
(@idHV, N'hocvien', N'8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92', N'hocvien@tienganhai.vn', N'Học viên Trần Thị Mai', N'0977666555', 1, 0, SYSDATETIME()),
(@idHV, N'lan.nguyen', N'8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92', N'lan.nguyen@gmail.com', N'Nguyễn Thị Lan', N'0901234561', 1, 0, SYSDATETIME());
GO

-- 12.4. Hồ sơ học tập
INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan)
SELECT MaNguoiDung, 7.0, N'IELTS', N'B2', N'B1', 6.5, 420, N'Tập trung luyện Reading và Writing'
FROM NguoiDung WHERE TenDangNhap=N'hocvien';

INSERT INTO HoSoHocTap(MaNguoiDung, DiemMucTieu, KhungNangLucMucTieu, TrinhDoMucTieu, TrinhDoDauVao, DiemDanhGiaDauVao, SoPhutHocTichLuy, GhiChuCaNhan)
SELECT MaNguoiDung, 6.5, N'IELTS', N'B2', N'B1', 6.0, 310, N'Mục tiêu xét tuyển đại học'
FROM NguoiDung WHERE TenDangNhap=N'lan.nguyen';
GO

-- 12.5. Danh mục Chủ đề (ChuDe)
INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Tiếng Anh Giao Tiếp', N'Các chủ đề giao tiếp thực tế hàng ngày', 1);
DECLARE @cdGiaoTiep INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@cdGiaoTiep, N'Chào hỏi & Giới thiệu bản thân', N'Từ vựng và mẫu câu khi mới gặp mặt', 1),
(@cdGiaoTiep, N'Mua sắm & Nhà hàng ẩm thực', N'Mẫu câu gọi món, trả giá, thanh toán', 1),
(@cdGiaoTiep, N'Du lịch & Phương tiện giao thông', N'Hỏi đường, thủ tục sân bay, khách sạn', 1);

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Tiếng Anh Học Thuật (IELTS)', N'Luyện thi học thuật và các kỹ năng chuyên sâu', 1);
DECLARE @cdAcademic INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@cdAcademic, N'IELTS Reading - Skimming & Scanning', N'Kỹ năng đọc nhanh, tìm từ khóa', 1),
(@cdAcademic, N'IELTS Writing Task 2 - Luận xã hội', N'Cấu trúc bài luận 4 đoạn chuẩn học thuật', 1),
(@cdAcademic, N'IELTS Listening - Section 3 & 4', N'Hội thoại học thuật và bài giảng đại học', 1);

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES (NULL, N'Ngữ Pháp Cốt Lõi', N'Hệ thống ngữ pháp tiếng Anh từ căn bản đến nâng cao', 1);
DECLARE @cdGrammar INT = SCOPE_IDENTITY();

INSERT INTO ChuDe(MaChuDeCha, TenChuDe, MoTa, TrangThaiHoatDong)
VALUES 
(@cdGrammar, N'Các thì trong tiếng Anh (Tenses)', N'12 thì và ứng dụng thực tiễn', 1),
(@cdGrammar, N'Mệnh đề quan hệ & Câu bị động', N'Ngữ pháp ghép câu và diễn đạt tự nhiên', 1),
(@cdGrammar, N'Câu điều kiện & Đảo ngữ', N'Ngữ pháp nâng cao cho band điểm cao', 1);
GO

-- 12.6. Danh mục Từ Vựng & Flashcard (TuVung)
DECLARE @tdA1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'A1');
DECLARE @tdA2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'A2');
DECLARE @tdB1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'B1');
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'B2');
DECLARE @tdC1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'C1');
DECLARE @cdGT INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Giao Tiếp%');
DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS%');

INSERT INTO TuVung (MaChuDe, MaTrinhDo, Tu, LoaiTu, PhienAmIPA, Nghia, CauViDu, CumTuDiKem)
VALUES
(@cdIELTS, @tdC1, N'Ubiquitous', N'adj', N'/juːˈbɪk.wɪ.təs/', 
 N'Phổ biến ở khắp mọi nơi, hiện diện đồng thời ở mọi chỗ (seeming to be everywhere).', 
 N'"Smartphones have become ubiquitous in daily modern life, radically transforming how students study."', 
 N'• Ubiquitous influence • Ubiquitous presence of technology'),
(@cdIELTS, @tdC1, N'Meticulous', N'adj', N'/məˈtɪk.jə.ləs/', 
 N'Tỉ mỉ, cẩn thận từng chi tiết nhỏ nhất (very careful and with great attention to every detail).', 
 N'"The research paper required meticulous attention to detail to avoid statistical discrepancies."', 
 N'• Meticulous research • Meticulous planning'),
(@cdIELTS, @tdB2, N'Pragmatic', N'adj', N'/præɡˈmæt.ɪk/', 
 N'Thực tế, thực dụng, dựa trên tính khả thi thay vì lý thuyết suông.', 
 N'"In dealing with environmental issues, governments must take a pragmatic and balanced approach."', 
 N'• Pragmatic solution • Pragmatic approach'),
(@cdIELTS, @tdC1, N'Corroborate', N'verb', N'/kəˈrɒb.ə.reɪt/', 
 N'Chứng minh, củng cố thêm bằng chứng xác thực (to confirm or support with new evidence).', 
 N'"Subsequent geological studies corroborated the theory proposed by scientists a decade earlier."', 
 N'• Corroborate evidence • Corroborate findings'),
(@cdIELTS, @tdB2, N'Detrimental', N'adj', N'/ˌdet.rɪˈmen.təl/', 
 N'Gây hại, có hại (causing harm or damage).', 
 N'"Excessive screen time before sleeping is proven to have a detrimental effect on sleep quality."', 
 N'• Detrimental effect • Detrimental impact'),
(@cdIELTS, @tdB2, N'Inevitable', N'adj', N'/ɪnˈev.ɪ.tə.bəl/', 
 N'Không thể tránh khỏi, chắc chắn sẽ xảy ra.', 
 N'"With the rapid growth of AI, changes in the job market are inevitable."', 
 N'• Inevitable consequence • Inevitable outcome'),
(@cdIELTS, @tdB1, N'Resilient', N'adj', N'/rɪˈzɪl.jənt/', 
 N'Kiên cường, có khả năng phục hồi nhanh sau khó khăn hoặc nghịch cảnh.', 
 N'"Local communities showed themselves to be remarkably resilient after the severe storm."', 
 N'• Highly resilient • Resilient economy'),
(@cdIELTS, @tdB2, N'Feasible', N'adj', N'/ˈfiː.zə.bəl/', 
 N'Khả thi, có thể thực hiện được một cách thực tế.', 
 N'"It is not economically feasible to replace all fossil fuel power plants overnight."', 
 N'• Technically feasible • Feasible alternative'),
(@cdIELTS, @tdB2, N'Ambiguous', N'adj', N'/æmˈbɪɡ.ju.əs/', 
 N'Mơ hồ, có thể hiểu theo nhiều nghĩa khác nhau.', 
 N'"The instructions given by the supervisor were ambiguous and caused significant confusion."', 
 N'• Ambiguous statement • Ambiguous wording'),
(@cdIELTS, @tdB1, N'Procrastinate', N'verb', N'/prəˈkræs.tɪ.neɪt/', 
 N'Trì hoãn, chần chừ không làm việc cần làm ngay.', 
 N'"Most students tend to procrastinate until the night before the final assignment is due."', 
 N'• Procrastinate on homework • Chronic procrastination'),
(@cdGT, @tdA1, N'Enthusiastic', N'adj', N'/ɪnˌθjuː.ziˈæs.tɪk/', 
 N'Nhiệt tình, hăng hái, hào hứng.', 
 N'"The new teacher received an enthusiastic welcome from all the students."', 
 N'• Enthusiastic support • Enthusiastic about learning'),
(@cdGT, @tdA2, N'Diligent', N'adj', N'/ˈdɪl.ɪ.dʒənt/', 
 N'Chăm chỉ, siêng năng, cần cù.', 
 N'"She was a diligent student who consistently reviewed her lecture notes every evening."', 
 N'• Diligent worker • Diligent effort');
GO

-- 12.7. Ngân hàng Câu hỏi và Đáp án Chuẩn hóa (CauHoi & LuaChonDapAn)
DECLARE @tdA1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'A1');
DECLARE @tdA2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'A2');
DECLARE @tdB1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'B1');
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'B2');
DECLARE @tdC1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'C1');
DECLARE @cdGrammar INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%Ngữ Pháp%');
DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS%');
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDangNhap = N'giangvien');

-- Câu 1 (A1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA1, N'TracNghiem', N'NguPhap', N'Choose the correct verb form: She ______ to school by bus every morning.', 
        N'Chủ ngữ ngôi thứ ba số ít "She" trong thì hiện tại đơn đi với động từ thêm -s/-es ("goes").', @gvId, 1);
DECLARE @q1 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q1, 'A', N'go', 0), (@q1, 'B', N'goes', 1), (@q1, 'C', N'going', 0), (@q1, 'D', N'went', 0);

-- Câu 2 (A1 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA1, N'TracNghiem', N'TuVung', N'I usually have bread and eggs for ______ at 7:00 AM.', 
        N'"Breakfast" là bữa ăn sáng vào lúc 7:00 AM.', @gvId, 1);
DECLARE @q2 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q2, 'A', N'dinner', 0), (@q2, 'B', N'lunch', 0), (@q2, 'C', N'breakfast', 1), (@q2, 'D', N'supper', 0);

-- Câu 3 (A2 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdA2, N'TracNghiem', N'NguPhap', N'Yesterday we ______ a very interesting documentary about marine life.', 
        N'Trạng từ thời gian "Yesterday" báo hiệu thì quá khứ đơn ("watched").', @gvId, 1);
DECLARE @q3 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q3, 'A', N'watch', 0), (@q3, 'B', N'watches', 0), (@q3, 'C', N'watched', 1), (@q3, 'D', N'watching', 0);

-- Câu 4 (B1 Grammar)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB1, N'TracNghiem', N'NguPhap', N'If it ______ tomorrow, we will have to postpone our outdoor picnic.', 
        N'Mệnh đề điều kiện loại 1 (If + Present Simple, will + V) dùng "rains".', @gvId, 1);
DECLARE @q4 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q4, 'A', N'rained', 0), (@q4, 'B', N'will rain', 0), (@q4, 'C', N'rains', 1), (@q4, 'D', N'would rain', 0);

-- Câu 5 (B1 Reading)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, NguoiTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdB1, N'TracNghiem', N'Doc', 
 N'According to the passage, why do owls hunt predominantly at night?', 
 N'Owls are nocturnal birds of prey found on every continent except Antarctica. Their large eyes and exceptional binaural hearing allow them to detect the rustling of small rodents under leaf litter even in pitch-black conditions. Hunting at night also helps them avoid direct competition with diurnal raptors like hawks and eagles.',
 N'Đoạn văn nêu rõ cú đêm săn mồi vào ban đêm vì thính giác/thị giác vượt trội và để tránh cạnh tranh trực tiếp với các loài chim săn mồi ban ngày (hawks and eagles).',
 @gvId, 1);
DECLARE @q5 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q5, 'A', N'They cannot tolerate warm sunshine.', 0),
(@q5, 'B', N'Their specialized senses excel in the dark and they avoid daytime competitors.', 1),
(@q5, 'C', N'Rodents are only active during morning hours.', 0),
(@q5, 'D', N'They prefer socializing in darkness.', 0);

-- Câu 6 (B2 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdB2, N'TracNghiem', N'TuVung', N'The committee decided to ______ stricter environmental policies to curb industrial carbon emissions.', 
        N'Cụm từ "implement policies" có nghĩa là thi hành/áp dụng chính sách.', @gvId, 1);
DECLARE @q6 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q6, 'A', N'implement', 1), (@q6, 'B', N'compliment', 0), (@q6, 'C', N'supplement', 0), (@q6, 'D', N'experiment', 0);

-- Câu 7 (B2 Grammar - Inversion)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'Seldom ______ such a breathtaking display of aurora borealis in this region.', 
        N'Đảo ngữ với trạng từ phủ định đứng đầu câu: Seldom + trợ động từ + S + V chính ("have we witnessed").', @gvId, 1);
DECLARE @q7 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q7, 'A', N'we have witnessed', 0), (@q7, 'B', N'have we witnessed', 1), (@q7, 'C', N'we witnessed', 0), (@q7, 'D', N'had witnessed we', 0);

-- Câu 8 (C1 Vocabulary)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'TuVung', N'Subsequent empirical studies completely ______ the original hypothesis proposed by Dr. Vance.', 
        N'"Corroborate" nghĩa là chứng thực, củng cố bằng chứng cho giả thuyết.', @gvId, 1);
DECLARE @q8 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q8, 'A', N'corroborated', 1), (@q8, 'B', N'retracted', 0), (@q8, 'C', N'dismantled', 0), (@q8, 'D', N'diminished', 0);

-- Câu 9 (C1 Reading)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, DoanVanDoc, GiaiThichDapAn, NguoiTao, DaKiemDuyet)
VALUES (@cdIELTS, @tdC1, N'TracNghiem', N'Doc', 
 N'The author uses the phrase "double-edged sword" to emphasize that artificial intelligence:', 
 N'The integration of generative artificial intelligence across industrial workflows represents a double-edged sword. On one hand, automated synthesis and code generation accelerate throughput exponentially. Conversely, intellectual property vulnerabilities and cognitive atrophy among junior engineers present formidable existential hurdles for long-term innovation.', 
 N'"Double-edged sword" (con dao hai lưỡi) biểu thị cả lợi ích to lớn (tăng tốc độ) lẫn rủi ro tiềm tàng (vấn đề sở hữu trí tuệ, suy giảm tư duy).',
 @gvId, 1);
DECLARE @q9 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q9, 'A', N'is entirely hazardous and must be prohibited immediately', 0),
(@q9, 'B', N'delivers immense productivity gains while simultaneously introducing substantial perils', 1),
(@q9, 'C', N'has surpassed human cognition in ethical discernment', 0),
(@q9, 'D', N'is exclusively beneficial for junior software developers', 0);

-- Câu 10 (B2 Grammar - Subjunctive)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, NguoiTao, DaKiemDuyet)
VALUES (@cdGrammar, @tdB2, N'TracNghiem', N'NguPhap', N'The doctor recommended that the patient ______ adequate rest and avoid heavy lifting.', 
        N'Cấu trúc giả định thức (Subjunctive): recommend that S + (should) V-nguyên thể ("take").', @gvId, 1);
DECLARE @q10 INT = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q10, 'A', N'takes', 0), (@q10, 'B', N'took', 0), (@q10, 'C', N'take', 1), (@q10, 'D', N'will take', 0);
GO

-- 12.8. Khảo Sát Đánh Giá Năng Lực Đầu Vào (BaiKhaoSatDauVao & CauHoiKhaoSat)
INSERT INTO BaiKhaoSatDauVao (TenBaiKhaoSat, MoTa, TongSoCauHoi, DangHoatDong, NgayTao)
VALUES (N'Khảo sát Đánh giá Năng lực Đầu vào Chuẩn Quốc Tế CEFR', 
        N'Bài thi chẩn đoán phân loại trình độ từ A1 đến C1 giúp hệ thống cá nhân hóa lộ trình học.', 10, 1, SYSDATETIME());
DECLARE @maBKS INT = SCOPE_IDENTITY();

INSERT INTO CauHoiKhaoSat (MaBaiKhaoSat, MaCauHoi, ThuTuCauHoi)
SELECT @maBKS, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC)
FROM CauHoi;
GO

-- 12.9. Đề Thi Thử & Luyện Tập Chuẩn Hóa Sprint 2 (BaiTapDeThi & CauHoiBaiTap)
DECLARE @tdB2 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'B2');
DECLARE @tdB1 INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo WHERE MaCapDo=N'B1');
DECLARE @cdIELTS INT = (SELECT TOP 1 MaChuDe FROM ChuDe WHERE TenChuDe LIKE N'%IELTS%');
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE TenDangNhap = N'giangvien');

-- Đề 1: IELTS Academic Mock Test #05
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB2, N'IELTS Academic Reading & Listening Mini Mock Test #05', N'ThiThu', 
        N'Bao gồm bài đọc Skimming/Scanning, phân tích từ vựng học thuật C1 và cấu trúc ngữ pháp đảo ngữ.', 40, 1, @gvId);
DECLARE @de1 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @de1, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0 FROM CauHoi;

-- Đề 2: TOEIC Reading Comprehension
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB1, N'TOEIC Reading Comprehension Diagnostic Test', N'ThiThu', 
        N'Part 5 (Hoàn thành câu), Part 6 (Điền đoạn văn) và Part 7 (Đọc hiểu văn bản thương mại).', 45, 1, @gvId);
DECLARE @de2 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @de2, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi DESC), 1.0 FROM CauHoi;

-- Đề 3: Kiểm tra Đột phá Ngữ pháp & Collocations Band B2
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB2, N'Kiểm tra Đột phá Ngữ pháp & Collocations Band B2', N'ThiThu', 
        N'10 câu trắc nghiệm câu điều kiện đảo ngữ, mệnh đề quan hệ rút gọn, liên từ nâng cao.', 25, 1, @gvId);
DECLARE @de3 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @de3, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0 FROM CauHoi;

-- Đề 4: Thử Thách Phản Xạ Trắc Nghiệm 60s
INSERT INTO BaiTapDeThi (MaChuDe, MaTrinhDo, TenBaiTapDeThi, LoaiHinh, MoTa, ThoiGianLamBaiPhut, DaPhatHanh, NguoiTao)
VALUES (@cdIELTS, @tdB1, N'Thử Thách Phản Xạ Trắc Nghiệm 60s', N'LuyenTap', 
        N'Luyện tập phản xạ nhanh từ vựng và ngữ pháp tiếng Anh trong 60 giây.', 10, 1, @gvId);
DECLARE @de4 INT = SCOPE_IDENTITY();
INSERT INTO CauHoiBaiTap (MaBaiTapDeThi, MaCauHoi, ThuTuCauHoi, DiemSo)
SELECT @de4, MaCauHoi, ROW_NUMBER() OVER(ORDER BY MaCauHoi ASC), 1.0 FROM CauHoi WHERE MaCauHoi IN (SELECT TOP 5 MaCauHoi FROM CauHoi);
GO

PRINT 'Tạo Database và khởi tạo dữ liệu Sprint 2 hoàn chỉnh thành công!';
GO

/* VIEW tương thích cho KetQuaThi */
IF NOT EXISTS (SELECT 1 FROM sys.views WHERE Name = 'KetQuaThi')
BEGIN
    EXEC('CREATE VIEW KetQuaThi AS SELECT MaLuotLamBai AS MaKetQuaThi, MaNguoiDung, MaBaiTapDeThi, HinhThucLam, ThoiDiemBatDau, ThoiDiemNopBai, ThoiGianLamGiay, TongSoCau, SoCauDung, SoCauSai, DiemSo, TrangThai FROM LuotLamBai');
END
GO