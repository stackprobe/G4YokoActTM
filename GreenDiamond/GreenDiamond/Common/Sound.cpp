/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int LoadSound(autoList<uchar> *fileData)
{
	int h = LoadSoundMemByMemImage(fileData->ElementAt(0), fileData->GetCount());

	errorCase(h == -1); // ? ���s
	return h;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int DuplSound(int h)
{
	int dupl_h = DuplicateSoundMem(h);

	errorCase(dupl_h == -1); // ? ���s
	return dupl_h;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void UnloadSound(int h)
{
	errorCase(DeleteSoundMem(h)); // ? ���s
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void SoundPlay(int h, int once_mode, int resume_mode)
{
	switch(CheckSoundMem(h))
	{
	case 1: // ? �Đ���
		return;

	case 0: // ? �Đ����Ă��Ȃ��B
		break;

	case -1: // ? �G���[
		error();

	default: // ? �s��
		error();
	}
	errorCase(PlaySoundMem(h, once_mode ? DX_PLAYTYPE_BACK : DX_PLAYTYPE_LOOP, resume_mode ? 0 : 1)); // ? ���s
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void SetVolume(int h, double volume)
{
	m_range(volume, 0.0, 1.0);

	int pal = d2i(volume * 255.0);

	errorCase(pal < 0 || 255 < pal);
	errorCase(ChangeVolumeSoundMem(pal, h)); // ? ���s
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void SoundStop(int h)
{
	errorCase(StopSoundMem(h)); // ? ���s
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
double MixVolume(double volume1, double volume2) // (volume1, volume2): 0.0 - 1.0, ret: 0.0 - 1.0
{
	m_range(volume1, 0.0, 1.0);
	m_range(volume2, 0.0, 1.0);

	double mixedVolume = volume1 * volume2 * 2.0; // 0.0 - 2.0

	m_range(mixedVolume, 0.0, 1.0);

	return mixedVolume;
}
