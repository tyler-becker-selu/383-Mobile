using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamesToreAPI.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public bool CheckoutReady { get; set; }
        public int User_Id { get; set; }
        public virtual ICollection<CartGameQuantities> Games { get; set; }
    }
}