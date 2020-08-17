using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000315 RID: 789
	[NullableContext(2)]
	[Nullable(0)]
	internal class XAttributeWrapper : XObjectWrapper
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x00017C97 File Offset: 0x00015E97
		[Nullable(1)]
		private XAttribute Attribute
		{
			[NullableContext(1)]
			get
			{
				return (XAttribute)base.WrappedNode;
			}
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x00017B4A File Offset: 0x00015D4A
		[NullableContext(1)]
		public XAttributeWrapper(XAttribute attribute) : base(attribute)
		{
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x00017CA4 File Offset: 0x00015EA4
		// (set) Token: 0x060018AA RID: 6314 RVA: 0x00017CB1 File Offset: 0x00015EB1
		public override string Value
		{
			get
			{
				return this.Attribute.Value;
			}
			set
			{
				this.Attribute.Value = value;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x00017CBF File Offset: 0x00015EBF
		public override string LocalName
		{
			get
			{
				return this.Attribute.Name.LocalName;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x00017CD1 File Offset: 0x00015ED1
		public override string NamespaceUri
		{
			get
			{
				return this.Attribute.Name.NamespaceName;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x00017CE3 File Offset: 0x00015EE3
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Attribute.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Attribute.Parent);
			}
		}
	}
}
