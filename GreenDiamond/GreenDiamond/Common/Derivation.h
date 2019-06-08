/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
enum
{
	ENUM_RANGE(D_DUMMY_00, 4)

	// app > @ D_

	ENUM_RANGE(D_PLAYER_STAND_1_00, 2)
	ENUM_RANGE(D_PLAYER_STAND_2_00, 2)
	ENUM_RANGE(D_PLAYER_STAND_3_00, 2)
	ENUM_RANGE(D_PLAYER_STAND_4_00, 2)
	ENUM_RANGE(D_PLAYER_STAND_5_00, 2)

	ENUM_RANGE(D_PLAYER_TAKL_1_00, 2)
	ENUM_RANGE(D_PLAYER_TAKL_2_00, 2)
	ENUM_RANGE(D_PLAYER_TAKL_3_00, 2)
	ENUM_RANGE(D_PLAYER_TAKL_4_00, 2)
	ENUM_RANGE(D_PLAYER_TAKL_5_00, 2)

	D_PLAYER_SHAGAMI,

	ENUM_RANGE(D_PLAYER_WALK_00, 2)
	ENUM_RANGE(D_PLAYER_DASH_00, 2)
	ENUM_RANGE(D_PLAYER_STOP_00, 2)

	D_PLAYER_JUMP_1,
	D_PLAYER_JUMP_2,
	D_PLAYER_JUMP_3,

	D_PLAYER_ATTACK,
	D_PLAYER_ATTACK_SHAGAMI,
	ENUM_RANGE(D_PLAYER_ATTACK_WALK_00, 2)
	ENUM_RANGE(D_PLAYER_ATTACK_DASH_00, 2)
	D_PLAYER_ATTACK_JUMP,

	ENUM_RANGE(D_PLAYER_DAMAGE_00, 8)

	// < app

	D_MAX, // num of member
};

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
typedef struct DerInfo_st
{
	int ParentPicId;
	int X;
	int Y;
	int W;
	int H;
}
DerInfo_t;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Der(int derId);
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Der(int derId, resCluster<PicInfo_t *> *resclu);
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Der_W(int derId);
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int Der_H(int derId);

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void UnloadAllDer(autoList<int> *derHandleList);
