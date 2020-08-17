using System;
using System.Security.Cryptography;
using System.Text;

namespace Medo.Security.Cryptography
{
	// Token: 0x02000140 RID: 320
	public class Pbkdf2
	{
		// Token: 0x060008A8 RID: 2216 RVA: 0x0004AA98 File Offset: 0x00048C98
		public Pbkdf2(HMAC algorithm, byte[] password, byte[] salt, int iterations)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm", "Algorithm cannot be null.");
			}
			if (salt == null)
			{
				throw new ArgumentNullException("salt", "Salt cannot be null.");
			}
			if (password == null)
			{
				throw new ArgumentNullException("password", "Password cannot be null.");
			}
			this.Algorithm = algorithm;
			this.Algorithm.Key = password;
			this.Salt = salt;
			this.IterationCount = iterations;
			this.BlockSize = this.Algorithm.HashSize / 8;
			this.BufferBytes = new byte[this.BlockSize];
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0000D26A File Offset: 0x0000B46A
		public Pbkdf2(HMAC algorithm, byte[] password, byte[] salt) : this(algorithm, password, salt, 1000)
		{
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0000D27A File Offset: 0x0000B47A
		public Pbkdf2(HMAC algorithm, string password, string salt, int iterations) : this(algorithm, Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt), iterations)
		{
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0000D29B File Offset: 0x0000B49B
		public Pbkdf2(HMAC algorithm, string password, string salt) : this(algorithm, password, salt, 1000)
		{
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0000D2AB File Offset: 0x0000B4AB
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x0000D2B3 File Offset: 0x0000B4B3
		public HMAC Algorithm { get; private set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0000D2BC File Offset: 0x0000B4BC
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
		public byte[] Salt { get; private set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0000D2CD File Offset: 0x0000B4CD
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x0000D2D5 File Offset: 0x0000B4D5
		public int IterationCount { get; private set; }

		// Token: 0x060008B2 RID: 2226 RVA: 0x0004AB34 File Offset: 0x00048D34
		public byte[] GetBytes(int count)
		{
			byte[] array = new byte[count];
			int i = 0;
			int num = this.BufferEndIndex - this.BufferStartIndex;
			if (num > 0)
			{
				if (count < num)
				{
					Buffer.BlockCopy(this.BufferBytes, this.BufferStartIndex, array, 0, count);
					this.BufferStartIndex += count;
					return array;
				}
				Buffer.BlockCopy(this.BufferBytes, this.BufferStartIndex, array, 0, num);
				this.BufferEndIndex = 0;
				this.BufferStartIndex = 0;
				i += num;
			}
			while (i < count)
			{
				int num2 = count - i;
				this.BufferBytes = this.Func();
				if (num2 <= this.BlockSize)
				{
					Buffer.BlockCopy(this.BufferBytes, 0, array, i, num2);
					this.BufferStartIndex = num2;
					this.BufferEndIndex = this.BlockSize;
					return array;
				}
				Buffer.BlockCopy(this.BufferBytes, 0, array, i, this.BlockSize);
				i += this.BlockSize;
			}
			return array;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0004AC10 File Offset: 0x00048E10
		private byte[] Func()
		{
			byte[] array = new byte[this.Salt.Length + 4];
			Buffer.BlockCopy(this.Salt, 0, array, 0, this.Salt.Length);
			Buffer.BlockCopy(Pbkdf2.GetBytesFromInt(this.BlockIndex), 0, array, this.Salt.Length, 4);
			byte[] array2 = this.Algorithm.ComputeHash(array);
			byte[] array3 = array2;
			for (int i = 2; i <= this.IterationCount; i++)
			{
				array2 = this.Algorithm.ComputeHash(array2, 0, array2.Length);
				for (int j = 0; j < this.BlockSize; j++)
				{
					array3[j] ^= array2[j];
				}
			}
			if (this.BlockIndex == 4294967295U)
			{
				throw new InvalidOperationException("Derived key too long.");
			}
			this.BlockIndex += 1U;
			return array3;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0004ACD8 File Offset: 0x00048ED8
		private static byte[] GetBytesFromInt(uint i)
		{
			byte[] bytes = BitConverter.GetBytes(i);
			if (BitConverter.IsLittleEndian)
			{
				return new byte[]
				{
					bytes[3],
					bytes[2],
					bytes[1],
					bytes[0]
				};
			}
			return bytes;
		}

		// Token: 0x0400065B RID: 1627
		private readonly int BlockSize;

		// Token: 0x0400065C RID: 1628
		private uint BlockIndex = 1U;

		// Token: 0x0400065D RID: 1629
		private byte[] BufferBytes;

		// Token: 0x0400065E RID: 1630
		private int BufferStartIndex;

		// Token: 0x0400065F RID: 1631
		private int BufferEndIndex;
	}
}
