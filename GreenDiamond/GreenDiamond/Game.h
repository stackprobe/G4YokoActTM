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
		double XAdd;
		double YAdd;
		int Direction; // 4 or 6
		int Walking; // ? 歩いている。
		int Dashing; // ? 走っている。
		int TouchGround; // ? 接地している。
	}
	Player;

	int ICameraX;
	int ICameraY;
}
GDc_t;

extern GDc_t GDc;

void GameInit(void);
void GameFnlz(void);
