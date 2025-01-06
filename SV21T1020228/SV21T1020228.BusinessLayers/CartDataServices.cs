using SV21T1020228.DataLayers;
using SV21T1020228.DataLayers.SQL_Server;
using SV21T1020228.DomainModels;

namespace SV21T1020228.BusinessLayers
{
    public static class CartDataServices
    {
        public static readonly ICartDAL cartDB;
        static CartDataServices()
        {
            cartDB = new CartDAL(Configuration.ConnectionString);
        }

        public static List<CartItem>? GetCart(int cartID)
        {
            return (List<CartItem>?)cartDB.Get(cartID);
        }

        public static bool DeleteCart(int cartID)
        {
            return cartDB.Delete(cartID);
        }

        public static bool AddToCart(CartItem data)
        {
            return cartDB.AddItem(data);
        }

        public static bool UpdateItem(CartItem data)
        {
            return cartDB.UpdateItem(data);
		}

        public static bool RemoveFromCart(int cartID, int productID)
        {
            return cartDB.RemoveItem(cartID, productID);
        }
    }
}
