using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000313 RID: 787
	[NullableContext(1)]
	[Nullable(0)]
	internal class XContainerWrapper : XObjectWrapper
	{
		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001895 RID: 6293 RVA: 0x00017C0D File Offset: 0x00015E0D
		private XContainer Container
		{
			get
			{
				return (XContainer)base.WrappedNode;
			}
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x00017B4A File Offset: 0x00015D4A
		public XContainerWrapper(XContainer container) : base(container)
		{
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001897 RID: 6295 RVA: 0x0007507C File Offset: 0x0007327C
		public override List<IXmlNode> ChildNodes
		{
			get
			{
				if (this._childNodes == null)
				{
					if (!this.HasChildNodes)
					{
						this._childNodes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._childNodes = new List<IXmlNode>();
						foreach (XNode node in this.Container.Nodes())
						{
							this._childNodes.Add(XContainerWrapper.WrapNode(node));
						}
					}
				}
				return this._childNodes;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001898 RID: 6296 RVA: 0x00017C1A File Offset: 0x00015E1A
		protected virtual bool HasChildNodes
		{
			get
			{
				return this.Container.LastNode != null;
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001899 RID: 6297 RVA: 0x00017C2A File Offset: 0x00015E2A
		[Nullable(2)]
		public override IXmlNode ParentNode
		{
			[NullableContext(2)]
			get
			{
				if (this.Container.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Container.Parent);
			}
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x00075108 File Offset: 0x00073308
		internal static IXmlNode WrapNode(XObject node)
		{
			XDocument xdocument = node as XDocument;
			if (xdocument != null)
			{
				return new XDocumentWrapper(xdocument);
			}
			XElement xelement = node as XElement;
			if (xelement != null)
			{
				return new XElementWrapper(xelement);
			}
			XContainer xcontainer = node as XContainer;
			if (xcontainer != null)
			{
				return new XContainerWrapper(xcontainer);
			}
			XProcessingInstruction xprocessingInstruction = node as XProcessingInstruction;
			if (xprocessingInstruction != null)
			{
				return new XProcessingInstructionWrapper(xprocessingInstruction);
			}
			XText xtext = node as XText;
			if (xtext != null)
			{
				return new XTextWrapper(xtext);
			}
			XComment xcomment = node as XComment;
			if (xcomment != null)
			{
				return new XCommentWrapper(xcomment);
			}
			XAttribute xattribute = node as XAttribute;
			if (xattribute != null)
			{
				return new XAttributeWrapper(xattribute);
			}
			XDocumentType xdocumentType = node as XDocumentType;
			if (xdocumentType != null)
			{
				return new XDocumentTypeWrapper(xdocumentType);
			}
			return new XObjectWrapper(node);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x00017C4B File Offset: 0x00015E4B
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			this.Container.Add(newChild.WrappedNode);
			this._childNodes = null;
			return newChild;
		}

		// Token: 0x04000CFA RID: 3322
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private List<IXmlNode> _childNodes;
	}
}
