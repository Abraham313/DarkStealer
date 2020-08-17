using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020001A3 RID: 419
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableContextAttribute : Attribute
	{
		// Token: 0x06000B10 RID: 2832 RVA: 0x0000E761 File Offset: 0x0000C961
		public NullableContextAttribute(byte byte_0)
		{
			this.Flag = byte_0;
		}

		// Token: 0x0400077D RID: 1917
		public readonly byte Flag;
	}
}
