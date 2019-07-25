#include "all.h"

static int ReleasedVersionFlag = -1;

int IsReleasedVersion(void)
{
	if(ReleasedVersionFlag == -1)
		ReleasedVersionFlag = accessible(RELEASE_SIG_FILE) ? 1 : 0;

	return ReleasedVersionFlag;
}
