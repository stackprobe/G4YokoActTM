/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SmplMenu_Color = -1;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SmplMenu_BorderColor = -1;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SmplMenu_WallColor = -1;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SmplMenu_WallPicId = -1;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
double SmplMenu_WallCurtain = 0.0;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SmplMenu_X = 16;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SmplMenu_Y = 16;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SmplMenu_YStep = 32;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SmplMenu(char *menuTitle, char **menuItems, int selectMax, int selectIndex)
{
	SetCurtain();
	FreezeInput();

	for(; ; )
	{
		if(GetPound(INP_A))
		{
			break;
		}
		if(GetPound(INP_B))
		{
			if(selectIndex == selectMax - 1)
				break;

			selectIndex = selectMax - 1;
		}
		if(GetPound(INP_DIR_8))
		{
			selectIndex--;
		}
		if(GetPound(INP_DIR_2))
		{
			selectIndex++;
		}

		selectIndex += selectMax;
		selectIndex %= selectMax;

		DrawCurtain();

		if(SmplMenu_WallColor != -1)
			DrawBox(0, 0, SCREEN_W, SCREEN_H, SmplMenu_WallColor, 1);

		if(SmplMenu_WallPicId != -1)
		{
			DrawRect(SmplMenu_WallPicId, 0, 0, SCREEN_W, SCREEN_H);
//			DrawCenter(SmplMenu_WallPicId, SCREEN_W / 2.0, SCREEN_H / 2.0); // old
			DrawCurtain(SmplMenu_WallCurtain);
		}
		if(SmplMenu_Color != -1)
			PE.Color = SmplMenu_Color;

		if(SmplMenu_BorderColor != -1)
			PE_Border(SmplMenu_BorderColor);

		SetPrint(SmplMenu_X, SmplMenu_Y, SmplMenu_YStep);
//		SetPrint(16, 16, 32); // old
		Print(menuTitle);
		PrintRet();

		for(int c = 0; c < selectMax; c++)
		{
			Print_x(xcout("[%c]�@%s\n", selectIndex == c ? '>' : ' ', menuItems[c]));
			PrintRet();
		}
		PE_Reset();

		EachFrame();
	}
	FreezeInput();

	return selectIndex;
}

// ---- �{�^���ݒ� ----

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void *PadBtnIdBkup;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void RestorePadBtnId(void)
{
	memcpy(&Gnd.PadBtnId, PadBtnIdBkup, sizeof(Gnd.PadBtnId));
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void SmplPadConfig(void)
{
	int *BtnPList[INP_MAX] =
	{
		&Gnd.PadBtnId.Dir_2,
		&Gnd.PadBtnId.Dir_4,
		&Gnd.PadBtnId.Dir_6,
		&Gnd.PadBtnId.Dir_8,
		&Gnd.PadBtnId.A,
		&Gnd.PadBtnId.B,
		&Gnd.PadBtnId.C,
		&Gnd.PadBtnId.D,
		&Gnd.PadBtnId.E,
		&Gnd.PadBtnId.F,
		&Gnd.PadBtnId.L,
		&Gnd.PadBtnId.R,
		&Gnd.PadBtnId.Pause,
		&Gnd.PadBtnId.Start,
	};

	/*
		NULL == �g�p���Ă��Ȃ��B
	*/
	char *BTN_LIST[INP_MAX] =
	{
		// app > @ BTN_LIST

		"��", // INP_DIR_2
		"��", // INP_DIR_4
		"�E", // INP_DIR_6
		"��", // INP_DIR_8
		"�V���b�g�{�^��", // INP_A
		"�ᑬ�{�^��", // INP_B
		"�{���{�^��", // INP_C
		NULL, // INP_D
		NULL, // INP_E
		NULL, // INP_F
		NULL, // INP_L
		NULL, // INP_R
		"�|�[�Y�{�^��", // INP_PAUSE
		NULL, // INP_START

		// < app
	};

	PadBtnIdBkup = memClone(&Gnd.PadBtnId, sizeof(Gnd.PadBtnId));
	GetEndProcFinalizers()->AddFunc(RestorePadBtnId);

	for(int c = 0; c < INP_MAX; c++)
		*BtnPList[c] = -1;

	SetCurtain();
	FreezeInput();

	int currBtnIndex = 0;

	while(currBtnIndex < INP_MAX)
	{
		if(!BTN_LIST[currBtnIndex])
		{
			currBtnIndex++;
			continue;
		}

		if(GetKeyInput(KEY_INPUT_SPACE) == 1)
		{
			RestorePadBtnId();
			break;
		}
		if(GetKeyInput(KEY_INPUT_Z) == 1)
		{
			currBtnIndex++;
			goto endInput;
		}
		int pressBtnId = -1;

		for(int padId = 0; padId < GetPadCount(); padId++)
		for(int btnId = 0; btnId < PAD_BUTTON_MAX; btnId++)
			if(GetPadInput(padId, btnId) == 1)
				pressBtnId = btnId;

		for(int c = 0; c < currBtnIndex; c++)
			if(*BtnPList[c] == pressBtnId)
				pressBtnId = -1;

		if(pressBtnId != -1)
		{
			*BtnPList[currBtnIndex] = pressBtnId;
			currBtnIndex++;
		}
endInput:

		DrawCurtain();

		if(SmplMenu_WallPicId != -1)
		{
			DrawRect(SmplMenu_WallPicId, 0, 0, SCREEN_W, SCREEN_H);
//			DrawCenter(SmplMenu_WallPicId, SCREEN_W / 2.0, SCREEN_H / 2.0); // old
			DrawCurtain(SmplMenu_WallCurtain);
		}
		if(SmplMenu_Color != -1)
			PE.Color = SmplMenu_Color;

		if(SmplMenu_BorderColor != -1)
			PE_Border(SmplMenu_BorderColor);

		SetPrint(SmplMenu_X, SmplMenu_Y, SmplMenu_YStep);
//		SetPrint(16, 16, 32); // old
		Print("�Q�[���p�b�h�̃{�^���ݒ�");
		PrintRet();

		for(int c = 0; c < INP_MAX; c++)
		{
			if(!BTN_LIST[c])
				continue;

			Print_x(xcout("[%c]�@%s", currBtnIndex == c ? '>' : ' ', BTN_LIST[c]));

			if(c < currBtnIndex)
			{
				int btnId = *BtnPList[c];

				Print("�@->�@");

				if(btnId == -1)
					Print("���蓖�ăi�V");
				else
					Print_x(xcout("%d", btnId));
			}
			PrintRet();
		}
		Print("���@�J�[�\���̋@�\�Ɋ��蓖�Ă�{�^���������ĉ������B");
		PrintRet();
		Print("���@�X�y�[�X�������ƃL�����Z�����܂��B");
		PrintRet();
		Print("���@[Z]�������ƃ{�^���̊��蓖�Ă��X�L�b�v���܂��B");
		PrintRet();

		EachFrame();
	}

	GetEndProcFinalizers()->RemoveFunc(RestorePadBtnId);
	memFree(PadBtnIdBkup);

	FreezeInput();
}

// ---- ��ʃT�C�Y ----

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void SmplWindowSizeConfig(void)
{
	char *MENU_ITEMS[] =
	{
		"800 x 600 (�f�t�H���g)",
		"900 x 675",
		"1000 x 750",
		"1100 x 825",
		"1200 x 900",
		"1300 x 975",
		"1400 x 1050",
		"1500 x 1125",
		"1600 x 1200",
		"1700 x 1275",
		"1800 x 1350",
		"�t���X�N���[��",
		"�t���X�N���[�� (�c����ێ�)",
		"�t���X�N���[�� (���w�i)",
		"�߂�",
	};

	int selectIndex = 0;

	for(; ; )
	{
		selectIndex = SmplMenu("�E�B���h�E�T�C�Y�ݒ�", MENU_ITEMS, lengthof(MENU_ITEMS), selectIndex);

		switch(selectIndex)
		{
		case 0: SetScreenSize(800, 600); break;
		case 1: SetScreenSize(900, 675); break;
		case 2: SetScreenSize(1000, 750); break;
		case 3: SetScreenSize(1100, 825); break;
		case 4: SetScreenSize(1200, 900); break;
		case 5: SetScreenSize(1300, 975); break;
		case 6: SetScreenSize(1400, 1050); break;
		case 7: SetScreenSize(1500, 1125); break;
		case 8: SetScreenSize(1600, 1200); break;
		case 9: SetScreenSize(1700, 1275); break;
		case 10: SetScreenSize(1800, 1350); break;
		case 11: SetScreenSize(Gnd.MonitorRect.W, Gnd.MonitorRect.H); break;
		case 12:
			{
				int w = Gnd.MonitorRect.W;
				int h = (SCREEN_H * Gnd.MonitorRect.W) / SCREEN_W;

				if(Gnd.MonitorRect.H < h)
				{
					h = Gnd.MonitorRect.H;
					w = (SCREEN_W * Gnd.MonitorRect.H) / SCREEN_H;

					errorCase(Gnd.MonitorRect.W < w);
				}
				SetScreenSize(w, h);
			}
			break;

		case 13:
			{
				int w = Gnd.MonitorRect.W;
				int h = (SCREEN_H * Gnd.MonitorRect.W) / SCREEN_W;

				if(Gnd.MonitorRect.H < h)
				{
					h = Gnd.MonitorRect.H;
					w = (SCREEN_W * Gnd.MonitorRect.H) / SCREEN_H;

					errorCase(Gnd.MonitorRect.W < w);
				}
				SetScreenSize(Gnd.MonitorRect.W, Gnd.MonitorRect.H);

				Gnd.RealScreenDraw_L = (Gnd.MonitorRect.W - w) / 2;
				Gnd.RealScreenDraw_T = (Gnd.MonitorRect.H - h) / 2;
				Gnd.RealScreenDraw_W = w;
				Gnd.RealScreenDraw_H = h;
			}
			break;

		case 14:
			goto endLoop;

		default:
			error();
		}
	}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
endLoop:;
}

// ---- �{�����[�� ----
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
/*
	-- e.g.

	static void XXX_BGMVolumeChanged(double volume)
	{
		Gnd.MusicVolume = volume;
		UpdateMusicVolume();
	}
	static void XXX_SEVolumeChanged(double volume)
	{
		Gnd.SEVolume = volume;
		UpdateSEVolume();
	}
	static void XXX_SEVolumePulse(void)
	{
		SEPlay(rndPct(50) ? SE_PAUSE_IN : SE_PAUSE_OUT);
	}

	SmplVolumeConfig("�a�f�l����", Gnd.MusicVolume, 0, 100, 1, 10, XXX_BGMVolumeChanged);

	SmplVolumeConfig("�r�d����", Gnd.SEVolume, 0, 100, 1, 10, XXX_SEVolumeChanged, XXX_SEVolumePulse);
*/

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static double SVC_ValueToRate(double value, double minval, double valRange)
{
	return (double)(value - minval) / valRange;
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
/*
	(ret, rate): 0.0 - 1.0
	pulse: NULL == ����
*/
double SmplVolumeConfig(char *menuTitle, double rate, int minval, int maxval, int valStep, int valFastStep, void (*valChanged)(double), void (*pulse)(void))
{
	const int PULSE_FRM = 60;

	int valRange = maxval - minval;
	int value = minval + d2i(rate * valRange);
	int origval = value;

	SetCurtain();
	FreezeInput();

	for(; ; )
	{
		int chgval = 0;

		if(GetPound(INP_A))
		{
			break;
		}
		if(GetPound(INP_B))
		{
			if(value == origval)
				break;

			value = origval;
			chgval = 1;
		}
		if(GetPound(INP_DIR_8))
		{
			value += valFastStep;
			chgval = 1;
		}
		if(GetPound(INP_DIR_6))
		{
			value += valStep;
			chgval = 1;
		}
		if(GetPound(INP_DIR_4))
		{
			value -= valStep;
			chgval = 1;
		}
		if(GetPound(INP_DIR_2))
		{
			value -= valFastStep;
			chgval = 1;
		}
		if(chgval)
		{
			m_range(value, minval, maxval);
			valChanged(SVC_ValueToRate(value, minval, valRange));
		}
		if(pulse && ProcFrame % PULSE_FRM == 0)
		{
			pulse();
		}

		DrawCurtain();

		if(SmplMenu_WallPicId != -1)
		{
			DrawRect(SmplMenu_WallPicId, 0, 0, SCREEN_W, SCREEN_H);
			DrawCurtain(SmplMenu_WallCurtain);
		}
		if(SmplMenu_Color != -1)
			PE.Color = SmplMenu_Color;

		if(SmplMenu_BorderColor != -1)
			PE_Border(SmplMenu_BorderColor);

		SetPrint(SmplMenu_X, SmplMenu_Y, SmplMenu_YStep);
		Print(menuTitle);
		PrintRet();

		Print_x(xcout("[ %d ]�@�ŏ�=%d�@�ő�=%d", value, minval, maxval));
		PrintRet();

		Print("���@����������");
		PrintRet();
		Print("���@�E���グ��");
		PrintRet();
		Print("���@��������������");
		PrintRet();
		Print("���@�ぁ�����グ��");
		PrintRet();
		Print("���@�������I������猈��{�^���������ĉ������B");
		PrintRet();

		EachFrame();
	}
	FreezeInput();

	return SVC_ValueToRate(value, minval, valRange);
}
