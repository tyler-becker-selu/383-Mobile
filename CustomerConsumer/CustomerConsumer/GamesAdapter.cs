using System;
using System.Collections.Generic;
using Android.Widget;
using Android.App;
using Android.Views;
using System.Linq;


namespace CustomerConsumer
{
	public class GamesAdapter : BaseAdapter<Game>
	{
		List<Game> games;
		Activity context;

		public GamesAdapter (Activity context, List<Game> games):base()
		{
			this.context = context;
			this.games = games;
		}
		public override int Count {
			get {
				return games.Count ();
			}
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override Game this[int position] {  
			get { return games[position]; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
			
			view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = games[position].GameName;
			view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = "$"+games[position].Price;
			view.FindViewById<TextView> (Android.Resource.Id.Text1).SetTextColor (Android.Graphics.Color.ParseColor ("#000000"));
			view.FindViewById<TextView> (Android.Resource.Id.Text2).SetTextColor (Android.Graphics.Color.ParseColor ("#6D6D6D"));
			return view;
		}
	}
}

