using System.Collections.Generic;

namespace QuestionBuilder
{
    public static class Batch02
    {
        public static List<QuestionItem> GetQuestions()
        {
            var list = new List<QuestionItem>();

            // --- BÀI HỌC 6: Tiếng Anh Sân bay: Check-in, Cân hành lý và Cửa an ninh (A2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_051",
                TopicId = 4, LevelId = 2, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "De",
                QuestionText = "At airport check-in, what document must all international passengers present alongside their passport?",
                Explanation = "Hành khách bay quốc tế phải xuất trình hộ chiếu (passport) và thẻ lên máy bay / vé máy bay (boarding pass / flight ticket).",
                Citation = "Bài học 6 - Thủ tục Check-in sân bay: Xuất trình Passport và Boarding Pass.",
                SourceType = "Extracted", SourceFile = "BaiHoc_06_AirportNavigation.txt", LessonId = 6,
                Options = new() { ("A", "Gym membership card", false), ("B", "Boarding pass", true), ("C", "Student discount card", false), ("D", "Store receipt", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_052",
                TopicId = 4, LevelId = 2, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "What is the difference between 'carry-on luggage' and 'checked luggage'?",
                Explanation = "'Carry-on' (hành lý xách tay) được mang lên khoang máy bay, trong khi 'checked luggage' (hành lý ký gửi) được gửi vào khoang hàng hóa.",
                Citation = "Bài học 6 - Phân biệt thuật ngữ: Carry-on baggage (xách tay) vs. Checked baggage (ký gửi).",
                SourceType = "Extracted", SourceFile = "BaiHoc_06_AirportNavigation.txt", LessonId = 6,
                Options = new() { ("A", "Carry-on luggage goes into the aircraft cabin with you, while checked luggage goes into the cargo hold.", true), ("B", "Checked luggage is carried by hand onto the plane.", false), ("C", "Carry-on luggage has no weight limits at all.", false), ("D", "Both mean exactly luggage that is lost by airlines.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_053",
                TopicId = 4, LevelId = 2, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Fill in the blank: 'Passengers are requested to proceed to _______ number 14 for boarding.'",
                Explanation = "Cổng ra máy bay tại sân bay được gọi là 'gate' (cổng lên máy bay).",
                Citation = "Bài học 6 - Thông báo phát thanh sân bay: 'Proceed to Gate [number]'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_06_AirportNavigation.txt", LessonId = 6,
                Options = new() { ("A", "gate", true), ("B", "fence", false), ("C", "port", false), ("D", "dock", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_054",
                TopicId = 4, LevelId = 2, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "When going through airport security screening, what items must passengers take out of their bags?",
                Explanation = "Quy định an ninh hàng không quốc tế: Phải lấy máy tính xách tay (laptops) và chất lỏng dưới 100ml ra khay riêng.",
                Citation = "Bài học 6 - Quy trình qua cửa kiểm tra an ninh (Security Checkpoint protocol).",
                SourceType = "Extracted", SourceFile = "BaiHoc_06_AirportNavigation.txt", LessonId = 6,
                Options = new() { ("A", "Laptops and liquids in clear plastic containers", true), ("B", "All clothes and jackets inside the suitcase", false), ("C", "Books and notebooks only", false), ("D", "Empty water bottles", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_055",
                TopicId = 4, LevelId = 2, QuestionType = "DungSai", Skill = "TuVung", Difficulty = "De",
                QuestionText = "True or False: If a flight is marked as 'Delayed' on the departures board, it means the plane has been completely cancelled.",
                Explanation = "Sai. 'Delayed' nghĩa là bị hoãn / trễ giờ, còn 'Cancelled' mới là bị hủy chuyến bay.",
                Citation = "Bài học 6 - Bảng thông tin chuyến bay: On time, Delayed, Boarding, Cancelled.",
                SourceType = "Extracted", SourceFile = "BaiHoc_06_AirportNavigation.txt", LessonId = 6,
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            // --- BÀI HỌC 7: Chỉ dẫn đường phố và sử dụng Tàu điện ngầm (A1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_056",
                TopicId = 4, LevelId = 1, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "De",
                QuestionText = "What is the most polite question to ask a stranger for directions on the street?",
                Explanation = "'Excuse me, could you tell me how to get to the nearest metro station?' là lời mở đầu hỏi đường lịch sự nhất.",
                Citation = "Bài học 7 - Mẫu câu hỏi đường phố: 'Excuse me, could you tell me how to get to...?'",
                SourceType = "Extracted", SourceFile = "BaiHoc_07_MetroNavigation.txt", LessonId = 7,
                Options = new() { ("A", "Where is station? Tell me.", false), ("B", "Excuse me, could you tell me how to get to the central station?", true), ("C", "Show me the way immediately.", false), ("D", "Hey, walk me to the subway.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_057",
                TopicId = 4, LevelId = 1, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "What does the phrase 'Turn right at the crossroads' instruct you to do?",
                Explanation = "Rẽ phải tại ngã tư giao lộ (crossroads / intersection).",
                Citation = "Bài học 7 - Từ vựng định hướng: Turn left, Turn right, Go straight, Crossroads / Intersection.",
                SourceType = "Extracted", SourceFile = "BaiHoc_07_MetroNavigation.txt", LessonId = 7,
                Options = new() { ("A", "Go straight without stopping.", false), ("B", "Turn to the right hand side at the intersection of streets.", true), ("C", "Turn around and go backwards.", false), ("D", "Cross over the pedestrian bridge.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_058",
                TopicId = 4, LevelId = 1, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Fill in the blank: 'Go _______ on this avenue for two blocks, and the museum is on your left.'",
                Explanation = "Cụm chỉ đường 'Go straight on' có nghĩa là 'Đi thẳng tiếp tục'.",
                Citation = "Bài học 7 - Chỉ đường cơ bản: 'Go straight on / ahead'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_07_MetroNavigation.txt", LessonId = 7,
                Options = new() { ("A", "straight", true), ("B", "back", false), ("C", "crooked", false), ("D", "round", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_059",
                TopicId = 4, LevelId = 1, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "De",
                QuestionText = "In a metro system, what is the platform where you change from one subway line to another called?",
                Explanation = "Ga đổi tuyến hoặc điểm trung chuyển tàu điện ngầm được gọi là 'Transfer station' (hoặc Interchange station).",
                Citation = "Bài học 7 - Thuật ngữ tàu điện ngầm: Transfer station / Interchange.",
                SourceType = "Extracted", SourceFile = "BaiHoc_07_MetroNavigation.txt", LessonId = 7,
                Options = new() { ("A", "Terminal line", false), ("B", "Transfer station", true), ("C", "Dead end", false), ("D", "Depot", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_060",
                TopicId = 4, LevelId = 1, QuestionType = "NoiCap", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Match the navigation phrases with their visual meanings:\n1. 'Go straight ahead'\n2. 'It's across from the bank'\n3. 'Take the second left'",
                Explanation = "1 là đi thẳng; 2 là ở đối diện ngân hàng; 3 là rẽ vào khúc cua bên trái thứ hai.",
                Citation = "Bài học 7 - Biểu tượng và chỉ dẫn không gian trong đô thị.",
                SourceType = "Applied", SourceFile = "BaiHoc_07_MetroNavigation.txt", LessonId = 7,
                Options = new() { ("A", "1-Continue forward, 2-On the opposite side, 3-Turn at the 2nd left corner", true), ("B", "1-Stop, 2-Turn around, 3-Behind", false), ("C", "1-Turn right, 2-Next to, 3-Above", false), ("D", "1-Wait, 2-Underground, 3-Far away", false) }
            });

            // --- BÀI HỌC 15: Phân biệt Quá khứ Đơn (Past Simple) và Quá khứ Tiếp diễn (Past Continuous) (A2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_061",
                TopicId = 10, LevelId = 2, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Choose the correct sentence: 'While I _______ dinner, the telephone suddenly _______.'",
                Explanation = "Hành động đang diễn ra kéo dài trong quá khứ dùng Past Continuous ('was cooking'), hành động khác cắt ngang dùng Past Simple ('rang').",
                Citation = "Bài học 15 - Cấu trúc giao thoa While: While + Past Continuous, Past Simple.",
                SourceType = "Extracted", SourceFile = "BaiHoc_15_PastSimpleVsContinuous.txt", LessonId = 15,
                Options = new() { ("A", "cooked / was ringing", false), ("B", "was cooking / rang", true), ("C", "am cooking / rings", false), ("D", "was cooked / was ringing", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_062",
                TopicId = 10, LevelId = 2, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "What was Emily doing at 8:00 PM yesterday when the power went out?",
                Explanation = "Tại một thời điểm xác định cụ thể trong quá khứ (at 8:00 PM yesterday), hành động đang diễn ra chia ở Past Continuous: 'was studying'.",
                Citation = "Bài học 15 - Quy tắc thời điểm xác định trong quá khứ: At + giờ cụ thể + thời gian quá khứ -> Past Continuous.",
                SourceType = "Applied", SourceFile = "BaiHoc_15_PastSimpleVsContinuous.txt", LessonId = 15,
                Options = new() { ("A", "She studied her lessons.", false), ("B", "She was studying in her room.", true), ("C", "She has studied lessons.", false), ("D", "She is studying right then.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_063",
                TopicId = 10, LevelId = 2, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank with the correct form of 'see': 'Yesterday, we _______ an interesting movie about marine biology.'",
                Explanation = "Dấu hiệu 'Yesterday' đi với sự việc đã chấm dứt hoàn toàn trong quá khứ, dùng quá khứ đơn 'saw' (cột 2 của see).",
                Citation = "Bài học 15 - Dấu hiệu nhận biết thì Quá khứ đơn: Yesterday, ago, last week/year.",
                SourceType = "Extracted", SourceFile = "BaiHoc_15_PastSimpleVsContinuous.txt", LessonId = 15,
                Options = new() { ("A", "saw", true), ("B", "seen", false), ("C", "was seeing", false), ("D", "see", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_064",
                TopicId = 10, LevelId = 2, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Identify the mistake: 'When David arrived home, his children played games in the living room.'",
                Explanation = "Khi David về đến nhà, bọn trẻ 'đang chơi' dở dang (hành động đang xảy ra khi David bước vào), phải dùng 'were playing'.",
                Citation = "Bài học 15 - Phân tích câu ngữ cảnh cắt ngang: 'When + Past Simple, Past Continuous'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_15_PastSimpleVsContinuous.txt", LessonId = 15,
                Options = new() { ("A", "'arrived' should be 'arriving'", false), ("B", "'played' should be 'were playing'", true), ("C", "'in' should be 'at'", false), ("D", "'living room' should be 'lived room'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_065",
                TopicId = 10, LevelId = 2, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Combine the two sentences using 'While': 'I was walking home. It started raining heavily.'",
                Explanation = "Kết hợp chuẩn: 'While I was walking home, it started raining heavily.'",
                Citation = "Bài học 15 - Kỹ thuật nối câu với liên từ While và When.",
                SourceType = "Applied", SourceFile = "BaiHoc_15_PastSimpleVsContinuous.txt", LessonId = 15,
                Options = new() { ("A", "While I was walking home, it started raining heavily.", true), ("B", "While it started raining, I walked home.", false), ("C", "While I walked home, it was raining heavily.", false), ("D", "I was walking while it was starting rain.", false) }
            });

            // --- BÀI HỌC 16: Làm chủ Tương lai Đơn (Will) và Tương lai Gần (Be Going To) (A1 - A2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_066",
                TopicId = 10, LevelId = 1, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Look at the dark clouds in the sky! Which sentence expresses a prediction based on present evidence?",
                Explanation = "Dự đoán dựa trên bằng chứng hiện tại (nhìn thấy mây đen vần vũ) phải dùng 'be going to': 'It is going to rain.'",
                Citation = "Bài học 16 - Quy tắc Be Going To: Dự đoán có bằng chứng cụ thể ở hiện tại (Look at those black clouds!).",
                SourceType = "Extracted", SourceFile = "BaiHoc_16_WillVsBeGoingTo.txt", LessonId = 16,
                Options = new() { ("A", "It will rain probably someday.", false), ("B", "It is going to rain soon.", true), ("C", "It rains right now.", false), ("D", "It was raining yesterday.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_067",
                TopicId = 10, LevelId = 1, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "'The doorbell is ringing!' - What is the spontaneous decision reply?",
                Explanation = "Quyết định ngay tại thời điểm nói (spontaneous decision), không có kế hoạch từ trước, dùng 'will': 'I will get it.'",
                Citation = "Bài học 16 - Quy tắc thì Tương lai đơn (Will): Quyết định tức thời ngay khi nói.",
                SourceType = "Extracted", SourceFile = "BaiHoc_16_WillVsBeGoingTo.txt", LessonId = 16,
                Options = new() { ("A", "I am going to get it since last week.", false), ("B", "I will answer it!", true), ("C", "I have answered it already.", false), ("D", "I am answering forever.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_068",
                TopicId = 10, LevelId = 2, QuestionType = "DienKhuyet", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Complete the planned intention: 'Mark bought plane tickets yesterday. He _______ visit Japan next month.'",
                Explanation = "Kế hoạch hoặc dự định đã lên lịch trước (đã mua vé máy bay) dùng 'is going to'.",
                Citation = "Bài học 16 - Kế hoạch có sự chuẩn bị từ trước: 'be going to + V-infinitive'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_16_WillVsBeGoingTo.txt", LessonId = 16,
                Options = new() { ("A", "is going to", true), ("B", "will perhaps", false), ("C", "shall might", false), ("D", "goes to", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_069",
                TopicId = 10, LevelId = 1, QuestionType = "DungSai", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "True or False: We can use 'will' to make offers, promises, and quick requests (e.g., 'I promise I will not be late').",
                Explanation = "Đúng. 'Will' được dùng trong các lời hứa hẹn (promises), lời đề nghị giúp đỡ (offers) và lời yêu cầu.",
                Citation = "Bài học 16 - Chức năng giao tiếp của Will: Hứa hẹn, cảnh báo và lời đề nghị.",
                SourceType = "Extracted", SourceFile = "BaiHoc_16_WillVsBeGoingTo.txt", LessonId = 16,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_070",
                TopicId = 10, LevelId = 2, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the mistake: 'Don't worry, I am going to carry that heavy box for you right now!'",
                Explanation = "Đưa ra lời đề nghị giúp đỡ tức thì ngay lúc nhìn thấy hộp nặng phải dùng 'I will carry', không dùng 'am going to'.",
                Citation = "Bài học 16 - Lưu ý về lời đề nghị giúp đỡ (Offers): Dùng 'I will help you'.",
                SourceType = "Applied", SourceFile = "BaiHoc_16_WillVsBeGoingTo.txt", LessonId = 16,
                Options = new() { ("A", "'Don't worry' should be 'Not worry'", false), ("B", "'am going to carry' should be 'will carry'", true), ("C", "'heavy' should be 'heavily'", false), ("D", "'for you' should be 'to you'", false) }
            });

            // --- BÀI HỌC 29: Watch Sport (A2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_071",
                TopicId = 21, LevelId = 2, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "De",
                QuestionText = "In the audio text 'Watch Sport', what sport does Hazal like to watch the most on TV?",
                ReadingPassage = "Hello, my name is Hazal and I'm from Turkiye. My question is, do you watch sports on TV? Yes, I watch sports on TV. I like to watch tennis the most because it is very exciting and competitive.",
                Explanation = "Transcript nêu rõ: 'I like to watch tennis the most because it is very exciting and competitive.'",
                Citation = "Bài học 29 - Transcript: 'I like to watch tennis the most because it is very exciting'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_29_WatchSport.txt", LessonId = 29,
                Options = new() { ("A", "Football / Soccer", false), ("B", "Tennis", true), ("C", "Basketball", false), ("D", "Swimming", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_072",
                TopicId = 21, LevelId = 2, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Which adjective was used by Hazal to explain why tennis is enjoyable to watch?",
                Explanation = "Hazal giải thích cô ấy thích xem tennis vì nó 'exciting' (hào hứng, sôi nổi) và 'competitive' (tính cạnh tranh).",
                Citation = "Bài học 29 - Câu giải thích sở thích: 'because it is very exciting'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_29_WatchSport.txt", LessonId = 29,
                Options = new() { ("A", "Boring", false), ("B", "Exciting", true), ("C", "Cheap", false), ("D", "Noisy", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_073",
                TopicId = 21, LevelId = 2, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Complete the sentence: 'Hazal is from _______.'",
                Explanation = "Transcript câu đầu tiên: 'Hello, my name is Hazal and I'm from Turkiye.'",
                Citation = "Bài học 29 - Câu 1: 'my name is Hazal and I'm from Turkiye.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_29_WatchSport.txt", LessonId = 29,
                Options = new() { ("A", "Turkiye", true), ("B", "Tunisia", false), ("C", "Thailand", false), ("D", "Tanzania", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_074",
                TopicId = 21, LevelId = 2, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "What do you call the person who enforces the rules in a tennis match from an elevated chair?",
                Explanation = "Trọng tài điều khiển trận đấu tennis ngồi trên ghế cao được gọi là 'umpire'. (Trong bóng đá gọi là referee).",
                Citation = "Bài học 29 - Mở rộng từ vựng thể thao: Umpire (quần vợt/bóng chày) vs. Referee (bóng đá/bóng rổ).",
                SourceType = "Applied", SourceFile = "BaiHoc_29_WatchSport.txt", LessonId = 29,
                Options = new() { ("A", "Referee", false), ("B", "Umpire", true), ("C", "Judge", false), ("D", "Coach", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_075",
                TopicId = 21, LevelId = 2, QuestionType = "DungSai", Skill = "TuVung", Difficulty = "De",
                QuestionText = "True or False: In tennis, the four premier international tournaments (Australian Open, Roland Garros, Wimbledon, US Open) are known as the 'Grand Slams'.",
                Explanation = "Đúng. 4 giải đấu lớn nhất hành tinh của làng quần vợt thế giới được gọi là hệ thống Grand Slam.",
                Citation = "Bài học 29 - Kiến thức bổ trợ về môn quần vợt thế giới.",
                SourceType = "Applied", SourceFile = "BaiHoc_29_WatchSport.txt", LessonId = 29,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 30: Travelling VIETNAM, MYANMAR (A2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_076",
                TopicId = 21, LevelId = 2, QuestionType = "TracNghiem", Skill = "Nghe", Difficulty = "TrungBinh",
                QuestionText = "In the travel conversation, how did Abidemi travel into Vietnam after visiting Laos?",
                ReadingPassage = "Todd: So after Laos, where did you go?\nAbidemi: After Laos, I flew to Vietnam. And actually I took the bus into Vietnam.\nTodd: OK.\nAbidemi: Yes. And it was a very long ride through the mountains.",
                Explanation = "Abidemi đính chính: 'And actually I took the bus into Vietnam.' (đi xe buýt qua cửa khẩu vùng núi).",
                Citation = "Bài học 30 - Transcript: 'And actually I took the bus into Vietnam.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_30_TravellingVietnam.txt", LessonId = 30,
                Options = new() { ("A", "She took a train from Bangkok.", false), ("B", "She took the bus into Vietnam.", true), ("C", "She sailed on a cruise ship.", false), ("D", "She walked across the river.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_077",
                TopicId = 21, LevelId = 2, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "How did Abidemi describe the bus journey into Vietnam?",
                Explanation = "Abidemi chia sẻ chuyến đi xe buýt rất dài xuyên qua các dãy núi: 'it was a very long ride through the mountains.'",
                Citation = "Bài học 30 - Transcript: 'it was a very long ride through the mountains.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_30_TravellingVietnam.txt", LessonId = 30,
                Options = new() { ("A", "Short and boring", false), ("B", "A very long ride through the mountains", true), ("C", "Extremely luxurious and fast", false), ("D", "A flat trip across the desert", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_078",
                TopicId = 21, LevelId = 2, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Fill in the blank with a travel preposition: 'We travelled _______ bus from Vientiane to Hanoi.'",
                Explanation = "Cụm chỉ phương tiện giao thông đi với giới từ 'by': by bus, by train, by plane, by car.",
                Citation = "Bài học 30 - Ngữ pháp giới từ chỉ phương tiện giao thông (by bus / train / plane).",
                SourceType = "Extracted", SourceFile = "BaiHoc_30_TravellingVietnam.txt", LessonId = 30,
                Options = new() { ("A", "by", true), ("B", "with", false), ("C", "on", false), ("D", "in", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_079",
                TopicId = 21, LevelId = 2, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What iconic UNESCO World Heritage bay in northern Vietnam is famous for emerald waters and thousands of limestone islands?",
                Explanation = "Vịnh Hạ Long (Ha Long Bay) là di sản thiên nhiên thế giới nổi tiếng của miền bắc Việt Nam với hàng ngàn đảo đá vôi.",
                Citation = "Bài học 30 - Điểm đến du lịch Việt Nam: Ha Long Bay limestone karsts.",
                SourceType = "Applied", SourceFile = "BaiHoc_30_TravellingVietnam.txt", LessonId = 30,
                Options = new() { ("A", "Nha Trang Bay", false), ("B", "Ha Long Bay", true), ("C", "Phu Quoc Island", false), ("D", "Con Dao Archipelago", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_080",
                TopicId = 21, LevelId = 2, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: The ancient city of Bagan in Myanmar is world-renowned for its thousands of historical Buddhist temples and pagodas across the plains.",
                Explanation = "Đúng. Cố đô Bagan của Myanmar nổi tiếng toàn cầu với quần thể hàng ngàn ngôi đền và chùa tháp cổ kính.",
                Citation = "Bài học 30 - Phần chia sẻ du lịch Myanmar: The ancient temples of Bagan.",
                SourceType = "Extracted", SourceFile = "BaiHoc_30_TravellingVietnam.txt", LessonId = 30,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- TÀI LIỆU TEST UPLOAD: 01_04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf (A2 - B1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_081",
                TopicId = 17, LevelId = 2, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "In a standard international IT resume, which section lists programming languages, frameworks, and databases?",
                Explanation = "Mục 'Technical Skills' (Kỹ năng chuyên môn) là nơi liệt kê các ngôn ngữ lập trình, framework và công nghệ.",
                Citation = "Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Mục 2: Cấu trúc Resume quốc tế: Technical Skills.",
                SourceType = "Extracted", SourceFile = "04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf",
                Options = new() { ("A", "Hobbies and Interests", false), ("B", "Technical Skills", true), ("C", "Personal References", false), ("D", "High School Extracurriculars", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_082",
                TopicId = 17, LevelId = 2, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Which strong action verb should an engineer use on a CV instead of 'worked on' to show leadership?",
                Explanation = "Các động từ hành động mạnh như 'Spearheaded', 'Engineered', 'Architected' gây ấn tượng vượt trội so với 'worked on'.",
                Citation = "Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Action Verbs: Dùng động từ mạnh như Spearheaded, Implemented, Deployed.",
                SourceType = "Extracted", SourceFile = "04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf",
                Options = new() { ("A", "Helped with", false), ("B", "Spearheaded", true), ("C", "Did some", false), ("D", "Was involved in", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_083",
                TopicId = 17, LevelId = 3, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Transform the passive bullet point into a quantifiable achievement: 'The API was optimized by me.'",
                Explanation = "Quy tắc viết CV quốc tế: Động từ hành động + Nhiệm vụ + Kết quả định lượng (ví dụ: 'Optimized API response time by 40%').",
                Citation = "Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Công thức XYZ: Accomplished [X] as measured by [Y] by doing [Z].",
                SourceType = "Applied", SourceFile = "04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf",
                Options = new() { ("A", "Optimized RESTful API endpoints, reducing response latency by 40%.", true), ("B", "I was optimizing the API slowly.", false), ("C", "The API had been made faster by my code.", false), ("D", "Good job done on API.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_084",
                TopicId = 17, LevelId = 2, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: According to international tech hiring standards, a candidate's resume should ideally be concise and fit within 1 to 2 pages.",
                Explanation = "Đúng. Tiêu chuẩn CV công nghệ cho lập trình viên dưới 5 năm kinh nghiệm là 1 trang gọn gàng, tối đa 2 trang.",
                Citation = "Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Chiều dài tiêu chuẩn của Resume: 1-2 trang.",
                SourceType = "Extracted", SourceFile = "04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_085",
                TopicId = 17, LevelId = 2, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Complete the CV header term: 'GitHub _______: github.com/developer-profile'",
                Explanation = "Trong CV lập trình viên, link mã nguồn được gọi là 'GitHub Profile' hoặc 'GitHub Repository'.",
                Citation = "Tài liệu 04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf - Thông tin liên hệ: GitHub Profile, LinkedIn.",
                SourceType = "Extracted", SourceFile = "04_Mau_CV_Resume_chuan_Quoc_te_IT_Engineer.pdf",
                Options = new() { ("A", "Profile", true), ("B", "Novel", false), ("C", "Ticket", false), ("D", "Poster", false) }
            });

            // --- THÊM CÂU HỎI VẬN DỤNG DU LỊCH & ĐỜI SỐNG (A1 - A2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_086",
                TopicId = 4, LevelId = 2, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "You see a sign at the railway platform: 'Mind the gap.' What does it instruct passengers to do?",
                Explanation = "'Mind the gap' là khẩu hiệu nổi tiếng ở các ga tàu điện ngầm Luân Đôn nhắc nhở chú ý khoảng cách giữa mép tàu và sân ga.",
                Citation = "Bài học 7 - Biển báo an toàn giao thông công cộng: 'Mind the gap'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_07_MetroNavigation.txt", LessonId = 7,
                Options = new() { ("A", "Jump over the railway tracks quickly.", false), ("B", "Be careful of the space between the train door and the station platform.", true), ("C", "Do not talk to anyone while riding the train.", false), ("D", "Buy extra tickets before entering.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_087",
                TopicId = 4, LevelId = 2, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Which term describes a hotel room with two separate single beds?",
                Explanation = "'Twin room' là phòng khách sạn có 2 giường đơn riêng biệt. 'Double room' là phòng có 1 giường đôi lớn.",
                Citation = "Bài học 6 - Từ vựng lưu trú du lịch: Single room, Double room, Twin room.",
                SourceType = "Applied", SourceFile = "BaiHoc_06_AirportNavigation.txt", LessonId = 6,
                Options = new() { ("A", "Single room", false), ("B", "Twin room", true), ("C", "Double room", false), ("D", "Studio room", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_088",
                TopicId = 10, LevelId = 2, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Choose the irregular past simple form of 'fly': 'Last summer, our family _______ to Da Nang for holiday.'",
                Explanation = "Động từ bất quy tắc 'fly' chuyển sang quá khứ đơn là 'flew' (cột 2).",
                Citation = "Bài học 15 - Bảng động từ bất quy tắc thông dụng: fly -> flew -> flown.",
                SourceType = "Extracted", SourceFile = "BaiHoc_15_PastSimpleVsContinuous.txt", LessonId = 15,
                Options = new() { ("A", "flied", false), ("B", "flew", true), ("C", "flown", false), ("D", "flying", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_089",
                TopicId = 10, LevelId = 2, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Which sentence correctly contrasts an ongoing action with an interruption?",
                Explanation = "'I was reading a novel when the doorbell rang.' đúng cấu trúc: Past Continuous diễn tả hành động đang xảy ra, Past Simple diễn tả hành động chen ngang.",
                Citation = "Bài học 15 - Công thức liên kết When: Past Continuous + when + Past Simple.",
                SourceType = "Extracted", SourceFile = "BaiHoc_15_PastSimpleVsContinuous.txt", LessonId = 15,
                Options = new() { ("A", "I read a novel when the doorbell was ringing.", false), ("B", "I was reading a novel when the doorbell rang.", true), ("C", "I have read a novel when the doorbell has rung.", false), ("D", "I was read a novel when the doorbell ringed.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_090",
                TopicId = 4, LevelId = 1, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "De",
                QuestionText = "You want to buy a return ticket at the train station. What do you say?",
                Explanation = "'I'd like a return ticket to Oxford, please.' (return ticket: vé khứ hồi, Anh-Mỹ gọi là round-trip ticket).",
                Citation = "Bài học 7 - Mẫu câu mua vé tàu xe: Single ticket (vé 1 chiều) vs. Return ticket (vé khứ hồi).",
                SourceType = "Extracted", SourceFile = "BaiHoc_07_MetroNavigation.txt", LessonId = 7,
                Options = new() { ("A", "One return ticket to Oxford, please.", true), ("B", "Give me train to go and never back.", false), ("C", "I demand Oxford seat right away.", false), ("D", "Oxford ticket is wanted by me.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_091",
                TopicId = 4, LevelId = 2, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Fill in the blank: 'Please keep your seatbelt _______ while the aircraft is experiencing turbulence.'",
                Explanation = "Thông báo máy bay: 'fastened' (được thắt chặt): 'fasten your seatbelt'.",
                Citation = "Bài học 6 - Thông báo an toàn bay: 'Keep seatbelts fastened'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_06_AirportNavigation.txt", LessonId = 6,
                Options = new() { ("A", "fastened", true), ("B", "unlocked", false), ("C", "loosened", false), ("D", "broken", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_092",
                TopicId = 10, LevelId = 2, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "TrungBinh",
                QuestionText = "Find the error: 'Yesterday afternoon at 3 PM, she cooked in the kitchen.'",
                Explanation = "Thời điểm cụ thể trong quá khứ (at 3 PM yesterday afternoon) phải dùng thì Quá khứ tiếp diễn: 'was cooking'.",
                Citation = "Bài học 15 - Lưu ý mốc thời gian cụ thể: At + giờ + quá khứ -> Past Continuous.",
                SourceType = "Applied", SourceFile = "BaiHoc_15_PastSimpleVsContinuous.txt", LessonId = 15,
                Options = new() { ("A", "'Yesterday' should be 'Tomorrow'", false), ("B", "'cooked' should be 'was cooking'", true), ("C", "'in' should be 'into'", false), ("D", "'kitchen' should be 'kitchens'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_093",
                TopicId = 4, LevelId = 2, QuestionType = "NoiCap", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Match airport facilities with their purposes:\n1. 'Baggage Claim'\n2. 'Currency Exchange'\n3. 'Duty-Free Shop'",
                Explanation = "1 là nơi nhận hành lý sau chuyến bay; 2 là quầy đổi ngoại tệ; 3 là cửa hàng miễn thuế tại sân bay.",
                Citation = "Bài học 6 - Sơ đồ và tiện ích sân bay quốc tế.",
                SourceType = "Extracted", SourceFile = "BaiHoc_06_AirportNavigation.txt", LessonId = 6,
                Options = new() { ("A", "1-Collecting checked luggage, 2-Converting foreign money, 3-Buying tax-exempt products", true), ("B", "1-Buying food, 2-Checking passports, 3-Boarding plane", false), ("C", "1-Sleeping, 2-Driving cars, 3-Booking flights", false), ("D", "1-Medical room, 2-Security check, 3-Lost property", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_094",
                TopicId = 10, LevelId = 1, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Complete the future promise: 'I promise I _______ tell anyone your secret.'",
                Explanation = "Lời hứa phủ định dùng 'will not' hoặc 'won't': 'I promise I won't tell...'.",
                Citation = "Bài học 16 - Lời hứa (Promises) dùng thì Tương lai đơn (Will / Won't).",
                SourceType = "Extracted", SourceFile = "BaiHoc_16_WillVsBeGoingTo.txt", LessonId = 16,
                Options = new() { ("A", "won't", true), ("B", "am not going to ever", false), ("C", "did not", false), ("D", "have not", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_095",
                TopicId = 4, LevelId = 1, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "What is the opposite direction of 'North'?",
                Explanation = "Hướng đối diện của Bắc (North) là Nam (South).",
                Citation = "Bài học 7 - 4 hướng cơ bản trên bản đồ: North, South, East, West.",
                SourceType = "Extracted", SourceFile = "BaiHoc_07_MetroNavigation.txt", LessonId = 7,
                Options = new() { ("A", "East", false), ("B", "South", true), ("C", "West", false), ("D", "Northeast", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_096",
                TopicId = 21, LevelId = 2, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "In football, what happens when a player receives two yellow cards in a single match?",
                Explanation = "Trong bóng đá, 2 thẻ vàng tương đương 1 thẻ đỏ (red card) và cầu thủ bị đuổi khỏi sân.",
                Citation = "Bài học 29 - Luật chơi bóng đá cơ bản: 2 yellow cards = red card (sent off).",
                SourceType = "Applied", SourceFile = "BaiHoc_29_WatchSport.txt", LessonId = 29,
                Options = new() { ("A", "The player is given a penalty kick.", false), ("B", "The player is shown a red card and sent off the pitch.", true), ("C", "The match ends immediately.", false), ("D", "The team gets an extra player.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_097",
                TopicId = 4, LevelId = 2, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Fill in the blank: 'A tourist visa allows travelers to stay _______ a maximum of 30 days.'",
                Explanation = "Cụm chỉ thời lượng: 'stay for a maximum of 30 days' (lưu trú trong tối đa 30 ngày).",
                Citation = "Bài học 6 - Thuật ngữ Visa du lịch và thời hạn lưu trú.",
                SourceType = "Applied", SourceFile = "BaiHoc_06_AirportNavigation.txt", LessonId = 6,
                Options = new() { ("A", "for", true), ("B", "during", false), ("C", "at", false), ("D", "by", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_098",
                TopicId = 10, LevelId = 1, QuestionType = "TracNghiem", Skill = "NguPhap", Difficulty = "De",
                QuestionText = "Which question correctly asks for permission or offers help?",
                Explanation = "'Shall I carry your bag for you?' là mẫu câu đề nghị giúp đỡ trang trọng và lịch sự của ngôi 'I'.",
                Citation = "Bài học 16 - Mẫu câu Shall I / Shall we trong lời đề nghị lịch sự.",
                SourceType = "Extracted", SourceFile = "BaiHoc_16_WillVsBeGoingTo.txt", LessonId = 16,
                Options = new() { ("A", "Shall I carry your bag?", true), ("B", "Will I be carrying your bag?", false), ("C", "Do I carry your bag necessarily?", false), ("D", "Am I carrying your bag now?", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_099",
                TopicId = 21, LevelId = 2, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Read the notice: 'Passengers must keep their luggage with them at all times. Unattended items will be removed and may be destroyed.' What should travelers do?",
                Explanation = "Hành khách phải luôn mang hành lý bên mình; hành lý vô chủ (unattended) sẽ bị tịch thu và tiêu hủy vì lý do an ninh.",
                Citation = "Bài học 6 - Cảnh báo an ninh hành lý vô chủ: 'Unattended baggage will be confiscated.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_06_AirportNavigation.txt", LessonId = 6,
                Options = new() { ("A", "Leave bags with any stranger.", false), ("B", "Never leave personal baggage unattended in public airport areas.", true), ("C", "Put bags on the railway tracks.", false), ("D", "Forget about their baggage during flights.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_100",
                TopicId = 4, LevelId = 1, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "De",
                QuestionText = "A fellow traveler asks: 'Is this seat taken?' - If nobody is sitting there, what do you say?",
                Explanation = "Đáp lại lịch sự: 'No, it's free. Please go ahead!' (Không, ghế trống. Xin mời bạn ngồi!).",
                Citation = "Bài học 7 - Giao tiếp trên tàu xe công cộng: Hỏi ghế trống.",
                SourceType = "Extracted", SourceFile = "BaiHoc_07_MetroNavigation.txt", LessonId = 7,
                Options = new() { ("A", "No, it's free. Please have a seat!", true), ("B", "Yes, go away.", false), ("C", "I bought this entire coach.", false), ("D", "Never ask me questions.", false) }
            });

            return list;
        }
    }
}

