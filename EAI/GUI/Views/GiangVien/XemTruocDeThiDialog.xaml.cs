using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class XemTruocDeThiDialog : Window
    {
        private MediaPlayer? _mediaPlayer;
        private bool _isPlaying = false;

        public XemTruocDeThiDialog(BaiTapDeThiDTO deThi, List<CauHoiKhaoSatDTO> dsCauHoi)
        {
            InitializeComponent();
            txtTitle.Text = $"Xem trước: {deThi?.TenBaiTapDeThi ?? ""}";
            txtSubtitle.Text = $"{deThi?.LoaiHinhHienThi ?? ""} • {deThi?.ThoiGianHienThi ?? ""} • {dsCauHoi?.Count ?? 0} câu hỏi";

            if (deThi != null && deThi.HasAudio && !string.IsNullOrWhiteSpace(deThi.DuongDanAudio))
            {
                pnlExamAudio.Visibility = Visibility.Visible;
                txtAudioFileName.Text = deThi.TenFileAudioHienThi;
                txtAudioRule.Text = deThi.QuyTacAudioText;

                try
                {
                    string fullPath = Path.IsPathRooted(deThi.DuongDanAudio)
                        ? deThi.DuongDanAudio
                        : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, deThi.DuongDanAudio);

                    if (File.Exists(fullPath))
                    {
                        _mediaPlayer = new MediaPlayer();
                        _mediaPlayer.Open(new Uri(fullPath));
                        _mediaPlayer.MediaEnded += (s, e) =>
                        {
                            _isPlaying = false;
                            btnPlayAudio.Content = "▶ Nghe thử";
                        };
                    }
                }
                catch
                {
                    // Ignore media load error in preview mode
                }
            }
            else
            {
                pnlExamAudio.Visibility = Visibility.Collapsed;
            }

            icQuestions.ItemsSource = dsCauHoi;
        }

        private void BtnTogglePlay_Click(object sender, RoutedEventArgs e)
        {
            if (_mediaPlayer == null) return;

            if (_isPlaying)
            {
                _mediaPlayer.Pause();
                _isPlaying = false;
                btnPlayAudio.Content = "▶ Nghe thử";
            }
            else
            {
                _mediaPlayer.Play();
                _isPlaying = true;
                btnPlayAudio.Content = "⏸ Tạm dừng";
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            StopAudio();
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            StopAudio();
            base.OnClosed(e);
        }

        private void StopAudio()
        {
            try
            {
                if (_mediaPlayer != null)
                {
                    _mediaPlayer.Stop();
                    _mediaPlayer.Close();
                    _mediaPlayer = null;
                }
            }
            catch
            {
                // Ignore cleanup error
            }
        }
    }
}

