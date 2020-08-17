using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002B8 RID: 696
	[NullableContext(1)]
	[Nullable(0)]
	public class JObject : JContainer, IEnumerable, IDictionary<string, JToken>, ICollection<KeyValuePair<string, JToken>>, IEnumerable<KeyValuePair<string, JToken>>, INotifyPropertyChanged, ICustomTypeDescriptor, INotifyPropertyChanging
	{
		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x00015A01 File Offset: 0x00013C01
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._properties;
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06001520 RID: 5408 RVA: 0x0006BA18 File Offset: 0x00069C18
		// (remove) Token: 0x06001521 RID: 5409 RVA: 0x0006BA50 File Offset: 0x00069C50
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06001522 RID: 5410 RVA: 0x0006BA88 File Offset: 0x00069C88
		// (remove) Token: 0x06001523 RID: 5411 RVA: 0x0006BAC0 File Offset: 0x00069CC0
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event PropertyChangingEventHandler PropertyChanging;

		// Token: 0x06001524 RID: 5412 RVA: 0x00015A09 File Offset: 0x00013C09
		public JObject()
		{
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x00015A1C File Offset: 0x00013C1C
		public JObject(JObject other) : base(other)
		{
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x00015A30 File Offset: 0x00013C30
		public JObject(params object[] content) : this(content)
		{
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x00015A39 File Offset: 0x00013C39
		public JObject(object content)
		{
			this.Add(content);
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x0006BAF8 File Offset: 0x00069CF8
		internal override bool DeepEquals(JToken node)
		{
			JObject jobject = node as JObject;
			return jobject != null && this._properties.Compare(jobject._properties);
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x00015A53 File Offset: 0x00013C53
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._properties.IndexOfReference(item);
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x00015A66 File Offset: 0x00013C66
		[NullableContext(2)]
		internal override void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return;
			}
			base.InsertItem(index, item, skipParentCheck);
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0006BB24 File Offset: 0x00069D24
		internal override void ValidateToken(JToken o, [Nullable(2)] JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type != JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, o.GetType(), base.GetType()));
			}
			JProperty jproperty = (JProperty)o;
			if (existing != null)
			{
				JProperty jproperty2 = (JProperty)existing;
				if (jproperty.Name == jproperty2.Name)
				{
					return;
				}
			}
			if (this._properties.TryGetValue(jproperty.Name, out existing))
			{
				throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith(CultureInfo.InvariantCulture, jproperty.Name, base.GetType()));
			}
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0006BBC4 File Offset: 0x00069DC4
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			JObject jobject = content as JObject;
			if (jobject == null)
			{
				return;
			}
			foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
			{
				JProperty jproperty = this.Property(keyValuePair.Key, (settings != null) ? settings.PropertyNameComparison : StringComparison.Ordinal);
				if (jproperty == null)
				{
					this.Add(keyValuePair.Key, keyValuePair.Value);
				}
				else if (keyValuePair.Value != null)
				{
					JContainer jcontainer = jproperty.Value as JContainer;
					if (jcontainer != null)
					{
						if (jcontainer.Type == keyValuePair.Value.Type)
						{
							jcontainer.Merge(keyValuePair.Value, settings);
							continue;
						}
					}
					if (!JObject.IsNull(keyValuePair.Value) || (settings != null && settings.MergeNullValueHandling == MergeNullValueHandling.Merge))
					{
						jproperty.Value = keyValuePair.Value;
					}
				}
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0006BCB4 File Offset: 0x00069EB4
		private static bool IsNull(JToken token)
		{
			if (token.Type == JTokenType.Null)
			{
				return true;
			}
			JValue jvalue = token as JValue;
			return jvalue != null && jvalue.Value == null;
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0006BCE4 File Offset: 0x00069EE4
		internal void InternalPropertyChanged(JProperty childProperty)
		{
			this.OnPropertyChanged(childProperty.Name);
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.IndexOfItem(childProperty)));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, childProperty, childProperty, this.IndexOfItem(childProperty)));
			}
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x00015A7E File Offset: 0x00013C7E
		internal void InternalPropertyChanging(JProperty childProperty)
		{
			this.OnPropertyChanging(childProperty.Name);
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x00015A8C File Offset: 0x00013C8C
		internal override JToken CloneToken()
		{
			return new JObject(this);
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001531 RID: 5425 RVA: 0x00009F16 File Offset: 0x00008116
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Object;
			}
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x00015A94 File Offset: 0x00013C94
		public IEnumerable<JProperty> Properties()
		{
			return this._properties.Cast<JProperty>();
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x00015AA1 File Offset: 0x00013CA1
		[return: Nullable(2)]
		public JProperty Property(string name)
		{
			return this.Property(name, StringComparison.Ordinal);
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0006BD38 File Offset: 0x00069F38
		[return: Nullable(2)]
		public JProperty Property(string name, StringComparison comparison)
		{
			if (name == null)
			{
				return null;
			}
			JToken jtoken;
			if (this._properties.TryGetValue(name, out jtoken))
			{
				return (JProperty)jtoken;
			}
			if (comparison != StringComparison.Ordinal)
			{
				for (int i = 0; i < this._properties.Count; i++)
				{
					JProperty jproperty = (JProperty)this._properties[i];
					if (string.Equals(jproperty.Name, name, comparison))
					{
						return jproperty;
					}
				}
			}
			return null;
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x00015AAB File Offset: 0x00013CAB
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		public JEnumerable<JToken> PropertyValues()
		{
			return new JEnumerable<JToken>(from p in this.Properties()
			select p.Value);
		}

		// Token: 0x17000446 RID: 1094
		[Nullable(2)]
		public override JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this[text];
			}
			[param: Nullable(2)]
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this[text] = value;
			}
		}

		// Token: 0x17000447 RID: 1095
		[Nullable(2)]
		public JToken this[string propertyName]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(propertyName, "propertyName");
				JProperty jproperty = this.Property(propertyName, StringComparison.Ordinal);
				if (jproperty == null)
				{
					return null;
				}
				return jproperty.Value;
			}
			[param: Nullable(2)]
			set
			{
				JProperty jproperty = this.Property(propertyName, StringComparison.Ordinal);
				if (jproperty != null)
				{
					jproperty.Value = value;
					return;
				}
				this.OnPropertyChanging(propertyName);
				this.Add(propertyName, value);
				this.OnPropertyChanged(propertyName);
			}
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x00015AFC File Offset: 0x00013CFC
		public new static JObject Load(JsonReader reader)
		{
			return JObject.Load(reader, null);
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x0006BE68 File Offset: 0x0006A068
		public new static JObject Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JObject jobject = new JObject();
			jobject.SetLineInfo(reader as IJsonLineInfo, settings);
			jobject.ReadTokenFrom(reader, settings);
			return jobject;
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x00015B05 File Offset: 0x00013D05
		public new static JObject Parse(string json)
		{
			return JObject.Parse(json, null);
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0006BEE8 File Offset: 0x0006A0E8
		public new static JObject Parse(string json, [Nullable(2)] JsonLoadSettings settings)
		{
			JObject result;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JObject jobject = JObject.Load(jsonReader, settings);
				while (jsonReader.Read())
				{
				}
				result = jobject;
			}
			return result;
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x00015B0E File Offset: 0x00013D0E
		public new static JObject FromObject(object o)
		{
			return JObject.FromObject(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0006BF30 File Offset: 0x0006A130
		public new static JObject FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken.Type != JTokenType.Object)
			{
				throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith(CultureInfo.InvariantCulture, jtoken.Type));
			}
			return (JObject)jtoken;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0006BF74 File Offset: 0x0006A174
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartObject();
			for (int i = 0; i < this._properties.Count; i++)
			{
				this._properties[i].WriteTo(writer, converters);
			}
			writer.WriteEndObject();
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x00015B1B File Offset: 0x00013D1B
		[NullableContext(2)]
		public JToken GetValue(string propertyName)
		{
			return this.GetValue(propertyName, StringComparison.Ordinal);
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x00015B25 File Offset: 0x00013D25
		[NullableContext(2)]
		public JToken GetValue(string propertyName, StringComparison comparison)
		{
			if (propertyName == null)
			{
				return null;
			}
			JProperty jproperty = this.Property(propertyName, comparison);
			if (jproperty == null)
			{
				return null;
			}
			return jproperty.Value;
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00015B3F File Offset: 0x00013D3F
		public bool TryGetValue(string propertyName, StringComparison comparison, [Nullable(2)] [NotNullWhen(true)] out JToken value)
		{
			value = this.GetValue(propertyName, comparison);
			return value != null;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00015B50 File Offset: 0x00013D50
		public void Add(string propertyName, [Nullable(2)] JToken value)
		{
			this.Add(new JProperty(propertyName, value));
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00015B5F File Offset: 0x00013D5F
		public bool ContainsKey(string propertyName)
		{
			ValidationUtils.ArgumentNotNull(propertyName, "propertyName");
			return this._properties.Contains(propertyName);
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x00015B78 File Offset: 0x00013D78
		ICollection<string> IDictionary<string, JToken>.Keys
		{
			get
			{
				return this._properties.Keys;
			}
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0006BFB8 File Offset: 0x0006A1B8
		public bool Remove(string propertyName)
		{
			JProperty jproperty = this.Property(propertyName, StringComparison.Ordinal);
			if (jproperty == null)
			{
				return false;
			}
			jproperty.Remove();
			return true;
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0006BFDC File Offset: 0x0006A1DC
		public bool TryGetValue(string propertyName, [Nullable(2)] [NotNullWhen(true)] out JToken value)
		{
			JProperty jproperty = this.Property(propertyName, StringComparison.Ordinal);
			if (jproperty == null)
			{
				value = null;
				return false;
			}
			value = jproperty.Value;
			return true;
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001549 RID: 5449 RVA: 0x0000A639 File Offset: 0x00008839
		[Nullable(new byte[]
		{
			1,
			2
		})]
		ICollection<JToken> IDictionary<string, JToken>.Values
		{
			[return: Nullable(new byte[]
			{
				1,
				2
			})]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x00015B85 File Offset: 0x00013D85
		void ICollection<KeyValuePair<string, JToken>>.Add([Nullable(new byte[]
		{
			0,
			1,
			2
		})] KeyValuePair<string, JToken> item)
		{
			this.Add(new JProperty(item.Key, item.Value));
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x00015BA0 File Offset: 0x00013DA0
		void ICollection<KeyValuePair<string, JToken>>.Clear()
		{
			base.RemoveAll();
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0006C004 File Offset: 0x0006A204
		bool ICollection<KeyValuePair<string, JToken>>.Contains([Nullable(new byte[]
		{
			0,
			1,
			2
		})] KeyValuePair<string, JToken> item)
		{
			JProperty jproperty = this.Property(item.Key, StringComparison.Ordinal);
			return jproperty != null && jproperty.Value == item.Value;
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0006C034 File Offset: 0x0006A234
		void ICollection<KeyValuePair<string, JToken>>.CopyTo([Nullable(new byte[]
		{
			1,
			0,
			1,
			2
		})] KeyValuePair<string, JToken>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length && arrayIndex != 0)
			{
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (base.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JToken jtoken in this._properties)
			{
				JProperty jproperty = (JProperty)jtoken;
				array[arrayIndex + num] = new KeyValuePair<string, JToken>(jproperty.Name, jproperty.Value);
				num++;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x00009021 File Offset: 0x00007221
		bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x00015BA8 File Offset: 0x00013DA8
		bool ICollection<KeyValuePair<string, JToken>>.Remove([Nullable(new byte[]
		{
			0,
			1,
			2
		})] KeyValuePair<string, JToken> item)
		{
			if (!((ICollection<KeyValuePair<string, JToken>>)this).Contains(item))
			{
				return false;
			}
			((IDictionary<string, JToken>)this).Remove(item.Key);
			return true;
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x00015515 File Offset: 0x00013715
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00015BC4 File Offset: 0x00013DC4
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			2
		})]
		public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
		{
			JObject.<GetEnumerator>d__60 <GetEnumerator>d__ = new JObject.<GetEnumerator>d__60(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00015BD3 File Offset: 0x00013DD3
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00015BEC File Offset: 0x00013DEC
		protected virtual void OnPropertyChanging(string propertyName)
		{
			PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
			if (propertyChanging == null)
			{
				return;
			}
			propertyChanging(this, new PropertyChangingEventArgs(propertyName));
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x00015C05 File Offset: 0x00013E05
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x0006C0F0 File Offset: 0x0006A2F0
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			PropertyDescriptor[] array = new PropertyDescriptor[base.Count];
			int num = 0;
			foreach (KeyValuePair<string, JToken> keyValuePair in this)
			{
				array[num] = new JPropertyDescriptor(keyValuePair.Key);
				num++;
			}
			return new PropertyDescriptorCollection(array);
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00015C0E File Offset: 0x00013E0E
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return AttributeCollection.Empty;
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x000158F9 File Offset: 0x00013AF9
		[NullableContext(2)]
		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x000158F9 File Offset: 0x00013AF9
		[NullableContext(2)]
		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00015C15 File Offset: 0x00013E15
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return new TypeConverter();
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x000158F9 File Offset: 0x00013AF9
		[NullableContext(2)]
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x000158F9 File Offset: 0x00013AF9
		[NullableContext(2)]
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x000158F9 File Offset: 0x00013AF9
		[return: Nullable(2)]
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x00015C1C File Offset: 0x00013E1C
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x00015C1C File Offset: 0x00013E1C
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x00015C23 File Offset: 0x00013E23
		[return: Nullable(2)]
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			if (pd is JPropertyDescriptor)
			{
				return this;
			}
			return null;
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x00015C30 File Offset: 0x00013E30
		protected override DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JObject>(parameter, this, new JObject.JObjectDynamicProxy());
		}

		// Token: 0x04000BD9 RID: 3033
		private readonly JPropertyKeyedCollection _properties = new JPropertyKeyedCollection();

		// Token: 0x020002B9 RID: 697
		[Nullable(new byte[]
		{
			0,
			1
		})]
		private class JObjectDynamicProxy : DynamicProxy<JObject>
		{
			// Token: 0x06001561 RID: 5473 RVA: 0x00015C3E File Offset: 0x00013E3E
			public override bool TryGetMember(JObject instance, GetMemberBinder binder, [Nullable(2)] out object result)
			{
				result = instance[binder.Name];
				return true;
			}

			// Token: 0x06001562 RID: 5474 RVA: 0x0006C160 File Offset: 0x0006A360
			public override bool TrySetMember(JObject instance, SetMemberBinder binder, object value)
			{
				JToken jtoken = value as JToken;
				if (jtoken == null)
				{
					jtoken = new JValue(value);
				}
				instance[binder.Name] = jtoken;
				return true;
			}

			// Token: 0x06001563 RID: 5475 RVA: 0x00015C4F File Offset: 0x00013E4F
			public override IEnumerable<string> GetDynamicMemberNames(JObject instance)
			{
				return from p in instance.Properties()
				select p.Name;
			}
		}
	}
}
