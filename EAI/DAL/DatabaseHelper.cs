using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class DatabaseHelper
    {
        private static string _connectionString = "Server=.\\SQLEXPRESS;Database=TiengAnhAI;Trusted_Connection=True;TrustServerCertificate=True;";

        public static string ConnectionString
        {   
            get
            {
                if (!string.IsNullOrEmpty(SqlDataProvider.Instance.ConnectionString))
                    return SqlDataProvider.Instance.ConnectionString;
                return _connectionString;
            }
            set => _connectionString = value;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Thực thi câu lệnh SELECT trả về DataTable
        /// </summary>
        public static DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            var dataTable = new DataTable();
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        connection.Open();
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Thực thi câu lệnh INSERT, UPDATE, DELETE trả về số dòng ảnh hưởng
        /// </summary>
        public static int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Thực thi câu lệnh đếm hoặc trả về giá trị đơn (COUNT, MAX, SCOPE_IDENTITY)
        /// </summary>
        public static object? ExecuteScalar(string query, SqlParameter[]? parameters = null)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }
    }
}
