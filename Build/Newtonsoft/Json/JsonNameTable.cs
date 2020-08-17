using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x020001C1 RID: 449
	public abstract class JsonNameTable
	{
		// Token: 0x06000BAB RID: 2987
		[NullableContext(1)]
		[return: Nullable(2)]
		public abstract string Get(char[] key, int start, int length);
	}
}
