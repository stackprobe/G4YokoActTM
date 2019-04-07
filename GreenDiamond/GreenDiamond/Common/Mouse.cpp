/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int MouseRot;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static int MouseStatus[MOUBTN_MAX];

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void MouseEachFrame(void)
{
	int status;

	if(WindowIsActive)
	{
		MouseRot = GetMouseWheelRotVol();
		status = GetMouseInput();
	}
	else // ? ��A�N�e�B�u -> ������
	{
		MouseRot = 0;
		status = 0;
	}
	m_range(MouseRot, -IMAX, IMAX);

	updateInput(MouseStatus[MOUBTN_L], status & MOUSE_INPUT_LEFT);
	updateInput(MouseStatus[MOUBTN_M], status & MOUSE_INPUT_MIDDLE);
	updateInput(MouseStatus[MOUBTN_R], status & MOUSE_INPUT_RIGHT);
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int GetMouInput(int mouBtnId)
{
	errorCase(mouBtnId < 0 || MOUBTN_MAX <= mouBtnId);

	return FreezeInputFrame ? 0 : MouseStatus[mouBtnId];
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int GetMouPound(int mouBtnId)
{
	return isPound(GetMouInput(mouBtnId));
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int MouseX = SCREEN_CENTER_X;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int MouseY = SCREEN_CENTER_Y;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void UpdateMousePos(void)
{
	errorCase(GetMousePoint(&MouseX, &MouseY)); // ? ���s

	MouseX *= SCREEN_W;
	MouseX /= Gnd.RealScreen_W;
	MouseY *= SCREEN_H;
	MouseY /= Gnd.RealScreen_H;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void ApplyMousePos(void)
{
	int mx = MouseX;
	int my = MouseY;

	mx *= Gnd.RealScreen_W;
	mx /= SCREEN_W;
	my *= Gnd.RealScreen_H;
	my /= SCREEN_H;

	errorCase(SetMousePoint(mx, my)); // ? ���s
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int MouseMoveX;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int MouseMoveY;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void UpdateMouseMove(void)
{
	static int lastFrame = -IMAX;
	static int centerX = SCREEN_CENTER_X;
	static int centerY = SCREEN_CENTER_Y;

	errorCase(ProcFrame <= lastFrame); // ? 2��ȏ�X�V�����B

	UpdateMousePos();

	MouseMoveX = MouseX - centerX;
	MouseMoveY = MouseY - centerY;

	MouseX = centerX;
	MouseY = centerY;

	ApplyMousePos();

	if(lastFrame + 1 < ProcFrame) // ? 1�t���[���ȏ�X�V���Ȃ������B
	{
		MouseMoveX = 0;
		MouseMoveY = 0;
	}
	lastFrame = ProcFrame;
}
