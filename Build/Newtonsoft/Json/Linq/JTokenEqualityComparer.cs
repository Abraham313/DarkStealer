using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002CC RID: 716
	[NullableContext(1)]
	[Nullable(0)]
	public class JTokenEqualityComparer : IEqualityComparer<JToken>
	{
		// Token: 0x06001690 RID: 5776 RVA: 0x000166A4 File Offset: 0x000148A4
		public bool Equals(JToken x, JToken y)
		{
			return JToken.DeepEquals(x, y);
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x000166AD File Offset: 0x000148AD
		public int GetHashCode(JToken obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetDeepHashCode();
		}
	}
}
