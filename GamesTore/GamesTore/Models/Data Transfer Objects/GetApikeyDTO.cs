using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamesTore.Models.Data_Transfer_Objects
{
    public class GetApikeyDTO
    {
        public string ApiKey { get; set; }
        public int UserId { get; set; }
    }
}