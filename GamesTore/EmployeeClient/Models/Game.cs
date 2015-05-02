using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient.Models
{
    public class Game
    {
        public string URL { get; set; }
        public int Id { get; set; }

        [DisplayName("Game Name")]
        public string GameName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        
        [DisplayName("Inventory Stock")]
        public int InventoryStock { get; set; }
        public virtual List<Genre> Genres { get; set; }
        public virtual List<Tag> Tags { get; set; }
    }
}
