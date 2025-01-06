using Dapper;
using SV21T1020228.DomainModels;
using System.Data;

namespace SV21T1020228.DataLayers.SQL_Server
{
    public class CustomerAccountDAL : BaseDAL, IUserAccountDAL
    {
        public CustomerAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount Authorize(string username, string password)
        {
            UserAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select	CustomerID as UserID,
		                            Email as UserName,
		                            CustomerName as DisplayName,
                                    N'customer' as RoleNames
                            from Customers
                            where Email = @Email and Password = @Password and IsLocked = 0";
                var parameters = new
                {
                    Email = username,
                    Password = password
                };

                data = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return data;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"update Customers
                            set Password = @NewPassword
                            where Email = @Email and Password = @OldPassword";
                var parameters = new
                {
                    Email = username,
                    OldPassword = oldPassword,
                    NewPassword = newPassword
                };

                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public int Create(string fullName, string username, string password)
        {
            int id = 0;

            using (var connection = OpenConnection())
            {
                var sql = @"if exists (select * from Customers where Email = @Email)
                                select -1;
                            else
                                begin
                                    insert into Customers(CustomerName, Email, Password, IsLocked)
                                    values (@CustomerName, @Email, @Password, 0)
                                    select scope_identity()
                                end";
                var parameters = new
                {
                    CustomerName = fullName,
                    Email = username,
                    Password = password
                };

                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return id;
        }
    }
}
