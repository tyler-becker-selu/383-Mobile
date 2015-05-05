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
    public class BaseController : Controller
    {

        //public RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/api");
        public RestClient client = new RestClient("http://localhost:12932/api/");
        public JsonDeserializer _deserializer = new JsonDeserializer();
    
        #region Algorithms
        public bool isLoggedIn()
        {
            return (System.Web.HttpContext.Current.Session["ApiKey"] != null
                && System.Web.HttpContext.Current.Session["UserId"] != null);
        }
        public void APIHeaders(RestRequest request)
        {
            if (isLoggedIn())
            {
                request.AddHeader("xcmps383authenticationkey",
                    System.Web.HttpContext.Current.Session["ApiKey"].ToString());

                request.AddHeader("xcmps383authenticationid", 
                    System.Web.HttpContext.Current.Session["UserId"].ToString());
            }
        }

        public int GetID(string p)
        {
            string[] x = p.Split('/');
            return Convert.ToInt32(x[x.Length - 1]);
        }

        public List<Game> getGames()
        {
            var request = new RestRequest("Games/", Method.GET);
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

        public List<Game> getGames(string genreName)
        {
            var request = new RestRequest("Games/", Method.GET);
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

        public dynamic getGenres()
        {
            var request = new RestRequest("Genres/", Method.GET);
            var genreList = new List<Genre>();

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                genreList = deserial.Deserialize<List<Genre>>(APIresponse);

                foreach (Genre gen in genreList)
                {
                    gen.Id = GetID(gen.URL);
                }
            }

            return genreList;
        }

        public dynamic getTags()
        {
            var request = new RestRequest("Tags/", Method.GET);
            var tagList = new List<Tag>();

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                tagList = deserial.Deserialize<List<Tag>>(APIresponse);

                foreach (Tag tag in tagList)
                {
                    tag.Id = GetID(tag.URL);
                }
            }

            return tagList;
        }

        #endregion

    }
}