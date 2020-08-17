using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020001E9 RID: 489
	internal interface IWrappedCollection : IEnumerable, IList, ICollection
	{
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000E57 RID: 3671
		[Nullable(1)]
		object UnderlyingCollection { [NullableContext(1)] get; }
	}
}
