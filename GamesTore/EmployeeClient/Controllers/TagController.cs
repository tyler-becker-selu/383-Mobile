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
    public class TagController : Controller
    {
        private RestClient client = new RestClient("http://localhost:12932/");

        #region Algorthms

        public List<Game> GetGamesForTag(string tagName)
        {
            List<Game> gameList = new List<Game>();

            gameList = getGames(tagName);

            return gameList;
        }

        private List<Game> getGames(string tagName)
        {
            var request = new RestRequest("api/Games", Method.GET);
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


        private void APIHeaders(RestRequest request)
        {
            if (Session["ApiKey"] != null && Session["UserId"] != null)
            {
                request.AddHeader("xcmps383authenticationkey", Session["ApiKey"].ToString());
                request.AddHeader("xcmps383authenticationid", Session["UserId"].ToString());
            }
        }

        private int GetID(string p)
        {
            string[] x = p.Split('/');
            return Convert.ToInt32(x[x.Length - 1]);
        }


        private dynamic getTags()
        {
            var request = new RestRequest("api/Tags", Method.GET);
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
            List<Tag> tagList = new List<Tag>();
            tagList = getTags();

            foreach (Tag item in tagList)
            {
                item.Id = GetID(item.URL);
            }

            return View(tagList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Tag tag)
        {
            var request = new RestRequest("api/Tags/", Method.POST);

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
            var request = new RestRequest("api/Tags/" + Id, Method.GET);

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

            var request = new RestRequest("api/Tags/" + tag.Id, Method.PUT);

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
            var request = new RestRequest("api/Tags/" + Id, Method.GET);

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                var tag = deserial.Deserialize<Tag>(APIresponse);
                tag.Games = getGames(tag.Name);

                return View(tag);

            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var request = new RestRequest("api/Tags/" + Id, Method.GET);

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
            var request = new RestRequest("api/Tags/" + tag.Id, Method.DELETE);

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
