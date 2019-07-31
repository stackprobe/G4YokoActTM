#include "all.h"

#define MENU_L 0
#define MENU_T 0
#define MENU_W 200
#define MENU_H SCREEN_H

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

void InitEditMenu(void)
{
	EventName = strx("");
}
static int IsOutOfEditMenu(double x, double y)
{
	return IsOut(x, y, MENU_L, MENU_T, MENU_W, MENU_H);
}
void EditMenuEachFrame(void)
{
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
		// TODO ƒƒjƒ…[‘€ì
	}
}
void DrawEditMenu(void)
{
	DPE_SetBright(GetColor(64, 0, 128));
	DPE_SetAlpha(0.8);
	DrawRect(P_WHITEBOX, MENU_L, MENU_T, MENU_W, MENU_H);
	DPE_Reset();

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
}
