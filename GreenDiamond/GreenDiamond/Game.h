typedef struct GDc_st
{
	// Params {
	double Prm_StartX;
	double Prm_StartY;
	// }

	// Return {
	// }

	struct
	{
		double X;
		double Y;
		double YSpeed;
		int FacingLeft;
		int MoveFrame;
		int MoveSlow; // ? í·ë¨à⁄ìÆ
		int JumpFrame;
		int TouchGround;
		int AirborneFrame;
	}
	Player;

	int ICameraX;
	int ICameraY;

	int CamSlideMode; // ? ÉÇÅ[ÉhíÜ
	int CamSlideCount;
	int CamSlideX; // -1 Å` 1
	int CamSlideY; // -1 Å` 1
}
GDc_t;

extern GDc_t GDc;

void GameInit(void);
void GameFnlz(void);
