using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Tools;
using Charlotte.Common;
using Charlotte.Game3Common;
using Charlotte.Games.Enemies.Objects;
using Charlotte.Games.Weapons;

namespace Charlotte.Games
{
	public class Game : IDisposable
	{
		public Map Map;
		public Status Status;

		// <---- prm

		public int ExitDir = 5; // 2468 == 画面から出た方向, 5 == それ以外の理由

		// <---- ret

		public static Game I = null;

		public Game()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public Player Player = new Player();

		private bool CamSlideMode; // ? モード中
		private int CamSlideCount;
		private int CamSlideX; // -1 ～ 1
		private int CamSlideY; // -1 ～ 1

		private DDPicture WallPicture;
		private double WallPicture_Zoom;
		private const double WallPicture_SlideRate = 0.1;

		public int Frame;

		public void Perform()
		{
			this.ReloadEnemies();

			// デフォルトのスタート位置
			this.Player.X = this.Map.W * MapTile.WH / 2.0;
			this.Player.Y = this.Map.H * MapTile.WH / 2.0;

			foreach (EnemyBox enemy in this.Enemies.Iterate()) // スタート位置
			{
				StartPoint sp = enemy.Value as StartPoint;

				if (sp != null)
				{
					if (sp.Index == this.Status.StartPointIndex)
					{
						this.Player.X = sp.X;
						this.Player.Y = sp.Y;
						break;
					}
				}
			}

			this.Player.HP = this.Status.CurrHP;
			this.Player.FacingLeft = this.Status.FacingLeft;

			this.WallPicture = WallPictureManager.GetPicutre(this.Map.GetProperty("WALL", "09311.jpg"));

			{
				double w = DDConsts.Screen_W + (this.Map.W * MapTile.WH - DDConsts.Screen_W) * WallPicture_SlideRate;
				double h = DDConsts.Screen_H + (this.Map.H * MapTile.WH - DDConsts.Screen_H) * WallPicture_SlideRate;

				double zw = w / this.WallPicture.Get_W();
				double zh = h / this.WallPicture.Get_H();

				double z = Math.Max(zw, zh);

				z *= 1.01; // margin

				this.WallPicture_Zoom = z;
			}

			DDGround.Camera.X = this.Player.X - DDConsts.Screen_W / 2.0;
			DDGround.Camera.Y = this.Player.Y - DDConsts.Screen_H / 2.0;

			DDCurtain.SetCurtain(10);
			DDEngine.FreezeInput();

			// TODO music

			for (; ; this.Frame++)
			{
				{
					double targCamX = this.Player.X - DDConsts.Screen_W / 2 + (this.CamSlideX * DDConsts.Screen_W / 3);
					double targCamY = this.Player.Y - DDConsts.Screen_H / 2 + (this.CamSlideY * DDConsts.Screen_H / 3);

					DDUtils.Range(ref targCamX, 0.0, this.Map.W * MapTile.WH - DDConsts.Screen_W);
					DDUtils.Range(ref targCamY, 0.0, this.Map.H * MapTile.WH - DDConsts.Screen_H);

					DDUtils.Approach(ref DDGround.Camera.X, targCamX, 0.8);
					DDUtils.Approach(ref DDGround.Camera.Y, targCamY, 0.8);
				}

				DDUtils.Range(ref DDGround.Camera.X, 0.0, this.Map.W * MapTile.WH - DDConsts.Screen_W);
				DDUtils.Range(ref DDGround.Camera.Y, 0.0, this.Map.H * MapTile.WH - DDConsts.Screen_H);

				DDGround.ICamera.X = DoubleTools.ToInt(DDGround.Camera.X);
				DDGround.ICamera.Y = DoubleTools.ToInt(DDGround.Camera.Y);

				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_E) == 1)
				{
					this.EditMode();
					this.ReloadEnemies();
					this.Frame = 0;
				}

				// プレイヤー入力
				{
					bool deadOrDamage = this.Player.DeadScene.IsFlaming() || this.Player.DamageScene.IsFlaming();
					bool move = false;
					bool slow = false;
					bool camSlide = false;
					int jumpPress = DDInput.A.GetInput();
					bool jump = false;
					bool shagami = false;
					bool attack = false;

					if (!deadOrDamage && 1 <= DDInput.DIR_2.GetInput())
					{
						shagami = true;
					}
					if (!deadOrDamage && 1 <= DDInput.DIR_4.GetInput())
					{
						this.Player.FacingLeft = true;
						move = true;
					}
					if (!deadOrDamage && 1 <= DDInput.DIR_6.GetInput())
					{
						this.Player.FacingLeft = false;
						move = true;
					}
					if (1 <= DDInput.L.GetInput())
					{
						move = false;
						camSlide = true;
					}
					if (!deadOrDamage && 1 <= DDInput.R.GetInput())
					{
						slow = true;
					}
					if (!deadOrDamage && 1 <= jumpPress)
					{
						jump = true;
					}
					if (!deadOrDamage && 1 <= DDInput.B.GetInput())
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

				{
					DDScene scene = this.Player.DeadScene.GetScene();

					if (scene.Numer != -1)
					{
						if (scene.Numer < 30)
						{
							double rate = scene.Numer / 30.0;

							this.Player.X -= 10.0 * (1.0 - rate) * (this.Player.FacingLeft ? -1 : 1);
						}

						if (scene.Remaining == 0)
						{
							break;
						}
					}
				}

				{
					DDScene scene = this.Player.DamageScene.GetScene();

					if (scene.Numer != -1)
					{
						this.Player.X -= (9.0 - 6.0 * scene.Rate) * (this.Player.FacingLeft ? -1 : 1);

						if (scene.Remaining == 0)
						{
							this.Player.MutekiScene.FireDelay();
						}
					}
				}

				{
					DDScene scene = this.Player.MutekiScene.GetScene();

					if (scene.Numer != -1)
					{
						// noop
					}
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

				if (this.Player.X < 0.0) // ? マップの左側に出た。
				{
					this.ExitDir = 4;
					break;
				}
				if (this.Map.W * MapTile.WH < this.Player.X) // ? マップの右側に出た。
				{
					this.ExitDir = 6;
					break;
				}
				if (this.Player.Y < 0.0) // ? マップの上側に出た。
				{
					this.ExitDir = 8;
					break;
				}
				if (this.Map.H * MapTile.WH < this.Player.Y) // ? マップの下側に出た。
				{
					this.ExitDir = 2;
					break;
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

				if (this.Frame == 0) // 画面遷移時の微妙なカメラ位置ズレ解消
				{
					DDGround.Camera.X = this.Player.X - DDConsts.Screen_W / 2.0;
					DDGround.Camera.Y = this.Player.Y - DDConsts.Screen_H / 2.0;

					DDUtils.Range(ref DDGround.Camera.X, 0.0, this.Map.W * MapTile.WH - DDConsts.Screen_W);
					DDUtils.Range(ref DDGround.Camera.Y, 0.0, this.Map.H * MapTile.WH - DDConsts.Screen_H);

					DDGround.ICamera.X = DoubleTools.ToInt(DDGround.Camera.X);
					DDGround.ICamera.Y = DoubleTools.ToInt(DDGround.Camera.Y);
				}

				if (1 <= this.Player.AttackFrame)
				{
					this.Player.Attack();
				}

				this.EnemyEachFrame();
				this.WeaponEachFrame();

				// Crash
				{
					Crash playerCrash = CrashUtils.Point(new D2Point(this.Player.X, this.Player.Y));

					foreach (WeaponBox weapon in this.Weapons.Iterate())
						weapon.Crash = weapon.Value.GetCrash();

					foreach (EnemyBox enemy in this.Enemies.Iterate())
					{
						Crash enemyCrash = enemy.Value.GetCrash();

						foreach (WeaponBox weapon in this.Weapons.Iterate())
						{
							if (enemyCrash.IsCrashed(weapon.Crash))
							{
								if (enemy.Value.Crashed(weapon.Value) == false) // ? 消滅
									enemy.Dead = true;

								if (weapon.Value.Crashed(enemy.Value) == false) // ? 消滅
									weapon.Dead = true;
							}
						}
						this.Weapons.RemoveAll(weapon => weapon.Dead);

						if (this.Player.DeadScene.IsFlaming() == false &&
							this.Player.DamageScene.IsFlaming() == false &&
							this.Player.MutekiScene.IsFlaming() == false && enemyCrash.IsCrashed(playerCrash))
						{
							if (enemy.Value.CrashedToPlayer() == false) // ? 消滅
								enemy.Dead = true;

							this.Player.Crashed(enemy.Value);
						}
					}
					this.Enemies.RemoveAll(enemy => enemy.Dead);
				}

				// 描画ここから

				this.DrawWall();
				this.DrawMap();
				this.Player.Draw();
				this.DrawEnemies();
				this.DrawWeapons();

				DDPrint.SetPrint();
				DDPrint.Print(DDEngine.FrameProcessingMillis_Worst + " " + this.Player.HP);

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();

			if (this.ExitDir == 5)
			{
				DDMusicUtils.Fade();
				DDCurtain.SetCurtain(30, -1.0);

				foreach (DDScene scene in DDSceneUtils.Create(40))
				{
					this.DrawWall();
					this.DrawMap();

					DDEngine.EachFrame();
				}
			}
			else
			{
				double destSlideX = 0.0;
				double destSlideY = 0.0;

				switch (this.ExitDir)
				{
					case 4:
						destSlideX = DDConsts.Screen_W;
						break;

					case 6:
						destSlideX = -DDConsts.Screen_W;
						break;

					case 8:
						destSlideY = DDConsts.Screen_H;
						break;

					case 2:
						destSlideY = -DDConsts.Screen_H;
						break;

					default:
						throw null; // never
				}
#if true
				using (DDSubScreen wallScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H))
				{
					using (wallScreen.Section())
					{
						this.DrawWall();
					}

					foreach (DDScene scene in DDSceneUtils.Create(30))
					{
						this.DrawMap_SlideX = destSlideX * scene.Rate;
						this.DrawMap_SlideY = destSlideY * scene.Rate;

						DDCurtain.DrawCurtain();
						DDDraw.DrawSimple(wallScreen.ToPicture(), this.DrawMap_SlideX, this.DrawMap_SlideY);
						this.DrawMap();

						DDEngine.EachFrame();
					}
				}
#else // old
				foreach (DDScene scene in DDSceneUtils.Create(10))
				{
					this.DrawWall();
					DDCurtain.DrawCurtain(-scene.Rate);
					this.DrawMap();

					DDEngine.EachFrame();
				}
				foreach (DDScene scene in DDSceneUtils.Create(20))
				{
					this.DrawMap_SlideX = destSlideX * scene.Rate;
					this.DrawMap_SlideY = destSlideY * scene.Rate;

					DDCurtain.DrawCurtain(-1.0);
					this.DrawMap();

					DDEngine.EachFrame();
				}
#endif
				this.DrawMap_SlideX = 0.0; // 復元
				this.DrawMap_SlideY = 0.0; // 復元

				DDCurtain.SetCurtain(0, -1.0);
			}

			// ここでステータスに反映

			this.Status.CurrHP = this.Player.HP;
			this.Status.FacingLeft = this.Player.FacingLeft;
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

				if (DDKey.GetInput(DX.KEY_INPUT_E) == 1)
					break;

				if (1 <= DDKey.GetInput(DX.KEY_INPUT_LSHIFT) || 1 <= DDKey.GetInput(DX.KEY_INPUT_RSHIFT)) // シフト押下 -> 移動モード
				{
					if (1 <= DDMouse.L.GetInput())
					{
						DDGround.Camera.X -= DDMouse.X - lastMouseX;
						DDGround.Camera.Y -= DDMouse.Y - lastMouseY;

						DDUtils.Range(ref DDGround.Camera.X, 0.0, this.Map.W * MapTile.WH - DDConsts.Screen_W);
						DDUtils.Range(ref DDGround.Camera.Y, 0.0, this.Map.H * MapTile.WH - DDConsts.Screen_H);

						DDGround.ICamera.X = DoubleTools.ToInt(DDGround.Camera.X);
						DDGround.ICamera.Y = DoubleTools.ToInt(DDGround.Camera.Y);
					}
				}
				else // 編集モード
				{
					GameEdit.EachFrame();
				}

				DrawWall();

				if (GameEdit.DisplayTileFlag)
					DrawMap();

				GameEdit.Draw();

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();
			DDUtils.SetMouseDispMode(false);
		}

		private void DrawWall()
		{
			DDDraw.DrawBegin(this.WallPicture, DDConsts.Screen_W / 2, DDConsts.Screen_H / 2);
			DDDraw.DrawZoom(this.WallPicture_Zoom);
			DDDraw.DrawSlide(
				((this.Map.W * MapTile.WH - DDConsts.Screen_W) / 2 - DDGround.Camera.X) * WallPicture_SlideRate,
				((this.Map.H * MapTile.WH - DDConsts.Screen_H) / 2 - DDGround.Camera.Y) * WallPicture_SlideRate
				);
			DDDraw.DrawEnd();

			DDCurtain.DrawCurtain(-0.7);
		}

		private double DrawMap_SlideX = 0.0;
		private double DrawMap_SlideY = 0.0;

		private void DrawMap()
		{
			int w = this.Map.W;
			int h = this.Map.H;

			int camL = DDGround.ICamera.X;
			int camT = DDGround.ICamera.Y;
			int camR = camL + DDConsts.Screen_W;
			int camB = camT + DDConsts.Screen_H;

			I2Point lt = Map.ToTablePoint(camL, camT);
			I2Point rb = Map.ToTablePoint(camR, camB);

			lt.X -= 2; // margin
			lt.Y -= 2; // margin
			rb.X += 2; // margin
			rb.Y += 2; // margin

			lt.X = IntTools.ToRange(lt.X, 0, w - 1);
			lt.Y = IntTools.ToRange(lt.Y, 0, h - 1);
			rb.X = IntTools.ToRange(rb.X, 0, w - 1);
			rb.Y = IntTools.ToRange(rb.Y, 0, h - 1);

			for (int x = lt.X; x <= rb.X; x++)
			{
				for (int y = lt.Y; y <= rb.Y; y++)
				{
					int mapTileX = x * MapTile.WH + MapTile.WH / 2;
					int mapTileY = y * MapTile.WH + MapTile.WH / 2;

					//if (DDUtils.IsOut(new D2Point(mapTileX, mapTileY), new D4Rect(camL, camT, camR, camB), MapTile.WH * 2) == false) // old
					{
						MapCell cell = this.Map.GetCell(x, y);

						if (cell.Tile != null) // ? ! 描画無し
						{
							DDDraw.DrawCenter(
								cell.Tile.Picture,
								mapTileX - camL + this.DrawMap_SlideX,
								mapTileY - camT + this.DrawMap_SlideY
								);
						}
					}
				}
			}
		}

		private class EnemyBox
		{
			public IEnemy Value;
			public bool Dead;
		}

		private DDList<EnemyBox> Enemies = new DDList<EnemyBox>();

		// 敵弾とかで使うだろうから、とりま public で設置
		public void AddEnemy(IEnemy enemy)
		{
			this.Enemies.Add(new EnemyBox()
			{
				Value = enemy,
			});
		}

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
						this.AddEnemy(IEnemies.Load(
							cell.EnemyLoader.CreateEnemy(),
							x * MapTile.WH + MapTile.WH / 2,
							y * MapTile.WH + MapTile.WH / 2
							));
					}
				}
			}
		}

		private void EnemyEachFrame()
		{
			foreach (EnemyBox enemy in this.Enemies.Iterate())
			{
				if (enemy.Value.EachFrame() == false) // ? 消滅
				{
					enemy.Dead = true;
				}
			}
			this.Enemies.RemoveAll(enemy => enemy.Dead);
		}

		private void DrawEnemies()
		{
			foreach (EnemyBox enemy in this.Enemies.Iterate())
			{
				enemy.Value.Draw();
			}
		}

		private class WeaponBox
		{
			public IWeapon Value;
			public Crash Crash;
			public bool Dead;
		}

		private DDList<WeaponBox> Weapons = new DDList<WeaponBox>();

		public void AddWeapon(IWeapon weapon)
		{
			this.Weapons.Add(new WeaponBox()
			{
				Value = weapon,
			});
		}

		private void WeaponEachFrame()
		{
			foreach (WeaponBox weapon in this.Weapons.Iterate())
			{
				if (weapon.Value.EachFrame() == false) // ? 消滅
				{
					weapon.Dead = true;
				}
			}
			this.Weapons.RemoveAll(weapon => weapon.Dead);
		}

		private void DrawWeapons()
		{
			foreach (WeaponBox weapon in this.Weapons.Iterate())
			{
				weapon.Value.Draw();
			}
		}
	}
}
