using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200025E RID: 606
	[NullableContext(1)]
	public interface IAttributeProvider
	{
		// Token: 0x0600111E RID: 4382
		IList<Attribute> GetAttributes(bool inherit);

		// Token: 0x0600111F RID: 4383
		IList<Attribute> GetAttributes(Type attributeType, bool inherit);
	}
}
