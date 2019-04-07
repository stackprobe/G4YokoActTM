/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#include "all.h"

// ---- getFileList ----

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
/*
	unsigned attrib;
		_A_ARCH
		_A_HIDDEN
		_A_NORMAL
		_A_RDONLY
		_A_SUBDIR
		_A_SYSTEM

	time_t time_create;
	time_t time_access;
	time_t time_write;
	_fsize_t size;
	char name[_MAX_PATH];
*/
struct _finddata_t lastFindData;

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
/*
	dir �����̃T�u�f�B���N�g���E�t�@�C���̃��X�g��Ԃ��B
	�Ԃ��T�u�f�B���N�g���E�t�@�C���́u���[�J�����v�Ȃ̂Œ��ӁI

	subDirs: NULL == �o�͂��Ȃ��B
	files: NULL == �o�͂��Ȃ��B
*/
void getFileList(char *dir, autoList<char *> *subDirs, autoList<char *> *files)
{
	errorCase(m_isEmpty(dir));

	char *wCard = xcout("%s\\*", dir);
	intptr_t h = _findfirst(wCard, &lastFindData);
	memFree(wCard);

	if(h != -1)
	{
		do
		{
			char *name = lastFindData.name;

			if(name[0] == '.' && (name[1] == '\0' || name[1] == '.' && name[2] == '\0')) // ".", ".." �����O
				continue;

			errorCase(m_isEmpty(name));
			errorCase(strchr(name, '?')); // ? �ςȕ������܂�

			if(lastFindData.attrib & _A_SUBDIR) // ? subDir
			{
				if(subDirs)
					subDirs->AddElement(strx(name));
			}
			else // ? file
			{
				if(files)
					files->AddElement(strx(name));
			}
		}
		while(_findnext(h, &lastFindData) == 0);

		_findclose(h);
	}
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void updateFindData(char *path)
{
	errorCase(m_isEmpty(path));

	intptr_t h = _findfirst(path, &lastFindData);
	errorCase(h == -1);
	_findclose(h);
}

// ----

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
char *combine(char *path1, char *path2)
{
	char *path;

	if(path1[0] && path1[1] == ':' && path1[2] == '\0')
		path = xcout("%s.\\%s", path1, path2);
	else
		path = xcout("%s\\%s", path1, path2);

	replaceChar(path, '\\', '/');
	path = replacePtnLoop(path, "//", "/");
	replaceChar(path, '/', '\\');

	return path;
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static int Game_mkdir(char *dir) // ret: ? ���s
{
	if(CreateDirectory(dir, NULL) == 0) // ? ���s
	{
		if(accessible(dir))
			return 0;

		execute_x(xcout("MD \"%s\"", dir));

		if(!accessible(dir))
			return 1;
	}
	return 0;
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
int accessible(char *path)
{
	return !_access(path, 0);
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
char *refLocalPath(char *path)
{
	char *p = mbs_strrchr(path, '\\');

	if(p)
		return p + 1;

	return path;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void createDir(char *dir)
{
	errorCase(m_isEmpty(dir));
	errorCase(Game_mkdir(dir)); // ? ���s
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void deleteDir(char *dir)
{
	errorCase(m_isEmpty(dir));

	autoList<char *> *subDirs = new autoList<char *>();
	autoList<char *> *files = new autoList<char *>();

	getFileList(dir, subDirs, files);
	addCwd(dir);

	for(int index = 0; index < subDirs->GetCount(); index++)
		deleteDir(subDirs->GetElement(index));

	for(int index = 0; index < files->GetCount(); index++)
		remove(files->GetElement(index));

	unaddCwd();
	_rmdir(dir);

	releaseList(subDirs, (void (*)(char *))memFree);
	releaseList(files, (void (*)(char *))memFree);
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static oneObject(autoList<char *>, new autoList<char *>(), GetCwdStack);

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
char *getCwd(void)
{
	char *tmp = _getcwd(NULL, 0);

	errorCase(m_isEmpty(tmp));

	char *dir = strx(tmp);
	free(tmp);
	return dir;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void changeCwd(char *dir)
{
	errorCase(m_isEmpty(dir));
	errorCase(_chdir(dir)); // ? ���s
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void addCwd(char *dir)
{
	GetCwdStack()->AddElement(getCwd());
	changeCwd(dir);
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
void unaddCwd(void)
{
	char *dir = GetCwdStack()->UnaddElement();

	changeCwd(dir);
	memFree(dir);
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
#define APP_TEMP_DIR_UUID "{8f8ce7a6-fea3-455c-8d7a-a6ebfd9b9944}" // shared_uuid@g -- ���̃\�[�X���g���񂷂̂ŁAglobal�w��

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static char *GetSystemTempDir(void)
{
	static char *dir;

	if(!dir)
	{
		dir = getenv("TMP");

		if(m_isEmpty(dir))
		{
			dir = getenv("TEMP");
			errorCase(m_isEmpty(dir));
		}
	}
	return dir;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
static void DeleteAppTempDir(void)
{
	deleteDir(getAppTempDir());
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
char *getAppTempDir(void)
{
	static char *dir;

	if(!dir)
	{
		dir = combine(GetSystemTempDir(), APP_TEMP_DIR_UUID);

		if(accessible(dir))
			deleteDir(dir);

		createDir(dir);
		GetFinalizers()->AddFunc(DeleteAppTempDir);
	}
	return dir;
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
__int64 getFileSizeFP(FILE *fp)
{
	errorCase(_fseeki64(fp, 0I64, SEEK_END) != 0); // ? ���s

	__int64 size = _ftelli64(fp);

	errorCase(size < 0I64);
	return size;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
__int64 getFileSizeFPSS(FILE *fp)
{
	__int64 size = getFileSizeFP(fp);

	errorCase(_fseeki64(fp, 0I64, SEEK_SET) != 0); // ? ���s
	return size;
}
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
__int64 getFileSize(char *file)
{
	FILE *fp = fileOpen(file, "rb");

	__int64 size = getFileSizeFP(fp);

	fileClose(fp);
	return size;
}
