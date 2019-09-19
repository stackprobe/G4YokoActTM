using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;

namespace Charlotte.Game3Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public class SceneKeeper
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private int FrameMax;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private int StartedProcFrame = -1;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public SceneKeeper(int frameMax)
		{
			if (frameMax < 1 || IntTools.IMAX < frameMax)
				throw new DDError();

			this.FrameMax = frameMax;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Fire()
		{
			this.StartedProcFrame = DDEngine.ProcFrame;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Clear()
		{
			this.StartedProcFrame = -1;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public bool IsJustFired()
		{
			return this.StartedProcFrame == DDEngine.ProcFrame;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public bool IsFlaming()
		{
			return this.StartedProcFrame != -1 && (DDEngine.ProcFrame - this.StartedProcFrame) <= this.FrameMax;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int Count
		{
			get
			{
				return this.StartedProcFrame == -1 ? -1 : DDEngine.ProcFrame - this.StartedProcFrame;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDScene GetScene()
		{
			if (this.StartedProcFrame != -1)
			{
				int count = DDEngine.ProcFrame - this.StartedProcFrame;

				if (count <= this.FrameMax)
				{
					return new DDScene()
					{
						Numer = count,
						Denom = this.FrameMax,
						Rate = count / (double)this.FrameMax,
					};
				}
				this.StartedProcFrame = -1;
			}
			return null;
		}
	}
}
