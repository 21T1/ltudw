using Microsoft.AspNetCore.Authentication.Cookies;
using SV21T1020228.Shop.Models;
using System.Security.Claims;

namespace SV21T1020228.Shop
{
    public class WebUserData
    {
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string DisplayName { get; set; } = "";


        public ClaimsPrincipal CreatePrincipal()
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(nameof(UserId), UserId),
                new Claim(nameof(UserName), UserName),
                new Claim(nameof(DisplayName), DisplayName),
            };
            

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
