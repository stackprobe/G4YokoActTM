typedef struct MapCell_st
{
	int Wall; // �ǃt���O
	int PicId;

	// <---- access free
}
MapCell_t;

MapCell_t *CreateMapCell(void);
void ReleaseMapCell(MapCell_t *i);

// <-- cdtor

// <-- accessor
