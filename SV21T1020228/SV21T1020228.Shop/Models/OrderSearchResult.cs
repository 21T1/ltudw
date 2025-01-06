using SV21T1020228.DomainModels;

namespace SV21T1020228.Shop.Models
{
    public class OrderSearchResult : PaginationSearchResult
    {
        public int CustomerID { get; set; } = 0;
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
        public List<Order> Data { get; set; } = new List<Order>();
    }
}
