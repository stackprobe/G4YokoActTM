using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;

namespace Charlotte.Tests.Games
{
	public class GameTest
	{
		public void Test01()
		{
			using (Game game = new Game())
			{
				game.Map = MapLoader.Load(@"Map\t0001.txt");
				game.Status = new Status();
				game.Perform();
			}
		}
	}
}
