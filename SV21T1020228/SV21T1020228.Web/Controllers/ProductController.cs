using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SV21T1020228.BusinessLayers;
using SV21T1020228.DataLayers;
using SV21T1020228.DomainModels;
using SV21T1020228.Web.AppCodes;
using SV21T1020228.Web.Models;
using System;

namespace SV21T1020228.Web.Controllers
{
    public class ProductController : Controller
    {
        public const int PAGE_SIZE = 20;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";

        public IActionResult Index()
        {
            ProductSearchInput? condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION);
            if (condition == null)
            {
                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    SupplierID = 0,
                    CategoryID = 0,
                    MinPrice = 0,
                    MaxPrice = 0
                };
            }
            return View(condition);
        }

        public IActionResult Search(ProductSearchInput condition)
        {
            int rowCount;
            var data = ProductDataService.ListOfProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "", condition.SupplierID, condition.CategoryID, condition.MinPrice, condition.MaxPrice);
            ProductSearchResult model = new ProductSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                SupplierID = condition.SupplierID,
                CategoryID = condition.CategoryID,
                MinPrice = condition.MinPrice,
                MaxPrice = condition.MaxPrice,
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            Product data = new Product()
            {
                ProductID = 0,
                IsSelling = true,
            };

            return View("Edit", data);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
            //var data = ProductDataService.GetProduct(id);
            Product data = ProductDataService.GetProduct(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }

            var data = ProductDataService.GetProduct(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }

            return View(data);
        }

        public IActionResult Photo(int id = 0, string method = "", int photoId = 0)
        {
            ProductPhoto data;
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    data = new ProductPhoto
                    {
                        PhotoID = 0,
                        ProductID = id,
                        IsHidden = false
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh của mặt hàng";
                    data = ProductDataService.GetProductPhoto(photoId);
                    return View(data);
                case "delete":
                    ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? uploadPhoto)
        {
            ViewBag.Title = (data.PhotoID == 0 ? "Bổ sung" : "Thay đổi ảnh") + " cho mặt hàng";

            // TODO: Xử lý displayOrder

            // Xử lý với ảnh
            if (uploadPhoto == null)
            {
                if (data.Photo == null)
                {
                    ModelState.AddModelError(nameof(data.Photo), "Vui lòng thêm ảnh mặt hàng");
                    return View("Photo", data);
                }
            }       
            else 
            {
                string fileName = $"{DateTime.Now.Ticks}--{uploadPhoto.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\\products", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            if (data.PhotoID == 0) {
                ProductDataService.AddPhoto(data);
            } 
            else
            {
                ProductDataService.UpdatePhoto(data);
            }

            var product = ProductDataService.GetProduct(data.ProductID);
            return View("Edit", product);
        }

        public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0)
        {
            ProductAttribute data;
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    data = new ProductAttribute
                    {
                        ProductID = id,
                        AttributeID = 0
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính của mặt hàng";
                    data = ProductDataService.GetProductAttribute(attributeId);
                    return View(data);
                case "delete":
                    ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SaveAttribute(ProductAttribute data)
        {
            ViewBag.Title = (data.AttributeID == 0 ? "Bổ sung" : "Thay đổi thuộc tính") + " cho mặt hàng";

            if (string.IsNullOrWhiteSpace(data.AttributeName))
            {
                ModelState.AddModelError(nameof(data.AttributeName), "Vui lòng nhập tên thuộc tính");
            }
            if (string.IsNullOrWhiteSpace(data.AttributeValue))
            {
                ModelState.AddModelError(nameof(data.AttributeValue), "Thứ tự hiển thị không được để trống");
            }
            // TODO: Xử lý displayOrder

            if (!ModelState.IsValid)
            {
                return View("Attribute", data);
            }

            if (data.AttributeID == 0)
            {
                long id = ProductDataService.AddAttribute(data);
                Console.WriteLine(data.ProductID);
                if (id < 0)
                {
                    ModelState.AddModelError(nameof(data.AttributeName), "Tên thuộc tính đã được sử dụng");
                    return View("Attribute", data);
                }
            }
            else
            {
                bool result = ProductDataService.UpdateAttribute(data);
                if (!result)
                {
                    ModelState.AddModelError(nameof(data.AttributeName), "Tên thuộc tính đã được sử dụng");
                    return View("Attribute", data);
                }
            }

            var product = ProductDataService.GetProduct(data.ProductID);
            return View("Edit", product);
        }

        [HttpPost]
        public IActionResult Save(Product data, IFormFile? uploadPhoto, String _price)
        {
            ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";

            if (string.IsNullOrWhiteSpace(data.ProductName))
            {
                ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.ProductDescription))
            {
                data.ProductDescription = "";
            }
            if (data.CategoryID == 0)
            {
                ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng chọn loại hàng");
            }
            if (data.SupplierID == 0)
            {
                ModelState.AddModelError(nameof(data.SupplierID), "Vui lòng chọn nhà cung cấp");
            }
            if (string.IsNullOrWhiteSpace(data.Unit))
            {
                ModelState.AddModelError(nameof(data.Unit), "Vui lòng nhập đơn vị tính");
            }

            if (Decimal.TryParse(_price, out decimal price) && (price >= 0))
            {
                data.Price = price;
            } 
            else
            {
                ModelState.AddModelError(nameof(data.Price), "Giá cả không hợp lệ");
            }
            if (!ModelState.IsValid)
            {
                return View("Edit", data);  //  Trả dữ liệu về View kèm theo các thông báo lỗi
            }

            // Xử lý với ảnh
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}--{uploadPhoto.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\\products", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }


            if (data.ProductID == 0)
            {
                int id = ProductDataService.AddProduct(data);
                if (id < 0)
                {
                    ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng đã được sử dụng");
                    return View("Edit", data);
                }
            }
            else
            {
                bool result = ProductDataService.UpdateProduct(data);
                if (!result)
                {
                    ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng đã được sử dụng");
                    return View("Edit", data);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
