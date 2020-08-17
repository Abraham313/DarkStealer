using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000260 RID: 608
	[NullableContext(1)]
	public interface IReferenceResolver
	{
		// Token: 0x06001121 RID: 4385
		object ResolveReference(object context, string reference);

		// Token: 0x06001122 RID: 4386
		string GetReference(object context, object value);

		// Token: 0x06001123 RID: 4387
		bool IsReferenced(object context, object value);

		// Token: 0x06001124 RID: 4388
		void AddReference(object context, string reference, object value);
	}
}
