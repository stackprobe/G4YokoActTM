using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Game01.Enemy01
{
	public static class EnemyUtils
	{
		public static List<Enemy> Enemies = new List<Enemy>();

		public static void Add(Enemy enemy)
		{
			Enemies.Add(enemy);
		}

		public static Enemy GetEnemy(string name)
		{
			if (string.IsNullOrEmpty(name))
				return null;

			return Enemies.First(v => v.Name == name);
		}
	}
}
