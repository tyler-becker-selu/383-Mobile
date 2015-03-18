using GamesToreAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
namespace GamesTore.Models.Data_Transfer_Objects
{
    public class GetCartDTO
    {

        public string URL { get; set; }
        public int Id { get; set; }
        public bool CheckoutReady { get; set; }
        public int User_Id { get; set; }
        public List<GetGameDTO> Games { get; set; }

    }
}