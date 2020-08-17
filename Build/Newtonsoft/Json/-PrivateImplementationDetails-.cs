using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x0200032A RID: 810
[CompilerGenerated]
internal sealed class <PrivateImplementationDetails>
{
	// Token: 0x06001969 RID: 6505 RVA: 0x000780DC File Offset: 0x000762DC
	internal static uint ComputeStringHash(string s)
	{
		uint num;
		if (s != null)
		{
			num = 2166136261U;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((uint)s[i] ^ num) * 16777619U;
			}
		}
		return num;
	}

	// Token: 0x04000D5D RID: 3421 RVA: 0x00008688 File Offset: 0x00006888
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.Struct34 struct34_0;

	// Token: 0x04000D5E RID: 3422 RVA: 0x00008690 File Offset: 0x00006890
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.Struct36 struct36_0;

	// Token: 0x04000D5F RID: 3423 RVA: 0x000086B0 File Offset: 0x000068B0
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.Struct35 D40004AB0E92BF6C8DFE481B56BE3D04ABDA76EB;

	// Token: 0x04000D60 RID: 3424 RVA: 0x000086C0 File Offset: 0x000068C0
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.Struct38 DD3AEFEADB1CD615F3017763F1568179FEE640B0;

	// Token: 0x04000D61 RID: 3425 RVA: 0x000086F8 File Offset: 0x000068F8
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.Struct37 E289D9D3D233BC253E8C0FA8C2AFDD86A407CE30;

	// Token: 0x04000D62 RID: 3426 RVA: 0x00008720 File Offset: 0x00006920
	internal static readonly Newtonsoft.Json.<PrivateImplementationDetails>.Struct38 E92B39D8233061927D9ACDE54665E68E7535635A;

	// Token: 0x0200032B RID: 811
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 6)]
	private struct Struct34
	{
	}

	// Token: 0x0200032C RID: 812
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 10)]
	private struct Struct35
	{
	}

	// Token: 0x0200032D RID: 813
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 28)]
	private struct Struct36
	{
	}

	// Token: 0x0200032E RID: 814
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 36)]
	private struct Struct37
	{
	}

	// Token: 0x0200032F RID: 815
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 52)]
	private struct Struct38
	{
	}
}
