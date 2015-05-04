using System;
using Android.App;
using RestSharp;
using Android.Content;

namespace CustomerConsumer
{
	public class UserSessionInfo : Application
	{
		private static bool SearchTag;
		private static int UserId;
		private static int CartId;
		private static string ApiKey;
		private static Cart UserCart;
		private static string Toast;
		private static RestClient Client=new RestClient("http://dev.envocsupport.com/GameStore4/");
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


		public void onCreate(){
			this.onCreate ();
			SearchTag = false;
			UserId = 0;
			UserId = 0;
			ApiKey = "";
			UserCart = null;
			Toast = "";
		}
			
		public static bool GetSearchTag(){
			return SearchTag;
		}

		public static void SetSearchTag(bool change) {
			UserSessionInfo.SearchTag = change;
		}

		public static string getApiKey(){
			return ApiKey;
		}

		public static void setApiKey(String key) {
			UserSessionInfo.ApiKey = key;
		}

		public static int getUserId(){
			return UserId;
		}

		public static void setUserId(int id) {
			UserSessionInfo.UserId = id;
		}
		public static int getCartId(){
			return CartId;
		}

		public static void setCartId(int id) {
			UserSessionInfo.CartId = id;
		}
		public static Cart getUserCart(){
			return UserCart;
		}
		public static void setUserCart(Cart userCart) {
			UserSessionInfo.UserCart = userCart;
		}
		public static string GetToast(){
			return Toast;
		}

		public static void SetToast(string toast) {
			UserSessionInfo.Toast = toast;
		}

		public static RestClient getClient(){
			return Client;
		}

		public static void Logout (){
			var cart = UserSessionInfo.getUserCart ();
			if (cart != null) {

				var request = new RestRequest ("api/Carts/" + UserSessionInfo.getUserId(), Method.PUT);
				APIHeaders (request);
				RestSharp.Serializers.JsonSerializer serial = new RestSharp.Serializers.JsonSerializer ();
				var json = serial.Serialize (cart);

				request.AddParameter ("text/json", json, ParameterType.RequestBody);

				var response = Client.Execute (request);
			}

			UserSessionInfo.setUserId (0);
			UserSessionInfo.setApiKey("");
			UserSessionInfo.setUserCart(null);
			UserSessionInfo.SetToast("");
		}
	}
}

