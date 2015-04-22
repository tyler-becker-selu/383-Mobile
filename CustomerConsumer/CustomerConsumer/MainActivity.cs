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

namespace CustomerConsumer
{
	[Activity (Label = "CustomerConsumer", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private Button loginBTN;

		public RestClient client = new RestClient("http://147.174.76.223:12932/");

		int count = 1;

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
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
			};
			loginBTN = FindViewById<Button> (Resource.Id.LoginBTN);
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
				ApiKeys	apiKey = deserial.Deserialize<ApiKeys> (response);
				count = 0;
				Button button = FindViewById<Button> (Resource.Id.myButton);
				button.Text = string.Format ("{0} clicks!", count++);
				text.Text = string.Format (apiKey.ApiKey + "\n" + apiKey.UserId);
			} else if (response.StatusCode == HttpStatusCode.Forbidden) {
				text.Text = string.Format ("Incorrect email/password combination");
			} else {
				text.Text = string.Format ("An error has occurred");
			}
		}


	}
}


