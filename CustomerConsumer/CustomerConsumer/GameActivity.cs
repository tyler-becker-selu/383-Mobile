using RestSharp;
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
using System.Net;

namespace CustomerConsumer
{
	[Activity (Label = "GameActivity")]			
	public class GameActivity : ListActivity
	{
		private RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/");
		//Don't forget to change the url as appropriate.

		private RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();

		private void APIHeaders(RestRequest request)
		{
			if (UserSessionInfo.getUserId() != 0 && UserSessionInfo.getApiKey() != null)
			{
				request.AddHeader("xcmps383authenticationkey", UserSessionInfo.getApiKey());
				request.AddHeader("xcmps383authenticationid", UserSessionInfo.getUserId().ToString());
			}
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			try{
				SetContentView (Resource.Layout.Games);
			}
			catch (Exception ex){
				string x = ex.Message;
			}

			ListItems ();
				
		}

		void ListItems()
		{
			var request = new RestRequest ("Games", Method.GET);
			APIHeaders (request);
			Button gBtn = FindViewById<Button> (Resource.Id.button1);
			var response = client.Execute (request);

			if (response.StatusCode == HttpStatusCode.OK) 
			{
				IEnumerable<Game> games = _deserializer.Deserialize<List<Game>> (response);
				gBtn.Text = games.First().GameName;
			}
		}
	}
}

