using System;
using System.Collections.Generic;
using Android.Widget;
using Android.App;
using Android.Views;
using System.Linq;
using Java.Lang;
using Object = Java.Lang.Object;
using RestSharp;


namespace CustomerConsumer
{
	public class GamesAdapter : BaseAdapter<Game>, IFilterable
	{
		List<Game> _originalGames;
		List<Game> _games;
		Activity _context;

		private static void APIHeaders(RestRequest request)
		{
			if (UserSessionInfo.getUserId() != 0 && UserSessionInfo.getApiKey() != null)
			{
				string apikey = UserSessionInfo.getApiKey ();
				int id = UserSessionInfo.getUserId ();
				request.AddHeader("xcmps383authenticationkey", UserSessionInfo.getApiKey());
				request.AddHeader ("xcmps383authenticationid", UserSessionInfo.getUserId ().ToString());
			}
		}


		public GamesAdapter (Activity context, List<Game> games):base()
		{
			_context = context;
			_games = games;

			Filter = new GameFilter (this);
		}
		public override int Count {
			get {
				return _games.Count ();
			}
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override Game this[int position] {  
			get { return _games[position]; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
			
			view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = _games[position].GameName;
			view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = "$"+_games[position].Price;
			view.FindViewById<TextView> (Android.Resource.Id.Text1).SetTextColor (Android.Graphics.Color.ParseColor ("#000000"));
			view.FindViewById<TextView> (Android.Resource.Id.Text2).SetTextColor (Android.Graphics.Color.ParseColor ("#6D6D6D"));
			return view;
		}

		public Filter Filter{ get; set;}

		public override void NotifyDataSetChanged()
		{
			base.NotifyDataSetChanged ();
		}

		private class GameFilter : Filter
		{
			private RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/");
			private RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();

			private readonly GamesAdapter _gameAdapter;
			public GameFilter(GamesAdapter gamesAdapter)
			{
				_gameAdapter = gamesAdapter;
			}

			protected override FilterResults PerformFiltering (ICharSequence constraint)
			{
				var retObj = new FilterResults ();
				var results = new List<Game>();

				if (_gameAdapter._originalGames == null)
					_gameAdapter._originalGames = _gameAdapter._games;

				if (_gameAdapter._originalGames.Any() && constraint == null) {
					List<Object> tempObjectList = new List<Object>();

					foreach (Game g in _gameAdapter._originalGames) {
						tempObjectList.Add (g.ToJavaObject ());
					}

					retObj.Values = FromArray (tempObjectList.ToArray());
					retObj.Count = _gameAdapter._originalGames.Count;
					return retObj;
				}
					

				if (_gameAdapter._originalGames != null && _gameAdapter._originalGames.Any ()) {
					var request = new RestRequest ("api/Games", Method.GET);
					APIHeaders (request);

					bool check = UserSessionInfo.GetSearchTag ();
					if (check==false) {
						request.AddParameter ("genre", constraint.ToString ());
					} 
					if(check == true ){
						request.AddParameter ("tag", constraint.ToString ());
					}
						
					request.RequestFormat = DataFormat.Json;

					var response = client.Execute (request);
					if (response.StatusCode == System.Net.HttpStatusCode.OK) {
						List<Game> idk = _deserializer.Deserialize<List<Game>> (response);
						results.AddRange(idk);

						retObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
						retObj.Count = results.Count;

						constraint.Dispose();
					}
				}

				return retObj;
			}

			protected override void PublishResults(ICharSequence constraint, FilterResults results)
			{
				using(var values = results.Values)
					_gameAdapter._games = values.ToArray<Object>()
						.Select(r => r.ToNetObject<Game>()).Where(r=>r.InventoryStock > 0).ToList();

				_gameAdapter.NotifyDataSetChanged ();

				if(constraint !=null)
					constraint.Dispose ();
				
				results.Dispose ();
			}
		}
	}
}

