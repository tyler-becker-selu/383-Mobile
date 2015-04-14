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

		private RestClient client = new RestClient("http://localhost:12932/");

		//int count = 1;

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
			
		//	button.Click += delegate {
		//		button.Text = string.Format ("{0} clicks!", count++);
		//	};
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
			var request = new RestRequest("ApiKey?email=" + e.UserName + "&password=" + e.Password, Method.GET);

			var response = client.Execute(request);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				//ApiKeys userData =  _deserializer.Deserialize<ApiKeys>(response);
				//var x = userData.ApiKey;
				//var y = userData.UserId;
			
			}

		}


	}
}


