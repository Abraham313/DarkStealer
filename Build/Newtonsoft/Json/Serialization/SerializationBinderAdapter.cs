using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200028B RID: 651
	[NullableContext(1)]
	[Nullable(0)]
	internal class SerializationBinderAdapter : ISerializationBinder
	{
		// Token: 0x060012F3 RID: 4851 RVA: 0x00013D92 File Offset: 0x00011F92
		public SerializationBinderAdapter(SerializationBinder serializationBinder)
		{
			this.SerializationBinder = serializationBinder;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00013DA1 File Offset: 0x00011FA1
		public Type BindToType([Nullable(2)] string assemblyName, string typeName)
		{
			return this.SerializationBinder.BindToType(assemblyName, typeName);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00013DB0 File Offset: 0x00011FB0
		[NullableContext(2)]
		public void BindToName([Nullable(1)] Type serializedType, out string assemblyName, out string typeName)
		{
			this.SerializationBinder.BindToName(serializedType, out assemblyName, out typeName);
		}

		// Token: 0x04000B0B RID: 2827
		public readonly SerializationBinder SerializationBinder;
	}
}
