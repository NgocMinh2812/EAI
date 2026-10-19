using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Views
{
    public partial class TroLyAIView : Page
    {
        public TroLyAIView()
        {
            InitializeComponent();
        }

        private void BtnPrompt_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string prompt = btn.Content.ToString() ?? "";
                if (prompt.StartsWith("💡 ") || prompt.StartsWith("📝 ") || prompt.StartsWith("📚 ") || prompt.StartsWith("🗣️ "))
                {
                    prompt = prompt.Substring(2).Trim();
                }
                txtChatInput.Text = prompt;
                SendMessage(prompt);
            }
        }

        private void TxtChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSendMessage_Click(sender, e);
            }
        }

        private void BtnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = txtChatInput.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            SendMessage(message);
            txtChatInput.Text = "";
        }

        private void SendMessage(string userMsg)
        {
            // 1. Add User Message
            var userBorder = new Border
            {
                Background = (SolidColorBrush)FindResource("BrandTeal"),
                CornerRadius = new CornerRadius(16),
                Padding = new Thickness(18, 12, 18, 12),
                Margin = new Thickness(80, 0, 10, 16),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            var userText = new TextBlock
            {
                Text = userMsg,
                Foreground = Brushes.White,
                FontSize = 14,
                LineHeight = 22,
                TextWrapping = TextWrapping.Wrap
            };
            userBorder.Child = userText;
            pnlChatMessages.Children.Add(userBorder);

            // 2. Generate AI Response
            string aiResponse = GenerateAIResponse(userMsg);

            var botGrid = new Grid { Margin = new Thickness(0, 0, 0, 20) };
            botGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(42) });
            botGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var botAvatar = new Border
            {
                Background = (SolidColorBrush)FindResource("BrandTealLight"),
                Width = 38,
                Height = 38,
                CornerRadius = new CornerRadius(19),
                VerticalAlignment = VerticalAlignment.Top
            };
            var botIcon = new TextBlock
            {
                Text = "🤖",
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            botAvatar.Child = botIcon;
            Grid.SetColumn(botAvatar, 0);

            var botContentBorder = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(16),
                BorderBrush = (SolidColorBrush)FindResource("BorderLight"),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(18, 14, 18, 14),
                Margin = new Thickness(10, 0, 80, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var botStack = new StackPanel();
            var botName = new TextBlock
            {
                Text = "Trợ lý TiếngAnhAI",
                FontWeight = FontWeights.Bold,
                FontSize = 12.5,
                Foreground = (SolidColorBrush)FindResource("BrandTeal"),
                Margin = new Thickness(0, 0, 0, 6)
            };
            var botReply = new TextBlock
            {
                Text = aiResponse,
                FontSize = 14,
                LineHeight = 22,
                TextWrapping = TextWrapping.Wrap,
                Foreground = (SolidColorBrush)FindResource("TextDark")
            };
            botStack.Children.Add(botName);
            botStack.Children.Add(botReply);
            botContentBorder.Child = botStack;
            Grid.SetColumn(botContentBorder, 1);

            botGrid.Children.Add(botAvatar);
            botGrid.Children.Add(botContentBorder);
            pnlChatMessages.Children.Add(botGrid);

            chatScrollViewer.ScrollToEnd();
        }

        private string GenerateAIResponse(string query)
        {
            query = query.ToLower();
            if (query.Contains("present perfect") || query.Contains("past simple") || query.Contains("hiện tại hoàn thành"))
            {
                return "💡 Phân biệt nhanh Hiện tại hoàn thành (Present Perfect) & Quá khứ đơn (Past Simple):\n\n" +
                       "1. Quá khứ đơn (S + V2/ed): Dùng cho hành động ĐÃ CHẤM DỨT hoàn toàn trong quá khứ, có mốc thời gian cụ thể (yesterday, in 2020, ago).\n" +
                       "Ví dụ: 'I visited Paris in 2019.' (Năm 2019 đã qua và hành động đã kết thúc).\n\n" +
                       "2. Hiện tại hoàn thành (S + have/has + V3/ed): Dùng cho trải nghiệm tính tới nay, hoặc hành động quá khứ để lại KẾT QUẢ/HẬU QUẢ ở hiện tại (since, for, already, yet).\n" +
                       "Ví dụ: 'I have lost my key.' (Tôi đã làm mất chìa khóa và hiện tại vẫn chưa vào được nhà).";
            }
            if (query.Contains("environment") || query.Contains("môi trường") || query.Contains("collocation"))
            {
                return "🌿 5 Collocations học thuật cực kỳ ăn điểm về chủ đề Environment (IELTS Band 7.5+):\n\n" +
                       "1. Combat climate change: Đấu tranh chống lại biến đổi khí hậu.\n" +
                       "2. Renewable energy sources: Các nguồn năng lượng tái tạo (solar, wind).\n" +
                       "3. Drastic reduction in emissions: Cắt giảm khí thải quyết liệt.\n" +
                       "4. Ecological footprint: Dấu chân sinh thái / Tác động môi trường.\n" +
                       "5. Sustainable development: Phát triển bền vững.";
            }
            if (query.Contains("sửa") || query.Contains("ngữ pháp") || query.Contains("câu này"))
            {
                return "✍️ AI sẵn sàng sửa câu cho bạn! Hãy gõ câu tiếng Anh bạn muốn kiểm tra (ví dụ: 'He go to school yesterday because he is sick') vào khung chat, mình sẽ phân tích lỗi sai và đề xuất câu viết chuẩn bản xứ ngay nhé!";
            }
            if (query.Contains("phỏng vấn") || query.Contains("job") || query.Contains("interview"))
            {
                return "💼 3 Mẫu câu trả lời phỏng vấn xin việc (Job Interview) chuyên nghiệp:\n\n" +
                       "• Giới thiệu thế mạnh: 'My primary strength lies in problem-solving and cross-functional team collaboration.'\n" +
                       "• Động lực ứng tuyển: 'I am particularly drawn to this role because your company fosters innovation and career growth.'\n" +
                       "• Ứng phó áp lực: 'I thrive in fast-paced environments by effectively prioritizing tasks and maintaining clear communication.'";
            }

            return $"✨ Cảm ơn bạn đã hỏi về '{query}'!\n" +
                   "Gia sư AI đã ghi nhận yêu cầu của bạn. Trong tiếng Anh, việc áp dụng từ vựng theo ngữ cảnh cụ thể và cấu trúc ngữ pháp chuẩn xác sẽ giúp bạn đạt điểm cao trong các kỳ thi quốc tế như IELTS/TOEIC. Bạn muốn mình giải thích chi tiết hơn về mặt Ngữ pháp hay luyện thêm Bài tập ví dụ của phần này?";
        }
    }
}
