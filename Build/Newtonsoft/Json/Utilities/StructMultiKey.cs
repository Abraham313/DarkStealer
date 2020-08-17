using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000243 RID: 579
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct StructMultiKey<[Nullable(2)] T1, [Nullable(2)] T2> : IEquatable<StructMultiKey<T1, T2>>
	{
		// Token: 0x06001073 RID: 4211 RVA: 0x000125F2 File Offset: 0x000107F2
		public StructMultiKey(T1 v1, T2 v2)
		{
			this.Value1 = v1;
			this.Value2 = v2;
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0005E290 File Offset: 0x0005C490
		public override int GetHashCode()
		{
			T1 value = this.Value1;
			ref T1 ptr = ref value;
			T1 t = default(T1);
			int num;
			if (t == null)
			{
				t = value;
				ptr = ref t;
				if (t == null)
				{
					num = 0;
					goto IL_38;
				}
			}
			num = ptr.GetHashCode();
			IL_38:
			T2 value2 = this.Value2;
			ref T2 ptr2 = ref value2;
			T2 t2 = default(T2);
			int num2;
			if (t2 == null)
			{
				t2 = value2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					num2 = 0;
					goto IL_70;
				}
			}
			num2 = ptr2.GetHashCode();
			IL_70:
			return num ^ num2;
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0005E310 File Offset: 0x0005C510
		public override bool Equals(object obj)
		{
			if (obj is StructMultiKey<T1, T2>)
			{
				StructMultiKey<T1, T2> other = (StructMultiKey<T1, T2>)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00012602 File Offset: 0x00010802
		public bool Equals([Nullable(new byte[]
		{
			0,
			1,
			1
		})] StructMultiKey<T1, T2> other)
		{
			return object.Equals(this.Value1, other.Value1) && object.Equals(this.Value2, other.Value2);
		}

		// Token: 0x04000A1D RID: 2589
		public readonly T1 Value1;

		// Token: 0x04000A1E RID: 2590
		public readonly T2 Value2;
	}
}
