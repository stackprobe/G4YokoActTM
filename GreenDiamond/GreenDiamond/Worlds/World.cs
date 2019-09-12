using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;

namespace Charlotte.Worlds
{
	public class World
	{
		public string MapFile;

		// <---- prm

		public void Perform()
		{
			using (Game game = new Game())
			{
				game.Map = MapLoader.Load(MapFile);
				game.Status = new Status();

				// TODO
			}
		}
	}
}
