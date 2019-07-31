void InitMap(int w, int h);
int GetMap_W(void);
int GetMap_H(void);
MapCell_t *GetMapCell(i2D_t pt);
MapCell_t *GetMapCell(int x, int y);
MapCell_t *TryGetMapCell(int x, int y, MapCell_t *defaultMapCell = NULL);
