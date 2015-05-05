using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.Models
{
    public class Sale
    {
        public string URL { get; set; }
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Sale Date")]
        public DateTime SalesDate { get; set; }
        public decimal Total { get; set; }
        public Cart Cart { get; set; }
        public int EmployeeID { get; set; }

    }
}
