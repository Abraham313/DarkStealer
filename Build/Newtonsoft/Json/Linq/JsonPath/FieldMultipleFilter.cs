using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002DD RID: 733
	[NullableContext(1)]
	[Nullable(0)]
	internal class FieldMultipleFilter : PathFilter
	{
		// Token: 0x06001743 RID: 5955 RVA: 0x00016F1D File Offset: 0x0001511D
		public FieldMultipleFilter(List<string> names)
		{
			this.Names = names;
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00016F2C File Offset: 0x0001512C
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			FieldMultipleFilter.<ExecuteFilter>d__2 <ExecuteFilter>d__ = new FieldMultipleFilter.<ExecuteFilter>d__2(-2);
			<ExecuteFilter>d__.<>4__this = this;
			<ExecuteFilter>d__.<>3__current = current;
			<ExecuteFilter>d__.<>3__errorWhenNoMatch = errorWhenNoMatch;
			return <ExecuteFilter>d__;
		}

		// Token: 0x04000C7D RID: 3197
		internal List<string> Names;
	}
}
