
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

namespace CustomerConsumer
{
	[Activity (Label = "Menu")]			
	public class MenuActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Menu);

			Button games = FindViewById<Button> (Resource.Id.goToGames);
			games.Click += (sender, e) => {
				var intent = new Intent(this, typeof(GameActivity));
				StartActivity(intent);
			};
			Button cart = FindViewById<Button> (Resource.Id.goToCart);
			cart.Click += (sender, e) => {
				var intent = new Intent(this, typeof(CartActivity));
				StartActivity(intent);
			};
		}
	}
}

