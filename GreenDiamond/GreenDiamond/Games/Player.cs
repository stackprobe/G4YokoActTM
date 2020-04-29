using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;
using Charlotte.Games.Weapons;
using Charlotte.Game3Common;

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
		public SceneKeeper DeadScene = new SceneKeeper(180);
		public SceneKeeper DamageScene = new SceneKeeper(20);
		public SceneKeeper MutekiScene = new SceneKeeper(60);
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

			if (this.DeadScene.IsFlaming())
			{
				int koma = IntTools.ToRange(this.DeadScene.Count / 20, 0, 1);

				if (this.TouchGround)
					koma *= 2;

				koma *= 2;
				koma++;

				picture = Ground.I.Picture.PlayerDamage[koma];

				DDDraw.SetTaskList(DDGround.EL);
			}
			if (this.DamageScene.IsFlaming())
			{
				picture = Ground.I.Picture.PlayerDamage[0];
				xZoom *= -1;
			}

			if (this.DamageScene.IsFlaming() || this.MutekiScene.IsFlaming())
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
			this.HP -= enemy.GetAttackPoint();

			if (this.HP <= 0)
				this.DeadScene.Fire();
			else
				this.DamageScene.Fire();
		}

		public void Attack()
		{
			// 将来的に武器毎にコードが実装され、メソッドがでかくなると思われる。

			if (this.AttackFrame % 6 == 1)
			{
				double x = this.X;
				double y = this.Y;

				x += 30.0 * (this.FacingLeft ? -1 : 1);

				if (1 <= this.ShagamiFrame)
					y += 10.0;
				else
					y -= 4.0;

				Game.I.AddWeapon(IWeapons.Load(new Weapon0001(), x, y, this.FacingLeft));
			}
		}
	}
}
