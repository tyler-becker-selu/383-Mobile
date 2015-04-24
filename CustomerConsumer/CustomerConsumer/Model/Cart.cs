using System;
using System.Collections.Generic;

namespace CustomerConsumer
{
	public class Cart
	{
		public string URL { get; set; }
		public bool CheckoutReady { get; set; }
		public int User_Id	{ get; set; }
		public virtual List<GamesForCart> Games { get; set; }
	}

}

