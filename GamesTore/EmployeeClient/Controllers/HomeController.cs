using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GamesTore.Models;
using RestSharp;
using System.Net;
using System.Web.Helpers;

namespace EmployeeClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string bruh)
        {
            ViewBag.Message = bruh;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            var client = new RestClient("http://localhost:12932/api");
            var request = new RestRequest("ApiKey?email={email}&password={password}", Method.GET);
            request.AddUrlSegment("email", email);
            request.AddUrlSegment("password", password);
            
            var response = client.Execute(request);

          //  if (response.StatusCode == HttpStatusCode.OK)
            //{
            return RedirectToAction("Index", new {bruh= response.StatusCode.ToString() });
          //  }

           // return HttpNotFound();
        }

    }
}