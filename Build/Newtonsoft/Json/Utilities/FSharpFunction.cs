using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000217 RID: 535
	[NullableContext(2)]
	[Nullable(0)]
	internal class FSharpFunction
	{
		// Token: 0x06000F6D RID: 3949 RVA: 0x00011B7D File Offset: 0x0000FD7D
		public FSharpFunction(object instance, [Nullable(new byte[]
		{
			1,
			2,
			1
		})] MethodCall<object, object> invoker)
		{
			this._instance = instance;
			this._invoker = invoker;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00011B93 File Offset: 0x0000FD93
		[NullableContext(1)]
		public object Invoke(params object[] args)
		{
			return this._invoker(this._instance, args);
		}

		// Token: 0x040009B1 RID: 2481
		private readonly object _instance;

		// Token: 0x040009B2 RID: 2482
		[Nullable(new byte[]
		{
			1,
			2,
			1
		})]
		private readonly MethodCall<object, object> _invoker;
	}
}
