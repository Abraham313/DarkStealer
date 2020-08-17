using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000C5 RID: 197
	[ComVisible(true)]
	public class SaveProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000385 RID: 901 RVA: 0x0000A39B File Offset: 0x0000859B
		internal SaveProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesSaved, ZipEntry entry) : base(archiveName, before ? ZipProgressEventType.Saving_BeforeWriteEntry : ZipProgressEventType.Saving_AfterWriteEntry)
		{
			base.EntriesTotal = entriesTotal;
			base.CurrentEntry = entry;
			this._entriesSaved = entriesSaved;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000A389 File Offset: 0x00008589
		internal SaveProgressEventArgs()
		{
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000A391 File Offset: 0x00008591
		internal SaveProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0002CE70 File Offset: 0x0002B070
		internal static SaveProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_EntryBytesRead)
			{
				ArchiveName = archiveName,
				CurrentEntry = entry,
				BytesTransferred = bytesXferred,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0002CEA4 File Offset: 0x0002B0A4
		internal static SaveProgressEventArgs Started(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Started);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0002CEBC File Offset: 0x0002B0BC
		internal static SaveProgressEventArgs Completed(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Completed);
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000A3C4 File Offset: 0x000085C4
		public int EntriesSaved
		{
			get
			{
				return this._entriesSaved;
			}
		}

		// Token: 0x04000278 RID: 632
		private int _entriesSaved;
	}
}
