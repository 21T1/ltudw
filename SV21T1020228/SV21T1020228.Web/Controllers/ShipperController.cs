using Microsoft.AspNetCore.Mvc;

namespace SV21T1020228.Web.Controllers
{
    public class ShipperController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung người giao hàng";
            return View("Edit");
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin người giao hàng";
            return View();
        }

        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}
