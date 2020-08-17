using System;
using System.Security.Cryptography;

namespace CG.Web.MegaApiClient.Cryptography
{
	// Token: 0x02000198 RID: 408
	internal class CachedCryptoTransform : IDisposable, ICryptoTransform
	{
		// Token: 0x06000AF9 RID: 2809 RVA: 0x0000E60A File Offset: 0x0000C80A
		public CachedCryptoTransform(Func<ICryptoTransform> factory, bool isKnownReusable)
		{
			this.factory = factory;
			this.isKnownReusable = isKnownReusable;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0000E620 File Offset: 0x0000C820
		public void Dispose()
		{
			ICryptoTransform cryptoTransform = this.cachedInstance;
			if (cryptoTransform == null)
			{
				return;
			}
			cryptoTransform.Dispose();
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0004E23C File Offset: 0x0004C43C
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			return this.Forward<int>((ICryptoTransform x) => x.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset));
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0004E288 File Offset: 0x0004C488
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (this.isKnownReusable && this.cachedInstance != null)
			{
				return this.cachedInstance.TransformFinalBlock(inputBuffer, inputOffset, inputCount);
			}
			return this.Forward<byte[]>((ICryptoTransform x) => x.TransformFinalBlock(inputBuffer, inputOffset, inputCount));
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x0000E632 File Offset: 0x0000C832
		public int InputBlockSize
		{
			get
			{
				return this.Forward<int>((ICryptoTransform x) => x.InputBlockSize);
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0000E659 File Offset: 0x0000C859
		public int OutputBlockSize
		{
			get
			{
				return this.Forward<int>((ICryptoTransform x) => x.OutputBlockSize);
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0000E680 File Offset: 0x0000C880
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return this.Forward<bool>((ICryptoTransform x) => x.CanTransformMultipleBlocks);
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0000E6A7 File Offset: 0x0000C8A7
		public bool CanReuseTransform
		{
			get
			{
				return this.Forward<bool>((ICryptoTransform x) => x.CanReuseTransform);
			}
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0004E2F0 File Offset: 0x0004C4F0
		private T Forward<T>(Func<ICryptoTransform, T> action)
		{
			ICryptoTransform cryptoTransform = this.cachedInstance ?? this.factory();
			T result;
			try
			{
				result = action(cryptoTransform);
			}
			finally
			{
				if (!this.isKnownReusable && !cryptoTransform.CanReuseTransform)
				{
					cryptoTransform.Dispose();
				}
				else
				{
					this.cachedInstance = cryptoTransform;
				}
			}
			return result;
		}

		// Token: 0x04000768 RID: 1896
		private readonly Func<ICryptoTransform> factory;

		// Token: 0x04000769 RID: 1897
		private readonly bool isKnownReusable;

		// Token: 0x0400076A RID: 1898
		private ICryptoTransform cachedInstance;
	}
}
