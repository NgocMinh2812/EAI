using System;
using System.Data;
using System.Threading.Tasks;
using BUS;
using BUS.Assistant;
using DAL;
using DAL.Assistant;
using DTO;
using DTO.Assistant;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace EAI.Tests
{
    public class AIAssistantPersistenceTests
    {
        [Fact]
        public void SessionHelper_Accessors_ValidateCorrectly()
        {
            var user = new NguoiDungDTO
            {
                MaNguoiDung = 1234,
                TenDangNhap = "worker_m1",
                TenVaiTro = "QuanTriVien"
            };

            SessionHelper.CurrentUser = user;
            Assert.Equal(1234, SessionHelper.CurrentMaNguoiDung);
            Assert.Equal("QuanTriVien", SessionHelper.CurrentRole);

            SessionHelper.CurrentUser = null;
            Assert.Equal(-1, SessionHelper.CurrentMaNguoiDung);
            Assert.Equal("Khach", SessionHelper.CurrentRole);
        }

        [Fact]
        public void DTO_ConversationAndMessage_PropertiesAndHelpers()
        {
            var conv = new AIAssistantConversationDTO
            {
                MaHoiThoai = 10,
                MaNguoiDung = 5,
                TieuDe = "Hội thoại thử nghiệm",
                TrangThai = "Active",
                TongSoTinNhan = 3,
                NgayTao = DateTime.Now,
                NgayCapNhat = DateTime.Now
            };

            Assert.Equal(10, conv.ConversationId);
            Assert.Equal(5, conv.UserId);
            Assert.Equal("Hội thoại thử nghiệm", conv.Title);
            Assert.True(conv.IsActive);
            Assert.False(conv.IsArchived);
            Assert.False(conv.IsDeleted);
            Assert.False(string.IsNullOrWhiteSpace(conv.FormattedTime));

            var msg = new AIAssistantMessageDTO
            {
                MaTinNhan = 1001,
                MaHoiThoai = 10,
                VaiTro = "assistant",
                NoiDung = "Đây là câu trả lời",
                ToolCallsJson = "{\"tool\": \"search\"}",
                ToolResultJson = "{\"status\": \"ok\"}",
                CardPayloadJson = "{\"card\": \"preview\"}",
                ThoiGianGui = DateTime.Now
            };

            Assert.Equal(1001, msg.MessageId);
            Assert.Equal(10, msg.ConversationId);
            Assert.Equal("assistant", msg.Role);
            Assert.Equal("Đây là câu trả lời", msg.Content);
            Assert.True(msg.IsAssistant);
            Assert.False(msg.IsUser);
            Assert.True(msg.HasToolCalls);
            Assert.True(msg.HasToolResult);
            Assert.True(msg.HasCard);
            Assert.False(string.IsNullOrWhiteSpace(msg.FormattedTime));
        }

        [Fact]
        public void Service_GenerateTitleFromPrompt_ValidatesCorrectly()
        {
            // Empty / null
            Assert.Equal("Cuộc trò chuyện mới", AssistantConversationService.GenerateTitleFromPrompt(null!));
            Assert.Equal("Cuộc trò chuyện mới", AssistantConversationService.GenerateTitleFromPrompt("   "));

            // Short prompt
            string shortPrompt = "Hỏi về thì quá khứ đơn";
            Assert.Equal(shortPrompt, AssistantConversationService.GenerateTitleFromPrompt(shortPrompt));

            // Prompt with multiple whitespace and newlines
            string messyPrompt = "  Hướng dẫn   học \r\n từ vựng \t chủ đề môi trường   ";
            Assert.Equal("Hướng dẫn học từ vựng chủ đề môi trường", AssistantConversationService.GenerateTitleFromPrompt(messyPrompt));

            // Long prompt (>45 chars)
            string longPrompt = "Hãy giải thích chi tiết cấu trúc câu bị động trong tiếng Anh kèm theo 10 ví dụ thực tế";
            string generated = AssistantConversationService.GenerateTitleFromPrompt(longPrompt);
            Assert.True(generated.Length <= 45);
            Assert.EndsWith("...", generated);
        }

        [Fact]
        public async Task Service_CRUD_And_DAL_DatabaseRoundtrip()
        {
            // 1. Kiểm tra kết nối CSDL và chạy tương thích lược đồ
            bool canConnect = SqlDataProvider.Instance.TestConnection();
            if (!canConnect)
            {
                // Bỏ qua nếu môi trường test không có SQL Server cục bộ
                return;
            }

            // Lấy một người dùng hợp lệ từ bảng NguoiDung để thỏa mãn FK ON DELETE CASCADE
            string userQuery = "SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaNguoiDung > 0 ORDER BY MaNguoiDung ASC";
            DataTable dtUser = SqlDataProvider.Instance.ExecuteQuery(userQuery);
            if (dtUser.Rows.Count == 0)
            {
                return;
            }
            int testUserId = Convert.ToInt32(dtUser.Rows[0]["MaNguoiDung"]);

            var prevUser = SessionHelper.CurrentUser;
            SessionHelper.CurrentUser = new NguoiDungDTO { MaNguoiDung = testUserId, TenDangNhap = "test_user", TenVaiTro = "SinhVien" };

            var service = AssistantConversationService.Instance;

            try
            {
                // 2. Tạo cuộc hội thoại mới
                string initialTitle = $"Test Hội Thoại {Guid.NewGuid().ToString().Substring(0, 8)}";
                var conv = await service.CreateConversationAsync(testUserId, initialTitle);
                Assert.NotNull(conv);
                Assert.True(conv.MaHoiThoai > 0, "Mã hội thoại mới phải lớn hơn 0");
                Assert.Equal(initialTitle, conv.TieuDe);
                Assert.Equal(testUserId, conv.MaNguoiDung);

                try
                {
                    // 3. Thêm tin nhắn người dùng
                    var userMsg = await service.AddMessageAsync(conv.MaHoiThoai, "user", "Xin chào Trợ lý EAI!");
                    Assert.NotNull(userMsg);
                    Assert.True(userMsg.MaTinNhan > 0);
                    Assert.Equal("user", userMsg.VaiTro);

                    // 4. Thêm tin nhắn phản hồi trợ lý kèm Tool Calls và Card Payload
                    string toolJson = "{\"name\":\"TimKiemCauHoi\",\"query\":\"Present Perfect\"}";
                    string resultJson = "{\"count\":5}";
                    string cardJson = "{\"type\":\"StatsCard\",\"title\":\"Thống kê câu hỏi\"}";
                    var assistantMsg = await service.AddMessageAsync(
                        conv.MaHoiThoai, 
                        "assistant", 
                        "Dưới đây là 5 câu hỏi thì hiện tại hoàn thành:",
                        toolCallsJson: toolJson,
                        toolResultJson: resultJson,
                        cardPayloadJson: cardJson
                    );
                    Assert.NotNull(assistantMsg);
                    Assert.True(assistantMsg.MaTinNhan > userMsg.MaTinNhan);
                    Assert.True(assistantMsg.HasToolCalls);
                    Assert.True(assistantMsg.HasCard);

                    // 5. Lấy danh sách tin nhắn và kiểm tra thứ tự
                    var messages = await service.GetMessagesAsync(conv.MaHoiThoai, testUserId);
                    Assert.Equal(2, messages.Count);
                    Assert.Equal("user", messages[0].VaiTro);
                    Assert.Equal("assistant", messages[1].VaiTro);
                    Assert.Equal(toolJson, messages[1].ToolCallsJson);
                    Assert.Equal(cardJson, messages[1].CardPayloadJson);

                    // 6. Đổi tên cuộc hội thoại
                    string updatedTitle = $"Đã đổi tên {Guid.NewGuid().ToString().Substring(0, 8)}";
                    bool renamed = await service.RenameConversationAsync(conv.MaHoiThoai, testUserId, updatedTitle);
                    Assert.True(renamed);

                    var fetchedConv = await service.GetConversationByIdAsync(conv.MaHoiThoai, testUserId);
                    Assert.NotNull(fetchedConv);
                    Assert.Equal(updatedTitle, fetchedConv.TieuDe);

                    // 7. Tìm kiếm cuộc hội thoại theo từ khóa
                    var searchList = await service.GetConversationsAsync(testUserId, updatedTitle);
                    Assert.NotEmpty(searchList);
                    Assert.Contains(searchList, c => c.MaHoiThoai == conv.MaHoiThoai);

                    // 8. Xóa tất cả tin nhắn trong hội thoại
                    bool cleared = await service.ClearMessagesAsync(conv.MaHoiThoai, testUserId);
                    Assert.True(cleared);

                    var msgsAfterClear = await service.GetMessagesAsync(conv.MaHoiThoai, testUserId);
                    Assert.Empty(msgsAfterClear);

                    // 9. Xóa mềm cuộc hội thoại
                    bool deleted = await service.DeleteConversationAsync(conv.MaHoiThoai, testUserId);
                    Assert.True(deleted);

                    var activeList = await service.GetConversationsAsync(testUserId);
                    Assert.DoesNotContain(activeList, c => c.MaHoiThoai == conv.MaHoiThoai);
                }
                finally
                {
                    // Dọn dẹp dữ liệu test (Hard delete)
                    AIAssistantDAL.Instance.XoaHoiThoai(conv.MaHoiThoai, testUserId, hardDelete: true);
                }
            }
            finally
            {
                SessionHelper.CurrentUser = prevUser;
            }
        }

        [Fact]
        public async Task Service_FirstUserMessage_AutoUpdatesDefaultTitle()
        {
            bool canConnect = SqlDataProvider.Instance.TestConnection();
            if (!canConnect) return;

            string userQuery = "SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaNguoiDung > 0 ORDER BY MaNguoiDung ASC";
            DataTable dtUser = SqlDataProvider.Instance.ExecuteQuery(userQuery);
            if (dtUser.Rows.Count == 0) return;
            int testUserId = Convert.ToInt32(dtUser.Rows[0]["MaNguoiDung"]);

            var prevUser = SessionHelper.CurrentUser;
            SessionHelper.CurrentUser = new NguoiDungDTO { MaNguoiDung = testUserId, TenDangNhap = "test_user", TenVaiTro = "SinhVien" };

            var service = AssistantConversationService.Instance;

            try
            {
                // Tạo hội thoại không truyền title -> sinh tiêu đề mặc định "Cuộc trò chuyện mới"
                var conv = await service.CreateConversationAsync(testUserId, null);
                Assert.NotNull(conv);
                Assert.StartsWith("Cuộc trò chuyện", conv.TieuDe);

                try
                {
                    // Gửi tin nhắn đầu tiên của user
                    string userPrompt = "Hướng dẫn phân biệt Thì Hiện Tại Hoàn Thành và Quá Khứ Đơn";
                    await service.AddMessageAsync(conv.MaHoiThoai, "user", userPrompt);

                    // Kiểm tra tiêu đề đã được tự động cập nhật
                    var updatedConv = await service.GetConversationByIdAsync(conv.MaHoiThoai, testUserId);
                    Assert.NotNull(updatedConv);
                    Assert.Equal(AssistantConversationService.GenerateTitleFromPrompt(userPrompt), updatedConv.TieuDe);

                    // Gửi tin nhắn thứ hai của user -> tiêu đề KHÔNG bị ghi đè
                    await service.AddMessageAsync(conv.MaHoiThoai, "user", "Tin nhắn thứ hai không làm đổi tiêu đề");
                    var convAfterSecondMsg = await service.GetConversationByIdAsync(conv.MaHoiThoai, testUserId);
                    Assert.NotNull(convAfterSecondMsg);
                    Assert.Equal(AssistantConversationService.GenerateTitleFromPrompt(userPrompt), convAfterSecondMsg.TieuDe);
                }
                finally
                {
                    AIAssistantDAL.Instance.XoaHoiThoai(conv.MaHoiThoai, testUserId, hardDelete: true);
                }
            }
            finally
            {
                SessionHelper.CurrentUser = prevUser;
            }
        }

        [Fact]
        public async Task Security_UserIdZeroOrNegative_DeniesAccess_NoIDORBypass()
        {
            bool canConnect = SqlDataProvider.Instance.TestConnection();
            if (!canConnect) return;

            string userQuery = "SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaNguoiDung > 0 ORDER BY MaNguoiDung ASC";
            DataTable dtUser = SqlDataProvider.Instance.ExecuteQuery(userQuery);
            if (dtUser.Rows.Count == 0) return;
            int validUserId = Convert.ToInt32(dtUser.Rows[0]["MaNguoiDung"]);

            var prevUser = SessionHelper.CurrentUser;
            SessionHelper.CurrentUser = null; // Giả lập người dùng vãng lai / chưa đăng nhập

            var service = AssistantConversationService.Instance;
            var dal = AIAssistantDAL.Instance;

            // Tạo hội thoại hợp lệ thuộc về validUserId
            var conv = await service.CreateConversationAsync(validUserId, "Cuộc hội thoại bảo mật của người dùng");
            Assert.NotNull(conv);
            Assert.True(conv.MaHoiThoai > 0);

            try
            {
                await service.AddMessageAsync(conv.MaHoiThoai, "user", "Dữ liệu nhạy cảm cần bảo vệ");

                // --- Kiểm tra DAL trực tiếp với userId = 0 và userId = -1 (Phòng ngừa IDOR) ---
                // 1. Không được lấy danh sách hội thoại của người khác
                var dalList0 = dal.LayDanhSachHoiThoai(0);
                Assert.Empty(dalList0);
                var dalListNeg = dal.LayDanhSachHoiThoai(-1);
                Assert.Empty(dalListNeg);

                // 2. Không được đọc chi tiết hội thoại
                var dalConv0 = dal.LayHoiThoaiTheoId(conv.MaHoiThoai, 0);
                Assert.Null(dalConv0);
                var dalConvNeg = dal.LayHoiThoaiTheoId(conv.MaHoiThoai, -1);
                Assert.Null(dalConvNeg);

                // 3. Không được đổi tên hội thoại
                bool dalRename0 = dal.CapNhatTieuDeHoiThoai(conv.MaHoiThoai, 0, "Bị đổi tên trái phép");
                Assert.False(dalRename0);

                // 4. Không được đọc tin nhắn
                var dalMsgs0 = dal.LayDanhSachTinNhan(conv.MaHoiThoai, 0);
                Assert.Empty(dalMsgs0);

                // 5. Không được xóa tin nhắn
                bool dalClear0 = dal.XoaTatCaTinNhan(conv.MaHoiThoai, 0);
                Assert.False(dalClear0);

                // 6. Không được xóa hội thoại
                bool dalDelete0 = dal.XoaHoiThoai(conv.MaHoiThoai, 0);
                Assert.False(dalDelete0);

                // --- Kiểm tra tầng Service với userId = 0 ---
                // 1. GetConversationsAsync(0) phải trả về rỗng
                var srvList0 = await service.GetConversationsAsync(0);
                Assert.Empty(srvList0);

                // 2. GetConversationByIdAsync(id, 0) phải trả về null
                var srvConv0 = await service.GetConversationByIdAsync(conv.MaHoiThoai, 0);
                Assert.Null(srvConv0);

                // 3. RenameConversationAsync(id, 0, ...) phải trả về false
                bool srvRename0 = await service.RenameConversationAsync(conv.MaHoiThoai, 0, "Hack");
                Assert.False(srvRename0);

                // 4. DeleteConversationAsync(id, 0) phải trả về false
                bool srvDelete0 = await service.DeleteConversationAsync(conv.MaHoiThoai, 0);
                Assert.False(srvDelete0);

                // 5. ClearMessagesAsync(id, 0) phải trả về false
                bool srvClear0 = await service.ClearMessagesAsync(conv.MaHoiThoai, 0);
                Assert.False(srvClear0);

                // 6. GetMessagesAsync(id, 0) phải trả về rỗng
                var srvMsgs0 = await service.GetMessagesAsync(conv.MaHoiThoai, 0);
                Assert.Empty(srvMsgs0);

                // 7. CreateConversationAsync với userId = 0 phải ném ngoại lệ
                await Assert.ThrowsAsync<ArgumentException>(() => service.CreateConversationAsync(0, "Hack"));

                // Xác nhận dữ liệu hội thoại của victim hoàn toàn nguyên vẹn
                var victimConv = await service.GetConversationByIdAsync(conv.MaHoiThoai, validUserId);
                Assert.NotNull(victimConv);
                Assert.Equal("Cuộc hội thoại bảo mật của người dùng", victimConv.TieuDe);
                Assert.Equal(1, victimConv.TongSoTinNhan);
            }
            finally
            {
                dal.XoaHoiThoai(conv.MaHoiThoai, validUserId, hardDelete: true);
                SessionHelper.CurrentUser = prevUser;
            }
        }

        [Fact]
        public async Task Concurrency_HighLoadParallelInserts_PassWithoutDeadlock1205()
        {
            bool canConnect = SqlDataProvider.Instance.TestConnection();
            if (!canConnect) return;

            string userQuery = "SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaNguoiDung > 0 ORDER BY MaNguoiDung ASC";
            DataTable dtUser = SqlDataProvider.Instance.ExecuteQuery(userQuery);
            if (dtUser.Rows.Count == 0) return;
            int testUserId = Convert.ToInt32(dtUser.Rows[0]["MaNguoiDung"]);

            var prevUser = SessionHelper.CurrentUser;
            SessionHelper.CurrentUser = new NguoiDungDTO { MaNguoiDung = testUserId, TenDangNhap = "test_user", TenVaiTro = "SinhVien" };

            var service = AssistantConversationService.Instance;
            var conv = await service.CreateConversationAsync(testUserId, "Stress Test Concurrency Deadlock");
            Assert.NotNull(conv);

            try
            {
                const int concurrentTasks = 20;

                // Chạy đồng thời 20 tác vụ chèn tin nhắn vào cùng 1 cuộc hội thoại
                var tasks = new Task<AIAssistantMessageDTO>[concurrentTasks];
                for (int i = 0; i < concurrentTasks; i++)
                {
                    int index = i;
                    tasks[i] = Task.Run(async () =>
                    {
                        return await service.AddMessageAsync(
                            conv.MaHoiThoai, 
                            "user", 
                            $"Concurrent message payload #{index} - {Guid.NewGuid()}"
                        );
                    });
                }

                AIAssistantMessageDTO[] results = await Task.WhenAll(tasks);

                // Xác nhận 100% tin nhắn chèn thành công không gặp SqlException 1205 Deadlock
                Assert.Equal(concurrentTasks, results.Length);
                foreach (var r in results)
                {
                    Assert.NotNull(r);
                    Assert.True(r.MaTinNhan > 0);
                }

                // Kiểm tra số lượng tin nhắn đã cập nhật nguyên tử trong CSDL
                var updatedConv = await service.GetConversationByIdAsync(conv.MaHoiThoai, testUserId);
                Assert.NotNull(updatedConv);
                Assert.Equal(concurrentTasks, updatedConv.TongSoTinNhan);

                var messages = await service.GetMessagesAsync(conv.MaHoiThoai, testUserId);
                Assert.Equal(concurrentTasks, messages.Count);
            }
            finally
            {
                AIAssistantDAL.Instance.XoaHoiThoai(conv.MaHoiThoai, testUserId, hardDelete: true);
                SessionHelper.CurrentUser = prevUser;
            }
        }

        [Fact]
        public async Task Robustness_LongKeywordAndLongTitle_NoSqlException()
        {
            bool canConnect = SqlDataProvider.Instance.TestConnection();
            if (!canConnect) return;

            string userQuery = "SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaNguoiDung > 0 ORDER BY MaNguoiDung ASC";
            DataTable dtUser = SqlDataProvider.Instance.ExecuteQuery(userQuery);
            if (dtUser.Rows.Count == 0) return;
            int testUserId = Convert.ToInt32(dtUser.Rows[0]["MaNguoiDung"]);

            var prevUser = SessionHelper.CurrentUser;
            SessionHelper.CurrentUser = new NguoiDungDTO { MaNguoiDung = testUserId, TenDangNhap = "test_user", TenVaiTro = "SinhVien" };

            var service = AssistantConversationService.Instance;
            var dal = AIAssistantDAL.Instance;

            // 1. Kiểm tra từ khóa tìm kiếm siêu dài (>4000 ký tự) không gây sập (SqlException 8152)
            string extremeKeyword = new string('A', 5000);
            var listFromService = await service.GetConversationsAsync(testUserId, extremeKeyword);
            Assert.NotNull(listFromService);

            var listFromDal = dal.LayDanhSachHoiThoai(testUserId, extremeKeyword);
            Assert.NotNull(listFromDal);

            // 2. Kiểm tra tạo hội thoại với tiêu đề siêu dài (>300 ký tự) không gây lỗi tràn cột (SqlException 2628)
            string veryLongTitle = new string('B', 400);
            var conv = await service.CreateConversationAsync(testUserId, veryLongTitle);
            Assert.NotNull(conv);
            Assert.True(conv.MaHoiThoai > 0);
            Assert.True(conv.TieuDe.Length <= 250);

            try
            {
                // 3. Đổi tên hội thoại với tiêu đề siêu dài (>300 ký tự)
                string renameLongTitle = new string('C', 600);
                bool renamedSrv = await service.RenameConversationAsync(conv.MaHoiThoai, testUserId, renameLongTitle);
                Assert.True(renamedSrv);

                var convAfterRename = await service.GetConversationByIdAsync(conv.MaHoiThoai, testUserId);
                Assert.NotNull(convAfterRename);
                Assert.True(convAfterRename.TieuDe.Length <= 250);

                // 4. Đổi tên trực tiếp qua DAL với tiêu đề dài
                string dalLongTitle = new string('D', 500);
                bool renamedDal = dal.CapNhatTieuDeHoiThoai(conv.MaHoiThoai, testUserId, dalLongTitle);
                Assert.True(renamedDal);

                var convAfterDalRename = dal.LayHoiThoaiTheoId(conv.MaHoiThoai, testUserId);
                Assert.NotNull(convAfterDalRename);
                Assert.True(convAfterDalRename.TieuDe.Length <= 250);
            }
            finally
            {
                dal.XoaHoiThoai(conv.MaHoiThoai, testUserId, hardDelete: true);
                SessionHelper.CurrentUser = prevUser;
            }
        }

        [Fact]
        public async Task Service_CustomTitle_NotOverwrittenByFirstUserMessage()
        {
            bool canConnect = SqlDataProvider.Instance.TestConnection();
            if (!canConnect) return;

            string userQuery = "SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaNguoiDung > 0 ORDER BY MaNguoiDung ASC";
            DataTable dtUser = SqlDataProvider.Instance.ExecuteQuery(userQuery);
            if (dtUser.Rows.Count == 0) return;
            int testUserId = Convert.ToInt32(dtUser.Rows[0]["MaNguoiDung"]);

            var prevUser = SessionHelper.CurrentUser;
            SessionHelper.CurrentUser = new NguoiDungDTO { MaNguoiDung = testUserId, TenDangNhap = "test_user", TenVaiTro = "SinhVien" };

            var service = AssistantConversationService.Instance;

            // Tạo hội thoại với tiêu đề tùy chỉnh cố định
            string customTitle = "Chủ đề học tập tùy chỉnh cố định";
            var conv = await service.CreateConversationAsync(testUserId, customTitle);
            Assert.NotNull(conv);
            Assert.Equal(customTitle, conv.TieuDe);

            try
            {
                // Gửi tin nhắn user đầu tiên
                await service.AddMessageAsync(conv.MaHoiThoai, "user", "Tin nhắn user đầu tiên");

                // Tiêu đề tùy chỉnh KHÔNG được phép bị ghi đè
                var refreshed = await service.GetConversationByIdAsync(conv.MaHoiThoai, testUserId);
                Assert.NotNull(refreshed);
                Assert.Equal(customTitle, refreshed.TieuDe);
            }
            finally
            {
                AIAssistantDAL.Instance.XoaHoiThoai(conv.MaHoiThoai, testUserId, hardDelete: true);
                SessionHelper.CurrentUser = prevUser;
            }
        }
    }
}
