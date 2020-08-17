using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000244 RID: 580
	[NullableContext(1)]
	[Nullable(0)]
	internal class ThreadSafeStore<[Nullable(2)] TKey, [Nullable(2)] TValue>
	{
		// Token: 0x06001077 RID: 4215 RVA: 0x0001263E File Offset: 0x0001083E
		public ThreadSafeStore(Func<TKey, TValue> creator)
		{
			ValidationUtils.ArgumentNotNull(creator, "creator");
			this._creator = creator;
			this._concurrentStore = new ConcurrentDictionary<TKey, TValue>();
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x00012663 File Offset: 0x00010863
		public TValue Get(TKey key)
		{
			return this._concurrentStore.GetOrAdd(key, this._creator);
		}

		// Token: 0x04000A1F RID: 2591
		private readonly ConcurrentDictionary<TKey, TValue> _concurrentStore;

		// Token: 0x04000A20 RID: 2592
		private readonly Func<TKey, TValue> _creator;
	}
}
