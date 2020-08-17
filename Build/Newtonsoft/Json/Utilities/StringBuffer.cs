using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200023D RID: 573
	[NullableContext(2)]
	[Nullable(0)]
	internal struct StringBuffer
	{
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x000123F0 File Offset: 0x000105F0
		// (set) Token: 0x06001048 RID: 4168 RVA: 0x000123F8 File Offset: 0x000105F8
		public int Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x00012401 File Offset: 0x00010601
		public bool IsEmpty
		{
			get
			{
				return this._buffer == null;
			}
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0001240C File Offset: 0x0001060C
		public StringBuffer(IArrayPool<char> bufferPool, int initalSize)
		{
			this = new StringBuffer(BufferUtils.RentBuffer(bufferPool, initalSize));
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0001241B File Offset: 0x0001061B
		[NullableContext(1)]
		private StringBuffer(char[] buffer)
		{
			this._buffer = buffer;
			this._position = 0;
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0005DD90 File Offset: 0x0005BF90
		public void Append(IArrayPool<char> bufferPool, char value)
		{
			if (this._position == this._buffer.Length)
			{
				this.EnsureSize(bufferPool, 1);
			}
			char[] buffer = this._buffer;
			int position = this._position;
			this._position = position + 1;
			buffer[position] = value;
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0005DDD0 File Offset: 0x0005BFD0
		[NullableContext(1)]
		public void Append([Nullable(2)] IArrayPool<char> bufferPool, char[] buffer, int startIndex, int count)
		{
			if (this._position + count >= this._buffer.Length)
			{
				this.EnsureSize(bufferPool, count);
			}
			Array.Copy(buffer, startIndex, this._buffer, this._position, count);
			this._position += count;
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0001242B File Offset: 0x0001062B
		public void Clear(IArrayPool<char> bufferPool)
		{
			if (this._buffer != null)
			{
				BufferUtils.ReturnBuffer(bufferPool, this._buffer);
				this._buffer = null;
			}
			this._position = 0;
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0005DE20 File Offset: 0x0005C020
		private void EnsureSize(IArrayPool<char> bufferPool, int appendLength)
		{
			char[] array = BufferUtils.RentBuffer(bufferPool, (this._position + appendLength) * 2);
			if (this._buffer != null)
			{
				Array.Copy(this._buffer, array, this._position);
				BufferUtils.ReturnBuffer(bufferPool, this._buffer);
			}
			this._buffer = array;
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0001244F File Offset: 0x0001064F
		[NullableContext(1)]
		public override string ToString()
		{
			return this.ToString(0, this._position);
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0001245E File Offset: 0x0001065E
		[NullableContext(1)]
		public string ToString(int start, int length)
		{
			return new string(this._buffer, start, length);
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x0001246D File Offset: 0x0001066D
		public char[] InternalBuffer
		{
			get
			{
				return this._buffer;
			}
		}

		// Token: 0x04000A0C RID: 2572
		private char[] _buffer;

		// Token: 0x04000A0D RID: 2573
		private int _position;
	}
}
