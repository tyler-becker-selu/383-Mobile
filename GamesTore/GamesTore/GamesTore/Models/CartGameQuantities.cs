using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GamesToreAPI.Models
{
    public class CartGameQuantities
    {
        [Key]
        public int ID { get; set; }
        public virtual CartModel Cart { get; set; }
        public virtual GameModel Game { get; set; }
        public int Quantity { get; set; }
    }
}