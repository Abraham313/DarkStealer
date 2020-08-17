using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000231 RID: 561
	[NullableContext(1)]
	[Nullable(0)]
	internal abstract class ReflectionDelegateFactory
	{
		// Token: 0x06000FEB RID: 4075 RVA: 0x0005CB18 File Offset: 0x0005AD18
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public Func<T, object> CreateGet<[Nullable(2)] T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				if (propertyInfo.PropertyType.IsByRef)
				{
					throw new InvalidOperationException("Could not create getter for {0}. ByRef return values are not supported.".FormatWith(CultureInfo.InvariantCulture, propertyInfo));
				}
				return this.CreateGet<T>(propertyInfo);
			}
			else
			{
				FieldInfo fieldInfo = memberInfo as FieldInfo;
				if (fieldInfo == null)
				{
					throw new Exception("Could not create getter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
				}
				return this.CreateGet<T>(fieldInfo);
			}
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0005CB84 File Offset: 0x0005AD84
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public Action<T, object> CreateSet<[Nullable(2)] T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return this.CreateSet<T>(propertyInfo);
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo == null)
			{
				throw new Exception("Could not create setter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
			}
			return this.CreateSet<T>(fieldInfo);
		}

		// Token: 0x06000FED RID: 4077
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public abstract MethodCall<T, object> CreateMethodCall<[Nullable(2)] T>(MethodBase method);

		// Token: 0x06000FEE RID: 4078
		public abstract ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method);

		// Token: 0x06000FEF RID: 4079
		public abstract Func<T> CreateDefaultConstructor<[Nullable(2)] T>(Type type);

		// Token: 0x06000FF0 RID: 4080
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public abstract Func<T, object> CreateGet<[Nullable(2)] T>(PropertyInfo propertyInfo);

		// Token: 0x06000FF1 RID: 4081
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public abstract Func<T, object> CreateGet<[Nullable(2)] T>(FieldInfo fieldInfo);

		// Token: 0x06000FF2 RID: 4082
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public abstract Action<T, object> CreateSet<[Nullable(2)] T>(FieldInfo fieldInfo);

		// Token: 0x06000FF3 RID: 4083
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public abstract Action<T, object> CreateSet<[Nullable(2)] T>(PropertyInfo propertyInfo);
	}
}
