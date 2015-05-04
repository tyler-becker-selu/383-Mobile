using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using System.Linq;
using Android.Views;

namespace CustomerConsumer
{
	public class GamesForCartsAdapter : BaseAdapter<GamesForCart>
	{
		List<GamesForCart> _cartGamesList;
		Activity _context;

		public GamesForCartsAdapter (Activity context, List<GamesForCart> cartGamesList):base()
		{
			_context = context;
			_cartGamesList = cartGamesList;
		}
		public override int Count
		{
			get{
					return _cartGamesList.Count ();
				}

		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override GamesForCart this[int position]
		{
			get{ return _cartGamesList [position];}
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			string totallyLegitStyler="";
			if (view == null) // otherwise create a new one
				view = _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);

			if (_cartGamesList [position].m_Item1.Price.ToString ().Length < 4)
				totallyLegitStyler = "\t\t\t   ";
			
			view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = _cartGamesList [position].m_Item1.GameName;
			view.FindViewById<TextView> (Android.Resource.Id.Text2).Text ="Quantity: " + _cartGamesList [position].m_Item2+"\t\t\t Unit Price: $"+_cartGamesList[position].m_Item1.Price+totallyLegitStyler+"\t\t\t Total: $"+(_cartGamesList [position].m_Item2*_cartGamesList[position].m_Item1.Price);
			view.FindViewById<TextView> (Android.Resource.Id.Text1).SetTextColor (Android.Graphics.Color.ParseColor ("#000000"));
			view.FindViewById<TextView> (Android.Resource.Id.Text2).SetTextColor (Android.Graphics.Color.ParseColor ("#6D6D6D"));
			return view;
		}
	}
}

