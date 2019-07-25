#include "all.h"

MapCell_t *CreateMapCell(void)
{
	MapCell_t *i = nb(MapCell_t);

	i->PicId = P_WALL_00 + 0;

	return i;
}
void ReleaseMapCell(MapCell_t *i)
{
	if(!i)
		return;

	memFree(i);
}

// <-- cdtor

// <-- accessor
