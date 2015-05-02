using EmployeeClient.Models;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EmployeeClient.Controllers
{
    public class Access : BaseController
    {
        public bool isAuth()
        {
           return isLoggedIn();
        }

        /// <summary>
        /// Get the Role for the user
        /// </summary>
        /// <param name="cUser"></param>
        /// <returns></returns>
        public string GetUserRights()
        {
            var role = string.Empty;

            if (System.Web.HttpContext.Current.Session["Role"] != null)
                return System.Web.HttpContext.Current.Session["Role"].ToString();


            if (Admin())
            {
                System.Web.HttpContext.Current.Session["Role"] = "Admin";
                role = "Admin";
            }
            else
            {
                System.Web.HttpContext.Current.Session["Role"] = "Employee";
                role = "Employee";
            }

            return role;
        }

        private bool Admin()
        {
            var request = new RestRequest("Users", Method.GET);

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }
    }

    public class AuthController : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var auth = new Access();
            var isAuthorized = auth.isAuth();

            if (!isAuthorized)
            {
                return false;
            }

            string privilegeLevels = string.Join("", auth.GetUserRights());

            if (privilegeLevels.Contains(this.AccessLevel) || privilegeLevels.Equals("Admin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Home",
                                action = "Login"
                            })
                        );
        }
    }
}
