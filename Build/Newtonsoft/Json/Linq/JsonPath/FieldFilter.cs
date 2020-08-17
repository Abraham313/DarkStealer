using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002DB RID: 731
	[NullableContext(2)]
	[Nullable(0)]
	internal class FieldFilter : PathFilter
	{
		// Token: 0x06001737 RID: 5943 RVA: 0x00016E88 File Offset: 0x00015088
		public FieldFilter(string name)
		{
			this.Name = name;
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00016E97 File Offset: 0x00015097
		[NullableContext(1)]
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			FieldFilter.<ExecuteFilter>d__2 <ExecuteFilter>d__ = new FieldFilter.<ExecuteFilter>d__2(-2);
			<ExecuteFilter>d__.<>4__this = this;
			<ExecuteFilter>d__.<>3__current = current;
			<ExecuteFilter>d__.<>3__errorWhenNoMatch = errorWhenNoMatch;
			return <ExecuteFilter>d__;
		}

		// Token: 0x04000C72 RID: 3186
		internal string Name;
	}
}
