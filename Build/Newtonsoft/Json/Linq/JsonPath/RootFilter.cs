using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002EA RID: 746
	[NullableContext(1)]
	[Nullable(0)]
	internal class RootFilter : PathFilter
	{
		// Token: 0x06001792 RID: 6034 RVA: 0x00016CDB File Offset: 0x00014EDB
		private RootFilter()
		{
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x00017207 File Offset: 0x00015407
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			return new JToken[]
			{
				root
			};
		}

		// Token: 0x04000CB8 RID: 3256
		public static readonly RootFilter Instance = new RootFilter();
	}
}
