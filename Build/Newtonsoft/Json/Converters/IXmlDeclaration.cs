using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000309 RID: 777
	[NullableContext(1)]
	internal interface IXmlDeclaration : IXmlNode
	{
		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001850 RID: 6224
		string Version { get; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001851 RID: 6225
		// (set) Token: 0x06001852 RID: 6226
		string Encoding { get; set; }

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001853 RID: 6227
		// (set) Token: 0x06001854 RID: 6228
		string Standalone { get; set; }
	}
}
