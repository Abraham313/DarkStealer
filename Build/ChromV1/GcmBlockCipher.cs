using System;
using SmartAssembly.StringsEncoding;

namespace ChromV1
{
	// Token: 0x02000076 RID: 118
	public class GcmBlockCipher : IAeadBlockCipher
	{
		// Token: 0x0600027D RID: 637 RVA: 0x00009CBD File Offset: 0x00007EBD
		public GcmBlockCipher(IBlockCipher c) : this(c, null)
		{
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00026300 File Offset: 0x00024500
		public GcmBlockCipher(IBlockCipher c, IGcmMultiplier m)
		{
			if (c.GetBlockSize() != 16)
			{
				throw new ArgumentException(Strings.Get(107395085) + 16.ToString() + Strings.Get(107395576));
			}
			if (m == null)
			{
				m = new Tables8kGcmMultiplier();
			}
			this.cipher = c;
			this.multiplier = m;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600027F RID: 639 RVA: 0x00009CC7 File Offset: 0x00007EC7
		public virtual string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + Strings.Get(107395571);
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00009B54 File Offset: 0x00007D54
		public virtual int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00026360 File Offset: 0x00024560
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			this.macBlock = null;
			if (parameters is AeadParameters)
			{
				AeadParameters aeadParameters = (AeadParameters)parameters;
				this.nonce = aeadParameters.GetNonce();
				this.A = aeadParameters.GetAssociatedText();
				int num = aeadParameters.MacSize;
				if (num < 96 || num > 128 || num % 8 != 0)
				{
					throw new ArgumentException(Strings.Get(107395530) + num.ToString());
				}
				this.macSize = num / 8;
				this.keyParam = aeadParameters.Key;
			}
			else
			{
				if (!(parameters is ParametersWithIV))
				{
					throw new ArgumentException(Strings.Get(107395489));
				}
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				this.nonce = parametersWithIV.GetIV();
				this.A = null;
				this.macSize = 16;
				this.keyParam = (KeyParameter)parametersWithIV.Parameters;
			}
			int num2 = forEncryption ? 16 : (16 + this.macSize);
			this.bufBlock = new byte[num2];
			if (this.nonce != null && this.nonce.Length >= 1)
			{
				if (this.A == null)
				{
					this.A = new byte[0];
				}
				this.cipher.Init(true, this.keyParam);
				this.H = new byte[16];
				this.cipher.ProcessBlock(this.H, 0, this.H, 0);
				this.multiplier.Init(this.H);
				this.initS = this.gHASH(this.A);
				if (this.nonce.Length == 12)
				{
					this.J0 = new byte[16];
					Array.Copy(this.nonce, 0, this.J0, 0, this.nonce.Length);
					this.J0[15] = 1;
				}
				else
				{
					this.J0 = this.gHASH(this.nonce);
					byte[] array = new byte[16];
					GcmBlockCipher.packLength((ulong)((long)this.nonce.Length * 8L), array, 8);
					GcmUtilities.Xor(this.J0, array);
					this.multiplier.MultiplyH(this.J0);
				}
				this.S = Arrays.Clone(this.initS);
				this.counter = Arrays.Clone(this.J0);
				this.bufOff = 0;
				this.totalLength = 0UL;
				return;
			}
			throw new ArgumentException(Strings.Get(107395476));
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00009CE3 File Offset: 0x00007EE3
		public virtual byte[] GetMac()
		{
			return Arrays.Clone(this.macBlock);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00009CF0 File Offset: 0x00007EF0
		public virtual int GetOutputSize(int len)
		{
			if (this.forEncryption)
			{
				return len + this.bufOff + this.macSize;
			}
			return len + this.bufOff - this.macSize;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00009D19 File Offset: 0x00007F19
		public virtual int GetUpdateOutputSize(int len)
		{
			return (len + this.bufOff) / 16 * 16;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00009D29 File Offset: 0x00007F29
		public virtual int ProcessByte(byte input, byte[] output, int outOff)
		{
			return this.Process(input, output, outOff);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x000265C0 File Offset: 0x000247C0
		public virtual int ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			int num = 0;
			for (int num2 = 0; num2 != len; num2++)
			{
				byte[] array = this.bufBlock;
				int num3 = this.bufOff;
				this.bufOff = num3 + 1;
				array[num3] = input[inOff + num2];
				if (this.bufOff == this.bufBlock.Length)
				{
					this.gCTRBlock(this.bufBlock, 16, output, outOff + num);
					if (!this.forEncryption)
					{
						Array.Copy(this.bufBlock, 16, this.bufBlock, 0, this.macSize);
					}
					this.bufOff = this.bufBlock.Length - 16;
					num += 16;
				}
			}
			return num;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00026658 File Offset: 0x00024858
		public int DoFinal(byte[] output, int outOff)
		{
			int num = this.bufOff;
			if (!this.forEncryption)
			{
				if (num < this.macSize)
				{
					throw new InvalidCipherTextException(Strings.Get(107395407));
				}
				num -= this.macSize;
			}
			if (num > 0)
			{
				byte[] array = new byte[16];
				Array.Copy(this.bufBlock, 0, array, 0, num);
				this.gCTRBlock(array, num, output, outOff);
			}
			byte[] array2 = new byte[16];
			GcmBlockCipher.packLength((ulong)((long)this.A.Length * 8L), array2, 0);
			GcmBlockCipher.packLength(this.totalLength * 8UL, array2, 8);
			GcmUtilities.Xor(this.S, array2);
			this.multiplier.MultiplyH(this.S);
			byte[] array3 = new byte[16];
			this.cipher.ProcessBlock(this.J0, 0, array3, 0);
			GcmUtilities.Xor(array3, this.S);
			int num2 = num;
			this.macBlock = new byte[this.macSize];
			Array.Copy(array3, 0, this.macBlock, 0, this.macSize);
			if (this.forEncryption)
			{
				Array.Copy(this.macBlock, 0, output, outOff + this.bufOff, this.macSize);
				num2 += this.macSize;
			}
			else
			{
				byte[] array4 = new byte[this.macSize];
				Array.Copy(this.bufBlock, num, array4, 0, this.macSize);
				if (!Arrays.ConstantTimeAreEqual(this.macBlock, array4))
				{
					throw new InvalidCipherTextException(Strings.Get(107395418));
				}
			}
			this.Reset(false);
			return num2;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00009D34 File Offset: 0x00007F34
		public virtual void Reset()
		{
			this.Reset(true);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x000267DC File Offset: 0x000249DC
		private int Process(byte byte_0, byte[] byte_1, int int_0)
		{
			byte[] array = this.bufBlock;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = byte_0;
			if (this.bufOff == this.bufBlock.Length)
			{
				this.gCTRBlock(this.bufBlock, 16, byte_1, int_0);
				if (!this.forEncryption)
				{
					Array.Copy(this.bufBlock, 16, this.bufBlock, 0, this.macSize);
				}
				this.bufOff = this.bufBlock.Length - 16;
				return 16;
			}
			return 0;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0002685C File Offset: 0x00024A5C
		private void Reset(bool bool_0)
		{
			this.S = Arrays.Clone(this.initS);
			this.counter = Arrays.Clone(this.J0);
			this.bufOff = 0;
			this.totalLength = 0UL;
			if (this.bufBlock != null)
			{
				Array.Clear(this.bufBlock, 0, this.bufBlock.Length);
			}
			if (bool_0)
			{
				this.macBlock = null;
			}
			this.cipher.Reset();
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000268D4 File Offset: 0x00024AD4
		private void gCTRBlock(byte[] byte_0, int int_0, byte[] byte_1, int int_1)
		{
			for (int i = 15; i >= 12; i--)
			{
				byte[] array = this.counter;
				int num = i;
				byte b = array[num] + 1;
				array[num] = b;
				if (b != 0)
				{
					break;
				}
			}
			byte[] array2 = new byte[16];
			this.cipher.ProcessBlock(this.counter, 0, array2, 0);
			byte[] byte_2;
			if (this.forEncryption)
			{
				Array.Copy(GcmBlockCipher.Zeroes, int_0, array2, int_0, 16 - int_0);
				byte_2 = array2;
			}
			else
			{
				byte_2 = byte_0;
			}
			for (int j = int_0 - 1; j >= 0; j--)
			{
				byte[] array3 = array2;
				int num2 = j;
				array3[num2] ^= byte_0[j];
				byte_1[int_1 + j] = array2[j];
			}
			GcmUtilities.Xor(this.S, byte_2);
			this.multiplier.MultiplyH(this.S);
			this.totalLength += (ulong)((long)int_0);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0002699C File Offset: 0x00024B9C
		private byte[] gHASH(byte[] byte_0)
		{
			byte[] array = new byte[16];
			for (int i = 0; i < byte_0.Length; i += 16)
			{
				byte[] array2 = new byte[16];
				int length = Math.Min(byte_0.Length - i, 16);
				Array.Copy(byte_0, i, array2, 0, length);
				GcmUtilities.Xor(array, array2);
				this.multiplier.MultiplyH(array);
			}
			return array;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00009D3D File Offset: 0x00007F3D
		private static void packLength(ulong ulong_0, byte[] byte_0, int int_0)
		{
			Pack.UInt32_To_BE((uint)(ulong_0 >> 32), byte_0, int_0);
			Pack.UInt32_To_BE((uint)ulong_0, byte_0, int_0 + 4);
		}

		// Token: 0x04000166 RID: 358
		private static readonly byte[] Zeroes = new byte[16];

		// Token: 0x04000167 RID: 359
		private readonly IBlockCipher cipher;

		// Token: 0x04000168 RID: 360
		private readonly IGcmMultiplier multiplier;

		// Token: 0x04000169 RID: 361
		private byte[] A;

		// Token: 0x0400016A RID: 362
		private byte[] bufBlock;

		// Token: 0x0400016B RID: 363
		private int bufOff;

		// Token: 0x0400016C RID: 364
		private byte[] counter;

		// Token: 0x0400016D RID: 365
		private bool forEncryption;

		// Token: 0x0400016E RID: 366
		private byte[] H;

		// Token: 0x0400016F RID: 367
		private byte[] initS;

		// Token: 0x04000170 RID: 368
		private byte[] J0;

		// Token: 0x04000171 RID: 369
		private KeyParameter keyParam;

		// Token: 0x04000172 RID: 370
		private byte[] macBlock;

		// Token: 0x04000173 RID: 371
		private int macSize;

		// Token: 0x04000174 RID: 372
		private byte[] nonce;

		// Token: 0x04000175 RID: 373
		private byte[] S;

		// Token: 0x04000176 RID: 374
		private ulong totalLength;
	}
}
