namespace GamesTore.Migrations
{
    using GamesTore.Models;
    using GamesToreAPI.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Helpers;

    internal sealed class Configuration : DbMigrationsConfiguration<GamesToreAPI.Models.ApiDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GamesToreAPI.Models.ApiDbContext context)
        {

            try
            {
                string HashedStoreAdminPassword = Crypto.HashPassword("selu2015");
                string HashedStoreEmployerPassword = Crypto.HashPassword("password");
                string HashedCustomerPassword = Crypto.HashPassword("password");

                var users = new List<UserModel> {
                new UserModel { Email = "383@envoc.com", FirstName = "Admin", LastName = "Istrator", Role = Roles.Admin, ApiKey = null, Password = HashedStoreAdminPassword },
                new UserModel { Email = "383User@selu.edu", FirstName = "Micheal", LastName = "Ballmann", Role = Roles.Employee, ApiKey = null, Password = HashedStoreEmployerPassword },
                new UserModel { Email = "383Customer@selu.edu", FirstName = "Shane", LastName = "Cao", Role = Roles.User, ApiKey = null, Password = HashedCustomerPassword }
            };

                users.ForEach(s => context.Users.AddOrUpdate(u => u.Email, s));
                context.SaveChanges();



                var genres = new List<GenreModel>
            {
                new GenreModel { Id = 1, Name = "Action", Games = new List<GameModel>()},
                new GenreModel { Id = 2, Name = "Adventure", Games = new List<GameModel>()},
                new GenreModel { Id = 3, Name = "Role-Playing", Games = new List<GameModel>()},
                new GenreModel { Id = 4, Name = "First-Person Shooter", Games = new List<GameModel>()},
                new GenreModel { Id = 5, Name = "Fantasy", Games = new List<GameModel>()},
                new GenreModel { Id = 6, Name = "Third-Person Shooter", Games = new List<GameModel>()},
                new GenreModel { Id = 7, Name = "Sports", Games = new List<GameModel>()},
                new GenreModel { Id = 8, Name = "Strategy", Games = new List<GameModel>()},
                new GenreModel { Id = 9, Name = "MMO", Games = new List<GameModel>()},
                new GenreModel { Id = 10, Name = "Fighting", Games = new List<GameModel>()},
 
            };
                genres.ForEach(g => context.Genres.AddOrUpdate(p => p.Name, g));
                context.SaveChanges();

                var tags = new List<TagModel>
            {
                new TagModel { Id = 1, Name = "Hard", Games = new List<GameModel>()},
                new TagModel { Id = 2, Name = "Open World", Games = new List<GameModel>()},
                new TagModel { Id = 3, Name = "Walking Simulator", Games = new List<GameModel>()},
                new TagModel { Id = 4, Name = "Nanomachines Son", Games = new List<GameModel>()},
                new TagModel { Id = 5, Name = "Dark Fantasy", Games = new List<GameModel>()},
                new TagModel { Id = 6, Name = "Get Wrekt Simulator", Games = new List<GameModel>()},
                new TagModel { Id = 7, Name = "Sandbox", Games = new List<GameModel>()},
                new TagModel { Id = 8, Name = "Soccer", Games = new List<GameModel>()},
                new TagModel { Id = 9, Name = "FTW", Games = new List<GameModel>()},
                new TagModel { Id = 10, Name = "Get Ulted", Games = new List<GameModel>()},
 
            };
                tags.ForEach(g => context.Tags.AddOrUpdate(p => p.Name, g));
                context.SaveChanges();

                var games = new List<GameModel>
            {
                new GameModel 
                { 
                    Id = 1, 
                    GameName = "Destiny",   
                    ReleaseDate = DateTime.Parse("09/09/2014"), 
                    Price = 59.99M, 
                    InventoryStock = 10,
                    Genres = new List<GenreModel>
                    {
                        context.Genres.FirstOrDefault(m => m.Name == "Action"),
                        context.Genres.FirstOrDefault(m => m.Name == "First-Person Shooter"),
                        context.Genres.FirstOrDefault(m => m.Name == "MMO")
                    },
                    Tags = new List<TagModel>
                    {
                        context.Tags.FirstOrDefault(m => m.Name == "Hard"),
                        context.Tags.FirstOrDefault(m => m.Name == "Walking Simulator"),
                        context.Tags.FirstOrDefault(m => m.Name == "Get Wrekt Simulator")
                    }
                },
                new GameModel 
                { 
                    Id = 2, 
                    GameName = "Fifa 2015",   
                    ReleaseDate = DateTime.Parse("09/23/2014"), 
                    Price = 59.99M, 
                    InventoryStock = 10,
                    Genres = new List<GenreModel>
                    {
                        context.Genres.FirstOrDefault(m => m.Name == "Sports"),
                        context.Genres.FirstOrDefault(m => m.Name == "Role-Playing"),
                        context.Genres.FirstOrDefault(m => m.Name == "Strategy")
                    },
                    Tags = new List<TagModel>
                    {
                        context.Tags.FirstOrDefault(m => m.Name == "Hard"),
                        context.Tags.FirstOrDefault(m => m.Name == "Soccer"),
                        context.Tags.FirstOrDefault(m => m.Name == "Get Wrekt Simulator")
                    }
                },
                new GameModel 
                { Id = 3, 
                    GameName = "League Of Legends",   
                    ReleaseDate = DateTime.Parse("10/27/2009"), 
                    Price = 0.00M, 
                    InventoryStock = 99999,
                    Genres = new List<GenreModel>
                    {
                        context.Genres.FirstOrDefault(m => m.Name == "Action"),
                        context.Genres.FirstOrDefault(m => m.Name == "Role-Playing"),
                        context.Genres.FirstOrDefault(m => m.Name == "Fantasy")
                    },
                    Tags = new List<TagModel>
                    {
                        context.Tags.FirstOrDefault(m => m.Name == "Hard"),
                        context.Tags.FirstOrDefault(m => m.Name == "Get Ulted"),
                        context.Tags.FirstOrDefault(m => m.Name == "Get Wrekt Simulator")
                    }
                }
 
            };
                foreach (var item in games)
                {
                    foreach (var genre in item.Genres)
                    {
                        genre.Games.Add(item);
                        context.Entry(genre).CurrentValues.SetValues(genre);
                    }

                    foreach (var tag in item.Tags)
                    {
                        tag.Games.Add(item);
                        context.Entry(tag).CurrentValues.SetValues(tag);
                    }

                }
                games.ForEach(g => context.Games.AddOrUpdate(p => p.GameName, g));
                context.SaveChanges();

                var cart = new CartModel()
                {
                    CheckoutReady = true,
                    User_Id = 3
                };
                var ActionGenre = context.Genres.FirstOrDefault(m => m.Name == "Action");
                List<CartGameQuantities> Games = new List<CartGameQuantities>();
                foreach(var item in ActionGenre.Games)
                {
                    Games.Add(new CartGameQuantities() {Cart = cart, Game = item, Quantity = 1 });
                }
                cart.Games = Games;
                context.Carts.Add(cart);
                context.SaveChanges();

                var soldcart = new CartModel()
                {
                    CheckoutReady = false,
                    User_Id = 3
                };
                var SportsGenre = context.Genres.FirstOrDefault(m => m.Name == "Sports");
                List<CartGameQuantities> SoldGames = new List<CartGameQuantities>();
                foreach (var item in SportsGenre.Games)
                {
                    SoldGames.Add(new CartGameQuantities() { Cart = soldcart, Game = item, Quantity = 1 });
                }
                soldcart.Games = SoldGames;
                context.Carts.Add(soldcart);
                context.SaveChanges();

                var sale = new SalesModel()
                {
                    SalesDate = DateTime.Now,
                    EmployeeId = 2,
                    Cart = context.Carts.FirstOrDefault(m => m.CheckoutReady == false),
                };
                foreach (var item in sale.Cart.Games)
                {
                    sale.Total += item.Game.Price;
                }
                sale.User = context.Users.FirstOrDefault(m => m.Id == sale.Cart.User_Id);

                context.Sales.Add(sale);
                context.SaveChanges();

            }
            catch
            (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new Exception(sb.ToString());
            }

        }
    }
}