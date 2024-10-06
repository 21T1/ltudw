using SV21T1020228.DomainModels;
using Dapper;
using Azure;
using System.Data;

namespace SV21T1020228.DataLayers.SQL_Server
{
    public class CustomerDAL : BaseDAL, ICommonDAL<Customer>
    {
        public CustomerDAL(string connectionString) : base(connectionString)
        {


        }

        public int Add(Customer data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            searchValue = $"%{searchValue.Trim()}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*)
                            from Customers
                            where (CustomerName like @searchValue) or (ContactName like @searchValue)";
                var parameters = new
                {
                    searchValue
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
            return count;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Customer? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUse(int id)
        {
            throw new NotImplementedException();
        }

        public List<Customer> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Customer> data = new List<Customer>();
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select *
                            from (
		                            select *,
			                            row_number() over(order by CustomerName) as RowNumber
		                            from Customers
		                            where (CustomerName like @searchValue) or (ContactName like @searchValue)
	                            ) as t
                            where (@pageSize = 0)
	                            or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            order by RowNumber";
                var parameters = new
                {
                    page,
                    pageSize,
                    searchValue
                };
                data = connection.Query<Customer>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Customer data)
        {
            throw new NotImplementedException();
        }
    }
}
