using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000321 RID: 801
	internal class BsonEmpty : BsonToken
	{
		// Token: 0x06001924 RID: 6436 RVA: 0x0001818E File Offset: 0x0001638E
		private BsonEmpty(BsonType type)
		{
			this.Type = type;
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001925 RID: 6437 RVA: 0x0001819D File Offset: 0x0001639D
		public override BsonType Type { get; }

		// Token: 0x04000D36 RID: 3382
		public static readonly BsonToken Null = new BsonEmpty(BsonType.Null);

		// Token: 0x04000D37 RID: 3383
		public static readonly BsonToken Undefined = new BsonEmpty(BsonType.Undefined);
	}
}
