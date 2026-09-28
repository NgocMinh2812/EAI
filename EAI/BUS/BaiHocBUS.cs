using System;
using System.Collections.Generic;
using System.Linq;
using DTO;

namespace BUS
{
    public class BaiHocBUS
    {
        private static List<BaiHocDTO>? _cacheBaiHoc;

        public static List<BaiHocDTO> LayDanhSachBaiHoc()
        {
            if (_cacheBaiHoc == null)
            {
                KhoiTaoDuLieuMau();
            }
            return _cacheBaiHoc!;
        }

        public static BaiHocDTO? LayChiTietBaiHoc(int maBaiHoc)
        {
            var list = LayDanhSachBaiHoc();
            return list.FirstOrDefault(b => b.MaBaiHoc == maBaiHoc);
        }

        public static void CapNhatTienDo(int maBaiHoc, double phanTram, string trangThai, string diemSo)
        {
            var baiHoc = LayChiTietBaiHoc(maBaiHoc);
            if (baiHoc != null)
            {
                baiHoc.PhanTramTienDo = Math.Min(100, Math.Max(0, phanTram));
                baiHoc.TrangThai = trangThai;
                baiHoc.DiemSo = diemSo;
                baiHoc.NgayHocGanNhat = "Hôm nay";

                if (baiHoc.PhanTramTienDo >= 100)
                {
                    baiHoc.TrangThai = "Hoàn thành";
                    baiHoc.StatusColor = "#0F766E";
                    baiHoc.StatusBackground = "#E6F7F3";
                    baiHoc.ButtonActionText = "Ôn tập lại";
                }
                else if (baiHoc.PhanTramTienDo > 0)
                {
                    baiHoc.TrangThai = "Đang học";
                    baiHoc.StatusColor = "#2563EB";
                    baiHoc.StatusBackground = "#EFF6FF";
                    baiHoc.ButtonActionText = "Học tiếp";
                }
            }
        }

        private static void KhoiTaoDuLieuMau()
        {
            _cacheBaiHoc = new List<BaiHocDTO>
            {
                new BaiHocDTO
                {
                    MaBaiHoc = 1,
                    TieuDe = "Grammar B2 - Conditional Sentences (Câu điều kiện)",
                    LoaiBaiHoc = "NguPhap",
                    TenLoaiHienThi = "Ngữ pháp",
                    MaTrinhDo = "B2",
                    TenChuDe = "Học thuật",
                    TomTat = "Nắm vững cấu trúc, cách dùng câu điều kiện loại 0, 1, 2, 3 và câu điều kiện hỗn hợp (Mixed Conditionals).",
                    NoiDung = @"### 1. Tổng quan về Câu điều kiện (Conditional Sentences)

Câu điều kiện bao gồm 2 mệnh đề:
- **Mệnh đề If (If-clause)**: Nêu lên điều kiện.
- **Mệnh đề chính (Main clause)**: Nêu lên kết quả xảy ra.

---

### 2. Các loại câu điều kiện cơ bản

#### 🔹 Câu điều kiện loại 0 (Zero Conditional): Sự thật hiển nhiên, chân lý khoa học
- **Cấu trúc:** `If + S + V(s/es), S + V(s/es)`
- **Ví dụ:** *If you heat ice, it melts.* (Nếu bạn đun đá, nó sẽ tan chảy).

#### 🔹 Câu điều kiện loại 1 (First Conditional): Khả năng xảy ra ở hiện tại hoặc tương lai
- **Cấu trúc:** `If + S + V(s/es), S + will / can / may + V (nguyên thể)`
- **Ví dụ:** *If it rains tomorrow, we will stay at home.*

#### 🔹 Câu điều kiện loại 2 (Second Conditional): Giả định trái ngược với thực tế ở hiện tại
- **Cấu trúc:** `If + S + V2/ed (to be dùng 'were' cho mọi ngôi), S + would / could + V`
- **Ví dụ:** *If I had a lot of money, I would travel around the world.*

#### 🔹 Câu điều kiện loại 3 (Third Conditional): Giả định trái ngược với thực tế trong quá khứ
- **Cấu trúc:** `If + S + had + V3/ed, S + would have + V3/ed`
- **Ví dụ:** *If she had studied harder, she would have passed the exam.*

---

### 3. Câu điều kiện hỗn hợp (Mixed Conditionals)
- **Hỗn hợp 3 & 2 (Quá khứ ảnh hưởng Hiện tại):**
  `If + S + had + V3/ed, S + would + V`
  *Ví dụ:* *If I had taken that job last year, I would be a manager now.*",
                    ThoiGianUocTinhPhut = 20,
                    PhanTramTienDo = 80,
                    TrangThai = "Hoàn thành",
                    DiemSo = "8/10",
                    NgayHocGanNhat = "Hôm nay",
                    BadgeBackground = "#E0F2FE",
                    BadgeForeground = "#0369A1",
                    IconSkill = "📝",
                    StatusColor = "#0F766E",
                    StatusBackground = "#E6F7F3",
                    ButtonActionText = "Ôn tập lại",
                    DanhSachCauHoi = new List<CauHoiDTO>
                    {
                        new CauHoiDTO
                        {
                            MaCauHoi = 101,
                            NoiDungCauHoi = "If I ______ you, I would take that opportunity immediately.",
                            DanhSachLuaChon = new List<string> { "am", "was", "were", "had been" },
                            DapAnDungIndex = 2,
                            GiaiThichChiTiet = "Đây là câu điều kiện loại 2 giả định trái ngược với hiện tại. Với động từ 'to be', trong văn phong chuẩn tắc luôn dùng 'were' cho tất cả các ngôi."
                        },
                        new CauHoiDTO
                        {
                            MaCauHoi = 102,
                            NoiDungCauHoi = "If she ______ her alarm clock, she wouldn't have missed the morning flight.",
                            DanhSachLuaChon = new List<string> { "sets", "had set", "set", "would set" },
                            DapAnDungIndex = 1,
                            GiaiThichChiTiet = "Mệnh đề chính dùng 'wouldn't have missed' (loại 3), do đó mệnh đề If cần chia ở thì quá khứ hoàn thành (Past Perfect): 'had set'."
                        },
                        new CauHoiDTO
                        {
                            MaCauHoi = 103,
                            NoiDungCauHoi = "If you heat water to 100 degrees Celsius, it ______.",
                            DanhSachLuaChon = new List<string> { "boils", "will boil", "would boil", "boiled" },
                            DapAnDungIndex = 0,
                            GiaiThichChiTiet = "Đây là sự thật hiển nhiên (chân lý khoa học), sử dụng câu điều kiện loại 0: 'If + Present Simple, Present Simple'."
                        }
                    }
                },

                new BaiHocDTO
                {
                    MaBaiHoc = 2,
                    TieuDe = "Vocabulary - Academic Word List Group 5",
                    LoaiBaiHoc = "TuVung",
                    TenLoaiHienThi = "Từ vựng",
                    MaTrinhDo = "B2",
                    TenChuDe = "Giáo dục",
                    TomTat = "Mở rộng vốn từ vựng học thuật chuẩn IELTS/TOEFL: analyze, approach, benefit, concept, context, derive...",
                    NoiDung = @"### 📚 Danh sách từ vựng học thuật quan trọng (AWL Group 5)

| Từ vựng | Loại từ | Phiên âm IPA | Ý nghĩa | Ví dụ & Collocation |
|:---|:---|:---|:---|:---|
| **Analyze** | Verb | `/ˈæn.əl.aɪz/` | Phân tích chi tiết | *We need to analyze the test results thoroughly.* (Collocation: *analyze data*) |
| **Approach** | Noun/Verb | `/əˈprəʊtʃ/` | Tiếp cận, phương pháp | *A holistic approach to education.* (Collocation: *approach a problem*) |
| **Benefit** | Noun/Verb | `/ˈben.ɪ.fɪt/` | Lợi ích, có lợi | *Regular exercise brings immense benefits to health.* |
| **Concept** | Noun | `/ˈkɒn.sept/` | Khái niệm, ý niệm | *The basic concept behind artificial intelligence.* |
| **Context** | Noun | `/ˈkɒn.tekst/` | Bối cảnh, ngữ cảnh | *You must consider the historical context of the event.* |
| **Establish**| Verb | `/ɪˈstæb.lɪʃ/` | Thiết lập, thành lập | *The university was established in 1995.* (Collocation: *establish rules*) |
| **Significant** | Adj | `/sɪɡˈnɪf.ɪ.kənt/` | Quan trọng, đáng kể | *There is a significant difference between the two methods.* |

---

### 💡 Mẹo ghi nhớ từ vựng với AI Flashcard
- Hãy học từ vựng kèm theo **collocation** (cụm từ đi kèm) thay vì học từ đơn lẻ.
- Sử dụng phương pháp lặp lại ngắt quãng (Spaced Repetition) để lưu giữ từ vựng vào trí nhớ dài hạn.",
                    ThoiGianUocTinhPhut = 15,
                    PhanTramTienDo = 70,
                    TrangThai = "Hoàn thành",
                    DiemSo = "7/10",
                    NgayHocGanNhat = "Hôm qua",
                    BadgeBackground = "#F3E8FF",
                    BadgeForeground = "#7E22CE",
                    IconSkill = "📖",
                    StatusColor = "#0F766E",
                    StatusBackground = "#E6F7F3",
                    ButtonActionText = "Ôn tập lại",
                    DanhSachCauHoi = new List<CauHoiDTO>
                    {
                        new CauHoiDTO
                        {
                            MaCauHoi = 201,
                            NoiDungCauHoi = "Scientists need to ______ the data carefully before publishing their findings.",
                            DanhSachLuaChon = new List<string> { "analyze", "benefit", "context", "establish" },
                            DapAnDungIndex = 0,
                            GiaiThichChiTiet = "'Analyze the data' là cụm từ học thuật chuẩn mang nghĩa 'phân tích dữ liệu'."
                        },
                        new CauHoiDTO
                        {
                            MaCauHoi = 202,
                            NoiDungCauHoi = "In order to understand this sentence, you should look at the surrounding ______.",
                            DanhSachLuaChon = new List<string> { "concept", "context", "approach", "benefit" },
                            DapAnDungIndex = 1,
                            GiaiThichChiTiet = "'Surrounding context' nghĩa là ngữ cảnh xung quanh câu văn để suy ra ý nghĩa."
                        }
                    }
                },

                new BaiHocDTO
                {
                    MaBaiHoc = 3,
                    TieuDe = "Reading Comprehension - Renewable Energy & Climate Change",
                    LoaiBaiHoc = "DocHieu",
                    TenLoaiHienThi = "Đọc hiểu",
                    MaTrinhDo = "B1",
                    TenChuDe = "Môi trường",
                    TomTat = "Luyện kỹ năng Skimming & Scanning qua bài đọc về năng lượng tái tạo và giải pháp chống biến đổi khí hậu.",
                    NoiDung = @"### 🌍 Renewable Energy: The Road to Sustainability

As the world grapples with the unprecedented impacts of global climate change, the transition from fossil fuels to renewable energy sources has become more imperative than ever. 

Solar and wind energy have made exponential progress over the last decade, primarily propelled by drastic reductions in manufacturing costs and significant breakthroughs in energy storage solutions. Today, photovoltaic solar farms can generate electricity at a cost comparable to, or even lower than, traditional coal-fired power stations.

However, challenges remain. The intermittent nature of sunlight and wind necessitates robust smart grid management and large-scale battery storage to ensure steady electricity supply around the clock.

Government incentives, coupled with private sector innovations, are pivotal in accelerating this green transition to achieve net-zero carbon emissions by 2050.

---

### 🔑 Key Vocabulary from Passage:
- **Intermittent (adj)**: Gián đoạn, không liên tục.
- **Propel (verb)**: Thúc đẩy, đẩy mạnh.
- **Drastic (adj)**: Quyết liệt, mạnh mẽ.
- **Incentive (noun)**: Sự khích lệ, chính sách khuyến khích.",
                    ThoiGianUocTinhPhut = 25,
                    PhanTramTienDo = 30,
                    TrangThai = "Đang học",
                    DiemSo = "--",
                    NgayHocGanNhat = "3 ngày trước",
                    BadgeBackground = "#FEF3C7",
                    BadgeForeground = "#B45309",
                    IconSkill = "📚",
                    StatusColor = "#2563EB",
                    StatusBackground = "#EFF6FF",
                    ButtonActionText = "Học tiếp",
                    DanhSachCauHoi = new List<CauHoiDTO>
                    {
                        new CauHoiDTO
                        {
                            MaCauHoi = 301,
                            NoiDungCauHoi = "What has primarily driven the growth of solar and wind energy?",
                            DanhSachLuaChon = new List<string> 
                            { 
                                "Increased taxes on coal power", 
                                "Reductions in costs and breakthroughs in energy storage", 
                                "Decreased global energy demand", 
                                "A complete ban on fossil fuels" 
                            },
                            DapAnDungIndex = 1,
                            GiaiThichChiTiet = "Theo đoạn 2: 'primarily propelled by drastic reductions in manufacturing costs and significant breakthroughs in energy storage solutions'."
                        },
                        new CauHoiDTO
                        {
                            MaCauHoi = 302,
                            NoiDungCauHoi = "The word 'Intermittent' in paragraph 3 is closest in meaning to:",
                            DanhSachLuaChon = new List<string> { "Continuous", "Occasional/irregular", "Permanent", "Predictable" },
                            DapAnDungIndex = 1,
                            GiaiThichChiTiet = "'Intermittent' có nghĩa là chập chờn, lúc có lúc không (irregular/occasional)."
                        }
                    }
                },

                new BaiHocDTO
                {
                    MaBaiHoc = 4,
                    TieuDe = "Listening - Daily Conversation & Airport Check-in",
                    LoaiBaiHoc = "NgheHieu",
                    TenLoaiHienThi = "Nghe hiểu",
                    MaTrinhDo = "A2",
                    TenChuDe = "Du lịch",
                    TomTat = "Luyện nghe hội thoại thực tế tại sân bay: làm thủ tục check-in, cân hành lý, hỏi cổng lên máy bay.",
                    NoiDung = @"### 🎧 Audio Dialogue: At the Airport Check-in Counter

**Agent:** Good morning! Where are you flying to today?  
**Passenger:** Good morning. I'm flying to London Heathrow on flight VN105.  
**Agent:** Could I please see your passport and booking reference?  
**Passenger:** Sure, here they are.  
**Agent:** Thank you, Mr. Nguyen. How many bags will you be checking in today?  
**Passenger:** Just this one suitcase, and I have this backpack as a carry-on.  
**Agent:** Alright, please place your suitcase on the scale... It's 18 kilograms, which is well within your allowance. Would you prefer a window or an aisle seat?  
**Passenger:** A window seat, please.  
**Agent:** Here is your boarding pass. Your gate is B14, and boarding starts at 09:45 AM. Have a wonderful flight!  
**Passenger:** Thank you very much!

---

### 💡 Common Expressions:
- **Boarding pass**: Thẻ lên máy bay.
- **Carry-on luggage**: Hành lý xách tay.
- **Checked baggage**: Hành lý ký gửi.
- **Aisle seat**: Ghế gần lối đi (phát âm: `/aɪl/`).",
                    ThoiGianUocTinhPhut = 15,
                    PhanTramTienDo = 0,
                    TrangThai = "Chưa học",
                    DiemSo = "--",
                    NgayHocGanNhat = "Chưa học",
                    BadgeBackground = "#FFE4E6",
                    BadgeForeground = "#E11D48",
                    IconSkill = "🎧",
                    StatusColor = "#64748B",
                    StatusBackground = "#F1F5F9",
                    ButtonActionText = "Vào học ngay",
                    DanhSachCauHoi = new List<CauHoiDTO>
                    {
                        new CauHoiDTO
                        {
                            MaCauHoi = 401,
                            NoiDungCauHoi = "What seat preference did the passenger request?",
                            DanhSachLuaChon = new List<string> { "Middle seat", "Aisle seat", "Window seat", "Front row seat" },
                            DapAnDungIndex = 2,
                            GiaiThichChiTiet = "Hành khách trả lời: 'A window seat, please.'"
                        },
                        new CauHoiDTO
                        {
                            MaCauHoi = 402,
                            NoiDungCauHoi = "What time does the boarding begin?",
                            DanhSachLuaChon = new List<string> { "09:15 AM", "09:45 AM", "10:00 AM", "08:45 AM" },
                            DapAnDungIndex = 1,
                            GiaiThichChiTiet = "Nhân viên thông báo: 'Your gate is B14, and boarding starts at 09:45 AM.'"
                        }
                    }
                },

                new BaiHocDTO
                {
                    MaBaiHoc = 5,
                    TieuDe = "Writing Task 2 - Opinion Essay Structure & Connectors",
                    LoaiBaiHoc = "VietLuan",
                    TenLoaiHienThi = "Viết luận",
                    MaTrinhDo = "B2",
                    TenChuDe = "Kinh tế",
                    TomTat = "Phương pháp viết bài luận quan điểm (Agree or Disagree), cách bố cục 4 đoạn chuẩn IELTS Band 7.0+.",
                    NoiDung = @"### ✍️ IELTS Writing Task 2: Opinion Essay

#### 1. Bố cục 4 đoạn chuẩn:
- **Đoạn 1 - Introduction (Mở bài)**:
  - *Sentence 1 (Paraphrase)*: Viết lại đề bài bằng từ đồng nghĩa.
  - *Sentence 2 (Thesis Statement)*: Nêu rõ quan điểm của bạn (hoàn toàn đồng ý / không đồng ý).

- **Đoạn 2 - Body 1 (Thân bài 1)**:
  - *Topic sentence*: Luận điểm thứ nhất ủng hộ quan điểm.
  - *Explanation*: Giải thích tại sao luận điểm này đúng.
  - *Example*: Đưa ra dẫn chứng thực tế hoặc số liệu minh họa.

- **Đoạn 3 - Body 2 (Thân bài 2)**:
  - *Topic sentence*: Luận điểm thứ hai củng cố góc nhìn.
  - *Explanation & Impact*: Phân tích chiều sâu tác động.
  - *Counter-argument (optional)*: Phản biện ý kiến trái chiều.

- **Đoạn 4 - Conclusion (Kết luận)**:
  - Khẳng định lại lập trường một cách súc tích mà không dùng lại y nguyên từ ngữ của mở bài.

---

### 🔗 Useful Linking Words (Từ nối ăn điểm tiêu chí Coherence & Cohesion):
- **Bắt đầu luận điểm**: *First and foremost, It is undeniable that, Primarily...*
- **Bổ sung ý**: *Furthermore, Moreover, In addition, Consequently...*
- **Đưa ví dụ**: *For instance, To illustrate, A prime example is...*
- **Kết luận**: *In conclusion, To summarize, All things considered...*",
                    ThoiGianUocTinhPhut = 30,
                    PhanTramTienDo = 0,
                    TrangThai = "Chưa học",
                    DiemSo = "--",
                    NgayHocGanNhat = "Chưa học",
                    BadgeBackground = "#DCFCE7",
                    BadgeForeground = "#15803D",
                    IconSkill = "✍️",
                    StatusColor = "#64748B",
                    StatusBackground = "#F1F5F9",
                    ButtonActionText = "Vào học ngay",
                    DanhSachCauHoi = new List<CauHoiDTO>
                    {
                        new CauHoiDTO
                        {
                            MaCauHoi = 501,
                            NoiDungCauHoi = "Which sentence in the Introduction states your personal stance on the topic?",
                            DanhSachLuaChon = new List<string> { "Paraphrase sentence", "Thesis statement", "Example sentence", "Topic sentence" },
                            DapAnDungIndex = 1,
                            GiaiThichChiTiet = "'Thesis statement' (câu tuyên ngôn luận điểm) là câu thể hiện rõ lập trường/quan điểm của người viết trong phần Mở bài."
                        }
                    }
                },

                new BaiHocDTO
                {
                    MaBaiHoc = 6,
                    TieuDe = "Grammar B1 - Present Perfect vs. Past Simple",
                    LoaiBaiHoc = "NguPhap",
                    TenLoaiHienThi = "Ngữ pháp",
                    MaTrinhDo = "B1",
                    TenChuDe = "Đời sống",
                    TomTat = "Phân biệt dứt điểm cách dùng thì Hiện tại hoàn thành và Quá khứ đơn thông qua dấu hiệu nhận biết.",
                    NoiDung = @"### ⚖️ So sánh thì Hiện tại hoàn thành & Quá khứ đơn

#### 1. Quá khứ đơn (Past Simple):
- Dùng cho hành động **đã xảy ra và kết thúc hoàn toàn** trong quá khứ, có thời gian xác định rõ ràng.
- **Dấu hiệu:** *yesterday, last night, in 2018, two days ago, when I was a child...*
- *Ví dụ:* *I saw that movie last night.* (Hành động xem phim đã kết thúc đêm qua).

#### 2. Hiện tại hoàn thành (Present Perfect):
- Dùng cho hành động xảy ra trong quá khứ nhưng **kết quả hoặc ảnh hưởng vẫn liên quan đến hiện tại**, hoặc trải nghiệm tính tới thời điểm hiện tại.
- **Dấu hiệu:** *already, yet, just, ever, never, since, for, so far, recently...*
- *Ví dụ:* *I have seen that movie three times.* (Trải nghiệm tính đến nay).",
                    ThoiGianUocTinhPhut = 15,
                    PhanTramTienDo = 50,
                    TrangThai = "Đang học",
                    DiemSo = "--",
                    NgayHocGanNhat = "Hôm qua",
                    BadgeBackground = "#E0F2FE",
                    BadgeForeground = "#0369A1",
                    IconSkill = "📝",
                    StatusColor = "#2563EB",
                    StatusBackground = "#EFF6FF",
                    ButtonActionText = "Học tiếp",
                    DanhSachCauHoi = new List<CauHoiDTO>
                    {
                        new CauHoiDTO
                        {
                            MaCauHoi = 601,
                            NoiDungCauHoi = "I ______ to Paris three years ago.",
                            DanhSachLuaChon = new List<string> { "have gone", "went", "had gone", "go" },
                            DapAnDungIndex = 1,
                            GiaiThichChiTiet = "Có mốc thời gian xác định trong quá khứ 'three years ago' nên dùng thì Quá khứ đơn: 'went'."
                        },
                        new CauHoiDTO
                        {
                            MaCauHoi = 602,
                            NoiDungCauHoi = "She ______ in this company since 2020.",
                            DanhSachLuaChon = new List<string> { "worked", "has worked", "is working", "works" },
                            DapAnDungIndex = 1,
                            GiaiThichChiTiet = "Có 'since + mốc thời gian' thể hiện hành động bắt đầu từ quá khứ và vẫn đang tiếp diễn ở hiện tại, dùng thì Hiện tại hoàn thành: 'has worked'."
                        }
                    }
                }
            };
        }
    }
}
