using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x020001BB RID: 443
	[NullableContext(1)]
	[Nullable(0)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonConverterAttribute : Attribute
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x0000EC74 File Offset: 0x0000CE74
		public Type ConverterType
		{
			get
			{
				return this._converterType;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0000EC7C File Offset: 0x0000CE7C
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public object[] ConverterParameters { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; }

		// Token: 0x06000B9B RID: 2971 RVA: 0x0000EC84 File Offset: 0x0000CE84
		public JsonConverterAttribute(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this._converterType = converterType;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0000ECA7 File Offset: 0x0000CEA7
		public JsonConverterAttribute(Type converterType, params object[] converterParameters) : this(converterType)
		{
			this.ConverterParameters = converterParameters;
		}

		// Token: 0x040007BA RID: 1978
		private readonly Type _converterType;
	}
}
