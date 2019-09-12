using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;

namespace Charlotte.Games
{
	public class Player
	{
		public double X;
		public double Y;
		public double YSpeed;
		public bool FacingLeft;
		public int MoveFrame;
		public bool MoveSlow; // ? 低速移動
		public int JumpFrame;
		public bool TouchGround;
		public int AirborneFrame;
		public int ShagamiFrame;
		public int AttackFrame;
		public int DeadFrame;
		public int DamageFrame;
		public int MutekiFrame;
		public int HP = 1;

		private int PlayerLookLeftFrm = 0;

		public void Draw()
		{
			if (PlayerLookLeftFrm == 0 && DDUtils.Random.Real2() < 0.002) // キョロキョロするレート
				PlayerLookLeftFrm = 150 + (int)(DDUtils.Random.Real2() * 90.0);

			DDUtils.CountDown(ref PlayerLookLeftFrm);

			double xZoom = this.FacingLeft ? -1 : 1;

			// 立ち >

			DDPicture picture = Ground.I.Picture.PlayerStands[120 < PlayerLookLeftFrm ? 1 : 0][(DDEngine.ProcFrame / 20) % 2];

			if (1 <= this.MoveFrame)
			{
				if (this.MoveSlow)
				{
					picture = Ground.I.Picture.PlayerWalk[(DDEngine.ProcFrame / 10) % 2];
				}
				else
				{
					picture = Ground.I.Picture.PlayerDash[(DDEngine.ProcFrame / 5) % 2];
				}
			}
			if (this.TouchGround == false)
			{
				picture = Ground.I.Picture.PlayerJump[0];
			}
			if (1 <= this.ShagamiFrame)
			{
				picture = Ground.I.Picture.PlayerShagami;
			}

			// < 立ち

			// 攻撃中 >

			if (1 <= this.AttackFrame)
			{
				picture = Ground.I.Picture.PlayerAttack;

				if (1 <= this.MoveFrame)
				{
					if (this.MoveSlow)
					{
						picture = Ground.I.Picture.PlayerAttackWalk[(DDEngine.ProcFrame / 10) % 2];
					}
					else
					{
						picture = Ground.I.Picture.PlayerAttackDash[(DDEngine.ProcFrame / 5) % 2];
					}
				}
				if (this.TouchGround == false)
				{
					picture = Ground.I.Picture.PlayerAttackJump;
				}
				if (1 <= this.ShagamiFrame)
				{
					picture = Ground.I.Picture.PlayerAttackShagami;
				}
			}

			// < 攻撃中

			if (1 <= this.DeadFrame)
			{
				int koma = IntTools.Range(this.DeadFrame / 20, 0, 1);

				if (this.TouchGround)
					koma *= 2;

				koma *= 2;
				koma++;

				picture = Ground.I.Picture.PlayerDamage[koma];

				DDDraw.SetTaskList(DDGround.EL);
			}
			if (1 <= this.DamageFrame)
			{
				picture = Ground.I.Picture.PlayerDamage[0];
				xZoom *= -1;
			}

			if (1 <= this.DamageFrame || 1 <= this.MutekiFrame)
			{
				DDDraw.SetTaskList(DDGround.EL);
				DDDraw.SetAlpha(0.5);
			}
			DDDraw.DrawBegin(
					picture,
					DoubleTools.ToInt(this.X - DDGround.ICamera.X),
					DoubleTools.ToInt(this.Y - DDGround.ICamera.Y) - 16
					);
			DDDraw.DrawZoom_X(xZoom);
			DDDraw.DrawEnd();
			DDDraw.Reset();

			// debug
			{
				DDDraw.DrawBegin(DDGround.GeneralResource.Dummy, this.X - DDGround.ICamera.X, this.Y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(0.1);
				DDDraw.DrawRotate(DDEngine.ProcFrame * 0.01);
				DDDraw.DrawEnd();
			}
		}

		public void Crashed(IEnemy enemy)
		{
			// 同時に複数の敵と衝突すると、その分何度も呼ばれることに注意！

			if (this.DamageFrame != 0) // ? Already crashed
				return;

			this.HP -= enemy.GetAttackPoint();

			if (this.HP <= 0)
				this.DeadFrame = 1;
			else
				this.DamageFrame = 1;
		}
	}
}
