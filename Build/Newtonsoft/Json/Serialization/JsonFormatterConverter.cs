using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000270 RID: 624
	[NullableContext(1)]
	[Nullable(0)]
	internal class JsonFormatterConverter : IFormatterConverter
	{
		// Token: 0x0600118D RID: 4493 RVA: 0x00012FD4 File Offset: 0x000111D4
		public JsonFormatterConverter(JsonSerializerInternalReader reader, JsonISerializableContract contract, [Nullable(2)] JsonProperty member)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(contract, "contract");
			this._reader = reader;
			this._contract = contract;
			this._member = member;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x00013007 File Offset: 0x00011207
		private T GetTokenValue<[Nullable(2)] T>(object value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			return (T)((object)System.Convert.ChangeType(((JValue)value).Value, typeof(T), CultureInfo.InvariantCulture));
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00061618 File Offset: 0x0005F818
		[return: Nullable(2)]
		public object Convert(object value, Type type)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JToken jtoken = value as JToken;
			if (jtoken == null)
			{
				throw new ArgumentException("Value is not a JToken.", "value");
			}
			return this._reader.CreateISerializableItem(jtoken, type, this._contract, this._member);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00061664 File Offset: 0x0005F864
		public object Convert(object value, TypeCode typeCode)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JValue jvalue = value as JValue;
			return System.Convert.ChangeType((jvalue != null) ? jvalue.Value : value, typeCode, CultureInfo.InvariantCulture);
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00013038 File Offset: 0x00011238
		public bool ToBoolean(object value)
		{
			return this.GetTokenValue<bool>(value);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00013041 File Offset: 0x00011241
		public byte ToByte(object value)
		{
			return this.GetTokenValue<byte>(value);
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0001304A File Offset: 0x0001124A
		public char ToChar(object value)
		{
			return this.GetTokenValue<char>(value);
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00013053 File Offset: 0x00011253
		public DateTime ToDateTime(object value)
		{
			return this.GetTokenValue<DateTime>(value);
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0001305C File Offset: 0x0001125C
		public decimal ToDecimal(object value)
		{
			return this.GetTokenValue<decimal>(value);
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00013065 File Offset: 0x00011265
		public double ToDouble(object value)
		{
			return this.GetTokenValue<double>(value);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0001306E File Offset: 0x0001126E
		public short ToInt16(object value)
		{
			return this.GetTokenValue<short>(value);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00013077 File Offset: 0x00011277
		public int ToInt32(object value)
		{
			return this.GetTokenValue<int>(value);
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00013080 File Offset: 0x00011280
		public long ToInt64(object value)
		{
			return this.GetTokenValue<long>(value);
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00013089 File Offset: 0x00011289
		public sbyte ToSByte(object value)
		{
			return this.GetTokenValue<sbyte>(value);
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00013092 File Offset: 0x00011292
		public float ToSingle(object value)
		{
			return this.GetTokenValue<float>(value);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0001309B File Offset: 0x0001129B
		public string ToString(object value)
		{
			return this.GetTokenValue<string>(value);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x000130A4 File Offset: 0x000112A4
		public ushort ToUInt16(object value)
		{
			return this.GetTokenValue<ushort>(value);
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x000130AD File Offset: 0x000112AD
		public uint ToUInt32(object value)
		{
			return this.GetTokenValue<uint>(value);
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x000130B6 File Offset: 0x000112B6
		public ulong ToUInt64(object value)
		{
			return this.GetTokenValue<ulong>(value);
		}

		// Token: 0x04000AA1 RID: 2721
		private readonly JsonSerializerInternalReader _reader;

		// Token: 0x04000AA2 RID: 2722
		private readonly JsonISerializableContract _contract;

		// Token: 0x04000AA3 RID: 2723
		[Nullable(2)]
		private readonly JsonProperty _member;
	}
}
