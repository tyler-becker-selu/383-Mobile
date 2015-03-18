using GamesTore.Models;
using GamesTore.Models.Data_Transfer_Objects;
using GamesToreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GamesToreAPI.Controllers
{
    public abstract class BaseApiController : ApiController
    {

        public ApiDbContext db = new ApiDbContext();

        DTOFactory _factory;

        protected bool IsAuthorized(HttpRequestMessage request, List<Roles> role)
        {

            int userID = Convert.ToInt32(request.Headers.Where(m => m.Key == "xcmps383authenticationid").FirstOrDefault().Value.FirstOrDefault());

            UserModel user = db.Users.Find(userID);

            return (role.Contains(user.Role));

        }

        protected DTOFactory Factory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = new DTOFactory(this.Request);
                }
                return _factory;
            }
        }

    }
}
