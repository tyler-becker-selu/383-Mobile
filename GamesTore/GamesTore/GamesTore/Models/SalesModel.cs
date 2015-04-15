using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamesToreAPI.Models
{
    public class SalesModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal Total { get; set; }
        public int EmployeeId { get; set; }
        public virtual CartModel Cart { get; set; }
        public virtual UserModel User { get; set; }
    }
}