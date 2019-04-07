#include "all.h"

static void DrawWall(void)
{
	DrawCurtain();
	DrawRect(P_WHITEBOX, 100, 100, SCREEN_W - 200, SCREEN_H - 200);
}

static void LeaveTitleMenu(void)
{
	MusicFade();
	SetCurtain(30, -1.0);
}
static void ReturnTitleMenu(void)
{
//	MusicPlay(MUS_TITLE);
}

static void BGMVolumeChanged(double volume)
{
	Gnd.MusicVolume = volume;
	UpdateMusicVolume();
}
static void SEVolumeChanged(double volume)
{
	Gnd.SEVolume = volume;
	UpdateSEVolume();
}
static void SEVolumePulse(void)
{
//	SEPlay(rndPct(50) ? SE_PAUSE_IN : SE_PAUSE_OUT);
}
static void SettingMenu(void)
{
	SetCurtain();
	FreezeInput();

	char *MENU_ITEMS[] =
	{
		"�p�b�h�̃{�^���ݒ�",
		"�E�B���h�E�T�C�Y�ύX",
		"�a�f�l����",
		"�r�d����",
		"�߂�",
	};
	int selectIndex = 0;

	for(; ; )
	{
		selectIndex = SmplMenu("�ݒ�", MENU_ITEMS, lengthof(MENU_ITEMS), selectIndex);

		switch(selectIndex)
		{
		case 0:
			SmplPadConfig();
			break;

		case 1:
			SmplWindowSizeConfig();
			break;

		case 2:
			SmplVolumeConfig("�a�f�l����", Gnd.MusicVolume, 0, 100, 1, 10, BGMVolumeChanged); 
			break;

		case 3:
			SmplVolumeConfig("�r�d����", Gnd.SEVolume, 0, 100, 1, 10, SEVolumeChanged, SEVolumePulse); 
			break;

		case 4:
			goto endMenu;

		default:
			error();
		}
	}
endMenu:
	FreezeInput();
}
void TitleMenu(void)
{
	SetCurtain();
	FreezeInput();

//	MusicPlay(MUS_TITLE);

	char *MENU_ITEMS[] =
	{
		"�Q�[���X�^�[�g",
		"�R���e�j���[�H",
		"�ݒ�",
		"�I��",
	};
	int selectIndex = 0;

	for(; ; )
	{
		selectIndex = SmplMenu("GreenDiamond", MENU_ITEMS, lengthof(MENU_ITEMS), selectIndex);

		switch(selectIndex)
		{
		case 0:
			LeaveTitleMenu();
			GameInit();
			GameMain();
			GameFnlz();
			ReturnTitleMenu();
			break;

		case 1:
			// TODO
			break;

		case 2:
			SettingMenu();
			break;

		case 3:
			goto endMenu;

		default:
			error();
		}
	}
endMenu:
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
