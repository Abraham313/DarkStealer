using System;

namespace ChromV1
{
	// Token: 0x0200006D RID: 109
	public interface IBlockCipher
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000253 RID: 595
		string AlgorithmName { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000254 RID: 596
		bool IsPartialBlockOkay { get; }

		// Token: 0x06000255 RID: 597
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x06000256 RID: 598
		int GetBlockSize();

		// Token: 0x06000257 RID: 599
		int ProcessBlock(byte[] inBuf, int inOff, byte[] outBuf, int outOff);

		// Token: 0x06000258 RID: 600
		void Reset();
	}
}
