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
    [AuthController(AccessLevel = "Admin")]
    public class UserController : BaseController
    {

        // GET: User
        public ActionResult Index(string message)
        {
            ViewBag.Message = message;

            var request = new RestRequest("Users", Method.GET);
            APIHeaders(request);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                IEnumerable<GetUserDTO> users = _deserializer.Deserialize<List<GetUserDTO>>(response);

                foreach (GetUserDTO item in users)
                {
                    item.Id = GetID(item.URL);
                }

                return View(users);
            }

            return RedirectToAction("Login", "Home");
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Message = "Users";

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

                var json = JsonConvert.SerializeObject(user);

                request.AddParameter("text/json", json, ParameterType.RequestBody);

                var response = client.Execute(request);

                if(response.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("", "Email is already in use.");
            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            var request = new RestRequest("Users/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            APIHeaders(request);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                SetUserDTO user = _deserializer.Deserialize<SetUserDTO>(response);
                return View(user);
            }

            return HttpNotFound();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SetUserDTO user)
        {
            var request = new RestRequest("Users/{id}", Method.PUT);
            request.AddUrlSegment("id", id.ToString());
            APIHeaders(request);

            var json = JsonConvert.SerializeObject(user);

            request.AddParameter("text/json", json, ParameterType.RequestBody);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index", new { id = id });
            }

            return HttpNotFound();
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
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

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, string deleteMessage)
        {
            var request = new RestRequest("Users/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());
            APIHeaders(request);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index", new { message = deleteMessage});
            }
            return HttpNotFound();
        }
    }
}
