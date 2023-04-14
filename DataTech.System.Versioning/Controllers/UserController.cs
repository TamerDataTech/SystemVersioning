using DataTech.System.Versioning.Helpers.Identity;
using DataTech.System.Versioning.Models.Common;
using DataTech.System.Versioning.Models.Request;
using DataTech.System.Versioning.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DataTech.System.Versioning.Controllers
{
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(ILogger<UserController> logger, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Login(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnURL = returnUrl;
            }

            if (CurrentUser != null)
            {
                if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("index", "home");
                }

            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoLogin(LoginRequest login)
        {
            var result = await _userService.Login(login);
            if (result.Result && result.Response.Id != Guid.Empty)
            {
                var user = result.Response;                   

                var identiyUser = new UserIdentity
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.Fullname,
                    Culture = "en-us"
                };

                await UserIdentityHelper.SetIdentity(HttpContext, identiyUser);

                result.Result = true;
                result.Message = login.ReturnUrl;

                return Ok(result);
            }

            PrepareResult(result);

            return Ok(result);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Items[SystemVariables.CurrentUser] = null;
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(SystemVariables.ClientId);
            return Redirect("/user/login");
        }
    }
}
