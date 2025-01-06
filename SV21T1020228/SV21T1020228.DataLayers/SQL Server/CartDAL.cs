using Dapper;
using SV21T1020228.DomainModels;
using System.Data;

namespace SV21T1020228.DataLayers.SQL_Server
{
    public class CartDAL : BaseDAL, ICartDAL
    {
        public CartDAL(string connectionString) : base(connectionString)
        {
        }

        public bool AddItem(CartItem data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists (select * from CartItems where CartID = @CartID and ProductID = @ProductID)
								begin
									update CartItems
									set Quantity = Quantity + @Quantity
									where CartID = @CartID and ProductID = @ProductID
								end
                            else
                                begin
                                    insert into CartItems
									values (@CartID, @ProductID, @Quantity)
                                end";
                var parameter = new 
                {
                    CartID = data.CartID,
                    ProductID = data.ProductID,
                    Quantity = data.Quantity
                };

                try
                {
                    result = connection.Execute(sql: sql, param: parameter, commandType: CommandType.Text) > 0;
                }
                catch (Exception ex)
                {
                }
                connection.Close();
            }

            return result;
        }

        public int CountItem(int cartID)
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*)
                            from CartItems
                            where CartID = @CartID";
                var parameters = new
                {
                    cartID
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
            return count;
        }

        public bool Delete(int cartID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from CartItems where CartID = @CartID";
                var parameters = new
                {
                    cartID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool RemoveItem(int cartID, int productID)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"delete from CartItems 
                            where CartID = @CartID and ProductID = @ProductID";
                var parameters = new
                {
                    cartID,
                    productID
                };

                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }

            return result;
        }

        /*        public bool InUse(int cartID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Customers where IsLocked = 0 and CustomerID = @CustomerID)
                                select 1
                            else
                                select 0";
                var parameters = new
                {
                    CustomerID = cartID
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }
*/

        public IList<CartItem>? Get(int cartID)
        {
            List<CartItem> data = new List<CartItem>();

            using (var connection = OpenConnection())
            {
                var sql = @"select ct.ProductID, ProductName, Photo, Quantity, Price from CartItems as ct
                            inner join Products as p on ct.ProductID = p.ProductID
                            where CartID = @CartID";
                var parameters = new
                {
                    cartID
                };

                data = connection.Query<CartItem>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }

            return data;
        }

        public bool UpdateItem(CartItem data)
        {
            bool result = false;
            using (var connnection = OpenConnection())
            {
                var sql = @"update CartItems
                            set Quantity = @Quantity
                            where CartID = @CartID and ProductID = @ProductID";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    Quantity = data.Quantity,
                    CartID = data.CartID
                };
                result = connnection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connnection.Close();
            }
            return result;
        }
    }
}
