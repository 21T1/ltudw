using Dapper;
using SV21T1020228.DomainModels;
using System;
using System.Buffers;
using System.Data;
using System.Diagnostics;

namespace SV21T1020228.DataLayers.SQL_Server
{
    public class ProductDAL : BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }
        // TODO: Bổ sung các chức năng xử lý + thư viện ảnh
        public int Add(Product data)
        {
            int id = 0;
            using (var connnection = OpenConnection())
            {
                var sql = @"if exists (select * from Products where ProductName = @ProductName)
                                select -1;
                            else
                                begin
                                    insert into Products(ProductName, ProductDescription, SupplierID, CategoryID, Unit, Price, Photo, IsSelling)
                                    values (@ProductName, @ProductDescription, @SupplierID, @CategoryID, @Unit, @Price,	@Photo, @IsSelling)
                                    select scope_identity()
                                end";
                var parameters = new
                {
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling
                };
                id = connnection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connnection.Close();
            }
            
            return id;
        }

        public long AddAttribute(ProductAttribute data)
        {
            int id = 0;
            using (var connnection = OpenConnection())
            {
                var sql = @"if exists (select * from ProductAttributes where AttributeName = @AttributeName)
                                select -1;
                            else
                                begin
                                    insert into ProductAttributes(ProductID, AttributeName, AttributeValue, DisplayOrder)
                                    values (@ProductID, @AttributeName, @AttributeValue, @DisplayOrder)
                                    select scope_identity()
                                end";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName,
                    AttributeValue = data.AttributeValue,
                    DisplayOrder = data.DisplayOrder,
                };
                id = connnection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connnection.Close();
            }
            return id;
        }

        public long AddPhoto(ProductPhoto data)
        {
            int id = 0;
            using (var connnection = OpenConnection())
            {
                var sql = @"if exists (select * from ProductPhotos where Photo = @Photo)
                                select -1;
                            else
                                begin
                                    insert into ProductPhotos(ProductID, Photo, Description, DisplayOrder, IsHidden)
                                    values (@ProductID, @Photo, @Description, @DisplayOrder, @IsHidden)
                                    select scope_identity()
                                end";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    Photo = data.Photo,
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden,
                };
                id = connnection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connnection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int count = 0;
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*)
                            from Products
                            where (@SearchValue = N'' or ProductName like @SearchValue)
                                and (@CategoryID = 0 or CategoryID = @CategoryID)
                                and (@SupplierID = 0 or SupplierId = @SupplierID)
                                and (Price >= @MinPrice)
                                and (@MaxPrice <= 0 or Price <= @MaxPrice)";
                var parameters = new
                {
                    searchValue,
                    categoryID,
                    supplierID,
                    minPrice,
                    maxPrice
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
            return count;
        }

        public bool Delete(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from Products 
                            where ProductID = @ProductID";
                var parameters = new
                {
                    productID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeleteAttribute(long attributeID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductAttributes 
                            where AttributeID = @AttributeID";
                var parameters = new
                {
                    attributeID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeletePhoto(long photoID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductPhotos 
                            where PhotoID = @PhotoID";
                var parameters = new
                {
                    photoID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Product? Get(int productID)
        {
            Product? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Products 
                            where ProductID = @ProductID";
                var parameters = new
                {
                    productID
                };
                data = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductAttribute? GetAttribute(long attributeID)
        {
            ProductAttribute? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductAttributes 
                            where AttributeID = @AttributeID";
                var parameters = new
                {
                    attributeID
                };
                data = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductPhoto? GetPhoto(long photoID)
        {
            ProductPhoto? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductPhotos 
                            where PhotoID = @PhotoID";
                var parameters = new
                {
                    photoID
                };
                data = connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Orders where ProductID = @ProductID)
                                select 1
                            else
                                select 0";
                var parameters = new
                {
                    productID
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public List<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> data = new List<Product>();
            searchValue = $" %{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select *
                            from (
                                select *,
                                    row_number() over(order by ProductName) as RowNumber
                                from Products
                                where (@SearchValue = N'' or ProductName like @SearchValue)
                                    and (@CategoryID = 0 or CategoryID = @CategoryID)
                                    and (@SupplierID = 0 or SupplierId = @SupplierID)
                                    and (Price >= @MinPrice)
                                    and (@MaxPrice <= 0 or Price <= @MaxPrice)
                                 ) as t
                            where (@PageSize = 0)
                                or (RowNumber between (@Page - 1)*@PageSize + 1 and @Page * @PageSize)
                            order by ProductName";
                var parameters = new
                {
                    searchValue,
                    categoryID,
                    supplierID,
                    minPrice,
                    maxPrice,
                    pageSize,
                    page
                };
                data = connection.Query<Product>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
            }

            return data;
        }

        public IList<ProductAttribute> ListAttributes(int productID)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();
            using (var connection = OpenConnection())
            {
                var sql = "select * from ProductAttributes where ProductID = @ProductID";
                var parameters = new
                {
                    productID
                };
                data = connection.Query<ProductAttribute>(sql: sql, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            return data;
        }

        public IList<ProductPhoto> ListPhotos(int productID)
        {
            List<ProductPhoto> data = new List<ProductPhoto>();
            using (var connection = OpenConnection())
            {
                var sql = "select * from ProductPhotos where ProductID = @ProductID";
                var parameters = new
                {
                    productID
                };
                data = connection.Query<ProductPhoto>(sql: sql, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            return data;
        }

        public bool Update(Product data)
        {
            bool result = false;
            using (var connnection = OpenConnection())
            {
                var sql = @"if not exists (select * from Products where ProductID <> @ProductID)
                            begin
                                update Products
                                set ProductName = @ProductName,
                                    ProductDescription = @ProductDescription,
                                    CategoryID = @CategoryID,
                                    SupplierID = @SupplierID,
                                    Price = @Price,
                                    Photo = @Photo,
                                    IsSelling = @IsSelling
                                where ProductID = @ProductID
                            end;";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    CategoryID = data.CategoryID,
                    SupplierID = data.SupplierID,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling
                };
                result = connnection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connnection.Close();
            }
            return result;
        }

        public bool UpdateAttribute(ProductAttribute data)
        {
            bool result = false;
            using (var connnection = OpenConnection())
            {
                var sql = @"if not exists (select * from ProductAttributes where AttributeID <> @AttributeID and AttributeName = @AttributeName)
                                begin
                                    update ProductAttributes
                                    set ProductID = @ProductID, 
                                        AttributeName = @AttributeName, 
                                        AttributeValue = @AttributeValue, 
                                        DisplayOrder = @DisplayOrder)
                                    where AttributeID = @AttributeID
                                end";
                var parameters = new
                {
                    AttributeID = data.AttributeID,
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName,
                    AttributeValue = data.AttributeValue,
                    DisplayOrder = data.DisplayOrder,
                };
                result = connnection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connnection.Close();
            }
            return result;
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result = false;
            using (var connnection = OpenConnection())
            {
                var sql = @"if exists (select * from ProductPhotos where PhotoID <> @PhotoID and Photo = @Photo)
                                begin
                                    update ProductPhotos
                                    set ProductID = data.ProductID,
                                        Photo = data.Photo,
                                        Description = data.Description ?? "",
                                        DisplayOrder = data.DisplayOrder,
                                        IsHidden = data.IsHidden
                                    where PhotoID = @PhotoID
                                end";
                var parameters = new
                {
                    PhotoID = data.PhotoID,
                    ProductID = data.ProductID,
                    Photo = data.Photo,
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden,
                };
                result = connnection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connnection.Close();
            }
            return result;
        }
    }
}
