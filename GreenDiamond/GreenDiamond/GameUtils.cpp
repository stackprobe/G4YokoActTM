#include "all.h"

i2D_t PositionToMapCellPoint(double x, double y)
{
	int mapTileX = (int)(x / MAP_TILE_WH);
	int mapTileY = (int)(y / MAP_TILE_WH);

	return makeI2D(mapTileX, mapTileY);
}
