typedef struct MapCell_st
{
	int Wall; // ? ��
	int PicId; // -1 == �摜����
	int EnemyId; // -1 == �G����
	char *EventName; // "" == �C�x���g����

	// <---- access free
}
MapCell_t;

MapCell_t *CreateMapCell(void);
void ReleaseMapCell(MapCell_t *i);

// <-- cdtor

// <-- accessor
