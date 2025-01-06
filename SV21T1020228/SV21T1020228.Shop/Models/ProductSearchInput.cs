namespace SV21T1020228.Shop.Models
{
    public class ProductSearchInput : PaginationSearchInput
    {
		public String SearchValue { get; set; } = "";
		public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set;} = 0;
    }
}
