using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020228.BusinessLayers;

namespace SV21T1020228.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]  // Ngăn chặn việc post thông tin từ bên ngoài web
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            ViewBag.Username = username;

            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Vui lòng nhập tên đăng nhập và mật khẩu");
                return View();
            }

            // Kiểm tra username & password của Employee
            var userAccount = UserAccountService.Authorize(UserTypes.Employee, username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập không thành công");
                return View();
            }

            // Đăng nhập thành công

            // 1. Tạo thông tin "định danh" người dùng
            WebUserData userData = new WebUserData()
            {
                UserId = userAccount.UserId,
                UserName = userAccount.UserName,
                DisplayName = userAccount.DisplayName,
                Photo = userAccount.Photo,
                Roles = userAccount.RoleNames.Trim().Split(',').ToList()
            };

            // 2. Ghi nhận trạng thái đăng nhập
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userData.CreatePrincipal());

            // 3. Quay về trang chủ
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult ChangePassword(string username, string oldPassword, string newPassword, string confirmPassword)
        {
            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ModelState.AddModelError("Error", "Vui lòng nhập đầy đủ các trường");
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("Error", "Mật khẩu xác nhận không trùng khớp");
                return View();
            }

            if (oldPassword == newPassword)
            {
                ModelState.AddModelError("Error", "Mật khẩu mới không được trùng với mật khẩu hiện tại");
                return View();
            }

            bool result = UserAccountService.ChangePassword(UserTypes.Employee, username, oldPassword, newPassword);
            if (!result)
            {
                ModelState.AddModelError("Error", "Mật khẩu hiện tại không đúng");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        public IActionResult AccessDenined()
        {
            return View();
        }
    }
}
