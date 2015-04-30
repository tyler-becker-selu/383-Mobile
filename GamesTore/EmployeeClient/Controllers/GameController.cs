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
    public class GameController : Controller
    {
        private RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/");

        #region Algorthms

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

        private List<Game> getGames()
        {
            var request = new RestRequest("api/Games", Method.GET);
            var gameList = new List<Game>();

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                gameList = deserial.Deserialize<List<Game>>(APIresponse);

                foreach (Game item in gameList)
                {
                    item.Id = GetID(item.URL);
                }
            }

            return gameList;
        }

        private dynamic getGenres()
        {
            var request = new RestRequest("api/Genres", Method.GET);
            var genreList = new List<Genre>();

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                genreList = deserial.Deserialize<List<Genre>>(APIresponse);
            }

            return genreList;
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
        public ActionResult Create(Game game, FormCollection formCollection)
        {
            var request = new RestRequest("api/Games/", Method.POST);

            APIHeaders(request);

         //   var x = formCollection["tags"];

            //////////////////////////////////Dummy Data (for now)/////////////////////////////////////
            AddDummyGenre(game);
            AddDummyTags(game);
            ///////////////////////////////////////////////////////////////////////////////////////////

            request.AddObject(game);

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