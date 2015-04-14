using System;

using Xamarin.Forms;

namespace CustomerConsumer
{
	public class Genre : ContentPage
	{
		public Genre ()
		{
			Content = new StackLayout { 
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}


