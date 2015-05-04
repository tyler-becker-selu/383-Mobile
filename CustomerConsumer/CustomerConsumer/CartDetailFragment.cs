
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CustomerConsumer
{
	[Activity (Label = "Cart", Theme="@android:style/Theme.Black.NoTitleBar")]
	public class CartDetailFragment : DialogFragment
	{
		private RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/");
		private RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();
		private Game _detailedGame;
		private TextView _priceText;
		private TextView _gmName;
		private TextView _genres;
		private TextView _tags;
		private TextView _close;
		private NumberPicker _numPicker;
		private Button _addToCart;
		private ImageButton _delete;
		public event EventHandler<EventArgs> AddedToCart;
		//private Button _closeFragment;

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

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Dialog.SetTitle ("Details");

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.CartDetailLayout,container, false);
			Cart tempCart = UserSessionInfo.getUserCart();
			GamesForCart game = tempCart.Games.FirstOrDefault(x => x.m_Item1.GameName.Equals(_detailedGame.GameName));

			_priceText = view.FindViewById<TextView> (Resource.Id.itemPrice);
			_priceText.Text = "Price: $" + (_detailedGame.Price*game.m_Item2);
			_gmName = view.FindViewById<TextView> (Resource.Id.gameName);
			_gmName.Text = _detailedGame.GameName;
			_genres = view.FindViewById<TextView> (Resource.Id.cartGenres);
			foreach (Genre g in _detailedGame.Genres) {
				_genres.Text += g.Name + ", ";
			}
			_tags = view.FindViewById<TextView> (Resource.Id.cartTags);
			foreach (Tag t in _detailedGame.Tags) {
				_tags.Text += t.Name + ", ";
			}
			_close = view.FindViewById<TextView> (Resource.Id.closeCart);
			_close.Click += delegate {
				this.Dismiss();
			};
			_numPicker = view.FindViewById<NumberPicker> (Resource.Id.numberPicker1);
			_numPicker.MinValue = 1;
			_numPicker.MaxValue = _detailedGame.InventoryStock;
			_numPicker.Value = game.m_Item2;
			_delete = view.FindViewById<ImageButton> (Resource.Id.delete);
			_delete.Click += delegate {
				List<GamesForCart> gameList = new List<GamesForCart>();
				foreach(GamesForCart g in tempCart.Games){
					if(!g.m_Item1.GameName.Equals(_detailedGame.GameName)){
						gameList.Add(g);
					}
				}
				tempCart.Games = gameList;
				UserSessionInfo.setUserCart(tempCart);
				AddedToCart.Invoke (this, new EventArgs ());
				this.Dismiss();
			};
			_numPicker.ValueChanged += delegate {
				onValueChange(_numPicker.Value);	
			};

			_addToCart = view.FindViewById<Button> (Resource.Id.updateCart);
			_addToCart.Click += delegate {
				game.m_Item2 = _numPicker.Value;
				UserSessionInfo.setUserCart(tempCart);
				AddedToCart.Invoke (this, new EventArgs ());
				this.Dismiss();
			};

			return view;
		}
		public void onValueChange(int newVal){
			_priceText.Text = "Price: $" + (_detailedGame.Price * newVal);
		}
		public void setGame(Game detailedGame)
		{
			_detailedGame = detailedGame;
		}
	}
}

