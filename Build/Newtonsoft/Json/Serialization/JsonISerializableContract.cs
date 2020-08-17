using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000271 RID: 625
	public class JsonISerializableContract : JsonContainerContract
	{
		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060011A0 RID: 4512 RVA: 0x000130BF File Offset: 0x000112BF
		// (set) Token: 0x060011A1 RID: 4513 RVA: 0x000130C7 File Offset: 0x000112C7
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public ObjectConstructor<object> ISerializableCreator { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x060011A2 RID: 4514 RVA: 0x000130D0 File Offset: 0x000112D0
		[NullableContext(1)]
		public JsonISerializableContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Serializable;
		}
	}
}
