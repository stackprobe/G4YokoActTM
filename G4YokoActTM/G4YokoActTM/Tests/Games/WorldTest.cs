using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;

namespace Charlotte.Tests.Games
{
	public class WorldTest
	{
		public void Test01()
		{
			using (World world = new World())
			{
				//world.MapFile = "t0001.txt";
				world.Perform();
			}
		}
	}
}
