using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002BD RID: 701
	[NullableContext(1)]
	[Nullable(0)]
	public class JProperty : JContainer
	{
		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x00015CEB File Offset: 0x00013EEB
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x00015CF3 File Offset: 0x00013EF3
		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x00015CFB File Offset: 0x00013EFB
		// (set) Token: 0x06001575 RID: 5493 RVA: 0x0006C280 File Offset: 0x0006A480
		public new JToken Value
		{
			[DebuggerStepThrough]
			get
			{
				return this._content._token;
			}
			set
			{
				base.CheckReentrancy();
				JToken item = value ?? JValue.CreateNull();
				if (this._content._token == null)
				{
					this.InsertItem(0, item, false);
					return;
				}
				this.SetItem(0, item);
			}
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x00015D08 File Offset: 0x00013F08
		public JProperty(JProperty other) : base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00015D28 File Offset: 0x00013F28
		internal override JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.Value;
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0006C2C0 File Offset: 0x0006A4C0
		[NullableContext(2)]
		internal override void SetItem(int index, JToken item)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (JContainer.IsTokenUnchanged(this.Value, item))
			{
				return;
			}
			JObject jobject = (JObject)base.Parent;
			if (jobject != null)
			{
				jobject.InternalPropertyChanging(this);
			}
			base.SetItem(0, item);
			JObject jobject2 = (JObject)base.Parent;
			if (jobject2 == null)
			{
				return;
			}
			jobject2.InternalPropertyChanged(this);
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x00015D39 File Offset: 0x00013F39
		[NullableContext(2)]
		internal override bool RemoveItem(JToken item)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x00015D39 File Offset: 0x00013F39
		internal override void RemoveItemAt(int index)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x00015D59 File Offset: 0x00013F59
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._content.IndexOf(item);
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x00015D6C File Offset: 0x00013F6C
		[NullableContext(2)]
		internal override void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return;
			}
			if (this.Value != null)
			{
				throw new JsonException("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
			}
			base.InsertItem(0, item, false);
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00015DAB File Offset: 0x00013FAB
		[NullableContext(2)]
		internal override bool ContainsItem(JToken item)
		{
			return this.Value == item;
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0006C31C File Offset: 0x0006A51C
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			JProperty jproperty = content as JProperty;
			JToken jtoken = (jproperty != null) ? jproperty.Value : null;
			if (jtoken != null && jtoken.Type != JTokenType.Null)
			{
				this.Value = jtoken;
			}
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x00015D39 File Offset: 0x00013F39
		internal override void ClearItems()
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0006C350 File Offset: 0x0006A550
		internal override bool DeepEquals(JToken node)
		{
			JProperty jproperty = node as JProperty;
			return jproperty != null && this._name == jproperty.Name && base.ContentsEqual(jproperty);
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00015DB6 File Offset: 0x00013FB6
		internal override JToken CloneToken()
		{
			return new JProperty(this);
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001582 RID: 5506 RVA: 0x00015DBE File Offset: 0x00013FBE
		public override JTokenType Type
		{
			[DebuggerStepThrough]
			get
			{
				return JTokenType.Property;
			}
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00015DC1 File Offset: 0x00013FC1
		internal JProperty(string name)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00015DE6 File Offset: 0x00013FE6
		public JProperty(string name, params object[] content) : this(name, content)
		{
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0006C384 File Offset: 0x0006A584
		public JProperty(string name, [Nullable(2)] object content)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
			this.Value = (base.IsMultiContent(content) ? new JArray(content) : JContainer.CreateFromContent(content));
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0006C3D4 File Offset: 0x0006A5D4
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WritePropertyName(this._name);
			JToken value = this.Value;
			if (value != null)
			{
				value.WriteTo(writer, converters);
				return;
			}
			writer.WriteNull();
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00015DF0 File Offset: 0x00013FF0
		internal override int GetDeepHashCode()
		{
			int hashCode = this._name.GetHashCode();
			JToken value = this.Value;
			return hashCode ^ ((value != null) ? value.GetDeepHashCode() : 0);
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x00015E10 File Offset: 0x00014010
		public new static JProperty Load(JsonReader reader)
		{
			return JProperty.Load(reader, null);
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0006C408 File Offset: 0x0006A608
		public new static JProperty Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty jproperty = new JProperty((string)reader.Value);
			jproperty.SetLineInfo(reader as IJsonLineInfo, settings);
			jproperty.ReadTokenFrom(reader, settings);
			return jproperty;
		}

		// Token: 0x04000BE4 RID: 3044
		private readonly JProperty.JPropertyList _content = new JProperty.JPropertyList();

		// Token: 0x04000BE5 RID: 3045
		private readonly string _name;

		// Token: 0x020002BE RID: 702
		[Nullable(0)]
		private class JPropertyList : IEnumerable, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>
		{
			// Token: 0x0600158A RID: 5514 RVA: 0x00015E19 File Offset: 0x00014019
			public IEnumerator<JToken> GetEnumerator()
			{
				if (this._token != null)
				{
					yield return this._token;
				}
				yield break;
			}

			// Token: 0x0600158B RID: 5515 RVA: 0x00015E28 File Offset: 0x00014028
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0600158C RID: 5516 RVA: 0x00015E30 File Offset: 0x00014030
			public void Add(JToken item)
			{
				this._token = item;
			}

			// Token: 0x0600158D RID: 5517 RVA: 0x00015E39 File Offset: 0x00014039
			public void Clear()
			{
				this._token = null;
			}

			// Token: 0x0600158E RID: 5518 RVA: 0x00015E42 File Offset: 0x00014042
			public bool Contains(JToken item)
			{
				return this._token == item;
			}

			// Token: 0x0600158F RID: 5519 RVA: 0x00015E4D File Offset: 0x0001404D
			public void CopyTo(JToken[] array, int arrayIndex)
			{
				if (this._token != null)
				{
					array[arrayIndex] = this._token;
				}
			}

			// Token: 0x06001590 RID: 5520 RVA: 0x00015E64 File Offset: 0x00014064
			public bool Remove(JToken item)
			{
				if (this._token == item)
				{
					this._token = null;
					return true;
				}
				return false;
			}

			// Token: 0x17000451 RID: 1105
			// (get) Token: 0x06001591 RID: 5521 RVA: 0x00015E79 File Offset: 0x00014079
			public int Count
			{
				get
				{
					if (this._token == null)
					{
						return 0;
					}
					return 1;
				}
			}

			// Token: 0x17000452 RID: 1106
			// (get) Token: 0x06001592 RID: 5522 RVA: 0x00009021 File Offset: 0x00007221
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06001593 RID: 5523 RVA: 0x00015E86 File Offset: 0x00014086
			public int IndexOf(JToken item)
			{
				if (this._token != item)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x06001594 RID: 5524 RVA: 0x00015E94 File Offset: 0x00014094
			public void Insert(int index, JToken item)
			{
				if (index == 0)
				{
					this._token = item;
				}
			}

			// Token: 0x06001595 RID: 5525 RVA: 0x00015EA0 File Offset: 0x000140A0
			public void RemoveAt(int index)
			{
				if (index == 0)
				{
					this._token = null;
				}
			}

			// Token: 0x17000453 RID: 1107
			public JToken this[int index]
			{
				get
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException();
					}
					return this._token;
				}
				set
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException();
					}
					this._token = value;
				}
			}

			// Token: 0x04000BE6 RID: 3046
			[Nullable(2)]
			internal JToken _token;
		}
	}
}
