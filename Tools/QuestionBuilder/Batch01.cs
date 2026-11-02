using System.Collections.Generic;

namespace QuestionBuilder
{
    public static class Batch01
    {
        public static List<QuestionItem> GetQuestions()
        {
            var list = new List<QuestionItem>();

            // --- BÀI HỌC 0: Giao tiếp cơ bản: Làm quen và giới thiệu bản thân (A1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_001",
                TopicId = 2, LevelId = 1, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "De",
                QuestionText = "When meeting someone for the first time in an informal context, which greeting is the most natural?",
                Explanation = "Chào hỏi thân mật thường dùng 'Hi, how is it going?' hoặc 'Nice to meet you'. Các cách như 'How do you do?' là quá trang trọng.",
                Citation = "Bài học 0 - Mục 1: Mẫu câu chào hỏi thường ngày (Informal Greetings): 'Hi / Hello, how is it going?'",
                SourceType = "Extracted", SourceFile = "BaiHoc_00_LaoQuen_GioiThieu.txt", LessonId = 0,
                Options = new() { ("A", "How do you do, sir?", false), ("B", "Hi, nice to meet you!", true), ("C", "I hereby greet you.", false), ("D", "Good day to your presence.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_002",
                TopicId = 2, LevelId = 1, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Complete the sentence: 'Let me introduce _______. My name is Alex, and I am from Da Nang.'",
                Explanation = "Cụm từ chuẩn 'introduce myself' mang nghĩa 'Cho phép tôi tự giới thiệu bản thân'.",
                Citation = "Bài học 0 - Mục 2: Tự giới thiệu bản thân: 'Let me introduce myself. My name is...'",
                SourceType = "Extracted", SourceFile = "BaiHoc_00_LaoQuen_GioiThieu.txt", LessonId = 0,
                Options = new() { ("A", "myself", true), ("B", "me", false), ("C", "mine", false), ("D", "I", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_003",
                TopicId = 2, LevelId = 1, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Choose the correct question to ask someone about their job:",
                Explanation = "Câu hỏi nghề nghiệp thông dụng trong tiếng Anh là 'What do you do?' hoặc 'What is your occupation?'.",
                Citation = "Bài học 0 - Mục 3: Hỏi về nghề nghiệp: 'What do you do? / What do you do for a living?'",
                SourceType = "Extracted", SourceFile = "BaiHoc_00_LaoQuen_GioiThieu.txt", LessonId = 0,
                Options = new() { ("A", "What are you doing now?", false), ("B", "What do you do?", true), ("C", "Where do you work at this second?", false), ("D", "Who is your job?", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_004",
                TopicId = 2, LevelId = 1, QuestionType = "DungSai", Skill = "TuVung", Difficulty = "De",
                QuestionText = "True or False: In English, 'Where do you come from?' and 'Where are you from?' have the exact same meaning.",
                Explanation = "Đúng. Cả hai câu đều dùng để hỏi về quê hương hoặc quốc tịch xuất thân của đối phương.",
                Citation = "Bài học 0 - Mục 2: Hỏi xuất xứ quê hương: 'Where are you from? = Where do you come from?'",
                SourceType = "Extracted", SourceFile = "BaiHoc_00_LaoQuen_GioiThieu.txt", LessonId = 0,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_005",
                TopicId = 2, LevelId = 1, QuestionType = "NoiCap", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Match each self-introduction phrase with its correct communicative purpose:\n1. 'I was born and raised in Hanoi.'\n2. 'I am keen on reading tech blogs.'\n3. 'I work as a frontend developer.'",
                Explanation = "1 nói về nguồn gốc/quê quán; 2 nói về sở thích/đam mê; 3 nói về nghề nghiệp chuyên môn.",
                Citation = "Bài học 0 - Mục 4: Mở rộng thông tin cá nhân: Quê hương, Nghề nghiệp và Sở thích cá nhân.",
                SourceType = "Applied", SourceFile = "BaiHoc_00_LaoQuen_GioiThieu.txt", LessonId = 0,
                Options = new() { ("A", "1-Hometown, 2-Hobby, 3-Occupation", true), ("B", "1-Occupation, 2-Hometown, 3-Hobby", false), ("C", "1-Hobby, 2-Occupation, 3-Hometown", false), ("D", "1-Hometown, 2-Occupation, 3-Hobby", false) }
            });

            // --- BÀI HỌC 1: Hội thoại Chào hỏi Trang trọng vs. Thân mật (Formal vs. Informal) (A2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_006",
                TopicId = 2, LevelId = 2, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "De",
                QuestionText = "Which greeting is appropriate when meeting a business partner or client for the first time?",
                Explanation = "'Good morning, pleased to meet you' là lời chào trang trọng (Formal), phù hợp trong môi trường kinh doanh.",
                Citation = "Bài học 1 - Bảng so sánh Formal vs Informal: Trang trọng dùng 'Pleased to meet you / Good morning Mr./Ms...'",
                SourceType = "Extracted", SourceFile = "BaiHoc_01_Formal_Informal.txt", LessonId = 1,
                Options = new() { ("A", "What's up, buddy?", false), ("B", "Good morning, it is a pleasure to meet you.", true), ("C", "Hey there, how ya doing?", false), ("D", "Catch you later!", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_007",
                TopicId = 2, LevelId = 2, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "In a formal business meeting, complete the sentence: 'It is an honor to _______ with your prestigious company.'",
                Explanation = "Cụm 'collaborate with' hoặc 'partner with' mang tính trang trọng lịch sự cao.",
                Citation = "Bài học 1 - Mục 2: Ngữ cảnh giao tiếp đối tác thương mại.",
                SourceType = "Applied", SourceFile = "BaiHoc_01_Formal_Informal.txt", LessonId = 1,
                Options = new() { ("A", "collaborate", true), ("B", "hang out", false), ("C", "mess around", false), ("D", "chill", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_008",
                TopicId = 2, LevelId = 2, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the mistake in this formal email greeting: 'Dear Sir, I am writing to you because I wanna ask about the contract.'",
                Explanation = "'wanna' là dạng viết tắt thông tục (slang/informal), trong văn bản trang trọng phải viết 'would like to' hoặc 'wish to'.",
                Citation = "Bài học 1 - Quy tắc viết email công việc: Tuyệt đối không dùng tiếng lóng (gonna, wanna, gotta) trong thư trang trọng.",
                SourceType = "Extracted", SourceFile = "BaiHoc_01_Formal_Informal.txt", LessonId = 1,
                Options = new() { ("A", "'Dear Sir' should be 'Hey Sir'", false), ("B", "'wanna' should be 'would like to'", true), ("C", "'about' should be 'with'", false), ("D", "'am writing' should be 'write'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_009",
                TopicId = 2, LevelId = 2, QuestionType = "SapXep", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Rearrange the dialogue lines in a formal introduction:\n(1) 'Pleased to meet you too, Dr. Davis. Thank you for inviting me.'\n(2) 'Good afternoon, Mr. Green. Allow me to introduce Dr. Davis, our department head.'\n(3) 'Good afternoon. It is a pleasure to make your acquaintance, Mr. Green.'",
                Explanation = "Thứ tự hợp lý: (2) Người dẫn giới thiệu -> (3) Dr. Davis chào và bày tỏ sự hân hạnh -> (1) Mr. Green đáp lại.",
                Citation = "Bài học 1 - Mục 3: Thứ tự đối thoại giới thiệu bên thứ 3 trong công sở.",
                SourceType = "Extracted", SourceFile = "BaiHoc_01_Formal_Informal.txt", LessonId = 1,
                Options = new() { ("A", "(2) -> (3) -> (1)", true), ("B", "(1) -> (2) -> (3)", false), ("C", "(3) -> (1) -> (2)", false), ("D", "(2) -> (1) -> (3)", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_010",
                TopicId = 2, LevelId = 2, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Which response corresponds to 'How do you do?' in formal British English?",
                Explanation = "'How do you do?' là một lời chào nghi thức trang trọng chứ không phải câu hỏi thăm sức khỏe; câu trả lời chuẩn tắc là 'How do you do?'.",
                Citation = "Bài học 1 - Lưu ý văn hóa: 'How do you do?' được đáp lại bằng chính 'How do you do?'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_01_Formal_Informal.txt", LessonId = 1,
                Options = new() { ("A", "I am fine, thank you.", false), ("B", "How do you do?", true), ("C", "Yes, I do.", false), ("D", "Not much, you?", false) }
            });

            // --- BÀI HỌC 2: Giới thiệu nghề nghiệp, quê hương và đam mê (B1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_011",
                TopicId = 2, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Choose the word that best fits: 'As a project manager, I am _______ for coordinating cross-functional engineering teams.'",
                Explanation = "Cấu trúc 'responsible for + V-ing' mang nghĩa 'chịu trách nhiệm về việc gì'.",
                Citation = "Bài học 2 - Phần 2: Cấu trúc miêu tả trách nhiệm công việc: 'be responsible for / in charge of'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_02_ElevatorPitch.txt", LessonId = 2,
                Options = new() { ("A", "responsible", true), ("B", "charge", false), ("C", "duty", false), ("D", "capable", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_012",
                TopicId = 2, LevelId = 3, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Rewrite the sentence using 'specialize in': 'My primary field of work is mobile app development.'",
                Explanation = "'I specialize in mobile app development.' là cấu trúc chuẩn để giới thiệu chuyên môn sâu trong Elevator Pitch.",
                Citation = "Bài học 2 - Elevator Pitch: Dùng động từ 'specialize in' để nêu bật thế mạnh chuyên môn.",
                SourceType = "Applied", SourceFile = "BaiHoc_02_ElevatorPitch.txt", LessonId = 2,
                Options = new() { ("A", "I specialize in mobile app development.", true), ("B", "I am specialize of mobile app development.", false), ("C", "I specialized on development of mobile app.", false), ("D", "My special is mobile app develop.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_013",
                TopicId = 2, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "In an Elevator Pitch, what is the ideal duration recommended for introducing oneself effectively?",
                Explanation = "Theo bài học, một bài Elevator Pitch hiệu quả nên kéo dài từ 30 đến 60 giây (khoảng 75-150 từ).",
                Citation = "Bài học 2 - Mục 1: Định nghĩa Elevator Pitch: Thời lượng tối ưu 30 - 60 giây.",
                SourceType = "Extracted", SourceFile = "BaiHoc_02_ElevatorPitch.txt", LessonId = 2,
                Options = new() { ("A", "5 to 10 minutes", false), ("B", "30 to 60 seconds", true), ("C", "3 to 5 minutes", false), ("D", "10 to 15 seconds", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_014",
                TopicId = 2, LevelId = 3, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Complete the sentence describing a passion: 'I have a strong _______ for artificial intelligence and its applications in healthcare.'",
                Explanation = "Cụm danh từ 'have a strong passion for sth' nghĩa là 'có niềm đam mê mãnh liệt với cái gì'.",
                Citation = "Bài học 2 - Mục 3: Thể hiện đam mê cá nhân: 'have a passion for / be passionate about'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_02_ElevatorPitch.txt", LessonId = 2,
                Options = new() { ("A", "passion", true), ("B", "passionate", false), ("C", "passionately", false), ("D", "passions", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_015",
                TopicId = 2, LevelId = 3, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: When introducing your hometown to an international audience, you should include a key feature such as culture, tourism, or economic significance.",
                Explanation = "Đúng. Cung cấp một điểm nhấn nổi bật (landscape, heritage, economy) giúp đối phương dễ ghi nhớ quê hương bạn hơn.",
                Citation = "Bài học 2 - Mục 4: Giới thiệu quê hương gắn liền với bản sắc độc đáo.",
                SourceType = "Extracted", SourceFile = "BaiHoc_02_ElevatorPitch.txt", LessonId = 2,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 3: Đặt bàn và gọi món tại nhà hàng quốc tế (A2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_016",
                TopicId = 3, LevelId = 2, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "De",
                QuestionText = "What is the most polite way to ask for a table for two at a restaurant?",
                Explanation = "'We would like a table for two, please' hoặc 'Could we have a table for two, please?' là cách diễn đạt lịch sự chuẩn mực.",
                Citation = "Bài học 3 - Phần 1: Mẫu câu đặt bàn: 'Could I book a table for two? / A table for two, please.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_03_DiningOut.txt", LessonId = 3,
                Options = new() { ("A", "Give us a table for two right now.", false), ("B", "Could we have a table for two, please?", true), ("C", "Two seats are ours.", false), ("D", "I take two chairs here.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_017",
                TopicId = 3, LevelId = 2, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "What do you call the small amount of food served before the main course?",
                Explanation = "'Appetizer' (tiếng Anh Mỹ) hoặc 'Starter' (tiếng Anh Anh) là món khai vị ăn trước món chính.",
                Citation = "Bài học 3 - Từ vựng thực đơn: Starter / Appetizer: Món khai vị; Main course: Món chính; Dessert: Tráng miệng.",
                SourceType = "Extracted", SourceFile = "BaiHoc_03_DiningOut.txt", LessonId = 3,
                Options = new() { ("A", "Dessert", false), ("B", "Main course", false), ("C", "Appetizer / Starter", true), ("D", "Beverage", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_018",
                TopicId = 3, LevelId = 2, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "When asking the server for the total payment, you say: 'Could we have the _______, please?'",
                Explanation = "Trong nhà hàng, 'bill' (Anh-Anh) hoặc 'check' (Anh-Mỹ) là hóa đơn thanh toán tiền ăn.",
                Citation = "Bài học 3 - Phần 3: Yêu cầu thanh toán: 'Could we get the bill/check, please?'",
                SourceType = "Extracted", SourceFile = "BaiHoc_03_DiningOut.txt", LessonId = 3,
                Options = new() { ("A", "bill", true), ("B", "menu", false), ("C", "recipe", false), ("D", "napkin", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_019",
                TopicId = 3, LevelId = 2, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "If a customer cannot eat peanuts due to health risks, how should they inform the waiter?",
                Explanation = "Mẫu câu chuẩn: 'I am allergic to peanuts' (Tôi bị dị ứng với đậu phộng).",
                Citation = "Bài học 3 - Mục Lưu ý về dị ứng thực phẩm: 'I am allergic to [food item]'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_03_DiningOut.txt", LessonId = 3,
                Options = new() { ("A", "I hate peanuts completely.", false), ("B", "I am allergic to peanuts.", true), ("C", "Peanuts are bad for my cooking.", false), ("D", "I refuse peanuts because of habit.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_020",
                TopicId = 3, LevelId = 2, QuestionType = "SapXep", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Order the sequence of dining out steps:\n(1) Ordering the main course and beverages\n(2) Asking for the bill and paying\n(3) Reserving or requesting a table\n(4) Enjoying the dessert",
                Explanation = "Trình tự ăn uống tại nhà hàng: Đặt bàn (3) -> Gọi món & đồ uống (1) -> Thưởng thức tráng miệng (4) -> Thanh toán (2).",
                Citation = "Bài học 3 - Trình tự trải nghiệm dịch vụ nhà hàng quốc tế.",
                SourceType = "Applied", SourceFile = "BaiHoc_03_DiningOut.txt", LessonId = 3,
                Options = new() { ("A", "(3) -> (1) -> (4) -> (2)", true), ("B", "(1) -> (3) -> (2) -> (4)", false), ("C", "(3) -> (4) -> (1) -> (2)", false), ("D", "(2) -> (3) -> (1) -> (4)", false) }
            });

            // --- BÀI HỌC 4: Mua sắm, chọn size và đổi trả hàng hóa (A1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_021",
                TopicId = 3, LevelId = 1, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "De",
                QuestionText = "Where in a clothing store do you go to test if garments fit you?",
                Explanation = "'Fitting room' (hoặc 'dressing room') là phòng thử đồ trong cửa hàng thời trang.",
                Citation = "Bài học 4 - Từ vựng mua sắm: Fitting room: Phòng thử quần áo.",
                SourceType = "Extracted", SourceFile = "BaiHoc_04_ShoppingReturns.txt", LessonId = 4,
                Options = new() { ("A", "Cash register", false), ("B", "Fitting room", true), ("C", "Warehouse", false), ("D", "Showcase", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_022",
                TopicId = 3, LevelId = 1, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Complete the sentence: 'Excuse me, do you have this shirt in a larger _______?'",
                Explanation = "Trong mua sắm quần áo, kích cỡ dùng từ 'size' (size S, M, L, XL).",
                Citation = "Bài học 4 - Mẫu câu hỏi kích cỡ: 'Do you have this in size M / a larger size?'",
                SourceType = "Extracted", SourceFile = "BaiHoc_04_ShoppingReturns.txt", LessonId = 4,
                Options = new() { ("A", "size", true), ("B", "length", false), ("C", "volume", false), ("D", "mass", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_023",
                TopicId = 3, LevelId = 1, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "De",
                QuestionText = "What official document must you show to the store clerk to request a refund or return?",
                Explanation = "'Receipt' là hóa đơn mua lẻ cần xuất trình khi đổi trả sản phẩm.",
                Citation = "Bài học 4 - Chính sách đổi trả: Phải giữ lại 'receipt' (hóa đơn) trong thời hạn bảo hành.",
                SourceType = "Extracted", SourceFile = "BaiHoc_04_ShoppingReturns.txt", LessonId = 4,
                Options = new() { ("A", "Menu", false), ("B", "Receipt", true), ("C", "Ticket", false), ("D", "Leaflet", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_024",
                TopicId = 3, LevelId = 1, QuestionType = "DungSai", Skill = "TuVung", Difficulty = "De",
                QuestionText = "True or False: 'A discount of 20%' means the customer will pay 20% more than the original price.",
                Explanation = "Sai. 'Discount' có nghĩa là giảm giá, khách hàng được giảm 20% so với giá gốc.",
                Citation = "Bài học 4 - Thuật ngữ khuyến mãi: Discount / Sale off: Giảm giá.",
                SourceType = "Extracted", SourceFile = "BaiHoc_04_ShoppingReturns.txt", LessonId = 4,
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_025",
                TopicId = 3, LevelId = 1, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the mistake: 'I would like to return this jacket because it don't fit me.'",
                Explanation = "Chủ ngữ 'it' là ngôi thứ ba số ít, trợ động từ phủ định ở hiện tại đơn phải là 'doesn't', không dùng 'don't'.",
                Citation = "Bài học 4 & Ngữ pháp ngôi thứ 3: 'it doesn't fit'.",
                SourceType = "Applied", SourceFile = "BaiHoc_04_ShoppingReturns.txt", LessonId = 4,
                Options = new() { ("A", "'would like' should be 'wanting'", false), ("B", "'don't' should be 'doesn't'", true), ("C", "'jacket' should be 'jackets'", false), ("D", "'fit' should be 'fitting'", false) }
            });

            // --- BÀI HỌC 5: Ẩm thực đường phố và miêu tả vị giác (B1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_026",
                TopicId = 3, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Which adjective best describes food with a hot burning sensation caused by chili peppers?",
                Explanation = "'Spicy' nghĩa là cay nồng do ớt hoặc tiêu. 'Sweet' là ngọt, 'Sour' là chua, 'Bitter' là đắng.",
                Citation = "Bài học 5 - Từ vựng miêu tả vị giác: Spicy: Cay nồng; Sour: Chua; Savory: Đậm đà thơm ngon.",
                SourceType = "Extracted", SourceFile = "BaiHoc_05_StreetFoodCulture.txt", LessonId = 5,
                Options = new() { ("A", "Savory", false), ("B", "Spicy", true), ("C", "Crispy", false), ("D", "Bitter", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_027",
                TopicId = 3, LevelId = 3, QuestionType = "NoiCap", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Match each sensory adjective with its texture / taste description:\n1. 'Crispy'\n2. 'Savory'\n3. 'Tender'",
                Explanation = "1-Crispy: giòn tan khi nhai; 2-Savory: đậm đà thơm ngon (thịt/gia vị); 3-Tender: mềm mọng dễ cắt (thịt chín vừa).",
                Citation = "Bài học 5 - Bảng tính từ cảm giác (Texture & Taste Descriptors).",
                SourceType = "Extracted", SourceFile = "BaiHoc_05_StreetFoodCulture.txt", LessonId = 5,
                Options = new() { ("A", "1-Crunchy and breaking easily, 2-Pleasant salty/meaty flavor, 3-Soft and easy to cut/chew", true), ("B", "1-Sweet, 2-Soft, 3-Spicy", false), ("C", "1-Liquid, 2-Sour, 3-Hard", false), ("D", "1-Bitter, 2-Sweet, 3-Rough", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_028",
                TopicId = 3, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Why is Vietnamese street food (such as Banh Mi and Pho) celebrated globally according to culinary reviews?",
                Explanation = "Ẩm thực đường phố Việt Nam nổi tiếng nhờ sự cân bằng hoàn hảo giữa rau thơm tươi, nước dùng đậm đà và độ giòn.",
                Citation = "Bài học 5 - Đọc hiểu văn hóa ẩm thực đường phố Việt Nam: 'Harmonious balance of herbs, savory broth, and fresh ingredients.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_05_StreetFoodCulture.txt", LessonId = 5,
                Options = new() { ("A", "Because it uses excessive dairy products.", false), ("B", "Due to its harmonious balance of fresh herbs, savory flavors, and aromatic spices.", true), ("C", "Because it is completely unspiced.", false), ("D", "Because it can only be cooked in industrial ovens.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_029",
                TopicId = 3, LevelId = 3, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Complete the sentence: 'The grilled pork skewers have an irresistible smoky _______ that attracts tourists.'",
                Explanation = "'Aroma' nghĩa là mùi thơm đặc trưng hấp dẫn của thức ăn nướng/nấu chín.",
                Citation = "Bài học 5 - Từ vựng khứu giác: Smoky aroma: Mùi thơm khói nướng hấp dẫn.",
                SourceType = "Extracted", SourceFile = "BaiHoc_05_StreetFoodCulture.txt", LessonId = 5,
                Options = new() { ("A", "aroma", true), ("B", "noise", false), ("C", "texture", false), ("D", "weight", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_030",
                TopicId = 3, LevelId = 3, QuestionType = "DungSai", Skill = "TuVung", Difficulty = "De",
                QuestionText = "True or False: 'Mouth-watering' is an idiom used to describe food that looks or smells so delicious it makes your saliva flow.",
                Explanation = "Đúng. 'Mouth-watering' có nghĩa là ngon chảy nước miếng, kích thích vị giác.",
                Citation = "Bài học 5 - Thành ngữ ẩm thực: 'Mouth-watering food'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_05_StreetFoodCulture.txt", LessonId = 5,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 27: Nerry Describe Family (A1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_031",
                TopicId = 20, LevelId = 1, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "De",
                QuestionText = "In the audio listening text 'Nerry Describe Family', where is Neri originally from?",
                ReadingPassage = "Hello, my name is Neri and I am from Belize. My question today is describe a family member. The family member that I am going to describe is my little sister.",
                Explanation = "Đoạn transcript nói rõ: 'Hello, my name is Neri and I am from Belize.'",
                Citation = "Bài học 27 - Đoạn mở đầu transcript: 'Hello, my name is Neri and I am from Belize.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_27_NerryFamily.txt", LessonId = 27,
                Options = new() { ("A", "Brazil", false), ("B", "Belize", true), ("C", "Belgium", false), ("D", "Bolivia", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_032",
                TopicId = 20, LevelId = 1, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "De",
                QuestionText = "Which family member does Neri choose to describe?",
                ReadingPassage = "The family member that I am going to describe is my little sister. She is sixteen years old and she attends high school.",
                Explanation = "Neri mô tả em gái của mình: 'The family member that I am going to describe is my little sister.'",
                Citation = "Bài học 27 - Câu 2: 'The family member that I am going to describe is my little sister.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_27_NerryFamily.txt", LessonId = 27,
                Options = new() { ("A", "Her grandmother", false), ("B", "Her elder brother", false), ("C", "Her little sister", true), ("D", "Her mother", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_033",
                TopicId = 20, LevelId = 1, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Complete the sentence based on family descriptions: 'My sister _______ sixteen years old.'",
                Explanation = "Động từ 'to be' đi với chủ ngữ ngôi thứ 3 số ít 'My sister' ở hiện tại đơn là 'is'.",
                Citation = "Bài học 27 - Cấu trúc miêu tả tuổi tác: 'She is sixteen years old.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_27_NerryFamily.txt", LessonId = 27,
                Options = new() { ("A", "is", true), ("B", "are", false), ("C", "am", false), ("D", "be", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_034",
                TopicId = 20, LevelId = 1, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "What is the correct English term for your mother's or father's sister?",
                Explanation = "Chị gái hoặc em gái của bố mẹ được gọi là 'aunt' (cô, dì, bác gái).",
                Citation = "Bài học 27 - Mở rộng từ vựng cây gia đình (Family Tree Vocabulary).",
                SourceType = "Applied", SourceFile = "BaiHoc_27_NerryFamily.txt", LessonId = 27,
                Options = new() { ("A", "Cousin", false), ("B", "Aunt", true), ("C", "Niece", false), ("D", "Sibling", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_035",
                TopicId = 20, LevelId = 1, QuestionType = "DungSai", Skill = "TuVung", Difficulty = "De",
                QuestionText = "True or False: The word 'siblings' refers to brothers and sisters.",
                Explanation = "Đúng. 'Siblings' là danh từ chỉ anh chị em ruột trong gia đình.",
                Citation = "Bài học 27 - Từ vựng thành viên gia đình: 'Siblings = brothers and sisters.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_27_NerryFamily.txt", LessonId = 27,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 28: Third Person Singular (Hiện tại đơn ngôi thứ 3 số ít) (A1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_036",
                TopicId = 20, LevelId = 1, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Read the excerpt: 'Woman: What does your mom do? - Man: She _______ women's clothing.' Choose the correct verb form:",
                ReadingPassage = "Woman: What does your mom do?\nMan: She sells women's clothing.\nWoman: Oh, yeah. Where does she work?\nMan: She has a small shop in town.",
                Explanation = "Chủ ngữ 'She' là ngôi thứ 3 số ít, động từ 'sell' chia ở hiện tại đơn thêm 's' thành 'sells'.",
                Citation = "Bài học 28 - Hội thoại 1: 'She sells women's clothing.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_28_ThirdPersonSingular.txt", LessonId = 28,
                Options = new() { ("A", "sell", false), ("B", "sells", true), ("C", "selling", false), ("D", "is sell", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_037",
                TopicId = 20, LevelId = 1, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "In the dialogue: 'Where does she work? - She _______ a small shop in town.'",
                Explanation = "Động từ bất quy tắc 'have' chia ở ngôi thứ 3 số ít thành 'has'.",
                Citation = "Bài học 28 - Hội thoại 1: 'She has a small shop in town.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_28_ThirdPersonSingular.txt", LessonId = 28,
                Options = new() { ("A", "has", true), ("B", "haves", false), ("C", "have", false), ("D", "having", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_038",
                TopicId = 20, LevelId = 1, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Complete the negative sentence: 'My brother _______ not like spicy food.'",
                Explanation = "Trợ động từ phủ định ngôi thứ ba số ít là 'does'. 'does not like' = 'doesn't like'.",
                Citation = "Bài học 28 - Quy tắc trợ động từ phủ định ngôi thứ 3 số ít: does not + V-infinitive.",
                SourceType = "Extracted", SourceFile = "BaiHoc_28_ThirdPersonSingular.txt", LessonId = 28,
                Options = new() { ("A", "does", true), ("B", "do", false), ("C", "is", false), ("D", "are", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_039",
                TopicId = 20, LevelId = 1, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Which verb takes '-es' in the third person singular form?",
                Explanation = "Các động từ tận cùng bằng -ch, -sh, -s, -x, -o thêm -es: watch -> watches, go -> goes.",
                Citation = "Bài học 28 - Quy tắc chính tả đuôi -s / -es ở ngôi thứ 3 số ít.",
                SourceType = "Extracted", SourceFile = "BaiHoc_28_ThirdPersonSingular.txt", LessonId = 28,
                Options = new() { ("A", "play", false), ("B", "watch", true), ("C", "read", false), ("D", "eat", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_040",
                TopicId = 20, LevelId = 1, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the grammatical error: 'Does he lives in an apartment or in a house?'",
                Explanation = "Khi đã dùng trợ động từ 'Does' trong câu hỏi, động từ chính phải ở dạng nguyên mẫu không 's': 'Does he live...'.",
                Citation = "Bài học 28 - Lưu ý câu hỏi: Does + S + V(nguyên mẫu).",
                SourceType = "Applied", SourceFile = "BaiHoc_28_ThirdPersonSingular.txt", LessonId = 28,
                Options = new() { ("A", "'Does' should be 'Do'", false), ("B", "'lives' should be 'live'", true), ("C", "'in' should be 'on'", false), ("D", "'an' should be 'a'", false) }
            });

            // --- CÂU HỎI VẬN DỤNG & TỔNG HỢP GIAO TIẾP HÀNG NGÀY (A1 - A2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_041",
                TopicId = 2, LevelId = 1, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "De",
                QuestionText = "You accidentally bump into someone on the subway. What is the quickest and most polite response?",
                Explanation = "Lời xin lỗi phản xạ nhanh và lịch sự nhất trong giao tiếp đời thường là 'Oh, I'm so sorry!' hoặc 'Excuse me!'.",
                Citation = "Bài học 0 - Phản xạ giao tiếp lịch sự hàng ngày.",
                SourceType = "Applied", SourceFile = "BaiHoc_00_LaoQuen_GioiThieu.txt", LessonId = 0,
                Options = new() { ("A", "Watch out yourself!", false), ("B", "I'm so sorry, excuse me!", true), ("C", "No problem at all.", false), ("D", "You are welcome.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_042",
                TopicId = 3, LevelId = 2, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Read the restaurant sign: 'Reservations recommended for dinner on weekends.' What does it mean?",
                Explanation = "Bảng hiệu thông báo thực khách nên gọi điện đặt bàn trước nếu muốn ăn tối vào các ngày cuối tuần.",
                Citation = "Bài học 3 - Đọc hiểu thông báo nhà hàng: 'Reservations recommended'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_03_DiningOut.txt", LessonId = 3,
                Options = new() { ("A", "Dinner is free on weekends.", false), ("B", "You should book a table in advance for weekend dinner.", true), ("C", "The restaurant is closed on weekends.", false), ("D", "Only two people can dine together.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_043",
                TopicId = 3, LevelId = 1, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "De",
                QuestionText = "Rewrite the sentence politely: 'I want water.'",
                Explanation = "'Could I have some water, please?' hoặc 'I would like some water, please' thể hiện sự lịch sự hơn hẳn 'I want'.",
                Citation = "Bài học 3 - Văn hóa giao tiếp lịch sự: 'Could I have... please?' thay cho 'I want'.",
                SourceType = "Applied", SourceFile = "BaiHoc_03_DiningOut.txt", LessonId = 3,
                Options = new() { ("A", "Could I have some water, please?", true), ("B", "Water is what I want.", false), ("C", "Give me water now.", false), ("D", "I need water fast.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_044",
                TopicId = 3, LevelId = 2, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "When paying for meals with friends, what idiom means everyone pays for their own share?",
                Explanation = "'Go Dutch' là thành ngữ phổ biến nghĩa là 'tiền ai người nấy trả' (chia đôi hoặc chia theo phần ăn).",
                Citation = "Bài học 3 - Thành ngữ thanh toán: 'Go Dutch / Split the bill'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_03_DiningOut.txt", LessonId = 3,
                Options = new() { ("A", "Go French", false), ("B", "Go Dutch", true), ("C", "Pay double", false), ("D", "Leave the money", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_045",
                TopicId = 2, LevelId = 2, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "De",
                QuestionText = "Which closing sign-off is best for a formal letter to someone whose name you know (e.g., Dear Mr. Miller)?",
                Explanation = "Quy tắc chuẩn văn thư Anh Quốc: Nếu thư bắt đầu bằng 'Dear [Tên]', kết thư dùng 'Yours sincerely'. Nếu bắt đầu 'Dear Sir/Madam', kết thư dùng 'Yours faithfully'.",
                Citation = "Bài học 1 - Quy tắc viết thư trang trọng: Dear Mr. X -> Yours sincerely.",
                SourceType = "Extracted", SourceFile = "BaiHoc_01_Formal_Informal.txt", LessonId = 1,
                Options = new() { ("A", "Cheers,", false), ("B", "Yours sincerely,", true), ("C", "See ya,", false), ("D", "Later,", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_046",
                TopicId = 20, LevelId = 1, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Choose the grammatically correct sentence about daily routines:",
                Explanation = "'My father drinks coffee every morning.' tuân thủ đúng quy tắc chia động từ ngôi thứ 3 số ít 'drinks'.",
                Citation = "Bài học 28 - Thói quen hàng ngày ở thì hiện tại đơn.",
                SourceType = "Applied", SourceFile = "BaiHoc_28_ThirdPersonSingular.txt", LessonId = 28,
                Options = new() { ("A", "My father drink coffee every morning.", false), ("B", "My father drinks coffee every morning.", true), ("C", "My father drinking coffee every morning.", false), ("D", "My father is drink coffee every morning.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_047",
                TopicId = 3, LevelId = 1, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Fill in the blank: 'Can I pay _______ credit card or cash?'",
                Explanation = "Giới từ đi với phương thức thanh toán thẻ tín dụng là 'by': 'pay by credit card'.",
                Citation = "Bài học 4 - Phương thức thanh toán: Pay by card / in cash.",
                SourceType = "Extracted", SourceFile = "BaiHoc_04_ShoppingReturns.txt", LessonId = 4,
                Options = new() { ("A", "by", true), ("B", "on", false), ("C", "with", false), ("D", "at", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_048",
                TopicId = 20, LevelId = 1, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "How do you refer to your husband's or wife's mother in English?",
                Explanation = "Mẹ của vợ hoặc chồng là 'mother-in-law' (mẹ vợ / mẹ chồng).",
                Citation = "Bài học 27 - Mối quan hệ họ hàng qua hôn nhân (-in-law).",
                SourceType = "Applied", SourceFile = "BaiHoc_27_NerryFamily.txt", LessonId = 27,
                Options = new() { ("A", "Step-mother", false), ("B", "Mother-in-law", true), ("C", "Godmother", false), ("D", "Foster mother", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_049",
                TopicId = 3, LevelId = 2, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "De",
                QuestionText = "When a sales assistant asks 'May I help you?', what is the standard polite reply if you just want to browse?",
                Explanation = "Mẫu câu văn hóa khi chỉ muốn xem đồ: 'No thank you, I'm just browsing / looking around.'",
                Citation = "Bài học 4 - Phản hồi nhân viên bán hàng: 'I'm just browsing, thank you.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_04_ShoppingReturns.txt", LessonId = 4,
                Options = new() { ("A", "Don't bother me.", false), ("B", "I'm just browsing, thank you.", true), ("C", "Go away please.", false), ("D", "I don't need your voice.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_050",
                TopicId = 2, LevelId = 1, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "De",
                QuestionText = "Which response is appropriate when someone says 'Thank you very much for your help!'?",
                Explanation = "Đáp lại lời cảm ơn chuẩn mực là 'You're welcome!' hoặc 'My pleasure!'.",
                Citation = "Bài học 0 - Phản hồi lời cảm ơn: 'You're welcome / Not at all / My pleasure.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_00_LaoQuen_GioiThieu.txt", LessonId = 0,
                Options = new() { ("A", "You're welcome!", true), ("B", "Yes, of course I did.", false), ("C", "Nothing special for you.", false), ("D", "Don't say thank you.", false) }
            });

            return list;
        }
    }
}

