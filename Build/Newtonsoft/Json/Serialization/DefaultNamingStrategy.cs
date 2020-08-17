using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000256 RID: 598
	public class DefaultNamingStrategy : NamingStrategy
	{
		// Token: 0x060010F8 RID: 4344 RVA: 0x000105B2 File Offset: 0x0000E7B2
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return name;
		}
	}
}
