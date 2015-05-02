
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
using Android.Graphics.Drawables;

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

				Button checkoutBTN = FindViewById<Button> (Resource.Id.checkout);
				checkoutBTN.Click += delegate {
					FragmentTransaction transaction = FragmentManager.BeginTransaction ();
					DialogCheckout checkoutDialog = new DialogCheckout ();
					checkoutDialog.Show (transaction, "dialog fragment");
				};

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
						TableRow.LayoutParams.MatchParent,
						.33f);
					tRow.AddView (button);
					button.Text = string.Format (game.m_Item1.GameName);
					TextView price = new TextView (this);
					price.LayoutParameters = new TableRow.LayoutParams (
						TableRow.LayoutParams.MatchParent,
						TableRow.LayoutParams.MatchParent,
						.34f);
					price.TextSize = 20;
					price.Gravity = GravityFlags.Right;
					tRow.AddView (price);
					total = total + (game.m_Item1.Price);
					price.Text = string.Format ("$" + game.m_Item1.Price);
				price.SetTextColor (Android.Graphics.Color.Black);
					TextView invCount = new TextView (this);
					invCount.LayoutParameters = new TableRow.LayoutParams (
						TableRow.LayoutParams.MatchParent,
						TableRow.LayoutParams.MatchParent,
						.33f);
					invCount.TextSize = 20;
					invCount.Gravity = GravityFlags.Right;
					tRow.AddView (invCount);
					invCount.Text = string.Format ("" + game.m_Item2);
				TableRow tRow2 = new TableRow (this);
				layout.AddView (tRow2);
					Space space = new Space (this);
					ImageButton delete = new ImageButton (this);
					Button plus = new Button (this);
					Button minus = new Button (this);
					tRow2.AddView (space);
					tRow2.AddView (delete);
					tRow2.AddView (plus);
					tRow2.AddView (minus);
					plus.LayoutParameters = new TableRow.LayoutParams (
						TableRow.LayoutParams.MatchParent,
						ViewGroup.LayoutParams.MatchParent,
						.25f);
					plus.Text = string.Format ("+");
					minus.LayoutParameters = new TableRow.LayoutParams (
						TableRow.LayoutParams.MatchParent,
						TableRow.LayoutParams.MatchParent,
						.25f);
					minus.Text = string.Format ("-");
					delete.LayoutParameters = new TableRow.LayoutParams (
						TableRow.LayoutParams.MatchParent,
						TableRow.LayoutParams.MatchParent,
						.25f);
					space.LayoutParameters = new TableRow.LayoutParams (
						TableRow.LayoutParams.MatchParent,
						ViewGroup.LayoutParams.MatchParent,
						.25f);
					delete.SetBackgroundResource(Resource.Drawable.delete);

			}
			return total;
		}
		public void processClick(int i){
			//do button stuff here
		}
	}
}

