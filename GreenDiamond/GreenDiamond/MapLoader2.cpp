#include "all.h"

static char *ActiveFile = NULL;

static int Default_W = 100;
static int Default_H = 30;

void ML_SetFile(char *file)
{
	memFree(ActiveFile);
	ActiveFile = strx(file);
}
void ML_ClearFile(void)
{
	memFree(ActiveFile);
	ActiveFile = NULL;
}
void ML_SetSize(int w, int h)
{
	Default_W = w;
	Default_H = h;
}
void ML_LoadMap(void)
{
	errorCase(!ActiveFile);

	if(SH_ExistFile(ActiveFile))
		ML_LoadMap(ActiveFile);
	else
		ML_InitMap(Default_W, Default_H);
}
void ML_SaveMap(void)
{
	if(ActiveFile)
	{
		ML_SaveMap(ActiveFile);
	}
}
