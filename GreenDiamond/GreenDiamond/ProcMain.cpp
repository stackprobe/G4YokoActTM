#include "all.h"

void ProcMain(void)
{
#if !LOG_ENABLED
	TitleMenu();
#elif 0 // test
	TitleMenu();
#elif 0 // test
	GameInit();
	GameMain();
	GameFnlz();
#else // test
	Test0001();
#endif
}
