using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000C4 RID: 196
	[ComVisible(true)]
	public class AddProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000380 RID: 896 RVA: 0x0000A389 File Offset: 0x00008589
		internal AddProgressEventArgs()
		{
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000A391 File Offset: 0x00008591
		private AddProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0002CE1C File Offset: 0x0002B01C
		internal static AddProgressEventArgs AfterEntry(string archiveName, ZipEntry entry, int entriesTotal)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_AfterAddEntry)
			{
				EntriesTotal = entriesTotal,
				CurrentEntry = entry
			};
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0002CE40 File Offset: 0x0002B040
		internal static AddProgressEventArgs Started(string archiveName)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Started);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0002CE58 File Offset: 0x0002B058
		internal static AddProgressEventArgs Completed(string archiveName)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Completed);
		}
	}
}
