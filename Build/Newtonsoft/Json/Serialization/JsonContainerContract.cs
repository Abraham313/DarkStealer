using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000265 RID: 613
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonContainerContract : JsonContract
	{
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x0600113B RID: 4411 RVA: 0x00012CDD File Offset: 0x00010EDD
		// (set) Token: 0x0600113C RID: 4412 RVA: 0x00012CE5 File Offset: 0x00010EE5
		internal JsonContract ItemContract
		{
			get
			{
				return this._itemContract;
			}
			set
			{
				this._itemContract = value;
				if (this._itemContract != null)
				{
					this._finalItemContract = (this._itemContract.UnderlyingType.IsSealed() ? this._itemContract : null);
					return;
				}
				this._finalItemContract = null;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x0600113D RID: 4413 RVA: 0x00012D1F File Offset: 0x00010F1F
		internal JsonContract FinalItemContract
		{
			get
			{
				return this._finalItemContract;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x00012D27 File Offset: 0x00010F27
		// (set) Token: 0x0600113F RID: 4415 RVA: 0x00012D2F File Offset: 0x00010F2F
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x00012D38 File Offset: 0x00010F38
		// (set) Token: 0x06001141 RID: 4417 RVA: 0x00012D40 File Offset: 0x00010F40
		public bool? ItemIsReference { get; set; }

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x00012D49 File Offset: 0x00010F49
		// (set) Token: 0x06001143 RID: 4419 RVA: 0x00012D51 File Offset: 0x00010F51
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x00012D5A File Offset: 0x00010F5A
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x00012D62 File Offset: 0x00010F62
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x06001146 RID: 4422 RVA: 0x00060F10 File Offset: 0x0005F110
		[NullableContext(1)]
		internal JsonContainerContract(Type underlyingType) : base(underlyingType)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(underlyingType);
			if (cachedAttribute != null)
			{
				if (cachedAttribute.ItemConverterType != null)
				{
					this.ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(cachedAttribute.ItemConverterType, cachedAttribute.ItemConverterParameters);
				}
				this.ItemIsReference = cachedAttribute._itemIsReference;
				this.ItemReferenceLoopHandling = cachedAttribute._itemReferenceLoopHandling;
				this.ItemTypeNameHandling = cachedAttribute._itemTypeNameHandling;
			}
		}

		// Token: 0x04000A69 RID: 2665
		private JsonContract _itemContract;

		// Token: 0x04000A6A RID: 2666
		private JsonContract _finalItemContract;
	}
}
