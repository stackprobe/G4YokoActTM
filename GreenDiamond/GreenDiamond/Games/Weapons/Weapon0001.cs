using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;
using Charlotte.Game3Common;

namespace Charlotte.Games.Weapons
{
	public class Weapon0001 : IWeapon
	{
		private double X;
		private double Y;
		private bool FacingLeft;

		public void Loaded(D2Point pt, bool facingLeft)
		{
			this.X = pt.X;
			this.Y = pt.Y;
			this.FacingLeft = facingLeft;
		}

		public bool IsFacingLeft()
		{
			return this.FacingLeft;
		}

		public bool EachFrame()
		{
			this.X += 8.0 * (this.FacingLeft ? -1 : 1);

			if (Game.I.Map.GetCellByPixelPoint(this.X, this.Y).Wall)
			{
				EffectUtils.小爆発(this.X, this.Y);
				return false;
			}
			return DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 100.0) == false;
		}

		public Crash GetCrash()
		{
			return CrashUtils.Circle(new D2Point(this.X, this.Y), 5.0);
		}

		public bool Crashed(IEnemy enemy)
		{
			EffectUtils.小爆発(this.X, this.Y);
			return false;
		}

		public int GetAttackPoint()
		{
			return 1;
		}

		public void Draw()
		{
			DDDraw.DrawBegin(DDGround.GeneralResource.Dummy, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
			DDDraw.DrawZoom(0.1);
			DDDraw.DrawEnd();
		}
	}
}
