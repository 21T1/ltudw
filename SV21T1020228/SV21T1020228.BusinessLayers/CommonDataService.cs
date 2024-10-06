using SV21T1020228.DataLayers;
using SV21T1020228.DataLayers.SQL_Server;
using SV21T1020228.DomainModels;

namespace SV21T1020228.BusinessLayers
{
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Customer> customerDB;

        /// <summary>
        /// Ctor
        /// </summary>
        static CommonDataService()
        {
            string connectionString =
                @"uid=sa;pwd=123;
                            database=LiteCommerce;
                            server=Duckyy;
                            TrustServerCertificate=true";

            customerDB = new CustomerDAL(connectionString);
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách khách hàng dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue);
        }
    }
}
