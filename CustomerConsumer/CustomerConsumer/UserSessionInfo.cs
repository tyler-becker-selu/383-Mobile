using System;
using Android.App;

namespace CustomerConsumer
{
	public class UserSessionInfo : Application
	{
		private static int UserId;
		private static string ApiKey;
		private static Cart UserCart;

		public void onCreate(){
			this.onCreate ();
			UserId = 0;
			ApiKey = "";
			UserCart = null;
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

	}
}

