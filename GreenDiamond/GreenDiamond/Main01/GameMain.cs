using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;
using DxLibDLL;

namespace Charlotte.Main01
{
	public static class GameMain
	{
		public static void Perform()
		{
			Game.I.Player.X = Game.I.Prm_StartX;
			Game.I.Player.Y = Game.I.Prm_StartY;

			DDGround.Camera.X = Game.I.Player.X - DDConsts.Screen_W / 2.0;
			DDGround.Camera.Y = Game.I.Player.Y - DDConsts.Screen_H / 2.0;

			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			// TODO music

			for (; ; )
			{
				DDUtils.Approach(ref DDGround.Camera.X, Game.I.Player.X - DDConsts.Screen_W / 2 + (Game.I.CamSlideX * DDConsts.Screen_W / 3), 0.8);
				DDUtils.Approach(ref DDGround.Camera.Y, Game.I.Player.Y - DDConsts.Screen_H / 2 + (Game.I.CamSlideY * DDConsts.Screen_H / 3), 0.8);

				DDUtils.Range(ref DDGround.Camera.X, 0.0, Map.Get_W() * Consts.MAP_TILE_WH - DDConsts.Screen_W);
				DDUtils.Range(ref DDGround.Camera.Y, 0.0, Map.Get_H() * Consts.MAP_TILE_WH - DDConsts.Screen_H);

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
						Game.I.Player.FacingLeft = true;
						move = true;
					}
					if (1 <= DDInput.DIR_6.GetInput())
					{
						Game.I.Player.FacingLeft = false;
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
						Game.I.Player.MoveFrame++;
					else
						Game.I.Player.MoveFrame = 0;

					Game.I.Player.MoveSlow = move && slow;

					if (1 <= Game.I.Player.JumpFrame)
					{
						if (jump && Game.I.Player.JumpFrame < 22)
							Game.I.Player.JumpFrame++;
						else
							Game.I.Player.JumpFrame = 0;
					}
					else
					{
						//if (jump && jumpPress < 5 && AGame.I.Player.TouchGround)
						if (jump && jumpPress < 5 && Game.I.Player.AirborneFrame < 5)
							Game.I.Player.JumpFrame = 1;
					}

					if (camSlide)
					{
						if (DDInput.DIR_4.IsPound())
						{
							Game.I.CamSlideCount++;
							Game.I.CamSlideX--;
						}
						if (DDInput.DIR_6.IsPound())
						{
							Game.I.CamSlideCount++;
							Game.I.CamSlideX++;
						}
						if (DDInput.DIR_8.IsPound())
						{
							Game.I.CamSlideCount++;
							Game.I.CamSlideY--;
						}
						if (DDInput.DIR_2.IsPound())
						{
							Game.I.CamSlideCount++;
							Game.I.CamSlideY++;
						}
						DDUtils.Range(ref Game.I.CamSlideX, -1, 1);
						DDUtils.Range(ref Game.I.CamSlideY, -1, 1);
					}
					else
					{
						if (Game.I.CamSlideMode && Game.I.CamSlideCount == 0)
						{
							Game.I.CamSlideX = 0;
							Game.I.CamSlideY = 0;
						}
						Game.I.CamSlideCount = 0;
					}
					Game.I.CamSlideMode = camSlide;
				}

				// プレイヤー移動
				{
					if (1 <= Game.I.Player.MoveFrame)
					{
						double speed = 0.0;

						if (Game.I.Player.MoveSlow)
						{
							speed = Game.I.Player.MoveFrame * 0.2;
							DDUtils.Minim(ref speed, 2.0);
						}
						else
							speed = 6.0;

						speed *= Game.I.Player.FacingLeft ? -1 : 1;

						Game.I.Player.X += speed;
					}
					else
						Game.I.Player.X = (double)DoubleTools.ToInt(Game.I.Player.X);

					Game.I.Player.YSpeed += 1.0; // += 重力加速度

					if (1 <= Game.I.Player.JumpFrame)
						Game.I.Player.YSpeed = -8.0;

					DDUtils.Minim(ref Game.I.Player.YSpeed, 8.0); // 落下する最高速度

					Game.I.Player.Y += Game.I.Player.YSpeed;
				}

				// プレイヤー位置矯正
				{
					bool touchSide_L =
						Map.GetCell(GameUtils.PointToMapCellPoint(Game.I.Player.X - 10.0, Game.I.Player.Y - Consts.MAP_TILE_WH / 2)).Wall ||
						Map.GetCell(GameUtils.PointToMapCellPoint(Game.I.Player.X - 10.0, Game.I.Player.Y)).Wall ||
						Map.GetCell(GameUtils.PointToMapCellPoint(Game.I.Player.X - 10.0, Game.I.Player.Y + Consts.MAP_TILE_WH / 2)).Wall;

					bool touchSide_R =
						Map.GetCell(GameUtils.PointToMapCellPoint(Game.I.Player.X + 10.0, Game.I.Player.Y - Consts.MAP_TILE_WH / 2)).Wall ||
						Map.GetCell(GameUtils.PointToMapCellPoint(Game.I.Player.X + 10.0, Game.I.Player.Y)).Wall ||
						Map.GetCell(GameUtils.PointToMapCellPoint(Game.I.Player.X + 10.0, Game.I.Player.Y + Consts.MAP_TILE_WH / 2)).Wall;

					if (touchSide_L && touchSide_R)
					{
						// noop
					}
					else if (touchSide_L)
					{
						Game.I.Player.X = (double)DoubleTools.ToInt(Game.I.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH + 10.0;
					}
					else if (touchSide_R)
					{
						Game.I.Player.X = (double)DoubleTools.ToInt(Game.I.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH - 10.0;
					}

					bool touchCeiling_L = Map.GetCell(GameUtils.PointToMapCellPoint(Game.I.Player.X - 9.0, Game.I.Player.Y - Consts.MAP_TILE_WH)).Wall;
					bool touchCeiling_R = Map.GetCell(GameUtils.PointToMapCellPoint(Game.I.Player.X + 9.0, Game.I.Player.Y - Consts.MAP_TILE_WH)).Wall;
					bool touchCeiling = touchCeiling_L && touchCeiling_R;

					if (touchCeiling_L && touchCeiling_R)
					{
						if (Game.I.Player.YSpeed < 0.0)
						{
							Game.I.Player.Y = (int)(Game.I.Player.Y / Consts.MAP_TILE_WH + 1) * Consts.MAP_TILE_WH;
							Game.I.Player.YSpeed = 0.0;
							Game.I.Player.JumpFrame = 0;
						}
					}
					else if (touchCeiling_L)
					{
						Game.I.Player.X = (double)DoubleTools.ToInt(Game.I.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH + 9.0;
					}
					else if (touchCeiling_R)
					{
						Game.I.Player.X = (double)DoubleTools.ToInt(Game.I.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH - 9.0;
					}

					Game.I.Player.TouchGround =
						Map.GetCell(GameUtils.PointToMapCellPoint(Game.I.Player.X - 9.0, Game.I.Player.Y + Consts.MAP_TILE_WH)).Wall ||
						Map.GetCell(GameUtils.PointToMapCellPoint(Game.I.Player.X + 9.0, Game.I.Player.Y + Consts.MAP_TILE_WH)).Wall;

					if (Game.I.Player.TouchGround)
					{
						DDUtils.Minim(ref Game.I.Player.YSpeed, 0.0);

						double plY = (int)(Game.I.Player.Y / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH;

						DDUtils.Minim(ref Game.I.Player.Y, plY);
					}

					if (Game.I.Player.TouchGround)
						Game.I.Player.AirborneFrame = 0;
					else
						Game.I.Player.AirborneFrame++;
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

		private static void Edit()
		{
			EditMenu.MCPicture = null;
			EditMenu.Enemy = null;
			EditMenu.EventName = "";

			DDEngine.FreezeInput();
			DDDxUtils.SetMouseDispMode(true);

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
			DDDxUtils.SetMouseDispMode(false);
		}

		private static void DrawWall()
		{
			DDCurtain.DrawCurtain();
		}

		private static void DrawMap()
		{
			int w = Map.Get_W();
			int h = Map.Get_H();

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
						MapCell cell = Map.GetCell(x, y);

						if (cell.MCPicture != null) // ? ! 描画無し
						{
							DDDraw.DrawCenter(cell.MCPicture.Picture, mapTileX - camL, mapTileY - camT);
						}
					}
				}
			}
		}

		private static int PlayerLookLeftFrm = 0;

		private static void DrawPlayer()
		{
			if (PlayerLookLeftFrm == 0 && DDUtils.Random() < 0.002) // キョロキョロするレート
				PlayerLookLeftFrm = 150 + (int)(DDUtils.Random() * 90.0);

			DDUtils.CountDown(ref PlayerLookLeftFrm);

			DDPicture picture = Ground.I.Picture.PlayerStands[120 < PlayerLookLeftFrm ? 1 : 0][(DDEngine.ProcFrame / 20) % 2];

			if (1 <= Game.I.Player.MoveFrame)
			{
				if (Game.I.Player.MoveSlow)
				{
					picture = Ground.I.Picture.PlayerWalk[(DDEngine.ProcFrame / 10) % 2];
				}
				else
				{
					picture = Ground.I.Picture.PlayerDash[(DDEngine.ProcFrame / 5) % 2];
				}
			}
			if (Game.I.Player.TouchGround == false)
			{
				picture = Ground.I.Picture.PlayerJump[0];
			}

			DDDraw.DrawBegin(
					picture,
					DoubleTools.ToInt(Game.I.Player.X - DDGround.ICamera.X),
					DoubleTools.ToInt(Game.I.Player.Y - DDGround.ICamera.Y) - 16
					);
			DDDraw.DrawZoom_X(Game.I.Player.FacingLeft ? -1 : 1);
			DDDraw.DrawEnd();

			// debug
			{
				DDDraw.DrawBegin(DDGround.GeneralResource.Dummy, Game.I.Player.X - DDGround.ICamera.X, Game.I.Player.Y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(0.1);
				DDDraw.DrawRotate(DDEngine.ProcFrame * 0.01);
				DDDraw.DrawEnd();
			}
		}
	}
}
