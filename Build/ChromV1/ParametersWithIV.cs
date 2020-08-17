using System;
using SmartAssembly.StringsEncoding;

namespace ChromV1
{
	// Token: 0x0200007D RID: 125
	public class ParametersWithIV : ICipherParameters
	{
		// Token: 0x060002AE RID: 686 RVA: 0x00009E9F File Offset: 0x0000809F
		public ParametersWithIV(ICipherParameters parameters, byte[] iv) : this(parameters, iv, 0, iv.Length)
		{
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00026B70 File Offset: 0x00024D70
		public ParametersWithIV(ICipherParameters parameters, byte[] iv, int ivOff, int ivLen)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(Strings.Get(107395330));
			}
			if (iv == null)
			{
				throw new ArgumentNullException(Strings.Get(107395345));
			}
			this.Parameters = parameters;
			this.iv = new byte[ivLen];
			Array.Copy(iv, ivOff, this.iv, 0, ivLen);
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00009EAD File Offset: 0x000080AD
		public ICipherParameters Parameters { get; }

		// Token: 0x060002B1 RID: 689 RVA: 0x00009EB5 File Offset: 0x000080B5
		public byte[] GetIV()
		{
			return (byte[])this.iv.Clone();
		}

		// Token: 0x04000178 RID: 376
		private readonly byte[] iv;
	}
}
