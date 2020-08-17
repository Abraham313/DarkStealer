using System;

namespace ChromV1
{
	// Token: 0x02000079 RID: 121
	public interface IGcmMultiplier
	{
		// Token: 0x060002A1 RID: 673
		void Init(byte[] H);

		// Token: 0x060002A2 RID: 674
		void MultiplyH(byte[] x);
	}
}
