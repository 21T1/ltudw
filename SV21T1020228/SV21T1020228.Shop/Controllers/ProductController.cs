﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020228.BusinessLayers;
using SV21T1020228.Shop.AppCodes;
using SV21T1020228.Shop.Models;

namespace SV21T1020228.Shop.Controllers
{
    [AllowAnonymous]
    public class ProductController : Controller
    {
        public const int PAGE_SIZE = 30;
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

        public IActionResult Detail(int id)
        {
            var data = ProductDataService.GetProduct(id);

            if (data != null)
            {
                return View(data);
            }
            return View();
        }
    }
}
