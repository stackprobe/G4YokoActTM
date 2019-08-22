using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Game01.Enemy01.Enemy01;
using Charlotte.Tools;
using Charlotte.Game01.Map01;
using Charlotte.Common;

namespace Charlotte.Game01.Enemy01
{
	public static class EnemyManager
	{
		public static void INIT()
		{
			Add("Enemy0001", () => new Enemy0001());
			Add("Enemy0002", () => new Enemy0002());

			// 新しい敵をここへ追加...
		}

		private static void Add(string name, Func<AEnemy> createEnemy)
		{
			EnemyLoader loader = new EnemyLoader()
			{
				Name = name,
				CreateEnemy = createEnemy,
			};

			Names.Add(name);
			EnemyLoaders.Add(name, loader);
		}

		private static List<string> Names = new List<string>();
		private static Dictionary<string, EnemyLoader> EnemyLoaders = DictionaryTools.CreateIgnoreCase<EnemyLoader>();

		public static EnemyLoader GetEnemyLoader(string name)
		{
			if (EnemyLoaders.ContainsKey(name) == false)
				return null;

			return EnemyLoaders[name];
		}

		public static List<string> GetNames()
		{
			return Names;
		}

		public static int GetCount()
		{
			return Names.Count;
		}
	}
}
