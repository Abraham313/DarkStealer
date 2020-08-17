using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000289 RID: 649
	[NullableContext(1)]
	[Nullable(0)]
	public class ReflectionAttributeProvider : IAttributeProvider
	{
		// Token: 0x060012ED RID: 4845 RVA: 0x00013D40 File Offset: 0x00011F40
		public ReflectionAttributeProvider(object attributeProvider)
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			this._attributeProvider = attributeProvider;
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00013D5A File Offset: 0x00011F5A
		public IList<Attribute> GetAttributes(bool inherit)
		{
			return ReflectionUtils.GetAttributes(this._attributeProvider, null, inherit);
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x00013D69 File Offset: 0x00011F69
		public IList<Attribute> GetAttributes(Type attributeType, bool inherit)
		{
			return ReflectionUtils.GetAttributes(this._attributeProvider, attributeType, inherit);
		}

		// Token: 0x04000B09 RID: 2825
		private readonly object _attributeProvider;
	}
}
