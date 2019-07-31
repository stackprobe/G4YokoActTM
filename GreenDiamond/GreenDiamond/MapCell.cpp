#include "all.h"

MapCell_t *CreateMapCell(void)
{
	MapCell_t *i = nb(MapCell_t);

//	i->Wall = 0;
	i->PicId = -1;
	i->EnemyId = -1;
	i->EventName = strx("");

	return i;
}
void ReleaseMapCell(MapCell_t *i)
{
	if(!i)
		return;

	memFree(i->EventName);
	memFree(i);
}

// <-- cdtor

// <-- accessor
