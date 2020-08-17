using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002ED RID: 749
	[NullableContext(1)]
	[Nullable(0)]
	internal class ScanMultipleFilter : PathFilter
	{
		// Token: 0x060017A0 RID: 6048 RVA: 0x00017290 File Offset: 0x00015490
		public ScanMultipleFilter(List<string> names)
		{
			this._names = names;
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x0001729F File Offset: 0x0001549F
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			ScanMultipleFilter.<ExecuteFilter>d__2 <ExecuteFilter>d__ = new ScanMultipleFilter.<ExecuteFilter>d__2(-2);
			<ExecuteFilter>d__.<>4__this = this;
			<ExecuteFilter>d__.<>3__current = current;
			return <ExecuteFilter>d__;
		}

		// Token: 0x04000CC3 RID: 3267
		private List<string> _names;
	}
}
