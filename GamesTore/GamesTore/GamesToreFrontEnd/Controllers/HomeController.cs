using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamesToreFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }

        public ActionResult Games()
        {
            return View();
        }

        public ActionResult Carts()
        {
            return View();
        }

        public ActionResult Sales()
        {
            return View();
        }

        public ActionResult Genres()
        {
            return View();
        }

        public ActionResult Tags()
        {
            return View();
        }
    }
}
