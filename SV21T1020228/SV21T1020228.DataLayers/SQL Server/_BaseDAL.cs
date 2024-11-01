using Microsoft.Data.SqlClient;

namespace SV21T1020228.DataLayers.SQL_Server
{
    public abstract class BaseDAL
    {
        protected string _connectionString = "";

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Tạo & mở kết nối đến CSDL SQL Server
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
