using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200030C RID: 780
	[NullableContext(2)]
	internal interface IXmlNode
	{
		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x0600185C RID: 6236
		XmlNodeType NodeType { get; }

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600185D RID: 6237
		string LocalName { get; }

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600185E RID: 6238
		[Nullable(1)]
		List<IXmlNode> ChildNodes { [NullableContext(1)] get; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600185F RID: 6239
		[Nullable(1)]
		List<IXmlNode> Attributes { [NullableContext(1)] get; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001860 RID: 6240
		IXmlNode ParentNode { get; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001861 RID: 6241
		// (set) Token: 0x06001862 RID: 6242
		string Value { get; set; }

		// Token: 0x06001863 RID: 6243
		[NullableContext(1)]
		IXmlNode AppendChild(IXmlNode newChild);

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001864 RID: 6244
		string NamespaceUri { get; }

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001865 RID: 6245
		object WrappedNode { get; }
	}
}
