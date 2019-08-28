using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.PEnemy
{
	public class EnemyLoader
	{
		public string Name;
		public Func<Enemy> CreateEnemy;
	}
}
