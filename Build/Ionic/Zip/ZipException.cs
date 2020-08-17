using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x020000C8 RID: 200
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00006")]
	[ComVisible(true)]
	[Serializable]
	public class ZipException : Exception
	{
		// Token: 0x0600039B RID: 923 RVA: 0x00009C87 File Offset: 0x00007E87
		public ZipException()
		{
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00009C8F File Offset: 0x00007E8F
		public ZipException(string message) : base(message)
		{
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00009C98 File Offset: 0x00007E98
		public ZipException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000A422 File Offset: 0x00008622
		protected ZipException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
