using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Games
{
	public class EnemyLoader
	{
		public string Name;
		public Func<IEnemy> CreateEnemy;
	}
}
