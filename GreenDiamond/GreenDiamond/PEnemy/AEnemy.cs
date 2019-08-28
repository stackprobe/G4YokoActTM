using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Utils;
using Charlotte.PWeapon;
using Charlotte.PMap;

namespace Charlotte.PEnemy
{
	public abstract class AEnemy
	{
		public int Frame = 0;
		public double X = -10000.0;
		public double Y = -10000.0;
		public Crash Crash = CrashUtils.None();
		public int HP = 1;
		public int AttackPoint = 1;
		public AWeapon CrashedWeapon = null;
		public bool CrashedToPlayerFlag = false;

		public void SetTablePoint(I2Point pt)
		{
			this.X = pt.X * MapTile.WH + MapTile.WH / 2;
			this.Y = pt.Y * MapTile.WH + MapTile.WH / 2;
		}

		public abstract bool EachFrame(); // ret: ? ! 破棄
		public abstract void Draw();

		public void Crashed(AWeapon weapon)
		{
			this.HP -= weapon.AttackPoint;
			this.CrashedWeapon = weapon;
		}

		public void CrashedToPlayer()
		{
			this.CrashedToPlayerFlag = true;
		}

		public void PostEachFrame()
		{
			this.Frame++;
			this.CrashedWeapon = null;
			this.CrashedToPlayerFlag = false;
		}
	}
}
