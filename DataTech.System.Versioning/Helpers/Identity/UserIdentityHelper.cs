
using DataTech.System.Versioning.Models.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
namespace DataTech.System.Versioning.Helpers.Identity
{
    public class UserIdentityHelper
    {
        public static async Task SetIdentity(HttpContext context, UserIdentity identity)
        {
            var userIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            List<Claim> userClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, identity.UserId.ToString()),
                    new Claim(ClaimTypes.Name, identity.Username??string.Empty),
                    new Claim(ClaimTypes.Locality, identity.Culture??string.Empty),
                    new Claim(ClaimTypes.Email, identity.Email??string.Empty),
                    new Claim(ClaimTypes.GivenName, identity.FullName??string.Empty),
                    new Claim(ClaimTypes.Actor, identity.Avatar??string.Empty)
                };

            userIdentity.AddClaims(userClaims);
            var principal = new ClaimsPrincipal(userIdentity);
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddMinutes(60) });
           
        }

        public static UserIdentity GetUserFromIdentity(HttpContext context)
        {
            try
            {

                var currentUser = context.User;

                UserIdentity user = new UserIdentity
                {
                    Username = currentUser.FindFirstValue(ClaimTypes.Name),
                    UserId = Guid.Parse(currentUser.FindFirstValue(ClaimTypes.NameIdentifier)),
                    FullName = currentUser.FindFirstValue(ClaimTypes.GivenName),
                    Avatar = currentUser.FindFirstValue(ClaimTypes.Actor),
                    Email = currentUser.FindFirstValue(ClaimTypes.Email),
                    Culture = currentUser.FindFirstValue(ClaimTypes.Locality)
                };

                return user;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
