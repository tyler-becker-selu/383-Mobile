using System;
using Android.App;

namespace CustomerConsumer
{
	public class UserSessionInfo : Application
	{
		private static int UserId;
		private static string ApiKey;

		public void onCreate(){
			this.onCreate ();
			UserId = 0;
			ApiKey = "";
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

	}
}

