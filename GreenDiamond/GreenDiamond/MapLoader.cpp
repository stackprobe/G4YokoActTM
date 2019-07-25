#include "all.h"

#define MAP_DIR "..\\..\\Etcetera\\Map"

void MapLoader(int etcId, char *localFile)
{
	if(IsReleasedVersion())
	{
		GetEtcRes()->GetHandle(etcId);

		// TODO
	}
	else
	{
		// TODO
	}
}
