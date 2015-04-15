﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using GamesToreAPI.Models;
using GamesToreAPI.Controllers;
using GamesTore.Models;
using System.Web.Helpers;
using GamesTore.Models.Data_Transfer_Objects;

namespace GamesTore.Controllers
{
    public class SalesController : BaseApiController
    {

        // GET api/Cart
        [HttpGet]
        public HttpResponseMessage GetSales()
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                List<GetSalesDTO> SalesList = new List<GetSalesDTO>();
                foreach (var item in db.Sales)
                {

                    SalesList.Add(Factory.Create(item));

                }
                if (SalesList.Count < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Bad business, no sales");
                }
                return Request.CreateResponse(HttpStatusCode.OK, SalesList);

            }
            else
            {
                var QueryString = Request.RequestUri.ParseQueryString();
                if (!(QueryString.Count < 1))
                {
                    var employee = QueryString["empId"];
                    var user = QueryString["userid"];
                    if (employee != null)
                    {
                        var empId = int.Parse(employee);
                        List<GetSalesDTO> EmployeeSaleList = new List<GetSalesDTO>();
                        foreach (var item in db.Sales)
                        {

                            if (item.EmployeeId == empId)
                            {
                                EmployeeSaleList.Add(Factory.Create(item));
                            }
                        }
                        if (EmployeeSaleList.Count < 1)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "No sales made by employee");
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, EmployeeSaleList);
                    }
                    else if (user != null)
                    {
                        var userId = int.Parse(user);
                        List<GetSalesDTO> UserSaleList = new List<GetSalesDTO>();
                        foreach (var item in db.Sales)
                        {
                            if (item.User.Id == userId)
                            {
                                UserSaleList.Add(Factory.Create(item));
                            }
                        }
                        if (UserSaleList.Count < 1)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "No sales present for user");
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, UserSaleList);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User Id not found");
            }
        }

        [HttpGet]
        [ActionName("User")]
        public HttpResponseMessage GetUsersSales(int id, int saleid)
        {
            var userID = Convert.ToInt32(Request.Headers.Where(m => m.Key == "xcmps383authenticationid").First().Value.First());
            if (id != userID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error reading which user is logged in");
            }
            SalesModel sale = db.Sales.FirstOrDefault(m => m.Id == saleid);
            if (sale == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No sale found for user");
            }
            if (sale.User.Id != id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Sale found belongs to different user");
            }
            return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(sale));
        }

        // GET: api/SalesModels/5
        [HttpGet]
        public HttpResponseMessage GetSalesModel(int id)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                var sale = db.Sales.FirstOrDefault(u => u.Id == id);
                if (sale == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sale cannot be found");
                }

                return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(sale));
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);

        }

        // PUT api/Sales/5
        [HttpPut]
        public HttpResponseMessage PutSalesModel(int id, [FromBody]SetSalesDTO editedSale)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }

                var original = db.Sales.FirstOrDefault(m => m.Id == id);

                if (original != null)
                {
                    original.SalesDate = editedSale.SalesDate;
                    //TODO Fix editing cart in editing sale
                    CartModel ParsedCart = Factory.Parse(editedSale.Cart);
                    var TempGList = original.Cart.Games.ToList();
                    foreach (var item in TempGList)
                    {
                        item.Game.InventoryStock += item.Quantity;
                        original.Cart.Games.Remove(item);
                        db.SaveChanges();
                    }
                    foreach (var item in ParsedCart.Games)
                    {
                        GameModel game = db.Games.FirstOrDefault(m => m.GameName == item.Game.GameName);
                        original.Cart.Games.Add(new CartGameQuantities() { Game = game, Cart = original.Cart, Quantity = item.Quantity });
                    }

                    foreach (var item in original.Cart.Games)
                    {
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            original.Total += item.Game.Price;
                            item.Game.InventoryStock--;
                            db.Entry(item.Game).CurrentValues.SetValues(item.Game);
                            db.SaveChanges();
                        }
                    }
                    
                    db.Entry(original).CurrentValues.SetValues(original);
                    try
                    {
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(original));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (!SalesModelExists(id))
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NoContent, ex.Message);
                        }
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sale not found");
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        private bool SalesModelExists(int id)
        {
            throw new NotImplementedException();
        }


        // POST api/Sales
        [HttpPost]
        public HttpResponseMessage PostSale([FromBody]SetCartDTO cart)
        {
            if (IsAuthorized(Request, new List<Roles>() { Roles.Admin, Roles.Employee }))
            {
                if (ModelState.IsValid)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var employId = Convert.ToInt32(Request.Headers.Where(m => m.Key == "xcmps383authenticationid").First().Value.First());

                            var checkout = db.Carts.FirstOrDefault(m => m.User_Id == cart.User_Id && m.CheckoutReady == true);
                            if (checkout == null)
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find cart");
                            }
                            SalesModel newSale = new SalesModel()
                            {
                                SalesDate = DateTime.Now,
                                Cart = checkout,
                                User = db.Users.FirstOrDefault(m => m.Id == checkout.User_Id),
                                EmployeeId = employId
                            };
                            foreach (var item in checkout.Games)
                            {
                                for (int i = 0; i < item.Quantity; i++)
                                {
                                    newSale.Total += item.Game.Price;
                                    item.Game.InventoryStock--;
                                    db.Entry(item.Game).CurrentValues.SetValues(item.Game);
                                    db.SaveChanges();
                                }
                            }
                            try
                            {
                                checkout.CheckoutReady = false;
                                db.Entry(checkout).CurrentValues.SetValues(checkout);
                                db.SaveChanges();
                            }
                            catch (DbUpdateConcurrencyException ex)
                            {
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                            }
                            try
                            {
                                db.Sales.Add(newSale);
                                db.SaveChanges();
                                transaction.Commit();
                                return Request.CreateResponse(HttpStatusCode.Created, cart);
                            }
                            catch (DbUpdateConcurrencyException ex)
                            {
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                            }
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            return Request.CreateResponse(HttpStatusCode.BadRequest, e);
                        }

                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // DELETE api/Sales/5
        [HttpDelete]
        public HttpResponseMessage DeleteSalesModel(int id)
        {
            if (IsAuthorized(Request, new List<Roles>() { Roles.Admin }))
            {
                SalesModel Salesmodel = db.Sales.Find(id);
                if (Salesmodel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                db.Sales.Remove(Salesmodel);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }

                return Request.CreateResponse(HttpStatusCode.OK, Salesmodel);
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        private bool SaleModelExists(int id)
        {
            return db.Sales.Count(e => e.Id == id) > 0;
        }


    }
}