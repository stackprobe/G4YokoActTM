using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Utils;

namespace Charlotte.Games
{
	public abstract class Weapon
	{
		public int Frame = 0;
		public double X = -10000.0;
		public double Y = -10000.0;
		public Crash Crash = CrashUtils.None();
		public bool FacingLeft = false;
		public int AttackPoint = 1;
		public Enemy CrashedEnemy = null;

		public abstract bool EachFrame(); // ret: ? ! 破棄
		public abstract void Draw();

		public void Crashed(Enemy enemy)
		{
			this.CrashedEnemy = enemy;
		}

		public void PostEachFrame()
		{
			this.Frame++;
		}
	}
}
