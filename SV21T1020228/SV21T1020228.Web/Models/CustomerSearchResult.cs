﻿using SV21T1020228.DomainModels;

namespace SV21T1020228.Web.Models
{
    public class CustomerSearchResult : PaginationSearchResult
    {
        public required List<Customer> Data { get; set; }
    }
}