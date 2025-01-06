namespace SV21T1020228.DomainModels
{
    /// <summary>
    /// Sản phẩm trong giỏ hàng
    /// </summary>
    public class CartItem
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public string Photo { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal TotalPrice
        {
            get
            {
                return Quantity * Price;
            }
        }
    }

}
