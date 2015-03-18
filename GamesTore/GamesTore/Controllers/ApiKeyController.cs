using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Web.Http;
using GamesTore.Controllers;
using GamesTore.Models;
using System.Net.Http.Formatting;
using System.Data.Entity.Infrastructure;

namespace GamesToreAPI.Controllers
{
    public class ApiKeyController : BaseApiController
    {
    [HttpGet]
        public HttpResponseMessage GET()
        {
            
            var QueryString = Request.RequestUri.ParseQueryString();
            if (QueryString != null)
            {

                var email = QueryString["email"];
                var password = QueryString["password"];

                if (!(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)))
                {

                    var user = db.Users.FirstOrDefault(m => m.Email == email);

                    if (user != null)
                    {

                        if (Crypto.VerifyHashedPassword(user.Password, password))
                        {
                            try
                            {
                                user.ApiKey = GetApiKey();
                                db.Users.Attach(user);
                                var entry = db.Entry(user);
                                entry.State = EntityState.Modified;
                                entry.Property(e => e.Password).IsModified = false;
                                db.SaveChanges();
                            }catch(DbUpdateConcurrencyException)
                            {
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to save API key to database");
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(user.ApiKey, user.Id));
                        }

                        return Request.CreateResponse(HttpStatusCode.Forbidden, "Incorrect Password");

                    }

                    return Request.CreateResponse(HttpStatusCode.Forbidden, "Incorrect Email");

                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Email or Password Missing");

            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Email and Password Missing");

        }

        private string GetApiKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[16];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

    }
}
