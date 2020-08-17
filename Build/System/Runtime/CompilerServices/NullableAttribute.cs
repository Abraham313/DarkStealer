using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020001A2 RID: 418
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableAttribute : Attribute
	{
		// Token: 0x06000B0E RID: 2830 RVA: 0x0000E73A File Offset: 0x0000C93A
		public NullableAttribute(byte byte_0)
		{
			this.NullableFlags = new byte[]
			{
				byte_0
			};
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0000E752 File Offset: 0x0000C952
		public NullableAttribute(byte[] byte_0)
		{
			this.NullableFlags = byte_0;
		}

		// Token: 0x0400077C RID: 1916
		public readonly byte[] NullableFlags;
	}
}
