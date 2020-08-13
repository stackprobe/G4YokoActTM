C:\Factory\Tools\RDMD.exe /RC out

C:\Factory\SubTools\makeDDResourceFile.exe ^
	C:\Dat\Resource ^
	/SD Fairy\Donut3\General ^
	/SD Etoile\G4YokoActTM ^
	out\Resource.dat ^
	C:\Factory\Program\MaskGZDataForDonut3\MaskGZData.exe

C:\Factory\SubTools\CallConfuserCLI.exe G4YokoActTM\G4YokoActTM\bin\Release\G4YokoActTM.exe out\G4YokoActTM.exe
rem COPY /B G4YokoActTM\G4YokoActTM\bin\Release\G4YokoActTM.exe out
COPY /B G4YokoActTM\G4YokoActTM\bin\Release\Chocolate.dll out
COPY /B G4YokoActTM\G4YokoActTM\bin\Release\DxLib.dll out
COPY /B G4YokoActTM\G4YokoActTM\bin\Release\DxLib_x64.dll out
COPY /B G4YokoActTM\G4YokoActTM\bin\Release\DxLibDotNet.dll out

C:\Factory\Tools\xcp.exe doc out
C:\Factory\Tools\xcp.exe C:\Dev\Fairy\Donut3\doc out

C:\Factory\SubTools\zip.exe /PE- /RVE- /G out G4YokoActTM
C:\Factory\Tools\summd5.exe /M out

IF NOT "%1" == "/-P" PAUSE
