#include "all.h"

GDc_t GDc;

void GameInit(void)
{
	memset(&GDc, 0x00, sizeof(GDc));

	GDc.Prm_StartX = GetMap_W() * MAP_TILE_WH / 2.0;
	GDc.Prm_StartY = GetMap_H() * MAP_TILE_WH / 2.0;
}
void GameFnlz(void)
{
	memset(&GDc, 0x00, sizeof(GDc));
}
