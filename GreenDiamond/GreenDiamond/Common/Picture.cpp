/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

// Pic_ >

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Pic_FileData2SoftImage(autoList<uchar> *fileData) // fileData: unbind
{
	int hdl = LoadSoftImageToMem(fileData->ElementAt(0), fileData->GetCount());

	errorCase(hdl == -1); // ? ���s

	int w;
	int h;

	errorCase(GetSoftImageSize(hdl, &w, &h)); // ? ���s

	errorCase(w < 1 || IMAX < w);
	errorCase(h < 1 || IMAX < h);

	// RGB -> RGBA
	{
		int h2 = MakeARGB8ColorSoftImage(w, h);

		errorCase(h2 == -1); // ? ���s
		errorCase(BltSoftImage(0, 0, w, h, hdl, 0, 0, h2)); // ? ���s
		errorCase(DeleteSoftImage(hdl) == -1); // ? ���s

		hdl = h2;
	}

	return hdl;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Pic_SoftImage2GraphicHandle(int si_h) // si_h: bind
{
	int h = CreateGraphFromSoftImage(si_h);

	errorCase(h == -1); // ? ���s
	errorCase(DeleteSoftImage(si_h) == -1); // ? ���s

	return h;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
PicInfo_t *Pic_GraphicHandle2PicInfo(int handle) // handle: bind
{
	PicInfo_t *i = nb(PicInfo_t);
	int w;
	int h;

	Pic_GetGraphicHandleSize(handle, w, h);

	i->Handle = handle;
	i->W = w;
	i->H = h;

	return i;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void Pic_ReleasePicInfo(PicInfo_t *i)
{
	errorCase(DeleteGraph(i->Handle)); // ? ���s
	memFree(i);
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void Pic_GetSoftImageSize(int si_h, int &w, int &h)
{
	errorCase(GetSoftImageSize(si_h, &w, &h)); // ? ���s

	errorCase(w < 1 || IMAX < w);
	errorCase(h < 1 || IMAX < h);
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void Pic_GetGraphicHandleSize(int handle, int &w, int &h)
{
	errorCase(GetGraphSize(handle, &w, &h)); // ? ���s

	errorCase(w < 1 || IMAX < w);
	errorCase(h < 1 || IMAX < h);
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SI_R;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SI_G;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SI_B;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int SI_A;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void Pic_GetSIPixel(int si_h, int x, int y)
{
	errorCase(GetPixelSoftImage(si_h, x, y, &SI_R, &SI_G, &SI_B, &SI_A)); // ? ���s

	errorCase(SI_R < 0 || 255 < SI_R);
	errorCase(SI_G < 0 || 255 < SI_G);
	errorCase(SI_B < 0 || 255 < SI_B);
	errorCase(SI_A < 0 || 255 < SI_A);
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void Pic_SetSIPixel(int si_h, int x, int y)
{
	m_range(SI_R, 0, 255);
	m_range(SI_G, 0, 255);
	m_range(SI_B, 0, 255);
	m_range(SI_A, 0, 255);

	errorCase(DrawPixelSoftImage(si_h, x, y, SI_R, SI_G, SI_B, SI_A)); // ? ���s
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Pic_CreateSoftImage(int w, int h)
{
	errorCase(w < 1 || IMAX < w);
	errorCase(h < 1 || IMAX < h);

	int hdl = MakeARGB8ColorSoftImage(w, h);
	errorCase(hdl == -1); // ? ���s
	return hdl;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void Pic_ReleaseSoftImage(int si_h)
{
	errorCase(DeleteSoftImage(si_h) == -1); // ? ���s
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void Pic_ReleaseGraphicHandle(int handle)
{
	errorCase(DeleteGraph(handle)); // ? ���s
}

// < Pic_

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static oneObject(autoList<resCluster<PicInfo_t *> *>, new autoList<resCluster<PicInfo_t *> *>(), GetPicResList);

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
resCluster<PicInfo_t *> *CreatePicRes(PicInfo_t *(*picLoader)(autoList<uchar> *), void (*picUnloader)(PicInfo_t *))
{
	resCluster<PicInfo_t *> *res = new resCluster<PicInfo_t *>("Picture.dat", "..\\..\\Picture.txt", P_MAX, 110000000, picLoader, picUnloader);

	GetPicResList()->AddElement(res);
	return res;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void UnloadAllPicResHandle(void) // �X�N���[�����[�h�؂�ւ����O�ɌĂԂ��ƁB
{
	for(int index = 0; index < GetPicResList()->GetCount(); index++)
	{
		GetPicResList()->GetElement(index)->UnloadAllHandle();
	}
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static resCluster<PicInfo_t *> *CurrPicRes;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void SetPicRes(resCluster<PicInfo_t *> *resclu) // resclu: NULL == reset
{
	CurrPicRes = resclu;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
resCluster<PicInfo_t *> *GetPicRes(void)
{
	if(!CurrPicRes)
		return GetStdPicRes();

	return CurrPicRes;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void ResetPicRes(void)
{
	CurrPicRes = NULL;
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Pic(int picId)
{
	if(picId & DTP)
		return Der(GetPicRes(), picId & ~DTP);

	return GetPicRes()->GetHandle(picId)->Handle;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Pic_W(int picId)
{
	if(picId & DTP)
		return Der_W(picId & ~DTP);

	return GetPicRes()->GetHandle(picId)->W;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Pic_H(int picId)
{
	if(picId & DTP)
		return Der_H(picId & ~DTP);

	return GetPicRes()->GetHandle(picId)->H;
}
