using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000303 RID: 771
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlDocumentWrapper : XmlNodeWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x06001817 RID: 6167 RVA: 0x00017744 File Offset: 0x00015944
		public XmlDocumentWrapper(XmlDocument document) : base(document)
		{
			this._document = document;
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x00017754 File Offset: 0x00015954
		public IXmlNode CreateComment([Nullable(2)] string data)
		{
			return new XmlNodeWrapper(this._document.CreateComment(data));
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x00017767 File Offset: 0x00015967
		public IXmlNode CreateTextNode([Nullable(2)] string text)
		{
			return new XmlNodeWrapper(this._document.CreateTextNode(text));
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0001777A File Offset: 0x0001597A
		public IXmlNode CreateCDataSection([Nullable(2)] string data)
		{
			return new XmlNodeWrapper(this._document.CreateCDataSection(data));
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x0001778D File Offset: 0x0001598D
		public IXmlNode CreateWhitespace([Nullable(2)] string text)
		{
			return new XmlNodeWrapper(this._document.CreateWhitespace(text));
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x000177A0 File Offset: 0x000159A0
		public IXmlNode CreateSignificantWhitespace([Nullable(2)] string text)
		{
			return new XmlNodeWrapper(this._document.CreateSignificantWhitespace(text));
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x000177B3 File Offset: 0x000159B3
		[NullableContext(2)]
		[return: Nullable(1)]
		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new XmlDeclarationWrapper(this._document.CreateXmlDeclaration(version, encoding, standalone));
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x000177C8 File Offset: 0x000159C8
		[NullableContext(2)]
		[return: Nullable(1)]
		public IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new XmlDocumentTypeWrapper(this._document.CreateDocumentType(name, publicId, systemId, null));
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x000177DE File Offset: 0x000159DE
		public IXmlNode CreateProcessingInstruction(string target, [Nullable(2)] string data)
		{
			return new XmlNodeWrapper(this._document.CreateProcessingInstruction(target, data));
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x000177F2 File Offset: 0x000159F2
		public IXmlElement CreateElement(string elementName)
		{
			return new XmlElementWrapper(this._document.CreateElement(elementName));
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x00017805 File Offset: 0x00015A05
		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XmlElementWrapper(this._document.CreateElement(qualifiedName, namespaceUri));
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x00017819 File Offset: 0x00015A19
		public IXmlNode CreateAttribute(string name, [Nullable(2)] string value)
		{
			return new XmlNodeWrapper(this._document.CreateAttribute(name))
			{
				Value = value
			};
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x00017833 File Offset: 0x00015A33
		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, [Nullable(2)] string value)
		{
			return new XmlNodeWrapper(this._document.CreateAttribute(qualifiedName, namespaceUri))
			{
				Value = value
			};
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001824 RID: 6180 RVA: 0x0001784E File Offset: 0x00015A4E
		[Nullable(2)]
		public IXmlElement DocumentElement
		{
			[NullableContext(2)]
			get
			{
				if (this._document.DocumentElement == null)
				{
					return null;
				}
				return new XmlElementWrapper(this._document.DocumentElement);
			}
		}

		// Token: 0x04000CF1 RID: 3313
		private readonly XmlDocument _document;
	}
}
