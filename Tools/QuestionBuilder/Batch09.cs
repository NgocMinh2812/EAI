using System.Collections.Generic;

namespace QuestionBuilder
{
    public static class Batch09
    {
        public static List<QuestionItem> GetQuestions()
        {
            var list = new List<QuestionItem>();

            // ====================================================================================
            // 1. TÀI LIỆU: 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx (Questions 401-412)
            // ====================================================================================

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_401",
                TopicId = 17, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "When a patient presents with sudden dizziness, shortness of breath, and palpitations, which hospital department should they be directed to immediately?",
                Explanation = "Bệnh nhân có triệu chứng khó thở đột ngột, chóng mặt và đánh trống ngực cần được đưa ngay đến khoa cấp cứu (Emergency Department / A&E - Accident & Emergency).",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Phần I: Triage và phân loại tiếp nhận bệnh nhân khẩn cấp.",
                SourceType = "Extracted", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "Dermatology Clinic", false), ("B", "Emergency Department (A&E)", true), ("C", "Orthopedic Outpatient Desk", false), ("D", "Ophthalmology Consultation", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_402",
                TopicId = 17, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'The doctor explained that an _______ illness develops rapidly and lasts a short period, whereas a chronic condition persists over months or years.'",
                Explanation = "'acute' (cấp tính) đối lập với 'chronic' (mãn tính).",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Thuật ngữ lâm sàng: Acute vs. Chronic conditions.",
                SourceType = "Extracted", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "acute", true), ("B", "accustomed", false), ("C", "acute-free", false), ("D", "acquired", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_403",
                TopicId = 17, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "In clinical pharmacotherapy, what does the term 'adverse drug reaction' (ADR) precisely refer to?",
                Explanation = "'Adverse drug reaction' là phản ứng có hại, ngoài ý muốn xảy ra ở liều dùng thông thường của thuốc.",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Phần II: Dược lý và hướng dẫn sử dụng đơn thuốc (Prescription & Posology).",
                SourceType = "Extracted", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "The intended therapeutic efficacy of the antibiotic", false), ("B", "An unintended, harmful reaction resulting from medication at standard doses", true), ("C", "The physiological shelf-life expiration date of a tablet", false), ("D", "The commercial promotion of over-the-counter medicine", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_404",
                TopicId = 17, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "An 'outpatient' is someone who stays overnight or for weeks in a hospital ward to receive round-the-clock intensive nursing.",
                Explanation = "Sai (False). 'Outpatient' là bệnh nhân ngoại trú (khám và về trong ngày). Bệnh nhân lưu trú qua đêm hoặc điều trị nội trú được gọi là 'inpatient'.",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Phân biệt Inpatient và Outpatient trong quản trị bệnh viện.",
                SourceType = "Extracted", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_405",
                TopicId = 17, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "Doctor: 'How long have you been experiencing this throbbing pain in your temples?' — Which answer is the most clinically helpful?",
                Explanation = "Bệnh nhân cần cung cấp chính xác thời gian khởi phát và tính chất cơn đau (e.g., 'It started three days ago and worsens whenever I look at bright screens').",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Kỹ năng hỏi bệnh sử (Anamnesis & Pain assessment).",
                SourceType = "Extracted", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "Yes, I like temples and ancient architecture.", false), ("B", "It started about three days ago and worsens whenever I look at bright screens.", true), ("C", "I bought some aspirin last month.", false), ("D", "My cousin had a headache two years ago.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_406",
                TopicId = 17, LevelId = 4, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Find the underlined part that contains an error: 'The nurse instructed the patient to take two tablets (A) daily, refrain from (B) alcohol, and coming back (C) for a follow-up consultation on Friday (D).'",
                Explanation = "Lỗi ở (C) 'coming back'. Cần dùng động từ nguyên mẫu 'come back' để song hành với 'take' và 'refrain'.",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Cấu trúc câu chỉ định điều trị y khoa & Song hành ngữ pháp.",
                SourceType = "Applied", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "daily", false), ("B", "refrain from", false), ("C", "coming back", true), ("D", "on Friday", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_407",
                TopicId = 17, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "The label on the medicine bottle reads: 'Do not exceed the recommended _______ of 500mg every 8 hours.'",
                Explanation = "'dosage' (liều lượng sử dụng).",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Hướng dẫn đọc nhãn dược phẩm (Prescription Labels).",
                SourceType = "Extracted", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "dosage", true), ("B", "diet", false), ("C", "duration", false), ("D", "dimension", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_408",
                TopicId = 17, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "What is the primary role of a 'triage nurse' in an emergency room setting?",
                Explanation = "Y tá sàng lọc (triage nurse) có nhiệm vụ đánh giá mức độ khẩn cấp về y tế của bệnh nhân khi mới đến để ưu tiên điều trị ca nguy kịch trước.",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Quy trình phân loại bệnh nhân cấp cứu (Triage Protocol).",
                SourceType = "Extracted", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "To calculate hospital accounting invoices", false), ("B", "To assess clinical acuity and prioritize patients according to medical urgency", true), ("C", "To perform complex neurosurgery operations", false), ("D", "To manage hospital parking facilities", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_409",
                TopicId = 17, LevelId = 4, QuestionType = "NoiCap", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Match each medical specialist with their diagnostic field:\n1. Cardiologist\n2. Neurologist\n3. Pediatrician\n4. Oncologist",
                Explanation = "1-Heart diseases, 2-Brain & nervous system, 3-Children medical care, 4-Cancer treatment.",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Danh mục các chuyên khoa y tế bệnh viện.",
                SourceType = "Extracted", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "1-Heart, 2-Nervous system, 3-Children, 4-Cancer", true), ("B", "1-Cancer, 2-Heart, 3-Nervous system, 4-Children", false), ("C", "1-Skin, 2-Bones, 3-Eyes, 4-Teeth", false), ("D", "1-Teeth, 2-Skin, 3-Cancer, 4-Heart", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_410",
                TopicId = 17, LevelId = 4, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Rewrite the sentence using passive voice with modal verbs: 'The pharmacist must inspect all prescriptions before dispensing narcotics.'",
                Explanation = "Dạng bị động: 'All prescriptions must be inspected by the pharmacist before narcotics are dispensed.'",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Quy định kiểm soát dược phẩm & Câu bị động phương thái.",
                SourceType = "Applied", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "All prescriptions must be inspected by the pharmacist before narcotics are dispensed.", true), ("B", "Narcotics must inspect all prescriptions by the pharmacist.", false), ("C", "The pharmacist is inspecting all prescriptions before dispensing.", false), ("D", "All prescriptions has to be inspected by narcotics.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_411",
                TopicId = 17, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "Which term describes a non-invasive diagnostic technique that uses high-frequency sound waves to visualize internal body organs?",
                Explanation = "Siêu âm (Ultrasound / Ultrasonography) sử dụng sóng âm tần số cao.",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Chẩn đoán hình ảnh: X-ray, MRI, CT Scan, Ultrasound.",
                SourceType = "Extracted", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "Magnetic Resonance Imaging (MRI)", false), ("B", "Ultrasound scan", true), ("C", "Computerized Tomography (CT)", false), ("D", "Positron Emission Tomography", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_412",
                TopicId = 17, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "The surgeon asked the scrub nurse for a sterile _______ to make the initial abdominal incision.",
                Explanation = "'scalpel' (dao mổ phẫu thuật).",
                Citation = "Tài liệu 05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx - Dụng cụ phẫu thuật cơ bản trong phòng mổ.",
                SourceType = "Extracted", SourceFile = "05_Tieng_Anh_chuyen_nganh_Y_te_va_Giao_tiep_Benh_vien.docx",
                Options = new() { ("A", "scalpel", true), ("B", "stethoscope", false), ("C", "thermometer", false), ("D", "bandage", false) }
            });

            // ====================================================================================
            // 2. TÀI LIỆU: 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx (Questions 413-424)
            // ====================================================================================

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_413",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "In a US F-1 visa interview, what is the legal presumption that every applicant must overcome under Section 214(b) of the INA?",
                Explanation = "Theo điều 214(b) Đạo luật Di trú Mỹ (INA), mọi người nộp đơn xin visa không định cư đều bị mặc định coi là có ý định nhập cư (immigrant intent) cho đến khi chứng minh được điều ngược lại.",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 1: Nguyên tắc vàng và điều khoản pháp lý 214(b).",
                SourceType = "Extracted", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "The presumption that the applicant has poor English skills", false), ("B", "The presumption that the applicant intends to immigrate permanently to the US", true), ("C", "The presumption that tuition fees cannot be paid", false), ("D", "The presumption that universities in the US are overpopulated", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_414",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "Consular Officer: 'Why did you choose this specific university in Ohio instead of others?' — Which response demonstrates strong preparation?",
                Explanation = "Ứng viên cần giải thích cụ thể về chương trình học, giáo sư, phòng nghiên cứu hoặc thế mạnh chuyên môn của trường thay vì nói chung chung.",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 2: Trả lời câu hỏi học thuật và lý do chọn trường.",
                SourceType = "Extracted", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "Because my best friend is studying there and we want to share an apartment.", false), ("B", "Their MS program in Data Science offers a specialized curriculum in NLP under Prof. Smith, which directly aligns with my AI career goals in Vietnam.", true), ("C", "Because Ohio has nice weather and cheap shopping malls.", false), ("D", "My study abroad agency picked it for me randomly.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_415",
                TopicId = 18, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'To demonstrate non-immigrant intent, applicants must prove strong _______ ties to their home country, such as familial duties, business ownership, or guaranteed job offers.'",
                Explanation = "'ties' / 'social and economic ties' (mối ràng buộc vững chắc về mặt kinh tế, xã hội và gia đình tại quê hương).",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 3: Chứng minh mối ràng buộc chặt chẽ tại Việt Nam (Strong Ties).",
                SourceType = "Extracted", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "ties", true), ("B", "borders", false), ("C", "passports", false), ("D", "tickets", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_416",
                TopicId = 18, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "Form I-20 is issued by the US Embassy in Hanoi after an applicant successfully passes the in-person consular interview.",
                Explanation = "Sai (False). Mẫu I-20 do chính trường đại học được SEVP chứng nhận cấp cho sinh viên sau khi trúng tuyển; sinh viên mang I-20 đến phỏng vấn tại Đại sứ quán / Lãnh sự quán.",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 4: Giấy tờ hồ sơ: Phân biệt Form I-20, Form DS-160 và SEVIS Fee I-901.",
                SourceType = "Extracted", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_417",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "When asked: 'Who is funding your education and what does your sponsor do?', what is the best strategy?",
                Explanation = "Cần nêu rõ danh tính người tài trợ (thường là cha mẹ hoặc học bổng), nghề nghiệp cụ thể và thu nhập ròng ổn định hàng tháng/năm có khả năng chi trả học phí.",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 5: Chiến lược trả lời năng lực tài chính (Financial Solvency).",
                SourceType = "Extracted", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "I will find a part-time job under the table in American supermarkets to pay for my school.", false), ("B", "My parents are funding my studies; my father owns a certified logistics enterprise earning \\$60,000 annually, backed by savings accounts.", true), ("C", "I have rich relatives in California who promised to help if I run out of cash.", false), ("D", "Money does not matter because education should be free.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_418",
                TopicId = 18, LevelId = 4, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Identify the grammatical mistake: 'Unless you do not prove (A) substantial ties to your home country, the consular officer (B) will be forced (C) to reject your application under Section 214(b) (D).'",
                Explanation = "Lỗi ở (A) 'do not prove'. 'Unless' mang nghĩa 'nếu không', không dùng thể phủ định kép 'Unless you do not prove'. Phải sửa thành 'Unless you prove'.",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Cấu trúc điều kiện với Unless trong ngữ cảnh visa.",
                SourceType = "Applied", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "do not prove", true), ("B", "the consular officer", false), ("C", "will be forced", false), ("D", "under Section 214(b)", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_419",
                TopicId = 18, LevelId = 4, QuestionType = "DienKhuyet", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "The mandatory tracking system operated by DHS to monitor international students in the United States is known by the acronym _______.",
                Explanation = "'SEVIS' (Student and Exchange Visitor Information System).",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 4: Khái niệm SEVIS và cách nộp phí I-901 trực tuyến.",
                SourceType = "Extracted", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "SEVIS", true), ("B", "TOEFL", false), ("C", "ESTA", false), ("D", "GRE", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_420",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "What behavior during an interview can inadvertently cause suspicion from consular officers?",
                Explanation = "Học thuộc lòng như vẹt (rote memorization), mắt nhìn đảo liên tục, hoặc né tránh giao tiếp bằng mắt (avoiding eye contact).",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 6: Ngôn ngữ cơ thể và tâm lý phòng phỏng vấn.",
                SourceType = "Extracted", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "Maintaining steady and polite eye contact with the interviewer", false), ("B", "Reciting memorized answers like a robot without emotional connection or spontaneity", true), ("C", "Dressing in clean, professional business attire", false), ("D", "Greeting the officer clearly with 'Good morning'", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_421",
                TopicId = 18, LevelId = 4, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Rewrite the sentence beginning with 'Only by': 'You can convince the visa officer of your bona fide student intentions only by answering concisely and truthfully.'",
                Explanation = "Đảo ngữ với 'Only by': 'Only by answering concisely and truthfully can you convince the visa officer of your bona fide student intentions.'",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Kỹ thuật phỏng vấn & Đảo ngữ nhấn mạnh.",
                SourceType = "Applied", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "Only by answering concisely and truthfully can you convince the visa officer of your bona fide student intentions.", true), ("B", "Only by answering concisely and truthfully you can convince the visa officer.", false), ("C", "Only by you convince the visa officer answering concisely and truthfully.", false), ("D", "Only by answering truthfully the visa officer can convince you.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_422",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh",
                QuestionText = "What does the term 'Bona fide student' mean in the context of US visa adjudication?",
                Explanation = "'Bona fide student' là sinh viên chân chính, có mục tiêu học tập thực sự, năng lực học vấn phù hợp và kế hoạch tài chính rõ ràng.",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Định nghĩa pháp lý Bona Fide Student.",
                SourceType = "Extracted", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "A student who holds high government scholarships", false), ("B", "A genuine student with authentic educational intent and academic capability", true), ("C", "An international student who works full-time in off-campus restaurants", false), ("D", "A student who has already completed a master's degree", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_423",
                TopicId = 18, LevelId = 4, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "If asked: 'What will you do after graduating from your university in America?', what is the most appropriate answer?",
                Explanation = "Nêu rõ lộ trình trở về nước để phát triển sự nghiệp trong ngành tại các tổ chức, doanh nghiệp ở Việt Nam.",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Mục 7: Kế hoạch nghề nghiệp dài hạn sau khi tốt nghiệp (Career Roadmap).",
                SourceType = "Extracted", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "I hope to stay in New York forever and marry an American citizen.", false), ("B", "I plan to immediately return to Vietnam to join a tech corporation as a Cloud Architect, applying the specialized skills I acquired.", true), ("C", "I have not thought about it yet; I will decide after I finish partying.", false), ("D", "I will look for any permanent job in California.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_424",
                TopicId = 18, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "An online nonimmigrant visa electronic application form required for all F-1 applicants is designated as Form _______.",
                Explanation = "'DS-160' (Online Nonimmigrant Visa Application).",
                Citation = "Tài liệu 06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx - Hướng dẫn điền đơn DS-160 chuẩn xác từng mục.",
                SourceType = "Extracted", SourceFile = "06_Bi_quyet_phong_van_Visa_F1_du_hoc_My.docx",
                Options = new() { ("A", "DS-160", true), ("B", "W-2", false), ("C", "1040-EZ", false), ("D", "I-94", false) }
            });

            // ====================================================================================
            // 3. TÀI LIỆU: 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx (Questions 425-434)
            // ====================================================================================

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_425",
                TopicId = 18, LevelId = 5, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "What is the primary difference between a Statement of Purpose (SOP) and a Personal Statement (PS) in graduate admissions?",
                Explanation = "SOP tập trung vào học thuật, định hướng nghiên cứu chuyên sâu, mục tiêu nghề nghiệp và sự phù hợp với chương trình đào tạo; còn PS tập trung hơn vào hành trình cuộc sống cá nhân, thử thách và động lực nội tâm.",
                Citation = "Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần I: Phân biệt SOP (Học thuật & Nghiên cứu) và Personal Statement.",
                SourceType = "Extracted", SourceFile = "07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx",
                Options = new() { ("A", "An SOP is a list of grades while a PS is a resume", false), ("B", "An SOP focuses on academic trajectory, research questions, and faculty alignment, whereas a PS highlights personal backstory and resilience", true), ("C", "An SOP is written by parents while a PS is written by professors", false), ("D", "An SOP is only required for high school students", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_426",
                TopicId = 18, LevelId = 5, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Which opening hook in a Master's SOP is strongly DISCOURAGED by top-tier admissions committees?",
                Explanation = "Bắt đầu bằng câu trích dẫn nổi tiếng sáo rỗng hoặc câu nói mòn tai như 'Since my early childhood, I have always loved science' bị hội đồng tuyển sinh đánh giá rất thấp.",
                Citation = "Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần II: Các câu mở đầu sáo rỗng (Cliches to avoid in Paragraph 1).",
                SourceType = "Extracted", SourceFile = "07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx",
                Options = new() { ("A", "A specific intellectual problem encountered during undergraduate capstone research", false), ("B", "'Ever since I was a five-year-old child, I have always dreamed of becoming an engineer.'", true), ("C", "An outline of current industry challenges and the applicant's concrete research hypothesis", false), ("D", "A concise overview of technical expertise and graduate study ambitions", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_427",
                TopicId = 18, LevelId = 5, QuestionType = "DienKhuyet", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "In a graduate SOP, when discussing fit with the institution, applicants must demonstrate academic _______ by referencing specific faculty research labs and recent publications.",
                Explanation = "'alignment' (sự liên kết, tương thích học thuật).",
                Citation = "Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần IV: Xây dựng mục Why School & Academic Alignment.",
                SourceType = "Extracted", SourceFile = "07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx",
                Options = new() { ("A", "alignment", true), ("B", "isolation", false), ("C", "distance", false), ("D", "indifference", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_428",
                TopicId = 18, LevelId = 5, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "How should academic shortcomings (such as one semester with low GPA) be addressed in an SOP?",
                Explanation = "Trình bày ngắn gọn, nhận trách nhiệm cá nhân, nêu rõ nguyên nhân khách quan nếu có và nhấn mạnh giải pháp khắc phục cũng như sự tiến bộ vượt bậc sau đó (Upward trend).",
                Citation = "Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần V: Cách xử lý điểm yếu học tập (Handling Red Flags and Low GPAs).",
                SourceType = "Extracted", SourceFile = "07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx",
                Options = new() { ("A", "Blame the professors for giving unfair exam questions", false), ("B", "Address it briefly, take accountability, highlight lessons learned, and demonstrate a clear upward trajectory in subsequent coursework", true), ("C", "Pretend it never happened and alter the official transcript", false), ("D", "Write three full pages complaining about bad health without evidence", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_429",
                TopicId = 18, LevelId = 5, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "Kho",
                QuestionText = "Find the grammatical fault in this SOP sentence: 'Having successfully completing (A) my undergraduate thesis on NLP, my aspiration (B) is to further investigate (C) semantic ambiguity at your laboratory (D).'",
                Explanation = "Lỗi dùng thì ở phân từ hoàn thành (A): phải là 'Having successfully completed', không dùng V-ing 'completing' sau 'having'.",
                Citation = "Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Ngữ pháp viết học thuật cao cấp: Phân từ hoàn thành (Perfect Participle).",
                SourceType = "Applied", SourceFile = "07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx",
                Options = new() { ("A", "Having successfully completing", true), ("B", "my aspiration", false), ("C", "further investigate", false), ("D", "at your laboratory", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_430",
                TopicId = 18, LevelId = 5, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "What is the recommended word count range for a competitive Master's SOP at US/UK universities?",
                Explanation = "Độ dài tiêu chuẩn của một bài SOP Thạc sĩ là khoảng 800 - 1.000 từ (khoảng 2 trang A4 giãn dòng đơn).",
                Citation = "Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần VI: Quy cách định dạng, font chữ và dung lượng từ (800-1000 words).",
                SourceType = "Extracted", SourceFile = "07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx",
                Options = new() { ("A", "100 - 200 words", false), ("B", "800 - 1,000 words (approximately 2 single-spaced pages)", true), ("C", "4,000 - 6,000 words", false), ("D", "At least 15 pages", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_431",
                TopicId = 18, LevelId = 5, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Enhance the academic sophistication of: 'I want to study in your university because your labs are very famous.'",
                Explanation = "'I am keen to join the Department of Computer Science to collaborate with Dr. X whose seminal work on neural transformers resonates deeply with my research interests.'",
                Citation = "Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Kỹ thuật nâng cấp văn phong học thuật (Elevating academic tone).",
                SourceType = "Applied", SourceFile = "07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx",
                Options = new() { ("A", "I am eager to join your department to collaborate with Dr. X, whose seminal work on neural transformers directly aligns with my research objectives.", true), ("B", "I want to enter your lab because everyone knows it is very popular.", false), ("C", "Your university is famous and I like fame.", false), ("D", "Because your lab is cool, I will come.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_432",
                TopicId = 18, LevelId = 5, QuestionType = "DungSai", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "True or False: Using the identical generic SOP for ten different universities without mentioning faculty names or program specifics is considered best practice.",
                Explanation = "Sai (False). Việc nộp cùng 1 bài SOP chung chung (generic copy-paste) là nguyên nhân hàng đầu khiến hồ sơ bị loại ở vòng thẩm định đầu tiên.",
                Citation = "Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần VII: Nguyên tắc tùy biến riêng biệt cho từng trường (Customization rule).",
                SourceType = "Extracted", SourceFile = "07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx",
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_433",
                TopicId = 18, LevelId = 5, QuestionType = "DienKhuyet", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Fill in the blank: 'Applicants should demonstrate intellectual _______ by formulating concise research questions and hypotheses for future graduate work.'",
                Explanation = "'curiosity' hoặc 'maturity' (sự trưởng thành và tò mò trí tuệ).",
                Citation = "Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phẩm chất học thuật cốt lõi (Intellectual Curiosity).",
                SourceType = "Extracted", SourceFile = "07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx",
                Options = new() { ("A", "curiosity", true), ("B", "negligence", false), ("C", "arrogance", false), ("D", "exhaustion", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_434",
                TopicId = 18, LevelId = 5, QuestionType = "TracNghiem", Skill = "Viet", Difficulty = "Kho",
                QuestionText = "Which concluding statement effectively synthesizes the applicant's readiness in an SOP?",
                Explanation = "Câu kết luận cần khẳng định sự sẵn sàng đóng góp cho cộng đồng học thuật của khoa và tiềm năng phát triển nghề nghiệp lâu dài.",
                Citation = "Tài liệu 07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx - Phần VIII: Cách viết đoạn kết ấn tượng (Strong Conclusion).",
                SourceType = "Extracted", SourceFile = "07_Huong_dan_viet_SOP_Statement_of_Purpose_Thac_si.docx",
                Options = new() { ("A", "'Please admit me because I have nowhere else to go.'", false), ("B", "'With my rigorous foundation in software engineering and passion for AI safety, I am prepared to contribute meaningfully to your research community and excel in your master's cohort.'", true), ("C", "'I hope you like this essay and give me a full scholarship.'", false), ("D", "'That is all I have to say about myself.'", false) }
            });

            // ====================================================================================
            // 4. BÀI HỌC 33: Môi trường, Biến đổi khí hậu & Năng lượng tái tạo (Questions 435-442)
            // ====================================================================================

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_435",
                TopicId = 13, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh", LessonId = 33,
                QuestionText = "According to Lesson 33, which renewable energy source harnesses heat stored beneath the Earth's surface?",
                Explanation = "Năng lượng địa nhiệt (Geothermal energy) khai thác nhiệt năng tích tụ sâu trong lòng vỏ trái đất.",
                Citation = "Bài học 33: Từ vựng chuyên sâu: Môi trường, Biến đổi khí hậu & Năng lượng tái tạo - Phần: Năng lượng tái tạo (Renewable Energy Sources).",
                SourceType = "Extracted", SourceFile = "BaiHoc_33_Moi_truong_va_Nang_luong.md",
                Options = new() { ("A", "Hydroelectric energy", false), ("B", "Geothermal energy", true), ("C", "Solar photovoltaic power", false), ("D", "Biomass combustion", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_436",
                TopicId = 13, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh", LessonId = 33,
                QuestionText = "Complete the sentence: 'The total amount of greenhouse gases generated by human actions and manufacturing is referred to as our carbon _______.'",
                Explanation = "'carbon footprint' (dấu chân carbon).",
                Citation = "Bài học 33: Từ vựng chuyên sâu: Môi trường - Khái niệm Dấu chân Carbon (Carbon Footprint).",
                SourceType = "Extracted", SourceFile = "BaiHoc_33_Moi_truong_va_Nang_luong.md",
                Options = new() { ("A", "footprint", true), ("B", "handprint", false), ("C", "shoe", false), ("D", "fingerprint", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_437",
                TopicId = 13, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "TrungBinh", LessonId = 33,
                QuestionText = "What environmental disaster is directly exacerbated by rapid deforestation in mountainous areas?",
                Explanation = "Mất rừng ở vùng đồi núi làm giảm độ liên kết đất, trực tiếp gây ra sạt lở đất nghiêm trọng (severe soil erosion and landslides).",
                Citation = "Bài học 33: Tác động của nạn phá rừng (Deforestation & Soil Degradation).",
                SourceType = "Extracted", SourceFile = "BaiHoc_33_Moi_truong_va_Nang_luong.md",
                Options = new() { ("A", "Severe soil erosion and catastrophic landslides", true), ("B", "Increase in ozone layer thickness", false), ("C", "Reduction of ocean microplastics", false), ("D", "Accelerated glacier formation", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_438",
                TopicId = 13, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De", LessonId = 33,
                QuestionText = "True or False: Biodegradable packaging materials persist in oceans for thousands of years without decomposing.",
                Explanation = "Sai (False). Vật liệu phân hủy sinh học (biodegradable) có khả năng phân rã tự nhiên nhờ vi sinh vật thành các chất vô hại trong thời gian tương đối ngắn.",
                Citation = "Bài học 33: Khái niệm rác thải sinh thái và vật liệu phân hủy sinh học (Biodegradable Materials).",
                SourceType = "Extracted", SourceFile = "BaiHoc_33_Moi_truong_va_Nang_luong.md",
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_439",
                TopicId = 13, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho", LessonId = 33,
                QuestionText = "International climate treaties mandate countries to reach net-_______ carbon emissions by 2050.",
                Explanation = "'net-zero' (phát thải ròng bằng không).",
                Citation = "Bài học 33: Cam kết quốc tế về khí hậu và mục tiêu Net-Zero Emissions.",
                SourceType = "Extracted", SourceFile = "BaiHoc_33_Moi_truong_va_Nang_luong.md",
                Options = new() { ("A", "zero", true), ("B", "fifty", false), ("C", "equal", false), ("D", "half", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_440",
                TopicId = 13, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho", LessonId = 33,
                QuestionText = "Which term describes the process of capturing carbon dioxide from industrial exhausts and trapping it deep underground?",
                Explanation = "Carbon Capture and Storage (CCS - Thu giữ và chôn lấp carbon).",
                Citation = "Bài học 33: Công nghệ giảm phát thải: Carbon Capture and Storage (CCS).",
                SourceType = "Extracted", SourceFile = "BaiHoc_33_Moi_truong_va_Nang_luong.md",
                Options = new() { ("A", "Carbon Capture and Storage (CCS)", true), ("B", "Deforestation mechanism", false), ("C", "Greenhouse acceleration", false), ("D", "Thermal inversion", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_441",
                TopicId = 13, LevelId = 4, QuestionType = "SuaLoiSai", Skill = "NguPhap", Difficulty = "Kho", LessonId = 33,
                QuestionText = "Spot the error: 'If all industrialized nations were to transition (A) to solar energy, greenhouse gas emissions would have decreased (B) significantly within (C) the upcoming decade (D).'",
                Explanation = "Lỗi ở (B) 'would have decreased'. Mệnh đề 'If were to transition' (điều kiện loại 2) chỉ tình huống giả định ở hiện tại/tương lai, nên mệnh đề chính phải là 'would decrease', không dùng 'would have decreased'.",
                Citation = "Bài học 33: Câu điều kiện loại 2 giả định chính sách môi trường.",
                SourceType = "Applied", SourceFile = "BaiHoc_33_Moi_truong_va_Nang_luong.md",
                Options = new() { ("A", "were to transition", false), ("B", "would have decreased", true), ("C", "within", false), ("D", "the upcoming decade", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_442",
                TopicId = 13, LevelId = 4, QuestionType = "VietLaiCau", Skill = "NguPhap", Difficulty = "Kho", LessonId = 33,
                QuestionText = "Transform using inversion: 'If governments had invested heavily in clean wind power ten years ago, air quality would be vastly superior today.'",
                Explanation = "Đảo ngữ điều kiện hỗn hợp loại 3 & 2: 'Had governments invested heavily in clean wind power ten years ago, air quality would be vastly superior today.'",
                Citation = "Bài học 33: Ngữ pháp đảo ngữ điều kiện hỗn hợp chủ đề biến đổi khí hậu.",
                SourceType = "Applied", SourceFile = "BaiHoc_33_Moi_truong_va_Nang_luong.md",
                Options = new() { ("A", "Had governments invested heavily in clean wind power ten years ago, air quality would be vastly superior today.", true), ("B", "Were governments invested heavily, air quality will be vastly superior.", false), ("C", "Should governments invest heavily, air quality would be vastly superior.", false), ("D", "If had governments invested, air quality would be vastly superior.", false) }
            });

            // ====================================================================================
            // 5. BÀI HỌC 34 & 35: Âm nhạc & Nghệ thuật: Cảm xúc, Nhạc cụ & Thành ngữ sáng tạo (Questions 443-450)
            // ====================================================================================

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_443",
                TopicId = 15, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh", LessonId = 34,
                QuestionText = "According to Lesson 34, which musical term refers to the speed or pace of a musical piece?",
                Explanation = "'Tempo' là thuật ngữ chỉ tốc độ hay nhịp độ diễn tấu của một bản nhạc.",
                Citation = "Bài học 34: Âm nhạc và Nghệ thuật: Diễn đạt cảm xúc, Thể loại và Nhạc cụ - Thuật ngữ âm nhạc cơ bản (Tempo, Pitch, Harmony).",
                SourceType = "Extracted", SourceFile = "BaiHoc_34_Am_nhac_va_Nghe_thuat.md",
                Options = new() { ("A", "Tempo", true), ("B", "Pitch", false), ("C", "Lyrics", false), ("D", "Chorus", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_444",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh", LessonId = 35,
                QuestionText = "What is the meaning of the idiom 'music to someone's ears'?",
                Explanation = "Thành ngữ 'music to my ears' chỉ thông tin hoặc tin tức khiến người nghe vô cùng vui mừng, phấn khởi.",
                Citation = "Bài học 35: Thành ngữ và Cụm từ tiếng Anh về Nghệ thuật và Sáng tạo - Thành ngữ âm nhạc thông dụng.",
                SourceType = "Extracted", SourceFile = "BaiHoc_35_Thanh_ngu_Nghe_thuat.md",
                Options = new() { ("A", "Information that is highly pleasing or welcome news to receive", true), ("B", "An unpleasant loud ringing noise in the ear", false), ("C", "A recommendation for new wireless headphones", false), ("D", "A boring lecture on classical violin compositions", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_445",
                TopicId = 15, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh", LessonId = 35,
                QuestionText = "We don't have a rigid schedule for the weekend project, so let's just play it by _______.",
                Explanation = "'play it by ear' (tùy cơ ứng biến, linh hoạt theo tình hình thực tế).",
                Citation = "Bài học 35: Thành ngữ 'Play it by ear' trong giao tiếp tiếng Anh thường nhật.",
                SourceType = "Extracted", SourceFile = "BaiHoc_35_Thanh_ngu_Nghe_thuat.md",
                Options = new() { ("A", "ear", true), ("B", "eye", false), ("C", "mouth", false), ("D", "hand", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_446",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho", LessonId = 35,
                QuestionText = "When someone has to 'face the music', what are they forced to do?",
                Explanation = "'Face the music' có nghĩa là phải chấp nhận đối mặt với hậu quả hoặc sự chỉ trích từ hành động sai trái của mình.",
                Citation = "Bài học 35: Thành ngữ 'Face the music' và nguồn gốc lịch sử.",
                SourceType = "Extracted", SourceFile = "BaiHoc_35_Thanh_ngu_Nghe_thuat.md",
                Options = new() { ("A", "Accept the unpleasant consequences or criticism of one's actions", true), ("B", "Buy front-row concert tickets to an orchestra", false), ("C", "Learn how to conduct a choir", false), ("D", "Dance to classical waltz rhythms", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_447",
                TopicId = 15, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh", LessonId = 35,
                QuestionText = "Nobody likes coworkers who constantly blow their own _______ instead of sharing credit with the team.",
                Explanation = "'blow one's own trumpet / horn' (tự khoe khoang, tự tâng bốc bản thân).",
                Citation = "Bài học 35: Cụm từ 'Blow one's own trumpet' trong môi trường làm việc chuyên nghiệp.",
                SourceType = "Extracted", SourceFile = "BaiHoc_35_Thanh_ngu_Nghe_thuat.md",
                Options = new() { ("A", "trumpet", true), ("B", "flute", false), ("C", "guitar", false), ("D", "drum", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_448",
                TopicId = 15, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh", LessonId = 35,
                QuestionText = "To 'fine-tune' an algorithm or business process means to:",
                Explanation = "'fine-tune' có nghĩa là tinh chỉnh, điều chỉnh chi tiết nhỏ để đạt được hiệu suất tối ưu nhất.",
                Citation = "Bài học 35: Từ vựng mượn từ âm nhạc dùng trong công nghệ: 'Fine-tune'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_35_Thanh_ngu_Nghe_thuat.md",
                Options = new() { ("A", "Make minor, precise adjustments to achieve the best possible performance", true), ("B", "Cancel the entire system due to critical failure", false), ("C", "Play background music while coding", false), ("D", "Delete all database tables", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_449",
                TopicId = 15, LevelId = 4, QuestionType = "NoiCap", Skill = "TuVung", Difficulty = "TrungBinh", LessonId = 34,
                QuestionText = "Match each instrument to its acoustic category:\n1. Cello\n2. Clarinet\n3. Trombone\n4. Timpani",
                Explanation = "1-String, 2-Woodwind, 3-Brass, 4-Percussion.",
                Citation = "Bài học 34: Phân loại các bộ nhạc cụ trong dàn nhạc giao hưởng (Orchestra Families).",
                SourceType = "Extracted", SourceFile = "BaiHoc_34_Am_nhac_va_Nghe_thuat.md",
                Options = new() { ("A", "1-String, 2-Woodwind, 3-Brass, 4-Percussion", true), ("B", "1-Brass, 2-String, 3-Percussion, 4-Woodwind", false), ("C", "1-Percussion, 2-Brass, 3-String, 4-Woodwind", false), ("D", "1-Woodwind, 2-Percussion, 3-Brass, 4-String", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_450",
                TopicId = 15, LevelId = 4, QuestionType = "TraLoiNgan", Skill = "Doc", Difficulty = "Kho", LessonId = 35,
                QuestionText = "In art criticism, what French-origin term describes the prevailing mood, atmosphere, or environment of an artistic installation?",
                Explanation = "'Ambiance' (hoặc 'Atmosphere' / 'Milieu') chỉ bầu không khí, không gian bao quanh của một tác phẩm sắp đặt.",
                Citation = "Bài học 35: Thuật ngữ phê bình mỹ thuật và triển lãm không gian đương đại.",
                SourceType = "Extracted", SourceFile = "BaiHoc_35_Thanh_ngu_Nghe_thuat.md",
                Options = new() { ("A", "Ambiance", true), ("B", "Collage", false), ("C", "Fresco", false), ("D", "Silhouette", false) }
            });

            return list;
        }
    }
}

