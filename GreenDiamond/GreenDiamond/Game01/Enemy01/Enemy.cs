using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Game01.Enemy01
{
	public class Enemy
	{
		public string Name;

		public Enemy()
		{
			// TODO

			EnemyUtils.Add(this);
		}
	}
}
