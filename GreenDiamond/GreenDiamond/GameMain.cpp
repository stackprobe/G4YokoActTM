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

	if(!lookLeftFrm && dRnd() < 0.002)
		lookLeftFrm = 150 + rnd(90);

	m_countDown(lookLeftFrm);

	int picId = D_PLAYER_STAND_1_00 + (120 < lookLeftFrm ? 2 : 0) + ProcFrame / 20 % 2 | DTP;

	if(GDc.Player.Walking)
	{
		picId = D_PLAYER_WALK_00 + ProcFrame / 10 % 2 | DTP;
	}
	if(GDc.Player.Dashing)
	{
		picId = D_PLAYER_DASH_00 + ProcFrame / 5 % 2 | DTP;
	}

	SetPicRes(GetLTTransPicRes());
	DrawBegin(
		picId,
		d2i(GDc.Player.X - GDc.ICameraX),
		d2i(GDc.Player.Y - GDc.ICameraY) - 16
		);
	DrawZoom_X(GDc.Player.Direction == 4 ? -1.0 : 1.0);
	DrawEnd();
	ResetPicRes();
}
void GameMain(void)
{
	GDc.Player.X = GDc.Prm_StartX;
	GDc.Player.Y = GDc.Prm_StartY;

	CameraX = GDc.Player.X - SCREEN_W / 2.0;
	CameraY = GDc.Player.Y - SCREEN_H / 2.0;

	GDc.Player.Direction = 6;

	SetCurtain();
	FreezeInput();

//	MusicPlay(MUS_XXX);

	for(; ; )
	{
		m_approach(CameraX, GDc.Player.X - SCREEN_W / 2.0, 0.9);
		m_approach(CameraY, GDc.Player.Y - SCREEN_H / 2.0, 0.9);

		m_range(CameraX, 0.0, GetMap_W() * MAP_TILE_WH - SCREEN_W);
		m_range(CameraY, 0.0, GetMap_H() * MAP_TILE_WH - SCREEN_H);

		GDc.ICameraX = d2i(CameraX);
		GDc.ICameraY = d2i(CameraY);

		// プレイヤー入力
		{
			int moving = 0;
			int walking = 0;

			if(1 <= GetInput(INP_DIR_4))
			{
				moving = -1;
				GDc.Player.Direction = 4;
			}
			if(1 <= GetInput(INP_DIR_6))
			{
				moving = 1;
				GDc.Player.Direction = 6;
			}
			if(1 <= GetInput(INP_R))
			{
				walking = 1;
			}

			double moveSpeed = 0.0;
			
			if(moving)
				moveSpeed = moving * (walking ? 3.0 : 6.0);

			GDc.Player.Walking = moving &&  walking;
			GDc.Player.Dashing = moving && !walking;

			double rate = 0.6;

			if(moving)
				rate = walking ? 0.8 : 0.7;

			m_approach(GDc.Player.XAdd, moveSpeed, rate);

			if(1 <= GetInput(INP_A))
			{
				GDc.Player.YAdd = -10.0;
			}
		}

		// プレイヤー移動
		{
			GDc.Player.YAdd += 1.0; // 重力加速度

			m_range(GDc.Player.YAdd, -100.0, 8.0);

			GDc.Player.X += GDc.Player.XAdd;
			GDc.Player.Y += GDc.Player.YAdd;
		}

		// プレイヤー状態確認と矯正
		{
			int plMapTileX = d2i(GDc.Player.X / MAP_TILE_WH);
			int plMapTileY = d2i(GDc.Player.Y / MAP_TILE_WH);

			GDc.Player.TouchGround = GetMapCell(plMapTileX, plMapTileY + 1)->Wall;

			if(GDc.Player.TouchGround)
			{
				m_minim(GDc.Player.Y, plMapTileY * MAP_TILE_WH);
				m_maxim(GDc.Player.YAdd, 0.0);
			}
			if(abs(GDc.Player.XAdd) < 0.001)
			{
				GDc.Player.X = (double)d2i(GDc.Player.X);
			}
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
