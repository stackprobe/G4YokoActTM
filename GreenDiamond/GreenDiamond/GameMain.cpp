#include "all.h"

static void DrawWall(void)
{
	DrawCurtain();
}
static void DrawMap(void)
{
	int w = GetMap_W();
	int h = GetMap_H();

	int camL = GDc.ICameraX;
	int camT = GDc.ICameraY;
	int camR = camL + SCREEN_W;
	int camB = camT + SCREEN_H;

	for(int x = 0; x < w; x++)
	for(int y = 0; y < h; y++)
	{
		int mapTileX = x * MAP_TILE_WH + MAP_TILE_WH / 2;
		int mapTileY = y * MAP_TILE_WH + MAP_TILE_WH / 2;

		if(!IsOut(mapTileX, mapTileY, camL, camT, camR, camB, 100.0)) // マージン要調整
		{
			MapCell_t *cell = GetMapCell(x, y);

			if(cell->PicId != -1) // ? ! 描画無し
			{
				DrawCenter(cell->PicId, mapTileX - camL, mapTileY - camT);
			}
		}
	}
}
static void DrawPlayer(void)
{
	static int lookLeftFrm = 0;

	if(!lookLeftFrm && dRnd() < 0.002) // キョロキョロするレート
		lookLeftFrm = 150 + rnd(90);

	m_countDown(lookLeftFrm);

	int picId = D_PLAYER_STAND_1_00 + (120 < lookLeftFrm ? 2 : 0) + ProcFrame / 20 % 2 | DTP;

	if(GDc.Player.MoveFrame)
	{
		if(GDc.Player.MoveSlow)
		{
			picId = D_PLAYER_WALK_00 + ProcFrame / 10 % 2 | DTP;
		}
		else
		{
			picId = D_PLAYER_DASH_00 + ProcFrame / 5 % 2 | DTP;
		}
	}
	if(!GDc.Player.TouchGround)
	{
		picId = D_PLAYER_JUMP_1 | DTP;
	}

	SetPicRes(GetLTTransPicRes());
	DrawBegin(
		picId,
		d2i(GDc.Player.X - GDc.ICameraX),
		d2i(GDc.Player.Y - GDc.ICameraY) - 16
		);
	DrawZoom_X(GDc.Player.FacingLeft ? -1 : 1);
	DrawEnd();
	ResetPicRes();

	// debug
	{
		DrawBegin(P_DUMMY, GDc.Player.X - GDc.ICameraX, GDc.Player.Y - GDc.ICameraY);
		DrawZoom(0.1);
		DrawRotate(ProcFrame * 0.01);
		DrawEnd();
	}
}
static void EditMain(void)
{
	const int MENU_L = 0;
	const int MENU_T = 0;
	const int MENU_W = 200;
	const int MENU_H = SCREEN_H;

	int picId = 0;
	int enemyId = 0;
	char *eventName = strx("");

	FreezeInput();
	SetMouseDispMode(1);

	for(; ; )
	{
		int lastMouseX = MouseX;
		int lastMouseY = MouseY;

		UpdateMousePos();

		GDc.ICameraX = d2i(CameraX);
		GDc.ICameraY = d2i(CameraY);

		if(GetKeyInput(KEY_INPUT_E) == 1)
			break;

		if(1 <= GetKeyInput(KEY_INPUT_LSHIFT) || 1 <= GetKeyInput(KEY_INPUT_RSHIFT)) // 移動モード
		{
			if(1 <= GetMouInput(MOUBTN_L))
			{
				CameraX -= MouseX - lastMouseX;
				CameraY -= MouseY - lastMouseY;
			}
		}
		else // 編集モード
		{
			MapCell_t *cell = TryGetMapCell((MouseX + GDc.ICameraX) / MAP_TILE_WH, (MouseY + GDc.ICameraY) / MAP_TILE_WH);

			if(cell)
			{
				if(1 <= GetMouInput(MOUBTN_L))
				{
					cell->Wall = 1;
					cell->PicId = picId;
					cell->EnemyId = enemyId;
					strz(cell->EventName, eventName);
				}
				if(1 <= GetMouInput(MOUBTN_R))
				{
					cell->Wall = 0;
					cell->PicId = -1;
					cell->EnemyId = -1;
					*cell->EventName = '\0';
				}
			}
		}

		DrawWall();
		DrawMap();

		DPE_SetAlpha(0.8);
		DPE_SetBright(GetColor(0, 0, 0));
		DrawRect(P_WHITEBOX, MENU_L, MENU_T, MENU_W, MENU_H);
		DPE_Reset();

		EachFrame();
	}
	FreezeInput();
	SetMouseDispMode(0);

	memFree(eventName);
}
void GameMain(void)
{
	GDc.Player.X = GDc.Prm_StartX;
	GDc.Player.Y = GDc.Prm_StartY;

	CameraX = GDc.Player.X - SCREEN_W / 2.0;
	CameraY = GDc.Player.Y - SCREEN_H / 2.0;

	SetCurtain();
	FreezeInput();

//	MusicPlay(MUS_XXX);

	for(; ; )
	{
		m_approach(CameraX, GDc.Player.X - SCREEN_W / 2 + (GDc.CamSlideX * SCREEN_W / 3), 0.8);
		m_approach(CameraY, GDc.Player.Y - SCREEN_H / 2 + (GDc.CamSlideY * SCREEN_H / 3), 0.8);

		m_range(CameraX, 0.0, GetMap_W() * MAP_TILE_WH - SCREEN_W);
		m_range(CameraY, 0.0, GetMap_H() * MAP_TILE_WH - SCREEN_H);

		GDc.ICameraX = d2i(CameraX);
		GDc.ICameraY = d2i(CameraY);

		if(LOG_ENABLED && GetKeyInput(KEY_INPUT_E) == 1)
		{
			EditMain();
		}

		// プレイヤー入力
		{
			int move = 0;
			int slow = 0;
			int camSlide = 0;
			int jumpPress;
			int jump = 0;

			if(1 <= GetInput(INP_DIR_4))
			{
				GDc.Player.FacingLeft = 1;
				move = 1;
			}
			if(1 <= GetInput(INP_DIR_6))
			{
				GDc.Player.FacingLeft = 0;
				move = 1;
			}
			if(1 <= GetInput(INP_L))
			{
				move = 0;
				camSlide = 1;
			}
			if(1 <= GetInput(INP_R))
			{
				slow = 1;
			}
			if(1 <= (jumpPress = GetInput(INP_A)))
			{
				jump = 1;
			}
			if(1 <= GetInput(INP_B))
			{
				// TODO
			}

			if(move)
				GDc.Player.MoveFrame++;
			else
				GDc.Player.MoveFrame = 0;

			GDc.Player.MoveSlow = move && slow;

			if(GDc.Player.JumpFrame) // ? ジャンプ中
			{
				if(jump && GDc.Player.JumpFrame < 22)
					GDc.Player.JumpFrame++;
				else
					GDc.Player.JumpFrame = 0;
			}
			else
			{
				if(jump && jumpPress < 5 && GDc.Player.AirborneFrame < 5)
//				if(jump && jumpPress < 5 && GDc.Player.TouchGround) // old
					GDc.Player.JumpFrame = 1;
			}

			if(camSlide)
			{
				if(GetInput(INP_DIR_4))
				{
					GDc.CamSlideCount++;
					GDc.CamSlideX--;
				}
				if(GetInput(INP_DIR_6))
				{
					GDc.CamSlideCount++;
					GDc.CamSlideX++;
				}
				if(GetInput(INP_DIR_8))
				{
					GDc.CamSlideCount++;
					GDc.CamSlideY--;
				}
				if(GetInput(INP_DIR_2))
				{
					GDc.CamSlideCount++;
					GDc.CamSlideY++;
				}
				m_range(GDc.CamSlideX, -1, 1);
				m_range(GDc.CamSlideY, -1, 1);
			}
			else
			{
				if(GDc.CamSlideMode && GDc.CamSlideCount == 0)
				{
					GDc.CamSlideX = 0;
					GDc.CamSlideY = 0;
				}
				GDc.CamSlideCount = 0;
			}
			GDc.CamSlideMode = camSlide;
		}

		// プレイヤー移動
		{
			if(GDc.Player.MoveFrame)
			{
				double speed = 0.0;

				if(GDc.Player.MoveSlow)
				{
					speed = GDc.Player.MoveFrame * 0.25;
					m_minim(speed, 3.0);
				}
				else
					speed = 6.0;

				speed *= GDc.Player.FacingLeft ? -1 : 1;

				GDc.Player.X += speed;
			}
			else
				GDc.Player.X = (double)d2i(GDc.Player.X);

			GDc.Player.YSpeed += 1.0; // += 重力加速度

			if(GDc.Player.JumpFrame)
				GDc.Player.YSpeed = -8.0;

			m_minim(GDc.Player.YSpeed, 8.0); // 落下する最高速度

			GDc.Player.Y += GDc.Player.YSpeed;
		}

		// プレイヤー位置矯正
		{
			int touchSide_L = 
				GetMapCell(PositionToMapCellPoint(GDc.Player.X - 10.0, GDc.Player.Y - MAP_TILE_WH / 2))->Wall ||
				GetMapCell(PositionToMapCellPoint(GDc.Player.X - 10.0, GDc.Player.Y                  ))->Wall ||
				GetMapCell(PositionToMapCellPoint(GDc.Player.X - 10.0, GDc.Player.Y + MAP_TILE_WH / 2))->Wall;

			int touchSide_R =
				GetMapCell(PositionToMapCellPoint(GDc.Player.X + 10.0, GDc.Player.Y - MAP_TILE_WH / 2))->Wall ||
				GetMapCell(PositionToMapCellPoint(GDc.Player.X + 10.0, GDc.Player.Y                  ))->Wall ||
				GetMapCell(PositionToMapCellPoint(GDc.Player.X + 10.0, GDc.Player.Y + MAP_TILE_WH / 2))->Wall;

			if(touchSide_L && touchSide_R)
			{
				// noop
			}
			else if(touchSide_L)
			{
				GDc.Player.X = d2i(GDc.Player.X / MAP_TILE_WH) * MAP_TILE_WH + 10.0;
			}
			else if(touchSide_R)
			{
				GDc.Player.X = d2i(GDc.Player.X / MAP_TILE_WH) * MAP_TILE_WH - 10.0;
			}

			int touchCeiling_L = GetMapCell(PositionToMapCellPoint(GDc.Player.X - 9.0, GDc.Player.Y - MAP_TILE_WH))->Wall;
			int touchCeiling_R = GetMapCell(PositionToMapCellPoint(GDc.Player.X + 9.0, GDc.Player.Y - MAP_TILE_WH))->Wall;
			int touchCeiling = touchCeiling_L && touchCeiling_R;

			if(touchCeiling_L && touchCeiling_R)
			{
				if(GDc.Player.YSpeed < 0.0)
				{
					GDc.Player.Y = (int)(GDc.Player.Y / MAP_TILE_WH + 1) * MAP_TILE_WH;
					GDc.Player.YSpeed = 0.0;
					GDc.Player.JumpFrame = 0;
				}
			}
			else if(touchCeiling_L)
			{
				GDc.Player.X = d2i(GDc.Player.X / MAP_TILE_WH) * MAP_TILE_WH + 9.0;
			}
			else if(touchCeiling_R)
			{
				GDc.Player.X = d2i(GDc.Player.X / MAP_TILE_WH) * MAP_TILE_WH - 9.0;
			}

			GDc.Player.TouchGround =
				GetMapCell(PositionToMapCellPoint(GDc.Player.X - 9.0, GDc.Player.Y + MAP_TILE_WH))->Wall ||
				GetMapCell(PositionToMapCellPoint(GDc.Player.X + 9.0, GDc.Player.Y + MAP_TILE_WH))->Wall;

			if(GDc.Player.TouchGround)
			{
				m_minim(GDc.Player.YSpeed, 0.0);

				double plY = (int)(GDc.Player.Y / MAP_TILE_WH) * MAP_TILE_WH;

				m_minim(GDc.Player.Y, plY);
			}

			if(GDc.Player.TouchGround)
				GDc.Player.AirborneFrame = 0;
			else
				GDc.Player.AirborneFrame++;
		}

		// 描画ここから

		DrawWall();
		DrawMap();
		DrawPlayer();

		// debug
		{
			SetPrint();
			Print_x(xcout("%f %f %f %f", GDc.Player.X, GDc.Player.Y, CameraX, CameraY));
		}

		EachFrame();
	}
	FreezeInput();
	MusicFade();
	SetCurtain(30, -1.0);

	forscene(40)
	{
		DrawWall();
		EachFrame();
	}
	sceneLeave();
}
