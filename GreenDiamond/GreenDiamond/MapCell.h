typedef struct MapCell_st
{
	int Wall; // �ǃt���O
	int PicId; // -1 == �摜����

	// <---- access free
}
MapCell_t;

MapCell_t *CreateMapCell(void);
void ReleaseMapCell(MapCell_t *i);

// <-- cdtor

// <-- accessor
