using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000170 RID: 368
	internal class AccountInformationRequest : RequestBase
	{
		// Token: 0x06000A1D RID: 2589 RVA: 0x0000DDE6 File Offset: 0x0000BFE6
		public AccountInformationRequest() : base("uq")
		{
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x00009F16 File Offset: 0x00008116
		[JsonProperty("strg")]
		public int Storage
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x00009021 File Offset: 0x00007221
		[JsonProperty("xfer")]
		public int Transfer
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x00009021 File Offset: 0x00007221
		[JsonProperty("pro")]
		public int AccountType
		{
			get
			{
				return 0;
			}
		}
	}
}
