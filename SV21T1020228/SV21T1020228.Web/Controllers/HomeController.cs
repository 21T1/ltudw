using Microsoft.AspNetCore.Mvc;

namespace SV21T1020228.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
