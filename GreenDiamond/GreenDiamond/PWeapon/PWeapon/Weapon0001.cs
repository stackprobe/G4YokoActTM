using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;
using Charlotte.Utils;

namespace Charlotte.PWeapon.PWeapon
{
	public class Weapon0001 : AWeapon
	{
		public Weapon0001(double x, double y, bool facingLeft)
		{
			this.X = x;
			this.Y = y;
			this.FacingLeft = facingLeft;
		}

		public override bool EachFrame()
		{
			if (this.CrashedEnemy != null)
				return false;

			this.X += 8.0 * (this.FacingLeft ? -1 : 1);

			this.Crash = CrashUtils.Circle(new D2Point(this.X, this.Y), 5.0);

			return DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y), 100.0) == false;
		}

		public override void Draw()
		{
			DDDraw.DrawBegin(DDGround.GeneralResource.Dummy, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
			DDDraw.DrawZoom(0.1);
			DDDraw.DrawEnd();
		}
	}
}
