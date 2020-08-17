using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020001EA RID: 490
	[NullableContext(1)]
	[Nullable(0)]
	internal class CollectionWrapper<[Nullable(2)] T> : IEnumerable, ICollection<T>, IEnumerable<T>, IList, ICollection, IWrappedCollection
	{
		// Token: 0x06000E58 RID: 3672 RVA: 0x00056D3C File Offset: 0x00054F3C
		public CollectionWrapper(IList list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			ICollection<T> collection = list as ICollection<T>;
			if (collection != null)
			{
				this._genericCollection = collection;
				return;
			}
			this._list = list;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00010D76 File Offset: 0x0000EF76
		public CollectionWrapper(ICollection<T> list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			this._genericCollection = list;
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00010D90 File Offset: 0x0000EF90
		public virtual void Add(T item)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Add(item);
				return;
			}
			this._list.Add(item);
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00010DB9 File Offset: 0x0000EFB9
		public virtual void Clear()
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Clear();
				return;
			}
			this._list.Clear();
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00010DDA File Offset: 0x0000EFDA
		public virtual bool Contains(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Contains(item);
			}
			return this._list.Contains(item);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00010E02 File Offset: 0x0000F002
		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.CopyTo(array, arrayIndex);
				return;
			}
			this._list.CopyTo(array, arrayIndex);
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x00010E27 File Offset: 0x0000F027
		public virtual int Count
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.Count;
				}
				return this._list.Count;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00010E48 File Offset: 0x0000F048
		public virtual bool IsReadOnly
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.IsReadOnly;
				}
				return this._list.IsReadOnly;
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00010E69 File Offset: 0x0000F069
		public virtual bool Remove(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Remove(item);
			}
			bool flag = this._list.Contains(item);
			if (flag)
			{
				this._list.Remove(item);
			}
			return flag;
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00056D74 File Offset: 0x00054F74
		public virtual IEnumerator<T> GetEnumerator()
		{
			IEnumerable<T> genericCollection = this._genericCollection;
			return (genericCollection ?? this._list.Cast<T>()).GetEnumerator();
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00056DA0 File Offset: 0x00054FA0
		IEnumerator IEnumerable.GetEnumerator()
		{
			IEnumerable genericCollection = this._genericCollection;
			return (genericCollection ?? this._list).GetEnumerator();
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00010EA5 File Offset: 0x0000F0A5
		int IList.Add(object value)
		{
			CollectionWrapper<T>.VerifyValueType(value);
			this.Add((T)((object)value));
			return this.Count - 1;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00010EC1 File Offset: 0x0000F0C1
		bool IList.Contains(object value)
		{
			return CollectionWrapper<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00010ED9 File Offset: 0x0000F0D9
		int IList.IndexOf(object value)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support IndexOf.");
			}
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				return this._list.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00010F0E File Offset: 0x0000F10E
		void IList.RemoveAt(int index)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support RemoveAt.");
			}
			this._list.RemoveAt(index);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00010F2F File Offset: 0x0000F12F
		void IList.Insert(int index, object value)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support Insert.");
			}
			CollectionWrapper<T>.VerifyValueType(value);
			this._list.Insert(index, (T)((object)value));
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x00010F61 File Offset: 0x0000F161
		bool IList.IsFixedSize
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.IsReadOnly;
				}
				return this._list.IsFixedSize;
			}
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00010F82 File Offset: 0x0000F182
		void IList.Remove(object value)
		{
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				this.Remove((T)((object)value));
			}
		}

		// Token: 0x17000301 RID: 769
		object IList.this[int index]
		{
			get
			{
				if (this._genericCollection != null)
				{
					throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
				}
				return this._list[index];
			}
			set
			{
				if (this._genericCollection != null)
				{
					throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
				}
				CollectionWrapper<T>.VerifyValueType(value);
				this._list[index] = (T)((object)value);
			}
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00010FEC File Offset: 0x0000F1EC
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			this.CopyTo((T[])array, arrayIndex);
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x00009021 File Offset: 0x00007221
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x00010FFB File Offset: 0x0000F1FB
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

		// Token: 0x06000E6F RID: 3695 RVA: 0x0001101D File Offset: 0x0000F21D
		private static void VerifyValueType(object value)
		{
			if (!CollectionWrapper<T>.IsCompatibleObject(value))
			{
				throw new ArgumentException("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.".FormatWith(CultureInfo.InvariantCulture, value, typeof(T)), "value");
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0001104C File Offset: 0x0000F24C
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && (!typeof(T).IsValueType() || ReflectionUtils.IsNullableType(typeof(T))));
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0001107E File Offset: 0x0000F27E
		public object UnderlyingCollection
		{
			get
			{
				return this._genericCollection ?? this._list;
			}
		}

		// Token: 0x04000900 RID: 2304
		[Nullable(2)]
		private readonly IList _list;

		// Token: 0x04000901 RID: 2305
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private readonly ICollection<T> _genericCollection;

		// Token: 0x04000902 RID: 2306
		[Nullable(2)]
		private object _syncRoot;
	}
}
