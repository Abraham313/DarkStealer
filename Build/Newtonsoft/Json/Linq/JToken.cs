using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq.JsonPath;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002C5 RID: 709
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JToken : IEnumerable, IEnumerable<JToken>, IJEnumerable<JToken>, IDynamicMetaObjectProvider, ICloneable, IJsonLineInfo
	{
		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x0001618F File Offset: 0x0001438F
		public static JTokenEqualityComparer EqualityComparer
		{
			get
			{
				if (JToken._equalityComparer == null)
				{
					JToken._equalityComparer = new JTokenEqualityComparer();
				}
				return JToken._equalityComparer;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060015D1 RID: 5585 RVA: 0x000161A7 File Offset: 0x000143A7
		// (set) Token: 0x060015D2 RID: 5586 RVA: 0x000161AF File Offset: 0x000143AF
		[Nullable(2)]
		public JContainer Parent
		{
			[NullableContext(2)]
			[DebuggerStepThrough]
			get
			{
				return this._parent;
			}
			[NullableContext(2)]
			internal set
			{
				this._parent = value;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060015D3 RID: 5587 RVA: 0x0006C7C0 File Offset: 0x0006A9C0
		public JToken Root
		{
			get
			{
				JContainer parent = this.Parent;
				if (parent == null)
				{
					return this;
				}
				while (parent.Parent != null)
				{
					parent = parent.Parent;
				}
				return parent;
			}
		}

		// Token: 0x060015D4 RID: 5588
		internal abstract JToken CloneToken();

		// Token: 0x060015D5 RID: 5589
		internal abstract bool DeepEquals(JToken node);

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060015D6 RID: 5590
		public abstract JTokenType Type { get; }

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060015D7 RID: 5591
		public abstract bool HasValues { get; }

		// Token: 0x060015D8 RID: 5592 RVA: 0x000161B8 File Offset: 0x000143B8
		[NullableContext(2)]
		public static bool DeepEquals(JToken t1, JToken t2)
		{
			return t1 == t2 || (t1 != null && t2 != null && t1.DeepEquals(t2));
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060015D9 RID: 5593 RVA: 0x000161CF File Offset: 0x000143CF
		// (set) Token: 0x060015DA RID: 5594 RVA: 0x000161D7 File Offset: 0x000143D7
		[Nullable(2)]
		public JToken Next
		{
			[NullableContext(2)]
			get
			{
				return this._next;
			}
			[NullableContext(2)]
			internal set
			{
				this._next = value;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060015DB RID: 5595 RVA: 0x000161E0 File Offset: 0x000143E0
		// (set) Token: 0x060015DC RID: 5596 RVA: 0x000161E8 File Offset: 0x000143E8
		[Nullable(2)]
		public JToken Previous
		{
			[NullableContext(2)]
			get
			{
				return this._previous;
			}
			[NullableContext(2)]
			internal set
			{
				this._previous = value;
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060015DD RID: 5597 RVA: 0x0006C7EC File Offset: 0x0006A9EC
		public string Path
		{
			get
			{
				if (this.Parent == null)
				{
					return string.Empty;
				}
				List<JsonPosition> list = new List<JsonPosition>();
				JToken jtoken = null;
				for (JToken jtoken2 = this; jtoken2 != null; jtoken2 = jtoken2.Parent)
				{
					JTokenType type = jtoken2.Type;
					if (type - JTokenType.Array > 1)
					{
						if (type == JTokenType.Property)
						{
							JProperty jproperty = (JProperty)jtoken2;
							List<JsonPosition> list2 = list;
							JsonPosition item = new JsonPosition(JsonContainerType.Object)
							{
								PropertyName = jproperty.Name
							};
							list2.Add(item);
						}
					}
					else if (jtoken != null)
					{
						int position = ((IList<JToken>)jtoken2).IndexOf(jtoken);
						List<JsonPosition> list3 = list;
						JsonPosition item = new JsonPosition(JsonContainerType.Array)
						{
							Position = position
						};
						list3.Add(item);
					}
					jtoken = jtoken2;
				}
				list.FastReverse<JsonPosition>();
				return JsonPosition.BuildPath(list, null);
			}
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x00008754 File Offset: 0x00006954
		internal JToken()
		{
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x0006C898 File Offset: 0x0006AA98
		[NullableContext(2)]
		public void AddAfterSelf(object content)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			int num = this._parent.IndexOfItem(this);
			this._parent.AddInternal(num + 1, content, false);
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x0006C8D8 File Offset: 0x0006AAD8
		[NullableContext(2)]
		public void AddBeforeSelf(object content)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			int index = this._parent.IndexOfItem(this);
			this._parent.AddInternal(index, content, false);
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x000161F1 File Offset: 0x000143F1
		public IEnumerable<JToken> Ancestors()
		{
			return this.GetAncestors(false);
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x000161FA File Offset: 0x000143FA
		public IEnumerable<JToken> AncestorsAndSelf()
		{
			return this.GetAncestors(true);
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00016203 File Offset: 0x00014403
		internal IEnumerable<JToken> GetAncestors(bool self)
		{
			JToken.<GetAncestors>d__42 <GetAncestors>d__ = new JToken.<GetAncestors>d__42(-2);
			<GetAncestors>d__.<>4__this = this;
			<GetAncestors>d__.<>3__self = self;
			return <GetAncestors>d__;
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x0001621A File Offset: 0x0001441A
		public IEnumerable<JToken> AfterSelf()
		{
			JToken.<AfterSelf>d__43 <AfterSelf>d__ = new JToken.<AfterSelf>d__43(-2);
			<AfterSelf>d__.<>4__this = this;
			return <AfterSelf>d__;
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0001622A File Offset: 0x0001442A
		public IEnumerable<JToken> BeforeSelf()
		{
			if (this.Parent == null)
			{
				yield break;
			}
			JToken o = this.Parent.First;
			while (o != this && o != null)
			{
				yield return o;
				o = o.Next;
			}
			o = null;
			yield break;
		}

		// Token: 0x1700046B RID: 1131
		[Nullable(2)]
		public virtual JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
			[param: Nullable(2)]
			set
			{
				throw new InvalidOperationException("Cannot set child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0006C914 File Offset: 0x0006AB14
		public virtual T Value<[Nullable(2)] T>(object key)
		{
			JToken jtoken = this[key];
			if (jtoken != null)
			{
				return jtoken.Convert<JToken, T>();
			}
			return default(T);
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x060015E9 RID: 5609 RVA: 0x0001623A File Offset: 0x0001443A
		[Nullable(2)]
		public virtual JToken First
		{
			[NullableContext(2)]
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x0001623A File Offset: 0x0001443A
		[Nullable(2)]
		public virtual JToken Last
		{
			[NullableContext(2)]
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x00016272 File Offset: 0x00014472
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		public virtual JEnumerable<JToken> Children()
		{
			return JEnumerable<JToken>.Empty;
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00016279 File Offset: 0x00014479
		[NullableContext(0)]
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		public JEnumerable<T> Children<T>() where T : JToken
		{
			return new JEnumerable<T>(this.Children().OfType<T>());
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0001623A File Offset: 0x0001443A
		public virtual IEnumerable<T> Values<[Nullable(2)] T>()
		{
			throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00016290 File Offset: 0x00014490
		public void Remove()
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			this._parent.RemoveItem(this);
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x000162B2 File Offset: 0x000144B2
		public void Replace(JToken value)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			this._parent.ReplaceItem(this, value);
		}

		// Token: 0x060015F0 RID: 5616
		public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

		// Token: 0x060015F1 RID: 5617 RVA: 0x000162D4 File Offset: 0x000144D4
		public override string ToString()
		{
			return this.ToString(Formatting.Indented, new JsonConverter[0]);
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x0006C93C File Offset: 0x0006AB3C
		public string ToString(Formatting formatting, params JsonConverter[] converters)
		{
			string result;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				this.WriteTo(new JsonTextWriter(stringWriter)
				{
					Formatting = formatting
				}, converters);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0006C990 File Offset: 0x0006AB90
		[return: Nullable(2)]
		private static JValue EnsureValue(JToken value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			JProperty jproperty = value as JProperty;
			if (jproperty != null)
			{
				value = jproperty.Value;
			}
			return value as JValue;
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x0006C9C4 File Offset: 0x0006ABC4
		private static string GetType(JToken token)
		{
			ValidationUtils.ArgumentNotNull(token, "token");
			JProperty jproperty = token as JProperty;
			if (jproperty != null)
			{
				token = jproperty.Value;
			}
			return token.Type.ToString();
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x000162E3 File Offset: 0x000144E3
		private static bool ValidateToken(JToken o, JTokenType[] validTypes, bool nullable)
		{
			return Array.IndexOf<JTokenType>(validTypes, o.Type) != -1 || (nullable && (o.Type == JTokenType.Null || o.Type == JTokenType.Undefined));
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0006CA04 File Offset: 0x0006AC04
		public static explicit operator bool(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BooleanTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return Convert.ToBoolean((int)value3);
			}
			return Convert.ToBoolean(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x0006CA78 File Offset: 0x0006AC78
		public static explicit operator DateTimeOffset(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is DateTimeOffset)
			{
				return (DateTimeOffset)value2;
			}
			string text = jvalue.Value as string;
			if (text != null)
			{
				return DateTimeOffset.Parse(text, CultureInfo.InvariantCulture);
			}
			return new DateTimeOffset(Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0006CB00 File Offset: 0x0006AD00
		[NullableContext(2)]
		public static explicit operator bool?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BooleanTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new bool?(Convert.ToBoolean((int)value3));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new bool?(Convert.ToBoolean(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0006CB9C File Offset: 0x0006AD9C
		public static explicit operator long(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (long)value3;
			}
			return Convert.ToInt64(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x0006CC0C File Offset: 0x0006AE0C
		[NullableContext(2)]
		public static explicit operator DateTime?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is DateTimeOffset)
			{
				return new DateTime?(((DateTimeOffset)value2).DateTime);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new DateTime?(Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0006CCA4 File Offset: 0x0006AEA4
		[NullableContext(2)]
		public static explicit operator DateTimeOffset?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			object value2 = jvalue.Value;
			if (value2 is DateTimeOffset)
			{
				DateTimeOffset value3 = (DateTimeOffset)value2;
				return new DateTimeOffset?(value3);
			}
			string text = jvalue.Value as string;
			if (text != null)
			{
				return new DateTimeOffset?(DateTimeOffset.Parse(text, CultureInfo.InvariantCulture));
			}
			return new DateTimeOffset?(new DateTimeOffset(Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture)));
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x0006CD60 File Offset: 0x0006AF60
		[NullableContext(2)]
		public static explicit operator decimal?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new decimal?((decimal)value3);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new decimal?(Convert.ToDecimal(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0006CDF8 File Offset: 0x0006AFF8
		[NullableContext(2)]
		public static explicit operator double?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new double?((double)value3);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new double?(Convert.ToDouble(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x0006CE90 File Offset: 0x0006B090
		[NullableContext(2)]
		public static explicit operator char?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.CharTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Char.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new char?((char)((ushort)value3));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new char?(Convert.ToChar(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0006CF28 File Offset: 0x0006B128
		public static explicit operator int(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (int)value3;
			}
			return Convert.ToInt32(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0006CF98 File Offset: 0x0006B198
		public static explicit operator short(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (short)value3;
			}
			return Convert.ToInt16(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0006D008 File Offset: 0x0006B208
		[CLSCompliant(false)]
		public static explicit operator ushort(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (ushort)value3;
			}
			return Convert.ToUInt16(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0006D078 File Offset: 0x0006B278
		[CLSCompliant(false)]
		public static explicit operator char(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.CharTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Char.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (char)((ushort)value3);
			}
			return Convert.ToChar(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0006D0E8 File Offset: 0x0006B2E8
		public static explicit operator byte(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Byte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (byte)value3;
			}
			return Convert.ToByte(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x0006D158 File Offset: 0x0006B358
		[CLSCompliant(false)]
		public static explicit operator sbyte(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to SByte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (sbyte)value3;
			}
			return Convert.ToSByte(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0006D1C8 File Offset: 0x0006B3C8
		[NullableContext(2)]
		public static explicit operator int?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new int?((int)value3);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new int?(Convert.ToInt32(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0006D260 File Offset: 0x0006B460
		[NullableContext(2)]
		public static explicit operator short?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new short?((short)value3);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new short?(Convert.ToInt16(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0006D2F8 File Offset: 0x0006B4F8
		[NullableContext(2)]
		[CLSCompliant(false)]
		public static explicit operator ushort?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new ushort?((ushort)value3);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new ushort?(Convert.ToUInt16(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x0006D390 File Offset: 0x0006B590
		[NullableContext(2)]
		public static explicit operator byte?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Byte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new byte?((byte)value3);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new byte?(Convert.ToByte(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x0006D428 File Offset: 0x0006B628
		[NullableContext(2)]
		[CLSCompliant(false)]
		public static explicit operator sbyte?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to SByte.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new sbyte?((sbyte)value3);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new sbyte?(Convert.ToSByte(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x0006D4C0 File Offset: 0x0006B6C0
		public static explicit operator DateTime(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.DateTimeTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is DateTimeOffset)
			{
				return ((DateTimeOffset)value2).DateTime;
			}
			return Convert.ToDateTime(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x0006D530 File Offset: 0x0006B730
		[NullableContext(2)]
		public static explicit operator long?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new long?((long)value3);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new long?(Convert.ToInt64(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x0006D5C8 File Offset: 0x0006B7C8
		[NullableContext(2)]
		public static explicit operator float?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new float?((float)value3);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new float?(Convert.ToSingle(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x0006D660 File Offset: 0x0006B860
		public static explicit operator decimal(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (decimal)value3;
			}
			return Convert.ToDecimal(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x0006D6D0 File Offset: 0x0006B8D0
		[NullableContext(2)]
		[CLSCompliant(false)]
		public static explicit operator uint?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new uint?((uint)value3);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new uint?(Convert.ToUInt32(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x0006D768 File Offset: 0x0006B968
		[NullableContext(2)]
		[CLSCompliant(false)]
		public static explicit operator ulong?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return new ulong?((ulong)value3);
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new ulong?(Convert.ToUInt64(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x0006D800 File Offset: 0x0006BA00
		public static explicit operator double(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (double)value3;
			}
			return Convert.ToDouble(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x0006D870 File Offset: 0x0006BA70
		public static explicit operator float(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (float)value3;
			}
			return Convert.ToSingle(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x0006D8E0 File Offset: 0x0006BAE0
		[NullableContext(2)]
		public static explicit operator string(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.StringTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to String.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			byte[] array = jvalue.Value as byte[];
			if (array != null)
			{
				return Convert.ToBase64String(array);
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				return ((System.Numerics.BigInteger)value2).ToString(CultureInfo.InvariantCulture);
			}
			return Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x0006D978 File Offset: 0x0006BB78
		[CLSCompliant(false)]
		public static explicit operator uint(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (uint)value3;
			}
			return Convert.ToUInt32(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x0006D9E8 File Offset: 0x0006BBE8
		[CLSCompliant(false)]
		public static explicit operator ulong(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.NumberTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger value3 = (System.Numerics.BigInteger)value2;
				return (ulong)value3;
			}
			return Convert.ToUInt64(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x0006DA58 File Offset: 0x0006BC58
		[NullableContext(2)]
		public static explicit operator byte[](JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BytesTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value is string)
			{
				return Convert.FromBase64String(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			object value2 = jvalue.Value;
			if (value2 is System.Numerics.BigInteger)
			{
				return ((System.Numerics.BigInteger)value2).ToByteArray();
			}
			byte[] array = jvalue.Value as byte[];
			if (array == null)
			{
				throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			return array;
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x0006DB0C File Offset: 0x0006BD0C
		public static explicit operator Guid(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.GuidTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to Guid.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			byte[] array = jvalue.Value as byte[];
			if (array != null)
			{
				return new Guid(array);
			}
			object value2 = jvalue.Value;
			if (value2 is Guid)
			{
				return (Guid)value2;
			}
			return new Guid(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x0006DB90 File Offset: 0x0006BD90
		[NullableContext(2)]
		public static explicit operator Guid?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.GuidTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Guid.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			byte[] array = jvalue.Value as byte[];
			if (array != null)
			{
				return new Guid?(new Guid(array));
			}
			object value2 = jvalue.Value;
			Guid value3;
			if (value2 is Guid)
			{
				Guid guid = (Guid)value2;
				value3 = guid;
			}
			else
			{
				value3 = new Guid(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			return new Guid?(value3);
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x0006DC44 File Offset: 0x0006BE44
		public static explicit operator TimeSpan(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.TimeSpanTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			object value2 = jvalue.Value;
			if (value2 is TimeSpan)
			{
				return (TimeSpan)value2;
			}
			return ConvertUtils.ParseTimeSpan(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x0006DCB4 File Offset: 0x0006BEB4
		[NullableContext(2)]
		public static explicit operator TimeSpan?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.TimeSpanTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			object value2 = jvalue.Value;
			TimeSpan value3;
			if (value2 is TimeSpan)
			{
				TimeSpan timeSpan = (TimeSpan)value2;
				value3 = timeSpan;
			}
			else
			{
				value3 = ConvertUtils.ParseTimeSpan(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			return new TimeSpan?(value3);
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x0006DD48 File Offset: 0x0006BF48
		[NullableContext(2)]
		public static explicit operator Uri(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.UriTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to Uri.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			Uri uri = jvalue.Value as Uri;
			if (uri == null)
			{
				return new Uri(Convert.ToString(jvalue.Value, CultureInfo.InvariantCulture));
			}
			return uri;
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x0006DDC0 File Offset: 0x0006BFC0
		private static System.Numerics.BigInteger ToBigInteger(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BigIntegerTypes, false))
			{
				throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			return ConvertUtils.ToBigInteger(jvalue.Value);
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x0006DE0C File Offset: 0x0006C00C
		private static System.Numerics.BigInteger? ToBigIntegerNullable(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateToken(jvalue, JToken.BigIntegerTypes, true))
			{
				throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, JToken.GetType(value)));
			}
			if (jvalue.Value == null)
			{
				return null;
			}
			return new System.Numerics.BigInteger?(ConvertUtils.ToBigInteger(jvalue.Value));
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x00016311 File Offset: 0x00014511
		public static implicit operator JToken(bool value)
		{
			return new JValue(value);
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x00016319 File Offset: 0x00014519
		public static implicit operator JToken(DateTimeOffset value)
		{
			return new JValue(value);
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00016321 File Offset: 0x00014521
		public static implicit operator JToken(byte value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x0001632A File Offset: 0x0001452A
		public static implicit operator JToken(byte? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00016337 File Offset: 0x00014537
		[CLSCompliant(false)]
		public static implicit operator JToken(sbyte value)
		{
			return new JValue((long)value);
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00016340 File Offset: 0x00014540
		[CLSCompliant(false)]
		public static implicit operator JToken(sbyte? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x0001634D File Offset: 0x0001454D
		public static implicit operator JToken(bool? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0001635A File Offset: 0x0001455A
		public static implicit operator JToken(long value)
		{
			return new JValue(value);
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00016362 File Offset: 0x00014562
		public static implicit operator JToken(DateTime? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0001636F File Offset: 0x0001456F
		public static implicit operator JToken(DateTimeOffset? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0001637C File Offset: 0x0001457C
		public static implicit operator JToken(decimal? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00016389 File Offset: 0x00014589
		public static implicit operator JToken(double? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00016337 File Offset: 0x00014537
		[CLSCompliant(false)]
		public static implicit operator JToken(short value)
		{
			return new JValue((long)value);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00016321 File Offset: 0x00014521
		[CLSCompliant(false)]
		public static implicit operator JToken(ushort value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00016337 File Offset: 0x00014537
		public static implicit operator JToken(int value)
		{
			return new JValue((long)value);
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00016396 File Offset: 0x00014596
		public static implicit operator JToken(int? value)
		{
			return new JValue(value);
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x000163A3 File Offset: 0x000145A3
		public static implicit operator JToken(DateTime value)
		{
			return new JValue(value);
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x000163AB File Offset: 0x000145AB
		public static implicit operator JToken(long? value)
		{
			return new JValue(value);
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x000163B8 File Offset: 0x000145B8
		public static implicit operator JToken(float? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x000163C5 File Offset: 0x000145C5
		public static implicit operator JToken(decimal value)
		{
			return new JValue(value);
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x000163CD File Offset: 0x000145CD
		[CLSCompliant(false)]
		public static implicit operator JToken(short? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x000163DA File Offset: 0x000145DA
		[CLSCompliant(false)]
		public static implicit operator JToken(ushort? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x000163E7 File Offset: 0x000145E7
		[CLSCompliant(false)]
		public static implicit operator JToken(uint? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x000163F4 File Offset: 0x000145F4
		[CLSCompliant(false)]
		public static implicit operator JToken(ulong? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00016401 File Offset: 0x00014601
		public static implicit operator JToken(double value)
		{
			return new JValue(value);
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00016409 File Offset: 0x00014609
		public static implicit operator JToken(float value)
		{
			return new JValue(value);
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x00016411 File Offset: 0x00014611
		public static implicit operator JToken([Nullable(2)] string value)
		{
			return new JValue(value);
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x00016321 File Offset: 0x00014521
		[CLSCompliant(false)]
		public static implicit operator JToken(uint value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00016419 File Offset: 0x00014619
		[CLSCompliant(false)]
		public static implicit operator JToken(ulong value)
		{
			return new JValue(value);
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00016421 File Offset: 0x00014621
		public static implicit operator JToken(byte[] value)
		{
			return new JValue(value);
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x00016429 File Offset: 0x00014629
		public static implicit operator JToken([Nullable(2)] Uri value)
		{
			return new JValue(value);
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x00016431 File Offset: 0x00014631
		public static implicit operator JToken(TimeSpan value)
		{
			return new JValue(value);
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x00016439 File Offset: 0x00014639
		public static implicit operator JToken(TimeSpan? value)
		{
			return new JValue(value);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x00016446 File Offset: 0x00014646
		public static implicit operator JToken(Guid value)
		{
			return new JValue(value);
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x0001644E File Offset: 0x0001464E
		public static implicit operator JToken(Guid? value)
		{
			return new JValue(value);
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x0001645B File Offset: 0x0001465B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<JToken>)this).GetEnumerator();
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x0006A978 File Offset: 0x00068B78
		IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		// Token: 0x06001642 RID: 5698
		internal abstract int GetDeepHashCode();

		// Token: 0x1700046E RID: 1134
		IJEnumerable<JToken> IJEnumerable<JToken>.this[object key]
		{
			get
			{
				return this[key];
			}
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x0001646C File Offset: 0x0001466C
		public JsonReader CreateReader()
		{
			return new JTokenReader(this);
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x0006DE70 File Offset: 0x0006C070
		internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			JToken token;
			using (JTokenWriter jtokenWriter = new JTokenWriter())
			{
				jsonSerializer.Serialize(jtokenWriter, o);
				token = jtokenWriter.Token;
			}
			return token;
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00016474 File Offset: 0x00014674
		public static JToken FromObject(object o)
		{
			return JToken.FromObjectInternal(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x00016481 File Offset: 0x00014681
		public static JToken FromObject(object o, JsonSerializer jsonSerializer)
		{
			return JToken.FromObjectInternal(o, jsonSerializer);
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x0001648A File Offset: 0x0001468A
		[return: MaybeNull]
		public T ToObject<[Nullable(2)] T>()
		{
			return (T)((object)this.ToObject(typeof(T)));
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0006DEC8 File Offset: 0x0006C0C8
		[return: Nullable(2)]
		public object ToObject(Type objectType)
		{
			if (JsonConvert.DefaultSettings == null)
			{
				bool flag;
				PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(objectType, out flag);
				if (flag)
				{
					if (this.Type == JTokenType.String)
					{
						object result;
						try
						{
							result = this.ToObject(objectType, JsonSerializer.CreateDefault());
						}
						catch (Exception innerException)
						{
							Type type = objectType.IsEnum() ? objectType : Nullable.GetUnderlyingType(objectType);
							throw new ArgumentException("Could not convert '{0}' to {1}.".FormatWith(CultureInfo.InvariantCulture, (string)this, type.Name), innerException);
						}
						return result;
					}
					if (this.Type == JTokenType.Integer)
					{
						return Enum.ToObject(objectType.IsEnum() ? objectType : Nullable.GetUnderlyingType(objectType), ((JValue)this).Value);
					}
				}
				switch (typeCode)
				{
				case PrimitiveTypeCode.Char:
					return (char)this;
				case PrimitiveTypeCode.CharNullable:
					return (char?)this;
				case PrimitiveTypeCode.Boolean:
					return (bool)this;
				case PrimitiveTypeCode.BooleanNullable:
					return (bool?)this;
				case PrimitiveTypeCode.SByte:
					return (sbyte)this;
				case PrimitiveTypeCode.SByteNullable:
					return (sbyte?)this;
				case PrimitiveTypeCode.Int16:
					return (short)this;
				case PrimitiveTypeCode.Int16Nullable:
					return (short?)this;
				case PrimitiveTypeCode.UInt16:
					return (ushort)this;
				case PrimitiveTypeCode.UInt16Nullable:
					return (ushort?)this;
				case PrimitiveTypeCode.Int32:
					return (int)this;
				case PrimitiveTypeCode.Int32Nullable:
					return (int?)this;
				case PrimitiveTypeCode.Byte:
					return (byte)this;
				case PrimitiveTypeCode.ByteNullable:
					return (byte?)this;
				case PrimitiveTypeCode.UInt32:
					return (uint)this;
				case PrimitiveTypeCode.UInt32Nullable:
					return (uint?)this;
				case PrimitiveTypeCode.Int64:
					return (long)this;
				case PrimitiveTypeCode.Int64Nullable:
					return (long?)this;
				case PrimitiveTypeCode.UInt64:
					return (ulong)this;
				case PrimitiveTypeCode.UInt64Nullable:
					return (ulong?)this;
				case PrimitiveTypeCode.Single:
					return (float)this;
				case PrimitiveTypeCode.SingleNullable:
					return (float?)this;
				case PrimitiveTypeCode.Double:
					return (double)this;
				case PrimitiveTypeCode.DoubleNullable:
					return (double?)this;
				case PrimitiveTypeCode.DateTime:
					return (DateTime)this;
				case PrimitiveTypeCode.DateTimeNullable:
					return (DateTime?)this;
				case PrimitiveTypeCode.DateTimeOffset:
					return (DateTimeOffset)this;
				case PrimitiveTypeCode.DateTimeOffsetNullable:
					return (DateTimeOffset?)this;
				case PrimitiveTypeCode.Decimal:
					return (decimal)this;
				case PrimitiveTypeCode.DecimalNullable:
					return (decimal?)this;
				case PrimitiveTypeCode.Guid:
					return (Guid)this;
				case PrimitiveTypeCode.GuidNullable:
					return (Guid?)this;
				case PrimitiveTypeCode.TimeSpan:
					return (TimeSpan)this;
				case PrimitiveTypeCode.TimeSpanNullable:
					return (TimeSpan?)this;
				case PrimitiveTypeCode.BigInteger:
					return JToken.ToBigInteger(this);
				case PrimitiveTypeCode.BigIntegerNullable:
					return JToken.ToBigIntegerNullable(this);
				case PrimitiveTypeCode.Uri:
					return (Uri)this;
				case PrimitiveTypeCode.String:
					return (string)this;
				}
			}
			return this.ToObject(objectType, JsonSerializer.CreateDefault());
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x000164A1 File Offset: 0x000146A1
		[return: MaybeNull]
		public T ToObject<[Nullable(2)] T>(JsonSerializer jsonSerializer)
		{
			return (T)((object)this.ToObject(typeof(T), jsonSerializer));
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0006E1E8 File Offset: 0x0006C3E8
		[return: Nullable(2)]
		public object ToObject(Type objectType, JsonSerializer jsonSerializer)
		{
			ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			object result;
			using (JTokenReader jtokenReader = new JTokenReader(this))
			{
				result = jsonSerializer.Deserialize(jtokenReader, objectType);
			}
			return result;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x000164B9 File Offset: 0x000146B9
		public static JToken ReadFrom(JsonReader reader)
		{
			return JToken.ReadFrom(reader, null);
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x0006E230 File Offset: 0x0006C430
		public static JToken ReadFrom(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			bool flag;
			if (reader.TokenType == JsonToken.None)
			{
				flag = ((settings == null || settings.CommentHandling != CommentHandling.Ignore) ? reader.Read() : reader.ReadAndMoveToContent());
			}
			else
			{
				flag = (reader.TokenType != JsonToken.Comment || settings == null || settings.CommentHandling != CommentHandling.Ignore || reader.ReadAndMoveToContent());
			}
			if (!flag)
			{
				throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader.");
			}
			IJsonLineInfo lineInfo = reader as IJsonLineInfo;
			switch (reader.TokenType)
			{
			case JsonToken.StartObject:
				return JObject.Load(reader, settings);
			case JsonToken.StartArray:
				return JArray.Load(reader, settings);
			case JsonToken.StartConstructor:
				return JConstructor.Load(reader, settings);
			case JsonToken.PropertyName:
				return JProperty.Load(reader, settings);
			case JsonToken.Comment:
			{
				JValue jvalue = JValue.CreateComment(reader.Value.ToString());
				jvalue.SetLineInfo(lineInfo, settings);
				return jvalue;
			}
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Date:
			case JsonToken.Bytes:
			{
				JValue jvalue2 = new JValue(reader.Value);
				jvalue2.SetLineInfo(lineInfo, settings);
				return jvalue2;
			}
			case JsonToken.Null:
			{
				JValue jvalue3 = JValue.CreateNull();
				jvalue3.SetLineInfo(lineInfo, settings);
				return jvalue3;
			}
			case JsonToken.Undefined:
			{
				JValue jvalue4 = JValue.CreateUndefined();
				jvalue4.SetLineInfo(lineInfo, settings);
				return jvalue4;
			}
			}
			throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x000164C2 File Offset: 0x000146C2
		public static JToken Parse(string json)
		{
			return JToken.Parse(json, null);
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x0006E380 File Offset: 0x0006C580
		public static JToken Parse(string json, [Nullable(2)] JsonLoadSettings settings)
		{
			JToken result;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JToken jtoken = JToken.Load(jsonReader, settings);
				while (jsonReader.Read())
				{
				}
				result = jtoken;
			}
			return result;
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x000164CB File Offset: 0x000146CB
		public static JToken Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			return JToken.ReadFrom(reader, settings);
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x000164D4 File Offset: 0x000146D4
		public static JToken Load(JsonReader reader)
		{
			return JToken.Load(reader, null);
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x000164DD File Offset: 0x000146DD
		[NullableContext(2)]
		internal void SetLineInfo(IJsonLineInfo lineInfo, JsonLoadSettings settings)
		{
			if (settings != null && settings.LineInfoHandling != LineInfoHandling.Load)
			{
				return;
			}
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				this.SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
				return;
			}
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x0001650A File Offset: 0x0001470A
		internal void SetLineInfo(int lineNumber, int linePosition)
		{
			this.AddAnnotation(new JToken.LineInfoAnnotation(lineNumber, linePosition));
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00016519 File Offset: 0x00014719
		bool IJsonLineInfo.HasLineInfo()
		{
			return this.Annotation<JToken.LineInfoAnnotation>() != null;
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001655 RID: 5717 RVA: 0x0006E3C8 File Offset: 0x0006C5C8
		int IJsonLineInfo.LineNumber
		{
			get
			{
				JToken.LineInfoAnnotation lineInfoAnnotation = this.Annotation<JToken.LineInfoAnnotation>();
				if (lineInfoAnnotation != null)
				{
					return lineInfoAnnotation.LineNumber;
				}
				return 0;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001656 RID: 5718 RVA: 0x0006E3E8 File Offset: 0x0006C5E8
		int IJsonLineInfo.LinePosition
		{
			get
			{
				JToken.LineInfoAnnotation lineInfoAnnotation = this.Annotation<JToken.LineInfoAnnotation>();
				if (lineInfoAnnotation != null)
				{
					return lineInfoAnnotation.LinePosition;
				}
				return 0;
			}
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00016524 File Offset: 0x00014724
		[return: Nullable(2)]
		public JToken SelectToken(string path)
		{
			return this.SelectToken(path, false);
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x0006E408 File Offset: 0x0006C608
		[return: Nullable(2)]
		public JToken SelectToken(string path, bool errorWhenNoMatch)
		{
			JPath jpath = new JPath(path);
			JToken jtoken = null;
			foreach (JToken jtoken2 in jpath.Evaluate(this, this, errorWhenNoMatch))
			{
				if (jtoken != null)
				{
					throw new JsonException("Path returned multiple tokens.");
				}
				jtoken = jtoken2;
			}
			return jtoken;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0001652E File Offset: 0x0001472E
		public IEnumerable<JToken> SelectTokens(string path)
		{
			return this.SelectTokens(path, false);
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00016538 File Offset: 0x00014738
		public IEnumerable<JToken> SelectTokens(string path, bool errorWhenNoMatch)
		{
			return new JPath(path).Evaluate(this, this, errorWhenNoMatch);
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00016548 File Offset: 0x00014748
		protected virtual DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JToken>(parameter, this, new DynamicProxy<JToken>());
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00016556 File Offset: 0x00014756
		DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
		{
			return this.GetMetaObject(parameter);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x0001655F File Offset: 0x0001475F
		object ICloneable.Clone()
		{
			return this.DeepClone();
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00016567 File Offset: 0x00014767
		public JToken DeepClone()
		{
			return this.CloneToken();
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0006E46C File Offset: 0x0006C66C
		public void AddAnnotation(object annotation)
		{
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			if (this._annotations == null)
			{
				object annotations;
				if (!(annotation is object[]))
				{
					annotations = annotation;
				}
				else
				{
					(annotations = new object[1])[0] = annotation;
				}
				this._annotations = annotations;
				return;
			}
			object[] array = this._annotations as object[];
			if (array == null)
			{
				this._annotations = new object[]
				{
					this._annotations,
					annotation
				};
				return;
			}
			int num = 0;
			while (num < array.Length && array[num] != null)
			{
				num++;
			}
			if (num == array.Length)
			{
				Array.Resize<object>(ref array, num * 2);
				this._annotations = array;
			}
			array[num] = annotation;
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x0006E504 File Offset: 0x0006C704
		[return: Nullable(2)]
		public T Annotation<T>() where T : class
		{
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					return this._annotations as T;
				}
				foreach (object obj in array)
				{
					if (obj == null)
					{
						break;
					}
					T t = obj as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x0006E570 File Offset: 0x0006C770
		[return: Nullable(2)]
		public object Annotation(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					if (type.IsInstanceOfType(this._annotations))
					{
						return this._annotations;
					}
				}
				else
				{
					foreach (object obj in array)
					{
						if (obj == null)
						{
							break;
						}
						if (type.IsInstanceOfType(obj))
						{
							return obj;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x0001656F File Offset: 0x0001476F
		public IEnumerable<T> Annotations<T>() where T : class
		{
			if (this._annotations == null)
			{
				yield break;
			}
			object annotations2 = this._annotations;
			object[] annotations = annotations2 as object[];
			if (annotations != null)
			{
				int num;
				for (int i = 0; i < annotations.Length; i = num + 1)
				{
					object obj = annotations[i];
					if (obj == null)
					{
						break;
					}
					T t = obj as T;
					if (t != null)
					{
						yield return t;
					}
					num = i;
				}
				yield break;
			}
			T t2 = this._annotations as T;
			if (t2 == null)
			{
				yield break;
			}
			yield return t2;
			yield break;
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x0001657F File Offset: 0x0001477F
		public IEnumerable<object> Annotations(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this._annotations == null)
			{
				yield break;
			}
			object annotations2 = this._annotations;
			object[] annotations = annotations2 as object[];
			if (annotations != null)
			{
				int num;
				for (int i = 0; i < annotations.Length; i = num + 1)
				{
					object obj = annotations[i];
					if (obj == null)
					{
						break;
					}
					if (type.IsInstanceOfType(obj))
					{
						yield return obj;
					}
					num = i;
				}
				yield break;
			}
			if (!type.IsInstanceOfType(this._annotations))
			{
				yield break;
			}
			yield return this._annotations;
			yield break;
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x0006E5E0 File Offset: 0x0006C7E0
		public void RemoveAnnotations<T>() where T : class
		{
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					if (this._annotations is T)
					{
						this._annotations = null;
						return;
					}
				}
				else
				{
					int i = 0;
					int j = 0;
					while (i < array.Length)
					{
						object obj = array[i];
						if (obj == null)
						{
							break;
						}
						if (!(obj is T))
						{
							array[j++] = obj;
						}
						i++;
					}
					if (j != 0)
					{
						while (j < i)
						{
							array[j++] = null;
						}
						return;
					}
					this._annotations = null;
				}
			}
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0006E65C File Offset: 0x0006C85C
		public void RemoveAnnotations(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this._annotations != null)
			{
				object[] array = this._annotations as object[];
				if (array == null)
				{
					if (type.IsInstanceOfType(this._annotations))
					{
						this._annotations = null;
						return;
					}
				}
				else
				{
					int i = 0;
					int j = 0;
					while (i < array.Length)
					{
						object obj = array[i];
						if (obj == null)
						{
							break;
						}
						if (!type.IsInstanceOfType(obj))
						{
							array[j++] = obj;
						}
						i++;
					}
					if (j != 0)
					{
						while (j < i)
						{
							array[j++] = null;
						}
						return;
					}
					this._annotations = null;
				}
			}
		}

		// Token: 0x04000BF2 RID: 3058
		[Nullable(2)]
		private static JTokenEqualityComparer _equalityComparer;

		// Token: 0x04000BF3 RID: 3059
		[Nullable(2)]
		private JContainer _parent;

		// Token: 0x04000BF4 RID: 3060
		[Nullable(2)]
		private JToken _previous;

		// Token: 0x04000BF5 RID: 3061
		[Nullable(2)]
		private JToken _next;

		// Token: 0x04000BF6 RID: 3062
		[Nullable(2)]
		private object _annotations;

		// Token: 0x04000BF7 RID: 3063
		private static readonly JTokenType[] BooleanTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean
		};

		// Token: 0x04000BF8 RID: 3064
		private static readonly JTokenType[] NumberTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean
		};

		// Token: 0x04000BF9 RID: 3065
		private static readonly JTokenType[] BigIntegerTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean,
			JTokenType.Bytes
		};

		// Token: 0x04000BFA RID: 3066
		private static readonly JTokenType[] StringTypes = new JTokenType[]
		{
			JTokenType.Date,
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean,
			JTokenType.Bytes,
			JTokenType.Guid,
			JTokenType.TimeSpan,
			JTokenType.Uri
		};

		// Token: 0x04000BFB RID: 3067
		private static readonly JTokenType[] GuidTypes = new JTokenType[]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Guid,
			JTokenType.Bytes
		};

		// Token: 0x04000BFC RID: 3068
		private static readonly JTokenType[] TimeSpanTypes = new JTokenType[]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.TimeSpan
		};

		// Token: 0x04000BFD RID: 3069
		private static readonly JTokenType[] UriTypes = new JTokenType[]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Uri
		};

		// Token: 0x04000BFE RID: 3070
		private static readonly JTokenType[] CharTypes = new JTokenType[]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw
		};

		// Token: 0x04000BFF RID: 3071
		private static readonly JTokenType[] DateTimeTypes = new JTokenType[]
		{
			JTokenType.Date,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw
		};

		// Token: 0x04000C00 RID: 3072
		private static readonly JTokenType[] BytesTypes = new JTokenType[]
		{
			JTokenType.Bytes,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Integer
		};

		// Token: 0x020002C6 RID: 710
		[NullableContext(0)]
		private class LineInfoAnnotation
		{
			// Token: 0x06001667 RID: 5735 RVA: 0x00016596 File Offset: 0x00014796
			public LineInfoAnnotation(int lineNumber, int linePosition)
			{
				this.LineNumber = lineNumber;
				this.LinePosition = linePosition;
			}

			// Token: 0x04000C01 RID: 3073
			internal readonly int LineNumber;

			// Token: 0x04000C02 RID: 3074
			internal readonly int LinePosition;
		}
	}
}
