using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000237 RID: 567
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ReflectionUtils
	{
		// Token: 0x0600100B RID: 4107 RVA: 0x0005CDD4 File Offset: 0x0005AFD4
		public static bool IsVirtual(this PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			MethodInfo methodInfo = propertyInfo.GetGetMethod(true);
			if (methodInfo != null && methodInfo.IsVirtual)
			{
				return true;
			}
			methodInfo = propertyInfo.GetSetMethod(true);
			return methodInfo != null && methodInfo.IsVirtual;
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0005CE24 File Offset: 0x0005B024
		[return: Nullable(2)]
		public static MethodInfo GetBaseDefinition(this PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			if (getMethod != null)
			{
				return getMethod.GetBaseDefinition();
			}
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			if (setMethod == null)
			{
				return null;
			}
			return setMethod.GetBaseDefinition();
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0005CE68 File Offset: 0x0005B068
		public static bool IsPublic(PropertyInfo property)
		{
			MethodInfo getMethod = property.GetGetMethod();
			if (getMethod != null && getMethod.IsPublic)
			{
				return true;
			}
			MethodInfo setMethod = property.GetSetMethod();
			return setMethod != null && setMethod.IsPublic;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x000121CC File Offset: 0x000103CC
		[NullableContext(2)]
		public static Type GetObjectType(object v)
		{
			if (v == null)
			{
				return null;
			}
			return v.GetType();
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0005CEAC File Offset: 0x0005B0AC
		public static string GetTypeName(Type t, TypeNameAssemblyFormatHandling assemblyFormat, [Nullable(2)] ISerializationBinder binder)
		{
			string fullyQualifiedTypeName = ReflectionUtils.GetFullyQualifiedTypeName(t, binder);
			if (assemblyFormat == TypeNameAssemblyFormatHandling.Simple)
			{
				return ReflectionUtils.RemoveAssemblyDetails(fullyQualifiedTypeName);
			}
			if (assemblyFormat != TypeNameAssemblyFormatHandling.Full)
			{
				throw new ArgumentOutOfRangeException();
			}
			return fullyQualifiedTypeName;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0005CED8 File Offset: 0x0005B0D8
		private static string GetFullyQualifiedTypeName(Type t, [Nullable(2)] ISerializationBinder binder)
		{
			if (binder != null)
			{
				string text;
				string str;
				binder.BindToName(t, out text, out str);
				return str + ((text == null) ? "" : (", " + text));
			}
			return t.AssemblyQualifiedName;
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0005CF18 File Offset: 0x0005B118
		private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool flag2 = false;
			foreach (char c in fullyQualifiedTypeName)
			{
				if (c != ',')
				{
					if (c != '[')
					{
						if (c != ']')
						{
							if (!flag2)
							{
								stringBuilder.Append(c);
								goto IL_5A;
							}
							goto IL_5A;
						}
					}
					flag = false;
					flag2 = false;
					stringBuilder.Append(c);
				}
				else if (!flag)
				{
					flag = true;
					stringBuilder.Append(c);
				}
				else
				{
					flag2 = true;
				}
				IL_5A:;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x000121D9 File Offset: 0x000103D9
		public static bool HasDefaultConstructor(Type t, bool nonPublic)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.IsValueType() || ReflectionUtils.GetDefaultConstructor(t, nonPublic) != null;
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x000121FD File Offset: 0x000103FD
		public static ConstructorInfo GetDefaultConstructor(Type t)
		{
			return ReflectionUtils.GetDefaultConstructor(t, false);
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0005CF94 File Offset: 0x0005B194
		public static ConstructorInfo GetDefaultConstructor(Type t, bool nonPublic)
		{
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
			if (nonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			return t.GetConstructors(bindingFlags).SingleOrDefault((ConstructorInfo c) => !c.GetParameters().Any<ParameterInfo>());
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00012206 File Offset: 0x00010406
		public static bool IsNullable(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return !t.IsValueType() || ReflectionUtils.IsNullableType(t);
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00012223 File Offset: 0x00010423
		public static bool IsNullableType(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.IsGenericType() && t.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0001224F File Offset: 0x0001044F
		public static Type EnsureNotNullableType(Type t)
		{
			if (!ReflectionUtils.IsNullableType(t))
			{
				return t;
			}
			return Nullable.GetUnderlyingType(t);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00012261 File Offset: 0x00010461
		public static Type EnsureNotByRefType(Type t)
		{
			if (t.IsByRef && t.HasElementType)
			{
				return t.GetElementType();
			}
			return t;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0001227B File Offset: 0x0001047B
		public static bool IsGenericDefinition(Type type, Type genericInterfaceDefinition)
		{
			return type.IsGenericType() && type.GetGenericTypeDefinition() == genericInterfaceDefinition;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0005CFD8 File Offset: 0x0005B1D8
		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
		{
			Type type2;
			return ReflectionUtils.ImplementsGenericDefinition(type, genericInterfaceDefinition, out type2);
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0005CFF0 File Offset: 0x0005B1F0
		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, [Nullable(2)] [NotNullWhen(true)] out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericInterfaceDefinition, "genericInterfaceDefinition");
			if (genericInterfaceDefinition.IsInterface() && genericInterfaceDefinition.IsGenericTypeDefinition())
			{
				if (type.IsInterface() && type.IsGenericType())
				{
					Type genericTypeDefinition = type.GetGenericTypeDefinition();
					if (genericInterfaceDefinition == genericTypeDefinition)
					{
						implementingType = type;
						return true;
					}
				}
				foreach (Type type2 in type.GetInterfaces())
				{
					if (type2.IsGenericType())
					{
						Type genericTypeDefinition2 = type2.GetGenericTypeDefinition();
						if (genericInterfaceDefinition == genericTypeDefinition2)
						{
							implementingType = type2;
							return true;
						}
					}
				}
				implementingType = null;
				return false;
			}
			throw new ArgumentNullException("'{0}' is not a generic interface definition.".FormatWith(CultureInfo.InvariantCulture, genericInterfaceDefinition));
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0005D0A0 File Offset: 0x0005B2A0
		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition)
		{
			Type type2;
			return ReflectionUtils.InheritsGenericDefinition(type, genericClassDefinition, out type2);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0005D0B8 File Offset: 0x0005B2B8
		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition, [Nullable(2)] out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericClassDefinition, "genericClassDefinition");
			if (!genericClassDefinition.IsClass() || !genericClassDefinition.IsGenericTypeDefinition())
			{
				throw new ArgumentNullException("'{0}' is not a generic class definition.".FormatWith(CultureInfo.InvariantCulture, genericClassDefinition));
			}
			return ReflectionUtils.InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00012293 File Offset: 0x00010493
		private static bool InheritsGenericDefinitionInternal(Type currentType, Type genericClassDefinition, [Nullable(2)] out Type implementingType)
		{
			while (!currentType.IsGenericType() || !(genericClassDefinition == currentType.GetGenericTypeDefinition()))
			{
				currentType = currentType.BaseType();
				if (!(currentType != null))
				{
					implementingType = null;
					return false;
				}
			}
			implementingType = currentType;
			return true;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0005D10C File Offset: 0x0005B30C
		[return: Nullable(2)]
		public static Type GetCollectionItemType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsArray)
			{
				return type.GetElementType();
			}
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(type, typeof(IEnumerable<>), out type2))
			{
				if (type2.IsGenericTypeDefinition())
				{
					throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, type));
				}
				return type2.GetGenericArguments()[0];
			}
			else
			{
				if (!typeof(IEnumerable).IsAssignableFrom(type))
				{
					throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, type));
				}
				return null;
			}
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0005D19C File Offset: 0x0005B39C
		[NullableContext(2)]
		public static void GetDictionaryKeyValueTypes([Nullable(1)] Type dictionaryType, out Type keyType, out Type valueType)
		{
			ValidationUtils.ArgumentNotNull(dictionaryType, "dictionaryType");
			Type type;
			if (ReflectionUtils.ImplementsGenericDefinition(dictionaryType, typeof(IDictionary<, >), out type))
			{
				if (type.IsGenericTypeDefinition())
				{
					throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, dictionaryType));
				}
				Type[] genericArguments = type.GetGenericArguments();
				keyType = genericArguments[0];
				valueType = genericArguments[1];
				return;
			}
			else
			{
				if (!typeof(IDictionary).IsAssignableFrom(dictionaryType))
				{
					throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, dictionaryType));
				}
				keyType = null;
				valueType = null;
				return;
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0005D230 File Offset: 0x0005B430
		public static Type GetMemberUnderlyingType(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			MemberTypes memberTypes = member.MemberType();
			if (memberTypes <= MemberTypes.Field)
			{
				if (memberTypes == MemberTypes.Event)
				{
					return ((EventInfo)member).EventHandlerType;
				}
				if (memberTypes == MemberTypes.Field)
				{
					return ((FieldInfo)member).FieldType;
				}
			}
			else
			{
				if (memberTypes == MemberTypes.Method)
				{
					return ((MethodInfo)member).ReturnType;
				}
				if (memberTypes == MemberTypes.Property)
				{
					return ((PropertyInfo)member).PropertyType;
				}
			}
			throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo, EventInfo or MethodInfo", "member");
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0005D2A8 File Offset: 0x0005B4A8
		public static bool IsByRefLikeType(Type type)
		{
			if (!type.IsValueType())
			{
				return false;
			}
			Attribute[] attributes = ReflectionUtils.GetAttributes(type, null, false);
			for (int i = 0; i < attributes.Length; i++)
			{
				if (string.Equals(attributes[i].GetType().FullName, "System.Runtime.CompilerServices.IsByRefLikeAttribute", StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x000122C7 File Offset: 0x000104C7
		public static bool IsIndexedProperty(PropertyInfo property)
		{
			ValidationUtils.ArgumentNotNull(property, "property");
			return property.GetIndexParameters().Length != 0;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0005D2F8 File Offset: 0x0005B4F8
		public static object GetMemberValue(MemberInfo member, object target)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberTypes = member.MemberType();
			if (memberTypes == MemberTypes.Field)
			{
				return ((FieldInfo)member).GetValue(target);
			}
			if (memberTypes != MemberTypes.Property)
			{
				throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, member.Name), "member");
			}
			object value;
			try
			{
				value = ((PropertyInfo)member).GetValue(target, null);
			}
			catch (TargetParameterCountException innerException)
			{
				throw new ArgumentException("MemberInfo '{0}' has index parameters".FormatWith(CultureInfo.InvariantCulture, member.Name), innerException);
			}
			return value;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0005D398 File Offset: 0x0005B598
		public static void SetMemberValue(MemberInfo member, object target, [Nullable(2)] object value)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberTypes = member.MemberType();
			if (memberTypes == MemberTypes.Field)
			{
				((FieldInfo)member).SetValue(target, value);
				return;
			}
			if (memberTypes != MemberTypes.Property)
			{
				throw new ArgumentException("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, member.Name), "member");
			}
			((PropertyInfo)member).SetValue(target, value, null);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0005D408 File Offset: 0x0005B608
		public static bool CanReadMemberValue(MemberInfo member, bool nonPublic)
		{
			MemberTypes memberTypes = member.MemberType();
			if (memberTypes == MemberTypes.Field)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				return nonPublic || fieldInfo.IsPublic;
			}
			if (memberTypes != MemberTypes.Property)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)member;
			return propertyInfo.CanRead && (nonPublic || propertyInfo.GetGetMethod(nonPublic) != null);
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0005D464 File Offset: 0x0005B664
		public static bool CanSetMemberValue(MemberInfo member, bool nonPublic, bool canSetReadOnly)
		{
			MemberTypes memberTypes = member.MemberType();
			if (memberTypes == MemberTypes.Field)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				return !fieldInfo.IsLiteral && (!fieldInfo.IsInitOnly || canSetReadOnly) && (nonPublic || fieldInfo.IsPublic);
			}
			if (memberTypes != MemberTypes.Property)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)member;
			return propertyInfo.CanWrite && (nonPublic || propertyInfo.GetSetMethod(nonPublic) != null);
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0005D4D8 File Offset: 0x0005B6D8
		public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
		{
			List<MemberInfo> list = new List<MemberInfo>();
			list.AddRange(ReflectionUtils.GetFields(type, bindingAttr));
			list.AddRange(ReflectionUtils.GetProperties(type, bindingAttr));
			List<MemberInfo> list2 = new List<MemberInfo>(list.Count);
			foreach (IGrouping<string, MemberInfo> grouping in from m in list
			group m by m.Name)
			{
				if (grouping.Count<MemberInfo>() == 1)
				{
					list2.Add(grouping.First<MemberInfo>());
				}
				else
				{
					List<MemberInfo> list3 = new List<MemberInfo>();
					using (IEnumerator<MemberInfo> enumerator2 = grouping.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							MemberInfo memberInfo = enumerator2.Current;
							if (list3.Count == 0)
							{
								list3.Add(memberInfo);
							}
							else if ((!ReflectionUtils.IsOverridenGenericMember(memberInfo, bindingAttr) || memberInfo.Name == "Item") && !list3.Any((MemberInfo m) => m.DeclaringType == memberInfo.DeclaringType))
							{
								list3.Add(memberInfo);
							}
						}
					}
					list2.AddRange(list3);
				}
			}
			return list2;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0005D63C File Offset: 0x0005B83C
		private static bool IsOverridenGenericMember(MemberInfo memberInfo, BindingFlags bindingAttr)
		{
			if (memberInfo.MemberType() != MemberTypes.Property)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			if (!propertyInfo.IsVirtual())
			{
				return false;
			}
			Type declaringType = propertyInfo.DeclaringType;
			if (!declaringType.IsGenericType())
			{
				return false;
			}
			Type genericTypeDefinition = declaringType.GetGenericTypeDefinition();
			if (genericTypeDefinition == null)
			{
				return false;
			}
			MemberInfo[] member = genericTypeDefinition.GetMember(propertyInfo.Name, bindingAttr);
			return member.Length != 0 && ReflectionUtils.GetMemberUnderlyingType(member[0]).IsGenericParameter;
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x000122DE File Offset: 0x000104DE
		[return: Nullable(2)]
		public static T GetAttribute<[Nullable(0)] T>(object attributeProvider) where T : Attribute
		{
			return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0005D6B4 File Offset: 0x0005B8B4
		[return: Nullable(2)]
		public static T GetAttribute<[Nullable(0)] T>(object attributeProvider, bool inherit) where T : Attribute
		{
			T[] attributes = ReflectionUtils.GetAttributes<T>(attributeProvider, inherit);
			if (attributes == null)
			{
				return default(T);
			}
			return attributes.FirstOrDefault<T>();
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0005D6DC File Offset: 0x0005B8DC
		public static T[] GetAttributes<[Nullable(0)] T>(object attributeProvider, bool inherit) where T : Attribute
		{
			Attribute[] attributes = ReflectionUtils.GetAttributes(attributeProvider, typeof(T), inherit);
			T[] array = attributes as T[];
			if (array != null)
			{
				return array;
			}
			return attributes.Cast<T>().ToArray<T>();
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0005D714 File Offset: 0x0005B914
		public static Attribute[] GetAttributes(object attributeProvider, [Nullable(2)] Type attributeType, bool inherit)
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			Type type = attributeProvider as Type;
			if (type != null)
			{
				return ((attributeType != null) ? type.GetCustomAttributes(attributeType, inherit) : type.GetCustomAttributes(inherit)).Cast<Attribute>().ToArray<Attribute>();
			}
			Assembly assembly = attributeProvider as Assembly;
			if (assembly == null)
			{
				MemberInfo memberInfo = attributeProvider as MemberInfo;
				if (memberInfo == null)
				{
					Module module = attributeProvider as Module;
					if (module == null)
					{
						ParameterInfo parameterInfo = attributeProvider as ParameterInfo;
						if (parameterInfo == null)
						{
							ICustomAttributeProvider customAttributeProvider = (ICustomAttributeProvider)attributeProvider;
							return (Attribute[])((attributeType != null) ? customAttributeProvider.GetCustomAttributes(attributeType, inherit) : customAttributeProvider.GetCustomAttributes(inherit));
						}
						if (!(attributeType != null))
						{
							return Attribute.GetCustomAttributes(parameterInfo, inherit);
						}
						return Attribute.GetCustomAttributes(parameterInfo, attributeType, inherit);
					}
					else
					{
						if (!(attributeType != null))
						{
							return Attribute.GetCustomAttributes(module, inherit);
						}
						return Attribute.GetCustomAttributes(module, attributeType, inherit);
					}
				}
				else
				{
					if (!(attributeType != null))
					{
						return Attribute.GetCustomAttributes(memberInfo, inherit);
					}
					return Attribute.GetCustomAttributes(memberInfo, attributeType, inherit);
				}
			}
			else
			{
				if (!(attributeType != null))
				{
					return Attribute.GetCustomAttributes(assembly);
				}
				return Attribute.GetCustomAttributes(assembly, attributeType);
			}
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0005D824 File Offset: 0x0005BA24
		[return: Nullable(new byte[]
		{
			0,
			2,
			1
		})]
		public static StructMultiKey<string, string> SplitFullyQualifiedTypeName(string fullyQualifiedTypeName)
		{
			int? assemblyDelimiterIndex = ReflectionUtils.GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
			string v;
			string v2;
			if (assemblyDelimiterIndex != null)
			{
				v = fullyQualifiedTypeName.Trim(0, assemblyDelimiterIndex.GetValueOrDefault());
				v2 = fullyQualifiedTypeName.Trim(assemblyDelimiterIndex.GetValueOrDefault() + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.GetValueOrDefault() - 1);
			}
			else
			{
				v = fullyQualifiedTypeName;
				v2 = null;
			}
			return new StructMultiKey<string, string>(v2, v);
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0005D880 File Offset: 0x0005BA80
		private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
		{
			int num = 0;
			for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
			{
				char c = fullyQualifiedTypeName[i];
				if (c != ',')
				{
					if (c != '[')
					{
						if (c == ']')
						{
							num--;
						}
					}
					else
					{
						num++;
					}
				}
				else if (num == 0)
				{
					return new int?(i);
				}
			}
			return null;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0005D8D8 File Offset: 0x0005BAD8
		public static MemberInfo GetMemberInfoFromType(Type targetType, MemberInfo memberInfo)
		{
			if (memberInfo.MemberType() == MemberTypes.Property)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				Type[] types = (from p in propertyInfo.GetIndexParameters()
				select p.ParameterType).ToArray<Type>();
				return targetType.GetProperty(propertyInfo.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, propertyInfo.PropertyType, types, null);
			}
			return targetType.GetMember(memberInfo.Name, memberInfo.MemberType(), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SingleOrDefault<MemberInfo>();
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x000122E7 File Offset: 0x000104E7
		public static IEnumerable<FieldInfo> GetFields(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<MemberInfo> list = new List<MemberInfo>(targetType.GetFields(bindingAttr));
			ReflectionUtils.GetChildPrivateFields(list, targetType, bindingAttr);
			return list.Cast<FieldInfo>();
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0005D958 File Offset: 0x0005BB58
		private static void GetChildPrivateFields(IList<MemberInfo> initialFields, Type targetType, BindingFlags bindingAttr)
		{
			if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
			{
				BindingFlags bindingAttr2 = bindingAttr.RemoveFlag(BindingFlags.Public);
				while ((targetType = targetType.BaseType()) != null)
				{
					IEnumerable<FieldInfo> collection = from f in targetType.GetFields(bindingAttr2)
					where f.IsPrivate
					select f;
					initialFields.AddRange(collection);
				}
			}
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0005D9BC File Offset: 0x0005BBBC
		public static IEnumerable<PropertyInfo> GetProperties(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<PropertyInfo> list = new List<PropertyInfo>(targetType.GetProperties(bindingAttr));
			if (targetType.IsInterface())
			{
				foreach (Type type in targetType.GetInterfaces())
				{
					list.AddRange(type.GetProperties(bindingAttr));
				}
			}
			ReflectionUtils.GetChildPrivateProperties(list, targetType, bindingAttr);
			for (int j = 0; j < list.Count; j++)
			{
				PropertyInfo propertyInfo = list[j];
				if (propertyInfo.DeclaringType != targetType)
				{
					PropertyInfo value = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(propertyInfo.DeclaringType, propertyInfo);
					list[j] = value;
				}
			}
			return list;
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0001230D File Offset: 0x0001050D
		public static BindingFlags RemoveFlag(this BindingFlags bindingAttr, BindingFlags flag)
		{
			if ((bindingAttr & flag) != flag)
			{
				return bindingAttr;
			}
			return bindingAttr ^ flag;
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0005DA6C File Offset: 0x0005BC6C
		private static void GetChildPrivateProperties(IList<PropertyInfo> initialProperties, Type targetType, BindingFlags bindingAttr)
		{
			while ((targetType = targetType.BaseType()) != null)
			{
				foreach (PropertyInfo subTypeProperty in targetType.GetProperties(bindingAttr))
				{
					ReflectionUtils.<>c__DisplayClass44_0 CS$<>8__locals1 = new ReflectionUtils.<>c__DisplayClass44_0();
					CS$<>8__locals1.subTypeProperty = subTypeProperty;
					if (CS$<>8__locals1.subTypeProperty.IsVirtual())
					{
						ReflectionUtils.<>c__DisplayClass44_1 CS$<>8__locals2 = new ReflectionUtils.<>c__DisplayClass44_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						ReflectionUtils.<>c__DisplayClass44_1 CS$<>8__locals3 = CS$<>8__locals2;
						MethodInfo baseDefinition = CS$<>8__locals2.CS$<>8__locals1.subTypeProperty.GetBaseDefinition();
						if (baseDefinition == null)
						{
							goto IL_E7;
						}
						Type declaringType;
						if ((declaringType = baseDefinition.DeclaringType) == null)
						{
							goto IL_E7;
						}
						IL_F9:
						CS$<>8__locals3.subTypePropertyDeclaringType = declaringType;
						if (initialProperties.IndexOf(delegate(PropertyInfo p)
						{
							if (p.Name == CS$<>8__locals2.CS$<>8__locals1.subTypeProperty.Name && p.IsVirtual())
							{
								MethodInfo baseDefinition2 = p.GetBaseDefinition();
								Type declaringType2;
								if (baseDefinition2 != null)
								{
									if ((declaringType2 = baseDefinition2.DeclaringType) != null)
									{
										goto IL_41;
									}
								}
								declaringType2 = p.DeclaringType;
								IL_41:
								return declaringType2.IsAssignableFrom(CS$<>8__locals2.subTypePropertyDeclaringType);
							}
							return false;
						}) == -1)
						{
							initialProperties.Add(CS$<>8__locals2.CS$<>8__locals1.subTypeProperty);
							goto IL_126;
						}
						goto IL_126;
						IL_E7:
						declaringType = CS$<>8__locals2.CS$<>8__locals1.subTypeProperty.DeclaringType;
						goto IL_F9;
					}
					if (!ReflectionUtils.IsPublic(CS$<>8__locals1.subTypeProperty))
					{
						int num = initialProperties.IndexOf((PropertyInfo p) => p.Name == CS$<>8__locals1.subTypeProperty.Name);
						if (num == -1)
						{
							initialProperties.Add(CS$<>8__locals1.subTypeProperty);
						}
						else if (!ReflectionUtils.IsPublic(initialProperties[num]))
						{
							initialProperties[num] = CS$<>8__locals1.subTypeProperty;
						}
					}
					else if (initialProperties.IndexOf((PropertyInfo p) => p.Name == CS$<>8__locals1.subTypeProperty.Name && p.DeclaringType == CS$<>8__locals1.subTypeProperty.DeclaringType) == -1)
					{
						initialProperties.Add(CS$<>8__locals1.subTypeProperty);
					}
					IL_126:;
				}
			}
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0005DBC0 File Offset: 0x0005BDC0
		public static bool IsMethodOverridden(Type currentType, Type methodDeclaringType, string method)
		{
			return currentType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any((MethodInfo info) => info.Name == method && info.DeclaringType != methodDeclaringType && info.GetBaseDefinition().DeclaringType == methodDeclaringType);
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0005DBFC File Offset: 0x0005BDFC
		[return: Nullable(2)]
		public static object GetDefaultValue(Type type)
		{
			if (!type.IsValueType())
			{
				return null;
			}
			PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(type);
			switch (typeCode)
			{
			case PrimitiveTypeCode.Char:
			case PrimitiveTypeCode.SByte:
			case PrimitiveTypeCode.Int16:
			case PrimitiveTypeCode.UInt16:
			case PrimitiveTypeCode.Int32:
			case PrimitiveTypeCode.Byte:
			case PrimitiveTypeCode.UInt32:
				return 0;
			case PrimitiveTypeCode.CharNullable:
			case PrimitiveTypeCode.BooleanNullable:
			case PrimitiveTypeCode.SByteNullable:
			case PrimitiveTypeCode.Int16Nullable:
			case PrimitiveTypeCode.UInt16Nullable:
			case PrimitiveTypeCode.Int32Nullable:
			case PrimitiveTypeCode.ByteNullable:
			case PrimitiveTypeCode.UInt32Nullable:
			case PrimitiveTypeCode.Int64Nullable:
			case PrimitiveTypeCode.UInt64Nullable:
			case PrimitiveTypeCode.SingleNullable:
			case PrimitiveTypeCode.DoubleNullable:
			case PrimitiveTypeCode.DateTimeNullable:
			case PrimitiveTypeCode.DateTimeOffsetNullable:
			case PrimitiveTypeCode.DecimalNullable:
				break;
			case PrimitiveTypeCode.Boolean:
				return false;
			case PrimitiveTypeCode.Int64:
			case PrimitiveTypeCode.UInt64:
				return 0L;
			case PrimitiveTypeCode.Single:
				return 0f;
			case PrimitiveTypeCode.Double:
				return 0.0;
			case PrimitiveTypeCode.DateTime:
				return default(DateTime);
			case PrimitiveTypeCode.DateTimeOffset:
				return default(DateTimeOffset);
			case PrimitiveTypeCode.Decimal:
				return 0m;
			case PrimitiveTypeCode.Guid:
				return default(Guid);
			default:
				if (typeCode == PrimitiveTypeCode.BigInteger)
				{
					return default(System.Numerics.BigInteger);
				}
				break;
			}
			if (ReflectionUtils.IsNullable(type))
			{
				return null;
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x04000A00 RID: 2560
		public static readonly Type[] EmptyTypes = Type.EmptyTypes;
	}
}
