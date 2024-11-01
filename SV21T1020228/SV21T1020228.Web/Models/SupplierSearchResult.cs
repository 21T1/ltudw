using SV21T1020228.DomainModels;

namespace SV21T1020228.Web.Models
{
    public class SupplierSearchResult : PaginationSearchResult
    {
        public required List<Supplier> Data { get; set; }
    }
}
