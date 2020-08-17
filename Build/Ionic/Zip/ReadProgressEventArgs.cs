using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000C3 RID: 195
	[ComVisible(true)]
	public class ReadProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000379 RID: 889 RVA: 0x0000A389 File Offset: 0x00008589
		internal ReadProgressEventArgs()
		{
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000A391 File Offset: 0x00008591
		private ReadProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0002CD7C File Offset: 0x0002AF7C
		internal static ReadProgressEventArgs Before(string archiveName, int entriesTotal)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_BeforeReadEntry)
			{
				EntriesTotal = entriesTotal
			};
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0002CD9C File Offset: 0x0002AF9C
		internal static ReadProgressEventArgs After(string archiveName, ZipEntry entry, int entriesTotal)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_AfterReadEntry)
			{
				EntriesTotal = entriesTotal,
				CurrentEntry = entry
			};
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0002CDC0 File Offset: 0x0002AFC0
		internal static ReadProgressEventArgs Started(string archiveName)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Started);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0002CDD8 File Offset: 0x0002AFD8
		internal static ReadProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_ArchiveBytesRead)
			{
				CurrentEntry = entry,
				BytesTransferred = bytesXferred,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0002CE04 File Offset: 0x0002B004
		internal static ReadProgressEventArgs Completed(string archiveName)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Completed);
		}
	}
}
