namespace SV21T1020228.Shop.Models
{
    public abstract class PaginationSearchResult
    {
        // <summary>
        /// Trang cần hiển thị
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Số dòng hiển thị trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Tổng số dòng dữ liệu truy vấn được
        /// </summary>
        public int RowCount { get; set; }
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                {
                    return 1;
                }

                int pageCount = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                {
                    pageCount += 1;
                }

                return pageCount;
            }
        }
    }
}
