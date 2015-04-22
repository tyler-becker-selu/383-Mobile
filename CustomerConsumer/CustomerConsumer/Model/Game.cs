using System;
using System.Collections.Generic;

namespace CustomerConsumer
{
	public class Game
	{
			public string URL { get; set; }
			public int Id { get; set; }

			
			public string GameName { get; set; }

			public DateTime ReleaseDate { get; set; }
			public decimal Price { get; set; }

			public int InventoryStock { get; set; }
			public virtual List<Genre> Genres { get; set; }
			public virtual List<Tag> Tags { get; set; }
	}
}

