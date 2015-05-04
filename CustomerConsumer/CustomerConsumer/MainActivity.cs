using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using RestSharp;
using System.Net;
using RestSharp.Deserializers;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Support.V7.App;

namespace CustomerConsumer
{
	[Activity (Label = "Game Store 4", MainLauncher = true, Icon = "@drawable/hugeNoBorder")]
	public class MainActivity : Activity
	{
		private Button loginBTN;
		public RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/");

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

				SetContentView (Resource.Layout.Main);
			}
			catch (Exception ex){
				string x = ex.Message;
			}
		
			// Set our view from the "main" layout resource

			// Get our button from the layout resource,
			// and attach an event to it

			loginBTN = FindViewById<Button> (Resource.Id.LoginBTN);
			Button h2g = FindViewById<Button> (Resource.Id.homeToGames);
			Button h2c = FindViewById<Button> (Resource.Id.homeToCart);
			if (UserSessionInfo.getUserId () == 0) {
				h2g.Visibility = ViewStates.Invisible;
				h2c.Visibility = ViewStates.Invisible;
				loginBTN.Click += delegate {
					FragmentTransaction transaction = FragmentManager.BeginTransaction ();
					DialogLogin loginDialog = new DialogLogin ();
					loginDialog.Show (transaction, "dialog fragment");
					loginDialog.globOnLoginComplete += LoginDialog_globOnLoginComplete;
				};
			} else {
				loginBTN.Text = "Logout";
				h2g.Visibility = ViewStates.Visible;
				h2g.Click += delegate {
					Intent intent = new Intent (this, typeof(GameActivity));
					StartActivity (intent);
				};
				h2c.Click += delegate {
					Intent intent = new Intent (this, typeof(CartActivity));
					StartActivity (intent);
				};
				h2c.Visibility = ViewStates.Visible;
				loginBTN.Click += delegate {
					UserSessionInfo.Logout();
					Intent intent = new Intent (this, typeof(MainActivity));
					StartActivity (intent);
					Finish ();	
				};
			}
		}


		void LoginDialog_globOnLoginComplete (object sender, OnLoginEventArgs e)
		{
			var request = new RestRequest("api/ApiKey?email=" + e.UserName + "&password=" + e.Password, Method.GET);
			request.AddHeader ("Content-Type", "application/json");

			var response = client.Execute(request);
			RestSharp.Deserializers.JsonDeserializer deserial= new JsonDeserializer();

			TextView text = FindViewById<TextView> (Resource.Id.outputText);
			if (response.StatusCode == HttpStatusCode.OK) {
				ApiKeys userData =  _deserializer.Deserialize<ApiKeys>(response);

				UserSessionInfo.setApiKey (userData.ApiKey);
				UserSessionInfo.setUserId (userData.UserId);

				var cartRequest = new RestRequest ("api/Carts/" + UserSessionInfo.getUserId (), Method.GET);
				APIHeaders (cartRequest);
				cartRequest.RequestFormat = DataFormat.Json;

				Cart userCart;

				var cartResponse = client.Execute (cartRequest);
				if (cartResponse.StatusCode != HttpStatusCode.NotFound) {
					userCart = _deserializer.Deserialize<Cart> (cartResponse);
				} else {
					userCart = null;
				}

				if (userCart != null) {
					UserSessionInfo.setUserCart (userCart);
				} else {
					userCart = new Cart ();
					UserSessionInfo.setUserCart (userCart);
				}
				Intent intent = new Intent (this, typeof(MainActivity));
				StartActivity (intent);
				Finish ();	
				Intent myIntent = new Intent(this, typeof(GameActivity));
				StartActivity (myIntent);

			} else if (response.StatusCode == HttpStatusCode.Forbidden) {
				text.Text = string.Format ("Incorrect email/password combination");
			} else {
				text.Text = string.Format ("An error has occurred "+response.StatusCode.ToString());
				//text.Text = string.Format ("An error has occurred");
			}
		}
	}
}


