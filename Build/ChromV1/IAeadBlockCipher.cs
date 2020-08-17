using System;

namespace ChromV1
{
	// Token: 0x02000077 RID: 119
	public interface IAeadBlockCipher
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600028F RID: 655
		string AlgorithmName { get; }

		// Token: 0x06000290 RID: 656
		void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x06000291 RID: 657
		int GetBlockSize();

		// Token: 0x06000292 RID: 658
		int ProcessByte(byte input, byte[] outBytes, int outOff);

		// Token: 0x06000293 RID: 659
		int ProcessBytes(byte[] inBytes, int inOff, int len, byte[] outBytes, int outOff);

		// Token: 0x06000294 RID: 660
		int DoFinal(byte[] outBytes, int outOff);

		// Token: 0x06000295 RID: 661
		byte[] GetMac();

		// Token: 0x06000296 RID: 662
		int GetUpdateOutputSize(int len);

		// Token: 0x06000297 RID: 663
		int GetOutputSize(int len);

		// Token: 0x06000298 RID: 664
		void Reset();
	}
}
