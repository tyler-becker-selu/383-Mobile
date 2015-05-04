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
using RestSharp;
using System.Net;

namespace CustomerConsumer
{
	public class DialogCheckout : DialogFragment
	{
		private RestClient client = new RestClient("http://dev.envocsupport.com/GameStore4/");
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
		public override View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			Dialog.SetTitle ("Checkout");
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.DialogCheckoutLayout,container, false);
			TextView text = view.FindViewById<TextView> (Resource.Id.cartID);
			text.TextSize = 100;
			Button button = view.FindViewById<Button> (Resource.Id.confirmation);
			button.Click += delegate {
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
					
				Intent myIntent = new Intent(Activity, typeof(GameActivity));
				Activity.StartActivity(myIntent);
				Activity.Finish();

			};
			text.Text = "" + UserSessionInfo.getCartId();
			return view;
		}
	}
}


