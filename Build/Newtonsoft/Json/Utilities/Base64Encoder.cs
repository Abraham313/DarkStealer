using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020001E5 RID: 485
	[NullableContext(1)]
	[Nullable(0)]
	internal class Base64Encoder
	{
		// Token: 0x06000E37 RID: 3639 RVA: 0x00010C4E File Offset: 0x0000EE4E
		public Base64Encoder(TextWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = writer;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00056674 File Offset: 0x00054874
		private void ValidateEncode(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x000566C0 File Offset: 0x000548C0
		public void Encode(byte[] buffer, int index, int count)
		{
			this.ValidateEncode(buffer, index, count);
			if (this._leftOverBytesCount > 0)
			{
				if (this.FulfillFromLeftover(buffer, index, ref count))
				{
					return;
				}
				int count2 = Convert.ToBase64CharArray(this._leftOverBytes, 0, 3, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, count2);
			}
			this.StoreLeftOverBytes(buffer, index, ref count);
			int num = index + count;
			int num2 = 57;
			while (index < num)
			{
				if (index + num2 > num)
				{
					num2 = num - index;
				}
				int count3 = Convert.ToBase64CharArray(buffer, index, num2, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, count3);
				index += num2;
			}
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00056754 File Offset: 0x00054954
		private void StoreLeftOverBytes(byte[] buffer, int index, ref int count)
		{
			int num = count % 3;
			if (num > 0)
			{
				count -= num;
				if (this._leftOverBytes == null)
				{
					this._leftOverBytes = new byte[3];
				}
				for (int i = 0; i < num; i++)
				{
					this._leftOverBytes[i] = buffer[index + count + i];
				}
			}
			this._leftOverBytesCount = num;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x000567A8 File Offset: 0x000549A8
		private bool FulfillFromLeftover(byte[] buffer, int index, ref int count)
		{
			int leftOverBytesCount = this._leftOverBytesCount;
			while (leftOverBytesCount < 3 && count > 0)
			{
				this._leftOverBytes[leftOverBytesCount++] = buffer[index++];
				count--;
			}
			if (count == 0 && leftOverBytesCount < 3)
			{
				this._leftOverBytesCount = leftOverBytesCount;
				return true;
			}
			return false;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x000567F4 File Offset: 0x000549F4
		public void Flush()
		{
			if (this._leftOverBytesCount > 0)
			{
				int count = Convert.ToBase64CharArray(this._leftOverBytes, 0, this._leftOverBytesCount, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, count);
				this._leftOverBytesCount = 0;
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00010C75 File Offset: 0x0000EE75
		private void WriteChars(char[] chars, int index, int count)
		{
			this._writer.Write(chars, index, count);
		}

		// Token: 0x040008F5 RID: 2293
		private const int Base64LineSize = 76;

		// Token: 0x040008F6 RID: 2294
		private const int LineSizeInBytes = 57;

		// Token: 0x040008F7 RID: 2295
		private readonly char[] _charsLine = new char[76];

		// Token: 0x040008F8 RID: 2296
		private readonly TextWriter _writer;

		// Token: 0x040008F9 RID: 2297
		[Nullable(2)]
		private byte[] _leftOverBytes;

		// Token: 0x040008FA RID: 2298
		private int _leftOverBytesCount;
	}
}
