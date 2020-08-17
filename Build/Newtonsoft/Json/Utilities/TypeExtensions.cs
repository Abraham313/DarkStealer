using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000245 RID: 581
	[NullableContext(1)]
	[Nullable(0)]
	internal static class TypeExtensions
	{
		// Token: 0x06001079 RID: 4217 RVA: 0x00012677 File Offset: 0x00010877
		public static MethodInfo Method(this Delegate d)
		{
			return d.Method;
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0001267F File Offset: 0x0001087F
		public static MemberTypes MemberType(this MemberInfo memberInfo)
		{
			return memberInfo.MemberType;
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00012687 File Offset: 0x00010887
		public static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0001268F File Offset: 0x0001088F
		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00012697 File Offset: 0x00010897
		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0001269F File Offset: 0x0001089F
		public static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x000126A7 File Offset: 0x000108A7
		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x000126AF File Offset: 0x000108AF
		public static Assembly Assembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x000126B7 File Offset: 0x000108B7
		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x000126BF File Offset: 0x000108BF
		public static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x000126C7 File Offset: 0x000108C7
		public static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x000126CF File Offset: 0x000108CF
		public static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x000126D7 File Offset: 0x000108D7
		public static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x000126DF File Offset: 0x000108DF
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x000126E7 File Offset: 0x000108E7
		public static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive;
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0005E338 File Offset: 0x0005C538
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces, [Nullable(2)] [NotNullWhen(true)] out Type match)
		{
			Type type2 = type;
			while (type2 != null)
			{
				if (string.Equals(type2.FullName, fullTypeName, StringComparison.Ordinal))
				{
					match = type2;
					return true;
				}
				type2 = type2.BaseType();
			}
			if (searchInterfaces)
			{
				Type[] interfaces = type.GetInterfaces();
				for (int i = 0; i < interfaces.Length; i++)
				{
					if (string.Equals(interfaces[i].Name, fullTypeName, StringComparison.Ordinal))
					{
						match = type;
						return true;
					}
				}
			}
			match = null;
			return false;
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0005E3A8 File Offset: 0x0005C5A8
		public static bool AssignableToTypeName(this Type type, string fullTypeName, bool searchInterfaces)
		{
			Type type2;
			return type.AssignableToTypeName(fullTypeName, searchInterfaces, out type2);
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0005E3C0 File Offset: 0x0005C5C0
		public static bool ImplementInterface(this Type type, Type interfaceType)
		{
			Type type2 = type;
			while (type2 != null)
			{
				foreach (Type type3 in ((IEnumerable<Type>)type2.GetInterfaces()))
				{
					if (type3 == interfaceType || (type3 != null && type3.ImplementInterface(interfaceType)))
					{
						return true;
					}
				}
				type2 = type2.BaseType();
			}
			return false;
		}
	}
}
