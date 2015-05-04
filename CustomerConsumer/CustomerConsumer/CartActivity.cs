
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
		private RestClient client = UserSessionInfo.getClient();

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
				Cart cart = UserSessionInfo.getUserCart();
				SetContentView (Resource.Layout.Cart);
				decimal total = 0;
				TextView totalText = FindViewById<TextView>(Resource.Id.totalForCart);
				totalText.Text = string.Format("$"+total);

				Button checkoutBTN = FindViewById<Button> (Resource.Id.checkout);
				checkoutBTN.Click += delegate {
						var request = new RestRequest ("api/Carts/" + UserSessionInfo.getUserId(), Method.PUT);
						APIHeaders (request);
						RestSharp.Serializers.JsonSerializer serial = new RestSharp.Serializers.JsonSerializer ();
						var json = serial.Serialize (cart);
						request.AddParameter ("text/json", json, ParameterType.RequestBody);
						string[] url = cart.URL.Split('/');
						UserSessionInfo.setCartId(Convert.ToInt32(url[url.Length-1]));
						var response = client.Execute (request);
					FragmentTransaction transaction = FragmentManager.BeginTransaction ();
					DialogCheckout checkoutDialog = new DialogCheckout ();
					checkoutDialog.Show (transaction, "dialog fragment");
				};
				if(cart.Games == null || !cart.Games.Any()){
					totalText.Text = "You don't have any games in your cart! Click the Go To Games button to find some games.";
					totalText.Gravity = GravityFlags.Center;
					TextView tText = FindViewById<TextView>(Resource.Id.textview);
					tText.Text = "";
					checkoutBTN.Visibility = ViewStates.Invisible;
					
				}else{
					total = listGames();
					totalText.Text = string.Format("$"+total);
				}

				Button goToGamesBTN = FindViewById<Button> (Resource.Id.cartToGames);
				goToGamesBTN.Click += delegate {
					Intent myIntent = new Intent(this, typeof(GameActivity));
					StartActivity (myIntent);
					Finish();
				};
				Button logout = FindViewById<Button> (Resource.Id.logoutCart);
				logout.Click += delegate {
					UserSessionInfo.Logout();
					Intent myIntent = new Intent(this, typeof(MainActivity));
					StartActivity (myIntent);
					Finish();
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

