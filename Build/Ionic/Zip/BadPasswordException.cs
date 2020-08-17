using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x020000C9 RID: 201
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000B")]
	[ComVisible(true)]
	[Serializable]
	public class BadPasswordException : ZipException
	{
		// Token: 0x0600039F RID: 927 RVA: 0x0000A42C File Offset: 0x0000862C
		public BadPasswordException()
		{
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000A434 File Offset: 0x00008634
		public BadPasswordException(string message) : base(message)
		{
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000A43D File Offset: 0x0000863D
		public BadPasswordException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000A447 File Offset: 0x00008647
		protected BadPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
