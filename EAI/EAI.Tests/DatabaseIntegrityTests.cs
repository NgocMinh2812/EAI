using System;
using System.Data;
using DAL;
using Xunit;

namespace EAI.Tests
{
    public class DatabaseIntegrityTests
    {
        [Fact]
        public void SqlDataProvider_Connection_MustBeOpenAndFunctional()
        {
            bool ok = SqlDataProvider.Instance.TestConnection();
            Assert.True(ok, "SqlDataProvider must successfully connect to SQL Server database TiengAnhAI.");
        }

        [Fact]
        public void DeleteUser_NonExistentId_MustNotThrowMissingTableException()
        {
            // Bug DB-01 fix verification: Previously, calling Delete(-9999) threw
            // SqlException: Invalid object name 'ThongBao'.
            // Now, with IF OBJECT_ID('dbo.ThongBao', 'U') IS NOT NULL, it must execute cleanly without exception.
            var ex = Record.Exception(() =>
            {
                bool result = NguoiDungDAL.Instance.Delete(-9999);
                Assert.False(result); // ID -9999 doesn't exist, returns false, but NO SQL EXCEPTION
            });

            Assert.Null(ex);
        }

        [Fact]
        public void SeededListeningQuestions_MustExistInDatabaseWithValidForeignKeys()
        {
            // Bug DB-02 fix verification: IDs 9001-9005 must exist in CauHoi and choices in LuaChonDapAn
            string queryQ = "SELECT COUNT(*) FROM CauHoi WHERE MaCauHoi BETWEEN 9001 AND 9005";
            DataTable dtQ = DatabaseHelper.ExecuteQuery(queryQ);
            int countQ = Convert.ToInt32(dtQ.Rows[0][0]);
            Assert.Equal(5, countQ);

            string queryC = "SELECT COUNT(*) FROM LuaChonDapAn WHERE MaCauHoi BETWEEN 9001 AND 9005";
            DataTable dtC = DatabaseHelper.ExecuteQuery(queryC);
            int countC = Convert.ToInt32(dtC.Rows[0][0]);
            Assert.Equal(20, countC); // 5 questions * 4 choices
        }
    }
}

