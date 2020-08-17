using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000225 RID: 549
	[NullableContext(1)]
	[Nullable(0)]
	internal class LateBoundReflectionDelegateFactory : ReflectionDelegateFactory
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x00011EC9 File Offset: 0x000100C9
		internal static ReflectionDelegateFactory Instance
		{
			get
			{
				return LateBoundReflectionDelegateFactory._instance;
			}
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0005C650 File Offset: 0x0005A850
		public override ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			ConstructorInfo c = method as ConstructorInfo;
			if (c != null)
			{
				return ([Nullable(new byte[]
				{
					1,
					2
				})] object[] a) => c.Invoke(a);
			}
			return ([Nullable(new byte[]
			{
				1,
				2
			})] object[] a) => method.Invoke(null, a);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0005C6AC File Offset: 0x0005A8AC
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public override MethodCall<T, object> CreateMethodCall<[Nullable(2)] T>(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			ConstructorInfo c = method as ConstructorInfo;
			if (c != null)
			{
				return (T o, [Nullable(new byte[]
				{
					1,
					2
				})] object[] a) => c.Invoke(a);
			}
			return (T o, [Nullable(new byte[]
			{
				1,
				2
			})] object[] a) => method.Invoke(o, a);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0005C708 File Offset: 0x0005A908
		public override Func<T> CreateDefaultConstructor<[Nullable(2)] T>(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsValueType())
			{
				return () => (T)((object)Activator.CreateInstance(type));
			}
			ConstructorInfo constructorInfo = ReflectionUtils.GetDefaultConstructor(type, true);
			return () => (T)((object)constructorInfo.Invoke(null));
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00011ED0 File Offset: 0x000100D0
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public override Func<T, object> CreateGet<[Nullable(2)] T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return (T o) => propertyInfo.GetValue(o, null);
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00011EF9 File Offset: 0x000100F9
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public override Func<T, object> CreateGet<[Nullable(2)] T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return (T o) => fieldInfo.GetValue(o);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00011F22 File Offset: 0x00010122
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public override Action<T, object> CreateSet<[Nullable(2)] T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return delegate(T o, [Nullable(2)] object v)
			{
				fieldInfo.SetValue(o, v);
			};
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00011F4B File Offset: 0x0001014B
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public override Action<T, object> CreateSet<[Nullable(2)] T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return delegate(T o, [Nullable(2)] object v)
			{
				propertyInfo.SetValue(o, v, null);
			};
		}

		// Token: 0x040009ED RID: 2541
		private static readonly LateBoundReflectionDelegateFactory _instance = new LateBoundReflectionDelegateFactory();
	}
}
