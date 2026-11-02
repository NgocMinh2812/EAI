using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BUS.AI
{
    public static class WavAudioHelper
    {
        /// <summary>
        /// Tính SHA256 mã băm tệp tin để chống gọi AI trùng lặp
        /// </summary>
        public static string TinhFileHash(string filePath)
        {
            if (!File.Exists(filePath)) return string.Empty;
            try
            {
                using var sha256 = SHA256.Create();
                using var fs = File.OpenRead(filePath);
                byte[] hash = sha256.ComputeHash(fs);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Tính SHA256 mã băm chuỗi văn bản (script, voice, model)
        /// </summary>
        public static string TinhStringHash(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Chia nhỏ script thành từng đoạn/câu hợp lý đảm bảo không vượt quá giới hạn 180 ký tự của Groq Orpheus TTS
        /// </summary>
        public static List<string> TachScriptThanhChunks(string script, int maxCharsPerChunk = 175)
        {
            var chunks = new List<string>();
            if (string.IsNullOrWhiteSpace(script)) return chunks;

            string normalized = script.Replace("\r\n", "\n").Trim();
            // Tách theo câu dựa trên dấu chấm, hỏi, than hoặc xuống dòng
            var rawSentences = Regex.Split(normalized, @"(?<=[.?!;:\n])\s+");

            var currentSb = new StringBuilder();

            foreach (var raw in rawSentences)
            {
                string sentence = raw.Trim();
                if (string.IsNullOrEmpty(sentence)) continue;

                // Nếu riêng câu này đã dài hơn maxCharsPerChunk, tách nhỏ theo dấu phẩy hoặc từ
                if (sentence.Length > maxCharsPerChunk)
                {
                    if (currentSb.Length > 0)
                    {
                        chunks.Add(currentSb.ToString().Trim());
                        currentSb.Clear();
                    }

                    var subClauses = Regex.Split(sentence, @"(?<=[,])\s+");
                    foreach (var clause in subClauses)
                    {
                        if (currentSb.Length + clause.Length + 1 <= maxCharsPerChunk)
                        {
                            if (currentSb.Length > 0) currentSb.Append(" ");
                            currentSb.Append(clause);
                        }
                        else
                        {
                            if (currentSb.Length > 0)
                            {
                                chunks.Add(currentSb.ToString().Trim());
                                currentSb.Clear();
                            }

                            // Nếu một vế vẫn quá dài, cắt theo từ
                            if (clause.Length > maxCharsPerChunk)
                            {
                                var words = clause.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                                foreach (var w in words)
                                {
                                    if (currentSb.Length + w.Length + 1 <= maxCharsPerChunk)
                                    {
                                        if (currentSb.Length > 0) currentSb.Append(" ");
                                        currentSb.Append(w);
                                    }
                                    else
                                    {
                                        if (currentSb.Length > 0)
                                        {
                                            chunks.Add(currentSb.ToString().Trim());
                                            currentSb.Clear();
                                        }
                                        currentSb.Append(w);
                                    }
                                }
                            }
                            else
                            {
                                currentSb.Append(clause);
                            }
                        }
                    }
                }
                else
                {
                    if (currentSb.Length + sentence.Length + 1 <= maxCharsPerChunk)
                    {
                        if (currentSb.Length > 0) currentSb.Append(" ");
                        currentSb.Append(sentence);
                    }
                    else
                    {
                        if (currentSb.Length > 0)
                        {
                            chunks.Add(currentSb.ToString().Trim());
                            currentSb.Clear();
                        }
                        currentSb.Append(sentence);
                    }
                }
            }

            if (currentSb.Length > 0)
            {
                chunks.Add(currentSb.ToString().Trim());
            }

            return chunks;
        }

        /// <summary>
        /// Ghép nhiều tệp/mảng byte WAV PCM thành một tệp WAV chuẩn duy nhất
        /// </summary>
        public static byte[] GhepDanhSachWav(List<byte[]> wavBytesList, int sampleRate = 24000, short channels = 1, short bitsPerSample = 16)
        {
            if (wavBytesList == null || wavBytesList.Count == 0) return Array.Empty<byte>();
            if (wavBytesList.Count == 1) return wavBytesList[0];

            using var pcmDataStream = new MemoryStream();

            foreach (var wavBytes in wavBytesList)
            {
                if (wavBytes == null || wavBytes.Length < 44) continue;

                // Trích xuất dữ liệu PCM thuần (bỏ qua 44 byte RIFF header)
                int dataOffset = TimOffsetDuLieuPcm(wavBytes, out int pcmLength);
                if (dataOffset > 0 && pcmLength > 0 && dataOffset + pcmLength <= wavBytes.Length)
                {
                    pcmDataStream.Write(wavBytes, dataOffset, pcmLength);
                }
                else if (wavBytes.Length > 44)
                {
                    pcmDataStream.Write(wavBytes, 44, wavBytes.Length - 44);
                }
            }

            byte[] combinedPcm = pcmDataStream.ToArray();

            // Tạo header chuẩn cho file WAV tổng hợp
            return TaoWavHeader(combinedPcm, sampleRate, channels, bitsPerSample);
        }

        /// <summary>
        /// Tạo khoảng lặng âm thanh (silence) định dạng PCM WAV để chèn giữa các lượt thoại
        /// </summary>
        public static byte[] TaoKhoangLangWav(double seconds, int sampleRate = 24000, short channels = 1, short bitsPerSample = 16)
        {
            if (seconds <= 0) return Array.Empty<byte>();
            int byteRate = sampleRate * channels * (bitsPerSample / 8);
            int silenceBytesLength = (int)(seconds * byteRate);
            if (silenceBytesLength % 2 != 0) silenceBytesLength++;

            byte[] silencePcm = new byte[silenceBytesLength];
            return TaoWavHeader(silencePcm, sampleRate, channels, bitsPerSample);
        }

        public static byte[] TaoWavHeader(byte[] pcmData, int sampleRate, short channels, short bitsPerSample)
        {
            int byteRate = sampleRate * channels * (bitsPerSample / 8);
            short blockAlign = (short)(channels * (bitsPerSample / 8));
            int dataLength = pcmData.Length;
            int totalLength = 36 + dataLength;

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);

            bw.Write(new char[] { 'R', 'I', 'F', 'F' });
            bw.Write(totalLength);
            bw.Write(new char[] { 'W', 'A', 'V', 'E' });
            bw.Write(new char[] { 'f', 'm', 't', ' ' });
            bw.Write(16); // Subchunk1Size (16 for PCM)
            bw.Write((short)1); // AudioFormat (1 for PCM)
            bw.Write(channels);
            bw.Write(sampleRate);
            bw.Write(byteRate);
            bw.Write(blockAlign);
            bw.Write(bitsPerSample);
            bw.Write(new char[] { 'd', 'a', 't', 'a' });
            bw.Write(dataLength);
            bw.Write(pcmData);

            return ms.ToArray();
        }

        private static int TimOffsetDuLieuPcm(byte[] wavBytes, out int dataLength)
        {
            dataLength = 0;
            if (wavBytes.Length < 44) return 0;

            for (int i = 12; i < wavBytes.Length - 8; i++)
            {
                if (wavBytes[i] == 'd' && wavBytes[i + 1] == 'a' && wavBytes[i + 2] == 't' && wavBytes[i + 3] == 'a')
                {
                    dataLength = BitConverter.ToInt32(wavBytes, i + 4);
                    return i + 8;
                }
            }

            // Fallback chuẩn 44 bytes
            dataLength = wavBytes.Length - 44;
            return 44;
        }
    }
}

