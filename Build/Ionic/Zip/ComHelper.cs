using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000BB RID: 187
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000F")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ComHelper
	{
		// Token: 0x06000352 RID: 850 RVA: 0x0000A2B3 File Offset: 0x000084B3
		public bool IsZipFile(string filename)
		{
			return ZipFile.IsZipFile(filename);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000A2BB File Offset: 0x000084BB
		public bool IsZipFileWithExtract(string filename)
		{
			return ZipFile.IsZipFile(filename, true);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000A2C4 File Offset: 0x000084C4
		public bool CheckZip(string filename)
		{
			return ZipFile.CheckZip(filename);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000A2CC File Offset: 0x000084CC
		public bool CheckZipPassword(string filename, string password)
		{
			return ZipFile.CheckZipPassword(filename, password);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000A2D5 File Offset: 0x000084D5
		public void FixZipDirectory(string filename)
		{
			ZipFile.FixZipDirectory(filename);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000A2DD File Offset: 0x000084DD
		public string GetZipLibraryVersion()
		{
			return ZipFile.LibraryVersion.ToString();
		}
	}
}
