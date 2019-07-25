#include "all.h"

void ML_InitMap(int w, int h)
{
	InitMap(w, h);

	for(int x = 0; x < w; x++)
	{
		MapCell_t *cell = GetMapCell(x, h - 1);

		cell->Wall = 1;
		cell->PicId = P_MAP_TILE_00 + 1;
	}
}
void ML_LoadMap(char *file)
{
	autoList<uchar> *fileData = SH_LoadFile(file);
	int rIndex = 0;
	int w = atoi_x(readLine(fileData, rIndex));
	int h = atoi_x(readLine(fileData, rIndex));

	errorCase(!m_isRange(w, 1, IMAX));
	errorCase(!m_isRange(h, 1, IMAX));

	for(int x = 0; x < w; x++)
	for(int y = 0; y < h; y++)
	{
		MapCell_t *cell = GetMapCell(x, y);
		char *line = nnReadLine(fileData, rIndex);
		autoList<char *> *tokens = tokenize(line, ":");
		int c = 0;

		// 古い(項目が少ない)データを読み込んでも良いように RefElement を使用する。

		cell->Wall  = atoi(tokens->RefElement(c++, "0"));
		cell->PicId = atoi(tokens->RefElement(c++, "-1"));

		// 新しい項目は、ここへ追加...

		memFree(line);
		releaseList(tokens, (void (*)(char *))memFree);
	}
	delete fileData;
}
void ML_SaveMap(char *file)
{
	autoList<uchar> *fileData = new autoList<uchar>();
	int w = GetMap_W();
	int h = GetMap_H();

	writeLine_x(fileData, xcout("%d", w));
	writeLine_x(fileData, xcout("%d", h));

	for(int x = 0; x < w; x++)
	for(int y = 0; y < h; y++)
	{
		MapCell_t *cell = GetMapCell(x, y);
		autoList<char *> *tokens = new autoList<char *>();

		tokens->AddElement(xcout("%d", cell->Wall));
		tokens->AddElement(xcout("%d", cell->PicId));

		// 新しい項目は、ここへ追加...

		writeLine_x(fileData, untokenize(tokens, ":"));

		releaseList(tokens, (void (*)(char *))memFree);
	}
	SH_SaveFile(file, fileData);
	delete fileData;
}
