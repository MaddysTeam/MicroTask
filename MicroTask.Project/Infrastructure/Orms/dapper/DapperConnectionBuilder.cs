using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;
using Oracle.ManagedDataAccess.Client;

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

        //public MySqlConnection BuildMySqlConnection()
        //{
        //    return new MySqlConnection();
        //}

        public OracleConnection BuildOracleConneciton()
        {
            return new OracleConnection();
        }

    }

}