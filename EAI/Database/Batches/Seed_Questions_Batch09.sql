-- ====================================================================================
-- SEED QUESTIONS BATCH: Batch 09 (SEED_EAI_401 -> SEED_EAI_450)
-- Số lượng câu hỏi: 50
-- Đảm bảo: Idempotent (Chạy lại nhiều lần không trùng lặp)
-- ====================================================================================

USE TiengAnhAI;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT N'Đang nạp Batch 09 (SEED_EAI_401 -> SEED_EAI_450) (50 câu hỏi)...';

-- Câu hỏi SEED_EAI_401 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_401')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'When a patient presents with sudden dizziness, shortness of breath, and palpitations, which hospital department should they be directed to immediately?',
            NULL, NULL, N'Bệnh nhân có triệu chứng khó thở đột ngột, chóng mặt và đánh trống ngực cần được đưa ngay đến khoa cấp cứu (Emergency Department / A&E - Accident & Emergency).', N'Bệnh nhân có triệu chứng khó thở đột ngột, chóng mặt và đánh trống ngực cần được đưa ngay đến khoa cấp cứu (Emergency Department / A&E - Accident & Emergency).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Phần I: Triage và phân loại tiếp nhận bệnh nhân khẩn cấp.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_401');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Dermatology Clinic', 0),
        (@MaCH, N'B', N'Emergency Department (A&E)', 1),
        (@MaCH, N'C', N'Orthopedic Outpatient Desk', 0),
        (@MaCH, N'D', N'Ophthalmology Consultation', 0);
END
GO

-- Câu hỏi SEED_EAI_402 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_402')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''The doctor explained that an _______ illness develops rapidly and lasts a short period, whereas a chronic condition persists over months or years.''',
            NULL, NULL, N'''acute'' (cấp tính) đối lập với ''chronic'' (mãn tính).', N'''acute'' (cấp tính) đối lập với ''chronic'' (mãn tính).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Thuật ngữ lâm sàng: Acute vs. Chronic conditions.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_402');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'acute', 1),
        (@MaCH, N'B', N'accustomed', 0),
        (@MaCH, N'C', N'acute-free', 0),
        (@MaCH, N'D', N'acquired', 0);
END
GO

-- Câu hỏi SEED_EAI_403 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_403')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'TuVung', N'Kho', N'In clinical pharmacotherapy, what does the term ''adverse drug reaction'' (ADR) precisely refer to?',
            NULL, NULL, N'''Adverse drug reaction'' là phản ứng có hại, ngoài ý muốn xảy ra ở liều dùng thông thường của thuốc.', N'''Adverse drug reaction'' là phản ứng có hại, ngoài ý muốn xảy ra ở liều dùng thông thường của thuốc.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Phần II: Dược lý và hướng dẫn sử dụng đơn thuốc (Prescription & Posology).', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_403');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The intended therapeutic efficacy of the antibiotic', 0),
        (@MaCH, N'B', N'An unintended, harmful reaction resulting from medication at standard doses', 1),
        (@MaCH, N'C', N'The physiological shelf-life expiration date of a tablet', 0),
        (@MaCH, N'D', N'The commercial promotion of over-the-counter medicine', 0);
END
GO

-- Câu hỏi SEED_EAI_404 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_404')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'DungSai', N'Doc', N'TrungBinh', N'An ''outpatient'' is someone who stays overnight or for weeks in a hospital ward to receive round-the-clock intensive nursing.',
            NULL, NULL, N'Sai (False). ''Outpatient'' là bệnh nhân ngoại trú (khám và về trong ngày). Bệnh nhân lưu trú qua đêm hoặc điều trị nội trú được gọi là ''inpatient''.', N'Sai (False). ''Outpatient'' là bệnh nhân ngoại trú (khám và về trong ngày). Bệnh nhân lưu trú qua đêm hoặc điều trị nội trú được gọi là ''inpatient''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Phân biệt Inpatient và Outpatient trong quản trị bệnh viện.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_404');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_405 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_405')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'Doctor: ''How long have you been experiencing this throbbing pain in your temples?'' — Which answer is the most clinically helpful?',
            NULL, NULL, N'Bệnh nhân cần cung cấp chính xác thời gian khởi phát và tính chất cơn đau (e.g., ''It started three days ago and worsens whenever I look at bright screens'').', N'Bệnh nhân cần cung cấp chính xác thời gian khởi phát và tính chất cơn đau (e.g., ''It started three days ago and worsens whenever I look at bright screens'').', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Kỹ năng hỏi bệnh sử (Anamnesis & Pain assessment).', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_405');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Yes, I like temples and ancient architecture.', 0),
        (@MaCH, N'B', N'It started about three days ago and worsens whenever I look at bright screens.', 1),
        (@MaCH, N'C', N'I bought some aspirin last month.', 0),
        (@MaCH, N'D', N'My cousin had a headache two years ago.', 0);
END
GO

-- Câu hỏi SEED_EAI_406 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_406')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'SuaLoiSai', N'NguPhap', N'Kho', N'Find the underlined part that contains an error: ''The nurse instructed the patient to take two tablets (A) daily, refrain from (B) alcohol, and coming back (C) for a follow-up consultation on Friday (D).''',
            NULL, NULL, N'Lỗi ở (C) ''coming back''. Cần dùng động từ nguyên mẫu ''come back'' để song hành với ''take'' và ''refrain''.', N'Lỗi ở (C) ''coming back''. Cần dùng động từ nguyên mẫu ''come back'' để song hành với ''take'' và ''refrain''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Cấu trúc câu chỉ định điều trị y khoa & Song hành ngữ pháp.', N'Applied', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_406');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'daily', 0),
        (@MaCH, N'B', N'refrain from', 0),
        (@MaCH, N'C', N'coming back', 1),
        (@MaCH, N'D', N'on Friday', 0);
END
GO

-- Câu hỏi SEED_EAI_407 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_407')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'The label on the medicine bottle reads: ''Do not exceed the recommended _______ of 500mg every 8 hours.''',
            NULL, NULL, N'''dosage'' (liều lượng sử dụng).', N'''dosage'' (liều lượng sử dụng).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Hướng dẫn đọc nhãn dược phẩm (Prescription Labels).', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_407');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'dosage', 1),
        (@MaCH, N'B', N'diet', 0),
        (@MaCH, N'C', N'duration', 0),
        (@MaCH, N'D', N'dimension', 0);
END
GO

-- Câu hỏi SEED_EAI_408 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_408')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What is the primary role of a ''triage nurse'' in an emergency room setting?',
            NULL, NULL, N'Y tá sàng lọc (triage nurse) có nhiệm vụ đánh giá mức độ khẩn cấp về y tế của bệnh nhân khi mới đến để ưu tiên điều trị ca nguy kịch trước.', N'Y tá sàng lọc (triage nurse) có nhiệm vụ đánh giá mức độ khẩn cấp về y tế của bệnh nhân khi mới đến để ưu tiên điều trị ca nguy kịch trước.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Quy trình phân loại bệnh nhân cấp cứu (Triage Protocol).', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_408');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'To calculate hospital accounting invoices', 0),
        (@MaCH, N'B', N'To assess clinical acuity and prioritize patients according to medical urgency', 1),
        (@MaCH, N'C', N'To perform complex neurosurgery operations', 0),
        (@MaCH, N'D', N'To manage hospital parking facilities', 0);
END
GO

-- Câu hỏi SEED_EAI_409 (TuVung - Trình độ 4 - Dạng NoiCap - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_409')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'NoiCap', N'TuVung', N'Kho', N'Match each medical specialist with their diagnostic field:
1. Cardiologist
2. Neurologist
3. Pediatrician
4. Oncologist',
            NULL, NULL, N'1-Heart diseases, 2-Brain & nervous system, 3-Children medical care, 4-Cancer treatment.', N'1-Heart diseases, 2-Brain & nervous system, 3-Children medical care, 4-Cancer treatment.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Danh mục các chuyên khoa y tế bệnh viện.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_409');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'1-Heart, 2-Nervous system, 3-Children, 4-Cancer', 1),
        (@MaCH, N'B', N'1-Cancer, 2-Heart, 3-Nervous system, 4-Children', 0),
        (@MaCH, N'C', N'1-Skin, 2-Bones, 3-Eyes, 4-Teeth', 0),
        (@MaCH, N'D', N'1-Teeth, 2-Skin, 3-Cancer, 4-Heart', 0);
END
GO

-- Câu hỏi SEED_EAI_410 (NguPhap - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_410')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'VietLaiCau', N'NguPhap', N'Kho', N'Rewrite the sentence using passive voice with modal verbs: ''The pharmacist must inspect all prescriptions before dispensing narcotics.''',
            NULL, NULL, N'Dạng bị động: ''All prescriptions must be inspected by the pharmacist before narcotics are dispensed.''', N'Dạng bị động: ''All prescriptions must be inspected by the pharmacist before narcotics are dispensed.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Quy định kiểm soát dược phẩm & Câu bị động phương thái.', N'Applied', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_410');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'All prescriptions must be inspected by the pharmacist before narcotics are dispensed.', 1),
        (@MaCH, N'B', N'Narcotics must inspect all prescriptions by the pharmacist.', 0),
        (@MaCH, N'C', N'The pharmacist is inspecting all prescriptions before dispensing.', 0),
        (@MaCH, N'D', N'All prescriptions has to be inspected by narcotics.', 0);
END
GO

-- Câu hỏi SEED_EAI_411 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_411')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'Which term describes a non-invasive diagnostic technique that uses high-frequency sound waves to visualize internal body organs?',
            NULL, NULL, N'Siêu âm (Ultrasound / Ultrasonography) sử dụng sóng âm tần số cao.', N'Siêu âm (Ultrasound / Ultrasonography) sử dụng sóng âm tần số cao.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Chẩn đoán hình ảnh: X-ray, MRI, CT Scan, Ultrasound.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_411');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Magnetic Resonance Imaging (MRI)', 0),
        (@MaCH, N'B', N'Ultrasound scan', 1),
        (@MaCH, N'C', N'Computerized Tomography (CT)', 0),
        (@MaCH, N'D', N'Positron Emission Tomography', 0);
END
GO

-- Câu hỏi SEED_EAI_412 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_412')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (17, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'The surgeon asked the scrub nurse for a sterile _______ to make the initial abdominal incision.',
            NULL, NULL, N'''scalpel'' (dao mổ phẫu thuật).', N'''scalpel'' (dao mổ phẫu thuật).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Dụng cụ phẫu thuật cơ bản trong phòng mổ.', N'Extracted', N'05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx', NULL, 'SEED_EAI_412');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'scalpel', 1),
        (@MaCH, N'B', N'stethoscope', 0),
        (@MaCH, N'C', N'thermometer', 0),
        (@MaCH, N'D', N'bandage', 0);
END
GO

-- Câu hỏi SEED_EAI_413 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_413')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'In a US F-1 visa interview, what is the legal presumption that every applicant must overcome under Section 214(b) of the INA?',
            NULL, NULL, N'Theo điều 214(b) Đạo luật Di trú Mỹ (INA), mọi người nộp đơn xin visa không định cư đều bị mặc định coi là có ý định nhập cư (immigrant intent) cho đến khi chứng minh được điều ngược lại.', N'Theo điều 214(b) Đạo luật Di trú Mỹ (INA), mọi người nộp đơn xin visa không định cư đều bị mặc định coi là có ý định nhập cư (immigrant intent) cho đến khi chứng minh được điều ngược lại.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 1: Nguyên tắc vàng và điều khoản pháp lý 214(b).', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_413');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'The presumption that the applicant has poor English skills', 0),
        (@MaCH, N'B', N'The presumption that the applicant intends to immigrate permanently to the US', 1),
        (@MaCH, N'C', N'The presumption that tuition fees cannot be paid', 0),
        (@MaCH, N'D', N'The presumption that universities in the US are overpopulated', 0);
END
GO

-- Câu hỏi SEED_EAI_414 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_414')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'Consular Officer: ''Why did you choose this specific university in Ohio instead of others?'' — Which response demonstrates strong preparation?',
            NULL, NULL, N'Ứng viên cần giải thích cụ thể về chương trình học, giáo sư, phòng nghiên cứu hoặc thế mạnh chuyên môn của trường thay vì nói chung chung.', N'Ứng viên cần giải thích cụ thể về chương trình học, giáo sư, phòng nghiên cứu hoặc thế mạnh chuyên môn của trường thay vì nói chung chung.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 2: Trả lời câu hỏi học thuật và lý do chọn trường.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_414');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Because my best friend is studying there and we want to share an apartment.', 0),
        (@MaCH, N'B', N'Their MS program in Data Science offers a specialized curriculum in NLP under Prof. Smith, which directly aligns with my AI career goals in Vietnam.', 1),
        (@MaCH, N'C', N'Because Ohio has nice weather and cheap shopping malls.', 0),
        (@MaCH, N'D', N'My study abroad agency picked it for me randomly.', 0);
END
GO

-- Câu hỏi SEED_EAI_415 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_415')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Fill in the blank: ''To demonstrate non-immigrant intent, applicants must prove strong _______ ties to their home country, such as familial duties, business ownership, or guaranteed job offers.''',
            NULL, NULL, N'''ties'' / ''social and economic ties'' (mối ràng buộc vững chắc về mặt kinh tế, xã hội và gia đình tại quê hương).', N'''ties'' / ''social and economic ties'' (mối ràng buộc vững chắc về mặt kinh tế, xã hội và gia đình tại quê hương).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 3: Chứng minh mối ràng buộc chặt chẽ tại Việt Nam (Strong Ties).', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_415');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'ties', 1),
        (@MaCH, N'B', N'borders', 0),
        (@MaCH, N'C', N'passports', 0),
        (@MaCH, N'D', N'tickets', 0);
END
GO

-- Câu hỏi SEED_EAI_416 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_416')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DungSai', N'Doc', N'De', N'Form I-20 is issued by the US Embassy in Hanoi after an applicant successfully passes the in-person consular interview.',
            NULL, NULL, N'Sai (False). Mẫu I-20 do chính trường đại học được SEVP chứng nhận cấp cho sinh viên sau khi trúng tuyển; sinh viên mang I-20 đến phỏng vấn tại Đại sứ quán / Lãnh sự quán.', N'Sai (False). Mẫu I-20 do chính trường đại học được SEVP chứng nhận cấp cho sinh viên sau khi trúng tuyển; sinh viên mang I-20 đến phỏng vấn tại Đại sứ quán / Lãnh sự quán.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 4: Giấy tờ hồ sơ: Phân biệt Form I-20, Form DS-160 và SEVIS Fee I-901.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_416');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_417 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_417')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'When asked: ''Who is funding your education and what does your sponsor do?'', what is the best strategy?',
            NULL, NULL, N'Cần nêu rõ danh tính người tài trợ (thường là cha mẹ hoặc học bổng), nghề nghiệp cụ thể và thu nhập ròng ổn định hàng tháng/năm có khả năng chi trả học phí.', N'Cần nêu rõ danh tính người tài trợ (thường là cha mẹ hoặc học bổng), nghề nghiệp cụ thể và thu nhập ròng ổn định hàng tháng/năm có khả năng chi trả học phí.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 5: Chiến lược trả lời năng lực tài chính (Financial Solvency).', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_417');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I will find a part-time job under the table in American supermarkets to pay for my school.', 0),
        (@MaCH, N'B', N'My parents are funding my studies; my father owns a certified logistics enterprise earning \$60,000 annually, backed by savings accounts.', 1),
        (@MaCH, N'C', N'I have rich relatives in California who promised to help if I run out of cash.', 0),
        (@MaCH, N'D', N'Money does not matter because education should be free.', 0);
END
GO

-- Câu hỏi SEED_EAI_418 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_418')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'SuaLoiSai', N'NguPhap', N'Kho', N'Identify the grammatical mistake: ''Unless you do not prove (A) substantial ties to your home country, the consular officer (B) will be forced (C) to reject your application under Section 214(b) (D).''',
            NULL, NULL, N'Lỗi ở (A) ''do not prove''. ''Unless'' mang nghĩa ''nếu không'', không dùng thể phủ định kép ''Unless you do not prove''. Phải sửa thành ''Unless you prove''.', N'Lỗi ở (A) ''do not prove''. ''Unless'' mang nghĩa ''nếu không'', không dùng thể phủ định kép ''Unless you do not prove''. Phải sửa thành ''Unless you prove''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Cấu trúc điều kiện với Unless trong ngữ cảnh visa.', N'Applied', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_418');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'do not prove', 1),
        (@MaCH, N'B', N'the consular officer', 0),
        (@MaCH, N'C', N'will be forced', 0),
        (@MaCH, N'D', N'under Section 214(b)', 0);
END
GO

-- Câu hỏi SEED_EAI_419 (Doc - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_419')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DienKhuyet', N'Doc', N'TrungBinh', N'The mandatory tracking system operated by DHS to monitor international students in the United States is known by the acronym _______.',
            NULL, NULL, N'''SEVIS'' (Student and Exchange Visitor Information System).', N'''SEVIS'' (Student and Exchange Visitor Information System).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 4: Khái niệm SEVIS và cách nộp phí I-901 trực tuyến.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_419');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'SEVIS', 1),
        (@MaCH, N'B', N'TOEFL', 0),
        (@MaCH, N'C', N'ESTA', 0),
        (@MaCH, N'D', N'GRE', 0);
END
GO

-- Câu hỏi SEED_EAI_420 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_420')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'What behavior during an interview can inadvertently cause suspicion from consular officers?',
            NULL, NULL, N'Học thuộc lòng như vẹt (rote memorization), mắt nhìn đảo liên tục, hoặc né tránh giao tiếp bằng mắt (avoiding eye contact).', N'Học thuộc lòng như vẹt (rote memorization), mắt nhìn đảo liên tục, hoặc né tránh giao tiếp bằng mắt (avoiding eye contact).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 6: Ngôn ngữ cơ thể và tâm lý phòng phỏng vấn.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_420');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Maintaining steady and polite eye contact with the interviewer', 0),
        (@MaCH, N'B', N'Reciting memorized answers like a robot without emotional connection or spontaneity', 1),
        (@MaCH, N'C', N'Dressing in clean, professional business attire', 0),
        (@MaCH, N'D', N'Greeting the officer clearly with ''Good morning''', 0);
END
GO

-- Câu hỏi SEED_EAI_421 (NguPhap - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_421')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'VietLaiCau', N'NguPhap', N'Kho', N'Rewrite the sentence beginning with ''Only by'': ''You can convince the visa officer of your bona fide student intentions only by answering concisely and truthfully.''',
            NULL, NULL, N'Đảo ngữ với ''Only by'': ''Only by answering concisely and truthfully can you convince the visa officer of your bona fide student intentions.''', N'Đảo ngữ với ''Only by'': ''Only by answering concisely and truthfully can you convince the visa officer of your bona fide student intentions.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Kỹ thuật phỏng vấn & Đảo ngữ nhấn mạnh.', N'Applied', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_421');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Only by answering concisely and truthfully can you convince the visa officer of your bona fide student intentions.', 1),
        (@MaCH, N'B', N'Only by answering concisely and truthfully you can convince the visa officer.', 0),
        (@MaCH, N'C', N'Only by you convince the visa officer answering concisely and truthfully.', 0),
        (@MaCH, N'D', N'Only by answering truthfully the visa officer can convince you.', 0);
END
GO

-- Câu hỏi SEED_EAI_422 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_422')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What does the term ''Bona fide student'' mean in the context of US visa adjudication?',
            NULL, NULL, N'''Bona fide student'' là sinh viên chân chính, có mục tiêu học tập thực sự, năng lực học vấn phù hợp và kế hoạch tài chính rõ ràng.', N'''Bona fide student'' là sinh viên chân chính, có mục tiêu học tập thực sự, năng lực học vấn phù hợp và kế hoạch tài chính rõ ràng.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Định nghĩa pháp lý Bona Fide Student.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_422');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A student who holds high government scholarships', 0),
        (@MaCH, N'B', N'A genuine student with authentic educational intent and academic capability', 1),
        (@MaCH, N'C', N'An international student who works full-time in off-campus restaurants', 0),
        (@MaCH, N'D', N'A student who has already completed a master''s degree', 0);
END
GO

-- Câu hỏi SEED_EAI_423 (Noi - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_423')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'TracNghiem', N'Noi', N'TrungBinh', N'If asked: ''What will you do after graduating from your university in America?'', what is the most appropriate answer?',
            NULL, NULL, N'Nêu rõ lộ trình trở về nước để phát triển sự nghiệp trong ngành tại các tổ chức, doanh nghiệp ở Việt Nam.', N'Nêu rõ lộ trình trở về nước để phát triển sự nghiệp trong ngành tại các tổ chức, doanh nghiệp ở Việt Nam.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 7: Kế hoạch nghề nghiệp dài hạn sau khi tốt nghiệp (Career Roadmap).', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_423');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I hope to stay in New York forever and marry an American citizen.', 0),
        (@MaCH, N'B', N'I plan to immediately return to Vietnam to join a tech corporation as a Cloud Architect, applying the specialized skills I acquired.', 1),
        (@MaCH, N'C', N'I have not thought about it yet; I will decide after I finish partying.', 0),
        (@MaCH, N'D', N'I will look for any permanent job in California.', 0);
END
GO

-- Câu hỏi SEED_EAI_424 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_424')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'An online nonimmigrant visa electronic application form required for all F-1 applicants is designated as Form _______.',
            NULL, NULL, N'''DS-160'' (Online Nonimmigrant Visa Application).', N'''DS-160'' (Online Nonimmigrant Visa Application).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Hướng dẫn điền đơn DS-160 chuẩn xác từng mục.', N'Extracted', N'06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx', NULL, 'SEED_EAI_424');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'DS-160', 1),
        (@MaCH, N'B', N'W-2', 0),
        (@MaCH, N'C', N'1040-EZ', 0),
        (@MaCH, N'D', N'I-94', 0);
END
GO

-- Câu hỏi SEED_EAI_425 (Viet - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_425')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'TracNghiem', N'Viet', N'Kho', N'What is the primary difference between a Statement of Purpose (SOP) and a Personal Statement (PS) in graduate admissions?',
            NULL, NULL, N'SOP tập trung vào học thuật, định hướng nghiên cứu chuyên sâu, mục tiêu nghề nghiệp và sự phù hợp với chương trình đào tạo; còn PS tập trung hơn vào hành trình cuộc sống cá nhân, thử thách và động lực nội tâm.', N'SOP tập trung vào học thuật, định hướng nghiên cứu chuyên sâu, mục tiêu nghề nghiệp và sự phù hợp với chương trình đào tạo; còn PS tập trung hơn vào hành trình cuộc sống cá nhân, thử thách và động lực nội tâm.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần I: Phân biệt SOP (Học thuật & Nghiên cứu) và Personal Statement.', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_425');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'An SOP is a list of grades while a PS is a resume', 0),
        (@MaCH, N'B', N'An SOP focuses on academic trajectory, research questions, and faculty alignment, whereas a PS highlights personal backstory and resilience', 1),
        (@MaCH, N'C', N'An SOP is written by parents while a PS is written by professors', 0),
        (@MaCH, N'D', N'An SOP is only required for high school students', 0);
END
GO

-- Câu hỏi SEED_EAI_426 (Viet - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_426')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'TracNghiem', N'Viet', N'Kho', N'Which opening hook in a Master''s SOP is strongly DISCOURAGED by top-tier admissions committees?',
            NULL, NULL, N'Bắt đầu bằng câu trích dẫn nổi tiếng sáo rỗng hoặc câu nói mòn tai như ''Since my early childhood, I have always loved science'' bị hội đồng tuyển sinh đánh giá rất thấp.', N'Bắt đầu bằng câu trích dẫn nổi tiếng sáo rỗng hoặc câu nói mòn tai như ''Since my early childhood, I have always loved science'' bị hội đồng tuyển sinh đánh giá rất thấp.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần II: Các câu mở đầu sáo rỗng (Cliches to avoid in Paragraph 1).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_426');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'A specific intellectual problem encountered during undergraduate capstone research', 0),
        (@MaCH, N'B', N'''Ever since I was a five-year-old child, I have always dreamed of becoming an engineer.''', 1),
        (@MaCH, N'C', N'An outline of current industry challenges and the applicant''s concrete research hypothesis', 0),
        (@MaCH, N'D', N'A concise overview of technical expertise and graduate study ambitions', 0);
END
GO

-- Câu hỏi SEED_EAI_427 (Viet - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_427')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'DienKhuyet', N'Viet', N'Kho', N'In a graduate SOP, when discussing fit with the institution, applicants must demonstrate academic _______ by referencing specific faculty research labs and recent publications.',
            NULL, NULL, N'''alignment'' (sự liên kết, tương thích học thuật).', N'''alignment'' (sự liên kết, tương thích học thuật).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần IV: Xây dựng mục Why School & Academic Alignment.', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_427');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'alignment', 1),
        (@MaCH, N'B', N'isolation', 0),
        (@MaCH, N'C', N'distance', 0),
        (@MaCH, N'D', N'indifference', 0);
END
GO

-- Câu hỏi SEED_EAI_428 (Viet - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_428')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'TracNghiem', N'Viet', N'Kho', N'How should academic shortcomings (such as one semester with low GPA) be addressed in an SOP?',
            NULL, NULL, N'Trình bày ngắn gọn, nhận trách nhiệm cá nhân, nêu rõ nguyên nhân khách quan nếu có và nhấn mạnh giải pháp khắc phục cũng như sự tiến bộ vượt bậc sau đó (Upward trend).', N'Trình bày ngắn gọn, nhận trách nhiệm cá nhân, nêu rõ nguyên nhân khách quan nếu có và nhấn mạnh giải pháp khắc phục cũng như sự tiến bộ vượt bậc sau đó (Upward trend).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần V: Cách xử lý điểm yếu học tập (Handling Red Flags and Low GPAs).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_428');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Blame the professors for giving unfair exam questions', 0),
        (@MaCH, N'B', N'Address it briefly, take accountability, highlight lessons learned, and demonstrate a clear upward trajectory in subsequent coursework', 1),
        (@MaCH, N'C', N'Pretend it never happened and alter the official transcript', 0),
        (@MaCH, N'D', N'Write three full pages complaining about bad health without evidence', 0);
END
GO

-- Câu hỏi SEED_EAI_429 (NguPhap - Trình độ 5 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_429')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'SuaLoiSai', N'NguPhap', N'Kho', N'Find the grammatical fault in this SOP sentence: ''Having successfully completing (A) my undergraduate thesis on NLP, my aspiration (B) is to further investigate (C) semantic ambiguity at your laboratory (D).''',
            NULL, NULL, N'Lỗi dùng thì ở phân từ hoàn thành (A): phải là ''Having successfully completed'', không dùng V-ing ''completing'' sau ''having''.', N'Lỗi dùng thì ở phân từ hoàn thành (A): phải là ''Having successfully completed'', không dùng V-ing ''completing'' sau ''having''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Ngữ pháp viết học thuật cao cấp: Phân từ hoàn thành (Perfect Participle).', N'Applied', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_429');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Having successfully completing', 1),
        (@MaCH, N'B', N'my aspiration', 0),
        (@MaCH, N'C', N'further investigate', 0),
        (@MaCH, N'D', N'at your laboratory', 0);
END
GO

-- Câu hỏi SEED_EAI_430 (Viet - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_430')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'TracNghiem', N'Viet', N'Kho', N'What is the recommended word count range for a competitive Master''s SOP at US/UK universities?',
            NULL, NULL, N'Độ dài tiêu chuẩn của một bài SOP Thạc sĩ là khoảng 800 - 1.000 từ (khoảng 2 trang A4 giãn dòng đơn).', N'Độ dài tiêu chuẩn của một bài SOP Thạc sĩ là khoảng 800 - 1.000 từ (khoảng 2 trang A4 giãn dòng đơn).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần VI: Quy cách định dạng, font chữ và dung lượng từ (800-1000 words).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_430');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'100 - 200 words', 0),
        (@MaCH, N'B', N'800 - 1,000 words (approximately 2 single-spaced pages)', 1),
        (@MaCH, N'C', N'4,000 - 6,000 words', 0),
        (@MaCH, N'D', N'At least 15 pages', 0);
END
GO

-- Câu hỏi SEED_EAI_431 (Viet - Trình độ 5 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_431')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'VietLaiCau', N'Viet', N'Kho', N'Enhance the academic sophistication of: ''I want to study in your university because your labs are very famous.''',
            NULL, NULL, N'''I am keen to join the Department of Computer Science to collaborate with Dr. X whose seminal work on neural transformers resonates deeply with my research interests.''', N'''I am keen to join the Department of Computer Science to collaborate with Dr. X whose seminal work on neural transformers resonates deeply with my research interests.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Kỹ thuật nâng cấp văn phong học thuật (Elevating academic tone).', N'Applied', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_431');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'I am eager to join your department to collaborate with Dr. X, whose seminal work on neural transformers directly aligns with my research objectives.', 1),
        (@MaCH, N'B', N'I want to enter your lab because everyone knows it is very popular.', 0),
        (@MaCH, N'C', N'Your university is famous and I like fame.', 0),
        (@MaCH, N'D', N'Because your lab is cool, I will come.', 0);
END
GO

-- Câu hỏi SEED_EAI_432 (Viet - Trình độ 5 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_432')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'DungSai', N'Viet', N'TrungBinh', N'True or False: Using the identical generic SOP for ten different universities without mentioning faculty names or program specifics is considered best practice.',
            NULL, NULL, N'Sai (False). Việc nộp cùng 1 bài SOP chung chung (generic copy-paste) là nguyên nhân hàng đầu khiến hồ sơ bị loại ở vòng thẩm định đầu tiên.', N'Sai (False). Việc nộp cùng 1 bài SOP chung chung (generic copy-paste) là nguyên nhân hàng đầu khiến hồ sơ bị loại ở vòng thẩm định đầu tiên.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần VII: Nguyên tắc tùy biến riêng biệt cho từng trường (Customization rule).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_432');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_433 (Viet - Trình độ 5 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_433')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'DienKhuyet', N'Viet', N'Kho', N'Fill in the blank: ''Applicants should demonstrate intellectual _______ by formulating concise research questions and hypotheses for future graduate work.''',
            NULL, NULL, N'''curiosity'' hoặc ''maturity'' (sự trưởng thành và tò mò trí tuệ).', N'''curiosity'' hoặc ''maturity'' (sự trưởng thành và tò mò trí tuệ).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phẩm chất học thuật cốt lõi (Intellectual Curiosity).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_433');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'curiosity', 1),
        (@MaCH, N'B', N'negligence', 0),
        (@MaCH, N'C', N'arrogance', 0),
        (@MaCH, N'D', N'exhaustion', 0);
END
GO

-- Câu hỏi SEED_EAI_434 (Viet - Trình độ 5 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_434')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (18, 5, N'TracNghiem', N'Viet', N'Kho', N'Which concluding statement effectively synthesizes the applicant''s readiness in an SOP?',
            NULL, NULL, N'Câu kết luận cần khẳng định sự sẵn sàng đóng góp cho cộng đồng học thuật của khoa và tiềm năng phát triển nghề nghiệp lâu dài.', N'Câu kết luận cần khẳng định sự sẵn sàng đóng góp cho cộng đồng học thuật của khoa và tiềm năng phát triển nghề nghiệp lâu dài.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần VIII: Cách viết đoạn kết ấn tượng (Strong Conclusion).', N'Extracted', N'07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx', NULL, 'SEED_EAI_434');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'''Please admit me because I have nowhere else to go.''', 0),
        (@MaCH, N'B', N'''With my rigorous foundation in software engineering and passion for AI safety, I am prepared to contribute meaningfully to your research community and excel in your master''s cohort.''', 1),
        (@MaCH, N'C', N'''I hope you like this essay and give me a full scholarship.''', 0),
        (@MaCH, N'D', N'''That is all I have to say about myself.''', 0);
END
GO

-- Câu hỏi SEED_EAI_435 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_435')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'According to Lesson 33, which renewable energy source harnesses heat stored beneath the Earth''s surface?',
            NULL, NULL, N'Năng lượng địa nhiệt (Geothermal energy) khai thác nhiệt năng tích tụ sâu trong lòng vỏ trái đất.', N'Năng lượng địa nhiệt (Geothermal energy) khai thác nhiệt năng tích tụ sâu trong lòng vỏ trái đất.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Từ vựng chuyên sâu: Môi trường, Biến đổi khí hậu & Năng lượng tái tạo - Phần: Năng lượng tái tạo (Renewable Energy Sources).', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_435');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Hydroelectric energy', 0),
        (@MaCH, N'B', N'Geothermal energy', 1),
        (@MaCH, N'C', N'Solar photovoltaic power', 0),
        (@MaCH, N'D', N'Biomass combustion', 0);
END
GO

-- Câu hỏi SEED_EAI_436 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_436')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Complete the sentence: ''The total amount of greenhouse gases generated by human actions and manufacturing is referred to as our carbon _______.''',
            NULL, NULL, N'''carbon footprint'' (dấu chân carbon).', N'''carbon footprint'' (dấu chân carbon).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Từ vựng chuyên sâu: Môi trường - Khái niệm Dấu chân Carbon (Carbon Footprint).', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_436');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'footprint', 1),
        (@MaCH, N'B', N'handprint', 0),
        (@MaCH, N'C', N'shoe', 0),
        (@MaCH, N'D', N'fingerprint', 0);
END
GO

-- Câu hỏi SEED_EAI_437 (Doc - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_437')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'TracNghiem', N'Doc', N'TrungBinh', N'What environmental disaster is directly exacerbated by rapid deforestation in mountainous areas?',
            NULL, NULL, N'Mất rừng ở vùng đồi núi làm giảm độ liên kết đất, trực tiếp gây ra sạt lở đất nghiêm trọng (severe soil erosion and landslides).', N'Mất rừng ở vùng đồi núi làm giảm độ liên kết đất, trực tiếp gây ra sạt lở đất nghiêm trọng (severe soil erosion and landslides).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Tác động của nạn phá rừng (Deforestation & Soil Degradation).', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_437');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Severe soil erosion and catastrophic landslides', 1),
        (@MaCH, N'B', N'Increase in ozone layer thickness', 0),
        (@MaCH, N'C', N'Reduction of ocean microplastics', 0),
        (@MaCH, N'D', N'Accelerated glacier formation', 0);
END
GO

-- Câu hỏi SEED_EAI_438 (Doc - Trình độ 4 - Dạng DungSai - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_438')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'DungSai', N'Doc', N'De', N'True or False: Biodegradable packaging materials persist in oceans for thousands of years without decomposing.',
            NULL, NULL, N'Sai (False). Vật liệu phân hủy sinh học (biodegradable) có khả năng phân rã tự nhiên nhờ vi sinh vật thành các chất vô hại trong thời gian tương đối ngắn.', N'Sai (False). Vật liệu phân hủy sinh học (biodegradable) có khả năng phân rã tự nhiên nhờ vi sinh vật thành các chất vô hại trong thời gian tương đối ngắn.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Khái niệm rác thải sinh thái và vật liệu phân hủy sinh học (Biodegradable Materials).', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_438');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'True', 0),
        (@MaCH, N'B', N'False', 1);
END
GO

-- Câu hỏi SEED_EAI_439 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_439')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'DienKhuyet', N'TuVung', N'Kho', N'International climate treaties mandate countries to reach net-_______ carbon emissions by 2050.',
            NULL, NULL, N'''net-zero'' (phát thải ròng bằng không).', N'''net-zero'' (phát thải ròng bằng không).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Cam kết quốc tế về khí hậu và mục tiêu Net-Zero Emissions.', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_439');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'zero', 1),
        (@MaCH, N'B', N'fifty', 0),
        (@MaCH, N'C', N'equal', 0),
        (@MaCH, N'D', N'half', 0);
END
GO

-- Câu hỏi SEED_EAI_440 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_440')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'TracNghiem', N'TuVung', N'Kho', N'Which term describes the process of capturing carbon dioxide from industrial exhausts and trapping it deep underground?',
            NULL, NULL, N'Carbon Capture and Storage (CCS - Thu giữ và chôn lấp carbon).', N'Carbon Capture and Storage (CCS - Thu giữ và chôn lấp carbon).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Công nghệ giảm phát thải: Carbon Capture and Storage (CCS).', N'Extracted', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_440');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Carbon Capture and Storage (CCS)', 1),
        (@MaCH, N'B', N'Deforestation mechanism', 0),
        (@MaCH, N'C', N'Greenhouse acceleration', 0),
        (@MaCH, N'D', N'Thermal inversion', 0);
END
GO

-- Câu hỏi SEED_EAI_441 (NguPhap - Trình độ 4 - Dạng SuaLoiSai - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_441')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'SuaLoiSai', N'NguPhap', N'Kho', N'Spot the error: ''If all industrialized nations were to transition (A) to solar energy, greenhouse gas emissions would have decreased (B) significantly within (C) the upcoming decade (D).''',
            NULL, NULL, N'Lỗi ở (B) ''would have decreased''. Mệnh đề ''If were to transition'' (điều kiện loại 2) chỉ tình huống giả định ở hiện tại/tương lai, nên mệnh đề chính phải là ''would decrease'', không dùng ''would have decreased''.', N'Lỗi ở (B) ''would have decreased''. Mệnh đề ''If were to transition'' (điều kiện loại 2) chỉ tình huống giả định ở hiện tại/tương lai, nên mệnh đề chính phải là ''would decrease'', không dùng ''would have decreased''.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Câu điều kiện loại 2 giả định chính sách môi trường.', N'Applied', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_441');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'were to transition', 0),
        (@MaCH, N'B', N'would have decreased', 1),
        (@MaCH, N'C', N'within', 0),
        (@MaCH, N'D', N'the upcoming decade', 0);
END
GO

-- Câu hỏi SEED_EAI_442 (NguPhap - Trình độ 4 - Dạng VietLaiCau - Applied)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_442')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (13, 4, N'VietLaiCau', N'NguPhap', N'Kho', N'Transform using inversion: ''If governments had invested heavily in clean wind power ten years ago, air quality would be vastly superior today.''',
            NULL, NULL, N'Đảo ngữ điều kiện hỗn hợp loại 3 & 2: ''Had governments invested heavily in clean wind power ten years ago, air quality would be vastly superior today.''', N'Đảo ngữ điều kiện hỗn hợp loại 3 & 2: ''Had governments invested heavily in clean wind power ten years ago, air quality would be vastly superior today.''', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 33: Ngữ pháp đảo ngữ điều kiện hỗn hợp chủ đề biến đổi khí hậu.', N'Applied', N'BaiHoc_33_Moi_truong_va_Nang_luong.md', 33, 'SEED_EAI_442');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Had governments invested heavily in clean wind power ten years ago, air quality would be vastly superior today.', 1),
        (@MaCH, N'B', N'Were governments invested heavily, air quality will be vastly superior.', 0),
        (@MaCH, N'C', N'Should governments invest heavily, air quality would be vastly superior.', 0),
        (@MaCH, N'D', N'If had governments invested, air quality would be vastly superior.', 0);
END
GO

-- Câu hỏi SEED_EAI_443 (TuVung - Trình độ 3 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_443')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 3, N'TracNghiem', N'TuVung', N'TrungBinh', N'According to Lesson 34, which musical term refers to the speed or pace of a musical piece?',
            NULL, NULL, N'''Tempo'' là thuật ngữ chỉ tốc độ hay nhịp độ diễn tấu của một bản nhạc.', N'''Tempo'' là thuật ngữ chỉ tốc độ hay nhịp độ diễn tấu của một bản nhạc.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 34: Âm nhạc và Nghệ thuật: Diễn đạt cảm xúc, Thể loại và Nhạc cụ - Thuật ngữ âm nhạc cơ bản (Tempo, Pitch, Harmony).', N'Extracted', N'BaiHoc_34_Am_nhac_va_Nghe_thuat.md', 34, 'SEED_EAI_443');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Tempo', 1),
        (@MaCH, N'B', N'Pitch', 0),
        (@MaCH, N'C', N'Lyrics', 0),
        (@MaCH, N'D', N'Chorus', 0);
END
GO

-- Câu hỏi SEED_EAI_444 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_444')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'What is the meaning of the idiom ''music to someone''s ears''?',
            NULL, NULL, N'Thành ngữ ''music to my ears'' chỉ thông tin hoặc tin tức khiến người nghe vô cùng vui mừng, phấn khởi.', N'Thành ngữ ''music to my ears'' chỉ thông tin hoặc tin tức khiến người nghe vô cùng vui mừng, phấn khởi.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Thành ngữ và Cụm từ tiếng Anh về Nghệ thuật và Sáng tạo - Thành ngữ âm nhạc thông dụng.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_444');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Information that is highly pleasing or welcome news to receive', 1),
        (@MaCH, N'B', N'An unpleasant loud ringing noise in the ear', 0),
        (@MaCH, N'C', N'A recommendation for new wireless headphones', 0),
        (@MaCH, N'D', N'A boring lecture on classical violin compositions', 0);
END
GO

-- Câu hỏi SEED_EAI_445 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_445')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'We don''t have a rigid schedule for the weekend project, so let''s just play it by _______.',
            NULL, NULL, N'''play it by ear'' (tùy cơ ứng biến, linh hoạt theo tình hình thực tế).', N'''play it by ear'' (tùy cơ ứng biến, linh hoạt theo tình hình thực tế).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Thành ngữ ''Play it by ear'' trong giao tiếp tiếng Anh thường nhật.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_445');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'ear', 1),
        (@MaCH, N'B', N'eye', 0),
        (@MaCH, N'C', N'mouth', 0),
        (@MaCH, N'D', N'hand', 0);
END
GO

-- Câu hỏi SEED_EAI_446 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_446')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'TuVung', N'Kho', N'When someone has to ''face the music'', what are they forced to do?',
            NULL, NULL, N'''Face the music'' có nghĩa là phải chấp nhận đối mặt với hậu quả hoặc sự chỉ trích từ hành động sai trái của mình.', N'''Face the music'' có nghĩa là phải chấp nhận đối mặt với hậu quả hoặc sự chỉ trích từ hành động sai trái của mình.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Thành ngữ ''Face the music'' và nguồn gốc lịch sử.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_446');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Accept the unpleasant consequences or criticism of one''s actions', 1),
        (@MaCH, N'B', N'Buy front-row concert tickets to an orchestra', 0),
        (@MaCH, N'C', N'Learn how to conduct a choir', 0),
        (@MaCH, N'D', N'Dance to classical waltz rhythms', 0);
END
GO

-- Câu hỏi SEED_EAI_447 (TuVung - Trình độ 4 - Dạng DienKhuyet - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_447')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'DienKhuyet', N'TuVung', N'TrungBinh', N'Nobody likes coworkers who constantly blow their own _______ instead of sharing credit with the team.',
            NULL, NULL, N'''blow one''s own trumpet / horn'' (tự khoe khoang, tự tâng bốc bản thân).', N'''blow one''s own trumpet / horn'' (tự khoe khoang, tự tâng bốc bản thân).', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Cụm từ ''Blow one''s own trumpet'' trong môi trường làm việc chuyên nghiệp.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_447');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'trumpet', 1),
        (@MaCH, N'B', N'flute', 0),
        (@MaCH, N'C', N'guitar', 0),
        (@MaCH, N'D', N'drum', 0);
END
GO

-- Câu hỏi SEED_EAI_448 (TuVung - Trình độ 4 - Dạng TracNghiem - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_448')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TracNghiem', N'TuVung', N'TrungBinh', N'To ''fine-tune'' an algorithm or business process means to:',
            NULL, NULL, N'''fine-tune'' có nghĩa là tinh chỉnh, điều chỉnh chi tiết nhỏ để đạt được hiệu suất tối ưu nhất.', N'''fine-tune'' có nghĩa là tinh chỉnh, điều chỉnh chi tiết nhỏ để đạt được hiệu suất tối ưu nhất.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Từ vựng mượn từ âm nhạc dùng trong công nghệ: ''Fine-tune''.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_448');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Make minor, precise adjustments to achieve the best possible performance', 1),
        (@MaCH, N'B', N'Cancel the entire system due to critical failure', 0),
        (@MaCH, N'C', N'Play background music while coding', 0),
        (@MaCH, N'D', N'Delete all database tables', 0);
END
GO

-- Câu hỏi SEED_EAI_449 (TuVung - Trình độ 4 - Dạng NoiCap - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_449')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'NoiCap', N'TuVung', N'TrungBinh', N'Match each instrument to its acoustic category:
1. Cello
2. Clarinet
3. Trombone
4. Timpani',
            NULL, NULL, N'1-String, 2-Woodwind, 3-Brass, 4-Percussion.', N'1-String, 2-Woodwind, 3-Brass, 4-Percussion.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 34: Phân loại các bộ nhạc cụ trong dàn nhạc giao hưởng (Orchestra Families).', N'Extracted', N'BaiHoc_34_Am_nhac_va_Nghe_thuat.md', 34, 'SEED_EAI_449');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'1-String, 2-Woodwind, 3-Brass, 4-Percussion', 1),
        (@MaCH, N'B', N'1-Brass, 2-String, 3-Percussion, 4-Woodwind', 0),
        (@MaCH, N'C', N'1-Percussion, 2-Brass, 3-String, 4-Woodwind', 0),
        (@MaCH, N'D', N'1-Woodwind, 2-Percussion, 3-Brass, 4-String', 0);
END
GO

-- Câu hỏi SEED_EAI_450 (Doc - Trình độ 4 - Dạng TraLoiNgan - Extracted)
IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = 'SEED_EAI_450')
BEGIN
    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,
                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,
                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)
    VALUES (15, 4, N'TraLoiNgan', N'Doc', N'Kho', N'In art criticism, what French-origin term describes the prevailing mood, atmosphere, or environment of an artistic installation?',
            NULL, NULL, N'''Ambiance'' (hoặc ''Atmosphere'' / ''Milieu'') chỉ bầu không khí, không gian bao quanh của một tác phẩm sắp đặt.', N'''Ambiance'' (hoặc ''Atmosphere'' / ''Milieu'') chỉ bầu không khí, không gian bao quanh của một tác phẩm sắp đặt.', 1,
            1, N'HocLieuChuan', SYSDATETIME(), N'Bài học 35: Thuật ngữ phê bình mỹ thuật và triển lãm không gian đương đại.', N'Extracted', N'BaiHoc_35_Thanh_ngu_Nghe_thuat.md', 35, 'SEED_EAI_450');

    DECLARE @MaCH INT = SCOPE_IDENTITY();
    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
        (@MaCH, N'A', N'Ambiance', 1),
        (@MaCH, N'B', N'Collage', 0),
        (@MaCH, N'C', N'Fresco', 0),
        (@MaCH, N'D', N'Silhouette', 0);
END
GO

PRINT N'Đã hoàn thành nạp Batch 09 (SEED_EAI_401 -> SEED_EAI_450) thành công!';
GO
