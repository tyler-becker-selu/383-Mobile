using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.Models.ViewModels
{
    public class EditGameViewModel : Game
    {
        public List<Genre> dbGenres { get; set; }
        public List<Tag> dbTags { get; set; }
    }
}
