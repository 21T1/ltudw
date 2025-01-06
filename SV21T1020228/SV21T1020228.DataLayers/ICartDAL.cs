using SV21T1020228.DomainModels;

namespace SV21T1020228.DataLayers
{
    public interface ICartDAL
    {
        bool AddItem(CartItem data);
        int CountItem(int cartID);
        bool Delete(int cartID);
        bool RemoveItem(int cartID, int productID);
        IList<CartItem>? Get(int cartID);
        bool UpdateItem(CartItem data);
    }
}
