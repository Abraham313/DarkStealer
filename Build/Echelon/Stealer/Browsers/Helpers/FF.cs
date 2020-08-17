using System;

namespace Echelon.Stealer.Browsers.Helpers
{
	// Token: 0x02000032 RID: 50
	public struct FF
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00008CAF File Offset: 0x00006EAF
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00008CB7 File Offset: 0x00006EB7
		public long ID { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00008CC0 File Offset: 0x00006EC0
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00008CC8 File Offset: 0x00006EC8
		public string Type { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00008CD1 File Offset: 0x00006ED1
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00008CD9 File Offset: 0x00006ED9
		public string Name { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00008CE2 File Offset: 0x00006EE2
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00008CEA File Offset: 0x00006EEA
		public string AstableName { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00008CF3 File Offset: 0x00006EF3
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00008CFB File Offset: 0x00006EFB
		public long RootNum { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00008D04 File Offset: 0x00006F04
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00008D0C File Offset: 0x00006F0C
		public string SqlStatement { get; set; }
	}
}
