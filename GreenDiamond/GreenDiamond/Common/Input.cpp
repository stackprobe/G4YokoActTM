/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
/*
	n�t���[���ԃ{�^���������� ... 0, 0, 0... 0, 1, 2... (n-2), (n-1), n, -1, 0, 0, 0... �ƂȂ�B

	FreezeInput()�ɂ��r���X�L�b�v���邱�Ƃ�����̂ŁA
	���1��������Ƃ��A(-1)�̒��O�͏��1�ȏ�Ƃ��A�Ō�͏��(-1)�ɂȂ�Ƃ��z�肷��̂͊댯�I

	... 0, 0, 0, 1, 2, 0, 4, 5, -1, 0, 0, 0...
	                   ^
	              FreezeInput();

	... 0, 0, 0, 1, 2, 3, 4, 0, -1, 0, 0, 0...
	                         ^
	                    FreezeInput();

	... 0, 0, 0, 1, 2, 3, 4, 5, 0, 0, 0, 0...
	                            ^
	                       FreezeInput();
*/

#include "all.h"

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static int InputStatus[INP_MAX];

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void MixInput(int inpId, int keyId, int btnId)
{
	int freezeInputFrame_BKUP = FreezeInputFrame;
	FreezeInputFrame = 0;

	int keyDown = 1 <= GetKeyInput(keyId);
	int btnDown = 1 <= GetPadInput(Gnd.PrimaryPadId, btnId);

	FreezeInputFrame = freezeInputFrame_BKUP;

	updateInput(InputStatus[inpId], keyDown || btnDown);
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void InputEachFrame(void)
{
	MixInput(INP_DIR_2, Gnd.KbdKeyId.Dir_2, Gnd.PadBtnId.Dir_2);
	MixInput(INP_DIR_4, Gnd.KbdKeyId.Dir_4, Gnd.PadBtnId.Dir_4);
	MixInput(INP_DIR_6, Gnd.KbdKeyId.Dir_6, Gnd.PadBtnId.Dir_6);
	MixInput(INP_DIR_8, Gnd.KbdKeyId.Dir_8, Gnd.PadBtnId.Dir_8);
	MixInput(INP_A, Gnd.KbdKeyId.A, Gnd.PadBtnId.A);
	MixInput(INP_B, Gnd.KbdKeyId.B, Gnd.PadBtnId.B);
	MixInput(INP_C, Gnd.KbdKeyId.C, Gnd.PadBtnId.C);
	MixInput(INP_D, Gnd.KbdKeyId.D, Gnd.PadBtnId.D);
	MixInput(INP_E, Gnd.KbdKeyId.E, Gnd.PadBtnId.E);
	MixInput(INP_F, Gnd.KbdKeyId.F, Gnd.PadBtnId.F);
	MixInput(INP_L, Gnd.KbdKeyId.L, Gnd.PadBtnId.L);
	MixInput(INP_R, Gnd.KbdKeyId.R, Gnd.PadBtnId.R);
	MixInput(INP_PAUSE, Gnd.KbdKeyId.Pause, Gnd.PadBtnId.Pause);
	MixInput(INP_START, Gnd.KbdKeyId.Start, Gnd.PadBtnId.Start);
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int GetInput(int inpId)
{
	errorCase(inpId < 0 || INP_MAX <= inpId);

	return FreezeInputFrame ? 0 : InputStatus[inpId];
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int GetPound(int inpId)
{
	errorCase(inpId < 0 || INP_MAX <= inpId);

	return FreezeInputFrame ? 0 : isPound(InputStatus[inpId]);
}
