using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000281 RID: 641
	[NullableContext(1)]
	[Nullable(0)]
	internal static class JsonTypeReflector
	{
		// Token: 0x060012B6 RID: 4790 RVA: 0x00013B74 File Offset: 0x00011D74
		[return: Nullable(2)]
		public static T GetCachedAttribute<[Nullable(0)] T>(object attributeProvider) where T : Attribute
		{
			return CachedAttributeGetter<T>.GetAttribute(attributeProvider);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00066F80 File Offset: 0x00065180
		public static bool CanTypeDescriptorConvertString(Type type, out TypeConverter typeConverter)
		{
			typeConverter = TypeDescriptor.GetConverter(type);
			if (typeConverter != null)
			{
				Type type2 = typeConverter.GetType();
				if (!string.Equals(type2.FullName, "System.ComponentModel.ComponentConverter", StringComparison.Ordinal) && !string.Equals(type2.FullName, "System.ComponentModel.ReferenceConverter", StringComparison.Ordinal) && !string.Equals(type2.FullName, "System.Windows.Forms.Design.DataSourceConverter", StringComparison.Ordinal) && type2 != typeof(TypeConverter))
				{
					return typeConverter.CanConvertTo(typeof(string));
				}
			}
			return false;
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00067000 File Offset: 0x00065200
		[return: Nullable(2)]
		public static DataContractAttribute GetDataContractAttribute(Type type)
		{
			Type type2 = type;
			while (type2 != null)
			{
				DataContractAttribute attribute = CachedAttributeGetter<DataContractAttribute>.GetAttribute(type2);
				if (attribute != null)
				{
					return attribute;
				}
				type2 = type2.BaseType();
			}
			return null;
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00067030 File Offset: 0x00065230
		[return: Nullable(2)]
		public static DataMemberAttribute GetDataMemberAttribute(MemberInfo memberInfo)
		{
			if (memberInfo.MemberType() == MemberTypes.Field)
			{
				return CachedAttributeGetter<DataMemberAttribute>.GetAttribute(memberInfo);
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			DataMemberAttribute attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo);
			if (attribute == null && propertyInfo.IsVirtual())
			{
				Type type = propertyInfo.DeclaringType;
				while (attribute == null && type != null)
				{
					PropertyInfo propertyInfo2 = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(type, propertyInfo);
					if (propertyInfo2 != null && propertyInfo2.IsVirtual())
					{
						attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo2);
					}
					type = type.BaseType();
				}
			}
			return attribute;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x000670A8 File Offset: 0x000652A8
		public static MemberSerialization GetObjectMemberSerialization(Type objectType, bool ignoreSerializableAttribute)
		{
			JsonObjectAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonObjectAttribute>(objectType);
			if (cachedAttribute != null)
			{
				return cachedAttribute.MemberSerialization;
			}
			if (JsonTypeReflector.GetDataContractAttribute(objectType) != null)
			{
				return MemberSerialization.OptIn;
			}
			if (!ignoreSerializableAttribute && JsonTypeReflector.IsSerializable(objectType))
			{
				return MemberSerialization.Fields;
			}
			return MemberSerialization.OptOut;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x000670E0 File Offset: 0x000652E0
		[return: Nullable(2)]
		public static JsonConverter GetJsonConverter(object attributeProvider)
		{
			JsonConverterAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonConverterAttribute>(attributeProvider);
			if (cachedAttribute != null)
			{
				Func<object[], object> func = JsonTypeReflector.CreatorCache.Get(cachedAttribute.ConverterType);
				if (func != null)
				{
					return (JsonConverter)func(cachedAttribute.ConverterParameters);
				}
			}
			return null;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00013B7C File Offset: 0x00011D7C
		public static JsonConverter CreateJsonConverterInstance(Type converterType, [Nullable(new byte[]
		{
			2,
			1
		})] object[] args)
		{
			return (JsonConverter)JsonTypeReflector.CreatorCache.Get(converterType)(args);
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00013B94 File Offset: 0x00011D94
		public static NamingStrategy CreateNamingStrategyInstance(Type namingStrategyType, [Nullable(new byte[]
		{
			2,
			1
		})] object[] args)
		{
			return (NamingStrategy)JsonTypeReflector.CreatorCache.Get(namingStrategyType)(args);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00013BAC File Offset: 0x00011DAC
		[return: Nullable(2)]
		public static NamingStrategy GetContainerNamingStrategy(JsonContainerAttribute containerAttribute)
		{
			if (containerAttribute.NamingStrategyInstance == null)
			{
				if (containerAttribute.NamingStrategyType == null)
				{
					return null;
				}
				containerAttribute.NamingStrategyInstance = JsonTypeReflector.CreateNamingStrategyInstance(containerAttribute.NamingStrategyType, containerAttribute.NamingStrategyParameters);
			}
			return containerAttribute.NamingStrategyInstance;
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00067120 File Offset: 0x00065320
		[return: Nullable(new byte[]
		{
			1,
			2,
			1,
			1
		})]
		private static Func<object[], object> GetCreator(Type type)
		{
			Func<object> defaultConstructor = ReflectionUtils.HasDefaultConstructor(type, false) ? JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type) : null;
			return delegate([Nullable(new byte[]
			{
				2,
				1
			})] object[] parameters)
			{
				object result;
				try
				{
					if (parameters != null)
					{
						Type[] types = parameters.Select(delegate(object param)
						{
							if (param == null)
							{
								throw new InvalidOperationException("Cannot pass a null parameter to the constructor.");
							}
							return param.GetType();
						}).ToArray<Type>();
						ConstructorInfo constructor = type.GetConstructor(types);
						if (!(constructor != null))
						{
							throw new JsonException("No matching parameterized constructor found for '{0}'.".FormatWith(CultureInfo.InvariantCulture, type));
						}
						result = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor)(parameters);
					}
					else
					{
						if (defaultConstructor == null)
						{
							throw new JsonException("No parameterless constructor defined for '{0}'.".FormatWith(CultureInfo.InvariantCulture, type));
						}
						result = defaultConstructor();
					}
				}
				catch (Exception innerException)
				{
					throw new JsonException("Error creating '{0}'.".FormatWith(CultureInfo.InvariantCulture, type), innerException);
				}
				return result;
			};
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00013BE3 File Offset: 0x00011DE3
		[return: Nullable(2)]
		private static Type GetAssociatedMetadataType(Type type)
		{
			return JsonTypeReflector.AssociatedMetadataTypesCache.Get(type);
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00067170 File Offset: 0x00065370
		[return: Nullable(2)]
		private static Type GetAssociateMetadataTypeFromAttribute(Type type)
		{
			foreach (Attribute attribute in ReflectionUtils.GetAttributes(type, null, true))
			{
				Type type2 = attribute.GetType();
				if (string.Equals(type2.FullName, "System.ComponentModel.DataAnnotations.MetadataTypeAttribute", StringComparison.Ordinal))
				{
					if (JsonTypeReflector._metadataTypeAttributeReflectionObject == null)
					{
						JsonTypeReflector._metadataTypeAttributeReflectionObject = ReflectionObject.Create(type2, new string[]
						{
							"MetadataClassType"
						});
					}
					return (Type)JsonTypeReflector._metadataTypeAttributeReflectionObject.GetValue(attribute, "MetadataClassType");
				}
			}
			return null;
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x000671F0 File Offset: 0x000653F0
		[return: Nullable(2)]
		private static T GetAttribute<[Nullable(0)] T>(Type type) where T : Attribute
		{
			Type associatedMetadataType = JsonTypeReflector.GetAssociatedMetadataType(type);
			T attribute;
			if (associatedMetadataType != null)
			{
				attribute = ReflectionUtils.GetAttribute<T>(associatedMetadataType, true);
				if (attribute != null)
				{
					return attribute;
				}
			}
			attribute = ReflectionUtils.GetAttribute<T>(type, true);
			if (attribute != null)
			{
				return attribute;
			}
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				attribute = ReflectionUtils.GetAttribute<T>(interfaces[i], true);
				if (attribute != null)
				{
					return attribute;
				}
			}
			return default(T);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00067268 File Offset: 0x00065468
		[return: Nullable(2)]
		private static T GetAttribute<[Nullable(0)] T>(MemberInfo memberInfo) where T : Attribute
		{
			Type associatedMetadataType = JsonTypeReflector.GetAssociatedMetadataType(memberInfo.DeclaringType);
			T attribute;
			if (associatedMetadataType != null)
			{
				MemberInfo memberInfoFromType = ReflectionUtils.GetMemberInfoFromType(associatedMetadataType, memberInfo);
				if (memberInfoFromType != null)
				{
					attribute = ReflectionUtils.GetAttribute<T>(memberInfoFromType, true);
					if (attribute != null)
					{
						return attribute;
					}
				}
			}
			attribute = ReflectionUtils.GetAttribute<T>(memberInfo, true);
			if (attribute != null)
			{
				return attribute;
			}
			if (memberInfo.DeclaringType != null)
			{
				Type[] interfaces = memberInfo.DeclaringType.GetInterfaces();
				for (int i = 0; i < interfaces.Length; i++)
				{
					MemberInfo memberInfoFromType2 = ReflectionUtils.GetMemberInfoFromType(interfaces[i], memberInfo);
					if (memberInfoFromType2 != null)
					{
						attribute = ReflectionUtils.GetAttribute<T>(memberInfoFromType2, true);
						if (attribute != null)
						{
							return attribute;
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00013BF0 File Offset: 0x00011DF0
		public static bool IsNonSerializable(object provider)
		{
			return ReflectionUtils.GetAttribute<NonSerializedAttribute>(provider, false) != null;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00013BFC File Offset: 0x00011DFC
		public static bool IsSerializable(object provider)
		{
			return ReflectionUtils.GetAttribute<SerializableAttribute>(provider, false) != null;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00067324 File Offset: 0x00065524
		[return: Nullable(2)]
		public static T GetAttribute<[Nullable(0)] T>(object provider) where T : Attribute
		{
			Type type = provider as Type;
			if (type != null)
			{
				return JsonTypeReflector.GetAttribute<T>(type);
			}
			MemberInfo memberInfo = provider as MemberInfo;
			if (memberInfo != null)
			{
				return JsonTypeReflector.GetAttribute<T>(memberInfo);
			}
			return ReflectionUtils.GetAttribute<T>(provider, true);
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x0006735C File Offset: 0x0006555C
		public static bool DynamicCodeGeneration
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonTypeReflector._dynamicCodeGeneration == null)
				{
					try
					{
						new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
						new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess).Demand();
						new SecurityPermission(SecurityPermissionFlag.SkipVerification).Demand();
						new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
						new SecurityPermission(PermissionState.Unrestricted).Demand();
						JsonTypeReflector._dynamicCodeGeneration = new bool?(true);
					}
					catch (Exception)
					{
						JsonTypeReflector._dynamicCodeGeneration = new bool?(false);
					}
				}
				return JsonTypeReflector._dynamicCodeGeneration.GetValueOrDefault();
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x000673E4 File Offset: 0x000655E4
		public static bool FullyTrusted
		{
			get
			{
				if (JsonTypeReflector._fullyTrusted == null)
				{
					AppDomain currentDomain = AppDomain.CurrentDomain;
					JsonTypeReflector._fullyTrusted = new bool?(currentDomain.IsHomogenous && currentDomain.IsFullyTrusted);
				}
				return JsonTypeReflector._fullyTrusted.GetValueOrDefault();
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x00013C08 File Offset: 0x00011E08
		public static ReflectionDelegateFactory ReflectionDelegateFactory
		{
			get
			{
				if (JsonTypeReflector.DynamicCodeGeneration)
				{
					return DynamicReflectionDelegateFactory.Instance;
				}
				return LateBoundReflectionDelegateFactory.Instance;
			}
		}

		// Token: 0x04000AF2 RID: 2802
		private static bool? _dynamicCodeGeneration;

		// Token: 0x04000AF3 RID: 2803
		private static bool? _fullyTrusted;

		// Token: 0x04000AF4 RID: 2804
		public const string IdPropertyName = "$id";

		// Token: 0x04000AF5 RID: 2805
		public const string RefPropertyName = "$ref";

		// Token: 0x04000AF6 RID: 2806
		public const string TypePropertyName = "$type";

		// Token: 0x04000AF7 RID: 2807
		public const string ValuePropertyName = "$value";

		// Token: 0x04000AF8 RID: 2808
		public const string ArrayValuesPropertyName = "$values";

		// Token: 0x04000AF9 RID: 2809
		public const string ShouldSerializePrefix = "ShouldSerialize";

		// Token: 0x04000AFA RID: 2810
		public const string SpecifiedPostfix = "Specified";

		// Token: 0x04000AFB RID: 2811
		public const string ConcurrentDictionaryTypeName = "System.Collections.Concurrent.ConcurrentDictionary`2";

		// Token: 0x04000AFC RID: 2812
		[Nullable(new byte[]
		{
			1,
			1,
			1,
			2,
			1,
			1
		})]
		private static readonly ThreadSafeStore<Type, Func<object[], object>> CreatorCache = new ThreadSafeStore<Type, Func<object[], object>>(new Func<Type, Func<object[], object>>(JsonTypeReflector.GetCreator));

		// Token: 0x04000AFD RID: 2813
		[Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		private static readonly ThreadSafeStore<Type, Type> AssociatedMetadataTypesCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(JsonTypeReflector.GetAssociateMetadataTypeFromAttribute));

		// Token: 0x04000AFE RID: 2814
		[Nullable(2)]
		private static ReflectionObject _metadataTypeAttributeReflectionObject;
	}
}
