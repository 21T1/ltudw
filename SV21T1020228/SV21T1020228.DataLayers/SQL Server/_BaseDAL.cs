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
            //SqlConnection connection = new SqlConnection(@"uid=sa;pwd=123;database=LiteCommerce;server=Duckyy;TrustServerCertificate=true;");
            SqlConnection connection = new SqlConnection(@"Server=tcp:Duckyy;Database=LiteCommerceDB;User Id=sa;Password=123;Encrypt=true;TrustServerCertificate=true;");
            connection.Open();
            return connection;
        }
    }
}
