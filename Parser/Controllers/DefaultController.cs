using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Parser.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace Parser.Controllers
{
    [Route("api/[controller]")]
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("[action]")]
        public IWebElement TitleArticles()
        {
            IWebDriver driver=new ChromeDriver();
            driver.Url = @"https://habr.com/ru/news/";
            IWebElement links = (OpenQA.Selenium.IWebElement)driver.FindElements(By.ClassName("post__title_link"));
            return links;
        }
    }
}