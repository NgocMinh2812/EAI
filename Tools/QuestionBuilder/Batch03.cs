using System.Collections.Generic;

namespace QuestionBuilder
{
    public static class Batch03
    {
        public static List<QuestionItem> GetQuestions()
        {
            var list = new List<QuestionItem>();

            // --- BÀI HỌC 21: Quy chuẩn Viết Email Thương mại (B1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_101",
                TopicId = 14, LevelId = 3, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "When writing an email to announce a product delay to business clients, what is the best opening sentence?",
                Explanation = "'We are writing to inform you that...' là mẫu câu mở đầu chuẩn mực, chuyên nghiệp và lịch thiệp trong thư tín thương mại.",
                Citation = "Bài học 21 - Phần 1: Mẫu câu mở đầu email thương mại: 'I am writing to inform / confirm / request...'",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "I am writing to inform you of an unexpected delay regarding your recent order.", true), ("B", "Hey clients, the products are late, deal with it.", false), ("C", "Products are slow because of reasons.", false), ("D", "I tell you today that nothing is delivered.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_102",
                TopicId = 14, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What does the abbreviation 'FYI' mean in corporate email communication?",
                Explanation = "'FYI' là viết tắt của 'For Your Information' (Để bạn nắm thông tin, không yêu cầu hành động bắt buộc).",
                Citation = "Bài học 21 - Viết tắt thông dụng trong văn phòng: FYI (For Your Information), ASAP (As Soon As Possible), EOD (End of Day).",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "For Your Inspection", false), ("B", "For Your Information", true), ("C", "Follow Your Instincts", false), ("D", "Find Your Invoice", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_103",
                TopicId = 14, LevelId = 3, QuestionType = "DienKhuyet", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Complete the professional email closing: 'Please do not _______ to contact me if you require any further assistance.'",
                Explanation = "Thành ngữ thư tín kinh điển: 'do not hesitate to contact me' (đừng ngần ngại liên hệ với tôi).",
                Citation = "Bài học 21 - Lời kết thúc email chuyên nghiệp: 'Please do not hesitate to contact me...'",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "hesitate", true), ("B", "delay", false), ("C", "refuse", false), ("D", "doubt", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_104",
                TopicId = 14, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Which phrase is used to introduce an attached file in a formal email?",
                Explanation = "'Please find attached the quarterly financial report' là cấu trúc quy chuẩn báo tệp đính kèm.",
                Citation = "Bài học 21 - Đính kèm tài liệu: 'Please find attached / I have attached...'",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "Look down for files.", false), ("B", "Please find attached the quarterly financial report.", true), ("C", "Files are glued below.", false), ("D", "Here are random documents.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_105",
                TopicId = 14, LevelId = 3, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the mistake: 'I look forward to hear from you soon regarding this proposal.'",
                Explanation = "Cấu trúc 'look forward to' đi kèm với V-ing hoặc danh từ: 'look forward to hearing from you'.",
                Citation = "Bài học 21 - Cấu trúc câu kết thư: Look forward to + V-ing.",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "'look' should be 'looking'", false), ("B", "'hear' should be 'hearing'", true), ("C", "'from' should be 'to'", false), ("D", "'proposal' should be 'proposed'", false) }
            });

            // --- BÀI HỌC 22: Email Đặt lịch hẹn, Xác nhận và Xử lý Khiếu nại (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_106",
                TopicId = 14, LevelId = 4, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "When proposing a rescheduled meeting time to a client, which expression is most professional?",
                Explanation = "'Would 10:00 AM this Thursday be convenient for you?' là cách hỏi lịch sự và chuyên nghiệp nhất.",
                Citation = "Bài học 22 - Đặt lịch hẹn và dời lịch: 'Would [time] suit you? / Would [time] be convenient?'",
                SourceType = "Extracted", SourceFile = "BaiHoc_22_AppointmentAndComplaints.txt", LessonId = 22,
                Options = new() { ("A", "Come to my office at 10 AM Thursday or else.", false), ("B", "Would 10:00 AM this Thursday be convenient for you?", true), ("C", "Thursday 10 AM is the only time I have.", false), ("D", "I force our meeting to be on Thursday.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_107",
                TopicId = 14, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What is the primary objective of the opening paragraph in a customer complaint response email?",
                Explanation = "Khi trả lời khiếu nại của khách hàng, việc đầu tiên là ghi nhận vấn đề, đồng cảm và gửi lời xin lỗi chân thành vì sự bất tiện.",
                Citation = "Bài học 22 - Khung giải quyết khiếu nại (Acknowledge & Apologize): Thừa nhận sự cố và xin lỗi vì trải nghiệm bất tiện.",
                SourceType = "Extracted", SourceFile = "BaiHoc_22_AppointmentAndComplaints.txt", LessonId = 22,
                Options = new() { ("A", "To blame the third-party logistics company.", false), ("B", "To acknowledge the issue, apologize sincerely, and validate the customer's frustration.", true), ("C", "To demand that the customer pay extra fees.", false), ("D", "To ignore the complaint entirely.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_108",
                TopicId = 14, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Complete the complaint resolution phrase: 'To _______ for the inconvenience caused, we would like to offer you a 25% discount voucher.'",
                Explanation = "'Compensate for' có nghĩa là bồi thường hoặc bù đắp cho sự bất tiện.",
                Citation = "Bài học 22 - Đưa ra giải pháp bồi thường: 'To compensate for the inconvenience...'",
                SourceType = "Extracted", SourceFile = "BaiHoc_22_AppointmentAndComplaints.txt", LessonId = 22,
                Options = new() { ("A", "compensate", true), ("B", "punish", false), ("C", "charge", false), ("D", "increase", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_109",
                TopicId = 14, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Rewrite more diplomatically: 'You sent the wrong invoice.'",
                Explanation = "Cách diễn đạt khéo léo trong kinh doanh: 'There seems to be a discrepancy in the invoice received.' (Có vẻ như có sự sai lệch nhỏ trong hóa đơn).",
                Citation = "Bài học 22 - Ngoại giao thương mại (Diplomatic language in emails).",
                SourceType = "Applied", SourceFile = "BaiHoc_22_AppointmentAndComplaints.txt", LessonId = 22,
                Options = new() { ("A", "There appears to be a slight discrepancy in the invoice we received.", true), ("B", "Your accounting team made a terrible mistake.", false), ("C", "I demand you learn how to make invoices.", false), ("D", "This invoice is totally fake.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_110",
                TopicId = 14, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: In business emails, using ALL CAPITAL LETTERS in a sentence is interpreted as aggressive and unprofessional (shouting).",
                Explanation = "Đúng. Viết hoa toàn bộ chữ cái (ALL CAPS) trong thư điện tử được coi như đang la hét, thô lỗ.",
                Citation = "Bài học 22 - Văn hóa email doanh nghiệp (Netiquette): Tránh viết hoa toàn bộ từ ngữ.",
                SourceType = "Extracted", SourceFile = "BaiHoc_22_AppointmentAndComplaints.txt", LessonId = 22,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 23: Chiến lược Đàm phán Hợp đồng, Thỏa thuận Giá cả (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_111",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "In contract negotiations, what does the term 'concession' mean?",
                Explanation = "'Concession' là sự nhượng bộ (chấp nhận từ bỏ hoặc nới lỏng một yêu cầu để đạt được thỏa thuận chung).",
                Citation = "Bài học 23 - Thuật ngữ đàm phán hợp đồng: Concession (sự nhượng bộ lẫn nhau để đạt Win-Win).",
                SourceType = "Extracted", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "A complete cancellation of business ties.", false), ("B", "Something granted or yielded, especially in response to demands during negotiations.", true), ("C", "A penalty fine for violating delivery terms.", false), ("D", "An upfront payment before delivery.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_112",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Which conditional structure expresses a flexible negotiation stance: 'If you _______ the order volume to 1,000 units, we _______ offer a 10% discount.'",
                Explanation = "Đàm phán thương mại thường dùng câu điều kiện loại 1: 'If you increase... we will / can offer...'.",
                Citation = "Bài học 23 - Mẫu câu điều kiện trao đổi quyền lợi: If you [concession], we will [benefit].",
                SourceType = "Extracted", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "increased / would have offered", false), ("B", "increase / can offer", true), ("C", "had increased / will offer", false), ("D", "increasing / offered", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_113",
                TopicId = 15, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank with an international trade term: 'The parties agreed on FOB (Free _______ Board) shipping terms.'",
                Explanation = "FOB là Incoterm viết tắt của 'Free On Board' (Giao hàng lên tàu).",
                Citation = "Bài học 23 - Điều khoản thương mại quốc tế Incoterms: FOB (Free On Board), CIF (Cost, Insurance and Freight).",
                SourceType = "Extracted", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "On", true), ("B", "In", false), ("C", "At", false), ("D", "By", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_114",
                TopicId = 15, LevelId = 4, QuestionType = "NoiCap", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Match each negotiation term with its definition:\n1. 'Deadlock / Impasse'\n2. 'Win-Win outcome'\n3. 'Warranty period'",
                Explanation = "1 là bế tắc đàm phán; 2 là kết quả hai bên cùng có lợi; 3 là thời hạn bảo hành sản phẩm.",
                Citation = "Bài học 23 - Bảng thuật ngữ chuyên sâu về hợp đồng thương mại.",
                SourceType = "Extracted", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "1-A state where no progress is possible, 2-Both parties benefit mutually, 3-Duration of product guarantee", true), ("B", "1-Payment, 2-Loss, 3-Delivery", false), ("C", "1-Meeting, 2-Contract, 3-Signature", false), ("D", "1-Profit, 2-Debt, 3-Tax", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_115",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "What does a 'force majeure' clause in a commercial contract protect companies against?",
                Explanation = "'Force majeure' (Bất khả kháng) bảo vệ các bên khỏi trách nhiệm pháp lý khi xảy ra thiên tai, chiến tranh hoặc thảm họa ngoài tầm kiểm soát.",
                Citation = "Bài học 23 - Điều khoản bất khả kháng (Force Majeure clause).",
                SourceType = "Extracted", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "Minor employee tardiness", false), ("B", "Unforeseeable extraordinary events like natural disasters or war that prevent fulfillment of obligations", true), ("C", "Routine fluctuations in stock prices", false), ("D", "Changes in executive management", false) }
            });

            // --- BÀI HỌC 24: Kỹ năng Thuyết trình Dự án (Pitching) trước Hội đồng Đầu tư (C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_116",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "In a startup pitch to venture capitalists, what does the 'Value Proposition' articulate?",
                Explanation = "'Value Proposition' (Đề xuất giá trị) nêu rõ lợi ích cốt lõi độc đáo mà giải pháp của startup mang lại để giải quyết nỗi đau của khách hàng.",
                Citation = "Bài học 24 - Cấu trúc bài thuyết trình Pitching: Problem, Solution, Value Proposition, Market Size, Traction.",
                SourceType = "Extracted", SourceFile = "BaiHoc_24_ProjectPitching.txt", LessonId = 24,
                Options = new() { ("A", "The list of founder office amenities", false), ("B", "The unique, measurable value and benefits a solution delivers to solve customer pain points", true), ("C", "The physical weight of the product packaging", false), ("D", "The personal hobbies of the coding team", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_117",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "What startup metric does 'TAM' stand for during an investment pitch?",
                Explanation = "'TAM' là viết tắt của 'Total Addressable Market' (Tổng dung lượng thị trường tiềm năng).",
                Citation = "Bài học 24 - Thuật ngữ quy mô thị trường: TAM (Total Addressable Market), SAM, SOM.",
                SourceType = "Extracted", SourceFile = "BaiHoc_24_ProjectPitching.txt", LessonId = 24,
                Options = new() { ("A", "Total Addressable Market", true), ("B", "Targeted Annual Margin", false), ("C", "Technology Assessment Model", false), ("D", "Technical Asset Management", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_118",
                TopicId = 15, LevelId = 5, QuestionType = "DienKhuyet", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "Complete the transition phrase in an investment pitch: 'Now that we have covered the market size, let's _______ our attention to our traction and revenue growth.'",
                Explanation = "'Turn our attention to' là cụm chuyển ý chuyên nghiệp kinh điển khi thuyết trình.",
                Citation = "Bài học 24 - Kỹ năng kết nối chuyển ý (Signposting language): 'Let us turn our attention to...'",
                SourceType = "Extracted", SourceFile = "BaiHoc_24_ProjectPitching.txt", LessonId = 24,
                Options = new() { ("A", "turn", true), ("B", "throw", false), ("C", "kick", false), ("D", "jump", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_119",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What is the function of the 'Hook' in the opening 30 seconds of an executive pitch?",
                Explanation = "Cú 'Hook' mở đầu có vai trò thu hút sự chú ý tức thì của các nhà đầu tư thông qua số liệu ấn tượng hoặc câu hỏi gợi mở.",
                Citation = "Bài học 24 - Nghệ thuật Hook khán giả trong 30 giây đầu.",
                SourceType = "Extracted", SourceFile = "BaiHoc_24_ProjectPitching.txt", LessonId = 24,
                Options = new() { ("A", "To bore the audience with legal definitions", false), ("B", "To seize audience attention immediately via compelling statistics, stories, or provocative questions", true), ("C", "To distribute handouts silently", false), ("D", "To apologize for technical flaws", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_120",
                TopicId = 15, LevelId = 5, QuestionType = "DungSai", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "True or False: During a pitch, founders should read word-for-word from text-heavy presentation slides.",
                Explanation = "Sai. Người thuyết trình tuyệt đối không đọc nguyên văn từng chữ trên slide; slide chỉ nên chứa từ khóa, hình ảnh và số liệu trực quan.",
                Citation = "Bài học 24 - Quy tắc vàng thiết kế Slide thuyết trình: 'Slides are visual aids, not teleprompters.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_24_ProjectPitching.txt", LessonId = 24,
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            // --- TÀI LIỆU TEST UPLOAD: 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx (B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_121",
                TopicId = 13, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "In the salary negotiation guide, when is the recommended timing to discuss exact compensation numbers with a recruiter?",
                Explanation = "Thời điểm tối ưu để đàm phán lương là sau khi nhận được thư mời nhận việc chính thức (Official Job Offer), khi nhà tuyển dụng đã chọn bạn.",
                Citation = "Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Thời điểm vàng đàm phán lương: Sau khi nhận Official Job Offer.",
                SourceType = "Extracted", SourceFile = "03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx",
                Options = new() { ("A", "In the first 2 minutes of the initial phone screening", false), ("B", "After receiving an official job offer, when your leverage is highest", true), ("C", "On the job application form before applying", false), ("D", "After working for six months without asking", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_122",
                TopicId = 13, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What does the term 'Total Compensation Package' include besides base salary?",
                Explanation = "'Total Compensation' bao gồm lương cứng cùng các phúc lợi bổ sung như thưởng hiệu suất, bảo hiểm y tế cao cấp, ngày nghỉ phép và cổ phiếu (equity).",
                Citation = "Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Cấu phần gói đãi ngộ: Base salary, performance bonus, stock options, health insurance.",
                SourceType = "Extracted", SourceFile = "03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx",
                Options = new() { ("A", "Only cash in hand", false), ("B", "Base salary plus bonuses, health insurance, paid leave, equity, and retirement benefits", true), ("C", "Office furniture allowance only", false), ("D", "Free parking tickets exclusively", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_123",
                TopicId = 13, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Rewrite the blunt salary request politely: 'Give me $3,000 or I quit.'",
                Explanation = "Mẫu câu đàm phán dựa trên giá trị thị trường: 'Based on my experience and industry benchmarks, I am targeting a compensation range around $3,000.'",
                Citation = "Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Mẫu câu đàm phán khéo léo dựa trên dữ liệu thị trường.",
                SourceType = "Applied", SourceFile = "03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx",
                Options = new() { ("A", "Based on industry benchmarks and my proven track record, I am targeting a range around $3,000.", true), ("B", "Pay me $3,000 because I like money.", false), ("C", "I need $3,000 to buy luxury clothes.", false), ("D", "Why is your salary so cheap? Make it $3,000.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_124",
                TopicId = 13, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'Candidates should research market _______ on Glassdoor or LinkedIn Salary before interview rounds.'",
                Explanation = "'Market benchmarks' hoặc 'rates' có nghĩa là mức chuẩn thị trường.",
                Citation = "Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Bước 1: Khảo sát Market Benchmarks trên Glassdoor/LinkedIn.",
                SourceType = "Extracted", SourceFile = "03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx",
                Options = new() { ("A", "benchmarks", true), ("B", "secrets", false), ("C", "rumors", false), ("D", "jokes", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_125",
                TopicId = 13, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: If an employer cannot meet your desired base salary, you can negotiate other aspects such as flexible working hours, remote days, or annual review cycles.",
                Explanation = "Đúng. Đàm phán đãi ngộ linh hoạt bao gồm cả chế độ làm việc từ xa (remote work), thời gian linh hoạt và hỗ trợ đào tạo.",
                Citation = "Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Phương án thay thế khi ngân sách lương cố định: Hybrid work, review định kỳ.",
                SourceType = "Extracted", SourceFile = "03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- CÁC CÂU HỎI BỔ TRỢ GIAO TIẾP VÀ VĂN BẢN THƯƠNG MẠI (B1 - B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_126",
                TopicId = 14, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What does the abbreviation 'CC' stand for in email software?",
                Explanation = "'CC' là viết tắt của 'Carbon Copy' (Gửi bản sao để các bên liên quan cùng theo dõi).",
                Citation = "Bài học 21 - Các trường gửi email: To, CC (Carbon Copy), BCC (Blind Carbon Copy).",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "Computer Code", false), ("B", "Carbon Copy", true), ("C", "Client Contact", false), ("D", "Critical Case", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_127",
                TopicId = 14, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What is the key privacy advantage of using 'BCC' (Blind Carbon Copy)?",
                Explanation = "'BCC' ẩn danh tính và địa chỉ email của những người nhận trong danh sách, bảo vệ quyền riêng tư.",
                Citation = "Bài học 21 - Chức năng trường BCC: Ẩn danh sách email người nhận.",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "It sends the email twice as fast.", false), ("B", "Recipients in the BCC field cannot see each other's email addresses.", true), ("C", "It encrypts attachments with passwords automatically.", false), ("D", "It deletes the email after reading.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_128",
                TopicId = 14, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Rewrite the opening sentence to be more formal: 'Thanks a lot for the email you sent yesterday.'",
                Explanation = "'Thank you for your email of [date].' là cách diễn đạt trang trọng chuẩn văn phòng.",
                Citation = "Bài học 21 - Mẫu câu cảm ơn trang trọng trong email thương mại.",
                SourceType = "Applied", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "Thank you for your email correspondence dated yesterday regarding our partnership.", true), ("B", "I loved the mail you dropped yesterday.", false), ("C", "Nice mail yesterday, bro.", false), ("D", "Your mail was cool yesterday.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_129",
                TopicId = 14, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "What is an 'Out of Office' (OOO) auto-responder intended to inform senders about?",
                Explanation = "Thư tự động báo vắng mặt (Out of Office) thông báo thời gian nhân viên đi công tác/nghỉ phép và người phụ trách thay thế.",
                Citation = "Bài học 22 - Soạn thảo Out of Office message chuẩn quốc tế.",
                SourceType = "Extracted", SourceFile = "BaiHoc_22_AppointmentAndComplaints.txt", LessonId = 22,
                Options = new() { ("A", "That the company has permanently closed down.", false), ("B", "That the recipient is away from work, their expected return date, and emergency contact details.", true), ("C", "That the sender should never email again.", false), ("D", "That server disk space is full.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_130",
                TopicId = 15, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'The contract is legally _______ once signed by authorized representatives of both corporations.'",
                Explanation = "Cụm từ pháp lý: 'legally binding' (có hiệu lực ràng buộc về mặt pháp lý).",
                Citation = "Bài học 23 - Thuật ngữ pháp lý hợp đồng: Legally binding contract.",
                SourceType = "Extracted", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "binding", true), ("B", "locking", false), ("C", "gluing", false), ("D", "tying", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_131",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "What is a 'Non-Disclosure Agreement' (NDA) designed to protect?",
                Explanation = "'NDA' (Thỏa thuận bảo mật thông tin) được ký kết để bảo vệ bí mật kinh doanh và thông tin độc quyền giữa các đối tác.",
                Citation = "Bài học 23 - Các loại hợp đồng thương mại: NDA (Non-Disclosure Agreement).",
                SourceType = "Extracted", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "Public marketing slogans", false), ("B", "Confidential business information, intellectual property, and proprietary trade secrets", true), ("C", "Office recycling guidelines", false), ("D", "Cafeteria lunch menus", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_132",
                TopicId = 15, LevelId = 4, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the mistake: 'Unless both parties will agree in writing, no amendments can be made.'",
                Explanation = "Trong mệnh đề chỉ điều kiện bắt đầu bằng 'Unless' (trừ khi), động từ chia ở thì hiện tại đơn, không dùng 'will': 'Unless both parties agree...'.",
                Citation = "Bài học 23 - Quy tắc ngữ pháp mệnh đề điều kiện trong hợp đồng (Unless + Present Simple).",
                SourceType = "Applied", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "'parties' should be 'partys'", false), ("B", "'will agree' should be 'agree'", true), ("C", "'in writing' should be 'on written'", false), ("D", "'can' should be 'could'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_133",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "During Q&A after a project pitch, how should a presenter handle a question they do not immediately have the exact data for?",
                Explanation = "Cách xử lý chuyên nghiệp: Thừa nhận câu hỏi sâu sắc, ghi nhận và cam kết gửi dữ liệu chi tiết sau buổi thuyết trình.",
                Citation = "Bài học 24 - Kỹ năng ứng biến trong phần Q&A: 'That is a pertinent question; let me follow up with the exact figures after this session.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_24_ProjectPitching.txt", LessonId = 24,
                Options = new() { ("A", "Make up random numbers to sound smart.", false), ("B", "Acknowledge the question, express willingness to follow up with exact figures, and move on smoothly.", true), ("C", "Tell the investor they have no right to ask that.", false), ("D", "End the pitch and walk off stage.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_134",
                TopicId = 14, LevelId = 3, QuestionType = "DienKhuyet", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'We look forward to establishing a mutually _______ partnership with your organization.'",
                Explanation = "'mutually beneficial' (đôi bên cùng có lợi) là tính từ ghép chuẩn trong thư đối ngoại.",
                Citation = "Bài học 21 - Mẫu câu thiết lập quan hệ đối tác: Mutually beneficial partnership.",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "beneficial", true), ("B", "benefit", false), ("C", "benefiting", false), ("D", "beneficiary", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_135",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What does the commercial term 'Lead Time' refer to?",
                Explanation = "'Lead time' là khoảng thời gian từ khi bắt đầu đặt đơn hàng cho đến khi hàng được sản xuất và giao tới nơi.",
                Citation = "Bài học 23 - Điều khoản giao hàng: Lead time (thời gian từ đặt hàng đến giao nhận).",
                SourceType = "Extracted", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "The time taken to drink coffee at work", false), ("B", "The total amount of time between order placement and the delivery of finished goods", true), ("C", "The duration of leadership training courses", false), ("D", "The time taken to sign an email signature", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_136",
                TopicId = 14, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "What does 'Subject Line' in a professional email need to achieve?",
                Explanation = "Dòng tiêu đề (Subject line) phải ngắn gọn, súc tích và nêu bật ngay nội dung trọng tâm của email.",
                Citation = "Bài học 21 - Quy tắc vàng viết tiêu đề email (Clear & Concise Subject Lines).",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "Be vague and mysterious to force people to open it.", false), ("B", "Be concise, clear, and accurately summarize the email's core purpose.", true), ("C", "Contain humorous emojis exclusively.", false), ("D", "Be left completely empty.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_137",
                TopicId = 14, LevelId = 3, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: 'Reply All' should be used cautiously to avoid cluttering inboxes of colleagues who do not need the message.",
                Explanation = "Đúng. Cần cân nhắc kỹ trước khi bấm 'Reply All' để tránh làm phiền những người không liên quan.",
                Citation = "Bài học 21 - Quy tắc giao tiếp văn phòng: Sử dụng Reply vs. Reply All đúng người đúng việc.",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_138",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "What does 'Burn Rate' represent in startup finance and executive pitch decks?",
                Explanation = "'Burn Rate' (Tốc độ đốt tiền) là tốc độ mà một công ty khởi nghiệp tiêu hao vốn khả dụng trước khi đạt điểm hòa vốn hoặc có dòng tiền dương.",
                Citation = "Bài học 24 - Chỉ số tài chính Startup: Burn rate and Runway.",
                SourceType = "Extracted", SourceFile = "BaiHoc_24_ProjectPitching.txt", LessonId = 24,
                Options = new() { ("A", "The speed at which physical paper catches fire in the office", false), ("B", "The rate at which a company spends its available capital before achieving positive cash flow", true), ("C", "The electricity consumption of server racks", false), ("D", "The turnover rate of kitchen staff", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_139",
                TopicId = 13, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "In employment contracts, what is a 'Probationary Period'?",
                Explanation = "'Probationary period' (Thời gian thử việc) là giai đoạn đầu làm việc nhằm đánh giá sự phù hợp của nhân viên trước khi ký hợp đồng chính thức.",
                Citation = "Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Thuật ngữ hợp đồng lao động: Probationary period.",
                SourceType = "Extracted", SourceFile = "03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx",
                Options = new() { ("A", "A retirement vacation", false), ("B", "A trial period where employee performance and suitability are formally evaluated", true), ("C", "An annual bonus award celebration", false), ("D", "A permanent contract with zero reviews", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_140",
                TopicId = 14, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Rewrite the sentence into a polite business confirmation: 'I got the file and everything is good.'",
                Explanation = "'I hereby acknowledge safe receipt of the documents, which are in order.' là cách xác nhận chuyên nghiệp.",
                Citation = "Bài học 22 - Xác nhận nhận tài liệu trong văn phòng.",
                SourceType = "Applied", SourceFile = "BaiHoc_22_AppointmentAndComplaints.txt", LessonId = 22,
                Options = new() { ("A", "We hereby acknowledge receipt of the documents, and confirm that all items are in order.", true), ("B", "File is in my hands and cool.", false), ("C", "I have downloaded the thing you sent.", false), ("D", "File confirmed by me.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_141",
                TopicId = 15, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Complete the contract clause: 'This agreement shall remain in _______ for a period of three calendar years.'",
                Explanation = "Thành ngữ pháp lý: 'remain in effect / force' (duy trì hiệu lực thi hành).",
                Citation = "Bài học 23 - Điều khoản thời hạn hợp đồng: Remain in effect for a period of...",
                SourceType = "Extracted", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "effect", true), ("B", "sleep", false), ("C", "pause", false), ("D", "doubt", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_142",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What does 'ROI' stand for in business investment evaluations?",
                Explanation = "'ROI' là viết tắt của 'Return on Investment' (Tỷ suất hoàn vốn đầu tư).",
                Citation = "Bài học 24 - Chỉ số đo lường hiệu quả kinh doanh: ROI (Return on Investment).",
                SourceType = "Extracted", SourceFile = "BaiHoc_24_ProjectPitching.txt", LessonId = 24,
                Options = new() { ("A", "Return on Investment", true), ("B", "Rate of Interest", false), ("C", "Revenue on Invoice", false), ("D", "Real Operating Income", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_143",
                TopicId = 14, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "De",
                QuestionText = "Which salutation is appropriate in an email to a woman when you do not know her marital status?",
                Explanation = "'Ms.' là danh xưng chuẩn mực và an toàn nhất trong công sở khi gửi thư cho phụ nữ (không phân biệt đã kết hôn hay chưa).",
                Citation = "Bài học 21 - Quy chuẩn xưng hô danh xưng: Ms. [Họ] là chuẩn mực trung lập nhất.",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "Mrs.", false), ("B", "Ms.", true), ("C", "Miss", false), ("D", "Madamoiselle", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_144",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "What does the business idiom 'think outside the box' encourage employees to do?",
                Explanation = "'Think outside the box' khuyến khích tư duy đột phá, sáng tạo và tìm kiếm giải pháp phi truyền thống.",
                Citation = "Bài học 24 - Thành ngữ kinh doanh quốc tế: Think outside the box.",
                SourceType = "Extracted", SourceFile = "BaiHoc_24_ProjectPitching.txt", LessonId = 24,
                Options = new() { ("A", "Work outdoors in the courtyard", false), ("B", "Approach problems in an innovative, unconventional, and creative manner", true), ("C", "Throw away shipping containers", false), ("D", "Focus exclusively on old methodologies", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_145",
                TopicId = 14, LevelId = 3, QuestionType = "DungSai", Skill = "Viet", Difficulty = "De",
                QuestionText = "True or False: Using a clear call-to-action (CTA) at the end of an email makes it easier for the recipient to know what next steps are expected.",
                Explanation = "Đúng. Lời kêu gọi hành động rõ ràng (Call to Action) ở cuối thư giúp người nhận biết chính xác việc cần làm tiếp theo.",
                Citation = "Bài học 21 - Cấu trúc Call to Action trong email bán hàng và giao dịch.",
                SourceType = "Extracted", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_146",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "What is a 'Milestone' in a project deliverable schedule?",
                Explanation = "'Milestone' (Cột mốc dự án) là sự kiện đánh dấu sự hoàn thành của một giai đoạn quan trọng trong tiến độ dự án.",
                Citation = "Bài học 23 - Điều khoản tiến độ giao nhận: Project Milestones.",
                SourceType = "Extracted", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "A physical boundary stone on a highway", false), ("B", "A significant checkpoint or stage completion in a project lifecycle", true), ("C", "A daily coffee break tracker", false), ("D", "The maximum file size of an email", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_147",
                TopicId = 13, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'Employees who perform exceptionally well may receive an annual performance _______.'",
                Explanation = "'Performance bonus' (tiền thưởng theo hiệu suất công việc).",
                Citation = "Tài liệu 03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx - Các chế độ đãi ngộ: Performance bonus.",
                SourceType = "Extracted", SourceFile = "03_Dam_phan_luong_va_Quyen_loi_Salary_Negotiation.docx",
                Options = new() { ("A", "bonus", true), ("B", "fine", false), ("C", "debt", false), ("D", "penalty", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_148",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "What is the recommended body language posture during a business negotiation meeting?",
                Explanation = "Ngồi thẳng người, duy trì giao tiếp bằng mắt (eye contact) và cử chỉ mở (open gestures) thể hiện sự tự tin và cởi mở.",
                Citation = "Bài học 23 - Ngôn ngữ cơ thể trong đàm phán thương mại (Body language and eye contact).",
                SourceType = "Extracted", SourceFile = "BaiHoc_23_ContractNegotiation.txt", LessonId = 23,
                Options = new() { ("A", "Slouching and looking at your phone continuously", false), ("B", "Upright posture, respectful eye contact, and attentive listening", true), ("C", "Arms tightly crossed and turning your back", false), ("D", "Staring blankly at the floor", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_149",
                TopicId = 14, LevelId = 4, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the mistake: 'We appreciate if you could sends us the updated quotation by Monday.'",
                Explanation = "Sau trợ động từ khiếm khuyết 'could', động từ chính phải ở dạng nguyên mẫu không chia: 'could send us'.",
                Citation = "Bài học 21 - Trợ động từ khiếm khuyết trong câu yêu cầu lịch sự: Could + V-inf.",
                SourceType = "Applied", SourceFile = "BaiHoc_21_CommercialEmail.txt", LessonId = 21,
                Options = new() { ("A", "'appreciate' should be 'deprecate'", false), ("B", "'sends' should be 'send'", true), ("C", "'updated' should be 'updating'", false), ("D", "'by' should be 'on'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_150",
                TopicId = 15, LevelId = 5, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "Kho",
                QuestionText = "What does the presentation principle 'Signposting' mean?",
                Explanation = "'Signposting' là việc dùng các từ ngữ chỉ dẫn (như 'First of all', 'Turning to', 'In conclusion') để người nghe dễ dàng theo dõi dòng ý tưởng.",
                Citation = "Bài học 24 - Kỹ thuật Signposting trong thuyết trình chuyên nghiệp.",
                SourceType = "Extracted", SourceFile = "BaiHoc_24_ProjectPitching.txt", LessonId = 24,
                Options = new() { ("A", "Hanging physical road signs on the classroom walls", false), ("B", "Using verbal verbal cues and roadmap phrases that guide listeners through the presentation structure", true), ("C", "Drawing arrows on a whiteboard silently", false), ("D", "Asking the audience to sign an attendance sheet", false) }
            });

            return list;
        }
    }
}

