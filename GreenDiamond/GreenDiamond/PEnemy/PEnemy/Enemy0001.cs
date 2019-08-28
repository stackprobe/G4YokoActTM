using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;
using Charlotte.PGame;

namespace Charlotte.PEnemy.PEnemy
{
	public class Enemy0001 : AEnemy
	{
		private D2Point Speed = new D2Point();

		public override bool EachFrame()
		{
			double rot = DDUtils.GetAngle(Game.I.Player.X - this.X, Game.I.Player.Y - this.Y);
			rot += DDUtils.Random.Real2() * 0.05;
			D2Point speedAdd = DDUtils.AngleToPoint(rot, 0.1);

			if (DDUtils.GetDistance(Game.I.Player.X - this.X, Game.I.Player.Y - this.Y) < 50.0)
			{
				speedAdd *= -300.0;
			}
			this.Speed += speedAdd;
			this.Speed *= 0.93;

			this.X += this.Speed.X;
			this.Y += this.Speed.Y;

			this.CrashedWeapon = null;

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
