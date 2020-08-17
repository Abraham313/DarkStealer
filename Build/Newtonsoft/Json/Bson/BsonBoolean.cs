using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000323 RID: 803
	internal class BsonBoolean : BsonValue
	{
		// Token: 0x0600192A RID: 6442 RVA: 0x000181E4 File Offset: 0x000163E4
		private BsonBoolean(bool value) : base(value, BsonType.Boolean)
		{
		}

		// Token: 0x04000D3B RID: 3387
		public static readonly BsonBoolean False = new BsonBoolean(false);

		// Token: 0x04000D3C RID: 3388
		public static readonly BsonBoolean True = new BsonBoolean(true);
	}
}
