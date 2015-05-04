
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
using RestSharp.Serializers;

namespace CustomerConsumer
{
	
	public class GamesDetailFragment : DialogFragment
	{
		private RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/");
		private RestSharp.Deserializers.JsonDeserializer _deserializer = new RestSharp.Deserializers.JsonDeserializer();
		private Game _detailedGame;
		private TextView _priceText;
		private TextView _gmName;
		private NumberPicker _numPicker;
		private Button _addToCart;
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
			var view = inflater.Inflate (Resource.Layout.GamesDetailsLayout,container, false);
			_priceText = view.FindViewById<TextView> (Resource.Id.itemPrice);
			_priceText.Text = "$" + _detailedGame.Price.ToString();

			_gmName = view.FindViewById<TextView> (Resource.Id.gameName);
			_gmName.Text = _detailedGame.GameName;
			_numPicker = view.FindViewById<NumberPicker> (Resource.Id.numberPicker1);
			_numPicker.MinValue = 1;
			_numPicker.MaxValue = _detailedGame.InventoryStock;
			_numPicker.ValueChanged += delegate {
				onValueChange(_numPicker.Value);	
			};

			_addToCart = view.FindViewById<Button> (Resource.Id.addToCart);
			_addToCart.Click += delegate {
				Cart tempCart = UserSessionInfo.getUserCart();
				if(tempCart.Games == null )
				{
					var postRequest = new RestRequest ("api/Carts/",Method.POST);
					APIHeaders (postRequest);

					postRequest.AddHeader ("Content-Type","application/json");

					Game tempGame = new Game();
					tempGame = _detailedGame;

					List<Game> thisBreaksTheCode = new List<Game>();
					thisBreaksTheCode.Add(tempGame);
					var serializer = new RestSharp.Serializers.JsonSerializer();
					postRequest.AddParameter("text/json",serializer.Serialize(thisBreaksTheCode), ParameterType.RequestBody );

					var postResponse = client.Execute (postRequest);

					if(postResponse.StatusCode == System.Net.HttpStatusCode.OK){
						var cartRequest = new RestRequest ("api/Carts/" + UserSessionInfo.getUserId (), Method.GET);
						APIHeaders (cartRequest);
						cartRequest.RequestFormat = DataFormat.Json;

						Cart userCart;

						var cartResponse = client.Execute (cartRequest);
						userCart = _deserializer.Deserialize<Cart> (cartResponse);
						UserSessionInfo.setUserCart(userCart);
					}

				}
				else if(tempCart.Games.Find(r=> r.m_Item1.URL.Equals(_detailedGame.URL))==null)
				{
					GamesForCart tempGame = new GamesForCart();
					tempGame.m_Item1 = _detailedGame;
					tempGame.m_Item2 = _numPicker.Value;
					tempCart.Games.Add(tempGame);
					UserSessionInfo.setUserCart(tempCart);
				}
				else
				{
					tempCart.Games.Find(r => r.m_Item1.URL.Equals(_detailedGame.URL)).m_Item2 = _numPicker.Value;
					UserSessionInfo.setUserCart(tempCart);
				}
				this.Dismiss();
			};


			return view;
		}
		public void onValueChange(int newVal){
			_priceText.Text = "$" + (_detailedGame.Price * newVal);
		}
		public void setGame(Game detailedGame)
		{
			_detailedGame = detailedGame;
		}
	}
}

