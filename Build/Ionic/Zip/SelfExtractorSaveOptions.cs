using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000F9 RID: 249
	[ComVisible(true)]
	public class SelfExtractorSaveOptions
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x0000B859 File Offset: 0x00009A59
		// (set) Token: 0x060005FA RID: 1530 RVA: 0x0000B861 File Offset: 0x00009A61
		public SelfExtractorFlavor Flavor { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x0000B86A File Offset: 0x00009A6A
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x0000B872 File Offset: 0x00009A72
		public string PostExtractCommandLine { get; set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x0000B87B File Offset: 0x00009A7B
		// (set) Token: 0x060005FE RID: 1534 RVA: 0x0000B883 File Offset: 0x00009A83
		public string DefaultExtractDirectory { get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x0000B88C File Offset: 0x00009A8C
		// (set) Token: 0x06000600 RID: 1536 RVA: 0x0000B894 File Offset: 0x00009A94
		public string IconFile { get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x0000B89D File Offset: 0x00009A9D
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x0000B8A5 File Offset: 0x00009AA5
		public bool Quiet { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x0000B8AE File Offset: 0x00009AAE
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x0000B8B6 File Offset: 0x00009AB6
		public ExtractExistingFileAction ExtractExistingFile { get; set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x0000B8BF File Offset: 0x00009ABF
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x0000B8C7 File Offset: 0x00009AC7
		public bool RemoveUnpackedFilesAfterExecute { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x0000B8D0 File Offset: 0x00009AD0
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x0000B8D8 File Offset: 0x00009AD8
		public Version FileVersion { get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0000B8E1 File Offset: 0x00009AE1
		// (set) Token: 0x0600060A RID: 1546 RVA: 0x0000B8E9 File Offset: 0x00009AE9
		public string ProductVersion { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0000B8F2 File Offset: 0x00009AF2
		// (set) Token: 0x0600060C RID: 1548 RVA: 0x0000B8FA File Offset: 0x00009AFA
		public string Copyright { get; set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x0000B903 File Offset: 0x00009B03
		// (set) Token: 0x0600060E RID: 1550 RVA: 0x0000B90B File Offset: 0x00009B0B
		public string Description { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x0000B914 File Offset: 0x00009B14
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x0000B91C File Offset: 0x00009B1C
		public string ProductName { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x0000B925 File Offset: 0x00009B25
		// (set) Token: 0x06000612 RID: 1554 RVA: 0x0000B92D File Offset: 0x00009B2D
		public string SfxExeWindowTitle { get; set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x0000B936 File Offset: 0x00009B36
		// (set) Token: 0x06000614 RID: 1556 RVA: 0x0000B93E File Offset: 0x00009B3E
		public string AdditionalCompilerSwitches { get; set; }
	}
}
