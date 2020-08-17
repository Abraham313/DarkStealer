using System;
using System.Runtime.InteropServices;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x020000C0 RID: 192
	// (Invoke) Token: 0x06000366 RID: 870
	[ComVisible(true)]
	public delegate CompressionLevel SetCompressionCallback(string localFileName, string fileNameInArchive);
}
