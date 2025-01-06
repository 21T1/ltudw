using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020228.BusinessLayers;
using SV21T1020228.DomainModels;

namespace SV21T1020228.Shop.Controllers
{
	[Authorize]
	public class CartController : Controller
    {
		private int GetCartID()
        {
            var userData = User.GetUserData();
            int id = int.Parse(userData.UserId);
            return id;
        }

        private List<CartItem> GetShoppingCart()
        {
            var data = CartDataServices.GetCart(GetCartID());
            if (data == null)
            {
                data = new List<CartItem>();
            }
			return data;
        }

        private IActionResult ClearShopingCart()
        {
            bool result = CartDataServices.DeleteCart(GetCartID());
            if (result)
            {
                return Json("");
            }
            else
            {
                return Json("Không thể xóa sản phẩm trong giỏ hàng");
            }
        }

        public IActionResult Index()
        {
            var data = GetShoppingCart();
			return View(data);
        }

        public IActionResult Add(int productID, int quantity)
        {
            if (quantity < 1)
            {
                quantity = 1;
            }
            var item = new CartItem
            {
                CartID = GetCartID(),
                ProductID = productID,
                Quantity = quantity
            };

            bool result = CartDataServices.AddToCart(item);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            bool result = CartDataServices.RemoveFromCart(GetCartID(), id);
            if (result)
            {
                return Json("");
            }
            else
            {
                return Json("???");
            }
        }

        [HttpPost]
		public IActionResult Update([FromBody] CartItem data)
		{
			bool result = CartDataServices.UpdateItem(data);
			if (result)
			{
				return Json("");
			}
			else
			{
				return Json("???");
			}
		}

	}
}
