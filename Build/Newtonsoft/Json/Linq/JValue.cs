using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002D0 RID: 720
	[NullableContext(1)]
	[Nullable(0)]
	public class JValue : JToken, IEquatable<JValue>, IComparable<JValue>, IConvertible, IFormattable, IComparable
	{
		// Token: 0x060016CB RID: 5835 RVA: 0x00016A1E File Offset: 0x00014C1E
		[NullableContext(2)]
		internal JValue(object value, JTokenType type)
		{
			this._value = value;
			this._valueType = type;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00016A34 File Offset: 0x00014C34
		public JValue(JValue other) : this(other.Value, other.Type)
		{
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00016A48 File Offset: 0x00014C48
		public JValue(long value) : this(value, JTokenType.Integer)
		{
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00016A57 File Offset: 0x00014C57
		public JValue(decimal value) : this(value, JTokenType.Float)
		{
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00016A66 File Offset: 0x00014C66
		public JValue(char value) : this(value, JTokenType.String)
		{
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00016A75 File Offset: 0x00014C75
		[CLSCompliant(false)]
		public JValue(ulong value) : this(value, JTokenType.Integer)
		{
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00016A84 File Offset: 0x00014C84
		public JValue(double value) : this(value, JTokenType.Float)
		{
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00016A93 File Offset: 0x00014C93
		public JValue(float value) : this(value, JTokenType.Float)
		{
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00016AA2 File Offset: 0x00014CA2
		public JValue(DateTime value) : this(value, JTokenType.Date)
		{
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00016AB2 File Offset: 0x00014CB2
		public JValue(DateTimeOffset value) : this(value, JTokenType.Date)
		{
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00016AC2 File Offset: 0x00014CC2
		public JValue(bool value) : this(value, JTokenType.Boolean)
		{
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00016AD2 File Offset: 0x00014CD2
		[NullableContext(2)]
		public JValue(string value) : this(value, JTokenType.String)
		{
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00016ADC File Offset: 0x00014CDC
		public JValue(Guid value) : this(value, JTokenType.Guid)
		{
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x00016AEC File Offset: 0x00014CEC
		[NullableContext(2)]
		public JValue(Uri value) : this(value, (value != null) ? JTokenType.Uri : JTokenType.Null)
		{
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00016B04 File Offset: 0x00014D04
		public JValue(TimeSpan value) : this(value, JTokenType.TimeSpan)
		{
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x0006F410 File Offset: 0x0006D610
		[NullableContext(2)]
		public JValue(object value) : this(value, JValue.GetValueType(null, value))
		{
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x0006F434 File Offset: 0x0006D634
		internal override bool DeepEquals(JToken node)
		{
			JValue jvalue = node as JValue;
			return jvalue != null && (jvalue == this || JValue.ValuesEquals(this, jvalue));
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060016DC RID: 5852 RVA: 0x00009021 File Offset: 0x00007221
		public override bool HasValues
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x0006F45C File Offset: 0x0006D65C
		private static int CompareBigInteger(System.Numerics.BigInteger i1, object i2)
		{
			int num = i1.CompareTo(ConvertUtils.ToBigInteger(i2));
			if (num != 0)
			{
				return num;
			}
			if (i2 is decimal)
			{
				decimal num2 = (decimal)i2;
				return 0m.CompareTo(Math.Abs(num2 - Math.Truncate(num2)));
			}
			if (!(i2 is double) && !(i2 is float))
			{
				return num;
			}
			double num3 = Convert.ToDouble(i2, CultureInfo.InvariantCulture);
			return 0.0.CompareTo(Math.Abs(num3 - Math.Truncate(num3)));
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x0006F4E8 File Offset: 0x0006D6E8
		[NullableContext(2)]
		internal static int Compare(JTokenType valueType, object objA, object objB)
		{
			if (objA == objB)
			{
				return 0;
			}
			if (objB == null)
			{
				return 1;
			}
			if (objA == null)
			{
				return -1;
			}
			switch (valueType)
			{
			case JTokenType.Comment:
			case JTokenType.String:
			case JTokenType.Raw:
			{
				string strA = Convert.ToString(objA, CultureInfo.InvariantCulture);
				string strB = Convert.ToString(objB, CultureInfo.InvariantCulture);
				return string.CompareOrdinal(strA, strB);
			}
			case JTokenType.Integer:
				if (objA is System.Numerics.BigInteger)
				{
					System.Numerics.BigInteger i = (System.Numerics.BigInteger)objA;
					return JValue.CompareBigInteger(i, objB);
				}
				if (objB is System.Numerics.BigInteger)
				{
					System.Numerics.BigInteger i2 = (System.Numerics.BigInteger)objB;
					return -JValue.CompareBigInteger(i2, objA);
				}
				if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
				{
					return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
				}
				if (!(objA is float) && !(objB is float) && !(objA is double) && !(objB is double))
				{
					return Convert.ToInt64(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, CultureInfo.InvariantCulture));
				}
				return JValue.CompareFloat(objA, objB);
			case JTokenType.Float:
				if (objA is System.Numerics.BigInteger)
				{
					System.Numerics.BigInteger i3 = (System.Numerics.BigInteger)objA;
					return JValue.CompareBigInteger(i3, objB);
				}
				if (objB is System.Numerics.BigInteger)
				{
					System.Numerics.BigInteger i4 = (System.Numerics.BigInteger)objB;
					return -JValue.CompareBigInteger(i4, objA);
				}
				if (!(objA is ulong) && !(objB is ulong) && !(objA is decimal) && !(objB is decimal))
				{
					return JValue.CompareFloat(objA, objB);
				}
				return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
			case JTokenType.Boolean:
			{
				bool flag = Convert.ToBoolean(objA, CultureInfo.InvariantCulture);
				bool value = Convert.ToBoolean(objB, CultureInfo.InvariantCulture);
				return flag.CompareTo(value);
			}
			case JTokenType.Date:
			{
				if (objA is DateTime)
				{
					DateTime dateTime = (DateTime)objA;
					DateTime value2;
					if (objB is DateTimeOffset)
					{
						value2 = ((DateTimeOffset)objB).DateTime;
					}
					else
					{
						value2 = Convert.ToDateTime(objB, CultureInfo.InvariantCulture);
					}
					return dateTime.CompareTo(value2);
				}
				DateTimeOffset dateTimeOffset = (DateTimeOffset)objA;
				DateTimeOffset other;
				if (objB is DateTimeOffset)
				{
					other = (DateTimeOffset)objB;
				}
				else
				{
					other = new DateTimeOffset(Convert.ToDateTime(objB, CultureInfo.InvariantCulture));
				}
				return dateTimeOffset.CompareTo(other);
			}
			case JTokenType.Bytes:
			{
				byte[] array = objB as byte[];
				if (array == null)
				{
					throw new ArgumentException("Object must be of type byte[].");
				}
				return MiscellaneousUtils.ByteArrayCompare(objA as byte[], array);
			}
			case JTokenType.Guid:
			{
				if (!(objB is Guid))
				{
					throw new ArgumentException("Object must be of type Guid.");
				}
				Guid guid = (Guid)objA;
				Guid value3 = (Guid)objB;
				return guid.CompareTo(value3);
			}
			case JTokenType.Uri:
			{
				Uri uri = objB as Uri;
				if (uri == null)
				{
					throw new ArgumentException("Object must be of type Uri.");
				}
				Uri uri2 = (Uri)objA;
				return Comparer<string>.Default.Compare(uri2.ToString(), uri.ToString());
			}
			case JTokenType.TimeSpan:
			{
				if (!(objB is TimeSpan))
				{
					throw new ArgumentException("Object must be of type TimeSpan.");
				}
				TimeSpan timeSpan = (TimeSpan)objA;
				TimeSpan value4 = (TimeSpan)objB;
				return timeSpan.CompareTo(value4);
			}
			}
			throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, valueType));
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x0006F814 File Offset: 0x0006DA14
		private static int CompareFloat(object objA, object objB)
		{
			double d = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
			double num = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
			if (MathUtils.ApproxEquals(d, num))
			{
				return 0;
			}
			return d.CompareTo(num);
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x0006F84C File Offset: 0x0006DA4C
		[NullableContext(2)]
		private static bool Operation(ExpressionType operation, object objA, object objB, out object result)
		{
			if (objA is string || objB is string)
			{
				if (operation != ExpressionType.Add)
				{
					if (operation != ExpressionType.AddAssign)
					{
						goto IL_20;
					}
				}
				result = ((objA != null) ? objA.ToString() : null) + ((objB != null) ? objB.ToString() : null);
				return true;
			}
			IL_20:
			if (!(objA is System.Numerics.BigInteger) && !(objB is System.Numerics.BigInteger))
			{
				if (!(objA is ulong) && !(objB is ulong) && !(objA is decimal) && !(objB is decimal))
				{
					if (!(objA is float) && !(objB is float) && !(objA is double) && !(objB is double))
					{
						if (objA is int || objA is uint || objA is long || objA is short || objA is ushort || objA is sbyte || objA is byte || objB is int || objB is uint || objB is long || objB is short || objB is ushort || objB is sbyte || objB is byte)
						{
							if (objA != null && objB != null)
							{
								long num = Convert.ToInt64(objA, CultureInfo.InvariantCulture);
								long num2 = Convert.ToInt64(objB, CultureInfo.InvariantCulture);
								if (operation <= ExpressionType.Subtract)
								{
									if (operation <= ExpressionType.Divide)
									{
										if (operation == ExpressionType.Add)
										{
											goto IL_16F;
										}
										if (operation != ExpressionType.Divide)
										{
											goto IL_380;
										}
									}
									else
									{
										if (operation == ExpressionType.Multiply)
										{
											goto IL_19A;
										}
										if (operation != ExpressionType.Subtract)
										{
											goto IL_380;
										}
										goto IL_18C;
									}
								}
								else if (operation <= ExpressionType.DivideAssign)
								{
									if (operation == ExpressionType.AddAssign)
									{
										goto IL_16F;
									}
									if (operation != ExpressionType.DivideAssign)
									{
										goto IL_380;
									}
								}
								else
								{
									if (operation == ExpressionType.MultiplyAssign)
									{
										goto IL_19A;
									}
									if (operation != ExpressionType.SubtractAssign)
									{
										goto IL_380;
									}
									goto IL_18C;
								}
								result = num / num2;
								return true;
								IL_16F:
								result = num + num2;
								return true;
								IL_18C:
								result = num - num2;
								return true;
								IL_19A:
								result = num * num2;
								return true;
							}
							result = null;
							return true;
						}
					}
					else
					{
						if (objA != null && objB != null)
						{
							double num3 = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
							double num4 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
							if (operation <= ExpressionType.Subtract)
							{
								if (operation <= ExpressionType.Divide)
								{
									if (operation == ExpressionType.Add)
									{
										goto IL_21B;
									}
									if (operation != ExpressionType.Divide)
									{
										goto IL_380;
									}
								}
								else
								{
									if (operation == ExpressionType.Multiply)
									{
										goto IL_246;
									}
									if (operation != ExpressionType.Subtract)
									{
										goto IL_380;
									}
									goto IL_238;
								}
							}
							else if (operation <= ExpressionType.DivideAssign)
							{
								if (operation == ExpressionType.AddAssign)
								{
									goto IL_21B;
								}
								if (operation != ExpressionType.DivideAssign)
								{
									goto IL_380;
								}
							}
							else
							{
								if (operation == ExpressionType.MultiplyAssign)
								{
									goto IL_246;
								}
								if (operation != ExpressionType.SubtractAssign)
								{
									goto IL_380;
								}
								goto IL_238;
							}
							result = num3 / num4;
							return true;
							IL_21B:
							result = num3 + num4;
							return true;
							IL_238:
							result = num3 - num4;
							return true;
							IL_246:
							result = num3 * num4;
							return true;
						}
						result = null;
						return true;
					}
				}
				else
				{
					if (objA != null && objB != null)
					{
						decimal d = Convert.ToDecimal(objA, CultureInfo.InvariantCulture);
						decimal d2 = Convert.ToDecimal(objB, CultureInfo.InvariantCulture);
						if (operation <= ExpressionType.Subtract)
						{
							if (operation <= ExpressionType.Divide)
							{
								if (operation == ExpressionType.Add)
								{
									goto IL_2C7;
								}
								if (operation != ExpressionType.Divide)
								{
									goto IL_380;
								}
							}
							else
							{
								if (operation == ExpressionType.Multiply)
								{
									goto IL_2F6;
								}
								if (operation != ExpressionType.Subtract)
								{
									goto IL_380;
								}
								goto IL_2E6;
							}
						}
						else if (operation <= ExpressionType.DivideAssign)
						{
							if (operation == ExpressionType.AddAssign)
							{
								goto IL_2C7;
							}
							if (operation != ExpressionType.DivideAssign)
							{
								goto IL_380;
							}
						}
						else
						{
							if (operation == ExpressionType.MultiplyAssign)
							{
								goto IL_2F6;
							}
							if (operation != ExpressionType.SubtractAssign)
							{
								goto IL_380;
							}
							goto IL_2E6;
						}
						result = d / d2;
						return true;
						IL_2C7:
						result = d + d2;
						return true;
						IL_2E6:
						result = d - d2;
						return true;
						IL_2F6:
						result = d * d2;
						return true;
					}
					result = null;
					return true;
				}
			}
			else
			{
				if (objA != null && objB != null)
				{
					System.Numerics.BigInteger bigInteger = ConvertUtils.ToBigInteger(objA);
					System.Numerics.BigInteger bigInteger2 = ConvertUtils.ToBigInteger(objB);
					if (operation <= ExpressionType.Subtract)
					{
						if (operation <= ExpressionType.Divide)
						{
							if (operation == ExpressionType.Add)
							{
								goto IL_366;
							}
							if (operation != ExpressionType.Divide)
							{
								goto IL_380;
							}
						}
						else
						{
							if (operation == ExpressionType.Multiply)
							{
								goto IL_395;
							}
							if (operation != ExpressionType.Subtract)
							{
								goto IL_380;
							}
							goto IL_385;
						}
					}
					else if (operation <= ExpressionType.DivideAssign)
					{
						if (operation == ExpressionType.AddAssign)
						{
							goto IL_366;
						}
						if (operation != ExpressionType.DivideAssign)
						{
							goto IL_380;
						}
					}
					else
					{
						if (operation == ExpressionType.MultiplyAssign)
						{
							goto IL_395;
						}
						if (operation != ExpressionType.SubtractAssign)
						{
							goto IL_380;
						}
						goto IL_385;
					}
					result = bigInteger / bigInteger2;
					return true;
					IL_366:
					result = bigInteger + bigInteger2;
					return true;
					IL_385:
					result = bigInteger - bigInteger2;
					return true;
					IL_395:
					result = bigInteger * bigInteger2;
					return true;
				}
				result = null;
				return true;
			}
			IL_380:
			result = null;
			return false;
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x00016B14 File Offset: 0x00014D14
		internal override JToken CloneToken()
		{
			return new JValue(this);
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00016B1C File Offset: 0x00014D1C
		public static JValue CreateComment([Nullable(2)] string value)
		{
			return new JValue(value, JTokenType.Comment);
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00016B25 File Offset: 0x00014D25
		public static JValue CreateString([Nullable(2)] string value)
		{
			return new JValue(value, JTokenType.String);
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00016B2E File Offset: 0x00014D2E
		public static JValue CreateNull()
		{
			return new JValue(null, JTokenType.Null);
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x00016B38 File Offset: 0x00014D38
		public static JValue CreateUndefined()
		{
			return new JValue(null, JTokenType.Undefined);
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x0006FC24 File Offset: 0x0006DE24
		[NullableContext(2)]
		private static JTokenType GetValueType(JTokenType? current, object value)
		{
			if (value == null)
			{
				return JTokenType.Null;
			}
			if (value == DBNull.Value)
			{
				return JTokenType.Null;
			}
			if (value is string)
			{
				return JValue.GetStringValueType(current);
			}
			if (value is long || value is int || value is short || value is sbyte || value is ulong || value is uint || value is ushort || value is byte)
			{
				return JTokenType.Integer;
			}
			if (value is Enum)
			{
				return JTokenType.Integer;
			}
			if (value is System.Numerics.BigInteger)
			{
				return JTokenType.Integer;
			}
			if (value is double || value is float || value is decimal)
			{
				return JTokenType.Float;
			}
			if (value is DateTime)
			{
				return JTokenType.Date;
			}
			if (value is DateTimeOffset)
			{
				return JTokenType.Date;
			}
			if (value is byte[])
			{
				return JTokenType.Bytes;
			}
			if (value is bool)
			{
				return JTokenType.Boolean;
			}
			if (value is Guid)
			{
				return JTokenType.Guid;
			}
			if (value is Uri)
			{
				return JTokenType.Uri;
			}
			if (value is TimeSpan)
			{
				return JTokenType.TimeSpan;
			}
			throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x0006FD40 File Offset: 0x0006DF40
		private static JTokenType GetStringValueType(JTokenType? current)
		{
			if (current == null)
			{
				return JTokenType.String;
			}
			JTokenType valueOrDefault = current.GetValueOrDefault();
			if (valueOrDefault != JTokenType.Comment && valueOrDefault != JTokenType.String)
			{
				if (valueOrDefault != JTokenType.Raw)
				{
					return JTokenType.String;
				}
			}
			return current.GetValueOrDefault();
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060016E8 RID: 5864 RVA: 0x00016B42 File Offset: 0x00014D42
		public override JTokenType Type
		{
			get
			{
				return this._valueType;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060016E9 RID: 5865 RVA: 0x00016B4A File Offset: 0x00014D4A
		// (set) Token: 0x060016EA RID: 5866 RVA: 0x0006FD78 File Offset: 0x0006DF78
		[Nullable(2)]
		public new object Value
		{
			[NullableContext(2)]
			get
			{
				return this._value;
			}
			[NullableContext(2)]
			set
			{
				object value2 = this._value;
				Type left = (value2 != null) ? value2.GetType() : null;
				Type right = (value != null) ? value.GetType() : null;
				if (left != right)
				{
					this._valueType = JValue.GetValueType(new JTokenType?(this._valueType), value);
				}
				this._value = value;
			}
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x0006FDCC File Offset: 0x0006DFCC
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			if (converters != null && converters.Length != 0 && this._value != null)
			{
				JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter(converters, this._value.GetType());
				if (matchingConverter != null && matchingConverter.CanWrite)
				{
					matchingConverter.WriteJson(writer, this._value, JsonSerializer.CreateDefault());
					return;
				}
			}
			switch (this._valueType)
			{
			case JTokenType.Comment:
			{
				object value = this._value;
				writer.WriteComment((value != null) ? value.ToString() : null);
				return;
			}
			case JTokenType.Integer:
			{
				object value2 = this._value;
				if (value2 is int)
				{
					int value3 = (int)value2;
					writer.WriteValue(value3);
					return;
				}
				value2 = this._value;
				if (value2 is long)
				{
					long value4 = (long)value2;
					writer.WriteValue(value4);
					return;
				}
				value2 = this._value;
				if (value2 is ulong)
				{
					ulong value5 = (ulong)value2;
					writer.WriteValue(value5);
					return;
				}
				value2 = this._value;
				if (value2 is System.Numerics.BigInteger)
				{
					System.Numerics.BigInteger bigInteger = (System.Numerics.BigInteger)value2;
					writer.WriteValue(bigInteger);
					return;
				}
				writer.WriteValue(Convert.ToInt64(this._value, CultureInfo.InvariantCulture));
				return;
			}
			case JTokenType.Float:
			{
				object value2 = this._value;
				if (value2 is decimal)
				{
					decimal value6 = (decimal)value2;
					writer.WriteValue(value6);
					return;
				}
				value2 = this._value;
				if (value2 is double)
				{
					double value7 = (double)value2;
					writer.WriteValue(value7);
					return;
				}
				value2 = this._value;
				if (value2 is float)
				{
					float value8 = (float)value2;
					writer.WriteValue(value8);
					return;
				}
				writer.WriteValue(Convert.ToDouble(this._value, CultureInfo.InvariantCulture));
				return;
			}
			case JTokenType.String:
			{
				object value9 = this._value;
				writer.WriteValue((value9 != null) ? value9.ToString() : null);
				return;
			}
			case JTokenType.Boolean:
				writer.WriteValue(Convert.ToBoolean(this._value, CultureInfo.InvariantCulture));
				return;
			case JTokenType.Null:
				writer.WriteNull();
				return;
			case JTokenType.Undefined:
				writer.WriteUndefined();
				return;
			case JTokenType.Date:
			{
				object value2 = this._value;
				if (value2 is DateTimeOffset)
				{
					DateTimeOffset value10 = (DateTimeOffset)value2;
					writer.WriteValue(value10);
					return;
				}
				writer.WriteValue(Convert.ToDateTime(this._value, CultureInfo.InvariantCulture));
				return;
			}
			case JTokenType.Raw:
			{
				object value11 = this._value;
				writer.WriteRawValue((value11 != null) ? value11.ToString() : null);
				return;
			}
			case JTokenType.Bytes:
				writer.WriteValue((byte[])this._value);
				return;
			case JTokenType.Guid:
				writer.WriteValue((this._value != null) ? ((Guid?)this._value) : null);
				return;
			case JTokenType.Uri:
				writer.WriteValue((Uri)this._value);
				return;
			case JTokenType.TimeSpan:
				writer.WriteValue((this._value != null) ? ((TimeSpan?)this._value) : null);
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", this._valueType, "Unexpected token type.");
			}
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x000700A0 File Offset: 0x0006E2A0
		internal override int GetDeepHashCode()
		{
			int num = (this._value != null) ? this._value.GetHashCode() : 0;
			int valueType = (int)this._valueType;
			return valueType.GetHashCode() ^ num;
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x00016B52 File Offset: 0x00014D52
		private static bool ValuesEquals(JValue v1, JValue v2)
		{
			return v1 == v2 || (v1._valueType == v2._valueType && JValue.Compare(v1._valueType, v1._value, v2._value) == 0);
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x00016B84 File Offset: 0x00014D84
		public bool Equals([AllowNull] JValue other)
		{
			return other != null && JValue.ValuesEquals(this, other);
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x000700D4 File Offset: 0x0006E2D4
		public override bool Equals(object obj)
		{
			JValue jvalue = obj as JValue;
			return jvalue != null && this.Equals(jvalue);
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00016B92 File Offset: 0x00014D92
		public override int GetHashCode()
		{
			if (this._value == null)
			{
				return 0;
			}
			return this._value.GetHashCode();
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x00016BA9 File Offset: 0x00014DA9
		public override string ToString()
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			return this._value.ToString();
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00016BC4 File Offset: 0x00014DC4
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.CurrentCulture);
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00016BD2 File Offset: 0x00014DD2
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x000700F4 File Offset: 0x0006E2F4
		public string ToString([Nullable(2)] string format, IFormatProvider formatProvider)
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			IFormattable formattable = this._value as IFormattable;
			if (formattable != null)
			{
				return formattable.ToString(format, formatProvider);
			}
			return this._value.ToString();
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00016BDC File Offset: 0x00014DDC
		protected override DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JValue>(parameter, this, new JValue.JValueDynamicProxy());
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00070134 File Offset: 0x0006E334
		int IComparable.CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			JValue jvalue = obj as JValue;
			object objB;
			JTokenType valueType2;
			if (jvalue != null)
			{
				objB = jvalue.Value;
				JTokenType valueType;
				if (this._valueType == JTokenType.String)
				{
					if (this._valueType != jvalue._valueType)
					{
						valueType = jvalue._valueType;
						goto IL_3D;
					}
				}
				valueType = this._valueType;
				IL_3D:
				valueType2 = valueType;
			}
			else
			{
				objB = obj;
				valueType2 = this._valueType;
			}
			return JValue.Compare(valueType2, this._value, objB);
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00016BEA File Offset: 0x00014DEA
		public int CompareTo(JValue obj)
		{
			if (obj == null)
			{
				return 1;
			}
			JTokenType valueType;
			if (this._valueType == JTokenType.String)
			{
				if (this._valueType != obj._valueType)
				{
					valueType = obj._valueType;
					goto IL_2C;
				}
			}
			valueType = this._valueType;
			IL_2C:
			return JValue.Compare(valueType, this._value, obj._value);
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x00070198 File Offset: 0x0006E398
		TypeCode IConvertible.GetTypeCode()
		{
			if (this._value == null)
			{
				return TypeCode.Empty;
			}
			IConvertible convertible = this._value as IConvertible;
			if (convertible != null)
			{
				return convertible.GetTypeCode();
			}
			return TypeCode.Object;
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00016C29 File Offset: 0x00014E29
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return (bool)this;
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00016C31 File Offset: 0x00014E31
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return (char)this;
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x00016C39 File Offset: 0x00014E39
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return (sbyte)this;
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x00016C41 File Offset: 0x00014E41
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return (byte)this;
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x00016C49 File Offset: 0x00014E49
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return (short)this;
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00016C51 File Offset: 0x00014E51
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return (ushort)this;
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00016C59 File Offset: 0x00014E59
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return (int)this;
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x00016C61 File Offset: 0x00014E61
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return (uint)this;
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00016C69 File Offset: 0x00014E69
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return (long)this;
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00016C71 File Offset: 0x00014E71
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return (ulong)this;
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00016C79 File Offset: 0x00014E79
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return (float)this;
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00016C82 File Offset: 0x00014E82
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return (double)this;
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x00016C8B File Offset: 0x00014E8B
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return (decimal)this;
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x00016C93 File Offset: 0x00014E93
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return (DateTime)this;
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x00016C9B File Offset: 0x00014E9B
		[return: Nullable(2)]
		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return base.ToObject(conversionType);
		}

		// Token: 0x04000C3D RID: 3133
		private JTokenType _valueType;

		// Token: 0x04000C3E RID: 3134
		[Nullable(2)]
		private object _value;

		// Token: 0x020002D1 RID: 721
		[Nullable(new byte[]
		{
			0,
			1
		})]
		private class JValueDynamicProxy : DynamicProxy<JValue>
		{
			// Token: 0x06001708 RID: 5896 RVA: 0x000701C8 File Offset: 0x0006E3C8
			public override bool TryConvert(JValue instance, ConvertBinder binder, [Nullable(2)] [NotNullWhen(true)] out object result)
			{
				if (binder.Type == typeof(JValue) || binder.Type == typeof(JToken))
				{
					result = instance;
					return true;
				}
				object value = instance.Value;
				if (value == null)
				{
					result = null;
					return ReflectionUtils.IsNullable(binder.Type);
				}
				result = ConvertUtils.Convert(value, CultureInfo.InvariantCulture, binder.Type);
				return true;
			}

			// Token: 0x06001709 RID: 5897 RVA: 0x00070238 File Offset: 0x0006E438
			public override bool TryBinaryOperation(JValue instance, BinaryOperationBinder binder, object arg, [Nullable(2)] [NotNullWhen(true)] out object result)
			{
				JValue jvalue = arg as JValue;
				object objB = (jvalue != null) ? jvalue.Value : arg;
				ExpressionType operation = binder.Operation;
				if (operation <= ExpressionType.NotEqual)
				{
					if (operation <= ExpressionType.LessThanOrEqual)
					{
						if (operation != ExpressionType.Add)
						{
							switch (operation)
							{
							case ExpressionType.Divide:
								break;
							case ExpressionType.Equal:
								result = (JValue.Compare(instance.Type, instance.Value, objB) == 0);
								return true;
							case ExpressionType.ExclusiveOr:
							case ExpressionType.Invoke:
							case ExpressionType.Lambda:
							case ExpressionType.LeftShift:
								goto IL_178;
							case ExpressionType.GreaterThan:
								result = (JValue.Compare(instance.Type, instance.Value, objB) > 0);
								return true;
							case ExpressionType.GreaterThanOrEqual:
								result = (JValue.Compare(instance.Type, instance.Value, objB) >= 0);
								return true;
							case ExpressionType.LessThan:
								result = (JValue.Compare(instance.Type, instance.Value, objB) < 0);
								return true;
							case ExpressionType.LessThanOrEqual:
								result = (JValue.Compare(instance.Type, instance.Value, objB) <= 0);
								return true;
							default:
								goto IL_178;
							}
						}
					}
					else if (operation != ExpressionType.Multiply)
					{
						if (operation != ExpressionType.NotEqual)
						{
							goto IL_178;
						}
						result = (JValue.Compare(instance.Type, instance.Value, objB) != 0);
						return true;
					}
				}
				else if (operation <= ExpressionType.AddAssign)
				{
					if (operation != ExpressionType.Subtract && operation != ExpressionType.AddAssign)
					{
						goto IL_178;
					}
				}
				else if (operation != ExpressionType.DivideAssign && operation != ExpressionType.MultiplyAssign && operation != ExpressionType.SubtractAssign)
				{
					goto IL_178;
				}
				if (JValue.Operation(binder.Operation, instance.Value, objB, out result))
				{
					result = new JValue(result);
					return true;
				}
				IL_178:
				result = null;
				return false;
			}
		}
	}
}
