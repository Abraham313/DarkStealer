using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x020001A4 RID: 420
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
	internal sealed class NotNullAttribute : Attribute
	{
	}
}
