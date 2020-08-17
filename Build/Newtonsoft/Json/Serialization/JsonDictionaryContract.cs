using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200026E RID: 622
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonDictionaryContract : JsonContainerContract
	{
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x00012EC9 File Offset: 0x000110C9
		// (set) Token: 0x06001176 RID: 4470 RVA: 0x00012ED1 File Offset: 0x000110D1
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		public Func<string, string> DictionaryKeyResolver { [return: Nullable(new byte[]
		{
			2,
			1,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1,
			1
		})] set; }

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x00012EDA File Offset: 0x000110DA
		public Type DictionaryKeyType { get; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x00012EE2 File Offset: 0x000110E2
		public Type DictionaryValueType { get; }

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001179 RID: 4473 RVA: 0x00012EEA File Offset: 0x000110EA
		// (set) Token: 0x0600117A RID: 4474 RVA: 0x00012EF2 File Offset: 0x000110F2
		internal JsonContract KeyContract { get; set; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600117B RID: 4475 RVA: 0x00012EFB File Offset: 0x000110FB
		internal bool ShouldCreateWrapper { get; }

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x00012F03 File Offset: 0x00011103
		[Nullable(new byte[]
		{
			2,
			1
		})]
		internal ObjectConstructor<object> ParameterizedCreator
		{
			[return: Nullable(new byte[]
			{
				2,
				1
			})]
			get
			{
				if (this._parameterizedCreator == null && this._parameterizedConstructor != null)
				{
					this._parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(this._parameterizedConstructor);
				}
				return this._parameterizedCreator;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x00012F37 File Offset: 0x00011137
		// (set) Token: 0x0600117E RID: 4478 RVA: 0x00012F3F File Offset: 0x0001113F
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public ObjectConstructor<object> OverrideCreator
		{
			[return: Nullable(new byte[]
			{
				2,
				1
			})]
			get
			{
				return this._overrideCreator;
			}
			[param: Nullable(new byte[]
			{
				2,
				1
			})]
			set
			{
				this._overrideCreator = value;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x0600117F RID: 4479 RVA: 0x00012F48 File Offset: 0x00011148
		// (set) Token: 0x06001180 RID: 4480 RVA: 0x00012F50 File Offset: 0x00011150
		public bool HasParameterizedCreator { get; set; }

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x00012F59 File Offset: 0x00011159
		internal bool HasParameterizedCreatorInternal
		{
			get
			{
				return this.HasParameterizedCreator || this._parameterizedCreator != null || this._parameterizedConstructor != null;
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00061230 File Offset: 0x0005F430
		[NullableContext(1)]
		public JsonDictionaryContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Dictionary;
			Type type;
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IDictionary<, >), out this._genericCollectionDefinitionType))
			{
				type = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				type2 = this._genericCollectionDefinitionType.GetGenericArguments()[1];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IDictionary<, >)))
				{
					base.CreatedType = typeof(Dictionary<, >).MakeGenericType(new Type[]
					{
						type,
						type2
					});
				}
				else if (underlyingType.IsGenericType() && underlyingType.GetGenericTypeDefinition().FullName == "System.Collections.Concurrent.ConcurrentDictionary`2")
				{
					this.ShouldCreateWrapper = 1;
				}
			}
			else
			{
				ReflectionUtils.GetDictionaryKeyValueTypes(base.UnderlyingType, out type, out type2);
				if (base.UnderlyingType == typeof(IDictionary))
				{
					base.CreatedType = typeof(Dictionary<object, object>);
				}
			}
			if (type != null && type2 != null)
			{
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(base.CreatedType, typeof(KeyValuePair<, >).MakeGenericType(new Type[]
				{
					type,
					type2
				}), typeof(IDictionary<, >).MakeGenericType(new Type[]
				{
					type,
					type2
				}));
				if (!this.HasParameterizedCreatorInternal && underlyingType.Name == "FSharpMap`2")
				{
					FSharpUtils.EnsureInitialized(underlyingType.Assembly());
					this._parameterizedCreator = FSharpUtils.Instance.CreateMap(type, type2);
				}
			}
			if (!typeof(IDictionary).IsAssignableFrom(base.CreatedType))
			{
				this.ShouldCreateWrapper = 1;
			}
			this.DictionaryKeyType = type;
			this.DictionaryValueType = type2;
			Type createdType;
			ObjectConstructor<object> parameterizedCreator;
			if (this.DictionaryKeyType != null && this.DictionaryValueType != null && ImmutableCollectionsUtils.TryBuildImmutableForDictionaryContract(underlyingType, this.DictionaryKeyType, this.DictionaryValueType, out createdType, out parameterizedCreator))
			{
				base.CreatedType = createdType;
				this._parameterizedCreator = parameterizedCreator;
				this.IsReadOnlyOrFixedSize = true;
			}
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x00061434 File Offset: 0x0005F634
		[NullableContext(1)]
		internal IWrappedDictionary CreateWrapper(object dictionary)
		{
			if (this._genericWrapperCreator == null)
			{
				this._genericWrapperType = typeof(DictionaryWrapper<, >).MakeGenericType(new Type[]
				{
					this.DictionaryKeyType,
					this.DictionaryValueType
				});
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[]
				{
					this._genericCollectionDefinitionType
				});
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			}
			return (IWrappedDictionary)this._genericWrapperCreator(new object[]
			{
				dictionary
			});
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x000614BC File Offset: 0x0005F6BC
		[NullableContext(1)]
		internal IDictionary CreateTemporaryDictionary()
		{
			if (this._genericTemporaryDictionaryCreator == null)
			{
				Type type = typeof(Dictionary<, >).MakeGenericType(new Type[]
				{
					this.DictionaryKeyType ?? typeof(object),
					this.DictionaryValueType ?? typeof(object)
				});
				this._genericTemporaryDictionaryCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type);
			}
			return (IDictionary)this._genericTemporaryDictionaryCreator();
		}

		// Token: 0x04000A94 RID: 2708
		private readonly Type _genericCollectionDefinitionType;

		// Token: 0x04000A95 RID: 2709
		private Type _genericWrapperType;

		// Token: 0x04000A96 RID: 2710
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private ObjectConstructor<object> _genericWrapperCreator;

		// Token: 0x04000A97 RID: 2711
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private Func<object> _genericTemporaryDictionaryCreator;

		// Token: 0x04000A99 RID: 2713
		private readonly ConstructorInfo _parameterizedConstructor;

		// Token: 0x04000A9A RID: 2714
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private ObjectConstructor<object> _overrideCreator;

		// Token: 0x04000A9B RID: 2715
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private ObjectConstructor<object> _parameterizedCreator;
	}
}
