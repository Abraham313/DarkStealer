using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000264 RID: 612
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonArrayContract : JsonContainerContract
	{
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x0600112B RID: 4395 RVA: 0x00012BF2 File Offset: 0x00010DF2
		public Type CollectionItemType { get; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x00012BFA File Offset: 0x00010DFA
		public bool IsMultidimensionalArray { get; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x00012C02 File Offset: 0x00010E02
		internal bool IsArray { get; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x00012C0A File Offset: 0x00010E0A
		internal bool ShouldCreateWrapper { get; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x0600112F RID: 4399 RVA: 0x00012C12 File Offset: 0x00010E12
		// (set) Token: 0x06001130 RID: 4400 RVA: 0x00012C1A File Offset: 0x00010E1A
		internal bool CanDeserialize { get; private set; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x00012C23 File Offset: 0x00010E23
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

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001132 RID: 4402 RVA: 0x00012C57 File Offset: 0x00010E57
		// (set) Token: 0x06001133 RID: 4403 RVA: 0x00012C5F File Offset: 0x00010E5F
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
				this.CanDeserialize = true;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x00012C6F File Offset: 0x00010E6F
		// (set) Token: 0x06001135 RID: 4405 RVA: 0x00012C77 File Offset: 0x00010E77
		public bool HasParameterizedCreator { get; set; }

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06001136 RID: 4406 RVA: 0x00012C80 File Offset: 0x00010E80
		internal bool HasParameterizedCreatorInternal
		{
			get
			{
				return this.HasParameterizedCreator || this._parameterizedCreator != null || this._parameterizedConstructor != null;
			}
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x000609E0 File Offset: 0x0005EBE0
		[NullableContext(1)]
		public JsonArrayContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Array;
			this.IsArray = (base.CreatedType.IsArray || (this.NonNullableUnderlyingType.IsGenericType() && this.NonNullableUnderlyingType.GetGenericTypeDefinition().FullName == "System.Linq.EmptyPartition`1"));
			bool canDeserialize;
			Type type;
			if (this.IsArray)
			{
				this.CollectionItemType = ReflectionUtils.GetCollectionItemType(base.UnderlyingType);
				this.IsReadOnlyOrFixedSize = true;
				this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[]
				{
					this.CollectionItemType
				});
				canDeserialize = true;
				this.IsMultidimensionalArray = (base.CreatedType.IsArray && base.UnderlyingType.GetArrayRank() > 1);
			}
			else if (typeof(IList).IsAssignableFrom(this.NonNullableUnderlyingType))
			{
				if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(ICollection<>), out this._genericCollectionDefinitionType))
				{
					this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				}
				else
				{
					this.CollectionItemType = ReflectionUtils.GetCollectionItemType(this.NonNullableUnderlyingType);
				}
				if (this.NonNullableUnderlyingType == typeof(IList))
				{
					base.CreatedType = typeof(List<object>);
				}
				if (this.CollectionItemType != null)
				{
					this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
				}
				this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(this.NonNullableUnderlyingType, typeof(ReadOnlyCollection<>));
				canDeserialize = true;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(ICollection<>), out this._genericCollectionDefinitionType))
			{
				this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(ICollection<>)) || ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(IList<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(new Type[]
					{
						this.CollectionItemType
					});
				}
				if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(ISet<>)))
				{
					base.CreatedType = typeof(HashSet<>).MakeGenericType(new Type[]
					{
						this.CollectionItemType
					});
				}
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
				canDeserialize = true;
				this.ShouldCreateWrapper = 1;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(IEnumerable<>), out type))
			{
				this.CollectionItemType = type.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IEnumerable<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(new Type[]
					{
						this.CollectionItemType
					});
				}
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
				this.StoreFSharpListCreatorIfNecessary(this.NonNullableUnderlyingType);
				if (this.NonNullableUnderlyingType.IsGenericType() && this.NonNullableUnderlyingType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					this._genericCollectionDefinitionType = type;
					this.IsReadOnlyOrFixedSize = false;
					this.ShouldCreateWrapper = 0;
					canDeserialize = true;
				}
				else
				{
					this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[]
					{
						this.CollectionItemType
					});
					this.IsReadOnlyOrFixedSize = true;
					this.ShouldCreateWrapper = 1;
					canDeserialize = this.HasParameterizedCreatorInternal;
				}
			}
			else
			{
				canDeserialize = false;
				this.ShouldCreateWrapper = 1;
			}
			this.CanDeserialize = canDeserialize;
			Type createdType;
			ObjectConstructor<object> parameterizedCreator;
			if (this.CollectionItemType != null && ImmutableCollectionsUtils.TryBuildImmutableForArrayContract(this.NonNullableUnderlyingType, this.CollectionItemType, out createdType, out parameterizedCreator))
			{
				base.CreatedType = createdType;
				this._parameterizedCreator = parameterizedCreator;
				this.IsReadOnlyOrFixedSize = true;
				this.CanDeserialize = true;
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00060DBC File Offset: 0x0005EFBC
		[NullableContext(1)]
		internal IWrappedCollection CreateWrapper(object list)
		{
			if (this._genericWrapperCreator == null)
			{
				this._genericWrapperType = typeof(CollectionWrapper<>).MakeGenericType(new Type[]
				{
					this.CollectionItemType
				});
				Type type;
				if (!ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof(List<>)) && !(this._genericCollectionDefinitionType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
				{
					type = this._genericCollectionDefinitionType;
				}
				else
				{
					type = typeof(ICollection<>).MakeGenericType(new Type[]
					{
						this.CollectionItemType
					});
				}
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[]
				{
					type
				});
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			}
			return (IWrappedCollection)this._genericWrapperCreator(new object[]
			{
				list
			});
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00060E94 File Offset: 0x0005F094
		[NullableContext(1)]
		internal IList CreateTemporaryCollection()
		{
			if (this._genericTemporaryCollectionCreator == null)
			{
				Type type = (this.IsMultidimensionalArray || this.CollectionItemType == null) ? typeof(object) : this.CollectionItemType;
				Type type2 = typeof(List<>).MakeGenericType(new Type[]
				{
					type
				});
				this._genericTemporaryCollectionCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type2);
			}
			return (IList)this._genericTemporaryCollectionCreator();
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00012CA0 File Offset: 0x00010EA0
		[NullableContext(1)]
		private void StoreFSharpListCreatorIfNecessary(Type underlyingType)
		{
			if (!this.HasParameterizedCreatorInternal && underlyingType.Name == "FSharpList`1")
			{
				FSharpUtils.EnsureInitialized(underlyingType.Assembly());
				this._parameterizedCreator = FSharpUtils.Instance.CreateSeq(this.CollectionItemType);
			}
		}

		// Token: 0x04000A5E RID: 2654
		private readonly Type _genericCollectionDefinitionType;

		// Token: 0x04000A5F RID: 2655
		private Type _genericWrapperType;

		// Token: 0x04000A60 RID: 2656
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private ObjectConstructor<object> _genericWrapperCreator;

		// Token: 0x04000A61 RID: 2657
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private Func<object> _genericTemporaryCollectionCreator;

		// Token: 0x04000A65 RID: 2661
		private readonly ConstructorInfo _parameterizedConstructor;

		// Token: 0x04000A66 RID: 2662
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private ObjectConstructor<object> _parameterizedCreator;

		// Token: 0x04000A67 RID: 2663
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private ObjectConstructor<object> _overrideCreator;
	}
}
