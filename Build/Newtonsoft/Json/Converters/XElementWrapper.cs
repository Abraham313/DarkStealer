using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000316 RID: 790
	[NullableContext(1)]
	[Nullable(0)]
	internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x00017D04 File Offset: 0x00015F04
		private XElement Element
		{
			get
			{
				return (XElement)base.WrappedNode;
			}
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x00017A4E File Offset: 0x00015C4E
		public XElementWrapper(XElement element) : base(element)
		{
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x000751B0 File Offset: 0x000733B0
		public void SetAttributeNode(IXmlNode attribute)
		{
			XObjectWrapper xobjectWrapper = (XObjectWrapper)attribute;
			this.Element.Add(xobjectWrapper.WrappedNode);
			this._attributes = null;
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x000751DC File Offset: 0x000733DC
		public override List<IXmlNode> Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					if (!this.Element.HasAttributes && !this.HasImplicitNamespaceAttribute(this.NamespaceUri))
					{
						this._attributes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._attributes = new List<IXmlNode>();
						foreach (XAttribute attribute in this.Element.Attributes())
						{
							this._attributes.Add(new XAttributeWrapper(attribute));
						}
						string namespaceUri = this.NamespaceUri;
						if (this.HasImplicitNamespaceAttribute(namespaceUri))
						{
							this._attributes.Insert(0, new XAttributeWrapper(new XAttribute("xmlns", namespaceUri)));
						}
					}
				}
				return this._attributes;
			}
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x000752B0 File Offset: 0x000734B0
		private bool HasImplicitNamespaceAttribute(string namespaceUri)
		{
			if (!StringUtils.IsNullOrEmpty(namespaceUri))
			{
				IXmlNode parentNode = this.ParentNode;
				if (namespaceUri != ((parentNode != null) ? parentNode.NamespaceUri : null) && StringUtils.IsNullOrEmpty(this.GetPrefixOfNamespace(namespaceUri)))
				{
					bool flag = false;
					if (this.Element.HasAttributes)
					{
						foreach (XAttribute xattribute in this.Element.Attributes())
						{
							if (xattribute.Name.LocalName == "xmlns" && StringUtils.IsNullOrEmpty(xattribute.Name.NamespaceName) && xattribute.Value == namespaceUri)
							{
								flag = true;
							}
						}
					}
					if (!flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x00017D11 File Offset: 0x00015F11
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			IXmlNode result = base.AppendChild(newChild);
			this._attributes = null;
			return result;
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060018B4 RID: 6324 RVA: 0x00017D21 File Offset: 0x00015F21
		// (set) Token: 0x060018B5 RID: 6325 RVA: 0x00017D2E File Offset: 0x00015F2E
		[Nullable(2)]
		public override string Value
		{
			[NullableContext(2)]
			get
			{
				return this.Element.Value;
			}
			[NullableContext(2)]
			set
			{
				this.Element.Value = value;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060018B6 RID: 6326 RVA: 0x00017D3C File Offset: 0x00015F3C
		[Nullable(2)]
		public override string LocalName
		{
			[NullableContext(2)]
			get
			{
				return this.Element.Name.LocalName;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x00017D4E File Offset: 0x00015F4E
		[Nullable(2)]
		public override string NamespaceUri
		{
			[NullableContext(2)]
			get
			{
				return this.Element.Name.NamespaceName;
			}
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x00017D60 File Offset: 0x00015F60
		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this.Element.GetPrefixOfNamespace(namespaceUri);
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x00017D73 File Offset: 0x00015F73
		public bool IsEmpty
		{
			get
			{
				return this.Element.IsEmpty;
			}
		}

		// Token: 0x04000CFC RID: 3324
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private List<IXmlNode> _attributes;
	}
}
