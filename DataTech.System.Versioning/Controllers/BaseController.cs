using DataTech.System.Versioning.Extensions;
using DataTech.System.Versioning.Models.Common;
using DataTech.System.Versioning.Models.DataTable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures; 
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataTech.System.Versioning.Controllers
{
    public class BaseController : Controller
    {

        public string CurrentCulture { get; set; }
        public UserIdentity CurrentUser { get; set; } 



        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HandelLanguage();

            CurrentUser = HttpContext.Items[SystemVariables.CurrentUser] as UserIdentity;
            ViewBag.ViewUser = CurrentUser;
        }


        protected string Translate(string message)
        {
            CurrentCulture = string.IsNullOrEmpty(CurrentCulture) ? "en-us" : CurrentCulture;
            return message; //Shams.Erp.Common.Language.Messages.GetMessage(message, CurrentCulture);
        }


        private void HandelLanguage()
        {
            string culture = "en-us";
            CurrentCulture = culture;

            if (HttpContext.Request.Query.ContainsKey("Culture"))
            {
                culture = HttpContext.Request.Query["Culture"].ToString();
                HttpContext.Response.Cookies.Delete("Culture");
                HttpContext.Response.Cookies.Append("Culture", culture);
                CurrentCulture = culture;

                //TODO: Update db
            }
            else
            {
                if (Request.Cookies.ContainsKey("Culture"))
                {
                    CurrentCulture = Request.Cookies.TryGetValue("Culture", out culture) ? culture : "en-us";
                }
                else
                {
                    CurrentCulture = culture;
                    Response.Cookies.Append("Culture", culture);
                }
            }

            ViewBag.Culture = CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CurrentCulture, false);

        }


        public string RenderPartialViewToString(ICompositeViewEngine viewEngine, string viewName, object model)
        {
            viewName ??= ControllerContext.ActionDescriptor.ActionName;
            ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                IView view = viewEngine.FindView(ControllerContext, viewName, false).View;
                ViewContext viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, sw, new HtmlHelperOptions());
                view.RenderAsync(viewContext).Wait();
                return sw.GetStringBuilder().ToString();
            }
        }

        protected void PrepareResult<T>(OperationResult<T> result)
        {
            if (!result.Result)
            {
                if (result.ErrorCategory != "Exception")
                {
                    if (!string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        result.ErrorMessage = Translate(result.ErrorMessage);
                    }
                    else if (string.IsNullOrEmpty(result.ErrorMessage) && !string.IsNullOrEmpty(result.ErrorCode))
                    {
                        result.ErrorMessage = Translate(result.ErrorCode);
                    }
                }
                else if (result.ErrorCode.EqualsInsensitive("MissingParameter"))
                {
                    result.ErrorMessage = Translate("CheckYourInputs") + $" - ({result.ErrorMessage})";
                }
                 

            }
        }

        protected void PrepareResult<T, R>(OperationResult<T> source, OperationResult<R> target)
        {
            if (!source.Result)
            {
                if (source.ErrorCategory != "Exception")
                {
                    if (!string.IsNullOrEmpty(source.ErrorMessage))
                    {
                        target.ErrorMessage = Translate(source.ErrorMessage);
                    }
                    else if (string.IsNullOrEmpty(source.ErrorMessage) && !string.IsNullOrEmpty(source.ErrorCode))
                    {
                        target.ErrorMessage = Translate(source.ErrorCode);
                    }
                } 
            }
            else
            {
                target.Result = source.Result;
            }
        }

        protected Query<T> GetSearchQuery<T>(out DataTableOptions options)
        {
            Query<T> query = new Query<T>();

            options = new DataTableOptions();

            try
            {
                options = Request.GetDataTableOptions();
                OrderDir orderDir = OrderDir.ASC;
                Enum.TryParse(options.SortColumns.FirstOrDefault().Direction, true, out orderDir);

                query.PageSize = options.PageSize;
                query.PageIndex = options.PageIndex;
                query.OrderDir = orderDir;
                query.OrderBy = options.SortColumns?.FirstOrDefault()?.Name;
            }
            catch
            {

            }

            return query;
        }



        protected JsonResult DTResult<T>(DataTableOptions options = null,
            PaggingOperationResult<T> result = null,
            object data = null)
        {
            return Json(new
            {
                draw = options == null ? "" : options.Draw,
                recordsFiltered = result == null ? 0 : result.TotalCount,
                recordsTotal = result == null ? 0 : result.TotalCount,
                data = data == null ? (result.Response ?? new List<T>()) : data
            });
        }

    }
}
