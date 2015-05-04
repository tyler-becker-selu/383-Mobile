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
using Java.Lang;

namespace CustomerConsumer
{
	[Activity (Label = "Games", Theme="@android:style/Theme", Icon = "@drawable/hugeNoBorder")]			
	public class GameActivity : Activity
	{
		private RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();
		private RestClient _client = new RestClient("http://dev.envocsupport.com/GameStore4/");
		private SearchView _searchBar;
		private GamesAdapter _adapter;
		private List<Game> _gamesList;
		private RadioButton _genreBtn;
		private RadioButton _tagBtn;

		private void APIHeaders(RestRequest request)
		{
			if (UserSessionInfo.getUserId() != 0 && UserSessionInfo.getApiKey() != null)
			{
				request.AddHeader("xcmps383authenticationkey", UserSessionInfo.getApiKey());
				request.AddHeader ("xcmps383authenticationid", UserSessionInfo.getUserId().ToString());
			}
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			try{
				SetContentView (Resource.Layout.Games);
			}
			catch (System.Exception ex){
				string x = ex.Message;
			}
			Button goToCart = FindViewById<Button> (Resource.Id.gamesToCart);
			goToCart.Click += delegate {
				Intent myIntent = new Intent(this, typeof(CartActivity));
				StartActivity (myIntent);
			};
			_searchBar = FindViewById<SearchView> (Resource.Id.genreSearch);
			_genreBtn = FindViewById <RadioButton> (Resource.Id.genre);
			_tagBtn = FindViewById<RadioButton> (Resource.Id.tags);

			_genreBtn.Click += delegate {
				UserSessionInfo.SetSearchTag(false);
				_searchBar.SetQueryHint("Genres");
			};

			_tagBtn.Click += delegate {
				UserSessionInfo.SetSearchTag(true);
				_searchBar.SetQueryHint("Tags");
			};

			ListItems ();
				
		}
			
		public void ListItems()
		{
			var request = new RestRequest ("api/Games", Method.GET);
			APIHeaders (request);
			var response = _client.Execute (request);

			if (response.StatusCode == HttpStatusCode.OK) 
			{
				ListView listView;
				listView = FindViewById<ListView> (Resource.Id.listOfGames);
				_gamesList = _deserializer.Deserialize<List<Game>> (response);
				_adapter = new GamesAdapter (this, _gamesList);
				listView.Adapter = _adapter;
				listView.ItemClick += OnGameClick;

				_searchBar.QueryTextSubmit += SearchByGenres;
				_searchBar.QueryTextChange += CheckNull;


			}
		}
		public void OnGameClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			Game t = _gamesList[e.Position];
			FragmentTransaction transaction = FragmentManager.BeginTransaction();
			GamesDetailFragment details = new GamesDetailFragment();
			details.Show(transaction, "dialog fragment");
			details.setGame(t);
		}
		public void SearchByGenres(object sender, Android.Widget.SearchView.QueryTextSubmitEventArgs args){
			_adapter.Filter.InvokeFilter (_searchBar.Query);
		}
		public void CheckNull(object sender, Android.Widget.SearchView.QueryTextChangeEventArgs args){
			if (_searchBar.Query == "") {
				ICharSequence seq = null;
				_adapter.Filter.InvokeFilter (seq);
			}
		}
			
	}
}

