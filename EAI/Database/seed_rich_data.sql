-- ===================================================================
-- Script: seed_rich_data.sql (Chạy UTF-8 chuẩn xác)
-- ===================================================================
USE TiengAnhAI;
GO

-- 1. Xóa chủ đề rác nếu có
DELETE FROM BaiHoc WHERE MaChuDe = 16;
DELETE FROM ChuDe WHERE MaChuDe = 16 OR TenChuDe LIKE N'%Test%';

-- Lấy giảng viên tạo bài
DECLARE @gvId INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro = N'GiangVien'));
IF @gvId IS NULL SET @gvId = 1;

-- Cập nhật tên chuẩn cho các chủ đề nếu bị lỗi font trước đó
UPDATE ChuDe SET TenChuDe = N'Tiếng Anh Giao Tiếp' WHERE MaChuDe = 1;
UPDATE ChuDe SET TenChuDe = N'Chào hỏi & Giới thiệu bản thân' WHERE MaChuDe = 2;
UPDATE ChuDe SET TenChuDe = N'Mua sắm & Nhà hàng ẩm thực' WHERE MaChuDe = 3;
UPDATE ChuDe SET TenChuDe = N'Du lịch & Phương tiện giao thông' WHERE MaChuDe = 4;
UPDATE ChuDe SET TenChuDe = N'Tiếng Anh Học Thuật (IELTS)' WHERE MaChuDe = 5;
UPDATE ChuDe SET TenChuDe = N'IELTS Reading - Skimming & Scanning' WHERE MaChuDe = 6;
UPDATE ChuDe SET TenChuDe = N'IELTS Writing Task 2 - Luận xã hội' WHERE MaChuDe = 7;
UPDATE ChuDe SET TenChuDe = N'IELTS Listening - Section 3 & 4' WHERE MaChuDe = 8;
UPDATE ChuDe SET TenChuDe = N'Ngữ Pháp Cốt Lõi' WHERE MaChuDe = 9;
UPDATE ChuDe SET TenChuDe = N'Các thì trong tiếng Anh (Tenses)' WHERE MaChuDe = 10;
UPDATE ChuDe SET TenChuDe = N'Mệnh đề quan hệ & Câu bị động' WHERE MaChuDe = 11;
UPDATE ChuDe SET TenChuDe = N'Câu điều kiện & Đảo ngữ' WHERE MaChuDe = 12;
UPDATE ChuDe SET TenChuDe = N'Tiếng Anh Thương Mại & Công Sở' WHERE MaChuDe = 13;
UPDATE ChuDe SET TenChuDe = N'Viết Email & Thư tín Thương mại' WHERE MaChuDe = 14;
UPDATE ChuDe SET TenChuDe = N'Đàm phán & Thuyết trình Dự án' WHERE MaChuDe = 15;

IF EXISTS (SELECT 1 FROM ChuDe WHERE MaChuDe = 18)
    UPDATE ChuDe SET TenChuDe = N'Tiếng Anh Công Nghệ Thông Tin (IT English)' WHERE MaChuDe = 18;
IF EXISTS (SELECT 1 FROM ChuDe WHERE MaChuDe = 19)
    UPDATE ChuDe SET TenChuDe = N'Thuật ngữ Lập trình & Kỹ thuật Phần mềm' WHERE MaChuDe = 19;
IF EXISTS (SELECT 1 FROM ChuDe WHERE MaChuDe = 20)
    UPDATE ChuDe SET TenChuDe = N'Giao tiếp Agile, Daily Scrum & Phỏng vấn IT' WHERE MaChuDe = 20;

-- 2. Bổ sung bài học phong phú vào từng chuyên đề con theo MaChuDe cụ thể

-- -------------------------------------------------------------
-- [MaChuDe = 2]: Chào hỏi & Giới thiệu bản thân
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 2 AND TieuDe LIKE N'%Hội thoại Chào hỏi%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (2, 2, @gvId, N'Hội thoại Chào hỏi Trang trọng vs. Thân mật (Formal & Informal)', N'TuVung',
N'Phân biệt cách chào hỏi trong môi trường học thuật/công sở với bạn bè hàng ngày.',
N'# Phân biệt Chào hỏi Trang trọng và Thân mật
1. **Formal Greetings**: "Good morning / afternoon", "How do you do?", "Pleased to meet you".
2. **Informal Greetings**: "Hey, what’s up?", "How’s it going?", "Long time no see!".
3. **Thực hành**: Đọc đoạn hội thoại giữa 2 đồng nghiệp mới gặp ngày đầu làm việc.', 15, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 2 AND TieuDe LIKE N'%Tự tin giới thiệu%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (2, 3, @gvId, N'Tự tin giới thiệu Nghề nghiệp, Quê hương và Đam mê cá nhân', N'Doc',
N'Mẫu câu và cấu trúc đoạn văn ngắn giúp bạn tự tin tỏa sáng khi được hỏi "Tell me about yourself".',
N'# Kỹ thuật Self-Introduction 3 Bước
- **Bước 1: Hook & Background**: Tên, quê hương và chuyên môn hiện tại.
- **Bước 2: Highlight Achievement**: Dự án hoặc sở thích nổi bật.
- **Bước 3: Future Goal**: Mục tiêu phát triển bản thân trong tương lai.', 20, 1);

-- -------------------------------------------------------------
-- [MaChuDe = 3]: Mua sắm & Nhà hàng ẩm thực
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 3 AND TieuDe LIKE N'%Đặt bàn và Gọi món%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (3, 2, @gvId, N'Đặt bàn và Gọi món tại Nhà hàng Quốc tế (Dining Out & Ordering Food)', N'Nghe',
N'Học các mẫu câu gọi món, hỏi thực đơn món chay, yêu cầu hóa đơn và thanh toán tiền tip.',
N'# Dining Out English Mastery
- "Could we have a table for four, please?"
- "What do you recommend for the main course?"
- "Could we split the bill, please?"', 20, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 3 AND TieuDe LIKE N'%Kỹ năng Mua sắm%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (3, 1, @gvId, N'Kỹ năng Mua sắm, Hỏi kích cỡ và Đổi trả hàng hóa (Shopping & Returns)', N'TuVung',
N'Bộ từ vựng phong phú về quần áo, phụ kiện, chính sách hoàn tiền và thẻ khách hàng thân thiết.',
N'# Shopping Vocabulary & Expressions
- Fitting room (Phòng thử đồ)
- "Do you have this shirt in a medium size?"
- "I would like to exchange this item or get a refund."', 15, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 3 AND TieuDe LIKE N'%Văn hóa Ẩm thực%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (3, 3, @gvId, N'Văn hóa Ẩm thực Đường phố và Miêu tả Hương vị Món ăn', N'Doc',
N'Bài đọc hiểu về văn hóa ẩm thực Á - Âu cùng cách dùng tính từ miêu tả vị giác tinh tế.',
N'# Food Culture & Taste Descriptors
- Crispy (giòn tan), Savory (đậm đà vừa miệng), Fragrant (thơm phức).
- Đọc đoạn văn so sánh giữa ẩm thực đường phố Bangkok và Paris.', 25, 1);

-- -------------------------------------------------------------
-- [MaChuDe = 4]: Du lịch & Phương tiện giao thông
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 4 AND TieuDe LIKE N'%Tiếng Anh Sân bay%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (4, 2, @gvId, N'Tiếng Anh Sân bay: Check-in, Cân hành lý và Qua cửa An ninh', N'Doc',
N'Các đoạn hội thoại chuẩn tại quầy vé, khu vực kiểm tra hành lý và cửa ra máy bay (Boarding gate).',
N'# Airport Survival Guide
- Boarding pass, Carry-on luggage, Baggage allowance
- "Would you prefer a window or an aisle seat?"
- "Please place all liquids and electronic devices in the tray."', 25, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 4 AND TieuDe LIKE N'%Hỏi đường và Sử dụng Tàu điện%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (4, 1, @gvId, N'Hỏi đường và Sử dụng Tàu điện ngầm (Metro & City Navigation)', N'TuVung',
N'Chỉ dẫn phương hướng: rẽ trái, ngã tư, đi thẳng và mua vé tàu điện ngầm tự động.',
N'# Directions & Metro Navigation
- "Excuse me, how do I get to Central Station?"
- "Take the red line and transfer at City Hall."', 15, 1);

-- -------------------------------------------------------------
-- [MaChuDe = 6]: IELTS Reading - Skimming & Scanning
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 6 AND TieuDe LIKE N'%Chiến lược Skimming%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (6, 4, @gvId, N'Chiến lược Skimming & Scanning bài đọc IELTS Academic chuyên sâu', N'Doc',
N'Phương pháp đọc lướt nắm ý chính và quét từ khóa giúp tiết kiệm 40% thời gian làm bài Reading.',
N'# IELTS Reading Mastery: Skimming & Scanning
1. **Skimming**: Đọc tiêu đề, câu đầu (topic sentence) và câu cuối mỗi đoạn văn.
2. **Scanning**: Tìm kiếm tên riêng, con số, thuật ngữ in hoa hoặc in nghiêng.
3. **Paraphrasing**: Chú ý các từ đồng nghĩa giữa câu hỏi và bài đọc.', 35, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 6 AND TieuDe LIKE N'%True / False / Not Given%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (6, 4, @gvId, N'Tuyệt chiêu xử lý dạng bài True / False / Not Given & Yes / No / Not Given', N'Doc',
N'Phân biệt rõ ràng giữa False (thông tin mâu thuẫn) và Not Given (không đủ căn cứ kết luận).',
N'# Dạng bài T/F/NG trong IELTS
- **True**: Thông tin trùng khớp hoàn toàn về ngữ nghĩa.
- **False**: Thông tin trái ngược trực tiếp với bài viết.
- **Not Given**: Bài viết không đề cập hoặc không thể suy diễn chắc chắn.', 30, 1);

-- -------------------------------------------------------------
-- [MaChuDe = 7]: IELTS Writing Task 2 - Luận xã hội
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 7 AND TieuDe LIKE N'%Opinion Essay%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (7, 4, @gvId, N'Cấu trúc viết bài Opinion Essay (Agree or Disagree) chuẩn Band 7.5+', N'Viet',
N'Bố cục 4 đoạn chuẩn mực: Introduction, 2 Body Paragraphs với luận cứ chặt chẽ và Conclusion sắc bén.',
N'# Template 4 Đoạn Essay Band 7.5+
- **Introduction**: Paraphrase đề bài + Thesis statement nêu rõ quan điểm cá nhân.
- **Body 1**: Luận điểm 1 + Phân tích giải thích + Ví dụ minh họa thực tế.
- **Body 2**: Luận điểm 2 + So sánh phản biện + Tác động tích cực/tiêu cực.
- **Conclusion**: Tóm tắt lại 2 luận điểm và khẳng định lại quan điểm.', 40, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 7 AND TieuDe LIKE N'%Problem & Solution%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (7, 5, @gvId, N'Phát triển ý tưởng và Từ vựng dạng Problem & Solution Essay (Môi trường & Đô thị hóa)', N'Viet',
N'Cách liên kết nguyên nhân - giải pháp logic, sử dụng từ nối học thuật C1 như Consequently, Mitigate, Implement.',
N'# Problem & Solution Essay Strategy
- Cấu trúc Cause-Effect-Solution.
- Từ vựng chủ đề môi trường: carbon footprint, renewable energy, deforestation.', 45, 1);

-- -------------------------------------------------------------
-- [MaChuDe = 8]: IELTS Listening - Section 3 & 4
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 8 AND TieuDe LIKE N'%Section 3%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (8, 4, @gvId, N'Chiến lược chinh phục IELTS Listening Section 3: Thảo luận Đề tài Nhóm', N'Nghe',
N'Nhận diện các bẫy đổi ý kiến, bẫy đồng thuận và kỹ thuật ghi chú nhanh câu trả lời.',
N'# Listening Section 3 Strategies
- Theo dõi mạch tranh luận giữa 2-3 người nói.
- Chú ý các liên từ chỉ sự tương phản: "However, actually, to be honest, I thought so at first but...".', 30, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 8 AND TieuDe LIKE N'%Section 4%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (8, 5, @gvId, N'Kỹ thuật bắt từ khóa trong bài giảng học thuật Section 4 (Monologue Lecture)', N'Nghe',
N'Luyện nghe độc thoại tốc độ nhanh về các đề tài khảo cổ, sinh học và công nghệ vũ trụ.',
N'# Section 4 Note-taking
- Signposting words: "Turning now to...", "What is surprising is that...", "In conclusion...".', 35, 1);

-- -------------------------------------------------------------
-- [MaChuDe = 10]: Các thì trong tiếng Anh (Tenses)
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 10 AND TieuDe LIKE N'%Phân biệt Quá khứ Đơn%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (10, 2, @gvId, N'Phân biệt Quá khứ Đơn (Past Simple) và Quá khứ Tiếp diễn (Past Continuous)', N'NguPhap',
N'Cách dùng cấu trúc When và While để diễn tả hành động đang xảy ra thì có hành động khác xen vào.',
N'# Past Simple vs. Past Continuous
- **Công thức**: S + was/were + V-ing WHEN S + V2/ed.
- **Ví dụ**: "I was studying for the exam when the phone suddenly rang."
- **Bài tập**: Điền dạng đúng của động từ trong ngoặc.', 20, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 10 AND TieuDe LIKE N'%Làm chủ Tương lai Đơn%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (10, 1, @gvId, N'Làm chủ Tương lai Đơn (Will) và Tương lai Gần (Be Going To)', N'NguPhap',
N'Phân biệt quyết định tức thời tại thời điểm nói với kế hoạch hoặc dự đoán có căn cứ xác thực.',
N'# Will vs. Be Going To
- **Will**: Quyết định bột phát ("I will help you with that heavy box").
- **Be going to**: Dự định từ trước hoặc có bằng chứng cụ thể ("Look at the dark clouds! It is going to rain").', 15, 1);

-- -------------------------------------------------------------
-- [MaChuDe = 11]: Mệnh đề quan hệ & Câu bị động
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 11 AND TieuDe LIKE N'%Bí quyết Rút gọn%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (11, 4, @gvId, N'Bí quyết Rút gọn Mệnh đề Quan hệ dạng V-ing, V-ed và To-Infinitive', N'NguPhap',
N'Chuyên đề ngữ pháp nâng cao giúp câu văn gọn gàng, tăng điểm Grammatical Range & Accuracy.',
N'# Rút gọn Mệnh đề Quan hệ
1. **Chủ động -> V-ing**: The man *standing over there* is my teacher.
2. **Bị động -> V-ed/V3**: The documents *sent yesterday* were confidential.
3. **Số thứ tự (the first, the only...) -> To-V**: She was the only student *to pass* the test.', 25, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 11 AND TieuDe LIKE N'%Câu Bị Động Đặc Biệt%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (11, 3, @gvId, N'Câu Bị Động Đặc Biệt: Động từ tường thuật và Thể truyền khiến (Have/Get)', N'NguPhap',
N'Cấu trúc It is said that..., He is believed to be... và cấu trúc Have something done.',
N'# Thể Bị Động Đặc Biệt
- **Tường thuật**: "It is rumored that the merger will take place soon."
- **Causative**: "I had my car repaired last weekend" (nhờ thợ sửa).', 20, 1);

-- -------------------------------------------------------------
-- [MaChuDe = 12]: Câu điều kiện & Đảo ngữ
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 12 AND TieuDe LIKE N'%Tổng hợp Câu Điều Kiện%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (12, 3, @gvId, N'Tổng hợp Câu Điều Kiện loại 1, 2, 3 và Điều Kiện Trộn (Mixed Conditionals)', N'NguPhap',
N'Nắm trọn bản chất giả định điều kiện hiện tại, quá khứ và sự kết hợp kết quả hiện tại.',
N'# Conditionals Masterclass
- Type 1: If + V(s/es), will + V (Có thể xảy ra).
- Type 2: If + V2/ed, would + V (Trái thực tế hiện tại).
- Type 3: If + had + V3/ed, would have + V3/ed (Trái thực tế quá khứ).
- Mixed: If I had studied harder (type 3), I would have a degree now (type 2).', 25, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 12 AND TieuDe LIKE N'%Đảo ngữ%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (12, 5, @gvId, N'Cấu trúc Đảo ngữ nâng cao với Never, Seldom, Not only và Đảo ngữ câu điều kiện', N'NguPhap',
N'Tuyệt chiêu biến hóa câu văn đẳng cấp C1 với Had I known..., Were she to arrive..., Should you need help.',
N'# Advanced Inversion Structures
- "Never have I witnessed such a remarkable performance."
- "Had they followed the instructions, the system would not have crashed."', 30, 1);

-- -------------------------------------------------------------
-- [MaChuDe = 14]: Viết Email & Thư tín Thương mại
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 14 AND TieuDe LIKE N'%Quy chuẩn Viết Email%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (14, 3, @gvId, N'Quy chuẩn Viết Email Thương mại: Mở đầu, Thân bài và Lời chào kết', N'Viet',
N'Cách mở đầu chuyên nghiệp (Dear Mr./Ms.), nêu mục đích viết thư rõ ràng và kết thúc lịch sự.',
N'# Professional Business Email Framework
- "I am writing to inquire about your corporate training programs."
- "Please find attached the latest budget estimation."
- "I look forward to hearing from you soon. Sincerely yours,"', 25, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 14 AND TieuDe LIKE N'%Email Đặt lịch hẹn%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (14, 4, @gvId, N'Soạn thảo Email Đặt lịch hẹn, Xác nhận và Xử lý Khiếu nại Khách hàng', N'Viet',
N'Mẫu thư xin lỗi đối tác chuyên nghiệp, cam kết phương án bồi thường và giữ gìn uy tín doanh nghiệp.',
N'# Client Complaint Resolution Email
- "We sincerely apologize for the delay in dispatching your consignment."
- "To compensate for the inconvenience, we have upgraded your subscription free of charge."', 30, 1);

-- -------------------------------------------------------------
-- [MaChuDe = 15]: Đàm phán & Thuyết trình Dự án
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 15 AND TieuDe LIKE N'%Chiến lược Đàm phán%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (15, 4, @gvId, N'Chiến lược Đàm phán Hợp đồng, Thỏa thuận Giá cả và Điều khoản Giao hàng', N'TuVung',
N'Bộ từ vựng và câu nói ngoại giao giúp bạn đạt được thỏa thuận có lợi nhất (Win-Win Solution).',
N'# Business Negotiation Phrases
- "We are willing to consider your proposal if you can offer a 5% discount."
- "That sounds like a fair compromise. Let us put that in writing."', 30, 1);

IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 15 AND TieuDe LIKE N'%Thuyết trình Dự án%')
INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
VALUES (15, 5, @gvId, N'Kỹ năng Thuyết trình Dự án (Pitching) trước Hội đồng Đầu tư Quốc tế', N'Doc',
N'Cấu trúc Slide bài thuyết trình, ngôn ngữ cơ thể, cách dẫn dắt số liệu và trả lời phần Q&A sắc bén.',
N'# Project Pitching Mastery
- The Hook: Problem statement and market opportunity.
- The Solution: Core value proposition and scalability.
- Financial Projections & Call to Action.', 35, 1);

-- -------------------------------------------------------------
-- [MaChuDe = 19 & 20]: Công nghệ Thông tin (IT)
-- -------------------------------------------------------------
IF EXISTS (SELECT 1 FROM ChuDe WHERE MaChuDe = 19)
BEGIN
    IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 19 AND TieuDe LIKE N'%Kỹ thuật Phần mềm%')
    INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
    VALUES (19, 3, @gvId, N'Từ vựng tiếng Anh Chuyên ngành Kỹ thuật Phần mềm & Cơ sở dữ liệu', N'TuVung',
    N'Thuật ngữ chuyên sâu: Microservices, Concurrency, Indexing, ORM, Refactoring và CI/CD Pipeline.',
    N'# Software Engineering Terminology
- Throughput, Latency, Scalability, Fault Tolerance
- "The pull request has passed all automated unit tests and code linting."
- "We need to optimize the database query to reduce API response time."', 25, 1);
END;

IF EXISTS (SELECT 1 FROM ChuDe WHERE MaChuDe = 20)
BEGIN
    IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaChuDe = 20 AND TieuDe LIKE N'%Daily Standup%')
    INSERT INTO BaiHoc (MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, TomTat, NoiDung, ThoiGianUocTinhPhut, DaXuatBan)
    VALUES (20, 3, @gvId, N'Giao tiếp trong Buổi họp Daily Standup và Báo cáo Tiến độ Sprint', N'Doc',
    N'Mẫu câu 3 câu hỏi kinh điển trong Daily Standup: Đã làm gì hôm qua? Làm gì hôm nay? Có gặp khó khăn (blocker) gì không?',
    N'# Daily Scrum Communication Guide
1. "Yesterday, I completed the user authentication module and wrote unit tests."
2. "Today, I am going to integrate the payment gateway API."
3. "I have no blockers, but I need feedback from the design team regarding the modal dialog."', 20, 1);
END;

-- 3. CẬP NHẬT LẦN ĐĂNG NHẬP CUỐI
UPDATE NguoiDung SET LanDangNhapCuoi = SYSDATETIME() WHERE TenDangNhap = N'admin';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(minute, -12, SYSDATETIME()) WHERE TenDangNhap = N'giangvien';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(hour, -2, SYSDATETIME()) WHERE TenDangNhap = N'tung.hoang';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -1, SYSDATETIME()) WHERE TenDangNhap = N'maianh.tran';

UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(hour, -5, SYSDATETIME()) WHERE TenDangNhap = N'lan.nguyen';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -2, SYSDATETIME()) WHERE TenDangNhap = N'khoa.tran';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -4, SYSDATETIME()) WHERE TenDangNhap = N'huong.le';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -9, SYSDATETIME()) WHERE TenDangNhap = N'nam.vu';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -15, SYSDATETIME()) WHERE TenDangNhap = N'chau.ngo';
UPDATE NguoiDung SET LanDangNhapCuoi = DATEADD(day, -22, SYSDATETIME()) WHERE TenDangNhap = N'bao.do';

-- Chưa đăng nhập
UPDATE NguoiDung SET LanDangNhapCuoi = NULL WHERE TenDangNhap IN (N'ducanh.pham', N'mai.dang', N'hocvien');

PRINT N'Cập nhật thành công toàn bộ bài học và lần đăng nhập cuối!';
GO
