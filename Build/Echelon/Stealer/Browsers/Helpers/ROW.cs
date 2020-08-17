using System;

namespace Echelon.Stealer.Browsers.Helpers
{
	// Token: 0x02000034 RID: 52
	public struct ROW
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00008DD9 File Offset: 0x00006FD9
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00008DE1 File Offset: 0x00006FE1
		public long ID { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00008DEA File Offset: 0x00006FEA
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00008DF2 File Offset: 0x00006FF2
		public string[] RowData { get; set; }
	}
}
