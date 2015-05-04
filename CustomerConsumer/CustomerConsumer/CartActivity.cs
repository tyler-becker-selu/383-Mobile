
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
	[Activity (Label = "Cart", Icon = "@drawable/hugeNoBorder")]			
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
					Cart cart = UserSessionInfo.getUserCart();
						var request = new RestRequest ("api/Carts/" + UserSessionInfo.getUserId(), Method.PUT);
						APIHeaders (request);
						RestSharp.Serializers.JsonSerializer serial = new RestSharp.Serializers.JsonSerializer ();
						var json = serial.Serialize (cart);

						request.AddParameter ("text/json", json, ParameterType.RequestBody);

						var response = client.Execute (request);
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
			decimal total = 0;
			ListView cartList = FindViewById<ListView>(Resource.Id.cartList);
			GamesForCartsAdapter adapter = new GamesForCartsAdapter (this,UserSessionInfo.getUserCart().Games);
			cartList.Adapter = adapter;
			cartList.ItemClick += OnGameClick;
			foreach (GamesForCart game in UserSessionInfo.getUserCart().Games) {
				for (int i = 0; i < game.m_Item2; ++i) {
					total = total + (game.m_Item1.Price);
				}
			}
			return total;
		}

		/*public decimal makeGameButtons(List<GamesForCart> cartGames){
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
		}*/
		public void OnGameClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			Game t = UserSessionInfo.getUserCart().Games[e.Position].m_Item1;
			FragmentTransaction transaction = FragmentManager.BeginTransaction();
			CartDetailFragment details = new CartDetailFragment();
			details.Show(transaction, "dialog fragment");
			details.setGame (t);
			details.AddedToCart += Details_AddedToCart;
		}

		void Details_AddedToCart (object sender, EventArgs e){
			Intent intent = new Intent (this, typeof(CartActivity));
			StartActivity (intent);
			Finish ();
		}
	}
}

