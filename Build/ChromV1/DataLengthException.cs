using System;

namespace ChromV1
{
	// Token: 0x02000072 RID: 114
	public class DataLengthException : CryptoException
	{
		// Token: 0x06000277 RID: 631 RVA: 0x00009CA2 File Offset: 0x00007EA2
		public DataLengthException()
		{
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00009CAA File Offset: 0x00007EAA
		public DataLengthException(string message) : base(message)
		{
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00009CB3 File Offset: 0x00007EB3
		public DataLengthException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
