using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;
using DxLibDLL;

namespace Charlotte.Game
{
	public static class AGameMain
	{
		public static void GameMain()
		{
			AGame.I.Player.X = AGame.I.Prm_StartX;
			AGame.I.Player.Y = AGame.I.Prm_StartY;

			GameGround.Camera.X = AGame.I.Player.X - GameConsts.Screen_W / 2.0;
			GameGround.Camera.Y = AGame.I.Player.Y - GameConsts.Screen_H / 2.0;

			GameCurtain.SetCurtain();
			GameEngine.FreezeInput();

			// TODO music

			for (; ; )
			{
				GameUtils.Approach(ref GameGround.Camera.X, AGame.I.Player.X - GameConsts.Screen_W / 2 + (AGame.I.CamSlideX * GameConsts.Screen_W / 3), 0.8);
				GameUtils.Approach(ref GameGround.Camera.Y, AGame.I.Player.X - GameConsts.Screen_H / 2 + (AGame.I.CamSlideY * GameConsts.Screen_H / 3), 0.8);

				GameUtils.Range(ref GameGround.Camera.X, 0.0, Map.Get_W() + Consts.MAP_TILE_WH - GameConsts.Screen_W);
				GameUtils.Range(ref GameGround.Camera.Y, 0.0, Map.Get_W() + Consts.MAP_TILE_WH - GameConsts.Screen_W);

				GameGround.ICamera.X = DoubleTools.ToInt(GameGround.Camera.X);
				GameGround.ICamera.Y = DoubleTools.ToInt(GameGround.Camera.Y);

				if (GameConfig.LOG_ENABLED && GameKey.GetInput(DX.KEY_INPUT_E) == 1)
				{
					EditMain();
				}

				// プレイヤー入力
				{
					bool move = false;
					bool slow = false;
					bool camSlide = false;
					int jumpPress;
					bool jump = false;

					if (1 <= GameInput.DIR_4.GetInput())
					{
						AGame.I.Player.FacingLeft = true;
						move = true;
					}
					if (1 <= GameInput.DIR_6.GetInput())
					{
						AGame.I.Player.FacingLeft = false;
						move = true;
					}
					if (1 <= GameInput.L.GetInput())
					{
						move = false;
						camSlide = true;
					}
					if (1 <= GameInput.R.GetInput())
					{
						slow = true;
					}
					if (1 <= (jumpPress = GameInput.A.GetInput()))
					{
						jump = true;
					}
					if (1 <= GameInput.B.GetInput())
					{
						// TODO
					}

					if (move)
						AGame.I.Player.MoveFrame++;
					else
						AGame.I.Player.MoveFrame = 0;

					AGame.I.Player.MoveSlow = move && slow;

					if (1 <= AGame.I.Player.JumpFrame)
					{
						if (jump && AGame.I.Player.JumpFrame < 22)
							AGame.I.Player.JumpFrame++;
						else
							AGame.I.Player.JumpFrame = 0;
					}
					else
					{
						//if (jump && jumpPress < 5 && AGame.I.Player.TouchGround)
						if (jump && jumpPress < 5 && AGame.I.Player.AirborneFrame < 5)
							AGame.I.Player.JumpFrame = 1;
					}

					if (camSlide)
					{
						if (1 <= GameInput.DIR_4.GetInput())
						{
							AGame.I.CamSlideCount++;
							AGame.I.CamSlideX--;
						}
						if (1 <= GameInput.DIR_6.GetInput())
						{
							AGame.I.CamSlideCount++;
							AGame.I.CamSlideX++;
						}
						if (1 <= GameInput.DIR_8.GetInput())
						{
							AGame.I.CamSlideCount++;
							AGame.I.CamSlideY--;
						}
						if (1 <= GameInput.DIR_2.GetInput())
						{
							AGame.I.CamSlideCount++;
							AGame.I.CamSlideY++;
						}
						GameUtils.Range(ref AGame.I.CamSlideX, -1, 1);
						GameUtils.Range(ref AGame.I.CamSlideY, -1, 1);
					}
					else
					{
						if (AGame.I.CamSlideMode && AGame.I.CamSlideCount == 0)
						{
							AGame.I.CamSlideX = 0;
							AGame.I.CamSlideY = 0;
						}
						AGame.I.CamSlideCount = 0;
					}
					AGame.I.CamSlideMode = camSlide;
				}

				// プレイヤー移動
				{
					if (1 <= AGame.I.Player.MoveFrame)
					{
						double speed = 0.0;

						if (AGame.I.Player.MoveSlow)
						{
							speed = AGame.I.Player.MoveFrame * 0.2;
							speed = Math.Min(speed, 2.0);
						}
						else
							speed = 6.0;

						speed *= AGame.I.Player.FacingLeft ? -1 : 1;

						AGame.I.Player.X += speed;
					}
					else
						AGame.I.Player.X = (double)DoubleTools.ToInt(AGame.I.Player.X);

					AGame.I.Player.YSpeed += 1.0; // += 重力加速度

					if (1 <= AGame.I.Player.JumpFrame)
						AGame.I.Player.YSpeed = -8.0;

					AGame.I.Player.YSpeed = Math.Min(AGame.I.Player.YSpeed, 8.0); // 落下する最高速度
					AGame.I.Player.Y += AGame.I.Player.YSpeed;
				}

				// プレイヤー位置矯正
				{
					bool touchSide_L =
						Map.GetMapCell(AGameUtils.PointToMapCellPoint(AGame.I.Player.X - 10.0, AGame.I.Player.Y - Consts.MAP_TILE_WH / 2)).Wall ||
						Map.GetMapCell(AGameUtils.PointToMapCellPoint(AGame.I.Player.X - 10.0, AGame.I.Player.Y)).Wall ||
						Map.GetMapCell(AGameUtils.PointToMapCellPoint(AGame.I.Player.X - 10.0, AGame.I.Player.Y + Consts.MAP_TILE_WH / 2)).Wall;

					bool touchSide_R =
						Map.GetMapCell(AGameUtils.PointToMapCellPoint(AGame.I.Player.X + 10.0, AGame.I.Player.Y - Consts.MAP_TILE_WH / 2)).Wall ||
						Map.GetMapCell(AGameUtils.PointToMapCellPoint(AGame.I.Player.X + 10.0, AGame.I.Player.Y)).Wall ||
						Map.GetMapCell(AGameUtils.PointToMapCellPoint(AGame.I.Player.X + 10.0, AGame.I.Player.Y + Consts.MAP_TILE_WH / 2)).Wall;

					if (touchSide_L && touchSide_R)
					{
						// noop
					}
					else if (touchSide_L)
					{
						AGame.I.Player.X = DoubleTools.ToInt(AGame.I.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH + 10.0;
					}
					else if (touchSide_R)
					{
						AGame.I.Player.X = DoubleTools.ToInt(AGame.I.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH - 10.0;
					}

					bool touchCeiling_L = Map.GetMapCell(AGameUtils.PointToMapCellPoint(AGame.I.Player.X - 9.0, AGame.I.Player.Y - Consts.MAP_TILE_WH)).Wall;
					bool touchCeiling_R = Map.GetMapCell(AGameUtils.PointToMapCellPoint(AGame.I.Player.X + 9.0, AGame.I.Player.Y - Consts.MAP_TILE_WH)).Wall;
					bool touchCeiling = touchCeiling_L && touchCeiling_R;

					if (touchCeiling_L && touchCeiling_R)
					{
						if (AGame.I.Player.YSpeed < 0.0)
						{
							AGame.I.Player.Y = (int)(AGame.I.Player.Y / Consts.MAP_TILE_WH + 1) * Consts.MAP_TILE_WH;
							AGame.I.Player.YSpeed = 0.0;
							AGame.I.Player.JumpFrame = 0;
						}
					}
					else if (touchCeiling_L)
					{
						AGame.I.Player.X = DoubleTools.ToInt(AGame.I.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH + 9.0;
					}
					else if (touchCeiling_R)
					{
						AGame.I.Player.X = DoubleTools.ToInt(AGame.I.Player.X / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH - 9.0;
					}

					AGame.I.Player.TouchGround =
						Map.GetMapCell(AGameUtils.PointToMapCellPoint(AGame.I.Player.X - 9.0, AGame.I.Player.Y + Consts.MAP_TILE_WH)).Wall ||
						Map.GetMapCell(AGameUtils.PointToMapCellPoint(AGame.I.Player.X + 9.0, AGame.I.Player.Y + Consts.MAP_TILE_WH)).Wall;

					if (AGame.I.Player.TouchGround)
					{
						AGame.I.Player.YSpeed = Math.Min(AGame.I.Player.YSpeed, 0.0);

						double plY = (int)(AGame.I.Player.Y / Consts.MAP_TILE_WH) * Consts.MAP_TILE_WH;

						AGame.I.Player.Y = Math.Min(AGame.I.Player.Y, plY);
					}

					if (AGame.I.Player.TouchGround)
						AGame.I.Player.AirborneFrame = 0;
					else
						AGame.I.Player.AirborneFrame++;
				}

				// 描画ここから

				DrawWall();
				DrawMap();
				DrawPlayer();

				GameEngine.EachFrame();
			}
			GameEngine.FreezeInput();
			GameMusicUtils.Fade();
			GameCurtain.SetCurtain(30, -1.0);

			foreach (GameScene scene in GameSceneUtils.Create(40))
			{
				DrawWall();
				GameEngine.EachFrame();
			}
		}

		private static void DrawWall()
		{
			throw new NotImplementedException();
		}

		private static void DrawMap()
		{
			throw new NotImplementedException();
		}

		private static void DrawPlayer()
		{
			throw new NotImplementedException();
		}

		private static void EditMain()
		{
			// TODO
		}
	}
}
