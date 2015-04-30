
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
		private string _name;
		private decimal _price;
		private TextView priceText;
		private TextView gmName;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Dialog.SetTitle ("Details");

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.GamesDetailsLayout,container, false);
			priceText = view.FindViewById<TextView> (Resource.Id.itemPrice);
			priceText.Text = _price.ToString();
			gmName = view.FindViewById<TextView> (Resource.Id.gameName);
			gmName.Text = _name;

			return view;
		}

		public void setName(string name)
		{
			_name = name;
		}
		public void setPrice(decimal price)
		{
			_price = price;
		}
	}
}

