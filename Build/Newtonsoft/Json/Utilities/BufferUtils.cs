using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000222 RID: 546
	[NullableContext(2)]
	[Nullable(0)]
	internal static class BufferUtils
	{
		// Token: 0x06000FAB RID: 4011 RVA: 0x00011E48 File Offset: 0x00010048
		[NullableContext(1)]
		public static char[] RentBuffer([Nullable(2)] IArrayPool<char> bufferPool, int minSize)
		{
			if (bufferPool == null)
			{
				return new char[minSize];
			}
			return bufferPool.Rent(minSize);
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00011E5B File Offset: 0x0001005B
		public static void ReturnBuffer(IArrayPool<char> bufferPool, char[] buffer)
		{
			if (bufferPool != null)
			{
				bufferPool.Return(buffer);
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00011E67 File Offset: 0x00010067
		[return: Nullable(1)]
		public static char[] EnsureBufferSize(IArrayPool<char> bufferPool, int size, char[] buffer)
		{
			if (bufferPool == null)
			{
				return new char[size];
			}
			if (buffer != null)
			{
				bufferPool.Return(buffer);
			}
			return bufferPool.Rent(size);
		}
	}
}
