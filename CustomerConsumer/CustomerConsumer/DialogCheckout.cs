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
		public override View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			Dialog.SetTitle ("Checkout");
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.DialogCheckoutLayout,container, false);
			TextView text = view.FindViewById<TextView> (Resource.Id.cartID);
			text.TextSize = 100;

			text.Text = "" + UserSessionInfo.getCartId();
			return view;
		}
	}
}


