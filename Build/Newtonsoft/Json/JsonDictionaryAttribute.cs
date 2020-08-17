using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x020001BD RID: 445
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonDictionaryAttribute : JsonContainerAttribute
	{
		// Token: 0x06000B9E RID: 2974 RVA: 0x0000E7FB File Offset: 0x0000C9FB
		public JsonDictionaryAttribute()
		{
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0000E812 File Offset: 0x0000CA12
		[NullableContext(1)]
		public JsonDictionaryAttribute(string id) : base(id)
		{
		}
	}
}
