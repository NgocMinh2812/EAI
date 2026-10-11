using System;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Bắt lỗi ngoại lệ chưa xử lý trên Dispatcher UI Thread
            this.DispatcherUnhandledException += (s, args) =>
            {
                try
                {
                    Views.Common.ThongBaoDialog.Loi(
                        null,
                        $"Hệ thống phát hiện lỗi không mong muốn:\n\n{args.Exception.Message}",
                        "Thông báo ngoại lệ hệ thống");
                }
                catch
                {
                    MessageBox.Show($"Đã xảy ra lỗi:\n{args.Exception.Message}", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                args.Handled = true; // Ngăn chặn ứng dụng crash đột ngột
            };

            // Bắt lỗi ngoại lệ chưa xử lý trên AppDomain (Background Threads)
            AppDomain.CurrentDomain.UnhandledException += (s, args) =>
            {
                if (args.ExceptionObject is Exception ex)
                {
                    try
                    {
                        Views.Common.ThongBaoDialog.Loi(
                            null,
                            $"Lỗi luồng xử lý nền:\n\n{ex.Message}",
                            "Lỗi hệ thống");
                    }
                    catch
                    {
                        MessageBox.Show($"Lỗi luồng nền:\n{ex.Message}", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            };
        }
    }
}
