using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000261 RID: 609
	[NullableContext(1)]
	public interface ISerializationBinder
	{
		// Token: 0x06001125 RID: 4389
		Type BindToType([Nullable(2)] string assemblyName, string typeName);

		// Token: 0x06001126 RID: 4390
		[NullableContext(2)]
		void BindToName([Nullable(1)] Type serializedType, out string assemblyName, out string typeName);
	}
}
