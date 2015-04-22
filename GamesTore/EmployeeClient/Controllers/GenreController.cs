using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeClient.Models;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using RestSharp.Deserializers;
namespace EmployeeClient.Controllers
{
    public class GenreController : Controller
    {
        private RestClient client = new RestClient("http://localhost:12932/api");

        RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();

        private void APIHeaders(RestRequest request)
        {
            if (Session["ApiKey"] != null && Session["UserId"] != null)
            {
                request.AddHeader("xcmps383authenticationkey", Session["ApiKey"].ToString());
                request.AddHeader("xcmps383authenticationid", Session["UserId"].ToString());
            }
        }


        public List<Game> GetGamesForGenre(string  genreName)
        {
            List<Game> gameList = new List<Game>();

            gameList = getGames(genreName);

            return gameList;
        }

        private List<Game> getGames(string genreName)
        {
            var request = new RestRequest("Games", Method.GET);
            var gameList = new List<Game>();

            APIHeaders(request);
            request.AddParameter("genre", genreName);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                gameList = deserial.Deserialize<List<Game>>(APIresponse);
            }

            return gameList;
        }

        private int GetID(string p)
        {
            string[] x = p.Split('/');
            return Convert.ToInt32(x[x.Length - 1]);
        }

        // GET: Genre
        public ActionResult Index()
        {
            var request = new RestRequest("Genres", Method.GET);
            APIHeaders(request);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                IEnumerable<Genre> genres = _deserializer.Deserialize<List<Genre>>(response);

                foreach(Genre item in genres)
                {
                   item.Id = GetID(item.URL);
                }

                return View(genres);
            }

            return RedirectToAction("Index","Home");
        }

        // GET: Genre/Details/5
        public ActionResult Details(int id)
        {
            var request = new RestRequest("Genres/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            APIHeaders(request);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Genre genre = _deserializer.Deserialize<Genre>(response);
                genre.Games = GetGamesForGenre(genre.Name);

                return View(genre);
            }

            return HttpNotFound();
        }

        // GET: Genre/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Genre/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Genre genre)
        {
            if(ModelState.IsValid)
            {
                var request = new RestRequest("Genres", Method.POST);
                APIHeaders(request);

                var json = JsonConvert.SerializeObject(genre);

                request.AddParameter("text/json", json, ParameterType.RequestBody);

                var response = client.Execute(request);

                if(response.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction("Index");
                }

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Genre/Edit/5
        public ActionResult Edit(int id)
        {
            var request = new RestRequest("Genres/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            APIHeaders(request);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Genre genre = _deserializer.Deserialize<Genre>(response);
                return View(genre);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Genre/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Genre genre)
        {
            var request = new RestRequest("Genres/{id}", Method.PUT);
            request.AddUrlSegment("id", id.ToString());
            APIHeaders(request);

            var json = JsonConvert.SerializeObject(genre);

            request.AddParameter("text/json", json, ParameterType.RequestBody);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Genre/Delete/5
        public ActionResult Delete(int id)
        {
            var request = new RestRequest("Genres/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            APIHeaders(request);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Genre genre = _deserializer.Deserialize<Genre>(response);
                return View(genre);
            }

            return HttpNotFound();
        }

        // POST: Genre/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string message)
        {
            var request = new RestRequest("Genres/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());
            APIHeaders(request);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}
