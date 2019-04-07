/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
typedef struct CommonEffectExtra_st
{
	int EndPicId; // PicId ���� EndPicId �܂ŕ\�����ďI������, 0 == ����, PicIdFrmPerInc�ƘA��
	int ModPicId; // PicId ���� (PicId + ModPicId - 1) �܂Ń��[�v����, 0 == ����, PicIdFrmPerInc�ƘA��
	int PicIdFrmPerInc;
	double SlideX;
	double SlideY;
	double SlideX_B;
	double SlideY_B;
	double R_B;
	double Z_B;
	double SlideX_F;
	double SlideY_F;
	double R_F;
	double Z_F;
	double SpeedRate;
	int IgnoreCamera;
	int BlendAddOn;

	// wrapped by CEE_* -->

	int BrightOn;
	double Bright_R;
	double Bright_G;
	double Bright_B;
	int GrphHdlFlag;
	i2D_t GrphSize;
}
CommonEffectExtra_t;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
extern CommonEffectExtra_t CEE;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
extern double CameraX;
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
extern double CameraY;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void AddCommonEffect(
	taskList *tl,
	int topMode,
	int picId,
	double x, double y, double r = 0.0, double z = 1.0, double a = 1.0,
	double x_add = 0.0, double y_add = 0.0, double r_add = 0.0, double z_add = 0.0, double a_add = 0.0,
	double x_add2 = 0.0, double y_add2 = 0.0, double r_add2 = 0.0, double z_add2 = 0.0, double a_add2 = 0.0
	);

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void CEE_SetBright(double r, double g, double b);
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void CEE_SetGraphicSize(i2D_t size);
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void CEE_Reset(void);

// ---- tools ----

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int IsInCamera(double x, double y, double margin);

// ----
