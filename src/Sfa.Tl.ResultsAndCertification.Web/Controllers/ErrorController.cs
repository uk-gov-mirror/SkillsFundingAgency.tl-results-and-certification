﻿using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.Models;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        [Route("page-not-found", Name = "PageNotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            return statusCode switch
            {
                404 => RedirectToRoute("PageNotFound"),
                _ => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }),
            };
        }
    }
}