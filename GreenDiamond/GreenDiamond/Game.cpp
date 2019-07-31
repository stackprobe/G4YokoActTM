#include "all.h"

GDc_t GDc;

void GameInit(void)
{
	memset(&GDc, 0x00, sizeof(GDc));

	GDc.Prm_StartX = 0.0;
	GDc.Prm_StartY = 0.0;
}
void GameFnlz(void)
{
	memset(&GDc, 0x00, sizeof(GDc));
}
