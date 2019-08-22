using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Game01.Map01;
using Charlotte.Tools;
using Charlotte.Game01.Map01.Tile01;

namespace Charlotte.Game01.Enemy01
{
	public abstract class AEnemy
	{
		public int Frame = 0;
		public double X = -10000.0;
		public double Y = -10000.0;

		public void SetTablePoint(I2Point pt)
		{
			this.X = pt.X * MapTile.WH + MapTile.WH / 2;
			this.Y = pt.Y * MapTile.WH + MapTile.WH / 2;
		}

		public abstract bool EachFrame(); // ret: ? ! 破棄
		public abstract void Draw();
	}
}
