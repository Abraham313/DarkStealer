using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000262 RID: 610
	[NullableContext(1)]
	public interface ITraceWriter
	{
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06001127 RID: 4391
		TraceLevel LevelFilter { get; }

		// Token: 0x06001128 RID: 4392
		void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex);
	}
}
