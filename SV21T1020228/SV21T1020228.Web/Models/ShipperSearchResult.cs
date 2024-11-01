﻿using SV21T1020228.DomainModels;

namespace SV21T1020228.Web.Models
{
    public class ShipperSearchResult : PaginationSearchResult
    {
        public required List<Shipper> Data { get; set; }
    }
}