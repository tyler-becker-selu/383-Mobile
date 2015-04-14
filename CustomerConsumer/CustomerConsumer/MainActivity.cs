﻿using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace CustomerConsumer
{
	[Activity (Label = "CustomerConsumer", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
<<<<<<< Updated upstream
		private Button loginBTN;

		int count = 1;
=======
		//int count = 1;
>>>>>>> Stashed changes

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			loginBTN = FindViewById<Button> (Resource.Id.LoginBTN);
			loginBTN.Click += (object sender, EventArgs e) => {
				FragmentTransaction transaction = FragmentManager.BeginTransaction ();
				DialogLogin loginDialog = new DialogLogin ();
				loginDialog.Show (transaction, "dialog fragment");
			};
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Games);

			// Get our button from the layout resource,
			// and attach an event to it
	//		Button button = FindViewById<Button> (Resource.Id.myButton);
			
<<<<<<< Updated upstream
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
			};

=======
	//		button.Click += delegate {
	//			button.Text = string.Format ("{0} clicks!", count++);
	//		};
>>>>>>> Stashed changes
		}
	}
}


