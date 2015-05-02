using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;

namespace CustomerConsumer
{
	[Activity (Label = "GameActivity", Theme="@android:style/Theme.Black.NoTitleBar")]			
	public class GameActivity : Activity
	{
		private RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/");
		//Don't forget to change the url as appropriate.
		private RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();
		private List<Game> gamesList;

		private void APIHeaders(RestRequest request)
		{
			if (UserSessionInfo.getUserId() != 0 && UserSessionInfo.getApiKey() != null)
			{
				string apikey = UserSessionInfo.getApiKey ();
				int id = UserSessionInfo.getUserId ();
				request.AddHeader("xcmps383authenticationkey", UserSessionInfo.getApiKey());
				request.AddHeader ("xcmps383authenticationid", UserSessionInfo.getUserId().ToString());
			}
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			try{
				SetContentView (Resource.Layout.Games);
			}
			catch (Exception ex){
				string x = ex.Message;
			}

			ListItems ();
				
		}
			
		public void ListItems()
		{
			var request = new RestRequest ("api/Games", Method.GET);
			APIHeaders (request);
			var response = client.Execute (request);

			if (response.StatusCode == HttpStatusCode.OK) 
			{
				ListView listView;
				listView = FindViewById<ListView> (Resource.Id.listOfGames);
				IEnumerable<Game> games = _deserializer.Deserialize<List<Game>> (response);
				gamesList = games.ToList ();
				listView.Adapter = new GamesAdapter (this, gamesList);
				listView.ItemClick += OnGameClick;
			}
		}
		public void OnGameClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var listView = sender as ListView;
			Game t = gamesList[e.Position];
			FragmentTransaction transaction = FragmentManager.BeginTransaction();
			GamesDetailFragment details = new GamesDetailFragment();
			details.Show(transaction, "dialog fragment");
			details.setGame(t);
		}
	}
}

