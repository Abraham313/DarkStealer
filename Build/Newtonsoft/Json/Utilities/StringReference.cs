using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200023E RID: 574
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct StringReference
	{
		// Token: 0x17000333 RID: 819
		public char this[int i]
		{
			get
			{
				return this._chars[i];
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001054 RID: 4180 RVA: 0x0001247F File Offset: 0x0001067F
		public char[] Chars
		{
			get
			{
				return this._chars;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x00012487 File Offset: 0x00010687
		public int StartIndex
		{
			get
			{
				return this._startIndex;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0001248F File Offset: 0x0001068F
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00012497 File Offset: 0x00010697
		public StringReference(char[] chars, int startIndex, int length)
		{
			this._chars = chars;
			this._startIndex = startIndex;
			this._length = length;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x000124AE File Offset: 0x000106AE
		public override string ToString()
		{
			return new string(this._chars, this._startIndex, this._length);
		}

		// Token: 0x04000A0E RID: 2574
		private readonly char[] _chars;

		// Token: 0x04000A0F RID: 2575
		private readonly int _startIndex;

		// Token: 0x04000A10 RID: 2576
		private readonly int _length;
	}
}
