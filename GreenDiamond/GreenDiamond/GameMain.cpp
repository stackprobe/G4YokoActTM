#include "all.h"

static void DrawWall(void)
{
	DrawCurtain();
}
void GameMain(void)
{
	SetCurtain();
	FreezeInput();

//	MusicPlay(MUS_XXX);

	for(; ; )
	{
		if(GetPound(INP_PAUSE))
		{
			break;
		}



		DrawWall();
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
