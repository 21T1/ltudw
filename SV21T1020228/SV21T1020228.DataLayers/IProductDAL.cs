using SV21T1020228.DomainModels;

namespace SV21T1020228.DataLayers
{
    public interface IProductDAL
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang</param>
        /// <param name="searchValue">Tên mặt hàng cần tìm</param>
        /// <param name="categoryID">Mã loại hàng cần tìm</param>
        /// <param name="supplierID">Mã nhà cung cấp cần tìm</param>
        /// <param name="minPrice">Mức giá nhỏ nhất trong khoảng giá cần tìm</param>
        /// <param name="maxPrice">Mức giá lớn nhất trong khoảng giá cần tìm</param>
        /// <returns></returns>
        List<Product> List(int page = 1, int pageSize = 0,
            string searchValue = "", int categoryID = 0, int supplierID = 0,
             decimal minPrice = 0, decimal maxPrice = 0);

        /// <summary>
        /// Đếm số lượng mặt hàng tìm kiếm được
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="categoryID"></param>
        /// <param name="supplierID"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);
        /// <summary>
        /// Lấy thông tin mặt hàng theo mã hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        Product? Get(int productID);
        /// <summary>
        /// Thêm một mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Product data);
        /// <summary>
        /// Cập nhật thông tin mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Product data);
        /// <summary>
        /// Xóa mặt hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        bool Delete(int productID);
        /// <summary>
        /// Kiểm tra đơn hàng liên quan
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        bool InUsed(int productID);

        IList<ProductPhoto> ListPhotos(int productID);
        ProductPhoto? GetPhoto(long photoID);
        long AddPhoto(ProductPhoto data);
        bool UpdatePhoto(ProductPhoto data);
        bool DeletePhoto(long photoID);

        IList<ProductAttribute> ListAttributes(int productID);
        ProductAttribute? GetAttribute(long attributeID);
        long AddAttribute(ProductAttribute data);
        bool UpdateAttribute(ProductAttribute data);
        bool DeleteAttribute(long attributeID);

    }
}
