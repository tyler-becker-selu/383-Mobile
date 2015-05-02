using System;
using Android.App;

namespace CustomerConsumer
{
	public class UserSessionInfo : Application
	{
		private static bool SearchTag;
		private static int UserId;
		private static string ApiKey;
		private static Cart UserCart;
		private static int CartId;


		public void onCreate(){
			this.onCreate ();
			SearchTag = false;
			UserId = 0;
			ApiKey = "";
			UserCart = null;
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

		public static Cart getUserCart(){
			return UserCart;
		}
		public static void setUserCart(Cart userCart) {
			UserSessionInfo.UserCart = userCart;
		}
		public static int getCartId(){
			return CartId;
		}

		public static void setCartId(int id) {
			UserSessionInfo.CartId = id;
		}

	}
}

