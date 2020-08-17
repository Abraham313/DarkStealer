using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200025F RID: 607
	[NullableContext(1)]
	public interface IContractResolver
	{
		// Token: 0x06001120 RID: 4384
		JsonContract ResolveContract(Type type);
	}
}
