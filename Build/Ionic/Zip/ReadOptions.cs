using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Ionic.Zip
{
	// Token: 0x020000F6 RID: 246
	[ComVisible(true)]
	public class ReadOptions
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0000B826 File Offset: 0x00009A26
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x0000B82E File Offset: 0x00009A2E
		public EventHandler<ReadProgressEventArgs> ReadProgress { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0000B837 File Offset: 0x00009A37
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x0000B83F File Offset: 0x00009A3F
		public TextWriter StatusMessageWriter { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0000B848 File Offset: 0x00009A48
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x0000B850 File Offset: 0x00009A50
		public Encoding Encoding { get; set; }
	}
}
