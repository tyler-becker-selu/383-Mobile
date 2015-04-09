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
    public class HomeController : Controller
    {
        private RestClient client = new RestClient("http://localhost:12932/api");

        RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();

        private void APIHeaders(RestRequest request)
        {
            if(Session["ApiKey"] != null && Session["UserId"] != null)
            {
                request.AddHeader("xcmps383authenticationkey", Session["ApiKey"].ToString());
                request.AddHeader("xcmps383authenticationid", Session["UserId"].ToString());
            }
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            var request = new RestRequest("ApiKey?email={email}&password={password}", Method.GET);
            request.AddUrlSegment("email", email);
            request.AddUrlSegment("password", password);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                GetApikeyDTO userData = _deserializer.Deserialize<GetApikeyDTO>(response);
                Session["ApiKey"] = userData.ApiKey;
                Session["UserId"] = userData.UserId;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login");
        }

    }
}