using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;
using Charlotte.Game3Common;

namespace Charlotte.Games.Enemies
{
	public class Enemy0002 : IEnemy
	{
		private double X;
		private double Y;

		public void Loaded(D2Point pt)
		{
			this.X = pt.X;
			this.Y = pt.Y;
		}

		public bool EachFrame()
		{
			const double SPEED = 2.0;

			switch (Game.I.Frame / 60 % 4)
			{
				case 0: this.X += SPEED; break;
				case 1: this.Y += SPEED; break;
				case 2: this.X -= SPEED; break;
				case 3: this.Y -= SPEED; break;

				default:
					throw null; // never
			}
			return true;
		}

		public Crash GetCrash()
		{
			return CrashUtils.Rect_CenterSize(new D2Point(this.X, this.Y), new D2Size(100.0, 100.0));
		}

		public int HP = 10;

		public bool Crashed(IWeapon weapon)
		{
			this.X += 10.0 * (weapon.IsFacingLeft() ? -1 : 1); // ヒットバック

			this.HP -= weapon.GetAttackPoint();

			if (this.HP <= 0) // ? dead
			{
				EffectUtils.中爆発(this.X, this.Y);
				return false;
			}
			return true;
		}

		public bool CrashedToPlayer()
		{
			return true;
		}

		public int GetAttackPoint()
		{
			return 3;
		}

		public void Draw()
		{
			DDDraw.SetBright(1.0, 0.5, 0.0);
			DDDraw.DrawBegin(DDGround.GeneralResource.WhiteBox, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
			DDDraw.DrawSetSize(100.0, 100.0);
			DDDraw.DrawEnd();
			DDDraw.Reset();
		}
	}
}
