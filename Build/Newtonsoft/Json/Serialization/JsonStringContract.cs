using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000280 RID: 640
	public class JsonStringContract : JsonPrimitiveContract
	{
		// Token: 0x060012B5 RID: 4789 RVA: 0x00013B64 File Offset: 0x00011D64
		[NullableContext(1)]
		public JsonStringContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.String;
		}
	}
}
