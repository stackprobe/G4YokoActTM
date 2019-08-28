using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.PEnemy.PObject
{
	public class StartPoint : Enemy
	{
		public int Index;

		public StartPoint(int index)
		{
			this.Index = index;
		}

		public override bool EachFrame()
		{
			return false; // noop
		}

		public override void Draw()
		{
			// noop
		}
	}
}
