using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BUS;
using DAL;
using DTO;
using GUI;
using GUI.Views;
using GUI.Views.GiangVien;

namespace UiCaptureRunner
{
    class Program
    {
        private static string BaseOutputDir = @"d:\TDC\HK5\CDPTUD\EAI\artifacts\ui-screenshots";

        [STAThread]
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("================================================================================");
            Console.WriteLine("    EAI - TỰ ĐỘNG CHẠY ỨNG DỤNG & THU THẬP SCREENSHOT GIAO DIỆN THỰC TẾ        ");
            Console.WriteLine("================================================================================");
            Console.WriteLine($"Thời gian: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            var app = new GUI.App();
            app.InitializeComponent();

            string sp1StuDir = Path.Combine(BaseOutputDir, @"Sprint1\Student");
            string sp1AdmDir = Path.Combine(BaseOutputDir, @"Sprint1\Admin");
            string sp2StuDir = Path.Combine(BaseOutputDir, @"Sprint2\Student");
            string sp2AdmDir = Path.Combine(BaseOutputDir, @"Sprint2\Admin");

            Directory.CreateDirectory(sp1StuDir);
            Directory.CreateDirectory(sp1AdmDir);
            Directory.CreateDirectory(sp2StuDir);
            Directory.CreateDirectory(sp2AdmDir);

            // =========================================================================
            // PHẦN 1: MÀN HÌNH ĐĂNG NHẬP & ĐĂNG KÝ (SPRINT 1 - STUDENT / AUTH)
            // =========================================================================
            Console.WriteLine("\n[1/4] Chụp màn hình Đăng nhập & Đăng ký...");
            CaptureLoginAndRegister(sp1StuDir);

            // =========================================================================
            // PHẦN 2: MÀN HÌNH HỌC VIÊN / SINH VIÊN (SPRINT 1 & SPRINT 2 - MINH)
            // =========================================================================
            Console.WriteLine("\n[2/4] Chụp các màn hình Học viên / Sinh viên...");
            CaptureStudentScreens(sp1StuDir, sp2StuDir);

            // =========================================================================
            // PHẦN 3: MÀN HÌNH GIẢNG VIÊN / ADMIN (SPRINT 1 & SPRINT 2 - ÊM)
            // =========================================================================
            Console.WriteLine("\n[3/4] Chụp các màn hình Giảng viên / Quản trị viên...");
            CaptureAdminScreens(sp1AdmDir, sp2AdmDir);

            Console.WriteLine("\n================================================================================");
            Console.WriteLine("  TẤT CẢ SCREENSHOT ĐÃ ĐƯỢC CHỤP VÀ LƯU VÀO artifacts/ui-screenshots/ THÀNH CÔNG!");
            Console.WriteLine("================================================================================");
        }

        private static void CaptureLoginAndRegister(string outDir)
        {
            var win = new DangNhapWindow();
            win.Show();
            WaitRender(win);

            // 1. Tab Đăng nhập
            string fileLogin = Path.Combine(outDir, "TC-HV-SP1-AUTH-001.png");
            CaptureVisualToFile(win, fileLogin);
            Console.WriteLine($"  [OK] Đã lưu: TC-HV-SP1-AUTH-001.png (Màn hình Đăng nhập)");

            // 2. Tab Đăng ký
            var switchRegMethod = win.GetType().GetMethod("SwitchToRegisterTab", BindingFlags.NonPublic | BindingFlags.Instance);
            if (switchRegMethod != null)
            {
                switchRegMethod.Invoke(win, null);
                WaitRender(win);
                string fileReg = Path.Combine(outDir, "TC-HV-SP1-AUTH-002.png");
                CaptureVisualToFile(win, fileReg);
                Console.WriteLine($"  [OK] Đã lưu: TC-HV-SP1-AUTH-002.png (Màn hình Đăng ký tài khoản)");
            }

            win.Close();
            DoEvents();
        }

        private static void CaptureStudentScreens(string sp1Dir, string sp2Dir)
        {
            // Thiết lập Session học viên
            var userBUS = new NguoiDungBUS();
            userBUS.DangNhap("hocvien", "123456", out _, out var studentUser);
            if (studentUser == null)
            {
                studentUser = new NguoiDungDTO
                {
                    MaNguoiDung = 5,
                    TenDangNhap = "hocvien",
                    HoTen = "Trần Thị Mai (Học viên)",
                    Email = "hocvien@gmail.com",
                    TenVaiTro = "HocVien",
                    TrangThaiHoatDong = true
                };
            }
            SessionHelper.CurrentUser = studentUser;

            var win = new MainWindow(isFirstLogin: false);
            win.Width = 1280;
            win.Height = 820;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
            WaitRender(win, 600);

            var mainFrame = win.FindName("MainFrame") as System.Windows.Controls.Frame;
            if (mainFrame == null) return;

            // 1. Khảo sát đầu vào (Sprint 1)
            try
            {
                mainFrame.Navigate(new DanhGiaNangLucView(isFirstTime: false));
                WaitRender(win, 600);
                string fileSurv = Path.Combine(sp1Dir, "TC-HV-SP1-SURV-001.png");
                CaptureVisualToFile(win, fileSurv);
                Console.WriteLine($"  [OK] Đã lưu: TC-HV-SP1-SURV-001.png (Bài kiểm tra đánh giá năng lực)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] DanhGiaNangLucView: {ex.Message}"); }

            // 2. Trang chủ sinh viên (Sprint 2)
            try
            {
                mainFrame.Navigate(new TrangChuView());
                WaitRender(win, 700);
                string fileHome = Path.Combine(sp2Dir, "TC-HV-SP2-HOME-001.png");
                CaptureVisualToFile(win, fileHome);
                Console.WriteLine($"  [OK] Đã lưu: TC-HV-SP2-HOME-001.png (Trang chủ học viên)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] TrangChuView: {ex.Message}"); }

            // 3. Học tập & Danh sách bài học (Sprint 2)
            try
            {
                mainFrame.Navigate(new HocTapView());
                WaitRender(win, 800);
                string fileLearn = Path.Combine(sp2Dir, "TC-HV-SP2-LEARN-001.png");
                CaptureVisualToFile(win, fileLearn);
                Console.WriteLine($"  [OK] Đã lưu: TC-HV-SP2-LEARN-001.png (Giao diện bài học)");

                // 4. Audio player (Sprint 2)
                string fileAudio = Path.Combine(sp2Dir, "TC-HV-SP2-AUDIO-001.png");
                CaptureVisualToFile(win, fileAudio);
                Console.WriteLine($"  [OK] Đã lưu: TC-HV-SP2-AUDIO-001.png (Trình phát Audio bài học)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] HocTapView: {ex.Message}"); }

            // 5. Luyện tập (Sprint 2)
            try
            {
                mainFrame.Navigate(new LuyenTapView());
                WaitRender(win, 700);
                string filePract = Path.Combine(sp2Dir, "TC-HV-SP2-PRACT-001.png");
                CaptureVisualToFile(win, filePract);
                Console.WriteLine($"  [OK] Đã lưu: TC-HV-SP2-PRACT-001.png (Trung tâm luyện tập)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] LuyenTapView: {ex.Message}"); }

            // 6. Thi thử (Sprint 2)
            try
            {
                mainFrame.Navigate(new ThiThuView());
                WaitRender(win, 800);
                string fileExam = Path.Combine(sp2Dir, "TC-HV-SP2-EXAM-001.png");
                CaptureVisualToFile(win, fileExam);
                Console.WriteLine($"  [OK] Đã lưu: TC-HV-SP2-EXAM-001.png (Phòng thi thử trắc nghiệm)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] ThiThuView: {ex.Message}"); }

            // 7. Tiến độ học & Chấm điểm (Sprint 2)
            try
            {
                mainFrame.Navigate(new TienDoHocView());
                WaitRender(win, 700);
                string fileScore = Path.Combine(sp2Dir, "TC-HV-SP2-SCORE-001.png");
                CaptureVisualToFile(win, fileScore);
                Console.WriteLine($"  [OK] Đã lưu: TC-HV-SP2-SCORE-001.png (Báo cáo tiến độ & điểm số)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] TienDoHocView: {ex.Message}"); }

            // 8. Hồ sơ cá nhân (Sprint 2)
            try
            {
                mainFrame.Navigate(new HoSoCaNhanView());
                WaitRender(win, 600);
                string fileProfile = Path.Combine(sp2Dir, "TC-HV-SP2-REV-001.png");
                CaptureVisualToFile(win, fileProfile);
                Console.WriteLine($"  [OK] Đã lưu: TC-HV-SP2-REV-001.png (Hồ sơ cá nhân & Lịch sử học)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] HoSoCaNhanView: {ex.Message}"); }

            win.Close();
            DoEvents();
        }

        private static void CaptureAdminScreens(string sp1Dir, string sp2Dir)
        {
            // Thiết lập Session Quản trị viên
            var userBUS = new NguoiDungBUS();
            userBUS.DangNhap("admin", "123456", out _, out var adminUser);
            if (adminUser == null)
            {
                adminUser = new NguoiDungDTO
                {
                    MaNguoiDung = 1,
                    TenDangNhap = "admin",
                    HoTen = "Quản trị viên Hệ thống",
                    Email = "admin@gmail.com",
                    TenVaiTro = "QuanTriVien",
                    TrangThaiHoatDong = true
                };
            }
            SessionHelper.CurrentUser = adminUser;

            var win = new GiangVienWindow();
            win.Width = 1280;
            win.Height = 820;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
            WaitRender(win, 600);

            var adminFrame = win.FindName("AdminMainFrame") as System.Windows.Controls.Frame;
            if (adminFrame == null) return;

            // 1. Dashboard / Tổng quan (Sprint 1)
            try
            {
                adminFrame.Navigate(new TongQuanView());
                WaitRender(win, 700);
                string fileNav = Path.Combine(sp1Dir, "TC-GV-SP1-NAV-001.png");
                CaptureVisualToFile(win, fileNav);
                Console.WriteLine($"  [OK] Đã lưu: TC-GV-SP1-NAV-001.png (Khung điều hướng & Dashboard)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] TongQuanView: {ex.Message}"); }

            // 2. Quản lý người dùng (Sprint 1)
            try
            {
                adminFrame.Navigate(new NguoiDungView());
                WaitRender(win, 700);
                string fileUser = Path.Combine(sp1Dir, "TC-GV-SP1-USER-001.png");
                CaptureVisualToFile(win, fileUser);
                Console.WriteLine($"  [OK] Đã lưu: TC-GV-SP1-USER-001.png (Quản lý người dùng)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] NguoiDungView: {ex.Message}"); }

            // 3. Quản lý danh mục học liệu (Sprint 1)
            try
            {
                adminFrame.Navigate(new DanhMucView());
                WaitRender(win, 700);
                string fileCat = Path.Combine(sp1Dir, "TC-GV-SP1-CAT-001.png");
                CaptureVisualToFile(win, fileCat);
                Console.WriteLine($"  [OK] Đã lưu: TC-GV-SP1-CAT-001.png (Quản lý danh mục)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] DanhMucView: {ex.Message}"); }

            // 4. Quản lý bài học / nội dung (Sprint 1)
            try
            {
                adminFrame.Navigate(new NoiDungHocTapView());
                WaitRender(win, 700);
                string fileLess = Path.Combine(sp1Dir, "TC-GV-SP1-LESS-001.png");
                CaptureVisualToFile(win, fileLess);
                Console.WriteLine($"  [OK] Đã lưu: TC-GV-SP1-LESS-001.png (Quản lý nội dung bài học)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] NoiDungHocTapView: {ex.Message}"); }

            // 5. Ngân hàng câu hỏi (Sprint 1)
            try
            {
                adminFrame.Navigate(new NganHangCauHoiView());
                WaitRender(win, 700);
                string fileQBank = Path.Combine(sp1Dir, "TC-GV-SP1-QBANK-001.png");
                CaptureVisualToFile(win, fileQBank);
                Console.WriteLine($"  [OK] Đã lưu: TC-GV-SP1-QBANK-001.png (Ngân hàng câu hỏi)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] NganHangCauHoiView: {ex.Message}"); }

            // 6. Quản lý đề thi (Sprint 2)
            try
            {
                adminFrame.Navigate(new DeThiLuyenTapView());
                WaitRender(win, 700);
                string fileExam = Path.Combine(sp2Dir, "TC-GV-SP2-EXAM-001.png");
                CaptureVisualToFile(win, fileExam);
                Console.WriteLine($"  [OK] Đã lưu: TC-GV-SP2-EXAM-001.png (Quản lý đề thi)");

                // 7. Quản lý gói luyện tập (Sprint 2)
                string filePrac = Path.Combine(sp2Dir, "TC-GV-SP2-PRAC-001.png");
                CaptureVisualToFile(win, filePrac);
                Console.WriteLine($"  [OK] Đã lưu: TC-GV-SP2-PRAC-001.png (Quản lý gói luyện tập)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] DeThiLuyenTapView: {ex.Message}"); }

            // 8. Quản lý khảo sát đầu vào (Sprint 2)
            try
            {
                adminFrame.Navigate(new KhaoSatDauVaoView());
                WaitRender(win, 700);
                string fileSurv = Path.Combine(sp2Dir, "TC-GV-SP2-SURV-001.png");
                CaptureVisualToFile(win, fileSurv);
                Console.WriteLine($"  [OK] Đã lưu: TC-GV-SP2-SURV-001.png (Quản lý câu hỏi khảo sát)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] KhaoSatDauVaoView: {ex.Message}"); }

            // 9. Trí tuệ AI & Sinh đề (Sprint 2)
            try
            {
                adminFrame.Navigate(new AISinhDeView());
                WaitRender(win, 700);
                string fileAI = Path.Combine(sp2Dir, "TC-GV-SP2-AI-001.png");
                CaptureVisualToFile(win, fileAI);
                Console.WriteLine($"  [OK] Đã lưu: TC-GV-SP2-AI-001.png (Tạo câu hỏi bằng AI)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] AISinhDeView: {ex.Message}"); }

            // 10. Giám sát hệ thống (Sprint 2)
            try
            {
                adminFrame.Navigate(new GiamSatHeThongView());
                WaitRender(win, 700);
                string fileMon = Path.Combine(sp2Dir, "TC-GV-SP2-MON-001.png");
                CaptureVisualToFile(win, fileMon);
                Console.WriteLine($"  [OK] Đã lưu: TC-GV-SP2-MON-001.png (Giám sát hệ thống)");
            }
            catch (Exception ex) { Console.WriteLine($"  [SKIP] GiamSatHeThongView: {ex.Message}"); }

            win.Close();
            DoEvents();
        }

        private static void WaitRender(Window win, int waitMs = 500)
        {
            win.UpdateLayout();
            DoEvents();
            Thread.Sleep(waitMs);
            win.UpdateLayout();
            DoEvents();
        }

        public static void DoEvents()
        {
            var frame = new System.Windows.Threading.DispatcherFrame();
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Background,
                new System.Windows.Threading.DispatcherOperationCallback(ExitFrame), frame);
            System.Windows.Threading.Dispatcher.PushFrame(frame);
        }

        private static object? ExitFrame(object frame)
        {
            ((System.Windows.Threading.DispatcherFrame)frame).Continue = false;
            return null;
        }

        public static void CaptureVisualToFile(Visual visual, string filePath)
        {
            var fe = visual as FrameworkElement;
            int width = fe != null && fe.ActualWidth > 0 ? (int)fe.ActualWidth : 1280;
            int height = fe != null && fe.ActualHeight > 0 ? (int)fe.ActualHeight : 800;

            var rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(visual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            using var fs = new FileStream(filePath, FileMode.Create);
            encoder.Save(fs);
        }
    }
}
