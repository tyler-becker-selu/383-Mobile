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

namespace CustomerConsumer
{
	[Activity (Label = "Game Store 4", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private Button loginBTN;

		public RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/");

		private RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();


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
				Intent myIntent = new Intent(this, typeof(MenuActivity));
				StartActivity (myIntent);
			} else if (response.StatusCode == HttpStatusCode.Forbidden) {
				text.Text = string.Format ("Incorrect email/password combination");
			} else {
				text.Text = string.Format ("An error has occurred");
			}
		}


	}
}


