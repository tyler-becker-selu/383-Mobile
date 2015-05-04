using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using System.Net;
using System.Web.Helpers;
using GamesTore.Models.Data_Transfer_Objects;

namespace EmployeeClient.Controllers
{
    public class HomeController : BaseController
    {
       
        [AuthController(AccessLevel = "Employee")]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Group 4 Store!";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Please Login to use this website";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            var request = new RestRequest("ApiKey?email=" + email + "&password=" + password, Method.GET);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                GetApikeyDTO userData = _deserializer.Deserialize<GetApikeyDTO>(response);
                Session["ApiKey"] = userData.ApiKey;
                Session["UserId"] = userData.UserId;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Login data is incorrect!");
            return View();
        }


        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session["ApiKey"] = null;
            Session["Role"] = null;

            return RedirectToAction("Login");
        }
    }
}