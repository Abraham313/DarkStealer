using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000312 RID: 786
	[NullableContext(2)]
	[Nullable(0)]
	internal class XProcessingInstructionWrapper : XObjectWrapper
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001890 RID: 6288 RVA: 0x00017BD8 File Offset: 0x00015DD8
		[Nullable(1)]
		private XProcessingInstruction ProcessingInstruction
		{
			[NullableContext(1)]
			get
			{
				return (XProcessingInstruction)base.WrappedNode;
			}
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x00017B4A File Offset: 0x00015D4A
		[NullableContext(1)]
		public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction) : base(processingInstruction)
		{
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001892 RID: 6290 RVA: 0x00017BE5 File Offset: 0x00015DE5
		public override string LocalName
		{
			get
			{
				return this.ProcessingInstruction.Target;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001893 RID: 6291 RVA: 0x00017BF2 File Offset: 0x00015DF2
		// (set) Token: 0x06001894 RID: 6292 RVA: 0x00017BFF File Offset: 0x00015DFF
		public override string Value
		{
			get
			{
				return this.ProcessingInstruction.Data;
			}
			set
			{
				this.ProcessingInstruction.Data = value;
			}
		}
	}
}
