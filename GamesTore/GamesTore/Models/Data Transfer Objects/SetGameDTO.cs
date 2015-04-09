using GamesToreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamesTore.Models.Data_Transfer_Objects
{
    public class SetGameDTO
    {
        public string GameName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public int InventoryStock { get; set; }
        public virtual ICollection<SetGenreDTO> Genres { get; set; }
        public virtual ICollection<SetTagDTO> Tags { get; set; }
    }
}