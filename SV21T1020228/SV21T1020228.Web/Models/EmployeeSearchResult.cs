using SV21T1020228.DomainModels;

namespace SV21T1020228.Web.Models
{
    public class EmployeeSearchResult : PaginationSearchResult
    {
        public required List<Employee> Data { get; set; }
    }
}
