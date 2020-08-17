using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020001E6 RID: 486
	[NullableContext(1)]
	[Nullable(0)]
	internal class BidirectionalDictionary<[Nullable(2)] TFirst, [Nullable(2)] TSecond>
	{
		// Token: 0x06000E3E RID: 3646 RVA: 0x00010C85 File Offset: 0x0000EE85
		public BidirectionalDictionary() : this(EqualityComparer<TFirst>.Default, EqualityComparer<TSecond>.Default)
		{
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00010C97 File Offset: 0x0000EE97
		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer) : this(firstEqualityComparer, secondEqualityComparer, "Duplicate item already exists for '{0}'.", "Duplicate item already exists for '{0}'.")
		{
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00010CAB File Offset: 0x0000EEAB
		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer, string duplicateFirstErrorMessage, string duplicateSecondErrorMessage)
		{
			this._firstToSecond = new Dictionary<TFirst, TSecond>(firstEqualityComparer);
			this._secondToFirst = new Dictionary<TSecond, TFirst>(secondEqualityComparer);
			this._duplicateFirstErrorMessage = duplicateFirstErrorMessage;
			this._duplicateSecondErrorMessage = duplicateSecondErrorMessage;
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0005683C File Offset: 0x00054A3C
		public void Set(TFirst first, TSecond second)
		{
			TSecond tsecond;
			if (this._firstToSecond.TryGetValue(first, out tsecond) && !tsecond.Equals(second))
			{
				throw new ArgumentException(this._duplicateFirstErrorMessage.FormatWith(CultureInfo.InvariantCulture, first));
			}
			TFirst tfirst;
			if (this._secondToFirst.TryGetValue(second, out tfirst) && !tfirst.Equals(first))
			{
				throw new ArgumentException(this._duplicateSecondErrorMessage.FormatWith(CultureInfo.InvariantCulture, second));
			}
			this._firstToSecond.Add(first, second);
			this._secondToFirst.Add(second, first);
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00010CDA File Offset: 0x0000EEDA
		public bool TryGetByFirst(TFirst first, out TSecond second)
		{
			return this._firstToSecond.TryGetValue(first, out second);
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00010CE9 File Offset: 0x0000EEE9
		public bool TryGetBySecond(TSecond second, out TFirst first)
		{
			return this._secondToFirst.TryGetValue(second, out first);
		}

		// Token: 0x040008FB RID: 2299
		private readonly IDictionary<TFirst, TSecond> _firstToSecond;

		// Token: 0x040008FC RID: 2300
		private readonly IDictionary<TSecond, TFirst> _secondToFirst;

		// Token: 0x040008FD RID: 2301
		private readonly string _duplicateFirstErrorMessage;

		// Token: 0x040008FE RID: 2302
		private readonly string _duplicateSecondErrorMessage;
	}
}
