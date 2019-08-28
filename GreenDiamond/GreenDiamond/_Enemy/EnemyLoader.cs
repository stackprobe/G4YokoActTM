using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte._Enemy
{
	public class EnemyLoader
	{
		public string Name;
		public Func<AEnemy> CreateEnemy;
	}
}
