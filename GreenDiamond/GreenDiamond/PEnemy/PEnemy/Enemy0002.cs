using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;
using Charlotte.Utils;

namespace Charlotte.PEnemy.PEnemy
{
	public class Enemy0002 : AEnemy
	{
		public override bool EachFrame()
		{
			const double SPEED = 2.0;

			switch (this.Frame / 60 % 4)
			{
				case 0: this.X += SPEED; break;
				case 1: this.Y += SPEED; break;
				case 2: this.X -= SPEED; break;
				case 3: this.Y -= SPEED; break;

				default:
					throw null; // never
			}

			if (this.CrashedWeapon != null)
			{
				this.X += 10.0 * (this.CrashedWeapon.FacingLeft ? -1 : 1); // ヒットバック
			}
			this.Crash = CrashUtils.Rect_CenterSize(new D2Point(this.X, this.Y), new D2Size(100.0, 100.0));

			return true;
		}

		public override void Draw()
		{
			DDDraw.SetBright(1.0, 0.5, 0.0);
			DDDraw.DrawBegin(DDGround.GeneralResource.WhiteBox, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
			DDDraw.DrawSetSize(100.0, 100.0);
			DDDraw.DrawEnd();
			DDDraw.Reset();
		}
	}
}
