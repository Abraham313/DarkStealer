using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002D9 RID: 729
	internal class ArraySliceFilter : PathFilter
	{
		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001725 RID: 5925 RVA: 0x00016DDE File Offset: 0x00014FDE
		// (set) Token: 0x06001726 RID: 5926 RVA: 0x00016DE6 File Offset: 0x00014FE6
		public int? Start { get; set; }

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001727 RID: 5927 RVA: 0x00016DEF File Offset: 0x00014FEF
		// (set) Token: 0x06001728 RID: 5928 RVA: 0x00016DF7 File Offset: 0x00014FF7
		public int? End { get; set; }

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001729 RID: 5929 RVA: 0x00016E00 File Offset: 0x00015000
		// (set) Token: 0x0600172A RID: 5930 RVA: 0x00016E08 File Offset: 0x00015008
		public int? Step { get; set; }

		// Token: 0x0600172B RID: 5931 RVA: 0x00016E11 File Offset: 0x00015011
		[NullableContext(1)]
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			ArraySliceFilter.<ExecuteFilter>d__12 <ExecuteFilter>d__ = new ArraySliceFilter.<ExecuteFilter>d__12(-2);
			<ExecuteFilter>d__.<>4__this = this;
			<ExecuteFilter>d__.<>3__current = current;
			<ExecuteFilter>d__.<>3__errorWhenNoMatch = errorWhenNoMatch;
			return <ExecuteFilter>d__;
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x00016E2F File Offset: 0x0001502F
		private bool IsValid(int index, int stopIndex, bool positiveStep)
		{
			if (positiveStep)
			{
				return index < stopIndex;
			}
			return index > stopIndex;
		}
	}
}
