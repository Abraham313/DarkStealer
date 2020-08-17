using System;

namespace Newtonsoft.Json
{
	// Token: 0x020001BF RID: 447
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class JsonExtensionDataAttribute : Attribute
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x0000ECD1 File Offset: 0x0000CED1
		// (set) Token: 0x06000BA6 RID: 2982 RVA: 0x0000ECD9 File Offset: 0x0000CED9
		public bool WriteData { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0000ECE2 File Offset: 0x0000CEE2
		// (set) Token: 0x06000BA8 RID: 2984 RVA: 0x0000ECEA File Offset: 0x0000CEEA
		public bool ReadData { get; set; }

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0000ECF3 File Offset: 0x0000CEF3
		public JsonExtensionDataAttribute()
		{
			this.WriteData = true;
			this.ReadData = true;
		}
	}
}
