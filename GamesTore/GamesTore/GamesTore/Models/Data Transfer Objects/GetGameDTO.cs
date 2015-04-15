using GamesToreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamesTore.Models.Data_Transfer_Objects
{
    public class GetGameDTO
    {
        public string URL { get; set; }
        public string GameName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public int InventoryStock { get; set; }
        public virtual ICollection<GetGenreDTO> Genres { get; set; }
        public virtual ICollection<GetTagDTO> Tags { get; set; }

    }
}