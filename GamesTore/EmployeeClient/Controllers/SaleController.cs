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
                }
            }

            return cartList;
        }




        // GET: Sale
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

        // GET: Sale/Create
        public ActionResult Create()
        {
            ViewBag.Message = "Sale";

            return View();
        }

        // POST: Sale/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
