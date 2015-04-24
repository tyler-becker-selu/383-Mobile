using System;
using System.Collections.Generic;

namespace CustomerConsumer
{
	public class MItem1
	{
		public string URL { get; set; }
		public string GameName { get; set; }
		public string ReleaseDate { get; set; }
		public decimal Price { get; set; }
		public int InventoryStock { get; set; }
		public List<Genre> Genres { get; set; }
		public List<Tag> Tags { get; set; }
	}
}

