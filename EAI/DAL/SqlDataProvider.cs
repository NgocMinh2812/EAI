using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class SqlDataProvider
    {
        private static SqlDataProvider? _instance;
        public static SqlDataProvider Instance => _instance ??= new SqlDataProvider();

        // Chuỗi kết nối mặc định - có thể thay đổi linh hoạt hoặc nạp từ biến môi trường
        private string _connectionString = "Server=.\\SQLEXPRESS;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;";

        public string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }

        public SqlDataProvider()
        {
            var envConn = Environment.GetEnvironmentVariable("EAI_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(envConn))
            {
                _connectionString = envConn;
            }
            TestConnection();
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                EnsureSchemaCompatibility();
                return true;
            }
            catch
            {
                // Thử fallback sang các instance SQL thông dụng
                string[] fallbackStrings = new[]
                {
                    "Server=.\\SQLEXPRESS;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;",
                    "Server=.;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;",
                    "Server=localhost\\SQLEXPRESS;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;",
                    "Server=localhost;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;",
                    "Server=(localdb)\\mssqllocaldb;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;",
                    "Server=127.0.0.1;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;"
                };

                foreach (var fb in fallbackStrings)
                {
                    try
                    {
                        using var fbConn = new SqlConnection(fb);
                        fbConn.Open();
                        _connectionString = fb;
                        return true;
                    }
                    catch
                    {
                        // thử tiếp
                    }
                }
                return false;
            }
        }

        public void EnsureSchemaCompatibility()
        {
            try
            {
                string script = @"
                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'NguoiDung')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'LaLanDauDangNhap' AND Object_ID = Object_ID('NguoiDung'))
                        BEGIN
                            ALTER TABLE NguoiDung ADD LaLanDauDangNhap BIT NOT NULL DEFAULT 1;
                        END
                    END;

                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'HoSoHocTap')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'DiemDanhGiaDauVao' AND Object_ID = Object_ID('HoSoHocTap'))
                        BEGIN
                            ALTER TABLE HoSoHocTap ADD DiemDanhGiaDauVao DECIMAL(5,2) NULL;
                        END

                        IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = 'TrinhDoDauVao' AND Object_ID = Object_ID('HoSoHocTap'))
                        BEGIN
                            ALTER TABLE HoSoHocTap ADD TrinhDoDauVao NVARCHAR(10) NULL;
                        END
                    END;

                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'BaiKhaoSatDauVao')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM BaiKhaoSatDauVao WHERE MaBaiKhaoSat = 1)
                        BEGIN
                            SET IDENTITY_INSERT BaiKhaoSatDauVao ON;
                            INSERT INTO BaiKhaoSatDauVao (MaBaiKhaoSat, TenBaiKhaoSat, MoTa, TongSoCauHoi, DangHoatDong, NgayTao)
                            VALUES (1, N'Khảo sát đánh giá năng lực đầu vào CEFR', N'Bài khảo sát phân loại trình độ từ A1 đến C1', 15, 1, SYSDATETIME());
                            SET IDENTITY_INSERT BaiKhaoSatDauVao OFF;
                        END
                    END;

                    IF EXISTS (SELECT 1 FROM sys.tables WHERE Name = 'LuotLamBai')
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.views WHERE Name = 'KetQuaThi')
                        BEGIN
                            EXEC('CREATE VIEW KetQuaThi AS SELECT MaLuotLamBai AS MaKetQuaThi, MaNguoiDung, MaBaiTapDeThi, HinhThucLam, ThoiDiemBatDau, ThoiDiemNopBai, ThoiGianLamGiay, TongSoCau, SoCauDung, SoCauSai, DiemSo, TrangThai FROM LuotLamBai');
                        END
                    END;";
                ExecuteNonQuery(script);
            }
            catch
            {
                // Bỏ qua nếu chưa có bảng
            }
        }

        public DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            DataTable data = new DataTable();
            using var conn = GetConnection();
            using var cmd = new SqlCommand(query, conn);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(data);
            return data;
        }

        public int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(query, conn);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd.ExecuteNonQuery();
        }

        public object? ExecuteScalar(string query, SqlParameter[]? parameters = null)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(query, conn);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd.ExecuteScalar();
        }
    }
}
