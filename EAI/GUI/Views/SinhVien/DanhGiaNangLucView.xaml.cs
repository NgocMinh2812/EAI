using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace GUI.Views
{
    public partial class DanhGiaNangLucView : Page
    {
        private class QuestionItem
        {
            public string Category { get; set; } = string.Empty;
            public string? Passage { get; set; }
            public string Prompt { get; set; } = string.Empty;
            public string[] Options { get; set; } = Array.Empty<string>();
            public int CorrectIndex { get; set; }
        }

        private readonly List<QuestionItem> _questions = new List<QuestionItem>()
        {
            new QuestionItem
            {
                Category = "NGỮ PHÁP • CÂU ĐIỀU KIỆN (B2)",
                Passage = null,
                Prompt = "Câu 1: If I ______ more time yesterday, I would have traveled around the city with you.",
                Options = new string[] { "have", "had had", "had", "would have" },
                CorrectIndex = 1
            },
            new QuestionItem
            {
                Category = "TỪ VỰNG • HỌC THUẬT (C1)",
                Passage = null,
                Prompt = "Câu 2: The university decided to ______ innovative teaching methods to stimulate students' critical thinking.",
                Options = new string[] { "adopt", "adapt", "adept", "admit" },
                CorrectIndex = 0
            },
            new QuestionItem
            {
                Category = "NGỮ PHÁP • GIỚI TỪ & COLLOCATION (B1)",
                Passage = null,
                Prompt = "Câu 3: She has always been capable ______ solving complicated mathematical equations under pressure.",
                Options = new string[] { "with", "in", "of", "for" },
                CorrectIndex = 2
            },
            new QuestionItem
            {
                Category = "ĐỌC HIỂU • BÀI ĐỌC NGẮN (B2-C1)",
                Passage = "Artificial Intelligence is increasingly integrated into modern education, providing personalized pathways based on real-time learner diagnostics. By continuously assessing grammar precision and lexical range, AI systems adapt subsequent exercises to optimize language acquisition without causing cognitive overload.",
                Prompt = "Câu 4: According to the passage, how does AI optimize language learning?",
                Options = new string[]
                {
                    "By replacing traditional classroom teachers completely.",
                    "By tailoring exercises based on real-time learner diagnostics.",
                    "By making all grammar rules and vocabulary lists obsolete.",
                    "By giving students unlimited time for examination."
                },
                CorrectIndex = 1
            },
            new QuestionItem
            {
                Category = "THÀNH NGỮ & GIAO TIẾP TỰ NHIÊN (B2)",
                Passage = null,
                Prompt = "Câu 5: When managing a large-scale project, it is crucial not to get bogged down in the ______.",
                Options = new string[] { "details", "skies", "moments", "pictures" },
                CorrectIndex = 0
            }
        };

        private int _currentIndex = 0;
        private readonly int[] _userAnswers = new int[5] { -1, -1, -1, -1, -1 };
        private DispatcherTimer? _timer;
        private int _secondsRemaining = 900; // 15 phút

        public DanhGiaNangLucView()
        {
            InitializeComponent();
            StartTimer();
            LoadQuestion(0);
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) =>
            {
                if (_secondsRemaining > 0)
                {
                    _secondsRemaining--;
                    int minutes = _secondsRemaining / 60;
                    int seconds = _secondsRemaining % 60;
                    txtTimer.Text = $"{minutes:D2}:{seconds:D2}";
                }
                else
                {
                    _timer.Stop();
                    BtnSubmit_Click(null, null);
                }
            };
            _timer.Start();
        }

        private void LoadQuestion(int index)
        {
            _currentIndex = index;
            var q = _questions[index];

            txtProgressCount.Text = $"Câu {index + 1} / {_questions.Count}";
            testProgressBar.Value = ((index + 1) * 100.0) / _questions.Count;

            txtCategoryBadge.Text = q.Category;
            txtQuestionPrompt.Text = q.Prompt;

            if (!string.IsNullOrEmpty(q.Passage))
            {
                passageBox.Visibility = Visibility.Visible;
                txtPassage.Text = q.Passage;
            }
            else
            {
                passageBox.Visibility = Visibility.Collapsed;
            }

            txtOptionA.Text = q.Options[0];
            txtOptionB.Text = q.Options[1];
            txtOptionC.Text = q.Options[2];
            txtOptionD.Text = q.Options[3];

            // Restore previously selected answer
            int ans = _userAnswers[index];
            rbOptionA.IsChecked = (ans == 0);
            rbOptionB.IsChecked = (ans == 1);
            rbOptionC.IsChecked = (ans == 2);
            rbOptionD.IsChecked = (ans == 3);

            // Previous button enabled?
            btnPrev.IsEnabled = (index > 0);

            // Next vs Submit button
            if (index == _questions.Count - 1)
            {
                btnNext.Visibility = Visibility.Collapsed;
                btnSubmit.Visibility = Visibility.Visible;
            }
            else
            {
                btnNext.Visibility = Visibility.Visible;
                btnSubmit.Visibility = Visibility.Collapsed;
            }

            UpdateNextButtonState();
        }

        private void SaveCurrentSelection()
        {
            if (rbOptionA.IsChecked == true) _userAnswers[_currentIndex] = 0;
            else if (rbOptionB.IsChecked == true) _userAnswers[_currentIndex] = 1;
            else if (rbOptionC.IsChecked == true) _userAnswers[_currentIndex] = 2;
            else if (rbOptionD.IsChecked == true) _userAnswers[_currentIndex] = 3;
            else _userAnswers[_currentIndex] = -1;
        }

        private void UpdateNextButtonState()
        {
            bool hasAnswer = (_userAnswers[_currentIndex] >= 0);
            btnNext.IsEnabled = hasAnswer;
            btnSubmit.IsEnabled = hasAnswer;
        }

        private void Option_Checked(object sender, RoutedEventArgs e)
        {
            SaveCurrentSelection();
            UpdateNextButtonState();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentSelection();
            if (_currentIndex > 0)
            {
                LoadQuestion(_currentIndex - 1);
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentSelection();
            if (_userAnswers[_currentIndex] < 0)
            {
                MessageBox.Show("Vui lòng chọn một đáp án cho câu hỏi trước khi tiếp tục.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentIndex < _questions.Count - 1)
            {
                LoadQuestion(_currentIndex + 1);
            }
        }

        private void BtnSubmit_Click(object? sender, RoutedEventArgs? e)
        {
            SaveCurrentSelection();
            if (_userAnswers[_currentIndex] < 0)
            {
                MessageBox.Show("Vui lòng chọn đáp án cho câu hỏi này trước khi nộp bài.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _timer?.Stop();

            // Tính điểm
            int correctCount = 0;
            for (int i = 0; i < _questions.Count; i++)
            {
                if (_userAnswers[i] == _questions[i].CorrectIndex)
                {
                    correctCount++;
                }
            }

            txtResultScore.Text = $"Bạn đã hoàn thành bài kiểm tra và trả lời đúng {correctCount}/{_questions.Count} câu hỏi.";

            // Ước tính trình độ
            if (correctCount >= 4)
            {
                txtEstimatedLevel.Text = "CEFR B2 • IELTS 6.5";
            }
            else if (correctCount >= 2)
            {
                txtEstimatedLevel.Text = "CEFR B1 • IELTS 5.0 - 5.5";
            }
            else
            {
                txtEstimatedLevel.Text = "CEFR A2 • Mất gốc / Cần củng cố nền tảng";
            }

            ResultOverlay.Visibility = Visibility.Visible;
        }

        private void BtnSkip_Click(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
            // Bỏ qua đánh giá năng lực và vào thẳng trang chủ
            this.NavigationService?.Navigate(new TrangChuView());
        }

        private void BtnGoHome_Click(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
            this.NavigationService?.Navigate(new TrangChuView());
        }
    }
}
