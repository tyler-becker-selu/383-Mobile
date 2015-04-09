using System;
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

    public class CartsController : BaseApiController
    {
        // GET api/Cart
        [HttpGet]
        public HttpResponseMessage GetCarts()
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                List<GetCartDTO> CartList = new List<GetCartDTO>();
                foreach (var item in db.Carts)
                {

                    CartList.Add(Factory.Create(item));

                }
                return Request.CreateResponse(HttpStatusCode.OK, CartList);
            }
            else if (IsAuthorized(Request, new List<Roles> { Roles.Employee }))
            {
                List<GetCartDTO> CartListE = new List<GetCartDTO>();
                foreach (var item in db.Carts)
                {
                    if (item.CheckoutReady) CartListE.Add(Factory.Create(item));
                }
                if (CartListE.Count < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No carts available for checkout");
                }
                return Request.CreateResponse(HttpStatusCode.OK, CartListE);
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // GET api/Cart/5
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var cart = db.Carts.FirstOrDefault(c => c.User_Id == id);
            if (cart == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Cannot find User's cart");

            }
            return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(cart));
        }

        [HttpPut]
        public HttpResponseMessage PutCartModel(int id, [FromBody]List<SetGameDTO> game)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {


                var original = db.Carts.FirstOrDefault(m => m.User_Id == id);

                if (original != null)
                {
                    foreach (var item in game)
                    {
                        GameModel ParsedGame = Factory.Parse(item);
                        var Game = db.Games.FirstOrDefault(m => m.GameName == ParsedGame.GameName);
                        if (Game == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Game could not be read");
                        }
                        var QueryString = Request.RequestUri.ParseQueryString();
                        if (!(QueryString.Count < 1))
                        {
                            var remove = QueryString["remove"];
                            if (remove == "true")
                            {
                                if (original.Games.Contains(Game))
                                {
                                    original.Games.Remove(Game);
                                    db.Entry(original).CurrentValues.SetValues(original);
                                }

                            }
                            else if (remove == "false")
                            {
                                original.Games.Add(Game);
                                db.Entry(original).CurrentValues.SetValues(original);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "You need to type true or false for remove in query string since I couldn't get it working with routes");
                            }


                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "You need to type true or false for remove in query string since I couldn't get it working with routes");

                        }
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Cart not found");
                }

                try
                {
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(original));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!CartModelExists(id))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, ex.Message);
                    }
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Game could not be read from body of request");
            }

        }

        // POST api/Cart
        [HttpPost]
        public HttpResponseMessage PostCartModel([FromBody]List<SetGameDTO> games)
        {
            if (ModelState.IsValid)
            {
                if (games.Count > 0)
                {
                    var headers = Request.Headers;
                    var userID = Convert.ToInt32(headers.Where(m => m.Key == "xcmps383authenticationid").First().Value.First());
                    if (!(db.Carts.Where(m => m.User_Id == userID).Count() > 0))
                    {
                        List<GameModel> gamelist = new List<GameModel>();
                        foreach (var item in games)
                        {
                            GameModel TheGame;
                            try
                            {
                                TheGame = Factory.Parse(item);
                            }
                            catch
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Coud not read one or more games from body of request");
                            }
                            var game = db.Games.FirstOrDefault(m => m.GameName == TheGame.GameName);
                            if (game == null)
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "One or more games could not be found");
                            }
                            gamelist.Add(game);
                        }
                        CartModel cart = new CartModel()
                                            {
                                                Games = gamelist,
                                                CheckoutReady = true,
                                                User_Id = userID
                                            };
                        try
                        {
                            db.Carts.Add(cart);
                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(cart));
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "User already has an active cart");
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Games not found");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Cart/5
        [HttpDelete]
        public HttpResponseMessage DeleteCartModel(int id)
        {
            if (IsAuthorized(Request, new List<Roles>() { Roles.Admin }))
            {
                CartModel cartmodel = db.Carts.Find(id);
                if (cartmodel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                db.Carts.Remove(cartmodel);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }

                return Request.CreateResponse(HttpStatusCode.OK, cartmodel);
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        private bool CartModelExists(int id)
        {
            return db.Carts.Count(e => e.Id == id) > 0;
        }
    }
}