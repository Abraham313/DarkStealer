using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002C1 RID: 705
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	internal class JPropertyKeyedCollection : Collection<JToken>
	{
		// Token: 0x060015AA RID: 5546 RVA: 0x00015F31 File Offset: 0x00014131
		public JPropertyKeyedCollection() : base(new List<JToken>())
		{
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x00015F3E File Offset: 0x0001413E
		private void AddKey(string key, JToken item)
		{
			this.EnsureDictionary();
			this._dictionary[key] = item;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0006C514 File Offset: 0x0006A714
		protected void ChangeItemKey(JToken item, string newKey)
		{
			if (!this.ContainsItem(item))
			{
				throw new ArgumentException("The specified item does not exist in this KeyedCollection.");
			}
			string keyForItem = this.GetKeyForItem(item);
			if (!JPropertyKeyedCollection.Comparer.Equals(keyForItem, newKey))
			{
				if (newKey != null)
				{
					this.AddKey(newKey, item);
				}
				if (keyForItem != null)
				{
					this.RemoveKey(keyForItem);
				}
			}
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00015F53 File Offset: 0x00014153
		protected override void ClearItems()
		{
			base.ClearItems();
			Dictionary<string, JToken> dictionary = this._dictionary;
			if (dictionary == null)
			{
				return;
			}
			dictionary.Clear();
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00015F6B File Offset: 0x0001416B
		public bool Contains(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this._dictionary != null && this._dictionary.ContainsKey(key);
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x0006C560 File Offset: 0x0006A760
		private bool ContainsItem(JToken item)
		{
			if (this._dictionary == null)
			{
				return false;
			}
			string keyForItem = this.GetKeyForItem(item);
			JToken jtoken;
			return this._dictionary.TryGetValue(keyForItem, out jtoken);
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00015F91 File Offset: 0x00014191
		private void EnsureDictionary()
		{
			if (this._dictionary == null)
			{
				this._dictionary = new Dictionary<string, JToken>(JPropertyKeyedCollection.Comparer);
			}
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00015FAB File Offset: 0x000141AB
		private string GetKeyForItem(JToken item)
		{
			return ((JProperty)item).Name;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00015FB8 File Offset: 0x000141B8
		protected override void InsertItem(int index, JToken item)
		{
			this.AddKey(this.GetKeyForItem(item), item);
			base.InsertItem(index, item);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0006C590 File Offset: 0x0006A790
		public bool Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			JToken item;
			return this._dictionary != null && this._dictionary.TryGetValue(key, out item) && base.Remove(item);
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0006C5D0 File Offset: 0x0006A7D0
		protected override void RemoveItem(int index)
		{
			string keyForItem = this.GetKeyForItem(base.Items[index]);
			this.RemoveKey(keyForItem);
			base.RemoveItem(index);
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x00015FD0 File Offset: 0x000141D0
		private void RemoveKey(string key)
		{
			Dictionary<string, JToken> dictionary = this._dictionary;
			if (dictionary == null)
			{
				return;
			}
			dictionary.Remove(key);
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x0006C600 File Offset: 0x0006A800
		protected override void SetItem(int index, JToken item)
		{
			string keyForItem = this.GetKeyForItem(item);
			string keyForItem2 = this.GetKeyForItem(base.Items[index]);
			if (JPropertyKeyedCollection.Comparer.Equals(keyForItem2, keyForItem))
			{
				if (this._dictionary != null)
				{
					this._dictionary[keyForItem] = item;
				}
			}
			else
			{
				this.AddKey(keyForItem, item);
				if (keyForItem2 != null)
				{
					this.RemoveKey(keyForItem2);
				}
			}
			base.SetItem(index, item);
		}

		// Token: 0x1700045A RID: 1114
		public JToken this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				if (this._dictionary == null)
				{
					throw new KeyNotFoundException();
				}
				return this._dictionary[key];
			}
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0001600E File Offset: 0x0001420E
		public bool TryGetValue(string key, [Nullable(2)] [NotNullWhen(true)] out JToken value)
		{
			if (this._dictionary == null)
			{
				value = null;
				return false;
			}
			return this._dictionary.TryGetValue(key, out value);
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060015B9 RID: 5561 RVA: 0x0001602A File Offset: 0x0001422A
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this._dictionary.Keys;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060015BA RID: 5562 RVA: 0x0001603D File Offset: 0x0001423D
		public ICollection<JToken> Values
		{
			get
			{
				this.EnsureDictionary();
				return this._dictionary.Values;
			}
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x00016050 File Offset: 0x00014250
		public int IndexOfReference(JToken t)
		{
			return ((List<JToken>)base.Items).IndexOfReference(t);
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0006C668 File Offset: 0x0006A868
		public bool Compare(JPropertyKeyedCollection other)
		{
			if (this == other)
			{
				return true;
			}
			Dictionary<string, JToken> dictionary = this._dictionary;
			Dictionary<string, JToken> dictionary2 = other._dictionary;
			if (dictionary == null && dictionary2 == null)
			{
				return true;
			}
			if (dictionary == null)
			{
				return dictionary2.Count == 0;
			}
			if (dictionary2 == null)
			{
				return dictionary.Count == 0;
			}
			if (dictionary.Count != dictionary2.Count)
			{
				return false;
			}
			foreach (KeyValuePair<string, JToken> keyValuePair in dictionary)
			{
				JToken jtoken;
				if (!dictionary2.TryGetValue(keyValuePair.Key, out jtoken))
				{
					return false;
				}
				JProperty jproperty = (JProperty)keyValuePair.Value;
				JProperty jproperty2 = (JProperty)jtoken;
				if (jproperty.Value == null)
				{
					return jproperty2.Value == null;
				}
				if (!jproperty.Value.DeepEquals(jproperty2.Value))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000BEA RID: 3050
		private static readonly IEqualityComparer<string> Comparer = StringComparer.Ordinal;

		// Token: 0x04000BEB RID: 3051
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		private Dictionary<string, JToken> _dictionary;
	}
}
