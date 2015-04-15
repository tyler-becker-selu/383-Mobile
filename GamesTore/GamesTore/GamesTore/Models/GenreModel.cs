using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamesToreAPI.Models
{
    public class GenreModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<GameModel> Games { get; set; }

    }
}