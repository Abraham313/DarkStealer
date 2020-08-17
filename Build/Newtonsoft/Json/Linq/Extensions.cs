using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002A9 RID: 681
	[NullableContext(1)]
	[Nullable(0)]
	public static class Extensions
	{
		// Token: 0x06001440 RID: 5184 RVA: 0x00015073 File Offset: 0x00013273
		public static IJEnumerable<JToken> Ancestors<[Nullable(0)] T>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((T j) => j.Ancestors()).AsJEnumerable();
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x000150AA File Offset: 0x000132AA
		public static IJEnumerable<JToken> AncestorsAndSelf<[Nullable(0)] T>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((T j) => j.AncestorsAndSelf()).AsJEnumerable();
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x000150E1 File Offset: 0x000132E1
		public static IJEnumerable<JToken> Descendants<[Nullable(0)] T>(this IEnumerable<T> source) where T : JContainer
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((T j) => j.Descendants()).AsJEnumerable();
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00015118 File Offset: 0x00013318
		public static IJEnumerable<JToken> DescendantsAndSelf<[Nullable(0)] T>(this IEnumerable<T> source) where T : JContainer
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((T j) => j.DescendantsAndSelf()).AsJEnumerable();
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0001514F File Offset: 0x0001334F
		public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((JObject d) => d.Properties()).AsJEnumerable<JProperty>();
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00015186 File Offset: 0x00013386
		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, [Nullable(2)] object key)
		{
			return source.Values(key).AsJEnumerable();
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00015194 File Offset: 0x00013394
		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source)
		{
			return source.Values(null);
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0001519D File Offset: 0x0001339D
		public static IEnumerable<U> Values<[Nullable(2)] U>(this IEnumerable<JToken> source, object key)
		{
			return source.Values(key);
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x000151A6 File Offset: 0x000133A6
		public static IEnumerable<U> Values<[Nullable(2)] U>(this IEnumerable<JToken> source)
		{
			return source.Values(null);
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x000151AF File Offset: 0x000133AF
		public static U Value<[Nullable(2)] U>(this IEnumerable<JToken> value)
		{
			return value.Value<JToken, U>();
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x000151B7 File Offset: 0x000133B7
		public static U Value<[Nullable(0)] T, [Nullable(2)] U>(this IEnumerable<T> value) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JToken jtoken = value as JToken;
			if (jtoken == null)
			{
				throw new ArgumentException("Source value must be a JToken.");
			}
			return jtoken.Convert<JToken, U>();
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x000151DD File Offset: 0x000133DD
		internal static IEnumerable<U> Values<[Nullable(0)] T, [Nullable(2)] U>(this IEnumerable<T> source, [Nullable(2)] object key) where T : JToken
		{
			Extensions.<Values>d__11<T, U> <Values>d__ = new Extensions.<Values>d__11<T, U>(-2);
			<Values>d__.<>3__source = source;
			<Values>d__.<>3__key = key;
			return <Values>d__;
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x000151F4 File Offset: 0x000133F4
		public static IJEnumerable<JToken> Children<[Nullable(0)] T>(this IEnumerable<T> source) where T : JToken
		{
			return source.Children<T, JToken>().AsJEnumerable();
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00015201 File Offset: 0x00013401
		public static IEnumerable<U> Children<[Nullable(0)] T, [Nullable(2)] U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((T c) => c.Children()).Convert<JToken, U>();
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x00015238 File Offset: 0x00013438
		internal static IEnumerable<U> Convert<[Nullable(0)] T, [Nullable(2)] U>(this IEnumerable<T> source) where T : JToken
		{
			Extensions.<Convert>d__14<T, U> <Convert>d__ = new Extensions.<Convert>d__14<T, U>(-2);
			<Convert>d__.<>3__source = source;
			return <Convert>d__;
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0006A2A0 File Offset: 0x000684A0
		[return: MaybeNull]
		internal static U Convert<[Nullable(0)] T, [Nullable(2)] U>(this T token) where T : JToken
		{
			if (token == null)
			{
				return default(U);
			}
			if (token is U)
			{
				U result = token as U;
				if (typeof(U) != typeof(IComparable) && typeof(U) != typeof(IFormattable))
				{
					return result;
				}
			}
			JValue jvalue = token as JValue;
			if (jvalue == null)
			{
				throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, token.GetType(), typeof(T)));
			}
			object value = jvalue.Value;
			if (value is U)
			{
				return (U)((object)value);
			}
			Type type = typeof(U);
			if (ReflectionUtils.IsNullableType(type))
			{
				if (jvalue.Value == null)
				{
					return default(U);
				}
				type = Nullable.GetUnderlyingType(type);
			}
			return (U)((object)System.Convert.ChangeType(jvalue.Value, type, CultureInfo.InvariantCulture));
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x00015248 File Offset: 0x00013448
		public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source)
		{
			return source.AsJEnumerable<JToken>();
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0006A3B0 File Offset: 0x000685B0
		public static IJEnumerable<T> AsJEnumerable<[Nullable(0)] T>(this IEnumerable<T> source) where T : JToken
		{
			if (source == null)
			{
				return null;
			}
			IJEnumerable<T> ijenumerable = source as IJEnumerable<T>;
			if (ijenumerable != null)
			{
				return ijenumerable;
			}
			return new JEnumerable<T>(source);
		}
	}
}
