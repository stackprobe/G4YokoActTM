using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Game01.Enemy01.Enemy01;
using Charlotte.Tools;
using Charlotte.Game01.Map01;

namespace Charlotte.Game01.Enemy01
{
	public static class EnemyManager
	{
		public static void INIT()
		{
			Add("Enemy0001", () => new Enemy0001());

			// 新しい敵をここへ追加...
		}

		private static void Add(string name, Func<IEnemySpec> getEnemySpec)
		{
			EnemySpecLoader loader = new EnemySpecLoader()
			{
				Name = name,
				CreateSpec = getEnemySpec,
			};

			Names.Add(name);
			SpecLoaders.Add(name, loader);
		}

		private static List<string> Names = new List<string>();
		private static Dictionary<string, EnemySpecLoader> SpecLoaders = DictionaryTools.CreateIgnoreCase<EnemySpecLoader>();

		private static EnemySpecLoader GetSpecLoader(string name)
		{
			if (string.IsNullOrEmpty(name))
				return null;

			return SpecLoaders[name];
		}

		private static IEnemySpec CreateSpec(string name)
		{
			EnemySpecLoader loader = GetSpecLoader(name);

			if (loader == null)
				return null;

			return loader.CreateSpec();
		}

		public static Enemy Create(string name, MapCell cell, int x, int y)
		{
			IEnemySpec spec = CreateSpec(name);

			if (spec == null)
				return null;

			Enemy enemy = new Enemy()
			{
				Name = name,

				HomeCell = cell,
				HomeCellTablePoint = new I2Point(x, y),

				Spec = spec,
			};

			enemy.Created();

			return enemy;
		}

		public static IEnumerable<string> GetNames()
		{
			return Names;
		}

		public static int GetCount()
		{
			return Names.Count;
		}
	}
}
