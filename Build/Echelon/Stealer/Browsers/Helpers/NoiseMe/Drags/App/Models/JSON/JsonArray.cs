using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Echelon.Stealer.Browsers.Helpers.NoiseMe.Drags.App.Models.JSON
{
	// Token: 0x0200003A RID: 58
	public class JsonArray : JsonValue, IList<JsonValue>, ICollection<JsonValue>, IEnumerable<JsonValue>, IEnumerable
	{
		// Token: 0x06000125 RID: 293 RVA: 0x00008FD5 File Offset: 0x000071D5
		public JsonArray(params JsonValue[] items)
		{
			this.list = new List<JsonValue>();
			this.AddRange(items);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00008FEF File Offset: 0x000071EF
		public JsonArray(IEnumerable<JsonValue> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			this.list = new List<JsonValue>(items);
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00009011 File Offset: 0x00007211
		public override JsonType JsonType
		{
			get
			{
				return JsonType.Array;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00009014 File Offset: 0x00007214
		public override int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00009021 File Offset: 0x00007221
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000018 RID: 24
		public sealed override JsonValue this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				this.list[index] = value;
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00009041 File Offset: 0x00007241
		public void Add(JsonValue item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.list.Add(item);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000905D File Offset: 0x0000725D
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000906A File Offset: 0x0000726A
		public bool Contains(JsonValue item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00009078 File Offset: 0x00007278
		public void CopyTo(JsonValue[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00009087 File Offset: 0x00007287
		public int IndexOf(JsonValue item)
		{
			return this.list.IndexOf(item);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00009095 File Offset: 0x00007295
		public void Insert(int index, JsonValue item)
		{
			this.list.Insert(index, item);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000090A4 File Offset: 0x000072A4
		public bool Remove(JsonValue item)
		{
			return this.list.Remove(item);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000090B2 File Offset: 0x000072B2
		public void RemoveAt(int index)
		{
			this.list.RemoveAt(index);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000090C0 File Offset: 0x000072C0
		IEnumerator<JsonValue> IEnumerable<JsonValue>.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000090C0 File Offset: 0x000072C0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000090D2 File Offset: 0x000072D2
		public void AddRange(IEnumerable<JsonValue> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			this.list.AddRange(items);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000090EE File Offset: 0x000072EE
		public void AddRange(params JsonValue[] items)
		{
			if (items != null)
			{
				this.list.AddRange(items);
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0001F718 File Offset: 0x0001D918
		public override void Save(Stream stream, bool parsing)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			stream.WriteByte(91);
			for (int i = 0; i < this.list.Count; i++)
			{
				JsonValue jsonValue = this.list[i];
				if (jsonValue != null)
				{
					jsonValue.Save(stream, parsing);
				}
				else
				{
					stream.WriteByte(110);
					stream.WriteByte(117);
					stream.WriteByte(108);
					stream.WriteByte(108);
				}
				if (i < this.Count - 1)
				{
					stream.WriteByte(44);
					stream.WriteByte(32);
				}
			}
			stream.WriteByte(93);
		}

		// Token: 0x040000AD RID: 173
		private List<JsonValue> list;
	}
}
