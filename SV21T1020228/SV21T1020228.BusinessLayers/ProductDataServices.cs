using SV21T1020228.DataLayers;
using SV21T1020228.DataLayers.SQL_Server;
using SV21T1020228.DomainModels;

namespace SV21T1020228.BusinessLayers
{
    public static class ProductDataService
    {
        private static readonly IProductDAL productDB;
        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }

        public static List<Product> ListOfProducts()
        {
            return productDB.List();
        }

        public static List<Product> ListOfProducts(out int rowCount, int page = 1, int pageSize = 0,
            string searchValue = "", int categoryID = 0, int supplierID = 0,
             decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue, categoryID, supplierID, minPrice, maxPrice);
            return productDB.List(page, pageSize, searchValue, categoryID, supplierID, minPrice, maxPrice);
        }

        public static Product? GetProduct(int productID)
        {
            return productDB.Get(productID);
        }

        public static int AddProduct (Product data)
        {
            return productDB.Add(data);
        }

        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }

        public static bool DeleteProduct(int productID) 
        {
            return productDB.Delete(productID);
        }

        public static bool InUsedProduct(int productID) 
        {
            return productDB.InUsed(productID);
        }

        public static List<ProductPhoto> ListPhotos(int productID)
        {
            return (List<ProductPhoto>)productDB.ListPhotos(productID);
        }

        public static ProductPhoto? GetProductPhoto(int productID) 
        {
            return productDB.GetPhoto(productID);
        }

        public static long AddPhoto(ProductPhoto data) 
        { 
            return productDB.AddPhoto(data);
        }

        public static bool UpdatePhoto(ProductPhoto data) 
        { 
            return productDB.UpdatePhoto(data);
        }
        public static bool DeletePhoto(long photoID) 
        { 
            return productDB.DeletePhoto(photoID);
        }
        public static List<ProductAttribute> ListAttributes(int productID)
        {
            return (List<ProductAttribute>)productDB.ListAttributes(productID);
        }

        public static ProductAttribute? GetProductAttribute(int productID) 
        {
            return productDB.GetAttribute(productID);
        }

        public static long AddAttribute(ProductAttribute data) 
        { 
            return productDB.AddAttribute(data);
        }

        public static bool UpdateAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }
        public static bool DeleteAttribute(long attributeID)
        {
            return productDB.DeleteAttribute(attributeID);
        }


    }
}
