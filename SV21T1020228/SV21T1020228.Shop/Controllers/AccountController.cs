using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SV21T1020228.BusinessLayers;
using SV21T1020228.DomainModels;

namespace SV21T1020228.Shop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            var userData = User.GetUserData();
            if (userData != null)
            {
                int id = int.Parse(userData.UserId);
				var data = CommonDataService.GetCustomer(id);
				return View(data);

			}
            return View("Login");
		}

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            ViewBag.Username = username;

            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Vui lòng nhập thông tin đăng nhập");
                return View();
            }

            // Kiểm tra thông tin khách hàng
            var userAccount = UserAccountService.Authorize(UserTypes.Customer, username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập không thành công!");
                return View();
            }

            // Đăng nhập thành công
            WebUserData userData = new WebUserData()
            {
                UserId = userAccount.UserId,
                UserName = userAccount.UserName,
                DisplayName = userAccount.DisplayName,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userData.CreatePrincipal());

            TempData["Message"] = "Chào mừng " + userData.DisplayName;
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

		[AllowAnonymous]
		[HttpGet]
		public IActionResult SignUp()
		{
			return View();
		}

		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> SignUp(String fullName, String username, String password, String confirmPassword)
		{

            if (string.IsNullOrWhiteSpace(fullName)
                || string.IsNullOrWhiteSpace(username)
                || string.IsNullOrWhiteSpace(password)
                || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ModelState.AddModelError("Error", "Vui lòng nhập đầy đủ thông tin");
                return View();
            }

            if (password != confirmPassword)
            {
				ModelState.AddModelError("Error", "Mật khẩu xác nhận không trùng khớp");
				return View();
			}

            int id = UserAccountService.Create(UserTypes.Customer, fullName, username, password);

            if (id > 0)
            {
				return await Login(username, password);
			} 
            else
            {
				ModelState.AddModelError("Error", "Email đã được sử dụng");
				return View();
			}
		}


		[HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var userData = User.GetUserData();
            if (userData == null)
            {
                return RedirectToAction("Login");
            }

            var username = userData.UserName;
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
			{
				ModelState.AddModelError("Error", "Vui lòng nhập đầy đủ thông tin");
				return View();
			}

			if (oldPassword == newPassword)
			{
				ModelState.AddModelError("Error", "Mật khẩu mới không được trùng với mật khẩu hiện tại");
				return View();
			}

			if (newPassword != confirmPassword)
            {
				ModelState.AddModelError("Error", "Mật khẩu xác nhận không trùng khớp");
				return View();
			}

            bool result = UserAccountService.ChangePassword(UserTypes.Customer, username, oldPassword, newPassword);
            if (!result)
            {
				ModelState.AddModelError("Error", "Mật khẩu hiện tại không chính xác!");
				return View();
			}

			TempData["Message"] = "Thay đổi mật khẩu thành công";
			return View();
        }

        [HttpPost]
		public IActionResult Save(Customer data)
		{
			if (string.IsNullOrWhiteSpace(data.CustomerName) 
                || string.IsNullOrWhiteSpace(data.Phone)
			    || string.IsNullOrWhiteSpace(data.Email)
			    || string.IsNullOrWhiteSpace(data.Address)
                || string.IsNullOrWhiteSpace(data.Province))
			{
				ModelState.AddModelError("Error", "Vui lòng nhập/chọn đầy đủ thông tin");
			}

			if (!ModelState.IsValid)
			{
				return View("Index", data); 
			}

			bool result = CommonDataService.UpdateCustomer(data);
			if (!result)
			{
				ModelState.AddModelError("InfoError", "Email đã được sử dụng");
				return View("Index", data);
			}

            TempData["Message"] = "Cập nhật thông tin thành công";
			return RedirectToAction("Index");
		}
    }
}
