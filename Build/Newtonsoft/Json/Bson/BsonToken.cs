using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200031E RID: 798
	internal abstract class BsonToken
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001914 RID: 6420
		public abstract BsonType Type { get; }

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001915 RID: 6421 RVA: 0x000180D0 File Offset: 0x000162D0
		// (set) Token: 0x06001916 RID: 6422 RVA: 0x000180D8 File Offset: 0x000162D8
		public BsonToken Parent { get; set; }

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001917 RID: 6423 RVA: 0x000180E1 File Offset: 0x000162E1
		// (set) Token: 0x06001918 RID: 6424 RVA: 0x000180E9 File Offset: 0x000162E9
		public int CalculatedSize { get; set; }
	}
}
