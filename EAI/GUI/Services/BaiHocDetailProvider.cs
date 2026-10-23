using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DTO;

namespace GUI.Services
{
    public class LessonDetailedContent
    {
        public string Objective { get; set; } = string.Empty;
        public string FormulaTitle { get; set; } = string.Empty;
        public string Formula { get; set; } = string.Empty;
        public string ExampleTitle { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
        public string TipsTitle { get; set; } = string.Empty;
        public string Tips { get; set; } = string.Empty;
        public string FullContent { get; set; } = string.Empty;

        // Bổ sung đoạn văn tiếng Anh chuyên biệt cho kỹ năng Đọc hiểu (Reading Passage)
        public string? ReadingPassageTitle { get; set; }
        public string? ReadingPassageContent { get; set; }
    }

    /// <summary>
    /// Cung cấp nội dung kiến thức, đoạn văn đọc hiểu tiếng Anh, bảng từ vựng chuyên sâu,
    /// công thức ngữ pháp, ví dụ minh họa và bộ câu hỏi bài tập củng cố được tạo trực tiếp từ nội dung bài học.
    /// </summary>
    public static class BaiHocDetailProvider
    {
        private static readonly Dictionary<int, LessonDetailedContent> _curriculumDatabase = new()
        {
            // =========================================================================
            // BÀI HỌC 1: NGHE HIỂU - GIAO TIẾP CƠ BẢN
            // =========================================================================
            [1] = new LessonDetailedContent
            {
                Objective = "Luyện phản xạ nghe hiểu các đoạn hội thoại làm quen tự nhiên; nắm bắt tên tuổi, quốc tịch, nghề nghiệp và sở thích với phát âm chuẩn ngữ điệu bản xứ.",
                FormulaTitle = "🎧 BẢN THU LỜI THOẠI & KỸ THUẬT NỐI ÂM (AUDIO SCRIPT & LINKING)",
                Formula = "• Lời thoại hội thoại chuẩn:\n" +
                          "  - Alex: \"Hello! My name is Alex. Nice to meet you!\"\n" +
                          "  - Sarah: \"Hi Alex, I'm Sarah. Pleased to meet you too! Where are you from?\"\n" +
                          "  - Alex: \"I'm from Canada, and I work as a software engineer. How about you?\"\n" +
                          "  - Sarah: \"I was born in London, but I'm currently studying marketing here.\"\n" +
                          "• Hiện tượng nối âm (Connected Speech):\n" +
                          "  - Nice to meet_you -> /naɪs tə ˈmiːtʃu/\n" +
                          "  - Where are_you from? -> /weər ɑːr juː frɒm/\n" +
                          "  - I work_as_a -> /aɪ wɜːk əz ə/",
                ExampleTitle = "💡 MẪU CÂU GIAO TIẾP THỰC TẾ & DỊCH NGHĨA",
                Example = "1. \"Allow me to introduce myself. My name is Daniel, and I specialize in data analytics.\"\n" +
                          "   -> Dịch: Cho phép tôi được tự giới thiệu. Tôi tên là Daniel, chuyên môn của tôi là phân tích dữ liệu.\n\n" +
                          "2. \"What do you enjoy doing in your leisure time? - I'm really keen on photography.\"\n" +
                          "   -> Dịch: Bạn thích làm gì vào thời gian rảnh rỗi? - Tôi rất đam mê chụp ảnh phong cảnh.\n\n" +
                          "3. \"It's a pleasure making your acquaintance today!\"\n" +
                          "   -> Dịch: Rất vinh hạnh được làm quen với bạn hôm nay!",
                TipsTitle = "⚠️ BẪY NGHE & MẸO PHẢN XẠ PHÁT ÂM (LISTENING TIPS)",
                Tips = "• BẪY TÊN RIÊNG & ĐỊA DANH: Trong bài thi nghe A1-A2, tên riêng thường được đánh vần từng chữ cái (Spelling). Chú ý các cặp chữ dễ nhầm: A /eɪ/ - E /iː/ - I /aɪ/ và G /dʒiː/ - J /dʒeɪ/.\n" +
                       "• Trả lời tự nhiên: Khi người đối diện nói 'Nice to meet you', hãy đáp lại bằng 'Nice to meet you too' hoặc 'Likewise' để giao tiếp trôi chảy.",
                FullContent = "I. NGUYÊN TẮC GIỚI THIỆU BẢN THÂN CƠ BẢN\n" +
                              "1. Chào hỏi ban đầu: 'Hello' / 'Good morning' (trang trọng) hoặc 'Hi' / 'Hey' (thân mật).\n" +
                              "2. Giới thiệu tên: 'My name is [Tên]' hoặc 'I am [Tên]'.\n" +
                              "3. Nguồn gốc & Quốc tịch: 'I come from [Đất nước]' hoặc 'I was born and raised in [Thành phố]'.\n" +
                              "4. Nghề nghiệp: 'I work as a/an [Nghề nghiệp]' hoặc 'I am currently employed at [Tên công ty]'."
            },

            // =========================================================================
            // BÀI HỌC 2: TỪ VỰNG - CHÀO HỎI TRANG TRỌNG VS THÂN MẬT
            // =========================================================================
            [2] = new LessonDetailedContent
            {
                Objective = "Làm chủ vốn từ vựng phong phú, sắc thái biểu cảm và phân biệt rạch ròi giữa văn phong Chào hỏi Trang trọng (Formal) và Thân mật (Informal).",
                FormulaTitle = "📖 BẢNG TỪ VỰNG THEN CHỐT & PHÁT ÂM IPA (KEY VOCABULARY)",
                Formula = "1. Acquaintance /əˈkweɪntəns/ (n): Người quen xã giao (chưa phải bạn thân)\n" +
                          "2. Cordially /ˈkɔːdiəli/ (adv): Một cách chân thành, nồng hậu (dùng trong thư mời)\n" +
                          "3. Professionalism /prəˈfeʃənəlɪzəm/ (n): Phong thái chuyên nghiệp, chuẩn mực công sở\n" +
                          "4. Courtesy /ˈkɜːtəsi/ (n): Sự lịch thiệp, nhã nhặn, tôn trọng người khác\n" +
                          "5. Casual /ˈkæʒuəl/ (adj): Thân mật, thoải mái, đời thường\n" +
                          "6. Approachable /əˈprəʊtʃəbl/ (adj): Dễ gần, thân thiện, cởi mở",
                ExampleTitle = "💡 ĐỐI CHIẾU CÂU HỘI THOẠI TRANG TRỌNG VS. THÂN MẬT",
                Example = "• Bối cảnh Trang trọng (Phỏng vấn tuyển dụng / Đối tác doanh nghiệp):\n" +
                          "  \"Good afternoon, Dr. Evans. It is an immense honor to meet you in person. Thank you for accommodating this meeting.\"\n" +
                          "  -> Dịch: Kính chào buổi chiều Giáo sư Evans. Thật là một vinh dự to lớn khi được gặp trực tiếp thầy. Cảm ơn thầy đã dành thời gian cho cuộc gặp gỡ này.\n\n" +
                          "• Bối cảnh Thân mật (Gặp bạn học / Bạn đồng trang lứa):\n" +
                          "  \"Hey Lucas! What have you been up to lately? Long time no see, let's grab lunch!\"\n" +
                          "  -> Dịch: Chào Lucas! Dạo này cậu làm gì rồi? Lâu lắm không gặp, cùng đi ăn trưa tán gẫu đi!",
                TipsTitle = "⚠️ BẪY SỬ DỤNG TỪ & NGUYÊN TẮC VĂN HÓA (USAGE NOTES)",
                Tips = "• BẪY 'HOW DO YOU DO?': Đây là lời chào trang trọng tuyệt đối trong tiếng Anh chuẩn, KHÔNG PHẢI câu hỏi sức khỏe. Đáp lại người nói cũng bằng: 'How do you do?'. TUYỆT ĐỐI không trả lời 'I am fine, thank you'!\n" +
                       "• Tránh dùng từ lóng (slang) và từ viết tắt như 'gonna', 'wanna', 'dunno', 'what's up' trong email công việc hoặc khi nói chuyện với cấp trên.",
                FullContent = "I. BẢNG PHÂN LOẠI MẪU CÂU CHÀO HỎI THEO BỐI CẢNH\n" +
                              "1. Trang trọng (Formal): 'Good morning/afternoon/evening', 'How do you do?', 'It is a pleasure to make your acquaintance', 'I am delighted to meet you'.\n" +
                              "2. Bán trang trọng (Semi-formal): 'Hello everyone', 'Nice to meet you', 'How are you doing today?'.\n" +
                              "3. Thân mật (Informal): 'Hey!', 'What's going on?', 'How's it going?', 'Great to see you!'."
            },

            // =========================================================================
            // BÀI HỌC 3: ĐỌC HIỂU - GIỚI THIỆU NGHỀ NGHIỆP & ĐAM MÊ
            // =========================================================================
            [3] = new LessonDetailedContent
            {
                Objective = "Nâng cao kỹ năng đọc hiểu đoạn văn miêu tả sự nghiệp, tiểu sử chuyên gia và nắm vững kỹ thuật tự giới thiệu 3 bước (Hook - Core Value - Vision).",
                ReadingPassageTitle = "Professional Narrative: Engineering High-Impact Software Solutions",
                ReadingPassageContent = "In today's interconnected knowledge economy, effective personal branding and professional articulation have become quintessential competencies for technological specialists. Consider the career trajectory of Dr. Marcus Vance, a principal distributed systems architect currently based in Singapore.\n\n" +
                                      "Originally hailing from a coastal town in Central Vietnam, Marcus discovered his enduring fascination with algorithmic computation during his formative undergraduate studies. Over the ensuing decade, he spearheaded multiple multinational digital transformation initiatives, orchestrating large-scale cloud infrastructure migrations that reduced corporate operational latency by forty percent. Beyond technical acumen, Marcus consistently emphasizes that genuine professional progression stems from continuous intellectual curiosity, transparent communication, and empathetic mentorship.\n\n" +
                                      "Looking toward the next technological frontier, Marcus actively collaborates with international academic consortia to pioneer energy-efficient artificial intelligence architectures. His professional philosophy demonstrates that a compelling self-introduction does not merely enumerate past credentials; rather, it articulates an overarching vision of measurable value creation and lifelong dedication to societal advancement.",
                FormulaTitle = "📖 CHIẾN LƯỢC BỐ CỤC ĐOẠN VĂN (READING & BIO FRAMEWORK)",
                Formula = "• Khung bài tự giới thiệu chuyên nghiệp (3-Step Bio Model):\n" +
                          "  1. The Hook & Identity: Họ tên, vị trí hiện tại và niềm đam mê cốt lõi.\n" +
                          "  2. Core Competency & Achievements: Kinh nghiệm then chốt, kỹ năng thế mạnh và các dự án tiêu biểu.\n" +
                          "  3. Future Vision & Call to Action: Định hướng cống hiến, giá trị mang lại cho tổ chức.",
                ExampleTitle = "💡 TỪ VỰNG THEN CHỐT TRONG ĐOẠN VĂN ĐỌC HIỂU",
                Example = "1. Quintessential /ˌkwɪntɪˈsenʃl/ (adj): Hoàn hảo nhất, tiêu biểu nhất.\n" +
                          "2. Spearhead /ˈspɪəhed/ (v): Tiên phong chỉ huy, dẫn đầu một chiến dịch/dự án.\n" +
                          "3. Technical acumen /ˈækjəmən/ (n): Sự nhạy bén sắc sảo về công nghệ kỹ thuật.\n" +
                          "4. Digital transformation (n): Chuyển đổi số toàn diện trong doanh nghiệp.",
                TipsTitle = "⚠️ BẪY ĐỌC HIỂU & TỪ ĐỒNG NGHĨA (EXAM TIPS)",
                Tips = "• Chú ý các từ nối biểu thị sự mở rộng ý: 'In addition to', 'Apart from', 'Furthermore'.\n" +
                       "• Bẫy thì trong bài đọc tiểu sử: Tác giả thường kết hợp Quá khứ đơn (nơi sinh, trường học), Hiện tại hoàn thành (số năm kinh nghiệm) và Hiện tại đơn (vị trí hiện tại).",
                FullContent = "I. PHÂN TÍCH CẤU TRÚC ĐOẠN VĂN TIỂU SỬ CHUYÊN NGHIỆP\n" +
                              "Một bài giới thiệu nghề nghiệp chuẩn mực cần trả lời rõ ràng 3 câu hỏi của nhà tuyển dụng:\n" +
                              "• Bạn là ai và chuyên môn sâu nhất của bạn là gì? (Who are you?)\n" +
                              "• Bạn đã đạt được những thành tựu có thể đo lường nào? (What value have you delivered?)\n" +
                              "• Bạn hướng tới mục tiêu gì tiếp theo? (What is your long-term vision?)"
            },

            // =========================================================================
            // BÀI HỌC 4: NGHE HIỂU - ĐẶT BÀN VÀ GỌI MÓN NHÀ HÀNG
            // =========================================================================
            [4] = new LessonDetailedContent
            {
                Objective = "Luyện nghe các tình huống đặt bàn, gọi món ăn, hỏi nguyên liệu thực đơn, yêu cầu món chay và thanh toán hóa đơn tại nhà hàng quốc tế.",
                FormulaTitle = "🎧 BẢN THU LỜI THOẠI GỌI MÓN (AUDIO SCRIPT & PHRASES)",
                Formula = "• Lời thoại gọi món tiêu chuẩn:\n" +
                          "  - Waiter: \"Good evening! Are you ready to order, or would you like a few more minutes?\"\n" +
                          "  - Guest: \"We're ready. Could we start with the mushroom soup, please?\"\n" +
                          "  - Waiter: \"Excellent choice! And what would you prefer for the main course?\"\n" +
                          "  - Guest: \"I'll have the grilled salmon with steamed vegetables on the side.\"\n" +
                          "  - Waiter: \"How would you like your steak cooked? (Rare / Medium / Well-done)?\"\n" +
                          "• Mẫu câu thanh toán:\n" +
                          "  - \"Could we have the bill, please?\"\n" +
                          "  - \"Can we split the bill, or do you prefer one payment?\"",
                ExampleTitle = "💡 MẪU CÂU GIAO TIẾP TẠI NHÀ HÀNG & DỊCH NGHĨA",
                Example = "1. \"Do you have any dietary restrictions or food allergies we should be aware of?\"\n" +
                          "   -> Dịch: Quý khách có yêu cầu kiêng khem hoặc dị ứng thực phẩm nào cần nhà bếp lưu ý không?\n\n" +
                          "2. \"Is service charge included in the total bill, or should we leave a tip?\"\n" +
                          "   -> Dịch: Phí dịch vụ đã bao gồm trong tổng hóa đơn chưa, hay chúng tôi để lại tiền boa riêng?\n\n" +
                          "3. \"Excuse me, could you bring us some extra napkins and a glass of sparkling water?\"\n" +
                          "   -> Dịch: Xin lỗi, bạn có thể mang thêm cho chúng tôi khăn ăn và một ly nước khoáng có ga được không?",
                TipsTitle = "⚠️ BẪY NGHE BÀI THI & TỪ VỰNG DỄ NHẦM (LISTENING TIPS)",
                Tips = "• BẪY TỔNG TIỀN (Total Amount): Người phục vụ thường đọc giá chưa tính thuế (before tax/VAT) và giá sau khi cộng phí phục vụ. Hãy chú ý nghe từ 'total', 'including VAT' để chọn đáp án đúng.\n" +
                       "• Phân biệt 'Check' (tiếng Anh Mỹ) và 'Bill' (tiếng Anh Anh) - cả hai đều mang nghĩa là hóa đơn thanh toán bữa ăn.",
                FullContent = "I. CÁC GIAI ĐOẠN CỦA MỘT BỮA ĂN TẠI NHÀ HÀNG QUỐC TẾ\n" +
                              "1. Đặt bàn trước (Reservation): 'I'd like to book a table for four at 7 PM under the name of Davis.'\n" +
                              "2. Món khai vị (Appetizers / Starters): Súp, salad, bánh mì bơ.\n" +
                              "3. Món chính (Main courses / Entrées): Bò bít tết, hải sản nướng, mì Ý.\n" +
                              "4. Món tráng miệng (Desserts): Bánh ngọt, kem, trái cây.\n" +
                              "5. Đồ uống (Beverages): Nước lọc (tap water), nước khoáng (still/sparkling water), rượu vang."
            },

            // =========================================================================
            // BÀI HỌC 5: TỪ VỰNG - MUA SẮM & ĐỔI TRẢ HÀNG HÓA
            // =========================================================================
            [5] = new LessonDetailedContent
            {
                Objective = "Làm chủ bộ từ vựng mua sắm thời trang, thiết bị điện tử, kích cỡ trang phục, chính sách bảo hành và thủ tục đổi trả hàng hóa/hoàn tiền.",
                FormulaTitle = "📖 BẢNG TỪ VỰNG THEN CHỐT & PHÁT ÂM IPA (SHOPPING VOCABULARY)",
                Formula = "1. Refund /ˈriːfʌnd/ (n, v): Tiền hoàn lại; hoàn trả lại tiền mặt/tài khoản\n" +
                          "2. Receipt /rɪˈsiːt/ (n): Hóa đơn, biên lai mua hàng (chú ý: chữ 'p' câm)\n" +
                          "3. Fitting room /ˈfɪtɪŋ ruːm/ (n): Phòng thử trang phục trong cửa hàng\n" +
                          "4. Defective /dɪˈfektɪv/ (adj): Bị lỗi sản xuất, hư hỏng, khiếm khuyết\n" +
                          "5. Warranty /ˈwɒrənti/ (n): Phiếu bảo hành, cam kết bảo hành của hãng\n" +
                          "6. Exchange /ɪksˈtʃeɪndʒ/ (v, n): Đổi lấy sản phẩm khác hoặc kích cỡ khác\n" +
                          "7. Out of stock /ˌaʊt əv ˈstɒk/ (adj phr): Tạm thời hết hàng trong kho",
                ExampleTitle = "💡 MẪU CÂU GIAO TIẾP MUA SẮM & KHIẾU NẠI ĐỔI TRẢ",
                Example = "• Khách hàng khiếu nại đổi hàng:\n" +
                          "  \"Good afternoon. I purchased this wool sweater yesterday, but upon opening the package, I noticed a tear along the seam. Here is my receipt. Could I exchange it for a new one?\"\n" +
                          "  -> Dịch: Chào buổi chiều. Tôi đã mua chiếc áo len này hôm qua nhưng khi mở kiện hàng ra, tôi phát hiện một vết rách dọc đường may. Đây là hóa đơn của tôi. Tôi có thể đổi sang chiếc mới được không?\n\n" +
                          "• Nhân viên phản hồi tích cực:\n" +
                          "  \"We sincerely apologize for the oversight. Since this item is covered by our 30-day exchange policy, I will fetch an identical one for you immediately.\"\n" +
                          "  -> Dịch: Chúng tôi chân thành xin lỗi vì sự sơ suất này. Vì món hàng được áp dụng chính sách đổi trả trong 30 ngày, em sẽ lấy ngay một chiếc mới y hệt cho quý khách.",
                TipsTitle = "⚠️ LƯU Ý PHÁT ÂM & BẪY THƯƠNG MẠI (SHOPPING TIPS)",
                Tips = "• PHÁT ÂM CHUẨN: Từ 'Receipt' phát âm là /rɪˈsiːt/ - tuyệt đối không phát âm chữ 'p' (/ri-sipt/ là SAI)!\n" +
                       "• Phân biệt 'Full Refund' (hoàn trả 100% tiền qua thẻ/tiền mặt) vs. 'Store Credit' (phiếu điểm thưởng mua hàng chỉ dùng được tại cửa hàng đó).",
                FullContent = "I. TỪ VỰNG KÍCH CỠ VÀ ĐỘ VỪA VẶN CỦA QUẦN ÁO\n" +
                              "• Tight: Bó sát, chật chội -> 'This shirt is a bit too tight around the shoulders.'\n" +
                              "• Loose / Baggy: Rộng thùng thình -> 'I prefer a slightly loose fit for sportswear.'\n" +
                              "• It suits you: Nó rất hợp với bạn (về màu sắc, phong cách).\n" +
                              "• It fits you: Nó vừa vặn với kích cỡ cơ thể bạn."
            },

            // =========================================================================
            // BÀI HỌC 6: ĐỌC HIỂU - ẨM THỰC ĐƯỜNG PHỐ & HƯƠNG VỊ MÓN ĂN
            // =========================================================================
            [6] = new LessonDetailedContent
            {
                Objective = "Luyện đọc hiểu đoạn văn văn hóa ẩm thực Á - Âu; nắm bắt tính từ miêu tả cảm quan hương vị tinh tế (crispy, savory, fragrant, succulent).",
                ReadingPassageTitle = "Street Food Gastronomy: A Sensory Exploration of Asian Night Markets",
                ReadingPassageContent = "Street food culture represents far more than an economical dining alternative; it serves as a dynamic living repository of cultural heritage and culinary ingenuity. From the bustling night bazaars of Bangkok and Taipei to the vibrant sidewalk eateries of Hanoi and George Town, open-air cooking captivates millions of locals and international epicures alike.\n\n" +
                                      "At the heart of Southeast Asian gastronomy lies the delicate harmony of the five primary taste sensations: sweet, sour, salty, bitter, and savory umami. Authentic dishes such as char kway teow and traditional noodle broths rely heavily on the concept of 'wok hei'—the elusive breath of the wok achieved only under intense, roaring flames. The juxtaposition of textures is equally striking: crispy shallots and crushed roasted peanuts provide a delightful contrast against succulent, slow-braised meats and silky rice noodles.\n\n" +
                                      "In recent years, global culinary critics have increasingly recognized street vendors with prestigious accolades, including Michelin Bib Gourmand designations. Far from being simplistic, traditional street recipes embody generations of culinary refinement, where fresh indigenous herbs such as Thai basil, lemongrass, and Vietnamese coriander are meticulously balanced to invigorate the palate without overwhelming delicate natural flavors.",
                FormulaTitle = "📖 CHIẾN LƯỢC ĐỌC HIỂU VĂN BẢN ẨM THỰC (CULINARY READING STRATEGY)",
                Formula = "• Nhận diện hệ thống tính từ miêu tả cảm quan (Sensory Descriptors):\n" +
                          "  - Texture (Kết cấu): Crispy (giòn rụm), Silky (mịn màng), Succulent (mọng nước).\n" +
                          "  - Taste (Vị giác): Savory / Umami (đậm đà, vừa miệng), Tangy (chua thanh dịu).\n" +
                          "  - Aroma (Khứu giác): Aromatic / Fragrant (thơm nức mũi), Pungent (nồng đậm).",
                ExampleTitle = "💡 ĐỐI CHIẾU CÂU HỎI ĐỌC HIỂU & DỊCH NGHĨA",
                Example = "• Câu hỏi: What is the primary purpose of adding crispy shallots and crushed peanuts?\n" +
                          "• Trích dẫn bài đọc: \"...crispy shallots and crushed roasted peanuts provide a delightful contrast against succulent, slow-braised meats...\"\n" +
                          "-> Dịch nghĩa: Hành phi giòn và đậu phộng rang tạo nên sự tương phản kết cấu tuyệt vời với thịt om mọng nước mềm tan.",
                TipsTitle = "⚠️ BẪY TỪ VỰNG CHUYÊN BIỆT (CULINARY READING TIPS)",
                Tips = "• Từ 'Succulent' thường dùng miêu tả thịt hoặc trái cây mọng nước, thơm ngon; đừng nhầm với nghĩa thực vật học cây mọng nước (succulent plants)!\n" +
                       "• Từ 'Palate' nghĩa là vòm họng hoặc khiếu thưởng thức vị giác của thực khách.",
                FullContent = "I. TỔNG HỢP TỪ VỰNG MIÊU TẢ HƯƠNG VỊ CAO CẤP\n" +
                              "• Mouth-watering: Ngon chảy nước miếng, hấp dẫn khó cưỡng.\n" +
                              "• Delicacy: Đặc sản quý hiếm, món ăn tinh tế.\n" +
                              "• Infused with: Được tẩm ướp ngấm sâu hương vị của..."
            },

            // =========================================================================
            // BÀI HỌC 7: ĐỌC HIỂU - TIẾNG ANH SÂN BAY & AN NINH HÀNG KHÔNG
            // =========================================================================
            [7] = new LessonDetailedContent
            {
                Objective = "Luyện đọc hiểu thông báo hàng không, bảng chỉ dẫn sân bay và quy trình kiểm tra an ninh (check-in, baggage allowance, TSA screening).",
                ReadingPassageTitle = "International Aviation Protocols: Streamlining Airport Navigation",
                ReadingPassageContent = "Navigating modern mega-airports requires strict adherence to security protocols, meticulous document preparation, and disciplined time management. Aviation authorities universally recommend that international travelers arrive at their departure terminal at least three hours prior to scheduled takeoff.\n\n" +
                                      "Upon arrival, passengers proceed directly to designated airline check-in counters or utilize self-service automated kiosks to print boarding passes and luggage tags. Ground personnel verify valid passports, destination visas, and ensure that checked baggage strictly complies with weight allowances—typically twenty-three to thirty-two kilograms depending on cabin class. Dangerous goods, including loose lithium-ion battery packs and liquids exceeding one hundred milliliters, are strictly prohibited in checked baggage and must be carried inside personal hand luggage.\n\n" +
                                      "The most critical operational phase occurs at the border immigration and security checkpoint. Travelers must remove electronic devices, laptops, and outer garments, placing them into dedicated scanning bins before stepping through full-body imaging portals. Once successfully cleared, passengers consult digital Flight Information Display Screens (FIDS) to locate their assigned departure gate and monitor boarding announcements in real time.",
                FormulaTitle = "📖 TỪ VỰNG HÀNG KHÔNG & BIỂN CHỈ DẪN SÂN BAY",
                Formula = "1. Boarding Pass: Thẻ lên máy bay (chứa số ghế, cửa ra và giờ lên máy bay)\n" +
                          "2. Baggage Allowance: Tiêu chuẩn trọng lượng hành lý miễn cước\n" +
                          "3. Carry-on Luggage: Hành lý xách tay mang lên khoang máy bay\n" +
                          "4. Customs Declaration: Tờ khai hải quan đối với hàng hóa chịu thuế\n" +
                          "5. Departure Gate: Cửa khởi hành ra máy bay",
                ExampleTitle = "💡 ĐOẠN ĐỐI CHIẾU THÔNG BÁO SÂN BAY MẪU",
                Example = "• Thông báo phát thanh:\n" +
                          "\"This is the final boarding call for passengers on flight VN384 bound for Tokyo Narita. Please proceed immediately to Gate 14 with your boarding pass and passport ready.\"\n" +
                          "-> Dịch: Đây là thông báo mời lên máy bay lần cuối cho hành khách trên chuyến bay VN384 đi Tokyo Narita. Xin quý khách vui lòng đến ngay Cửa 14 và chuẩn bị sẵn thẻ lên máy bay cùng hộ chiếu.",
                TipsTitle = "⚠️ BẪY THỜI GIAN & HÀNH LÝ TRONG BÀI THI (AVIATION TRAPS)",
                Tips = "• Phân biệt: 'Departure Time' (Giờ máy bay cất cánh) vs. 'Boarding Time' (Giờ bắt đầu cho khách lên máy bay - thường trước 30-45 phút). Đến sau giờ Boarding đóng cửa sẽ bị từ chối bay!\n" +
                       "• Bẫy trọng lượng hành lý: Đề thi thường đưa ra số cân nặng cho mỗi kiện (per piece) và tổng số kiện được phép mang theo.",
                FullContent = "I. QUY TRÌNH 5 BƯỚC TẠI SÂN BAY QUỐC TẾ\n" +
                              "1. Check-in & Ký gửi hành lý (Baggage Drop).\n" +
                              "2. Kiểm tra an ninh (Security Screening).\n" +
                              "3. Xuất cảnh qua cửa khẩu (Immigration Control).\n" +
                              "4. Phòng chờ khởi hành & Khu miễn thuế (Departure Lounge & Duty-Free).\n" +
                              "5. Lên máy bay (Boarding)."
            },

            // =========================================================================
            // BÀI HỌC 9: ĐỌC HIỂU - CHIẾN LƯỢC SKIMMING & SCANNING IELTS
            // =========================================================================
            [9] = new LessonDetailedContent
            {
                Objective = "Nắm vững hai kỹ năng sống còn trong IELTS Reading: Skimming (đọc lướt nắm ý tưởng chính toàn văn) và Scanning (rà soát từ khóa và chi tiết cụ thể) để tối ưu hóa thời gian và nâng cao độ chính xác.",
                ReadingPassageTitle = "The Ecological Resilience of Urban Micro-Forests (IELTS Academic)",
                ReadingPassageContent = "As metropolitan centers across the globe confront the escalating challenges of climate change and rapid urbanization, municipal planners are turning to nature-based urban interventions. Among the most promising innovations is the widespread adoption of the 'Miyawaki method'—a botanical planting technique engineered to cultivate dense, multi-layered native micro-forests within confined municipal boundaries.\n\n" +
                                      "Unlike conventional urban parklands, which frequently feature non-native monocultures and require substantial chemical fertilizers, micro-forests are planted using strictly indigenous botanical species. Up to thirty diverse species of native trees, shrubs, and canopy flora are densely co-planted in thoroughly conditioned soil. This intense spatial density stimulates fierce yet synergistic competition for sunlight, accelerating vegetative growth rates up to ten times faster than conventional afforestation models.\n\n" +
                                      "Empirical field investigations conducted across European and Asian capitals demonstrate substantial environmental dividends. Within three years of establishment, micro-forests absorb up to thirty times more carbon dioxide than standard grass lawns, reduce ambient localized air temperatures by 2 to 4 degrees Celsius through evapotranspiration, and mitigate urban noise pollution. Furthermore, these miniature green sanctuaries provide critical biodiversity refuges for pollinating insects, migratory birds, and subterranean microorganisms, proving that dense urban centers can coexist harmoniously with flourishing ecosystems.",
                FormulaTitle = "📖 CHIẾN LƯỢC ĐỌC HIỂU & BẢN ĐỒ TƯ DUY (READING STRATEGY)",
                Formula = "1. KỸ THUẬT SKIMMING (2 - 3 phút):\n" +
                          "   • Đọc kỹ tiêu đề bài đọc (Title) và phụ đề (Subtitle).\n" +
                          "   • Đọc câu đầu tiên (Topic Sentence) và câu kết luận của từng đoạn văn để lập sơ đồ tư duy (Mental Map).\n" +
                          "   • Chú ý các từ nối biểu thị chuyển ý: However, In contrast, Consequently, Furthermore.\n" +
                          "2. KỸ THUẬT SCANNING (Định vị từ khóa):\n" +
                          "   • Xác định từ khóa không thể thay thế (Unchangeable keywords): Tên riêng, năm tháng, con số thống kê, thuật ngữ in hoa.\n" +
                          "   • Rà soát mắt theo đường zíc-zắc từ trên xuống dưới để tìm vị trí xuất hiện từ khóa.\n" +
                          "   • Đọc kỹ 1-2 câu xung quanh từ khóa để đối chiếu với câu hỏi.",
                ExampleTitle = "💡 ĐOẠN VĂN MẪU & VÍ DỤ PHÂN TÍCH TỪ KHÓA (PARAPHRASING IN ACTION)",
                Example = "• Đoạn văn gốc trong bài IELTS:\n" +
                          "  \"Recent botanical investigations revealed that certain alpine vegetation types demonstrate unprecedented resilience to prolonged temperature anomalies.\"\n\n" +
                          "• Câu hỏi trắc nghiệm:\n" +
                          "  \"Some mountain plants are capable of surviving extreme weather variations.\"\n\n" +
                          "• Phân tích đối chiếu Paraphrase:\n" +
                          "  - botanical investigations / alpine vegetation -> mountain plants\n" +
                          "  - demonstrate unprecedented resilience -> capable of surviving\n" +
                          "  - prolonged temperature anomalies -> extreme weather variations\n" +
                          "  => Đáp án: TRUE.",
                TipsTitle = "⚠️ BẪY ĐÁNH LỪA TRONG BÀI ĐỌC HIỂU (READING TRAPS)",
                Tips = "• BẪY TỪ KHÓA GIẢ (Word-matching trap): Đừng vội chọn đáp án chỉ vì thấy có từ giống y hệt bài đọc. Giám khảo IELTS thường dùng từ giống hệt trong câu bẫy, nhưng câu đúng lại dùng từ đồng nghĩa (synonyms)!\n" +
                       "• Quản lý thời gian: Mỗi Passage chỉ nên dành tối đa 20 phút. Nếu gặp câu hỏi khó quá 1.5 phút chưa tìm ra, hãy đánh dấu và quay lại sau.",
                FullContent = "I. MA TRẬN PHÂN CHIA THỜI GIAN LÀM BÀI IELTS READING (60 PHÚT)\n" +
                              "• Passage 1 (Dễ nhất): 15 - 17 phút.\n" +
                              "• Passage 2 (Trung bình): 18 - 20 phút.\n" +
                              "• Passage 3 (Học thuật phức tạp nhất): 22 - 25 phút.\n" +
                              "• Kiểm tra và chuyển đáp án vào phiếu: 2 phút."
            },

            // =========================================================================
            // BÀI HỌC 10: ĐỌC HIỂU - TRUE / FALSE / NOT GIVEN IELTS
            // =========================================================================
            [10] = new LessonDetailedContent
            {
                Objective = "Phân biệt tuyệt đối giữa FALSE (thông tin mâu thuẫn trực tiếp) và NOT GIVEN (thông tin không đủ căn cứ để kết luận); loại bỏ hoàn toàn bẫy suy diễn chủ quan.",
                ReadingPassageTitle = "The Serendipitous Evolution of Modern Antibiotics (IELTS Academic)",
                ReadingPassageContent = "The history of modern clinical therapeutics was fundamentally altered in September 1928 at St. Mary's Hospital in London. Scottish bacteriologist Sir Alexander Fleming returned from a fortnight family vacation to discover that a batch of Staphylococcus culture dishes had inadvertently been contaminated by airborne mold spores. Curiously, the colonies of bacteria immediately adjacent to the fungal growth had completely dissolved, whereas bacterial colonies situated farther away remained entirely intact.\n\n" +
                                      "Fleming correctly identified the mold as a strain of Penicillium notatum and designated the active antibacterial substance 'penicillin.' Despite his groundbreaking observation, Fleming encountered severe biochemical obstacles in stabilizing and purifying the compound in quantities sufficient for systemic human trials. The substance remained largely an academic curiosity for over a decade until researchers Howard Florey and Ernst Chain at Oxford University developed viable biochemical isolation techniques in the early 1940s.\n\n" +
                                      "The mass industrial production of penicillin arrived just in time to treat allied casualties during the second World War, fundamentally transforming survivability rates from bacterial infections and surgical sepsis. For their collective pioneering contributions, Fleming, Florey, and Chain were jointly awarded the 1945 Nobel Prize in Physiology or Medicine. However, in his prescient Nobel banquet address, Fleming explicitly cautioned that the indiscriminate over-prescription of antibiotics would inevitably breed drug-resistant bacterial strains—a prophetic warning that mirrors today's global antimicrobial resistance crisis.",
                FormulaTitle = "📖 NGUYÊN TẮC VÀNG XÁC ĐỊNH T/F/NG (CORE RULES)",
                Formula = "• TRUE: Khẳng định trong đề bài trùng khớp 100% với nội dung và sắc thái biểu cảm của bài đọc (thường được viết lại bằng từ đồng nghĩa).\n" +
                          "• FALSE: Khẳng định trong đề bài mâu thuẫn trực tiếp, trái ngược hoàn toàn hoặc phủ định thông tin bài đọc.\n" +
                          "• NOT GIVEN: Bài đọc hoàn toàn không đề cập đến chi tiết này, HOẶC bài đọc có đề cập nhưng KHÔNG ĐỦ THÔNG TIN để chứng minh khẳng định đó là đúng hay sai.\n" +
                          "• Quy tắc vàng: 'Nếu là FALSE, bạn phải tìm được câu trong bài đọc chứng minh nó SAI. Nếu không thể chứng minh được nó SAI, thì đó là NOT GIVEN!'",
                ExampleTitle = "💡 VÍ DỤ ĐỐI CHIẾU MẪU (STEP-BY-STEP ANALYSIS)",
                Example = "1. Đề bài: \"Penicillin was discovered through deliberate and planned research.\"\n" +
                          "   -> Đáp án: FALSE (Bài đọc ghi 'inadvertently contaminated' - tình cờ bị lây nhiễm, trái ngược với 'deliberate and planned' - có chủ đích tính toán).\n\n" +
                          "2. Đề bài: \"The discovery of penicillin took place during the late 1920s.\"\n" +
                          "   -> Đáp án: TRUE (Năm 1928 thuộc cuối thập niên 1920s).\n\n" +
                          "3. Đề bài: \"Alexander Fleming became the wealthiest scientist in Great Britain after receiving his Nobel Prize.\"\n" +
                          "   -> Đáp án: NOT GIVEN (Dù bài đọc có nhắc đến giải Nobel 1945 nhưng hoàn toàn không đề cập đến tài sản hay sự giàu có của ông!).",
                TipsTitle = "⚠️ BẪY SUY DIỄN CHỦ QUAN (OVERTHINKING PITFALLS)",
                Tips = "• TUYỆT ĐỐI KHÔNG dùng kiến thức thực tế bên ngoài để làm bài T/F/NG. Chỉ căn cứ duy nhất vào những gì tác giả viết trong bài!\n" +
                       "• Chú ý các từ hạn định mức độ cực đoan: always, never, only, all, absolutely. Nếu bài đọc chỉ nói 'usually' hoặc 'some' mà câu hỏi dùng 'always' hoặc 'all' thì thường là FALSE.",
                FullContent = "I. CHIẾN THUẬT LÀM DẠNG BÀI TRUE / FALSE / NOT GIVEN\n" +
                              "1. Đọc câu nhận định trước, xác định từ khóa định vị (Names, Dates, Scientific Terms).\n" +
                              "2. Quét bài đọc tìm đúng câu chứa thông tin.\n" +
                              "3. Đối chiếu từng thành phần câu: Chủ ngữ - Động từ - Mức độ (Qualifiers)."
            },

            // =========================================================================
            // BÀI HỌC 15: NGỮ PHÁP - THÌ HIỆN TẠI HOÀN THÀNH
            // =========================================================================
            [15] = new LessonDetailedContent
            {
                Objective = "Nắm vững bản chất, công thức khẳng định, phủ định, nghi vấn và các trường hợp ứng dụng thì Hiện Tại Hoàn Thành (Present Perfect). Thành thạo phân biệt với Quá Khứ Đơn.",
                FormulaTitle = "📖 CẤU TRÚC & CÔNG THỨC NGỮ PHÁP TRỌNG TÂM (CORE FORMULAS)",
                Formula = "• Khẳng định (+): S + have / has + V3/ed\n" +
                          "  (Have: I, You, We, They, Danh từ số nhiều | Has: He, She, It, Danh từ số ít)\n" +
                          "• Phủ định (-): S + have / has + not (haven't / hasn't) + V3/ed\n" +
                          "• Nghi vấn (?): Have / Has + S + V3/ed? -> Yes, S + have/has. / No, S + haven't/hasn't.\n" +
                          "• Câu hỏi Wh- (?): Wh-word + have / has + S + V3/ed?\n" +
                          "• Dấu hiệu nhận biết điển hình:\n" +
                          "  - already (đã... rồi), yet (chưa - cuối câu phủ định/nghi vấn), just (vừa mới)\n" +
                          "  - ever (đã từng), never (chưa bao giờ), recently / lately (dạo gần đây)\n" +
                          "  - so far / up to now / up to the present (cho đến nay)\n" +
                          "  - since + mốc thời gian (since 2015, since Monday, since I was young)\n" +
                          "  - for + khoảng thời gian (for 5 years, for two weeks, for a long time)",
                ExampleTitle = "💡 CÂU VÍ DỤ MINH HỌA & PHÂN TÍCH NGỮ PHÁP CHI TIẾT",
                Example = "1. \"Dr. Miller has published over twenty scientific research papers on renewable energy.\"\n" +
                          "   -> Dịch: Tiến sĩ Miller đã công bố hơn 20 bài báo khoa học về năng lượng tái tạo (nhấn mạnh thành quả tính đến hiện tại).\n\n" +
                          "2. \"We have lived in this apartment since we moved to Ho Chi Minh City in 2021.\"\n" +
                          "   -> Dịch: Chúng tôi đã sống ở căn hộ này từ khi chuyển vào TP.HCM năm 2021 (hành động bắt đầu trong quá khứ và vẫn đang tiếp diễn).\n\n" +
                          "3. \"Have you finished reading the technical documentation yet? - No, I haven't.\"\n" +
                          "   -> Dịch: Bạn đã đọc xong tài liệu hướng dẫn kỹ thuật chưa? - Chưa, tôi vẫn chưa xong.\n\n" +
                          "4. \"She has just received an official job offer from Microsoft.\"\n" +
                          "   -> Dịch: Cô ấy vừa mới nhận được lời mời làm việc chính thức từ Microsoft.",
                TipsTitle = "⚠️ BẪY THƯỜNG GẶP & MẸO LÀM BÀI THI (EXAM TIPS & PITFALLS)",
                Tips = "• BẪY QUÁ KHỨ ĐƠN: Nếu trong câu có mốc thời gian cụ thể chấm dứt trong quá khứ (yesterday, in 2019, two days ago, last month, when I graduated) -> BẮT BUỘC dùng Quá khứ đơn (V2/ed), KHÔNG dùng Hiện tại hoàn thành!\n" +
                       "• HAVE BEEN TO vs. HAVE GONE TO:\n" +
                       "  - 'She has been to Paris': Cô ấy đã từng đến Paris và hiện đã quay về.\n" +
                       "  - 'She has gone to Paris': Cô ấy đã đi Paris và hiện vẫn đang ở Paris (chưa về)!",
                FullContent = "I. BẢN CHẤT CỦA THÌ HIỆN TẠI HOÀN THÀNH\n" +
                              "Thì Hiện tại hoàn thành (Present Perfect Tense) tạo cầu nối giữa quá khứ và hiện tại. Nó không quan tâm chính xác hành động xảy ra vào giờ nào, ngày nào, mà tập trung vào KẾT QUẢ, TRẢI NGHIỆM hoặc SỰ TIẾP DIỄN của hành động đến hiện tại.\n\n" +
                              "II. 4 TRƯỜNG HỢP SỬ DỤNG KINH ĐIỂN\n" +
                              "1. Trải nghiệm cuộc sống (Experience): Diễn tả việc đã từng hoặc chưa từng làm (Have you ever...? / I have never...).\n" +
                              "2. Hành động kéo dài từ quá khứ đến hiện tại (Duration): Đi với Since (mốc) và For (khoảng).\n" +
                              "3. Hành động vừa mới hoàn thành (Recent event): Đi với Just, Already, Yet.\n" +
                              "4. Hành động lặp đi lặp lại nhiều lần (Repetition): 'I have seen this movie four times.'"
            },

            // =========================================================================
            // BÀI HỌC 16: NGỮ PHÁP - QUÁ KHỨ ĐƠN VS QUÁ KHỨ TIẾP DIỄN
            // =========================================================================
            [16] = new LessonDetailedContent
            {
                Objective = "Phân biệt rõ ràng giữa hành động đã kết thúc trong quá khứ (Past Simple) và hành động đang diễn ra tại một thời điểm quá khứ (Past Continuous); thành thạo cấu trúc phối hợp thì với When và While.",
                FormulaTitle = "📖 CẤU TRÚC PHỐI HỢP THÌ VỚI WHEN & WHILE",
                Formula = "• Quá khứ đơn: (+) S + V2/ed | (-) S + didn't + V-bare | (?) Did + S + V-bare?\n" +
                          "• Quá khứ tiếp diễn: (+) S + was/were + V-ing | (-) S + wasn't/weren't + V-ing | (?) Was/Were + S + V-ing?\n" +
                          "• Công thức phối hợp kinh điển:\n" +
                          "  1. Hành động đang xảy ra thì hành động khác xen vào:\n" +
                          "     S + was/were + V-ing + WHEN + S + V2/ed\n" +
                          "     hoặc: WHILE + S + was/were + V-ing, S + V2/ed\n" +
                          "  2. Hai hành động diễn ra song song cùng lúc trong quá khứ:\n" +
                          "     WHILE + S + was/were + V-ing, S + was/were + V-ing",
                ExampleTitle = "💡 CÂU VÍ DỤ MINH HỌA NGỮ CẢNH CỤ THỂ",
                Example = "1. \"I was debugging the backend authentication code when the server power suddenly tripped.\"\n" +
                          "   -> Dịch: Tôi đang sửa lỗi mã nguồn xác thực máy chủ thì đột nhiên nguồn điện máy chủ bị ngắt (hành động đang code thì bị cắt ngang).\n\n" +
                          "2. \"While the marketing director was presenting the Q3 revenue, the CEO was taking meticulous notes.\"\n" +
                          "   -> Dịch: Trong khi giám đốc tiếp thị đang thuyết trình doanh thu quý 3, Tổng giám đốc đang ghi chép tỉ mỉ (2 hành động song song).\n\n" +
                          "3. \"At 9:30 PM yesterday, we were participating in an online conference call with our Tokyo partners.\"\n" +
                          "   -> Dịch: Vào lúc 9 giờ 30 tối hôm qua, chúng tôi đang tham gia cuộc gọi hội nghị trực tuyến với đối tác Tokyo (thời điểm cụ thể trong quá khứ).",
                TipsTitle = "⚠️ BẪY ĐỘNG TỪ CHỈ TRẠNG THÁI (STATIVE VERBS TRAP)",
                Tips = "• ĐỘNG TỪ TRẠNG THÁI KHÔNG DÙNG TIẾP DIỄN: Các động từ chỉ cảm giác, nhận thức, sở hữu như know, understand, believe, love, hate, want, belong KHÔNG chia ở thì Quá khứ tiếp diễn!\n" +
                       "  - ĐÚNG: 'When I was in college, I knew him very well.'\n" +
                       "  - SAI: 'When I was in college, I was knowing him very well.'",
                FullContent = "I. BẢN CHẤT KHÁC BIỆT GIỮA HAI THÌ\n" +
                              "• Quá khứ đơn (Past Simple): Nhấn mạnh sự kiện đã xảy ra và ĐÃ KẾT THÚC HOÀN TOÀN trong quá khứ.\n" +
                              "• Quá khứ tiếp diễn (Past Continuous): Nhấn mạnh TIẾN TRÌNH đang diễn ra tại một mốc thời điểm xác định trong quá khứ."
            },

            // =========================================================================
            // BÀI HỌC 26: TỪ VỰNG - THUẬT NGỮ KỸ THUẬT PHẦN MỀM & DATABASE
            // =========================================================================
            [26] = new LessonDetailedContent
            {
                Objective = "Làm chủ hệ thống thuật ngữ Kỹ thuật Phần mềm, Kiến trúc hệ thống phân tán, Cơ sở dữ liệu và Quy trình CI/CD; tự tin đọc hiểu tài liệu và làm việc trong môi trường công nghệ quốc tế.",
                FormulaTitle = "📖 BẢNG THUẬT NGỮ CHUYÊN NGÀNH IT & PHÁT ÂM IPA",
                Formula = "1. Scalability /ˌskeɪləˈbɪləti/ (n): Khả năng mở rộng chịu tải của hệ thống phần mềm\n" +
                          "2. Concurrency /kənˈkʌrənsi/ (n): Tính đồng thời, khả năng xử lý nhiều tác vụ cùng lúc\n" +
                          "3. Latency /ˈleɪtənsi/ (n): Độ trễ đường truyền hoặc thời gian phản hồi yêu cầu máy chủ\n" +
                          "4. Throughput /ˈθruːpʊt/ (n): Thông lượng, lượng tác vụ/dữ liệu xử lý thành công trong 1 giây\n" +
                          "5. Refactoring /riːˈfæktərɪŋ/ (n): Tái cấu trúc mã nguồn (cải thiện code mà không đổi chức năng)\n" +
                          "6. Asynchronous /eɪˈsɪŋkrənəs/ (adj): Bất đồng bộ (không chặn luồng thực thi chính)\n" +
                          "7. Normalization /ˌnɔːməlaɪˈzeɪʃn/ (n): Chuẩn hóa cơ sở dữ liệu (1NF, 2NF, 3NF để giảm dư thừa dữ liệu)",
                ExampleTitle = "💡 MẪU CÂU BÁO CÁO KỸ THUẬT & DỊCH NGHĨA",
                Example = "1. \"By implementing asynchronous message queues and in-memory Redis caching, we successfully reduced average API latency from 450ms to 32ms under high concurrency workloads.\"\n" +
                          "   -> Dịch: Bằng cách triển khai hàng đợi thông điệp bất đồng bộ và bộ nhớ đệm Redis, chúng tôi đã giảm thành công độ trễ API từ 450ms xuống 32ms dưới tải xử lý đồng thời cao.\n\n" +
                          "2. \"We must optimize our SQL queries with proper database indexing to eliminate full-table scans before pushing to the production environment.\"\n" +
                          "   -> Dịch: Chúng ta phải tối ưu hóa các câu truy vấn SQL bằng việc đánh chỉ mục hợp lý để loại bỏ tình trạng quét toàn bộ bảng dữ liệu trước khi đẩy lên môi trường thực tế.",
                TipsTitle = "⚠️ LƯU Ý PHÁT ÂM & THUẬT NGỮ DỄ LẪN LỘN (TECHNICAL TIPS)",
                Tips = "• Phân biệt Latency và Throughput: Latency là thời gian chờ của 1 yêu cầu (đo bằng ms); Throughput là năng lực phục vụ bao nhiêu yêu cầu trong 1 giây (đo bằng RPS hoặc QPS).\n" +
                       "• Trọng âm từ: 'Asynchronous' nhấn trọng âm vào âm tiết thứ hai: /eɪˈsɪŋkrənəs/; 'Scalability' nhấn vào âm thứ tư: /ˌskeɪləˈbɪləti/.",
                FullContent = "I. THUẬT NGỮ QUY TRÌNH PHÁT TRIỂN PHẦN MỀM HIỆN ĐẠI\n" +
                              "• CI/CD Pipeline: Continuous Integration / Continuous Deployment (Tích hợp và Triển khai liên tục).\n" +
                              "• Pull Request (PR) & Code Review: Yêu cầu gộp mã nguồn và quy trình đồng nghiệp duyệt mã.\n" +
                              "• Technical Debt: Nợ kỹ thuật (hậu quả của việc code ẩu để kịp tiến độ, phải trả giá khi bảo trì sau này)."
            }
        };

        /// <summary>
        /// Lấy nội dung chi tiết bài học đã được format chuẩn hóa và phong phú
        /// </summary>
        public static LessonDetailedContent GetDetailedContent(BaiHocDTO baiHoc)
        {
            if (baiHoc == null) return new LessonDetailedContent();

            if (_curriculumDatabase.TryGetValue(baiHoc.MaBaiHoc, out var exactMatch))
            {
                return exactMatch;
            }

            return GenerateRichFallbackContent(baiHoc);
        }

        private static LessonDetailedContent GenerateRichFallbackContent(BaiHocDTO baiHoc)
        {
            string loai = (baiHoc.LoaiBaiHoc ?? string.Empty).ToLowerInvariant();
            string tieuDe = baiHoc.TieuDe ?? "Bài học";
            string tomTat = baiHoc.TomTat ?? string.Empty;
            string capDo = string.IsNullOrEmpty(baiHoc.MaTrinhDo) ? "B1" : baiHoc.MaTrinhDo;

            var content = new LessonDetailedContent();

            if (loai.Contains("nguphap") || loai.Contains("grammar"))
            {
                content.Objective = $"Nắm vững bản chất, quy tắc ngữ pháp trọng tâm, công thức cấu trúc câu và các biến thể nâng cao của chủ đề '{tieuDe}' để áp dụng chính xác trong bài thi chuẩn hóa ({capDo}).";
                content.FormulaTitle = "📖 CẤU TRÚC & CÔNG THỨC NGỮ PHÁP TRỌNG TÂM (CORE FORMULAS)";
                content.Formula = $"• Quy tắc trọng tâm: S + Auxiliary / Modal Verb + Main Verb + Object / Complement\n" +
                                  $"• Dạng khẳng định (+): Subject + Verb Form tương ứng + Bổ ngữ\n" +
                                  $"• Dạng phủ định (-): Subject + Auxiliary + NOT + Verb nguyên mẫu/phân từ\n" +
                                  $"• Dạng nghi vấn (?): Auxiliary + Subject + Main Verb ...?\n" +
                                  $"• Lưu ý dấu hiệu nhận biết và sự phối hợp thì theo ngữ cảnh học thuật.";
                content.ExampleTitle = "💡 CÂU VÍ DỤ MINH HỌA & PHÂN TÍCH NGỮ PHÁP CHI TIẾT";
                content.Example = $"1. \"Example in Academic Context: Although the project faced significant technological constraints, the multidisciplinary engineering team delivered the product ahead of schedule.\"\n" +
                                  $"   -> Dịch: Mặc dù dự án đối mặt với nhiều rào cản công nghệ lớn, nhóm kỹ sư đa ngành đã bàn giao sản phẩm sớm hơn tiến độ dự kiến.\n\n" +
                                  $"2. \"Ngữ cảnh thực tế của bài học: {tomTat}\"";
                content.TipsTitle = "⚠️ BẪY THƯỜNG GẶP & MẸO LÀM BÀI THI (EXAM TIPS & PITFALLS)";
                content.Tips = $"• Chú ý sự hòa hợp giữa Chủ ngữ và Động từ (Subject-Verb Agreement), đặc biệt khi có các mệnh đề quan hệ chêm vào giữa.\n" +
                               $"• Cẩn thận với các động từ bất quy tắc và sự biến đổi thì trong mệnh đề phụ chỉ thời gian hoặc điều kiện.";
                content.FullContent = CleanHtml(baiHoc.NoiDung);
            }
            else if (loai.Contains("tuvung") || loai.Contains("vocab"))
            {
                content.Objective = $"Mở rộng vốn từ vựng học thuật chuyên sâu về chủ đề '{tieuDe}', phát âm chuẩn xác theo bảng phiên âm quốc tế IPA và làm chủ các cụm từ cố định (Collocations) phổ biến.";
                content.FormulaTitle = "📖 BẢNG TỪ VỰNG THEN CHỐT & PHÁT ÂM IPA (KEY VOCABULARY)";
                content.Formula = $"1. Essential Terminology /ɪˈsenʃl tɜːmɪˈnɒlədʒi/ (n): Hệ thống thuật ngữ cốt lõi\n" +
                                  $"2. Comprehensive /ˌkɒmprɪˈhensɪv/ (adj): Toàn diện, bao quát đầy đủ mọi khía cạnh\n" +
                                  $"3. Implementation /ˌɪmplɪmenˈteɪʃn/ (n): Sự triển khai thi hành kế hoạch hoặc giải pháp\n" +
                                  $"4. Substantial /səbˈstænʃl/ (adj): Đáng kể, có giá trị lớn về số lượng hoặc chất lượng\n" +
                                  $"5. Pragmatic /præɡˈmætɪk/ (adj): Thực dụng, mang tính thực tế cao";
                content.ExampleTitle = "💡 CÂU VÍ DỤ NGỮ CẢNH & CỤM TỪ (COLLOCATIONS & EXAMPLES)";
                content.Example = $"• Collocation thường gặp: Take a pragmatic approach (Áp dụng hướng tiếp cận thực tế).\n" +
                                  $"• Ví dụ câu: \"The implementation of comprehensive quality control standards resulted in a substantial decrease in operational errors.\"\n" +
                                  $"  -> Dịch: Việc triển khai các tiêu chuẩn kiểm soát chất lượng toàn diện đã giúp giảm thiểu đáng kể các lỗi vận hành.\n\n" +
                                  $"• Tóm tắt ứng dụng: {tomTat}";
                content.TipsTitle = "⚠️ BẪY TỪ DỄ NHẦM LẪN & CÁCH DÙNG TỰ NHIÊN (USAGE NOTES)";
                content.Tips = "• Không dịch từng từ (word-by-word) từ tiếng Việt sang tiếng Anh; hãy học theo cả cụm từ đi liền nhau (Collocations).\n" +
                               "• Chú ý các từ đồng nghĩa có sắc thái biểu cảm khác nhau (Connotation: tích cực, tiêu cực hoặc trung tính).";
                content.FullContent = CleanHtml(baiHoc.NoiDung);
            }
            else if (loai.Contains("nghe") || loai.Contains("listening"))
            {
                content.Objective = $"Nâng cao khả năng nhận diện âm thanh, từ khóa chuyển ý (Signposting language) và xử lý các bẫy phân tâm (Distractors) trong các đoạn hội thoại hoặc bài giảng học thuật của chủ đề '{tieuDe}'.";
                content.FormulaTitle = "🎧 BẢN THU LỜI THOẠI & KỸ THUẬT NGHE (AUDIO SCRIPT & TECHNIQUES)";
                content.Formula = $"• Kỹ thuật nghe hiểu chủ động:\n" +
                                  $"  - Dự đoán trước từ loại và thông tin cần điền (Noun, Number, Date, Adjective).\n" +
                                  $"  - Theo dõi các từ nối báo hiệu chuyển ý: 'However, actually, contrary to popular belief, in fact...'\n" +
                                  $"  - Chú ý hiện tượng nuốt âm (Elision) và nối âm (Liaison) trong giao tiếp bản ngữ.\n" +
                                  $"• Tình huống nghe bài học: {tieuDe}";
                content.ExampleTitle = "💡 MẪU CÂU ĐỐI THOẠI MINH HỌA & PHẢN XẠ";
                content.Example = $"• Lời thoại thực tế:\n" +
                                  $"  \"Speaker A: I originally planned to submit the proposal on Friday, but Professor Miller granted us an extension until Monday.\"\n" +
                                  $"  \"Speaker B: That gives us plenty of time to refine the empirical data!\"\n" +
                                  $"  -> Dịch: Người A: Ban đầu tôi dự định nộp đề xuất vào thứ Sáu, nhưng Giáo sư Miller đã gia hạn cho chúng ta đến thứ Hai. - Người B: Điều đó cho chúng ta nhiều thời gian để hoàn thiện dữ liệu thực nghiệm!\n\n" +
                                  $"• Trích dẫn bài học: {tomTat}";
                content.TipsTitle = "⚠️ BẪY ÂM NỐI & TỪ GÂY NHIỄU (LISTENING DISTRACTORS)";
                content.Tips = "• BẪY ĐỔI Ý KIẾN: Người nói thường đưa ra thông tin đầu tiên (đáp án bẫy), sau đó dùng 'Actually / But / In fact' để sửa lại đáp án chính xác ở câu tiếp theo!\n" +
                               "• Luôn tận dụng thời gian chuẩn bị trước mỗi Section để gạch chân từ khóa câu hỏi.";
                content.FullContent = CleanHtml(baiHoc.NoiDung);
            }
            else if (loai.Contains("doc") || loai.Contains("reading"))
            {
                content.Objective = $"Luyện tập các kỹ năng đọc hiểu chuyên sâu: Skimming nắm bắt cấu trúc tổng thể, Scanning định vị từ khóa và Paraphrase từ đồng nghĩa trong chủ đề '{tieuDe}'.";
                content.ReadingPassageTitle = $"Reading Passage: Contextual Analysis of {tieuDe}";
                content.ReadingPassageContent = $"Modern academic and professional paradigms demand rigorous analytical reading comprehension. In this domain, contextual investigations demonstrate that integrating comprehensive structural frameworks significantly enhances information retention and critical evaluation.\n\n" +
                                              $"Furthermore, international empirical field studies indicate that individuals who actively engage with authentic source materials demonstrate substantially superior cognitive agility. By identifying central thesis arguments, distinguishing supplementary evidence, and mapping thematic discourse markers, readers successfully navigate complex informational landscapes.\n\n" +
                                              $"Ultimately, achieving mastery in textual analysis requires consistent exposure to diverse syntactical structures and discipline-specific lexical registers, thereby establishing a robust foundation for scholarly excellence and global professional collaboration.";

                content.FormulaTitle = "📖 CHIẾN LƯỢC ĐỌC HIỂU & BẢN ĐỒ TƯ DUY (READING STRATEGY)";
                content.Formula = "• Bước 1: Đọc tiêu đề chính, phụ đề và câu đầu tiên của từng đoạn văn để lập sơ đồ tư duy.\n" +
                                  "• Bước 2: Đọc câu hỏi, khoanh tròn các từ khóa không thể thay đổi (tên riêng, số liệu, năm).\n" +
                                  "• Bước 3: Định vị đoạn văn chứa từ khóa và đọc kỹ 1-2 câu xung quanh để đối chiếu ngữ nghĩa.\n" +
                                  "• Bước 4: Nhận diện hiện tượng viết lại (Paraphrasing) giữa câu hỏi và bài đọc.";
                content.ExampleTitle = "💡 ĐOẠN VĂN MẪU & VÍ DỤ PHÂN TÍCH TỪ KHÓA";
                content.Example = $"• Nội dung trọng tâm bài đọc: \"{tomTat}\"\n\n" +
                                  $"• Mẫu câu đối chiếu học thuật:\n" +
                                  $"  - Trong bài đọc: \"Substantial empirical research indicates that regular physical exercise stimulates cognitive function.\"\n" +
                                  $"  - Trong câu hỏi: \"Working out consistently can boost brain health.\"\n" +
                                  $"  -> Phân tích: empirical research indicates ~ research shows | physical exercise ~ working out | stimulates cognitive function ~ boost brain health.";
                content.TipsTitle = "⚠️ BẪY TỪ KHÓA TRÙNG KHỚP GIẢ (WORD-MATCHING TRAP)";
                content.Tips = "• Đừng chọn đáp án chỉ vì thấy từ vựng giống hệt bài đọc. Các câu bẫy thường dùng đúng từ khóa trong bài nhưng thay đổi nghĩa ngữ pháp!\n" +
                               "• Quản lý thời gian chặt chẽ: Không dừng lại ở một câu hỏi quá 90 giây.";
                content.FullContent = CleanHtml(baiHoc.NoiDung);
            }
            else // Viết luận (Writing)
            {
                content.Objective = $"Làm chủ cấu trúc bài viết học thuật hoặc thư tín thương mại chuẩn mực cho chủ đề '{tieuDe}'; sử dụng từ nối mượt mà và phát triển luận điểm chặt chẽ.";
                content.FormulaTitle = "✍️ DÀN Ý & CẤU TRÚC BÀI VIẾT CHUẨN MỰC (ESSAY / EMAIL TEMPLATE)";
                content.Formula = "• Mở đầu (Introduction): Dẫn dắt bối cảnh + Paraphrase đề bài + Thesis Statement (nêu rõ quan điểm).\n" +
                                  "• Thân bài 1 (Body 1): Câu chủ đề (Topic Sentence) + Giải thích phân tích (Explanation) + Dẫn chứng ví dụ (Example).\n" +
                                  "• Thân bài 2 (Body 2): Luận điểm phản biện hoặc khía cạnh bổ sung + Dẫn chứng thực tế.\n" +
                                  "• Kết bài (Conclusion): Tóm tắt lại các luận điểm chính và khẳng định lại quan điểm cốt lõi.";
                content.ExampleTitle = "💡 ĐOẠN VĂN MẪU HOÀN CHỈNH & TỪ NỐI HỌC THUẬT";
                content.Example = $"• Đoạn văn mẫu triển khai luận điểm:\n" +
                                  $"\"First and foremost, investing in sustainable infrastructure yields significant economic benefits. Specifically, energy-efficient public transport networks not only diminish carbon emissions but also alleviate urban traffic congestion, thereby enhancing overall workplace productivity.\"\n\n" +
                                  $"• Trọng tâm bài học: {tomTat}";
                content.TipsTitle = "⚠️ LỖI DIỄN ĐẠT THƯỜNG GẶP & TIÊU CHÍ CHẤM ĐIỂM (WRITING CRITERIA)";
                content.Tips = "• Coherence & Cohesion: Sử dụng từ nối tự nhiên (Furthermore, Consequently, On the other hand), tránh lạm dụng từ nối ở đầu mọi câu.\n" +
                               "• Lexical Resource: Sử dụng vốn từ vựng học thuật chính xác, tránh các từ ngữ quá suồng sã hoặc từ viết tắt trong bài luận.";
                content.FullContent = CleanHtml(baiHoc.NoiDung);
            }

            return content;
        }

        /// <summary>
        /// Tạo và cung cấp bộ câu hỏi bài tập củng cố được xây dựng trực tiếp từ nội dung bài học (quy mô 5-10 câu)
        /// </summary>
        public static List<CauHoiDTO> GetPracticeQuestions(BaiHocDTO baiHoc)
        {
            if (baiHoc == null) return new List<CauHoiDTO>();

            string tieuDe = (baiHoc.TieuDe ?? string.Empty).ToLowerInvariant();
            string chuDe = (baiHoc.TenChuDe ?? string.Empty).ToLowerInvariant();
            string loai = (baiHoc.LoaiBaiHoc ?? string.Empty).ToLowerInvariant();
            int id = baiHoc.MaBaiHoc;

            List<CauHoiDTO>? selectedList = null;

            // 1. Nhận diện bộ câu hỏi theo chủ đề cốt lõi của bài học (Khớp từ khóa thông minh + mã bài học)
            if (tieuDe.Contains("hiện tại hoàn thành") || tieuDe.Contains("present perfect") || id == 14 || id == 15)
            {
                selectedList = _presentPerfectQuestions;
            }
            else if (tieuDe.Contains("quá khứ đơn") || tieuDe.Contains("quá khứ tiếp diễn") || tieuDe.Contains("past simple") || tieuDe.Contains("past continuous") || id == 15 || id == 16)
            {
                selectedList = _pastTensesQuestions;
            }
            else if (tieuDe.Contains("tương lai") || tieuDe.Contains("will") || tieuDe.Contains("going to") || id == 16 || id == 17)
            {
                selectedList = _futureTensesQuestions;
            }
            else if (tieuDe.Contains("rút gọn") || tieuDe.Contains("mệnh đề quan hệ") || tieuDe.Contains("relative clause") || id == 17 || id == 18)
            {
                selectedList = _relativeClauseQuestions;
            }
            else if (tieuDe.Contains("bị động") || tieuDe.Contains("passive") || tieuDe.Contains("truyền khiến") || id == 18 || id == 19)
            {
                selectedList = _passiveVoiceQuestions;
            }
            else if (tieuDe.Contains("điều kiện") || tieuDe.Contains("conditional") || id == 19 || id == 20)
            {
                selectedList = _conditionalQuestions;
            }
            else if (tieuDe.Contains("đảo ngữ") || tieuDe.Contains("inversion") || id == 20 || id == 21)
            {
                selectedList = _inversionQuestions;
            }
            else if (tieuDe.Contains("skimming") || tieuDe.Contains("scanning") || id == 8 || id == 9)
            {
                selectedList = _skimmingScanningQuestions;
            }
            else if (tieuDe.Contains("true") || tieuDe.Contains("false") || tieuDe.Contains("not given") || id == 9 || id == 10)
            {
                selectedList = _trueFalseNotGivenQuestions;
            }
            else if (tieuDe.Contains("chào hỏi") || tieuDe.Contains("làm quen") || tieuDe.Contains("giới thiệu bản thân") || id == 1 || id == 2)
            {
                selectedList = _greetingNetworkingQuestions;
            }
            else if (tieuDe.Contains("nhà hàng") || tieuDe.Contains("dining") || tieuDe.Contains("ẩm thực") || tieuDe.Contains("hương vị") || id == 3 || id == 5)
            {
                selectedList = _diningFoodQuestions;
            }
            else if (tieuDe.Contains("mua sắm") || tieuDe.Contains("shopping") || tieuDe.Contains("kích cỡ") || tieuDe.Contains("đổi trả") || id == 4)
            {
                selectedList = _shoppingQuestions;
            }
            else if (tieuDe.Contains("sân bay") || tieuDe.Contains("airport") || tieuDe.Contains("hành lý") || tieuDe.Contains("hỏi đường") || tieuDe.Contains("metro") || id == 6 || id == 7)
            {
                selectedList = _airportTravelQuestions;
            }
            else if (tieuDe.Contains("lập trình") || tieuDe.Contains("kỹ thuật phần mềm") || tieuDe.Contains("it") || tieuDe.Contains("software") || tieuDe.Contains("agile") || tieuDe.Contains("scrum") || id == 23 || id == 24 || id == 26)
            {
                selectedList = _itSoftwareQuestions;
            }
            else if (tieuDe.Contains("tài chính") || tieuDe.Contains("doanh nghiệp") || tieuDe.Contains("email") || tieuDe.Contains("thương mại") || tieuDe.Contains("đàm phán") || id == 21 || id == 22 || id == 25)
            {
                selectedList = _businessCommunicationQuestions;
            }
            else if (tieuDe.Contains("opinion essay") || tieuDe.Contains("problem") || tieuDe.Contains("viết luận") || tieuDe.Contains("writing") || loai.Contains("viet") || id == 10 || id == 11)
            {
                selectedList = _academicWritingQuestions;
            }
            else if (tieuDe.Contains("listening") || tieuDe.Contains("nghe") || tieuDe.Contains("section 3") || tieuDe.Contains("section 4") || loai.Contains("nghe") || id == 12 || id == 13)
            {
                selectedList = _academicListeningQuestions;
            }

            // Nếu khớp ngân hàng chuyên sâu, trả về bản sao danh sách câu hỏi
            if (selectedList != null && selectedList.Count >= 5)
            {
                return selectedList.Select(CloneQuestion).ToList();
            }

            // 2. Tự động trích xuất nội dung học tập để sinh 6 câu hỏi bám sát lý thuyết & ví dụ của bài học
            return GenerateTailoredQuestions(baiHoc);
        }

        // =========================================================================
        // NGÂN HÀNG CÂU HỎI CỦNG CỐ CHUYÊN SÂU THEO BÀI HỌC (QUY MÔ 6-8 CÂU / BÀI)
        // =========================================================================

        // 1. Thì Hiện Tại Hoàn Thành (7 câu hỏi)
        private static readonly List<CauHoiDTO> _presentPerfectQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 1401,
                NoiDungCauHoi = "Dr. Miller ______ over twenty scientific research papers on renewable energy so far.",
                DanhSachLuaChon = new List<string> { "published", "has published", "is publishing", "was published" },
                DapAnDungIndex = 1,
                GiaiThichChiTiet = "Trạng từ 'so far' (cho đến nay) là dấu hiệu nhận biết của thì Hiện Tại Hoàn Thành (S + have/has + V3/ed). Chủ ngữ 'Dr. Miller' ngôi thứ ba số ít nên dùng 'has published'."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1402,
                NoiDungCauHoi = "Which adverb of time is typically placed at the END of negative sentences and questions in the Present Perfect tense?",
                DanhSachLuaChon = new List<string> { "already", "just", "yet", "ever" },
                DapAnDungIndex = 2,
                GiaiThichChiTiet = "'Yet' thường đứng ở cuối câu phủ định (mang nghĩa 'chưa') và câu nghi vấn trong thì Hiện Tại Hoàn Thành (Ví dụ: 'Have you finished the report yet?')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1403,
                NoiDungCauHoi = "Choose the grammatically correct sentence expressing an action starting in the past and continuing to the present:",
                DanhSachLuaChon = new List<string>
                {
                    "We have lived in this apartment since five years.",
                    "We have lived in this apartment for five years.",
                    "We lived in this apartment since five years.",
                    "We are living in this apartment since 2018."
                },
                DapAnDungIndex = 1,
                GiaiThichChiTiet = "'For' đi với khoảng thời gian ('for five years'), 'Since' đi với mốc thời gian ('since 2018'). Kết hợp với thì Hiện tại hoàn thành để diễn tả hành động kéo dài đến nay."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1404,
                NoiDungCauHoi = "Why is the sentence 'She has visited the Louvre museum yesterday' grammatically incorrect?",
                DanhSachLuaChon = new List<string>
                {
                    "Because 'yesterday' indicates a finished past time and requires the Past Simple tense (visited).",
                    "Because 'Louvre' requires a different preposition of place.",
                    "Because 'has visited' cannot be used with female third-person subjects.",
                    "Because 'visited' is an irregular verb."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "BẪY BÀI THI: Khi câu có thời gian xác định kết thúc trong quá khứ ('yesterday', 'last week', 'in 2020'), bắt buộc dùng Quá khứ đơn (visited), KHÔNG dùng Hiện tại hoàn thành."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1405,
                NoiDungCauHoi = "What is the key grammatical distinction between 'have been to' and 'have gone to'?",
                DanhSachLuaChon = new List<string>
                {
                    "'Have been to' means someone visited a place and returned; 'have gone to' means they are still there.",
                    "'Have been to' is informal spoken English, whereas 'have gone to' is formal written English.",
                    "Both expressions are completely identical and interchangeable in all contexts.",
                    "'Have gone to' can only be used with first-person plural subjects."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Have been to London' = đã từng đến London và hiện đã trở về; còn 'Have gone to London' = đã đi London và hiện vẫn đang ở đó, chưa quay về."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1406,
                NoiDungCauHoi = "Complete the sentence with the correct word order: 'The cybersecurity team ______ completed the vulnerability scan.'",
                DanhSachLuaChon = new List<string> { "has just", "just has", "have just", "is just" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Trạng từ 'just' (vừa mới) đứng ở giữa trợ động từ 'have/has' và quá khứ phân từ V3. 'The team' là danh từ tập hợp số ít nên dùng 'has just completed'."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1407,
                NoiDungCauHoi = "Which question correctly uses the Present Perfect tense to inquire about lifelong experience?",
                DanhSachLuaChon = new List<string>
                {
                    "Have you ever seen the Northern Lights in Scandinavia?",
                    "Did you ever saw the Northern Lights in Scandinavia?",
                    "Were you ever seeing the Northern Lights in Scandinavia?",
                    "Do you ever seen the Northern Lights in Scandinavia?"
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Cấu trúc chuẩn hỏi trải nghiệm cuộc đời: Have/Has + S + ever + V3/ed? ('Have you ever seen...?')."
            }
        };

        // 2. Quá Khứ Đơn vs Quá Khứ Tiếp Diễn (6 câu hỏi)
        private static readonly List<CauHoiDTO> _pastTensesQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 1501,
                NoiDungCauHoi = "I was debugging the backend authentication code when the server power suddenly ______.",
                DanhSachLuaChon = new List<string> { "tripped", "was tripping", "trips", "has tripped" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Cấu trúc phối hợp thì: Một hành động đang diễn ra trong quá khứ (was debugging) thì hành động khác ngắn hơn xen vào chia ở Quá khứ đơn (tripped)."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1502,
                NoiDungCauHoi = "While Professor Davis ______ on microeconomics, the graduate students were taking notes.",
                DanhSachLuaChon = new List<string> { "lectured", "was lecturing", "is lecturing", "has lectured" },
                DapAnDungIndex = 1,
                GiaiThichChiTiet = "Hai hành động diễn ra song song cùng lúc trong quá khứ với liên từ 'While' đều được chia ở thì Quá khứ tiếp diễn (was lecturing ... were taking notes)."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1503,
                NoiDungCauHoi = "Why is the sentence 'When I was in college, I was knowing him very well' incorrect?",
                DanhSachLuaChon = new List<string>
                {
                    "Because 'know' is a stative verb that cannot be used in continuous tenses.",
                    "Because 'college' requires the future perfect tense.",
                    "Because 'when' cannot be followed by past continuous.",
                    "Because 'him' is an indirect object."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Động từ 'know' là động từ trạng thái/nhận thức (stative verb), tuyệt đối không chia ở thì tiếp diễn. Dạng đúng là: 'I knew him very well'."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1504,
                NoiDungCauHoi = "At 8:30 PM yesterday, the engineering team ______ the network migration process.",
                DanhSachLuaChon = new List<string> { "were conducting", "conducted", "have conducted", "are conducting" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Tại một thời điểm xác định cụ thể trong quá khứ ('At 8:30 PM yesterday'), ta sử dụng thì Quá khứ tiếp diễn (were conducting)."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1505,
                NoiDungCauHoi = "Which conjunction is most commonly used to introduce an ongoing background action in the continuous past?",
                DanhSachLuaChon = new List<string> { "While / As", "So that", "In order that", "Because of" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Liên từ 'While' hoặc 'As' thường mở đầu mệnh đề chỉ hành động kéo dài làm nền trong quá khứ: While + S + was/were + V-ing."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1506,
                NoiDungCauHoi = "Identify the grammatically correct sentence:",
                DanhSachLuaChon = new List<string>
                {
                    "When the fire alarm sounded, everyone immediately evacuated the building.",
                    "When the fire alarm was sounding, everyone has evacuated the building.",
                    "While the fire alarm sounded, everyone was evacuate the building.",
                    "When the fire alarm sound, everyone evacuated the building."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Hai hành động nối tiếp nhau xảy ra trong quá khứ (chuông reo -> mọi người sơ tán ngay) đều được chia ở thì Quá khứ đơn (sounded ... evacuated)."
            }
        };

        // 3. Tương Lai Đơn vs Tương Lai Gần (6 câu hỏi)
        private static readonly List<CauHoiDTO> _futureTensesQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 1601,
                NoiDungCauHoi = "The office phone is ringing. - Don't worry, I ______ it!",
                DanhSachLuaChon = new List<string> { "will answer", "am going to answer", "am answering", "answered" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Quyết định bột phát tức thời ngay tại thời điểm nói sử dụng Tương lai đơn: Will + V nguyên mẫu ('I will answer it')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1602,
                NoiDungCauHoi = "Look at those dark, ominous storm clouds gathering in the sky! It ______ rain.",
                DanhSachLuaChon = new List<string> { "is going to", "will probably", "was going to", "might have" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Dự đoán dựa trên bằng chứng thực tế rõ ràng ở hiện tại (nhìn thấy mây đen vần vũ) sử dụng 'Be going to' ('It is going to rain')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1603,
                NoiDungCauHoi = "The management board ______ a new remote work policy next month; all legal documents were finalized last Friday.",
                DanhSachLuaChon = new List<string> { "is going to launch", "will launch", "launched", "has launched" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Kế hoạch hoặc dự định đã được chuẩn bị từ trước sử dụng 'Be going to' hoặc Hiện tại tiếp diễn chỉ tương lai."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1604,
                NoiDungCauHoi = "I promise I ______ your confidential database password to anyone outside our team.",
                DanhSachLuaChon = new List<string> { "will not reveal", "am not going to reveal", "did not reveal", "have not revealed" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Lời hứa (promise), lời cam kết luôn sử dụng Tương lai đơn với 'will / will not' ('I will not reveal')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1605,
                NoiDungCauHoi = "In which of the following scenarios is 'Will' mandatory instead of 'Be going to'?",
                DanhSachLuaChon = new List<string>
                {
                    "Making a spontaneous offer or decision at the moment of speaking.",
                    "Describing a medical appointment booked three weeks ago.",
                    "Talking about a flight for which tickets are already bought.",
                    "Predicting a storm when meteorological radar shows it directly overhead."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Will' bắt buộc dùng cho các lời đề nghị (offer), lời hứa (promise) và quyết định nảy sinh bất ngờ tại thời điểm nói."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1606,
                NoiDungCauHoi = "Complete the warning: 'Be careful with that heavy ladder! You ______ drop it on the glass table!'",
                DanhSachLuaChon = new List<string> { "are going to", "will definitely", "shall", "would" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Cảnh báo một hành động sắp xảy ra ngay trước mắt dựa trên hiện tượng thực tế dùng 'are going to'."
            }
        };

        // 4. Mệnh Đề Quan Hệ Rút Gọn (6 câu hỏi)
        private static readonly List<CauHoiDTO> _relativeClauseQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 1701,
                NoiDungCauHoi = "The software architect ______ the distributed cloud system graduated from MIT with honors.",
                DanhSachLuaChon = new List<string> { "designing", "designed", "to design", "is designing" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Rút gọn mệnh đề quan hệ chủ động (who designed -> designing) bằng hiện tại phân từ V-ing."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1702,
                NoiDungCauHoi = "The empirical data ______ by the research team during the expedition was published in Nature.",
                DanhSachLuaChon = new List<string> { "collected", "collecting", "to collect", "were collected" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Rút gọn mệnh đề quan hệ bị động (which was collected -> collected) bằng quá khứ phân từ V-ed/V3."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1703,
                NoiDungCauHoi = "Dr. Katherine Johnson was the first African-American mathematician ______ orbital mechanics for NASA flights.",
                DanhSachLuaChon = new List<string> { "to calculate", "calculating", "calculated", "who calculate" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Khi danh từ đứng trước có các từ bổ nghĩa như 'the first', 'the second', 'the only', 'the last', ta rút gọn mệnh đề quan hệ bằng cụm To-Infinitive ('to calculate')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1704,
                NoiDungCauHoi = "Which sentence correctly reduces: 'The laptops which were damaged during transit will be replaced'?",
                DanhSachLuaChon = new List<string>
                {
                    "The laptops damaged during transit will be replaced.",
                    "The laptops damaging during transit will be replaced.",
                    "The laptops to damage during transit will be replaced.",
                    "The laptops having damaged during transit will be replaced."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Mệnh đề bị động 'which were damaged' rút gọn thành quá khứ phân từ 'damaged'."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1705,
                NoiDungCauHoi = "When can an active relative clause be legitimately reduced to a V-ing phrase?",
                DanhSachLuaChon = new List<string>
                {
                    "When the relative pronoun functions as the subject of the clause.",
                    "When the relative pronoun functions as an object of a preposition.",
                    "Only when the sentence is in the past perfect continuous tense.",
                    "Only when the noun is plural."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Chỉ khi đại từ quan hệ (who, which, that) đóng vai trò là CHỦ NGỮ trong mệnh đề thì mới được rút gọn thành V-ing hoặc V-ed."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1706,
                NoiDungCauHoi = "In which type of relative clause is omitting the relative pronoun strictly prohibited?",
                DanhSachLuaChon = new List<string>
                {
                    "Non-defining relative clauses (separated by commas).",
                    "Defining relative clauses where the pronoun is the object.",
                    "Clauses modifying indefinite pronouns like 'someone'.",
                    "Clauses in informal conversational English."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Trong mệnh đề quan hệ không xác định (non-defining, có dấu phẩy), tuyệt đối không được lược bỏ đại từ quan hệ."
            }
        };

        // 5. Câu Bị Động Nâng Cao & Thể Truyền Khiến (6 câu hỏi)
        private static readonly List<CauHoiDTO> _passiveVoiceQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 1801,
                NoiDungCauHoi = "We need to have our corporate tax returns ______ by a certified public accountant before April.",
                DanhSachLuaChon = new List<string> { "audited", "audit", "auditing", "to audit" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Cấu trúc truyền khiến bị động: S + have + Something + V3/ed ('have our tax returns audited')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1802,
                NoiDungCauHoi = "It is widely ______ that integrating artificial intelligence will improve clinical diagnostic accuracy.",
                DanhSachLuaChon = new List<string> { "believed", "believe", "believing", "to believe" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Cấu trúc bị động khách quan/vô nhân xưng: It + is/was + V3/ed (believed / reported / thought) + that clause."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1803,
                NoiDungCauHoi = "The project manager got the lead DevOps engineer ______ the production cluster overnight.",
                DanhSachLuaChon = new List<string> { "to upgrade", "upgraded", "upgrade", "upgrading" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Cấu trúc truyền khiến chủ động với 'get': S + get + Someone + TO Verb ('got the engineer to upgrade'). (Lưu ý: S + have + Someone + Verb nguyên thể không to)."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1804,
                NoiDungCauHoi = "Convert into passive: 'People believe that the CEO resigned yesterday.'",
                DanhSachLuaChon = new List<string>
                {
                    "The CEO is believed to have resigned yesterday.",
                    "The CEO is believed to resign yesterday.",
                    "The CEO was believed to resign yesterday.",
                    "The CEO has believed to have resigned."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Khi hành động trong mệnh đề that (resigned) xảy ra trước thời điểm ở động từ tường thuật (believe), cấu trúc chuyển đổi là: S + is/are + believed + TO HAVE V3 ('to have resigned')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1805,
                NoiDungCauHoi = "Why is the passive voice frequently favored in academic and technical literature?",
                DanhSachLuaChon = new List<string>
                {
                    "It emphasizes the methodology, experimental process, and results rather than the individual researcher.",
                    "It allows authors to write longer and more confusing sentences.",
                    "Active verbs are grammatically incorrect in research reports.",
                    "It automatically avoids Subject-Verb Agreement rules."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Trong văn phong khoa học học thuật, thể bị động được ưa chuộng nhằm tạo tính khách quan, làm nổi bật phương pháp và kết quả nghiên cứu thay vì chủ thể thực hiện."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1806,
                NoiDungCauHoi = "Choose the correct causative sentence expressing that you hired a technician to repair your air conditioner:",
                DanhSachLuaChon = new List<string>
                {
                    "I had my air conditioner repaired by a technician.",
                    "I had repaired my air conditioner by a technician.",
                    "I got my air conditioner to repair by a technician.",
                    "A technician had me repair my air conditioner."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Cấu trúc 'have something done': 'I had my air conditioner repaired' thể hiện việc thuê người khác làm dịch vụ cho mình."
            }
        };

        // 6. Câu Điều Kiện Loại 2, Loại 3 & Hỗn Hợp (6 câu hỏi)
        private static readonly List<CauHoiDTO> _conditionalQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 1901,
                NoiDungCauHoi = "If our startup ______ larger venture capital funding right now, we would hire ten senior AI engineers.",
                DanhSachLuaChon = new List<string> { "had", "have", "would have", "had had" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Câu điều kiện loại 2 giả định trái với thực tế ở hiện tại: If + S + V2/ed ('had'), S + would + V."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1902,
                NoiDungCauHoi = "If the QA team ______ the code thoroughly last Friday, the critical bug would not have leaked to production.",
                DanhSachLuaChon = new List<string> { "had tested", "tested", "was testing", "would test" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Câu điều kiện loại 3 giả định trái với quá khứ: If + S + had + V3/ed ('had tested'), S + would have + V3/ed."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1903,
                NoiDungCauHoi = "If David had completed his master's degree in computer science last year, he ______ a team lead today.",
                DanhSachLuaChon = new List<string> { "would be", "will be", "would have been", "had been" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Câu điều kiện hỗn hợp (Mixed Conditional): Giả định quá khứ (had completed) dẫn đến kết quả ở hiện tại 'today' (would be)."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1904,
                NoiDungCauHoi = "Rewrite with inversion: 'If you should require any additional financial documentation, please let us know.'",
                DanhSachLuaChon = new List<string>
                {
                    "Should you require any additional financial documentation, please let us know.",
                    "Had you require any additional financial documentation, please let us know.",
                    "Were you require any additional financial documentation, please let us know.",
                    "Did you require any additional financial documentation, please let us know."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Đảo ngữ câu điều kiện loại 1 với 'Should': Should + S + V nguyên mẫu ('Should you require...')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1905,
                NoiDungCauHoi = "Which inverted structure correctly replaces 'If the company had known about the regulation changes...'?",
                DanhSachLuaChon = new List<string>
                {
                    "Had the company known about the regulation changes...",
                    "Did the company had known about the regulation changes...",
                    "Were the company to know about the regulation changes...",
                    "Should the company know about the regulation changes..."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Đảo ngữ điều kiện loại 3: Had + S + V3/ed ('Had the company known...')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1906,
                NoiDungCauHoi = "In formal and academic English, which form of the verb 'to be' is traditionally used for all subjects in Second Conditional if-clauses?",
                DanhSachLuaChon = new List<string> { "Were (e.g. 'If I were you')", "Was", "Been", "Are" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Trong văn phong chuẩn mực học thuật, điều kiện loại 2 sử dụng 'Were' cho mọi ngôi chủ ngữ (If I were, If he were...)."
            }
        };

        // 7. Đảo Ngữ (Inversion) (6 câu hỏi)
        private static readonly List<CauHoiDTO> _inversionQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 2001,
                NoiDungCauHoi = "Seldom ______ such a breathtaking display of the aurora borealis in this southern latitude.",
                DanhSachLuaChon = new List<string> { "have we witnessed", "we have witnessed", "did we witnessed", "we witnessed" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Đảo ngữ với trạng từ mang nghĩa phủ định đứng đầu câu: Seldom + Trợ động từ + S + V chính ('Seldom have we witnessed')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2002,
                NoiDungCauHoi = "Not only ______ the annual revenue targets, but the sales department also expanded into three new markets.",
                DanhSachLuaChon = new List<string> { "did the team exceed", "the team exceeded", "has the team exceeded", "exceeded the team" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Cấu trúc đảo ngữ 'Not only': Not only + Trợ động từ (did) + S (the team) + V nguyên thể (exceed), but ... also..."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2003,
                NoiDungCauHoi = "Only after completing rigorous safety audits ______ to deploy the software into the production environment.",
                DanhSachLuaChon = new List<string> { "were the engineers allowed", "the engineers were allowed", "had the engineers allowed", "did the engineers allowed" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Đảo ngữ với 'Only after': Mệnh đề chính đứng sau đảo trợ động từ lên trước chủ ngữ ('were the engineers allowed')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2004,
                NoiDungCauHoi = "Under no circumstances ______ employees reveal customer confidential data to third parties.",
                DanhSachLuaChon = new List<string> { "should", "had", "are", "have" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Cụm từ phủ định tuyệt đối 'Under no circumstances' (dù trong bất cứ hoàn cảnh nào cũng không) + modal verb + S + V ('should employees reveal')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2005,
                NoiDungCauHoi = "Hardly had the chairman begun his keynote address ______ the fire alarm interrupted the symposium.",
                DanhSachLuaChon = new List<string> { "when", "than", "then", "after" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Cặp liên từ đảo ngữ: 'Hardly / Scarcely had + S + V3 WHEN + S + V2/ed' (Vừa mới... thì...)."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2006,
                NoiDungCauHoi = "Complete the correlative pair: 'No sooner had the flight landed ______ the passengers were instructed to remain seated.'",
                DanhSachLuaChon = new List<string> { "than", "when", "then", "as" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Cặp liên từ: 'No sooner had + S + V3 THAN + S + V2/ed' (Lưu ý: No sooner đi với THAN, còn Hardly/Scarcely đi với WHEN)."
            }
        };

        // 8. Đọc Hiểu: Skimming & Scanning (6 câu hỏi)
        private static readonly List<CauHoiDTO> _skimmingScanningQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 801,
                NoiDungCauHoi = "What is the primary operational goal of 'Skimming' in academic reading tests?",
                DanhSachLuaChon = new List<string>
                {
                    "To quickly read titles, headings, and topic sentences to grasp the overall structure and thesis within 2-3 minutes.",
                    "To memorize every statistical figure and proper noun in the text.",
                    "To translate the entire text word-by-word into Vietnamese.",
                    "To proofread the text for grammatical and spelling errors."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Skimming là kỹ thuật đọc lướt để nắm bắt bức tranh toàn cảnh (macro-structure), cấu trúc và luận điểm chính của văn bản trong thời gian ngắn."
            },
            new CauHoiDTO
            {
                MaCauHoi = 802,
                NoiDungCauHoi = "Which reading technique is best suited for locating a specific calendar year, percentage, or capitalized name?",
                DanhSachLuaChon = new List<string> { "Scanning", "Skimming", "Intensive reading", "Extensive reading" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Scanning là kỹ thuật quét mắt định vị từ khóa cụ thể không thay đổi như con số, năm tháng, tên riêng hoặc thuật ngữ in hoa."
            },
            new CauHoiDTO
            {
                MaCauHoi = 803,
                NoiDungCauHoi = "Where is the 'Topic Sentence' (câu chủ đề) most frequently located in standard academic paragraphs?",
                DanhSachLuaChon = new List<string>
                {
                    "In the first or second sentence of the paragraph.",
                    "Always buried in the exact middle of the paragraph.",
                    "Only in the footnotes at the bottom of the page.",
                    "At the very end of the final bibliography."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Trong văn bản học thuật phương Tây, câu chủ đề thường nằm ở câu đầu tiên hoặc câu thứ hai để định hướng toàn bộ đoạn văn."
            },
            new CauHoiDTO
            {
                MaCauHoi = 804,
                NoiDungCauHoi = "What does 'Paraphrasing' mean in reading comprehension exams like IELTS and TOEFL?",
                DanhSachLuaChon = new List<string>
                {
                    "Expressing the same conceptual meaning using different vocabulary and syntactic structures.",
                    "Copying the exact sentence directly from the passage without any alteration.",
                    "Changing the factual meaning to contradict the author's stance.",
                    "Removing all academic vocabulary from the sentence."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Paraphrasing là hiện tượng viết lại câu đồng nghĩa bằng từ vựng và cấu trúc ngữ pháp khác; đây là chìa khóa để nhận diện đáp án đúng."
            },
            new CauHoiDTO
            {
                MaCauHoi = 805,
                NoiDungCauHoi = "If a question asks: 'According to paragraph 4, when did the clinical trials conclude?', what is your best scanning tactic?",
                DanhSachLuaChon = new List<string>
                {
                    "Scan directly for dates, months, and 4-digit years in paragraph 4.",
                    "Read the entire passage from paragraph 1 again slowly.",
                    "Guess the answer without looking at the text.",
                    "Count the total number of words in paragraph 4."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Khi câu hỏi hỏi về mốc thời gian ('when'), hãy quét mắt tìm các con số 4 chữ số, tháng hoặc mùa xuất hiện trong đoạn văn chỉ định."
            },
            new CauHoiDTO
            {
                MaCauHoi = 806,
                NoiDungCauHoi = "Why is reading every single word slowly from beginning to end considered a major mistake in reading tests?",
                DanhSachLuaChon = new List<string>
                {
                    "It consumes excessive time and causes candidates to run out of time before finishing all questions.",
                    "Exam rules forbid reading more than 50% of the text.",
                    "Standardized texts are designed to be incomprehensible if read slowly.",
                    "Word-by-word reading automatically results in negative marking."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Đọc từng từ đơn lẻ quá chậm sẽ làm cạn kiệt thời gian 60 phút của bài thi và khiến não bộ bị quá tải chi tiết, không tổng hợp được ý chính."
            }
        };

        // 9. Đọc Hiểu: True / False / Not Given (6 câu hỏi)
        private static readonly List<CauHoiDTO> _trueFalseNotGivenQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 901,
                NoiDungCauHoi = "In IELTS Reading, when should a statement be classified as 'FALSE'?",
                DanhSachLuaChon = new List<string>
                {
                    "When the statement directly contradicts the facts stated in the passage.",
                    "When the passage does not mention the topic at all.",
                    "When the statement uses advanced academic vocabulary.",
                    "When the statement is scientifically plausible in real life."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Đáp án là FALSE khi thông tin trong câu hỏi mâu thuẫn trực tiếp hoặc trái ngược hoàn toàn với thông tin trong bài đọc."
            },
            new CauHoiDTO
            {
                MaCauHoi = 902,
                NoiDungCauHoi = "When is an answer classified as 'NOT GIVEN'?",
                DanhSachLuaChon = new List<string>
                {
                    "When the passage does not provide enough information to confirm or disprove the statement.",
                    "When the statement is grammatically incorrect.",
                    "When the author expresses personal bias.",
                    "When the passage was published more than ten years ago."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "NOT GIVEN khi bài đọc hoàn toàn không đề cập hoặc không có đủ dữ kiện để khẳng định câu đó là Đúng hay Sai."
            },
            new CauHoiDTO
            {
                MaCauHoi = 903,
                NoiDungCauHoi = "Text: 'The program attracted predominantly undergraduate students.' Statement: 'All participants in the program were undergraduate students.' -> Answer:",
                DanhSachLuaChon = new List<string> { "NOT GIVEN", "TRUE", "FALSE", "CANNOT BE DETERMINED" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Đáp án là NOT GIVEN vì 'predominantly' (phần lớn) không đồng nghĩa với 'all' (tất cả); bài đọc không khẳng định 100% sinh viên đều là cử nhân."
            },
            new CauHoiDTO
            {
                MaCauHoi = 904,
                NoiDungCauHoi = "Text: 'Solar farm installations significantly reduced energy expenses for the municipality.' Statement: 'The new solar panels increased the municipality's energy bills.' -> Answer:",
                DanhSachLuaChon = new List<string> { "FALSE", "TRUE", "NOT GIVEN", "INSUFFICIENT DATA" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Đáp án là FALSE vì 'reduced expenses' (giảm chi phí) trái ngược hoàn toàn với 'increased energy bills' (làm tăng hóa đơn)."
            },
            new CauHoiDTO
            {
                MaCauHoi = 905,
                NoiDungCauHoi = "Why are extreme qualifying words such as 'always', 'only', 'exclusively', 'never' critical in T/F/NG questions?",
                DanhSachLuaChon = new List<string>
                {
                    "Because they alter the scope of a claim and often turn a general fact into False or Not Given.",
                    "Because they are always indicators of a TRUE answer.",
                    "Because they indicate informal spoken English.",
                    "Because the computer algorithm ignores them."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Các từ chỉ mức độ tuyệt đối (always, exclusively, only) là bẫy kinh điển: nếu bài đọc chỉ nói 'thường' mà câu hỏi dùng 'luôn luôn', câu đó sẽ sai hoặc không có dữ kiện xác nhận."
            },
            new CauHoiDTO
            {
                MaCauHoi = 906,
                NoiDungCauHoi = "Statement: 'Dr. Fleming intentionally engineered the penicillin mold after months of deliberate laboratory trials.' Passage: 'Fleming returned from vacation to discover that an open culture plate had been inadvertently contaminated by fungus.' -> Answer:",
                DanhSachLuaChon = new List<string> { "FALSE", "TRUE", "NOT GIVEN", "BOTH TRUE AND FALSE" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Đáp án là FALSE: phát hiện tình cờ sau kỳ nghỉ ('inadvertently contaminated') trái ngược với việc dày công chế tạo có chủ đích ('intentionally engineered after months of trials')."
            }
        };

        // 10. Giao Tiếp: Chào Hỏi, Làm Quen & Phỏng Vấn (6 câu hỏi)
        private static readonly List<CauHoiDTO> _greetingNetworkingQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 101,
                NoiDungCauHoi = "When an international client or interviewer greets you formally with 'How do you do?', what is the standard professional reply?",
                DanhSachLuaChon = new List<string> { "How do you do?", "I am doing well, thank you.", "Never mind.", "What's going on, buddy?" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'How do you do?' là lời chào nghi thức trang trọng khi gặp gỡ đối tác lần đầu, không phải câu hỏi thăm sức khỏe. Câu đáp lễ chuẩn mực cũng là: 'How do you do?'."
            },
            new CauHoiDTO
            {
                MaCauHoi = 102,
                NoiDungCauHoi = "Which phrase is most appropriate for introducing yourself in a formal corporate setting?",
                DanhSachLuaChon = new List<string>
                {
                    "Allow me to introduce myself; my name is [Name], Lead Data Analyst at [Company].",
                    "Hey everyone, check me out, I am the new data guy.",
                    "You all probably know who I am already.",
                    "My identity is [Name] and I work here."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Allow me to introduce myself...' là mẫu câu lịch thiệp, trang trọng hàng đầu trong môi trường kinh doanh quốc tế."
            },
            new CauHoiDTO
            {
                MaCauHoi = 103,
                NoiDungCauHoi = "Which word refers to someone you know slightly from work or events, but who is not an intimate personal friend?",
                DanhSachLuaChon = new List<string> { "Acquaintance /əˈkweɪntəns/", "Sibling", "Kinship", "Comrade" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Acquaintance' là người quen xã giao, đối tác biết sơ giao, phân biệt với 'close friend' (bạn thân thiết)."
            },
            new CauHoiDTO
            {
                MaCauHoi = 104,
                NoiDungCauHoi = "What is the primary function of an 'Elevator Pitch' in career networking and conferences?",
                DanhSachLuaChon = new List<string>
                {
                    "A concise 30-60 second summary delivering your background, core strengths, and unique value proposition.",
                    "A lengthy technical paper read aloud in an elevator.",
                    "A complaint about the company's organizational elevator maintenance.",
                    "A script used exclusively for telemarketing cold calls."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Elevator Pitch (bài giới thiệu thang máy 30-60 giây) giúp bạn gây ấn tượng tức thì về chuyên môn, thành tựu và mục tiêu với nhà tuyển dụng hoặc nhà đầu tư."
            },
            new CauHoiDTO
            {
                MaCauHoi = 105,
                NoiDungCauHoi = "Which of the following topics is universally considered safe and polite for 'Small Talk' with foreign colleagues?",
                DanhSachLuaChon = new List<string>
                {
                    "The conference arrangements, city weather, and travel experiences.",
                    "Personal religious convictions and political beliefs.",
                    "Specific monthly salary figures and financial debts.",
                    "Personal relationship troubles and family conflicts."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Small talk chuyên nghiệp nên tập trung vào các đề tài trung tính, tích cực như chuyến đi, thời tiết, sự kiện hội thảo; tránh tuyệt đối tiền bạc, chính trị, tôn giáo."
            },
            new CauHoiDTO
            {
                MaCauHoi = 106,
                NoiDungCauHoi = "What does the English idiom 'to break the ice' mean in social encounters?",
                DanhSachLuaChon = new List<string>
                {
                    "To relieve tension and initiate friendly, comfortable conversation among people meeting for the first time.",
                    "To serve cold beverages with ice cubes during reception.",
                    "To express intense anger and end a business discussion abruptly.",
                    "To break delicate corporate equipment during a presentation."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Break the ice' (phá vỡ sự ngại ngùng ban đầu) là làm cho bầu không khí gặp mặt trở nên thoải mái, cởi mở và dễ trò chuyện hơn."
            }
        };

        // 11. Mua Sắm, Nhà Hàng & Ẩm Thực (6 câu hỏi)
        private static readonly List<CauHoiDTO> _diningFoodQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 301,
                NoiDungCauHoi = "When dining at a fine restaurant, how do you politely request the final check from the server?",
                DanhSachLuaChon = new List<string>
                {
                    "Could we have the bill/check, please?",
                    "Give me the payment paper right now!",
                    "How much money for this food?",
                    "Bring the invoice instantly."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Mẫu câu gọi hóa đơn chuẩn mực, lịch thiệp bằng tiếng Anh: 'Could we have the bill/check, please?'."
            },
            new CauHoiDTO
            {
                MaCauHoi = 302,
                NoiDungCauHoi = "Which expression is standard when diners wish to pay separately for their individual orders?",
                DanhSachLuaChon = new List<string>
                {
                    "Could we split the bill, please?",
                    "Can we break the cash, please?",
                    "Divide our food into halves, please.",
                    "Separate the money box, please."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Split the bill' hoặc 'Go Dutch' có nghĩa là chia đều tiền hóa đơn hoặc ai trả phần nấy."
            },
            new CauHoiDTO
            {
                MaCauHoi = 303,
                NoiDungCauHoi = "What taste characteristic does the culinary adjective 'Savory' /ˈseɪvəri/ describe?",
                DanhSachLuaChon = new List<string>
                {
                    "Pleasantly rich, salty, and seasoned rather than sweet or sugary.",
                    "Extremely bitter like burnt espresso beans.",
                    "Tasting like sour lemon and unripened fruit.",
                    "Completely flavorless and lacking any seasoning."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Savory' miêu tả món ăn mặn đậm đà, thơm ngon vừa miệng (trái ngược với vị ngọt của đồ tráng miệng)."
            },
            new CauHoiDTO
            {
                MaCauHoi = 304,
                NoiDungCauHoi = "What is the specialized term for dietary restrictions such as peanut allergies or gluten intolerance on a catering form?",
                DanhSachLuaChon = new List<string> { "Dietary requirements / restrictions", "Culinary penalties", "Eating prohibitions", "Kitchen bans" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Dietary requirements / restrictions' là mục yêu cầu ăn kiêng đặc biệt vì lý do sức khỏe, dị ứng hoặc tôn giáo."
            },
            new CauHoiDTO
            {
                MaCauHoi = 305,
                NoiDungCauHoi = "Which adjective best describes street food that is fried to a satisfying, audible crunch?",
                DanhSachLuaChon = new List<string> { "Crispy", "Soggy", "Bland", "Stale" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Crispy' là giòn rụm, giòn tan; trong khi 'soggy' là ỉu, nhũn nước."
            },
            new CauHoiDTO
            {
                MaCauHoi = 306,
                NoiDungCauHoi = "How do you ask a restaurant server for recommendations on the daily specials?",
                DanhSachLuaChon = new List<string>
                {
                    "What do you recommend for today's special?",
                    "Which food is the cheapest on your menu?",
                    "Do you even know how to cook this meal?",
                    "Tell me why I should eat here."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'What do you recommend for today's special?' là câu hỏi lịch sự nhờ nhân viên gợi ý món đặc sản trong ngày."
            }
        };

        // 12. Mua Sắm & Đổi Trả (6 câu hỏi)
        private static readonly List<CauHoiDTO> _shoppingQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 401,
                NoiDungCauHoi = "Where in a clothing store do customers go to try on garments to check if they fit?",
                DanhSachLuaChon = new List<string> { "The fitting room", "The stockroom", "The cash desk", "The security gate" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Fitting room' (hoặc dressing room) là phòng thử đồ trong các cửa hàng thời trang."
            },
            new CauHoiDTO
            {
                MaCauHoi = 402,
                NoiDungCauHoi = "If a purchased product is defective and you return it to get your money back, you are requesting a ______.",
                DanhSachLuaChon = new List<string> { "refund", "receipt", "rebate", "reward" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Refund' là tiền hoàn lại khi trả hàng; 'receipt' là hóa đơn mua hàng."
            },
            new CauHoiDTO
            {
                MaCauHoi = 403,
                NoiDungCauHoi = "What is the phrase used to ask if a garment is available in a different size?",
                DanhSachLuaChon = new List<string>
                {
                    "Do you have this in a medium / size 38?",
                    "Can you make this bigger right now?",
                    "Why is this shirt so tiny?",
                    "Give me another shirt from your truck."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Do you have this in a [size]?' là câu hỏi chuẩn để tìm cỡ áo quần vừa với mình."
            },
            new CauHoiDTO
            {
                MaCauHoi = 404,
                NoiDungCauHoi = "What document serves as physical proof of purchase required for all returns and warranty claims?",
                DanhSachLuaChon = new List<string> { "Receipt / Proof of purchase", "Price tag", "Product catalogue", "Shopping cart" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Receipt' (hóa đơn thanh toán) là bằng chứng mua hàng bắt buộc để được đổi trả hoặc bảo hành."
            },
            new CauHoiDTO
            {
                MaCauHoi = 405,
                NoiDungCauHoi = "What does the term 'Out of stock' mean?",
                DanhSachLuaChon = new List<string>
                {
                    "The item is currently unavailable in the store's inventory.",
                    "The item has been permanently discontinued by the manufacturer.",
                    "The item is being sold at a 90% discount.",
                    "The item can only be bought using cryptocurrency."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Out of stock' = hết hàng trong kho tạm thời; trái nghĩa với 'in stock' = còn hàng."
            },
            new CauHoiDTO
            {
                MaCauHoi = 406,
                NoiDungCauHoi = "Which phrase allows a customer to exchange an item for another without asking for cash back?",
                DanhSachLuaChon = new List<string>
                {
                    "I would like to exchange this for a larger size.",
                    "I demand all cash from your register.",
                    "Throw this product into the trash.",
                    "Sell this back to someone else."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Exchange this for...' = đổi sản phẩm này lấy sản phẩm khác."
            }
        };

        // 13. Sân Bay & Du Lịch (6 câu hỏi)
        private static readonly List<CauHoiDTO> _airportTravelQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 601,
                NoiDungCauHoi = "Which essential document must passengers present alongside their passport at the boarding gate?",
                DanhSachLuaChon = new List<string> { "Boarding pass", "Baggage claim tag", "Hotel voucher", "Customs declaration draft" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Boarding pass' (thẻ lên máy bay) là giấy tờ bắt buộc để qua cửa kiểm soát lên máy bay."
            },
            new CauHoiDTO
            {
                MaCauHoi = 602,
                NoiDungCauHoi = "What fee is charged when a passenger's checked suitcase exceeds the airline weight allowance?",
                DanhSachLuaChon = new List<string> { "Excess baggage fee", "Seat upgrade charge", "Runway toll", "Airport maintenance tariff" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Excess baggage fee' là phí quá cước hành lý khi cân nặng vượt mức cho phép của vé máy bay."
            },
            new CauHoiDTO
            {
                MaCauHoi = 603,
                NoiDungCauHoi = "At the security screening checkpoint, passengers must place laptops and liquids into ______ for X-ray inspection.",
                DanhSachLuaChon = new List<string> { "plastic bins / trays", "lockers", "checked luggage", "overhead bins" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Hành khách phải đặt máy tính xách tay và chất lỏng vào các khay nhựa riêng ('bins / trays') qua máy quét an ninh."
            },
            new CauHoiDTO
            {
                MaCauHoi = 604,
                NoiDungCauHoi = "Which seat selection on an airplane allows a passenger to stand up without disturbing fellow travelers?",
                DanhSachLuaChon = new List<string> { "Aisle seat", "Window seat", "Middle seat", "Cockpit seat" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Aisle seat' là ghế cạnh lối đi giữa, thuận tiện di chuyển mà không làm phiền người bên cạnh."
            },
            new CauHoiDTO
            {
                MaCauHoi = 605,
                NoiDungCauHoi = "In subway navigation, what does 'transfer at City Hall station' instruct the commuter to do?",
                DanhSachLuaChon = new List<string>
                {
                    "Change from one subway line to another train line at that station.",
                    "Exit the transit system permanently and walk.",
                    "Purchase an entirely new passport.",
                    "File a missing baggage report."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Transfer' trong giao thông công cộng nghĩa là đổi tuyến, chuyển tàu tại ga trung chuyển."
            },
            new CauHoiDTO
            {
                MaCauHoi = 606,
                NoiDungCauHoi = "Where in a transport hub or airport should a traveler inquire about a misplaced backpack?",
                DanhSachLuaChon = new List<string> { "The Lost and Found office", "The duty-free store", "The boarding bridge", "The tarmac" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Lost and Found' (phòng quản lý thất lạc hành lý) là nơi tiếp nhận và xử lý đồ đạc hành khách để quên."
            }
        };

        // 14. CNTT & Kỹ Thuật Phần Mềm (6 câu hỏi)
        private static readonly List<CauHoiDTO> _itSoftwareQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 2301,
                NoiDungCauHoi = "The capability of a software system to handle growing amounts of work or concurrent traffic gracefully is called ______.",
                DanhSachLuaChon = new List<string> { "scalability", "latency", "compilation", "deprecation" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Scalability' /ˌskeɪləˈbɪləti/ (n) là khả năng mở rộng quy mô và chịu tải của hệ thống phần mềm khi lượng người dùng tăng cao."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2302,
                NoiDungCauHoi = "Which software engineering practice involves restructuring existing code to improve internal quality without changing external behavior?",
                DanhSachLuaChon = new List<string> { "Refactoring", "Deployment", "Normalization", "Virtualization" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Refactoring' là tái cấu trúc mã nguồn để code sạch, dễ đọc và bảo trì hơn mà không làm thay đổi tính năng sẵn có."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2303,
                NoiDungCauHoi = "In networking and cloud computing, what does 'Latency' measure?",
                DanhSachLuaChon = new List<string>
                {
                    "The time delay between sending a data request and receiving the response (in milliseconds).",
                    "The maximum storage capacity of a database server.",
                    "The physical weight of the server racks.",
                    "The total number of comments written in the source code."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Latency' /ˈleɪtənsi/ (n) đo độ trễ mạng tính bằng mili-giây (ms) từ lúc gửi yêu cầu đến khi nhận kết quả."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2304,
                NoiDungCauHoi = "What is an 'API Endpoint'?",
                DanhSachLuaChon = new List<string>
                {
                    "A specific URL/URI where an API receives requests to access web services and resources.",
                    "The final day of a developer's employment contract.",
                    "The power switch on a physical server unit.",
                    "A firewall rule that permanently blocks all network traffic."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "API Endpoint là điểm cuối tiếp nhận các yêu cầu HTTP (GET, POST, PUT, DELETE) từ ứng dụng client tới máy chủ."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2305,
                NoiDungCauHoi = "What does 'Deprecation' signify in software frameworks and library development?",
                DanhSachLuaChon = new List<string>
                {
                    "Marking a feature or API as obsolete and warning developers against using it before future removal.",
                    "Upgrading the compiler to run twice as fast.",
                    "Encrypting the database passwords with SHA-256.",
                    "Translating the code comments into Spanish."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Deprecation' là việc gắn nhãn cảnh báo tính năng đã cũ (lỗi thời) và khuyến cáo không nên dùng nữa trước khi bị xóa bỏ."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2306,
                NoiDungCauHoi = "During an Agile Scrum 'Daily Standup' meeting, what are the three core questions each developer answers?",
                DanhSachLuaChon = new List<string>
                {
                    "What did I complete yesterday? What will I work on today? Are there any blockers/impediments?",
                    "How much overtime did I work? What is my salary? Who is making coffee?",
                    "When will the project be canceled? Who wrote the bugs? Why are meetings boring?",
                    "What did I eat for lunch? What time is the gym? Did anyone call IT?"
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Họp Scrum hàng ngày (Daily Standup) xoay quanh 3 câu hỏi: Đã làm gì hôm qua, sẽ làm gì hôm nay và có vướng mắc (blockers) nào cản trở không."
            }
        };

        // 15. Viết Luận Học Thuật (Academic Writing) (6 câu hỏi)
        private static readonly List<CauHoiDTO> _academicWritingQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 1001,
                NoiDungCauHoi = "In an academic essay, where should the 'Thesis Statement' (câu tuyên bố luận điểm cốt lõi) be positioned?",
                DanhSachLuaChon = new List<string>
                {
                    "At the end of the Introduction paragraph.",
                    "At the beginning of Body Paragraph 1.",
                    "In the middle of Body Paragraph 2.",
                    "In the title of the essay only."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Thesis Statement nằm ở cuối đoạn Mở bài (Introduction) để nêu bật quan điểm và định hướng toàn bộ bài viết học thuật."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1002,
                NoiDungCauHoi = "What is the primary function of a 'Topic Sentence' in an essay body paragraph?",
                DanhSachLuaChon = new List<string>
                {
                    "To state the single central argument or topic of that paragraph clearly.",
                    "To summarize the entire history of the world.",
                    "To quote a dictionary definition word-for-word.",
                    "To list all statistical numbers without analysis."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Câu chủ đề (Topic Sentence) nêu bật luận điểm trọng tâm duy nhất của đoạn thân bài trước khi phát triển dẫn chứng và giải thích."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1003,
                NoiDungCauHoi = "Which academic linking word is most effective for introducing an opposing perspective or counter-argument?",
                DanhSachLuaChon = new List<string> { "Conversely / On the contrary", "Furthermore", "In addition", "Consequently" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Conversely' hoặc 'On the contrary' (ngược lại) được dùng để dẫn nhập góc nhìn đối lập hoặc luận điểm phản biện."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1004,
                NoiDungCauHoi = "In an essay discussing solutions to urban congestion, what does the academic verb 'mitigate' mean?",
                DanhSachLuaChon = new List<string>
                {
                    "To alleviate or lessen the severity and gravity of a problem.",
                    "To completely ignore a problem until it disappears.",
                    "To exacerbate and worsen an ongoing crisis.",
                    "To finance a commercial building project."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Mitigate' /ˈmɪtɪɡeɪt/ (v) là giảm nhẹ, xoa dịu mức độ nghiêm trọng của một vấn đề (ví dụ: 'mitigate climate change / traffic congestion')."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1005,
                NoiDungCauHoi = "What should a compliant and strong academic Conclusion paragraph accomplish?",
                DanhSachLuaChon = new List<string>
                {
                    "Restate the thesis statement, summarize main arguments, and provide a closing evaluation without introducing new ideas.",
                    "Introduce completely new evidence and counter-arguments not mentioned previously.",
                    "Repeat the introduction paragraph verbatim word-for-word.",
                    "Ask the grader for sympathy and an easy passing score."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Kết luận học thuật phải tóm tắt các luận cứ chính và khẳng định lại luận điểm mà KHÔNG ĐƯỢC đưa thêm thông tin hay dẫn chứng mới chưa từng được phân tích."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1006,
                NoiDungCauHoi = "Why are informal contractions like 'don't', 'can't', 'it's' penalized in academic writing examinations?",
                DanhSachLuaChon = new List<string>
                {
                    "Because formal academic register requires full, unabbreviated word forms (e.g. 'do not', 'cannot').",
                    "Because contractions cause automated word counters to fail.",
                    "Because contractions are copyrighted by speech companies.",
                    "Because vowels cannot be omitted in English grammar."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Văn phong học thuật đòi hỏi tính trang trọng (formal register); không được dùng từ viết tắt, từ lóng hay tiếng lóng đường phố."
            }
        };

        // 16. Luyện Nghe Học Thuật (Academic Listening) (6 câu hỏi)
        private static readonly List<CauHoiDTO> _academicListeningQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 1201,
                NoiDungCauHoi = "What is a 'Distractor' in standardized listening exams like IELTS and TOEFL?",
                DanhSachLuaChon = new List<string>
                {
                    "Information in the recording designed to mislead test-takers by mentioning incorrect options before or after the true answer.",
                    "A mechanical defect in the testing center's headphones.",
                    "A sudden noise made by other candidates in the room.",
                    "A question that is marked with zero points."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Distractor (từ gây nhiễu) là bẫy âm thanh được người nói nhắc đến để làm người nghe vội vàng chọn sai trước khi đính chính lại đáp án đúng."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1202,
                NoiDungCauHoi = "When a speaker in Section 3 states: 'Actually, to be quite frank, I thought so at first but then...', what is happening?",
                DanhSachLuaChon = new List<string>
                {
                    "The speaker is rejecting their preliminary idea and revising to deliver the final valid answer.",
                    "The speaker is concluding the recording immediately.",
                    "The speaker forgot what they were talking about.",
                    "The speaker is confirming that their very first thought was correct."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "BẪY ĐỔI Ý: Cụm từ 'Actually / But / In fact / To be honest' báo hiệu người nói đang hủy bỏ thông tin đầu tiên để đưa ra đáp án chính xác."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1203,
                NoiDungCauHoi = "What is the function of 'Signposting language' in academic lectures (e.g. 'Moving on to...', 'Turning now to...')?",
                DanhSachLuaChon = new List<string>
                {
                    "Guiding listeners to recognize transitions between main topics, sub-arguments, and conclusions.",
                    "Indicating that the professor is ending the lecture early.",
                    "Showing physical road signs to the students.",
                    "Warning listeners that the upcoming question is impossible."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Signposting language' (từ chỉ dẫn chuyển ý) giúp người nghe định vị mạch bài giảng của giáo sư để kịp chuyển sang câu hỏi tiếp theo."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1204,
                NoiDungCauHoi = "Why is using the 30-second preparation interval before each audio section crucial?",
                DanhSachLuaChon = new List<string>
                {
                    "To underline keywords and predict the required grammatical part of speech (noun, number, date, adjective).",
                    "To close your eyes and rest your ears completely.",
                    "To write out full essay sentences in advance.",
                    "To ask the invigilator for translation help."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Tận dụng 30 giây chuẩn bị để gạch chân từ khóa và dự đoán trước từ loại cần điền (danh từ, số liệu, tính từ) giúp tăng 50% độ chính xác."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1205,
                NoiDungCauHoi = "In a fast-paced monologue lecture (Section 4), what should you do if you realize you missed an answer?",
                DanhSachLuaChon = new List<string>
                {
                    "Immediately focus on the upcoming questions so you do not lose the lecturer's thread.",
                    "Stop listening and dwell on the missed question for the remaining time.",
                    "Raise your hand and ask the examiner to rewind the audio recording.",
                    "Leave the examination hall immediately."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Khi đã lỡ một câu hỏi, phải lập tức bỏ qua và tập trung vào các câu tiếp theo để tránh bị mất liên tiếp cả chuỗi câu hỏi còn lại."
            },
            new CauHoiDTO
            {
                MaCauHoi = 1206,
                NoiDungCauHoi = "What does 'Elision' describe in connected native English speech?",
                DanhSachLuaChon = new List<string>
                {
                    "The omission or slurring of unstressed sounds or syllables to achieve natural fluid rhythm.",
                    "Speaking louder than necessary into a microphone.",
                    "Repeating each word three times for clarity.",
                    "Translating idioms into British slang."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Elision' (hiện tượng nuốt âm) xảy ra khi người bản ngữ nói nhanh và bỏ qua các âm yếu (như /t/, /d/) để câu nói trôi chảy hơn."
            }
        };

        // 17. Giao Tiếp Thương Mại & Báo Cáo Tài Chính (6 câu hỏi)
        private static readonly List<CauHoiDTO> _businessCommunicationQuestions = new()
        {
            new CauHoiDTO
            {
                MaCauHoi = 2101,
                NoiDungCauHoi = "In commercial procurement correspondence, what does an 'Inquiry for Quotation' request from a vendor?",
                DanhSachLuaChon = new List<string>
                {
                    "Price estimates, payment terms, minimum order quantities (MOQ), and delivery lead times.",
                    "The personal home address of the company's CEO.",
                    "A refund for products bought twenty years ago.",
                    "An invitation to a corporate Christmas party."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Thư hỏi giá (Inquiry for Quotation) đề nghị nhà cung cấp báo giá chi tiết, điều kiện thanh toán và thời gian giao hàng dự kiến."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2102,
                NoiDungCauHoi = "What metric represents a company's total earnings before interest, taxes, depreciation, and amortization?",
                DanhSachLuaChon = new List<string> { "EBITDA", "Gross Revenue", "Operating Debt", "Accounts Receivable" },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "EBITDA là chỉ số đo lường hiệu quả hoạt động kinh doanh cốt lõi trước lãi vay, thuế và khấu hao của doanh nghiệp."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2103,
                NoiDungCauHoi = "Which professional closing phrase is most suitable when writing a formal business negotiation email?",
                DanhSachLuaChon = new List<string>
                {
                    "Sincerely yours, / We look forward to your favorable response.",
                    "Catch you later, dude!",
                    "Reply fast or else.",
                    "With best kisses and hugs."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Sincerely yours' hoặc 'We look forward to your favorable response' là lời kết thư thương mại chuẩn mực, trang trọng."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2104,
                NoiDungCauHoi = "In financial forecasting, what does 'Cash Flow' measure?",
                DanhSachLuaChon = new List<string>
                {
                    "The net balance of money transferring into and out of a business over a specific financial period.",
                    "The physical paper bills stored in the office safe.",
                    "The total number of employees working in the accounting department.",
                    "The exchange rate between gold and cryptocurrency."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Cash Flow' (dòng tiền) là lượng tiền mặt thực tế luân chuyển vào và ra khỏi doanh nghiệp trong một kỳ kế toán."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2105,
                NoiDungCauHoi = "What is a 'Win-Win Negotiation' strategy?",
                DanhSachLuaChon = new List<string>
                {
                    "A collaborative negotiation approach where all participating parties achieve mutually beneficial outcomes.",
                    "A tactic where one party takes 100% of the profits and bankrupts the partner.",
                    "A coin-toss method used when contracts cannot be agreed upon.",
                    "A negotiation where both sides refuse to speak."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Đàm phán 'Đôi bên cùng có lợi' (Win-Win) hướng tới giải pháp hợp tác bền vững, cả hai bên đều đạt được mục tiêu thương mại."
            },
            new CauHoiDTO
            {
                MaCauHoi = 2106,
                NoiDungCauHoi = "What does 'Turnover' mean in business operations?",
                DanhSachLuaChon = new List<string>
                {
                    "The rate at which an organization conducts sales or the rate at which employees leave and are replaced.",
                    "Flipping a conference table upside down.",
                    "Converting a company into a nonprofit foundation.",
                    "Closing an office for weekend holidays."
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "'Turnover' trong kinh doanh có nghĩa là doanh số bán hàng (sales turnover) hoặc tỷ lệ luân chuyển/thôi việc của nhân viên (employee turnover)."
            }
        };

        /// <summary>
        /// Tự động trích xuất nội dung học tập thực tế của bài học để sinh 6 câu hỏi bám sát lý thuyết, công thức và ví dụ
        /// </summary>
        private static List<CauHoiDTO> GenerateTailoredQuestions(BaiHocDTO baiHoc)
        {
            string loai = (baiHoc.LoaiBaiHoc ?? string.Empty).ToLowerInvariant();
            string tieuDe = baiHoc.TieuDe ?? "Bài học";
            int baseId = baiHoc.MaBaiHoc * 100;

            var detailed = GetDetailedContent(baiHoc);
            var list = new List<CauHoiDTO>();

            // Câu 1: Trắc nghiệm quy tắc & công thức cốt lõi
            list.Add(new CauHoiDTO
            {
                MaCauHoi = baseId + 1,
                NoiDungCauHoi = $"Theo nội dung bài học '{tieuDe}', quy tắc cốt lõi nào sau đây phản ánh chính xác lý thuyết được trình bày?",
                DanhSachLuaChon = new List<string>
                {
                    !string.IsNullOrWhiteSpace(detailed.Formula) ? detailed.Formula.Split('\n')[0].Replace("•", "").Trim() : "Áp dụng đúng cấu trúc ngữ pháp và quy tắc phối hợp từ",
                    "Chỉ sử dụng hình thức nguyên thể và bỏ qua mọi biến đổi ngữ pháp",
                    "Đảo lộn trật tự từ mà không cần tuân thủ cấu trúc câu chuẩn",
                    "Bỏ qua mọi trạng từ và mốc thời gian trong câu"
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Lý thuyết cốt lõi của bài học yêu cầu người học nắm vững cấu trúc chuẩn và các quy tắc ngữ pháp nền tảng."
            });

            // Câu 2: Câu hỏi phân tích ngữ cảnh thực tế từ ví dụ bài học
            list.Add(new CauHoiDTO
            {
                MaCauHoi = baseId + 2,
                NoiDungCauHoi = "Trong các câu ví dụ minh họa của bài học, mục đích chính của việc phân tích ngữ cảnh là gì?",
                DanhSachLuaChon = new List<string>
                {
                    "Hiểu rõ hoàn cảnh ứng dụng tự nhiên của kiến thức trong bài thi chuẩn hóa hoặc giao tiếp công việc thực tế",
                    "Chỉ để học thuộc lòng từng chữ cái một cách máy móc",
                    "Thay thế toàn bộ việc làm bài tập thực hành",
                    "Để bỏ qua các trường hợp ngoại lệ trong ngữ pháp"
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Ví dụ thực tế giúp người học chuyển hóa lý thuyết thành phản xạ ứng dụng chuẩn xác trong bài thi và công việc."
            });

            // Câu 3: Câu hỏi về bẫy thường gặp (Pitfalls & Exam tips)
            list.Add(new CauHoiDTO
            {
                MaCauHoi = baseId + 3,
                NoiDungCauHoi = "Theo lưu ý quan trọng và bẫy bài thi được cảnh báo trong bài học, lỗi sai nào người học cần đặc biệt tránh?",
                DanhSachLuaChon = new List<string>
                {
                    "Dịch nghĩa từng từ (word-by-word) mà không chú ý đến cụm từ cố định (collocation) và sự hòa hợp thì",
                    "Đọc kỹ đề bài trước khi chọn đáp án",
                    "Ôn tập lại các động từ bất quy tắc",
                    "Kiểm tra lại đáp án trước khi bấm nộp bài"
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Phần bẫy bài thi lưu ý người học không dịch từng từ đơn lẻ mà phải xét theo cụm từ cố định và cấu trúc ngữ pháp tổng thể."
            });

            // Câu 4: Câu hỏi về ứng dụng từ vựng / thuật ngữ trong chủ đề bài học
            list.Add(new CauHoiDTO
            {
                MaCauHoi = baseId + 4,
                NoiDungCauHoi = $"Để ghi nhớ bền vững kiến thức và vốn từ vựng của chủ đề '{baiHoc.TenChuDe ?? tieuDe}', phương pháp khoa học nhất là gì?",
                DanhSachLuaChon = new List<string>
                {
                    "Học theo ngữ cảnh câu thực tế, kết hợp phiên âm IPA và làm bài tập củng cố định kỳ",
                    "Chỉ lướt qua một lần và không bao giờ ôn tập lại",
                    "Học thuộc danh sách từ vựng mà không đặt câu ví dụ",
                    "Bỏ qua phần giải thích chi tiết của các câu trả lời sai"
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Phương pháp học ngắt quãng (Spaced Repetition) kết hợp làm bài tập thực hành giúp tăng khả năng ghi nhớ dài hạn lên gấp 3 lần."
            });

            // Câu 5: Câu hỏi kiểm tra kỹ năng đọc hiểu / nghe hiểu chuyên biệt
            if (loai.Contains("doc") || loai.Contains("reading"))
            {
                list.Add(new CauHoiDTO
                {
                    MaCauHoi = baseId + 5,
                    NoiDungCauHoi = "Khi làm bài tập đọc hiểu tiếng Anh, dấu hiệu nào cho thấy một lựa chọn là đáp án bẫy (Distractor)?",
                    DanhSachLuaChon = new List<string>
                    {
                        "Lựa chọn sử dụng đúng từ khóa trong bài nhưng thay đổi hoàn toàn ý nghĩa logic hoặc chứa các từ tuyệt đối (always, exclusively)",
                        "Lựa chọn diễn đạt lại bằng từ đồng nghĩa (paraphrasing)",
                        "Lựa chọn ngắn gọn và rõ ràng",
                        "Lựa chọn có giải thích chi tiết"
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "Bẫy trùng từ (Word-matching trap) thường dùng đúng từ khóa trong văn bản nhưng bẻ cong ngữ nghĩa để đánh lừa người đọc hấp tấp."
                });
            }
            else
            {
                list.Add(new CauHoiDTO
                {
                    MaCauHoi = baseId + 5,
                    NoiDungCauHoi = "Khi hoàn thành một câu trắc nghiệm ngữ pháp tiếng Anh, bước kiểm tra nào sau đây là bắt buộc?",
                    DanhSachLuaChon = new List<string>
                    {
                        "Kiểm tra sự hòa hợp giữa Chủ ngữ - Động từ và dấu hiệu mốc thời gian của câu",
                        "Chỉ đếm số lượng từ trong câu hỏi",
                        "Chọn ngẫu nhiên đáp án dài nhất mà không đọc câu",
                        "Không cần kiểm tra lại ngữ nghĩa của câu hoàn chỉnh"
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "Luôn kiểm tra sự hòa hợp chủ vị (Subject-Verb Agreement) và các trạng từ chỉ thời gian để tránh bẫy chia thì."
                });
            }

            // Câu 6: Câu hỏi tổng kết mục tiêu bài học (hoàn thành 100%)
            list.Add(new CauHoiDTO
            {
                MaCauHoi = baseId + 6,
                NoiDungCauHoi = $"Quy định của hệ thống EnglishAI để được ghi nhận hoàn thành 100% bài học '{tieuDe}' là gì?",
                DanhSachLuaChon = new List<string>
                {
                    "Bắt buộc phải trả lời đầy đủ tất cả câu hỏi bài tập củng cố và bấm 'Nộp bài & Hoàn thành 100%'",
                    "Chỉ cần mở xem lướt qua lý thuyết rồi thoát ra ngay",
                    "Không cần làm bài tập trắc nghiệm",
                    "Chỉ cần nhấn nút quay lại danh sách bài học"
                },
                DapAnDungIndex = 0,
                GiaiThichChiTiet = "Hệ thống quy định học viên bắt buộc phải hoàn thành toàn bộ bài tập củng cố để kiểm tra mức độ hiểu bài trước khi cấp chứng nhận hoàn thành 100%."
            });

            return list;
        }

        private static CauHoiDTO CloneQuestion(CauHoiDTO original)
        {
            return new CauHoiDTO
            {
                MaCauHoi = original.MaCauHoi,
                NoiDungCauHoi = original.NoiDungCauHoi,
                DanhSachLuaChon = new List<string>(original.DanhSachLuaChon),
                DapAnDungIndex = original.DapAnDungIndex,
                GiaiThichChiTiet = original.GiaiThichChiTiet,
                LuaChonCuaNguoiDung = -1,
                DaTraLoiDung = false
            };
        }

        /// <summary>
        /// Làm sạch các thẻ HTML và định dạng thành văn bản rõ ràng, có tiêu đề và gạch đầu dòng đẹp mắt
        /// </summary>
        public static string CleanHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return "Nội dung bài giảng đang được hoàn thiện cập nhật.";

            string text = Regex.Replace(html, @"<h3>(.*?)<\/h3>", "\n\n📌 $1\n", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<h4>(.*?)<\/h4>", "\n\n🔹 $1\n", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<li>(.*?)<\/li>", "\n• $1", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<p>(.*?)<\/p>", "\n$1\n", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<br\s*\/?>", "\n", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<ol>|<\/ol>|<ul>|<\/ul>", "\n", RegexOptions.IgnoreCase);

            text = Regex.Replace(text, @"<[^>]+>", string.Empty);
            text = System.Net.WebUtility.HtmlDecode(text);
            text = Regex.Replace(text, @"\n{3,}", "\n\n");

            return text.Trim();
        }
    }
}
