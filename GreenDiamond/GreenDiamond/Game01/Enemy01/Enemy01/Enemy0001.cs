using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Game01.Enemy01.Enemy01
{
	public class Enemy0001 : IEnemySpec
	{
		public void Created(Enemy enemy)
		{
			// noop
		}

		public void Destroy(Enemy enemy)
		{
			// noop
		}

		public bool EachFrame(Enemy enemy)
		{
			return false; // noop
		}

		public void Draw(Enemy enemy)
		{
			// noop
		}
	}
}
