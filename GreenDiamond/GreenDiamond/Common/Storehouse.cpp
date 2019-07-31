/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#define CLUSTER_FILE "Storehouse.dat"
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#define STOREHOUSE_DIR "..\\..\\Storehouse"

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static int IsClusterMode(void)
{
	static int ret = -1;

	if(ret == -1)
		ret = accessible(CLUSTER_FILE) ? 1 : 0;

	return ret;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static autoList<uchar> *LoadFileData(autoList<uchar> *fileData)
{
	return fileData->Eject();
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void UnloadFileData(autoList<uchar> *fileData)
{
	delete fileData;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
/*
	file:
		STOREHOUSE_DIR ����̑��΃p�X�ł��邱�ƁB�]�v�� ".", ".." �Ȃǂ��܂܂Ȃ����ƁB

	ret:
		�Ăяo�����ŊJ�����Ȃ���΂Ȃ�Ȃ��B
*/
autoList<uchar> *SH_LoadFile(char *file)
{
	autoList<uchar> *fileData;

	if(IsClusterMode())
	{
		static resCluster<autoList<uchar> *> *res;
		static autoList<char *> *fileList;

		if(!res)
		{
			int resCount;

			{
				FILE *fp = fileOpen(CLUSTER_FILE, "rb");
				resCount = readUI32(fp);
				fileClose(fp);
			}

			char *DUMMY_STRING = "*Dummy";

			res = new resCluster<autoList<uchar> *>(CLUSTER_FILE, DUMMY_STRING, resCount, 150000000, LoadFileData, UnloadFileData);
			fileList = readLines(res->GetHandle(0));

			errorCase(fileList->GetCount() != resCount - 1);
		}
		int index;

		for(index = 0; index < fileList->GetCount(); index++)
			if(!_stricmp(file, fileList->GetElement(index)))
				break;

		errorCase(index == fileList->GetCount());

		fileData = res->GetHandle(index + 1)->Eject();
		res->UnloadAllHandle();
	}
	else
	{
		file = combine(STOREHOUSE_DIR, file);
		fileData = readAllBytes(file);
		memFree(file);
	}
	return fileData;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void SH_SaveFile(char *file, autoList<uchar> *fileData) // file: STOREHOUSE_DIR ����̑��΃p�X�ł��邱�ƁB
{
	if(IsClusterMode())
	{
		error();
	}
	else
	{
		file = combine(STOREHOUSE_DIR, file);
		writeAllBytes(file, fileData);
		memFree(file);
	}
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SH_IsClusterMode(void)
{
	return IsClusterMode();
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SH_ExistFile(char *file)
{
	errorCase(IsClusterMode()); // �����[�X���͎g�p�s�I

	file = combine(STOREHOUSE_DIR, file);
	int ret = accessible(file);
	memFree(file);
	return ret;
}
