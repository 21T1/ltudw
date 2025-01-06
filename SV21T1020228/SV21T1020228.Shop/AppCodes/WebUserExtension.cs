using System.Security.Claims;

namespace SV21T1020228.Shop
{
    public static class WebUserExtension
    {
        /// <summary>
        /// Đọc thông tin người dùng được "ghi" trong principal
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static WebUserData? GetUserData(this ClaimsPrincipal principal)
        {
            try
            {
                Console.WriteLine("This is principal: ", principal, " ", principal.Identity);
                if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
                {
                    return null;
                }

                var userData = new WebUserData();

                userData.UserId = principal.FindFirstValue(nameof(userData.UserId)) ?? "";
                userData.UserName = principal.FindFirstValue(nameof(userData.UserName)) ?? "";
                userData.DisplayName = principal.FindFirstValue(nameof(userData.DisplayName)) ?? "";

                return userData;
            }
            catch
            {
                return null;
            }
        }
    }
}
