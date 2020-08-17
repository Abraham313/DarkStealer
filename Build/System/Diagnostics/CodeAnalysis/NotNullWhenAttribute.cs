using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x020001A5 RID: 421
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	internal sealed class NotNullWhenAttribute : Attribute
	{
		// Token: 0x06000B12 RID: 2834 RVA: 0x0000E770 File Offset: 0x0000C970
		public NotNullWhenAttribute(bool returnValue)
		{
			this.ReturnValue = returnValue;
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0000E77F File Offset: 0x0000C97F
		public bool ReturnValue { get; }
	}
}
