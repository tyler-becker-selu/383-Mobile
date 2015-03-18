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
    public class GetSalesDTO
    {

        public string URL { get; set; }
        public int Id { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal Total { get; set; }
        public GetCartDTO Cart { get; set; }


    }
}