using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GamesToreAPI.Models;
using GamesToreAPI.Controllers;
using GamesTore.Models;
using System.Web.Helpers;
using GamesTore.Models.Data_Transfer_Objects;

namespace GamesTore.Controllers
{
    public class UsersController : BaseApiController
    {

        // GET: api/Users
        [HttpGet]
        public HttpResponseMessage GetUsers()
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                List<GetUserDTO> UserList = new List<GetUserDTO>();
                foreach(var item in db.Users)
                {

                    UserList.Add(Factory.Create(item));                    

                }
                return Request.CreateResponse(HttpStatusCode.OK, UserList);
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // GET: api/Users/5
        [HttpGet]
        public HttpResponseMessage GetUserModel(int id)
        {
            if(IsAuthorized(Request, new List<Roles> {Roles.Admin}))
            {
                UserModel userModel = db.Users.Find(id);
                if (userModel == null)
                {
                     throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }

                return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(userModel));
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // PUT: api/Users/5
        [HttpPut]
        public HttpResponseMessage PutUserModel(int id, [FromBody]SetUserDTO usermodel)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                if (usermodel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }

         
                var original = db.Users.Find(id);

                if (original != null)
                {
                    original.FirstName = usermodel.FirstName;
                    original.LastName = usermodel.LastName;
                    original.Email = usermodel.Email;
                    original.Role = usermodel.Role;
                    if (!string.IsNullOrWhiteSpace(usermodel.Password))
                    {
                        original.Password = Crypto.HashPassword(usermodel.Password);
                    }
                    db.Entry(original).CurrentValues.SetValues(original);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
                }
                
                try
                {
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(original));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!UserModelExists(id))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, ex.Message);
                    }
                }

            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // POST: api/Users
        [HttpPost]
        public HttpResponseMessage PostUserModel([FromBody]UserModel userModel)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (db.Users.Count(m => m.Email == userModel.Email) > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Email is already used");
                        }
                        userModel.Password = Crypto.HashPassword(userModel.Password);
                        db.Users.Add(userModel);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.Created, Factory.Create(userModel));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                    }
                    
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // DELETE: api/Users/5
        [HttpDelete]
        public HttpResponseMessage DeleteUserModel(int id)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                UserModel userModel = db.Users.Find(id);
                if (userModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                db.Users.Remove(userModel);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }

                return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(userModel));
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserModelExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}