using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200030A RID: 778
	[NullableContext(1)]
	internal interface IXmlDocumentType : IXmlNode
	{
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001855 RID: 6229
		string Name { get; }

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001856 RID: 6230
		string System { get; }

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001857 RID: 6231
		string Public { get; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001858 RID: 6232
		string InternalSubset { get; }
	}
}
