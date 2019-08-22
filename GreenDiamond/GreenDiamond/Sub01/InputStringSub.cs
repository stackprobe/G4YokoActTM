using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using DxLibDLL;

namespace Charlotte.Sub01
{
	public static class InputStringSub
	{
		public static string InputString(string prompt, string initValue = "", int maxlen = 100)
		{
			StringBuilder buff = new StringBuilder(maxlen * 3); // FIXME 必要なバッファ長が不明

			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			int inputHdl = DX.MakeKeyInput((uint)maxlen, 1, 0, 0); // ハンドル生成

			DX.SetActiveKeyInput(inputHdl); // 入力開始

			DX.SetKeyInputString(initValue, inputHdl); // 初期値設定

			for (; ; )
			{
				if (DX.CheckKeyInput(inputHdl) != 0) // 入力終了
					break;

				DDCurtain.DrawCurtain();

				DDPrint.SetPrint(50, 50);
				DDPrint.Print("InputString > " + prompt);

				DX.DrawKeyInputModeString(50, 100); // IMEモードとか表示

				DX.GetKeyInputString(buff, inputHdl); // 現状の文字列を取得

				DDPrint.SetPrint(50, 150);
				DDPrint.Print(buff.ToString());

				DX.DrawKeyInputString(50, 200, inputHdl); // 入力中の文字列の描画

				DDEngine.EachFrame();
			}
			DX.DeleteKeyInput(inputHdl); // ハンドル開放

			DDEngine.FreezeInput();

			return buff.ToString();
		}
	}
}
