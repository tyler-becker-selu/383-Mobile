using GamesTore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Routing;

namespace GamesTore.Models.Data_Transfer_Objects
{
    public class GetUserDTO
    {
        public string URL { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Roles Role { get; set; }
    }
}