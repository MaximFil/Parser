using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using AngleSharp;
using AngleSharp.Dom;
using Parser.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using AngleSharp.Html.Parser;

namespace Parser.Controllers
{
    [Route("api/[controller]")]
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}