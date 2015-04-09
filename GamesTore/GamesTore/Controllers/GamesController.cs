using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using GamesToreAPI.Models;
using GamesToreAPI.Controllers;
using GamesTore.Models;
using GamesTore.Models.Data_Transfer_Objects;

namespace GamesTore.Controllers
{
    public class GamesController : BaseApiController
    {

        // GET api/Games
        [HttpGet]
        public HttpResponseMessage GetGameModels()
        {
            var QString = Request.RequestUri.ParseQueryString();
            if (QString != null)
            {
                var genre = QString["genre"];
                var tag = QString["tag"];
                if (genre != null)
                {
                    if (!(genre is string) || string.IsNullOrWhiteSpace(genre))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Error: Genre not found");
                    }
                    GenreModel TheGenre = db.Genres.FirstOrDefault(m => m.Name == genre);
                    List<GetGameDTO> GameListByGenre = new List<GetGameDTO>();
                    foreach (var item in db.Games)
                    {
                        if (item.Genres.Contains(TheGenre))
                        {
                            GameListByGenre.Add(Factory.Create(item));
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, GameListByGenre);
                }
                else if (tag != null)
                {
                    if (!(tag is string) || string.IsNullOrWhiteSpace(tag))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Error: Tag not found");
                    }
                    TagModel TheTag = db.Tags.FirstOrDefault(m => m.Name == tag);
                    List<GetGameDTO> GameListByTag = new List<GetGameDTO>();
                    foreach (var item in db.Games)
                    {
                        if (item.Tags.Contains(TheTag))
                        {
                            GameListByTag.Add(Factory.Create(item));
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, GameListByTag);
                }

            }
            List<GetGameDTO> GameList = new List<GetGameDTO>();
            foreach (var item in db.Games)
            {

                GameList.Add(Factory.Create(item));

            }
            return Request.CreateResponse(HttpStatusCode.OK, GameList);

        }

        // GET api/Games/5
        [HttpGet]
        public HttpResponseMessage GetGameModel(int id)
        {
            GameModel gamemodel = db.Games.Find(id);
            if (gamemodel == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(gamemodel));
        }

        // PUT api/Games/5
        [HttpPut]
        public HttpResponseMessage PutGameModel(int id, [FromBody]SetGameDTO gamemodel)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var gameInfo = db.Games.Find(id);
                        gameInfo.GameName = gamemodel.GameName;
                        gameInfo.ReleaseDate = gamemodel.ReleaseDate;
                        gameInfo.Price = gamemodel.Price;
                        gameInfo.InventoryStock = gamemodel.InventoryStock;
                        var TempGList = gameInfo.Genres.ToList();
                        var TempTList = gameInfo.Tags.ToList();
                        foreach (var genre in TempGList)
                        {
                            var tog = db.Genres.FirstOrDefault(g => g.Name == genre.Name);
                            tog.Games.Remove(gameInfo);
                            db.SaveChanges();
                        }
                        foreach (var tag in TempTList)
                        {
                            var myGeglibnre = db.Tags.FirstOrDefault(g => g.Name == tag.Name);
                            myGeglibnre.Games.Remove(gameInfo);
                            db.SaveChanges();
                        }
                        
                        if (gamemodel.Genres.Count() > 0)
                        {
                            var genreList = new List<GenreModel>();
                            foreach (var genre in gamemodel.Genres)
                            {
                                if (!db.Genres.Any(g => g.Name == genre.Name))
                                {
                                    db.Genres.Add(Factory.Parse(genre));
                                    db.SaveChanges();
                                }
                                var myGenre = db.Genres.FirstOrDefault(g => g.Name == genre.Name);
                                genreList.Add(myGenre);
                            }
                            gameInfo.Genres = genreList;
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Game must have one Genre");
                        }

                        if (gamemodel.Tags.Count() > 0)
                        {
                            var tagList = new List<TagModel>();
                            foreach (var tag in gamemodel.Tags)
                            {
                                if (!db.Tags.Any(g => g.Name == tag.Name))
                                {
                                    db.Tags.Add(Factory.Parse(tag));
                                    db.SaveChanges();
                                }
                                var myTag = db.Tags.FirstOrDefault(g => g.Name == tag.Name);
                                tagList.Add(myTag);
                            }
                            gameInfo.Tags = tagList;
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Game must have one Tag");
                        }

                        db.Entry(gameInfo).CurrentValues.SetValues(gameInfo);

                        try
                        {
                            db.SaveChanges();

                            transaction.Commit();

                            return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(gameInfo));
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            if (!GameModelExists(id))
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NoContent, ex.Message);
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
                    }
                }

            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // POST api/Games
        [HttpPost]
        public HttpResponseMessage PostGameModel([FromBody]SetGameDTO gamemodel)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                if (ModelState.IsValid)
                {
                    if (db.Games.Count(g => g.GameName == gamemodel.GameName) > 0)
                    {

                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Game already exist.");
                    }

                    GameModel newGame = Factory.Parse(gamemodel);

                    if (newGame.Genres == null || newGame.Tags == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Games must contain one genre and one tag");
                    }

                    var genreList = new List<GenreModel>();
                    foreach (var genre in newGame.Genres)
                    {
                        if (db.Genres.Any(g => g.Name == genre.Name))
                        {
                            var myGenre = db.Genres.FirstOrDefault(g => g.Name == genre.Name);
                            genreList.Add(myGenre);
                        }
                        else
                        {
                            genreList.Add(genre);
                        }
                    }
                    newGame.Genres = genreList;

                    var tagList = new List<TagModel>();
                    foreach (var tag in newGame.Tags)
                    {
                        if (db.Tags.Any(g => g.Name == tag.Name))
                        {
                            var myTag = db.Tags.FirstOrDefault(g => g.Name == tag.Name);
                            tagList.Add(myTag);
                        }
                        else
                        {
                            tagList.Add(tag);
                        }
                    }
                    newGame.Tags = tagList;

                    db.Games.Add(newGame);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.Created, Factory.Create(newGame));
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // DELETE api/Games/5
        [HttpDelete]
        public HttpResponseMessage DeleteGameModel(int id)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                GameModel gamemodel = db.Games.Find(id);
                if (gamemodel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                db.Games.Remove(gamemodel);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }

                return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(gamemodel));
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private bool GameModelExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}