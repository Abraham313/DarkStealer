using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200025C RID: 604
	[NullableContext(1)]
	[Nullable(0)]
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06001118 RID: 4376 RVA: 0x00012BB2 File Offset: 0x00010DB2
		[Nullable(2)]
		public object CurrentObject { [NullableContext(2)] get; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x00012BBA File Offset: 0x00010DBA
		public ErrorContext ErrorContext { get; }

		// Token: 0x0600111A RID: 4378 RVA: 0x00012BC2 File Offset: 0x00010DC2
		public ErrorEventArgs([Nullable(2)] object currentObject, ErrorContext errorContext)
		{
			this.CurrentObject = currentObject;
			this.ErrorContext = errorContext;
		}
	}
}
