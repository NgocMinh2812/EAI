using System.Collections.Generic;

namespace QuestionBuilder
{
    public static class Batch04
    {
        public static List<QuestionItem> GetQuestions()
        {
            var list = new List<QuestionItem>();

            // --- TÀI LIỆU: 01_TOEIC_Listening_Part1_Photographs.docx (B1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_151",
                TopicId = 8, LevelId = 3, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "TrungBinh",
                QuestionText = "In TOEIC Part 1, why is the statement 'The bicycle is being repaired' incorrect if the photo shows bicycles parked on an empty street with no mechanic present?",
                ReadingPassage = "Statement A: A man is fixing a bicycle wheel.\nStatement B: Some boxes are stacked on the shelf.\nStatement C: The woman is paying for merchandise.\nStatement D: A pedestrian is crossing the street.",
                Explanation = "Trong TOEIC Part 1, cấu trúc bị động tiếp diễn 'is being + V3' bắt buộc phải có người đang trực tiếp thực hiện hành động. Nếu không có người thao tác, câu này chắc chắn là bẫy.",
                Citation = "Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Mục 2: Bẫy Be + V-ing vs Be + being + V3: 'Tranh không có người thao tác nhưng đáp án dùng is being repaired là SAI.'",
                SourceType = "Extracted", SourceFile = "01_TOEIC_Listening_Part1_Photographs.docx",
                Options = new() { ("A", "Because bicycles cannot be repaired anywhere.", false), ("B", "Because 'is being + V3' requires a human subject actively performing the action in the photograph.", true), ("C", "Because bicycles are always plural.", false), ("D", "Because 'repair' is an informal verb.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_152",
                TopicId = 8, LevelId = 3, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "De",
                QuestionText = "Which common pronunciation trap does ETS use in TOEIC Part 1 to confuse listeners with similar sounding words?",
                Explanation = "ETS thường dùng các cặp từ phát âm gần giống nhau (similar sounding words) như walking/working, coffee/copy để đánh bẫy thí sinh.",
                Citation = "Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Mục 2: Bẫy từ đồng âm / gần âm: walking vs working, coffee vs copy.",
                SourceType = "Extracted", SourceFile = "01_TOEIC_Listening_Part1_Photographs.docx",
                Options = new() { ("A", "Walking vs. Working", true), ("B", "Table vs. Chair", false), ("C", "Elephant vs. Cat", false), ("D", "Computer vs. Mouse", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_153",
                TopicId = 8, LevelId = 3, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "TrungBinh",
                QuestionText = "Look at the sample statement from Part 1: 'Some boxes are stacked on the shelf.' What does 'stacked' describe?",
                Explanation = "'Stacked' miêu tả trạng thái các hộp được xếp chồng ngay ngắn lên nhau trên kệ.",
                Citation = "Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Mục 3: Ví dụ mẫu: 'Statement B: Some boxes are stacked on the shelf.'",
                SourceType = "Extracted", SourceFile = "01_TOEIC_Listening_Part1_Photographs.docx",
                Options = new() { ("A", "Boxes being torn apart", false), ("B", "Boxes neatly arranged in a pile on top of one another", true), ("C", "Boxes falling down the stairs", false), ("D", "Boxes being burnt", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_154",
                TopicId = 8, LevelId = 3, QuestionType = "DungSai", Skill = "Nghe", Difficulty = "De",
                QuestionText = "True or False: In TOEIC Part 1, an answer that makes an exaggerated assumption not visible in the picture (e.g., 'He is thinking about his wife') is typically incorrect.",
                Explanation = "Đúng. TOEIC Part 1 chỉ chấp nhận các mô tả khách quan, nhìn thấy rõ ràng trên tranh. Mọi suy đoán cảm xúc hoặc ý nghĩ đều là đáp án sai.",
                Citation = "Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Nguyên tắc chọn đáp án miêu tả khách quan trực quan.",
                SourceType = "Extracted", SourceFile = "01_TOEIC_Listening_Part1_Photographs.docx",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_155",
                TopicId = 8, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Which verb is used in TOEIC Part 1 to describe a person who is wearing a watch or glasses (state of wearing)?",
                Explanation = "Trong TOEIC, 'is wearing' miêu tả trạng thái đang mặc/đeo đồ, còn 'is putting on' miêu tả hành động đang xỏ tay vào áo hay đeo kính.",
                Citation = "Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Phân biệt Wearing (trạng thái) vs. Putting on (hành động).",
                SourceType = "Extracted", SourceFile = "01_TOEIC_Listening_Part1_Photographs.docx",
                Options = new() { ("A", "is putting on", false), ("B", "is wearing", true), ("C", "is taking off", false), ("D", "is holding up", false) }
            });

            // --- TÀI LIỆU: 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf (B1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_156",
                TopicId = 10, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct word to complete the TOEIC sentence: 'The marketing department completed the project _______ despite tight deadlines.'",
                Explanation = "Cần một trạng từ (Adverb) để bổ nghĩa cho động từ 'completed'. 'successfully' là trạng từ mang nghĩa 'một cách thành công'.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Quy tắc vị trí trạng từ: Động từ + Tân ngữ + Trạng từ bổ nghĩa.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "success", false), ("B", "successful", false), ("C", "successfully", true), ("D", "succeed", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_157",
                TopicId = 10, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'Ms. Jenkins reviewed the financial audit _______ before submitting it to the board.'",
                Explanation = "Đại từ phản thân 'herself' được dùng ở vị trí nhấn mạnh chủ ngữ: chính cô ấy đã kiểm tra bản kiểm toán.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Chuyên đề đại từ: Đại từ phản thân (Reflexive Pronouns) nhấn mạnh.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "she", false), ("B", "her", false), ("C", "herself", true), ("D", "hers", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_158",
                TopicId = 10, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Select the appropriate conjunction: '_______ the severe weather conditions, the cargo ship arrived at the port safely.'",
                Explanation = "'Despite' hoặc 'In spite of' đi với cụm danh từ (severe weather conditions) để chỉ sự nhượng bộ (mặc dù). 'Although' phải đi với mệnh đề S+V.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Phân biệt Although / Even though (+ Clause) vs. Despite / In spite of (+ Noun phrase).",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "Although", false), ("B", "Despite", true), ("C", "Because", false), ("D", "Unless", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_159",
                TopicId = 10, LevelId = 3, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Complete the corporate sentence: 'All employees are required to comply _______ the updated safety regulations.'",
                Explanation = "Cụm động từ cố định trong TOEIC: 'comply with' nghĩa là 'tuân thủ quy định/luật lệ'.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Cụm động từ kèm giới từ thông dụng: Comply with, Adhere to, Conform to.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "with", true), ("B", "for", false), ("C", "on", false), ("D", "at", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_160",
                TopicId = 10, LevelId = 3, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the error: 'The manager was pleased with how quick the staff responded to the inquiry.'",
                Explanation = "Bổ nghĩa cho động từ 'responded' phải là một phó từ: 'quickly', không thể dùng tính từ 'quick'.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Lỗi sai từ loại Tính từ vs Trạng từ trong Part 5.",
                SourceType = "Applied", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "'pleased' should be 'pleasure'", false), ("B", "'quick' should be 'quickly'", true), ("C", "'with' should be 'by'", false), ("D", "'inquiry' should be 'inquire'", false) }
            });

            // --- TÀI LIỆU: 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf (B1 - B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_161",
                TopicId = 8, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "According to the mini test reading passage, why was the annual leadership banquet rescheduled?",
                ReadingPassage = "MEMORANDUM\nTo: All Department Heads\nFrom: Facilities Management\nDate: October 12\nSubject: Annual Banquet Rescheduling\nPlease note that due to ongoing electrical renovations in the Grand Ballroom, the annual leadership banquet originally scheduled for November 5 has been postponed to November 19.",
                Explanation = "Văn bản nêu rõ nguyên nhân dời lịch: 'due to ongoing electrical renovations in the Grand Ballroom' (do đang cải tạo hệ thống điện).",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Bài đọc thông báo nội bộ (Internal Memo).",
                SourceType = "Extracted", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "Because of bad weather in another city", false), ("B", "Due to electrical renovations taking place in the Grand Ballroom", true), ("C", "Because the CEO cancelled the celebration", false), ("D", "Because of food delivery problems", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_162",
                TopicId = 8, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "On what date will the leadership banquet actually take place?",
                Explanation = "Ngày mới được ấn định là: 'has been postponed to November 19'.",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Thông tin ngày tổ chức mới.",
                SourceType = "Extracted", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "October 12", false), ("B", "November 5", false), ("C", "November 19", true), ("D", "December 1", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_163",
                TopicId = 10, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the best verb tense: 'By the time the new regional director arrives next week, the team _______ the audit report.'",
                Explanation = "Cấu trúc tương lai hoàn thành: 'By the time + S + V(hiện tại đơn), S + will have + V3'. Ở đây dùng 'will have finalized'.",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Cấu trúc thì nâng cao với By the time.",
                SourceType = "Extracted", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "finalized", false), ("B", "will have finalized", true), ("C", "finalizes", false), ("D", "has finalized", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_164",
                TopicId = 13, LevelId = 3, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Complete the corporate sentence: 'The HR department aims to attract highly _______ candidates for the software architect position.'",
                Explanation = "'highly qualified' (đủ trình độ chuyên môn cao) là tính từ ghép thông dụng trong tuyển dụng.",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Collocations tuyển dụng nhân sự: Highly qualified candidates.",
                SourceType = "Extracted", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "qualified", true), ("B", "qualify", false), ("C", "qualification", false), ("D", "qualifyingly", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_165",
                TopicId = 8, LevelId = 3, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: In a TOEIC business memo, the 'Date' line indicates the deadline by which the task must be completed.",
                Explanation = "Sai. Dòng 'Date' ở đầu thông báo là ngày phát hành văn bản, còn hạn chót được nêu bằng từ 'Deadline' hoặc 'Due date' trong nội dung.",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Cấu trúc tiêu đề Memo thương mại.",
                SourceType = "Extracted", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            // --- TÀI LIỆU: 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_166",
                TopicId = 8, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "In TOEIC Part 7 Double Passages, what is a cross-reference question?",
                Explanation = "Câu hỏi chéo (cross-reference question) yêu cầu thí sinh phải kết hợp thông tin từ cả 2 văn bản (ví dụ: email đặt hàng và bảng báo giá/hóa đơn) mới tìm ra đáp án.",
                Citation = "Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Chiến lược câu hỏi liên kết thông tin 2 đoạn (Cross-reference).",
                SourceType = "Extracted", SourceFile = "04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx",
                Options = new() { ("A", "A question about grammar rules only", false), ("B", "A question requiring the reader to synthesize and link clues from both Passage 1 and Passage 2 to deduce the correct answer", true), ("C", "A question asking for the font size of the text", false), ("D", "A question that has two identical answers", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_167",
                TopicId = 8, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Read the double passage summary: Document 1 is a conference schedule; Document 2 is an email from Dr. Adams stating he can only attend on the afternoon of Day 2. Which workshop will Dr. Adams be able to attend?",
                Explanation = "Thí sinh phải đối chiếu lịch trình Ngày 2 buổi chiều trong Tài liệu 1 với nguyện vọng của Dr. Adams trong Tài liệu 2.",
                Citation = "Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Bài tập mẫu đối chiếu lịch trình hội nghị và thư xác nhận.",
                SourceType = "Applied", SourceFile = "04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx",
                Options = new() { ("A", "Morning keynote on Day 1", false), ("B", "The workshop scheduled for Day 2 after 1:00 PM", true), ("C", "Breakfast meeting on Day 1", false), ("D", "Opening ceremony on Day 1", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_168",
                TopicId = 8, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'The updated price list will supersede and _______ all previous promotional catalogs.'",
                Explanation = "'replace' nghĩa là thay thế toàn bộ danh mục cũ.",
                Citation = "Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Từ vựng hợp đồng và biểu phí thương mại: Supersede / Replace.",
                SourceType = "Extracted", SourceFile = "04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx",
                Options = new() { ("A", "replace", true), ("B", "break", false), ("C", "hide", false), ("D", "copy", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_169",
                TopicId = 8, LevelId = 4, QuestionType = "TrueFalseNotGiven", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Based on double passages, determine: 'Dr. Adams requested reimbursement for his flight ticket.'",
                Explanation = "Nếu trong cả hai văn bản không hề nhắc đến việc hoàn trả tiền vé máy bay, câu trả lời chính xác là 'Not Given'.",
                Citation = "Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Kỹ thuật nhận diện thông tin không được đề cập (Not Given).",
                SourceType = "Applied", SourceFile = "04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx",
                Options = new() { ("A", "True", false), ("B", "False", false), ("C", "Not Given", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_170",
                TopicId = 8, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "What is the recommended reading strategy for Double Passages in TOEIC Part 7 to save time?",
                Explanation = "Chiến lược tối ưu: Đọc lướt tiêu đề & người gửi của cả hai đoạn, đọc câu hỏi trước để xác định cần tìm manh mối ở văn bản 1 hay văn bản 2.",
                Citation = "Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Chiến lược quản lý thời gian 55 giây/câu trong Part 7.",
                SourceType = "Extracted", SourceFile = "04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx",
                Options = new() { ("A", "Read every single word slowly three times before looking at questions.", false), ("B", "Skim headings and context, read questions first, then scan for linked clues across both documents.", true), ("C", "Guess all answers as B without reading.", false), ("D", "Only read the first line of passage 1.", false) }
            });

            // --- BỔ SUNG CÁC CÂU HỎI THƯƠNG MẠI & NGỮ PHÁP TOEIC PART 5 & 6 (B1 - B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_171",
                TopicId = 13, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct word: 'The company provides an _______ training program for newly hired software engineers.'",
                Explanation = "Trước danh từ 'training program' cần một tính từ. 'extensive' (chuyên sâu, toàn diện) là tính từ phù hợp nghĩa.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Vị trí tính từ đứng trước danh từ.",
                SourceType = "Applied", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "extension", false), ("B", "extensive", true), ("C", "extensively", false), ("D", "extend", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_172",
                TopicId = 13, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'Our customer support team is available _______ the clock to assist overseas clients.'",
                Explanation = "Thành ngữ thương mại 'around the clock' nghĩa là 24/7, liên tục suốt ngày đêm.",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Thành ngữ thời gian trong dịch vụ khách hàng: Around the clock.",
                SourceType = "Extracted", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "around", true), ("B", "beside", false), ("C", "under", false), ("D", "between", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_173",
                TopicId = 10, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Select the correct relative pronoun: 'The architect _______ designed the eco-friendly headquarters received an international award.'",
                Explanation = "Đại từ quan hệ thay thế cho danh từ chỉ người 'The architect' làm chủ ngữ là 'who'.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Mệnh đề quan hệ trong đề thi TOEIC: who, which, that, whose.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "which", false), ("B", "who", true), ("C", "whom", false), ("D", "whose", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_174",
                TopicId = 13, LevelId = 3, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Fill in the blank: 'Payment must be received _______ 30 days of the invoice date.'",
                Explanation = "Giới từ 'within' đi với khoảng thời gian: 'within 30 days' (trong vòng 30 ngày).",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Cụm từ thanh toán: Within 30 days.",
                SourceType = "Extracted", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "within", true), ("B", "among", false), ("C", "between", false), ("D", "along", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_175",
                TopicId = 10, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Choose the correct subjunctive form: 'The committee recommended that the contract _______ renegotiated immediately.'",
                Explanation = "Cấu trúc giả định (Subjunctive mood) sau động từ 'recommend that' yêu cầu động từ ở dạng nguyên thể không chia: 'be renegotiated'.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Thể giả định (Subjunctive Mood): recommend/require that S + (should) be + V3.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "is", false), ("B", "be", true), ("C", "was", false), ("D", "will be", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_176",
                TopicId = 13, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What is a 'vending machine'?",
                Explanation = "'Vending machine' là máy bán hàng tự động (nước ngọt, đồ ăn nhẹ) hoạt động bằng tiền xu hoặc thanh toán thẻ.",
                Citation = "Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Đồ vật phổ biến trong tranh văn phòng: Vending machine.",
                SourceType = "Extracted", SourceFile = "01_TOEIC_Listening_Part1_Photographs.docx",
                Options = new() { ("A", "A machine that washes cars", false), ("B", "An automated machine that dispenses snacks and drinks upon payment", true), ("C", "A printer for printing badges", false), ("D", "A vacuum cleaner", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_177",
                TopicId = 8, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "In TOEIC Part 6 (Text Completion), what type of item was added in the revised format?",
                Explanation = "Định dạng TOEIC mới bổ sung câu hỏi 'Sentence Insertion' (chọn cả một câu hoàn chỉnh phù hợp nhất để điền vào đoạn văn).",
                Citation = "Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Cấu trúc câu hỏi TOEIC hiện đại: Sentence Insertion.",
                SourceType = "Extracted", SourceFile = "04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx",
                Options = new() { ("A", "Drawing a diagram", false), ("B", "Inserting an entire sentence that fits the context logically", true), ("C", "Translating into another language", false), ("D", "Recording voice notes", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_178",
                TopicId = 10, LevelId = 3, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank with the correct preposition: 'The flight was cancelled due _______ technical difficulties.'",
                Explanation = "Cụm chỉ nguyên nhân: 'due to + Noun phrase' (do / bởi vì).",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Cụm liên từ chỉ nguyên nhân: Due to / Because of / Owing to.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "to", true), ("B", "of", false), ("C", "for", false), ("D", "with", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_179",
                TopicId = 13, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Which word refers to an official reduction in the price of goods or services?",
                Explanation = "'Discount' (chiết khấu, giảm giá).",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Từ vựng bán hàng và chiết khấu thương mại.",
                SourceType = "Applied", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "Tax", false), ("B", "Discount", true), ("C", "Penalty", false), ("D", "Interest", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_180",
                TopicId = 10, LevelId = 4, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Find the mistake: 'Hardly the manager had stepped into the room when the alarm went off.'",
                Explanation = "Khi trạng từ phủ định 'Hardly' đứng đầu câu, phải thực hiện đảo ngữ: 'Hardly had the manager stepped...'.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Đảo ngữ với phó từ phủ định (Hardly... when).",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "'Hardly the manager had' should be 'Hardly had the manager'", true), ("B", "'stepped' should be 'stepping'", false), ("C", "'when' should be 'than'", false), ("D", "'went off' should be 'gone off'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_181",
                TopicId = 8, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "What document lists items shipped to a customer along with prices and payment terms?",
                Explanation = "'Invoice' (Hóa đơn thương mại) ghi rõ danh mục hàng hóa, đơn giá và điều khoản thanh toán.",
                Citation = "Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Các dạng văn bản Part 7: Invoice, Purchase Order.",
                SourceType = "Extracted", SourceFile = "04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx",
                Options = new() { ("A", "Brochure", false), ("B", "Invoice", true), ("C", "Pamphlet", false), ("D", "Itinerary", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_182",
                TopicId = 10, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Select the correct form: 'The CEO insisted on _______ the site inspection personally.'",
                Explanation = "Sau giới từ 'on', động từ phải ở dạng V-ing: 'insisted on conducting'.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Động từ đi kèm giới từ + V-ing: insist on, succeed in.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "conduct", false), ("B", "conducting", true), ("C", "conducted", false), ("D", "conductor", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_183",
                TopicId = 13, LevelId = 3, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Fill in the blank: 'Please retain your ticket as proof of _______.'",
                Explanation = "'proof of purchase' là cụm cố định nghĩa là 'bằng chứng mua hàng'.",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Collocations bán lẻ: Proof of purchase.",
                SourceType = "Extracted", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "purchase", true), ("B", "cooking", false), ("C", "traveling", false), ("D", "sleeping", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_184",
                TopicId = 8, LevelId = 3, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "De",
                QuestionText = "In TOEIC Part 1, if a woman is walking down the stairs, what is the British / American synonym often heard?",
                Explanation = "'Descending the stairs' là từ vựng học thuật thường dùng trong TOEIC để miêu tả đi xuống cầu thang.",
                Citation = "Tài liệu 01_TOEIC_Listening_Part1_Photographs.docx - Từ vựng chuyển động: Ascending (đi lên) vs. Descending (đi xuống).",
                SourceType = "Extracted", SourceFile = "01_TOEIC_Listening_Part1_Photographs.docx",
                Options = new() { ("A", "Descending the stairs", true), ("B", "Climbing up the roof", false), ("C", "Sitting on a bench", false), ("D", "Waiting at the curb", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_185",
                TopicId = 10, LevelId = 3, QuestionType = "DungSai", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "True or False: In business English, 'Information' is an uncountable noun and can never take a plural '-s'.",
                Explanation = "Đúng. 'Information' là danh từ không đếm được, không bao giờ có dạng 'informations' hay dùng với mạo từ 'an'.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Danh từ không đếm được thông dụng: Information, Equipment, Furniture.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_186",
                TopicId = 13, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What does 'unanimously' mean in board meeting minutes: 'The board approved the merger unanimously.'?",
                Explanation = "'Unanimously' nghĩa là nhất trí 100%, không có phiếu chống nào.",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Thuật ngữ biên bản họp hội đồng: Approved unanimously.",
                SourceType = "Extracted", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "With strong disagreement", false), ("B", "With total agreement and no votes against", true), ("C", "After cancelling the company", false), ("D", "By secret lottery", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_187",
                TopicId = 10, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct correlative conjunction: 'The new software is _______ user-friendly but also highly secure.'",
                Explanation = "Cặp liên từ tương quan chuẩn: 'not only... but also...' (không những... mà còn...).",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Liên từ tương quan: Either... or, Neither... nor, Not only... but also.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "neither", false), ("B", "not only", true), ("C", "either", false), ("D", "both", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_188",
                TopicId = 8, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "In a business schedule, what does 'TBD' stand for?",
                Explanation = "'TBD' là viết tắt của 'To Be Determined' (Sẽ được quyết định / thông báo sau).",
                Citation = "Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Viết tắt trong biểu mẫu: TBD (To Be Determined), TBA (To Be Announced).",
                SourceType = "Extracted", SourceFile = "04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx",
                Options = new() { ("A", "To Be Deleted", false), ("B", "To Be Determined", true), ("C", "Total Bank Deposit", false), ("D", "Time Before Departure", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_189",
                TopicId = 10, LevelId = 3, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Complete the sentence: 'Mr. Tanaka has worked here _______ more than fifteen years.'",
                Explanation = "'For' đi với khoảng thời gian kéo dài (for more than fifteen years). 'Since' chỉ đi với mốc thời gian bắt đầu.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Phân biệt For (khoảng thời gian) và Since (mốc thời gian).",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "for", true), ("B", "since", false), ("C", "during", false), ("D", "in", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_190",
                TopicId = 8, LevelId = 3, QuestionType = "NoiCap", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Match TOEIC business terms with their equivalents:\n1. 'Quarterly'\n2. 'Annual'\n3. 'Biannual'",
                Explanation = "1 là 3 tháng 1 lần (hàng quý); 2 là 1 năm 1 lần (hàng năm); 3 là 2 lần 1 năm (nửa năm 1 lần).",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Thuật ngữ chu kỳ tài chính và báo cáo doanh nghiệp.",
                SourceType = "Applied", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "1-Every three months, 2-Once a year, 3-Twice a year", true), ("B", "1-Every week, 2-Every month, 3-Every day", false), ("C", "1-Once in ten years, 2-Twice a month, 3-Every hour", false), ("D", "1-Daily, 2-Weekly, 3-Hourly", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_191",
                TopicId = 13, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What is a 'Press Release' issued by a corporation?",
                Explanation = "'Thông cáo báo chí' (Press Release) là bản tuyên bố chính thức gửi cho giới truyền thông để công bố tin tức mới về sản phẩm hoặc sự kiện công ty.",
                Citation = "Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Thể loại văn bản Press Release trong Part 7.",
                SourceType = "Extracted", SourceFile = "04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx",
                Options = new() { ("A", "An internal memo reprimanding a worker", false), ("B", "An official public statement sent to media outlets providing information or making an announcement", true), ("C", "A personal handwritten diary", false), ("D", "A factory operating handbook", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_192",
                TopicId = 10, LevelId = 3, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct preposition: 'The workshop will be held _______ Tuesday morning, October 24.'",
                Explanation = "Đi với các buổi trong một ngày cụ thể (Tuesday morning) phải dùng giới từ 'on'.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Giới từ chỉ thời gian: In / On / At.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "in", false), ("B", "on", true), ("C", "at", false), ("D", "by", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_193",
                TopicId = 13, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'The new smartphone model features a battery life that is _______ to its predecessor.'",
                Explanation = "Cấu trúc so sánh: 'superior to' (vượt trội hơn hẳn so với cái gì).",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Tính từ so sánh đi với giới từ 'to': Superior to, Inferior to, Prior to.",
                SourceType = "Extracted", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "superior", true), ("B", "greater", false), ("C", "higher", false), ("D", "richer", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_194",
                TopicId = 10, LevelId = 3, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the mistake: 'Neither the CEO nor the board members was satisfied with the outcome.'",
                Explanation = "Quy tắc hòa hợp chủ vị với 'Neither... nor': Động từ chia theo chủ ngữ gần nhất. 'board members' là số nhiều, do đó phải chia 'were satisfied', không dùng 'was'.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Quy tắc hòa hợp chủ ngữ số ít/số nhiều với Neither... nor.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "'Neither' should be 'Either'", false), ("B", "'was' should be 'were'", true), ("C", "'with' should be 'about'", false), ("D", "'outcome' should be 'outcomes'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_195",
                TopicId = 8, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "De",
                QuestionText = "What is the purpose of an 'Itinerary' sent to a corporate traveler?",
                Explanation = "'Itinerary' là lịch trình chi tiết chuyến đi bao gồm số hiệu chuyến bay, giờ hạ cánh, khách sạn và các cuộc hẹn công tác.",
                Citation = "Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Lịch trình công tác (Travel Itinerary).",
                SourceType = "Extracted", SourceFile = "04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx",
                Options = new() { ("A", "A medical prescription", false), ("B", "A detailed travel plan outlining flight times, hotels, and scheduled activities", true), ("C", "A bank loan approval letter", false), ("D", "A tax return form", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_196",
                TopicId = 10, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Rewrite using 'adhere to': 'All factory staff must follow safety protocols strictly.'",
                Explanation = "'All factory staff must adhere strictly to safety protocols.' là dạng câu tương đương chuyên nghiệp trong văn bản quy định.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Cụm từ đồng nghĩa: Follow strictly = Adhere strictly to.",
                SourceType = "Applied", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "All factory staff must adhere strictly to safety protocols.", true), ("B", "All staff adhere of safety strictly.", false), ("C", "Staff must adhere safety with factories.", false), ("D", "Safety must adhere the staff.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_197",
                TopicId = 13, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Which noun denotes a person who uses goods or services produced in an economy?",
                Explanation = "'Consumer' (Người tiêu dùng).",
                Citation = "Tài liệu 03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf - Từ vựng thị trường: Consumer, Producer, Wholesaler, Retailer.",
                SourceType = "Extracted", SourceFile = "03_De_Thi_Thu_TOEIC_Mini_Test_50_Cau.pdf",
                Options = new() { ("A", "Manufacturer", false), ("B", "Consumer", true), ("C", "Distributor", false), ("D", "Competitor", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_198",
                TopicId = 10, LevelId = 3, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Complete the sentence: 'The seminar was so informative _______ every attendee praised the speaker.'",
                Explanation = "Cấu trúc kết quả 'so + adj + that + clause' (quá... đến nỗi mà...).",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Mệnh đề chỉ kết quả với So... that và Such... that.",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "that", true), ("B", "as", false), ("C", "than", false), ("D", "since", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_199",
                TopicId = 8, LevelId = 3, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: In a TOEIC business letter, the abbreviation 'Encl.' at the very bottom indicates that additional documents are enclosed in the envelope.",
                Explanation = "Đúng. 'Encl.' là viết tắt của 'Enclosure' (Tài liệu đính kèm bên trong phong bì thư).",
                Citation = "Tài liệu 04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx - Ký hiệu thư tín: Encl. (Enclosure).",
                SourceType = "Extracted", SourceFile = "04_Chien_luoc_TOEIC_Part7_Doan_van_doi_Double_Passages.docx",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_200",
                TopicId = 10, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Select the sentence with correct participle clause reduction:",
                Explanation = "'Having reviewed the quarterly data, the CFO drafted the budget recommendations.' rút gọn mệnh đề cùng chủ ngữ ở dạng phân từ hoàn thành (Having + V3) diễn tả hành động xảy ra trước.",
                Citation = "Tài liệu 02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf - Rút gọn mệnh đề trạng ngữ bằng phân từ (Participle Clauses).",
                SourceType = "Extracted", SourceFile = "02_TOEIC_Reading_Part5_Ngu_phap_va_Tu_loai.pdf",
                Options = new() { ("A", "Reviewing the data, the budget was drafted by the computer.", false), ("B", "Having reviewed the quarterly data, the CFO drafted the budget recommendations.", true), ("C", "Reviewed the data, drafting the budget happened.", false), ("D", "Having been reviewing data, CFO drafting.", false) }
            });

            return list;
        }
    }
}

