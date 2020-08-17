using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002B2 RID: 690
	[NullableContext(1)]
	public interface IJEnumerable<[Nullable(0)] out T> : IEnumerable, IEnumerable<T> where T : JToken
	{
		// Token: 0x17000422 RID: 1058
		IJEnumerable<JToken> this[object key]
		{
			get;
		}
	}
}
