using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;

namespace Charlotte.Game01.Enemy01.Enemy01
{
	public class Enemy0001 : AEnemy
	{
		public override bool EachFrame()
		{
			double rot = DDUtils.GetAngle(Game.I.Player.X - this.X, Game.I.Player.Y - this.Y);
			D2Point speed = DDUtils.AngleToPoint(rot, 2.0);

			this.X += speed.X;
			this.Y += speed.Y;

			return true;
		}

		public override void Draw()
		{
			DDDraw.DrawBegin(DDGround.GeneralResource.Dummy, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
			DDDraw.DrawRotate(DDEngine.ProcFrame / 10.0);
			DDDraw.DrawEnd();
		}
	}
}
