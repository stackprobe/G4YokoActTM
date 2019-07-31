#include "all.h"

void ProcMain(void)
{
#if !LOG_ENABLED
	TitleMenu();
#elif 0 // test
	TitleMenu();
#elif 1 // test
	ML_InitMap(100, 30);
	GameInit();
	GDc.Prm_StartX = GetMap_W() * MAP_TILE_WH * 0.5;
	GDc.Prm_StartY = GetMap_H() * MAP_TILE_WH * 0.5;
	GameMain();
	GameFnlz();
#else // test
	Test0001();
#endif
}
