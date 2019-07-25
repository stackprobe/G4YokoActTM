typedef struct MapCell_st
{
	int Wall; // •Çƒtƒ‰ƒO
	int PicId; // -1 == ‰æ‘œ–³‚µ

	// <---- access free
}
MapCell_t;

MapCell_t *CreateMapCell(void);
void ReleaseMapCell(MapCell_t *i);

// <-- cdtor

// <-- accessor
