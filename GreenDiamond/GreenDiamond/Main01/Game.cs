using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;
using DxLibDLL;

namespace Charlotte.Main01
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
		}

		public PlayerInfo Player = new PlayerInfo();

		public bool CamSlideMode; // ? モード中
		public int CamSlideCount;
		public int CamSlideX; // -1 ～ 1
		public int CamSlideY; // -1 ～ 1

		public void Perform()
		{
			this.Player.X = this.Map.W * Consts.MAP_TILE_WH / 2.0;
			this.Player.Y = this.Map.H * Consts.MAP_TILE_WH / 2.0;

			DDGround.Camera.X = this.Player.X - DDConsts.Screen_W / 2.0;
			DDGround.Camera.Y = this.Player.Y - DDConsts.Screen_H / 2.0;

			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			// TODO music

			for (; ; )
			{
				DDUtils.Approach(ref DDGround.Camera.X, this.Player.X - DDConsts.Screen_W / 2 + (this.CamSlideX * DDConsts.Screen_W / 3), 0.8);
				DDUtils.Approach(ref DDGround.Camera.Y, this.Player.Y - DDConsts.Screen_H / 2 + (this.CamSlideY * DDConsts.Screen_H / 3), 0.8);

				DDUtils.Range(ref DDGround.Camera.X, 0.0, this.Map.W * Consts.MAP_TILE_WH - DDConsts.Screen_W);
				DDUtils.Range(ref DDGround.Camera.Y, 0.0, this.Map.H * Consts.MAP_TILE_WH - DDConsts.Screen_H);

				DDGround.ICamera.X = DoubleTools.ToInt(DDGround.Camera.X);
				DDGround.ICamera.Y = DoubleTools.ToInt(DDGround.Camera.Y);

				if (DDConfig.LOG_ENABLED && DDKey.GetInput(DX.KEY_INPUT_E) == 1)
				{
					Edit();
				}

				// プレイヤー入力
				{
					bool move = false;
					bool slow = false;
					bool camSlide = false;
					int jumpPress;
					bool jump = false;

					if (1 <= DDInput.DIR_4.GetInput())
					{
						this.Player.FacingLeft = true;
						move = true;
					}
					if (1 <= DDInput.DIR_6.GetInput())
					{
						this.Player.FacingLeft = false;
						move = true;
					}
					if (1 <= DDInput.L.GetInput())
					{
						move = false;
						camSlide = true;
					}
					if (1 <= DDInput.R.GetInput())
					{
						slow = true;
					}
					if (1 <= (jumpPress = DDInput.A.GetInput()))
					{
						jump = true;
					}
					if (1 <= DDInput.B.GetInput())
					{
						// TODO
					}
					if (DDInput.C.GetInput() == 1)
					{
						break; // kari
					}

					if (move)
						this.Player.MoveFrame++;
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
						this.Map.GetCell(Map.ToTablePoint(this.Player.X - 10.0, this.Player.Y - Consts.MAP_TILE_WH / 2)).Wall ||
						this.Map.GetCell(Map.ToTablePoint(this.Player.X - 10.0, this.Player.Y)).Wall ||
						this.Map.GetCell(Map.ToTablePoint(this.Player.X - 10.0, this.Player.Y + Consts.MAP_TILE_WH / 2)).Wall;

					bool touchSide_R =
						this.Map.GetCell(Map.ToTablePoint(this.Player.X + 10.0, this.Player.Y - Consts.MAP_TILE_WH / 2)).Wall ||
						this.Map.GetCell(Map.ToTablePoint(this.Player.X + 10.0, this.Player.Y)).Wall ||
						this.Map.GetCell(Map.ToTablePoint(this.Player.X + 10.0, this.Player.Y + Consts.MAP_TILE_WH / 2)).Wall;

					if (touchSide_L && touchSide_R)
					{
						// noop
					}
					else if (touchSide_L)
					{
						this.Player.X = (double)DoubleTools.ToInt(this.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH + 10.0;
					}
					else if (touchSide_R)
					{
						this.Player.X = (double)DoubleTools.ToInt(this.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH - 10.0;
					}

					bool touchCeiling_L = this.Map.GetCell(Map.ToTablePoint(this.Player.X - 9.0, this.Player.Y - Consts.MAP_TILE_WH)).Wall;
					bool touchCeiling_R = this.Map.GetCell(Map.ToTablePoint(this.Player.X + 9.0, this.Player.Y - Consts.MAP_TILE_WH)).Wall;
					bool touchCeiling = touchCeiling_L && touchCeiling_R;

					if (touchCeiling_L && touchCeiling_R)
					{
						if (this.Player.YSpeed < 0.0)
						{
							this.Player.Y = (int)(this.Player.Y / Consts.MAP_TILE_WH + 1) * Consts.MAP_TILE_WH;
							this.Player.YSpeed = 0.0;
							this.Player.JumpFrame = 0;
						}
					}
					else if (touchCeiling_L)
					{
						this.Player.X = (double)DoubleTools.ToInt(this.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH + 9.0;
					}
					else if (touchCeiling_R)
					{
						this.Player.X = (double)DoubleTools.ToInt(this.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH - 9.0;
					}

					this.Player.TouchGround =
						this.Map.GetCell(Map.ToTablePoint(this.Player.X - 9.0, this.Player.Y + Consts.MAP_TILE_WH)).Wall ||
						this.Map.GetCell(Map.ToTablePoint(this.Player.X + 9.0, this.Player.Y + Consts.MAP_TILE_WH)).Wall;

					if (this.Player.TouchGround)
					{
						DDUtils.Minim(ref this.Player.YSpeed, 0.0);

						double plY = (int)(this.Player.Y / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH;

						DDUtils.Minim(ref this.Player.Y, plY);
					}

					if (this.Player.TouchGround)
						this.Player.AirborneFrame = 0;
					else
						this.Player.AirborneFrame++;
				}

				// 描画ここから

				DrawWall();
				DrawMap();
				DrawPlayer();

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

		private void Edit()
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
					if (1 <= DDMouse.Get_L())
					{
						DDGround.Camera.X -= DDMouse.X - lastMouseX;
						DDGround.Camera.Y -= DDMouse.Y - lastMouseY;
					}
				}
				else // 編集モード
				{
					EditMenu.EachFrame();
				}

				DrawWall();
				DrawMap();

				EditMenu.Draw();

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
					int mapTileX = x * Consts.MAP_TILE_WH + Consts.MAP_TILE_WH / 2;
					int mapTileY = y * Consts.MAP_TILE_WH + Consts.MAP_TILE_WH / 2;

					if (DDUtils.IsOut(new D2Point(mapTileX, mapTileY), new D4Rect(camL, camT, camR, camB), 100.0) == false) // マージン要調整
					{
						MapCell cell = this.Map.GetCell(x, y);

						if (cell.MCPicture != null) // ? ! 描画無し
						{
							DDDraw.DrawCenter(cell.MCPicture.Picture, mapTileX - camL, mapTileY - camT);
						}
					}
				}
			}
		}

		private int PlayerLookLeftFrm = 0;

		private void DrawPlayer()
		{
			if (PlayerLookLeftFrm == 0 && DDUtils.Random() < 0.002) // キョロキョロするレート
				PlayerLookLeftFrm = 150 + (int)(DDUtils.Random() * 90.0);

			DDUtils.CountDown(ref PlayerLookLeftFrm);

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

			DDDraw.DrawBegin(
					picture,
					DoubleTools.ToInt(this.Player.X - DDGround.ICamera.X),
					DoubleTools.ToInt(this.Player.Y - DDGround.ICamera.Y) - 16
					);
			DDDraw.DrawZoom_X(this.Player.FacingLeft ? -1 : 1);
			DDDraw.DrawEnd();

			// debug
			{
				DDDraw.DrawBegin(DDGround.GeneralResource.Dummy, this.Player.X - DDGround.ICamera.X, this.Player.Y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(0.1);
				DDDraw.DrawRotate(DDEngine.ProcFrame * 0.01);
				DDDraw.DrawEnd();
			}
		}
	}
}
