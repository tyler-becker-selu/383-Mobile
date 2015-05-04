using GamesToreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace GamesTore.AuthenticationStuff
{
    public class GamesToreAuth : DelegatingHandler
    {

       ApiDbContext db = new ApiDbContext();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        { 
            db = new ApiDbContext();
            if (!ValidateApiKey(request))
            {

                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;

            }

            return base.SendAsync(request, cancellationToken);
        }

        private bool ValidateApiKey(HttpRequestMessage request)
        {
            db = new ApiDbContext();
            var headers = request.Headers;

            if (headers.Contains("xcmps383authenticationkey") && headers.Contains("xcmps383authenticationid"))
            {
                var apiKey = headers.Where(m => m.Key == "xcmps383authenticationkey").FirstOrDefault().Value.FirstOrDefault();
                var userID = Convert.ToInt32(headers.Where(m => m.Key == "xcmps383authenticationid").First().Value.First());
                var user = db.Users.FirstOrDefault(m => m.Id == userID);
                return (user != null && user.ApiKey == apiKey);
            }
            return false;
        }

    }
}