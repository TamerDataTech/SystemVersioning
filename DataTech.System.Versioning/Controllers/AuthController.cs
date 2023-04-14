using DataTech.System.Versioning.Extensions;
using DataTech.System.Versioning.Models.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace DataTech.System.Versioning.Controllers
{
    public class AuthController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //  var path = filterContext.HttpContext.Request.RouteValues["controller"].ToString().ToLower();

            base.OnActionExecuting(filterContext);
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (CurrentUser == null)
            {
                filterContext.HttpContext.SignOutAsync();

                if (isAjax)
                {
                    if (Request.Headers["Accept"].ToString().IndexOf("json") > -1)
                    {
                        filterContext.Result = Ok(new OperationResult
                        {
                            ErrorMessage = "YourSessionHasTimedOut",
                            ErrorCode = "NotAuthorized"
                        });
                    }
                }
                else
                {
                    var dic = new Microsoft.AspNetCore.Routing.RouteValueDictionary
                        {
                            {"controller", "user"},
                            {"action", "login"}
                        };

                    var path = Request.Path.ToString();
                    if (path.IsNotEmpty())
                    {
                        dic.Add("returnUrl", path);
                    }
                    filterContext.Result = new RedirectToRouteResult(dic);
                }

                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

        }
    }
}
