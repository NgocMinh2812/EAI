using System.Collections.Generic;

namespace QuestionBuilder
{
    public static class Batch07
    {
        public static List<QuestionItem> GetQuestions()
        {
            var list = new List<QuestionItem>();

            // --- BÀI HỌC 25: Từ vựng tiếng Anh Chuyên ngành Kỹ thuật Phần mềm & Cơ sở dữ liệu (B1 - B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_301",
                TopicId = 17, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "In software engineering, what is the process of restructuring existing code without altering its external behavior called?",
                Explanation = "'Refactoring' là quá trình tái cấu trúc và làm sạch mã nguồn nội bộ mà không làm thay đổi hành vi đầu ra bên ngoài của phần mềm.",
                Citation = "Bài học 25 - Thuật ngữ công nghệ phần mềm: Refactoring: Tái cấu trúc code.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "Rebooting", false), ("B", "Refactoring", true), ("C", "Overclocking", false), ("D", "Defragmenting", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_302",
                TopicId = 17, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What does the database constraint 'Foreign Key' ensure between relational tables?",
                Explanation = "'Foreign Key' (Khóa ngoại) đảm bảo tính toàn vẹn tham chiếu (referential integrity) giữa hai bảng có liên kết với nhau.",
                Citation = "Bài học 25 - Thuật ngữ Cơ sở dữ liệu: Foreign Key and Referential Integrity.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "It translates table column names into foreign languages.", false), ("B", "It enforces referential integrity by linking a record in one table to the primary key of another table.", true), ("C", "It encrypts stored passwords automatically.", false), ("D", "It prevents computers from shutting down.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_303",
                TopicId = 17, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What is the key performance benefit of creating an 'Index' on a frequently queried SQL database column?",
                Explanation = "Tạo chỉ mục (Index) giúp tăng tốc độ tìm kiếm và truy vấn dữ liệu từ O(N) xuống O(log N) bằng cấu trúc B-Tree.",
                Citation = "Bài học 25 - Tối ưu hóa truy vấn Database: Database Indexing.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "It decreases the physical disk storage used by tables.", false), ("B", "It significantly speeds up data retrieval operations by allowing the engine to locate records without scanning the entire table.", true), ("C", "It automatically generates graphical charts.", false), ("D", "It prevents duplicate user sign-ups completely.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_304",
                TopicId = 17, LevelId = 3, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Complete the software term: 'Continuous Integration and Continuous _______ (CI/CD) automates software delivery pipelines.'",
                Explanation = "CI/CD là viết tắt của Continuous Integration và Continuous Deployment (hoặc Delivery).",
                Citation = "Bài học 25 - Khái niệm DevOps & Tự động hóa: CI/CD (Continuous Integration / Continuous Deployment).",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "Deployment", true), ("B", "Destruction", false), ("C", "Decline", false), ("D", "Dispute", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_305",
                TopicId = 17, LevelId = 4, QuestionType = "DungSai", Skill = "TuVung", Difficulty = "De",
                QuestionText = "True or False: In concurrent computing, a 'Deadlock' refers to a state where two or more threads are permanently blocked, each waiting for a lock held by the other.",
                Explanation = "Đúng. 'Deadlock' trong lập trình đồng thời là tình trạng bế tắc khi hai hay nhiều tiến trình khóa tài nguyên của nhau.",
                Citation = "Bài học 25 - Khái niệm đa luồng và đồng thời: Concurrency and Deadlocks.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BÀI HỌC 26: Giao tiếp trong Daily Standup và Báo cáo Tiến độ Sprint (B1 - B2) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_306",
                TopicId = 18, LevelId = 3, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "De",
                QuestionText = "What are the three core questions answered by every team member in a Daily Scrum Standup meeting?",
                Explanation = "3 câu hỏi kinh điển trong cuộc họp Daily Scrum: 'Hôm qua tôi đã làm gì?', 'Hôm nay tôi sẽ làm gì?', và 'Tôi có gặp vướng mắc/trở ngại gì không?'.",
                Citation = "Bài học 26 - Cấu trúc 3 câu hỏi Daily Standup: What I did yesterday, what I will do today, any blockers.",
                SourceType = "Extracted", SourceFile = "BaiHoc_26_DailyStandupScrum.txt", LessonId = 26,
                Options = new() { ("A", "What did I do yesterday? What will I do today? Are there any blockers or impediments?", true), ("B", "Who is the boss? What time is lunch? When is salary day?", false), ("C", "Why is the code broken? Who made the bug? Where is the coffee?", false), ("D", "What is my job? Who are you? Where do we go?", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_307",
                TopicId = 18, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "In Agile development, what does the term 'Blocker' (or Impediment) signify?",
                Explanation = "'Blocker' là bất kỳ trở ngại hoặc vướng mắc kỹ thuật/quy trình nào cản trở lập trình viên hoàn thành nhiệm vụ.",
                Citation = "Bài học 26 - Thuật ngữ Scrum: Blockers / Impediments.",
                SourceType = "Extracted", SourceFile = "BaiHoc_26_DailyStandupScrum.txt", LessonId = 26,
                Options = new() { ("A", "A physical lock on the office front gate", false), ("B", "An obstacle or issue that prevents a team member from making progress on a task", true), ("C", "A special software firewall tool", false), ("D", "A keyboard key that is stuck", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_308",
                TopicId = 18, LevelId = 3, QuestionType = "DienKhuyet", Skill = "Noi", Difficulty = "De",
                QuestionText = "Fill in the blank in a standup update: 'Yesterday I finished implementing the login API. Today I am going to write unit _______.'",
                Explanation = "'unit tests' (các bài kiểm thử đơn vị).",
                Citation = "Bài học 26 - Mẫu câu báo cáo tiến độ Sprint: 'Today I will write unit tests.'",
                SourceType = "Extracted", SourceFile = "BaiHoc_26_DailyStandupScrum.txt", LessonId = 26,
                Options = new() { ("A", "tests", true), ("B", "novels", false), ("C", "games", false), ("D", "movies", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_309",
                TopicId = 18, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "De",
                QuestionText = "What is the recommended maximum duration for an effective Daily Standup meeting?",
                Explanation = "Theo chuẩn Scrum quốc tế, buổi họp Daily Standup chỉ nên kéo dài tối đa 15 phút và tất cả mọi người đứng họp để giữ sự tập trung, nhanh gọn.",
                Citation = "Bài học 26 - Thời lượng tiêu chuẩn của Daily Standup: 15 phút.",
                SourceType = "Extracted", SourceFile = "BaiHoc_26_DailyStandupScrum.txt", LessonId = 26,
                Options = new() { ("A", "2 hours", false), ("B", "15 minutes", true), ("C", "45 minutes", false), ("D", "1 full working day", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_310",
                TopicId = 18, LevelId = 4, QuestionType = "VietLaiCau", Skill = "Viet", Difficulty = "TrungBinh",
                QuestionText = "Rewrite the informal blocker report professionally: 'The third party API is dead so I can't do anything.'",
                Explanation = "'I am currently blocked by downtime on the third-party payment API; our team has escalated the issue to vendor support.'",
                Citation = "Bài học 26 - Kỹ năng báo cáo Blocker chuyên nghiệp trong họp kỹ thuật.",
                SourceType = "Applied", SourceFile = "BaiHoc_26_DailyStandupScrum.txt", LessonId = 26,
                Options = new() { ("A", "I am currently blocked by an outage on the third-party API, and we have escalated the issue to vendor support.", true), ("B", "API died, not my problem anymore.", false), ("C", "I am sleeping until API wakes up.", false), ("D", "Third party ruined everything today.", false) }
            });

            // --- TÀI LIỆU: 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx (B2 - C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_311",
                TopicId = 16, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "In Machine Learning, what is 'Overfitting'?",
                Explanation = "'Overfitting' (Quá khớp) xảy ra khi mô hình học thuộc lòng quá kỹ dữ liệu huấn luyện (training data), dẫn đến khả năng tổng quát hóa kém trên dữ liệu mới (test data).",
                Citation = "Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Mục 2: Khái niệm Overfitting vs. Underfitting.",
                SourceType = "Extracted", SourceFile = "01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx",
                Options = new() { ("A", "When a computer program runs out of memory", false), ("B", "When a model learns training data noise too closely, resulting in poor generalization on unseen test data", true), ("C", "When a neural network has zero parameters", false), ("D", "When the training dataset is deleted accidentally", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_312",
                TopicId = 16, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "What is the mathematical function that quantifies the difference between model predictions and ground-truth labels during training?",
                Explanation = "'Loss Function' (hoặc Cost Function) đo lường sai số giữa giá trị dự đoán của mô hình và nhãn thực tế để tối ưu hóa trọng số.",
                Citation = "Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Mục 3: Loss Function (Hàm mất mát) và Gradient Descent.",
                SourceType = "Extracted", SourceFile = "01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx",
                Options = new() { ("A", "Loss Function", true), ("B", "Linear Regression", false), ("C", "Hash Function", false), ("D", "Boolean Operator", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_313",
                TopicId = 16, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Fill in the blank: 'In deep learning training, one complete pass of the entire training dataset through the neural network is called an _______.'",
                Explanation = "Một chu kỳ duyệt qua toàn bộ tập dữ liệu huấn luyện được gọi là một 'Epoch'.",
                Citation = "Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Mục 4: Epoch, Batch Size và Iterations.",
                SourceType = "Extracted", SourceFile = "01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx",
                Options = new() { ("A", "epoch", true), ("B", "era", false), ("C", "century", false), ("D", "interval", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_314",
                TopicId = 16, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What distinguishes 'Supervised Learning' from 'Unsupervised Learning'?",
                Explanation = "Học có giám sát (Supervised Learning) yêu cầu dữ liệu huấn luyện đã được gán nhãn sẵn (labeled data), trong khi Học không giám sát tự tìm cụm quy luật trên dữ liệu chưa gán nhãn (unlabeled data).",
                Citation = "Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Mục 1: Phân loại Supervised, Unsupervised, Reinforcement Learning.",
                SourceType = "Extracted", SourceFile = "01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx",
                Options = new() { ("A", "Supervised learning uses labeled training datasets, whereas unsupervised learning identifies patterns in unlabeled data.", true), ("B", "Unsupervised learning requires humans to watch the computer constantly.", false), ("C", "Supervised learning can only be programmed in Python 2.", false), ("D", "There is no technical difference.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_315",
                TopicId = 16, LevelId = 4, QuestionType = "DungSai", Skill = "TuVung", Difficulty = "De",
                QuestionText = "True or False: Regularization techniques (such as L1 Lasso, L2 Ridge, and Dropout) are specifically employed to combat and prevent overfitting in machine learning models.",
                Explanation = "Đúng. Các kỹ thuật điều chuẩn (Regularization) như L1, L2 và Dropout được áp dụng để hạn chế hiện tượng overfitting.",
                Citation = "Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Mục 5: Kỹ thuật chống Overfitting: Regularization and Dropout.",
                SourceType = "Extracted", SourceFile = "01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- TÀI LIỆU: 02_Prompt_Engineering_Principles_for_Developers.pdf (B2 - C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_316",
                TopicId = 16, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What is 'Few-Shot Prompting' in modern Large Language Model (LLM) interaction?",
                Explanation = "'Few-shot prompting' là kỹ thuật cung cấp một vài cặp ví dụ mẫu (input - output examples) ngay trong câu nhắc để mô hình hiểu rõ định dạng và phong cách mong muốn trước khi xử lý yêu cầu chính.",
                Citation = "Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Phần 2: Kỹ thuật Few-Shot Prompting vs Zero-Shot Prompting.",
                SourceType = "Extracted", SourceFile = "02_Prompt_Engineering_Principles_for_Developers.pdf",
                Options = new() { ("A", "Sending only short prompts under five words", false), ("B", "Providing a few explicit input-output demonstration examples within the prompt context before the target task", true), ("C", "Reloading the browser three times before prompting", false), ("D", "Training an entirely new neural network from scratch", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_317",
                TopicId = 16, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "Why does 'Chain-of-Thought' (CoT) prompting significantly improve reasoning performance on multi-step math and logic problems?",
                Explanation = "Chain-of-Thought thúc đẩy mô hình tạo ra các bước suy luận trung gian (intermediate reasoning steps) giống con người trước khi đưa ra kết luận cuối cùng.",
                Citation = "Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Phần 3: Chain-of-Thought (CoT) Reasoning.",
                SourceType = "Extracted", SourceFile = "02_Prompt_Engineering_Principles_for_Developers.pdf",
                Options = new() { ("A", "Because it forces the AI to break down complex reasoning into explicit intermediate steps, reducing hasty logical leaps.", true), ("B", "Because it doubles the graphics card GPU clock speed.", false), ("C", "Because it translates text into binary code.", false), ("D", "Because it suppresses all safety filters.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_318",
                TopicId = 16, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Fill in the prompt engineering term: 'Assigning a clear persona or role (e.g., \"Act as a Senior Cloud Architect\") is known as a _______ prompt.'",
                Explanation = "'Role' hoặc 'System' prompt xác định vai trò, phong cách và góc nhìn chuyên môn của AI.",
                Citation = "Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Phần 1: Role-based System Prompting.",
                SourceType = "Extracted", SourceFile = "02_Prompt_Engineering_Principles_for_Developers.pdf",
                Options = new() { ("A", "Role", true), ("B", "Joke", false), ("C", "Random", false), ("D", "Secret", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_319",
                TopicId = 16, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What does 'Hallucination' refer to in generative AI and LLM outputs?",
                Explanation = "'Hallucination' (Hiện tượng ảo giác) là khi mô hình ngôn ngữ tự bịa ra thông tin sai lệch nhưng diễn đạt với giọng điệu rất tự tin và trôi chảy như thật.",
                Citation = "Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Phần 4: Nhận diện và giảm thiểu hiện tượng Hallucination.",
                SourceType = "Extracted", SourceFile = "02_Prompt_Engineering_Principles_for_Developers.pdf",
                Options = new() { ("A", "When the monitor display flickers", false), ("B", "When an LLM produces plausible-sounding but factually fabricated or incorrect information presented confidently", true), ("C", "When the computer runs out of power", false), ("D", "When the network cable is unplugged", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_320",
                TopicId = 16, LevelId = 4, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: Using precise delimiters (such as triple backticks ``` or XML tags <context>) helps an LLM distinguish between developer instructions and external user input.",
                Explanation = "Đúng. Sử dụng dấu phân cách rõ ràng (Delimiters như triple backticks hoặc XML tags) là nguyên tắc cốt lõi giúp phòng tránh lỗi Prompt Injection và tăng độ chính xác.",
                Citation = "Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Nguyên tắc phân tách ngữ cảnh bằng Delimiters.",
                SourceType = "Extracted", SourceFile = "02_Prompt_Engineering_Principles_for_Developers.pdf",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- TÀI LIỆU: 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf (C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_321",
                TopicId = 16, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What architectural breakthrough was introduced by Vaswani et al. in the landmark 2017 paper 'Attention Is All You Need'?",
                Explanation = "Bài báo 'Attention Is All You Need' giới thiệu kiến trúc Transformer dựa hoàn toàn trên cơ chế Attention, loại bỏ hoàn toàn tính tuần tự chậm chạp của mạng RNN và LSTM.",
                Citation = "Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Tóm tắt bài báo Attention Is All You Need: The Transformer Architecture.",
                SourceType = "Extracted", SourceFile = "03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf",
                Options = new() { ("A", "Convolutional neural networks for video games", false), ("B", "The Transformer architecture, replacing recurrent and convolutional layers entirely with self-attention mechanisms", true), ("C", "A new hardware processor made of gold", false), ("D", "A relational database storage engine", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_322",
                TopicId = 16, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What are the three fundamental vectors calculated in the Scaled Dot-Product Attention formula?",
                Explanation = "Cơ chế Self-Attention dựa trên 3 vector: Queries (Q), Keys (K) và Values (V): Attention(Q, K, V) = softmax(QK^T / sqrt(d_k)) * V.",
                Citation = "Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Công thức Scaled Dot-Product Attention: Query, Key, Value vectors.",
                SourceType = "Extracted", SourceFile = "03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf",
                Options = new() { ("A", "Queries (Q), Keys (K), and Values (V)", true), ("B", "Quality, Knowledge, and Verification", false), ("C", "Quantity, Kernel, and Variables", false), ("D", "Question, Keyword, and Vocabulary", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_323",
                TopicId = 16, LevelId = 5, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "Fill in the blank: 'Because Transformers process all tokens simultaneously without recurrence, _______ Encoding is injected to inform the model of word order.'",
                Explanation = "'Positional' Encoding (Mã hóa vị trí) được cộng vào các vector nhúng để mô hình nhận biết thứ tự trước sau của các từ trong câu.",
                Citation = "Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Cơ chế Positional Encoding trong Transformer.",
                SourceType = "Extracted", SourceFile = "03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf",
                Options = new() { ("A", "Positional", true), ("B", "Alphabetical", false), ("C", "Random", false), ("D", "Color", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_324",
                TopicId = 16, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What major computational advantage did Transformers offer over traditional Recurrent Neural Networks (RNNs)?",
                Explanation = "Ưu thế vượt trội của Transformer là khả năng song song hóa tính toán tối đa trên GPU (Massive Parallelization), giúp huấn luyện nhanh hơn gấp nhiều lần so với tính tuần tự của RNN.",
                Citation = "Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - So sánh hiệu năng: Khả năng tính toán song song (Parallelization).",
                SourceType = "Extracted", SourceFile = "03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf",
                Options = new() { ("A", "They consume zero electricity.", false), ("B", "Massive parallelization across GPUs because input sequences are processed all at once rather than sequentially step-by-step.", true), ("C", "They run on devices without memory.", false), ("D", "They require no training data.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_325",
                TopicId = 16, LevelId = 5, QuestionType = "DungSai", Skill = "Doc", Difficulty = "De",
                QuestionText = "True or False: Modern foundational LLMs like GPT-4, Gemini, and Claude are fundamentally based on the Transformer architecture.",
                Explanation = "Đúng. Toàn bộ các mô hình ngôn ngữ lớn hiện đại (GPT, Gemini, Llama, Claude) đều phát triển từ nền tảng kiến trúc Transformer.",
                Citation = "Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Ứng dụng của Transformer trong AI tạo sinh hiện đại.",
                SourceType = "Extracted", SourceFile = "03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf",
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            // --- BỔ SUNG CÂU HỎI KỸ THUẬT CNTT & TIẾNG ANH LẬP TRÌNH (B1 - C1) ---
            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_326",
                TopicId = 17, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "In Git version control, what does 'git commit' do?",
                Explanation = "'git commit' lưu lại ảnh chụp trạng thái (snapshot) các thay đổi đã được staged vào lịch sử kho mã nguồn cục bộ.",
                Citation = "Bài học 25 - Thuật ngữ quản lý phiên bản Git: Commit, Branch, Merge, Pull Request.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "It permanently deletes all files in the repository.", false), ("B", "It records a snapshot of the staged changes to the local repository history with a descriptive message.", true), ("C", "It uploads code directly to the live production server.", false), ("D", "It reboots the computer operating system.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_327",
                TopicId = 17, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What does the architectural term 'Microservices' contrast with?",
                Explanation = "'Microservices' (Kiến trúc vi dịch vụ phân tán) đối lập trực tiếp với 'Monolithic architecture' (Kiến trúc khối đơn hợp nhất).",
                Citation = "Bài học 25 - Kiến trúc hệ thống phần mềm: Monolith vs. Microservices.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "Relational database", false), ("B", "Monolithic architecture", true), ("C", "Operating system", false), ("D", "Agile methodology", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_328",
                TopicId = 17, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'To prevent SQL injection attacks, developers must always use parameterized _______.'",
                Explanation = "'parameterized queries' (các câu truy vấn tham số hóa) là biện pháp bảo mật cốt lõi chống SQL Injection.",
                Citation = "Bài học 25 - An toàn bảo mật cơ sở dữ liệu: Parameterized queries against SQL Injection.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "queries", true), ("B", "passwords", false), ("C", "hard drives", false), ("D", "screens", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_329",
                TopicId = 18, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "De",
                QuestionText = "What is a 'Pull Request' (PR) used for in collaborative GitHub workflows?",
                Explanation = "'Pull Request' là đề xuất trộn (merge) các commit từ một nhánh tính năng (feature branch) vào nhánh chính sau khi được đồng nghiệp review.",
                Citation = "Bài học 26 - Quy trình làm việc nhóm phần mềm: Code Review via Pull Requests.",
                SourceType = "Extracted", SourceFile = "BaiHoc_26_DailyStandupScrum.txt", LessonId = 26,
                Options = new() { ("A", "A request to pull all ethernet cables out", false), ("B", "A proposed set of code changes submitted for peer review before merging into the main branch", true), ("C", "An order for new office hardware", false), ("D", "A formal resignation notice", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_330",
                TopicId = 16, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What is 'Latent Space' in deep learning and autoencoder models?",
                Explanation = "'Không gian tiềm ẩn' (Latent space) là không gian biểu diễn đa chiều nén chứa các đặc trưng cốt lõi rút trích được từ dữ liệu gốc.",
                Citation = "Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Khái niệm Latent Space và Feature Representation.",
                SourceType = "Extracted", SourceFile = "01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx",
                Options = new() { ("A", "An empty hard drive partition", false), ("B", "A compressed, lower-dimensional mathematical space where meaningful features of data are represented", true), ("C", "The physical area behind a server rack", false), ("D", "The RAM cache of a web browser", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_331",
                TopicId = 16, LevelId = 4, QuestionType = "DungSai", Skill = "TuVung", Difficulty = "De",
                QuestionText = "True or False: In API design, 'REST' stands for Representational State Transfer.",
                Explanation = "Đúng. 'REST' là viết tắt của 'Representational State Transfer' - tiêu chuẩn thiết kế web service thông dụng nhất.",
                Citation = "Bài học 25 - Thuật ngữ API: RESTful Web Services.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_332",
                TopicId = 16, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "In AI Prompt Engineering, what is 'Temperature' as an LLM hyperparameter?",
                Explanation = "'Temperature' điều khiển tính ngẫu nhiên và sáng tạo của câu trả lời. Temperature = 0 cho kết quả tất định, chính xác; Temperature cao (>0.8) cho kết quả phong phú, sáng tạo.",
                Citation = "Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Tham số suy luận LLM: Temperature, Top-p, Top-k.",
                SourceType = "Extracted", SourceFile = "02_Prompt_Engineering_Principles_for_Developers.pdf",
                Options = new() { ("A", "The physical heat of the server room measured in Celsius", false), ("B", "A parameter controlling the randomness and creativity of generated token probabilities (0 = deterministic, 1 = creative)", true), ("C", "The speed at which characters appear on screen", false), ("D", "The length of the prompt in kilobytes", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_333",
                TopicId = 17, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What does 'ACID' represent in database transaction theory?",
                Explanation = "'ACID' là 4 thuộc tính đảm bảo giao dịch cơ sở dữ liệu tin cậy: Atomicity (Nguyên tử), Consistency (Nhất quán), Isolation (Cô lập), Durability (Bền vững).",
                Citation = "Bài học 25 - Thuộc tính giao dịch Database: ACID properties.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "Atomicity, Consistency, Isolation, Durability", true), ("B", "Automatic, Constant, Infinite, Dynamic", false), ("C", "Access, Control, Identify, Delete", false), ("D", "Application, Code, Integration, Deployment", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_334",
                TopicId = 18, LevelId = 3, QuestionType = "DienKhuyet", Skill = "Noi", Difficulty = "De",
                QuestionText = "Fill in the Agile term: 'At the end of each two-week cycle, the team holds a Sprint _______ to discuss what went well and what can be improved.'",
                Explanation = "'Sprint Retrospective' là buổi họp hồi tưởng cuối Sprint để đúc rút kinh nghiệm cải tiến quy trình.",
                Citation = "Bài học 26 - Các sự kiện trong Scrum: Sprint Planning, Daily Standup, Sprint Review, Sprint Retrospective.",
                SourceType = "Extracted", SourceFile = "BaiHoc_26_DailyStandupScrum.txt", LessonId = 26,
                Options = new() { ("A", "Retrospective", true), ("B", "Cancellation", false), ("C", "Punishment", false), ("D", "Celebration", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_335",
                TopicId = 16, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "In neural networks, what is the role of an 'Activation Function' (e.g., ReLU, Sigmoid)?",
                Explanation = "Hàm kích hoạt (Activation Function) đưa tính phi tuyến (non-linearity) vào mạng nơ-ron, giúp mạng học được các quan hệ phức tạp.",
                Citation = "Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Hàm kích hoạt trong Deep Learning: Activation Functions (ReLU, Softmax).",
                SourceType = "Extracted", SourceFile = "01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx",
                Options = new() { ("A", "To turn on the computer monitor", false), ("B", "To introduce non-linearity into the network, enabling it to learn complex, non-linear relationships", true), ("C", "To format the hard disk cleanly", false), ("D", "To send emails to the administrator", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_336",
                TopicId = 17, LevelId = 3, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Which port number is standard for encrypted web traffic using HTTPS?",
                Explanation = "Cổng tiêu chuẩn của giao thức bảo mật HTTPS là Port 443. (Cổng HTTP không mã hóa là Port 80).",
                Citation = "Bài học 25 - Mạng máy tính và giao thức: HTTP vs. HTTPS Port 443.",
                SourceType = "Applied", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "80", false), ("B", "443", true), ("C", "21", false), ("D", "22", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_337",
                TopicId = 16, LevelId = 5, QuestionType = "DienKhuyet", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "Fill in the blank: 'Fine-tuning with RLHF (Reinforcement Learning from Human _______) aligns models with human ethical values.'",
                Explanation = "'RLHF' là viết tắt của 'Reinforcement Learning from Human Feedback' (Học tăng cường từ phản hồi của con người).",
                Citation = "Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - Kỹ thuật căn chỉnh mô hình AI: RLHF (Reinforcement Learning from Human Feedback).",
                SourceType = "Extracted", SourceFile = "02_Prompt_Engineering_Principles_for_Developers.pdf",
                Options = new() { ("A", "Feedback", true), ("B", "Finance", false), ("C", "Food", false), ("D", "Force", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_338",
                TopicId = 17, LevelId = 4, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What does 'Latency' measure in distributed client-server systems?",
                Explanation = "'Latency' (Độ trễ) đo lường khoảng thời gian cần thiết để một gói tin dữ liệu truyền từ điểm gửi đến điểm nhận và phản hồi lại.",
                Citation = "Bài học 25 - Hiệu năng hệ thống: Latency and Throughput.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "The weight of the copper networking cables", false), ("B", "The time delay experienced by a data packet traveling from its source to its destination and back", true), ("C", "The brightness of monitor screens", false), ("D", "The number of lines of source code", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_339",
                TopicId = 18, LevelId = 3, QuestionType = "DungSai", Skill = "Noi", Difficulty = "De",
                QuestionText = "True or False: Detailed technical arguments or debugging sessions should be taken 'offline' (discussed after the standup) to respect everyone's time.",
                Explanation = "Đúng. Trong Daily Standup, các cuộc tranh luận sâu về kỹ thuật phải được chuyển ra sau buổi họp ('take it offline') để không làm mất thời gian cả nhóm.",
                Citation = "Bài học 26 - Quy tắc điều phối Standup: 'Take technical debates offline'.",
                SourceType = "Extracted", SourceFile = "BaiHoc_26_DailyStandupScrum.txt", LessonId = 26,
                Options = new() { ("A", "True", true), ("B", "False", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_340",
                TopicId = 16, LevelId = 5, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "In NLP tokenization, what is a 'Token'?",
                Explanation = "'Token' là đơn vị cơ bản nhỏ nhất (từ, phần của từ hoặc ký tự) mà mô hình ngôn ngữ lớn xử lý.",
                Citation = "Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Quá trình Tokenization và Embedding.",
                SourceType = "Extracted", SourceFile = "03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf",
                Options = new() { ("A", "A physical metal coin used in arcades", false), ("B", "The basic building block or sub-word segment that a language model uses to parse and generate text", true), ("C", "A computer antivirus license key", false), ("D", "A database table index", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_341",
                TopicId = 17, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What is the primary role of a 'Load Balancer' in high-traffic web architectures?",
                Explanation = "'Load Balancer' (Bộ cân bằng tải) phân phối lưu lượng truy cập mạng đồng đều giữa nhiều máy chủ hậu cần để tránh quá tải.",
                Citation = "Bài học 25 - Kiến trúc chịu tải cao: Load Balancing and Horizontal Scaling.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "To weigh physical server computers on a scale", false), ("B", "To distribute incoming network traffic across multiple backend servers to ensure high availability and responsiveness", true), ("C", "To turn off unused monitors at night", false), ("D", "To calculate employee payroll", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_342",
                TopicId = 16, LevelId = 4, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "Fill in the blank: 'Transfer learning allows a model pre-trained on a massive dataset to be _______ on a smaller, domain-specific dataset.'",
                Explanation = "'fine-tuned' (tinh chỉnh) mô hình cho bài toán chuyên biệt cụ thể.",
                Citation = "Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Khái niệm Transfer Learning và Fine-tuning.",
                SourceType = "Extracted", SourceFile = "01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx",
                Options = new() { ("A", "fine-tuned", true), ("B", "deleted", false), ("C", "formatted", false), ("D", "rejected", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_343",
                TopicId = 18, LevelId = 3, QuestionType = "TracNghiem", Skill = "Noi", Difficulty = "TrungBinh",
                QuestionText = "Which sentence is best to offer assistance to a teammate facing a blocker during standup?",
                Explanation = "'I can sync with you after this meeting to help troubleshoot that issue.' thể hiện tinh thần hợp tác chuyên nghiệp.",
                Citation = "Bài học 26 - Mẫu câu tương tác hợp tác trong Daily Standup.",
                SourceType = "Extracted", SourceFile = "BaiHoc_26_DailyStandupScrum.txt", LessonId = 26,
                Options = new() { ("A", "You should have known how to code that already.", false), ("B", "I have experience with that service; let's sync right after this standup to troubleshoot together.", true), ("C", "Don't bother me with your bugs.", false), ("D", "Leave the team if you are blocked.", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_344",
                TopicId = 16, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "In the Transformer architecture, what is 'Multi-Head Attention'?",
                Explanation = "'Multi-Head Attention' cho phép mô hình đồng thời chú ý đến thông tin từ các không gian biểu diễn khác nhau ở các vị trí khác nhau.",
                Citation = "Tài liệu 03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf - Cơ chế Multi-Head Attention.",
                SourceType = "Extracted", SourceFile = "03_Research_Paper_Summary_Attention_Is_All_You_Need.pdf",
                Options = new() { ("A", "Having multiple human engineers monitor the code", false), ("B", "Splitting attention into multiple projection heads so the model can simultaneously attend to information from different representation subspaces", true), ("C", "Connecting multiple monitors to one PC", false), ("D", "Printing multiple copies of a paper", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_345",
                TopicId = 17, LevelId = 4, QuestionType = "DungSai", Skill = "TuVung", Difficulty = "De",
                QuestionText = "True or False: In relational databases, a 'Primary Key' column allows NULL values.",
                Explanation = "Sai. Khóa chính (Primary Key) bắt buộc phải là giá trị duy nhất (Unique) và tuyệt đối KHÔNG được chứa giá trị NULL (NOT NULL).",
                Citation = "Bài học 25 - Quy tắc khóa chính trong Database: Unique and NOT NULL.",
                SourceType = "Extracted", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "True", false), ("B", "False", true) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_346",
                TopicId = 16, LevelId = 4, QuestionType = "TracNghiem", Skill = "TuVung", Difficulty = "TrungBinh",
                QuestionText = "What is 'Data Augmentation' in computer vision and machine learning?",
                Explanation = "'Data Augmentation' (Tăng cường dữ liệu) là việc tạo ra các mẫu huấn luyện mới bằng cách xoay, lật, cắt hoặc đổi màu các ảnh hiện có để mô hình học tốt hơn.",
                Citation = "Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Kỹ thuật tiền xử lý dữ liệu: Data Augmentation.",
                SourceType = "Extracted", SourceFile = "01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx",
                Options = new() { ("A", "Buying more server computers", false), ("B", "Artificially increasing the diversity of training data by applying transformations like rotations, flips, and cropping", true), ("C", "Encrypting database backups with passwords", false), ("D", "Deleting duplicate records from Excel", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_347",
                TopicId = 17, LevelId = 3, QuestionType = "DienKhuyet", Skill = "TuVung", Difficulty = "De",
                QuestionText = "Fill in the blank: 'A software _______ fixes unexpected bugs or security vulnerabilities in deployed systems.'",
                Explanation = "'patch' (bản vá lỗi phần mềm).",
                Citation = "Bài học 25 - Vận hành và bảo trì: Security Patches.",
                SourceType = "Applied", SourceFile = "BaiHoc_25_SoftwareEngineeringVocab.txt", LessonId = 25,
                Options = new() { ("A", "patch", true), ("B", "blanket", false), ("C", "curtain", false), ("D", "carpet", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_348",
                TopicId = 18, LevelId = 3, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "De",
                QuestionText = "What does the Scrum metric 'Velocity' track across development sprints?",
                Explanation = "'Velocity' đo lường số lượng đơn vị công việc (Story Points) mà nhóm hoàn thành trung bình trong mỗi Sprint.",
                Citation = "Bài học 26 - Các chỉ số đo lường Sprint: Velocity and Burndown Chart.",
                SourceType = "Extracted", SourceFile = "BaiHoc_26_DailyStandupScrum.txt", LessonId = 26,
                Options = new() { ("A", "The typing speed of developers in words per minute", false), ("B", "The amount of work (in Story Points) a development team completes successfully per sprint cycle", true), ("C", "The internet download speed of the office", false), ("D", "The CPU clock speed of laptops", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_349",
                TopicId = 16, LevelId = 5, QuestionType = "TracNghiem", Skill = "Doc", Difficulty = "Kho",
                QuestionText = "What vulnerability occurs when an attacker crafts malicious input to override an LLM's system instructions?",
                Explanation = "'Prompt Injection' là lỗ hổng bảo mật khi kẻ tấn công chèn lệnh độc hại vào văn bản đầu vào để ép mô hình bỏ qua hướng dẫn hệ thống.",
                Citation = "Tài liệu 02_Prompt_Engineering_Principles_for_Developers.pdf - An ninh bảo mật Prompt: Prompt Injection attacks and defenses.",
                SourceType = "Extracted", SourceFile = "02_Prompt_Engineering_Principles_for_Developers.pdf",
                Options = new() { ("A", "Hardware overheating", false), ("B", "Prompt Injection attack", true), ("C", "Memory leakage", false), ("D", "Disk fragmentation", false) }
            });

            list.Add(new QuestionItem
            {
                SeedId = "SEED_EAI_350",
                TopicId = 16, LevelId = 5, QuestionType = "TraLoiNgan", Skill = "TuVung", Difficulty = "Kho",
                QuestionText = "What does 'NLP' stand for in modern Artificial Intelligence?",
                Explanation = "'NLP' là viết tắt của 'Natural Language Processing' (Xử lý ngôn ngữ tự nhiên).",
                Citation = "Tài liệu 01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx - Định nghĩa các phân ngành AI: NLP (Natural Language Processing).",
                SourceType = "Extracted", SourceFile = "01_Thuat_ngu_Machine_Learning_va_Deep_Learning.docx",
                Options = new() { ("A", "Natural Language Processing", true), ("B", "Network Line Protocol", false), ("C", "New Logic Programming", false), ("D", "Node Level Path", false) }
            });

            return list;
        }
    }
}

