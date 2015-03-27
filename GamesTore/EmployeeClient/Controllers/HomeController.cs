using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GamesTore.Models;

namespace EmployeeClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

        public ApiKey CheckCredentials(string email, string password)
        {
            var restClient = new RestSharp.RestClient("http://localhost:12932/");
            var request = new RestSharp.RestRequest(RestSharp.Method.GET);

            request.AddParameter("email", email);
            request.AddParameter("password", password);

            var response = restClient.Execute<ApiKey>(request);

            return response.Data;
        }
    }
}