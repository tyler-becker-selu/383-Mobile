using GamesTore.Models.Data_Transfer_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.Models.ViewModels
{
    public class SaleIndexViewModel:Sale
    {
        public GetUserDTO User { get; set; }
    }
}
