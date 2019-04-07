/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int RC_ResId;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static int Res_ReadInt(FILE *fp)
{
	int c1 = fgetc(fp);
	int c2 = fgetc(fp);
	int c3 = fgetc(fp);
	int c4 = fgetc(fp);

	errorCase(c4 == EOF);
/*	errorCase(
		c1 == EOF ||
		c2 == EOF ||
		c3 == EOF ||
		c4 == EOF
		); */

	int value =
		c1 << 8 * 0 |
		c2 << 8 * 1 |
		c3 << 8 * 2 |
		c4 << 8 * 3;

	errorCase(!m_isRange(value, 0, IMAX));
	return value;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void Res_FSeek(FILE *fp, __int64 pos)
{
	errorCase(_fseeki64(fp, pos, SEEK_SET) != 0); // ? ���s
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void *Res_FRead(FILE *fp, int size)
{
	void *buff = memAlloc(size);
	errorCase(fread(buff, 1, size, fp) != size); // ? ���s
	return buff;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
autoList<uchar> *LoadFileDataFromCluster(char *clusterFile, int resId, int tweek)
{
	errorCase(m_isEmpty(clusterFile));
	errorCase(!m_isRange(resId, 0, IMAX));
	errorCase(!m_isRange(tweek, 0, IMAX));

	FILE *fp = fileOpen(clusterFile, "rb");

	int num = Res_ReadInt(fp);
	errorCase(num <= resId);
	__int64 pos = (num + 1) * 4;

	for(int count = 0; count < resId; count++)
	{
		pos += Res_ReadInt(fp);
	}
	int size = Res_ReadInt(fp);

	Res_FSeek(fp, pos);
	void *fileData = Res_FRead(fp, size);

	fileClose(fp);

	errorCase(!aes128_decrypt_extend(fileData, size, tweek));
	return new autoList<uchar>((uchar *)fileData, size);
}
