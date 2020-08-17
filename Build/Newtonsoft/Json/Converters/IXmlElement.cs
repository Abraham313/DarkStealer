using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200030B RID: 779
	[NullableContext(1)]
	internal interface IXmlElement : IXmlNode
	{
		// Token: 0x06001859 RID: 6233
		void SetAttributeNode(IXmlNode attribute);

		// Token: 0x0600185A RID: 6234
		string GetPrefixOfNamespace(string namespaceUri);

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x0600185B RID: 6235
		bool IsEmpty { get; }
	}
}
