using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BUS;
using BUS.AI.Configuration;
using GUI.Views.Common;
using DTO;

namespace GUI.Views
{
    public partial class BaiVietAIView : Page
    {
        private int CurrentUserId => SessionHelper.CurrentUser?.MaNguoiDung ?? 1;
        private List<BaiVietDaChamDTO> _historyList = new();

        // Current Level State
        private string _currentLevel = "B1";
        private int _targetMinWords = 120;
        private int _targetMaxWords = 180;

        // Predefined Curriculum Topics & Scaffolding Guides by CEFR Level
        private class LevelSample
        {
            public string Level { get; set; } = "B1";
            public string Genre { get; set; } = "OPINION PARAGRAPH";
            public int MinWords { get; set; } = 120;
            public int MaxWords { get; set; } = 180;
            public string Topic { get; set; } = "";
            
            // 1. Mở đoạn
            public string QuestionMoDoan { get; set; } = "";
            public string TopicSentence { get; set; } = "";
            
            // 2. Thân đoạn
            public string QuestionThanDoan { get; set; } = "";
            public string Idea1 { get; set; } = "";
            public string Idea2 { get; set; } = "";
            public string Idea3 { get; set; } = "";
            public string ExampleBody { get; set; } = "";
            public string Connectors { get; set; } = "";
            public string Vocab { get; set; } = "";
            
            // 3. Kết đoạn
            public string QuestionKetDoan { get; set; } = "";
            public string ConcludingSentence { get; set; } = "";
            
            public string DefaultEssay { get; set; } = "";
        }

        private readonly Dictionary<string, List<LevelSample>> _levelCurriculum = new()
        {
            ["A1"] = new List<LevelSample>
            {
                new LevelSample
                {
                    Level = "A1",
                    Genre = "DESCRIPTIVE (MIÊU TẢ SỞ THÍCH)",
                    MinWords = 50,
                    MaxWords = 80,
                    Topic = "Describe your favorite hobby and explain why you enjoy spending time on it.",
                    QuestionMoDoan = "❓ Câu hỏi gợi ý: Sở thích lớn nhất trong thời gian rảnh rỗi của bạn là gì? Bạn đã bắt đầu nó từ bao giờ?",
                    TopicSentence = "\"My favorite hobby in my free time is reading interesting books.\"",
                    QuestionThanDoan = "❓ Câu hỏi gợi ý: Tại sao bạn thích sở thích này? Bạn thường làm nó khi nào, ở đâu và cùng ai?",
                    Idea1 = "• Ý 1 (Lý do): Đọc sách giúp thư giãn đầu óc sau giờ học (It helps me relax after school).",
                    Idea2 = "• Ý 2 (Lợi ích): Học thêm từ vựng mới và kiến thức bổ ích (I learn many new words and fascinating facts).",
                    Idea3 = "• Ý 3 (Thói quen): Thường đọc sách cùng người thân vào buổi tối (I usually read with my brother in the evening).",
                    ExampleBody = "\"For example, yesterday I read an exciting science book about animals and learned 5 new English words.\"",
                    Connectors = "First, Also, Because, In addition, For example, Finally",
                    Vocab = "hobby, fascinating, relax, daily routine, broaden knowledge",
                    QuestionKetDoan = "❓ Câu hỏi gợi ý: Tóm lại, sở thích này mang lại cảm xúc hoặc ý nghĩa gì cho cuộc sống của bạn?",
                    ConcludingSentence = "\"In short, reading books brings me a lot of joy, knowledge, and peace every single day.\"",
                    DefaultEssay = "My favorite hobby in my free time is reading interesting books. First, it helps me relax after school because the stories are fascinating. Also, I learn many new words and useful things from reading. I often read books with my brother in the evening. In short, reading books brings me a lot of joy and peace every day."
                },
                new LevelSample
                {
                    Level = "A1",
                    Genre = "DAILY ROUTINE (THÓI QUEN HÀNG NGÀY)",
                    MinWords = 50,
                    MaxWords = 80,
                    Topic = "Describe your typical daily routine on a school day.",
                    QuestionMoDoan = "❓ Câu hỏi gợi ý: Bạn có một ngày học tập bận rộn hay thư thả? Cảm nhận chung của bạn về một ngày ở trường?",
                    TopicSentence = "\"On weekdays, I usually have a busy but productive daily schedule.\"",
                    QuestionThanDoan = "❓ Câu hỏi gợi ý: Bạn làm gì vào buổi sáng, buổi chiều và buổi tối? Hoạt động nào bạn yêu thích nhất?",
                    Idea1 = "• Ý 1 (Buổi sáng): Thức dậy lúc 6:00, vệ sinh cá nhân và ăn sáng dinh dưỡng (Wake up at 6:00 AM and eat breakfast).",
                    Idea2 = "• Ý 2 (Buổi chiều): Đi học và chơi thể thao cùng bạn bè (Attend afternoon classes and play football with classmates).",
                    Idea3 = "• Ý 3 (Buổi tối): Ăn cơm gia đình và làm bài tập về nhà (Have dinner with family and finish homework at night).",
                    ExampleBody = "\"For example, from 4:30 PM to 5:30 PM, my best friends and I play badminton in the school yard to stay healthy.\"",
                    Connectors = "First, Then, In the afternoon, At night, Finally",
                    Vocab = "schedule, productive, breakfast, concentrate, finish homework",
                    QuestionKetDoan = "❓ Câu hỏi gợi ý: Việc duy trì thói quen khoa học này giúp ích gì cho sức khỏe và tinh thần của bạn?",
                    ConcludingSentence = "\"In conclusion, following a good daily routine keeps me energetic, disciplined, and healthy.\"",
                    DefaultEssay = "On weekdays, I usually have a busy but productive schedule. First, I wake up at 6:00 AM, wash my face, and eat a healthy breakfast. Then, I go to school and attend classes until noon. In the afternoon, I play football with my classmates and do my homework at night. In conclusion, following this daily routine keeps me energetic and happy."
                }
            },
            ["A2"] = new List<LevelSample>
            {
                new LevelSample
                {
                    Level = "A2",
                    Genre = "NARRATIVE (KỂ VỀ CHUYẾN ĐI)",
                    MinWords = 80,
                    MaxWords = 120,
                    Topic = "Describe a memorable vacation or trip you took with your family or friends.",
                    QuestionMoDoan = "❓ Câu hỏi gợi ý: Chuyến đi đáng nhớ nhất của bạn diễn ra ở đâu, vào thời gian nào và cùng ai?",
                    TopicSentence = "\"Last summer, I had an unforgettable family vacation to Da Nang coastal city.\"",
                    QuestionThanDoan = "❓ Câu hỏi gợi ý: Những địa điểm nổi bật bạn đã ghé thăm? Ẩm thực hoặc trải nghiệm nào ấn tượng nhất?",
                    Idea1 = "• Ý 1: Nghỉ dưỡng gần bãi biển Mỹ Khê và tắm biển buổi sáng (Stayed near My Khe beach and swam at sunrise).",
                    Idea2 = "• Ý 2: Thưởng thức hải sản tươi ngon với giá cả hợp lý (Enjoyed delicious, fresh seafood at local restaurants).",
                    Idea3 = "• Ý 3: Đi cáp treo Bà Nà Hills và chụp nhiều ảnh kỷ niệm (Visited Ba Na Hills, took the cable car, and captured photos).",
                    ExampleBody = "\"For instance, on the second day, we tasted grilled seafood by the beach while enjoying the cool sea breeze.\"",
                    Connectors = "To begin with, On the next day, Moreover, For instance, Finally",
                    Vocab = "unforgettable, coastal city, local cuisine, cable car, scenery",
                    QuestionKetDoan = "❓ Câu hỏi gợi ý: Chuyến đi để lại cho bạn cảm xúc gì và có giúp gắn kết các thành viên không?",
                    ConcludingSentence = "\"All in all, this wonderful vacation not only helped us unwind but also strengthened our family bond.\"",
                    DefaultEssay = "Last summer, I had an unforgettable family trip to Da Nang coastal city. First, we stayed at a lovely hotel near My Khe beach, where we enjoyed swimming and watching the sunrise every morning. On the next day, we tasted delicious fresh seafood at local restaurants, which was both affordable and tasty. Moreover, we visited Ba Na Hills, rode the famous cable car, and captured many beautiful pictures together. All in all, this wonderful vacation not only helped us unwind after hard study but also strengthened our family bond."
                },
                new LevelSample
                {
                    Level = "A2",
                    Genre = "OPINION (QUAN ĐIỂM VỀ THỂ THAO)",
                    MinWords = 80,
                    MaxWords = 120,
                    Topic = "Why is regular physical exercise essential for students today?",
                    QuestionMoDoan = "❓ Câu hỏi gợi ý: Tập thể dục có vai trò quan trọng như thế nào đối với học sinh/sinh viên hiện nay?",
                    TopicSentence = "\"Regular physical exercise plays a vital role in maintaining students' health and academic performance.\"",
                    QuestionThanDoan = "❓ Câu hỏi gợi ý: Lợi ích về thể chất là gì? Lợi ích về tinh thần và giảm áp lực học tập ra sao?",
                    Idea1 = "• Ý 1 (Thể chất): Nâng cao thể lực và sức bền (It improves stamina and prevents common illnesses).",
                    Idea2 = "• Ý 2 (Tinh thần): Giảm bớt căng thẳng sau các kỳ thi (Physical activity effectively reduces stress after intense exams).",
                    Idea3 = "• Ý 3 (Học tập): Giúp não bộ tập trung và ghi nhớ tốt hơn (It enhances mental alertness and self-discipline).",
                    ExampleBody = "\"For example, students who jog or play badminton for 30 minutes every afternoon concentrate much better in evening study sessions.\"",
                    Connectors = "First, Furthermore, For example, In addition, To sum up",
                    Vocab = "vital role, stamina, alleviate stress, cognitive performance, endurance",
                    QuestionKetDoan = "❓ Câu hỏi gợi ý: Bạn có lời khuyên gì để sinh viên bắt đầu thói quen tập luyện mỗi ngày?",
                    ConcludingSentence = "\"To sum up, dedicating 30 minutes every day to sports brings lifelong physical and mental benefits.\"",
                    DefaultEssay = "Regular physical exercise plays a vital role in maintaining students' health and academic performance. First, playing sports regularly improves physical stamina and protects learners from common illnesses such as obesity or fatigue. Furthermore, engaging in outdoor activities like jogging or badminton effectively alleviates mental stress after long hours of studying. For example, students who exercise daily often concentrate better in class. To sum up, dedicating 30 minutes every day to sports brings lifelong physical and mental benefits."
                }
            },
            ["B1"] = new List<LevelSample>
            {
                new LevelSample
                {
                    Level = "B1",
                    Genre = "OPINION PARAGRAPH (LỢI ÍCH HỌC TRỰC TUYẾN)",
                    MinWords = 120,
                    MaxWords = 180,
                    Topic = "The Benefits of Learning English Online: In recent years, online English learning has become increasingly popular. What are the key advantages of this learning method?",
                    QuestionMoDoan = "❓ Câu hỏi gợi ý: Xu hướng học tiếng Anh trực tuyến hiện nay ra sao? Nhận định khái quát của bạn về phương pháp này?",
                    TopicSentence = "\"In recent years, studying English online has brought about significant advantages for modern students.\"",
                    QuestionThanDoan = "❓ Câu hỏi gợi ý: Linh hoạt về thời gian/địa điểm thế nào? Công nghệ AI hỗ trợ ra sao? Chi phí có tiết kiệm hơn không?",
                    Idea1 = "• Ý 1: Linh hoạt thời gian và địa điểm học tập (Flexible study schedule anytime, anywhere).",
                    Idea2 = "• Ý 2: Tiếp cận kho học liệu phong phú và công nghệ AI (Access to rich online resources and AI tools).",
                    Idea3 = "• Ý 3: Tiết kiệm chi phí đi lại và tự chủ tiến độ (Cost-effective and autonomous pace).",
                    ExampleBody = "\"For example, students can practice speaking with AI chatbots and review recorded grammar lectures at midnight without commuting.\"",
                    Connectors = "First and foremost, Moreover, In addition, For example, Finally, In conclusion",
                    Vocab = "flexibility, autonomous learning, interactive tools, cost-effective, self-discipline",
                    QuestionKetDoan = "❓ Câu hỏi gợi ý: Tóm lược lại, phương pháp học này giúp người học đạt được mục tiêu như thế nào?",
                    ConcludingSentence = "\"In conclusion, online English acquisition is a highly rewarding approach that empowers learners to excel at their own pace.\"",
                    DefaultEssay = "In recent years, studying English online has brought about significant advantages for modern students. First and foremost, digital learning offers unprecedented flexibility in terms of time and location. Learners can easily review video lectures, practice vocabulary, and complete assignments at their convenience without commuting. Moreover, online educational platforms provide access to rich multimedia materials and interactive AI tools that simulate real-world conversations. For example, students can practice speaking skills and receive instant grammatical feedback at any time. Finally, virtual courses tend to be more cost-effective while encouraging strong self-discipline and independent study habits. In conclusion, online English acquisition is a highly rewarding approach that empowers learners to excel at their own pace."
                },
                new LevelSample
                {
                    Level = "B1",
                    Genre = "DISCUSSION (THẢO LUẬN LÀM THÊM)",
                    MinWords = 120,
                    MaxWords = 180,
                    Topic = "Should university students work part-time jobs while studying? Discuss the advantages and potential drawbacks.",
                    QuestionMoDoan = "❓ Câu hỏi gợi ý: Việc làm thêm thời sinh viên mang lại cả cơ hội lẫn thách thức gì cho giới trẻ?",
                    TopicSentence = "\"Taking up part-time employment during university offers valuable career preparation as well as notable academic challenges.\"",
                    QuestionThanDoan = "❓ Câu hỏi gợi ý: Làm thêm giúp phát triển kỹ năng mềm nào? Kiếm thêm thu nhập ra sao? Nguy cơ ảnh hưởng học tập thế nào?",
                    Idea1 = "• Ý 1 (Ưu điểm): Tích lũy kinh nghiệm làm việc và kỹ năng mềm (Gaining practical work experience and interpersonal skills).",
                    Idea2 = "• Ý 2 (Ưu điểm): Có thu nhập trang trải sinh hoạt phí tự lập (Earning pocket money to support personal living costs).",
                    Idea3 = "• Ý 3 (Nhược điểm): Nguy cơ xao nhãng và ảnh hưởng kết quả học (Risk of burnout and diminished academic performance).",
                    ExampleBody = "\"For instance, working as an English tutor helps undergraduates hone communication skills, but working late night shifts may lead to missing morning lectures.\"",
                    Connectors = "On the one hand, Furthermore, On the other hand, Consequently, In summary",
                    Vocab = "part-time employment, soft skills, financial independence, physical burnout, strike a balance",
                    QuestionKetDoan = "❓ Câu hỏi gợi ý: Sinh viên nên làm gì để vừa có kinh nghiệm làm việc vừa đảm bảo kết quả học tập?",
                    ConcludingSentence = "\"In summary, students should carefully balance work and study to maximize benefits without sacrificing academic goals.\"",
                    DefaultEssay = "Taking up part-time employment during university offers valuable advantages as well as notable challenges. On the one hand, working alongside classes allows students to cultivate essential soft skills, such as time management, teamwork, and professional communication. Furthermore, having an independent income helps young adults cover living expenses and develop financial awareness early. On the other hand, excessive working hours can easily lead to physical exhaustion and academic negligence. Consequently, students may struggle with assignment deadlines or exam preparation. In summary, students should carefully balance work and study to maximize benefits without sacrificing academic goals."
                }
            },
            ["B2"] = new List<LevelSample>
            {
                new LevelSample
                {
                    Level = "B2",
                    Genre = "ACADEMIC ARGUMENTATIVE (TRANH LUẬN HỌC THUẬT)",
                    MinWords = 180,
                    MaxWords = 250,
                    Topic = "Some people argue that artificial intelligence and technology will completely replace human teachers in the future. To what extent do you agree or disagree?",
                    QuestionMoDoan = "❓ Câu hỏi gợi ý: Trí tuệ nhân tạo phát triển mạnh mẽ thế nào? Quan điểm dứt khoát của bạn về việc AI có thay thế được giáo viên người thật không?",
                    TopicSentence = "\"While advanced artificial intelligence accelerates pedagogical efficiency, human instructors remain fundamentally irreplaceable in nurturing emotional intelligence and moral discernment.\"",
                    QuestionThanDoan = "❓ Câu hỏi gợi ý: Điều gì mà thuật toán máy móc không bao giờ thay thế được con người? Khả năng đồng cảm, ứng biến sư phạm và khơi dậy đam mê ra sao?",
                    Idea1 = "• Ý 1: Đồng cảm xúc và cố vấn tinh thần từ giáo viên người thật (Emotional resonance, empathy, and moral mentorship that algorithms lack).",
                    Idea2 = "• Ý 2: Quản lý lớp học linh hoạt và đối thoại đạo đức thực tế (Adaptive classroom facilitation and real-world ethical deliberation).",
                    Idea3 = "• Ý 3: AI là công cụ hỗ trợ bổ trợ đắc lực chứ không thay thế hoàn toàn (AI functions as a formidable auxiliary instrument rather than a substitute).",
                    ExampleBody = "\"For instance, while an algorithm can objectively correct syntax errors, only a compassionate educator can perceive a student's hidden anxiety and offer timely encouragement.\"",
                    Connectors = "First and foremost, By contrast, Furthermore, Consequently, In light of this, In conclusion",
                    Vocab = "pedagogical efficiency, emotional resonance, moral mentorship, auxiliary instrument, critical discourse",
                    QuestionKetDoan = "❓ Câu hỏi gợi ý: Tương lai giáo dục sẽ phát triển theo mô hình cộng hưởng như thế nào giữa người thầy và AI?",
                    ConcludingSentence = "\"In conclusion, the future of education thrives not on automated replacement, but on harmonious synergy between dedicated mentors and intelligent tools.\"",
                    DefaultEssay = @"While advanced artificial intelligence accelerates pedagogical efficiency, human instructors remain fundamentally irreplaceable in nurturing emotional intelligence and moral discernment.

First and foremost, human teachers provide emotional resonance and moral mentorship that algorithms inherently lack. Education encompasses not merely information dissemination, but character development and social empathy. A compassionate educator can perceive subtle signs of demotivation or anxiety, subsequently tailoring emotional encouragement to nurture the learner's psychological well-being. By contrast, AI systems lack authentic affective consciousness, rendering them incapable of replicating genuine interpersonal mentorship.

Furthermore, dynamic classroom management and real-world ethical discourse necessitate human intuition. While AI can deliver personalized grammar drills and assess objective metrics with unprecedented precision, cultivating critical debate demands flexible moderation that accounts for nuanced cultural perspectives.

In conclusion, although artificial intelligence undoubtedly streamlines administrative duties and augments self-paced acquisition, human educators remain irreplaceable in inspiring curiosity and shaping human values."
                },
                new LevelSample
                {
                    Level = "B2",
                    Genre = "PROBLEM & SOLUTION (GIẢI PHÁP ĐÔ THỊ)",
                    MinWords = 180,
                    MaxWords = 250,
                    Topic = "Rapid urbanization has led to severe air pollution and traffic congestion in metropolitan areas. What measures can governments and individuals take to tackle these issues?",
                    QuestionMoDoan = "❓ Câu hỏi gợi ý: Sự bùng nổ đô thị hóa đã gây ra những hệ lụy môi trường nghiêm trọng nào cần giải quyết cấp bách?",
                    TopicSentence = "\"The exponential expansion of metropolitan regions has precipitated severe vehicular congestion and air quality deterioration, necessitating coordinated structural and civic countermeasures.\"",
                    QuestionThanDoan = "❓ Câu hỏi gợi ý: Chính quyền cần có chính sách hạ tầng và quy định nào? Mỗi công dân cá nhân có thể đóng góp hành động gì?",
                    Idea1 = "• Ý 1 (Nhà nước): Đầu tư mạnh vào mạng lưới xe buýt điện, tàu điện ngầm và làn đường xe đạp (Subsidizing zero-emission rapid transit and expanding urban subway networks).",
                    Idea2 = "• Ý 2 (Chính sách): Ban hành chính sách hạn chế phương tiện cá nhân và phí ùn tắc (Enforcing emission taxation and urban congestion charging).",
                    Idea3 = "• Ý 3 (Cá nhân): Nâng cao ý thức cộng đồng chuyển đổi sang lối sống xanh (Cultivating civic responsibility toward carpooling and cycling).",
                    ExampleBody = "\"For example, cities like Singapore and London have effectively reduced traffic jams by 20% through smart electronic congestion pricing combined with modern subway connectivity.\"",
                    Connectors = "To begin with, Additionally, From an individual standpoint, As a result, Ultimately",
                    Vocab = "exponential expansion, vehicular congestion, zero-emission transit, congestion pricing, sustainable coexistence",
                    QuestionKetDoan = "❓ Câu hỏi gợi ý: Giải pháp tổng thể nào sẽ bảo đảm tương lai phát triển bền vững cho các siêu đô thị?",
                    ConcludingSentence = "\"Ultimately, harmonizing municipal infrastructure enhancement with widespread civic commitment to green mobility holds the definitive remedy to sustainable urban coexistence.\"",
                    DefaultEssay = @"The exponential expansion of metropolitan regions has precipitated severe vehicular congestion and air quality deterioration, necessitating coordinated structural and civic countermeasures.

To begin with, municipal authorities must prioritize substantial investment in eco-friendly public transportation infrastructure. Subsidizing electric buses, expanding underground metro networks, and developing dedicated bicycle expressways provide citizens with reliable alternatives to fossil-fueled private vehicles. Additionally, enforcing congestion pricing zones in central business districts alongside stringent industrial emission benchmarks can effectively deter carbon-intensive commuting habits.

From an individual standpoint, citizens should actively transition toward sustainable transit practices, such as carpooling, utilizing public transportation, and adopting non-motorized travel for short commutes. Furthermore, fostering urban greening initiatives within local communities substantially helps absorb airborne pollutants.

Ultimately, harmonizing state-level infrastructure enhancement with widespread civic commitment to green mobility holds the definitive remedy to achieving sustainable urban coexistence."
                }
            }
        };

        public BaiVietAIView()
        {
            InitializeComponent();
            UpdateActiveModelBadge();
            DetectAndApplyInitialLevel();
            LoadHistoryFromDatabase();
        }

        private void UpdateActiveModelBadge()
        {
            try
            {
                string provider = AIConfigurationService.Instance.ActiveProvider.ToString();
                string model = AIConfigurationService.Instance.ActiveModel;
                txtActiveModelBadge.Text = $"🤖 {provider}: {model}";
            }
            catch
            {
                txtActiveModelBadge.Text = "🤖 AI Writing Coach";
            }
        }

        #region Level Adaptive System
        /// <summary>
        /// Tự động nhận diện trình độ học viên từ Session (Ngọc Minh: IELTS Target 5.0 => B1)
        /// </summary>
        private void DetectAndApplyInitialLevel()
        {
            string detectedLevel = "B1";
            var hoSo = SessionHelper.CurrentHoSo;

            if (hoSo != null)
            {
                if (!string.IsNullOrWhiteSpace(hoSo.TrinhDoMucTieu))
                {
                    string t = hoSo.TrinhDoMucTieu.ToUpperInvariant().Trim();
                    if (t.Contains("A1")) detectedLevel = "A1";
                    else if (t.Contains("A2")) detectedLevel = "A2";
                    else if (t.Contains("B1")) detectedLevel = "B1";
                    else if (t.Contains("B2") || t.Contains("C1") || t.Contains("C2")) detectedLevel = "B2";
                }
                else if (hoSo.DiemMucTieu.HasValue)
                {
                    decimal score = hoSo.DiemMucTieu.Value;
                    if (score >= 6.5m) detectedLevel = "B2";
                    else if (score >= 5.0m) detectedLevel = "B1";
                    else if (score >= 4.0m) detectedLevel = "A2";
                    else detectedLevel = "A1";
                }
                else if (!string.IsNullOrWhiteSpace(hoSo.TrinhDoDauVao))
                {
                    string d = hoSo.TrinhDoDauVao.ToUpperInvariant().Trim();
                    if (d.Contains("A1")) detectedLevel = "A1";
                    else if (d.Contains("A2")) detectedLevel = "A2";
                    else if (d.Contains("B1")) detectedLevel = "B1";
                    else if (d.Contains("B2") || d.Contains("C1") || d.Contains("C2")) detectedLevel = "B2";
                }
            }

            string targetDisplay = hoSo?.HienThiMucTieu ?? "Target 5.0";
            txtUserLevelBadge.Text = $"Học viên: {detectedLevel} ({targetDisplay})";

            ApplyLevel(detectedLevel, loadSampleContent: true);
        }

        private void BtnSelectLevel_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string level)
            {
                ApplyLevel(level, loadSampleContent: true);
            }
        }

        private void ApplyLevel(string level, bool loadSampleContent = true)
        {
            _currentLevel = level;

            // Cập nhật giao diện Tab Pills
            var activeStyle = (Style)FindResource("ActiveLevelTabPill");
            var normalStyle = (Style)FindResource("LevelTabPill");

            btnLevelA1.Style = (_currentLevel == "A1") ? activeStyle : normalStyle;
            btnLevelA2.Style = (_currentLevel == "A2") ? activeStyle : normalStyle;
            btnLevelB1.Style = (_currentLevel == "B1") ? activeStyle : normalStyle;
            btnLevelB2.Style = (_currentLevel == "B2") ? activeStyle : normalStyle;

            // Cập nhật thông số từ mục tiêu
            switch (_currentLevel)
            {
                case "A1":
                    _targetMinWords = 50;
                    _targetMaxWords = 80;
                    break;
                case "A2":
                    _targetMinWords = 80;
                    _targetMaxWords = 120;
                    break;
                case "B1":
                    _targetMinWords = 120;
                    _targetMaxWords = 180;
                    break;
                case "B2":
                default:
                    _targetMinWords = 180;
                    _targetMaxWords = 250;
                    break;
            }

            txtBadgeLevel.Text = $"TRÌNH ĐỘ {_currentLevel}";
            txtBadgeTargetWords.Text = $"MỤC TIÊU: {_targetMinWords}-{_targetMaxWords} TỪ";

            if (loadSampleContent && _levelCurriculum.TryGetValue(_currentLevel, out var list) && list.Count > 0)
            {
                LoadCurriculumSample(list[0]);
            }

            UpdateWordCount();
        }

        private void LoadCurriculumSample(LevelSample s)
        {
            txtBadgeGenre.Text = s.Genre;
            txtEssayTopic.Text = s.Topic;

            txtGuideQuestionMoDoan.Text = s.QuestionMoDoan;
            txtGuideTopicSentence.Text = s.TopicSentence;
            txtGuideQuestionThanDoan.Text = s.QuestionThanDoan;
            txtGuideIdea1.Text = s.Idea1;
            txtGuideIdea2.Text = s.Idea2;
            txtGuideIdea3.Text = s.Idea3;
            txtGuideExampleBody.Text = s.ExampleBody;
            txtGuideConnectors.Text = s.Connectors;
            txtGuideVocab.Text = s.Vocab;
            txtGuideQuestionKetDoan.Text = s.QuestionKetDoan;
            txtGuideConcludingSentence.Text = s.ConcludingSentence;

            txtEssayInput.Text = s.DefaultEssay;
            UpdateWordCount();

            // Reset kết quả
            pnlEmptyPlaceholder.Visibility = Visibility.Visible;
            scrollResults.Visibility = Visibility.Collapsed;
            pnlError.Visibility = Visibility.Collapsed;
            pnlLoading.Visibility = Visibility.Collapsed;
        }

        private void BtnToggleScaffolding_Click(object sender, RoutedEventArgs e)
        {
            if (pnlScaffoldingGuide.Visibility == Visibility.Visible)
            {
                pnlScaffoldingGuide.Visibility = Visibility.Collapsed;
                btnToggleScaffolding.Content = "💡 Hướng dẫn bố cục ▾";
            }
            else
            {
                pnlScaffoldingGuide.Visibility = Visibility.Visible;
                btnToggleScaffolding.Content = "💡 Hướng dẫn bố cục ▴";
            }
        }

        private async void BtnCustomAITopic_Click(object sender, RoutedEventArgs e)
        {
            btnCustomAITopic.IsEnabled = false;
            var originalContent = btnCustomAITopic.Content;
            btnCustomAITopic.Content = "⏳ Đang tạo đề...";

            try
            {
                var req = new DeBaiYeuCauDTO
                {
                    TrinhDo = _currentLevel,
                    DoDai = $"{_targetMinWords}-{_targetMaxWords} từ",
                    ChuDe = "Đời sống, Học tập hoặc Công nghệ"
                };

                var (success, deBai, err) = await BaiVietAIBUS.Instance.TaoDeBaiVietAIAsync(req);

                if (success && deBai != null)
                {
                    txtBadgeGenre.Text = $"{deBai.DangBai.ToUpperInvariant()} • {deBai.ChuDe.ToUpperInvariant()}";
                    txtEssayTopic.Text = !string.IsNullOrWhiteSpace(deBai.MoTaYeuCau) ? $"{deBai.TieuDe}: {deBai.MoTaYeuCau}" : deBai.TieuDe;

                    if (deBai.HuongDanBoCuc != null)
                    {
                        txtGuideQuestionMoDoan.Text = !string.IsNullOrWhiteSpace(deBai.HuongDanBoCuc.MoDoan?.CauHoiDinhHuong)
                            ? $"❓ Câu hỏi gợi ý: {deBai.HuongDanBoCuc.MoDoan.CauHoiDinhHuong}"
                            : "❓ Câu hỏi gợi ý: Chủ đề chính của đề bài là gì? Nhận định khái quát của bạn ra sao?";

                        txtGuideTopicSentence.Text = $"\"{deBai.HuongDanBoCuc.MoDoan?.CauMoDauGoiY ?? "In recent years, this topic has sparked meaningful discussion..."}\"";

                        txtGuideQuestionThanDoan.Text = !string.IsNullOrWhiteSpace(deBai.HuongDanBoCuc.ThanDoan?.CauHoiDinhHuong)
                            ? $"❓ Câu hỏi gợi ý: {deBai.HuongDanBoCuc.ThanDoan.CauHoiDinhHuong}"
                            : "❓ Câu hỏi gợi ý: Đâu là 2-3 lý do hoặc luận điểm chính? Có ví dụ thực tế nào chứng minh không?";

                        var ideas = deBai.HuongDanBoCuc.ThanDoan?.DanhSachYChinh;
                        txtGuideIdea1.Text = ideas != null && ideas.Count > 0 ? $"• Ý 1: {ideas[0].YChinh} ({ideas[0].CauHoiDinhHuong})" : "• Ý 1: Phân tích luận điểm đầu tiên.";
                        txtGuideIdea2.Text = ideas != null && ideas.Count > 1 ? $"• Ý 2: {ideas[1].YChinh} ({ideas[1].CauHoiDinhHuong})" : "• Ý 2: Phân tích luận điểm thứ hai.";
                        txtGuideIdea3.Text = ideas != null && ideas.Count > 2 ? $"• Ý 3: {ideas[2].YChinh} ({ideas[2].CauHoiDinhHuong})" : "• Ý 3: Nêu ví dụ thực tế hoặc giải pháp.";

                        txtGuideExampleBody.Text = !string.IsNullOrWhiteSpace(deBai.HuongDanBoCuc.ThanDoan?.ViDuMinhHoaSatDe)
                            ? $"\"{deBai.HuongDanBoCuc.ThanDoan.ViDuMinhHoaSatDe}\""
                            : "\"For example, real-world practical instances demonstrate these direct outcomes in daily life.\"";

                        if (deBai.HuongDanBoCuc.ThanDoan?.TuNoiKhuyenDung != null && deBai.HuongDanBoCuc.ThanDoan.TuNoiKhuyenDung.Count > 0)
                        {
                            txtGuideConnectors.Text = string.Join(", ", deBai.HuongDanBoCuc.ThanDoan.TuNoiKhuyenDung);
                        }

                        if (deBai.TuVungGoiY != null && deBai.TuVungGoiY.Count > 0)
                        {
                            txtGuideVocab.Text = string.Join(", ", deBai.TuVungGoiY);
                        }

                        txtGuideQuestionKetDoan.Text = !string.IsNullOrWhiteSpace(deBai.HuongDanBoCuc.KetDoan?.CauHoiDinhHuong)
                            ? $"❓ Câu hỏi gợi ý: {deBai.HuongDanBoCuc.KetDoan.CauHoiDinhHuong}"
                            : "❓ Câu hỏi gợi ý: Tóm lại, thông điệp hoặc lời khuyên then chốt bạn muốn nhấn mạnh là gì?";

                        txtGuideConcludingSentence.Text = $"\"{deBai.HuongDanBoCuc.KetDoan?.CauKetLuanGoiY ?? "In conclusion, this topic highlights important implications for learners."}\"";
                    }

                    pnlScaffoldingGuide.Visibility = Visibility.Visible;
                    btnToggleScaffolding.Content = "💡 Hướng dẫn bố cục ▴";

                    // Dọn ô nhập để sinh viên tự do thực hành
                    txtEssayInput.Text = string.Empty;
                    txtEssayInput.Focus();
                    UpdateWordCount();

                    pnlEmptyPlaceholder.Visibility = Visibility.Visible;
                    scrollResults.Visibility = Visibility.Collapsed;

                    ThongBaoDialog.ThanhCong($"AI đã thiết kế xong đề bài mới chuẩn trình độ {_currentLevel} kèm dàn ý chi tiết!", "Tạo đề thành công");
                }
                else
                {
                    ThongBaoDialog.CanhBao(err ?? "Không thể tạo đề bài bằng AI. Vui lòng kiểm tra lại kết nối API.", "Chưa tạo được đề");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.CanhBao($"Lỗi khi tạo đề: {ex.Message}", "Lỗi");
            }
            finally
            {
                btnCustomAITopic.IsEnabled = true;
                btnCustomAITopic.Content = originalContent;
            }
        }
        #endregion

        #region History Management
        private void LoadHistoryFromDatabase()
        {
            try
            {
                _historyList = BaiVietAIBUS.Instance.LayLichSuBaiViet(CurrentUserId);
                txtToggleHistoryLabel.Text = $"Lịch sử bài chấm ({_historyList.Count})";
                if (txtModalHistoryStats != null)
                {
                    txtModalHistoryStats.Text = $"Tổng cộng: {_historyList.Count} bài đã chấm";
                }
                RenderHistoryList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiVietAIView] Lỗi tải lịch sử: {ex.Message}");
            }
        }

        private void RenderHistoryList()
        {
            pnlHistoryList.Children.Clear();

            if (_historyList.Count == 0)
            {
                var emptyNotice = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(248, 250, 252)),
                    BorderBrush = (SolidColorBrush)FindResource("BorderLight"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(16, 24, 16, 24),
                    Margin = new Thickness(0, 10, 0, 0)
                };
                var emptyStack = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
                emptyStack.Children.Add(new TextBlock
                {
                    Text = "Chưa có bài viết nào trong lịch sử",
                    FontSize = 13.5,
                    FontWeight = FontWeights.Bold,
                    Foreground = (SolidColorBrush)FindResource("TextDark"),
                    HorizontalAlignment = HorizontalAlignment.Center
                });
                emptyStack.Children.Add(new TextBlock
                {
                    Text = "Các bài viết sau khi được chấm điểm bằng AI sẽ tự động lưu trữ tại đây.",
                    FontSize = 12,
                    Foreground = (SolidColorBrush)FindResource("TextGray"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 4, 0, 0)
                });
                emptyNotice.Child = emptyStack;
                pnlHistoryList.Children.Add(emptyNotice);
                return;
            }

            foreach (var item in _historyList)
            {
                var card = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = (SolidColorBrush)FindResource("BorderLight"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(14, 12, 14, 12),
                    Margin = new Thickness(0, 0, 0, 10)
                };

                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(56) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(160) });

                // Col 0: Band Score Badge
                var scoreBorder = new Border
                {
                    Background = (SolidColorBrush)FindResource("BrandTealLight"),
                    CornerRadius = new CornerRadius(8),
                    Width = 48,
                    Height = 48,
                    VerticalAlignment = VerticalAlignment.Center,
                    BorderBrush = (SolidColorBrush)FindResource("BrandTealBorder"),
                    BorderThickness = new Thickness(1)
                };
                var scoreStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
                scoreStack.Children.Add(new TextBlock
                {
                    Text = item.DiemTongQuan > 0 ? item.DiemTongQuan.ToString("0.0") : "7.0",
                    FontWeight = FontWeights.Bold,
                    FontSize = 15,
                    Foreground = (SolidColorBrush)FindResource("BrandTeal"),
                    HorizontalAlignment = HorizontalAlignment.Center
                });
                scoreStack.Children.Add(new TextBlock
                {
                    Text = "BAND",
                    FontWeight = FontWeights.Bold,
                    FontSize = 8,
                    Foreground = (SolidColorBrush)FindResource("BrandTeal"),
                    HorizontalAlignment = HorizontalAlignment.Center
                });
                scoreBorder.Child = scoreStack;
                Grid.SetColumn(scoreBorder, 0);

                // Col 1: Topic, Level & Excerpt
                var infoStack = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 12, 0)
                };

                // Meta row: Level badge + Date
                var metaWrap = new WrapPanel { Margin = new Thickness(0, 0, 0, 3) };
                if (!string.IsNullOrWhiteSpace(item.TrinhDo))
                {
                    var levelBadge = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(219, 234, 254)),
                        CornerRadius = new CornerRadius(4),
                        Padding = new Thickness(6, 1, 6, 1),
                        Margin = new Thickness(0, 0, 6, 0)
                    };
                    levelBadge.Child = new TextBlock
                    {
                        Text = $"CẤP ĐỘ {item.TrinhDo}",
                        FontSize = 9.5,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Color.FromRgb(29, 78, 216))
                    };
                    metaWrap.Children.Add(levelBadge);
                }

                metaWrap.Children.Add(new TextBlock
                {
                    Text = $"🕒 {item.NgayTao:dd/MM/yyyy HH:mm}",
                    FontSize = 10.5,
                    Foreground = (SolidColorBrush)FindResource("TextGray"),
                    VerticalAlignment = VerticalAlignment.Center
                });
                infoStack.Children.Add(metaWrap);

                // Topic title
                infoStack.Children.Add(new TextBlock
                {
                    Text = item.DeBai,
                    FontSize = 12.5,
                    FontWeight = FontWeights.Bold,
                    Foreground = (SolidColorBrush)FindResource("TextDark"),
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 3)
                });

                // Essay preview
                string preview = !string.IsNullOrWhiteSpace(item.BaiVietGoc) 
                    ? item.BaiVietGoc.Replace("\r", " ").Replace("\n", " ").Trim() 
                    : "";
                if (preview.Length > 100) preview = preview.Substring(0, 100) + "...";
                infoStack.Children.Add(new TextBlock
                {
                    Text = $"\"{preview}\"",
                    FontSize = 11,
                    FontStyle = FontStyles.Italic,
                    Foreground = (SolidColorBrush)FindResource("TextMuted"),
                    TextTrimming = TextTrimming.CharacterEllipsis
                });
                Grid.SetColumn(infoStack, 1);

                // Col 2: Action Buttons (Xem lại & Xóa)
                var actions = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center
                };

                var btnView = new Button
                {
                    Content = "📖 Xem lại",
                    Style = (Style)FindResource("SecondaryButton"),
                    Padding = new Thickness(10, 6, 10, 6),
                    FontSize = 11.5,
                    Margin = new Thickness(0, 0, 6, 0),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = item
                };
                btnView.Click += (s, e) =>
                {
                    LoadHistoryItemToUI(item);
                    modalHistory.Visibility = Visibility.Collapsed;
                    ThongBaoDialog.ThanhCong("Đã tải lại kết quả bài viết lên màn hình!", "Xem lại bài");
                };

                var btnDel = new Button
                {
                    Content = "🗑️",
                    Style = (Style)FindResource("SecondaryButton"),
                    Padding = new Thickness(8, 6, 8, 6),
                    FontSize = 11.5,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    ToolTip = "Xóa bài viết này khỏi CSDL",
                    Tag = item
                };
                btnDel.Click += (s, e) =>
                {
                    DeleteHistoryItem(item);
                };

                actions.Children.Add(btnView);
                actions.Children.Add(btnDel);
                Grid.SetColumn(actions, 2);

                grid.Children.Add(scoreBorder);
                grid.Children.Add(infoStack);
                grid.Children.Add(actions);
                card.Child = grid;

                pnlHistoryList.Children.Add(card);
            }
        }

        private void LoadHistoryItemToUI(BaiVietDaChamDTO item)
        {
            txtEssayTopic.Text = item.DeBai;
            txtEssayInput.Text = item.BaiVietGoc;

            if (!string.IsNullOrWhiteSpace(item.TrinhDo) && _levelCurriculum.ContainsKey(item.TrinhDo))
            {
                ApplyLevel(item.TrinhDo, loadSampleContent: false);
            }
            else
            {
                UpdateWordCount();
            }

            if (item.KetQuaParsed != null)
            {
                DisplayGradingResult(item.KetQuaParsed);
            }
            else
            {
                pnlEmptyPlaceholder.Visibility = Visibility.Visible;
                scrollResults.Visibility = Visibility.Collapsed;
            }
        }

        private void DeleteHistoryItem(BaiVietDaChamDTO item)
        {
            if (ThongBaoDialog.XacNhan(
                $"Bạn có chắc chắn muốn xóa bài viết đã chấm này?\n\"{item.DeBai}\"",
                "Xác nhận xóa bài viết",
                "Xóa ngay",
                "Hủy"))
            {
                BaiVietAIBUS.Instance.XoaKetQuaBaiViet(item.MaKetQuaPhanTich, CurrentUserId);
                _historyList.Remove(item);
                txtToggleHistoryLabel.Text = $"Lịch sử bài chấm ({_historyList.Count})";
                if (txtModalHistoryStats != null)
                {
                    txtModalHistoryStats.Text = $"Tổng cộng: {_historyList.Count} bài đã chấm";
                }
                RenderHistoryList();
                ThongBaoDialog.ThanhCong("Đã xóa bài viết thành công.", "Thành công");
            }
        }

        private void BtnToggleHistory_Click(object sender, RoutedEventArgs e)
        {
            LoadHistoryFromDatabase();
            modalHistory.Visibility = Visibility.Visible;
        }

        private void BtnCloseHistoryModal_Click(object sender, RoutedEventArgs e)
        {
            modalHistory.Visibility = Visibility.Collapsed;
        }

        private void BtnNewEssay_Click(object sender, RoutedEventArgs e)
        {
            txtEssayInput.Text = string.Empty;
            pnlEmptyPlaceholder.Visibility = Visibility.Visible;
            scrollResults.Visibility = Visibility.Collapsed;
            pnlError.Visibility = Visibility.Collapsed;
            txtEssayInput.Focus();
            UpdateWordCount();
        }
        #endregion

        #region Word Count & Inputs
        private void TxtEssayInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateWordCount();
        }

        private void UpdateWordCount()
        {
            if (txtWordCount == null || txtEssayInput == null) return;

            string text = txtEssayInput.Text.Trim();
            int count = 0;
            if (!string.IsNullOrEmpty(text))
            {
                var matches = Regex.Matches(text, @"[\S]+");
                count = matches.Count;
            }

            if (count >= _targetMinWords && count <= _targetMaxWords)
            {
                txtWordCount.Text = $"Số từ: {count} • Chuẩn {_currentLevel} ({_targetMinWords}–{_targetMaxWords} từ)";
                txtWordCount.Foreground = (Brush)FindResource("BrandTeal");
            }
            else if (count > _targetMaxWords)
            {
                txtWordCount.Text = $"Số từ: {count} • Vượt chuẩn ({_targetMinWords}–{_targetMaxWords} từ)";
                txtWordCount.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38)); // Red
            }
            else if (count > 0)
            {
                txtWordCount.Text = $"Số từ: {count} • Cần thêm ~{_targetMinWords - count} từ ({_targetMinWords}–{_targetMaxWords} từ)";
                txtWordCount.Foreground = new SolidColorBrush(Color.FromRgb(217, 119, 6)); // Amber
            }
            else
            {
                txtWordCount.Text = $"Số từ: 0 • Mục tiêu: {_targetMinWords}–{_targetMaxWords} từ ({_currentLevel})";
                txtWordCount.Foreground = (Brush)FindResource("TextGray");
            }
        }

        private void BtnPaste_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string text = Clipboard.GetText();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    txtEssayInput.Text = text;
                    txtEssayInput.Focus();
                    ThongBaoDialog.ThanhCong("Đã dán bài viết từ Clipboard thành công!", "Dán văn bản");
                }
                else
                {
                    ThongBaoDialog.ThongTin("Bộ nhớ tạm (Clipboard) đang trống.", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.CanhBao($"Không thể đọc Clipboard: {ex.Message}", "Lỗi");
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEssayInput.Text)) return;

            if (ThongBaoDialog.XacNhan("Bạn có chắc chắn muốn xóa toàn bộ nội dung bài viết không?", "Xác nhận làm mới", "Xóa hết", "Giữ lại"))
            {
                txtEssayInput.Text = string.Empty;
                pnlEmptyPlaceholder.Visibility = Visibility.Visible;
                scrollResults.Visibility = Visibility.Collapsed;
                UpdateWordCount();
            }
        }
        #endregion

        #region AI Essay Grading Engine
        private async void BtnGradeEssay_Click(object sender, RoutedEventArgs e)
        {
            string essay = txtEssayInput.Text.Trim();
            string topic = txtEssayTopic.Text.Trim();

            // 1. Kiểm tra bài viết rỗng
            if (string.IsNullOrWhiteSpace(essay))
            {
                ThongBaoDialog.CanhBao("Vui lòng nhập hoặc dán nội dung bài luận tiếng Anh trước khi gửi chấm.", "Chưa có bài viết");
                txtEssayInput.Focus();
                return;
            }

            // 2. Kiểm tra độ dài tối thiểu tương ứng từng trình độ
            var wordMatches = Regex.Matches(essay, @"[\S]+");
            int minAcceptable = Math.Max(20, _targetMinWords / 2);

            if (wordMatches.Count < minAcceptable)
            {
                ThongBaoDialog.CanhBao($"Bài viết quá ngắn ({wordMatches.Count} từ). Trình độ {_currentLevel} yêu cầu tối thiểu khoảng {_targetMinWords} từ. Hãy viết thêm ý theo gợi ý để AI có đủ dữ liệu đánh giá học thuật nhé!", "Bài viết quá ngắn");
                return;
            }

            // 3. Kiểm tra độ dài quá dài (tránh vượt token)
            if (wordMatches.Count > 2500)
            {
                ThongBaoDialog.CanhBao("Bài viết vượt quá giới hạn cho phép (tối đa 2500 từ). Vui lòng tóm gọn bài luận phù hợp chuẩn đoạn văn/bài thi.", "Bài viết quá dài");
                return;
            }

            // Bật trạng thái Loading
            pnlEmptyPlaceholder.Visibility = Visibility.Collapsed;
            scrollResults.Visibility = Visibility.Collapsed;
            pnlError.Visibility = Visibility.Collapsed;
            pnlLoading.Visibility = Visibility.Visible;
            btnGradeEssay.IsEnabled = false;

            UpdateActiveModelBadge();

            try
            {
                string soTuYeuCau = $"{_targetMinWords}-{_targetMaxWords} từ";
                var result = await BaiVietAIBUS.Instance.ChamBaiVietAsync(topic, essay, _currentLevel, soTuYeuCau);

                if (!result.Success || result.Result == null)
                {
                    pnlLoading.Visibility = Visibility.Collapsed;
                    pnlError.Visibility = Visibility.Visible;
                    txtErrorDetail.Text = result.ErrorMessage ?? "AI API không phản hồi kết quả hợp lệ.";
                    return;
                }

                // Lưu kết quả vào CSDL theo UserId và Trình độ
                try
                {
                    BaiVietAIBUS.Instance.LuuKetQuaChamBai(CurrentUserId, topic, essay, result.RawResponse, _currentLevel, soTuYeuCau);
                }
                catch (Exception dbEx)
                {
                    System.Diagnostics.Debug.WriteLine($"[BaiVietAIView] Lỗi lưu DB: {dbEx.Message}");
                }

                // Hiển thị kết quả chi tiết lên giao diện
                DisplayGradingResult(result.Result);

                // Tải lại lịch sử để bài viết mới hiển thị trên cùng
                LoadHistoryFromDatabase();
            }
            catch (Exception ex)
            {
                pnlLoading.Visibility = Visibility.Collapsed;
                pnlError.Visibility = Visibility.Visible;
                txtErrorDetail.Text = $"Đã xảy ra lỗi: {ex.Message}";
            }
            finally
            {
                pnlLoading.Visibility = Visibility.Collapsed;
                btnGradeEssay.IsEnabled = true;
            }
        }

        private void BtnRetry_Click(object sender, RoutedEventArgs e)
        {
            pnlError.Visibility = Visibility.Collapsed;
            BtnGradeEssay_Click(sender, e);
        }

        private void DisplayGradingResult(KetQuaChamBaiVietJsonDTO r)
        {
            pnlLoading.Visibility = Visibility.Collapsed;
            pnlError.Visibility = Visibility.Collapsed;
            pnlEmptyPlaceholder.Visibility = Visibility.Collapsed;
            scrollResults.Visibility = Visibility.Visible;

            // Overall band score & general review
            txtBandScore.Text = r.DiemTongQuan.ToString("0.0");
            txtNhanXetTongQuan.Text = !string.IsNullOrWhiteSpace(r.NhanXetTongQuan) 
                ? r.NhanXetTongQuan 
                : "Bài viết triển khai ý mạch lạc, cấu trúc rõ ràng.";

            // 4 Criteria Scores & Progress Bars
            txtTaskScore.Text = $"{r.TaskAchievement:0.0} / 9.0";
            pbTask.Value = Math.Min(100, Math.Max(0, (r.TaskAchievement / 9.0) * 100));

            txtCoherenceScore.Text = $"{r.CoherenceCohesion:0.0} / 9.0";
            pbCoherence.Value = Math.Min(100, Math.Max(0, (r.CoherenceCohesion / 9.0) * 100));

            txtLexicalScore.Text = $"{r.LexicalResource:0.0} / 9.0";
            pbLexical.Value = Math.Min(100, Math.Max(0, (r.LexicalResource / 9.0) * 100));

            txtGrammarScore.Text = $"{r.GrammarRangeAccuracy:0.0} / 9.0";
            pbGrammar.Value = Math.Min(100, Math.Max(0, (r.GrammarRangeAccuracy / 9.0) * 100));

            // Render Error List with Vietnamese explanation
            RenderErrorList(r.DanhSachLoiSai);

            // Corrected essay & Band 8 rewrite with friendly display and strict sanitization
            txtCorrectedEssay.Text = CleanDisplayEssay(r.BaiVietSauChinhSua, txtEssayInput.Text);
            txtBand8Essay.Text = CleanDisplayBand8(r.BanGoiYVietLaiBand8, txtCorrectedEssay.Text);
        }

        private string CleanDisplayBand8(string? raw, string fallbackText)
        {
            if (string.IsNullOrWhiteSpace(raw) || 
                raw.TrimStart().StartsWith("{") || 
                raw.Contains("\"DiemTongQuan\"") ||
                raw.Contains("\"TaskAchievement\""))
            {
                return $"🌟 Phiên bản tham khảo nâng cấp Band 8.0+ từ Gia sư AI:\n\n{fallbackText}\n\n" +
                       $"💡 Lời khuyên nâng Band điểm từ chuyên gia:\n" +
                       $"• Sử dụng linh hoạt các liên từ chuyển ý học thuật như 'Furthermore', 'Consequently', 'In light of this'.\n" +
                       $"• Đa dạng hóa cấu trúc câu phức với mệnh đề quan hệ rút gọn hoặc cấu trúc đảo ngữ.\n" +
                       $"• Vận dụng các Collocations học thuật đã được gợi ý ở danh sách góp ý phía trên để bài viết thêm tự nhiên.";
            }

            string cleaned = raw.Trim();
            if (cleaned.StartsWith("\"") && cleaned.EndsWith("\""))
            {
                cleaned = cleaned.Substring(1, cleaned.Length - 2);
            }
            return cleaned;
        }

        private string CleanDisplayEssay(string? raw, string fallbackText)
        {
            if (string.IsNullOrWhiteSpace(raw) || 
                raw.TrimStart().StartsWith("{") || 
                raw.Contains("\"DiemTongQuan\""))
            {
                return fallbackText;
            }

            string cleaned = raw.Trim();
            if (cleaned.StartsWith("\"") && cleaned.EndsWith("\""))
            {
                cleaned = cleaned.Substring(1, cleaned.Length - 2);
            }
            return cleaned;
        }

        private void RenderErrorList(List<ChiTietLoiSaiDTO> errors)
        {
            pnlErrorList.Children.Clear();

            if (errors == null || errors.Count == 0)
            {
                var noErrorBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(240, 253, 244)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(187, 247, 208)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(14, 12, 14, 12),
                    Margin = new Thickness(0, 0, 0, 10)
                };
                noErrorBorder.Child = new TextBlock
                {
                    Text = "🎉 Tuyệt vời! Không phát hiện lỗi chính tả hoặc lỗi ngữ pháp nghiêm trọng trong bài viết của bạn.",
                    FontSize = 12.5,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(Color.FromRgb(22, 101, 52))
                };
                pnlErrorList.Children.Add(noErrorBorder);
                return;
            }

            int index = 1;
            foreach (var err in errors)
            {
                var card = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = (SolidColorBrush)FindResource("BorderLight"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(14, 12, 14, 12),
                    Margin = new Thickness(0, 0, 0, 10)
                };

                var stack = new StackPanel();

                // Row 1: Error Type Tag & Index
                var topRow = new Grid { Margin = new Thickness(0, 0, 0, 8) };
                var tagBorder = new Border
                {
                    Background = GetErrorTagBg(err.LoaiLoi),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(8, 3, 8, 3),
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                tagBorder.Child = new TextBlock
                {
                    Text = $"💡 Góp ý học tập #{index} • {err.LoaiLoi}",
                    FontSize = 11,
                    FontWeight = FontWeights.Bold,
                    Foreground = GetErrorTagFg(err.LoaiLoi)
                };
                topRow.Children.Add(tagBorder);
                stack.Children.Add(topRow);

                // Row 2: Original Mistake
                var mistakeBlock = new TextBlock
                {
                    Text = $"📝 Câu gốc của bạn: \"{err.ViTriHoacDoanSai}\"",
                    FontSize = 12.5,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(Color.FromRgb(185, 28, 28)), // Red-700
                    Margin = new Thickness(0, 0, 0, 4),
                    TextWrapping = TextWrapping.Wrap // FIX: ĐẢM BẢO HIỂN THỊ ĐẦY ĐỦ 100%, KHÔNG CỤT CHỮ
                };
                stack.Children.Add(mistakeBlock);

                // Row 3: Suggestion
                var fixBlock = new TextBlock
                {
                    Text = $"✨ Gợi ý viết hay hơn: \"{err.GoiYSua}\"",
                    FontSize = 12.5,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(21, 128, 61)), // Green-700
                    Margin = new Thickness(0, 0, 0, 6),
                    TextWrapping = TextWrapping.Wrap // FIX: ĐẢM BẢO HIỂN THỊ ĐẦY ĐỦ 100%, KHÔNG CỤT CHỮ
                };
                stack.Children.Add(fixBlock);

                // Row 4: Vietnamese Explanation
                var explainBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(248, 250, 252)),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(10, 8, 10, 8)
                };
                explainBorder.Child = new TextBlock
                {
                    Text = $"💬 Lời khuyên của Gia sư AI: {err.GiaiThichTiengViet}",
                    FontSize = 12,
                    LineHeight = 18,
                    Foreground = (SolidColorBrush)FindResource("TextDark"),
                    TextWrapping = TextWrapping.Wrap
                };
                stack.Children.Add(explainBorder);

                card.Child = stack;
                pnlErrorList.Children.Add(card);
                index++;
            }
        }

        private SolidColorBrush GetErrorTagBg(string errorType)
        {
            string lower = (errorType ?? "").ToLower();
            if (lower.Contains("chính tả") || lower.Contains("spelling")) return new SolidColorBrush(Color.FromRgb(254, 243, 199)); // Amber-100
            if (lower.Contains("ngữ pháp") || lower.Contains("grammar")) return new SolidColorBrush(Color.FromRgb(254, 226, 226)); // Red-100
            if (lower.Contains("diễn đạt") || lower.Contains("tự nhiên")) return new SolidColorBrush(Color.FromRgb(243, 232, 255)); // Purple-100
            return new SolidColorBrush(Color.FromRgb(224, 242, 254)); // Blue-100
        }

        private SolidColorBrush GetErrorTagFg(string errorType)
        {
            string lower = (errorType ?? "").ToLower();
            if (lower.Contains("chính tả") || lower.Contains("spelling")) return new SolidColorBrush(Color.FromRgb(180, 83, 9)); // Amber-700
            if (lower.Contains("ngữ pháp") || lower.Contains("grammar")) return new SolidColorBrush(Color.FromRgb(185, 28, 28)); // Red-700
            if (lower.Contains("diễn đạt") || lower.Contains("tự nhiên")) return new SolidColorBrush(Color.FromRgb(126, 34, 206)); // Purple-700
            return new SolidColorBrush(Color.FromRgb(29, 78, 216)); // Blue-700
        }

        private void BtnCopyCorrected_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtCorrectedEssay.Text))
                {
                    Clipboard.SetText(txtCorrectedEssay.Text);
                    ThongBaoDialog.ThanhCong("Đã sao chép bài viết sau chỉnh sửa vào Clipboard!", "Đã sao chép");
                }
            }
            catch { }
        }

        private void BtnCopyBand8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtBand8Essay.Text))
                {
                    Clipboard.SetText(txtBand8Essay.Text);
                    ThongBaoDialog.ThanhCong("Đã sao chép bản viết lại Band 8.0+ vào Clipboard!", "Đã sao chép");
                }
            }
            catch { }
        }
        #endregion
    }
}