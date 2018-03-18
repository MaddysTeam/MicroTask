using System.Data.SqlClient;

namespace Infrastructure.Orms.dapper
{
    public class DapperConnectionBuilder
    {

        public DapperConnectionBuilder()
        {
        }

        public SqlConnection BuildSqlConnection()
        {
            return new SqlConnection();
        }
    }

}