using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000306 RID: 774
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlDocumentTypeWrapper : XmlNodeWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x0600182F RID: 6191 RVA: 0x000178ED File Offset: 0x00015AED
		public XmlDocumentTypeWrapper(XmlDocumentType documentType) : base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001830 RID: 6192 RVA: 0x000178FD File Offset: 0x00015AFD
		public string Name
		{
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001831 RID: 6193 RVA: 0x0001790A File Offset: 0x00015B0A
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001832 RID: 6194 RVA: 0x00017917 File Offset: 0x00015B17
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001833 RID: 6195 RVA: 0x00017924 File Offset: 0x00015B24
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001834 RID: 6196 RVA: 0x00017931 File Offset: 0x00015B31
		[Nullable(2)]
		public override string LocalName
		{
			[NullableContext(2)]
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x04000CF4 RID: 3316
		private readonly XmlDocumentType _documentType;
	}
}
