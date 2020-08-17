using System;

namespace ChromV1
{
	// Token: 0x02000071 RID: 113
	public class CryptoException : Exception
	{
		// Token: 0x06000274 RID: 628 RVA: 0x00009C87 File Offset: 0x00007E87
		public CryptoException()
		{
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00009C8F File Offset: 0x00007E8F
		public CryptoException(string message) : base(message)
		{
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00009C98 File Offset: 0x00007E98
		public CryptoException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
