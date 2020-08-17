using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x020000CC RID: 204
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00008")]
	[ComVisible(true)]
	[Serializable]
	public class SfxGenerationException : ZipException
	{
		// Token: 0x060003AA RID: 938 RVA: 0x0000A42C File Offset: 0x0000862C
		public SfxGenerationException()
		{
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000A434 File Offset: 0x00008634
		public SfxGenerationException(string message) : base(message)
		{
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000A447 File Offset: 0x00008647
		protected SfxGenerationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
