typedef struct MapCell_st
{
	int Wall; // ? 壁
	int PicId; // -1 == 画像無し
	int EnemyId; // -1 == 敵無し
	char *EventName; // "" == イベント無し

	// <---- access free
}
MapCell_t;

MapCell_t *CreateMapCell(void);
void ReleaseMapCell(MapCell_t *i);

// <-- cdtor

// <-- accessor
