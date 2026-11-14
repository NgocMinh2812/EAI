using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BUS;
using DAL;
using DTO;
using Xunit;

namespace EAI.Tests
{
    public class AISinhDeFeatureTests
    {
        [Fact]
        public async Task Test_01_SinhCauHoiVaLuuHangDoi()
        {
            // 1. Kiểm tra gọi AI sinh 5 câu hỏi ngữ pháp B1
            string prompt = "5 câu ngữ pháp Thì Hiện Tại Hoàn Thành, B1, kèm giải thích chi tiết";
            var (questions, error) = await AISinhDeBUS.Instance.SinhCauHoiVaLuuHangDoiAsync(
                prompt: prompt,
                level: "B1",
                count: 5,
                skill: "NguPhap",
                questionType: "TracNghiem",
                modelSelection: "",
                userId: 0
            );

            Assert.Null(error);
            Assert.NotNull(questions);
            Assert.True(questions.Count > 0, "AI phải trả về ít nhất 1 câu hỏi hợp lệ");

            // 2. Kiểm tra dữ liệu được nạp vào Hàng đợi kiểm duyệt
            var pendingList = AISinhDeBUS.Instance.LayDanhSachChoDuyet();
            Assert.NotNull(pendingList);
            Assert.True(pendingList.Count >= questions.Count, "Hàng đợi phải chứa các câu hỏi vừa tạo");

            var firstQ = pendingList.FirstOrDefault(x => x.MaCauHoiAISinh == questions[0].MaCauHoiAISinh);
            Assert.NotNull(firstQ);
            Assert.Equal("DangCho", firstQ.TrangThaiDuyet);
            Assert.False(string.IsNullOrWhiteSpace(firstQ.NoiDungCauHoi));
            Assert.True(firstQ.DanhSachLuaChon.Count >= 2, "Câu hỏi trắc nghiệm phải có ít nhất 2 đáp án");
        }

        [Fact]
        public async Task Test_02_CapNhatNoiDungCauHoi_Sua()
        {
            // Đảm bảo có câu hỏi trong hàng đợi
            var pendingList = AISinhDeBUS.Instance.LayDanhSachChoDuyet();
            if (pendingList == null || pendingList.Count == 0)
            {
                await Test_01_SinhCauHoiVaLuuHangDoi();
                pendingList = AISinhDeBUS.Instance.LayDanhSachChoDuyet();
            }

            var itemToEdit = pendingList.First();
            string updatedContent = itemToEdit.NoiDungCauHoi + " [Đã kiểm định]";
            string updatedExplain = "Giải thích cập nhật: Thì hiện tại hoàn thành diễn tả hành động bắt đầu trong quá khứ kéo dài đến hiện tại.";

            var options = new List<LuaChonDapAnDTO>
            {
                new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "has gone", LaDapAnDung = true },
                new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "have gone", LaDapAnDung = false },
                new LuaChonDapAnDTO { KyHieuLuaChon = "C", NoiDungLuaChon = "went", LaDapAnDung = false },
                new LuaChonDapAnDTO { KyHieuLuaChon = "D", NoiDungLuaChon = "going", LaDapAnDung = false }
            };

            var jsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = false,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string jsonLuaChon = System.Text.Json.JsonSerializer.Serialize(options, jsonOptions);

            bool ok = AISinhDeBUS.Instance.CapNhatNoiDungCauHoiAISinh(
                itemToEdit.MaCauHoiAISinh,
                updatedContent,
                itemToEdit.DoanVanDoc,
                updatedExplain,
                jsonLuaChon,
                out string err,
                kyNang: itemToEdit.KyNang,
                trinhDo: itemToEdit.MaCapDo,
                loaiCauHoi: itemToEdit.LoaiCauHoi,
                doKho: itemToEdit.DoKho
            );

            Assert.True(ok, $"Cập nhật câu hỏi thất bại: {err}");

            // Tải lại và kiểm tra nội dung đã sửa
            var reloadedList = AISinhDeBUS.Instance.LayDanhSachChoDuyet();
            var reloadedItem = reloadedList.FirstOrDefault(x => x.MaCauHoiAISinh == itemToEdit.MaCauHoiAISinh);
            Assert.NotNull(reloadedItem);
            Assert.Contains("[Đã kiểm định]", reloadedItem.NoiDungCauHoi);
            Assert.Equal("has gone", reloadedItem.DanhSachLuaChon[0].NoiDungLuaChon);
            Assert.True(reloadedItem.DanhSachLuaChon[0].LaDapAnDung);
        }

        [Fact]
        public async Task Test_03_PheDuyetVaTaoBoCauHoiMoi()
        {
            var pendingList = AISinhDeBUS.Instance.LayDanhSachChoDuyet();
            if (pendingList == null || pendingList.Count == 0)
            {
                await Test_01_SinhCauHoiVaLuuHangDoi();
                pendingList = AISinhDeBUS.Instance.LayDanhSachChoDuyet();
            }

            int countBefore = pendingList.Count;

            // 1. Phê duyệt toàn bộ các câu hỏi đang chờ
            var (success, fail, dsMaMoi, err) = AISinhDeBUS.Instance.PheDuyetTatCa(pendingList);
            Assert.True(success > 0, $"Phê duyệt thất bại: {err}");
            Assert.Equal(0, fail);
            Assert.Equal(countBefore, dsMaMoi.Count);

            // 2. Tạo Bộ câu hỏi mới từ danh sách câu hỏi vừa phê duyệt
            string tenBoMoi = $"Bộ câu hỏi AI Test #{DateTime.Now:HHmmss}";
            string moTa = "Bộ câu hỏi tạo tự động từ quá trình kiểm thử tự động của hệ thống";
            bool okBo = BoCauHoiBUS.Instance.TaoBoCauHoiTuDanhSachMaCauHoi(
                tenBo: tenBoMoi,
                moTa: moTa,
                tenNguon: "AI Generator Test",
                maCauHois: dsMaMoi,
                nguoiTaoId: 0,
                out int maBoMoi,
                out string errBo
            );

            Assert.True(okBo, $"Tạo bộ câu hỏi thất bại: {errBo}");
            Assert.True(maBoMoi > 0, "Mã bộ câu hỏi mới phải > 0");

            // 3. Kiểm tra chi tiết bộ câu hỏi đã tạo trong DB
            var dsBo = BoCauHoiBUS.Instance.LayDanhSachBoCauHoi();
            var boCreated = dsBo.FirstOrDefault(b => b.MaBoCauHoi == maBoMoi);
            Assert.NotNull(boCreated);
            Assert.Equal(tenBoMoi, boCreated.TenBoCauHoi);
            Assert.Equal(dsMaMoi.Count, boCreated.SoLuongCauHoi);

            // 3b. Kiểm tra các câu hỏi thành viên trong bộ: nội dung và đáp án phải bảo toàn nguyên vẹn
            var dsCauHoiTrongBo = BoCauHoiDAL.Instance.LayDanhSachCauHoiTheoBo(maBoMoi);
            Assert.Equal(dsMaMoi.Count, dsCauHoiTrongBo.Count);
            foreach (var ch in dsCauHoiTrongBo)
            {
                Assert.False(string.IsNullOrWhiteSpace(ch.NoiDungCauHoi), $"Câu hỏi #{ch.MaCauHoi} không được có nội dung rỗng");
                if (ch.LoaiCauHoi == "TracNghiem" || ch.LoaiCauHoi == "DungSai" || ch.LoaiCauHoi == "TrueFalseNotGiven")
                {
                    Assert.True(ch.DanhSachLuaChon.Count >= 2, $"Câu hỏi trắc nghiệm #{ch.MaCauHoi} phải có ít nhất 2 lựa chọn");
                }
                else
                {
                    Assert.True(ch.DanhSachLuaChon.Count >= 1, $"Câu hỏi #{ch.MaCauHoi} ({ch.LoaiCauHoi}) phải có ít nhất 1 đáp án hợp lệ");
                }
            }

            // 4. Kiểm tra hàng đợi sau khi duyệt hết
            int countPendingAfter = AISinhDeBUS.Instance.DemSoCauChoDuyet();
            Assert.Equal(0, countPendingAfter);
        }

        [Fact]
        public async Task Test_04_XuatFileMauVaBocTach()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "EAI_Test_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string txtPath = Path.Combine(tempDir, "Mau_Test.txt");
                string jsonPath = Path.Combine(tempDir, "Mau_Test.json");

                // 1. Xuất file mẫu
                AISinhDeBUS.Instance.TaoFileMauTxt(txtPath);
                AISinhDeBUS.Instance.TaoFileMauJson(jsonPath);

                Assert.True(File.Exists(txtPath), "File TXT mẫu phải được tạo");
                Assert.True(File.Exists(jsonPath), "File JSON mẫu phải được tạo");

                // 2. Bóc tách file JSON
                var resJson = await AISinhDeBUS.Instance.BocTachTepTinAsync(jsonPath);
                Assert.True(resJson.DanhSachCauHoi.Count >= 3, "File JSON mẫu phải có ít nhất 3 câu");
                Assert.Empty(resJson.DanhSachLoi);

                // 3. Bóc tách file TXT
                var resTxt = await AISinhDeBUS.Instance.BocTachTepTinAsync(txtPath);
                Assert.True(resTxt.DanhSachCauHoi.Count >= 3, "File TXT mẫu phải có ít nhất 3 câu");

                // 4. Lưu câu hỏi bóc tách vào Hàng đợi kiểm duyệt
                bool okLuuHangDoi = AISinhDeBUS.Instance.LuuCauHoiVaoHangDoiTuTep(
                    resJson.DanhSachCauHoi,
                    "Mau_Test.json",
                    userId: 0,
                    out int soLuongLuu,
                    out string errLuu
                );
                Assert.True(okLuuHangDoi, $"Lưu hàng đợi từ tệp thất bại: {errLuu}");
                Assert.Equal(resJson.DanhSachCauHoi.Count, soLuongLuu);

                // 5. Kiểm tra hàng đợi có chứa các câu hỏi vừa nhập
                var listQueue = AISinhDeBUS.Instance.LayDanhSachChoDuyet();
                Assert.True(listQueue.Count >= soLuongLuu);

                // 6. Dọn dẹp hàng đợi bằng Từ chối tất cả
                var (rejectedCount, errReject) = AISinhDeBUS.Instance.TuChoiTatCa();
                Assert.True(rejectedCount >= soLuongLuu, $"Từ chối thất bại: {errReject}");
                Assert.Equal(0, AISinhDeBUS.Instance.DemSoCauChoDuyet());
            }
            finally
            {
                if (Directory.Exists(tempDir))
                {
                    try { Directory.Delete(tempDir, true); } catch { }
                }
            }
        }

        [Fact]
        public async Task Test_05_NhapTepBangAI_VaRangBuocDungLuong()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "EAI_TestAIFile_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                // 1. Kiểm tra ràng buộc dung lượng tệp vượt quá 10MB
                string largeFilePath = Path.Combine(tempDir, "LargeFile_11MB.txt");
                using (var fs = new FileStream(largeFilePath, FileMode.CreateNew))
                {
                    fs.SetLength(11 * 1024 * 1024); // 11MB
                }

                var resLarge = await AISinhDeBUS.Instance.BocTachTepTinAsync(largeFilePath);
                Assert.True(resLarge.DanhSachLoi.Count > 0, "File quá 10MB phải bị từ chối");
                Assert.Contains("vượt quá giới hạn cho phép (tối đa 10MB)", resLarge.DanhSachLoi[0]);
                Assert.Empty(resLarge.DanhSachCauHoi);

                // 2. Kiểm tra AI đọc nội dung bài học/chiến lược và tự động biên soạn câu hỏi hoàn chỉnh
                string lessonDocPath = Path.Combine(tempDir, "04_Chien_luoc_TOEIC_Part7_Doan_van_doi.txt");
                string lessonContent = @"
Chiến lược làm bài TOEIC Part 7: Đoạn văn kép (Double Passages).
Đặc trưng: Một bài thường gồm 2 văn bản có liên hệ mật thiết, ví dụ Đơn đặt hàng (Purchase Order) và Hóa đơn giao hàng (Invoice), hoặc Thư mời họp và Email phản hồi.
Quy tắc vàng: Để trả lời các câu hỏi liên kết chéo (Cross-referencing questions), thí sinh cần kết hợp thông tin từ cả 2 văn bản.
Ví dụ: 
Văn bản 1: Email thông báo từ Phòng Nhân sự: 'Tất cả nhân viên nộp hóa đơn thanh toán trước ngày 15 hàng tháng để được hoàn tiền vào kỳ lương ngày 25.'
Văn bản 2: Email phản hồi từ kế toán: 'Hóa đơn tiền taxi của cô Smith gửi ngày 18, do đó sẽ được xử lý vào kỳ lương tháng tiếp theo.'
Câu hỏi phân tích tình huống: Thí sinh cần xác định rõ nguyên nhân trễ hạn và thời gian nhận lại tiền hoàn trả.
";
                File.WriteAllText(lessonDocPath, lessonContent, System.Text.Encoding.UTF8);

                var resAI = await AISinhDeBUS.Instance.BocTachTepTinAsync(lessonDocPath);

                // Kiểm tra AI nhận diện và tạo câu hỏi
                Assert.NotNull(resAI);
                Assert.True(resAI.DanhSachCauHoi.Count > 0, "AI phải đọc nội dung bài học và biên soạn được câu hỏi");
                Assert.Equal("Doc", resAI.DanhSachCauHoi[0].KyNang);

                foreach (var q in resAI.DanhSachCauHoi)
                {
                    Assert.False(string.IsNullOrWhiteSpace(q.NoiDungCauHoi), "Nội dung câu hỏi không được rỗng");
                    bool hasOptions = q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count >= 2 && q.DapAnDungIndex.HasValue;
                    bool hasTextAnswer = !string.IsNullOrWhiteSpace(q.DapAnVanBan);
                    Assert.True(hasOptions || hasTextAnswer, $"Câu hỏi '{q.NoiDungCauHoi}' ({q.LoaiCauHoi}) phải có các phương án lựa chọn hoặc đáp án văn bản");
                }
            }
            finally
            {
                if (Directory.Exists(tempDir))
                {
                    try { Directory.Delete(tempDir, true); } catch { }
                }
            }
        }

        [Fact]
        public async Task Test_06_Tao3FileMauChuanVaKiemTraBocTachTrucTiep()
        {
            // Thư mục lưu trữ 3 tệp mẫu chuẩn cho người dùng kiểm thử
            string targetFolder = @"D:\TDC\HK5\CDPTUD\EAI\FileMau_NhapTep";
            Directory.CreateDirectory(targetFolder);

            string docxPath = Path.Combine(targetFolder, "Mau_DeThi_Chuan_TracNghiem.docx");
            string txtPath = Path.Combine(targetFolder, "Mau_DeThi_Chuan_TracNghiem.txt");
            string jsonPath = Path.Combine(targetFolder, "Mau_DeThi_Chuan_TracNghiem.json");

            // 1. Tạo 3 file mẫu chuẩn hóa
            AISinhDeBUS.Instance.TaoFileMauDocx(docxPath);
            AISinhDeBUS.Instance.TaoFileMauTxt(txtPath);
            AISinhDeBUS.Instance.TaoFileMauJson(jsonPath);

            Assert.True(File.Exists(docxPath), "File .docx mẫu phải tồn tại");
            Assert.True(File.Exists(txtPath), "File .txt mẫu phải tồn tại");
            Assert.True(File.Exists(jsonPath), "File .json mẫu phải tồn tại");

            // 2. Bóc tách trực tiếp tệp Word .docx (Đã có sẵn câu hỏi và đáp án -> Không cần gọi AI)
            var resWord = await AISinhDeBUS.Instance.BocTachTepTinAsync(docxPath);
            Assert.NotNull(resWord);
            Assert.Equal("Word", resWord.DinhDang);
            Assert.Equal(5, resWord.DanhSachCauHoi.Count);
            Assert.All(resWord.DanhSachCauHoi, q =>
            {
                Assert.False(string.IsNullOrWhiteSpace(q.NoiDungCauHoi));
                Assert.Equal(4, q.DanhSachLuaChon.Count);
                Assert.NotNull(q.DapAnDungIndex);
                Assert.InRange(q.DapAnDungIndex.Value, 0, 3);
            });

            // 3. Bóc tách trực tiếp tệp Text .txt (Đã có sẵn câu hỏi và đáp án -> Không cần gọi AI)
            var resTxt = await AISinhDeBUS.Instance.BocTachTepTinAsync(txtPath);
            Assert.NotNull(resTxt);
            Assert.Equal("Text", resTxt.DinhDang);
            Assert.Equal(5, resTxt.DanhSachCauHoi.Count);
            Assert.All(resTxt.DanhSachCauHoi, q =>
            {
                Assert.False(string.IsNullOrWhiteSpace(q.NoiDungCauHoi));
                Assert.Equal(4, q.DanhSachLuaChon.Count);
                Assert.NotNull(q.DapAnDungIndex);
                Assert.InRange(q.DapAnDungIndex.Value, 0, 3);
            });

            // 4. Bóc tách trực tiếp tệp JSON .json (Đã có sẵn câu hỏi và đáp án -> Không cần gọi AI)
            var resJson = await AISinhDeBUS.Instance.BocTachTepTinAsync(jsonPath);
            Assert.NotNull(resJson);
            Assert.Equal("JSON", resJson.DinhDang);
            Assert.Equal(5, resJson.DanhSachCauHoi.Count);
            Assert.All(resJson.DanhSachCauHoi, q =>
            {
                Assert.False(string.IsNullOrWhiteSpace(q.NoiDungCauHoi));
                Assert.Equal(4, q.DanhSachLuaChon.Count);
                Assert.NotNull(q.DapAnDungIndex);
                Assert.InRange(q.DapAnDungIndex.Value, 0, 3);
            });
        }
    }
}
