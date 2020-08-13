using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;
using Charlotte.Games.Enemies;
using Charlotte.Games.Enemies.Objects;

namespace Charlotte.Games
{
	public static class EnemyManager
	{
		public static void INIT()
		{
			Add("Enemy0001", () => new Enemy0001());
			Add("Enemy0002", () => new Enemy0002());
			Add("StartPoint00", () => new StartPoint(0));
			Add("StartPoint01", () => new StartPoint(1));
			Add("StartPoint02", () => new StartPoint(2));
			Add("StartPoint03", () => new StartPoint(3));
			Add("StartPoint04", () => new StartPoint(4));
			Add("StartPoint05", () => new StartPoint(5));
			Add("StartPoint06", () => new StartPoint(6));
			Add("StartPoint07", () => new StartPoint(7));
			Add("StartPoint08", () => new StartPoint(8));
			Add("StartPoint09", () => new StartPoint(9));

			// 新しい敵をここへ追加...
		}

		private static void Add(string name, Func<IEnemy> createEnemy)
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
