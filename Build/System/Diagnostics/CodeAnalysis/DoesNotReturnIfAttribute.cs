using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x020001A8 RID: 424
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal class DoesNotReturnIfAttribute : Attribute
	{
		// Token: 0x06000B16 RID: 2838 RVA: 0x0000E787 File Offset: 0x0000C987
		public DoesNotReturnIfAttribute(bool parameterValue)
		{
			this.ParameterValue = parameterValue;
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0000E796 File Offset: 0x0000C996
		public bool ParameterValue { get; }
	}
}
