using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Enemy01;
using Charlotte.Utils;

namespace Charlotte.Weapon01
{
	public abstract class AWeapon
	{
		public int Frame = 0;
		public double X = -10000.0;
		public double Y = -10000.0;
		public Crash Crash = CrashUtils.None();
		public bool FacingLeft = false;
		public int AttackPoint = 1;
		public AEnemy CrashedEnemy = null;

		public abstract bool EachFrame(); // ret: ? ! 破棄
		public abstract void Draw();

		public void Crashed(AEnemy enemy)
		{
			this.CrashedEnemy = enemy;
		}

		public void PostEachFrame()
		{
			this.Frame++;
		}
	}
}
