using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000318 RID: 792
	internal enum BsonBinaryType : byte
	{
		// Token: 0x04000D0A RID: 3338
		Binary,
		// Token: 0x04000D0B RID: 3339
		Function,
		// Token: 0x04000D0C RID: 3340
		[Obsolete("This type has been deprecated in the BSON specification. Use Binary instead.")]
		BinaryOld,
		// Token: 0x04000D0D RID: 3341
		[Obsolete("This type has been deprecated in the BSON specification. Use Uuid instead.")]
		UuidOld,
		// Token: 0x04000D0E RID: 3342
		Uuid,
		// Token: 0x04000D0F RID: 3343
		Md5,
		// Token: 0x04000D10 RID: 3344
		UserDefined = 128
	}
}
