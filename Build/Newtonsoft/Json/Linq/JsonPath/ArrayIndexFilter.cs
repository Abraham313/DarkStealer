using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002D5 RID: 725
	internal class ArrayIndexFilter : PathFilter
	{
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x00016CAC File Offset: 0x00014EAC
		// (set) Token: 0x0600170C RID: 5900 RVA: 0x00016CB4 File Offset: 0x00014EB4
		public int? Index { get; set; }

		// Token: 0x0600170D RID: 5901 RVA: 0x00016CBD File Offset: 0x00014EBD
		[NullableContext(1)]
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			ArrayIndexFilter.<ExecuteFilter>d__4 <ExecuteFilter>d__ = new ArrayIndexFilter.<ExecuteFilter>d__4(-2);
			<ExecuteFilter>d__.<>4__this = this;
			<ExecuteFilter>d__.<>3__current = current;
			<ExecuteFilter>d__.<>3__errorWhenNoMatch = errorWhenNoMatch;
			return <ExecuteFilter>d__;
		}
	}
}
