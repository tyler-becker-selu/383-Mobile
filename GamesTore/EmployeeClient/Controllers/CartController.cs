using EmployeeClient.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EmployeeClient.Controllers
{

    public class CartController : BaseController
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

                    foreach (var game in item.Games)
                    {
                        game.m_Item1.Id = GetID(game.m_Item1.URL);
                    }
                }
            }

            return cartList;
        }

        // GET: Cart
        public ActionResult Index()
        {
            ViewBag.Message = "Cart";

            var cart = GetCart();
            return View(cart);
        }
    }
}