/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static autoList<DerInfo_t *> *DerList;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void AddDer(int parentPicId, int x, int y, int w, int h)
{
	DerInfo_t *i = nb(DerInfo_t);

	i->ParentPicId = parentPicId;
	i->X = x;
	i->Y = y;
	i->W = w;
	i->H = h;

	DerList->AddElement(i);
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void AddAnimation(int parentPicId, int x, int y, int w, int h, int xNum, int yNum, int xStep = -1, int yStep = -1)
{
	if(xStep == -1) xStep = w;
	if(yStep == -1) yStep = h;

	for(int yc = 0; yc < yNum; yc++)
	for(int xc = 0; xc < xNum; xc++)
	{
		AddDer(parentPicId, x + xc * xStep, y + yc * yStep, w, h);
	}
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void CreateDerList(void)
{
	DerList = new autoList<DerInfo_t *>();

	AddAnimation(P_DUMMY, 0, 0, 25, 25, 2, 2); // D_DUMMY_00

	// app > @ AddDer

	// < app

	errorCase(DerList->GetCount() != D_MAX);
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static autoList<DerInfo_t *> *GetDerList(void)
{
	if(!DerList)
		CreateDerList();

	return DerList;
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static int LoadDer(PicInfo_t *parent, int x, int y, int w, int h)
{
	int der_h;

	// check rect {
	errorCase(x < 0 || IMAX < x);
	errorCase(y < 0 || IMAX < y);
	errorCase(w < 1 || IMAX - x < w);
	errorCase(h < 1 || IMAX - y < h);
	// }

	if(
		parent->W < x + w ||
		parent->H < y + h
		)
		// ? �͈͊O
	{
		m_range(w, 1, 50);
		m_range(h, 1, 50);

		der_h = DerivationGraph(0, 0, w, h, GetPicRes()->GetHandle(P_DUMMY)->Handle); // �_�~�[�摜
	}
	else
		der_h = DerivationGraph(x, y, w, h, parent->Handle);

	errorCase(der_h == -1); // ? ���s
	return der_h;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void UnloadDer(int h)
{
	if(h == -1) // ? ���I�[�v��
		return;

	errorCase(DeleteGraph(h)); // ? ���s
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Der(int derId)
{
	return Der(derId, GetPicRes());
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Der(int derId, resCluster<PicInfo_t *> *resclu)
{
	errorCase(derId < 0 || D_MAX <= derId);

	int h = resclu->GetDerHandleList()->RefElement(derId, -1);

	if(h == -1)
	{
		DerInfo_t *i = GetDerList()->GetElement(derId);

		h = LoadDer(
			resclu->GetHandle(i->ParentPicId),
			i->X,
			i->Y,
			i->W,
			i->H
			);

		resclu->GetDerHandleList()->PutElement(derId, h, -1);
	}
	return h;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Der_W(int derId)
{
	return GetDerList()->GetElement(derId)->W;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Der_H(int derId)
{
	return GetDerList()->GetElement(derId)->H;
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void UnloadAllDer(autoList<int> *derHandleList)
{
	for(int derId = 0; derId < derHandleList->GetCount(); derId++)
	{
		UnloadDer(derHandleList->GetElement(derId));
	}
	derHandleList->Clear();
}
