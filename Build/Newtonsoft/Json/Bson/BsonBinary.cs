using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000325 RID: 805
	internal class BsonBinary : BsonValue
	{
		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001930 RID: 6448 RVA: 0x00018235 File Offset: 0x00016435
		// (set) Token: 0x06001931 RID: 6449 RVA: 0x0001823D File Offset: 0x0001643D
		public BsonBinaryType BinaryType { get; set; }

		// Token: 0x06001932 RID: 6450 RVA: 0x00018246 File Offset: 0x00016446
		public BsonBinary(byte[] value, BsonBinaryType binaryType) : base(value, BsonType.Binary)
		{
			this.BinaryType = binaryType;
		}
	}
}
