using Microsoft.AspNetCore.Mvc;
using SV21T1020228.BusinessLayers;

namespace SV21T1020228.Web.Controllers
{
    public class CustomerController : Controller
    {
        public const int PAGE_SIZE = 20;

        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount;
            var data = CommonDataService.ListOfCustomers(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            int pageCount = rowCount / PAGE_SIZE;
            if (rowCount % PAGE_SIZE > 0)
            {
                pageCount += 1;
            }

            ViewBag.Page = page;
            ViewBag.RowCount = rowCount;
            ViewBag.PageCount = pageCount;
            ViewBag.searchValue = searchValue;

            return  View(data);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung khách hàng";
            return View("Edit");
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            return View();
        }

        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}
