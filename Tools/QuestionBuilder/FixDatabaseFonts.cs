using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace QuestionBuilder
{
    public static class DatabaseFontFixer
    {
        public static void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string connStr = "Server=.\\SQLEXPRESS;Database=TiengAnhAI;Integrated Security=True;TrustServerCertificate=True;";
            using var conn = new SqlConnection(connStr);
            conn.Open();

            Console.WriteLine("1. Đang sửa font bảng CauTrucDeThi...");
            var blueprints = new List<(int Id, int TongSoCau, string Ten, string MoTa, string Nguon)>
            {
                (1, 82, "IELTS Academic Full Examination Blueprint",
                    "Cấu trúc bài thi IELTS Học thuật chuẩn quốc tế gồm 4 kỹ năng: Listening (4 Sections), Reading (3 Passages), Writing (2 Tasks) và Speaking (3 Parts).",
                    "Cambridge English Language Assessment & British Council IELTS Specifications"),

                (2, 40, "IELTS Academic Reading Examination Blueprint",
                    "Cấu trúc bài thi IELTS Academic Reading chuẩn quốc tế gồm 40 câu hỏi phân bổ qua 3 bài đọc học thuật (Passages 1, 2, 3), đánh giá toàn diện kỹ năng Skimming, Scanning, suy luận logic và vốn từ vựng học thuật B2-C1.",
                    "IELTS Academic Official Test Guidelines (British Council & IDP)"),

                (3, 200, "TOEIC Listening & Reading 200 Questions Blueprint",
                    "Cấu trúc đề thi TOEIC Quốc tế 2 kỹ năng Nghe & Đọc chuẩn ETS gồm 7 Part đánh giá năng lực tiếng Anh trong môi trường doanh nghiệp quốc tế.",
                    "ETS (Educational Testing Service) TOEIC Test Specifications"),

                (4, 40, "TOEIC Reading Test Part 5 & 6 Blueprint",
                    "Cấu trúc bài thi TOEIC Reading chuẩn hóa Phần 5 & 6 gồm 40 câu hỏi: Kiểm tra ngữ pháp thực hành công sở, thì động từ, liên từ, từ loại và đọc điền đoạn văn bản thương mại.",
                    "ETS TOEIC Reading Test Handbook"),

                (5, 80, "VSTEP B1-B2-C1 Examination Blueprint",
                    "Định dạng đề thi Đánh giá năng lực tiếng Anh bậc 3-5 theo Khung năng lực ngoại ngữ 6 bậc dùng cho Việt Nam gồm đầy đủ 4 kỹ năng.",
                    "Quyết định số 729/QĐ-BGDĐT của Bộ Giáo dục và Đào tạo"),

                (6, 40, "VSTEP B2 Standard Reading & Language Blueprint",
                    "Cấu trúc đề thi Đánh giá năng lực tiếng Anh Bậc 4 (VSTEP B2) chuẩn Bộ Giáo dục & Đào tạo gồm 40 câu hỏi phân bổ qua các phần đọc hiểu và kiến thức ngôn ngữ học thuật chuyên sâu.",
                    "Khung ĐGNL Tiếng Anh Bậc 4 VSTEP - ĐHQGHN & Bộ GD&ĐT"),

                (7, 60, "Cambridge A2 Key (KET) Examination Blueprint",
                    "Cấu trúc bài thi quốc tế Cambridge A2 Key đánh giá khả năng giao tiếp đơn giản và đọc hiểu trong các tình huống thường nhật.",
                    "Cambridge Assessment English - A2 Key Handbook for Teachers"),

                (8, 63, "Cambridge B1 Preliminary (PET) Examination Blueprint",
                    "Cấu trúc bài thi quốc tế Cambridge B1 Preliminary chứng nhận khả năng nắm vững các kiến thức tiếng Anh căn bản trong công việc và du lịch.",
                    "Cambridge Assessment English - B1 Preliminary Specifications"),

                (9, 88, "Cambridge B2 First (FCE) Examination Blueprint",
                    "Cấu trúc bài thi quốc tế Cambridge B2 First chứng nhận trình độ trung cao cấp, đủ năng lực làm việc trong môi trường nói tiếng Anh độc lập.",
                    "Cambridge Assessment English - B2 First Handbook for Teachers"),

                (10, 92, "Cambridge C1 Advanced (CAE) Examination Blueprint",
                    "Cấu trúc bài thi chuyên sâu Cambridge C1 Advanced chứng nhận năng lực ngôn ngữ xuất sắc ở bậc đại học và chuyên nghiệp toàn cầu.",
                    "Cambridge Assessment English - C1 Advanced Specifications"),

                (11, 45, "Cambridge Pre A1 Starters (YLE) Blueprint",
                    "Khung đánh giá tiếng Anh thiếu nhi khởi đầu Pre A1 Starters: Nhận biết đồ vật, màu sắc, từ vựng gia đình và chỉ dẫn đơn giản.",
                    "Cambridge English Young Learners (YLE) Pre A1 Handbook"),

                (12, 60, "Cambridge A1 Movers (YLE) Blueprint",
                    "Khung đánh giá tiếng Anh thiếu nhi A1 Movers: Nghe và đọc các mẩu hội thoại ngắn, mô tả người và đồ vật quen thuộc.",
                    "Cambridge English Young Learners (YLE) A1 Handbook"),

                (13, 70, "Cambridge A2 Flyers (YLE) Blueprint",
                    "Khung đánh giá tiếng Anh thiếu nhi A2 Flyers: Đọc hiểu các câu chuyện ngắn, trả lời câu hỏi và viết câu miêu tả hoàn chỉnh.",
                    "Cambridge English Young Learners (YLE) A2 Handbook"),

                (14, 10, "CEFR Adaptive Skills Practice Blueprint",
                    "Cấu trúc gói luyện tập thích ứng linh hoạt theo 6 bậc CEFR (A1-C1): Học viên ôn tập nhanh 10-15 câu theo từng chuyên đề ngữ pháp, từ vựng hoặc đọc hiểu không chịu áp lực thời gian.",
                    "Council of Europe - Common European Framework of Reference for Languages (CEFR)"),

                (15, 50, "Đề thi Tốt nghiệp THPT Quốc Gia môn Tiếng Anh Blueprint",
                    "Cấu trúc đề thi Tốt nghiệp THPT Quốc Gia môn Tiếng Anh chuẩn quy định của Bộ Giáo dục và Đào tạo gồm 50 câu trắc nghiệm khách quan trong thời gian 60 phút.",
                    "Bộ Giáo dục và Đào tạo - Ma trận đề thi Tốt nghiệp THPT môn Tiếng Anh")
            };

            foreach (var bp in blueprints)
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE CauTrucDeThi SET TenCauTruc = @Ten, MoTa = @MoTa, NguonThamChieu = @Nguon, TongSoCau = @TongSoCau WHERE MaCauTruc = @Id";
                cmd.Parameters.AddWithValue("@Ten", bp.Ten);
                cmd.Parameters.AddWithValue("@MoTa", bp.MoTa);
                cmd.Parameters.AddWithValue("@Nguon", bp.Nguon);
                cmd.Parameters.AddWithValue("@TongSoCau", bp.TongSoCau);
                cmd.Parameters.AddWithValue("@Id", bp.Id);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("2. Đang sửa font bảng ChiTietCauTrucDeThi (49 phần thi)...");
            var sections = new List<(int MaCauTruc, int ThuTuPhan, string TenPhan, string MoTa)>
            {
                // MaCauTruc = 1 (IELTS Full)
                (1, 1, "Section 1: Listening Social Dialogue", "Hội thoại đời sống thường nhật giữa 2 người (đặt phòng, hỏi thông tin dịch vụ)."),
                (1, 2, "Section 2: Listening Public Monologue", "Bài độc thoại hướng dẫn về tiện ích công cộng hoặc sự kiện văn hóa địa phương."),
                (1, 3, "Section 3: Listening Educational Discussion", "Thảo luận học thuật từ 2-4 người (sinh viên và giáo sư hướng dẫn đề tài nghiên cứu)."),
                (1, 4, "Section 4: Listening Academic Lecture", "Bài giảng học thuật độc thoại chuyên ngành (khoa học, lịch sử, môi trường)."),
                (1, 5, "Passage 1: Academic Reading Foundation", "Văn bản đọc học thuật mô tả thực tế, sự kiện lịch sử hoặc tiểu sử khoa học."),
                (1, 6, "Passage 2: Academic Reading Analysis", "Bài đọc phân tích quy trình công nghệ hoặc thảo luận chuyên ngành."),
                (1, 7, "Passage 3: Complex Discussion & Argument", "Văn bản lập luận phức hợp với các quan điểm đối lập nhau."),
                (1, 8, "Task 1: Academic Writing Data Report", "Miêu tả và phân tích biểu đồ, bảng biểu hoặc quy trình (tối thiểu 150 từ)."),
                (1, 9, "Task 2: Discursive Essay", "Viết bài luận nghị luận xã hội về các vấn đề toàn cầu (tối thiểu 250 từ)."),

                // MaCauTruc = 2 (IELTS Reading)
                (2, 1, "Passage 1: Academic Reading Foundation & Core Facts", "Bài đọc học thuật giới thiệu chủ đề thực tế, tiểu sử hoặc lịch sử khoa học."),
                (2, 2, "Passage 2: Technology, Science & Research Analysis", "Bài đọc chuyên ngành công nghệ, y sinh hoặc trí tuệ nhân tạo."),
                (2, 3, "Passage 3: Discursive Argument & Complex Critical Thinking", "Bài đọc phân tích các luồng quan điểm đối lập, suy luận học thuật chuyên sâu."),

                // MaCauTruc = 3 (TOEIC Full)
                (3, 1, "Part 1: Photographs (Mô tả tranh)", "Nghe 4 phương án miêu tả khách quan bức tranh chụp hình động và đồ vật."),
                (3, 2, "Part 2: Question - Response (Hỏi - Đáp)", "Nghe một câu hỏi và 3 câu phản hồi để chọn phương án giao tiếp phù hợp."),
                (3, 3, "Part 3: Short Conversations (Hội thoại ngắn)", "Nghe 13 đoạn hội thoại công sở giữa 2-3 người, mỗi đoạn trả lời 3 câu hỏi."),
                (3, 4, "Part 4: Short Talks (Bài nói ngắn)", "Nghe 10 bài thông báo, tin nhắn thoại, bản tin hoặc bài phát biểu, mỗi bài 3 câu hỏi."),
                (3, 5, "Part 5: Incomplete Sentences (Điền câu)", "Hoàn thành câu thương mại với các phương án ngữ pháp, từ loại và từ vựng doanh nghiệp."),
                (3, 6, "Part 6: Text Completion (Điền đoạn văn)", "Điền từ và câu phù hợp vào 4 văn bản công sở (email, thông báo, thư ngỏ)."),
                (3, 7, "Part 7: Reading Comprehension (Đọc hiểu văn bản)", "Đọc hiểu đoạn đơn (Single passages) và đoạn đa (Double & Triple passages) thương mại."),

                // MaCauTruc = 4 (TOEIC Part 5 & 6)
                (4, 1, "Part 5: Incomplete Sentences (Hoàn thành câu thương mại)", "Ngữ pháp công sở, từ loại, thì động từ và từ vựng hợp đồng doanh nghiệp."),
                (4, 2, "Part 6: Text Completion (Điền đoạn văn bản công sở)", "Đọc hiểu điền thông báo, email và bản tin công ty."),

                // MaCauTruc = 5 (VSTEP Full)
                (5, 1, "Kỹ năng Nghe (Listening) - 3 Phần", "Part 1: 8 hướng dẫn thông báo; Part 2: 3 bài hội thoại; Part 3: 3 bài giảng/thuyết trình."),
                (5, 2, "Kỹ năng Đọc (Reading) - 4 Bài đọc", "4 bài đọc độ dài khoảng 450-500 từ mỗi bài thuộc các chủ đề xã hội, văn hóa, khoa học."),
                (5, 3, "Kỹ năng Viết (Writing) - 2 Bài viết", "Task 1: Viết thư/email (120 từ); Task 2: Viết bài luận tranh luận xã hội (250 từ)."),
                (5, 4, "Kỹ năng Nói (Speaking) - 3 Phần", "Part 1: Giao tiếp xã hội; Part 2: Thảo luận giải pháp; Part 3: Phát triển chủ đề tự chọn."),

                // MaCauTruc = 6 (VSTEP B2 Standard)
                (6, 1, "Phần 1: Ngữ pháp phức hợp, Đảo ngữ & Cấu trúc B2", "Mệnh đề quan hệ rút gọn, đảo ngữ điều kiện, bị động nâng cao."),
                (6, 2, "Phần 2: Đọc hiểu văn bản Khoa học & Công nghệ", "Đọc hiểu tài liệu khoa học và công nghệ hiện đại."),
                (6, 3, "Phần 3: Đọc hiểu Môi trường & Phát triển bền vững", "Đọc hiểu biến đổi khí hậu, sinh thái và bảo tồn."),
                (6, 4, "Phần 4: Đọc hiểu Văn hóa, Xã hội & Nghị luận", "Văn bản phân tích tác động xã hội và tư duy biện luận."),

                // MaCauTruc = 7 (Cambridge KET)
                (7, 1, "Paper 1: Reading and Writing", "Gồm 7 phần: Biển báo thông tin ngắn, đọc hiểu văn bản, điền từ vào chỗ trống và viết tin nhắn ngắn."),
                (7, 2, "Paper 2: Listening", "Gồm 5 phần nghe: Thông tin đời sống thường ngày, điền ghi chú số điện thoại/giờ giấc."),
                (7, 3, "Paper 3: Speaking", "Gồm 2 phần: Phỏng vấn thông tin cá nhân và thảo luận chủ đề quen thuộc với giám khảo."),

                // MaCauTruc = 8 (Cambridge PET)
                (8, 1, "Paper 1: Reading", "6 phần đọc: Đọc biển báo, ghép người với thông tin (Matching), đọc hiểu bài báo và điền từ cloze."),
                (8, 2, "Paper 2: Writing", "2 phần viết: Viết email hồi đáp (100 từ) và viết một bài báo hoặc câu chuyện ngắn (100 từ)."),
                (8, 3, "Paper 3: Listening", "4 phần nghe: Thông báo ngắn, cuộc hội thoại 2 người và bài phỏng vấn dài."),
                (8, 4, "Paper 4: Speaking", "4 phần nói: Chào hỏi, mô tả tranh ảnh màu đơn, thảo luận tình huống và bày tỏ ý kiến."),

                // MaCauTruc = 9 (Cambridge FCE)
                (9, 1, "Paper 1: Reading and Use of English", "7 phần: Multiple-choice cloze, Open cloze, Word formation, Key word transformation, Reading comprehension."),
                (9, 2, "Paper 2: Writing", "Part 1: Viết bài luận (140-190 từ); Part 2: Chọn 1 trong các dạng (article, email, review, report)."),
                (9, 3, "Paper 3: Listening", "4 phần nghe: Độc thoại ngắn, bài nói có khoảng trống, ghép nhiều người nói và phỏng vấn."),
                (9, 4, "Paper 4: Speaking", "4 phần nói: Phỏng vấn, so sánh 2 bức ảnh (1 phút độc thoại), nhiệm vụ tương tác và thảo luận sâu."),

                // MaCauTruc = 10 (Cambridge CAE)
                (10, 1, "Paper 1: Reading and Use of English", "8 phần: Multiple-choice cloze, Open cloze, Word formation, Key word transformation, Gapped text, Multiple matching."),
                (10, 2, "Paper 2: Writing", "Part 1: Viết bài luận học thuật dựa trên văn bản đọc (220-260 từ); Part 2: Viết thư, proposal hoặc review."),
                (10, 3, "Paper 3: Listening", "4 phần nghe: Chiết đoạn ngắn, bài nói điền thông tin chi tiết, phỏng vấn và ghép đa người nói."),
                (10, 4, "Paper 4: Speaking", "4 phần nói: Trao đổi mở rộng, so sánh phân tích ảnh, thảo luận hợp tác giải pháp và tranh luận quan điểm."),

                // MaCauTruc = 15 (THPT Quốc Gia)
                (15, 1, "Phần 1: Ngữ âm & Trọng âm", "Phát âm nguyên âm/phụ âm và trọng âm từ 2-3 âm tiết."),
                (15, 2, "Phần 2: Ngữ pháp & Từ vựng căn bản đến nâng cao", "Thì động từ, câu bị động, điều kiện, mệnh đề quan hệ, từ loại và thành ngữ."),
                (15, 3, "Phần 3: Chức năng giao tiếp & Từ đồng nghĩa - Trái nghĩa", "Giao tiếp xã hội và tìm từ đồng nghĩa, trái nghĩa theo ngữ cảnh."),
                (15, 4, "Phần 4: Đọc điền từ & Đọc hiểu 2 đoạn văn", "Đọc điền từ (Cloze test 5 câu) và 2 bài đọc hiểu phân tích chi tiết (12 câu)."),
                (15, 5, "Phần 5: Tìm lỗi sai & Biến đổi câu, Kết hợp câu", "Xác định lỗi ngữ pháp và chọn câu đồng nghĩa, câu ghép chuẩn xác.")
            };

            foreach (var s in sections)
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE ChiTietCauTrucDeThi SET TenPhan = @TenPhan, MoTa = @MoTa WHERE MaCauTruc = @MaCauTruc AND ThuTuPhan = @ThuTuPhan";
                cmd.Parameters.AddWithValue("@TenPhan", s.TenPhan);
                cmd.Parameters.AddWithValue("@MoTa", s.MoTa);
                cmd.Parameters.AddWithValue("@MaCauTruc", s.MaCauTruc);
                cmd.Parameters.AddWithValue("@ThuTuPhan", s.ThuTuPhan);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("3. Đang sửa font bảng BaiTapDeThi (23 đề thi)...");
            var exams = new List<(int Id, string Ten, string MoTa)>
            {
                (0, "Cambridge Pre A1 Starters: Khảo Thí Từ Vựng & Phản Xạ Căn Bản (30 Câu)", "Bài thi khảo thí năng lực Pre A1 Starters: 30 câu hỏi nhận diện từ vựng hình ảnh, màu sắc, chỉ dẫn lớp học và giao tiếp căn bản."),
                (1, "Cambridge A2 Key (KET): Đọc Hiểu & Ngữ Pháp Quá Khứ, So Sánh (30 Câu)", "Bài thi mô phỏng định dạng Cambridge A2 Key: 30 câu trắc nghiệm đọc hiểu đoạn văn ngắn, biển báo và ngữ pháp thì quá khứ, cấu trúc so sánh."),
                (2, "TOEIC Reading Diagnostic Test: Khảo Thí Đọc Hiểu Thương Mại (40 Câu)", "Đề thi khảo thí định dạng TOEIC Reading: 40 câu hỏi hoàn thành câu và đọc hiểu văn bản thương mại, tuyển dụng, báo cáo tài chính."),
                (3, "IELTS Academic Reading Examination: Standard Mock Test #05 (40 Câu)", "Đề thi chuẩn hóa IELTS Academic Reading gồm 40 câu hỏi trải đều trên 3 bài đọc chuyên sâu: Giáo dục, Công nghệ và Biến đổi khí hậu."),
                (4, "Cambridge B2 First (FCE): Đột Phá Ngữ Pháp & Đọc Hiểu Chuyên Sâu (35 Câu)", "Đề thi chuẩn Cambridge B2 First (FCE) Paper 1: 35 câu kiểm tra biến đổi từ loại, điền khuyết đoạn văn và đọc hiểu phân tích luận điểm."),
                (5, "IELTS Advanced Academic Mock Test: Phân Tích Luận Điểm & Tư Duy Phản Biện (40 Câu)", "Bài thi mô phỏng IELTS Academic nâng cao: 40 câu hỏi phân tích các bài nghiên cứu y sinh, công nghệ thông tin và xã hội học."),
                (6, "Thử Thách Phản Xạ Trắc Nghiệm 60s", "Luyện tập phản xạ nhanh 10 câu trắc nghiệm ngữ pháp và từ vựng cốt lõi."),
                (7, "TOEIC Reading Test Part 5 & 6: 40 Câu Chuẩn Hóa Doanh Nghiệp", "Đề thi TOEIC Reading chuẩn hóa: 40 câu hoàn thành câu công sở (Part 5) và đọc điền đoạn văn thương mại quốc tế (Part 6)."),
                (8, "IELTS Academic Reading Examination: Standard 40 Questions Mock Test", "Đề thi mô phỏng định dạng IELTS Academic Reading chuẩn quốc tế: 40 câu hỏi phân bổ qua 3 bài đọc học thuật về AI, Y học và Khoa học môi trường."),
                (9, "VSTEP B2 Chuẩn Hóa: Khảo Thí Đọc Hiểu & Ngôn Ngữ Học Thuật (40 Câu)", "Đề thi VSTEP Bậc 4 (B2) chuẩn hóa: 40 câu trắc nghiệm chuyên sâu ngữ pháp phức hợp, cấu trúc đảo ngữ và 4 bài đọc hiểu học thuật tổng hợp."),
                (10, "Gói Luyện Tập: Chuyên Đề Câu Điều Kiện & Đảo Ngữ Nâng Cao", "Luyện tập chuyên sâu các dạng câu điều kiện loại 1, 2, 3, hỗn hợp và cấu trúc đảo ngữ thường gặp trong đề thi."),
                (11, "Gói Luyện Tập: 100 Từ Vựng & Collocations Oxford Cốt Lõi", "Rèn luyện 100 cụm từ và collocations học thuật thường gặp giúp nâng cao vốn từ và diễn đạt tự nhiên."),
                (12, "Gói Luyện Tập: Đọc Hiểu Chuyên Sâu Trí Tuệ Nhân Tạo & Y Học", "Rèn luyện kỹ năng đọc hiểu chuyên đề công nghệ AI và thuật ngữ y khoa hiện đại."),
                (13, "Gói Luyện Tập: Cấu Trúc Viết & Phát Hiện Lỗi Sai Học Thuật", "Luyện tập phát hiện lỗi ngữ pháp, chia thì và cấu trúc câu trong văn phong học thuật."),
                (14, "Gói Luyện Tập Tổng Hợp: Ngữ Pháp Căn Bản & Giao Tiếp Đời Sống", "Ôn tập củng cố ngữ pháp căn bản và các mẫu câu giao tiếp tình huống đời sống."),
                (15, "Gói Luyện Tập: Thành Ngữ & Cụm Từ Học Thuật Band C1 (Advanced Idioms)", "Nâng cao vốn thành ngữ và cụm diễn đạt cao cấp cho mục tiêu Band C1."),
                (18, "Cambridge A2 Key (KET): Giao Tiếp & Đọc Hiểu Nền Tảng (30 Câu)", "Đề khảo thí chuẩn Cambridge A2 Key: 30 câu kiểm tra kỹ năng đọc biển báo, mẩu tin ngắn, giao tiếp thường nhật và ngữ pháp căn bản."),
                (19, "Gói Luyện Tập: Từ Vựng & Thuật Ngữ Học Thuật Nâng Cao (B2)", "Rèn luyện vốn từ vựng học thuật B2 thường gặp trong các kỳ thi quốc tế và giao tiếp chuyên nghiệp."),
                (20, "Gói Luyện Tập: Chuyên Đề Ngữ Pháp Phức Hợp & Mệnh Đề Quan Hệ (B2)", "Luyện tập chuyên sâu ngữ pháp bậc B2: Mệnh đề quan hệ rút gọn, liên từ đẳng lập và thì hoàn thành."),
                (21, "Cambridge B2 First (FCE): Use of English & Reading Comprehension (35 Câu)", "Đề thi chuẩn Cambridge B2 First (FCE): 35 câu Use of English và Đọc hiểu phân tích luận điểm học thuật."),
                (22, "Đề thi Tốt nghiệp THPT Quốc Gia môn Tiếng Anh: Khảo Thí Chuẩn Hóa Bộ GD&ĐT (50 Câu)", "Đề thi mô phỏng kỳ thi Tốt nghiệp THPT Quốc Gia môn Tiếng Anh chuẩn quy định Bộ GD&ĐT gồm 50 câu hỏi trắc nghiệm hoàn chỉnh trong 60 phút.")
            };

            foreach (var u in exams)
            {
                using var c = conn.CreateCommand();
                c.CommandText = "UPDATE BaiTapDeThi SET TenBaiTapDeThi = @Ten, MoTa = @MoTa WHERE MaBaiTapDeThi = @Id";
                c.Parameters.AddWithValue("@Ten", u.Ten);
                c.Parameters.AddWithValue("@MoTa", u.MoTa);
                c.Parameters.AddWithValue("@Id", u.Id);
                c.ExecuteNonQuery();
            }

            Console.WriteLine("4. Kiểm tra toàn diện kết quả sau khi sửa...");
            using (var chkCmd = conn.CreateCommand())
            {
                chkCmd.CommandText = "SELECT COUNT(*) FROM CauTrucDeThi WHERE TenCauTruc LIKE '%?%' OR MoTa LIKE '%?%'";
                int bpErr = (int)chkCmd.ExecuteScalar();
                chkCmd.CommandText = "SELECT COUNT(*) FROM ChiTietCauTrucDeThi WHERE TenPhan LIKE '%?%' OR MoTa LIKE '%?%'";
                int secErr = (int)chkCmd.ExecuteScalar();
                chkCmd.CommandText = "SELECT COUNT(*) FROM BaiTapDeThi WHERE TenBaiTapDeThi LIKE '%?%' OR MoTa LIKE '%?%'";
                int exErr = (int)chkCmd.ExecuteScalar();

                Console.WriteLine($"-> CauTrucDeThi còn lỗi font: {bpErr}");
                Console.WriteLine($"-> ChiTietCauTrucDeThi còn lỗi font: {secErr}");
                Console.WriteLine($"-> BaiTapDeThi còn lỗi font: {exErr}");

                if (bpErr == 0 && secErr == 0 && exErr == 0)
                {
                    Console.WriteLine("SUCCESS: 100% CSDL TiengAnhAI ĐÃ KHÔNG CÒN BẤT KỲ KÝ TỰ LỖI NÀO!");
                }
            }
        }

        public static void PrintVstep()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string connStr = "Server=.\\SQLEXPRESS;Database=TiengAnhAI;Integrated Security=True;TrustServerCertificate=True;";
            using var conn = new SqlConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TenCauTruc, MoTa FROM CauTrucDeThi WHERE MaCauTruc = 6";
            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                Console.WriteLine($"Ten: {r[0]}");
                Console.WriteLine($"MoTa: {r[1]}");
            }
            r.Close();
            cmd.CommandText = "SELECT ThuTuPhan, TenPhan, MoTa FROM ChiTietCauTrucDeThi WHERE MaCauTruc = 6 ORDER BY ThuTuPhan";
            using var r2 = cmd.ExecuteReader();
            while (r2.Read())
            {
                Console.WriteLine($"  P{r2[0]}: {r2[1]} -> {r2[2]}");
            }
        }

        public static void PrintTop()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string connStr = "Server=.\\SQLEXPRESS;Database=TiengAnhAI;Integrated Security=True;TrustServerCertificate=True;";
            using var conn = new SqlConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MaCauTruc, TenCauTruc FROM CauTrucDeThi ORDER BY MaCauTruc";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader[0]} -> {reader[1]}");
            }
        }
    }
}
