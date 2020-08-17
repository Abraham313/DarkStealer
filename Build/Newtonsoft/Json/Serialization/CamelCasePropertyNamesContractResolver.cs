using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000249 RID: 585
	[NullableContext(1)]
	[Nullable(0)]
	public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
	{
		// Token: 0x06001092 RID: 4242 RVA: 0x00012757 File Offset: 0x00010957
		public CamelCasePropertyNamesContractResolver()
		{
			base.NamingStrategy = new CamelCaseNamingStrategy
			{
				ProcessDictionaryKeys = true,
				OverrideSpecifiedNames = true
			};
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0005E440 File Offset: 0x0005C640
		public override JsonContract ResolveContract(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			StructMultiKey<Type, Type> key = new StructMultiKey<Type, Type>(base.GetType(), type);
			Dictionary<StructMultiKey<Type, Type>, JsonContract> contractCache = CamelCasePropertyNamesContractResolver._contractCache;
			JsonContract jsonContract;
			if (contractCache == null || !contractCache.TryGetValue(key, out jsonContract))
			{
				jsonContract = this.CreateContract(type);
				object typeContractCacheLock = CamelCasePropertyNamesContractResolver.TypeContractCacheLock;
				lock (typeContractCacheLock)
				{
					contractCache = CamelCasePropertyNamesContractResolver._contractCache;
					Dictionary<StructMultiKey<Type, Type>, JsonContract> dictionary = (contractCache != null) ? new Dictionary<StructMultiKey<Type, Type>, JsonContract>(contractCache) : new Dictionary<StructMultiKey<Type, Type>, JsonContract>();
					dictionary[key] = jsonContract;
					CamelCasePropertyNamesContractResolver._contractCache = dictionary;
				}
			}
			return jsonContract;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00012778 File Offset: 0x00010978
		internal override DefaultJsonNameTable GetNameTable()
		{
			return CamelCasePropertyNamesContractResolver.NameTable;
		}

		// Token: 0x04000A22 RID: 2594
		private static readonly object TypeContractCacheLock = new object();

		// Token: 0x04000A23 RID: 2595
		private static readonly DefaultJsonNameTable NameTable = new DefaultJsonNameTable();

		// Token: 0x04000A24 RID: 2596
		[Nullable(new byte[]
		{
			2,
			0,
			1,
			1,
			1
		})]
		private static Dictionary<StructMultiKey<Type, Type>, JsonContract> _contractCache;
	}
}
