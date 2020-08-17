using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200031A RID: 794
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonObjectId
	{
		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x00017EFD File Offset: 0x000160FD
		public byte[] Value { get; }

		// Token: 0x060018F0 RID: 6384 RVA: 0x00017F05 File Offset: 0x00016105
		public BsonObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new ArgumentException("An ObjectId must be 12 bytes", "value");
			}
			this.Value = value;
		}
	}
}
