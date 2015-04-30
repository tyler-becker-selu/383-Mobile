using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.Models
{
    class Sale
    {
        public string URL { get; set; }
        public int ID { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal Total { get; set; }
        public Cart Cart { get; set; }

    }
}
