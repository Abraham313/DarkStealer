using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000307 RID: 775
	[NullableContext(2)]
	[Nullable(0)]
	internal class XmlNodeWrapper : IXmlNode
	{
		// Token: 0x06001835 RID: 6197 RVA: 0x00017938 File Offset: 0x00015B38
		[NullableContext(1)]
		public XmlNodeWrapper(XmlNode node)
		{
			this._node = node;
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001836 RID: 6198 RVA: 0x00017947 File Offset: 0x00015B47
		public object WrappedNode
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001837 RID: 6199 RVA: 0x0001794F File Offset: 0x00015B4F
		public XmlNodeType NodeType
		{
			get
			{
				return this._node.NodeType;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001838 RID: 6200 RVA: 0x0001795C File Offset: 0x00015B5C
		public virtual string LocalName
		{
			get
			{
				return this._node.LocalName;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001839 RID: 6201 RVA: 0x00074DA0 File Offset: 0x00072FA0
		[Nullable(1)]
		public List<IXmlNode> ChildNodes
		{
			[NullableContext(1)]
			get
			{
				if (this._childNodes == null)
				{
					if (!this._node.HasChildNodes)
					{
						this._childNodes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._childNodes = new List<IXmlNode>(this._node.ChildNodes.Count);
						foreach (object obj in this._node.ChildNodes)
						{
							XmlNode node = (XmlNode)obj;
							this._childNodes.Add(XmlNodeWrapper.WrapNode(node));
						}
					}
				}
				return this._childNodes;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x0600183A RID: 6202 RVA: 0x00017969 File Offset: 0x00015B69
		protected virtual bool HasChildNodes
		{
			get
			{
				return this._node.HasChildNodes;
			}
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00074E50 File Offset: 0x00073050
		[NullableContext(1)]
		internal static IXmlNode WrapNode(XmlNode node)
		{
			XmlNodeType nodeType = node.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				return new XmlElementWrapper((XmlElement)node);
			}
			if (nodeType == XmlNodeType.DocumentType)
			{
				return new XmlDocumentTypeWrapper((XmlDocumentType)node);
			}
			if (nodeType != XmlNodeType.XmlDeclaration)
			{
				return new XmlNodeWrapper(node);
			}
			return new XmlDeclarationWrapper((XmlDeclaration)node);
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x0600183C RID: 6204 RVA: 0x00074E9C File Offset: 0x0007309C
		[Nullable(1)]
		public List<IXmlNode> Attributes
		{
			[NullableContext(1)]
			get
			{
				if (this._attributes == null)
				{
					if (!this.HasAttributes)
					{
						this._attributes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._attributes = new List<IXmlNode>(this._node.Attributes.Count);
						foreach (object obj in this._node.Attributes)
						{
							XmlAttribute node = (XmlAttribute)obj;
							this._attributes.Add(XmlNodeWrapper.WrapNode(node));
						}
					}
				}
				return this._attributes;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x0600183D RID: 6205 RVA: 0x00074F44 File Offset: 0x00073144
		private bool HasAttributes
		{
			get
			{
				XmlElement xmlElement = this._node as XmlElement;
				if (xmlElement != null)
				{
					return xmlElement.HasAttributes;
				}
				XmlAttributeCollection attributes = this._node.Attributes;
				return attributes != null && attributes.Count > 0;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600183E RID: 6206 RVA: 0x00074F80 File Offset: 0x00073180
		public IXmlNode ParentNode
		{
			get
			{
				XmlAttribute xmlAttribute = this._node as XmlAttribute;
				XmlNode xmlNode = (xmlAttribute != null) ? xmlAttribute.OwnerElement : this._node.ParentNode;
				if (xmlNode == null)
				{
					return null;
				}
				return XmlNodeWrapper.WrapNode(xmlNode);
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x0600183F RID: 6207 RVA: 0x00017976 File Offset: 0x00015B76
		// (set) Token: 0x06001840 RID: 6208 RVA: 0x00017983 File Offset: 0x00015B83
		public string Value
		{
			get
			{
				return this._node.Value;
			}
			set
			{
				this._node.Value = value;
			}
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00074FBC File Offset: 0x000731BC
		[NullableContext(1)]
		public IXmlNode AppendChild(IXmlNode newChild)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)newChild;
			this._node.AppendChild(xmlNodeWrapper._node);
			this._childNodes = null;
			this._attributes = null;
			return newChild;
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001842 RID: 6210 RVA: 0x00017991 File Offset: 0x00015B91
		public string NamespaceUri
		{
			get
			{
				return this._node.NamespaceURI;
			}
		}

		// Token: 0x04000CF5 RID: 3317
		[Nullable(1)]
		private readonly XmlNode _node;

		// Token: 0x04000CF6 RID: 3318
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private List<IXmlNode> _childNodes;

		// Token: 0x04000CF7 RID: 3319
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private List<IXmlNode> _attributes;
	}
}
