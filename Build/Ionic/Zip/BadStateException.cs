using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x020000CD RID: 205
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00007")]
	[ComVisible(true)]
	[Serializable]
	public class BadStateException : ZipException
	{
		// Token: 0x060003AD RID: 941 RVA: 0x0000A42C File Offset: 0x0000862C
		public BadStateException()
		{
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000A434 File Offset: 0x00008634
		public BadStateException(string message) : base(message)
		{
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000A43D File Offset: 0x0000863D
		public BadStateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000A447 File Offset: 0x00008647
		protected BadStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
