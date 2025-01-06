using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020228.BusinessLayers;
using SV21T1020228.DomainModels;
using SV21T1020228.Shop.AppCodes;
using SV21T1020228.Shop.Models;
using System.Globalization;

namespace SV21T1020228.Shop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        public const int PAGE_SIZE = 20;
        private const string ORDER_SEARCH_CONDITION = "OrderSearchCondition";

        private const string SHOPPING_CART = "ShoppingCart";

        public IActionResult Index()
        {
            var userData = User.GetUserData();
            var condition = ApplicationContext.GetSessionData<OrderSearchInput>(ORDER_SEARCH_CONDITION);
            if (condition == null)
            {
                var cultureInfo = new CultureInfo("en-GB");
                condition = new OrderSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    Status = 0,
                    TimeRange = $"({DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy", cultureInfo)} - {DateTime.Today.ToString("dd/MM/yyyy", cultureInfo)})"
                };
            }
            return View(condition);
        }

        public IActionResult Search(OrderSearchInput condition)
        {
            Console.Write(int.Parse(User.GetUserData().UserId));
            int rowCount;
            var data = OrderDataService.ListOrders(out rowCount, condition.Page, condition.PageSize, condition.Status, condition.FromTime, condition.ToTime, int.Parse(User.GetUserData().UserId));
            var model = new OrderSearchResult()
            {
                Page = condition.Page,
                PageSize = PAGE_SIZE,
                RowCount = rowCount,
                Status = condition.Status,
                TimeRange = condition.TimeRange,
                Data = data
            };

            ApplicationContext.SetSessionData(ORDER_SEARCH_CONDITION, condition);

            return View(model);
        }

        public IActionResult Details(int id)
        {
			var order = OrderDataService.GetOrder(id);
			if (order == null)
			{
				return RedirectToAction("Index");
			}

			var details = OrderDataService.ListOrderDetails(id);
			var model = new OrderDetailModel()
			{
				Order = order,
				Details = details,
			};

			return View(model);
		}

        [HttpGet]
        public IActionResult Init()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Init(string deliveryProvince = "", string deliveryAddress = "")
        {
            var userData = User.GetUserData();
            int cartID = int.Parse(userData.UserId);

            var shoppingCart = CartDataServices.GetCart(cartID);
            if (shoppingCart.Count == 0)
            {
                return Json("Giỏ hàng trống. Vui lòng chọn thêm mặt hàng vào giỏ");
            }

            if (string.IsNullOrWhiteSpace(deliveryProvince) || string.IsNullOrWhiteSpace(deliveryAddress))
            {
                //TODO: xử lý dữ liệu
                return Json("Vui lòng nhập đầy đủ thông tin giao hàng");
            }

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in shoppingCart)
            {
                orderDetails.Add(new OrderDetail()
                {
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                });
            }

            int orderID = OrderDataService.InitOrder(1,cartID, deliveryProvince, deliveryAddress, orderDetails);

            if (orderID > 0)
            {
                CartDataServices.DeleteCart(cartID);
                return RedirectToAction("Details", new { id = orderID });
            }

            return Json(orderID);
        }
    }
}
