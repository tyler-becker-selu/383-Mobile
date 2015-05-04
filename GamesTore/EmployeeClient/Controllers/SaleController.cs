using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeClient.Models;
using RestSharp;
using RestSharp.Deserializers;
using System.Net;
using Newtonsoft.Json;
using EmployeeClient.ViewModels;
using EmployeeClient.Models.ViewModels;
using GamesTore.Models.Data_Transfer_Objects;

namespace EmployeeClient.Controllers
{
    [AuthController(AccessLevel = "Employee")]
    public class SaleController : BaseController
    {


        public List<SaleIndexViewModel> GetSale()
        {
            var request = new RestRequest("Sales", Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            APIHeaders(request);

            var APIresponse = client.Execute(request);
            List<SaleIndexViewModel> cartList = new List<SaleIndexViewModel>();

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                cartList = JsonConvert.DeserializeObject<List<SaleIndexViewModel>>(APIresponse.Content);

                foreach (var item in cartList)
                {
                    item.ID = GetID(item.URL);
                }
            }

            return cartList;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Sale";

            List<SaleIndexViewModel> sale = GetSale();
            foreach (var item in sale)
            {
                item.ID = GetID(item.URL);
                item.Cart.ID = GetID(item.Cart.URL);
                item.User = GetUser(item.Cart.User_Id);
            }

            return View(sale);
        }

        private GetUserDTO GetUser(int id)
        {
            if (Session["Role"] == "Admin")
            {
                var request = new RestRequest("Users/" + id, Method.GET);
                request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                APIHeaders(request);

                var APIresponse = client.Execute(request);

                if (APIresponse.StatusCode == HttpStatusCode.OK)
                {
                    var returnVar = _deserializer.Deserialize<GetUserDTO>(APIresponse);
                    return returnVar;
                }
            }
            return new GetUserDTO();
        }


        [HttpGet]
        public ActionResult CartForSale(int id)
        {
            var request = new RestRequest("Carts/" + id, Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            APIHeaders(request);

            request.AddParameter("checkout", "herdier");

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                var cart = JsonConvert.DeserializeObject<Cart>(APIresponse.Content);
                if (cart.CheckoutReady)
                {
                    return PartialView(cart);
                }
            }
            return PartialView(new Cart());
        }

        // POST: Sale/Create
        [HttpPost]
        public ActionResult CreateSale(int cartID)
        {
            try
            {
                var request = new RestRequest("Carts/" + cartID, Method.GET);
                request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

                APIHeaders(request);

                request.AddParameter("checkout", "herdier");

                var APIresponse = client.Execute(request);

                if (APIresponse.StatusCode == HttpStatusCode.OK)
                {
                    var cart = JsonConvert.DeserializeObject<Cart>(APIresponse.Content);

                    request = new RestRequest("Sales/", Method.POST);
                    request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                    request.RequestFormat = DataFormat.Json;
                    APIHeaders(request);

                    request.AddBody(cart);

                    var response = client.Execute(request);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        var redirect = new UrlHelper(Request.RequestContext).Action("Index", "Home");
                        return Json(new { Url = redirect });
                    }
                }

                throw new Exception("Couldn't make sale");
            }
            catch
            {
                ModelState.AddModelError("", "Could not create a sale.");
                var redirect = new UrlHelper(Request.RequestContext).Action("Index", "Sale");
                return Json(new { Url = redirect });
            }
        }

        // GET: Sale/Edit/5
        public ActionResult Edit(int id)
        {
            var request = new RestRequest("Sales/" + id, Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            APIHeaders(request);

            request.AddParameter("checkout", "herdier");

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                var sale = JsonConvert.DeserializeObject<Sale>(APIresponse.Content);
                sale.ID = GetID(sale.URL);
                sale.Cart.ID = GetID(sale.Cart.URL);

                foreach (GamesForCart item in sale.Cart.Games)
                {
                    item.m_Item1.Id = GetID(item.m_Item1.URL);
                }
                return View(sale);
            }
            return RedirectToAction("Login", "Home", "");

        }

        // POST: Sale/Edit/5
        [HttpPost]
        public ActionResult Edit(EditSaleViewModel sale)
        {
            try
            {
                var request = new RestRequest("Sales/" + sale.Id, Method.GET);
                request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

                APIHeaders(request);

                request.AddParameter("checkout", "herdier");

                var APIresponse = client.Execute(request);

                if (APIresponse.StatusCode == HttpStatusCode.OK)
                { 
                    var sales = JsonConvert.DeserializeObject<Sale>(APIresponse.Content);
                   
                   // sales.

                }

                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



    }
}
