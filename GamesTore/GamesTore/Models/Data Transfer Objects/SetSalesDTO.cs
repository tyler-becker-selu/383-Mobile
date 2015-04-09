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
    public class SetSalesDTO
    {
        public DateTime SalesDate { get; set; }
        public SetCartDTO Cart { get; set; }
    }
}