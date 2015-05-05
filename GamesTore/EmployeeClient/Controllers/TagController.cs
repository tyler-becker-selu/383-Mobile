using EmployeeClient.Models;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EmployeeClient.Controllers
{
    [AuthController(AccessLevel = "Admin")]
    public class TagController : BaseController
    {
        
        #region Algorthms

        public List<Game> GetGamesForTag(string tagName)
        {
            List<Game> gameList = new List<Game>();

            gameList = getGamesTags(tagName);

            return gameList;
        }

        private  List<Game> getGamesTags(string tagName)
        {
            var request = new RestRequest("Games", Method.GET);
            var gameList = new List<Game>();

            APIHeaders(request);
            request.AddParameter("Tag", tagName);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                gameList = deserial.Deserialize<List<Game>>(APIresponse);
            }

            return gameList;
        }

        private dynamic getTagsforController()
        {
            var request = new RestRequest("Tags", Method.GET);
            var genreList = new List<Tag>();

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                genreList = deserial.Deserialize<List<Tag>>(APIresponse);
            }

            return genreList;
        }

        #endregion

        #region ViewControllers

        // GET: Game
        public ActionResult Index()
        {
            ViewBag.Message = "Tag";


            List<Tag> tagList = new List<Tag>();
            tagList = getTagsforController();

            foreach (Tag item in tagList)
            {
                item.Id = GetID(item.URL);
            }

            return View(tagList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Message = "Tag";


            return View();
        }

        [HttpPost]
        public ActionResult Create(Tag tag)
        {
            var request = new RestRequest("Tags/", Method.POST);

            APIHeaders(request);

            request.AddObject(tag);

            var APIresponse = client.Execute(request);


            if (APIresponse.StatusCode == HttpStatusCode.Created)
            {
                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            ViewBag.Message = "Tag";

            var request = new RestRequest("Tags/" + Id, Method.GET);

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                return View(deserial.Deserialize<Tag>(APIresponse));
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var request = new RestRequest("Tags/" + tag.Id, Method.PUT);

            APIHeaders(request);

            request.AddObject(tag);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Details(int Id)
        {
            ViewBag.Message = "Tag";

            var request = new RestRequest("Tags/" + Id, Method.GET);

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                var tag = _deserializer.Deserialize<Tag>(APIresponse);
                tag.Games = getGamesTags(tag.Name);

                return View(tag);

            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            ViewBag.Message = "Tag";

            var request = new RestRequest("Tags/" + Id, Method.GET);

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();
                return View(deserial.Deserialize<Tag>(APIresponse));
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Tag tag)
        {
            var request = new RestRequest("Tags/" + tag.Id, Method.DELETE);

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        #endregion

    }
}
