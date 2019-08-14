﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Charlotte.Tools;
using DxLibDLL;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class GameMain
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static List<Action> Finalizers = new List<Action>();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static int LogCount = 0;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void GameStart()
		{
			GameConfig.Load(); // LogFile, LOG_ENABLED 等を含むので真っ先に

			// Log >

			File.WriteAllBytes(GameConfig.LogFile, BinTools.EMPTY);

			ProcMain.WriteLog = message =>
			{
				if (LogCount < GameConfig.LogCountMax)
				{
					using (StreamWriter writer = new StreamWriter(GameConfig.LogFile, true, Encoding.UTF8))
					{
						writer.WriteLine("[" + DateTime.Now + "] " + message);
					}
					LogCount++;
				}
			};

			// < Log

			// *.INIT
			{
				GameGround.INIT();
				GameResource.INIT();
				GameDatStrings.INIT();
				GameUserDatStrings.INIT();
				GameFontRegister.INIT();
			}

			GameSaveData.Load();

			// DxLib >

			if (GameConfig.LOG_ENABLED)
				DX.SetApplicationLogSaveDirectory(GameConfig.ApplicationLogSaveDirectory);

			DX.SetOutApplicationLogValidFlag(GameConfig.LOG_ENABLED ? 1 : 0); // DxLib のログを出力 1: する 0: しない

			DX.SetAlwaysRunFlag(1); // ? 非アクティブ時に 1: 動く 0: 止まる

			SetMainWindowTitle();

			//DX.SetGraphMode(GameConsts.Screen_W, GameConsts.Screen_H, 32);
			DX.SetGraphMode(GameGround.RealScreen_W, GameGround.RealScreen_H, 32);
			DX.ChangeWindowMode(1); // 1: ウィンドウ 0: フルスクリーン

			//DX.SetFullSceneAntiAliasingMode(4, 2); // 適当な値が分からん。フルスクリーン廃止したので不要

			DX.SetWindowIconHandle(GetAppIcon()); // ウィンドウ左上のアイコン

			if (GameConfig.DisplayIndex != -1)
				DX.SetUseDirectDrawDeviceIndex(GameConfig.DisplayIndex);

			if (DX.DxLib_Init() != 0) // ? 失敗
				throw new GameError();

			Finalizers.Add(() =>
			{
				if (DX.DxLib_End() != 0) // ? 失敗
					throw new GameError();
			});

			GameDxUtils.SetMouseDispMode(GameGround.RO_MouseDispMode); // ? マウスを表示する。
			DX.SetWindowSizeChangeEnableFlag(0); // ウィンドウの右下をドラッグで伸縮 1: する 0: しない

			DX.SetDrawScreen(DX.DX_SCREEN_BACK);
			DX.SetDrawMode(DX.DX_DRAWMODE_BILINEAR); // これをデフォルトとする。

			// < DxLib

			{
				int l;
				int t;
				int w;
				int h;
				int p1;
				int p2;
				int p3;
				int p4;

				DX.GetDefaultState(out w, out h, out p1, out p2, out l, out t, out p3, out p4);

				if (
					w < 1 || IntTools.IMAX < w ||
					h < 1 || IntTools.IMAX < h ||
					l < -IntTools.IMAX || IntTools.IMAX < l ||
					t < -IntTools.IMAX || IntTools.IMAX < t
					)
					throw new GameError();

				GameGround.MonitorRect = new I4Rect(l, t, w, h);
			}

			PostSetScreenSize(GameGround.RealScreen_W, GameGround.RealScreen_H);

			GameGround.GeneralResource = new GameGeneralResource();

			GameAdditionalEvents.PostGameStart();
			GameAdditionalEvents.PostGameStart_G2();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void GameEnd()
		{
			GameSaveData.Save();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void GameEnd2(ExceptionDam eDam)
		{
			while (1 <= Finalizers.Count)
			{
				eDam.Invoke(ExtraTools.UnaddElement(Finalizers));
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetMainWindowTitle()
		{
			DX.SetMainWindowText(GameDatStrings.Title + " " + GameUserDatStrings.Version);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static IntPtr GetAppIcon()
		{
			using (MemoryStream mem = new MemoryStream(GameResource.Load(@"General\game_app.ico")))
			{
				return new Icon(mem).Handle;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetScreenSize(int w, int h)
		{
			if (
				w < GameConsts.Screen_W_Min || GameConsts.Screen_W_Max < w ||
				h < GameConsts.Screen_H_Min || GameConsts.Screen_H_Max < h
				)
				throw new GameError();

			GameGround.RealScreenDraw_W = -1; // 無効化

			if (GameGround.RealScreen_W != w || GameGround.RealScreen_H != h)
			{
				GameGround.RealScreen_W = w;
				GameGround.RealScreen_H = h;

				ApplyScreenSize();

				PostSetScreenSize(w, h);
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void ApplyScreenSize()
		{
			ApplyScreenSize(GameGround.RealScreen_W, GameGround.RealScreen_H);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void ApplyScreenSize(int w, int h)
		{
			bool mdm = GameDxUtils.GetMouseDispMode();

			//GameDerivationUtils.UnloadAll(); // -> GamePictureUtils.UnloadAll
			GamePictureUtils.UnloadAll();
			GameSubScreenUtils.UnloadAll();
			GameFontUtils.UnloadAll();

			if (DX.SetGraphMode(w, h, 32) != DX.DX_CHANGESCREEN_OK)
				throw new GameError();

			DX.SetDrawScreen(DX.DX_SCREEN_BACK);
			DX.SetDrawMode(DX.DX_DRAWMODE_BILINEAR);

			GameDxUtils.SetMouseDispMode(mdm);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void PostSetScreenSize(int w, int h)
		{
			if (GameGround.MonitorRect.W == w && GameGround.MonitorRect.H == h)
			{
				SetScreenPosition(GameGround.MonitorRect.L, GameGround.MonitorRect.T);
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetScreenPosition(int l, int t)
		{
			DX.SetWindowPosition(l, t);

			GameWin32.POINT p;

			p.X = 0;
			p.Y = 0;

			GameWin32.ClientToScreen(GameWin32.GetMainWindowHandle(), out p);

			int pToTrgX = l - (int)p.X;
			int pToTrgY = t - (int)p.Y;

			DX.SetWindowPosition(l + pToTrgX, t + pToTrgY);
		}
	}
}
