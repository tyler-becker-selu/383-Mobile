
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
	public class GamesDetailFragment : DialogFragment
	{
		private string _title;
		private decimal _price;
		private TextView priceText;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Dialog.SetTitle (_title);

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.GamesDetailsLayout,container, false);
			priceText = view.FindViewById<TextView> (Resource.Id.itemPrice);
			priceText.Text = _price.ToString();

			return view;
		}

		public void setTitle(string title)
		{
			_title = title;
		}
		public void setPrice(decimal price)
		{
			_price = price;
		}
	}
}

