using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002D7 RID: 727
	[NullableContext(1)]
	[Nullable(0)]
	internal class ArrayMultipleIndexFilter : PathFilter
	{
		// Token: 0x06001719 RID: 5913 RVA: 0x00016D4B File Offset: 0x00014F4B
		public ArrayMultipleIndexFilter(List<int> indexes)
		{
			this.Indexes = indexes;
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x00016D5A File Offset: 0x00014F5A
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			ArrayMultipleIndexFilter.<ExecuteFilter>d__2 <ExecuteFilter>d__ = new ArrayMultipleIndexFilter.<ExecuteFilter>d__2(-2);
			<ExecuteFilter>d__.<>4__this = this;
			<ExecuteFilter>d__.<>3__current = current;
			<ExecuteFilter>d__.<>3__errorWhenNoMatch = errorWhenNoMatch;
			return <ExecuteFilter>d__;
		}

		// Token: 0x04000C55 RID: 3157
		internal List<int> Indexes;
	}
}
