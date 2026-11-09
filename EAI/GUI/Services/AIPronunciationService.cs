using System;
using System.IO;
using System.Net.Http;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using BUS.AI;

namespace GUI.Services
{
    public class AIPronunciationService
    {
        private static AIPronunciationService? _instance;
        public static AIPronunciationService Instance => _instance ??= new AIPronunciationService();

        private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(8) };
        private MediaPlayer? _mediaPlayer;

        public static string GetAudioCacheDirectory()
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "AudioResources", "Pronunciations");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        /// <summary>
        /// Phát âm từ vựng tiếng Anh bằng giọng AI bản xứ tự nhiên (có bộ đệm cache cục bộ).
        /// </summary>
        public async Task PlayPronunciationAsync(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return;

            // Làm sạch từ nếu có phần ghi chú trong ngoặc (ví dụ "Ubiquitous (adj)" -> "Ubiquitous")
            string cleanWord = word.Split('(')[0].Trim();
            if (string.IsNullOrWhiteSpace(cleanWord)) return;

            // Đường dẫn tệp cache theo tên từ vựng
            string safeName = string.Join("_", cleanWord.Split(Path.GetInvalidFileNameChars())).ToLowerInvariant();
            string cachePath = Path.Combine(GetAudioCacheDirectory(), $"{safeName}.mp3");

            // 1. Nếu tệp âm thanh AI đã có trong cache, phát lại ngay lập tức
            if (File.Exists(cachePath) && new FileInfo(cachePath).Length > 200)
            {
                PlayAudioFile(cachePath);
                return;
            }

            // 2. Tải phát âm AI giọng bản xứ từ dịch vụ AI Google TTS / Groq TTS
            bool downloaded = await DownloadGoogleAITtsAsync(cleanWord, cachePath);

            // 3. Nếu tải không thành công, thử qua Groq AI TTS (mô hình Orpheus)
            if (!downloaded)
            {
                downloaded = await DownloadGroqAITtsAsync(cleanWord, cachePath);
            }

            // 4. Phát tệp âm thanh AI tải về được, hoặc fallback về SAPI cục bộ nếu không có mạng
            if (downloaded && File.Exists(cachePath))
            {
                PlayAudioFile(cachePath);
            }
            else
            {
                PlayLocalSpeechSynthesizer(cleanWord);
            }
        }

        private async Task<bool> DownloadGoogleAITtsAsync(string text, string outputPath)
        {
            try
            {
                string url = $"https://translate.google.com/translate_tts?ie=UTF-8&tl=en&client=tw-ob&q={Uri.EscapeDataString(text)}";
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    byte[] bytes = await response.Content.ReadAsByteArrayAsync();
                    if (bytes.Length > 200)
                    {
                        await File.WriteAllBytesAsync(outputPath, bytes);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AIPronunciation] Lỗi Google AI TTS: {ex.Message}");
            }
            return false;
        }

        private async Task<bool> DownloadGroqAITtsAsync(string text, string outputPath)
        {
            try
            {
                var (success, result, _) = await GroqAudioService.Instance.GenerateSpeechAsync(text, voice: "hannah");
                if (success && result != null && !string.IsNullOrEmpty(result.AudioPath) && File.Exists(result.AudioPath))
                {
                    File.Copy(result.AudioPath, outputPath, overwrite: true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AIPronunciation] Lỗi Groq AI TTS: {ex.Message}");
            }
            return false;
        }

        private void PlayAudioFile(string filePath)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                try
                {
                    _mediaPlayer ??= new MediaPlayer();
                    _mediaPlayer.Stop();
                    _mediaPlayer.Open(new Uri(filePath, UriKind.Absolute));
                    _mediaPlayer.Play();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[AIPronunciation] Lỗi MediaPlayer: {ex.Message}");
                    PlayLocalSpeechSynthesizer(Path.GetFileNameWithoutExtension(filePath));
                }
            });
        }

        private void PlayLocalSpeechSynthesizer(string word)
        {
            Task.Run(() =>
            {
                try
                {
                    using var synth = new SpeechSynthesizer();
                    synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
                    synth.Rate = -1;
                    synth.Volume = 100;
                    synth.Speak(word);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[AIPronunciation] Lỗi SpeechSynthesizer: {ex.Message}");
                }
            });
        }
    }
}

