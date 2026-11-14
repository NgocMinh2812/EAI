using System;
using System.Runtime.Versioning;
using System.Speech.Synthesis;

#pragma warning disable CA1416 // Validate platform compatibility

namespace BUS.Services
{
    /// <summary>
    /// Bộ điều phối âm thanh trung tâm: Quản lý tính loại trừ tương hỗ giữa MediaPlayer và TTS SpeechSynthesizer,
    /// triệt tiêu hoàn toàn hiện tượng chồng chéo âm thanh (Audio/TTS Overlap).
    /// </summary>
    public class AudioPlaybackCoordinator
    {
        private static AudioPlaybackCoordinator? _instance;
        public static AudioPlaybackCoordinator Instance => _instance ??= new AudioPlaybackCoordinator();

        private readonly object _lock = new();
        private SpeechSynthesizer? _speechSynthesizer;
        private Action? _activeMediaPlayerPauser;

        public bool IsMediaPlayerPlaying { get; set; }
        public bool IsSpeaking { get; set; }
        public int SpeechCancelCount { get; private set; }
        public int MediaPlayerPauseCount { get; private set; }

        public AudioPlaybackCoordinator()
        {
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    _speechSynthesizer = new SpeechSynthesizer();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AudioCoordinator] Không thể khởi tạo SpeechSynthesizer: {ex.Message}");
            }
        }

        /// <summary>
        /// Đăng ký hàm tạm dừng/dừng của MediaPlayer đang tích cực trong View hiện tại.
        /// </summary>
        public void RegisterActiveMediaPlayer(Action? pauser)
        {
            lock (_lock)
            {
                _activeMediaPlayerPauser = pauser;
            }
        }

        /// <summary>
        /// Hủy đăng ký MediaPlayer khi view đóng hoặc chuyển bài
        /// </summary>
        public void UnregisterActiveMediaPlayer(Action pauser)
        {
            lock (_lock)
            {
                if (_activeMediaPlayerPauser == pauser)
                {
                    _activeMediaPlayerPauser = null;
                }
            }
        }

        /// <summary>
        /// Hủy ngay lập tức toàn bộ giọng đọc TTS đang phát hoặc đang chờ trong hàng đợi.
        /// </summary>
        public void StopAllSpeech()
        {
            lock (_lock)
            {
                SpeechCancelCount++;
                IsSpeaking = false;
                if (_speechSynthesizer != null)
                {
                    try
                    {
                        _speechSynthesizer.SpeakAsyncCancelAll();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[AudioCoordinator] Lỗi SpeakAsyncCancelAll: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Khi bất kỳ MediaPlayer nào bắt đầu phát (Play), thông báo cho Coordinator để dừng ngay mọi giọng đọc TTS.
        /// </summary>
        public void NotifyMediaPlayerStarting()
        {
            lock (_lock)
            {
                IsMediaPlayerPlaying = true;
                StopAllSpeech();
            }
        }

        /// <summary>
        /// Phát giọng đọc TTS bất đồng bộ có điều phối:
        /// Tự động dừng mọi phát âm TTS trước đó, đồng thời tạm dừng MediaPlayer đang phát.
        /// </summary>
        public void SpeakAsync(string text, double speed = 1.0, int volume = 100, Action? onCompleted = null)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            lock (_lock)
            {
                // 1. Tạm dừng MediaPlayer đang phát nếu có
                if (_activeMediaPlayerPauser != null)
                {
                    try
                    {
                        MediaPlayerPauseCount++;
                        IsMediaPlayerPlaying = false;
                        _activeMediaPlayerPauser.Invoke();
                    }
                    catch { }
                }

                // 2. Hủy các tác vụ speech đang chạy trước đó
                StopAllSpeech();

                IsSpeaking = true;

                if (_speechSynthesizer == null)
                {
                    try
                    {
                        if (OperatingSystem.IsWindows())
                        {
                            _speechSynthesizer = new SpeechSynthesizer();
                        }
                    }
                    catch
                    {
                        IsSpeaking = false;
                        onCompleted?.Invoke();
                        return;
                    }
                }

                if (_speechSynthesizer != null)
                {
                    try
                    {
                        _speechSynthesizer.Volume = Math.Clamp(volume, 0, 100);
                        _speechSynthesizer.Rate = Math.Clamp((int)((speed - 1.0) * 4), -10, 10);

                        EventHandler<SpeakCompletedEventArgs>? handler = null;
                        handler = (s, e) =>
                        {
                            lock (_lock)
                            {
                                IsSpeaking = false;
                            }
                            try
                            {
                                if (_speechSynthesizer != null)
                                {
                                    _speechSynthesizer.SpeakCompleted -= handler;
                                }
                            }
                            catch { }
                            onCompleted?.Invoke();
                        };
                        _speechSynthesizer.SpeakCompleted += handler;

                        _speechSynthesizer.SpeakAsync(text);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[AudioCoordinator] Lỗi SpeakAsync: {ex.Message}");
                        IsSpeaking = false;
                    }
                }
                else
                {
                    IsSpeaking = false;
                    onCompleted?.Invoke();
                }
            }
        }
    }
}
