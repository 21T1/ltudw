namespace SV21T1020228.Web.Models
{
    /// <summary>
    /// Lưu giữ các thông tin đầu vào sử dụng cho chức năng tìm kiếm và hiển thị dữ liệu dưới dạng phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        /// <summary>
        /// Trang cần hiển thị
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// Số dòng hiển thị trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }
		/// <summary>
		/// Chuỗi giá trị cần tìm kiếm
		/// </summary>
		public String SearchValue { get; set; } = "";
	}
}
