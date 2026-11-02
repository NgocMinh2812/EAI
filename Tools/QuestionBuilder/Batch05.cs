using System.Collections.Generic;

namespace QuestionBuilder
{
    public static class Batch05
    {
        public static List<QuestionItem> GetQuestions()
        {
            var list = new List<QuestionItem>();

            // --- BÀI HỌC 14: Thì Hiện Tại Hoàn Thành (Present Perfect) (B1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_201",
                TopicId = 10, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct sentence: 'I _______ in this tech startup since it was founded in 2021.'",
                Explanation = "Hành động bắt đầu trong quá khứ kéo dài liên tục đến hiện tại với dấu hiệu 'since + mốc thời gian' phải dùng thì Hiện tại hoàn thành: 'have worked' hoặc 'have been working'.",
                Citation = "Bài học 14 - Cấu trúc thì Hiện tại hoàn thành với Since và For: S + have/has + V3/ed + since + mốc thời gian.",
                SourceType = "Extracted", SourceFile = "BaiHoc_14_PresentPerfect.txt", LessonId = 14,
                Options = new() { ("A", "worked", false), ("B", "have worked", true), ("C", "had worked", false), ("D", "am working", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_202",
                TopicId = 10, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Fill in the blank with the appropriate adverb: 'Have you _______ visited the software research lab in Silicon Valley?'",
                Explanation = "Trong câu hỏi trải nghiệm của thì Hiện tại hoàn thành, dùng 'ever' (đã từng bao giờ chưa).",
                Citation = "Bài học 14 - Phó từ trong câu nghi vấn Hiện tại hoàn thành: 'Have you ever + V3...?'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_14_PresentPerfect.txt", LessonId = 14,
                Options = new() { ("A", "never", false), ("B", "ever", true), ("C", "already", false), ("D", "yet", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_203",
                TopicId = 10, LevelId = 3, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Complete the sentence with 'already' or 'yet': 'We have _______ deployed the latest server update; our systems are fully operational now.'",
                Explanation = "'Already' dùng trong câu khẳng định chỉ hành động đã hoàn tất sớm hơn dự kiến.",
                Citation = "Bài học 14 - Phân biệt Already (câu khẳng định) vs. Yet (cuối câu phủ định và nghi vấn).",
                SourceType = "Extracted", SourceFile = "BaiHoc_14_PresentPerfect.txt", LessonId = 14,
                Options = new() { ("A", "already", true), ("B", "yet", false), ("C", "still", false), ("D", "ever", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_204",
                TopicId = 10, LevelId = 3, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Identify the error: 'The development team has deployed the v2.0 release yesterday morning.'",
                Explanation = "Khi có trạng từ chỉ thời gian quá khứ xác định cụ thể ('yesterday morning'), không được dùng Hiện tại hoàn thành mà phải dùng Quá khứ đơn: 'deployed'.",
                Citation = "Bài học 14 - Lỗi sai phổ biến: Nhầm lẫn giữa Past Simple (thời điểm xác định) và Present Perfect (không nêu thời điểm).",
                SourceType = "Extracted", SourceFile = "BaiHoc_14_PresentPerfect.txt", LessonId = 14,
                Options = new() { ("A", "'has deployed' should be 'deployed'", true), ("B", "'yesterday' should be 'tomorrow'", false), ("C", "'release' should be 'releasing'", false), ("D", "'The' should be 'A'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_205",
                TopicId = 10, LevelId = 3, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Rewrite using Present Perfect: 'The last time I talked to Professor Miller was two months ago.'",
                Explanation = "'I have not talked to Professor Miller for two months.' là dạng biến đổi câu chuẩn giữa Quá khứ đơn và Hiện tại hoàn thành.",
                Citation = "Bài học 14 - Chuyển đổi câu tương đương: The last time S + V2 was [time] ago -> S + have not + V3 for [time].",
                SourceType = "Applied", SourceFile = "BaiHoc_14_PresentPerfect.txt", LessonId = 14,
                Options = new() { ("A", "I have not talked to Professor Miller for two months.", true), ("B", "I talked to Professor Miller since two months.", false), ("C", "I have talked to Professor Miller two months ago.", false), ("D", "I had not been talking to Professor Miller.", false) }
            });

            // --- BÀI HỌC 17 & 36: Mệnh đề quan hệ xác định và không xác định (B1 - B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_206",
                TopicId = 11, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct sentence with non-defining relative clause punctuation:",
                Explanation = "Mệnh đề quan hệ không xác định (Non-defining) bắt buộc phải có dấu phẩy ngăn cách và tuyệt đối không dùng đại từ 'that': 'Dr. Nguyen, who pioneered AI research in Vietnam, was invited to speak.'",
                Citation = "Bài học 17 & 36 - Mệnh đề quan hệ không xác định: Có dấu phẩy, cung cấp thông tin phụ, không dùng 'that'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_17_RelativeClauses.txt", LessonId = 17,
                Options = new() { ("A", "Dr. Nguyen who pioneered AI research in Vietnam, was invited.", false), ("B", "Dr. Nguyen, that pioneered AI research in Vietnam, was invited.", false), ("C", "Dr. Nguyen, who pioneered AI research in Vietnam, was invited to speak.", true), ("D", "Dr. Nguyen which pioneered AI research, was invited.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_207",
                TopicId = 11, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Select the correct relative pronoun showing possession: 'The engineer _______ laptop was stolen filed a police report.'",
                Explanation = "Đại từ chỉ sở hữu cho người hoặc vật trong mệnh đề quan hệ là 'whose': 'The engineer whose laptop...'.",
                Citation = "Bài học 17 - Đại từ quan hệ chỉ sở hữu: Whose + Noun.",
                SourceType = "Extracted", SourceFile = "BaiHoc_17_RelativeClauses.txt", LessonId = 17,
                Options = new() { ("A", "who", false), ("B", "whose", true), ("C", "whom", false), ("D", "which", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_208",
                TopicId = 11, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Reduce the relative clause into a present participle phrase: 'Software engineers who specialize in cybersecurity earn high salaries.'",
                Explanation = "Rút gọn mệnh đề quan hệ chủ động bằng V-ing: 'Software engineers specializing in cybersecurity earn high salaries.'",
                Citation = "Bài học 17 & 36 - Rút gọn mệnh đề quan hệ chủ động: V-ing.",
                SourceType = "Extracted", SourceFile = "BaiHoc_17_RelativeClauses.txt", LessonId = 17,
                Options = new() { ("A", "Software engineers specializing in cybersecurity earn high salaries.", true), ("B", "Software engineers specialized in cybersecurity earn high salaries.", false), ("C", "Software engineers to specialize in cybersecurity earn.", false), ("D", "Specializing software engineers earn high salaries.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_209",
                TopicId = 11, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Reduce the relative clause into a past participle phrase: 'The research paper which was published by MIT researchers attracted global attention.'",
                Explanation = "Rút gọn mệnh đề quan hệ bị động bằng V3/ed: 'The research paper published by MIT researchers attracted global attention.'",
                Citation = "Bài học 17 & 36 - Rút gọn mệnh đề quan hệ bị động: V3/ed.",
                SourceType = "Extracted", SourceFile = "BaiHoc_17_RelativeClauses.txt", LessonId = 17,
                Options = new() { ("A", "The research paper publishing by MIT researchers attracted attention.", false), ("B", "The research paper published by MIT researchers attracted global attention.", true), ("C", "The research paper having publish by MIT attracted.", false), ("D", "Published research paper attracted global attention.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_210",
                TopicId = 11, LevelId = 3, QuestionType = "DungSai", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "True or False: In formal English, prepositions can precede relative pronouns (e.g., 'the company for which I work').",
                Explanation = "Đúng. Trong văn phong trang trọng, giới từ có thể đứng trước 'whom' hoặc 'which' (for which, in which, to whom).",
                Citation = "Bài học 17 - Giới từ trong mệnh đề quan hệ trang trọng: Preposition + whom / which.",
                SourceType = "Extracted", SourceFile = "BaiHoc_17_RelativeClauses.txt", LessonId = 17,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 18: Động từ tường thuật và Thể truyền khiến (Have/Get) (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_211",
                TopicId = 11, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct causative passive structure: 'I need to have my air conditioner _______ before summer begins.'",
                Explanation = "Cấu trúc truyền khiến bị động: 'have / get something + V3/ed' (nhờ/thuê ai làm việc gì cho mình). Ở đây dùng 'have my air conditioner serviced'.",
                Citation = "Bài học 18 - Thể truyền khiến bị động: S + have / get + O(vật) + V3/ed.",
                SourceType = "Extracted", SourceFile = "BaiHoc_18_ReportedSpeechAndCausatives.txt", LessonId = 18,
                Options = new() { ("A", "service", false), ("B", "servicing", false), ("C", "serviced", true), ("D", "to service", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_212",
                TopicId = 11, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct causative active structure: 'The project manager got the lead developer _______ the legacy database.'",
                Explanation = "Cấu trúc truyền khiến chủ động với 'get': 'get someone TO DO something'. (Khác với 'have someone DO something').",
                Citation = "Bài học 18 - Phân biệt thể truyền khiến chủ động: Have someone DO sth vs. Get someone TO DO sth.",
                SourceType = "Extracted", SourceFile = "BaiHoc_18_ReportedSpeechAndCausatives.txt", LessonId = 18,
                Options = new() { ("A", "migrate", false), ("B", "to migrate", true), ("C", "migrated", false), ("D", "migrating", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_213",
                TopicId = 11, LevelId = 4, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Complete the causative sentence: 'She had her assistant _______ the flight tickets to Tokyo.'",
                Explanation = "'Have someone do something' dùng động từ nguyên thể không 'to': 'book'.",
                Citation = "Bài học 18 - Cấu trúc: S + have + O(người) + V-bare.",
                SourceType = "Extracted", SourceFile = "BaiHoc_18_ReportedSpeechAndCausatives.txt", LessonId = 18,
                Options = new() { ("A", "book", true), ("B", "to book", false), ("C", "booked", false), ("D", "booking", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_214",
                TopicId = 11, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Which reporting verb requires the pattern 'verb + object + to-infinitive'?",
                Explanation = "Động từ 'encourage' đi với tân ngữ + to-V: 'The mentor encouraged him to apply for the scholarship.'",
                Citation = "Bài học 18 & 31 - Động từ tường thuật chỉ dẫn/khuyên bảo: S + advise/encourage/remind + O + to-V.",
                SourceType = "Extracted", SourceFile = "BaiHoc_18_ReportedSpeechAndCausatives.txt", LessonId = 18,
                Options = new() { ("A", "suggest", false), ("B", "encourage", true), ("C", "admit", false), ("D", "deny", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_215",
                TopicId = 11, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Transform into reported speech: '\"I will submit the assignment tomorrow,\" Linda told her professor.'",
                Explanation = "Lùi thì 'will' thành 'would', trạng từ 'tomorrow' đổi thành 'the next day' hoặc 'the following day': 'Linda told her professor that she would submit the assignment the following day.'",
                Citation = "Bài học 18 & 31 - Quy tắc lùi thì và chuyển đổi trạng từ thời gian trong câu gián tiếp.",
                SourceType = "Applied", SourceFile = "BaiHoc_18_ReportedSpeechAndCausatives.txt", LessonId = 18,
                Options = new() { ("A", "Linda told her professor that she would submit the assignment the following day.", true), ("B", "Linda told her professor she will submit the assignment tomorrow.", false), ("C", "Linda told her professor she had submitted the assignment yesterday.", false), ("D", "Linda asked her professor if she would submit assignment.", false) }
            });

            // --- BÀI HỌC 19: Câu Điều Kiện Loại 1, 2, 3 và Điều Kiện Trộn (Mixed Conditionals) (B1 - B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_216",
                TopicId = 12, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Which sentence represents a First Conditional (real possibility in the future)?",
                Explanation = "Câu điều kiện loại 1: 'If + S + V(hiện tại đơn), S + will + V(nguyên mẫu)'.",
                Citation = "Bài học 19 - Cấu trúc câu điều kiện loại 1 (First Conditional).",
                SourceType = "Extracted", SourceFile = "BaiHoc_19_ConditionalsMixed.txt", LessonId = 19,
                Options = new() { ("A", "If it rains tomorrow, we will postpone the outdoor sports match.", true), ("B", "If I had wings, I would fly across the ocean.", false), ("C", "If you had studied harder, you would have passed.", false), ("D", "If ice melts, it becomes water.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_217",
                TopicId = 12, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct Second Conditional sentence (hypothetical present situation):",
                Explanation = "Câu điều kiện loại 2 giả định trái ngược hiện tại: 'If + S + V2/were, S + would + V'.",
                Citation = "Bài học 19 - Cấu trúc câu điều kiện loại 2 (Second Conditional): If I were you, I would...",
                SourceType = "Extracted", SourceFile = "BaiHoc_19_ConditionalsMixed.txt", LessonId = 19,
                Options = new() { ("A", "If I won the lottery tomorrow, I buy a house.", false), ("B", "If I were the CEO of that multinational company, I would implement a four-day workweek.", true), ("C", "If I will be CEO, I would implement four-day workweek.", false), ("D", "If I am CEO, I would have implemented.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_218",
                TopicId = 12, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Select the correct Third Conditional sentence (regret about past unfulfilled event):",
                Explanation = "Câu điều kiện loại 3 giả định trái ngược quá khứ: 'If + S + had + V3, S + would have + V3'.",
                Citation = "Bài học 19 - Cấu trúc câu điều kiện loại 3 (Third Conditional).",
                SourceType = "Extracted", SourceFile = "BaiHoc_19_ConditionalsMixed.txt", LessonId = 19,
                Options = new() { ("A", "If we had left the hotel earlier, we would not have missed our flight.", true), ("B", "If we left earlier, we would not miss flight.", false), ("C", "If we have left earlier, we will not miss flight.", false), ("D", "If we had left earlier, we will not have missed.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_219",
                TopicId = 12, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Analyze the mixed conditional: 'If Mark had taken the job offer in London last year, he _______ living there now.'",
                Explanation = "Điều kiện trộn (Past cause -> Present result): Mệnh đề If loại 3 (had taken) kết hợp với mệnh đề chính loại 2 chỉ kết quả ở hiện tại (would be living now).",
                Citation = "Bài học 19 - Câu điều kiện trộn (Mixed Conditionals): Giả định quá khứ dẫn đến kết quả ở hiện tại.",
                SourceType = "Extracted", SourceFile = "BaiHoc_19_ConditionalsMixed.txt", LessonId = 19,
                Options = new() { ("A", "would have been", false), ("B", "would be", true), ("C", "will be", false), ("D", "is being", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_220",
                TopicId = 12, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Rewrite using inverted conditional: 'If you should require any further information, please contact our helpdesk.'",
                Explanation = "Đảo ngữ điều kiện loại 1: Bỏ If, đảo 'Should' lên đầu câu: 'Should you require any further information, please contact our helpdesk.'",
                Citation = "Bài học 19 & 20 - Đảo ngữ câu điều kiện loại 1: Should + S + V-inf.",
                SourceType = "Extracted", SourceFile = "BaiHoc_19_ConditionalsMixed.txt", LessonId = 19,
                Options = new() { ("A", "Should you require any further information, please contact our helpdesk.", true), ("B", "Did you require further information, contact helpdesk.", false), ("C", "Were you requiring information, contact helpdesk.", false), ("D", "Had you required information, please contact helpdesk.", false) }
            });

            // --- BÀI HỌC 31: Câu Tường Thuật Chuyên Sâu (Reported Speech) (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_221",
                TopicId = 23, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Which reporting verb is followed by a gerund (V-ing)?",
                Explanation = "Động từ 'admit' và 'deny' đi trực tiếp với danh động từ V-ing: 'He admitted breaking the test tube.'",
                Citation = "Bài học 31 - Cấu trúc động từ tường thuật đặc biệt: admit / deny + V-ing.",
                SourceType = "Extracted", SourceFile = "BaiHoc_31_ReportedSpeechB2.txt", LessonId = 31,
                Options = new() { ("A", "refuse", false), ("B", "admit", true), ("C", "threaten", false), ("D", "agree", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_222",
                TopicId = 23, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "How do you report an apology in indirect speech: '\"I am truly sorry I forgot our appointment,\" John said.'?",
                Explanation = "'apologize for + V-ing': 'John apologized for forgetting their appointment.'",
                Citation = "Bài học 31 - Mẫu câu xin lỗi gián tiếp: S + apologize to O for + V-ing.",
                SourceType = "Extracted", SourceFile = "BaiHoc_31_ReportedSpeechB2.txt", LessonId = 31,
                Options = new() { ("A", "John said sorry that he forgot.", false), ("B", "John apologized for forgetting their appointment.", true), ("C", "John apologized to forget their appointment.", false), ("D", "John denied forgetting the appointment.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_223",
                TopicId = 23, LevelId = 4, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Complete the indirect question: 'She asked me where I _______ during my summer holiday.'",
                Explanation = "Trong câu hỏi gián tiếp, cấu trúc từ hỏi là S+V (không đảo trợ động từ), lùi thì Quá khứ đơn thành Quá khứ hoàn thành: 'had gone'.",
                Citation = "Bài học 31 - Cấu trúc câu hỏi gián tiếp: Wh-word + S + V(lùi thì).",
                SourceType = "Extracted", SourceFile = "BaiHoc_31_ReportedSpeechB2.txt", LessonId = 31,
                Options = new() { ("A", "had gone", true), ("B", "did go", false), ("C", "went I", false), ("D", "have gone", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_224",
                TopicId = 23, LevelId = 4, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the mistake: 'My supervisor warned me not touch the exposed electrical wire.'",
                Explanation = "Cấu trúc cảnh báo phủ định: 'warn someone NOT TO DO something'. Thiếu giới từ 'to': 'not to touch'.",
                Citation = "Bài học 31 - Động từ cảnh báo: S + warn + O + (not) to-V.",
                SourceType = "Extracted", SourceFile = "BaiHoc_31_ReportedSpeechB2.txt", LessonId = 31,
                Options = new() { ("A", "'warned' should be 'warning'", false), ("B", "'not touch' should be 'not to touch'", true), ("C", "'exposed' should be 'exposing'", false), ("D", "'electrical' should be 'electrician'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_225",
                TopicId = 23, LevelId = 4, QuestionType = "DungSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "True or False: When reporting a universal truth or scientific law (e.g., 'The teacher said the Earth revolves around the Sun'), backshifting of tenses is not required.",
                Explanation = "Đúng. Với chân lý vĩnh cửu hoặc quy luật khoa học hiển nhiên, không bắt buộc lùi thì động từ.",
                Citation = "Bài học 31 - Trường hợp ngoại lệ không lùi thì: Sự thật hiển nhiên và chân lý khoa học.",
                SourceType = "Extracted", SourceFile = "BaiHoc_31_ReportedSpeechB2.txt", LessonId = 31,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 32: Câu Bị Động Chuyên Sâu (Passive Voice) (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_226",
                TopicId = 23, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Select the impersonal passive structure used in academic writing:",
                Explanation = "'It is widely believed that renewable energy will replace fossil fuels.' là cấu trúc bị động khách quan (Impersonal Passive) rất được ưa chuộng trong văn phong học thuật.",
                Citation = "Bài học 32 - Bị động khách quan: It is said / believed / reported that + clause.",
                SourceType = "Extracted", SourceFile = "BaiHoc_32_PassiveVoiceB2.txt", LessonId = 32,
                Options = new() { ("A", "People widely believe that renewable energy replaces.", false), ("B", "It is widely believed that renewable energy will replace fossil fuels.", true), ("C", "Renewable energy widely believes replacing fossil fuels.", false), ("D", "Believing widely is that energy replaces.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_227",
                TopicId = 23, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Transform into personal passive with infinitive: 'People say that the CEO donated two million dollars to cancer research.'",
                Explanation = "Chuyển thành: 'The CEO is said to have donated two million dollars to cancer research.' (Hành động 'donated' xảy ra trước hành động 'say', dùng 'to have + V3').",
                Citation = "Bài học 32 - Chuyển đổi bị động kép: S + be + V3 + to have + V3 (hành động trước thì hiện tại).",
                SourceType = "Extracted", SourceFile = "BaiHoc_32_PassiveVoiceB2.txt", LessonId = 32,
                Options = new() { ("A", "The CEO is said to have donated two million dollars to cancer research.", true), ("B", "The CEO is said to donate two million dollars.", false), ("C", "The CEO was said donating two million dollars.", false), ("D", "The CEO said to be donated two million dollars.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_228",
                TopicId = 23, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the passive form with a modal verb: 'Critical security patches _______ without delay.'",
                Explanation = "Bị động với động từ khiếm khuyết (Modal verb): 'Modal + must be + V3/ed'. Ở đây dùng 'must be installed'.",
                Citation = "Bài học 32 - Câu bị động với động từ khiếm khuyết: Modal + be + V3/ed.",
                SourceType = "Extracted", SourceFile = "BaiHoc_32_PassiveVoiceB2.txt", LessonId = 32,
                Options = new() { ("A", "must install", false), ("B", "must be installed", true), ("C", "must have install", false), ("D", "must being installed", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_229",
                TopicId = 23, LevelId = 4, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Complete the passive continuous sentence: 'A state-of-the-art laboratory is currently _______ constructed on campus.'",
                Explanation = "Thì Hiện tại tiếp diễn ở thể bị động: 'is being + V3/ed'.",
                Citation = "Bài học 32 - Bị động ở các thì tiếp diễn: be + being + V3/ed.",
                SourceType = "Extracted", SourceFile = "BaiHoc_32_PassiveVoiceB2.txt", LessonId = 32,
                Options = new() { ("A", "being", true), ("B", "been", false), ("C", "be", false), ("D", "having", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_230",
                TopicId = 23, LevelId = 4, QuestionType = "DungSai", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "True or False: Intransitive verbs such as 'arrive', 'happen', and 'die' cannot be used in the passive voice.",
                Explanation = "Đúng. Nội động từ (Intransitive verbs) không có tân ngữ trực tiếp nên không thể chuyển sang thể bị động.",
                Citation = "Bài học 32 - Điều kiện chuyển câu bị động: Động từ phải là Ngoại động từ (Transitive Verbs).",
                SourceType = "Extracted", SourceFile = "BaiHoc_32_PassiveVoiceB2.txt", LessonId = 32,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 20: Cấu trúc Đảo ngữ nâng cao (Inversion) (C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_231",
                TopicId = 12, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Choose the grammatically correct inversion sentence:",
                Explanation = "Khi cụm từ phủ định 'Under no circumstances' đứng đầu câu, phải đảo trợ động từ lên trước chủ ngữ: 'Under no circumstances should confidential files be shared.'",
                Citation = "Bài học 20 - Đảo ngữ với cụm từ phủ định: Under no circumstances + Auxiliary + S + V.",
                SourceType = "Extracted", SourceFile = "BaiHoc_20_AdvancedInversion.txt", LessonId = 20,
                Options = new() { ("A", "Under no circumstances confidential files should be shared.", false), ("B", "Under no circumstances should confidential files be shared.", true), ("C", "Under no circumstances files are confidential to share.", false), ("D", "Under no circumstances sharing files should happen.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_232",
                TopicId = 12, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Complete the inverted sentence: 'Seldom _______ such an inspiring keynote address at a tech summit.'",
                Explanation = "Sau 'Seldom', đảo trợ động từ của thì Hiện tại hoàn thành: 'have I heard'.",
                Citation = "Bài học 20 - Đảo ngữ với Seldom, Rarely, Never.",
                SourceType = "Extracted", SourceFile = "BaiHoc_20_AdvancedInversion.txt", LessonId = 20,
                Options = new() { ("A", "I have heard", false), ("B", "have I heard", true), ("C", "heard I", false), ("D", "did I heard", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_233",
                TopicId = 12, LevelId = 5, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Invert the conditional sentence: 'If they had known about the severe traffic, they would have taken the train.'",
                Explanation = "Đảo ngữ câu điều kiện loại 3: Bỏ If, đảo 'Had' lên trước chủ ngữ: 'Had they known about the severe traffic, they would have taken the train.'",
                Citation = "Bài học 20 - Đảo ngữ câu điều kiện loại 3: Had + S + (not) + V3/ed.",
                SourceType = "Extracted", SourceFile = "BaiHoc_20_AdvancedInversion.txt", LessonId = 20,
                Options = new() { ("A", "Had they known about the severe traffic, they would have taken the train.", true), ("B", "Were they known about traffic, they would take train.", false), ("C", "Did they know about traffic, they had taken train.", false), ("D", "Should they have known traffic, they take train.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_234",
                TopicId = 12, LevelId = 5, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Invert the conditional sentence: 'If I were in your position, I would accept the overseas fellowship.'",
                Explanation = "Đảo ngữ câu điều kiện loại 2: Bỏ If, đảo 'Were' lên trước chủ ngữ: 'Were I in your position, I would accept the overseas fellowship.'",
                Citation = "Bài học 20 - Đảo ngữ câu điều kiện loại 2: Were + S + to-V / complement.",
                SourceType = "Extracted", SourceFile = "BaiHoc_20_AdvancedInversion.txt", LessonId = 20,
                Options = new() { ("A", "Were I in your position, I would accept the overseas fellowship.", true), ("B", "Was I in your position, I accept.", false), ("C", "Had I been in position, I will accept.", false), ("D", "Should I be in position, I would accepted.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_235",
                TopicId = 12, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Fill in the correlative pair: 'No sooner had the keynote begun _______ a power outage plunged the hall into darkness.'",
                Explanation = "Cặp liên từ đi với No sooner là 'than' ('No sooner had + S + V3 than + S + V2'). (Khác với 'Hardly... when').",
                Citation = "Bài học 20 - Cặp đảo ngữ thời gian: No sooner... than vs. Hardly / Scarcely... when.",
                SourceType = "Extracted", SourceFile = "BaiHoc_20_AdvancedInversion.txt", LessonId = 20,
                Options = new() { ("A", "when", false), ("B", "than", true), ("C", "then", false), ("D", "as", false) }
            });

            // --- BỔ SUNG CÂU HỎI THỰC HÀNH NGỮ PHÁP TỔNG HỢP (B1 - B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_236",
                TopicId = 10, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Which sentence uses the correct order of adjectives before a noun (OSASCOMP)?",
                Explanation = "Thứ tự tính từ OSASCOMP: Opinion -> Size -> Age -> Shape -> Color -> Origin -> Material -> Purpose. 'a beautiful small antique wooden desk' đúng thứ tự: Opinion (beautiful) -> Size (small) -> Age (antique) -> Material (wooden).",
                Citation = "Bài học 14 - Trật tự tính từ trước danh từ (OSASCOMP rule).",
                SourceType = "Applied", SourceFile = "BaiHoc_14_PresentPerfect.txt", LessonId = 14,
                Options = new() { ("A", "a wooden antique beautiful small desk", false), ("B", "a beautiful small antique wooden desk", true), ("C", "a small wooden antique beautiful desk", false), ("D", "an antique beautiful wooden small desk", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_237",
                TopicId = 10, LevelId = 3, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Fill in the blank with a comparative form: 'Electric cars are becoming significantly _______ expensive than they were a decade ago.'",
                Explanation = "'less expensive' (ít đắt đỏ hơn) hoặc 'more expensive'. Ở đây ngữ cảnh xe điện ngày càng dễ tiếp cận là 'less expensive'.",
                Citation = "Bài học 15 - Cấu trúc so sánh hơn kém của tính từ dài.",
                SourceType = "Applied", SourceFile = "BaiHoc_15_PastSimpleVsContinuous.txt", LessonId = 15,
                Options = new() { ("A", "less", true), ("B", "fewer", false), ("C", "least", false), ("D", "little", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_238",
                TopicId = 11, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct prepositional relative clause: 'The framework _______ the web application is built requires Node.js v18.'",
                Explanation = "'on which' bổ nghĩa cho nền tảng framework mà trên đó ứng dụng được xây dựng.",
                Citation = "Bài học 17 & 36 - Giới từ đi kèm đại từ quan hệ: on which / in which.",
                SourceType = "Applied", SourceFile = "BaiHoc_17_RelativeClauses.txt", LessonId = 17,
                Options = new() { ("A", "on which", true), ("B", "at that", false), ("C", "with whom", false), ("D", "for whose", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_239",
                TopicId = 12, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Which clause is synonymous with 'Unless you study daily, you will not master the language'?",
                Explanation = "'Unless' = 'If not'. Câu tương đương là: 'If you do not study daily, you will not master the language.'",
                Citation = "Bài học 19 - Cách dùng Unless = If not.",
                SourceType = "Extracted", SourceFile = "BaiHoc_19_ConditionalsMixed.txt", LessonId = 19,
                Options = new() { ("A", "If you study daily, you fail.", false), ("B", "If you do not study daily, you will not master the language.", true), ("C", "Provided you study daily, you will fail.", false), ("D", "As long as you study, you do not master.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_240",
                TopicId = 23, LevelId = 4, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the mistake: 'The ancient manuscript was discovered by accidental during excavations.'",
                Explanation = "Cụm trạng từ đúng là 'by accident' (tình cờ) hoặc dùng trạng từ 'accidentally', không dùng 'by accidental'.",
                Citation = "Bài học 32 - Trạng từ chỉ cách thức trong câu bị động.",
                SourceType = "Applied", SourceFile = "BaiHoc_32_PassiveVoiceB2.txt", LessonId = 32,
                Options = new() { ("A", "'was discovered' should be 'is discover'", false), ("B", "'by accidental' should be 'by accident'", true), ("C", "'during' should be 'while'", false), ("D", "'excavations' should be 'excavate'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_241",
                TopicId = 10, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Select the sentence with the correct past perfect continuous tense:",
                Explanation = "'They had been debugging the codebase for five hours before they found the memory leak.' diễn tả hành động xảy ra và kéo dài liên tục trước một mốc quá khứ khác.",
                Citation = "Bài học 15 - Thì Quá khứ hoàn thành tiếp diễn (Past Perfect Continuous): had been + V-ing.",
                SourceType = "Extracted", SourceFile = "BaiHoc_15_PastSimpleVsContinuous.txt", LessonId = 15,
                Options = new() { ("A", "They had been debugging the codebase for five hours before they found the memory leak.", true), ("B", "They have been debugging for five hours before found leak.", false), ("C", "They were debugging since five hours before found.", false), ("D", "They had debugged continually before find.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_242",
                TopicId = 12, LevelId = 4, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Complete the conditional phrase: '_______ for your timely intervention, the company would have gone bankrupt.'",
                Explanation = "Thành ngữ điều kiện: 'But for + Noun' = 'Were it not for / Had it not been for' (Nếu không nhờ có...).",
                Citation = "Bài học 19 - Cụm từ thay thế câu điều kiện: But for / Without + Noun.",
                SourceType = "Extracted", SourceFile = "BaiHoc_19_ConditionalsMixed.txt", LessonId = 19,
                Options = new() { ("A", "But", true), ("B", "Except", false), ("C", "Unless", false), ("D", "Besides", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_243",
                TopicId = 23, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct preposition: 'The candidate was praised _______ her exceptional analytical skills during the debate.'",
                Explanation = "Cụm động từ bị động: 'be praised for sth' (được khen ngợi vì điều gì).",
                Citation = "Bài học 32 - Động từ bị động đi kèm giới từ cố định: be praised for, be accused of.",
                SourceType = "Extracted", SourceFile = "BaiHoc_32_PassiveVoiceB2.txt", LessonId = 32,
                Options = new() { ("A", "with", false), ("B", "for", true), ("C", "at", false), ("D", "of", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_244",
                TopicId = 10, LevelId = 3, QuestionType = "DungSai", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "True or False: Stative verbs (e.g., 'belong', 'know', 'believe') are rarely used in continuous tenses.",
                Explanation = "Đúng. Động từ chỉ trạng thái (Stative verbs) chỉ nhận thức, sở hữu và cảm xúc thường không chia ở thì tiếp diễn.",
                Citation = "Bài học 14 & 15 - Phân biệt Action Verbs vs. Stative Verbs.",
                SourceType = "Extracted", SourceFile = "BaiHoc_14_PresentPerfect.txt", LessonId = 14,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_245",
                TopicId = 12, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Invert the sentence starting with 'Not only': 'The software is both fast and energy-efficient.'",
                Explanation = "'Not only is the software fast, but it is also energy-efficient.' đảo trợ động từ 'is' lên trước chủ ngữ.",
                Citation = "Bài học 20 - Đảo ngữ với Not only: Not only + Aux + S + V, but S + also + V.",
                SourceType = "Extracted", SourceFile = "BaiHoc_20_AdvancedInversion.txt", LessonId = 20,
                Options = new() { ("A", "Not only the software is fast, but it is also energy-efficient.", false), ("B", "Not only is the software fast, but it is also energy-efficient.", true), ("C", "Not only fast software is, but energy-efficient.", false), ("D", "Not only does the software fast, but also energy.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_246",
                TopicId = 11, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Which relative pronoun is used to refer to a place: 'Da Nang is the coastal city _______ my parents were born.'",
                Explanation = "Trạng từ quan hệ chỉ nơi chốn là 'where'.",
                Citation = "Bài học 17 & 36 - Trạng từ quan hệ chỉ nơi chốn: Where = In/at which.",
                SourceType = "Extracted", SourceFile = "BaiHoc_17_RelativeClauses.txt", LessonId = 17,
                Options = new() { ("A", "when", false), ("B", "where", true), ("C", "which", false), ("D", "why", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_247",
                TopicId = 23, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Change into active voice: 'The bridge was designed by Gustave Eiffel in 1889.'",
                Explanation = "Chuyển sang chủ động: 'Gustave Eiffel designed the bridge in 1889.'",
                Citation = "Bài học 32 - Chuyển đổi giữa câu Chủ động và Bị động.",
                SourceType = "Applied", SourceFile = "BaiHoc_32_PassiveVoiceB2.txt", LessonId = 32,
                Options = new() { ("A", "Gustave Eiffel designed the bridge in 1889.", true), ("B", "Gustave Eiffel was designing the bridge in 1889.", false), ("C", "Gustave Eiffel has designed the bridge in 1889.", false), ("D", "The bridge had Gustave Eiffel designed in 1889.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_248",
                TopicId = 12, LevelId = 4, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank with the correct conditional connector: 'You can borrow my car _______ you return it with a full tank of fuel.'",
                Explanation = "'as long as' hoặc 'provided that' mang nghĩa 'miễn là' (điều kiện ràng buộc).",
                Citation = "Bài học 19 - Các từ nối điều kiện: As long as / Provided that / In case.",
                SourceType = "Extracted", SourceFile = "BaiHoc_19_ConditionalsMixed.txt", LessonId = 19,
                Options = new() { ("A", "as long as", true), ("B", "unless", false), ("C", "in spite of", false), ("D", "although", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_249",
                TopicId = 10, LevelId = 4, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Find the mistake: 'I would rather you don't reveal the project timeline until the launch.'",
                Explanation = "Cấu trúc 'would rather someone did/didn't do sth' (muốn ai làm/không làm gì ở hiện tại/tương lai) lùi 1 thì về quá khứ giả định: 'didn't reveal', không dùng 'don't'.",
                Citation = "Bài học 14 - Cấu trúc giả định với Would rather: S1 + would rather + S2 + Past Simple.",
                SourceType = "Extracted", SourceFile = "BaiHoc_14_PresentPerfect.txt", LessonId = 14,
                Options = new() { ("A", "'would rather' should be 'had rather'", false), ("B", "'don't' should be 'didn't'", true), ("C", "'reveal' should be 'revealed'", false), ("D", "'until' should be 'since'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_250",
                TopicId = 12, LevelId = 5, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Choose the inverted sentence using 'Only after': 'The researchers understood the anomaly only after they repeated the experiment five times.'",
                Explanation = "Khi 'Only after + clause' đứng đầu câu, mệnh đề chính phía sau phải đảo ngữ: 'Only after they repeated the experiment five times did the researchers understand the anomaly.'",
                Citation = "Bài học 20 - Đảo ngữ với Only after: Only after + Clause + Aux + S + V.",
                SourceType = "Extracted", SourceFile = "BaiHoc_20_AdvancedInversion.txt", LessonId = 20,
                Options = new() { ("A", "Only after they repeated the experiment five times, the researchers understood the anomaly.", false), ("B", "Only after they repeated the experiment five times did the researchers understand the anomaly.", true), ("C", "Only after did they repeat the experiment five times, researchers understood.", false), ("D", "Only after repeating five times, anomaly was understood by researchers.", false) }
            });

            return list;
        }
    }
}

