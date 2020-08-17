using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000328 RID: 808
	internal enum BsonType : sbyte
	{
		// Token: 0x04000D45 RID: 3397
		Number = 1,
		// Token: 0x04000D46 RID: 3398
		String,
		// Token: 0x04000D47 RID: 3399
		Object,
		// Token: 0x04000D48 RID: 3400
		Array,
		// Token: 0x04000D49 RID: 3401
		Binary,
		// Token: 0x04000D4A RID: 3402
		Undefined,
		// Token: 0x04000D4B RID: 3403
		Oid,
		// Token: 0x04000D4C RID: 3404
		Boolean,
		// Token: 0x04000D4D RID: 3405
		Date,
		// Token: 0x04000D4E RID: 3406
		Null,
		// Token: 0x04000D4F RID: 3407
		Regex,
		// Token: 0x04000D50 RID: 3408
		Reference,
		// Token: 0x04000D51 RID: 3409
		Code,
		// Token: 0x04000D52 RID: 3410
		Symbol,
		// Token: 0x04000D53 RID: 3411
		CodeWScope,
		// Token: 0x04000D54 RID: 3412
		Integer,
		// Token: 0x04000D55 RID: 3413
		TimeStamp,
		// Token: 0x04000D56 RID: 3414
		Long,
		// Token: 0x04000D57 RID: 3415
		MinKey = -1,
		// Token: 0x04000D58 RID: 3416
		MaxKey = 127
	}
}
