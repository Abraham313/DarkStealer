using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000C1 RID: 193
	[ComVisible(true)]
	public enum ZipProgressEventType
	{
		// Token: 0x04000259 RID: 601
		Adding_Started,
		// Token: 0x0400025A RID: 602
		Adding_AfterAddEntry,
		// Token: 0x0400025B RID: 603
		Adding_Completed,
		// Token: 0x0400025C RID: 604
		Reading_Started,
		// Token: 0x0400025D RID: 605
		Reading_BeforeReadEntry,
		// Token: 0x0400025E RID: 606
		Reading_AfterReadEntry,
		// Token: 0x0400025F RID: 607
		Reading_Completed,
		// Token: 0x04000260 RID: 608
		Reading_ArchiveBytesRead,
		// Token: 0x04000261 RID: 609
		Saving_Started,
		// Token: 0x04000262 RID: 610
		Saving_BeforeWriteEntry,
		// Token: 0x04000263 RID: 611
		Saving_AfterWriteEntry,
		// Token: 0x04000264 RID: 612
		Saving_Completed,
		// Token: 0x04000265 RID: 613
		Saving_AfterSaveTempArchive,
		// Token: 0x04000266 RID: 614
		Saving_BeforeRenameTempArchive,
		// Token: 0x04000267 RID: 615
		Saving_AfterRenameTempArchive,
		// Token: 0x04000268 RID: 616
		Saving_AfterCompileSelfExtractor,
		// Token: 0x04000269 RID: 617
		Saving_EntryBytesRead,
		// Token: 0x0400026A RID: 618
		Extracting_BeforeExtractEntry,
		// Token: 0x0400026B RID: 619
		Extracting_AfterExtractEntry,
		// Token: 0x0400026C RID: 620
		Extracting_ExtractEntryWouldOverwrite,
		// Token: 0x0400026D RID: 621
		Extracting_EntryBytesWritten,
		// Token: 0x0400026E RID: 622
		Extracting_BeforeExtractAll,
		// Token: 0x0400026F RID: 623
		Extracting_AfterExtractAll,
		// Token: 0x04000270 RID: 624
		Error_Saving
	}
}
