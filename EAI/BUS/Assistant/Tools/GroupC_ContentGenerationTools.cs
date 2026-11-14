using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS;
using BUS.AI;
using DAL;
using DTO;
using DTO.Assistant;

namespace BUS.Assistant.Tools
{
    public class QuestionPreviewCardPayloadDTO
    {
        public string CardType { get; set; } = "question_preview_card";
        public string ToolName { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public string Level { get; set; } = "B1";
        public string Skill { get; set; } = "NguPhap";
        public string? Passage { get; set; }
        public bool RequiresConfirmation { get; set; }
        public bool IsWrite { get; set; }
        public string? ConfirmationToken { get; set; }
        public List<QuestionPreviewItemDTO> Questions { get; set; } = new();
        public string StagedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public class QuestionPreviewItemDTO
    {
        public int Index { get; set; }
        public string Content { get; set; } = string.Empty;
        public string QuestionType { get; set; } = "TracNghiem";
        public string QuestionTypeDisplay { get; set; } = "Trắc nghiệm";
        public string Skill { get; set; } = "Doc";
        public string Level { get; set; } = "B1";
        public string Difficulty { get; set; } = "TrungBinh";
        public List<QuestionOptionPreviewDTO> Options { get; set; } = new();
        public int CorrectIndex { get; set; }
        public string? TextAnswer { get; set; }
        public string? Explanation { get; set; }
        public string? Passage { get; set; }
    }

    public class QuestionOptionPreviewDTO
    {
        public string Symbol { get; set; } = "A";
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }

    public static class QuestionPreviewCardHelper
    {
        public static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static QuestionPreviewCardPayloadDTO CreateMultipleChoicePreviewPayload(
            string toolName, string topic, string level, string skill, List<CauHoiDTO> questions)
        {
            var payload = new QuestionPreviewCardPayloadDTO
            {
                ToolName = toolName,
                Topic = topic,
                Level = level,
                Skill = skill,
                RequiresConfirmation = false,
                IsWrite = false
            };

            for (int i = 0; i < questions.Count; i++)
            {
                var q = questions[i];
                var item = new QuestionPreviewItemDTO
                {
                    Index = i + 1,
                    Content = q.NoiDungCauHoi,
                    QuestionType = "TracNghiem",
                    QuestionTypeDisplay = "Trắc nghiệm",
                    Skill = q.KyNang ?? skill,
                    Level = q.TrinhDo ?? level,
                    Difficulty = q.DoKho ?? "TrungBinh",
                    CorrectIndex = q.DapAnDungIndex ?? 0,
                    Explanation = q.GiaiThichChiTiet
                };

                if (q.DanhSachLuaChon != null)
                {
                    for (int j = 0; j < q.DanhSachLuaChon.Count; j++)
                    {
                        item.Options.Add(new QuestionOptionPreviewDTO
                        {
                            Symbol = ((char)('A' + j)).ToString(),
                            Text = CauHoiDTO.StripOptionPrefix(q.DanhSachLuaChon[j]),
                            IsCorrect = (j == (q.DapAnDungIndex ?? 0))
                        });
                    }
                }

                payload.Questions.Add(item);
            }

            return payload;
        }

        public static QuestionPreviewCardPayloadDTO CreateFillBlankPreviewPayload(
            string toolName, string topic, string level, string skill, List<CauHoiDTO> questions)
        {
            var payload = new QuestionPreviewCardPayloadDTO
            {
                ToolName = toolName,
                Topic = topic,
                Level = level,
                Skill = skill,
                RequiresConfirmation = false,
                IsWrite = false
            };

            for (int i = 0; i < questions.Count; i++)
            {
                var q = questions[i];
                payload.Questions.Add(new QuestionPreviewItemDTO
                {
                    Index = i + 1,
                    Content = q.NoiDungCauHoi,
                    QuestionType = "DienKhuyet",
                    QuestionTypeDisplay = "Điền khuyết",
                    Skill = q.KyNang ?? skill,
                    Level = q.TrinhDo ?? level,
                    Difficulty = q.DoKho ?? "TrungBinh",
                    TextAnswer = q.DapAnVanBan,
                    Explanation = q.GiaiThichChiTiet
                });
            }

            return payload;
        }

        public static QuestionPreviewCardPayloadDTO CreateReadingPassagePreviewPayload(
            string toolName, string topic, string level, string passage, List<CauHoiDTO> questions)
        {
            var payload = new QuestionPreviewCardPayloadDTO
            {
                ToolName = toolName,
                Topic = topic,
                Level = level,
                Skill = "Doc",
                Passage = passage,
                RequiresConfirmation = false,
                IsWrite = false
            };

            for (int i = 0; i < questions.Count; i++)
            {
                var q = questions[i];
                var item = new QuestionPreviewItemDTO
                {
                    Index = i + 1,
                    Content = q.NoiDungCauHoi,
                    QuestionType = q.LoaiCauHoi ?? "TracNghiem",
                    QuestionTypeDisplay = CauHoiKhaoSatDTO.LayTenLoaiCauHoi(q.LoaiCauHoi),
                    Skill = "Doc",
                    Level = level,
                    Difficulty = q.DoKho ?? "TrungBinh",
                    CorrectIndex = q.DapAnDungIndex ?? 0,
                    TextAnswer = q.DapAnVanBan,
                    Explanation = q.GiaiThichChiTiet,
                    Passage = passage
                };

                if (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 0)
                {
                    for (int j = 0; j < q.DanhSachLuaChon.Count; j++)
                    {
                        item.Options.Add(new QuestionOptionPreviewDTO
                        {
                            Symbol = ((char)('A' + j)).ToString(),
                            Text = CauHoiDTO.StripOptionPrefix(q.DanhSachLuaChon[j]),
                            IsCorrect = (j == (q.DapAnDungIndex ?? 0))
                        });
                    }
                }

                payload.Questions.Add(item);
            }

            return payload;
        }
    }

    /// <summary>
    /// Công cụ sinh câu hỏi trắc nghiệm tiếng Anh 4 lựa chọn (A, B, C, D) qua AI.
    /// </summary>
    public class GenerateMultipleChoiceQuestionTool : IAssistantTool
    {
        public string Name => "generate_multiple_choice_question";
        public string Description => "Tạo câu hỏi trắc nghiệm tiếng Anh (4 lựa chọn A, B, C, D) theo chủ đề, trình độ CEFR (A1-C2) và kỹ năng qua AI hoặc bộ sinh thông minh.";
        public string Category => "ContentGen";
        public string RequiredRole => AssistantPermissionService.RoleLecturer; // GiangVien & QuanTriVien
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("topic", "string", "Chủ đề hoặc điểm ngữ pháp/từ vựng (ví dụ: 'Present Perfect', 'Environment', 'Workplace idioms')", isRequired: true),
            new("level", "string", "Trình độ CEFR (A1, A2, B1, B2, C1, C2)", isRequired: false, allowedValues: new[] { "A1", "A2", "B1", "B2", "C1", "C2" }, defaultValue: "B1"),
            new("skill", "string", "Kỹ năng trọng tâm (NguPhap, TuVung, Doc, Nghe)", isRequired: false, allowedValues: new[] { "NguPhap", "TuVung", "Doc", "Nghe" }, defaultValue: "NguPhap"),
            new("count", "int", "Số lượng câu hỏi cần tạo (tối thiểu 1, tối đa 5)", isRequired: false, defaultValue: 1)
        };

        public async Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            string topic = ToolArgumentHelper.GetString(arguments, "topic")?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(topic))
            {
                return AssistantToolResult.Fail("Tham số 'topic' (chủ đề tạo câu hỏi) không được để trống.", "MISSING_ARGUMENT");
            }

            string level = ToolArgumentHelper.GetString(arguments, "level", "B1")!;
            string skill = ToolArgumentHelper.GetString(arguments, "skill", "NguPhap")!;
            int count = Math.Clamp(ToolArgumentHelper.GetInt(arguments, "count", 1), 1, 5);

            List<CauHoiDTO> questions;
            try
            {
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                linkedCts.CancelAfter(TimeSpan.FromSeconds(3));
                questions = await AIServiceManager.Instance.BocTachCauHoiAsync(
                    tieuDe: topic,
                    noiDung: topic,
                    trinhDo: level,
                    soLuong: count,
                    loaiBaiHoc: skill,
                    yeuCauLoaiCauHoi: "TracNghiem",
                    ct: linkedCts.Token);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GenerateMultipleChoiceQuestionTool] Fallback: {ex.Message}");
                questions = GeminiAIService.TaoCauHoiThongMinhTheoNoiDung(topic, topic, level, skill, count);
            }

            if (questions == null || questions.Count == 0)
            {
                questions = GeminiAIService.TaoCauHoiThongMinhTheoNoiDung(topic, topic, level, skill, count);
            }

            var previewPayload = QuestionPreviewCardHelper.CreateMultipleChoicePreviewPayload(
                toolName: Name,
                topic: topic,
                level: level,
                skill: skill,
                questions: questions);

            string previewJson = JsonSerializer.Serialize(previewPayload, QuestionPreviewCardHelper.JsonOptions);
            string message = $"Đã tạo thành công {questions.Count} câu hỏi trắc nghiệm tiếng Anh cho chủ đề '{topic}' (Trình độ {level}). Bạn có thể xem trước nội dung bên dưới.";

            return AssistantToolResult.Ok(message, data: questions, cardType: "question_preview_card", cardPayloadJson: previewJson);
        }
    }

    /// <summary>
    /// Công cụ tạo bài tập điền từ còn thiếu vào chỗ trống (Fill in the blank / Sentence Completion).
    /// </summary>
    public class GenerateFillBlankQuestionTool : IAssistantTool
    {
        public string Name => "generate_fill_blank_question";
        public string Description => "Tạo bài tập điền từ khuyết vào chỗ trống (Fill in the blank) theo chủ đề, trình độ CEFR và ngữ cảnh bài học.";
        public string Category => "ContentGen";
        public string RequiredRole => AssistantPermissionService.RoleLecturer; // GiangVien & QuanTriVien
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("topic", "string", "Chủ đề hoặc cấu trúc cần điền từ (ví dụ: 'Prepositions', 'Modal verbs', 'Business terms')", isRequired: true),
            new("level", "string", "Trình độ CEFR (A1, A2, B1, B2, C1, C2)", isRequired: false, allowedValues: new[] { "A1", "A2", "B1", "B2", "C1", "C2" }, defaultValue: "B1"),
            new("skill", "string", "Kỹ năng (NguPhap, TuVung, Doc, Viet)", isRequired: false, defaultValue: "NguPhap"),
            new("count", "int", "Số lượng câu hỏi điền từ (1-5)", isRequired: false, defaultValue: 1)
        };

        public async Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            string topic = ToolArgumentHelper.GetString(arguments, "topic")?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(topic))
            {
                return AssistantToolResult.Fail("Tham số 'topic' không được để trống.", "MISSING_ARGUMENT");
            }

            string level = ToolArgumentHelper.GetString(arguments, "level", "B1")!;
            string skill = ToolArgumentHelper.GetString(arguments, "skill", "NguPhap")!;
            int count = Math.Clamp(ToolArgumentHelper.GetInt(arguments, "count", 1), 1, 5);

            List<CauHoiDTO> questions;
            try
            {
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                linkedCts.CancelAfter(TimeSpan.FromSeconds(3));
                questions = await AIServiceManager.Instance.BocTachCauHoiAsync(
                    tieuDe: topic,
                    noiDung: topic,
                    trinhDo: level,
                    soLuong: count,
                    loaiBaiHoc: skill,
                    yeuCauLoaiCauHoi: "DienKhuyet",
                    ct: linkedCts.Token);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GenerateFillBlankQuestionTool] Fallback: {ex.Message}");
                questions = GenerateFallbackFillBlankQuestions(topic, level, skill, count);
            }

            if (questions == null || questions.Count == 0 || !questions.Any(q => q.LoaiCauHoi == "DienKhuyet"))
            {
                questions = GenerateFallbackFillBlankQuestions(topic, level, skill, count);
            }

            var previewPayload = QuestionPreviewCardHelper.CreateFillBlankPreviewPayload(
                toolName: Name,
                topic: topic,
                level: level,
                skill: skill,
                questions: questions);

            string previewJson = JsonSerializer.Serialize(previewPayload, QuestionPreviewCardHelper.JsonOptions);
            string message = $"Đã tạo thành công {questions.Count} bài tập điền từ khuyết cho chủ đề '{topic}' (Trình độ {level}). Bạn có thể xem trước và kiểm tra đáp án bên dưới.";

            return AssistantToolResult.Ok(message, data: questions, cardType: "question_preview_card", cardPayloadJson: previewJson);
        }

        private static List<CauHoiDTO> GenerateFallbackFillBlankQuestions(string topic, string level, string skill, int count)
        {
            var list = new List<CauHoiDTO>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = $"Fill in the missing word related to '{topic}':\n\"The engineering team must ______ all compliance requirements prior to deployment.\"",
                    LoaiCauHoi = "DienKhuyet",
                    DapAnVanBan = "fulfill|meet|satisfy",
                    KyNang = skill,
                    TrinhDo = level,
                    DoKho = "TrungBinh",
                    GiaiThichChiTiet = "Động từ 'fulfill/meet requirements' mang nghĩa hoàn thành hoặc đáp ứng các tiêu chuẩn quy định."
                });
            }
            return list;
        }
    }

    /// <summary>
    /// Công cụ tạo đoạn văn đọc hiểu học thuật kèm bộ câu hỏi đọc hiểu khai thác nội dung.
    /// </summary>
    public class GenerateReadingPassageTool : IAssistantTool
    {
        public string Name => "generate_reading_passage";
        public string Description => "Tạo đoạn văn đọc hiểu học thuật (Reading Passage) kèm bộ câu hỏi đọc hiểu (Main Idea, Factual Detail, True/False/Not Given).";
        public string Category => "ContentGen";
        public string RequiredRole => AssistantPermissionService.RoleLecturer; // GiangVien & QuanTriVien
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("topic", "string", "Chủ đề đoạn văn đọc hiểu (ví dụ: 'Artificial Intelligence in Healthcare', 'Renewable Energy Innovations')", isRequired: true),
            new("level", "string", "Trình độ CEFR (A2, B1, B2, C1, C2)", isRequired: false, allowedValues: new[] { "A2", "B1", "B2", "C1", "C2" }, defaultValue: "B2"),
            new("questionCount", "int", "Số lượng câu hỏi đọc hiểu đi kèm (1-5)", isRequired: false, defaultValue: 3)
        };

        public async Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            string topic = ToolArgumentHelper.GetString(arguments, "topic")?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(topic))
            {
                return AssistantToolResult.Fail("Tham số 'topic' không được để trống.", "MISSING_ARGUMENT");
            }

            string level = ToolArgumentHelper.GetString(arguments, "level", "B2")!;
            int qCount = Math.Clamp(ToolArgumentHelper.GetInt(arguments, "questionCount", 3), 1, 5);

            string passageText;
            List<CauHoiDTO> questions;

            try
            {
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                linkedCts.CancelAfter(TimeSpan.FromSeconds(3));
                passageText = await AIServiceManager.Instance.SinhNoiDungMauAsync(topic, level, "Doc", ct: linkedCts.Token);
                if (string.IsNullOrWhiteSpace(passageText))
                {
                    passageText = GeminiAIService.TaoNoiDungMauThongMinh(topic, level, "Doc");
                }

                questions = await AIServiceManager.Instance.BocTachCauHoiAsync(
                    tieuDe: topic,
                    noiDung: passageText,
                    trinhDo: level,
                    soLuong: qCount,
                    loaiBaiHoc: "Doc",
                    ct: linkedCts.Token);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GenerateReadingPassageTool] Fallback: {ex.Message}");
                passageText = GeminiAIService.TaoNoiDungMauThongMinh(topic, level, "Doc");
                questions = GeminiAIService.TaoCauHoiThongMinhTheoNoiDung(topic, passageText, level, "Doc", qCount);
            }

            if (questions == null || questions.Count == 0)
            {
                questions = GeminiAIService.TaoCauHoiThongMinhTheoNoiDung(topic, passageText, level, "Doc", qCount);
            }

            foreach (var q in questions)
            {
                if (string.IsNullOrWhiteSpace(q.DoanVanDoc))
                {
                    q.DoanVanDoc = passageText;
                }
            }

            var previewPayload = QuestionPreviewCardHelper.CreateReadingPassagePreviewPayload(
                toolName: Name,
                topic: topic,
                level: level,
                passage: passageText,
                questions: questions);

            string previewJson = JsonSerializer.Serialize(previewPayload, QuestionPreviewCardHelper.JsonOptions);
            string message = $"Đã tạo thành công bài đọc hiểu cho chủ đề '{topic}' (Trình độ {level}) kèm {questions.Count} câu hỏi khai thác nội dung. Bạn có thể xem trước bên dưới.";

            return AssistantToolResult.Ok(message, data: new { Passage = passageText, Questions = questions }, cardType: "question_preview_card", cardPayloadJson: previewJson);
        }
    }

    /// <summary>
    /// Công cụ lưu câu hỏi đã sinh vào Ngân hàng câu hỏi trong CSDL SQL Server TiengAnhAI.
    /// Yêu cầu xác nhận hai bước (Staged Two-Step Confirmation).
    /// </summary>
    public class SaveGeneratedQuestionTool : IAssistantTool
    {
        public string Name => "save_generated_question";
        public string Description => "Lưu câu hỏi do AI tạo (trắc nghiệm, điền từ, đọc hiểu) vào Ngân hàng câu hỏi trong CSDL SQL Server TiengAnhAI. Thao tác ghi yêu cầu xem trước và người dùng xác nhận.";
        public string Category => "ContentGen";
        public string RequiredRole => AssistantPermissionService.RoleLecturer; // GiangVien & QuanTriVien
        public bool IsWriteOperation => true;
        public bool RequiresConfirmation => true;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("content", "string", "Nội dung câu hỏi (NoiDungCauHoi)", isRequired: true),
            new("questionType", "string", "Loại câu hỏi (TracNghiem, DienKhuyet, TrueFalseNotGiven, DungSai, TraLoiNgan, VietLaiCau, TuLuan)", isRequired: false, defaultValue: "TracNghiem"),
            new("skill", "string", "Kỹ năng (Doc, Nghe, NguPhap, TuVung, Viet, Noi)", isRequired: false, defaultValue: "Doc"),
            new("level", "string", "Trình độ CEFR (A1, A2, B1, B2, C1, C2)", isRequired: false, defaultValue: "B1"),
            new("difficulty", "string", "Độ khó (De, TrungBinh, Kho)", isRequired: false, defaultValue: "TrungBinh"),
            new("options", "array", "Danh sách các phương án lựa chọn A, B, C, D (dành cho Trắc nghiệm/TrueFalse)", isRequired: false),
            new("correctIndex", "int", "Chỉ số đáp án đúng (0 = A, 1 = B, 2 = C, 3 = D)", isRequired: false, defaultValue: 0),
            new("textAnswer", "string", "Đáp án dạng chữ chính xác (dành cho câu hỏi điền từ / trả lời ngắn)", isRequired: false),
            new("explanation", "string", "Giải thích chi tiết lý do đáp án đúng", isRequired: false),
            new("passage", "string", "Đoạn văn đọc hiểu (DoanVanDoc) nếu là câu hỏi đọc hiểu", isRequired: false),
            new("lessonId", "int", "Mã bài học (MaBaiHoc) liên kết nếu có", isRequired: false),
            new("topicId", "int", "Mã chủ đề (MaChuDe) liên kết nếu có", isRequired: false)
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            string content = ToolArgumentHelper.GetString(arguments, "content")?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(content))
            {
                return Task.FromResult(AssistantToolResult.Fail("Nội dung câu hỏi (content) không được để trống.", "MISSING_CONTENT"));
            }

            string rawQt = ToolArgumentHelper.GetString(arguments, "questionType", "TracNghiem")!;
            string questionType = CauHoiKhaoSatDTO.ChuanHoaLoaiCauHoi(rawQt);

            string rawSkill = ToolArgumentHelper.GetString(arguments, "skill", "Doc")!;
            string skill = CauHoiKhaoSatDTO.ChuanHoaKyNang(rawSkill);

            string level = ToolArgumentHelper.GetString(arguments, "level", "B1")!.Trim().ToUpperInvariant();
            string rawDiff = ToolArgumentHelper.GetString(arguments, "difficulty", "TrungBinh")!;
            string difficulty = CauHoiKhaoSatDTO.ChuanHoaDoKho(rawDiff);

            string? explanation = ToolArgumentHelper.GetString(arguments, "explanation");
            string? passage = ToolArgumentHelper.GetString(arguments, "passage");
            string? textAnswer = ToolArgumentHelper.GetString(arguments, "textAnswer")?.Trim();

            int? lessonId = ToolArgumentHelper.GetNullableInt(arguments, "lessonId");
            int? topicId = ToolArgumentHelper.GetNullableInt(arguments, "topicId");
            int correctIndex = ToolArgumentHelper.GetInt(arguments, "correctIndex", 0);

            var rawOptions = ExtractOptionsList(arguments);

            var cauHoiDto = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 0,
                NoiDungCauHoi = content,
                LoaiCauHoi = questionType,
                KyNang = skill,
                DoKho = difficulty,
                MaTrinhDo = level,
                GiaiThichDapAn = explanation,
                DoanVanDoc = passage,
                MaBaiHoc = lessonId,
                MaChuDe = topicId,
                DaKiemDuyet = true
            };

            var dsDapAn = new List<LuaChonDapAnDTO>();

            if (questionType == "DienKhuyet" || questionType == "TraLoiNgan" || questionType == "VietLaiCau" || questionType == "SuaLoiSai")
            {
                if (string.IsNullOrWhiteSpace(textAnswer) && rawOptions.Count > 0)
                {
                    textAnswer = rawOptions[0];
                }

                if (string.IsNullOrWhiteSpace(textAnswer))
                {
                    return Task.FromResult(AssistantToolResult.Fail("Câu hỏi điền từ / trả lời ngắn yêu cầu phải có đáp án chữ (textAnswer).", "MISSING_ANSWER"));
                }

                dsDapAn.Add(new LuaChonDapAnDTO
                {
                    KyHieuLuaChon = "ANS",
                    NoiDungLuaChon = textAnswer,
                    LaDapAnDung = true
                });
            }
            else
            {
                if (rawOptions.Count < 2)
                {
                    return Task.FromResult(AssistantToolResult.Fail("Câu hỏi trắc nghiệm cần tối thiểu 2 phương án lựa chọn.", "INSUFFICIENT_OPTIONS"));
                }

                if (correctIndex < 0 || correctIndex >= rawOptions.Count)
                {
                    correctIndex = 0;
                }

                for (int i = 0; i < rawOptions.Count; i++)
                {
                    string kyHieu = ((char)('A' + i)).ToString();
                    string noiDung = CauHoiDTO.StripOptionPrefix(rawOptions[i]);
                    bool laDung = (i == correctIndex);

                    dsDapAn.Add(new LuaChonDapAnDTO
                    {
                        KyHieuLuaChon = kyHieu,
                        NoiDungLuaChon = noiDung,
                        LaDapAnDung = laDung
                    });
                }

                if (!dsDapAn.Any(x => x.LaDapAnDung))
                {
                    dsDapAn[0].LaDapAnDung = true;
                }
            }

            int userId = context.UserId > 0 ? context.UserId : 1;
            bool success = CauHoiDAL.Instance.LuuCauHoiVaDapAn(cauHoiDto, dsDapAn, userId, out string errorMsg, nguonTao: "AI");

            if (!success)
            {
                return Task.FromResult(AssistantToolResult.Fail($"Lỗi khi ghi dữ liệu câu hỏi vào SQL Server: {errorMsg}", "DB_WRITE_ERROR"));
            }

            var savedPayload = new
            {
                ma_cau_hoi = cauHoiDto.MaCauHoi,
                noi_dung = cauHoiDto.NoiDungCauHoi,
                loai_cau_hoi = cauHoiDto.LoaiCauHoi,
                ky_nang = cauHoiDto.KyNang,
                trinh_do = cauHoiDto.MaTrinhDo,
                so_luong_dap_an = dsDapAn.Count,
                nguon_tao = "AI",
                ngay_luu = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            string successMsg = $"Đã lưu thành công câu hỏi vào Ngân hàng câu hỏi của hệ thống (Mã câu hỏi: #{cauHoiDto.MaCauHoi}).";
            return Task.FromResult(AssistantToolResult.Ok(
                successMsg, 
                data: savedPayload, 
                cardType: "save_question_success_card", 
                cardPayloadJson: JsonSerializer.Serialize(savedPayload)));
        }

        private static List<string> ExtractOptionsList(Dictionary<string, object?> arguments)
        {
            var result = new List<string>();
            if (!arguments.TryGetValue("options", out var val) || val == null)
            {
                return result;
            }

            if (val is IEnumerable<string> strEnum)
            {
                result.AddRange(strEnum);
            }
            else if (val is IEnumerable<object> objEnum)
            {
                result.AddRange(objEnum.Select(o => o?.ToString() ?? string.Empty).Where(s => !string.IsNullOrWhiteSpace(s)));
            }
            else if (val is JsonElement jsonElem)
            {
                if (jsonElem.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in jsonElem.EnumerateArray())
                    {
                        string s = item.GetString() ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(s)) result.Add(s);
                    }
                }
                else if (jsonElem.ValueKind == JsonValueKind.String)
                {
                    string raw = jsonElem.GetString() ?? string.Empty;
                    try
                    {
                        var parsed = JsonSerializer.Deserialize<List<string>>(raw);
                        if (parsed != null) result.AddRange(parsed);
                    }
                    catch { result.Add(raw); }
                }
            }
            else if (val is string strVal)
            {
                if (strVal.StartsWith("[") && strVal.EndsWith("]"))
                {
                    try
                    {
                        var parsed = JsonSerializer.Deserialize<List<string>>(strVal);
                        if (parsed != null) result.AddRange(parsed);
                    }
                    catch { }
                }
                if (result.Count == 0)
                {
                    var lines = strVal.Split(new[] { '\n', '\r', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    result.AddRange(lines.Select(l => l.Trim()));
                }
            }

            return result;
        }
    }
}
