using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000304 RID: 772
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x06001825 RID: 6181 RVA: 0x0001786F File Offset: 0x00015A6F
		public XmlElementWrapper(XmlElement element) : base(element)
		{
			this._element = element;
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x00074D74 File Offset: 0x00072F74
		public void SetAttributeNode(IXmlNode attribute)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)attribute;
			this._element.SetAttributeNode((XmlAttribute)xmlNodeWrapper.WrappedNode);
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0001787F File Offset: 0x00015A7F
		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this._element.GetPrefixOfNamespace(namespaceUri);
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001828 RID: 6184 RVA: 0x0001788D File Offset: 0x00015A8D
		public bool IsEmpty
		{
			get
			{
				return this._element.IsEmpty;
			}
		}

		// Token: 0x04000CF2 RID: 3314
		private readonly XmlElement _element;
	}
}
