
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
	[Activity (Label = "Cart", Theme="@android:style/Theme.Black.NoTitleBar")]
	public class CartDetailFragment : DialogFragment
	{
		private Game _detailedGame;
		private TextView _priceText;
		private TextView _gmName;
		private NumberPicker _numPicker;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Dialog.SetTitle ("Details");

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.CartDetailsLayout,container, false);
			_priceText = view.FindViewById<TextView> (Resource.Id.itemPrice);
			_priceText.Text = "$" + _detailedGame.Price.ToString();

			_gmName = view.FindViewById<TextView> (Resource.Id.gameName);
			_gmName.Text = _detailedGame.GameName;

			_numPicker = view.FindViewById<NumberPicker> (Resource.Id.numberPicker1);
			_numPicker.MinValue = 1;
			_numPicker.MaxValue = _detailedGame.InventoryStock;
			_numPicker.ValueChanged += delegate {
				onValueChange(_numPicker.Value);	
			};
			Button update = view.FindViewById<Button> (Resource.Id.updateCart);
			return view;
		}
		public void onValueChange(int newVal){
			_priceText.Text = "$" + (_detailedGame.Price * newVal);
		}
		public void setGame(Game detailedGame)
		{
			_detailedGame = detailedGame;
		}
	}
}

