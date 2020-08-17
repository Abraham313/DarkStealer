using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Echelon.Stealer.Browsers.Helpers.NoiseMe.Drags.App.Models.JSON
{
	// Token: 0x0200003C RID: 60
	public class JsonObject : JsonValue, IDictionary<string, JsonValue>, ICollection<KeyValuePair<string, JsonValue>>, IEnumerable<KeyValuePair<string, JsonValue>>, IEnumerable
	{
		// Token: 0x0600013B RID: 315 RVA: 0x00009119 File Offset: 0x00007319
		public JsonObject(params KeyValuePair<string, JsonValue>[] items)
		{
			this.map = new SortedDictionary<string, JsonValue>(StringComparer.Ordinal);
			if (items != null)
			{
				this.AddRange(items);
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000913B File Offset: 0x0000733B
		public JsonObject(IEnumerable<KeyValuePair<string, JsonValue>> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			this.map = new SortedDictionary<string, JsonValue>(StringComparer.Ordinal);
			this.AddRange(items);
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00009168 File Offset: 0x00007368
		public override JsonType JsonType
		{
			get
			{
				return JsonType.Object;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000916B File Offset: 0x0000736B
		public override int Count
		{
			get
			{
				return this.map.Count;
			}
		}

		// Token: 0x1700001B RID: 27
		public sealed override JsonValue this[string key]
		{
			get
			{
				return this.map[key];
			}
			set
			{
				this.map[key] = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00009195 File Offset: 0x00007395
		public ICollection<string> Keys
		{
			get
			{
				return this.map.Keys;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000142 RID: 322 RVA: 0x000091A2 File Offset: 0x000073A2
		public ICollection<JsonValue> Values
		{
			get
			{
				return this.map.Values;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00009021 File Offset: 0x00007221
		bool ICollection<KeyValuePair<string, JsonValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000091AF File Offset: 0x000073AF
		public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator()
		{
			return this.map.GetEnumerator();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000091AF File Offset: 0x000073AF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.map.GetEnumerator();
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000091C1 File Offset: 0x000073C1
		public void Add(string key, JsonValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.map.Add(key, value);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000091DE File Offset: 0x000073DE
		public void Add(KeyValuePair<string, JsonValue> pair)
		{
			this.Add(pair.Key, pair.Value);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000091F4 File Offset: 0x000073F4
		public void Clear()
		{
			this.map.Clear();
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00009201 File Offset: 0x00007401
		bool ICollection<KeyValuePair<string, JsonValue>>.Contains(KeyValuePair<string, JsonValue> item)
		{
			return ((ICollection<KeyValuePair<string, JsonValue>>)this.map).Contains(item);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000920F File Offset: 0x0000740F
		bool ICollection<KeyValuePair<string, JsonValue>>.Remove(KeyValuePair<string, JsonValue> item)
		{
			return ((ICollection<KeyValuePair<string, JsonValue>>)this.map).Remove(item);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000921D File Offset: 0x0000741D
		public override bool ContainsKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.map.ContainsKey(key);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00009239 File Offset: 0x00007439
		public void CopyTo(KeyValuePair<string, JsonValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, JsonValue>>)this.map).CopyTo(array, arrayIndex);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00009248 File Offset: 0x00007448
		public bool Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.map.Remove(key);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00009264 File Offset: 0x00007464
		public bool TryGetValue(string key, out JsonValue value)
		{
			return this.map.TryGetValue(key, out value);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0001F7B0 File Offset: 0x0001D9B0
		public void AddRange(IEnumerable<KeyValuePair<string, JsonValue>> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			foreach (KeyValuePair<string, JsonValue> keyValuePair in items)
			{
				this.map.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00009273 File Offset: 0x00007473
		public void AddRange(params KeyValuePair<string, JsonValue>[] items)
		{
			this.AddRange(items);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0001F81C File Offset: 0x0001DA1C
		public override void Save(Stream stream, bool parsing)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			stream.WriteByte(123);
			foreach (KeyValuePair<string, JsonValue> keyValuePair in this.map)
			{
				stream.WriteByte(34);
				byte[] bytes = Encoding.UTF8.GetBytes(base.EscapeString(keyValuePair.Key));
				stream.Write(bytes, 0, bytes.Length);
				stream.WriteByte(34);
				stream.WriteByte(44);
				stream.WriteByte(32);
				if (keyValuePair.Value == null)
				{
					stream.WriteByte(110);
					stream.WriteByte(117);
					stream.WriteByte(108);
					stream.WriteByte(108);
				}
				else
				{
					keyValuePair.Value.Save(stream, parsing);
				}
			}
			stream.WriteByte(125);
		}

		// Token: 0x040000AE RID: 174
		private SortedDictionary<string, JsonValue> map;
	}
}
