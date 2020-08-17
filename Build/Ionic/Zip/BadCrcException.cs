using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x020000CB RID: 203
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00009")]
	[ComVisible(true)]
	[Serializable]
	public class BadCrcException : ZipException
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x0000A42C File Offset: 0x0000862C
		public BadCrcException()
		{
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000A434 File Offset: 0x00008634
		public BadCrcException(string message) : base(message)
		{
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000A447 File Offset: 0x00008647
		protected BadCrcException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
