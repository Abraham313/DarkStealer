using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000C6 RID: 198
	[ComVisible(true)]
	public class ExtractProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x0600038C RID: 908 RVA: 0x0000A3CC File Offset: 0x000085CC
		internal ExtractProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesExtracted, ZipEntry entry, string extractLocation) : base(archiveName, before ? ZipProgressEventType.Extracting_BeforeExtractEntry : ZipProgressEventType.Extracting_AfterExtractEntry)
		{
			base.EntriesTotal = entriesTotal;
			base.CurrentEntry = entry;
			this._entriesExtracted = entriesExtracted;
			this._target = extractLocation;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000A391 File Offset: 0x00008591
		internal ExtractProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000A389 File Offset: 0x00008589
		internal ExtractProgressEventArgs()
		{
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0002CED4 File Offset: 0x0002B0D4
		internal static ExtractProgressEventArgs BeforeExtractEntry(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_BeforeExtractEntry,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0002CF08 File Offset: 0x0002B108
		internal static ExtractProgressEventArgs ExtractExisting(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0002CF3C File Offset: 0x0002B13C
		internal static ExtractProgressEventArgs AfterExtractEntry(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_AfterExtractEntry,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0002CF70 File Offset: 0x0002B170
		internal static ExtractProgressEventArgs ExtractAllStarted(string archiveName, string extractLocation)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_BeforeExtractAll)
			{
				_target = extractLocation
			};
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0002CF90 File Offset: 0x0002B190
		internal static ExtractProgressEventArgs ExtractAllCompleted(string archiveName, string extractLocation)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_AfterExtractAll)
			{
				_target = extractLocation
			};
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0002CFB0 File Offset: 0x0002B1B0
		internal static ExtractProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesWritten, long totalBytes)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_EntryBytesWritten)
			{
				ArchiveName = archiveName,
				CurrentEntry = entry,
				BytesTransferred = bytesWritten,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0000A3FD File Offset: 0x000085FD
		public int EntriesExtracted
		{
			get
			{
				return this._entriesExtracted;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000A405 File Offset: 0x00008605
		public string ExtractLocation
		{
			get
			{
				return this._target;
			}
		}

		// Token: 0x04000279 RID: 633
		private int _entriesExtracted;

		// Token: 0x0400027A RID: 634
		private string _target;
	}
}
