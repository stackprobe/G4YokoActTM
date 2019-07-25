#include "all.h"

void LoadMap(char *file)
{
	autoList<uchar> *fileData = SH_LoadFile(file);

	// TODO

	delete fileData;
}
void SaveMap(char *file)
{
	autoList<uchar> *fileData = new autoList<uchar>();

	// TODO

	SH_SaveFile(file, fileData);
	delete fileData;
}
