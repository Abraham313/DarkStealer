using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200030F RID: 783
	[NullableContext(1)]
	[Nullable(0)]
	internal class XDocumentWrapper : XContainerWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001874 RID: 6260 RVA: 0x00017A41 File Offset: 0x00015C41
		private XDocument Document
		{
			get
			{
				return (XDocument)base.WrappedNode;
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x00017A4E File Offset: 0x00015C4E
		public XDocumentWrapper(XDocument document) : base(document)
		{
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001876 RID: 6262 RVA: 0x00074FF4 File Offset: 0x000731F4
		public override List<IXmlNode> ChildNodes
		{
			get
			{
				List<IXmlNode> childNodes = base.ChildNodes;
				if (this.Document.Declaration != null && (childNodes.Count == 0 || childNodes[0].NodeType != XmlNodeType.XmlDeclaration))
				{
					childNodes.Insert(0, new XDeclarationWrapper(this.Document.Declaration));
				}
				return childNodes;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001877 RID: 6263 RVA: 0x00017A57 File Offset: 0x00015C57
		protected override bool HasChildNodes
		{
			get
			{
				return base.HasChildNodes || this.Document.Declaration != null;
			}
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x00017A71 File Offset: 0x00015C71
		public IXmlNode CreateComment([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XComment(text));
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00017A7E File Offset: 0x00015C7E
		public IXmlNode CreateTextNode([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00017A8B File Offset: 0x00015C8B
		public IXmlNode CreateCDataSection([Nullable(2)] string data)
		{
			return new XObjectWrapper(new XCData(data));
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00017A7E File Offset: 0x00015C7E
		public IXmlNode CreateWhitespace([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00017A7E File Offset: 0x00015C7E
		public IXmlNode CreateSignificantWhitespace([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00017A98 File Offset: 0x00015C98
		[NullableContext(2)]
		[return: Nullable(1)]
		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new XDeclarationWrapper(new XDeclaration(version, encoding, standalone));
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x00017AA7 File Offset: 0x00015CA7
		[NullableContext(2)]
		[return: Nullable(1)]
		public IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new XDocumentTypeWrapper(new XDocumentType(name, publicId, systemId, internalSubset));
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x00017AB8 File Offset: 0x00015CB8
		public IXmlNode CreateProcessingInstruction(string target, [Nullable(2)] string data)
		{
			return new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00017AC6 File Offset: 0x00015CC6
		public IXmlElement CreateElement(string elementName)
		{
			return new XElementWrapper(new XElement(elementName));
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00017AD8 File Offset: 0x00015CD8
		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XElementWrapper(new XElement(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri)));
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x00017AF0 File Offset: 0x00015CF0
		public IXmlNode CreateAttribute(string name, [Nullable(2)] string value)
		{
			return new XAttributeWrapper(new XAttribute(name, value));
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00017B03 File Offset: 0x00015D03
		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, [Nullable(2)] string value)
		{
			return new XAttributeWrapper(new XAttribute(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri), value));
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001884 RID: 6276 RVA: 0x00017B1C File Offset: 0x00015D1C
		[Nullable(2)]
		public IXmlElement DocumentElement
		{
			[NullableContext(2)]
			get
			{
				if (this.Document.Root == null)
				{
					return null;
				}
				return new XElementWrapper(this.Document.Root);
			}
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00075048 File Offset: 0x00073248
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			XDeclarationWrapper xdeclarationWrapper = newChild as XDeclarationWrapper;
			if (xdeclarationWrapper != null)
			{
				this.Document.Declaration = xdeclarationWrapper.Declaration;
				return xdeclarationWrapper;
			}
			return base.AppendChild(newChild);
		}
	}
}
