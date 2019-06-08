#include "all.h"

void Test0001(void)
{
	const int MAX = 45;
	int i = 0;

	SetCurtain();
	FreezeInput();

	for(; ; )
	{
		if(1 <= GetInput(INP_A))
		{
			break;
		}
		if(GetInput(INP_DIR_4) == 1)
		{
			i--;
		}
		if(GetInput(INP_DIR_6) == 1)
		{
			i++;
		}
		i = (i + MAX) % MAX;

		DrawCurtain();

		SetPrint();
		Print_x(xcout("%d", i));

		DrawCenter(D_PLAYER_STAND_1_00 + i | DTP, SCREEN_CENTER_X, SCREEN_CENTER_Y);
		EachFrame();
	}
	FreezeInput();
}
