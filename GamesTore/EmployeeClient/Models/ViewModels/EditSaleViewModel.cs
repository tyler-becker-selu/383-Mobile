using EmployeeClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.ViewModels
{
    public class EditSaleViewModel
    {
        public int Id{get;set;}
        public DateTime SaleDate{get;set;}
        public decimal Amount { get; set; }
        public List<SaleGameQuaninty> Games {get;set;}
    }
}
