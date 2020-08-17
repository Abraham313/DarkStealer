using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000326 RID: 806
	internal class BsonRegex : BsonToken
	{
		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001933 RID: 6451 RVA: 0x00018257 File Offset: 0x00016457
		// (set) Token: 0x06001934 RID: 6452 RVA: 0x0001825F File Offset: 0x0001645F
		public BsonString Pattern { get; set; }

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001935 RID: 6453 RVA: 0x00018268 File Offset: 0x00016468
		// (set) Token: 0x06001936 RID: 6454 RVA: 0x00018270 File Offset: 0x00016470
		public BsonString Options { get; set; }

		// Token: 0x06001937 RID: 6455 RVA: 0x00018279 File Offset: 0x00016479
		public BsonRegex(string pattern, string options)
		{
			this.Pattern = new BsonString(pattern, false);
			this.Options = new BsonString(options, false);
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001938 RID: 6456 RVA: 0x0001829B File Offset: 0x0001649B
		public override BsonType Type
		{
			get
			{
				return BsonType.Regex;
			}
		}
	}
}
