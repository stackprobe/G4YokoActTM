using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Game01.Enemy01
{
	public static class EnemyLoader
	{
		public static void INIT()
		{
			EnemyUtils.Add(new Enemy()
			{
				Name = "Dummy",
			});

			// TODO
		}
	}
}
