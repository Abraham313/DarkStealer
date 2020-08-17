using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x020001AD RID: 429
	[NullableContext(1)]
	[Nullable(0)]
	public class DefaultJsonNameTable : JsonNameTable
	{
		// Token: 0x06000B19 RID: 2841 RVA: 0x0000E7AA File Offset: 0x0000C9AA
		public DefaultJsonNameTable()
		{
			this._entries = new DefaultJsonNameTable.Entry[this._mask + 1];
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0004E350 File Offset: 0x0004C550
		[return: Nullable(2)]
		public override string Get(char[] key, int start, int length)
		{
			if (length == 0)
			{
				return string.Empty;
			}
			int num = length + DefaultJsonNameTable.HashCodeRandomizer;
			num += (num << 7 ^ (int)key[start]);
			int num2 = start + length;
			for (int i = start + 1; i < num2; i++)
			{
				num += (num << 7 ^ (int)key[i]);
			}
			num -= num >> 17;
			num -= num >> 11;
			num -= num >> 5;
			int num3 = num & this._mask;
			for (DefaultJsonNameTable.Entry entry = this._entries[num3]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && DefaultJsonNameTable.TextEquals(entry.Value, key, start, length))
				{
					return entry.Value;
				}
			}
			return null;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0004E3F4 File Offset: 0x0004C5F4
		public string Add(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int length = key.Length;
			if (length == 0)
			{
				return string.Empty;
			}
			int num = length + DefaultJsonNameTable.HashCodeRandomizer;
			for (int i = 0; i < key.Length; i++)
			{
				num += (num << 7 ^ (int)key[i]);
			}
			num -= num >> 17;
			num -= num >> 11;
			num -= num >> 5;
			for (DefaultJsonNameTable.Entry entry = this._entries[num & this._mask]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && entry.Value.Equals(key, StringComparison.Ordinal))
				{
					return entry.Value;
				}
			}
			return this.AddEntry(key, num);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0004E4A4 File Offset: 0x0004C6A4
		private string AddEntry(string str, int hashCode)
		{
			int num = hashCode & this._mask;
			DefaultJsonNameTable.Entry entry = new DefaultJsonNameTable.Entry(str, hashCode, this._entries[num]);
			this._entries[num] = entry;
			int count = this._count;
			this._count = count + 1;
			if (count == this._mask)
			{
				this.Grow();
			}
			return entry.Value;
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0004E500 File Offset: 0x0004C700
		private void Grow()
		{
			DefaultJsonNameTable.Entry[] entries = this._entries;
			int num = this._mask * 2 + 1;
			DefaultJsonNameTable.Entry[] array = new DefaultJsonNameTable.Entry[num + 1];
			foreach (DefaultJsonNameTable.Entry entry in entries)
			{
				while (entry != null)
				{
					int num2 = entry.HashCode & num;
					DefaultJsonNameTable.Entry next = entry.Next;
					entry.Next = array[num2];
					array[num2] = entry;
					entry = next;
				}
			}
			this._entries = array;
			this._mask = num;
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0004E584 File Offset: 0x0004C784
		private static bool TextEquals(string str1, char[] str2, int str2Start, int str2Length)
		{
			if (str1.Length != str2Length)
			{
				return false;
			}
			for (int i = 0; i < str1.Length; i++)
			{
				if (str1[i] != str2[str2Start + i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400078F RID: 1935
		private static readonly int HashCodeRandomizer = Environment.TickCount;

		// Token: 0x04000790 RID: 1936
		private int _count;

		// Token: 0x04000791 RID: 1937
		private DefaultJsonNameTable.Entry[] _entries;

		// Token: 0x04000792 RID: 1938
		private int _mask = 31;

		// Token: 0x020001AE RID: 430
		[Nullable(0)]
		private class Entry
		{
			// Token: 0x06000B1F RID: 2847 RVA: 0x0000E7CD File Offset: 0x0000C9CD
			internal Entry(string value, int hashCode, DefaultJsonNameTable.Entry next)
			{
				this.Value = value;
				this.HashCode = hashCode;
				this.Next = next;
			}

			// Token: 0x04000793 RID: 1939
			internal readonly string Value;

			// Token: 0x04000794 RID: 1940
			internal readonly int HashCode;

			// Token: 0x04000795 RID: 1941
			internal DefaultJsonNameTable.Entry Next;
		}
	}
}
