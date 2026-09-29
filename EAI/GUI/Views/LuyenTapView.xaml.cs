using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI.Views
{
    public partial class LuyenTapView : Page
    {
        private class FlashcardItem
        {
            public string Word { get; set; } = "";
            public string Ipa { get; set; } = "";
            public string Topic { get; set; } = "";
            public string Meaning { get; set; } = "";
            public string Example { get; set; } = "";
            public string Collocation { get; set; } = "";
        }

        private readonly List<FlashcardItem> _cards = new List<FlashcardItem>
        {
            new FlashcardItem
            {
                Word = "Ubiquitous",
                Ipa = "/juːˈbɪk.wɪ.təs/ (adj)",
                Topic = "ACADEMIC VOCABULARY • C1",
                Meaning = "Phổ biến ở khắp mọi nơi, hiện diện đồng thời ở mọi chỗ (seeming to be everywhere).",
                Example = "\"Smartphones have become ubiquitous in daily modern life, radically transforming how students study.\"",
                Collocation = "• Ubiquitous influence • Ubiquitous presence of technology"
            },
            new FlashcardItem
            {
                Word = "Meticulous",
                Ipa = "/məˈtɪk.jə.ləs/ (adj)",
                Topic = "RESEARCH & SCIENCE • C1",
                Meaning = "Tỉ mỉ, cẩn thận từng chi tiết nhỏ nhất (very careful and with great attention to every detail).",
                Example = "\"The research paper required meticulous attention to detail to avoid statistical discrepancies.\"",
                Collocation = "• Meticulous research • Meticulous planning"
            },
            new FlashcardItem
            {
                Word = "Pragmatic",
                Ipa = "/præɡˈmæt.ɪk/ (adj)",
                Topic = "PROBLEM SOLVING • B2",
                Meaning = "Thực tế, thực dụng, dựa trên tính khả thi thay vì lý thuyết suông.",
                Example = "\"In dealing with environmental issues, governments must take a pragmatic and balanced approach.\"",
                Collocation = "• Pragmatic solution • Pragmatic approach"
            },
            new FlashcardItem
            {
                Word = "Corroborate",
                Ipa = "/kəˈrɒb.ə.reɪt/ (verb)",
                Topic = "EVIDENCE & ARGUMENT • C1",
                Meaning = "Chứng minh, củng cố thêm bằng chứng xác thực (to confirm or support with new evidence).",
                Example = "\"Subsequent geological studies corroborated the theory proposed by scientists a decade earlier.\"",
                Collocation = "• Corroborate evidence • Corroborate findings"
            },
            new FlashcardItem
            {
                Word = "Detrimental",
                Ipa = "/ˌdet.rɪˈmen.təl/ (adj)",
                Topic = "HEALTH & SOCIETY • B2",
                Meaning = "Gây hại, có hại (causing harm or damage).",
                Example = "\"Excessive screen time before sleeping is proven to have a detrimental effect on sleep quality.\"",
                Collocation = "• Detrimental effect • Detrimental impact"
            }
        };

        private int _currentCardIndex = 0;
        private int _score = 150;

        public LuyenTapView()
        {
            InitializeComponent();
            UpdateCardView();
        }

        private void TabFlashcard_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(tabFlashcard, pnlFlashcard);
        }

        private void TabQuiz_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(tabQuiz, pnlQuiz);
        }

        private void TabDictation_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(tabDictation, pnlDictation);
        }

        private void SetActiveTab(Button activeBtn, StackPanel activePnl)
        {
            var brandTeal = (SolidColorBrush)FindResource("BrandTeal");
            var textGray = (SolidColorBrush)FindResource("TextGray");

            tabFlashcard.BorderBrush = Brushes.Transparent;
            tabFlashcard.Foreground = textGray;
            tabFlashcard.FontWeight = FontWeights.Medium;

            tabQuiz.BorderBrush = Brushes.Transparent;
            tabQuiz.Foreground = textGray;
            tabQuiz.FontWeight = FontWeights.Medium;

            tabDictation.BorderBrush = Brushes.Transparent;
            tabDictation.Foreground = textGray;
            tabDictation.FontWeight = FontWeights.Medium;

            activeBtn.BorderBrush = brandTeal;
            activeBtn.Foreground = brandTeal;
            activeBtn.FontWeight = FontWeights.SemiBold;

            pnlFlashcard.Visibility = Visibility.Collapsed;
            pnlQuiz.Visibility = Visibility.Collapsed;
            pnlDictation.Visibility = Visibility.Collapsed;

            activePnl.Visibility = Visibility.Visible;
        }

        private void UpdateCardView()
        {
            var card = _cards[_currentCardIndex];
            txtCardIndex.Text = $"{_currentCardIndex + 1} / {_cards.Count}";
            txtCardWord.Text = card.Word;
            txtCardIpa.Text = card.Ipa;
            txtCardTopic.Text = card.Topic;
            txtCardMeaning.Text = card.Meaning;
            txtCardExample.Text = card.Example;
            txtCardCollocation.Text = card.Collocation;

            cardFront.Visibility = Visibility.Visible;
            cardBack.Visibility = Visibility.Collapsed;
        }

        private void BtnFlipCard_Click(object sender, RoutedEventArgs e)
        {
            cardFront.Visibility = Visibility.Collapsed;
            cardBack.Visibility = Visibility.Visible;
        }

        private void BtnRateCard_Click(object sender, RoutedEventArgs e)
        {
            _score += 10;
            txtUserScore.Text = $"Điểm: {_score} pts";

            _currentCardIndex = (_currentCardIndex + 1) % _cards.Count;
            UpdateCardView();
        }

        private void BtnShuffleCard_Click(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();
            _currentCardIndex = rnd.Next(0, _cards.Count);
            UpdateCardView();
        }

        private void BtnPlayAudio_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🔊 Đang phát audio phát âm chuẩn giọng bản xứ (UK/US Accent)...", "Phát âm chuẩn", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnQuizAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.Name == "btnOptA")
                {
                    btn.Background = new SolidColorBrush(Color.FromRgb(209, 250, 229)); // Xanh nhạt
                    btn.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                    pnlQuizFeedback.Visibility = Visibility.Visible;
                    _score += 20;
                    txtUserScore.Text = $"Điểm: {_score} pts";
                }
                else
                {
                    btn.Background = new SolidColorBrush(Color.FromRgb(254, 226, 226)); // Đỏ nhạt
                    btn.BorderBrush = new SolidColorBrush(Color.FromRgb(239, 68, 68));
                    MessageBox.Show("Chưa chính xác! Đáp án đúng là 'A. implement' (áp dụng/thi hành quy định).", "Gợi ý", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void BtnCheckDictation_Click(object sender, RoutedEventArgs e)
        {
            string input = txtDictationInput.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Vui lòng gõ nội dung bạn nghe được trước khi kiểm tra.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            txtTranscriptResult.Visibility = Visibility.Visible;
            txtTranscriptResult.Text = "✅ Độ chính xác của bạn: 92%\nBản gốc: \"The transition to renewable energy sources is imperative for achieving long-term sustainability.\"";
            _score += 25;
            txtUserScore.Text = $"Điểm: {_score} pts";
        }

        private void BtnShowTranscript_Click(object sender, RoutedEventArgs e)
        {
            txtTranscriptResult.Visibility = Visibility.Visible;
            txtTranscriptResult.Text = "📝 Bản dịch transcript: \"Việc chuyển dịch sang các nguồn năng lượng tái tạo là điều cấp thiết để đạt được tính bền vững lâu dài.\"";
        }
    }
}
