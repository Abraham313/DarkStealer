using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000311 RID: 785
	[NullableContext(2)]
	[Nullable(0)]
	internal class XCommentWrapper : XObjectWrapper
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x00017B8F File Offset: 0x00015D8F
		[Nullable(1)]
		private XComment Text
		{
			[NullableContext(1)]
			get
			{
				return (XComment)base.WrappedNode;
			}
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00017B4A File Offset: 0x00015D4A
		[NullableContext(1)]
		public XCommentWrapper(XComment text) : base(text)
		{
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600188D RID: 6285 RVA: 0x00017B9C File Offset: 0x00015D9C
		// (set) Token: 0x0600188E RID: 6286 RVA: 0x00017BA9 File Offset: 0x00015DA9
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

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600188F RID: 6287 RVA: 0x00017BB7 File Offset: 0x00015DB7
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
