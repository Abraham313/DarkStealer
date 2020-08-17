using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x020000CA RID: 202
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000A")]
	[ComVisible(true)]
	[Serializable]
	public class BadReadException : ZipException
	{
		// Token: 0x060003A3 RID: 931 RVA: 0x0000A42C File Offset: 0x0000862C
		public BadReadException()
		{
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000A434 File Offset: 0x00008634
		public BadReadException(string message) : base(message)
		{
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000A43D File Offset: 0x0000863D
		public BadReadException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000A447 File Offset: 0x00008647
		protected BadReadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
