using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.Models
{
    public class Cart
    {
        public string URL { get; set; }
        public int ID { get; set; }
        public bool CheckoutReady { get; set; }
        public int User_Id { get; set; }
        public List<GamesForCart> Games { get; set; }

    }
}
