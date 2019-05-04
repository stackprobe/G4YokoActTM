/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int IgnoreEscapeKey;

// ëºÇÃÉtÉ@ÉCÉãÇ©ÇÁÇÕ read only {
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
__int64 FrameStartTime;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
__int64 LangolierTime;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
double EatenByLangolierEval = 0.5;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int ProcFrame;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int FreezeInputFrame;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int WindowIsActive;
// }

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void CheckHz(void)
{
	__int64 currTime = GetCurrTime();

	if(!ProcFrame)
		LangolierTime = currTime;
	else
		LangolierTime += 16; // 16.666 ÇÊÇËè¨Ç≥Ç¢ÇÃÇ≈ÅA60HzÇ»ÇÁÇ«ÇÒÇ«ÇÒà¯Ç´ó£Ç≥ÇÍÇÈÇÕÇ∏ÅB
//		LangolierTime += 17; // test -- EBLE 0.20 Ç†ÇΩÇË
//		LangolierTime += 18; // test -- EBLE 0.45 Ç†ÇΩÇË
//		LangolierTime += 19; // test -- EBLE 0.59 Ç†ÇΩÇË
//		LangolierTime += 20; // test -- EBLE 0.67 Ç†ÇΩÇË

	while(currTime < LangolierTime)
	{
		Sleep(1);

		// DxLib >

		ScreenFlip();

		if(ProcessMessage() == -1)
		{
			EndProc();
		}

		// < DxLib

		currTime = GetCurrTime();
		m_approach(EatenByLangolierEval, 1.0, 0.9);
	}
	EatenByLangolierEval *= 0.99;

	FrameStartTime = currTime;
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void EachFrame(void)
{
	if(!SEEachFrame())
	{
		MusicEachFrame();
	}
	Gnd.EL->ExecuteAllTask();
	CurtainEachFrame();

	if(900 < ProcFrame && 0.1 < EatenByLangolierEval) // ébíË ébíË ébíË ébíË ébíË
	{
		static int passedCount = 900;

		if(m_countDown(passedCount))
		{
			DPE_SetBright(GetColor(128, 0, 0));
			DPE_SetAlpha(0.5);
			DrawRect(P_WHITEBOX, 0, 0, SCREEN_W, 16);
			DPE_Reset();

			SetPrint();
			PE.Color = GetColor(255, 255, 0);
			Print_x(xcout("V-SYNC ALERT / EBLE=%.3f FST=%I64d LT=%I64d (%d) %d", EatenByLangolierEval, FrameStartTime, LangolierTime, passedCount, ProcFrame));
			PE_Reset();
		}
	}

	// app > @ before draw screen

	// < app

	if(Gnd.MainScreen && CurrDrawScreenHandle == GetHandle(Gnd.MainScreen))
	{
		ChangeDrawScreen(DX_SCREEN_BACK);

		// app > @ draw screen

		if(Gnd.RealScreenDraw_W == -1)
		{
			errorCase(DrawExtendGraph(0, 0, Gnd.RealScreen_W, Gnd.RealScreen_H, GetHandle(Gnd.MainScreen), 0)); // ? é∏îs
		}
		else
		{
			errorCase(DrawBox(0, 0, Gnd.RealScreen_W, Gnd.RealScreen_H, GetColor(0, 0, 0), 1)); // ? é∏îs
			errorCase(DrawExtendGraph(
				Gnd.RealScreenDraw_L,
				Gnd.RealScreenDraw_T,
				Gnd.RealScreenDraw_L + Gnd.RealScreenDraw_W,
				Gnd.RealScreenDraw_T + Gnd.RealScreenDraw_H, GetHandle(Gnd.MainScreen), 0)); // ? é∏îs
		}

		// < app
	}

	// app > @ post draw screen

	// < app

	// DxLib >

	ScreenFlip();

	if(!IgnoreEscapeKey && CheckHitKey(KEY_INPUT_ESCAPE) == 1 || ProcessMessage() == -1)
	{
		EndProc();
	}

	// < DxLib

	CheckHz();

	ProcFrame++;
	errorCase(IMAX < ProcFrame); // 192.9ì˙íˆìxÇ≈ÉJÉìÉXÉg
	m_countDown(FreezeInputFrame);
	WindowIsActive = IsWindowActive();

	PadEachFrame();
	KeyEachFrame();
	InputEachFrame();
	MouseEachFrame();

	if(Gnd.RealScreen_W != SCREEN_W || Gnd.RealScreen_H != SCREEN_H)
	{
		if(!Gnd.MainScreen)
			Gnd.MainScreen = CreateSubScreen(SCREEN_W, SCREEN_H);

		ChangeDrawScreen(Gnd.MainScreen);
	}
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void FreezeInput(int frame) // frame: 1 == Ç±ÇÃÉtÉåÅ[ÉÄÇÃÇ›, 2 == Ç±ÇÃÉtÉåÅ[ÉÄÇ∆éüÇÃÉtÉåÅ[ÉÄ ...
{
	errorCase(frame < 1 || IMAX < frame);
	FreezeInputFrame = frame;
}
