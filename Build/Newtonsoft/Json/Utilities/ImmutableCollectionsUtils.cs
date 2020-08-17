using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200021D RID: 541
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ImmutableCollectionsUtils
	{
		// Token: 0x06000F99 RID: 3993 RVA: 0x0005BBD4 File Offset: 0x00059DD4
		internal static bool TryBuildImmutableForArrayContract(Type underlyingType, Type collectionItemType, [Nullable(2)] [NotNullWhen(true)] out Type createdType, [Nullable(new byte[]
		{
			2,
			1
		})] [NotNullWhen(true)] out ObjectConstructor<object> parameterizedCreator)
		{
			if (underlyingType.IsGenericType())
			{
				Type genericTypeDefinition = underlyingType.GetGenericTypeDefinition();
				string name = genericTypeDefinition.FullName;
				ImmutableCollectionsUtils.ImmutableCollectionTypeInfo immutableCollectionTypeInfo = ImmutableCollectionsUtils.ArrayContractImmutableCollectionDefinitions.FirstOrDefault((ImmutableCollectionsUtils.ImmutableCollectionTypeInfo d) => d.ContractTypeName == name);
				if (immutableCollectionTypeInfo != null)
				{
					Type type = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.CreatedTypeName);
					Type type2 = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.BuilderTypeName);
					if (type != null && type2 != null)
					{
						MethodInfo methodInfo = type2.GetMethods().FirstOrDefault((MethodInfo m) => m.Name == "CreateRange" && m.GetParameters().Length == 1);
						if (methodInfo != null)
						{
							createdType = type.MakeGenericType(new Type[]
							{
								collectionItemType
							});
							MethodInfo method = methodInfo.MakeGenericMethod(new Type[]
							{
								collectionItemType
							});
							parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(method);
							return true;
						}
					}
				}
			}
			createdType = null;
			parameterizedCreator = null;
			return false;
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0005BCD0 File Offset: 0x00059ED0
		internal static bool TryBuildImmutableForDictionaryContract(Type underlyingType, Type keyItemType, Type valueItemType, [Nullable(2)] [NotNullWhen(true)] out Type createdType, [Nullable(new byte[]
		{
			2,
			1
		})] [NotNullWhen(true)] out ObjectConstructor<object> parameterizedCreator)
		{
			if (underlyingType.IsGenericType())
			{
				Type genericTypeDefinition = underlyingType.GetGenericTypeDefinition();
				string name = genericTypeDefinition.FullName;
				ImmutableCollectionsUtils.ImmutableCollectionTypeInfo immutableCollectionTypeInfo = ImmutableCollectionsUtils.DictionaryContractImmutableCollectionDefinitions.FirstOrDefault((ImmutableCollectionsUtils.ImmutableCollectionTypeInfo d) => d.ContractTypeName == name);
				if (immutableCollectionTypeInfo != null)
				{
					Type type = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.CreatedTypeName);
					Type type2 = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.BuilderTypeName);
					if (type != null && type2 != null)
					{
						MethodInfo methodInfo = type2.GetMethods().FirstOrDefault(delegate(MethodInfo m)
						{
							ParameterInfo[] parameters = m.GetParameters();
							return m.Name == "CreateRange" && parameters.Length == 1 && parameters[0].ParameterType.IsGenericType() && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
						});
						if (methodInfo != null)
						{
							createdType = type.MakeGenericType(new Type[]
							{
								keyItemType,
								valueItemType
							});
							MethodInfo method = methodInfo.MakeGenericMethod(new Type[]
							{
								keyItemType,
								valueItemType
							});
							parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(method);
							return true;
						}
					}
				}
			}
			createdType = null;
			parameterizedCreator = null;
			return false;
		}

		// Token: 0x040009C9 RID: 2505
		private const string ImmutableListGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableList`1";

		// Token: 0x040009CA RID: 2506
		private const string ImmutableQueueGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableQueue`1";

		// Token: 0x040009CB RID: 2507
		private const string ImmutableStackGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableStack`1";

		// Token: 0x040009CC RID: 2508
		private const string ImmutableSetGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableSet`1";

		// Token: 0x040009CD RID: 2509
		private const string ImmutableArrayTypeName = "System.Collections.Immutable.ImmutableArray";

		// Token: 0x040009CE RID: 2510
		private const string ImmutableArrayGenericTypeName = "System.Collections.Immutable.ImmutableArray`1";

		// Token: 0x040009CF RID: 2511
		private const string ImmutableListTypeName = "System.Collections.Immutable.ImmutableList";

		// Token: 0x040009D0 RID: 2512
		private const string ImmutableListGenericTypeName = "System.Collections.Immutable.ImmutableList`1";

		// Token: 0x040009D1 RID: 2513
		private const string ImmutableQueueTypeName = "System.Collections.Immutable.ImmutableQueue";

		// Token: 0x040009D2 RID: 2514
		private const string ImmutableQueueGenericTypeName = "System.Collections.Immutable.ImmutableQueue`1";

		// Token: 0x040009D3 RID: 2515
		private const string ImmutableStackTypeName = "System.Collections.Immutable.ImmutableStack";

		// Token: 0x040009D4 RID: 2516
		private const string ImmutableStackGenericTypeName = "System.Collections.Immutable.ImmutableStack`1";

		// Token: 0x040009D5 RID: 2517
		private const string ImmutableSortedSetTypeName = "System.Collections.Immutable.ImmutableSortedSet";

		// Token: 0x040009D6 RID: 2518
		private const string ImmutableSortedSetGenericTypeName = "System.Collections.Immutable.ImmutableSortedSet`1";

		// Token: 0x040009D7 RID: 2519
		private const string ImmutableHashSetTypeName = "System.Collections.Immutable.ImmutableHashSet";

		// Token: 0x040009D8 RID: 2520
		private const string ImmutableHashSetGenericTypeName = "System.Collections.Immutable.ImmutableHashSet`1";

		// Token: 0x040009D9 RID: 2521
		private static readonly IList<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo> ArrayContractImmutableCollectionDefinitions = new List<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>
		{
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableList`1", "System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableStack`1", "System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableSet`1", "System.Collections.Immutable.ImmutableHashSet`1", "System.Collections.Immutable.ImmutableHashSet"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableSortedSet`1", "System.Collections.Immutable.ImmutableSortedSet`1", "System.Collections.Immutable.ImmutableSortedSet"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableHashSet`1", "System.Collections.Immutable.ImmutableHashSet`1", "System.Collections.Immutable.ImmutableHashSet"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableArray`1", "System.Collections.Immutable.ImmutableArray`1", "System.Collections.Immutable.ImmutableArray")
		};

		// Token: 0x040009DA RID: 2522
		private const string ImmutableDictionaryGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableDictionary`2";

		// Token: 0x040009DB RID: 2523
		private const string ImmutableDictionaryTypeName = "System.Collections.Immutable.ImmutableDictionary";

		// Token: 0x040009DC RID: 2524
		private const string ImmutableDictionaryGenericTypeName = "System.Collections.Immutable.ImmutableDictionary`2";

		// Token: 0x040009DD RID: 2525
		private const string ImmutableSortedDictionaryTypeName = "System.Collections.Immutable.ImmutableSortedDictionary";

		// Token: 0x040009DE RID: 2526
		private const string ImmutableSortedDictionaryGenericTypeName = "System.Collections.Immutable.ImmutableSortedDictionary`2";

		// Token: 0x040009DF RID: 2527
		private static readonly IList<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo> DictionaryContractImmutableCollectionDefinitions = new List<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>
		{
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableSortedDictionary`2", "System.Collections.Immutable.ImmutableSortedDictionary`2", "System.Collections.Immutable.ImmutableSortedDictionary"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary")
		};

		// Token: 0x0200021E RID: 542
		[Nullable(0)]
		internal class ImmutableCollectionTypeInfo
		{
			// Token: 0x06000F9C RID: 3996 RVA: 0x00011DA5 File Offset: 0x0000FFA5
			public ImmutableCollectionTypeInfo(string contractTypeName, string createdTypeName, string builderTypeName)
			{
				this.ContractTypeName = contractTypeName;
				this.CreatedTypeName = createdTypeName;
				this.BuilderTypeName = builderTypeName;
			}

			// Token: 0x17000327 RID: 807
			// (get) Token: 0x06000F9D RID: 3997 RVA: 0x00011DC2 File Offset: 0x0000FFC2
			// (set) Token: 0x06000F9E RID: 3998 RVA: 0x00011DCA File Offset: 0x0000FFCA
			public string ContractTypeName { get; set; }

			// Token: 0x17000328 RID: 808
			// (get) Token: 0x06000F9F RID: 3999 RVA: 0x00011DD3 File Offset: 0x0000FFD3
			// (set) Token: 0x06000FA0 RID: 4000 RVA: 0x00011DDB File Offset: 0x0000FFDB
			public string CreatedTypeName { get; set; }

			// Token: 0x17000329 RID: 809
			// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x00011DE4 File Offset: 0x0000FFE4
			// (set) Token: 0x06000FA2 RID: 4002 RVA: 0x00011DEC File Offset: 0x0000FFEC
			public string BuilderTypeName { get; set; }
		}
	}
}
