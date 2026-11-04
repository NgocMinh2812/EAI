-- ====================================================================================
-- EAI - MIGRATION SPRINT: AUDIO, SPEECH-TO-TEXT & TEXT-TO-SPEECH MANAGEMENT
-- Lưu trữ metadata tài nguyên Audio, Bản chép lời (Transcript), Phân đoạn (Segments)
-- Hỗ trợ cơ chế Cache & Chống trùng lặp API (Idempotency)
-- ====================================================================================

USE TiengAnhAI;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Bắt đầu thực thi Migration: Hệ thống Audio, STT và TTS...';
GO

-- 1. Bảng lưu trữ Audio & Transcript cho Bài học
IF OBJECT_ID('dbo.BaiHocAudioTranscript', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.BaiHocAudioTranscript
    (
        MaAudioTranscript INT IDENTITY(1,1) NOT NULL,
        MaBaiHoc INT NULL,
        AudioPath NVARCHAR(500) NOT NULL,
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

        CONSTRAINT PK_BaiHocAudioTranscript PRIMARY KEY CLUSTERED (MaAudioTranscript ASC),
        CONSTRAINT FK_BaiHocAudioTranscript_BaiHoc FOREIGN KEY (MaBaiHoc) REFERENCES dbo.BaiHoc(MaBaiHoc) ON DELETE SET NULL
    );

    PRINT N'-> Đã tạo bảng BaiHocAudioTranscript thành công.';
END
ELSE
BEGIN
    PRINT N'-> Bảng BaiHocAudioTranscript đã tồn tại, bỏ qua tạo mới.';
END
GO

-- 2. Tạo chỉ mục tối ưu hóa truy vấn cache
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BaiHocAudioTranscript_MaBaiHoc' AND object_id = OBJECT_ID('dbo.BaiHocAudioTranscript'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_BaiHocAudioTranscript_MaBaiHoc 
    ON dbo.BaiHocAudioTranscript (MaBaiHoc ASC, TrangThaiDuyet ASC);
    PRINT N'-> Đã tạo index IX_BaiHocAudioTranscript_MaBaiHoc.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BaiHocAudioTranscript_AudioHash' AND object_id = OBJECT_ID('dbo.BaiHocAudioTranscript'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_BaiHocAudioTranscript_AudioHash 
    ON dbo.BaiHocAudioTranscript (AudioHash ASC);
    PRINT N'-> Đã tạo index IX_BaiHocAudioTranscript_AudioHash.';
END
GO

PRINT N'Hoàn thành Migration Hệ thống Audio, STT và TTS!';
GO

