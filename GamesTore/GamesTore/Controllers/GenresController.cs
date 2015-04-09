using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GamesToreAPI.Models;
using GamesToreAPI.Controllers;
using GamesTore.Models;
using GamesTore.Models.Data_Transfer_Objects;

namespace GamesTore.Controllers
{
    public class GenresController : BaseApiController
    {

        // GET: api/Genre
        [HttpGet]
        public HttpResponseMessage GetGenres()
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                List<GetGenreDTO> GenresList = new List<GetGenreDTO>();
                foreach (var item in db.Genres)
                {

                    GenresList.Add(Factory.Create(item));

                }
                return Request.CreateResponse(HttpStatusCode.OK, GenresList);
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // GET: api/Genre/5
        [HttpGet]
        public HttpResponseMessage GetGenreModel(int id)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                GenreModel genremodel = db.Genres.Find(id);
                if (genremodel == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }

                return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(genremodel));
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // PUT: api/Genre/5
        [HttpPut]
        public HttpResponseMessage PutGenreModel(int id, [FromBody]SetGenreDTO genreModel)
        {
            if(IsAuthorized(Request, new List<Roles> {Roles.Admin}))
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }

                GenreModel editedGenres = Factory.Parse(genreModel);

                editedGenres.Id = id;

                db.Entry(genreModel).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, genreModel);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!GenreModelExists(id))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, ex.Message);
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
            
        }

        // POST: api/Genre
        [HttpPost]
        public HttpResponseMessage PostGenreModel([FromBody]SetGenreDTO genreModel)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                if (ModelState.IsValid)
                {
                    GenreModel newGenre = Factory.Parse(genreModel);
                    db.Genres.Add(newGenre);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.Created, Factory.Create(newGenre));
                    
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);

        }

        // DELETE: api/Genre/5
        [HttpDelete]
        public HttpResponseMessage DeleteGenreModel(int id)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                GenreModel genreModel = db.Genres.Find(id);
                if (genreModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                db.Genres.Remove(genreModel);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }

                return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(genreModel));
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GenreModelExists(int id)
        {
            return db.Genres.Count(e => e.Id == id) > 0;
        }
    }
}