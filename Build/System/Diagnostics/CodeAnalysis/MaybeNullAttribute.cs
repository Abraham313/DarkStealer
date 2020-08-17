using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x020001A6 RID: 422
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
	internal sealed class MaybeNullAttribute : Attribute
	{
	}
}
