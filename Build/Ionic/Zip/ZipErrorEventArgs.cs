using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000C7 RID: 199
	[ComVisible(true)]
	public class ZipErrorEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000397 RID: 919 RVA: 0x0000A389 File Offset: 0x00008589
		private ZipErrorEventArgs()
		{
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0002CFE4 File Offset: 0x0002B1E4
		internal static ZipErrorEventArgs Saving(string archiveName, ZipEntry entry, Exception exception)
		{
			return new ZipErrorEventArgs
			{
				EventType = ZipProgressEventType.Error_Saving,
				ArchiveName = archiveName,
				CurrentEntry = entry,
				_exc = exception
			};
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000A40D File Offset: 0x0000860D
		public Exception Exception
		{
			get
			{
				return this._exc;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000A415 File Offset: 0x00008615
		public string FileName
		{
			get
			{
				return base.CurrentEntry.LocalFileName;
			}
		}

		// Token: 0x0400027B RID: 635
		private Exception _exc;
	}
}
