-- =========================================================================================
-- FILE: 06_capnhat_baihoc_trichxuat.sql
-- MỤC ĐÍCH: Bổ sung và đồng bộ đầy đủ nội dung các bài học trích xuất học liệu vào bảng BaiHoc:
--   1. Bài 33: Môi trường, Biến đổi khí hậu & Năng lượng tái tạo (BaiHoc_33_Moi_truong_va_Nang_luong.md)
--   2. Bài 34: Âm nhạc & Nghệ thuật: Cảm xúc, Nhạc cụ & Thuật ngữ âm nhạc (BaiHoc_34_Am_nhac_va_Nghe_thuat.md)
--   3. Bài 35: Thành ngữ tiếng Anh trong Nghệ thuật, Sáng tạo & Quản trị (BaiHoc_35_Thanh_ngu_Nghe_thuat.md)
--   4. Bài 38 (hoặc mới): Đọc hiểu Nâng cao C1: Đột phá Trí tuệ Nhân tạo & Tác động Xã hội (BaiHoc_37_Doc_hieu_Nang_cao_AI.md)
-- =========================================================================================

USE TiengAnhAI;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;

BEGIN TRANSACTION;

BEGIN TRY
    DECLARE @gvDefaultId INT = 1;

    -- =====================================================================================
    -- 1. BÀI HỌC 33: Môi trường, Biến đổi khí hậu & Năng lượng tái tạo
    -- =====================================================================================
    IF EXISTS (SELECT 1 FROM BaiHoc WHERE MaBaiHoc = 33)
    BEGIN
        UPDATE BaiHoc
        SET MaChuDe = 22, -- Thời tiết & Môi trường sống
            MaTrinhDo = 3, -- B2 (Upper-Intermediate)
            NguoiTao = @gvDefaultId,
            TieuDe = N'Từ vựng chuyên sâu: Môi trường, Biến đổi khí hậu & Năng lượng tái tạo (Environmental Science & Renewable Energy)',
            LoaiBaiHoc = N'Doc',
            TomTat = N'Làm chủ vốn từ vựng học thuật về khoa học môi trường, năng lượng tái tạo (địa nhiệt, điện mặt trời, sinh khối), dấu chân carbon, thảm họa xói mòn do phá rừng và công nghệ lưu trữ carbon (CCS).',
            NoiDung = N'<div class=''lesson-content''><h2>Khoa Học Môi Trường & Năng Lượng Tái Tạo (Environmental Science & Renewable Energy Sources)</h2><p class=''lead''>Nắm bắt hệ thống từ vựng học thuật chuyên sâu và các cấu trúc ngữ pháp trọng điểm liên quan đến biến đổi khí hậu toàn cầu, các giải pháp năng lượng sạch và chiến lược phát thải ròng bằng không (Net-Zero).</p><h3>1. Các nguồn năng lượng tái tạo cốt lõi (Core Renewable Energy Sources)</h3><ul><li><strong>Geothermal energy (Năng lượng địa nhiệt):</strong> Khai thác nguồn nhiệt năng tích tụ sâu trong lòng vỏ trái đất (harnesses thermal heat stored deep beneath the Earth''s crust) để vận hành tuabin phát điện không phát thải carbon.</li><li><strong>Solar photovoltaic power (Điện mặt trời quang điện):</strong> Chuyển đổi trực tiếp quang năng thành điện năng thông qua các tấm pin bán dẫn quang điện.</li><li><strong>Hydroelectric energy (Thủy điện):</strong> Khai thác thế năng của dòng nước chảy qua các đập thủy điện để sản xuất điện sạch quy mô lớn.</li><li><strong>Biomass combustion (Năng lượng sinh khối):</strong> Tái tạo năng lượng từ các phế phẩm hữu cơ nông - lâm nghiệp theo chu trình tuần hoàn.</li></ul><h3>2. Thuật ngữ biến đổi khí hậu & chính sách sinh thái (Climate Change & Eco-Policies)</h3><ul><li><strong>Carbon footprint (Dấu chân carbon):</strong> Tổng khối lượng khí nhà kính (chủ yếu là CO2 và Metan) phát sinh trực tiếp và gián tiếp từ hoạt động sản xuất, sinh hoạt của con người.</li><li><strong>Net-zero carbon emissions (Phát thải ròng bằng không):</strong> Mục tiêu tối thượng của các công ước khí hậu quốc tế (như Paris Agreement) nhằm cân bằng tuyệt đối giữa lượng khí thải thải ra và lượng khí được hấp thụ trước năm 2050.</li><li><strong>Carbon Capture and Storage - CCS (Thu hồi và chôn lấp carbon):</strong> Công nghệ tiên tiến thu giữ khí CO2 ngay tại ống khói công nghiệp và bơm nén chôn lấp vĩnh viễn sâu dưới các tầng địa chất ngầm.</li><li><strong>Deforestation & Soil Erosion:</strong> Nạn phá rừng ồ ạt ở các sườn đồi núi làm suy giảm trầm trọng độ kết dính của tầng đất mặt, trực tiếp dẫn đến xói mòn đất và các trận lũ quét, sạt lở thảm khốc (severe soil erosion and catastrophic landslides).</li><li><strong>Biodegradable materials:</strong> Vật liệu phân hủy sinh học có khả năng tự phân rã tự nhiên nhờ vi sinh vật thành chất mùn hữu cơ vô hại trong thời gian tương đối ngắn, trái ngược với rác thải nhựa tồn tại hàng thiên niên kỷ.</li></ul><h3>3. Cấu trúc ngữ pháp trọng điểm trong văn bản học thuật môi trường (Grammar Focus)</h3><ul><li><strong>Câu điều kiện loại 2 giả định chính sách:</strong><br/><em>"If all industrialized nations were to transition to solar energy, greenhouse gas emissions would decrease significantly within the upcoming decade."</em></li><li><strong>Đảo ngữ câu điều kiện hỗn hợp loại 3 & 2 (Inversion in Mixed Conditionals):</strong><br/><em>"Had governments invested heavily in clean wind power ten years ago, air quality would be vastly superior today."</em> (Đảo ngữ đảo trợ động từ ''Had'' lên đầu câu thay thế liên từ ''If'').</li></ul><h3>4. Đoạn văn đọc hiểu ứng dụng thực tế (Academic Reading Passage)</h3><div class=''reading-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6;''>Humanity stands at a decisive juncture where transitioning away from petrochemical reliance is no longer an idealistic aspiration but an existential imperative. Among emerging non-intermittent clean energy alternatives, geothermal energy stands out due to its relentless capacity to supply continuous baseload electricity independent of prevailing weather vagaries.<br/><br/>Simultaneously, international climate summits emphasize that individual behavioral adjustments—such as curtailing personal carbon footprints—must coalesce with aggressive industrial decarbonization. Through the strategic implementation of Carbon Capture and Storage (CCS) infrastructure and large-scale afforestation initiatives, forward-thinking nations strive to hit the landmark net-zero threshold by mid-century. Safeguarding mountainous watersheds from rampant deforestation remains equally paramount to curb catastrophic landslides and secure ecological equilibrium for future generations.</div></div>',
            ThoiGianUocTinhPhut = 30,
            ThoiLuongUocTinh = 30,
            DaXuatBan = 1,
            NgayCapNhat = SYSUTCDATETIME()
        WHERE MaBaiHoc = 33;
    END

    -- =====================================================================================
    -- 2. BÀI HỌC 34: Âm nhạc & Nghệ thuật: Cảm xúc, Nhạc cụ & Thuật ngữ âm nhạc
    -- =====================================================================================
    IF EXISTS (SELECT 1 FROM BaiHoc WHERE MaBaiHoc = 34)
    BEGIN
        UPDATE BaiHoc
        SET MaChuDe = 21, -- Thể thao & Giải trí
            MaTrinhDo = 2, -- B1 (Intermediate)
            NguoiTao = @gvDefaultId,
            TieuDe = N'Âm nhạc & Nghệ thuật: Diễn đạt cảm xúc, Thể loại và Nhạc cụ (Music, Arts & Instruments)',
            LoaiBaiHoc = N'TuVung',
            TomTat = N'Nắm bắt thuật ngữ âm nhạc chuyên môn (Tempo, Pitch, Harmony...), 4 bộ nhạc cụ chính trong dàn nhạc giao hưởng (String, Woodwind, Brass, Percussion) và cách diễn đạt cảm xúc khi thưởng thức nghệ thuật.',
            NoiDung = N'<div class=''lesson-content''><h2>Thế Giới Âm Nhạc & Nghệ Thuật: Thuật Ngữ, Nhạc Cụ & Cảm Xúc (Music & Acoustic Dimensions)</h2><p class=''lead''>Trang bị trọn bộ vốn từ vựng miêu tả các phong cách âm nhạc, các thành phần cấu tạo nên tác phẩm nghệ thuật và phân loại khoa học các họ nhạc cụ trong dàn nhạc giao hưởng.</p><h3>1. Thuật ngữ âm nhạc cơ bản (Essential Musical Terminology)</h3><ul><li><strong>Tempo (Nhịp độ / Tốc độ):</strong> Tốc độ diễn tấu của một tác phẩm âm nhạc (ví dụ: Allegro - nhanh, sôi nổi; Adagio - chậm rãi, thanh thoát; Andante - thong thả).</li><li><strong>Pitch (Cao độ):</strong> Độ cao hay trầm của âm thanh được xác định bởi tần số dao động sóng âm.</li><li><strong>Harmony (Hòa âm):</strong> Sự kết hợp đồng thời của các nốt nhạc khác nhau tạo nên sự hòa quyện êm ái, đầy đặn cho giai điệu chính.</li><li><strong>Melody (Giai điệu) & Rhythm (Tiết tấu):</strong> Chuỗi các nốt nhạc nối tiếp nhau và mô hình nhịp phách cấu thành nên linh hồn bản nhạc.</li><li><strong>Chorus (Điệp khúc) & Lyrics (Lời bài hát):</strong> Đoạn nhạc trọng tâm được lặp lại nhiều lần mang thông điệp chính và câu chữ ca từ.</li></ul><h3>2. Bốn họ nhạc cụ chính trong dàn nhạc giao hưởng (Orchestra Instrument Families)</h3><table style=''width:100%; border-collapse:collapse; margin-top:10px; margin-bottom:15px; background:white; font-size:13px;''><thead><tr style=''background:#f1f5f9; border-bottom:2px solid #cbd5e1; text-align:left;''><th style=''padding:8px;''>Họ nhạc cụ (Family)</th><th style=''padding:8px;''>Đặc điểm cấu tạo & Phát âm</th><th style=''padding:8px;''>Các nhạc cụ tiêu biểu</th></tr></thead><tbody><tr style=''border-bottom:1px solid #e2e8f0;''><td style=''padding:8px; font-weight:bold; color:#0369a1;''>String (Bộ dây)</td><td style=''padding:8px;''>Phát ra âm thanh bằng cách gảy, kéo cung vĩ hoặc gõ vào dây đàn căng sẵn.</td><td style=''padding:8px;''>Violin, Viola, <strong>Cello</strong>, Double Bass, Harp.</td></tr><tr style=''border-bottom:1px solid #e2e8f0;''><td style=''padding:8px; font-weight:bold; color:#047857;''>Woodwind (Bộ gỗ)</td><td style=''padding:8px;''>Âm thanh tạo bởi luồng hơi thổi qua lỗ thổi hoặc qua lưỡi gà dăm kèn.</td><td style=''padding:8px;''>Flute, Oboe, <strong>Clarinet</strong>, Bassoon, Piccolo.</td></tr><tr style=''border-bottom:1px solid #e2e8f0;''><td style=''padding:8px; font-weight:bold; color:#b45309;''>Brass (Bộ đồng)</td><td style=''padding:8px;''>Nhạc cụ bằng đồng tạo âm bằng sự rung của môi người biểu diễn vào ống ngậm.</td><td style=''padding:8px;''>Trumpet, French Horn, <strong>Trombone</strong>, Tuba.</td></tr><tr style=''border-bottom:1px solid #e2e8f0;''><td style=''padding:8px; font-weight:bold; color:#7c3aed;''>Percussion (Bộ gõ)</td><td style=''padding:8px;''>Tạo âm thanh bằng cách dùng dùi gõ, đánh, lắc hoặc cọ xát trực tiếp.</td><td style=''padding:8px;''><strong>Timpani</strong> (trống định âm), Snare Drum, Cymbals, Xylophone.</td></tr></tbody></table><h3>3. Mẫu câu miêu tả cảm thụ âm nhạc (Expressions for Musical Appreciation)</h3><ul><li><em>"The symphonic piece began with an exhilarating tempo that captivated the entire auditorium."</em> (Bản giao hưởng mở đầu với nhịp độ dồn dập đầy hứng khởi khiến toàn thể khán phòng bị cuốn hút).</li><li><em>"The harmonious blend of the cello section and the warm brass tones evoked profound nostalgia."</em> (Sự hòa quyện êm ái giữa bè cello và âm sắc ấm áp của dàn kèn đồng gợi lên nỗi nhớ da diết).</li></ul></div>',
            ThoiGianUocTinhPhut = 20,
            ThoiLuongUocTinh = 20,
            DaXuatBan = 1,
            NgayCapNhat = SYSUTCDATETIME()
        WHERE MaBaiHoc = 34;
    END

    -- =====================================================================================
    -- 3. BÀI HỌC 35: Thành ngữ tiếng Anh trong Nghệ thuật, Sáng tạo & Quản trị
    -- =====================================================================================
    IF EXISTS (SELECT 1 FROM BaiHoc WHERE MaBaiHoc = 35)
    BEGIN
        UPDATE BaiHoc
        SET MaChuDe = 21, -- Thể thao & Giải trí (hoặc Nghệ thuật)
            MaTrinhDo = 3, -- B2 (Upper-Intermediate)
            NguoiTao = @gvDefaultId,
            TieuDe = N'Thành ngữ tiếng Anh trong Nghệ thuật, Sáng tạo & Quản trị (Idioms in Arts & Leadership)',
            LoaiBaiHoc = N'TuVung',
            TomTat = N'Làm chủ kho thành ngữ xuất phát từ âm nhạc và nghệ thuật biểu diễn, cách ứng dụng tự nhiên vào giao tiếp công sở và đàm phán thương mại quốc tế.',
            NoiDung = N'<div class=''lesson-content''><h2>Thành Ngữ Tiếng Anh Chủ Đề Nghệ Thuật & Ứng Dụng Đời Sống (Idiomatic Artistry & Professional Fluency)</h2><p class=''lead''>Khám phá những thành ngữ xuất xứ từ sân khấu, phòng hòa nhạc và hội họa giúp câu chữ của bạn trở nên sống động, tinh tế và đậm phong thái bản ngữ trong cả giao tiếp hàng ngày lẫn công việc.</p><h3>1. Tuyển tập thành ngữ nghệ thuật & ý nghĩa chuyên sâu (Art & Performance Idioms)</h3><ul><li><strong>Music to someone''s ears:</strong> Tin tức hay thông tin vô cùng dễ chịu, đáng mừng và đúng như kỳ vọng của người nghe.<br/><em>Ví dụ: "When the project director announced an early release date, it was music to our ears."</em></li><li><strong>Play it by ear:</strong> Tùy cơ ứng biến, linh hoạt hành động dựa trên tình hình thực tế diễn ra mà không lập kế hoạch quá cứng nhắc.<br/><em>Ví dụ: "We don''t have a rigid schedule for the weekend project, so let''s just play it by ear."</em></li><li><strong>Face the music:</strong> Dũng cảm đối mặt và gánh chịu những hậu quả, trách nhiệm hoặc lời khiển trách từ những hành động sai lầm của mình.<br/><em>Ví dụ: "After the security audit identified vulnerabilities, the management team had to face the music."</em></li><li><strong>Strike a chord:</strong> Chạm đúng cảm xúc, gợi lên sự đồng cảm sâu sắc hoặc làm ai đó nhớ lại trải nghiệm cá nhân thân thuộc.<br/><em>Ví dụ: "His presentation on user privacy struck a chord with the tech community."</em></li><li><strong>Blow one''s own trumpet (Mỹ: Blow one''s own horn):</strong> Tự khoe khoang, tự tâng bốc tài năng hoặc chiến tích của chính mình.<br/><em>Ví dụ: "She is remarkably humble and rarely blows her own trumpet despite her academic accolades."</em></li><li><strong>State of the art:</strong> Đạt đỉnh cao công nghệ, hiện đại nhất, tối tân nhất tại thời điểm hiện tại.<br/><em>Ví dụ: "Our engineering department has implemented a state-of-the-art neural translation system."</em></li></ul><h3>2. Thành ngữ tư duy bản lĩnh trong công việc (Workplace Resilience Idioms)</h3><ul><li><strong>Bite the bullet:</strong> Nghiến răng chấp nhận đối mặt với một tình huống đau đớn, khó khăn sau khi đã chần chừ, né tránh.<br/><em>Ví dụ: "The company finally decided to bite the bullet and replace the legacy mainframe infrastructure."</em></li><li><strong>Burn the midnight oil:</strong> Thức khuya miệt mài học tập hoặc làm việc chăm chỉ thâu đêm suốt sáng để kịp tiến độ.<br/><em>Ví dụ: "The engineering squad burned the midnight oil throughout the weekend to resolve the server outage."</em></li></ul></div>',
            ThoiGianUocTinhPhut = 25,
            ThoiLuongUocTinh = 25,
            DaXuatBan = 1,
            NgayCapNhat = SYSUTCDATETIME()
        WHERE MaBaiHoc = 35;
    END

    -- =====================================================================================
    -- 4. BÀI HỌC 37 (AI): Đọc hiểu Nâng cao C1: Đột phá Trí tuệ Nhân tạo & Tác động Xã hội
    -- =====================================================================================
    DECLARE @maBaiHocAI INT;

    -- Kiểm tra xem bài học AI đã được tạo trước đó chưa
    SELECT TOP 1 @maBaiHocAI = MaBaiHoc 
    FROM BaiHoc 
    WHERE TieuDe LIKE N'%Đột phá Trí tuệ Nhân tạo%' OR TieuDe LIKE N'%AI Breakthroughs%';

    IF @maBaiHocAI IS NULL
    BEGIN
        INSERT INTO BaiHoc (
            MaChuDe, MaTrinhDo, NguoiTao, TieuDe, LoaiBaiHoc, 
            TomTat, NoiDung, ThoiGianUocTinhPhut, ThoiLuongUocTinh, DaXuatBan, NgayTao, NgayCapNhat
        )
        VALUES (
            16, -- Tiếng Anh Công Nghệ Thông Tin (IT English)
            4,  -- C1 (Advanced)
            @gvDefaultId,
            N'Đọc hiểu Nâng cao C1: Đột phá Trí tuệ Nhân tạo & Tác động Xã hội (AI Paradigm Shifts & Societal Impact)',
            N'Doc',
            N'Bài đọc học thuật C1 chuẩn khảo thí VSTEP / IELTS về trí tuệ nhân tạo toàn năng (AGI), tác động nhận thức người học (cognitive atrophy), thiên kiến thuật toán (algorithmic bias) và thức giả định (subjunctive mood).',
            N'<div class=''lesson-content''><h2>Đột Phá Trí Tuệ Nhân Tạo & Tác Động Xã Hội Sâu Rộng (AI Paradigm Shifts & Societal Ramifications)</h2><p class=''lead''>Phân tích bài đọc học thuật nâng cao cấp độ C1, giải mã tư duy biện luận đa chiều về làn sóng Trí tuệ Nhân tạo tổng quát (AGI), các thách thức đạo đức, suy giảm nhận thức độc lập và cấu trúc thức giả định pháp quy.</p><h3>1. Bài đọc học thuật C1 chuẩn khảo thí (Academic Reading Passage)</h3><div class=''reading-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6; font-size:13.5px;''>The ascendancy of artificial general intelligence (AGI) represents a profound paradigm shift in human history. While proponents accentuate unprecedented productivity leaps in scientific discovery and biomedical automation, skeptics articulate legitimate apprehensions regarding cognitive atrophy and algorithmic bias.<br/><br/>Empirical studies indicate that over-reliance on generative models diminishes critical inquiry and independent problem-solving among university cohorts. Furthermore, socio-economic stratification risks exacerbation as capital accrues disproportionately to technology conglomerates orchestrating proprietary foundational models.<br/><br/>Consequently, international regulatory bodies advocate for stringent algorithmic transparency and human-in-the-loop oversight before automated decision systems govern criminal justice, healthcare triage, or financial credit scoring. Ultimately, AI must be viewed as a double-edged sword: it brings extraordinary economic benefits while simultaneously posing profound ethical and existential hazards if deployed without rigorous human governance.</div><h3>2. Bản đồ từ vựng học thuật C1 trọng điểm (Lexical Architecture)</h3><ul><li><strong>Ascendancy (Danh từ - C1):</strong> Sự thống trị, vị thế vươn lên dẫn đầu hoặc quyền uy ngày càng lớn (tương đương với <em>dominance, supremacy</em>).</li><li><strong>Double-edged sword (Thành ngữ học thuật):</strong> Con dao hai lưỡi - mang lại những lợi ích kinh tế phi thường nhưng đồng thời tiềm ẩn rủi ro đạo đức và nguy cơ sống còn sâu sắc.</li><li><strong>Cognitive atrophy (Thuật ngữ tâm lý nhận thức):</strong> Sự thoái hóa, suy mòn dần dần năng lực trí tuệ và kỹ năng tự giải quyết vấn đề độc lập do sự thiếu rèn luyện tư duy thường xuyên.</li><li><strong>Algorithmic bias (Thiên kiến thuật toán):</strong> Xu hướng sai lệch mang tính hệ thống được tích hợp trong các mô hình máy học do dữ liệu huấn luyện mang tính thiên vị.</li><li><strong>Human-in-the-loop (Nguyên tắc thiết kế hệ thống):</strong> Cơ chế bắt buộc luôn phải có sự can thiệp, giám sát và thẩm định phê duyệt của con người trước khi quyết định tự động có hiệu lực.</li><li><strong>Algorithmic transparency (Tính minh bạch thuật toán):</strong> Yêu cầu các mô hình hộp đen phải có khả năng giải trình cơ chế ra quyết định rõ ràng.</li><li><strong>Socio-economic stratification:</strong> Sự phân tầng kinh tế - xã hội gia tăng khi tư bản và giá trị thặng dư tập trung cục bộ vào các tập đoàn công nghệ lớn.</li></ul><h3>3. Ngữ pháp C1: Thức giả định (Subjunctive Mood) trong văn bản quy chuẩn</h3><ul><li><strong>Công thức:</strong> <em>It is essential / imperative / crucial that + S + (should) V-bare...</em><br/>Ví dụ chuẩn: <em>"It is essential that governments <strong>regulate</strong> artificial intelligence deployment."</em> (Động từ regulate ở dạng nguyên thể, không chia số ít).</li><li><strong>Sau các động từ yêu cầu/đề xuất:</strong> <em>insist, demand, recommend, mandate that + S + V-bare...</em><br/>Ví dụ chuẩn: <em>"The ethics committee insisted that every neural network model <strong>adhere</strong> to transparent verification guidelines prior to deployment."</em></li></ul></div>',
            40,
            40,
            1,
            SYSUTCDATETIME(),
            SYSUTCDATETIME()
        );

        SET @maBaiHocAI = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        UPDATE BaiHoc
        SET MaChuDe = 16,
            MaTrinhDo = 4,
            TieuDe = N'Đọc hiểu Nâng cao C1: Đột phá Trí tuệ Nhân tạo & Tác động Xã hội (AI Paradigm Shifts & Societal Impact)',
            LoaiBaiHoc = N'Doc',
            TomTat = N'Bài đọc học thuật C1 chuẩn khảo thí VSTEP / IELTS về trí tuệ nhân tạo toàn năng (AGI), tác động nhận thức người học (cognitive atrophy), thiên kiến thuật toán (algorithmic bias) và thức giả định (subjunctive mood).',
            NoiDung = N'<div class=''lesson-content''><h2>Đột Phá Trí Tuệ Nhân Tạo & Tác Động Xã Hội Sâu Rộng (AI Paradigm Shifts & Societal Ramifications)</h2><p class=''lead''>Phân tích bài đọc học thuật nâng cao cấp độ C1, giải mã tư duy biện luận đa chiều về làn sóng Trí tuệ Nhân tạo tổng quát (AGI), các thách thức đạo đức, suy giảm nhận thức độc lập và cấu trúc thức giả định pháp quy.</p><h3>1. Bài đọc học thuật C1 chuẩn khảo thí (Academic Reading Passage)</h3><div class=''reading-sample'' style=''background:#f8fafc; border:1px solid #e2e8f0; padding:18px; border-radius:6px; font-family:sans-serif; line-height:1.6; font-size:13.5px;''>The ascendancy of artificial general intelligence (AGI) represents a profound paradigm shift in human history. While proponents accentuate unprecedented productivity leaps in scientific discovery and biomedical automation, skeptics articulate legitimate apprehensions regarding cognitive atrophy and algorithmic bias.<br/><br/>Empirical studies indicate that over-reliance on generative models diminishes critical inquiry and independent problem-solving among university cohorts. Furthermore, socio-economic stratification risks exacerbation as capital accrues disproportionately to technology conglomerates orchestrating proprietary foundational models.<br/><br/>Consequently, international regulatory bodies advocate for stringent algorithmic transparency and human-in-the-loop oversight before automated decision systems govern criminal justice, healthcare triage, or financial credit scoring. Ultimately, AI must be viewed as a double-edged sword: it brings extraordinary economic benefits while simultaneously posing profound ethical and existential hazards if deployed without rigorous human governance.</div><h3>2. Bản đồ từ vựng học thuật C1 trọng điểm (Lexical Architecture)</h3><ul><li><strong>Ascendancy (Danh từ - C1):</strong> Sự thống trị, vị thế vươn lên dẫn đầu hoặc quyền uy ngày càng lớn (tương đương với <em>dominance, supremacy</em>).</li><li><strong>Double-edged sword (Thành ngữ học thuật):</strong> Con dao hai lưỡi - mang lại những lợi ích kinh tế phi thường nhưng đồng thời tiềm ẩn rủi ro đạo đức và nguy cơ sống còn sâu sắc.</li><li><strong>Cognitive atrophy (Thuật ngữ tâm lý nhận thức):</strong> Sự thoái hóa, suy mòn dần dần năng lực trí tuệ và kỹ năng tự giải quyết vấn đề độc lập do sự thiếu rèn luyện tư duy thường xuyên.</li><li><strong>Algorithmic bias (Thiên kiến thuật toán):</strong> Xu hướng sai lệch mang tính hệ thống được tích hợp trong các mô hình máy học do dữ liệu huấn luyện mang tính thiên vị.</li><li><strong>Human-in-the-loop (Nguyên tắc thiết kế hệ thống):</strong> Cơ chế bắt buộc luôn phải có sự can thiệp, giám sát và thẩm định phê duyệt của con người trước khi quyết định tự động có hiệu lực.</li><li><strong>Algorithmic transparency (Tính minh bạch thuật toán):</strong> Yêu cầu các mô hình hộp đen phải có khả năng giải trình cơ chế ra quyết định rõ ràng.</li><li><strong>Socio-economic stratification:</strong> Sự phân tầng kinh tế - xã hội gia tăng khi tư bản và giá trị thặng dư tập trung cục bộ vào các tập đoàn công nghệ lớn.</li></ul><h3>3. Ngữ pháp C1: Thức giả định (Subjunctive Mood) trong văn bản quy chuẩn</h3><ul><li><strong>Công thức:</strong> <em>It is essential / imperative / crucial that + S + (should) V-bare...</em><br/>Ví dụ chuẩn: <em>"It is essential that governments <strong>regulate</strong> artificial intelligence deployment."</em> (Động từ regulate ở dạng nguyên thể, không chia số ít).</li><li><strong>Sau các động từ yêu cầu/đề xuất:</strong> <em>insist, demand, recommend, mandate that + S + V-bare...</em><br/>Ví dụ chuẩn: <em>"The ethics committee insisted that every neural network model <strong>adhere</strong> to transparent verification guidelines prior to deployment."</em></li></ul></div>',
            DaXuatBan = 1,
            NgayCapNhat = SYSUTCDATETIME()
        WHERE MaBaiHoc = @maBaiHocAI;
    END

    -- Cập nhật 12 câu hỏi trích xuất từ BaiHoc_37_Doc_hieu_Nang_cao_AI.md liên kết về bài học AI này
    IF @maBaiHocAI IS NOT NULL AND @maBaiHocAI > 0
    BEGIN
        UPDATE CauHoi
        SET MaBaiHoc = @maBaiHocAI
        WHERE TepTinNguon = 'BaiHoc_37_Doc_hieu_Nang_cao_AI.md'
          AND (MaBaiHoc IS NULL OR MaBaiHoc <> @maBaiHocAI);
    END

    COMMIT TRANSACTION;
    PRINT N'SUCCESS: Đã cập nhật và đồng bộ toàn bộ bài học trích xuất vào CSDL thành công!';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    DECLARE @errMsg NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@errMsg, 16, 1);
END CATCH;
GO
