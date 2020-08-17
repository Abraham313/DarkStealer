using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020001F4 RID: 500
	internal interface IWrappedDictionary : IEnumerable, ICollection, IDictionary
	{
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000EB6 RID: 3766
		[Nullable(1)]
		object UnderlyingDictionary { [NullableContext(1)] get; }
	}
}
