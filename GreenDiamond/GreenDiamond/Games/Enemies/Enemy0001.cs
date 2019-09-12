using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;
using Charlotte.Game3Common;

namespace Charlotte.Games.Enemies
{
	public class Enemy0001 : IEnemy
	{
		private double X;
		private double Y;

		public void Loaded(D2Point pt)
		{
			this.X = pt.X;
			this.Y = pt.Y;
		}

		private D2Point Speed = new D2Point();

		public bool EachFrame()
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

			return true;
		}

		public Crash GetCrash()
		{
			return CrashUtils.None();
		}

		public bool Crashed(IWeapon weapon)
		{
			return true;
		}

		public bool CrashedToPlayer()
		{
			return true;
		}

		public int GetAttackPoint()
		{
			return 0;
		}

		public void Draw()
		{
			DDDraw.DrawBegin(DDGround.GeneralResource.Dummy, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
			DDDraw.DrawRotate(DDEngine.ProcFrame / 10.0);
			DDDraw.DrawEnd();
		}
	}
}
