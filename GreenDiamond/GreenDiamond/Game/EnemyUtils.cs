using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Game
{
	public static class EnemyUtils
	{
		public static List<Enemy> Enemies = new List<Enemy>();

		public static Enemy GetEnemy(string name)
		{
			return Enemies.FirstOrDefault(v => v.Name == name);
		}
	}
}
