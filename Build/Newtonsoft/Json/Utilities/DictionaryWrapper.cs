using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020001F5 RID: 501
	[NullableContext(1)]
	[Nullable(0)]
	internal class DictionaryWrapper<[Nullable(2)] TKey, [Nullable(2)] TValue> : IEnumerable, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, ICollection, IDictionary, IWrappedDictionary
	{
		// Token: 0x06000EB7 RID: 3767 RVA: 0x00011297 File Offset: 0x0000F497
		public DictionaryWrapper(IDictionary dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._dictionary = dictionary;
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x000112B1 File Offset: 0x0000F4B1
		public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._genericDictionary = dictionary;
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x000112CB File Offset: 0x0000F4CB
		internal IDictionary<TKey, TValue> GenericDictionary
		{
			get
			{
				return this._genericDictionary;
			}
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x000112D3 File Offset: 0x0000F4D3
		public void Add(TKey key, TValue value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			if (this._genericDictionary == null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.Add(key, value);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00011310 File Offset: 0x0000F510
		public bool ContainsKey(TKey key)
		{
			if (this._dictionary != null)
			{
				return this._dictionary.Contains(key);
			}
			return this.GenericDictionary.ContainsKey(key);
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x00011338 File Offset: 0x0000F538
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Keys.Cast<TKey>().ToList<TKey>();
				}
				return this.GenericDictionary.Keys;
			}
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00011363 File Offset: 0x0000F563
		public bool Remove(TKey key)
		{
			if (this._dictionary == null)
			{
				return this.GenericDictionary.Remove(key);
			}
			if (this._dictionary.Contains(key))
			{
				this._dictionary.Remove(key);
				return true;
			}
			return false;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00059400 File Offset: 0x00057600
		public bool TryGetValue(TKey key, [MaybeNull] out TValue value)
		{
			if (this._dictionary == null)
			{
				return this.GenericDictionary.TryGetValue(key, out value);
			}
			if (!this._dictionary.Contains(key))
			{
				value = default(TValue);
				return false;
			}
			value = (TValue)((object)this._dictionary[key]);
			return true;
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x000113A1 File Offset: 0x0000F5A1
		public ICollection<TValue> Values
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Values.Cast<TValue>().ToList<TValue>();
				}
				return this.GenericDictionary.Values;
			}
		}

		// Token: 0x1700030B RID: 779
		public TValue this[TKey key]
		{
			get
			{
				if (this._dictionary != null)
				{
					return (TValue)((object)this._dictionary[key]);
				}
				return this.GenericDictionary[key];
			}
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				this.GenericDictionary[key] = value;
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00011428 File Offset: 0x0000F628
		public void Add([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				((IList)this._dictionary).Add(item);
				return;
			}
			IDictionary<TKey, TValue> genericDictionary = this._genericDictionary;
			if (genericDictionary == null)
			{
				return;
			}
			genericDictionary.Add(item);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0001145B File Offset: 0x0000F65B
		public void Clear()
		{
			if (this._dictionary != null)
			{
				this._dictionary.Clear();
				return;
			}
			this.GenericDictionary.Clear();
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0001147C File Offset: 0x0000F67C
		public bool Contains([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				return ((IList)this._dictionary).Contains(item);
			}
			return this.GenericDictionary.Contains(item);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0005945C File Offset: 0x0005765C
		public void CopyTo([Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this._dictionary != null)
			{
				using (IDictionaryEnumerator enumerator = this._dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DictionaryEntry entry = enumerator.Entry;
						array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey)((object)entry.Key), (TValue)((object)entry.Value));
					}
					return;
				}
			}
			this.GenericDictionary.CopyTo(array, arrayIndex);
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x000114A9 File Offset: 0x0000F6A9
		public int Count
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Count;
				}
				return this.GenericDictionary.Count;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x000114CA File Offset: 0x0000F6CA
		public bool IsReadOnly
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.IsReadOnly;
				}
				return this.GenericDictionary.IsReadOnly;
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x000594EC File Offset: 0x000576EC
		public bool Remove([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary == null)
			{
				return this.GenericDictionary.Remove(item);
			}
			if (!this._dictionary.Contains(item.Key))
			{
				return true;
			}
			if (object.Equals(this._dictionary[item.Key], item.Value))
			{
				this._dictionary.Remove(item.Key);
				return true;
			}
			return false;
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00059570 File Offset: 0x00057770
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (this._dictionary != null)
			{
				return (from DictionaryEntry de in this._dictionary
				select new KeyValuePair<TKey, TValue>((TKey)((object)de.Key), (TValue)((object)de.Value))).GetEnumerator();
			}
			return this.GenericDictionary.GetEnumerator();
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x000114EB File Offset: 0x0000F6EB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x000114F3 File Offset: 0x0000F6F3
		void IDictionary.Add(object key, object value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			this.GenericDictionary.Add((TKey)((object)key), (TValue)((object)value));
		}

		// Token: 0x1700030E RID: 782
		[Nullable(2)]
		object IDictionary.this[object key]
		{
			[return: Nullable(2)]
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary[key];
				}
				return this.GenericDictionary[(TKey)((object)key)];
			}
			[param: Nullable(2)]
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				this.GenericDictionary[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0001157E File Offset: 0x0000F77E
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			if (this._dictionary != null)
			{
				return this._dictionary.GetEnumerator();
			}
			return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this.GenericDictionary.GetEnumerator());
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x000115A9 File Offset: 0x0000F7A9
		bool IDictionary.Contains(object key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey((TKey)((object)key));
			}
			return this._dictionary.Contains(key);
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x000115D1 File Offset: 0x0000F7D1
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this._genericDictionary == null && this._dictionary.IsFixedSize;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x000115E8 File Offset: 0x0000F7E8
		ICollection IDictionary.Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Keys.ToList<TKey>();
				}
				return this._dictionary.Keys;
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0001160E File Offset: 0x0000F80E
		public void Remove(object key)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Remove(key);
				return;
			}
			this.GenericDictionary.Remove((TKey)((object)key));
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x00011637 File Offset: 0x0000F837
		ICollection IDictionary.Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Values.ToList<TValue>();
				}
				return this._dictionary.Values;
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0001165D File Offset: 0x0000F85D
		void ICollection.CopyTo(Array array, int index)
		{
			if (this._dictionary != null)
			{
				this._dictionary.CopyTo(array, index);
				return;
			}
			this.GenericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x00011687 File Offset: 0x0000F887
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._dictionary != null && this._dictionary.IsSynchronized;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x0001169E File Offset: 0x0000F89E
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x000116C0 File Offset: 0x0000F8C0
		public object UnderlyingDictionary
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary;
				}
				return this.GenericDictionary;
			}
		}

		// Token: 0x04000967 RID: 2407
		[Nullable(2)]
		private readonly IDictionary _dictionary;

		// Token: 0x04000968 RID: 2408
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		private readonly IDictionary<TKey, TValue> _genericDictionary;

		// Token: 0x04000969 RID: 2409
		[Nullable(2)]
		private object _syncRoot;

		// Token: 0x020001F6 RID: 502
		[Nullable(0)]
		private readonly struct DictionaryEnumerator<[Nullable(2)] TEnumeratorKey, [Nullable(2)] TEnumeratorValue> : IEnumerator, IDictionaryEnumerator
		{
			// Token: 0x06000ED8 RID: 3800 RVA: 0x000116D7 File Offset: 0x0000F8D7
			public DictionaryEnumerator([Nullable(new byte[]
			{
				1,
				0,
				1,
				1
			})] IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				this._e = e;
			}

			// Token: 0x17000315 RID: 789
			// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x000116EB File Offset: 0x0000F8EB
			public DictionaryEntry Entry
			{
				get
				{
					return (DictionaryEntry)this.Current;
				}
			}

			// Token: 0x17000316 RID: 790
			// (get) Token: 0x06000EDA RID: 3802 RVA: 0x000595C8 File Offset: 0x000577C8
			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			// Token: 0x17000317 RID: 791
			// (get) Token: 0x06000EDB RID: 3803 RVA: 0x000595E4 File Offset: 0x000577E4
			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}

			// Token: 0x17000318 RID: 792
			// (get) Token: 0x06000EDC RID: 3804 RVA: 0x00059600 File Offset: 0x00057800
			public object Current
			{
				get
				{
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> keyValuePair = this._e.Current;
					object key = keyValuePair.Key;
					keyValuePair = this._e.Current;
					return new DictionaryEntry(key, keyValuePair.Value);
				}
			}

			// Token: 0x06000EDD RID: 3805 RVA: 0x000116F8 File Offset: 0x0000F8F8
			public bool MoveNext()
			{
				return this._e.MoveNext();
			}

			// Token: 0x06000EDE RID: 3806 RVA: 0x00011705 File Offset: 0x0000F905
			public void Reset()
			{
				this._e.Reset();
			}

			// Token: 0x0400096A RID: 2410
			[Nullable(new byte[]
			{
				1,
				0,
				1,
				1
			})]
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;
		}
	}
}
