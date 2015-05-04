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

namespace EmployeeClient.Controllers
{
    [AuthController(AccessLevel = "Employee")]
    public class SaleController : BaseController
    {
      


        public List<Cart> GetCart()
        {
            var request = new RestRequest("Carts", Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            APIHeaders(request);

           

            var APIresponse = client.Execute(request);
            List<Cart> cartList = new List<Cart>();

            if (APIresponse.StatusCode == HttpStatusCode.OK)  
            {
                cartList = JsonConvert.DeserializeObject<List<Cart>>(APIresponse.Content);
                
                foreach (var item in cartList)
                {
                    item.ID = GetID(item.URL);

                    foreach (var game in  item.Games)
                    {
                        game.m_Item1.Id = GetID(game.m_Item1.URL);
                    }
                }
            }

            return cartList;
        }

        public List<Sale> GetSale()
        {
            var request = new RestRequest("Sales", Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            APIHeaders(request);



            var APIresponse = client.Execute(request);
            List<Sale> cartList = new List<Sale>();

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                cartList = JsonConvert.DeserializeObject<List<Sale>>(APIresponse.Content);

                foreach (var item in cartList)
                {
                    item.ID = GetID(item.URL);
                }
            }

            return cartList;
        }

        public ActionResult SaleList()
        {
            ViewBag.Message = "Sale";

            var cart = GetSale();
            return View(cart);
        }

        public ActionResult Index() 
        {
            ViewBag.Message = "Sale";

            var cart = GetCart();
            return View(cart);
        }



        // GET: Sale/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Message = "Sale";

            return View();
        }

        // POST: Sale/Create
        [HttpPost]
        public ActionResult CreateSale(int id)
        {
            try
            {

                var request = new RestRequest("Carts/" + id, Method.GET);
                request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                
                APIHeaders(request);

                request.AddHeader("checkout", "herdier");

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

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var redirect = new UrlHelper(Request.RequestContext).Action("Index", "Sale");
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
            return View();
        }

        // POST: Sale/Edit/5
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


      
    }
}
