using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;

namespace Charlotte.Game01.Weapon01.Weapon01
{
	public class Weapon0001 : AWeapon
	{
		public double XSpeed;

		public Weapon0001(double x, double y, bool left)
		{
			this.X = x;
			this.Y = y;
			this.XSpeed = 8.0 * (left ? -1 : 1);
		}

		public override bool EachFrame()
		{
			this.X += this.XSpeed;

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
