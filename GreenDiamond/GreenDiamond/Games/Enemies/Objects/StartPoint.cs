using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Game3Common;

namespace Charlotte.Games.Enemies.Objects
{
	public class StartPoint : IEnemy
	{
		public int Index;

		public StartPoint(int index)
		{
			this.Index = index;
		}

		public double X;
		public double Y;

		public void Loaded(D2Point pt)
		{
			this.X = pt.X;
			this.Y = pt.Y;
		}

		public bool EachFrame()
		{
			return false; // 即消滅
		}

		public Crash GetCrash()
		{
			return CrashUtils.None();
		}

		public bool Crashed(IWeapon weapon)
		{
			return false; // 即消滅
		}

		public bool CrashedToPlayer()
		{
			return false; // 即消滅
		}

		public int GetAttackPoint()
		{
			return 0;
		}

		public void Draw()
		{
			// noop
		}
	}
}
