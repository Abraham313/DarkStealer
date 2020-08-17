using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000285 RID: 645
	[NullableContext(1)]
	[Nullable(0)]
	public class MemoryTraceWriter : ITraceWriter
	{
		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060012D4 RID: 4820 RVA: 0x00013C85 File Offset: 0x00011E85
		// (set) Token: 0x060012D5 RID: 4821 RVA: 0x00013C8D File Offset: 0x00011E8D
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x060012D6 RID: 4822 RVA: 0x00013C96 File Offset: 0x00011E96
		public MemoryTraceWriter()
		{
			this.LevelFilter = TraceLevel.Verbose;
			this._traceMessages = new Queue<string>();
			this._lock = new object();
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00067508 File Offset: 0x00065708
		public void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", CultureInfo.InvariantCulture));
			stringBuilder.Append(" ");
			stringBuilder.Append(level.ToString("g"));
			stringBuilder.Append(" ");
			stringBuilder.Append(message);
			string item = stringBuilder.ToString();
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._traceMessages.Count >= 1000)
				{
					this._traceMessages.Dequeue();
				}
				this._traceMessages.Enqueue(item);
			}
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00013CBB File Offset: 0x00011EBB
		public IEnumerable<string> GetTraceMessages()
		{
			return this._traceMessages;
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x000675CC File Offset: 0x000657CC
		public override string ToString()
		{
			object @lock = this._lock;
			string result;
			lock (@lock)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string value in this._traceMessages)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(value);
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x04000B03 RID: 2819
		private readonly Queue<string> _traceMessages;

		// Token: 0x04000B04 RID: 2820
		private readonly object _lock;
	}
}
