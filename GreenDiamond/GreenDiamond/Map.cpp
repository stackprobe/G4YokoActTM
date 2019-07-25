#include "all.h"

static oneObject(autoTable<MapCell_t *>, new autoTable<MapCell_t *>(CreateMapCell, ReleaseMapCell), GetTable);

void MapResize(int w, int h)
{
	GetTable()->Resize(w, h);
}
