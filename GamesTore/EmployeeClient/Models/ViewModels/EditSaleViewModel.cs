using EmployeeClient.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.ViewModels
{
    public class EditSaleViewModel
    {
        public int Id{get;set;}
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Release Date")]
        public DateTime SaleDate{get;set;}
        public decimal Amount { get; set; }
        public List<SaleGameQuaninty> Games {get;set;}
    }
}
