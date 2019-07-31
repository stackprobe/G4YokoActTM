typedef struct MapCell_st
{
	int Wall; // ? •Ç
	int PicId; // -1 == ‰æ‘œ–³‚µ
	int EnemyId; // -1 == “G–³‚µ
	char *EventName; // "" == ƒCƒxƒ“ƒg–³‚µ

	// <---- access free
}
MapCell_t;

MapCell_t *CreateMapCell(void);
void ReleaseMapCell(MapCell_t *i);

// <-- cdtor

// <-- accessor
