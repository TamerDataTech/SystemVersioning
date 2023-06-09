﻿using DataTech.System.Versioning.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using System.IO;

using IoPath = System.IO.Path;
namespace DataTech.System.Versioning.Attributes
{
    public abstract class BasePage<T> : RazorPage<T>
    {

        protected BasePage()
        {

        }

        public string CurrentCulture => ViewData["Culture"] == null ? "en-US" : ViewData["Culture"].ToString();

        public UserIdentity CurrentUser => ViewBag.ViewUser;
        public string RequestedPagePath => Context.Request.Path;


        // Source: https://dotnetstories.com/blog/How-to-implement-a-custom-base-class-for-razor-views-in-ASPNET-Core-en-7106773524?o=rss 
        // Returns the file path of the view
        public string ViewPath => ViewContext.ExecutingFilePath;
        // Returns the file name of the view
        public string ViewFileName => IoPath.GetFileName(ViewContext.ExecutingFilePath);

        // Standardized way to fill the title and description,
        // avoiding any misspell and factorized way to do the action
        public void SetTitle(string title) { ViewBag.PageTitle = Translate(title); }
        public void SetDescription(string descr) { ViewBag.PageDescription = descr; }


        // Allows to easily get a query parameter
        public string GetQueryParameter(string key)
        {
            return Context.Request.Query[key];
        }

        public string Translate(string key)
        {
            return key;  
        } 

        // Returns the site address
        public string SiteBasePath
        {
            get
            {
                return $"{Context.Request.Scheme}://{Context.Request.Host.Host}{(Context.Request.Host.Port != 80 && Context.Request.Host.Port != 443 ? $":{Context.Request.Host.Port}" : "")}";
            }
        }

        public override void BeginContext(int position, int length, bool isLiteral)
        {
            // Do some work here will be executed before every part of contexts of the view. 
            // Each code block of the view, taghelper or html block will act as a context.
            base.BeginContext(position, length, isLiteral);
        }


    }
}
