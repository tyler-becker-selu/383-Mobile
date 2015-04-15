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
    public class TagsController : BaseApiController
    {

        // GET: api/Tag
        [HttpGet]
        public HttpResponseMessage GetTags()
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                List<GetTagDTO> TagList = new List<GetTagDTO>();
                foreach (var item in db.Tags)
                {

                    TagList.Add(Factory.Create(item));

                }
                return Request.CreateResponse(HttpStatusCode.OK, TagList);
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // GET: api/Tag/5
        [HttpGet]
        public HttpResponseMessage GetTagModel(int id)
        {
            if(IsAuthorized(Request, new List<Roles> {Roles.Admin}))
            {
                TagModel tagModel = db.Tags.Find(id);
                if (tagModel == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }

                return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(tagModel));
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // PUT: api/Tag/5
        [HttpPut]
        public HttpResponseMessage PutTagModel(int id, [FromBody]SetTagDTO tagModel)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }

                TagModel editedGenres = Factory.Parse(tagModel);

                editedGenres.Id = id;

                db.Entry(tagModel).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, tagModel);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TagModelExists(id))
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

        // POST: api/Tag
        [HttpPost]
        public HttpResponseMessage PostTagModel([FromBody]SetTagDTO tagModel)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                if (ModelState.IsValid)
                {
                    TagModel newTag = Factory.Parse(tagModel);
                    db.Tags.Add(newTag);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.Created, Factory.Create(newTag));

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        // DELETE: api/Tag/5
        [HttpDelete]
        public HttpResponseMessage DeleteTagModel(int id)
        {
            if (IsAuthorized(Request, new List<Roles> { Roles.Admin }))
            {
                TagModel tagModel = db.Tags.Find(id);
                if (tagModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                db.Tags.Remove(tagModel);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }

                return Request.CreateResponse(HttpStatusCode.OK, Factory.Create(tagModel));
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

        private bool TagModelExists(int id)
        {
            return db.Tags.Count(e => e.Id == id) > 0;
        }
    }
}