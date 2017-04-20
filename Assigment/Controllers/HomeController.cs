using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assigment.Models;

namespace Assigment.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var carEntities = new MyDBEntities();
            return View(carEntities.Cars.ToList());
        }

    }
}