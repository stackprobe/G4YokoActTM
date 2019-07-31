#include "all.h"

i2D_t PointToMapCellPoint(double x, double y) // ピクセル座標 -> GetMapCellの座標
{
	int mapTileX = (int)(x / MAP_TILE_WH);
	int mapTileY = (int)(y / MAP_TILE_WH);

	return makeI2D(mapTileX, mapTileY);
}

#define INPUT_STRING_LENMAX 100

char *InputString(char *initialValue)
{
	errorCase(INPUT_STRING_LENMAX < strlen(initialValue));

	int inputHdl = MakeKeyInput(INPUT_STRING_LENMAX, 0, 0, 0);
	char buff[INPUT_STRING_LENMAX + 1];

	FreezeInput();

	SetKeyInputString(initialValue, inputHdl);
	SetActiveKeyInput(inputHdl);

	for(; ; )
	{
		if(CheckKeyInput(inputHdl))
			break;

		DPE_SetBright(GetColor(128, 0, 128));
		DrawRect(P_WHITEBOX, 0, 0, SCREEN_W, SCREEN_H);
		DPE_Reset();

		GetKeyInputString(buff, inputHdl);

		SetPrint((SCREEN_W - strlen(buff) * 8) / 2, (SCREEN_H - 16) / 2);
		Print(buff);

		EachFrame();
	}
	FreezeInput();
	DeleteKeyInput(inputHdl);

	return strx(buff);
}
char *InputString_x(char *initialValue)
{
	char *ret = InputString(initialValue);
	memFree(initialValue);
	return ret;
}
