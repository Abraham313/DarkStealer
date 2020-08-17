using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002B3 RID: 691
	[NullableContext(1)]
	[Nullable(0)]
	public class JArray : JContainer, IEnumerable, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>
	{
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001479 RID: 5241 RVA: 0x000153B3 File Offset: 0x000135B3
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x00009168 File Offset: 0x00007368
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Array;
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x000153BB File Offset: 0x000135BB
		public JArray()
		{
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x000153CE File Offset: 0x000135CE
		public JArray(JArray other) : base(other)
		{
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x000153E2 File Offset: 0x000135E2
		public JArray(params object[] content) : this(content)
		{
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x000153EB File Offset: 0x000135EB
		public JArray(object content)
		{
			this.Add(content);
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0006A7E0 File Offset: 0x000689E0
		internal override bool DeepEquals(JToken node)
		{
			JArray jarray = node as JArray;
			return jarray != null && base.ContentsEqual(jarray);
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00015405 File Offset: 0x00013605
		internal override JToken CloneToken()
		{
			return new JArray(this);
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0001540D File Offset: 0x0001360D
		public new static JArray Load(JsonReader reader)
		{
			return JArray.Load(reader, null);
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0006A800 File Offset: 0x00068A00
		public new static JArray Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JArray jarray = new JArray();
			jarray.SetLineInfo(reader as IJsonLineInfo, settings);
			jarray.ReadTokenFrom(reader, settings);
			return jarray;
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00015416 File Offset: 0x00013616
		public new static JArray Parse(string json)
		{
			return JArray.Parse(json, null);
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0006A874 File Offset: 0x00068A74
		public new static JArray Parse(string json, [Nullable(2)] JsonLoadSettings settings)
		{
			JArray result;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JArray jarray = JArray.Load(jsonReader, settings);
				while (jsonReader.Read())
				{
				}
				result = jarray;
			}
			return result;
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0001541F File Offset: 0x0001361F
		public new static JArray FromObject(object o)
		{
			return JArray.FromObject(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0006A8BC File Offset: 0x00068ABC
		public new static JArray FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken.Type != JTokenType.Array)
			{
				throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith(CultureInfo.InvariantCulture, jtoken.Type));
			}
			return (JArray)jtoken;
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0006A900 File Offset: 0x00068B00
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartArray();
			for (int i = 0; i < this._values.Count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndArray();
		}

		// Token: 0x17000425 RID: 1061
		[Nullable(2)]
		public override JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this.GetItem((int)key);
			}
			[param: Nullable(2)]
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Set JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this.SetItem((int)key, value);
			}
		}

		// Token: 0x17000426 RID: 1062
		public JToken this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x000154B8 File Offset: 0x000136B8
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._values.IndexOfReference(item);
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0006A944 File Offset: 0x00068B44
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			IEnumerable enumerable = (base.IsMultiContent(content) || content is JArray) ? ((IEnumerable)content) : null;
			if (enumerable == null)
			{
				return;
			}
			JContainer.MergeEnumerableContent(this, enumerable, settings);
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x000154CB File Offset: 0x000136CB
		public int IndexOf(JToken item)
		{
			return this.IndexOfItem(item);
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x000154D4 File Offset: 0x000136D4
		public void Insert(int index, JToken item)
		{
			this.InsertItem(index, item, false);
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x000154DF File Offset: 0x000136DF
		public void RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0006A978 File Offset: 0x00068B78
		public IEnumerator<JToken> GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x000154E8 File Offset: 0x000136E8
		public void Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x000154F1 File Offset: 0x000136F1
		public void Clear()
		{
			this.ClearItems();
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x000154F9 File Offset: 0x000136F9
		public bool Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x00015502 File Offset: 0x00013702
		public void CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x00009021 File Offset: 0x00007221
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0001550C File Offset: 0x0001370C
		public bool Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x00015515 File Offset: 0x00013715
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x04000BC6 RID: 3014
		private readonly List<JToken> _values = new List<JToken>();
	}
}
