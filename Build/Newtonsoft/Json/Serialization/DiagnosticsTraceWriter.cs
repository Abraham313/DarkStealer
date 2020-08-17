using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000259 RID: 601
	public class DiagnosticsTraceWriter : ITraceWriter
	{
		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x00012AF3 File Offset: 0x00010CF3
		// (set) Token: 0x06001108 RID: 4360 RVA: 0x00012AFB File Offset: 0x00010CFB
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x06001109 RID: 4361 RVA: 0x00012B04 File Offset: 0x00010D04
		private TraceEventType GetTraceEventType(TraceLevel level)
		{
			switch (level)
			{
			case TraceLevel.Error:
				return TraceEventType.Error;
			case TraceLevel.Warning:
				return TraceEventType.Warning;
			case TraceLevel.Info:
				return TraceEventType.Information;
			case TraceLevel.Verbose:
				return TraceEventType.Verbose;
			default:
				throw new ArgumentOutOfRangeException("level");
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0006074C File Offset: 0x0005E94C
		[NullableContext(1)]
		public void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex)
		{
			if (level == TraceLevel.Off)
			{
				return;
			}
			TraceEventCache eventCache = new TraceEventCache();
			TraceEventType traceEventType = this.GetTraceEventType(level);
			foreach (object obj in System.Diagnostics.Trace.Listeners)
			{
				TraceListener traceListener = (TraceListener)obj;
				if (!traceListener.IsThreadSafe)
				{
					TraceListener obj2 = traceListener;
					lock (obj2)
					{
						traceListener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
						goto IL_6F;
					}
					goto IL_60;
				}
				goto IL_60;
				IL_6F:
				if (System.Diagnostics.Trace.AutoFlush)
				{
					traceListener.Flush();
					continue;
				}
				continue;
				IL_60:
				traceListener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
				goto IL_6F;
			}
		}
	}
}
