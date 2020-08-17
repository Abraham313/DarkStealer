using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000324 RID: 804
	internal class BsonString : BsonValue
	{
		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x0600192C RID: 6444 RVA: 0x0001820B File Offset: 0x0001640B
		// (set) Token: 0x0600192D RID: 6445 RVA: 0x00018213 File Offset: 0x00016413
		public int ByteCount { get; set; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x0600192E RID: 6446 RVA: 0x0001821C File Offset: 0x0001641C
		public bool IncludeLength { get; }

		// Token: 0x0600192F RID: 6447 RVA: 0x00018224 File Offset: 0x00016424
		public BsonString(object value, bool includeLength) : base(value, BsonType.String)
		{
			this.IncludeLength = includeLength;
		}
	}
}
