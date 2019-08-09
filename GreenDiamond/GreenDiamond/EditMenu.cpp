#include "all.h"

#define MENU_L 0
#define MENU_T 0
#define MENU_W 200
#define MENU_H SCREEN_H

#define WALL_PIC_X 100
#define WALL_PIC_Y 500

#define ENEMY_PIC_X 100
#define ENEMY_PIC_Y 600

static int InputWallFlag = 1;
static int InputPicIdFlag = 1;
static int InputEnemyIdFlag = 1;
static int InputEventNameFlag = 1;

static int DisplayWallFlag = 0;
static int DisplayPicIdFlag = 0;
static int DisplayEnemyIdFlag = 0;
static int DisplayEventNameFlag = 0;

static int Wall = 1;
static int PicId = P_MAP_TILE_00 + 0;
static int EnemyId = -1;
static char *EventName;

static char *Status;
static int CursorMenuItemIndex = -1; // -1 == 未フォーカス

void InitEditMenu(void)
{
	EventName = strx("");
	Status = strx("");
}
static void SetStatus(char *status)
{
	strz_x(Status, xcout(".......... %s", status));
}
void EditMenuEachFrame(void)
{
	CursorMenuItemIndex = -1;

	if(ProcFrame % 6 == 0 && *Status)
		memmove(Status, Status + 1, strlen(Status));

	if(GetKeyInput(KEY_INPUT_S) == 1)
	{
		ML_SaveMapToLastLoadedFile();
		SetStatus("SAVED");
	}

	if(IsOut(MouseX, MouseY, MENU_L, MENU_T, MENU_W, MENU_H))
	{
		MapCell_t *cell = TryGetMapCell((MouseX + GDc.ICameraX) / MAP_TILE_WH, (MouseY + GDc.ICameraY) / MAP_TILE_WH);

		if(cell)
		{
			if(1 <= GetMouInput(MOUBTN_L))
			{
				if(InputWallFlag)      cell->Wall = Wall;
				if(InputPicIdFlag)     cell->PicId = PicId;
				if(InputEnemyIdFlag)   cell->EnemyId = EnemyId;
				if(InputEventNameFlag) strz(cell->EventName, EventName);
			}
			if(1 <= GetMouInput(MOUBTN_R))
			{
				if(InputWallFlag)      cell->Wall = 0;
				if(InputPicIdFlag)     cell->PicId = -1;
				if(InputEnemyIdFlag)   cell->EnemyId = -1;
				if(InputEventNameFlag) *cell->EventName = '\0';
			}
		}
	}
	else
	{
		CursorMenuItemIndex = MouseY / 16;

		if(!m_isRange(CursorMenuItemIndex, 0, 7)) // メニュー項目数と同期すること。
			CursorMenuItemIndex = -1;

		int flag = -1;

		if(1 <= GetMouInput(MOUBTN_L)) flag = 1;
		if(1 <= GetMouInput(MOUBTN_R)) flag = 0;

		if(flag != -1)
		{
			switch(CursorMenuItemIndex)
			{
			case -1:
				break;

			case 0: InputWallFlag = flag; break;
			case 1: InputPicIdFlag = flag; break;
			case 2: InputEnemyIdFlag = flag; break;
			case 3: InputEventNameFlag = flag; break;
			case 4: DisplayWallFlag = flag; break;
			case 5: DisplayPicIdFlag = flag; break;
			case 6: DisplayEnemyIdFlag = flag; break;
			case 7: DisplayEventNameFlag = flag; break;

			default:
				error();
			}
		}
	}
}
void DrawEditMenu(void)
{
	DPE_SetBright(GetColor(64, 0, 128));
	DPE_SetAlpha(0.8);
	DrawRect(P_WHITEBOX, MENU_L, MENU_T, MENU_W, MENU_H);
	DPE_Reset();

	if(CursorMenuItemIndex != -1)
	{
		DPE_SetBright(GetColor(128, 0, 255));
		DPE_SetAlpha(0.5);
		DrawRect(P_WHITEBOX, MENU_L, MENU_T + CursorMenuItemIndex * 16, MENU_W, 16);
		DPE_Reset();
	}

	i2D_t currCellPt = PointToMapCellPoint(GDc.ICameraX + MouseX, GDc.ICameraY + MouseY);
	MapCell_t *currCell = GetMapCell(currCellPt);

	PE_Border(GetColor(32, 0, 64));
	SetPrint(); Print_x(xcout("INPUT WALL: %d", InputWallFlag));
	PrintRet(); Print_x(xcout("INPUT PIC-ID: %d", InputPicIdFlag));
	PrintRet(); Print_x(xcout("INPUT ENEMY-ID: %d", InputEnemyIdFlag));
	PrintRet(); Print_x(xcout("INPUT EVENT-NAME: %d", InputEventNameFlag));
	PrintRet(); Print_x(xcout("DISPLAY WALL: %d", DisplayWallFlag));
	PrintRet(); Print_x(xcout("DISPLAY PIC-ID: %d", DisplayPicIdFlag));
	PrintRet(); Print_x(xcout("DISPLAY ENEMY-ID: %d", DisplayEnemyIdFlag));
	PrintRet(); Print_x(xcout("DISPLAY EVENT-NAME: %d", DisplayEventNameFlag));
	PrintRet(); Print_x(xcout("WALL: %d", Wall));
	PrintRet(); Print_x(xcout("PIC-ID: %d", PicId));
	PrintRet(); Print_x(xcout("ENEMY-ID: %d", EnemyId));
	PrintRet(); Print_x(xcout("EVENT-NAME: %s", EventName));
	PrintRet(); Print_x(xcout("CURR-PT: %d, %d", currCellPt.X, currCellPt.Y));
	PrintRet(); Print_x(xcout("CURR-WALL: %d", currCell->Wall));
	PrintRet(); Print_x(xcout("CURR-PIC-ID: %d", currCell->PicId));
	PrintRet(); Print_x(xcout("CURR-ENEMY-ID: %d", currCell->EnemyId));
	PrintRet(); Print_x(xcout("CURR-EVENT-NAME: %s", currCell->EventName));
	PE_Reset();

	PE.Color = GetColor(255, 255, 128);
	PE_Border(GetColor(64, 32, 0));
	SetPrint(0, SCREEN_H - 16);
	Print(Status);
	PE_Reset();
}
