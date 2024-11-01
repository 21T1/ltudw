using SV21T1020228.DomainModels;

namespace SV21T1020228.Web.Models
{
    public class CategorySearchResult : PaginationSearchResult
    {
        public required List<Category> Data { get; set; }
    }
}
