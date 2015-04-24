
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
using RestSharp;
using System.Net;

namespace CustomerConsumer
{
	[Activity (Label = "Cart")]			
	public class CartActivity : Activity
	{
		private RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/");

		private RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();

		private void APIHeaders(RestRequest request)
		{
			if (UserSessionInfo.getUserId() != 0 && UserSessionInfo.getApiKey() != null)
			{
				string apikey = UserSessionInfo.getApiKey ();
				int id = UserSessionInfo.getUserId ();
				request.AddHeader("xcmps383authenticationkey", UserSessionInfo.getApiKey());
				request.AddHeader ("xcmps383authenticationid", UserSessionInfo.getUserId ().ToString());
			}
		}



		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			try{
				SetContentView (Resource.Layout.Cart);
				decimal total = 0;
				total = listGames();
				TextView totalText = FindViewById<TextView>(Resource.Id.totalForCart);
				totalText.Text = string.Format("$"+total);
			}
			catch (Exception ex){
				string x = ex.Message;
			}
		}



		public decimal listGames(){
			var request = new RestRequest ("api/Carts/" + UserSessionInfo.getUserId (), Method.GET);
			APIHeaders (request);
			request.OnBeforeDeserialization = resp => {
				resp.ContentType = "application/json";
			};
			var response = client.Execute <Cart> (request);
			Button gBtn = FindViewById<Button> (Resource.Id.checkout);
			decimal total = 0;
			if (response.StatusCode == HttpStatusCode.OK) {
				Cart cart = _deserializer.Deserialize<Cart> (response);
				//IEnumerable<Game> gamesInCart = cart.Games;
				total = makeGameButtons (cart.Games);
			}
			return total;
		}

		public decimal makeGameButtons(List<GamesForCart> cartGames){
			decimal total = 0;
			TableLayout layout = FindViewById<TableLayout>(Resource.Id.buttonLayoutCart);
			foreach (GamesForCart game in cartGames) {
				TableRow tRow = new TableRow (this);
				layout.AddView (tRow);
				Button button = new Button (this);
				button.LayoutParameters = new TableRow.LayoutParams (
					TableRow.LayoutParams.MatchParent,
					TableRow.LayoutParams.WrapContent,
					.5f);
				tRow.AddView (button);
				button.Text = string.Format (game.m_Item1.GameName);
				TextView price = new TextView (this);
				price.LayoutParameters = new TableRow.LayoutParams (
					TableRow.LayoutParams.MatchParent,
					TableRow.LayoutParams.WrapContent,
					.5f);
				price.TextSize = 20;
				price.Gravity = GravityFlags.Right;
				tRow.AddView (price);
				total = total + (game.m_Item1.Price);
				price.Text = string.Format ("$" + game.m_Item1.Price);
			}
			return total;
		}
		public void processClick(int i){
			//do button stuff here
		}
	}
}

