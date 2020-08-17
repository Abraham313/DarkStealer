using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002CF RID: 719
	[NullableContext(2)]
	[Nullable(0)]
	public class JTokenWriter : JsonWriter
	{
		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x060016A2 RID: 5794 RVA: 0x0001670A File Offset: 0x0001490A
		public JToken CurrentToken
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060016A3 RID: 5795 RVA: 0x00016712 File Offset: 0x00014912
		public JToken Token
		{
			get
			{
				if (this._token != null)
				{
					return this._token;
				}
				return this._value;
			}
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00016729 File Offset: 0x00014929
		[NullableContext(1)]
		public JTokenWriter(JContainer container)
		{
			ValidationUtils.ArgumentNotNull(container, "container");
			this._token = container;
			this._parent = container;
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x0001674A File Offset: 0x0001494A
		public JTokenWriter()
		{
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00009B58 File Offset: 0x00007D58
		public override void Flush()
		{
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00016752 File Offset: 0x00014952
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x0001675A File Offset: 0x0001495A
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new JObject());
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x0001676D File Offset: 0x0001496D
		[NullableContext(1)]
		private void AddParent(JContainer container)
		{
			if (this._parent == null)
			{
				this._token = container;
			}
			else
			{
				this._parent.AddAndSkipParentCheck(container);
			}
			this._parent = container;
			this._current = container;
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x0006F250 File Offset: 0x0006D450
		private void RemoveParent()
		{
			this._current = this._parent;
			this._parent = this._parent.Parent;
			if (this._parent != null && this._parent.Type == JTokenType.Property)
			{
				this._parent = this._parent.Parent;
			}
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x0001679A File Offset: 0x0001499A
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new JArray());
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x000167AD File Offset: 0x000149AD
		[NullableContext(1)]
		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			this.AddParent(new JConstructor(name));
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x000167C2 File Offset: 0x000149C2
		protected override void WriteEnd(JsonToken token)
		{
			this.RemoveParent();
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x000167CA File Offset: 0x000149CA
		[NullableContext(1)]
		public override void WritePropertyName(string name)
		{
			JObject jobject = this._parent as JObject;
			if (jobject != null)
			{
				jobject.Remove(name);
			}
			this.AddParent(new JProperty(name));
			base.WritePropertyName(name);
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x000167F7 File Offset: 0x000149F7
		private void AddValue(object value, JsonToken token)
		{
			this.AddValue(new JValue(value), token);
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x0006F2A4 File Offset: 0x0006D4A4
		internal void AddValue(JValue value, JsonToken token)
		{
			if (this._parent != null)
			{
				this._parent.Add(value);
				this._current = this._parent.Last;
				if (this._parent.Type == JTokenType.Property)
				{
					this._parent = this._parent.Parent;
					return;
				}
			}
			else
			{
				this._value = (value ?? JValue.CreateNull());
				this._current = this._value;
			}
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x00016806 File Offset: 0x00014A06
		public override void WriteValue(object value)
		{
			if (value is System.Numerics.BigInteger)
			{
				base.InternalWriteValue(JsonToken.Integer);
				this.AddValue(value, JsonToken.Integer);
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x00016827 File Offset: 0x00014A27
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddValue(null, JsonToken.Null);
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x00016838 File Offset: 0x00014A38
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddValue(null, JsonToken.Undefined);
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x00016849 File Offset: 0x00014A49
		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			this.AddValue(new JRaw(json), JsonToken.Raw);
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x0001685F File Offset: 0x00014A5F
		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			this.AddValue(JValue.CreateComment(text), JsonToken.Comment);
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x00016875 File Offset: 0x00014A75
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x00016887 File Offset: 0x00014A87
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x0001689D File Offset: 0x00014A9D
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x000168B3 File Offset: 0x00014AB3
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x000168C9 File Offset: 0x00014AC9
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x000168DF File Offset: 0x00014ADF
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x000168F5 File Offset: 0x00014AF5
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x0001690B File Offset: 0x00014B0B
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Boolean);
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x00016922 File Offset: 0x00014B22
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x00016938 File Offset: 0x00014B38
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x0006F314 File Offset: 0x0006D514
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string value2 = value.ToString(CultureInfo.InvariantCulture);
			this.AddValue(value2, JsonToken.String);
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x0001694E File Offset: 0x00014B4E
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x00016964 File Offset: 0x00014B64
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x0001697A File Offset: 0x00014B7A
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00016990 File Offset: 0x00014B90
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x000169B5 File Offset: 0x00014BB5
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x000169CC File Offset: 0x00014BCC
		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Bytes);
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x000169DE File Offset: 0x00014BDE
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x000169F5 File Offset: 0x00014BF5
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x00016A0C File Offset: 0x00014C0C
		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.String);
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x0006F340 File Offset: 0x0006D540
		[NullableContext(1)]
		internal override void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments)
		{
			JTokenReader jtokenReader = reader as JTokenReader;
			if (jtokenReader == null || !writeChildren || !writeDateConstructorAsDate || !writeComments)
			{
				base.WriteToken(reader, writeChildren, writeDateConstructorAsDate, writeComments);
				return;
			}
			if (jtokenReader.TokenType == JsonToken.None && !jtokenReader.Read())
			{
				return;
			}
			JToken jtoken = jtokenReader.CurrentToken.CloneToken();
			if (this._parent != null)
			{
				this._parent.Add(jtoken);
				this._current = this._parent.Last;
				if (this._parent.Type == JTokenType.Property)
				{
					this._parent = this._parent.Parent;
					base.InternalWriteValue(JsonToken.Null);
				}
			}
			else
			{
				this._current = jtoken;
				if (this._token == null && this._value == null)
				{
					this._token = (jtoken as JContainer);
					this._value = (jtoken as JValue);
				}
			}
			jtokenReader.Skip();
		}

		// Token: 0x04000C39 RID: 3129
		private JContainer _token;

		// Token: 0x04000C3A RID: 3130
		private JContainer _parent;

		// Token: 0x04000C3B RID: 3131
		private JValue _value;

		// Token: 0x04000C3C RID: 3132
		private JToken _current;
	}
}
