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

namespace CustomerConsumer
{
	[Activity (Label = "Game Store 4", MainLauncher = true, Icon = "@drawable/icon")]
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
			loginBTN.TextSize = 50;
			loginBTN.Click += delegate {
				FragmentTransaction transaction = FragmentManager.BeginTransaction ();
				DialogLogin loginDialog = new DialogLogin ();
				loginDialog.Show (transaction, "dialog fragment");
				loginDialog.globOnLoginComplete += LoginDialog_globOnLoginComplete;
			};
				
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
					
				Intent myIntent = new Intent(this, typeof(MenuActivity));
				StartActivity (myIntent);
			} else if (response.StatusCode == HttpStatusCode.Forbidden) {
				text.Text = string.Format ("Incorrect email/password combination");
			} else {
				text.Text = string.Format ("An error has occurred");
			}
		}

		protected override void OnDestroy ()
		{
			var cart = UserSessionInfo.getUserCart ();
			if (cart != null) {
				
				var request = new RestRequest ("api/Carts/" + UserSessionInfo.getUserId(), Method.PUT);
				APIHeaders (request);
				RestSharp.Serializers.JsonSerializer serial = new RestSharp.Serializers.JsonSerializer ();
				var json = serial.Serialize (cart);

				request.AddParameter ("text/json", json, ParameterType.RequestBody);

				var response = client.Execute (request);
			}

			base.OnDestroy ();

		}
	}
}


