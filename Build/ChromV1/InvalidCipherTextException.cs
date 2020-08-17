using System;

namespace ChromV1
{
	// Token: 0x0200007A RID: 122
	public class InvalidCipherTextException : CryptoException
	{
		// Token: 0x060002A3 RID: 675 RVA: 0x00009CA2 File Offset: 0x00007EA2
		public InvalidCipherTextException()
		{
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00009CAA File Offset: 0x00007EAA
		public InvalidCipherTextException(string message) : base(message)
		{
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00009CB3 File Offset: 0x00007EB3
		public InvalidCipherTextException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
