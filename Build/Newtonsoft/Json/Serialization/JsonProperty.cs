using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000275 RID: 629
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonProperty
	{
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060011C1 RID: 4545 RVA: 0x00013246 File Offset: 0x00011446
		// (set) Token: 0x060011C2 RID: 4546 RVA: 0x0001324E File Offset: 0x0001144E
		internal JsonContract PropertyContract { get; set; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x00013257 File Offset: 0x00011457
		// (set) Token: 0x060011C4 RID: 4548 RVA: 0x0001325F File Offset: 0x0001145F
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
			set
			{
				this._propertyName = value;
				this._skipPropertyNameEscape = !JavaScriptUtils.ShouldEscapeJavaScriptString(this._propertyName, JavaScriptUtils.HtmlCharEscapeFlags);
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x00013281 File Offset: 0x00011481
		// (set) Token: 0x060011C6 RID: 4550 RVA: 0x00013289 File Offset: 0x00011489
		public Type DeclaringType { get; set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00013292 File Offset: 0x00011492
		// (set) Token: 0x060011C8 RID: 4552 RVA: 0x0001329A File Offset: 0x0001149A
		public int? Order { get; set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060011C9 RID: 4553 RVA: 0x000132A3 File Offset: 0x000114A3
		// (set) Token: 0x060011CA RID: 4554 RVA: 0x000132AB File Offset: 0x000114AB
		public string UnderlyingName { get; set; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060011CB RID: 4555 RVA: 0x000132B4 File Offset: 0x000114B4
		// (set) Token: 0x060011CC RID: 4556 RVA: 0x000132BC File Offset: 0x000114BC
		public IValueProvider ValueProvider { get; set; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060011CD RID: 4557 RVA: 0x000132C5 File Offset: 0x000114C5
		// (set) Token: 0x060011CE RID: 4558 RVA: 0x000132CD File Offset: 0x000114CD
		public IAttributeProvider AttributeProvider { get; set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x000132D6 File Offset: 0x000114D6
		// (set) Token: 0x060011D0 RID: 4560 RVA: 0x000132DE File Offset: 0x000114DE
		public Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
			set
			{
				if (this._propertyType != value)
				{
					this._propertyType = value;
					this._hasGeneratedDefaultValue = false;
				}
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x000132FC File Offset: 0x000114FC
		// (set) Token: 0x060011D2 RID: 4562 RVA: 0x00013304 File Offset: 0x00011504
		public JsonConverter Converter { get; set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x0001330D File Offset: 0x0001150D
		// (set) Token: 0x060011D4 RID: 4564 RVA: 0x00013315 File Offset: 0x00011515
		[Obsolete("MemberConverter is obsolete. Use Converter instead.")]
		public JsonConverter MemberConverter
		{
			get
			{
				return this.Converter;
			}
			set
			{
				this.Converter = value;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x0001331E File Offset: 0x0001151E
		// (set) Token: 0x060011D6 RID: 4566 RVA: 0x00013326 File Offset: 0x00011526
		public bool Ignored { get; set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x0001332F File Offset: 0x0001152F
		// (set) Token: 0x060011D8 RID: 4568 RVA: 0x00013337 File Offset: 0x00011537
		public bool Readable { get; set; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x00013340 File Offset: 0x00011540
		// (set) Token: 0x060011DA RID: 4570 RVA: 0x00013348 File Offset: 0x00011548
		public bool Writable { get; set; }

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x00013351 File Offset: 0x00011551
		// (set) Token: 0x060011DC RID: 4572 RVA: 0x00013359 File Offset: 0x00011559
		public bool HasMemberAttribute { get; set; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x00013362 File Offset: 0x00011562
		// (set) Token: 0x060011DE RID: 4574 RVA: 0x00013374 File Offset: 0x00011574
		public object DefaultValue
		{
			get
			{
				if (!this._hasExplicitDefaultValue)
				{
					return null;
				}
				return this._defaultValue;
			}
			set
			{
				this._hasExplicitDefaultValue = true;
				this._defaultValue = value;
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00013384 File Offset: 0x00011584
		internal object GetResolvedDefaultValue()
		{
			if (this._propertyType == null)
			{
				return null;
			}
			if (!this._hasExplicitDefaultValue && !this._hasGeneratedDefaultValue)
			{
				this._defaultValue = ReflectionUtils.GetDefaultValue(this._propertyType);
				this._hasGeneratedDefaultValue = true;
			}
			return this._defaultValue;
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x000133C4 File Offset: 0x000115C4
		// (set) Token: 0x060011E1 RID: 4577 RVA: 0x000133D1 File Offset: 0x000115D1
		public Required Required
		{
			get
			{
				return this._required.GetValueOrDefault();
			}
			set
			{
				this._required = new Required?(value);
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060011E2 RID: 4578 RVA: 0x000133DF File Offset: 0x000115DF
		public bool IsRequiredSpecified
		{
			get
			{
				return this._required != null;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x000133EC File Offset: 0x000115EC
		// (set) Token: 0x060011E4 RID: 4580 RVA: 0x000133F4 File Offset: 0x000115F4
		public bool? IsReference { get; set; }

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060011E5 RID: 4581 RVA: 0x000133FD File Offset: 0x000115FD
		// (set) Token: 0x060011E6 RID: 4582 RVA: 0x00013405 File Offset: 0x00011605
		public NullValueHandling? NullValueHandling { get; set; }

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060011E7 RID: 4583 RVA: 0x0001340E File Offset: 0x0001160E
		// (set) Token: 0x060011E8 RID: 4584 RVA: 0x00013416 File Offset: 0x00011616
		public DefaultValueHandling? DefaultValueHandling { get; set; }

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x0001341F File Offset: 0x0001161F
		// (set) Token: 0x060011EA RID: 4586 RVA: 0x00013427 File Offset: 0x00011627
		public ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060011EB RID: 4587 RVA: 0x00013430 File Offset: 0x00011630
		// (set) Token: 0x060011EC RID: 4588 RVA: 0x00013438 File Offset: 0x00011638
		public ObjectCreationHandling? ObjectCreationHandling { get; set; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060011ED RID: 4589 RVA: 0x00013441 File Offset: 0x00011641
		// (set) Token: 0x060011EE RID: 4590 RVA: 0x00013449 File Offset: 0x00011649
		public TypeNameHandling? TypeNameHandling { get; set; }

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x00013452 File Offset: 0x00011652
		// (set) Token: 0x060011F0 RID: 4592 RVA: 0x0001345A File Offset: 0x0001165A
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public Predicate<object> ShouldSerialize { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x00013463 File Offset: 0x00011663
		// (set) Token: 0x060011F2 RID: 4594 RVA: 0x0001346B File Offset: 0x0001166B
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public Predicate<object> ShouldDeserialize { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x00013474 File Offset: 0x00011674
		// (set) Token: 0x060011F4 RID: 4596 RVA: 0x0001347C File Offset: 0x0001167C
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public Predicate<object> GetIsSpecified { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060011F5 RID: 4597 RVA: 0x00013485 File Offset: 0x00011685
		// (set) Token: 0x060011F6 RID: 4598 RVA: 0x0001348D File Offset: 0x0001168D
		[Nullable(new byte[]
		{
			2,
			1,
			2
		})]
		public Action<object, object> SetIsSpecified { [return: Nullable(new byte[]
		{
			2,
			1,
			2
		})] get; [param: Nullable(new byte[]
		{
			2,
			1,
			2
		})] set; }

		// Token: 0x060011F7 RID: 4599 RVA: 0x00013496 File Offset: 0x00011696
		[NullableContext(1)]
		public override string ToString()
		{
			return this.PropertyName ?? string.Empty;
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x000134A7 File Offset: 0x000116A7
		// (set) Token: 0x060011F9 RID: 4601 RVA: 0x000134AF File Offset: 0x000116AF
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x000134B8 File Offset: 0x000116B8
		// (set) Token: 0x060011FB RID: 4603 RVA: 0x000134C0 File Offset: 0x000116C0
		public bool? ItemIsReference { get; set; }

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x000134C9 File Offset: 0x000116C9
		// (set) Token: 0x060011FD RID: 4605 RVA: 0x000134D1 File Offset: 0x000116D1
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x000134DA File Offset: 0x000116DA
		// (set) Token: 0x060011FF RID: 4607 RVA: 0x000134E2 File Offset: 0x000116E2
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x06001200 RID: 4608 RVA: 0x000618DC File Offset: 0x0005FADC
		[NullableContext(1)]
		internal void WritePropertyName(JsonWriter writer)
		{
			string propertyName = this.PropertyName;
			if (this._skipPropertyNameEscape)
			{
				writer.WritePropertyName(propertyName, false);
				return;
			}
			writer.WritePropertyName(propertyName);
		}

		// Token: 0x04000AB5 RID: 2741
		internal Required? _required;

		// Token: 0x04000AB6 RID: 2742
		internal bool _hasExplicitDefaultValue;

		// Token: 0x04000AB7 RID: 2743
		private object _defaultValue;

		// Token: 0x04000AB8 RID: 2744
		private bool _hasGeneratedDefaultValue;

		// Token: 0x04000AB9 RID: 2745
		private string _propertyName;

		// Token: 0x04000ABA RID: 2746
		internal bool _skipPropertyNameEscape;

		// Token: 0x04000ABB RID: 2747
		private Type _propertyType;
	}
}
