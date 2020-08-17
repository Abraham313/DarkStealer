using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000308 RID: 776
	[NullableContext(1)]
	internal interface IXmlDocument : IXmlNode
	{
		// Token: 0x06001843 RID: 6211
		IXmlNode CreateComment([Nullable(2)] string text);

		// Token: 0x06001844 RID: 6212
		IXmlNode CreateTextNode([Nullable(2)] string text);

		// Token: 0x06001845 RID: 6213
		IXmlNode CreateCDataSection([Nullable(2)] string data);

		// Token: 0x06001846 RID: 6214
		IXmlNode CreateWhitespace([Nullable(2)] string text);

		// Token: 0x06001847 RID: 6215
		IXmlNode CreateSignificantWhitespace([Nullable(2)] string text);

		// Token: 0x06001848 RID: 6216
		[NullableContext(2)]
		[return: Nullable(1)]
		IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone);

		// Token: 0x06001849 RID: 6217
		[NullableContext(2)]
		[return: Nullable(1)]
		IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset);

		// Token: 0x0600184A RID: 6218
		IXmlNode CreateProcessingInstruction(string target, [Nullable(2)] string data);

		// Token: 0x0600184B RID: 6219
		IXmlElement CreateElement(string elementName);

		// Token: 0x0600184C RID: 6220
		IXmlElement CreateElement(string qualifiedName, string namespaceUri);

		// Token: 0x0600184D RID: 6221
		IXmlNode CreateAttribute(string name, [Nullable(2)] string value);

		// Token: 0x0600184E RID: 6222
		IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, [Nullable(2)] string value);

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x0600184F RID: 6223
		[Nullable(2)]
		IXmlElement DocumentElement { [NullableContext(2)] get; }
	}
}
