using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeClient.Controllers;
using RestSharp;
using System.Net;
using GamesTore.Models.Data_Transfer_Objects;
using GamesTore.Models;
using Newtonsoft.Json;

namespace EmployeeClient.Controllers
{
    public class UserController : Controller
    {
        private RestClient client = new RestClient("http://localhost:12932/api");

        RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();

        private void APIHeaders(RestRequest request)
        {
            if (Session["ApiKey"] != null && Session["UserId"] != null)
            {
                request.AddHeader("xcmps383authenticationkey", Session["ApiKey"].ToString());
                request.AddHeader("xcmps383authenticationid", Session["UserId"].ToString());
            }
        }

        // GET: User
        public ActionResult Index()
        {
            var request = new RestRequest("Users", Method.GET);
            APIHeaders(request);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                IEnumerable<GetUserDTO> users = _deserializer.Deserialize<List<GetUserDTO>>(response);
                return View(users);
            }

            return RedirectToAction("Login", "Home");
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            var request = new RestRequest("Users/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            APIHeaders(request);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                GetUserDTO user = _deserializer.Deserialize<GetUserDTO>(response);
                return View(user);
            }

            return HttpNotFound();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SetUserDTO user)
        {
            if(ModelState.IsValid)
            {
                var request = new RestRequest("Users", Method.POST);
                APIHeaders(request);

                user.Role = Roles.User;

                var json = JsonConvert.SerializeObject(user);

                request.AddParameter("text/json", json, ParameterType.RequestBody);

                var response = client.Execute(request);

                if(response.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction("Index");
                }
            }
            return HttpNotFound();
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
