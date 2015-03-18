using GamesTore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GamesToreAPI.Models
{
    public class ApiDbContext : DbContext
    {

        public ApiDbContext() : base("DefaultConnection")
        {
        }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<GameModel> Games { get; set; }
        public DbSet<GenreModel> Genres { get; set; }
        public DbSet<SalesModel> Sales { get; set; }
        public DbSet<TagModel> Tags { get; set; }
        public DbSet<UserModel> Users { get; set; }
        //public DbSet<GenreModelGameModel> GenreModelGameModels { get; set;  }
        //public DbSet<TagModelGameModel> TagModelGameModels { get; set; }
        //public DbSet<CartModelGameModel> CartModelGameModels { get; set; }


        public IEnumerable<object> Cart { get; set; }
    }
}