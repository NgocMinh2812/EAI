using System.Collections.Generic;

namespace QuestionBuilder
{
    public static class Batch10
    {
        public static List<QuestionItem> GetQuestions()
        {
            var list = new List<QuestionItem>();

            // ====================================================================================
            // 1. TÀI LIỆU: 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx (Questions 451-470)
            // ====================================================================================

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_451",
                TopicId = 19, LevelId = 5, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Invert the sentence starting with 'No sooner': 'As soon as the CEO stepped into the conference hall, the journalists bombarded him with inquiries.'",
                Explanation = "Cấu trúc đảo ngữ: 'No sooner had + S + V3/ed + than + S + V2/ed'.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 1: Đảo ngữ với No sooner ... than và Hardly ... when.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "No sooner had the CEO stepped into the conference hall than the journalists bombarded him with inquiries.", true), ("B", "No sooner the CEO had stepped into the conference hall when the journalists bombarded him.", false), ("C", "No sooner did the CEO stepped into the conference hall than the journalists bombarded him.", false), ("D", "No sooner had the CEO stepped into the hall when journalists did bombard him.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_452",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Choose the correct inversion form: 'Under no circumstances _______ the confidential cryptographic keys to any third party.'",
                Explanation = "Sau cụm từ phủ định 'Under no circumstances' (dù trong bất cứ trường hợp nào cũng không), ta đảo trợ động từ lên trước chủ ngữ: 'should you disclose'.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 2: Đảo ngữ với cụm từ phủ định tuyệt đối (Under no circumstances, On no account).",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "you should disclose", false), ("B", "should you disclose", true), ("C", "you will disclose", false), ("D", "do you must disclose", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_453",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Hardly had the server system rebooted _______ a fresh wave of DDoS attacks overwhelmed the firewall.",
                Explanation = "Cấu trúc tương liên: 'Hardly had + S + V3/ed + WHEN + S + V2/ed'.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Phân biệt cặp liên từ: Hardly/Scarcely... WHEN đối chiếu với No sooner... THAN.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "when", true), ("B", "than", false), ("C", "then", false), ("D", "that", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_454",
                TopicId = 19, LevelId = 5, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Invert the conditional sentence: 'If the board of directors were to reject our proposal, we would pursue venture debt.'",
                Explanation = "Đảo ngữ câu điều kiện loại 2 với 'Were': 'Were the board of directors to reject our proposal, we would pursue venture debt.'",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 3: Đảo ngữ câu điều kiện (Should / Were / Had).",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Were the board of directors to reject our proposal, we would pursue venture debt.", true), ("B", "Were the board reject our proposal, we would pursue venture debt.", false), ("C", "If were the board of directors rejecting our proposal, we pursue venture debt.", false), ("D", "Did the board of directors reject our proposal, we would pursue venture debt.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_455",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Which sentence correctly illustrates inversion with 'Seldom'?",
                Explanation = "'Seldom have I witnessed such profound resilience in the face of insurmountable economic hardship.' (Đảo trợ động từ 'have' lên trước 'I').",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 4: Đảo ngữ với phó từ tần suất bán phủ định (Seldom, Rarely, Little).",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Seldom I have witnessed such profound resilience in the face of hardship.", false), ("B", "Seldom have I witnessed such profound resilience in the face of insurmountable economic hardship.", true), ("C", "Seldom did I witnessed such profound resilience in the face of hardship.", false), ("D", "Seldom I witnessed such profound resilience.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_456",
                TopicId = 19, LevelId = 5, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Find the error: 'Not until the audit report was published (A) last week the shareholders realized (B) the sheer extent (C) of corporate mismanagement (D).'",
                Explanation = "Lỗi ở (B) 'the shareholders realized'. Sau mệnh đề thời gian 'Not until...', mệnh đề chính phải đảo ngữ: 'did the shareholders realize'.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 5: Đảo ngữ với 'Not until' và vị trí đảo ngữ ở mệnh đề chính.",
                SourceType = "Applied", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "was published", false), ("B", "the shareholders realized", true), ("C", "the sheer extent", false), ("D", "of corporate mismanagement", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_457",
                TopicId = 19, LevelId = 5, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Invert using 'Should': 'If you require any further documentation regarding the offshore investment, please contact compliance.'",
                Explanation = "Đảo ngữ điều kiện loại 1: 'Should you require any further documentation regarding the offshore investment, please contact compliance.'",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ điều kiện loại 1 với 'Should'.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Should you require any further documentation regarding the offshore investment, please contact compliance.", true), ("B", "Should you requiring any further documentation, contact compliance.", false), ("C", "If should you require documentation, contact compliance.", false), ("D", "Should you to require further documentation, contact compliance.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_458",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Complete the inverted sentence: 'So intense _______ that all scheduled flights were grounded indefinitely.'",
                Explanation = "Cấu trúc: 'So + Adj + be + S + that...': 'So intense was the blizzard that all scheduled flights were grounded indefinitely.'",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 6: Đảo ngữ nhấn mạnh tính từ với So ... that và Such ... that.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "the blizzard was", false), ("B", "was the blizzard", true), ("C", "did the blizzard be", false), ("D", "the blizzard had been", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_459",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Little _______ the lead developer suspect that a tiny memory leak would crash the core production database during peak hours.",
                Explanation = "Sau 'Little' mang nghĩa phủ định, dùng đảo ngữ thời quá khứ đơn: 'Little did the lead developer suspect...'.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ với 'Little did + S + V'.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "did", true), ("B", "does", false), ("C", "was", false), ("D", "had", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_460",
                TopicId = 19, LevelId = 5, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Invert using 'Such': 'The chaos caused by the power outage was so severe that emergency protocols were enacted.'",
                Explanation = "Cấu trúc: 'Such + be + S + that...': 'Such was the chaos caused by the power outage that emergency protocols were enacted.'",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Cấu trúc đảo ngữ Such was... that...",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Such was the chaos caused by the power outage that emergency protocols were enacted.", true), ("B", "Such the chaos was that emergency protocols were enacted.", false), ("C", "Such did the chaos caused that emergency protocols were enacted.", false), ("D", "Such was chaos that it caused power outage.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_461",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "'Only after conducting an exhaustive forensic analysis _______ the root cause of the network breach.'",
                Explanation = "Sau 'Only after + clause/phrase', mệnh đề sau phải đảo trợ động từ: 'did the cybersecurity team pinpoint'.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 7: Đảo ngữ với Only after / Only when.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "the cybersecurity team pinpointed", false), ("B", "did the cybersecurity team pinpoint", true), ("C", "pinpointed the cybersecurity team", false), ("D", "the team had pinpointed", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_462",
                TopicId = 19, LevelId = 5, QuestionType = "DungSai", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "True or False: In conditional inversion type 3 ('Had + S + V3'), the word 'if' must be preserved immediately before 'Had'.",
                Explanation = "Sai (False). Khi thực hiện đảo ngữ câu điều kiện, từ 'if' bắt buộc phải bị lược bỏ hoàn toàn (e.g. 'Had I known', KHÔNG dùng 'If had I known').",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Quy tắc lược bỏ 'If' khi đảo ngữ câu điều kiện.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_463",
                TopicId = 19, LevelId = 5, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Find the mistake: 'On no account (A) passengers should attempt (B) to disembark before (C) the aircraft has come to a complete standstill (D).'",
                Explanation = "Lỗi ở (B) 'passengers should attempt'. Sau 'On no account' (tuyệt đối không), phải đảo ngữ: 'should passengers attempt'.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ bắt buộc với On no account.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "On no account", false), ("B", "passengers should attempt", true), ("C", "before", false), ("D", "complete standstill", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_464",
                TopicId = 19, LevelId = 5, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Invert using 'Not only': 'The new quantum processor performs calculations at unprecedented speeds, and it also consumes 40% less electrical power.'",
                Explanation = "'Not only does the new quantum processor perform calculations at unprecedented speeds, but it also consumes 40% less electrical power.'",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 8: Đảo ngữ với Not only... but also.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Not only does the new quantum processor perform calculations at unprecedented speeds, but it also consumes 40% less electrical power.", true), ("B", "Not only the quantum processor performs calculations, but also consumes power.", false), ("C", "Not only did the processor performed calculations, but it consumes power.", false), ("D", "Not only performs the processor calculations, but it also consumes.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_465",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "'At no time _______ aware of the fraudulent transactions taking place inside the branch.'",
                Explanation = "Sau cụm phủ định 'At no time', ta đảo trợ động từ to be: 'was the manager aware'.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ với At no time.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "the manager was", false), ("B", "was the manager", true), ("C", "did the manager", false), ("D", "the manager had been", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_466",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Only by mastering prompt architecture and algorithmic foundations _______ software engineers thrive in the era of generative AI.",
                Explanation = "'can' (sau 'Only by...', ta đảo động từ khuyết thiếu 'can software engineers thrive').",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ với Only by.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "can", true), ("B", "did", false), ("C", "are", false), ("D", "have", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_467",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Which sentence showcases correct locative inversion (đảo ngữ chỉ nơi chốn/hướng di chuyển)?",
                Explanation = "'Along the cobblestone avenue stood rows of centuries-old neoclassical buildings.' (Đưa cụm trạng từ chỉ nơi chốn lên đầu câu, đảo trực tiếp nội động từ 'stood' lên trước chủ ngữ).",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mục 9: Đảo ngữ toàn bộ với trạng từ chỉ địa điểm/phương hướng (Locative Inversion).",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Along the cobblestone avenue rows of buildings stood.", false), ("B", "Along the cobblestone avenue stood rows of centuries-old neoclassical buildings.", true), ("C", "Along the avenue did stand rows of buildings.", false), ("D", "Along the avenue was standing rows of buildings.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_468",
                TopicId = 19, LevelId = 5, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Invert using 'Had': 'If we had recognized the algorithmic bias earlier, the model deployment would not have triggered public controversy.'",
                Explanation = "'Had we recognized the algorithmic bias earlier, the model deployment would not have triggered public controversy.'",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ điều kiện loại 3.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Had we recognized the algorithmic bias earlier, the model deployment would not have triggered public controversy.", true), ("B", "Had we recognize the algorithmic bias earlier, model deployment would not trigger controversy.", false), ("C", "If had we recognized the bias earlier, deployment would not trigger controversy.", false), ("D", "Did we have recognized the bias earlier, controversy would not happen.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_469",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "'Scarcely _______ the opening speech when the electrical grid suffered a sudden blackout.'",
                Explanation = "Cấu trúc: 'Scarcely had + S + V3/ed + when + S + V2/ed': 'Scarcely had the keynote speaker concluded'.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Cấu trúc Scarcely had ... when.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "the keynote speaker had concluded", false), ("B", "had the keynote speaker concluded", true), ("C", "concluded the keynote speaker", false), ("D", "the keynote speaker concluded", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_470",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Rarely _______ an academic paper provoke such passionate discourse among international scholars.",
                Explanation = "'does' (đảo trợ động từ hiện tại đơn: 'Rarely does an academic paper provoke...').",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ với Rarely ở thì hiện tại đơn.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "does", true), ("B", "is", false), ("C", "has", false), ("D", "was", false) }
            });

            // ====================================================================================
            // 2. C1 IDIOMS, COLLOCATIONS & ADVANCED LEXIS (Questions 471-485)
            // ====================================================================================

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_471",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho", LessonId = 35,
                QuestionText = "After months of delaying the painful restructuring, the CEO finally decided to 'bite the bullet'. What does this idiom mean?",
                Explanation = "'Bite the bullet' có nghĩa là can đảm đưa ra quyết định hoặc đối mặt với một tình huống đau đớn, khó khăn mà mình từng né tránh.",
                Citation = "Bài học 35: Thành ngữ nâng cao C1 & Quản trị kinh doanh - Cụm từ 'Bite the bullet'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_35_Thanh_ngu_Nghe_thuat.md",
                Options = new() { ("A", "To face a painful or difficult situation with fortitude after prolonging it", true), ("B", "To purchase weapons for security defense", false), ("C", "To cancel all future board meetings", false), ("D", "To invest capital in pharmaceutical startups", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_472",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho", LessonId = 35,
                QuestionText = "The final research deadline was tight, forcing the entire laboratory staff to burn the midnight _______.",
                Explanation = "'burn the midnight oil' (thức khuya làm việc hoặc học tập cật lực).",
                Citation = "Bài học 35: Thành ngữ tiếng Anh C1 - 'Burn the midnight oil'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_35_Thanh_ngu_Nghe_thuat.md",
                Options = new() { ("A", "oil", true), ("B", "lamp", false), ("C", "candle", false), ("D", "fuel", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_473",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "When evaluating anonymous political commentary, wise analysts always 'take it with a grain of salt'. This implies:",
                Explanation = "'Take with a grain of salt' có nghĩa là tiếp nhận thông tin với thái độ hoài nghi, dè dặt, không tin tưởng hoàn toàn 100%.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Phụ lục Thành ngữ tư duy phản biện C1.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Accepting the claim with healthy skepticism rather than literal unquestioning belief", true), ("B", "Adding sodium to one's diet for cognitive health", false), ("C", "Rejecting every single proposition without reading", false), ("D", "Memorizing every single word exactly", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_474",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Her meticulous analysis really 'hit the nail on the head'. What did she do?",
                Explanation = "'Hit the nail on the head' nghĩa là nói trúng phóc, miêu tả chính xác tuyệt đối nguyên nhân hoặc bản chất của vấn đề.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Thành ngữ diễn đạt sự chính xác tuyệt đối.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Described the exact cause or truth of a situation with precision", true), ("B", "Damaged the carpentry equipment during construction", false), ("C", "Made an irrelevant comment that confused everyone", false), ("D", "Failed to identify the root cause of the crisis", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_475",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "A diplomatic statement often requires readers to read between the _______ to perceive subtle underlying meanings.",
                Explanation = "'read between the lines' (hiểu được ẩn ý ngầm đằng sau câu từ).",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Kỹ năng đọc hiểu C1: Reading between the lines.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "lines", true), ("B", "words", false), ("C", "pages", false), ("D", "letters", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_476",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Artificial intelligence is often described as a 'double-edged sword' because:",
                Explanation = "'A double-edged sword' (con dao hai lưỡi) có nghĩa là một giải pháp hay công nghệ vừa mang lại lợi ích to lớn nhưng đồng thời cũng tiềm ẩn những rủi ro hoặc tác hại khôn lường.",
                Citation = "Bài học 37: Đọc hiểu Nâng cao: Đột phá Trí tuệ Nhân tạo - Khái niệm Double-edged Sword trong công nghệ.",
                SourceType = "Extracted", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "It brings extraordinary economic benefits while simultaneously posing profound ethical and existential hazards", true), ("B", "It is manufactured using sharp metallic semiconductors", false), ("C", "It cannot be utilized on modern mobile smartphones", false), ("D", "It is twice as cheap as conventional software systems", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_477",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "The initial detection of accounting discrepancies was merely the tip of the _______; billions in unrecorded debt remained concealed.",
                Explanation = "'the tip of the iceberg' (chỉ là bề nổi của tảng băng chìm).",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Thành ngữ kinh doanh C1: The tip of the iceberg.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "iceberg", true), ("B", "mountain", false), ("C", "volcano", false), ("D", "pyramid", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_478",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "The detective vowed to 'leave no stone unturned' in the search for physical evidence. This signifies:",
                Explanation = "'Leave no stone unturned' có nghĩa là làm tất cả mọi cách, rà soát mọi khả năng có thể để đạt được mục tiêu.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Thành ngữ C1 diễn tả sự nỗ lực toàn diện triệt để.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Trying every possible course of action or looking everywhere meticulously", true), ("B", "Collecting rare geological rocks from caves", false), ("C", "Renovating residential gardens using stones", false), ("D", "Ignoring the crime scene completely", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_479",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Which advanced C1 collocation correctly completes: 'The government's new fiscal austerity policy _______ far short of the public's expectations.'?",
                Explanation = "'fell short of expectations' (không đạt được như kỳ vọng mong đợi).",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - C1 Collocations: 'Fall short of expectations'.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "dropped", false), ("B", "fell", true), ("C", "cut", false), ("D", "sunk", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_480",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "When evaluating research integrity, one must bear in _______ that correlation does not imply causation.",
                Explanation = "'bear in mind' (ghi nhớ, khắc sâu trong tâm trí).",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Cụm từ học thuật C1: 'Bear in mind'.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "mind", true), ("B", "head", false), ("C", "heart", false), ("D", "brain", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_481",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "In clinical trials, patient safety must take _______ over commercial profit deadlines.",
                Explanation = "'take precedence over' (được ưu tiên hàng đầu, xếp trên những thứ khác).",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - C1 Formal Collocations: 'Take precedence over'.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "precedence", true), ("B", "preference", false), ("C", "presence", false), ("D", "presumption", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_482",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "After years of theoretical research, their groundbreaking quantum computing prototypes finally came to _______.",
                Explanation = "'came to fruition' (đạt được thành quả thực tế, kết trái ngọt sau bao nỗ lực).",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Cụm từ C1: 'Come to fruition'.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "fruition", true), ("B", "fruit", false), ("C", "fulfillment", false), ("D", "fortune", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_483",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Which idiomatic phrase means to reveal a secret prematurely, often inadvertently ruining a surprise?",
                Explanation = "'Spill the beans' nghĩa là vô tình hay thiếu thận trọng tiết lộ bí mật.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - C1 Idioms: 'Spill the beans'.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Spill the beans", true), ("B", "Bite the bullet", false), ("C", "Burn the candle", false), ("D", "Break the ice", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_484",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Despite intense pressure from investors, the visionary founder refused to throw in the _______ and continued developing the autonomous rover.",
                Explanation = "'throw in the towel' (bỏ cuộc, đầu hàng).",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Thành ngữ 'Throw in the towel'.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "towel", true), ("B", "sponge", false), ("C", "sheet", false), ("D", "blanket", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_485",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "When two negotiating delegations do not 'see eye to eye' on carbon tariffs, they:",
                Explanation = "'See eye to eye' có nghĩa là đồng thuận quan điểm; 'not see eye to eye' là bất đồng ý kiến.",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Thành ngữ đàm phán: 'See eye to eye'.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Disagree with each other regarding the proposed terms", true), ("B", "Wear protective eyewear during diplomatic talks", false), ("C", "Refuse to look at the contract papers", false), ("D", "Immediately sign the treaty without reading", false) }
            });

            // ====================================================================================
            // 3. VSTEP BẬC 4 & 5 (B2-C1) READING & ADVANCED SYNTAX (BaiHoc 37) (Questions 486-500)
            // ====================================================================================

            string aiReadingPassage = "The ascendancy of artificial general intelligence (AGI) represents a profound paradigm shift in human history. " +
                "While proponents accentuate unprecedented productivity leaps in scientific discovery and biomedical automation, skeptics articulate legitimate apprehensions regarding cognitive atrophy and algorithmic bias. " +
                "Empirical studies indicate that over-reliance on generative models diminishes critical inquiry and independent problem-solving among university cohorts. " +
                "Furthermore, socio-economic stratification risks exacerbation as capital accrues disproportionately to technology conglomerates orchestrating proprietary foundational models. " +
                "Consequently, international regulatory bodies advocate for stringent algorithmic transparency and human-in-the-loop oversight before automated decision systems govern criminal justice, healthcare triage, or financial credit scoring.";

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_486",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                ReadingPassage = aiReadingPassage,
                QuestionText = "According to the passage, what unintended consequence of generative AI usage has emerged among university students?",
                Explanation = "Nghiên cứu thực nghiệm chỉ ra sự phụ thuộc quá mức vào AI tạo sinh làm suy giảm khả năng tư duy phản biện và giải quyết vấn đề độc lập của sinh viên ('diminishes critical inquiry and independent problem-solving').",
                Citation = "Bài học 37: Đọc hiểu Nâng cao: Đột phá Trí tuệ Nhân tạo & Tác động Xã hội - Đoạn 1: Tác động của AI đối với nhận thức người học.",
                SourceType = "Extracted", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "Complete abandonment of digital electronic hardware", false), ("B", "Reduction in critical inquiry and autonomous problem-solving capabilities", true), ("C", "Substantial increase in handwriting speed", false), ("D", "Universal mastery of multi-lingual programming languages", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_487",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                ReadingPassage = aiReadingPassage,
                QuestionText = "The term 'ascendancy' in the first sentence is closest in meaning to:",
                Explanation = "'Ascendancy' mang nghĩa sự trỗi dậy, vị thế thống trị hoặc vươn lên dẫn đầu ('dominance' / 'rise to power').",
                Citation = "Bài học 37: Phân tích từ vựng học thuật C1 trong văn cảnh công nghệ.",
                SourceType = "Extracted", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "Decline", false), ("B", "Dominance", true), ("C", "Confusion", false), ("D", "Stagnation", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_488",
                TopicId = 19, LevelId = 5, QuestionType = "DungSai", Skill = "Doc", Difficulty = "Kho",
                ReadingPassage = aiReadingPassage,
                QuestionText = "True or False: The author argues that automated decision systems should govern criminal justice without any human intervention.",
                Explanation = "Sai (False). Tác giả nêu rõ các cơ quan quản lý quốc tế yêu cầu phải có sự giám sát của con người ('human-in-the-loop oversight') trước khi AI được ứng dụng trong tư pháp hình sự.",
                Citation = "Bài học 37: Vai trò của con người trong chu trình AI (Human-in-the-loop).",
                SourceType = "Extracted", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_489",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                ReadingPassage = aiReadingPassage,
                QuestionText = "What potential societal risk is highlighted regarding tech conglomerates in the text?",
                Explanation = "Sự bất bình đẳng kinh tế - xã hội có nguy cơ trầm trọng hơn do lợi nhuận và tư bản dồn phần lớn vào các tập đoàn công nghệ nắm giữ mô hình nền tảng.",
                Citation = "Bài học 37: Phân tầng kinh tế xã hội và độc quyền công nghệ.",
                SourceType = "Extracted", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "Deepening socio-economic stratification due to monopolized capital concentration", true), ("B", "Imminent bankruptcy of all major software corporations", false), ("C", "An oversupply of hardware microchips worldwide", false), ("D", "Complete disappearance of venture capital investments", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_490",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                ReadingPassage = aiReadingPassage,
                QuestionText = "Fill in the blank: 'Regulatory bodies mandate that algorithmic _______ must be demonstrated so that stakeholders understand why automated models make specific decisions.'",
                Explanation = "'transparency' (tính minh bạch của thuật toán).",
                Citation = "Bài học 37: Nguyên tắc minh bạch thuật toán (Algorithmic Transparency).",
                SourceType = "Extracted", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "transparency", true), ("B", "opacity", false), ("C", "secrecy", false), ("D", "ambiguity", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_491",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                ReadingPassage = aiReadingPassage,
                QuestionText = "What is the primary tone of the passage regarding artificial intelligence?",
                Explanation = "Giọng văn mang tính phân tích, cân bằng và cảnh giác thận trọng (Analytic, measured, and cautiously critical).",
                Citation = "Bài học 37: Nhận diện thái độ và giọng điệu tác giả (Author's Tone and Stance).",
                SourceType = "Extracted", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "Analytic, objective, and cautiously critical", true), ("B", "Blindly optimistic and dismissive of hazards", false), ("C", "Purely satirical and mocking", false), ("D", "Hopeless, apocalyptic, and fatalistic", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_492",
                TopicId = 19, LevelId = 5, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Transform into the subjunctive mood: 'It is essential that governments regulate artificial intelligence deployment.'",
                Explanation = "Thức giả định hiện tại (Present Subjunctive) dùng động từ nguyên mẫu không 'to': 'It is essential that governments regulate artificial intelligence deployment.'",
                Citation = "Bài học 37: Cấu trúc thức giả định Subjunctive Mood trong văn bản quy phạm.",
                SourceType = "Applied", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "It is essential that governments regulate artificial intelligence deployment.", true), ("B", "It is essential that governments regulates AI deployment.", false), ("C", "It is essential that governments will regulate AI deployment.", false), ("D", "It is essential for governments to having regulated AI.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_493",
                TopicId = 19, LevelId = 5, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Identify the grammatical flaw: 'The ethics committee insisted that every AI engineer adheres (A) strictly to confidentiality protocols and submits (B) algorithmic models for peer review (C) prior to deployment (D).'",
                Explanation = "Lỗi ở (A) 'adheres' và (B) 'submits'. Sau cấu trúc 'insisted that...', mệnh đề phải dùng động từ nguyên mẫu theo thức giả định: 'adhere' và 'submit'.",
                Citation = "Bài học 37: Thức giả định Subjunctive Mood sau insist/recommend.",
                SourceType = "Applied", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "adheres", true), ("B", "submits", false), ("C", "for peer review", false), ("D", "prior to deployment", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_494",
                TopicId = 19, LevelId = 5, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Combine using a non-defining relative clause with preposition: 'The European Union enacted the AI Act. Under this legislation, high-risk biometric surveillance systems are strictly prohibited.'",
                Explanation = "'The European Union enacted the AI Act, under which high-risk biometric surveillance systems are strictly prohibited.'",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Mệnh đề quan hệ chứa giới từ (Preposition + Which).",
                SourceType = "Applied", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "The European Union enacted the AI Act, under which high-risk biometric surveillance systems are strictly prohibited.", true), ("B", "The European Union enacted the AI Act under that high-risk systems are prohibited.", false), ("C", "The European Union enacted the AI Act which under high-risk surveillance is prohibited.", false), ("D", "The European Union enacted the AI Act, whereunder systems was prohibited.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_495",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "When evaluating model robustness, adversarial testing seeks to identify systemic _______ in training datasets.",
                Explanation = "'biases' (thiên kiến, định kiến hệ thống).",
                Citation = "Bài học 37: Thiên kiến thuật toán và kiểm thử đối kháng (Algorithmic Bias).",
                SourceType = "Extracted", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "biases", true), ("B", "wires", false), ("C", "batteries", false), ("D", "keyboards", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_496",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "In cognitive psychology, 'cognitive atrophy' refers to:",
                Explanation = "'Cognitive atrophy' là hiện tượng thoái hóa chức năng tư duy, ghi nhớ hoặc kỹ năng nhận thức do lâu ngày không được vận dụng não bộ độc lập.",
                Citation = "Bài học 37: Thuật ngữ tâm lý nhận thức và tác động của tự động hóa lên não bộ.",
                SourceType = "Extracted", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "The gradual deterioration of intellectual faculties and problem-solving skills from lack of mental exercise", true), ("B", "A physical headache caused by staring at phone displays", false), ("C", "An immediate increase in mathematical computation speed", false), ("D", "The complete recovery of lost memory cells", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_497",
                TopicId = 19, LevelId = 5, QuestionType = "TuLuan", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Write a concise argumentative paragraph (80-120 words) analyzing whether generative AI should be permitted in academic university examinations.",
                Explanation = "Yêu cầu: Nêu rõ lập trường (đồng ý, phản đối hoặc kết hợp có kiểm soát), đưa ra ít nhất 2 luận cứ xác đáng (tính liêm chính học thuật vs chuẩn bị kỹ năng thực tế cho thị trường lao động), sử dụng liên từ mạch lạc và vốn từ vựng C1 chuẩn xác.",
                Citation = "Bài học 37: Chủ đề thảo luận nghị luận xã hội: Liêm chính học thuật trong thời đại AI.",
                SourceType = "Applied", SourceFile = "BaiHoc_37_Doc_hieu_Nang_cao_AI.md",
                Options = new() { ("A", "Model Rubric: Clear thesis statement, academic vocabulary (integrity, cognitive engagement, pedagogical efficacy), coherent discourse markers, balanced synthesis.", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_498",
                TopicId = 19, LevelId = 5, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Rewrite using 'No matter how': 'Although the regulatory guidelines are intricate, developers must adhere to compliance standards.'",
                Explanation = "'No matter how intricate the regulatory guidelines are, developers must adhere to compliance standards.'",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Cấu trúc nhượng bộ: No matter how + Adj + S + V.",
                SourceType = "Applied", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "No matter how intricate the regulatory guidelines are, developers must adhere to compliance standards.", true), ("B", "No matter what intricate guidelines are, developers must adhere.", false), ("C", "No matter how the regulatory guidelines are intricate, developers adhere.", false), ("D", "No matter intricate guidelines, developers must adhere.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_499",
                TopicId = 19, LevelId = 5, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Not for one moment _______ the researcher anticipate that her open-source library would power global autonomous vehicle fleets.",
                Explanation = "'did' (đảo trợ động từ quá khứ đơn sau cụm phủ định: 'Not for one moment did the researcher anticipate...').",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Đảo ngữ với 'Not for one moment'.",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "did", true), ("B", "was", false), ("C", "had", false), ("D", "does", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_500",
                TopicId = 19, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Which sentence achieves the most formal, syntactically sophisticated conclusion for a C1 academic essay?",
                Explanation = "'Only through rigorous empirical inquiry and transparent international governance can humanity harness artificial intelligence ethically and sustainably.'",
                Citation = "Tài liệu 17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx - Tổng kết cấu trúc câu đảo ngữ đỉnh cao C1 (Climax Inversion).",
                SourceType = "Extracted", SourceFile = "17_Ngu_phap_tieng_Anh_nang_cao_C1_va_dao_ngu.docx",
                Options = new() { ("A", "Only through rigorous empirical inquiry and transparent international governance can humanity harness artificial intelligence ethically and sustainably.", true), ("B", "In conclusion, AI is very nice and we hope humans will use it nicely without bad things.", false), ("C", "Humanity can harness AI if they do good governance and do not be greedy.", false), ("D", "That is why AI will change everything tomorrow.", false) }
            });

            return list;
        }
    }
}
