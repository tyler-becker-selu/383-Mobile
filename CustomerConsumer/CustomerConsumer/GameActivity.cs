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
	[Activity (Label = "GameActivity")]			
	public class GameActivity : ListActivity
	{
		private RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/");
		//Don't forget to change the url as appropriate.

		private RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();

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
				IEnumerable<Game> games = _deserializer.Deserialize<List<Game>> (response);
				makeGameButtons (games.ToList ());
			}
		}
		public void makeGameButtons(List<Game> games){
			TableLayout layout = FindViewById<TableLayout>(Resource.Id.gamesList);
			foreach (Game game in games) {
				TableRow tRow = new TableRow (this);
				layout.AddView (tRow);
				Button button = new Button (this);
				button.LayoutParameters = new TableRow.LayoutParams (
					TableRow.LayoutParams.MatchParent,
					TableRow.LayoutParams.WrapContent,
					.5f);
				tRow.AddView (button);
				button.Text = string.Format (game.GameName);

				button.Click += delegate {
					FragmentTransaction transaction = FragmentManager.BeginTransaction();
					GamesDetailFragment details = new GamesDetailFragment();
					details.Show(transaction, "dialog fragment");
					details.setTitle(game.GameName);
					details.setPrice(game.Price);
				};

				TextView price = new TextView (this);
				price.LayoutParameters = new TableRow.LayoutParams (
					TableRow.LayoutParams.MatchParent,
					TableRow.LayoutParams.WrapContent,
					.5f);
				price.TextSize = 20;
				price.SetTextColor (Android.Graphics.Color.Black);
				price.Gravity = GravityFlags.Right;
				tRow.AddView (price);
				price.Text = string.Format ("$" + game.Price);
			}
		}
		private int GetID(string p)
		{
			string[] x = p.Split('/');
			return Convert.ToInt32(x[x.Length - 1]);
		}
	}
}

