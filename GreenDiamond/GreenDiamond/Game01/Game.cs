using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Tools;
using Charlotte.Common;
using Charlotte.Game01.Map01;
using Charlotte.Game01.Map01.Tile01;
using Charlotte.Game01.Enemy01;
using Charlotte.Game01.Edit01;
using Charlotte.Game01.Weapon01;
using Charlotte.Game01.Weapon01.Weapon01;
using Charlotte.Game01.Crash01;

namespace Charlotte.Game01
{
	public class Game : IDisposable
	{
		public Map Map;

		// <---- prm

		public static Game I = null;

		public Game()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public class PlayerInfo
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
			public Crash Crash = CrashUtils.None();
			public int DamageFrame;
		}

		public PlayerInfo Player = new PlayerInfo();

		public bool CamSlideMode; // ? モード中
		public int CamSlideCount;
		public int CamSlideX; // -1 ～ 1
		public int CamSlideY; // -1 ～ 1

		public void Perform()
		{
			this.ReloadEnemies();

			this.Player.X = this.Map.W * MapTile.WH / 2.0;
			this.Player.Y = this.Map.H * MapTile.WH / 2.0;

			DDGround.Camera.X = this.Player.X - DDConsts.Screen_W / 2.0;
			DDGround.Camera.Y = this.Player.Y - DDConsts.Screen_H / 2.0;

			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			// TODO music

			for (; ; )
			{
				DDUtils.Approach(ref DDGround.Camera.X, this.Player.X - DDConsts.Screen_W / 2 + (this.CamSlideX * DDConsts.Screen_W / 3), 0.8);
				DDUtils.Approach(ref DDGround.Camera.Y, this.Player.Y - DDConsts.Screen_H / 2 + (this.CamSlideY * DDConsts.Screen_H / 3), 0.8);

				DDUtils.Range(ref DDGround.Camera.X, 0.0, this.Map.W * MapTile.WH - DDConsts.Screen_W);
				DDUtils.Range(ref DDGround.Camera.Y, 0.0, this.Map.H * MapTile.WH - DDConsts.Screen_H);

				DDGround.ICamera.X = DoubleTools.ToInt(DDGround.Camera.X);
				DDGround.ICamera.Y = DoubleTools.ToInt(DDGround.Camera.Y);

				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_E) == 1)
				{
					this.EditMode();
					this.ReloadEnemies();
				}

				// プレイヤー入力
				{
					bool damage = 1 <= this.Player.DamageFrame;
					bool move = false;
					bool slow = false;
					bool camSlide = false;
					int jumpPress = DDInput.A.GetInput();
					bool jump = false;
					bool shagami = false;
					bool attack = false;

					if (!damage && 1 <= DDInput.DIR_2.GetInput())
					{
						shagami = true;
					}
					if (!damage && 1 <= DDInput.DIR_4.GetInput())
					{
						this.Player.FacingLeft = true;
						move = true;
					}
					if (!damage && 1 <= DDInput.DIR_6.GetInput())
					{
						this.Player.FacingLeft = false;
						move = true;
					}
					if (1 <= DDInput.L.GetInput())
					{
						move = false;
						camSlide = true;
					}
					if (!damage && 1 <= DDInput.R.GetInput())
					{
						slow = true;
					}
					if (!damage && 1 <= jumpPress)
					{
						jump = true;
					}
					if (!damage && 1 <= DDInput.B.GetInput())
					{
						attack = true;
					}
					if (DDKey.GetInput(DX.KEY_INPUT_Q) == 1)
					{
						break;
					}

					if (move)
					{
						this.Player.MoveFrame++;
						shagami = false;
					}
					else
						this.Player.MoveFrame = 0;

					this.Player.MoveSlow = move && slow;

					if (1 <= this.Player.JumpFrame)
					{
						if (jump && this.Player.JumpFrame < 22)
							this.Player.JumpFrame++;
						else
							this.Player.JumpFrame = 0;
					}
					else
					{
						//if (jump && jumpPress < 5 && this.Player.TouchGround)
						if (jump && jumpPress < 5 && this.Player.AirborneFrame < 5)
							this.Player.JumpFrame = 1;
					}

					if (camSlide)
					{
						if (DDInput.DIR_4.IsPound())
						{
							this.CamSlideCount++;
							this.CamSlideX--;
						}
						if (DDInput.DIR_6.IsPound())
						{
							this.CamSlideCount++;
							this.CamSlideX++;
						}
						if (DDInput.DIR_8.IsPound())
						{
							this.CamSlideCount++;
							this.CamSlideY--;
						}
						if (DDInput.DIR_2.IsPound())
						{
							this.CamSlideCount++;
							this.CamSlideY++;
						}
						DDUtils.Range(ref this.CamSlideX, -1, 1);
						DDUtils.Range(ref this.CamSlideY, -1, 1);
					}
					else
					{
						if (this.CamSlideMode && this.CamSlideCount == 0)
						{
							this.CamSlideX = 0;
							this.CamSlideY = 0;
						}
						this.CamSlideCount = 0;
					}
					this.CamSlideMode = camSlide;

					if (this.Player.TouchGround == false)
						shagami = false;

					if (shagami)
						this.Player.ShagamiFrame++;
					else
						this.Player.ShagamiFrame = 0;

					if (attack)
						this.Player.AttackFrame++;
					else
						this.Player.AttackFrame = 0;
				}

				if (1 <= this.Player.DamageFrame)
				{
					double rate = this.Player.DamageFrame / 20.0;

					if (rate < 1.0)
					{
						this.Player.X -= (9.0 - 6.0 * rate) * (this.Player.FacingLeft ? -1 : 1);
						this.Player.DamageFrame++;
					}
					else
						this.Player.DamageFrame = 0;
				}


				// プレイヤー移動
				{
					if (1 <= this.Player.MoveFrame)
					{
						double speed = 0.0;

						if (this.Player.MoveSlow)
						{
							speed = this.Player.MoveFrame * 0.2;
							DDUtils.Minim(ref speed, 2.0);
						}
						else
							speed = 6.0;

						speed *= this.Player.FacingLeft ? -1 : 1;

						this.Player.X += speed;
					}
					else
						this.Player.X = (double)DoubleTools.ToInt(this.Player.X);

					this.Player.YSpeed += 1.0; // += 重力加速度

					if (1 <= this.Player.JumpFrame)
						this.Player.YSpeed = -8.0;

					DDUtils.Minim(ref this.Player.YSpeed, 8.0); // 落下する最高速度

					this.Player.Y += this.Player.YSpeed;
				}

				// プレイヤー位置矯正
				{
					bool touchSide_L =
						this.Map.GetCell(Map.ToTablePoint(this.Player.X - 10.0, this.Player.Y - MapTile.WH / 2)).Wall ||
						this.Map.GetCell(Map.ToTablePoint(this.Player.X - 10.0, this.Player.Y)).Wall ||
						this.Map.GetCell(Map.ToTablePoint(this.Player.X - 10.0, this.Player.Y + MapTile.WH / 2)).Wall;

					bool touchSide_R =
						this.Map.GetCell(Map.ToTablePoint(this.Player.X + 10.0, this.Player.Y - MapTile.WH / 2)).Wall ||
						this.Map.GetCell(Map.ToTablePoint(this.Player.X + 10.0, this.Player.Y)).Wall ||
						this.Map.GetCell(Map.ToTablePoint(this.Player.X + 10.0, this.Player.Y + MapTile.WH / 2)).Wall;

					if (touchSide_L && touchSide_R)
					{
						// noop
					}
					else if (touchSide_L)
					{
						this.Player.X = (double)DoubleTools.ToInt(this.Player.X / MapTile.WH) * MapTile.WH + 10.0;
					}
					else if (touchSide_R)
					{
						this.Player.X = (double)DoubleTools.ToInt(this.Player.X / MapTile.WH) * MapTile.WH - 10.0;
					}

					bool touchCeiling_L = this.Map.GetCell(Map.ToTablePoint(this.Player.X - 9.0, this.Player.Y - MapTile.WH)).Wall;
					bool touchCeiling_R = this.Map.GetCell(Map.ToTablePoint(this.Player.X + 9.0, this.Player.Y - MapTile.WH)).Wall;
					bool touchCeiling = touchCeiling_L && touchCeiling_R;

					if (touchCeiling_L && touchCeiling_R)
					{
						if (this.Player.YSpeed < 0.0)
						{
							this.Player.Y = (int)(this.Player.Y / MapTile.WH + 1) * MapTile.WH;
							this.Player.YSpeed = 0.0;
							this.Player.JumpFrame = 0;
						}
					}
					else if (touchCeiling_L)
					{
						this.Player.X = (double)DoubleTools.ToInt(this.Player.X / MapTile.WH) * MapTile.WH + 9.0;
					}
					else if (touchCeiling_R)
					{
						this.Player.X = (double)DoubleTools.ToInt(this.Player.X / MapTile.WH) * MapTile.WH - 9.0;
					}

					this.Player.TouchGround =
						this.Map.GetCell(Map.ToTablePoint(this.Player.X - 9.0, this.Player.Y + MapTile.WH)).Wall ||
						this.Map.GetCell(Map.ToTablePoint(this.Player.X + 9.0, this.Player.Y + MapTile.WH)).Wall;

					if (this.Player.TouchGround)
					{
						DDUtils.Minim(ref this.Player.YSpeed, 0.0);

						double plY = (int)(this.Player.Y / MapTile.WH) * MapTile.WH;

						DDUtils.Minim(ref this.Player.Y, plY);
					}

					if (this.Player.TouchGround)
						this.Player.AirborneFrame = 0;
					else
						this.Player.AirborneFrame++;
				}

				if (this.Player.AttackFrame % 6 == 1)
				{
					double x = this.Player.X;
					double y = this.Player.Y;

					x += 30.0 * (this.Player.FacingLeft ? -1 : 1);

					if (1 <= this.Player.ShagamiFrame)
						y += 10.0;
					else
						y -= 4.0;

					this.Weapons.Add(new Weapon0001(x, y, this.Player.FacingLeft));
				}

				this.Player.Crash = CrashUtils.Point(new D2Point(this.Player.X, this.Player.Y));

				this.EnemyEachFrame();
				this.WeaponEachFrame();

				// Crash
				{
					foreach (AEnemy enemy in this.Enemies)
					{
						foreach (AWeapon weapon in this.Weapons)
						{
							if (enemy.Crash.IsCrashed(weapon.Crash))
							{
								enemy.Crashed(weapon);
								weapon.Crashed(enemy);
							}
						}
						if (this.Player.DamageFrame == 0 && enemy.Crash.IsCrashed(this.Player.Crash))
						{
							enemy.CrashedToPlayer();
							this.Player.DamageFrame = 1;
						}
					}
				}

				// 描画ここから

				DrawWall();
				DrawMap();
				DrawPlayer();
				DrawEnemies();
				DrawWeapons();

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();
			DDMusicUtils.Fade();
			DDCurtain.SetCurtain(30, -1.0);

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				DrawWall();
				DDEngine.EachFrame();
			}
		}

		private void EditMode()
		{
			DDEngine.FreezeInput();
			DDUtils.SetMouseDispMode(true);

			for (; ; )
			{
				int lastMouseX = DDMouse.X;
				int lastMouseY = DDMouse.Y;

				DDMouse.UpdatePos();

				DDGround.ICamera.X = DoubleTools.ToInt(DDGround.Camera.X);
				DDGround.ICamera.Y = DoubleTools.ToInt(DDGround.Camera.Y);

				if (DDKey.GetInput(DX.KEY_INPUT_E) == 1)
					break;

				if (1 <= DDKey.GetInput(DX.KEY_INPUT_LSHIFT) || 1 <= DDKey.GetInput(DX.KEY_INPUT_RSHIFT)) // シフト押下 -> 移動モード
				{
					if (1 <= DDMouse.L.GetInput())
					{
						DDGround.Camera.X -= DDMouse.X - lastMouseX;
						DDGround.Camera.Y -= DDMouse.Y - lastMouseY;
					}
				}
				else // 編集モード
				{
					Edit.EachFrame();
				}

				DrawWall();

				if (Edit.DisplayTileFlag)
					DrawMap();

				Edit.Draw();

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();
			DDUtils.SetMouseDispMode(false);
		}

		private void DrawWall()
		{
			DDCurtain.DrawCurtain();
		}

		private void DrawMap()
		{
			int w = this.Map.W;
			int h = this.Map.H;

			int camL = DDGround.ICamera.X;
			int camT = DDGround.ICamera.Y;
			int camR = camL + DDConsts.Screen_W;
			int camB = camT + DDConsts.Screen_H;

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					int mapTileX = x * MapTile.WH + MapTile.WH / 2;
					int mapTileY = y * MapTile.WH + MapTile.WH / 2;

					if (DDUtils.IsOut(new D2Point(mapTileX, mapTileY), new D4Rect(camL, camT, camR, camB), 100.0) == false) // マージン要調整
					{
						MapCell cell = this.Map.GetCell(x, y);

						if (cell.Tile != null) // ? ! 描画無し
						{
							DDDraw.DrawCenter(cell.Tile.Picture, mapTileX - camL, mapTileY - camT);
						}
					}
				}
			}
		}

		private int PlayerLookLeftFrm = 0;

		private void DrawPlayer()
		{
			if (PlayerLookLeftFrm == 0 && DDUtils.Random.Real2() < 0.002) // キョロキョロするレート
				PlayerLookLeftFrm = 150 + (int)(DDUtils.Random.Real2() * 90.0);

			DDUtils.CountDown(ref PlayerLookLeftFrm);

			double xZoom = this.Player.FacingLeft ? -1 : 1;

			// 立ち >

			DDPicture picture = Ground.I.Picture.PlayerStands[120 < PlayerLookLeftFrm ? 1 : 0][(DDEngine.ProcFrame / 20) % 2];

			if (1 <= this.Player.MoveFrame)
			{
				if (this.Player.MoveSlow)
				{
					picture = Ground.I.Picture.PlayerWalk[(DDEngine.ProcFrame / 10) % 2];
				}
				else
				{
					picture = Ground.I.Picture.PlayerDash[(DDEngine.ProcFrame / 5) % 2];
				}
			}
			if (this.Player.TouchGround == false)
			{
				picture = Ground.I.Picture.PlayerJump[0];
			}
			if (1 <= this.Player.ShagamiFrame)
			{
				picture = Ground.I.Picture.PlayerShagami;
			}

			// < 立ち

			// 攻撃中 >

			if (1 <= this.Player.AttackFrame)
			{
				picture = Ground.I.Picture.PlayerAttack;

				if (1 <= this.Player.MoveFrame)
				{
					if (this.Player.MoveSlow)
					{
						picture = Ground.I.Picture.PlayerAttackWalk[(DDEngine.ProcFrame / 10) % 2];
					}
					else
					{
						picture = Ground.I.Picture.PlayerAttackDash[(DDEngine.ProcFrame / 5) % 2];
					}
				}
				if (this.Player.TouchGround == false)
				{
					picture = Ground.I.Picture.PlayerAttackJump;
				}
				if (1 <= this.Player.ShagamiFrame)
				{
					picture = Ground.I.Picture.PlayerAttackShagami;
				}
			}

			// < 攻撃中

			if (1 <= this.Player.DamageFrame)
			{
				picture = Ground.I.Picture.PlayerDamage[0];
				xZoom *= -1;
			}

			DDDraw.DrawBegin(
					picture,
					DoubleTools.ToInt(this.Player.X - DDGround.ICamera.X),
					DoubleTools.ToInt(this.Player.Y - DDGround.ICamera.Y) - 16
					);
			DDDraw.DrawZoom_X(xZoom);
			DDDraw.DrawEnd();

			// debug
			{
				DDDraw.DrawBegin(DDGround.GeneralResource.Dummy, this.Player.X - DDGround.ICamera.X, this.Player.Y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(0.1);
				DDDraw.DrawRotate(DDEngine.ProcFrame * 0.01);
				DDDraw.DrawEnd();
			}
		}

		public List<AEnemy> Enemies = new List<AEnemy>();

		private void ReloadEnemies()
		{
			this.Enemies.Clear();

			for (int x = 0; x < this.Map.W; x++)
			{
				for (int y = 0; y < this.Map.H; y++)
				{
					MapCell cell = this.Map.GetCell(x, y);

					if (cell.EnemyLoader != null)
					{
						AEnemy enemy = cell.EnemyLoader.CreateEnemy();

						enemy.SetTablePoint(new I2Point(x, y));

						this.Enemies.Add(enemy);
					}
				}
			}
		}

		private void EnemyEachFrame()
		{
			for (int index = 0; index < this.Enemies.Count; index++)
			{
				AEnemy enemy = this.Enemies[index];

				if (enemy.EachFrame() == false) // ? 消滅
				{
					ExtraTools.FastDesertElement(this.Enemies, index--);
				}
				else
				{
					enemy.Frame++;
				}
			}
		}

		private void DrawEnemies()
		{
			foreach (AEnemy enemy in this.Enemies)
			{
				enemy.Draw();
			}
		}

		public List<AWeapon> Weapons = new List<AWeapon>();

		private void WeaponEachFrame()
		{
			for (int index = 0; index < this.Weapons.Count; index++)
			{
				AWeapon weapon = this.Weapons[index];

				if (weapon.EachFrame() == false) // ? 消滅
				{
					ExtraTools.FastDesertElement(this.Weapons, index--);
				}
				else
				{
					weapon.Frame++;
				}
			}
		}

		private void DrawWeapons()
		{
			foreach (AWeapon weapon in this.Weapons)
			{
				weapon.Draw();
			}
		}
	}
}
