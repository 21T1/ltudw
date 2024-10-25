using SV21T1020228.DataLayers;
using SV21T1020228.DataLayers.SQL_Server;
using SV21T1020228.DomainModels;
using System.Reflection.Metadata.Ecma335;

namespace SV21T1020228.BusinessLayers
{
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Category> categoryDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Employee> employeeDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ISimpleQueryDAL<Province> provinceDAL;

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
            categoryDB = new CategoryDAL(connectionString);
            supplierDB = new SupplierDAL(connectionString);
            employeeDB = new EmployeeDAL(connectionString);
            shipperDB = new ShipperDAL(connectionString);
            provinceDAL = new ProvinceDAL(connectionString);
        }

        /// <summary>
        /// Lấy danh sách toàn bộ các tỉnh thành
        /// </summary>
        /// <returns></returns>
        public static List<Province> ListOfProvinces()
        {
            return provinceDAL.List();
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

        /// <summary>
        /// Lấy thông tin khách hàng có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);
        }

        /// <summary>
        /// Thêm một khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer data)
        {
            return customerDB.Add(data);
        }

        /// <summary>
        /// Cập nhật thông tin một khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }

        /// <summary>
        /// Xóa một khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int id)
        {
            return customerDB.Delete(id);
        }

        /// <summary>
        /// Kiểm tra một khách hàng hiện có đơn hàng liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool InUsedCustomer(int id)
        {
            return customerDB.InUse(id);
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách loại hàng dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// Lấy thông tin loại hàng có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Category? GetCategory(int id)
        {
            return categoryDB.Get(id);
        }

        /// <summary>
        /// Thêm một loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCategory(Category data) { 
            return categoryDB.Add(data); 
        }

        /// <summary>
        /// Cập nhật thông tin một loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }

        /// <summary>
        /// Xóa một loại hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCategory(int id) { 
            return categoryDB.Delete(id); 
        }

        /// <summary>
        /// Kiểm tra một loại hàng hiện có sản phẩm liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool InUsedCategory(int id)
        {
            return categoryDB.InUse(id);
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách người giao hàng dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// Lấy thông tin người giao hàng có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public static Shipper? GetShipper(int id)
        {
            return shipperDB.Get(id);
        }

        /// <summary>
        /// Thêm một người giao hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }

        /// <summary>
        /// Cập nhật thông tin một người giao hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }

        /// <summary>
        /// Xóa một khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteShipper(int id)
        {
            return shipperDB.Delete(id);
        }

        /// <summary>
        /// Kiểm tra một khách hàng hiện có đơn hàng liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool InUsedShipper(int id)
        {
            return shipperDB.InUse(id);
        }


        /// <summary>
        /// Tìm kiếm và lấy danh sách nhân viên dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue);
        }

        public static Employee? GetEmployee(int id)
        {
            return employeeDB.Get(id);
        }

        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }

        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }

        public static bool DeleteEmployee(int id)
        {
            return employeeDB.Delete(id);
        }

        public static bool InUsedEmployee(int id)
        {
            return employeeDB.InUse(id);
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách nhà cung cấp dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue);
        }

        public static Supplier? GetSupplier(int id)
        {
            return supplierDB.Get(id);
        }

        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }

        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }

        public static bool DeleteSupplier(int id)
        {
            return supplierDB.Delete(id);
        }

        public static bool InUsedSupplier(int id)
        {
            return supplierDB.InUse(id);
        }
    }
}
