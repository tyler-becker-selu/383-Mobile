
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
	[Activity (Label = "Cart", Theme="@android:style/Theme.Black")]			
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
				Button goToGamesBTN = FindViewById<Button> (Resource.Id.cartToGames);
				goToGamesBTN.Click += delegate {
					Intent myIntent = new Intent(this, typeof(GameActivity));
					StartActivity (myIntent);

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
			total = makeGameButtons (UserSessionInfo.getUserCart().Games);
			if (response.StatusCode == HttpStatusCode.OK) {
				Cart cart = _deserializer.Deserialize<Cart> (response);
				string[] url = cart.URL.Split ('/');
				UserSessionInfo.setCartId (Convert.ToInt32 (url [6]));
				total = makeGameButtons (UserSessionInfo.getUserCart().Games);
			} else if (response.StatusCode == HttpStatusCode.NotFound) {
				TableLayout layout = FindViewById<TableLayout>(Resource.Id.buttonLayoutCart);
				TableRow tRow = new TableRow (this);
				TableRow titleRow = FindViewById<TableRow> (Resource.Id.tableRow1);
				titleRow.Visibility = ViewStates.Invisible;
				layout.AddView (tRow);
				TextView emptyCart = new TextView (this);
				emptyCart.LayoutParameters = new TableRow.LayoutParams (
					TableRow.LayoutParams.WrapContent,
					TableRow.LayoutParams.WrapContent);
				emptyCart.TextSize = 30;
				emptyCart.SetTextColor (Android.Graphics.Color.Black);
				emptyCart.Gravity = GravityFlags.CenterHorizontal|GravityFlags.Top;
				tRow.AddView (emptyCart);
				emptyCart.Text = string.Format ("You don't have any games in your cart yet! Click the Go To Games button to start adding games");

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
				button.Click+= delegate {
					Game g = game.m_Item1;
					FragmentTransaction transaction = FragmentManager.BeginTransaction();
					CartDetailFragment details = new CartDetailFragment();
					details.Show(transaction, "dialog fragment");
					details.setGame(g);
				};
					TextView price = new TextView (this);
					price.LayoutParameters = new TableRow.LayoutParams (
						TableRow.LayoutParams.MatchParent,
						TableRow.LayoutParams.MatchParent,
						.34f);
					price.TextSize = 20;
					price.Gravity = GravityFlags.Right;
					tRow.AddView (price);
					for (int i = 0; i < game.m_Item2; ++i) {
						total = total + (game.m_Item1.Price);
					}
					price.Text = string.Format ("$" + game.m_Item1.Price);
					price.SetTextColor (Android.Graphics.Color.Black);
					TextView invCount = new TextView (this);
					invCount.LayoutParameters = new TableRow.LayoutParams (
						TableRow.LayoutParams.MatchParent,
						TableRow.LayoutParams.MatchParent,
						.33f);
					invCount.TextSize = 20;
					invCount.SetTextColor (Android.Graphics.Color.Black);
					invCount.Gravity = GravityFlags.Right;
					tRow.AddView (invCount);
					invCount.Text = string.Format ("" + game.m_Item2);

			}
			return total;
		}
	}
}

