-- ===================================================================
-- SCRIPT BỔ SUNG 135 CÂU HỎI TRẮC NGHIỆM CHO 27 BÀI HỌC HỆ THỐNG
-- 5 CÂU/BÀI CHO 27 BÀI HỌC (ID 0 ĐẾN 26) - BỎ QUA BÀI 27 (VERBS AI TEST)
-- KHÔNG BAO GỒM KỸ NĂNG NGHE AUDIO, ĐẦY ĐỦ 4 LỰA CHỌN VÀ LỜI GIẢI CHI TIẾT
-- ===================================================================
USE TiengAnhAI;
GO
SET NOCOUNT ON;
DECLARE @gvId INT;
SELECT TOP 1 @gvId = MaNguoiDung FROM NguoiDung WHERE MaVaiTro = 2;
IF @gvId IS NULL SET @gvId = 1;
DECLARE @q INT;

-- -------------------------------------------------------------------
-- BÀI HỌC ID 0
-- -------------------------------------------------------------------
-- Q1 (Lesson 0 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'NguPhap', N'Choose the best response: "Hello, my name is Alex. Nice to meet you!"', N'Đáp lại lời chào ''Nice to meet you!'' lịch sự và chuẩn mực nhất là ''Nice to meet you too!''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Nice to meet you too!', 1),
(@q, 'B', N'I am fine, and you?', 0),
(@q, 'C', N'Yes, of course.', 0),
(@q, 'D', N'See you later.', 0);

-- Q2 (Lesson 0 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'NguPhap', N'Complete the sentence: "Where ______ you from?" - "I am from Vietnam."', N'Chủ ngữ ''you'' đi với động từ to be ''are'' trong thì hiện tại đơn.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'is', 0),
(@q, 'B', N'are', 1),
(@q, 'C', N'am', 0),
(@q, 'D', N'do', 0);

-- Q3 (Lesson 0 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'TuVung', N'Which phrase is used to ask for someone''s name politely?', N'''What is your name?'' hoặc ''May I have your name?'' dùng để hỏi tên đối phương.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How are you named?', 0),
(@q, 'B', N'What is your name?', 1),
(@q, 'C', N'Who are you called?', 0),
(@q, 'D', N'Where is your name?', 0);

-- Q4 (Lesson 0 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'NguPhap', N'Choose the correct verb form: "She ______ a first-year college student."', N'Chủ ngữ ngôi thứ ba số ít ''She'' đi với ''is''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'is', 1),
(@q, 'B', N'are', 0),
(@q, 'C', N'be', 0),
(@q, 'D', N'am', 0);

-- Q5 (Lesson 0 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 1, 0, N'TracNghiem', N'TuVung', N'Choose the best introduction for a new team member:', N'Câu giới thiệu tự nhiên và lịch sự: ''Hi, I am David from the Marketing department.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hi, I am David from the Marketing department.', 1),
(@q, 'B', N'Hey, you must know my name is David.', 0),
(@q, 'C', N'Look at me, I work here.', 0),
(@q, 'D', N'My call is David.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 1
-- -------------------------------------------------------------------
-- Q6 (Lesson 1 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'TuVung', N'Which greeting is the most formal in a corporate seminar?', N'''Good morning, ladies and gentlemen'' là lời chào trang trọng chuẩn mực.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey guys, what''s up?', 0),
(@q, 'B', N'Good morning, ladies and gentlemen.', 1),
(@q, 'C', N'Howdy folks!', 0),
(@q, 'D', N'Yo, what is going on?', 0);

-- Q7 (Lesson 1 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'NguPhap', N'In British formal etiquette, what is the customary reply to ''How do you do?''', N'Trong quy chuẩn chào hỏi trang trọng, ''How do you do?'' được đáp lại bằng chính câu ''How do you do?''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How do you do?', 1),
(@q, 'B', N'I do homework every day.', 0),
(@q, 'C', N'I am doing fine, thank you.', 0),
(@q, 'D', N'Yes, I can do it.', 0);

-- Q8 (Lesson 1 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'TuVung', N'Which expression is informal and used between close acquaintances?', N'''Long time no see!'' là câu chào thân mật khi gặp lại bạn bè.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It is an honor to make your acquaintance.', 0),
(@q, 'B', N'Long time no see! How have you been?', 1),
(@q, 'C', N'Allow me to extend my regards.', 0),
(@q, 'D', N'I trust this finds you well.', 0);

-- Q9 (Lesson 1 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'Viet', N'Choose the standard complimentary closing for a formal email:', N'''Sincerely yours'' hoặc ''Best regards'' là lời chào kết trang trọng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Catch ya later!', 0),
(@q, 'B', N'Sincerely yours,', 1),
(@q, 'C', N'Cheers mate,', 0),
(@q, 'D', N'Talk soon,', 0);

-- Q10 (Lesson 1 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 2, 1, N'TracNghiem', N'TuVung', N'How should you answer when a client says ''Pleased to meet you''?', N'''The pleasure is mine'' là cách đáp lễ tinh tế và chuyên nghiệp.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The pleasure is mine, thank you.', 1),
(@q, 'B', N'Never mind about it.', 0),
(@q, 'C', N'You are very welcome.', 0),
(@q, 'D', N'No problem at all.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 2
-- -------------------------------------------------------------------
-- Q11 (Lesson 2 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'TuVung', N'Fill in the blank: "I have been working ______ a software developer for three years."', N'Cấu trúc ''work as + danh từ chỉ nghề nghiệp''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'as', 1),
(@q, 'B', N'like', 0),
(@q, 'C', N'with', 0),
(@q, 'D', N'for', 0);

-- Q12 (Lesson 2 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'TuVung', N'Complete the sentence: "I was born and ______ up in Da Nang city."', N'Cụm ''bring up'' mang nghĩa nuôi nấng, dạng bị động ''brought up'' (lớn lên tại).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'grown', 0),
(@q, 'B', N'brought', 1),
(@q, 'C', N'raised', 0),
(@q, 'D', N'lived', 0);

-- Q13 (Lesson 2 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'TuVung', N'Which phrase expresses dedication to personal interests?', N'''Follow one''s passion'' nghĩa là theo đuổi đam mê cá nhân.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'follow one''s passion', 1),
(@q, 'B', N'call it a day', 0),
(@q, 'C', N'bite the bullet', 0),
(@q, 'D', N'break the ice', 0);

-- Q14 (Lesson 2 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'NguPhap', N'Choose the correct relative pronoun: "Da Lat, ______ is known as the city of flowers, attracts many visitors."', N'Mệnh đề quan hệ không xác định bổ nghĩa cho danh từ riêng dùng ''which''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'which', 1),
(@q, 'B', N'that', 0),
(@q, 'C', N'where', 0),
(@q, 'D', N'whose', 0);

-- Q15 (Lesson 2 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (2, 3, 2, N'TracNghiem', N'Doc', N'In a career bio, what does ''drive for continuous learning'' signify?', N'Nghĩa là tinh thần chủ động học hỏi và trau dồi kiến thức không ngừng.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A proactive commitment to expanding knowledge and skills.', 1),
(@q, 'B', N'Traveling frequently for vacation.', 0),
(@q, 'C', N'Avoiding new technology tools.', 0),
(@q, 'D', N'Following orders blindly.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 3
-- -------------------------------------------------------------------
-- Q16 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'What should you say to reserve a table for four tonight?', N'Mẫu câu đặt bàn chuẩn: ''I would like to book a table for four at 7 PM.''', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I want food for four people.', 0),
(@q, 'B', N'I would like to book a table for four at 7 PM.', 1),
(@q, 'C', N'Give me four chairs tonight.', 0),
(@q, 'D', N'Are there four people here?', 0);

-- Q17 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'Which term refers to a dish served before the main course?', N'''Appetizer'' hoặc ''starter'' là món khai vị.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Dessert', 0),
(@q, 'B', N'Appetizer', 1),
(@q, 'C', N'Beverage', 0),
(@q, 'D', N'Condiment', 0);

-- Q18 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'How do you ask for the bill politely in a restaurant?', N'Cách xin hóa đơn thông dụng: ''Could we have the bill, please?''', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How much money now?', 0),
(@q, 'B', N'Give me the payment paper.', 0),
(@q, 'C', N'Could we have the bill, please?', 1),
(@q, 'D', N'I take the money away.', 0);

-- Q19 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'If you are allergic to peanuts, what should you notify the waiter?', N'''I have a severe peanut allergy; does this dish contain nuts?''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I hate peanuts so much.', 0),
(@q, 'B', N'I have a severe peanut allergy; does this dish contain nuts?', 1),
(@q, 'C', N'Peanuts are not good today.', 0),
(@q, 'D', N'Do not show me peanuts.', 0);

-- Q20 (Lesson 3 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 2, 3, N'TracNghiem', N'TuVung', N'How would you order a steak cooked completely through?', N'''Well-done'' là mức độ thịt bò nướng chín kỹ hoàn toàn.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Rare', 0),
(@q, 'B', N'Medium-rare', 0),
(@q, 'C', N'Well-done', 1),
(@q, 'D', N'Raw', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 4
-- -------------------------------------------------------------------
-- Q21 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'Where can customers try on clothes before buying in a shop?', N'''Fitting room'' hoặc ''changing room'' là phòng thử đồ.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Storage room', 0),
(@q, 'B', N'Fitting room', 1),
(@q, 'C', N'Checkout counter', 0),
(@q, 'D', N'Parking lot', 0);

-- Q22 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'What document must you present to request an item exchange or refund?', N'''Receipt'' là hóa đơn thanh toán cần có khi đổi trả hàng.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Receipt', 1),
(@q, 'B', N'Menu', 0),
(@q, 'C', N'Passport', 0),
(@q, 'D', N'Brochure', 0);

-- Q23 (Lesson 4 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'NguPhap', N'Complete the question: "Do you have this shirt ______ a larger size?"', N'Giới từ ''in'' đi với kích cỡ: in a larger size / in size M.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'on', 0),
(@q, 'B', N'at', 0),
(@q, 'C', N'in', 1),
(@q, 'D', N'by', 0);

-- Q24 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'What does ''out of stock'' mean?', N'''Out of stock'' nghĩa là tạm thời hết hàng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Available at a discount', 0),
(@q, 'B', N'Temporarily unavailable in the store', 1),
(@q, 'C', N'Damaged and unsellable', 0),
(@q, 'D', N'Made of organic material', 0);

-- Q25 (Lesson 4 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 1, 4, N'TracNghiem', N'TuVung', N'Which question politely asks the price of an item?', N'''How much is this jacket?'' là câu hỏi giá phổ biến và chính xác.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'How many is this jacket?', 0),
(@q, 'B', N'How much is this jacket?', 1),
(@q, 'C', N'What cost does this jacket?', 0),
(@q, 'D', N'Which money is this jacket?', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 5
-- -------------------------------------------------------------------
-- Q26 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'Which adjective describes food that has a pleasant salty or spicy taste, not sweet?', N'''Savory'' (hoặc savoury) mang nghĩa có vị mặn đậm đà hấp dẫn.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Bitter', 0),
(@q, 'B', N'Savory', 1),
(@q, 'C', N'Bland', 0),
(@q, 'D', N'Sour', 0);

-- Q27 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'What does ''mouth-watering'' describe in culinary reviews?', N'''Mouth-watering'' miêu tả món ăn thơm ngon hấp dẫn đến ứa nước miếng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Extremely appealing and delicious-smelling', 1),
(@q, 'B', N'Too salty to swallow', 0),
(@q, 'C', N'Overly dry and tough', 0),
(@q, 'D', N'Completely tasteless', 0);

-- Q28 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'Complete the sentence: "The soup is ______ with star anise and fresh herbs."', N'Cụm ''seasoned with'' nghĩa là được nêm nếm bằng gia vị.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'spilled', 0),
(@q, 'B', N'seasoned', 1),
(@q, 'C', N'painted', 0),
(@q, 'D', N'boiled', 0);

-- Q29 (Lesson 5 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'Doc', N'Street vendors are famous for preserving ______ family recipes across generations.', N'Từ ''authentic'' nghĩa là chuẩn vị, đích thực truyền thống.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'artificial', 0),
(@q, 'B', N'authentic', 1),
(@q, 'C', N'synthetic', 0),
(@q, 'D', N'reluctant', 0);

-- Q30 (Lesson 5 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (3, 3, 5, N'TracNghiem', N'TuVung', N'Which word describes a texture that makes a sharp sound when bitten into?', N'''Crispy'' (giòn rụm) tạo âm thanh giòn tan khi cắn.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Soggy', 0),
(@q, 'B', N'Crispy', 1),
(@q, 'C', N'Greasy', 0),
(@q, 'D', N'Chewy', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 6
-- -------------------------------------------------------------------
-- Q31 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'What is the document given to a passenger that allows them to board the airplane?', N'''Boarding pass'' là thẻ lên máy bay.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Boarding pass', 1),
(@q, 'B', N'Customs declaration', 0),
(@q, 'C', N'Flight manual', 0),
(@q, 'D', N'Luggage tag', 0);

-- Q32 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'What is luggage that passengers are allowed to take inside the cabin called?', N'''Carry-on luggage'' (hoặc hand luggage) là hành lý xách tay mang lên khoang máy bay.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Checked baggage', 0),
(@q, 'B', N'Carry-on luggage', 1),
(@q, 'C', N'Cargo shipment', 0),
(@q, 'D', N'Excess parcel', 0);

-- Q33 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'When going through airport security, you must place metallic items into a ______.', N'''Tray'' (khay nhựa) dùng để đựng đồ kim loại qua máy quét an ninh.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'trash bin', 0),
(@q, 'B', N'tray', 1),
(@q, 'C', N'suitcase', 0),
(@q, 'D', N'drawer', 0);

-- Q34 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'Which term refers to a flight that does not stop between departure and destination?', N'''Direct flight'' hoặc ''non-stop flight'' là chuyến bay thẳng.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Layover flight', 0),
(@q, 'B', N'Non-stop flight', 1),
(@q, 'C', N'Connecting flight', 0),
(@q, 'D', N'Delayed flight', 0);

-- Q35 (Lesson 6 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 2, 6, N'TracNghiem', N'TuVung', N'What does ''excess baggage fee'' mean?', N'Phí hành lý quá cước khi vượt trọng lượng quy định.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A discount for booking early', 0),
(@q, 'B', N'A charge for luggage exceeding the weight limit', 1),
(@q, 'C', N'A fee for changing flight dates', 0),
(@q, 'D', N'Insurance for lost items', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 7
-- -------------------------------------------------------------------
-- Q36 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'Choose the best question to ask a stranger for directions:', N'''Excuse me, could you tell me how to get to the nearest metro station?''', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey, where is the train right now?', 0),
(@q, 'B', N'Excuse me, could you tell me how to get to the nearest metro station?', 1),
(@q, 'C', N'You tell me the way immediately.', 0),
(@q, 'D', N'Train station location now please.', 0);

-- Q37 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'Where do passengers wait for their train in a subway station?', N'''Platform'' (sân ga/ ke ga) là nơi hành khách chờ tàu.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Runway', 0),
(@q, 'B', N'Platform', 1),
(@q, 'C', N'Intersection', 0),
(@q, 'D', N'Roundabout', 0);

-- Q38 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'What does ''change lines at Central Station'' mean?', N'Nghĩa là chuyển sang tuyến tàu khác tại ga trung tâm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Buy a new ticket', 0),
(@q, 'B', N'Transfer to a different subway route', 1),
(@q, 'C', N'Exit the station immediately', 0),
(@q, 'D', N'Wait for tomorrow''s train', 0);

-- Q39 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'Complete the sentence: "Walk straight for two blocks, then turn ______ at the traffic lights."', N'Cụm chỉ hướng: ''turn left'' (rẽ trái) hoặc ''turn right''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'left', 1),
(@q, 'B', N'up', 0),
(@q, 'C', N'down', 0),
(@q, 'D', N'out', 0);

-- Q40 (Lesson 7 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (4, 1, 7, N'TracNghiem', N'TuVung', N'If a destination is ''within walking distance'', it means:', N'''Within walking distance'' nghĩa là ở rất gần, có thể đi bộ được.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'You must take a taxi', 0),
(@q, 'B', N'It is close enough to reach on foot', 1),
(@q, 'C', N'It is located in another city', 0),
(@q, 'D', N'The road is completely blocked', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 8
-- -------------------------------------------------------------------
-- Q41 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'What is the primary objective of ''skimming'' in an academic reading test?', N'Mục tiêu của skimming là đọc lướt nhanh để nắm được ý chính và bố cục toàn bài.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'To locate specific dates and numbers', 0),
(@q, 'B', N'To read quickly to comprehend the general gist and layout', 1),
(@q, 'C', N'To translate every sentence word by word', 0),
(@q, 'D', N'To memorize technical jargon', 0);

-- Q42 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'When applying the ''scanning'' technique, a reader should:', N'Kỹ thuật scanning tập trung tìm kiếm thông tin cụ thể (từ khóa, con số, tên riêng).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Focus on identifying specific keywords, names, or figures without reading whole paragraphs.', 1),
(@q, 'B', N'Analyze the author''s philosophical viewpoint.', 0),
(@q, 'C', N'Underline every unfamiliar vocabulary item.', 0),
(@q, 'D', N'Read aloud to improve pronunciation.', 0);

-- Q43 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'Where is the topic sentence of an academic paragraph typically located?', N'Câu chủ đề của đoạn văn thường nằm ở câu đầu tiên hoặc câu thứ hai.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'In the very first or second sentence', 1),
(@q, 'B', N'Strictly inside the footnotes', 0),
(@q, 'C', N'Always hidden in the exact middle', 0),
(@q, 'D', N'Only in the bibliography', 0);

-- Q44 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'Why is identifying synonyms crucial for IELTS Reading questions?', N'Vì câu hỏi trong bài đọc IELTS luôn được diễn đạt lại (paraphrased) bằng từ đồng nghĩa.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Questions almost always paraphrase information from the text rather than using identical words.', 1),
(@q, 'B', N'Synonyms are required to be memorized for speaking tests.', 0),
(@q, 'C', N'Texts contain no grammatical conjunctions.', 0),
(@q, 'D', N'IELTS examiners penalize short answers.', 0);

-- Q45 (Lesson 8 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 8, N'TracNghiem', N'Doc', N'What should a test-taker do when encountering an unfamiliar scientific term?', N'Dựa vào ngữ cảnh xung quanh để suy đoán nghĩa khái quát thay vì dừng lại quá lâu.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Stop the test and consult a dictionary', 0),
(@q, 'B', N'Deduce its rough meaning from contextual clues and move forward', 1),
(@q, 'C', N'Assume the entire paragraph is erroneous', 0),
(@q, 'D', N'Leave all subsequent questions blank', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 9
-- -------------------------------------------------------------------
-- Q46 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'A statement in IELTS Reading should be marked ''FALSE'' when:', N'Chọn FALSE khi thông tin trong câu hỏi đối lập, mâu thuẫn trực tiếp với bài đọc.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The passage mentions no related information whatsoever.', 0),
(@q, 'B', N'The statement directly contradicts the facts stated in the passage.', 1),
(@q, 'C', N'The author''s tone is slightly ambiguous.', 0),
(@q, 'D', N'The passage was written over a decade ago.', 0);

-- Q47 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'A statement should be evaluated as ''NOT GIVEN'' if:', N'Chọn NOT GIVEN khi bài đọc không có đủ thông tin để xác nhận câu đó đúng hay sai.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The passage provides insufficient information to confirm or refute the claim.', 1),
(@q, 'B', N'The author explicitly denies the statement.', 0),
(@q, 'C', N'The claim uses simple past tense.', 0),
(@q, 'D', N'The vocabulary is too elementary.', 0);

-- Q48 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'How do absolute qualifiers such as ''always'', ''all'', or ''never'' affect statement validity?', N'Các từ tuyệt đối (all, always) thường biến câu thành FALSE nếu bài đọc chỉ nói ở mức độ đa số hoặc có ngoại lệ.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'They usually make a statement FALSE if the text only suggests a tendency with exceptions.', 1),
(@q, 'B', N'They guarantee the statement is always TRUE.', 0),
(@q, 'C', N'They turn every question into a multiple-choice item.', 0),
(@q, 'D', N'They are completely ignored by examiners.', 0);

-- Q49 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'Text: "The vaccine showed 90% efficacy in lab trials." Statement: "The vaccine guarantees complete immunity in all humans." Answer:', N'Bài đọc chỉ nói 90% trong phòng thí nghiệm, câu hỏi khẳng định ''guarantee complete immunity in all'' -> mâu thuẫn -> FALSE.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'TRUE', 0),
(@q, 'B', N'FALSE', 1),
(@q, 'C', N'NOT GIVEN', 0),
(@q, 'D', N'CANNOT DETERMINE', 0);

-- Q50 (Lesson 9 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (6, 4, 9, N'TracNghiem', N'Doc', N'Text: "Dr. Miller graduated from Cambridge in 1998." Statement: "Dr. Miller was the top student in his graduating class." Answer:', N'Bài đọc chỉ nói tốt nghiệp năm 1998, không nhắc đến việc có đứng đầu lớp hay không -> NOT GIVEN.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'TRUE', 0),
(@q, 'B', N'FALSE', 0),
(@q, 'C', N'NOT GIVEN', 1),
(@q, 'D', N'YES', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 10
-- -------------------------------------------------------------------
-- Q51 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'What must an effective introduction of an IELTS Opinion Essay contain?', N'Mở bài hiệu quả phải có câu diễn giải đề (paraphrase) và luận đề rõ ràng (thesis statement).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A paraphrase of the prompt and a clear thesis statement stating the author''s stance.', 1),
(@q, 'B', N'Ten rhetorical questions without any direct opinion.', 0),
(@q, 'C', N'A detailed list of all statistics in the world.', 0),
(@q, 'D', N'A quotation from an ancient philosopher only.', 0);

-- Q52 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'Which cohesive device expresses a concession in an argumentative essay?', N'''Admittedly'' hoặc ''While it is true that'' dùng để nhượng bộ một quan điểm đối lập trước khi phản bác.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Furthermore', 0),
(@q, 'B', N'Admittedly', 1),
(@q, 'C', N'Consequently', 0),
(@q, 'D', N'In addition', 0);

-- Q53 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'What is the primary role of a counter-argument paragraph in high-band writing?', N'Phân tích góc nhìn đối lập rồi phản bác để chứng minh tính thuyết phục vượt trội của lập luận chính.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'To confuse the reader about the author''s real stance', 0),
(@q, 'B', N'To acknowledge opposing views and refute them to reinforce the core thesis', 1),
(@q, 'C', N'To lengthen the essay without adding substance', 0),
(@q, 'D', N'To replace the conclusion section', 0);

-- Q54 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'Which sentence exhibits sophisticated academic style avoiding first-person overuse?', N'Phong cách học thuật khách quan: ''It is widely contended that technological automation enhances economic productivity.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I think tech makes money fast.', 0),
(@q, 'B', N'It is widely contended that technological automation enhances economic productivity.', 1),
(@q, 'C', N'You should believe that robots are awesome.', 0),
(@q, 'D', N'Me and my friends agree computers work well.', 0);

-- Q55 (Lesson 10 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 4, 10, N'TracNghiem', N'Viet', N'What should NEVER appear in the conclusion of an academic essay?', N'Tuyệt đối không đưa ý tưởng hoặc luận điểm hoàn toàn mới chưa được phân tích vào phần kết bài.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A restatement of the thesis', 0),
(@q, 'B', N'Brand-new arguments that were not discussed in the body paragraphs', 1),
(@q, 'C', N'A summary of main body arguments', 0),
(@q, 'D', N'A concluding recommendation', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 11
-- -------------------------------------------------------------------
-- Q56 (Lesson 11 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'Viet', N'Which collocation describes the rapid, uncontrolled expansion of metropolitan boundaries?', N'''Urban sprawl'' là thuật ngữ chỉ sự mở rộng tự phát, thiếu kiểm soát của các khu đô thị.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Urban sprawl', 1),
(@q, 'B', N'City shrinking', 0),
(@q, 'C', N'Rural stagnation', 0),
(@q, 'D', N'Metropolitan contraction', 0);

-- Q57 (Lesson 11 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'TuVung', N'Select the academic verb meaning ''to reduce the severity or impact of climate disruption'':', N'''Mitigate'' nghĩa là giảm thiểu tác động tiêu cực.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Exacerbate', 0),
(@q, 'B', N'Mitigate', 1),
(@q, 'C', N'Proliferate', 0),
(@q, 'D', N'Deteriorate', 0);

-- Q58 (Lesson 11 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'Viet', N'Choose the best academic sentence proposing a governmental policy for traffic congestion:', N'''Governments should implement congestion pricing schemes to disincentivize private vehicle usage in city centers.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Governments should implement congestion pricing schemes to disincentivize private vehicle usage.', 1),
(@q, 'B', N'Make drivers pay big cash so they stop driving cars.', 0),
(@q, 'C', N'Stop all automobiles from entering roads immediately.', 0),
(@q, 'D', N'People should walk more because cars are annoying.', 0);

-- Q59 (Lesson 11 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'TuVung', N'What does ''sustainable development'' encompass in urban planning?', N'Phát triển bền vững: đáp ứng nhu cầu hiện tại mà không làm tổn hại đến tài nguyên của các thế hệ tương lai.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Meeting current societal needs without compromising the ability of future generations to meet theirs.', 1),
(@q, 'B', N'Exploiting all oil reserves as swiftly as possible.', 0),
(@q, 'C', N'Banning all commercial activities in major cities.', 0),
(@q, 'D', N'Building skyscrapers regardless of ecological damage.', 0);

-- Q60 (Lesson 11 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (7, 5, 11, N'TracNghiem', N'Viet', N'Complete the sentence with an appropriate transition: "Heavy reliance on fossil fuels degrades air quality; ______, it accelerates global warming."', N'''Furthermore'' hoặc ''Moreover'' dùng để bổ sung luận điểm cùng chiều.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Nevertheless', 0),
(@q, 'B', N'Furthermore', 1),
(@q, 'C', N'In contrast', 0),
(@q, 'D', N'Conversely', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 12
-- -------------------------------------------------------------------
-- Q61 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'In university group discussions, what is a ''rubric''?', N'''Rubric'' là bảng tiêu chí đánh giá và chấm điểm bài tập học thuật.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A grading criterion guide detailing assessment expectations.', 1),
(@q, 'B', N'A certificate of attendance.', 0),
(@q, 'C', N'A break time between lectures.', 0),
(@q, 'D', N'A list of campus restaurants.', 0);

-- Q62 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'Which phrase expresses polite academic disagreement between team members?', N'''I take your point, but we might also consider...'' thể hiện sự phản biện lịch sự, tôn trọng đối phương.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'You are totally wrong about everything.', 0),
(@q, 'B', N'I take your point, but we might also consider another angle.', 1),
(@q, 'C', N'Stop talking nonsense.', 0),
(@q, 'D', N'That idea makes no sense whatsoever.', 0);

-- Q63 (Lesson 12 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'Doc', N'What does ''peer review'' refer to in collaborative academic projects?', N'Đánh giá chéo: sinh viên nhận xét và góp ý phản biện bài làm của bạn cùng nhóm/lớp.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Evaluation of work by peers possessing similar competencies.', 1),
(@q, 'B', N'Grading conducted exclusively by the university president.', 0),
(@q, 'C', N'Submitting essays to a public newspaper.', 0),
(@q, 'D', N'Writing exams without supervision.', 0);

-- Q64 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'If a student suggests ''dividing the workload evenly'', what do they intend?', N'Họ đề xuất phân chia khối lượng công việc công bằng cho từng thành viên trong nhóm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Assigning equal responsibilities to each team member.', 1),
(@q, 'B', N'Leaving the entire assignment to one individual.', 0),
(@q, 'C', N'Postponing the project until next semester.', 0),
(@q, 'D', N'Asking the professor to write the report.', 0);

-- Q65 (Lesson 12 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 4, 12, N'TracNghiem', N'TuVung', N'What is an ''executive summary'' in a project portfolio?', N'''Executive summary'' là bản tóm tắt súc tích các kết quả và điểm then chốt của dự án.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A brief overview synthesizing the key findings and recommendations.', 1),
(@q, 'B', N'A list of all team member phone numbers.', 0),
(@q, 'C', N'The final receipt for printing the document.', 0),
(@q, 'D', N'A biography of corporate executives.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 13
-- -------------------------------------------------------------------
-- Q66 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'In academic monologues, what function do ''signposting phrases'' serve?', N'Cụm từ báo hiệu giúp người nghe định hướng cấu trúc và bước chuyển ý trong bài giảng.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'They orient the listener regarding the structure and transitions of the lecture.', 1),
(@q, 'B', N'They indicate grammatical errors in the textbook.', 0),
(@q, 'C', N'They are used to fill silent pauses when the speaker forgets words.', 0),
(@q, 'D', N'They signal the end of the semester.', 0);

-- Q67 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'When a lecturer states: "Turning now to the empirical evidence...", what are they doing?', N'Diễn giả đang chuyển tiếp sang phần phân tích dữ liệu thực nghiệm.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Transitioning to discuss observational and experimental data.', 1),
(@q, 'B', N'Ending the lecture abruptly.', 0),
(@q, 'C', N'Asking the students to close their laptops.', 0),
(@q, 'D', N'Repeating the introductory slide.', 0);

-- Q68 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'Which phrase marks a critical emphasis or surprising revelation in scientific speech?', N'''Crucially'' hoặc ''What is particularly noteworthy is that...''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Incidentally', 0),
(@q, 'B', N'What is particularly noteworthy is that...', 1),
(@q, 'C', N'As a trivial aside', 0),
(@q, 'D', N'Needless to mention', 0);

-- Q69 (Lesson 13 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'Doc', N'What distinguishes an ''empirical study'' from a purely theoretical hypothesis?', N'Nghiên cứu thực nghiệm dựa trên việc quan sát và thu thập dữ liệu thực tế, không chỉ là suy luận lý thuyết.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It is grounded on verifiable observation and measurable data collection.', 1),
(@q, 'B', N'It uses fictional stories instead of numbers.', 0),
(@q, 'C', N'It requires no verification by other scientists.', 0),
(@q, 'D', N'It cannot be published in journals.', 0);

-- Q70 (Lesson 13 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (8, 5, 13, N'TracNghiem', N'TuVung', N'Select the word synonymous with ''ubiquitous'' in academic discourse:', N'''Ubiquitous'' = ''omnipresent'' (phổ biến khắp mọi nơi).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Rare', 0),
(@q, 'B', N'Omnipresent', 1),
(@q, 'C', N'Obsolete', 0),
(@q, 'D', N'Ephemeral', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 14
-- -------------------------------------------------------------------
-- Q71 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'Choose the correct auxiliary verb: "They ______ lived in London since 2015."', N'Chủ ngữ ''They'' đi với trợ động từ ''have'' trong thì hiện tại hoàn thành.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'has', 0),
(@q, 'B', N'have', 1),
(@q, 'C', N'had', 0),
(@q, 'D', N'are', 0);

-- Q72 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'Complete the sentence: "I haven''t submitted the quarterly report ______."', N'Trong câu phủ định của hiện tại hoàn thành, từ ''yet'' thường đặt ở cuối câu.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'already', 0),
(@q, 'B', N'yet', 1),
(@q, 'C', N'since', 0),
(@q, 'D', N'just', 0);

-- Q73 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'What is the difference between ''have been to'' and ''have gone to''?', N'''have been to'' = đã đến và đã trở về; ''have gone to'' = đã đi và hiện vẫn đang ở đó chưa về.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'''have been to'' means visited and returned; ''have gone to'' means departed and not yet back.', 1),
(@q, 'B', N'''have been to'' is past tense; ''have gone to'' is future tense.', 0),
(@q, 'C', N'Both phrases are completely interchangeable in all contexts.', 0),
(@q, 'D', N'''have gone to'' can only be used with first-person singular.', 0);

-- Q74 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'Which preposition correctly completes: "She has worked here ______ five years."', N'Dùng ''for'' với khoảng thời gian (for five years); dùng ''since'' với mốc thời gian.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'since', 0),
(@q, 'B', N'for', 1),
(@q, 'C', N'during', 0),
(@q, 'D', N'at', 0);

-- Q75 (Lesson 14 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 3, 14, N'TracNghiem', N'NguPhap', N'"Have you ______ tasted authentic Italian risotto?"', N'''Ever'' thường dùng trong câu hỏi thì hiện tại hoàn thành để hỏi về trải nghiệm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'never', 0),
(@q, 'B', N'ever', 1),
(@q, 'C', N'already', 0),
(@q, 'D', N'still', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 15
-- -------------------------------------------------------------------
-- Q76 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"While we ______ dinner, the power suddenly went out."', N'Hành động đang diễn ra trong quá khứ dùng quá khứ tiếp diễn (were having), hành động chen ngang dùng quá khứ đơn (went out).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'had', 0),
(@q, 'B', N'were having', 1),
(@q, 'C', N'have had', 0),
(@q, 'D', N'are having', 0);

-- Q77 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"She ______ to Tokyo last summer on a business assignment."', N'Thời gian xác định trong quá khứ ''last summer'' chia thì quá khứ đơn (went).', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'goes', 0),
(@q, 'B', N'went', 1),
(@q, 'C', N'has gone', 0),
(@q, 'D', N'was going', 0);

-- Q78 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"At 8:00 PM yesterday, what ______ you doing?"', N'Câu hỏi thời điểm cụ thể đang diễn ra trong quá khứ chia quá khứ tiếp diễn: were you doing.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'did', 0),
(@q, 'B', N'were', 1),
(@q, 'C', N'are', 0),
(@q, 'D', N'have', 0);

-- Q79 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'"He ______ his leg while he was playing football on Sunday."', N'Hành động gãy chân chen ngang hành động đang chơi bóng đá, chia quá khứ đơn (broke).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'broke', 1),
(@q, 'B', N'was breaking', 0),
(@q, 'C', N'breaks', 0),
(@q, 'D', N'has broken', 0);

-- Q80 (Lesson 15 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 2, 15, N'TracNghiem', N'NguPhap', N'Which conjunction commonly introduces a past continuous clause for simultaneous background actions?', N'''While'' thường dùng để mở đầu mệnh đề quá khứ tiếp diễn chỉ hành động diễn ra song song.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'While', 1),
(@q, 'B', N'Immediately', 0),
(@q, 'C', N'Soon', 0),
(@q, 'D', N'Unless', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 16
-- -------------------------------------------------------------------
-- Q81 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"Look at those dark clouds! It ______ rain."', N'Dự đoán có bằng chứng cụ thể trước mắt (những đám mây đen) dùng ''is going to''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 0),
(@q, 'B', N'is going to', 1),
(@q, 'C', N'rains', 0),
(@q, 'D', N'shall', 0);

-- Q82 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"The phone is ringing." - "Don''t worry, I ______ answer it."', N'Quyết định đưa ra ngay tại thời điểm nói (spontaneous decision) dùng ''will''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 1),
(@q, 'B', N'am going to', 0),
(@q, 'C', N'answer', 0),
(@q, 'D', N'shall to', 0);

-- Q83 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"We have already booked the flight. We ______ visit our grandparents next week."', N'Kế hoạch đã chuẩn bị và sắp xếp trước (đã đặt vé) dùng ''are going to''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 0),
(@q, 'B', N'are going to', 1),
(@q, 'C', N'visit', 0),
(@q, 'D', N'might to', 0);

-- Q84 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'"I promise I ______ keep your secret confidential."', N'Lời hứa (promise) dùng ''will''.', N'De', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'will', 1),
(@q, 'B', N'am going to', 0),
(@q, 'C', N'kept', 0),
(@q, 'D', N'going to', 0);

-- Q85 (Lesson 16 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (10, 1, 16, N'TracNghiem', N'NguPhap', N'Which sentence indicates a pre-arranged personal decision?', N'''I am going to study abroad next autumn; I''ve already received the acceptance letter.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I will probably study tomorrow maybe.', 0),
(@q, 'B', N'I am going to study abroad next autumn; I''ve already received the acceptance letter.', 1),
(@q, 'C', N'I think I will go buy some milk right now.', 0),
(@q, 'D', N'I shall see about it.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 17
-- -------------------------------------------------------------------
-- Q86 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Reduce the clause: "The engineers who are designing the new bridge have extensive experience."', N'Mệnh đề chủ động rút gọn bằng V-ing: ''The engineers designing the new bridge...''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The engineers designed the new bridge...', 0),
(@q, 'B', N'The engineers designing the new bridge...', 1),
(@q, 'C', N'The engineers to design the new bridge...', 0),
(@q, 'D', N'The engineers being designed...', 0);

-- Q87 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Reduce the clause: "The vehicles which were damaged in the hail storm have been towed away."', N'Mệnh đề bị động rút gọn bằng V-ed/V3: ''The vehicles damaged in the hail storm...''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The vehicles damaged in the hail storm...', 1),
(@q, 'B', N'The vehicles damaging in the hail storm...', 0),
(@q, 'C', N'The vehicles to damage in the hail storm...', 0),
(@q, 'D', N'The vehicles have damaged...', 0);

-- Q88 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Complete the sentence: "Neil Armstrong was the first person ______ on the surface of the Moon."', N'Sau các từ chỉ thứ tự ''the first'', ''the second'', ''the only'', rút gọn mệnh đề quan hệ bằng ''to + V-infinitive''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'stepping', 0),
(@q, 'B', N'to step', 1),
(@q, 'C', N'stepped', 0),
(@q, 'D', N'had stepped', 0);

-- Q89 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'"The document ______ by the legal team yesterday must be signed immediately."', N'Mệnh đề rút gọn bị động: ''reviewed by the legal team''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'reviewing', 0),
(@q, 'B', N'reviewed', 1),
(@q, 'C', N'to review', 0),
(@q, 'D', N'was reviewed', 0);

-- Q90 (Lesson 17 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 4, 17, N'TracNghiem', N'NguPhap', N'Which sentence correctly uses a perfect participle clause indicating an earlier completed action?', N'''Having finished all testing procedures, the software team deployed the update.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Finished all testing procedures, the software team deployed the update.', 0),
(@q, 'B', N'Having finished all testing procedures, the software team deployed the update.', 1),
(@q, 'C', N'To have finish all testing procedures, the team deployed.', 0),
(@q, 'D', N'Being finish all testing procedures, the team deployed.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 18
-- -------------------------------------------------------------------
-- Q91 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'"People say that he owns three software companies." -> Passive: "He is said ______ three software companies."', N'Cấu trúc bị động với động từ tường thuật: He is said to own...', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'owning', 0),
(@q, 'B', N'to own', 1),
(@q, 'C', N'owned', 0),
(@q, 'D', N'to have owned', 0);

-- Q92 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'"People believe that the ancient city was destroyed by an earthquake." -> "The ancient city is believed ______ by an earthquake."', N'Hành động bị phá hủy xảy ra trước thời điểm tin tưởng, dùng ''to have been destroyed''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'to be destroyed', 0),
(@q, 'B', N'to have been destroyed', 1),
(@q, 'C', N'destroying', 0),
(@q, 'D', N'was destroyed', 0);

-- Q93 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'Causative form: "I need to have my car ______ before taking the long road trip."', N'Cấu trúc truyền khiến: ''have something done'' (nhờ/thuê ai làm gì đó cho mình).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'repair', 0),
(@q, 'B', N'repaired', 1),
(@q, 'C', N'repairing', 0),
(@q, 'D', N'to repair', 0);

-- Q94 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'"The manager got the contractor ______ the blueprints by Friday."', N'Cấu trúc truyền khiến: ''get someone to do something''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'finish', 0),
(@q, 'B', N'to finish', 1),
(@q, 'C', N'finished', 0),
(@q, 'D', N'finishing', 0);

-- Q95 (Lesson 18 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (11, 3, 18, N'TracNghiem', N'NguPhap', N'Complete the impersonal passive: "______ is anticipated that consumer spending will rebound this quarter."', N'Chủ ngữ giả ''It'' trong cấu trúc bị động khách quan: It is anticipated that...', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It', 1),
(@q, 'B', N'There', 0),
(@q, 'C', N'What', 0),
(@q, 'D', N'Which', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 19
-- -------------------------------------------------------------------
-- Q96 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Type 1: "If the weather ______ favorable tomorrow, we will launch the drone survey."', N'Mệnh đề If của câu điều kiện loại 1 chia thì hiện tại đơn (is).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'is', 1),
(@q, 'B', N'will be', 0),
(@q, 'C', N'were', 0),
(@q, 'D', N'had been', 0);

-- Q97 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Type 2: "If I ______ in your position, I would renegotiate the contractual terms."', N'Câu điều kiện loại 2 giả định trái với hiện tại, to be chia ''were'' cho tất cả các ngôi.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'am', 0),
(@q, 'B', N'were', 1),
(@q, 'C', N'have been', 0),
(@q, 'D', N'had been', 0);

-- Q98 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Type 3: "If they ______ the backup server, they wouldn''t have lost client records."', N'Câu điều kiện loại 3 giả định trái với quá khứ: If + had + P2.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'configured', 0),
(@q, 'B', N'had configured', 1),
(@q, 'C', N'would configure', 0),
(@q, 'D', N'were configuring', 0);

-- Q99 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'Mixed conditional (Past condition -> Present result): "If she ______ French in college, she would understand this report today."', N'Điều kiện trong quá khứ (had studied) dẫn đến kết quả ở hiện tại (would understand today).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'studied', 0),
(@q, 'B', N'had studied', 1),
(@q, 'C', N'studies', 0),
(@q, 'D', N'would study', 0);

-- Q100 (Lesson 19 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 3, 19, N'TracNghiem', N'NguPhap', N'"______ you submit the required permits on time, your construction will proceed smoothly."', N'''Provided that'' (hoặc ''Providing that'') mang nghĩa ''với điều kiện là / miễn là''.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Unless', 0),
(@q, 'B', N'Provided that', 1),
(@q, 'C', N'Although', 0),
(@q, 'D', N'In spite of', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 20
-- -------------------------------------------------------------------
-- Q101 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'"Never in my career ______ such negligence in quality control."', N'Đảo ngữ với ''Never'': Trợ động từ đứng trước chủ ngữ: have I witnessed.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I have witnessed', 0),
(@q, 'B', N'have I witnessed', 1),
(@q, 'C', N'I witnessed', 0),
(@q, 'D', N'did I witnessed', 0);

-- Q102 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'"Not only ______ the deadline, but they also exceeded all benchmark metrics."', N'Đảo ngữ với ''Not only'': Not only did they meet...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'they met', 0),
(@q, 'B', N'did they meet', 1),
(@q, 'C', N'have they met', 0),
(@q, 'D', N'they were meeting', 0);

-- Q103 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'"Under no circumstances ______ confidential credentials be shared across public channels."', N'Đảo ngữ với ''Under no circumstances'': should confidential credentials be...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'confidential credentials should', 0),
(@q, 'B', N'should confidential credentials', 1),
(@q, 'C', N'they will', 0),
(@q, 'D', N'credentials would', 0);

-- Q104 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'Inversion in conditional type 3: "______ we received earlier notification, we would have averted the incident."', N'Đảo ngữ điều kiện loại 3 bỏ ''If'', đưa ''Had'' lên đầu: Had we received...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Had', 1),
(@q, 'B', N'Were', 0),
(@q, 'C', N'Should', 0),
(@q, 'D', N'If had', 0);

-- Q105 (Lesson 20 - NguPhap)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (12, 5, 20, N'TracNghiem', N'NguPhap', N'Inversion in conditional type 1: "______ any discrepancy arise, please alert the compliance officer immediately."', N'Đảo ngữ điều kiện loại 1 bỏ ''If'', đưa ''Should'' lên đầu: Should any discrepancy arise...', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Were', 0),
(@q, 'B', N'Had', 0),
(@q, 'C', N'Should', 1),
(@q, 'D', N'Could', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 21
-- -------------------------------------------------------------------
-- Q106 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'Choose the most professional opening sentence for a business inquiry email:', N'''I am writing with regard to your company''s logistics software solutions.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey, tell me about your software.', 0),
(@q, 'B', N'I am writing with regard to your company''s logistics software solutions.', 1),
(@q, 'C', N'I wanna know what you do.', 0),
(@q, 'D', N'Listen, can you explain your app?', 0);

-- Q107 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'What is the standard phrase to refer to an attached invoice in a commercial email?', N'''Please find attached the finalized invoice for services rendered.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Look at the invoice down here.', 0),
(@q, 'B', N'Please find attached the finalized invoice for services rendered.', 1),
(@q, 'C', N'I dropped the invoice paper.', 0),
(@q, 'D', N'See the attachment now.', 0);

-- Q108 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'Which phrase expresses anticipated future communication courteously?', N'''I look forward to hearing from you at your earliest convenience.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Reply fast or else.', 0),
(@q, 'B', N'I look forward to hearing from you at your earliest convenience.', 1),
(@q, 'C', N'You must answer quickly.', 0),
(@q, 'D', N'I wait for your typing.', 0);

-- Q109 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'When addressing an email to ''Dear Sir/Madam'' (recipient unknown by name), what is the traditional British sign-off?', N'Quy tắc British English: Khi không biết tên người nhận (''Dear Sir/Madam''), lời chào kết là ''Yours faithfully''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Yours sincerely,', 0),
(@q, 'B', N'Yours faithfully,', 1),
(@q, 'C', N'Best buddy,', 0),
(@q, 'D', N'Warm hugs,', 0);

-- Q110 (Lesson 21 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 3, 21, N'TracNghiem', N'Viet', N'Select the appropriate subject line for a job interview confirmation:', N'''Interview Confirmation: Senior Business Analyst - [Candidate Name]''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Hey about the interview', 0),
(@q, 'B', N'Interview Confirmation: Senior Business Analyst - [Candidate Name]', 1),
(@q, 'C', N'Can we talk tomorrow?', 0),
(@q, 'D', N'Job thing discussion', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 22
-- -------------------------------------------------------------------
-- Q111 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'How do you professionally request to reschedule an upcoming consultation meeting?', N'''Due to an unexpected scheduling conflict, would it be possible to reschedule our meeting to Thursday?''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I can''t come, pick another day.', 0),
(@q, 'B', N'Due to an unexpected scheduling conflict, would it be possible to reschedule our meeting to Thursday?', 1),
(@q, 'C', N'Cancel tomorrow, see you later.', 0),
(@q, 'D', N'Not coming today.', 0);

-- Q112 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'In customer complaint handling, which sentence expresses sincere accountability?', N'''We sincerely apologize for the inconvenience caused and have initiated an immediate investigation.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It wasn''t our fault, blame the courier.', 0),
(@q, 'B', N'We sincerely apologize for the inconvenience caused and have initiated an immediate investigation.', 1),
(@q, 'C', N'You should have checked earlier.', 0),
(@q, 'D', N'Everyone makes mistakes, calm down.', 0);

-- Q113 (Lesson 22 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'TuVung', N'What does ''expedite the replacement shipment'' mean?', N'''Expedite'' nghĩa là đẩy nhanh tiến độ xử lý/giao hàng.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Cancel the delivery entirely', 0),
(@q, 'B', N'Accelerate the shipping process of the replacement goods', 1),
(@q, 'C', N'Charge double for postage', 0),
(@q, 'D', N'Return the package to the manufacturer', 0);

-- Q114 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'Choose the best sentence to confirm an appointment time and location:', N'''This is to confirm our meeting on October 28th at 10:00 AM in Conference Room B.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Yes we meet tomorrow morning.', 0),
(@q, 'B', N'This is to confirm our meeting on October 28th at 10:00 AM in Conference Room B.', 1),
(@q, 'C', N'I will be there whenever.', 0),
(@q, 'D', N'Okay see you.', 0);

-- Q115 (Lesson 22 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (14, 4, 22, N'TracNghiem', N'Viet', N'Which closing statement maintains goodwill after resolving a dispute?', N'''We greatly value your partnership and remain committed to ensuring your satisfaction.''', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Don''t complain again next time.', 0),
(@q, 'B', N'We greatly value your partnership and remain committed to ensuring your satisfaction.', 1),
(@q, 'C', N'Case closed forever.', 0),
(@q, 'D', N'Be glad we gave you a refund.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 23
-- -------------------------------------------------------------------
-- Q116 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'In procurement negotiations, what does ''mutual benefit'' mean?', N'''Mutual benefit'' (đôi bên cùng có lợi) tương đương với giải pháp ''win-win''.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'An outcome where only one party profits.', 0),
(@q, 'B', N'An arrangement that advantages both contractual parties equitably.', 1),
(@q, 'C', N'A cancellation of business ties.', 0),
(@q, 'D', N'A legal penalty for delayed deliveries.', 0);

-- Q117 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'What legal term refers to a failure to fulfill agreed terms in a commercial contract?', N'''Breach of contract'' nghĩa là hành vi vi phạm hợp đồng.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Breach of contract', 1),
(@q, 'B', N'Amendment of agreement', 0),
(@q, 'C', N'Discharge of duty', 0),
(@q, 'D', N'Arbitration award', 0);

-- Q118 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'If a clause is described as ''non-negotiable'', it means:', N'Điều khoản ''non-negotiable'' là điều khoản cố định, bắt buộc, không thể thương lượng thêm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Subject to compromise', 0),
(@q, 'B', N'Fixed and mandatory without room for modification', 1),
(@q, 'C', N'Completely optional', 0),
(@q, 'D', N'Informal recommendation', 0);

-- Q119 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'What does ''FOB'' (Free On Board) signify in international freight terms?', N'FOB: người bán chịu trách nhiệm và chi phí cho đến khi hàng hóa được xếp lên tàu tại cảng bốc.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'The seller delivers when goods pass the ship''s rail at the named port of shipment.', 1),
(@q, 'B', N'The buyer pays nothing under any circumstance.', 0),
(@q, 'C', N'Freight is handled solely by postal courier.', 0),
(@q, 'D', N'Cargo must travel only by land.', 0);

-- Q120 (Lesson 23 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 4, 23, N'TracNghiem', N'TuVung', N'When parties ''reach a consensus'', they:', N'Hai bên đạt được sự đồng thuận và thống nhất chung.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Terminate negotiations angrily', 0),
(@q, 'B', N'Reach a collective agreement acceptable to all participants', 1),
(@q, 'C', N'File a lawsuit in court', 0),
(@q, 'D', N'Refuse to sign documents', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 24
-- -------------------------------------------------------------------
-- Q121 (Lesson 24 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'Doc', N'In a venture capital pitch, what is the ''value proposition''?', N'''Value proposition'' là tuyên bố giá trị cốt lõi giải thích vì sao sản phẩm vượt trội và đáng để đầu tư.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A clear statement explaining the unique benefit a product provides to solve customer problems.', 1),
(@q, 'B', N'The total electrical power needed to run office computers.', 0),
(@q, 'C', N'A list of employee social media accounts.', 0),
(@q, 'D', N'The personal history of the accountant.', 0);

-- Q122 (Lesson 24 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'TuVung', N'What does ''ROI'' represent in financial presentations?', N'ROI = Return on Investment (Tỷ suất hoàn vốn đầu tư).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Rate of Interest', 0),
(@q, 'B', N'Return on Investment', 1),
(@q, 'C', N'Revenue of Industry', 0),
(@q, 'D', N'Risk of Inflation', 0);

-- Q123 (Lesson 24 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'Doc', N'What does ''scaling the business'' mean to potential angel investors?', N'''Scaling the business'' là mở rộng quy mô kinh doanh nhanh chóng mà không làm tăng chi phí tỷ lệ thuận.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Reducing production output to cut costs.', 0),
(@q, 'B', N'Growing revenue exponentially while maintaining cost efficiency.', 1),
(@q, 'C', N'Closing international branch offices.', 0),
(@q, 'D', N'Filing for corporate bankruptcy.', 0);

-- Q124 (Lesson 24 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'TuVung', N'Which phrase refers to the percentage of total sales within a market earned by a company?', N'''Market share'' là thị phần của doanh nghiệp.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Market share', 1),
(@q, 'B', N'Stock volatility', 0),
(@q, 'C', N'Gross margin', 0),
(@q, 'D', N'Liquidity pool', 0);

-- Q125 (Lesson 24 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (15, 5, 24, N'TracNghiem', N'Doc', N'Why is demonstrating a ''defensible competitive moat'' vital during pitch sessions?', N'Vì nó chứng minh sản phẩm có rào cản cạnh tranh vững chắc khiến đối thủ không thể dễ dàng sao chép.', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It shows that competitors cannot easily duplicate the company''s advantage.', 1),
(@q, 'B', N'It proves the company has dug a water canal around its headquarters.', 0),
(@q, 'C', N'It guarantees zero taxation forever.', 0),
(@q, 'D', N'It eliminates the need for software engineering.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 25
-- -------------------------------------------------------------------
-- Q126 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'What is an ''API endpoint'' in modern software architecture?', N'API endpoint là một URL cụ thể nơi ứng dụng gửi yêu cầu để truy xuất hoặc sửa đổi tài nguyên.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'A physical cable connecting servers', 0),
(@q, 'B', N'A designated URL where client applications access server resources', 1),
(@q, 'C', N'The final line of a computer monitor', 0),
(@q, 'D', N'A password used for database encryption', 0);

-- Q127 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'Which process involves identifying, analyzing, and removing defects or bugs from computer code?', N'''Debugging'' là quá trình dò tìm và gỡ lỗi phần mềm.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Compiling', 0),
(@q, 'B', N'Debugging', 1),
(@q, 'C', N'Refactoring', 0),
(@q, 'D', N'Deployment', 0);

-- Q128 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'What does ''CRUD'' stand for in database management applications?', N'CRUD = Create, Read, Update, Delete (4 thao tác cơ bản với dữ liệu).', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Compile, Run, Undo, Deploy', 0),
(@q, 'B', N'Create, Read, Update, Delete', 1),
(@q, 'C', N'Copy, Rename, Upload, Download', 0),
(@q, 'D', N'Connect, Route, Unify, Disconnect', 0);

-- Q129 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'In Git version control, submitting code changes for team review is called creating a ______.', N'''Pull request'' (PR) là yêu cầu hợp nhất và rà soát mã nguồn.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'pull request', 1),
(@q, 'B', N'hard reboot', 0),
(@q, 'C', N'database rollback', 0),
(@q, 'D', N'local commit only', 0);

-- Q130 (Lesson 25 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (17, 3, 25, N'TracNghiem', N'TuVung', N'What is the primary characteristic of a ''relational database'' (RDBMS)?', N'Cơ sở dữ liệu quan hệ tổ chức dữ liệu thành các bảng có hàng và cột, liên kết với nhau bằng khóa (keys).', N'Kho', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'It stores data in tables linked by predefined relationships and foreign keys.', 1),
(@q, 'B', N'It cannot execute SQL queries.', 0),
(@q, 'C', N'It stores everything inside unstructured text files without columns.', 0),
(@q, 'D', N'It requires no primary key.', 0);

-- -------------------------------------------------------------------
-- BÀI HỌC ID 26
-- -------------------------------------------------------------------
-- Q131 (Lesson 26 - Doc)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'Doc', N'What are the three core questions answered in an Agile Daily Standup meeting?', N'3 câu hỏi cốt lõi: Hôm qua làm gì? Hôm nay sẽ làm gì? Có gặp trở ngại/khó khăn gì không?', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'What did you do yesterday? What will you do today? Are there any blockers?', 1),
(@q, 'B', N'How much is your salary? When is the party? Why are you late?', 0),
(@q, 'C', N'What is the company profit? Who is the president? What time is lunch?', 0),
(@q, 'D', N'Can you write all code today? Did you finish the entire project?', 0);

-- Q132 (Lesson 26 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'TuVung', N'In Scrum terminology, what is a ''blocker'' (or impediment)?', N'''Blocker'' là trở ngại, khó khăn kỹ thuật hoặc quy trình ngăn cản tiến độ công việc.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'An issue or dependency that prevents a team member from making progress.', 1),
(@q, 'B', N'A developer who refuses to attend meetings.', 0),
(@q, 'C', N'A firewall software blocking external attacks.', 0),
(@q, 'D', N'A completed task ready for production.', 0);

-- Q133 (Lesson 26 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'TuVung', N'What does ''wrap up the sprint'' mean in Agile project cycles?', N'''Wrap up the sprint'' nghĩa là hoàn tất và đóng kỳ Sprint.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'Finalize remaining sprint tasks, conduct review and prepare for retrospective.', 1),
(@q, 'B', N'Cancel the project immediately.', 0),
(@q, 'C', N'Delete all user stories from the backlog.', 0),
(@q, 'D', N'Skip quality assurance testing.', 0);

-- Q134 (Lesson 26 - TuVung)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'TuVung', N'How do Scrum teams estimate task effort during sprint planning?', N'Các nhóm Scrum thường dùng ''story points'' để ước lượng độ phức tạp và công sức của task.', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'By counting words in the user story', 0),
(@q, 'B', N'Using story points based on relative complexity and effort', 1),
(@q, 'C', N'By flipping a coin', 0),
(@q, 'D', N'By measuring keyboard typing speed', 0);

-- Q135 (Lesson 26 - Viet)
INSERT INTO CauHoi (MaChuDe, MaTrinhDo, MaBaiHoc, LoaiCauHoi, KyNang, NoiDungCauHoi, GiaiThichDapAn, DoKho, NguoiTao, NguonTao, DaKiemDuyet, NgayTao)
VALUES (18, 3, 26, N'TracNghiem', N'Viet', N'Choose the best concise update for a daily standup meeting:', N'''Yesterday I finished API integration for payments; today I will run unit tests; no blockers currently.''', N'TrungBinh', @gvId, N'GiaoVien', 1, SYSDATETIME());
SET @q = SCOPE_IDENTITY();
INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES
(@q, 'A', N'I worked on some stuff, today I do more stuff, see ya.', 0),
(@q, 'B', N'Yesterday I finished API integration for payments; today I will run unit tests; no blockers currently.', 1),
(@q, 'C', N'I didn''t do anything because the internet was slow.', 0),
(@q, 'D', N'Ask my manager what I am doing.', 0);

PRINT N'===================================================================';
PRINT N'ĐÃ THÊM THÀNH CÔNG 135 CÂU HỎI TRẮC NGHIỆM CHO 27 BÀI HỌC VÀO DATABASE!';
PRINT N'===================================================================';
GO
