using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000272 RID: 626
	public class JsonLinqContract : JsonContract
	{
		// Token: 0x060011A3 RID: 4515 RVA: 0x000130E0 File Offset: 0x000112E0
		[NullableContext(1)]
		public JsonLinqContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Linq;
		}
	}
}
