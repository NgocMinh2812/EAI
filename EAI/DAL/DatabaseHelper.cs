using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class DatabaseHelper
    {
        public static string ConnectionString
        {
            get => SqlDataProvider.Instance.ConnectionString;
            set => SqlDataProvider.Instance.ConnectionString = value;
        }

        public static SqlConnection GetConnection()
        {
            return SqlDataProvider.Instance.GetConnection();
        }

        /// <summary>
        /// Thực thi câu lệnh SELECT trả về DataTable
        /// </summary>
        public static DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            return SqlDataProvider.Instance.ExecuteQuery(query, parameters);
        }

        /// <summary>
        /// Thực thi câu lệnh INSERT, UPDATE, DELETE trả về số dòng ảnh hưởng
        /// </summary>
        public static int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            return SqlDataProvider.Instance.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Thực thi câu lệnh đếm hoặc trả về giá trị đơn (COUNT, MAX, SCOPE_IDENTITY)
        /// </summary>
        public static object? ExecuteScalar(string query, SqlParameter[]? parameters = null)
        {
            return SqlDataProvider.Instance.ExecuteScalar(query, parameters);
        }
    }
}
