/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

// �ݒ荀�� >

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
/*
	-1 == �f�t�H���g
	0  == �ŏ��̃��j�^
	1  == 2�Ԗڂ̃��j�^
	2  == 3�Ԗڂ̃��j�^
	...
*/
int Conf_DisplayIndex = 1;

// < �ݒ荀��

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void LoadConfig(void)
{
	char *confFile = "Config.conf";

	if(accessible(confFile))
	{
		autoList<uchar> *fileData = readAllBytes(confFile);
		int rIndex = 0;

		// �ݒ荀�� >

		Conf_DisplayIndex = atoi_x(neReadCfgLine(fileData, rIndex));

		// < �ݒ荀��

		{
			char *line = neReadCfgLine(fileData, rIndex);

			errorCase(strcmp(line, "\\e"));
			memFree(line);
		}
	}
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
char *neReadCfgLine(autoList<uchar> *fileData, int &rIndex)
{
	for(; ; )
	{
		char *line = neReadLine(fileData, rIndex);

		if(*line && *line != ';')
			return line;

		memFree(line);
	}
}
