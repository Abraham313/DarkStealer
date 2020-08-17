using System;

namespace Ionic.Zip
{
	// Token: 0x020000E2 RID: 226
	internal static class ZipConstants
	{
		// Token: 0x040002D5 RID: 725
		public const uint PackedToRemovableMedia = 808471376U;

		// Token: 0x040002D6 RID: 726
		public const uint Zip64EndOfCentralDirectoryRecordSignature = 101075792U;

		// Token: 0x040002D7 RID: 727
		public const uint Zip64EndOfCentralDirectoryLocatorSignature = 117853008U;

		// Token: 0x040002D8 RID: 728
		public const uint EndOfCentralDirectorySignature = 101010256U;

		// Token: 0x040002D9 RID: 729
		public const int ZipEntrySignature = 67324752;

		// Token: 0x040002DA RID: 730
		public const int ZipEntryDataDescriptorSignature = 134695760;

		// Token: 0x040002DB RID: 731
		public const int SplitArchiveSignature = 134695760;

		// Token: 0x040002DC RID: 732
		public const int ZipDirEntrySignature = 33639248;

		// Token: 0x040002DD RID: 733
		public const int AesKeySize = 192;

		// Token: 0x040002DE RID: 734
		public const int AesBlockSize = 128;

		// Token: 0x040002DF RID: 735
		public const ushort AesAlgId128 = 26126;

		// Token: 0x040002E0 RID: 736
		public const ushort AesAlgId192 = 26127;

		// Token: 0x040002E1 RID: 737
		public const ushort AesAlgId256 = 26128;
	}
}
