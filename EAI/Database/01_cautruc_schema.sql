/*
    DATABASE: TiengAnhAI
    Mục đích: CSDL cho đề tài "Ứng dụng khảo thí & học tiếng Anh tích hợp AI"
    Hệ quản trị: Microsoft SQL Server
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
    TrinhDoMucTieu NVARCHAR(10) NULL,     -- A1-C1...
    TrinhDoDauVao NVARCHAR(10) NULL,      -- A1, A2, B1, B2, C1... (Trình độ khảo sát đầu vào)
    DiemDanhGiaDauVao DECIMAL(5,2) NULL,  -- Điểm bài khảo sát / đánh giá năng lực đầu vào
    SoPhutHocTichLuy INT NOT NULL DEFAULT 0,
    GhiChuCaNhan NVARCHAR(MAX) NULL,
    NgayCapNhat DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_HoSoHocTap_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);
GO

/* =========================================================
   2. TRÌNH ĐỘ - CHỦ ĐỀ - BÀI HỌC - AUDIO TRANSCRIPT
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
    AudioPath NVARCHAR(500) NULL,
    AnhMinhHoaPath NVARCHAR(500) NULL,
    ThoiGianUocTinhPhut INT NULL,
    ThoiLuongUocTinh INT NOT NULL DEFAULT 15,
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

CREATE TABLE BaiHocAudioTranscript (
    MaAudioTranscript INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaBaiHoc INT NULL,
    AudioPath NVARCHAR(500) NULL,
    LoaiAudio NVARCHAR(50) NOT NULL CONSTRAINT DF_BaiHocAudio_Loai DEFAULT N'Uploaded', -- 'Uploaded', 'AiTts', 'SpeakingPractice'
    TieuDeAudio NVARCHAR(200) NULL,
    ScriptGoc NVARCHAR(MAX) NULL,                     -- Kịch bản nếu được tạo từ TTS
    TranscriptText NVARCHAR(MAX) NULL,                -- Nội dung bản chép lời toàn bài
    TranscriptSegmentsJson NVARCHAR(MAX) NULL,        -- JSON danh sách phân đoạn: [{id, start, end, text}]
    Provider NVARCHAR(50) NOT NULL CONSTRAINT DF_BaiHocAudio_Provider DEFAULT N'Groq',
    MoHinhSTT NVARCHAR(100) NULL,                    -- Ví dụ: 'whisper-large-v3-turbo'
    MoHinhTTS NVARCHAR(100) NULL,                    -- Ví dụ: 'canopylabs/orpheus-v1-english'
    VoiceName NVARCHAR(50) NULL,                     -- Ví dụ: 'diana', 'troy', 'autumn', 'daniel'
    AudioHash NVARCHAR(64) NULL,                     -- SHA256 mã băm audio/script để tra cứu cache
    TrangThaiDuyet NVARCHAR(50) NOT NULL CONSTRAINT DF_BaiHocAudio_TrangThai DEFAULT N'ChoDuyet', -- 'ChoDuyet', 'DaDuyet', 'TuChoi'
    ChoPhepSinhVienXem BIT NOT NULL CONSTRAINT DF_BaiHocAudio_ChoPhepXem DEFAULT 1,
    ThoiLuongGiay FLOAT NULL,
    DungLuongBytes BIGINT NULL,
    NguoiTao INT NULL,
    NgayTao DATETIME2 NOT NULL CONSTRAINT DF_BaiHocAudio_NgayTao DEFAULT SYSDATETIME(),
    NgayCapNhat DATETIME2 NOT NULL CONSTRAINT DF_BaiHocAudio_NgayCapNhat DEFAULT SYSDATETIME(),
    CONSTRAINT FK_BaiHocAudioTranscript_BaiHoc 
        FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc) ON DELETE SET NULL
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
   4. CẤU TRÚC ĐỀ THI (BLUEPRINTS) - CÂU HỎI - ĐỀ THI
   ========================================================= */

CREATE TABLE CauTrucDeThi (
    MaCauTruc INT IDENTITY(1,1) PRIMARY KEY,
    TenCauTruc NVARCHAR(200) NOT NULL,
    MaKyThi NVARCHAR(50) NOT NULL,               -- IELTS, TOEIC, VSTEP, CAMBRIDGE_KET, CAMBRIDGE_PET, CAMBRIDGE_FCE, CAMBRIDGE_CAE, STARTERS, MOVERS, FLYERS
    MaCapDo NVARCHAR(10) NOT NULL,               -- A1, A2, B1, B2, C1, MULTI
    LoaiDeThi NVARCHAR(50) NOT NULL DEFAULT 'ThiThu', -- ThiThu, LuyenTap, TongHop
    ThoiGianPhut INT NOT NULL,
    TongSoCau INT NOT NULL,
    TongDiem FLOAT NOT NULL DEFAULT 10.0,
    MoTa NVARCHAR(1000) NULL,
    NguonThamChieu NVARCHAR(255) NULL,           -- Link / Tài liệu quy chuẩn khảo thí quốc tế
    NgayXacMinh DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    TrangThai NVARCHAR(30) NOT NULL DEFAULT 'HoatDong'
);
GO

CREATE TABLE ChiTietCauTrucDeThi (
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaCauTruc INT NOT NULL,
    TenPhan NVARCHAR(200) NOT NULL,              -- Ví dụ: 'Part 1: Photographs', 'Reading Passage 1', 'Section 1: Listening'
    ThuTuPhan INT NOT NULL DEFAULT 1,
    KyNang NVARCHAR(50) NOT NULL,                -- Nghe, Doc, Viet, Noi, NguPhap, TuVung
    LoaiCauHoiChoPhep NVARCHAR(100) NOT NULL,    -- TracNghiem, DienKhuyet, TrueFalseNotGiven, DungSai, NoiCap, TuLuan, v.v.
    SoLuongCau INT NOT NULL,
    DiemMoiCau FLOAT NOT NULL DEFAULT 1.0,
    ThoiGianPhut INT NULL,
    YeuCauDoanVan BIT NOT NULL DEFAULT 0,
    YeuCauAudio BIT NOT NULL DEFAULT 0,
    MoTa NVARCHAR(500) NULL,
    CONSTRAINT FK_ChiTietCauTrucDeThi_CauTrucDeThi
        FOREIGN KEY (MaCauTruc) REFERENCES CauTrucDeThi(MaCauTruc) ON DELETE CASCADE
);
GO

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
    DoKho NVARCHAR(20) NOT NULL DEFAULT N'TrungBinh',
    LoiGiaiChiTiet NVARCHAR(MAX) NULL,
    NguoiTao INT NULL,
    NguonTao NVARCHAR(30) NOT NULL DEFAULT N'ThuCong', -- ThuCong / NhapFile / AI / HocLieuChuan
    DaKiemDuyet BIT NOT NULL DEFAULT 0,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    LoaiNguon NVARCHAR(50) NULL DEFAULT 'Extracted',
    TepTinNguon NVARCHAR(255) NULL,
    MaDinhDanhSeed VARCHAR(100) NULL,
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
    KyHieuLuaChon NVARCHAR(20) NOT NULL, -- A / B / C / D / RUBRIC
    NoiDungLuaChon NVARCHAR(MAX) NOT NULL,
    LaDapAnDung BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_LuaChonDapAn_CauHoi
        FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi) ON DELETE CASCADE
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
    DiemDat FLOAT NOT NULL DEFAULT 5.0,
    TongDiem FLOAT NOT NULL DEFAULT 10.0,
    TongSoCau INT NOT NULL DEFAULT 0,
    XaoTronCauHoi BIT NOT NULL DEFAULT 1,
    DaPhatHanh BIT NOT NULL DEFAULT 0,
    ChoXemDapAn BIT NOT NULL DEFAULT 1,
    ChoXemLoiGiai BIT NOT NULL DEFAULT 1,
    SoLuotLamToiDa INT NOT NULL DEFAULT 0,
    TrangThai NVARCHAR(30) NOT NULL DEFAULT N'BanNhap', -- BanNhap / DaXuatBan / DaDong
    DuongDanAudio NVARCHAR(500) NULL,
    SoLanPhatAudio INT NOT NULL DEFAULT 0,
    NguoiTao INT NULL,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    MaCauTrucDeThi INT NULL,
    CONSTRAINT FK_BaiTapDeThi_BaiHoc
        FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc),
    CONSTRAINT FK_BaiTapDeThi_ChuDe
        FOREIGN KEY (MaChuDe) REFERENCES ChuDe(MaChuDe),
    CONSTRAINT FK_BaiTapDeThi_TrinhDo
        FOREIGN KEY (MaTrinhDo) REFERENCES TrinhDo(MaTrinhDo),
    CONSTRAINT FK_BaiTapDeThi_NguoiTao
        FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_BaiTapDeThi_CauTrucDeThi
        FOREIGN KEY (MaCauTrucDeThi) REFERENCES CauTrucDeThi(MaCauTruc)
);
GO

CREATE TABLE CauHoiBaiTap (
    MaCauHoiBaiTap INT IDENTITY(1,1) PRIMARY KEY,
    MaBaiTapDeThi INT NOT NULL,
    MaCauHoi INT NOT NULL,
    ThuTuCauHoi INT NOT NULL,
    DiemSo DECIMAL(5,2) NOT NULL DEFAULT 1,
    NoiDungCauHoiSnapshot NVARCHAR(MAX) NULL,
    DoanVanDocSnapshot NVARCHAR(MAX) NULL,
    GiaiThichDapAnSnapshot NVARCHAR(MAX) NULL,
    AudioPathSnapshot NVARCHAR(500) NULL,
    LoaiCauHoiSnapshot NVARCHAR(30) NULL,
    KyNangSnapshot NVARCHAR(30) NULL,
    DoKhoSnapshot NVARCHAR(20) NULL,
    DanhSachLuaChonJson NVARCHAR(MAX) NULL,
    CONSTRAINT UQ_CauHoiBaiTap UNIQUE(MaBaiTapDeThi, MaCauHoi),
    CONSTRAINT FK_CauHoiBaiTap_BaiTapDeThi
        FOREIGN KEY (MaBaiTapDeThi) REFERENCES BaiTapDeThi(MaBaiTapDeThi) ON DELETE CASCADE,
    CONSTRAINT FK_CauHoiBaiTap_CauHoi
        FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi)
);
GO

/* =========================================================
   5. KHẢO SÁT TRÌNH ĐỘ ĐẦU VÀO
   ========================================================= */

CREATE TABLE CauHinhQuyDoiKhaoSat (
    MaCauHinh INT IDENTITY(1,1) PRIMARY KEY,
    MaCapDo NVARCHAR(10) NOT NULL,            -- A1, A2, B1, B2, C1
    TenHienThi NVARCHAR(100) NOT NULL,        -- A1 (Mất gốc), A2 (Sơ cấp), ...
    DiemPhanTramMin INT NOT NULL,             -- 0, 31, 51, 71, 86
    DiemPhanTramMax INT NOT NULL,             -- 30, 50, 70, 85, 100
    QuyDoiIelts NVARCHAR(50) NOT NULL,        -- IELTS < 3.0, ...
    QuyDoiToeic NVARCHAR(50) NOT NULL,        -- TOEIC 100 - 250, ...
    LoTrinhDeXuat NVARCHAR(500) NOT NULL,
    MauSac NVARCHAR(20) NULL,                 -- Mã màu HEX giao diện (#DC2626, ...)
    ThuTuSapXep INT NOT NULL DEFAULT 1
);
GO

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
        FOREIGN KEY (MaBaiKhaoSat) REFERENCES BaiKhaoSatDauVao(MaBaiKhaoSat) ON DELETE CASCADE,
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
        FOREIGN KEY (MaLuotKhaoSat) REFERENCES LuotKhaoSat(MaLuotKhaoSat) ON DELETE CASCADE,
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
        FOREIGN KEY (MaLuotLamBai) REFERENCES LuotLamBai(MaLuotLamBai) ON DELETE CASCADE,
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
        FOREIGN KEY (MaChiTietTraLoi) REFERENCES ChiTietTraLoi(MaChiTietTraLoi) ON DELETE CASCADE
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
        FOREIGN KEY (MaPhienHoiThoai) REFERENCES PhienHoiThoaiAI(MaPhienHoiThoai) ON DELETE CASCADE
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
        FOREIGN KEY (MaYeuCauSinhDe) REFERENCES YeuCauSinhDeAI(MaYeuCauSinhDe) ON DELETE CASCADE,
    CONSTRAINT FK_CauHoiDoAISinh_CauHoi
        FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi),
    CONSTRAINT FK_CauHoiDoAISinh_NguoiDuyet
        FOREIGN KEY (NguoiDuyet) REFERENCES NguoiDung(MaNguoiDung)
);
GO

CREATE TABLE NoiDungHocTapAIBaiHoc (
    MaNoiDungAI INT IDENTITY(1,1) PRIMARY KEY,
    MaBaiHoc INT NOT NULL,
    ContentHash NVARCHAR(64) NOT NULL,
    TrangThai NVARCHAR(30) NOT NULL DEFAULT N'ChuaTao', -- ChuaTao / DangXuLy / HoanThanh / ThatBai
    KyNang NVARCHAR(50) NULL,
    MucTieu NVARCHAR(1000) NULL,
    DuLieuKetQuaJson NVARCHAR(MAX) NULL,
    MoHinhAI NVARCHAR(100) NULL,
    ThongBaoLoi NVARCHAR(MAX) NULL,
    ThoiDiemBatDauXuLy DATETIME2 NULL,
    NgayTao DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    NgayCapNhat DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT UQ_NoiDungHocTapAIBaiHoc_MaBaiHoc UNIQUE (MaBaiHoc),
    CONSTRAINT FK_NoiDungHocTapAIBaiHoc_BaiHoc 
        FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc) ON DELETE CASCADE
);
GO

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
    NgayCapNhat DATETIME2 NULL,
    CONSTRAINT FK_BoCauHoi_BaiHoc
        FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc) ON DELETE SET NULL,
    CONSTRAINT FK_BoCauHoi_ChuDe
        FOREIGN KEY (MaChuDe) REFERENCES ChuDe(MaChuDe),
    CONSTRAINT FK_BoCauHoi_TrinhDo
        FOREIGN KEY (MaTrinhDo) REFERENCES TrinhDo(MaTrinhDo),
    CONSTRAINT FK_BoCauHoi_NguoiTao
        FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaNguoiDung)
);
GO

CREATE TABLE BoCauHoi_CauHoi (
    MaBoCauHoi INT NOT NULL,
    MaCauHoi INT NOT NULL,
    ThuTu INT NOT NULL DEFAULT 1,
    PRIMARY KEY (MaBoCauHoi, MaCauHoi),
    CONSTRAINT FK_BoCauHoi_CauHoi_Bo
        FOREIGN KEY (MaBoCauHoi) REFERENCES BoCauHoi(MaBoCauHoi) ON DELETE CASCADE,
    CONSTRAINT FK_BoCauHoi_CauHoi_CH
        FOREIGN KEY (MaCauHoi) REFERENCES CauHoi(MaCauHoi) ON DELETE CASCADE
);
GO

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
    CONSTRAINT FK_KetQuaLamBaiBoCauHoi_NguoiDung
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung) ON DELETE CASCADE,
    CONSTRAINT FK_KetQuaLamBaiBoCauHoi_BoCauHoi
        FOREIGN KEY (MaBoCauHoi) REFERENCES BoCauHoi(MaBoCauHoi) ON DELETE CASCADE,
    CONSTRAINT FK_KetQuaLamBaiBoCauHoi_BaiHoc
        FOREIGN KEY (MaBaiHoc) REFERENCES BaiHoc(MaBaiHoc) ON DELETE NO ACTION
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
    SoCauHoiNhanDien INT NULL DEFAULT 0,
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
CREATE UNIQUE INDEX UX_CauHoi_MaDinhDanhSeed ON CauHoi(MaDinhDanhSeed) WHERE MaDinhDanhSeed IS NOT NULL;
CREATE INDEX IX_LuotLamBai_NguoiDung_Ngay ON LuotLamBai(MaNguoiDung, ThoiDiemBatDau);
CREATE INDEX IX_LichSuHocTap_NguoiDung_Ngay ON LichSuHocTap(MaNguoiDung, NgayGhiNhan);
CREATE INDEX IX_PhienHoiThoaiAI_NguoiDung_Ngay ON PhienHoiThoaiAI(MaNguoiDung, NgayTao);
CREATE INDEX IX_NhatKyGoiAPI_NgayGhiNhan ON NhatKyGoiAPI(NgayGhiNhan);
CREATE NONCLUSTERED INDEX IX_NoiDungHocTapAIBaiHoc_Hash ON NoiDungHocTapAIBaiHoc(MaBaiHoc, ContentHash, TrangThai);
CREATE NONCLUSTERED INDEX IX_BoCauHoi_BaiHoc ON BoCauHoi(MaBaiHoc, LoaiBoCauHoi, TrangThai);
CREATE NONCLUSTERED INDEX IX_KetQuaLamBaiBoCauHoi_User_Lesson ON KetQuaLamBaiBoCauHoi(MaNguoiDung, MaBaiHoc, LoaiBoCauHoi, ContentHash);
CREATE NONCLUSTERED INDEX IX_BaiHocAudioTranscript_MaBaiHoc ON BaiHocAudioTranscript (MaBaiHoc ASC, TrangThaiDuyet ASC);
CREATE NONCLUSTERED INDEX IX_BaiHocAudioTranscript_AudioHash ON BaiHocAudioTranscript (AudioHash ASC);
GO

/* =========================================================
   11. VIEW TỔNG HỢP VÀ TƯƠNG THÍCH
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

CREATE VIEW KetQuaThi 
AS 
SELECT 
    MaLuotLamBai AS MaKetQuaThi, 
    MaNguoiDung, 
    MaBaiTapDeThi, 
    HinhThucLam, 
    ThoiDiemBatDau, 
    ThoiDiemNopBai, 
    ThoiGianLamGiay, 
    TongSoCau, 
    SoCauDung, 
    SoCauSai, 
    DiemSo, 
    TrangThai 
FROM LuotLamBai;
GO

CREATE VIEW ChiTietKetQuaThi 
AS 
SELECT 
    MaChiTietTraLoi AS MaChiTietKetQuaThi, 
    MaLuotLamBai AS MaKetQuaThi, 
    MaCauHoi, 
    MaLuaChonDaChon, 
    CauTraLoiVanBan, 
    KetQuaDungSai, 
    DiemSo, 
    AIGiaiThichChiTiet, 
    ThoiDiemTraLoi 
FROM ChiTietTraLoi;
GO

/* =========================================================
   12. DỮ LIỆU CƠ BẢN / DANH MỤC HỆ THỐNG BAN ĐẦU
   ========================================================= */


PRINT N'===================================================================';
PRINT N'TẠO HOÀN TẤT SCHEMA CƠ SỞ DỮ LIỆU TiengAnhAI (39 BẢNG, RÀNG BUỘC & VIEWS)!';
PRINT N'===================================================================';
GO
