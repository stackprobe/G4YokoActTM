using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Game01.Enemy01.Enemy01;

namespace Charlotte.Game01.Enemy01
{
	public class EnemySpecLoader
	{
		public string Name;
		public Func<IEnemySpec> CreateSpec;
	}
}
