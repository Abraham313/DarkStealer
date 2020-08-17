using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000314 RID: 788
	[NullableContext(2)]
	[Nullable(0)]
	internal class XObjectWrapper : IXmlNode
	{
		// Token: 0x0600189C RID: 6300 RVA: 0x00017C66 File Offset: 0x00015E66
		public XObjectWrapper(XObject xmlObject)
		{
			this._xmlObject = xmlObject;
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x0600189D RID: 6301 RVA: 0x00017C75 File Offset: 0x00015E75
		public object WrappedNode
		{
			get
			{
				return this._xmlObject;
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x0600189E RID: 6302 RVA: 0x00017C7D File Offset: 0x00015E7D
		public virtual XmlNodeType NodeType
		{
			get
			{
				XObject xmlObject = this._xmlObject;
				if (xmlObject == null)
				{
					return XmlNodeType.None;
				}
				return xmlObject.NodeType;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x0600189F RID: 6303 RVA: 0x000158F9 File Offset: 0x00013AF9
		public virtual string LocalName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060018A0 RID: 6304 RVA: 0x00017C90 File Offset: 0x00015E90
		[Nullable(1)]
		public virtual List<IXmlNode> ChildNodes
		{
			[NullableContext(1)]
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060018A1 RID: 6305 RVA: 0x00017C90 File Offset: 0x00015E90
		[Nullable(1)]
		public virtual List<IXmlNode> Attributes
		{
			[NullableContext(1)]
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060018A2 RID: 6306 RVA: 0x000158F9 File Offset: 0x00013AF9
		public virtual IXmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x000158F9 File Offset: 0x00013AF9
		// (set) Token: 0x060018A4 RID: 6308 RVA: 0x00009411 File Offset: 0x00007611
		public virtual string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x00009411 File Offset: 0x00007611
		[NullableContext(1)]
		public virtual IXmlNode AppendChild(IXmlNode newChild)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060018A6 RID: 6310 RVA: 0x000158F9 File Offset: 0x00013AF9
		public virtual string NamespaceUri
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000CFB RID: 3323
		private readonly XObject _xmlObject;
	}
}
