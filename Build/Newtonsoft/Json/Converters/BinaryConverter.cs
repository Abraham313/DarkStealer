using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020002EF RID: 751
	[NullableContext(1)]
	[Nullable(0)]
	public class BinaryConverter : JsonConverter
	{
		// Token: 0x060017AC RID: 6060 RVA: 0x00073000 File Offset: 0x00071200
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			byte[] byteArray = this.GetByteArray(value);
			writer.WriteValue(byteArray);
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00073028 File Offset: 0x00071228
		private byte[] GetByteArray(object value)
		{
			if (value.GetType().FullName == "System.Data.Linq.Binary")
			{
				BinaryConverter.EnsureReflectionObject(value.GetType());
				return (byte[])BinaryConverter._reflectionObject.GetValue(value, "ToArray");
			}
			if (!(value is SqlBinary))
			{
				throw new JsonSerializationException("Unexpected value type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
			}
			return ((SqlBinary)value).Value;
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0001731C File Offset: 0x0001551C
		private static void EnsureReflectionObject(Type t)
		{
			if (BinaryConverter._reflectionObject == null)
			{
				BinaryConverter._reflectionObject = ReflectionObject.Create(t, t.GetConstructor(new Type[]
				{
					typeof(byte[])
				}), new string[]
				{
					"ToArray"
				});
			}
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x000730A0 File Offset: 0x000712A0
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullable(objectType))
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			else
			{
				byte[] array;
				if (reader.TokenType == JsonToken.StartArray)
				{
					array = this.ReadByteArray(reader);
				}
				else
				{
					if (reader.TokenType != JsonToken.String)
					{
						throw JsonSerializationException.Create(reader, "Unexpected token parsing binary. Expected String or StartArray, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
					}
					array = Convert.FromBase64String(reader.Value.ToString());
				}
				Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
				if (type.FullName == "System.Data.Linq.Binary")
				{
					BinaryConverter.EnsureReflectionObject(type);
					return BinaryConverter._reflectionObject.Creator(new object[]
					{
						array
					});
				}
				if (!(type == typeof(SqlBinary)))
				{
					throw JsonSerializationException.Create(reader, "Unexpected object type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return new SqlBinary(array);
			}
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x000731A0 File Offset: 0x000713A0
		private byte[] ReadByteArray(JsonReader reader)
		{
			List<byte> list = new List<byte>();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType != JsonToken.Integer)
					{
						if (tokenType != JsonToken.EndArray)
						{
							throw JsonSerializationException.Create(reader, "Unexpected token when reading bytes: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
						}
						return list.ToArray();
					}
					else
					{
						list.Add(Convert.ToByte(reader.Value, CultureInfo.InvariantCulture));
					}
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading bytes.");
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00017357 File Offset: 0x00015557
		public override bool CanConvert(Type objectType)
		{
			return objectType.FullName == "System.Data.Linq.Binary" || objectType == typeof(SqlBinary) || objectType == typeof(SqlBinary?);
		}

		// Token: 0x04000CCF RID: 3279
		private const string BinaryTypeName = "System.Data.Linq.Binary";

		// Token: 0x04000CD0 RID: 3280
		private const string BinaryToArrayName = "ToArray";

		// Token: 0x04000CD1 RID: 3281
		[Nullable(2)]
		private static ReflectionObject _reflectionObject;
	}
}
