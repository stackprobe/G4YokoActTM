#include "all.h"

static oneObject(autoTable<MapCell_t *>, new autoTable<MapCell_t *>(CreateMapCell, ReleaseMapCell), GetTable);
static oneObject(MapCell_t, CreateMapCell(), GetDefaultMapCell);

void InitMap(int w, int h)
{
	GetTable()->Resize(w, h);
	GetTable()->Reset();
}
int GetMap_W(void)
{
	return GetTable()->GetWidth();
}
int GetMap_H(void)
{
	return GetTable()->GetHeight();
}
MapCell_t *GetMapCell(int x, int y)
{
//	return GetTable()->GetCell(x, y);
	return GetTable()->RefCell(x, y, GetDefaultMapCell());
}
