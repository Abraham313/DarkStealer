using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000C2 RID: 194
	[ComVisible(true)]
	public class ZipProgressEventArgs : EventArgs
	{
		// Token: 0x06000369 RID: 873 RVA: 0x0000A2E9 File Offset: 0x000084E9
		internal ZipProgressEventArgs()
		{
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000A2F1 File Offset: 0x000084F1
		internal ZipProgressEventArgs(string archiveName, ZipProgressEventType flavor)
		{
			this._archiveName = archiveName;
			this._flavor = flavor;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0000A307 File Offset: 0x00008507
		// (set) Token: 0x0600036C RID: 876 RVA: 0x0000A30F File Offset: 0x0000850F
		public int EntriesTotal
		{
			get
			{
				return this._entriesTotal;
			}
			set
			{
				this._entriesTotal = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0000A318 File Offset: 0x00008518
		// (set) Token: 0x0600036E RID: 878 RVA: 0x0000A320 File Offset: 0x00008520
		public ZipEntry CurrentEntry
		{
			get
			{
				return this._latestEntry;
			}
			set
			{
				this._latestEntry = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000A329 File Offset: 0x00008529
		// (set) Token: 0x06000370 RID: 880 RVA: 0x0000A331 File Offset: 0x00008531
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = (this._cancel || value);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0000A345 File Offset: 0x00008545
		// (set) Token: 0x06000372 RID: 882 RVA: 0x0000A34D File Offset: 0x0000854D
		public ZipProgressEventType EventType
		{
			get
			{
				return this._flavor;
			}
			set
			{
				this._flavor = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000373 RID: 883 RVA: 0x0000A356 File Offset: 0x00008556
		// (set) Token: 0x06000374 RID: 884 RVA: 0x0000A35E File Offset: 0x0000855E
		public string ArchiveName
		{
			get
			{
				return this._archiveName;
			}
			set
			{
				this._archiveName = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000375 RID: 885 RVA: 0x0000A367 File Offset: 0x00008567
		// (set) Token: 0x06000376 RID: 886 RVA: 0x0000A36F File Offset: 0x0000856F
		public long BytesTransferred
		{
			get
			{
				return this._bytesTransferred;
			}
			set
			{
				this._bytesTransferred = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000377 RID: 887 RVA: 0x0000A378 File Offset: 0x00008578
		// (set) Token: 0x06000378 RID: 888 RVA: 0x0000A380 File Offset: 0x00008580
		public long TotalBytesToTransfer
		{
			get
			{
				return this._totalBytesToTransfer;
			}
			set
			{
				this._totalBytesToTransfer = value;
			}
		}

		// Token: 0x04000271 RID: 625
		private int _entriesTotal;

		// Token: 0x04000272 RID: 626
		private bool _cancel;

		// Token: 0x04000273 RID: 627
		private ZipEntry _latestEntry;

		// Token: 0x04000274 RID: 628
		private ZipProgressEventType _flavor;

		// Token: 0x04000275 RID: 629
		private string _archiveName;

		// Token: 0x04000276 RID: 630
		private long _bytesTransferred;

		// Token: 0x04000277 RID: 631
		private long _totalBytesToTransfer;
	}
}
