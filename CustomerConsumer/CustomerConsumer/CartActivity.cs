
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
	[Activity (Label = "Cart")]			
	public class CartActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			try{
				SetContentView (Resource.Layout.Cart);
				double total = 0;
				int rand = new Random().Next(1, 70);
				TableLayout layout = FindViewById<TableLayout>(Resource.Id.buttonLayoutCart);
				for(int i = 1; i <= rand; ++i){
					TableRow tRow = new TableRow(this);
					layout.AddView(tRow);
					Button button = new Button(this);
					button.LayoutParameters = new TableRow.LayoutParams(
						TableRow.LayoutParams.MatchParent,
						TableRow.LayoutParams.WrapContent,
						.5f);
					tRow.AddView (button);
					button.Text = string.Format("Game "+i);
					TextView price = new TextView(this);
					price.LayoutParameters = new TableRow.LayoutParams(
						TableRow.LayoutParams.MatchParent,
						TableRow.LayoutParams.WrapContent,
						.5f);
					price.TextSize = 20;
					price.Gravity = GravityFlags.Right;
					tRow.AddView (price);
					total = total + (i*10+4.99);
					price.Text = string.Format("$"+i+"4.99");
				}
				TextView totalText = FindViewById<TextView>(Resource.Id.totalForCart);
				totalText.Text = string.Format("$"+total);
			}
			catch (Exception ex){
				string x = ex.Message;
			}
		}
		public void addButton(int i){

		}
		public void processClick(int i){
			//do button stuff here
		}
	}
}

