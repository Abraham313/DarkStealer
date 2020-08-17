using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002B4 RID: 692
	[NullableContext(1)]
	[Nullable(0)]
	public class JConstructor : JContainer
	{
		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x0001551D File Offset: 0x0001371D
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x00015525 File Offset: 0x00013725
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._values.IndexOfReference(item);
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0006A994 File Offset: 0x00068B94
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			JConstructor jconstructor = content as JConstructor;
			if (jconstructor == null)
			{
				return;
			}
			if (jconstructor.Name != null)
			{
				this.Name = jconstructor.Name;
			}
			JContainer.MergeEnumerableContent(this, jconstructor, settings);
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x00015538 File Offset: 0x00013738
		// (set) Token: 0x0600149D RID: 5277 RVA: 0x00015540 File Offset: 0x00013740
		[Nullable(2)]
		public string Name
		{
			[NullableContext(2)]
			get
			{
				return this._name;
			}
			[NullableContext(2)]
			set
			{
				this._name = value;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x00009011 File Offset: 0x00007211
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Constructor;
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x00015549 File Offset: 0x00013749
		public JConstructor()
		{
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0001555C File Offset: 0x0001375C
		public JConstructor(JConstructor other) : base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0001557C File Offset: 0x0001377C
		public JConstructor(string name, params object[] content) : this(name, content)
		{
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00015586 File Offset: 0x00013786
		public JConstructor(string name, object content) : this(name)
		{
			this.Add(content);
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00015596 File Offset: 0x00013796
		public JConstructor(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Constructor name cannot be empty.", "name");
			}
			this._name = name;
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x0006A9C8 File Offset: 0x00068BC8
		internal override bool DeepEquals(JToken node)
		{
			JConstructor jconstructor = node as JConstructor;
			return jconstructor != null && this._name == jconstructor.Name && base.ContentsEqual(jconstructor);
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x000155D6 File Offset: 0x000137D6
		internal override JToken CloneToken()
		{
			return new JConstructor(this);
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0006A9FC File Offset: 0x00068BFC
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartConstructor(this._name);
			int count = this._values.Count;
			for (int i = 0; i < count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndConstructor();
		}

		// Token: 0x1700042B RID: 1067
		[Nullable(2)]
		public override JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				int index = (int)key;
				return this.GetItem(index);
			}
			[param: Nullable(2)]
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				int index = (int)key;
				this.SetItem(index, value);
			}
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x000155DE File Offset: 0x000137DE
		internal override int GetDeepHashCode()
		{
			string name = this._name;
			return ((name != null) ? name.GetHashCode() : 0) ^ base.ContentsHashCode();
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x000155F9 File Offset: 0x000137F9
		public new static JConstructor Load(JsonReader reader)
		{
			return JConstructor.Load(reader, null);
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0006AAE0 File Offset: 0x00068CE0
		public new static JConstructor Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor jconstructor = new JConstructor((string)reader.Value);
			jconstructor.SetLineInfo(reader as IJsonLineInfo, settings);
			jconstructor.ReadTokenFrom(reader, settings);
			return jconstructor;
		}

		// Token: 0x04000BC7 RID: 3015
		[Nullable(2)]
		private string _name;

		// Token: 0x04000BC8 RID: 3016
		private readonly List<JToken> _values = new List<JToken>();
	}
}
