using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x020001B3 RID: 435
	[NullableContext(1)]
	public interface IArrayPool<[Nullable(2)] T>
	{
		// Token: 0x06000B20 RID: 2848
		T[] Rent(int minimumLength);

		// Token: 0x06000B21 RID: 2849
		void Return([Nullable(new byte[]
		{
			2,
			1
		})] T[] array);
	}
}
