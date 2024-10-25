﻿using SV21T1020228.DomainModels;
using Dapper;
using System.Data;

namespace SV21T1020228.DataLayers.SQL_Server
{
    public class EmployeeDAL : BaseDAL, ICommonDAL<Employee>
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Employee data)
        {
            int id = 0;
            using (var connnection = OpenConnection())
            {
                var sql = @"insert into Employees(FullName, BirthDate, Address, Phone, Email, Photo, IsWorking)
                     values (@FullName, @BirthDate, @Address, @Phone, @Email, @Photo, @IsWorking)
                     select scope_identity();";
                var parameters = new
                {
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address,
                    Phone = data.Phone,
                    Email = data.Email,
                    Photo = data.Photo,
                    IsWorking = data.IsWorking,
                };
                connnection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connnection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*)
                            from Employees
                            where (FullName like @searchValue)";
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
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from Employees 
                            where EmployeeID = @EmployeeID";
                var parameters = new
                {
                    EmployeeID = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Employee? Get(int id)
        {
            Employee? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Employees 
                            where EmployeeID = @EmployeeID";
                var parameters = new
                {
                    EmployeeID = id
                };
                data = connection.QueryFirstOrDefault<Employee>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUse(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Orders where EmployeeID = @EmployeeID)
                                select 1
                            else
                                select 0";
                var parameters = new
                {
                    EmployeeID = id
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public List<Employee> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Employee> data = new List<Employee>();
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select *
                            from (
		                            select *,
			                            row_number() over(order by FullName) as RowNumber
		                            from Employees
		                            where (FullName like @searchValue)
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
                data = connection.Query<Employee>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Employee data)
        {
            bool result = false;
            using (var connnection = OpenConnection())
            {
                var sql = @"update Employees
                            set FullName = @FullName,
                                BirthDate = @BirthDate,
                                Address = @Address,
                                Phone = @Phone,
                                Email = @Email,
                                Photo = @Photo,
                                IsWorking = @IsWorking
                            where EmployeeID = @EmployeeID";
                var parameters = new
                {
                    EmployeeID = data.EmployeeID,
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address,
                    Phone = data.Phone,
                    Email = data.Email,
                    Photo = data.Photo,
                    IsWorking = data.IsWorking,
                };
                result = connnection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connnection.Close();
            }
            return result;
        }
    }
}