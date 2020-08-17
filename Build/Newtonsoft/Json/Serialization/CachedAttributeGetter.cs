using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000247 RID: 583
	internal static class CachedAttributeGetter<T> where T : Attribute
	{
		// Token: 0x0600108C RID: 4236 RVA: 0x000126FB File Offset: 0x000108FB
		[NullableContext(1)]
		[return: Nullable(2)]
		public static T GetAttribute(object type)
		{
			return CachedAttributeGetter<T>.TypeAttributeCache.Get(type);
		}

		// Token: 0x04000A21 RID: 2593
		[Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		private static readonly ThreadSafeStore<object, T> TypeAttributeCache = new ThreadSafeStore<object, T>(new Func<object, T>(JsonTypeReflector.GetAttribute<T>));
	}
}
