using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200030E RID: 782
	[NullableContext(1)]
	[Nullable(0)]
	internal class XDocumentTypeWrapper : XObjectWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x0600186E RID: 6254 RVA: 0x000179FD File Offset: 0x00015BFD
		public XDocumentTypeWrapper(XDocumentType documentType) : base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600186F RID: 6255 RVA: 0x00017A0D File Offset: 0x00015C0D
		public string Name
		{
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001870 RID: 6256 RVA: 0x00017A1A File Offset: 0x00015C1A
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001871 RID: 6257 RVA: 0x00017A27 File Offset: 0x00015C27
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001872 RID: 6258 RVA: 0x00017A34 File Offset: 0x00015C34
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001873 RID: 6259 RVA: 0x00017931 File Offset: 0x00015B31
		[Nullable(2)]
		public override string LocalName
		{
			[NullableContext(2)]
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x04000CF9 RID: 3321
		private readonly XDocumentType _documentType;
	}
}
