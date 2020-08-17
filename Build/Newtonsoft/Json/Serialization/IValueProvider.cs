using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000263 RID: 611
	[NullableContext(1)]
	public interface IValueProvider
	{
		// Token: 0x06001129 RID: 4393
		void SetValue(object target, [Nullable(2)] object value);

		// Token: 0x0600112A RID: 4394
		[return: Nullable(2)]
		object GetValue(object target);
	}
}
