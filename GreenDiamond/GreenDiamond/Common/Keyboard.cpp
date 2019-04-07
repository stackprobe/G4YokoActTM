/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static int KeyStatus[KEY_MAX];

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void KeyEachFrame(void)
{
	if(WindowIsActive)
	{
		static uchar statusMap[KEY_MAX];

		errorCase(GetHitKeyStateAll((char *)statusMap)); // ? ���s

		for(int keyId = 0; keyId < KEY_MAX; keyId++)
		{
			updateInput(KeyStatus[keyId], statusMap[keyId]);
		}
	}
	else // ? ��A�N�e�B�u -> ������
	{
		for(int keyId = 0; keyId < KEY_MAX; keyId++)
		{
			updateInput(KeyStatus[keyId], 0);
		}
	}
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int GetKeyInput(int keyId)
{
	KEY_INPUT_BACK; // keyId, ���̃L�[�� F12 �Ŕ��ł��Ċm�F���ĂˁB

	errorCase(keyId < 0 || KEY_MAX <= keyId);

	return FreezeInputFrame ? 0 : KeyStatus[keyId];
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int GetKeyPound(int keyId)
{
	return isPound(GetKeyInput(keyId));
}
