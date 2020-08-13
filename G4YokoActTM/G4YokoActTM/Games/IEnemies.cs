using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Games
{
	public static class IEnemies
	{
		public static IEnemy Load(IEnemy enemy, double x, double y)
		{
			return Load(enemy, new D2Point(x, y));
		}

		public static IEnemy Load(IEnemy enemy, D2Point pt)
		{
			enemy.Loaded(pt);
			return enemy;
		}
	}
}
