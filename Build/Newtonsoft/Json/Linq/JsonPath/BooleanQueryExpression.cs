using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002E5 RID: 741
	[NullableContext(1)]
	[Nullable(0)]
	internal class BooleanQueryExpression : QueryExpression
	{
		// Token: 0x06001773 RID: 6003 RVA: 0x000170C6 File Offset: 0x000152C6
		public BooleanQueryExpression(QueryOperator @operator, object left, [Nullable(2)] object right) : base(@operator)
		{
			this.Left = left;
			this.Right = right;
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x000723A8 File Offset: 0x000705A8
		private IEnumerable<JToken> GetResult(JToken root, JToken t, [Nullable(2)] object o)
		{
			JToken jtoken = o as JToken;
			if (jtoken != null)
			{
				return new JToken[]
				{
					jtoken
				};
			}
			List<PathFilter> list = o as List<PathFilter>;
			if (list != null)
			{
				return JPath.Evaluate(list, root, t, false);
			}
			return CollectionUtils.ArrayEmpty<JToken>();
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x000723E4 File Offset: 0x000705E4
		public override bool IsMatch(JToken root, JToken t)
		{
			if (this.Operator == QueryOperator.Exists)
			{
				return this.GetResult(root, t, this.Left).Any<JToken>();
			}
			using (IEnumerator<JToken> enumerator = this.GetResult(root, t, this.Left).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					IEnumerable<JToken> result = this.GetResult(root, t, this.Right);
					ICollection<JToken> collection = (result as ICollection<JToken>) ?? result.ToList<JToken>();
					do
					{
						JToken leftResult = enumerator.Current;
						foreach (JToken rightResult in collection)
						{
							if (this.MatchTokens(leftResult, rightResult))
							{
								return true;
							}
						}
					}
					while (enumerator.MoveNext());
				}
			}
			return false;
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x000724C4 File Offset: 0x000706C4
		private bool MatchTokens(JToken leftResult, JToken rightResult)
		{
			JValue jvalue = leftResult as JValue;
			if (jvalue != null)
			{
				JValue jvalue2 = rightResult as JValue;
				if (jvalue2 != null)
				{
					switch (this.Operator)
					{
					case QueryOperator.Equals:
						if (BooleanQueryExpression.EqualsWithStringCoercion(jvalue, jvalue2))
						{
							return true;
						}
						return false;
					case QueryOperator.NotEquals:
						if (!BooleanQueryExpression.EqualsWithStringCoercion(jvalue, jvalue2))
						{
							return true;
						}
						return false;
					case QueryOperator.Exists:
						return true;
					case QueryOperator.LessThan:
						if (jvalue.CompareTo(jvalue2) < 0)
						{
							return true;
						}
						return false;
					case QueryOperator.LessThanOrEquals:
						if (jvalue.CompareTo(jvalue2) <= 0)
						{
							return true;
						}
						return false;
					case QueryOperator.GreaterThan:
						if (jvalue.CompareTo(jvalue2) > 0)
						{
							return true;
						}
						return false;
					case QueryOperator.GreaterThanOrEquals:
						if (jvalue.CompareTo(jvalue2) >= 0)
						{
							return true;
						}
						return false;
					case QueryOperator.And:
					case QueryOperator.Or:
						return false;
					case QueryOperator.RegexEquals:
						if (BooleanQueryExpression.RegexEquals(jvalue, jvalue2))
						{
							return true;
						}
						return false;
					case QueryOperator.StrictEquals:
						if (BooleanQueryExpression.EqualsWithStrictMatch(jvalue, jvalue2))
						{
							return true;
						}
						return false;
					case QueryOperator.StrictNotEquals:
						if (!BooleanQueryExpression.EqualsWithStrictMatch(jvalue, jvalue2))
						{
							return true;
						}
						return false;
					default:
						return false;
					}
				}
			}
			QueryOperator @operator = this.Operator;
			if (@operator - QueryOperator.NotEquals <= 1)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x000725A8 File Offset: 0x000707A8
		private static bool RegexEquals(JValue input, JValue pattern)
		{
			if (input.Type == JTokenType.String)
			{
				if (pattern.Type == JTokenType.String)
				{
					string text = (string)pattern.Value;
					int num = text.LastIndexOf('/');
					string pattern2 = text.Substring(1, num - 1);
					string optionsText = text.Substring(num + 1);
					return Regex.IsMatch((string)input.Value, pattern2, MiscellaneousUtils.GetRegexOptions(optionsText));
				}
			}
			return false;
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x0007260C File Offset: 0x0007080C
		internal static bool EqualsWithStringCoercion(JValue value, JValue queryValue)
		{
			if (value.Equals(queryValue))
			{
				return true;
			}
			if ((value.Type == JTokenType.Integer && queryValue.Type == JTokenType.Float) || (value.Type == JTokenType.Float && queryValue.Type == JTokenType.Integer))
			{
				return JValue.Compare(value.Type, value.Value, queryValue.Value) == 0;
			}
			if (queryValue.Type != JTokenType.String)
			{
				return false;
			}
			string b = (string)queryValue.Value;
			string a;
			switch (value.Type)
			{
			case JTokenType.Date:
				using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
				{
					object value2 = value.Value;
					if (value2 is DateTimeOffset)
					{
						DateTimeOffset value3 = (DateTimeOffset)value2;
						DateTimeUtils.WriteDateTimeOffsetString(stringWriter, value3, DateFormatHandling.IsoDateFormat, null, CultureInfo.InvariantCulture);
					}
					else
					{
						DateTimeUtils.WriteDateTimeString(stringWriter, (DateTime)value.Value, DateFormatHandling.IsoDateFormat, null, CultureInfo.InvariantCulture);
					}
					a = stringWriter.ToString();
					goto IL_11E;
				}
				break;
			case JTokenType.Bytes:
				a = Convert.ToBase64String((byte[])value.Value);
				goto IL_11E;
			case JTokenType.Guid:
			case JTokenType.TimeSpan:
				a = value.Value.ToString();
				goto IL_11E;
			case JTokenType.Uri:
				a = ((Uri)value.Value).OriginalString;
				goto IL_11E;
			}
			return false;
			IL_11E:
			return string.Equals(a, b, StringComparison.Ordinal);
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x00072750 File Offset: 0x00070950
		internal static bool EqualsWithStrictMatch(JValue value, JValue queryValue)
		{
			if ((value.Type == JTokenType.Integer && queryValue.Type == JTokenType.Float) || (value.Type == JTokenType.Float && queryValue.Type == JTokenType.Integer))
			{
				return JValue.Compare(value.Type, value.Value, queryValue.Value) == 0;
			}
			return value.Type == queryValue.Type && value.Equals(queryValue);
		}

		// Token: 0x04000CA0 RID: 3232
		public readonly object Left;

		// Token: 0x04000CA1 RID: 3233
		[Nullable(2)]
		public readonly object Right;
	}
}
