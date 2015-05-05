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

        public ActionResult SalesPDF()
        {
            List<SaleIndexViewModel> sales = GetSale();

            foreach (var item in sales)
            {
                item.ID = GetID(item.URL);
                item.Cart.ID = GetID(item.Cart.URL);
                item.User = GetUser(item.Cart.User_Id);
                if (item.User != null)
                {
                    Session["User"] = true;
                }
                else
                {
                    Session["User"] = null;
                }
            }

            return new Rotativa.ViewAsPdf("SaleListPDF", sales);
        }

        public List<SaleIndexViewModel> GetSale()
        {
            var request = new RestRequest("Sales", Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            APIHeaders(request);

            request.AddQueryParameter("empId", Session["UserID"].ToString());
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
                if (item.User != null)
                {
                    Session["User"] = true;
                }
                else
                {
                    Session["User"] = null;
                }
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
            return null;
        }


        [HttpGet]
        public ActionResult CartForSale(int id)
        {
            ViewBag.Message = "Sale";

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

                        var redirect = new UrlHelper(Request.RequestContext).Action("Index", "Sales");

                        return Json(new { Url = redirect });
                    }
                }

                throw new Exception("Couldn't make sale");
            }
            catch
            {
                ModelState.AddModelError("", "Could not create a sale.");
                var redirect = new UrlHelper(Request.RequestContext).Action("Index", "Home");
                return Json(new { Url = redirect });
            }
        }

        // GET: Sale/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Message = "Sale";

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


                    sales.SalesDate = sale.SaleDate;
                    sales.Total = sale.Amount;
                    var list = new List<GamesForCart>();
                    var y = new GamesForCart();

                    foreach (var game in sales.Cart.Games)
                    {
                        game.m_Item1.Id = GetID(game.m_Item1.URL);
                    }

                    foreach (var item in sale.Games)
                    {

                        y = sales.Cart.Games.FirstOrDefault(x => x.m_Item1.Id == item.GameID);

                        if (y != null)
                        {
                            y.m_Item2 = item.Quaninity;

                            list.Add(y);
                        }
                    }
                    sales.Cart.Games = list;




                    request = new RestRequest("Sales/" + sale.Id, Method.PUT);
                    request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

                    APIHeaders(request);

                    request.AddBody(sales);

                    var response = client.Execute(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var redirect = new UrlHelper(Request.RequestContext).Action("Index", "Sale");
                        return Json(new { Url = redirect });
                    }
                }
                var redirects = new UrlHelper(Request.RequestContext).Action("Edit/" + sale.Id, "Sale");
                return Json(new { Url = redirects });
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return View();
            }
        }

       
    }
}