using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using RestSharp;
using RestSharp.Deserializers;
using EmployeeClient.Models;
using EmployeeClient.Models.ViewModels;
namespace EmployeeClient.Controllers
{
    [AuthController(AccessLevel="Employee")]
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
            var request = new RestRequest("Games/", Method.POST);
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
            var request = new RestRequest("Games/" + Id, Method.GET);

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                var game = _deserializer.Deserialize<EditGameViewModel>(APIresponse);

                game.Id = GetID(game.URL);

                foreach (Tag item in game.Tags)
                {
                    item.Id = GetID(item.URL);
                }
                foreach (Genre item in game.Genres)
                {
                    item.Id = GetID(item.URL);
                }
               
                game.dbGenres = getGenres();
                game.dbTags = getTags();

                return View(game);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Edit(Game game)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var request = new RestRequest("Games/" + game.Id, Method.PUT);
            request.RequestFormat = DataFormat.Json;
            APIHeaders(request);

            request.AddBody(game);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Details(int Id)
        {
            var request = new RestRequest("Games/" + Id, Method.GET);

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
            var request = new RestRequest("Games/" + Id, Method.GET);

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
            var request = new RestRequest("Games/" + game.Id, Method.DELETE);

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