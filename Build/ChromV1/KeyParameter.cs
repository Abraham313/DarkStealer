using System;
using SmartAssembly.StringsEncoding;

namespace ChromV1
{
	// Token: 0x0200007B RID: 123
	public class KeyParameter : ICipherParameters
	{
		// Token: 0x060002A6 RID: 678 RVA: 0x00009DB7 File Offset: 0x00007FB7
		public KeyParameter(byte[] key)
		{
			if (key == null)
			{
				throw new ArgumentNullException(Strings.Get(107395385));
			}
			this.key = (byte[])key.Clone();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00026AF8 File Offset: 0x00024CF8
		public KeyParameter(byte[] key, int keyOff, int keyLen)
		{
			if (key == null)
			{
				throw new ArgumentNullException(Strings.Get(107395385));
			}
			if (keyOff < 0 || keyOff > key.Length)
			{
				throw new ArgumentOutOfRangeException(Strings.Get(107395380));
			}
			if (keyLen < 0 || keyOff + keyLen > key.Length)
			{
				throw new ArgumentOutOfRangeException(Strings.Get(107395339));
			}
			this.key = new byte[keyLen];
			Array.Copy(key, keyOff, this.key, 0, keyLen);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00009DE3 File Offset: 0x00007FE3
		public byte[] GetKey()
		{
			return (byte[])this.key.Clone();
		}

		// Token: 0x04000177 RID: 375
		private readonly byte[] key;
	}
}
