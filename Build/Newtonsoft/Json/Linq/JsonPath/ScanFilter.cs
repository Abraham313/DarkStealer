using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002EB RID: 747
	[NullableContext(2)]
	[Nullable(0)]
	internal class ScanFilter : PathFilter
	{
		// Token: 0x06001795 RID: 6037 RVA: 0x0001721F File Offset: 0x0001541F
		public ScanFilter(string name)
		{
			this.Name = name;
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x0001722E File Offset: 0x0001542E
		[NullableContext(1)]
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			ScanFilter.<ExecuteFilter>d__2 <ExecuteFilter>d__ = new ScanFilter.<ExecuteFilter>d__2(-2);
			<ExecuteFilter>d__.<>4__this = this;
			<ExecuteFilter>d__.<>3__current = current;
			return <ExecuteFilter>d__;
		}

		// Token: 0x04000CB9 RID: 3257
		internal string Name;
	}
}
