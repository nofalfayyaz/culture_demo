using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CultureDemo.Models;
using LazZiya.ExpressLocalization;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace CultureDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        // To localize backend strings inject SahredCultureLocalizer
        private readonly ISharedCultureLocalizer _loc;
        

        public HomeController(ILogger<HomeController> logger, ISharedCultureLocalizer loc)
        {
            _logger = logger;
            _loc = loc;
        }

        public IActionResult ActionOne()
        {  
            return View();
        }

        public IActionResult ActionTwo()
        {
          
            var msg = _loc.GetLocalizedString("Privacy Policy");
            TempData.Warning(msg);
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult OnGetSetCultureCookie(string cltr, string returnUrl)
        {

            

            if (string.IsNullOrWhiteSpace(cltr))
            {
                string exisitingCookie = Request.Cookies[".AspNetCore.Culture"];
             
                if (!string.IsNullOrWhiteSpace(exisitingCookie))
                {
                    var cookieParsedValue = CookieRequestCultureProvider.ParseCookieValue(exisitingCookie);
                    Response.Cookies.Append(
                       CookieRequestCultureProvider.DefaultCookieName,
                       CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cookieParsedValue.UICultures[0].Value)),
                       new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                       );
                }
               
            }
            else
            {
                Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            return LocalRedirect(returnUrl);
        }
    }
}
