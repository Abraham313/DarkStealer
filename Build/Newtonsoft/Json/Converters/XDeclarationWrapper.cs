using System;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200030D RID: 781
	[NullableContext(1)]
	[Nullable(0)]
	internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001866 RID: 6246 RVA: 0x0001799E File Offset: 0x00015B9E
		internal XDeclaration Declaration { get; }

		// Token: 0x06001867 RID: 6247 RVA: 0x000179A6 File Offset: 0x00015BA6
		public XDeclarationWrapper(XDeclaration declaration) : base(null)
		{
			this.Declaration = declaration;
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001868 RID: 6248 RVA: 0x000179B6 File Offset: 0x00015BB6
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.XmlDeclaration;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001869 RID: 6249 RVA: 0x000179BA File Offset: 0x00015BBA
		public string Version
		{
			get
			{
				return this.Declaration.Version;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x0600186A RID: 6250 RVA: 0x000179C7 File Offset: 0x00015BC7
		// (set) Token: 0x0600186B RID: 6251 RVA: 0x000179D4 File Offset: 0x00015BD4
		public string Encoding
		{
			get
			{
				return this.Declaration.Encoding;
			}
			set
			{
				this.Declaration.Encoding = value;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x0600186C RID: 6252 RVA: 0x000179E2 File Offset: 0x00015BE2
		// (set) Token: 0x0600186D RID: 6253 RVA: 0x000179EF File Offset: 0x00015BEF
		public string Standalone
		{
			get
			{
				return this.Declaration.Standalone;
			}
			set
			{
				this.Declaration.Standalone = value;
			}
		}
	}
}
