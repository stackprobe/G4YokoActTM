/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

// ---- FontFile ----

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static autoList<char *> *FontFileList;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static char *GetFontDir(void)
{
	static char *dir;

	if(!dir)
	{
		dir = makeTempDir();
		createDir(dir);
	}
	return dir;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void RemoveAllFontFile(void)
{
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
LOGPOS(); // test
	while(FontFileList->GetCount())
	{
		errorCase(!RemoveFontResourceEx(FontFileList->UnaddElement(), FR_PRIVATE, NULL)); // ? ���s
	}
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void AddFontFile(int etcId, char *localFile)
{
	char *file = combine(GetFontDir(), localFile);

	{
		autoList<uchar> *fileData = GetEtcRes()->GetHandle(etcId);
		writeAllBytes(file, fileData);
	}

	GetEtcRes()->UnloadAllHandle(); // �e��S�~�J��

	LOG("AddFontResourceEx ST %u\n", (uint)time(NULL));
	errorCase(!AddFontResourceEx(file, FR_PRIVATE, NULL)); // ? ���s
	LOG("AddFontResourceEx ED %u\n", (uint)time(NULL));

	if(!FontFileList)
	{
		FontFileList = new autoList<char *>();
		GetEndProcFinalizers()->AddFunc(ReleaseAllFontHandle);
		GetFinalizers()->AddFunc(RemoveAllFontFile);
	}
	FontFileList->AddElement(file);
}

// ---- FontHandle ----

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
/*
	fontThick: 1 - 9, def == 6   -- ���� DxLib �̃\�[�X�� SetFontThickness() �����ăf�t�H�� 6 �������񂾂Ǝv���B
*/
FontHandle_t *CreateFontHandle(char *fontName, int fontSize, int fontThick, int antiAliasing, int edgeSize, int italicFlag)
{
	errorCase(m_isEmpty(fontName));
	errorCase(!m_isRange(fontSize, 1, IMAX));
	errorCase(!m_isRange(fontThick, 1, 9));
	// antiAliasing
	errorCase(!m_isRange(edgeSize, 0, IMAX));
	// italicFlag

	int h = CreateFontToHandle(
		fontName,
		fontSize,
		fontThick,
		DX_FONTTYPE_NORMAL | (antiAliasing ? DX_FONTTYPE_ANTIALIASING : 0) | (edgeSize ? DX_FONTTYPE_EDGE : 0),
		-1,
		edgeSize
		);

	errorCase(h == -1); // ? ���s

	FontHandle_t *fh = nb(FontHandle_t);
	fh->Handle = h;
	fh->FontName = strx(fontName);
	fh->FontSize = fontSize;
	fh->FontThick = fontThick;
	fh->AntiAliasing = antiAliasing;
	fh->EdgeSize = edgeSize;
	fh->ItalicFlag = italicFlag;
	return fh;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void ReleaseFontHandle(FontHandle_t *fh)
{
	if(!fh)
		return;

	errorCase(DeleteFontToHandle(fh->Handle)); // ? ���s
	memFree(fh->FontName);
	memFree(fh);
}

// <-- cdtor

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static oneObject(autoList<FontHandle_t *>, new autoList<FontHandle_t *>(), GetFontHandleList);

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
FontHandle_t *GetFontHandle(char *fontName, int fontSize, int fontThick, int antiAliasing, int edgeSize, int italicFlag)
{
	errorCase(!fontName);

	FontHandle_t *fh;

	for(int index = 0; index < GetFontHandleList()->GetCount(); index++)
	{
		fh = GetFontHandleList()->GetElement(index);

		if(
			!strcmp(fh->FontName, fontName) &&
			fh->FontSize == fontSize &&
			fh->FontThick == fontThick &&
			(fh->AntiAliasing ? antiAliasing : !antiAliasing) &&
			fh->EdgeSize == edgeSize &&
			(fh->ItalicFlag ? italicFlag : !italicFlag)
			)
		{
			goto found;
		}
	}
	fh = CreateFontHandle(fontName, fontSize, fontThick, antiAliasing, edgeSize, italicFlag);
	GetFontHandleList()->AddElement(fh);
found:
	return fh;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void ReleaseAllFontHandle(void)
{
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
LOGPOS(); // test
	while(GetFontHandleList()->GetCount())
	{
		ReleaseFontHandle(GetFontHandleList()->UnaddElement());
	}
}

// ----

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void DrawStringByFont(int x, int y, char *str, FontHandle_t *fh, int tategakiFlag, int color, int edgeColor)
{
	errorCase(!str);
	errorCase(!fh);

	DrawStringToHandle(x, y, str, color, fh->Handle, edgeColor, tategakiFlag);
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int GetDrawStringByFontWidth(char *str, FontHandle_t *fh, int tategakiFlag)
{
	errorCase(!str);
	errorCase(!fh);

	return GetDrawStringWidthToHandle(str, strlen(str), fh->Handle, tategakiFlag);
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void DrawStringByFont_XCenter(int x, int y, char *str, FontHandle_t *fh, int tategakiFlag, int color, int edgeColor)
{
	x -= GetDrawStringByFontWidth(str, fh, tategakiFlag) / 2;

	DrawStringByFont(x, y, str, fh, tategakiFlag, color, edgeColor);
}
