using System.Collections.Generic;

namespace QuestionBuilder
{
    public static class Batch08
    {
        public static List<QuestionItem> GetQuestions()
        {
            var list = new List<QuestionItem>();

            // --- TÀI LIỆU: 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_351",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "According to the interview guide 'Tell Me About Yourself', what is the recommended 3-part framework for crafting a compelling response?",
                Explanation = "Công thức chuẩn mực quốc tế: 'Present - Past - Future' (Hiện tại: vai trò hiện tại & thế mạnh -> Quá khứ: kinh nghiệm & thành tựu -> Tương lai: lý do ứng tuyển vào vị trí này).",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Công thức Present - Past - Future trong phỏng vấn tuyển dụng.",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "Childhood - High School - University", false), ("B", "Present (current role & strengths) - Past (relevant experience & accomplishments) - Future (why this role & company)", true), ("C", "Salary history - Personal debt - Family tree", false), ("D", "Hobbies - Favorite food - Music tastes", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_352",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "What is the biggest mistake candidates make when answering 'Tell Me About Yourself'?",
                Explanation = "Sai lầm lớn nhất là đọc lại nguyên văn từng dòng trong bản CV hoặc kể lể chi tiết tiểu sử cá nhân đời tư từ thời thơ ấu.",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Các sai lầm chí mạng cần tránh: Đọc lại CV, kể chuyện đời tư.",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "Smiling politely at the interviewer", false), ("B", "Reciting their entire resume chronologically or recounting personal life stories irrelevant to the job", true), ("C", "Mentioning their core technical skills", false), ("D", "Speaking clearly in professional English", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_353",
                TopicId = 18, LevelId = 4, QuestionType = "DienKhuyet", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank in a strong interview opening: 'Currently, I am a software engineer _______ in cloud-native microservices architecture.'",
                Explanation = "'specializing' (chuyên về mảng).",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Mẫu câu mở đầu phần Present: Currently, I am a [role] specializing in...",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "specializing", true), ("B", "special", false), ("C", "specially", false), ("D", "specialty", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_354",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "De",
                QuestionText = "What is the ideal time duration for answering 'Tell Me About Yourself' in an interview?",
                Explanation = "Thời lượng tối ưu cho câu trả lời là khoảng 90 đến 120 giây (1.5 - 2 phút).",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Độ dài tối ưu: 90 đến 120 giây.",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "10 seconds", false), ("B", "90 to 120 seconds", true), ("C", "15 minutes", false), ("D", "30 minutes", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_355",
                TopicId = 18, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: Every achievement shared in 'Tell Me About Yourself' should be tailored directly to the specific job requirements listed in the company's job description.",
                Explanation = "Đúng. Mọi thành tựu chia sẻ cần được may đo trực tiếp để chứng minh bạn là mảnh ghép hoàn hảo cho vị trí đang tuyển.",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Nguyên tắc May đo câu trả lời (Tailoring to the Job Description).",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- TÀI LIỆU: 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_356",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "In behavioral interviews, what does the 'STAR' acronym stand for?",
                Explanation = "'STAR' là viết tắt của: Situation (Tình huống), Task (Nhiệm vụ), Action (Hành động cụ thể của bạn), Result (Kết quả định lượng đạt được).",
                Citation = "Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Định nghĩa mô hình STAR: Situation, Task, Action, Result.",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx",
                Options = new() { ("A", "Strategy, Timing, Analysis, Review", false), ("B", "Situation, Task, Action, Result", true), ("C", "Software, Testing, Automation, Release", false), ("D", "Salary, Title, Allowance, Reward", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_357",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "Which part of the STAR method should receive the largest proportion (approximately 60%) of your answer time?",
                Explanation = "Phần 'Action' (Hành động bạn đã thực hiện) là trọng tâm quan trọng nhất mà nhà tuyển dụng muốn nghe để đánh giá năng lực thực chiến và kỹ năng giải quyết vấn đề.",
                Citation = "Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Phân bổ thời lượng mô hình STAR: Action chiếm 50-60% dung lượng.",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx",
                Options = new() { ("A", "Situation (complaining about old colleagues)", false), ("B", "Action (the specific steps, technologies, and decisions you took to resolve the challenge)", true), ("C", "Task (describing the project assignment)", false), ("D", "Waiting silently", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_358",
                TopicId = 18, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Convert the generic result into an impactful STAR outcome: 'Our team improved system performance a lot.'",
                Explanation = "'As a result, we reduced server latency by 45% and scaled our daily active users from 10k to 50k without downtime.' cung cấp số liệu định lượng cụ thể, minh chứng rõ ràng.",
                Citation = "Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Quy tắc trình bày phần Result: Định lượng hóa bằng số liệu cụ thể.",
                SourceType = "Applied", SourceFile = "02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx",
                Options = new() { ("A", "As a result of our optimization, API latency dropped by 45%, and the platform handled 50,000 concurrent users without downtime.", true), ("B", "The code was faster and boss was super happy.", false), ("C", "Performance was good because I coded well.", false), ("D", "Everything improved by unknown amounts.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_359",
                TopicId = 18, LevelId = 4, QuestionType = "DienKhuyet", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'In the STAR Action section, candidates should use \"I\" rather than \"_______\" to highlight their individual contribution to the team.'",
                Explanation = "Khi trả lời phỏng vấn hành vi, phải dùng 'I' để nêu bật đóng góp cá nhân, tránh dùng 'we' chung chung làm mờ vai trò của bản thân.",
                Citation = "Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Lưu ý đại từ nhân xưng: Dùng 'I' thay vì 'we' trong phần Action.",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx",
                Options = new() { ("A", "we", true), ("B", "they", false), ("C", "you", false), ("D", "he", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_360",
                TopicId = 18, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: An effective STAR Result can also include reflective lessons learned (what you would do differently in hindsight).",
                Explanation = "Đúng. Nêu bài học kinh nghiệm (Lesson learned) thể hiện tư duy cầu thị (growth mindset) được đánh giá rất cao.",
                Citation = "Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Mở rộng phần Result: Bài học kinh nghiệm và tư duy hoàn thiện bản thân.",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- TÀI LIỆU: 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx (B2 - C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_361",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "What are the three most powerful 'Hook' techniques for opening an international presentation?",
                Explanation = "3 kỹ thuật Hook kinh điển: 1. Đưa ra một số liệu thống kê giật mình; 2. Đặt một câu hỏi khiêu khích tư duy; 3. Kể một câu chuyện ngắn có tính liên hệ sâu sắc.",
                Citation = "Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - 3 Chiến thuật Hook kinh điển: Surprising statistic, Provocative question, Personal narrative.",
                SourceType = "Extracted", SourceFile = "01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx",
                Options = new() { ("A", "Clearing throat, adjusting glasses, reading agenda", false), ("B", "A shocking statistical data point, a thought-provoking question, or a compelling brief story", true), ("C", "Apologizing for being nervous, checking microphone repeatedly, coughing", false), ("D", "Complaining about hotel traffic and bad weather", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_362",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "Analyze this opening hook: 'Did you know that 85% of corporate cyberattacks stem from human error rather than technical flaws?' Which technique is this?",
                Explanation = "Đây là kỹ thuật 'Startling Statistic Hook' (Mở đầu bằng số liệu thống kê giật mình gây tò mò).",
                Citation = "Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Ví dụ phân tích Startling Statistic Hook.",
                SourceType = "Extracted", SourceFile = "01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx",
                Options = new() { ("A", "Startling Statistic Hook", true), ("B", "Fictional fairy tale", false), ("C", "Self-deprecating humor", false), ("D", "Product complaint", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_363",
                TopicId = 15, LevelId = 5, QuestionType = "DienKhuyet", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "Fill in the blank: 'After delivering the hook, the speaker must establish the WIIFM factor: \"What's In It For _______?\"'",
                Explanation = "'WIIFM' là nguyên tắc tâm lý khán giả: 'What's In It For Me?' (Bài nói này mang lại giá trị gì cho tôi/người nghe?).",
                Citation = "Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Nguyên tắc tâm lý WIIFM (What's In It For Me).",
                SourceType = "Extracted", SourceFile = "01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx",
                Options = new() { ("A", "Me", true), ("B", "You", false), ("C", "Us", false), ("D", "Them", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_364",
                TopicId = 15, LevelId = 4, QuestionType = "DungSai", Skill = "Noi", Difficulty = "De",
                QuestionText = "True or False: Opening a presentation by saying 'I didn't have much time to prepare this slide deck' builds credibility with executive audiences.",
                Explanation = "Sai. Tuyệt đối không bao giờ mở đầu bằng lời xin lỗi về việc thiếu chuẩn bị vì nó phá hủy hoàn toàn uy tín chuyên môn của diễn giả.",
                Citation = "Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Những câu mở đầu phá hủy uy tín cần tránh tuyệt đối.",
                SourceType = "Extracted", SourceFile = "01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx",
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_365",
                TopicId = 15, LevelId = 5, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Turn this boring opening into an engaging presentation hook: 'Today I will talk about our company's new AI software.'",
                Explanation = "'Imagine reducing your customer support turnaround time from four hours to four seconds. Today, I am excited to reveal how our new AI platform makes that a reality.'",
                Citation = "Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Bài tập thực hành chuyển đổi mở bài nhàm chán thành Hook truyền cảm hứng.",
                SourceType = "Applied", SourceFile = "01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx",
                Options = new() { ("A", "Imagine reducing customer support turnaround from four hours to four seconds. Today, I will reveal how our AI engine achieves exactly that.", true), ("B", "I have some AI slides to show today if you care.", false), ("C", "Software is being discussed by me today.", false), ("D", "Listen to me because I have software.", false) }
            });

            // --- TÀI LIỆU: 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx (C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_366",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "In high-stakes executive Q&A, what should a speaker do before immediately answering an aggressive or hostile question?",
                Explanation = "Kỹ thuật chuyên nghiệp: Giữ bình tĩnh, dừng lại 2 giây, cảm ơn và diễn đạt lại (rephrase) câu hỏi bằng ngôn từ trung tính nhằm giảm tính công kích.",
                Citation = "Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Kỹ thuật Rephrasing và De-escalating câu hỏi công kích.",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx",
                Options = new() { ("A", "Shout back angrily at the attendee", false), ("B", "Pause, thank the questioner, and rephrase the question using neutral, objective terminology", true), ("C", "Pretend not to hear and look out the window", false), ("D", "Call security immediately", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_367",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "Which phrase politely defuses a multi-part, rambling question from an audience member?",
                Explanation = "'Thank you. To ensure I address your core concern, are you primarily asking about our data privacy compliance?' giúp cô đọng trọng tâm câu hỏi.",
                Citation = "Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Xử lý câu hỏi lan man (Rambling questions).",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx",
                Options = new() { ("A", "You talk too much, stop talking.", false), ("B", "Thank you. To ensure I address your core concern, are you primarily asking about our data privacy compliance?", true), ("C", "I have no idea what you just said.", false), ("D", "Sit down please.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_368",
                TopicId = 15, LevelId = 5, QuestionType = "DienKhuyet", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "Fill in the blank with a diplomatic phrase: 'That is an insightful observation; however, our empirical data indicates a somewhat _______ trend.'",
                Explanation = "'different' (một xu hướng hơi khác biệt) là cách bác bỏ ý kiến đối lập một cách lịch thiệp và khách quan.",
                Citation = "Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Mẫu câu phản biện lịch sự (Diplomatic disagreement).",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx",
                Options = new() { ("A", "different", true), ("B", "stupid", false), ("C", "terrible", false), ("D", "worthless", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_369",
                TopicId = 15, LevelId = 5, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: Repeating or paraphrasing an audience question benefits everyone in the room because other attendees may not have heard the question clearly.",
                Explanation = "Đúng. Lặp lại câu hỏi vừa giúp toàn thể hội trường nghe rõ, vừa giúp diễn giả có thêm 3-5 giây suy nghĩ câu trả lời thấu đáo.",
                Citation = "Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Lợi ích của việc lặp lại câu hỏi trước toàn thể hội trường.",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_370",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "How do you smoothly bridge back to your core presentation message when answering a tangential question?",
                Explanation = "Kỹ thuật 'Bridging': Trả lời ngắn gọn câu hỏi rồi dùng cụm nối 'And that ties directly into our broader mission to...' để đưa câu chuyện trở lại thông điệp trọng tâm.",
                Citation = "Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Kỹ thuật Bridging (Cầu nối thông điệp).",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx",
                Options = new() { ("A", "Ignore the question and tell everyone to look at slide 1 again.", false), ("B", "Answer briefly, then use a transitional bridge like 'And that connects directly with our primary objective of...'", true), ("C", "Tell the attendee that their question is off-topic and ban them.", false), ("D", "End the Q&A abruptly.", false) }
            });

            // --- TÀI LIỆU: 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf (C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_371",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "In the EdTech startup pitch deck summary, what core customer pain point is addressed?",
                Explanation = "Vấn đề cốt lõi: Người học tiếng Anh thiếu môi trường luyện tập phản xạ thực tế cá nhân hóa và chi phí gia sư 1-1 bản xứ quá đắt đỏ.",
                Citation = "Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 2: The Problem: High 1-on-1 tutoring costs and lack of personalized adaptive practice.",
                SourceType = "Extracted", SourceFile = "03_Slide_Pitching_Goi_von_Startup_EdTech.pdf",
                Options = new() { ("A", "A shortage of physical pencils in school classrooms", false), ("B", "Prohibitive costs of private 1-on-1 tutoring and lack of scalable, personalized speaking practice", true), ("C", "The internet being too fast for students", false), ("D", "Students disliking mobile games", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_372",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What is the EdTech startup's proposed technology solution according to the pitch deck?",
                Explanation = "Giải pháp: Ứng dụng AI Voice Agent tương tác thời gian thực, cung cấp phản hồi ngữ pháp tức thì và lộ trình học cá nhân hóa theo CEFR.",
                Citation = "Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 3: The Solution: Real-time AI Voice Tutor with personalized CEFR learning paths.",
                SourceType = "Extracted", SourceFile = "03_Slide_Pitching_Goi_von_Startup_EdTech.pdf",
                Options = new() { ("A", "A printed dictionary textbook", false), ("B", "An AI-powered interactive voice tutor delivering personalized feedback and adaptive curriculum at 1/10th the cost", true), ("C", "A traditional chalkboard classroom chain", false), ("D", "A social media messaging app for celebrities", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_373",
                TopicId = 15, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Fill in the business model term: 'The EdTech startup operates on a _______ model offering free baseline access with tiered monthly subscriptions.'",
                Explanation = "'Freemium' là mô hình kinh doanh phổ biến cho phép dùng thử miễn phí các tính năng cơ bản và trả phí để mở khóa tính năng nâng cao.",
                Citation = "Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 5: Business Model: Freemium with B2C subscriptions and B2B school licensing.",
                SourceType = "Extracted", SourceFile = "03_Slide_Pitching_Goi_von_Startup_EdTech.pdf",
                Options = new() { ("A", "Freemium", true), ("B", "Charity", false), ("C", "Donation", false), ("D", "Barter", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_374",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "What metric does 'CAC' represent in startup growth slides?",
                Explanation = "'CAC' là viết tắt của 'Customer Acquisition Cost' (Chi phí để thu hút được một khách hàng mới).",
                Citation = "Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 6: Unit Economics: LTV (Lifetime Value) to CAC ratio.",
                SourceType = "Extracted", SourceFile = "03_Slide_Pitching_Goi_von_Startup_EdTech.pdf",
                Options = new() { ("A", "Customer Acquisition Cost", true), ("B", "Corporate Account Credit", false), ("C", "Cash Asset Calculation", false), ("D", "Client Allowance Charge", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_375",
                TopicId = 15, LevelId = 5, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: In a venture pitch deck, 'The Ask' slide specifies the exact amount of capital being raised and how the funds will be allocated across engineering, marketing, and expansion.",
                Explanation = "Đúng. Slide 'The Ask' (Kêu gọi vốn) nêu rõ số vốn cần huy động và kế hoạch phân bổ sử dụng vốn cụ thể.",
                Citation = "Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 10: The Ask: Capital requirements and use of funds allocation.",
                SourceType = "Extracted", SourceFile = "03_Slide_Pitching_Goi_von_Startup_EdTech.pdf",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BỔ SUNG CÁC CÂU HỎI PHỎNG VẤN VÀ GIAO TIẾP LÃNH ĐẠO (B2 - C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_376",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "How should a candidate answer the interview question: 'What is your greatest weakness?'",
                Explanation = "Chia sẻ một điểm yếu có thật nhưng không mang tính chí mạng, kèm theo các hành động cụ thể bạn đang chủ động rèn luyện để khắc phục.",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Chiến lược trả lời câu hỏi điểm yếu (Genuine weakness + Actionable improvement).",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "Claim that you have zero weaknesses and are completely perfect.", false), ("B", "Name a genuine, non-critical weakness and detail the proactive steps you are currently taking to improve it.", true), ("C", "Admit that you hate waking up early and dislike working in teams.", false), ("D", "Say you work too hard and care too much as a disguised humblebrag.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_377",
                TopicId = 18, LevelId = 4, QuestionType = "SapXep", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "Order the four stages of the STAR interview answer:\n(1) Actions taken by you\n(2) Situation context\n(3) Quantifiable results achieved\n(4) Task and responsibilities",
                Explanation = "Thứ tự: Situation (2) -> Task (4) -> Action (1) -> Result (3).",
                Citation = "Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Trình tự 4 bước của mô hình STAR.",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx",
                Options = new() { ("A", "(2) -> (4) -> (1) -> (3)", true), ("B", "(1) -> (2) -> (3) -> (4)", false), ("C", "(4) -> (2) -> (3) -> (1)", false), ("D", "(3) -> (1) -> (4) -> (2)", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_378",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "De",
                QuestionText = "At the end of an interview, the interviewer asks: 'Do you have any questions for us?' What is the best strategy?",
                Explanation = "Luôn chuẩn bị sẵn 2-3 câu hỏi sâu sắc về văn hóa công ty, thách thức của vị trí hoặc định hướng công nghệ để thể hiện sự quan tâm thực sự.",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Phần kết thúc phỏng vấn: Đặt câu hỏi cho nhà tuyển dụng.",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "Say 'No, I just want to know when my salary arrives.'", false), ("B", "Ask thoughtful questions about the team's engineering culture, upcoming challenges, and growth opportunities.", true), ("C", "Ask nothing and leave immediately.", false), ("D", "Ask personal questions about the interviewer's family.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_379",
                TopicId = 15, LevelId = 5, QuestionType = "TuLuan", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "Speaking Task Prompt (STAR Interview Simulation):\n'Describe a time when you had to resolve a high-pressure conflict within your engineering team. What actions did you take, and what was the outcome?'\nStructure your 2-minute response explicitly following the STAR methodology.",
                Explanation = "Tiêu chí chấm: Cấu trúc rõ ràng 4 phần (S-T-A-R), sử dụng từ vựng giải quyết xung đột (active listening, mediation, consensus), kết quả hòa giải thành công và giữ vững tiến độ dự án.",
                Citation = "Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Tình huống mẫu: Xung đột nhóm (Team Conflict Resolution).",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx",
                Options = new() { ("RUBRIC", "Situation & Task (25%), Action with diplomatic initiative (50%), Result & Lesson learned (25%)", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_380",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "What does the presentation acronym 'KISS' stand for?",
                Explanation = "'Keep It Simple, Stupid' (hoặc Keep It Short and Simple) nhắc nhở việc trình bày súc tích, tránh đưa thông tin rườm rà phức tạp.",
                Citation = "Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Nguyên tắc KISS trong thiết kế thông điệp thuyết trình.",
                SourceType = "Extracted", SourceFile = "01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx",
                Options = new() { ("A", "Keep It Short and Simple (or Keep It Simple, Stupid)", true), ("B", "Knowledge Is Super Special", false), ("C", "Key Information System Software", false), ("D", "Kindness Is So Sweet", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_381",
                TopicId = 18, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'Her strong interpersonal skills allowed her to build immediate _______ with interviewers.'",
                Explanation = "'rapport' nghĩa là mối liên kết hòa hợp, thiện cảm tự nhiên giữa người với người.",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Nghệ thuật tạo thiện cảm trong phỏng vấn: Building rapport.",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "rapport", true), ("B", "wall", false), ("C", "barrier", false), ("D", "conflict", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_382",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "When facing an audience question that criticizes your startup's competitor analysis, which stance is most professional?",
                Explanation = "Thừa nhận thế mạnh của đối thủ một cách khách quan, sau đó làm nổi bật điểm khác biệt cốt lõi (unique differentiator) của sản phẩm mình.",
                Citation = "Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Ứng xử trước câu hỏi so sánh đối thủ cạnh tranh.",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx",
                Options = new() { ("A", "Insult the competitor and call their product garbage.", false), ("B", "Acknowledge the competitor's established presence respectfully, then articulate your unique value differentiator and proprietary technology.", true), ("C", "Pretend the competitor does not exist.", false), ("D", "Refuse to comment on anything related to the market.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_383",
                TopicId = 18, LevelId = 4, QuestionType = "DungSai", Skill = "Noi", Difficulty = "De",
                QuestionText = "True or False: Using mock interviews with a peer or recording your video answers helps identify unconscious speech fillers like 'um', 'ah', and 'like'.",
                Explanation = "Đúng. Luyện tập phỏng vấn thử (mock interviews) và quay video lại giúp nhận biết và loại bỏ các từ đệm vô nghĩa (filler words).",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Luyện tập thực tế: Loại bỏ Filler words.",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_384",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "In investor terminology, what is 'Runway'?",
                Explanation = "'Runway' (Đường băng tài chính) là số tháng mà một công ty khởi nghiệp có thể tiếp tục hoạt động với số tiền mặt hiện có trước khi hết vốn.",
                Citation = "Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Chỉ số tài chính: Runway (thời gian hoạt động còn lại).",
                SourceType = "Extracted", SourceFile = "03_Slide_Pitching_Goi_von_Startup_EdTech.pdf",
                Options = new() { ("A", "An airport concrete strip for airplanes", false), ("B", "The number of months a startup can operate before running out of money, calculated as cash reserves divided by monthly burn rate", true), ("C", "A fashion catwalk model stage", false), ("D", "The length of optical fiber cables", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_385",
                TopicId = 18, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Rewrite the task description concisely: 'My boss told me that I had to fix the broken login screen.'",
                Explanation = "'I was tasked with resolving critical authentication failures on the user login portal.' diễn đạt chuyên nghiệp và sắc sảo.",
                Citation = "Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Cách diễn đạt phần Task trong STAR.",
                SourceType = "Applied", SourceFile = "02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx",
                Options = new() { ("A", "I was tasked with resolving critical authentication failures on the user login portal.", true), ("B", "Boss ordered me to do screens.", false), ("C", "Screen was bad and my boss hated it.", false), ("D", "I had task of clicking buttons.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_386",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "What is the 'Rule of Three' in rhetoric and presentation design?",
                Explanation = "'Quy tắc số ba' (Rule of Three) dựa trên nguyên lý tâm lý học: Bộ não con người ghi nhớ thông tin tốt nhất khi được phân thành 3 cụm ý tưởng, 3 lợi ích hoặc 3 ví dụ.",
                Citation = "Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Quy tắc số ba (Rule of Three) trong thuật hùng biện.",
                SourceType = "Extracted", SourceFile = "01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx",
                Options = new() { ("A", "Speaking for exactly three seconds", false), ("B", "Grouping concepts, benefits, or examples into sets of three for maximum cognitive retention and rhetorical impact", true), ("C", "Wearing three layers of jackets", false), ("D", "Inviting three people on stage", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_387",
                TopicId = 18, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Complete the STAR Result statement: 'Our redesign led to a 35% increase in user _______.'",
                Explanation = "'retention' (tỷ lệ giữ chân người dùng) là chỉ số sản phẩm then chốt.",
                Citation = "Tài liệu 02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx - Các chỉ số đo lường kết quả công nghệ: User retention, engagement.",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_STAR_tra_loi_cau_hoi_hanh_vi.docx",
                Options = new() { ("A", "retention", true), ("B", "rejection", false), ("C", "refusal", false), ("D", "reluctance", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_388",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What is an 'Elevator Pitch' in startup capital fundraising?",
                Explanation = "Bài giới thiệu siêu ngắn (30-60 giây) tóm tắt trọn vẹn vấn đề, giải pháp và giá trị cốt lõi của startup, đủ nhanh để trình bày trong một chuyến đi thang máy với nhà đầu tư.",
                Citation = "Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Khái niệm Elevator Pitch.",
                SourceType = "Extracted", SourceFile = "03_Slide_Pitching_Goi_von_Startup_EdTech.pdf",
                Options = new() { ("A", "A mechanical inspection of elevators", false), ("B", "A concise 30-to-60-second summary that articulates a venture's core value proposition so compellingly that investors want to learn more", true), ("C", "A speech given only in high-rise buildings", false), ("D", "A conversation with elevator technicians", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_389",
                TopicId = 18, LevelId = 4, QuestionType = "DungSai", Skill = "Noi", Difficulty = "De",
                QuestionText = "True or False: Maintaining steady, natural eye contact with all interviewers on a panel (rather than looking solely at the most senior person) conveys inclusivity and confidence.",
                Explanation = "Đúng. Duy trì giao tiếp mắt đều khắp với tất cả các thành viên trong hội đồng phỏng vấn thể hiện sự tự tin và tôn trọng toàn diện.",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Giao tiếp phi ngôn ngữ trong phỏng vấn hội đồng (Panel interview).",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_390",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "Which concluding statement effectively closes an executive investment pitch?",
                Explanation = "'We invite you to join us in revolutionizing personalized language education. Thank you, and we welcome your questions.' là lời kết thúc tự tin và chuyên nghiệp.",
                Citation = "Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Slide 10: Closing statement and call to partnership.",
                SourceType = "Extracted", SourceFile = "03_Slide_Pitching_Goi_von_Startup_EdTech.pdf",
                Options = new() { ("A", "That's all, give us money if you want.", false), ("B", "We invite you to partner with us in revolutionizing global language education; thank you, and we look forward to your questions.", true), ("C", "I'm exhausted, no more talking.", false), ("D", "Bye everyone.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_391",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What does the interview idiom 'hit the ground running' mean?",
                Explanation = "'Hit the ground running' nghĩa là bắt nhịp làm việc hiệu quả ngay từ ngày đầu tiên mà không mất thời gian hòa nhập chậm chạp.",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Thành ngữ phỏng vấn tuyển dụng: Hit the ground running.",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "To trip and fall on the pavement", false), ("B", "To start a new job or project enthusiastically, competently, and with immediate productivity", true), ("C", "To run in a marathon race", false), ("D", "To dig a hole in the ground", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_392",
                TopicId = 15, LevelId = 4, QuestionType = "DienKhuyet", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'To keep audience engagement high, presenters should vary their vocal _______ (pitch, tone, and pacing).'",
                Explanation = "'variety' (sự đa dạng ngữ điệu giọng nói): vocal variety.",
                Citation = "Tài liệu 01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx - Kỹ thuật điều khiển giọng nói (Vocal variety).",
                SourceType = "Extracted", SourceFile = "01_Khai_cuoc_bai_thuyet_trinh_Hook_the_Audience.docx",
                Options = new() { ("A", "variety", true), ("B", "silence", false), ("C", "damage", false), ("D", "shouting", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_393",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "In technical interviews, what is a 'Whiteboard Coding Challenge' designed to assess?",
                Explanation = "'Thử thách code bảng' kiểm tra tư duy thuật toán, kỹ năng giao tiếp truyền đạt giải pháp và cách ứng phó dưới áp lực, chứ không chỉ là cú pháp.",
                Citation = "Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Chuẩn bị phỏng vấn kỹ thuật: Whiteboard coding challenges.",
                SourceType = "Extracted", SourceFile = "04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf",
                Options = new() { ("A", "Penmanship and whiteboard cleanliness", false), ("B", "Algorithmic problem-solving, real-time communication of thought processes, and composure under pressure", true), ("C", "Drawing artistic sketches", false), ("D", "Speed reading abilities", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_394",
                TopicId = 15, LevelId = 5, QuestionType = "DungSai", Skill = "Noi", Difficulty = "De",
                QuestionText = "True or False: Using the speaker's name when answering a question from the audience (e.g., 'Thank you for that question, John') builds personal rapport and commands executive respect.",
                Explanation = "Đúng. Xưng hô tên người hỏi thể hiện sự chú ý lắng nghe và tác phong ngoại giao lịch thiệp.",
                Citation = "Tài liệu 02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx - Tương tác cá nhân hóa trong phần Q&A.",
                SourceType = "Extracted", SourceFile = "02_Ky_thuat_xu_ly_cau_hoi_QnA_va_Phan_bien.docx",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_395",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "Which response appropriately addresses why a candidate wants to leave their current employer?",
                Explanation = "Tập trung vào cơ hội phát triển nghề nghiệp mới thay vì chỉ trích tiêu cực về sếp hoặc công ty cũ.",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Trả lời lý do chuyển việc: Tập trung vào Career growth thay vì than phiền.",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "My current manager is toxic and I cannot stand my colleagues.", false), ("B", "I have had valuable growth at my current company, and I am now ready for a new challenge where I can apply my cloud architecture expertise to larger scale systems.", true), ("C", "The commute is five minutes longer than I like.", false), ("D", "I just want higher pay with less work.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_396",
                TopicId = 15, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Complete the startup valuation term: 'The valuation agreed upon before receiving new investment capital is the _______-money valuation.'",
                Explanation = "'Pre-money valuation' là định giá doanh nghiệp trước khi nhận dòng vốn mới (khác với Post-money valuation = Pre-money + Vốn đầu tư).",
                Citation = "Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Thuật ngữ gọi vốn: Pre-money vs. Post-money valuation.",
                SourceType = "Extracted", SourceFile = "03_Slide_Pitching_Goi_von_Startup_EdTech.pdf",
                Options = new() { ("A", "Pre", true), ("B", "Post", false), ("C", "Mid", false), ("D", "After", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_397",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What is a 'Cover Letter' submitted alongside a job resume?",
                Explanation = "'Cover Letter' (Thư ứng tuyển) là bức thư ngắn 1 trang giới thiệu bản thân, giải thích lý do bạn quan tâm đến vị trí và nêu bật các phẩm chất phù hợp nhất.",
                Citation = "Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Hồ sơ ứng tuyển hoàn chỉnh: Resume + Cover Letter.",
                SourceType = "Extracted", SourceFile = "04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf",
                Options = new() { ("A", "A picture of the applicant's house", false), ("B", "A personalized formal letter accompanying a resume explaining why you are the ideal fit for the role", true), ("C", "A receipt from university tuition payments", false), ("D", "A blank document with a signature only", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_398",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "What is the primary objective of a 'Call to Action' (CTA) at the end of a product pitch?",
                Explanation = "Kêu gọi hành động (CTA) hướng dẫn khách hàng hoặc nhà đầu tư thực hiện bước đi cụ thể tiếp theo (ví dụ: đăng ký bản dùng thử, hẹn gặp trực tiếp thẩm định tài chính).",
                Citation = "Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Thiết kế Call to Action (CTA) trong Pitch Deck.",
                SourceType = "Extracted", SourceFile = "03_Slide_Pitching_Goi_von_Startup_EdTech.pdf",
                Options = new() { ("A", "To confuse the audience so they stay longer", false), ("B", "To clearly instruct investors or clients on the exact next step to take (e.g., schedule a due-diligence meeting or sign up for a pilot)", true), ("C", "To ask for free lunch vouchers", false), ("D", "To show funny internet memes", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_399",
                TopicId = 18, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: Sending a polite thank-you email within 24 hours of completing a job interview reinforces your interest and leaves a lasting professional impression.",
                Explanation = "Đúng. Thư cảm ơn (Thank-you note) gửi trong vòng 24 giờ sau buổi phỏng vấn là nghi thức tuyển dụng quốc tế rất được đánh giá cao.",
                Citation = "Tài liệu 01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx - Nghi thức sau phỏng vấn: Post-interview Thank You email trong 24h.",
                SourceType = "Extracted", SourceFile = "01_Tra_loi_cau_hoi_kinh_dien_Tell_Me_About_Yourself.docx",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_400",
                TopicId = 15, LevelId = 5, QuestionType = "TuLuan", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "Speaking Task Prompt (Elevator Pitch Simulation):\n'Deliver a 60-second elevator pitch introducing an innovative AI educational tool to an angel investor. Highlight the core problem, your technological solution, and the market potential.'\nProvide a scripted 120-word pitch demonstration.",
                Explanation = "Tiêu chí chấm: Cấu trúc Hook ấn tượng (15s), nêu giải pháp AI cốt lõi (30s) và lời kêu gọi đầu tư mạnh mẽ (15s).",
                Citation = "Tài liệu 03_Slide_Pitching_Goi_von_Startup_EdTech.pdf - Bài tập thực hành thuyết trình gọi vốn 60 giây.",
                SourceType = "Extracted", SourceFile = "03_Slide_Pitching_Goi_von_Startup_EdTech.pdf",
                Options = new() { ("RUBRIC", "Hook & Problem (25%), Solution & Uniqueness (50%), Business Model & CTA (25%)", true) }
            });

            return list;
        }
    }
}

