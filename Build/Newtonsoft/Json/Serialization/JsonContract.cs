using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200026B RID: 619
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonContract
	{
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x00012D6B File Offset: 0x00010F6B
		public Type UnderlyingType { get; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x00012D73 File Offset: 0x00010F73
		// (set) Token: 0x06001159 RID: 4441 RVA: 0x00060F78 File Offset: 0x0005F178
		public Type CreatedType
		{
			get
			{
				return this._createdType;
			}
			set
			{
				ValidationUtils.ArgumentNotNull(value, "value");
				this._createdType = value;
				this.IsSealed = this._createdType.IsSealed();
				this.IsInstantiable = (!this._createdType.IsInterface() && !this._createdType.IsAbstract());
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x00012D7B File Offset: 0x00010F7B
		// (set) Token: 0x0600115B RID: 4443 RVA: 0x00012D83 File Offset: 0x00010F83
		public bool? IsReference { get; set; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x00012D8C File Offset: 0x00010F8C
		// (set) Token: 0x0600115D RID: 4445 RVA: 0x00012D94 File Offset: 0x00010F94
		[Nullable(2)]
		public JsonConverter Converter { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x00012D9D File Offset: 0x00010F9D
		// (set) Token: 0x0600115F RID: 4447 RVA: 0x00012DA5 File Offset: 0x00010FA5
		[Nullable(2)]
		public JsonConverter InternalConverter { [NullableContext(2)] get; [NullableContext(2)] internal set; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x00012DAE File Offset: 0x00010FAE
		public IList<SerializationCallback> OnDeserializedCallbacks
		{
			get
			{
				if (this._onDeserializedCallbacks == null)
				{
					this._onDeserializedCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializedCallbacks;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06001161 RID: 4449 RVA: 0x00012DC9 File Offset: 0x00010FC9
		public IList<SerializationCallback> OnDeserializingCallbacks
		{
			get
			{
				if (this._onDeserializingCallbacks == null)
				{
					this._onDeserializingCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializingCallbacks;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x00012DE4 File Offset: 0x00010FE4
		public IList<SerializationCallback> OnSerializedCallbacks
		{
			get
			{
				if (this._onSerializedCallbacks == null)
				{
					this._onSerializedCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializedCallbacks;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001163 RID: 4451 RVA: 0x00012DFF File Offset: 0x00010FFF
		public IList<SerializationCallback> OnSerializingCallbacks
		{
			get
			{
				if (this._onSerializingCallbacks == null)
				{
					this._onSerializingCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializingCallbacks;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x00012E1A File Offset: 0x0001101A
		public IList<SerializationErrorCallback> OnErrorCallbacks
		{
			get
			{
				if (this._onErrorCallbacks == null)
				{
					this._onErrorCallbacks = new List<SerializationErrorCallback>();
				}
				return this._onErrorCallbacks;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x00012E35 File Offset: 0x00011035
		// (set) Token: 0x06001166 RID: 4454 RVA: 0x00012E3D File Offset: 0x0001103D
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public Func<object> DefaultCreator { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001167 RID: 4455 RVA: 0x00012E46 File Offset: 0x00011046
		// (set) Token: 0x06001168 RID: 4456 RVA: 0x00012E4E File Offset: 0x0001104E
		public bool DefaultCreatorNonPublic { get; set; }

		// Token: 0x06001169 RID: 4457 RVA: 0x00060FCC File Offset: 0x0005F1CC
		internal JsonContract(Type underlyingType)
		{
			ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			this.UnderlyingType = underlyingType;
			underlyingType = ReflectionUtils.EnsureNotByRefType(underlyingType);
			this.IsNullable = ReflectionUtils.IsNullable(underlyingType);
			this.NonNullableUnderlyingType = ((!this.IsNullable || !ReflectionUtils.IsNullableType(underlyingType)) ? underlyingType : Nullable.GetUnderlyingType(underlyingType));
			this._createdType = (this.CreatedType = this.NonNullableUnderlyingType);
			this.IsConvertable = ConvertUtils.IsConvertible(this.NonNullableUnderlyingType);
			this.IsEnum = this.NonNullableUnderlyingType.IsEnum();
			this.InternalReadType = ReadType.Read;
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00061064 File Offset: 0x0005F264
		internal void InvokeOnSerializing(object o, StreamingContext context)
		{
			if (this._onSerializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x000610C0 File Offset: 0x0005F2C0
		internal void InvokeOnSerialized(object o, StreamingContext context)
		{
			if (this._onSerializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0006111C File Offset: 0x0005F31C
		internal void InvokeOnDeserializing(object o, StreamingContext context)
		{
			if (this._onDeserializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00061178 File Offset: 0x0005F378
		internal void InvokeOnDeserialized(object o, StreamingContext context)
		{
			if (this._onDeserializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x000611D4 File Offset: 0x0005F3D4
		internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
		{
			if (this._onErrorCallbacks != null)
			{
				foreach (SerializationErrorCallback serializationErrorCallback in this._onErrorCallbacks)
				{
					serializationErrorCallback(o, context, errorContext);
				}
			}
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00012E57 File Offset: 0x00011057
		internal static SerializationCallback CreateSerializationCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context)
			{
				callbackMethodInfo.Invoke(o, new object[]
				{
					context
				});
			};
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00012E70 File Offset: 0x00011070
		internal static SerializationErrorCallback CreateSerializationErrorCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context, ErrorContext econtext)
			{
				callbackMethodInfo.Invoke(o, new object[]
				{
					context,
					econtext
				});
			};
		}

		// Token: 0x04000A79 RID: 2681
		internal bool IsNullable;

		// Token: 0x04000A7A RID: 2682
		internal bool IsConvertable;

		// Token: 0x04000A7B RID: 2683
		internal bool IsEnum;

		// Token: 0x04000A7C RID: 2684
		internal Type NonNullableUnderlyingType;

		// Token: 0x04000A7D RID: 2685
		internal ReadType InternalReadType;

		// Token: 0x04000A7E RID: 2686
		internal JsonContractType ContractType;

		// Token: 0x04000A7F RID: 2687
		internal bool IsReadOnlyOrFixedSize;

		// Token: 0x04000A80 RID: 2688
		internal bool IsSealed;

		// Token: 0x04000A81 RID: 2689
		internal bool IsInstantiable;

		// Token: 0x04000A82 RID: 2690
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private List<SerializationCallback> _onDeserializedCallbacks;

		// Token: 0x04000A83 RID: 2691
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private List<SerializationCallback> _onDeserializingCallbacks;

		// Token: 0x04000A84 RID: 2692
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private List<SerializationCallback> _onSerializedCallbacks;

		// Token: 0x04000A85 RID: 2693
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private List<SerializationCallback> _onSerializingCallbacks;

		// Token: 0x04000A86 RID: 2694
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private List<SerializationErrorCallback> _onErrorCallbacks;

		// Token: 0x04000A87 RID: 2695
		private Type _createdType;
	}
}
