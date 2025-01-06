using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SV21T1020228.Web
{
    /// <summary>
    /// Những thông tin cần dùng để biểu diễn/mô tả "danh tính" user
    /// </summary>
    public class WebUserData
    {
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Photo { get; set; } = "";
        public List<string>? Roles { get; set; }
        
        /// <summary>
        /// Chứng nhận ghi nhận danh tính người dùng
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal CreatePrincipal()
        {
            // Tạo danh sách các Claim
            // Mỗi Claim lưu giữ 1 thông tin trong danh tính người dùng
            List<Claim> claims = new List<Claim>()
            {
                new Claim(nameof(UserId), UserId),
                new Claim(nameof(UserName), UserName),
                new Claim(nameof(DisplayName), DisplayName),
                new Claim(nameof(Photo), Photo),
            };
            if (Roles != null)
            {
                foreach (var role in Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // Tạo identity
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Tạp principal
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
