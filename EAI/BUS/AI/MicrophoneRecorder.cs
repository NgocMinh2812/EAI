using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace BUS.AI
{
    public class MicrophoneRecorder
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int MciSendString(string lpstrCommand, StringBuilder? lpstrReturnString, int uReturnLength, IntPtr hwndCallback);

        private bool _isRecording;
        private string? _currentTempFile;

        public bool IsRecording => _isRecording;
        public string? CurrentTempFile => _currentTempFile;

        /// <summary>
        /// Bắt đầu ghi âm từ microphone thông qua Windows Multimedia API.
        /// Ghi âm định dạng WAV chuẩn 16-bit PCM Mono.
        /// </summary>
        public bool StartRecording(out string error)
        {
            error = string.Empty;
            if (_isRecording)
            {
                error = "Hệ thống đang trong trạng thái ghi âm.";
                return false;
            }

            try
            {
                // Đóng phiên ghi âm cũ nếu còn sót
                MciSendString("close eai_rec", null, 0, IntPtr.Zero);

                string tempDir = Path.Combine(Path.GetTempPath(), "EAI_AudioRecordings");
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                _currentTempFile = Path.Combine(tempDir, $"speak_{Guid.NewGuid():N}.wav");

                // Mở waveaudio mới: 16-bit mono 16000Hz (chuẩn tối ưu cho Whisper STT)
                int res = MciSendString("open new Type waveaudio Alias eai_rec", null, 0, IntPtr.Zero);
                if (res != 0)
                {
                    error = "Không thể khởi động thiết bị microphone trên hệ điều hành (MCI Error Code: " + res + "). Vui lòng kiểm tra quyền truy cập microphone.";
                    return false;
                }

                MciSendString("set eai_rec bitspersample 16", null, 0, IntPtr.Zero);
                MciSendString("set eai_rec channels 1", null, 0, IntPtr.Zero);
                MciSendString("set eai_rec samplespersec 16000", null, 0, IntPtr.Zero);
                MciSendString("set eai_rec bytespersec 32000", null, 0, IntPtr.Zero);
                MciSendString("set eai_rec alignment 2", null, 0, IntPtr.Zero);

                int recordRes = MciSendString("record eai_rec", null, 0, IntPtr.Zero);
                if (recordRes != 0)
                {
                    error = "Lỗi khi bắt đầu ghi âm microphone (Code: " + recordRes + ").";
                    MciSendString("close eai_rec", null, 0, IntPtr.Zero);
                    return false;
                }

                _isRecording = true;
                return true;
            }
            catch (Exception ex)
            {
                error = $"Lỗi khởi tạo micro: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Dừng ghi âm và lưu ra tệp WAV tạm
        /// </summary>
        public bool StopRecording(out string recordedFilePath, out string error)
        {
            recordedFilePath = string.Empty;
            error = string.Empty;

            if (!_isRecording || string.IsNullOrEmpty(_currentTempFile))
            {
                error = "Chưa có phiên ghi âm nào đang chạy.";
                return false;
            }

            try
            {
                // Lưu tệp WAV
                MciSendString($"save eai_rec \"{_currentTempFile}\"", null, 0, IntPtr.Zero);
                MciSendString("close eai_rec", null, 0, IntPtr.Zero);

                _isRecording = false;

                if (File.Exists(_currentTempFile) && new FileInfo(_currentTempFile).Length > 44)
                {
                    recordedFilePath = _currentTempFile;
                    return true;
                }
                else
                {
                    error = "Tệp ghi âm không có tín hiệu âm thanh hoặc thiết bị micro chưa thu được tiếng.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = $"Lỗi kết thúc ghi âm: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Xóa bỏ an toàn tệp ghi âm tạm nhằm tuân thủ quy tắc bảo mật và quyền riêng tư của sinh viên
        /// </summary>
        public void CleanupTempFile()
        {
            try
            {
                if (!string.IsNullOrEmpty(_currentTempFile) && File.Exists(_currentTempFile))
                {
                    File.Delete(_currentTempFile);
                }
            }
            catch { }
            _currentTempFile = null;
        }
    }
}

