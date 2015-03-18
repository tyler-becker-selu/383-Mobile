using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamesToreAPI.Models
{
    public class GameModel
    {
        [Key]
        public int Id { get; set; }
        public string GameName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public int InventoryStock { get; set; }

        public virtual ICollection<TagModel> Tags { get; set; }
        public virtual ICollection<CartModel> Carts { get; set; }
        public virtual ICollection<GenreModel> Genres { get; set; }

    }
}