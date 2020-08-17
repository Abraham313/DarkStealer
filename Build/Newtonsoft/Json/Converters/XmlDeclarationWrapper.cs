using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000305 RID: 773
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x06001829 RID: 6185 RVA: 0x0001789A File Offset: 0x00015A9A
		public XmlDeclarationWrapper(XmlDeclaration declaration) : base(declaration)
		{
			this._declaration = declaration;
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600182A RID: 6186 RVA: 0x000178AA File Offset: 0x00015AAA
		public string Version
		{
			get
			{
				return this._declaration.Version;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x0600182B RID: 6187 RVA: 0x000178B7 File Offset: 0x00015AB7
		// (set) Token: 0x0600182C RID: 6188 RVA: 0x000178C4 File Offset: 0x00015AC4
		public string Encoding
		{
			get
			{
				return this._declaration.Encoding;
			}
			set
			{
				this._declaration.Encoding = value;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x0600182D RID: 6189 RVA: 0x000178D2 File Offset: 0x00015AD2
		// (set) Token: 0x0600182E RID: 6190 RVA: 0x000178DF File Offset: 0x00015ADF
		public string Standalone
		{
			get
			{
				return this._declaration.Standalone;
			}
			set
			{
				this._declaration.Standalone = value;
			}
		}

		// Token: 0x04000CF3 RID: 3315
		private readonly XmlDeclaration _declaration;
	}
}
