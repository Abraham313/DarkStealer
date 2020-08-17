using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000310 RID: 784
	[NullableContext(2)]
	[Nullable(0)]
	internal class XTextWrapper : XObjectWrapper
	{
		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001886 RID: 6278 RVA: 0x00017B3D File Offset: 0x00015D3D
		[Nullable(1)]
		private XText Text
		{
			[NullableContext(1)]
			get
			{
				return (XText)base.WrappedNode;
			}
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00017B4A File Offset: 0x00015D4A
		[NullableContext(1)]
		public XTextWrapper(XText text) : base(text)
		{
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001888 RID: 6280 RVA: 0x00017B53 File Offset: 0x00015D53
		// (set) Token: 0x06001889 RID: 6281 RVA: 0x00017B60 File Offset: 0x00015D60
		public override string Value
		{
			get
			{
				return this.Text.Value;
			}
			set
			{
				this.Text.Value = value;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x0600188A RID: 6282 RVA: 0x00017B6E File Offset: 0x00015D6E
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Text.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Text.Parent);
			}
		}
	}
}
