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
		int Walking; // ? �����Ă���B
		int Dashing; // ? �����Ă���B
		int TouchGround; // ? �ڒn���Ă���B
	}
	Player;

	int ICameraX;
	int ICameraY;
}
GDc_t;

extern GDc_t GDc;

void GameInit(void);
void GameFnlz(void);
