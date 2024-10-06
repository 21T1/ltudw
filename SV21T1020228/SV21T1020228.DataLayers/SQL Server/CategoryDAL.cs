using Dapper;
using SV21T1020228.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020228.DataLayers.SQL_Server
{
    public class CategoryDAL : BaseDAL, ICommonDAL<Category>
    {
        public CategoryDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Category data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Category? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUse(int id)
        {
            throw new NotImplementedException();
        }

        public List<Category> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Category> data = new List<Category>();
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select *
                            from (
		                            select *,
			                            row_number() over(order by CategoryName) as RowNumber
		                            from Category
		                            where (CategoryName like @searchValue)
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
                data = connection.Query<Category>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Category data)
        {
            throw new NotImplementedException();
        }
    }
}
