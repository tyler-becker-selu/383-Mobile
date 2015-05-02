using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using RestSharp;
using RestSharp.Deserializers;
using EmployeeClient.Models;
namespace EmployeeClient.Controllers
{
    [AuthController(AccessLevel="Admin")]
    public class GameController : BaseController
    {
     
        #region Dummy Data
        private void AddDummyTags(Game game)
        {
            Tag tag = new Tag();
            List<Tag> wee = new List<Tag>();
            tag.Id = 1;
            tag.Name = "Hard";
            wee.Add(tag);
            game.Tags = wee;

        }

        private void AddDummyGenre(Game game)
        {
            Genre genre = new Genre();
            List<Genre> wee = new List<Genre>();
            genre.Id = 1;
            genre.Name = "Action";

            wee.Add(genre);
            game.Genres = wee;

        }
        #endregion

        #region ViewControllers

        // GET: Game
        public ActionResult Index()
        {

            var gameList = new List<Game>();

            gameList = getGames();

            return View(gameList);
        }

        [HttpGet]
        public ActionResult Create()
        {

            Game game = new Game();
            game.Genres = getGenres();
            game.Tags = getTags();

            return View(game);
        }

        [HttpPost]
        public ActionResult Create(Game game)
        {
            var request = new RestRequest("api/Games/", Method.POST);
            request.RequestFormat = DataFormat.Json;
            APIHeaders(request);

            request.AddBody(game);

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
            var request = new RestRequest("api/Games/" + Id, Method.GET);

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                var genreList = new List<Genre>();
                var tagList = new List<Tag>();

                genreList = getGenres();
                tagList = getTags();

                ViewBag.Tags = tagList;
                ViewBag.Genres = genreList;

                return View(deserial.Deserialize<Game>(APIresponse));
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Game game)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var request = new RestRequest("api/Games/" + game.Id, Method.PUT);

            APIHeaders(request);

            //////////////////////////////////Dummy Data (for now)/////////////////////////////////////
            AddDummyGenre(game);
            AddDummyTags(game);
            ///////////////////////////////////////////////////////////////////////////////////////////

            request.AddObject(game);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Details(int Id)
        {
            var request = new RestRequest("api/Games/" + Id, Method.GET);

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                var game = deserial.Deserialize<Game>(APIresponse);
                game.Id = GetID(game.URL);

                return View(game);

            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var request = new RestRequest("api/Games/" + Id, Method.GET);

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();
                return View(deserial.Deserialize<Game>(APIresponse));
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Game game)
        {
            var request = new RestRequest("api/Games/" + game.Id, Method.DELETE);

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