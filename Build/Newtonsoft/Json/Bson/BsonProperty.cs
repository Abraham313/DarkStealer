using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000327 RID: 807
	internal class BsonProperty
	{
		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001939 RID: 6457 RVA: 0x0001829F File Offset: 0x0001649F
		// (set) Token: 0x0600193A RID: 6458 RVA: 0x000182A7 File Offset: 0x000164A7
		public BsonString Name { get; set; }

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x000182B0 File Offset: 0x000164B0
		// (set) Token: 0x0600193C RID: 6460 RVA: 0x000182B8 File Offset: 0x000164B8
		public BsonToken Value { get; set; }
	}
}
