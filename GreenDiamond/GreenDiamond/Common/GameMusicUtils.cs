﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class GameMusicUtils
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static List<GameMusic> Musics = new List<GameMusic>();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Add(GameMusic music)
		{
			Musics.Add(music);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private class PlayInfo
		{
			public enum Command_e
			{
				PLAY = 1,
				VOLUME_RATE,
				STOP,
			}

			public Command_e Command;
			public GameMusic Music;
			public bool Once;
			public bool Resume;
			public double VolumeRate;

			public PlayInfo(Command_e command, GameMusic music, bool once, bool resume, double volumeRate)
			{
				this.Command = command;
				this.Music = music;
				this.Once = once;
				this.Resume = resume;
				this.VolumeRate = volumeRate;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static Queue<PlayInfo> PlayInfos = new Queue<PlayInfo>();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void EachFrame()
		{
			if (1 <= PlayInfos.Count)
			{
				PlayInfo info = PlayInfos.Dequeue();

				if (info != null)
				{
					switch (info.Command)
					{
						case PlayInfo.Command_e.PLAY:
							GameSoundUtils.Play(info.Music.Sound.GetHandle(0), info.Once, info.Resume);
							break;

						case PlayInfo.Command_e.VOLUME_RATE:
							GameSoundUtils.SetVolume(info.Music.Sound.GetHandle(0), GameSoundUtils.MixVolume(GameGround.MusicVolume, info.Music.Volume) * info.VolumeRate);
							break;

						case PlayInfo.Command_e.STOP:
							GameSoundUtils.Stop(info.Music.Sound.GetHandle(0));
							break;

						default:
							throw new GameError();
					}
				}
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static GameMusic CurrDestMusic = null;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static double CurrDestVolume = 0.0;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Play(GameMusic music, bool once = false, bool resume = false, double volume = 1.0, int fadeFrameMax = 30)
		{
			if (CurrDestMusic != null) // ? 再生中
			{
				if (CurrDestMusic == music)
					return;

				if (1 <= fadeFrameMax)
					Fade(fadeFrameMax, 0.0, CurrDestVolume);
				else
					Stop();
			}
			PlayInfos.Enqueue(new PlayInfo(PlayInfo.Command_e.PLAY, music, once, resume, 0.0));
			PlayInfos.Enqueue(null);
			PlayInfos.Enqueue(new PlayInfo(PlayInfo.Command_e.VOLUME_RATE, music, false, false, volume));
			PlayInfos.Enqueue(null);
			PlayInfos.Enqueue(null);
			PlayInfos.Enqueue(null);

			CurrDestMusic = music;
			CurrDestVolume = volume;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Fade(int frameMax = 30, double destVolume = 0.0)
		{
			Fade(frameMax, destVolume, CurrDestVolume);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Fade(int frameMax, double destVolumeRate, double startVolumeRate)
		{
			if (CurrDestMusic == null)
				return;

			for (int frmcnt = 0; frmcnt <= frameMax; frmcnt++)
			{
				double volumeRate;

				if (frmcnt == 0)
					volumeRate = startVolumeRate;
				else if (frmcnt == frameMax)
					volumeRate = destVolumeRate;
				else
					volumeRate = startVolumeRate + ((destVolumeRate - startVolumeRate) * frmcnt) / frameMax;

				PlayInfos.Enqueue(new PlayInfo(PlayInfo.Command_e.VOLUME_RATE, CurrDestMusic, false, false, volumeRate));
			}
			CurrDestVolume = destVolumeRate;

			if (destVolumeRate == 0.0) // ? フェード目標音量ゼロ -> 曲停止
			{
				Stop();
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Stop()
		{
			if (CurrDestMusic == null)
				return;

			PlayInfos.Enqueue(new PlayInfo(PlayInfo.Command_e.VOLUME_RATE, CurrDestMusic, false, false, 0.0));
			PlayInfos.Enqueue(null);
			PlayInfos.Enqueue(null);
			PlayInfos.Enqueue(null);
			PlayInfos.Enqueue(new PlayInfo(PlayInfo.Command_e.STOP, CurrDestMusic, false, false, 0.0));
			PlayInfos.Enqueue(null);
			PlayInfos.Enqueue(null);
			PlayInfos.Enqueue(null);

			CurrDestMusic = null;
			CurrDestVolume = 0.0;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void UpdateVolume()
		{
			Fade(0, 1.0);
		}
	}
}
