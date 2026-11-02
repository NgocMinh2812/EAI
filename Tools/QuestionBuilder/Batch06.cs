using System.Collections.Generic;

namespace QuestionBuilder
{
    public static class Batch06
    {
        public static List<QuestionItem> GetQuestions()
        {
            var list = new List<QuestionItem>();

            // --- BÀI HỌC 8: Chiến lược Skimming & Scanning bài đọc IELTS Academic chuyên sâu (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_251",
                TopicId = 6, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "What is the primary operational distinction between 'Skimming' and 'Scanning' in IELTS Academic Reading?",
                Explanation = "'Skimming' là kỹ thuật đọc lướt để nắm ý chính (gist) và bố cục toàn bài, còn 'Scanning' là đọc quét mắt tìm từ khóa cụ thể (con số, tên riêng, thuật ngữ).",
                Citation = "Bài học 8 - Mục 1: Phân biệt Skimming (đọc lấy ý chính) vs. Scanning (đọc tìm dữ liệu cụ thể).",
                SourceType = "Extracted", SourceFile = "BaiHoc_08_IeltsSkimmingScanning.txt", LessonId = 8,
                Options = new() { ("A", "Skimming is for spelling checks, while scanning is for translation.", false), ("B", "Skimming quickly captures the central gist and paragraph structure, while scanning targets specific keywords, dates, and names.", true), ("C", "Scanning reads the entire passage slowly word-by-word.", false), ("D", "Both techniques require vocal reading out loud.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_252",
                TopicId = 6, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Where in an academic paragraph can readers typically locate the 'Topic Sentence'?",
                Explanation = "Trong văn bản học thuật IELTS, câu chủ đề (Topic Sentence) thường nằm ở câu đầu tiên hoặc câu thứ hai của đoạn văn.",
                Citation = "Bài học 8 - Mục 2: Nhận diện Topic Sentence: Nằm ở vị trí câu đầu hoặc câu thứ hai của đoạn.",
                SourceType = "Extracted", SourceFile = "BaiHoc_08_IeltsSkimmingScanning.txt", LessonId = 8,
                Options = new() { ("A", "Exclusively in the footnotes", false), ("B", "Usually in the first or second sentence of the paragraph", true), ("C", "Only in the title of the book", false), ("D", "In the appendix at the back", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_253",
                TopicId = 6, LevelId = 4, QuestionType = "DienKhuyet", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Complete the reading technique: 'When scanning, candidates should search for _______ rather than looking for identical words, because IELTS tests paraphrase skills.'",
                Explanation = "Đề thi IELTS hầu như luôn 'paraphrase' (diễn đạt lại bằng từ đồng nghĩa). Do đó khi scan phải tìm từ đồng nghĩa (synonyms).",
                Citation = "Bài học 8 - Mục 3: Cảnh báo bẫy Paraphrase trong IELTS Reading: Tìm kiếm Synonyms.",
                SourceType = "Extracted", SourceFile = "BaiHoc_08_IeltsSkimmingScanning.txt", LessonId = 8,
                Options = new() { ("A", "synonyms", true), ("B", "rhymes", false), ("C", "emojis", false), ("D", "spaces", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_254",
                TopicId = 6, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: In IELTS Reading, candidates should spend approximately 20 minutes on each of the three passages to manage the 60-minute time limit effectively.",
                Explanation = "Đúng. Bài thi IELTS Reading gồm 3 bài đọc với 40 câu trong 60 phút, trung bình mỗi bài phân bổ khoảng 20 phút.",
                Citation = "Bài học 8 - Chiến lược quản lý thời gian: 20 phút / Passage.",
                SourceType = "Extracted", SourceFile = "BaiHoc_08_IeltsSkimmingScanning.txt", LessonId = 8,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_255",
                TopicId = 6, LevelId = 4, QuestionType = "NoiCap", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Match each IELTS Reading question type with its primary strategy:\n1. 'Matching Headings'\n2. 'Sentence Completion'\n3. 'True / False / Not Given'",
                Explanation = "1-Đọc câu đầu/cuối tìm ý chính đoạn; 2-Xác định loại từ cần điền và scan từ khóa; 3-Đối chiếu tính chính xác của dữ kiện văn bản.",
                Citation = "Bài học 8 - Bảng chiến thuật tương ứng từng dạng bài IELTS Reading.",
                SourceType = "Extracted", SourceFile = "BaiHoc_08_IeltsSkimmingScanning.txt", LessonId = 8,
                Options = new() { ("A", "1-Skim topic sentences for paragraph gist, 2-Identify grammatical word form and scan keyword, 3-Verify factual claims against text", true), ("B", "1-Count words, 2-Translate into Vietnamese, 3-Guess randomly", false), ("C", "1-Look at images, 2-Memorize spelling, 3-Read backwards", false), ("D", "1-Underline nouns, 2-Skip text, 3-Check punctuation", false) }
            });

            // --- BÀI HỌC 9: Tuyệt chiêu xử lý dạng bài True / False / Not Given & Yes / No / Not Given (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_256",
                TopicId = 6, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "In IELTS Reading, what is the crucial definition of 'NOT GIVEN'?",
                Explanation = "'NOT GIVEN' có nghĩa là thông tin đó không thể khẳng định cũng không thể bác bỏ dựa trên văn bản đọc (văn bản không đề cập đủ để kết luận).",
                Citation = "Bài học 9 - Định nghĩa chuẩn mực: TRUE = matches information; FALSE = contradicts information; NOT GIVEN = impossible to verify from text.",
                SourceType = "Extracted", SourceFile = "BaiHoc_09_TrueFalseNotGiven.txt", LessonId = 9,
                Options = new() { ("A", "The statement is 100% false in real life.", false), ("B", "There is not enough information in the passage to confirm or contradict the statement.", true), ("C", "The author personally dislikes the statement.", false), ("D", "The words in the statement are completely misspelled.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_257",
                TopicId = 6, LevelId = 4, QuestionType = "TrueFalseNotGiven", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "Read the excerpt: 'While electric cars produce zero tailpipe emissions, the carbon footprint of battery manufacturing remains a challenge.' Determine: 'Electric vehicles have no negative environmental impacts whatsoever.'",
                ReadingPassage = "While electric cars produce zero tailpipe emissions, the carbon footprint of battery manufacturing remains a challenge.",
                Explanation = "Văn bản nêu rõ việc chế tạo pin vẫn tạo ra 'carbon footprint' (vấn đề môi trường), trái ngược hoàn toàn với mệnh đề 'không có bất kỳ tác động tiêu cực nào'. Vì vậy đáp án là FALSE.",
                Citation = "Bài học 9 - Phân tích câu mâu thuẫn trực tiếp (Contradiction -> FALSE).",
                SourceType = "Extracted", SourceFile = "BaiHoc_09_TrueFalseNotGiven.txt", LessonId = 9,
                Options = new() { ("A", "True", false), ("B", "False", true), ("C", "Not Given", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_258",
                TopicId = 6, LevelId = 4, QuestionType = "TrueFalseNotGiven", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "Read the excerpt: 'Dr. Carter developed the synthetic vaccine in his laboratory in Zurich.' Determine: 'Dr. Carter spent ten years researching the synthetic vaccine.'",
                ReadingPassage = "Dr. Carter developed the synthetic vaccine in his laboratory in Zurich.",
                Explanation = "Văn bản chỉ nói Dr. Carter phát triển vắc-xin tại Zurich, hoàn toàn không nói ông ấy mất 10 năm hay bao lâu. Do đó đáp án là NOT GIVEN.",
                Citation = "Bài học 9 - Bẫy thông tin bổ sung không được đề cập -> NOT GIVEN.",
                SourceType = "Extracted", SourceFile = "BaiHoc_09_TrueFalseNotGiven.txt", LessonId = 9,
                Options = new() { ("A", "True", false), ("B", "False", false), ("C", "Not Given", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_259",
                TopicId = 6, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What danger do 'Qualifying Words' (such as all, always, solely vs. some, sometimes, often) present in True/False/Not Given questions?",
                Explanation = "Các từ hạn định tuyệt đối (all, solely, always) thường bóp méo ý của bài đọc (bài đọc chỉ nói 'some' hoặc 'tend to'). Nếu câu hỏi dùng từ tuyệt đối hóa thì câu đó thường là FALSE.",
                Citation = "Bài học 9 - Bẫy từ hạn định (Qualifying words): all/only/always vs some/often/might.",
                SourceType = "Extracted", SourceFile = "BaiHoc_09_TrueFalseNotGiven.txt", LessonId = 9,
                Options = new() { ("A", "They make sentences sound poetic.", false), ("B", "They alter the scope of claims; absolute words (all, always) often contradict moderate statements (some, likely) in the text.", true), ("C", "They make text impossible to translate.", false), ("D", "They are always typos by the examiner.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_260",
                TopicId = 6, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "True or False: In IELTS Reading, questions in True/False/Not Given tasks always follow the chronological order of the reading passage.",
                Explanation = "Đúng. Dạng bài TFNG/YNNG tuân theo trình tự tuần tự của văn bản đọc (thông tin câu 2 sẽ xuất hiện sau câu 1).",
                Citation = "Bài học 9 - Quy tắc tuần tự của dạng bài TFNG (Questions follow chronological text order).",
                SourceType = "Extracted", SourceFile = "BaiHoc_09_TrueFalseNotGiven.txt", LessonId = 9,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 10: Cấu trúc viết bài Opinion Essay (Agree or Disagree) Band 7.5+ (B2 - C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_261",
                TopicId = 7, LevelId = 4, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "In an IELTS Writing Task 2 Opinion Essay, what two elements MUST be included in the Introduction paragraph?",
                Explanation = "Mở bài Task 2 chuẩn gồm 2 phần: Paraphrase lại đề bài (Background statement) và Nêu rõ lập trường cá nhân (Thesis statement).",
                Citation = "Bài học 10 - Cấu trúc mở bài Task 2: Paraphrase topic + Clear Thesis Statement.",
                SourceType = "Extracted", SourceFile = "BaiHoc_10_OpinionEssayBand7.txt", LessonId = 10,
                Options = new() { ("A", "A funny joke and personal phone number", false), ("B", "A paraphrase of the prompt prompt and a clear thesis statement stating your position", true), ("C", "A list of bullet points and dictionary definitions", false), ("D", "A summary of counter-arguments with no personal stance", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_262",
                TopicId = 7, LevelId = 5, QuestionType = "TuLuan", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "IELTS Writing Task 2 Prompt:\n'Some people believe that artificial intelligence will replace human teachers in the future. To what extent do you agree or disagree?'\nDraft a 250-word discursive essay with a clear thesis, two body paragraphs, and a conclusion.",
                Explanation = "Tiêu chí chấm điểm Band 7.5+: Task Response (trả lời đầy đủ, lập trường rõ ràng), Coherence & Cohesion (liên kết đoạn mạch lạc), Lexical Resource (từ vựng học thuật C1), Grammatical Range & Accuracy (câu phức, đảo ngữ, bị động).",
                Citation = "Bài học 10 - Tiêu chuẩn chấm bài IELTS Writing Task 2 theo 4 tiêu chí của British Council.",
                SourceType = "Extracted", SourceFile = "BaiHoc_10_OpinionEssayBand7.txt", LessonId = 10,
                Options = new() { ("RUBRIC", "Task Achievement (25%), Coherence & Cohesion (25%), Lexical Resource (25%), Grammatical Range (25%)", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_263",
                TopicId = 7, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Which academic linking phrase expresses a strong counter-argument?",
                Explanation = "'On the other hand' hoặc 'Conversely' biểu thị quan điểm tương phản học thuật.",
                Citation = "Bài học 10 - Từ nối học thuật Task 2: Furthermore, In contrast, Conversely.",
                SourceType = "Extracted", SourceFile = "BaiHoc_10_OpinionEssayBand7.txt", LessonId = 10,
                Options = new() { ("A", "By the way", false), ("B", "On the other hand / Conversely", true), ("C", "Also guess what", false), ("D", "Anyway", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_264",
                TopicId = 7, LevelId = 4, QuestionType = "DienKhuyet", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Complete the concluding phrase: 'In _______, while technology offers benefits, human educators remain indispensable.'",
                Explanation = "'In conclusion' (Tóm lại) là cụm từ kết luận chuẩn mực trong văn bản nghị luận Task 2.",
                Citation = "Bài học 10 - Phần Kết bài: 'In conclusion / To conclude...'",
                SourceType = "Extracted", SourceFile = "BaiHoc_10_OpinionEssayBand7.txt", LessonId = 10,
                Options = new() { ("A", "conclusion", true), ("B", "end", false), ("C", "final", false), ("D", "finish", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_265",
                TopicId = 7, LevelId = 4, QuestionType = "DungSai", Skill = "Viet", Difficulty = "De",
                QuestionText = "True or False: Writing fewer than 250 words in IELTS Writing Task 2 incurs an automatic penalty on the Task Response score.",
                Explanation = "Đúng. Viết dưới 250 từ ở Task 2 bị trừ điểm tiêu chí Task Response do bài chưa phát triển đầy đủ ý.",
                Citation = "Bài học 10 - Quy định độ dài Task 2: Tối thiểu 250 từ.",
                SourceType = "Extracted", SourceFile = "BaiHoc_10_OpinionEssayBand7.txt", LessonId = 10,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 11: Dạng bài Problem & Solution Essay (Môi trường & Đô thị hóa) (B2 - C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_266",
                TopicId = 7, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Which advanced collocations best describe severe urban environmental challenges?",
                Explanation = "'Unprecedented smog levels' (mức độ khói bụi chưa từng có) và 'unchecked carbon emissions' (khí thải carbon không kiểm soát) là từ vựng C1 chuẩn xác.",
                Citation = "Bài học 11 - Bộ từ vựng chủ đề Đô thị hóa & Môi trường Band C1: Unprecedented smog, unchecked emissions.",
                SourceType = "Extracted", SourceFile = "BaiHoc_11_ProblemSolutionEssay.txt", LessonId = 11,
                Options = new() { ("A", "Bad dirty air and dirty rivers", false), ("B", "Unprecedented smog levels and unchecked carbon emissions", true), ("C", "Smoke from cooking food", false), ("D", "Loud street noise from bikes", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_267",
                TopicId = 7, LevelId = 5, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "In a Problem-Solution essay, what must each proposed solution directly address?",
                Explanation = "Mỗi giải pháp đề xuất trong Body 2 bắt buộc phải tương ứng và giải quyết trực tiếp một nguyên nhân hoặc vấn đề đã nêu ở Body 1.",
                Citation = "Bài học 11 - Nguyên tắc gắn kết Cause - Effect - Solution: Giải pháp phải ánh xạ tương ứng vấn đề.",
                SourceType = "Extracted", SourceFile = "BaiHoc_11_ProblemSolutionEssay.txt", LessonId = 11,
                Options = new() { ("A", "Random worldwide dilemmas", false), ("B", "The specific causes and problems identified in the preceding paragraph", true), ("C", "The author's childhood memories", false), ("D", "Historical battles from centuries ago", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_268",
                TopicId = 7, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Fill in the academic term: 'Governments should enact stricter legislation to _______ corporate polluters.'",
                Explanation = "'penalize' (xử phạt) các doanh nghiệp gây ô nhiễm môi trường.",
                Citation = "Bài học 11 - Từ vựng giải pháp chính sách: Enact legislation, Penalize corporate polluters.",
                SourceType = "Extracted", SourceFile = "BaiHoc_11_ProblemSolutionEssay.txt", LessonId = 11,
                Options = new() { ("A", "penalize", true), ("B", "reward", false), ("C", "praise", false), ("D", "ignore", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_269",
                TopicId = 7, LevelId = 5, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Rewrite using 'result in': 'Rapid urbanization causes traffic congestion and habitat loss.'",
                Explanation = "'Rapid urbanization results in severe traffic congestion and habitat loss.'",
                Citation = "Bài học 11 - Cấu trúc diễn đạt quan hệ nhân quả: lead to / result in / give rise to.",
                SourceType = "Applied", SourceFile = "BaiHoc_11_ProblemSolutionEssay.txt", LessonId = 11,
                Options = new() { ("A", "Rapid urbanization results in severe traffic congestion and habitat loss.", true), ("B", "Traffic congestion results in urbanization rapidly.", false), ("C", "Rapid urbanization is resulted of traffic.", false), ("D", "Habitat loss resulting rapid urbanization.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_270",
                TopicId = 7, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: The term 'renewable energy' refers to natural energy resources that do not deplete, such as solar, wind, and hydroelectric power.",
                Explanation = "Đúng. Năng lượng tái tạo (renewable energy) bao gồm năng lượng mặt trời, gió và thủy điện.",
                Citation = "Bài học 11 - Thuật ngữ môi trường cốt lõi: Renewable energy resources.",
                SourceType = "Extracted", SourceFile = "BaiHoc_11_ProblemSolutionEssay.txt", LessonId = 11,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 12: Chiến lược chinh phục IELTS Listening Section 3 (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_271",
                TopicId = 8, LevelId = 4, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "TrungBinh",
                QuestionText = "In IELTS Listening Section 3, what context is always depicted?",
                Explanation = "IELTS Listening Section 3 luôn là cuộc thảo luận học thuật từ 2 đến 4 người (thường là giữa sinh viên với nhau hoặc sinh viên với giáo sư hướng dẫn).",
                Citation = "Bài học 12 - Đặc điểm Section 3: Cuộc thảo luận học thuật nhóm (2-4 người).",
                SourceType = "Extracted", SourceFile = "BaiHoc_12_IeltsListeningSec3.txt", LessonId = 12,
                Options = new() { ("A", "A monologue advertising grocery discounts", false), ("B", "An academic discussion between 2-4 speakers (students and tutors) about an educational assignment or research project", true), ("C", "A single person giving a bedtime story", false), ("D", "A song recital", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_272",
                TopicId = 8, LevelId = 4, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "Kho",
                QuestionText = "What listening trap is most prevalent in Section 3 when speakers change their minds?",
                Explanation = "Bẫy thay đổi ý định (Distractor / Self-correction): Người nói đưa ra một ý kiến nhưng sau đó nói 'Actually, on second thought...' và chọn phương án khác.",
                Citation = "Bài học 12 - Bẫy sửa đổi quyết định (Self-correction & Distractor trap).",
                SourceType = "Extracted", SourceFile = "BaiHoc_12_IeltsListeningSec3.txt", LessonId = 12,
                Options = new() { ("A", "Distractor trap where speakers initially agree on a plan but revise it with words like 'Actually' or 'However'", true), ("B", "The audio speed dropping to zero", false), ("C", "Speakers speaking in foreign languages", false), ("D", "Background music drowning out all speech", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_273",
                TopicId = 8, LevelId = 4, QuestionType = "DienKhuyet", Skill = "Nghe", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'Before listening begins, candidates have 30 seconds to _______ the question stems and underline keywords.'",
                Explanation = "'analyze' hoặc 'read' các câu hỏi và gạch chân từ khóa cốt lõi.",
                Citation = "Bài học 12 - Tận dụng 30 giây chuẩn bị trước mỗi Section.",
                SourceType = "Extracted", SourceFile = "BaiHoc_12_IeltsListeningSec3.txt", LessonId = 12,
                Options = new() { ("A", "analyze", true), ("B", "ignore", false), ("C", "erase", false), ("D", "memorize", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_274",
                TopicId = 8, LevelId = 4, QuestionType = "DungSai", Skill = "Nghe", Difficulty = "De",
                QuestionText = "True or False: In Section 3 multiple matching questions, options are often paraphrased using synonyms rather than repeated word-for-word.",
                Explanation = "Đúng. Đề thi luôn diễn đạt lại ý bằng từ đồng nghĩa (paraphrasing).",
                Citation = "Bài học 12 - Kỹ thuật nhận diện Paraphrasing trong Section 3.",
                SourceType = "Extracted", SourceFile = "BaiHoc_12_IeltsListeningSec3.txt", LessonId = 12,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_275",
                TopicId = 8, LevelId = 4, QuestionType = "TraLoiNgan", Skill = "Nghe", Difficulty = "TrungBinh",
                QuestionText = "In IELTS Listening, what is the maximum word limit when instructions say 'NO MORE THAN TWO WORDS AND/OR A NUMBER'?",
                Explanation = "Thí sinh chỉ được viết tối đa 2 từ và/hoặc 1 con số. Viết từ thứ 3 sẽ bị 0 điểm.",
                Citation = "Bài học 12 - Quy tắc đếm từ trong bài thi Nghe IELTS.",
                SourceType = "Extracted", SourceFile = "BaiHoc_12_IeltsListeningSec3.txt", LessonId = 12,
                Options = new() { ("A", "Two words and/or a number", true), ("B", "Three words", false), ("C", "Four words", false), ("D", "Unlimited", false) }
            });

            // --- BÀI HỌC 13: Kỹ thuật bắt từ khóa trong bài giảng học thuật Section 4 (C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_276",
                TopicId = 8, LevelId = 5, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "Kho",
                QuestionText = "In IELTS Listening Section 4, what is unique about the audio delivery compared to earlier sections?",
                Explanation = "Section 4 là một bài giảng học thuật độc thoại (monologue lecture) kéo dài liên tục không có thời gian tạm nghỉ giữa chừng.",
                Citation = "Bài học 13 - Đặc thù Section 4: Độc thoại học thuật liên tục không ngắt quãng giữa chừng.",
                SourceType = "Extracted", SourceFile = "BaiHoc_13_IeltsListeningSec4.txt", LessonId = 13,
                Options = new() { ("A", "There is a 2-minute music break in the middle.", false), ("B", "It is an uninterrupted university monologue lecture without any mid-section pauses.", true), ("C", "The speaker asks the test takers questions directly.", false), ("D", "Audio is played three times.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_277",
                TopicId = 8, LevelId = 5, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "Kho",
                QuestionText = "How do lecture signpost words (e.g., 'Turning now to...', 'In addition to this...') assist listeners in Section 4?",
                Explanation = "Từ chỉ dẫn (signpost words) báo hiệu giáo sư chuyển sang mục tiếp theo trong dàn ý ghi chú, giúp thí sinh không bị lạc khỏi vị trí câu hỏi.",
                Citation = "Bài học 13 - Kỹ thuật bám sát cấu trúc bài giảng bằng Signpost words.",
                SourceType = "Extracted", SourceFile = "BaiHoc_13_IeltsListeningSec4.txt", LessonId = 13,
                Options = new() { ("A", "They warn students that the exam has concluded.", false), ("B", "They signal transitions to new bullet points and prevent test takers from losing their place in the notes.", true), ("C", "They provide humorous distractions.", false), ("D", "They define simple grammar rules.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_278",
                TopicId = 8, LevelId = 5, QuestionType = "DienKhuyet", Skill = "Nghe", Difficulty = "Kho",
                QuestionText = "Fill in the blank: 'In Section 4 note completion, answers must be spelled _______; misspelled words are marked incorrect.'",
                Explanation = "Trong IELTS Listening, từ điền vào phải viết đúng chính tả (correctly/accurately); sai chính tả bị tính là sai.",
                Citation = "Bài học 13 - Quy định tính điểm chính tả trong IELTS Listening.",
                SourceType = "Extracted", SourceFile = "BaiHoc_13_IeltsListeningSec4.txt", LessonId = 13,
                Options = new() { ("A", "accurately", true), ("B", "loosely", false), ("C", "randomly", false), ("D", "vaguely", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_279",
                TopicId = 8, LevelId = 5, QuestionType = "TraLoiNgan", Skill = "Nghe", Difficulty = "Kho",
                QuestionText = "If the listening audio says 'twenty percent of marine species', and the note template says '_______ of ocean organisms', what goes in the blank?",
                Explanation = "Cụm cần điền là '20%' hoặc 'twenty percent' (marine species đã được paraphrase thành ocean organisms).",
                Citation = "Bài học 13 - Ví dụ mẫu Paraphrase trong bài giảng sinh học đại dương.",
                SourceType = "Extracted", SourceFile = "BaiHoc_13_IeltsListeningSec4.txt", LessonId = 13,
                Options = new() { ("A", "20%", true), ("B", "50%", false), ("C", "Zero", false), ("D", "All", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_280",
                TopicId = 8, LevelId = 5, QuestionType = "DungSai", Skill = "Nghe", Difficulty = "De",
                QuestionText = "True or False: In computer-delivered IELTS, candidates have 2 minutes at the end of the Listening test to review their answers, compared to 10 minutes in paper-based tests.",
                Explanation = "Đúng. Thi máy có 2 phút kiểm tra lại, thi giấy có 10 phút chép đáp án sang Answer Sheet.",
                Citation = "Bài học 13 - Khác biệt giữa IELTS trên máy tính và trên giấy.",
                SourceType = "Extracted", SourceFile = "BaiHoc_13_IeltsListeningSec4.txt", LessonId = 13,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BỔ SUNG CÁC CÂU HỎI HỌC THUẬT IELTS & NGHIÊN CỨU (B2 - C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_281",
                TopicId = 6, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "What is the primary function of a literature review in an academic research paper?",
                Explanation = "Tổng quan y văn (Literature review) nhằm khảo sát, phân tích và tổng hợp các nghiên cứu đã công bố liên quan đến đề tài.",
                Citation = "Bài học 8 - Cấu trúc bài báo nghiên cứu khoa học: Literature review.",
                SourceType = "Extracted", SourceFile = "BaiHoc_08_IeltsSkimmingScanning.txt", LessonId = 8,
                Options = new() { ("A", "To critique poetry and novels for entertainment", false), ("B", "To survey, analyze, and synthesize previously published scholarly research on the subject", true), ("C", "To advertise research equipment for sale", false), ("D", "To thank personal family friends", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_282",
                TopicId = 6, LevelId = 5, QuestionType = "TrueFalseNotGiven", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "Excerpt: 'Empirical data demonstrated a moderate correlation between sleep deprivation and cognitive decline, though causation was not conclusively proven.' Determine: 'Researchers proved that lack of sleep directly causes cognitive decline.'",
                ReadingPassage = "Empirical data demonstrated a moderate correlation between sleep deprivation and cognitive decline, though causation was not conclusively proven.",
                Explanation = "Văn bản khẳng định: 'though causation was not conclusively proven' (mối quan hệ nhân quả chưa được chứng minh dứt khoát). Câu hỏi bảo 'đã chứng minh gây ra' là sai trái với bài -> FALSE.",
                Citation = "Bài học 9 - Bẫy Correlation (tương quan) vs. Causation (nhân quả) trong IELTS Reading.",
                SourceType = "Applied", SourceFile = "BaiHoc_09_TrueFalseNotGiven.txt", LessonId = 9,
                Options = new() { ("A", "True", false), ("B", "False", true), ("C", "Not Given", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_283",
                TopicId = 7, LevelId = 4, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Which academic introductory phrase introduces an opposing viewpoint politely?",
                Explanation = "'While it is often argued that... I would contend that...' là cấu trúc nhượng bộ phản biện cao cấp.",
                Citation = "Bài học 10 - Mẫu câu nhượng bộ nâng cao: While it is argued that X, I would contend that Y.",
                SourceType = "Extracted", SourceFile = "BaiHoc_10_OpinionEssayBand7.txt", LessonId = 10,
                Options = new() { ("A", "People are silly to think that", false), ("B", "While it is commonly argued that X, I would contend that Y", true), ("C", "Nobody knows anything except me that", false), ("D", "Whatever people say is invalid because", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_284",
                TopicId = 7, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Complete the academic collocations: 'Over-reliance on fossil fuels exacerbates climate change and leads to severe _______ degradation.'",
                Explanation = "'environmental degradation' (suy thoái môi trường) là cụm từ học thuật C1 chuẩn xác.",
                Citation = "Bài học 11 - Collocation môi trường: Environmental degradation, habitat fragmentation.",
                SourceType = "Extracted", SourceFile = "BaiHoc_11_ProblemSolutionEssay.txt", LessonId = 11,
                Options = new() { ("A", "environmental", true), ("B", "environmentally", false), ("C", "environmentalist", false), ("D", "environment", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_285",
                TopicId = 6, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "When skimming an academic text, which elements should the reader pay special attention to?",
                Explanation = "Khi skim bài đọc, cần chú ý tiêu đề chính (headings), tiêu đề phụ (sub-headings), câu đầu/cuối đoạn và các hình ảnh/biểu đồ.",
                Citation = "Bài học 8 - Kỹ năng Skim bài đọc: Headings, Subheadings, Visuals and Paragraph Openers.",
                SourceType = "Extracted", SourceFile = "BaiHoc_08_IeltsSkimmingScanning.txt", LessonId = 8,
                Options = new() { ("A", "The page numbers and font size", false), ("B", "Headings, subheadings, topic sentences, and visual data graphs", true), ("C", "Every single preposition and comma", false), ("D", "Only the bibliography at the very end", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_286",
                TopicId = 6, LevelId = 5, QuestionType = "TrueFalseNotGiven", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "Excerpt: 'The archeological team uncovered fragments of pottery dating back to the Bronze Age in the northern sector of the excavation site.' Determine: 'The archeologists found gold jewelry alongside the pottery.'",
                ReadingPassage = "The archeological team uncovered fragments of pottery dating back to the Bronze Age in the northern sector of the excavation site.",
                Explanation = "Văn bản chỉ đề cập đến các mảnh gốm (pottery fragments), hoàn toàn không đề cập đến trang sức vàng. Đáp án là NOT GIVEN.",
                Citation = "Bài học 9 - Phân biệt thông tin chưa được kiểm chứng trong bài -> NOT GIVEN.",
                SourceType = "Extracted", SourceFile = "BaiHoc_09_TrueFalseNotGiven.txt", LessonId = 9,
                Options = new() { ("A", "True", false), ("B", "False", false), ("C", "Not Given", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_287",
                TopicId = 7, LevelId = 4, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "What is the consequence of writing an essay with memorized template sentences that do not fit the specific topic?",
                Explanation = "Việc học vẹt câu mẫu không ăn khớp với đề bài sẽ bị giám khảo trừ điểm nặng ở tiêu chí Task Response và Coherence.",
                Citation = "Bài học 10 - Cảnh báo lỗi học vẹt Memorized templates trong IELTS Writing.",
                SourceType = "Extracted", SourceFile = "BaiHoc_10_OpinionEssayBand7.txt", LessonId = 10,
                Options = new() { ("A", "It automatically awards a Band 9.0 score.", false), ("B", "It results in penalties under Task Response and Coherence because arguments appear generic and disconnected from the prompt.", true), ("C", "Examiners will invite the student for tea.", false), ("D", "No effect on scores whatsoever.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_288",
                TopicId = 8, LevelId = 4, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "TrungBinh",
                QuestionText = "In IELTS Listening, if you miss an answer in Section 2 or 3, what should you do immediately?",
                Explanation = "Chiến lược tâm lý phòng thi: Bỏ qua ngay câu đã lỡ để tập trung nghe câu kế tiếp, tuyệt đối không dừng lại hoảng loạn làm mất chuỗi các câu sau.",
                Citation = "Bài học 12 - Chiến lược xử lý sự cố phòng thi: Move on immediately if you miss a question.",
                SourceType = "Extracted", SourceFile = "BaiHoc_12_IeltsListeningSec3.txt", LessonId = 12,
                Options = new() { ("A", "Stop listening and cry loudly.", false), ("B", "Let it go immediately and refocus on the next question to avoid losing the entire stream of answers.", true), ("C", "Ask the exam invigilator to pause the audio recording.", false), ("D", "Leave the examination hall right away.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_289",
                TopicId = 7, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Complete the academic conclusion: 'In summary, tackling climate change requires concerted global efforts and innovative technological _______.'",
                Explanation = "'solutions' (các giải pháp công nghệ đổi mới sáng tạo).",
                Citation = "Bài học 11 - Từ vựng kết luận bài nghị luận môi trường: Concerted global efforts.",
                SourceType = "Extracted", SourceFile = "BaiHoc_11_ProblemSolutionEssay.txt", LessonId = 11,
                Options = new() { ("A", "solutions", true), ("B", "problems", false), ("C", "failures", false), ("D", "debts", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_290",
                TopicId = 6, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "What does the Latin academic abbreviation 'e.g.' mean in scholarly texts?",
                Explanation = "'e.g.' là viết tắt của 'exempli gratia' (nghĩa là 'for example' - ví dụ như). Khác với 'i.e.' nghĩa là 'that is' (tức là).",
                Citation = "Bài học 8 - Các từ viết tắt học thuật phổ biến: e.g. (for example), i.e. (that is), et al. (and others).",
                SourceType = "Extracted", SourceFile = "BaiHoc_08_IeltsSkimmingScanning.txt", LessonId = 8,
                Options = new() { ("A", "For example (exempli gratia)", true), ("B", "In conclusion", false), ("C", "That is to say", false), ("D", "Without question", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_291",
                TopicId = 6, LevelId = 5, QuestionType = "TrueFalseNotGiven", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "Excerpt: 'The preliminary findings were restricted to laboratory mice; trials on human subjects have yet to be approved by the ethics committee.' Determine: 'The pharmaceutical treatment has been extensively tested on human patients.'",
                ReadingPassage = "The preliminary findings were restricted to laboratory mice; trials on human subjects have yet to be approved by the ethics committee.",
                Explanation = "Văn bản nêu rõ thử nghiệm trên người 'chưa được phê duyệt' (have yet to be approved), mâu thuẫn hoàn toàn với mệnh đề 'đã được thử nghiệm rộng rãi trên người'. Đáp án là FALSE.",
                Citation = "Bài học 9 - Phân tích câu mâu thuẫn trực tiếp (Contradiction -> FALSE).",
                SourceType = "Applied", SourceFile = "BaiHoc_09_TrueFalseNotGiven.txt", LessonId = 9,
                Options = new() { ("A", "True", false), ("B", "False", true), ("C", "Not Given", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_292",
                TopicId = 7, LevelId = 4, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Which sentence best avoids overgeneralization by using academic hedging?",
                Explanation = "'Evidence suggests that regular physical exercise tends to improve cognitive memory in older adults.' sử dụng kỹ thuật 'hedging' (suggests, tends to) để đưa ra khẳng định khoa học cẩn trọng, không tuyệt đối hóa.",
                Citation = "Bài học 10 - Kỹ thuật Hedging trong văn viết học thuật Band 7.5+.",
                SourceType = "Extracted", SourceFile = "BaiHoc_10_OpinionEssayBand7.txt", LessonId = 10,
                Options = new() { ("A", "Exercise always cures all mental illnesses without exception.", false), ("B", "Evidence suggests that regular physical exercise tends to improve cognitive memory in older adults.", true), ("C", "Without sports everyone becomes completely foolish.", false), ("D", "Exercise is 100% the only thing brains need.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_293",
                TopicId = 8, LevelId = 5, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "Kho",
                QuestionText = "In Section 4 lectures, which word indicates that the professor is presenting an exception to a general theory?",
                Explanation = "'However', 'Nonetheless', hoặc 'An exception to this rule is...' báo hiệu ngoại lệ của lý thuyết.",
                Citation = "Bài học 13 - Nhận biết từ tín hiệu tương phản trong bài giảng học thuật: Exception, However, Nonetheless.",
                SourceType = "Extracted", SourceFile = "BaiHoc_13_IeltsListeningSec4.txt", LessonId = 13,
                Options = new() { ("A", "Furthermore", false), ("B", "Nonetheless / However", true), ("C", "Similarly", false), ("D", "Consequently", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_294",
                TopicId = 6, LevelId = 4, QuestionType = "DienKhuyet", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'IELTS Academic Reading texts are sourced from books, journals, magazines, and _______ written for a non-specialist audience.'",
                Explanation = "Văn bản đọc IELTS được trích xuất từ sách, tạp chí, tập san và báo chí (newspapers).",
                Citation = "Bài học 8 - Nguồn ngữ liệu đề thi IELTS Reading: Books, journals, magazines and newspapers.",
                SourceType = "Extracted", SourceFile = "BaiHoc_08_IeltsSkimmingScanning.txt", LessonId = 8,
                Options = new() { ("A", "newspapers", true), ("B", "novels", false), ("C", "comics", false), ("D", "scripts", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_295",
                TopicId = 7, LevelId = 5, QuestionType = "TuLuan", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "IELTS Writing Task 2 Problem-Solution Prompt:\n'In many modern megacities, traffic congestion and air pollution have reached critical levels. Discuss the primary causes of this phenomenon and propose viable governmental solutions.'\nProvide a structured outline including 2 main causes and 2 corresponding solutions.",
                Explanation = "Đề bài yêu cầu lập dàn ý chi tiết phân tích 2 nguyên nhân (quá tải xe cá nhân, cơ sở hạ tầng thiếu đồng bộ) và 2 giải pháp (phát triển giao thông công cộng metro, đánh thuế phát thải carbon).",
                Citation = "Bài học 11 - Hướng dẫn dàn bài Problem & Solution chuẩn Band 7.5+.",
                SourceType = "Extracted", SourceFile = "BaiHoc_11_ProblemSolutionEssay.txt", LessonId = 11,
                Options = new() { ("RUBRIC", "Causes: Private vehicle overload & inadequate public transit. Solutions: Metro expansion & Congestion pricing.", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_296",
                TopicId = 6, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "In IELTS Reading, how should you transfer your answers to the Answer Sheet?",
                Explanation = "Không giống như Listening (có thêm thời gian chép bài ở cuối bài thi giấy), IELTS Reading KHÔNG CÓ thời gian chuyển đáp án riêng; thí sinh phải ghi trực tiếp đáp án vào phiếu trong vòng 60 phút.",
                Citation = "Bài học 8 - Lưu ý chí mạng về thời gian làm bài IELTS Reading: Ghi trực tiếp đáp án trong 60 phút.",
                SourceType = "Extracted", SourceFile = "BaiHoc_08_IeltsSkimmingScanning.txt", LessonId = 8,
                Options = new() { ("A", "Wait for the 10-minute transfer period at the end of the test.", false), ("B", "Write answers directly onto the answer sheet as you complete each passage, because no extra transfer time is provided.", true), ("C", "Tell the invigilator your answers verbally.", false), ("D", "Only write answers on the question booklet.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_297",
                TopicId = 8, LevelId = 4, QuestionType = "DungSai", Skill = "Nghe", Difficulty = "De",
                QuestionText = "True or False: In IELTS Listening, writing '10 Downing St' when the audio says '10 Downing Street' is acceptable if word count permits.",
                Explanation = "Đúng. Viết tắt thông dụng như 'St' cho 'Street' hoặc 'Ave' cho 'Avenue' được chấp nhận nếu đúng nghĩa và trong giới hạn số từ.",
                Citation = "Bài học 12 - Quy tắc chấp nhận chữ viết tắt địa chỉ thông dụng trong IELTS Listening.",
                SourceType = "Extracted", SourceFile = "BaiHoc_12_IeltsListeningSec3.txt", LessonId = 12,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_298",
                TopicId = 6, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What does the Latin abbreviation 'et al.' mean in academic in-text citations (e.g., Smith et al., 2024)?",
                Explanation = "'et al.' là viết tắt của 'et alia' (nghĩa là 'và các cộng sự' - and others), dùng khi bài báo có từ 3 tác giả trở lên.",
                Citation = "Bài học 8 - Trích dẫn học thuật quốc tế: et al. (and others).",
                SourceType = "Extracted", SourceFile = "BaiHoc_08_IeltsSkimmingScanning.txt", LessonId = 8,
                Options = new() { ("A", "And others (et alia)", true), ("B", "At all times", false), ("C", "End of article", false), ("D", "Edited along", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_299",
                TopicId = 7, LevelId = 4, QuestionType = "DienKhuyet", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank with an academic adverb: 'Public transit investments have _______ reduced carbon emissions in several European metropolitan areas.'",
                Explanation = "'substantially' (đáng kể) là phó từ học thuật C1 chuẩn xác để mô tả mức độ tác động.",
                Citation = "Bài học 11 - Phó từ chỉ mức độ học thuật: Substantially, significantly, considerably.",
                SourceType = "Extracted", SourceFile = "BaiHoc_11_ProblemSolutionEssay.txt", LessonId = 11,
                Options = new() { ("A", "substantially", true), ("B", "a lot", false), ("C", "very much", false), ("D", "super", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_300",
                TopicId = 8, LevelId = 5, QuestionType = "TraLoiNgan", Skill = "Nghe", Difficulty = "Kho",
                QuestionText = "In Section 4, what is the term for a scholarly article that undergoes rigorous evaluation by other experts in the same field before publication?",
                Explanation = "'Peer-reviewed journal' hoặc 'Peer review' (bài báo được bình duyệt học thuật).",
                Citation = "Bài học 13 - Thuật ngữ quy trình nghiên cứu học thuật: Peer-reviewed journal.",
                SourceType = "Extracted", SourceFile = "BaiHoc_13_IeltsListeningSec4.txt", LessonId = 13,
                Options = new() { ("A", "Peer-reviewed article / journal", true), ("B", "Popular magazine", false), ("C", "Blog post", false), ("D", "Daily tabloid", false) }
            });

            return list;
        }
    }
}

