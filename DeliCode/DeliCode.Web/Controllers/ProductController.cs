using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            List<string> test = new List<string>();
            test.Add("Hej");
            test.Add("Funkar");
            test.Add("Detta");
            test.Add("Verkligen");
            return View(test);
        }
    }
}
