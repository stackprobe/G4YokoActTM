using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.PGame;
using Charlotte.PMap;
using Charlotte.PStatus;

namespace Charlotte.Tests
{
	public class Test0002
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
